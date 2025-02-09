// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.OpportunityForm
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class OpportunityForm : Form, IBindingForm
  {
    private System.ComponentModel.Container components;
    private Panel pnlMiddle;
    private Label lblLoanPurpose;
    private Label lblLoanAmount;
    private Label lblDownPayment;
    private TextBox txtBoxMortgageBalance;
    private Label lblMortgageBalance;
    private TextBox txtBoxLoanAmount;
    private TextBox txtBoxLoanTerm;
    private Label lblLoanTerm;
    private Label lblAmortization;
    private TextBox txtBoxDownPayment;
    private ComboBox cmbBoxLoanPurpose;
    private TextBox txtBoxAddress;
    private Label lblMortgageRate;
    private Label lblPaymentHousing;
    private Label lblPaymentNonhousing;
    private TextBox txtBoxPurposeOther;
    private Label lblPurposeOther;
    private TextBox txtBoxMortgageRate;
    private TextBox txtBoxPaymentHousing;
    private TextBox txtBoxPaymentNonhousing;
    private Label lblCreditRating;
    private TextBox txtBoxCreditRating;
    private Label lblPropertyType;
    private TextBox txtBoxState;
    private Label lblZip;
    private Label lblState;
    private TextBox txtBoxZip;
    private Label lblAddress;
    private Label lblCity;
    private TextBox txtBoxCity;
    private ComboBox cmbBoxPropertyType;
    private ComboBox cmbBoxPropertyUse;
    private Label lblPropertyUse;
    private ComboBox cmbBoxAmortization;
    private Label lblPurchaseDate;
    private Label lblPropertyValue;
    private TextBox txtBoxPropertyValue;
    private CheckBox chkBoxBankruptcy;
    private ComboBox cmbBoxEmployment;
    private Label lblEmployment;
    private bool changed;
    private bool intermidiateData;
    private bool deleteBackKey;
    private bool initialLoad = true;
    public bool IsReadOnly;
    private Opportunity opportunity;
    private TextBox txtCashOut;
    private Label label7;
    private GroupContainer gcLoanInfo;
    private Label label9;
    private GroupContainer gcProperty;
    private GroupContainer groupContainer1;
    private Label label1;
    private Label lblIncome;
    private TextBox txtBoxIncome;
    private int currentContactID = -1;
    private Label label2;
    private DatePicker dpPurchaseDate;
    private TextBox txtCreditScore;
    private Label lblCreditScore;
    private Label lblLoanType;
    private ComboBox cmbLoanType;
    private TextBox txtTypeOther;
    private Label lblTypeOther;
    private BorrowerInfo borrowerObj;

    public event BorrowerSummaryChangedEventHandler SummaryChanged;

    public OpportunityForm()
    {
      this.InitializeComponent();
      this.cmbBoxAmortization.Items.AddRange(AmortizationTypeEnumUtil.GetDisplayNames());
      this.cmbBoxEmployment.Items.AddRange(EmploymentStatusEnumUtil.GetDisplayNames());
      this.cmbBoxLoanPurpose.Items.AddRange(LoanPurposeEnumUtil.GetDisplayNames());
      this.cmbBoxPropertyType.Items.AddRange(PropertyTypeEnumUtil.GetDisplayNames());
      this.cmbBoxPropertyUse.Items.AddRange(PropertyUseEnumUtil.GetDisplayNames());
      this.cmbLoanType.Items.AddRange(LoanTypeEnumUtil.GetDisplayNames());
      this.CurrentContactID = -1;
      this.txtBoxState.CharacterCasing = CharacterCasing.Upper;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtBoxMortgageBalance = new TextBox();
      this.lblMortgageBalance = new Label();
      this.lblCreditRating = new Label();
      this.txtBoxCreditRating = new TextBox();
      this.lblLoanPurpose = new Label();
      this.lblLoanAmount = new Label();
      this.txtBoxLoanAmount = new TextBox();
      this.lblPropertyValue = new Label();
      this.txtBoxPropertyValue = new TextBox();
      this.lblPurchaseDate = new Label();
      this.cmbBoxPropertyUse = new ComboBox();
      this.lblPropertyUse = new Label();
      this.cmbBoxPropertyType = new ComboBox();
      this.lblCity = new Label();
      this.txtBoxCity = new TextBox();
      this.txtBoxAddress = new TextBox();
      this.lblAddress = new Label();
      this.lblPropertyType = new Label();
      this.txtBoxState = new TextBox();
      this.lblZip = new Label();
      this.lblState = new Label();
      this.txtBoxZip = new TextBox();
      this.txtBoxLoanTerm = new TextBox();
      this.lblDownPayment = new Label();
      this.lblLoanTerm = new Label();
      this.lblAmortization = new Label();
      this.txtBoxDownPayment = new TextBox();
      this.chkBoxBankruptcy = new CheckBox();
      this.cmbBoxEmployment = new ComboBox();
      this.lblEmployment = new Label();
      this.pnlMiddle = new Panel();
      this.groupContainer1 = new GroupContainer();
      this.label2 = new Label();
      this.lblIncome = new Label();
      this.txtBoxIncome = new TextBox();
      this.label1 = new Label();
      this.txtBoxPaymentNonhousing = new TextBox();
      this.txtBoxPaymentHousing = new TextBox();
      this.txtBoxMortgageRate = new TextBox();
      this.lblMortgageRate = new Label();
      this.lblPaymentHousing = new Label();
      this.lblPaymentNonhousing = new Label();
      this.txtCashOut = new TextBox();
      this.label7 = new Label();
      this.lblPurposeOther = new Label();
      this.cmbBoxAmortization = new ComboBox();
      this.txtBoxPurposeOther = new TextBox();
      this.cmbBoxLoanPurpose = new ComboBox();
      this.gcLoanInfo = new GroupContainer();
      this.txtTypeOther = new TextBox();
      this.lblTypeOther = new Label();
      this.txtCreditScore = new TextBox();
      this.lblCreditScore = new Label();
      this.lblLoanType = new Label();
      this.cmbLoanType = new ComboBox();
      this.label9 = new Label();
      this.gcProperty = new GroupContainer();
      this.dpPurchaseDate = new DatePicker();
      this.pnlMiddle.SuspendLayout();
      this.groupContainer1.SuspendLayout();
      this.gcLoanInfo.SuspendLayout();
      this.gcProperty.SuspendLayout();
      this.SuspendLayout();
      this.txtBoxMortgageBalance.Location = new Point(180, 76);
      this.txtBoxMortgageBalance.MaxLength = 18;
      this.txtBoxMortgageBalance.Name = "txtBoxMortgageBalance";
      this.txtBoxMortgageBalance.Size = new Size(125, 20);
      this.txtBoxMortgageBalance.TabIndex = 13;
      this.txtBoxMortgageBalance.TextChanged += new EventHandler(this.controlChanged);
      this.lblMortgageBalance.Location = new Point(10, 79);
      this.lblMortgageBalance.Name = "lblMortgageBalance";
      this.lblMortgageBalance.Size = new Size(140, 14);
      this.lblMortgageBalance.TabIndex = 12;
      this.lblMortgageBalance.Text = "Current Mortgage Balance";
      this.lblCreditRating.Location = new Point(10, 39);
      this.lblCreditRating.Name = "lblCreditRating";
      this.lblCreditRating.Size = new Size(72, 15);
      this.lblCreditRating.TabIndex = 1;
      this.lblCreditRating.Text = "Credit Rating";
      this.txtBoxCreditRating.Location = new Point(180, 36);
      this.txtBoxCreditRating.MaxLength = 30;
      this.txtBoxCreditRating.Name = "txtBoxCreditRating";
      this.txtBoxCreditRating.Size = new Size(125, 20);
      this.txtBoxCreditRating.TabIndex = 2;
      this.txtBoxCreditRating.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxCreditRating.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.lblLoanPurpose.Location = new Point(10, 60);
      this.lblLoanPurpose.Name = "lblLoanPurpose";
      this.lblLoanPurpose.Size = new Size(76, 15);
      this.lblLoanPurpose.TabIndex = 2;
      this.lblLoanPurpose.Text = "Loan Purpose";
      this.lblLoanAmount.Location = new Point(10, 38);
      this.lblLoanAmount.Name = "lblLoanAmount";
      this.lblLoanAmount.Size = new Size(76, 15);
      this.lblLoanAmount.TabIndex = 0;
      this.lblLoanAmount.Text = "Loan Amount";
      this.txtBoxLoanAmount.Location = new Point((int) sbyte.MaxValue, 36);
      this.txtBoxLoanAmount.MaxLength = 18;
      this.txtBoxLoanAmount.Name = "txtBoxLoanAmount";
      this.txtBoxLoanAmount.Size = new Size(195, 20);
      this.txtBoxLoanAmount.TabIndex = 1;
      this.txtBoxLoanAmount.TextChanged += new EventHandler(this.controlChanged);
      this.lblPropertyValue.Location = new Point(10, 153);
      this.lblPropertyValue.Name = "lblPropertyValue";
      this.lblPropertyValue.RightToLeft = RightToLeft.No;
      this.lblPropertyValue.Size = new Size(88, 16);
      this.lblPropertyValue.TabIndex = 12;
      this.lblPropertyValue.Text = "Property Value";
      this.txtBoxPropertyValue.Location = new Point(113, 150);
      this.txtBoxPropertyValue.MaxLength = 18;
      this.txtBoxPropertyValue.Name = "txtBoxPropertyValue";
      this.txtBoxPropertyValue.Size = new Size(209, 20);
      this.txtBoxPropertyValue.TabIndex = 13;
      this.txtBoxPropertyValue.TextChanged += new EventHandler(this.controlChanged);
      this.lblPurchaseDate.Location = new Point(10, 175);
      this.lblPurchaseDate.Name = "lblPurchaseDate";
      this.lblPurchaseDate.Size = new Size(88, 17);
      this.lblPurchaseDate.TabIndex = 14;
      this.lblPurchaseDate.Text = "Purchase Date";
      this.cmbBoxPropertyUse.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPropertyUse.Location = new Point(113, 102);
      this.cmbBoxPropertyUse.Name = "cmbBoxPropertyUse";
      this.cmbBoxPropertyUse.Size = new Size(209, 22);
      this.cmbBoxPropertyUse.TabIndex = 9;
      this.cmbBoxPropertyUse.SelectedIndexChanged += new EventHandler(this.controlChanged);
      this.lblPropertyUse.Location = new Point(10, 105);
      this.lblPropertyUse.Name = "lblPropertyUse";
      this.lblPropertyUse.Size = new Size(93, 19);
      this.lblPropertyUse.TabIndex = 8;
      this.lblPropertyUse.Text = "Occupancy Type";
      this.cmbBoxPropertyType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxPropertyType.Location = new Point(113, 126);
      this.cmbBoxPropertyType.Name = "cmbBoxPropertyType";
      this.cmbBoxPropertyType.Size = new Size(209, 22);
      this.cmbBoxPropertyType.TabIndex = 11;
      this.cmbBoxPropertyType.SelectedIndexChanged += new EventHandler(this.controlChanged);
      this.lblCity.Location = new Point(10, 61);
      this.lblCity.Name = "lblCity";
      this.lblCity.RightToLeft = RightToLeft.No;
      this.lblCity.Size = new Size(33, 16);
      this.lblCity.TabIndex = 2;
      this.lblCity.Text = "City";
      this.txtBoxCity.Location = new Point(113, 58);
      this.txtBoxCity.MaxLength = 50;
      this.txtBoxCity.Name = "txtBoxCity";
      this.txtBoxCity.Size = new Size(209, 20);
      this.txtBoxCity.TabIndex = 3;
      this.txtBoxCity.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxAddress.Location = new Point(113, 36);
      this.txtBoxAddress.MaxLength = (int) byte.MaxValue;
      this.txtBoxAddress.Name = "txtBoxAddress";
      this.txtBoxAddress.Size = new Size(209, 20);
      this.txtBoxAddress.TabIndex = 1;
      this.txtBoxAddress.TextChanged += new EventHandler(this.controlChanged);
      this.lblAddress.Location = new Point(10, 39);
      this.lblAddress.Name = "lblAddress";
      this.lblAddress.Size = new Size(57, 15);
      this.lblAddress.TabIndex = 0;
      this.lblAddress.Text = "Address";
      this.lblPropertyType.Location = new Point(10, 129);
      this.lblPropertyType.Name = "lblPropertyType";
      this.lblPropertyType.Size = new Size(76, 19);
      this.lblPropertyType.TabIndex = 10;
      this.lblPropertyType.Text = "Property Type";
      this.txtBoxState.Location = new Point(113, 80);
      this.txtBoxState.MaxLength = 2;
      this.txtBoxState.Name = "txtBoxState";
      this.txtBoxState.Size = new Size(28, 20);
      this.txtBoxState.TabIndex = 5;
      this.txtBoxState.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxState.Leave += new EventHandler(this.txtBoxState_Leave);
      this.lblZip.Location = new Point(145, 83);
      this.lblZip.Name = "lblZip";
      this.lblZip.RightToLeft = RightToLeft.No;
      this.lblZip.Size = new Size(23, 16);
      this.lblZip.TabIndex = 6;
      this.lblZip.Text = "Zip";
      this.lblState.Location = new Point(10, 83);
      this.lblState.Name = "lblState";
      this.lblState.RightToLeft = RightToLeft.No;
      this.lblState.Size = new Size(33, 15);
      this.lblState.TabIndex = 4;
      this.lblState.Text = "State";
      this.txtBoxZip.Location = new Point(174, 80);
      this.txtBoxZip.MaxLength = 20;
      this.txtBoxZip.Name = "txtBoxZip";
      this.txtBoxZip.Size = new Size(76, 20);
      this.txtBoxZip.TabIndex = 7;
      this.txtBoxZip.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxZip.KeyDown += new KeyEventHandler(this.txtBoxKeyDown);
      this.txtBoxZip.Leave += new EventHandler(this.txtBoxZip_Leave);
      this.txtBoxLoanTerm.Location = new Point((int) sbyte.MaxValue, 103);
      this.txtBoxLoanTerm.MaxLength = 18;
      this.txtBoxLoanTerm.Name = "txtBoxLoanTerm";
      this.txtBoxLoanTerm.Size = new Size(31, 20);
      this.txtBoxLoanTerm.TabIndex = 7;
      this.txtBoxLoanTerm.TextChanged += new EventHandler(this.controlChanged);
      this.lblDownPayment.Location = new Point(10, 223);
      this.lblDownPayment.Name = "lblDownPayment";
      this.lblDownPayment.RightToLeft = RightToLeft.No;
      this.lblDownPayment.Size = new Size(94, 20);
      this.lblDownPayment.TabIndex = 10;
      this.lblDownPayment.Text = "Down Payment";
      this.lblLoanTerm.Location = new Point(10, 108);
      this.lblLoanTerm.Name = "lblLoanTerm";
      this.lblLoanTerm.Size = new Size(67, 14);
      this.lblLoanTerm.TabIndex = 6;
      this.lblLoanTerm.Text = "Loan Term";
      this.lblAmortization.Location = new Point(10, 200);
      this.lblAmortization.Name = "lblAmortization";
      this.lblAmortization.Size = new Size(76, 15);
      this.lblAmortization.TabIndex = 8;
      this.lblAmortization.Text = "Amortization";
      this.txtBoxDownPayment.Location = new Point((int) sbyte.MaxValue, 221);
      this.txtBoxDownPayment.MaxLength = 18;
      this.txtBoxDownPayment.Name = "txtBoxDownPayment";
      this.txtBoxDownPayment.Size = new Size(195, 20);
      this.txtBoxDownPayment.TabIndex = 12;
      this.txtBoxDownPayment.TextChanged += new EventHandler(this.controlChanged);
      this.chkBoxBankruptcy.Location = new Point(180, 58);
      this.chkBoxBankruptcy.Name = "chkBoxBankruptcy";
      this.chkBoxBankruptcy.Size = new Size(19, 16);
      this.chkBoxBankruptcy.TabIndex = 3;
      this.chkBoxBankruptcy.CheckedChanged += new EventHandler(this.controlChanged);
      this.cmbBoxEmployment.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxEmployment.Location = new Point(180, 164);
      this.cmbBoxEmployment.Name = "cmbBoxEmployment";
      this.cmbBoxEmployment.Size = new Size(125, 22);
      this.cmbBoxEmployment.TabIndex = 20;
      this.cmbBoxEmployment.SelectedIndexChanged += new EventHandler(this.controlChanged);
      this.lblEmployment.Location = new Point(10, 167);
      this.lblEmployment.Name = "lblEmployment";
      this.lblEmployment.Size = new Size(68, 14);
      this.lblEmployment.TabIndex = 4;
      this.lblEmployment.Text = "Employment";
      this.pnlMiddle.BackColor = SystemColors.Window;
      this.pnlMiddle.Controls.Add((Control) this.groupContainer1);
      this.pnlMiddle.Dock = DockStyle.Fill;
      this.pnlMiddle.Location = new Point(335, 1);
      this.pnlMiddle.Name = "pnlMiddle";
      this.pnlMiddle.Padding = new Padding(4, 0, 4, 0);
      this.pnlMiddle.Size = new Size(340, 276);
      this.pnlMiddle.TabIndex = 0;
      this.groupContainer1.Controls.Add((Control) this.label2);
      this.groupContainer1.Controls.Add((Control) this.lblIncome);
      this.groupContainer1.Controls.Add((Control) this.txtBoxIncome);
      this.groupContainer1.Controls.Add((Control) this.label1);
      this.groupContainer1.Controls.Add((Control) this.lblCreditRating);
      this.groupContainer1.Controls.Add((Control) this.txtBoxPaymentNonhousing);
      this.groupContainer1.Controls.Add((Control) this.cmbBoxEmployment);
      this.groupContainer1.Controls.Add((Control) this.txtBoxPaymentHousing);
      this.groupContainer1.Controls.Add((Control) this.txtBoxCreditRating);
      this.groupContainer1.Controls.Add((Control) this.txtBoxMortgageRate);
      this.groupContainer1.Controls.Add((Control) this.lblEmployment);
      this.groupContainer1.Controls.Add((Control) this.lblMortgageRate);
      this.groupContainer1.Controls.Add((Control) this.lblPaymentHousing);
      this.groupContainer1.Controls.Add((Control) this.txtBoxMortgageBalance);
      this.groupContainer1.Controls.Add((Control) this.lblPaymentNonhousing);
      this.groupContainer1.Controls.Add((Control) this.chkBoxBankruptcy);
      this.groupContainer1.Controls.Add((Control) this.lblMortgageBalance);
      this.groupContainer1.Dock = DockStyle.Fill;
      this.groupContainer1.HeaderForeColor = SystemColors.ControlText;
      this.groupContainer1.Location = new Point(4, 0);
      this.groupContainer1.Name = "groupContainer1";
      this.groupContainer1.Size = new Size(332, 276);
      this.groupContainer1.TabIndex = 20;
      this.groupContainer1.Text = "Financial Information";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Font = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(305, 102);
      this.label2.Name = "label2";
      this.label2.Size = new Size(17, 14);
      this.label2.TabIndex = 24;
      this.label2.Text = "%";
      this.lblIncome.Location = new Point(10, 191);
      this.lblIncome.Name = "lblIncome";
      this.lblIncome.Size = new Size(88, 14);
      this.lblIncome.TabIndex = 21;
      this.lblIncome.Text = "Annual Income";
      this.txtBoxIncome.Location = new Point(180, 188);
      this.txtBoxIncome.MaxLength = 18;
      this.txtBoxIncome.Name = "txtBoxIncome";
      this.txtBoxIncome.Size = new Size(125, 20);
      this.txtBoxIncome.TabIndex = 23;
      this.txtBoxIncome.TextChanged += new EventHandler(this.controlChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 60);
      this.label1.Name = "label1";
      this.label1.Size = new Size(62, 14);
      this.label1.TabIndex = 20;
      this.label1.Text = "Bankruptcy";
      this.txtBoxPaymentNonhousing.Location = new Point(180, 142);
      this.txtBoxPaymentNonhousing.MaxLength = 18;
      this.txtBoxPaymentNonhousing.Name = "txtBoxPaymentNonhousing";
      this.txtBoxPaymentNonhousing.Size = new Size(125, 20);
      this.txtBoxPaymentNonhousing.TabIndex = 19;
      this.txtBoxPaymentNonhousing.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxPaymentHousing.Location = new Point(180, 120);
      this.txtBoxPaymentHousing.MaxLength = 18;
      this.txtBoxPaymentHousing.Name = "txtBoxPaymentHousing";
      this.txtBoxPaymentHousing.Size = new Size(125, 20);
      this.txtBoxPaymentHousing.TabIndex = 17;
      this.txtBoxPaymentHousing.TextChanged += new EventHandler(this.controlChanged);
      this.txtBoxMortgageRate.Location = new Point(180, 98);
      this.txtBoxMortgageRate.MaxLength = 8;
      this.txtBoxMortgageRate.Name = "txtBoxMortgageRate";
      this.txtBoxMortgageRate.Size = new Size(125, 20);
      this.txtBoxMortgageRate.TabIndex = 15;
      this.txtBoxMortgageRate.TextChanged += new EventHandler(this.controlChanged);
      this.lblMortgageRate.Location = new Point(10, 101);
      this.lblMortgageRate.Name = "lblMortgageRate";
      this.lblMortgageRate.Size = new Size(119, 19);
      this.lblMortgageRate.TabIndex = 14;
      this.lblMortgageRate.Text = "Current Mortgage Rate";
      this.lblPaymentHousing.Location = new Point(10, 123);
      this.lblPaymentHousing.Name = "lblPaymentHousing";
      this.lblPaymentHousing.Size = new Size(168, 15);
      this.lblPaymentHousing.TabIndex = 16;
      this.lblPaymentHousing.Text = "Monthly Payment (housing)";
      this.lblPaymentNonhousing.Location = new Point(10, 145);
      this.lblPaymentNonhousing.Name = "lblPaymentNonhousing";
      this.lblPaymentNonhousing.Size = new Size(160, 15);
      this.lblPaymentNonhousing.TabIndex = 18;
      this.lblPaymentNonhousing.Text = "Monthly Payment (non-housing)";
      this.txtCashOut.Location = new Point((int) sbyte.MaxValue, 243);
      this.txtCashOut.MaxLength = 18;
      this.txtCashOut.Name = "txtCashOut";
      this.txtCashOut.Size = new Size(195, 20);
      this.txtCashOut.TabIndex = 13;
      this.txtCashOut.TextChanged += new EventHandler(this.controlChanged);
      this.label7.AutoSize = true;
      this.label7.Location = new Point(10, 246);
      this.label7.Name = "label7";
      this.label7.Size = new Size(52, 14);
      this.label7.TabIndex = 25;
      this.label7.Text = "Cash Out";
      this.lblPurposeOther.Location = new Point(124, 85);
      this.lblPurposeOther.Name = "lblPurposeOther";
      this.lblPurposeOther.Size = new Size(34, 16);
      this.lblPurposeOther.TabIndex = 4;
      this.lblPurposeOther.Text = "other";
      this.cmbBoxAmortization.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxAmortization.Location = new Point((int) sbyte.MaxValue, 197);
      this.cmbBoxAmortization.Name = "cmbBoxAmortization";
      this.cmbBoxAmortization.Size = new Size(195, 22);
      this.cmbBoxAmortization.TabIndex = 11;
      this.cmbBoxAmortization.SelectedIndexChanged += new EventHandler(this.controlChanged);
      this.txtBoxPurposeOther.Location = new Point(161, 83);
      this.txtBoxPurposeOther.MaxLength = 30;
      this.txtBoxPurposeOther.Name = "txtBoxPurposeOther";
      this.txtBoxPurposeOther.Size = new Size(161, 20);
      this.txtBoxPurposeOther.TabIndex = 5;
      this.txtBoxPurposeOther.TextChanged += new EventHandler(this.controlChanged);
      this.cmbBoxLoanPurpose.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbBoxLoanPurpose.Location = new Point((int) sbyte.MaxValue, 58);
      this.cmbBoxLoanPurpose.Name = "cmbBoxLoanPurpose";
      this.cmbBoxLoanPurpose.Size = new Size(195, 22);
      this.cmbBoxLoanPurpose.TabIndex = 3;
      this.cmbBoxLoanPurpose.SelectedIndexChanged += new EventHandler(this.cmbBoxLoanPurpose_SelectedIndexChanged);
      this.gcLoanInfo.Controls.Add((Control) this.txtTypeOther);
      this.gcLoanInfo.Controls.Add((Control) this.lblTypeOther);
      this.gcLoanInfo.Controls.Add((Control) this.txtCreditScore);
      this.gcLoanInfo.Controls.Add((Control) this.lblCreditScore);
      this.gcLoanInfo.Controls.Add((Control) this.lblLoanType);
      this.gcLoanInfo.Controls.Add((Control) this.cmbLoanType);
      this.gcLoanInfo.Controls.Add((Control) this.label9);
      this.gcLoanInfo.Controls.Add((Control) this.txtCashOut);
      this.gcLoanInfo.Controls.Add((Control) this.lblAmortization);
      this.gcLoanInfo.Controls.Add((Control) this.lblDownPayment);
      this.gcLoanInfo.Controls.Add((Control) this.label7);
      this.gcLoanInfo.Controls.Add((Control) this.txtBoxDownPayment);
      this.gcLoanInfo.Controls.Add((Control) this.txtBoxLoanTerm);
      this.gcLoanInfo.Controls.Add((Control) this.lblLoanTerm);
      this.gcLoanInfo.Controls.Add((Control) this.txtBoxLoanAmount);
      this.gcLoanInfo.Controls.Add((Control) this.lblLoanAmount);
      this.gcLoanInfo.Controls.Add((Control) this.lblLoanPurpose);
      this.gcLoanInfo.Controls.Add((Control) this.cmbBoxLoanPurpose);
      this.gcLoanInfo.Controls.Add((Control) this.txtBoxPurposeOther);
      this.gcLoanInfo.Controls.Add((Control) this.cmbBoxAmortization);
      this.gcLoanInfo.Controls.Add((Control) this.lblPurposeOther);
      this.gcLoanInfo.Dock = DockStyle.Left;
      this.gcLoanInfo.HeaderForeColor = SystemColors.ControlText;
      this.gcLoanInfo.Location = new Point(1, 1);
      this.gcLoanInfo.Name = "gcLoanInfo";
      this.gcLoanInfo.Size = new Size(334, 276);
      this.gcLoanInfo.TabIndex = 2;
      this.gcLoanInfo.Text = "Loan Information";
      this.txtTypeOther.Location = new Point(161, 151);
      this.txtTypeOther.MaxLength = 30;
      this.txtTypeOther.Name = "txtTypeOther";
      this.txtTypeOther.Size = new Size(161, 20);
      this.txtTypeOther.TabIndex = 9;
      this.txtTypeOther.TextChanged += new EventHandler(this.controlChanged);
      this.lblTypeOther.Location = new Point(124, 154);
      this.lblTypeOther.Name = "lblTypeOther";
      this.lblTypeOther.Size = new Size(34, 16);
      this.lblTypeOther.TabIndex = 33;
      this.lblTypeOther.Text = "other";
      this.txtCreditScore.Location = new Point((int) sbyte.MaxValue, 174);
      this.txtCreditScore.MaxLength = 30;
      this.txtCreditScore.Name = "txtCreditScore";
      this.txtCreditScore.Size = new Size(195, 20);
      this.txtCreditScore.TabIndex = 10;
      this.txtCreditScore.TextChanged += new EventHandler(this.controlChanged);
      this.lblCreditScore.Location = new Point(10, 176);
      this.lblCreditScore.Name = "lblCreditScore";
      this.lblCreditScore.Size = new Size(121, 15);
      this.lblCreditScore.TabIndex = 31;
      this.lblCreditScore.Text = "Estimated Credit Score";
      this.lblLoanType.Location = new Point(10, 131);
      this.lblLoanType.Name = "lblLoanType";
      this.lblLoanType.Size = new Size(76, 15);
      this.lblLoanType.TabIndex = 29;
      this.lblLoanType.Text = "Loan Type";
      this.cmbLoanType.DropDownStyle = ComboBoxStyle.DropDownList;
      this.cmbLoanType.Location = new Point((int) sbyte.MaxValue, (int) sbyte.MaxValue);
      this.cmbLoanType.Name = "cmbLoanType";
      this.cmbLoanType.Size = new Size(195, 22);
      this.cmbLoanType.TabIndex = 8;
      this.cmbLoanType.SelectedIndexChanged += new EventHandler(this.cmbLoanType_SelectedIndexChanged);
      this.label9.AutoSize = true;
      this.label9.Location = new Point(164, 106);
      this.label9.Name = "label9";
      this.label9.Size = new Size(30, 14);
      this.label9.TabIndex = 28;
      this.label9.Text = "mths";
      this.gcProperty.Controls.Add((Control) this.dpPurchaseDate);
      this.gcProperty.Controls.Add((Control) this.lblPropertyValue);
      this.gcProperty.Controls.Add((Control) this.txtBoxPropertyValue);
      this.gcProperty.Controls.Add((Control) this.lblAddress);
      this.gcProperty.Controls.Add((Control) this.txtBoxZip);
      this.gcProperty.Controls.Add((Control) this.lblPurchaseDate);
      this.gcProperty.Controls.Add((Control) this.lblState);
      this.gcProperty.Controls.Add((Control) this.cmbBoxPropertyUse);
      this.gcProperty.Controls.Add((Control) this.lblPropertyUse);
      this.gcProperty.Controls.Add((Control) this.lblZip);
      this.gcProperty.Controls.Add((Control) this.cmbBoxPropertyType);
      this.gcProperty.Controls.Add((Control) this.txtBoxState);
      this.gcProperty.Controls.Add((Control) this.lblPropertyType);
      this.gcProperty.Controls.Add((Control) this.txtBoxAddress);
      this.gcProperty.Controls.Add((Control) this.lblCity);
      this.gcProperty.Controls.Add((Control) this.txtBoxCity);
      this.gcProperty.Dock = DockStyle.Right;
      this.gcProperty.HeaderForeColor = SystemColors.ControlText;
      this.gcProperty.Location = new Point(675, 1);
      this.gcProperty.Name = "gcProperty";
      this.gcProperty.Size = new Size(334, 276);
      this.gcProperty.TabIndex = 3;
      this.gcProperty.Text = "Subject Property";
      this.dpPurchaseDate.BackColor = SystemColors.Window;
      this.dpPurchaseDate.Location = new Point(113, 172);
      this.dpPurchaseDate.MaxValue = new DateTime(2100, 1, 1, 0, 0, 0, 0);
      this.dpPurchaseDate.MinValue = new DateTime(1900, 1, 1, 0, 0, 0, 0);
      this.dpPurchaseDate.Name = "dpPurchaseDate";
      this.dpPurchaseDate.Size = new Size(209, 22);
      this.dpPurchaseDate.TabIndex = 16;
      this.dpPurchaseDate.ToolTip = "";
      this.dpPurchaseDate.Value = new DateTime(0L);
      this.dpPurchaseDate.ValueChanged += new EventHandler(this.controlChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.White;
      this.ClientSize = new Size(1010, 278);
      this.Controls.Add((Control) this.pnlMiddle);
      this.Controls.Add((Control) this.gcProperty);
      this.Controls.Add((Control) this.gcLoanInfo);
      this.Font = new Font("Arial", 11f, FontStyle.Regular, GraphicsUnit.Pixel, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (OpportunityForm);
      this.Padding = new Padding(1);
      this.Text = "BorrowerInfoForm";
      this.SizeChanged += new EventHandler(this.Form_SizeChanged);
      this.pnlMiddle.ResumeLayout(false);
      this.groupContainer1.ResumeLayout(false);
      this.groupContainer1.PerformLayout();
      this.gcLoanInfo.ResumeLayout(false);
      this.gcLoanInfo.PerformLayout();
      this.gcProperty.ResumeLayout(false);
      this.gcProperty.PerformLayout();
      this.ResumeLayout(false);
    }

    public int CurrentContactID
    {
      get => this.borrowerObj != null ? this.borrowerObj.ContactID : -1;
      set
      {
        if (this.CurrentContactID == value)
          return;
        this.opportunity = (Opportunity) null;
        this.borrowerObj = (BorrowerInfo) null;
        this.clearAndDisableControls();
        if (value < 0)
          return;
        this.currentContactID = value;
        this.opportunity = Session.ContactManager.GetOpportunityByBorrowerId(value);
        this.borrowerObj = Session.ContactManager.GetBorrower(value);
        this.loadForm();
      }
    }

    public object CurrentContact
    {
      get => (object) this.borrowerObj;
      set
      {
        if (this.CurrentContact == value)
          return;
        this.opportunity = (Opportunity) null;
        this.borrowerObj = (BorrowerInfo) null;
        this.clearAndDisableControls();
        if (value == null)
          return;
        this.borrowerObj = (BorrowerInfo) value;
        this.currentContactID = this.borrowerObj.ContactID;
        this.opportunity = Session.ContactManager.GetOpportunityByBorrowerId(this.borrowerObj.ContactID);
        this.loadForm();
      }
    }

    public bool SaveChanges()
    {
      if (!this.changed)
        return false;
      bool flag = false;
      if (this.opportunity == null)
      {
        this.opportunity = new Opportunity();
        this.opportunity.ContactID = this.currentContactID;
        flag = true;
      }
      else if (this.opportunity.ContactID != this.currentContactID)
        throw new ApplicationException("Borrower opportunity does not belong to the specified borrower.");
      this.borrowerObj = Session.ContactManager.GetBorrower(this.currentContactID);
      this.opportunity.LoanAmount = Utils.ParseDecimal((object) this.txtBoxLoanAmount.Text);
      this.opportunity.CashOut = Utils.ParseDecimal((object) this.txtCashOut.Text);
      this.opportunity.Purpose = LoanPurposeEnumUtil.NameToValue(this.cmbBoxLoanPurpose.Text);
      this.opportunity.PurposeOther = this.txtBoxPurposeOther.Text;
      this.opportunity.LoanType = LoanTypeEnumUtil.NameToValue(this.cmbLoanType.Text);
      this.opportunity.TypeOther = this.txtTypeOther.Text;
      this.opportunity.EstimatedCreditScore = this.txtCreditScore.Text;
      this.opportunity.Term = Utils.ParseInt((object) this.txtBoxLoanTerm.Text);
      this.opportunity.Amortization = AmortizationTypeEnumUtil.NameToValue(this.cmbBoxAmortization.Text);
      this.opportunity.DownPayment = Utils.ParseDecimal((object) this.txtBoxDownPayment.Text);
      this.opportunity.PropertyAddress = new Address(this.txtBoxAddress.Text, string.Empty, this.txtBoxCity.Text, this.txtBoxState.Text, this.txtBoxZip.Text);
      this.opportunity.PropUse = PropertyUseEnumUtil.NameToValue(this.cmbBoxPropertyUse.Text);
      this.opportunity.PropType = PropertyTypeEnumUtil.NameToValue(this.cmbBoxPropertyType.Text);
      this.opportunity.PropertyValue = Utils.ParseDecimal((object) this.txtBoxPropertyValue.Text);
      this.opportunity.PurchaseDate = Utils.ParseDate((object) this.dpPurchaseDate.Text);
      this.opportunity.MortgageBalance = Utils.ParseDecimal((object) this.txtBoxMortgageBalance.Text);
      this.opportunity.MortgageRate = Utils.ParseDecimal((object) this.txtBoxMortgageRate.Text);
      this.opportunity.HousingPayment = Utils.ParseDecimal((object) this.txtBoxPaymentHousing.Text);
      this.opportunity.NonhousingPayment = Utils.ParseDecimal((object) this.txtBoxPaymentNonhousing.Text);
      this.opportunity.CreditRating = this.txtBoxCreditRating.Text;
      this.opportunity.IsBankruptcy = this.chkBoxBankruptcy.Checked;
      this.opportunity.Employment = EmploymentStatusEnumUtil.NameToValue(this.cmbBoxEmployment.Text);
      this.borrowerObj.Income = Utils.ParseDecimal((object) this.txtBoxIncome.Text);
      try
      {
        if (flag)
          Session.ContactManager.CreateBorrowerOpportunity(this.opportunity);
        else
          Session.ContactManager.UpdateBorrowerOpportunity(this.opportunity);
        Session.ContactManager.UpdateBorrower(this.borrowerObj);
      }
      catch (Exception ex)
      {
        throw new ObjectNotFoundException("Unable to update borrower contact '" + this.borrowerObj.LastName + ", " + this.borrowerObj.FirstName + "' (ContactID:" + (object) this.borrowerObj.ContactID + ").", ObjectType.Contact, (object) this.currentContactID);
      }
      this.changed = false;
      return true;
    }

    public void disableForm() => this.disableControls();

    private void disableControls()
    {
      this.txtBoxAddress.ReadOnly = true;
      this.txtBoxCity.ReadOnly = true;
      this.txtBoxCreditRating.ReadOnly = true;
      this.txtBoxDownPayment.ReadOnly = true;
      this.txtBoxLoanAmount.ReadOnly = true;
      this.txtBoxLoanTerm.ReadOnly = true;
      this.txtBoxMortgageBalance.ReadOnly = true;
      this.txtBoxMortgageRate.ReadOnly = true;
      this.txtBoxPaymentHousing.ReadOnly = true;
      this.txtBoxPaymentNonhousing.ReadOnly = true;
      this.txtBoxPropertyValue.ReadOnly = true;
      this.dpPurchaseDate.ReadOnly = true;
      this.txtBoxPurposeOther.ReadOnly = true;
      this.txtTypeOther.ReadOnly = true;
      this.txtCreditScore.ReadOnly = true;
      this.txtBoxState.ReadOnly = true;
      this.txtBoxZip.ReadOnly = true;
      this.chkBoxBankruptcy.Enabled = false;
      this.cmbBoxAmortization.Enabled = false;
      this.cmbBoxEmployment.Enabled = false;
      this.cmbBoxLoanPurpose.Enabled = false;
      this.cmbLoanType.Enabled = false;
      this.cmbBoxPropertyType.Enabled = false;
      this.cmbBoxPropertyUse.Enabled = false;
      this.txtCashOut.ReadOnly = true;
      this.txtBoxIncome.ReadOnly = true;
    }

    private void clearAndDisableControls()
    {
      this.clearControls();
      this.disableControls();
    }

    private void enableControls()
    {
      this.txtBoxAddress.ReadOnly = false;
      this.txtBoxCity.ReadOnly = false;
      this.txtBoxCreditRating.ReadOnly = false;
      this.txtBoxDownPayment.ReadOnly = false;
      this.txtBoxLoanAmount.ReadOnly = false;
      this.txtBoxLoanTerm.ReadOnly = false;
      this.txtBoxMortgageBalance.ReadOnly = false;
      this.txtBoxMortgageRate.ReadOnly = false;
      this.txtBoxPaymentHousing.ReadOnly = false;
      this.txtBoxPaymentNonhousing.ReadOnly = false;
      this.txtBoxPropertyValue.ReadOnly = false;
      this.dpPurchaseDate.ReadOnly = false;
      this.txtBoxPurposeOther.ReadOnly = false;
      this.txtTypeOther.ReadOnly = false;
      this.txtCreditScore.ReadOnly = false;
      this.txtBoxState.ReadOnly = false;
      this.txtBoxZip.ReadOnly = false;
      this.chkBoxBankruptcy.Enabled = true;
      this.cmbBoxAmortization.Enabled = true;
      this.cmbBoxEmployment.Enabled = true;
      this.cmbBoxLoanPurpose.Enabled = true;
      this.cmbLoanType.Enabled = true;
      this.cmbBoxPropertyType.Enabled = true;
      this.cmbBoxPropertyUse.Enabled = true;
      this.txtCashOut.ReadOnly = false;
      this.txtBoxIncome.ReadOnly = false;
    }

    private void clearControls()
    {
      this.txtBoxAddress.Text = "";
      this.txtBoxCity.Text = "";
      this.txtBoxCreditRating.Text = "";
      this.txtBoxDownPayment.Text = "";
      this.txtBoxLoanAmount.Text = "";
      this.txtBoxLoanTerm.Text = "";
      this.txtBoxMortgageBalance.Text = "";
      this.txtBoxMortgageRate.Text = "";
      this.txtBoxPaymentHousing.Text = "";
      this.txtBoxPaymentNonhousing.Text = "";
      this.txtBoxPropertyValue.Text = "";
      this.dpPurchaseDate.Text = "";
      this.txtBoxPurposeOther.Text = "";
      this.txtTypeOther.Text = "";
      this.txtCreditScore.Text = "";
      this.txtBoxState.Text = "";
      this.txtBoxZip.Text = "";
      this.chkBoxBankruptcy.Checked = false;
      this.cmbBoxAmortization.SelectedIndex = 0;
      this.cmbBoxEmployment.SelectedIndex = 0;
      this.cmbBoxLoanPurpose.SelectedIndex = 0;
      this.cmbLoanType.SelectedIndex = 0;
      this.cmbBoxPropertyType.SelectedIndex = 0;
      this.cmbBoxPropertyUse.SelectedIndex = 0;
      this.txtCashOut.Text = "";
      this.txtBoxIncome.Text = "";
      this.changed = false;
    }

    private void loadForm()
    {
      this.initialLoad = true;
      if (!this.IsReadOnly)
        this.enableControls();
      if (this.borrowerObj != null)
        this.txtBoxIncome.Text = this.borrowerObj.Income == 0M ? "" : this.borrowerObj.Income.ToString("N0");
      if (this.opportunity == null)
      {
        this.txtBoxPurposeOther.Enabled = false;
        this.changed = false;
        this.initialLoad = false;
      }
      else
      {
        this.txtBoxAddress.Text = this.opportunity.PropertyAddress.Street1;
        this.txtBoxCity.Text = this.opportunity.PropertyAddress.City;
        this.txtBoxCreditRating.Text = this.opportunity.CreditRating;
        this.txtBoxDownPayment.Text = this.opportunity.DownPayment == 0M ? "" : this.opportunity.DownPayment.ToString("N0");
        this.txtBoxLoanAmount.Text = this.opportunity.LoanAmount == 0M ? "" : this.opportunity.LoanAmount.ToString("N0");
        this.txtBoxLoanTerm.Text = this.opportunity.Term == -1 ? "" : this.opportunity.Term.ToString();
        this.txtBoxMortgageBalance.Text = this.opportunity.MortgageBalance == 0M ? "" : this.opportunity.MortgageBalance.ToString("N0");
        this.txtBoxMortgageRate.Text = this.opportunity.MortgageRate == 0M ? "" : this.opportunity.MortgageRate.ToString("F3");
        this.txtBoxPaymentHousing.Text = this.opportunity.HousingPayment == 0M ? "" : this.opportunity.HousingPayment.ToString("N2");
        this.txtBoxPaymentNonhousing.Text = this.opportunity.NonhousingPayment == 0M ? "" : this.opportunity.NonhousingPayment.ToString("N2");
        this.txtBoxPropertyValue.Text = this.opportunity.PropertyValue == 0M ? "" : this.opportunity.PropertyValue.ToString("N0");
        this.txtCashOut.Text = this.opportunity.CashOut == 0M ? "" : this.opportunity.CashOut.ToString("N0");
        if (this.opportunity.PurchaseDate == DateTime.MinValue)
          this.dpPurchaseDate.Text = "";
        else
          this.dpPurchaseDate.Text = this.opportunity.PurchaseDate.ToString("MM/dd/yyyy");
        if (this.opportunity.Purpose == EllieMae.EMLite.Common.Contact.LoanPurpose.Other)
        {
          this.txtBoxPurposeOther.Enabled = true;
          this.txtBoxPurposeOther.Text = this.opportunity.PurposeOther;
        }
        else
        {
          this.txtBoxPurposeOther.Text = string.Empty;
          this.txtBoxPurposeOther.Enabled = false;
        }
        if (this.opportunity.LoanType == LoanTypeEnum.Other)
        {
          this.txtTypeOther.Enabled = true;
          this.txtTypeOther.Text = this.opportunity.TypeOther;
        }
        else
        {
          this.txtTypeOther.Text = string.Empty;
          this.txtTypeOther.Enabled = false;
        }
        this.txtCreditScore.Text = this.opportunity.EstimatedCreditScore;
        this.txtBoxState.Text = this.opportunity.PropertyAddress.State;
        this.txtBoxZip.Text = this.opportunity.PropertyAddress.Zip;
        this.chkBoxBankruptcy.Checked = this.opportunity.IsBankruptcy;
        this.cmbBoxAmortization.Text = AmortizationTypeEnumUtil.ValueToName(this.opportunity.Amortization);
        this.cmbBoxEmployment.Text = EmploymentStatusEnumUtil.ValueToName(this.opportunity.Employment);
        this.cmbBoxLoanPurpose.Text = LoanPurposeEnumUtil.ValueToName(this.opportunity.Purpose);
        this.cmbLoanType.Text = LoanTypeEnumUtil.ValueToName(this.opportunity.LoanType);
        this.cmbBoxPropertyType.Text = PropertyTypeEnumUtil.ValueToName(this.opportunity.PropType);
        this.cmbBoxPropertyUse.Text = PropertyUseEnumUtil.ValueToName(this.opportunity.PropUse);
        this.changed = false;
        this.initialLoad = false;
      }
    }

    private void Form_SizeChanged(object sender, EventArgs e)
    {
      this.gcProperty.Size = this.gcLoanInfo.Size = this.Width <= 1010 ? new Size(334, this.Height) : new Size((this.Width - 8) / 3, this.Height);
    }

    private void controlChanged(object sender, EventArgs e)
    {
      if (this.initialLoad)
        return;
      this.formatText(sender, e);
      this.changed = true;
      if (this.SummaryChanged == null)
        return;
      this.SummaryChanged();
    }

    private void txtBoxZip_Leave(object sender, EventArgs e)
    {
      if (this.txtBoxZip.Text == "")
        return;
      ZipCodeInfo zipCodeInfo = ZipcodeSelector.GetZipCodeInfo(this.txtBoxZip.Text, ZipCodeUtils.GetMultipleZipInfoAt(this.txtBoxZip.Text));
      if (zipCodeInfo == null)
        return;
      this.txtBoxCity.Text = zipCodeInfo.City;
      this.txtBoxState.Text = zipCodeInfo.State;
    }

    public bool isDirty() => this.changed;

    private void txtBoxState_Leave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      textBox.Text = textBox.Text.ToUpper();
    }

    private void txtBoxKeyDown(object sender, KeyEventArgs e)
    {
      if (this.initialLoad || e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete)
        return;
      this.deleteBackKey = true;
    }

    public void formatText(object sender, EventArgs e)
    {
      if (this.intermidiateData)
        this.intermidiateData = false;
      else if (this.deleteBackKey)
      {
        this.deleteBackKey = false;
      }
      else
      {
        FieldFormat dataFormat;
        if (sender == this.txtBoxZip)
          dataFormat = FieldFormat.ZIPCODE;
        else if (sender == this.txtBoxState)
          dataFormat = FieldFormat.STATE;
        else if (sender == this.txtBoxPaymentHousing || sender == this.txtBoxPaymentNonhousing)
          dataFormat = FieldFormat.DECIMAL_2;
        else if (sender == this.txtBoxMortgageRate)
        {
          dataFormat = FieldFormat.DECIMAL_3;
        }
        else
        {
          if (sender != this.txtBoxLoanAmount && sender != this.txtBoxDownPayment && sender != this.txtBoxPropertyValue && sender != this.txtBoxMortgageBalance && sender != this.txtBoxLoanTerm && sender != this.txtCashOut && sender != this.txtBoxIncome)
            return;
          dataFormat = FieldFormat.INTEGER;
        }
        TextBox textBox = (TextBox) sender;
        string text = textBox.Text;
        bool needsUpdate = false;
        string str = Utils.FormatInput(text, dataFormat, ref needsUpdate);
        if (sender == this.txtBoxIncome)
          str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
        try
        {
          if (sender == this.txtBoxLoanTerm)
          {
            if (str.StartsWith("-"))
              str = str.Substring(1);
            if (str.Length > 3)
              str = str.Substring(0, 3);
          }
          if (sender == this.txtBoxIncome)
            str = Decimal.Parse(str).ToString("N0");
          if (sender == this.txtBoxLoanAmount || sender == this.txtBoxDownPayment || sender == this.txtBoxPropertyValue || sender == this.txtBoxMortgageBalance || sender == this.txtCashOut)
            str = Decimal.Parse(str).ToString("N0");
          if (sender == this.txtBoxPaymentHousing || sender == this.txtBoxPaymentNonhousing)
          {
            int num = str.LastIndexOf('.');
            if (num == -1)
              str = Decimal.Parse(str).ToString("N0");
            else if (num == str.Length - 3)
              str = Decimal.Parse(str).ToString("N2");
            else if (num == str.Length - 2)
              str = Decimal.Parse(str).ToString("N1");
            else if (num == str.Length - 1)
              str = Decimal.Parse(str).ToString("N0") + ".";
          }
          TextBox txtBoxMortgageRate = this.txtBoxMortgageRate;
        }
        catch
        {
        }
        if (!(str != textBox.Text))
          return;
        this.intermidiateData = true;
        int selectionStart = textBox.SelectionStart;
        int newCursorPosition;
        if (sender == this.txtBoxMortgageRate)
          newCursorPosition = Utils.GetNewCursorPosition(textBox.Text, str, textBox.SelectionStart, new string[1]
          {
            "."
          });
        else
          newCursorPosition = Utils.GetNewCursorPosition(textBox.Text, str, textBox.SelectionStart, new string[2]
          {
            "-",
            ","
          });
        textBox.Text = str;
        textBox.SelectionStart = newCursorPosition;
      }
    }

    private void cmbBoxLoanPurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbBoxLoanPurpose.Text == "Other")
        this.txtBoxPurposeOther.Enabled = true;
      else
        this.txtBoxPurposeOther.Enabled = false;
      this.controlChanged(sender, e);
    }

    private void cmbLoanType_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.cmbLoanType.Text == "Other")
        this.txtTypeOther.Enabled = true;
      else
        this.txtTypeOther.Enabled = false;
      this.controlChanged(sender, e);
    }
  }
}
