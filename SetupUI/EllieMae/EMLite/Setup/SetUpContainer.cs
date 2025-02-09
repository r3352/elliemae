// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.SetUpContainer
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.CustomLetters;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Import;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.Autosave;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.Setup.eFolder;
using EllieMae.EMLite.Setup.ePass;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.Features;
using EllieMae.EMLite.Setup.LoanServices;
using EllieMae.EMLite.Setup.ProductPricing;
using EllieMae.EMLite.Setup.SecondaryMarketing;
using EllieMae.EMLite.Setup.StatusOnline;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.AsmResolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class SetUpContainer : Form, IOnlineHelpTarget, ISetup
  {
    private const string className = "SetUpContainer";
    private static string sw = Tracing.SwOutsideLoan;
    private GradientMenuStrip mainFormMenuStrip;
    private ToolStripMenuItem helpMenuItem;
    private readonly TreeViewEventHandler treeViewEventHandler;
    private Image bgImage;
    private Form mainForm;
    private IMainScreen mainScreen;
    private FeaturesAclManager aclMgr;
    private ContextMenu contextMenu1;
    private MenuItem miExpand;
    private MenuItem miCollapse;
    private GradientMenuStrip gradientMenuStrip1;
    private ToolStripMenuItem tsMenuItemGoToRecent;
    private ToolStripMenuItem tsMenuItemImport;
    private Panel pnlContent;
    private PanelEx pnlRight;
    private SetUpDialog setupDialog;
    private CollapsibleSplitter collapsibleSplitter1;
    private Button btnClose;
    private ToolStripMenuItem tsMenuItemPointLP;
    private ToolStripMenuItem tsMenuItemPointCC;
    private ToolStripMenuItem tsMenuItemPointSettings;
    private ToolStripMenuItem tsMenuItemCustomForms;
    private PanelEx pnlExBottom;
    private BorderPanel bpTree;
    private ToolStripMenuItem tsMenuItemEncompassSettings;
    private ToolStripMenuItem tsMenuItemClose;
    private Sessions.Session session;
    private CustomFieldsUserControl customFieldsUserControl;
    private AnalysisTools analysisTools;
    private static bool assignedToAllowLoanErrorInfo = false;
    internal bool IsThickFormUnload;
    private OrgHierarchyForm orgHierarchyForm;
    private AllTPOUserPanel tpoPanel;
    private InvestorConnectSettings investorConnectSettingsPanel;
    private string currentPage = string.Empty;
    private System.ComponentModel.Container components;
    private TreeView treeView;
    private bool suspendBeforeSelect;

    public SetUpContainer(
      Form mainForm,
      IMainScreen mainScreen,
      GradientMenuStrip mainMenu,
      ToolStripMenuItem helpMenuItem,
      Sessions.Session session)
    {
      this.session = session;
      this.bgImage = Image.FromFile(AssemblyResolver.GetResourceFileFullPath(SystemSettings.SettingsSplashImgFileRelPath, SystemSettings.LocalAppDir));
      this.mainForm = mainForm;
      this.mainScreen = mainScreen;
      this.setupDialog = new SetUpDialog();
      this.setupDialog.TopBorder = false;
      this.InitializeComponent();
      this.mainFormMenuStrip = mainMenu;
      this.helpMenuItem = helpMenuItem;
      this.gradientMenuStrip1.Items.Add((ToolStripItem) this.helpMenuItem);
      this.collapsibleSplitter1.BackColor = EncompassColors.Primary1;
      this.pnlRight.Controls.Add((Control) this.setupDialog);
      this.setImportMenuItemsAccess();
      this.tsMenuItemImport.Visible = true;
      this.Text = "Encompass  Settings";
      this.treeViewEventHandler = new TreeViewEventHandler(this.treeView_AfterSelect);
      this.treeView.AfterSelect += this.treeViewEventHandler;
      this.initNodes();
      this.addAndRefreshRecent((string) null);
      this.selectFirstPage();
    }

    public void EditSelectedLoanCustomField()
    {
      if (this.customFieldsUserControl == null)
        this.customFieldsUserControl = new CustomFieldsUserControl(Session.DefaultInstance, this, false);
      this.customFieldsUserControl.EditSelectedItem();
    }

    public void SetLoanCustomFieldId(string fieldId)
    {
      if (this.customFieldsUserControl == null)
        this.customFieldsUserControl = new CustomFieldsUserControl(Session.DefaultInstance, this, false);
      this.customFieldsUserControl.SetLoanCustomFieldId(fieldId);
    }

    private void setImportMenuItemsAccess()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.tsMenuItemCustomForms.Enabled = false;
      if (aclManager.GetUserApplicationRight(AclFeature.SettingsTab_Personal_CustomPrintForms))
        this.tsMenuItemCustomForms.Enabled = true;
      else if (Session.AclGroupManager.GetMaxPublicFolderAccess(AclFileType.CustomPrintForms) == AclResourceAccess.ReadWrite)
        this.tsMenuItemCustomForms.Enabled = true;
      if (Session.UserInfo.IsAdministrator())
        return;
      this.tsMenuItemPointLP.Enabled = false;
      this.tsMenuItemPointCC.Enabled = false;
      this.tsMenuItemPointSettings.Enabled = false;
    }

    private void goToRecentEventHandler(object sender, EventArgs e)
    {
      TreeNode node = this.findNode(((ToolStripItem) sender).Text);
      if (node == null)
        return;
      this.treeView.SuspendLayout();
      this.treeView.CollapseAll();
      this.treeView.ResumeLayout(true);
      this.treeView.SelectedNode = node;
    }

    private void addAndRefreshRecent(string pageTitle)
    {
      this.tsMenuItemGoToRecent.DropDownItems.Clear();
      string[] recentSettings = Session.User.AddRemoveAndGetRecentSettings(true, pageTitle, true, 10, true);
      if (recentSettings == null)
        return;
      foreach (string text in recentSettings)
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text);
        toolStripMenuItem.Click += new EventHandler(this.goToRecentEventHandler);
        this.tsMenuItemGoToRecent.DropDownItems.Add((ToolStripItem) toolStripMenuItem);
      }
    }

    private void initNodes()
    {
      this.aclMgr = (FeaturesAclManager) Session.ACL.GetAclManager(AclCategory.Features);
      this.buildTree();
    }

    private void buildTree()
    {
      SetupPage setupPage = SetupPage.GetSetupPage("Settings Overview");
      this.treeView.Nodes.Add(new TreeNode(setupPage.Title)
      {
        Tag = (object) setupPage
      });
      this.buildSettingsTree();
    }

    private bool isLoanErrorInfoAccessible()
    {
      if (SetUpContainer.assignedToAllowLoanErrorInfo)
        return this.session.StartupInfo.AllowLoanErrorInfo;
      bool flag = false;
      try
      {
        flag = Modules.GetModuleLicense(EncompassModule.ConsumerConnect, this.session) != null && AccessUtils.UserCanAccessLoanErrorInformation();
      }
      catch (Exception ex)
      {
      }
      this.session.StartupInfo.AllowLoanErrorInfo = flag;
      SetUpContainer.assignedToAllowLoanErrorInfo = true;
      return flag;
    }

    private bool isAccessible(SetupPage setupPage, Hashtable publicAccess)
    {
      if (!setupPage.AppliesToEdition(Session.EncompassEdition))
        return false;
      if (setupPage.Title == "FHA Informed Consumer Choice Disclosure" && Session.UserInfo.IsSuperAdministrator())
        return true;
      if (setupPage.Title == "Compliance Review Setup" && !LoanServiceManager.ComplianceReviewInstalled || setupPage.Title == "4506C Service" && !LoanServiceManager.TaxReturnInstalled || setupPage.Title == "SSN Verification Service" && !LoanServiceManager.SSNInstalled || setupPage.Title == "e360Select Setup" && !LoanScreeningManager.IsFeatureEnabled() || setupPage.Title == "Appraisal Service" && !LoanServiceManager.PaymentSettingsInstalled || setupPage.Title == "Title Service" && !LoanServiceManager.PaymentSettingsInstalled || setupPage.Title == "Fraud Service" && !LoanServiceManager.FraudInstalled || setupPage.Title == "Fannie Mae Workflow" && !LoanServiceManager.FannieInstalled || setupPage.Title == "Freddie Mac Order Alert" && !LoanServiceManager.FreddieInstalled || setupPage.Title == "Mortgage Insurance Service" && !LoanServiceManager.MiServiceInstalled || setupPage.Title == "Freddie Mac Loan Assignment" && !LoanServiceManager.FreddieMacCACServiceInstalled || setupPage.Title == "Freddie Mac LPA Batch" && !LoanServiceManager.FreddieMacLPABatchServiceInstalled || setupPage.Title == "Ginnie Mae PDD Batch" && !LoanServiceManager.GinnieMaePDDBatchServiceInstalled || setupPage.Title == "Fannie Mae UCD Transfer" && !LoanServiceManager.FannieMaeUCDTransferServiceInstalled || setupPage.Title == "Valuation Service" && !LoanServiceManager.AVMInstalled || setupPage.Title == "DataTrac Connection" && !LoanServiceManager.DataTracInstalled || setupPage.Title == "TQL Services" && !LoanServiceManager.TQLClientInstalled || setupPage.Title == "Flood Service" && !LoanServiceManager.FloodInstalled || (setupPage.Title == "Collateral Tracking" || setupPage.Title == "Persona Access to Loan Actions") && Session.UserInfo.IsAdministrator() && !Session.UserInfo.IsTopLevelAdministrator() || setupPage.Title == "Rebuild Pipeline" && Session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted && !Session.Connection.IsServerInProcess && !(bool) Session.StartupInfo.ComponentSettings[(object) "Components.RebuildPipeline"])
        return false;
      ILoanServices service = Session.Application.GetService<ILoanServices>();
      if ((setupPage.Title == "eDisclosure Plan Codes" || setupPage.Title == "eDisclosure Stacking Templates") && !service.IsEncompassDocServiceAvailable(DocumentOrderType.Opening) || (setupPage.Title == "Closing Doc Plan Codes" || setupPage.Title == "Closing Doc Stacking Templates" || setupPage.Title == "Compliance Audit Settings") && !service.IsEncompassDocServiceAvailable(DocumentOrderType.Closing))
        return false;
      if (setupPage.Title == "Rebuild Field Search Data")
        return AccessUtils.CanAccessFeature(Feature.RebuildFieldSearchData);
      if (setupPage.Title == "Loan Error Information")
        return this.isLoanErrorInfoAccessible();
      if (setupPage.Title == "Insights Setup" && (!this.session.StartupInfo.AllowInsightsSetup || this.session.IsBrokerEdition()))
        return false;
      if (publicAccess == null && SetupPage.LoanTemplatesPages.Contains(setupPage.Title))
        publicAccess = Session.AclGroupManager.CheckPublicAccessPermissions(FeatureSets.FileTypes);
      return setupPage.Access == SetupPage.AccessControl.All || setupPage.Access == SetupPage.AccessControl.Admin && Session.UserInfo.IsAdministrator() || setupPage.Access == SetupPage.AccessControl.TopLevelAdmin && Session.UserInfo.IsTopLevelAdministrator() || setupPage.Access == SetupPage.AccessControl.SuperAdmin && Session.UserInfo.IsSuperAdministrator() || setupPage.Access == SetupPage.AccessControl.AdminUser && Session.UserInfo.Userid.ToLower().Equals("admin") || setupPage.Access == SetupPage.AccessControl.CustomPrintForms && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_CustomPrintForms) || Session.AclGroupManager.CheckPublicAccessPermission(AclFileType.CustomPrintForms)) || setupPage.Access == SetupPage.AccessControl.PrintFormGroups && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_PrintGroups) || Session.AclGroupManager.CheckPublicAccessPermission(AclFileType.PrintGroups)) || setupPage.Access == SetupPage.AccessControl.LoanCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LoanCustomFields) || setupPage.Access == SetupPage.AccessControl.ConditionSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionSetup) || setupPage.Access == SetupPage.AccessControl.DocumentSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_DocumentSetup) || setupPage.Access == SetupPage.AccessControl.PostClosingConditionSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionSetup) || setupPage.Access == SetupPage.AccessControl.EnhancedConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EnhancedConditions) || setupPage.Access == SetupPage.AccessControl.EnhancedConditionSets && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EnhancedConditionSets) || setupPage.Access == SetupPage.AccessControl.SecondarySetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_SecondarySetup) || setupPage.Access == SetupPage.AccessControl.PublicBizContactGroup && Session.EncompassEdition == EncompassEdition.Banker && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_PublicBizContactGroup) || setupPage.Access == SetupPage.AccessControl.LoanPrograms && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_LoanPrograms) || (bool) publicAccess[(object) AclFileType.LoanProgram]) || setupPage.Access == SetupPage.AccessControl.ClosingCosts && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_ClosingCosts) || (bool) publicAccess[(object) AclFileType.ClosingCost]) || setupPage.Access == SetupPage.AccessControl.InputFormSets && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_InputFormSets) || (bool) publicAccess[(object) AclFileType.FormList]) || setupPage.Access == SetupPage.AccessControl.SettlementServiceProviders && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_SettlementServiceProvider) || (bool) publicAccess[(object) AclFileType.SettlementServiceProviders]) || setupPage.Access == SetupPage.AccessControl.AffiliatedBusinessArrangements && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_Affiliate) || (bool) publicAccess[(object) AclFileType.AffiliatedBusinessArrangements]) || setupPage.Access == SetupPage.AccessControl.DocumentSets && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_DocumentSets) || (bool) publicAccess[(object) AclFileType.DocumentSet]) || setupPage.Access == SetupPage.AccessControl.TaskSets && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_TaskSets) || (bool) publicAccess[(object) AclFileType.TaskSet]) || setupPage.Access == SetupPage.AccessControl.MiscDataTemplates && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_MiscDataTemplates) || (bool) publicAccess[(object) AclFileType.MiscData]) || setupPage.Access == SetupPage.AccessControl.LoanTemplateSets && (this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_LoanTemplateSets) || (bool) publicAccess[(object) AclFileType.LoanTemplate]) || setupPage.Access == SetupPage.AccessControl.TablesAndFees && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_TableFee) || setupPage.Access == SetupPage.AccessControl.ItemizationFeeManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt) || setupPage.Access == SetupPage.AccessControl.AnalysisTools && this.aclMgr.GetUserApplicationRight(AclFeature.ThinThick_AnalysisTool_Access) || setupPage.Access == SetupPage.AccessControl.CurrentLogins && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_CurrentLogins) || setupPage.Access == SetupPage.AccessControl.SettingsReport && (this.aclMgr.GetUserApplicationRight(AclFeature.ViewNewSettings_Report) || this.aclMgr.GetUserApplicationRight(AclFeature.ViewOtherSettings_Report) || this.aclMgr.GetUserApplicationRight(AclFeature.CancelSettings_Report) || this.aclMgr.GetUserApplicationRight(AclFeature.CancelOtherSettings_Report) || this.aclMgr.GetUserApplicationRight(AclFeature.DeleteSettings_Report)) || setupPage.Access == SetupPage.AccessControl.DefaultFileContacts && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_DefaultFileContacts) || setupPage.Access == SetupPage.AccessControl.GrantFileAccess && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personal_AssignmentOfRights) || setupPage.Access == SetupPage.AccessControl.AutosaveConfiguration && AutosaveConfigManager.UserConfigEnabled || setupPage.Access == SetupPage.AccessControl.PipelineRefresh && this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_PipelineAutoRefresh) || setupPage.Access == SetupPage.AccessControl.StatusOnlineConfiguration && Session.UserInfo.PersonalStatusOnline && Modules.IsModuleAvailableForUser(EncompassModule.StatusOnline, false) || setupPage.Access == SetupPage.AccessControl.SystemAuditTrail && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_SystemAuditTrail) || setupPage.Access == SetupPage.AccessControl.LoanReassignment && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LoanReassignment) || setupPage.Access == SetupPage.AccessControl.ConditionalApprovalLetters && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionalApprovalLetter) || setupPage.Access == SetupPage.AccessControl.ComplianceSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ComplianceReviewSetup) || setupPage.Access == SetupPage.AccessControl.ClosingPlanCodes && this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_Other_PlanCode) || setupPage.Access == SetupPage.AccessControl.ClosingStackingTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_EMClosingDocs_ManageStackingOrders) || setupPage.Access == SetupPage.AccessControl.EDisclosurePlanCodes && this.aclMgr.GetUserApplicationRight(AclFeature.eFolder_Other_eDisclosure_ManagePlanCodes) || setupPage.Access == SetupPage.AccessControl.EDisclosureStackingTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.eFolder_Other_eDisclosure_ManageStackingTemplates) || setupPage.Access == SetupPage.AccessControl.DocumentTraining && this.aclMgr.GetUserApplicationRight(AclFeature.eFolder_UF_Approver) || setupPage.Access == SetupPage.AccessControl.CompanySetup && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_CompanyDetails) || setupPage.Access == SetupPage.AccessControl.CompanySettingSetup && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOSettings) || setupPage.Access == SetupPage.AccessControl.TPOFees && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFees) || setupPage.Access == SetupPage.AccessControl.TPOCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOCustomFields) || setupPage.Access == SetupPage.AccessControl.TPOReassignment && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOAssignment) || setupPage.Access == SetupPage.AccessControl.TPOCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOCustomFields) || setupPage.Access == SetupPage.AccessControl.AllTPOInformation && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_AllTPOInformation) || setupPage.Access == SetupPage.AccessControl.TPOGlobalLenderContact && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOGlobalLenderContact) || setupPage.Access == SetupPage.AccessControl.TPOWCDocs && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDocuments) || setupPage.Access == SetupPage.AccessControl.TPODTSetting && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPODisclosureSettings) || setupPage.Access == SetupPage.AccessControl.InvestorConnectSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_InvestorConnectSetup) || setupPage.Access == SetupPage.AccessControl.DeliverLoans && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DeliverLoans) || setupPage.Access == SetupPage.AccessControl.PartnerSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PartnerSetup) || setupPage.Access == SetupPage.AccessControl.CompanyUserSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_CompanyUser) || setupPage.Access == SetupPage.AccessControl.CompanyInformation && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_CompanyInformation) || setupPage.Access == SetupPage.AccessControl.EpassCompanyPassword && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EllieMaeNetworkCompany) || setupPage.Access == SetupPage.AccessControl.EPassPasswordManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ServicesPasswordManager) || setupPage.Access == SetupPage.AccessControl.Personas && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Personas) || setupPage.Access == SetupPage.AccessControl.OrganizationUsers && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_OrganizationsUser) || setupPage.Access == SetupPage.AccessControl.Roles && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Roles) || setupPage.Access == SetupPage.AccessControl.Milestones80 && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Milestones) || setupPage.Access == SetupPage.AccessControl.UserGroups && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_UserGroups) || setupPage.Access == SetupPage.AccessControl.ExternalCompanySetup && (this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_CompanyDetails) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOSettings) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFees) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOAssignment) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOCustomFields) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_AllTPOInformation) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOGlobalLenderContact) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDocuments) || this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPODisclosureSettings)) || setupPage.Access == SetupPage.AccessControl.CompanySetup && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_CompanyDetails) || setupPage.Access == SetupPage.AccessControl.CompanySettingSetup && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOSettings) || setupPage.Access == SetupPage.AccessControl.TPOFees && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOFees) || setupPage.Access == SetupPage.AccessControl.TPOReassignment && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOAssignment) || setupPage.Access == SetupPage.AccessControl.TPOCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOCustomFields) || setupPage.Access == SetupPage.AccessControl.TPOAllContactInfo && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_AllTPOInformation) || setupPage.Access == SetupPage.AccessControl.TPOGlobalLenderContact && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOGlobalLenderContact) || setupPage.Access == SetupPage.AccessControl.TPOWCDocs && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCDocuments) || setupPage.Access == SetupPage.AccessControl.TPOConnectSiteMngmnt && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOConnectSiteManagement) || setupPage.Access == SetupPage.AccessControl.TPOWCSiteMngmnt && this.aclMgr.GetUserApplicationRight(AclFeature.ExternalSettings_TPOWCSiteManagement) || setupPage.Access == SetupPage.AccessControl.LoanSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Loan) || setupPage.Access == SetupPage.AccessControl.AutoLoanNumbering && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AutoLoanNumbering) || setupPage.Access == SetupPage.AccessControl.AutoMersMinNumbering && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AutoMERSMINNumbering) || setupPage.Access == SetupPage.AccessControl.LoanFolders && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanFolders) || setupPage.Access == SetupPage.AccessControl.LoanDuplication && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanDuplication) || setupPage.Access == SetupPage.AccessControl.Alerts && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Alerts) || setupPage.Access == SetupPage.AccessControl.Log && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Log) || setupPage.Access == SetupPage.AccessControl.Tasks && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Tasks) || setupPage.Access == SetupPage.AccessControl.DefaultInputForms && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DefaultInputForms) || setupPage.Access == SetupPage.AccessControl.ConditionalLetterOption && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConditionalApprovalLetter) || setupPage.Access == SetupPage.AccessControl.LoanCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LoanCustomFields) || setupPage.Access == SetupPage.AccessControl.ConfigurableKeyDates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ConfigurableKeyDates) || setupPage.Access == SetupPage.AccessControl.ChannelOptions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ChannelOptions) || setupPage.Access == SetupPage.AccessControl.RESPA && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_RESPA) || setupPage.Access == SetupPage.AccessControl.ChangeCircumstanceSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ChangedCircumstancesSetup) || setupPage.Access == SetupPage.AccessControl.DisclosureTracking && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DisclosureTrackingSettings) || setupPage.Access == SetupPage.AccessControl.ComplianceCalendar && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ComplianceCalendar) || setupPage.Access == SetupPage.AccessControl.GfePrint2009 && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_2009GFEPrint) || setupPage.Access == SetupPage.AccessControl.TrusteeList && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_TrusteeList) || setupPage.Access == SetupPage.AccessControl.PiggybackLoanSynchronization && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PiggybackLoanSynchronization) || setupPage.Access == SetupPage.AccessControl.SyncTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_SyncTemplates) || setupPage.Access == SetupPage.AccessControl.PrivacyPolicyForm && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PrivacyPolicy) || setupPage.Access == SetupPage.AccessControl.ZipcodeSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ZipcodeSetup) || setupPage.Access == SetupPage.AccessControl.HMDA && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_HMDASetup) || setupPage.Access == SetupPage.AccessControl.NMLSSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_NMLSReportSetup) || setupPage.Access == SetupPage.AccessControl.VerifContactSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_VerificationContactSetup) || setupPage.Access == SetupPage.AccessControl.EfolderSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EFolder) || setupPage.Access == SetupPage.AccessControl.DocumentConversion && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentConversion) || setupPage.Access == SetupPage.AccessControl.Documents && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Documents) || setupPage.Access == SetupPage.AccessControl.DocumentExportTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentExportTemplates) || setupPage.Access == SetupPage.AccessControl.DocumentGroups && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentGroups) || setupPage.Access == SetupPage.AccessControl.DocumentStackingOrders && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentStackingTemplates) || setupPage.Access == SetupPage.AccessControl.Barcodes && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentIdentification) || setupPage.Access == SetupPage.AccessControl.EnhancedConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EnhancedConditions) || setupPage.Access == SetupPage.AccessControl.Conditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Conditions) || setupPage.Access == SetupPage.AccessControl.ConditionSets && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ConditionSets) || setupPage.Access == SetupPage.AccessControl.PostClosingConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PostClosingConditions) || setupPage.Access == SetupPage.AccessControl.PostClosingConditionSets && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PostClosingConditionSets) || setupPage.Access == SetupPage.AccessControl.PurchaseConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PurchaseConditions) || setupPage.Access == SetupPage.AccessControl.PurchaseConditionSets && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PurchaseConditionSets) || setupPage.Access == SetupPage.AccessControl.HtmlEmailTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_HTMLEmailTemplates) || setupPage.Access == SetupPage.AccessControl.NotificationTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_NotificationTemplates) || setupPage.Access == SetupPage.AccessControl.WebCenterConfiguration && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_WebCenterConfiguration) || setupPage.Access == SetupPage.AccessControl.DocServices && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Docs) || setupPage.Access == SetupPage.AccessControl.EDisclosures && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EDisclosurePackages) || setupPage.Access == SetupPage.AccessControl.ComplianceAuditSettings && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ComplianceAuditSettings) || setupPage.Access == SetupPage.AccessControl.SecondarySetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_SecondarySetup) || setupPage.Access == SetupPage.AccessControl.ProductAndPricing && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ProductandPricing) || setupPage.Access == SetupPage.AccessControl.SecondaryLockFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_SecondaryLockFields) || setupPage.Access == SetupPage.AccessControl.LockRequestAdditionalFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LockRequestAdditionalFields) || setupPage.Access == SetupPage.AccessControl.LockComparisonToolFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LockComparisonToolFields) || setupPage.Access == SetupPage.AccessControl.InvestorTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_InvestorTemplates) || setupPage.Access == SetupPage.AccessControl.EPPSLoanProgram && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EPPSLoanProgram) || setupPage.Access == SetupPage.AccessControl.TradeManagementFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_TradeManagementSetup) || setupPage.Access == SetupPage.AccessControl.NormalizedBidTapeTemplate && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_NormalizedBidTapeTemplate) || setupPage.Access == SetupPage.AccessControl.AdjustmentTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AdjustmentTemplates) || setupPage.Access == SetupPage.AccessControl.LockExpirationDate && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LockDeskSetup) || setupPage.Access == SetupPage.AccessControl.SrpTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_SRPTemplates) || setupPage.Access == SetupPage.AccessControl.FundingTemplates && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FundingTemplates) || setupPage.Access == SetupPage.AccessControl.Servicing && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Servicing) || setupPage.Access == SetupPage.AccessControl.CorrespondentPurchaseAdvice && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_CorrespondentPurchaseAdviceManagement) || setupPage.Access == SetupPage.AccessControl.PurchaseAdviceForm && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PurchaseAdviceForm) || setupPage.Access == SetupPage.AccessControl.LoanPricingDecimalPlaces && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanPricingDecimalPlaces) || setupPage.Access == SetupPage.AccessControl.ContactSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Contact) || setupPage.Access == SetupPage.AccessControl.BorrowerCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BorrowerCustomFields) || setupPage.Access == SetupPage.AccessControl.BorrowerContactStatus && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BorrowerContactStatus) || setupPage.Access == SetupPage.AccessControl.BorrowerContactUpdate && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BorrwerContactUpdate) || setupPage.Access == SetupPage.AccessControl.BusinessCustomFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BusinessCustomFields) || setupPage.Access == SetupPage.AccessControl.BusinessCategories && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BusinessCategories) || setupPage.Access == SetupPage.AccessControl.PublicBusinessContactGroups && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_PublicBizContactGroup) || setupPage.Access == SetupPage.AccessControl.EmailServerSettings && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EmailServerSettings) || setupPage.Access == SetupPage.AccessControl.TablesAndFees && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_TableFee) || setupPage.Access == SetupPage.AccessControl.EscrowTable && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Escrow) || setupPage.Access == SetupPage.AccessControl.TitleTable && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Title) || setupPage.Access == SetupPage.AccessControl.HelocTable && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_HELOCTable) || setupPage.Access == SetupPage.AccessControl.MiTables && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_MITables) || setupPage.Access == SetupPage.AccessControl.FhaCountyLimits && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FHACountyLimits) || setupPage.Access == SetupPage.AccessControl.ConventionalCountyLimits && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ConventionalCountyLimits) || setupPage.Access == SetupPage.AccessControl.FedTresholdAdjustments && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FedTresholdAdjustments) || setupPage.Access == SetupPage.AccessControl.AMILimits && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AMILimits) || setupPage.Access == SetupPage.AccessControl.MFILimits && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_MFILimits) || setupPage.Access == SetupPage.AccessControl.CityTax && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_CityTax) || setupPage.Access == SetupPage.AccessControl.StateTax && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_StateTax) || setupPage.Access == SetupPage.AccessControl.UserDefinedFee && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_UserDefinedFee) || setupPage.Access == SetupPage.AccessControl.ItemizationFeeManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_TF_2010ItemizationFeeMgmt) || setupPage.Access == SetupPage.AccessControl.LOCompensation && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LOCompensation) || setupPage.Access == SetupPage.AccessControl.TemporaryBuydown && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_TemporaryBuydown) || setupPage.Access == SetupPage.AccessControl.SpecialFeatureCodes && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_SpecialFeatureCodes) || setupPage.Access == SetupPage.AccessControl.BusinessRules && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_BusinessRules) || setupPage.Access == SetupPage.AccessControl.LoanFolderBusinessRule && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanFolderBusinessRule) || setupPage.Access == SetupPage.AccessControl.MilestoneCompletion && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_MilestoneCompletion) || setupPage.Access == SetupPage.AccessControl.LoanActionCompletion && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanActionCompletion) || setupPage.Access == SetupPage.AccessControl.FieldDataEntry && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FieldDataEntry) || setupPage.Access == SetupPage.AccessControl.FieldTriggers && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FieldTriggers) || setupPage.Access == SetupPage.AccessControl.AutomatedConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AutomatedConditions) || setupPage.Access == SetupPage.AccessControl.AutomatedEnhancedConditions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AutomatedEnhancedConditions) || setupPage.Access == SetupPage.AccessControl.PersonaAccessToFields && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoFields) || setupPage.Access == SetupPage.AccessControl.PersonaAccessToLoans && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoLoans) || setupPage.Access == SetupPage.AccessControl.PersonaAccessToLoanActions && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonaAccesstoLoanActions) || setupPage.Access == SetupPage.AccessControl.RoleAccessToDocuments && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_RoleAccesstoDocuments) || setupPage.Access == SetupPage.AccessControl.InputFormList && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_InputFormList) || setupPage.Access == SetupPage.AccessControl.LoanFormPrinting && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LoanFormPrinting) || setupPage.Access == SetupPage.AccessControl.PrintAutoSelection && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PrintAutoSelection) || setupPage.Access == SetupPage.AccessControl.AppraisalManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AppraisalOrderManagement) || setupPage.Access == SetupPage.AccessControl.LOCompensationControl && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_LOCompensationRule) || setupPage.Access == SetupPage.AccessControl.DocumentTrackingMgmt && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DocumentTracking) || setupPage.Access == SetupPage.AccessControl.SystemAdministration && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_SysAdmin) || setupPage.Access == SetupPage.AccessControl.AnalysisTools && this.aclMgr.GetUserApplicationRight(AclFeature.ThinThick_AnalysisTool_Access) || setupPage.Access == SetupPage.AccessControl.CurrentLogins && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_CurrentLogins) || setupPage.Access == SetupPage.AccessControl.AllUserInformation && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AllUserInformation) || setupPage.Access == SetupPage.AccessControl.LoanReassignment && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_LoanReassignment) || setupPage.Access == SetupPage.AccessControl.UnlockLoanFile && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_UnlockLoanFiles) || setupPage.Access == SetupPage.AccessControl.UnlockTrade && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_UnlockTrade) || setupPage.Access == SetupPage.AccessControl.SystemAuditTrail && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_SystemAuditTrail) || setupPage.Access == SetupPage.AccessControl.LicenseManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AddServices) || setupPage.Access == SetupPage.AccessControl.EdocumentManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EDocumentManagement) || setupPage.Access == SetupPage.AccessControl.StatusOnline && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_CompanyStatusOnline) || setupPage.Access == SetupPage.AccessControl.Fulfillment && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EDisclosureFulfillment) || setupPage.Access == SetupPage.AccessControl.eClose && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_EClose) || setupPage.Access == SetupPage.AccessControl.ComplianceReview && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Company_ComplianceReviewSetup) || setupPage.Access == SetupPage.AccessControl.TaxReturnService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_Tservice4506) || setupPage.Access == SetupPage.AccessControl.TQLServices && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_TQLServices) || setupPage.Access == SetupPage.AccessControl.AppraisalCenter && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AppraisalService) || setupPage.Access == SetupPage.AccessControl.TitleCenter && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_TitleService) || setupPage.Access == SetupPage.AccessControl.FraudService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FraudService) || setupPage.Access == SetupPage.AccessControl.FannieService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FannieService) || setupPage.Access == SetupPage.AccessControl.FreddieService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FreddieService) || setupPage.Access == SetupPage.AccessControl.MIService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_MIService) || setupPage.Access == SetupPage.AccessControl.FreddieMacCACService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FreddieMacCACService) || setupPage.Access == SetupPage.AccessControl.FreddieMacLPAService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FreddieMacCACService) || setupPage.Access == SetupPage.AccessControl.FannieMaeUCDTransferService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FannieMaeUCDTransferService) || setupPage.Access == SetupPage.AccessControl.GinnieMaePddService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_GinnieMaePdd) || setupPage.Access == SetupPage.AccessControl.AvmService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ValuationService) || setupPage.Access == SetupPage.AccessControl.FloodService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FloodService) || setupPage.Access == SetupPage.AccessControl.DataTracService && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DataTracConnection) || setupPage.Access == SetupPage.AccessControl.InsightsSetup && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_InsightsSetup) || setupPage.Access == SetupPage.AccessControl.PersonalSettings && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_PersonalTemplates) || setupPage.Access == SetupPage.AccessControl.AutoLock && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_AutoLock) || setupPage.Access == SetupPage.AccessControl.DynamicDataManagement && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DynamicDataManagement) || setupPage.Access == SetupPage.AccessControl.FeeRules && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FeeRules) || setupPage.Access == SetupPage.AccessControl.FieldRules && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FieldRules) || setupPage.Access == SetupPage.AccessControl.DataTables && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DataTables) || setupPage.Access == SetupPage.AccessControl.FeeGroups && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_FeeGroups) || setupPage.Access == SetupPage.AccessControl.DataPopulationTiming && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_DataPopulationTiming) || setupPage.Access == SetupPage.AccessControl.EncompassAIQ && this.aclMgr.GetUserApplicationRight(AclFeature.SettingsTab_ElliemaeAIQ);
    }

    private void buildSubTree(SetupPage parent, SetupPage[] children)
    {
      Hashtable publicAccess = (Hashtable) null;
      if (parent.Title == "Loan Templates")
        publicAccess = Session.AclGroupManager.CheckPublicAccessPermissions(FeatureSets.FileTypes);
      TreeNode node = new TreeNode(parent.Title);
      node.Tag = (object) parent;
      foreach (SetupPage child in children)
      {
        if (this.isAccessible(child, publicAccess))
          node.Nodes.Add(new TreeNode(child.Title)
          {
            Tag = (object) child
          });
      }
      if (node.Nodes.Count <= 0)
        return;
      this.treeView.Nodes.Add(node);
    }

    private void buildSettingsTree()
    {
      foreach (List<string> settingsList in SetupPage.SettingsLists)
      {
        List<SetupPage> setupPageList = new List<SetupPage>();
        SetupPage parent = (SetupPage) null;
        foreach (string pageTitle in settingsList)
        {
          SetupPage setupPage = SetupPage.GetSetupPage(pageTitle);
          if (setupPage.IsFirstLevelPage)
            parent = setupPage;
          else
            setupPageList.Add(setupPage);
        }
        this.buildSubTree(parent, setupPageList.ToArray());
      }
    }

    private void selectFirstPage()
    {
      this.treeView.SelectedNode = this.treeView.Nodes[0];
      this.treeView.CollapseAll();
    }

    public void ShowPage(string nodeText)
    {
      foreach (TreeNode node1 in this.treeView.Nodes)
      {
        TreeNode node2;
        if ((node2 = this.FindNode(node1, nodeText)) != null)
        {
          this.treeView.SelectedNode = node2;
          break;
        }
      }
      this.Focus();
    }

    public void ShowOrgUserSetupPage(string userid)
    {
      TreeNode treeNode = (TreeNode) null;
      foreach (TreeNode node in this.treeView.Nodes)
      {
        if ((treeNode = this.FindNode(node, "Organization/Users")) != null)
          break;
      }
      if (treeNode == null)
        return;
      this.treeView.AfterSelect -= this.treeViewEventHandler;
      this.treeView.SelectedNode = treeNode;
      this.treeView.AfterSelect += this.treeViewEventHandler;
      SetupPage tag = (SetupPage) treeNode.Tag;
      this.orgHierarchyForm = new OrgHierarchyForm(Session.DefaultInstance, userid);
      this.showDialog(tag, (Control) this.orgHierarchyForm);
    }

    public void ShowExternalUserSetupPage(string userid)
    {
      TreeNode treeNode = (TreeNode) null;
      foreach (TreeNode node in this.treeView.Nodes)
      {
        if ((treeNode = this.FindNode(node, "Company Details")) != null)
          break;
      }
      if (treeNode == null)
        return;
      this.treeView.AfterSelect -= this.treeViewEventHandler;
      this.treeView.SelectedNode = treeNode;
      this.treeView.AfterSelect += this.treeViewEventHandler;
      this.showDialog((SetupPage) treeNode.Tag, (Control) new CompanySetupForm(Session.DefaultInstance, userid));
    }

    public string GetHelpTargetName()
    {
      TreeNode selectedNode = this.treeView.SelectedNode;
      return selectedNode != null && (selectedNode.Nodes.Count == 0 || selectedNode.Text == "Log Setup") ? "Setup\\" + selectedNode.Text : "Setup\\Main";
    }

    public TreeNode GetRootNode(string rootNodeText)
    {
      foreach (TreeNode node in this.treeView.Nodes)
      {
        if (node.Text == rootNodeText)
          return node;
      }
      return (TreeNode) null;
    }

    private TreeNode findNode(string text)
    {
      TreeNode node1 = (TreeNode) null;
      foreach (TreeNode node2 in this.treeView.Nodes)
      {
        node1 = this.FindNode(node2, text);
        if (node1 != null)
          break;
      }
      return node1;
    }

    public TreeNode FindNode(TreeNode root, string text)
    {
      TreeNode node1 = (TreeNode) null;
      if (root.Text == text)
      {
        node1 = root;
      }
      else
      {
        foreach (TreeNode node2 in root.Nodes)
        {
          node1 = this.FindNode(node2, text);
          if (node1 != null)
            break;
        }
      }
      return node1;
    }

    public void SetMenu(ToolStripItemCollection items)
    {
      foreach (ToolStripMenuItem toolStripMenuItem in (ArrangedElementCollection) items)
      {
        string str = toolStripMenuItem.Text.Trim('.').Replace("&", string.Empty);
        foreach (TreeNode node in this.treeView.Nodes)
        {
          if (node.Text == str)
          {
            toolStripMenuItem.Visible = true;
            this.enableMenus(toolStripMenuItem, node);
            break;
          }
          toolStripMenuItem.Visible = false;
        }
      }
    }

    public void Menu_Click(string item)
    {
      TreeNode clickedNode = this.getClickedNode(this.treeView.Nodes, item);
      if (clickedNode == null)
        return;
      this.treeView.SelectedNode = clickedNode;
    }

    public void ShowSelectedLink(TreeNode selectedNode)
    {
      if (selectedNode == null)
        return;
      this.treeView.SuspendLayout();
      this.treeView.CollapseAll();
      this.treeView.ResumeLayout(true);
      this.treeView.SelectedNode = (TreeNode) selectedNode.Tag;
    }

    private TreeNode getClickedNode(TreeNodeCollection nodes, string item)
    {
      TreeNode clickedNode = (TreeNode) null;
      foreach (TreeNode node in nodes)
      {
        if (node.Text == item)
        {
          clickedNode = node;
          break;
        }
        if (node.Nodes.Count != 0)
        {
          if ((clickedNode = this.getClickedNode(node.Nodes, item)) != null)
            break;
        }
      }
      return clickedNode;
    }

    private void enableMenus(ToolStripMenuItem item, TreeNode node)
    {
      if (item.DropDownItems.Count > 0)
      {
        foreach (ToolStripMenuItem dropDownItem in (ArrangedElementCollection) item.DropDownItems)
        {
          bool flag = false;
          string str = dropDownItem.Text.Trim('.').Replace("&", string.Empty);
          foreach (TreeNode node1 in node.Nodes)
          {
            if (node1.Text == str)
            {
              flag = true;
              if (dropDownItem.DropDownItems.Count != 0)
              {
                this.enableMenus(dropDownItem, node1);
                break;
              }
              break;
            }
          }
          dropDownItem.Visible = flag;
        }
      }
      else
      {
        if (!(item.Text.Trim('.').Replace("&", string.Empty) != node.Text))
          return;
        item.Visible = false;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private DialogResult promptForSave()
    {
      DialogResult dialogResult = DialogResult.No;
      SettingsUserControl currentContent = this.setupDialog.CurrentContent as SettingsUserControl;
      if (currentContent is LockComparisonToolSetup)
        return currentContent.PromptForSave();
      if (currentContent != null && currentContent.IsDirty && currentContent != null && currentContent.IsDirty)
      {
        switch (currentContent)
        {
          case ConfigurableWorkFlow _:
            dialogResult = Utils.Dialog((IWin32Window) this.TopLevelControl, "Changes have been made to the Configurable Workflow Template settings. If you wish to Save those changes click yes and then click the Save button. Otherwise click No.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.No ? DialogResult.Cancel : DialogResult.OK;
            break;
          case NormalizedBidTapeTemplate _:
            dialogResult = Utils.Dialog((IWin32Window) this.TopLevelControl, "Changes have been made to the Normalized Bid Tape Template settings. If you wish to Save those changes click Yes and then click the Save button. Otherwise click No.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No ? DialogResult.Cancel : DialogResult.OK;
            break;
          case LockComparisonToolSetup _:
            dialogResult = Utils.Dialog((IWin32Window) this.TopLevelControl, "Changes have been made to the Lock Comparison Tool Fields settings. If you wish to Save those changes click Yes and then click the Save button. Otherwise click No.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.No ? DialogResult.Cancel : DialogResult.OK;
            break;
          case NormalizedBidTapeTemplate2 _:
            dialogResult = Utils.Dialog((IWin32Window) this.TopLevelControl, "Changes have been made to the Normalized Bid Tape Template settings. If you wish to Save those changes click Yes and then click the Save button. Otherwise click No.", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) != DialogResult.No ? DialogResult.Cancel : DialogResult.OK;
            break;
          default:
            dialogResult = Utils.Dialog((IWin32Window) this.TopLevelControl, "Do you want to save the changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            switch (dialogResult)
            {
              case DialogResult.Yes:
                bool flag = currentContent.SaveRtnBool();
                if (currentContent is PersonalSettingsForm && currentContent != null && !((PersonalSettingsForm) currentContent).ChangesSaved || currentContent is DocumentTrackingSettings && currentContent != null && !((DocumentTrackingSettings) currentContent).ChangesSaved || !flag)
                {
                  dialogResult = DialogResult.Cancel;
                  break;
                }
                break;
              case DialogResult.No:
                currentContent.Reset();
                break;
            }
            break;
        }
      }
      return dialogResult;
    }

    private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
    {
      if (this.suspendBeforeSelect)
      {
        e.Cancel = true;
      }
      else
      {
        SettingsUserControl currentContent = this.setupDialog.CurrentContent as SettingsUserControl;
        if (currentContent is LockComparisonToolSetup)
        {
          currentContent.Node = e.Node;
          currentContent.TreeView = this.treeView;
        }
        SetupPage tag = (SetupPage) e.Node.Tag;
        if (tag.IsFirstLevelPage && tag.Title != "Settings Overview")
        {
          if (e.Node.IsExpanded)
            e.Node.Collapse();
          else
            e.Node.Expand();
          e.Cancel = true;
        }
        else
        {
          if (this.promptForSave() != DialogResult.Cancel)
            return;
          e.Cancel = true;
        }
      }
    }

    private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
      TreeNode node = e.Node;
      string str1 = node.Text;
      SetupPage tag = (SetupPage) node.Tag;
      if (!this.isAccessible(SetupPage.GetSetupPage(str1), (Hashtable) null))
      {
        string str2 = "\"" + str1 + "\" is not accessible by user " + Session.UserID;
        Tracing.Log(SetUpContainer.sw, TraceLevel.Warning, nameof (SetUpContainer), str2);
        int num = (int) MessageBox.Show((IWin32Window) this, str2);
      }
      else
      {
        switch (str1)
        {
          case "2009 GFE Print":
            this.showDialog(tag, (Control) new GFESettings(this));
            break;
          case "4506C Service":
            Control dialog1 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetTaxReturnSettingsControl();
            if (dialog1 != null)
            {
              this.showDialog(tag, dialog1);
              break;
            }
            break;
          case "Additional Services":
          case "Business Rules":
          case "Company/User Setup":
          case "Contact Setup":
          case "Dynamic Data Management":
          case "External Company Setup":
          case "Investor Connect Setup":
          case "Loan Setup":
          case "Loan Templates":
          case "Personal Settings":
          case "Secondary Setup":
          case "System Administration":
          case "Tables and Fees":
          case "eFolder Setup":
            str1 = (string) null;
            break;
          case "Adjustment Templates":
            this.showDialog(tag, (Control) new PriceAdjustmentTemplateExplorer());
            break;
          case "Affiliated Business Arrangement Templates":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements, Session.DefaultInstance));
            break;
          case "Affordable Lending":
            this.showDialog(tag, (Control) new AreaMedianIncomeLimitsControl(this, Session.DefaultInstance));
            break;
          case "Alerts":
            this.showDialog(tag, (Control) new Alerts(this));
            break;
          case "All TPO Contact Information":
            if (this.tpoPanel == null)
              this.tpoPanel = new AllTPOUserPanel(Session.DefaultInstance, this);
            else
              this.tpoPanel.Refresh();
            this.showDialog(tag, (Control) this.tpoPanel);
            break;
          case "All User Information":
            this.showDialog(tag, (Control) new AllUserPanel(this));
            break;
          case "Analysis Tools":
            if (this.analysisTools != null)
              this.analysisTools.Dispose();
            this.analysisTools = new AnalysisTools();
            this.showDialog(tag, (Control) this.analysisTools);
            break;
          case "Appraisal Order Management":
            this.showDialog(tag, (Control) new AppraisalManagementControl());
            break;
          case "Appraisal Service":
            Control dialog2 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetPaymentSettingsControl("Appraisal");
            if (dialog2 != null)
            {
              this.showDialog(tag, dialog2);
              break;
            }
            break;
          case "Auto Loan Numbering":
            this.showDialog(tag, (Control) new AutoLoanNoPanel(this));
            break;
          case "Auto MERS MIN Numbering":
            this.showDialog(tag, (Control) new AutoMERSPanel(this));
            break;
          case "Auto-Lock":
            this.showDialog(tag, (Control) new AutoLockSetup(this, Session.DefaultInstance));
            break;
          case "Automated Conditions":
            this.showDialog(tag, (Control) new AutomatedConditionsListPanel(Session.DefaultInstance, false));
            break;
          case "Automated Enhanced Conditions":
            this.showDialog(tag, (Control) new AutomatedEnhancedConditionsListPanel(Session.DefaultInstance, false));
            break;
          case "Autosave Configuration":
            this.showDialog(tag, (Control) new AutosaveSettingsPage(this));
            break;
          case "Borrower Contact Status":
            this.showDialog(tag, (Control) new BorrowerStatusSetupForm(this));
            break;
          case "Borrower Contact Update":
            this.showDialog(tag, (Control) new BorrowerContactUpdateDialog(this));
            break;
          case "Borrower Custom Fields":
            this.showDialog(tag, (Control) new ContactCustomFieldsForm(this, EllieMae.EMLite.ContactUI.ContactType.Borrower));
            break;
          case "Business Categories":
            this.showDialog(tag, (Control) new BizCategorySetupForm(this));
            break;
          case "Business Custom Fields":
            this.showDialog(tag, (Control) new ContactCustomFieldsForm(this, EllieMae.EMLite.ContactUI.ContactType.BizPartner));
            break;
          case "Changed Circumstances Setup":
            this.showDialog(tag, (Control) new ChangeCircumstancesSetupForm(this));
            break;
          case "Channel Options":
            this.showDialog(tag, (Control) new ChannelOptionForm(this));
            break;
          case "City Tax":
            this.showDialog(tag, (Control) new FeePanel(FeePanel.FeeType.City));
            break;
          case "Closing Costs":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.ClosingCost, Session.DefaultInstance));
            break;
          case "Closing Doc Plan Codes":
            this.showDialog(tag, (Control) new PlanCodeManagementControl(this, Session.DefaultInstance, DocumentOrderType.Closing));
            break;
          case "Closing Doc Stacking Templates":
            this.showDialog(tag, (Control) new StackingOrderMgmtControl(this, Session.DefaultInstance, DocumentOrderType.Closing));
            break;
          case "Collateral Tracking":
            this.showDialog(tag, (Control) new DocumentTrackingSettings(this));
            break;
          case "Company Details":
            this.showDialog(tag, (Control) new CompanySetupForm(Session.DefaultInstance));
            break;
          case "Company Information":
            this.showDialog(tag, (Control) new CompanyInfoPanelEx(this));
            break;
          case "Company Status Online":
            this.showDialog(tag, (Control) new FeaturePanel(this, EncompassModule.StatusOnline));
            break;
          case "Compliance Audit Settings":
            this.showDialog(tag, (Control) new ComplianceAuditSettingControl(this));
            break;
          case "Compliance Calendar":
            this.showDialog(tag, (Control) new BusinessCalendarPanel(this));
            break;
          case "Compliance Review Setup":
            Control dialog3 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetComplianceSettingsControl();
            if (dialog3 != null)
            {
              this.showDialog(tag, dialog3);
              break;
            }
            break;
          case "Condition Forms":
            Cursor.Current = Cursors.WaitCursor;
            this.showDialog(tag, (Control) new ConditionalLetterExplorer(Session.DefaultInstance));
            break;
          case "Condition Sets":
            this.showDialog(tag, (Control) new ConditionSetsSetupControl(Session.DefaultInstance, ConditionType.Underwriting));
            break;
          case "Conditions":
            this.showDialog(tag, (Control) new ConditionTrackingSetupControl(Session.DefaultInstance, ConditionType.Underwriting));
            break;
          case "Configurable Workflow Templates":
            this.showDialog(tag, (Control) new ConfigurableWorkFlow(this));
            break;
          case "Conventional County Limits":
            this.showDialog(tag, (Control) new ConventionalCountyLimitControl(this, Session.DefaultInstance));
            break;
          case "Correspondent Purchase Advice":
            this.showDialog(tag, (Control) new CorrespondentPurchaseAdviceSetup(this));
            break;
          case "Current Logins":
            this.showDialog(tag, (Control) new CurrentLoginsUserControl(this));
            break;
          case "Custom Print Forms":
            Cursor.Current = Cursors.WaitCursor;
            this.showDialog(tag, (Control) new CustomLetterPanel(Session.DefaultInstance, this.mainScreen));
            break;
          case "Data & Document Automation and Mortgage Analyzers":
            Control dialog4 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : (!this.session.StartupInfo.HasAIQLicense ? (Control) new EncompassAIQDemoControl(this, Session.DefaultInstance) : (Control) new EncompassAIQSetupControl(this, Session.DefaultInstance));
            this.showDialog(tag, dialog4);
            break;
          case "Data Tables":
            this.showDialog(tag, (Control) new DataTableForm(this, Session.DefaultInstance));
            break;
          case "Data Templates":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData, Session.DefaultInstance));
            break;
          case "DataTrac Connection":
            Control dialog5 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetDataTracSettingsControl();
            if (dialog5 != null)
            {
              this.showDialog(tag, dialog5);
              break;
            }
            break;
          case "Default File Contacts":
            Cursor.Current = Cursors.WaitCursor;
            DefaultTemplateControl dialog6 = (DefaultTemplateControl) new DefaultProvidersTemplateControl(this);
            dialog6.Dock = DockStyle.Fill;
            this.pnlRight.AutoScroll = true;
            this.showDialog(tag, (Control) dialog6);
            break;
          case "Default Input Forms":
            this.showDialog(tag, (Control) new InputFormOrderPanel(this));
            break;
          case "Default Template Setting":
            this.showDialog(tag, (Control) new TemplateAppendValueConfig(this));
            break;
          case "Deliver Loans":
          case "Partner Setup":
            if (this.investorConnectSettingsPanel == null)
              this.investorConnectSettingsPanel = new InvestorConnectSettings(Session.DefaultInstance, str1);
            else if (string.Equals(this.currentPage, str1))
              this.investorConnectSettingsPanel.investorForm.ReloadModule();
            else
              this.investorConnectSettingsPanel = new InvestorConnectSettings(Session.DefaultInstance, str1);
            this.currentPage = str1;
            this.showDialog(tag, (Control) this.investorConnectSettingsPanel);
            break;
          case "Disclosure Tracking Settings":
            this.showDialog(tag, (Control) new DisclosureTrackingSetting(this));
            break;
          case "Document Conversion":
            this.showDialog(tag, (Control) new DocumentConversionSetupControl(Session.DefaultInstance, this));
            break;
          case "Document Export Templates":
            this.showDialog(tag, (Control) new ExportTemplateSetupControl(this, Session.DefaultInstance));
            break;
          case "Document Groups":
            this.showDialog(tag, (Control) new DocumentGroupSetupControl(this));
            break;
          case "Document Identification":
            this.showDialog(tag, (Control) new BarcodeSetupControl(this));
            break;
          case "Document Sets":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.DocumentSet, Session.DefaultInstance));
            break;
          case "Document Stacking Templates":
            this.showDialog(tag, (Control) new StackingTemplateSetupControl(this, Session.DefaultInstance));
            break;
          case "Document Training":
            this.showDialog(tag, (Control) new DocumentTrainingSetupControl(Session.DefaultInstance, this));
            break;
          case "Documents":
            this.showDialog(tag, (Control) new DocumentTrackingSetupControl(Session.DefaultInstance, this));
            break;
          case "E-Document Management":
            this.showDialog(tag, (Control) new FeaturePanel(this, EncompassModule.EDM));
            break;
          case "Email Server Settings":
            this.showDialog(tag, (Control) new EmailSettingsPanel(this));
            break;
          case "Encompass eClose Setup":
            Control dialog7 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : (Control) new eCloseSetupControl(Session.DefaultInstance);
            this.showDialog(tag, dialog7);
            break;
          case "Enhanced Condition Sets":
            this.showDialog(tag, (Control) new EnhancedConditionSetsControl(Session.DefaultInstance));
            break;
          case "Enhanced Conditions":
            this.showDialog(tag, (Control) new EnhancedConditionsSetupControl(Session.DefaultInstance));
            break;
          case "Escrow":
            this.showDialog(tag, (Control) new TablePanel(TablePanel.TableID.Escrow));
            break;
          case "FHA County Limits":
            this.showDialog(tag, (Control) new CountyLimitControl(this, Session.DefaultInstance));
            break;
          case "FHA Informed Consumer Choice Disclosure":
            Cursor.Current = Cursors.WaitCursor;
            DefaultTemplateControl dialog8 = (DefaultTemplateControl) new DefaultFHAConsumerChoiceTemplateControl(this);
            dialog8.Dock = DockStyle.Fill;
            this.pnlRight.AutoScroll = true;
            this.showDialog(tag, (Control) dialog8);
            break;
          case "Fannie Mae UCD Transfer":
            Control dialog9 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFannieMaeUCDTransferServiceSettingsControl();
            if (dialog9 != null)
            {
              this.showDialog(tag, dialog9);
              break;
            }
            break;
          case "Fannie Mae Workflow":
            Control dialog10 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFannieSettingsControl();
            if (dialog10 != null)
            {
              this.showDialog(tag, dialog10);
              break;
            }
            break;
          case "Federal Threshold Adjustments":
            this.showDialog(tag, (Control) new FedTresholdAdjustmentControl(this, Session.DefaultInstance));
            break;
          case "Fee Groups":
            this.showDialog(tag, (Control) new FeeGroupsForm());
            break;
          case "Fee Rules":
            this.showDialog(tag, (Control) new FeeRulesMain(this, Session.DefaultInstance));
            break;
          case "Field Data Entry":
            this.showDialog(tag, (Control) new FieldRulePanel(Session.DefaultInstance, false));
            break;
          case "Field Rules":
            this.showDialog(tag, (Control) new FieldRulesForm(this, Session.DefaultInstance));
            break;
          case "Field Triggers":
            this.showDialog(tag, (Control) new TriggerListPanel(Session.DefaultInstance, false));
            break;
          case "Flood Service":
            Control dialog11 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFloodSettingsControl();
            if (dialog11 != null)
            {
              this.showDialog(tag, dialog11);
              break;
            }
            break;
          case "Fraud Service":
            Control dialog12 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFraudSettingsControl();
            if (dialog12 != null)
            {
              this.showDialog(tag, dialog12);
              break;
            }
            break;
          case "Freddie Mac LPA Batch":
            Control dialog13 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFreddieMacLPABatchServiceSettingsControl();
            if (dialog13 != null)
            {
              this.showDialog(tag, dialog13);
              break;
            }
            break;
          case "Freddie Mac Loan Assignment":
            Control dialog14 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFreddieMacCACServiceSettingsControl();
            if (dialog14 != null)
            {
              this.showDialog(tag, dialog14);
              break;
            }
            break;
          case "Freddie Mac Order Alert":
            Control dialog15 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetFreddieSettingsControl();
            if (dialog15 != null)
            {
              this.showDialog(tag, dialog15);
              break;
            }
            break;
          case "Funding Templates":
            this.showDialog(tag, (Control) new FundingTemplateExplorer());
            break;
          case "Ginnie Mae PDD Batch":
            Control dialog16 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetGinnieMaePDDBatchServiceSettingsControl();
            if (dialog16 != null)
            {
              this.showDialog(tag, dialog16);
              break;
            }
            break;
          case "Global DDM Settings":
            this.showDialog(tag, (Control) new DataPopulationTiming(this, Session.DefaultInstance));
            break;
          case "Grant File Access":
            Cursor.Current = Cursors.WaitCursor;
            this.showDialog(tag, (Control) new RightsAssignmentForm(Session.DefaultInstance));
            break;
          case "HELOC Table":
            this.showDialog(tag, (Control) new HelocPanel(Session.DefaultInstance, false));
            break;
          case "HMDA Profiles":
            this.showDialog(tag, (Control) new HMDASettingsForm(Session.DefaultInstance, this));
            break;
          case "HTML Email Templates":
            this.showDialog(tag, (Control) new EDMEmailControl(Session.DefaultInstance, this, false));
            break;
          case "ICE Mortgage Technology Network Company Password":
            this.showDialog(tag, (Control) new CompanyPasswordPanel(this));
            break;
          case "ICE PPE Loan Program Table":
            this.showDialog(tag, (Control) new EPPSProgramTableExplorer(this, Session.DefaultInstance));
            break;
          case "Input Form List":
            this.showDialog(tag, (Control) new InputFormRulePanel(Session.DefaultInstance, false));
            break;
          case "Input Form Sets":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, Session.DefaultInstance));
            break;
          case "Insights Setup":
            this.showDialog(tag, (Control) new InsightsSetupControl(this, this.session));
            break;
          case "Investor Templates":
            this.showDialog(tag, (Control) new InvestorTemplateExplorer());
            break;
          case "Itemization Fee Management":
            this.showDialog(tag, (Control) new FeeManagementControl(this));
            break;
          case "LO Compensation":
            this.showDialog(tag, (Control) new LOCompensationList(this));
            break;
          case "LO Compensation Rule":
            this.showDialog(tag, (Control) new LOCompensationControl(this));
            break;
          case "Loan Action Completion":
            this.showDialog(tag, (Control) new LoanActionCompletionRulePanel(this.session, false));
            break;
          case "Loan Custom Fields":
            this.customFieldsUserControl = new CustomFieldsUserControl(Session.DefaultInstance, this, false);
            this.showDialog(tag, (Control) this.customFieldsUserControl);
            break;
          case "Loan Duplication":
            this.showDialog(tag, (Control) new LoanDuplicationTemplateExplorer(Session.DefaultInstance, this));
            break;
          case "Loan Error Information":
            this.showDialog(tag, (Control) new LoanErrorInformation(this, Session.DefaultInstance));
            break;
          case "Loan Folder Business Rule":
            this.showDialog(tag, (Control) new LoanFolderBizRuleDialog(Session.DefaultInstance, this, false));
            break;
          case "Loan Folders":
            this.showDialog(tag, (Control) new FolderSettingsForm(this));
            break;
          case "Loan Form Printing":
            this.showDialog(tag, (Control) new PrintFormRulePanel(Session.DefaultInstance, false));
            break;
          case "Loan Pricing Decimal Places":
            this.showDialog(tag, (Control) new LoanPricingDecimalPlacesControl(this));
            break;
          case "Loan Program Additional Fields":
            this.showDialog(tag, (Control) new LoanProgramAdditionalFieldsControl(Session.DefaultInstance, this));
            break;
          case "Loan Programs":
            this.showDialog(tag, (Control) new TemplateExplorerControl(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanProgram, Session.DefaultInstance));
            break;
          case "Loan Reassignment":
            this.showDialog(tag, (Control) new BatchLoanReassignmentForm(Session.DefaultInstance, this));
            break;
          case "Loan Template Sets":
            this.showDialog(tag, (Control) new LoanTemplateSettingContainer(Session.DefaultInstance));
            break;
          case "Lock Comparison Tool Fields":
            this.showDialog(tag, (Control) new LockComparisonToolSetup(this));
            break;
          case "Lock Desk Setup":
            this.showDialog(tag, (Control) new LockExpDateSetup(this));
            break;
          case "Lock Request Additional Fields":
            this.showDialog(tag, (Control) new AdditionalFieldListControl(Session.DefaultInstance, this));
            break;
          case "Log":
            this.showDialog(tag, (Control) new LogLockPanel(this, "Conversation Log"));
            break;
          case "MI Tables":
            this.showDialog(tag, (Control) new MITableSetupPanel(Session.DefaultInstance));
            break;
          case "Milestone Completion":
            this.showDialog(tag, (Control) new ConditionRulePanelContainer(this));
            break;
          case "Milestones":
            this.showDialog(tag, (Control) new MilestoneSettingsPanel(Session.DefaultInstance, this, false));
            break;
          case "Mortgage Insurance Service":
            Control dialog17 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetMIServiceSettingsControl();
            if (dialog17 != null)
            {
              this.showDialog(tag, dialog17);
              break;
            }
            break;
          case "My Email Server Settings":
            this.showDialog(tag, (Control) new PersonalEmailSettingsPanel(this));
            break;
          case "My Profile":
            this.showDialog(tag, (Control) new PersonalSettingsForm(this));
            break;
          case "NMLS Report Setup":
            this.showDialog(tag, (Control) new NMLSReportSettingsControl(Session.DefaultInstance, this));
            break;
          case "Normalized Bid Tape Template":
            this.showDialog(tag, (Control) new NormalizedBidTapeTemplate2(this));
            break;
          case "Notification Templates":
            this.showDialog(tag, (Control) new NotificationTemplatesSetupControl());
            break;
          case "Organization/Users":
            if (this.orgHierarchyForm == null)
              this.orgHierarchyForm = new OrgHierarchyForm(Session.DefaultInstance);
            this.showDialog(tag, (Control) this.orgHierarchyForm);
            break;
          case "Persona Access to Fields":
            this.showDialog(tag, (Control) new FieldAccessRulePanel(Session.DefaultInstance, false));
            break;
          case "Persona Access to Loan Actions":
            this.showDialog(tag, (Control) new LoanActionsAccessRulePanel(Session.DefaultInstance, false));
            break;
          case "Persona Access to Loans":
            if (Session.EncompassEdition == EncompassEdition.Broker)
            {
              this.showDialog(tag, (Control) new BrokerLoanAccessRulePanel(this));
              break;
            }
            this.showDialog(tag, (Control) new LoanAccessRulePanel(Session.DefaultInstance, false));
            break;
          case "Personal Status Online":
            this.showDialog(tag, (Control) new StatusOnlineSetupPanel(Session.DefaultInstance, this, Session.UserID));
            break;
          case "Personas":
            this.showDialog(tag, (Control) new PersonaSetupForm(this));
            break;
          case "Piggyback Loan Synchronization":
            this.showDialog(tag, (Control) new PiggybackSetupPanel(Session.DefaultInstance, this));
            break;
          case "Pipeline Refresh":
            this.showDialog(tag, (Control) new PipelineSettingsPage(this));
            break;
          case "Post-Closing Condition Sets":
            this.showDialog(tag, (Control) new ConditionSetsSetupControl(Session.DefaultInstance, ConditionType.PostClosing));
            break;
          case "Post-Closing Conditions":
            this.showDialog(tag, (Control) new ConditionTrackingSetupControl(Session.DefaultInstance, ConditionType.PostClosing));
            break;
          case "Print Auto Selection":
            this.showDialog(tag, (Control) new PrintSelectionRuleListPanel(Session.DefaultInstance, false));
            break;
          case "Print Form Groups":
            Cursor.Current = Cursors.WaitCursor;
            this.showDialog(tag, (Control) new PrintFormGroupPanel(Session.DefaultInstance, this));
            break;
          case "Privacy Policy":
            Cursor.Current = Cursors.WaitCursor;
            DefaultTemplateControl dialog18 = (DefaultTemplateControl) new DefaultPrivacyPolicyTemplateControl(this);
            dialog18.Dock = DockStyle.Fill;
            this.pnlRight.AutoScroll = true;
            this.showDialog(tag, (Control) dialog18);
            break;
          case "Product and Pricing":
            this.showDialog(tag, (Control) new ProductPricingAdmin(this, Session.DefaultInstance));
            break;
          case "Public Business Contact Groups":
            this.showDialog(tag, (Control) new PublicBizContactGroup());
            break;
          case "Purchase Advice Form":
            this.showDialog(tag, (Control) new PurchaseAdviceFormSetup(this));
            break;
          case "Purchase Condition Sets":
            this.showDialog(tag, (Control) new PurchaseConditionSetsDialog());
            break;
          case "Purchase Conditions":
            this.showDialog(tag, (Control) new PurchaseConditions());
            break;
          case "RESPA":
            Cursor.Current = Cursors.WaitCursor;
            DefaultTemplateControl dialog19 = (DefaultTemplateControl) new DefaultRespaTemplateControl(this);
            dialog19.Dock = DockStyle.Fill;
            this.pnlRight.AutoScroll = true;
            this.showDialog(tag, (Control) dialog19);
            break;
          case "Rebuild Field Search Data":
            this.showDialog(tag, (Control) new FieldSearchControl());
            break;
          case "Rebuild Pipeline":
            this.showDialog(tag, (Control) new RebuildUserControl(this));
            break;
          case "Restore Warning Messages":
            this.showDialog(tag, (Control) new RestoreWarningMessagesControl(Session.DefaultInstance, this));
            break;
          case "Role Access to Documents":
            this.showDialog(tag, (Control) new DocumentAccessPanel(Session.DefaultInstance, this, false));
            break;
          case "Roles":
            this.showDialog(tag, (Control) new RoleFunctionPanel(this));
            break;
          case "SRP Templates":
            this.showDialog(tag, (Control) new SRPTemplateExplorer());
            break;
          case "SSN Verification Service":
            Control dialog20 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetSSNSettingsControl();
            if (dialog20 != null)
            {
              this.showDialog(tag, dialog20);
              break;
            }
            break;
          case "Secondary Lock Fields":
            this.showDialog(tag, (Control) new SecondaryFieldsSetup(this));
            break;
          case "Services Password Management":
            this.showDialog(tag, (Control) new ePassPasswordMgr());
            break;
          case "Servicing":
            this.showDialog(tag, (Control) new ServicingLateChargeSetup(this));
            break;
          case "Settings Overview":
            Cursor.Current = Cursors.WaitCursor;
            SettingsOverview dialog21 = new SettingsOverview(this.treeView, this);
            dialog21.Dock = DockStyle.Fill;
            this.showDialog(tag, (Control) dialog21);
            str1 = (string) null;
            break;
          case "Settings Reports":
            this.showDialog(tag, (Control) new SettingsRptJobQueue(this, Session.DefaultInstance));
            break;
          case "Settlement Service Providers":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders, Session.DefaultInstance));
            break;
          case "Special Feature Codes":
            this.showDialog(tag, (Control) new SpecialFeatureCodesSetup(Session.DefaultInstance));
            break;
          case "State Tax":
            this.showDialog(tag, (Control) new FeePanel(FeePanel.FeeType.State));
            break;
          case "Sync Templates":
            this.showDialog(tag, (Control) new SyncTemplateSetupPanel(Session.DefaultInstance));
            break;
          case "System Audit Trail":
            this.showDialog(tag, (Control) new SystemAuditTrail(Session.DefaultInstance));
            break;
          case "TPO Connect Site Management":
            this.showDialog(tag, (Control) new TPOConnectSiteMngmnt(this, Session.DefaultInstance));
            break;
          case "TPO Custom Fields":
            this.showDialog(tag, (Control) new TPOCustomFieldsForm(Session.DefaultInstance, this, EllieMae.EMLite.ContactUI.ContactType.TPO));
            break;
          case "TPO Disclosure Settings":
            this.showDialog(tag, (Control) new TPODisclosureSetting(this));
            break;
          case "TPO Docs":
            this.showDialog(tag, (Control) new TPODocsForm(Session.DefaultInstance, (SetUpContainer) null));
            break;
          case "TPO Fees":
            this.showDialog(tag, (Control) new TPOFees(Session.DefaultInstance));
            break;
          case "TPO Global Lender Contacts":
            this.showDialog(tag, (Control) new TPOGlobalLenderContacts(Session.DefaultInstance, (SetUpContainer) null));
            break;
          case "TPO Reassignment":
            this.showDialog(tag, (Control) new TPOReassignment(Session.DefaultInstance, (SetUpContainer) null));
            break;
          case "TPO Settings":
            this.showDialog(tag, (Control) new CompanySettingForm(Session.DefaultInstance));
            break;
          case "TPO WebCenter Site Management":
            this.showDialog(tag, (Control) new TPOWCSiteMngmnt(this, Session.DefaultInstance));
            break;
          case "TQL Services":
            Control dialog22 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetTQLSettingsControl();
            if (dialog22 != null)
            {
              this.showDialog(tag, dialog22);
              break;
            }
            break;
          case "Task Sets":
            this.showDialog(tag, (Control) new TemplatePanelExplorer(EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet, Session.DefaultInstance));
            break;
          case "Tasks":
            this.showDialog(tag, (Control) new MilestoneTaskListPanel(Session.DefaultInstance, false));
            break;
          case "Temporary Buydown":
            this.showDialog(tag, (Control) new TemporaryBuydownPanel());
            break;
          case "Title":
            this.showDialog(tag, (Control) new TablePanel(TablePanel.TableID.Title));
            break;
          case "Title Service":
            Control dialog23 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetPaymentSettingsControl("Title");
            if (dialog23 != null)
            {
              this.showDialog(tag, dialog23);
              break;
            }
            break;
          case "Trade Management Setup":
            this.showDialog(tag, (Control) new TradeManagementFieldsSetup(this));
            break;
          case "Transcript Request Templates":
            this.showDialog(tag, (Control) new IRS4506TSettingsForm(Session.DefaultInstance, this));
            break;
          case "Trustee List":
            this.showDialog(tag, (Control) new TrusteeDatabaseSettingForm(this));
            break;
          case "Unlock Loan File":
            this.showDialog(tag, (Control) new UnlockLoansForm(this));
            break;
          case "Unlock Trade":
            this.showDialog(tag, (Control) new UnlockTrade(this));
            break;
          case "User Defined Fee":
            this.showDialog(tag, (Control) new FeePanel(FeePanel.FeeType.UserDefined));
            break;
          case "User Groups":
            this.showDialog(tag, (Control) new SecurityGroupSetupForm(this));
            break;
          case "Valuation Service":
            Control dialog24 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : LoanServiceManager.GetAVMSettingsControl();
            if (dialog24 != null)
            {
              this.showDialog(tag, dialog24);
              break;
            }
            break;
          case "Verification Contact Setup":
            this.showDialog(tag, (Control) new VerifContactSettings(Session.DefaultInstance, this));
            break;
          case "WebCenter Configuration":
            this.showDialog(tag, (Control) new WebCenterConfigurationControl(Session.DefaultInstance, this));
            break;
          case "Zipcode Setup":
            this.showDialog(tag, (Control) new ZipcodeSetupControl());
            break;
          case "e360Select Setup":
            Control dialog25 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : (Control) new LoanScreeningSetupControl(this);
            if (dialog25 != null)
            {
              this.showDialog(tag, dialog25);
              break;
            }
            break;
          case "eDisclosure Fulfillment":
            Control dialog26 = !EpassLogin.LoginRequired(true) ? (Control) new AccessDeniedPanel() : (Control) new FulfillmentSetupControl();
            this.showDialog(tag, dialog26);
            break;
          case "eDisclosure Packages":
            this.showDialog(tag, (Control) new EDisclosureSetupControl(this));
            if (this.Height < 700)
            {
              this.Height = Convert.ToInt32((double) this.Height * 1.15);
              break;
            }
            break;
          case "eDisclosure Plan Codes":
            this.showDialog(tag, (Control) new PlanCodeManagementControl(this, Session.DefaultInstance, DocumentOrderType.Opening));
            break;
          case "eDisclosure Stacking Templates":
            this.showDialog(tag, (Control) new StackingOrderMgmtControl(this, Session.DefaultInstance, DocumentOrderType.Opening));
            break;
          default:
            Session.User.AddRemoveAndGetRecentSettings(true, str1, false, 0, false);
            str1 = (string) null;
            break;
        }
        this.addAndRefreshRecent(str1);
      }
    }

    public void TransferToAutoLoanNumbering()
    {
      foreach (TreeNode node1 in this.treeView.Nodes)
      {
        if (node1.Text == "Loan Setup")
        {
          foreach (TreeNode node2 in node1.Nodes)
          {
            if (node2.Text == "Auto Loan Numbering")
            {
              this.treeView.SelectedNode = node2;
              return;
            }
          }
        }
      }
    }

    public void TransferToEDisclosurePackages()
    {
      foreach (TreeNode node1 in this.treeView.Nodes)
      {
        if (node1.Text == "Docs Setup")
        {
          foreach (TreeNode node2 in node1.Nodes)
          {
            if (node2.Text == "eDisclosure Packages")
            {
              this.treeView.SelectedNode = node2;
              return;
            }
          }
        }
      }
    }

    public void UpdatesetupDialogSubTitle(string subTitle)
    {
      this.setupDialog.SetSubTitle(subTitle);
    }

    private void showDialog(SetupPage setupPage, Control dialog)
    {
      this.setupDialog.SubTitleBottomBorder = setupPage.Title == "Settings Overview";
      if (setupPage.Title == "TPO Fees" || setupPage.Title == "TPO Global Lender Contacts")
        this.setupDialog.AlignSubTitle(0, 0);
      else
        this.setupDialog.AlignSubTitle(10, 9);
      this.setupDialog.ChangeContent(setupPage.Title, setupPage.SubTitle, setupPage.ContainerSaveResetBtns, dialog);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SetUpContainer));
      this.treeView = new TreeView();
      this.contextMenu1 = new ContextMenu();
      this.miExpand = new MenuItem();
      this.miCollapse = new MenuItem();
      this.gradientMenuStrip1 = new GradientMenuStrip();
      this.tsMenuItemEncompassSettings = new ToolStripMenuItem();
      this.tsMenuItemClose = new ToolStripMenuItem();
      this.tsMenuItemGoToRecent = new ToolStripMenuItem();
      this.tsMenuItemImport = new ToolStripMenuItem();
      this.tsMenuItemPointLP = new ToolStripMenuItem();
      this.tsMenuItemPointCC = new ToolStripMenuItem();
      this.tsMenuItemPointSettings = new ToolStripMenuItem();
      this.tsMenuItemCustomForms = new ToolStripMenuItem();
      this.pnlContent = new Panel();
      this.pnlRight = new PanelEx();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.bpTree = new BorderPanel();
      this.btnClose = new Button();
      this.pnlExBottom = new PanelEx();
      this.gradientMenuStrip1.SuspendLayout();
      this.pnlContent.SuspendLayout();
      this.bpTree.SuspendLayout();
      this.pnlExBottom.SuspendLayout();
      this.SuspendLayout();
      this.treeView.BorderStyle = BorderStyle.None;
      this.treeView.ContextMenu = this.contextMenu1;
      this.treeView.Dock = DockStyle.Fill;
      this.treeView.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.treeView.FullRowSelect = true;
      this.treeView.HideSelection = false;
      this.treeView.Location = new Point(1, 0);
      this.treeView.Name = "treeView";
      this.treeView.Size = new Size(235, 306);
      this.treeView.TabIndex = 1;
      this.treeView.BeforeCollapse += new TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
      this.treeView.AfterCollapse += new TreeViewEventHandler(this.treeView_AfterCollapse);
      this.treeView.BeforeSelect += new TreeViewCancelEventHandler(this.treeView_BeforeSelect);
      this.contextMenu1.MenuItems.AddRange(new MenuItem[2]
      {
        this.miExpand,
        this.miCollapse
      });
      this.miExpand.Index = 0;
      this.miExpand.Text = "Expand All";
      this.miExpand.Click += new EventHandler(this.miExpand_Click);
      this.miCollapse.Index = 1;
      this.miCollapse.Text = "Collapse All";
      this.miCollapse.Click += new EventHandler(this.miCollapse_Click);
      this.gradientMenuStrip1.ImageScalingSize = new Size(24, 24);
      this.gradientMenuStrip1.Items.AddRange(new ToolStripItem[3]
      {
        (ToolStripItem) this.tsMenuItemEncompassSettings,
        (ToolStripItem) this.tsMenuItemGoToRecent,
        (ToolStripItem) this.tsMenuItemImport
      });
      this.gradientMenuStrip1.Location = new Point(0, 0);
      this.gradientMenuStrip1.Name = "gradientMenuStrip1";
      this.gradientMenuStrip1.Padding = new Padding(0);
      this.gradientMenuStrip1.Size = new Size(701, 24);
      this.gradientMenuStrip1.TabIndex = 7;
      this.gradientMenuStrip1.Text = "gradientMenuStrip1";
      this.tsMenuItemEncompassSettings.DropDownItems.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemClose
      });
      this.tsMenuItemEncompassSettings.Name = "tsMenuItemEncompassSettings";
      this.tsMenuItemEncompassSettings.Size = new Size(124, 24);
      this.tsMenuItemEncompassSettings.Text = "Encompass &Settings";
      this.tsMenuItemClose.Name = "tsMenuItemClose";
      this.tsMenuItemClose.ShortcutKeys = Keys.F4 | Keys.Alt;
      this.tsMenuItemClose.Size = new Size(253, 22);
      this.tsMenuItemClose.Text = "&Close Encompass Settings";
      this.tsMenuItemClose.Click += new EventHandler(this.btnClose_Click);
      this.tsMenuItemGoToRecent.Name = "tsMenuItemGoToRecent";
      this.tsMenuItemGoToRecent.Padding = new Padding(0);
      this.tsMenuItemGoToRecent.ShortcutKeys = Keys.G | Keys.Alt;
      this.tsMenuItemGoToRecent.Size = new Size(79, 24);
      this.tsMenuItemGoToRecent.Text = "&Go to Recent";
      this.tsMenuItemImport.DropDownItems.AddRange(new ToolStripItem[4]
      {
        (ToolStripItem) this.tsMenuItemPointLP,
        (ToolStripItem) this.tsMenuItemPointCC,
        (ToolStripItem) this.tsMenuItemPointSettings,
        (ToolStripItem) this.tsMenuItemCustomForms
      });
      this.tsMenuItemImport.Name = "tsMenuItemImport";
      this.tsMenuItemImport.ShortcutKeys = Keys.I | Keys.Alt;
      this.tsMenuItemImport.Size = new Size(55, 24);
      this.tsMenuItemImport.Text = "&Import";
      this.tsMenuItemPointLP.Name = "tsMenuItemPointLP";
      this.tsMenuItemPointLP.Size = new Size(217, 22);
      this.tsMenuItemPointLP.Text = "Calyx Point &Loan Programs";
      this.tsMenuItemPointLP.Click += new EventHandler(this.tsMenuItemPointLP_Click);
      this.tsMenuItemPointCC.Name = "tsMenuItemPointCC";
      this.tsMenuItemPointCC.Size = new Size(217, 22);
      this.tsMenuItemPointCC.Text = "Calyx Point &Closing Costs";
      this.tsMenuItemPointCC.Click += new EventHandler(this.tsMenuItemPointCC_Click);
      this.tsMenuItemPointSettings.Name = "tsMenuItemPointSettings";
      this.tsMenuItemPointSettings.Size = new Size(217, 22);
      this.tsMenuItemPointSettings.Text = "Calyx Point &Settings";
      this.tsMenuItemPointSettings.Click += new EventHandler(this.tsMenuItemPointSettings_Click);
      this.tsMenuItemCustomForms.Name = "tsMenuItemCustomForms";
      this.tsMenuItemCustomForms.Size = new Size(217, 22);
      this.tsMenuItemCustomForms.Text = "Custom &Forms";
      this.tsMenuItemCustomForms.Click += new EventHandler(this.tsMenuItemCustomForms_Click);
      this.pnlContent.BackColor = Color.White;
      this.pnlContent.Controls.Add((Control) this.pnlRight);
      this.pnlContent.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlContent.Controls.Add((Control) this.bpTree);
      this.pnlContent.Dock = DockStyle.Fill;
      this.pnlContent.Location = new Point(0, 24);
      this.pnlContent.Name = "pnlContent";
      this.pnlContent.Size = new Size(701, 307);
      this.pnlContent.TabIndex = 8;
      this.pnlRight.BackColor = Color.White;
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(244, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(457, 307);
      this.pnlRight.TabIndex = 3;
      this.pnlRight.Scroll += new ScrollEventHandler(this.pnlRight_Scroll);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.bpTree;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(237, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 2;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.bpTree.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.bpTree.Controls.Add((Control) this.treeView);
      this.bpTree.Dock = DockStyle.Left;
      this.bpTree.Location = new Point(0, 0);
      this.bpTree.Name = "bpTree";
      this.bpTree.Size = new Size(237, 307);
      this.bpTree.TabIndex = 4;
      this.btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnClose.BackColor = SystemColors.Control;
      this.btnClose.DialogResult = DialogResult.Cancel;
      this.btnClose.Location = new Point(611, 7);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(75, 22);
      this.btnClose.TabIndex = 9;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.pnlExBottom.Controls.Add((Control) this.btnClose);
      this.pnlExBottom.Dock = DockStyle.Bottom;
      this.pnlExBottom.Location = new Point(0, 331);
      this.pnlExBottom.Name = "pnlExBottom";
      this.pnlExBottom.Size = new Size(701, 36);
      this.pnlExBottom.TabIndex = 10;
      this.BackColor = Color.WhiteSmoke;
      this.CancelButton = (IButtonControl) this.btnClose;
      this.ClientSize = new Size(701, 367);
      this.Controls.Add((Control) this.pnlContent);
      this.Controls.Add((Control) this.pnlExBottom);
      this.Controls.Add((Control) this.gradientMenuStrip1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MainMenuStrip = (MenuStrip) this.gradientMenuStrip1;
      this.MinimumSize = new Size(570, 250);
      this.Name = nameof (SetUpContainer);
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterParent;
      this.FormClosing += new FormClosingEventHandler(this.SetUpContainer_FormClosing);
      this.FormClosed += new FormClosedEventHandler(this.SetUpContainer_FormClosed);
      this.Scroll += new ScrollEventHandler(this.SetUpContainer_Scroll);
      this.SizeChanged += new EventHandler(this.SetUpContainer_SizeChanged);
      this.VisibleChanged += new EventHandler(this.SetUpContainer_VisibleChanged);
      this.gradientMenuStrip1.ResumeLayout(false);
      this.gradientMenuStrip1.PerformLayout();
      this.pnlContent.ResumeLayout(false);
      this.bpTree.ResumeLayout(false);
      this.pnlExBottom.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void miExpand_Click(object sender, EventArgs e) => this.treeView.ExpandAll();

    private void miCollapse_Click(object sender, EventArgs e) => this.treeView.CollapseAll();

    private void tsMenuItemNotImplementedYet_Click(object sender, EventArgs e)
    {
      int num = (int) MessageBox.Show("Not implemented yet.");
    }

    private void tsMenuItemPointLP_Click(object sender, EventArgs e)
    {
      int num = (int) new ImportPointTemplates("LP").ShowDialog((IWin32Window) this);
    }

    private void tsMenuItemPointCC_Click(object sender, EventArgs e)
    {
      int num = (int) new ImportPointTemplates("CC").ShowDialog((IWin32Window) this);
    }

    private void tsMenuItemPointSettings_Click(object sender, EventArgs e)
    {
      int num = (int) new ImportPointSettings("").ShowDialog((IWin32Window) this);
    }

    private void tsMenuItemCustomForms_Click(object sender, EventArgs e)
    {
      int num = (int) new ImportCustomForms().ShowDialog((IWin32Window) this);
    }

    public bool ButtonSaveEnabled
    {
      get => this.setupDialog.ButtonSaveEnabled;
      set => this.setupDialog.ButtonSaveEnabled = value;
    }

    public bool ButtonResetEnabled
    {
      get => this.setupDialog.ButtonResetEnabled;
      set => this.setupDialog.ButtonResetEnabled = value;
    }

    private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void SetUpContainer_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.promptForSave() == DialogResult.Cancel)
      {
        e.Cancel = true;
      }
      else
      {
        this.setupDialog.RemoveContent();
        this.mainForm.ShowInTaskbar = true;
        this.mainForm.BringToFront();
      }
    }

    public void CloseForm() => this.DialogResult = DialogResult.OK;

    private void treeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
    {
      this.suspendBeforeSelect = true;
    }

    private void treeView_AfterCollapse(object sender, TreeViewEventArgs e)
    {
      this.suspendBeforeSelect = false;
    }

    private void SetUpContainer_SizeChanged(object sender, EventArgs e)
    {
      if (this.WindowState == FormWindowState.Minimized)
        this.mainForm.Visible = false;
      else
        this.mainForm.Show();
    }

    private void SetUpContainer_VisibleChanged(object sender, EventArgs e)
    {
      if (this.Visible && !this.gradientMenuStrip1.Items.Contains((ToolStripItem) this.helpMenuItem))
        this.gradientMenuStrip1.Items.Add((ToolStripItem) this.helpMenuItem);
      if (!this.Visible)
        return;
      this.collapsibleSplitter1.IsCollapsed = false;
    }

    private void tsMenuItemClose_Click(object sender, EventArgs e)
    {
      this.btnClose_Click(sender, e);
    }

    private void encompassHelpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) Session.MainForm, "", this.GetHelpTargetName());
    }

    private void SetUpContainer_FormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.mainFormMenuStrip.Items.Contains((ToolStripItem) this.helpMenuItem))
        return;
      this.mainFormMenuStrip.Items.Add((ToolStripItem) this.helpMenuItem);
    }

    private void pnlRight_Scroll(object sender, ScrollEventArgs e)
    {
    }

    private void SetUpContainer_Scroll(object sender, ScrollEventArgs e)
    {
    }
  }
}
