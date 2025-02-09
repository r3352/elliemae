// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.ConnectionManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Belikov.GenuineChannels;
using Belikov.GenuineChannels.Connection;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.Security;
using Belikov.GenuineChannels.TransportContext;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Threading;

#nullable disable
namespace Elli.Server.Remoting
{
  public class ConnectionManager
  {
    private const string className = "ConnectionManager";
    private static bool remotingInitialized = false;
    private static SecuritySessionParameters securityContext = (SecuritySessionParameters) null;
    private static IPAddressRange[] allowedIpAddresses = (IPAddressRange[]) null;
    private static Dictionary<string, EllieMae.EMLite.Server.Connection> connections = new Dictionary<string, EllieMae.EMLite.Server.Connection>();
    private static bool disableParallelTerminateConn = false;
    private static Thread connectionMonitorThread = (Thread) null;
    private static readonly TimeSpan monitorInterval = TimeSpan.FromSeconds(60.0);
    private static bool? _useXFF = new bool?();

    static ConnectionManager()
    {
      ConnectionManager.securityContext = ConnectionManager.getSecurityContext();
      ConnectionManager.allowedIpAddresses = EnConfigurationSettings.GlobalSettings.ServerIPAddressRestrictions;
      if (!EncompassServer.IsRunningInProcess)
      {
        ConnectionManager.connectionMonitorThread = new Thread(new ThreadStart(ConnectionManager.monitorConnections));
        ConnectionManager.connectionMonitorThread.IsBackground = true;
        ConnectionManager.connectionMonitorThread.Priority = ThreadPriority.AboveNormal;
        ConnectionManager.connectionMonitorThread.Start();
      }
      string appSetting = ConfigurationManager.AppSettings["DisableParallelTerminateConnections"];
      ConnectionManager.disableParallelTerminateConn = !string.IsNullOrWhiteSpace(appSetting) && string.Compare(appSetting, "true", StringComparison.OrdinalIgnoreCase) == 0;
    }

    private ConnectionManager()
    {
    }

    public static void Start()
    {
      if (ConnectionManager.remotingInitialized)
        return;
      ConnectionManager.registerChannelEvents();
      RemotingTracker.Register();
      ConnectionManager.remotingInitialized = true;
    }

    public static SecurityContextOptions SecurityContext
    {
      get
      {
        return new SecurityContextOptions(ConnectionManager.securityContext.Name, (ConnectionManager.securityContext.Attributes & SecuritySessionAttributes.EnableCompression) == SecuritySessionAttributes.EnableCompression);
      }
    }

    public static void RegisterSession(IClientSession session)
    {
      string connectionId = session.LoginParams.ConnectionID;
      if ((connectionId ?? "") == "")
        return;
      lock (ConnectionManager.connections)
      {
        if (!ConnectionManager.connections.ContainsKey(connectionId))
          ConnectionManager.connections.Add(connectionId, new EllieMae.EMLite.Server.Connection(connectionId, session.LoginParams.Hostname));
        EllieMae.EMLite.Server.Connection connection = ConnectionManager.connections[connectionId];
        connection.AddSession(session);
        TraceLog.WriteInfo(nameof (ConnectionManager), "Registered new session " + session.SessionID + " on connection " + (object) connection);
      }
    }

    public static void UnregisterSession(IClientSession session)
    {
      string connectionId = session.LoginParams.ConnectionID;
      if ((connectionId ?? "") == "")
        return;
      lock (ConnectionManager.connections)
      {
        if (!ConnectionManager.connections.ContainsKey(connectionId))
          return;
        EllieMae.EMLite.Server.Connection connection = ConnectionManager.connections[connectionId];
        connection.RemoveSession(session);
        TraceLog.WriteInfo(nameof (ConnectionManager), "Unregistered session " + session.SessionID + " from connection " + (object) connection);
        if (connection.SessionCount != 0)
          return;
        ConnectionManager.connections.Remove(connection.ConnectionID);
        TraceLog.WriteInfo(nameof (ConnectionManager), "Connection " + (object) connection + " has no valid sessions and is being discarded");
      }
    }

    public static bool? useXFF
    {
      get
      {
        if (!ConnectionManager._useXFF.HasValue)
        {
          using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass"))
            ConnectionManager._useXFF = registryKey == null ? new bool?(false) : new bool?(string.Concat(registryKey.GetValue("UseXFF")).Trim() == "1");
        }
        return ConnectionManager._useXFF;
      }
    }

