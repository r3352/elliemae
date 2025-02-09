// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentMasterCommitmentEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.InputEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentMasterCommitmentEditor : UserControl, IMenuProvider
  {
    private IContainer components;
    private ToolTip toolTips;
    private CollapsibleSplitter collapsibleSplitter1;
    private System.Windows.Forms.Panel pnlLeft;
    private System.Windows.Forms.Panel panel1;
    private GroupContainer groupContainer1;
    private System.Windows.Forms.Panel pnlSecurityTradeInfo;
    private TabControl tabTrade;
    private TabPage tpDetails;
    private System.Windows.Forms.Panel pnlDetails;
    private System.Windows.Forms.Panel pnlRight;
    private TabPage tpHistory;
    private System.Windows.Forms.Panel pnlHistory;
    private GroupContainer grpHistory;
    private StandardIconButton btnExportHistory;
    private EllieMae.EMLite.UI.GridView gvHistory;
    private GroupContainer grpNotes;
    private System.Windows.Forms.TextBox txtNotes;
    private System.Windows.Forms.Button btnDateStamp;
    private BorderPanel grpEditor;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TextBox txtName;
    private System.Windows.Forms.Label label13;
    private System.Windows.Forms.TextBox txtOrgId;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtTPOID;
    private GradientPanel gradientPanel1;
    private System.Windows.Forms.Label lblTradeName;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton btnClose;
    private StandardIconButton btnSave;
    private TableContainer tableContainer1;
    private EllieMae.EMLite.UI.GridView grdDeliveryMethod;
    private System.Windows.Forms.Panel pnlDetailBottom;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox txtAvailableAmount;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtMasterCommitmentAmount;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label3;
    private FlowLayoutPanel flowLayoutPanel2;
    private StandardIconButton siBtnPrint;
    private StandardIconButton siBtnExcel;
    private StandardIconButton btnDeleteDelivery;
    private StandardIconButton btnNew;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.Label label9;
    private ComboBox cmbRateSheet;
    private DatePicker dtExpireDate;
    private DatePicker dtStartDate;
    private System.Windows.Forms.CheckBox chkBestEfforts;
    private System.Windows.Forms.CheckBox chkMandatory;
    private ComboBox cmbExpMinute;
    private ComboBox cmbExpHour;
    private System.Windows.Forms.Label label14;
    private System.Windows.Forms.Label label12;
    private ComboBox cmbEffectiveMinute;
    private ComboBox cmbEffectiveHour;
    private TableLayoutPanel tableLayOut;
    private System.Windows.Forms.TextBox txtNumber;
    private static TradeStatusEnumNameProvider tradeStatusNameProvider = new TradeStatusEnumNameProvider();
    private AssignedCorrespondentTradeList actList;
    private string className = nameof (CorrespondentMasterCommitmentEditor);
    private static string sw = Tracing.SwOutsideLoan;
    private bool commitmentAmountChanged;
    private FieldFilterList advFilter;
    private GridViewReportFilterManager gvFilterManager;
    private string originalMasterNumber = string.Empty;
    private List<CorrespondentMasterDeliveryType> deliveryTypes = new List<CorrespondentMasterDeliveryType>();
    private CorrespondentMasterInfo contractInfo;
    private bool modified;
    private bool readOnly;
    private ExternalOriginatorManagementData tpoSettings;
    private bool refresh;
    private DateTime emptyDate;
    private Decimal CMCavailableAmount;
    private bool readyToSave = true;
    private bool allowPublishEvent;

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
      this.toolTips = new ToolTip(this.components);
      this.siBtnPrint = new StandardIconButton();
      this.siBtnExcel = new StandardIconButton();
      this.btnDeleteDelivery = new StandardIconButton();
      this.btnNew = new StandardIconButton();
      this.btnClose = new StandardIconButton();
      this.btnSave = new StandardIconButton();
      this.btnExportHistory = new StandardIconButton();
      this.grpEditor = new BorderPanel();
      this.tabTrade = new TabControl();
      this.tpDetails = new TabPage();
      this.pnlDetailBottom = new System.Windows.Forms.Panel();
      this.pnlDetails = new System.Windows.Forms.Panel();
      this.pnlRight = new System.Windows.Forms.Panel();
      this.tableContainer1 = new TableContainer();
      this.flowLayoutPanel2 = new FlowLayoutPanel();
      this.grdDeliveryMethod = new EllieMae.EMLite.UI.GridView();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.pnlLeft = new System.Windows.Forms.Panel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.groupContainer1 = new GroupContainer();
      this.txtNumber = new System.Windows.Forms.TextBox();
      this.cmbExpMinute = new ComboBox();
      this.cmbExpHour = new ComboBox();
      this.label14 = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.cmbEffectiveMinute = new ComboBox();
      this.cmbEffectiveHour = new ComboBox();
      this.chkBestEfforts = new System.Windows.Forms.CheckBox();
      this.chkMandatory = new System.Windows.Forms.CheckBox();
      this.dtExpireDate = new DatePicker();
      this.dtStartDate = new DatePicker();
      this.label10 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.cmbRateSheet = new ComboBox();
      this.label6 = new System.Windows.Forms.Label();
      this.txtAvailableAmount = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.txtMasterCommitmentAmount = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label13 = new System.Windows.Forms.Label();
      this.txtOrgId = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.txtTPOID = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.txtName = new System.Windows.Forms.TextBox();
      this.pnlSecurityTradeInfo = new System.Windows.Forms.Panel();
      this.tpHistory = new TabPage();
      this.pnlHistory = new System.Windows.Forms.Panel();
      this.tableLayOut = new TableLayoutPanel();
      this.grpNotes = new GroupContainer();
      this.txtNotes = new System.Windows.Forms.TextBox();
      this.btnDateStamp = new System.Windows.Forms.Button();
      this.grpHistory = new GroupContainer();
      this.gvHistory = new EllieMae.EMLite.UI.GridView();
      this.gradientPanel1 = new GradientPanel();
      this.lblTradeName = new System.Windows.Forms.Label();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      ((ISupportInitialize) this.siBtnPrint).BeginInit();
      ((ISupportInitialize) this.siBtnExcel).BeginInit();
      ((ISupportInitialize) this.btnDeleteDelivery).BeginInit();
      ((ISupportInitialize) this.btnNew).BeginInit();
      ((ISupportInitialize) this.btnClose).BeginInit();
      ((ISupportInitialize) this.btnSave).BeginInit();
      ((ISupportInitialize) this.btnExportHistory).BeginInit();
      this.grpEditor.SuspendLayout();
      this.tabTrade.SuspendLayout();
      this.tpDetails.SuspendLayout();
      this.pnlDetails.SuspendLayout();
      this.pnlRight.SuspendLayout();
      this.tableContainer1.SuspendLayout();
      this.flowLayoutPanel2.SuspendLayout();
      this.pnlLeft.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.tpHistory.SuspendLayout();
      this.pnlHistory.SuspendLayout();
      this.tableLayOut.SuspendLayout();
      this.grpNotes.SuspendLayout();
      this.grpHistory.SuspendLayout();
      this.gradientPanel1.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.siBtnPrint.BackColor = Color.Transparent;
      this.siBtnPrint.Location = new Point(442, 3);
      this.siBtnPrint.Margin = new Padding(3, 3, 2, 3);
      this.siBtnPrint.MouseDownImage = (System.Drawing.Image) null;
      this.siBtnPrint.Name = "siBtnPrint";
      this.siBtnPrint.Size = new Size(16, 16);
      this.siBtnPrint.StandardButtonType = StandardIconButton.ButtonType.PrintButton;
      this.siBtnPrint.TabIndex = 13;
      this.siBtnPrint.TabStop = false;
      this.toolTips.SetToolTip((Control) this.siBtnPrint, "Print Forms");
      this.siBtnPrint.Click += new EventHandler(this.siBtnPrint_Click);
      this.siBtnExcel.BackColor = Color.Transparent;
      this.siBtnExcel.Location = new Point(421, 3);
      this.siBtnExcel.Margin = new Padding(3, 3, 2, 3);
      this.siBtnExcel.MouseDownImage = (System.Drawing.Image) null;
      this.siBtnExcel.Name = "siBtnExcel";
      this.siBtnExcel.Size = new Size(16, 16);
      this.siBtnExcel.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.siBtnExcel.TabIndex = 12;
      this.siBtnExcel.TabStop = false;
      this.toolTips.SetToolTip((Control) this.siBtnExcel, "Exports to Excel");
      this.siBtnExcel.Click += new EventHandler(this.siBtnExcel_Click);
      this.btnDeleteDelivery.BackColor = Color.Transparent;
      this.btnDeleteDelivery.Location = new Point(400, 3);
      this.btnDeleteDelivery.Margin = new Padding(3, 3, 2, 3);
      this.btnDeleteDelivery.MouseDownImage = (System.Drawing.Image) null;
      this.btnDeleteDelivery.Name = "btnDeleteDelivery";
      this.btnDeleteDelivery.Size = new Size(16, 16);
      this.btnDeleteDelivery.StandardButtonType = StandardIconButton.ButtonType.DeleteButton;
      this.btnDeleteDelivery.TabIndex = 0;
      this.btnDeleteDelivery.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnDeleteDelivery, "Delete Delivery Type");
      this.btnDeleteDelivery.Click += new EventHandler(this.btnDeleteDelivery_Click);
      this.btnNew.BackColor = Color.Transparent;
      this.btnNew.Location = new Point(379, 3);
      this.btnNew.Margin = new Padding(3, 3, 2, 3);
      this.btnNew.MouseDownImage = (System.Drawing.Image) null;
      this.btnNew.Name = "btnNew";
      this.btnNew.Size = new Size(16, 16);
      this.btnNew.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.btnNew.TabIndex = 3;
      this.btnNew.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnNew, "New Delivery Type");
      this.btnNew.Click += new EventHandler(this.btnNew_Click);
      this.btnClose.BackColor = Color.Transparent;
      this.btnClose.Location = new Point(922, 3);
      this.btnClose.Margin = new Padding(2, 3, 0, 3);
      this.btnClose.MouseDownImage = (System.Drawing.Image) null;
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new Size(16, 16);
      this.btnClose.StandardButtonType = StandardIconButton.ButtonType.CloseButton;
      this.btnClose.TabIndex = 7;
      this.btnClose.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnClose, "Exit Correspondent Master");
      this.btnClose.Click += new EventHandler(this.btnClose_Click);
      this.btnSave.BackColor = Color.Transparent;
      this.btnSave.Location = new Point(901, 3);
      this.btnSave.MouseDownImage = (System.Drawing.Image) null;
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(16, 16);
      this.btnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.btnSave.TabIndex = 6;
      this.btnSave.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnSave, "Save Correspondent Master");
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnExportHistory.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnExportHistory.BackColor = Color.Transparent;
      this.btnExportHistory.Location = new Point(639, 5);
      this.btnExportHistory.MouseDownImage = (System.Drawing.Image) null;
      this.btnExportHistory.Name = "btnExportHistory";
      this.btnExportHistory.Size = new Size(16, 16);
      this.btnExportHistory.StandardButtonType = StandardIconButton.ButtonType.ExcelButton;
      this.btnExportHistory.TabIndex = 3;
      this.btnExportHistory.TabStop = false;
      this.toolTips.SetToolTip((Control) this.btnExportHistory, "Export to Excel");
      this.btnExportHistory.Click += new EventHandler(this.btnExportHistory_Click);
      this.grpEditor.BackColor = Color.Transparent;
      this.grpEditor.Borders = AnchorStyles.None;
      this.grpEditor.Controls.Add((Control) this.tabTrade);
      this.grpEditor.Controls.Add((Control) this.gradientPanel1);
      this.grpEditor.Dock = DockStyle.Fill;
      this.grpEditor.Location = new Point(0, 0);
      this.grpEditor.Name = "grpEditor";
      this.grpEditor.Padding = new Padding(2, 2, 0, 0);
      this.grpEditor.Size = new Size(1345, 646);
      this.grpEditor.TabIndex = 7;
      this.grpEditor.Text = "<Trade Name>";
      this.tabTrade.Controls.Add((Control) this.tpDetails);
      this.tabTrade.Controls.Add((Control) this.tpHistory);
      this.tabTrade.Dock = DockStyle.Fill;
      this.tabTrade.ItemSize = new Size(44, 20);
      this.tabTrade.Location = new Point(2, 33);
      this.tabTrade.Name = "tabTrade";
      this.tabTrade.Padding = new Point(11, 3);
      this.tabTrade.SelectedIndex = 0;
      this.tabTrade.Size = new Size(1343, 613);
      this.tabTrade.TabIndex = 1;
      this.tpDetails.Controls.Add((Control) this.pnlDetailBottom);
      this.tpDetails.Controls.Add((Control) this.pnlDetails);
      this.tpDetails.Location = new Point(4, 24);
      this.tpDetails.Name = "tpDetails";
      this.tpDetails.Padding = new Padding(0, 2, 2, 2);
      this.tpDetails.Size = new Size(1335, 585);
      this.tpDetails.TabIndex = 0;
      this.tpDetails.Tag = (object) "Details";
      this.tpDetails.Text = "Details";
      this.tpDetails.UseVisualStyleBackColor = true;
      this.pnlDetailBottom.Dock = DockStyle.Fill;
      this.pnlDetailBottom.Location = new Point(0, 300);
      this.pnlDetailBottom.Name = "pnlDetailBottom";
      this.pnlDetailBottom.Size = new Size(1333, 283);
      this.pnlDetailBottom.TabIndex = 11;
      this.pnlDetails.Controls.Add((Control) this.pnlRight);
      this.pnlDetails.Controls.Add((Control) this.collapsibleSplitter1);
      this.pnlDetails.Controls.Add((Control) this.pnlLeft);
      this.pnlDetails.Dock = DockStyle.Top;
      this.pnlDetails.Location = new Point(0, 2);
      this.pnlDetails.Name = "pnlDetails";
      this.pnlDetails.Size = new Size(1333, 298);
      this.pnlDetails.TabIndex = 6;
      this.pnlRight.Controls.Add((Control) this.tableContainer1);
      this.pnlRight.Dock = DockStyle.Fill;
      this.pnlRight.ImeMode = ImeMode.NoControl;
      this.pnlRight.Location = new Point(485, 0);
      this.pnlRight.Name = "pnlRight";
      this.pnlRight.Size = new Size(848, 298);
      this.pnlRight.TabIndex = 8;
      this.tableContainer1.Controls.Add((Control) this.flowLayoutPanel2);
      this.tableContainer1.Controls.Add((Control) this.grdDeliveryMethod);
      this.tableContainer1.Dock = DockStyle.Fill;
      this.tableContainer1.Location = new Point(0, 0);
      this.tableContainer1.Name = "tableContainer1";
      this.tableContainer1.Size = new Size(848, 298);
      this.tableContainer1.Style = TableContainer.ContainerStyle.HeaderOnly;
      this.tableContainer1.TabIndex = 9;
      this.tableContainer1.Text = "Delivery Type View";
      this.flowLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel2.BackColor = Color.Transparent;
      this.flowLayoutPanel2.Controls.Add((Control) this.siBtnPrint);
      this.flowLayoutPanel2.Controls.Add((Control) this.siBtnExcel);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnDeleteDelivery);
      this.flowLayoutPanel2.Controls.Add((Control) this.btnNew);
      this.flowLayoutPanel2.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel2.Location = new Point(388, 3);
      this.flowLayoutPanel2.Name = "flowLayoutPanel2";
      this.flowLayoutPanel2.Size = new Size(460, 22);
      this.flowLayoutPanel2.TabIndex = 5;
      this.flowLayoutPanel2.WrapContents = false;
      this.grdDeliveryMethod.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "clmDeliveryMethod";
      gvColumn1.Text = "Delivery Type";
      gvColumn1.Width = 150;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "clmDeliveryDays";
      gvColumn2.Text = "Delivery Days";
      gvColumn2.Width = 100;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "clmTolerance";
      gvColumn3.Text = "Tolerance%";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "clmEffectiveDate";
      gvColumn4.Text = "Effective Date & Time";
      gvColumn4.Width = 120;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "clmExpireDate";
      gvColumn5.Text = "Expire Date & Time";
      gvColumn5.Width = 120;
      this.grdDeliveryMethod.Columns.AddRange(new GVColumn[5]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5
      });
      this.grdDeliveryMethod.Dock = DockStyle.Fill;
      this.grdDeliveryMethod.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.grdDeliveryMethod.Location = new Point(1, 26);
      this.grdDeliveryMethod.Name = "grdDeliveryMethod";
      this.grdDeliveryMethod.Size = new Size(846, 271);
      this.grdDeliveryMethod.TabIndex = 0;
      this.grdDeliveryMethod.TabStop = false;
      this.grdDeliveryMethod.ItemDoubleClick += new GVItemEventHandler(this.grdDeliveryMethod_ItemDoubleClick);
      this.grdDeliveryMethod.MouseUp += new MouseEventHandler(this.grdDeliveryMethod_MouseUp);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.pnlLeft;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(478, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 7;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.pnlLeft.Controls.Add((Control) this.panel1);
      this.pnlLeft.Controls.Add((Control) this.pnlSecurityTradeInfo);
      this.pnlLeft.Dock = DockStyle.Left;
      this.pnlLeft.Location = new Point(0, 0);
      this.pnlLeft.Name = "pnlLeft";
      this.pnlLeft.Size = new Size(478, 298);
      this.pnlLeft.TabIndex = 6;
      this.panel1.Controls.Add((Control) this.groupContainer1);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(478, 298);
      this.panel1.TabIndex = 2;
      this.groupContainer1.Controls.Add((Control) this.txtNumber);
      this.groupContainer1.Controls.Add((Control) this.cmbExpMinute);
      this.groupContainer1.Controls.Add((Control) this.cmbExpHour);
      this.groupContainer1.Controls.Add((Control) this.label14);
      this.groupContainer1.Controls.Add((Control) this.label12);
      this.groupContainer1.Controls.Add((Control) this.cmbEffectiveMinute);
      this.groupContainer1.Controls.Add((Control) this.cmbEffectiveHour);
      this.groupContainer1.Controls.Add((Control) this.chkBestEfforts);
      this.groupContainer1.Controls.Add((Control) this.chkMandatory);
      this.groupContainer1.Controls.Add((Control) this.dtExpireDate);
      this.groupContainer1.Controls.Add((Control) this.dtStartDate);
      this.groupContainer1.Controls.Add((Control) this.label10);
      this.groupContainer1.Controls.Add((Control) this.label9);
      this.groupContainer1.Controls.Add((Control) this.cmbRateSheet);
      this.groupContainer1.Controls.Add((Control) this.label6);
      this.groupContainer1.Controls.Add((Control) this.txtAvailableAmount);
      this.groupContainer1.Controls.Add((Control) this.label5);
      this.groupContainer1.Controls.Add((Control) this.txtMasterCommitmentAmount);
      this.groupContainer1.Controls.Add((Control) this.label4);
      this.groupContainer1.Controls.Add((Control) this.label3);
      this.groupContainer1.Controls.Add((Control) this.label13);
      this.groupContainer1.Controls.Add((Control) this.txtOrgId);
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.txtTPOID);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.label11);
      this.groupContainer1.Controls.Add((Control) this.txtName);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(0, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(478, 298);
      this.groupContainer1.TabIndex = 0;
      this.groupContainer1.Text = "Master Commitment Info";
      this.txtNumber.Location = new Point(202, 32);
      this.txtNumber.MaxLength = 15;
      this.txtNumber.Name = "txtNumber";
      this.txtNumber.ReadOnly = true;
      this.txtNumber.Size = new Size(146, 20);
      this.txtNumber.TabIndex = 1;
      this.cmbExpMinute.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbExpMinute.FormattingEnabled = true;
      this.cmbExpMinute.ImeMode = ImeMode.NoControl;
      this.cmbExpMinute.Items.AddRange(new object[60]
      {
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbExpMinute.Location = new Point(417, 235);
      this.cmbExpMinute.Name = "cmbExpMinute";
      this.cmbExpMinute.Size = new Size(36, 22);
      this.cmbExpMinute.TabIndex = 86;
      this.cmbExpMinute.Tag = (object) "DateRangeExpiration";
      this.cmbExpMinute.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbExpHour.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbExpHour.FormattingEnabled = true;
      this.cmbExpHour.ImeMode = ImeMode.NoControl;
      this.cmbExpHour.Items.AddRange(new object[24]
      {
        (object) "12 AM",
        (object) "01 AM",
        (object) "02 AM",
        (object) "03 AM",
        (object) "04 AM",
        (object) "05 AM",
        (object) "06 AM",
        (object) "07 AM",
        (object) "08 AM",
        (object) "09 AM",
        (object) "10 AM",
        (object) "11 AM",
        (object) "12 PM",
        (object) "01 PM",
        (object) "02 PM",
        (object) "03 PM",
        (object) "04 PM",
        (object) "05 PM",
        (object) "06 PM",
        (object) "07 PM",
        (object) "08 PM",
        (object) "09 PM",
        (object) "10 PM",
        (object) "11 PM"
      });
      this.cmbExpHour.Location = new Point(353, 235);
      this.cmbExpHour.Name = "cmbExpHour";
      this.cmbExpHour.Size = new Size(58, 22);
      this.cmbExpHour.TabIndex = 85;
      this.cmbExpHour.Tag = (object) "DateRangeExpiration";
      this.cmbExpHour.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(423, 197);
      this.label14.Name = "label14";
      this.label14.Size = new Size(23, 14);
      this.label14.TabIndex = 84;
      this.label14.Text = "MM";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(370, 197);
      this.label12.Name = "label12";
      this.label12.Size = new Size(21, 14);
      this.label12.TabIndex = 83;
      this.label12.Text = "HH";
      this.cmbEffectiveMinute.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEffectiveMinute.FormattingEnabled = true;
      this.cmbEffectiveMinute.ImeMode = ImeMode.NoControl;
      this.cmbEffectiveMinute.Items.AddRange(new object[60]
      {
        (object) "00",
        (object) "01",
        (object) "02",
        (object) "03",
        (object) "04",
        (object) "05",
        (object) "06",
        (object) "07",
        (object) "08",
        (object) "09",
        (object) "10",
        (object) "11",
        (object) "12",
        (object) "13",
        (object) "14",
        (object) "15",
        (object) "16",
        (object) "17",
        (object) "18",
        (object) "19",
        (object) "20",
        (object) "21",
        (object) "22",
        (object) "23",
        (object) "24",
        (object) "25",
        (object) "26",
        (object) "27",
        (object) "28",
        (object) "29 ",
        (object) "30 ",
        (object) "31 ",
        (object) "32 ",
        (object) "33 ",
        (object) "34 ",
        (object) "35 ",
        (object) "36 ",
        (object) "37 ",
        (object) "38 ",
        (object) "39 ",
        (object) "40 ",
        (object) "41 ",
        (object) "42 ",
        (object) "43 ",
        (object) "44 ",
        (object) "45 ",
        (object) "46 ",
        (object) "47 ",
        (object) "48 ",
        (object) "49 ",
        (object) "50 ",
        (object) "51 ",
        (object) "52 ",
        (object) "53 ",
        (object) "54 ",
        (object) "55 ",
        (object) "56 ",
        (object) "57 ",
        (object) "58 ",
        (object) "59 "
      });
      this.cmbEffectiveMinute.Location = new Point(417, 211);
      this.cmbEffectiveMinute.Name = "cmbEffectiveMinute";
      this.cmbEffectiveMinute.Size = new Size(36, 22);
      this.cmbEffectiveMinute.TabIndex = 82;
      this.cmbEffectiveMinute.Tag = (object) "DateRangeEffective";
      this.cmbEffectiveMinute.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbEffectiveHour.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbEffectiveHour.FormattingEnabled = true;
      this.cmbEffectiveHour.ImeMode = ImeMode.NoControl;
      this.cmbEffectiveHour.Items.AddRange(new object[24]
      {
        (object) "12 AM",
        (object) "01 AM",
        (object) "02 AM",
        (object) "03 AM",
        (object) "04 AM",
        (object) "05 AM",
        (object) "06 AM",
        (object) "07 AM",
        (object) "08 AM",
        (object) "09 AM",
        (object) "10 AM",
        (object) "11 AM",
        (object) "12 PM",
        (object) "01 PM",
        (object) "02 PM",
        (object) "03 PM",
        (object) "04 PM",
        (object) "05 PM",
        (object) "06 PM",
        (object) "07 PM",
        (object) "08 PM",
        (object) "09 PM",
        (object) "10 PM",
        (object) "11 PM"
      });
      this.cmbEffectiveHour.Location = new Point(353, 211);
      this.cmbEffectiveHour.Name = "cmbEffectiveHour";
      this.cmbEffectiveHour.Size = new Size(58, 22);
      this.cmbEffectiveHour.TabIndex = 81;
      this.cmbEffectiveHour.Tag = (object) "DateRangeEffective";
      this.cmbEffectiveHour.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.cmbEffectiveHour.Leave += new EventHandler(this.OnFieldValueChangeDateRangeValidation);
      this.chkBestEfforts.AutoSize = true;
      this.chkBestEfforts.Enabled = false;
      this.chkBestEfforts.Location = new Point(285, 121);
      this.chkBestEfforts.Name = "chkBestEfforts";
      this.chkBestEfforts.Size = new Size(84, 18);
      this.chkBestEfforts.TabIndex = 78;
      this.chkBestEfforts.Text = "Best Efforts";
      this.chkBestEfforts.UseVisualStyleBackColor = true;
      this.chkBestEfforts.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.chkMandatory.AutoSize = true;
      this.chkMandatory.Enabled = false;
      this.chkMandatory.Location = new Point(202, 121);
      this.chkMandatory.Name = "chkMandatory";
      this.chkMandatory.Size = new Size(77, 18);
      this.chkMandatory.TabIndex = 77;
      this.chkMandatory.Text = "Mandatory";
      this.chkMandatory.UseVisualStyleBackColor = true;
      this.chkMandatory.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.dtExpireDate.BackColor = SystemColors.Window;
      this.dtExpireDate.Location = new Point(202, 234);
      this.dtExpireDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtExpireDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtExpireDate.Name = "dtExpireDate";
      this.dtExpireDate.Size = new Size(145, 22);
      this.dtExpireDate.TabIndex = 76;
      this.dtExpireDate.Tag = (object) "DateRangeExpiration";
      this.dtExpireDate.ToolTip = "";
      this.dtExpireDate.Value = new DateTime(0L);
      this.dtExpireDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.dtStartDate.BackColor = SystemColors.Window;
      this.dtStartDate.Location = new Point(202, 210);
      this.dtStartDate.MaxValue = new DateTime(2199, 12, 31, 0, 0, 0, 0);
      this.dtStartDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dtStartDate.Name = "dtStartDate";
      this.dtStartDate.Size = new Size(145, 22);
      this.dtStartDate.TabIndex = 75;
      this.dtStartDate.Tag = (object) "DateRangeEffective";
      this.dtStartDate.ToolTip = "";
      this.dtStartDate.Value = new DateTime(0L);
      this.dtStartDate.ValueChanged += new EventHandler(this.onFieldValueChanged);
      this.label10.AutoSize = true;
      this.label10.Location = new Point(7, 239);
      this.label10.Name = "label10";
      this.label10.Size = new Size(133, 14);
      this.label10.TabIndex = 67;
      this.label10.Text = "Master Expire Date && Time";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(7, 217);
      this.label9.Name = "label9";
      this.label9.Size = new Size(146, 14);
      this.label9.TabIndex = 65;
      this.label9.Text = "Master Effective Date && Time";
      this.cmbRateSheet.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRateSheet.FormattingEnabled = true;
      this.cmbRateSheet.Location = new Point(202, 142);
      this.cmbRateSheet.Name = "cmbRateSheet";
      this.cmbRateSheet.Size = new Size(113, 22);
      this.cmbRateSheet.TabIndex = 63;
      this.cmbRateSheet.SelectedIndexChanged += new EventHandler(this.onFieldValueChanged);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(7, 194);
      this.label6.Name = "label6";
      this.label6.Size = new Size(90, 14);
      this.label6.TabIndex = 59;
      this.label6.Text = "Available Amount";
      this.txtAvailableAmount.BackColor = SystemColors.Control;
      this.txtAvailableAmount.Enabled = false;
      this.txtAvailableAmount.Location = new Point(202, 188);
      this.txtAvailableAmount.MaxLength = 12;
      this.txtAvailableAmount.Name = "txtAvailableAmount";
      this.txtAvailableAmount.Size = new Size(146, 20);
      this.txtAvailableAmount.TabIndex = 58;
      this.txtAvailableAmount.TextAlign = HorizontalAlignment.Right;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(7, 172);
      this.label5.Name = "label5";
      this.label5.Size = new Size(139, 14);
      this.label5.TabIndex = 57;
      this.label5.Text = "Master Commitment Amount";
      this.txtMasterCommitmentAmount.BackColor = SystemColors.ControlLightLight;
      this.txtMasterCommitmentAmount.Location = new Point(202, 166);
      this.txtMasterCommitmentAmount.MaxLength = 12;
      this.txtMasterCommitmentAmount.Name = "txtMasterCommitmentAmount";
      this.txtMasterCommitmentAmount.Size = new Size(146, 20);
      this.txtMasterCommitmentAmount.TabIndex = 56;
      this.txtMasterCommitmentAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMasterCommitmentAmount.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 149);
      this.label4.Name = "label4";
      this.label4.Size = new Size(64, 14);
      this.label4.TabIndex = 55;
      this.label4.Text = "Price Group";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 126);
      this.label3.Name = "label3";
      this.label3.Size = new Size(140, 14);
      this.label3.TabIndex = 53;
      this.label3.Text = "Master Commitment Type(s)";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(7, 104);
      this.label13.Name = "label13";
      this.label13.Size = new Size(80, 14);
      this.label13.TabIndex = 50;
      this.label13.Text = "Organization ID";
      this.txtOrgId.BackColor = SystemColors.Control;
      this.txtOrgId.Enabled = false;
      this.txtOrgId.Location = new Point(202, 98);
      this.txtOrgId.MaxLength = 64;
      this.txtOrgId.Name = "txtOrgId";
      this.txtOrgId.Size = new Size(146, 20);
      this.txtOrgId.TabIndex = 49;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 82);
      this.label2.Name = "label2";
      this.label2.Size = new Size(39, 14);
      this.label2.TabIndex = 48;
      this.label2.Text = "TPO ID";
      this.txtTPOID.BackColor = SystemColors.Control;
      this.txtTPOID.Enabled = false;
      this.txtTPOID.Location = new Point(202, 76);
      this.txtTPOID.MaxLength = 64;
      this.txtTPOID.Name = "txtTPOID";
      this.txtTPOID.Size = new Size(146, 20);
      this.txtTPOID.TabIndex = 47;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(7, 35);
      this.label1.Name = "label1";
      this.label1.Size = new Size(109, 14);
      this.label1.TabIndex = 22;
      this.label1.Text = "Master Commitment #";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(7, 60);
      this.label11.Name = "label11";
      this.label11.Size = new Size(82, 14);
      this.label11.TabIndex = 39;
      this.label11.Text = "Company Name";
      this.txtName.BackColor = SystemColors.Control;
      this.txtName.Enabled = false;
      this.txtName.Location = new Point(202, 54);
      this.txtName.MaxLength = 64;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(146, 20);
      this.txtName.TabIndex = 26;
      this.pnlSecurityTradeInfo.Dock = DockStyle.Fill;
      this.pnlSecurityTradeInfo.Location = new Point(0, 0);
      this.pnlSecurityTradeInfo.Name = "pnlSecurityTradeInfo";
      this.pnlSecurityTradeInfo.Size = new Size(478, 298);
      this.pnlSecurityTradeInfo.TabIndex = 0;
      this.tpHistory.Controls.Add((Control) this.pnlHistory);
      this.tpHistory.Location = new Point(4, 24);
      this.tpHistory.Name = "tpHistory";
      this.tpHistory.Padding = new Padding(0, 2, 2, 2);
      this.tpHistory.Size = new Size(1335, 585);
      this.tpHistory.TabIndex = 4;
      this.tpHistory.Tag = (object) "History";
      this.tpHistory.Text = "Notes/History";
      this.tpHistory.UseVisualStyleBackColor = true;
      this.pnlHistory.Controls.Add((Control) this.tableLayOut);
      this.pnlHistory.Dock = DockStyle.Fill;
      this.pnlHistory.Location = new Point(0, 2);
      this.pnlHistory.Name = "pnlHistory";
      this.pnlHistory.Size = new Size(1333, 581);
      this.pnlHistory.TabIndex = 4;
      this.tableLayOut.ColumnCount = 2;
      this.tableLayOut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayOut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayOut.Controls.Add((Control) this.grpNotes, 0, 0);
      this.tableLayOut.Controls.Add((Control) this.grpHistory, 1, 0);
      this.tableLayOut.Dock = DockStyle.Fill;
      this.tableLayOut.Location = new Point(0, 0);
      this.tableLayOut.Name = "tableLayOut";
      this.tableLayOut.RowCount = 1;
      this.tableLayOut.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayOut.Size = new Size(1333, 581);
      this.tableLayOut.TabIndex = 5;
      this.tableLayOut.Resize += new EventHandler(this.tableLayOut_Resize);
      this.grpNotes.Controls.Add((Control) this.txtNotes);
      this.grpNotes.Controls.Add((Control) this.btnDateStamp);
      this.grpNotes.Dock = DockStyle.Fill;
      this.grpNotes.HeaderForeColor = SystemColors.ControlText;
      this.grpNotes.Location = new Point(3, 3);
      this.grpNotes.Name = "grpNotes";
      this.grpNotes.Size = new Size(660, 575);
      this.grpNotes.TabIndex = 3;
      this.grpNotes.Text = "Notes";
      this.txtNotes.BorderStyle = System.Windows.Forms.BorderStyle.None;
      this.txtNotes.Dock = DockStyle.Fill;
      this.txtNotes.Location = new Point(1, 26);
      this.txtNotes.Multiline = true;
      this.txtNotes.Name = "txtNotes";
      this.txtNotes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.txtNotes.Size = new Size(658, 548);
      this.txtNotes.TabIndex = 2;
      this.txtNotes.TextChanged += new EventHandler(this.onFieldValueChanged);
      this.btnDateStamp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnDateStamp.BackColor = SystemColors.Control;
      this.btnDateStamp.Location = new Point(566, 2);
      this.btnDateStamp.Name = "btnDateStamp";
      this.btnDateStamp.Size = new Size(89, 22);
      this.btnDateStamp.TabIndex = 1;
      this.btnDateStamp.Text = "&Date Stamp";
      this.btnDateStamp.UseVisualStyleBackColor = true;
      this.btnDateStamp.Click += new EventHandler(this.btnDateStamp_Click);
      this.grpHistory.Controls.Add((Control) this.btnExportHistory);
      this.grpHistory.Controls.Add((Control) this.gvHistory);
      this.grpHistory.Dock = DockStyle.Fill;
      this.grpHistory.HeaderForeColor = SystemColors.ControlText;
      this.grpHistory.Location = new Point(669, 3);
      this.grpHistory.Name = "grpHistory";
      this.grpHistory.Size = new Size(661, 575);
      this.grpHistory.TabIndex = 4;
      this.grpHistory.Text = "History";
      this.gvHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column1";
      gvColumn6.Text = "Event Time";
      gvColumn6.Width = 125;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.SpringToFit = true;
      gvColumn7.Text = "Event";
      gvColumn7.Width = 409;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column3";
      gvColumn8.Text = "By";
      gvColumn8.Width = 125;
      this.gvHistory.Columns.AddRange(new GVColumn[3]
      {
        gvColumn6,
        gvColumn7,
        gvColumn8
      });
      this.gvHistory.Dock = DockStyle.Fill;
      this.gvHistory.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gvHistory.Location = new Point(1, 26);
      this.gvHistory.Name = "gvHistory";
      this.gvHistory.Size = new Size(659, 548);
      this.gvHistory.TabIndex = 2;
      this.gradientPanel1.BackColorGlassyStyle = true;
      this.gradientPanel1.Borders = AnchorStyles.Bottom;
      this.gradientPanel1.Controls.Add((Control) this.lblTradeName);
      this.gradientPanel1.Controls.Add((Control) this.flowLayoutPanel1);
      this.gradientPanel1.Dock = DockStyle.Top;
      this.gradientPanel1.GradientColor1 = Color.FromArgb(81, 123, 184);
      this.gradientPanel1.GradientColor2 = Color.FromArgb(167, 201, 239);
      this.gradientPanel1.Location = new Point(2, 2);
      this.gradientPanel1.Name = "gradientPanel1";
      this.gradientPanel1.Size = new Size(1343, 31);
      this.gradientPanel1.Style = GradientPanel.PanelStyle.PageHeader;
      this.gradientPanel1.TabIndex = 7;
      this.lblTradeName.AutoSize = true;
      this.lblTradeName.BackColor = Color.Transparent;
      this.lblTradeName.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblTradeName.Location = new Point(8, 8);
      this.lblTradeName.Name = "lblTradeName";
      this.lblTradeName.Size = new Size(84, 14);
      this.lblTradeName.TabIndex = 6;
      this.lblTradeName.Text = "<Trade Name>";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.btnClose);
      this.flowLayoutPanel1.Controls.Add((Control) this.btnSave);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(399, 4);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(938, 22);
      this.flowLayoutPanel1.TabIndex = 5;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((Control) this.grpEditor);
      this.Font = new Font("Arial", 8.25f);
      this.Name = nameof (CorrespondentMasterCommitmentEditor);
      this.Size = new Size(1345, 646);
      ((ISupportInitialize) this.siBtnPrint).EndInit();
      ((ISupportInitialize) this.siBtnExcel).EndInit();
      ((ISupportInitialize) this.btnDeleteDelivery).EndInit();
      ((ISupportInitialize) this.btnNew).EndInit();
      ((ISupportInitialize) this.btnClose).EndInit();
      ((ISupportInitialize) this.btnSave).EndInit();
      ((ISupportInitialize) this.btnExportHistory).EndInit();
      this.grpEditor.ResumeLayout(false);
      this.tabTrade.ResumeLayout(false);
      this.tpDetails.ResumeLayout(false);
      this.pnlDetails.ResumeLayout(false);
      this.pnlRight.ResumeLayout(false);
      this.tableContainer1.ResumeLayout(false);
      this.flowLayoutPanel2.ResumeLayout(false);
      this.pnlLeft.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.tpHistory.ResumeLayout(false);
      this.pnlHistory.ResumeLayout(false);
      this.tableLayOut.ResumeLayout(false);
      this.grpNotes.ResumeLayout(false);
      this.grpNotes.PerformLayout();
      this.grpHistory.ResumeLayout(false);
      this.gradientPanel1.ResumeLayout(false);
      this.gradientPanel1.PerformLayout();
      this.flowLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public event EventHandler DataChange;

    public bool DataModified => this.modified;

    public bool ReadOnly
    {
      get => this.readOnly;
      set
      {
        this.readOnly = value;
        this.setReadOnly();
      }
    }

    private void setReadOnly()
    {
      this.txtNumber.ReadOnly = this.readOnly;
      this.cmbRateSheet.Enabled = this.readOnly;
      this.txtMasterCommitmentAmount.ReadOnly = this.readOnly;
      this.txtMasterCommitmentAmount.Enabled = !this.readOnly;
      this.dtStartDate.ReadOnly = this.readOnly;
      this.dtExpireDate.ReadOnly = this.readOnly;
      this.cmbRateSheet.Enabled = !this.readOnly;
      this.cmbEffectiveHour.Enabled = !this.readOnly;
      this.cmbEffectiveMinute.Enabled = !this.readOnly;
      this.cmbExpHour.Enabled = !this.readOnly;
      this.cmbExpMinute.Enabled = !this.readOnly;
      this.btnSave.Enabled = !this.readOnly;
      this.btnDateStamp.Visible = !this.readOnly;
      this.txtNotes.Enabled = !this.readOnly;
      if (!this.readOnly)
        return;
      this.btnDeleteDelivery.Enabled = false;
      this.btnNew.Enabled = false;
    }

    public CorrespondentMasterCommitmentEditor()
    {
      this.InitializeComponent();
      TextBoxFormatter.Attach(this.txtMasterCommitmentAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      TextBoxFormatter.Attach(this.txtAvailableAmount, TextBoxContentRule.NonNegativeDecimal, "#,##0");
      this.allowPublishEvent = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.AllowPublishEvent"]);
      this.actList = new AssignedCorrespondentTradeList(this.allowPublishEvent);
      this.actList.Dock = DockStyle.Fill;
      this.pnlDetailBottom.Controls.Add((Control) this.actList);
    }

    public void PopulateGrids()
    {
      this.grdDeliveryMethod.Items.Clear();
      foreach (MasterCommitmentDeliveryInfo deliveryInfo in this.contractInfo.DeliveryInfos)
        this.grdDeliveryMethod.Items.Add(this.AddDeliverTypesToGrid(deliveryInfo));
      this.btnNew.Enabled = this.EnableBtnNew();
      this.btnDeleteDelivery.Enabled = this.grdDeliveryMethod.SelectedItems.Count != 0;
      CorrespondentMasterHistoryItem[] correspondentMasterHistory = Session.CorrespondentMasterManager.GetCorrespondentMasterHistory(this.contractInfo.ID);
      this.gvHistory.Items.Clear();
      foreach (CorrespondentMasterHistoryItem historyItem in correspondentMasterHistory)
        this.gvHistory.Items.Add(this.createTradeHistoryListItem(historyItem));
      this.btnExportHistory.Enabled = this.gvHistory.Items.Count > 0;
    }

    private GVItem createTradeHistoryListItem(CorrespondentMasterHistoryItem historyItem)
    {
      return new GVItem()
      {
        Text = historyItem.Timestamp.ToString("MM/dd/yyyy h:mm tt"),
        SubItems = {
          (object) historyItem.Description,
          (object) historyItem.UserName
        },
        Tag = (object) historyItem
      };
    }

    private bool EnableBtnNew()
    {
      return this.grdDeliveryMethod.Items.Count < this.deliveryTypes.Count && !this.readOnly;
    }

    public bool PreValidateCommit() => this.prevalidateCommit();

    private bool prevalidateCommit()
    {
      if (this.dtStartDate.Value.Equals(this.emptyDate) || this.dtExpireDate.Value.Equals(this.emptyDate))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Master Effective and Expire Dates cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      DateTime dateTime = this.GetMasterEffectiveDateTime();
      dateTime = dateTime.Date;
      if (!dateTime.Equals(this.emptyDate))
      {
        dateTime = this.GetMasterExpirationDateTime();
        dateTime = dateTime.Date;
        if (!dateTime.Equals(this.emptyDate))
        {
          if (this.GetMasterEffectiveDateTime() > this.GetMasterExpirationDateTime())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "This Expire Date must occur after the Effective Date.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
          if (this.contractInfo != null && this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => this.GetMasterEffectiveDateTime() > ((MasterCommitmentDeliveryInfo) x.Tag).EffectiveDateTime)).ToList<GVItem>().Count + this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => this.GetMasterExpirationDateTime() < ((MasterCommitmentDeliveryInfo) x.Tag).ExpireDateTime)).ToList<GVItem>().Count > 0)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "All Delivery Type Effective/Expire dates must occur within the Master Effective/Expire Dates.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
          }
        }
      }
      if (this.grdDeliveryMethod.Items.Count > 0)
      {
        dateTime = this.GetMasterEffectiveDateTime();
        dateTime = dateTime.Date;
        if (!dateTime.Equals(this.emptyDate))
        {
          dateTime = this.GetMasterExpirationDateTime();
          dateTime = dateTime.Date;
          if (!dateTime.Equals(this.emptyDate))
            goto label_12;
        }
        int num = (int) Utils.Dialog((IWin32Window) this, "All Delivery Type Effective/Expire dates must occur within the Master Effective/Expire Dates.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
label_12:
      return true;
    }

    public FieldFilterList GetCurrentFilter()
    {
      FieldFilterList fieldFilterList = this.gvFilterManager.ToFieldFilterList();
      if (this.advFilter != null)
        fieldFilterList.AddRange((IEnumerable<FieldFilter>) this.advFilter);
      return fieldFilterList;
    }

    private void btnInvestorTemplate_Click(object sender, EventArgs e)
    {
      using (TPOCompanyMasterSelectorForm masterSelectorForm = new TPOCompanyMasterSelectorForm(Session.ConfigurationManager.GetExternalOrganizationsWithoutExtension(Session.UserID, (string) null), Session.DefaultInstance))
      {
        if (masterSelectorForm.ShowDialog((IWin32Window) Session.DefaultInstance.MainForm) != DialogResult.OK)
          return;
        this.txtTPOID.Text = masterSelectorForm.SelectedOrganization.ExternalID;
        this.txtOrgId.Text = masterSelectorForm.SelectedOrganization.OrgID;
        this.txtName.Text = masterSelectorForm.SelectedOrganization.CompanyLegalName;
      }
    }

    private Decimal GetAvailableAmount(CorrespondentMasterInfo masterInfo, Decimal commitmentAmount)
    {
      return this.tpoSettings == null ? commitmentAmount : CorrespondentMasterCalculation.CalculateAvailableAmountForCmc(commitmentAmount, this.tpoSettings.CommitmentUseBestEffortLimited, Session.CorrespondentTradeManager.GetTradeInfosByMasterId(masterInfo.ID));
    }

    private void onFieldValueChanged(object sender, EventArgs e)
    {
      try
      {
        Utils.ParseDecimal((object) this.txtMasterCommitmentAmount.Text);
      }
      catch
      {
      }
      this.modified = true;
      if (!this.readOnly)
        this.btnSave.Enabled = true;
      if (sender != null && (Convert.ToString(((Control) sender).Tag) == "DateRangeEffective" || Convert.ToString(((Control) sender).Tag) == "DateRangeExpiration"))
        this.OnFieldValueChangeDateRangeValidation(sender, e);
      if (this.DataChange != null)
        this.DataChange((object) this, EventArgs.Empty);
      if (sender == null || ((Control) sender).Name == null || !string.Equals(((Control) sender).Name, "txtMasterCommitmentAmount"))
        return;
      this.txtAvailableAmount.Text = (Utils.ParseDecimal((object) ((Control) sender).Text) + this.CMCavailableAmount).ToString("#,##0;;\"\"");
      if (this.contractInfo.ID <= 0)
        return;
      this.commitmentAmountChanged = true;
    }

    private void OnFieldValueChangeDateRangeValidation(object sender, EventArgs e)
    {
      if (this.refresh)
        return;
      DateTime dateTime1 = this.GetMasterEffectiveDateTime();
      dateTime1 = dateTime1.Date;
      if (dateTime1.Equals(this.emptyDate))
        return;
      DateTime dateTime2 = this.GetMasterExpirationDateTime();
      dateTime2 = dateTime2.Date;
      if (dateTime2.Equals(this.emptyDate))
        return;
      if (this.GetMasterEffectiveDateTime() > this.GetMasterExpirationDateTime())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "This Expire Date must occur after the Effective Date.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        if (!((Control) sender).Focused && sender.GetType() != this.dtStartDate.GetType())
          ((Control) sender).Focus();
        this.readyToSave = false;
      }
      else
      {
        if (this.contractInfo == null)
          return;
        ((Control) sender).Tag.ToString();
        if (this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => this.GetMasterEffectiveDateTime() > ((MasterCommitmentDeliveryInfo) x.Tag).EffectiveDateTime)).ToList<GVItem>().Count + this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => this.GetMasterExpirationDateTime() < ((MasterCommitmentDeliveryInfo) x.Tag).ExpireDateTime)).ToList<GVItem>().Count <= 0)
          return;
        int num = (int) Utils.Dialog((IWin32Window) this, "All Delivery Type Effective/Expire dates must occur within the Master Effective/Expire Dates.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        if (!((Control) sender).Focused && sender.GetType() != this.dtStartDate.GetType())
          ((Control) sender).Focus();
        this.readyToSave = false;
      }
    }

    public void RefreshData(CorrespondentMasterInfo master)
    {
      this.refresh = true;
      this.tpoSettings = Session.DefaultInstance.ConfigurationManager.GetExternalOrganization(false, master.ExternalOriginatorManagementID);
      List<ExternalSettingValue> orgSettingsByName = Session.DefaultInstance.ConfigurationManager.GetExternalOrgSettingsByName("Price Group");
      this.contractInfo = master;
      this.lblTradeName.Text = master.ID > 0 ? "Master Commitment# " + master.Name : "New Master Commitment";
      if (master.ID < 0)
      {
        this.txtNumber.Enabled = !Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableAutoMasterCommitmentNumber"]);
        this.onFieldValueChanged((object) null, (EventArgs) null);
      }
      else
        this.txtNumber.Enabled = false;
      this.cmbRateSheet.Items.Clear();
      foreach (ExternalSettingValue externalSettingValue in orgSettingsByName)
        this.cmbRateSheet.Items.Add((object) new System.Web.UI.WebControls.ListItem(externalSettingValue.settingValue, externalSettingValue.settingValue));
      this.originalMasterNumber = master.Name;
      this.txtNumber.Text = master.Name;
      this.txtName.Text = master.CompanyName;
      this.txtTPOID.Text = master.TpoId;
      this.txtOrgId.Text = master.OrganizationId;
      this.chkMandatory.Checked = MasterCommitmentType.BothMandatoryAndBestEfforts == master.CommitmentType || master.CommitmentType == MasterCommitmentType.Mandatory;
      this.chkBestEfforts.Checked = MasterCommitmentType.BothMandatoryAndBestEfforts == master.CommitmentType || master.CommitmentType == MasterCommitmentType.BestEfforts;
      this.txtMasterCommitmentAmount.Text = Utils.ParseDecimal((object) master.CommitmentAmount, 0M).ToString("#,##0;;\"\"");
      this.txtAvailableAmount.Text = Utils.ParseDecimal((object) master.AvailableAmount, 0M).ToString("#,##0;;\"\"");
      this.dtStartDate.Value = master.MasterEffectiveDateTime;
      this.dtExpireDate.Value = master.MasterExpirationDateTime;
      this.cmbRateSheet.SelectedItem = (object) new System.Web.UI.WebControls.ListItem(master.RateSheet, master.RateSheet);
      ComboBox cmbEffectiveHour = this.cmbEffectiveHour;
      DateTime effectiveDateTime = master.MasterEffectiveDateTime;
      int hour1 = effectiveDateTime.Hour;
      cmbEffectiveHour.SelectedIndex = hour1;
      ComboBox cmbEffectiveMinute = this.cmbEffectiveMinute;
      effectiveDateTime = master.MasterEffectiveDateTime;
      int minute1 = effectiveDateTime.Minute;
      cmbEffectiveMinute.SelectedIndex = minute1;
      ComboBox cmbExpHour = this.cmbExpHour;
      DateTime expirationDateTime = master.MasterExpirationDateTime;
      int hour2 = expirationDateTime.Hour;
      cmbExpHour.SelectedIndex = hour2;
      ComboBox cmbExpMinute = this.cmbExpMinute;
      expirationDateTime = master.MasterExpirationDateTime;
      int minute2 = expirationDateTime.Minute;
      cmbExpMinute.SelectedIndex = minute2;
      this.txtNotes.Text = master.Notes;
      this.CMCavailableAmount = this.GetAvailableAmount(master, 0M);
      this.txtAvailableAmount.Text = (master.CommitmentAmount + this.CMCavailableAmount).ToString("#,##0;;\"\"");
      this.DeliveryTypesByTPO(this.tpoSettings, master.CommitmentType);
      this.PopulateGrids();
      this.loadTradeList();
      this.setEditorTitle();
      if (master.ID > 0)
        this.txtNumber.Enabled = false;
      this.tabTrade.SelectedTab = this.tpDetails;
      this.ReadOnly = this.contractInfo.Status == MasterCommitmentStatus.Archived;
      this.refresh = false;
      this.commitmentAmountChanged = false;
      this.modified = false;
    }

    private List<CorrespondentMasterDeliveryType> DeliveryTypesByTPO(
      ExternalOriginatorManagementData tpoSettings,
      MasterCommitmentType commitmentType)
    {
      this.deliveryTypes.Clear();
      if (tpoSettings != null)
      {
        if (tpoSettings.CommitmentUseBestEffort && (commitmentType == MasterCommitmentType.BestEfforts || commitmentType == MasterCommitmentType.BothMandatoryAndBestEfforts))
          this.deliveryTypes.Add(CorrespondentMasterDeliveryType.IndividualBestEfforts);
        if (tpoSettings.CommitmentMandatory && (commitmentType == MasterCommitmentType.Mandatory || commitmentType == MasterCommitmentType.BothMandatoryAndBestEfforts))
        {
          if (tpoSettings.IsCommitmentDeliveryIndividual)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.IndividualMandatory);
          if (tpoSettings.IsCommitmentDeliveryBulk)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.Bulk);
          if (tpoSettings.IsCommitmentDeliveryAOT)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.AOT);
          if (tpoSettings.IsCommitmentDeliveryBulkAOT)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.BulkAOT);
          if (tpoSettings.IsCommitmentDeliveryLiveTrade)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.LiveTrade);
          if (tpoSettings.IsCommitmentDeliveryCoIssue)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.CoIssue);
          if (tpoSettings.IsCommitmentDeliveryForward)
            this.deliveryTypes.Add(CorrespondentMasterDeliveryType.Forwards);
        }
      }
      return this.deliveryTypes;
    }

    private bool validateData()
    {
      string masterNumber = this.txtNumber.Text.Trim();
      bool boolean = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.EnableAutoMasterCommitmentNumber"]);
      if (!boolean)
      {
        if (this.contractInfo.ID <= 0)
          this.txtNumber.Enabled = true;
        if (masterNumber == "")
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A master commitment number must be provided for this correspondent master commitment.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (string.IsNullOrEmpty(this.originalMasterNumber) && Session.CorrespondentMasterManager.CheckCorrespondentMasterByMasterNumber(masterNumber) != null)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "A Master Commitment with the name/number '" + masterNumber + "' already exists. You must enter a unique name for this correspondent masters.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      if (Utils.ParseDecimal((object) this.txtAvailableAmount.Text, 0M) < 0M && (this.commitmentAmountChanged || this.contractInfo.ID < 0))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Available amount of the Master Commitment cannot be less than 0.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.grdDeliveryMethod.Items.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The delivery type cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (this.contractInfo.CommitmentType == MasterCommitmentType.BothMandatoryAndBestEfforts)
      {
        int count1 = this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => ((MasterCommitmentDeliveryInfo) x.Tag).Type == CorrespondentMasterDeliveryType.IndividualBestEfforts)).ToList<GVItem>().Count;
        int count2 = this.grdDeliveryMethod.Items.Where<GVItem>((Func<GVItem, bool>) (x => ((MasterCommitmentDeliveryInfo) x.Tag).Type != CorrespondentMasterDeliveryType.IndividualBestEfforts)).ToList<GVItem>().Count;
        if (count1 != 1 || count2 < 1)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The delivery type must correspond with both  commitment types selected.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return false;
        }
      }
      Decimal tpoAvailableAmount = Session.CorrespondentTradeManager.CalculateTPOAvailableAmount(this.contractInfo.CommitmentType, this.tpoSettings);
      if (this.tpoSettings.CommitmentTradePolicy == ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation && (this.contractInfo.CommitmentType != MasterCommitmentType.BestEfforts || this.tpoSettings.CommitmentUseBestEffortLimited) && Utils.ParseDecimal((object) this.txtMasterCommitmentAmount.Text, 0M) > tpoAvailableAmount && (this.commitmentAmountChanged || this.contractInfo.ID < 0))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Master Commitment amount cannot be greater than the Available Amount in TPO commitment settings.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      if (boolean && this.contractInfo.ID < 0)
      {
        string createMasterName = Session.CorrespondentMasterManager.GetNextAutoCreateMasterName();
        if (createMasterName == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Correspondent Master cannot be saved because the maximum number of master commitment numbers has been reached. Please go to settings and adjust the starting number", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        this.contractInfo.Name = createMasterName;
      }
      return true;
    }

    private void setEditorTitle()
    {
      string str = "Correspondent Master Contract Details";
      if (this.txtNumber.Text != "")
        str = str + " - " + this.txtNumber.Text;
      this.grpEditor.Text = str;
    }

    private void loadTradeList()
    {
      this.actList.RefreshData(this.contractInfo.ID);
      this.actList.SetMasterSummaryInfo(Session.CorrespondentMasterManager.GetCorrespondentMasterSummary(this.contractInfo.ID));
    }

    private GVItem createTradeListViewItem(CorrespondentTradeViewModel trade)
    {
      return new GVItem()
      {
        Text = trade.Name,
        SubItems = {
          (object) CorrespondentMasterCommitmentEditor.tradeStatusNameProvider.GetName((object) trade.Status),
          trade.DeliveryExpirationDate == DateTime.MinValue ? (object) "" : (object) trade.DeliveryExpirationDate.ToString("MM/dd/yyyy"),
          (object) trade.TradeAmount.ToString("#,##0"),
          (object) trade.AssignedAmount.ToString("#,##0"),
          (object) 0M.ToString("#,##0")
        },
        Tag = (object) trade
      };
    }

    public void MenuClicked(ToolStripItem menuItem)
    {
      switch (string.Concat(menuItem.Tag))
      {
        case "CMCE_SaveCorrespondentMasterCommitment":
          this.SaveCorrespondentMasterEditor();
          break;
        case "CMCE_ExitCorrespondentMasterCommitment":
          TradeManagementConsole.Instance.CloseCorrespondentMaster();
          break;
      }
    }

    public bool SetMenuItemState(ToolStripItem menuItem)
    {
      Control stateControl = (Control) null;
      switch (string.Concat(menuItem.Tag))
      {
        case "CMCE_SaveCorrespodentMasterCommitment":
          stateControl = (Control) this.btnSave;
          break;
        case "CMCE_ExitCorrespodentMasterCommitment":
          stateControl = (Control) this.btnClose;
          break;
      }
      if (stateControl == null)
        return true;
      ClientCommonUtils.ApplyControlStateToMenu(menuItem, stateControl);
      return stateControl.Visible;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
      DateTime dateTime1 = this.dtStartDate.Value;
      dateTime1 = dateTime1.AddHours(Utils.ParseDouble((object) this.cmbEffectiveHour.SelectedIndex, 0.0));
      DateTime startRange = dateTime1.AddMinutes(Utils.ParseDouble((object) this.cmbEffectiveMinute.SelectedIndex, 0.0));
      DateTime dateTime2 = this.dtExpireDate.Value;
      dateTime2 = dateTime2.AddHours(Utils.ParseDouble((object) this.cmbExpHour.SelectedIndex, 0.0));
      DateTime endRange = dateTime2.AddMinutes(Utils.ParseDouble((object) this.cmbExpMinute.SelectedIndex, 0.0));
      if (startRange.Equals(this.emptyDate) || endRange.Equals(this.emptyDate))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Master Effective and Expire Dates cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        MasterCommitmentDeliveryInfo deliveryInfo = new MasterCommitmentDeliveryInfo();
        using (CorrespondentDeliveryType correspondentDeliveryType = new CorrespondentDeliveryType(this.deliveryTypes.Except<CorrespondentMasterDeliveryType>((IEnumerable<CorrespondentMasterDeliveryType>) this.grdDeliveryMethod.Items.Select<GVItem, CorrespondentMasterDeliveryType>((Func<GVItem, CorrespondentMasterDeliveryType>) (s => (CorrespondentMasterDeliveryType) s.SubItems[0].Tag)).ToArray<CorrespondentMasterDeliveryType>()).ToArray<CorrespondentMasterDeliveryType>(), deliveryInfo, startRange: startRange, endRange: endRange))
        {
          if (correspondentDeliveryType.ShowDialog((IWin32Window) this) == DialogResult.OK)
          {
            this.grdDeliveryMethod.Items.Add(this.AddDeliverTypesToGrid(correspondentDeliveryType.SelectedMasterCommitmentDeliveryInfo));
            this.onFieldValueChanged(sender, e);
          }
        }
        this.btnNew.Enabled = this.EnableBtnNew();
      }
    }

    private GVItem AddDeliverTypesToGrid(MasterCommitmentDeliveryInfo masterDeliveryInfo)
    {
      return new GVItem()
      {
        SubItems = {
          [0] = {
            Value = (object) masterDeliveryInfo.Type.ToDescription(),
            Tag = (object) masterDeliveryInfo.Type
          },
          [1] = {
            Value = (object) masterDeliveryInfo.DeliveryDays.ToString()
          },
          [2] = {
            Value = (object) masterDeliveryInfo.Tolerance.ToString("0.0000")
          },
          [3] = {
            Value = (object) string.Format("{0:MM/dd/yy hh:mm tt}", (object) masterDeliveryInfo.EffectiveDateTime),
            Tag = (object) masterDeliveryInfo.EffectiveDateTime
          },
          [4] = {
            Value = (object) string.Format("{0:MM/dd/yy hh:mm tt}", (object) masterDeliveryInfo.ExpireDateTime),
            Tag = (object) masterDeliveryInfo.ExpireDateTime
          }
        },
        Tag = (object) masterDeliveryInfo
      };
    }

    private void btnDeleteDelivery_Click(object sender, EventArgs e)
    {
      this.grdDeliveryMethod.Items.Remove(this.grdDeliveryMethod.SelectedItems[0]);
      this.btnDeleteDelivery.Enabled = false;
      this.btnNew.Enabled = true;
      this.onFieldValueChanged(sender, e);
    }

    private void grdDeliveryMethod_MouseUp(object sender, MouseEventArgs e)
    {
      if (this.readOnly)
        return;
      if (this.grdDeliveryMethod.SelectedItems.Count == 0)
        this.btnDeleteDelivery.Enabled = false;
      else
        this.btnDeleteDelivery.Enabled = true;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      TradeManagementConsole.Instance.CloseCorrespondentMaster();
    }

    public void DisconnectDateRangeValidationEvents()
    {
      this.dtStartDate.Leave -= new EventHandler(this.dtStartDate_Leave);
      this.dtExpireDate.Leave -= new EventHandler(this.dtExpireDate_Leave);
    }

    public void InitializeDateRangeValidationEvents()
    {
      this.dtStartDate.Leave += new EventHandler(this.dtStartDate_Leave);
      this.dtExpireDate.Leave += new EventHandler(this.dtExpireDate_Leave);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.SaveCorrespondentMasterEditor();

    public bool SaveCorrespondentMasterEditor()
    {
      if (!this.SaveCorrespondentMaster())
        return false;
      this.modified = false;
      return true;
    }

    public bool SaveCorrespondentMaster()
    {
      try
      {
        if (!this.readyToSave)
        {
          this.readyToSave = true;
          return false;
        }
        if (!this.prevalidateCommit() || !this.validateData())
          return false;
        if (this.txtNumber.Enabled)
          this.contractInfo.Name = this.txtNumber.Text;
        this.contractInfo.CommitmentAmount = Utils.ParseDecimal((object) this.txtMasterCommitmentAmount.Text, 0M);
        this.contractInfo.MasterEffectiveDateTime = this.GetMasterEffectiveDateTime();
        this.contractInfo.MasterExpirationDateTime = this.GetMasterExpirationDateTime();
        if (!this.chkBestEfforts.Checked && !this.chkMandatory.Checked)
          this.contractInfo.CommitmentType = MasterCommitmentType.None;
        if (!this.chkBestEfforts.Checked && this.chkMandatory.Checked)
          this.contractInfo.CommitmentType = MasterCommitmentType.Mandatory;
        if (this.chkBestEfforts.Checked && !this.chkMandatory.Checked)
          this.contractInfo.CommitmentType = MasterCommitmentType.BestEfforts;
        if (this.chkBestEfforts.Checked && this.chkMandatory.Checked)
          this.contractInfo.CommitmentType = MasterCommitmentType.BothMandatoryAndBestEfforts;
        this.contractInfo.Notes = this.txtNotes.Text;
        if (this.cmbRateSheet.SelectedItem != null)
          this.contractInfo.RateSheet = ((System.Web.UI.WebControls.ListItem) this.cmbRateSheet.SelectedItem).Text;
        this.contractInfo.CommitmentAmount = Utils.ParseDecimal((object) this.txtMasterCommitmentAmount.Text, 0M);
        this.contractInfo.AvailableAmount = Utils.ParseDecimal((object) this.txtAvailableAmount.Text, 0M);
        this.contractInfo.DeliveryInfos = this.grdDeliveryMethod.Items.Select<GVItem, MasterCommitmentDeliveryInfo>((Func<GVItem, MasterCommitmentDeliveryInfo>) (x => new MasterCommitmentDeliveryInfo()
        {
          Type = (CorrespondentMasterDeliveryType) x.SubItems["clmDeliveryMethod"].Tag,
          DeliveryDays = Utils.ParseInt((object) x.SubItems["clmDeliveryDays"].Value.ToString()),
          Tolerance = Utils.ParseDecimal((object) x.SubItems["clmTolerance"].Value.ToString()),
          EffectiveDateTime = Utils.ParseDate(x.SubItems["clmEffectiveDate"].Tag),
          ExpireDateTime = Utils.ParseDate(x.SubItems["clmExpireDate"].Tag)
        })).ToList<MasterCommitmentDeliveryInfo>();
        if (this.contractInfo.ID < 0)
          this.contractInfo.ID = Session.CorrespondentMasterManager.CreateCorrespondentMaster(this.contractInfo);
        else
          Session.CorrespondentMasterManager.UpdateCorrespondentMaster(this.contractInfo);
        this.contractInfo = Session.CorrespondentMasterManager.GetCorrespondentMaster(this.contractInfo.ID);
        this.RefreshData(this.contractInfo);
        return true;
      }
      catch (ObjectNotFoundException ex)
      {
        return false;
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterCommitmentEditor.sw, this.className, TraceLevel.Error, ex.ToString());
        int num = (int) Utils.Dialog((IWin32Window) this, "The correspondent master could not be saved due to the following error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void grdDeliveryMethod_ItemDoubleClick(object source, GVItemEventArgs e)
    {
      DateTime effectiveDateTime = this.GetMasterEffectiveDateTime();
      DateTime expirationDateTime = this.GetMasterExpirationDateTime();
      MasterCommitmentDeliveryInfo tag = (MasterCommitmentDeliveryInfo) this.grdDeliveryMethod.SelectedItems[0].Tag;
      using (CorrespondentDeliveryType correspondentDeliveryType = new CorrespondentDeliveryType(new CorrespondentMasterDeliveryType[1]
      {
        tag.Type
      }, tag, true, effectiveDateTime, expirationDateTime))
      {
        correspondentDeliveryType.ReadOnly = this.contractInfo.Status == MasterCommitmentStatus.Archived;
        if (correspondentDeliveryType.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        GVItem grid = this.AddDeliverTypesToGrid(correspondentDeliveryType.SelectedMasterCommitmentDeliveryInfo);
        foreach (GVSubItem subItem in (IEnumerable<GVSubItem>) this.grdDeliveryMethod.SelectedItems[0].SubItems)
        {
          this.grdDeliveryMethod.SelectedItems[0].SubItems[subItem.Index].Value = grid.SubItems[subItem.Index].Value;
          this.grdDeliveryMethod.SelectedItems[0].SubItems[subItem.Index].Tag = grid.SubItems[subItem.Index].Tag;
        }
        this.onFieldValueChanged(source, (EventArgs) e);
      }
    }

    private void siBtnExcel_Click(object sender, EventArgs e)
    {
      if (this.grdDeliveryMethod.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more correspondent delivery types from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.exportDeliveryTypes();
    }

    private bool exportDeliveryTypes()
    {
      if (this.grdDeliveryMethod.Columns.Count > ExcelHandler.GetMaximumColumnCount())
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You contract list cannot be exported because the number of columns exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumColumnCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      if (this.grdDeliveryMethod.Items.Count > ExcelHandler.GetMaximumRowCount() - 1)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You contract list cannot be exported because the number of rows exceeds the limit supported by Excel (" + (object) ExcelHandler.GetMaximumRowCount() + ")", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.exportSelectedRowsToExcel();
      return true;
    }

    private void exportSelectedRowsToExcel()
    {
      CorrespondentMasterReportFieldDefs fieldDefs = CorrespondentMasterReportFieldDefs.GetFieldDefs();
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.grdDeliveryMethod, (ReportFieldDefs) fieldDefs, false);
      excelHandler.CreateExcel();
    }

    private void siBtnPrint_Click(object sender, EventArgs e)
    {
      if (this.grdDeliveryMethod.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must first select one or more correspondent delivery types from the list.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
        this.printSelectedDeliveryTypes();
    }

    private void printSelectedDeliveryTypes()
    {
      CorrespondentMasterReportFieldDefs fieldDefs = CorrespondentMasterReportFieldDefs.GetFieldDefs();
      ExcelHandler excelHandler = new ExcelHandler();
      excelHandler.AddDataTable(this.grdDeliveryMethod, (ReportFieldDefs) fieldDefs, true);
      excelHandler.Print();
    }

    private void btnDateStamp_Click(object sender, EventArgs e)
    {
      string str = Utils.CreateTimestamp(true) + " " + Session.UserInfo.FullName + "> ";
      if (this.txtNotes.Text.Length != 0)
        this.txtNotes.Text += Environment.NewLine;
      this.txtNotes.Text += str;
      this.txtNotes.SelectionStart = this.txtNotes.Text.Length;
      this.txtNotes.Focus();
    }

    private void tableLayOut_Resize(object sender, EventArgs e)
    {
      this.grpNotes.Width = this.grpNotes.Parent.Width;
      this.grpHistory.Width = this.grpHistory.Parent.Width;
    }

    private void btnExportHistory_Click(object sender, EventArgs e)
    {
      try
      {
        ExcelHandler excelHandler = new ExcelHandler();
        excelHandler.AddDataTable(this.gvHistory, false);
        excelHandler.CreateExcel();
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentMasterCommitmentEditor.sw, this.className, TraceLevel.Error, "Error during export: " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) this, "An error occurred while attempting to export the loans to Microsoft Excel. Ensure that you have Excel installed and it is working properly.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private DateTime GetMasterEffectiveDateTime()
    {
      return this.dtStartDate.Value.Equals(this.emptyDate) || this.dtStartDate.Value < this.dtStartDate.MinValue || this.dtStartDate.Value > this.dtStartDate.MaxValue ? this.emptyDate : this.dtStartDate.Value.AddHours((double) this.IndexToHour(Utils.ParseInt((object) this.cmbEffectiveHour.SelectedIndex))).AddMinutes(Utils.ParseDouble((object) this.cmbEffectiveMinute.Text, 0.0));
    }

    private DateTime GetMasterExpirationDateTime()
    {
      return this.dtExpireDate.Value.Equals(this.emptyDate) || this.dtExpireDate.Value < this.dtExpireDate.MinValue || this.dtExpireDate.Value > this.dtExpireDate.MaxValue ? this.emptyDate : this.dtExpireDate.Value.AddHours((double) this.IndexToHour(Utils.ParseInt((object) this.cmbExpHour.SelectedIndex))).AddMinutes(Utils.ParseDouble((object) this.cmbExpMinute.Text, 0.0));
    }

    private void dtStartDate_Leave(object sender, EventArgs e)
    {
      this.OnFieldValueChangeDateRangeValidation(sender, e);
    }

    private void dtExpireDate_Leave(object sender, EventArgs e)
    {
      this.OnFieldValueChangeDateRangeValidation(sender, e);
    }

    public int HourToIndex(int hour) => hour == 0 ? 0 : hour;

    public int IndexToHour(int index) => index;
  }
}
