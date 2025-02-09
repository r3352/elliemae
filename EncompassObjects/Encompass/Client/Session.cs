// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Client.Session
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using Elli.Metrics.Client;
using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.Diagnostics;
using EllieMae.EMLite.Common.Diagnostics.ConfigChangeHandlers;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using EllieMae.Encompass.BusinessObjects;
using EllieMae.Encompass.BusinessObjects.LockDeskSettings;
using EllieMae.Encompass.BusinessObjects.Settings;
using EllieMae.Encompass.BusinessObjects.TradeManagement;
using EllieMae.Encompass.BusinessObjects.Users;
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
  /// <summary>
  /// Represents a single client connection to an Encompass Server or to an Offline
  /// Encompass database.
  /// </summary>
  /// <remarks>The Session object represents the entry point for an Encompass
  /// client session. The client application uses this object to establish a login session
  /// with the server and then to subsequently access object data from the
  /// server.
  /// <p>Session instances, as well as instances of any other class within the EncompassObjects
  /// API, are not thread-safe. If you wish to use the objects from multiple threads,
  /// you will need to protect the objects to safeguard against concurrent access.</p>
  /// </remarks>
  /// <example>
  /// The following code opens a session to a remote Encompass Server using a TCP/IP
  /// connection on port 11081. It then opens a loan using the specified GUID value.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Loans;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Fetch a loan from the session
  ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
  /// 
  ///       if (loan == null)
  ///          Console.WriteLine("Loan not found");
  ///       else
  ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
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
    private EllieMae.Encompass.Configuration.SystemSettings settings;
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

    /// <summary>
    /// An event which is raised when the session is disconnected from the server.
    /// </summary>
    /// <example>
    /// The following code opens a session to a remote Encompass server and attaches an
    /// event handler to the Disconnected event. When the client session is disconnected
    /// from the server, whether due to a connection error, an administrator forcibly
    /// terminating the session or the Session.End() method being called, the event gets
    /// fired.
    /// <code>
    ///  <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("remoteserver", "mary", "maryspwd");
    /// 
    ///       // Add an event handler to listen for asynchronous messages
    ///       session.Disconnected += new DisconnectedEventHandler(sessionDisconnected);
    /// 
    ///       // Wait indefinitely
    ///       Console.ReadLine();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    /// 
    ///       // Wait for a second newline
    ///       Console.ReadLine();
    ///    }
    /// 
    ///    // Event handler for the Disconnected session event
    ///    private static void sessionDisconnected(object sender, DisconnectedEventArgs e)
    ///    {
    ///       Console.WriteLine("The session has been disconnected");
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public event DisconnectedEventHandler Disconnected
    {
      add
      {
        if (value == null)
          return;
        this.disconnected.Add(new ScopedEventHandler<DisconnectedEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.disconnected.Remove(new ScopedEventHandler<DisconnectedEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    /// <summary>
    /// An event which is raised when a message arrives from the server for the current
    /// session.
    /// </summary>
    /// <example>
    /// The following code opens a session to a remote Encompass server and attaches an
    /// event handler to the MessageArrived event. When an event occurs, a message is
    /// written to the console.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("remoteserver", "mary", "maryspwd");
    /// 
    ///       // Add an event handler to listen for asynchronous messages
    ///       session.MessageArrived += new ServerMessageEventHandler(sessionMessageArrived);
    /// 
    ///       // Wait indefinitely
    ///       Console.ReadLine();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Event handler for the MessageArrived event
    ///    private static void sessionMessageArrived(object sender, ServerMessageEventArgs e)
    ///    {
    ///       Console.WriteLine("Message arrived: " + e.Text);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public event ServerMessageEventHandler MessageArrived
    {
      add
      {
        if (value == null)
          return;
        this.messageArrived.Add(new ScopedEventHandler<ServerMessageEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.messageArrived.Remove(new ScopedEventHandler<ServerMessageEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    static Session()
    {
      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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

    /// <summary>Constructs a new, disconnected session.</summary>
    public Session()
    {
      this.InitEvents();
      this.InitAppName();
      if (!new LicenseManager().ValidateLicense(true))
        throw new EllieMae.Encompass.Licensing.LicenseException("This machine is not licensed to use the Encompass API.");
    }

    private void InitEvents()
    {
      this.disconnected = new ScopedEventHandler<DisconnectedEventArgs>(nameof (Session), "Disconnected");
      this.messageArrived = new ScopedEventHandler<ServerMessageEventArgs>(nameof (Session), "MessageArrived");
    }

    /// <summary>
    /// Constructs a session from an existing Connection object
    /// </summary>
    /// <param name="conn">The underlying connection to wrap this Session around.</param>
    /// <param name="startupInfo">The session starup information.</param>
    /// <param name="userPassword">The password for the session.</param>
    /// <param name="serverUri">The Encompass Server URI.</param>
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

    /// <summary>Starts a session with a remote Encompass Server.</summary>
    /// <param name="serverUri">The URI of the remote server. This URI should include the
    /// protocol to use (tcp or http), the hostname/port number of the remote server and,
    /// in the case of an HTTP server, the virtual root at which the server resides.</param>
    /// <param name="userId">The Encompass login ID for the current user.</param>
    /// <param name="password">The password for the current user.</param>
    /// <remarks>
    /// When starting a remote session, care should be taken to catch the VersionException
    /// if it is thrown. This exception indicates that the Encompass versions on the client
    /// and server are incompatible. The client or server will need to be updated with the
    /// correct software version in order to log in again.
    /// </remarks>
    /// <exception cref="T:EllieMae.Encompass.Client.ConnectionException">
    /// Indicates that the client was unable to establish a connection to the remote
    /// Encompass Server.
    /// </exception>
    /// <exception cref="T:EllieMae.Encompass.Client.VersionException">
    /// Indicates that Encompass API on the client (local) computer is incompatible with
    /// the version of the Encompass Server.
    /// </exception>
    /// <exception cref="T:EllieMae.Encompass.Client.LoginException">
    /// Indicates that the server was unable to satisfy the login request. This may be due to
    /// an invalid user ID or password, the user's account being disabled, etc.
    /// </exception>
    /// <example>
    /// The following code opens a session to a remote Encompass Server using a TCP/IP
    /// connection on port 11081. It then opens a loan using the specified GUID value.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session
    ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Start(string serverUri, string userId, string password)
    {
      this.ensureNotDisposed();
      if (this.conn != null)
        throw new InvalidOperationException("Session has already been started");
      if (Session.externalApplication)
        APICallContext.SetAsCurrent((IApiSourceContext) new APICallContext(Session.externalModuleName, Session.externalAssemblyName, APICallSourceType.Application, (string) null));
      try
      {
        DateTime now = DateTime.Now;
        EllieMae.EMLite.Client.Connection c = new EllieMae.EMLite.Client.Connection();
        c.Open(serverUri, (userId ?? "").ToLower(), password ?? "", this.appName);
        this.initializeSession((IConnection) c, (ISessionStartupInfo) null, password, serverUri);
        TimeSpan time = DateTime.Now - now;
        ClientMetricsProviderFactory.Initiate(this.ClientID, c.Server.InstanceName, this.UserID, string.Format("{0}(V{1})", (object) this.asmName, (object) this.asmVersion));
        ClientMetricsProviderFactory.IncrementCounter("LogonIncCounter", (SFxTag) new SFxSdkTag());
        ClientMetricsProviderFactory.RecordIncrementalTimerSample("LogonIncTimer", time, (SFxTag) new SFxSdkTag());
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LoginException ex)
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

    /// <summary>
    /// Starts an offline session with the local Encompass database.
    /// </summary>
    /// <param name="userId">The Encompass login ID for the current user.</param>
    /// <param name="password">The password for the current user.</param>
    /// <exception cref="T:EllieMae.Encompass.Client.LoginException">
    /// Indicates that the server was unable to satisfy the login request. This may be due to
    /// an invalid user ID or password, the user's account being disabled, etc.
    /// </exception>
    /// <example>
    /// The following code opens a session to the offline Encompass database.
    /// It then opens a loan using the specified GUID value.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.StartOffline("mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session
    ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void StartOffline(string userId, string password)
    {
      this.StartInstance(userId, password, (string) null);
    }

    /// <summary>
    /// Starts an Encompass Session in a named system instance.
    /// </summary>
    /// <param name="userId">The Encompass login ID for the current user.</param>
    /// <param name="password">The password for the current user.</param>
    /// <param name="instanceName">The name of the system instance.</param>
    /// <remarks>This method is intended for internal use by Encompass only and should not
    /// be invoked directly from user code.</remarks>
    public void StartInstance(string userId, string password, string instanceName)
    {
      this.ensureNotDisposed();
      if (this.conn != null)
        throw new InvalidOperationException("Session has already been started");
      if (Session.externalApplication)
        APICallContext.SetAsCurrent((IApiSourceContext) new APICallContext(Session.externalModuleName, Session.externalAssemblyName, APICallSourceType.Application, (string) null));
      if (EnConfigurationSettings.GlobalSettings.InstallationMode == InstallationMode.Client)
        throw new InvalidOperationException("Encompass client installations cannot be used to start an offline session.");
      try
      {
        if (instanceName == null)
          instanceName = EnConfigurationSettings.InstanceName;
        InProcConnection c = new InProcConnection();
        c.OpenInProcess((userId ?? "").ToLower(), password ?? "", instanceName, this.appName, true);
        this.initializeSession((IConnection) c, (ISessionStartupInfo) null, password, (string) null);
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LoginException ex)
      {
        throw new LoginException(ex);
      }
    }

    /// <summary>Disposes of the current session.</summary>
    /// <remarks>Any subsequent calls on this Session object or any objects
    /// retrieved from this Session will fail after calling the Dispose() method.</remarks>
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

    /// <summary>Ends the current session.</summary>
    /// <remarks>Any subsequent calls on this Session object or any objects
    /// retrieved from this Session will fail after calling the End() method.
    /// A session cannot be reused, thus any subsequent attempt to re-start
    /// the session will result in an exception.</remarks>
    /// <example>
    /// The following code opens a session to a remote Encompass Server using a TCP/IP
    /// connection on port 11081. It then opens a loan using the specified GUID value.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session
    ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
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
            EllieMae.EMLite.RemotingServices.SystemSettings.DeleteTempFolderFiles();
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

    /// <summary>Gets the Encompass login ID for the current session.</summary>
    public string UserID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.UserInfo.Userid;
      }
    }

    /// <summary>
    /// Gets the ClientID from the server to which the connection is made.
    /// </summary>
    public string ClientID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.ServerLicense.ClientID;
      }
    }

    /// <summary>
    /// Returns a unique identifier for the Encompass system to which the session
    /// is connected.
    /// </summary>
    /// <remarks>The system identifier is created when Encompass is installed
    /// and remains the same for all time (unless physically re-created). Additionally,
    /// in environment that use multiple Encompass servers, this identifier will
    /// be the same regardless of the server to which you connect.</remarks>
    public string SystemID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.SystemInformation.SystemID;
      }
    }

    /// <summary>
    /// Returns the unique identifier for the Encompass Server instance.
    /// </summary>
    public string ServerID
    {
      get
      {
        this.ensureConnected();
        return this.sessionObjects.ServerID;
      }
    }

    /// <summary>
    /// Gets the server name and protocol information for the current session.
    /// </summary>
    /// <remarks>If the session is started with a call to StartOffline(),
    /// this property will return <c>null</c>.</remarks>
    public string ServerURI => this.serverUri;

    /// <summary>
    /// Gets a unique session identifier (GUID) for the current session.
    /// </summary>
    /// <remarks>This value is guaranteed to be unique across all time
    /// and all Encompass Server instances.</remarks>
    public string ID => this.guid;

    /// <summary>Gets the unique for the current server session.</summary>
    public string ServerSessionID => this.sessionObjects.SessionID;

    /// <summary>OAuth Gateway base uri</summary>
    public string OAPIGatewayBaseUri => this.oapiGatewayBaseUri;

    /// <summary>
    /// allow user to get auth token.
    /// The return type is a string, can be split by one space.
    /// After split, the first part is scheme, the second part is auth token.
    /// </summary>
    public string GetAuthToken()
    {
      try
      {
        return new OAuth2(this.oapiGatewayBaseUri).GetAccessToken(ServerIdentity.Parse(this.serverUri).InstanceName, this.sessionObjects.SessionID, "sc", true).TypeAndToken;
      }
      catch
      {
        return (string) null;
      }
    }

    /// <summary>
    /// Allows the caller to determine which edition of the Encompass software is installed
    /// on the local computer.
    /// </summary>
    public EncompassEdition EncompassEdition
    {
      get
      {
        this.ensureConnected();
        if (this.sessionObjects.ServerLicense.Edition == EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker)
          return EncompassEdition.Broker;
        return this.sessionObjects.ServerLicense.Edition == EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker ? EncompassEdition.Banker : EncompassEdition.Unknown;
      }
    }

    /// <summary>Returns the current time using the server's timezone.</summary>
    public DateTime GetServerTime() => this.sessionObjects.Session.ServerTime;

    /// <summary>
    /// Provides access to the loan-related operations provided by the connected
    /// server.
    /// </summary>
    /// <example>
    /// The following code opens a session to a remote Encompass Server using a TCP/IP
    /// connection on port 11081. It then opens a loan using the specified GUID value.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch a loan from the session
    ///       Loan loan = session.Loans.Open("{9885d88a-78af-44a7-977d-5d5fd6e41a96}");
    /// 
    ///       if (loan == null)
    ///          Console.WriteLine("Loan not found");
    ///       else
    ///          Console.WriteLine("Successfully opened loan " + loan.LoanNumber);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public EllieMae.Encompass.BusinessObjects.Loans.Loans Loans
    {
      get
      {
        this.ensureConnected();
        return this.loans;
      }
    }

    /// <summary>Provides access to the current user's Calendar.</summary>
    /// <include file="Session.xml" path="Examples/Example[@name=&quot;Session.Calendar&quot;]/*" />
    public EllieMae.Encompass.BusinessObjects.Calendar.Calendar Calendar
    {
      get
      {
        this.ensureConnected();
        return this.calendar;
      }
    }

    /// <summary>
    /// Provides access to the contacts-related operations provided by the connected
    /// server.
    /// </summary>
    /// <example>
    /// The following code opens a session to a remote Encompass server and prints out
    /// a list of all of the Business Contacts defined on the server.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("remoteserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve the list of Business Contacts
    ///       ContactList contacts = session.Contacts.GetAll(ContactType.Biz);
    /// 
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          Console.WriteLine(contacts[i].FirstName + " " + contacts[i].LastName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public EllieMae.Encompass.BusinessObjects.Contacts.Contacts Contacts
    {
      get
      {
        this.ensureConnected();
        return this.contacts;
      }
    }

    /// <summary>
    /// Provides access to the user database on the Encompass server.
    /// </summary>
    /// <example>
    /// The following code opens a session to a remote Encompass server and prints out
    /// all of the users defined on the server.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("remoteserver", "mary", "maryspwd");
    /// 
    ///       // Retrieve the list of Business Contacts
    ///       UserList users = session.Users.GetAllUsers();
    /// 
    ///       for (int i = 0; i < users.Count; i++)
    ///          Console.WriteLine(users[i].FirstName + " " + users[i].LastName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public EllieMae.Encompass.BusinessObjects.Users.Users Users
    {
      get
      {
        this.ensureConnected();
        return this.users;
      }
    }

    /// <summary>
    /// Provides access to the organization hierarchy on the Encompass server.
    /// </summary>
    /// <include file="Session.xml" path="Examples/Example[@name=&quot;Session.Organizations&quot;]/*" />
    public Organizations Organizations
    {
      get
      {
        this.ensureConnected();
        return this.orgs;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.ServerEvents" /> object, allowing the session to subcribe to events
    /// from the Encompass server.
    /// </summary>
    public ServerEvents ServerEvents
    {
      get
      {
        this.ensureConnected();
        return this.serverEvents;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.DataExchange" /> object, allowing the session to pass data to another
    /// Encompass session on the same server.
    /// </summary>
    public DataExchange DataExchange
    {
      get
      {
        this.ensureConnected();
        return this.dataExchange;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.Reports" /> object, which provide access to efficient reporting functions.
    /// </summary>
    public Reports Reports
    {
      get
      {
        this.ensureConnected();
        return this.reports;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.Reports" /> object, which provide access to efficient reporting functions.
    /// </summary>
    public EllieMae.Encompass.Configuration.SystemSettings SystemSettings
    {
      get
      {
        this.ensureConnected();
        return this.settings;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.CorrespondentMaster" /> object, which provide access to Correspondent Master objects.
    /// </summary>
    public CorrespondentMasterService CorrespondentMaster
    {
      get
      {
        this.ensureConnected();
        return this.corresondentMaster;
      }
    }

    /// <summary>
    /// Gets a boolean indicating if the session is currently connected either to a remote
    /// Encompass Server or to the local, offline database.
    /// </summary>
    public bool IsConnected => this.conn != null;

    /// <summary>
    /// Provides access to the organization hierarchy on the Encompass server.
    /// </summary>
    public CorrespondentTradeService CorrespondentTradeService
    {
      get
      {
        this.ensureConnected();
        return this.correspondentTradeService;
      }
    }

    /// <summary>
    /// Provides access to the organization hierarchy on the Encompass server.
    /// </summary>
    public GSECommitmentService GSECommitmentService
    {
      get
      {
        this.ensureConnected();
        return this.gseCommitmentService;
      }
    }

    /// <summary>
    /// Provides access to the organization hierarchy on the Encompass server.
    /// </summary>
    public LockDeskSettingsService LockDeskSettingsService
    {
      get
      {
        this.ensureConnected();
        return this.lockDeskSettingsService;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade" /> object, which provide access to Loan Trade objects.
    /// </summary>
    public LoanTradeService LoanTradeService
    {
      get
      {
        this.ensureConnected();
        return this.loanTradeService;
      }
    }

    /// <summary>
    /// Gets the <see cref="P:EllieMae.Encompass.Client.Session.SecurityTradeService" /> object, which provide access to Securty Trade objects.
    /// </summary>
    public SecurityTradeService SecurityTradeService
    {
      get
      {
        this.ensureConnected();
        return this.securityService;
      }
    }

    /// <summary>MbsPoolService</summary>
    public MbsPoolService MbsPoolService
    {
      get
      {
        this.ensureConnected();
        return this.mbsPoolService;
      }
    }

    /// <summary>MasterContractService</summary>
    public MasterContractService MasterContractService
    {
      get
      {
        this.ensureConnected();
        return this.masterContractService;
      }
    }

    /// <summary>TradeBatchService</summary>
    public TradeBatchService TradeBatchService
    {
      get
      {
        this.ensureConnected();
        return this.tradeBatchService;
      }
    }

    /// <summary>
    /// Provides access to the organization hierarchy on the Encompass server.
    /// </summary>
    public SettingsService SettingsService
    {
      get
      {
        this.ensureConnected();
        return this.settingsService;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> who is currently logged in.
    /// </summary>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User" /> representing the logged in user.</returns>
    public User GetCurrentUser()
    {
      this.ensureConnected();
      return this.Users.GetUser(this.UserID);
    }

    /// <summary>
    /// Modifies the session to impersonate a different User account.
    /// </summary>
    /// <param name="userId">The UserID of the user to impersonate.</param>
    /// <remarks>The session must be started by a "root administrator"
    /// in order to perform impersonation. Use the RestoreIdentity() method to return the
    /// effective User ID of the session to the desired value.</remarks>
    public void ImpersonateUser(string userId)
    {
      this.ensureConnected();
      this.sessionObjects.Session.ImpersonateUser(userId);
      this.sessionObjects.InvalidateUserInfo();
    }

    /// <summary>
    /// Restores the identity of the session's logged in user after a call to
    /// <see cref="M:EllieMae.Encompass.Client.Session.ImpersonateUser(System.String)" />.
    /// </summary>
    public void RestoreIdentity()
    {
      this.ensureConnected();
      this.sessionObjects.Session.RestoreIdentity();
      this.sessionObjects.InvalidateUserInfo();
    }

    /// <summary>
    /// Provides the directory in which the Encompass program files are stored.
    /// </summary>
    public static string EncompassProgramDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassProgramDirectory;
    }

    /// <summary>
    /// Provides the directory in which the local EncompassData files are stored.
    /// </summary>
    public static string EncompassDataDirectory
    {
      get => EnConfigurationSettings.GlobalSettings.EncompassDataDirectory;
    }

    /// <summary>
    /// Provides the directory in which the Encompass program files are stored.
    /// </summary>
    public static string EpassDataDirectory => EllieMae.EMLite.RemotingServices.SystemSettings.EpassDataDir;

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
        this.messageArrived.Invoke((object) this, new ServerMessageEventArgs(msg));
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
        this.disconnected.Invoke((object) this, new DisconnectedEventArgs(reason));
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
      this.settings = new EllieMae.Encompass.Configuration.SystemSettings(this);
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
        Tracing.InitSendToServerDelegate(new Tracing.SendErrorToServerType(RemoteLogger.Write), "MileStoneFinishedDateLog");
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

    internal EllieMae.EMLite.ClientServer.ISession Unwrap()
    {
      this.ensureConnected();
      return this.conn.Session;
    }

    internal IConnection Connection => this.conn;

    /// <summary>
    /// This method is for internal Encompass use only and should not be called from
    /// your code.
    /// </summary>
    /// <exclude />
    public static Session Wrap(
      IConnection conn,
      ISessionStartupInfo startupInfo,
      string userPassword,
      string serverUri)
    {
      return new Session(conn, startupInfo, userPassword, serverUri);
    }

    /// <summary>
    /// NICE-40504: Unity.Interception nuget package references an older version of System.Runtime.CompilerServices.Unsafe
    /// Since we have updated System.Runtime.CompilerServices.Unsafe to a newer version in EMLite solution
    /// existing SDK Applications may run into a runtime exception if assembly binding redirect
    /// hasn't been added to app.config.
    /// To resolve this issue we will force the assembly resolution to System.Runtime.CompilerServices.Unsafe.dll
    /// published with the SDK.
    /// If app.config contains the required binding redirect, the application uses the version designated
    /// by the app.config and CurrentDomain.AssemblyResolve event won't be invoked for System.Runtime.CompilerServices.Unsafe.dll
    /// </summary>
    /// <param name="sender">event sender</param>
    /// <param name="args">event arguments</param>
    /// <returns>Resolved assembly or null</returns>
    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
      return string.Equals(new AssemblyName(args.Name).Name, "System.Runtime.CompilerServices.Unsafe", StringComparison.OrdinalIgnoreCase) ? typeof (Unsafe).Assembly : (Assembly) null;
    }
  }
}
