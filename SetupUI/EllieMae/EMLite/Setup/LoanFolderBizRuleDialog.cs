// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LoanFolderBizRuleDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Setup
{
  public class LoanFolderBizRuleDialog : SettingsUserControl
  {
    private Sessions.Session session;
    private string loanFolder;
    private LoanFolderRuleManager bpmMgr;
    private GroupContainer gcBizRule;
    private Label label1;
    private Label label2;
    private ListView listViewLoanStatuses;
    private RadioButton radioBtnOriginateYes;
    private ComboBox cmbBoxMilestones;
    private RadioButton radioBtnOriginateNo;
    private CheckBox chkBoxMoveRule;
    private RadioButton radioBtnLoanStatuses;
    private RadioButton radioBtnFinishedMS;
    private Label label4;
    private StandardIconButton stdIconBtnReset;
    private StandardIconButton stdIconBtnSave;
    private GroupContainer gcLoanFolders;
    private ToolTip toolTip1;
    private IconButton iconBtnHelp;
    private GridView gridViewFolders;
    private CollapsibleSplitter collapsibleSplitter1;
    private BorderPanel borderPanel1;
    private Label label5;
    private Panel panel3;
    private RadioButton radioBtnImportYes;
    private RadioButton radioBtnImportNo;
    private Label label3;
    private Panel panel2;
    private RadioButton radioBtnDuplicateFromYes;
    private RadioButton radioBtnDuplicateFromNo;
    private Panel panel1;
    private Label label6;
    private Panel panel4;
    private RadioButton radioBtnDuplicateIntoYes;
    private RadioButton radioBtnDuplicateIntoNo;
    private Panel panel5;
    private Label label7;
    private RadioButton radioBtnPiggybackYes;
    private RadioButton radioBtnPiggybackNo;
    private IContainer components;

    public LoanFolderBizRuleDialog(
      Sessions.Session session,
      SetUpContainer setupContainer,
      bool allowMultiSelect)
      : base(setupContainer)
    {
      this.InitializeComponent();
      this.session = session;
      this.gridViewFolders.AllowMultiselect = allowMultiSelect;
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
      ListViewItem listViewItem1 = new ListViewItem("");
      ListViewItem listViewItem2 = new ListViewItem("");
      ListViewItem listViewItem3 = new ListViewItem("");
      ListViewItem listViewItem4 = new ListViewItem("");
      ListViewItem listViewItem5 = new ListViewItem("");
      ListViewItem listViewItem6 = new ListViewItem("");
      ListViewItem listViewItem7 = new ListViewItem("");
      ListViewItem listViewItem8 = new ListViewItem("");
      ListViewItem listViewItem9 = new ListViewItem("");
      GVColumn gvColumn = new GVColumn();
      this.gcBizRule = new GroupContainer();
      this.borderPanel1 = new BorderPanel();
      this.label7 = new Label();
      this.panel5 = new Panel();
      this.radioBtnPiggybackYes = new RadioButton();
      this.radioBtnPiggybackNo = new RadioButton();
      this.label6 = new Label();
      this.panel4 = new Panel();
      this.radioBtnDuplicateIntoYes = new RadioButton();
      this.radioBtnDuplicateIntoNo = new RadioButton();
      this.label5 = new Label();
      this.panel3 = new Panel();
      this.radioBtnImportYes = new RadioButton();
      this.radioBtnImportNo = new RadioButton();
      this.label3 = new Label();
      this.panel2 = new Panel();
      this.radioBtnDuplicateFromYes = new RadioButton();
      this.radioBtnDuplicateFromNo = new RadioButton();
      this.label2 = new Label();
      this.label1 = new Label();
      this.panel1 = new Panel();
      this.radioBtnOriginateYes = new RadioButton();
      this.radioBtnOriginateNo = new RadioButton();
      this.iconBtnHelp = new IconButton();
      this.stdIconBtnReset = new StandardIconButton();
      this.stdIconBtnSave = new StandardIconButton();
      this.radioBtnLoanStatuses = new RadioButton();
      this.label4 = new Label();
      this.radioBtnFinishedMS = new RadioButton();
      this.listViewLoanStatuses = new ListView();
      this.cmbBoxMilestones = new ComboBox();
      this.chkBoxMoveRule = new CheckBox();
      this.gcLoanFolders = new GroupContainer();
      this.gridViewFolders = new GridView();
      this.toolTip1 = new ToolTip(this.components);
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.gcBizRule.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel4.SuspendLayout();
      this.panel3.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel1.SuspendLayout();
      ((ISupportInitialize) this.iconBtnHelp).BeginInit();
      ((ISupportInitialize) this.stdIconBtnReset).BeginInit();
      ((ISupportInitialize) this.stdIconBtnSave).BeginInit();
      this.gcLoanFolders.SuspendLayout();
      this.SuspendLayout();
      this.gcBizRule.AutoScroll = true;
      this.gcBizRule.Controls.Add((Control) this.borderPanel1);
      this.gcBizRule.Controls.Add((Control) this.iconBtnHelp);
      this.gcBizRule.Controls.Add((Control) this.stdIconBtnReset);
      this.gcBizRule.Controls.Add((Control) this.stdIconBtnSave);
      this.gcBizRule.Controls.Add((Control) this.radioBtnLoanStatuses);
      this.gcBizRule.Controls.Add((Control) this.label4);
      this.gcBizRule.Controls.Add((Control) this.radioBtnFinishedMS);
      this.gcBizRule.Controls.Add((Control) this.listViewLoanStatuses);
      this.gcBizRule.Controls.Add((Control) this.cmbBoxMilestones);
      this.gcBizRule.Controls.Add((Control) this.chkBoxMoveRule);
      this.gcBizRule.Dock = DockStyle.Fill;
      this.gcBizRule.HeaderForeColor = SystemColors.ControlText;
      this.gcBizRule.Location = new Point(227, 0);
      this.gcBizRule.Name = "gcBizRule";
      this.gcBizRule.Size = new Size(688, 582);
      this.gcBizRule.TabIndex = 16;
      this.gcBizRule.Text = "<folder> Business Rule";
      this.borderPanel1.Borders = AnchorStyles.Bottom;
      this.borderPanel1.Controls.Add((Control) this.label7);
      this.borderPanel1.Controls.Add((Control) this.panel5);
      this.borderPanel1.Controls.Add((Control) this.label6);
      this.borderPanel1.Controls.Add((Control) this.panel4);
      this.borderPanel1.Controls.Add((Control) this.label5);
      this.borderPanel1.Controls.Add((Control) this.panel3);
      this.borderPanel1.Controls.Add((Control) this.label3);
      this.borderPanel1.Controls.Add((Control) this.panel2);
      this.borderPanel1.Controls.Add((Control) this.label2);
      this.borderPanel1.Controls.Add((Control) this.label1);
      this.borderPanel1.Controls.Add((Control) this.panel1);
      this.borderPanel1.Dock = DockStyle.Top;
      this.borderPanel1.Location = new Point(1, 26);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(686, 150);
      this.borderPanel1.TabIndex = 22;
      this.label7.AutoSize = true;
      this.label7.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label7.Location = new Point(3, 126);
      this.label7.Name = "label7";
      this.label7.Size = new Size(324, 16);
      this.label7.TabIndex = 21;
      this.label7.Text = "Can Piggyback loans be added to the loan folder?";
      this.panel5.Controls.Add((Control) this.radioBtnPiggybackYes);
      this.panel5.Controls.Add((Control) this.radioBtnPiggybackNo);
      this.panel5.Location = new Point(259, 123);
      this.panel5.Name = "panel5";
      this.panel5.Size = new Size(144, 24);
      this.panel5.TabIndex = 22;
      this.radioBtnPiggybackYes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnPiggybackYes.Location = new Point(4, 0);
      this.radioBtnPiggybackYes.Name = "radioBtnPiggybackYes";
      this.radioBtnPiggybackYes.Size = new Size(49, 24);
      this.radioBtnPiggybackYes.TabIndex = 1;
      this.radioBtnPiggybackYes.Text = "Yes";
      this.radioBtnPiggybackYes.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radioBtnPiggybackNo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnPiggybackNo.Location = new Point(58, 0);
      this.radioBtnPiggybackNo.Name = "radioBtnPiggybackNo";
      this.radioBtnPiggybackNo.Size = new Size(45, 24);
      this.radioBtnPiggybackNo.TabIndex = 2;
      this.radioBtnPiggybackNo.Text = "No";
      this.label6.AutoSize = true;
      this.label6.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label6.Location = new Point(3, 80);
      this.label6.Name = "label6";
      this.label6.Size = new Size(291, 16);
      this.label6.TabIndex = 19;
      this.label6.Text = "Can loans be duplicated into the loan folder?";
      this.panel4.Controls.Add((Control) this.radioBtnDuplicateIntoYes);
      this.panel4.Controls.Add((Control) this.radioBtnDuplicateIntoNo);
      this.panel4.Location = new Point(259, 75);
      this.panel4.Name = "panel4";
      this.panel4.Size = new Size(144, 24);
      this.panel4.TabIndex = 20;
      this.radioBtnDuplicateIntoYes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnDuplicateIntoYes.Location = new Point(4, 0);
      this.radioBtnDuplicateIntoYes.Name = "radioBtnDuplicateIntoYes";
      this.radioBtnDuplicateIntoYes.Size = new Size(49, 24);
      this.radioBtnDuplicateIntoYes.TabIndex = 1;
      this.radioBtnDuplicateIntoYes.Text = "Yes";
      this.radioBtnDuplicateIntoYes.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radioBtnDuplicateIntoNo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnDuplicateIntoNo.Location = new Point(58, 0);
      this.radioBtnDuplicateIntoNo.Name = "radioBtnDuplicateIntoNo";
      this.radioBtnDuplicateIntoNo.Size = new Size(45, 24);
      this.radioBtnDuplicateIntoNo.TabIndex = 2;
      this.radioBtnDuplicateIntoNo.Text = "No";
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(3, 104);
      this.label5.Name = "label5";
      this.label5.Size = new Size(281, 16);
      this.label5.TabIndex = 17;
      this.label5.Text = "Can loans be imported into the loan folder?";
      this.panel3.Controls.Add((Control) this.radioBtnImportYes);
      this.panel3.Controls.Add((Control) this.radioBtnImportNo);
      this.panel3.Location = new Point(259, 99);
      this.panel3.Name = "panel3";
      this.panel3.Size = new Size(144, 24);
      this.panel3.TabIndex = 18;
      this.radioBtnImportYes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnImportYes.Location = new Point(4, 0);
      this.radioBtnImportYes.Name = "radioBtnImportYes";
      this.radioBtnImportYes.Size = new Size(49, 24);
      this.radioBtnImportYes.TabIndex = 1;
      this.radioBtnImportYes.Text = "Yes";
      this.radioBtnImportYes.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radioBtnImportNo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnImportNo.Location = new Point(58, 0);
      this.radioBtnImportNo.Name = "radioBtnImportNo";
      this.radioBtnImportNo.Size = new Size(45, 24);
      this.radioBtnImportNo.TabIndex = 2;
      this.radioBtnImportNo.Text = "No";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.Location = new Point(3, 56);
      this.label3.Name = "label3";
      this.label3.Size = new Size(296, 16);
      this.label3.TabIndex = 15;
      this.label3.Text = "Can loans be duplicated from the loan folder?";
      this.panel2.Controls.Add((Control) this.radioBtnDuplicateFromYes);
      this.panel2.Controls.Add((Control) this.radioBtnDuplicateFromNo);
      this.panel2.Location = new Point(259, 51);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(144, 24);
      this.panel2.TabIndex = 16;
      this.radioBtnDuplicateFromYes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnDuplicateFromYes.Location = new Point(4, 0);
      this.radioBtnDuplicateFromYes.Name = "radioBtnDuplicateFromYes";
      this.radioBtnDuplicateFromYes.Size = new Size(49, 24);
      this.radioBtnDuplicateFromYes.TabIndex = 1;
      this.radioBtnDuplicateFromYes.Text = "Yes";
      this.radioBtnDuplicateFromYes.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radioBtnDuplicateFromNo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnDuplicateFromNo.Location = new Point(58, 0);
      this.radioBtnDuplicateFromNo.Name = "radioBtnDuplicateFromNo";
      this.radioBtnDuplicateFromNo.Size = new Size(45, 24);
      this.radioBtnDuplicateFromNo.TabIndex = 2;
      this.radioBtnDuplicateFromNo.Text = "No";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(3, 32);
      this.label2.Name = "label2";
      this.label2.Size = new Size(277, 16);
      this.label2.TabIndex = 9;
      this.label2.Text = "Can loans be originated in the loan folder?";
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(4, 3);
      this.label1.Name = "label1";
      this.label1.Size = new Size(148, 21);
      this.label1.TabIndex = 13;
      this.label1.Text = "Set Up Origination Rule";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.panel1.Controls.Add((Control) this.radioBtnOriginateYes);
      this.panel1.Controls.Add((Control) this.radioBtnOriginateNo);
      this.panel1.Location = new Point(259, 27);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(144, 24);
      this.panel1.TabIndex = 14;
      this.radioBtnOriginateYes.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnOriginateYes.Location = new Point(4, 0);
      this.radioBtnOriginateYes.Name = "radioBtnOriginateYes";
      this.radioBtnOriginateYes.Size = new Size(49, 24);
      this.radioBtnOriginateYes.TabIndex = 1;
      this.radioBtnOriginateYes.Text = "Yes";
      this.radioBtnOriginateYes.CheckedChanged += new EventHandler(this.onFieldValueChanged);
      this.radioBtnOriginateNo.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnOriginateNo.Location = new Point(58, 0);
      this.radioBtnOriginateNo.Name = "radioBtnOriginateNo";
      this.radioBtnOriginateNo.Size = new Size(45, 24);
      this.radioBtnOriginateNo.TabIndex = 2;
      this.radioBtnOriginateNo.Text = "No";
      this.iconBtnHelp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.iconBtnHelp.BackColor = Color.Transparent;
      this.iconBtnHelp.DisabledImage = (Image) null;
      this.iconBtnHelp.Image = (Image) Resources.help;
      this.iconBtnHelp.Location = new Point(665, 5);
      this.iconBtnHelp.MouseDownImage = (Image) null;
      this.iconBtnHelp.MouseOverImage = (Image) Resources.help_over;
      this.iconBtnHelp.Name = "iconBtnHelp";
      this.iconBtnHelp.Size = new Size(16, 16);
      this.iconBtnHelp.TabIndex = 21;
      this.iconBtnHelp.TabStop = false;
      this.iconBtnHelp.Click += new EventHandler(this.iconBtnHelp_Click);
      this.stdIconBtnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnReset.BackColor = Color.Transparent;
      this.stdIconBtnReset.Location = new Point(643, 5);
      this.stdIconBtnReset.MouseDownImage = (Image) null;
      this.stdIconBtnReset.Name = "stdIconBtnReset";
      this.stdIconBtnReset.Size = new Size(16, 16);
      this.stdIconBtnReset.StandardButtonType = StandardIconButton.ButtonType.ResetButton;
      this.stdIconBtnReset.TabIndex = 20;
      this.stdIconBtnReset.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnReset, "Reset");
      this.stdIconBtnReset.Click += new EventHandler(this.stdIconBtnReset_Click);
      this.stdIconBtnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.stdIconBtnSave.BackColor = Color.Transparent;
      this.stdIconBtnSave.Location = new Point(621, 5);
      this.stdIconBtnSave.MouseDownImage = (Image) null;
      this.stdIconBtnSave.Name = "stdIconBtnSave";
      this.stdIconBtnSave.Size = new Size(16, 16);
      this.stdIconBtnSave.StandardButtonType = StandardIconButton.ButtonType.SaveButton;
      this.stdIconBtnSave.TabIndex = 19;
      this.stdIconBtnSave.TabStop = false;
      this.toolTip1.SetToolTip((Control) this.stdIconBtnSave, "Save");
      this.stdIconBtnSave.Click += new EventHandler(this.btnSave_Click);
      this.radioBtnLoanStatuses.BackColor = Color.Transparent;
      this.radioBtnLoanStatuses.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnLoanStatuses.Location = new Point(218, 226);
      this.radioBtnLoanStatuses.Name = "radioBtnLoanStatuses";
      this.radioBtnLoanStatuses.Size = new Size(104, 25);
      this.radioBtnLoanStatuses.TabIndex = 6;
      this.radioBtnLoanStatuses.Text = "Loan Status";
      this.radioBtnLoanStatuses.UseVisualStyleBackColor = false;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(25, 207);
      this.label4.Name = "label4";
      this.label4.Size = new Size(748, 16);
      this.label4.TabIndex = 18;
      this.label4.Text = "Select a condition that must be met (a finished milestone or loan statuses) before a loan can be moved into the folder.";
      this.radioBtnFinishedMS.BackColor = Color.Transparent;
      this.radioBtnFinishedMS.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.radioBtnFinishedMS.Location = new Point(28, 226);
      this.radioBtnFinishedMS.Name = "radioBtnFinishedMS";
      this.radioBtnFinishedMS.Size = new Size(128, 25);
      this.radioBtnFinishedMS.TabIndex = 4;
      this.radioBtnFinishedMS.Text = "Finished Milestone";
      this.radioBtnFinishedMS.UseVisualStyleBackColor = false;
      this.radioBtnFinishedMS.CheckedChanged += new EventHandler(this.radioBtnFinishedMS_CheckedChanged);
      this.listViewLoanStatuses.BackColor = SystemColors.Window;
      this.listViewLoanStatuses.CheckBoxes = true;
      this.listViewLoanStatuses.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.listViewLoanStatuses.HideSelection = false;
      listViewItem1.StateImageIndex = 0;
      listViewItem2.StateImageIndex = 0;
      listViewItem3.StateImageIndex = 0;
      listViewItem4.StateImageIndex = 0;
      listViewItem5.StateImageIndex = 0;
      listViewItem6.StateImageIndex = 0;
      listViewItem7.StateImageIndex = 0;
      listViewItem8.StateImageIndex = 0;
      listViewItem9.StateImageIndex = 0;
      this.listViewLoanStatuses.Items.AddRange(new ListViewItem[9]
      {
        listViewItem1,
        listViewItem2,
        listViewItem3,
        listViewItem4,
        listViewItem5,
        listViewItem6,
        listViewItem7,
        listViewItem8,
        listViewItem9
      });
      this.listViewLoanStatuses.Location = new Point(218, 257);
      this.listViewLoanStatuses.Name = "listViewLoanStatuses";
      this.listViewLoanStatuses.Size = new Size(278, 174);
      this.listViewLoanStatuses.TabIndex = 7;
      this.listViewLoanStatuses.UseCompatibleStateImageBehavior = false;
      this.listViewLoanStatuses.View = View.List;
      this.listViewLoanStatuses.ItemChecked += new ItemCheckedEventHandler(this.listViewLoanStatuses_ItemChecked);
      this.cmbBoxMilestones.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxMilestones.Location = new Point(28, 257);
      this.cmbBoxMilestones.Name = "cmbBoxMilestones";
      this.cmbBoxMilestones.Size = new Size(160, 24);
      this.cmbBoxMilestones.TabIndex = 5;
      this.cmbBoxMilestones.SelectedIndexChanged += new EventHandler(this.cmbBoxMilestones_SelectedIndexChanged);
      this.chkBoxMoveRule.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.chkBoxMoveRule.Location = new Point(8, 175);
      this.chkBoxMoveRule.Name = "chkBoxMoveRule";
      this.chkBoxMoveRule.Size = new Size(488, 29);
      this.chkBoxMoveRule.TabIndex = 3;
      this.chkBoxMoveRule.Text = "Setup Move Rule";
      this.chkBoxMoveRule.CheckedChanged += new EventHandler(this.chkBoxMoveRule_CheckedChanged);
      this.gcLoanFolders.Controls.Add((Control) this.gridViewFolders);
      this.gcLoanFolders.Dock = DockStyle.Left;
      this.gcLoanFolders.HeaderForeColor = SystemColors.ControlText;
      this.gcLoanFolders.Location = new Point(0, 0);
      this.gcLoanFolders.Name = "gcLoanFolders";
      this.gcLoanFolders.Size = new Size(220, 582);
      this.gcLoanFolders.TabIndex = 18;
      this.gcLoanFolders.Text = "Loan Folders";
      this.gridViewFolders.AllowMultiselect = false;
      this.gridViewFolders.BorderStyle = BorderStyle.None;
      this.gridViewFolders.ClearSelectionsOnEmptyRowClick = false;
      gvColumn.ImageIndex = -1;
      gvColumn.Name = "Column1";
      gvColumn.SpringToFit = true;
      gvColumn.Text = "Column";
      gvColumn.Width = 218;
      this.gridViewFolders.Columns.AddRange(new GVColumn[1]
      {
        gvColumn
      });
      this.gridViewFolders.Dock = DockStyle.Fill;
      this.gridViewFolders.HeaderHeight = 0;
      this.gridViewFolders.HeaderVisible = false;
      this.gridViewFolders.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridViewFolders.Location = new Point(1, 26);
      this.gridViewFolders.Name = "gridViewFolders";
      this.gridViewFolders.Size = new Size(218, 555);
      this.gridViewFolders.TabIndex = 1;
      this.gridViewFolders.SelectedIndexChanged += new EventHandler(this.gridViewFolders_SelectedIndexChanged);
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (Control) this.gcLoanFolders;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(220, 0);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 19;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.Controls.Add((Control) this.gcBizRule);
      this.Controls.Add((Control) this.collapsibleSplitter1);
      this.Controls.Add((Control) this.gcLoanFolders);
      this.ForeColor = SystemColors.ControlText;
      this.Name = nameof (LoanFolderBizRuleDialog);
      this.Size = new Size(915, 582);
      this.Load += new EventHandler(this.LoanFolderBizRuleDialog_Load);
      this.gcBizRule.ResumeLayout(false);
      this.gcBizRule.PerformLayout();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.panel5.ResumeLayout(false);
      this.panel4.ResumeLayout(false);
      this.panel3.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      ((ISupportInitialize) this.iconBtnHelp).EndInit();
      ((ISupportInitialize) this.stdIconBtnReset).EndInit();
      ((ISupportInitialize) this.stdIconBtnSave).EndInit();
      this.gcLoanFolders.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public string[] SelectedFolders
    {
      get
      {
        if (this.gridViewFolders.SelectedItems == null)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (GVItem selectedItem in this.gridViewFolders.SelectedItems)
          stringList.Add(selectedItem.Text);
        return stringList.ToArray();
      }
      set
      {
        if (value == null || value.Length == 0)
          return;
        List<string> stringList = new List<string>((IEnumerable<string>) value);
        foreach (GVItem gvItem in (IEnumerable<GVItem>) this.gridViewFolders.Items)
          gvItem.Selected = stringList.Contains(gvItem.Text);
      }
    }

    private void initFolderList()
    {
      LoanFolderInfo[] allLoanFolderInfos = this.session.LoanManager.GetAllLoanFolderInfos(false);
      this.gridViewFolders.Items.Clear();
      for (int index = 0; index < allLoanFolderInfos.Length; ++index)
        this.gridViewFolders.Items.Add(new GVItem(allLoanFolderInfos[index].Name)
        {
          Tag = (object) allLoanFolderInfos[index]
        });
      this.gridViewFolders.Items[0].Selected = true;
    }

    private void chkBoxMoveRule_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkBoxMoveRule.Checked)
      {
        this.radioBtnFinishedMS.Enabled = this.radioBtnLoanStatuses.Enabled = true;
        this.radioBtnFinishedMSCheckedChanged();
      }
      else
      {
        this.radioBtnFinishedMS.Enabled = this.radioBtnLoanStatuses.Enabled = false;
        this.cmbBoxMilestones.Enabled = false;
        this.listViewLoanStatuses.Enabled = false;
        this.listViewLoanStatuses.BackColor = SystemColors.Control;
      }
      this.setDirtyFlag(true);
    }

    private void radioBtnFinishedMSCheckedChanged()
    {
      this.cmbBoxMilestones.Enabled = this.chkBoxMoveRule.Checked && this.radioBtnFinishedMS.Checked;
      this.listViewLoanStatuses.Enabled = this.radioBtnLoanStatuses.Checked;
      if (this.listViewLoanStatuses.Enabled)
        this.listViewLoanStatuses.BackColor = SystemColors.Window;
      else
        this.listViewLoanStatuses.BackColor = SystemColors.Control;
    }

    private void radioBtnFinishedMS_CheckedChanged(object sender, EventArgs e)
    {
      this.radioBtnFinishedMSCheckedChanged();
      this.setDirtyFlag(true);
    }

    private void btnSave_Click(object sender, EventArgs e) => this.Save();

    private void stdIconBtnReset_Click(object sender, EventArgs e)
    {
      if (ResetConfirmDialog.ShowDialog((IWin32Window) this.setupContainer, (string) null) == DialogResult.Cancel)
        return;
      this.Reset();
    }

    public void SetDirtyFlag(bool val) => this.setDirtyFlag(val);

    protected override void setDirtyFlag(bool val)
    {
      base.setDirtyFlag(val);
      this.stdIconBtnSave.Enabled = this.stdIconBtnReset.Enabled = val;
    }

    public override void Save()
    {
      bool[] loanStatusSettings = new bool[9];
      for (int index = 0; index < 9; ++index)
        loanStatusSettings[index] = this.listViewLoanStatuses.Items[index].Checked;
      BizRule.LoanFolderMoveRuleOption moveRuleOption = BizRule.LoanFolderMoveRuleOption.None;
      string milestoneID = "";
      if (this.chkBoxMoveRule.Checked)
      {
        if (this.radioBtnFinishedMS.Checked)
        {
          if (this.cmbBoxMilestones.SelectedIndex == 0)
          {
            int num = (int) MessageBox.Show((IWin32Window) this, "You must select a milestone.", "Select a Milestone", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
          }
          milestoneID = ((EllieMae.EMLite.Workflow.Milestone) this.cmbBoxMilestones.SelectedItem).MilestoneID;
          moveRuleOption = BizRule.LoanFolderMoveRuleOption.Milestone;
        }
        else
          moveRuleOption = BizRule.LoanFolderMoveRuleOption.LoanStatus;
      }
      this.bpmMgr.SetRule(new LoanFolderRuleInfo(this.loanFolder, this.radioBtnOriginateYes.Checked, this.radioBtnDuplicateFromYes.Checked, this.radioBtnDuplicateIntoYes.Checked, this.radioBtnImportYes.Checked, this.radioBtnPiggybackYes.Checked, moveRuleOption, milestoneID, loanStatusSettings));
      this.setDirtyFlag(false);
    }

    public override void Reset()
    {
      if (this.gridViewFolders.SelectedItems.Count == 0)
        return;
      this.loanFolder = ((LoanFolderInfo) this.gridViewFolders.SelectedItems[0].Tag).Name;
      this.gcBizRule.Text = this.loanFolder + " Business Rule";
      this.listViewLoanStatuses.Items.Clear();
      ListViewItem[] items = new ListViewItem[LoanStatusMap.LoanStatusStrings.Length];
      for (int index = 0; index < items.Length; ++index)
        items[index] = new ListViewItem(LoanStatusMap.LoanStatusStrings[index]);
      this.listViewLoanStatuses.Items.AddRange(items);
      this.bpmMgr = (LoanFolderRuleManager) this.session.BPM.GetBpmManager(BpmCategory.LoanFolder);
      LoanFolderRuleInfo rule = this.bpmMgr.GetRule(this.loanFolder);
      this.radioBtnOriginateYes.Checked = rule.CanOriginateLoans;
      this.radioBtnOriginateNo.Checked = !rule.CanOriginateLoans;
      this.radioBtnDuplicateFromYes.Checked = rule.CanDuplicateLoansFrom;
      this.radioBtnDuplicateFromNo.Checked = !rule.CanDuplicateLoansFrom;
      this.radioBtnDuplicateIntoYes.Checked = rule.CanDuplicateLoansInto;
      this.radioBtnDuplicateIntoNo.Checked = !rule.CanDuplicateLoansInto;
      this.radioBtnImportYes.Checked = rule.CanImportLoans;
      this.radioBtnImportNo.Checked = !rule.CanImportLoans;
      this.radioBtnPiggybackYes.Checked = rule.CanPiggybackLoans;
      this.radioBtnPiggybackNo.Checked = !rule.CanPiggybackLoans;
      this.chkBoxMoveRule.Checked = rule.MoveRuleOption != 0;
      this.radioBtnFinishedMS.Checked = true;
      if (this.chkBoxMoveRule.Checked && rule.MoveRuleOption == BizRule.LoanFolderMoveRuleOption.LoanStatus)
        this.radioBtnLoanStatuses.Checked = true;
      this.chkBoxMoveRule_CheckedChanged((object) this, (EventArgs) null);
      if (rule.LoanStatusSettings != null)
      {
        for (int index = 0; index < 9; ++index)
          this.listViewLoanStatuses.Items[index].Checked = rule.LoanStatusSettings[index];
      }
      else
      {
        for (int index = 0; index < 9; ++index)
          this.listViewLoanStatuses.Items[index].Checked = true;
      }
      this.cmbBoxMilestones.Items.Clear();
      this.cmbBoxMilestones.Items.Add((object) "<Select One>");
      this.cmbBoxMilestones.SelectedIndex = 0;
      List<EllieMae.EMLite.Workflow.Milestone> milestones = this.session.StartupInfo.Milestones;
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
      for (int index = 0; index < milestones.Count; ++index)
      {
        if (milestones[index].Archived)
          milestoneList.Add(milestones[index]);
        else
          this.cmbBoxMilestones.Items.Add((object) milestones[index]);
        if (milestones[index].MilestoneID == rule.MilestoneID)
          this.cmbBoxMilestones.SelectedIndex = this.cmbBoxMilestones.Items.Count - 1;
      }
      this.cmbBoxMilestones.Items.AddRange((object[]) milestoneList.ToArray());
      this.radioBtnFinishedMS_CheckedChanged((object) this, (EventArgs) null);
      this.setDirtyFlag(false);
    }

    private void listViewLoanStatuses_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void onFieldValueChanged(object sender, EventArgs e) => this.setDirtyFlag(true);

    private void cmbBoxMilestones_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.setDirtyFlag(true);
    }

    private void LoanFolderBizRuleDialog_Load(object sender, EventArgs e)
    {
      this.initFolderList();
      this.Reset();
    }

    private void iconBtnHelp_Click(object sender, EventArgs e)
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Loan Folder Business Rule");
    }

    private void gridViewFolders_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.IsDirty && Utils.Dialog((IWin32Window) this.setupContainer, "Do you want to save the changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        this.Save();
      if (this.gridViewFolders.AllowMultiselect)
        this.gcBizRule.Enabled = this.gridViewFolders.SelectedItems.Count <= 1;
      if (this.gridViewFolders.SelectedItems.Count != 1)
        return;
      this.Reset();
    }
  }
}
