// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.InsuranceDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  internal class InsuranceDialog : Form
  {
    private ComboBox typeCombo;
    private Button okBtn;
    private Button cancelBtn;
    private Label label1;
    private Label label;
    private TextBox rateTxt;
    private Label label2;
    private IContainer components;
    private LoanData loan;
    private ToolTip fieldToolTip;
    private CheckBox chkUseREGZMI;
    private PictureBox pboxAsterisk;
    private bool inTemplate;
    private PictureBox pboxDownArrow;
    private Label labelMonths;
    private TextBox txtMonths;
    private CheckBox chkPrepaid;
    private TextBox txtField1766;
    private TextBox txtLoanAmount;
    private TextBox txtMonthlyAmount;
    private BorderPanel borderPanel2;
    private BorderPanel borderPanel1;
    private Label label5;
    private PopupBusinessRules popupRules;
    private Sessions.Session session = Session.DefaultInstance;
    private double amount;
    private string priceType;
    private double rateFactor;
    private int monthsMIP;
    private string useREGZMI;
    private string isPrepaid;

    internal InsuranceDialog(string caption, IHtmlInput lpLoan, Sessions.Session session)
    {
      this.inTemplate = true;
      this.session = session;
      this.InitializeComponent();
      string field1 = lpLoan.GetField("19");
      string field2 = lpLoan.GetField("1964");
      if (lpLoan.GetField("1172") == "FHA")
        this.chkPrepaid.Enabled = false;
      this.Text = caption;
      if (caption == "Mortgage Insurance Premium")
      {
        this.txtMonths.Visible = true;
        this.labelMonths.Visible = true;
        this.chkPrepaid.Visible = true;
      }
      else
      {
        this.txtMonths.Visible = false;
        this.labelMonths.Visible = false;
        this.chkPrepaid.Visible = false;
      }
      string str = "";
      switch (caption)
      {
        case "Hazard Insurance":
        case "Homeowner's Insurance":
          this.rateTxt.Text = lpLoan.GetField("1322");
          str = lpLoan.GetField("1750");
          if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
            str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1322");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1750");
          break;
        case "Taxes":
        case "Taxes Reserved":
          this.rateTxt.Text = lpLoan.GetField("1752");
          str = lpLoan.GetField("1751");
          if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
            str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1752");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1751");
          break;
        case "Mortgage Insurance":
        case "Mortgage Insurance Reserved":
          this.chkUseREGZMI.Visible = true;
          this.txtField1766.Visible = true;
          this.useREGZMI = lpLoan.GetField("USEREGZMI");
          this.chkUseREGZMI.Checked = this.useREGZMI == "Y";
          this.rateTxt.Text = lpLoan.GetField("1199");
          this.txtField1766.Text = lpLoan.GetField("1766");
          str = lpLoan.GetField("1757");
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1199");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1757");
          this.fieldToolTip.SetToolTip((Control) this.chkUseREGZMI, "USEREGZMI");
          this.fieldToolTip.SetToolTip((Control) this.txtField1766, "1766");
          break;
        case "Mortgage Insurance Premium":
          this.rateTxt.Text = lpLoan.GetField("1807");
          str = lpLoan.GetField("1806");
          this.monthsMIP = Utils.ParseInt((object) lpLoan.GetField("1209"));
          if (Utils.ParseInt((object) lpLoan.GetField("1209")) > 0)
            this.txtMonths.Text = lpLoan.GetField("1209");
          else
            this.txtMonths.Text = string.Empty;
          this.chkPrepaid.Checked = lpLoan.GetField("2978") == "Y";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1807");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1806");
          this.fieldToolTip.SetToolTip((Control) this.txtMonths, "1209");
          this.fieldToolTip.SetToolTip((Control) this.chkPrepaid, "2978");
          break;
      }
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      int num;
      switch (str)
      {
        case "Purchase Price":
          num = 1;
          break;
        case "Appraisal Value":
          num = 2;
          break;
        case "Base Loan Amount":
          num = 3;
          break;
        case "As Completed Purchase Price":
          num = 4;
          break;
        case "As Completed Appraised Value":
          num = 5;
          break;
        default:
          num = 0;
          break;
      }
      this.typeCombo.SelectedIndex = num;
      this.typeCombo_SelectedIndexChanged((object) null, (EventArgs) null);
      ResourceManager resources = new ResourceManager(typeof (InsuranceDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      switch (caption)
      {
        case "Hazard Insurance":
        case "Homeowner's Insurance":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1322");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1750");
          this.rateTxt.Tag = (object) "1322";
          break;
        case "Taxes":
        case "Taxes Reserved":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1752");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1751");
          this.rateTxt.Tag = (object) "1752";
          break;
        case "Mortgage Insurance":
        case "Mortgage Insurance Reserved":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1199");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1757");
          this.popupRules.SetBusinessRules((object) this.chkUseREGZMI, "USEREGZMI");
          this.rateTxt.Tag = (object) "1199";
          this.txtField1766.Tag = (object) "1766";
          break;
        case "Mortgage Insurance Premium":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1807");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1806");
          this.popupRules.SetBusinessRules((object) this.txtMonths, "1209");
          this.popupRules.SetBusinessRules((object) this.chkPrepaid, "2978");
          this.rateTxt.Tag = (object) "1807";
          this.txtMonths.Tag = (object) "1209";
          this.chkPrepaid.Tag = (object) "2978";
          break;
      }
      this.txtLoanAmount.Enabled = false;
      this.txtMonthlyAmount.Enabled = false;
    }

    internal InsuranceDialog(string caption, LoanData loan, Sessions.Session session)
    {
      this.loan = loan;
      this.session = session;
      this.inTemplate = false;
      this.InitializeComponent();
      string field1 = loan.GetField("19");
      string field2 = loan.GetField("1964");
      if (loan.GetField("1172") == "FHA")
        this.chkPrepaid.Enabled = false;
      this.Text = caption;
      if (caption == "Mortgage Insurance Premium")
      {
        this.txtMonths.Visible = true;
        this.labelMonths.Visible = true;
        this.chkPrepaid.Visible = true;
      }
      else
      {
        this.txtMonths.Visible = false;
        this.labelMonths.Visible = false;
        this.chkPrepaid.Visible = false;
      }
      string str = "";
      ResourceManager resources = new ResourceManager(typeof (InsuranceDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      switch (caption)
      {
        case "Hazard Insurance":
        case "Homeowner's Insurance":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1322");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1750");
          this.rateTxt.Text = loan.GetField("1322");
          str = loan.GetField("1750");
          if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
            str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1322");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1750");
          this.rateTxt.Tag = (object) "1322";
          break;
        case "Taxes":
        case "Taxes Reserved":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1752");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1751");
          this.rateTxt.Text = loan.GetField("1752");
          str = loan.GetField("1751");
          if (str == string.Empty)
            str = "Purchase Price";
          if (field1 == "ConstructionOnly" || field1 == "ConstructionToPermanent")
            str = !(field2 == "Y") ? "As Completed Appraised Value" : "As Completed Purchase Price";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1752");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1751");
          this.rateTxt.Tag = (object) "1752";
          break;
        case "Mortgage Insurance":
        case "Mortgage Insurance Reserved":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1199");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1757");
          this.popupRules.SetBusinessRules((object) this.chkUseREGZMI, "USEREGZMI");
          this.chkUseREGZMI.Visible = true;
          this.txtField1766.Visible = true;
          this.useREGZMI = loan.GetField("USEREGZMI");
          this.chkUseREGZMI.Checked = this.useREGZMI == "Y";
          this.rateTxt.Text = loan.GetField("1199");
          this.txtField1766.Text = loan.GetField("1766");
          str = loan.GetField("1757");
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1199");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1757");
          this.fieldToolTip.SetToolTip((Control) this.chkUseREGZMI, "USEREGZMI");
          this.fieldToolTip.SetToolTip((Control) this.txtField1766, "1766");
          this.rateTxt.Tag = (object) "1199";
          this.txtField1766.Tag = (object) "1766";
          break;
        case "Mortgage Insurance Premium":
          this.popupRules.SetBusinessRules((object) this.rateTxt, "1807");
          this.popupRules.SetBusinessRules((object) this.typeCombo, "1806");
          this.popupRules.SetBusinessRules((object) this.txtMonths, "1209");
          this.popupRules.SetBusinessRules((object) this.chkPrepaid, "2978");
          this.rateTxt.Text = loan.GetField("1807");
          str = loan.GetField("1806");
          this.monthsMIP = Utils.ParseInt((object) loan.GetField("1209"));
          if (Utils.ParseInt((object) loan.GetField("1209")) > 0)
            this.txtMonths.Text = loan.GetField("1209");
          else
            this.txtMonths.Text = string.Empty;
          this.chkPrepaid.Checked = loan.GetField("2978") == "Y";
          this.fieldToolTip.SetToolTip((Control) this.rateTxt, "1807");
          this.fieldToolTip.SetToolTip((Control) this.txtMonths, "1209");
          this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1806");
          this.fieldToolTip.SetToolTip((Control) this.chkPrepaid, "2978");
          this.rateTxt.Tag = (object) "1807";
          this.txtMonths.Tag = (object) "1209";
          this.chkPrepaid.Tag = (object) "2978";
          break;
      }
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      int num;
      switch (str)
      {
        case "Purchase Price":
          num = 1;
          break;
        case "Appraisal Value":
          num = 2;
          break;
        case "Base Loan Amount":
          num = 3;
          break;
        case "As Completed Purchase Price":
          num = 4;
          break;
        case "As Completed Appraised Value":
          num = 5;
          break;
        default:
          num = 0;
          break;
      }
      this.typeCombo.SelectedIndex = num;
      this.typeCombo_SelectedIndexChanged((object) null, (EventArgs) null);
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (InsuranceDialog));
      this.typeCombo = new ComboBox();
      this.rateTxt = new TextBox();
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.label = new Label();
      this.label2 = new Label();
      this.fieldToolTip = new ToolTip(this.components);
      this.chkUseREGZMI = new CheckBox();
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.labelMonths = new Label();
      this.txtMonths = new TextBox();
      this.chkPrepaid = new CheckBox();
      this.txtField1766 = new TextBox();
      this.txtLoanAmount = new TextBox();
      this.txtMonthlyAmount = new TextBox();
      this.borderPanel2 = new BorderPanel();
      this.borderPanel1 = new BorderPanel();
      this.label5 = new Label();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.borderPanel1.SuspendLayout();
      this.SuspendLayout();
      this.typeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.typeCombo.Items.AddRange(new object[6]
      {
        (object) "Loan Amount",
        (object) "Purchase Price",
        (object) "Appraisal Value",
        (object) "Base Loan Amount",
        (object) "As Completed Purchase Price",
        (object) "As Completed Appraised Value"
      });
      this.typeCombo.Location = new Point(20, 41);
      this.typeCombo.Name = "typeCombo";
      this.typeCombo.Size = new Size(174, 21);
      this.typeCombo.TabIndex = 2;
      this.typeCombo.SelectedIndexChanged += new EventHandler(this.typeCombo_SelectedIndexChanged);
      this.typeCombo.Leave += new EventHandler(this.leave);
      this.rateTxt.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.rateTxt.Location = new Point(200, 66);
      this.rateTxt.MaxLength = 8;
      this.rateTxt.Name = "rateTxt";
      this.rateTxt.Size = new Size(95, 21);
      this.rateTxt.TabIndex = 3;
      this.rateTxt.TextAlign = HorizontalAlignment.Right;
      this.rateTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateTxt.Leave += new EventHandler(this.leave);
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(181, 178);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 8;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(262, 178);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 9;
      this.cancelBtn.Text = "&Cancel";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(167, 69);
      this.label1.Name = "label1";
      this.label1.Size = new Size(30, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Rate";
      this.label.AutoSize = true;
      this.label.Location = new Point(70, 98);
      this.label.Name = "label";
      this.label.Size = new Size(88, 13);
      this.label.TabIndex = 8;
      this.label.Text = "Monthly Payment";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(301, 69);
      this.label2.Name = "label2";
      this.label2.Size = new Size(15, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "%";
      this.chkUseREGZMI.Location = new Point(20, 18);
      this.chkUseREGZMI.Name = "chkUseREGZMI";
      this.chkUseREGZMI.Size = new Size(164, 16);
      this.chkUseREGZMI.TabIndex = 1;
      this.chkUseREGZMI.Text = "Override with MI from RegZ";
      this.chkUseREGZMI.Visible = false;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(24, 67);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 18;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(52, 67);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 68;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.labelMonths.AutoSize = true;
      this.labelMonths.Location = new Point(116, 122);
      this.labelMonths.Name = "labelMonths";
      this.labelMonths.Size = new Size(42, 13);
      this.labelMonths.TabIndex = 70;
      this.labelMonths.Text = "Months";
      this.txtMonths.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtMonths.Location = new Point(200, 119);
      this.txtMonths.MaxLength = 8;
      this.txtMonths.Name = "txtMonths";
      this.txtMonths.Size = new Size(95, 21);
      this.txtMonths.TabIndex = 4;
      this.txtMonths.TextAlign = HorizontalAlignment.Right;
      this.txtMonths.Leave += new EventHandler(this.leave);
      this.chkPrepaid.Location = new Point(20, 121);
      this.chkPrepaid.Name = "chkPrepaid";
      this.chkPrepaid.Size = new Size(72, 16);
      this.chkPrepaid.TabIndex = 5;
      this.chkPrepaid.Tag = (object) "";
      this.chkPrepaid.Text = "Prepaid";
      this.chkPrepaid.Visible = false;
      this.txtField1766.BackColor = Color.WhiteSmoke;
      this.txtField1766.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtField1766.Location = new Point(200, 18);
      this.txtField1766.MaxLength = 8;
      this.txtField1766.Name = "txtField1766";
      this.txtField1766.ReadOnly = true;
      this.txtField1766.Size = new Size(95, 21);
      this.txtField1766.TabIndex = 77;
      this.txtField1766.TabStop = false;
      this.txtField1766.TextAlign = HorizontalAlignment.Right;
      this.txtField1766.Visible = false;
      this.txtLoanAmount.BackColor = Color.WhiteSmoke;
      this.txtLoanAmount.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtLoanAmount.Location = new Point(200, 42);
      this.txtLoanAmount.MaxLength = 8;
      this.txtLoanAmount.Name = "txtLoanAmount";
      this.txtLoanAmount.ReadOnly = true;
      this.txtLoanAmount.Size = new Size(95, 21);
      this.txtLoanAmount.TabIndex = 78;
      this.txtLoanAmount.TabStop = false;
      this.txtLoanAmount.TextAlign = HorizontalAlignment.Right;
      this.txtMonthlyAmount.BackColor = Color.WhiteSmoke;
      this.txtMonthlyAmount.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.txtMonthlyAmount.Location = new Point(200, 94);
      this.txtMonthlyAmount.MaxLength = 8;
      this.txtMonthlyAmount.Name = "txtMonthlyAmount";
      this.txtMonthlyAmount.ReadOnly = true;
      this.txtMonthlyAmount.Size = new Size(95, 21);
      this.txtMonthlyAmount.TabIndex = 79;
      this.txtMonthlyAmount.TabStop = false;
      this.txtMonthlyAmount.TextAlign = HorizontalAlignment.Right;
      this.borderPanel2.Location = new Point(70, 90);
      this.borderPanel2.Name = "borderPanel2";
      this.borderPanel2.Size = new Size(240, 1);
      this.borderPanel2.TabIndex = 81;
      this.borderPanel1.BackColor = Color.WhiteSmoke;
      this.borderPanel1.Controls.Add((Control) this.label5);
      this.borderPanel1.Controls.Add((Control) this.txtField1766);
      this.borderPanel1.Controls.Add((Control) this.chkUseREGZMI);
      this.borderPanel1.Controls.Add((Control) this.borderPanel2);
      this.borderPanel1.Controls.Add((Control) this.typeCombo);
      this.borderPanel1.Controls.Add((Control) this.txtMonthlyAmount);
      this.borderPanel1.Controls.Add((Control) this.label2);
      this.borderPanel1.Controls.Add((Control) this.txtLoanAmount);
      this.borderPanel1.Controls.Add((Control) this.rateTxt);
      this.borderPanel1.Controls.Add((Control) this.label1);
      this.borderPanel1.Controls.Add((Control) this.label);
      this.borderPanel1.Controls.Add((Control) this.pboxAsterisk);
      this.borderPanel1.Controls.Add((Control) this.chkPrepaid);
      this.borderPanel1.Controls.Add((Control) this.pboxDownArrow);
      this.borderPanel1.Controls.Add((Control) this.labelMonths);
      this.borderPanel1.Controls.Add((Control) this.txtMonths);
      this.borderPanel1.Location = new Point(12, 12);
      this.borderPanel1.Name = "borderPanel1";
      this.borderPanel1.Size = new Size(325, 154);
      this.borderPanel1.TabIndex = 0;
      this.borderPanel1.TabStop = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(70, 69);
      this.label5.Name = "label5";
      this.label5.Size = new Size(14, 13);
      this.label5.TabIndex = 82;
      this.label5.Text = "X";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(350, 214);
      this.Controls.Add((Control) this.borderPanel1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (InsuranceDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (InsuranceDialog);
      this.KeyPress += new KeyPressEventHandler(this.InsuranceDialog_KeyPress);
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.borderPanel1.ResumeLayout(false);
      this.borderPanel1.PerformLayout();
      this.ResumeLayout(false);
    }

    internal void PrequalInitialization()
    {
      this.rateTxt.Text = this.rateFactor.ToString();
      string priceType = this.priceType;
      if (this.monthsMIP > 0)
        this.txtMonths.Text = this.monthsMIP.ToString();
      else
        this.txtMonths.Text = "";
      int num;
      switch (priceType)
      {
        case "PurchasePrice":
          num = 1;
          break;
        case "AppraisalValue":
          num = 2;
          break;
        default:
          num = 0;
          break;
      }
      this.typeCombo.SelectedIndex = num;
      this.typeCombo_SelectedIndexChanged((object) null, (EventArgs) null);
    }

    private void typeCombo_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.inTemplate)
        return;
      switch (this.typeCombo.SelectedItem.ToString())
      {
        case "Loan Amount":
          this.txtLoanAmount.Text = this.loan.GetField("2");
          break;
        case "Purchase Price":
          this.txtLoanAmount.Text = this.loan.GetField("136");
          break;
        case "Appraisal Value":
          this.txtLoanAmount.Text = this.loan.GetField("356");
          break;
        case "Base Loan Amount":
          this.txtLoanAmount.Text = this.loan.GetField("1109");
          break;
        case "As Completed Purchase Price":
          this.txtLoanAmount.Text = this.loan.GetField("CONST.X58");
          break;
        case "As Completed Appraised Value":
          this.txtLoanAmount.Text = this.loan.GetField("CONST.X59");
          break;
      }
      this.leave((object) null, (EventArgs) null);
    }

    internal double Amount
    {
      get => this.amount;
      set => this.amount = value;
    }

    internal string PriceType
    {
      get => this.priceType;
      set => this.priceType = value;
    }

    internal double RateFactor
    {
      get => this.rateFactor;
      set => this.rateFactor = value;
    }

    internal int MonthsMIP
    {
      get => this.monthsMIP;
      set => this.monthsMIP = value;
    }

    internal string UseREGZMI
    {
      get => this.useREGZMI;
      set => this.useREGZMI = value;
    }

    internal string IsPrepaid
    {
      get => this.isPrepaid;
      set => this.isPrepaid = value;
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender is TextBox)
      {
        TextBox ctrl = (TextBox) sender;
        if (ctrl.Tag != null && this.popupRules != null)
          this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag);
      }
      this.amount = 0.0;
      this.rateFactor = Utils.ParseDouble((object) this.rateTxt.Text);
      if (this.rateFactor != 0.0)
        this.rateTxt.Text = this.rateFactor.ToString("N4");
      else
        this.rateTxt.Text = "";
      this.amount = Utils.ArithmeticRounding(this.DoubleValue(this.txtLoanAmount.Text.Replace(",", string.Empty)) * (this.rateFactor / 1200.0), 2);
      if (this.loan != null)
        this.txtMonthlyAmount.Text = this.amount.ToString("N2");
      else
        this.txtMonthlyAmount.Text = "";
      switch (this.typeCombo.SelectedIndex)
      {
        case 1:
          this.priceType = "Purchase Price";
          break;
        case 2:
          this.priceType = "Appraisal Value";
          break;
        case 3:
          this.priceType = "Base Loan Amount";
          break;
        case 4:
          this.priceType = "As Completed Purchase Price";
          break;
        case 5:
          this.priceType = "As Completed Appraised Value";
          break;
        default:
          this.priceType = "Loan Amount";
          break;
      }
      this.useREGZMI = !this.chkUseREGZMI.Checked ? "N" : "Y";
      if (!this.txtMonths.Visible)
        return;
      this.monthsMIP = !(this.txtMonths.Text.Trim() != string.Empty) ? 0 : Utils.ParseInt((object) this.txtMonths.Text.Trim());
      this.amount *= (double) this.monthsMIP;
      this.isPrepaid = this.chkPrepaid.Checked ? "Y" : "";
    }

    private double DoubleValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0.0 : double.Parse(strValue.Replace(",", string.Empty));
    }

    private int IntValue(string strValue)
    {
      return strValue == string.Empty || strValue == null ? 0 : Convert.ToInt32(strValue);
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_4;
      TextBox textBox = (TextBox) sender;
      if (textBox.Tag != null && textBox.Tag.ToString() == "1199")
        dataFormat = FieldFormat.DECIMAL_6;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      this.leave((object) null, (EventArgs) null);
    }

    private void InsuranceDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
