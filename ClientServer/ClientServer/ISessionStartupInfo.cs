// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ISessionStartupInfo
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
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ISessionStartupInfo
  {
    CompanyInfo CompanyInfo { get; set; }

    LicenseInfo ServerLicense { get; set; }

    EncompassSystemInfo SystemInfo { get; set; }

    RuntimeEnvironment RuntimeEnvironment { get; set; }

    string SessionID { get; set; }

    string ServerID { get; set; }

    string ServerInstanceName { get; set; }

    PluginInfo[] Plugins { get; set; }

    PreauthorizedModule[] PreauthorizedModules { get; set; }

    ClientAppVersion AuthorizedVersion { get; set; }

    string DbVersion { get; set; }

    bool AllowDDM { get; set; }

    bool RevertPluginChanges { get; }

    bool EnableDDMPerformance { get; set; }

    bool AllowAutoEConsent { get; set; }

    bool AllowAutoDisclosure { get; set; }

    bool AllowDRSBarCoding { get; set; }

    bool AllowLoanErrorInfo { get; set; }

    bool AllowInsightsSetup { get; set; }

    bool SendBusinessRuleErrorsToServer { get; set; }

    bool AllowDataAndDocs { get; set; }

    bool AllowDataAndDocsReview { get; set; }

    bool AllowMileStoneAdjustDateLog { get; set; }

    bool EnableCoC { get; set; }

    bool UsePipelineTabStartup { get; set; }

    bool EnableWorkFlowTasks { get; set; }

    bool ShowUIWorkflowTasksTools { get; set; }

    bool AllowNotifications { get; set; }

    bool Correspondent { get; set; }

    bool EnableTempBuyDown { get; set; }

    bool EnableESignDocuments { get; set; }

    bool AllowURLA2020 { get; set; }

    bool OtpSupport { get; set; }

    bool EnableSendDisclosureSmartClient { get; set; }

    bool EnableSendDisclosureENCW { get; set; }

    bool EnableSendDisclosureTPOC { get; set; }

    bool UseBackgroundConversion { get; set; }

    bool AllowWCRouting { get; set; }

    bool EnableAutoRetrieveSettings { get; set; }

    string ViewerUrl { get; set; }

    string eCloseUrl { get; set; }

    string eConsentUrl { get; set; }

    string eDisclosuresUrl { get; set; }

    string eNoteUrl { get; set; }

    string eSignPackagesUrl { get; set; }

    string NotificationTemplatesUrl { get; set; }

    bool EnableTriggerToRunEscrowDateCalc { get; set; }

    bool FastLoanLoad { get; set; }

    SupportingDataSource SupportingDataSource { get; set; }

    bool EnableMFA { get; set; }

    bool SkipCustomCalcsExecutionOnLoanOpen { get; set; }

    bool EnableSSO { get; set; }

    bool IsWebLoginEnabled { get; set; }

    bool EnhancedConditionSettings { get; set; }

    bool EnhancedConditionsWorkflow { get; set; }

    IDictionary ComponentSettings { get; set; }

    IDictionary EFolderSettings { get; set; }

    IDictionary PolicySettings { get; set; }

    IDictionary PrintSettings { get; set; }

    IDictionary UnpublishedSettings { get; set; }

    IDictionary LicenseSettings { get; set; }

    IDictionary TradeSettings { get; set; }

    RolesMappingInfo[] RoleMappings { get; set; }

    BizRuleInfo[] ActiveRules { get; set; }

    AlertConfig[] AlertConfigs { get; set; }

    List<FieldRuleInfo> MilestoneTemplate { get; set; }

    List<EllieMae.EMLite.Workflow.Milestone> Milestones { get; set; }

    UserInfo UserInfo { get; set; }

    OrgInfo OrgInfo { get; set; }

    OrgInfo BranchInfo { get; set; }

    UserLicenseInfo UserLicense { get; set; }

    IDictionary UserProfileSettings { get; set; }

    bool IsPasswordChangeRequired { get; set; }

    IMControlMessage[] StoredIMMessages { get; set; }

    bool DataServicesOptOut { get; set; }

    RoleInfo[] AllowedRoles { get; set; }

    ConditionTrackingView DefaultPreliminaryConditionTrackingView { get; set; }

    ConditionTrackingView DefaultUnderwritingConditionTrackingView { get; set; }

    ConditionTrackingView DefaultPostClosingConditionTrackingView { get; set; }

    ConditionTrackingView DefaultSellConditionTrackingView { get; set; }

    ConditionTrackingView DefaultEnhancedConditionTrackingView { get; set; }

    LoanFolderAclInfo[] AccessibleFoldersForMove { get; set; }

    LoanFolderAclInfo[] AccessibleFoldersForImport { get; set; }

    Hashtable UserAclFeatureRights { get; set; }

    Dictionary<string, Hashtable> UserAclEnhancedConditionRights { get; set; }

    Dictionary<AclFeature, int> UserAclFeaturConfigRights { get; set; }

    IDictionary ServerObjects { get; set; }

    IDictionary AclObjects { get; set; }

    ICurrentUser CurrentUser { get; set; }

    ProductPricingSetting ProductPricingPartner { get; set; }

    string OAPIGatewayBaseUri { get; set; }

    string RestApiBaseUriKey { get; set; }

    string FsApiBaseUriKey { get; set; }

    int AccessTokenBaseDelay { get; set; }

    int AccessTokenMaxDelay { get; set; }

    int AccessTokenTimeOut { get; set; }

    string SSFClientId { get; set; }

    string SSFClientSecret { get; set; }

    string CustomLoanDeliveryPageURL { get; set; }

    string CustomLoanSubmissionStatusPageURL { get; set; }

    string CustomImportConditionsPageURL { get; set; }

    string InsightsSetupUrl { get; }

    bool AllowDetailedPerfMeter { get; }

    string SSOGuestLoginUrl { get; set; }

    string Epc2HostAdapterUrl { get; set; }

    double DocsPerformancePublishingRate { get; set; }

    string OAuthClientId { get; set; }

    string OAuthEncWebClientId { get; set; }

    string OAuthClientSecret { get; set; }

    bool CollectLanguagePreference { get; set; }

    bool CollectHomeownershipCounseling { get; set; }

    bool EnableTradeLoanUpdateNotification { get; set; }

    bool HasAIQLicense { get; set; }

    string AIQSiteID { get; set; }

    string AIQBaseAddress { get; set; }

    string ResourceCenterUrl { get; set; }

    string InvestorConnectAppUrl { get; set; }

    string WorkflowTasksPageUrl { get; set; }

    string LOConnectUrl { get; set; }

    string AllRegsUrl { get; set; }

    bool IsUserPipelineViewFromDB { get; set; }

    ServiceUrls ServiceUrls { get; set; }

    bool UpdateXDBBasedOnFieldChanges { get; set; }

    bool SkipLogInitializationForLoanCalculatorClone { get; set; }

    bool EnableLoanSoftArchival { get; set; }

    string WebhookEventXapiKey { get; set; }

    bool UseFullProjectPaymentTriggers { get; set; }

    IDictionary ClientSettings { get; set; }

    IList<LockComparisonField> LockComparisonFields { get; set; }

    bool EnableConcurrentLoanEditing { get; set; }
  }
}
