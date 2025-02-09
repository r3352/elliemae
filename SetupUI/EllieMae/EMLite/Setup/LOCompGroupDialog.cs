// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.LOCompGroupDialog
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.HelpAPI;
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
  internal class LOCompGroupDialog : Form, IHelp
  {
    private const string className = "LOCompGroupDialog";
    private static string sw = Tracing.SwInputEngine;
    private Sessions.Session session;
    private LoanCompPlan lcp;
    private LoanCompDefaultPlan loanCompDefaultPlan;
    private List<ExternalOriginatorManagementData> externalCompaniesUseThisPlan;
    private List<OrgInfo> orgsUseThisPlan;
    private List<object[]> userWithOrgUseThisPlan;
    private Label lblStatus;
    private CheckBox chkStatus;
    private Label lblName;
    private Label lblDescription;
    private Label lblType;
    private Label lblActivation;
    private Label lblMinTerm;
    private Label lblAmount;
    private Label lblTriggerField;
    private TextBox txtName;
    private TextBox txtDescription;
    private ComboBox cmbType;
    private TextBox textMinTerm;
    private TextBox txtAmount;
    private TextBox txtAmount2;
    private Label lblAmount2;
    private Label lblMinAmount;
    private TextBox txtMaxAmt;
    private Label lblMaxAmount;
    private TextBox txtTriggerField;
    private Label label1;
    private ComboBox cmbRounding;
    private Button btnSave;
    private Button btnCancel;
    private TabControl tabPlan;
    private TabPage tabPageBranch;
    private TabPage tabPageLO;
    private Label lblPlan;
    private TabPage tabPageBroker;
    private GridView gridViewBranch;
    private IContainer components;
    private TextBox txtMinAmt;
    private TextBox txtActivationDate;
    private ComboBox cmbAmount;
    private GridView gridViewLO;
    private GridView gridViewBroker;
    private Label label2;

    public LOCompGroupDialog(Sessions.Session session, LoanCompPlan lcp)
      : this(session, (LoanCompDefaultPlan) null, lcp)
    {
    }

    public LOCompGroupDialog(
      Sessions.Session session,
      LoanCompDefaultPlan loanCompDefaultPlan,
      LoanCompPlan lcp)
    {
      this.session = session;
      this.lcp = lcp;
      this.loanCompDefaultPlan = loanCompDefaultPlan;
      this.InitializeComponent();
      this.initForm();
      this.chkStatus.CheckedChanged += new EventHandler(this.chkStatus_CheckedChanged);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
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
      this.txtName = new TextBox();
      this.lblStatus = new Label();
      this.chkStatus = new CheckBox();
      this.lblName = new Label();
      this.lblDescription = new Label();
      this.lblType = new Label();
      this.lblActivation = new Label();
      this.lblMinTerm = new Label();
      this.lblAmount = new Label();
      this.lblTriggerField = new Label();
      this.txtDescription = new TextBox();
      this.cmbType = new ComboBox();
      this.textMinTerm = new TextBox();
      this.txtAmount = new TextBox();
      this.txtAmount2 = new TextBox();
      this.lblAmount2 = new Label();
      this.lblMinAmount = new Label();
      this.txtMaxAmt = new TextBox();
      this.lblMaxAmount = new Label();
      this.txtTriggerField = new TextBox();
      this.label1 = new Label();
      this.cmbRounding = new ComboBox();
      this.btnSave = new Button();
      this.btnCancel = new Button();
      this.tabPlan = new TabControl();
      this.tabPageBranch = new TabPage();
      this.gridViewBranch = new GridView();
      this.tabPageLO = new TabPage();
      this.gridViewLO = new GridView();
      this.tabPageBroker = new TabPage();
      this.gridViewBroker = new GridView();
      this.lblPlan = new Label();
      this.txtMinAmt = new TextBox();
      this.txtActivationDate = new TextBox();
      this.cmbAmount = new ComboBox();
      this.label2 = new Label();
      this.tabPlan.SuspendLayout();
      this.tabPageBranch.SuspendLayout();
      this.tabPageLO.SuspendLayout();
      this.tabPageBroker.SuspendLayout();
      this.SuspendLayout();
      this.txtName.Location = new Point(148, 29);
      this.txtName.MaxLength = 100;
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(346, 20);
      this.txtName.TabIndex = 1;
      this.txtName.TextChanged += new EventHandler(this.txtName_TextChanged);
      this.lblStatus.AutoSize = true;
      this.lblStatus.Location = new Point(9, 9);
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new Size(37, 13);
      this.lblStatus.TabIndex = 0;
      this.lblStatus.Text = "Status";
      this.chkStatus.AutoSize = true;
      this.chkStatus.Location = new Point(150, 8);
      this.chkStatus.Name = "chkStatus";
      this.chkStatus.Size = new Size(56, 17);
      this.chkStatus.TabIndex = 13;
      this.chkStatus.Text = "Active";
      this.chkStatus.UseVisualStyleBackColor = true;
      this.lblName.AutoSize = true;
      this.lblName.Location = new Point(9, 32);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(35, 13);
      this.lblName.TabIndex = 2;
      this.lblName.Text = "Name";
      this.lblDescription.AutoSize = true;
      this.lblDescription.Location = new Point(9, 58);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new Size(60, 13);
      this.lblDescription.TabIndex = 3;
      this.lblDescription.Text = "Description";
      this.lblType.AutoSize = true;
      this.lblType.Location = new Point(9, 85);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(131, 13);
      this.lblType.TabIndex = 4;
      this.lblType.Text = "Loan Officer/Broker Value";
      this.lblActivation.AutoSize = true;
      this.lblActivation.Location = new Point(9, 110);
      this.lblActivation.Name = "lblActivation";
      this.lblActivation.Size = new Size(80, 13);
      this.lblActivation.TabIndex = 5;
      this.lblActivation.Text = "Activation Date";
      this.lblMinTerm.AutoSize = true;
      this.lblMinTerm.Location = new Point(265, 85);
      this.lblMinTerm.Name = "lblMinTerm";
      this.lblMinTerm.Size = new Size(112, 13);
      this.lblMinTerm.TabIndex = 6;
      this.lblMinTerm.Text = "Minimum Term # Days";
      this.lblAmount.AutoSize = true;
      this.lblAmount.Location = new Point(9, 136);
      this.lblAmount.Name = "lblAmount";
      this.lblAmount.Size = new Size(54, 13);
      this.lblAmount.TabIndex = 7;
      this.lblAmount.Text = "% Amount";
      this.lblTriggerField.AutoSize = true;
      this.lblTriggerField.Location = new Point(9, 240);
      this.lblTriggerField.Name = "lblTriggerField";
      this.lblTriggerField.Size = new Size(68, 13);
      this.lblTriggerField.TabIndex = 9;
      this.lblTriggerField.Text = "Trigger Basis";
      this.txtDescription.Location = new Point(148, 55);
      this.txtDescription.MaxLength = 256;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.Size = new Size(346, 20);
      this.txtDescription.TabIndex = 2;
      this.cmbType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbType.FormattingEnabled = true;
      this.cmbType.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Loan Officer",
        (object) "Broker",
        (object) "Both"
      });
      this.cmbType.Location = new Point(148, 81);
      this.cmbType.Name = "cmbType";
      this.cmbType.Size = new Size(99, 21);
      this.cmbType.TabIndex = 3;
      this.textMinTerm.Location = new Point(379, 81);
      this.textMinTerm.MaxLength = 5;
      this.textMinTerm.Name = "textMinTerm";
      this.textMinTerm.Size = new Size(115, 20);
      this.textMinTerm.TabIndex = 4;
      this.textMinTerm.TextAlign = HorizontalAlignment.Right;
      this.textMinTerm.KeyPress += new KeyPressEventHandler(this.numericalField_KeyPress);
      this.txtAmount.Location = new Point(148, 133);
      this.txtAmount.MaxLength = 15;
      this.txtAmount.Name = "txtAmount";
      this.txtAmount.Size = new Size(99, 20);
      this.txtAmount.TabIndex = 7;
      this.txtAmount.TextAlign = HorizontalAlignment.Right;
      this.txtAmount.KeyPress += new KeyPressEventHandler(this.numericalField_KeyPress);
      this.txtAmount.KeyUp += new KeyEventHandler(this.numericalField_KeyUp);
      this.txtAmount.Leave += new EventHandler(this.numericalField_Leave);
      this.txtAmount2.Location = new Point(148, 159);
      this.txtAmount2.MaxLength = 15;
      this.txtAmount2.Name = "txtAmount2";
      this.txtAmount2.Size = new Size(99, 20);
      this.txtAmount2.TabIndex = 9;
      this.txtAmount2.TextAlign = HorizontalAlignment.Right;
      this.txtAmount2.KeyPress += new KeyPressEventHandler(this.numericalField_KeyPress);
      this.txtAmount2.KeyUp += new KeyEventHandler(this.numericalField_KeyUp);
      this.txtAmount2.Leave += new EventHandler(this.numericalField_Leave);
      this.lblAmount2.AutoSize = true;
      this.lblAmount2.Location = new Point(9, 162);
      this.lblAmount2.Name = "lblAmount2";
      this.lblAmount2.Size = new Size(52, 13);
      this.lblAmount2.TabIndex = 20;
      this.lblAmount2.Text = "$ Amount";
      this.lblMinAmount.AutoSize = true;
      this.lblMinAmount.Location = new Point(9, 188);
      this.lblMinAmount.Name = "lblMinAmount";
      this.lblMinAmount.Size = new Size(57, 13);
      this.lblMinAmount.TabIndex = 22;
      this.lblMinAmount.Text = "Minimum $";
      this.txtMaxAmt.Location = new Point(147, 211);
      this.txtMaxAmt.MaxLength = 15;
      this.txtMaxAmt.Name = "txtMaxAmt";
      this.txtMaxAmt.Size = new Size(100, 20);
      this.txtMaxAmt.TabIndex = 11;
      this.txtMaxAmt.TextAlign = HorizontalAlignment.Right;
      this.txtMaxAmt.KeyPress += new KeyPressEventHandler(this.numericalField_KeyPress);
      this.txtMaxAmt.KeyUp += new KeyEventHandler(this.numericalField_KeyUp);
      this.txtMaxAmt.Leave += new EventHandler(this.numericalField_Leave);
      this.lblMaxAmount.AutoSize = true;
      this.lblMaxAmount.Location = new Point(9, 214);
      this.lblMaxAmount.Name = "lblMaxAmount";
      this.lblMaxAmount.Size = new Size(60, 13);
      this.lblMaxAmount.TabIndex = 24;
      this.lblMaxAmount.Text = "Maximum $";
      this.txtTriggerField.Location = new Point(145, 237);
      this.txtTriggerField.MaxLength = 50;
      this.txtTriggerField.Name = "txtTriggerField";
      this.txtTriggerField.ReadOnly = true;
      this.txtTriggerField.Size = new Size(103, 20);
      this.txtTriggerField.TabIndex = 12;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(265, 110);
      this.label1.Name = "label1";
      this.label1.Size = new Size(53, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Rounding";
      this.cmbRounding.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbRounding.FormattingEnabled = true;
      this.cmbRounding.Items.AddRange(new object[2]
      {
        (object) "",
        (object) "To Nearest $"
      });
      this.cmbRounding.Location = new Point(379, 107);
      this.cmbRounding.Name = "cmbRounding";
      this.cmbRounding.Size = new Size(115, 21);
      this.cmbRounding.TabIndex = 6;
      this.btnSave.Location = new Point(340, 466);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new Size(75, 24);
      this.btnSave.TabIndex = 48;
      this.btnSave.Text = "&Save";
      this.btnSave.Click += new EventHandler(this.btnSave_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(419, 466);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 24);
      this.btnCancel.TabIndex = 49;
      this.btnCancel.Text = "&Cancel";
      this.tabPlan.Controls.Add((Control) this.tabPageBranch);
      this.tabPlan.Controls.Add((Control) this.tabPageLO);
      this.tabPlan.Controls.Add((Control) this.tabPageBroker);
      this.tabPlan.Location = new Point(12, 288);
      this.tabPlan.Name = "tabPlan";
      this.tabPlan.SelectedIndex = 0;
      this.tabPlan.Size = new Size(482, 172);
      this.tabPlan.TabIndex = 50;
      this.tabPlan.SelectedIndexChanged += new EventHandler(this.tabPlanIndexChanged);
      this.tabPageBranch.Controls.Add((Control) this.gridViewBranch);
      this.tabPageBranch.Location = new Point(4, 22);
      this.tabPageBranch.Name = "tabPageBranch";
      this.tabPageBranch.Padding = new Padding(3);
      this.tabPageBranch.Size = new Size(474, 146);
      this.tabPageBranch.TabIndex = 0;
      this.tabPageBranch.Text = "Branches";
      this.tabPageBranch.UseVisualStyleBackColor = true;
      this.gridViewBranch.BorderStyle = BorderStyle.None;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "Column1";
      gvColumn1.SortMethod = GVSortMethod.Numeric;
      gvColumn1.Text = "Organization Name";
      gvColumn1.Width = 110;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "Column2";
      gvColumn2.SortMethod = GVSortMethod.Numeric;
      gvColumn2.Text = "Street Address";
      gvColumn2.Width = 170;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "Column3";
      gvColumn3.SortMethod = GVSortMethod.Numeric;
      gvColumn3.Text = "State";
      gvColumn3.Width = 100;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "Column4";
      gvColumn4.SortMethod = GVSortMethod.Numeric;
      gvColumn4.Text = "Zip Code";
      gvColumn4.Width = 90;
      this.gridViewBranch.Columns.AddRange(new GVColumn[4]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4
      });
      this.gridViewBranch.Dock = DockStyle.Fill;
      this.gridViewBranch.Location = new Point(3, 3);
      this.gridViewBranch.Name = "gridViewBranch";
      this.gridViewBranch.Size = new Size(468, 140);
      this.gridViewBranch.SortOption = GVSortOption.None;
      this.gridViewBranch.TabIndex = 12;
      this.tabPageLO.Controls.Add((Control) this.gridViewLO);
      this.tabPageLO.Location = new Point(4, 22);
      this.tabPageLO.Name = "tabPageLO";
      this.tabPageLO.Padding = new Padding(3);
      this.tabPageLO.Size = new Size(474, 146);
      this.tabPageLO.TabIndex = 1;
      this.tabPageLO.Text = "Loan Officers";
      this.tabPageLO.UseVisualStyleBackColor = true;
      this.gridViewLO.BorderStyle = BorderStyle.None;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "Column1";
      gvColumn5.SortMethod = GVSortMethod.Numeric;
      gvColumn5.Text = "Name";
      gvColumn5.Width = 110;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "Column5";
      gvColumn6.Text = "Branch";
      gvColumn6.Width = 100;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "Column2";
      gvColumn7.SortMethod = GVSortMethod.Numeric;
      gvColumn7.Text = "Street Address";
      gvColumn7.Width = 170;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "Column3";
      gvColumn8.SortMethod = GVSortMethod.Numeric;
      gvColumn8.Text = "State";
      gvColumn8.Width = 100;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "Column4";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Text = "Zip Code";
      gvColumn9.Width = 90;
      this.gridViewLO.Columns.AddRange(new GVColumn[5]
      {
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9
      });
      this.gridViewLO.Dock = DockStyle.Fill;
      this.gridViewLO.Location = new Point(3, 3);
      this.gridViewLO.Name = "gridViewLO";
      this.gridViewLO.Size = new Size(468, 140);
      this.gridViewLO.SortOption = GVSortOption.None;
      this.gridViewLO.TabIndex = 13;
      this.tabPageBroker.Controls.Add((Control) this.gridViewBroker);
      this.tabPageBroker.Location = new Point(4, 22);
      this.tabPageBroker.Name = "tabPageBroker";
      this.tabPageBroker.Padding = new Padding(3);
      this.tabPageBroker.Size = new Size(474, 146);
      this.tabPageBroker.TabIndex = 2;
      this.tabPageBroker.Text = "Brokers";
      this.tabPageBroker.UseVisualStyleBackColor = true;
      this.gridViewBroker.BorderStyle = BorderStyle.None;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "Column1";
      gvColumn10.SortMethod = GVSortMethod.Numeric;
      gvColumn10.Text = "Name";
      gvColumn10.Width = 110;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "Column2";
      gvColumn11.SortMethod = GVSortMethod.Numeric;
      gvColumn11.Text = "Street Address";
      gvColumn11.Width = 170;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "Column3";
      gvColumn12.SortMethod = GVSortMethod.Numeric;
      gvColumn12.Text = "State";
      gvColumn12.Width = 100;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "Column4";
      gvColumn13.SortMethod = GVSortMethod.Numeric;
      gvColumn13.Text = "Zip Code";
      gvColumn13.Width = 90;
      this.gridViewBroker.Columns.AddRange(new GVColumn[4]
      {
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13
      });
      this.gridViewBroker.Dock = DockStyle.Fill;
      this.gridViewBroker.Location = new Point(3, 3);
      this.gridViewBroker.Name = "gridViewBroker";
      this.gridViewBroker.Size = new Size(468, 140);
      this.gridViewBroker.SortOption = GVSortOption.None;
      this.gridViewBroker.TabIndex = 14;
      this.lblPlan.AutoSize = true;
      this.lblPlan.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblPlan.Location = new Point(9, 272);
      this.lblPlan.Name = "lblPlan";
      this.lblPlan.Size = new Size(68, 13);
      this.lblPlan.TabIndex = 51;
      this.lblPlan.Text = "Plan Users";
      this.txtMinAmt.Location = new Point(147, 185);
      this.txtMinAmt.MaxLength = 15;
      this.txtMinAmt.Name = "txtMinAmt";
      this.txtMinAmt.Size = new Size(100, 20);
      this.txtMinAmt.TabIndex = 10;
      this.txtMinAmt.TextAlign = HorizontalAlignment.Right;
      this.txtMinAmt.KeyPress += new KeyPressEventHandler(this.numericalField_KeyPress);
      this.txtMinAmt.KeyUp += new KeyEventHandler(this.numericalField_KeyUp);
      this.txtMinAmt.Leave += new EventHandler(this.numericalField_Leave);
      this.txtActivationDate.Enabled = false;
      this.txtActivationDate.Location = new Point(148, 107);
      this.txtActivationDate.Name = "txtActivationDate";
      this.txtActivationDate.Size = new Size(100, 20);
      this.txtActivationDate.TabIndex = 5;
      this.txtActivationDate.TabStop = false;
      this.cmbAmount.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbAmount.FormattingEnabled = true;
      this.cmbAmount.Items.AddRange(new object[3]
      {
        (object) "",
        (object) "Total Loan",
        (object) "Base Loan"
      });
      this.cmbAmount.Location = new Point(268, 133);
      this.cmbAmount.Name = "cmbAmount";
      this.cmbAmount.Size = new Size(101, 21);
      this.cmbAmount.TabIndex = 8;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(249, 136);
      this.label2.Name = "label2";
      this.label2.Size = new Size(16, 13);
      this.label2.TabIndex = 52;
      this.label2.Text = "of";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(506, 497);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtActivationDate);
      this.Controls.Add((Control) this.lblPlan);
      this.Controls.Add((Control) this.tabPlan);
      this.Controls.Add((Control) this.btnSave);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.txtTriggerField);
      this.Controls.Add((Control) this.txtMaxAmt);
      this.Controls.Add((Control) this.lblMaxAmount);
      this.Controls.Add((Control) this.txtMinAmt);
      this.Controls.Add((Control) this.lblMinAmount);
      this.Controls.Add((Control) this.txtAmount2);
      this.Controls.Add((Control) this.lblAmount2);
      this.Controls.Add((Control) this.cmbRounding);
      this.Controls.Add((Control) this.cmbAmount);
      this.Controls.Add((Control) this.txtAmount);
      this.Controls.Add((Control) this.textMinTerm);
      this.Controls.Add((Control) this.cmbType);
      this.Controls.Add((Control) this.txtDescription);
      this.Controls.Add((Control) this.txtName);
      this.Controls.Add((Control) this.lblTriggerField);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblAmount);
      this.Controls.Add((Control) this.lblMinTerm);
      this.Controls.Add((Control) this.lblActivation);
      this.Controls.Add((Control) this.lblType);
      this.Controls.Add((Control) this.lblDescription);
      this.Controls.Add((Control) this.lblName);
      this.Controls.Add((Control) this.chkStatus);
      this.Controls.Add((Control) this.lblStatus);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LOCompGroupDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "LO Comp Plan Details";
      this.KeyDown += new KeyEventHandler(this.Help_KeyDown);
      this.tabPlan.ResumeLayout(false);
      this.tabPageBranch.ResumeLayout(false);
      this.tabPageLO.ResumeLayout(false);
      this.tabPageBroker.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void initForm()
    {
      if (this.lcp == null)
      {
        this.cmbType.SelectedIndex = this.loanCompDefaultPlan.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (this.loanCompDefaultPlan.PlanType == LoanCompPlanType.Broker ? 2 : (this.loanCompDefaultPlan.PlanType == LoanCompPlanType.Both ? 3 : 0));
        this.cmbRounding.SelectedIndex = this.loanCompDefaultPlan.RoundingMethod > 1 ? 1 : 0;
        this.textMinTerm.Text = this.loanCompDefaultPlan.MinTermDays == 0 ? "" : this.loanCompDefaultPlan.MinTermDays.ToString();
        this.txtTriggerField.Text = this.loanCompDefaultPlan.TriggerField;
        this.txtName_TextChanged((object) null, (EventArgs) null);
      }
      else
      {
        this.txtName.Text = this.lcp.Name;
        this.txtDescription.Text = this.lcp.Description;
        this.cmbType.SelectedIndex = this.lcp.PlanType == LoanCompPlanType.LoanOfficer ? 1 : (this.lcp.PlanType == LoanCompPlanType.Broker ? 2 : (this.lcp.PlanType == LoanCompPlanType.Both ? 3 : 0));
        this.chkStatus.Checked = this.lcp.Status;
        this.txtActivationDate.Text = !(this.lcp.ActivationDate != DateTime.MinValue) || !(this.lcp.ActivationDate != DateTime.MaxValue) ? "" : this.lcp.ActivationDate.ToString("MM/dd/yyyy");
        this.textMinTerm.Text = this.lcp.MinTermDays != 0 ? this.lcp.MinTermDays.ToString() : "";
        TextBox txtAmount = this.txtAmount;
        Decimal num;
        string str1;
        if (!(this.lcp.PercentAmt != 0M))
        {
          str1 = "";
        }
        else
        {
          num = this.lcp.PercentAmt;
          str1 = num.ToString("N5");
        }
        txtAmount.Text = str1;
        this.cmbAmount.SelectedIndex = this.lcp.PercentAmtBase;
        this.cmbRounding.SelectedIndex = this.lcp.RoundingMethod > 1 ? 1 : 0;
        TextBox txtAmount2 = this.txtAmount2;
        string str2;
        if (!(this.lcp.DollarAmount != 0M))
        {
          str2 = "";
        }
        else
        {
          num = this.lcp.DollarAmount;
          str2 = num.ToString("N2");
        }
        txtAmount2.Text = str2;
        TextBox txtMinAmt = this.txtMinAmt;
        string str3;
        if (!(this.lcp.MinDollarAmount != 0M))
        {
          str3 = "";
        }
        else
        {
          num = this.lcp.MinDollarAmount;
          str3 = num.ToString("N2");
        }
        txtMinAmt.Text = str3;
        TextBox txtMaxAmt = this.txtMaxAmt;
        string str4;
        if (!(this.lcp.MaxDollarAmount != 0M))
        {
          str4 = "";
        }
        else
        {
          num = this.lcp.MaxDollarAmount;
          str4 = num.ToString("N2");
        }
        txtMaxAmt.Text = str4;
        this.txtTriggerField.Text = this.loanCompDefaultPlan != null ? this.loanCompDefaultPlan.TriggerField : "";
        this.tabPlanIndexChanged((object) null, (EventArgs) null);
        this.chkStatus_CheckedChanged((object) null, (EventArgs) null);
      }
    }

    private void Help_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.F1)
        return;
      this.ShowHelp();
    }

    public void ShowHelp()
    {
      JedHelp.ShowHelp((Control) this, SystemSettings.HelpFile, "Setup\\Loan Originator Compensation");
    }

    private void tabPlanIndexChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      if (this.tabPlan.SelectedTab == this.tabPageBranch && this.gridViewBranch.Items.Count == 0)
      {
        if (this.orgsUseThisPlan == null)
          this.orgsUseThisPlan = this.lcp != null ? this.session.ConfigurationManager.GetOrganizationsUsingCompPlan(this.lcp.Id) : (List<OrgInfo>) null;
        if (this.orgsUseThisPlan == null || this.orgsUseThisPlan.Count == 0)
          return;
        this.gridViewBranch.BeginUpdate();
        foreach (OrgInfo orgInfo in this.orgsUseThisPlan)
          this.buildPlanUserItem(this.gridViewBranch, orgInfo.OrgName, (string) null, orgInfo.CompanyAddress.Street1, orgInfo.CompanyAddress.State, orgInfo.CompanyAddress.Zip);
        this.gridViewBranch.EndUpdate();
      }
      else if (this.tabPlan.SelectedTab == this.tabPageLO && this.gridViewLO.Items.Count == 0)
      {
        if (this.userWithOrgUseThisPlan == null)
          this.userWithOrgUseThisPlan = this.lcp != null ? this.session.ConfigurationManager.GetUsersUsingCompPlan(this.lcp.Id) : (List<object[]>) null;
        if (this.userWithOrgUseThisPlan == null || this.userWithOrgUseThisPlan.Count == 0)
          return;
        this.gridViewLO.BeginUpdate();
        string empty = string.Empty;
        foreach (object[] objArray in this.userWithOrgUseThisPlan)
        {
          UserInfo userInfo = (UserInfo) objArray[0];
          OrgInfo orgInfo = (OrgInfo) objArray[1];
          string addr = orgInfo != null ? orgInfo.CompanyAddress.Street1 + " " + orgInfo.CompanyAddress.Street2 : string.Empty;
          this.buildPlanUserItem(this.gridViewLO, userInfo.FullName, orgInfo.OrgName, addr, orgInfo.CompanyAddress.State, orgInfo.CompanyAddress.Zip);
        }
        this.gridViewLO.EndUpdate();
      }
      else if (this.tabPlan.SelectedTab == this.tabPageBroker && this.gridViewBroker.Items.Count == 0)
      {
        this.gridViewBroker.Items.Clear();
        if (this.externalCompaniesUseThisPlan == null)
          this.externalCompaniesUseThisPlan = this.lcp != null ? this.session.ConfigurationManager.GetExternalOrganizationsUsingCompPlan(this.lcp.Id) : (List<ExternalOriginatorManagementData>) null;
        if (this.externalCompaniesUseThisPlan == null || this.externalCompaniesUseThisPlan.Count == 0)
          return;
        this.gridViewBroker.BeginUpdate();
        foreach (ExternalOriginatorManagementData originatorManagementData in this.externalCompaniesUseThisPlan)
          this.buildPlanUserItem(this.gridViewBroker, originatorManagementData.OrganizationName, (string) null, originatorManagementData.Address, originatorManagementData.State, originatorManagementData.Zip);
        this.gridViewBroker.EndUpdate();
      }
      Cursor.Current = Cursors.Default;
    }

    private bool checkWhoIsUsingCurrentPlan()
    {
      if (this.lcp == null)
        return false;
      if (this.orgsUseThisPlan == null)
        this.orgsUseThisPlan = this.session.ConfigurationManager.GetOrganizationsUsingCompPlan(this.lcp.Id);
      if (this.userWithOrgUseThisPlan == null)
        this.userWithOrgUseThisPlan = this.session.ConfigurationManager.GetUsersUsingCompPlan(this.lcp.Id);
      if (this.externalCompaniesUseThisPlan == null)
        this.externalCompaniesUseThisPlan = this.session.ConfigurationManager.GetExternalOrganizationsUsingCompPlan(this.lcp.Id);
      return this.orgsUseThisPlan != null && this.orgsUseThisPlan.Count > 0 || this.userWithOrgUseThisPlan != null && this.userWithOrgUseThisPlan.Count > 0 || this.externalCompaniesUseThisPlan != null && this.externalCompaniesUseThisPlan.Count > 0;
    }

    private void buildPlanUserItem(
      GridView view,
      string id,
      string branch,
      string addr,
      string state,
      string zip)
    {
      GVItem gvItem = new GVItem(id);
      if (branch != null)
        gvItem.SubItems.Add((object) branch);
      gvItem.SubItems.Add((object) addr);
      gvItem.SubItems.Add((object) state);
      gvItem.SubItems.Add((object) zip);
      view.Items.Add(gvItem);
    }

    private void chkStatus_CheckedChanged(object sender, EventArgs e)
    {
      if (this.chkStatus.Checked && (this.txtActivationDate.Text.Trim() == string.Empty || this.lcp == null || this.lcp.Id == -1))
        this.txtActivationDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
      else if (!this.chkStatus.Checked && (this.lcp == null || this.lcp.Id == -1))
        this.txtActivationDate.Text = "";
      TextBox txtName = this.txtName;
      TextBox txtDescription = this.txtDescription;
      TextBox textMinTerm = this.textMinTerm;
      TextBox txtAmount = this.txtAmount;
      TextBox txtAmount2 = this.txtAmount2;
      TextBox txtMinAmt = this.txtMinAmt;
      bool flag1;
      this.txtMaxAmt.ReadOnly = flag1 = this.chkStatus.Checked || this.checkWhoIsUsingCurrentPlan();
      int num1;
      bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
      txtMinAmt.ReadOnly = num1 != 0;
      int num2;
      bool flag3 = (num2 = flag2 ? 1 : 0) != 0;
      txtAmount2.ReadOnly = num2 != 0;
      int num3;
      bool flag4 = (num3 = flag3 ? 1 : 0) != 0;
      txtAmount.ReadOnly = num3 != 0;
      int num4;
      bool flag5 = (num4 = flag4 ? 1 : 0) != 0;
      textMinTerm.ReadOnly = num4 != 0;
      int num5;
      bool flag6 = (num5 = flag5 ? 1 : 0) != 0;
      txtDescription.ReadOnly = num5 != 0;
      int num6 = flag6 ? 1 : 0;
      txtName.ReadOnly = num6 != 0;
      ComboBox cmbType = this.cmbType;
      ComboBox cmbRounding = this.cmbRounding;
      bool flag7;
      this.cmbAmount.Enabled = flag7 = !this.chkStatus.Checked && !this.checkWhoIsUsingCurrentPlan();
      int num7;
      bool flag8 = (num7 = flag7 ? 1 : 0) != 0;
      cmbRounding.Enabled = num7 != 0;
      int num8 = flag8 ? 1 : 0;
      cmbType.Enabled = num8 != 0;
      if (sender == null || !this.chkStatus.Checked)
        return;
      int num9 = (int) Utils.Dialog((IWin32Window) this, "For audit purposes, compensation plans cannot be modified once they are activated and assigned to an originator. Be sure to make any needed changes here before assigning the plan to an originator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void numericalField_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      TextBox textBox = (TextBox) sender;
      if (e.KeyChar.Equals('.') && textBox.Name == "textMinTerm")
        e.Handled = true;
      else if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void numericalField_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.Name == "txtAmount")
      {
        textBox.Text = textBox.Text.Trim() != string.Empty ? Utils.ParseDecimal((object) textBox.Text, 0M).ToString("N5") : "";
      }
      else
      {
        if (!(textBox.Text.Trim() != string.Empty))
          return;
        textBox.Text = Utils.ParseDecimal((object) textBox.Text, 0M).ToString("N2");
      }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      if (this.txtName.Text.Trim() == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The name field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
      }
      else if (this.txtName.Text.Trim().Length > 100)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The number of characters in the name field exceed 100 characters.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtName.Focus();
      }
      else if (string.IsNullOrEmpty(this.cmbType.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Loan Officer/Broker Value field cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbType.Focus();
      }
      else if (this.textMinTerm.Text != string.Empty && Utils.ParseInt((object) this.textMinTerm.Text) > 36500)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Minimum Term # Days field cannot be greater than 36500.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.textMinTerm.Focus();
      }
      else if (Utils.ParseDouble((object) this.txtAmount.Text) > 100.0)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The % Amount field cannot be greater than 100.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtAmount.Focus();
      }
      else if (!string.IsNullOrEmpty(this.txtAmount.Text) && Utils.ParseDouble((object) this.txtAmount.Text) != 0.0 && this.cmbAmount.Text == string.Empty)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The % Amount type cannot be blank.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.cmbAmount.Focus();
      }
      else if (!string.IsNullOrEmpty(this.txtMinAmt.Text) && !string.IsNullOrEmpty(this.txtMaxAmt.Text) && Utils.ParseDouble((object) this.txtMinAmt.Text) > Utils.ParseDouble((object) this.txtMaxAmt.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Minimum Amount cannot be greater than Maximum Amount.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.txtMinAmt.Focus();
      }
      else
      {
        if (!string.IsNullOrEmpty(this.txtTriggerField.Text))
        {
          FieldDefinition field = EncompassFields.GetField(this.txtTriggerField.Text);
          if (field == null || !field.IsDateValued())
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "The Trigger Basis field '" + this.txtTriggerField.Text + "' is not a date field. Please change field value in the Default Settings first.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return;
          }
        }
        if (this.lcp == null)
          this.lcp = new LoanCompPlan();
        this.lcp.Name = string.IsNullOrEmpty(this.txtName.Text) ? "" : this.txtName.Text.Trim();
        this.lcp.Description = string.IsNullOrEmpty(this.txtDescription.Text) ? "" : this.txtDescription.Text.Trim();
        this.lcp.PlanType = (LoanCompPlanType) Enum.ToObject(typeof (LoanCompPlanType), this.cmbType.SelectedIndex);
        this.lcp.Status = this.chkStatus.Checked;
        this.lcp.ActivationDate = string.IsNullOrEmpty(this.txtActivationDate.Text) ? DateTime.MaxValue : Utils.ParseDate((object) this.txtActivationDate.Text);
        this.lcp.MinTermDays = string.IsNullOrEmpty(this.textMinTerm.Text) ? 0 : Utils.ParseInt((object) this.textMinTerm.Text, 0);
        this.lcp.PercentAmt = string.IsNullOrEmpty(this.txtAmount.Text) ? 0M : Utils.ParseDecimal((object) this.txtAmount.Text);
        this.lcp.PercentAmtBase = this.cmbAmount.SelectedItem != null ? this.cmbAmount.SelectedIndex : 0;
        this.lcp.RoundingMethod = this.cmbRounding.SelectedItem == null || this.cmbRounding.SelectedIndex != 1 ? 1 : 2;
        this.lcp.DollarAmount = string.IsNullOrEmpty(this.txtAmount2.Text) ? 0M : Utils.ParseDecimal((object) this.txtAmount2.Text.Trim());
        this.lcp.MinDollarAmount = string.IsNullOrEmpty(this.txtMinAmt.Text) ? 0M : Utils.ParseDecimal((object) this.txtMinAmt.Text.Trim());
        this.lcp.MaxDollarAmount = string.IsNullOrEmpty(this.txtMaxAmt.Text) ? 0M : Utils.ParseDecimal((object) this.txtMaxAmt.Text.Trim());
        this.lcp.TriggerField = string.IsNullOrEmpty(this.txtTriggerField.Text) ? "" : this.txtTriggerField.Text.Trim();
        int num1 = this.lcp.Id != -1 ? Session.ConfigurationManager.UpdateCompPlan(this.lcp) : Session.ConfigurationManager.AddCompPlan(this.lcp);
        if (num1 == -1)
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Loan Compensation list already contains name '" + this.txtName.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.txtName.Focus();
        }
        else
        {
          this.lcp.Id = num1;
          this.DialogResult = DialogResult.OK;
        }
      }
    }

    public LoanCompPlan CurrentLoanCompPlans => this.lcp;

    private void txtName_TextChanged(object sender, EventArgs e)
    {
      this.btnSave.Enabled = this.txtName.Text.Trim().Length > 0;
    }

    private void numericalField_KeyUp(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, textBox.Name == "txtAmount" ? FieldFormat.DECIMAL_5 : FieldFormat.DECIMAL_2, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }
  }
}
