// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.Connection
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using Belikov.GenuineChannels;
using Belikov.GenuineChannels.Security;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.VersionInterface15;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public class Connection : ConnectionBase
  {
    private const string className = "Connection";
    private static string sw = Tracing.SwRemoting;

    ~Connection() => this.Dispose(false);

    public void Open(string serverName, string userId, string password, string appName)
    {
      this.Open(serverName, userId, password, appName, true, (string) null);
    }

    public void Open(
      string serverName,
      string userId,
      string password,
      string appName,
      bool licenseRequired)
    {
      this.Open(serverName, userId, password, appName, licenseRequired, (string) null);
    }

    public void Open(
      string serverName,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      string prevSessionID,
      string authCode)
    {
      this.ensureDisconnected();
      EllieMae.EMLite.ClientServer.ServerIdentity serverIdentity = EllieMae.EMLite.ClientServer.ServerIdentity.Parse(serverName);
      EllieMae.EMLite.ClientServer.ServerIdentity[] serverIdentityArray = ServerResolver.Resolve(serverIdentity);
      LoginParameters loginParams = new LoginParameters(userId, password, serverIdentity, ConnectionManager.ConnectionID, appName, licenseRequired, prevSessionID);
      if (userId == "(trusted)")
        throw new LoginException(loginParams, (IPAddress) null, LoginReturnCode.USERID_NOT_FOUND);
      Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Beginning to connect to server " + serverName.ToString());
      for (int index = 0; index < serverIdentityArray.Length; ++index)
      {
        try
        {
          this.connectToHost(serverIdentityArray[index], userId, password, appName, licenseRequired, false, prevSessionID, authCode);
          this.serverId = serverIdentity;
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Info, "Connecting to server " + serverIdentityArray[index].Uri.ToString());
          Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Connecting to server" + serverIdentityArray[index].Uri.ToString());
          return;
        }
        catch (ServerConnectionException ex)
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Info, "Error connecting to server " + serverIdentityArray[index].Uri.ToString() + ": " + ex.Message);
          Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Error connecting to server " + serverIdentityArray[index].Uri.ToString() + ": " + ex.Message);
        }
      }
      Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Unable to connect to remote server. Server may be stopped of refusing connections.");
      throw new ServerConnectionException(this.serverId, "Unable to connect to remote server. Server may be stopped or refusing connections.");
    }

    public void Open(
      string serverName,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      string prevSessionID)
    {
      this.ensureDisconnected();
      EllieMae.EMLite.ClientServer.ServerIdentity serverIdentity = EllieMae.EMLite.ClientServer.ServerIdentity.Parse(serverName);
      EllieMae.EMLite.ClientServer.ServerIdentity[] serverIdentityArray = ServerResolver.Resolve(serverIdentity);
      LoginParameters loginParams = new LoginParameters(userId, password, serverIdentity, ConnectionManager.ConnectionID, appName, licenseRequired, prevSessionID);
      if (userId == "(trusted)")
        throw new LoginException(loginParams, (IPAddress) null, LoginReturnCode.USERID_NOT_FOUND);
      Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Beginning to connect to server " + serverName.ToString());
      for (int index = 0; index < serverIdentityArray.Length; ++index)
      {
        try
        {
          this.connectToHost(serverIdentityArray[index], userId, password, appName, licenseRequired, false, prevSessionID);
          this.serverId = serverIdentity;
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Info, "Connecting to server " + serverIdentityArray[index].Uri.ToString());
          Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Connecting to server" + serverIdentityArray[index].Uri.ToString());
          return;
        }
        catch (ServerConnectionException ex)
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Info, "Error connecting to server " + serverIdentityArray[index].Uri.ToString() + ": " + ex.Message);
          Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Error connecting to server " + serverIdentityArray[index].Uri.ToString() + ": " + ex.Message);
        }
      }
      Tracing.Log(Connection.sw, "SMU", TraceLevel.Info, "Unable to connect to remote server. Server may be stopped of refusing connections.");
      throw new ServerConnectionException(this.serverId, "Unable to connect to remote server. Server may be stopped or refusing connections.");
    }

    private void connectToHost(
      EllieMae.EMLite.ClientServer.ServerIdentity sid,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      bool isRemoteAdmin,
      string prevSessionID)
    {
      try
      {
        JedVersion version = VersionInformation.CurrentVersion.Version;
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Retrieving VersionControl interface from server " + sid.ToString());
        IVersionControl serverVersionControl = (IVersionControl) Activator.GetObject(typeof (IVersionControl), sid.Uri.AbsoluteUri + "VersionControl.rem", (object) null);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Get Server Version", 156, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Performing version compatibility check to server " + sid.Uri.AbsoluteUri);
        if (!serverVersionControl.IsCompatibleWithVersion(version, !AssemblyResolver.IsSmartClient))
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Version compatibility check failed. Raising exception.");
          throw new VersionMismatchException(version, serverVersionControl);
        }
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Version compatibility checked passed");
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Version Check", 168, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        ILoginManager mngr = (ILoginManager) Activator.GetObject(typeof (ILoginManager), sid.Uri.AbsoluteUri + "LoginManager.rem", (object) null);
        using (new SecurityContextKeeper(this.getLoginSecurityContext(mngr)))
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Logging in user \"" + userId + "\" to server \"" + sid.ToString() + "\"");
          LoginParameters loginParams = new LoginParameters(userId, password, sid, ConnectionManager.ConnectionID, appName, licenseRequired, prevSessionID);
          if (isRemoteAdmin)
            this.session = (IClientSession) mngr.Manage(loginParams, (IServerCallback) this);
          else
            this.session = (IClientSession) mngr.Login(loginParams, (IServerCallback) this);
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Login successful.");
        }
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Remoting Server Login", 189, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        ConnectionManager.StartMonitoring(sid);
      }
      catch (Exception ex)
      {
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Warning, "Error establishing connection to remote host \"" + sid.ToString() + "\" : " + (object) ex);
        switch (ex)
        {
          case RemotingException _ when ex.Message.IndexOf("IsCompatibleWithVersion") >= 0:
            throw new VersionMismatchException(VersionInformation.CurrentVersion.Version, (IVersionControl) new ConnectionVersionControl());
          case OperationException _:
          case SocketException _:
          case RemotingException _:
          case WebException _:
          case SecurityException _:
            throw new ServerConnectionException(sid, "Unable to connect to remote server. Server may be stopped or refusing connections.");
          default:
            throw;
        }
      }
    }

    private void connectToHost(
      EllieMae.EMLite.ClientServer.ServerIdentity sid,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      bool isRemoteAdmin,
      string prevSessionID,
      string authCode)
    {
      try
      {
        JedVersion version = VersionInformation.CurrentVersion.Version;
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Retrieving VersionControl interface from server " + sid.ToString());
        IVersionControl serverVersionControl = (IVersionControl) Activator.GetObject(typeof (IVersionControl), sid.Uri.AbsoluteUri + "VersionControl.rem", (object) null);
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Get Server Version", 224, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Performing version compatibility check to server " + sid.Uri.AbsoluteUri);
        if (!serverVersionControl.IsCompatibleWithVersion(version, !AssemblyResolver.IsSmartClient))
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Version compatibility check failed. Raising exception.");
          throw new VersionMismatchException(version, serverVersionControl);
        }
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Version compatibility checked passed");
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Version Check", 236, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        ILoginManager mngr = (ILoginManager) Activator.GetObject(typeof (ILoginManager), sid.Uri.AbsoluteUri + "LoginManager.rem", (object) null);
        using (new SecurityContextKeeper(this.getLoginSecurityContext(mngr)))
        {
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Logging in user \"" + userId + "\" to server \"" + sid.ToString() + "\"" + authCode + "\"");
          LoginParameters loginParams = new LoginParameters(userId, password, sid, ConnectionManager.ConnectionID, appName, licenseRequired, prevSessionID, authCode);
          this.session = (IClientSession) mngr.Login(loginParams, (IServerCallback) this);
          Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Verbose, "Login successful.");
        }
        PerformanceMeter.Current.AddCheckpoint("Session.Start - Connection - Remoting Server Login", 254, nameof (connectToHost), "D:\\ws\\24.3.0.0\\EmLite\\Client\\Connection.cs");
        ConnectionManager.StartMonitoring(sid);
      }
      catch (Exception ex)
      {
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Warning, "Error establishing connection to remote host \"" + sid.ToString() + "\" : " + (object) ex);
        switch (ex)
        {
          case RemotingException _ when ex.Message.IndexOf("IsCompatibleWithVersion") >= 0:
            throw new VersionMismatchException(VersionInformation.CurrentVersion.Version, (IVersionControl) new ConnectionVersionControl());
          case OperationException _:
          case SocketException _:
          case RemotingException _:
          case WebException _:
          case SecurityException _:
            throw new ServerConnectionException(sid, "Unable to connect to remote server. Server may be stopped or refusing connections.");
          default:
            throw;
        }
      }
    }

    public void OpenRemoteManagementInterface(string serverName, string password)
    {
      this.ensureDisconnected();
      EllieMae.EMLite.ClientServer.ServerIdentity sid = EllieMae.EMLite.ClientServer.ServerIdentity.Parse(serverName);
      try
      {
        this.connectToHost(sid, "(rmi)", password, "", false, true, (string) null);
        this.serverId = sid;
        return;
      }
      catch (ServerConnectionException ex)
      {
        Tracing.Log(Connection.sw, nameof (Connection), TraceLevel.Info, "Error connecting to server " + sid.Uri.ToString() + ": " + ex.Message);
      }
      throw new ServerConnectionException(this.serverId, "Unable to connect to remote server. Server may be stopped or refusing connections.");
    }

    public IManagementSession ManagementSession
    {
      get
      {
        this.ensureConnected();
        return this.session as IManagementSession;
      }
    }

    public override bool IsServerInProcess => false;
  }
}
