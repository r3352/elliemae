// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.DataTableDlg
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup.DynamicDataManagement;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class DataTableDlg : Form
  {
    private const string className = "DataTableDlg";
    private static readonly string sw = Tracing.SwReportControl;
    private IContainer components;
    private Button btnCancel;
    private Button btnUpdate;
    private FieldSettings _fieldSettings;
    private Panel panel3;
    private ToolTip toolTip;
    private Panel panel1;
    private TextBox textBoxDDMTableDescription;
    private TextBox textBoxDDMTableName;
    private Label label2;
    private Label label1;
    private DDMDataTableInfo ddmDataTableInfo;
    private StandardFields _standardFields;
    private Label label3;
    private GroupContainer grpSource;
    private CollapsibleSplitter collapsibleSplitter1;
    private Panel panel4;
    private GroupContainer grpSelected;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton downArrowButton;
    private StandardIconButton upArrowButton;
    private Button addBtn;
    private Button removeBtn;
    private Panel panel2;
    private List<FieldDefinition> FieldsToRemove = new List<FieldDefinition>();
    private const string MODIFYDATATABLE = "Modify Data Table";
    private DDMDataTableInfo originalDataTableInfo;
    private string originalTableNameValue = string.Empty;
    private bool _importMode;
    private int _importColumnCount;
    private int _importRowCount;
    private Dictionary<string, List<string>> _dataTableData;
    private Dictionary<string, List<string>> _outputColDataTableData;
    private bool _overwriteExisting;
    private DataTableDlg.DataTableDlgImportHandler _importHandler;
    private TabControl tabControlFields;
    private TabPage tabFields;
    private GridView listViewSource;
    private Button findBtn;
    private TextBox textBoxFind;
    private TabPage tabLA;
    private TabPage tabMilestones;
    private TabPage tabDocuments;
    private TabPage tabTasks;
    private TabPage tabConditions;
    private TabPage tabPreliminary;
    private TabPage tabPost;
    private TabPage tabPageLock;
    private TabPage tabPageIS;
    private TabPage tabPageGFE;
    private TabPage tabPageTIL;
    private TabPage tabPageLECD;
    private TabPage tabPageEDisclosures;
    private TabPage tabPageAlerts;
    private TabPage tabPageAUSTracking;
    private GridView listViewDocument;
    private GridView listViewTask;
    private GridView listViewCondition;
    private GridView listViewPreliminary;
    private GridView listViewPost;
    private GridView listViewLocks;
    private GridView listViewIS;
    private GridView listViewGFE;
    private GridView listViewTIL;
    private GridView listViewLECD;
    private GridView listViewEDisclosures;
    private GridView listViewAlerts;
    private GridView listViewAUSTracking;
    private GridView listViewMilestone;
    private GridView listViewMember;
    private DialogResult userResponseOnUpdate = DialogResult.Yes;
    private TabControl tabSelectedFields;
    private TabPage tabPage1;
    private GridView listViewTarget;
    private DialogResult userResponseOnNameChange;
    private DataTableOutputColumnSetupDialog outputColumns;
    private Button btn_back;
    private EMHelpLink emHelpLink1;
    private bool BackButtonPressed;

    public DDMDataTable ImportedDataTable { get; private set; }

    public DataTableDlg()
      : this((StandardFields) null, (FieldSettings) null)
    {
    }

    public DataTableDlg(StandardFields standardFields, FieldSettings fieldSettings)
    {
      try
      {
        this._standardFields = standardFields ?? Session.LoanManager.GetStandardFields();
        this._fieldSettings = fieldSettings ?? Session.LoanManager.GetFieldSettings();
      }
      catch (Exception ex)
      {
        this._standardFields = StandardFields.Instance;
        int num = (int) Utils.Dialog((IWin32Window) this, "Error getting standard fields definition from server. Use local definition file instead.\r\n" + ex.Message);
      }
      this.ddmDataTableInfo = new DDMDataTableInfo();
      this.InitializeComponent();
      this.btnReset_Click((object) null, (EventArgs) null);
      this.listViewSource.Sort(0, SortOrder.Ascending);
      this.listViewTarget.Sort(0, SortOrder.Ascending);
      this.enableUpdate();
      this.originalTableNameValue = this.textBoxDDMTableName.Text;
      this.handleNormalFieldsTabAssets(true);
    }

    public DataTableDlg(
      DDMDataTable dataTable,
      string[] columns,
      string[] outputCol,
      int rowCount,
      Dictionary<string, List<string>> dataTableData,
      Dictionary<string, List<string>> outputColDataTableData,
      bool csvHasHeader,
      bool overwriteExisting,
      StandardFields standardFields,
      FieldSettings fieldSettings)
      : this(standardFields, fieldSettings)
    {
      this._importMode = true;
      this._dataTableData = dataTableData;
      this._outputColDataTableData = outputColDataTableData;
      this._importRowCount = rowCount;
      this._overwriteExisting = overwriteExisting;
      this.textBoxDDMTableName.Text = dataTable.Name;
      this.textBoxDDMTableDescription.Text = dataTable.Description;
      this._importHandler = new DataTableDlg.DataTableDlgImportHandler(this);
      this._importHandler.SetupUIForImport();
      this._importHandler.SetTargetListViewData(columns, csvHasHeader);
      this.btnReset_Click((object) null, (EventArgs) null);
      this.enableUpdate();
      this.originalTableNameValue = this.textBoxDDMTableName.Text;
      this.listViewTarget.ItemDoubleClick += new GVItemEventHandler(this._importHandler.listViewTarget_ItemDoubleClick);
      this.removeBtn.Visible = false;
      this.addBtn.Text = "Replace";
      this.handleNormalFieldsTabAssets(true);
      this.btn_back.Visible = true;
    }

    private DDMDataTableInfo copyDataTableInfo(DDMDataTableInfo ddmInfo)
    {
      if (ddmInfo == null || ddmInfo.Fields == null)
        return (DDMDataTableInfo) null;
      DDMDataTableInfo ddmDataTableInfo = new DDMDataTableInfo()
      {
        Fields = new DDMDataTableFieldInfo[((IEnumerable<DDMDataTableFieldInfo>) ddmInfo.Fields).Count<DDMDataTableFieldInfo>()],
        Name = ddmInfo.Name,
        Description = ddmInfo.Description
      };
      int index = 0;
      foreach (DDMDataTableFieldInfo field in ddmInfo.Fields)
      {
        ddmDataTableInfo.Fields[index] = new DDMDataTableFieldInfo(field.FieldId, field.Description, field.Type, field.Category, field.ComortgagorPair, field.Format);
        ddmDataTableInfo.Fields[index].IsOutput = field.IsOutput;
        ++index;
      }
      return ddmDataTableInfo;
    }

    public DataTableDlg(DDMDataTableInfo ddmInfo)
      : this(ddmInfo, (StandardFields) null, (FieldSettings) null)
    {
    }

    public DataTableDlg(
      DDMDataTableInfo ddmInfo,
      StandardFields standardFields,
      FieldSettings fieldSettings)
    {
      try
      {
        this._standardFields = standardFields ?? Session.LoanManager.GetStandardFields();
        this._fieldSettings = fieldSettings ?? Session.LoanManager.GetFieldSettings();
      }
      catch (Exception ex)
      {
        this._standardFields = StandardFields.Instance;
        int num = (int) Utils.Dialog((IWin32Window) this, "Error getting standard fields definition from server. Use local definition file instead.\r\n" + ex.Message);
      }
      this.ddmDataTableInfo = ddmInfo;
      this.originalDataTableInfo = this.copyDataTableInfo(ddmInfo);
      this.InitializeComponent();
      this.Text = "Modify Data Table";
      this.textBoxDDMTableName.Text = this.ddmDataTableInfo.Name;
      this.textBoxDDMTableDescription.Text = this.ddmDataTableInfo.Description;
      if (this.ddmDataTableInfo == null || this.ddmDataTableInfo.Fields == null)
        return;
      foreach (DDMDataTableFieldInfo field1 in this.ddmDataTableInfo.Fields)
      {
        if (!field1.IsOutput)
        {
          FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(field1.FieldId);
          FieldDefinition field2 = EncompassFields.GetField(fieldPairInfo.FieldID, this._fieldSettings);
          if (field2.Category != FieldCategory.Common)
            field2.FieldID = FieldPairParser.GetFieldIDForBorrowerPair(fieldPairInfo.FieldID, fieldPairInfo.PairIndex > 1 ? fieldPairInfo.PairIndex : 1);
          this.addSourceField(this.listViewTarget, field2, false);
          this.FieldsToRemove.Add(field2);
        }
      }
      this.btnReset_Click((object) null, (EventArgs) null);
      this.listViewSource.Sort(0, SortOrder.Ascending);
      this.updateFieldCounter();
      this.enableUpdate();
      this.originalTableNameValue = this.textBoxDDMTableName.Text;
    }

    public DDMDataTableInfo DdmDataTableInfo
    {
      get => this.ddmDataTableInfo;
      set => this.ddmDataTableInfo = value;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      GVColumn gvColumn1 = new GVColumn();
      GVColumn gvColumn2 = new GVColumn();
      GVColumn gvColumn3 = new GVColumn();
      GVColumn gvColumn4 = new GVColumn();
      GVColumn gvColumn5 = new GVColumn();
      GVColumn gvColumn6 = new GVColumn();
      GVColumn gvColumn7 = new GVColumn();
      GVColumn gvColumn8 = new GVColumn();
      GVColumn gvColumn9 = new GVColumn();
      GVColumn gvColumn10 = new GVColumn();
      GVColumn gvColumn11 = new GVColumn();
      GVColumn gvColumn12 = new GVColumn();
      GVColumn gvColumn13 = new GVColumn();
      GVColumn gvColumn14 = new GVColumn();
      GVColumn gvColumn15 = new GVColumn();
      GVColumn gvColumn16 = new GVColumn();
      GVColumn gvColumn17 = new GVColumn();
      GVColumn gvColumn18 = new GVColumn();
      GVColumn gvColumn19 = new GVColumn();
      GVColumn gvColumn20 = new GVColumn();
      GVColumn gvColumn21 = new GVColumn();
      GVColumn gvColumn22 = new GVColumn();
      GVColumn gvColumn23 = new GVColumn();
      GVColumn gvColumn24 = new GVColumn();
      GVColumn gvColumn25 = new GVColumn();
      GVColumn gvColumn26 = new GVColumn();
      GVColumn gvColumn27 = new GVColumn();
      GVColumn gvColumn28 = new GVColumn();
      GVColumn gvColumn29 = new GVColumn();
      GVColumn gvColumn30 = new GVColumn();
      GVColumn gvColumn31 = new GVColumn();
      GVColumn gvColumn32 = new GVColumn();
      GVColumn gvColumn33 = new GVColumn();
      GVColumn gvColumn34 = new GVColumn();
      GVColumn gvColumn35 = new GVColumn();
      GVColumn gvColumn36 = new GVColumn();
      GVColumn gvColumn37 = new GVColumn();
      GVColumn gvColumn38 = new GVColumn();
      GVColumn gvColumn39 = new GVColumn();
      GVColumn gvColumn40 = new GVColumn();
      GVColumn gvColumn41 = new GVColumn();
      GVColumn gvColumn42 = new GVColumn();
      GVColumn gvColumn43 = new GVColumn();
      GVColumn gvColumn44 = new GVColumn();
      GVColumn gvColumn45 = new GVColumn();
      GVColumn gvColumn46 = new GVColumn();
      GVColumn gvColumn47 = new GVColumn();
      GVColumn gvColumn48 = new GVColumn();
      GVColumn gvColumn49 = new GVColumn();
      GVColumn gvColumn50 = new GVColumn();
      GVColumn gvColumn51 = new GVColumn();
      GVColumn gvColumn52 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DataTableDlg));
      this.btnCancel = new Button();
      this.btnUpdate = new Button();
      this.panel3 = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      this.btn_back = new Button();
      this.toolTip = new ToolTip(this.components);
      this.downArrowButton = new StandardIconButton();
      this.upArrowButton = new StandardIconButton();
      this.panel1 = new Panel();
      this.label3 = new Label();
      this.textBoxDDMTableDescription = new TextBox();
      this.textBoxDDMTableName = new TextBox();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel4 = new Panel();
      this.grpSelected = new GroupContainer();
      this.tabSelectedFields = new TabControl();
      this.tabPage1 = new TabPage();
      this.listViewTarget = new GridView();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.addBtn = new Button();
      this.removeBtn = new Button();
      this.panel2 = new Panel();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.grpSource = new GroupContainer();
      this.tabControlFields = new TabControl();
      this.tabFields = new TabPage();
      this.listViewSource = new GridView();
      this.findBtn = new Button();
      this.textBoxFind = new TextBox();
      this.tabLA = new TabPage();
      this.listViewMember = new GridView();
      this.tabMilestones = new TabPage();
      this.listViewMilestone = new GridView();
      this.tabDocuments = new TabPage();
      this.listViewDocument = new GridView();
      this.tabTasks = new TabPage();
      this.listViewTask = new GridView();
      this.tabConditions = new TabPage();
      this.listViewCondition = new GridView();
      this.tabPreliminary = new TabPage();
      this.listViewPreliminary = new GridView();
      this.tabPost = new TabPage();
      this.listViewPost = new GridView();
      this.tabPageLock = new TabPage();
      this.listViewLocks = new GridView();
      this.tabPageIS = new TabPage();
      this.listViewIS = new GridView();
      this.tabPageGFE = new TabPage();
      this.listViewGFE = new GridView();
      this.tabPageTIL = new TabPage();
      this.listViewTIL = new GridView();
      this.tabPageLECD = new TabPage();
      this.listViewLECD = new GridView();
      this.tabPageEDisclosures = new TabPage();
      this.listViewEDisclosures = new GridView();
      this.tabPageAlerts = new TabPage();
      this.listViewAlerts = new GridView();
      this.tabPageAUSTracking = new TabPage();
      this.listViewAUSTracking = new GridView();
      this.panel3.SuspendLayout();
      ((ISupportInitialize) this.downArrowButton).BeginInit();
      ((ISupportInitialize) this.upArrowButton).BeginInit();
      this.panel1.SuspendLayout();
      this.panel4.SuspendLayout();
      this.grpSelected.SuspendLayout();
      this.tabSelectedFields.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.grpSource.SuspendLayout();
      this.tabControlFields.SuspendLayout();
      this.tabFields.SuspendLayout();
      this.tabLA.SuspendLayout();
      this.tabMilestones.SuspendLayout();
      this.tabDocuments.SuspendLayout();
      this.tabTasks.SuspendLayout();
      this.tabConditions.SuspendLayout();
      this.tabPreliminary.SuspendLayout();
      this.tabPost.SuspendLayout();
      this.tabPageLock.SuspendLayout();
      this.tabPageIS.SuspendLayout();
      this.tabPageGFE.SuspendLayout();
      this.tabPageTIL.SuspendLayout();
      this.tabPageLECD.SuspendLayout();
      this.tabPageEDisclosures.SuspendLayout();
      this.tabPageAlerts.SuspendLayout();
      this.tabPageAUSTracking.SuspendLayout();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(794, 9);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnUpdate.Location = new Point(716, 9);
      this.btnUpdate.Name = "btnUpdate";
      this.btnUpdate.Size = new Size(75, 22);
      this.btnUpdate.TabIndex = 4;
      this.btnUpdate.Text = "&Next";
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.panel3.Controls.Add((Control) this.emHelpLink1);
      this.panel3.Controls.Add((Control) this.btn_back);
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Controls.Add((Control) this.btnUpdate);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(5, 589);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(874, 51);
      this.panel3.TabIndex = 22;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "Setup\\Data Tables";
      this.emHelpLink1.Location = new Point(13, 15);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 22;
      this.btn_back.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btn_back.DialogResult = DialogResult.Retry;
      this.btn_back.Location = new Point(635, 9);
      this.btn_back.Name = "btn_back";
      this.btn_back.Size = new Size(75, 22);
      this.btn_back.TabIndex = 8;
      this.btn_back.Text = "&Back";
      this.btn_back.Visible = false;
      this.downArrowButton.BackColor = Color.Transparent;
      this.downArrowButton.Location = new Point(101, 3);
      this.downArrowButton.MouseDownImage = (Image) null;
      this.downArrowButton.Name = "downArrowButton";
      this.downArrowButton.Size = new Size(16, 16);
      this.downArrowButton.StandardButtonType = StandardIconButton.ButtonType.DownArrowButton;
      this.downArrowButton.TabIndex = 1;
      this.downArrowButton.TabStop = false;
      this.toolTip.SetToolTip((Control) this.downArrowButton, "Move Down");
      this.downArrowButton.Click += new EventHandler(this.downArrowButton_Click);
      this.upArrowButton.BackColor = Color.Transparent;
      this.upArrowButton.Location = new Point(79, 3);
      this.upArrowButton.MouseDownImage = (Image) null;
      this.upArrowButton.Name = "upArrowButton";
      this.upArrowButton.Size = new Size(16, 16);
      this.upArrowButton.StandardButtonType = StandardIconButton.ButtonType.UpArrowButton;
      this.upArrowButton.TabIndex = 0;
      this.upArrowButton.TabStop = false;
      this.toolTip.SetToolTip((Control) this.upArrowButton, "Move Up");
      this.upArrowButton.Click += new EventHandler(this.upArrowButton_Click);
      this.panel1.Controls.Add((Control) this.label3);
      this.panel1.Controls.Add((Control) this.textBoxDDMTableDescription);
      this.panel1.Controls.Add((Control) this.textBoxDDMTableName);
      this.panel1.Controls.Add((Control) this.label2);
      this.panel1.Controls.Add((Control) this.label1);
      this.panel1.Location = new Point(10, 8);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(869, 68);
      this.panel1.TabIndex = 23;
      this.label3.AutoSize = true;
      this.label3.ForeColor = Color.Red;
      this.label3.Location = new Point(35, 9);
      this.label3.Name = "label3";
      this.label3.Size = new Size(11, 14);
      this.label3.TabIndex = 4;
      this.label3.Text = "*";
      this.textBoxDDMTableDescription.Location = new Point(72, 35);
      this.textBoxDDMTableDescription.MaxLength = 256;
      this.textBoxDDMTableDescription.Name = "textBoxDDMTableDescription";
      this.textBoxDDMTableDescription.Size = new Size(794, 20);
      this.textBoxDDMTableDescription.TabIndex = 3;
      this.textBoxDDMTableDescription.TextChanged += new EventHandler(this.Filter_TextChanged);
      this.textBoxDDMTableName.Location = new Point(72, 9);
      this.textBoxDDMTableName.MaxLength = 64;
      this.textBoxDDMTableName.Name = "textBoxDDMTableName";
      this.textBoxDDMTableName.Size = new Size(170, 20);
      this.textBoxDDMTableName.TabIndex = 2;
      this.textBoxDDMTableName.TextChanged += new EventHandler(this.Filter_TextChanged);
      this.textBoxDDMTableName.Leave += new EventHandler(this.FocusLeave_textBoxDDMTableName);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 35);
      this.label2.Name = "label2";
      this.label2.Size = new Size(61, 14);
      this.label2.TabIndex = 1;
      this.label2.Text = "Description";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(34, 14);
      this.label1.TabIndex = 0;
      this.label1.Text = "Name";
      this.panel4.Controls.Add((Control) this.grpSelected);
      this.panel4.Controls.Add((Control) this.addBtn);
      this.panel4.Controls.Add((Control) this.removeBtn);
      this.panel4.Dock = DockStyle.Fill;
      this.panel4.Location = new Point(353, 0);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(516, 502);
      this.panel4.TabIndex = 29;
      this.grpSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpSelected.Controls.Add((Control) this.tabSelectedFields);
      this.grpSelected.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpSelected.HeaderForeColor = SystemColors.ControlText;
      this.grpSelected.Location = new Point(83, 0);
      this.grpSelected.Name = "grpSelected";
      this.grpSelected.Size = new Size(432, 502);
      this.grpSelected.TabIndex = 30;
      this.grpSelected.Text = "Selected Fields";
      this.tabSelectedFields.Controls.Add((Control) this.tabPage1);
      this.tabSelectedFields.Dock = DockStyle.Fill;
      this.tabSelectedFields.Location = new Point(1, 26);
      this.tabSelectedFields.Name = "tabSelectedFields";
      this.tabSelectedFields.SelectedIndex = 0;
      this.tabSelectedFields.Size = new Size(430, 475);
      this.tabSelectedFields.TabIndex = 20;
      this.tabSelectedFields.SelectedIndexChanged += new EventHandler(this.tabSelectedFields_SelectedIndexChanged);
      this.tabPage1.Controls.Add((Control) this.listViewTarget);
      this.tabPage1.Location = new Point(4, 23);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new Padding(3);
      this.tabPage1.Size = new Size(422, 448);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Selected Fields";
      this.tabPage1.UseVisualStyleBackColor = true;
      this.listViewTarget.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 111;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pair";
      gvColumn2.Width = 50;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 185;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 80;
      this.listViewTarget.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.listViewTarget.Dock = DockStyle.Fill;
      this.listViewTarget.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTarget.Location = new Point(3, 3);
      this.listViewTarget.Name = "listViewTarget";
      this.listViewTarget.Size = new Size(416, 442);
      this.listViewTarget.TabIndex = 3;
      this.listViewTarget.ItemDoubleClick += new GVItemEventHandler(this.listViewTarget_ItemDoubleClick);
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.downArrowButton);
      this.flowLayoutPanel1.Controls.Add((Control) this.upArrowButton);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(311, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(120, 22);
      this.flowLayoutPanel1.TabIndex = 19;
      this.addBtn.Image = (Image) Resources.arrow_forward;
      this.addBtn.ImageAlign = ContentAlignment.TopCenter;
      this.addBtn.Location = new Point(5, 191);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 25);
      this.addBtn.TabIndex = 28;
      this.addBtn.Text = "Add";
      this.addBtn.TextAlign = ContentAlignment.MiddleRight;
      this.addBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.removeBtn.Image = (Image) Resources.arrow_back;
      this.removeBtn.Location = new Point(5, 222);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 25);
      this.removeBtn.TabIndex = 29;
      this.removeBtn.Text = "Remove";
      this.removeBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.panel2.Controls.Add((Control) this.panel4);
      this.panel2.Controls.Add((Control) this.collapsibleSplitter1);
      this.panel2.Controls.Add((Control) this.grpSource);
      this.panel2.Location = new Point(10, 82);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(869, 502);
      this.panel2.TabIndex = 24;
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpSource;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(346, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 28;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpSource.Controls.Add((Control) this.tabControlFields);
      this.grpSource.Dock = DockStyle.Left;
      this.grpSource.HeaderForeColor = SystemColors.ControlText;
      this.grpSource.Location = new Point(0, 0);
      this.grpSource.Name = "grpSource";
      this.grpSource.Padding = new Padding(2, 2, 1, 1);
      this.grpSource.Size = new Size(346, 502);
      this.grpSource.TabIndex = 24;
      this.grpSource.Text = "Source Fields";
      this.tabControlFields.Controls.Add((Control) this.tabFields);
      this.tabControlFields.Controls.Add((Control) this.tabLA);
      this.tabControlFields.Controls.Add((Control) this.tabMilestones);
      this.tabControlFields.Controls.Add((Control) this.tabDocuments);
      this.tabControlFields.Controls.Add((Control) this.tabTasks);
      this.tabControlFields.Controls.Add((Control) this.tabConditions);
      this.tabControlFields.Controls.Add((Control) this.tabPreliminary);
      this.tabControlFields.Controls.Add((Control) this.tabPost);
      this.tabControlFields.Controls.Add((Control) this.tabPageLock);
      this.tabControlFields.Controls.Add((Control) this.tabPageIS);
      this.tabControlFields.Controls.Add((Control) this.tabPageGFE);
      this.tabControlFields.Controls.Add((Control) this.tabPageTIL);
      this.tabControlFields.Controls.Add((Control) this.tabPageLECD);
      this.tabControlFields.Controls.Add((Control) this.tabPageEDisclosures);
      this.tabControlFields.Controls.Add((Control) this.tabPageAlerts);
      this.tabControlFields.Controls.Add((Control) this.tabPageAUSTracking);
      this.tabControlFields.Dock = DockStyle.Fill;
      this.tabControlFields.Location = new Point(3, 28);
      this.tabControlFields.Name = "tabControlFields";
      this.tabControlFields.SelectedIndex = 0;
      this.tabControlFields.Size = new Size(341, 472);
      this.tabControlFields.TabIndex = 0;
      this.tabControlFields.SelectedIndexChanged += new EventHandler(this.tabControlFields_SelectedIndexChanged);
      this.tabFields.Controls.Add((Control) this.listViewSource);
      this.tabFields.Controls.Add((Control) this.findBtn);
      this.tabFields.Controls.Add((Control) this.textBoxFind);
      this.tabFields.Location = new Point(4, 23);
      this.tabFields.Name = "tabFields";
      this.tabFields.Padding = new Padding(3);
      this.tabFields.Size = new Size(333, 445);
      this.tabFields.TabIndex = 0;
      this.tabFields.Text = "Fields";
      this.tabFields.UseVisualStyleBackColor = true;
      this.listViewSource.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.Text = "Field ID";
      gvColumn5.Width = 90;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column2";
      gvColumn6.Text = "Description";
      gvColumn6.Width = 166;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column3";
      gvColumn7.Text = "Type";
      gvColumn7.Width = 100;
      this.listViewSource.Columns.AddRange(new GVColumn[3]
      {
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.listViewSource.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewSource.Location = new Point(0, 32);
      this.listViewSource.Name = "listViewSource";
      this.listViewSource.Size = new Size(324, 377);
      this.listViewSource.TabIndex = 20;
      this.findBtn.Location = new Point(3, 5);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(64, 22);
      this.findBtn.TabIndex = 22;
      this.findBtn.Text = "Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.textBoxFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxFind.Location = new Point(71, 6);
      this.textBoxFind.Name = "textBoxFind";
      this.textBoxFind.Size = new Size(256, 20);
      this.textBoxFind.TabIndex = 21;
      this.textBoxFind.TextChanged += new EventHandler(this.textBoxFind_TextChanged);
      this.tabLA.Controls.Add((Control) this.listViewMember);
      this.tabLA.Location = new Point(4, 23);
      this.tabLA.Name = "tabLA";
      this.tabLA.Size = new Size(333, 445);
      this.tabLA.TabIndex = 1;
      this.tabLA.Text = "Team Members";
      this.tabLA.UseVisualStyleBackColor = true;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column1";
      gvColumn8.Text = "Field ID";
      gvColumn8.Width = 118;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column2";
      gvColumn9.Text = "Description";
      gvColumn9.Width = 137;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column3";
      gvColumn10.Text = "Type";
      gvColumn10.Width = 100;
      this.listViewMember.Columns.AddRange(new GVColumn[3]
      {
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.listViewMember.Dock = DockStyle.Fill;
      this.listViewMember.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewMember.Location = new Point(0, 0);
      this.listViewMember.Name = "listViewMember";
      this.listViewMember.Size = new Size(333, 445);
      this.listViewMember.TabIndex = 0;
      this.tabMilestones.Controls.Add((Control) this.listViewMilestone);
      this.tabMilestones.Location = new Point(4, 23);
      this.tabMilestones.Name = "tabMilestones";
      this.tabMilestones.Size = new Size(333, 445);
      this.tabMilestones.TabIndex = 2;
      this.tabMilestones.Text = "Milestones";
      this.tabMilestones.UseVisualStyleBackColor = true;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column1";
      gvColumn11.Text = "Field ID";
      gvColumn11.Width = 118;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column2";
      gvColumn12.Text = "Description";
      gvColumn12.Width = 137;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column3";
      gvColumn13.Text = "Type";
      gvColumn13.Width = 100;
      this.listViewMilestone.Columns.AddRange(new GVColumn[3]
      {
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.listViewMilestone.Dock = DockStyle.Fill;
      this.listViewMilestone.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewMilestone.Location = new Point(0, 0);
      this.listViewMilestone.Name = "listViewMilestone";
      this.listViewMilestone.Size = new Size(333, 445);
      this.listViewMilestone.TabIndex = 3;
      this.tabDocuments.Controls.Add((Control) this.listViewDocument);
      this.tabDocuments.Location = new Point(4, 23);
      this.tabDocuments.Name = "tabDocuments";
      this.tabDocuments.Size = new Size(333, 445);
      this.tabDocuments.TabIndex = 3;
      this.tabDocuments.Text = "Documents";
      this.tabDocuments.UseVisualStyleBackColor = true;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "Column1";
      gvColumn14.Text = "Field ID";
      gvColumn14.Width = 118;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "Column2";
      gvColumn15.Text = "Description";
      gvColumn15.Width = 137;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "Column3";
      gvColumn16.Text = "Type";
      gvColumn16.Width = 100;
      this.listViewDocument.Columns.AddRange(new GVColumn[3]
      {
        gvColumn14,
        gvColumn15,
        gvColumn16
      });
      this.listViewDocument.Dock = DockStyle.Fill;
      this.listViewDocument.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewDocument.Location = new Point(0, 0);
      this.listViewDocument.Name = "listViewDocument";
      this.listViewDocument.Size = new Size(333, 445);
      this.listViewDocument.TabIndex = 2;
      this.tabTasks.Controls.Add((Control) this.listViewTask);
      this.tabTasks.Location = new Point(4, 23);
      this.tabTasks.Name = "tabTasks";
      this.tabTasks.Size = new Size(333, 445);
      this.tabTasks.TabIndex = 4;
      this.tabTasks.Text = "Tasks";
      this.tabTasks.UseVisualStyleBackColor = true;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "Column1";
      gvColumn17.Text = "Field ID";
      gvColumn17.Width = 118;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "Column2";
      gvColumn18.Text = "Description";
      gvColumn18.Width = 137;
      gvColumn19.ImageIndex = -1;
      gvColumn19.Name = "Column3";
      gvColumn19.Text = "Type";
      gvColumn19.Width = 100;
      this.listViewTask.Columns.AddRange(new GVColumn[3]
      {
        gvColumn17,
        gvColumn18,
        gvColumn19
      });
      this.listViewTask.Dock = DockStyle.Fill;
      this.listViewTask.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTask.Location = new Point(0, 0);
      this.listViewTask.Name = "listViewTask";
      this.listViewTask.Size = new Size(333, 445);
      this.listViewTask.TabIndex = 3;
      this.tabConditions.Controls.Add((Control) this.listViewCondition);
      this.tabConditions.Location = new Point(4, 23);
      this.tabConditions.Name = "tabConditions";
      this.tabConditions.Size = new Size(333, 445);
      this.tabConditions.TabIndex = 5;
      this.tabConditions.Text = "Conditions";
      this.tabConditions.UseVisualStyleBackColor = true;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "Column1";
      gvColumn20.Text = "Field ID";
      gvColumn20.Width = 103;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "Column2";
      gvColumn21.Text = "Description";
      gvColumn21.Width = 152;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "Column3";
      gvColumn22.Text = "Type";
      gvColumn22.Width = 100;
      this.listViewCondition.Columns.AddRange(new GVColumn[3]
      {
        gvColumn20,
        gvColumn21,
        gvColumn22
      });
      this.listViewCondition.Dock = DockStyle.Fill;
      this.listViewCondition.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewCondition.Location = new Point(0, 0);
      this.listViewCondition.Name = "listViewCondition";
      this.listViewCondition.Size = new Size(333, 445);
      this.listViewCondition.TabIndex = 2;
      this.tabPreliminary.Controls.Add((Control) this.listViewPreliminary);
      this.tabPreliminary.Location = new Point(4, 23);
      this.tabPreliminary.Name = "tabPreliminary";
      this.tabPreliminary.Size = new Size(333, 445);
      this.tabPreliminary.TabIndex = 6;
      this.tabPreliminary.Text = "Preliminary Conditions";
      this.tabPreliminary.UseVisualStyleBackColor = true;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "Column1";
      gvColumn23.Text = "Field ID";
      gvColumn23.Width = 118;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "Column2";
      gvColumn24.Text = "Description";
      gvColumn24.Width = 137;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "Column3";
      gvColumn25.Text = "Type";
      gvColumn25.Width = 100;
      this.listViewPreliminary.Columns.AddRange(new GVColumn[3]
      {
        gvColumn23,
        gvColumn24,
        gvColumn25
      });
      this.listViewPreliminary.Dock = DockStyle.Fill;
      this.listViewPreliminary.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewPreliminary.Location = new Point(0, 0);
      this.listViewPreliminary.Name = "listViewPreliminary";
      this.listViewPreliminary.Size = new Size(333, 445);
      this.listViewPreliminary.TabIndex = 3;
      this.tabPost.Controls.Add((Control) this.listViewPost);
      this.tabPost.Location = new Point(4, 23);
      this.tabPost.Name = "tabPost";
      this.tabPost.Size = new Size(333, 445);
      this.tabPost.TabIndex = 7;
      this.tabPost.Text = "Post-Conditions";
      this.tabPost.UseVisualStyleBackColor = true;
      gvColumn26.ImageIndex = -1;
      gvColumn26.Name = "Column1";
      gvColumn26.Text = "Field ID";
      gvColumn26.Width = 118;
      gvColumn27.ImageIndex = -1;
      gvColumn27.Name = "Column2";
      gvColumn27.Text = "Description";
      gvColumn27.Width = 137;
      gvColumn28.ImageIndex = -1;
      gvColumn28.Name = "Column3";
      gvColumn28.Text = "Type";
      gvColumn28.Width = 100;
      this.listViewPost.Columns.AddRange(new GVColumn[3]
      {
        gvColumn26,
        gvColumn27,
        gvColumn28
      });
      this.listViewPost.Dock = DockStyle.Fill;
      this.listViewPost.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewPost.Location = new Point(0, 0);
      this.listViewPost.Name = "listViewPost";
      this.listViewPost.Size = new Size(333, 445);
      this.listViewPost.TabIndex = 2;
      this.tabPageLock.Controls.Add((Control) this.listViewLocks);
      this.tabPageLock.Location = new Point(4, 23);
      this.tabPageLock.Name = "tabPageLock";
      this.tabPageLock.Size = new Size(333, 445);
      this.tabPageLock.TabIndex = 8;
      this.tabPageLock.Text = "Locks";
      this.tabPageLock.UseVisualStyleBackColor = true;
      gvColumn29.ImageIndex = -1;
      gvColumn29.Name = "Column1";
      gvColumn29.Text = "Field ID";
      gvColumn29.Width = 118;
      gvColumn30.ImageIndex = -1;
      gvColumn30.Name = "Column2";
      gvColumn30.Text = "Description";
      gvColumn30.Width = 137;
      gvColumn31.ImageIndex = -1;
      gvColumn31.Name = "Column3";
      gvColumn31.Text = "Type";
      gvColumn31.Width = 100;
      this.listViewLocks.Columns.AddRange(new GVColumn[3]
      {
        gvColumn29,
        gvColumn30,
        gvColumn31
      });
      this.listViewLocks.Dock = DockStyle.Fill;
      this.listViewLocks.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewLocks.Location = new Point(0, 0);
      this.listViewLocks.Name = "listViewLocks";
      this.listViewLocks.Size = new Size(333, 445);
      this.listViewLocks.TabIndex = 3;
      this.tabPageIS.Controls.Add((Control) this.listViewIS);
      this.tabPageIS.Location = new Point(4, 23);
      this.tabPageIS.Name = "tabPageIS";
      this.tabPageIS.Size = new Size(333, 445);
      this.tabPageIS.TabIndex = 9;
      this.tabPageIS.Text = "Interim Servicing";
      this.tabPageIS.UseVisualStyleBackColor = true;
      gvColumn32.ImageIndex = -1;
      gvColumn32.Name = "Column1";
      gvColumn32.Text = "Field ID";
      gvColumn32.Width = 118;
      gvColumn33.ImageIndex = -1;
      gvColumn33.Name = "Column2";
      gvColumn33.Text = "Description";
      gvColumn33.Width = 137;
      gvColumn34.ImageIndex = -1;
      gvColumn34.Name = "Column3";
      gvColumn34.Text = "Type";
      gvColumn34.Width = 100;
      this.listViewIS.Columns.AddRange(new GVColumn[3]
      {
        gvColumn32,
        gvColumn33,
        gvColumn34
      });
      this.listViewIS.Dock = DockStyle.Fill;
      this.listViewIS.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewIS.Location = new Point(0, 0);
      this.listViewIS.Name = "listViewIS";
      this.listViewIS.Size = new Size(333, 445);
      this.listViewIS.TabIndex = 3;
      this.tabPageGFE.Controls.Add((Control) this.listViewGFE);
      this.tabPageGFE.Location = new Point(4, 23);
      this.tabPageGFE.Name = "tabPageGFE";
      this.tabPageGFE.Size = new Size(333, 445);
      this.tabPageGFE.TabIndex = 10;
      this.tabPageGFE.Text = "GFE Disclosure";
      this.tabPageGFE.UseVisualStyleBackColor = true;
      gvColumn35.ImageIndex = -1;
      gvColumn35.Name = "Column1";
      gvColumn35.Text = "Field ID";
      gvColumn35.Width = 118;
      gvColumn36.ImageIndex = -1;
      gvColumn36.Name = "Column2";
      gvColumn36.Text = "Description";
      gvColumn36.Width = 137;
      gvColumn37.ImageIndex = -1;
      gvColumn37.Name = "Column3";
      gvColumn37.Text = "Type";
      gvColumn37.Width = 100;
      this.listViewGFE.Columns.AddRange(new GVColumn[3]
      {
        gvColumn35,
        gvColumn36,
        gvColumn37
      });
      this.listViewGFE.Dock = DockStyle.Fill;
      this.listViewGFE.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewGFE.Location = new Point(0, 0);
      this.listViewGFE.Name = "listViewGFE";
      this.listViewGFE.Size = new Size(333, 445);
      this.listViewGFE.TabIndex = 4;
      this.tabPageTIL.Controls.Add((Control) this.listViewTIL);
      this.tabPageTIL.Location = new Point(4, 23);
      this.tabPageTIL.Name = "tabPageTIL";
      this.tabPageTIL.Size = new Size(333, 445);
      this.tabPageTIL.TabIndex = 11;
      this.tabPageTIL.Text = "TIL Disclosure";
      this.tabPageTIL.UseVisualStyleBackColor = true;
      gvColumn38.ImageIndex = -1;
      gvColumn38.Name = "Column1";
      gvColumn38.Text = "Field ID";
      gvColumn38.Width = 118;
      gvColumn39.ImageIndex = -1;
      gvColumn39.Name = "Column2";
      gvColumn39.Text = "Description";
      gvColumn39.Width = 137;
      gvColumn40.ImageIndex = -1;
      gvColumn40.Name = "Column3";
      gvColumn40.Text = "Type";
      gvColumn40.Width = 100;
      this.listViewTIL.Columns.AddRange(new GVColumn[3]
      {
        gvColumn38,
        gvColumn39,
        gvColumn40
      });
      this.listViewTIL.Dock = DockStyle.Fill;
      this.listViewTIL.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTIL.Location = new Point(0, 0);
      this.listViewTIL.Name = "listViewTIL";
      this.listViewTIL.Size = new Size(333, 445);
      this.listViewTIL.TabIndex = 4;
      this.tabPageLECD.Controls.Add((Control) this.listViewLECD);
      this.tabPageLECD.Location = new Point(4, 23);
      this.tabPageLECD.Name = "tabPageLECD";
      this.tabPageLECD.Size = new Size(333, 445);
      this.tabPageLECD.TabIndex = 12;
      this.tabPageLECD.Text = "LE and CD Disclosure";
      this.tabPageLECD.UseVisualStyleBackColor = true;
      gvColumn41.ImageIndex = -1;
      gvColumn41.Name = "Column1";
      gvColumn41.Text = "Field ID";
      gvColumn41.Width = 118;
      gvColumn42.ImageIndex = -1;
      gvColumn42.Name = "Column2";
      gvColumn42.Text = "Description";
      gvColumn42.Width = 137;
      gvColumn43.ImageIndex = -1;
      gvColumn43.Name = "Column3";
      gvColumn43.Text = "Type";
      gvColumn43.Width = 100;
      this.listViewLECD.Columns.AddRange(new GVColumn[3]
      {
        gvColumn41,
        gvColumn42,
        gvColumn43
      });
      this.listViewLECD.Dock = DockStyle.Fill;
      this.listViewLECD.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewLECD.Location = new Point(0, 0);
      this.listViewLECD.Name = "listViewLECD";
      this.listViewLECD.Size = new Size(333, 445);
      this.listViewLECD.TabIndex = 5;
      this.tabPageEDisclosures.Controls.Add((Control) this.listViewEDisclosures);
      this.tabPageEDisclosures.Location = new Point(4, 23);
      this.tabPageEDisclosures.Name = "tabPageEDisclosures";
      this.tabPageEDisclosures.Size = new Size(333, 445);
      this.tabPageEDisclosures.TabIndex = 13;
      this.tabPageEDisclosures.Text = "eDisclosures";
      this.tabPageEDisclosures.UseVisualStyleBackColor = true;
      gvColumn44.ImageIndex = -1;
      gvColumn44.Name = "Column1";
      gvColumn44.Text = "Field ID";
      gvColumn44.Width = 118;
      gvColumn45.ImageIndex = -1;
      gvColumn45.Name = "Column2";
      gvColumn45.Text = "Description";
      gvColumn45.Width = 137;
      gvColumn46.ImageIndex = -1;
      gvColumn46.Name = "Column3";
      gvColumn46.Text = "Type";
      gvColumn46.Width = 100;
      this.listViewEDisclosures.Columns.AddRange(new GVColumn[3]
      {
        gvColumn44,
        gvColumn45,
        gvColumn46
      });
      this.listViewEDisclosures.Dock = DockStyle.Fill;
      this.listViewEDisclosures.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewEDisclosures.Location = new Point(0, 0);
      this.listViewEDisclosures.Name = "listViewEDisclosures";
      this.listViewEDisclosures.Size = new Size(333, 445);
      this.listViewEDisclosures.TabIndex = 2;
      this.tabPageAlerts.Controls.Add((Control) this.listViewAlerts);
      this.tabPageAlerts.Location = new Point(4, 23);
      this.tabPageAlerts.Name = "tabPageAlerts";
      this.tabPageAlerts.Size = new Size(333, 445);
      this.tabPageAlerts.TabIndex = 14;
      this.tabPageAlerts.Text = "Alerts";
      this.tabPageAlerts.UseVisualStyleBackColor = true;
      gvColumn47.ImageIndex = -1;
      gvColumn47.Name = "Column1";
      gvColumn47.Text = "Field ID";
      gvColumn47.Width = 118;
      gvColumn48.ImageIndex = -1;
      gvColumn48.Name = "Column2";
      gvColumn48.Text = "Description";
      gvColumn48.Width = 137;
      gvColumn49.ImageIndex = -1;
      gvColumn49.Name = "Column3";
      gvColumn49.Text = "Type";
      gvColumn49.Width = 100;
      this.listViewAlerts.Columns.AddRange(new GVColumn[3]
      {
        gvColumn47,
        gvColumn48,
        gvColumn49
      });
      this.listViewAlerts.Dock = DockStyle.Fill;
      this.listViewAlerts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewAlerts.Location = new Point(0, 0);
      this.listViewAlerts.Name = "listViewAlerts";
      this.listViewAlerts.Size = new Size(333, 445);
      this.listViewAlerts.TabIndex = 3;
      this.tabPageAUSTracking.Controls.Add((Control) this.listViewAUSTracking);
      this.tabPageAUSTracking.Location = new Point(4, 23);
      this.tabPageAUSTracking.Name = "tabPageAUSTracking";
      this.tabPageAUSTracking.Size = new Size(333, 445);
      this.tabPageAUSTracking.TabIndex = 15;
      this.tabPageAUSTracking.Text = "AUS Tracking";
      this.tabPageAUSTracking.UseVisualStyleBackColor = true;
      gvColumn50.ImageIndex = -1;
      gvColumn50.Name = "Column1";
      gvColumn50.Text = "Field ID";
      gvColumn50.Width = 118;
      gvColumn51.ImageIndex = -1;
      gvColumn51.Name = "Column2";
      gvColumn51.Text = "Description";
      gvColumn51.Width = 137;
      gvColumn52.ImageIndex = -1;
      gvColumn52.Name = "Column3";
      gvColumn52.Text = "Type";
      gvColumn52.Width = 100;
      this.listViewAUSTracking.Columns.AddRange(new GVColumn[3]
      {
        gvColumn50,
        gvColumn51,
        gvColumn52
      });
      this.listViewAUSTracking.Dock = DockStyle.Fill;
      this.listViewAUSTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewAUSTracking.Location = new Point(0, 0);
      this.listViewAUSTracking.Name = "listViewAUSTracking";
      this.listViewAUSTracking.Size = new Size(333, 445);
      this.listViewAUSTracking.TabIndex = 4;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(884, 640);
      this.Controls.Add((Control) this.panel2);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(900, 679);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(900, 679);
      this.Name = nameof (DataTableDlg);
      this.Padding = new Padding(5, 5, 5, 0);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.SizeGripStyle = SizeGripStyle.Hide;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Add Data Table";
      this.Closing += new CancelEventHandler(this.DataTableDlg_Closing);
      this.Load += new EventHandler(this.DataTableDlg_Load);
      this.KeyDown += new KeyEventHandler(this.DataTableDlg_KeyDown);
      this.panel3.ResumeLayout(false);
      ((ISupportInitialize) this.downArrowButton).EndInit();
      ((ISupportInitialize) this.upArrowButton).EndInit();
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.grpSelected.ResumeLayout(false);
      this.tabSelectedFields.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.grpSource.ResumeLayout(false);
      this.tabControlFields.ResumeLayout(false);
      this.tabFields.ResumeLayout(false);
      this.tabFields.PerformLayout();
      this.tabLA.ResumeLayout(false);
      this.tabMilestones.ResumeLayout(false);
      this.tabDocuments.ResumeLayout(false);
      this.tabTasks.ResumeLayout(false);
      this.tabConditions.ResumeLayout(false);
      this.tabPreliminary.ResumeLayout(false);
      this.tabPost.ResumeLayout(false);
      this.tabPageLock.ResumeLayout(false);
      this.tabPageIS.ResumeLayout(false);
      this.tabPageGFE.ResumeLayout(false);
      this.tabPageTIL.ResumeLayout(false);
      this.tabPageLECD.ResumeLayout(false);
      this.tabPageEDisclosures.ResumeLayout(false);
      this.tabPageAlerts.ResumeLayout(false);
      this.tabPageAUSTracking.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void initializeSourceFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "initializeSourceFields: Loading source table...");
      this.listViewSource.Items.Clear();
      this.listViewSource.BeginUpdate();
      Dictionary<string, FieldDefinition> dictionary = new Dictionary<string, FieldDefinition>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      FieldDefinition[] fieldDefs = this._standardFields.AllFields.ToSortedList();
      for (int i = 0; i < fieldDefs.Length; ++i)
      {
        if (fieldDefs[i].AllowInReportingDatabase && fieldDefs[i].AppliesToEdition(Session.EncompassEdition) && !CustomFieldInfo.IsCustomFieldID(fieldDefs[i].FieldID))
        {
          if (fieldDefs[i].MultiInstance && fieldDefs[i].InstanceSpecifierType == FieldInstanceSpecifierType.Index)
          {
            for (int instanceSpecifier = 1; instanceSpecifier <= 10; ++instanceSpecifier)
            {
              FieldDefinition instance = fieldDefs[i].CreateInstance((object) instanceSpecifier);
              if (!dictionary.ContainsKey(instance.FieldID) && this.FieldsToRemove.All<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.FieldID != fieldDefs[i].FieldID)))
              {
                this.addSourceField(this.listViewSource, instance, false);
                dictionary[instance.FieldID] = instance;
              }
            }
          }
          else if (fieldDefs[i].Category != FieldCategory.Common)
          {
            fieldDefs[i].FieldID = FieldPairParser.ParseFieldPairInfo(fieldDefs[i].FieldID).FieldID;
            if (!dictionary.ContainsKey(fieldDefs[i].FieldID))
            {
              this.addSourceField(this.listViewSource, fieldDefs[i], false);
              dictionary[fieldDefs[i].FieldID] = fieldDefs[i];
            }
          }
          else if (!dictionary.ContainsKey(fieldDefs[i].FieldID) && this.FieldsToRemove.All<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.FieldID != fieldDefs[i].FieldID)))
          {
            this.addSourceField(this.listViewSource, fieldDefs[i], false);
            dictionary[fieldDefs[i].FieldID] = fieldDefs[i];
          }
        }
      }
      fieldDefs = this._standardFields.VirtualFields.ToSortedList();
      for (int index = 0; index < fieldDefs.Length; ++index)
      {
        string fieldId = fieldDefs[index].FieldID;
        if (!(fieldId.ToUpper() == "LOANLASTMODIFIED") && !(fieldId == "LOANFOLDER") && fieldDefs[index].AllowInReportingDatabase && !dictionary.ContainsKey(fieldDefs[index].FieldID) && fieldDefs[index].AppliesToEdition(Session.EncompassEdition))
        {
          this.addSourceField(this.listViewSource, fieldDefs[index], false);
          dictionary[fieldDefs[index].FieldID] = fieldDefs[index];
        }
      }
      this.loadCustomFields();
      foreach (FieldDefinition field in MilestoneTemplateFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewSource, field, false);
      }
      if (this.listViewSource.Items.Count > 0)
        this.listViewSource.Items[0].Selected = true;
      this.listViewSource.EndUpdate();
      this.loadTeamMembers();
      this.loadCustomMilestone();
      this.loadDocumentTracking();
      this.loadTasks();
      this.loadConditionTracking();
      this.loadPreliminaryCondition();
      this.loadPostConditionTracking();
      this.loadLocks();
      this.loadInterimServicingFields();
      this.loadGFEDisclosureFields();
      this.loadLECDDisclosureFields();
      this.loadTILDisclosureFields();
      this.loadEDisclosureFields();
      this.loadAlertFields();
      this.loadAUSTrackingFields();
      if (Session.EncompassEdition == EncompassEdition.Broker)
      {
        this.tabControlFields.TabPages.Remove(this.tabPageLock);
        this.tabControlFields.TabPages.Remove(this.tabPost);
        this.tabControlFields.TabPages.Remove(this.tabPageIS);
        this.tabControlFields.TabPages.Remove(this.tabConditions);
      }
      if (!Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "initializeSourceFields: Loading source table succeed");
    }

    private void loadTeamMembers()
    {
      this.listViewMember.BeginUpdate();
      this.listViewMember.Items.Clear();
      foreach (FieldDefinition field in LoanAssociateFields.All.Cast<FieldDefinition>().Where<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.AllowInReportingDatabase)))
        this.addSourceField(this.listViewMember, field, false);
      this.listViewMember.EndUpdate();
    }

    private void loadInterimServicingFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadInterimServicingFields: load Interim Servicing Fields setting");
      this.listViewIS.BeginUpdate();
      this.listViewIS.Items.Clear();
      foreach (FieldDefinition field in InterimServicingFields.All)
        this.addSourceField(this.listViewIS, field, false);
      this.listViewIS.EndUpdate();
    }

    private void loadEDisclosureFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadEDisclosureFields: load eDisclosure Fields setting");
      this.listViewEDisclosures.BeginUpdate();
      this.listViewEDisclosures.Items.Clear();
      foreach (FieldDefinition field in EDisclosureTrackingFields.All)
        this.addSourceField(this.listViewEDisclosures, field, false);
      foreach (FieldDefinition field in EDisclosure2015TrackingFields.All)
        this.addSourceField(this.listViewEDisclosures, field, false);
      this.listViewEDisclosures.EndUpdate();
    }

    private void loadGFEDisclosureFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadGFEDisclosureFields: load GFE Disclosure Fields setting");
      this.listViewGFE.BeginUpdate();
      this.listViewGFE.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedGFEFields.All)
        this.addSourceField(this.listViewGFE, field, false);
      this.listViewGFE.EndUpdate();
    }

    private void loadLECDDisclosureFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadLEDisclosureFields: load LE Disclosure Fields setting");
      this.listViewLECD.BeginUpdate();
      this.listViewLECD.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedLEFields.All)
        this.addSourceField(this.listViewLECD, field, false);
      foreach (FieldDefinition field in LastDisclosedCDFields.All)
        this.addSourceField(this.listViewLECD, field, false);
      this.listViewLECD.EndUpdate();
    }

    private void loadTILDisclosureFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadTILDisclosureFields: load TIL Disclosure Fields setting");
      this.listViewTIL.BeginUpdate();
      this.listViewTIL.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedTILFields.All)
        this.addSourceField(this.listViewTIL, field, false);
      this.listViewTIL.EndUpdate();
    }

    private void loadAlertFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadAlertFields: load Alert field setting");
      this.listViewAlerts.BeginUpdate();
      this.listViewAlerts.Items.Clear();
      foreach (FieldDefinition field in AlertFields.All)
        this.addSourceField(this.listViewAlerts, field, false);
      this.listViewAlerts.EndUpdate();
    }

    private void loadAUSTrackingFields()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadAUSTrackingFields: load AUS Tracking field setting");
      this.listViewAUSTracking.BeginUpdate();
      this.listViewAUSTracking.Items.Clear();
      foreach (FieldDefinition field in AUSTrackingFields.All)
        this.addSourceField(this.listViewAUSTracking, field, false);
      this.listViewAUSTracking.EndUpdate();
    }

    private void loadLocks()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadLocks: Loading lock buyside/sellside/request setting");
      this.listViewLocks.BeginUpdate();
      this.listViewLocks.Items.Clear();
      foreach (FieldDefinition lockRequestField in EncompassFields.GetAllLockRequestFields(this._fieldSettings))
        this.addSourceField(this.listViewLocks, lockRequestField, false);
      this.listViewLocks.EndUpdate();
      if (!Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadLocks: Loading lock buyside/sellside/request setting succeed");
    }

    private void loadCustomMilestone()
    {
      if (Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadCustomMilestone: Loading custom milestone setting");
      this.listViewMilestone.BeginUpdate();
      this.listViewMilestone.Items.Clear();
      foreach (FieldDefinition field in MilestoneFields.All)
      {
        if (!(field is CoreMilestoneField) && field.AllowInReportingDatabase)
          this.addSourceField(this.listViewMilestone, field, false);
      }
      foreach (FieldDefinition field in MilestoneTemplateFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewMilestone, field, false);
      }
      this.listViewMilestone.EndUpdate();
      if (!Tracing.IsSwitchActive(DataTableDlg.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(DataTableDlg.sw, TraceLevel.Verbose, nameof (DataTableDlg), "loadCustomMilestone: Loading custom milestone setting succeed");
    }

    private void loadDocumentTracking()
    {
      this.listViewDocument.BeginUpdate();
      this.listViewDocument.Items.Clear();
      foreach (FieldDefinition field in DocumentTrackingFields.All.Cast<FieldDefinition>().Where<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.AllowInReportingDatabase)))
        this.addSourceField(this.listViewDocument, field, false);
      this.listViewDocument.EndUpdate();
    }

    private void loadTasks()
    {
      this.listViewTask.BeginUpdate();
      this.listViewTask.Items.Clear();
      foreach (FieldDefinition field in MilestoneTaskFields.All.Cast<FieldDefinition>().Where<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.AllowInReportingDatabase)))
        this.addSourceField(this.listViewTask, field, false);
      this.listViewTask.EndUpdate();
    }

    private void loadPreliminaryCondition()
    {
      this.listViewPreliminary.BeginUpdate();
      this.listViewPreliminary.Items.Clear();
      foreach (FieldDefinition field in PreliminaryConditionFields.All.Cast<FieldDefinition>().Where<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.AllowInReportingDatabase)))
        this.addSourceField(this.listViewPreliminary, field, false);
      this.listViewPreliminary.EndUpdate();
    }

    private void loadConditionTracking()
    {
      this.listViewCondition.BeginUpdate();
      this.listViewCondition.Items.Clear();
      foreach (FieldDefinition field in UnderwritingConditionFields.All.Cast<FieldDefinition>().Where<FieldDefinition>((Func<FieldDefinition, bool>) (field => field.AllowInReportingDatabase)))
        this.addSourceField(this.listViewCondition, field, false);
      this.listViewCondition.EndUpdate();
    }

    private void loadPostConditionTracking()
    {
      this.listViewPost.BeginUpdate();
      this.listViewPost.Items.Clear();
      foreach (FieldDefinition field in PostClosingConditionFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewPost, field, false);
      }
      this.listViewPost.EndUpdate();
    }

    private void loadCustomFields()
    {
      if (this._fieldSettings == null)
        return;
      try
      {
        foreach (CustomFieldInfo customField in this._fieldSettings.CustomFields)
        {
          if (!customField.IsEmpty())
            this.addSourceField(this.listViewSource, (FieldDefinition) new CustomField(customField), false);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error in loading Custom Fields: " + (object) ex);
      }
    }

    private void addSourceField(
      GridView listViewObject,
      FieldDefinition field,
      bool selected,
      string csvColumn = null)
    {
      string fieldId = field.FieldID;
      DDMField ddmField = new DDMField(new LoanXDBField(field));
      FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
      GVItem gvItem;
      if (listViewObject.Name == "listViewTarget" && field.Category != FieldCategory.Common)
      {
        fieldId = fieldPairInfo.FieldID;
        ddmField.FieldId = fieldId;
        ddmField.ComortgagorPair = fieldPairInfo.PairIndex;
        if (ddmField.ComortgagorPair == 0)
          ddmField.ComortgagorPair = 1;
        if (csvColumn != null)
        {
          gvItem = new GVItem(csvColumn);
          gvItem.SubItems.Add((object) ddmField);
        }
        else
          gvItem = new GVItem((object) ddmField);
      }
      else
      {
        gvItem = new GVItem(csvColumn ?? fieldId);
        if (!string.IsNullOrEmpty(csvColumn))
          gvItem.SubItems.Add((object) fieldId);
      }
      int nItemIndex = string.IsNullOrEmpty(csvColumn) ? 0 : 1;
      gvItem.SubItems[nItemIndex].SortValue = !Utils.IsInt((object) fieldId) ? (object) ("B" + fieldId) : (object) ("A" + Utils.ParseInt((object) fieldId).ToString("000000"));
      if (listViewObject.Name == "listViewTarget")
      {
        if (field.Category != FieldCategory.Common)
          gvItem.SubItems.Add(ddmField.ComortgagorPair > 0 ? (object) ddmField.PairText : (object) string.Empty);
        else
          gvItem.SubItems.Add((object) string.Empty);
      }
      gvItem.SubItems.Add((object) field.Description);
      string str = "String";
      if (field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Date || field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.DateTime)
        str = "Date";
      else if (field.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Numeric)
        str = "Numeric";
      gvItem.SubItems.Add((object) str);
      gvItem.Tag = (object) field;
      if (selected)
        gvItem.Selected = true;
      listViewObject.Items.Add(gvItem);
      if (!selected)
        return;
      listViewObject.EnsureVisible(gvItem.Index);
    }

    private void updateOutputColumnDictionary(GVItemCollection newOutputColumns)
    {
      string empty = string.Empty;
      foreach (GVItem newOutputColumn in (IEnumerable<GVItem>) newOutputColumns)
      {
        if (!(newOutputColumn.SubItems[1].Text == newOutputColumn.SubItems[2].Text))
        {
          this._outputColDataTableData[newOutputColumn.SubItems[1].Text.ToUpperInvariant()] = this._outputColDataTableData[newOutputColumn.SubItems[2].Text];
          this._outputColDataTableData.Remove(newOutputColumn.SubItems[2].Text);
        }
      }
    }

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      if (this.Text == "Modify Data Table" && this.originalTableNameValue != this.textBoxDDMTableName.Text && this.userResponseOnNameChange == DialogResult.None)
      {
        int num1 = (int) this.PromptUserWithDataTableReference();
      }
      this.userResponseOnUpdate = DialogResult.Yes;
      if (this.originalTableNameValue != this.textBoxDDMTableName.Text && ((DDMDataTableBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMDataTables)).DDMDataTableExists(this.textBoxDDMTableName.Text, true) && !this._overwriteExisting)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "This table name already exists in the database , please choose a different name", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.textBoxDDMTableName.Focus();
      }
      else
      {
        this.ddmDataTableInfo.Name = this.textBoxDDMTableName.Text;
        this.ddmDataTableInfo.Description = this.textBoxDDMTableDescription.Text;
        if (!this.BackButtonPressed)
          this.outputColumns = !this._importMode ? (!(this.Text == "Modify Data Table") ? new DataTableOutputColumnSetupDialog() : new DataTableOutputColumnSetupDialog(this.ddmDataTableInfo)) : new DataTableOutputColumnSetupDialog(this.ddmDataTableInfo, this._outputColDataTableData);
        switch (this.outputColumns.ShowDialog())
        {
          case DialogResult.OK:
            if (this._importMode)
              this.updateOutputColumnDictionary(this.outputColumns.OutputGrid.Items);
            this.ddmDataTableInfo.Fields = new DDMDataTableFieldInfo[this.listViewTarget.Items.Count + this.outputColumns.OutputGrid.Items.Count];
            this.BackButtonPressed = false;
            if (this._importMode)
            {
              int index1 = 0;
              for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
              {
                object tag = this.listViewTarget.Items[nItemIndex].Tag;
                if (tag is string)
                {
                  int num3 = (int) MessageBox.Show((IWin32Window) this, "Please assign valid fields to unassigned fields before updating", "Unassigned Fields", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                  return;
                }
                string empty = string.Empty;
                string fieldId = !(tag is FieldDefinition) ? ((LoanXDBField) tag).FieldID : ((FieldDefinition) tag).FieldID;
                if (!string.IsNullOrEmpty(fieldId))
                {
                  FieldDefinition fieldDefinition = EncompassFields.GetField(fieldId, this._fieldSettings);
                  if (fieldDefinition == null && this._standardFields.VirtualFields.Contains(fieldId))
                    fieldDefinition = this._standardFields.VirtualFields[fieldId];
                  else if (fieldDefinition != null && fieldDefinition.Category != FieldCategory.Common && this.listViewTarget.Items[nItemIndex].SubItems[1].Value is DDMField ddmField)
                    fieldDefinition.FieldID = ddmField.FieldIdWithPair;
                  this.ddmDataTableInfo.Fields[index1] = new DDMDataTableFieldInfo(fieldDefinition);
                  ++index1;
                }
              }
              for (int index2 = 0; index2 < this.outputColumns.OutputGrid.Items.Count; ++index2)
              {
                string empty = string.Empty;
                string str = !(this.outputColumns.OutputGrid.Items[index2].SubItems[1].Text == string.Empty) ? this.outputColumns.OutputGrid.Items[index2].SubItems[1].Text : this.outputColumns.OutputGrid.Items[index2].SubItems[0].Text;
                this.ddmDataTableInfo.Fields[index1] = new DDMDataTableFieldInfo(str.ToUpper(), outputIdx: index2);
                ++index1;
              }
            }
            else
            {
              int num4 = 0;
              for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
              {
                string fieldId = this.listViewTarget.Items[nItemIndex].SubItems[0].Value is DDMField ddmField ? ddmField.FieldIdWithPair : this.listViewTarget.Items[nItemIndex].Text;
                if (!string.IsNullOrEmpty(fieldId))
                {
                  FieldDefinition fieldDefinition = EncompassFields.GetField(fieldId, this._fieldSettings);
                  if (fieldDefinition == null && this._standardFields.VirtualFields.Contains(fieldId))
                    fieldDefinition = this._standardFields.VirtualFields[fieldId];
                  else if (fieldDefinition != null && fieldDefinition.Category == FieldCategory.Common)
                    fieldId = this.listViewTarget.Items[nItemIndex].Text;
                  this.ddmDataTableInfo.Fields[num4++] = new DDMDataTableFieldInfo(fieldDefinition)
                  {
                    FieldId = fieldId
                  };
                }
              }
              List<GVItem> list = this.outputColumns.OutputGrid.Items.OrderBy<GVItem, string>((Func<GVItem, string>) (item => item.SubItems[0].Text)).ToList<GVItem>();
              for (int index = 0; index < list.Count; ++index)
              {
                string text = list[index].SubItems[1].Text;
                this.ddmDataTableInfo.Fields[num4++] = new DDMDataTableFieldInfo(text, outputIdx: index);
              }
            }
            if (this.Text != "Modify Data Table")
            {
              this.Hide();
              if (this._importMode)
              {
                DataTableImportRawData tableImportRawData = new DataTableImportRawData(this.ddmDataTableInfo, this._importRowCount, this._dataTableData, this._outputColDataTableData, this._standardFields, this._fieldSettings);
                ImportProcessResult importResult = new ImportProcessor((IImportErrorProvider) new DataTableImportErrorProvider()).Process((IRawImportData) tableImportRawData);
                if (importResult.Result == ImportResult.Error)
                {
                  using (ImportErrorForm importErrorForm = new ImportErrorForm("Data Table Import Errors", "The data table cannot be imported because the row(s) have invalid data. Please refer to the Error column to correct data entries.", new ImportErrorDataProvider((IRawImportData) tableImportRawData, importResult).GetErrorDataForReport(), this._standardFields, this._fieldSettings, ImportType.DataTable))
                  {
                    int num5 = (int) importErrorForm.ShowDialog();
                  }
                  this.Close();
                  break;
                }
                DDMDataTableBpmManager bpmManager = (DDMDataTableBpmManager) Session.BPM.GetBpmManager(BpmCategory.DDMDataTables);
                DDMDataTable ddmDataTable;
                if (this._overwriteExisting)
                {
                  ddmDataTable = bpmManager.GetDDMDataTableAndFieldValuesByName(this.textBoxDDMTableName.Text, true);
                  ddmDataTable.Name = this.textBoxDDMTableName.Text;
                  ddmDataTable.Description = this.textBoxDDMTableDescription.Text;
                  ddmDataTable.LastModDt = DateTime.Now.ToString();
                  ddmDataTable.LastModByUserID = Session.DefaultInstance.UserID;
                  ddmDataTable.LastModByFullName = Session.DefaultInstance.UserInfo.FullName;
                  ddmDataTable.FieldIdList = this.ddmDataTableInfo.GetFieldIdListString();
                  ddmDataTable.OutputIdList = this.ddmDataTableInfo.GetOutputIdListString();
                }
                else
                  ddmDataTable = new DDMDataTable(-1, this.textBoxDDMTableName.Text, this.textBoxDDMTableDescription.Text, DateTime.Now.ToString(), Session.DefaultInstance.UserID, Session.DefaultInstance.UserInfo.FullName, "", this.ddmDataTableInfo.GetFieldIdListString(), this.ddmDataTableInfo.GetOutputIdListString());
                ddmDataTable.FieldValues = new Dictionary<int, List<DDMDataTableFieldValue>>();
                DataTableValueBuilder tableValueBuilder = new DataTableValueBuilder(tableImportRawData);
                for (int index = 0; index < tableImportRawData.RowCount(); ++index)
                {
                  List<DDMDataTableFieldValue> fieldValues = tableValueBuilder.GetFieldValues(index);
                  ddmDataTable.FieldValues.Add(index, fieldValues);
                }
                ddmDataTable.Id = bpmManager.UpsertDDMDataTable(ddmDataTable, this._importMode, true);
                using (DataTableSetupDlg dataTableSetupDlg = new DataTableSetupDlg(ddmDataTable.Id, Session.DefaultInstance))
                {
                  int num6 = (int) dataTableSetupDlg.ShowDialog();
                }
                this.ImportedDataTable = ddmDataTable;
                this.Close();
                break;
              }
              using (DataTableSetupDlg dataTableSetupDlg = new DataTableSetupDlg(this.ddmDataTableInfo, Session.DefaultInstance))
              {
                if (this.Text == "Add Data Table")
                  dataTableSetupDlg.saveAllChangeAfterCheckStatus();
                int num7 = (int) dataTableSetupDlg.ShowDialog();
              }
              this.Close();
              break;
            }
            if (this.wereExistingFieldsRemoved(this.ddmDataTableInfo))
            {
              DialogResult dialogResult = Utils.Dialog((IWin32Window) this, "Removing existing columns would result in losing the data values defined for that column. Data rows rendered empty due to this change may be removed from Data table. Do you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
              this.userResponseOnUpdate = dialogResult;
              if (!dialogResult.Equals((object) DialogResult.Yes))
                break;
              this.Close();
              break;
            }
            this.Close();
            break;
          case DialogResult.Retry:
            this.BackButtonPressed = true;
            break;
          default:
            this.BackButtonPressed = false;
            break;
        }
      }
    }

    private bool wereExistingFieldsRemoved(DDMDataTableInfo newDdmTableInfo)
    {
      if (this.originalDataTableInfo != null && this.originalDataTableInfo.Fields != null && newDdmTableInfo != null && newDdmTableInfo.Fields != null)
      {
        if (newDdmTableInfo.GetNonOutputFieldCount() < this.originalDataTableInfo.GetNonOutputFieldCount() || newDdmTableInfo.GetOutputFieldCount() < this.originalDataTableInfo.GetOutputFieldCount())
          return true;
        HashSet<string> stringSet = new HashSet<string>();
        foreach (DDMDataTableFieldInfo field in newDdmTableInfo.Fields)
        {
          if (!field.IsOutput)
            stringSet.Add(field.FieldId);
        }
        foreach (DDMDataTableFieldInfo field in this.originalDataTableInfo.Fields)
        {
          if (!field.IsOutput && !stringSet.Contains(field.FieldId))
            return true;
        }
      }
      return false;
    }

    private Func<string, bool> checkExistingFieldsRemovalStat(DDMDataTableInfo newDdmTableInfo)
    {
      int? removedCount = this.countExistingFieldsRemoved(newDdmTableInfo);
      return (Func<string, bool>) (checkMode =>
      {
        if (!removedCount.HasValue)
          return false;
        switch (checkMode)
        {
          case "ALL":
            int length = this.originalDataTableInfo.Fields.Length;
            int? nullable1 = removedCount;
            int valueOrDefault = nullable1.GetValueOrDefault();
            return length == valueOrDefault & nullable1.HasValue;
          default:
            int? nullable2 = removedCount;
            int num = 0;
            return nullable2.GetValueOrDefault() > num & nullable2.HasValue;
        }
      });
    }

    private int? countExistingFieldsRemoved(DDMDataTableInfo newDdmTableInfo)
    {
      int? nullable1 = new int?();
      if (this.originalDataTableInfo != null && this.originalDataTableInfo.Fields != null && newDdmTableInfo != null && newDdmTableInfo.Fields != null)
      {
        nullable1 = new int?(0);
        foreach (DDMDataTableFieldInfo field1 in this.originalDataTableInfo.Fields)
        {
          DDMDataTableFieldInfo field = field1;
          if (!((IEnumerable<DDMDataTableFieldInfo>) newDdmTableInfo.Fields).Any<DDMDataTableFieldInfo>((Func<DDMDataTableFieldInfo, bool>) (innerField => innerField.FieldId == field.FieldId)))
          {
            int? nullable2 = nullable1;
            nullable1 = nullable2.HasValue ? new int?(nullable2.GetValueOrDefault() + 1) : new int?();
          }
        }
      }
      return nullable1;
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      if (!this.addBtn.Enabled)
        return;
      if (this._importMode && this.listViewTarget.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please select a Field and a Data Table column to replace.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.tabControlFields.SelectedTab == this.tabFields)
        {
          if (this._importMode && this.listViewSource.SelectedItems.Count > 1)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "Updating multiple fields at a time is not supported during import. Please select one field at a time.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
          this.addFieldsToTargetList(this.listViewSource);
        }
        else if (this.tabControlFields.SelectedTab == this.tabDocuments)
          this.addFieldsToTargetList(this.listViewDocument);
        else if (this.tabControlFields.SelectedTab == this.tabTasks)
          this.addFieldsToTargetList(this.listViewTask);
        else if (this.tabControlFields.SelectedTab == this.tabConditions)
          this.addFieldsToTargetList(this.listViewCondition);
        else if (this.tabControlFields.SelectedTab == this.tabLA)
          this.addFieldsToTargetList(this.listViewMember);
        else if (this.tabControlFields.SelectedTab == this.tabMilestones)
          this.addFieldsToTargetList(this.listViewMilestone);
        else if (this.tabControlFields.SelectedTab == this.tabPost)
          this.addFieldsToTargetList(this.listViewPost);
        else if (this.tabControlFields.SelectedTab == this.tabPageLock)
          this.addFieldsToTargetList(this.listViewLocks);
        else if (this.tabControlFields.SelectedTab == this.tabPageIS)
          this.addFieldsToTargetList(this.listViewIS);
        else if (this.tabControlFields.SelectedTab == this.tabPageGFE)
          this.addFieldsToTargetList(this.listViewGFE);
        else if (this.tabControlFields.SelectedTab == this.tabPageTIL)
          this.addFieldsToTargetList(this.listViewTIL);
        else if (this.tabControlFields.SelectedTab == this.tabPageLECD)
          this.addFieldsToTargetList(this.listViewLECD);
        else if (this.tabControlFields.SelectedTab == this.tabPageAlerts)
          this.addFieldsToTargetList(this.listViewAlerts);
        else if (this.tabControlFields.SelectedTab == this.tabPreliminary)
          this.addFieldsToTargetList(this.listViewPreliminary);
        else if (this.tabControlFields.SelectedTab == this.tabPageAUSTracking)
          this.addFieldsToTargetList(this.listViewAUSTracking);
        else if (this.tabControlFields.SelectedTab == this.tabPageEDisclosures)
          this.addFieldsToTargetList(this.listViewEDisclosures);
        this.enableUpdate();
      }
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewTarget.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.Cursor = Cursors.WaitCursor;
        if (this.listViewSource.SelectedItems.Count > 0)
          this.listViewSource.SelectedItems.Clear();
        ArrayList arrayList = new ArrayList();
        foreach (GVItem selectedItem in this.listViewTarget.SelectedItems)
          arrayList.Add((object) selectedItem);
        foreach (GVItem gvItem in arrayList)
          this.listViewTarget.Items.Remove(gvItem);
        foreach (GVItem gvItem in arrayList)
        {
          string text = gvItem.Text;
          FieldDefinition field = EncompassFields.GetField(text, this._fieldSettings);
          if (field == null && this._standardFields.VirtualFields.Contains(text))
            field = this._standardFields.VirtualFields[text];
          if (!text.ToLower().StartsWith("disclosedtil") && !text.ToLower().StartsWith("austracking.") && field != null && field.Category == FieldCategory.Common)
            this.addSourceField(this.listViewSource, field, true);
        }
        this.updateFieldCounter();
        this.Cursor = Cursors.Default;
        this.enableUpdate();
      }
    }

    private GVItem createTargetFieldListItem(LoanXDBField dbField)
    {
      string fieldId = dbField.FieldID;
      string description = dbField.Description;
      DDMField ddmField = new DDMField(dbField);
      FieldDefinition fieldDefinition = EncompassFields.GetField(dbField.FieldID, this._fieldSettings);
      if (fieldDefinition != null)
        fieldId = fieldDefinition.FieldID;
      else if (this._standardFields.VirtualFields.Contains(dbField.FieldID))
      {
        fieldDefinition = this._standardFields.VirtualFields[dbField.FieldID];
        description = fieldDefinition.Description;
      }
      GVItem lvwItem = new GVItem((object) ddmField);
      lvwItem.SubItems[0].SortValue = !Utils.IsInt((object) fieldId) ? (object) ("B" + fieldId) : (object) ("A" + Utils.ParseInt((object) fieldId).ToString("000000"));
      if (fieldDefinition != null && fieldDefinition.Category != FieldCategory.Common)
      {
        using (DDMFieldDialog ddmFieldDialog = new DDMFieldDialog(ddmField))
        {
          if (ddmFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return (GVItem) null;
          lvwItem.SubItems.Add((object) ddmFieldDialog.DdmField.PairText);
          dbField.ComortgagorPair = ddmFieldDialog.DdmField.ComortgagorPair;
          ddmField = new DDMField(dbField);
          lvwItem.SubItems[0].Value = (object) ddmField;
          if (this.listViewTarget.Items.Select(item => new
          {
            item = item,
            field = item.SubItems[0].Value as DDMField
          }).Select(_param1 => new
          {
            \u003C\u003Eh__TransparentIdentifier0 = _param1,
            ddmfield = lvwItem.SubItems[0].Value as DDMField
          }).Where(_param1 => ddmField != null).Where(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.field != null && _param1.ddmfield.FieldIdWithPair == _param1.\u003C\u003Eh__TransparentIdentifier0.field.FieldIdWithPair).Select(_param1 => _param1.\u003C\u003Eh__TransparentIdentifier0.item).Any<GVItem>())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This field and borrower pair index have already been added", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return (GVItem) null;
          }
        }
      }
      else
        lvwItem.SubItems.Add((object) "");
      lvwItem.SubItems.Add((object) description);
      switch (dbField.FieldType)
      {
        case LoanXDBTableList.TableTypes.IsNumeric:
          lvwItem.SubItems.Add((object) "Numeric");
          break;
        case LoanXDBTableList.TableTypes.IsDate:
          lvwItem.SubItems.Add((object) "Date");
          break;
        default:
          lvwItem.SubItems.Add((object) "String");
          break;
      }
      lvwItem.Tag = (object) dbField;
      return lvwItem;
    }

    private bool hasDuplicateField(LoanXDBField dbField)
    {
      for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
      {
        if (!(this.listViewTarget.Items[nItemIndex].Tag is LoanXDBField loanXdbField))
          loanXdbField = new LoanXDBField((FieldDefinition) this.listViewTarget.Items[nItemIndex].Tag);
        if (loanXdbField.FieldID == dbField.FieldID && loanXdbField.ComortgagorPair == dbField.ComortgagorPair)
          return true;
      }
      return false;
    }

    public void addFieldsToTargetList(
      GridView sourceListView,
      string fieldInstanceSpecifier,
      bool clearTargetSelect,
      bool isSortTargetList)
    {
      if (sourceListView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a source field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        bool pointToAdd = false;
        int insertAt = 0;
        if (clearTargetSelect && this.listViewTarget.SelectedItems.Count > 0)
        {
          insertAt = this.listViewTarget.SelectedItems[this.listViewTarget.SelectedItems.Count - 1].Index;
          pointToAdd = true;
          this.listViewTarget.SelectedItems.Clear();
        }
        FieldInstanceSpecifierType specifierType = FieldInstanceSpecifierType.None;
        bool flag = false;
        ArrayList selectList = new ArrayList();
        foreach (GVItem selectedItem in sourceListView.SelectedItems)
        {
          FieldDefinition tag = (FieldDefinition) selectedItem.Tag;
          if (tag.FieldID.ToLower() == "log.ms.duration")
            flag = true;
          selectList.Add((object) selectedItem);
          if (tag.InstanceSpecifierType != FieldInstanceSpecifierType.None)
            specifierType = tag.InstanceSpecifierType;
        }
        if (this.MaxFieldsExceeded(this.listViewTarget, (ICollection) selectList))
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, string.Format("The Data table can have a maximum of {0} fields ", (object) 40), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          string instanceSpecifier = fieldInstanceSpecifier;
          if (string.IsNullOrEmpty(instanceSpecifier) && specifierType != FieldInstanceSpecifierType.None)
          {
            string selectedFieldID = flag ? "log.ms.duration" : "";
            using (InstanceSelectorDialog instanceSelectorDialog = new InstanceSelectorDialog(specifierType, selectedFieldID))
            {
              if (instanceSelectorDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              if (instanceSelectorDialog.SelectedInstance != "")
                instanceSpecifier = instanceSelectorDialog.SelectedInstance;
            }
          }
          this.Cursor = Cursors.WaitCursor;
          GVItem gvItem1 = (GVItem) null;
          GVItem gvItem2 = (GVItem) null;
          List<GVItem> gvItemList = new List<GVItem>();
          foreach (GVItem sourceItem in selectList)
          {
            FieldDefinition fieldDef = (FieldDefinition) sourceItem.Tag;
            if (instanceSpecifier != null && fieldDef.InstanceSpecifierType != FieldInstanceSpecifierType.None)
              fieldDef = fieldDef.CreateInstance((object) instanceSpecifier);
            LoanXDBField loanXdbField = new LoanXDBField(fieldDef);
            if (loanXdbField.FieldID == "663")
              loanXdbField.FieldSize = 15;
            GVItem gvItem3;
            if (!this._importMode)
            {
              if (this.tabControlFields.SelectedTab != this.tabPageIS && this.tabControlFields.SelectedTab != this.tabPageLock && this.tabControlFields.SelectedTab != this.tabFields && this.hasDuplicateField(loanXdbField))
              {
                int num3 = (int) Utils.Dialog((IWin32Window) this, "The selected field list already contains field '" + loanXdbField.FieldID + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                continue;
              }
              gvItem3 = this.createTargetFieldListItem(loanXdbField);
              if (gvItem3 == null)
              {
                this.Cursor = Cursors.Default;
                return;
              }
              if (gvItem1 == null)
                gvItem1 = gvItem3;
              if (!pointToAdd)
              {
                this.listViewTarget.Items.Add(gvItem3);
              }
              else
              {
                if (insertAt + 1 > this.listViewTarget.Items.Count)
                  this.listViewTarget.Items.Add(gvItem3);
                else
                  this.listViewTarget.Items.Insert(insertAt + 1, gvItem3);
                ++insertAt;
              }
            }
            else
            {
              gvItem3 = this._importHandler.AddFieldToDestinationList(sourceItem, loanXdbField, pointToAdd, insertAt);
              if (gvItem3 != null)
                gvItemList.Add(gvItem3);
            }
            if (gvItem3 != null)
              gvItem3.Selected = true;
          }
          if (this.tabControlFields.SelectedTab == this.tabFields)
          {
            gvItem2 = (GVItem) null;
            foreach (GVItem gvItem4 in selectList)
            {
              FieldDefinition field = EncompassFields.GetField(gvItem4.Text, this._fieldSettings);
              if (gvItem4.Text.ToLower().StartsWith("cx.") || gvItem4.Text.ToLower() == "loanfolder" || gvItem4.Text.ToLower() == "loanlastmodified" || field != null && field.Category == FieldCategory.Common)
                this.listViewSource.Items.Remove(gvItem4);
            }
          }
          else if (this.tabControlFields.SelectedTab == this.tabPageEDisclosures)
          {
            gvItem2 = (GVItem) null;
            foreach (GVItem gvItem5 in selectList)
            {
              FieldDefinition field = EncompassFields.GetField(gvItem5.Text, this._fieldSettings);
              if (string.Compare(field.FieldID, "EDISCLOSEDTRK.DisclosureCount", true) == 0)
                this.listViewEDisclosures.Items.Remove(gvItem5);
              if (string.Compare(field.FieldID, "EDISCLOSED2015TRK.DisclosureCount", true) == 0)
                this.listViewEDisclosures.Items.Remove(gvItem5);
            }
          }
          else if (this.tabControlFields.SelectedTab == this.tabPageLock || this.tabControlFields.SelectedTab == this.tabPageIS)
          {
            GridView gridView = this.tabControlFields.SelectedTab != this.tabPageLock ? (this.tabControlFields.SelectedTab != this.tabPageIS ? this.listViewAUSTracking : this.listViewIS) : this.listViewLocks;
            gvItem2 = (GVItem) null;
            foreach (GVItem gvItem6 in selectList)
            {
              FieldDefinition fieldDefinition = EncompassFields.GetField(gvItem6.Text, this._fieldSettings);
              if (fieldDefinition == null && this._standardFields.VirtualFields.Contains(gvItem6.Text))
                fieldDefinition = this._standardFields.VirtualFields[gvItem6.Text];
              if (fieldDefinition != null)
                gridView.Items.Remove(gvItem6);
            }
          }
          if (this._importMode)
          {
            foreach (GVItem gvItem7 in selectList)
            {
              if (!gvItemList.Contains(gvItem7))
              {
                FieldDefinition field = EncompassFields.GetField(gvItem7.Text, this._fieldSettings);
                if (gvItem7.Text.ToLower().StartsWith("cx.") || gvItem7.Text.ToLower() == "loanfolder" || gvItem7.Text.ToLower() == "loanlastmodified" || field != null && field.Category == FieldCategory.Common)
                  this.listViewSource.Items.Remove(gvItem7);
              }
            }
          }
          if (isSortTargetList)
            this.listViewTarget.ReSort();
          this.updateFieldCounter();
          if (gvItem1 != null)
            this.listViewTarget.EnsureVisible(gvItem1.Index);
          this.Cursor = Cursors.Default;
        }
      }
    }

    private void addFieldsToTargetList(GridView sourceListView)
    {
      this.addFieldsToTargetList(sourceListView, (string) null, true, false);
    }

    private bool findField(GridView list)
    {
      string lower = this.textBoxFind.Text.Trim().ToLower();
      if (lower == "" || list.Items.Count == 0)
        return false;
      bool flag = false;
      string str;
      if (lower.Substring(0, 1) == "*")
      {
        flag = true;
        str = lower.Substring(1);
      }
      else
        str = " " + lower + " ";
      int num = -1;
      if (list.SelectedItems.Count > 0)
        num = list.SelectedItems[0].Index;
      list.SelectedItems.Clear();
      bool field = false;
      string empty = string.Empty;
      for (int index = 0; index < list.Items.Count; ++index)
      {
        ++num;
        if (num > list.Items.Count - 1)
          num = 0;
        for (int nItemIndex = 0; nItemIndex < list.Items[num].SubItems.Count; ++nItemIndex)
        {
          if (flag)
          {
            if (list.Items[num].SubItems[nItemIndex].Text.ToLower().IndexOf(str) > -1)
            {
              field = true;
              break;
            }
          }
          else if ((" " + list.Items[num].SubItems[nItemIndex].Text.ToLower() + " ").IndexOf(str) > -1)
          {
            field = true;
            break;
          }
        }
        if (field)
        {
          list.Items[num].Selected = true;
          list.EnsureVisible(num);
          break;
        }
      }
      return field;
    }

    private void textBoxFind_TextChanged(object sender, EventArgs e)
    {
      if (this.textBoxFind.Text.Trim() == string.Empty)
        this.findBtn.Text = "Find";
      this.findField(this.listViewSource);
      this.findField(this.listViewTarget);
      if (this.textBoxFind.Text.Trim() != string.Empty)
        this.findBtn.Text = "Find Next";
      this.textBoxFind.SelectionStart = this.textBoxFind.Text.Length;
    }

    private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (sender != null && Utils.Dialog((IWin32Window) null, "Are you sure you want to cancel the changes that you just did?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.initializeSourceFields();
      this.updateFieldCounter();
      this.Cursor = Cursors.Default;
    }

    private void updateFieldCounter()
    {
      this.grpSelected.Text = "Selected Fields (" + (object) this.listViewTarget.Items.Count + ")";
      int num = 0;
      if (this.tabControlFields.SelectedTab == this.tabFields)
        num = this.listViewSource.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabDocuments)
        num = this.listViewDocument.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabTasks)
        num = this.listViewTask.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabConditions)
        num = this.listViewCondition.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabLA)
        num = this.listViewMember.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabMilestones)
        num = this.listViewMilestone.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPost)
        num = this.listViewPost.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageLock)
        num = this.listViewLocks.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageIS)
        num = this.listViewIS.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageGFE)
        num = this.listViewGFE.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageTIL)
        num = this.listViewTIL.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageLECD)
        num = this.listViewLECD.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageAlerts)
        num = this.listViewAlerts.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPreliminary)
        num = this.listViewPreliminary.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageAUSTracking)
        num = this.listViewAUSTracking.Items.Count;
      else if (this.tabControlFields.SelectedTab == this.tabPageEDisclosures)
        num = this.listViewEDisclosures.Items.Count;
      this.grpSource.Text = "Fields (" + (object) num + ")";
    }

    private void sourceFields_DoubleClick(object sender, EventArgs e)
    {
      this.addBtn_Click((object) null, (EventArgs) null);
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      this.findField(this.listViewSource);
      this.findField(this.listViewTarget);
    }

    private void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Data Tables");
    }

    private void DataTableDlg_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    private void DataTableDlg_Closing(object sender, CancelEventArgs e)
    {
      if (this.userResponseOnUpdate == DialogResult.Yes)
        return;
      this.DdmDataTableInfo = (DDMDataTableInfo) null;
    }

    private void DataTableDlg_Load(object sender, EventArgs e)
    {
    }

    private void enableUpdate()
    {
      if (!string.IsNullOrEmpty(this.textBoxDDMTableName.Text) && this.listViewTarget.Items.Count > 0)
        this.btnUpdate.Enabled = true;
      else
        this.btnUpdate.Enabled = false;
    }

    private void upArrowButton_Click(object sender, EventArgs e)
    {
      this.MoveItem(DataTableDlg.Direction.UP);
    }

    private void downArrowButton_Click(object sender, EventArgs e)
    {
      this.MoveItem(DataTableDlg.Direction.DOWN);
    }

    private void MoveItem(DataTableDlg.Direction direction)
    {
      this.listViewTarget.BeginUpdate();
      switch (direction)
      {
        case DataTableDlg.Direction.UP:
          for (int index = 0; index < this.listViewTarget.SelectedItems.Count; ++index)
          {
            GVItem selectedItem = this.listViewTarget.SelectedItems[index];
            if (selectedItem.Index + direction >= ~DataTableDlg.Direction.UP && (!this.listViewTarget.Items[0].Selected || !this.listViewTarget.Items[(int) (selectedItem.Index + direction)].Selected))
            {
              int nIndex = (int) (selectedItem.Index + direction);
              this.listViewTarget.Items.RemoveAt(selectedItem.Index);
              this.listViewTarget.Items.Insert(nIndex, selectedItem);
              selectedItem.Selected = true;
            }
          }
          break;
        case DataTableDlg.Direction.DOWN:
          for (int index = this.listViewTarget.SelectedItems.Count - 1; index >= 0; --index)
          {
            GVItem selectedItem = this.listViewTarget.SelectedItems[index];
            if (selectedItem.Index + direction < (DataTableDlg.Direction) this.listViewTarget.Items.Count && (!this.listViewTarget.Items[this.listViewTarget.Items.Count - 1].Selected || !this.listViewTarget.Items[(int) (selectedItem.Index + direction)].Selected))
            {
              int nIndex = (int) (selectedItem.Index + direction);
              this.listViewTarget.Items.RemoveAt(selectedItem.Index);
              this.listViewTarget.Items.Insert(nIndex, selectedItem);
              selectedItem.Selected = true;
            }
          }
          break;
      }
      this.listViewTarget.EndUpdate();
    }

    private void Filter_TextChanged(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      int selectionStart = textBox.SelectionStart;
      textBox.Text = Regex.Replace(textBox.Text, "[^0-9a-zA-Z ]", "");
      textBox.SelectionStart = selectionStart;
      this.enableUpdate();
    }

    private bool MaxFieldsExceeded(GridView targetList, ICollection selectList)
    {
      return targetList.Items.Count + selectList.Count > 40;
    }

    private void tabControlFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.updateFieldCounter();
    }

    private void FocusLeave_textBoxDDMTableName(object sender, EventArgs e)
    {
      this.userResponseOnNameChange = DialogResult.None;
      if (!(this.Text == "Modify Data Table") || this.IsMouseOverUpdateOrClient())
        return;
      this.userResponseOnNameChange = this.PromptUserWithDataTableReference();
    }

    private DialogResult PromptUserWithDataTableReference()
    {
      DialogResult dialogResult = DialogResult.No;
      if (this.textBoxDDMTableName.Text.ToLower() != this.originalTableNameValue.ToLower())
      {
        using (DataTableReference dataTableReference = new DataTableReference(this.ddmDataTableInfo.Name))
        {
          int num = (int) dataTableReference.ShowDialog((IWin32Window) this);
          dialogResult = ((Form) dataTableReference).DialogResult;
        }
      }
      if (dialogResult == DialogResult.Cancel)
        this.textBoxDDMTableName.Text = this.originalTableNameValue;
      return dialogResult;
    }

    private bool IsMouseOverUpdateOrClient()
    {
      Point mousePosition = Control.MousePosition;
      return this.btnUpdate.ClientRectangle.Contains(this.btnUpdate.PointToClient(mousePosition)) | this.btnCancel.ClientRectangle.Contains(this.btnCancel.PointToClient(mousePosition));
    }

    private void listViewTarget_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      if (!(e.Item.SubItems[0].Value is DDMField dbField) || EncompassFields.GetField(dbField.FieldId, this._fieldSettings).Category == FieldCategory.Common || dbField.ComortgagorPair <= 0)
        return;
      using (DDMFieldDialog ddmFd = new DDMFieldDialog(dbField))
      {
        if (ddmFd.ShowDialog((IWin32Window) this) != DialogResult.OK || !ddmFd.DdmField.IsDirty)
          return;
        if (this.listViewTarget.Items.Select<GVItem, DDMField>((Func<GVItem, DDMField>) (item => item.SubItems[0].Value as DDMField)).Any<DDMField>((Func<DDMField, bool>) (listItemField => listItemField != null && listItemField.Equals((object) ddmFd.DdmField))))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "This field and borrower pair index have already been added", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          e.Item.SubItems[0].Value = (object) ddmFd.DdmField;
          e.Item.SubItems[1].Text = ddmFd.DdmField.PairText;
          e.Item.SubItems[2].Text = ddmFd.Description;
        }
      }
    }

    private void tabSelectedFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.tabSelectedFields.SelectedIndex == 0)
        this.handleNormalFieldsTabAssets(true);
      else
        this.handleNormalFieldsTabAssets(false);
    }

    private void handleNormalFieldsTabAssets(bool doesShow)
    {
      this.upArrowButton.Enabled = doesShow;
      this.upArrowButton.Visible = doesShow;
      this.downArrowButton.Enabled = doesShow;
      this.downArrowButton.Visible = doesShow;
      this.addBtn.Enabled = doesShow;
      this.removeBtn.Enabled = doesShow;
    }

    private enum Direction
    {
      UP = -1, // 0xFFFFFFFF
      DOWN = 1,
    }

    internal class DataTableDlgImportHandler
    {
      private DataTableDlg _parent;
      private List<GVItem> _unknownItems = new List<GVItem>();
      private List<GVItem> _replacableItems = new List<GVItem>();

      internal DataTableDlgImportHandler(DataTableDlg parent) => this._parent = parent;

      internal void removeBtn_Click_ImportMode(object sender, EventArgs e)
      {
        if (this._parent.listViewTarget.SelectedItems.Count == 0)
        {
          int num = (int) Utils.Dialog((IWin32Window) this._parent, "You must select a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          this._parent.Cursor = Cursors.WaitCursor;
          if (this._parent.listViewSource.SelectedItems.Count > 0)
            this._parent.listViewSource.SelectedItems.Clear();
          ArrayList arrayList = new ArrayList();
          foreach (GVItem selectedItem in this._parent.listViewTarget.SelectedItems)
            arrayList.Add((object) selectedItem);
          foreach (GVItem gvItem in arrayList)
            this._parent.listViewTarget.Items.Remove(gvItem);
          foreach (GVItem gVItem in arrayList)
          {
            bool flag = this.IsSelectedUnknownItem(gVItem);
            string text = gVItem.SubItems[0].Text;
            string str = flag ? (string) gVItem.Tag : gVItem.SubItems[1].Text;
            FieldDefinition field = EncompassFields.GetField(str, this._parent._fieldSettings);
            if (field == null && this._parent._standardFields.VirtualFields.Contains(str))
              field = this._parent._standardFields.VirtualFields[str];
            if (!str.ToLower().StartsWith("disclosedtil") && !str.ToLower().StartsWith("austracking."))
            {
              if (field != null && field.Category == FieldCategory.Common)
                this._parent.addSourceField(this._parent.listViewSource, field, true);
              this._parent._dataTableData.Remove(str);
              --this._parent._importColumnCount;
            }
          }
          this._parent.updateFieldCounter();
          this._parent.Cursor = Cursors.Default;
          this._parent.enableUpdate();
        }
      }

      internal void listViewTarget_ItemDoubleClick(object sender, GVItemEventArgs e)
      {
        if (this._parent.listViewTarget.SelectedItems.Count == 0)
          return;
        object obj = e.Item.Tag;
        switch (obj)
        {
          case FieldDefinition fieldDefinition when fieldDefinition.Category != FieldCategory.Common:
            DDMField dbField = e.Item.SubItems[1].Value as DDMField;
            using (DDMFieldDialog ddmFieldDialog = new DDMFieldDialog(dbField))
            {
              if (ddmFieldDialog.ShowDialog() != DialogResult.OK)
                return;
              GridView gridView = sender as GridView;
              if (!ddmFieldDialog.DdmField.IsDirty)
                return;
              if (gridView.Items.Select<GVItem, DDMField>((Func<GVItem, DDMField>) (item => item.SubItems[1].Value as DDMField)).Any<DDMField>((Func<DDMField, bool>) (listItemField => listItemField != null && listItemField.Equals((object) ddmFieldDialog.DdmField))))
              {
                int num = (int) Utils.Dialog((IWin32Window) this._parent, "This field and borrower pair index have already been added", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
              }
              e.Item.SubItems[1].Value = (object) ddmFieldDialog.DdmField;
              e.Item.SubItems[2].Text = ddmFieldDialog.DdmField.PairText;
              e.Item.SubItems[3].Text = ddmFieldDialog.Description;
              return;
            }
          case string _:
            int num1 = (int) MessageBox.Show((IWin32Window) this._parent, "Cannot show database field definition for an unassigned field. Please assign a valid field to show it's properties.", "Undefined Field", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          case FieldDefinition _:
            obj = (object) new LoanXDBField((FieldDefinition) obj);
            break;
        }
        using (LoanXDBFieldDialog loanXdbFieldDialog = new LoanXDBFieldDialog((LoanXDBField) obj))
        {
          int num2 = (int) loanXdbFieldDialog.ShowDialog();
        }
      }

      internal string GetSelectedItemFieldID()
      {
        if (this._parent.listViewTarget.SelectedItems.Count == 0)
          return string.Empty;
        object fieldDef = this._parent.listViewTarget.SelectedItems[0].Tag;
        switch (fieldDef)
        {
          case string _:
            int num = (int) MessageBox.Show((IWin32Window) this._parent, "Cannot show database field definition for an unassigned field. Please assign a valid field to show it's properties.", "Undefined Field", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return (string) fieldDef;
          case FieldDefinition _:
            fieldDef = (object) new LoanXDBField((FieldDefinition) fieldDef);
            break;
        }
        return ((LoanXDBField) fieldDef).FieldID;
      }

      internal void SetupUIForImport()
      {
        this.SetTextboxesEditable(false);
        this.SetTargetListViewColumns();
        this.AllowListViewMultiselect(false);
      }

      internal void AllowListViewMultiselect(bool allow)
      {
        this._parent.listViewSource.AllowMultiselect = this._parent.listViewTarget.AllowMultiselect = allow;
      }

      internal void SetTextboxesEditable(bool editable)
      {
        this._parent.textBoxDDMTableDescription.ReadOnly = this._parent.textBoxDDMTableName.ReadOnly = !editable;
      }

      internal void SetTargetListViewData(string[] columns, bool csvHasHeader)
      {
        for (int index = 0; index < columns.Length; ++index)
        {
          string column = columns[index];
          FieldDefinition field = EncompassFields.GetField(column, this._parent._fieldSettings);
          if (field == null)
          {
            this.AddUnknownField(this._parent.listViewTarget, "Column" + (object) (index + 1), column);
            ++this._parent._importColumnCount;
          }
          else
          {
            if (field.Category != FieldCategory.Common)
              field.FieldID = column;
            this._parent.addSourceField(this._parent.listViewTarget, field, false, "Column" + (object) (index + 1));
            if (field.FieldID.ToLower().StartsWith("cx.") || field.FieldID.ToLower() == "loanfolder" || field.FieldID.ToLower() == "loanlastmodified" || field != null && field.Category == FieldCategory.Common)
              this._parent.FieldsToRemove.Add(field);
            ++this._parent._importColumnCount;
          }
        }
      }

      internal void AddUnknownField(
        GridView listViewObject,
        string csvColumn,
        string missingColumn)
      {
        GVItem gvItem = new GVItem(csvColumn);
        GVSubItem gvSubItem = new GVSubItem((object) "Unassigned!");
        gvItem.ForeColor = AppColors.AlertRed;
        gvItem.SubItems.Add(gvSubItem);
        gvItem.Tag = (object) missingColumn;
        listViewObject.Items.Add(gvItem);
        this._unknownItems.Add(gvItem);
      }

      internal void SetTargetListViewColumns()
      {
        this._parent.listViewTarget.Columns.Insert(0, new GVColumn("Column"));
      }

      private void AddReplacableItem(GVItem lvwItem) => this._replacableItems.Add(lvwItem);

      private void RemoveReplacableItem(GVItem lvwItem)
      {
        if (!this._replacableItems.Contains(lvwItem))
          return;
        this._replacableItems.Remove(lvwItem);
      }

      private void UpdateDataTableDataDictionary(string missingField, LoanXDBField loanXDBField)
      {
        if (loanXDBField == null)
          return;
        List<string> stringList = this._parent._dataTableData[missingField.ToUpper()];
        this._parent._dataTableData.Add(loanXDBField.FieldID.ToUpper(), stringList);
        this._parent._dataTableData.Remove(missingField.ToUpper());
      }

      private void RemoveUnknownItem(GVItem unknownItem) => this._unknownItems.Remove(unknownItem);

      private bool UpdateTargetFieldListItem(LoanXDBField dbField, GVItem lvwItem)
      {
        string fieldId = dbField.FieldID;
        string description = dbField.Description;
        FieldDefinition fieldDefinition = EncompassFields.GetField(dbField.FieldID, this._parent._fieldSettings);
        if (fieldDefinition != null)
          fieldId = fieldDefinition.FieldID;
        else if (this._parent._standardFields.VirtualFields.Contains(dbField.FieldID))
        {
          fieldDefinition = this._parent._standardFields.VirtualFields[dbField.FieldID];
          description = fieldDefinition.Description;
        }
        DDMField ddmField = new DDMField(FieldPairParser.ParseFieldPairInfo(fieldDefinition.FieldID));
        if (this._parent.listViewTarget.Items.Select<GVItem, DDMField>((Func<GVItem, DDMField>) (item => item.SubItems[1].Value as DDMField)).Any<DDMField>((Func<DDMField, bool>) (field => field != null && field.FieldIdWithPair == ddmField.FieldIdWithPair)))
        {
          int num = (int) Utils.Dialog((IWin32Window) this._parent, "This Field is already in the selected fields", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
        lvwItem.SubItems[1].Text = fieldDefinition.FieldID;
        lvwItem.SubItems[1].SortValue = !Utils.IsInt((object) fieldId) ? (object) ("B" + fieldId) : (object) ("A" + Utils.ParseInt((object) fieldId).ToString("000000"));
        lvwItem.SubItems[2].Text = fieldDefinition.Category != FieldCategory.Common ? "1st" : "";
        lvwItem.SubItems[3].Text = description;
        switch (dbField.FieldType)
        {
          case LoanXDBTableList.TableTypes.IsNumeric:
            lvwItem.SubItems[4].Text = "Numeric";
            break;
          case LoanXDBTableList.TableTypes.IsDate:
            lvwItem.SubItems[4].Text = "Date";
            break;
          default:
            lvwItem.SubItems[4].Text = "String";
            break;
        }
        lvwItem.Tag = (object) dbField;
        lvwItem.ForeColor = EncompassColors.ControlText;
        return true;
      }

      private GVItem GetNextUnknowItem() => this._unknownItems.FirstOrDefault<GVItem>();

      internal GVItem AddFieldToDestinationList(
        GVItem sourceItem,
        LoanXDBField dbFieldOfSourceItem,
        bool pointToAdd,
        int insertAt)
      {
        string empty = string.Empty;
        GVItem destinationList;
        if (!pointToAdd)
        {
          destinationList = this.GetNextUnknowItem();
          if (destinationList == null)
          {
            int num = (int) Utils.Dialog((IWin32Window) this._parent, string.Format("Cannot add more fields to the datatable", (object) this._parent._importColumnCount), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return (GVItem) null;
          }
          string tag = (string) destinationList.Tag;
          if (!this.UpdateTargetFieldListItem(dbFieldOfSourceItem, destinationList))
            return (GVItem) null;
          this.UpdateDataTableDataDictionary(tag, (LoanXDBField) destinationList.Tag);
          this.RemoveUnknownItem(destinationList);
        }
        else
        {
          GVItem gvItem = this._parent.listViewTarget.Items[insertAt];
          bool flag = this.IsSelectedUnknownItem(gvItem);
          destinationList = new GVItem((object) gvItem.SubItems[0]);
          string str = gvItem.SubItems[1].Value is DDMField ddmField ? (flag ? (string) gvItem.Tag : ddmField.FieldIdWithPair) : (flag ? (string) gvItem.Tag : gvItem.SubItems[1].Text);
          if (!this.UpdateTargetFieldListItem(dbFieldOfSourceItem, destinationList))
            return (GVItem) null;
          this.UpdateDataTableDataDictionary(str, (LoanXDBField) destinationList.Tag);
          this._parent.listViewTarget.Items.Insert(insertAt, destinationList);
          this._parent.listViewTarget.Items.Remove(gvItem);
          if (!flag)
          {
            FieldDefinition field = EncompassFields.GetField(str, this._parent._fieldSettings);
            if (field == null && this._parent._standardFields.VirtualFields.Contains(str))
              field = this._parent._standardFields.VirtualFields[str];
            if (field != null && field.Category == FieldCategory.Common)
              this._parent.addSourceField(this._parent.listViewSource, field, true);
          }
          else
            this.RemoveUnknownItem(gvItem);
          this._parent.updateFieldCounter();
        }
        return destinationList;
      }

      private bool IsSelectedUnknownItem(GVItem gVItem)
      {
        return gVItem != null && this._unknownItems.Contains(gVItem);
      }
    }
  }
}
