// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.SessionStartupInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Loan;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class SessionStartupInfo : ISessionStartupInfo
  {
    public SessionStartupInfo() => this.RuntimeEnvironment = RuntimeEnvironment.Default;

    public CompanyInfo CompanyInfo { get; set; }

    public LicenseInfo ServerLicense { get; set; }

    public EncompassSystemInfo SystemInfo { get; set; }

    public RuntimeEnvironment RuntimeEnvironment { get; set; }

    public string SessionID { get; set; }

    public string ServerID { get; set; }

    public string ServerInstanceName { get; set; }

    public PluginInfo[] Plugins { get; set; }

    public PreauthorizedModule[] PreauthorizedModules { get; set; }

    public ClientAppVersion AuthorizedVersion { get; set; }

    public string DbVersion { get; set; }

    public bool AllowDDM { get; set; }

    public bool RevertPluginChanges { get; set; }

    public bool EnableDDMPerformance { get; set; }

    public bool AllowAutoEConsent { get; set; }

    public bool AllowAutoDisclosure { get; set; }

    public bool AllowDRSBarCoding { get; set; }

    public bool HasAIQLicense { get; set; }

    public string AIQSiteID { get; set; }

    public string AIQBaseAddress { get; set; }

    public bool AllowLoanErrorInfo { get; set; }

    public bool AllowInsightsSetup { get; set; }

    public bool SendBusinessRuleErrorsToServer { get; set; }

    public bool AllowDataAndDocs { get; set; }

    public bool AllowDataAndDocsReview { get; set; }

    public bool AllowMileStoneAdjustDateLog { get; set; }

    public bool EnableCoC { get; set; }

    public bool UsePipelineTabStartup { get; set; }

    public bool EnableWorkFlowTasks { get; set; }

    public bool ShowUIWorkflowTasksTools { get; set; }

    public bool AllowNotifications { get; set; }

    public bool Correspondent { get; set; }

    public bool EnableTempBuyDown { get; set; }

    public bool EnableESignDocuments { get; set; }

    public bool AllowURLA2020 { get; set; }

    public bool OtpSupport { get; set; }

    public bool EnableSendDisclosureSmartClient { get; set; }

    public bool EnableSendDisclosureENCW { get; set; }

    public bool EnableSendDisclosureTPOC { get; set; }

    public bool UseBackgroundConversion { get; set; }

    public bool FastLoanLoad { get; set; }

    public bool AllowWCRouting { get; set; }

    public bool EnableAutoRetrieveSettings { get; set; }

    public string ViewerUrl { get; set; }

    public string eCloseUrl { get; set; }

    public string eConsentUrl { get; set; }

    public string eDisclosuresUrl { get; set; }

    public string eNoteUrl { get; set; }

    public string eSignPackagesUrl { get; set; }

    public string NotificationTemplatesUrl { get; set; }

    public bool EnableTriggerToRunEscrowDateCalc { get; set; }

    public SupportingDataSource SupportingDataSource { get; set; }

    public bool EnableMFA { get; set; }

    public bool SkipCustomCalcsExecutionOnLoanOpen { get; set; }

    public bool EnableSSO { get; set; }

    public bool IsWebLoginEnabled { get; set; }

    public bool EnhancedConditionSettings { get; set; }

    public bool EnhancedConditionsWorkflow { get; set; }

    public IDictionary ComponentSettings { get; set; }

    public IDictionary EFolderSettings { get; set; }

    public IDictionary PolicySettings { get; set; }

    public IDictionary PrintSettings { get; set; }

    public IDictionary UnpublishedSettings { get; set; }

    public IDictionary LicenseSettings { get; set; }

    public IDictionary TradeSettings { get; set; }

    public RolesMappingInfo[] RoleMappings { get; set; }

    public BizRuleInfo[] ActiveRules { get; set; }

    public AlertConfig[] AlertConfigs { get; set; }

    public List<FieldRuleInfo> MilestoneTemplate { get; set; }

    public List<EllieMae.EMLite.Workflow.Milestone> Milestones { get; set; }

    public UserInfo UserInfo { get; set; }

    public OrgInfo OrgInfo { get; set; }

    public OrgInfo BranchInfo { get; set; }

    public UserLicenseInfo UserLicense { get; set; }

    public IDictionary UserProfileSettings { get; set; }

    public bool IsPasswordChangeRequired { get; set; }

    public IMControlMessage[] StoredIMMessages { get; set; }

    public bool DataServicesOptOut { get; set; }

    public RoleInfo[] AllowedRoles { get; set; }

    public ConditionTrackingView DefaultPreliminaryConditionTrackingView { get; set; }

    public ConditionTrackingView DefaultUnderwritingConditionTrackingView { get; set; }

    public ConditionTrackingView DefaultPostClosingConditionTrackingView { get; set; }

    public ConditionTrackingView DefaultSellConditionTrackingView { get; set; }

    public ConditionTrackingView DefaultEnhancedConditionTrackingView { get; set; }

    public LoanFolderAclInfo[] AccessibleFoldersForMove { get; set; }

    public LoanFolderAclInfo[] AccessibleFoldersForImport { get; set; }

    public Hashtable UserAclFeatureRights { get; set; }

    public Dictionary<string, Hashtable> UserAclEnhancedConditionRights { get; set; }

    public Dictionary<AclFeature, int> UserAclFeaturConfigRights { get; set; }

    public IDictionary ServerObjects { get; set; }

    public IDictionary AclObjects { get; set; }

    public ICurrentUser CurrentUser { get; set; }

    public ProductPricingSetting ProductPricingPartner { get; set; }

    public string OAPIGatewayBaseUri { get; set; }

    public string RestApiBaseUriKey { get; set; }

    public string FsApiBaseUriKey { get; set; }

    public int AccessTokenBaseDelay { get; set; }

    public int AccessTokenMaxDelay { get; set; }

    public int AccessTokenTimeOut { get; set; }

    public string SSFClientId { get; set; }

    public string SSFClientSecret { get; set; }

    public string CustomLoanDeliveryPageURL { get; set; }

    public string CustomLoanSubmissionStatusPageURL { get; set; }

    public string CustomImportConditionsPageURL { get; set; }

    public string InsightsSetupUrl { get; set; }

    public bool AllowDetailedPerfMeter { get; set; }

    public string SSOGuestLoginUrl { get; set; }

    public string Epc2HostAdapterUrl { get; set; }

    public double DocsPerformancePublishingRate { get; set; }

    public string OAuthClientId { get; set; }

    public string OAuthEncWebClientId { get; set; }

    public string OAuthClientSecret { get; set; }

    public bool CollectLanguagePreference { get; set; }

    public bool CollectHomeownershipCounseling { get; set; }

    public bool EnableTradeLoanUpdateNotification { get; set; }

    public IList<LockComparisonField> LockComparisonFields { get; set; } = (IList<LockComparisonField>) new List<LockComparisonField>();

    public string ResourceCenterUrl { get; set; }

    public string InvestorConnectAppUrl { get; set; }

    public string WorkflowTasksPageUrl { get; set; }

    public string LOConnectUrl { get; set; }

    public string AllRegsUrl { get; set; }

    public bool IsUserPipelineViewFromDB { get; set; }

    public ServiceUrls ServiceUrls { get; set; }

    public bool EnableLoanSoftArchival { get; set; }

    public bool UpdateXDBBasedOnFieldChanges { get; set; }

    public bool SkipLogInitializationForLoanCalculatorClone { get; set; }

    public string WebhookEventXapiKey { get; set; }

    public bool UseFullProjectPaymentTriggers { get; set; }

    public IDictionary ClientSettings { get; set; }

    public bool EnableConcurrentLoanEditing { get; set; }
  }
}
