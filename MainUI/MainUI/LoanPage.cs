// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoanPage
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Metrics.Client;
using Elli.Web.Host;
using Elli.Web.Host.SSF.Context;
using Elli.Web.Host.SSF.UI;
using EllieMae.EMLite.BusinessRules;
using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientCommon;
using EllieMae.EMLite.ClientCommon.AIQCapsilon;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.DeepLinking;
using EllieMae.EMLite.Common.DeepLinking.Context;
using EllieMae.EMLite.Common.DeepLinking.Context.Contract;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Metrics;
using EllieMae.EMLite.Common.ProductPricing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.eFolder;
using EllieMae.EMLite.eFolder.LoanCenter;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.Services;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.InputEngine.MilestoneManagement;
using EllieMae.EMLite.JedScriptEngine;
using EllieMae.EMLite.LoanServices;
using EllieMae.EMLite.LoanUtils.DataEngine;
using EllieMae.EMLite.LoanUtils.Workflow;
using EllieMae.EMLite.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.Setup.ExternalOriginatorManagement;
using EllieMae.EMLite.Setup.SecondaryMarketing;
using EllieMae.EMLite.StatusOnline;
using EllieMae.EMLite.ThinThick;
using EllieMae.EMLite.UI;
using EllieMae.EMLite.Verification;
using EllieMae.EMLite.Workflow;
using EllieMae.Encompass.AsmResolver;
using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoanPage : 
    Form,
    IOnlineHelpTarget,
    IWorkArea,
    IWin32Window,
    IPrint,
    IRefreshContents,
    ILoanEditor
  {
    private const string className = "LoanPage";
    protected static string sw = Tracing.SwOutsideLoan;
    private IContainer components;
    private string defaultForm;
    private Panel leftPanel;
    private Panel logContainerPanel;
    private TabControl toolsFormsTabControl;
    private TabPage formPage;
    private TabPage toolPage;
    private DateTime t = DateTime.MinValue;
    private string currentlySelectedScreen = "";
    private StatusReport statusReport;
    private string[] toolsList = new string[50]
    {
      "Workflow Tasks",
      "File Contacts",
      "Business Contacts",
      "Conversation Log",
      "TPO Information",
      "Correspondent Loan Status",
      "Tasks",
      "---------------------------",
      "AUS Tracking",
      "Rep and Warrant Tracker",
      "Disclosure Tracking",
      "Fee Variance Worksheet",
      "LO Compensation",
      "Anti-Steering Safe Harbor Disclosure",
      "Net Tangible Benefit",
      "Compliance Review",
      "ECS Data Viewer",
      "TQL Services",
      "MI Center",
      "Status Online",
      "Amortization Schedule",
      "Co-Mortgagors",
      "Piggyback Loans",
      "Secure Form Transfer",
      "--------------------------- ",
      "Prequalification",
      "Debt Consolidation",
      "Loan Comparison",
      "Cash-to-Close",
      "Rent vs. Own",
      "---------------------------",
      "Lock Request Form",
      "Lock Comparison Tool",
      "Project Review",
      "Underwriter Summary",
      "Verification and Documentation Tracking",
      "Funding Worksheet",
      "Funding Balancing Worksheet",
      "Broker Check Calculation",
      "Secondary Registration",
      "Worst Case Pricing",
      "Interim Servicing Worksheet",
      "Shipping Detail",
      "Collateral Tracking",
      "Correspondent Purchase Advice Form",
      "Purchase Advice Form",
      "---------------------------",
      "Audit Trail",
      "Profit Management",
      "Trust Account"
    };
    private Control topControl;
    private bool readOnly;
    private bool initialLoad = true;
    private Panel rightPanel;
    private InputFormInfo currForm;
    private CollapsibleSplitter horzSplitter;
    private Splitter vertSplitter;
    private Panel workPanel;
    private Panel panel2;
    private CheckBox allFormBox;
    private TabPage servicesPage;
    private EpassServicesControl servicesCtl;
    private EpassServicesSelfHostedDisabledControl disabledService;
    private LogViewer logPanel;
    private LoanScreen freeScreen;
    private string currentlySelectedTool = "";
    private string fieldGoTo = string.Empty;
    private bool suspendEvents;
    private bool noFormBeingShown = true;
    private ToolTip toolTip;
    private EMFormMenu emFormMenuBox;
    private EMFormMenu emToolMenuBox;
    private bool isLoading = true;
    private LoanBusinessRuleInfo loanBizInfo;
    private InputFormInfo[] allForms;
    private LoanLockTool loanLockTool;
    private GradientPanel pnlHeader;
    private ComboBox cboBorrowers;
    private Label label4;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnEditBorrowerPair;
    private StandardIconButton closeBtn;
    private VerticalSeparator verticalSeparator1;
    private StandardIconButton printBtn;
    internal StandardIconButton saveBtn;
    private LoanHeader pnlLoanInfo;
    private ElementControl elmBorrower;
    private CheckBox chkAlphaForms;
    private GradientPanel gradientPanel1;
    private Panel pnlFormDivider;
    private GradientPanel gradientPanel2;
    private CheckBox chkAlphaTools;
    private Panel panel1;
    private IconButton eFolderBtn;
    private Button aiqAnalyzers;
    private Button btnSearchAllRegs;
    private Button btnOpenWebView;
    private VerticalSeparator verticalSeparator2;
    private string[] piggyBackSyncFields;
    private StandardIconButton stdIconBtnSynch;
    private StandardIconButton stdIconBtnLock;
    private VerticalSeparator verticalSeparatorCE;
    private StandardIconButton stdIconBtnIM;
    private ComboBox cmbBoxCEUsers;
    private FeaturesAclManager aclMgr;
    private MilestonesAclManager mAclMgr;
    private LoanOffersControl ctlOffers;
    private TextBox textBoxCEAllowedBlocked;
    private CollapsibleSplitter splitOffers;
    private bool canExclusiveLock = true;
    private TriggerEmailTemplate milestoneTemplateEmailTemplate;
    private LoanConditionGroupMonitor formRuleConditionMonitor;
    private LoanConditionGroupMonitor fieldRuleConditionMonitor;
    private LoanConditionGroupMonitor fieldAccessConditionMonitor;
    private TPOWatchListControl tpoWatchListControl2;
    private FieldAccessBpmManager accessRuleManager;
    private List<string> itemizationReadonlyFields;
    private Sessions.Session session;
    private string formVersion = "";
    private Task<string> SmartClientAttributeFetchTask;
    private LoanSaveProgressDialog progressDialog;
    private bool isEnableLockSnapshotRecapture;
    private bool? _aiqAnalyzersAuthorized;
    private AIQButtonHelper aiqHelper;
    private VORPanel vorPan;
    private VOEPanel voePan;
    private VOLPanel volPan;
    private VODPanel vodPan;
    private VOGGPanel voggPan;
    private VOOIPanel vooiPan;
    private VOOLPanel voolPan;
    private VOOAPanel vooaPan;
    private VOALPanel voalPan;
    private VOMPanel vomPan;
    private TAX4506TPanel tax4506TPan;
    private TAX4506Panel tax4506Pan;
    private const int BORROWER = 0;
    private const int OFFICER = 1;
    private const int PROCESSOR = 2;
    private MilestoneWS milestoneWS;

    public Control TopControl
    {
      set => this.topControl = value;
    }

    private LoanDataMgr loanMgr => this.session.LoanDataMgr;

    private LoanData loan => this.loanMgr.LoanData;

    public bool StdIconBtnSynchEnabled => this.stdIconBtnSynch.Enabled;

    public LoanPage(Sessions.Session session)
    {
      this.session = session;
      if (session.StartupInfo.SendBusinessRuleErrorsToServer)
        Tracing.InitSendToServerDelegate(new Tracing.SendErrorToServerType(RemoteLogger.Write), "BusinessRule");
      if (session.StartupInfo.AllowMileStoneAdjustDateLog)
        Tracing.InitSendToServerDelegate(new Tracing.SendErrorToServerType(RemoteLogger.Write), "MileStoneFinishedDateLog");
      if ((bool) EllieMae.EMLite.RemotingServices.Session.StartupInfo.PolicySettings[(object) "Policies.AutoRecoverDTLogs"])
        Tracing.InitSendToServerDelegate(new Tracing.SendErrorToServerType(RemoteLogger.Write), "DisclosureTrackingLogs");
      this.SmartClientAttributeFetchTask = Task.Run<string>((Func<string>) (() => SmartClientUtils.GetAttribute(EllieMae.EMLite.RemotingServices.Session.CompanyInfo.ClientID, "Encompass.exe", "EnableLockSnapshotRecapture")));
      this.isEnableLockSnapshotRecapture = string.Equals(this.session.ServerManager.GetServerSetting("Policies.EnableLockSnapshotRecapture", true).ToString(), "Enabled", StringComparison.CurrentCultureIgnoreCase);
      this.InitializeComponent();
      this.aclMgr = (FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features);
      this.mAclMgr = (MilestonesAclManager) this.session.ACL.GetAclManager(AclCategory.Milestones);
      this.btnSearchAllRegs.Enabled = this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_SearchAllRegs);
      FeatureConfigsAclManager aclManager = (FeatureConfigsAclManager) this.session.ACL.GetAclManager(AclCategory.FeatureConfigs);
      this.btnOpenWebView.Visible = this.session.EncompassEdition != EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker && aclManager.GetUserApplicationRight(AclFeature.PlatForm_Access) > 0;
      this.formVersion = this.loan.GetField("3969");
      this.leftPanel.Resize += new EventHandler(this.leftPanel_Resize);
      Rectangle bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
      if (bounds.Size.Width > 1024)
        this.leftPanel.Size = new Size(260, this.leftPanel.Size.Height);
      else if (bounds.Size.Width > 800)
        this.leftPanel.Size = new Size(232, this.leftPanel.Size.Height);
      else
        this.leftPanel.Size = new Size(205, this.leftPanel.Size.Height);
      this.pnlFormDivider.BackColor = EncompassColors.Secondary1;
      this.TopLevel = false;
      this.Dock = DockStyle.Fill;
      this.Visible = true;
      this.applyEncompassEditionBehaviors();
      this.session.Application.RegisterService((object) this, typeof (ILoanEditor));
      this.splitOffers.AllowContinuousResize = false;
      this.splitOffers.ExpandedPanelSize = 138;
      this.splitOffers.CollapsedPanelSize = 30;
      if (!LoanScreeningManager.IsFeatureEnabledForUser())
      {
        this.ctlOffers.Visible = false;
        this.ctlOffers.Enabled = false;
        this.splitOffers.Visible = false;
      }
      this.aiqHelper = new AIQButtonHelper((IWin32Window) this, this.aiqAnalyzers);
      if (!this.AIQAnalyzersAuthorized())
        this.aiqAnalyzers.Visible = false;
      this.servicesCtl.InitializeContents();
      if (!this.session.UserInfo.IsAdministrator())
        this.enforceSecurity();
      if (!this.session.SessionObjects.AllowConcurrentEditing)
      {
        this.stdIconBtnSynch.Visible = false;
        this.verticalSeparatorCE.Visible = false;
        this.stdIconBtnLock.Visible = false;
        this.textBoxCEAllowedBlocked.Visible = false;
        this.stdIconBtnIM.Visible = false;
        this.cmbBoxCEUsers.Visible = false;
      }
      else
      {
        this.RefreshCE();
        this.Shown += new EventHandler(this.LoanPage_Shown);
      }
      EllieMae.EMLite.RemotingServices.Session.LoanOpened += new EventHandler(this.loanOpened);
      EllieMae.EMLite.RemotingServices.Session.LoanClosing += new EventHandler(this.loanClosing);
      this.loanOpened((object) null, EventArgs.Empty);
    }

    private bool AIQAnalyzersAuthorized()
    {
      if (!this._aiqAnalyzersAuthorized.HasValue)
      {
        int num;
        if (this.session.StartupInfo.UserAclFeaturConfigRights.ContainsKey(AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers))
          num = ((IEnumerable<int>) new int[2]
          {
            1,
            int.MaxValue
          }).Contains<int>(this.session.StartupInfo.UserAclFeaturConfigRights[AclFeature.SettingsTab_EncompassAIQAccess_AIQAnalyzers]) ? 1 : 0;
        else
          num = 0;
        this._aiqAnalyzersAuthorized = new bool?(num != 0);
      }
      return this._aiqAnalyzersAuthorized.Value;
    }

    private void loanOpened(object sender, EventArgs e)
    {
      EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.OnLoanRefreshedFromServer += new EventHandler(this.onLoanRefreshedFromServer);
    }

    private void loanClosing(object sender, EventArgs e)
    {
      EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.OnLoanRefreshedFromServer -= new EventHandler(this.onLoanRefreshedFromServer);
    }

    private void onLoanRefreshedFromServer(object sender, EventArgs e)
    {
      this.RefreshLoanContents();
      this.ApplyInputFormRules();
      if (!(sender is LoanDataMgr) || !((LoanDataMgr) sender).LauncheFolderNeeded)
        return;
      eFolderDialog.ShowInstance(this.session, true);
    }

    private void LoanPage_Shown(object sender, EventArgs e)
    {
      this.Shown -= new EventHandler(this.LoanPage_Shown);
      this.VisibleChanged += new EventHandler(this.LoanPage_VisibleChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (EllieMae.EMLite.RemotingServices.Session.LoanDataMgr != null)
        EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.OnLoanRefreshedFromServer -= new EventHandler(this.onLoanRefreshedFromServer);
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanPage));
      this.leftPanel = new Panel();
      this.panel2 = new Panel();
      this.toolsFormsTabControl = new TabControl();
      this.formPage = new TabPage();
      this.emFormMenuBox = new EMFormMenu();
      this.toolTip = new ToolTip(this.components);
      this.closeBtn = new StandardIconButton();
      this.printBtn = new StandardIconButton();
      this.saveBtn = new StandardIconButton();
      this.stdIconBtnSynch = new StandardIconButton();
      this.stdIconBtnLock = new StandardIconButton();
      this.stdIconBtnIM = new StandardIconButton();
      this.btnEditBorrowerPair = new StandardIconButton();
      this.pnlFormDivider = new Panel();
      this.gradientPanel1 = new GradientPanel();
      this.allFormBox = new CheckBox();
      this.chkAlphaForms = new CheckBox();
      this.toolPage = new TabPage();
      this.emToolMenuBox = new EMFormMenu();
      this.panel1 = new Panel();
      this.gradientPanel2 = new GradientPanel();
      this.chkAlphaTools = new CheckBox();
      this.servicesPage = new TabPage();
      this.servicesCtl = new EpassServicesControl();
      this.disabledService = new EpassServicesSelfHostedDisabledControl();
      this.vertSplitter = new Splitter();
      this.logContainerPanel = new Panel();
      this.logPanel = new LogViewer(this.session);
      this.rightPanel = new Panel();
      this.workPanel = new Panel();
      this.splitOffers = new CollapsibleSplitter();
      this.ctlOffers = new LoanOffersControl();
      this.horzSplitter = new CollapsibleSplitter();
      this.pnlHeader = new GradientPanel();
      this.tpoWatchListControl2 = new TPOWatchListControl();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.verticalSeparator1 = new VerticalSeparator();
      this.verticalSeparator2 = new VerticalSeparator();
      this.eFolderBtn = new IconButton();
      this.aiqAnalyzers = new Button();
      this.btnSearchAllRegs = new Button();
      this.btnOpenWebView = new Button();
      this.verticalSeparatorCE = new VerticalSeparator();
      this.textBoxCEAllowedBlocked = new TextBox();
      this.cmbBoxCEUsers = new ComboBox();
      this.cboBorrowers = new ComboBox();
      this.label4 = new Label();
      this.elmBorrower = new ElementControl();
      this.pnlLoanInfo = new LoanHeader();
      this.leftPanel.SuspendLayout();
      this.panel2.SuspendLayout();
      this.toolsFormsTabControl.SuspendLayout();
      this.formPage.SuspendLayout();
      ((ISupportInitialize) this.closeBtn).BeginInit();
      ((ISupportInitialize) this.printBtn).BeginInit();
      ((ISupportInitialize) this.saveBtn).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSynch).BeginInit();
      ((ISupportInitialize) this.stdIconBtnLock).BeginInit();
      ((ISupportInitialize) this.stdIconBtnIM).BeginInit();
      ((ISupportInitialize) this.btnEditBorrowerPair).BeginInit();
      this.gradientPanel1.SuspendLayout();
      this.toolPage.SuspendLayout();
      this.gradientPanel2.SuspendLayout();
      this.servicesPage.SuspendLayout();
      this.logContainerPanel.SuspendLayout();
      this.rightPanel.SuspendLayout();
      this.pnlHeader.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.eFolderBtn).BeginInit();
      this.SuspendLayout();
      this.leftPanel.Controls.Add((Control) this.panel2);
      this.leftPanel.Controls.Add((Control) this.vertSplitter);
      this.leftPanel.Controls.Add((Control) this.logContainerPanel);
      this.leftPanel.Dock = DockStyle.Left;
      this.leftPanel.Location = new Point(0, 83);
      this.leftPanel.Name = "leftPanel";
      this.leftPanel.Padding = new Padding(0, 1, 0, 1);
      this.leftPanel.Size = new Size(222, 395);
      this.leftPanel.TabIndex = 0;
      this.panel2.Controls.Add((Control) this.toolsFormsTabControl);
      this.panel2.Dock = DockStyle.Fill;
      this.panel2.Location = new Point(0, 248);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(222, 146);
      this.panel2.TabIndex = 3;
      this.toolsFormsTabControl.Controls.Add((Control) this.formPage);
      this.toolsFormsTabControl.Controls.Add((Control) this.toolPage);
      this.toolsFormsTabControl.Controls.Add((Control) this.servicesPage);
      this.toolsFormsTabControl.Dock = DockStyle.Fill;
      this.toolsFormsTabControl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.toolsFormsTabControl.ItemSize = new Size(39, 20);
      this.toolsFormsTabControl.Location = new Point(0, 0);
      this.toolsFormsTabControl.Name = "toolsFormsTabControl";
      this.toolsFormsTabControl.Padding = new Point(11, 3);
      this.toolsFormsTabControl.SelectedIndex = 0;
      this.toolsFormsTabControl.Size = new Size(222, 146);
      this.toolsFormsTabControl.TabIndex = 1;
      this.toolsFormsTabControl.SelectedIndexChanged += new EventHandler(this.toolsFormsTabControl_SelectedIndexChanged);
      this.formPage.BackColor = Color.White;
      this.formPage.Controls.Add((Control) this.emFormMenuBox);
      this.formPage.Controls.Add((Control) this.pnlFormDivider);
      this.formPage.Controls.Add((Control) this.gradientPanel1);
      this.formPage.Location = new Point(4, 24);
      this.formPage.Name = "formPage";
      this.formPage.Padding = new Padding(0, 2, 2, 2);
      this.formPage.Size = new Size(214, 118);
      this.formPage.TabIndex = 0;
      this.formPage.Text = "Forms";
      this.formPage.UseVisualStyleBackColor = true;
      this.emFormMenuBox.AlternatingColors = false;
      this.emFormMenuBox.BorderStyle = BorderStyle.None;
      this.emFormMenuBox.Dock = DockStyle.Fill;
      this.emFormMenuBox.GridLines = false;
      this.emFormMenuBox.HoverToolTip = this.toolTip;
      this.emFormMenuBox.IntegralHeight = false;
      this.emFormMenuBox.Location = new Point(0, 2);
      this.emFormMenuBox.Name = "emFormMenuBox";
      this.emFormMenuBox.Size = new Size(212, 89);
      this.emFormMenuBox.TabIndex = 0;
      this.emFormMenuBox.SelectedIndexChanged += new EventHandler(this.emFormMenuList_SelectedIndexChanged);
      this.toolTip.AutoPopDelay = 50000;
      this.toolTip.InitialDelay = 500;
      this.toolTip.ReshowDelay = 500;
      this.closeBtn.BackColor = Color.Transparent;
      this.closeBtn.Location = new Point(495, 5);
      this.closeBtn.Margin = new Padding(2, 5, 0, 3);
      this.closeBtn.MouseDownImage = (Image) null;
      this.closeBtn.Name = "closeBtn";
      this.closeBtn.Size = new Size(16, 16);
      this.closeBtn.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.closeBtn.TabIndex = 0;
      this.closeBtn.TabStop = false;
      this.toolTip.SetToolTip((Control) this.closeBtn, "Exit Loan File");
      this.closeBtn.Click += new EventHandler(this.closeBtn_Click);
      this.printBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.printBtn.BackColor = Color.Transparent;
      this.printBtn.Location = new Point(471, 5);
      this.printBtn.Margin = new Padding(2, 5, 2, 3);
      this.printBtn.MouseDownImage = (Image) null;
      this.printBtn.Name = "printBtn";
      this.printBtn.Size = new Size(16, 16);
      this.printBtn.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.printBtn.TabIndex = 2;
      this.printBtn.TabStop = false;
      this.toolTip.SetToolTip((Control) this.printBtn, "Print");
      this.printBtn.Click += new EventHandler(this.PrintPreview);
      this.saveBtn.BackColor = Color.Transparent;
      this.saveBtn.Location = new Point(451, 5);
      this.saveBtn.Margin = new Padding(2, 5, 2, 3);
      this.saveBtn.MouseDownImage = (Image) null;
      this.saveBtn.Name = "saveBtn";
      this.saveBtn.Size = new Size(16, 16);
      this.saveBtn.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.saveBtn.TabIndex = 3;
      this.saveBtn.TabStop = false;
      this.toolTip.SetToolTip((Control) this.saveBtn, "Save Loan");
      this.saveBtn.Click += new EventHandler(this.saveBtn_Click);
      this.stdIconBtnSynch.BackColor = Color.Transparent;
      this.stdIconBtnSynch.Enabled = false;
      this.stdIconBtnSynch.Location = new Point(430, 5);
      this.stdIconBtnSynch.Margin = new Padding(3, 5, 3, 3);
      this.stdIconBtnSynch.MouseDownImage = (Image) null;
      this.stdIconBtnSynch.Name = "stdIconBtnSynch";
      this.stdIconBtnSynch.Size = new Size(16, 16);
      this.stdIconBtnSynch.StandardButtonType = StandardIconButton.ButtonType.SynchLoanDataButton;
      this.stdIconBtnSynch.TabIndex = 34;
      this.stdIconBtnSynch.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnSynch, "Get Updates\r\nThe Get Updates icon turns green when the loan is saved\r\nby another user. To get the updates, click the icon.");
      this.stdIconBtnSynch.Click += new EventHandler(this.saveBtn_Click);
      this.stdIconBtnLock.BackColor = Color.Transparent;
      this.stdIconBtnLock.Location = new Point(322, 5);
      this.stdIconBtnLock.Margin = new Padding(3, 5, 3, 3);
      this.stdIconBtnLock.MouseDownImage = (Image) null;
      this.stdIconBtnLock.Name = "stdIconBtnLock";
      this.stdIconBtnLock.Size = new Size(16, 16);
      this.stdIconBtnLock.StandardButtonType = StandardIconButton.ButtonType.BlockCEButton;
      this.stdIconBtnLock.TabIndex = 35;
      this.stdIconBtnLock.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnLock, "Block Multi-User Editing");
      this.stdIconBtnLock.Click += new EventHandler(this.stdIconBtnLock_Click);
      this.stdIconBtnIM.BackColor = Color.Transparent;
      this.stdIconBtnIM.Location = new Point(155, 5);
      this.stdIconBtnIM.Margin = new Padding(2, 5, 2, 3);
      this.stdIconBtnIM.MouseDownImage = (Image) null;
      this.stdIconBtnIM.Name = "stdIconBtnIM";
      this.stdIconBtnIM.Size = new Size(16, 16);
      this.stdIconBtnIM.StandardButtonType = StandardIconButton.ButtonType.ChatButton;
      this.stdIconBtnIM.TabIndex = 37;
      this.stdIconBtnIM.TabStop = false;
      this.toolTip.SetToolTip((Control) this.stdIconBtnIM, "Start Chat");
      this.stdIconBtnIM.Click += new EventHandler(this.stdIconBtnIM_Click);
      this.btnEditBorrowerPair.BackColor = Color.Transparent;
      this.btnEditBorrowerPair.Location = new Point(334, 8);
      this.btnEditBorrowerPair.MouseDownImage = (Image) null;
      this.btnEditBorrowerPair.Name = "btnEditBorrowerPair";
      this.btnEditBorrowerPair.Size = new Size(16, 16);
      this.btnEditBorrowerPair.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.btnEditBorrowerPair.TabIndex = 5;
      this.btnEditBorrowerPair.TabStop = false;
      this.toolTip.SetToolTip((Control) this.btnEditBorrowerPair, "Manage Borrowers");
      this.btnEditBorrowerPair.Click += new EventHandler(this.btnEditBorrowerPair_Click);
      this.pnlFormDivider.BackColor = Color.FromArgb(200, 199, 199);
      this.pnlFormDivider.Dock = DockStyle.Bottom;
      this.pnlFormDivider.Location = new Point(0, 91);
      this.pnlFormDivider.Name = "pnlFormDivider";
      this.pnlFormDivider.Size = new Size(212, 1);
      this.pnlFormDivider.TabIndex = 5;
      this.gradientPanel1.Borders = AnchorStyles.None;
      this.gradientPanel1.Controls.Add((Control) this.allFormBox);
      this.gradientPanel1.Controls.Add((Control) this.chkAlphaForms);
      this.gradientPanel1.Dock = DockStyle.Bottom;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel1.Location = new Point(0, 92);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(212, 24);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel1.TabIndex = 4;
      this.allFormBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.allFormBox.BackColor = Color.Transparent;
      this.allFormBox.Location = new Point(134, 2);
      this.allFormBox.Name = "allFormBox";
      this.allFormBox.Size = new Size(70, 20);
      this.allFormBox.TabIndex = 2;
      this.allFormBox.TabStop = false;
      this.allFormBox.Text = "Sh&ow All";
      this.allFormBox.UseVisualStyleBackColor = false;
      this.allFormBox.CheckedChanged += new EventHandler(this.allFormBox_CheckedChanged);
      this.chkAlphaForms.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAlphaForms.BackColor = Color.Transparent;
      this.chkAlphaForms.Location = new Point(4, 2);
      this.chkAlphaForms.Name = "chkAlphaForms";
      this.chkAlphaForms.Size = new Size(128, 20);
      this.chkAlphaForms.TabIndex = 3;
      this.chkAlphaForms.TabStop = false;
      this.chkAlphaForms.Text = "Show in Alpha Order";
      this.chkAlphaForms.UseVisualStyleBackColor = false;
      this.chkAlphaForms.CheckedChanged += new EventHandler(this.chkAlphaForms_CheckedChanged);
      this.toolPage.BackColor = Color.White;
      this.toolPage.Controls.Add((Control) this.emToolMenuBox);
      this.toolPage.Controls.Add((Control) this.panel1);
      this.toolPage.Controls.Add((Control) this.gradientPanel2);
      this.toolPage.Location = new Point(4, 24);
      this.toolPage.Name = "toolPage";
      this.toolPage.Padding = new Padding(0, 2, 2, 2);
      this.toolPage.Size = new Size(214, 118);
      this.toolPage.TabIndex = 1;
      this.toolPage.Text = "Tools";
      this.toolPage.UseVisualStyleBackColor = true;
      this.emToolMenuBox.AlternatingColors = false;
      this.emToolMenuBox.BorderStyle = BorderStyle.None;
      this.emToolMenuBox.Dock = DockStyle.Fill;
      this.emToolMenuBox.GridLines = false;
      this.emToolMenuBox.HoverToolTip = this.toolTip;
      this.emToolMenuBox.IntegralHeight = false;
      this.emToolMenuBox.Location = new Point(0, 2);
      this.emToolMenuBox.Name = "emToolMenuBox";
      this.emToolMenuBox.Size = new Size(212, 89);
      this.emToolMenuBox.TabIndex = 1;
      this.emToolMenuBox.SelectedIndexChanged += new EventHandler(this.emToolMenuList_SelectedIndexChanged);
      this.panel1.BackColor = Color.FromArgb(200, 199, 199);
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 91);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(212, 1);
      this.panel1.TabIndex = 6;
      this.gradientPanel2.Borders = AnchorStyles.None;
      this.gradientPanel2.Controls.Add((Control) this.chkAlphaTools);
      this.gradientPanel2.Dock = DockStyle.Bottom;
      this.gradientPanel2.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gradientPanel2.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gradientPanel2.Location = new Point(0, 92);
      this.gradientPanel2.Name = "gradientPanel2";
      this.gradientPanel2.Size = new Size(212, 24);
      this.gradientPanel2.Style = GradientPanel.PanelStyle.TableFooter;
      this.gradientPanel2.TabIndex = 5;
      this.chkAlphaTools.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.chkAlphaTools.BackColor = Color.Transparent;
      this.chkAlphaTools.Location = new Point(4, 2);
      this.chkAlphaTools.Name = "chkAlphaTools";
      this.chkAlphaTools.Size = new Size(128, 20);
      this.chkAlphaTools.TabIndex = 3;
      this.chkAlphaTools.TabStop = false;
      this.chkAlphaTools.Text = "Show in Alpha Order";
      this.chkAlphaTools.UseVisualStyleBackColor = false;
      this.chkAlphaTools.CheckedChanged += new EventHandler(this.chkAlphaTools_CheckedChanged);
      if (this.servicesCtl.IsEncompassSelfHosted)
        this.servicesPage.Controls.Add((Control) this.disabledService);
      else
        this.servicesPage.Controls.Add((Control) this.servicesCtl);
      this.servicesPage.Location = new Point(4, 24);
      this.servicesPage.Name = "servicesPage";
      this.servicesPage.Padding = new Padding(0, 2, 2, 2);
      this.servicesPage.Size = new Size(214, 118);
      this.servicesPage.TabIndex = 3;
      this.servicesPage.Text = "Services";
      this.servicesPage.UseVisualStyleBackColor = true;
      this.servicesCtl.Dock = DockStyle.Fill;
      this.servicesCtl.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.servicesCtl.Location = new Point(0, 2);
      this.servicesCtl.Name = "servicesCtl";
      this.servicesCtl.Size = new Size(212, 114);
      this.servicesCtl.TabIndex = 0;
      this.vertSplitter.BorderStyle = BorderStyle.Fixed3D;
      this.vertSplitter.Dock = DockStyle.Top;
      this.vertSplitter.Location = new Point(0, 245);
      this.vertSplitter.Name = "vertSplitter";
      this.vertSplitter.Size = new Size(222, 3);
      this.vertSplitter.TabIndex = 1;
      this.vertSplitter.TabStop = false;
      this.logContainerPanel.Controls.Add((Control) this.logPanel);
      this.logContainerPanel.Dock = DockStyle.Top;
      this.logContainerPanel.Location = new Point(0, 1);
      this.logContainerPanel.Name = "logContainerPanel";
      this.logContainerPanel.Padding = new Padding(0, 2, 0, 0);
      this.logContainerPanel.Size = new Size(222, 244);
      this.logContainerPanel.TabIndex = 0;
      this.logPanel.AutoScroll = true;
      this.logPanel.BackColor = Color.Transparent;
      this.logPanel.Dock = DockStyle.Fill;
      this.logPanel.Location = new Point(0, 2);
      this.logPanel.Name = "logPanel";
      this.logPanel.Size = new Size(222, 242);
      this.logPanel.TabIndex = 1;
      this.rightPanel.Controls.Add((Control) this.workPanel);
      this.rightPanel.Controls.Add((Control) this.splitOffers);
      this.rightPanel.Controls.Add((Control) this.ctlOffers);
      this.rightPanel.Dock = DockStyle.Fill;
      this.rightPanel.Location = new Point(229, 83);
      this.rightPanel.Name = "rightPanel";
      this.rightPanel.Padding = new Padding(0, 1, 0, 1);
      this.rightPanel.Size = new Size(825, 395);
      this.rightPanel.TabIndex = 4;
      this.workPanel.BackColor = Color.Transparent;
      this.workPanel.Dock = DockStyle.Fill;
      this.workPanel.Location = new Point(0, 1);
      this.workPanel.Name = "workPanel";
      this.workPanel.Size = new Size(796, 393);
      this.workPanel.TabIndex = 25;
      this.splitOffers.AllowContinuousResize = false;
      this.splitOffers.AnimationDelay = 20;
      this.splitOffers.AnimationStep = 20;
      this.splitOffers.BorderStyle3D = Border3DStyle.Flat;
      this.splitOffers.CollapsedPanelSize = 30;
      this.splitOffers.ControlToHide = (Control) this.ctlOffers;
      this.splitOffers.Dock = DockStyle.Right;
      this.splitOffers.ExpandedPanelSize = 120;
      this.splitOffers.ExpandParentForm = false;
      this.splitOffers.Location = new Point(796, 1);
      this.splitOffers.Name = "splitOffers";
      this.splitOffers.TabIndex = 27;
      this.splitOffers.TabStop = false;
      this.splitOffers.UseAnimations = false;
      this.splitOffers.VisualStyle = VisualStyles.Encompass;
      this.ctlOffers.BackColor = Color.WhiteSmoke;
      this.ctlOffers.Dock = DockStyle.Right;
      this.ctlOffers.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlOffers.Location = new Point(799, 1);
      this.ctlOffers.Name = "ctlOffers";
      this.ctlOffers.Size = new Size(26, 393);
      this.ctlOffers.TabIndex = 26;
      this.horzSplitter.AnimationDelay = 20;
      this.horzSplitter.AnimationStep = 20;
      this.horzSplitter.BorderStyle3D = Border3DStyle.Flat;
      this.horzSplitter.ControlToHide = (Control) this.leftPanel;
      this.horzSplitter.ExpandParentForm = false;
      this.horzSplitter.Location = new Point(222, 83);
      this.horzSplitter.Name = "horzSplitter";
      this.horzSplitter.TabIndex = 1;
      this.horzSplitter.TabStop = false;
      this.horzSplitter.UseAnimations = false;
      this.horzSplitter.VisualStyle = VisualStyles.Encompass;
      this.horzSplitter.Paint += new PaintEventHandler(this.horzSplitter_Paint);
      this.pnlHeader.BackColorGlassyStyle = true;
      this.pnlHeader.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.pnlHeader.Controls.Add((Control) this.tpoWatchListControl2);
      this.pnlHeader.Controls.Add((Control) this.flowLayoutPanel1);
      this.pnlHeader.Controls.Add((Control) this.btnEditBorrowerPair);
      this.pnlHeader.Controls.Add((Control) this.cboBorrowers);
      this.pnlHeader.Controls.Add((Control) this.label4);
      this.pnlHeader.Controls.Add((Control) this.elmBorrower);
      this.pnlHeader.Dock = DockStyle.Top;
      this.pnlHeader.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.pnlHeader.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.pnlHeader.Location = new Point(0, 0);
      this.pnlHeader.Name = "pnlHeader";
      this.pnlHeader.Size = new Size(1054, 31);
      this.pnlHeader.Style = GradientPanel.PanelStyle.PageHeader;
      this.pnlHeader.TabIndex = 5;
      this.tpoWatchListControl2.Location = new Point(354, 5);
      this.tpoWatchListControl2.Name = "tpoWatchListControl2";
      this.tpoWatchListControl2.Size = new Size(175, 23);
      this.tpoWatchListControl2.TabIndex = 8;
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.closeBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator1);
      this.flowLayoutPanel1.Controls.Add((Control) this.printBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.saveBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnSynch);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparator2);
      this.flowLayoutPanel1.Controls.Add((Control) this.eFolderBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.aiqAnalyzers);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSearchAllRegs);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnOpenWebView);
      this.flowLayoutPanel1.Controls.Add((Control) this.verticalSeparatorCE);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnLock);
      this.flowLayoutPanel1.Controls.Add((Control) this.textBoxCEAllowedBlocked);
      this.flowLayoutPanel1.Controls.Add((Control) this.stdIconBtnIM);
      this.flowLayoutPanel1.Controls.Add((Control) this.cmbBoxCEUsers);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(531, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(511, 29);
      this.flowLayoutPanel1.TabIndex = 6;
      this.flowLayoutPanel1.DoubleClick += new EventHandler(this.flowLayoutPanel1_DoubleClick);
      this.verticalSeparator1.Location = new Point(490, 5);
      this.verticalSeparator1.Margin = new Padding(1, 5, 1, 3);
      this.verticalSeparator1.MaximumSize = new Size(2, 16);
      this.verticalSeparator1.MinimumSize = new Size(2, 16);
      this.verticalSeparator1.Name = "verticalSeparator1";
      this.verticalSeparator1.Size = new Size(2, 16);
      this.verticalSeparator1.TabIndex = 1;
      this.verticalSeparator1.Text = "verticalSeparator1";
      this.verticalSeparator2.Location = new Point(424, 5);
      this.verticalSeparator2.Margin = new Padding(1, 5, 1, 3);
      this.verticalSeparator2.MaximumSize = new Size(2, 16);
      this.verticalSeparator2.MinimumSize = new Size(2, 16);
      this.verticalSeparator2.Name = "verticalSeparator2";
      this.verticalSeparator2.Size = new Size(2, 16);
      this.verticalSeparator2.TabIndex = 4;
      this.verticalSeparator2.Text = "verticalSeparator2";
      this.aiqAnalyzers.Location = new Point(104, 5);
      this.aiqAnalyzers.Margin = new Padding(1, 3, 1, 3);
      this.aiqAnalyzers.Name = "aiqAnalyzers";
      this.aiqAnalyzers.Padding = new Padding(1, 0, 0, 0);
      this.aiqAnalyzers.Size = new Size(100, 22);
      this.aiqAnalyzers.TabIndex = 32;
      this.aiqAnalyzers.Text = "Analyzers";
      this.aiqAnalyzers.UseVisualStyleBackColor = true;
      this.aiqAnalyzers.Click += new EventHandler(this.aiqAnalyzers_Click);
      this.btnSearchAllRegs.Location = new Point(250, 5);
      this.btnSearchAllRegs.Margin = new Padding(1, 3, 1, 3);
      this.btnSearchAllRegs.Name = "btnSearchAllRegs";
      this.btnSearchAllRegs.Padding = new Padding(1, 0, 0, 0);
      this.btnSearchAllRegs.Size = new Size(100, 22);
      this.btnSearchAllRegs.TabIndex = 32;
      this.btnSearchAllRegs.Text = "Search AllRegs";
      this.btnSearchAllRegs.UseVisualStyleBackColor = true;
      this.btnSearchAllRegs.Click += new EventHandler(this.btnSearchAllRegs_Click);
      this.btnOpenWebView.Location = new Point(230, 5);
      this.btnOpenWebView.Margin = new Padding(1, 3, 1, 3);
      this.btnOpenWebView.Name = "btnOpenWebView";
      this.btnOpenWebView.Padding = new Padding(1, 0, 0, 0);
      this.btnOpenWebView.Size = new Size(98, 22);
      this.btnOpenWebView.TabIndex = 33;
      this.btnOpenWebView.Text = "Open Web View";
      this.btnOpenWebView.UseVisualStyleBackColor = true;
      this.btnOpenWebView.Click += new EventHandler(this.btnOpenWebView_ClickAsync);
      this.eFolderBtn.BackColor = Color.Transparent;
      this.eFolderBtn.DisabledImage = (Image) null;
      this.eFolderBtn.Image = (Image) componentResourceManager.GetObject("eFolderBtn.Image");
      this.eFolderBtn.Location = new Point(346, 3);
      this.eFolderBtn.Margin = new Padding(1, 3, 1, 3);
      this.eFolderBtn.MouseDownImage = (Image) null;
      this.eFolderBtn.MouseOverImage = (Image) componentResourceManager.GetObject("eFolderBtn.MouseOverImage");
      this.eFolderBtn.Name = "eFolderBtn";
      this.eFolderBtn.Size = new Size(76, 20);
      this.eFolderBtn.TabIndex = 33;
      this.eFolderBtn.TabStop = false;
      this.eFolderBtn.Click += new EventHandler(this.eFolderBtn_Click);
      this.verticalSeparatorCE.Location = new Point(342, 5);
      this.verticalSeparatorCE.Margin = new Padding(1, 5, 1, 3);
      this.verticalSeparatorCE.MaximumSize = new Size(2, 16);
      this.verticalSeparatorCE.MinimumSize = new Size(2, 16);
      this.verticalSeparatorCE.Name = "verticalSeparatorCE";
      this.verticalSeparatorCE.Size = new Size(2, 16);
      this.verticalSeparatorCE.TabIndex = 36;
      this.verticalSeparatorCE.Text = "verticalSeparatorCE";
      this.textBoxCEAllowedBlocked.BackColor = SystemColors.Window;
      this.textBoxCEAllowedBlocked.Location = new Point(176, 3);
      this.textBoxCEAllowedBlocked.Name = "textBoxCEAllowedBlocked";
      this.textBoxCEAllowedBlocked.ReadOnly = true;
      this.textBoxCEAllowedBlocked.Size = new Size(140, 20);
      this.textBoxCEAllowedBlocked.TabIndex = 39;
      this.textBoxCEAllowedBlocked.Text = "Multi-User Editing Allowed";
      this.cmbBoxCEUsers.Anchor = AnchorStyles.Top;
      this.cmbBoxCEUsers.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxCEUsers.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cmbBoxCEUsers.FormattingEnabled = true;
      this.cmbBoxCEUsers.Location = new Point(11, 3);
      this.cmbBoxCEUsers.Margin = new Padding(2, 3, 2, 3);
      this.cmbBoxCEUsers.Name = "cmbBoxCEUsers";
      this.cmbBoxCEUsers.Size = new Size(140, 22);
      this.cmbBoxCEUsers.TabIndex = 38;
      this.cboBorrowers.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboBorrowers.FormattingEnabled = true;
      this.cboBorrowers.Location = new Point(103, 5);
      this.cboBorrowers.Name = "cboBorrowers";
      this.cboBorrowers.Size = new Size(227, 22);
      this.cboBorrowers.TabIndex = 1;
      this.cboBorrowers.SelectedIndexChanged += new EventHandler(this.cboBorrowers_SelectedIndexChanged);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(25, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(73, 16);
      this.label4.TabIndex = 2;
      this.label4.Text = "Borrowers";
      this.elmBorrower.BackColor = Color.Transparent;
      this.elmBorrower.Element = (object) null;
      this.elmBorrower.Location = new Point(8, 7);
      this.elmBorrower.Name = "elmBorrower";
      this.elmBorrower.Size = new Size(16, 16);
      this.elmBorrower.TabIndex = 7;
      this.pnlLoanInfo.Dock = DockStyle.Top;
      this.pnlLoanInfo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.pnlLoanInfo.Location = new Point(0, 31);
      this.pnlLoanInfo.Name = "pnlLoanInfo";
      this.pnlLoanInfo.Size = new Size(1054, 52);
      this.pnlLoanInfo.TabIndex = 6;
      this.AutoScaleMode = AutoScaleMode.None;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(1054, 478);
      this.ControlBox = false;
      this.Controls.Add((Control) this.rightPanel);
      this.Controls.Add((Control) this.horzSplitter);
      this.Controls.Add((Control) this.leftPanel);
      this.Controls.Add((Control) this.pnlLoanInfo);
      this.Controls.Add((Control) this.pnlHeader);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (LoanPage);
      this.leftPanel.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.toolsFormsTabControl.ResumeLayout(false);
      this.formPage.ResumeLayout(false);
      ((ISupportInitialize) this.closeBtn).EndInit();
      ((ISupportInitialize) this.printBtn).EndInit();
      ((ISupportInitialize) this.saveBtn).EndInit();
      ((ISupportInitialize) this.stdIconBtnSynch).EndInit();
      ((ISupportInitialize) this.stdIconBtnLock).EndInit();
      ((ISupportInitialize) this.stdIconBtnIM).EndInit();
      ((ISupportInitialize) this.btnEditBorrowerPair).EndInit();
      this.gradientPanel1.ResumeLayout(false);
      this.toolPage.ResumeLayout(false);
      this.gradientPanel2.ResumeLayout(false);
      this.servicesPage.ResumeLayout(false);
      this.logContainerPanel.ResumeLayout(false);
      this.rightPanel.ResumeLayout(false);
      this.pnlHeader.ResumeLayout(false);
      this.pnlHeader.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.flowLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.eFolderBtn).EndInit();
      this.ResumeLayout(false);
    }

    public void ShowVerifPanel(string verifType)
    {
      PanelBase newControl = (PanelBase) null;
      switch (verifType)
      {
        case "TAX4506":
          if (this.tax4506Pan == null)
            this.tax4506Pan = new TAX4506Panel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.tax4506Pan;
          break;
        case "TAX4506T":
          if (this.tax4506TPan == null)
            this.tax4506TPan = new TAX4506TPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.tax4506TPan;
          break;
        case "VOAL":
          if (this.voalPan == null)
            this.voalPan = new VOALPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.voalPan;
          break;
        case "VOD":
          if (this.vodPan == null)
            this.vodPan = new VODPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.vodPan;
          break;
        case "VOE":
          if (this.voePan == null)
            this.voePan = new VOEPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.voePan;
          break;
        case "VOGG":
          if (this.voggPan == null)
            this.voggPan = new VOGGPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.voggPan;
          break;
        case "VOL":
          if (this.volPan == null)
            this.volPan = new VOLPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.volPan;
          break;
        case "VOM":
          if (this.vomPan == null)
            this.vomPan = new VOMPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.vomPan;
          break;
        case "VOOA":
          if (this.vooaPan == null)
            this.vooaPan = new VOOAPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.vooaPan;
          break;
        case "VOOI":
          if (this.vooiPan == null)
            this.vooiPan = new VOOIPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.vooiPan;
          break;
        case "VOOL":
          if (this.voolPan == null)
            this.voolPan = new VOOLPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.voolPan;
          break;
        case "VOR":
          if (this.vorPan == null)
            this.vorPan = new VORPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
          newControl = (PanelBase) this.vorPan;
          break;
      }
      this.AddToWorkArea((Control) newControl);
      this.currForm = this.loanMgr.InputFormSettings.GetForm(verifType);
      newControl.RefreshListView((object) null, (EventArgs) null);
    }

    private void leftPanel_Resize(object sender, EventArgs e)
    {
      this.logContainerPanel.Size = new Size(this.logContainerPanel.Size.Width, this.leftPanel.Size.Height / 2);
    }

    public void RefreshContents(string id)
    {
      if (this.freeScreen == null || !this.workPanel.Controls.Contains((Control) this.freeScreen))
        return;
      this.freeScreen.RefreshContents(id);
    }

    public bool GetInputEngineService(LoanData loan, InputEngineServiceType serviceType)
    {
      using (LoanScreen loanScreen = new LoanScreen(this.session, (IWin32Window) null, (IHtmlInput) loan))
        return loanScreen.GetInputEngineService(loan, serviceType);
    }

    public void AddMilestoneWorksheet(MilestoneLog milestoneLog)
    {
      this.AddToWorkArea((Control) new MilestoneWS(this.session, milestoneLog));
    }

    public bool IsPrimaryEditor => true;

    public void RefreshContents()
    {
      if (this.loanMgr == null || this.loan == null)
        return;
      foreach (Control control in (ArrangedElementCollection) this.workPanel.Controls)
      {
        if (control is IRefreshContents)
          ((IRefreshContents) control).RefreshContents();
      }
      this.pnlLoanInfo.RefreshContents();
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      if (this.loan == null || this.loan.LinkedData != null || this.emToolMenuBox.Items.Contains((object) "Piggyback Loan") || !(this.loan.GetField("19") != "ConstructionOnly") && !(this.loan.GetField("4084") != "Y"))
        return;
      this.loadToolList();
    }

    public void RefreshLoanContents()
    {
      for (int index = 4000; index <= 4007; ++index)
      {
        this.loan.UnregisterFieldValueChangeEventHandler(string.Concat((object) index), new Routine(this.onBorrowerNameChanged));
        this.loan.RegisterFieldValueChangeEventHandler(string.Concat((object) index), new Routine(this.onBorrowerNameChanged));
      }
      this.loan.UnregisterFieldValueChangeEventHandler("1172", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("1172", new Routine(this.loadFirstForm));
      this.loan.UnregisterFieldValueChangeEventHandler("19", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.RegisterFieldValueChangeEventHandler("19", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.UnregisterFieldValueChangeEventHandler("4084", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.RegisterFieldValueChangeEventHandler("4084", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.UnregisterFieldValueChangeEventHandler("HMDA.X27", new Routine(this.loadHMDAForm));
      this.loan.RegisterFieldValueChangeEventHandler("HMDA.X27", new Routine(this.loadHMDAForm));
      this.loan.UnregisterFieldValueChangeEventHandler("1393", new Routine(this.loadHMDAForm));
      this.loan.RegisterFieldValueChangeEventHandler("1393", new Routine(this.loadHMDAForm));
      this.loan.UnregisterFieldValueChangeEventHandler("3312", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("3312", new Routine(this.loadFirstForm));
      this.loan.UnregisterFieldValueChangeEventHandler("749", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("749", new Routine(this.loadFirstForm));
      this.loan.UnregisterFieldValueChangeEventHandler("1825", new Routine(this.refreshVerificationForm));
      this.loan.RegisterFieldValueChangeEventHandler("1825", new Routine(this.refreshVerificationForm));
      this.loan.FormVersionChanged -= new EventHandler(this.refreshFormMenuToolList);
      this.loan.FormVersionChanged += new EventHandler(this.refreshFormMenuToolList);
      this.loan.BorrowerPairCreated -= new EventHandler(this.loan_BorrowerPairCreated);
      this.loan.BorrowerPairCreated += new EventHandler(this.loan_BorrowerPairCreated);
      this.loan.Disclosure2015Created -= new EventHandler(this.refreshDT2015Log);
      this.loan.Disclosure2015Created += new EventHandler(this.refreshDT2015Log);
      foreach (Control control in (ArrangedElementCollection) this.workPanel.Controls)
      {
        if (control is IRefreshContents)
          ((IRefreshContents) control).RefreshLoanContents();
      }
      this.pnlLoanInfo.AttachToLoan(this.loanMgr);
      eFolderDialog.RefreshLoanContents();
      this.RefreshLogPanel();
      this.rebuildFormList();
      this.RefreshCE();
      this.RefreshBorrowerPairs();
      if (this.isTPOLoan())
      {
        this.RefreshWatchList();
        this.tpoWatchListControl2.Visible = true;
      }
      else
        this.tpoWatchListControl2.Visible = false;
    }

    private void loan_BorrowerPairCreated(object sender, EventArgs e)
    {
      LoanServiceManager.NewBorrowerPairCreated((string) sender, this.session.LoanDataMgr);
    }

    public void RefreshWatchList() => this.tpoWatchListControl2.RefreshContents();

    private bool isTPOLoan()
    {
      string field = this.session.LoanData.GetField("TPO.X86");
      this.session.LoanData.GetField("TPO.X87");
      return field != string.Empty && field.Equals("Y");
    }

    public void RefreshLogPanel() => this.logPanel.RefreshContents(false);

    public void InitContentsForNewBorrowerPair(bool reloadContents)
    {
      if (this.freeScreen != null)
        this.freeScreen.RefreshContents();
      string name = this.currForm == (InputFormInfo) null ? "" : this.currForm.Name;
      if (reloadContents)
        this.InitContents(false, this.readOnly, true);
      if (!(name != string.Empty))
        return;
      this.OpenForm(name);
    }

    public void InitContents(bool newLoan, bool readOnly)
    {
      this.allFormBox.CheckedChanged -= new EventHandler(this.allFormBox_CheckedChanged);
      this.allFormBox.Checked = false;
      this.allFormBox.CheckedChanged += new EventHandler(this.allFormBox_CheckedChanged);
      if (this.itemizationReadonlyFields != null)
      {
        this.itemizationReadonlyFields.Clear();
        this.itemizationReadonlyFields = (List<string>) null;
      }
      this.InitContents(newLoan, readOnly, false);
    }

    public void InitContents(bool newLoan, bool readOnly, bool changePair)
    {
      this.isLoading = true;
      Button btnOpenWebView = this.btnOpenWebView;
      LoanDataMgr loanMgr = this.loanMgr;
      int num = loanMgr != null ? (!loanMgr.IsNew() ? 1 : 0) : 0;
      btnOpenWebView.Enabled = num != 0;
      if (this.loanLockTool != null)
      {
        this.loanLockTool.Dispose();
        this.loanLockTool = (LoanLockTool) null;
      }
      if (this.freeScreen == null)
      {
        this.freeScreen = new LoanScreen(this.session);
        this.freeScreen.FormChanged += new EventHandler(this.freeScreen_FormChanged);
        this.freeScreen.FormLoaded += new EventHandler(this.freeScreen_FormLoaded);
        this.freeScreen.SetHelpTarget((IOnlineHelpTarget) this);
      }
      EncompassApplication.Session.Loans.FieldDescriptors.Refresh();
      this.noFormBeingShown = true;
      if (this.initialLoad)
        this.initialLoad = false;
      this.readOnly = readOnly;
      this.saveBtn.Enabled = !readOnly;
      this.freeScreen.CurrentLoan = this.loan;
      this.clearWorkArea();
      this.vorPan = (VORPanel) null;
      this.voePan = (VOEPanel) null;
      this.vodPan = (VODPanel) null;
      this.voggPan = (VOGGPanel) null;
      this.vooiPan = (VOOIPanel) null;
      this.vooaPan = (VOOAPanel) null;
      this.voolPan = (VOOLPanel) null;
      this.volPan = (VOLPanel) null;
      this.vomPan = (VOMPanel) null;
      this.tax4506Pan = (TAX4506Panel) null;
      this.tax4506TPan = (TAX4506TPanel) null;
      this.voalPan = (VOALPanel) null;
      this.pnlLoanInfo.AttachToLoan(this.loanMgr);
      if (this.ctlOffers.Enabled)
      {
        this.ctlOffers.AttachToLoan(this.loanMgr);
        this.ctlOffers.SetDisplayMode(LoanOffersControl.DisplayMode.Collapsed);
      }
      this.RefreshBorrowerPairs();
      this.RefreshLoanTeamMembers();
      for (int index = 4000; index <= 4007; ++index)
        this.loan.RegisterFieldValueChangeEventHandler(string.Concat((object) index), new Routine(this.onBorrowerNameChanged));
      this.loan.RegisterFieldValueChangeEventHandler("1172", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("19", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.RegisterFieldValueChangeEventHandler("4084", new Routine(this.refreshPiggybackToolAccessPermission));
      this.loan.RegisterFieldValueChangeEventHandler("HMDA.X27", new Routine(this.loadHMDAForm));
      this.loan.RegisterFieldValueChangeEventHandler("3312", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("1393", new Routine(this.loadHMDAForm));
      this.loan.RegisterFieldValueChangeEventHandler("749", new Routine(this.loadFirstForm));
      this.loan.RegisterFieldValueChangeEventHandler("1825", new Routine(this.refreshVerificationForm));
      this.loan.BorrowerPairCreated -= new EventHandler(this.loan_BorrowerPairCreated);
      this.loan.BorrowerPairCreated += new EventHandler(this.loan_BorrowerPairCreated);
      this.loan.FormVersionChanged -= new EventHandler(this.refreshFormMenuToolList);
      this.loan.FormVersionChanged += new EventHandler(this.refreshFormMenuToolList);
      this.loan.Disclosure2015Created -= new EventHandler(this.refreshDT2015Log);
      this.loan.Disclosure2015Created += new EventHandler(this.refreshDT2015Log);
      this.currForm = (InputFormInfo) null;
      this.emFormMenuBox.ClearFormList();
      this.loadToolList();
      this.Cursor = Cursors.WaitCursor;
      this.logPanel.Initialize();
      this.tpoWatchListControl2.Initialize(this.session);
      if (this.isTPOLoan())
      {
        this.RefreshWatchList();
        this.tpoWatchListControl2.Visible = true;
      }
      else
        this.tpoWatchListControl2.Visible = false;
      if (!changePair)
      {
        this.servicesCtl.InitializeLoan(this.loanMgr);
        this.toolsFormsTabControl.SelectedTab = newLoan || this.loan.IsULDDExporting ? this.formPage : this.servicesPage;
        LogList logList = this.loan.GetLogList();
        bool flag = false;
        if (!flag)
        {
          for (int i = 1; i < logList.GetNumberOfMilestones(); ++i)
          {
            MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
            if (!milestoneAt.Done)
            {
              DateTime dateTime = milestoneAt.Date;
              DateTime date1 = dateTime.Date;
              dateTime = DateTime.MaxValue;
              DateTime date2 = dateTime.Date;
              if (date1 != date2)
              {
                dateTime = milestoneAt.Date;
                DateTime date3 = dateTime.Date;
                dateTime = DateTime.MinValue;
                DateTime date4 = dateTime.Date;
                if (date3 != date4)
                {
                  DateTime today = DateTime.Today;
                  dateTime = milestoneAt.Date;
                  DateTime date5 = dateTime.Date;
                  if (today >= date5)
                    goto label_22;
                }
              }
            }
            if (milestoneAt.Done || milestoneAt.Reviewed || string.Compare(milestoneAt.LoanAssociateID, this.session.UserID, true) != 0)
              continue;
label_22:
            if (i != 1)
            {
              this.AddToWorkArea((Control) new MilestoneWS(this.session, milestoneAt));
              flag = true;
              break;
            }
          }
        }
        if (!flag && this.session.EncompassEdition == EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker)
        {
          if (!flag && this.emToolMenuBox.Items.Contains((object) "Secondary Registration"))
          {
            LockRequestLog[] allLockRequests = this.loan.GetLogList().GetAllLockRequests();
            if (allLockRequests != null && allLockRequests.Length != 0)
            {
              this.loanLockTool = new LoanLockTool(this.session, this.loanMgr);
              this.AddToWorkArea((Control) this.loanLockTool);
              flag = true;
            }
          }
          if (!flag)
          {
            LogRecordBase lastLockAlertLog = this.loan.GetLogList().GetLastLockAlertLog();
            switch (lastLockAlertLog)
            {
              case LockConfirmLog _:
                this.AddToWorkArea((Control) new LockRequestWS(this.session, (LockConfirmLog) lastLockAlertLog));
                flag = true;
                break;
              case LockDenialLog _:
                this.AddToWorkArea((Control) new LockDenialWS(this.session, (LockDenialLog) lastLockAlertLog));
                flag = true;
                break;
              case LockVoidLog _:
                this.AddToWorkArea((Control) new LockVoidWS(this.session, (LockVoidLog) lastLockAlertLog));
                flag = true;
                break;
            }
          }
        }
        this.loanBizInfo = new LoanBusinessRuleInfo(this.loan);
        this.loadFormMenu(flag ? -2 : -1);
        PerformanceMeter.Current.AddCheckpoint("Starting initialization of various services", 1607, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs");
        LoanServiceManager.InitializeLoan(this.session.LoanDataMgr);
        PerformanceMeter.Current.AddCheckpoint("Finished initialization of various services", 1610, nameof (InitContents), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs");
      }
      else
      {
        this.loanBizInfo = new LoanBusinessRuleInfo(this.loan);
        this.loadFormMenu(-1);
      }
      this.pnlLoanInfo.ApplyPRBRRules();
      this.Cursor = Cursors.Default;
      this.isLoading = false;
      if (this.loanMgr != null)
      {
        this.loanMgr.BeforeTriggerRuleApplied -= new EventHandler(this.loanMgr_BeforeTriggerRuleApplied);
        this.loanMgr.BeforeTriggerRuleApplied += new EventHandler(this.loanMgr_BeforeTriggerRuleApplied);
        this.loanMgr.OnLoanDataXmlReplaced -= new EventHandler(this.onLoanDataXmlReplaced);
        this.loanMgr.OnLoanDataXmlReplaced += new EventHandler(this.onLoanDataXmlReplaced);
        this.loanMgr.AfterDDMApplied -= new EventHandler(this.loanMgr_AfterDDMApplied);
        this.loanMgr.AfterDDMApplied += new EventHandler(this.loanMgr_AfterDDMApplied);
      }
      if (this.loan.IsULDDExporting)
      {
        if (this.toolsFormsTabControl.TabPages.Contains(this.servicesPage))
          this.toolsFormsTabControl.TabPages.Remove(this.servicesPage);
        this.logContainerPanel.Visible = false;
        this.OpenForm("ULDD");
      }
      else
      {
        if (this.toolsFormsTabControl.TabPages.Contains(this.servicesPage))
          return;
        this.toolsFormsTabControl.TabPages.Add(this.servicesPage);
      }
    }

    private void freeScreen_FormLoaded(object sender, EventArgs e)
    {
      PerformanceMeter.Get("OpenForm")?.Stop();
    }

    private void onLoanDataXmlReplaced(object sender, EventArgs e)
    {
      this.RefreshBorrowerPairs();
      this.loanBizInfo = new LoanBusinessRuleInfo(this.loan);
      LoanContentAccess loanContentAccess = ((LoanAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanAccess)).GetLoanContentAccess(this.loan);
      if (this.session.LoanDataMgr.AccessRules.AllowFullAccess())
        loanContentAccess = LoanContentAccess.FullAccess;
      this.loan.ContentAccess = loanContentAccess;
      if (loanContentAccess == LoanContentAccess.None)
        this.session.LoanDataMgr.Unlock();
      this.pnlLoanInfo.ResubscribeToEvents();
      this.RefreshLoanContents();
      this.ApplyBusinessRules();
    }

    private void loanMgr_AfterDDMApplied(object sender, EventArgs e)
    {
      if (this.session.LoanDataMgr.SystemConfiguration == null || this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting == null)
        return;
      LOCompensationInputHandler.CheckLOCompRuleConfliction(this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) this.loan, (string) null, (string) null, (string) null, true);
    }

    private void loanMgr_BeforeTriggerRuleApplied(object sender, EventArgs e)
    {
      if (sender == null || this.loan == null)
        return;
      string strB = sender.ToString();
      try
      {
        LogList logList = this.loan.GetLogList();
        MilestoneLog[] allMilestones = logList.GetAllMilestones();
        Hashtable tasksSetup = this.loanMgr.SystemConfiguration.TasksSetup;
        if (tasksSetup == null)
        {
          Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "No task definition in company setting");
        }
        else
        {
          foreach (MilestoneLog msLog in allMilestones)
          {
            BusinessRuleCheck businessRuleCheck = new BusinessRuleCheck();
            businessRuleCheck.HasRequirement(this.loan, msLog);
            if (businessRuleCheck.RequiredTasks != null && businessRuleCheck.RequiredTasks.Length != 0)
            {
              foreach (TaskMilestonePair requiredTask in businessRuleCheck.RequiredTasks)
              {
                if (tasksSetup.ContainsKey((object) requiredTask.TaskGuid))
                {
                  MilestoneTaskDefinition milestoneTaskDefinition = (MilestoneTaskDefinition) tasksSetup[(object) requiredTask.TaskGuid];
                  if (string.Compare(milestoneTaskDefinition.TaskName, strB, true) == 0)
                  {
                    logList.AddRecord((LogRecordBase) new MilestoneTaskLog(this.session.UserInfo, milestoneTaskDefinition.TaskName, milestoneTaskDefinition.TaskDescription)
                    {
                      TaskGUID = milestoneTaskDefinition.TaskGUID,
                      Stage = msLog.Stage,
                      IsRequired = true,
                      TaskPriority = milestoneTaskDefinition.TaskPriority.ToString(),
                      DaysToComplete = milestoneTaskDefinition.DaysToComplete
                    });
                    return;
                  }
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Cannot check task business rule: Error: " + ex.Message);
      }
    }

    private LoanConditions getCurrentLoanConditions()
    {
      MilestoneLog msCheck = (MilestoneLog) null;
      MilestoneLog msToBeFinished = (MilestoneLog) null;
      this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loan.GetLogList());
      return this.loanBizInfo.CurrentLoanForBusinessRule(msCheck, msToBeFinished);
    }

    private LoanConditions getLinkedLoanConditions()
    {
      if (this.loanMgr.LinkedLoan == null || this.loanMgr.LinkedLoan.LoanData == null)
        return (LoanConditions) null;
      MilestoneLog msCheck = (MilestoneLog) null;
      MilestoneLog msToBeFinished = (MilestoneLog) null;
      this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loan.LinkedData.GetLogList());
      return this.loanBizInfo.CurrentLinkedLoanForBusinessRule(msCheck, msToBeFinished);
    }

    public void ApplyBusinessRules()
    {
      try
      {
        using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying all Business Rules..."))
        {
          this.attachConditionMonitors();
          this.ApplyInputFormRules();
          this.ApplyLoanAccessRules();
          this.ApplyFieldAccessRules();
          this.ApplyFieldRules();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Error while attempting to apply business rules: " + (object) ex);
        try
        {
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, this.loan.GUID + " - Input Form/Loan Access/Field Access/Field Rule - LoanPage - Error while attempting to apply business rules: " + (object) ex);
        }
        catch
        {
        }
      }
    }

    public void ApplyOnDemandBusinessRules()
    {
      if (this.loan == null)
        return;
      if (this.loan.BusinessRuleTrigger == BusinessRuleOnDemandEnum.None)
        return;
      try
      {
        using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying Input Form rule, Field Access Rule and Data Entry Rule..."))
        {
          if ((this.loan.BusinessRuleTrigger & BusinessRuleOnDemandEnum.InputFormRule) == BusinessRuleOnDemandEnum.InputFormRule)
            this.ApplyInputFormRules();
          if ((this.loan.BusinessRuleTrigger & BusinessRuleOnDemandEnum.FieldAccessRule) == BusinessRuleOnDemandEnum.FieldAccessRule)
            this.ApplyFieldAccessRules();
          if ((this.loan.BusinessRuleTrigger & BusinessRuleOnDemandEnum.DataEntryRule) == BusinessRuleOnDemandEnum.DataEntryRule)
            this.ApplyFieldRules();
          this.loan.BusinessRuleTrigger = BusinessRuleOnDemandEnum.None;
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Error while attempting to apply one of following business rules: Input Form rule, Field Access Rule and Data Entry rule. Error: " + (object) ex);
        try
        {
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, this.loan.GUID + " - Input Form/Field Access/Field Rule - LoanPage - Error while attempting to apply business rules: " + (object) ex);
        }
        catch
        {
        }
      }
    }

    public void ApplyFieldRules()
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return;
      using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying Field Rules..."))
      {
        MilestoneLog msCheck = (MilestoneLog) null;
        MilestoneLog msToBeFinished = (MilestoneLog) null;
        this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loan.GetLogList());
        LoanConditions loanConditions = this.loanBizInfo.CurrentLoanForBusinessRule(msCheck, msToBeFinished);
        FieldMilestonePair[] requiredFields = new BusinessRuleCheck(this.loan, msToBeFinished).RequiredFields;
        FieldRulesBpmManager bpmManager = (FieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules);
        Hashtable allFieldRules = bpmManager.GetAllFieldRules(loanConditions, this.loan);
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Getting Required Field Rule...");
        Hashtable allRequiredFields = bpmManager.GetAllRequiredFields(loanConditions, this.loan);
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Attaching Field Rule...");
        this.session.LoanDataMgr.AttachFieldRules(allFieldRules, allRequiredFields, requiredFields);
      }
    }

    public void ApplyFieldAccessRules()
    {
      if (this.session.UserInfo.IsSuperAdministrator())
      {
        this.aiqHelper.EnableAIQLaunchButton(this.loanMgr, true);
      }
      else
      {
        using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying Field Access Rules..."))
        {
          if (this.accessRuleManager == null)
            this.accessRuleManager = (FieldAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldAccess);
          Hashtable fieldRights = this.accessRuleManager.GetFieldAccessRight(this.session.UserInfo.UserPersonas, this.getCurrentLoanConditions(), this.loan);
          if (this.AIQAnalyzersAuthorized())
          {
            this.aiqAnalyzers.Visible = true;
            this.aiqAnalyzers.Enabled = true;
            if (fieldRights != null && fieldRights.Contains((object) "BUTTON_AIQANALYZERS"))
            {
              if ((BizRule.FieldAccessRight) fieldRights[(object) "BUTTON_AIQANALYZERS"] == BizRule.FieldAccessRight.Hide)
                this.aiqAnalyzers.Visible = false;
              else if ((BizRule.FieldAccessRight) fieldRights[(object) "BUTTON_AIQANALYZERS"] == BizRule.FieldAccessRight.ViewOnly)
                this.aiqAnalyzers.Enabled = false;
              else
                this.aiqHelper.EnableAIQLaunchButton(this.loanMgr, true);
            }
            else
              this.aiqHelper.EnableAIQLaunchButton(this.loanMgr, true);
          }
          else
          {
            this.aiqAnalyzers.Visible = false;
            this.aiqAnalyzers.Enabled = false;
          }
          if (this.itemizationReadonlyFields == null)
            this.loadItemizationReadOnlyFields();
          if (this.itemizationReadonlyFields != null && this.itemizationReadonlyFields.Count > 0)
          {
            if (fieldRights == null)
              fieldRights = CollectionsUtil.CreateCaseInsensitiveHashtable();
            foreach (string itemizationReadonlyField in this.itemizationReadonlyFields)
            {
              if (!fieldRights.ContainsKey((object) itemizationReadonlyField))
                fieldRights.Add((object) itemizationReadonlyField, (object) BizRule.FieldAccessRight.ViewOnly);
              else if ((BizRule.FieldAccessRight) fieldRights[(object) itemizationReadonlyField] == BizRule.FieldAccessRight.Edit)
                fieldRights[(object) itemizationReadonlyField] = (object) BizRule.FieldAccessRight.ViewOnly;
            }
          }
          Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Attaching Field Access Rights to LoanDataMgr...");
          this.session.LoanDataMgr.AttachFieldAccessRights(fieldRights);
          if (this.loanMgr.LinkedLoan == null)
            return;
          LoanConditions linkedLoanConditions = this.getLinkedLoanConditions();
          if (linkedLoanConditions == null)
            return;
          Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Attaching Field Access Rule for Linked Loan...");
          this.loanMgr.LinkedLoan.AttachFieldAccessRights(this.accessRuleManager.GetFieldAccessRight(this.session.UserInfo.UserPersonas, linkedLoanConditions, this.loan.LinkedData));
        }
      }
    }

    private void loadItemizationReadOnlyFields()
    {
      Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Retrieving Persona Itemization Fee Management Access Permission Setting...");
      this.itemizationReadonlyFields = new List<string>();
      for (int index1 = 1; index1 <= 14; ++index1)
      {
        if ((index1 != 1 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BorrowerAmountOnly) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeAmount)) && (index1 != 2 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BorrwerCanShopFor) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 3 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BorrwerDidShopFor) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 4 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BrokerAmount) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeAmount)) && (index1 != 5 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_Optional1310) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 6 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_PaidToName)) && (index1 != 7 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_PaidToType)) && (index1 != 8 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_PropertyTaxes1007) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 9 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_PropertyTaxes907) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 10 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_SellerAmount) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeAmount)) && (index1 != 11 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_ImpactAPR) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 12 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_Escrowed) || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeOptions)) && (index1 != 13 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_FeeDescription)) && (index1 != 14 || !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BorrwerCanShopFor) && !this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ItemizationFee_BorrwerDidShopFor)))
        {
          switch (index1)
          {
            case 13:
              using (List<GFEItem>.Enumerator enumerator = GFEItemCollection.GFEItems2010.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  GFEItem current = enumerator.Current;
                  if (current.Description.StartsWith("NEWHUD") || current.Description.Length <= 4)
                    this.itemizationReadonlyFields.Add(current.Description);
                }
                continue;
              }
            case 14:
              this.itemizationReadonlyFields.Add("NEWHUD.X651");
              for (int index2 = 597; index2 <= 601; ++index2)
                this.itemizationReadonlyFields.Add("NEWHUD.X" + (object) index2);
              this.itemizationReadonlyFields.Add("NEWHUD.X957");
              this.itemizationReadonlyFields.Add("NEWHUD.X966");
              this.itemizationReadonlyFields.Add("NEWHUD.X975");
              this.itemizationReadonlyFields.Add("NEWHUD.X984");
              this.itemizationReadonlyFields.Add("NEWHUD.X993");
              this.itemizationReadonlyFields.Add("NEWHUD.X1002");
              for (int index3 = 96; index3 <= 103; ++index3)
                this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index3);
              this.itemizationReadonlyFields.Add("NEWHUD.X107");
              this.itemizationReadonlyFields.Add("NEWHUD.X573");
              for (int index4 = 576; index4 <= 581; ++index4)
                this.itemizationReadonlyFields.Add("NEWHUD.X" + (object) index4);
              for (int index5 = 108; index5 <= 115; ++index5)
                this.itemizationReadonlyFields.Add("NEWHUD.X" + (object) index5);
              continue;
            default:
              if (index1 >= 1 && index1 <= 4 || index1 >= 6 && index1 <= 7 || index1 >= 10 && index1 <= 11)
              {
                foreach (string[] strArray in HUDGFE2010Fields.WHOLEPOC_FIELDS)
                {
                  switch (index1)
                  {
                    case 1:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_BORPAID]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPACAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPOCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORPTCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORAMTPAID]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORFINANCEDAMT]);
                        continue;
                      }
                      continue;
                    case 2:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORCANSHOP]);
                        continue;
                      }
                      continue;
                    case 3:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BORDIDSHOP]);
                        continue;
                      }
                      continue;
                    case 4:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPACAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015BRKPOCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPACAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015LENDERPOCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPACAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015OTHPOCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDBY]);
                        continue;
                      }
                      continue;
                    case 6:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTONAME]);
                        continue;
                      }
                      continue;
                    case 7:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_PAIDTO]);
                        continue;
                      }
                      continue;
                    case 10:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_SELPAID] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_SELPAID]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPACAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELPOCAMT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELCREDIT]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED] != "")
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATED]);
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_2015SELOBLIGATEDAMT]);
                        continue;
                      }
                      continue;
                    case 11:
                      if (strArray[HUDGFE2010Fields.PTCPOCINDEX_APR] != "")
                      {
                        this.itemizationReadonlyFields.Add(strArray[HUDGFE2010Fields.PTCPOCINDEX_APR]);
                        continue;
                      }
                      continue;
                    default:
                      continue;
                  }
                }
              }
              switch (index1)
              {
                case 1:
                  this.itemizationReadonlyFields.Add("L211");
                  this.itemizationReadonlyFields.Add("L213");
                  this.itemizationReadonlyFields.Add("388");
                  this.itemizationReadonlyFields.Add("389");
                  this.itemizationReadonlyFields.Add("1620");
                  this.itemizationReadonlyFields.Add("NEWHUD.X223");
                  this.itemizationReadonlyFields.Add("NEWHUD.X224");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1141");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1225");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1143");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1226");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1150");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1227");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1154");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1158");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1162");
                  this.itemizationReadonlyFields.Add("NEWHUD2.X5");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1146");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1148");
                  this.itemizationReadonlyFields.Add("NEWHUD.X640");
                  this.itemizationReadonlyFields.Add("NEWHUD.X641");
                  this.itemizationReadonlyFields.Add("332");
                  this.itemizationReadonlyFields.Add("L244");
                  this.itemizationReadonlyFields.Add("L245");
                  this.itemizationReadonlyFields.Add("1296");
                  this.itemizationReadonlyFields.Add("232");
                  this.itemizationReadonlyFields.Add("L251");
                  this.itemizationReadonlyFields.Add("230");
                  this.itemizationReadonlyFields.Add("1322");
                  for (int index6 = 4397; index6 <= 4411; index6 += 2)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index6);
                  for (int index7 = 4400; index7 <= 4412; index7 += 2)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index7);
                  this.itemizationReadonlyFields.Add("1750");
                  this.itemizationReadonlyFields.Add("1751");
                  this.itemizationReadonlyFields.Add("1752");
                  this.itemizationReadonlyFields.Add("231");
                  this.itemizationReadonlyFields.Add("1387");
                  this.itemizationReadonlyFields.Add("1296");
                  this.itemizationReadonlyFields.Add("232");
                  this.itemizationReadonlyFields.Add("1386");
                  this.itemizationReadonlyFields.Add("1752");
                  this.itemizationReadonlyFields.Add("L267");
                  this.itemizationReadonlyFields.Add("L268");
                  this.itemizationReadonlyFields.Add("1388");
                  this.itemizationReadonlyFields.Add("235");
                  this.itemizationReadonlyFields.Add("1629");
                  this.itemizationReadonlyFields.Add("1630");
                  this.itemizationReadonlyFields.Add("340");
                  this.itemizationReadonlyFields.Add("253");
                  this.itemizationReadonlyFields.Add("341");
                  this.itemizationReadonlyFields.Add("254");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1706");
                  this.itemizationReadonlyFields.Add("NEWHUD.X1707");
                  this.itemizationReadonlyFields.Add("558");
                  this.itemizationReadonlyFields.Add("2402");
                  this.itemizationReadonlyFields.Add("2403");
                  this.itemizationReadonlyFields.Add("2404");
                  this.itemizationReadonlyFields.Add("2405");
                  this.itemizationReadonlyFields.Add("2406");
                  this.itemizationReadonlyFields.Add("2407");
                  this.itemizationReadonlyFields.Add("2408");
                  continue;
                case 5:
                  for (int index8 = 4196; index8 <= 4361; index8 += 33)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index8);
                  for (int index9 = 4447; index9 <= 4579; index9 += 33)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index9);
                  continue;
                case 6:
                  this.itemizationReadonlyFields.Add("NEWHUD.X206");
                  this.itemizationReadonlyFields.Add("NEWHUD.X207");
                  this.itemizationReadonlyFields.Add("646");
                  this.itemizationReadonlyFields.Add("1634");
                  continue;
                case 7:
                  for (int index10 = 1170; index10 <= 1174; index10 += 2)
                    this.itemizationReadonlyFields.Add("NEWHUD.X" + (object) index10);
                  continue;
                case 8:
                  for (int index11 = 124; index11 <= 132; ++index11)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index11);
                  continue;
                case 9:
                  for (int index12 = 4435; index12 <= 4440; ++index12)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index12);
                  for (int index13 = 4415; index13 <= 4426; ++index13)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index13);
                  continue;
                case 10:
                  this.itemizationReadonlyFields.Add("NEWHUD2.X6");
                  continue;
                case 12:
                  for (int index14 = 133; index14 <= 140; ++index14)
                    this.itemizationReadonlyFields.Add("NEWHUD2.X" + (object) index14);
                  continue;
                default:
                  continue;
              }
          }
        }
      }
    }

    public void ApplyLoanAccessRules()
    {
      if (this.session.UserInfo.IsSuperAdministrator())
        return;
      using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying Loan Access Rules..."))
      {
        if ((this.loan.ContentAccess & LoanContentAccess.FormFields) != LoanContentAccess.FormFields && (this.loanMgr.LinkedLoan == null || (this.loanMgr.LinkedLoan.LoanData.ContentAccess & LoanContentAccess.FormFields) != LoanContentAccess.FormFields))
          return;
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Getting Editable Fields...");
        LoanConditions currentLoanConditions = this.getCurrentLoanConditions();
        LoanAccessBpmManager bpmManager = (LoanAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.LoanAccess);
        Hashtable loanAccessFields = bpmManager.GetLoanAccessFields(this.session.UserInfo.UserPersonas, currentLoanConditions, this.loan);
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Attaching Editable Fields...");
        this.session.LoanDataMgr.AttachEditableFields(loanAccessFields);
        if (this.loanMgr.LinkedLoan == null || this.loanMgr.LinkedLoan.LoanData == null)
          return;
        LoanConditions linkedLoanConditions = this.getLinkedLoanConditions();
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Attaching Editable Fields for Linked Loan...");
        this.loanMgr.LinkedLoan.AttachEditableFields(bpmManager.GetLoanAccessFields(this.session.UserInfo.UserPersonas, linkedLoanConditions, this.loanMgr.LinkedLoan.LoanData));
      }
    }

    public void ApplyInputFormRules()
    {
      using (Tracing.StartTimer(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Applying Input Form Rules..."))
      {
        MilestoneLog msCheck = (MilestoneLog) null;
        MilestoneLog msToBeFinished = (MilestoneLog) null;
        this.getLoanMilestoneStatus(ref msCheck, ref msToBeFinished, this.loan.GetLogList());
        LoanConditions loanConditions = this.loanBizInfo.CurrentLoanForBusinessRule(msCheck, msToBeFinished);
        ArrayList arrayList = new ArrayList();
        string[] forms = ((InputFormsBpmManager) this.session.BPM.GetBpmManager(BpmCategory.InputForms)).GetForms(loanConditions, this.loan);
        if (forms != null)
        {
          if (this.allForms == null)
            this.allForms = this.loanMgr.InputFormSettings.GetFormList("All");
          Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
          for (int index = 0; index < forms.Length; ++index)
            dictionary[forms[index]] = true;
          for (int index = 0; index < this.allForms.Length; ++index)
          {
            if (dictionary.ContainsKey(this.allForms[index].FormID))
              arrayList.Add((object) this.allForms[index].Name);
          }
        }
        if (arrayList != null && arrayList.Contains((object) "203k Max Mortgage WS"))
        {
          if (!arrayList.Contains((object) "FHA Management"))
          {
            for (int index = 0; index < arrayList.Count; ++index)
            {
              if (arrayList[index].ToString() == "203k Max Mortgage WS")
              {
                arrayList[index] = (object) "FHA Management";
                break;
              }
            }
          }
          else
            arrayList.Remove((object) "203k Max Mortgage WS");
        }
        if (arrayList == null || arrayList.Count <= 0)
          return;
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Appending new form menu...");
        this.emFormMenuBox.AppendForms((string[]) arrayList.ToArray(typeof (string)), true);
      }
    }

    private void getLoanMilestoneStatus(
      ref MilestoneLog msCheck,
      ref MilestoneLog msToBeFinished,
      LogList logList)
    {
      MilestoneLog[] allMilestones = logList.GetAllMilestones();
      for (int index = 0; index < allMilestones.Length && allMilestones[index].Done; ++index)
      {
        if (index == allMilestones.Length - 1)
        {
          msCheck = allMilestones[index];
          msToBeFinished = msCheck;
        }
        else if (allMilestones[index + 1].RoleID < RoleInfo.FileStarter.ID || allMilestones[index + 1].LoanAssociateID != "")
        {
          msCheck = allMilestones[index];
          msToBeFinished = allMilestones[index + 1];
        }
      }
      msToBeFinished = logList.GetCurrentMilestone();
      if (msToBeFinished != null)
        return;
      msToBeFinished = msCheck;
    }

    private void clearConditionMonitors()
    {
      if (this.formRuleConditionMonitor != null)
      {
        this.formRuleConditionMonitor.ActiveStateChanged -= new EventHandler(this.onInputFormRuleConditionStateChanged);
        this.formRuleConditionMonitor.Dispose();
        this.formRuleConditionMonitor = (LoanConditionGroupMonitor) null;
      }
      if (this.fieldAccessConditionMonitor != null)
      {
        this.fieldAccessConditionMonitor.ActiveStateChanged -= new EventHandler(this.onFieldAccessRuleConditionStateChanged);
        this.fieldAccessConditionMonitor.Dispose();
        this.fieldAccessConditionMonitor = (LoanConditionGroupMonitor) null;
      }
      if (this.fieldRuleConditionMonitor == null)
        return;
      this.fieldRuleConditionMonitor.ActiveStateChanged -= new EventHandler(this.onFieldRuleConditionStateChanged);
      this.fieldRuleConditionMonitor.Dispose();
      this.fieldRuleConditionMonitor = (LoanConditionGroupMonitor) null;
    }

    private void attachConditionMonitors()
    {
      this.clearConditionMonitors();
      EllieMae.EMLite.Customization.ExecutionContext context = new EllieMae.EMLite.Customization.ExecutionContext(this.session.UserInfo, this.loan, (IServerDataProvider) new CustomCodeSessionDataProvider(this.session.SessionObjects));
      if (!this.session.UserInfo.IsSuperAdministrator())
      {
        FieldAccessBpmManager bpmManager1 = (FieldAccessBpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldAccess);
        this.fieldAccessConditionMonitor = new LoanConditionGroupMonitor(context);
        this.fieldAccessConditionMonitor.AddRange((IEnumerable<ConditionEvaluator>) bpmManager1.GetConditionEvaluators());
        this.fieldAccessConditionMonitor.ActiveStateChanged += new EventHandler(this.onFieldAccessRuleConditionStateChanged);
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Successfully attached field access rule condition monitor.");
        MilestoneRulesBpmManager bpmManager2 = (MilestoneRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.MilestoneRules);
        FieldRulesBpmManager bpmManager3 = (FieldRulesBpmManager) this.session.BPM.GetBpmManager(BpmCategory.FieldRules);
        this.fieldRuleConditionMonitor = new LoanConditionGroupMonitor(context);
        this.fieldRuleConditionMonitor.AddRange((IEnumerable<ConditionEvaluator>) bpmManager3.GetConditionEvaluators());
        this.fieldRuleConditionMonitor.AddRange((IEnumerable<ConditionEvaluator>) bpmManager2.GetConditionEvaluators());
        this.fieldRuleConditionMonitor.ActiveStateChanged += new EventHandler(this.onFieldRuleConditionStateChanged);
        Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Successfully attached field rule condition monitor.");
      }
      InputFormsBpmManager bpmManager = (InputFormsBpmManager) this.session.BPM.GetBpmManager(BpmCategory.InputForms);
      this.formRuleConditionMonitor = new LoanConditionGroupMonitor(context);
      this.formRuleConditionMonitor.AddRange((IEnumerable<ConditionEvaluator>) bpmManager.GetConditionEvaluators());
      this.formRuleConditionMonitor.ActiveStateChanged += new EventHandler(this.onInputFormRuleConditionStateChanged);
      Tracing.Log(LoanPage.sw, TraceLevel.Info, nameof (LoanPage), "Successfully attached input form condition monitor.");
    }

    private void onInputFormRuleConditionStateChanged(object sender, EventArgs e)
    {
      this.loan.SetBusinessRuleTriggerEnum(BusinessRuleOnDemandEnum.InputFormRule, true);
    }

    private void onFieldAccessRuleConditionStateChanged(object sender, EventArgs e)
    {
      this.loan.SetBusinessRuleTriggerEnum(BusinessRuleOnDemandEnum.FieldAccessRule, true);
      this.loan.SetBusinessRuleTriggerEnum(BusinessRuleOnDemandEnum.DataEntryRule, true);
    }

    private void onFieldRuleConditionStateChanged(object sender, EventArgs e)
    {
      this.loan.SetBusinessRuleTriggerEnum(BusinessRuleOnDemandEnum.DataEntryRule, true);
    }

    public void SetMilestoneStatus(MilestoneLog milestoneLog, int milestoneIndex, bool finished)
    {
      LogList logList = this.loan.GetLogList();
      if (finished)
      {
        DateTime newDate = DateTime.Now;
        for (int i = 0; i < milestoneIndex; ++i)
        {
          MilestoneLog milestoneAt = logList.GetMilestoneAt(i);
          if (milestoneAt != null && milestoneAt.Done && milestoneAt.Date > newDate)
            newDate = milestoneAt.Date;
        }
        milestoneLog.AdjustDate(newDate, true, true);
      }
      this.loanMgr.SetMilestoneStatus(milestoneLog.MilestoneID, finished);
      if (!finished)
      {
        this.ApplyBusinessRules();
        EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        this.RefreshLoanTeamMembers();
      }
      else
      {
        TriggerImplDef def = this.loanMgr.ApplyLoanTemplateTrigger(TriggerConditionType.MilestoneCompleted, milestoneLog.MilestoneID);
        if (def != null)
        {
          this.showApplyLoanTemplateProgress();
          this.session.LoanDataMgr.ApplyLoanTemplate(def);
          this.closeProgress();
        }
        MilestoneLog currentMilestone = logList.GetCurrentMilestone();
        if (currentMilestone != null && currentMilestone.LoanAssociateType == EllieMae.EMLite.DataEngine.Log.LoanAssociateType.User && currentMilestone.LoanAssociateID == this.session.UserID)
          currentMilestone.Reviewed = true;
        this.ApplyBusinessRules();
        EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
        this.RefreshLoanTeamMembers();
        this.RefreshContents();
      }
    }

    public void RefreshLoanTeamMembers() => this.pnlLoanInfo.RefreshContents();

    private void loadHMDAForm(string id, string val)
    {
      if (!(this.currForm.Name == "HMDA Information"))
        return;
      this.openFormInEditor(Utils.ParseInt((object) this.loan.GetSimpleField("HMDA.X27")) >= 2017 || this.loan.GetSimpleField("1825") != "Y" ? new InputFormInfo("HMDA_DENIAL04", "HMDA Information") : new InputFormInfo("HMDA_DENIAL", "HMDA Information"), (Control) null);
    }

    private void refreshVerificationForm(string id, string val)
    {
      if (!(this.currForm.Name == "VOD") && !(this.currForm.Name == "VOE"))
        return;
      this.openFormInEditor(new InputFormInfo(this.currForm.Name, this.currForm.Name), (Control) null);
    }

    private void loadFirstForm(string id, string val) => this.loadFormMenu(0);

    private void refreshPiggybackToolAccessPermission(string id, string val) => this.loadToolList();

    private void enforceSecurity()
    {
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.LoansTab_Print_PrintButton))
        this.printBtn.Enabled = false;
      if (this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_Other_ManualExclusiveLock))
        this.canExclusiveLock = true;
      else
        this.canExclusiveLock = false;
    }

    private void applyLOCompToolAccessRule(string id, string val)
    {
      if (this.session.EncompassEdition != EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker || this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LOCompOfficerTool) || this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LOCompBrokerTool))
        return;
      this.emToolMenuBox.RemoveForm("LO Compensation");
    }

    private void applyEncompassEditionBehaviors()
    {
      ArrayList arrayList = new ArrayList((ICollection) this.toolsList);
      if (this.session.EncompassEdition != EllieMae.EMLite.Common.Licensing.EncompassEdition.Banker)
      {
        foreach (string bankerOnlyTool in SecurityUtil.BankerOnlyTools(this.session.MainScreen.IsUnderwriterSummaryAccessibleForBroker))
          arrayList.Remove((object) bankerOnlyTool);
      }
      this.toolsList = (string[]) arrayList.ToArray(typeof (string));
    }

    private void loadToolList()
    {
      bool flag1 = true;
      if (UserInfo.IsAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
        flag1 = false;
      bool flag2 = this.session.EncompassEdition != EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker;
      this.emToolMenuBox.LoadFormList(this.toolsList);
      if (!this.session.StartupInfo.ShowUIWorkflowTasksTools || this.session.EncompassEdition == EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker)
        this.emToolMenuBox.RemoveForm("Workflow Tasks");
      if (!LoanServiceManager.ComplianceReviewInstalled)
        this.emToolMenuBox.RemoveForm("Compliance Review");
      if (!LoanServiceManager.TQLClientInstalled)
        this.emToolMenuBox.RemoveForm("TQL Services");
      if (this.loan.IsULDDExporting)
      {
        this.emToolMenuBox.RemoveForm("Business Contacts");
        this.emToolMenuBox.RemoveForm("Compliance Review");
        this.emToolMenuBox.RemoveForm("TQL Services");
        this.emToolMenuBox.RemoveForm("Status Online");
        this.emToolMenuBox.RemoveForm("Piggyback Loans");
        this.emToolMenuBox.RemoveForm("Secure Form Transfer");
        this.emToolMenuBox.RemoveForm("Audit Trail");
      }
      if (!Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
        this.emToolMenuBox.RemoveForm("Fee Variance Worksheet");
      if (flag1)
      {
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.LOConnectTab_WorkflowTasksTool))
          this.emToolMenuBox.RemoveForm("Workflow Tasks");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_BizContacts))
          this.emToolMenuBox.RemoveForm("Business Contacts");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FileContacts))
          this.emToolMenuBox.RemoveForm("File Contacts");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ConversationLog) || (this.loan.ContentAccess & LoanContentAccess.ConversationLog) != LoanContentAccess.ConversationLog && (this.loan.ContentAccess & LoanContentAccess.ConversationLogViewOnly) != LoanContentAccess.ConversationLogViewOnly)
          this.emToolMenuBox.RemoveForm("Conversation Log");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Task) || (this.loan.ContentAccess & LoanContentAccess.Task) != LoanContentAccess.Task && (this.loan.ContentAccess & LoanContentAccess.TaskViewOnly) != LoanContentAccess.TaskViewOnly && this.loan.ContentAccess != LoanContentAccess.FullAccess)
          this.emToolMenuBox.RemoveForm("Tasks");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_AUSTracking))
          this.emToolMenuBox.RemoveForm("AUS Tracking");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_RepAndWarrantTracker))
          this.emToolMenuBox.RemoveForm("Rep and Warrant Tracker");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_VerificationTracking))
          this.emToolMenuBox.RemoveForm("Verification and Documentation Tracking");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking) || (this.loan.ContentAccess & LoanContentAccess.DisclosureTracking) != LoanContentAccess.DisclosureTracking && (this.loan.ContentAccess & LoanContentAccess.DisclosureTrackingViewOnly) != LoanContentAccess.DisclosureTrackingViewOnly && this.loan.ContentAccess != LoanContentAccess.FullAccess)
          this.emToolMenuBox.RemoveForm("Disclosure Tracking");
        this.applyLOCompToolAccessRule((string) null, (string) null);
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FeeToleranceWorksheet) || !Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
          this.emToolMenuBox.RemoveForm("Fee Variance Worksheet");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_PiggybackLoans))
          this.emToolMenuBox.RemoveForm("Piggyback Loans");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_SecureFormTransfer))
          this.emToolMenuBox.RemoveForm("Secure Form Transfer");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FundingWS))
          this.emToolMenuBox.RemoveForm("Funding Worksheet");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_BrokerCheckCal))
          this.emToolMenuBox.RemoveForm("Broker Check Calculation");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_FundingBalWS))
          this.emToolMenuBox.RemoveForm("Funding Balancing Worksheet");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LockRequestForm))
          this.emToolMenuBox.RemoveForm("Lock Request Form");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_SecondaryRegistration))
        {
          this.emToolMenuBox.RemoveForm("Secondary Registration");
          this.emToolMenuBox.RemoveForm("Worst Case Pricing");
        }
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_PurchaseAdviceForm))
          this.emToolMenuBox.RemoveForm("Purchase Advice Form");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_CorrespondentPurchaseAdviceForm))
          this.emToolMenuBox.RemoveForm("Correspondent Purchase Advice Form");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_TPOWebCenterLoanInformation))
          this.emToolMenuBox.RemoveForm("TPO Information");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_CorrespondentLoanStatus))
          this.emToolMenuBox.RemoveForm("Correspondent Loan Status");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ShippingDetail))
          this.emToolMenuBox.RemoveForm("Shipping Detail");
        if (EllieMae.EMLite.RemotingServices.Session.IsBankerEdition() && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DocumentTracking))
          this.emToolMenuBox.RemoveForm("Collateral Tracking");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_InterimServicing))
          this.emToolMenuBox.RemoveForm("Interim Servicing Worksheet");
        if (EllieMae.EMLite.RemotingServices.Session.IsBankerEdition() && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_UnderwriterSummary))
          this.emToolMenuBox.RemoveForm("Underwriter Summary");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Prequal))
          this.emToolMenuBox.RemoveForm("Prequalification");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DebtConsolidation))
          this.emToolMenuBox.RemoveForm("Debt Consolidation");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LoanComparison))
          this.emToolMenuBox.RemoveForm("Loan Comparison");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_CashToClose))
          this.emToolMenuBox.RemoveForm("Cash-to-Close");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_RentOwn))
          this.emToolMenuBox.RemoveForm("Rent vs. Own");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ProfitMngt) || (this.loan.ContentAccess & LoanContentAccess.ProfitManagement) != LoanContentAccess.ProfitManagement && (this.loan.ContentAccess & LoanContentAccess.ProfitMgmtViewOnly) != LoanContentAccess.ProfitMgmtViewOnly && this.loan.ContentAccess != LoanContentAccess.FullAccess)
          this.emToolMenuBox.RemoveForm("Profit Management");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_TrustAccount))
          this.emToolMenuBox.RemoveForm("Trust Account");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_AuditTrail))
          this.emToolMenuBox.RemoveForm("Audit Trail");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Amortization))
          this.emToolMenuBox.RemoveForm("Amortization Schedule");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_TQLTool) & flag2)
          this.emToolMenuBox.RemoveForm("TQL Services");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_SafeHarborDisclosure))
          this.emToolMenuBox.RemoveForm("Anti-Steering Safe Harbor Disclosure");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ECSDataViewer))
          this.emToolMenuBox.RemoveForm("ECS Data Viewer");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_NetTangibleBenefit))
          this.emToolMenuBox.RemoveForm("Net Tangible Benefit");
        if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_ProjectReview))
          this.emToolMenuBox.RemoveForm("Project Review");
        if (flag2 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_MiCenter))
          this.emToolMenuBox.RemoveForm("MI Center");
      }
      if (this.session.SessionObjects.StartupInfo.ProductPricingPartner == null || this.session.SessionObjects.StartupInfo.ProductPricingPartner.PartnerID != "MPS" || !ProductPricingUtils.IsHistoricalPricingEnabled)
        this.emToolMenuBox.RemoveForm("Worst Case Pricing");
      if (this.loan != null && (this.loan.GetField("19") == "ConstructionOnly" && this.loan.GetField("4084") == "Y" || this.loan.LinkedData != null && this.loan.LinkedData.GetField("19") == "ConstructionOnly" && this.loan.LinkedData.GetField("4084") == "Y"))
        this.emToolMenuBox.RemoveForm("Piggyback Loans");
      if (flag1 && !this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_LockComparisonTool))
        this.emToolMenuBox.RemoveForm("Lock Comparison Tool");
      this.emToolMenuBox.RefreshFormList();
    }

    private void rebuildFormList()
    {
      if (this.loanMgr == null)
        return;
      InputFormInfo[] formList = this.loanMgr.InputFormSettings.GetFormList("All");
      ArrayList arrayList1 = new ArrayList();
      foreach (InputFormInfo inputFormInfo in formList)
        arrayList1.Add((object) inputFormInfo.Name);
      InputFormInfo[] inputFormInfoArray = new InputFormInfo[0];
      bool flag1 = false;
      this.defaultForm = (string) null;
      if (this.allFormBox.Checked)
      {
        flag1 = true;
      }
      else
      {
        string[] formListTemplate = this.loan.GetFormListTemplate();
        if (formListTemplate.Length != 0)
        {
          ArrayList arrayList2 = new ArrayList();
          for (int index = 0; index < formListTemplate.Length; ++index)
          {
            InputFormInfo inputFormInfo = !(formListTemplate[index] == "-") ? this.loanMgr.InputFormSettings.GetFormByName(formListTemplate[index]) : new InputFormInfo("-");
            if (formListTemplate[index] == "-")
              arrayList2.Add((object) inputFormInfo);
            else if (inputFormInfo != (InputFormInfo) null && arrayList1.Contains((object) inputFormInfo.Name))
              arrayList2.Add((object) inputFormInfo);
          }
          inputFormInfoArray = (InputFormInfo[]) arrayList2.ToArray(typeof (InputFormInfo));
        }
        else
        {
          inputFormInfoArray = this.loanMgr.InputFormSettings.GetDefaultFormList();
          if (inputFormInfoArray.Length == 0)
            flag1 = true;
        }
        if (inputFormInfoArray.Length != 0)
          this.defaultForm = inputFormInfoArray[0].Name;
      }
      if (flag1)
      {
        inputFormInfoArray = this.loanMgr.InputFormSettings.GetFormList("All");
        if (this.defaultForm == null && inputFormInfoArray.Length != 0)
          this.defaultForm = inputFormInfoArray[0].Name;
      }
      string field = this.session.LoanDataMgr.LoanData.GetField("3969");
      bool flag2 = this.session.LoanDataMgr.LoanData.GetField("1825") == "2020";
      string[] source1 = new string[5]
      {
        "GFE - Itemization",
        "HUD-1 Page 1",
        "HUD-1 Page 2",
        "REGZ - TIL",
        "Closing RegZ"
      };
      string[] source2 = new string[7]
      {
        "2010 GFE",
        "2010 Itemization",
        "2010 HUD-1 Page 1",
        "2010 HUD-1 Page 2",
        "2010 HUD-1 Page 3",
        "REGZ - TIL",
        "Closing RegZ"
      };
      string[] source3 = new string[11]
      {
        "2015 Itemization",
        "RegZ - LE",
        "Loan Estimate Page 1",
        "Loan Estimate Page 2",
        "Loan Estimate Page 3",
        "RegZ - CD",
        "Closing Disclosure Page 1",
        "Closing Disclosure Page 2",
        "Closing Disclosure Page 3",
        "Closing Disclosure Page 4",
        "Closing Disclosure Page 5"
      };
      string[] strArray1 = new string[5]
      {
        "1003 Page 1",
        "1003 Page 2",
        "1003 Page 3",
        "1003 Page 4",
        "FNMA Streamlined 1003"
      };
      string[] strArray2 = new string[12]
      {
        "1003 URLA Part 1",
        "1003 URLA Part 2",
        "1003 URLA Part 3",
        "1003 URLA Part 4",
        "1003 URLA - Lender",
        "1003 URLA Continuation",
        "Verification of Other Income",
        "Verification of Gifts and Grants",
        "Verification of Other Liability",
        "Verification of Other Assets",
        "Verification of Additional Loans",
        "Fannie Mae Additional Data"
      };
      List<string> stringList = new List<string>();
      string clientId = EllieMae.EMLite.RemotingServices.Session.CompanyInfo.ClientID;
      string empty1 = string.Empty;
      foreach (InputFormInfo inputFormInfo in inputFormInfoArray)
      {
        if (!(inputFormInfo == (InputFormInfo) null) && (!(inputFormInfo.FormID == "ULDD") || EllieMae.EMLite.RemotingServices.Session.MainScreen.IsClientEnabledToExportFNMFRE) && !InputFormInfo.IsChildForm(inputFormInfo.FormID))
        {
          string name = inputFormInfo.Name;
          if (name != null)
          {
            if (inputFormInfo.Name.ToLower() == "gfe - itemization" && field == "RESPA 2010 GFE and HUD-1" && !stringList.Contains("2010 GFE") && arrayList1.Contains((object) "REGZGFEHUD"))
              stringList.Add("2010 GFE");
            if (name == "-" || !stringList.Contains(name))
              stringList.Add(name);
            if (inputFormInfo.Name.ToLower() == "hud-1 page 2" && field == "RESPA 2010 GFE and HUD-1" && !stringList.Contains("2010 HUD-1 Page 3") && arrayList1.Contains((object) "HUD1PG3_2010"))
              stringList.Add("2010 HUD-1 Page 3");
          }
        }
      }
      string empty2 = string.Empty;
      for (int index = 0; index < stringList.Count; ++index)
      {
        string str = "";
        if (Utils.CheckIf2015RespaTila(field))
        {
          if (stringList[index].ToLower() == "2010 itemization" || stringList[index].ToLower() == "gfe - itemization")
            str = "2015 Itemization";
          else if (stringList[index].ToLower() == "regz - til")
            str = "RegZ - LE";
          else if (stringList[index].ToLower() == "closing regz")
            str = "RegZ - CD";
        }
        else
        {
          switch (field)
          {
            case "RESPA 2010 GFE and HUD-1":
              if (stringList[index].ToLower() == "2015 itemization" || stringList[index].ToLower() == "old gfe and hud-1")
              {
                str = "2010 Itemization";
                break;
              }
              if (stringList[index].ToLower() == "regZ - le")
              {
                str = "REGZ - TIL";
                break;
              }
              if (stringList[index].ToLower() == "regz - cd")
              {
                str = "Closing RegZ";
                break;
              }
              break;
            case "Old GFE and HUD-1":
              if (stringList[index].ToLower() == "2015 itemization" || stringList[index].ToLower() == "2010 itemization")
              {
                str = "GFE - Itemization";
                break;
              }
              if (stringList[index].ToLower() == "regZ - le")
              {
                str = "REGZ - TIL";
                break;
              }
              if (stringList[index].ToLower() == "regz - cd")
              {
                str = "Closing RegZ";
                break;
              }
              break;
            case "203k Max Mortgage WS":
              if (!stringList.Contains("FHA Management"))
              {
                str = "FHA Management";
                break;
              }
              break;
          }
        }
        if (str != "" && !stringList.Contains(str) && arrayList1.Contains((object) str))
          stringList[index] = str;
      }
      if (stringList.Contains("203k Max Mortgage WS"))
        stringList.Remove("203k Max Mortgage WS");
      switch (field)
      {
        case "Old GFE and HUD-1":
          foreach (string str in source2)
          {
            if (stringList.Contains(str) && !((IEnumerable<string>) source1).Contains<string>(str))
              stringList.Remove(str);
          }
          foreach (string str in source3)
          {
            if (stringList.Contains(str) && !((IEnumerable<string>) source1).Contains<string>(str))
              stringList.Remove(str);
          }
          break;
        case "RESPA 2010 GFE and HUD-1":
          foreach (string str in source1)
          {
            if (stringList.Contains(str) && !((IEnumerable<string>) source2).Contains<string>(str))
              stringList.Remove(str);
          }
          foreach (string str in source3)
          {
            if (stringList.Contains(str) && !((IEnumerable<string>) source2).Contains<string>(str))
              stringList.Remove(str);
          }
          break;
        default:
          if (Utils.CheckIf2015RespaTila(field))
          {
            foreach (string str in source2)
            {
              if (stringList.Contains(str) && !((IEnumerable<string>) source3).Contains<string>(str))
                stringList.Remove(str);
            }
            foreach (string str in source1)
            {
              if (stringList.Contains(str) && !((IEnumerable<string>) source3).Contains<string>(str))
                stringList.Remove(str);
            }
            break;
          }
          break;
      }
      if (this.session.StartupInfo.AllowURLA2020)
      {
        if (flag2)
        {
          foreach (string str in strArray1)
          {
            if (stringList.Contains(str))
              stringList.Remove(str);
          }
        }
        else
        {
          foreach (string str in strArray2)
          {
            if (stringList.Contains(str))
              stringList.Remove(str);
          }
        }
      }
      else
      {
        for (int index = 0; index < ShipInDarkValidation.URLA2020FormNames.Count; ++index)
        {
          if (stringList.Contains(ShipInDarkValidation.URLA2020FormNames[index]))
            stringList.Remove(ShipInDarkValidation.URLA2020FormNames[index]);
        }
      }
      if (this.emFormMenuBox.CompareToFormList(stringList.ToArray()))
        return;
      this.emFormMenuBox.LoadFormList(stringList.ToArray());
    }

    private void refreshFormMenuToolList(object sender, EventArgs e)
    {
      BizRule.FieldAccessRight fieldAccessRights = this.loanMgr.GetFieldAccessRights("3969");
      if (this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_RespaTileFomrmVersion) && fieldAccessRights != BizRule.FieldAccessRight.ViewOnly)
      {
        if (((IEnumerable<DisclosureTrackingLog>) this.loan.GetLogList().GetAllDisclosureTrackingLog(false)).Count<DisclosureTrackingLog>() == 0 && !this.loan.IsFieldReadOnly("NEWHUD.X354") && ((IEnumerable<IDisclosureTracking2015Log>) this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false)).Count<IDisclosureTracking2015Log>() == 0 && fieldAccessRights != BizRule.FieldAccessRight.Hide)
        {
          this.formVersion = this.loan.GetField("3969");
          this.RefreshFormMenuToolList();
        }
        else
        {
          if (!(this.loan.GetField("3969") != this.formVersion))
            return;
          this.loan.SetField("3969", this.formVersion);
        }
      }
      else
      {
        if (!(this.loan.GetField("3969") != this.formVersion))
          return;
        this.loan.SetField("3969", this.formVersion);
      }
    }

    public void RefreshFormMenuToolList()
    {
      this.loadFormMenu(-1);
      this.loadToolList();
    }

    private void loadFormMenu(int formIndex)
    {
      InputFormList inputFormSettings = this.loanMgr.InputFormSettings;
      this.rebuildFormList();
      if (this.currForm == (InputFormInfo) null)
      {
        if (formIndex >= 0)
          this.emFormMenuBox.SelectedIndex = formIndex;
        else if (formIndex == -1)
        {
          if (this.defaultForm != null && this.emFormMenuBox.Items.Contains((object) this.defaultForm))
            this.emFormMenuBox.SelectedItem = (object) this.defaultForm;
          else if (this.emFormMenuBox.Items.Count > 0)
            this.emFormMenuBox.SelectedIndex = 0;
        }
      }
      else
      {
        string strB = this.currForm.Name;
        if (strB == "MLDS CA - GFE Page 4")
          strB = "MLDS - CA GFE";
        if (this.emFormMenuBox.Items.Contains((object) strB) && string.Compare(string.Concat(this.emFormMenuBox.SelectedItem), strB, true) != 0)
          this.OpenForm(this.currForm.Name);
      }
      this.ApplyBusinessRules();
    }

    public void PromptCreateNewLogRecord()
    {
      using (AddLogDialog addLogDialog = new AddLogDialog(this.loan.ContentAccess))
      {
        if (!addLogDialog.Editable)
        {
          int num1 = (int) Utils.Dialog((IWin32Window) this, "You don't have rights to add Conversation log or Task log.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (addLogDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.loan.GetLogList();
          if (addLogDialog.SelectedType == 1)
          {
            using (ConversationContainer conversationContainer = new ConversationContainer(new ConversationLog(DateTime.Now, this.session.UserID), false))
            {
              int num2 = (int) conversationContainer.ShowDialog((IWin32Window) this);
            }
          }
          else
          {
            if (addLogDialog.SelectedType != 2)
              return;
            new MilestoneTaskListControl(this.loanMgr, false).AddNewTask();
          }
        }
      }
    }

    public bool ContainsControl(Control con) => this.workPanel.Controls.Contains(con);

    private void clearWorkArea()
    {
      ArrayList arrayList = new ArrayList();
      foreach (Control control in (ArrangedElementCollection) this.workPanel.Controls)
        arrayList.Add((object) control);
      this.workPanel.Controls.Clear();
      this.topControl = (Control) null;
      foreach (Control control in arrayList)
      {
        if (!(control is WorkAreaPanelBase))
        {
          if (!this.isLoading && control is LoanLockTool && BuySellForm.IsFormDisplayed)
            control.Hide();
          else
            control.Dispose();
        }
      }
    }

    public void AddToWorkArea(Control newControl) => this.AddToWorkArea(newControl, false);

    public void ClearMilestoneLogArea()
    {
      foreach (Control control in (ArrangedElementCollection) this.workPanel.Controls)
      {
        if (control is MilestoneWS)
        {
          this.workPanel.Controls.Remove(control);
          this.AddMilestoneWorksheet(EllieMae.EMLite.RemotingServices.Session.LoanData.GetLogList().GetMilestoneAt(0));
        }
      }
    }

    public void AddToWorkArea(Control newControl, bool rememberCurrentFormID)
    {
      this.clearWorkArea();
      if (this.currForm != (InputFormInfo) null && this.currForm.Name == "2015 Itemization")
      {
        this.loan.Calculator.CalcOnDemand(CalcOnDemandEnum.Update2010ItemizationFrom2015Itemization);
        this.loan.Calculator.CalculateFeeVariance();
        EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanEditor>().RefreshLogPanel();
      }
      if (!rememberCurrentFormID)
        this.currForm = (InputFormInfo) null;
      else if (newControl is MilestoneWS)
        this.currForm = new InputFormInfo("Milestone Worksheet");
      else if (newControl is TabLinksControl && this.currForm == (InputFormInfo) null)
        this.currForm = ((TabLinksControl) newControl).GetCurrentFormInfo();
      if (newControl is Form)
      {
        newControl.Parent = (Control) this.workPanel;
      }
      else
      {
        MainForm.Instance.DisplayFieldID(string.Empty);
        MainForm.Instance.DisplayHelpText("Press F1 for Help");
      }
      this.workPanel.Controls.Add(newControl);
      newControl.Focus();
      this.topControl = newControl;
    }

    public void RemoveFromWorkArea()
    {
      this.AddToWorkArea((Control) this.freeScreen);
      if (!this.noFormBeingShown)
        return;
      if (this.emFormMenuBox.Items.Count > 0)
        this.OpenForm(this.emFormMenuBox.Items[0].ToString());
      else
        this.freeScreen.RefreshContents();
    }

    public DateTime AddDays(DateTime date, int dayCount)
    {
      DateTime date1 = date;
      AutoDayCountSetting policySetting = (AutoDayCountSetting) this.session.StartupInfo.PolicySettings[(object) "Policies.MilestoneExpDayCount"];
      try
      {
        switch (policySetting)
        {
          case AutoDayCountSetting.CalendarDays:
            date1 = dayCount == 0 ? date1.AddMinutes(1.0) : date1.AddDays((double) dayCount);
            break;
          case AutoDayCountSetting.CompanyDays:
            try
            {
              date1 = this.session.SessionObjects.GetBusinessCalendar(CalendarType.Business).AddBusinessDays(date1, dayCount, false);
              break;
            }
            catch (ArgumentOutOfRangeException ex)
            {
              Tracing.Log(LoanPage.sw, nameof (LoanPage), TraceLevel.Error, ex.ToString());
              break;
            }
          default:
            int num1 = 0;
            int num2 = Math.Abs(dayCount);
            if (num2 == 0)
            {
              date1 = date1.AddMinutes(1.0);
              break;
            }
            while (num1 < num2)
            {
              date1 = date1.AddDays(dayCount > 0 ? 1.0 : -1.0);
              if (date1.DayOfWeek < DayOfWeek.Saturday && date1.DayOfWeek > DayOfWeek.Sunday)
                ++num1;
            }
            break;
        }
        return date1;
      }
      catch (Exception ex)
      {
        MetricsFactory.IncrementErrorCounter(ex, "GetBusinessCalendar", "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs", nameof (AddDays), 3306);
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return date1;
      }
    }

    public int MinusBusinessDays(DateTime previous, DateTime currentLog)
    {
      BusinessCalendar businessCalendar = EllieMae.EMLite.RemotingServices.Session.SessionObjects.GetBusinessCalendar(CalendarType.Business);
      int num = 0;
      if (previous < currentLog)
      {
        for (DateTime date = previous.Date; !date.Date.Equals(currentLog.Date); date = date.AddDays(1.0))
        {
          if (!businessCalendar.IsBusinessDay(date) && !businessCalendar.IsWeekendDay(date))
            --num;
        }
      }
      else
      {
        for (DateTime date = currentLog; !date.Date.Equals(previous.Date); date = date.AddDays(1.0))
        {
          if (!businessCalendar.IsBusinessDay(date) && !businessCalendar.IsWeekendDay(date))
            --num;
        }
      }
      return num;
    }

    private void emFormMenuList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.emFormMenuBox.SelectedIndex < 0)
        return;
      if (!this.suspendEvents)
        this.OpenForm(this.emFormMenuBox.SelectedFormName);
      if (InputHandlerBase.InputFormAutoFocus(this.session))
        return;
      this.emFormMenuBox.Select();
      this.emFormMenuBox.Focus();
    }

    public string CurrentForm
    {
      get => !(this.currForm != (InputFormInfo) null) ? "" : this.currForm.Name;
      set => this.OpenForm(value);
    }

    public object GetFormScreen()
    {
      if (this.currForm == (InputFormInfo) null || this.freeScreen == null)
        return (object) null;
      return this.currForm.Type == InputFormType.Virtual ? (object) null : (object) this.freeScreen.GetInputHandler();
    }

    public object GetVerifScreen()
    {
      if (this.currForm == (InputFormInfo) null)
        return (object) null;
      switch (this.currForm.FormID)
      {
        case "VOAL":
          return (object) this.voalPan;
        case "VOD":
          return (object) this.vodPan;
        case "VOE":
          return (object) this.voePan;
        case "VOGG":
          return (object) this.voggPan;
        case "VOL":
          return (object) this.volPan;
        case "VOM":
          return (object) this.vomPan;
        case "VOOA":
          return (object) this.vooaPan;
        case "VOOI":
          return (object) this.vooiPan;
        case "VOOL":
          return (object) this.voolPan;
        case "VOR":
          return (object) this.vorPan;
        default:
          return (object) null;
      }
    }

    public bool OpenForm(string formOrToolName)
    {
      return (!this.CurrentForm.Equals("Lock Request Form") || this.AllowUnloadCurrentForm()) && this.OpenForm(formOrToolName, (Control) null);
    }

    private bool AllowUnloadCurrentForm()
    {
      foreach (Control control1 in (ArrangedElementCollection) this.workPanel.Controls)
      {
        if (control1 is LoanScreen)
        {
          if (!((LoanScreen) control1).AllowUnloadForm())
            return false;
        }
        else if (control1 is TabLinksControl && control1.Controls.Count > 0)
        {
          Control control2 = control1.Controls[0];
          if (control2 != null && control2 is GroupContainer && control2.Controls.Count > 2)
          {
            Control control3 = control2.Controls[2];
            if (control3 != null && control3 is TabControl && control3.Controls.Count > 0)
            {
              Control control4 = control3.Controls[0];
              if (control4 != null && control4 is TabPage && control4.Controls.Count > 0)
              {
                Control control5 = control4.Controls[0];
                if (control5 != null && control5 is LoanScreen && !((LoanScreen) control5).AllowUnloadForm())
                {
                  if (this.emFormMenuBox.Items.Contains((object) this.currentlySelectedScreen))
                  {
                    this.toolsFormsTabControl.SelectedTab = this.formPage;
                    this.emFormMenuBox.SelectedIndexChanged -= new EventHandler(this.emFormMenuList_SelectedIndexChanged);
                    this.emFormMenuBox.SelectedFormName = this.currentlySelectedScreen;
                    this.emFormMenuBox.SelectedIndexChanged += new EventHandler(this.emFormMenuList_SelectedIndexChanged);
                  }
                  else if (this.emToolMenuBox.Items.Contains((object) this.currentlySelectedScreen))
                  {
                    this.toolsFormsTabControl.SelectedTab = this.toolPage;
                    this.emToolMenuBox.SelectedIndexChanged -= new EventHandler(this.emToolMenuList_SelectedIndexChanged);
                    this.emToolMenuBox.SelectedFormName = this.currentlySelectedScreen;
                    this.emToolMenuBox.SelectedIndexChanged += new EventHandler(this.emToolMenuList_SelectedIndexChanged);
                  }
                  return false;
                }
              }
            }
          }
        }
      }
      return true;
    }

    public bool OpenForm(string formOrToolName, Control navControl)
    {
      PerformanceMeter.Abort("InputForm.Load");
      PerformanceMeter performanceMeter = PerformanceMeter.StartNew("InputForm.Load", "Opening a form or tool", true, false, true, 3473, nameof (OpenForm), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs");
      performanceMeter.AddVariable("Name", (object) formOrToolName);
      this.suspendEvents = true;
      this.currentlySelectedScreen = formOrToolName;
      if (this.emFormMenuBox.Items.Contains((object) formOrToolName))
      {
        this.toolsFormsTabControl.SelectedTab = this.formPage;
        this.emFormMenuBox.SelectedFormName = formOrToolName;
        this.emToolMenuBox.SelectedIndex = -1;
      }
      else if (this.emToolMenuBox.Items.Contains((object) formOrToolName))
      {
        this.toolsFormsTabControl.SelectedTab = this.toolPage;
        this.emToolMenuBox.SelectedFormName = formOrToolName;
        this.emFormMenuBox.SelectedIndex = -1;
      }
      this.suspendEvents = false;
      InputFormInfo form = !(formOrToolName == "Construction Management : Linked Loans") ? this.loanMgr.InputFormSettings.GetFormByName(formOrToolName) : new InputFormInfo("CONSTRUCTIONMANAGEMENT:LinkedLoans", "Construction Management");
      if (form == (InputFormInfo) null && this.loan != null)
      {
        if ((this.loan.Use2010RESPA || this.loan.Use2015RESPA) && formOrToolName == "885 P1-3")
          form = new InputFormInfo("RE88395", "885 P1-3");
        else if (formOrToolName == "Anti-Steering Safe Harbor Disclosure")
          form = new InputFormInfo("SAFEHARBORDISCLOSURE", "Anti-Steering Safe Harbor Disclosure");
      }
      bool flag;
      if (form != (InputFormInfo) null)
      {
        performanceMeter.AddVariable("Custom Form", form.Type == InputFormType.Custom ? (object) "Yes" : (object) "No");
        if (this.openFormInEditor(form, navControl))
          performanceMeter.Stop();
        flag = true;
      }
      else
      {
        performanceMeter.Abort();
        flag = this.openToolInEditor(formOrToolName);
      }
      this.ApplyOnDemandBusinessRules();
      if (flag && this.loanMgr != null)
        this.loanMgr.CurrentFormOrTool = formOrToolName;
      return flag;
    }

    public bool OpenFormByID(string formId) => this.OpenFormByID(formId, (Control) null);

    public bool OpenFormByID(string formId, Control navControl)
    {
      InputFormInfo form = this.loanMgr.InputFormSettings.GetForm(formId);
      return form == (InputFormInfo) null ? this.OpenForm(formId, navControl) : this.OpenForm(form.Name, navControl);
    }

    private bool openFormInEditor(InputFormInfo form, Control navControl)
    {
      this.noFormBeingShown = false;
      Cursor.Current = Cursors.WaitCursor;
      string formId = form.FormID;
      if (form.FormID == "HMDA_DENIAL" && (Utils.ParseInt((object) this.loan.GetField("HMDA.X27")) >= 2017 || this.loan.GetSimpleField("1825") != "Y"))
        form = new InputFormInfo("HMDA_DENIAL04", form.MnemonicName, form.Type);
      if (form.FormID == "FM1084")
        form = new InputFormInfo("FM1084A", form.MnemonicName, InputFormType.Standard);
      if (form.FormID == "RE88395" && form.Name != "885 P1-3" && this.loan != null && (this.loan.Use2010RESPA || this.loan.Use2015RESPA))
        form = new InputFormInfo("RE882", form.MnemonicName, InputFormType.Standard);
      if (form.FormID == "FEEVARIANCEWORKSHEET" && Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
        this.loan.Calculator.CalculateFeeVariance();
      this.AddToWorkArea((Control) this.freeScreen);
      this.currForm = form;
      if (navControl == null)
      {
        QuickLink[] quickLinksForForm = QuickLinksControl.GetQuickLinksForForm(formId, this.loan.Use2010RESPA, this.loan.Use2015RESPA, this.session, this.loan.Use2020URLA);
        if (this.currForm.Name == "Custom Fields")
        {
          navControl = (Control) new CustomFieldsPanel(this.freeScreen, this.session);
        }
        else
        {
          if (this.currForm.Name == "Settlement Service Provider List")
          {
            this.AddToWorkArea((Control) new SettlementServiceForm((IHtmlInput) this.loan, false, this.session), true);
            return true;
          }
          if (this.currForm.Name == "Home Counseling Providers")
          {
            this.AddToWorkArea((Control) new HomeCounselingProviderForm((IHtmlInput) this.loan, false, this.session), true);
            return true;
          }
          if (this.currForm.Name == "Affiliated Business Arrangements")
          {
            this.AddToWorkArea((Control) new AffiliatedBusinessArrangementForm((IHtmlInput) this.loan, false, this.session), true);
            return true;
          }
          if (quickLinksForForm != null)
            navControl = (Control) new QuickLinksControl((ILoanEditor) this, quickLinksForForm, formId, this.session);
          else if (QuickButtonsControl.UseQuickPanel(formId))
            navControl = (Control) new QuickButtonsControl((IMainScreen) MainScreen.Instance, this.freeScreen, this.currForm);
          else if (ServicingDetailsWorksheet.HasLink(formId))
          {
            this.AddToWorkArea((Control) new ServicingDetailsWorksheet(this.loan));
            return true;
          }
        }
      }
      if (this.currForm.FormID == "VOR" || this.currForm.FormID == "VOL" || this.currForm.FormID == "VOM" || this.currForm.FormID == "VOD" || this.currForm.FormID == "VOGG" || this.currForm.FormID == "VOOI" || this.currForm.FormID == "VOOA" || this.currForm.FormID == "VOOL" || this.currForm.FormID == "VOAL" || this.currForm.FormID == "VOE" || this.currForm.FormID == "TAX4506T" || this.currForm.FormID == "TAX4506")
      {
        this.ShowVerifPanel(this.currForm.FormID);
        if (this.currForm.Name == "VOL" && this.loan != null && !this.loan.Calculator.ValidateRevolvingVOLs() && this.loan.Calculator.UpdateRevolvingLiabilities((string) null, (string) null, true, true, true))
          this.ShowVerifPanel(this.currForm.Name);
        if (this.currForm.FormID == "TAX4506T")
          this.tax4506TPan.SetHelpTopic(this.currForm.Name);
        if (this.currForm.FormID == "TAX4506")
          this.tax4506Pan.SetHelpTopic(this.currForm.Name);
        return true;
      }
      if (TabLinksControl.UseTabLinks(this.session, this.currForm, this.loan))
      {
        this.AddToWorkArea((Control) new TabLinksControl(this.session, this.currForm, (IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainScreen, this.loan), true);
      }
      else
      {
        this.freeScreen.SetTitle(this.currForm.Name, navControl);
        if (this.currForm.Name == "Request for Copy of Tax Return (Classic)" || this.currForm.Name == "Request for Transcript of Tax (Classic)")
          this.freeScreen.AddHelpIconToTitle("4506_4506T_VersionInfo");
        if (this.currForm.Type == InputFormType.Custom)
          this.freeScreen.SetHelpTopic("");
        else
          this.freeScreen.SetHelpTopic(this.currForm.Name);
        if (form.FormID != "BORVESTING")
          this.freeScreen.LoadForm(this.currForm);
      }
      return false;
    }

    private InputFormInfo getPhysicalFormForVirtualForm(string formId)
    {
      switch (formId.ToUpper())
      {
        case "FM1084":
          return new InputFormInfo("FM1084A", "108&4A Cash Analysis");
        case "CUSTOMFIELDS":
          return new InputFormInfo("CF_1", "Custom Fields - Page 1");
        default:
          return (InputFormInfo) null;
      }
    }

    private void freeScreen_FormChanged(object sender, EventArgs e)
    {
      switch (sender)
      {
        case LoanScreen _:
          LoanScreen loanScreen = (LoanScreen) sender;
          if (sender == null)
            break;
          this.currForm = loanScreen.CurrentForm;
          if (this.loan == null || this.loan.Calculator == null)
            break;
          this.loan.Calculator.CurrentFormID = this.currForm.FormID;
          break;
        case string _:
          InputFormInfo form = this.loanMgr.InputFormSettings.GetForm((string) sender);
          if (!(form != (InputFormInfo) null) || !(this.currForm != (InputFormInfo) null) || !(form.Name.ToLower() != this.currForm.Name.ToLower()))
            break;
          this.freeScreen.SetTitleOnly(form.Name);
          if (this.currForm.Type == InputFormType.Custom)
          {
            this.freeScreen.SetHelpTopic("");
            break;
          }
          this.freeScreen.SetHelpTopic(this.currForm.Name);
          break;
      }
    }

    private void PrintPreview(object sender, EventArgs e)
    {
      this.printBtn.Focus();
      FormSelectorDialog formSelectorDialog;
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        string inputFormName = this.currForm == (InputFormInfo) null ? "" : this.currForm.Name;
        switch (inputFormName)
        {
          case "885 P1-3":
          case "MLDS - CA GFE":
          case "MLDS - CA GFE Page 4":
          case "MLDS P1":
            inputFormName = "RE885";
            if (!this.session.LoanDataMgr.LoanData.Use2010RESPA && !this.session.LoanDataMgr.LoanData.Use2015RESPA)
            {
              AmortizationType amortizationType = AmortizationTypeEnumUtil.NameInLoanToValue(this.session.LoanData.GetSimpleField("608"));
              string simpleField = this.session.LoanData.GetSimpleField("1177");
              if ((amortizationType == AmortizationType.ARM || amortizationType == AmortizationType.FixedRate) && simpleField == string.Empty)
                inputFormName = "RE88395";
            }
            if (this.currForm != (InputFormInfo) null && this.currForm.FormID == "RE882")
            {
              inputFormName = "RE882";
              break;
            }
            break;
          case "Cash-to-Close":
            inputFormName = "CASH-TO-CLOSE";
            break;
          case "Debt Consolidation":
            inputFormName = "DEBT CONSOLIDATION";
            break;
          case "HELOC Management":
          case "VA Management":
            if (this.topControl != null && this.topControl is TabLinksControl)
            {
              TabLinksControl topControl = (TabLinksControl) this.topControl;
              if (topControl != null && topControl.CurrentFormName != null)
              {
                inputFormName = topControl.CurrentFormName;
                break;
              }
              break;
            }
            break;
          case "Loan Comparison":
            inputFormName = "LOAN COMPARISON";
            break;
          case "Prequalification":
            inputFormName = "PREQUALIFICATION";
            break;
          case "Profit Management":
            inputFormName = "PROFIT MANAGEMENT";
            break;
          case "Rent vs. Own":
            inputFormName = "RENT V OWN";
            break;
          case "Trust Account":
            inputFormName = "TRUST ACCOUNT";
            break;
          case "USDA Management":
            if (this.topControl != null && this.topControl is TabLinksControl)
            {
              TabLinksControl topControl = (TabLinksControl) this.topControl;
              if (topControl != null && topControl.CurrentFormName != null)
              {
                inputFormName = topControl.CurrentFormName;
                if (this.session.LoanDataMgr.LoanData.GetField("1825") == "2020" && !inputFormName.Contains("INCOME"))
                {
                  inputFormName += "_2020";
                  break;
                }
                break;
              }
              break;
            }
            break;
          default:
            if (this.currForm != (InputFormInfo) null)
            {
              inputFormName = this.currForm.FormID;
              break;
            }
            break;
        }
        if (inputFormName == "GVTADM" && this.session.LoanDataMgr.LoanData.GetSimpleField("1172") == "VA")
          inputFormName = "VAAddendum";
        LoanData loanData = this.session.LoanDataMgr.LoanData;
        int currentVerifs = -1;
        if ((inputFormName == "VOD" || inputFormName == "VOGG" || inputFormName == "VOOA" || inputFormName == "VOOI" || inputFormName == "VOL" || inputFormName == "VOOL" || this.currForm != (InputFormInfo) null && this.currForm.FormID == "VOAL" || inputFormName == "VOE" || inputFormName == "VOM" || inputFormName == "VOR") && this.topControl is VerificationBase)
          currentVerifs = ((VerificationBase) this.topControl).CurrentVerificationNo();
        if (this.topControl is ConversationWS)
          inputFormName = "ConversationLog";
        else if (this.topControl is MilestoneTaskListControl)
          inputFormName = "TasksList";
        else if (this.topControl is DisclosureTrackingWS)
          inputFormName = "Disclosure Tracking WS";
        else if (this.topControl is DisclosureTracking2015WS)
          inputFormName = "Disclosure Tracking 2015 WS";
        else if (this.topControl is AUSTrackingTool)
          inputFormName = "AUS Tracking";
        else if (this.topControl is RepAndWarrantTracker)
          inputFormName = "Rep and Warrant Tracker";
        else if (this.topControl is VerificationTool)
          inputFormName = "Verification and Documentation Tracking";
        else if (this.topControl is LoanScreen && this.freeScreen != null)
          this.freeScreen.UpdateCurrentField();
        formSelectorDialog = sender == null ? new FormSelectorDialog(this.session, inputFormName, loanData, true) : (!(inputFormName == "VOD") && !(inputFormName == "VOGG") && !(inputFormName == "VOOI") && !(inputFormName == "VOL") && (!(this.currForm != (InputFormInfo) null) || !(this.currForm.FormID == "VOAL")) && !(inputFormName == "VOE") && !(inputFormName == "VOM") && !(inputFormName == "VOR") || currentVerifs <= -1 ? new FormSelectorDialog(this.session, inputFormName, loanData) : new FormSelectorDialog(this.session, inputFormName, loanData, currentVerifs));
        if (this.session.LoanDataMgr.LoanData.Use2015RESPA)
          loanData.Calculator.CalculateFeeVariance();
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
      try
      {
        formSelectorDialog.LoanLogChangedEvent += new EventHandler(this.refreshLoanLogPanel);
        string[] disclosureEntities = new EllieMae.EMLite.LoanServices.Bam(this.session.LoanDataMgr).GetCompanyDisclosureEntities();
        formSelectorDialog.EntityList = disclosureEntities;
        formSelectorDialog.ShowDialog((IWin32Window) this);
      }
      finally
      {
        formSelectorDialog.Dispose();
      }
    }

    private void refreshLoanLogPanel(object sender, EventArgs e)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanEditor>().RefreshContents();
    }

    private void displayLoanStatus()
    {
      if (EpassLogin.IsEncompassSelfHosted)
      {
        SelfHostedMessage selfHostedMessage = new SelfHostedMessage();
        if (selfHostedMessage.ShowDialog() != DialogResult.OK)
          return;
        selfHostedMessage.Close();
      }
      else if (!Modules.IsModuleAvailableForUser(EncompassModule.StatusOnline, false))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to use the Status Online feature. Contact your system administrator to request access to this module.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (!this.session.LoanDataMgr.Writable)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "This loan file is opened in read-only mode.   You are not authorized to use the Status Online feature.");
      }
      else
      {
        EllieMae.EMLite.DataEngine.LoanIdentity loanIdentity = EllieMae.EMLite.RemotingServices.Session.LoanManager.GetLoanIdentity(EllieMae.EMLite.RemotingServices.Session.LoanData.GUID);
        if (loanIdentity == (EllieMae.EMLite.DataEngine.LoanIdentity) null)
        {
          if (EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.IsNew())
            MainScreen.Instance.SaveLoan(false, false);
          loanIdentity = EllieMae.EMLite.RemotingServices.Session.LoanManager.GetLoanIdentity(EllieMae.EMLite.RemotingServices.Session.LoanData.GUID);
        }
        if (loanIdentity == (EllieMae.EMLite.DataEngine.LoanIdentity) null)
          throw new ApplicationException("Status Online Tool is instantiated without a valid loan file.");
        using (StatusOnlineDialog statusOnlineDialog = new StatusOnlineDialog(this.session.LoanDataMgr, EllieMae.EMLite.RemotingServices.Session.LoanManager.GetStatusOnlineSetup(loanIdentity)))
        {
          int num3 = (int) statusOnlineDialog.ShowDialog((IWin32Window) this.ParentForm);
        }
      }
    }

    private void showWorkFlowTasksDialogue()
    {
      if (EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.IsNew())
      {
        if (Utils.Dialog((IWin32Window) this, "The loan should be saved to continue further. Would you like to save and continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
          return;
        this.saveBtn_Click((object) null, (EventArgs) null);
      }
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      string scope = "loc";
      string workflowTasksPageUrl = EllieMae.EMLite.RemotingServices.Session.DefaultInstance.StartupInfo.WorkflowTasksPageUrl;
      var data = new
      {
        firstName = this.loan.GetField("4000"),
        lastName = this.loan.GetField("4002"),
        middleName = this.loan.GetField("4001"),
        suffixToName = this.loan.GetField("4003")
      };
      dictionary.Add("borrowerNameData", (object) data);
      dictionary.Add("loanNumber", (object) this.loan.LoanNumber);
      dictionary.Add("loanId", (object) this.loan.GUID);
      dictionary.Add("hostname", (object) "smartclient");
      dictionary.Add("instanceId", (object) this.session?.Connection?.Server?.InstanceName);
      dictionary.Add("errorMessages", (object) new List<string>());
      WebHost newControl = new WebHost(scope);
      UserInfo userInfo = EllieMae.EMLite.RemotingServices.Session.DefaultInstance.UserInfo;
      newControl.LoadModule(workflowTasksPageUrl, new ModuleParameters()
      {
        User = new ModuleUser()
        {
          ID = userInfo.Userid,
          LastName = userInfo.LastName,
          FirstName = userInfo.FirstName,
          Email = userInfo.Email
        },
        Parameters = dictionary
      });
      newControl.Dock = DockStyle.Fill;
      this.AddToWorkArea((Control) newControl);
    }

    private void showLockComparisonToolDialogue()
    {
      Dictionary<string, object> webPageParams = new Dictionary<string, object>();
      webPageParams.Add("oapiBaseUrl", (object) EllieMae.EMLite.RemotingServices.Session.StartupInfo.OAPIGatewayBaseUri);
      webPageParams.Add("loanId", (object) this.loan.GUID);
      webPageParams.Add("validatePricing", (object) this.showValidatePricing());
      string tapeThinClientUrl = EllieMae.EMLite.RemotingServices.Session.SessionObjects.ConfigurationManager.GetBidTapeThinClientURL();
      string appSetting = EnConfigurationSettings.AppSettings["ThinClientBidTape.Url"];
      using (LoadWebPageForm loadWebPageForm = new LoadWebPageForm((string.IsNullOrEmpty(appSetting) ? tapeThinClientUrl : appSetting) + "/lockComparisonTool", webPageParams, "sc", "Lock Comparison Tool"))
      {
        loadWebPageForm.Height = Convert.ToInt32((double) this.Height * 0.9);
        loadWebPageForm.Width = 700;
        int num = (int) loadWebPageForm.ShowDialog((IWin32Window) this);
      }
    }

    private bool showValidatePricing()
    {
      bool flag1 = false;
      string field = this.loan.GetField("LOCKRATE.RATESTATUS");
      bool flag2 = !(field == "NotLocked") && !(field == "Cancelled") && !(field == "Expired") && !(field == "Voided");
      if (EllieMae.EMLite.RemotingServices.Session.ACL.IsAuthorizedForFeature(AclFeature.ToolsTab_ValidatePricing) & flag2)
        flag1 = true;
      return flag1;
    }

    private bool openToolInEditor(string toolName)
    {
      switch (toolName)
      {
        case "AUS Tracking":
          this.AddToWorkArea((Control) new AUSTrackingTool(this.session, this.loan));
          return true;
        case "Amortization Schedule":
          AmortSchDialog amortSchDialog = new AmortSchDialog(this.session.LoanData);
          amortSchDialog.OnExportClicked += new EventHandler(this.AmortSchDialog_OnExportClicked);
          int num1 = (int) amortSchDialog.ShowDialog((IWin32Window) this);
          return true;
        case "Audit Trail":
          this.AddToWorkArea((Control) new AuditTrail());
          return true;
        case "Business Contacts":
          RxBusinessContact rxBusinessContact = new RxBusinessContact(true);
          int num2 = (int) rxBusinessContact.ShowDialog((IWin32Window) this);
          if (rxBusinessContact.GoToContact)
            EllieMae.EMLite.RemotingServices.Session.MainScreen.NavigateToContact(rxBusinessContact.SelectedContactInfo);
          return true;
        case "Co-Mortgagors":
          using (SwapBorrowerPairForm borrowerPairForm = new SwapBorrowerPairForm(this.loan))
          {
            borrowerPairForm.ImportFromLoanClicked += new EventHandler(this.borManager_ImportFromLoanClicked);
            int num3 = (int) borrowerPairForm.ShowDialog((IWin32Window) this);
          }
          this.RefreshBorrowerPairs();
          return true;
        case "Collateral Tracking":
          this.AddToWorkArea((Control) new DocumentTrackingManagement(this.session));
          return true;
        case "Compliance Review":
          this.session.LoanData.Calculator?.CalcOnDemand();
          this.session.LoanData.GetSnapshotDataForAllDisclosureTracking2015LogsForLoan();
          if (this.session.LoanData.Calculator != null)
            this.session.LoanData.Calculator.FormCalculation(this.session.LoanData.GetField("LE2.X28") == "Y" ? "CD3.X87" : "CD3.X93", (string) null, (string) null);
          LoanServiceManager.ShowComplianceWorksheet();
          return true;
        case "Condition Tracking":
          eFolderDialog.ShowInstance(this.session);
          return true;
        case "Conversation Log":
          this.AddToWorkArea((Control) new ConversationWS(this.loan));
          return true;
        case "Disclosure Tracking":
          if (this.loan.GetField("3969") == "RESPA 2010 GFE and HUD-1" || this.loan.GetField("3969") == "Old GFE and HUD-1")
            this.AddToWorkArea((Control) new DisclosureTrackingWS(this.session));
          else if (Utils.CheckIf2015RespaTila(this.session.LoanDataMgr.LoanData.GetField("3969")))
            this.AddToWorkArea((Control) new DisclosureTracking2015WS(this.session));
          this.currentlySelectedTool = toolName;
          return true;
        case "Document Tracking":
          eFolderDialog.ShowInstance(this.session);
          return true;
        case "Fannie Service DU":
          LoanServiceManager.ShowFannieDuWorksheet();
          return true;
        case "Fannie Service EC":
          LoanServiceManager.ShowFannieEcWorksheet();
          return true;
        case "Fee Variance Worksheet":
          QuickLinksControl linkControl = new QuickLinksControl((ILoanEditor) this, QuickLinksControl.GetQuickLinksForForm("FEEVARIANCEWORKSHEET", this.loan.Use2015RESPA, this.loan.Use2015RESPA, this.session), "FEEVARIANCEWORKSHEET", this.session, true);
          if (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
          {
            this.loan.Calculator.CalculateFeeVariance();
            this.AddToWorkArea((Control) new FeeVarianceWorksheetForm((IHtmlInput) this.loan, false, this.session, linkControl), true);
          }
          return true;
        case "Freddie Service LPA":
          LoanServiceManager.ShowFreddieLpaWorksheet();
          return true;
        case "Freddie Service LQA":
          LoanServiceManager.ShowFreddieLqaWorksheet();
          return true;
        case "Lock Comparison Tool":
          this.AddToWorkArea((Control) new LockComparisonToolForm(this.session, this.loanMgr));
          return true;
        case "MI Center":
          MiCenterService miCenterService = new MiCenterService();
          MiCenterUiMetaInfo centerUiMetaInfo = miCenterService.GetMiCenterUiMetaInfo();
          if (centerUiMetaInfo == null || !centerUiMetaInfo.IsUiSupported)
          {
            this.AddToWorkArea((Control) new MiCenterInfoForm(centerUiMetaInfo == null || string.IsNullOrWhiteSpace(centerUiMetaInfo.UiMessage) ? "MI Center is coming soon!!!" : centerUiMetaInfo.UiMessage));
            return true;
          }
          this.AddToWorkArea((Control) new MiCenterInfoForm(string.IsNullOrWhiteSpace(centerUiMetaInfo.UiMessage) ? "MI Center" : centerUiMetaInfo.UiMessage));
          string labelToDisplay = miCenterService.LaunchMiCenter(EllieMae.EMLite.RemotingServices.Session.LoanDataMgr, centerUiMetaInfo, Convert.ToInt32((double) this.Width * 0.9), Convert.ToInt32((double) this.Height * 0.9));
          if (!string.IsNullOrWhiteSpace(labelToDisplay))
            this.AddToWorkArea((Control) new MiCenterInfoForm(labelToDisplay));
          return true;
        case "MI Service Arch":
        case "MI Service MGIC":
        case "MI Service Radian":
          LoanServiceManager.ShowMIServiceWorksheet(toolName);
          return true;
        case "Post-Closing Condition Tracking":
          eFolderDialog.ShowInstance(this.session);
          return true;
        case "Rep and Warrant Tracker":
          this.AddToWorkArea((Control) new RepAndWarrantTracker(this.session, this.loan));
          return true;
        case "Secondary Registration":
          if (this.loanLockTool == null || !BuySellForm.IsFormDisplayed)
          {
            if (this.loanLockTool != null)
              this.loanLockTool.Dispose();
            this.loanLockTool = new LoanLockTool(this.session, this.loanMgr);
            this.AddToWorkArea((Control) this.loanLockTool);
          }
          else
          {
            this.loanLockTool.BringToFront();
            this.loanLockTool.Show();
            this.AddToWorkArea((Control) this.loanLockTool);
          }
          return true;
        case "Secure Form Transfer":
          this.session.LoanDataMgr.SaveLoan(true, (ILoanMilestoneTemplateOrchestrator) null, false);
          this.PrintPreview((object) null, (EventArgs) null);
          return true;
        case "Status Online":
          this.displayLoanStatus();
          return true;
        case "TQL Services":
          this.AddToWorkArea((Control) new TQLServices());
          return true;
        case "Tasks":
          this.AddToWorkArea((Control) new MilestoneTaskListControl(this.loanMgr, false));
          return true;
        case "Verification and Documentation Tracking":
          this.AddToWorkArea((Control) new VerificationTool(this.session, this.loan));
          return true;
        case "Workflow Tasks":
          this.showWorkFlowTasksDialogue();
          return true;
        case "Worst Case Pricing":
          this.AddToWorkArea((Control) new WorstCasePricingTool());
          return true;
        default:
          return false;
      }
    }

    private void emToolMenuList_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.emToolMenuBox.SelectedIndex < 0 || this.suspendEvents)
        return;
      MetricsFactory.IncrementCounter(string.Format("LoanTool_{0}_UsageCounter", (object) this.emToolMenuBox.SelectedFormName.Replace(" ", string.Empty)), (SFxTag) new SFxUiTag());
      this.OpenForm(this.emToolMenuBox.SelectedFormName);
    }

    public string GetHelpTargetName()
    {
      string helpTargetName = this.topControl is IOnlineHelpTarget ? ((IOnlineHelpTarget) this.topControl).GetHelpTargetName() : string.Empty;
      if (this.currForm != (InputFormInfo) null)
        helpTargetName = this.currForm.Type == InputFormType.Custom ? "Custom Form" : helpTargetName;
      return helpTargetName;
    }

    private void borEmailPanel_Click(object sender, EventArgs e)
    {
      this.addConversationLogRecord(0, true);
    }

    private void borPhonePanel_Click(object sender, EventArgs e)
    {
      this.addConversationLogRecord(0, false);
    }

    public void SaveAndRefresh()
    {
      try
      {
        MainForm.Instance.Enabled = false;
        this.saveBtn_Click((object) null, (EventArgs) null);
        using (CursorActivator.Wait())
          EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.refreshLoanFromServer();
      }
      finally
      {
        MainForm.Instance.Enabled = true;
      }
    }

    private void saveBtn_Click(object sender, EventArgs e)
    {
      MainForm.Instance.HideInsightsOnStatusBar();
      MetricsFactory.IncrementCounter("LoanSaveIncCounter", (SFxTag) new SFxUiTag());
      using (MetricsFactory.GetIncrementalTimer("LoanSaveIncTimer", (SFxTag) new SFxUiTag()))
      {
        bool mergeOnly = false;
        if (sender as StandardIconButton == this.stdIconBtnSynch)
          mergeOnly = true;
        using (CursorActivator.Wait())
        {
          MainScreen.Instance.CloseCEGetUpdatesBalloonToolTip();
          if (this.freeScreen != null && this.workPanel.Controls.Contains((Control) this.freeScreen))
            this.freeScreen.UpdateCurrentField();
          if (this.loan != null)
          {
            if (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
            {
              this.loan.Calculator.CalculateFeeVariance();
              this.session.LoanDataMgr.SyncESignConsentData();
            }
            if (this.loan.GetField("1172") != "HELOC" && RegulationAlertDialog.DisplayAlerts((IWin32Window) this) == DialogResult.Cancel)
              return;
            LoanServiceManager.SaveLoan();
            bool flag = this.correspondentChannelEmailPopupDisable(this.loan.GetField("2626"));
            if (this.loan.GetField("1240") == string.Empty && this.loan.GetField("1178") == string.Empty && this.loan.GetField("3040") != "Y" && !flag)
            {
              using (EmailCheckDialog emailCheckDialog = new EmailCheckDialog())
              {
                DialogResult dialogResult = emailCheckDialog.ShowDialog((IWin32Window) this);
                if (dialogResult != DialogResult.Cancel)
                  this.loan.SetField("3040", emailCheckDialog.DoNotShowAgain ? "Y" : "");
                if (dialogResult == DialogResult.OK)
                  this.loan.SetField("1240", emailCheckDialog.emailAddress);
              }
            }
          }
          if (mergeOnly)
            this.stdIconBtnSynch.Focus();
          else
            this.saveBtn.Focus();
          this.RemoveLockSnapshotForQATest(this.session.CompanyInfo.ClientID);
          Stopwatch stopwatch = new Stopwatch();
          EncompassApplication.CurrentLoan.BeforeExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
          EncompassApplication.CurrentLoan.AfterExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
          bool flag1 = false;
          Hashtable settingsFromCache = EllieMae.EMLite.RemotingServices.Session.SessionObjects.GetCompanySettingsFromCache("FEATURE");
          if (settingsFromCache.Contains((object) "SHOWLOANSAVEPROGRESS"))
            flag1 = Convert.ToBoolean(settingsFromCache[(object) "SHOWLOANSAVEPROGRESS"]);
          try
          {
            if (flag1)
            {
              MainForm.Instance.Enabled = false;
              int num = (EncompassApplication.CurrentLoan.GetOnBeforeCommitExtensions() != null ? EncompassApplication.CurrentLoan.GetOnBeforeCommitExtensions().Count<System.Type>() : 0) + (EncompassApplication.CurrentLoan.GetOnCommittedExtensions() != null ? EncompassApplication.CurrentLoan.GetOnCommittedExtensions().Count<System.Type>() : 0);
              EncompassApplication.CurrentLoan.BeforeExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvokeForProgressBar);
              EncompassApplication.CurrentLoan.AfterExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvokeForProgressBar);
              EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity = new LoanActivityMetricData();
              EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.BeforeLoanActivity += new LoanActivityEventHandler(this.LoanDataMgr_BeforeLoanActivity_ProgressBar);
              EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.AfterLoanActivity += new LoanActivityEventHandler(this.LoanDataMgr_AfterLoanActivity_ProgressBar);
              MainForm.Instance.StatusBarProgressSetMax(50 + num * 10);
              if (ApplicationLog.DebugEnabled || EnConfigurationSettings.GlobalSettings.ShowEnhancedMetrics)
              {
                this.progressDialog = new LoanSaveProgressDialog();
                this.progressDialog.SetProgressMax(50 + num * 10);
                this.progressDialog.StartPosition = FormStartPosition.Manual;
                this.progressDialog.Top = MainForm.Instance.Top + MainForm.Instance.Height / 2 - this.progressDialog.Height / 2;
                this.progressDialog.Left = MainForm.Instance.Left + MainForm.Instance.Width / 2 - this.progressDialog.Width / 2;
                EncompassApplication.CurrentLoan.BeforeExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvoke);
                EncompassApplication.CurrentLoan.AfterExtensionInvoked += new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvoke);
                EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.BeforeLoanActivity += new LoanActivityEventHandler(this.LoanDataMgr_BeforeLoanActivity);
                EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.AfterLoanActivity += new LoanActivityEventHandler(this.LoanDataMgr_AfterLoanActivity);
                this.progressDialog.Owner = (Form) MainForm.Instance;
                this.progressDialog.TopMost = false;
                this.progressDialog.FormClosing += new FormClosingEventHandler(this.ProgressDialog_FormClosing);
                this.progressDialog.Show();
              }
              MainForm.Instance.StatusBarSetProgress(0);
              MainForm.Instance.StatusBarShowProgress();
              stopwatch.Start();
            }
            bool enableRateLockValidation = this.AllowLostSnapshotValidation(this.session.CompanyInfo.ClientID);
            try
            {
              bool flag2 = MainScreen.Instance.SaveLoanNoCalcAll(mergeOnly, enableRateLockValidation);
              if (!mergeOnly & flag2)
                SaveConfirmScreen.Show((IWin32Window) this, (string) null);
            }
            catch (MissingLockSnapshotException ex)
            {
              switch (Utils.Dialog((IWin32Window) this, "An error has occurred with the lock snapshot and lock confirmation log. The loan will be saved without the lock snapshot and lock confirmation log. Are you sure you want to continue saving?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
              {
                case DialogResult.Yes:
                  bool flag3 = MainScreen.Instance.SaveLoanNoCalcAll(mergeOnly, true, true);
                  if (!mergeOnly & flag3)
                  {
                    this.session.LoanDataMgr.SaveLockSnapshotRecapture(RecaptureUserDecision.SaveWithMissingData);
                    SaveConfirmScreen.Show((IWin32Window) this, (string) null);
                    break;
                  }
                  break;
                case DialogResult.No:
                  MainForm.Instance.Enabled = true;
                  if (Utils.Dialog((IWin32Window) this, "The changes made to the loan file will not be saved and the loan will be closed and reopened. Proceed to reopening this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                  {
                    string guid = this.loan.GUID;
                    if (MainScreen.Instance.CloseLoanWithoutPrompts(false))
                    {
                      MainScreen.Instance.OpenLoan(guid);
                      this.session.LoanDataMgr.SaveLockSnapshotRecapture(RecaptureUserDecision.ReopenLoan);
                      break;
                    }
                    break;
                  }
                  break;
              }
              Tracing.Log(LoanPage.sw, nameof (LoanPage), TraceLevel.Error, ex.ToString());
            }
            this.refreshCE();
            EncompassApplication.CurrentLoan.BeforeExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
            EncompassApplication.CurrentLoan.AfterExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
            if (this.freeScreen != null && this.workPanel.Controls.Contains((Control) this.freeScreen))
            {
              this.freeScreen.RefreshContents();
              this.pnlLoanInfo.RefreshContents();
            }
            if (this.workPanel.Controls.Count > 0 && this.workPanel.Controls[0] is eSignConsentAlertPanel)
              this.AddToWorkArea((Control) new eSignConsentAlertPanel(((AlertPanelBase) this.workPanel.Controls[0]).Alert));
            Button btnOpenWebView = this.btnOpenWebView;
            LoanDataMgr loanMgr = this.loanMgr;
            int num1 = loanMgr != null ? (!loanMgr.IsNew() ? 1 : 0) : 0;
            btnOpenWebView.Enabled = num1 != 0;
          }
          catch (Exception ex)
          {
            throw;
          }
          finally
          {
            if (flag1)
            {
              stopwatch.Stop();
              if (ApplicationLog.DebugEnabled || EnConfigurationSettings.GlobalSettings.ShowEnhancedMetrics)
              {
                EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity.TotalSaveTime = new TimeSpan?(stopwatch.Elapsed);
                this.progressDialog.Close();
                this.progressDialog.FormClosing -= new FormClosingEventHandler(this.ProgressDialog_FormClosing);
                this.progressDialog.Dispose();
                this.progressDialog = (LoanSaveProgressDialog) null;
                MainForm.Instance.ShowLoanMetricsOnStatusBar();
                EncompassApplication.CurrentLoan.BeforeExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvoke);
                EncompassApplication.CurrentLoan.AfterExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvoke);
                EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.BeforeLoanActivity -= new LoanActivityEventHandler(this.LoanDataMgr_BeforeLoanActivity);
                EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.AfterLoanActivity -= new LoanActivityEventHandler(this.LoanDataMgr_AfterLoanActivity);
                MainForm.Instance.Activate();
              }
              MainForm.Instance.StatusBarShowProgress(false);
              EncompassApplication.CurrentLoan.BeforeExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvokeForProgressBar);
              EncompassApplication.CurrentLoan.AfterExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_ExtensionInvokeForProgressBar);
              EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.BeforeLoanActivity -= new LoanActivityEventHandler(this.LoanDataMgr_BeforeLoanActivity_ProgressBar);
              EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.AfterLoanActivity -= new LoanActivityEventHandler(this.LoanDataMgr_AfterLoanActivity_ProgressBar);
              MainForm.Instance.Enabled = true;
            }
            EncompassApplication.CurrentLoan.BeforeExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
            EncompassApplication.CurrentLoan.AfterExtensionInvoked -= new ExtensionInvocationEventHandler(this.CurrentLoan_PluginElapsedTime);
            if (this.freeScreen != null && this.workPanel.Controls.Contains((Control) this.freeScreen))
            {
              this.freeScreen.RefreshContents();
              this.pnlLoanInfo.RefreshContents();
            }
          }
        }
      }
    }

    private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
    {
      e.Cancel = true;
    }

    private void Tracing_WriteMessage(TraceMessageEventArgs e)
    {
      string str = e.Message;
      if (!string.IsNullOrEmpty(e.Message) && e.Message.Length > 37 && e.Message[0] == '<' && e.Message[37] == '>')
        str = e.Message.Substring(38);
      EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity.LogOutput.AppendLine(e.Category + str);
    }

    private void LoanDataMgr_AfterLoanActivity_ProgressBar(object source, LoanActivityEventArgs e)
    {
      if (e.ActivityType == LoanActivityType.CalcAll || e.ActivityType == LoanActivityType.CalcOnDemand)
        MainForm.Instance.StatusBarIncrementProgress(25);
      if (e.ActivityType != LoanActivityType.LoanCommit)
        return;
      MainForm.Instance.StatusBarIncrementProgress(15);
    }

    private void LoanDataMgr_AfterLoanActivity(object source, LoanActivityEventArgs e)
    {
      LoanActivityType activityType;
      if (e.ActivityType == LoanActivityType.CalcAll || e.ActivityType == LoanActivityType.CalcOnDemand)
      {
        LoanSaveProgressDialog progressDialog = this.progressDialog;
        activityType = e.ActivityType;
        string detail = activityType.ToString();
        Color? color = new Color?();
        progressDialog.IncrementProgress("Running Business Rules", detail, 25, color);
        List<LoanActivityMetric> activityMetrics = EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity.ActivityMetrics;
        LoanActivityMetric loanActivityMetric = new LoanActivityMetric();
        activityType = e.ActivityType;
        loanActivityMetric.Name = activityType.ToString();
        TimeSpan? elapsed = e.Elapsed;
        ref TimeSpan? local = ref elapsed;
        loanActivityMetric.ExecutionTimeInMs = (int) (local.HasValue ? new double?(local.GetValueOrDefault().TotalMilliseconds) : new double?()).Value;
        loanActivityMetric.IsExtension = false;
        activityType = e.ActivityType;
        loanActivityMetric.ActivityName = activityType.ToString();
        activityMetrics.Add(loanActivityMetric);
      }
      if (e.ActivityType != LoanActivityType.LoanCommit)
        return;
      this.progressDialog.IncrementProgress("Saving Loan", "Loan Saved", 15);
      List<LoanActivityMetric> activityMetrics1 = EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity.ActivityMetrics;
      LoanActivityMetric loanActivityMetric1 = new LoanActivityMetric();
      activityType = e.ActivityType;
      loanActivityMetric1.Name = activityType.ToString();
      TimeSpan? elapsed1 = e.Elapsed;
      ref TimeSpan? local1 = ref elapsed1;
      loanActivityMetric1.ExecutionTimeInMs = (int) (local1.HasValue ? new double?(local1.GetValueOrDefault().TotalMilliseconds) : new double?()).Value;
      loanActivityMetric1.IsExtension = false;
      activityType = e.ActivityType;
      loanActivityMetric1.ActivityName = activityType.ToString();
      activityMetrics1.Add(loanActivityMetric1);
    }

    private void LoanDataMgr_BeforeLoanActivity_ProgressBar(object source, LoanActivityEventArgs e)
    {
      if (e.ActivityType == LoanActivityType.CalcAll || e.ActivityType == LoanActivityType.CalcOnDemand)
        MainForm.Instance.StatusBarIncrementProgress(0);
      if (e.ActivityType != LoanActivityType.LoanCommit)
        return;
      MainForm.Instance.StatusBarIncrementProgress(0);
    }

    private void LoanDataMgr_BeforeLoanActivity(object source, LoanActivityEventArgs e)
    {
      if (e.ActivityType == LoanActivityType.CalcAll || e.ActivityType == LoanActivityType.CalcOnDemand)
        this.progressDialog.UpdateProgress("Evaluating Business Rules", e.ActivityType.ToString());
      if (e.ActivityType != LoanActivityType.LoanCommit)
        return;
      this.progressDialog.UpdateProgress("Saving Loan", "Uploading loan");
    }

    private void CurrentLoan_PluginElapsedTime(object source, ExtensionInvocationEventArgs e)
    {
      if (e.Elapsed.HasValue)
      {
        string sw = LoanPage.sw;
        string fullName = e.Target.FullName;
        TimeSpan? elapsed = e.Elapsed;
        ref TimeSpan? local1 = ref elapsed;
        // ISSUE: variable of a boxed type
        __Boxed<double?> local2 = (ValueType) (local1.HasValue ? new double?(local1.GetValueOrDefault().TotalMilliseconds) : new double?());
        string msg = fullName + " elapsed time(ms): " + (object) local2;
        Tracing.Log(sw, TraceLevel.Verbose, nameof (LoanPage), msg);
        PerformanceMeter.Current.AddCheckpoint("Loan.save plugin " + e.Target.FullName + " ends.", 4603, nameof (CurrentLoan_PluginElapsedTime), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs");
      }
      else
        PerformanceMeter.Current.AddCheckpoint("Loan.save plugin " + e.Target.FullName + " starts.", 4607, nameof (CurrentLoan_PluginElapsedTime), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoanPage.cs");
    }

    private void CurrentLoan_ExtensionInvokeForProgressBar(
      object source,
      ExtensionInvocationEventArgs e)
    {
      if (!e.Elapsed.HasValue)
        return;
      MainForm.Instance.StatusBarIncrementProgress(10);
    }

    private void CurrentLoan_ExtensionInvoke(object source, ExtensionInvocationEventArgs e)
    {
      if (this.progressDialog == null)
        return;
      if (!e.Elapsed.HasValue)
      {
        this.progressDialog.UpdateProgress("Executing Plugins", e.Target.FullName);
      }
      else
      {
        this.progressDialog.IncrementProgress("Executing Plugins", e.Target.FullName, 10, new Color?(Color.Orange));
        List<LoanActivityMetric> activityMetrics = EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.LastActivity.ActivityMetrics;
        LoanActivityMetric loanActivityMetric = new LoanActivityMetric();
        loanActivityMetric.Name = e.Target.FullName;
        TimeSpan? elapsed = e.Elapsed;
        ref TimeSpan? local = ref elapsed;
        loanActivityMetric.ExecutionTimeInMs = (int) (local.HasValue ? new double?(local.GetValueOrDefault().TotalMilliseconds) : new double?()).Value;
        loanActivityMetric.IsExtension = true;
        loanActivityMetric.Plugin = e.Target;
        loanActivityMetric.ActivityName = e.InvocationType.ToString();
        activityMetrics.Add(loanActivityMetric);
      }
    }

    private void closeBtn_Click(object sender, EventArgs e)
    {
      this.closeBtn.Focus();
      if (!EllieMae.EMLite.RemotingServices.Session.LoanData.IsULDDExporting)
        MainScreen.Instance.CloseLoan(true);
      else
        InvestorExportDialog.Instance.CloseLoan(true);
      MainForm.Instance.HideInsightsOnStatusBar();
    }

    private void addConversationLogRecord(string name, string email, string phone, bool isEmail)
    {
      this.StartConversation(new ConversationLog(DateTime.Now, this.session.UserInfo.Userid)
      {
        IsEmail = isEmail,
        Name = name,
        Email = email,
        Phone = phone
      });
    }

    private void addConversationLogRecord(int person, bool isEmail)
    {
      string name = string.Empty;
      string email = string.Empty;
      string phone = string.Empty;
      switch (person)
      {
        case 0:
          name = this.session.LoanData.GetField("36") + " " + this.session.LoanData.GetField("37");
          email = this.session.LoanData.GetField("1240");
          phone = this.session.LoanData.GetField("66");
          break;
        case 1:
          name = this.session.LoanData.GetField("317");
          email = this.session.LoanData.GetField("1407");
          phone = this.session.LoanData.GetField("1406");
          break;
        case 2:
          name = this.session.LoanData.GetField("362");
          email = this.session.LoanData.GetField("1409");
          phone = this.session.LoanData.GetField("1408");
          break;
      }
      this.addConversationLogRecord(name, email, phone, isEmail);
    }

    private void allFormBox_CheckedChanged(object sender, EventArgs e) => this.loadFormMenu(-1);

    protected override void OnClosing(CancelEventArgs e)
    {
      foreach (Control control in (ArrangedElementCollection) this.workPanel.Controls)
      {
        if (!(control is LoanScreen) && !(control is VerificationBase))
          this.workPanel.Controls.Remove(control);
      }
    }

    private void FindField_Click(object sender, EventArgs e)
    {
      FieldGoToDialog fieldGoToDialog = new FieldGoToDialog(this.fieldGoTo);
      if (fieldGoToDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      this.GoToField(fieldGoToDialog.FieldID.ToUpper(), fieldGoToDialog.FindNext);
    }

    public void GoToField(string fieldID) => this.GoToField(fieldID, false);

    public void GoToField(string fieldID, EllieMae.EMLite.DataEngine.BorrowerPair targetPair)
    {
      EllieMae.EMLite.DataEngine.BorrowerPair selectedItem = (EllieMae.EMLite.DataEngine.BorrowerPair) this.cboBorrowers.SelectedItem;
      if (selectedItem != null && targetPair != null && selectedItem.Id != targetPair.Id)
      {
        for (int index = 0; index < this.cboBorrowers.Items.Count; ++index)
        {
          if (((EllieMae.EMLite.DataEngine.BorrowerPair) this.cboBorrowers.Items[index]).Id == targetPair.Id)
          {
            this.cboBorrowers.SelectedItem = this.cboBorrowers.Items[index];
            break;
          }
        }
      }
      this.GoToField(fieldID, false);
    }

    public void GoToField(string fieldID, bool findNext)
    {
      this.GoToField(fieldID, findNext, false);
    }

    public void BAMGoToField(string fieldID, bool findNext)
    {
      this.GoToField(fieldID, findNext, false);
      this.freeScreen.Paint += new PaintEventHandler(this.freeScreen_Paint);
    }

    private void freeScreen_Paint(object sender, PaintEventArgs e)
    {
      if (!(this.fieldGoTo != string.Empty))
        return;
      this.freeScreen.Paint -= new PaintEventHandler(this.freeScreen_Paint);
      try
      {
        if (!(this.currForm.Name.ToUpper() == "BORROWER INFORMATION - VESTING"))
          return;
        this.setFieldFocusInWindowsForm(this.fieldGoTo, this.freeScreen.Controls);
      }
      catch (Exception ex)
      {
      }
    }

    public void GoToField(string fieldID, string formName)
    {
      new SearchFields(this.loanMgr.InputFormSettings, this.loan).GoToField(fieldID, formName, this.loanMgr, this.freeScreen);
    }

    public void GoToField(string fieldID, bool findNext, bool searchToolPageOnly)
    {
      if (fieldID == string.Empty)
        return;
      this.fieldGoTo = fieldID;
      if (fieldID.StartsWith("AR") && this.tax4506Pan == null)
        this.tax4506Pan = new TAX4506Panel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
      if (fieldID.StartsWith("IR") && this.tax4506TPan == null)
        this.tax4506TPan = new TAX4506TPanel((IMainScreen) MainScreen.Instance, (IWorkArea) this);
      new SearchFields(this.loanMgr.InputFormSettings, this.loan).GoToField(fieldID, findNext, false, this.topControl, this.emFormMenuBox, this.emToolMenuBox, this.currForm, this.toolsFormsTabControl, this.volPan, this.vorPan, this.voePan, this.vomPan, this.tax4506Pan, this.tax4506TPan, this.allFormBox, this.freeScreen, this.session);
      EllieMae.EMLite.RemotingServices.Session.MainForm.BringToFront();
    }

    private void setFieldFocusInWindowsForm(string goToField, Control.ControlCollection cc)
    {
      foreach (Control control in (ArrangedElementCollection) cc)
      {
        switch (control)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
            if (control.Tag is string && control.Tag.ToString() == goToField)
            {
              control.Focus();
              return;
            }
            continue;
          default:
            this.setFieldFocusInWindowsForm(goToField, control.Controls);
            continue;
        }
      }
    }

    internal bool IsMenuItemEnabled(string item)
    {
      return this.emToolMenuBox.Items.Contains((object) item) || item == "Go To Field";
    }

    internal void ToolsMenuClick(string item)
    {
      if (item == "Go To Field")
        this.FindField_Click((object) null, (EventArgs) null);
      else
        this.OpenForm(item);
    }

    internal void SaveClicked() => this.saveBtn_Click((object) null, (EventArgs) null);

    internal void LoadFormsInMenu(ToolStripMenuItem head)
    {
      ArrayList arrayList1 = new ArrayList();
      ArrayList arrayList2 = new ArrayList((ICollection) this.emFormMenuBox.Items);
      ToolStripItem toolStripItem = (ToolStripItem) null;
      BizRule.FieldAccessRight fieldAccessRights = this.loanMgr.GetFieldAccessRights("3969");
      if (this.aclMgr.CheckPermission(AclFeature.SettingsTab_Company_DiagnosticMode, this.session.UserInfo) && EnConfigurationSettings.GlobalSettings.Debug || ((IEnumerable<DisclosureTrackingLog>) this.loan.GetLogList().GetAllDisclosureTrackingLog(false)).Count<DisclosureTrackingLog>() == 0 && !this.loan.IsFieldReadOnly("NEWHUD.X354") && ((IEnumerable<IDisclosureTracking2015Log>) this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false)).Count<IDisclosureTracking2015Log>() == 0 && fieldAccessRights != BizRule.FieldAccessRight.Hide)
      {
        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem("RESPA-TILA Form Version");
        ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem("Old GFE and HUD-1");
        ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem("RESPA 2010 GFE and HUD-1");
        ToolStripMenuItem toolStripMenuItem4 = new ToolStripMenuItem("RESPA-TILA 2015 LE and CD");
        ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem("URLA Form Version");
        ToolStripMenuItem toolStripMenuItem6 = new ToolStripMenuItem("URLA 2009");
        ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem("URLA 2020");
        bool allowUrlA2020 = EllieMae.EMLite.RemotingServices.Session.DefaultInstance.StartupInfo.AllowURLA2020;
        if (allowUrlA2020)
        {
          if (this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_ChangeURLAFormVersion) && this.IsURLAToggleEnabled() || this.session.EncompassEdition == EllieMae.EMLite.Common.Licensing.EncompassEdition.Broker)
          {
            toolStripMenuItem6.Click += new EventHandler(this.FormMenu_Click);
            toolStripMenuItem7.Click += new EventHandler(this.FormMenu_Click);
          }
          else
          {
            toolStripMenuItem6.Enabled = false;
            toolStripMenuItem7.Enabled = false;
          }
        }
        else
          toolStripMenuItem5.Visible = false;
        if (this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_RespaTileFomrmVersion) && fieldAccessRights != BizRule.FieldAccessRight.ViewOnly)
        {
          toolStripMenuItem2.Click += new EventHandler(this.FormMenu_Click);
          toolStripMenuItem3.Click += new EventHandler(this.FormMenu_Click);
          toolStripMenuItem4.Click += new EventHandler(this.FormMenu_Click);
        }
        else
        {
          toolStripMenuItem2.Enabled = false;
          toolStripMenuItem3.Enabled = false;
          toolStripMenuItem4.Enabled = false;
        }
        if (this.session.LoanDataMgr.LoanData.GetField("3969") == "Old GFE and HUD-1")
          toolStripMenuItem2.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
        else if (this.session.LoanDataMgr.LoanData.GetField("3969") == "RESPA 2010 GFE and HUD-1")
          toolStripMenuItem3.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
        else if (Utils.CheckIf2015RespaTila(this.session.LoanDataMgr.LoanData.GetField("3969")))
          toolStripMenuItem4.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
        if (allowUrlA2020)
        {
          toolStripMenuItem5.DropDownItems.AddRange(new ToolStripItem[2]
          {
            (ToolStripItem) toolStripMenuItem6,
            (ToolStripItem) toolStripMenuItem7
          });
          switch (this.session.LoanDataMgr.LoanData.GetField("1825"))
          {
            case "2009":
              toolStripMenuItem6.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
              break;
            case "2020":
              toolStripMenuItem7.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
              break;
          }
        }
        toolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[3]
        {
          (ToolStripItem) toolStripMenuItem2,
          (ToolStripItem) toolStripMenuItem3,
          (ToolStripItem) toolStripMenuItem4
        });
        toolStripMenuItem1.Click += new EventHandler(this.FormMenu_Click);
        arrayList1.Add((object) toolStripMenuItem5);
        arrayList1.Add((object) toolStripMenuItem1);
        arrayList1.Add((object) new ToolStripSeparator());
      }
      foreach (string name in arrayList2)
      {
        if (name.StartsWith("----"))
        {
          if (!(toolStripItem is ToolStripSeparator))
          {
            ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
            arrayList1.Add((object) toolStripSeparator);
            toolStripItem = (ToolStripItem) toolStripSeparator;
          }
        }
        else if (!name.StartsWith("VO"))
        {
          InputFormInfo formByName = this.loanMgr.InputFormSettings.GetFormByName(name);
          if (formByName != (InputFormInfo) null)
          {
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(formByName.MnemonicName);
            toolStripMenuItem.Click += new EventHandler(this.FormMenu_Click);
            arrayList1.Add((object) toolStripMenuItem);
            toolStripItem = (ToolStripItem) toolStripMenuItem;
          }
          else if (name == "Custom Fields")
          {
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("&Custom Fields");
            toolStripMenuItem.Click += new EventHandler(this.FormMenu_Click);
            arrayList1.Add((object) toolStripMenuItem);
            toolStripItem = (ToolStripItem) toolStripMenuItem;
          }
        }
      }
      if (toolStripItem is ToolStripMenuItem)
        arrayList1.Add((object) new ToolStripSeparator());
      ToolStripMenuItem toolStripMenuItem8 = new ToolStripMenuItem("Sh&ow All Forms");
      toolStripMenuItem8.Checked = this.allFormBox.Checked;
      toolStripMenuItem8.Click += new EventHandler(this.FormMenu_Click);
      arrayList1.Add((object) toolStripMenuItem8);
      ToolStripItem[] array = (ToolStripItem[]) arrayList1.ToArray(typeof (ToolStripItem));
      head.DropDownItems.Clear();
      head.DropDownItems.AddRange(array);
    }

    private bool IsURLAToggleEnabled() => this.loan.IsFieldEditable("1825") && !this.readOnly;

    internal string formTransfer(string currentFormID, string menuItemSelected)
    {
      if (menuItemSelected != "RESPA 2010 GFE and HUD-1" && menuItemSelected != "Old GFE and HUD-1" && !Utils.CheckIf2015RespaTila(menuItemSelected))
        return currentFormID;
      bool flag1 = Utils.CheckIf2015RespaTila(menuItemSelected);
      bool flag2 = menuItemSelected == "RESPA 2010 GFE and HUD-1";
      string str = currentFormID;
      switch (currentFormID.ToUpper())
      {
        case "CLOSINGDISCLOSUREPAGE1":
        case "CLOSINGDISCLOSUREPAGE2":
        case "HUD1PG2":
        case "HUD1PG2_2010":
          str = !flag1 ? (!flag2 ? "HUD1PG2" : "HUD1PG2_2010") : "CLOSINGDISCLOSUREPAGE2";
          break;
        case "CLOSINGDISCLOSUREPAGE3":
        case "HUD1PG1":
        case "HUD1PG1_2010":
          str = !flag1 ? (!flag2 ? "HUD1PG1" : "HUD1PG1_2010") : "CLOSINGDISCLOSUREPAGE3";
          break;
        case "CLOSINGDISCLOSUREPAGE4":
        case "CLOSINGDISCLOSUREPAGE5":
          str = !flag2 ? "HUD1PG1" : "HUD1PG3_2010";
          break;
        case "HUD1PG3_2010":
          str = !flag1 ? "HUD1PG1" : "CLOSINGDISCLOSUREPAGE1";
          break;
        case "LOANESTIMATEPAGE1":
        case "LOANESTIMATEPAGE2":
        case "LOANESTIMATEPAGE3":
          str = !flag2 ? "REGZGFE" : "REGZGFEHUD";
          break;
        case "REGZ50":
        case "REGZLE":
          str = !flag1 ? (!flag2 ? "REGZ50" : "REGZ50") : "REGZLE";
          break;
        case "REGZ50CLOSER":
        case "REGZCD":
          str = !flag1 ? (!flag2 ? "REGZ50CLOSER" : "REGZ50CLOSER") : "REGZCD";
          break;
        case "REGZGFE":
        case "REGZGFEHUD":
        case "REGZGFE_2010":
        case "REGZGFE_2015":
          str = !flag1 || !(currentFormID == "REGZGFEHUD") ? (!flag1 ? (!flag2 || !(currentFormID == "LOANESTIMATEPAGE1") ? (!flag2 ? "REGZGFE" : "REGZGFE_2010") : "REGZGFEHUD") : "REGZGFE_2015") : "LOANESTIMATEPAGE1";
          break;
      }
      return str;
    }

    internal void FormMenu_Click(object sender, EventArgs e)
    {
      string str1;
      if (sender is string)
        str1 = (string) sender;
      else
        str1 = ((ToolStripItem) sender).Text.Replace("&", string.Empty);
      InputFormInfo inputFormInfo = (InputFormInfo) null;
      if (str1 == "Show All Forms")
      {
        this.allFormBox.Checked = !this.allFormBox.Checked;
      }
      else
      {
        if (str1 == "URLA 2009" || str1 == "URLA 2020")
        {
          bool flag = this.session.LoanDataMgr.LoanData.LinkedData != null;
          if (str1 == "URLA 2020" && this.loan.DisplaySwitchURLAPopup)
          {
            this.loan.DisplaySwitchURLAPopup = false;
            this.loan.RefreshURLA2020Fields = true;
            if (flag)
              this.loan.LinkedData.RefreshURLA2020Fields = true;
          }
          this.session.LoanDataMgr.LoanData.SetField("1825", str1 == "URLA 2020" ? "2020" : "2009");
          if (flag)
            this.session.LoanDataMgr.LoanData.LinkedData.SetField("1825", this.session.LoanDataMgr.LoanData.GetField("1825"));
          if (this.freeScreen != null)
          {
            this.freeScreen.ClearFeeDetailsPopup();
            this.freeScreen.ClearCustomPanel();
          }
          this.rebuildFormList();
          this.loadToolList();
          this.RefreshLogPanel();
          if (this.currForm != (InputFormInfo) null)
          {
            string str2 = string.Empty;
            if (str1 == "URLA 2020")
            {
              if (this.currForm.FormID == "D10031" || this.currForm.FormID == "D10032" || this.currForm.FormID == "D10033" || this.currForm.FormID == "D10034")
                str2 = "1003 URLA Part 1";
              else if (this.currForm.FormID == "STREAMLINED1003")
                str2 = "Fannie Mae Additional Data";
            }
            else if (this.currForm.FormID == "D1003_2020P1" || this.currForm.FormID == "D1003_2020P2" || this.currForm.FormID == "D1003_2020P3" || this.currForm.FormID == "D1003_2020P4" || this.currForm.FormID == "D1003_2020P5" || this.currForm.FormID == "D1003_2020P6")
              str2 = "1003 Page 1";
            else if (this.currForm.FormID == "FANNIEMAEADDITIONALDATA")
              str2 = "FNMA Streamlined 1003";
            inputFormInfo = string.IsNullOrEmpty(str2) || this.loanMgr.InputFormSettings.IsAccessible(this.GetURLAFormIDFromFormName(str2)) ? new InputFormInfo(str2) : new InputFormInfo(this.defaultForm);
          }
        }
        switch (str1)
        {
          case "Old GFE and HUD-1":
            if (this.freeScreen != null)
            {
              this.freeScreen.ClearFeeDetailsPopup();
              this.freeScreen.ClearCustomPanel();
            }
            if (!(sender is string))
              this.session.LoanDataMgr.LoanData.SetField("3969", "Old GFE and HUD-1");
            this.rebuildFormList();
            this.loadToolList();
            if (this.currForm != (InputFormInfo) null)
            {
              if (this.currForm.FormID == "FUNDINGWORKSHEET")
                this.OpenForm("Funding Worksheet");
              else if (this.currForm.FormID == "RE882" || this.currForm.FormID == "RE88395PG4" || this.currForm.FormID == "RE88395")
                this.OpenFormByID("RE88395");
              else if (this.currForm.FormID == "SECTION32" || this.currForm.FormID == "SECTION32_2015")
                this.OpenFormByID("SECTION32");
              else if (this.currForm.FormID == "SETTLEMENTSERVICELIST")
              {
                this.OpenFormByID("Settlement Service Provider List");
              }
              else
              {
                string formId = this.formTransfer(this.currForm.FormID, str1);
                if (formId != this.currForm.FormID)
                  this.OpenFormByID(formId);
              }
            }
            else if (this.toolsFormsTabControl.SelectedTab == this.toolPage && this.currentlySelectedTool == "Disclosure Tracking")
              this.OpenForm("Disclosure Tracking");
            this.RefreshContents();
            break;
          case "RESPA 2010 GFE and HUD-1":
            if (this.freeScreen != null)
            {
              this.freeScreen.ClearFeeDetailsPopup();
              this.freeScreen.ClearCustomPanel();
            }
            if (!(sender is string))
            {
              this.session.LoanDataMgr.LoanData.SetField("3969", "RESPA 2010 GFE and HUD-1");
              this.session.LoanDataMgr.LoanData.SetField("NEWHUD.X354", "Y");
            }
            if (this.loan != null && this.loan.Calculator != null)
              this.loan.Calculator.FormCalculation("SWITCHTO2010", "", "");
            this.rebuildFormList();
            this.loadToolList();
            if (this.currForm != (InputFormInfo) null)
            {
              if (this.currForm.FormID == "FUNDINGWORKSHEET")
                this.OpenForm("Funding Worksheet");
              else if (this.currForm.FormID == "RE88395PG4" || this.currForm.FormID == "RE88395")
                this.OpenFormByID("RE88395");
              else if (this.currForm.FormID == "SECTION32_2009" || this.currForm.FormID == "SECTION32_2015")
                this.OpenFormByID("SECTION32");
              else if (this.currForm.FormID == "SETTLEMENTSERVICELIST")
              {
                this.OpenFormByID("Settlement Service Provider List");
              }
              else
              {
                string formId = this.formTransfer(this.currForm.FormID, str1);
                if (formId != this.currForm.FormID)
                  this.OpenFormByID(formId);
              }
            }
            else if (this.toolsFormsTabControl.SelectedTab == this.toolPage && this.currentlySelectedTool == "Disclosure Tracking")
              this.OpenForm("Disclosure Tracking");
            this.RefreshContents();
            break;
          default:
            if (!Utils.CheckIf2015RespaTila(str1))
            {
              switch (str1)
              {
                case "RESPA-TILA 2015 2.0":
                  break;
                case "URLA 2020":
                case "URLA 2009":
                  if (inputFormInfo != (InputFormInfo) null && !string.IsNullOrEmpty(inputFormInfo.Name))
                  {
                    this.OpenForm(inputFormInfo.Name);
                    return;
                  }
                  if (this.currForm != (InputFormInfo) null)
                  {
                    this.OpenForm(this.currForm.Name);
                    return;
                  }
                  this.RefreshContents();
                  return;
                default:
                  this.OpenForm(str1);
                  return;
              }
            }
            this.freeScreen.ClearCustomPanel();
            if (!(sender is string))
            {
              this.session.LoanDataMgr.LoanData.SetField("3969", "RESPA-TILA 2015 LE and CD");
              this.session.LoanDataMgr.LoanData.SetField("NEWHUD.X1139", "Y");
              this.session.LoanDataMgr.LoanData.SetField("NEWHUD.X713", "");
              this.session.LoanDataMgr.LoanData.SetField("LE3.X19", "AsApplicant");
              this.session.LoanDataMgr.LoanData.SetField("CD5.X67", "AsApplicant");
            }
            if (this.loan != null && this.loan.Calculator != null)
              this.loan.Calculator.FormCalculation("SWITCHTO2015", "", "");
            this.rebuildFormList();
            this.loadToolList();
            if (this.currForm != (InputFormInfo) null)
            {
              if (this.currForm.FormID == "FUNDINGWORKSHEET")
                this.OpenForm("Funding Worksheet");
              else if (this.currForm.FormID == "RE88395PG4" || this.currForm.FormID == "RE88395")
                this.OpenFormByID("RE88395");
              else if (this.currForm.FormID == "SECTION32" || this.currForm.FormID == "SECTION32_2009")
                this.OpenFormByID("SECTION32");
              else if (this.currForm.FormID == "SETTLEMENTSERVICELIST")
              {
                this.OpenFormByID("Settlement Service Provider List");
              }
              else
              {
                string formId = this.formTransfer(this.currForm.FormID, str1);
                if (formId != this.currForm.FormID)
                  this.OpenFormByID(formId);
              }
            }
            else if (this.toolsFormsTabControl.SelectedTab == this.toolPage && this.currentlySelectedTool == "Disclosure Tracking")
              this.OpenForm("Disclosure Tracking");
            this.RefreshContents();
            break;
        }
      }
    }

    private string GetURLAFormIDFromFormName(string frmname)
    {
      string empty = string.Empty;
      string formIdFromFormName;
      switch (frmname)
      {
        case "1003 Page 1":
          formIdFromFormName = "D10031";
          break;
        case "1003 Page 2":
          formIdFromFormName = "D10032";
          break;
        case "1003 Page 3":
          formIdFromFormName = "D10033";
          break;
        case "1003 Page 4":
          formIdFromFormName = "D10034";
          break;
        case "1003 URLA Part 1":
          formIdFromFormName = "D1003_2020P1";
          break;
        case "1003 URLA Part 2":
          formIdFromFormName = "D1003_2020P2";
          break;
        case "1003 URLA Part 3":
          formIdFromFormName = "D1003_2020P3";
          break;
        case "1003 URLA Part 4":
          formIdFromFormName = "D1003_2020P4";
          break;
        case "FNMA Streamlined 1003":
          formIdFromFormName = "STREAMLINED1003";
          break;
        case "Fannie Mae Additional Data":
          formIdFromFormName = "FANNIEMAEADDITIONALDATA";
          break;
        default:
          formIdFromFormName = "";
          break;
      }
      return formIdFromFormName;
    }

    public void RefreshBorrowerPairs()
    {
      this.cboBorrowers.Items.Clear();
      foreach (object borrowerPair in this.loan.GetBorrowerPairs())
        this.cboBorrowers.Items.Add(borrowerPair);
      ClientCommonUtils.PopulateDropdown(this.cboBorrowers, (object) this.loan.CurrentBorrowerPair, false);
      if (this.cboBorrowers.SelectedIndex < 0)
        this.cboBorrowers.SelectedIndex = 0;
      this.elmBorrower.Element = (object) new BorrowerLink((Control) this);
      if (this.workPanel.Controls.Count <= 0 || !(this.workPanel.Controls[0] is eSignConsentAlertPanel))
        return;
      this.AddToWorkArea((Control) new eSignConsentAlertPanel(((AlertPanelBase) this.workPanel.Controls[0]).Alert));
    }

    internal void PopulateMenuItems(ToolStripMenuItem head)
    {
      switch (this.trimMenuItemText(head.Text))
      {
        case "Loan":
          this.setupLoanMenuItems(head);
          break;
        case "Tools":
          this.setupToolsMenuItems(head);
          break;
      }
    }

    private string trimMenuItemText(string menuItem)
    {
      return menuItem.Trim('.').Replace("&", string.Empty);
    }

    private Control GetAncestor<T>()
    {
      Control ancestor = (Control) this;
      while (ancestor != null && !typeof (T).IsAssignableFrom(ancestor.GetType()))
        ancestor = ancestor.Parent;
      return ancestor;
    }

    private void setupLoanMenuItems(ToolStripMenuItem head)
    {
      ToolStripItem toolStripItem1 = (ToolStripItem) null;
      List<ToolStripItem> toolStripItemList = new List<ToolStripItem>();
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) head.DropDownItems)
      {
        if (dropDownItem is ToolStripSeparator)
        {
          dropDownItem.Visible = toolStripItem1 == null || !(toolStripItem1 is ToolStripSeparator);
        }
        else
        {
          switch (this.trimMenuItemText(dropDownItem.Text))
          {
            case "Add Borrower Pair":
              dropDownItem.Visible = this.btnEditBorrowerPair.Visible;
              dropDownItem.Enabled = this.btnEditBorrowerPair.Enabled;
              break;
            case "Append Data Template":
            case "Append Document Set":
            case "Append Task Set":
            case "Apply Affiliated Business Arrangement Template":
            case "Apply Closing Cost Template":
            case "Apply Input Form Set Template":
            case "Apply Settlement Service Providers Template":
            case "Go to Field":
            case "Manage Milestone Dates":
              dropDownItem.Visible = true;
              dropDownItem.Enabled = true;
              break;
            case "Apply Loan Program Template":
              if (this.loan != null && this.loan.IsFieldReadOnly("1401") && this.loan.IsFieldReadOnly("Button_loanprog"))
              {
                dropDownItem.Visible = true;
                dropDownItem.Enabled = false;
                break;
              }
              dropDownItem.Visible = true;
              dropDownItem.Enabled = true;
              break;
            case "Apply Loan Template Set":
              bool applicationRight = this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_Other_ApplyLoanTemplate);
              dropDownItem.Visible = applicationRight;
              dropDownItem.Enabled = applicationRight;
              break;
            case "Duplicate Loan Check":
              dropDownItem.Enabled = this.session.ServerManager.GetServerSetting("Components.DuplicateLoanCheck", false) != null ? (!EllieMae.EMLite.RemotingServices.Session.LoanDataMgr.SystemConfiguration.IsDuplicateLoanCheckGlobal ? (dropDownItem.Visible = false) : (dropDownItem.Visible = true)) : (dropDownItem.Visible = false);
              break;
            case "Exit Loan File":
              dropDownItem.Visible = this.closeBtn.Visible;
              dropDownItem.Enabled = this.closeBtn.Enabled;
              break;
            case "Manage Borrowers":
              dropDownItem.Visible = this.btnEditBorrowerPair.Visible;
              dropDownItem.Enabled = this.btnEditBorrowerPair.Enabled;
              break;
            case "Manage Milestone Templates":
              dropDownItem.Visible = this.isMilestoneMenuVisible((ToolStripMenuItem) dropDownItem);
              dropDownItem.Enabled = true;
              break;
            case "New Loan":
              PipelineScreen pipelineScreen;
              bool flag = this.GetAncestor<MainScreen>() is MainScreen ancestor && (pipelineScreen = ancestor.PipelineScreen) != null && pipelineScreen.CanOriginateLoans() && (this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank) || this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl));
              dropDownItem.Visible = flag;
              dropDownItem.Enabled = flag;
              break;
            case "Print":
              dropDownItem.Visible = this.printBtn.Visible;
              dropDownItem.Enabled = this.printBtn.Enabled;
              break;
            case "Revert to Default Form List":
              dropDownItem.Visible = this.loan.GetField("2864") != string.Empty;
              dropDownItem.Enabled = this.loan.GetField("2864") != string.Empty;
              break;
            case "Save Loan":
              dropDownItem.Visible = this.saveBtn.Visible;
              dropDownItem.Enabled = this.saveBtn.Enabled;
              break;
            case "Submit to DataTrac":
              dropDownItem.Visible = LoanServiceManager.ShowExportToDataTracButton();
              dropDownItem.Enabled = LoanServiceManager.ShowExportToDataTracButton();
              break;
            default:
              toolStripItemList.Add(dropDownItem);
              break;
          }
          if (dropDownItem.Visible)
            toolStripItem1 = dropDownItem;
        }
      }
      if (toolStripItemList.Count > 0)
      {
        foreach (ToolStripItem toolStripItem2 in toolStripItemList.ToArray())
          head.DropDownItems.Remove(toolStripItem2);
      }
      EllieMae.EMLite.DataEngine.BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
      foreach (EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair in this.loan.GetBorrowerPairs())
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(borrowerPair.Borrower.LastName + ", " + borrowerPair.Borrower.FirstName);
        if (borrowerPair.Borrower.Id == currentBorrowerPair.Borrower.Id)
          toolStripMenuItem.Image = (Image) EllieMae.EMLite.Properties.Resources.check_mark_green;
        toolStripMenuItem.Click += new EventHandler(this.BorrowersMenu_Click);
        toolStripMenuItem.Tag = (object) borrowerPair;
        toolStripMenuItemList.Add(toolStripMenuItem);
      }
      for (int index = toolStripMenuItemList.ToArray().Length - 1; index >= 0; --index)
        head.DropDownItems.Insert(5, (ToolStripItem) toolStripMenuItemList.ToArray()[index]);
    }

    private bool isMilestoneMenuVisible(ToolStripMenuItem tsMilestones)
    {
      return this.trimMenuItemText(tsMilestones.Text) == "Manage Milestone Templates" && this.checkMilestoneListsVisibility(tsMilestones);
    }

    private bool checkMilestoneListsVisibility(ToolStripMenuItem tsMilestoneList)
    {
      if ((MilestoneTemplatesSetting) EllieMae.EMLite.RemotingServices.Session.ServerManager.GetServerSetting("Policies.MilestoneTemplateSettings") == MilestoneTemplatesSetting.None)
        return false;
      FeaturesAclManager aclManager = (FeaturesAclManager) EllieMae.EMLite.RemotingServices.Session.ACL.GetAclManager(AclCategory.Features);
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) tsMilestoneList.DropDownItems)
      {
        switch (dropDownItem.Text.Trim('.').Replace("&", string.Empty))
        {
          case "Apply Milestone Template":
            if (this.checkAccessForManual())
              return true;
            continue;
          case "Apply Manual Mode":
            if (aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockUnlockMilestonesList))
              return true;
            continue;
          case "Apply Automatic Mode":
            if (aclManager.GetUserApplicationRight(AclFeature.LoanTab_LockUnlockMilestonesList))
              return true;
            continue;
          default:
            continue;
        }
      }
      return false;
    }

    private bool checkAccessForManual()
    {
      FeaturesAclManager aclManager = (FeaturesAclManager) EllieMae.EMLite.RemotingServices.Session.ACL.GetAclManager(AclCategory.Features);
      switch ((MilestoneTemplatesSetting) EllieMae.EMLite.RemotingServices.Session.StartupInfo.PolicySettings[(object) "Policies.MilestoneTemplateSettings"])
      {
        case MilestoneTemplatesSetting.Automatic:
        case MilestoneTemplatesSetting.None:
          return false;
        default:
          return aclManager.GetUserApplicationRight(AclFeature.LoanTab_ManuallyApplyMilestoneTemplate);
      }
    }

    private void setupToolsMenuItems(ToolStripMenuItem head)
    {
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) head.DropDownItems)
        dropDownItem.Visible = false;
      for (int index = 0; index < this.emToolMenuBox.Items.Count; ++index)
      {
        ToolStripItem toolStripItem = this.getToolStripItem(head, this.emToolMenuBox.Items[index].ToString());
        if (toolStripItem != null)
        {
          toolStripItem.Visible = true;
          head.DropDownItems.Remove(toolStripItem);
          head.DropDownItems.Insert(index, toolStripItem);
        }
      }
    }

    private ToolStripItem getToolStripItem(ToolStripMenuItem head, string text)
    {
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) head.DropDownItems)
      {
        if (this.trimMenuItemText(dropDownItem.Text) == text)
          return dropDownItem;
      }
      return (ToolStripItem) null;
    }

    internal void PopulateBorrowersMenu(ToolStripMenuItem head)
    {
      this.pnlLoanInfo.Focus();
      if (this.topControl != null)
        this.topControl.Focus();
      ArrayList arrayList = new ArrayList();
      foreach (ToolStripItem dropDownItem in (ArrangedElementCollection) head.DropDownItems)
      {
        if (dropDownItem.Text != "-")
          arrayList.Add((object) dropDownItem);
        else
          break;
      }
      foreach (ToolStripItem toolStripItem in arrayList)
        head.DropDownItems.Remove(toolStripItem);
      EllieMae.EMLite.DataEngine.BorrowerPair currentBorrowerPair = this.loan.CurrentBorrowerPair;
      arrayList.Clear();
      foreach (EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair in this.loan.GetBorrowerPairs())
      {
        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(borrowerPair.Borrower.LastName + ", " + borrowerPair.Borrower.FirstName);
        if (borrowerPair.Borrower.Id == currentBorrowerPair.Borrower.Id)
          toolStripMenuItem.Checked = true;
        toolStripMenuItem.Click += new EventHandler(this.BorrowersMenu_Click);
        arrayList.Add((object) toolStripMenuItem);
      }
      ToolStripMenuItem[] toolStripMenuItemArray = new ToolStripMenuItem[arrayList.Count + head.DropDownItems.Count];
      arrayList.CopyTo((Array) toolStripMenuItemArray, 0);
      head.DropDownItems.CopyTo((ToolStripItem[]) toolStripMenuItemArray, arrayList.Count);
      head.DropDownItems.Clear();
      head.DropDownItems.AddRange((ToolStripItem[]) toolStripMenuItemArray);
    }

    internal void BorrowersMenu_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.piggyBackSyncFields == null && this.session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields != null)
        this.piggyBackSyncFields = this.session.LoanDataMgr.SystemConfiguration.PiggybackSyncFields.GetSyncFields();
      ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem) sender;
      if (toolStripMenuItem.Text.StartsWith("&Swap"))
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to swap borrower and coborrower for this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          this.loan.SwapBorrowers(new EllieMae.EMLite.DataEngine.BorrowerPair[1]
          {
            this.loan.CurrentBorrowerPair
          });
          this.InitContentsForNewBorrowerPair(true);
          if (this.loan.LinkedData != null)
            this.loan.SyncPiggyBackFiles(this.piggyBackSyncFields, false, true, (string) null, (string) null, false);
        }
      }
      else if (toolStripMenuItem.Text.StartsWith("&Delete"))
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to delete the coborrower of this loan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        {
          this.loan.RemoveCoborrowers(new EllieMae.EMLite.DataEngine.BorrowerPair[1]
          {
            this.loan.CurrentBorrowerPair
          });
          this.InitContentsForNewBorrowerPair(true);
          if (this.loan.LinkedData != null)
            this.loan.SyncPiggyBackFiles(this.piggyBackSyncFields, false, true, (string) null, (string) null, false);
        }
      }
      else if (toolStripMenuItem.Text.StartsWith("&Add"))
      {
        this.loan.SetBorrowerPair(this.loan.CreateBorrowerPair());
        this.InitContentsForNewBorrowerPair(true);
        if (this.loan.LinkedData != null)
          this.loan.SyncPiggyBackFiles(this.piggyBackSyncFields, false, true, (string) null, (string) null, false);
      }
      else if (toolStripMenuItem.Text.StartsWith("&Manage"))
      {
        using (SwapBorrowerPairForm borrowerPairForm = new SwapBorrowerPairForm(this.loan))
        {
          borrowerPairForm.ImportFromLoanClicked += new EventHandler(this.borManager_ImportFromLoanClicked);
          int num = (int) borrowerPairForm.ShowDialog((IWin32Window) this);
        }
        this.RefreshBorrowerPairs();
      }
      else if (toolStripMenuItem.Text != "-")
      {
        EllieMae.EMLite.DataEngine.BorrowerPair tag = (EllieMae.EMLite.DataEngine.BorrowerPair) toolStripMenuItem.Tag;
        EllieMae.EMLite.DataEngine.BorrowerPair[] borrowerPairs = this.loan.GetBorrowerPairs();
        for (int index = 0; index < borrowerPairs.Length; ++index)
        {
          if (borrowerPairs[index].Id == tag.Id)
          {
            this.loan.SetBorrowerPair(borrowerPairs[index]);
            this.InitContentsForNewBorrowerPair(true);
            break;
          }
        }
      }
      Cursor.Current = Cursors.Default;
    }

    private void borManager_ImportFromLoanClicked(object sender, EventArgs e)
    {
      using (SelectLinkedLoanDialog linkedLoanDialog = new SelectLinkedLoanDialog(this.loan.GUID))
      {
        if (linkedLoanDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        ((SwapBorrowerPairForm) sender).ImportBorrowerFromAnotherLoan(linkedLoanDialog.SelectedPipelineInfo);
      }
    }

    public void Print() => this.PrintPreview((object) Missing.Value, (EventArgs) null);

    public void PrintPreview()
    {
    }

    internal void LoanConfig_Click(string item)
    {
      bool flag = true;
      switch (item)
      {
        case "Add Borrower Pair":
          this.addBorrowerPair();
          break;
        case "Append Data Template":
          if (this.selectDataTemplate())
          {
            flag = true;
            break;
          }
          break;
        case "Append Document Set":
          EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().AppendDocumentSet(this.session.LoanDataMgr);
          break;
        case "Append Task Set":
          if (this.selectTaskSet())
          {
            flag = true;
            break;
          }
          break;
        case "Apply Affiliated Business Arrangement Template":
          if (this.SelectAffilatesTemplate())
          {
            flag = true;
            break;
          }
          break;
        case "Apply Closing Cost Template":
          if (new ClosingCostSelect(this.loan).ShowDialog((IWin32Window) this) != DialogResult.OK)
          {
            flag = false;
            break;
          }
          break;
        case "Apply Input Form Set Template":
          this.selectFormList(false);
          break;
        case "Apply Loan Program Template":
          LoanProgramSelect loanProgramSelect = new LoanProgramSelect(this.loan, this.session);
          loanProgramSelect.TemplateApplied += new EventHandler(this.loan_TemplateApplied);
          if (loanProgramSelect.ShowDialog((IWin32Window) this) != DialogResult.OK)
          {
            flag = false;
            break;
          }
          break;
        case "Apply Loan Template Set":
          if (this.selectLoanTemplate())
          {
            flag = true;
            break;
          }
          break;
        case "Apply Milestone Template":
          if (this.session.SessionObjects.AllowConcurrentEditing && this.session.LoanDataMgr.IsLoanFileOnServerNewer(false))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "There is a newer version of the loan on server. Please get the latest loan first before applying a milestone template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
          this.applyMilestoneTemplate();
          flag = true;
          break;
        case "Apply Settlement Service Providers Template":
          if (this.SelectSettlementServiceProviders())
          {
            flag = true;
            break;
          }
          break;
        case "Change Milestone Dates":
          if (new ChangeMilestoneDates(this.session, false).ShowDialog((IWin32Window) this) != DialogResult.OK)
          {
            flag = true;
            break;
          }
          break;
        case "Duplicate Loan Check":
          this.duplicateLoanCheck();
          break;
        case "Exit Loan File":
          this.closeBtn_Click((object) null, (EventArgs) null);
          flag = false;
          break;
        case "Go to Field":
          this.FindField_Click((object) null, (EventArgs) null);
          break;
        case "Manage Borrowers":
          this.btnEditBorrowerPair_Click((object) null, (EventArgs) null);
          break;
        case "Print":
          this.PrintPreview((object) this.printBtn, (EventArgs) null);
          break;
        case "Revert to Default Form List":
          this.selectFormList(true);
          break;
        case "Save Loan":
          this.saveBtn_Click((object) null, (EventArgs) null);
          break;
        case "Submit to DataTrac":
          this.exportToDataTrac();
          break;
      }
      if (!flag)
        return;
      this.RefreshContents();
    }

    private void loan_TemplateApplied(object sender, EventArgs e)
    {
      object[] objArray = (object[]) sender;
      this.validateFHACountyLimit((string) objArray[0], (string) objArray[1], (string) objArray[2] == "Y", (DataTemplate) null, (LoanProgram) objArray[3]);
    }

    internal void lockUnlock_Click(ToolStripMenuItem item)
    {
      bool flag1 = true;
      bool flag2 = false;
      if (this.trimMenuItemText(item.Text) == "Apply Manual Mode" || this.trimMenuItemText(item.Text) == "Apply Automatic Mode")
      {
        if (this.trimMenuItemText(item.OwnerItem.Text) == "Manage Milestone Templates")
          flag2 = false;
        else if (this.trimMenuItemText(item.OwnerItem.Text) == "Manage Milestone Dates")
          flag2 = true;
      }
      switch (this.trimMenuItemText(item.Text))
      {
        case "Apply Manual Mode":
          if (flag2)
          {
            this.loan.GetLogList().MSDateLock = true;
            int num = (int) MessageBox.Show((IWin32Window) this, "In manual mode, you (and authorized users) can change a milestone’s expected completion date and the system will not automatically recalculate the subsequent milestones’ dates according to the settings configured by your system administrator. To change a milestone’s expected completion date, click Loan > Manage Milestone Dates > Change Milestone Dates.", "Milestone Dates are in Manual Mode", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          this.loan.GetLogList().MSLock = true;
          int num1 = (int) MessageBox.Show((IWin32Window) this, "In manual mode, authorized users can apply a new milestone template to this loan file at any time by clicking Loan > Manage Milestone Templates > Apply Milestone Template.\n\nNOTE: In manual mode the system does not automatically apply the best matching milestone template for your loan (as determined by your system administrator) if the loan’s channel, type, or other data is changed.", "Milestone Templates are in Manual Mode", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          break;
        case "Apply Automatic Mode":
          if (flag2)
          {
            this.loan.GetLogList().MSDateLock = false;
            int num2 = (int) MessageBox.Show((IWin32Window) this, "In automatic mode, when a milestone’s expected completion date is changed, the system automatically recalculates the subsequent milestones’ dates according to the settings configured by your system administrator. To change a milestone’s expected completion date, click Loan > Manage Milestone Dates > Change Milestone Dates.", "Milestone Dates are in Automatic Mode", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            break;
          }
          this.loan.GetLogList().MSLock = false;
          int num3 = (int) MessageBox.Show((IWin32Window) this, "Your system administrator may create different milestone templates for loans based on loan channel, loan type, or other conditions. In automatic mode, any time you change the loan’s channel, type, or other data, the system will automatically apply the milestone template that is the best match for the loan. If enabled by your administrator, an on-screen notification will display before a new milestone template is applied.\n\nNOTE: A milestone template contains the list of milestones that must be finished in order to complete the loan file.", "Milestone Templates are in Automatic Mode", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          break;
      }
      if (!flag1)
        return;
      this.RefreshContents();
    }

    public bool SelectSettlementServiceProviders()
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders, (FileSystemEntry) null, false))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
          if (this.session.LoanData != null)
          {
            if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders, selectedItem))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The settlement service provider template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            SettlementServiceTemplate templateSettings;
            try
            {
              templateSettings = (SettlementServiceTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.SettlementServiceProviders, selectedItem);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Can't open " + selectedItem.Name + " settlement service provider template file. Message: " + ex.Message);
              return false;
            }
            return this.session.LoanData.SetSettlementServiceProviders(templateSettings);
          }
        }
      }
      return false;
    }

    public bool SelectAffilatesTemplate()
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements, (FileSystemEntry) null, false))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          if (this.loan != null && this.loan.GetNumberOfAffiliates() > 0 && Utils.Dialog((IWin32Window) this, "The current affiliates will be deleted permanently. Do you want to continue?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.OK)
            return false;
          FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
          if (this.session.LoanData != null)
          {
            if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements, selectedItem))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The affiliate template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            AffiliateTemplate templateSettings;
            try
            {
              templateSettings = (AffiliateTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.AffiliatedBusinessArrangements, selectedItem);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Can't open " + selectedItem.Name + " affiliate template file. Message: " + ex.Message);
              return false;
            }
            return this.session.LoanData.SetAffiliateTemplate(templateSettings);
          }
        }
      }
      return false;
    }

    private bool selectDataTemplate()
    {
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData, (FileSystemEntry) null, false))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          Cursor.Current = Cursors.WaitCursor;
          FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
          if (this.session.LoanData != null)
          {
            if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData, selectedItem))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The data template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            DataTemplate templateSettings;
            try
            {
              templateSettings = (DataTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.MiscData, selectedItem);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Can't open " + selectedItem.Name + " data template file. Message: " + ex.Message);
              return false;
            }
            string simpleField1 = templateSettings.GetSimpleField("14");
            if (simpleField1 != string.Empty)
            {
              string simpleField2 = this.loan.GetSimpleField("LOID");
              if (simpleField2 != string.Empty && this.session.OrganizationManager.GetLOLicense(simpleField2, simpleField1) == null)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "This template contains an invalid state to current loan officer. You must modify it before using.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
            }
            if (this.session.LoanDataMgr.SystemConfiguration != null && this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting != null && !LOCompensationInputHandler.CheckLOCompRuleConfliction(this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) templateSettings, (string) null, (string) null, (string) null, false))
              return false;
            this.session.LoanData.VALoanValidation = true;
            string field1 = this.session.LoanData.GetField("3969");
            string field2 = this.session.LoanData.GetField("1172");
            string field3 = this.session.LoanData.GetField("HMDA.X100");
            if (!this.session.LoanData.SetDataTemplate(templateSettings))
              return false;
            if (this.session.LoanDataMgr.SystemConfiguration != null && this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting != null)
              LOCompensationInputHandler.CheckLOCompRuleConfliction(this.session.LoanDataMgr.SystemConfiguration.LoanOfficerCompensationSetting, (IHtmlInput) this.loan, (string) null, (string) null, (string) null, true);
            this.session.LoanDataMgr.UpdateCompanyStateLicense(true);
            this.session.LoanData.SetCurrentField("2865", selectedItem.Path);
            this.session.LoanDataMgr.setHMDAProfile(this.session.LoanData, templateSettings, field3, true);
            this.session.LoanData.Calculator.CalculateAll();
            if (!this.session.LoanData.VALoanValidation)
            {
              int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, this.session.LoanData.VALoanWarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              this.session.LoanData.VALoanValidation = true;
            }
            string field4 = this.session.LoanData.GetField("3969");
            if (field4 != field1)
              this.FormMenu_Click((object) field4, (EventArgs) null);
            this.validateFHACountyLimit(field2, this.session.LoanDataMgr.LoanData.GetSimpleField("1172"), this.session.LoanDataMgr.LoanData.GetSimpleField("3894") == "Y", templateSettings, (LoanProgram) null);
            Cursor.Current = Cursors.Default;
            return true;
          }
        }
      }
      return false;
    }

    private bool selectLoanTemplate()
    {
      using (LoanTemplateSelectDialog templateSelectDialog = new LoanTemplateSelectDialog(this.session, true, this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_CreateBlank), this.aclMgr.GetUserApplicationRight(AclFeature.LoanMgmt_CreateFromTmpl)))
      {
        if (templateSelectDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          Cursor.Current = Cursors.WaitCursor;
          FileSystemEntry selectedItem = templateSelectDialog.SelectedItem;
          if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.LoanTemplate, selectedItem))
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Loan Template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          LoanTemplateSelection templateSelection = new LoanTemplateSelection(selectedItem);
          if (templateSelection == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Loan Template '" + selectedItem.Name + "' format is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
          }
          try
          {
            Hashtable templateComponents = this.session.ConfigurationManager.GetLoanTemplateComponents(templateSelection.TemplateEntry);
            templateSelection.AppendData = templateSelectDialog.AppendData;
            ClosingCost closingCost = (ClosingCost) (BinaryObject) templateComponents[(object) "COST"];
            DataTemplate dt = (DataTemplate) (BinaryObject) templateComponents[(object) "MISCDATA"];
            LoanProgram lp = (LoanProgram) (BinaryObject) templateComponents[(object) "PROGRAM"];
            if (lp != null && this.loan != null && !this.loan.IsTemplate && this.loan.IsFieldReadOnly("1401") && this.loan.IsFieldReadOnly("Button_loanprog"))
            {
              if (Utils.Dialog((IWin32Window) this, "You don't have permission to apply loan program from the loan template you select. The loan program won't be applied to loan. Do you still want to proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                return false;
              if (templateComponents.ContainsKey((object) "PROGRAMFILE"))
                templateComponents.Remove((object) "PROGRAMFILE");
              if (templateComponents.ContainsKey((object) "PROGRAM"))
                templateComponents.Remove((object) "PROGRAM");
            }
            if (closingCost != null)
            {
              if (closingCost.RESPAVersion == "2015" && !this.session.LoanDataMgr.LoanData.Use2015RESPA)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for 2015 Itemization but current loan is for " + (this.session.LoanDataMgr.LoanData.Use2010RESPA ? "2010" : "old GFE") + " Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (closingCost.RESPAVersion == "2010" && !this.session.LoanDataMgr.LoanData.Use2010RESPA)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for 2010 Itemization but current loan is for " + (this.session.LoanDataMgr.LoanData.Use2015RESPA ? "2015" : "old GFE") + " Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (closingCost.RESPAVersion == "2009" && (this.session.LoanDataMgr.LoanData.Use2010RESPA || this.session.LoanDataMgr.LoanData.Use2015RESPA))
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for old GFE Itemization but current loan is for " + (this.session.LoanDataMgr.LoanData.Use2015RESPA ? "2015" : "2010") + " Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (!this.session.LoanDataMgr.LoanData.Use2010RESPA && !this.session.LoanDataMgr.LoanData.Use2015RESPA && closingCost != null && closingCost.For2010GFE)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The Closing Cost Template you selected is for new Itemization 2010 but current loan is for old GFE Itemization.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
            }
            else if (dt != null && (dt.RESPAVersion == "2015" && !this.loan.Use2015RESPA || dt.RESPAVersion == "2010" && !this.loan.Use2010RESPA || dt.RESPAVersion == "2009" && (this.loan.Use2010RESPA || this.loan.Use2015RESPA)))
            {
              if (this.loan != null && (this.loan.GetLogList().GetAllDisclosureTrackingLog(false).Length != 0 || this.loan.GetLogList().GetAllIDisclosureTracking2015Log(false).Length != 0))
              {
                int num = (int) Utils.Dialog((IWin32Window) this, "The current loan has been disclosed. Applying this template will change the RESPA-TILA Form Version currently being used for this loan.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
              }
              if (Utils.Dialog((IWin32Window) this, "Applying this template will change the RESPA-TILA Form Version currently being used for this loan. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return false;
            }
            string field1 = this.session.LoanDataMgr.LoanData.GetField("3969");
            string field2 = this.session.LoanDataMgr.LoanData.GetField("1172");
            this.session.LoanDataMgr.ApplyLoanTemplate(templateComponents, templateSelection.AppendData, true);
            if (field1 != this.session.LoanDataMgr.LoanData.GetField("3969"))
              this.FormMenu_Click((object) this.session.LoanDataMgr.LoanData.GetField("3969"), (EventArgs) null);
            else
              this.loadFormMenu(-1);
            this.validateFHACountyLimit(field2, this.session.LoanDataMgr.LoanData.GetSimpleField("1172"), this.session.LoanDataMgr.LoanData.GetSimpleField("3894") == "Y", dt, lp);
            return true;
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Loan Template '" + selectedItem.Name + "' cannot be applied to loan. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          }
          finally
          {
            Cursor.Current = Cursors.Default;
          }
        }
      }
      return false;
    }

    private void validateFHACountyLimit(
      string loanTypeBeforeChanged,
      string loanTypeAfterChange,
      bool enforcedCountyLimitCheck,
      DataTemplate dt,
      LoanProgram lp)
    {
      if (loanTypeBeforeChanged == "FHA" || lp == null && dt == null || !enforcedCountyLimitCheck)
        return;
      bool flag = true;
      if (lp != null && lp.GetSimpleField("1172") == "FHA" && loanTypeAfterChange != "FHA")
        flag = false;
      else if (dt != null && dt.GetSimpleField("1172") == "FHA" && loanTypeAfterChange != "FHA")
        flag = false;
      if (flag)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Loan amt(field: 1109) exceeds county limit. Loan Type cannot be changed to \"FHA\" by the template!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void selectFormList(bool useDefault)
    {
      if (useDefault)
      {
        this.session.LoanData.SetFormListTemplate((FormTemplate) null);
        this.loan.SetCurrentField("2864", "");
        this.loadFormMenu(-1);
      }
      else
      {
        TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, (FileSystemEntry) null, false);
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
        if (this.session.LoanData == null)
          return;
        if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, selectedItem))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The form template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          FormTemplate templateSettings;
          try
          {
            templateSettings = (FormTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.FormList, selectedItem);
            this.loan.SetCurrentField("2864", selectedItem.Path);
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Can't open " + selectedItem.Name + " form template file. Message: " + ex.Message);
            return;
          }
          this.session.LoanData.SetFormListTemplate(templateSettings);
          this.loadFormMenu(-1);
        }
      }
    }

    private bool selectTaskSet()
    {
      try
      {
        UserInfo userInfo = this.session.UserInfo;
        bool flag = true;
        if (UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas))
          flag = false;
        if (!flag || this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_Task))
        {
          if ((this.loan.ContentAccess & LoanContentAccess.Task) != LoanContentAccess.Task)
          {
            if (this.loan.ContentAccess == LoanContentAccess.FullAccess)
              goto label_7;
          }
          else
            goto label_7;
        }
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have rights to add task set.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Cannot check access rights: Error: " + ex.Message);
      }
