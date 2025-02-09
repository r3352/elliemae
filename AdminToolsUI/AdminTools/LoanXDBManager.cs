// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoanXDBManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Reporting;
using EllieMae.EMLite.Setup;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoanXDBManager : Form
  {
    private static int rebuildReportingDbThreadCount = 0;
    public const int MAXFIELDCOUNT = 1500;
    private const string className = "LoanXDBManager";
    private static readonly string sw = Tracing.SwReportControl;
    private TextBox textBoxFind;
    private GridView listViewSource;
    private IContainer components;
    private LoanXDBTableList tableList;
    private LoanXDBStatusInfo xdbStatus;
    private Button findBtn;
    private Panel panel1;
    private Button addBtn;
    private Button removeBtn;
    private Button btnCancel;
    private Button btnUpdate;
    private Button btnCreate;
    private GridView listViewTarget;
    private Label lblCount;
    private Button btnCompare;
    private Button btnReset;
    private TabPage tabFields;
    private TabPage tabConditions;
    private TabPage tabDocuments;
    private TabPage tabLA;
    private TabPage tabMilestones;
    private TabPage tabPost;
    private GridView listViewCondition;
    private GridView listViewDocument;
    private GridView listViewMember;
    private GridView listViewMilestone;
    private GridView listViewPost;
    private TabControl tabControlFields;
    private bool isDirty;
    private FieldSettings fieldSettings;
    private Hashtable quickTables;
    private Hashtable quickTablesTarget;
    private TabPage tabPageLock;
    private GridView listViewLocks;
    private Button btnPopulate;
    private TabPage tabTasks;
    private GridView listViewTask;
    private TabPage tabPageIS;
    private GridView listViewIS;
    private TabPage tabPageGFE;
    private TabPage tabPageTIL;
    private GridView listViewGFE;
    private GridView listViewTIL;
    private bool keepTables;
    private Panel panel3;
    private GroupContainer grpSelected;
    private GroupContainer grpSource;
    private CollapsibleSplitter collapsibleSplitter1;
    private FlowLayoutPanel flowLayoutPanel1;
    private StandardIconButton editBtn;
    private StandardIconButton quickAddBtn;
    private ToolTip toolTip1;
    private TabPage tabPageAlerts;
    private GridView listViewAlerts;
    private bool useERDB;
    private List<string> existingFields = new List<string>();
    private List<string> newFields = new List<string>();
    private HelpLink lnkHelp;
    private TabPage tabPreliminary;
    private GridView listViewPreliminary;
    private bool showFieldCountWarning = true;
    private StandardIconButton exportBtn;
    private GVItem lastAddedTargetGridViewItem;
    private TabPage tabPageAUSTracking;
    private GridView listViewAUSTracking;
    private TabPage tabPageEDisclosures;
    private GridView listViewEDisclosures;
    private TabPage tabPageLECD;
    private GridView listViewLECD;
    private Button btnPreferences;
    private StandardFields standardFields;
    private Hashtable selectedFields;

    public LoanXDBManager(bool useERDB)
    {
      try
      {
        this.standardFields = Session.LoanManager.GetStandardFields();
      }
      catch (Exception ex)
      {
        this.standardFields = StandardFields.Instance;
        int num = (int) MessageBox.Show("Error getting standard fields definition from server. Use local definition file instead.\r\n" + ex.Message);
      }
      this.useERDB = false;
      this.InitializeComponent();
      if (this.useERDB)
      {
        this.Text = "External Reporting Database";
        this.listViewTarget.Columns.RemoveAt(6);
      }
      this.btnReset_Click((object) null, (EventArgs) null);
      if (EnConfigurationSettings.GlobalSettings.AllowRecreateRDB)
      {
        this.addBtn.Enabled = false;
        this.removeBtn.Enabled = false;
        this.quickAddBtn.Enabled = false;
        this.editBtn.Enabled = false;
        this.btnUpdate.Enabled = false;
        this.btnReset.Enabled = false;
        this.btnCreate.Visible = this.btnPopulate.Visible = true;
      }
      this.listViewCondition.Sort(0, SortOrder.Ascending);
      this.listViewDocument.Sort(0, SortOrder.Ascending);
      this.listViewGFE.Sort(0, SortOrder.Ascending);
      this.listViewIS.Sort(0, SortOrder.Ascending);
      this.listViewLocks.Sort(0, SortOrder.Ascending);
      this.listViewMember.Sort(0, SortOrder.Ascending);
      this.listViewMilestone.Sort(0, SortOrder.Ascending);
      this.listViewPost.Sort(0, SortOrder.Ascending);
      this.listViewSource.Sort(0, SortOrder.Ascending);
      this.listViewTarget.Sort(0, SortOrder.Ascending);
      this.listViewTask.Sort(0, SortOrder.Ascending);
      this.listViewTIL.Sort(0, SortOrder.Ascending);
      this.listViewAlerts.Sort(0, SortOrder.Ascending);
      this.listViewPreliminary.Sort(0, SortOrder.Ascending);
      this.listViewAUSTracking.Sort(0, SortOrder.Ascending);
      this.listViewEDisclosures.Sort(0, SortOrder.Ascending);
      this.listViewLECD.Sort(0, SortOrder.Ascending);
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
      GVColumn gvColumn53 = new GVColumn();
      GVColumn gvColumn54 = new GVColumn();
      GVColumn gvColumn55 = new GVColumn();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoanXDBManager));
      this.panel1 = new Panel();
      this.grpSelected = new GroupContainer();
      this.flowLayoutPanel1 = new FlowLayoutPanel();
      this.exportBtn = new StandardIconButton();
      this.editBtn = new StandardIconButton();
      this.quickAddBtn = new StandardIconButton();
      this.listViewTarget = new GridView();
      this.addBtn = new Button();
      this.removeBtn = new Button();
      this.lblCount = new Label();
      this.btnPopulate = new Button();
      this.btnCancel = new Button();
      this.btnUpdate = new Button();
      this.btnCreate = new Button();
      this.btnCompare = new Button();
      this.btnReset = new Button();
      this.panel3 = new Panel();
      this.btnPreferences = new Button();
      this.lnkHelp = new HelpLink();
      this.toolTip1 = new ToolTip(this.components);
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
      this.panel1.SuspendLayout();
      this.grpSelected.SuspendLayout();
      this.flowLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.exportBtn).BeginInit();
      ((ISupportInitialize) this.editBtn).BeginInit();
      ((ISupportInitialize) this.quickAddBtn).BeginInit();
      this.panel3.SuspendLayout();
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
      this.panel1.Controls.Add((Control) this.grpSelected);
      this.panel1.Controls.Add((Control) this.addBtn);
      this.panel1.Controls.Add((Control) this.removeBtn);
      this.panel1.Controls.Add((Control) this.lblCount);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(358, 5);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(521, 595);
      this.panel1.TabIndex = 21;
      this.grpSelected.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.grpSelected.Controls.Add((Control) this.flowLayoutPanel1);
      this.grpSelected.Controls.Add((Control) this.listViewTarget);
      this.grpSelected.HeaderForeColor = SystemColors.ControlText;
      this.grpSelected.Location = new Point(80, 0);
      this.grpSelected.Name = "grpSelected";
      this.grpSelected.Size = new Size(440, 595);
      this.grpSelected.TabIndex = 19;
      this.grpSelected.Text = "Selected Fields";
      this.flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.flowLayoutPanel1.BackColor = Color.Transparent;
      this.flowLayoutPanel1.Controls.Add((Control) this.exportBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.editBtn);
      this.flowLayoutPanel1.Controls.Add((Control) this.quickAddBtn);
      this.flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
      this.flowLayoutPanel1.Location = new Point(361, 2);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new Size(74, 22);
      this.flowLayoutPanel1.TabIndex = 19;
      this.exportBtn.BackColor = Color.Transparent;
      this.exportBtn.Location = new Point(55, 3);
      this.exportBtn.MouseDownImage = (Image) null;
      this.exportBtn.Name = "exportBtn";
      this.exportBtn.Size = new Size(16, 16);
      this.exportBtn.StandardButtonType = StandardIconButton.ButtonType.ExportDataToFileButton;
      this.exportBtn.TabIndex = 20;
      this.exportBtn.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.exportBtn, "Export");
      this.exportBtn.Click += new EventHandler(this.exportBtn_Click);
      this.editBtn.BackColor = Color.Transparent;
      this.editBtn.Location = new Point(36, 3);
      this.editBtn.Margin = new Padding(5, 3, 0, 3);
      this.editBtn.MouseDownImage = (Image) null;
      this.editBtn.Name = "editBtn";
      this.editBtn.Size = new Size(16, 16);
      this.editBtn.StandardButtonType = StandardIconButton.ButtonType.EditButton;
      this.editBtn.TabIndex = 0;
      this.editBtn.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.editBtn, "Edit");
      this.editBtn.Click += new EventHandler(this.editBtn_Click);
      this.quickAddBtn.BackColor = Color.Transparent;
      this.quickAddBtn.Location = new Point(15, 3);
      this.quickAddBtn.Margin = new Padding(5, 3, 0, 3);
      this.quickAddBtn.MouseDownImage = (Image) null;
      this.quickAddBtn.Name = "quickAddBtn";
      this.quickAddBtn.Size = new Size(16, 16);
      this.quickAddBtn.StandardButtonType = StandardIconButton.ButtonType.NewButton;
      this.quickAddBtn.TabIndex = 1;
      this.quickAddBtn.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.quickAddBtn, "Quick Add");
      this.quickAddBtn.Click += new EventHandler(this.quickAddBtn_Click);
      this.listViewTarget.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.Text = "Field ID";
      gvColumn1.Width = 91;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.Text = "Pair";
      gvColumn2.Width = 39;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.Text = "Description";
      gvColumn3.Width = 180;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.Text = "Type";
      gvColumn4.Width = 80;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column5";
      gvColumn5.Text = "Size";
      gvColumn5.Width = 50;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column6";
      gvColumn6.Text = "Use Index";
      gvColumn6.Width = 60;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column7";
      gvColumn7.Text = "Auditable";
      gvColumn7.Width = 60;
      this.listViewTarget.Columns.AddRange(new GVColumn[7]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7
      });
      this.listViewTarget.Dock = DockStyle.Fill;
      this.listViewTarget.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTarget.Location = new Point(1, 26);
      this.listViewTarget.Name = "listViewTarget";
      this.listViewTarget.Size = new Size(438, 568);
      this.listViewTarget.TabIndex = 3;
      this.listViewTarget.DoubleClick += new EventHandler(this.listViewTarget_DoubleClick);
      this.addBtn.Image = (Image) Resources.arrow_forward;
      this.addBtn.ImageAlign = ContentAlignment.TopCenter;
      this.addBtn.Location = new Point(4, 222);
      this.addBtn.Name = "addBtn";
      this.addBtn.Size = new Size(72, 25);
      this.addBtn.TabIndex = 1;
      this.addBtn.Text = "Add";
      this.addBtn.TextAlign = ContentAlignment.MiddleRight;
      this.addBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
      this.addBtn.Click += new EventHandler(this.addBtn_Click);
      this.removeBtn.Image = (Image) Resources.arrow_back;
      this.removeBtn.Location = new Point(4, (int) byte.MaxValue);
      this.removeBtn.Name = "removeBtn";
      this.removeBtn.Size = new Size(72, 25);
      this.removeBtn.TabIndex = 2;
      this.removeBtn.Text = "Remove";
      this.removeBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
      this.removeBtn.Click += new EventHandler(this.removeBtn_Click);
      this.lblCount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblCount.Location = new Point(168, 9);
      this.lblCount.Name = "lblCount";
      this.lblCount.Size = new Size(168, 16);
      this.lblCount.TabIndex = 12;
      this.btnPopulate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPopulate.Location = new Point(373, 9);
      this.btnPopulate.Name = "btnPopulate";
      this.btnPopulate.Size = new Size(75, 22);
      this.btnPopulate.TabIndex = 19;
      this.btnPopulate.Text = "Re-populate";
      this.btnPopulate.Visible = false;
      this.btnPopulate.Click += new EventHandler(this.btnPopulate_Click);
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
      this.btnUpdate.Text = "&Update";
      this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
      this.btnCreate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCreate.Location = new Point(451, 9);
      this.btnCreate.Name = "btnCreate";
      this.btnCreate.Size = new Size(75, 22);
      this.btnCreate.TabIndex = 17;
      this.btnCreate.Text = "Re-create";
      this.btnCreate.Visible = false;
      this.btnCreate.Click += new EventHandler(this.btnCreate_Click);
      this.btnCompare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCompare.Location = new Point(607, 9);
      this.btnCompare.Name = "btnCompare";
      this.btnCompare.Size = new Size(106, 22);
      this.btnCompare.TabIndex = 5;
      this.btnCompare.Text = "Review Ch&anges";
      this.btnCompare.Click += new EventHandler(this.btnCompare_Click);
      this.btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnReset.Location = new Point(529, 9);
      this.btnReset.Name = "btnReset";
      this.btnReset.Size = new Size(75, 22);
      this.btnReset.TabIndex = 6;
      this.btnReset.Text = "&Reset";
      this.btnReset.Click += new EventHandler(this.btnReset_Click);
      this.panel3.Controls.Add((Control) this.btnPreferences);
      this.panel3.Controls.Add((Control) this.lnkHelp);
      this.panel3.Controls.Add((Control) this.btnPopulate);
      this.panel3.Controls.Add((Control) this.btnCancel);
      this.panel3.Controls.Add((Control) this.btnCreate);
      this.panel3.Controls.Add((Control) this.btnUpdate);
      this.panel3.Controls.Add((Control) this.btnReset);
      this.panel3.Controls.Add((Control) this.btnCompare);
      this.panel3.Dock = DockStyle.Bottom;
      this.panel3.Location = new Point(5, 600);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(874, 40);
      this.panel3.TabIndex = 22;
      this.btnPreferences.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnPreferences.Location = new Point(134, 9);
      this.btnPreferences.Name = "btnPreferences";
      this.btnPreferences.Size = new Size(75, 22);
      this.btnPreferences.TabIndex = 21;
      this.btnPreferences.Text = "&Preferences";
      this.btnPreferences.Click += new EventHandler(this.btnPreferences_Click);
      this.lnkHelp.BackColor = Color.Transparent;
      this.lnkHelp.Cursor = Cursors.Hand;
      this.lnkHelp.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lnkHelp.Location = new Point(5, 12);
      this.lnkHelp.Name = "lnkHelp";
      this.lnkHelp.Size = new Size(90, 17);
      this.lnkHelp.TabIndex = 20;
      this.lnkHelp.Help += new EventHandler(this.lnkHelp_Help);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.grpSource;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(351, 5);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 24;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.grpSource.Controls.Add((Control) this.tabControlFields);
      this.grpSource.Dock = DockStyle.Left;
      this.grpSource.HeaderForeColor = SystemColors.ControlText;
      this.grpSource.Location = new Point(5, 5);
      this.grpSource.Name = "grpSource";
      this.grpSource.Padding = new Padding(2, 2, 1, 1);
      this.grpSource.Size = new Size(346, 595);
      this.grpSource.TabIndex = 23;
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
      this.tabControlFields.Size = new Size(341, 565);
      this.tabControlFields.TabIndex = 17;
      this.tabControlFields.SelectedIndexChanged += new EventHandler(this.tabControlFields_SelectedIndexChanged);
      this.tabControlFields.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabFields.Controls.Add((Control) this.listViewSource);
      this.tabFields.Controls.Add((Control) this.findBtn);
      this.tabFields.Controls.Add((Control) this.textBoxFind);
      this.tabFields.Location = new Point(4, 23);
      this.tabFields.Name = "tabFields";
      this.tabFields.Size = new Size(333, 538);
      this.tabFields.TabIndex = 0;
      this.tabFields.Text = "Fields";
      this.tabFields.UseVisualStyleBackColor = true;
      this.listViewSource.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column1";
      gvColumn8.Text = "Field ID";
      gvColumn8.Width = 90;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column2";
      gvColumn9.Text = "Description";
      gvColumn9.Width = 166;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column3";
      gvColumn10.Text = "Type";
      gvColumn10.Width = 100;
      this.listViewSource.Columns.AddRange(new GVColumn[3]
      {
        gvColumn8,
        gvColumn9,
        gvColumn10
      });
      this.listViewSource.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewSource.Location = new Point(1, 35);
      this.listViewSource.Name = "listViewSource";
      this.listViewSource.Size = new Size(333, 460);
      this.listViewSource.TabIndex = 0;
      this.listViewSource.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.findBtn.Location = new Point(0, 7);
      this.findBtn.Name = "findBtn";
      this.findBtn.Size = new Size(64, 22);
      this.findBtn.TabIndex = 16;
      this.findBtn.Text = "Find";
      this.findBtn.Click += new EventHandler(this.findBtn_Click);
      this.textBoxFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxFind.Location = new Point(68, 8);
      this.textBoxFind.Name = "textBoxFind";
      this.textBoxFind.Size = new Size(258, 20);
      this.textBoxFind.TabIndex = 9;
      this.textBoxFind.TextChanged += new EventHandler(this.textBoxFind_TextChanged);
      this.tabLA.Controls.Add((Control) this.listViewMember);
      this.tabLA.Location = new Point(4, 22);
      this.tabLA.Name = "tabLA";
      this.tabLA.Size = new Size(333, 539);
      this.tabLA.TabIndex = 3;
      this.tabLA.Text = "Team Members";
      this.tabLA.UseVisualStyleBackColor = true;
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
      this.listViewMember.Columns.AddRange(new GVColumn[3]
      {
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.listViewMember.Dock = DockStyle.Fill;
      this.listViewMember.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewMember.Location = new Point(0, 0);
      this.listViewMember.Name = "listViewMember";
      this.listViewMember.Size = new Size(333, 539);
      this.listViewMember.TabIndex = 1;
      this.listViewMember.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabMilestones.Controls.Add((Control) this.listViewMilestone);
      this.tabMilestones.Location = new Point(4, 22);
      this.tabMilestones.Name = "tabMilestones";
      this.tabMilestones.Size = new Size(333, 539);
      this.tabMilestones.TabIndex = 4;
      this.tabMilestones.Text = "Milestones";
      this.tabMilestones.UseVisualStyleBackColor = true;
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
      this.listViewMilestone.Columns.AddRange(new GVColumn[3]
      {
        gvColumn14,
        gvColumn15,
        gvColumn16
      });
      this.listViewMilestone.Dock = DockStyle.Fill;
      this.listViewMilestone.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewMilestone.Location = new Point(0, 0);
      this.listViewMilestone.Name = "listViewMilestone";
      this.listViewMilestone.Size = new Size(333, 539);
      this.listViewMilestone.TabIndex = 1;
      this.listViewMilestone.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabDocuments.Controls.Add((Control) this.listViewDocument);
      this.tabDocuments.Location = new Point(4, 22);
      this.tabDocuments.Name = "tabDocuments";
      this.tabDocuments.Size = new Size(333, 539);
      this.tabDocuments.TabIndex = 2;
      this.tabDocuments.Text = "Documents";
      this.tabDocuments.UseVisualStyleBackColor = true;
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
      this.listViewDocument.Columns.AddRange(new GVColumn[3]
      {
        gvColumn17,
        gvColumn18,
        gvColumn19
      });
      this.listViewDocument.Dock = DockStyle.Fill;
      this.listViewDocument.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewDocument.Location = new Point(0, 0);
      this.listViewDocument.Name = "listViewDocument";
      this.listViewDocument.Size = new Size(333, 539);
      this.listViewDocument.TabIndex = 1;
      this.listViewDocument.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabTasks.Controls.Add((Control) this.listViewTask);
      this.tabTasks.Location = new Point(4, 22);
      this.tabTasks.Name = "tabTasks";
      this.tabTasks.Size = new Size(333, 539);
      this.tabTasks.TabIndex = 7;
      this.tabTasks.Text = "Tasks";
      this.tabTasks.UseVisualStyleBackColor = true;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "Column1";
      gvColumn20.Text = "Field ID";
      gvColumn20.Width = 118;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "Column2";
      gvColumn21.Text = "Description";
      gvColumn21.Width = 137;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "Column3";
      gvColumn22.Text = "Type";
      gvColumn22.Width = 100;
      this.listViewTask.Columns.AddRange(new GVColumn[3]
      {
        gvColumn20,
        gvColumn21,
        gvColumn22
      });
      this.listViewTask.Dock = DockStyle.Fill;
      this.listViewTask.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTask.Location = new Point(0, 0);
      this.listViewTask.Name = "listViewTask";
      this.listViewTask.Size = new Size(333, 539);
      this.listViewTask.TabIndex = 2;
      this.listViewTask.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabConditions.Controls.Add((Control) this.listViewCondition);
      this.tabConditions.Location = new Point(4, 22);
      this.tabConditions.Name = "tabConditions";
      this.tabConditions.Size = new Size(333, 539);
      this.tabConditions.TabIndex = 1;
      this.tabConditions.Text = "Conditions";
      this.tabConditions.UseVisualStyleBackColor = true;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "Column1";
      gvColumn23.Text = "Field ID";
      gvColumn23.Width = 103;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "Column2";
      gvColumn24.Text = "Description";
      gvColumn24.Width = 152;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "Column3";
      gvColumn25.Text = "Type";
      gvColumn25.Width = 100;
      this.listViewCondition.Columns.AddRange(new GVColumn[3]
      {
        gvColumn23,
        gvColumn24,
        gvColumn25
      });
      this.listViewCondition.Dock = DockStyle.Fill;
      this.listViewCondition.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewCondition.Location = new Point(0, 0);
      this.listViewCondition.Name = "listViewCondition";
      this.listViewCondition.Size = new Size(333, 539);
      this.listViewCondition.TabIndex = 1;
      this.listViewCondition.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPreliminary.Controls.Add((Control) this.listViewPreliminary);
      this.tabPreliminary.Location = new Point(4, 22);
      this.tabPreliminary.Name = "tabPreliminary";
      this.tabPreliminary.Size = new Size(333, 539);
      this.tabPreliminary.TabIndex = 12;
      this.tabPreliminary.Text = "Preliminary Conditions";
      this.tabPreliminary.UseVisualStyleBackColor = true;
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
      this.listViewPreliminary.Columns.AddRange(new GVColumn[3]
      {
        gvColumn26,
        gvColumn27,
        gvColumn28
      });
      this.listViewPreliminary.Dock = DockStyle.Fill;
      this.listViewPreliminary.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewPreliminary.Location = new Point(0, 0);
      this.listViewPreliminary.Name = "listViewPreliminary";
      this.listViewPreliminary.Size = new Size(333, 539);
      this.listViewPreliminary.TabIndex = 2;
      this.tabPost.Controls.Add((Control) this.listViewPost);
      this.tabPost.Location = new Point(4, 22);
      this.tabPost.Name = "tabPost";
      this.tabPost.Size = new Size(333, 539);
      this.tabPost.TabIndex = 5;
      this.tabPost.Text = "Post-Conditions";
      this.tabPost.UseVisualStyleBackColor = true;
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
      this.listViewPost.Columns.AddRange(new GVColumn[3]
      {
        gvColumn29,
        gvColumn30,
        gvColumn31
      });
      this.listViewPost.Dock = DockStyle.Fill;
      this.listViewPost.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewPost.Location = new Point(0, 0);
      this.listViewPost.Name = "listViewPost";
      this.listViewPost.Size = new Size(333, 539);
      this.listViewPost.TabIndex = 1;
      this.listViewPost.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageLock.Controls.Add((Control) this.listViewLocks);
      this.tabPageLock.Location = new Point(4, 22);
      this.tabPageLock.Name = "tabPageLock";
      this.tabPageLock.Size = new Size(333, 539);
      this.tabPageLock.TabIndex = 6;
      this.tabPageLock.Text = "Locks";
      this.tabPageLock.UseVisualStyleBackColor = true;
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
      this.listViewLocks.Columns.AddRange(new GVColumn[3]
      {
        gvColumn32,
        gvColumn33,
        gvColumn34
      });
      this.listViewLocks.Dock = DockStyle.Fill;
      this.listViewLocks.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewLocks.Location = new Point(0, 0);
      this.listViewLocks.Name = "listViewLocks";
      this.listViewLocks.Size = new Size(333, 539);
      this.listViewLocks.TabIndex = 2;
      this.listViewLocks.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageIS.Controls.Add((Control) this.listViewIS);
      this.tabPageIS.Location = new Point(4, 22);
      this.tabPageIS.Name = "tabPageIS";
      this.tabPageIS.Size = new Size(333, 539);
      this.tabPageIS.TabIndex = 8;
      this.tabPageIS.Text = "Interim Servicing";
      this.tabPageIS.UseVisualStyleBackColor = true;
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
      this.listViewIS.Columns.AddRange(new GVColumn[3]
      {
        gvColumn35,
        gvColumn36,
        gvColumn37
      });
      this.listViewIS.Dock = DockStyle.Fill;
      this.listViewIS.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewIS.Location = new Point(0, 0);
      this.listViewIS.Name = "listViewIS";
      this.listViewIS.Size = new Size(333, 539);
      this.listViewIS.TabIndex = 2;
      this.listViewIS.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageGFE.Controls.Add((Control) this.listViewGFE);
      this.tabPageGFE.Location = new Point(4, 22);
      this.tabPageGFE.Name = "tabPageGFE";
      this.tabPageGFE.Size = new Size(333, 539);
      this.tabPageGFE.TabIndex = 9;
      this.tabPageGFE.Text = "GFE Disclosure";
      this.tabPageGFE.UseVisualStyleBackColor = true;
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
      this.listViewGFE.Columns.AddRange(new GVColumn[3]
      {
        gvColumn38,
        gvColumn39,
        gvColumn40
      });
      this.listViewGFE.Dock = DockStyle.Fill;
      this.listViewGFE.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewGFE.Location = new Point(0, 0);
      this.listViewGFE.Name = "listViewGFE";
      this.listViewGFE.Size = new Size(333, 539);
      this.listViewGFE.TabIndex = 3;
      this.listViewGFE.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageTIL.Controls.Add((Control) this.listViewTIL);
      this.tabPageTIL.Location = new Point(4, 22);
      this.tabPageTIL.Name = "tabPageTIL";
      this.tabPageTIL.Size = new Size(333, 539);
      this.tabPageTIL.TabIndex = 10;
      this.tabPageTIL.Text = "TIL Disclosure";
      this.tabPageTIL.UseVisualStyleBackColor = true;
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
      this.listViewTIL.Columns.AddRange(new GVColumn[3]
      {
        gvColumn41,
        gvColumn42,
        gvColumn43
      });
      this.listViewTIL.Dock = DockStyle.Fill;
      this.listViewTIL.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewTIL.Location = new Point(0, 0);
      this.listViewTIL.Name = "listViewTIL";
      this.listViewTIL.Size = new Size(333, 539);
      this.listViewTIL.TabIndex = 3;
      this.listViewTIL.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageLECD.Controls.Add((Control) this.listViewLECD);
      this.tabPageLECD.Location = new Point(4, 22);
      this.tabPageLECD.Name = "tabPageLECD";
      this.tabPageLECD.Size = new Size(333, 539);
      this.tabPageLECD.TabIndex = 16;
      this.tabPageLECD.Text = "LE And CD Disclosure";
      this.tabPageLECD.UseVisualStyleBackColor = true;
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
      this.listViewLECD.Columns.AddRange(new GVColumn[3]
      {
        gvColumn44,
        gvColumn45,
        gvColumn46
      });
      this.listViewLECD.Dock = DockStyle.Fill;
      this.listViewLECD.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewLECD.Location = new Point(0, 0);
      this.listViewLECD.Name = "listViewLECD";
      this.listViewLECD.Size = new Size(333, 539);
      this.listViewLECD.TabIndex = 4;
      this.tabPageEDisclosures.Controls.Add((Control) this.listViewEDisclosures);
      this.tabPageEDisclosures.Location = new Point(4, 22);
      this.tabPageEDisclosures.Name = "tabPageEDisclosures";
      this.tabPageEDisclosures.Size = new Size(333, 539);
      this.tabPageEDisclosures.TabIndex = 15;
      this.tabPageEDisclosures.Text = "eDisclosures";
      this.tabPageEDisclosures.UseVisualStyleBackColor = true;
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
      this.listViewEDisclosures.Columns.AddRange(new GVColumn[3]
      {
        gvColumn47,
        gvColumn48,
        gvColumn49
      });
      this.listViewEDisclosures.Dock = DockStyle.Fill;
      this.listViewEDisclosures.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewEDisclosures.Location = new Point(0, 0);
      this.listViewEDisclosures.Name = "listViewEDisclosures";
      this.listViewEDisclosures.Size = new Size(333, 539);
      this.listViewEDisclosures.TabIndex = 1;
      this.listViewEDisclosures.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageAlerts.Controls.Add((Control) this.listViewAlerts);
      this.tabPageAlerts.Location = new Point(4, 22);
      this.tabPageAlerts.Name = "tabPageAlerts";
      this.tabPageAlerts.Size = new Size(333, 539);
      this.tabPageAlerts.TabIndex = 11;
      this.tabPageAlerts.Text = "Alerts";
      this.tabPageAlerts.UseVisualStyleBackColor = true;
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
      this.listViewAlerts.Columns.AddRange(new GVColumn[3]
      {
        gvColumn50,
        gvColumn51,
        gvColumn52
      });
      this.listViewAlerts.Dock = DockStyle.Fill;
      this.listViewAlerts.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewAlerts.Location = new Point(0, 0);
      this.listViewAlerts.Name = "listViewAlerts";
      this.listViewAlerts.Size = new Size(333, 539);
      this.listViewAlerts.TabIndex = 2;
      this.listViewAlerts.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.tabPageAUSTracking.Controls.Add((Control) this.listViewAUSTracking);
      this.tabPageAUSTracking.Location = new Point(4, 22);
      this.tabPageAUSTracking.Name = "tabPageAUSTracking";
      this.tabPageAUSTracking.Size = new Size(333, 539);
      this.tabPageAUSTracking.TabIndex = 14;
      this.tabPageAUSTracking.Text = "AUS Tracking";
      this.tabPageAUSTracking.UseVisualStyleBackColor = true;
      gvColumn53.ImageIndex = -1;
      gvColumn53.Name = "Column1";
      gvColumn53.Text = "Field ID";
      gvColumn53.Width = 118;
      gvColumn54.ImageIndex = -1;
      gvColumn54.Name = "Column2";
      gvColumn54.Text = "Description";
      gvColumn54.Width = 137;
      gvColumn55.ImageIndex = -1;
      gvColumn55.Name = "Column3";
      gvColumn55.Text = "Type";
      gvColumn55.Width = 100;
      this.listViewAUSTracking.Columns.AddRange(new GVColumn[3]
      {
        gvColumn53,
        gvColumn54,
        gvColumn55
      });
      this.listViewAUSTracking.Dock = DockStyle.Fill;
      this.listViewAUSTracking.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.listViewAUSTracking.Location = new Point(0, 0);
      this.listViewAUSTracking.Name = "listViewAUSTracking";
      this.listViewAUSTracking.Size = new Size(333, 539);
      this.listViewAUSTracking.TabIndex = 3;
      this.listViewAUSTracking.DoubleClick += new EventHandler(this.sourceFields_DoubleClick);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(884, 640);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.grpSource);
      this.Controls.Add((Control) this.panel3);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.Name = nameof (LoanXDBManager);
      this.Padding = new Padding(5, 5, 5, 0);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Reporting Database";
      this.Closing += new CancelEventHandler(this.LoanXDBManager_Closing);
      this.Load += new EventHandler(this.LoanXDBManager_Load);
      this.KeyDown += new KeyEventHandler(this.LoanXDBManager_KeyDown);
      this.panel1.ResumeLayout(false);
      this.grpSelected.ResumeLayout(false);
      this.flowLayoutPanel1.ResumeLayout(false);
      ((ISupportInitialize) this.exportBtn).EndInit();
      ((ISupportInitialize) this.editBtn).EndInit();
      ((ISupportInitialize) this.quickAddBtn).EndInit();
      this.panel3.ResumeLayout(false);
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

    private void initializeTargetFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "initializeTargetFields: Loading target fields");
      this.listViewTarget.Items.Clear();
      this.selectedFields = new Hashtable();
      this.tableList = Session.LoanManager.GetLoanXDBTableList(this.useERDB);
      if (this.tableList == null)
      {
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Error, nameof (LoanXDBManager), "initializeTargetFields: Can't load target field table");
      }
      else
      {
        this.xdbStatus = Session.LoanManager.GetLoanXDBStatus(this.useERDB);
        this.listViewTarget.BeginUpdate();
        for (int i1 = 0; i1 < this.tableList.TableCount; ++i1)
        {
          LoanXDBTable tableAt = this.tableList.GetTableAt(i1);
          if (tableAt != null)
          {
            for (int i2 = 0; i2 < tableAt.FieldCount; ++i2)
            {
              LoanXDBField fieldAt = tableAt.GetFieldAt(i2);
              if (fieldAt != null)
              {
                this.listViewTarget.Items.Add(this.createTargetFieldListItem(fieldAt));
                if (!this.selectedFields.ContainsKey((object) fieldAt.FieldID))
                  this.selectedFields.Add((object) fieldAt.FieldID, (object) "0");
                this.existingFields.Add(fieldAt.FieldID);
              }
            }
          }
        }
        this.listViewTarget.EndUpdate();
        if (!Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
          return;
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "initializeTargetFields: Loading target fields succeed");
      }
    }

    private void initializeSourceFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "initializeSourceFields: Loading source table...");
      this.listViewSource.Items.Clear();
      this.listViewSource.BeginUpdate();
      Dictionary<string, FieldDefinition> dictionary = new Dictionary<string, FieldDefinition>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      FieldDefinition[] sortedList1 = this.standardFields.AllFields.ToSortedList();
      for (int index = 0; index < sortedList1.Length; ++index)
      {
        if (sortedList1[index].AllowInReportingDatabase && (sortedList1[index].Category != FieldCategory.Common || !this.selectedFields.Contains((object) sortedList1[index].FieldID)) && sortedList1[index].AppliesToEdition(Session.EncompassEdition) && !CustomFieldInfo.IsCustomFieldID(sortedList1[index].FieldID))
        {
          this.addSourceField(this.listViewSource, sortedList1[index], false);
          dictionary[sortedList1[index].FieldID] = sortedList1[index];
        }
      }
      FieldDefinition[] sortedList2 = this.standardFields.VirtualFields.ToSortedList();
      for (int index = 0; index < sortedList2.Length; ++index)
      {
        if (sortedList2[index].AllowInReportingDatabase && !this.selectedFields.Contains((object) sortedList2[index].FieldID) && !dictionary.ContainsKey(sortedList2[index].FieldID) && sortedList2[index].AppliesToEdition(Session.EncompassEdition))
        {
          this.addSourceField(this.listViewSource, sortedList2[index], false);
          dictionary[sortedList2[index].FieldID] = sortedList2[index];
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
      if (!Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "initializeSourceFields: Loading source table succeed");
    }

    private void loadInterimServicingFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadInterimServicingFields: load Interim Servicing Fields setting");
      this.listViewIS.BeginUpdate();
      this.listViewIS.Items.Clear();
      foreach (FieldDefinition field in InterimServicingFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewIS, field, false);
      }
      this.listViewIS.EndUpdate();
    }

    private void loadEDisclosureFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadEDisclosureFields: load eDisclosure Fields setting");
      this.listViewEDisclosures.BeginUpdate();
      this.listViewEDisclosures.Items.Clear();
      foreach (FieldDefinition field in EDisclosureTrackingFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewEDisclosures, field, false);
      }
      foreach (FieldDefinition field in EDisclosure2015TrackingFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewEDisclosures, field, false);
      }
      this.listViewEDisclosures.EndUpdate();
    }

    private void loadGFEDisclosureFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadGFEDisclosureFields: load GFE Disclosure Fields setting");
      this.listViewGFE.BeginUpdate();
      this.listViewGFE.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedGFEFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewGFE, field, false);
      }
      this.listViewGFE.EndUpdate();
    }

    private void loadLECDDisclosureFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadLEDisclosureFields: load LE Disclosure Fields setting");
      this.listViewLECD.BeginUpdate();
      this.listViewLECD.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedLEFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewLECD, field, false);
      }
      foreach (FieldDefinition field in LastDisclosedCDFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewLECD, field, false);
      }
      this.listViewLECD.EndUpdate();
    }

    private void loadTILDisclosureFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadTILDisclosureFields: load TIL Disclosure Fields setting");
      this.listViewTIL.BeginUpdate();
      this.listViewTIL.Items.Clear();
      foreach (FieldDefinition field in LastDisclosedTILFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewTIL, field, false);
      }
      this.listViewTIL.EndUpdate();
    }

    private void loadAlertFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadAlertFields: load Alert field setting");
      this.listViewAlerts.BeginUpdate();
      this.listViewAlerts.Items.Clear();
      foreach (FieldDefinition field in AlertFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewAlerts, field, false);
      }
      this.listViewAlerts.EndUpdate();
    }

    private void loadAUSTrackingFields()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadAUSTrackingFields: load AUS Tracking field setting");
      this.listViewAUSTracking.BeginUpdate();
      this.listViewAUSTracking.Items.Clear();
      foreach (FieldDefinition field in AUSTrackingFields.All)
      {
        if (!this.selectedFields.Contains((object) field.FieldID))
          this.addSourceField(this.listViewAUSTracking, field, false);
      }
      this.listViewAUSTracking.EndUpdate();
    }

    private void loadLocks()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadLocks: Loading lock buyside/sellside/request setting");
      this.listViewLocks.BeginUpdate();
      this.listViewLocks.Items.Clear();
      foreach (FieldDefinition lockRequestField in EncompassFields.GetAllLockRequestFields(this.fieldSettings))
      {
        if (!this.selectedFields.Contains((object) lockRequestField.FieldID))
          this.addSourceField(this.listViewLocks, lockRequestField, false);
      }
      this.listViewLocks.EndUpdate();
      if (!Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadLocks: Loading lock buyside/sellside/request setting succeed");
    }

    private void loadCustomFields()
    {
      if (this.fieldSettings == null)
        return;
      try
      {
        foreach (CustomFieldInfo customField in this.fieldSettings.CustomFields)
        {
          if (!customField.IsEmpty() && !this.selectedFields.Contains((object) customField.FieldID))
            this.addSourceField(this.listViewSource, (FieldDefinition) new CustomField(customField), false);
        }
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Error in loading Custom Fields: " + (object) ex);
      }
    }

    private void loadCustomMilestone()
    {
      if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadCustomMilestone: Loading custom milestone setting");
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
      if (!Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
        return;
      Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "loadCustomMilestone: Loading custom milestone setting succeed");
    }

    private void loadDocumentTracking()
    {
      this.listViewDocument.BeginUpdate();
      this.listViewDocument.Items.Clear();
      foreach (FieldDefinition field in DocumentTrackingFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewDocument, field, false);
      }
      this.listViewDocument.EndUpdate();
    }

    private void loadTasks()
    {
      this.listViewTask.BeginUpdate();
      this.listViewTask.Items.Clear();
      foreach (FieldDefinition field in MilestoneTaskFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewTask, field, false);
      }
      this.listViewTask.EndUpdate();
    }

    private void loadPreliminaryCondition()
    {
      this.listViewPreliminary.BeginUpdate();
      this.listViewPreliminary.Items.Clear();
      foreach (FieldDefinition field in PreliminaryConditionFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewPreliminary, field, false);
      }
      this.listViewPreliminary.EndUpdate();
    }

    private void loadConditionTracking()
    {
      this.listViewCondition.BeginUpdate();
      this.listViewCondition.Items.Clear();
      foreach (FieldDefinition field in UnderwritingConditionFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewCondition, field, false);
      }
      this.listViewCondition.EndUpdate();
    }

    private void loadTeamMembers()
    {
      this.listViewMember.BeginUpdate();
      this.listViewMember.Items.Clear();
      foreach (FieldDefinition field in LoanAssociateFields.All)
      {
        if (field.AllowInReportingDatabase)
          this.addSourceField(this.listViewMember, field, false);
      }
      this.listViewMember.EndUpdate();
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

    private void addSourceField(GridView listViewObject, FieldDefinition field, bool selected)
    {
      string fieldId = field.FieldID;
      GVItem gvItem = new GVItem(fieldId)
      {
        SubItems = {
          [0] = {
            SortValue = !Utils.IsInt((object) fieldId) ? (object) ("B" + fieldId) : (object) ("A" + Utils.ParseInt((object) fieldId).ToString("000000"))
          }
        }
      };
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

    private void btnUpdate_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "Are you sure you want to update the Reporting Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      Hashtable hashtable = new Hashtable();
      string empty = string.Empty;
      for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
      {
        string str1 = this.listViewTarget.Items[nItemIndex].Text + "#" + (object) this.toPairNumber(this.listViewTarget.Items[nItemIndex].SubItems[1].Text);
        if (hashtable.ContainsKey((object) str1.ToLower()))
        {
          string str2 = "The field '" + this.listViewTarget.Items[nItemIndex].Text + "' ";
          if (this.listViewTarget.Items[nItemIndex].SubItems[1].Text.Trim() != "")
            str2 = str2 + "for " + this.listViewTarget.Items[nItemIndex].SubItems[1].Text.Trim() + " borrower pair ";
          int num = (int) Utils.Dialog((IWin32Window) this, str2 + "is duplicated in the list. Either change the position of the borrower pair or remove the field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.listViewTarget.Items[nItemIndex].Selected = true;
          return;
        }
        hashtable.Add((object) str1.ToLower(), (object) "");
      }
      for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
      {
        if (((LoanXDBField) this.listViewTarget.Items[nItemIndex].Tag).FieldSizeToInteger > 8000)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The Field Size specified for one or more of the fields you have added exceeds 8,000 characters. To proceed, you must update the Field Size for these fields. The Field Size must not exceed 8,000.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
        }
      }
      this.Cursor = Cursors.WaitCursor;
      LoanXDBField[] loanXdbFieldArray = (LoanXDBField[]) null;
      try
      {
        if (!this.updateReportingTableList())
          return;
        loanXdbFieldArray = this.tableList.GetUpdateList();
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
      if (loanXdbFieldArray == null || loanXdbFieldArray.Length == 0)
      {
        string text;
        if (this.xdbStatus.UpdatesPending)
        {
          try
          {
            this.validateUpdatePendingChanges();
          }
          catch (Exception ex)
          {
            int num = (int) MessageBox.Show("Error updating Reporting Database: " + ex.Message, "Reporting Database Tool");
          }
          text = "Do you want to close Reporting Database Tool?";
        }
        else
          text = "No changes have been made to the Reporting Database field list. Do you want to close Reporting Database Tool?";
        if (Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.Close();
      }
      else
      {
        bool flag = false;
        List<string> collection = new List<string>();
        this.newFields.Clear();
        if (!this.useERDB)
        {
          foreach (LoanXDBField loanXdbField in loanXdbFieldArray)
          {
            if (loanXdbField.FieldCurrentStatus == LoanXDBField.FieldStatus.New)
            {
              if (loanXdbField.ComortgagorPair <= 1)
                this.newFields.Add(loanXdbField.FieldID);
              else
                this.newFields.Add(loanXdbField.FieldID + "#" + (object) loanXdbField.ComortgagorPair);
              if (loanXdbField.Auditable)
              {
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedBy");
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedByFirstName");
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedByLastName");
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedDate");
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedValue");
                collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".PreviousValue");
              }
            }
            else if (loanXdbField.FieldCurrentStatus == LoanXDBField.FieldStatus.Updated && loanXdbField.AuditIsChanged && loanXdbField.Auditable)
            {
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedBy");
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedByFirstName");
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedByLastName");
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedDate");
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".ModifiedValue");
              collection.Add("AuditTrail." + loanXdbField.FieldID + (loanXdbField.ComortgagorPair > 1 ? "#" + (object) loanXdbField.ComortgagorPair : "") + ".PreviousValue");
            }
          }
          if ((this.newFields != null && this.newFields.Count > 0 || collection != null && collection.Count > 0) && Utils.Dialog((IWin32Window) this, "Should the newly added/updated fields be made accessible to all personas?", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            flag = true;
        }
        using (ProgressDialog progressDialog = new ProgressDialog((this.useERDB ? "External " : "") + "Reporting Database", new AsynchronousProcess(this.performDatabaseUpdate), (object) null, false))
        {
          if (progressDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
        }
        if (flag)
        {
          try
          {
            List<string> stringList = new List<string>();
            stringList.AddRange((IEnumerable<string>) this.newFields);
            if (collection != null && collection.Count > 0)
              stringList.AddRange((IEnumerable<string>) collection);
            ((FieldAccessAclManager) Session.ACL.GetAclManager(AclCategory.FieldAccess)).AddFieldPermissionToAllPersonas(stringList.ToArray());
          }
          catch (Exception ex)
          {
            Tracing.Log(LoanXDBManager.sw, TraceLevel.Error, nameof (LoanXDBManager), "btnUpdate_Click: Cannot update field permission in all personas! " + ex.Message);
          }
        }
        if (Tracing.IsSwitchActive(LoanXDBManager.sw, TraceLevel.Verbose))
          Tracing.Log(LoanXDBManager.sw, TraceLevel.Verbose, nameof (LoanXDBManager), "btnUpdate_Click: Trying to perform SetLoanXDBTableList API");
        try
        {
          if (this.tableList.GetFieldsRequiringRebuild().Length != 0)
          {
            if (Utils.Dialog((IWin32Window) this, "The Reporting Database has been updated. Would you like to populate it with the current loan data now?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
              this.populateReportingData(true);
          }
          else
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Reporting Database has been updated successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(LoanXDBManager.sw, TraceLevel.Error, nameof (LoanXDBManager), "btnUpdate_Click: Can't change reporting database status to \"Changed\", Error: " + ex.Message);
        }
        this.isDirty = false;
        if (Utils.Dialog((IWin32Window) this, "Do you want to close Reporting Database Tool?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
          return;
        this.Close();
      }
    }

    private bool updateReportingTableList()
    {
      try
      {
        this.tableList = Session.LoanManager.GetLoanXDBTableList(this.useERDB);
        this.xdbStatus = Session.LoanManager.GetLoanXDBStatus(this.useERDB);
        Hashtable fields = this.tableList.GetFields();
        Hashtable hashtable = new Hashtable();
        for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
        {
          LoanXDBField tag = (LoanXDBField) this.listViewTarget.Items[nItemIndex].Tag;
          if (!hashtable.ContainsKey((object) tag.FieldIDWithCoMortgagor))
            hashtable.Add((object) tag.FieldIDWithCoMortgagor, (object) tag);
          if (!fields.ContainsKey((object) tag.FieldIDWithCoMortgagor))
          {
            tag.TableName = string.Empty;
            this.tableList.AddUpdateList(tag, LoanXDBField.FieldStatus.New);
          }
          else
          {
            LoanXDBField dbField = (LoanXDBField) fields[(object) tag.FieldIDWithCoMortgagor];
            tag.TableName = dbField.TableName;
            if (tag.FieldSizeToInteger != dbField.FieldSizeToInteger)
            {
              dbField.FieldSize = tag.FieldSizeToInteger;
              dbField.SizeIsChanged = true;
              dbField.FieldCurrentStatus = LoanXDBField.FieldStatus.Updated;
            }
            if (tag.UseIndex != dbField.UseIndex)
            {
              dbField.UseIndex = tag.UseIndex;
              dbField.IndexIsChanged = true;
            }
            if (tag.Description != dbField.Description)
            {
              dbField.Description = tag.Description;
              dbField.DescriptionIsChanged = true;
            }
            if (tag.Auditable != dbField.Auditable)
            {
              dbField.Auditable = tag.Auditable;
              dbField.AuditIsChanged = true;
            }
            if (dbField.SizeIsChanged || dbField.IndexIsChanged || dbField.AuditIsChanged || dbField.DescriptionIsChanged)
              this.tableList.AddUpdateList(dbField, LoanXDBField.FieldStatus.Updated);
          }
        }
        ArrayList arrayList = new ArrayList();
        foreach (DictionaryEntry dictionaryEntry in fields)
        {
          LoanXDBField loanXdbField = (LoanXDBField) dictionaryEntry.Value;
          if (!hashtable.ContainsKey((object) loanXdbField.FieldIDWithCoMortgagor))
            arrayList.Add((object) loanXdbField);
        }
        for (int index = 0; index < arrayList.Count; ++index)
          this.tableList.AddUpdateList((LoanXDBField) arrayList[index], LoanXDBField.FieldStatus.Removed);
        this.tableList.UpdateTable();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The reporting database cannot be updated. Error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
      return true;
    }

    private bool populateReportingData(bool pendingFieldsOnly, bool promptForUpdateAllLoans = false)
    {
      bool updateAllLoans = true;
      if (promptForUpdateAllLoans)
        updateAllLoans = MessageBox.Show("Do you want to continue where the last update stopped?\r\n\r\nClick \"No\" to update all loans.", "Reporting Database", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No;
      LoanXDBManager.RebuildDatabaseAsyncParam state = new LoanXDBManager.RebuildDatabaseAsyncParam(pendingFieldsOnly, updateAllLoans);
      if (LoanXDBManager.rebuildReportingDbThreadCount <= 0)
        LoanXDBManager.rebuildReportingDbThreadCount = (int) Session.ServerManager.GetServerSetting("Internal.RebuildReportingDbThreadCount");
      using (ProgressDialog2 progressDialog2 = new ProgressDialog2("Rebuild Pipeline", new AsynchronousProcess2(this.rebuildDatabaseAsync), (object) state, true, LoanXDBManager.rebuildReportingDbThreadCount))
      {
        switch (progressDialog2.ShowDialog((IWin32Window) this))
        {
          case DialogResult.OK:
            int num1 = (int) Utils.Dialog((IWin32Window) this, "The Reporting Database has been successfully populated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return true;
          case DialogResult.Cancel:
            int num2 = (int) Utils.Dialog((IWin32Window) this, "The reporting data population process has been canceled. Reporting will be incomplete/inaccurate until the database is rebuilt.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            break;
          default:
            int num3 = (int) Utils.Dialog((IWin32Window) this, "The reporting data population process has been failed! Reporting will be incomplete/inaccurate until the database is rebuilt.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            break;
        }
        return false;
      }
    }

    private DialogResult rebuildDatabaseAsync(object state, IProgressFeedback2 feedback)
    {
      try
      {
        DialogResult dialogResult = DialogResult.OK;
        LoanXDBManager.RebuildDatabaseAsyncParam databaseAsyncParam = (LoanXDBManager.RebuildDatabaseAsyncParam) state;
        Session.LoanManager.RebuildReportingDb(this.useERDB, databaseAsyncParam.PendingFieldsOnly, (IServerProgressFeedback2) feedback, databaseAsyncParam.UpdateAllLoans);
        if (feedback == null)
          return dialogResult;
        return feedback.Cancel ? DialogResult.Cancel : DialogResult.OK;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error rebuilding reporting database: " + ex.Message);
        Tracing.Log(LoanXDBManager.sw, TraceLevel.Error, nameof (LoanXDBManager), "rebuildPipeline: " + ex.Message);
        return DialogResult.Abort;
      }
    }

    private void addBtn_Click(object sender, EventArgs e)
    {
      if (!this.addBtn.Enabled)
        return;
      if (this.tabControlFields.SelectedTab == this.tabFields)
        this.addFieldsToTargetList(this.listViewSource);
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
      {
        this.addFieldsToTargetList(this.listViewAUSTracking);
      }
      else
      {
        if (this.tabControlFields.SelectedTab != this.tabPageEDisclosures)
          return;
        this.addFieldsToTargetList(this.listViewEDisclosures);
      }
    }

    public void addFieldsToTargetList(
      GridView sourceListView,
      string fieldInstanceSpecifier,
      bool clearTargetSelect,
      bool isSortTargetList)
    {
      this.addFieldsToTargetList(sourceListView, fieldInstanceSpecifier, clearTargetSelect, isSortTargetList, (Dictionary<string, List<FieldPairInfo>>) null);
    }

    public void addFieldsToTargetList(
      GridView sourceListView,
      string fieldInstanceSpecifier,
      bool clearTargetSelect,
      bool isSortTargetList,
      Dictionary<string, List<FieldPairInfo>> fieldPairMap)
    {
      this.lastAddedTargetGridViewItem = (GVItem) null;
      if (sourceListView.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a source field first.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.isDirty = true;
        bool flag1 = false;
        int num2 = 0;
        if (clearTargetSelect && this.listViewTarget.SelectedItems.Count > 0)
        {
          num2 = this.listViewTarget.SelectedItems[this.listViewTarget.SelectedItems.Count - 1].Index;
          flag1 = true;
          this.listViewTarget.SelectedItems.Clear();
        }
        if (!this.useERDB && this.showFieldCountWarning && this.listViewTarget.Items.Count + sourceListView.SelectedItems.Count > 1500)
        {
          this.showFieldCountWarning = false;
          if (Utils.Dialog((IWin32Window) this, "Warning: You are about to exceed the recommended maximum of " + (object) 1500 + " fields in the Reporting Database. Adding fields beyond this limit can result in significant performance degradation." + Environment.NewLine + Environment.NewLine + "Are you sure you want to add the selected field(s) to the Reporting Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
        }
        FieldInstanceSpecifierType specifierType = FieldInstanceSpecifierType.None;
        bool flag2 = false;
        bool flag3 = false;
        ArrayList arrayList = new ArrayList();
        foreach (GVItem selectedItem in sourceListView.SelectedItems)
        {
          FieldDefinition tag = (FieldDefinition) selectedItem.Tag;
          if (tag.FieldID.ToLower() == "log.ms.duration")
            flag2 = true;
          if (tag.MaxLength > 8000)
            flag3 = true;
          arrayList.Add((object) selectedItem);
          if (tag.InstanceSpecifierType != FieldInstanceSpecifierType.None)
            specifierType = tag.InstanceSpecifierType;
        }
        if (flag3)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "The Field Size specified for the selected field(s) is more than 8,000. Data entered in these fields that exceeds 8,000 characters will be truncated in your reports! To avoid this truncation, update the field's Field Size to 8,000 or lower.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        string instanceSpecifier = fieldInstanceSpecifier;
        if (string.IsNullOrEmpty(instanceSpecifier) && specifierType != FieldInstanceSpecifierType.None)
        {
          string selectedFieldID = flag2 ? "log.ms.duration" : "";
          using (InstanceSelectorDialog instanceSelectorDialog = new InstanceSelectorDialog(specifierType, selectedFieldID))
          {
            if (instanceSelectorDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
              return;
            if (instanceSelectorDialog.SelectedInstance != "")
              instanceSpecifier = instanceSelectorDialog.SelectedInstance;
          }
        }
        this.Cursor = Cursors.WaitCursor;
        this.listViewTarget.BeginUpdate();
        GVItem gvItem1 = (GVItem) null;
        foreach (GVItem gvItem2 in arrayList)
        {
          FieldDefinition fieldDef = (FieldDefinition) gvItem2.Tag;
          if (instanceSpecifier != null && fieldDef.InstanceSpecifierType != FieldInstanceSpecifierType.None)
            fieldDef = fieldDef.CreateInstance((object) instanceSpecifier);
          List<FieldPairInfo> fieldPairInfoList = (List<FieldPairInfo>) null;
          if (fieldPairMap != null && fieldPairMap.ContainsKey(fieldDef.FieldID))
            fieldPairInfoList = fieldPairMap[fieldDef.FieldID];
          int num4 = fieldPairInfoList == null || fieldPairInfoList.Count <= 1 ? 1 : fieldPairInfoList.Count;
          for (int index = 0; index < num4; ++index)
          {
            LoanXDBField dbField = new LoanXDBField(fieldDef);
            if (dbField.FieldID == "663")
              dbField.FieldSize = 15;
            if (fieldPairMap != null && fieldPairMap.ContainsKey(fieldDef.FieldID) && (fieldDef.Category == FieldCategory.Borrower || fieldDef.Category == FieldCategory.Coborrower))
            {
              List<FieldPairInfo> fieldPair = fieldPairMap[fieldDef.FieldID];
              if (fieldPair != null && fieldPair.Count >= num4 && fieldPair[index].PairIndex > 1)
                dbField.ComortgagorPair = fieldPair[index].PairIndex;
            }
            if (this.tabControlFields.SelectedTab != this.tabPageIS && this.tabControlFields.SelectedTab != this.tabPageLock && this.tabControlFields.SelectedTab != this.tabFields && this.hasDuplicateField(dbField))
            {
              int num5 = (int) Utils.Dialog((IWin32Window) this, "The reporting field list already contains field '" + dbField.FieldID + "'.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
              GVItem targetFieldListItem = this.createTargetFieldListItem(dbField);
              if (gvItem1 == null)
                gvItem1 = targetFieldListItem;
              if (!flag1)
              {
                this.listViewTarget.Items.Add(targetFieldListItem);
              }
              else
              {
                if (num2 + 1 > this.listViewTarget.Items.Count)
                  this.listViewTarget.Items.Add(targetFieldListItem);
                else
                  this.listViewTarget.Items.Insert(num2 + 1, targetFieldListItem);
                ++num2;
              }
              targetFieldListItem.Selected = true;
              this.lastAddedTargetGridViewItem = targetFieldListItem;
            }
          }
        }
        if (this.tabControlFields.SelectedTab == this.tabFields)
        {
          foreach (GVItem gvItem3 in arrayList)
          {
            FieldDefinition field = EncompassFields.GetField(gvItem3.Text, this.fieldSettings);
            if (gvItem3.Text.ToLower().StartsWith("cx.") || gvItem3.Text.ToLower() == "loanfolder" || gvItem3.Text.ToLower() == "loanlastmodified" || field != null && field.Category == FieldCategory.Common && !field.MultiInstance && field.InstanceSpecifierType != FieldInstanceSpecifierType.Index)
              this.listViewSource.Items.Remove(gvItem3);
          }
        }
        else if (this.tabControlFields.SelectedTab == this.tabPageEDisclosures)
        {
          foreach (GVItem gvItem4 in arrayList)
          {
            FieldDefinition field = EncompassFields.GetField(gvItem4.Text, this.fieldSettings);
            if (string.Compare(field.FieldID, "EDISCLOSEDTRK.DisclosureCount", true) == 0)
              this.listViewEDisclosures.Items.Remove(gvItem4);
            if (string.Compare(field.FieldID, "EDISCLOSED2015TRK.DisclosureCount", true) == 0)
              this.listViewEDisclosures.Items.Remove(gvItem4);
          }
        }
        else if (this.tabControlFields.SelectedTab == this.tabPageLock || this.tabControlFields.SelectedTab == this.tabPageIS)
        {
          GridView gridView = this.tabControlFields.SelectedTab != this.tabPageLock ? (this.tabControlFields.SelectedTab != this.tabPageIS ? this.listViewAUSTracking : this.listViewIS) : this.listViewLocks;
          foreach (GVItem gvItem5 in arrayList)
          {
            FieldDefinition fieldDefinition = EncompassFields.GetField(gvItem5.Text, this.fieldSettings);
            if (fieldDefinition == null && this.standardFields.VirtualFields.Contains(gvItem5.Text))
              fieldDefinition = this.standardFields.VirtualFields[gvItem5.Text];
            if (fieldDefinition != null)
              gridView.Items.Remove(gvItem5);
          }
        }
        this.listViewTarget.EndUpdate();
        if (isSortTargetList)
          this.listViewTarget.ReSort();
        this.updateFieldCounter();
        if (gvItem1 != null)
          this.listViewTarget.EnsureVisible(gvItem1.Index);
        this.Cursor = Cursors.Default;
      }
    }

    private void addFieldsToTargetList(GridView sourceListView)
    {
      this.addFieldsToTargetList(sourceListView, (string) null, true, true);
    }

    private bool hasDuplicateField(LoanXDBField dbField)
    {
      for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
      {
        LoanXDBField tag = (LoanXDBField) this.listViewTarget.Items[nItemIndex].Tag;
        if (tag.FieldID == dbField.FieldID && tag.ComortgagorPair == dbField.ComortgagorPair)
          return true;
      }
      return false;
    }

    private GVItem getTargetListItemByID(string fieldId)
    {
      foreach (GVItem targetListItemById in (IEnumerable<GVItem>) this.listViewTarget.Items)
      {
        if (string.Compare(((LoanXDBField) targetListItemById.Tag).FieldID, fieldId, true) == 0)
          return targetListItemById;
      }
      return (GVItem) null;
    }

    private GVItem createTargetFieldListItem(LoanXDBField dbField)
    {
      string fieldId = dbField.FieldID;
      string description = dbField.Description;
      FieldDefinition fieldDefinition = EncompassFields.GetField(dbField.FieldID, this.fieldSettings);
      if (fieldDefinition != null)
        fieldId = fieldDefinition.FieldID;
      else if (this.standardFields.VirtualFields.Contains(dbField.FieldID))
      {
        fieldDefinition = this.standardFields.VirtualFields[dbField.FieldID];
        description = fieldDefinition.Description;
      }
      GVItem targetFieldListItem = new GVItem(fieldId);
      targetFieldListItem.SubItems[0].SortValue = !Utils.IsInt((object) fieldId) ? (object) ("B" + fieldId) : (object) ("A" + Utils.ParseInt((object) fieldId).ToString("000000"));
      int pair = dbField.ComortgagorPair;
      if (pair < 1)
        pair = 1;
      if (dbField.FieldID.ToLower().StartsWith("cx.") || fieldDefinition != null && fieldDefinition.Category == FieldCategory.Common)
        targetFieldListItem.SubItems.Add((object) "");
      else
        targetFieldListItem.SubItems.Add((object) LoanXDBManager.ToPairOrder(pair));
      targetFieldListItem.SubItems.Add((object) description);
      if (dbField.FieldType == LoanXDBTableList.TableTypes.IsNumeric)
      {
        targetFieldListItem.SubItems.Add((object) "Numeric");
        targetFieldListItem.SubItems.Add((object) "13");
      }
      else if (dbField.FieldType == LoanXDBTableList.TableTypes.IsDate)
      {
        targetFieldListItem.SubItems.Add((object) "Date");
        targetFieldListItem.SubItems.Add((object) "4");
      }
      else
      {
        targetFieldListItem.SubItems.Add((object) "String");
        targetFieldListItem.SubItems.Add((object) dbField.FieldSizeToString);
      }
      targetFieldListItem.SubItems.Add(dbField.UseIndex ? (object) "Y" : (object) "N");
      if (!this.useERDB)
        targetFieldListItem.SubItems.Add(dbField.Auditable ? (object) "Yes" : (object) "No");
      targetFieldListItem.Tag = (object) dbField;
      return targetFieldListItem;
    }

    private void removeBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewTarget.SelectedItems.Count == 0)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must select a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        if (this.listViewTarget.SelectedItems.Count == 1)
        {
          if (((LoanXDBField) this.listViewTarget.SelectedItems[0].Tag).Auditable)
          {
            int num2 = (int) Utils.Dialog((IWin32Window) this, "This field is auditable and cannot be removed from the Reporting Database.  Click Edit to clear the option to include this field in the Audit Trail before proceeding.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return;
          }
        }
        else if (this.listViewTarget.SelectedItems != null && this.listViewTarget.SelectedItems.Count > 1)
        {
          foreach (GVItem selectedItem in this.listViewTarget.SelectedItems)
          {
            if (((LoanXDBField) selectedItem.Tag).Auditable)
            {
              int num3 = (int) Utils.Dialog((IWin32Window) this, "One or more fields are auditable and cannot be removed from the Reporting Database.  Click Edit to clear the option to include this field in the Audit Trail for each of the fields before proceeding.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              return;
            }
          }
        }
        this.isDirty = true;
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
          LoanXDBField tag = (LoanXDBField) gvItem.Tag;
          FieldDefinition field = EncompassFields.GetField(tag.FieldID, this.fieldSettings);
          if (field == null && this.standardFields.VirtualFields.Contains(tag.FieldID))
            field = this.standardFields.VirtualFields[tag.FieldID];
          if (tag.FieldID.ToLower().StartsWith("ispay"))
            this.addSourceField(this.listViewIS, field, true);
          else if (tag.FieldID.ToLower().StartsWith("disclosedgfe"))
            this.addSourceField(this.listViewGFE, field, true);
          else if (!tag.FieldID.ToLower().StartsWith("disclosedtil") && !tag.FieldID.ToLower().StartsWith("austracking."))
          {
            if (field != null && (LockRequestCustomField.IsLockRequestCustomField(tag.FieldID) || RateLockField.IsRateLockField(tag.FieldID) || tag.FieldID.ToLower() == "ms.lockdays"))
              this.addSourceField(this.listViewLocks, field, true);
            else if (string.Compare(tag.FieldID, "EDISCLOSEDTRK.DisclosureCount", true) == 0)
              this.addSourceField(this.listViewEDisclosures, field, true);
            else if (field != null && field.Category == FieldCategory.Common && !field.MultiInstance)
              this.addSourceField(this.listViewSource, field, true);
          }
        }
        this.updateFieldCounter();
        this.Cursor = Cursors.Default;
      }
    }

    private void getCustomFieldType(LoanXDBField dbField)
    {
      try
      {
        CustomField customField = new CustomField(this.fieldSettings.CustomFields.GetField(dbField.FieldID));
        int num = 64;
        if (customField.MaxLength > 0)
          num = customField.MaxLength;
        LoanXDBTableList.TableTypes tableTypes = LoanXDBTableList.TableTypes.IsString;
        if (customField.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Numeric)
        {
          tableTypes = LoanXDBTableList.TableTypes.IsNumeric;
          num = 9;
        }
        else if (customField.ReportingDatabaseColumnType == ReportingDatabaseColumnType.Date || customField.ReportingDatabaseColumnType == ReportingDatabaseColumnType.DateTime)
        {
          tableTypes = LoanXDBTableList.TableTypes.IsDate;
          num = 4;
        }
        dbField.FieldType = tableTypes;
        dbField.FieldSize = num;
      }
      catch (Exception ex)
      {
      }
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

    private void editBtn_Click(object sender, EventArgs e)
    {
      if (this.listViewTarget.SelectedItems.Count == 0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must select a field.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.isDirty = true;
        int index = this.listViewTarget.SelectedItems[0].Index;
        using (LoanXDBFieldDialog loanXdbFieldDialog = new LoanXDBFieldDialog((LoanXDBField) this.listViewTarget.SelectedItems[0].Tag, this.useERDB))
        {
          if (loanXdbFieldDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.listViewTarget.Items[index] = this.createTargetFieldListItem(loanXdbFieldDialog.DBField);
          this.listViewTarget.Items[index].Selected = true;
        }
      }
    }

    private void listViewTarget_DoubleClick(object sender, EventArgs e)
    {
      this.editBtn_Click((object) null, (EventArgs) null);
    }

    private void btnReset_Click(object sender, EventArgs e)
    {
      if (sender != null && Utils.Dialog((IWin32Window) null, "Are you sure you want to cancel the changes that you just did?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.fieldSettings = Session.LoanManager.GetFieldSettings();
      this.initializeTargetFields();
      this.initializeSourceFields();
      this.updateFieldCounter();
      this.isDirty = false;
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

    private void btnCompare_Click(object sender, EventArgs e)
    {
      int num = (int) new LoanXDBCompareDialog(this.useERDB, this.listViewTarget).ShowDialog((IWin32Window) this);
    }

    private DialogResult performDatabaseUpdate(object state, IProgressFeedback feedback)
    {
      if (feedback != null)
        feedback.Status = "Applying database updates...";
      try
      {
        Session.LoanManager.ApplyReportingDatabaseChanges(this.useERDB, this.tableList, this.xdbStatus.ValidationKey);
      }
      catch (LockException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Reporting Database is locked by another process and cannot be updated at this time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return DialogResult.Abort;
      }
      catch (ObjectModifiedException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Reporting Database has been modified by another user. You should exit the application and restart to ensure your changes are not in conflict with those already made.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return DialogResult.Abort;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Update failed due to error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return DialogResult.Abort;
      }
      return DialogResult.OK;
    }

    private void LoanXDBManager_Closing(object sender, CancelEventArgs e)
    {
      if (!this.isDirty || Utils.Dialog((IWin32Window) this, "You have modified the field list without updating Reporting Database. Are you sure you want to cancel it?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != DialogResult.No)
        return;
      e.Cancel = true;
    }

    private void findBtn_Click(object sender, EventArgs e)
    {
      this.findField(this.listViewSource);
      this.findField(this.listViewTarget);
    }

    private void btnCreate_Click(object sender, EventArgs e)
    {
      if (this.tableList == null)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Encompass can't find your reporting database table.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        if (Utils.Dialog((IWin32Window) this, "Are you sure you want to recreate Reporting Database?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
          return;
        this.keepTables = false;
        using (LoanXDBRecreateOptionsDialog recreateOptionsDialog = new LoanXDBRecreateOptionsDialog())
        {
          if (recreateOptionsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          this.keepTables = recreateOptionsDialog.KeepTables;
        }
        Cursor.Current = Cursors.WaitCursor;
        using (ProgressDialog progressDialog = new ProgressDialog("Reporting Database", new AsynchronousProcess(this.recreateDatabase), (object) null, false))
        {
          if (progressDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
        }
        this.populateReportingData(false);
        Cursor.Current = Cursors.Default;
        this.Close();
      }
    }

    private void btnPopulate_Click(object sender, EventArgs e)
    {
      bool pendingFieldsOnly = true;
      using (LoanXDBPopulateOptionsDialog populateOptionsDialog = new LoanXDBPopulateOptionsDialog())
      {
        if (populateOptionsDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        pendingFieldsOnly = populateOptionsDialog.PendingFieldsOnly;
      }
      this.populateReportingData(pendingFieldsOnly, true);
    }

    private DialogResult recreateDatabase(object state, IProgressFeedback feedback)
    {
      try
      {
        this.xdbStatus = Session.LoanManager.GetLoanXDBStatus(this.useERDB);
        Session.LoanManager.ResetReportingDatabase(this.useERDB, this.tableList, this.xdbStatus.ValidationKey, this.keepTables, (IServerProgressFeedback) feedback);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Update failed: " + ex.Message);
        return DialogResult.Abort;
      }
      return DialogResult.OK;
    }

    private void tabControlFields_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.updateFieldCounter();
    }

    private void quickAddBtn_Click(object sender, EventArgs e)
    {
      this.tabControlFields.SelectedTab = this.tabFields;
      this.quickTables = CollectionsUtil.CreateCaseInsensitiveHashtable();
      this.quickTablesTarget = CollectionsUtil.CreateCaseInsensitiveHashtable();
      for (int nItemIndex = 0; nItemIndex < this.listViewSource.Items.Count; ++nItemIndex)
      {
        if (!this.quickTables.ContainsKey((object) this.listViewSource.Items[nItemIndex].Text))
          this.quickTables.Add((object) this.listViewSource.Items[nItemIndex].Text, (object) this.listViewSource.Items[nItemIndex]);
      }
      for (int nItemIndex = 0; nItemIndex < this.listViewTarget.Items.Count; ++nItemIndex)
      {
        if (!this.quickTablesTarget.ContainsKey((object) (this.listViewTarget.Items[nItemIndex].Text + "-" + this.listViewTarget.Items[nItemIndex].SubItems[1].Text)))
          this.quickTablesTarget.Add((object) (this.listViewTarget.Items[nItemIndex].Text + "-" + this.listViewTarget.Items[nItemIndex].SubItems[1].Text), (object) 1);
      }
      using (AddFields sender1 = new AddFields(Session.DefaultInstance))
      {
        sender1.ImportButtonEnabled = true;
        sender1.OnAddMoreButtonClick += new EventHandler(this.addFieldDlg_OnAddMoreButtonClick);
        sender1.OnImportButtonClick += new EventHandler(this.addFieldDlg_OnImportButtonClick);
        if (sender1.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        if (sender1.IsAddedByImport)
          this.addFieldDlg_OnImportButtonClick((object) sender1, new EventArgs());
        else
          this.addFieldDlg_OnAddMoreButtonClick((object) sender1, new EventArgs());
      }
    }

    private void exportBtn_Click(object sender, EventArgs e)
    {
      string str1 = string.Empty;
      string str2 = string.Empty;
      using (ExportToLocalDialog exportToLocalDialog = new ExportToLocalDialog("Export Fields", "Fields-" + DateTime.Now.ToString("MMddyyy") + ".txt"))
      {
        if (exportToLocalDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        str1 = exportToLocalDialog.SelectedFolder;
        str2 = exportToLocalDialog.SelectedFileName;
      }
      AddFields.ImportableLoanXDBFieldTable loanXdbFieldTable1 = new AddFields.ImportableLoanXDBFieldTable(new ImportableLoanXDBFieldName[7]
      {
        ImportableLoanXDBFieldName.FieldID,
        ImportableLoanXDBFieldName.BorrowerPair,
        ImportableLoanXDBFieldName.InstanceIndex,
        ImportableLoanXDBFieldName.FieldSize,
        ImportableLoanXDBFieldName.IncludeInAuditTrail,
        ImportableLoanXDBFieldName.UseIndexForThisField,
        ImportableLoanXDBFieldName.Description
      });
      foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewTarget.Items)
      {
        LoanXDBField tag = (LoanXDBField) gvItem.Tag;
        FieldDefinition field = EncompassFields.GetField(tag.FieldID, this.fieldSettings);
        string str3 = tag.FieldID;
        string str4 = tag.ComortgagorPair.ToString();
        string str5 = "";
        string str6 = "";
        if (field != null && field.InstanceSpecifier != null)
        {
          str5 = field.InstanceSpecifier.ToString();
          if (tag.FieldID.LastIndexOf("." + str5) > 0)
            str3 = tag.FieldID.Substring(0, tag.FieldID.LastIndexOf("." + str5));
        }
        if (tag.FieldType == LoanXDBTableList.TableTypes.IsString)
          str6 = tag.FieldSizeToString;
        if (str4 == "0")
          str4 = "1";
        AddFields.ImportableLoanXDBFieldTable loanXdbFieldTable2 = loanXdbFieldTable1;
        string[] strArray = new string[7]
        {
          str3,
          str4,
          str5,
          str6,
          null,
          null,
          null
        };
        bool flag = tag.Auditable;
        strArray[4] = flag.ToString();
        flag = tag.UseIndex;
        strArray[5] = flag.ToString();
        strArray[6] = tag.Description;
        loanXdbFieldTable2.AddNewRow(strArray);
      }
      try
      {
        FileStream fileStream = new FileStream(str1 + "\\" + str2, FileMode.Create, FileAccess.Write, FileShare.None);
        byte[] bytes = Encoding.ASCII.GetBytes(loanXdbFieldTable1.ToCSV());
        fileStream.Write(bytes, 0, bytes.Length);
        fileStream.Close();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You do not have access rights to write file to " + str1 + " folder. " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      int num1 = (int) Utils.Dialog((IWin32Window) this, "The file has been saved to " + str1 + "\\" + str2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void addFieldDlg_OnAddMoreButtonClick(object sender, EventArgs e)
    {
      AddFields addFieldDlg = (AddFields) sender;
      if (addFieldDlg == null)
        return;
      bool flag = false;
      this.listViewSource.SelectedItems.Clear();
      for (int index = 0; index < addFieldDlg.SelectedFieldIDs.Length; ++index)
      {
        if (this.quickTables.ContainsKey((object) addFieldDlg.SelectedFieldIDs[index]))
        {
          ((GVItem) this.quickTables[(object) addFieldDlg.SelectedFieldIDs[index]]).Selected = true;
          this.quickTables.Remove((object) addFieldDlg.SelectedFieldIDs[index]);
        }
        else if (addFieldDlg.SelectedFieldIDs[index].StartsWith("ENHCOND.") || addFieldDlg.SelectedFieldIDs[index].StartsWith("ENHOP.") || addFieldDlg.SelectedFieldIDs[index].StartsWith("Enhanced."))
          flag = true;
      }
      if (flag && this.addEnhancedFieldsToTargetList(addFieldDlg.SelectedFieldIDs) == addFieldDlg.SelectedFieldIDs.Length)
        return;
      this.addFieldsToTargetList(this.listViewSource);
      if (addFieldDlg.ErrorFields.Count <= 0)
        return;
      this.displayAddFieldError(addFieldDlg, true);
    }

    private LoanXDBTableList.TableTypes findFieldType(string fieldId)
    {
      fieldId = fieldId.ToLower();
      if (fieldId.Contains("count"))
        return LoanXDBTableList.TableTypes.IsNumeric;
      return fieldId.Contains("enhanced.adte") || fieldId.Contains("enhanced.sdte") || fieldId.Contains("enhanced.edte") ? LoanXDBTableList.TableTypes.IsDate : LoanXDBTableList.TableTypes.IsString;
    }

    private string findFieldSize(string fieldId, LoanXDBTableList.TableTypes fieldType)
    {
      string fieldSize;
      switch (fieldType)
      {
        case LoanXDBTableList.TableTypes.IsNumeric:
          fieldSize = "13";
          break;
        case LoanXDBTableList.TableTypes.IsDate:
          fieldSize = "4";
          break;
        default:
          fieldSize = fieldId.ToLower().Contains("enhanced.pint") || fieldId.ToLower().Contains("enhanced.pext") ? "1" : "4096";
          break;
      }
      return fieldSize;
    }

    private int addEnhancedFieldsToTargetList(string[] addedFields)
    {
      int targetList = 0;
      for (int index = 0; index < addedFields.Length; ++index)
      {
        if (addedFields[index].StartsWith("ENHCOND.") || addedFields[index].StartsWith("ENHOP.") || addedFields[index].StartsWith("Enhanced."))
        {
          LoanXDBTableList.TableTypes fieldType = this.findFieldType(addedFields[index]);
          string fieldSize = this.findFieldSize(addedFields[index], fieldType);
          ++targetList;
          string fieldDescription = this.generateFieldDescription(addedFields[index]);
          LoanXDBField dbField = new LoanXDBField(addedFields[index], fieldDescription, fieldType, fieldSize, false, false, 1, LoanXDBField.FieldStatus.New);
          if (!this.hasDuplicateField(dbField))
            this.listViewTarget.Items.Add(this.createTargetFieldListItem(dbField));
        }
      }
      return targetList;
    }

    private string findDispositionDescriptionType(string val)
    {
      string dispositionDescriptionType = "";
      if (string.IsNullOrEmpty(val))
        return dispositionDescriptionType;
      switch (val)
      {
        case "OP":
          dispositionDescriptionType = "Open";
          break;
        case "CL":
          dispositionDescriptionType = "Closed";
          break;
        case "OC":
          dispositionDescriptionType = "both Open and Closed";
          break;
      }
      return dispositionDescriptionType;
    }

    private string parentDescription(string abbreviation)
    {
      switch (abbreviation)
      {
        case "ADBY":
          return "Status Added by User";
        case "ADTE":
          return "Date of Status Added";
        case "CAT":
          return "Category";
        case "COP":
          return "Category Options";
        case "ECOM":
          return "External Comment";
        case "EDES":
          return "External Description";
        case "EDTE":
          return "Effective End Date";
        case "EID":
          return "External ID";
        case "ICOM":
          return "Internal Comment";
        case "IDES":
          return "Internal Description";
        case "IID":
          return "Internal ID";
        case "NAME":
          return "Condition Name";
        case "OWN":
          return "Condition Owner";
        case "PAIR":
          return "For Borrower Pair";
        case "PEXT":
          return "Print Externally";
        case "PINT":
          return "Print Internally";
        case "PTO":
          return "Prior To";
        case "RDTL":
          return "Recipient Details";
        case "ROP":
          return "Recipient Options";
        case "SCON":
          return "Source Of Condition";
        case "SDTE":
          return "Effective Start Date";
        case "SOP":
          return "Source Options";
        case "SRC":
          return "Source";
        case "TOP":
          return "Tracking Options";
        case "TOW":
          return "Tracking Owner";
        case "TYPE":
          return "Condition Type";
        default:
          return "";
      }
    }

    private string generateEnahcedFieldDescription(string fieldId)
    {
      string fieldDescription = "";
      string[] separator = new string[1]{ "." };
      string[] strArray = fieldId.Split(separator, StringSplitOptions.None);
      if (strArray.Length >= 4)
        fieldDescription = this.parentDescription(strArray[1]) + " - of conditon type " + strArray[2] + " and condition name " + strArray[3];
      if (fieldId.StartsWith("Enhanced.TOW") && strArray.Length >= 5)
        fieldDescription = fieldDescription + " for role " + strArray[4];
      else if ((fieldId.StartsWith("Enhanced.ADBY") || fieldId.StartsWith("Enhanced.ADTE")) && strArray.Length >= 5)
        fieldDescription = fieldDescription + " for tracking status " + strArray[4];
      return fieldDescription;
    }

    private string generateENHOPFieldDescription(string fieldId)
    {
      string fieldDescription = "";
      string[] separator = new string[1]{ "." };
      string[] strArray = fieldId.Split(separator, StringSplitOptions.None);
      if (strArray.Length >= 6)
        fieldDescription = "List of " + this.findDispositionDescriptionType(strArray[4]) + " conditions where " + this.parentDescription(strArray[1]) + " is " + strArray[5] + ", of condition type " + strArray[3] + " and print type " + strArray[2];
      return fieldDescription;
    }

    private string generateENHCONDFieldDescription(string fieldId)
    {
      string fieldDescription = "";
      if (fieldId.StartsWith("ENHCOND.EXT.OPEN.TYPE."))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of Open External Conditions of type - " + fieldId.Substring(28) : "Number of Open External Enhanced Conditions") : (fieldId.EndsWith("ALL") ? "All External Open Enhanced Conditions" : "External Open Enhanced Conditions of type - " + fieldId.Substring(22));
      else if (fieldId.StartsWith("ENHCOND.EXT.TYPE."))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of External Conditions of type - " + fieldId.Substring(23) : "Number of External Enhanced Conditions") : (fieldId.EndsWith("ALL") ? "All External Enhanced Conditions of all condition types" : "External Enhanced Conditions of type - " + fieldId.Substring(17));
      if (fieldId.StartsWith("ENHCOND.INT.OPEN.TYPE."))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of Open Internal Conditions of type - " + fieldId.Substring(28) : "Number of Open Internal Enhanced Conditions") : (fieldId.EndsWith("ALL") ? "All Internal Open Enhanced Conditions" : "Internal Open Enhanced Conditions of type - " + fieldId.Substring(22));
      else if (fieldId.StartsWith("ENHCOND.INT.TYPE."))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of Internal Conditions of type - " + fieldId.Substring(23) : "Number of Internal Enhanced Conditions") : (fieldId.EndsWith("ALL") ? "All Internal Enhanced Conditions of all condition types" : "Internal Enhanced Conditions of type - " + fieldId.Substring(17));
      if (fieldId.StartsWith("ENHCOND.EXT.SATISFIED"))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of External Enhanced Conditions which are not open of condition type - " + fieldId.Substring(33) : "Number of External Enhanced Conditions which are not open of all condition types") : (fieldId.EndsWith("ALL") ? "All External Enhanced Conditions which are not open of all condition types" : "All External Enhanced Conditions which are not open of condition type - " + fieldId.Substring(27));
      else if (fieldId.StartsWith("ENHCOND.INT.SATISFIED"))
        fieldDescription = fieldId.Contains("COUNT") ? (!fieldId.EndsWith("COUNT.ALL") ? "Number of Internal Enhanced Conditions which are not open of condition type - " + fieldId.Substring(33) : "Number of Internal Enhanced Conditions which are not open of all condition types") : (fieldId.EndsWith("ALL") ? "All Internal Enhanced Conditions which are not open of all condition types" : "All Internal Enhanced Conditions which are not open of condition type - " + fieldId.Substring(27));
      switch (fieldId)
      {
        case "ENHCOND.OPEN.COUNT.ALL":
          fieldDescription = "Number of Open External and Internal Conditions of all condition types";
          break;
        case "ENHCOND.COUNT.ALL":
          fieldDescription = "Number of External and Internal Conditions of all condition types";
          break;
        case "ENHCOND.OPEN.ALL":
          fieldDescription = "All Open Enhanced Conditions of all condition types";
          break;
        case "ENHCOND.ALL":
          fieldDescription = "All Enhanced Conditions of all condition types";
          break;
        case "ENHCOND.EXT.OPEN.PUB.TYPE.COUNT.ALL":
          fieldDescription = "Number of External Enhanced Conditions with an open Publish Date";
          break;
        case "ENHCOND.SATISFIED.COUNT.ALL":
          fieldDescription = "Number of Internal & External Enhanced Conditions which are not open of all condition types";
          break;
        case "ENHCOND.SATISFIED.ALL":
          fieldDescription = "All Internal & External Enhanced Conditions which are not open of all condition types";
          break;
      }
      return fieldDescription;
    }

    private string generateFieldDescription(string fieldId)
    {
      string fieldDescription = "";
      if (fieldId.StartsWith("ENHOP."))
        fieldDescription = this.generateENHOPFieldDescription(fieldId);
      else if (fieldId.StartsWith("Enhanced."))
        fieldDescription = this.generateEnahcedFieldDescription(fieldId);
      else if (fieldId.StartsWith("ENHCOND."))
        fieldDescription = this.generateENHCONDFieldDescription(fieldId);
      return fieldDescription;
    }

    private void addFieldDlg_OnImportButtonClick(object sender, EventArgs e)
    {
      AddFields addFieldDlg = (AddFields) sender;
      if (addFieldDlg == null)
        return;
      this.Cursor = Cursors.WaitCursor;
      this.showFieldCountWarning = false;
      this.listViewSource.SelectedItems.Clear();
      foreach (DataRow row in (InternalDataCollectionBase) addFieldDlg.ImportedFieldTable.Rows)
      {
        string loanXdbFieldValue1 = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.FieldID);
        string pairOrder = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.BorrowerPair);
        string loanXdbFieldValue2 = this.getImportableLoanXDBFieldValue(row, ImportableLoanXDBFieldName.InstanceIndex);
        FieldDefinition field = EncompassFields.GetField(loanXdbFieldValue1, this.fieldSettings);
        string str = loanXdbFieldValue1;
        if (!string.IsNullOrEmpty(loanXdbFieldValue2))
          str = str + "." + loanXdbFieldValue2;
        string key = str + "-";
        if (field != null && (field.Category == FieldCategory.Borrower || field.Category == FieldCategory.Coborrower))
        {
          if (string.IsNullOrEmpty(pairOrder))
            pairOrder = "1";
          key = loanXdbFieldValue1 + "-" + LoanXDBManager.ToPairOrder(this.toPairNumber(pairOrder));
        }
        if (this.quickTablesTarget[(object) key] == null)
        {
          GVItem targetList = this.addImportableFieldRowToTargetList(row);
          if (targetList != null && !this.quickTablesTarget.Contains((object) (targetList.Text + "-" + targetList.SubItems[1].Text)))
            this.quickTablesTarget.Add((object) (targetList.Text + "-" + targetList.SubItems[1].Text), (object) 1);
        }
      }
      this.Cursor = Cursors.Default;
      if (addFieldDlg.ErrorFields.Count <= 0)
        return;
      this.displayAddFieldError(addFieldDlg, false);
    }

    private string getImportableLoanXDBFieldValue(DataRow dr, ImportableLoanXDBFieldName fieldName)
    {
      try
      {
        if (dr[fieldName.ToString()] != null)
          return dr[fieldName.ToString()].ToString();
      }
      catch
      {
        return "";
      }
      return "";
    }

    private GVItem addImportableFieldRowToTargetList(DataRow dr)
    {
      string loanXdbFieldValue1 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.FieldID);
      string loanXdbFieldValue2 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.BorrowerPair);
      string loanXdbFieldValue3 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.InstanceIndex);
      string loanXdbFieldValue4 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.FieldSize);
      string loanXdbFieldValue5 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.IncludeInAuditTrail);
      string loanXdbFieldValue6 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.UseIndexForThisField);
      string loanXdbFieldValue7 = this.getImportableLoanXDBFieldValue(dr, ImportableLoanXDBFieldName.Description);
      GVItem targetList = this.addSingleFieldToTargetList(loanXdbFieldValue1, loanXdbFieldValue3);
      if (targetList != null)
      {
        LoanXDBField tag = (LoanXDBField) targetList.Tag;
        FieldDefinition field = EncompassFields.GetField(tag.FieldID, this.fieldSettings);
        if (field != null && (field.Category == FieldCategory.Borrower || field.Category == FieldCategory.Coborrower) && !string.IsNullOrEmpty(loanXdbFieldValue2))
        {
          tag.ComortgagorPair = this.toPairNumber(loanXdbFieldValue2);
          targetList.SubItems[1].Text = LoanXDBManager.ToPairOrder(this.toPairNumber(loanXdbFieldValue2));
        }
        if (!string.IsNullOrEmpty(loanXdbFieldValue5))
        {
          tag.Auditable = this.isTrueString(loanXdbFieldValue5);
          targetList.SubItems[6].Text = tag.Auditable ? "Yes" : "No";
        }
        if (!string.IsNullOrEmpty(loanXdbFieldValue6))
        {
          tag.UseIndex = this.isTrueString(loanXdbFieldValue6);
          targetList.SubItems[5].Text = tag.UseIndex ? "Y" : "N";
        }
        if (!string.IsNullOrEmpty(loanXdbFieldValue7))
        {
          tag.Description = loanXdbFieldValue7;
          targetList.SubItems[2].Text = loanXdbFieldValue7;
        }
        if (!string.IsNullOrEmpty(loanXdbFieldValue4) && tag.FieldType == LoanXDBTableList.TableTypes.IsString)
        {
          tag.FieldSize = int.Parse(loanXdbFieldValue4);
          targetList.SubItems[4].Text = loanXdbFieldValue4;
        }
        targetList.Tag = (object) tag;
      }
      return targetList;
    }

    private bool isTrueString(string input)
    {
      return !string.IsNullOrEmpty(input) && !(input.ToLower() == "false") && !(input.ToLower() == "f") && !(input.ToLower() == "0") && !(input.ToString() == "n") && !(input.ToLower() == "no") && !(input.ToLower() == "off");
    }

    private GVItem addSingleFieldToTargetList(string fieldId, string fieldInstance)
    {
      GridView containingListView = this.getContainingListView(fieldId);
      if (containingListView != null)
      {
        containingListView.SelectedItems.Clear();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) containingListView.Items)
        {
          if (string.Compare(gvItem.Text, fieldId, StringComparison.OrdinalIgnoreCase) == 0)
          {
            gvItem.Selected = true;
            break;
          }
        }
        if (containingListView.SelectedItems.Count > 0)
        {
          this.addFieldsToTargetList(containingListView, fieldInstance, false, false);
          return this.lastAddedTargetGridViewItem;
        }
      }
      return (GVItem) null;
    }

    private void displayAddFieldError(AddFields addFieldDlg, bool showMessageBox)
    {
      if (showMessageBox)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "There are " + addFieldDlg.ErrorFields.Count.ToString() + " fields cannot be added.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      int num2 = (int) new AddFieldErrors(Session.DefaultInstance, "The import is complete, but the following field IDs are invalid and were not imported:", addFieldDlg.ErrorFields, addFieldDlg.ImportedErrorFieldTable).ShowDialog((IWin32Window) this);
    }

    private GridView getContainingListView(string fieldId)
    {
      GridView containingListView = (GridView) null;
      foreach (TabPage tabPage in this.tabControlFields.TabPages)
      {
        foreach (Control control in (ArrangedElementCollection) tabPage.Controls)
        {
          if (control is GridView)
          {
            GridView gridView = (GridView) control;
            foreach (GVItem gvItem in (IEnumerable<GVItem>) gridView.Items)
            {
              if (string.Compare(gvItem.Text, fieldId, StringComparison.OrdinalIgnoreCase) == 0)
              {
                containingListView = gridView;
                break;
              }
            }
            if (containingListView != null)
              break;
          }
        }
        if (containingListView != null)
        {
          this.tabControlFields.SelectedTab = tabPage;
          break;
        }
      }
      return containingListView;
    }

    private int toPairNumber(string pairOrder)
    {
      return pairOrder.Length <= 1 ? Utils.ParseInt((object) pairOrder) : Utils.ParseInt((object) pairOrder.Substring(0, 1));
    }

    public static string ToPairOrder(int pair)
    {
      switch (pair)
      {
        case 1:
          return "1st";
        case 2:
          return "2nd";
        case 3:
          return "3rd";
        case 4:
        case 5:
        case 6:
          return pair.ToString() + "th";
        default:
          return string.Empty;
      }
    }

    private void LoanXDBManager_Load(object sender, EventArgs e)
    {
      if (this.xdbStatus.UpdatesPending)
        this.validateUpdatePendingChanges();
      if (!this.useERDB)
      {
        this.promptForNMLSReportFields();
        this.promptForNCMLDReportFields();
        this.promptForHMDAReportFields();
      }
      this.ValidateMaxFields();
    }

    private void promptForRequiredReportFields(
      string[] fieldIds,
      string reportId,
      string reportName)
    {
      List<object[]> missingFields = new List<object[]>();
      foreach (string fieldId in fieldIds)
      {
        FieldPairInfo fieldPairInfo = FieldPairParser.ParseFieldPairInfo(fieldId);
        if ((fieldPairInfo == null || fieldPairInfo.PairIndex <= 1 ? this.tableList.GetField(fieldId) : this.tableList.GetField(fieldPairInfo.FieldID, fieldPairInfo.PairIndex)) == null)
        {
          StandardField fieldI = this.standardFields.GetFieldI(fieldId);
          if (fieldI == null)
          {
            missingFields.Clear();
            break;
          }
          missingFields.Add(new object[2]
          {
            (object) fieldI,
            (object) fieldPairInfo
          });
        }
      }
      if (missingFields.Count == 0 || (Session.ConfigurationManager.GetCompanySetting(reportId, "HideRDBPopup") ?? "") == "1")
        return;
      using (LoanXDBNMLSReportFieldDialog reportFieldDialog = new LoanXDBNMLSReportFieldDialog(missingFields, reportName))
      {
        int num = (int) reportFieldDialog.ShowDialog((IWin32Window) this);
        if (reportFieldDialog.HideDialog)
          Session.ConfigurationManager.SetCompanySetting(reportId, "HideRDBPopup", "1");
        if (num != 6)
          return;
        Dictionary<string, StandardField> dictionary = new Dictionary<string, StandardField>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        Dictionary<string, List<FieldPairInfo>> fieldPairMap = new Dictionary<string, List<FieldPairInfo>>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        foreach (object[] objArray in missingFields)
        {
          StandardField standardField = (StandardField) objArray[0];
          dictionary[standardField.FieldID] = standardField;
          if (!fieldPairMap.ContainsKey(standardField.FieldID))
            fieldPairMap[standardField.FieldID] = new List<FieldPairInfo>();
          fieldPairMap[standardField.FieldID].Add((FieldPairInfo) objArray[1]);
        }
        this.listViewSource.SelectedItems.Clear();
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.listViewSource.Items)
        {
          if (gvItem.Tag is FieldDefinition tag && dictionary.ContainsKey(tag.FieldID))
            gvItem.Selected = true;
        }
        if (!this.addBtn.Enabled)
          return;
        this.addFieldsToTargetList(this.listViewSource, (string) null, true, true, fieldPairMap);
      }
    }

    private void promptForNCMLDReportFields()
    {
      this.promptForRequiredReportFields(NCMLDReportGenerator.GetRequiredReportingFields(Session.DefaultInstance), "NCMLD", "North Carolina Mortgage Loan Data Report");
    }

    private void promptForNMLSReportFields()
    {
      this.promptForRequiredReportFields(NMLSReportGenerator.GetRequiredReportingFields(Session.SessionObjects, NMLSReportType.Expanded), "NMLS", "NMLS Call Report");
    }

    private void promptForHMDAReportFields()
    {
      this.promptForRequiredReportFields(HMDAReportGenerator.GetRequiredReportingFields(Session.SessionObjects), "HMDA", "HMDA Report");
    }

    private void validateUpdatePendingChanges()
    {
      if (Utils.Dialog((IWin32Window) this, "One or more of the Reporting Database fields are out-of-date. Would you like to populate those fields now with data from the loan files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.populateReportingData(true, true);
    }

    private void ValidateMaxFields()
    {
      if (this.useERDB || this.listViewTarget.Items.Count <= 1500)
        return;
      this.showFieldCountWarning = false;
      int num = (int) Utils.Dialog((IWin32Window) this, "Warning: Your Reporting Database exceeds the recommended maximum of " + (object) 1500 + " fields. Adding fields beyond this limit can result in significant performance degradation.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void lnkHelp_Help(object sender, EventArgs e) => JedHelp.ShowHelp("ReportingDatabase");

    private void LoanXDBManager_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.lnkHelp_Help((object) null, (EventArgs) null);
    }

    private void btnPreferences_Click(object sender, EventArgs e)
    {
      if (!this.useERDB)
      {
        using (PreferencesDialog preferencesDialog = new PreferencesDialog())
        {
          if (preferencesDialog.ShowDialog((IWin32Window) this) != DialogResult.OK)
            return;
          if (this.xdbStatus.UpdatesPending)
            this.validateUpdatePendingChanges();
          if (preferencesDialog.HMDAoption != preferencesDialog.HMDAInitialState)
          {
            Session.ConfigurationManager.SetCompanySetting("HMDA", "HideRDBPopup", preferencesDialog.HMDAoption ? "0" : "1");
            if (preferencesDialog.HMDAoption)
              this.promptForHMDAReportFields();
          }
          if (preferencesDialog.NMLSoption != preferencesDialog.NMLSInitialState)
          {
            Session.ConfigurationManager.SetCompanySetting("NMLS", "HideRDBPopup", preferencesDialog.NMLSoption ? "0" : "1");
            if (preferencesDialog.NMLSoption)
              this.promptForNMLSReportFields();
          }
          if (preferencesDialog.NCMLDoption != preferencesDialog.NCMLDInitialState)
          {
            Session.ConfigurationManager.SetCompanySetting("NCMLD", "HideRDBPopup", preferencesDialog.NCMLDoption ? "0" : "1");
            if (preferencesDialog.NCMLDoption)
              this.promptForNCMLDReportFields();
          }
        }
      }
      this.ValidateMaxFields();
    }

    private class RebuildDatabaseAsyncParam
    {
      public readonly bool PendingFieldsOnly;
      public readonly bool UpdateAllLoans;

      public RebuildDatabaseAsyncParam(bool pendingFieldsOnly, bool updateAllLoans)
      {
        this.PendingFieldsOnly = pendingFieldsOnly;
        this.UpdateAllLoans = updateAllLoans;
      }
    }
  }
}
