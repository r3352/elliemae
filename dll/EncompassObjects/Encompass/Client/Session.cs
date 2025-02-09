// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.Session
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using Elli.Metrics.Client;
using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.LockDeskSettings;
using EllieMae.Encompass.BusinessObjects.Settings;
using EllieMae.Encompass.BusinessObjects.TradeManagement;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Configuration;
using EllieMae.Encompass.Licensing;
using EllieMae.Encompass.Reporting;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Config;
using Encompass.Diagnostics.Logging;
using Encompass.Diagnostics.Logging.Targets;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Client
{
  [ComSourceInterfaces(typeof (ISessionEvents))]
  public class Session : ISession, IDisposable
  {
    private ScopedEventHandler<DisconnectedEventArgs> disconnected;
    private ScopedEventHandler<ServerMessageEventArgs> messageArrived;
    private const string className = "Session�";
    private static string sw = Tracing.SwOutsideLoan;
    private string processName = Process.GetCurrentProcess().ProcessName.Replace(" ", string.Empty);
    private string asmName = Assembly.GetCallingAssembly().GetName().Name.Replace(" ", string.Empty);
    private string asmVersion = Assembly.GetCallingAssembly().GetName().Version.ToString();
    private string appName = string.Empty;
    private string serverUri;
    private string instanceName;
    private IConnection conn;
    private SessionObjects sessionObjects;
    private EllieMae.Encompass.BusinessObjects.Loans.Loans loans;
    private EllieMae.Encompass.BusinessObjects.Contacts.Contacts contacts;
    private EllieMae.Encompass.BusinessObjects.Users.Users users;
    private Organizations orgs;
    private EllieMae.Encompass.BusinessObjects.Calendar.Calendar calendar;
    private SystemSettings settings;
    private bool disposed;
    private string guid = "{" + Guid.NewGuid().ToString() + "}";
    private ServerEvents serverEvents;
    private DataExchange dataExchange;
    private Reports reports;
    private CorrespondentMasterService corresondentMaster;
    private bool isEncompassGuiSession;
    private CorrespondentTradeService correspondentTradeService;
    private GSECommitmentService gseCommitmentService;
    private LockDeskSettingsService lockDeskSettingsService;
    private LoanTradeService loanTradeService;
    private SecurityTradeService securityService;
    private MbsPoolService mbsPoolService;
    private MasterContractService masterContractService;
    private TradeBatchService tradeBatchService;
    private SettingsService settingsService;
    private static string serverLogListenerOwner = (string) null;
    private static RemotingLogConfigChangeHandler configChangeHandler = (RemotingLogConfigChangeHandler) null;
    private string oapiGatewayBaseUri;
    private static readonly Dictionary<string, string> sessions = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
    private static readonly bool externalApplication;
    private static readonly string externalModuleName;
    private static readonly string externalAssemblyName;

    public event DisconnectedEventHandler Disconnected
    {
      add
      {
        if (value == null)
          return;
        this.disconnected.Add(new ScopedEventHandler<DisconnectedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.disconnected.Remove(new ScopedEventHandler<DisconnectedEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    public event ServerMessageEventHandler MessageArrived
    {
      add
      {
        if (value == null)
          return;
        this.messageArrived.Add(new ScopedEventHandler<ServerMessageEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.messageArrived.Remove(new ScopedEventHandler<ServerMessageEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    static Session()
    {
      ServicePointManager.SecurityProtocol |= (SecurityProtocolType) 3840;
      bool flag;
      using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Ellie Mae\\Encompass\\SDK\\DisableCustomAssemblyResolution"))
        flag = registryKey != null;
      if (!flag)
        AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(Session.CurrentDomain_AssemblyResolve);
      StackTrace stackTrace = new StackTrace();
      int index = 2;
      while (stackTrace.GetFrame(index).GetMethod().DeclaringType.FullName.StartsWith("System") && stackTrace.GetFrame(index + 1) != null)
        ++index;
      MethodBase method = stackTrace.GetFrame(index).GetMethod();
      if (!method.DeclaringType.FullName.StartsWith("EllieMae.EMLite") && !method.DeclaringType.FullName.StartsWith("EllieMae.Encompass"))
      {
        Session.externalApplication = true;
        Session.externalAssemblyName = method.DeclaringType.Assembly.ToString();
        Session.externalModuleName = method.Module.Name;
      }
      else
      {
        Session.externalApplication = false;
        Session.externalAssemblyName = (string) null;
        Session.externalModuleName = (string) null;
      }
      ApplicationLog.Initialize();
    }

    private void InitAppName()
    {
      this.appName = "API." + this.processName;
      if (this.processName != this.asmName)
        this.appName = this.appName + "." + this.asmName;
      if (EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders.ContainsKey("X-ClientType"))
        return;
      EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders["X-ClientType"] = this.appName.Replace(":", "").Replace(";", "");
    }

    public Session()
    {
      this.InitEvents();
      this.InitAppName();
      if (!new LicenseManager().ValidateLicense(true))
        throw new LicenseException("This machine is not licensed to use the Encompass API.");
    }

    private void InitEvents()
    {
      this.disconnected = new ScopedEventHandler<DisconnectedEventArgs>(nameof (Session), "Disconnected");
      this.messageArrived = new ScopedEventHandler<ServerMessageEventArgs>(nameof (Session), "MessageArrived");
    }

    private Session(
      IConnection conn,
      ISessionStartupInfo startupInfo,
      string userPassword,
      string serverUri)
    {
      this.InitEvents();
      this.InitAppName();
      this.isEncompassGuiSession = true;
      this.initializeSession(conn, startupInfo, userPassword, serverUri);
    }

    public void Start(string serverUri, string userId, string password)
    {
      this.ensureNotDisposed();
      if (this.conn != null)
        throw new InvalidOperationException("Session has already been started");
      if (Session.externalApplication)
        APICallContext.SetAsCurrent((IApiSourceContext) new APICallContext(Session.externalModuleName, Session.externalAssemblyName, (APICallSourceType) 0, (string) null));
      try
      {
        DateTime now = DateTime.Now;
        EllieMae.EMLite.Client.Connection c = new EllieMae.EMLite.Client.Connection();
        c.Open(serverUri, (userId ?? "").ToLower(), password ?? "", this.appName);
        this.initializeSession((IConnection) c, (ISessionStartupInfo) null, password, serverUri);
        TimeSpan timeSpan = DateTime.Now - now;
        ClientMetricsProviderFactory.Initiate(this.ClientID, ((ConnectionBase) c).Server.InstanceName, this.UserID, string.Format("{0}(V{1})", (object) this.asmName, (object) this.asmVersion), false, "gFrGKy5i9_bqmbUaeaAeOQ", 1000);
        ClientMetricsProviderFactory.IncrementCounter("LogonIncCounter", new SFxTag[1]
        {
          (SFxTag) new SFxSdkTag()
        });
        ClientMetricsProviderFactory.RecordIncrementalTimerSample("LogonIncTimer", timeSpan, new SFxTag[1]
        {
          (SFxTag) new SFxSdkTag()
        });
      }
      catch (LoginException ex)
      {
        throw new LoginException(ex);
      }
      catch (VersionMismatchException ex)
      {
        throw new VersionException(ex);
      }
      catch (ServerConnectionException ex)
      {
        throw new ConnectionException(ex);
      }
    }

    public void StartOffline(string userId, string password)
    {
      this.StartInstance(userId, password, (string) null);
    }

    public void StartInstance(string userId, string password, string instanceName)
    {
      this.ensureNotDisposed();
      if (this.conn != null)
        throw new InvalidOperationException("Session has already been started");
      if (Session.externalApplication)
        APICallContext.SetAsCurrent((IApiSourceContext) new APICallContext(Session.externalModuleName, Session.externalAssemblyName, (APICallSourceType) 0, (string) null));
      if (EnConfigurationSettings.GlobalSettings.InstallationMode == null)
        throw new InvalidOperationException("Encompass client installations cannot be used to start an offline session.");
      try
      {
        if (instanceName == null)
          instanceName = EnConfigurationSettings.InstanceName;
        InProcConnection c = new InProcConnection();
        c.OpenInProcess((userId ?? "").ToLower(), password ?? "", instanceName, this.appName, true);
        this.initializeSession((IConnection) c, (ISessionStartupInfo) null, password, (string) null);
      }
      catch (LoginException ex)
      {
        throw new LoginException(ex);
      }
    }

    void IDisposable.Dispose()
    {
      if (this.isEncompassGuiSession)
        throw new InvalidOperationException("The Session cannot be closed in this manner when running within Encompass.");
      if (this.conn != null)
      {
        this.closeSession();
        this.onDisconnected(DisconnectReason.SessionDisposed);
      }
      if (Session.externalApplication)
        APICallContext.SetAsCurrent((IApiSourceContext) null);
      this.disposed = true;
    }

    public void End()
    {
      ((IDisposable) this).Dispose();
      try
      {
        lock (Session.sessions)
        {
          if (Session.sessions.ContainsKey(this.ID))
            Session.sessions.Remove(this.ID);
          if (Session.sessions.Count == 0)
            SystemSettings.DeleteTempFolderFiles((string[]) null);
          if (!string.Equals(Session.serverLogListenerOwner, this.ID))
            return;
          DiagConfig<ClientDiagConfigData>.Instance.RemoveGlobalData("SessionId");
          DiagConfig<ClientDiagConfigData>.Instance.RemoveGlobalData("UserId");
          DiagConfig<ClientDiagConfigData>.Instance.RemoveHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) Session.configChangeHandler);
          Session.configChangeHandler = (RemotingLogConfigChangeHandler) null;
          Session.serverLogListenerOwner = (string) null;
        }
      }
      catch
      {
      }
    }

    public string UserID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.UserInfo.Userid;
      }
    }

    public string ClientID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.ServerLicense.ClientID;
      }
    }

    public string SystemID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.SystemInformation.SystemID;
      }
    }

    public string ServerID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.ServerID;
      }
    }

    public string ServerURI => this.serverUri;

    public string ID => this.guid;

    public string ServerSessionID => this.sessionObjects.SessionID;

    public string OAPIGatewayBaseUri => this.oapiGatewayBaseUri;

    public string GetAuthToken()
    {
      try
      {
        return new OAuth2(this.oapiGatewayBaseUri, (CacheItemRetentionPolicy) null).GetAccessToken(ServerIdentity.Parse(this.serverUri).InstanceName, this.sessionObjects.SessionID, "sc", true).TypeAndToken;
      }
      catch
      {
        return (string) null;
      }
    }

    public EncompassEdition EncompassEdition
    {
      get
      {
        this.ensureConnected();
        if (this.sessionObjects.ServerLicense.Edition == 3)
          return EncompassEdition.Broker;
        return this.sessionObjects.ServerLicense.Edition == 5 ? EncompassEdition.Banker : EncompassEdition.Unknown;
      }
    }

    public DateTime GetServerTime() => ((IClientSession) this.sessionObjects.Session).ServerTime;

    public EllieMae.Encompass.BusinessObjects.Loans.Loans Loans
    {
      get
      {
        this.ensureConnected();
        return this.loans;
      }
    }

    public EllieMae.Encompass.BusinessObjects.Calendar.Calendar Calendar
    {
      get
      {
        this.ensureConnected();
        return this.calendar;
      }
    }

    public EllieMae.Encompass.BusinessObjects.Contacts.Contacts Contacts
    {
      get
      {
        this.ensureConnected();
        return this.contacts;
      }
    }

    public EllieMae.Encompass.BusinessObjects.Users.Users Users
    {
      get
      {
        this.ensureConnected();
        return this.users;
      }
    }

    public Organizations Organizations
    {
      get
      {
        this.ensureConnected();
        return this.orgs;
      }
    }

    public ServerEvents ServerEvents
    {
      get
      {
        this.ensureConnected();
        return this.serverEvents;
      }
    }

    public DataExchange DataExchange
    {
      get
      {
        this.ensureConnected();
        return this.dataExchange;
      }
    }

    public Reports Reports
    {
      get
      {
        this.ensureConnected();
        return this.reports;
      }
    }

    public SystemSettings SystemSettings
    {
      get
      {
        this.ensureConnected();
        return this.settings;
      }
    }

    public CorrespondentMasterService CorrespondentMaster
    {
      get
      {
        this.ensureConnected();
        return this.corresondentMaster;
      }
    }

    public bool IsConnected => this.conn != null;

    public CorrespondentTradeService CorrespondentTradeService
    {
      get
      {
        this.ensureConnected();
        return this.correspondentTradeService;
      }
    }

    public GSECommitmentService GSECommitmentService
    {
      get
      {
        this.ensureConnected();
        return this.gseCommitmentService;
      }
    }

    public LockDeskSettingsService LockDeskSettingsService
    {
      get
      {
        this.ensureConnected();
        return this.lockDeskSettingsService;
      }
    }

    public LoanTradeService LoanTradeService
    {
      get
      {
        this.ensureConnected();
        return this.loanTradeService;
      }
    }

    public SecurityTradeService SecurityTradeService
    {
      get
      {
        this.ensureConnected();
        return this.securityService;
      }
    }

    public MbsPoolService MbsPoolService
    {
      get
      {
        this.ensureConnected();
        return this.mbsPoolService;
      }
    }

    public MasterContractService MasterContractService
    {
      get
      {
        this.ensureConnected();
        return this.masterContractService;
      }
    }

    public TradeBatchService TradeBatchService
    {
      get
      {
        this.ensureConnected();
        return this.tradeBatchService;
      }
    }

    public SettingsService SettingsService
    {
      get
      {
        this.ensureConnected();
        return this.settingsService;
      }
    }

    public User GetCurrentUser()
    {
      this.ensureConnected();
      return this.Users.GetUser(this.UserID);
    }

    public void ImpersonateUser(string userId)
    {
      this.ensureConnected();
      this.sessionObjects.Session.ImpersonateUser(userId);
      this.sessionObjects.InvalidateUserInfo();
    }

    public void RestoreIdentity()
    {
      this.ensureConnected();
      this.sessionObjects.Session.RestoreIdentity();
      this.sessionObjects.InvalidateUserInfo();
    }

    public static string EncompassProgramDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
    }

    public static string EncompassDataDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassDataDirectory;
    }

    public static string EpassDataDirectory => SystemSettings.EpassDataDir;

    private void ensureConnected()
    {
      if (this.conn == null)
        throw new InvalidOperationException("Session is not connected.");
    }

    private void ensureNotDisposed()
    {
      if (this.disposed)
        throw new ObjectDisposedException(nameof (Session), "The object has been disposed");
    }

    internal object GetObject(string name)
    {
      lock (this)
      {
        this.ensureConnected();
        return this.sessionObjects[name];
      }
    }

    internal object GetAclManager(AclCategory category)
    {
      lock (this)
      {
        this.ensureConnected();
        return this.sessionObjects.GetAclManager(category);
      }
    }

    internal SessionObjects SessionObjects => this.sessionObjects;

    internal object GetSystemSettings(Type settingsType)
    {
      BinaryObject systemSettings = ((IConfigurationManager) this.GetObject("ConfigurationManager")).GetSystemSettings(settingsType.Name);
      return systemSettings == null ? settingsType.GetConstructor(Type.EmptyTypes).Invoke((object[]) null) : new XmlSerializer().Deserialize(systemSettings.OpenStream(), settingsType);
    }

    internal UserInfo GetUserInfo() => this.sessionObjects.UserInfo;

    private void onServerEvent(IConnection sender, ServerEvent e)
    {
      switch (e)
      {
        case MessageEvent _:
          this.onMessageArrived(((MessageEvent) e).Message);
          break;
        case DataExchangeEvent _:
          this.dataExchange.OnDataExchangeEvent((DataExchangeEvent) e);
          break;
        case DisconnectEvent _ when !this.isEncompassGuiSession:
          this.closeSession();
          this.onDisconnected(DisconnectReason.TerminatedByServer);
          break;
        case ServerMonitorEvent _:
          this.serverEvents.RaiseEvent((ServerMonitorEvent) e);
          break;
      }
    }

    private void onConnectionError(IConnection sender, ConnectionErrorType errType)
    {
      if (this.isEncompassGuiSession)
        return;
      this.closeSession();
      this.onDisconnected(DisconnectReason.ConnectionError);
    }

    private void onMessageArrived(Message msg)
    {
      if (this.messageArrived.IsNull())
        return;
      using (Tracing.StartTimer(Session.sw, nameof (Session), TraceLevel.Verbose, "Session.MessageArrived event"))
      {
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.MessageArrived event Type Passed: 1) " + this.GetType().ToString() + " 2) Message: " + msg.Text);
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.MessageArrived event subscribed Type : " + this.messageArrived.Method.DeclaringType.ToString());
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.MessageArrived event subscribed Method : " + this.messageArrived.Method.ToString());
        this.messageArrived((object) this, new ServerMessageEventArgs(msg));
      }
    }

    private void onDisconnected(DisconnectReason reason)
    {
      this.conn = (IConnection) null;
      if (this.disconnected.IsNull())
        return;
      using (Tracing.StartTimer(Session.sw, nameof (Session), TraceLevel.Verbose, "Session.Disconnected event"))
      {
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.Disconnected event Type Passed: 1)" + this.GetType().ToString() + " 2) DisconnectReason: " + reason.ToString());
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.Disconnected event subscribed Type : " + this.disconnected.Method.DeclaringType.ToString());
        Tracing.Log(Session.sw, nameof (Session), TraceLevel.Info, "Session.Disconnected event subscribed Method : " + this.disconnected.Method.ToString());
        this.disconnected((object) this, new DisconnectedEventArgs(reason));
      }
    }

    private void initializeSession(
      IConnection c,
      ISessionStartupInfo startupInfo,
      string userPassword,
      string serverUri)
    {
      if (startupInfo == null)
      {
        startupInfo = c.Session.GetSessionStartupInfo();
        try
        {
          new LicenseManager().AuthorizeSession(startupInfo);
        }
        catch
        {
          c.Close();
          throw;
        }
      }
      this.conn = c;
      this.serverUri = serverUri;
      this.sessionObjects = new SessionObjects(c.Session, userPassword, startupInfo);
      this.loans = new EllieMae.Encompass.BusinessObjects.Loans.Loans(this);
      this.contacts = new EllieMae.Encompass.BusinessObjects.Contacts.Contacts(this);
      this.users = new EllieMae.Encompass.BusinessObjects.Users.Users(this);
      this.orgs = new Organizations(this);
      this.calendar = new EllieMae.Encompass.BusinessObjects.Calendar.Calendar(this);
      this.serverEvents = new ServerEvents(this);
      this.dataExchange = new DataExchange(this);
      this.reports = new Reports(this);
      this.settings = new SystemSettings(this);
      this.correspondentTradeService = new CorrespondentTradeService(this);
      this.corresondentMaster = new CorrespondentMasterService(this);
      this.gseCommitmentService = new GSECommitmentService(this);
      this.lockDeskSettingsService = new LockDeskSettingsService(this);
      this.loanTradeService = new LoanTradeService(this);
      this.securityService = new SecurityTradeService(this);
      this.mbsPoolService = new MbsPoolService(this);
      this.masterContractService = new MasterContractService(this);
      this.tradeBatchService = new TradeBatchService(this);
      this.settingsService = new SettingsService(this);
      this.oapiGatewayBaseUri = startupInfo.OAPIGatewayBaseUri;
      this.instanceName = startupInfo.ServerInstanceName;
      if (this.conn is EllieMae.EMLite.Client.Connection || this.conn is InProcConnection)
      {
        this.conn.ServerEvent += new ServerEventHandler(this.onServerEvent);
        this.conn.ConnectionError += new ConnectionErrorEventHandler(this.onConnectionError);
      }
      if (this.sessionObjects.StartupInfo.AllowMileStoneAdjustDateLog)
      {
        // ISSUE: method pointer
        Tracing.InitSendToServerDelegate(new Tracing.SendErrorToServerType((object) null, __methodptr(Write)), "MileStoneFinishedDateLog");
      }
      try
      {
        lock (Session.sessions)
        {
          if (!Session.sessions.ContainsKey(this.ID))
            Session.sessions.Add(this.ID, this.ServerSessionID);
          if (Session.serverLogListenerOwner != null)
            return;
          DiagUtility.LoggerScopeProvider?.ModifyGlobal((Action<ILoggerScopeBuilder>) (globalScope => globalScope.SetInstance(this.instanceName)));
          DiagConfig<ClientDiagConfigData>.Instance.SetGlobalData<string>("SessionId", this.ServerSessionID);
          DiagConfig<ClientDiagConfigData>.Instance.SetGlobalData<string>("UserId", this.UserID);
          DiagConfig<ClientDiagConfigData>.Instance.ReloadConfig();
          Session.configChangeHandler = new RemotingLogConfigChangeHandler((IRemotingLogConsumer) this.sessionObjects.ServerManager);
          DiagConfig<ClientDiagConfigData>.Instance.AddHandler((IDiagConfigChangeHandler<ClientDiagConfigData>) Session.configChangeHandler);
          Session.serverLogListenerOwner = this.ID;
        }
      }
      catch
      {
      }
    }

    private void closeSession()
    {
      this.loans = (EllieMae.Encompass.BusinessObjects.Loans.Loans) null;
      this.contacts = (EllieMae.Encompass.BusinessObjects.Contacts.Contacts) null;
      this.sessionObjects = (SessionObjects) null;
      try
      {
        if (this.conn is EllieMae.EMLite.Client.Connection)
        {
          ((ConnectionBase) this.conn).ServerEvent -= new ServerEventHandler(this.onServerEvent);
          ((ConnectionBase) this.conn).ConnectionError -= new ConnectionErrorEventHandler(this.onConnectionError);
        }
        this.conn.Close();
      }
      catch
      {
      }
      this.conn = (IConnection) null;
      this.serverUri = (string) null;
    }

    internal ISession Unwrap()
    {
      this.ensureConnected();
      return this.conn.Session;
    }

    internal IConnection Connection => this.conn;

    public static Session Wrap(
      IConnection conn,
      ISessionStartupInfo startupInfo,
      string userPassword,
      string serverUri)
    {
      return new Session(conn, startupInfo, userPassword, serverUri);
    }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      return string.Equals(new AssemblyName(args.Name).Name, "System.Runtime.CompilerServices.Unsafe", StringComparison.OrdinalIgnoreCase) ? typeof (Unsafe).Assembly : (Assembly) null;
    }
  }
}