label_7:
      this.AddToWorkArea((Control) new MilestoneTaskListControl(this.loanMgr, false));
      using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet, (FileSystemEntry) null, false))
      {
        if (templateSelectionDialog.ShowDialog((IWin32Window) this) == DialogResult.OK)
        {
          FileSystemEntry selectedItem = templateSelectionDialog.SelectedItem;
          if (this.session.LoanData != null)
          {
            if (!this.session.ConfigurationManager.TemplateSettingsObjectExists(EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet, selectedItem))
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "The task set template '" + selectedItem.Name + "' has been deleted.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              return false;
            }
            TaskSetTemplate templateSettings;
            try
            {
              templateSettings = (TaskSetTemplate) this.session.ConfigurationManager.GetTemplateSettings(EllieMae.EMLite.ClientServer.TemplateSettingsType.TaskSet, selectedItem);
            }
            catch (Exception ex)
            {
              Tracing.Log(LoanPage.sw, TraceLevel.Error, nameof (LoanPage), "Can't open " + selectedItem.Name + " task set template file. Message: " + ex.Message);
              return false;
            }
            return this.loanMgr.ApplyTaskSetTemplate(templateSettings);
          }
        }
      }
      return false;
    }

    private void toolsFormsTabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void horzSplitter_Paint(object sender, PaintEventArgs e)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEncompassApplication>().GetTipControl().Refresh();
    }

    private void picBoxEmail_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (LoanPage)).GetObject("picBoxEmailOver.Image");
    }

    private void picBoxEmail_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (LoanPage)).GetObject("loEmailPanel.Image");
    }

    private void picBoxPhone_MouseEnter(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (LoanPage)).GetObject("picBoxPhoneOver.Image");
    }

    private void picBoxPhone_MouseLeave(object sender, EventArgs e)
    {
      ((PictureBox) sender).Image = (Image) new ResourceManager(typeof (LoanPage)).GetObject("loPhonePanel.Image");
    }

    internal ToolStripMenuItem[] ServiceMenuItems
    {
      get
      {
        return this.servicesCtl == null ? (ToolStripMenuItem[]) null : this.servicesCtl.GetMenuItems();
      }
    }

    private void Tips_Continue(object sender, TipContinueEventArgs e)
    {
      if (!(e.TipID == "Compliance"))
        return;
      EllieMae.EMLite.RemotingServices.Session.MainScreen.OpenURL("http://portal.elliemae.com/marketing/compliance/compliance.asp", "Compliance Report", 650, 550);
    }

    public bool IsPrintEnabled => this.printBtn.Enabled;

    private void cboBorrowers_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.cboBorrowers.SelectedIndexChanged -= new EventHandler(this.cboBorrowers_SelectedIndexChanged);
      EllieMae.EMLite.DataEngine.BorrowerPair selectedItem = (EllieMae.EMLite.DataEngine.BorrowerPair) this.cboBorrowers.SelectedItem;
      if (!selectedItem.Equals((object) this.loan.CurrentBorrowerPair))
      {
        this.loan.SetBorrowerPair(selectedItem);
        this.InitContentsForNewBorrowerPair(true);
        if (this.loan.Calculator != null)
          this.loan.Calculator.FormCalculation("1163", (string) null, (string) null);
        if (this.loan.LinkedData != null && this.loan.LinkSyncType == LinkSyncType.ConstructionPrimary)
        {
          foreach (EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair in this.loan.LinkedData.GetBorrowerPairs())
          {
            if (string.Compare(borrowerPair.Borrower.FirstName, selectedItem.Borrower.FirstName, true) == 0 && string.Compare(borrowerPair.Borrower.LastName, selectedItem.Borrower.LastName, true) == 0 && string.Compare(borrowerPair.CoBorrower.FirstName, selectedItem.CoBorrower.FirstName, true) == 0 && string.Compare(borrowerPair.CoBorrower.LastName, selectedItem.CoBorrower.LastName, true) == 0)
            {
              this.loan.LinkedData.SetBorrowerPair(borrowerPair);
              break;
            }
          }
        }
      }
      this.cboBorrowers.SelectedIndexChanged += new EventHandler(this.cboBorrowers_SelectedIndexChanged);
    }

    private void onBorrowerNameChanged(string fieldId, string val)
    {
      foreach (EllieMae.EMLite.DataEngine.BorrowerPair borrowerPair in this.loan.GetBorrowerPairs())
      {
        if (borrowerPair.Equals((object) this.loan.CurrentBorrowerPair))
        {
          this.cboBorrowers.Items[this.cboBorrowers.SelectedIndex] = (object) borrowerPair;
          break;
        }
      }
    }

    private void btnEditBorrowerPair_Click(object sender, EventArgs e)
    {
      using (SwapBorrowerPairForm borrowerPairForm = new SwapBorrowerPairForm(this.loan))
      {
        borrowerPairForm.ImportFromLoanClicked += new EventHandler(this.borManager_ImportFromLoanClicked);
        int num = (int) borrowerPairForm.ShowDialog((IWin32Window) null);
      }
      this.RefreshBorrowerPairs();
      this.InitContentsForNewBorrowerPair(false);
    }

    private void addBorrowerPair()
    {
      using (SwapBorrowerPairForm borrowerPairForm = new SwapBorrowerPairForm(this.loan))
        borrowerPairForm.AddBorrowerPair();
      this.RefreshBorrowerPairs();
      this.cboBorrowers.SelectedIndex = this.cboBorrowers.Items.Count - 1;
    }

    private void duplicateLoanCheck()
    {
      EllieMae.EMLite.ClientServer.Address address = new EllieMae.EMLite.ClientServer.Address(this.loan.GetField("11"), "", this.loan.GetField("13"), this.loan.GetField("14"), this.loan.GetField("15"));
      List<LoanDuplicateChecker> loanDuplicates = new List<LoanDuplicateChecker>();
      string duplicateLoanCheck = EllieMae.EMLite.RemotingServices.Session.LoanManager.GetAllIncludeInDuplicateLoanCheck();
      Dictionary<LoanDuplicateChecker.CheckField, string> info1 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
      info1.Add(LoanDuplicateChecker.CheckField.FirstName, this.loan.GetField("4000"));
      info1.Add(LoanDuplicateChecker.CheckField.LastName, this.loan.GetField("4002"));
      info1.Add(LoanDuplicateChecker.CheckField.SSN, this.loan.GetField("65"));
      info1.Add(LoanDuplicateChecker.CheckField.HomePhone, this.loan.GetField("66"));
      info1.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.loan.GetField("1490"));
      info1.Add(LoanDuplicateChecker.CheckField.Email, this.loan.GetField("1240"));
      info1.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.loan.GetField("1178"));
      Dictionary<LoanDuplicateChecker.CheckField, string> info2 = new Dictionary<LoanDuplicateChecker.CheckField, string>();
      info2.Add(LoanDuplicateChecker.CheckField.FirstName, this.loan.GetField("4004"));
      info2.Add(LoanDuplicateChecker.CheckField.LastName, this.loan.GetField("4006"));
      info2.Add(LoanDuplicateChecker.CheckField.SSN, this.loan.GetField("97"));
      info2.Add(LoanDuplicateChecker.CheckField.HomePhone, this.loan.GetField("98"));
      info2.Add(LoanDuplicateChecker.CheckField.MobilePhone, this.loan.GetField("1480"));
      info2.Add(LoanDuplicateChecker.CheckField.Email, this.loan.GetField("1268"));
      info2.Add(LoanDuplicateChecker.CheckField.WorkEmail, this.loan.GetField("1179"));
      bool flag1 = InputHandlerBase.canCheckDuplicate(info1, address);
      bool flag2 = InputHandlerBase.canCheckDuplicate(info2, address);
      List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo = new List<Dictionary<LoanDuplicateChecker.CheckField, string>>();
      if (flag1)
        borrowerInfo.Add(info1);
      if (flag2)
        borrowerInfo.Add(info2);
      if (borrowerInfo.Count > 0)
        loanDuplicates = this.session.ConfigurationManager.GetLoanDuplicateInfo(this.loan.GUID, borrowerInfo, address, duplicateLoanCheck);
      if (loanDuplicates != null && loanDuplicates.Count != 0)
      {
        string str1 = this.session.ConfigurationManager.GetDuplicates(this.loan.GUID);
        foreach (LoanDuplicateChecker duplicateChecker in loanDuplicates)
        {
          string str2 = str1;
          Guid guid = duplicateChecker.GUID;
          string str3 = guid.ToString();
          if (!str2.Contains(str3))
          {
            string str4 = str1;
            guid = duplicateChecker.GUID;
            string str5 = guid.ToString();
            str1 = str4 + str5 + ",";
          }
        }
        this.session.ConfigurationManager.SaveDuplicate(this.loan.GUID, str1.Trim(','));
        using (MatchedLoanDuplicate matchedLoanDuplicate = new MatchedLoanDuplicate(this.session, loanDuplicates))
        {
          int num = (int) matchedLoanDuplicate.ShowDialog();
        }
      }
      else
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Based on the data in this loan file, no duplicate loans were found.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    private void applyMilestoneTemplate()
    {
      new MilestoneTemplatesManager().ApplyMilestoneTemplate(this.session.SessionObjects, this.loan, (ILoanMilestoneTemplateOrchestrator) new StandardMilestoneTemplateApply(this.session, true, true, this.aclMgr.GetUserApplicationRight(AclFeature.LoanTab_DisplayMilestoneChangeScreen)), (MilestoneTemplate) null, "");
    }

    public TriggerEmailTemplate MilestoneTemplateEmailTemplate
    {
      get => this.milestoneTemplateEmailTemplate;
      set => this.milestoneTemplateEmailTemplate = value;
    }

    public bool OpenLogRecord(LogRecordBase log)
    {
      if (this.CurrentForm.Equals("Lock Request Form") && !this.AllowUnloadCurrentForm())
        return false;
      switch (log)
      {
        case DocumentLog _:
          this.ShowDocumentWorksheet(log as DocumentLog);
          break;
        case MilestoneLog _:
          this.ShowMilestoneWorksheet(log as MilestoneLog);
          break;
        case UnderwritingConditionLog _:
          this.ShowConditionWorksheet(log as UnderwritingConditionLog);
          break;
        case ConversationLog _:
          using (ConversationContainer conversationContainer = new ConversationContainer(log as ConversationLog, true))
          {
            int num = (int) conversationContainer.ShowDialog((IWin32Window) this);
          }
          return true;
        case StatusOnlineLog _:
          this.AddToWorkArea((Control) new StatusOnlineLogWS(log as StatusOnlineLog));
          break;
        case HtmlEmailLog _:
          this.AddToWorkArea((Control) new HtmlEmailLogWS(log as HtmlEmailLog));
          break;
        case EDMLog _:
          this.ShowEDMWorksheet(log as EDMLog);
          break;
        case DataTracLog _:
          this.ShowDataTracWorksheet(log as DataTracLog);
          break;
        case SystemLog _:
          this.AddToWorkArea((Control) new SystemLogWS(log as SystemLog));
          break;
        case LockRequestLog _:
          if (((LockRequestLog) log).IsLockCancellation)
          {
            this.AddToWorkArea((Control) new LockCancellationWS(this.session, log as LockRequestLog));
            break;
          }
          this.AddToWorkArea((Control) new LockRequestWS(this.session, log as LockRequestLog));
          break;
        case LockVoidLog _:
          this.AddToWorkArea((Control) new LockVoidWS(this.session, log as LockVoidLog));
          break;
        case LockRemovedLog _:
          this.AddToWorkArea((Control) new LockRemovedWS(this.session, log as LockRemovedLog));
          break;
        case LockConfirmLog _:
          this.AddToWorkArea((Control) new LockRequestWS(this.session, log as LockConfirmLog));
          break;
        case LockDenialLog _:
          this.AddToWorkArea((Control) new LockDenialWS(this.session, log as LockDenialLog));
          break;
        case LockCancellationLog _:
          this.AddToWorkArea((Control) new LockCancellationWS(this.session, log as LockCancellationLog));
          break;
        case RegistrationLog _:
          this.AddToWorkArea((Control) new RegistrationLogWS(log as RegistrationLog));
          break;
        case ServicingPrintLog _:
          this.AddToWorkArea((Control) new ServicingPrintWS(log as ServicingPrintLog));
          break;
        case LogEntryLog _:
          this.AddToWorkArea((Control) new LogEntryWS(log as LogEntryLog));
          break;
        case MilestoneTaskLog _:
          this.ShowTaskWorksheet(log as MilestoneTaskLog);
          break;
        case PrintLog _:
          Control newControl1 = (Control) new PrintLogWS(log as PrintLog);
          newControl1.Dock = DockStyle.Fill;
          this.AddToWorkArea(newControl1);
          break;
        case ExportLog _:
          Control newControl2 = (Control) new ExportLogWS(log as ExportLog);
          newControl2.Dock = DockStyle.Fill;
          this.AddToWorkArea(newControl2);
          break;
        case GetIndexLog _:
          Control newControl3 = (Control) new PrintIndexLogWS(log as GetIndexLog);
          newControl3.Dock = DockStyle.Fill;
          this.AddToWorkArea(newControl3);
          break;
        case EmailTriggerLog _:
          this.AddToWorkArea((Control) new EmailTriggerLogWS(log as EmailTriggerLog));
          break;
        case UnderwritingConditionLog _:
          this.ShowConditionWorksheet(log as UnderwritingConditionLog);
          break;
        case PreliminaryConditionLog _:
          this.ShowConditionWorksheet(log as PreliminaryConditionLog);
          break;
        case PostClosingConditionLog _:
          this.ShowConditionWorksheet(log as PostClosingConditionLog);
          break;
        case SellConditionLog _:
          this.ShowConditionWorksheet(log as SellConditionLog);
          break;
        case DisclosureTrackingLog _:
          if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking) || (this.loan.ContentAccess & LoanContentAccess.DisclosureTracking) != LoanContentAccess.DisclosureTracking && (this.loan.ContentAccess & LoanContentAccess.DisclosureTrackingViewOnly) != LoanContentAccess.DisclosureTrackingViewOnly && this.loan.ContentAccess != LoanContentAccess.FullAccess)
            return false;
          this.AddToWorkArea((Control) new DisclosureTrackingWS(log as DisclosureTrackingLog, true, this.session));
          break;
        case DocumentOrderLog _:
          this.AddToWorkArea((Control) new DocOrderLogWS(this.session, log as DocumentOrderLog));
          break;
        case ECloseLog _:
          this.AddToWorkArea((Control) new DocOrderLogWS(this.session, log as ECloseLog));
          break;
        case MilestoneHistoryLog _:
          this.AddToWorkArea((Control) new MilestoneHistoryChangeSheet(log as MilestoneHistoryLog));
          break;
        case DisclosureTracking2015Log _:
          if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking) || (this.loan.ContentAccess & LoanContentAccess.DisclosureTracking) != LoanContentAccess.DisclosureTracking && (this.loan.ContentAccess & LoanContentAccess.DisclosureTrackingViewOnly) != LoanContentAccess.DisclosureTrackingViewOnly && this.loan.ContentAccess != LoanContentAccess.FullAccess)
            return false;
          this.AddToWorkArea((Control) new DisclosureTracking2015WS((IDisclosureTracking2015Log) (log as DisclosureTracking2015Log), true, this.session));
          break;
        case GoodFaithFeeVarianceCureLog _:
          this.AddToWorkArea((Control) new GoodFaithFeeVarianceCureLogWS(log as GoodFaithFeeVarianceCureLog));
          break;
        default:
          return false;
      }
      this.emToolMenuBox.SelectedIndex = -1;
      this.emFormMenuBox.SelectedIndex = -1;
      return true;
    }

    public void OpenMilestoneLogReview(MilestoneLog log, MilestoneHistoryLog historyLog)
    {
      this.ShowMilestoneWorksheetReview(log, historyLog);
    }

    public void OpenMilestoneLogReview(MilestoneLog log)
    {
      this.ShowMilestoneWorksheetReview(log, (MilestoneHistoryLog) null);
    }

    public MilestoneTaskLog ShowTaskWorksheet(MilestoneTaskLog log)
    {
      MilestoneTaskWorksheetContainer worksheetContainer = log == null ? new MilestoneTaskWorksheetContainer(new MilestoneTaskLog(this.session.UserInfo, "", ""), true) : new MilestoneTaskWorksheetContainer(log, true);
      if (worksheetContainer.ShowDialog((IWin32Window) Form.ActiveForm) != DialogResult.OK)
        return (MilestoneTaskLog) null;
      if (this.topControl is MilestoneTaskListControl)
        ((MilestoneTaskListControl) this.topControl).RefreshContents(worksheetContainer.TaskLog);
      else if (this.topControl is MilestoneWS)
        ((MilestoneWS) this.topControl).RefreshTask(worksheetContainer.TaskLog);
      return worksheetContainer.TaskLog;
    }

    public void ShowDocumentWorksheet(DocumentLog log)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, log);
    }

    public void ShowConditionWorksheet(PostClosingConditionLog log)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, log);
    }

    public void ShowConditionWorksheet(SellConditionLog log)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, log);
    }

    public void ShowConditionWorksheet(UnderwritingConditionLog log)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, log);
    }

    public void ShowConditionWorksheet(PreliminaryConditionLog log)
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<IEFolder>().View(this.session.LoanDataMgr, log);
    }

    public void ShowEDMWorksheet(EDMLog log)
    {
      this.AddToWorkArea((Control) new EDMLogControl(log));
    }

    public void ShowDataTracWorksheet(DataTracLog log)
    {
      this.AddToWorkArea((Control) new DataTracLogControl(log));
    }

    public void ShowMilestoneWorksheet(MilestoneLog msLog)
    {
      if (this.milestoneWS != null)
        this.milestoneWS.UnRegisterEvent();
      this.milestoneWS = new MilestoneWS(this.session, msLog);
      this.AddToWorkArea((Control) this.milestoneWS, true);
    }

    public void ShowMilestoneWorksheetReview(MilestoneLog msLog, MilestoneHistoryLog historyLog)
    {
      Form form = new Form();
      form.Size = new Size(900, 700);
      form.StartPosition = FormStartPosition.CenterScreen;
      form.Text = "Existing Milestone Worksheet Details";
      form.FormBorderStyle = FormBorderStyle.FixedDialog;
      form.MinimizeBox = false;
      form.Icon = (Icon) new ComponentResourceManager(typeof (MainForm)).GetObject("$this.Icon");
      MilestoneWS milestoneWs = historyLog != null ? new MilestoneWS(this.session, msLog, historyLog) : new MilestoneWS(this.session, msLog);
      milestoneWs.Parent = (Control) form;
      foreach (Control control1 in (ArrangedElementCollection) milestoneWs.Controls)
      {
        if (control1.Name.Equals("topPanel"))
        {
          foreach (Control control2 in (ArrangedElementCollection) control1.Controls)
          {
            if (!control2.Name.Equals("label1"))
              control2.Enabled = false;
          }
        }
        else
          control1.Enabled = false;
      }
      form.Controls.Add((Control) milestoneWs);
      form.KeyDown += (KeyEventHandler) ((o, e) =>
      {
        if (e.KeyCode != Keys.Escape)
          return;
        form.Close();
      });
      int num = (int) form.ShowDialog();
    }

    public void ShoweDisclosureTrackingRecord(string packageID)
    {
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking))
        return;
      if (this.loan.GetField("3969") == "RESPA 2010 GFE and HUD-1" || this.loan.GetField("3969") == "Old GFE and HUD-1")
      {
        this.AddToWorkArea((Control) new DisclosureTrackingWS(packageID, true, this.session), true);
      }
      else
      {
        if (!Utils.CheckIf2015RespaTila(this.session.LoanDataMgr.LoanData.GetField("3969")))
          return;
        this.AddToWorkArea((Control) new DisclosureTracking2015WS(packageID, true, this.session), true);
      }
    }

    public void ShoweDisclosureTrackingRecord(
      DisclosureTrackingBase selectedLog,
      bool clearNotification)
    {
      if (!this.aclMgr.GetUserApplicationRight(AclFeature.ToolsTab_DisclosureTracking))
        return;
      if (this.loan.GetField("3969") == "RESPA 2010 GFE and HUD-1" || this.loan.GetField("3969") == "Old GFE and HUD-1")
      {
        this.AddToWorkArea((Control) new DisclosureTrackingWS((DisclosureTrackingLog) selectedLog, clearNotification, this.session));
      }
      else
      {
        if (!Utils.CheckIf2015RespaTila(this.session.LoanDataMgr.LoanData.GetField("3969")))
          return;
        this.AddToWorkArea((Control) new DisclosureTracking2015WS((IDisclosureTracking2015Log) selectedLog, clearNotification, this.session));
      }
    }

    public void ShowAIQAnalyzerMessage(
      string analyzerType,
      DateTime alertDateTime,
      string description,
      string messageID)
    {
      AnalyzerMessageControl newControl = new AnalyzerMessageControl(analyzerType + " Analyzer Message", description, alertDateTime, 1, this.session, this.session.LoanData, messageID);
      this.AddToWorkArea((Control) newControl, true);
      if (!(analyzerType == "Income"))
        return;
      newControl.RefreshIncomePage();
    }

    public void ShowAUSTrackingTool()
    {
      this.AddToWorkArea((Control) new AUSTrackingTool(this.session, this.session.LoanData), true);
    }

    public void ShowPlanCodeComparison(string fieldId, DocumentOrderType orderType)
    {
      if (PlanCodeConflictDialog.CurrentInstance != null && !PlanCodeConflictDialog.CurrentInstance.IsDisposed)
      {
        PlanCodeConflictDialog.CurrentInstance.BringToFront();
      }
      else
      {
        try
        {
          using (PlanCodeConflictDialog codeConflictDialog = new PlanCodeConflictDialog((IHtmlInput) this.loan, orderType))
          {
            int num = (int) codeConflictDialog.ShowDialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.Application);
          }
        }
        catch (InvalidPlanCodeException ex)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The plan code '" + ex.PlanCode + "' is no long valid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        this.RefreshContents();
      }
    }

    public void RefreshLoan()
    {
      this.InitContents(this.session.LoanDataMgr.IsNew(), !this.session.LoanDataMgr.Writable);
    }

    public void Unload()
    {
      if (this.GetFormScreen() is IInputHandler formScreen)
        formScreen.Unload();
      this.freeScreen.Unload();
      this.clearWorkArea();
      this.ctlOffers.ReleaseLoan();
      this.pnlLoanInfo.ReleaseLoan();
      this.servicesCtl.ReleaseLoan();
      this.logPanel.ReleaseLoan();
      if (this.vodPan != null)
      {
        this.vodPan.Dispose();
        this.vodPan = (VODPanel) null;
      }
      if (this.voggPan != null)
      {
        this.voggPan.Dispose();
        this.voggPan = (VOGGPanel) null;
      }
      if (this.vooiPan != null)
      {
        this.vooiPan.Dispose();
        this.vooiPan = (VOOIPanel) null;
      }
      if (this.vooaPan != null)
      {
        this.vooaPan.Dispose();
        this.vooaPan = (VOOAPanel) null;
      }
      if (this.voePan != null)
      {
        this.voePan.Dispose();
        this.voePan = (VOEPanel) null;
      }
      if (this.volPan != null)
      {
        this.volPan.Dispose();
        this.volPan = (VOLPanel) null;
      }
      if (this.voolPan != null)
      {
        this.voolPan.Dispose();
        this.voolPan = (VOOLPanel) null;
      }
      if (this.vomPan != null)
      {
        this.vomPan.Dispose();
        this.vomPan = (VOMPanel) null;
      }
      if (this.vorPan != null)
      {
        this.vorPan.Dispose();
        this.vorPan = (VORPanel) null;
      }
      if (this.voalPan != null)
      {
        this.voalPan.Dispose();
        this.voalPan = (VOALPanel) null;
      }
      if (this.tax4506Pan != null)
      {
        this.tax4506Pan.Dispose();
        this.tax4506Pan = (TAX4506Panel) null;
      }
      if (this.tax4506TPan != null)
      {
        this.tax4506TPan.Dispose();
        this.tax4506TPan = (TAX4506TPanel) null;
      }
      this.loanBizInfo = (LoanBusinessRuleInfo) null;
    }

    public void StartConversation(ConversationLog con)
    {
      if (con.IsEmail)
        SystemUtil.ShellExecute("mailto:" + con.Email);
      using (ConversationContainer conversationContainer = new ConversationContainer(con, false))
      {
        int num = (int) conversationContainer.ShowDialog();
      }
    }

    public void SendLockRequest(bool closeLoan)
    {
      if (this.session.LoanDataMgr == null)
        return;
      if (!this.session.LoanDataMgr.Writable)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You opened this loan in read-only mode. You cannot create a lock request.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        this.session.LoanDataMgr.CreateRateLockRequest();
        this.RefreshLogPanel();
        ILoanConsole service = EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanConsole>();
        if (!service.SaveLoan() || !closeLoan)
          return;
        service.CloseLoan(false);
      }
    }

    public bool Print(string[] formNames)
    {
      List<FormItemInfo> formItemInfoList = new List<FormItemInfo>();
      foreach (string formName in formNames)
      {
        if ((formName ?? "") != "")
        {
          FormItemInfo formItemInfo;
          if (FileSystemEntry.IsValidPath(formName, this.session.UserID))
          {
            formItemInfo = new FormItemInfo(nameof (Print), formName, OutputFormType.CustomLetters);
          }
          else
          {
            formItemInfo = new FormItemInfo(nameof (Print), OutputFormNameMap.GetFormNameToKey(formName, this.session), OutputFormType.PdfForms);
            string key = formName;
            string str = (string) null;
            if (formName.Trim() == "SSA-89 Social Security Number Verification (Borrower)")
            {
              key = "SSA-89 Social Security Number Verification";
              str = "Borrower";
            }
            if (formName.Trim() == "SSA-89 Social Security Number Verification (Coborrower)")
            {
              key = "SSA-89 Social Security Number Verification";
              str = "Coborrower";
            }
            else if (formName.Trim() == "W-8BEN Certificate of Foreign Status of Beneficial Owner (Borrower)")
            {
              key = "W-8BEN Certificate of Foreign Status of Beneficial Owner";
              str = "Borrower";
            }
            else if (formName.Trim() == "W-8BEN Certificate of Foreign Status of Beneficial Owner (Coborrower)")
            {
              key = "W-8BEN Certificate of Foreign Status of Beneficial Owner";
              str = "Coborrower";
            }
            if (OutputFormNameMap.AllEdsFormsAndParams.ContainsKey(key))
            {
              formItemInfo.MergeLocation = PrintForm.MergeLocationValues.EDS;
              formItemInfo.MergeParams = OutputFormNameMap.AllEdsFormsAndParams[key];
              if (key.Trim() != formName.Trim())
              {
                formItemInfo.MergeParams["BaseName"] = key;
                formItemInfo.MergeParams["BorrowerOrCoborrower"] = str;
              }
            }
            switch (formName.Trim())
            {
              case "Post-Closing Condition Detail":
              case "Preliminary Condition Detail":
              case "Underwriting Condition Detail":
                formItemInfo.MergeParams["Guid"] = "";
                break;
              case "VOAL":
              case "VOD":
              case "VOGG":
              case "VOL":
              case "VOM":
              case "VOOA":
              case "VOOI":
              case "VOOL":
                formItemInfo.MergeParams["Index"] = "1";
                break;
              case "VOE":
              case "VOR":
                formItemInfo.MergeParams["Index"] = "1";
                formItemInfo.MergeParams["BorrowerOrCoborrower"] = "Borrower";
                break;
            }
          }
          formItemInfoList.Add(formItemInfo);
        }
      }
      if (formItemInfoList.Count == 0)
        return false;
      new PdfFormFacade(this.session.LoanDataMgr, (Form) this)
      {
        EntityList = new EllieMae.EMLite.LoanServices.Bam(this.session.LoanDataMgr).GetCompanyDisclosureEntities()
      }.ProcessForms(formItemInfoList.ToArray(), PdfFormPrintOptions.WithData, nameof (Print));
      return true;
    }

    public bool ShowRegulationAlerts()
    {
      return RegulationAlertDialog.DisplayAlerts((IWin32Window) MainForm.Instance) == DialogResult.OK;
    }

    public bool ShowRegulationAlertsOrderDoc()
    {
      return this.loan.GetField("1172") == "HELOC" || RegulationAlertDialog.DisplayAlerts((IWin32Window) MainForm.Instance) == DialogResult.OK;
    }

    private void chkAlphaForms_CheckedChanged(object sender, EventArgs e)
    {
      this.emFormMenuBox.ListDisplayMode = this.chkAlphaForms.Checked ? EMFormMenu.DisplayMode.Alphabetical : EMFormMenu.DisplayMode.Default;
    }

    private void chkAlphaTools_CheckedChanged(object sender, EventArgs e)
    {
      this.emToolMenuBox.ListDisplayMode = this.chkAlphaTools.Checked ? EMFormMenu.DisplayMode.Alphabetical : EMFormMenu.DisplayMode.Default;
    }

    private void eFolderBtn_Click(object sender, EventArgs e)
    {
      this.ApplyOnDemandBusinessRules();
      eFolderDialog.ShowInstance(this.session);
    }

    private void aiqAnalyzers_Click(object sender, EventArgs e) => this.LaunchAIQIncomeAnalyzer();

    public void LaunchAIQIncomeAnalyzer()
    {
      if (this.aiqHelper == null || !QuickButtonsControl.ValidateButtonFieldDataEntryRule("BUTTON_AIQANALYZERS", this.loan))
        return;
      this.aiqHelper.btnClick_action(this.loan.GUID);
    }

    private async void btnOpenWebView_ClickAsync(object sender, EventArgs e)
    {
      await DeepLinkLauncher.LaunchWebAppInBrowserAsync(DeepLinkType.LoanDefaultPage, (IDeepLinkContext) new LoanPageContext());
    }

    private void btnSearchAllRegs_Click(object sender, EventArgs e)
    {
      string allRegsUrl = EllieMae.EMLite.RemotingServices.Session.DefaultInstance.StartupInfo.AllRegsUrl;
      SSFContext context = SSFContext.Create(EllieMae.EMLite.RemotingServices.Session.LoanDataMgr, SSFHostType.Network, new SSFGuest()
      {
        uri = allRegsUrl,
        scope = "sc"
      });
      if (context == null)
        return;
      context.parameters = new Dictionary<string, object>()
      {
        {
          "hostname",
          (object) "smartclient"
        },
        {
          "instanceId",
          (object) EllieMae.EMLite.RemotingServices.Session.Connection?.Server?.InstanceName
        },
        {
          "errorMessages",
          (object) new List<string>()
        }
      };
      using (SSFDialog ssfDialog = new SSFDialog(context))
      {
        ssfDialog.Text = "AllRegs Search";
        ssfDialog.Height = Convert.ToInt32((double) this.Height * 0.9);
        ssfDialog.Width = Convert.ToInt32((double) this.Width * 0.9);
        ssfDialog.ShowDialog((IWin32Window) EllieMae.EMLite.RemotingServices.Session.MainForm);
      }
    }

    public void RefreshSynchButton()
    {
      if (!this.Visible || !this.session.SessionObjects.AllowConcurrentEditing || this.session.LoanDataMgr == null)
        return;
      bool enabled = this.stdIconBtnSynch.Enabled;
      this.stdIconBtnSynch.Enabled = this.session.LoanDataMgr.Writable && this.session.LoanDataMgr.IsLoanFileOnServerNewer(false);
    }

    public void RefreshCE()
    {
      if (!this.Visible || !this.session.SessionObjects.AllowConcurrentEditing || this.session.LoanDataMgr == null)
        return;
      if (this.session.LoanDataMgr.IsNew())
      {
        this.textBoxCEAllowedBlocked.Text = "New Loan (Not Saved Yet)";
        this.stdIconBtnLock.Visible = false;
        this.textBoxCEAllowedBlocked.Visible = true;
        this.stdIconBtnIM.Visible = false;
        this.cmbBoxCEUsers.Visible = false;
      }
      else
      {
        UserShortInfoList workingOnTheLoan = this.session.LoanDataMgr.GetUsersWorkingOnTheLoan((string) null, true);
        UserShortInfo userShortInfo1 = (UserShortInfo) null;
        int num = 0;
        if (workingOnTheLoan != null)
        {
          num = workingOnTheLoan.Count;
          userShortInfo1 = workingOnTheLoan.GetUserInfo(this.session.SessionObjects.SessionID);
          if (userShortInfo1 != null)
            --num;
        }
        bool flag1 = num == 0;
        if (userShortInfo1 != null && (userShortInfo1.Exclusive == LockInfo.ExclusiveLock.Both || userShortInfo1.Exclusive == LockInfo.ExclusiveLock.Exclusive))
        {
          this.stdIconBtnLock.StandardButtonType = StandardIconButton.ButtonType.AllowCEButton;
          this.textBoxCEAllowedBlocked.Text = "Multi-User Editing Blocked";
        }
        else
        {
          this.stdIconBtnLock.StandardButtonType = StandardIconButton.ButtonType.BlockCEButton;
          this.textBoxCEAllowedBlocked.Text = "Multi-User Editing Allowed";
        }
        bool flag2 = this.session.LoanDataMgr.Writable & flag1 && this.canExclusiveLock;
        bool flag3 = this.session.LoanDataMgr.Writable && !flag1;
        this.verticalSeparatorCE.Visible = flag2 | flag3;
        this.textBoxCEAllowedBlocked.Visible = false;
        this.stdIconBtnLock.Visible = flag2;
        this.textBoxCEAllowedBlocked.Visible = flag2;
        this.stdIconBtnIM.Visible = flag3;
        this.cmbBoxCEUsers.Visible = flag3;
        if (!this.session.LoanDataMgr.Writable)
          return;
        this.cmbBoxCEUsers.Items.Clear();
        if (flag1)
          return;
        List<UserShortInfo> userShortInfoList = new List<UserShortInfo>((IEnumerable<UserShortInfo>) workingOnTheLoan.UserShortInfos);
        userShortInfoList.Sort();
        foreach (UserShortInfo userShortInfo2 in userShortInfoList.ToArray())
        {
          if (userShortInfo2.SessionID != this.session.SessionObjects.SessionID)
            this.cmbBoxCEUsers.Items.Add((object) userShortInfo2);
        }
        this.cmbBoxCEUsers.SelectedIndex = 0;
      }
    }

    public void HandleOtherUserOpenCloseLoan(string message)
    {
      if (!this.session.SessionObjects.AllowConcurrentEditing)
        return;
      this.RefreshCE();
      BalloonToolTip ceBalloonToolTip = MainScreen.Instance.GetCEBalloonToolTip();
      if (ceBalloonToolTip.Visible)
        ceBalloonToolTip.Close();
      BalloonToolTip updatesBalloonToolTip = MainScreen.Instance.GetCEGetUpdatesBalloonToolTip();
      if (updatesBalloonToolTip.Visible)
        updatesBalloonToolTip.Close();
      Control controlToReference = (Control) null;
      if (this.cmbBoxCEUsers.Visible)
        controlToReference = (Control) this.cmbBoxCEUsers;
      else if (this.textBoxCEAllowedBlocked.Visible)
        controlToReference = (Control) this.textBoxCEAllowedBlocked;
      else if (this.stdIconBtnLock.Visible)
        controlToReference = (Control) this.stdIconBtnLock;
      else if (this.eFolderBtn.Visible)
        controlToReference = (Control) this.eFolderBtn;
      if (controlToReference == null)
        return;
      ceBalloonToolTip.ShowTip(controlToReference, message, TipPointerPosition.TopRight);
    }

    private void stdIconBtnLock_Click(object sender, EventArgs e)
    {
      if (this.stdIconBtnLock.StandardButtonType == StandardIconButton.ButtonType.BlockCEButton)
      {
        if (!this.session.LoanDataMgr.LockLoanWithExclusive(true, "Error obtaining an exclusive lock on the loan."))
          return;
        this.toolTip.SetToolTip((Control) this.stdIconBtnLock, "Allow Multi-User Editing");
        this.stdIconBtnLock.StandardButtonType = StandardIconButton.ButtonType.AllowCEButton;
        this.textBoxCEAllowedBlocked.Text = "Multi-User Editing Blocked";
      }
      else
      {
        this.session.LoanDataMgr.ReleaseExclusiveLock();
        this.stdIconBtnLock.StandardButtonType = StandardIconButton.ButtonType.BlockCEButton;
        this.toolTip.SetToolTip((Control) this.stdIconBtnLock, "Block Multi-User Editing");
        this.textBoxCEAllowedBlocked.Text = "Multi-User Editing Allowed";
      }
    }

    private void stdIconBtnIM_Click(object sender, EventArgs e)
    {
      UserShortInfo selectedItem = (UserShortInfo) this.cmbBoxCEUsers.SelectedItem;
      ChatWindow chatWindow = new ChatWindow(selectedItem.SessionID, selectedItem.Userid);
      ChatWindow.AddChatWindow(selectedItem.SessionID, chatWindow);
      chatWindow.Show();
    }

    private void LoanPage_VisibleChanged(object sender, EventArgs e) => this.refreshCE();

    private void flowLayoutPanel1_DoubleClick(object sender, EventArgs e) => this.refreshCE();

    private void refreshCE()
    {
      this.RefreshSynchButton();
      this.RefreshCE();
    }

    public void SaveLoan()
    {
      if (!this.saveBtn.Enabled || !this.saveBtn.Visible)
        return;
      this.saveBtn_Click((object) null, (EventArgs) null);
    }

    private void exportToDataTrac()
    {
      EllieMae.EMLite.RemotingServices.Session.Application.GetService<ILoanServices>()?.OrderDataTrac();
    }

    private void refreshDT2015Log(object sender, EventArgs e)
    {
      new DisclosureDetailsDialog2015((DisclosureTracking2015Log) sender, true).RefreshData();
    }

    private void RemoveLockSnapshotForQATest(string clientId)
    {
      if (clientId != "3010000024")
        return;
      try
      {
        if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QA-LostSnapshotRecapture.txt")) || !this.AllowLostSnapshotValidation(clientId))
          return;
        LockConfirmLog[] allConfirmLocks = this.loanMgr.LoanData.GetLogList().GetAllConfirmLocks();
        if (allConfirmLocks.Length != 0)
        {
          LockConfirmLog lockConfirmLog = allConfirmLocks[allConfirmLocks.Length - 1];
          this.loanMgr.LoanData.GetLogList().RemoveRecord((LogRecordBase) allConfirmLocks[allConfirmLocks.Length - 1]);
          this.loanMgr.LoanData.GetLogList().RemoveRecord((LogRecordBase) this.loanMgr.LoanData.GetLogList().GetLockRequest(lockConfirmLog.RequestGUID));
        }
        else
        {
          LockRequestLog[] allLockRequests = this.loanMgr.LoanData.GetLogList().GetAllLockRequests();
          if (allLockRequests.Length == 0)
            return;
          this.loanMgr.LoanData.GetLogList().RemoveRecord((LogRecordBase) allLockRequests[allLockRequests.Length - 1]);
        }
      }
      catch (Exception ex)
      {
      }
    }

    private bool AllowLostSnapshotValidation(string clientId)
    {
      string str = (string) null;
      try
      {
        str = this.SmartClientAttributeFetchTask.Result;
      }
      catch (Exception ex)
      {
      }
      return !string.IsNullOrEmpty(str) && !(str.Trim() == "0") && this.isEnableLockSnapshotRecapture;
    }

    private void showApplyLoanTemplateProgress()
    {
      new Thread(new ParameterizedThreadStart(this.threadStart))
      {
        IsBackground = true
      }.Start((object) "Please wait. Applying loan template is in progress.");
    }

    private void threadStart(object message)
    {
      this.statusReport = new StatusReport(string.Concat(message));
      this.statusReport.Text = "Applying loan template";
      Application.Run((Form) this.statusReport);
    }

    private void closeProgress()
    {
      if (this.statusReport == null)
        return;
      if (this.statusReport.InvokeRequired)
      {
        this.statusReport.Invoke((Delegate) new MethodInvoker(this.closeProgress));
      }
      else
      {
        try
        {
          this.statusReport.Close();
        }
        catch
        {
        }
      }
    }

    public string[] SelectLinkAndSyncTemplate()
    {
      SyncTemplate syncTemplate = (SyncTemplate) null;
      List<SyncTemplate> allSyncTemplates = this.session.ConfigurationManager.GetAllSyncTemplates();
      if (allSyncTemplates == null || allSyncTemplates.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this.session.MainForm, "You don't have a Sync Template in your setting. Please create a Sync Template first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return (string[]) null;
      }
      if (allSyncTemplates != null && allSyncTemplates.Count > 1)
      {
        using (TemplateSelectionDialog templateSelectionDialog = new TemplateSelectionDialog(this.session, allSyncTemplates))
        {
          if (templateSelectionDialog.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
            return (string[]) null;
          syncTemplate = templateSelectionDialog.SelectedSyncTemplate;
        }
      }
      else
        syncTemplate = allSyncTemplates[0];
      if (syncTemplate.SyncFields == null || syncTemplate.SyncFields.Count == 0)
      {
        syncTemplate.AddFields(Utils.LoadPiggybackDefaultSyncFields((IWin32Window) this.session.MainForm, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList.xml", SystemSettings.LocalAppDir)));
        if (this.session.StartupInfo.AllowURLA2020)
          syncTemplate.AddURLA2020Fields(Utils.LoadPiggybackDefaultSyncFields((IWin32Window) null, AssemblyResolver.GetResourceFileFullPath(SystemSettings.DocDirRelPath + "PiggybackDefaultList20.xml", SystemSettings.LocalAppDir)));
      }
      return syncTemplate.SyncFields.ToArray();
    }

    private void AmortSchDialog_OnExportClicked(object sender, EventArgs e)
    {
      GridView gridView = (GridView) sender;
      try
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddHeaderColumn("Pmt#", "0");
        excelHandler.AddHeaderColumn("Pmt Date", "m/d/yyyy");
        excelHandler.AddHeaderColumn("Rate", "0.000");
        excelHandler.AddHeaderColumn("Payment", "0.00");
        excelHandler.AddHeaderColumn("Principal", "0.00");
        excelHandler.AddHeaderColumn("Interest", "0.00");
        excelHandler.AddHeaderColumn("MI", "0.00");
        excelHandler.AddHeaderColumn("Balance", "0.00");
        for (int nItemIndex1 = 0; nItemIndex1 < gridView.Items.Count; ++nItemIndex1)
        {
          string[] data = new string[gridView.Columns.Count];
          for (int nItemIndex2 = 0; nItemIndex2 < gridView.Columns.Count; ++nItemIndex2)
            data[nItemIndex2] = gridView.Items[nItemIndex1].SubItems[nItemIndex2].Text;
          excelHandler.AddDataRow(data);
        }
        excelHandler.CreateExcel();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, nameof (LoanPage), TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the Amortization Payment Schedule to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private bool correspondentChannelEmailPopupDisable(string channelValue)
    {
      bool result;
      bool.TryParse(EllieMae.EMLite.RemotingServices.Session.ConfigurationManager.GetCompanySetting("Policies", "DisableEmailPopup"), out result);
      return result && channelValue.ToLower() == "correspondent";
    }

    public DialogResult OpenModal(string openModalOptions)
    {
      DialogResult dialogResult = DialogResult.None;
      OpenModalOptionsModel modalOptionsModel1 = JsonConvert.DeserializeObject<OpenModalOptionsModel>(openModalOptions);
      switch (modalOptionsModel1.name.ToLower())
      {
        case "loan audit":
          AuditModalOptionsModel modalOptionsModel2 = JsonConvert.DeserializeObject<AuditModalOptionsModel>(openModalOptions);
          using (ThinThickEditorDialog dialog = new ThinThickEditorDialog(EllieMae.EMLite.RemotingServices.Session.DefaultInstance, EllieMae.EMLite.RemotingServices.Session.LoanData.GUID, modalOptionsModel2.loanAuditData))
          {
            dialogResult = (DialogResult) this.Invoke((Delegate) (() => dialog.ShowDialog((IWin32Window) Form.ActiveForm)));
            break;
          }
        case "econsent":
          if (modalOptionsModel1.modalData.action.ToLower() == "notifyadditionalusers")
          {
            NotifyUsersDialog notifyUsersDialog = new NotifyUsersDialog(new List<LoanDisplayInfo>()
            {
              new LoanDisplayInfo()
              {
                LoanGuid = new Guid(EllieMae.EMLite.RemotingServices.Session.LoanData.GUID)
              }
            }, false);
            using (NotifyUsersDialog dialog = notifyUsersDialog)
            {
              dialogResult = (DialogResult) this.Invoke((Delegate) (() => dialog.ShowDialog((IWin32Window) Form.ActiveForm)));
              break;
            }
          }
          else
            break;
      }
      return dialogResult;
    }

    public void RedirectToUrl(string targetName)
    {
      if (string.Compare(targetName, "Lock Comparison Tool", true) != 0)
        return;
      Tracing.Log(LoanPage.sw, nameof (LoanPage), TraceLevel.Info, "Redirect to : " + targetName + "for loan guid : " + this.session.LoanData.GUID);
      try
      {
        this.BeginInvoke((Delegate) (() => this.OpenForm(targetName)));
      }
      catch (Exception ex)
      {
        Tracing.Log(LoanPage.sw, nameof (LoanPage), TraceLevel.Error, "OpenLoan failed: " + ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "The following error occurred when trying to redirect to:\n\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }
  }
}
