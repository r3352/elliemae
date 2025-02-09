// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Client.ConnectionBase
// Assembly: Client, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: AD6D6217-37E4-4BE3-B44A-5E3BA190AF3A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Client.dll

using Belikov.GenuineChannels;
using Belikov.GenuineChannels.DotNetRemotingLayer;
using Belikov.GenuineChannels.GenuineHttp;
using Belikov.GenuineChannels.GenuineTcp;
using Belikov.GenuineChannels.Logbook;
using Belikov.GenuineChannels.Security;
using Belikov.GenuineChannels.TransportContext;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Client
{
  public abstract class ConnectionBase : 
    MarshalByRefObject,
    IConnection,
    IDisposable,
    IServerCallback
  {
    private const string className = "ConnectionBase";
    protected const string versionControlUri = "VersionControl.rem";
    protected const string loginManagerUri = "LoginManager.rem";
    protected const string encryptionContext = "/Encryption";
    protected const string basicContext = "/Basic";
    private static string sw = Tracing.SwRemoting;
    protected static Hashtable remoteConns = new Hashtable();
    protected IClientSession session;
    protected string remotingUri;
    protected EllieMae.EMLite.ClientServer.ServerIdentity serverId;

    public event ServerEventHandler ServerEvent;

    public event ConnectionErrorEventHandler ConnectionError;

    static ConnectionBase()
    {
      EnCertificatePolicy.SetDefaultPolicy();
      if (EnConfigurationSettings.GlobalSettings.GCTrace)
        GenuineLoggingServices.SetUpLoggingToFile(Path.Combine(SystemSettings.LogDir, Environment.UserName + "\\Encompass"), "C1E1M2I3S1B2T1X1H1A1D2V1L0N1");
      IDictionary channelProperties1 = EnConfigurationSettings.GlobalSettings.GetTCPClientChannelProperties();
      channelProperties1.Add((object) "priority", (object) "21");
      GenuineTcpChannel chnl1 = new GenuineTcpChannel(channelProperties1, (IClientChannelSinkProvider) null, (IServerChannelSinkProvider) null);
      chnl1.ITransportContext.IKeyStore.SetKey("/Encryption", (IKeyProvider) new KeyProvider_SelfEstablishingSymmetric());
      chnl1.ITransportContext.IKeyStore.SetKey("/Basic", (IKeyProvider) new KeyProvider_Basic());
      if (ChannelServices.GetChannel((string) channelProperties1[(object) "name"]) == null)
        ChannelServices.RegisterChannel((IChannel) chnl1, false);
      BinaryServerFormatterSinkProvider formatterSinkProvider = new BinaryServerFormatterSinkProvider();
      formatterSinkProvider.TypeFilterLevel = TypeFilterLevel.Full;
      CachingClientChannelSinkProvider channelSinkProvider = new CachingClientChannelSinkProvider();
      channelSinkProvider.Next = (IClientChannelSinkProvider) new BinaryClientFormatterSinkProvider();
      IDictionary channelProperties2 = EnConfigurationSettings.GlobalSettings.GetHTTPClientChannelProperties();
      channelProperties2.Add((object) "priority", (object) "21");
      if (EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders != null)
      {
        foreach (string key in EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders.Keys)
        {
          if (channelProperties2.Contains((object) "HttpCustomHeaders") && !string.IsNullOrWhiteSpace((string) channelProperties2[(object) "HttpCustomHeaders"]))
          {
            IDictionary dictionary = channelProperties2;
            dictionary[(object) "HttpCustomHeaders"] = (object) (dictionary[(object) "HttpCustomHeaders"].ToString() + ";");
          }
          IDictionary dictionary1 = channelProperties2;
          dictionary1[(object) "HttpCustomHeaders"] = (object) (dictionary1[(object) "HttpCustomHeaders"].ToString() + key + ":" + EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders[key]);
        }
      }
      GenuineHttpClientChannel chnl2 = new GenuineHttpClientChannel(channelProperties2, (IClientChannelSinkProvider) channelSinkProvider, (IServerChannelSinkProvider) formatterSinkProvider);
      chnl2.ITransportContext.IKeyStore.SetKey("/Encryption", (IKeyProvider) new KeyProvider_SelfEstablishingSymmetric());
      chnl2.ITransportContext.IKeyStore.SetKey("/Basic", (IKeyProvider) new KeyProvider_Basic());
      if (ChannelServices.GetChannel((string) channelProperties2[(object) "name"]) == null)
        ChannelServices.RegisterChannel((IChannel) chnl2, false);
      GenuineGlobalEventProvider.GenuineChannelsGlobalEvent += new GenuineChannelsGlobalEventHandler(ConnectionBase.channelEventHandler);
      GCUtility.AddBeforeStartRequestEventHandler((BeforeStartRequestEventHandler) (args =>
      {
        string correlationId = Guid.NewGuid().ToString();
        DiagUtility.LoggerScopeProvider.EnterNew((Action<ILoggerScopeBuilder>) (scope => scope.SetCorrelationId(correlationId)));
        GCUtility.SetTransportCorrelationIdentifer(correlationId);
      }));
      GCUtility.AddAfterFinishRequestEventHandler((AfterFinishRequestEventHandler) (args =>
      {
        try
        {
          ILogger logger = DiagUtility.LogManager.GetLogger("APITrace");
          if (!logger.IsEnabled(Encompass.Diagnostics.Logging.LogLevel.DEBUG))
            return;
          ApiTraceLog log = new ApiTraceLog();
          string fullName = args.MethodInfo.ReflectedType.FullName;
          string name = args.MethodInfo.Name;
          log.Message = "Call to " + fullName + "." + name + "() " + (args.Success ? "Completed " : "Failed ") + ".";
          log.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.StartTime, args.StartTime.ToUniversalTime());
          log.Set<DateTime>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.EndTime, args.EndTime.ToUniversalTime());
          log.Set<double>(Encompass.Diagnostics.Logging.Schema.Log.CommonFields.DurationMS, (args.EndTime - args.StartTime).TotalMilliseconds);
          logger.Write<ApiTraceLog>(log);
        }
        catch
        {
        }
        finally
        {
          DiagUtility.LoggerScopeProvider.ExitCurrent();
        }
      }));
    }

    public ISession Session
    {
      get
      {
        this.ensureConnected();
        return this.session as ISession;
      }
    }

    public abstract bool IsServerInProcess { get; }

    public EllieMae.EMLite.ClientServer.ServerIdentity Server
    {
      get
      {
        this.ensureConnected();
        return this.serverId;
      }
    }

    public bool IsConnected => this.session != null;

    public void Dispose() => this.Dispose(true);

    protected void Dispose(bool disposing)
    {
      try
      {
        this.Close();
        if (!disposing)
          return;
        GC.SuppressFinalize((object) this);
      }
      catch
      {
      }
    }

    ~ConnectionBase() => this.Dispose(false);

    public void Close()
    {
      try
      {
        System.Runtime.Remoting.RemotingServices.Disconnect((MarshalByRefObject) this);
        if (this.session != null)
          this.session.Abandon();
      }
      catch
      {
      }
      try
      {
        if (this.remotingUri != null)
          ConnectionBase.remoteConns.Remove((object) this.remotingUri);
      }
      catch
      {
      }
      this.session = (IClientSession) null;
    }

    public void OnServerEvent(EllieMae.EMLite.ClientServer.Events.ServerEvent e)
    {
      Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Info, "Received server event of type " + e.GetType().Name);
      switch (e)
      {
        case ConnectEvent _:
          if (!this.IsServerInProcess)
          {
            this.establishSecuritySession(((ConnectEvent) e).SecurityOptions);
            break;
          }
          break;
        case PingEvent _:
          return;
      }
      Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Verbose, "Queuing server event dispatch");
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.dispatchServerEvent), (object) e);
    }

    protected void establishSecuritySession(SecurityContextOptions securityOptions)
    {
      HostInformation hostInformation = (HostInformation) null;
      try
      {
        hostInformation = GenuineUtility.CurrentRemoteHost;
      }
      catch
      {
      }
      if (hostInformation != null)
      {
        Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Verbose, "Establishing session security parameters: " + securityOptions.ContextName);
        Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Verbose, "Establishing session compression parameters: " + securityOptions.CompressionEnabled.ToString());
        hostInformation.SecuritySessionParameters = new SecuritySessionParameters(securityOptions.ContextName, securityOptions.CompressionEnabled ? SecuritySessionAttributes.EnableCompression : SecuritySessionAttributes.None, TimeSpan.MinValue);
        this.remotingUri = GenuineUtility.CurrentMessage.ITransportContext.HostIdentifier;
        if (ConnectionBase.remoteConns.ContainsKey((object) this.remotingUri))
          ConnectionBase.remoteConns[(object) this.remotingUri] = (object) this;
        else
          ConnectionBase.remoteConns.Add((object) this.remotingUri, (object) this);
      }
      else
        Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Error, "NULL HostInfo received during connection event -- cannot establish security context.");
    }

    public override object InitializeLifetimeService() => (object) null;

    protected void dispatchServerEvent(object o)
    {
      if (this.ServerEvent == null)
        return;
      this.ServerEvent((IConnection) this, (EllieMae.EMLite.ClientServer.Events.ServerEvent) o);
    }

    protected void serializeRemoteObject(string filename, MarshalByRefObject obj)
    {
      ObjRef objRefForProxy = System.Runtime.Remoting.RemotingServices.GetObjRefForProxy(obj);
      if (objRefForProxy != null)
      {
        using (FileStream serializationStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
          new SoapFormatter().Serialize((Stream) serializationStream, (object) objRefForProxy);
      }
      else
      {
        using (StreamWriter streamWriter = new StreamWriter(filename, false, Encoding.ASCII))
        {
          streamWriter.WriteLine("Not an ObjRef");
          streamWriter.WriteLine("Transparent Proxy: " + System.Runtime.Remoting.RemotingServices.IsTransparentProxy((object) obj).ToString());
        }
      }
    }

    protected void onConnectionError(ConnectionErrorType errType)
    {
      if (this.ConnectionError == null)
        return;
      this.ConnectionError((IConnection) this, errType);
    }

    protected void ensureConnected()
    {
      if (this.session == null)
        throw new ApplicationException("Connection must be open for this operation.");
    }

    protected void ensureDisconnected()
    {
      if (this.session != null)
        throw new ApplicationException("Connection must be closed for this operation.");
    }

    protected SecuritySessionParameters getLoginSecurityContext(ILoginManager mngr)
    {
      SecurityContextOptions clientSecurityOptions = mngr.GetClientSecurityOptions();
      Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Info, "Establishing security context " + clientSecurityOptions.ContextName + " for login.");
      return new SecuritySessionParameters(clientSecurityOptions.ContextName, SecuritySessionAttributes.ForceSync, new TimeSpan(0, 1, 0));
    }

    protected static void channelEventHandler(object sender, GenuineEventArgs e)
    {
      Tracing.Log(ConnectionBase.sw, nameof (ConnectionBase), TraceLevel.Info, "Channel event occurred of type " + (object) e.EventType);
      if (e.EventType != GenuineEventType.GeneralConnectionClosed)
        return;
      string hostIdentifier = e.HostInformation.ITransportContext.HostIdentifier;
      if (!ConnectionBase.remoteConns.ContainsKey((object) hostIdentifier))
        return;
      ((ConnectionBase) ConnectionBase.remoteConns[(object) hostIdentifier]).onConnectionError(ConnectionErrorType.ConnectionClosed);
    }
  }
}
