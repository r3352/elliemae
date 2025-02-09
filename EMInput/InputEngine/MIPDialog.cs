// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.MIPDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.CalculationEngine;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class MIPDialog : Form
  {
    private Button okBtn;
    private Button cancelBtn;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private Label label10;
    private Label label11;
    private Label label12;
    private Label label13;
    private Label label14;
    private ComboBox typeCombo;
    private Label label15;
    private CheckBox balanceChkbox;
    private CheckBox midpointChkbox;
    private IContainer components;
    private TextBox rateFundingTxt;
    private TextBox rateMI1Txt;
    private TextBox rateMI2Txt;
    private TextBox monthMI1Txt;
    private TextBox monthMI2Txt;
    private TextBox rateMICancelTxt;
    private TextBox loanAmtTxt;
    private CheckBox lockChkbox;
    private TextBox fundingFeeTxt;
    private TextBox baseLoanAmtTxt;
    private TextBox fundingAmtTxt;
    private TextBox cashTxt;
    private LoanData loan;
    private IHtmlInput lpLoan;
    private bool inTemplate;
    private ToolTip fieldToolTip;
    private PictureBox pboxAsterisk;
    private string loanType = string.Empty;
    private PictureBox pboxDownArrow;
    private Button btnGetMI;
    private Label label17;
    private ComboBox cboVeteranType;
    private CheckBox chkFirstTime;
    private EMHelpLink emHelpLink1;
    private PopupBusinessRules popupRules;
    private bool needLoanAmountRounding = true;
    private CheckBox chkRounding;
    private CheckBox chkPrepaid;
    private TextBox txtPrepaidMonth;
    private Label labelPrepaid;
    private GroupContainer groupBoxTop;
    private GroupContainer groupBoxMiddle;
    private GroupContainer groupBoxFHA;
    private CheckBox chkDeclining;
    private CheckBox chkRefundUnearned;
    private bool forLP;
    private CheckBox chkField3533;
    private CheckBox chkField3532;
    private CheckBox chkField3531;
    private Panel pnlBody;
    private CheckBox chkLockFactor;
    private TextBox txtPrepaidAmount;
    private Label labelPrepaidAmount;
    private Sessions.Session session = Session.DefaultInstance;
    private double propertyValue;
    private double paidInCash;
    private double rateMI1;
    private double rateMI2;
    private double rateMICancel;
    private double amountMI1;
    private double amountMI2;
    private int monthMI1;
    private int monthMI2;
    private double baseLoanAmount;
    private double rateFunding;
    private double fundingAmt;
    private double fundingFee;
    private double loanAmount;
    private string lockPaidInCash = string.Empty;
    private double prepaidMIAmount;
    private int priceType;
    private string chkFirstTimeValue = string.Empty;

    public MIPDialog(IHtmlInput lpLoan, bool forLP, Sessions.Session session)
    {
      this.session = session;
      this.lpLoan = lpLoan;
      this.forLP = forLP;
      this.InitializeComponent();
      this.labelPrepaidAmount.Visible = this.txtPrepaidAmount.Visible = false;
      this.inTemplate = true;
      this.btnGetMI.Visible = false;
      this.groupBoxFHA.Visible = false;
      this.pnlBody.Height = this.groupBoxMiddle.Bottom;
      this.ClientSize = new Size(this.ClientSize.Width, this.pnlBody.Bottom + this.okBtn.Height + 20);
      this.typeCombo.SelectedItem = (object) "Loan Amount";
      this.rateFundingTxt.Text = lpLoan.GetField("1107");
      this.rateFunding = Utils.ParseDouble((object) this.rateFundingTxt.Text);
      this.fieldToolTip.SetToolTip((Control) this.rateFundingTxt, "1107");
      this.fundingAmtTxt.Text = lpLoan.GetField("1826");
      this.fundingAmt = Utils.ParseDouble((object) this.fundingAmtTxt.Text);
      this.fieldToolTip.SetToolTip((Control) this.fundingAmtTxt, "1826");
      this.lockPaidInCash = lpLoan.GetField("1765");
      this.lockChkbox.Checked = this.lockPaidInCash == "Y";
      this.fieldToolTip.SetToolTip((Control) this.lockChkbox, "1765");
      this.cashTxt.Text = lpLoan.GetField("1760");
      this.fieldToolTip.SetToolTip((Control) this.cashTxt, "1760");
      this.baseLoanAmtTxt.ReadOnly = true;
      this.baseLoanAmtTxt.TabStop = false;
      string str = lpLoan.GetField("1757");
      if (!forLP && lpLoan.GetField("1172") == "FarmersHomeAdministration")
        str = "Loan Amount";
      switch (str)
      {
        case "Purchase Price":
          this.priceType = 1;
          break;
        case "Appraisal Value":
          this.priceType = 2;
          break;
        case "Base Loan Amount":
          this.priceType = 3;
          break;
        default:
          this.priceType = 0;
          break;
      }
      if (this.typeCombo.Items.Count >= this.priceType)
        this.typeCombo.SelectedIndex = this.priceType;
      this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1757");
      this.lockChkbox_Click((object) null, (EventArgs) null);
      this.rateMI1Txt.Text = lpLoan.GetField("1199");
      this.monthMI1Txt.Text = lpLoan.GetField("1198");
      this.rateMI2Txt.Text = lpLoan.GetField("1201");
      this.monthMI2Txt.Text = lpLoan.GetField("1200");
      this.rateMICancelTxt.Text = lpLoan.GetField("1205");
      if (forLP && lpLoan.GetField("1172") != "Conventional")
      {
        lpLoan.SetCurrentField("3248", "");
        this.chkDeclining.Enabled = false;
      }
      this.chkPrepaid.Enabled = !(lpLoan.GetField("1172") == "FHA");
      this.txtPrepaidMonth.ReadOnly = !this.chkPrepaid.Enabled;
      this.txtPrepaidMonth.Text = lpLoan.GetField("1209");
      this.chkDeclining.Checked = lpLoan.GetField("3248") == "Y";
      this.chkRounding.Checked = lpLoan.GetField("SYS.X11") == "Y";
      this.chkPrepaid.Checked = lpLoan.GetField("2978") == "Y";
      this.chkRefundUnearned.Checked = lpLoan.GetField("3262") == "Y";
      this.chkField3531.Checked = lpLoan.GetField("3531") == "Y";
      this.chkField3532.Checked = lpLoan.GetField("3532") == "Y";
      this.chkField3533.Checked = lpLoan.GetField("3533") == "Y";
      this.chkLockFactor.Checked = lpLoan.GetField("3625") == "Y";
      this.fieldToolTip.SetToolTip((Control) this.rateMI1Txt, "1199");
      this.fieldToolTip.SetToolTip((Control) this.monthMI1Txt, "1198");
      this.fieldToolTip.SetToolTip((Control) this.rateMI2Txt, "1201");
      this.fieldToolTip.SetToolTip((Control) this.monthMI2Txt, "1200");
      this.fieldToolTip.SetToolTip((Control) this.rateMICancelTxt, "1205");
      this.fieldToolTip.SetToolTip((Control) this.txtPrepaidMonth, "1209");
      this.fieldToolTip.SetToolTip((Control) this.chkDeclining, "3248");
      this.fieldToolTip.SetToolTip((Control) this.chkRounding, "SYS.X11");
      this.fieldToolTip.SetToolTip((Control) this.chkPrepaid, "2978");
      this.fieldToolTip.SetToolTip((Control) this.chkRefundUnearned, "3262");
      this.fieldToolTip.SetToolTip((Control) this.chkField3531, "3531");
      this.fieldToolTip.SetToolTip((Control) this.chkField3532, "3532");
      this.fieldToolTip.SetToolTip((Control) this.chkField3533, "3533");
      this.fieldToolTip.SetToolTip((Control) this.chkLockFactor, "3625");
      if ((forLP || lpLoan is LoanData) && lpLoan.GetField("1172") == "Conventional")
      {
        lpLoan.SetCurrentField("1775", "");
        this.balanceChkbox.Enabled = false;
      }
      this.balanceChkbox.Checked = lpLoan.GetField("1775") == "Y";
      this.fieldToolTip.SetToolTip((Control) this.balanceChkbox, "1775");
      this.midpointChkbox.Checked = lpLoan.GetField("1753") == "Y";
      this.fieldToolTip.SetToolTip((Control) this.midpointChkbox, "1753");
      this.baseLoanAmtTxt.Enabled = this.fundingFeeTxt.Enabled = this.loanAmtTxt.Enabled = false;
      if (!forLP && lpLoan.GetField("1172") == "FarmersHomeAdministration")
      {
        this.btnGetMI.Visible = false;
        this.rateMICancelTxt.Text = string.Empty;
        this.rateMICancelTxt.Enabled = false;
        this.balanceChkbox.Checked = true;
        this.balanceChkbox.Enabled = false;
        this.typeCombo.Enabled = false;
        this.midpointChkbox.Checked = false;
        this.midpointChkbox.Enabled = false;
      }
      this.chkLockFactor.CheckedChanged += new EventHandler(this.chkLockFactor_CheckedChanged);
    }

    public MIPDialog(LoanData loan)
    {
      this.loan = loan;
      this.InitializeComponent();
      this.needLoanAmountRounding = loan.GetField("4745") == "Y";
      this.inTemplate = false;
      this.cboVeteranType.Text = loan.GetField("VAVOB.X72");
      this.chkFirstTime.Checked = loan.GetField("VASUMM.X49") == "Y";
      this.chkFirstTimeValue = loan.GetField("VASUMM.X49");
      this.chkRounding.Checked = loan.GetField("SYS.X11") == "Y";
      this.baseLoanAmtTxt.Text = loan.GetField("1109");
      this.baseLoanAmount = Utils.ParseDouble((object) this.baseLoanAmtTxt.Text);
      this.fieldToolTip.SetToolTip((Control) this.baseLoanAmtTxt, "1109");
      this.rateFunding = Utils.ParseDouble((object) loan.GetField("1107"));
      this.rateFundingTxt.Text = this.rateFunding != 0.0 ? this.rateFunding.ToString("N6") : "";
      this.fundingAmtTxt.Text = loan.GetField("1826");
      this.fundingAmt = Utils.ParseDouble((object) this.fundingAmtTxt.Text);
      this.fundingFeeTxt.Text = loan.GetField("1045");
      this.fundingFee = Utils.ParseDouble((object) this.fundingFeeTxt.Text);
      this.loanAmount = this.baseLoanAmount + this.fundingFee;
      this.loanAmtTxt.Text = this.loanAmount.ToString("N2");
      if (Utils.ParseDouble((object) loan.GetField("1199")) != 0.0)
        this.rateMI1Txt.Text = loan.GetField("1199");
      this.monthMI1Txt.Text = loan.GetField("1198");
      if (Utils.ParseDouble((object) loan.GetField("1201")) != 0.0)
        this.rateMI2Txt.Text = loan.GetField("1201");
      this.monthMI2Txt.Text = loan.GetField("1200");
      this.rateMICancelTxt.Text = loan.GetField("1205");
      this.txtPrepaidMonth.Text = loan.GetField("1209");
      this.chkPrepaid.Checked = loan.GetField("2978") == "Y";
      this.chkField3531.Checked = loan.GetField("3531") == "Y";
      this.chkField3532.Checked = loan.GetField("3532") == "Y";
      this.chkField3533.Checked = loan.GetField("3533") == "Y";
      this.chkLockFactor.Checked = loan.GetField("3625") == "Y";
      this.txtPrepaidAmount.Text = loan.GetField("3971");
      this.changeMIFieldStatus();
      this.loanType = loan.GetField("1172");
      this.chkPrepaid.Enabled = !(this.loanType == "FHA");
      this.txtPrepaidMonth.ReadOnly = !this.chkPrepaid.Enabled;
      if (this.loanType != "Conventional")
      {
        loan.SetCurrentField("3248", "");
        this.chkDeclining.Enabled = false;
      }
      if (this.loanType == "FarmersHomeAdministration")
      {
        this.btnGetMI.Visible = false;
        this.midpointChkbox.Enabled = false;
        this.rateMICancelTxt.ReadOnly = true;
        this.rateMICancelTxt.Enabled = false;
        this.typeCombo.Enabled = false;
        this.rateMICancelTxt.Text = string.Empty;
        loan.SetCurrentField("1775", "Y");
        loan.SetCurrentField("1757", "Loan Amount");
        loan.SetCurrentField("1205", "");
      }
      this.chkDeclining.Checked = loan.GetField("3248") == "Y";
      this.chkRefundUnearned.Checked = loan.GetField("3262") == "Y";
      this.lockPaidInCash = loan.GetField("1765");
      if (this.lockPaidInCash == "Y")
      {
        this.lockChkbox.Checked = true;
        this.cashTxt.Text = loan.GetField("1760");
      }
      else
      {
        this.lockChkbox.Checked = false;
        this.paidInCash = this.fundingAmt - this.fundingFee;
        this.cashTxt.Text = this.paidInCash != 0.0 ? this.paidInCash.ToString("N2") : "";
      }
      switch (loan.GetField("1757"))
      {
        case "Purchase Price":
          this.priceType = 1;
          break;
        case "Appraisal Value":
          this.priceType = 2;
          break;
        case "Base Loan Amount":
          this.priceType = 3;
          break;
        default:
          this.priceType = 0;
          break;
      }
      if (this.typeCombo.Items.Count >= this.priceType)
        this.typeCombo.SelectedIndex = this.priceType;
      this.lockChkbox_Click((object) null, (EventArgs) null);
      this.CalculateMIP(this.loan);
      if (this.loanType == "Conventional")
      {
        loan.SetCurrentField("1775", "");
        this.balanceChkbox.Enabled = false;
      }
      this.balanceChkbox.Checked = loan.GetField("1775") == "Y";
      this.midpointChkbox.Checked = loan.GetField("1753") == "Y";
      this.fieldToolTip.SetToolTip((Control) this.chkFirstTime, "VASUMM.X49");
      this.fieldToolTip.SetToolTip((Control) this.rateFundingTxt, "1107");
      this.fieldToolTip.SetToolTip((Control) this.fundingAmtTxt, "1826");
      this.fieldToolTip.SetToolTip((Control) this.fundingFeeTxt, "1045");
      this.fieldToolTip.SetToolTip((Control) this.typeCombo, "1757");
      this.fieldToolTip.SetToolTip((Control) this.cboVeteranType, "VAVOB.X72");
      this.fieldToolTip.SetToolTip((Control) this.balanceChkbox, "1775");
      this.fieldToolTip.SetToolTip((Control) this.rateMI1Txt, "1199");
      this.fieldToolTip.SetToolTip((Control) this.monthMI1Txt, "1198");
      this.fieldToolTip.SetToolTip((Control) this.rateMI2Txt, "1201");
      this.fieldToolTip.SetToolTip((Control) this.monthMI2Txt, "1200");
      this.fieldToolTip.SetToolTip((Control) this.rateMICancelTxt, "1205");
      this.fieldToolTip.SetToolTip((Control) this.chkRounding, "SYS.X11");
      this.fieldToolTip.SetToolTip((Control) this.txtPrepaidMonth, "1209");
      this.fieldToolTip.SetToolTip((Control) this.chkPrepaid, "2978");
      this.fieldToolTip.SetToolTip((Control) this.chkDeclining, "3248");
      this.fieldToolTip.SetToolTip((Control) this.chkRefundUnearned, "3262");
      this.fieldToolTip.SetToolTip((Control) this.midpointChkbox, "1753");
      this.fieldToolTip.SetToolTip((Control) this.loanAmtTxt, "2");
      this.fieldToolTip.SetToolTip((Control) this.lockChkbox, "1765");
      this.fieldToolTip.SetToolTip((Control) this.cashTxt, "1760");
      this.fieldToolTip.SetToolTip((Control) this.chkField3531, "3531");
      this.fieldToolTip.SetToolTip((Control) this.chkField3532, "3532");
      this.fieldToolTip.SetToolTip((Control) this.chkField3533, "3533");
      this.fieldToolTip.SetToolTip((Control) this.chkLockFactor, "3625");
      this.setBusinessRule();
      this.rateMI1Txt.Tag = (object) "1199";
      this.monthMI1Txt.Tag = (object) "1198";
      this.rateMI2Txt.Tag = (object) "1201";
      this.monthMI2Txt.Tag = (object) "1200";
      this.rateMICancelTxt.Tag = (object) "1205";
      this.typeCombo.Tag = (object) "1757";
      this.balanceChkbox.Tag = (object) "1775";
      this.midpointChkbox.Tag = (object) "1753";
      this.cboVeteranType.Tag = (object) "VAVOB.X72";
      this.chkFirstTime.Tag = (object) "VASUMM.X49";
      this.txtPrepaidMonth.Tag = (object) "1209";
      this.chkPrepaid.Tag = (object) "2978";
      this.chkDeclining.Tag = (object) "3248";
      this.chkRefundUnearned.Tag = (object) "3262";
      this.chkField3531.Tag = (object) "3531";
      this.chkField3532.Tag = (object) "3531";
      this.chkField3533.Tag = (object) "3531";
      this.setSelectedField((object) this.baseLoanAmtTxt);
      this.setSelectedField((object) this.rateFundingTxt);
      this.setSelectedField((object) this.rateMI1Txt);
      this.setSelectedField((object) this.monthMI1Txt);
      this.setSelectedField((object) this.rateMI2Txt);
      this.setSelectedField((object) this.monthMI2Txt);
      this.setSelectedField((object) this.rateMICancelTxt);
      this.setSelectedField((object) this.typeCombo);
      this.setSelectedField((object) this.balanceChkbox);
      this.setSelectedField((object) this.midpointChkbox);
      this.setSelectedField((object) this.txtPrepaidMonth);
      this.setSelectedField((object) this.chkPrepaid);
      this.setSelectedField((object) this.chkDeclining);
      this.setSelectedField((object) this.chkRefundUnearned);
      this.setSelectedField((object) this.chkField3531);
      this.setSelectedField((object) this.chkField3532);
      this.setSelectedField((object) this.chkField3533);
      this.setSelectedField((object) this.chkLockFactor);
      this.chkLockFactor.CheckedChanged += new EventHandler(this.chkLockFactor_CheckedChanged);
      this.chkField3531.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
      this.chkField3532.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
      this.chkField3533.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
    }

    private void setBusinessRule()
    {
      ResourceManager resources = new ResourceManager(typeof (MIPDialog));
      this.popupRules = new PopupBusinessRules(this.loan, resources, (Image) resources.GetObject("pboxAsterisk.Image"), (Image) resources.GetObject("pboxDownArrow.Image"), this.session);
      this.popupRules.SetBusinessRules((object) this.baseLoanAmtTxt, "1109");
      this.popupRules.SetBusinessRules((object) this.rateFundingTxt, "1107");
      this.popupRules.SetBusinessRules((object) this.fundingAmtTxt, "1826");
      this.popupRules.SetBusinessRules((object) this.cashTxt, "1760");
      this.popupRules.SetBusinessRules((object) this.rateMI1Txt, "1199");
      this.popupRules.SetBusinessRules((object) this.monthMI1Txt, "1198");
      this.popupRules.SetBusinessRules((object) this.rateMI2Txt, "1201");
      this.popupRules.SetBusinessRules((object) this.monthMI2Txt, "1200");
      this.popupRules.SetBusinessRules((object) this.rateMICancelTxt, "1205");
      this.popupRules.SetBusinessRules((object) this.midpointChkbox, "1753");
      this.popupRules.SetBusinessRules((object) this.typeCombo, "1757");
      this.popupRules.SetBusinessRules((object) this.balanceChkbox, "1775");
      this.popupRules.SetBusinessRules((object) this.cboVeteranType, "VAVOB.X72");
      this.popupRules.SetBusinessRules((object) this.chkFirstTime, "VASUMM.X49");
      this.popupRules.SetButtonAccessMode(this.btnGetMI, "getmi");
      this.popupRules.SetBusinessRules((object) this.chkRounding, "SYS.X11");
      this.popupRules.SetBusinessRules((object) this.txtPrepaidMonth, "1209");
      this.popupRules.SetBusinessRules((object) this.chkPrepaid, "2978");
      this.popupRules.SetBusinessRules((object) this.chkDeclining, "3248");
      this.popupRules.SetBusinessRules((object) this.chkRefundUnearned, "3262");
      this.popupRules.SetBusinessRules((object) this.lockChkbox, "1765");
      this.popupRules.SetBusinessRules((object) this.chkField3531, "3531");
      this.popupRules.SetBusinessRules((object) this.chkField3532, "3532");
      this.popupRules.SetBusinessRules((object) this.chkField3533, "3533");
      this.popupRules.SetBusinessRules((object) this.chkLockFactor, "3625");
      if (this.rateFundingTxt.ReadOnly)
        this.rateFundingTxt.BackColor = SystemColors.Control;
      if (this.fundingAmtTxt.ReadOnly)
        this.fundingAmtTxt.BackColor = SystemColors.Control;
      if (!this.cashTxt.ReadOnly)
        return;
      this.cashTxt.BackColor = SystemColors.Control;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MIPDialog));
      this.okBtn = new Button();
      this.cancelBtn = new Button();
      this.chkPrepaid = new CheckBox();
      this.txtPrepaidMonth = new TextBox();
      this.labelPrepaid = new Label();
      this.chkRounding = new CheckBox();
      this.cashTxt = new TextBox();
      this.fundingAmtTxt = new TextBox();
      this.fundingFeeTxt = new TextBox();
      this.lockChkbox = new CheckBox();
      this.label6 = new Label();
      this.label5 = new Label();
      this.loanAmtTxt = new TextBox();
      this.label4 = new Label();
      this.label3 = new Label();
      this.rateFundingTxt = new TextBox();
      this.label2 = new Label();
      this.baseLoanAmtTxt = new TextBox();
      this.label1 = new Label();
      this.btnGetMI = new Button();
      this.midpointChkbox = new CheckBox();
      this.balanceChkbox = new CheckBox();
      this.label15 = new Label();
      this.typeCombo = new ComboBox();
      this.rateMICancelTxt = new TextBox();
      this.monthMI2Txt = new TextBox();
      this.monthMI1Txt = new TextBox();
      this.label14 = new Label();
      this.label13 = new Label();
      this.label12 = new Label();
      this.rateMI2Txt = new TextBox();
      this.rateMI1Txt = new TextBox();
      this.label11 = new Label();
      this.label10 = new Label();
      this.label9 = new Label();
      this.label8 = new Label();
      this.label7 = new Label();
      this.fieldToolTip = new ToolTip(this.components);
      this.pboxAsterisk = new PictureBox();
      this.pboxDownArrow = new PictureBox();
      this.chkFirstTime = new CheckBox();
      this.label17 = new Label();
      this.cboVeteranType = new ComboBox();
      this.groupBoxTop = new GroupContainer();
      this.chkLockFactor = new CheckBox();
      this.chkField3533 = new CheckBox();
      this.chkField3532 = new CheckBox();
      this.chkField3531 = new CheckBox();
      this.chkRefundUnearned = new CheckBox();
      this.groupBoxMiddle = new GroupContainer();
      this.txtPrepaidAmount = new TextBox();
      this.labelPrepaidAmount = new Label();
      this.chkDeclining = new CheckBox();
      this.groupBoxFHA = new GroupContainer();
      this.pnlBody = new Panel();
      this.emHelpLink1 = new EMHelpLink();
      ((ISupportInitialize) this.pboxAsterisk).BeginInit();
      ((ISupportInitialize) this.pboxDownArrow).BeginInit();
      this.groupBoxTop.SuspendLayout();
      this.groupBoxMiddle.SuspendLayout();
      this.groupBoxFHA.SuspendLayout();
      this.pnlBody.SuspendLayout();
      this.SuspendLayout();
      this.okBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okBtn.DialogResult = DialogResult.OK;
      this.okBtn.Location = new Point(235, 606);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 24);
      this.okBtn.TabIndex = 25;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(316, 606);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 24);
      this.cancelBtn.TabIndex = 26;
      this.cancelBtn.Text = "&Cancel";
      this.chkPrepaid.Location = new Point(194, 186);
      this.chkPrepaid.Name = "chkPrepaid";
      this.chkPrepaid.Size = new Size(75, 16);
      this.chkPrepaid.TabIndex = 31;
      this.chkPrepaid.Text = "Prepaid";
      this.chkPrepaid.Leave += new EventHandler(this.leave);
      this.txtPrepaidMonth.Location = new Point(260, 183);
      this.txtPrepaidMonth.MaxLength = 3;
      this.txtPrepaidMonth.Name = "txtPrepaidMonth";
      this.txtPrepaidMonth.Size = new Size(95, 20);
      this.txtPrepaidMonth.TabIndex = 32;
      this.txtPrepaidMonth.TextAlign = HorizontalAlignment.Right;
      this.txtPrepaidMonth.KeyPress += new KeyPressEventHandler(this.keypress);
      this.txtPrepaidMonth.KeyUp += new KeyEventHandler(this.keyup);
      this.txtPrepaidMonth.Leave += new EventHandler(this.leave);
      this.txtPrepaidMonth.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.labelPrepaid.AutoSize = true;
      this.labelPrepaid.Location = new Point(4, 186);
      this.labelPrepaid.Name = "labelPrepaid";
      this.labelPrepaid.Size = new Size(185, 13);
      this.labelPrepaid.TabIndex = 16;
      this.labelPrepaid.Text = "Number of Months MI being Collected";
      this.chkRounding.Location = new Point(7, 160);
      this.chkRounding.Name = "chkRounding";
      this.chkRounding.Size = new Size(204, 16);
      this.chkRounding.TabIndex = 11;
      this.chkRounding.Text = "Round to nearest $50";
      this.chkRounding.Click += new EventHandler(this.lockChkbox_Click);
      this.chkRounding.Leave += new EventHandler(this.leave);
      this.cashTxt.Location = new Point(275, 74);
      this.cashTxt.MaxLength = 10;
      this.cashTxt.Name = "cashTxt";
      this.cashTxt.Size = new Size(95, 20);
      this.cashTxt.TabIndex = 4;
      this.cashTxt.TextAlign = HorizontalAlignment.Right;
      this.cashTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.cashTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.cashTxt.Leave += new EventHandler(this.leave);
      this.cashTxt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.fundingAmtTxt.Location = new Point(275, 52);
      this.fundingAmtTxt.MaxLength = 10;
      this.fundingAmtTxt.Name = "fundingAmtTxt";
      this.fundingAmtTxt.Size = new Size(95, 20);
      this.fundingAmtTxt.TabIndex = 2;
      this.fundingAmtTxt.TextAlign = HorizontalAlignment.Right;
      this.fundingAmtTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.fundingAmtTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.fundingAmtTxt.Leave += new EventHandler(this.leave);
      this.fundingAmtTxt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.fundingFeeTxt.Location = new Point(275, 96);
      this.fundingFeeTxt.MaxLength = 16;
      this.fundingFeeTxt.Name = "fundingFeeTxt";
      this.fundingFeeTxt.ReadOnly = true;
      this.fundingFeeTxt.Size = new Size(95, 20);
      this.fundingFeeTxt.TabIndex = 6;
      this.fundingFeeTxt.TabStop = false;
      this.fundingFeeTxt.TextAlign = HorizontalAlignment.Right;
      this.lockChkbox.Location = new Point(182, 77);
      this.lockChkbox.Name = "lockChkbox";
      this.lockChkbox.Size = new Size(59, 16);
      this.lockChkbox.TabIndex = 3;
      this.lockChkbox.Text = "Lock";
      this.lockChkbox.Click += new EventHandler(this.lockChkbox_Click);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(4, 121);
      this.label6.Name = "label6";
      this.label6.Size = new Size(216, 13);
      this.label6.TabIndex = 13;
      this.label6.Text = "Loan Amount with Upfront MIP/Funding Fee";
      this.label5.AutoSize = true;
      this.label5.Location = new Point(4, 99);
      this.label5.Name = "label5";
      this.label5.Size = new Size(230, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Upfront MIP/Funding/Guarantee Fee Financed";
      this.loanAmtTxt.Location = new Point(275, 118);
      this.loanAmtTxt.MaxLength = 16;
      this.loanAmtTxt.Name = "loanAmtTxt";
      this.loanAmtTxt.ReadOnly = true;
      this.loanAmtTxt.Size = new Size(95, 20);
      this.loanAmtTxt.TabIndex = 7;
      this.loanAmtTxt.TabStop = false;
      this.loanAmtTxt.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Location = new Point(4, 77);
      this.label4.Name = "label4";
      this.label4.Size = new Size(105, 13);
      this.label4.TabIndex = 9;
      this.label4.Text = "Amount Paid in Cash";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(247, 56);
      this.label3.Name = "label3";
      this.label3.Size = new Size(15, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "%";
      this.rateFundingTxt.Location = new Point(182, 53);
      this.rateFundingTxt.MaxLength = 10;
      this.rateFundingTxt.Name = "rateFundingTxt";
      this.rateFundingTxt.Size = new Size(63, 20);
      this.rateFundingTxt.TabIndex = 1;
      this.rateFundingTxt.TextAlign = HorizontalAlignment.Right;
      this.rateFundingTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateFundingTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateFundingTxt.Leave += new EventHandler(this.leave);
      this.rateFundingTxt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(4, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(136, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "MIP / Funding / Guarantee";
      this.baseLoanAmtTxt.Location = new Point(275, 30);
      this.baseLoanAmtTxt.MaxLength = 16;
      this.baseLoanAmtTxt.Name = "baseLoanAmtTxt";
      this.baseLoanAmtTxt.Size = new Size(95, 20);
      this.baseLoanAmtTxt.TabIndex = 0;
      this.baseLoanAmtTxt.TextAlign = HorizontalAlignment.Right;
      this.baseLoanAmtTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.baseLoanAmtTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.baseLoanAmtTxt.Leave += new EventHandler(this.leave);
      this.baseLoanAmtTxt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(4, 33);
      this.label1.Name = "label1";
      this.label1.Size = new Size(97, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Base Loan Amount";
      this.btnGetMI.BackColor = SystemColors.Control;
      this.btnGetMI.Location = new Point(288, 29);
      this.btnGetMI.Name = "btnGetMI";
      this.btnGetMI.Size = new Size(65, 23);
      this.btnGetMI.TabIndex = 14;
      this.btnGetMI.Text = "Get MI";
      this.btnGetMI.UseVisualStyleBackColor = true;
      this.btnGetMI.Click += new EventHandler(this.btnGetMI_Click);
      this.midpointChkbox.Location = new Point(9, 141);
      this.midpointChkbox.Name = "midpointChkbox";
      this.midpointChkbox.Size = new Size(235, 16);
      this.midpointChkbox.TabIndex = 21;
      this.midpointChkbox.Text = "Midpoint payment cancellation";
      this.midpointChkbox.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.balanceChkbox.Location = new Point(9, 122);
      this.balanceChkbox.Name = "balanceChkbox";
      this.balanceChkbox.Size = new Size(235, 16);
      this.balanceChkbox.TabIndex = 20;
      this.balanceChkbox.Text = "Calculate based on remaining balance";
      this.balanceChkbox.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.label15.AutoSize = true;
      this.label15.Location = new Point(4, 34);
      this.label15.Name = "label15";
      this.label15.Size = new Size(107, 13);
      this.label15.TabIndex = 29;
      this.label15.Text = "Calculated Based On";
      this.typeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
      this.typeCombo.Items.AddRange(new object[4]
      {
        (object) "Loan Amount",
        (object) "Purchase Price",
        (object) "Appraisal Value",
        (object) "Base Loan Amount"
      });
      this.typeCombo.Location = new Point((int) sbyte.MaxValue, 30);
      this.typeCombo.Name = "typeCombo";
      this.typeCombo.Size = new Size(155, 21);
      this.typeCombo.TabIndex = 13;
      this.typeCombo.Leave += new EventHandler(this.leave);
      this.typeCombo.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.rateMICancelTxt.Location = new Point((int) sbyte.MaxValue, 97);
      this.rateMICancelTxt.MaxLength = 7;
      this.rateMICancelTxt.Name = "rateMICancelTxt";
      this.rateMICancelTxt.Size = new Size(65, 20);
      this.rateMICancelTxt.TabIndex = 19;
      this.rateMICancelTxt.TextAlign = HorizontalAlignment.Right;
      this.rateMICancelTxt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMICancelTxt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateMICancelTxt.Leave += new EventHandler(this.leave);
      this.rateMICancelTxt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.monthMI2Txt.Location = new Point((int) sbyte.MaxValue, 75);
      this.monthMI2Txt.MaxLength = 3;
      this.monthMI2Txt.Name = "monthMI2Txt";
      this.monthMI2Txt.Size = new Size(65, 20);
      this.monthMI2Txt.TabIndex = 18;
      this.monthMI2Txt.TextAlign = HorizontalAlignment.Right;
      this.monthMI2Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.monthMI2Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.monthMI2Txt.Leave += new EventHandler(this.leave);
      this.monthMI2Txt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.monthMI1Txt.Location = new Point((int) sbyte.MaxValue, 53);
      this.monthMI1Txt.MaxLength = 3;
      this.monthMI1Txt.Name = "monthMI1Txt";
      this.monthMI1Txt.Size = new Size(65, 20);
      this.monthMI1Txt.TabIndex = 16;
      this.monthMI1Txt.TextAlign = HorizontalAlignment.Right;
      this.monthMI1Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.monthMI1Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.monthMI1Txt.Leave += new EventHandler(this.leave);
      this.monthMI1Txt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.label14.AutoSize = true;
      this.label14.Location = new Point(63, 102);
      this.label14.Name = "label14";
      this.label14.Size = new Size(53, 13);
      this.label14.TabIndex = 24;
      this.label14.Text = "Cancel At";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(206, 80);
      this.label13.Name = "label13";
      this.label13.Size = new Size(42, 13);
      this.label13.TabIndex = 23;
      this.label13.Text = "Months";
      this.label12.AutoSize = true;
      this.label12.Location = new Point(206, 58);
      this.label12.Name = "label12";
      this.label12.Size = new Size(42, 13);
      this.label12.TabIndex = 22;
      this.label12.Text = "Months";
      this.rateMI2Txt.Location = new Point(31, 75);
      this.rateMI2Txt.MaxLength = 9;
      this.rateMI2Txt.Name = "rateMI2Txt";
      this.rateMI2Txt.Size = new Size(63, 20);
      this.rateMI2Txt.TabIndex = 17;
      this.rateMI2Txt.TextAlign = HorizontalAlignment.Right;
      this.rateMI2Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMI2Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateMI2Txt.Leave += new EventHandler(this.leave);
      this.rateMI2Txt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.rateMI1Txt.Location = new Point(31, 53);
      this.rateMI1Txt.MaxLength = 9;
      this.rateMI1Txt.Name = "rateMI1Txt";
      this.rateMI1Txt.Size = new Size(63, 20);
      this.rateMI1Txt.TabIndex = 15;
      this.rateMI1Txt.TextAlign = HorizontalAlignment.Right;
      this.rateMI1Txt.KeyPress += new KeyPressEventHandler(this.keypress);
      this.rateMI1Txt.KeyUp += new KeyEventHandler(this.keyup);
      this.rateMI1Txt.Leave += new EventHandler(this.leave);
      this.rateMI1Txt.MouseDown += new MouseEventHandler(this.field_MouseDown);
      this.label11.AutoSize = true;
      this.label11.Location = new Point(206, 102);
      this.label11.Name = "label11";
      this.label11.Size = new Size(15, 13);
      this.label11.TabIndex = 19;
      this.label11.Text = "%";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(105, 80);
      this.label10.Name = "label10";
      this.label10.Size = new Size(15, 13);
      this.label10.TabIndex = 18;
      this.label10.Text = "%";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(105, 58);
      this.label9.Name = "label9";
      this.label9.Size = new Size(15, 13);
      this.label9.TabIndex = 17;
      this.label9.Text = "%";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(9, 80);
      this.label8.Name = "label8";
      this.label8.Size = new Size(16, 13);
      this.label8.TabIndex = 16;
      this.label8.Text = "2.";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(9, 58);
      this.label7.Name = "label7";
      this.label7.Size = new Size(16, 13);
      this.label7.TabIndex = 15;
      this.label7.Text = "1.";
      this.pboxAsterisk.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxAsterisk.Image = (Image) componentResourceManager.GetObject("pboxAsterisk.Image");
      this.pboxAsterisk.Location = new Point(159, 612);
      this.pboxAsterisk.Name = "pboxAsterisk";
      this.pboxAsterisk.Size = new Size(24, 12);
      this.pboxAsterisk.TabIndex = 17;
      this.pboxAsterisk.TabStop = false;
      this.pboxAsterisk.Visible = false;
      this.pboxDownArrow.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.pboxDownArrow.Image = (Image) componentResourceManager.GetObject("pboxDownArrow.Image");
      this.pboxDownArrow.Location = new Point(194, 610);
      this.pboxDownArrow.Name = "pboxDownArrow";
      this.pboxDownArrow.Size = new Size(17, 17);
      this.pboxDownArrow.TabIndex = 68;
      this.pboxDownArrow.TabStop = false;
      this.pboxDownArrow.Visible = false;
      this.chkFirstTime.AutoSize = true;
      this.chkFirstTime.Location = new Point(8, 54);
      this.chkFirstTime.Name = "chkFirstTime";
      this.chkFirstTime.Size = new Size(227, 17);
      this.chkFirstTime.TabIndex = 24;
      this.chkFirstTime.Text = "Is this the first use of the VA loan program?";
      this.chkFirstTime.UseVisualStyleBackColor = true;
      this.chkFirstTime.Click += new EventHandler(this.chkFirstTime_Click);
      this.label17.AutoSize = true;
      this.label17.Location = new Point(4, 32);
      this.label17.Name = "label17";
      this.label17.Size = new Size(83, 13);
      this.label17.TabIndex = 11;
      this.label17.Text = "Type of Veteran";
      this.cboVeteranType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cboVeteranType.Items.AddRange(new object[4]
      {
        (object) "",
        (object) "Regular Military",
        (object) "Reserves",
        (object) "National Guard"
      });
      this.cboVeteranType.Location = new Point(192, 29);
      this.cboVeteranType.Name = "cboVeteranType";
      this.cboVeteranType.Size = new Size(125, 21);
      this.cboVeteranType.TabIndex = 23;
      this.groupBoxTop.Borders = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.groupBoxTop.Controls.Add((Control) this.chkLockFactor);
      this.groupBoxTop.Controls.Add((Control) this.chkField3533);
      this.groupBoxTop.Controls.Add((Control) this.chkField3532);
      this.groupBoxTop.Controls.Add((Control) this.chkField3531);
      this.groupBoxTop.Controls.Add((Control) this.chkRefundUnearned);
      this.groupBoxTop.Controls.Add((Control) this.label1);
      this.groupBoxTop.Controls.Add((Control) this.baseLoanAmtTxt);
      this.groupBoxTop.Controls.Add((Control) this.label2);
      this.groupBoxTop.Controls.Add((Control) this.chkRounding);
      this.groupBoxTop.Controls.Add((Control) this.rateFundingTxt);
      this.groupBoxTop.Controls.Add((Control) this.cashTxt);
      this.groupBoxTop.Controls.Add((Control) this.label3);
      this.groupBoxTop.Controls.Add((Control) this.fundingAmtTxt);
      this.groupBoxTop.Controls.Add((Control) this.label4);
      this.groupBoxTop.Controls.Add((Control) this.fundingFeeTxt);
      this.groupBoxTop.Controls.Add((Control) this.loanAmtTxt);
      this.groupBoxTop.Controls.Add((Control) this.lockChkbox);
      this.groupBoxTop.Controls.Add((Control) this.label5);
      this.groupBoxTop.Controls.Add((Control) this.label6);
      this.groupBoxTop.Dock = DockStyle.Top;
      this.groupBoxTop.HeaderForeColor = SystemColors.ControlText;
      this.groupBoxTop.Location = new Point(0, 0);
      this.groupBoxTop.Name = "groupBoxTop";
      this.groupBoxTop.Size = new Size(381, 261);
      this.groupBoxTop.TabIndex = 0;
      this.groupBoxTop.Text = "Upfront Mortgage Insurance Premium / Funding / Guarantee Fee";
      this.chkLockFactor.Location = new Point(7, 236);
      this.chkLockFactor.Name = "chkLockFactor";
      this.chkLockFactor.Size = new Size(135, 18);
      this.chkLockFactor.TabIndex = 15;
      this.chkLockFactor.Text = "MI Factor Field Locked";
      this.chkField3533.AutoSize = true;
      this.chkField3533.Location = new Point(7, 217);
      this.chkField3533.Name = "chkField3533";
      this.chkField3533.Size = new Size(181, 17);
      this.chkField3533.TabIndex = 14;
      this.chkField3533.Text = "Lender Paid Mortgage Insurance";
      this.chkField3533.UseVisualStyleBackColor = true;
      this.chkField3532.AutoSize = true;
      this.chkField3532.Location = new Point(7, 198);
      this.chkField3532.Name = "chkField3532";
      this.chkField3532.Size = new Size(318, 17);
      this.chkField3532.TabIndex = 13;
      this.chkField3532.Text = "Charges for the insurance are collected upfront at loan closing";
      this.chkField3532.UseVisualStyleBackColor = true;
      this.chkField3531.AutoSize = true;
      this.chkField3531.Location = new Point(7, 179);
      this.chkField3531.Name = "chkField3531";
      this.chkField3531.Size = new Size(304, 17);
      this.chkField3531.TabIndex = 12;
      this.chkField3531.Text = "Charges for the insurance are added to your loan payments";
      this.chkField3531.UseVisualStyleBackColor = true;
      this.chkRefundUnearned.Location = new Point(7, 141);
      this.chkRefundUnearned.Name = "chkRefundUnearned";
      this.chkRefundUnearned.Size = new Size(204, 16);
      this.chkRefundUnearned.TabIndex = 10;
      this.chkRefundUnearned.Text = "Refund prorated unearned Up Front MI Premiums";
      this.groupBoxMiddle.Controls.Add((Control) this.txtPrepaidMonth);
      this.groupBoxMiddle.Controls.Add((Control) this.txtPrepaidAmount);
      this.groupBoxMiddle.Controls.Add((Control) this.labelPrepaidAmount);
      this.groupBoxMiddle.Controls.Add((Control) this.chkDeclining);
      this.groupBoxMiddle.Controls.Add((Control) this.btnGetMI);
      this.groupBoxMiddle.Controls.Add((Control) this.label15);
      this.groupBoxMiddle.Controls.Add((Control) this.midpointChkbox);
      this.groupBoxMiddle.Controls.Add((Control) this.label7);
      this.groupBoxMiddle.Controls.Add((Control) this.chkPrepaid);
      this.groupBoxMiddle.Controls.Add((Control) this.balanceChkbox);
      this.groupBoxMiddle.Controls.Add((Control) this.label8);
      this.groupBoxMiddle.Controls.Add((Control) this.label9);
      this.groupBoxMiddle.Controls.Add((Control) this.labelPrepaid);
      this.groupBoxMiddle.Controls.Add((Control) this.typeCombo);
      this.groupBoxMiddle.Controls.Add((Control) this.label10);
      this.groupBoxMiddle.Controls.Add((Control) this.rateMICancelTxt);
      this.groupBoxMiddle.Controls.Add((Control) this.label11);
      this.groupBoxMiddle.Controls.Add((Control) this.monthMI2Txt);
      this.groupBoxMiddle.Controls.Add((Control) this.rateMI1Txt);
      this.groupBoxMiddle.Controls.Add((Control) this.monthMI1Txt);
      this.groupBoxMiddle.Controls.Add((Control) this.rateMI2Txt);
      this.groupBoxMiddle.Controls.Add((Control) this.label14);
      this.groupBoxMiddle.Controls.Add((Control) this.label12);
      this.groupBoxMiddle.Controls.Add((Control) this.label13);
      this.groupBoxMiddle.Dock = DockStyle.Top;
      this.groupBoxMiddle.HeaderForeColor = SystemColors.ControlText;
      this.groupBoxMiddle.Location = new Point(0, 261);
      this.groupBoxMiddle.Name = "groupBoxMiddle";
      this.groupBoxMiddle.Size = new Size(381, 238);
      this.groupBoxMiddle.TabIndex = 15;
      this.groupBoxMiddle.Text = "Monthly Mortgage Insurance";
      this.txtPrepaidAmount.Location = new Point(260, 205);
      this.txtPrepaidAmount.MaxLength = 3;
      this.txtPrepaidAmount.Name = "txtPrepaidAmount";
      this.txtPrepaidAmount.ReadOnly = true;
      this.txtPrepaidAmount.Size = new Size(95, 20);
      this.txtPrepaidAmount.TabIndex = 0;
      this.txtPrepaidAmount.TabStop = false;
      this.txtPrepaidAmount.TextAlign = HorizontalAlignment.Right;
      this.labelPrepaidAmount.AutoSize = true;
      this.labelPrepaidAmount.Location = new Point(171, 208);
      this.labelPrepaidAmount.Name = "labelPrepaidAmount";
      this.labelPrepaidAmount.Size = new Size(82, 13);
      this.labelPrepaidAmount.TabIndex = 31;
      this.labelPrepaidAmount.Text = "Prepaid Amount";
      this.chkDeclining.Location = new Point(9, 161);
      this.chkDeclining.Name = "chkDeclining";
      this.chkDeclining.Size = new Size(235, 18);
      this.chkDeclining.TabIndex = 30;
      this.chkDeclining.Text = "Declining Renewals";
      this.groupBoxFHA.Controls.Add((Control) this.chkFirstTime);
      this.groupBoxFHA.Controls.Add((Control) this.label17);
      this.groupBoxFHA.Controls.Add((Control) this.cboVeteranType);
      this.groupBoxFHA.Dock = DockStyle.Top;
      this.groupBoxFHA.HeaderForeColor = SystemColors.ControlText;
      this.groupBoxFHA.Location = new Point(0, 499);
      this.groupBoxFHA.Name = "groupBoxFHA";
      this.groupBoxFHA.Size = new Size(381, 77);
      this.groupBoxFHA.TabIndex = 22;
      this.groupBoxFHA.Text = "Mortgage Insurance (FHA and VA)";
      this.pnlBody.Controls.Add((Control) this.groupBoxFHA);
      this.pnlBody.Controls.Add((Control) this.groupBoxMiddle);
      this.pnlBody.Controls.Add((Control) this.groupBoxTop);
      this.pnlBody.Location = new Point(12, 12);
      this.pnlBody.Name = "pnlBody";
      this.pnlBody.Size = new Size(381, 576);
      this.pnlBody.TabIndex = 69;
      this.emHelpLink1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.emHelpLink1.BackColor = Color.Transparent;
      this.emHelpLink1.Cursor = Cursors.Hand;
      this.emHelpLink1.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.emHelpLink1.HelpTag = "MIP Dialog";
      this.emHelpLink1.Location = new Point(12, 611);
      this.emHelpLink1.Name = "emHelpLink1";
      this.emHelpLink1.Size = new Size(90, 16);
      this.emHelpLink1.TabIndex = 27;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(404, 642);
      this.Controls.Add((Control) this.emHelpLink1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.pnlBody);
      this.Controls.Add((Control) this.pboxDownArrow);
      this.Controls.Add((Control) this.pboxAsterisk);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (MIPDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "MIP/PMI/Guarantee Fee Calculation";
      this.KeyPress += new KeyPressEventHandler(this.MIPDialog_KeyPress);
      ((ISupportInitialize) this.pboxAsterisk).EndInit();
      ((ISupportInitialize) this.pboxDownArrow).EndInit();
      this.groupBoxTop.ResumeLayout(false);
      this.groupBoxTop.PerformLayout();
      this.groupBoxMiddle.ResumeLayout(false);
      this.groupBoxMiddle.PerformLayout();
      this.groupBoxFHA.ResumeLayout(false);
      this.groupBoxFHA.PerformLayout();
      this.pnlBody.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void leave(object sender, EventArgs e)
    {
      if (sender != null && sender is TextBox)
      {
        TextBox ctrl = (TextBox) sender;
        if (!ctrl.ReadOnly)
        {
          if (!this.inTemplate && !this.popupRules.RuleValidate((object) ctrl, (string) ctrl.Tag))
            return;
          if (Utils.ParseDouble((object) ctrl.Text) == 0.0)
            ctrl.Text = "";
          else if (ctrl.Name == "rateMI1Txt" || ctrl.Name == "rateMI2Txt")
            ctrl.Text = Utils.ParseDouble((object) ctrl.Text).ToString("N6");
        }
      }
      if (this.inTemplate)
        this.CalculateLPMIP();
      else
        this.CalculateMIP(this.loan);
    }

    private void CalculateLPMIP()
    {
      this.lockPaidInCash = !this.lockChkbox.Checked ? "N" : "Y";
      this.rateFunding = Utils.ParseDouble((object) this.rateFundingTxt.Text);
      if (this.rateFunding != 0.0)
        this.rateFundingTxt.Text = this.rateFunding.ToString("N6");
      else
        this.rateFundingTxt.Text = "";
      this.fundingAmt = Utils.ParseDouble((object) this.fundingAmtTxt.Text);
      if (this.fundingAmt != 0.0)
        this.fundingAmtTxt.Text = this.fundingAmt.ToString("N2");
      else
        this.fundingAmtTxt.Text = "";
      this.paidInCash = Utils.ParseDouble((object) this.cashTxt.Text);
      if (this.paidInCash != 0.0)
        this.cashTxt.Text = this.paidInCash.ToString("N2");
      else
        this.cashTxt.Text = "";
      this.priceType = this.typeCombo.SelectedIndex;
    }

    private void CalculateMIP(LoanData loanData)
    {
      string field = loanData.GetField("1172");
      double num1 = !(field == "FarmersHomeAdministration") || loanData.IsLocked("NEWHUD.X1301") ? Utils.ParseDouble((object) loanData.GetField("562")) : Utils.ParseDouble((object) loanData.GetField("NEWHUD.X1302"));
      bool flag1 = field == "FHA";
      bool flag2 = this.chkRounding.Checked;
      this.lockPaidInCash = this.lockChkbox.Checked ? "Y" : "N";
      this.baseLoanAmount = Utils.ParseDouble((object) this.baseLoanAmtTxt.Text);
      if (flag1 || this.needLoanAmountRounding)
        this.baseLoanAmount = Utils.ArithmeticRounding(this.baseLoanAmount, 0);
      if (this.lockPaidInCash == "Y")
      {
        this.fundingAmt = Utils.ParseDouble((object) this.fundingAmtTxt.Text);
        this.rateFunding = 0.0;
        if (this.chkLockFactor.Checked)
          this.rateFunding = Utils.ArithmeticRounding(Utils.ParseDouble((object) this.rateFundingTxt.Text), 6);
        else if (this.baseLoanAmount != 0.0)
          this.rateFunding = Utils.ArithmeticRounding(this.fundingAmt / this.baseLoanAmount * 100.0, 6);
        this.paidInCash = Utils.ParseDouble((object) this.cashTxt.Text);
      }
      else
      {
        this.rateFunding = Utils.ParseDouble((object) this.rateFundingTxt.Text);
        this.rateFunding = Utils.ArithmeticRounding(this.rateFunding, 6);
        if (this.chkLockFactor.Checked && this.lockChkbox.Checked)
        {
          this.fundingAmt = Utils.ParseDouble((object) this.fundingAmtTxt.Text);
        }
        else
        {
          this.fundingAmt = this.baseLoanAmount * this.rateFunding / 100.0;
          this.fundingAmt = !flag1 ? Utils.ArithmeticRounding(this.fundingAmt, 2) : Convert.ToDouble(Math.Truncate(100M * Convert.ToDecimal(this.fundingAmt)) / 100M);
        }
        this.paidInCash = Utils.ArithmeticRounding(this.fundingAmt % 1.0, 2);
      }
      if (flag2)
        this.paidInCash = this.fundingAmt - (double) ((int) (this.fundingAmt / 50.0) * 50);
      if (flag1 || this.needLoanAmountRounding)
      {
        double num2 = this.fundingAmt - num1;
        if (num2 >= 0.0)
          this.paidInCash = (double) (int) this.paidInCash + Utils.ArithmeticRounding(num2 % 1.0, 2);
      }
      this.fundingFee = this.fundingAmt - this.paidInCash - num1;
      if (this.fundingFee < 0.0)
        this.fundingFee = 0.0;
      this.loanAmount = this.baseLoanAmount + this.fundingFee;
      this.rateMI1 = Utils.ParseDouble((object) this.rateMI1Txt.Text);
      this.rateMI2 = Utils.ParseDouble((object) this.rateMI2Txt.Text);
      this.rateMICancel = Utils.ParseDouble((object) this.rateMICancelTxt.Text);
      this.rateMICancelTxt.Text = this.rateMICancel != 0.0 ? this.rateMICancel.ToString("N3") : "";
      if (this.typeCombo.SelectedItem != null)
      {
        switch (this.typeCombo.SelectedItem.ToString())
        {
          case "Loan Amount":
            this.propertyValue = this.loanAmount;
            break;
          case "Purchase Price":
            this.propertyValue = Utils.ParseDouble((object) loanData.GetField("136"));
            break;
          case "Appraisal Value":
            this.propertyValue = Utils.ParseDouble((object) loanData.GetField("356"));
            break;
          case "Base Loan Amount":
            this.propertyValue = this.baseLoanAmount;
            break;
        }
      }
      this.amountMI1 = Utils.ArithmeticRounding(this.rateMI1 / 100.0 * this.propertyValue / 12.0, 2);
      this.amountMI2 = Utils.ArithmeticRounding(this.rateMI2 / 100.0 * this.propertyValue / 12.0, 2);
      this.prepaidMIAmount = Utils.ArithmeticRounding(this.amountMI1 * (double) Utils.ParseInt((object) this.txtPrepaidMonth.Text.Trim(), 0), 2);
      this.priceType = this.typeCombo.SelectedIndex;
      this.baseLoanAmtTxt.Text = this.baseLoanAmount != 0.0 ? this.baseLoanAmount.ToString("N2") : "";
      this.rateFundingTxt.Text = this.rateFunding != 0.0 ? this.rateFunding.ToString("N6") : "";
      this.fundingAmtTxt.Text = this.fundingAmt != 0.0 ? this.fundingAmt.ToString("N2") : "";
      this.cashTxt.Text = this.paidInCash != 0.0 ? this.paidInCash.ToString("N2") : "";
      this.fundingFeeTxt.Text = this.fundingFee != 0.0 ? this.fundingFee.ToString("N2") : "";
      this.loanAmtTxt.Text = this.loanAmount != 0.0 ? this.loanAmount.ToString("N2") : "";
      this.txtPrepaidAmount.Text = this.prepaidMIAmount != 0.0 ? this.prepaidMIAmount.ToString("N2") : "";
    }

    private void lockChkbox_Click(object sender, EventArgs e)
    {
      this.changeMIFieldStatus();
      if (!this.lockChkbox.Checked)
        this.chkLockFactor.Checked = false;
      if (this.inTemplate)
        this.CalculateLPMIP();
      else
        this.CalculateMIP(this.loan);
      this.setBusinessRule();
      this.lockChkbox.Focus();
    }

    private void chkFirstTime_Click(object sender, EventArgs e)
    {
      this.chkFirstTimeValue = this.chkFirstTime.Checked ? "Y" : "N";
    }

    private void keypress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar.Equals((object) Keys.Delete) || char.IsControl(e.KeyChar))
        return;
      TextBox textBox = (TextBox) sender;
      if (textBox != null && textBox.Name == "txtPrepaidMonth")
      {
        if (char.IsDigit(e.KeyChar))
          e.Handled = false;
        else
          e.Handled = true;
      }
      else if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals('.'))
        e.Handled = false;
      else
        e.Handled = true;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.inTemplate)
      {
        this.CalculateLPMIP();
        this.setLPLoanData();
      }
      else if (this.loan.GetField("1172") == "FarmersHomeAdministration" && !this.loan.IsLocked("NEWHUD.X1301") && this.loan.GetField("NEWHUD.X1299").ToLower() != "guarantee fee" && (this.loan.GetField("NEWHUD.X1299") != string.Empty || this.loan.GetField("NEWHUD.X1300") != string.Empty || this.loan.GetField("NEWHUD.X1301") != string.Empty || this.loan.GetField("NEWHUD.X1302") != string.Empty))
      {
        if (Utils.Dialog((IWin32Window) this, "The Guarantee Fee cannot be populated to line 819 in " + (Utils.CheckIf2015RespaTila(this.loan.GetField("3969")) ? "2015 Itemization" : "2010 Itemization") + " because this line is not blank. Do you want to overwrite it?", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
          this.setLoanData(this.loan);
        else
          this.DialogResult = DialogResult.Cancel;
      }
      else
        this.setLoanData(this.loan);
    }

    private void setLoanData(LoanData loanData)
    {
      string field = loanData.GetField("1172");
      if (field == "FHA")
      {
        this.priceType = 3;
        this.balanceChkbox.Checked = true;
      }
      bool flag = loanData.IsLocked("NEWHUD.X1301");
      loanData.SetCurrentField("SYS.X11", this.chkRounding.Checked ? "Y" : "N");
      loanData.SetCurrentField("VAVOB.X72", this.cboVeteranType.Text.Trim());
      loanData.SetCurrentField("1109", this.baseLoanAmount.ToString());
      loanData.SetCurrentField("1107", this.chkField3533.Checked ? "" : this.rateFunding.ToString("N6"));
      loanData.SetCurrentField("1045", this.fundingFee.ToString());
      loanData.SetCurrentField("1760", this.paidInCash.ToString());
      loanData.SetCurrentField("1765", this.lockPaidInCash.ToString());
      loanData.SetCurrentField("VASUMM.X49", this.chkFirstTimeValue);
      switch (this.priceType)
      {
        case 0:
          loanData.SetCurrentField("1757", "Loan Amount");
          break;
        case 1:
          loanData.SetCurrentField("1757", "Purchase Price");
          break;
        case 2:
          loanData.SetCurrentField("1757", "Appraisal Value");
          break;
        case 3:
          loanData.SetCurrentField("1757", "Base Loan Amount");
          break;
      }
      this.monthMI1 = Utils.ParseInt((object) this.monthMI1Txt.Text, 0);
      loanData.SetCurrentField("1198", this.monthMI1 != 0 ? this.monthMI1.ToString() : "");
      if (this.amountMI1 > 0.0)
      {
        loanData.SetCurrentField("1766", this.amountMI1.ToString("N2"));
        if (field == "FarmersHomeAdministration")
        {
          if (loanData.GetField("232") != string.Empty)
            loanData.Calculator.FormCalculation("COPYLINE1003TOLINE1010", (string) null, (string) null);
          loanData.SetCurrentField("NEWHUD.X1707", this.amountMI1.ToString("N2"));
          loanData.SetCurrentField("232", "");
        }
        else
        {
          if (!loanData.IsLocked("232"))
            loanData.SetCurrentField("232", this.amountMI1.ToString("N2"));
          loanData.SetCurrentField("NEWHUD.X1707", "");
        }
        loanData.SetCurrentField("3971", this.txtPrepaidAmount.Text.Trim());
      }
      else
      {
        loanData.SetCurrentField("1766", "");
        loanData.SetCurrentField("232", "");
        loanData.SetCurrentField("NEWHUD.X1707", "");
        loanData.SetCurrentField("3971", "");
      }
      this.rateMI1 = Utils.ParseDouble((object) this.rateMI1Txt.Text);
      loanData.SetCurrentField("1199", this.rateMI1 != 0.0 ? this.rateMI1.ToString("N6") : "");
      this.monthMI2 = Utils.ParseInt((object) this.monthMI2Txt.Text, 0);
      loanData.SetCurrentField("1200", this.monthMI2 != 0 ? this.monthMI2.ToString() : "");
      if (this.amountMI2 > 0.0)
        loanData.SetCurrentField("1770", this.amountMI2.ToString("N2"));
      else
        loanData.SetCurrentField("1770", "");
      this.rateMI2 = Utils.ParseDouble((object) this.rateMI2Txt.Text);
      loanData.SetCurrentField("1201", this.rateMI2 != 0.0 ? this.rateMI2.ToString("N6") : "");
      this.rateMICancel = Utils.ParseDouble((object) this.rateMICancelTxt.Text);
      loanData.SetCurrentField("1205", this.rateMICancel != 0.0 ? this.rateMICancel.ToString("N3") : "");
      loanData.SetCurrentField("1826", this.fundingAmt.ToString());
      if (!loanData.IsLocked("969"))
        loanData.SetCurrentField("969", this.fundingAmt.ToString());
      if (this.fundingAmt == 0.0)
        this.fundingAmt = Utils.ParseDouble((object) loanData.GetField("1766")) * Utils.ParseDouble((object) loanData.GetField("1209"));
      switch (field)
      {
        case "VA":
          this.fundingAmt -= Utils.ParseDouble((object) loanData.GetField("571"));
          loanData.SetCurrentField("1050", this.fundingAmt.ToString());
          loanData.SetCurrentField("337", "");
          if (string.Compare(loanData.GetField("NEWHUD.X1299"), "Guarantee Fee", true) == 0)
          {
            loanData.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
            break;
          }
          break;
        case "FarmersHomeAdministration":
          if (!flag)
          {
            this.fundingAmt -= Utils.ParseDouble((object) loanData.GetField("NEWHUD.X1302"));
            if (string.Compare(loanData.GetField("NEWHUD.X1299"), "Guarantee Fee", true) != 0 && !flag)
              loanData.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
            loanData.SetCurrentField("NEWHUD.X1299", "Guarantee Fee");
            loanData.SetCurrentField("NEWHUD.X1301", this.fundingAmt.ToString());
            loanData.SetCurrentField("337", "");
            loanData.Calculator.Calculate2015FeeDetails("NEWHUD.X1301");
            break;
          }
          goto default;
        default:
          this.fundingAmt -= Utils.ParseDouble((object) loanData.GetField("562"));
          loanData.SetCurrentField("337", this.fundingAmt != 0.0 ? this.fundingAmt.ToString("N2") : "");
          if (string.Compare(loanData.GetField("NEWHUD.X1299"), "Guarantee Fee", true) == 0)
          {
            loanData.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
            break;
          }
          break;
      }
      loanData.SetCurrentField("RE88395.X43", loanData.GetField("337"));
      loanData.SetCurrentField("1806", "Base Loan Amount");
      loanData.SetCurrentField("1807", "");
      loanData.SetCurrentField("1753", this.midpointChkbox.Checked ? "Y" : "");
      loanData.SetField("1775", this.balanceChkbox.Checked ? "Y" : "");
      loanData.SetCurrentField("1209", this.txtPrepaidMonth.Text.Trim());
      loanData.SetField("2978", this.chkPrepaid.Checked ? "Y" : "");
      loanData.SetField("3248", this.chkDeclining.Checked ? "Y" : "");
      loanData.SetField("3262", this.chkRefundUnearned.Checked ? "Y" : "");
      loanData.SetField("3531", this.chkField3531.Checked ? "Y" : "");
      loanData.SetField("3532", this.chkField3532.Checked ? "Y" : "");
      loanData.SetField("3533", this.chkField3533.Checked ? "Y" : "");
      loanData.SetField("3625", this.chkLockFactor.Checked ? "Y" : "");
      if (field == "FarmersHomeAdministration")
      {
        this.fundingFee += this.paidInCash;
        if (!flag)
        {
          if (this.fundingFee != 0.0)
          {
            loanData.SetCurrentField("NEWHUD.X1299", "Guarantee Fee");
            loanData.SetCurrentField("NEWHUD.X1301", this.fundingFee.ToString());
          }
          else
          {
            loanData.SetCurrentField("NEWHUD.X1299", "");
            loanData.SetCurrentField("NEWHUD.X1301", "");
          }
          loanData.Calculator.Calculate2015FeeDetails("NEWHUD.X1301");
        }
      }
      else if (string.Compare(loanData.GetField("NEWHUD.X1299"), "Guarantee Fee", true) == 0)
        loanData.Calculator.FormCalculation("CLEARLINE819", (string) null, (string) null);
      if (field == "FarmersHomeAdministration")
        loanData.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X1301", false);
      loanData.Calculator.CopyHUD2010ToGFE2010("337", false);
      if (field == "FarmersHomeAdministration")
        loanData.Calculator.CopyHUD2010ToGFE2010("NEWHUD.X1707", false);
      else
        loanData.Calculator.CopyHUD2010ToGFE2010("232", false);
    }

    private void setLPLoanData()
    {
      if (this.rateFunding == 0.0)
        this.lpLoan.SetCurrentField("1107", "");
      else
        this.lpLoan.SetCurrentField("1107", this.rateFunding.ToString());
      if (this.fundingAmt == 0.0)
        this.lpLoan.SetCurrentField("1826", "");
      else
        this.lpLoan.SetCurrentField("1826", this.fundingAmt.ToString());
      if (this.paidInCash == 0.0)
        this.lpLoan.SetCurrentField("1760", "");
      else
        this.lpLoan.SetCurrentField("1760", this.paidInCash.ToString());
      this.lpLoan.SetCurrentField("1765", this.lockPaidInCash);
      switch (this.priceType)
      {
        case 0:
          this.lpLoan.SetCurrentField("1757", "Loan Amount");
          break;
        case 1:
          this.lpLoan.SetCurrentField("1757", "Purchase Price");
          break;
        case 2:
          this.lpLoan.SetCurrentField("1757", "Appraisal Value");
          break;
        case 3:
          this.lpLoan.SetCurrentField("1757", "Base Loan Amount");
          break;
      }
      this.monthMI1 = Utils.ParseInt((object) this.monthMI1Txt.Text, 0);
      this.lpLoan.SetCurrentField("1198", (double) this.monthMI1 != 0.0 ? this.monthMI1.ToString() : "");
      this.rateMI1 = Utils.ParseDouble((object) this.rateMI1Txt.Text);
      this.lpLoan.SetCurrentField("1199", this.rateMI1 > 0.0 ? this.rateMI1.ToString() : "");
      this.monthMI2 = Utils.ParseInt((object) this.monthMI2Txt.Text, 0);
      this.lpLoan.SetCurrentField("1200", (double) this.monthMI2 != 0.0 ? this.monthMI2.ToString() : "");
      this.rateMI2 = Utils.ParseDouble((object) this.rateMI2Txt.Text);
      this.lpLoan.SetCurrentField("1201", this.rateMI2 > 0.0 ? this.rateMI2.ToString() : "");
      this.rateMICancel = Utils.ParseDouble((object) this.rateMICancelTxt.Text);
      this.lpLoan.SetCurrentField("1205", this.rateMICancel != 0.0 ? this.rateMICancel.ToString("N3") : "");
      this.lpLoan.SetCurrentField("1209", this.txtPrepaidMonth.Text.Trim());
      this.lpLoan.SetCurrentField("1753", this.midpointChkbox.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("1775", this.balanceChkbox.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3248", this.chkDeclining.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("SYS.X11", this.chkRounding.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("2978", this.chkPrepaid.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3262", this.chkRefundUnearned.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3531", this.chkField3531.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3532", this.chkField3532.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3533", this.chkField3533.Checked ? "Y" : "");
      this.lpLoan.SetCurrentField("3625", this.chkLockFactor.Checked ? "Y" : "");
      if (this.forLP)
        return;
      if (this.lpLoan.GetField("NEWHUD.X1706") == string.Empty)
      {
        this.lpLoan.SetCurrentField("NEWHUD.X1708", "");
        this.lpLoan.SetCurrentField("NEWHUD.X1709", "");
      }
      if (!(this.lpLoan.GetField("1107") == string.Empty) && !(this.lpLoan.GetField("1107") == "0.000000"))
        return;
      this.lpLoan.SetCurrentField("NEWHUD.X1299", "");
      this.lpLoan.SetCurrentField("NEWHUD.X1301", "");
    }

    private void keyup(object sender, KeyEventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      FieldFormat dataFormat = FieldFormat.DECIMAL_3;
      if (textBox.Name == "baseLoanAmtTxt" || textBox.Name == "fundingAmtTxt" || textBox.Name == "cashTxt")
        dataFormat = FieldFormat.DECIMAL_2;
      else if (textBox.Name == "rateMI1Txt" || textBox.Name == "rateMI2Txt" || textBox.Name == "rateFundingTxt")
        dataFormat = FieldFormat.DECIMAL_6;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void setSelectedField(object o)
    {
      if (this.loan == null || o == null || !this.loan.IsInFindFieldForm)
        return;
      string id = string.Empty;
      switch (o)
      {
        case TextBox _:
          id = (string) ((Control) o).Tag;
          break;
        case ComboBox _:
          id = (string) ((Control) o).Tag;
          break;
        case CheckBox _:
          id = (string) ((Control) o).Tag;
          break;
      }
      if (id == string.Empty)
        return;
      LoanData.FindFieldTypes findFieldTypes = this.loan.SelectedFieldType(id);
      Color color = Color.White;
      switch (findFieldTypes)
      {
        case LoanData.FindFieldTypes.NewSelect:
          color = Color.LightYellow;
          break;
        case LoanData.FindFieldTypes.Existing:
          color = Color.LightCyan;
          break;
      }
      switch (o)
      {
        case TextBox _:
          ((Control) o).BackColor = color;
          break;
        case ComboBox _:
          ((Control) o).BackColor = color;
          break;
        case CheckBox _:
          ((Control) o).BackColor = color;
          break;
      }
    }

    private void field_MouseDown(object sender, MouseEventArgs e)
    {
      if (this.loan == null || !this.loan.IsInFindFieldForm || e.Button != MouseButtons.Right)
        return;
      string id = string.Empty;
      try
      {
        switch (sender)
        {
          case TextBox _:
            id = (string) ((Control) sender).Tag;
            break;
          case ComboBox _:
            id = (string) ((Control) sender).Tag;
            break;
          case CheckBox _:
            id = (string) ((Control) sender).Tag;
            break;
        }
      }
      catch (Exception ex)
      {
        return;
      }
      if ((id ?? "") == string.Empty)
        return;
      Color color = Color.White;
      switch (this.loan.SelectedFieldType(id))
      {
        case LoanData.FindFieldTypes.None:
          this.loan.AddSelectedField(id);
          color = Color.LightYellow;
          break;
        case LoanData.FindFieldTypes.NewSelect:
          this.loan.RemoveSelectedField(id);
          color = Color.White;
          break;
        case LoanData.FindFieldTypes.Existing:
          int num = (int) Utils.Dialog((IWin32Window) null, "You can't remove existing selected field in current list. Please use 'Remove' button to remove existing fields.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return;
      }
      switch (sender)
      {
        case TextBox _:
          ((Control) sender).BackColor = color;
          break;
        case ComboBox _:
          ((Control) sender).BackColor = color;
          break;
        case CheckBox _:
          ((Control) sender).BackColor = color;
          break;
      }
    }

    private void btnGetMI_Click(object sender, EventArgs e)
    {
      Cursor.Current = Cursors.WaitCursor;
      string field = this.loan.GetField("1172");
      LoanTypeEnum loanTypeEnum = LoanTypeEnum.Other;
      switch (field)
      {
        case "Conventional":
          loanTypeEnum = LoanTypeEnum.Conventional;
          break;
        case "FHA":
          loanTypeEnum = LoanTypeEnum.FHA;
          break;
        case "VA":
          loanTypeEnum = LoanTypeEnum.VA;
          break;
      }
      MIRecord miRecord = (MIRecord) null;
      MIRecord[] miRecords = this.session.ConfigurationManager.GetMIRecords(loanTypeEnum, "");
      if (miRecords == null || miRecords.Length == 0)
      {
        Cursor.Current = Cursors.Default;
        int num = (int) Utils.Dialog((IWin32Window) this, "The current loan information does not meet any criteria in MI tables.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        ILoanConfigurationInfo configurationInfo = this.session.SessionObjects.LoanManager.GetLoanConfigurationInfo();
        LoanData loanData = new LoanData(this.loan, this.loan.Settings, false);
        loanData.AttachCalculator((ILoanCalculator) new LoanCalculator(this.session.SessionObjects, configurationInfo, loanData));
        this.setLoanData(loanData);
        bool isStreamLine = loanData.GetField("MORNET.X40") == "StreamlineWithAppraisal" || loanData.GetField("MORNET.X40") == "StreamlineWithoutAppraisal";
        ArrayList arrayList = new ArrayList();
        for (int index = 0; index < miRecords.Length; ++index)
        {
          if (miRecords[index].Scenarios != null && miRecords[index].Scenarios.Length != 0 && this.recordMatched(miRecords[index].Scenarios, field, isStreamLine, loanData))
            arrayList.Add((object) miRecords[index]);
        }
        if (arrayList.Count == 0)
        {
          Cursor.Current = Cursors.Default;
          int num = (int) Utils.Dialog((IWin32Window) this, "The current loan information does not meet any criteria in MI tables.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          MIRecord[] array = (MIRecord[]) arrayList.ToArray(typeof (MIRecord));
          if (array.Length == 1)
          {
            miRecord = array[0];
          }
          else
          {
            Cursor.Current = Cursors.Default;
            using (MIRecordsSelectForm recordsSelectForm = new MIRecordsSelectForm(loanTypeEnum, array, this.session))
            {
              if (recordsSelectForm.ShowDialog((IWin32Window) this) != DialogResult.OK)
                return;
              miRecord = recordsSelectForm.SelectedMI;
            }
          }
          this.chkLockFactor.Checked = false;
          if (loanTypeEnum == LoanTypeEnum.FHA || loanTypeEnum == LoanTypeEnum.Conventional || loanTypeEnum == LoanTypeEnum.VA && this.chkFirstTime.Checked)
            this.rateFundingTxt.Text = miRecord.Premium1st == 0.0 ? "" : miRecord.Premium1st.ToString("N6");
          else if (loanTypeEnum == LoanTypeEnum.VA)
            this.rateFundingTxt.Text = miRecord.Premium1st == 0.0 ? "" : miRecord.SubsequentPremium.ToString("N6");
          this.rateMI1Txt.Text = miRecord.Monthly1st == 0.0 ? "" : miRecord.Monthly1st.ToString("N3");
          this.rateMI2Txt.Text = miRecord.Monthly2st == 0.0 ? "" : miRecord.Monthly2st.ToString("N3");
          int num = Utils.ParseInt((object) loanData.GetField("4"));
          if (miRecord.Months1st > 0)
          {
            if (miRecord.Months1st > num && num > 0)
              this.monthMI1Txt.Text = num.ToString();
            else if (miRecord.Months1st >= 999 && num <= 0)
              this.monthMI1Txt.Text = "0";
            else
              this.monthMI1Txt.Text = miRecord.Months1st.ToString();
          }
          else
            this.monthMI1Txt.Text = "";
          if (miRecord.Months2st > 0)
          {
            if (miRecord.Months2st > num && num > 0)
              this.monthMI2Txt.Text = num.ToString();
            else if (miRecord.Months2st >= 999 && num <= 0)
              this.monthMI2Txt.Text = "0";
            else
              this.monthMI2Txt.Text = miRecord.Months2st.ToString();
          }
          else
            this.monthMI2Txt.Text = "";
          this.rateMICancelTxt.Text = miRecord.Cutoff == 0.0 ? "" : miRecord.Cutoff.ToString("N3");
          this.lockChkbox.Checked = false;
          this.lockChkbox_Click((object) null, (EventArgs) null);
          if (this.inTemplate)
            this.CalculateLPMIP();
          else
            this.CalculateMIP(loanData);
          Cursor.Current = Cursors.Default;
        }
      }
    }

    private bool recordMatched(
      FieldFilter[] filters,
      string loanType,
      bool isStreamLine,
      LoanData loanData)
    {
      string str1 = "";
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      bool flag = loanData.GetField("3000") == "Y";
      for (int index = 0; index < filters.Length; ++index)
      {
        string id = filters[index].FieldID;
        if (((loanType == "FHA" ? 1 : (loanType == "VA" ? 1 : 0)) & (flag ? 1 : 0)) != 0 && id == "353")
          id = "MAX23K.X104";
        string simpleField = loanData.GetSimpleField(id);
        if ((loanType == "FHA" && id == "3042" && filters[index].OperatorType != OperatorTypes.EmptyDate || loanType == "FHA" & isStreamLine && id == "3432") && (simpleField == string.Empty || simpleField == "//"))
          simpleField = DateTime.Today.ToString("MM/dd/yyyy");
        string scriptCommands = filters[index].GetScriptCommands(simpleField);
        str1 = !(str1 == "") ? str1 + " " + scriptCommands : scriptCommands;
      }
      string str2 = str1.Trim().ToLower();
      if (str2 == "")
        return true;
      if (str2.EndsWith("and"))
        str2 = str2.Substring(0, str2.Length - 3);
      if (str2.EndsWith("or"))
        str2 = str2.Substring(0, str2.Length - 2);
      return Utils.CheckFilter(str2.Trim()) == "true";
    }

    private void MIPDialog_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (e.KeyChar != '\u001B')
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void insuranceFields_Changed(object sender, EventArgs e)
    {
      CheckBox checkBox = (CheckBox) sender;
      this.chkField3531.CheckedChanged -= new EventHandler(this.insuranceFields_Changed);
      this.chkField3532.CheckedChanged -= new EventHandler(this.insuranceFields_Changed);
      this.chkField3533.CheckedChanged -= new EventHandler(this.insuranceFields_Changed);
      if (checkBox.Name == "chkField3533" && this.chkField3533.Checked)
      {
        this.chkField3531.Checked = this.chkField3532.Checked = false;
        this.rateFundingTxt.Text = this.cashTxt.Text = this.rateMI1Txt.Text = this.monthMI1Txt.Text = this.rateMI2Txt.Text = this.monthMI2Txt.Text = this.rateMICancelTxt.Text = this.fundingAmtTxt.Text = "";
        this.lockChkbox.Checked = this.chkLockFactor.Checked = false;
      }
      else if ((checkBox.Name == "chkField3531" || checkBox.Name == "chkField3532") && (this.chkField3531.Checked || this.chkField3532.Checked))
        this.chkField3533.Checked = false;
      this.changeMIFieldStatus();
      if (this.inTemplate)
        this.CalculateLPMIP();
      else
        this.CalculateMIP(this.loan);
      this.chkField3531.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
      this.chkField3532.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
      this.chkField3533.CheckedChanged += new EventHandler(this.insuranceFields_Changed);
    }

    private void changeMIFieldStatus()
    {
      this.rateFundingTxt.ReadOnly = this.lockChkbox.Checked || this.chkField3533.Checked;
      this.rateFundingTxt.Enabled = this.rateFundingTxt.TabStop = !this.rateFundingTxt.ReadOnly;
      this.rateFundingTxt.BackColor = this.rateFundingTxt.ReadOnly ? SystemColors.Control : SystemColors.Window;
      this.fundingAmtTxt.ReadOnly = !this.lockChkbox.Checked || this.chkField3533.Checked;
      this.fundingAmtTxt.Enabled = this.fundingAmtTxt.TabStop = !this.fundingAmtTxt.ReadOnly;
      this.fundingAmtTxt.BackColor = this.fundingAmtTxt.ReadOnly ? SystemColors.Control : SystemColors.Window;
      this.cashTxt.ReadOnly = !this.lockChkbox.Checked || this.chkField3533.Checked || this.chkRounding.Checked;
      this.cashTxt.Enabled = this.cashTxt.TabStop = !this.cashTxt.ReadOnly;
      this.cashTxt.BackColor = this.cashTxt.ReadOnly ? SystemColors.Control : SystemColors.Window;
      if (this.chkRounding.Checked)
        this.cashTxt.Text = "";
      if (this.chkField3533.Checked)
      {
        this.btnGetMI.Enabled = this.lockChkbox.Enabled = false;
        this.rateMI1Txt.ReadOnly = this.monthMI1Txt.ReadOnly = this.rateMI2Txt.ReadOnly = this.monthMI2Txt.ReadOnly = this.rateMICancelTxt.ReadOnly = true;
      }
      else
      {
        this.btnGetMI.Enabled = this.lockChkbox.Enabled = true;
        this.rateMI1Txt.ReadOnly = this.monthMI1Txt.ReadOnly = this.rateMI2Txt.ReadOnly = this.monthMI2Txt.ReadOnly = this.rateMICancelTxt.ReadOnly = false;
      }
      this.chkLockFactor.Enabled = this.lockChkbox.Checked;
      this.setBusinessRule();
    }

    internal void SetScreenDisabled()
    {
      this.setFieldToDisabled(this.pnlBody.Controls);
      this.cancelBtn.Text = "&Close";
      this.okBtn.Visible = false;
    }

    private void setFieldToDisabled(Control.ControlCollection cs)
    {
      foreach (Control c in (ArrangedElementCollection) cs)
      {
        switch (c)
        {
          case TextBox _:
          case ComboBox _:
          case CheckBox _:
          case Button _:
            c.Enabled = false;
            continue;
          default:
            this.setFieldToDisabled(c.Controls);
            continue;
        }
      }
    }

    private void chkLockFactor_CheckedChanged(object sender, EventArgs e)
    {
      this.changeMIFieldStatus();
      if (this.inTemplate)
        this.CalculateLPMIP();
      else
        this.CalculateMIP(this.loan);
      this.setBusinessRule();
    }
  }
}