    public static IPAddress GetCurrentClientIPAddress()
    {
      try
      {
        bool? useXff = ConnectionManager.useXFF;
        bool flag = true;
        if (useXff.GetValueOrDefault() == flag & useXff.HasValue)
        {
          if (EncompassServer.ServerMode == EncompassServerMode.HTTP)
          {
            string header = GenuineUtility.CurrentHttpContext.Request.Headers["x-forwarded-for"];
            TraceLog.WriteInfo(nameof (ConnectionManager), "XFF: '" + header + "'");
            string[] strArray = (string[]) null;
            if (header != null)
              strArray = header.Split(new string[1]{ "," }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray != null)
            {
              if (strArray.Length != 0)
              {
                string ipString = (strArray[strArray.Length - 1] ?? "").Trim();
                TraceLog.WriteInfo(nameof (ConnectionManager), "XFF client host: '" + ipString + "'");
                return IPAddress.Parse(ipString);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ConnectionManager), "Error getting XFF: " + ex.Message);
      }
      try
      {
        IPAddress hostAddress = ConnectionManager.getHostAddress(GenuineUtility.CurrentRemoteHost);
        TraceLog.WriteInfo(nameof (ConnectionManager), "Client host: '" + hostAddress.ToString() + "'");
        return hostAddress;
      }
      catch (Exception ex)
      {
        TraceLog.WriteWarning(nameof (ConnectionManager), "Error getting GC CurrentRemoteHost: " + ex.Message);
        return (IPAddress) null;
      }
    }

    public static void UpdateLastEchoTimeForConnection(string connectionId)
    {
      EllieMae.EMLite.Server.Connection connection = (EllieMae.EMLite.Server.Connection) null;
      lock (ConnectionManager.connections)
      {
        if (ConnectionManager.connections.ContainsKey(connectionId))
          connection = ConnectionManager.connections[connectionId];
      }
      if (connection == null)
        TraceLog.WriteWarning(nameof (ConnectionManager), "Received ECHO for unregistered connection '" + connectionId + "'");
      else
        connection.LastEchoTime = DateTime.Now;
    }

    private static void monitorConnections()
    {
      try
      {
        double totalSeconds = ConnectionManager.monitorInterval.TotalSeconds;
        string str1 = totalSeconds.ToString("#");
        totalSeconds = ServerGlobals.ConnectionTimeout.TotalSeconds;
        string str2 = totalSeconds.ToString("#");
        TraceLog.WriteInfo(nameof (ConnectionManager), "Connection monitoring thread started. Interval = " + str1 + ", Timeout = " + str2);
label_1:
        TimeSpan connectionTimeout;
        do
        {
          Thread.Sleep(ConnectionManager.monitorInterval);
          connectionTimeout = ServerGlobals.ConnectionTimeout;
        }
        while (connectionTimeout <= TimeSpan.Zero);
        EllieMae.EMLite.Server.Connection[] array = (EllieMae.EMLite.Server.Connection[]) null;
        lock (ConnectionManager.connections)
        {
          array = new EllieMae.EMLite.Server.Connection[ConnectionManager.connections.Count];
          ConnectionManager.connections.Values.CopyTo(array, 0);
        }
        Array.Sort<EllieMae.EMLite.Server.Connection>(array);
        foreach (EllieMae.EMLite.Server.Connection connection in array)
        {
          if (DateTime.Now - connection.LastEchoTime > connectionTimeout)
          {
            TraceLog.WriteInfo(nameof (ConnectionManager), "Abandoned connection detected (" + (object) connection + "). Last ECHO received " + connection.LastEchoTime.ToString("MM/dd/yyyy hh:mm:ss"));
            lock (ConnectionManager.connections)
              ConnectionManager.connections.Remove(connection.ConnectionID);
            if (!ConnectionManager.disableParallelTerminateConn)
              ThreadPool.QueueUserWorkItem(new WaitCallback(ConnectionManager.terminateConnection), (object) connection);
            else
              ConnectionManager.terminateConnection((object) connection);
          }
        }
        goto label_1;
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConnectionManager), "Connection monitoring thread terminated unexpectedly: " + (object) ex);
      }
    }

