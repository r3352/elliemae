// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.Session
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.Common.Security;
using Elli.Server.Remoting.Acl;
using Elli.Server.Remoting.DataSync;
using Elli.Server.Remoting.Interception;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Events;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Loan;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Acl;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class Session : ClientSession, ISession, IClientSession, IClientCacheSettings
  {
    private const string className = "Session";
    private string effectiveUserId;
    private Hashtable sessionObjects = new Hashtable();
    private Elli.Server.Remoting.SessionObjects.SecurityManager securityManager;
    private ICurrentUser currentUser;
    private IClientContext context;
    private string systemId;
    private bool loanImportInProgress;
    private SessionDiagnostics diagnostics;
    private int loginPwdLength;
    private string appName;
    private bool disconnected;
    private int _multiServer = -1;

    private int multiServer
    {
      get
      {
        if (this._multiServer < 0)
          this._multiServer = (int) this.context.Settings.GetServerSetting("Internal.MultiServer");
        return this._multiServer;
      }
    }

    public bool IMEnabled => (this.multiServer & 1) == 1;

    public bool CSEnabled => (this.multiServer & 2) == 2;

    public bool PersistentClientCacheEnabled { get; }

    public Session(
      LoginParameters loginParams,
      ClientContext context,
      IServerCallback callback,
      string SessionID)
      : base(loginParams, SessionID, callback)
    {
      this.context = (IClientContext) context;
      this.effectiveUserId = loginParams.UserID;
      this.diagnostics = new SessionDiagnostics(context.InstanceName, SessionID, this.effectiveUserId, this.LoginParams.AppName);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - Diagnostics", 90, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      if (loginParams.LicenseRequired)
        LicenseManager.AcquireLicense(loginParams, this.context);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - AcquireLicense", 96, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      InterceptionUtils.NewInstance<LoanManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ReportManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ConfigurationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ContactManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ContactGroupManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<AlertManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CalendarManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<WelcomeScreenSettingMgr>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CampaignManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<FormManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<PersonaManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<BpmManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<AclGroupManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MasterContractManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<SecurityTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MbsPoolManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<EncERDBRegMgr>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CorrespondentMasterManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CorrespondentTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LockComparisonFieldManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanSummaryExtensionManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<OverNightRateProtectionManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<IdentityManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<GseCommitmentManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeLoanUpdateQueueManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<BatchJobsManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeSynchronizationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeLoanEligibilityManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<SpecialFeatureCodeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanExternalFieldManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ServerManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MessengerListManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<OrganizationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<EfolderDocTrackViewManager>().Initialize((ISession) this);
      this.currentUser = (ICurrentUser) InterceptionUtils.NewInstance<CurrentUser>().Initialize((ISession) this);
      this.securityManager = InterceptionUtils.NewInstance<Elli.Server.Remoting.SessionObjects.SecurityManager>().Initialize((ISession) this);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - Managers", 141, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      try
      {
        this.context.Sessions.UpdateSession((IClientSession) this, (IConnectionManager) new ConnectionManagerNoop());
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (Session), "Error updating Session for MFA for SessionID :" + SessionID + " " + ex.Message);
        this.context.Sessions.DeleteSessionFromDBOnly(SessionID);
        Err.Raise(nameof (Session), new ServerException(string.Format("UpdateSession Failed during MFA Login Flow for SessionID : " + SessionID)));
      }
      if (this.appName != "<EncompassServer>" && loginParams.AppName != "trustedEnc")
      {
        this.RaiseEvent((ServerEvent) new ConnectEvent(this.SessionInfo, ConnectionManager.SecurityContext));
        EncompassServer.RaiseEvent(this.context, (ServerEvent) new SessionEvent(SessionEventType.Login, this.SessionInfo));
        if (this.IMEnabled)
          this.context.Sessions.RaiseServerEventOnRemoveServers((Message) new SessionEventMessage(SessionEventType.Login, this.SessionInfo));
      }
      this.PersistentClientCacheEnabled = PersistentClientCacheFeature.IsEnabled(this.context);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - RaiseEvent", 173, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
    }

    public Session(LoginParameters loginParams, ClientContext context, IServerCallback callback)
      : base(loginParams, Guid.NewGuid().ToString(), callback)
    {
      this.context = (IClientContext) context;
      this.effectiveUserId = loginParams.UserID;
      this.diagnostics = new SessionDiagnostics(context.InstanceName, this.SessionID, this.effectiveUserId, this.LoginParams.AppName);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - Diagnostics", 185, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      if (loginParams.LicenseRequired)
        LicenseManager.AcquireLicense(loginParams, this.context);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - AcquireLicense", 191, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      InterceptionUtils.NewInstance<LoanManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ReportManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ConfigurationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ContactManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ContactGroupManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<AlertManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CalendarManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<WelcomeScreenSettingMgr>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CampaignManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<FormManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<PersonaManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<BpmManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<AclGroupManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MasterContractManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<SecurityTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MbsPoolManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<EncERDBRegMgr>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CorrespondentMasterManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<CorrespondentTradeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LockComparisonFieldManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanSummaryExtensionManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<OverNightRateProtectionManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<IdentityManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<GseCommitmentManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeLoanUpdateQueueManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<BatchJobsManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeSynchronizationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<TradeLoanEligibilityManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<SpecialFeatureCodeManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<LoanExternalFieldManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<ServerManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<MessengerListManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<OrganizationManager>().Initialize((ISession) this);
      InterceptionUtils.NewInstance<EfolderDocTrackViewManager>().Initialize((ISession) this);
      this.currentUser = (ICurrentUser) InterceptionUtils.NewInstance<CurrentUser>().Initialize((ISession) this);
      this.securityManager = InterceptionUtils.NewInstance<Elli.Server.Remoting.SessionObjects.SecurityManager>().Initialize((ISession) this);
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - Managers", 237, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      if (EncompassServer.ServerMode == EncompassServerMode.Service)
        return;
      if (loginParams.IsTrustedServiceApp)
        this.context.Sessions.AddS2SSession((IClientSession) this, (IConnectionManager) new ConnectionManagerNoop());
      else
        this.context.Sessions.AddSession((IClientSession) this, (IConnectionManager) new ConnectionManagerNoop());
      context.TraceLog.WriteInfo(nameof (Session), "Session " + this.ToString() + " started");
      PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - SessionManager", 250, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      this.loginPwdLength = loginParams.PwdLength;
      this.appName = loginParams.AppName;
      if (!loginParams.IsTrustedServiceApp && this.UserID != "(trusted)")
      {
        string ipAddress = loginParams.OfflineMode ? "localhost" : ConnectionManager.GetCurrentClientIPAddress().ToString();
        this.InsertAuditRecord((SystemAuditRecord) new UserLoginAuditRecord(this.UserID, this.GetUserInfo().FullName, ActionType.UserLogin, DateTime.Now, this.UserID, this.GetUserInfo().FullName, ipAddress, loginParams.Hostname));
        PerformanceMeter.Current.AddCheckpoint("Server.Login - Login Sessoin - AuditRecord", 267, ".ctor", "D:\\ws\\24.3.0.0\\EmLite\\Elli.Server.Remoting\\SessionObjects\\Session.cs");
      }
      if (!loginParams.IsTrustedServiceApp)
      {
        this.RaiseEvent((ServerEvent) new ConnectEvent(this.SessionInfo, ConnectionManager.SecurityContext));
        EncompassServer.RaiseEvent(this.context, (ServerEvent) new SessionEvent(SessionEventType.Login, this.SessionInfo));
        if (this.IMEnabled)
          this.context.Sessions.RaiseServerEventOnRemoveServers((Message) new SessionEventMessage(SessionEventType.Login, this.SessionInfo));
      }
      this.PersistentClientCacheEnabled = PersistentClientCacheFeature.IsEnabled(this.context);
    }

    public IClientContext Context => this.context;

    public SessionDiagnostics Diagnostics => this.diagnostics;

    public override string UserID => this.effectiveUserId;

    public string ServerID => EncompassServer.ServerID;

    public bool LoanImportInProgress
    {
      get => this.loanImportInProgress;
      set => this.loanImportInProgress = value;
    }

    public string SystemID
    {
      get
      {
        if (this.systemId == null)
          this.systemId = EncompassSystemDbAccessor.GetEncompassSystemInfo(this.Context).SystemID;
        return this.systemId;
      }
    }

    public string SqlDbID => this.Context.Settings.GetSqlDbID();

    public ISecurityManager SecurityManager => (ISecurityManager) this.securityManager;

    public ICurrentUser GetUser()
    {
      this.onApiCalled(nameof (GetUser));
      return this.currentUser;
    }

    public object GetObject(string objectName)
    {
      this.onApiCalled(nameof (GetObject), (object) objectName);
      return this.GetObjectInternal(objectName);
    }

    internal object GetObjectInternal(string objectName)
    {
      string lower = objectName.ToLower();
      object staticObject;
      if ((staticObject = EncompassServer.GetStaticObject(objectName)) != null)
        return staticObject;
      SessionBoundObject sessionObject;
      if ((sessionObject = (SessionBoundObject) this.sessionObjects[(object) lower]) != null)
        return (object) sessionObject;
      Err.Raise(nameof (Session), new ServerException("Invalid object name \"" + objectName + "\"", this.SessionInfo));
      return (object) null;
    }

    public object GetAclManager(AclCategory category)
    {
      this.onApiCalled(nameof (GetAclManager), (object) (int) category);
      return this.GetAclManagerInternal(category);
    }

    internal object GetAclManagerInternal(AclCategory category)
    {
      string key = "aclmanager#" + (object) (int) category;
      object aclManagerInternal = this.sessionObjects[(object) key];
      if (aclManagerInternal != null)
        return aclManagerInternal;
      switch (category)
      {
        case AclCategory.Features:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<FeaturesAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.InputForms:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<InputFormsAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.Milestones:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<MilestonesAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.LoanFolderMove:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<LoanFoldersAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.ToolsGrantWriteAccess:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<ToolsAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.MilestonesFreeRole:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<MilestonesFreeRoleAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.Services:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<ServicesAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.FieldAccess:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<FieldAccessAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.PersonaPipelineView:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<PipelineViewAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.LoanDuplicationTemplates:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<LoanDuplicationAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.ExportServices:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<ExportServicesAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.FeatureConfigs:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<FeatureConfigsAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.InvestorServices:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<InvestorServicesAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.EnhancedConditions:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<EnhancedConditionsAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.LOConnectCustomServices:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<LoConnectServiceAclManager>().Initialize((ISession) this);
          break;
        case AclCategory.StandardWebforms:
          aclManagerInternal = (object) InterceptionUtils.NewInstance<StandardWebFormAclManager>().Initialize((ISession) this);
          break;
      }
      if (aclManagerInternal == null)
        Err.Raise(nameof (Session), new ServerException("Invalid object name \"" + key + "\"", this.SessionInfo));
      return aclManagerInternal;
    }

    public void ImpersonateUser(string userId)
    {
      this.onApiCalled(nameof (ImpersonateUser), (object) userId);
      try
      {
        this.securityManager.DemandRootAdministrator();
        if (!UserStore.GetLatestVersion(userId).Exists)
          Err.Raise(TraceLevel.Warning, nameof (Session), (ServerException) new ObjectNotFoundException("Invalid User ID \"" + userId + "\"", ObjectType.User, (object) userId));
        this.effectiveUserId = userId;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public void RestoreIdentity()
    {
      this.onApiCalled(nameof (RestoreIdentity));
      try
      {
        this.effectiveUserId = this.LoginParams.UserID;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public void RegisterForEvents(Type eventType)
    {
      this.onApiCalled(nameof (RegisterForEvents), (object) eventType);
      if (eventType == (Type) null)
        Err.Raise(TraceLevel.Warning, nameof (Session), (ServerException) new ServerArgumentException("Event type cannot be null", nameof (eventType), this.SessionInfo));
      try
      {
        if (!typeof (ServerMonitorEvent).IsAssignableFrom(eventType))
          Err.Raise(TraceLevel.Warning, nameof (Session), (ServerException) new ServerArgumentException("Event type must derive from ServerMonitorEvent class", nameof (eventType), this.SessionInfo));
        this.context.Sessions.Events.RegisterListener(eventType, (IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public void UnregisterForEvents(Type eventType)
    {
      this.onApiCalled(nameof (UnregisterForEvents), (object) eventType);
      if (eventType == (Type) null)
        Err.Raise(TraceLevel.Warning, nameof (Session), (ServerException) new ServerArgumentException("Event type cannot be null", nameof (eventType), this.SessionInfo));
      try
      {
        if (!typeof (ServerMonitorEvent).IsAssignableFrom(eventType))
          Err.Raise(TraceLevel.Warning, nameof (Session), (ServerException) new ServerArgumentException("Event type must derive from ServerMonitorEvent class", nameof (eventType), this.SessionInfo));
        this.context.Sessions.Events.UnregisterListener(eventType, (IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public void RegisterForTracing(TraceLevel traceLevel)
    {
      this.onApiCalled(nameof (RegisterForTracing), (object) traceLevel);
      try
      {
        this.Security.DemandAdministrator();
        this.context.Sessions.Tracing.RegisterListener(traceLevel, (IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public void UnregisterForTracing()
    {
      this.onApiCalled(nameof (UnregisterForTracing));
      try
      {
        this.Security.DemandAdministrator();
        this.context.Sessions.Tracing.UnregisterListener((IClientSession) this);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public ISessionStartupInfo GetSessionStartupInfo()
    {
      this.onApiCalled(nameof (GetSessionStartupInfo));
      try
      {
        SessionStartupInfo startupInfo = new SessionStartupInfo();
        startupInfo.SessionID = this.SessionID;
        using (User latestVersion = UserStore.GetLatestVersion(this.effectiveUserId))
        {
          startupInfo.UserInfo = latestVersion.UserInfo;
          startupInfo.UserLicense = latestVersion.GetUserLicense();
          startupInfo.UserProfileSettings = (IDictionary) latestVersion.GetAllPrivateProfileStrings();
          startupInfo.IsPasswordChangeRequired = latestVersion.IsPasswordChangeRequired();
          startupInfo.DataServicesOptOut = EncompassSystemDbAccessor.GetEncompassSystemInfo().DataServicesOptOut || latestVersion.UserInfo.DataServicesOptOut;
          startupInfo.AccessibleFoldersForMove = LoanFoldersAclDbAccessor.GetUserApplicationLoanFolders(AclFeature.LoanMgmt_Move, latestVersion.UserInfo);
          startupInfo.AccessibleFoldersForImport = LoanFoldersAclDbAccessor.GetUserApplicationLoanFolders(AclFeature.LoanMgmt_Import, latestVersion.UserInfo);
          startupInfo.UserAclFeatureRights = FeaturesAclDbAccessor.CheckPermissions(FeatureSets.All, latestVersion.UserInfo);
          startupInfo.UserAclEnhancedConditionRights = EnhancedConditionsAclDbAccessor.CheckPermissions(FeatureSets.AclEnhancedConditions, latestVersion.UserInfo);
          startupInfo.UserAclFeaturConfigRights = FeatureConfigsAclDbAccessor.CheckPermissions(FeatureSets.AllConfigs, latestVersion.UserInfo);
          startupInfo.AllowedRoles = WorkflowBpmDbAccessor.GetUsersAllowedRoles(latestVersion.UserID);
          startupInfo.AuthorizedVersion = VersionManagementSettings.GetAuthorizedVersion(latestVersion.UserID);
        }
        startupInfo.RoleMappings = WorkflowBpmDbAccessor.GetAllRoleMappingInfos();
        startupInfo.ActiveRules = BpmDbAccessor.GetRules(BizRule.BizRuleTypes, true);
        startupInfo.MilestoneTemplate = WorkflowBpmDbAccessor.GetMilestoneTemplate();
        startupInfo.Milestones = WorkflowBpmDbAccessor.GetMilestones(false);
        startupInfo.CompanyInfo = Company.GetCompanyInfo();
        startupInfo.ServerLicense = Company.GetServerLicense();
        startupInfo.SystemInfo = EncompassSystemDbAccessor.GetEncompassSystemInfo();
        startupInfo.ServerID = EncompassServer.ServerID;
        startupInfo.ServerInstanceName = this.Context.InstanceName;
        startupInfo.Plugins = Plugins.GetPluginInfos();
        startupInfo.RuntimeEnvironment = ServerGlobals.RuntimeEnvironment;
        startupInfo.PreauthorizedModules = PreauthorizedModuleAccessor.GetPreauthorizedModules();
        Dictionary<string, Hashtable> companySettings = Company.GetCompanySettings(new string[9]
        {
          "CLIENT",
          "DOCS",
          "TEMPLATE",
          "LOANSTORAGE",
          "PASSWORD",
          "FEATURE",
          "POLICIES",
          "LICENSE",
          "MIGRATION"
        });
        bool flag1 = companySettings["CLIENT"].ContainsKey((object) "ALLOWDDM");
        startupInfo.AllowDDM = flag1 && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWDDM"]) && Company.GetCurrentEdition() == EncompassEdition.Banker;
        startupInfo.RevertPluginChanges = !companySettings["CLIENT"].ContainsKey((object) "REVERTPLUGINCHANGES") || Convert.ToBoolean(companySettings["CLIENT"][(object) "REVERTPLUGINCHANGES"]);
        companySettings["CLIENT"].ContainsKey((object) "ALLOWAUTOECONSENT");
        startupInfo.AllowAutoEConsent = flag1 && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWAUTOECONSENT"]);
        companySettings["CLIENT"].ContainsKey((object) "ALLOWAUTODISCLOSURE");
        startupInfo.AllowAutoDisclosure = flag1 && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWAUTODISCLOSURE"]);
        startupInfo.AllowDRSBarCoding = companySettings["CLIENT"].ContainsKey((object) "ALLOWDRSBARCODING") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWDRSBARCODING"]);
        startupInfo.HasAIQLicense = companySettings["LICENSE"].ContainsKey((object) "ENABLEAIQLICENSE") && Convert.ToBoolean(companySettings["LICENSE"][(object) "ENABLEAIQLICENSE"]);
        startupInfo.AIQSiteID = string.Empty;
        startupInfo.AIQBaseAddress = string.Empty;
        startupInfo.AllowLoanErrorInfo = false;
        startupInfo.AllowInsightsSetup = companySettings["CLIENT"].ContainsKey((object) "ALLOWINSIGHTSSETUP") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWINSIGHTSSETUP"]);
        startupInfo.SendBusinessRuleErrorsToServer = companySettings["CLIENT"].ContainsKey((object) "SENDBUSINESSRULEERRORSTOSERVER") && Convert.ToBoolean(companySettings["CLIENT"][(object) "SENDBUSINESSRULEERRORSTOSERVER"]);
        startupInfo.AllowDataAndDocs = companySettings["CLIENT"].ContainsKey((object) "ALLOWDATADOCS") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWDATADOCS"]);
        startupInfo.AllowDetailedPerfMeter = companySettings["CLIENT"].ContainsKey((object) "ALLOWDETAILEDPERFMETER") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWDETAILEDPERFMETER"]);
        bool flag2 = companySettings["CLIENT"].ContainsKey((object) "ENHANCEDCONDITIONSETTINGS");
        startupInfo.EnhancedConditionSettings = flag2 && Convert.ToBoolean(companySettings["CLIENT"][(object) "ENHANCEDCONDITIONSETTINGS"]) && Company.GetCurrentEdition() == EncompassEdition.Banker;
        if (companySettings.ContainsKey("DOCS") && companySettings["DOCS"] != null)
        {
          if (companySettings["DOCS"].ContainsKey((object) "PerformancePublishingRate"))
          {
            try
            {
              startupInfo.DocsPerformancePublishingRate = Convert.ToDouble(companySettings["DOCS"][(object) "PerformancePublishingRate"]);
              goto label_11;
            }
            catch
            {
              startupInfo.DocsPerformancePublishingRate = -1.0;
              goto label_11;
            }
          }
        }
        startupInfo.DocsPerformancePublishingRate = 0.0;
label_11:
        startupInfo.AllowDataAndDocsReview = companySettings["CLIENT"].ContainsKey((object) "ALLOWDATADOCSREVIEW") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWDATADOCSREVIEW"]);
        startupInfo.EnableCoC = companySettings["CLIENT"].ContainsKey((object) "ENABLECOC") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ENABLECOC"]);
        startupInfo.UsePipelineTabStartup = companySettings["CLIENT"].ContainsKey((object) "USEPIPELINETABSTARTUP") && Convert.ToBoolean(companySettings["CLIENT"][(object) "USEPIPELINETABSTARTUP"]);
        startupInfo.EnableTempBuyDown = companySettings["CLIENT"].ContainsKey((object) "ENABLETEMPBUYDOWN") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ENABLETEMPBUYDOWN"]);
        startupInfo.EnableESignDocuments = companySettings["CLIENT"].ContainsKey((object) "ENABLEESIGNDOCUMENTS") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ENABLEESIGNDOCUMENTS"]);
        startupInfo.AllowMileStoneAdjustDateLog = companySettings["CLIENT"].ContainsKey((object) "ALLOWMILESTONEADJUSTDATELOG") && Convert.ToBoolean(companySettings["CLIENT"][(object) "ALLOWMILESTONEADJUSTDATELOG"]);
        startupInfo.UseBackgroundConversion = companySettings.ContainsKey("TEMPLATE") && companySettings["TEMPLATE"] != null && companySettings["TEMPLATE"].ContainsKey((object) "USEBACKGROUNDCONVERSION") && Convert.ToBoolean(companySettings["TEMPLATE"][(object) "USEBACKGROUNDCONVERSION"]);
        startupInfo.SupportingDataSource = !companySettings.ContainsKey("LOANSTORAGE") || companySettings["LOANSTORAGE"] == null || !companySettings["LOANSTORAGE"].ContainsKey((object) "SUPPORTINGDATA") ? SupportingDataSource.CIFS : (SupportingDataSource) Enum.Parse(typeof (SupportingDataSource), (string) companySettings["LOANSTORAGE"][(object) "SUPPORTINGDATA"], true);
        if (companySettings.ContainsKey("PASSWORD") && companySettings["PASSWORD"] != null && companySettings["PASSWORD"].ContainsKey((object) "MFAENABLEDPRODUCTS"))
        {
          MFAEnabledProducts mfaEnabledProducts = (MFAEnabledProducts) Enum.Parse(typeof (MFAEnabledProducts), companySettings["PASSWORD"][(object) "MFAENABLEDPRODUCTS"].ToString());
          startupInfo.EnableMFA = (mfaEnabledProducts & MFAEnabledProducts.Encompass) == MFAEnabledProducts.Encompass;
        }
        else
          startupInfo.EnableMFA = false;
        if (companySettings["FEATURE"].ContainsKey((object) "EnableConcurrentLoanEditing") && string.Equals((string) companySettings["FEATURE"][(object) "EnableConcurrentLoanEditing"], "true", StringComparison.OrdinalIgnoreCase))
          startupInfo.EnableConcurrentLoanEditing = true;
        if (companySettings["FEATURE"].ContainsKey((object) "ENABLEFASTLOANLOAD") && (string) companySettings["FEATURE"][(object) "ENABLEFASTLOANLOAD"] == "True")
          startupInfo.FastLoanLoad = true;
        if (companySettings["FEATURE"].ContainsKey((object) "UpdateXDBBasedOnFieldChanges") && string.Equals((string) companySettings["FEATURE"][(object) "UpdateXDBBasedOnFieldChanges"], "true", StringComparison.OrdinalIgnoreCase))
          startupInfo.UpdateXDBBasedOnFieldChanges = true;
        if (companySettings["FEATURE"].ContainsKey((object) "SkipLogListInitForLoanCalcClone") && string.Equals((string) companySettings["FEATURE"][(object) "SkipLogListInitForLoanCalcClone"], "true", StringComparison.OrdinalIgnoreCase))
          startupInfo.SkipLogInitializationForLoanCalculatorClone = true;
        startupInfo.SkipCustomCalcsExecutionOnLoanOpen = companySettings["POLICIES"].ContainsKey((object) "SKIPCUSTOMCALCSEXECONLOANOPEN") && Convert.ToBoolean(companySettings["POLICIES"][(object) "SKIPCUSTOMCALCSEXECONLOANOPEN"]);
        startupInfo.CollectLanguagePreference = companySettings["POLICIES"].ContainsKey((object) "CollectLanguagePreference") && Convert.ToBoolean(companySettings["POLICIES"][(object) "CollectLanguagePreference"]);
        startupInfo.CollectHomeownershipCounseling = companySettings["POLICIES"].ContainsKey((object) "CollectHomeownershipCounseling") && Convert.ToBoolean(companySettings["POLICIES"][(object) "CollectHomeownershipCounseling"]);
        startupInfo.OAPIGatewayBaseUri = EnConfigurationSettings.AppSettings["oAuth.Url"];
        startupInfo.Epc2HostAdapterUrl = EnConfigurationSettings.AppSettings["hostAdapter2.Url"];
        startupInfo.RestApiBaseUriKey = EnConfigurationSettings.AppSettings["restApiBaseUriKey"];
        startupInfo.FsApiBaseUriKey = EnConfigurationSettings.AppSettings["fsApiBaseUriKey"];
        startupInfo.AccessTokenBaseDelay = Convert.ToInt32(EnConfigurationSettings.AppSettings["AccessTokenBaseDelay"]);
        startupInfo.AccessTokenMaxDelay = Convert.ToInt32(EnConfigurationSettings.AppSettings["AccessTokenMaxDelay"]);
        startupInfo.AccessTokenTimeOut = Convert.ToInt32(EnConfigurationSettings.AppSettings["AccessTokenTimeOut"]);
        startupInfo.SSFClientId = EnConfigurationSettings.AppSettings["SSFClientID"];
        startupInfo.SSFClientSecret = EnConfigurationSettings.AppSettings["SSFClientSecret"];
        startupInfo.InsightsSetupUrl = EnConfigurationSettings.AppSettings["InsightsSetup.Url"];
        startupInfo.ViewerUrl = EnConfigurationSettings.AppSettings["Viewer.Url"];
        startupInfo.SSOGuestLoginUrl = EnConfigurationSettings.AppSettings["SSOGuestLoginUrl"];
        startupInfo.CustomLoanDeliveryPageURL = EnConfigurationSettings.AppSettings["CustomLoanDeliveryPageURL"];
        startupInfo.CustomLoanSubmissionStatusPageURL = EnConfigurationSettings.AppSettings["CustomLoanSubmissionStatusPageURL"];
        startupInfo.eCloseUrl = EnConfigurationSettings.AppSettings["eClose.Url"];
        startupInfo.eConsentUrl = EnConfigurationSettings.AppSettings["eConsent.Url"];
        startupInfo.eDisclosuresUrl = EnConfigurationSettings.AppSettings["eDisclosures.Url"];
        startupInfo.eNoteUrl = EnConfigurationSettings.AppSettings["eNote.Url"];
        startupInfo.eSignPackagesUrl = EnConfigurationSettings.AppSettings["eSignPackages.Url"];
        startupInfo.NotificationTemplatesUrl = EnConfigurationSettings.AppSettings["NotificationTemplates.Url"];
        startupInfo.CustomImportConditionsPageURL = EnConfigurationSettings.AppSettings["CustomImportConditionsPageURL"];
        startupInfo.OAuthClientId = EnConfigurationSettings.AppSettings["oAuth.ClientId"];
        startupInfo.OAuthEncWebClientId = EnConfigurationSettings.AppSettings["oAuth.EncWebClientId"];
        startupInfo.OAuthClientSecret = EnConfigurationSettings.AppSettings["oAuth.ClientSecret"];
        startupInfo.InvestorConnectAppUrl = EnConfigurationSettings.AppSettings["InvestorConnectApp.Url"];
        startupInfo.WorkflowTasksPageUrl = EnConfigurationSettings.AppSettings["WorkflowTasksPageUrl"];
        startupInfo.LOConnectUrl = EnConfigurationSettings.AppSettings["LOConnectUrl"];
        startupInfo.AllRegsUrl = EnConfigurationSettings.AppSettings["AllRegsUrl"];
        startupInfo.WebhookEventXapiKey = Rijndael256Util.Decrypt(EnConfigurationSettings.AppSettings["Webhook-x-api-key"]);
        startupInfo.ClientSettings = (IDictionary) EnConfigurationSettings.AppSettings.ClientSettings;
        startupInfo.IsUserPipelineViewFromDB = companySettings["POLICIES"].ContainsKey((object) "UseUserPipelineViewFromDB") && Utils.ParseBoolean(companySettings["POLICIES"][(object) "UseUserPipelineViewFromDB"]) && companySettings["MIGRATION"][(object) "MigrateUserPipelineView"].ToString() == "1";
        startupInfo.EnableLoanSoftArchival = companySettings["POLICIES"].ContainsKey((object) "EnableLoanSoftArchival") && Utils.ParseBoolean(companySettings["POLICIES"][(object) "EnableLoanSoftArchival"]);
        if (companySettings.ContainsKey("PASSWORD") && companySettings["PASSWORD"] != null && companySettings["PASSWORD"].ContainsKey((object) "SSOLOGIN"))
        {
          EnableDisableSetting enableDisableSetting = (EnableDisableSetting) Enum.Parse(typeof (EnableDisableSetting), companySettings["PASSWORD"][(object) "SSOLOGIN"].ToString());
          startupInfo.EnableSSO = enableDisableSetting == EnableDisableSetting.Enabled;
        }
        else
          startupInfo.EnableSSO = false;
        startupInfo.IsWebLoginEnabled = ((Func<bool>) (() =>
        {
          object obj = this.context.Cache.Get(string.Format("TokenLoginOnly_{0}", (object) startupInfo.ServerInstanceName));
          return obj != null && obj.ToString().Equals("1");
        }))();
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(startupInfo.UserInfo.OrgId))
        {
          if (latestVersion.Exists)
            startupInfo.OrgInfo = latestVersion.GetOrganizationInfo();
        }
        startupInfo.BranchInfo = OrganizationStore.GetFirstAvaliableOrganization(startupInfo.UserInfo.OrgId, true);
        startupInfo.ComponentSettings = this.Context.Settings.GetServerSettings("Components");
        startupInfo.UnpublishedSettings = this.Context.Settings.GetServerSettings("Unpublished");
        startupInfo.PrintSettings = this.Context.Settings.GetServerSettings("Printing");
        startupInfo.PolicySettings = this.Context.Settings.GetServerSettings("Policies");
        startupInfo.EFolderSettings = this.Context.Settings.GetServerSettings("eFolder");
        startupInfo.TradeSettings = this.Context.Settings.GetServerSettings("Trade");
        if (flag1 && startupInfo.PolicySettings != null)
          startupInfo.EnableDDMPerformance = startupInfo.PolicySettings.Contains((object) "Policies.EnableDDMPerformance") && (bool) startupInfo.PolicySettings[(object) "Policies.EnableDDMPerformance"];
        startupInfo.EnhancedConditionsWorkflow = flag2 && startupInfo.PolicySettings != null && startupInfo.PolicySettings.Contains((object) "Policies.ENHANCEDCONDITIONSWORKFLOW") && (bool) startupInfo.PolicySettings[(object) "Policies.ENHANCEDCONDITIONSWORKFLOW"];
        startupInfo.EnableTradeLoanUpdateNotification = companySettings["FEATURE"].ContainsKey((object) "TradeLoanUpdateNotification") && Convert.ToBoolean(companySettings["FEATURE"][(object) "TradeLoanUpdateNotification"]);
        startupInfo.LockComparisonFields = LockComparisonFields.GetLockComparisonFields();
        startupInfo.LicenseSettings = this.Context.Settings.GetServerSettings("License");
        startupInfo.ResourceCenterUrl = ((Func<string>) (() =>
        {
          string sessionStartupInfo = EnConfigurationSettings.AppSettings["ResourceCenter.Url"];
          if (string.IsNullOrWhiteSpace(sessionStartupInfo))
            sessionStartupInfo = "https://resourcecenter.elliemae.com";
          return sessionStartupInfo;
        }))();
        startupInfo.StoredIMMessages = IMDBAccessor.GetStoredIMMessages(this.UserID);
        startupInfo.AllowURLA2020 = Convert.ToBoolean(companySettings["FEATURE"][(object) "ENABLEURLA2020"]);
        startupInfo.OtpSupport = Convert.ToBoolean(companySettings["FEATURE"][(object) "Otpsupport"]);
        startupInfo.EnableSendDisclosureSmartClient = Convert.ToBoolean(companySettings["Policies"][(object) "EnableSendDisclosureSmartClient"]) && startupInfo.OtpSupport;
        startupInfo.EnableSendDisclosureENCW = Convert.ToBoolean(companySettings["Policies"][(object) "EnableSendDisclosure"]) && startupInfo.OtpSupport;
        startupInfo.EnableSendDisclosureTPOC = Convert.ToBoolean(companySettings["Policies"][(object) "EnableSendDisclosureTPOC"]) && startupInfo.OtpSupport;
        startupInfo.EnableTriggerToRunEscrowDateCalc = companySettings["Policies"].ContainsKey((object) "ENABLETRIGGERTORUNESCROWDATECALC") && companySettings["Policies"][(object) "ENABLETRIGGERTORUNESCROWDATECALC"].ToString().ToLower() == "enabled";
        startupInfo.EnableWorkFlowTasks = Convert.ToBoolean(companySettings["FEATURE"][(object) "ENABLEWORKFLOWTASKS"]);
        startupInfo.ShowUIWorkflowTasksTools = Convert.ToBoolean(companySettings["FEATURE"][(object) "ShowUIWorkflowTasksTools"]);
        startupInfo.AllowNotifications = Convert.ToBoolean(companySettings["Client"][(object) "ALLOWNOTIFICATIONS"]);
        startupInfo.Correspondent = Convert.ToBoolean(companySettings["Policies"][(object) "CORRESPONDENT"]);
        startupInfo.AllowWCRouting = false;
        startupInfo.EnableAutoRetrieveSettings = Convert.ToBoolean(companySettings["FEATURE"][(object) "EnableAutoRetrieveSettings"]);
        startupInfo.UseFullProjectPaymentTriggers = companySettings["FEATURE"].ContainsKey((object) "UseFullProjectPaymentTriggers") && ((string) companySettings["FEATURE"][(object) "UseFullProjectPaymentTriggers"]).ToLower() == "true";
        startupInfo.CurrentUser = this.currentUser;
        startupInfo.ServerObjects = (IDictionary) new Hashtable();
        startupInfo.ServerObjects.Add((object) "OrganizationManager", this.GetObjectInternal("OrganizationManager"));
        startupInfo.ServerObjects.Add((object) "LoanManager", this.GetObjectInternal("LoanManager"));
        startupInfo.ServerObjects.Add((object) "ConfigurationManager", this.GetObjectInternal("ConfigurationManager"));
        startupInfo.ServerObjects.Add((object) "CalendarManager", this.GetObjectInternal("CalendarManager"));
        startupInfo.ServerObjects.Add((object) "ContactManager", this.GetObjectInternal("ContactManager"));
        startupInfo.ServerObjects.Add((object) "ContactGroupManager", this.GetObjectInternal("ContactGroupManager"));
        startupInfo.ServerObjects.Add((object) "AlertManager", this.GetObjectInternal("AlertManager"));
        startupInfo.ServerObjects.Add((object) "PersonaManager", this.GetObjectInternal("PersonaManager"));
        startupInfo.ServerObjects.Add((object) "ServerManager", this.GetObjectInternal("ServerManager"));
        startupInfo.ServerObjects.Add((object) "CampaignManager", this.GetObjectInternal("CampaignManager"));
        startupInfo.ServerObjects.Add((object) "MessengerListManager", this.GetObjectInternal("MessengerListManager"));
        startupInfo.ServerObjects.Add((object) "BpmManager", this.GetObjectInternal("BpmManager"));
        startupInfo.ServerObjects.Add((object) "FormManager", this.GetObjectInternal("FormManager"));
        startupInfo.ServerObjects.Add((object) "AclGroupManager", this.GetObjectInternal("AclGroupManager"));
        startupInfo.ServerObjects.Add((object) "MasterContractManager", this.GetObjectInternal("MasterContractManager"));
        startupInfo.ServerObjects.Add((object) "IdentityManager", this.GetObjectInternal("IdentityManager"));
        startupInfo.ServerObjects.Add((object) "CorrespondentMasterManager", this.GetObjectInternal("CorrespondentMasterManager"));
        startupInfo.ServerObjects.Add((object) "LoanTradeManager", this.GetObjectInternal("LoanTradeManager"));
        startupInfo.ServerObjects.Add((object) "SecurityTradeManager", this.GetObjectInternal("SecurityTradeManager"));
        startupInfo.ServerObjects.Add((object) "MbsPoolManager", this.GetObjectInternal("MbsPoolManager"));
        startupInfo.ServerObjects.Add((object) "SpecialFeatureCodeManager", this.GetObjectInternal("SpecialFeatureCodeManager"));
        startupInfo.ServerObjects.Add((object) "LoanExternalFieldManager", this.GetObjectInternal("LoanExternalFieldManager"));
        startupInfo.ServerObjects.Add((object) "LockComparisonFieldManager", this.GetObjectInternal("LockComparisonFieldManager"));
        startupInfo.AclObjects = (IDictionary) new Hashtable();
        startupInfo.AclObjects.Add((object) AclCategory.Features, this.GetAclManagerInternal(AclCategory.Features));
        startupInfo.AclObjects.Add((object) AclCategory.FeatureConfigs, this.GetAclManagerInternal(AclCategory.FeatureConfigs));
        startupInfo.AclObjects.Add((object) AclCategory.LoanFolderMove, this.GetAclManagerInternal(AclCategory.LoanFolderMove));
        startupInfo.AclObjects.Add((object) AclCategory.InputForms, this.GetAclManagerInternal(AclCategory.InputForms));
        startupInfo.AclObjects.Add((object) AclCategory.Services, this.GetAclManagerInternal(AclCategory.Services));
        startupInfo.AclObjects.Add((object) AclCategory.FieldAccess, this.GetAclManagerInternal(AclCategory.FieldAccess));
        startupInfo.AclObjects.Add((object) AclCategory.PersonaPipelineView, this.GetAclManagerInternal(AclCategory.PersonaPipelineView));
        startupInfo.AclObjects.Add((object) AclCategory.ExportServices, this.GetAclManagerInternal(AclCategory.ExportServices));
        startupInfo.AclObjects.Add((object) AclCategory.Milestones, this.GetAclManagerInternal(AclCategory.Milestones));
        startupInfo.AclObjects.Add((object) AclCategory.LOConnectCustomServices, this.GetAclManagerInternal(AclCategory.LOConnectCustomServices));
        startupInfo.ServiceUrls = new ServiceUrls()
        {
          UpdateServicesUrl = EnConfigurationSettings.AppSettings["UpdateServicesUrl"],
          DataServicesUrl = EnConfigurationSettings.AppSettings["DataServicesUrl"],
          LoginServicesUrl = EnConfigurationSettings.AppSettings["LoginServicesUrl"],
          JedServicesUrl = EnConfigurationSettings.AppSettings["JedServicesUrl"],
          LoanCenterServiceUrl = EnConfigurationSettings.AppSettings["LoanCenterServiceUrl"],
          CenterwiseServicesUrl = EnConfigurationSettings.AppSettings["CenterwiseServicesUrl"],
          DataServices2Url = EnConfigurationSettings.AppSettings["DataServices2Url"],
          ERDBWebServiceUrl = EnConfigurationSettings.AppSettings["ERDBWebServiceUrl"],
          MaventServiceUrl = EnConfigurationSettings.AppSettings["MaventServiceUrl"],
          HomePageUrl = EnConfigurationSettings.AppSettings["HomePageUrl"],
          PPEServiceUrl = EnConfigurationSettings.AppSettings["PPEServiceUrl"],
          ePassAServiceUrl = EnConfigurationSettings.AppSettings["ePassAServiceUrl"],
          DocServiceUrl = EnConfigurationSettings.AppSettings["DocServiceUrl"],
          ImageLibraryServiceUrl = EnConfigurationSettings.AppSettings["ImageLibraryServiceUrl"],
          IDRServiceUrl = EnConfigurationSettings.AppSettings["IDRServiceUrl"],
          InboxServiceUrl = EnConfigurationSettings.AppSettings["InboxServiceUrl"],
          FulfillmentServiceUrl = EnConfigurationSettings.AppSettings["FulfillmentServiceUrl"],
          CompanySettingsServiceUrl = EnConfigurationSettings.AppSettings["CompanySettingsServiceUrl"],
          EVaultRetrieveServiceUrl = EnConfigurationSettings.AppSettings["EVaultRetrieveServiceUrl"],
          MaventProductDocsUrl = EnConfigurationSettings.AppSettings["MaventProductDocsUrl"],
          LogTransactionUrl = EnConfigurationSettings.AppSettings["LogTransactionUrl"],
          ECSProductsUrl = EnConfigurationSettings.AppSettings["ECSProductsUrl"],
          EPackageServiceUrl = EnConfigurationSettings.AppSettings["EPackageServiceUrl"],
          DownloadServiceUrl = EnConfigurationSettings.AppSettings["DownloadServiceUrl"],
          EpassAiUrl = EnConfigurationSettings.AppSettings["EpassAiUrl"],
          LicenseUrl = EnConfigurationSettings.AppSettings["LicenseUrl"],
          CustomerLoyalty = EnConfigurationSettings.AppSettings["CustomerLoyalty"],
          ConsentServiceUrl = EnConfigurationSettings.AppSettings["ConsentServiceUrl"],
          EmailNotificationUrl = EnConfigurationSettings.AppSettings["EmailNotificationUrl"],
          DocumentClassificationUrl = EnConfigurationSettings.AppSettings["DocumentClassificationUrl"],
          EppsPricingUrl = EnConfigurationSettings.AppSettings["EppsPricingUrl"],
          EppsWebserviceUrl = EnConfigurationSettings.AppSettings["EppsWebserviceUrl"],
          EppsCoreContourUrl = EnConfigurationSettings.AppSettings["EppsCoreContourUrl"],
          LenderCenterLogTransactionUrl = EnConfigurationSettings.AppSettings["LenderCenterLogTransactionUrl"],
          MiCenterUrl = EnConfigurationSettings.AppSettings["MiCenterUrl"],
          LoanCenterLogTransactionUrl = EnConfigurationSettings.AppSettings["LoanCenterLogTransactionUrl"],
          ePassGetMessageAlertsUrl = EnConfigurationSettings.AppSettings["ePassGetMessageAlertsUrl"],
          EcloseSetupUrl = EnConfigurationSettings.AppSettings["eCloseSetupUrl"],
          TqlAvmServiceUrl = EnConfigurationSettings.AppSettings["TqlAvmServiceUrl"],
          TqlFloodServiceUrl = EnConfigurationSettings.AppSettings["TqlFloodServiceUrl"],
          TqlFloodCoreLogicServiceUrl = EnConfigurationSettings.AppSettings["TqlFloodCoreLogicServiceUrl"],
          TqlFloodLpsServiceUrl = EnConfigurationSettings.AppSettings["TqlFloodLpsServiceUrl"],
          TqlFloodVendorServiceUrl = EnConfigurationSettings.AppSettings["TqlFloodVendorServiceUrl"],
          TqlFraudServiceUrl = EnConfigurationSettings.AppSettings["TqlFraudServiceUrl"],
          TqlTaxReturnServiceUrl = EnConfigurationSettings.AppSettings["TqlTaxReturnServiceUrl"],
          TqlDemoVendorServiceUrl = EnConfigurationSettings.AppSettings["TqlDemoVendorServiceUrl"],
          TqlNotificationServiceUrl = EnConfigurationSettings.AppSettings["TqlNotificationServiceUrl"],
          TqlPlatformServiceUrl = EnConfigurationSettings.AppSettings["TqlPlatformServiceUrl"],
          TqlVendorContentServiceUrl = EnConfigurationSettings.AppSettings["TqlVendorContentServiceUrl"],
          TqlVendorGatewayServiceUrl = EnConfigurationSettings.AppSettings["TqlVendorGatewayServiceUrl"],
          AppraisalCenterBaseUrl = EnConfigurationSettings.AppSettings["AppraisalCenterBaseUrl"],
          AppraisalCenterServiceUrl = EnConfigurationSettings.AppSettings["AppraisalCenterServiceUrl"],
          TitleCenterBaseUrl = EnConfigurationSettings.AppSettings["TitleCenterBaseUrl"],
          TitleCenterServiceUrl = EnConfigurationSettings.AppSettings["TitleCenterServiceUrl"],
          MisAppraisalBillingInterfaceServiceUrl = EnConfigurationSettings.AppSettings["MisAppraisalBillingInterfaceServiceUrl"],
          MisTitleBillingInterfaceServiceUrl = EnConfigurationSettings.AppSettings["MisTitleBillingInterfaceServiceUrl"],
          MarketPlaceUrl = EnConfigurationSettings.AppSettings["MarketPlaceUrl"],
          EncompassCRMUrl = EnConfigurationSettings.AppSettings["EncompassCRMUrl"],
          ResourceCenterUrl = EnConfigurationSettings.AppSettings["ResourceCenterUrl"],
          EllieMaeResourceCenterUrl = EnConfigurationSettings.AppSettings["EllieMaeResourceCenterUrl"],
          MessageListUrl = EnConfigurationSettings.AppSettings["MessageListUrl"],
          CBSMarketWatchUrl = EnConfigurationSettings.AppSettings["CBSMarketWatchUrl"],
          EncompassWebcenterUrl = EnConfigurationSettings.AppSettings["EncompassWebcenterUrl"],
          EducationUrl = EnConfigurationSettings.AppSettings["EducationUrl"],
          LogServicesUrl = EnConfigurationSettings.AppSettings["LogServicesUrl"],
          SSFHostUrl = string.IsNullOrWhiteSpace(EnConfigurationSettings.AppSettings["SSFHostUrl"]) || !Uri.IsWellFormedUriString(EnConfigurationSettings.AppSettings["SSFHostUrl"], UriKind.Absolute) ? "https://assets.ellieservices.com/encompass/" : EnConfigurationSettings.AppSettings["SSFHostUrl"]
        };
        startupInfo.AlertConfigs = AlertConfigAccessor.GetAlertConfigList();
        startupInfo.ProductPricingPartner = ProductPricingSettingsAccessor.GetActiveProductPricingPartner();
        using (DbAccessManager dbAccessManager = new DbAccessManager())
          startupInfo.DbVersion = dbAccessManager.GetDbVersion().ToString();
        try
        {
          List<TemplateSettingsType> templateSettingsTypeList = new List<TemplateSettingsType>();
          templateSettingsTypeList.Add(TemplateSettingsType.PreliminaryConditionTrackingView);
          templateSettingsTypeList.Add(TemplateSettingsType.PostClosingConditionTrackingView);
          templateSettingsTypeList.Add(TemplateSettingsType.UnderwritingConditionTrackingView);
          templateSettingsTypeList.Add(TemplateSettingsType.SellConditionTrackingView);
          templateSettingsTypeList.Add(TemplateSettingsType.EnhancedConditionTrackingView);
          for (int index = 0; index < templateSettingsTypeList.Count; ++index)
          {
            TemplateSettingsType type = templateSettingsTypeList[index];
            string str = type.ToString();
            string privateProfileString = User.GetPrivateProfileString(this.UserID, str.Substring(0, Math.Min(32, str.Length)), "DefaultView");
            if (!string.IsNullOrEmpty(privateProfileString) && string.Compare(privateProfileString, "Standard View", true) != 0)
            {
              FileSystemEntry entry = FileSystemEntry.Parse(privateProfileString);
              if (entry != null)
              {
                using (TemplateSettings latestVersion = TemplateSettingsStore.GetLatestVersion(type, entry))
                {
                  if (latestVersion != null)
                  {
                    if (latestVersion.Exists)
                    {
                      switch (type)
                      {
                        case TemplateSettingsType.PreliminaryConditionTrackingView:
                          startupInfo.DefaultPreliminaryConditionTrackingView = (ConditionTrackingView) latestVersion.Data;
                          continue;
                        case TemplateSettingsType.UnderwritingConditionTrackingView:
                          startupInfo.DefaultUnderwritingConditionTrackingView = (ConditionTrackingView) latestVersion.Data;
                          continue;
                        case TemplateSettingsType.PostClosingConditionTrackingView:
                          startupInfo.DefaultPostClosingConditionTrackingView = (ConditionTrackingView) latestVersion.Data;
                          continue;
                        case TemplateSettingsType.SellConditionTrackingView:
                          startupInfo.DefaultSellConditionTrackingView = (ConditionTrackingView) latestVersion.Data;
                          continue;
                        case TemplateSettingsType.EnhancedConditionTrackingView:
                          startupInfo.DefaultEnhancedConditionTrackingView = (ConditionTrackingView) latestVersion.Data;
                          continue;
                        default:
                          continue;
                      }
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (Session), ex);
        }
        return (ISessionStartupInfo) startupInfo;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex);
        return (ISessionStartupInfo) null;
      }
    }

    public override void Abandon()
    {
      lock (this)
      {
        if (this.disconnected)
          return;
        base.Abandon();
        string fullName = ((OrganizationManager) this.GetObjectInternal("OrganizationManager")).GetUser(this.UserID).FullName;
        if (this.appName != "trustedEnc" && this.appName != "<EncompassServer>" && this.UserID != "(trusted)")
          this.InsertAuditRecord((SystemAuditRecord) new UserLogoutAuditRecord(this.UserID, fullName, ActionType.UserLogout, this.LoginTime, this.UserID, fullName, "Logout"));
        if (this.appName != "trustedEnc" && this.appName != "<EncompassServer>")
        {
          EncompassServer.RaiseEvent(this.context, (ServerEvent) new SessionEvent(SessionEventType.Logout, this.SessionInfo));
          if (!this.IMEnabled)
            return;
          this.context.Sessions.RaiseServerEventOnRemoveServers((Message) new SessionEventMessage(SessionEventType.Logout, this.SessionInfo));
        }
        else
        {
          try
          {
            this.context.Sessions.RemoveS2SSession((IClientSession) this);
          }
          catch
          {
          }
        }
      }
    }

    private Elli.Server.Remoting.SessionObjects.SecurityManager Security => this.securityManager;

    public override void Terminate(DisconnectEventArgument disconnectEventArg, string source)
    {
      lock (this)
      {
        if (this.disconnected)
          return;
        this.onApiCalled(nameof (Terminate), (object) disconnectEventArg, (object) source);
        base.Terminate(disconnectEventArg, source);
        UserInfo userInfo = this.GetUserInfo();
        if (userInfo != (UserInfo) null)
        {
          string fullName = userInfo.FullName;
          UserLogoutAuditRecord record = new UserLogoutAuditRecord(this.UserID, fullName, ActionType.UserLogout, this.LoginTime, this.UserID, fullName, source);
          using (this.context.MakeCurrent())
            this.InsertAuditRecord((SystemAuditRecord) record);
        }
        if (disconnectEventArg == DisconnectEventArgument.Nonforce || !(this.appName != "<EncompassServer>") || !(this.appName != "trustedEnc"))
          return;
        EncompassServer.RaiseEvent(this.context, (ServerEvent) new SessionEvent(SessionEventType.Terminated, this.SessionInfo));
        if (!this.IMEnabled)
          return;
        this.context.Sessions.RaiseServerEventOnRemoveServers((Message) new SessionEventMessage(SessionEventType.Terminated, this.SessionInfo));
      }
    }

    public override void Disconnect()
    {
      lock (this)
      {
        if (this.disconnected)
          return;
        this.context.Sessions.RemoveSession((IClientSession) this, (IConnectionManager) new ConnectionManagerNoop());
        foreach (RemotedObject remotedObject in (IEnumerable) this.sessionObjects.Values)
          remotedObject.Disconnect();
        base.Disconnect();
        if (this.LoginParams.LicenseRequired)
          LicenseManager.ReleaseLicense(this.LoginParams, this.context);
        this.context.TraceLog.WriteVerbose(nameof (Session), "Session " + this.ToString() + " disconnected");
        this.disconnected = true;
      }
    }

    public override IAsyncResult SendMessage(Message message)
    {
      AsyncEventResult asyncEventResult = (AsyncEventResult) base.SendMessage(message);
      if (!(message is IMMessage))
        this.context.TraceLog.WriteVerbose(nameof (Session), "Message sent to session " + this.ToString());
      if (message is IMChatMessage)
      {
        if (asyncEventResult == null)
          throw new IMChatMessageException(IMChatMessageExceptionCause.Unknown, this.SessionID, this.UserID, "Unknown");
        if (!asyncEventResult.WaitOne(new TimeSpan(0, 0, 5), true))
          throw new IMChatMessageException(IMChatMessageExceptionCause.ConnectionTimeout, this.SessionID, this.UserID, "Connection timeout");
        if (asyncEventResult.EventStatus == AsyncEventStatus.Disconnected)
          throw new IMChatMessageException(IMChatMessageExceptionCause.NetworkDisconnected, this.SessionID, this.UserID, "Network disconnected");
        if (asyncEventResult.EventStatus == AsyncEventStatus.Failed)
          throw new IMChatMessageException(IMChatMessageExceptionCause.DeliveryFailed, this.SessionID, this.UserID, "Delivery failed");
      }
      return (IAsyncResult) asyncEventResult;
    }

    public override string ToString() => this.UserID + "\\" + this.SessionID;

    public int RemoteObjectCount
    {
      get
      {
        lock (this.sessionObjects)
          return this.sessionObjects.Count;
      }
    }

    public string[] SessionObjectNames
    {
      get
      {
        lock (this.sessionObjects)
        {
          string[] sessionObjectNames = new string[this.sessionObjects.Count];
          this.sessionObjects.Keys.CopyTo((Array) sessionObjectNames, 0);
          return sessionObjectNames;
        }
      }
    }

    public void ReleaseSessionObject(ISessionBoundObject o)
    {
      lock (this.sessionObjects)
        this.sessionObjects.Remove((object) o.ObjectKey);
    }

    public void RegisterSessionObject(ISessionBoundObject o)
    {
      lock (this.sessionObjects)
        this.sessionObjects[(object) o.ObjectKey] = (object) o;
    }

    public UserInfo GetUserInfo()
    {
      User latestVersion = UserStore.GetLatestVersion(this.UserID);
      if (latestVersion == null)
        return (UserInfo) null;
      UserInfo userInfo = (UserInfo) null;
      try
      {
        userInfo = latestVersion.UserInfo;
      }
      catch
      {
      }
      return userInfo;
    }

    private void onApiCalled(string apiName, params object[] parms)
    {
      try
      {
        if (ClientContext.GetCurrent(false) != null)
        {
          ClientContext.GetCurrent().RecordLogflag();
          ClientContext.GetCurrent().RecordClassName(nameof (Session));
          ClientContext.GetCurrent().RecordApiName(apiName);
          ClientContext.GetCurrent().RecordParms(parms);
          ClientContext.GetCurrent().RecordSession((ISession) this);
        }
        this.Diagnostics.RecordAPICall("Session." + apiName);
      }
      catch
      {
      }
    }

    public void InsertAuditRecord(SystemAuditRecord record)
    {
      this.onApiCalled(nameof (Session), (object) nameof (InsertAuditRecord), (object) record);
      try
      {
        SystemAuditTrailAccessor.InsertAuditRecord(record);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
      }
    }

    public SystemAuditRecord[] GetAuditRecord(
      string userID,
      ActionType actionType,
      DateTime startTime,
      DateTime endTime,
      string objectID,
      string objectName)
    {
      this.onApiCalled(nameof (Session), (object) nameof (GetAuditRecord), (object) userID, (object) actionType, (object) startTime, (object) endTime, (object) objectID, (object) objectName);
      try
      {
        return SystemAuditTrailAccessor.GetAuditRecord(userID, new ActionType[1]
        {
          actionType
        }, startTime, endTime, objectID, objectName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (Session), ex, this.SessionInfo);
        return (SystemAuditRecord[]) null;
      }
    }

    public void InitDataSyncManager()
    {
      if (this.sessionObjects.ContainsKey((object) "DataSyncManager"))
        return;
      InterceptionUtils.NewInstance<DataSyncManager>().Initialize((ISession) this);
    }
  }
}
