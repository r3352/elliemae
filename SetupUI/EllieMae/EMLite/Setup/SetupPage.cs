// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SetupPage
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SetupPage
  {
    internal const string SettingsOverview = "Settings Overview";
    internal const string CompanyUserSetup = "Company/User Setup";
    internal const string CompanyInformation = "Company Information";
    internal const string EpassCompanyPassword = "ICE Mortgage Technology Network Company Password";
    internal const string Personas = "Personas";
    internal const string OrganizationUsers = "Organization/Users";
    internal const string Roles = "Roles";
    internal const string Milestones80 = "Milestones";
    internal const string UserGroups = "User Groups";
    internal const string EPassPasswordManagement = "Services Password Management";
    internal const string ExternalCompanySetup = "External Company Setup";
    internal const string CompanySetup = "Company Details";
    internal const string CompanySettingSetup = "TPO Settings";
    internal const string TPOFees = "TPO Fees";
    internal const string TPOReassignment = "TPO Reassignment";
    internal const string TPOCustomFields = "TPO Custom Fields";
    internal const string TPOAllContactInfo = "All TPO Contact Information";
    internal const string TPOGlobalLenderContact = "TPO Global Lender Contacts";
    internal const string TPOWCDocs = "TPO Docs";
    internal const string TPODisclosureSettings = "TPO Disclosure Settings";
    internal const string TPOConnectSiteMngmnt = "TPO Connect Site Management";
    internal const string TPOWCSiteMngmnt = "TPO WebCenter Site Management";
    internal const string InvestorConnectSetup = "Investor Connect Setup";
    internal const string DeliverLoans = "Deliver Loans";
    internal const string PartnerSetup = "Partner Setup";
    internal const string LoanSetup = "Loan Setup";
    internal const string AutoLoanNumbering = "Auto Loan Numbering";
    internal const string AutoMersMinNumbering = "Auto MERS MIN Numbering";
    internal const string LoanFolders = "Loan Folders";
    internal const string LoanDuplication = "Loan Duplication";
    internal const string Alerts = "Alerts";
    internal const string Log = "Log";
    internal const string ComplianceCalendar = "Compliance Calendar";
    internal const string DefaultInputForms = "Default Input Forms";
    internal const string ConditionalLetterOption = "Condition Forms";
    internal const string CustomPrintForms = "Custom Print Forms";
    internal const string PrintFormGroups = "Print Form Groups";
    internal const string LoanCustomFields = "Loan Custom Fields";
    internal const string ConfigurableWorkFlow = "Configurable Workflow Templates";
    internal const string ChannelOptions = "Channel Options";
    internal const string LockExpirationDate = "Lock Desk Setup";
    internal const string RESPA = "RESPA";
    internal const string ChangeCircumstanceSetup = "Changed Circumstances Setup";
    internal const string DisclosureTracking = "Disclosure Tracking Settings";
    internal const string GfePrint2009 = "2009 GFE Print";
    internal const string TrusteeList = "Trustee List";
    internal const string PiggybackLoanSynchronization = "Piggyback Loan Synchronization";
    internal const string SyncTemplates = "Sync Templates";
    internal const string PrivacyPolicyForm = "Privacy Policy";
    internal const string ZipcodeSetup = "Zipcode Setup";
    internal const string HMDA = "HMDA Profiles";
    internal const string NMLSSetup = "NMLS Report Setup";
    internal const string VerifContactSetup = "Verification Contact Setup";
    internal const string FHAInformedConsumerChoiceDisclosure = "FHA Informed Consumer Choice Disclosure";
    internal const string EfolderSetup = "eFolder Setup";
    internal const string DocumentConversion = "Document Conversion";
    internal const string Documents = "Documents";
    internal const string DocumentExportTemplates = "Document Export Templates";
    internal const string DocumentGroups = "Document Groups";
    internal const string DocumentStackingOrders = "Document Stacking Templates";
    internal const string Barcodes = "Document Identification";
    internal const string DocumentTraining = "Document Training";
    internal const string EnhancedConditions = "Enhanced Conditions";
    internal const string EnhancedConditionSets = "Enhanced Condition Sets";
    internal const string Conditions = "Conditions";
    internal const string ConditionSets = "Condition Sets";
    internal const string PurchaseConditions = "Purchase Conditions";
    internal const string PostClosingConditions = "Post-Closing Conditions";
    internal const string PostClosingConditionSets = "Post-Closing Condition Sets";
    internal const string PurchaseConditionSets = "Purchase Condition Sets";
    internal const string Tasks = "Tasks";
    internal const string HtmlEmailTemplates = "HTML Email Templates";
    internal const string NotificationTemplates = "Notification Templates";
    internal const string WebCenterConfiguration = "WebCenter Configuration";
    internal const string DocServices = "Docs Setup";
    internal const string EDisclosures = "eDisclosure Packages";
    internal const string EDisclosurePlanCodes = "eDisclosure Plan Codes";
    internal const string EDisclosureStackingTemplates = "eDisclosure Stacking Templates";
    internal const string ClosingPlanCodes = "Closing Doc Plan Codes";
    internal const string ClosingStackingTemplates = "Closing Doc Stacking Templates";
    internal const string ComplianceAuditSettings = "Compliance Audit Settings";
    internal const string SecondarySetup = "Secondary Setup";
    internal const string ProductAndPricing = "Product and Pricing";
    internal const string SecondaryLockFields = "Secondary Lock Fields";
    internal const string LockRequestAdditionalFields = "Lock Request Additional Fields";
    internal const string LockComparisonToolFields = "Lock Comparison Tool Fields";
    internal const string InvestorTemplates = "Investor Templates";
    internal const string EPPSLoannProgramTable = "ICE PPE Loan Program Table";
    internal const string TradeManagementFields = "Trade Management Setup";
    internal const string NormalizedBidTapeTemplate = "Normalized Bid Tape Template";
    internal const string AdjustmentTemplates = "Adjustment Templates";
    internal const string SrpTemplates = "SRP Templates";
    internal const string FundingTemplates = "Funding Templates";
    internal const string Servicing = "Servicing";
    internal const string CorrespondentPurchaseAdvice = "Correspondent Purchase Advice";
    internal const string PurchaseAdviceForm = "Purchase Advice Form";
    internal const string LoanPricingDecimalPlaces = "Loan Pricing Decimal Places";
    internal const string AutoLock = "Auto-Lock";
    internal const string ContactSetup = "Contact Setup";
    internal const string BorrowerCustomFields = "Borrower Custom Fields";
    internal const string BorrowerContactStatus = "Borrower Contact Status";
    internal const string BorrowerContactUpdate = "Borrower Contact Update";
    internal const string BusinessCustomFields = "Business Custom Fields";
    internal const string BusinessCategories = "Business Categories";
    internal const string PublicBusinessContactGroups = "Public Business Contact Groups";
    internal const string EmailServerSettings = "Email Server Settings";
    internal const string LoanTemplates = "Loan Templates";
    internal const string LoanPrograms = "Loan Programs";
    internal const string ClosingCosts = "Closing Costs";
    internal const string InputFormSets = "Input Form Sets";
    internal const string SettlementServiceProviders = "Settlement Service Providers";
    internal const string AffiliatedBusinessArrangements = "Affiliated Business Arrangement Templates";
    internal const string DocumentSets = "Document Sets";
    internal const string TaskSets = "Task Sets";
    internal const string DataTemplates = "Data Templates";
    internal const string LoanTemplateSets = "Loan Template Sets";
    internal const string IRS4506TTemplates = "Transcript Request Templates";
    internal const string DefaultTemplateSetting = "Default Template Setting";
    internal const string LoanProgramAdditionalFields = "Loan Program Additional Fields";
    internal const string TablesAndFees = "Tables and Fees";
    internal const string EscrowTable = "Escrow";
    internal const string TitleTable = "Title";
    internal const string HelocTable = "HELOC Table";
    internal const string MiTables = "MI Tables";
    internal const string FhaCountyLimits = "FHA County Limits";
    internal const string ConventionalCountyLimits = "Conventional County Limits";
    internal const string FedThresholdAdjustments = "Federal Threshold Adjustments";
    internal const string AMILimits = "Affordable Lending";
    internal const string CityTax = "City Tax";
    internal const string StateTax = "State Tax";
    internal const string UserDefinedFee = "User Defined Fee";
    internal const string ItemizationFeeManagement = "Itemization Fee Management";
    internal const string LOCompensation = "LO Compensation";
    internal const string TemporaryBuydown = "Temporary Buydown";
    internal const string SpecialFeatureCodes = "Special Feature Codes";
    internal const string BusinessRules = "Business Rules";
    internal const string LoanFolderBusinessRule = "Loan Folder Business Rule";
    internal const string MilestoneCompletion = "Milestone Completion";
    internal const string LoanActionCompletion = "Loan Action Completion";
    internal const string FieldDataEntry = "Field Data Entry";
    internal const string FieldTriggers = "Field Triggers";
    internal const string AutomatedConditions = "Automated Conditions";
    internal const string AutomatedEnhancedConditions = "Automated Enhanced Conditions";
    internal const string PersonaAccessToFields = "Persona Access to Fields";
    internal const string PersonaAccessToLoans = "Persona Access to Loans";
    internal const string PersonaAccessToLoanActions = "Persona Access to Loan Actions";
    internal const string RoleAccessToDocuments = "Role Access to Documents";
    internal const string InputFormList = "Input Form List";
    internal const string LoanFormPrinting = "Loan Form Printing";
    internal const string PrintAutoSelection = "Print Auto Selection";
    internal const string LOCompensationControl = "LO Compensation Rule";
    internal const string DocumentTrackingMgmt = "Collateral Tracking";
    internal const string DynamicDataManagement = "Dynamic Data Management";
    internal const string FeeRules = "Fee Rules";
    internal const string FieldRules = "Field Rules";
    internal const string DataTables = "Data Tables";
    internal const string FeeGroups = "Fee Groups";
    internal const string DataPopulationTimeing = "Global DDM Settings";
    internal const string SystemAdministration = "System Administration";
    internal const string AnalysisTools = "Analysis Tools";
    internal const string RebuildPipeline = "Rebuild Pipeline";
    internal const string SettingsReport = "Settings Reports";
    internal const string CurrentLogins = "Current Logins";
    internal const string AllUserInformation = "All User Information";
    internal const string LoanReassignment = "Loan Reassignment";
    internal const string UnlockLoanFile = "Unlock Loan File";
    internal const string UnlockTrade = "Unlock Trade";
    internal const string SystemAuditTrail = "System Audit Trail";
    internal const string AppraisalManagement = "Appraisal Order Management";
    internal const string FieldSearch = "Rebuild Field Search Data";
    internal const string LoanErrorInformation = "Loan Error Information";
    internal const string LicenseManagement = "Additional Services";
    internal const string EdocumentManagement = "E-Document Management";
    internal const string StatusOnline = "Company Status Online";
    internal const string Fulfillment = "eDisclosure Fulfillment";
    internal const string EClose = "Encompass eClose Setup";
    internal const string EncompassAIQ = "Data & Document Automation and Mortgage Analyzers";
    internal const string ComplianceReview = "Compliance Review Setup";
    internal const string TaxReturnService = "4506C Service";
    internal const string SSNService = "SSN Verification Service";
    internal const string LoanScreening = "e360Select Setup";
    internal const string AppraisalCenter = "Appraisal Service";
    internal const string TitleCenter = "Title Service";
    internal const string FraudService = "Fraud Service";
    internal const string FannieService = "Fannie Mae Workflow";
    internal const string FreddieService = "Freddie Mac Order Alert";
    internal const string MIService = "Mortgage Insurance Service";
    internal const string FreddieMacCAC = "Freddie Mac Loan Assignment";
    internal const string FreddieMacLPABatch = "Freddie Mac LPA Batch";
    internal const string GinnieMaePDDBatch = "Ginnie Mae PDD Batch";
    internal const string FannieMaeUCDTransfer = "Fannie Mae UCD Transfer";
    internal const string AvmService = "Valuation Service";
    internal const string TQLServices = "TQL Services";
    internal const string DataTracService = "DataTrac Connection";
    internal const string InsightsSetup = "Insights Setup";
    internal const string FloodService = "Flood Service";
    internal const string PersonalSettings = "Personal Settings";
    internal const string MyProfile = "My Profile";
    internal const string DefaultFileContacts = "Default File Contacts";
    internal const string StatusOnlineConfiguration = "Personal Status Online";
    internal const string AutosaveConfiguration = "Autosave Configuration";
    internal const string PipelineRefresh = "Pipeline Refresh";
    internal const string MyEmailServerSettings = "My Email Server Settings";
    internal const string GrantFileAccess = "Grant File Access";
    internal const string RestoreWarningMessages = "Restore Warning Messages";
    internal static List<string> CompanyUserSetupPages = new List<string>();
    internal static List<string> ExternalCompanySetupPages = new List<string>();
    internal static List<string> InvestorConnectSetupPages = new List<string>();
    internal static List<string> LoanSetupPages = new List<string>();
    internal static List<string> EfolderSetupPages = new List<string>();
    internal static List<string> EllieMaeDocsSetupPages = new List<string>();
    internal static List<string> SecondarySetupPages = new List<string>();
    internal static List<string> ContactSetupPages = new List<string>();
    internal static List<string> LoanTemplatesPages = new List<string>();
    internal static List<string> TablesAndFeesPages = new List<string>();
    internal static List<string> BusinessRulesPages = new List<string>();
    internal static List<string> DynamicDataMgmtPages = new List<string>();
    internal static List<string> SystemAdministrationPages = new List<string>();
    internal static List<string> LicenseManagementPages = new List<string>();
    internal static List<string> PersonalSettingsPages = new List<string>();
    internal static List<List<string>> SettingsLists = new List<List<string>>();
    internal static List<string> AllSettingsPages = new List<string>();
    private static Dictionary<string, SetupPage> allSetupPages = new Dictionary<string, SetupPage>();
    internal readonly string Title;
    private string subTitle;
    private bool isFirstLevelPage;
    internal SetupPage.ContainerSaveResetButtons containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.DoNotShow;

    static SetupPage()
    {
      SetupPage.CompanyUserSetupPages.AddRange((IEnumerable<string>) new string[9]
      {
        "Company/User Setup",
        "Company Information",
        "ICE Mortgage Technology Network Company Password",
        "Services Password Management",
        nameof (Personas),
        "Organization/Users",
        nameof (Roles),
        "Milestones",
        "User Groups"
      });
      SetupPage.ExternalCompanySetupPages.AddRange((IEnumerable<string>) new string[9]
      {
        "External Company Setup",
        "Company Details",
        "TPO Settings",
        "TPO Fees",
        "TPO Reassignment",
        "TPO Custom Fields",
        "All TPO Contact Information",
        "TPO Global Lender Contacts",
        "TPO Docs"
      });
      SetupPage.InvestorConnectSetupPages.AddRange((IEnumerable<string>) new string[3]
      {
        "Investor Connect Setup",
        "Deliver Loans",
        "Partner Setup"
      });
      bool flag1 = Session.ConfigurationManager.CheckIfAnyTPOSiteExists();
      bool flag2 = Session.ConfigurationManager.CheckIfTPOWebCenterProvisioned();
      if (flag1)
        SetupPage.ExternalCompanySetupPages.Add("TPO Connect Site Management");
      if (!flag1 & flag2)
        SetupPage.ExternalCompanySetupPages.Add("TPO WebCenter Site Management");
      SetupPage.ExternalCompanySetupPages.Add("TPO Disclosure Settings");
      SetupPage.LoanSetupPages.AddRange((IEnumerable<string>) new string[28]
      {
        "Loan Setup",
        "Auto Loan Numbering",
        "Auto MERS MIN Numbering",
        "Loan Folders",
        "Loan Duplication",
        nameof (Alerts),
        nameof (Log),
        nameof (Tasks),
        "Default Input Forms",
        "Condition Forms",
        "Custom Print Forms",
        "Print Form Groups",
        "Loan Custom Fields",
        "Channel Options",
        nameof (RESPA),
        "Changed Circumstances Setup",
        "Disclosure Tracking Settings",
        "Compliance Calendar",
        "2009 GFE Print",
        "Trustee List",
        "Piggyback Loan Synchronization",
        "Sync Templates",
        "Privacy Policy",
        "Zipcode Setup",
        "HMDA Profiles",
        "NMLS Report Setup",
        "Verification Contact Setup",
        "FHA Informed Consumer Choice Disclosure"
      });
      if ((bool) Session.ServerManager.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS"))
        SetupPage.EfolderSetupPages.AddRange((IEnumerable<string>) new string[14]
        {
          "eFolder Setup",
          "Document Conversion",
          nameof (Documents),
          "Document Export Templates",
          "Document Groups",
          "Document Stacking Templates",
          "Document Identification",
          "Document Training",
          "Enhanced Conditions",
          "Enhanced Condition Sets",
          nameof (Conditions),
          "Condition Sets",
          "Post-Closing Conditions",
          "Post-Closing Condition Sets"
        });
      else
        SetupPage.EfolderSetupPages.AddRange((IEnumerable<string>) new string[12]
        {
          "eFolder Setup",
          "Document Conversion",
          nameof (Documents),
          "Document Export Templates",
          "Document Groups",
          "Document Stacking Templates",
          "Document Identification",
          "Document Training",
          nameof (Conditions),
          "Condition Sets",
          "Post-Closing Conditions",
          "Post-Closing Condition Sets"
        });
      if ((bool) Session.ServerManager.GetServerSetting("Policies.AllowPurchaseCondition"))
        SetupPage.EfolderSetupPages.AddRange((IEnumerable<string>) new string[5]
        {
          "Purchase Conditions",
          "Purchase Condition Sets",
          "HTML Email Templates",
          "Notification Templates",
          "WebCenter Configuration"
        });
      else
        SetupPage.EfolderSetupPages.AddRange((IEnumerable<string>) new string[3]
        {
          "HTML Email Templates",
          "Notification Templates",
          "WebCenter Configuration"
        });
      SetupPage.EllieMaeDocsSetupPages.AddRange((IEnumerable<string>) new string[7]
      {
        "Docs Setup",
        "eDisclosure Packages",
        "eDisclosure Plan Codes",
        "eDisclosure Stacking Templates",
        "Closing Doc Plan Codes",
        "Closing Doc Stacking Templates",
        "Compliance Audit Settings"
      });
      if ((bool) Session.ServerManager.GetServerSetting("Policies.EnableBidTape"))
        SetupPage.SecondarySetupPages.AddRange((IEnumerable<string>) new string[18]
        {
          "Secondary Setup",
          "Product and Pricing",
          "Secondary Lock Fields",
          "Lock Request Additional Fields",
          "Lock Comparison Tool Fields",
          "Auto-Lock",
          "Investor Templates",
          "ICE PPE Loan Program Table",
          "Trade Management Setup",
          "Normalized Bid Tape Template",
          "Adjustment Templates",
          "Lock Desk Setup",
          "SRP Templates",
          "Funding Templates",
          nameof (Servicing),
          "Correspondent Purchase Advice",
          "Purchase Advice Form",
          "Loan Pricing Decimal Places"
        });
      else
        SetupPage.SecondarySetupPages.AddRange((IEnumerable<string>) new string[17]
        {
          "Secondary Setup",
          "Product and Pricing",
          "Secondary Lock Fields",
          "Lock Request Additional Fields",
          "Lock Comparison Tool Fields",
          "Auto-Lock",
          "Investor Templates",
          "ICE PPE Loan Program Table",
          "Trade Management Setup",
          "Adjustment Templates",
          "Lock Desk Setup",
          "SRP Templates",
          "Funding Templates",
          nameof (Servicing),
          "Correspondent Purchase Advice",
          "Purchase Advice Form",
          "Loan Pricing Decimal Places"
        });
      SetupPage.ContactSetupPages.AddRange((IEnumerable<string>) new string[8]
      {
        "Contact Setup",
        "Borrower Custom Fields",
        "Borrower Contact Status",
        "Borrower Contact Update",
        "Business Custom Fields",
        "Business Categories",
        "Public Business Contact Groups",
        "Email Server Settings"
      });
      SetupPage.LoanTemplatesPages.AddRange((IEnumerable<string>) new string[13]
      {
        "Loan Templates",
        "Loan Programs",
        "Closing Costs",
        "Input Form Sets",
        "Settlement Service Providers",
        "Affiliated Business Arrangement Templates",
        "Document Sets",
        "Transcript Request Templates",
        "Task Sets",
        "Data Templates",
        "Loan Template Sets",
        "Default Template Setting",
        "Configurable Workflow Templates"
      });
      SetupPage.TablesAndFeesPages.AddRange((IEnumerable<string>) new string[16]
      {
        "Tables and Fees",
        "Escrow",
        nameof (Title),
        "HELOC Table",
        "MI Tables",
        "FHA County Limits",
        "Conventional County Limits",
        "Federal Threshold Adjustments",
        "Affordable Lending",
        "City Tax",
        "State Tax",
        "User Defined Fee",
        "Itemization Fee Management",
        "LO Compensation",
        "Temporary Buydown",
        "Special Feature Codes"
      });
      SetupPage.BusinessRulesPages.AddRange((IEnumerable<string>) new string[3]
      {
        "Business Rules",
        "Loan Folder Business Rule",
        "Milestone Completion"
      });
      if (flag1)
        SetupPage.BusinessRulesPages.Add("Loan Action Completion");
      if ((bool) Session.ServerManager.GetServerSetting("CLIENT.ENHANCEDCONDITIONSETTINGS"))
        SetupPage.BusinessRulesPages.AddRange((IEnumerable<string>) new string[6]
        {
          "Field Data Entry",
          "Field Triggers",
          "Automated Conditions",
          "Automated Enhanced Conditions",
          "Persona Access to Fields",
          "Persona Access to Loans"
        });
      else
        SetupPage.BusinessRulesPages.AddRange((IEnumerable<string>) new string[5]
        {
          "Field Data Entry",
          "Field Triggers",
          "Automated Conditions",
          "Persona Access to Fields",
          "Persona Access to Loans"
        });
      if (flag1)
        SetupPage.BusinessRulesPages.Add("Persona Access to Loan Actions");
      SetupPage.BusinessRulesPages.AddRange((IEnumerable<string>) new string[7]
      {
        "Role Access to Documents",
        "Input Form List",
        "Loan Form Printing",
        "Print Auto Selection",
        "Appraisal Order Management",
        "LO Compensation Rule",
        "Collateral Tracking"
      });
      SetupPage.DynamicDataMgmtPages.AddRange((IEnumerable<string>) new string[5]
      {
        "Dynamic Data Management",
        "Fee Rules",
        "Field Rules",
        "Data Tables",
        "Global DDM Settings"
      });
      SetupPage.SystemAdministrationPages.AddRange((IEnumerable<string>) new string[12]
      {
        "System Administration",
        "Analysis Tools",
        "Rebuild Pipeline",
        "Current Logins",
        "All User Information",
        "Settings Reports",
        "Loan Reassignment",
        "Unlock Loan File",
        "Unlock Trade",
        "System Audit Trail",
        "Rebuild Field Search Data",
        "Loan Error Information"
      });
      SetupPage.LicenseManagementPages.AddRange((IEnumerable<string>) new string[25]
      {
        "Additional Services",
        "E-Document Management",
        "Company Status Online",
        "eDisclosure Fulfillment",
        "Encompass eClose Setup",
        "Data & Document Automation and Mortgage Analyzers",
        "Compliance Review Setup",
        "4506C Service",
        "SSN Verification Service",
        "TQL Services",
        "e360Select Setup",
        "Appraisal Service",
        "Title Service",
        "Fraud Service",
        "Fannie Mae Workflow",
        "Fannie Mae UCD Transfer",
        "Freddie Mac Order Alert",
        "Freddie Mac Loan Assignment",
        "Freddie Mac LPA Batch",
        "Ginnie Mae PDD Batch",
        "Mortgage Insurance Service",
        "Valuation Service",
        "Flood Service",
        "DataTrac Connection",
        "Insights Setup"
      });
      IDictionary serverSettings = Session.ServerManager.GetServerSettings("Feature", false);
      if (serverSettings.Contains((object) "Feature.StatusOnline") && !(serverSettings[(object) "Feature.StatusOnline"].ToString().ToLower() == "true"))
      {
        SetupPage.LicenseManagementPages.Clear();
        SetupPage.LicenseManagementPages.AddRange((IEnumerable<string>) new string[24]
        {
          "Additional Services",
          "E-Document Management",
          "eDisclosure Fulfillment",
          "Encompass eClose Setup",
          "Data & Document Automation and Mortgage Analyzers",
          "Compliance Review Setup",
          "4506C Service",
          "SSN Verification Service",
          "TQL Services",
          "e360Select Setup",
          "Appraisal Service",
          "Title Service",
          "Fraud Service",
          "Fannie Mae Workflow",
          "Fannie Mae UCD Transfer",
          "Freddie Mac Order Alert",
          "Freddie Mac Loan Assignment",
          "Freddie Mac LPA Batch",
          "Ginnie Mae PDD Batch",
          "Mortgage Insurance Service",
          "Valuation Service",
          "Flood Service",
          "DataTrac Connection",
          "Insights Setup"
        });
      }
      SetupPage.PersonalSettingsPages.AddRange((IEnumerable<string>) new string[9]
      {
        "Personal Settings",
        "My Profile",
        "Default File Contacts",
        "Personal Status Online",
        "Autosave Configuration",
        "Pipeline Refresh",
        "My Email Server Settings",
        "Grant File Access",
        "Restore Warning Messages"
      });
      if (Session.EncompassEdition == EncompassEdition.Broker)
        SetupPage.SettingsLists.AddRange((IEnumerable<List<string>>) new List<string>[13]
        {
          SetupPage.CompanyUserSetupPages,
          SetupPage.InvestorConnectSetupPages,
          SetupPage.LoanSetupPages,
          SetupPage.EfolderSetupPages,
          SetupPage.EllieMaeDocsSetupPages,
          SetupPage.SecondarySetupPages,
          SetupPage.ContactSetupPages,
          SetupPage.LoanTemplatesPages,
          SetupPage.TablesAndFeesPages,
          SetupPage.BusinessRulesPages,
          SetupPage.DynamicDataMgmtPages,
          SetupPage.SystemAdministrationPages,
          SetupPage.LicenseManagementPages
        });
      else
        SetupPage.SettingsLists.AddRange((IEnumerable<List<string>>) new List<string>[14]
        {
          SetupPage.CompanyUserSetupPages,
          SetupPage.ExternalCompanySetupPages,
          SetupPage.InvestorConnectSetupPages,
          SetupPage.LoanSetupPages,
          SetupPage.EfolderSetupPages,
          SetupPage.EllieMaeDocsSetupPages,
          SetupPage.SecondarySetupPages,
          SetupPage.ContactSetupPages,
          SetupPage.LoanTemplatesPages,
          SetupPage.TablesAndFeesPages,
          SetupPage.BusinessRulesPages,
          SetupPage.DynamicDataMgmtPages,
          SetupPage.SystemAdministrationPages,
          SetupPage.LicenseManagementPages
        });
      SetupPage.SettingsLists.Add(SetupPage.PersonalSettingsPages);
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.CompanyUserSetupPages.ToArray());
      if (Session.EncompassEdition != EncompassEdition.Broker)
        SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.ExternalCompanySetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.InvestorConnectSetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.LoanSetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.EfolderSetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.EllieMaeDocsSetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.SecondarySetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.ContactSetupPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.LoanTemplatesPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.TablesAndFeesPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.BusinessRulesPages.ToArray());
      if (Session.DefaultInstance.StartupInfo.AllowDDM)
        SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.DynamicDataMgmtPages.ToArray());
      else
        SetupPage.SettingsLists.Remove(SetupPage.DynamicDataMgmtPages);
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.SystemAdministrationPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.LicenseManagementPages.ToArray());
      SetupPage.AllSettingsPages.AddRange((IEnumerable<string>) SetupPage.PersonalSettingsPages.ToArray());
      SetupPage.allSetupPages.Add("Settings Overview", new SetupPage("Settings Overview"));
      foreach (string allSettingsPage in SetupPage.AllSettingsPages)
        SetupPage.allSetupPages.Add(allSettingsPage, new SetupPage(allSettingsPage));
    }

    internal static SetupPage GetSetupPage(string pageTitle) => SetupPage.allSetupPages[pageTitle];

    internal string SubTitle => this.subTitle;

    internal bool IsFirstLevelPage => this.isFirstLevelPage;

    internal SetupPage.ContainerSaveResetButtons ContainerSaveResetBtns
    {
      get => this.containerSaveResetBtns;
    }

    private SetupPage(string title)
    {
      this.Title = title;
      this.setFields();
    }

    private void setFields()
    {
      this.setSubTitleAndFirstLevel();
      this.setShowParentSaveResetButtons();
    }

    internal bool AppliesToEdition(EncompassEdition edition)
    {
      switch (this.Title)
      {
        case "Adjustment Templates":
        case "All TPO Contact Information":
        case "Auto-Lock":
        case "Automated Conditions":
        case "Automated Enhanced Conditions":
        case "Collateral Tracking":
        case "Company Details":
        case "Condition Forms":
        case "Condition Sets":
        case "Conditions":
        case "Correspondent Purchase Advice":
        case "Deliver Loans":
        case "Document Training":
        case "Enhanced Condition Sets":
        case "Enhanced Conditions":
        case "External Company Setup":
        case "Field Data Entry":
        case "Field Triggers":
        case "Funding Templates":
        case "ICE PPE Loan Program Table":
        case "Input Form List":
        case "Investor Connect Setup":
        case "Investor Templates":
        case "LO Compensation":
        case "Loan Action Completion":
        case "Loan Duplication":
        case "Loan Folder Business Rule":
        case "Loan Form Printing":
        case "Loan Pricing Decimal Places":
        case "Lock Desk Setup":
        case "Lock Request Additional Fields":
        case "Milestone Completion":
        case "Normalized Bid Tape Template":
        case "Partner Setup":
        case "Persona Access to Fields":
        case "Persona Access to Loan Actions":
        case "Pipeline Refresh":
        case "Post-Closing Condition Sets":
        case "Post-Closing Conditions":
        case "Print Auto Selection":
        case "Product and Pricing":
        case "Public Business Contact Groups":
        case "Purchase Advice Form":
        case "Purchase Condition Sets":
        case "Purchase Conditions":
        case "Role Access to Documents":
        case "Roles":
        case "SRP Templates":
        case "Secondary Lock Fields":
        case "Secondary Setup":
        case "Servicing":
        case "TPO Connect Site Management":
        case "TPO Custom Fields":
        case "TPO Disclosure Settings":
        case "TPO Docs":
        case "TPO Fees":
        case "TPO Global Lender Contacts":
        case "TPO Reassignment":
        case "TPO Settings":
        case "TPO WebCenter Site Management":
        case "Trade Management Setup":
          return edition == EncompassEdition.Banker;
        default:
          return true;
      }
    }

    internal SetupPage.AccessControl Access
    {
      get
      {
        switch (this.Title)
        {
          case "2009 GFE Print":
            return SetupPage.AccessControl.GfePrint2009;
          case "4506C Service":
            return SetupPage.AccessControl.TaxReturnService;
          case "Additional Services":
          case "External Company Setup":
          case "Personal Settings":
            return SetupPage.AccessControl.ExternalCompanySetup;
          case "Adjustment Templates":
            return SetupPage.AccessControl.AdjustmentTemplates;
          case "Affiliated Business Arrangement Templates":
            return SetupPage.AccessControl.AffiliatedBusinessArrangements;
          case "Affordable Lending":
            return SetupPage.AccessControl.AMILimits;
          case "Alerts":
            return SetupPage.AccessControl.Alerts;
          case "All TPO Contact Information":
            return SetupPage.AccessControl.TPOAllContactInfo;
          case "All User Information":
            return SetupPage.AccessControl.AllUserInformation;
          case "Analysis Tools":
            return SetupPage.AccessControl.AnalysisTools;
          case "Appraisal Order Management":
            return SetupPage.AccessControl.AppraisalManagement;
          case "Appraisal Service":
          case "SSN Verification Service":
          case "e360Select Setup":
            return SetupPage.AccessControl.AppraisalCenter;
          case "Auto Loan Numbering":
            return SetupPage.AccessControl.AutoLoanNumbering;
          case "Auto MERS MIN Numbering":
            return SetupPage.AccessControl.AutoMersMinNumbering;
          case "Auto-Lock":
            return SetupPage.AccessControl.AutoLock;
          case "Automated Conditions":
            return SetupPage.AccessControl.AutomatedConditions;
          case "Automated Enhanced Conditions":
            return SetupPage.AccessControl.AutomatedEnhancedConditions;
          case "Autosave Configuration":
            return SetupPage.AccessControl.AutosaveConfiguration;
          case "Borrower Contact Status":
            return SetupPage.AccessControl.BorrowerContactStatus;
          case "Borrower Contact Update":
            return SetupPage.AccessControl.BorrowerContactUpdate;
          case "Borrower Custom Fields":
            return SetupPage.AccessControl.BorrowerCustomFields;
          case "Business Categories":
            return SetupPage.AccessControl.BusinessCategories;
          case "Business Custom Fields":
            return SetupPage.AccessControl.BusinessCustomFields;
          case "Business Rules":
            return SetupPage.AccessControl.BusinessRules;
          case "Changed Circumstances Setup":
            return SetupPage.AccessControl.ChangeCircumstanceSetup;
          case "Channel Options":
            return SetupPage.AccessControl.ChannelOptions;
          case "City Tax":
            return SetupPage.AccessControl.CityTax;
          case "Closing Costs":
            return SetupPage.AccessControl.ClosingCosts;
          case "Closing Doc Plan Codes":
            return SetupPage.AccessControl.ClosingPlanCodes;
          case "Closing Doc Stacking Templates":
            return SetupPage.AccessControl.ClosingStackingTemplates;
          case "Collateral Tracking":
            return SetupPage.AccessControl.DocumentTrackingMgmt;
          case "Company Details":
            return SetupPage.AccessControl.CompanySetup;
          case "Company Information":
            return SetupPage.AccessControl.CompanyInformation;
          case "Company Status Online":
            return SetupPage.AccessControl.StatusOnline;
          case "Company/User Setup":
            return SetupPage.AccessControl.CompanyUserSetup;
          case "Compliance Audit Settings":
            return SetupPage.AccessControl.ComplianceAuditSettings;
          case "Compliance Calendar":
            return SetupPage.AccessControl.ComplianceCalendar;
          case "Compliance Review Setup":
            return SetupPage.AccessControl.ComplianceSetup;
          case "Condition Forms":
            return SetupPage.AccessControl.ConditionalLetterOption;
          case "Condition Sets":
            return SetupPage.AccessControl.ConditionSets;
          case "Conditions":
            return SetupPage.AccessControl.Conditions;
          case "Configurable Workflow Templates":
            return SetupPage.AccessControl.ConfigurableKeyDates;
          case "Contact Setup":
            return SetupPage.AccessControl.ContactSetup;
          case "Conventional County Limits":
            return SetupPage.AccessControl.ConventionalCountyLimits;
          case "Correspondent Purchase Advice":
            return SetupPage.AccessControl.CorrespondentPurchaseAdvice;
          case "Current Logins":
            return SetupPage.AccessControl.CurrentLogins;
          case "Custom Print Forms":
            return SetupPage.AccessControl.CustomPrintForms;
          case "Data & Document Automation and Mortgage Analyzers":
            return SetupPage.AccessControl.EncompassAIQ;
          case "Data Tables":
            return SetupPage.AccessControl.DataTables;
          case "Data Templates":
            return SetupPage.AccessControl.MiscDataTemplates;
          case "DataTrac Connection":
            return SetupPage.AccessControl.DataTracService;
          case "Default File Contacts":
            return SetupPage.AccessControl.DefaultFileContacts;
          case "Default Input Forms":
            return SetupPage.AccessControl.DefaultInputForms;
          case "Default Template Setting":
          case "Loan Program Additional Fields":
          case "Milestone Completion":
            return SetupPage.AccessControl.MilestoneCompletion;
          case "Deliver Loans":
            return SetupPage.AccessControl.DeliverLoans;
          case "Disclosure Tracking Settings":
            return SetupPage.AccessControl.DisclosureTracking;
          case "Docs Setup":
            return SetupPage.AccessControl.DocServices;
          case "Document Conversion":
            return SetupPage.AccessControl.DocumentConversion;
          case "Document Export Templates":
            return SetupPage.AccessControl.DocumentExportTemplates;
          case "Document Groups":
            return SetupPage.AccessControl.DocumentGroups;
          case "Document Identification":
            return SetupPage.AccessControl.Barcodes;
          case "Document Sets":
            return SetupPage.AccessControl.DocumentSets;
          case "Document Stacking Templates":
            return SetupPage.AccessControl.DocumentStackingOrders;
          case "Document Training":
            return SetupPage.AccessControl.DocumentTraining;
          case "Documents":
            return SetupPage.AccessControl.Documents;
          case "Dynamic Data Management":
            return SetupPage.AccessControl.DynamicDataManagement;
          case "E-Document Management":
            return SetupPage.AccessControl.EdocumentManagement;
          case "Email Server Settings":
            return SetupPage.AccessControl.EmailServerSettings;
          case "Encompass eClose Setup":
            return SetupPage.AccessControl.eClose;
          case "Enhanced Condition Sets":
            return SetupPage.AccessControl.EnhancedConditionSets;
          case "Enhanced Conditions":
            return SetupPage.AccessControl.EnhancedConditions;
          case "Escrow":
            return SetupPage.AccessControl.EscrowTable;
          case "FHA County Limits":
            return SetupPage.AccessControl.FhaCountyLimits;
          case "FHA Informed Consumer Choice Disclosure":
            return SetupPage.AccessControl.FHAInformedConsumerChoiceDisclosure;
          case "Fannie Mae UCD Transfer":
            return SetupPage.AccessControl.FannieMaeUCDTransferService;
          case "Fannie Mae Workflow":
            return SetupPage.AccessControl.FannieService;
          case "Federal Threshold Adjustments":
            return SetupPage.AccessControl.FedTresholdAdjustments;
          case "Fee Groups":
            return SetupPage.AccessControl.FeeGroups;
          case "Fee Rules":
            return SetupPage.AccessControl.FeeRules;
          case "Field Data Entry":
            return SetupPage.AccessControl.FieldDataEntry;
          case "Field Rules":
            return SetupPage.AccessControl.FieldRules;
          case "Field Triggers":
            return SetupPage.AccessControl.FieldTriggers;
          case "Flood Service":
            return SetupPage.AccessControl.FloodService;
          case "Fraud Service":
            return SetupPage.AccessControl.FraudService;
          case "Freddie Mac LPA Batch":
            return SetupPage.AccessControl.FreddieMacLPAService;
          case "Freddie Mac Loan Assignment":
            return SetupPage.AccessControl.FreddieMacCACService;
          case "Freddie Mac Order Alert":
            return SetupPage.AccessControl.FreddieService;
          case "Funding Templates":
            return SetupPage.AccessControl.FundingTemplates;
          case "Ginnie Mae PDD Batch":
            return SetupPage.AccessControl.GinnieMaePddService;
          case "Global DDM Settings":
            return SetupPage.AccessControl.DataPopulationTiming;
          case "Grant File Access":
            return SetupPage.AccessControl.GrantFileAccess;
          case "HELOC Table":
            return SetupPage.AccessControl.HelocTable;
          case "HMDA Profiles":
            return SetupPage.AccessControl.HMDA;
          case "HTML Email Templates":
            return SetupPage.AccessControl.HtmlEmailTemplates;
          case "ICE Mortgage Technology Network Company Password":
            return SetupPage.AccessControl.EpassCompanyPassword;
          case "ICE PPE Loan Program Table":
            return SetupPage.AccessControl.EPPSLoanProgram;
          case "Input Form List":
            return SetupPage.AccessControl.InputFormList;
          case "Input Form Sets":
            return SetupPage.AccessControl.InputFormSets;
          case "Insights Setup":
            return SetupPage.AccessControl.InsightsSetup;
          case "Investor Connect Setup":
            return SetupPage.AccessControl.InvestorConnectSetup;
          case "Investor Templates":
            return SetupPage.AccessControl.InvestorTemplates;
          case "Itemization Fee Management":
            return SetupPage.AccessControl.ItemizationFeeManagement;
          case "LO Compensation":
            return SetupPage.AccessControl.LOCompensation;
          case "LO Compensation Rule":
            return SetupPage.AccessControl.LOCompensationControl;
          case "Loan Action Completion":
            return SetupPage.AccessControl.LoanActionCompletion;
          case "Loan Custom Fields":
            return SetupPage.AccessControl.LoanCustomFields;
          case "Loan Duplication":
            return SetupPage.AccessControl.LoanDuplication;
          case "Loan Folder Business Rule":
            return SetupPage.AccessControl.LoanFolderBusinessRule;
          case "Loan Folders":
            return SetupPage.AccessControl.LoanFolders;
          case "Loan Form Printing":
            return SetupPage.AccessControl.LoanFormPrinting;
          case "Loan Pricing Decimal Places":
            return SetupPage.AccessControl.LoanPricingDecimalPlaces;
          case "Loan Programs":
            return SetupPage.AccessControl.LoanPrograms;
          case "Loan Reassignment":
            return SetupPage.AccessControl.LoanReassignment;
          case "Loan Setup":
            return SetupPage.AccessControl.LoanSetup;
          case "Loan Template Sets":
            return SetupPage.AccessControl.LoanTemplateSets;
          case "Loan Templates":
          case "Tables and Fees":
            return SetupPage.AccessControl.TablesAndFees;
          case "Lock Comparison Tool Fields":
            return SetupPage.AccessControl.LockComparisonToolFields;
          case "Lock Desk Setup":
            return SetupPage.AccessControl.LockExpirationDate;
          case "Lock Request Additional Fields":
            return SetupPage.AccessControl.LockRequestAdditionalFields;
          case "Log":
            return SetupPage.AccessControl.Log;
          case "MI Tables":
            return SetupPage.AccessControl.MiTables;
          case "Milestones":
            return SetupPage.AccessControl.Milestones80;
          case "Mortgage Insurance Service":
            return SetupPage.AccessControl.MIService;
          case "My Email Server Settings":
          case "My Profile":
          case "Restore Warning Messages":
            return SetupPage.AccessControl.All;
          case "NMLS Report Setup":
            return SetupPage.AccessControl.NMLSSetup;
          case "Normalized Bid Tape Template":
            return SetupPage.AccessControl.NormalizedBidTapeTemplate;
          case "Notification Templates":
            return SetupPage.AccessControl.NotificationTemplates;
          case "Organization/Users":
            return SetupPage.AccessControl.OrganizationUsers;
          case "Partner Setup":
            return SetupPage.AccessControl.PartnerSetup;
          case "Persona Access to Fields":
            return SetupPage.AccessControl.PersonaAccessToFields;
          case "Persona Access to Loan Actions":
            return SetupPage.AccessControl.PersonaAccessToLoanActions;
          case "Persona Access to Loans":
            return SetupPage.AccessControl.PersonaAccessToLoans;
          case "Personal Status Online":
            return SetupPage.AccessControl.StatusOnlineConfiguration;
          case "Personas":
            return SetupPage.AccessControl.Personas;
          case "Piggyback Loan Synchronization":
            return SetupPage.AccessControl.PiggybackLoanSynchronization;
          case "Pipeline Refresh":
            return SetupPage.AccessControl.PipelineRefresh;
          case "Post-Closing Condition Sets":
            return SetupPage.AccessControl.PostClosingConditionSets;
          case "Post-Closing Conditions":
            return SetupPage.AccessControl.PostClosingConditions;
          case "Print Auto Selection":
            return SetupPage.AccessControl.PrintAutoSelection;
          case "Print Form Groups":
            return SetupPage.AccessControl.PrintFormGroups;
          case "Privacy Policy":
            return SetupPage.AccessControl.PrivacyPolicyForm;
          case "Product and Pricing":
            return SetupPage.AccessControl.ProductAndPricing;
          case "Public Business Contact Groups":
            return SetupPage.AccessControl.PublicBizContactGroup;
          case "Purchase Advice Form":
            return SetupPage.AccessControl.PurchaseAdviceForm;
          case "Purchase Condition Sets":
            return SetupPage.AccessControl.PurchaseConditionSets;
          case "Purchase Conditions":
            return SetupPage.AccessControl.PurchaseConditions;
          case "RESPA":
            return SetupPage.AccessControl.RESPA;
          case "Rebuild Field Search Data":
            return SetupPage.AccessControl.TopLevelAdmin;
          case "Rebuild Pipeline":
            return SetupPage.AccessControl.TopLevelAdmin;
          case "Role Access to Documents":
            return SetupPage.AccessControl.RoleAccessToDocuments;
          case "Roles":
            return SetupPage.AccessControl.Roles;
          case "SRP Templates":
            return SetupPage.AccessControl.SrpTemplates;
          case "Secondary Lock Fields":
            return SetupPage.AccessControl.SecondaryLockFields;
          case "Secondary Setup":
            return SetupPage.AccessControl.SecondarySetup;
          case "Services Password Management":
            return SetupPage.AccessControl.EPassPasswordManagement;
          case "Servicing":
            return SetupPage.AccessControl.Servicing;
          case "Settings Overview":
            return SetupPage.AccessControl.All;
          case "Settings Reports":
            return SetupPage.AccessControl.SettingsReport;
          case "Settlement Service Providers":
            return SetupPage.AccessControl.SettlementServiceProviders;
          case "Special Feature Codes":
            return SetupPage.AccessControl.SpecialFeatureCodes;
          case "State Tax":
            return SetupPage.AccessControl.StateTax;
          case "Sync Templates":
            return SetupPage.AccessControl.SyncTemplates;
          case "System Administration":
            return SetupPage.AccessControl.SystemAdministration;
          case "System Audit Trail":
            return SetupPage.AccessControl.SystemAuditTrail;
          case "TPO Connect Site Management":
            return SetupPage.AccessControl.TPOConnectSiteMngmnt;
          case "TPO Custom Fields":
            return SetupPage.AccessControl.TPOCustomFields;
          case "TPO Disclosure Settings":
            return SetupPage.AccessControl.TPODTSetting;
          case "TPO Docs":
            return SetupPage.AccessControl.TPOWCDocs;
          case "TPO Fees":
            return SetupPage.AccessControl.TPOFees;
          case "TPO Global Lender Contacts":
            return SetupPage.AccessControl.TPOGlobalLenderContact;
          case "TPO Reassignment":
            return SetupPage.AccessControl.TPOReassignment;
          case "TPO Settings":
            return SetupPage.AccessControl.CompanySettingSetup;
          case "TPO WebCenter Site Management":
            return SetupPage.AccessControl.TPOWCSiteMngmnt;
          case "TQL Services":
            return SetupPage.AccessControl.TQLServices;
          case "Task Sets":
            return SetupPage.AccessControl.TaskSets;
          case "Tasks":
            return SetupPage.AccessControl.Tasks;
          case "Temporary Buydown":
            return SetupPage.AccessControl.TemporaryBuydown;
          case "Title":
            return SetupPage.AccessControl.TitleTable;
          case "Title Service":
            return SetupPage.AccessControl.TitleCenter;
          case "Trade Management Setup":
            return SetupPage.AccessControl.TradeManagementFields;
          case "Transcript Request Templates":
            return SetupPage.AccessControl.SuperAdmin;
          case "Trustee List":
            return SetupPage.AccessControl.TrusteeList;
          case "Unlock Loan File":
            return SetupPage.AccessControl.UnlockLoanFile;
          case "Unlock Trade":
            return SetupPage.AccessControl.UnlockTrade;
          case "User Defined Fee":
            return SetupPage.AccessControl.UserDefinedFee;
          case "User Groups":
            return SetupPage.AccessControl.UserGroups;
          case "Valuation Service":
            return SetupPage.AccessControl.AvmService;
          case "Verification Contact Setup":
            return SetupPage.AccessControl.VerifContactSetup;
          case "WebCenter Configuration":
            return SetupPage.AccessControl.WebCenterConfiguration;
          case "Zipcode Setup":
            return SetupPage.AccessControl.ZipcodeSetup;
          case "eDisclosure Fulfillment":
            return SetupPage.AccessControl.Fulfillment;
          case "eDisclosure Packages":
            return SetupPage.AccessControl.EDisclosures;
          case "eDisclosure Plan Codes":
            return SetupPage.AccessControl.EDisclosurePlanCodes;
          case "eDisclosure Stacking Templates":
            return SetupPage.AccessControl.EDisclosureStackingTemplates;
          case "eFolder Setup":
            return SetupPage.AccessControl.EfolderSetup;
          default:
            throw new Exception(this.Title + ": unknown Settings");
        }
      }
    }

    private void setSubTitleAndFirstLevel()
    {
      this.isFirstLevelPage = false;
      switch (this.Title)
      {
        case "2009 GFE Print":
          this.subTitle = "This setting applies to the 2009 (and earlier) version of the GFE-Itemization input form only. Select the default GFE output forms to print. When printing the GFE, the default forms are included on the list of Selected Forms to print.";
          break;
        case "4506C Service":
          this.subTitle = "The 4506C service allows you to order tax transcripts and returns so you can verify borrower income against IRS tax data.";
          break;
        case "Additional Services":
          this.isFirstLevelPage = true;
          break;
        case "Adjustment Templates":
          this.subTitle = "Create templates of price adjustments that are applied to the loans in a trade.";
          break;
        case "Affiliated Business Arrangement Templates":
          this.subTitle = "Create sets (templates) of Affiliated Business Arrangements.";
          break;
        case "Affordable Lending":
          this.subTitle = "Affordable Lending Limits";
          break;
        case "Alerts":
          this.subTitle = "Create notifications to indicate when events have occurred, are pending, or are due.";
          break;
        case "All TPO Contact Information":
          this.subTitle = "View all TPO contacts, find a contact in the organization hierarchy, and send emails to selected contacts.";
          break;
        case "All User Information":
          this.subTitle = "View Encompass users, find a user in the organization hierarchy, and send emails to selected users.";
          break;
        case "Analysis Tools":
          this.subTitle = "Use this tool to search for input fields and locate the areas in Encompass Settings where the fields are used including business rules and alerts.";
          break;
        case "Appraisal Order Management":
          this.subTitle = "Set up the appraisers and appraisal management companies that users can access and the rules for processing the request.";
          break;
        case "Appraisal Service":
          this.subTitle = "Set up merchant accounts to process borrower payments for appraisal orders. Only users selected below can facilitate borrower payments for appraisals.";
          break;
        case "Auto Loan Numbering":
          this.subTitle = "Define the assignment of loan numbers to loans.";
          break;
        case "Auto MERS MIN Numbering":
          this.subTitle = "Enter the seven digit Organization ID provided by MERS.";
          break;
        case "Auto-Lock":
          this.subTitle = "Select options for Auto-Lock feature. Only supported when ICE Product and Pricing Engine is the selected Product and Pricing Engine";
          break;
        case "Automated Conditions":
          this.subTitle = "Create and manage rules that automatically populate conditions to the loan file when the Add Automated Conditions option is selected in the eFolder.";
          break;
        case "Automated Enhanced Conditions":
          this.subTitle = "Create and manage rules that automatically populate enhanced conditions to the loan file when the Add Automated Enhanced Conditions option is selected in the eFolder.";
          break;
        case "Autosave Configuration":
          this.subTitle = "Configure and modify your autosave settings on this computer.";
          break;
        case "Borrower Contact Status":
          this.subTitle = "Create custom statuses for borrower contacts.";
          break;
        case "Borrower Contact Update":
          this.subTitle = "Select a different milestone for updating your borrower contact information.";
          break;
        case "Borrower Custom Fields":
          this.subTitle = "Create global custom fields for borrower contacts.";
          break;
        case "Business Categories":
          this.subTitle = "Create categories for business contacts.";
          break;
        case "Business Custom Fields":
          this.subTitle = "Create global custom fields for business contacts.";
          break;
        case "Business Rules":
          this.isFirstLevelPage = true;
          break;
        case "Changed Circumstances Setup":
          this.subTitle = "Manage the options that users can select when indicating a Changed Circumstance.";
          break;
        case "Channel Options":
          this.subTitle = "Select the options to include in the Channel drop-down list on the Borrower Summary form.";
          break;
        case "City Tax":
          this.subTitle = "Create formulas that calculate city tax for a loan.";
          break;
        case "Closing Costs":
          this.subTitle = "Create templates of predefined values that apply primarily to the Itemization form and the 1003 application.";
          break;
        case "Closing Doc Plan Codes":
          this.subTitle = "Define the Plan Codes used by your company when ordering Closing Docs.";
          break;
        case "Closing Doc Stacking Templates":
          this.subTitle = "Define the Stacking Templates used to sort documents when ordering Closing Docs.";
          break;
        case "Collateral Tracking":
          this.subTitle = "Create and manage the rules that drive the scheduling and behavior of Collateral Tracking. The Scheduled Shipment Option is only for those Clients who schedule future shipments to Investors with a predetermined Tracking Number.";
          break;
        case "Company Details":
          this.subTitle = "Add and manage the banks, lenders, brokers, and third party originators (TPOs) your company does business with.";
          break;
        case "Company Information":
          this.subTitle = "Set up company information that displays on reports and forms, a company email signature, and licensing data for your company and its branches.";
          break;
        case "Company Status Online":
          this.subTitle = "Set up company status update templates, triggers, email templates, and authorized users. Changes to the setting apply only to loans created after the change.";
          break;
        case "Company/User Setup":
          this.isFirstLevelPage = true;
          break;
        case "Compliance Audit Settings":
          this.subTitle = "Configure Compliance Audit Settings";
          break;
        case "Compliance Calendar":
          this.subTitle = "The Reg-Z Business Day Calendar will be used for calculating LE && CD receipt dates. The Company Calendar will be used for calculating LE && CD due dates. The Postal Calendar is provided for informational purposes with the postal holidays being included in the Company Calendar by default.";
          break;
        case "Compliance Review Setup":
          this.subTitle = "The Encompass Compliance Service analyzes loan data for compliance with government regulatory policies and consumer protection laws.";
          break;
        case "Condition Forms":
          this.subTitle = "Create custom condition forms.";
          break;
        case "Condition Sets":
          this.subTitle = "Use the Condition Sets tool to create sets of conditions to meet the requirements of different loan criteria (for example, loan type).";
          break;
        case "Conditions":
          this.subTitle = "Use the Conditions tool to create conditions that you can add to a condition set and import into the eFolder for a loan. ";
          break;
        case "Configurable Workflow Templates":
          this.subTitle = "";
          break;
        case "Contact Setup":
          this.isFirstLevelPage = true;
          break;
        case "Conventional County Limits":
          this.subTitle = "Manage the table that contains the maximum Conventional County Loan Limits based on number of units in a property.";
          break;
        case "Correspondent Purchase Advice":
          this.subTitle = "Configure Correspondent Purchase Advice worksheet tab settings.";
          break;
        case "Current Logins":
          this.subTitle = "View and manage users who are logged in to the server.";
          break;
        case "Custom Print Forms":
          this.subTitle = "Create custom templates for forms, letters, and other documents.";
          break;
        case "Data & Document Automation and Mortgage Analyzers":
          this.subTitle = "";
          break;
        case "Data Tables":
          this.subTitle = "Create and manage data tables for defining values to fee scenario and field scenario rules.";
          break;
        case "Data Templates":
          this.subTitle = "Create templates of default loan data.";
          break;
        case "DataTrac Connection":
          this.subTitle = "Set up a connection between Encompass and DataTrac.";
          break;
        case "Default File Contacts":
          this.subTitle = "Enter information for your most frequently used providers. When a loan is originated, the providers will be added to the appropriate forms.";
          break;
        case "Default Input Forms":
          this.subTitle = "Set the default order of the input forms that display on the Forms tab.";
          break;
        case "Default Template Setting":
          this.subTitle = "Change the default method used to apply templates to a loan file.";
          break;
        case "Deliver Loans":
          this.subTitle = "Use this setting to manage delivery of loans and conditions through Investor Connect.";
          break;
        case "Disclosure Tracking Settings":
          this.subTitle = "Select forms to track in the Disclosure Tracking tool and the events that will trigger recording of disclosures.";
          break;
        case "Docs Setup":
          this.subTitle = "The Encompass Closing Docs service is current in BETA.";
          this.isFirstLevelPage = true;
          break;
        case "Document Conversion":
          this.subTitle = "Convert documents to image files when importing them into the eFolder.";
          break;
        case "Document Export Templates":
          this.subTitle = "Create and configure document export templates.";
          break;
        case "Document Groups":
          this.subTitle = "Create groups of related documents that simplify the sending of document requests from the eFolder. Drag documents from the left panel and drop in a destination group on the right panel.";
          break;
        case "Document Identification":
          this.subTitle = "Add bar codes to documents to improve the eFolder auto-assigning process.";
          break;
        case "Document Sets":
          this.subTitle = "Create sets (templates) of documents to meet the requirements of various loan scenarios or particular lenders.";
          break;
        case "Document Stacking Templates":
          this.subTitle = "Define the stacking templates in eFolder and set the order in which documents are listed when sending them to borrowers, partners, and lenders.";
          break;
        case "Document Training":
          this.subTitle = "Train the document identification system.";
          break;
        case "Documents":
          this.subTitle = "Create and manage the tracking information associated with loan documents and services.";
          break;
        case "Dynamic Data Management":
          this.isFirstLevelPage = true;
          break;
        case "E-Document Management":
          this.subTitle = "Control access to the E-Document Management feature.";
          break;
        case "Email Server Settings":
          this.subTitle = "Specify the email server used to send mail merges from the Contacts Manager.";
          break;
        case "Encompass eClose Setup":
          this.subTitle = "Encompass eClose Setup";
          break;
        case "Enhanced Condition Sets":
          this.subTitle = "Use the Enhanced Condition set tool to create enhanced condition sets.";
          break;
        case "Enhanced Conditions":
          this.subTitle = "Use the Enhanced Conditions tool to create conditions that you can add to a loan.";
          break;
        case "Escrow":
          this.subTitle = "Create tables of escrow fees for purchase or refinance loans.";
          break;
        case "External Company Setup":
          this.isFirstLevelPage = true;
          break;
        case "FHA County Limits":
          this.subTitle = "Manage the table that contains the maximum FHA loan amount allowed for each county.";
          break;
        case "FHA Informed Consumer Choice Disclosure":
          this.subTitle = "The following table includes data that will be populated by default in the FHA Informed Consumer Choice Notice.  It is recommended that lenders review and revise the data in this setting if necessary at least once a year to reflect prevailing market conditions.  ";
          break;
        case "Fannie Mae UCD Transfer":
          this.subTitle = "Use the Fannie Mae UCD Transfer service settings to enable and configure aggregators.";
          break;
        case "Fannie Mae Workflow":
          this.subTitle = "Use the Fannie Mae Workflow settings to enable and configure the Desktop Underwriting and EarlyCheck workflow for your Fannie Mae users.";
          break;
        case "Federal Threshold Adjustments":
          this.subTitle = "View and synchronize the annually adjusted thresholds for Qualified Mortgages (QM) and High-Cost Mortgages (HOEPA).";
          break;
        case "Fee Groups":
          this.subTitle = "Create Fee group to be associated with Fee rules.";
          break;
        case "Fee Rules":
          this.subTitle = "Create fee rules to manage automated fee data population.";
          break;
        case "Field Data Entry":
          this.subTitle = "Create and manage rules that control the values entered in loan fields.";
          break;
        case "Field Rules":
          this.subTitle = "Create field rules to manage automated field data population.";
          break;
        case "Field Triggers":
          this.subTitle = "Create and manage rules that execute custom actions when the value in a field is changed.";
          break;
        case "Flood Service":
          this.subTitle = "The Encompass Flood Service provides flood hazard compliance services to help you meet federal regulatory and secondary market requirements.";
          break;
        case "Fraud Service":
          this.subTitle = "The Encompass Fraud Service enables you to quickly identify a loan's fraud risk prior to funding.";
          break;
        case "Freddie Mac LPA Batch":
          this.subTitle = "Use the Freddie Mac LPA Batch service settings to configure the service.";
          break;
        case "Freddie Mac Loan Assignment":
          this.subTitle = "Use the Freddie Mac Loan Assignment service settings to enable and configure aggregators.";
          break;
        case "Freddie Mac Order Alert":
          this.subTitle = "Use the Freddie Mac Order Alert settings to enable and configure order alerts for Loan Product Advisor and Loan Quality Advisor for your Freddie Mac users.";
          break;
        case "Funding Templates":
          this.subTitle = "Create templates to select fee deductions typically used on the Funding Worksheet.";
          this.subTitle = "Create templates of the fee deductions typically used on the Funding Worksheet.";
          break;
        case "Ginnie Mae PDD Batch":
          this.subTitle = "Use the Ginnie Mae PDD Batch settings to add custodian and ACH bank account information.";
          break;
        case "Global DDM Settings":
          this.subTitle = "Configure when the DDM rules apply on a loan. Set the start condition and the stop condition.";
          break;
        case "Grant File Access":
          this.subTitle = "Grant or revoke loan file access rights.";
          break;
        case "HELOC Table":
          this.subTitle = "Create the example data used on the HELOC custom form, \"Important Terms of Our Home Equity Line of Credit.\"";
          break;
        case "HMDA Profiles":
          this.subTitle = "Enter the Filing Institution and Contact Information (Transmittal Sheet) for HMDA Row 1 Reporting. Then select the default content that displays in the HMDA Input form when you start a new loan.";
          break;
        case "HTML Email Templates":
          this.subTitle = "Create and configure HTML email templates for communications sent from the eFolder. A separate set of templates is used for loans that originate from Consumer Connect websites.";
          break;
        case "ICE Mortgage Technology Network Company Password":
          this.subTitle = "Change the company password created by the system administrator during the initial installation of Encompass.";
          break;
        case "ICE PPE Loan Program Table":
          this.subTitle = "This table is used to select the ICE PPE Loan Programs that will be associated with a Correspondent Trade for a subsequent program eligibility and pricing check. Table entries are populated directly from ICE PPE.";
          break;
        case "Input Form List":
          this.subTitle = "Create and manage rules that determine the available input forms for a loan file.";
          break;
        case "Input Form Sets":
          this.subTitle = "Create sets (templates) of forms to display in the input forms list on the loan workspace.";
          break;
        case "Insights Setup":
          this.subTitle = "The Elli Mae Insights helps in analyzing your loan portfolio and compares it against industry.";
          break;
        case "Investor Connect Setup":
          this.isFirstLevelPage = true;
          break;
        case "Investor Templates":
          this.subTitle = "Create templates of investor information to apply to fields on Trade Management screens as well as the Secondary Lock, Secondary Registration, and Shipping Details tools.";
          break;
        case "Itemization Fee Management":
          this.subTitle = "Map fees to the Encompass Compliance Service, map fees to UCD fees, manage Itemization fee lists, and grant persona rights to overwrite fee lists.";
          break;
        case "LO Compensation":
          this.subTitle = "Create LO Compensation Plans by specifying compensation criteria that will be used for loan originator.";
          break;
        case "LO Compensation Rule":
          this.subTitle = "Remain in compliance by preventing originator compensation from being paid by the borrower and a 3rd party source.";
          break;
        case "Loan Action Completion":
          this.subTitle = "Create and manage business requirements that must be met before a loan action is completed.";
          break;
        case "Loan Custom Fields":
          this.subTitle = "Create custom loan fields to meet your specific business requirements.";
          break;
        case "Loan Duplication":
          this.subTitle = "Create loan duplication templates for different personas.";
          break;
        case "Loan Error Information":
          this.subTitle = "View details of loan error information.";
          break;
        case "Loan Folder Business Rule":
          this.subTitle = "Create and manage rules that authorize users to originate and move loans in a folder.";
          break;
        case "Loan Folders":
          this.subTitle = "Create loan folders to organize loans into groups.";
          break;
        case "Loan Form Printing":
          this.subTitle = "Create and manage rules that control the print forms.";
          break;
        case "Loan Pricing Decimal Places":
          this.subTitle = "Configure the number of decimal places to show in loan pricing fields.";
          break;
        case "Loan Program Additional Fields":
          this.subTitle = "Configure additional fields to be included in Loan Program templates.";
          break;
        case "Loan Programs":
          this.subTitle = "Create templates of predefined values that apply primarily to RegZ-LE disclosures and the 1003 application.";
          break;
        case "Loan Reassignment":
          this.subTitle = "Assign a different user to one or more loans.  For example, if a user leaves the company, use this feature to assign a different user to his loans.";
          break;
        case "Loan Setup":
          this.isFirstLevelPage = true;
          break;
        case "Loan Template Sets":
          this.subTitle = "Create templates of loan data for frequently-used loan scenarios and specify if a template is required when importing Fannie Mae files or WebCenter applications.";
          break;
        case "Loan Templates":
          this.isFirstLevelPage = true;
          break;
        case "Lock Comparison Tool Fields":
          this.subTitle = "";
          break;
        case "Lock Desk Setup":
          this.subTitle = "";
          break;
        case "Lock Request Additional Fields":
          this.subTitle = "Configure additional fields for the Lock Request Form and the Loan Snapshot in the Secondary Registration tool.";
          break;
        case "Log":
          this.subTitle = "Configure the Loan Log settings.";
          break;
        case "MI Tables":
          this.subTitle = "Create and manage Mortgage Insurance (MI) tables. When working in the MIP/PMI/Guarantee Fee Calculation window, this data is used to calculate MI when the Get MI button is clicked.";
          break;
        case "Milestone Completion":
          this.subTitle = "Create and manage business requirements that must be met before a milestone is finished.";
          break;
        case "Milestones":
          if (Session.EncompassEdition == EncompassEdition.Broker)
          {
            this.subTitle = "Set up the milestones to apply to loan files.";
            break;
          }
          this.subTitle = "Set up the milestones to apply to loan files. You can also create custom milestone templates to apply to loans based on loan channel, loan type and other criteria.";
          break;
        case "Mortgage Insurance Service":
          this.subTitle = "Use the Mortgage Insurance Service settings to enable and configure order triggers for Arch, Essent, Enact, MGIC, National MI and Radian.";
          break;
        case "My Email Server Settings":
          this.subTitle = "Specify your personal preferences and server settings used to send mail merges from the Contacts Manager.";
          break;
        case "My Profile":
          this.subTitle = "Update password, name, and contact information in your Encompass user profile.";
          break;
        case "NMLS Report Setup":
          this.subTitle = "Select the states for which your company is required to submit an NMLS Mortgage Call Report.";
          break;
        case "Normalized Bid Tape Template":
          this.subTitle = "";
          break;
        case "Notification Templates":
          this.subTitle = "Create and configure templates for communications sent from the eFolder.";
          break;
        case "Organization/Users":
          this.subTitle = "Create and maintain a hierarchy of your organization and user accounts.";
          break;
        case "Partner Setup":
          this.subTitle = "Use these settings to manage the configuration of your Investor Connect products, and loans received through them.";
          break;
        case "Persona Access to Fields":
          this.subTitle = "Create and manage field-specific access rules by persona.";
          break;
        case "Persona Access to Loan Actions":
          this.subTitle = "Create and manage Persona Access to Loan Actions rules.";
          break;
        case "Persona Access to Loans":
          this.subTitle = "Create and manage loan access rules by persona.";
          break;
        case "Personal Settings":
          this.isFirstLevelPage = true;
          break;
        case "Personal Status Online":
          this.subTitle = "Set up personal status update templates to send manually or to use with company status online updates. Changes to the setting apply only to loans created after the change.";
          break;
        case "Personas":
          this.subTitle = "Personas represent job functions in your company. Each persona defines access to functions, forms, and tools in Encompass.";
          break;
        case "Piggyback Loan Synchronization":
          this.subTitle = "Create a list of the fields that are synchronized when you update a loan using the Piggyback Loans tool.";
          break;
        case "Pipeline Refresh":
          this.subTitle = "Set the automatic refresh frequency for the Pipeline.";
          break;
        case "Post-Closing Condition Sets":
          this.subTitle = "Create sets of conditions that must be met after the loan is closed. ";
          break;
        case "Post-Closing Conditions":
          this.subTitle = "Create conditions that must be met after the loan is closed. ";
          break;
        case "Print Auto Selection":
          this.subTitle = "Create and manage rules that selects forms automatically in the Print window.";
          break;
        case "Print Form Groups":
          this.subTitle = "Create sets of form groups to meet the requirements of various loan scenarios or particular lenders.";
          break;
        case "Privacy Policy":
          this.subTitle = "Select the version of the Privacy Policy statement to use when printing or ordering disclosures. Then select the default content that displays in the Privacy Policy input form when you start a new loan.";
          break;
        case "Product and Pricing":
          this.subTitle = "Select your Product and Pricing provider and configure integration options with the Encompass Lock Request Form and Secondary Registration tool.";
          break;
        case "Public Business Contact Groups":
          this.subTitle = "Create groups of public business contacts for which you can specify access by user groups.";
          break;
        case "Purchase Advice Form":
          this.subTitle = "Configure payout options and create templates for the Purchase Advice Form.";
          break;
        case "Purchase Condition Sets":
          this.subTitle = "Use the Purchase Condition Sets tool to create sets of Purchase Conditions to meet the requirements of different loan criteria (e.g. FHA loans, Construction loans).";
          break;
        case "Purchase Conditions":
          this.subTitle = "Use the Purchase Conditions settings tool to create Purchase Conditions that can be added to a Loan, Purchase Condition Set, or Automatic Purchase Condition.";
          break;
        case "RESPA":
          this.subTitle = "Place a check mark next to the desired statements. When you start a new loan, this information is displayed on the RESPA Servicing Disclosure.";
          break;
        case "Rebuild Field Search Data":
          this.subTitle = "Rebuild Field Search Data.";
          break;
        case "Rebuild Pipeline":
          this.subTitle = "Synchronize the contents of loan folders with the folders on the server.";
          break;
        case "Restore Warning Messages":
          this.subTitle = "Restore the default warning messages selected below.";
          break;
        case "Role Access to Documents":
          this.subTitle = "Set up access to documents by user roles.";
          break;
        case "Roles":
          this.subTitle = "Create roles to be assigned to each milestone in the workflow.";
          break;
        case "SRP Templates":
          this.subTitle = "Create templates of SRP adjustments to apply to loans in a trade.";
          break;
        case "SSN Verification Service":
          this.subTitle = "Encompass SSN Verification Service gives you direct access to the Social Security Administration (SSA) to verify your applicant’s name and Social Security Number (SSN).";
          break;
        case "Secondary Lock Fields":
          this.subTitle = "Create dropdown values of common base price, profitability, lock type, base rate and base ARM margin adjustments used on the Lock Request and Secondary Registration tool.";
          break;
        case "Secondary Setup":
          this.isFirstLevelPage = true;
          break;
        case "Services Password Management":
          this.subTitle = "Create and manage log in credentials for service providers.";
          break;
        case "Servicing":
          this.subTitle = "Select forms and set the due date for printing/mailing mortgage statements. Enter state regulations for late fees.";
          break;
        case "Settings Overview":
          this.subTitle = "The features and tools on the Company Settings and Personal Settings are used to define, configure, and manage Encompass.";
          this.isFirstLevelPage = true;
          break;
        case "Settings Reports":
          this.subTitle = "Generate new reports and view completed reports";
          break;
        case "Settlement Service Providers":
          this.subTitle = "Create sets (templates) of settlement service providers.";
          break;
        case "Special Feature Codes":
          this.subTitle = "Configure list of Special Feature Codes";
          break;
        case "State Tax":
          this.subTitle = "Create formulas that calculate state tax for a loan.";
          break;
        case "Sync Templates":
          this.subTitle = "Create sync templates that can be applied when users link two loans together. When a sync template is applied to the loans, the data in the fields specified in the template will be synchronized between the loans.";
          break;
        case "System Administration":
          this.isFirstLevelPage = true;
          break;
        case "System Audit Trail":
          this.subTitle = "Search for entries in the system audit trail, based on selected criteria.";
          break;
        case "TPO Connect Site Management":
          this.subTitle = "Use this setting to manage TPO Connect website settings.";
          break;
        case "TPO Custom Fields":
          this.subTitle = "Create global custom fields for TPOs.";
          break;
        case "TPO Disclosure Settings":
          this.subTitle = "Use this setting to manage the behavior and content of specific disclosure documents that are generated on behalf of the TPO.";
          break;
        case "TPO Docs":
          this.subTitle = "Use this setting to manage the documents TPOs can access from your TPO site.";
          break;
        case "TPO Fees":
          this.subTitle = "This area is used to maintain both TPO Fees and Late Fees that apply to the Correspondent Purchase Advice. Use the area below to maintain standard TPO Fees.\r\nClick the Late Fee Settings button to maintain Late Fees. Please Note: There are Fee settings in Company Details that correspond to these TPO Fees.";
          break;
        case "TPO Global Lender Contacts":
          this.subTitle = "Manage the Global Lender Contacts that will be combined with TPO's Sales Reps/AE contacts to populate the Company Contacts webpage generated by TPO Connect for each TPO.\r\nThe channel selection is only for display grouping purposes. When no channel is selected for a given contact, it will fall into an 'all channels' group.";
          break;
        case "TPO Reassignment":
          this.subTitle = "Assign a new sales rep to TPO organizations and contacts. If a sales rep is no longer with your company, use this screen to assign the TPO organizations and contacts they were managing to a new sales rep.";
          break;
        case "TPO Settings":
          this.subTitle = "Create and manage custom values for drop-down lists in the Company Details and Contact Details windows.";
          break;
        case "TPO WebCenter Site Management":
          this.subTitle = "Use this setting to manage TPO website settings.";
          break;
        case "TQL Services":
          this.subTitle = "Please contact your account executive at 1-888-955-9100 to sign up with any investor listed below.";
          break;
        case "Tables and Fees":
          this.isFirstLevelPage = true;
          break;
        case "Task Sets":
          this.subTitle = "Create sets (templates) of tasks to meet the requirements of various loan scenarios or particular lenders.";
          break;
        case "Tasks":
          this.subTitle = "Create and manage the task information.";
          break;
        case "Temporary Buydown":
          this.subTitle = "Create temporary buydown types with terms that will populate into a loan when selected.";
          break;
        case "Title":
          this.subTitle = "Create tables of title fees for purchase or refinance loans.";
          break;
        case "Title Service":
          this.subTitle = "Agents pay a fee for orders accepted via the Title Center. Set up credit card accounts that can be used to pay this fee for them.";
          break;
        case "Trade Management Setup":
          this.subTitle = "This settings screen includes dropdown list management, feature enablement, and other settings that affect the behavior of the Trades feature of Encompass. The various subsection will state which type(s) of Trade(s) that subsection pertains to.";
          break;
        case "Transcript Request Templates":
          this.subTitle = "Create templates of predefined values for Request for Transcripts to the IRS. They may be applied in the Request for Transcript Form with Add Template button.";
          break;
        case "Trustee List":
          this.subTitle = "Create a trustee with whom your company does business. Users can select a trustee from this list when completing the Closing Vendor Information form.";
          break;
        case "Unlock Loan File":
          this.subTitle = "Unlock a read-only file.";
          break;
        case "Unlock Trade":
          this.subTitle = "Unlock a pending trade.";
          break;
        case "User Defined Fee":
          this.subTitle = "Create formulas that calculate fees for a loan.";
          break;
        case "User Groups":
          this.subTitle = "Create groups of users that define access to loan and contact data.";
          break;
        case "Valuation Service":
          this.subTitle = "Valuation Services are ordered from CoreLogic via the TQL Services tool for loans published to Citibank. Before enabling users, send email to CorrespondentPrograms@corelogic.com to sign up for CoreLogic's Correspondent Collateral Valuation Program (CCVP).";
          break;
        case "Verification Contact Setup":
          this.subTitle = "Use this setting to specify the user contact information to display in the \"From\" section on all printed verification forms.";
          break;
        case "WebCenter Configuration":
          this.subTitle = "Configure and modify your webcenter settings";
          break;
        case "Zipcode Setup":
          this.subTitle = "Create zipcode that currently is not in Encompass zipcode database.";
          break;
        case "e360Select Setup":
          this.subTitle = "e360Select matches exclusive programs from ICE Mortgage Technology Network partners to loan files in real time based on the details of each loan.";
          break;
        case "eDisclosure Fulfillment":
          this.subTitle = "Enable the Fulfillment service.";
          break;
        case "eDisclosure Packages":
          this.subTitle = "Use the eDisclosures tool to select the eDisclosure packages to send to borrowers and to set the eSigning option for your loans.";
          break;
        case "eDisclosure Plan Codes":
          this.subTitle = "Define the Plan Codes used by your company when ordering eDisclosures.";
          break;
        case "eDisclosure Stacking Templates":
          this.subTitle = "Define the Stacking Templates used to sort documents when ordering eDisclosures.";
          break;
        case "eFolder Setup":
          this.isFirstLevelPage = true;
          break;
        default:
          throw new Exception("No page sub-title defined for '" + this.Title + "'; Please check SetUpContainer.SetupPage.SetupPage()");
      }
    }

    private void setShowParentSaveResetButtons()
    {
      switch (this.Title)
      {
        case "2009 GFE Print":
        case "Affordable Lending":
        case "Auto Loan Numbering":
        case "Auto MERS MIN Numbering":
        case "Auto-Lock":
        case "Autosave Configuration":
        case "Borrower Contact Update":
        case "Borrower Custom Fields":
        case "Business Custom Fields":
        case "Channel Options":
        case "Collateral Tracking":
        case "Company Information":
        case "Compliance Audit Settings":
        case "Compliance Calendar":
        case "Conventional County Limits":
        case "Correspondent Purchase Advice":
        case "Default File Contacts":
        case "Default Input Forms":
        case "Default Template Setting":
        case "Disclosure Tracking Settings":
        case "Document Conversion":
        case "Document Groups":
        case "Document Identification":
        case "Email Server Settings":
        case "FHA County Limits":
        case "FHA Informed Consumer Choice Disclosure":
        case "Federal Threshold Adjustments":
        case "Global DDM Settings":
        case "ICE PPE Loan Program Table":
        case "Itemization Fee Management":
        case "LO Compensation Rule":
        case "Loan Pricing Decimal Places":
        case "Loan Program Additional Fields":
        case "Lock Desk Setup":
        case "Lock Request Additional Fields":
        case "Log":
        case "My Email Server Settings":
        case "My Profile":
        case "NMLS Report Setup":
        case "Piggyback Loan Synchronization":
        case "Pipeline Refresh":
        case "Privacy Policy":
        case "Product and Pricing":
        case "RESPA":
        case "Restore Warning Messages":
        case "Secondary Lock Fields":
        case "Servicing":
        case "TPO Connect Site Management":
        case "TPO Custom Fields":
        case "TPO Disclosure Settings":
        case "TPO WebCenter Site Management":
        case "Trade Management Setup":
        case "Verification Contact Setup":
        case "WebCenter Configuration":
        case "e360Select Setup":
        case "eDisclosure Packages":
          this.containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.ShowBoth;
          break;
        case "Deliver Loans":
        case "Partner Setup":
          this.containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.DoNotShow;
          break;
        case "ICE Mortgage Technology Network Company Password":
          this.containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.ShowBothButDisableReset;
          break;
        case "Persona Access to Loans":
          if (Session.EncompassEdition != EncompassEdition.Broker)
            break;
          this.containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.ShowBoth;
          break;
        default:
          this.containerSaveResetBtns = SetupPage.ContainerSaveResetButtons.DoNotShow;
          break;
      }
    }

    internal void setSubtitleText(string text) => this.subTitle = text;

    internal enum AccessControl
    {
      All,
      Admin,
      TopLevelAdmin,
      SuperAdmin,
      ConditionalApprovalLetters,
      ConditionSetup,
      DocumentSetup,
      PostClosingConditionSetup,
      PublicBizContactGroup,
      MiscDataTemplates,
      DefaultFileContacts,
      GrantFileAccess,
      AutosaveConfiguration,
      StatusOnlineConfiguration,
      PipelineRefresh,
      AdminUser,
      ComplianceSetup,
      AllTPOInformation,
      CompanyUserSetup,
      CompanyInformation,
      EpassCompanyPassword,
      EPassPasswordManagement,
      Personas,
      OrganizationUsers,
      Roles,
      Milestones80,
      UserGroups,
      ExternalCompanySetup,
      CompanySetup,
      CompanySettingSetup,
      TPOFees,
      TPOReassignment,
      TPOCustomFields,
      TPOAllContactInfo,
      TPOGlobalLenderContact,
      TPOWCDocs,
      TPODTSetting,
      TPOConnectSiteMngmnt,
      TPOWCSiteMngmnt,
      LoanSetup,
      AutoLoanNumbering,
      AutoMersMinNumbering,
      LoanFolders,
      LoanDuplication,
      Alerts,
      Log,
      Tasks,
      DefaultInputForms,
      ConditionalLetterOption,
      LoanCustomFields,
      ConfigurableKeyDates,
      ChannelOptions,
      RESPA,
      ChangeCircumstanceSetup,
      DisclosureTracking,
      ComplianceCalendar,
      GfePrint2009,
      TrusteeList,
      PiggybackLoanSynchronization,
      SyncTemplates,
      PrivacyPolicyForm,
      ZipcodeSetup,
      HMDA,
      NMLSSetup,
      VerifContactSetup,
      FHAInformedConsumerChoiceDisclosure,
      EfolderSetup,
      DocumentConversion,
      Documents,
      DocumentExportTemplates,
      DocumentGroups,
      DocumentStackingOrders,
      Barcodes,
      DocumentTraining,
      EnhancedConditions,
      Conditions,
      ConditionSets,
      PostClosingConditions,
      PostClosingConditionSets,
      PurchaseConditions,
      PurchaseConditionSets,
      HtmlEmailTemplates,
      NotificationTemplates,
      WebCenterConfiguration,
      DocServices,
      EDisclosures,
      EDisclosurePlanCodes,
      EDisclosureStackingTemplates,
      ClosingPlanCodes,
      ClosingStackingTemplates,
      ComplianceAuditSettings,
      SecondarySetup,
      ProductAndPricing,
      SecondaryLockFields,
      LockRequestAdditionalFields,
      LockComparisonToolFields,
      AutoLock,
      InvestorTemplates,
      EPPSLoanProgram,
      TradeManagementFields,
      NormalizedBidTapeTemplate,
      AdjustmentTemplates,
      LockExpirationDate,
      SrpTemplates,
      FundingTemplates,
      Servicing,
      CorrespondentPurchaseAdvice,
      PurchaseAdviceForm,
      LoanPricingDecimalPlaces,
      ContactSetup,
      BorrowerCustomFields,
      BorrowerContactStatus,
      BorrowerContactUpdate,
      BusinessCustomFields,
      BusinessCategories,
      PublicBusinessContactGroups,
      EmailServerSettings,
      TablesAndFees,
      EscrowTable,
      TitleTable,
      HelocTable,
      HelocIndexTable,
      MiTables,
      FhaCountyLimits,
      ConventionalCountyLimits,
      FedTresholdAdjustments,
      AMILimits,
      CityTax,
      StateTax,
      UserDefinedFee,
      ItemizationFeeManagement,
      LOCompensation,
      TemporaryBuydown,
      SpecialFeatureCodes,
      BusinessRules,
      LoanFolderBusinessRule,
      MilestoneCompletion,
      LoanActionCompletion,
      FieldDataEntry,
      FieldTriggers,
      AutomatedConditions,
      AutomatedEnhancedConditions,
      PersonaAccessToFields,
      PersonaAccessToLoans,
      PersonaAccessToLoanActions,
      RoleAccessToDocuments,
      InputFormList,
      LoanFormPrinting,
      PrintAutoSelection,
      AppraisalManagement,
      LOCompensationControl,
      DocumentTrackingMgmt,
      DynamicDataManagement,
      FeeRules,
      FieldRules,
      DataTables,
      FeeGroups,
      DataPopulationTiming,
      SystemAdministration,
      AnalysisTools,
      RebuildPipeline,
      CurrentLogins,
      AllUserInformation,
      SettingsReport,
      LoanReassignment,
      UnlockLoanFile,
      UnlockTrade,
      SystemAuditTrail,
      LicenseManagement,
      EdocumentManagement,
      StatusOnline,
      Fulfillment,
      EncompassAIQ,
      ComplianceReview,
      TaxReturnService,
      TQLServices,
      AppraisalCenter,
      TitleCenter,
      FraudService,
      FannieService,
      FreddieService,
      MIService,
      FreddieMacCACService,
      FreddieMacLPAService,
      FannieMaeUCDTransferService,
      GinnieMaePddService,
      AvmService,
      FloodService,
      DataTracService,
      InsightsSetup,
      PersonalSettings,
      CustomPrintForms,
      PrintFormGroups,
      LoanPrograms,
      ClosingCosts,
      InputFormSets,
      SettlementServiceProviders,
      AffiliatedBusinessArrangements,
      DocumentSets,
      IRS4506TTemplates,
      TaskSets,
      DataTemplates,
      LoanTemplateSets,
      InvestorConnectSettings,
      EnhancedConditionSets,
      eClose,
      InvestorConnectSetup,
      DeliverLoans,
      PartnerSetup,
      MFILimits,
    }

    internal enum ContainerSaveResetButtons
    {
      ShowBoth,
      ShowBothButDisableReset,
      DoNotShow,
    }
  }
}
