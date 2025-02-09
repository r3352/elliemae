// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DashBoard.DashboardTemplateControl
// Assembly: DashBoard, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 99BFBD49-67F8-470C-81BC-FC4FAEA6C933
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DashBoard.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Dashboard;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.DashBoard
{
  public class DashboardTemplateControl : UserControl
  {
    private static readonly string sw = Tracing.SwOutsideLoan;
    private const string dashboardCategory = "Dashboard";
    private const string maxBarsSetting = "Dashboard.MaxBarChartBars";
    private const string maxLinesSetting = "Dashboard.MaxTrendChartLines";
    private const string maxRowsSetting = "Dashboard.MaxLoanTableRows";
    private const string maxColumnsSetting = "Dashboard.MaxLoanTableColumns";
    private const string maxGroupBySetting = "Dashboard.MaxUserTableGroups";
    private int minBarChartBars = 1;
    private int maxBarChartBars = 50;
    private int minTrendChartLines = 1;
    private int maxTrendChartLines = 10;
    private int minLoanTableRows = 1;
    private int maxLoanTableRows = 1000;
    private int maxLoanTableColumns = 20;
    private int maxUserTableGroups = 10;
    private DashboardTemplateControl.ProcessingMode processingMode;
    private DashboardIFSExplorer ifsExplorer;
    private FileSystemEntry fileSystemEntry;
    private DashboardTemplate dashboardTemplate;
    private SnapshotBaseControl snapshot;
    private LoanReportFieldDefs fieldDefinitions;
    private DashboardTemplateValidator templateValidator;
    private bool hasPublicRight;
    private bool hasPrivateRight;
    private DashboardChartType currentChartType = DashboardChartType.None;
    private bool isViewGlobal;
    private bool isTemplateGlobal;
    private bool isViewReadOnly;
    private bool isTemplateReadOnly;
    private bool dataChanged;
    private bool showPublicOnly;
    private bool isPublicTemplate;
    private Sessions.Session session;
    private Panel pnlExplorer;
    private Panel pnlRight;
    private FSExplorer fsExplorer;
    private TabControl tabDefineTemplate;
    private TabPage tpgSnapshot;
    private Button btnCancel;
    private TabPage tpgFolders;
    private TabPage tpgFilters;
    private Panel pnlFilters;
    private AdvancedSearchControl ctlAdvancedSearch;
    private Panel pnlLeft;
    private Button btnExplorerCancel;
    private Button btnExplorerOk;
    private SelectFolderControl ctlSelectFolder;
    private ImageList imageList;
    private IContainer components;
    private Panel pnlLoanTable;
    private Label lblMaxRowsStartLT;
    private NumericUpDown nudMaxRowsLT;
    private Panel pnlBarChart;
    private Panel pnlUserTable;
    private Label lblUserTable;
    private Panel pnlTrendChart;
    private ComboBox cboChartType;
    private Label lblChartType;
    private LoanFieldSelectionControl ctlFieldSelectionLT;
    private Label lblMaxBarsStartBC;
    private NumericUpDown nudMaxBarsBC;
    private ComboBox cboSubsetTypeBC;
    private Label lblBarChart;
    private TimeFrameControl ctlTimeFrameFieldUT;
    private TimeFrameControl ctlTimeFrameFieldBC;
    private TimeFrameControl ctlTimeFrameFieldLT;
    private HorizontalFieldControl ctlHorizontalFieldBC;
    private VerticleFieldControl ctlVerticleFieldBC;
    private ComboBox cboSubsetTypeTC;
    private Label lblMaxLinesStartTC;
    private TimeFrameControl ctlTimeFrameFieldTC;
    private VerticleFieldControl ctlVerticleFieldTC;
    private Label lblHorizontalFieldTC;
    private GroupByControl ctlGroupByFieldUT;
    private UserFilterControl ctlUserFilterUT;
    private SummaryFieldControl ctlSummaryFieldsUT;
    private GroupByControl ctlGroupByFieldTC;
    private EMHelpLink emHelpLink1;
    private Label lblMaxBarsEndBC;
    private Label lblSubsetTypeStartBC;
    private Label lblSubsetTypeEndBC;
    private Label lblTimeFrameBC;
    private Label lblVerticalFieldBC;
    private Label lblHorizontalFieldBC;
    private Label lblSubsetTypeMiddleBC;
    private Label lblBarCountBC;
    private Label lblGroupByTC;
    private Label lblVerticalFieldTC;
    private Label lblMaxLinesEndTC;
    private Label lblLineCountTC;
    private Label lblSubsetTypeMiddleTC;
    private Label lblSubsetTypeEndTC;
    private Label lblSubsetTypeStartTC;
    private Label lblTimeFrameLT;
    private Label lblMaxRowsEndLT;
    private Label lblGroupByUT;
    private Label lblTimeFrameUT;
    private Label lblColumn2;
    private Label lblColumn1;
    private Label lblColumn3;
    private ComboBox cboMaxLinesTC;
    private TableSummaryControl tableSummaryCtl;
    private Label label1;
    private Label label2;
    private TableSummaryControl tableSummaryCtlUserTable;
    private CheckBox chkDefaultDrill;
    private BorderPanel bpTop;
    private CollapsibleSplitter verSplitter;
    private BorderPanel bpBottom;
    private GroupContainer gcSnapshot;
    private GradientPanel gpSnapshotTop;
    private BorderPanel bpBody;
    private GroupContainer gcBarCharBottom;
    private Label label3;
    private GroupContainer gcBarChartTop;
    private GroupContainer gcUserTableTop;
    private GroupContainer gcUserTableBottom;
    private GroupContainer gcLoanTableBottom;
    private GroupContainer gcTrendTop;
    private GroupContainer gcTrendBottom;
    private Label label4;
    private Label label5;
    private StandardIconButton btnSave;
    private Label label6;
    private ToolTip toolTip1;
    private Panel pnlFolders;
    private Panel pnlRowFilter;
    private Panel pnlRowOrder;

    public DashboardTemplate SelectedTemplate => this.getSelectedTemplate();

    public FileSystemEntry SelectedFileSystemEntry => this.getSelectedFileSystemEntry();

    public DashboardTemplate EditedTemplate => this.dashboardTemplate;

    public Size GetSelectTemplateSize => new Size(this.pnlLeft.Width + 15, this.Height + 35);

    public Size GetManageTemplateSize => new Size(this.Width + 15, this.Height + 45);

    protected override void OnCreateControl()
    {
      base.OnCreateControl();
      if (this.ParentForm == null)
        return;
      this.ParentForm.FormClosing += new FormClosingEventHandler(this.DashboardTemplateForm_FormClosing);
      this.ParentForm.FormClosed += new FormClosedEventHandler(this.DashboardTemplateForm_FormClosed);
    }

    public DashboardTemplateControl(
      SnapshotBaseControl snapshot,
      bool isViewGlobal,
      bool isViewReadOnly)
      : this(snapshot, isViewGlobal, isViewReadOnly, Session.DefaultInstance)
    {
    }

    public DashboardTemplateControl(
      SnapshotBaseControl snapshot,
      bool isViewGlobal,
      bool isViewReadOnly,
      Sessions.Session session)
      : this(DashboardTemplateControl.ProcessingMode.EditTemplate, session)
    {
      if (snapshot == null || this.fieldDefinitions.Count == 0)
        return;
      this.snapshot = snapshot;
      this.isViewGlobal = isViewGlobal;
      this.isViewReadOnly = isViewReadOnly;
      FileSystemEntry fileSystemEntry = FileSystemEntry.Parse(snapshot.DashboardTemplatePath);
      fileSystemEntry.Access = !fileSystemEntry.IsPublic ? (this.hasPrivateRight ? AclResourceAccess.ReadWrite : AclResourceAccess.None) : this.session.AclGroupManager.GetUserFileFolderAccess(AclFileType.DashboardTemplate, fileSystemEntry);
      this.getTemplate(fileSystemEntry);
    }

    public DashboardTemplateControl(
      DashboardTemplateControl.ProcessingMode processingMode,
      Sessions.Session session)
      : this(processingMode, false, session)
    {
    }

    public DashboardTemplateControl(
      DashboardTemplateControl.ProcessingMode processingMode,
      Sessions.Session session,
      bool isMultiSelect,
      bool saveOnly)
      : this(processingMode, false, session)
    {
      switch (processingMode)
      {
        case DashboardTemplateControl.ProcessingMode.EditTemplate:
        case DashboardTemplateControl.ProcessingMode.ManageTemplates:
          this.btnCancel.Visible = !saveOnly;
          this.fsExplorer.SingleSelection = !isMultiSelect;
          break;
      }
    }

    public DashboardTemplateControl(
      DashboardTemplateControl.ProcessingMode processingMode,
      bool showPublicOnly,
      Sessions.Session session)
    {
      this.session = session;
      this.fsExplorer = new FSExplorer(this.session);
      this.ctlSelectFolder = new SelectFolderControl(this.session);
      this.ctlFieldSelectionLT = new LoanFieldSelectionControl(this.session);
      this.ctlUserFilterUT = new UserFilterControl(this.session);
      this.processingMode = processingMode == DashboardTemplateControl.ProcessingMode.Unspecified ? DashboardTemplateControl.ProcessingMode.ManageTemplates : processingMode;
      this.showPublicOnly = showPublicOnly;
      this.InitializeComponent();
      if (this.DesignMode || !this.session.IsConnected)
        return;
      this.emHelpLink1.AssignSession(this.session);
      this.initializeForm();
    }

    private void initializeForm()
    {
      this.getSecuritySettings();
      this.getCompanySettings();
      this.getFieldDefinitions();
      if (DashboardTemplateControl.ProcessingMode.SelectTemplate == this.processingMode)
      {
        this.Text = "Select Snapshot";
        Size size = this.pnlLeft.Size;
        this.btnExplorerOk.Text = "Select";
        this.btnExplorerCancel.Visible = true;
        this.btnExplorerOk.Visible = true;
        this.btnCancel.Visible = false;
        this.pnlRight.Visible = false;
        this.verSplitter.Visible = false;
        this.Size = new Size(size.Width, this.Height);
        this.initializeDashboardExplorer(FSExplorer.DialogMode.SelectFiles);
      }
      else if (DashboardTemplateControl.ProcessingMode.EditTemplate == this.processingMode)
      {
        this.Text = "Edit Snapshot";
        Size size = this.pnlRight.Size;
        this.btnExplorerOk.Visible = false;
        this.btnExplorerCancel.Visible = false;
        this.pnlLeft.Visible = false;
        this.pnlRight.Visible = true;
        this.verSplitter.Visible = false;
        this.Size = new Size(size.Width, this.Height);
        this.initializeDashboardExplorer(FSExplorer.DialogMode.ManageFiles);
      }
      else
      {
        this.Text = "Manage Snapshots";
        this.btnExplorerOk.Visible = false;
        this.btnExplorerCancel.Visible = false;
        this.pnlRight.Visible = true;
        this.tabDefineTemplate.Enabled = false;
        this.verSplitter.Visible = true;
        this.initializeDashboardExplorer(FSExplorer.DialogMode.ManageFiles);
      }
      this.templateValidator = new DashboardTemplateValidator(this.fieldDefinitions);
      this.setupCommonControls();
      this.setupBarChartControls();
      this.setupTrendChartControls();
      this.setupLoanTableControls();
      this.setupUserTableControls();
      this.clearUI();
      this.setActionButton();
    }

    private void getSecuritySettings()
    {
      this.hasPublicRight = UserInfo.IsSuperAdministrator(this.session.UserID, this.session.UserInfo.UserPersonas) || this.session.AclGroupManager.CheckPublicAccessPermission(AclFileType.DashboardTemplate);
      if (this.showPublicOnly)
        this.hasPrivateRight = false;
      else
        this.hasPrivateRight = ((FeaturesAclManager) this.session.ACL.GetAclManager(AclCategory.Features)).GetUserApplicationRight(AclFeature.DashboardTab_ManagePersonalViewTemplate);
    }

    private void getCompanySettings()
    {
      IDictionary serverSettings = this.session.ServerManager.GetServerSettings("Dashboard");
      this.maxBarChartBars = (int) serverSettings[(object) "Dashboard.MaxBarChartBars"];
      this.maxTrendChartLines = (int) serverSettings[(object) "Dashboard.MaxTrendChartLines"];
      this.maxLoanTableRows = (int) serverSettings[(object) "Dashboard.MaxLoanTableRows"];
      this.maxLoanTableColumns = (int) serverSettings[(object) "Dashboard.MaxLoanTableColumns"];
      this.maxUserTableGroups = (int) serverSettings[(object) "Dashboard.MaxUserTableGroups"];
    }

    private void getFieldDefinitions()
    {
      this.fieldDefinitions = DashboardSettings.GetLoanReportFieldDefs(this.session).Clone();
      if (this.fieldDefinitions == null || this.fieldDefinitions.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Reporting database field definitions could not be loaded. See your administrator for assistance.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.fieldDefinitions = new LoanReportFieldDefs(this.session);
        this.pnlExplorer.Enabled = false;
        this.tabDefineTemplate.Enabled = false;
      }
      else
      {
        LoanReportFieldDef fieldDef1 = new LoanReportFieldDef("Dashboard", "LoanCount", "Loan Count", "Number of Loans", FieldFormat.INTEGER, "Dashboard.LoanCount");
        LoanReportFieldDef fieldDef2 = new LoanReportFieldDef("Dashboard", "NoGroupBy", "No Group By", "No Group By", FieldFormat.STRING, "Dashboard.NoGroupBy");
        LoanReportFieldDef fieldDef3 = new LoanReportFieldDef("Dashboard", "NoSummary", "No Summary", "No Summary", FieldFormat.INTEGER, "Dashboard.NoSummary");
        this.fieldDefinitions.Add((ReportFieldDef) fieldDef1);
        this.fieldDefinitions.Add((ReportFieldDef) fieldDef2);
        this.fieldDefinitions.Add((ReportFieldDef) fieldDef3);
      }
    }

    private void initializeDashboardExplorer(FSExplorer.DialogMode fsExplorerMode)
    {
      this.fsExplorer.SetDashboardTemplateProperties(fsExplorerMode);
      this.ifsExplorer = new DashboardIFSExplorer(DashboardTemplateControl.ProcessingMode.SelectTemplate == this.processingMode, this.session);
      FileSystemEntry defaultFolder = FileSystemEntry.PublicRoot;
      if (this.hasPrivateRight)
        defaultFolder = FileSystemEntry.PrivateRoot(this.session.UserID);
      FileSystemEntry lastFolderViewed = this.getLastFolderViewed();
      if (lastFolderViewed != null)
        defaultFolder = lastFolderViewed;
      this.fsExplorer.InitDashboardTemplate((IFSExplorerBase) this.ifsExplorer, defaultFolder, this.showPublicOnly);
      this.fsExplorer.SelectedEntryChanged += new EventHandler(this.fsExplorer_SelectedEntryChanged);
      this.fsExplorer.FolderChanged += new EventHandler(this.fsExplorer_FolderChanged);
      this.fsExplorer.BeforeFolderRenamed += new EventHandler(this.fsExplorer_BeforeFolderRenamed);
      this.fsExplorer.Leave += new EventHandler(this.fsExplorer_Leave);
      this.ifsExplorer.DeleteFileEvent += new DashboardIFSExplorer.DeleteFileEventHandler(this.ifsExplorer_DeleteFileEvent);
      this.ifsExplorer.MoveEntryEvent += new DashboardIFSExplorer.MoveEntryEventHandler(this.ifsExplorer_MoveEntryEvent);
      if (DashboardTemplateControl.ProcessingMode.SelectTemplate != this.processingMode)
        return;
      this.ifsExplorer.OpenFileEvent += new DashboardIFSExplorer.OpenFileEventHandler(this.ifsExplorer_OpenFileEvent);
    }

    private FileSystemEntry getLastFolderViewed()
    {
      string privateProfileString = this.session.GetPrivateProfileString("DashboardTemplate", "LastFolderViewed");
      if (privateProfileString != null)
      {
        if (string.Empty != privateProfileString)
        {
          try
          {
            FileSystemEntry entry = FileSystemEntry.Parse(privateProfileString);
            if (this.ifsExplorer.EntryExists(entry))
            {
              if (!entry.IsPublic || !this.hasPublicRight)
              {
                if (!entry.IsPublic)
                {
                  if (!this.hasPrivateRight)
                    goto label_8;
                }
                else
                  goto label_8;
              }
              return entry;
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
label_8:
      return (FileSystemEntry) null;
    }

    private void setupCommonControls()
    {
      this.cboChartType.DataSource = SelectionOptions.ChartTypeOptions.Clone();
      this.cboChartType.DisplayMember = "Name";
      this.cboChartType.ValueMember = "Id";
      this.ctlSelectFolder.DataChangedEvent += new SelectFolderControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlAdvancedSearch.DataChange += new EventHandler(this.userControl_DataChangedEvent);
    }

    private void setupBarChartControls()
    {
      this.cboSubsetTypeBC.DataSource = SelectionOptions.SubsetTypeOptions.Clone();
      this.cboSubsetTypeBC.DisplayMember = "Name";
      this.cboSubsetTypeBC.ValueMember = "Id";
      this.ctlTimeFrameFieldBC.DataChangedEvent += new TimeFrameControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlVerticleFieldBC.DataChangedEvent += new VerticleFieldControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlHorizontalFieldBC.DataChangedEvent += new HorizontalFieldControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
    }

    private void setupTrendChartControls()
    {
      this.cboSubsetTypeTC.DataSource = SelectionOptions.SubsetTypeOptions.Clone();
      this.cboSubsetTypeTC.DisplayMember = "Name";
      this.cboSubsetTypeTC.ValueMember = "Id";
      for (int index = 1; index <= this.maxTrendChartLines; ++index)
        this.cboMaxLinesTC.Items.Add((object) index);
      this.ctlVerticleFieldTC.DataChangedEvent += new VerticleFieldControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlTimeFrameFieldTC.DataChangedEvent += new TimeFrameControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlGroupByFieldTC.DataChangedEvent += new GroupByControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
    }

    private void setupLoanTableControls()
    {
      this.ctlFieldSelectionLT.MaxSelectionCount = this.maxLoanTableColumns;
      this.ctlTimeFrameFieldLT.DataChangedEvent += new TimeFrameControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlFieldSelectionLT.DataChangedEvent += new LoanFieldSelectionControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.tableSummaryCtl.DataChangedEvent += new TableSummaryControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
    }

    private void setupUserTableControls()
    {
      this.ctlTimeFrameFieldUT.DataChangedEvent += new TimeFrameControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlUserFilterUT.DataChangedEvent += new UserFilterControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlGroupByFieldUT.DataChangedEvent += new GroupByControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.ctlSummaryFieldsUT.DataChangedEvent += new SummaryFieldControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
      this.tableSummaryCtlUserTable.DataChangedEvent += new TableSummaryControl.DataChangedEventHandler(this.userControl_DataChangedEvent);
    }

    private void clearUI()
    {
      this.gcSnapshot.Text = "Snapshot Details";
      this.currentChartType = DashboardChartType.None;
      this.cboChartType.SelectedIndex = -1;
      this.cboChartType.SelectedIndex = -1;
      this.cboChartType.Enabled = false;
      this.pnlBarChart.Visible = false;
      this.pnlTrendChart.Visible = false;
      this.pnlLoanTable.Visible = false;
      this.pnlUserTable.Visible = false;
      this.chkDefaultDrill.Visible = false;
      this.tabDefineTemplate.Enabled = false;
      this.clearDataChanged();
    }

    private void getTemplate(FileSystemEntry fileSystemEntry)
    {
      this.isPublicTemplate = fileSystemEntry.IsPublic;
      this.fileSystemEntry = fileSystemEntry;
      this.dashboardTemplate = this.ifsExplorer.LoadDashboardTemplate(fileSystemEntry);
      if (this.dashboardTemplate == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The '" + this.fileSystemEntry.Name + "' snapshot was not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.fileSystemEntry = (FileSystemEntry) null;
        this.clearUI();
      }
      else
      {
        this.isTemplateGlobal = this.session.ReportManager.IsTemplateReferencedByGlobalView(fileSystemEntry.ToString());
        this.isTemplateReadOnly = false;
        if (fileSystemEntry.IsPublic && AclResourceAccess.ReadWrite != fileSystemEntry.Access)
          this.isTemplateReadOnly = true;
        this.loadTemplate();
      }
    }

    private void loadTemplate()
    {
      this.setReadWriteMode();
      this.gcSnapshot.Text = this.dashboardTemplate.TemplateName;
      switch (this.dashboardTemplate.ChartType)
      {
        case DashboardChartType.None:
          this.loadNewTemplate();
          break;
        case DashboardChartType.BarChart:
          this.loadBarChartTemplate();
          break;
        case DashboardChartType.TrendChart:
          this.loadTrendChartTemplate();
          break;
        case DashboardChartType.LoanTable:
          this.loadLoanTableTemplate();
          break;
        case DashboardChartType.UserTable:
          this.loadUserTableTemplate();
          break;
      }
      this.ctlSelectFolder.SetFolderNameList(this.dashboardTemplate.Folders, this.dashboardTemplate.SelectAllFolders, this.dashboardTemplate.SelectAllWOArchiveFolders);
      this.ctlAdvancedSearch.SetCurrentFilter(this.dashboardTemplate.Filters);
      this.ctlAdvancedSearch.FieldDefs = (ReportFieldDefs) this.fieldDefinitions;
      this.clearDataChanged();
      this.fsExplorer.SetFocusToFileListView();
    }

    private void setReadWriteMode()
    {
      this.cboChartType.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlBarChart.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlTrendChart.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlLoanTable.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlUserTable.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlFolders.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
      this.pnlFilters.Enabled = !this.isViewReadOnly && !this.isTemplateReadOnly;
    }

    private void loadNewTemplate()
    {
      this.clearUI();
      this.gcSnapshot.Text = this.dashboardTemplate.TemplateName;
      this.cboChartType.Enabled = true;
      this.tabDefineTemplate.Enabled = true;
      this.pnlFolders.Enabled = false;
      this.pnlFilters.Enabled = false;
    }

    private void loadBarChartTemplate()
    {
      this.setUI(DashboardChartType.BarChart);
      LoanReportFieldDef fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.YAxisField);
      if (fieldByCriterionName1 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Verticle Axis Field' has been reset to 'Number of Loans'. Field '" + this.dashboardTemplate.YAxisField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.YAxisField = "Dashboard.LoanCount";
        fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.YAxisField);
        this.setDataChanged();
      }
      this.ctlVerticleFieldBC.VerticleFieldDefinition = fieldByCriterionName1;
      this.ctlVerticleFieldBC.SummaryType = this.dashboardTemplate.YAxisSummaryType;
      if (string.Empty == this.dashboardTemplate.XAxisField)
        this.dashboardTemplate.XAxisField = "Loan.CurrentMilestoneName";
      LoanReportFieldDef fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.XAxisField);
      if (fieldByCriterionName2 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Horizontal Axis Field' has been reset to 'Current Milestone'. Field '" + this.dashboardTemplate.XAxisField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.XAxisField = "Loan.CurrentMilestoneName";
        fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.XAxisField);
        this.setDataChanged();
      }
      this.ctlHorizontalFieldBC.HorizontalFieldDefinition = fieldByCriterionName2;
      LoanReportFieldDef fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
      if (fieldByCriterionName3 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Display Loans By Field' has been reset to 'File Started Date'. Field '" + this.dashboardTemplate.TimeFrameField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.TimeFrameField = "Loan.DateFileOpened";
        fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
        this.setDataChanged();
      }
      this.ctlTimeFrameFieldBC.TimeFrameFieldDefinition = fieldByCriterionName3;
      if (this.minBarChartBars > this.dashboardTemplate.MaxBars || this.maxBarChartBars < this.dashboardTemplate.MaxBars)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Maximum Number of Bars Field' has been reset to 50.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.MaxBars = this.maxBarChartBars;
        this.setDataChanged();
      }
      this.nudMaxBarsBC.Value = (Decimal) this.dashboardTemplate.MaxBars;
      this.cboSubsetTypeBC.SelectedValue = (object) (int) this.dashboardTemplate.SubsetType;
      this.emHelpLink1.HelpTag = "Dashboard Bar Chart";
    }

    private void loadTrendChartTemplate()
    {
      this.setUI(DashboardChartType.TrendChart);
      LoanReportFieldDef fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.YAxisField);
      if (fieldByCriterionName1 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Verticle Axis Field' has been reset to 'Number of Loans'. Field '" + this.dashboardTemplate.YAxisField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.YAxisField = "Dashboard.LoanCount";
        fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.YAxisField);
        this.setDataChanged();
      }
      this.ctlVerticleFieldTC.VerticleFieldDefinition = fieldByCriterionName1;
      this.ctlVerticleFieldTC.SummaryType = this.dashboardTemplate.YAxisSummaryType;
      if (string.Empty == this.dashboardTemplate.XAxisField)
        this.dashboardTemplate.XAxisField = "Loan.DateFileOpened";
      LoanReportFieldDef fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.XAxisField);
      if (fieldByCriterionName2 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Horizontal Axis Field' has been reset to 'File Started Date'. Field '" + this.dashboardTemplate.XAxisField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.XAxisField = "Loan.DateFileOpened";
        fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.XAxisField);
        this.setDataChanged();
      }
      this.ctlTimeFrameFieldTC.TimeFrameFieldDefinition = fieldByCriterionName2;
      LoanReportFieldDef fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.GroupByField);
      if (fieldByCriterionName3 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Data Source Field' has been reset to 'No Group By Field'. Field '" + this.dashboardTemplate.GroupByField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.GroupByField = "Dashboard.NoGroupBy";
        fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.GroupByField);
        this.setDataChanged();
      }
      this.ctlGroupByFieldTC.GroupByFieldDefinition = fieldByCriterionName3;
      if (this.minTrendChartLines > this.dashboardTemplate.MaxLines || this.maxTrendChartLines < this.dashboardTemplate.MaxLines)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Maximum Number of Lines Field' has been reset to 10.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.MaxLines = this.maxTrendChartLines;
        this.setDataChanged();
      }
      this.cboMaxLinesTC.SelectedItem = (object) this.dashboardTemplate.MaxLines;
      this.lblLineCountTC.Text = this.dashboardTemplate.MaxLines.ToString();
      this.cboSubsetTypeTC.SelectedValue = (object) (int) this.dashboardTemplate.SubsetType;
      this.emHelpLink1.HelpTag = "Dashboard Trend Chart";
    }

    private void loadLoanTableTemplate()
    {
      this.setUI(DashboardChartType.LoanTable);
      this.ctlFieldSelectionLT.SetColumnInfoList(this.dashboardTemplate.Fields);
      LoanReportFieldDef fieldByCriterionName = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
      if (fieldByCriterionName == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Display Loans By Field' has been reset to 'File Started Date'. Field '" + this.dashboardTemplate.TimeFrameField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.TimeFrameField = "Loan.DateFileOpened";
        fieldByCriterionName = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
        this.setDataChanged();
      }
      this.ctlTimeFrameFieldLT.TimeFrameFieldDefinition = fieldByCriterionName;
      if (this.minLoanTableRows > this.dashboardTemplate.MaxRows || this.maxLoanTableRows < this.dashboardTemplate.MaxRows)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Maximum Number of Loans Field' has been reset to 1000.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.MaxRows = this.maxLoanTableRows;
        this.setDataChanged();
      }
      this.nudMaxRowsLT.Value = (Decimal) this.dashboardTemplate.MaxRows;
      this.tableSummaryCtl.IncludeMin = this.dashboardTemplate.IncludeMin;
      this.tableSummaryCtl.IncludeMax = this.dashboardTemplate.IncludeMax;
      this.tableSummaryCtl.IncludeAverage = this.dashboardTemplate.IncludeAverage;
      this.tableSummaryCtl.IncludeTotal = this.dashboardTemplate.IncludeTotal;
      this.emHelpLink1.HelpTag = "Dashboard Loan Table";
      this.chkDefaultDrill.Visible = false;
      if (this.session.UserInfo.IsSuperAdministrator())
      {
        if (!this.isPublicTemplate)
          return;
        this.chkDefaultDrill.Visible = true;
        if (this.fileSystemEntry.Path == this.session.SessionObjects.GetCompanySettingFromCache("Dashboard", "DefaultDrilldownView"))
          this.setDrilldownView(true);
        else
          this.setDrilldownView(false);
      }
      else
        this.chkDefaultDrill.Visible = false;
    }

    private void loadUserTableTemplate()
    {
      this.setUI(DashboardChartType.UserTable);
      LoanReportFieldDef fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField1);
      if (fieldByCriterionName1 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Column 1 Field' has been reset to 'Number of Loans'. Field '" + this.dashboardTemplate.SummaryField1 + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.SummaryField1 = "Dashboard.LoanCount";
        this.dashboardTemplate.SummaryType1 = ColumnSummaryType.None;
        fieldByCriterionName1 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField1);
        this.setDataChanged();
      }
      this.ctlSummaryFieldsUT.SummaryFieldDefinition1 = fieldByCriterionName1;
      this.ctlSummaryFieldsUT.SummaryType1 = this.dashboardTemplate.SummaryType1;
      LoanReportFieldDef fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField2);
      if (fieldByCriterionName2 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Column 2 Field' has been cleared. Field '" + this.dashboardTemplate.SummaryField2 + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.SummaryField2 = "Dashboard.NoSummary";
        this.dashboardTemplate.SummaryType2 = ColumnSummaryType.None;
        fieldByCriterionName2 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField2);
        this.setDataChanged();
      }
      this.ctlSummaryFieldsUT.SummaryFieldDefinition2 = fieldByCriterionName2;
      this.ctlSummaryFieldsUT.SummaryType2 = this.dashboardTemplate.SummaryType2;
      LoanReportFieldDef fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField3);
      if (fieldByCriterionName3 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Column 3 Field' has been cleared. Field '" + this.dashboardTemplate.SummaryField3 + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.SummaryField3 = "Dashboard.NoSummary";
        this.dashboardTemplate.SummaryType3 = ColumnSummaryType.None;
        fieldByCriterionName3 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.SummaryField3);
        this.setDataChanged();
      }
      this.ctlSummaryFieldsUT.SummaryFieldDefinition3 = fieldByCriterionName3;
      this.ctlSummaryFieldsUT.SummaryType3 = this.dashboardTemplate.SummaryType3;
      LoanReportFieldDef fieldByCriterionName4 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.GroupByField);
      if (fieldByCriterionName4 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Group Columns By Field' has been reset to 'No Group By Field'. Field '" + this.dashboardTemplate.GroupByField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.GroupByField = "Dashboard.NoGroupBy";
        fieldByCriterionName4 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.GroupByField);
        this.setDataChanged();
      }
      this.ctlGroupByFieldUT.GroupByFieldDefinition = fieldByCriterionName4;
      this.ctlUserFilterUT.RoleId = this.dashboardTemplate.RoleId;
      this.ctlUserFilterUT.OrganizationId = this.dashboardTemplate.OrganizationId;
      this.ctlUserFilterUT.IncludeChildren = this.dashboardTemplate.IncludeChildren;
      this.ctlUserFilterUT.UserGroupId = this.dashboardTemplate.UserGroupId;
      LoanReportFieldDef fieldByCriterionName5 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
      if (fieldByCriterionName5 == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The 'Display Loans By Field' has been reset to 'File Started Date'. Field '" + this.dashboardTemplate.TimeFrameField + "' is not available in the Reporting Database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.dashboardTemplate.TimeFrameField = "Loan.DateFileOpened";
        fieldByCriterionName5 = this.fieldDefinitions.GetFieldByCriterionName(this.dashboardTemplate.TimeFrameField);
        this.setDataChanged();
      }
      this.ctlTimeFrameFieldUT.TimeFrameFieldDefinition = fieldByCriterionName5;
      this.tableSummaryCtlUserTable.IncludeMin = this.dashboardTemplate.IncludeMin;
      this.tableSummaryCtlUserTable.IncludeMax = this.dashboardTemplate.IncludeMax;
      this.tableSummaryCtlUserTable.IncludeAverage = this.dashboardTemplate.IncludeAverage;
      this.tableSummaryCtlUserTable.HideTotalOption = true;
      this.emHelpLink1.HelpTag = "Dashboard User Table";
    }

    private ColumnInfo createColumnInfo(string fieldId)
    {
      LoanReportFieldDef fieldById = this.fieldDefinitions.GetFieldByID(fieldId);
      if (fieldById == null)
        return (ColumnInfo) null;
      return new ColumnInfo(fieldById.FieldID, fieldById.Description, ColumnSortOrder.Ascending, ColumnSummaryType.Count, 0)
      {
        CriterionName = fieldById.CriterionFieldName
      };
    }

    private void refreshTemplate()
    {
      if (this.fileSystemEntry == null)
        return;
      this.getTemplate(this.fileSystemEntry);
    }

    private bool saveTemplate(bool forceValidation)
    {
      if (this.fileSystemEntry == null || !this.dataChanged || -1 == this.cboChartType.SelectedIndex)
      {
        this.clearDataChanged();
        return true;
      }
      if (!this.verifyTemplate(true) & forceValidation)
        return false;
      this.dashboardTemplate.TemplateName = this.gcSnapshot.Text.Trim();
      switch ((DashboardChartType) this.cboChartType.SelectedValue)
      {
        case DashboardChartType.BarChart:
          this.saveBarChartTemplate();
          break;
        case DashboardChartType.TrendChart:
          this.saveTrendChartTemplate();
          break;
        case DashboardChartType.LoanTable:
          this.saveLoanTableTemplate();
          break;
        case DashboardChartType.UserTable:
          this.saveUserTableTemplate();
          break;
      }
      this.dashboardTemplate.Folders = this.ctlSelectFolder.GetFolderNameList();
      this.dashboardTemplate.Filters = this.ctlAdvancedSearch.GetCurrentFilter();
      this.dashboardTemplate.IsNewTemplate = false;
      this.ifsExplorer.SaveDashboardTemplate(this.fileSystemEntry, this.dashboardTemplate);
      this.clearDataChanged();
      return true;
    }

    private void saveBarChartTemplate()
    {
      this.dashboardTemplate.ChartType = DashboardChartType.BarChart;
      this.dashboardTemplate.YAxisField = this.ctlVerticleFieldBC.VerticleFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.YAxisSummaryType = "Dashboard.LoanCount" == this.dashboardTemplate.YAxisField ? ColumnSummaryType.Count : this.ctlVerticleFieldBC.SummaryType;
      this.dashboardTemplate.XAxisField = this.ctlHorizontalFieldBC.HorizontalFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.TimeFrameField = this.ctlTimeFrameFieldBC.TimeFrameFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.MaxBars = Convert.ToInt32(this.nudMaxBarsBC.Value);
      this.dashboardTemplate.SubsetType = (DashboardSubsetType) this.cboSubsetTypeBC.SelectedValue;
    }

    private void saveTrendChartTemplate()
    {
      this.dashboardTemplate.ChartType = DashboardChartType.TrendChart;
      this.dashboardTemplate.YAxisField = this.ctlVerticleFieldTC.VerticleFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.YAxisSummaryType = "Dashboard.LoanCount" == this.dashboardTemplate.YAxisField ? ColumnSummaryType.Count : this.ctlVerticleFieldTC.SummaryType;
      this.dashboardTemplate.XAxisField = this.ctlTimeFrameFieldTC.TimeFrameFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.GroupByField = this.ctlGroupByFieldTC.GroupByFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.MaxLines = (int) this.cboMaxLinesTC.SelectedItem;
      this.dashboardTemplate.SubsetType = (DashboardSubsetType) this.cboSubsetTypeTC.SelectedValue;
    }

    private void saveLoanTableTemplate()
    {
      this.dashboardTemplate.ChartType = DashboardChartType.LoanTable;
      this.dashboardTemplate.Fields = this.ctlFieldSelectionLT.GetColumnInfoList();
      this.dashboardTemplate.TimeFrameField = this.ctlTimeFrameFieldLT.TimeFrameFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.MaxRows = Convert.ToInt32(this.nudMaxRowsLT.Value);
      this.dashboardTemplate.IncludeMin = this.tableSummaryCtl.IncludeMin;
      this.dashboardTemplate.IncludeMax = this.tableSummaryCtl.IncludeMax;
      this.dashboardTemplate.IncludeAverage = this.tableSummaryCtl.IncludeAverage;
      this.dashboardTemplate.IncludeTotal = this.tableSummaryCtl.IncludeTotal;
    }

    private void saveUserTableTemplate()
    {
      this.dashboardTemplate.ChartType = DashboardChartType.UserTable;
      this.dashboardTemplate.SummaryField1 = this.ctlSummaryFieldsUT.SummaryFieldDefinition1.CriterionFieldName;
      this.dashboardTemplate.SummaryType1 = this.ctlSummaryFieldsUT.SummaryType1;
      this.dashboardTemplate.SummaryField2 = this.ctlSummaryFieldsUT.SummaryFieldDefinition2.CriterionFieldName;
      this.dashboardTemplate.SummaryType2 = this.ctlSummaryFieldsUT.SummaryType2;
      this.dashboardTemplate.SummaryField3 = this.ctlSummaryFieldsUT.SummaryFieldDefinition3.CriterionFieldName;
      this.dashboardTemplate.SummaryType3 = this.ctlSummaryFieldsUT.SummaryType3;
      this.dashboardTemplate.GroupByField = this.ctlGroupByFieldUT.GroupByFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.RoleId = this.ctlUserFilterUT.RoleId;
      this.dashboardTemplate.OrganizationId = this.ctlUserFilterUT.OrganizationId;
      this.dashboardTemplate.IncludeChildren = this.ctlUserFilterUT.IncludeChildren;
      this.dashboardTemplate.UserGroupId = this.ctlUserFilterUT.UserGroupId;
      this.dashboardTemplate.TimeFrameField = this.ctlTimeFrameFieldUT.TimeFrameFieldDefinition.CriterionFieldName;
      this.dashboardTemplate.IncludeMin = this.tableSummaryCtlUserTable.IncludeMin;
      this.dashboardTemplate.IncludeMax = this.tableSummaryCtlUserTable.IncludeMax;
      this.dashboardTemplate.IncludeAverage = this.tableSummaryCtlUserTable.IncludeAverage;
      this.dashboardTemplate.IncludeTotal = false;
    }

    private void setUI(DashboardChartType chartType)
    {
      this.currentChartType = chartType;
      this.cboChartType.SelectedValue = (object) (int) this.currentChartType;
      this.cboChartType.Enabled = this.dashboardTemplate.IsNewTemplate;
      switch (chartType)
      {
        case DashboardChartType.BarChart:
          this.setUIForBarChart();
          break;
        case DashboardChartType.TrendChart:
          this.setUIForTrendChart();
          break;
        case DashboardChartType.LoanTable:
          this.setUIForLoanTable();
          break;
        case DashboardChartType.UserTable:
          this.setUIForUserTable();
          break;
        default:
          this.clearUI();
          return;
      }
      this.ctlSelectFolder.SetFolderNameList(new List<string>(), false, false);
      this.ctlSelectFolder.Visible = true;
      this.ctlAdvancedSearch.ClearFilters();
      this.ctlAdvancedSearch.Visible = true;
      this.tabDefineTemplate.Enabled = true;
    }

    private void setUIForBarChart()
    {
      this.resetUIType();
      this.pnlBarChart.Visible = true;
      this.pnlBarChart.Dock = DockStyle.Fill;
      this.pnlBarChart.BringToFront();
      this.ctlTimeFrameFieldBC.FieldDefinitions = this.fieldDefinitions;
      this.ctlVerticleFieldBC.FieldDefinitions = this.fieldDefinitions;
      this.ctlHorizontalFieldBC.FieldDefinitions = this.fieldDefinitions;
    }

    private void resetUIType()
    {
      this.pnlBarChart.Visible = false;
      this.pnlBarChart.Dock = DockStyle.None;
      this.pnlTrendChart.Visible = false;
      this.pnlTrendChart.Dock = DockStyle.None;
      this.pnlLoanTable.Visible = false;
      this.pnlLoanTable.Dock = DockStyle.None;
      this.pnlUserTable.Visible = false;
      this.pnlUserTable.Dock = DockStyle.None;
      this.chkDefaultDrill.Visible = false;
    }

    private void setUIForTrendChart()
    {
      this.resetUIType();
      this.pnlTrendChart.Visible = true;
      this.pnlTrendChart.Dock = DockStyle.Fill;
      this.pnlTrendChart.BringToFront();
      this.ctlVerticleFieldTC.FieldDefinitions = this.fieldDefinitions;
      this.ctlTimeFrameFieldTC.FieldDefinitions = this.fieldDefinitions;
      this.ctlGroupByFieldTC.FieldDefinitions = this.fieldDefinitions;
    }

    private void setUIForLoanTable()
    {
      this.resetUIType();
      this.pnlLoanTable.Visible = true;
      this.pnlLoanTable.Dock = DockStyle.Fill;
      this.chkDefaultDrill.Visible = true;
      this.pnlLoanTable.BringToFront();
      this.ctlTimeFrameFieldLT.FieldDefinitions = this.fieldDefinitions;
      this.ctlFieldSelectionLT.SetColumnInfoList(new List<ColumnInfo>());
      this.ctlFieldSelectionLT.Visible = true;
    }

    private void setUIForUserTable()
    {
      this.resetUIType();
      this.pnlUserTable.Visible = true;
      this.pnlUserTable.Dock = DockStyle.Fill;
      this.pnlUserTable.BringToFront();
      this.ctlTimeFrameFieldUT.FieldDefinitions = this.fieldDefinitions;
      this.ctlGroupByFieldUT.FieldDefinitions = this.fieldDefinitions;
      this.ctlSummaryFieldsUT.FieldDefinitions = this.fieldDefinitions;
      if (this.session.EncompassEdition != EncompassEdition.Broker)
        return;
      this.pnlRowFilter.Height = 29;
    }

    private void setDataChanged()
    {
      if (this.isViewReadOnly || this.isTemplateReadOnly)
        return;
      this.dataChanged = true;
      this.btnSave.Enabled = true;
      this.btnCancel.Text = "&Cancel";
    }

    private void clearDataChanged()
    {
      this.dataChanged = false;
      this.btnSave.Enabled = false;
      if (DashboardTemplateControl.ProcessingMode.EditTemplate == this.processingMode)
        return;
      this.btnCancel.Text = "&Close";
    }

    private void checkForDataChanged()
    {
      if (!this.dataChanged || DialogResult.Yes != Utils.Dialog((IWin32Window) this, "Do you want to save changes made to the current snapshot?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        return;
      this.saveTemplate(false);
    }

    private void setActionButton()
    {
      if (this.fsExplorer == null || DashboardTemplateControl.ProcessingMode.SelectTemplate != this.processingMode)
        return;
      bool flag = false;
      GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
      if (selectedItems.Count == 1 && FileSystemEntry.Types.File == ((FileSystemEntry) selectedItems[0].Tag).Type)
        flag = true;
      this.btnExplorerOk.Enabled = flag;
    }

    private DashboardTemplate getSelectedTemplate()
    {
      DashboardTemplate dashboardTemplate = (DashboardTemplate) null;
      FileSystemEntry selectedFileSystemEntry = this.getSelectedFileSystemEntry();
      if (selectedFileSystemEntry != null)
      {
        dashboardTemplate = this.ifsExplorer.LoadDashboardTemplate(selectedFileSystemEntry);
        if (dashboardTemplate == null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The '" + this.fileSystemEntry.Name + "' template was not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
      string errorMessage;
      if (!this.templateValidator.Validate(dashboardTemplate, out errorMessage))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, errorMessage, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        dashboardTemplate = (DashboardTemplate) null;
      }
      return dashboardTemplate;
    }

    private void setSelectedTemplate(FileSystemEntry fileSystemEntry)
    {
      if (fileSystemEntry == null)
        return;
      this.getTemplate(fileSystemEntry);
    }

    private FileSystemEntry getSelectedFileSystemEntry()
    {
      if (this.fileSystemEntry == null)
      {
        GVSelectedItemCollection selectedItems = this.fsExplorer.SelectedItems;
        if (selectedItems.Count != 1 || FileSystemEntry.Types.File != ((FileSystemEntry) selectedItems[0].Tag).Type)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please select a Dashboard Template.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
          this.fileSystemEntry = (FileSystemEntry) selectedItems[0].Tag;
      }
      return this.fileSystemEntry;
    }

    private void setLastFolderViewed()
    {
      try
      {
        if (this.fsExplorer.IsTopFolder)
          return;
        this.session.WritePrivateProfileString("DashboardTemplate", "LastFolderViewed", this.fsExplorer.CurrentFolder.ToString());
      }
      catch (Exception ex)
      {
      }
    }

    private bool verifyTemplate(bool promptUser)
    {
      List<string> todoList = new List<string>();
      switch (this.dashboardTemplate.ChartType)
      {
        case DashboardChartType.LoanTable:
          this.verifyLoanTableTemplate(todoList);
          break;
      }
      if (this.ctlSelectFolder.GetFolderNameList().Count == 0)
        todoList.Add("Select one or more Folders on the Folders tab");
      if (0 >= todoList.Count)
        return true;
      if (promptUser)
      {
        Encoding encoding = Encoding.Default;
        byte[] bytes = new byte[1]{ (byte) 149 };
        string str1 = Encoding.GetEncoding(1252).GetString(bytes);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("To complete the definition of this snapshot, the following adjustments are needed:\n");
        foreach (string str2 in todoList)
          stringBuilder.AppendLine(str1 + " " + str2 + ".");
        Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      return false;
    }

    private void verifyLoanTableTemplate(List<string> todoList)
    {
      if (this.ctlFieldSelectionLT.GetColumnInfoList().Count != 0)
        return;
      todoList.Add("Select one or more Columns on the Snapshot tab");
    }

    private void fsExplorer_SelectedEntryChanged(object sender, EventArgs e)
    {
      if (DashboardTemplateControl.ProcessingMode.SelectTemplate == this.processingMode)
      {
        this.setActionButton();
      }
      else
      {
        if (DashboardTemplateControl.ProcessingMode.ManageTemplates == this.processingMode)
          this.checkForDataChanged();
        if (this.fsExplorer.SelectedItems.Count == 0 || string.Empty == this.fsExplorer.SelectedItems[0].Tag.ToString() || FileSystemEntry.Types.Folder == ((FileSystemEntry) this.fsExplorer.SelectedItems[0].Tag).Type)
        {
          this.fileSystemEntry = (FileSystemEntry) null;
          this.clearUI();
        }
        else
          this.getTemplate((FileSystemEntry) this.fsExplorer.SelectedItems[0].Tag);
      }
    }

    private void fsExplorer_FolderChanged(object sender, EventArgs e)
    {
      if (DashboardTemplateControl.ProcessingMode.SelectTemplate == this.processingMode)
      {
        this.setActionButton();
      }
      else
      {
        if (DashboardTemplateControl.ProcessingMode.ManageTemplates != this.processingMode)
          return;
        this.checkForDataChanged();
        this.fileSystemEntry = (FileSystemEntry) null;
        this.clearUI();
      }
    }

    private void fsExplorer_BeforeFolderRenamed(object sender, EventArgs e)
    {
      if (DashboardTemplateControl.ProcessingMode.ManageTemplates != this.processingMode || this.fileSystemEntry == null)
        return;
      this.checkForDataChanged();
    }

    private void ifsExplorer_OpenFileEvent(object sender, SelectedFileEventArgs e)
    {
      if (DashboardTemplateControl.ProcessingMode.SelectTemplate != this.processingMode)
        return;
      this.fileSystemEntry = e.FSEntry;
      this.btnExplorerOK_Click((object) null, (EventArgs) null);
    }

    private void ifsExplorer_DeleteFileEvent(object sender, SelectedFileEventArgs e)
    {
      if (DashboardTemplateControl.ProcessingMode.ManageTemplates != this.processingMode)
        return;
      this.clearDataChanged();
    }

    private void ifsExplorer_MoveEntryEvent(object sender, SelectedFileEventArgs e)
    {
      if (this.fileSystemEntry == null)
        return;
      this.getTemplate(e.FSEntry);
    }

    private void userControl_DataChangedEvent(object sender, EventArgs e) => this.setDataChanged();

    private void btnExplorerOK_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.OK;
      this.ParentForm.Close();
    }

    private void btnExplorerCancel_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.Cancel;
      this.ParentForm.Close();
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.fileSystemEntry == null)
      {
        this.clearDataChanged();
      }
      else
      {
        if (!this.saveTemplate(true))
          return;
        this.refreshTemplate();
        if (DashboardTemplateControl.ProcessingMode.EditTemplate != this.processingMode || this.ParentForm == null)
          return;
        this.ParentForm.DialogResult = DialogResult.OK;
        this.ParentForm.Close();
      }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.ParentForm == null)
        return;
      this.ParentForm.DialogResult = DialogResult.Cancel;
      this.ParentForm.Close();
    }

    private void cboChartType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      if ((DashboardChartType) this.cboChartType.SelectedValue == this.currentChartType)
        return;
      if ((DashboardChartType) this.cboChartType.SelectedValue != this.dashboardTemplate.ChartType && !this.dashboardTemplate.IsNewTemplate && DialogResult.Cancel == Utils.Dialog((IWin32Window) this, "Changing the Chart Type will clear all currently selected fields and filters.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2))
      {
        this.cboChartType.SelectedValue = (object) (int) this.currentChartType;
      }
      else
      {
        this.dashboardTemplate.ChartType = (DashboardChartType) this.cboChartType.SelectedValue;
        this.dashboardTemplate.ResetDefaults();
        this.loadTemplate();
        this.setDataChanged();
      }
    }

    private void nudMaxBarsBC_KeyPress(object sender, KeyPressEventArgs e) => this.setDataChanged();

    private void nudMaxBarsBC_ValueChanged(object sender, EventArgs e)
    {
      this.nudMaxBarsBC.Value = Decimal.Round(this.nudMaxBarsBC.Value, 0, MidpointRounding.AwayFromZero);
      this.lblBarCountBC.Text = this.nudMaxBarsBC.Value.ToString();
      this.setDataChanged();
    }

    private void cboSubsetTypeBC_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.setDataChanged();
    }

    private void cboMaxLinesTC_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.lblLineCountTC.Text = ((int) this.cboMaxLinesTC.SelectedItem).ToString();
      this.setDataChanged();
    }

    private void cboSubsetTypeTC_SelectionChangeCommitted(object sender, EventArgs e)
    {
      this.setDataChanged();
    }

    private void nudMaxRowsLT_KeyPress(object sender, KeyPressEventArgs e) => this.setDataChanged();

    private void nudMaxRowsLT_ValueChanged(object sender, EventArgs e) => this.setDataChanged();

    private void DashboardTemplateForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.ParentForm == null || !this.dataChanged || DialogResult.Cancel == this.ParentForm.DialogResult)
        return;
      DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "Do you want to save changes made to the current snapshot?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
      if (DialogResult.Cancel == dialogResult)
      {
        e.Cancel = true;
      }
      else
      {
        if (DialogResult.Yes != dialogResult)
          return;
        if (!this.saveTemplate(true))
          e.Cancel = true;
        else
          this.ParentForm.DialogResult = DialogResult.OK;
      }
    }

    private void DashboardTemplateForm_FormClosed(object sender, FormClosedEventArgs e)
    {
      this.setLastFolderViewed();
    }

    private void setDrilldownView(bool isDefault)
    {
      this.chkDefaultDrill.CheckedChanged -= new EventHandler(this.chkDefaultDrill_CheckedChanged);
      if (isDefault)
      {
        this.chkDefaultDrill.Checked = true;
        this.chkDefaultDrill.Enabled = false;
      }
      else
      {
        this.chkDefaultDrill.Checked = false;
        this.chkDefaultDrill.Enabled = true;
      }
      this.chkDefaultDrill.CheckedChanged += new EventHandler(this.chkDefaultDrill_CheckedChanged);
    }

    private void chkDefaultDrill_CheckedChanged(object sender, EventArgs e)
    {
      this.session.ConfigurationManager.SetCompanySetting("Dashboard", "DefaultDrilldownView", this.fileSystemEntry.Path);
      this.setDrilldownView(true);
    }

    private void DashboardTemplateForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      if (this.btnCancel.Visible)
        this.btnCancel.PerformClick();
      else
        this.btnExplorerCancel.PerformClick();
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      JedHelp.ShowHelp(this.emHelpLink1.HelpTag);
    }

    private void fsExplorer_Leave(object sender, EventArgs e) => this.setLastFolderViewed();

    public FileSystemEntry SelectedFolder
    {
      get => this.fsExplorer.CurrentFolder;
      set => this.fsExplorer.SetFolder(value);
    }

    public string[] GetSelectedDashboardSnapshotTemplates
    {
      get
      {
        return this.fsExplorer.SelectedItems.Count == 0 ? (string[]) null : this.fsExplorer.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (items => ((FileSystemEntry) items.Tag).ToString())).ToArray<string>();
      }
    }

    public string[] SelectedDashboardSnapshotTemplates
    {
      get
      {
        return this.fsExplorer.SelectedItems.Count == 0 ? (string[]) null : this.fsExplorer.SelectedItems.Select<GVItem, string>((Func<GVItem, string>) (items => ((FileSystemEntry) items.Tag).ToString())).ToArray<string>();
      }
    }

    public void HighlightDashboardSnapshotTemplates(List<string> values)
    {
      if (values.Count == 0)
        return;
      for (int index = 0; index < values.Count; ++index)
      {
        for (int nItemIndex = 0; nItemIndex < this.fsExplorer.GVItems.Count; ++nItemIndex)
        {
          if (values[index] == ((FileSystemEntry) this.fsExplorer.GVItems[nItemIndex].Tag).ToString())
          {
            this.fsExplorer.GVItems[nItemIndex].Selected = true;
            break;
          }
        }
      }
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DashboardTemplateControl));
      this.imageList = new ImageList(this.components);
      this.toolTip1 = new ToolTip(this.components);
      this.btnSave = new StandardIconButton();
      this.btnCancel = new Button();
      this.bpTop = new BorderPanel();
      this.pnlRight = new Panel();
      this.gcSnapshot = new GroupContainer();
      this.tabDefineTemplate = new TabControl();
      this.tpgSnapshot = new TabPage();
      this.bpBody = new BorderPanel();
      this.pnlTrendChart = new Panel();
      this.gcTrendTop = new GroupContainer();
      this.label6 = new Label();
      this.lblVerticalFieldTC = new Label();
      this.ctlTimeFrameFieldTC = new TimeFrameControl();
      this.ctlVerticleFieldTC = new VerticleFieldControl();
      this.lblHorizontalFieldTC = new Label();
      this.gcTrendBottom = new GroupContainer();
      this.cboMaxLinesTC = new ComboBox();
      this.lblGroupByTC = new Label();
      this.cboSubsetTypeTC = new ComboBox();
      this.lblMaxLinesStartTC = new Label();
      this.lblLineCountTC = new Label();
      this.ctlGroupByFieldTC = new GroupByControl();
      this.lblSubsetTypeMiddleTC = new Label();
      this.lblMaxLinesEndTC = new Label();
      this.lblSubsetTypeEndTC = new Label();
      this.lblSubsetTypeStartTC = new Label();
      this.pnlLoanTable = new Panel();
      this.gcLoanTableBottom = new GroupContainer();
      this.label5 = new Label();
      this.label1 = new Label();
      this.lblTimeFrameLT = new Label();
      this.tableSummaryCtl = new TableSummaryControl();
      this.nudMaxRowsLT = new NumericUpDown();
      this.lblMaxRowsEndLT = new Label();
      this.lblMaxRowsStartLT = new Label();
      this.ctlTimeFrameFieldLT = new TimeFrameControl();
      this.pnlUserTable = new Panel();
      this.gcUserTableTop = new GroupContainer();
      this.lblColumn3 = new Label();
      this.lblColumn1 = new Label();
      this.lblColumn2 = new Label();
      this.ctlSummaryFieldsUT = new SummaryFieldControl();
      this.lblGroupByUT = new Label();
      this.ctlGroupByFieldUT = new GroupByControl();
      this.gcUserTableBottom = new GroupContainer();
      this.pnlRowOrder = new Panel();
      this.ctlTimeFrameFieldUT = new TimeFrameControl();
      this.lblTimeFrameUT = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.tableSummaryCtlUserTable = new TableSummaryControl();
      this.pnlRowFilter = new Panel();
      this.lblUserTable = new Label();
      this.pnlBarChart = new Panel();
      this.gcBarChartTop = new GroupContainer();
      this.ctlVerticleFieldBC = new VerticleFieldControl();
      this.lblHorizontalFieldBC = new Label();
      this.ctlHorizontalFieldBC = new HorizontalFieldControl();
      this.lblVerticalFieldBC = new Label();
      this.gcBarCharBottom = new GroupContainer();
      this.label3 = new Label();
      this.lblBarCountBC = new Label();
      this.lblTimeFrameBC = new Label();
      this.lblSubsetTypeMiddleBC = new Label();
      this.cboSubsetTypeBC = new ComboBox();
      this.nudMaxBarsBC = new NumericUpDown();
      this.lblSubsetTypeEndBC = new Label();
      this.lblMaxBarsStartBC = new Label();
      this.lblSubsetTypeStartBC = new Label();
      this.ctlTimeFrameFieldBC = new TimeFrameControl();
      this.lblMaxBarsEndBC = new Label();
      this.lblBarChart = new Label();
      this.gpSnapshotTop = new GradientPanel();
      this.lblChartType = new Label();
      this.cboChartType = new ComboBox();
      this.chkDefaultDrill = new CheckBox();
      this.tpgFolders = new TabPage();
      this.pnlFolders = new Panel();
      this.tpgFilters = new TabPage();
      this.pnlFilters = new Panel();
      this.ctlAdvancedSearch = new AdvancedSearchControl();
      this.verSplitter = new CollapsibleSplitter();
      this.pnlLeft = new Panel();
      this.pnlExplorer = new Panel();
      this.bpBottom = new BorderPanel();
      this.emHelpLink1 = new EMHelpLink();
      this.btnExplorerOk = new Button();
      this.btnExplorerCancel = new Button();
      ((ISupportInitialize) this.btnSave).BeginInit();
      this.bpTop.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.gcSnapshot.SuspendLayout();
      this.tabDefineTemplate.SuspendLayout();
      this.tpgSnapshot.SuspendLayout();
      this.bpBody.SuspendLayout();
      this.pnlTrendChart.SuspendLayout();
      this.gcTrendTop.SuspendLayout();
      this.gcTrendBottom.SuspendLayout();
      this.pnlLoanTable.SuspendLayout();
      this.gcLoanTableBottom.SuspendLayout();
      this.nudMaxRowsLT.BeginInit();
      this.pnlUserTable.SuspendLayout();
      this.gcUserTableTop.SuspendLayout();
      this.gcUserTableBottom.SuspendLayout();
      this.pnlRowOrder.SuspendLayout();
      this.pnlRowFilter.SuspendLayout();
      this.pnlBarChart.SuspendLayout();
      this.gcBarChartTop.SuspendLayout();
      this.gcBarCharBottom.SuspendLayout();
      this.nudMaxBarsBC.BeginInit();
      this.gpSnapshotTop.SuspendLayout();
      this.tpgFolders.SuspendLayout();
      this.pnlFolders.SuspendLayout();
      this.tpgFilters.SuspendLayout();
      this.pnlFilters.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.pnlExplorer.SuspendLayout();
      this.bpBottom.SuspendLayout();
      this.SuspendLayout();
      this.imageList.ImageStream = (ImageListStreamer) componentResourceManager.GetObject("imageList.ImageStream");
      this.imageList.TransparentColor = Color.Transparent;
      this.imageList.Images.SetKeyName(0, "search");
      this.imageList.Images.SetKeyName(1, "searchOver");
      this.btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(632, 5);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 28;
      this.btnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.btnSave, "Save");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.BackColor = SystemColors.Control;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnCancel.Location = new Point(886, 11);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 20);
      this.btnCancel.TabIndex = 28;
      this.btnCancel.Text = "&Close";
      this.btnCancel.UseCompatibleTextRendering = true;
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.bpTop.Borders = AnchorStyles.None;
      this.bpTop.Controls.Add((Control) this.pnlRight);
      this.bpTop.Controls.Add((Control) this.verSplitter);
      this.bpTop.Controls.Add((Control) this.pnlLeft);
      this.bpTop.Dock = DockStyle.Fill;
      this.bpTop.Location = new Point(0, 0);
      this.bpTop.Name = "bpTop";
      this.bpTop.Size = new Size(969, 597);
      this.bpTop.TabIndex = 7;
      this.pnlRight.BackColor = Color.Transparent;
      this.pnlRight.Controls.Add((Control) this.gcSnapshot);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.Location = new Point(311, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(658, 597);
      this.pnlRight.TabIndex = 1;
      this.gcSnapshot.Borders = AnchorStyles.Bottom | AnchorStyles.Left;
      this.gcSnapshot.Controls.Add((Control) this.btnSave);
      this.gcSnapshot.Controls.Add((Control) this.tabDefineTemplate);
      this.gcSnapshot.Dock = DockStyle.Fill;
      this.gcSnapshot.HeaderForeColor = SystemColors.ControlText;
      this.gcSnapshot.Location = new Point(0, 0);
      this.gcSnapshot.Name = "gcSnapshot";
      this.gcSnapshot.Padding = new Padding(2, 2, 0, 0);
      this.gcSnapshot.Size = new Size(658, 597);
      this.gcSnapshot.TabIndex = 28;
      this.gcSnapshot.Text = "Snapshot Details";
      this.tabDefineTemplate.Controls.Add((Control) this.tpgSnapshot);
      this.tabDefineTemplate.Controls.Add((Control) this.tpgFolders);
      this.tabDefineTemplate.Controls.Add((Control) this.tpgFilters);
      this.tabDefineTemplate.Dock = DockStyle.Fill;
      this.tabDefineTemplate.Location = new Point(3, 27);
      this.tabDefineTemplate.Name = "tabDefineTemplate";
      this.tabDefineTemplate.Padding = new Point(7, 3);
      this.tabDefineTemplate.SelectedIndex = 0;
      this.tabDefineTemplate.Size = new Size(655, 569);
      this.tabDefineTemplate.TabIndex = 27;
      this.tpgSnapshot.Controls.Add((Control) this.bpBody);
      this.tpgSnapshot.Controls.Add((Control) this.gpSnapshotTop);
      this.tpgSnapshot.Location = new Point(4, 22);
      this.tpgSnapshot.Name = "tpgSnapshot";
      this.tpgSnapshot.Padding = new Padding(0, 2, 2, 2);
      this.tpgSnapshot.Size = new Size(647, 543);
      this.tpgSnapshot.TabIndex = 0;
      this.tpgSnapshot.Text = "Snapshot";
      this.tpgSnapshot.UseVisualStyleBackColor = true;
      this.bpBody.Controls.Add((Control) this.pnlTrendChart);
      this.bpBody.Controls.Add((Control) this.pnlLoanTable);
      this.bpBody.Controls.Add((Control) this.pnlUserTable);
      this.bpBody.Controls.Add((Control) this.pnlBarChart);
      this.bpBody.Dock = DockStyle.Fill;
      this.bpBody.Location = new Point(0, 33);
      this.bpBody.Name = "bpBody";
      this.bpBody.Size = new Size(645, 508);
      this.bpBody.TabIndex = 75;
      this.pnlTrendChart.Controls.Add((Control) this.gcTrendTop);
      this.pnlTrendChart.Controls.Add((Control) this.gcTrendBottom);
      this.pnlTrendChart.Location = new Point(554, 319);
      this.pnlTrendChart.Name = "pnlTrendChart";
      this.pnlTrendChart.Size = new Size(85, 59);
      this.pnlTrendChart.TabIndex = 71;
      this.gcTrendTop.Borders = AnchorStyles.Bottom;
      this.gcTrendTop.Controls.Add((Control) this.label6);
      this.gcTrendTop.Controls.Add((Control) this.lblVerticalFieldTC);
      this.gcTrendTop.Controls.Add((Control) this.ctlTimeFrameFieldTC);
      this.gcTrendTop.Controls.Add((Control) this.ctlVerticleFieldTC);
      this.gcTrendTop.Controls.Add((Control) this.lblHorizontalFieldTC);
      this.gcTrendTop.Dock = DockStyle.Top;
      this.gcTrendTop.HeaderForeColor = SystemColors.ControlText;
      this.gcTrendTop.Location = new Point(0, 0);
      this.gcTrendTop.Name = "gcTrendTop";
      this.gcTrendTop.Size = new Size(85, 185);
      this.gcTrendTop.TabIndex = 76;
      this.gcTrendTop.Text = "Axis Definition";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(10, 112);
      this.label6.Name = "label6";
      this.label6.Size = new Size(83, 13);
      this.label6.TabIndex = 450;
      this.label6.Text = "Display loans by";
      this.lblVerticalFieldTC.AutoSize = true;
      this.lblVerticalFieldTC.Location = new Point(10, 36);
      this.lblVerticalFieldTC.Name = "lblVerticalFieldTC";
      this.lblVerticalFieldTC.Size = new Size(157, 13);
      this.lblVerticalFieldTC.TabIndex = 449;
      this.lblVerticalFieldTC.Text = "Value to use for the vertical axis";
      this.ctlTimeFrameFieldTC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlTimeFrameFieldTC.Location = new Point(150, 112);
      this.ctlTimeFrameFieldTC.Name = "ctlTimeFrameFieldTC";
      this.ctlTimeFrameFieldTC.Size = new Size(228, 60);
      this.ctlTimeFrameFieldTC.TabIndex = 444;
      this.ctlTimeFrameFieldTC.TimeFrameFieldDefinition = (LoanReportFieldDef) null;
      this.ctlVerticleFieldTC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlVerticleFieldTC.Location = new Point(150, 49);
      this.ctlVerticleFieldTC.Name = "ctlVerticleFieldTC";
      this.ctlVerticleFieldTC.Size = new Size(310, 39);
      this.ctlVerticleFieldTC.SummaryType = ColumnSummaryType.None;
      this.ctlVerticleFieldTC.TabIndex = 443;
      this.ctlVerticleFieldTC.VerticleFieldDefinition = (LoanReportFieldDef) null;
      this.lblHorizontalFieldTC.AutoSize = true;
      this.lblHorizontalFieldTC.Location = new Point(10, 95);
      this.lblHorizontalFieldTC.Name = "lblHorizontalFieldTC";
      this.lblHorizontalFieldTC.Size = new Size(276, 13);
      this.lblHorizontalFieldTC.TabIndex = 448;
      this.lblHorizontalFieldTC.Text = "The horizontal axis displays loans within a range of dates.";
      this.gcTrendBottom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcTrendBottom.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcTrendBottom.Controls.Add((Control) this.cboMaxLinesTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblGroupByTC);
      this.gcTrendBottom.Controls.Add((Control) this.cboSubsetTypeTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblMaxLinesStartTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblLineCountTC);
      this.gcTrendBottom.Controls.Add((Control) this.ctlGroupByFieldTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblSubsetTypeMiddleTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblMaxLinesEndTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblSubsetTypeEndTC);
      this.gcTrendBottom.Controls.Add((Control) this.lblSubsetTypeStartTC);
      this.gcTrendBottom.HeaderForeColor = SystemColors.ControlText;
      this.gcTrendBottom.Location = new Point(0, 191);
      this.gcTrendBottom.Name = "gcTrendBottom";
      this.gcTrendBottom.Size = new Size(85, 206);
      this.gcTrendBottom.TabIndex = 458;
      this.gcTrendBottom.Text = "Chart Content";
      this.cboMaxLinesTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboMaxLinesTC.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboMaxLinesTC.FormattingEnabled = true;
      this.cboMaxLinesTC.Location = new Point(112, (int) sbyte.MaxValue);
      this.cboMaxLinesTC.Name = "cboMaxLinesTC";
      this.cboMaxLinesTC.Size = new Size(42, 21);
      this.cboMaxLinesTC.TabIndex = 462;
      this.cboMaxLinesTC.SelectionChangeCommitted += new EventHandler(this.cboMaxLinesTC_SelectionChangeCommitted);
      this.lblGroupByTC.AutoSize = true;
      this.lblGroupByTC.Location = new Point(10, 36);
      this.lblGroupByTC.Name = "lblGroupByTC";
      this.lblGroupByTC.Size = new Size(269, 13);
      this.lblGroupByTC.TabIndex = 0;
      this.lblGroupByTC.Text = "Field to use as the data source for each line in the chart";
      this.cboSubsetTypeTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.cboSubsetTypeTC.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSubsetTypeTC.FormattingEnabled = true;
      this.cboSubsetTypeTC.Location = new Point(281, 159);
      this.cboSubsetTypeTC.Name = "cboSubsetTypeTC";
      this.cboSubsetTypeTC.Size = new Size(85, 21);
      this.cboSubsetTypeTC.TabIndex = 442;
      this.cboSubsetTypeTC.SelectionChangeCommitted += new EventHandler(this.cboSubsetTypeTC_SelectionChangeCommitted);
      this.lblMaxLinesStartTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMaxLinesStartTC.AutoSize = true;
      this.lblMaxLinesStartTC.Location = new Point(10, 131);
      this.lblMaxLinesStartTC.Name = "lblMaxLinesStartTC";
      this.lblMaxLinesStartTC.Size = new Size(99, 13);
      this.lblMaxLinesStartTC.TabIndex = 441;
      this.lblMaxLinesStartTC.Text = "Show no more than";
      this.lblMaxLinesStartTC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblLineCountTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblLineCountTC.Location = new Point(115, 158);
      this.lblLineCountTC.Name = "lblLineCountTC";
      this.lblLineCountTC.Size = new Size(20, 23);
      this.lblLineCountTC.TabIndex = 461;
      this.lblLineCountTC.Text = "50";
      this.lblLineCountTC.TextAlign = ContentAlignment.MiddleCenter;
      this.ctlGroupByFieldTC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlGroupByFieldTC.GroupByFieldDefinition = (LoanReportFieldDef) null;
      this.ctlGroupByFieldTC.Location = new Point(150, 52);
      this.ctlGroupByFieldTC.Name = "ctlGroupByFieldTC";
      this.ctlGroupByFieldTC.Size = new Size(228, 60);
      this.ctlGroupByFieldTC.TabIndex = 456;
      this.lblSubsetTypeMiddleTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSubsetTypeMiddleTC.AutoSize = true;
      this.lblSubsetTypeMiddleTC.Location = new Point(133, 163);
      this.lblSubsetTypeMiddleTC.Name = "lblSubsetTypeMiddleTC";
      this.lblSubsetTypeMiddleTC.Size = new Size(147, 13);
      this.lblSubsetTypeMiddleTC.TabIndex = 460;
      this.lblSubsetTypeMiddleTC.Text = "lines, then show lines with the";
      this.lblSubsetTypeMiddleTC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMaxLinesEndTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblMaxLinesEndTC.AutoSize = true;
      this.lblMaxLinesEndTC.Location = new Point(156, 131);
      this.lblMaxLinesEndTC.Name = "lblMaxLinesEndTC";
      this.lblMaxLinesEndTC.Size = new Size(87, 13);
      this.lblMaxLinesEndTC.TabIndex = 457;
      this.lblMaxLinesEndTC.Text = "lines in the chart.";
      this.lblMaxLinesEndTC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeEndTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSubsetTypeEndTC.AutoSize = true;
      this.lblSubsetTypeEndTC.Location = new Point(369, 164);
      this.lblSubsetTypeEndTC.Name = "lblSubsetTypeEndTC";
      this.lblSubsetTypeEndTC.Size = new Size(41, 13);
      this.lblSubsetTypeEndTC.TabIndex = 459;
      this.lblSubsetTypeEndTC.Text = "values.";
      this.lblSubsetTypeEndTC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeStartTC.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblSubsetTypeStartTC.AutoSize = true;
      this.lblSubsetTypeStartTC.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeStartTC.Location = new Point(10, 163);
      this.lblSubsetTypeStartTC.Name = "lblSubsetTypeStartTC";
      this.lblSubsetTypeStartTC.Size = new Size(108, 13);
      this.lblSubsetTypeStartTC.TabIndex = 458;
      this.lblSubsetTypeStartTC.Text = "If there are more than";
      this.lblSubsetTypeStartTC.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlLoanTable.Controls.Add((Control) this.ctlFieldSelectionLT);
      this.pnlLoanTable.Controls.Add((Control) this.gcLoanTableBottom);
      this.pnlLoanTable.Location = new Point(560, 397);
      this.pnlLoanTable.Name = "pnlLoanTable";
      this.pnlLoanTable.Size = new Size(79, 98);
      this.pnlLoanTable.TabIndex = 72;
      this.ctlFieldSelectionLT.AllowDatabaseFieldsOnly = true;
      this.ctlFieldSelectionLT.Dock = DockStyle.Top;
      this.ctlFieldSelectionLT.Location = new Point(0, 0);
      this.ctlFieldSelectionLT.MaxSelectionCount = 20;
      this.ctlFieldSelectionLT.Name = "ctlFieldSelectionLT";
      this.ctlFieldSelectionLT.Size = new Size(79, 192);
      this.ctlFieldSelectionLT.TabIndex = 1;
      this.gcLoanTableBottom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcLoanTableBottom.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcLoanTableBottom.Controls.Add((Control) this.label5);
      this.gcLoanTableBottom.Controls.Add((Control) this.label1);
      this.gcLoanTableBottom.Controls.Add((Control) this.lblTimeFrameLT);
      this.gcLoanTableBottom.Controls.Add((Control) this.tableSummaryCtl);
      this.gcLoanTableBottom.Controls.Add((Control) this.nudMaxRowsLT);
      this.gcLoanTableBottom.Controls.Add((Control) this.lblMaxRowsEndLT);
      this.gcLoanTableBottom.Controls.Add((Control) this.lblMaxRowsStartLT);
      this.gcLoanTableBottom.Controls.Add((Control) this.ctlTimeFrameFieldLT);
      this.gcLoanTableBottom.HeaderForeColor = SystemColors.ControlText;
      this.gcLoanTableBottom.Location = new Point(0, 198);
      this.gcLoanTableBottom.Name = "gcLoanTableBottom";
      this.gcLoanTableBottom.Size = new Size(79, 247);
      this.gcLoanTableBottom.TabIndex = 428;
      this.gcLoanTableBottom.Text = "Rows";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(10, 53);
      this.label5.Name = "label5";
      this.label5.Size = new Size(83, 13);
      this.label5.TabIndex = 426;
      this.label5.Text = "Display loans by";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 156);
      this.label1.Name = "label1";
      this.label1.Size = new Size(110, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Display summary rows";
      this.lblTimeFrameLT.AutoSize = true;
      this.lblTimeFrameLT.Location = new Point(10, 36);
      this.lblTimeFrameLT.Name = "lblTimeFrameLT";
      this.lblTimeFrameLT.Size = new Size(235, 13);
      this.lblTimeFrameLT.TabIndex = 424;
      this.lblTimeFrameLT.Text = "Table rows display loans within a range of dates.";
      this.lblTimeFrameLT.TextAlign = ContentAlignment.MiddleLeft;
      this.tableSummaryCtl.IncludeAverage = false;
      this.tableSummaryCtl.IncludeMax = false;
      this.tableSummaryCtl.IncludeMin = false;
      this.tableSummaryCtl.IncludeTotal = false;
      this.tableSummaryCtl.Location = new Point(129, 156);
      this.tableSummaryCtl.Name = "tableSummaryCtl";
      this.tableSummaryCtl.Size = new Size(69, 87);
      this.tableSummaryCtl.TabIndex = 0;
      this.nudMaxRowsLT.Increment = new Decimal(new int[4]
      {
        100,
        0,
        0,
        0
      });
      this.nudMaxRowsLT.Location = new Point(129, 120);
      this.nudMaxRowsLT.Maximum = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.nudMaxRowsLT.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.nudMaxRowsLT.Name = "nudMaxRowsLT";
      this.nudMaxRowsLT.Size = new Size(53, 20);
      this.nudMaxRowsLT.TabIndex = 0;
      this.nudMaxRowsLT.Value = new Decimal(new int[4]
      {
        1000,
        0,
        0,
        0
      });
      this.nudMaxRowsLT.ValueChanged += new EventHandler(this.nudMaxRowsLT_ValueChanged);
      this.nudMaxRowsLT.KeyPress += new KeyPressEventHandler(this.nudMaxRowsLT_KeyPress);
      this.lblMaxRowsEndLT.AutoSize = true;
      this.lblMaxRowsEndLT.Location = new Point(187, 123);
      this.lblMaxRowsEndLT.Name = "lblMaxRowsEndLT";
      this.lblMaxRowsEndLT.Size = new Size(90, 13);
      this.lblMaxRowsEndLT.TabIndex = 425;
      this.lblMaxRowsEndLT.Text = "loans in the table.";
      this.lblMaxRowsEndLT.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMaxRowsStartLT.AutoSize = true;
      this.lblMaxRowsStartLT.Location = new Point(11, 123);
      this.lblMaxRowsStartLT.Name = "lblMaxRowsStartLT";
      this.lblMaxRowsStartLT.Size = new Size(99, 13);
      this.lblMaxRowsStartLT.TabIndex = 1;
      this.lblMaxRowsStartLT.Text = "Show no more than";
      this.lblMaxRowsStartLT.TextAlign = ContentAlignment.MiddleLeft;
      this.ctlTimeFrameFieldLT.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlTimeFrameFieldLT.Location = new Point(129, 53);
      this.ctlTimeFrameFieldLT.Name = "ctlTimeFrameFieldLT";
      this.ctlTimeFrameFieldLT.Size = new Size(228, 60);
      this.ctlTimeFrameFieldLT.TabIndex = 423;
      this.ctlTimeFrameFieldLT.TimeFrameFieldDefinition = (LoanReportFieldDef) null;
      this.pnlUserTable.Controls.Add((Control) this.gcUserTableTop);
      this.pnlUserTable.Controls.Add((Control) this.gcUserTableBottom);
      this.pnlUserTable.Controls.Add((Control) this.lblUserTable);
      this.pnlUserTable.Location = new Point(9, 6);
      this.pnlUserTable.Name = "pnlUserTable";
      this.pnlUserTable.Size = new Size(539, 498);
      this.pnlUserTable.TabIndex = 73;
      this.gcUserTableTop.Borders = AnchorStyles.Bottom;
      this.gcUserTableTop.Controls.Add((Control) this.lblColumn3);
      this.gcUserTableTop.Controls.Add((Control) this.lblColumn1);
      this.gcUserTableTop.Controls.Add((Control) this.lblColumn2);
      this.gcUserTableTop.Controls.Add((Control) this.ctlSummaryFieldsUT);
      this.gcUserTableTop.Controls.Add((Control) this.lblGroupByUT);
      this.gcUserTableTop.Controls.Add((Control) this.ctlGroupByFieldUT);
      this.gcUserTableTop.Dock = DockStyle.Top;
      this.gcUserTableTop.HeaderForeColor = SystemColors.ControlText;
      this.gcUserTableTop.Location = new Point(0, 0);
      this.gcUserTableTop.Name = "gcUserTableTop";
      this.gcUserTableTop.Size = new Size(539, 180);
      this.gcUserTableTop.TabIndex = 76;
      this.gcUserTableTop.Text = "Columns";
      this.lblColumn3.AutoSize = true;
      this.lblColumn3.Font = new Font("Arial", 8.25f);
      this.lblColumn3.Location = new Point(10, 88);
      this.lblColumn3.Name = "lblColumn3";
      this.lblColumn3.Size = new Size(99, 14);
      this.lblColumn3.TabIndex = 478;
      this.lblColumn3.Text = "Column 3 (optional)";
      this.lblColumn3.TextAlign = ContentAlignment.MiddleLeft;
      this.lblColumn1.AutoSize = true;
      this.lblColumn1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblColumn1.Location = new Point(10, 40);
      this.lblColumn1.Name = "lblColumn1";
      this.lblColumn1.Size = new Size(51, 14);
      this.lblColumn1.TabIndex = 476;
      this.lblColumn1.Text = "Column 1";
      this.lblColumn1.TextAlign = ContentAlignment.MiddleLeft;
      this.lblColumn2.AutoSize = true;
      this.lblColumn2.Font = new Font("Arial", 8.25f);
      this.lblColumn2.Location = new Point(10, 64);
      this.lblColumn2.Name = "lblColumn2";
      this.lblColumn2.Size = new Size(99, 14);
      this.lblColumn2.TabIndex = 477;
      this.lblColumn2.Text = "Column 2 (optional)";
      this.lblColumn2.TextAlign = ContentAlignment.MiddleLeft;
      this.ctlSummaryFieldsUT.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlSummaryFieldsUT.Location = new Point(126, 36);
      this.ctlSummaryFieldsUT.Name = "ctlSummaryFieldsUT";
      this.ctlSummaryFieldsUT.Size = new Size(420, 75);
      this.ctlSummaryFieldsUT.SummaryFieldDefinition1 = (LoanReportFieldDef) null;
      this.ctlSummaryFieldsUT.SummaryFieldDefinition2 = (LoanReportFieldDef) null;
      this.ctlSummaryFieldsUT.SummaryFieldDefinition3 = (LoanReportFieldDef) null;
      this.ctlSummaryFieldsUT.SummaryType1 = ColumnSummaryType.None;
      this.ctlSummaryFieldsUT.SummaryType2 = ColumnSummaryType.None;
      this.ctlSummaryFieldsUT.SummaryType3 = ColumnSummaryType.None;
      this.ctlSummaryFieldsUT.TabIndex = 457;
      this.lblGroupByUT.AutoSize = true;
      this.lblGroupByUT.Font = new Font("Arial", 8.25f);
      this.lblGroupByUT.Location = new Point(10, 112);
      this.lblGroupByUT.Name = "lblGroupByUT";
      this.lblGroupByUT.Size = new Size(97, 14);
      this.lblGroupByUT.TabIndex = 458;
      this.lblGroupByUT.Text = "Group Columns By";
      this.lblGroupByUT.TextAlign = ContentAlignment.MiddleLeft;
      this.ctlGroupByFieldUT.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlGroupByFieldUT.GroupByFieldDefinition = (LoanReportFieldDef) null;
      this.ctlGroupByFieldUT.Location = new Point(126, 112);
      this.ctlGroupByFieldUT.Name = "ctlGroupByFieldUT";
      this.ctlGroupByFieldUT.Size = new Size(228, 60);
      this.ctlGroupByFieldUT.TabIndex = 455;
      this.gcUserTableBottom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcUserTableBottom.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcUserTableBottom.Controls.Add((Control) this.pnlRowOrder);
      this.gcUserTableBottom.Controls.Add((Control) this.pnlRowFilter);
      this.gcUserTableBottom.HeaderForeColor = SystemColors.ControlText;
      this.gcUserTableBottom.Location = new Point(0, 178);
      this.gcUserTableBottom.Name = "gcUserTableBottom";
      this.gcUserTableBottom.Size = new Size(539, 298);
      this.gcUserTableBottom.TabIndex = 459;
      this.gcUserTableBottom.Text = "Rows";
      this.pnlRowOrder.BackColor = Color.Transparent;
      this.pnlRowOrder.Controls.Add((Control) this.ctlTimeFrameFieldUT);
      this.pnlRowOrder.Controls.Add((Control) this.lblTimeFrameUT);
      this.pnlRowOrder.Controls.Add((Control) this.label4);
      this.pnlRowOrder.Controls.Add((Control) this.label2);
      this.pnlRowOrder.Controls.Add((Control) this.tableSummaryCtlUserTable);
      this.pnlRowOrder.Dock = DockStyle.Top;
      this.pnlRowOrder.Location = new Point(0, 84);
      this.pnlRowOrder.Name = "pnlRowOrder";
      this.pnlRowOrder.Size = new Size(539, 195);
      this.pnlRowOrder.TabIndex = 460;
      this.ctlTimeFrameFieldUT.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlTimeFrameFieldUT.Location = new Point((int) sbyte.MaxValue, 25);
      this.ctlTimeFrameFieldUT.Name = "ctlTimeFrameFieldUT";
      this.ctlTimeFrameFieldUT.Size = new Size(335, 60);
      this.ctlTimeFrameFieldUT.TabIndex = 420;
      this.ctlTimeFrameFieldUT.TimeFrameFieldDefinition = (LoanReportFieldDef) null;
      this.lblTimeFrameUT.AutoSize = true;
      this.lblTimeFrameUT.Location = new Point(10, 5);
      this.lblTimeFrameUT.Name = "lblTimeFrameUT";
      this.lblTimeFrameUT.Size = new Size(235, 13);
      this.lblTimeFrameUT.TabIndex = 457;
      this.lblTimeFrameUT.Text = "Table rows display loans within a range of dates.";
      this.lblTimeFrameUT.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(11, 25);
      this.label4.Name = "label4";
      this.label4.Size = new Size(83, 13);
      this.label4.TabIndex = 458;
      this.label4.Text = "Display loans by";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(11, 95);
      this.label2.Name = "label2";
      this.label2.Size = new Size(110, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Display summary rows";
      this.tableSummaryCtlUserTable.IncludeAverage = false;
      this.tableSummaryCtlUserTable.IncludeMax = false;
      this.tableSummaryCtlUserTable.IncludeMin = false;
      this.tableSummaryCtlUserTable.IncludeTotal = false;
      this.tableSummaryCtlUserTable.Location = new Point((int) sbyte.MaxValue, 95);
      this.tableSummaryCtlUserTable.Name = "tableSummaryCtlUserTable";
      this.tableSummaryCtlUserTable.Size = new Size(66, 86);
      this.tableSummaryCtlUserTable.TabIndex = 0;
      this.pnlRowFilter.BackColor = Color.Transparent;
      this.pnlRowFilter.Controls.Add((Control) this.ctlUserFilterUT);
      this.pnlRowFilter.Dock = DockStyle.Top;
      this.pnlRowFilter.Location = new Point(0, 26);
      this.pnlRowFilter.Name = "pnlRowFilter";
      this.pnlRowFilter.Padding = new Padding(10, 5, 0, 0);
      this.pnlRowFilter.Size = new Size(539, 58);
      this.pnlRowFilter.TabIndex = 459;
      this.ctlUserFilterUT.Dock = DockStyle.Fill;
      this.ctlUserFilterUT.IncludeChildren = false;
      this.ctlUserFilterUT.Location = new Point(10, 5);
      this.ctlUserFilterUT.Name = "ctlUserFilterUT";
      this.ctlUserFilterUT.OrganizationId = -1;
      this.ctlUserFilterUT.RoleId = -2;
      this.ctlUserFilterUT.Size = new Size(529, 53);
      this.ctlUserFilterUT.TabIndex = 456;
      this.ctlUserFilterUT.UserGroupId = -1;
      this.lblUserTable.AutoSize = true;
      this.lblUserTable.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblUserTable.Location = new Point(524, 8);
      this.lblUserTable.Name = "lblUserTable";
      this.lblUserTable.Size = new Size(98, 13);
      this.lblUserTable.TabIndex = 419;
      this.lblUserTable.Text = "User Table (UT)";
      this.lblUserTable.TextAlign = ContentAlignment.MiddleLeft;
      this.lblUserTable.Visible = false;
      this.pnlBarChart.Controls.Add((Control) this.gcBarChartTop);
      this.pnlBarChart.Controls.Add((Control) this.gcBarCharBottom);
      this.pnlBarChart.Controls.Add((Control) this.lblBarChart);
      this.pnlBarChart.Location = new Point(556, 27);
      this.pnlBarChart.Name = "pnlBarChart";
      this.pnlBarChart.Size = new Size(75, 81);
      this.pnlBarChart.TabIndex = 70;
      this.gcBarChartTop.Borders = AnchorStyles.Bottom;
      this.gcBarChartTop.Controls.Add((Control) this.ctlVerticleFieldBC);
      this.gcBarChartTop.Controls.Add((Control) this.lblHorizontalFieldBC);
      this.gcBarChartTop.Controls.Add((Control) this.ctlHorizontalFieldBC);
      this.gcBarChartTop.Controls.Add((Control) this.lblVerticalFieldBC);
      this.gcBarChartTop.Dock = DockStyle.Top;
      this.gcBarChartTop.HeaderForeColor = SystemColors.ControlText;
      this.gcBarChartTop.Location = new Point(0, 0);
      this.gcBarChartTop.Name = "gcBarChartTop";
      this.gcBarChartTop.Size = new Size(75, 165);
      this.gcBarChartTop.TabIndex = 450;
      this.gcBarChartTop.Text = "Axis Definition";
      this.ctlVerticleFieldBC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlVerticleFieldBC.Location = new Point(109, 53);
      this.ctlVerticleFieldBC.Name = "ctlVerticleFieldBC";
      this.ctlVerticleFieldBC.Size = new Size(315, 39);
      this.ctlVerticleFieldBC.SummaryType = ColumnSummaryType.None;
      this.ctlVerticleFieldBC.TabIndex = 446;
      this.ctlVerticleFieldBC.VerticleFieldDefinition = (LoanReportFieldDef) null;
      this.lblHorizontalFieldBC.AutoSize = true;
      this.lblHorizontalFieldBC.Location = new Point(10, 99);
      this.lblHorizontalFieldBC.Name = "lblHorizontalFieldBC";
      this.lblHorizontalFieldBC.Size = new Size(163, 13);
      this.lblHorizontalFieldBC.TabIndex = 449;
      this.lblHorizontalFieldBC.Text = "Field to use for the horizontal axis";
      this.lblHorizontalFieldBC.TextAlign = ContentAlignment.MiddleLeft;
      this.ctlHorizontalFieldBC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlHorizontalFieldBC.HorizontalFieldDefinition = (LoanReportFieldDef) null;
      this.ctlHorizontalFieldBC.Location = new Point(109, 115);
      this.ctlHorizontalFieldBC.Name = "ctlHorizontalFieldBC";
      this.ctlHorizontalFieldBC.Size = new Size(228, 39);
      this.ctlHorizontalFieldBC.TabIndex = 447;
      this.lblVerticalFieldBC.AutoSize = true;
      this.lblVerticalFieldBC.Location = new Point(10, 36);
      this.lblVerticalFieldBC.Name = "lblVerticalFieldBC";
      this.lblVerticalFieldBC.Size = new Size(194, 13);
      this.lblVerticalFieldBC.TabIndex = 448;
      this.lblVerticalFieldBC.Text = "Measurement to use for the vertical axis";
      this.lblVerticalFieldBC.TextAlign = ContentAlignment.MiddleLeft;
      this.gcBarCharBottom.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gcBarCharBottom.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.gcBarCharBottom.Controls.Add((Control) this.label3);
      this.gcBarCharBottom.Controls.Add((Control) this.lblBarCountBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblTimeFrameBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblSubsetTypeMiddleBC);
      this.gcBarCharBottom.Controls.Add((Control) this.cboSubsetTypeBC);
      this.gcBarCharBottom.Controls.Add((Control) this.nudMaxBarsBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblSubsetTypeEndBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblMaxBarsStartBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblSubsetTypeStartBC);
      this.gcBarCharBottom.Controls.Add((Control) this.ctlTimeFrameFieldBC);
      this.gcBarCharBottom.Controls.Add((Control) this.lblMaxBarsEndBC);
      this.gcBarCharBottom.HeaderForeColor = SystemColors.ControlText;
      this.gcBarCharBottom.Location = new Point(0, 170);
      this.gcBarCharBottom.Name = "gcBarCharBottom";
      this.gcBarCharBottom.Size = new Size(75, 185);
      this.gcBarCharBottom.TabIndex = 449;
      this.gcBarCharBottom.Text = "Chart Content";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(10, 56);
      this.label3.Name = "label3";
      this.label3.Size = new Size(83, 13);
      this.label3.TabIndex = 449;
      this.label3.Text = "Display loans by";
      this.lblBarCountBC.Location = new Point(108, 153);
      this.lblBarCountBC.Name = "lblBarCountBC";
      this.lblBarCountBC.Size = new Size(20, 23);
      this.lblBarCountBC.TabIndex = 448;
      this.lblBarCountBC.Text = "50";
      this.lblBarCountBC.TextAlign = ContentAlignment.MiddleCenter;
      this.lblTimeFrameBC.AutoSize = true;
      this.lblTimeFrameBC.Location = new Point(10, 36);
      this.lblTimeFrameBC.Name = "lblTimeFrameBC";
      this.lblTimeFrameBC.Size = new Size(234, 13);
      this.lblTimeFrameBC.TabIndex = 446;
      this.lblTimeFrameBC.Text = "The chart displays loans within a range of dates.";
      this.lblTimeFrameBC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeMiddleBC.AutoSize = true;
      this.lblSubsetTypeMiddleBC.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeMiddleBC.Location = new Point(125, 157);
      this.lblSubsetTypeMiddleBC.Name = "lblSubsetTypeMiddleBC";
      this.lblSubsetTypeMiddleBC.Size = new Size(145, 13);
      this.lblSubsetTypeMiddleBC.TabIndex = 447;
      this.lblSubsetTypeMiddleBC.Text = "bars, then show bars with the";
      this.lblSubsetTypeMiddleBC.TextAlign = ContentAlignment.MiddleLeft;
      this.cboSubsetTypeBC.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboSubsetTypeBC.FormattingEnabled = true;
      this.cboSubsetTypeBC.Location = new Point(273, 153);
      this.cboSubsetTypeBC.Name = "cboSubsetTypeBC";
      this.cboSubsetTypeBC.Size = new Size(85, 21);
      this.cboSubsetTypeBC.TabIndex = 439;
      this.cboSubsetTypeBC.SelectionChangeCommitted += new EventHandler(this.cboSubsetTypeBC_SelectionChangeCommitted);
      this.nudMaxBarsBC.Increment = new Decimal(new int[4]
      {
        5,
        0,
        0,
        0
      });
      this.nudMaxBarsBC.Location = new Point(111, 131);
      this.nudMaxBarsBC.Maximum = new Decimal(new int[4]
      {
        50,
        0,
        0,
        0
      });
      this.nudMaxBarsBC.Minimum = new Decimal(new int[4]
      {
        1,
        0,
        0,
        0
      });
      this.nudMaxBarsBC.Name = "nudMaxBarsBC";
      this.nudMaxBarsBC.Size = new Size(42, 20);
      this.nudMaxBarsBC.TabIndex = 421;
      this.nudMaxBarsBC.Value = new Decimal(new int[4]
      {
        50,
        0,
        0,
        0
      });
      this.nudMaxBarsBC.ValueChanged += new EventHandler(this.nudMaxBarsBC_ValueChanged);
      this.nudMaxBarsBC.KeyPress += new KeyPressEventHandler(this.nudMaxBarsBC_KeyPress);
      this.lblSubsetTypeEndBC.AutoSize = true;
      this.lblSubsetTypeEndBC.Location = new Point(360, 157);
      this.lblSubsetTypeEndBC.Name = "lblSubsetTypeEndBC";
      this.lblSubsetTypeEndBC.Size = new Size(41, 13);
      this.lblSubsetTypeEndBC.TabIndex = 442;
      this.lblSubsetTypeEndBC.Text = "values.";
      this.lblSubsetTypeEndBC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblMaxBarsStartBC.AutoSize = true;
      this.lblMaxBarsStartBC.Location = new Point(10, 133);
      this.lblMaxBarsStartBC.Name = "lblMaxBarsStartBC";
      this.lblMaxBarsStartBC.Size = new Size(99, 13);
      this.lblMaxBarsStartBC.TabIndex = 422;
      this.lblMaxBarsStartBC.Text = "Show no more than";
      this.lblMaxBarsStartBC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeStartBC.AutoSize = true;
      this.lblSubsetTypeStartBC.ImageAlign = ContentAlignment.MiddleLeft;
      this.lblSubsetTypeStartBC.Location = new Point(10, 158);
      this.lblSubsetTypeStartBC.Name = "lblSubsetTypeStartBC";
      this.lblSubsetTypeStartBC.Size = new Size(108, 13);
      this.lblSubsetTypeStartBC.TabIndex = 441;
      this.lblSubsetTypeStartBC.Text = "If there are more than";
      this.lblSubsetTypeStartBC.TextAlign = ContentAlignment.MiddleLeft;
      this.ctlTimeFrameFieldBC.FieldDefinitions = (LoanReportFieldDefs) null;
      this.ctlTimeFrameFieldBC.Location = new Point(109, 56);
      this.ctlTimeFrameFieldBC.Name = "ctlTimeFrameFieldBC";
      this.ctlTimeFrameFieldBC.Size = new Size(228, 60);
      this.ctlTimeFrameFieldBC.TabIndex = 445;
      this.ctlTimeFrameFieldBC.TimeFrameFieldDefinition = (LoanReportFieldDef) null;
      this.lblMaxBarsEndBC.AutoSize = true;
      this.lblMaxBarsEndBC.Location = new Point(158, 133);
      this.lblMaxBarsEndBC.Name = "lblMaxBarsEndBC";
      this.lblMaxBarsEndBC.Size = new Size(86, 13);
      this.lblMaxBarsEndBC.TabIndex = 440;
      this.lblMaxBarsEndBC.Text = "bars in the chart.";
      this.lblMaxBarsEndBC.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBarChart.AutoSize = true;
      this.lblBarChart.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblBarChart.Location = new Point(534, 8);
      this.lblBarChart.Name = "lblBarChart";
      this.lblBarChart.Size = new Size(88, 13);
      this.lblBarChart.TabIndex = 444;
      this.lblBarChart.Text = "Bar Chart (BC)";
      this.lblBarChart.TextAlign = ContentAlignment.MiddleLeft;
      this.lblBarChart.Visible = false;
      this.gpSnapshotTop.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.gpSnapshotTop.Controls.Add((Control) this.lblChartType);
      this.gpSnapshotTop.Controls.Add((Control) this.cboChartType);
      this.gpSnapshotTop.Controls.Add((Control) this.chkDefaultDrill);
      this.gpSnapshotTop.Dock = DockStyle.Top;
      this.gpSnapshotTop.GradientColor1 = Color.FromArgb(252, 252, 252);
      this.gpSnapshotTop.GradientColor2 = Color.FromArgb(224, 224, 224);
      this.gpSnapshotTop.Location = new Point(0, 2);
      this.gpSnapshotTop.Name = "gpSnapshotTop";
      this.gpSnapshotTop.Size = new Size(645, 31);
      this.gpSnapshotTop.Style = GradientPanel.PanelStyle.PageSubHeader;
      this.gpSnapshotTop.TabIndex = 74;
      this.lblChartType.AutoSize = true;
      this.lblChartType.Location = new Point(6, 8);
      this.lblChartType.Name = "lblChartType";
      this.lblChartType.Size = new Size(59, 13);
      this.lblChartType.TabIndex = 66;
      this.lblChartType.Text = "Chart Type";
      this.lblChartType.TextAlign = ContentAlignment.MiddleLeft;
      this.cboChartType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboChartType.Items.AddRange(new object[4]
      {
        (object) "Bar Chart",
        (object) "Trend Chart",
        (object) "Loan Table",
        (object) "User Table"
      });
      this.cboChartType.Location = new Point(73, 6);
      this.cboChartType.Name = "cboChartType";
      this.cboChartType.Size = new Size(206, 21);
      this.cboChartType.TabIndex = 65;
      this.cboChartType.SelectionChangeCommitted += new EventHandler(this.cboChartType_SelectionChangeCommitted);
      this.chkDefaultDrill.AutoSize = true;
      this.chkDefaultDrill.Location = new Point(289, 8);
      this.chkDefaultDrill.Name = "chkDefaultDrill";
      this.chkDefaultDrill.Size = new Size(189, 17);
      this.chkDefaultDrill.TabIndex = 427;
      this.chkDefaultDrill.Text = "Default Drilldown Loan Table View";
      this.chkDefaultDrill.UseVisualStyleBackColor = true;
      this.chkDefaultDrill.CheckedChanged += new EventHandler(this.chkDefaultDrill_CheckedChanged);
      this.tpgFolders.Controls.Add((Control) this.pnlFolders);
      this.tpgFolders.Location = new Point(4, 22);
      this.tpgFolders.Name = "tpgFolders";
      this.tpgFolders.Padding = new Padding(0, 2, 2, 2);
      this.tpgFolders.Size = new Size(651, 543);
      this.tpgFolders.TabIndex = 2;
      this.tpgFolders.Text = "Folders";
      this.tpgFolders.UseVisualStyleBackColor = true;
      this.pnlFolders.Controls.Add((Control) this.ctlSelectFolder);
      this.pnlFolders.Dock = DockStyle.Fill;
      this.pnlFolders.Location = new Point(0, 2);
      this.pnlFolders.Margin = new Padding(0);
      this.pnlFolders.Name = "pnlFolders";
      this.pnlFolders.Size = new Size(649, 539);
      this.pnlFolders.TabIndex = 1;
      this.ctlSelectFolder.BackColor = Color.Transparent;
      this.ctlSelectFolder.Dock = DockStyle.Fill;
      this.ctlSelectFolder.Location = new Point(0, 0);
      this.ctlSelectFolder.Margin = new Padding(0);
      this.ctlSelectFolder.Name = "ctlSelectFolder";
      this.ctlSelectFolder.Size = new Size(649, 539);
      this.ctlSelectFolder.TabIndex = 0;
      this.tpgFilters.Controls.Add((Control) this.pnlFilters);
      this.tpgFilters.Location = new Point(4, 22);
      this.tpgFilters.Name = "tpgFilters";
      this.tpgFilters.Padding = new Padding(0, 2, 2, 2);
      this.tpgFilters.Size = new Size(651, 543);
      this.tpgFilters.TabIndex = 3;
      this.tpgFilters.Text = "Filters";
      this.tpgFilters.UseVisualStyleBackColor = true;
      this.pnlFilters.Controls.Add((Control) this.ctlAdvancedSearch);
      this.pnlFilters.Dock = DockStyle.Fill;
      this.pnlFilters.Location = new Point(0, 2);
      this.pnlFilters.Name = "pnlFilters";
      this.pnlFilters.Size = new Size(649, 539);
      this.pnlFilters.TabIndex = 1;
      this.ctlAdvancedSearch.AllowDynamicOperators = false;
      this.ctlAdvancedSearch.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ctlAdvancedSearch.Dock = DockStyle.Fill;
      this.ctlAdvancedSearch.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.ctlAdvancedSearch.Location = new Point(0, 0);
      this.ctlAdvancedSearch.Margin = new Padding(0);
      this.ctlAdvancedSearch.Name = "ctlAdvancedSearch";
      this.ctlAdvancedSearch.Size = new Size(649, 539);
      this.ctlAdvancedSearch.TabIndex = 2;
      this.ctlAdvancedSearch.Title = "Filters";
      this.verSplitter.AnimationDelay = 20;
      this.verSplitter.AnimationStep = 20;
      this.verSplitter.BorderStyle3D = Border3DStyle.Flat;
      this.verSplitter.ControlToHide = (Control) this.pnlLeft;
      this.verSplitter.ExpandParentForm = false;
      this.verSplitter.Location = new Point(304, 0);
      this.verSplitter.Name = "verSplitter";
      this.verSplitter.TabIndex = 7;
      this.verSplitter.TabStop = false;
      this.verSplitter.UseAnimations = false;
      this.verSplitter.VisualStyle = VisualStyles.Encompass;
      this.pnlLeft.Controls.Add((Control) this.pnlExplorer);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(304, 597);
      this.pnlLeft.TabIndex = 6;
      this.pnlExplorer.Controls.Add((Control) this.fsExplorer);
      this.pnlExplorer.Dock = DockStyle.Fill;
      this.pnlExplorer.Location = new Point(0, 0);
      this.pnlExplorer.Name = "pnlExplorer";
      this.pnlExplorer.Size = new Size(304, 597);
      this.pnlExplorer.TabIndex = 0;
      this.fsExplorer.Dock = DockStyle.Fill;
      this.fsExplorer.FolderComboSelectedIndex = -1;
      this.fsExplorer.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.fsExplorer.HasPublicRight = true;
      this.fsExplorer.Location = new Point(0, 0);
      this.fsExplorer.Name = "fsExplorer";
      this.fsExplorer.RenameButtonSize = new Size(62, 22);
      this.fsExplorer.RESPAMode = FSExplorer.RESPAFilter.All;
      this.fsExplorer.setContactType = EllieMae.EMLite.ContactUI.ContactType.BizPartner;
      this.fsExplorer.Size = new Size(304, 597);
      this.fsExplorer.TabIndex = 1;
      this.bpBottom.BackColor = Color.WhiteSmoke;
      this.bpBottom.Borders = AnchorStyles.None;
      this.bpBottom.Controls.Add((Control) this.emHelpLink1);
      this.bpBottom.Controls.Add((Control) this.btnExplorerOk);
      this.bpBottom.Controls.Add((Control) this.btnCancel);
      this.bpBottom.Controls.Add((Control) this.btnExplorerCancel);
      this.bpBottom.Dock = DockStyle.Bottom;
      this.bpBottom.Location = new Point(0, 597);
      this.bpBottom.Name = "bpBottom";
      this.bpBottom.Size = new Size(969, 41);
      this.bpBottom.TabIndex = 8;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.HelpTag = "DashBoard";
      this.emHelpLink1.Location = new Point(12, 13);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 30;
      this.btnExplorerOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnExplorerOk.BackColor = SystemColors.Control;
      this.btnExplorerOk.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnExplorerOk.Location = new Point(135, 12);
      this.btnExplorerOk.Margin = new Padding(0);
      this.btnExplorerOk.Name = "btnExplorerOk";
      this.btnExplorerOk.Size = new Size(75, 20);
      this.btnExplorerOk.TabIndex = 1;
      this.btnExplorerOk.Text = "Select";
      this.btnExplorerOk.UseCompatibleTextRendering = true;
      this.btnExplorerOk.UseVisualStyleBackColor = true;
      this.btnExplorerOk.Click += new EventHandler(this.btnExplorerOK_Click);
      this.btnExplorerCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnExplorerCancel.BackColor = SystemColors.Control;
      this.btnExplorerCancel.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnExplorerCancel.Location = new Point(216, 12);
      this.btnExplorerCancel.Margin = new Padding(0);
      this.btnExplorerCancel.Name = "btnExplorerCancel";
      this.btnExplorerCancel.Size = new Size(75, 20);
      this.btnExplorerCancel.TabIndex = 0;
      this.btnExplorerCancel.Text = "Cancel";
      this.btnExplorerCancel.UseCompatibleTextRendering = true;
      this.btnExplorerCancel.UseVisualStyleBackColor = true;
      this.btnExplorerCancel.Click += new EventHandler(this.btnExplorerCancel_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(969, 638);
      this.Controls.Add((Control) this.bpTop);
      this.Controls.Add((Control) this.bpBottom);
      this.MaximumSize = new Size(975, 670);
      this.MinimumSize = new Size(312, 560);
      this.Name = "DashboardTemplateForm";
      this.Text = "Manage Snapshots";
      this.KeyUp += new KeyEventHandler(this.DashboardTemplateForm_KeyUp);
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      ((ISupportInitialize) this.btnSave).EndInit();
      this.bpTop.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.gcSnapshot.ResumeLayout(false);
      this.tabDefineTemplate.ResumeLayout(false);
      this.tpgSnapshot.ResumeLayout(false);
      this.bpBody.ResumeLayout(false);
      this.pnlTrendChart.ResumeLayout(false);
      this.gcTrendTop.ResumeLayout(false);
      this.gcTrendTop.PerformLayout();
      this.gcTrendBottom.ResumeLayout(false);
      this.gcTrendBottom.PerformLayout();
      this.pnlLoanTable.ResumeLayout(false);
      this.gcLoanTableBottom.ResumeLayout(false);
      this.gcLoanTableBottom.PerformLayout();
      this.nudMaxRowsLT.EndInit();
      this.pnlUserTable.ResumeLayout(false);
      this.pnlUserTable.PerformLayout();
      this.gcUserTableTop.ResumeLayout(false);
      this.gcUserTableTop.PerformLayout();
      this.gcUserTableBottom.ResumeLayout(false);
      this.pnlRowOrder.ResumeLayout(false);
      this.pnlRowOrder.PerformLayout();
      this.pnlRowFilter.ResumeLayout(false);
      this.pnlBarChart.ResumeLayout(false);
      this.pnlBarChart.PerformLayout();
      this.gcBarChartTop.ResumeLayout(false);
      this.gcBarChartTop.PerformLayout();
      this.gcBarCharBottom.ResumeLayout(false);
      this.gcBarCharBottom.PerformLayout();
      this.nudMaxBarsBC.EndInit();
      this.gpSnapshotTop.ResumeLayout(false);
      this.gpSnapshotTop.PerformLayout();
      this.tpgFolders.ResumeLayout(false);
      this.pnlFolders.ResumeLayout(false);
      this.tpgFilters.ResumeLayout(false);
      this.pnlFilters.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.pnlExplorer.ResumeLayout(false);
      this.bpBottom.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public enum ProcessingMode
    {
      Unspecified,
      SelectTemplate,
      EditTemplate,
      ManageTemplates,
    }
  }
}
