// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.Session
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class Session
  {
    private const string className = "Session";
    private static readonly string sw = Tracing.SwRemoting;
    private static Dictionary<string, string> companySettingCache = new Dictionary<string, string>();
    private static Dictionary<int, bool> tpoHierarchyAccessCache = new Dictionary<int, bool>();
    public static bool stopAutoReconnect;
    public static bool HideUI = false;

    public static event EventHandler WriteDTLog;

    internal static void OnWriteDTLog(object sender, EventArgs args)
    {
      if (Session.WriteDTLog == null)
        return;
      Session.WriteDTLog(sender, args);
    }

    public static Dictionary<int, bool> TpoHierarchyAccessCache
    {
      get => Session.tpoHierarchyAccessCache;
      set => Session.tpoHierarchyAccessCache = value;
    }

    public static void AddTpoHierarchyAccessCache(int oid, bool access)
    {
      if (Session.tpoHierarchyAccessCache.ContainsKey(oid))
        Session.tpoHierarchyAccessCache[oid] = access;
      else
        Session.tpoHierarchyAccessCache.Add(oid, access);
    }

    public static void clearTpoCache()
    {
      Session.tpoHierarchyAccessCache = new Dictionary<int, bool>();
    }

    public static event EventHandler Started;

    internal static void OnStarted(object sender, EventArgs args)
    {
      if (Session.Started == null)
        return;
      Session.Started(sender, args);
    }

    public static event EventHandler Ended;

    internal static void OnEnded(object sender, EventArgs args)
    {
      if (Session.Ended == null)
        return;
      Session.Ended(sender, args);
    }

    public static event EventHandler ApplicationReady;

    internal static void OnApplicationReady(object sender, EventArgs args)
    {
      if (Session.ApplicationReady == null)
        return;
      Session.ApplicationReady(sender, args);
    }

    public static event EventHandler LoanOpened;

    internal static void OnLoanOpened(object sender, EventArgs args)
    {
      if (Session.LoanOpened == null)
        return;
      Session.LoanOpened(sender, args);
    }

    public static event EventHandler LoanClosing;

    internal static void OnLoanClosing(object sender, EventArgs args)
    {
      if (Session.LoanClosing == null)
        return;
      Session.LoanClosing(sender, args);
    }

    public static event EventHandler FormLoaded;

    internal static void OnFormLoaded(object sender, EventArgs args)
    {
      if (Session.FormLoaded == null)
        return;
      Session.FormLoaded(sender, args);
    }

    public static event EventHandler FormUnloading;

    internal static void OnFormUnloading(object sender, EventArgs args)
    {
      if (Session.FormUnloading == null)
        return;
      Session.FormUnloading(sender, args);
    }

    public static event CacheControlEventHandler CacheControl;

    internal static void OnCacheControlEventHandlerReceived(
      object sender,
      CacheControlEventArgs args)
    {
      if (Session.CacheControl == null)
        return;
      Session.CacheControl(sender, args);
    }

    public static bool ExitOnDisconnect
    {
      set => Session.DefaultInstance.ExitOnDisconnect = value;
      get => Session.DefaultInstance == null || Session.DefaultInstance.ExitOnDisconnect;
    }

    public static Hashtable StandardFormCodebaseTypes
    {
      get
      {
        return Session.DefaultInstance != null ? Session.DefaultInstance.StandardFormCodebaseTypes : throw new Exception("Default session has not started yet");
      }
    }

    private Session()
    {
    }

    public static Sessions.Session DefaultInstance => Sessions.GetSession();

    public static ISession ISession => Session.DefaultInstance.ISession;

    public static void Start(string serverUri, string userId, string password, string appName)
    {
      Session.Start(serverUri, userId, password, appName, true, (string) null);
    }

    public static void Start(
      string serverUri,
      string userId,
      string password,
      string appName,
      string prevSessionID)
    {
      Session.Start(serverUri, userId, password, appName, true, prevSessionID);
    }

    public static void Start(
      string serverUri,
      string userId,
      string password,
      string appName,
      bool licenseRequired)
    {
      Session.Start(serverUri, userId, password, appName, licenseRequired, (string) null);
    }

    public static void Start(
      string serverUri,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      string prevSessionID)
    {
      Sessions.Session session = Sessions.GetSession(true);
      session.End();
      session.Start(serverUri, userId, password, appName, licenseRequired, prevSessionID);
    }

    public static void Start(
      string serverUri,
      string userId,
      string password,
      string appName,
      bool licenseRequired,
      string prevSessionID,
      string authCode)
    {
      Sessions.Session session = Sessions.GetSession(true);
      session.End();
      session.Start(serverUri, userId, password, appName, licenseRequired, prevSessionID, authCode);
    }

    public static void Start(string userId, string password, string appName)
    {
      Session.Start(userId, password, appName, true);
    }

    public static void Start(string userId, string password, string appName, bool licenseRequired)
    {
      Sessions.Session session = Sessions.GetSession(true);
      session.End();
      session.Start(userId, password, appName, licenseRequired);
    }

    public static string EncompassSystemID => Session.DefaultInstance.EncompassSystemID;

    public static void SetConnection(IConnection newConnection, string userPassword)
    {
      Sessions.Session session = Sessions.GetSession(true);
      session.End();
      session.SetConnection(newConnection, userPassword);
    }

    public static void End() => Session.DefaultInstance?.End();

    public static bool IsConnected
    {
      get => Session.DefaultInstance != null && Session.DefaultInstance.IsConnected;
    }

    public static string Password => Session.DefaultInstance.Password;

    public static IConnection Connection => Session.DefaultInstance.Connection;

    public static DateTime ServerTime => Session.DefaultInstance.ServerTime;

    public static DateTime ServerRealTime => Session.DefaultInstance.ServerRealTime;

    public static string UserID => Session.DefaultInstance.UserID;

    public static UserInfo UserInfo => Session.DefaultInstance.UserInfo;

    public static UserInfo RecacheUserInfo() => Session.DefaultInstance.RecacheUserInfo();

    public static CompanyInfo CompanyInfo => Session.DefaultInstance.CompanyInfo;

    public static CompanyInfo RecacheCompanyInfo() => Session.DefaultInstance.RecacheCompanyInfo();

    public static Hashtable InitialDataServicesData
    {
      get => Session.DefaultInstance.InitialDataServicesData;
      set => Session.DefaultInstance.InitialDataServicesData = value;
    }

    public static ServerIdentity ServerIdentity => Session.DefaultInstance.ServerIdentity;

    public static string RemoteServer => Session.DefaultInstance.RemoteServer;

    public static void ChangePassword(string newPassword)
    {
      Session.DefaultInstance.ChangePassword(newPassword);
    }

    public static string WorkingFolder
    {
      get => Session.DefaultInstance.WorkingFolder;
      set => Session.DefaultInstance.WorkingFolder = value;
    }

    public static EncompassEdition EncompassEdition => Session.DefaultInstance.EncompassEdition;

    public static bool IsBankerEdition() => Session.DefaultInstance.IsBankerEdition();

    public static bool IsBrokerEdition() => Session.DefaultInstance.IsBrokerEdition();

    public static ICurrentUser User => Session.DefaultInstance.User;

    public static IEncERDBRegMgr EncERDBRegMgr => Session.DefaultInstance.EncERDBRegMgr;

    public static IConfigurationManager ConfigurationManager
    {
      get => Session.DefaultInstance.ConfigurationManager;
    }

    public static IEfolderDocTrackViewManager EfolderDocTrackViewManager
    {
      get => Session.DefaultInstance.EfolderDocTrackViewManager;
    }

    public static IContactManager ContactManager => Session.DefaultInstance.ContactManager;

    public static IAclGroupManager AclGroupManager => Session.DefaultInstance.AclGroupManager;

    public static IContactGroup ContactGroupManager => Session.DefaultInstance.ContactGroupManager;

    public static IFormManager FormManager => Session.DefaultInstance.FormManager;

    public static ILoanManager LoanManager => Session.DefaultInstance.LoanManager;

    public static IReportManager ReportManager => Session.DefaultInstance.ReportManager;

    public static IOrganizationManager OrganizationManager
    {
      get => Session.DefaultInstance.OrganizationManager;
    }

    public static IServerManager ServerManager => Session.DefaultInstance.ServerManager;

    public static SettingsManager SettingsManager => Session.DefaultInstance.SettingsManager;

    public static IMessengerListManager MessengerListManager
    {
      get => Session.DefaultInstance.MessengerListManager;
    }

    public static IAlertManager AlertManager => Session.DefaultInstance.AlertManager;

    public static IWelcomeScreenSettingMgr WelcomeScreenSettingMgr
    {
      get => Session.DefaultInstance.WelcomeScreenSettingMgr;
    }

    public static ICalendarManager CalendarManager => Session.DefaultInstance.CalendarManager;

    public static ICampaign CampaignManager => Session.DefaultInstance.CampaignManager;

    public static IMasterContractManager MasterContractManager
    {
      get => Session.DefaultInstance.MasterContractManager;
    }

    public static ISecurityTradeManager SecurityTradeManager
    {
      get => Session.DefaultInstance.SecurityTradeManager;
    }

    public static ILoanTradeManager LoanTradeManager => Session.DefaultInstance.LoanTradeManager;

    public static IMbsPoolManager MbsPoolManager => Session.DefaultInstance.MbsPoolManager;

    public static ICorrespondentTradeManager CorrespondentTradeManager
    {
      get => Session.DefaultInstance.CorrespondentTradeManager;
    }

    public static ILockComparisonFieldManager LockComparisonFieldManager
    {
      get => Session.DefaultInstance.LockComparisonFieldManager;
    }

    public static IGseCommitmentManager GseCommitmentManager
    {
      get => Session.DefaultInstance.GseCommitmentManager;
    }

    public static IBatchJobsManager BatchJobsManager => Session.DefaultInstance.BatchJobsManager;

    public static ITradeSynchronizationManager TradeSynchronizationManager
    {
      get => Session.DefaultInstance.TradeSynchronizationManager;
    }

    public static ITradeLoanUpdateQueueManager TradeLoanUpdateQueueManager
    {
      get => Session.DefaultInstance.TradeLoanUpdateQueueManager;
    }

    public static IPersonaManager PersonaManager => Session.DefaultInstance.PersonaManager;

    public static IIdentityManager IdentityManager => Session.DefaultInstance.IdentityManager;

    public static ICorrespondentMasterManager CorrespondentMasterManager
    {
      get => Session.DefaultInstance.CorrespondentMasterManager;
    }

    public static ILoanSummaryExtensionManager LoanSummaryExtensionManager
    {
      get => Session.DefaultInstance.LoanSummaryExtensionManager;
    }

    public static IOverNightRateProtectionManager OverNightRateProtectionManager
    {
      get => Session.DefaultInstance.OverNightRateProtectionManager;
    }

    public static ACL ACL => Session.DefaultInstance.ACL;

    public static BPM BPM => Session.DefaultInstance.BPM;

    public static LoanDataMgr LoanDataMgr => Session.DefaultInstance.LoanDataMgr;

    public static void SetLoanDataMgr(LoanDataMgr val)
    {
      Session.DefaultInstance.SetLoanDataMgr(val, false);
    }

    public static void SetLoanDataMgr(LoanDataMgr val, bool autoSave)
    {
      Session.DefaultInstance.SetLoanDataMgr(val, autoSave);
    }

    public static LoanData LoanData => Session.DefaultInstance.LoanData;

    public static SessionObjects SessionObjects => Session.DefaultInstance?.SessionObjects;

    public static Form MainForm
    {
      get => Session.DefaultInstance.MainForm;
      set => Session.DefaultInstance.MainForm = value;
    }

    public static IMainScreen MainScreen
    {
      get => Session.DefaultInstance.MainScreen;
      set => Session.DefaultInstance.MainScreen = value;
    }

    public static ISetup Setup
    {
      get => Session.DefaultInstance.Setup;
      set => Session.DefaultInstance.Setup = value;
    }

    public static IApplicationScreen Application
    {
      get => Session.DefaultInstance.Application;
      set => Session.DefaultInstance.Application = value;
    }

    public static object GetSystemSettings(System.Type settingsType)
    {
      return Session.DefaultInstance.GetSystemSettings(settingsType);
    }

    public static void SaveSystemSettings(object settings)
    {
      Session.DefaultInstance.SaveSystemSettings(settings);
    }

    internal static object GetAclManager(AclCategory category)
    {
      return Session.DefaultInstance.GetAclManager(category);
    }

    public static object GetComponentSetting(string componentName, object defaultSetting)
    {
      return Session.DefaultInstance.GetComponentSetting(componentName, defaultSetting);
    }

    public static object GetComponentSetting(string componentName)
    {
      return Session.DefaultInstance.GetComponentSetting(componentName);
    }

    public static object GetPrintingSetting(string settingName, object defaultValue)
    {
      return Session.DefaultInstance.GetPrintingSetting(settingName, defaultValue);
    }

    public static object GetPrintingSetting(string settingName)
    {
      return Session.DefaultInstance.GetPrintingSetting(settingName);
    }

    public static string GetFormConfigFile(FormConfigFile fileType)
    {
      return Session.DefaultInstance.GetFormConfigFile(fileType);
    }

    public static string GetPrivateProfileString(string section, string key)
    {
      return Session.DefaultInstance.GetPrivateProfileString(section, key);
    }

    public static string GetPrivateProfileString(string path)
    {
      return Session.DefaultInstance.GetPrivateProfileString(path);
    }

    public static void WritePrivateProfileString(string section, string key, string value)
    {
      Session.DefaultInstance.WritePrivateProfileString(section, key, value);
    }

    public static void WritePrivateProfileString(string path, string value)
    {
      Session.DefaultInstance.WritePrivateProfileString(path, value);
    }

    public static UserLicenseInfo GetUserLicense() => Session.DefaultInstance.GetUserLicense();

    public static void UpdateUserLicense(UserLicenseInfo license)
    {
      Session.DefaultInstance.UpdateUserLicense(license);
    }

    public static ISessionStartupInfo StartupInfo => Session.DefaultInstance?.StartupInfo;

    public static LicenseInfo ServerLicense => Session.DefaultInstance.ServerLicense;

    public static void RefreshServerLicense() => Session.DefaultInstance.RefreshServerLicense();

    public static void NotifyApplicationReady() => Session.DefaultInstance.NotifyApplicationReady();

    public static string GetEPassPersonaDescriptor()
    {
      return Session.DefaultInstance.GetEPassPersonaDescriptor();
    }

    public static void InitializePlugins() => Session.DefaultInstance.InitializePlugins();

    public static void InvokeFormLoaded(object formObject)
    {
      Session.DefaultInstance.InvokeFormLoaded(formObject);
    }

    public static void InvokeFormUnloading(object formObject)
    {
      Session.DefaultInstance.InvokeFormUnloading(formObject);
    }

    public static void InsertSystemAuditRecord(SystemAuditRecord record)
    {
      Session.DefaultInstance.InsertSystemAuditRecord(record);
    }

    public static SystemAuditRecord[] GetAuditRecord(
      string userID,
      ActionType actionType,
      DateTime startTime,
      DateTime endTime,
      string objectID,
      string objectName)
    {
      return Session.DefaultInstance.GetAuditRecord(userID, actionType, startTime, endTime, objectID, objectName);
    }
  }
}
