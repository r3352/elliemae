// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.FundingWorksheet
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using EllieMae.Encompass.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class FundingWorksheet : CustomUserControl, IRefreshContents
  {
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Panel panelTop;
    private ToolTip fieldToolTip;
    private System.Windows.Forms.Panel panelBottom;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxDeduction;
    private System.Windows.Forms.TextBox textBoxWireAmount;
    private System.Windows.Forms.Label labelWireAmount;
    private System.Windows.Forms.Panel panelLowTop;
    private System.Windows.Forms.Panel panelForListView;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private IContainer components;
    private LoanData loan;
    private LoanScreen freeScreen;
    private System.Windows.Forms.CheckBox checkBoxHide;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox txtOverwireAmount;
    private System.Windows.Forms.Button buttonHUDP1;
    private System.Windows.Forms.Button btnTemplate;
    private System.Windows.Forms.CheckBox chkDeduct;
    private PictureBox pboxDownArrow;
    private PictureBox pboxAsterisk;
    private System.Windows.Forms.TextBox textBoxLoanAmount;
    private CollapsibleSplitter collapsibleSplitter1;
    private BorderPanel borderPanel1;
    private BorderPanel borderPanel2;
    private GridView gridFees;
    private PictureBox pictureBox1;
    private PopupBusinessRules popupRules;
    private GridView gridFees2015;
    private Sessions.Session session;
    private System.Windows.Forms.TextBox textBoxLenderCredit;
    private System.Windows.Forms.Label labelLenderCredit;
    private FieldLockButton lBtnLenderCredits;
    private int sellerColumn = 3;

    public FundingWorksheet(LoanData loan, Sessions.Session session)
    {
      this.loan = loan;
      this.session = session;
      this.InitializeComponent();
      if (Utils.CheckIf2015RespaTila(loan.GetField("3969")))
      {
        this.gridFees2015.Visible = true;
        this.gridFees.Visible = false;
        this.gridFees2015.Dock = DockStyle.Fill;
        this.buttonHUDP1.Text = "Transaction Summary";
        this.buttonHUDP1.Width = 136;
        this.sellerColumn = 5;
      }
      else
      {
        this.gridFees.Visible = true;
        this.gridFees2015.Visible = false;
        this.gridFees.Dock = DockStyle.Fill;
      }
      this.initialPage();
    }

    private void initialPage()
    {
      if (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")))
      {
        this.labelLenderCredit.Visible = this.textBoxLenderCredit.Visible = this.lBtnLenderCredits.Visible = true;
        this.textBoxWireAmount.Top = this.labelLenderCredit.Top + 18;
        this.labelWireAmount.Top = this.labelLenderCredit.Top + 22;
        if (this.loan.IsLocked("4083"))
          this.toggleLockButton((object) this.textBoxLenderCredit, (object) this.lBtnLenderCredits);
      }
      else
      {
        this.labelLenderCredit.Visible = this.textBoxLenderCredit.Visible = this.lBtnLenderCredits.Visible = false;
        this.textBoxWireAmount.Top = this.labelWireAmount.Top = this.labelLenderCredit.Top;
      }
      this.textBoxLoanAmount.Text = this.loan.GetField("2");
      this.txtOverwireAmount.Text = this.loan.GetField("2005");
      this.textBoxLenderCredit.Text = this.loan.GetField("4083");
      this.chkDeduct.Checked = this.loan.GetField("2833") == "Y";
      this.checkBoxHide_CheckedChanged((object) null, (EventArgs) null);
      this.freeScreen = new LoanScreen(this.session);
      string str = "FUNDERWORKSHEET";
      this.freeScreen.LoadForm(new InputFormInfo(str, str));
      this.freeScreen.RemoveTitle();
      this.freeScreen.RemoveBorder();
      this.panelTop.Controls.Add((System.Windows.Forms.Control) this.freeScreen);
      this.freeScreen.Focus();
      if (this.loan.FieldForGoTo != string.Empty)
      {
        this.freeScreen.SetGoToFieldFocus(this.loan.FieldForGoTo, 1);
        this.loan.FieldForGoTo = string.Empty;
      }
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBoxLoanAmount, "2");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBoxDeduction, "1989");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBoxLenderCredit, "4083");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.textBoxWireAmount, "1990");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.txtOverwireAmount, "2005");
      this.fieldToolTip.SetToolTip((System.Windows.Forms.Control) this.chkDeduct, "2833");
      this.doCalculation();
      if (this.gridFees2015.Visible)
        this.gridFees2015.SubItemCheck += new GVSubItemEventHandler(this.gridFees2015_SubitemCheck);
      else
        this.gridFees.SubItemCheck += new GVSubItemEventHandler(this.gridFees_SubitemCheck);
      if (this.loan != null && this.loan.IsFieldReadOnly("2005"))
        this.txtOverwireAmount.ReadOnly = true;
      if (this.loan != null && this.loan.IsFieldReadOnly("2833"))
        this.chkDeduct.Enabled = false;
      ResourceManager resources = new ResourceManager(typeof (FundingWorksheet));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (System.Drawing.Image) resources.GetObject("pboxAsterisk.Image"), (System.Drawing.Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      this.popupRules.SetBusinessRules((object) this.textBoxLoanAmount, "2");
      this.popupRules.SetBusinessRules((object) this.textBoxDeduction, "1989");
      this.popupRules.SetBusinessRules((object) this.textBoxLenderCredit, "4083");
      this.popupRules.SetBusinessRules((object) this.lBtnLenderCredits, "LOCKBUTTON_4083");
      this.popupRules.SetBusinessRules((object) this.textBoxWireAmount, "1990");
      this.popupRules.SetBusinessRules((object) this.txtOverwireAmount, "2005");
      this.popupRules.SetBusinessRules((object) this.chkDeduct, "2833");
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FundingWorksheet));
      this.panel1 = new System.Windows.Forms.Panel();
      this.borderPanel1 = new BorderPanel();
      this.panelForListView = new System.Windows.Forms.Panel();
      this.borderPanel2 = new BorderPanel();
      this.gridFees2015 = new GridView();
      this.gridFees = new GridView();
      this.panelLowTop = new System.Windows.Forms.Panel();
      this.pboxDownArrow = new PictureBox();
      this.pboxAsterisk = new PictureBox();
      this.chkDeduct = new System.Windows.Forms.CheckBox();
      this.btnTemplate = new System.Windows.Forms.Button();
      this.label5 = new System.Windows.Forms.Label();
      this.txtOverwireAmount = new System.Windows.Forms.TextBox();
      this.textBoxLoanAmount = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.panelBottom = new System.Windows.Forms.Panel();
      this.lBtnLenderCredits = new FieldLockButton();
      this.textBoxLenderCredit = new System.Windows.Forms.TextBox();
      this.labelLenderCredit = new System.Windows.Forms.Label();
      this.buttonHUDP1 = new System.Windows.Forms.Button();
      this.labelWireAmount = new System.Windows.Forms.Label();
      this.textBoxWireAmount = new System.Windows.Forms.TextBox();
      this.textBoxDeduction = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkBoxHide = new System.Windows.Forms.CheckBox();
      this.collapsibleSplitter1 = new CollapsibleSplitter();
      this.panelTop = new System.Windows.Forms.Panel();
      this.pictureBox1 = new PictureBox();
      this.fieldToolTip = new ToolTip(this.components);
      this.panel1.SuspendLayout();
      this.borderPanel1.SuspendLayout();
      this.panelForListView.SuspendLayout();
      this.borderPanel2.SuspendLayout();
      this.panelLowTop.SuspendLayout();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      this.panelBottom.SuspendLayout();
      this.panelTop.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.SuspendLayout();
      this.panel1.BackColor = SystemColors.ControlLight;
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.borderPanel1);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.collapsibleSplitter1);
      this.panel1.Controls.Add((System.Windows.Forms.Control) this.panelTop);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(804, 1238);
      this.panel1.TabIndex = 29;
      this.borderPanel1.Borders = AnchorStyles.Top | AnchorStyles.Right;
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.panelForListView);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.panelLowTop);
      this.borderPanel1.Controls.Add((System.Windows.Forms.Control) this.panelBottom);
      this.borderPanel1.Dock = DockStyle.Fill;
      this.borderPanel1.Location = new Point(0, 857);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(804, 381);
      this.borderPanel1.TabIndex = 71;
      this.panelForListView.Controls.Add((System.Windows.Forms.Control) this.borderPanel2);
      this.panelForListView.Dock = DockStyle.Fill;
      this.panelForListView.Location = new Point(0, 89);
      this.panelForListView.Name = "panelForListView";
      this.panelForListView.Size = new Size(803, 211);
      this.panelForListView.TabIndex = 12;
      this.borderPanel2.Borders = AnchorStyles.Top | AnchorStyles.Bottom;
      this.borderPanel2.Controls.Add((System.Windows.Forms.Control) this.gridFees2015);
      this.borderPanel2.Controls.Add((System.Windows.Forms.Control) this.gridFees);
      this.borderPanel2.Dock = DockStyle.Fill;
      this.borderPanel2.Location = new Point(0, 0);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(803, 211);
      this.borderPanel2.TabIndex = 71;
      this.gridFees2015.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn1.CheckBoxes = true;
      gvColumn1.ImageIndex = -1;
      gvColumn1.Name = "ColumnCheck";
      gvColumn1.Text = "";
      gvColumn1.Width = 30;
      gvColumn2.ImageIndex = -1;
      gvColumn2.Name = "ColumnCDLine";
      gvColumn2.Text = "CD Line #";
      gvColumn2.Width = 64;
      gvColumn3.ImageIndex = -1;
      gvColumn3.Name = "ColumnHUDLine";
      gvColumn3.Text = "Itemization Line #";
      gvColumn3.Width = 98;
      gvColumn4.ImageIndex = -1;
      gvColumn4.Name = "ColumnDesc";
      gvColumn4.Text = "Fee Description";
      gvColumn4.Width = 140;
      gvColumn5.ImageIndex = -1;
      gvColumn5.Name = "ColumnPayee";
      gvColumn5.Text = "Payee";
      gvColumn5.Width = 120;
      gvColumn6.ImageIndex = -1;
      gvColumn6.Name = "ColumnPaidBy";
      gvColumn6.Text = "Paid By";
      gvColumn6.Width = 70;
      gvColumn7.ImageIndex = -1;
      gvColumn7.Name = "ColumnPaidTo";
      gvColumn7.Text = "Paid To";
      gvColumn7.Width = 70;
      gvColumn8.ImageIndex = -1;
      gvColumn8.Name = "ColumnAmount";
      gvColumn8.SortMethod = GVSortMethod.Numeric;
      gvColumn8.Text = "Amount";
      gvColumn8.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn8.Width = 65;
      gvColumn9.ImageIndex = -1;
      gvColumn9.Name = "ColumnPOCBor";
      gvColumn9.SortMethod = GVSortMethod.Numeric;
      gvColumn9.Text = "POC Borrower";
      gvColumn9.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn9.Width = 74;
      gvColumn10.ImageIndex = -1;
      gvColumn10.Name = "ColumnPOCSeller";
      gvColumn10.SortMethod = GVSortMethod.Numeric;
      gvColumn10.Text = "POC Seller";
      gvColumn10.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn10.Width = 67;
      gvColumn11.ImageIndex = -1;
      gvColumn11.Name = "ColumnPOCBroker";
      gvColumn11.SortMethod = GVSortMethod.Numeric;
      gvColumn11.Text = "POC Broker";
      gvColumn11.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn11.Width = 74;
      gvColumn12.ImageIndex = -1;
      gvColumn12.Name = "ColumnPOCLender";
      gvColumn12.SortMethod = GVSortMethod.Numeric;
      gvColumn12.Text = "POC Lender";
      gvColumn12.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn12.Width = 74;
      gvColumn13.ImageIndex = -1;
      gvColumn13.Name = "ColumnPOCOther";
      gvColumn13.SortMethod = GVSortMethod.Numeric;
      gvColumn13.Text = "POC Other";
      gvColumn13.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn13.Width = 72;
      gvColumn14.ImageIndex = -1;
      gvColumn14.Name = "ColumnPACBroker";
      gvColumn14.SortMethod = GVSortMethod.Numeric;
      gvColumn14.Text = "PAC Broker";
      gvColumn14.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn14.Width = 74;
      gvColumn15.ImageIndex = -1;
      gvColumn15.Name = "ColumnPACLender";
      gvColumn15.SortMethod = GVSortMethod.Numeric;
      gvColumn15.Text = "PAC Lender";
      gvColumn15.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn15.Width = 74;
      gvColumn16.ImageIndex = -1;
      gvColumn16.Name = "ColumnPACOther";
      gvColumn16.SortMethod = GVSortMethod.Numeric;
      gvColumn16.Text = "PAC Other";
      gvColumn16.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn16.Width = 72;
      this.gridFees2015.Columns.AddRange(new GVColumn[16]
      {
        gvColumn1,
        gvColumn2,
        gvColumn3,
        gvColumn4,
        gvColumn5,
        gvColumn6,
        gvColumn7,
        gvColumn8,
        gvColumn9,
        gvColumn10,
        gvColumn11,
        gvColumn12,
        gvColumn13,
        gvColumn14,
        gvColumn15,
        gvColumn16
      });
      this.gridFees2015.Font = new Font("Arial", 8.25f);
      this.gridFees2015.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridFees2015.Location = new Point(380, 2);
      this.gridFees2015.Name = "gridFees2015";
      this.gridFees2015.Size = new Size(376, 291);
      this.gridFees2015.TabIndex = 6;
      this.gridFees.BorderStyle = System.Windows.Forms.BorderStyle.None;
      gvColumn17.CheckBoxes = true;
      gvColumn17.ImageIndex = -1;
      gvColumn17.Name = "Column1";
      gvColumn17.Text = "";
      gvColumn17.Width = 60;
      gvColumn18.ImageIndex = -1;
      gvColumn18.Name = "Column2";
      gvColumn18.Text = "Fee Description";
      gvColumn18.Width = 150;
      gvColumn19.ImageIndex = -1;
      gvColumn19.Name = "Column3";
      gvColumn19.Text = "Payee";
      gvColumn19.Width = 130;
      gvColumn20.ImageIndex = -1;
      gvColumn20.Name = "Column4";
      gvColumn20.Text = "Paid By";
      gvColumn20.Width = 70;
      gvColumn21.ImageIndex = -1;
      gvColumn21.Name = "Column5";
      gvColumn21.Text = "Paid To";
      gvColumn21.Width = 70;
      gvColumn22.ImageIndex = -1;
      gvColumn22.Name = "Column6";
      gvColumn22.Text = "Amount";
      gvColumn22.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn22.Width = 75;
      gvColumn23.ImageIndex = -1;
      gvColumn23.Name = "Column7";
      gvColumn23.Text = "POC Amt";
      gvColumn23.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn23.Width = 60;
      gvColumn24.ImageIndex = -1;
      gvColumn24.Name = "Column8";
      gvColumn24.Text = "POC Paid By";
      gvColumn24.Width = 70;
      gvColumn25.ImageIndex = -1;
      gvColumn25.Name = "Column9";
      gvColumn25.Text = "PTC Amt";
      gvColumn25.TextAlignment = ContentAlignment.MiddleRight;
      gvColumn25.Width = 60;
      gvColumn26.ImageIndex = -1;
      gvColumn26.Name = "Column10";
      gvColumn26.Text = "PTC Paid By";
      gvColumn26.Width = 70;
      this.gridFees.Columns.AddRange(new GVColumn[10]
      {
        gvColumn17,
        gvColumn18,
        gvColumn19,
        gvColumn20,
        gvColumn21,
        gvColumn22,
        gvColumn23,
        gvColumn24,
        gvColumn25,
        gvColumn26
      });
      this.gridFees.Font = new Font("Arial", 8.25f);
      this.gridFees.HotTrackingColor = Color.FromArgb(250, 248, 188);
      this.gridFees.Location = new Point(0, 1);
      this.gridFees.Name = "gridFees";
      this.gridFees.Size = new Size(337, 291);
      this.gridFees.SortOption = GVSortOption.None;
      this.gridFees.TabIndex = 5;
      this.panelLowTop.BackColor = Color.WhiteSmoke;
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.pboxDownArrow);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.pboxAsterisk);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.chkDeduct);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.btnTemplate);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.label5);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.txtOverwireAmount);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.textBoxLoanAmount);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.label4);
      this.panelLowTop.Controls.Add((System.Windows.Forms.Control) this.label3);
      this.panelLowTop.Dock = DockStyle.Top;
      this.panelLowTop.Location = new Point(0, 1);
      this.panelLowTop.Name = "panelLowTop";
      this.panelLowTop.Size = new Size(803, 88);
      this.panelLowTop.TabIndex = 2;
      this.pboxDownArrow.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(304, 46);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 70;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.pboxAsterisk.Image = (System.Drawing.Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(304, 26);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 69;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.chkDeduct.AutoSize = true;
      this.chkDeduct.Font = new Font("Arial", 8.25f);
      this.chkDeduct.Location = new Point(502, 61);
      this.chkDeduct.Name = "chkDeduct";
      this.chkDeduct.Size = new Size(236, 23);
      this.chkDeduct.TabIndex = 9;
      this.chkDeduct.Tag = (object) "2833";
      this.chkDeduct.Text = "Deduct From Broker Check";
      this.chkDeduct.UseVisualStyleBackColor = true;
      this.chkDeduct.Click += new EventHandler(this.chkDeduct_Click);
      this.btnTemplate.BackColor = SystemColors.Control;
      this.btnTemplate.Location = new Point(14, 33);
      this.btnTemplate.Name = "btnTemplate";
      this.btnTemplate.Size = new Size(96, 23);
      this.btnTemplate.TabIndex = 8;
      this.btnTemplate.Text = "Select Template";
      this.btnTemplate.UseVisualStyleBackColor = true;
      this.btnTemplate.Click += new EventHandler(this.btnTemplate_Click);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 8.25f);
      this.label5.Location = new Point(398, 40);
      this.label5.Name = "label5";
      this.label5.Size = new Size(132, 19);
      this.label5.TabIndex = 5;
      this.label5.Text = "Overwire Amount";
      this.txtOverwireAmount.Font = new Font("Arial", 8.25f);
      this.txtOverwireAmount.Location = new Point(502, 36);
      this.txtOverwireAmount.Name = "txtOverwireAmount";
      this.txtOverwireAmount.Size = new Size(132, 26);
      this.txtOverwireAmount.TabIndex = 3;
      this.txtOverwireAmount.TabStop = false;
      this.txtOverwireAmount.Tag = (object) "2005";
      this.txtOverwireAmount.TextAlign = HorizontalAlignment.Right;
      this.txtOverwireAmount.Enter += new EventHandler(this.textField_Enter);
      this.txtOverwireAmount.KeyUp += new KeyEventHandler(this.textField_KeyUp);
      this.txtOverwireAmount.Leave += new EventHandler(this.numField_Leave);
      this.textBoxLoanAmount.BackColor = Color.WhiteSmoke;
      this.textBoxLoanAmount.Font = new Font("Arial", 8.25f);
      this.textBoxLoanAmount.Location = new Point(502, 12);
      this.textBoxLoanAmount.Name = "textBoxLoanAmount";
      this.textBoxLoanAmount.ReadOnly = true;
      this.textBoxLoanAmount.Size = new Size(132, 26);
      this.textBoxLoanAmount.TabIndex = 3;
      this.textBoxLoanAmount.TabStop = false;
      this.textBoxLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 8.25f);
      this.label4.Location = new Point(416, 14);
      this.label4.Name = "label4";
      this.label4.Size = new Size(104, 19);
      this.label4.TabIndex = 1;
      this.label4.Text = "Loan Amount";
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 8.25f);
      this.label3.Location = new Point(12, 14);
      this.label3.Name = "label3";
      this.label3.Size = new Size(332, 19);
      this.label3.TabIndex = 0;
      this.label3.Text = "Select Fees to deduct from the Loan Amount";
      this.panelBottom.BackColor = Color.WhiteSmoke;
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.lBtnLenderCredits);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.textBoxLenderCredit);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.labelLenderCredit);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.buttonHUDP1);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.labelWireAmount);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.textBoxWireAmount);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.textBoxDeduction);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.label1);
      this.panelBottom.Controls.Add((System.Windows.Forms.Control) this.checkBoxHide);
      this.panelBottom.Dock = DockStyle.Bottom;
      this.panelBottom.Location = new Point(0, 300);
      this.panelBottom.Name = "panelBottom";
      this.panelBottom.Size = new Size(803, 81);
      this.panelBottom.TabIndex = 5;
      this.lBtnLenderCredits.Location = new Point(482, 35);
      this.lBtnLenderCredits.MaximumSize = new Size(16, 16);
      this.lBtnLenderCredits.MinimumSize = new Size(16, 16);
      this.lBtnLenderCredits.Name = "lBtnLenderCredits";
      this.lBtnLenderCredits.Size = new Size(16, 16);
      this.lBtnLenderCredits.TabIndex = 10;
      this.lBtnLenderCredits.Tag = (object) "LOCKBUTTON_LENDERCREDITS";
      this.lBtnLenderCredits.Click += new EventHandler(this.lBtnLenderCredits_Click);
      this.textBoxLenderCredit.BackColor = Color.WhiteSmoke;
      this.textBoxLenderCredit.Font = new Font("Arial", 8.25f);
      this.textBoxLenderCredit.Location = new Point(502, 32);
      this.textBoxLenderCredit.Name = "textBoxLenderCredit";
      this.textBoxLenderCredit.ReadOnly = true;
      this.textBoxLenderCredit.Size = new Size(132, 26);
      this.textBoxLenderCredit.TabIndex = 9;
      this.textBoxLenderCredit.TabStop = false;
      this.textBoxLenderCredit.Tag = (object) "4083";
      this.textBoxLenderCredit.TextAlign = HorizontalAlignment.Right;
      this.textBoxLenderCredit.Leave += new EventHandler(this.textBoxLenderCredit_Leave);
      this.labelLenderCredit.AutoSize = true;
      this.labelLenderCredit.Font = new Font("Arial", 8.25f);
      this.labelLenderCredit.Location = new Point(398, 35);
      this.labelLenderCredit.Name = "labelLenderCredit";
      this.labelLenderCredit.Size = new Size(117, 19);
      this.labelLenderCredit.TabIndex = 8;
      this.labelLenderCredit.Text = "Lender Credits";
      this.buttonHUDP1.BackColor = SystemColors.Control;
      this.buttonHUDP1.Location = new Point(16, 40);
      this.buttonHUDP1.Name = "buttonHUDP1";
      this.buttonHUDP1.Size = new Size(94, 23);
      this.buttonHUDP1.TabIndex = 7;
      this.buttonHUDP1.Text = "HUD-1 Page 1";
      this.buttonHUDP1.UseVisualStyleBackColor = true;
      this.buttonHUDP1.Click += new EventHandler(this.buttonHUDP1_Click);
      this.labelWireAmount.AutoSize = true;
      this.labelWireAmount.Font = new Font("Arial", 8.25f);
      this.labelWireAmount.Location = new Point(363, 57);
      this.labelWireAmount.Name = "labelWireAmount";
      this.labelWireAmount.Size = new Size(169, 19);
      this.labelWireAmount.TabIndex = 4;
      this.labelWireAmount.Text = "Wire Transfer Amount";
      this.textBoxWireAmount.BackColor = Color.WhiteSmoke;
      this.textBoxWireAmount.Font = new Font("Arial", 8.25f);
      this.textBoxWireAmount.Location = new Point(502, 54);
      this.textBoxWireAmount.Name = "textBoxWireAmount";
      this.textBoxWireAmount.ReadOnly = true;
      this.textBoxWireAmount.Size = new Size(132, 26);
      this.textBoxWireAmount.TabIndex = 3;
      this.textBoxWireAmount.TabStop = false;
      this.textBoxWireAmount.TextAlign = HorizontalAlignment.Right;
      this.textBoxDeduction.BackColor = Color.WhiteSmoke;
      this.textBoxDeduction.Font = new Font("Arial", 8.25f);
      this.textBoxDeduction.Location = new Point(502, 10);
      this.textBoxDeduction.Name = "textBoxDeduction";
      this.textBoxDeduction.ReadOnly = true;
      this.textBoxDeduction.Size = new Size(132, 26);
      this.textBoxDeduction.TabIndex = 2;
      this.textBoxDeduction.TabStop = false;
      this.textBoxDeduction.TextAlign = HorizontalAlignment.Right;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f);
      this.label1.Location = new Point(390, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(128, 19);
      this.label1.TabIndex = 1;
      this.label1.Text = "Total Deductions";
      this.checkBoxHide.Checked = true;
      this.checkBoxHide.CheckState = CheckState.Checked;
      this.checkBoxHide.Font = new Font("Arial", 8.25f);
      this.checkBoxHide.Location = new Point(16, 12);
      this.checkBoxHide.Name = "checkBoxHide";
      this.checkBoxHide.Size = new Size(200, 24);
      this.checkBoxHide.TabIndex = 6;
      this.checkBoxHide.Text = "Hide fees with $0 dollar amounts";
      this.collapsibleSplitter1.AnimationDelay = 20;
      this.collapsibleSplitter1.AnimationStep = 20;
      this.collapsibleSplitter1.BorderStyle3D = Border3DStyle.Flat;
      this.collapsibleSplitter1.ControlToHide = (System.Windows.Forms.Control) this.panelTop;
      this.collapsibleSplitter1.Dock = DockStyle.Top;
      this.collapsibleSplitter1.ExpandParentForm = false;
      this.collapsibleSplitter1.Location = new Point(0, 850);
      this.collapsibleSplitter1.Name = "collapsibleSplitter1";
      this.collapsibleSplitter1.TabIndex = 13;
      this.collapsibleSplitter1.TabStop = false;
      this.collapsibleSplitter1.UseAnimations = false;
      this.collapsibleSplitter1.VisualStyle = VisualStyles.Encompass;
      this.panelTop.BackColor = Color.Transparent;
      this.panelTop.Controls.Add((System.Windows.Forms.Control) this.pictureBox1);
      this.panelTop.Dock = DockStyle.Top;
      this.panelTop.Location = new Point(0, 0);
      this.panelTop.Name = "panelTop";
      this.panelTop.Size = new Size(804, 850);
      this.panelTop.TabIndex = 1;
      this.pictureBox1.Dock = DockStyle.Right;
      this.pictureBox1.Location = new Point(781, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(23, 850);
      this.pictureBox1.TabIndex = 1;
      this.pictureBox1.TabStop = false;
      this.BackColor = Color.WhiteSmoke;
      this.Controls.Add((System.Windows.Forms.Control) this.panel1);
      this.Name = nameof (FundingWorksheet);
      this.Size = new Size(804, 1238);
      this.panel1.ResumeLayout(false);
      this.borderPanel1.ResumeLayout(false);
      this.panelForListView.ResumeLayout(false);
      this.borderPanel2.ResumeLayout(false);
      this.panelLowTop.ResumeLayout(false);
      this.panelLowTop.PerformLayout();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      this.panelBottom.ResumeLayout(false);
      this.panelBottom.PerformLayout();
      this.panelTop.ResumeLayout(false);
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.ResumeLayout(false);
    }

    private void initForm(bool hideZero)
    {
      List<FundingFee> fundingFees = this.loan.Calculator.GetFundingFees(hideZero);
      if (this.gridFees2015.Visible)
      {
        this.gridFees2015.Items.Clear();
        this.gridFees2015.BeginUpdate();
      }
      else
      {
        this.gridFees.Items.Clear();
        this.gridFees.BeginUpdate();
      }
      double num;
      for (int index = 0; index < fundingFees.Count; ++index)
      {
        if (this.gridFees2015.Visible)
        {
          GVItem gvItem = new GVItem("");
          gvItem.SubItems.Add((object) fundingFees[index].CDLineID);
          gvItem.SubItems.Add(fundingFees[index].LineNumber < 520 ? (object) "" : (object) fundingFees[index].LineID);
          gvItem.SubItems.Add((object) fundingFees[index].FeeDescription2015);
          gvItem.SubItems.Add((object) fundingFees[index].Payee);
          gvItem.SubItems.Add((object) fundingFees[index].PaidBy);
          gvItem.SubItems.Add((object) fundingFees[index].PaidTo);
          GVSubItemCollection subItems1 = gvItem.SubItems;
          string str1;
          if (fundingFees[index].Amount == 0.0)
          {
            str1 = "";
          }
          else
          {
            num = fundingFees[index].Amount;
            str1 = num.ToString("N2");
          }
          subItems1.Add((object) str1);
          if (string.Compare(fundingFees[index].PaidBy, "Borrower", true) == 0)
          {
            GVSubItemCollection subItems2 = gvItem.SubItems;
            num = fundingFees[index].POCBorrower2015;
            string str2 = num.ToString("N2");
            subItems2.Add((object) str2);
            gvItem.SubItems.Add((object) "");
          }
          else
          {
            gvItem.SubItems.Add((object) "");
            GVSubItemCollection subItems3 = gvItem.SubItems;
            string str3;
            if (fundingFees[index].LineNumber != 1011 || fundingFees[index].POCSeller2015 != 0.0)
            {
              num = fundingFees[index].POCSeller2015;
              str3 = num.ToString("N2");
            }
            else
              str3 = "";
            subItems3.Add((object) str3);
          }
          GVSubItemCollection subItems4 = gvItem.SubItems;
          num = fundingFees[index].POCBroker2015;
          string str4 = num.ToString("N2");
          subItems4.Add((object) str4);
          GVSubItemCollection subItems5 = gvItem.SubItems;
          num = fundingFees[index].POCLender2015;
          string str5 = num.ToString("N2");
          subItems5.Add((object) str5);
          GVSubItemCollection subItems6 = gvItem.SubItems;
          num = fundingFees[index].POCOther2015;
          string str6 = num.ToString("N2");
          subItems6.Add((object) str6);
          GVSubItemCollection subItems7 = gvItem.SubItems;
          num = fundingFees[index].PACBroker2015;
          string str7 = num.ToString("N2");
          subItems7.Add((object) str7);
          GVSubItemCollection subItems8 = gvItem.SubItems;
          num = fundingFees[index].PACLender2015;
          string str8 = num.ToString("N2");
          subItems8.Add((object) str8);
          GVSubItemCollection subItems9 = gvItem.SubItems;
          num = fundingFees[index].PACOther2015;
          string str9 = num.ToString("N2");
          subItems9.Add((object) str9);
          if (fundingFees[index].BalanceChecked)
          {
            gvItem.Checked = true;
            gvItem.ForeColor = Color.Red;
          }
          gvItem.Tag = fundingFees[index].Tag;
          this.gridFees2015.Items.Add(gvItem);
        }
        else
        {
          GVItem gvItem = new GVItem(fundingFees[index].LineID);
          gvItem.SubItems.Add((object) fundingFees[index].FeeDescription);
          gvItem.SubItems.Add((object) fundingFees[index].Payee);
          gvItem.SubItems.Add((object) fundingFees[index].PaidBy);
          gvItem.SubItems.Add((object) fundingFees[index].PaidTo);
          GVSubItemCollection subItems10 = gvItem.SubItems;
          string str10;
          if (fundingFees[index].Amount == 0.0)
          {
            str10 = "";
          }
          else
          {
            num = fundingFees[index].Amount;
            str10 = num.ToString("N2");
          }
          subItems10.Add((object) str10);
          GVSubItemCollection subItems11 = gvItem.SubItems;
          string str11;
          if (fundingFees[index].POCAmount == 0.0)
          {
            str11 = "";
          }
          else
          {
            num = fundingFees[index].POCAmount;
            str11 = num.ToString("N2");
          }
          subItems11.Add((object) str11);
          gvItem.SubItems.Add((object) fundingFees[index].POCPaidBy);
          GVSubItemCollection subItems12 = gvItem.SubItems;
          string str12;
          if (fundingFees[index].PTCAmount == 0.0)
          {
            str12 = "";
          }
          else
          {
            num = fundingFees[index].PTCAmount;
            str12 = num.ToString("N2");
          }
          subItems12.Add((object) str12);
          gvItem.SubItems.Add((object) fundingFees[index].PTCPaidBy);
          if (fundingFees[index].BalanceChecked)
          {
            gvItem.Checked = true;
            gvItem.ForeColor = Color.Red;
          }
          gvItem.Tag = fundingFees[index].Tag;
          this.gridFees.Items.Add(gvItem);
        }
      }
      if (this.gridFees2015.Visible)
      {
        this.gridFees2015.EndUpdate();
        this.gridFees2015.Sort(1, SortOrder.Ascending);
      }
      else
        this.gridFees.EndUpdate();
    }

    private void gridFees2015_SubitemCheck(object source, GVSubItemEventArgs e)
    {
      this.gridFees2015.SubItemCheck -= new GVSubItemEventHandler(this.gridFees2015_SubitemCheck);
      try
      {
        this.gridSubitem_Checked(this.gridFees2015, e);
      }
      catch (Exception ex)
      {
      }
      this.gridFees2015.SubItemCheck += new GVSubItemEventHandler(this.gridFees2015_SubitemCheck);
    }

    private void gridFees_SubitemCheck(object source, GVSubItemEventArgs e)
    {
      this.gridFees.SubItemCheck -= new GVSubItemEventHandler(this.gridFees_SubitemCheck);
      try
      {
        this.gridSubitem_Checked(this.gridFees, e);
      }
      catch (Exception ex)
      {
      }
      this.gridFees.SubItemCheck += new GVSubItemEventHandler(this.gridFees_SubitemCheck);
    }

    private void gridSubitem_Checked(GridView view, GVSubItemEventArgs e)
    {
      int index = e.SubItem.Item.Index;
      GFEItem tag = (GFEItem) view.Items[index].Tag;
      if (e.SubItem.Checked)
      {
        if (view.Items[index].SubItems[this.sellerColumn].Text == "Seller")
        {
          if (!this.setField(tag.CheckSellerFieldID, "Y"))
          {
            e.SubItem.Checked = !e.SubItem.Checked;
            return;
          }
        }
        else if (!this.setField(tag.CheckBorrowerFieldID, "Y"))
        {
          e.SubItem.Checked = !e.SubItem.Checked;
          return;
        }
        view.Items[index].ForeColor = Color.Red;
      }
      else
      {
        if (view.Items[index].SubItems[this.sellerColumn].Text == "Seller")
        {
          if (!this.setField(tag.CheckSellerFieldID, ""))
          {
            e.SubItem.Checked = !e.SubItem.Checked;
            return;
          }
        }
        else if (!this.setField(tag.CheckBorrowerFieldID, ""))
        {
          e.SubItem.Checked = !e.SubItem.Checked;
          return;
        }
        view.Items[index].ForeColor = Color.Black;
      }
      this.doCalculation();
      this.updateBusinessRule();
    }

    private bool setField(string id, string val)
    {
      if (!this.session.UserInfo.IsSuperAdministrator() && this.loan.IsFieldReadOnly(id))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Field " + id + " is a read-only field", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      this.loan.SetCurrentField(id, val);
      return true;
    }

    private void checkBoxHide_CheckedChanged(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      this.checkBoxHide.CheckedChanged -= new EventHandler(this.checkBoxHide_CheckedChanged);
      this.initForm(this.checkBoxHide.Checked);
      this.doCalculation();
      this.checkBoxHide.CheckedChanged += new EventHandler(this.checkBoxHide_CheckedChanged);
      Cursor.Current = Cursors.Default;
    }

    private void doCalculation()
    {
      this.loan.Calculator.CalculateFunder((string) null, (string) null);
      this.textBoxDeduction.Text = this.loan.GetField("1989");
      this.textBoxWireAmount.Text = this.loan.GetField("1990");
    }

    private void digitOnly_keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void textField_Enter(object sender, EventArgs e)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      if (textBox == null || textBox.Tag == null)
        return;
      this.session.Application.GetService<IStatusDisplay>().DisplayFieldID(textBox.Tag.ToString());
    }

    private void numField_Leave(object sender, EventArgs e)
    {
      System.Windows.Forms.TextBox ctrl = (System.Windows.Forms.TextBox) sender;
      if (ctrl.Tag.ToString() + string.Empty == string.Empty || !this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag) || this.loan.IsFieldReadOnly(ctrl.Tag.ToString()))
        return;
      double num = Utils.ParseDouble((object) ctrl.Text.Trim());
      if (ctrl.Text.Trim() != string.Empty)
        ctrl.Text = num.ToString("N2");
      this.loan.SetField("2005", ctrl.Text);
      this.textBoxWireAmount.Text = this.loan.GetField("1990");
      this.updateBusinessRule();
    }

    private void textField_KeyUp(object sender, KeyEventArgs e)
    {
      this.formatFieldValue(sender, FieldFormat.DECIMAL_2);
    }

    private void formatFieldValue(object sender, FieldFormat format)
    {
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, format, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void buttonHUDP1_Click(object sender, EventArgs e)
    {
      if (this.loan.Use2015RESPA)
      {
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, "Transaction Summary", new InputFormInfo("CLOSINGDISCLOSUREPAGE3", "CLOSINGDISCLOSUREPAGE3"), 1000, 560, FieldSource.CurrentLoan, "", this.session))
        {
          entryPopupDialog.LoanScreenLoaded += new EventHandler(this.quickPage_LoanScreenLoaded);
          int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          this.checkBoxHide_CheckedChanged((object) null, (EventArgs) null);
          this.doCalculation();
        }
      }
      else
      {
        using (QuickEntryPopupDialog entryPopupDialog = new QuickEntryPopupDialog((IHtmlInput) this.loan, "HUD-1 Page 1 For Funder", new InputFormInfo("HUD1PG1ForFunder", "HUD1PG1ForFunder"), 800, 560, FieldSource.CurrentLoan, "", this.session))
        {
          int num = (int) entryPopupDialog.ShowDialog((IWin32Window) this.session.MainForm);
          this.checkBoxHide_CheckedChanged((object) null, (EventArgs) null);
          this.doCalculation();
        }
      }
    }

    private void quickPage_LoanScreenLoaded(object sender, EventArgs e)
    {
      ((CLOSINGDISCLOSUREPAGE3InputHandler) ((QuickEntryPopupDialog) sender).GetInputHandler()).SetFundingControls();
    }

    private void btnTemplate_Click(object sender, EventArgs e)
    {
      using (PurchaseAdviceTemplateSelector templateSelector = new PurchaseAdviceTemplateSelector(Session.DefaultInstance, EllieMae.EMLite.ClientServer.TemplateSettingsType.FundingTemplate))
      {
        if (templateSelector.ShowDialog((IWin32Window) this.session.MainForm) != DialogResult.OK)
          return;
        this.loan.SetFundingTemplate((FundingTemplate) templateSelector.SelectedTemplate, templateSelector.AppendTemplate);
        this.initForm(this.checkBoxHide.Checked);
        this.doCalculation();
      }
    }

    private void chkDeduct_Click(object sender, EventArgs e)
    {
      if (this.chkDeduct.Checked)
        this.loan.SetField("2833", "Y");
      else
        this.loan.SetField("2833", "");
      this.updateBusinessRule();
    }

    public void RefreshContents() => this.initForm(this.checkBoxHide.Checked);

    public void RefreshLoanContents()
    {
      this.freeScreen.RefreshLoanContents();
      this.initialPage();
    }

    private void toggleLockButton(object senderTextBox, object senderLockButton)
    {
      if (!(senderTextBox is System.Windows.Forms.TextBox) || !(senderLockButton is FieldLockButton))
        return;
      System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox) senderTextBox;
      FieldLockButton fieldLockButton = (FieldLockButton) senderLockButton;
      fieldLockButton.Locked = !fieldLockButton.Locked;
      textBox.ReadOnly = !textBox.ReadOnly;
      this.textBoxLenderCredit.BackColor = textBox.ReadOnly ? Color.WhiteSmoke : Color.Empty;
    }

    private void lBtnLenderCredits_Click(object sender, EventArgs e)
    {
      this.toggleLockButton((object) this.textBoxLenderCredit, sender);
      if (!this.textBoxLenderCredit.ReadOnly)
      {
        this.loan.AddLock("4083");
        this.loan.SetField("4083", "");
        this.textBoxLenderCredit.Text = "";
      }
      else
      {
        this.loan.RemoveLock("4083");
        this.textBoxLenderCredit.Text = this.loan.GetField("4083");
      }
      this.doCalculation();
      this.updateBusinessRule();
    }

    private void textBoxLenderCredit_Leave(object sender, EventArgs e)
    {
      System.Windows.Forms.TextBox ctrl = (System.Windows.Forms.TextBox) sender;
      if (ctrl.Tag.ToString() + string.Empty == string.Empty || !this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag) || this.loan.IsFieldReadOnly(ctrl.Tag.ToString()))
        return;
      double num = Utils.ParseDouble((object) ctrl.Text.Trim());
      if (ctrl.Text.Trim() != string.Empty)
        ctrl.Text = num.ToString("N2");
      this.loan.SetField("4083", ctrl.Text);
      this.doCalculation();
      this.updateBusinessRule();
    }

    private void picVideoOver_Click(object sender, EventArgs e)
    {
      WebViewer.OpenURL("http://help.icemortgagetechnology.com/GA/video_LenderCreditsFundingWkSheet.html", "Funding Worksheet - Lender Credits", 1046, 732);
    }

    private void updateBusinessRule()
    {
      try
      {
        if (this.loan == null || this.loan.IsTemplate)
          return;
        Session.Application.GetService<ILoanEditor>().ApplyOnDemandBusinessRules();
      }
      catch (Exception ex)
      {
      }
    }
  }
}