    private static void terminateConnection(object connObj)
    {
      if (!(connObj is EllieMae.EMLite.Server.Connection connection))
        return;
      try
      {
        TraceLog.WriteInfo(nameof (ConnectionManager), "Terminated " + (object) connection.TerminateAllSessions() + " session(s) on connection " + (object) connection);
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ConnectionManager), "Error terminating connection from client '" + connection.HostName + "': " + (object) ex);
      }
    }

    private static SecuritySessionParameters getSecurityContext()
    {
      bool serverEncryption = EnConfigurationSettings.GlobalSettings.ServerEncryption;
      bool serverCompression = EnConfigurationSettings.GlobalSettings.ServerCompression;
      TimeSpan minValue = TimeSpan.MinValue;
      if (serverEncryption & serverCompression)
        return new SecuritySessionParameters("/Encryption", SecuritySessionAttributes.EnableCompression, minValue);
      if (serverEncryption)
        return new SecuritySessionParameters("/Encryption", SecuritySessionAttributes.None, minValue);
      return serverCompression ? new SecuritySessionParameters("/Basic", SecuritySessionAttributes.EnableCompression, minValue) : new SecuritySessionParameters("/Basic", SecuritySessionAttributes.None, minValue);
    }

    private static bool isIpAddressAllowed(IPAddress addr)
    {
      if (ConnectionManager.allowedIpAddresses == null || ConnectionManager.allowedIpAddresses.Length == 0)
        return true;
      for (int index = 0; index < ConnectionManager.allowedIpAddresses.Length; ++index)
      {
        if (ConnectionManager.allowedIpAddresses[index].IsInRange(addr))
          return true;
      }
      return addr.ToString() == "127.0.0.1";
    }

    private static void establishClientSecuritySession(HostInformation hostInfo)
    {
      hostInfo.SecuritySessionParameters = ConnectionManager.securityContext;
      string str = (ConnectionManager.securityContext.Attributes & SecuritySessionAttributes.EnableCompression) != SecuritySessionAttributes.None ? " with Compression" : "";
      ServerGlobals.TraceLog.WriteVerbose(nameof (ConnectionManager), "Established security context " + ConnectionManager.securityContext.Name + str + " for " + ConnectionManager.hostInfoToClientID(hostInfo));
    }

    private static void registerChannelEvents()
    {
      foreach (IChannel registeredChannel in ChannelServices.RegisteredChannels)
      {
        if (registeredChannel is BasicChannelWithSecurity channelWithSecurity)
        {
          channelWithSecurity.GenuineChannelsEvent += new GenuineChannelsGlobalEventHandler(ConnectionManager.defaultChannelEventHandler);
          ConnectionManager.configureSecurityContexts(channelWithSecurity.ITransportContext);
          ServerGlobals.TraceLog.WriteVerbose(nameof (ConnectionManager), "Registered events for channel " + channelWithSecurity.ChannelName);
        }
      }
    }

    private static void configureSecurityContexts(ITransportContext transportContext)
    {
      transportContext.IKeyStore.SetKey("/Basic", (IKeyProvider) new KeyProvider_Basic());
      transportContext.IKeyStore.SetKey("/Encryption", (IKeyProvider) new KeyProvider_SelfEstablishingSymmetric());
    }

    private static void defaultChannelEventHandler(object sender, GenuineEventArgs e)
    {
      if (e.HostInformation == null)
        ServerGlobals.TraceLog.WriteVerbose(nameof (ConnectionManager), "Channel event caught of type " + e.EventType.ToString());
      else
        ServerGlobals.TraceLog.WriteVerbose(nameof (ConnectionManager), "Channel event caught of type " + e.EventType.ToString() + " from client " + ConnectionManager.hostInfoToClientID(e.HostInformation));
      if (e.EventType == GenuineEventType.GeneralConnectionEstablished)
      {
        string str = (string) e.HostInformation[(object) "Guid"];
        e.HostInformation[(object) "Guid"] = (object) Guid.NewGuid().ToString();
        ConnectionManager.establishClientSecuritySession(e.HostInformation);
        EncompassServer.RaiseEvent((IClientContext) null, (ServerEvent) new ConnectionEvent(ConnectionEventType.Accepted, e.HostInformation.Uri, ConnectionManager.getHostAddress(e.HostInformation)));
        ServerGlobals.TraceLog.WriteInfo(nameof (ConnectionManager), "Connection established from client " + ConnectionManager.hostInfoToClientID(e.HostInformation));
      }
      else if (e.EventType == GenuineEventType.GTcpConnectionAccepted)
      {
        ConnectionAcceptedCancellableEventParameter additionalInfo = (ConnectionAcceptedCancellableEventParameter) e.AdditionalInfo;
        if (ConnectionManager.isIpAddressAllowed(additionalInfo.IPEndPoint.Address))
          return;
        additionalInfo.Cancel = true;
        ServerGlobals.TraceLog.WriteWarning(nameof (ConnectionManager), "Connection refused from client " + additionalInfo.IPEndPoint.Address.ToString());
        EncompassServer.RaiseEvent((IClientContext) null, (ServerEvent) new ConnectionEvent(ConnectionEventType.Rejected, "", additionalInfo.IPEndPoint.Address));
      }
      else
      {
        if (e.EventType != GenuineEventType.GeneralConnectionClosed)
          return;
        EncompassServer.RaiseEvent((IClientContext) null, (ServerEvent) new ConnectionEvent(ConnectionEventType.Closed, e.HostInformation.Uri, ConnectionManager.getHostAddress(e.HostInformation)));
        ServerGlobals.TraceLog.WriteInfo(nameof (ConnectionManager), "Connection closed from client " + ConnectionManager.hostInfoToClientID(e.HostInformation));
      }
    }

    private static string hostInfoToClientID(HostInformation hi)
    {
      IPAddress hostAddress = ConnectionManager.getHostAddress(hi);
      return hi.Uri + (hostAddress != null ? " (" + hostAddress.ToString() + ")" : "");
    }

    private static IPAddress getHostAddress(HostInformation hi)
    {
      try
      {
        if (hi.PhysicalAddress is IPEndPoint)
          return ((IPEndPoint) hi.PhysicalAddress).Address;
        return hi.PhysicalAddress is string ? IPAddress.Parse(hi.PhysicalAddress.ToString()) : (IPAddress) null;
      }
      catch
      {
        return (IPAddress) null;
      }
    }
  }
}
