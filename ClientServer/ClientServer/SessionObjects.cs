// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SessionObjects
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.Interfaces;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SessionObjects
  {
    private ISession session;
    private ISessionStartupInfo startupInfo;
    private UserInfo userInfo;
    private string userPassword;
    private CompanyInfo companyInfo;
    private LicenseInfo serverLicense;
    private ICurrentUser currentUser;
    private Hashtable objectCache = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable aclObjectCache = new Hashtable();
    private UserLicenseInfo cachedUserLicense;
    private Task<EPassMessageInfo[]> asyncGetEpassMessagesTask;
    private Hashtable cityStateUserTables;
    private Hashtable titleEscrowTables;
    private List<HMDAProfile> hmdaProfiles;
    private bool interactive;
    public readonly bool AllowConcurrentEditing;
    public readonly bool fastLoanLoad;
    public readonly bool SkipCustomCalcsExecutionOnLoanOpen;
    public readonly bool AllowSdkCE;
    public readonly bool ExclusiveLockCurrLoginsOnly = true;
    public readonly bool Use10DecimalDigitInLockRequestSecondaryTradeAreas;
    public readonly bool EnableLoanSoftArchival;
    private URLAPageNumbering _URLAPageNumbering;
    private const string className = "SessionObjects�";
    private static readonly string sw = Tracing.SwRemoting;
    private Dictionary<string, Hashtable> companySettings;
    private Dictionary<string, ExternalLateFeeSettings> cachedExternalLateFeeSettings = new Dictionary<string, ExternalLateFeeSettings>();
    private Dictionary<CalendarType, BusinessCalendar> cachedBusinessCalendar = new Dictionary<CalendarType, BusinessCalendar>();
    private Dictionary<string, Hashtable> cachedMilestonePermissions;
    private Hashtable cachedWorkflowTaskGroupTemplates = CollectionsUtil.CreateCaseInsensitiveHashtable();
    private Hashtable cachedWorkflowTaskTemplates = CollectionsUtil.CreateCaseInsensitiveHashtable();
    public object DDMTrigger;
    public object DDMBackgroundLoadingState;
    private FieldSettings cachedFieldSettings;
    private RoleInfo[] cachedRoles;
    public Task<ILoanConfigurationInfo> LoadLoanConfigurationInfoTask;
    public Task PrecacheServiceConfigFilesTask;
    private ILoanConfigurationInfo loanConfigurationInfo;
    private bool refreshTresholdLimitsFromDB = true;
    private List<FedTresholdAdjustment> adjustments;
    private bool refreshCityStateCache = true;
    private ConfigInfoForTriggers configInfoForTriggers;
    private object lockobject = new object();
    private List<LoanFolderInfo> loanFolderInfos;
    private bool contextCreatedForFastLoanLoad;
    private static readonly string[] exclusion = new string[4]
    {
      "MIGRATION",
      "ERDB",
      "ERDBFAILURENOTIFICATION",
      "ERDBREGISTRATIONINFO"
    };

    public bool? isEclosingAllowed { get; set; }

    public string ecloseMessage { get; set; }

    public List<LoanFolderInfo> LoanFolderInfos
    {
      get => this.loanFolderInfos;
      set => this.loanFolderInfos = value;
    }

    public ConfigInfoForTriggers TriggersConfigInfo
    {
      get
      {
        if (this.configInfoForTriggers == null)
        {
          lock (this.lockobject)
          {
            if (this.configInfoForTriggers == null)
            {
              DateTime serverTime = this.Session.ServerTime;
              this.configInfoForTriggers = new ConfigInfoForTriggers((TriggerInfo[]) this.BpmManager.GetRules(BizRuleType.Triggers, true), this.StartupInfo.Milestones, serverTime);
            }
          }
        }
        return this.configInfoForTriggers;
      }
      set
      {
        lock (this.lockobject)
          this.configInfoForTriggers = value;
      }
    }

    public bool IsContextCreatedForFastLoanLoad
    {
      get => this.contextCreatedForFastLoanLoad;
      set => this.contextCreatedForFastLoanLoad = value;
    }

    public bool EditLoanConcurrently
    {
      get => this.AllowConcurrentEditing || this.EnableConcurrentLoanEditing;
    }

    public ILoanConfigurationInfo LoanConfigurationInfo
    {
      get
      {
        if (this.LoadLoanConfigurationInfoTask == null || this.loanConfigurationInfo != null)
          return this.loanConfigurationInfo;
        this.loanConfigurationInfo = this.LoadLoanConfigurationInfoTask.Result;
        return this.loanConfigurationInfo;
      }
    }

    public SessionObjects(ISession session)
      : this(session, (string) null)
    {
    }

    public SessionObjects(ISession session, string userPassword)
      : this(session, userPassword, session.GetSessionStartupInfo())
    {
    }

    public SessionObjects(ISession session, string userPassword, ISessionStartupInfo startupInfo)
    {
      this.session = session;
      this.startupInfo = startupInfo;
      this.userPassword = userPassword;
      foreach (DictionaryEntry serverObject in startupInfo.ServerObjects)
        this.objectCache[serverObject.Key] = serverObject.Value;
      foreach (DictionaryEntry aclObject in startupInfo.AclObjects)
        this.aclObjectCache[aclObject.Key] = aclObject.Value;
      this.userInfo = startupInfo.UserInfo;
      this.companyInfo = startupInfo.CompanyInfo;
      this.serverLicense = startupInfo.ServerLicense;
      this.currentUser = startupInfo.CurrentUser;
      bool flag = false;
      try
      {
        flag = this.ServerLicense.IsBankerEdition;
      }
      catch (LicenseException ex)
      {
      }
      bool policySetting1 = (bool) startupInfo.PolicySettings[(object) "Policies.AllowConcurrentEditing"];
      bool policySetting2 = (bool) startupInfo.PolicySettings[(object) "Policies.AllowSdkCE"];
      bool unpublishedSetting = (bool) startupInfo.UnpublishedSettings[(object) "Unpublished.ExclusiveLockCurrLoginsOnly"];
      try
      {
        if (this.StartupInfo.AllowURLA2020)
          this._URLAPageNumbering = (URLAPageNumbering) startupInfo.PrintSettings[(object) "Printing.URLAPageNumbering"];
      }
      catch
      {
        this._URLAPageNumbering = URLAPageNumbering.AllPages;
      }
      this.AllowConcurrentEditing = policySetting1 & flag;
      this.fastLoanLoad = startupInfo.FastLoanLoad;
      this.SkipCustomCalcsExecutionOnLoanOpen = startupInfo.SkipCustomCalcsExecutionOnLoanOpen;
      this.AllowSdkCE = this.AllowConcurrentEditing & policySetting2;
      this.ExclusiveLockCurrLoginsOnly = unpublishedSetting;
      this.Use10DecimalDigitInLockRequestSecondaryTradeAreas = (bool) startupInfo.PolicySettings[(object) "Policies.Use10DigitDecimalPricing"];
      this.EnableLoanSoftArchival = (bool) startupInfo.PolicySettings[(object) "Policies.EnableLoanSoftArchival"];
      string str = Path.Combine(AssemblyResolver.AppDataHashFolder == null ? EnConfigurationSettings.GlobalSettings.EncompassDataDirectory : Path.Combine(AssemblyResolver.AppDataHashFolder, "EncompassData"), "Settings\\Updates\\EPASSMX.INI");
      if (System.IO.File.Exists(str))
      {
        IniFile iniFile = new IniFile(str);
        if (startupInfo.ClientSettings.Contains((object) "ePassCoreUrl"))
          iniFile.Write("EPASS_URLS", "GNETHEXT", (string) startupInfo.ClientSettings[(object) "ePassCoreUrl"]);
        if (startupInfo.ClientSettings.Contains((object) "ePassMapDownloadUrl"))
          iniFile.Write("EPASS_URLS", "MAP_DOWNLOAD", (string) startupInfo.ClientSettings[(object) "ePassMapDownloadUrl"]);
        if (startupInfo.ClientSettings.Contains((object) "ePassBgdDownloadUrl"))
          iniFile.Write("EPASS_URLS", "BGD_DOWNLOAD", (string) startupInfo.ClientSettings[(object) "ePassBgdDownloadUrl"]);
        if (startupInfo.ClientSettings.Contains((object) "GNetMXUrls"))
        {
          try
          {
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass", true))
              registryKey.SetValue("GNetMXUrls", (object) (string) startupInfo.ClientSettings[(object) "GNetMXUrls"], RegistryValueKind.String);
          }
          catch
          {
          }
        }
      }
      if (this.startupInfo != null)
        return;
      this.startupInfo = (ISessionStartupInfo) new SessionStartupInfo();
    }

    public DDMFeeRule[] DDMFeeRules { get; set; }

    public DDMFieldRule[] DDMFieldRules { get; set; }

    public bool EnableConcurrentLoanEditing => this.startupInfo.EnableConcurrentLoanEditing;

    public bool FastLoanLoad => this.fastLoanLoad && this.contextCreatedForFastLoanLoad;

    public bool Offline => !System.Runtime.Remoting.RemotingServices.IsTransparentProxy((object) this.session);

    public ISession Session => this.session;

    public string SystemID => this.startupInfo.SystemInfo.SystemID;

    public string ServerID => this.startupInfo.ServerID;

    public string SessionID => this.startupInfo.SessionID;

    public bool Interactive
    {
      get => this.interactive;
      set => this.interactive = value;
    }

    public RuntimeEnvironment RuntimeEnvironment => this.startupInfo.RuntimeEnvironment;

    public EncompassSystemInfo SystemInformation => this.startupInfo.SystemInfo;

    public ISessionStartupInfo StartupInfo => this.startupInfo;

    public string UserID => this.UserInfo.Userid;

    public string UserPassword
    {
      get
      {
        ICache simpleCache = CacheManager.GetSimpleCache("SsoTokenCache");
        string key = this.SessionID + "_Elli.Emn";
        string userPassword = simpleCache.Get(key) as string;
        if (!string.IsNullOrWhiteSpace(userPassword))
          return userPassword;
        Tracing.Log(SessionObjects.sw, TraceLevel.Verbose, nameof (SessionObjects), "SSOJwt token not available in cache, regenerating token.");
        string ssoToken = this.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Emn"
        }, 15);
        simpleCache.Put(key, new CacheItem((object) ssoToken, CacheItemRetentionPolicy.ExpireIn10Mins, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior()));
        return ssoToken;
      }
    }

    public bool UsingSso => string.IsNullOrWhiteSpace(this.userPassword);

    public string CcAuthToken { get; private set; }

    public void SetCcAuthToken(string token) => this.CcAuthToken = token;

    public string IdpServer { get; private set; }

    public void SetIdpServer(string idpServer) => this.IdpServer = idpServer;

    public object this[string name]
    {
      get
      {
        lock (this)
        {
          object obj = this.objectCache[(object) name];
          if (obj == null)
          {
            obj = this.session.GetObject(name);
            this.objectCache[(object) name] = obj;
          }
          return obj;
        }
      }
    }

    public object GetAclManager(AclCategory category)
    {
      lock (this)
      {
        object aclManager = this.aclObjectCache[(object) category];
        if (aclManager == null)
        {
          aclManager = this.session.GetAclManager(category);
          this.aclObjectCache[(object) category] = aclManager;
        }
        return aclManager;
      }
    }

    public IEncERDBRegMgr EncERDBRegMgr => (IEncERDBRegMgr) this[nameof (EncERDBRegMgr)];

    public ILoanManager LoanManager => (ILoanManager) this[nameof (LoanManager)];

    public IDataSyncManager DataSyncManager
    {
      get
      {
        string str = nameof (DataSyncManager);
        if (!this.objectCache.ContainsKey((object) str))
          this.session.InitDataSyncManager();
        return (IDataSyncManager) this[str];
      }
    }

    public IServerManager ServerManager => (IServerManager) this[nameof (ServerManager)];

    public IConfigurationManager ConfigurationManager
    {
      get => (IConfigurationManager) this[nameof (ConfigurationManager)];
    }

    public IEfolderDocTrackViewManager EfolderDocTrackViewManager
    {
      get => (IEfolderDocTrackViewManager) this[nameof (EfolderDocTrackViewManager)];
    }

    public IBpmManager BpmManager => (IBpmManager) this[nameof (BpmManager)];

    public IContactManager ContactManager => (IContactManager) this[nameof (ContactManager)];

    public IFormManager FormManager => (IFormManager) this[nameof (FormManager)];

    public IMessengerListManager MessengerListManager
    {
      get => (IMessengerListManager) this[nameof (MessengerListManager)];
    }

    public IOrganizationManager OrganizationManager
    {
      get => (IOrganizationManager) this[nameof (OrganizationManager)];
    }

    public IPersonaManager PersonaManager => (IPersonaManager) this[nameof (PersonaManager)];

    public IReportManager ReportManager => (IReportManager) this[nameof (ReportManager)];

    public ICampaign CampaignManager => (ICampaign) this[nameof (CampaignManager)];

    public ICalendarManager CalendarManager => (ICalendarManager) this[nameof (CalendarManager)];

    public IWelcomeScreenSettingMgr WelcomeScreenSettingMgr
    {
      get => (IWelcomeScreenSettingMgr) this[nameof (WelcomeScreenSettingMgr)];
    }

    public IAclGroupManager AclGroupManager => (IAclGroupManager) this[nameof (AclGroupManager)];

    public IContactGroup ContactGroupManager => (IContactGroup) this[nameof (ContactGroupManager)];

    public IAlertManager AlertManager => (IAlertManager) this[nameof (AlertManager)];

    public IMasterContractManager MasterContractManager
    {
      get => (IMasterContractManager) this[nameof (MasterContractManager)];
    }

    public ISecurityTradeManager SecurityTradeManager
    {
      get => (ISecurityTradeManager) this[nameof (SecurityTradeManager)];
    }

    public ILoanTradeManager LoanTradeManager
    {
      get => (ILoanTradeManager) this[nameof (LoanTradeManager)];
    }

    public IMbsPoolManager MbsPoolManager => (IMbsPoolManager) this[nameof (MbsPoolManager)];

    public ICorrespondentTradeManager CorrespondentTradeManager
    {
      get => (ICorrespondentTradeManager) this[nameof (CorrespondentTradeManager)];
    }

    public ILockComparisonFieldManager LockComparisonFieldManager
    {
      get => (ILockComparisonFieldManager) this[nameof (LockComparisonFieldManager)];
    }

    public ICorrespondentMasterManager CorrespondentMasterManager
    {
      get => (ICorrespondentMasterManager) this[nameof (CorrespondentMasterManager)];
    }

    public IGseCommitmentManager GseCommitmentManager
    {
      get => (IGseCommitmentManager) this[nameof (GseCommitmentManager)];
    }

    public ITradeLoanUpdateQueueManager TradeLoanUpdateQueueManager
    {
      get => (ITradeLoanUpdateQueueManager) this[nameof (TradeLoanUpdateQueueManager)];
    }

    public IBatchJobsManager BatchJobsManager
    {
      get => (IBatchJobsManager) this[nameof (BatchJobsManager)];
    }

    public ITradeSynchronizationManager TradeSynchronizationManager
    {
      get => (ITradeSynchronizationManager) this[nameof (TradeSynchronizationManager)];
    }

    public ILoanSummaryExtensionManager LoanSummaryExtensionManager
    {
      get => (ILoanSummaryExtensionManager) this[nameof (LoanSummaryExtensionManager)];
    }

    public IOverNightRateProtectionManager OverNightRateProtectionManager
    {
      get => (IOverNightRateProtectionManager) this[nameof (OverNightRateProtectionManager)];
    }

    public IIdentityManager IdentityManager => (IIdentityManager) this[nameof (IdentityManager)];

    public ISpecialFeatureCodeManager SpecialFeatureCodeManager
    {
      get => (ISpecialFeatureCodeManager) this[nameof (SpecialFeatureCodeManager)];
    }

    public ILoanExternalFieldManager LoanExternalFieldManager
    {
      get => (ILoanExternalFieldManager) this[nameof (LoanExternalFieldManager)];
    }

    public ICurrentUser CurrentUser
    {
      get
      {
        lock (this)
        {
          if (this.currentUser == null)
            this.currentUser = this.session.GetUser();
          return this.currentUser;
        }
      }
    }

    public LicenseInfo ServerLicense
    {
      get
      {
        lock (this)
        {
          if (this.serverLicense == null)
            this.serverLicense = this.ConfigurationManager.GetServerLicense();
          return this.serverLicense;
        }
      }
    }

    public void InvalidateServerLicense()
    {
      lock (this)
        this.serverLicense = (LicenseInfo) null;
    }

    public UserInfo UserInfo
    {
      get
      {
        lock (this)
        {
          if (this.userInfo == (UserInfo) null)
            this.userInfo = this.CurrentUser.GetUserInfo();
          return this.userInfo;
        }
      }
    }

    public Dictionary<string, Hashtable> CachedMilestonePermissions
    {
      get
      {
        if (this.CacheMileStonePermissionsTask != null && this.CacheMileStonePermissionsTask.Status == TaskStatus.Running)
          this.CacheMileStonePermissionsTask.Wait();
        return this.cachedMilestonePermissions;
      }
    }

    public void InvalidateUserInfo()
    {
      lock (this)
        this.userInfo = (UserInfo) null;
    }

    public CompanyInfo CompanyInfo
    {
      get
      {
        lock (this)
        {
          if (this.companyInfo == null)
            this.companyInfo = this.ConfigurationManager.GetCompanyInfo();
          return this.companyInfo;
        }
      }
    }

    public void InvalidateCompanyInfo()
    {
      lock (this)
        this.companyInfo = (CompanyInfo) null;
    }

    private bool useCache(string section)
    {
      bool flag = true;
      foreach (string strB in SessionObjects.exclusion)
      {
        if (string.Compare(section, strB, true) == 0)
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    public Hashtable GetCompanySettingsFromCache(string section)
    {
      if (this.useCache(section))
      {
        try
        {
          if (this.companySettings == null)
            this.companySettings = new Dictionary<string, Hashtable>();
          if (!this.companySettings.ContainsKey(section))
          {
            Hashtable hashtable = this.ConfigurationManager.GetCompanySettings(section) ?? CollectionsUtil.CreateCaseInsensitiveHashtable();
            this.companySettings[section] = hashtable;
          }
          return this.companySettings[section];
        }
        catch (Exception ex)
        {
          Tracing.Log(SessionObjects.sw, nameof (SessionObjects), TraceLevel.Warning, "Unable to get company settings '" + section + "'. Reason resported = '" + ex.Message + "'.");
        }
      }
      return this.ConfigurationManager.GetCompanySettings(section);
    }

    public bool RefreshTresholdLimitsFromDB
    {
      get => this.refreshTresholdLimitsFromDB;
      set => this.refreshTresholdLimitsFromDB = value;
    }

    public bool RefreshCityStateCache
    {
      get => this.refreshCityStateCache;
      set => this.refreshCityStateCache = value;
    }

    public List<FedTresholdAdjustment> GetThresholdsFromCache()
    {
      if (this.adjustments == null || this.RefreshTresholdLimitsFromDB)
      {
        this.adjustments = ((IEnumerable<FedTresholdAdjustment>) this.ConfigurationManager.GetFedTresholdAdjustments()).ToList<FedTresholdAdjustment>();
        this.RefreshTresholdLimitsFromDB = false;
      }
      return this.adjustments;
    }

    public string GetCompanySettingFromCache(string section, string key)
    {
      try
      {
        Hashtable settingsFromCache = this.GetCompanySettingsFromCache(section);
        return settingsFromCache != null && settingsFromCache.ContainsKey((object) key) ? (string) settingsFromCache[(object) key] : "";
      }
      catch (Exception ex)
      {
        Tracing.Log(SessionObjects.sw, nameof (SessionObjects), TraceLevel.Warning, "Unable to get company setting '" + section + "/" + key + "'. Reason resported = '" + ex.Message + "'.");
      }
      return this.ConfigurationManager.GetCompanySetting(section, key);
    }

    public void FlushCompanySettingsCache()
    {
      this.companySettings = (Dictionary<string, Hashtable>) null;
    }

    public string GetEPassPersonaDescriptor()
    {
      RealWorldRoleIDNameProvider roleIdNameProvider = new RealWorldRoleIDNameProvider();
      ArrayList arrayList = new ArrayList();
      if (this.UserInfo.IsAdministrator())
        arrayList.Add((object) "Admin");
      foreach (RolesMappingInfo roleMapping in this.startupInfo.RoleMappings)
      {
        if (roleMapping.RoleIDList != null)
        {
          foreach (int roleId in roleMapping.RoleIDList)
          {
            foreach (RoleSummaryInfo allowedRole in this.startupInfo.AllowedRoles)
            {
              if (allowedRole.RoleID == roleId)
              {
                string name = roleIdNameProvider.GetName((object) roleMapping.RealWorldRoleID);
                if (!arrayList.Contains((object) name))
                {
                  arrayList.Add((object) name);
                  break;
                }
                break;
              }
            }
          }
        }
      }
      if (arrayList.Count == 0)
        arrayList.Add((object) "Other");
      return string.Join(" + ", (string[]) arrayList.ToArray(typeof (string)));
    }

    public UserLicenseInfo GetCachedUserLicense()
    {
      if (this.startupInfo.FastLoanLoad)
      {
        if (this.cachedUserLicense == null)
          this.cachedUserLicense = this.StartupInfo.UserLicense;
        Tracing.Log(SessionObjects.sw, TraceLevel.Info, nameof (SessionObjects), "GetCachedUserLicense uses FastLoanLoad flag, and get user license from startupInfo");
      }
      else if (this.cachedUserLicense == null)
        this.cachedUserLicense = this.currentUser.GetUserLicense();
      return this.cachedUserLicense;
    }

    public ExternalLateFeeSettings GetCachedExternalOrgLateFeeSettings(string externalTPOCompanyID)
    {
      if (!this.cachedExternalLateFeeSettings.ContainsKey(externalTPOCompanyID))
        this.cachedExternalLateFeeSettings[externalTPOCompanyID] = this.ConfigurationManager.GetExternalOrgLateFeeSettings(externalTPOCompanyID, true);
      return this.cachedExternalLateFeeSettings[externalTPOCompanyID];
    }

    public Task CacheMileStonePermissionsTask { get; set; }

    public void GetCachedMilestonePermissions()
    {
      if (this.cachedMilestonePermissions != null)
        return;
      IMilestonesAclManager aclManager = (IMilestonesAclManager) this.GetAclManager(AclCategory.Milestones);
      this.cachedMilestonePermissions = new Dictionary<string, Hashtable>();
      AclMilestone[] features = new AclMilestone[System.Enum.GetValues(typeof (AclMilestone)).Length];
      int index = 0;
      foreach (string name in System.Enum.GetNames(typeof (AclMilestone)))
      {
        AclMilestone aclMilestone = (AclMilestone) System.Enum.Parse(typeof (AclMilestone), name);
        features[index] = aclMilestone;
        ++index;
      }
      this.cachedMilestonePermissions = aclManager.GetPermissions(this.startupInfo.Milestones, features, this.UserInfo);
    }

    public Task CacheBusinessCalendarTask { get; set; }

    public void PopulateCalendarInCache()
    {
      List<BusinessCalendar> businessCalendars = this.ConfigurationManager.GetAllBusinessCalendars();
      foreach (CalendarType key in System.Enum.GetValues(typeof (CalendarType)))
      {
        foreach (BusinessCalendar businessCalendar in businessCalendars)
        {
          if (key == (CalendarType) businessCalendar.CalendarID && !this.cachedBusinessCalendar.ContainsKey(key))
            this.cachedBusinessCalendar[key] = businessCalendar;
        }
      }
    }

    public void StartAsyncGetEpassMessagesTask(string loanGuid, string userId)
    {
      this.asyncGetEpassMessagesTask = Task.Run<EPassMessageInfo[]>((Func<EPassMessageInfo[]>) (() => this.ConfigurationManager.GetEPassMessagesForLoan(loanGuid, userId)));
    }

    public EPassMessageInfo[] GetEpassMessagesForCurrentLoan(string loanGuid, string userId)
    {
      if (this.asyncGetEpassMessagesTask == null)
        return this.ConfigurationManager.GetEPassMessagesForLoan(loanGuid, userId);
      EPassMessageInfo[] result = this.asyncGetEpassMessagesTask.Result;
      this.asyncGetEpassMessagesTask = (Task<EPassMessageInfo[]>) null;
      return result;
    }

    public Hashtable GetCachedTitleEscrowTables()
    {
      if (this.titleEscrowTables == null)
        this.titleEscrowTables = this.ConfigurationManager.GetSystemSettings(new string[4]
        {
          "TblEscrowPurList",
          "TblEscrowRefiList",
          "TblTitlePurList",
          "TblTitleRefiList"
        });
      return this.titleEscrowTables;
    }

    public Hashtable GetCachedCityStateUserTables()
    {
      if (this.cityStateUserTables == null || this.RefreshCityStateCache)
      {
        this.cityStateUserTables = this.ConfigurationManager.GetSystemSettings(new string[3]
        {
          "FeeCityList",
          "FeeStateList",
          "FeeUserList"
        });
        this.RefreshCityStateCache = false;
      }
      return this.cityStateUserTables;
    }

    public List<HMDAProfile> GetCachedHMDAProfile()
    {
      if (this.hmdaProfiles == null)
        this.hmdaProfiles = this.ConfigurationManager.GetHMDAProfile();
      return this.hmdaProfiles;
    }

    public FieldSettings GetCachedFieldSettings()
    {
      lock (this)
      {
        if (this.cachedFieldSettings == null)
          this.cachedFieldSettings = this.LoanManager.GetFieldSettings();
        return this.cachedFieldSettings;
      }
    }

    public void ClearCachedFieldSettings() => this.cachedFieldSettings = (FieldSettings) null;

    public RoleInfo[] GetCachedAllRoleFunctions()
    {
      if (this.cachedRoles == null)
        this.cachedRoles = this.BpmManager.GetAllRoleFunctions();
      return this.cachedRoles;
    }

    public BusinessCalendar GetBusinessCalendar(CalendarType type)
    {
      if (this.CacheBusinessCalendarTask != null && this.CacheBusinessCalendarTask.Status == TaskStatus.Running)
        this.CacheBusinessCalendarTask.Wait();
      return this.cachedBusinessCalendar.ContainsKey(type) ? this.cachedBusinessCalendar[type] : (this.cachedBusinessCalendar[type] = this.ConfigurationManager.GetBusinessCalendar(type));
    }

    public bool GetPolicySetting(string setting)
    {
      return string.Equals(this.GetCompanySettingsFromCache("POLICIES")[(object) setting] as string, "true", StringComparison.CurrentCultureIgnoreCase);
    }

    public URLAPageNumbering URLAPageNumbering => this._URLAPageNumbering;

    public Task CacheWorkflowTaskGroupTemplatesTask { get; set; }

    public void PopulateWorkflowTaskGroupTemplatesInCache(Hashtable htWorkFlowTaskGroupTemplates)
    {
      this.cachedWorkflowTaskGroupTemplates = htWorkFlowTaskGroupTemplates;
    }

    public Hashtable CachedWorkflowTaskGroupTemplates
    {
      get
      {
        if (this.CacheWorkflowTaskGroupTemplatesTask != null && this.CacheWorkflowTaskGroupTemplatesTask.Status == TaskStatus.Running)
          this.CacheWorkflowTaskGroupTemplatesTask.Wait();
        return this.cachedWorkflowTaskGroupTemplates;
      }
    }

    public Task CacheWorkflowTaskTemplatesTask { get; set; }

    public void PopulateWorkflowTaskTemplatesInCache(Hashtable htWorkFlowTaskTemplates)
    {
      this.cachedWorkflowTaskTemplates = htWorkFlowTaskTemplates;
    }

    public Hashtable CachedWorkflowTaskTemplates
    {
      get
      {
        if (this.CacheWorkflowTaskTemplatesTask != null && this.CacheWorkflowTaskTemplatesTask.Status == TaskStatus.Running)
          this.CacheWorkflowTaskTemplatesTask.Wait();
        return this.cachedWorkflowTaskTemplates;
      }
    }

    public string GetHomeCounseling(string url, IWin32Window owner, bool triggeredFromCalc = false)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      httpWebRequest.Method = "GET";
      ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol;
      StringBuilder stringBuilder = new StringBuilder();
      HttpWebResponse httpWebResponse = (HttpWebResponse) null;
      Stream stream = (Stream) null;
      StreamReader streamReader = (StreamReader) null;
      try
      {
        httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();
        if (httpWebResponse.StatusCode == HttpStatusCode.OK)
        {
          stream = httpWebResponse.GetResponseStream();
          streamReader = new StreamReader(stream);
          char[] buffer = new char[1024];
          for (int length = streamReader.Read(buffer, 0, buffer.Length); length > 0; length = streamReader.Read(buffer, 0, buffer.Length))
          {
            string str = new string(buffer, 0, length);
            stringBuilder.Append(str);
          }
        }
        else if (!triggeredFromCalc)
          this.checkReponseCode(owner, httpWebResponse.StatusCode, string.Empty);
      }
      catch (Exception ex)
      {
        if (!triggeredFromCalc)
        {
          if (httpWebResponse != null)
          {
            this.checkReponseCode(owner, httpWebResponse.StatusCode, ex.Message);
          }
          else
          {
            int num = (int) Utils.Dialog(owner, "HUD Web Service is down. Please try again later. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
      finally
      {
        streamReader?.Close();
        stream?.Close();
        httpWebResponse?.Close();
      }
      return stringBuilder.ToString();
    }

    private void checkReponseCode(IWin32Window owner, HttpStatusCode status, string additionalMsg)
    {
      string str = string.Empty;
      switch (status)
      {
        case HttpStatusCode.Accepted:
          str = "You can retry your request, and when it's complete, you'll get a 200 instead.";
          break;
        case HttpStatusCode.BadRequest:
          str = "Encompass cannot find an agency. Bad request!";
          break;
        case HttpStatusCode.Unauthorized:
          str = "You're not authorized to access this resource!";
          break;
        case HttpStatusCode.NotFound:
          str = "The resource requested doesn't exist!";
          break;
        case HttpStatusCode.InternalServerError:
          str = "Server errors!";
          break;
      }
      int num = (int) Utils.Dialog(owner, "Encompass cannot get agency. Error: " + (str != string.Empty ? str : (additionalMsg != string.Empty ? additionalMsg : "")), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    public List<List<string[]>> ParseHomeCounselingResults(string results)
    {
      string str1 = results;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      List<List<string[]>> counselingResults = new List<List<string[]>>();
      List<string[]> strArrayList1 = new List<string[]>();
      while (str1.Length > 0)
      {
        int num1 = str1.IndexOf("{");
        int num2 = str1.IndexOf("}");
        if (num1 != -1 && num2 != -1)
        {
          string str2 = str1.Substring(num1 + 1, num2 - num1 - 1).Replace(":null", ":\"null\"");
          str1 = str1.Substring(num2 + 2);
          List<string[]> strArrayList2 = new List<string[]>();
          int num3;
          for (; str2.Length > 0; str2 = str2.Substring(num3 + 1))
          {
            int num4 = str2.IndexOf("\"");
            int num5 = str2.IndexOf("\":\"", num4 + 1);
            num3 = str2.IndexOf("\"", num5 + 3);
            if (num4 != -1 && num5 != -1 && num3 != -1)
            {
              string str3 = str2.Substring(num4 + 1, num5 - num4 - 1);
              string str4 = str2.Substring(num5 + 3, num3 - num5 - 3);
              if (str4 == "null" || str4 == " ")
                str4 = "";
              strArrayList2.Add(new string[2]{ str3, str4 });
            }
            else
              break;
          }
          counselingResults.Add(strArrayList2);
        }
        else
          break;
      }
      return counselingResults;
    }

    public string translateServiceLanguageCodes(
      string codeKey,
      bool forService,
      List<KeyValuePair<string, string>> serviceList,
      List<KeyValuePair<string, string>> languageList)
    {
      string[] keys = (string[]) null;
      if (codeKey.IndexOf(",") > -1)
        keys = codeKey.Split(',');
      else
        keys = new string[1]{ codeKey };
      List<KeyValuePair<string, string>> keyValuePairList = forService ? serviceList : languageList;
      string str1 = string.Empty;
      string empty = string.Empty;
      for (int i = 0; i < keys.Length; ++i)
      {
        string str2 = keyValuePairList.Find((Predicate<KeyValuePair<string, string>>) (x => x.Key == keys[i])).Value;
        if (str2 == string.Empty)
          str2 = keys[i];
        str1 = str1 + (str1 != string.Empty ? (forService ? "|" : ",") : "") + str2;
      }
      return str1;
    }
  }
}
