// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationObligationControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationObligationControl : UserControl, IVerificationDetails
  {
    public EventHandler OnStatusChanged;
    private VerificationDetailsControl.BorrowerEditMode editMode;
    private IContainer components;
    private GroupContainer groupContainerBor;
    private CheckBox chkRevolving;
    private CheckBox chkSimultaneous;
    private CheckBox chkObOther;
    private CheckBox chkHousing;
    private CheckBox chkAlimony;
    private CheckBox chkRealEstate;
    private CheckBox chkInstallment;
    private Label label1;
    private Panel panelNonEmployment;
    private Panel panelEmployment;
    private CheckBox chkChildSupport;
    private CheckBox chkRequiredEscrow;
    private TextBox txtOBOther;
    private Label label8;
    private CheckBox chk2ndLien;
    private CheckBox chkDebtOb;
    private TextBox txtRepayAmt;
    private Label label6;
    private TextBox txtHowMany;
    private Label label4;
    private CheckBox chkRental;
    private CheckBox chkAgeCredit;
    private CheckBox chkCollections;
    private CheckBox chkJudgements;
    private CheckBox chkCreditOther;
    private TextBox txtCreditOther;
    private CheckBox chkPaymentHistory;
    private CheckBox chkBackruptcies;
    private Label label2;
    private TextBox txtHowMuch;
    private CheckBox chkHELOC;
    private CheckBox chkMortgageLates;

    public VerificationObligationControl(
      VerificationDetailsControl.BorrowerEditMode editMode)
    {
      this.editMode = editMode;
      this.InitializeComponent();
      this.Dock = DockStyle.Fill;
      this.SetBorrowerType(editMode);
    }

    public void SetBorrowerType(
      VerificationDetailsControl.BorrowerEditMode editMode)
    {
      this.editMode = editMode;
      if (this.editMode == VerificationDetailsControl.BorrowerEditMode.CoBorrower)
        this.groupContainerBor.Text = "Co-Borrower";
      else
        this.groupContainerBor.Text = "Borrower";
    }

    public void SetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineObligationLog timelineObligationLog = (VerificationTimelineObligationLog) verificationLog;
      this.chkInstallment.Checked = timelineObligationLog.IsInstallmentLoans;
      this.chkRealEstate.Checked = timelineObligationLog.IsRealEstateLoans;
      this.chkAlimony.Checked = timelineObligationLog.IsAlimonyOrMaintenance;
      this.chkHousing.Checked = timelineObligationLog.IsMonthlyHousingExpense;
      this.chkRevolving.Checked = timelineObligationLog.IsRevolvingChargeAccounts;
      this.chkSimultaneous.Checked = timelineObligationLog.IsSimultaneousLoansOnProperty;
      this.chkChildSupport.Checked = timelineObligationLog.IsChildSupport;
      this.chkRequiredEscrow.Checked = timelineObligationLog.IsRequiredEscrows;
      this.chkObOther.Checked = timelineObligationLog.IsOtherMonthlyObligation;
      this.txtOBOther.Text = timelineObligationLog.OtherMonthlyObligationDescription;
      this.chkAgeCredit.Checked = timelineObligationLog.IsNoAndAgeOfCreditLines;
      this.chkJudgements.Checked = timelineObligationLog.IsJudgements;
      this.chkBackruptcies.Checked = timelineObligationLog.IsBackruptcies;
      this.chkMortgageLates.Checked = timelineObligationLog.IsMortgageLates;
      this.txtHowMany.Text = timelineObligationLog.NumberOfMortgageLates == 0 ? "" : timelineObligationLog.NumberOfMortgageLates.ToString("0");
      this.chkHELOC.Checked = timelineObligationLog.IsHELOC;
      TextBox txtRepayAmt = this.txtRepayAmt;
      Decimal num;
      string str1;
      if (!(timelineObligationLog.RepayAmountToHELOC == 0M))
      {
        num = timelineObligationLog.RepayAmountToHELOC;
        str1 = num.ToString("N2");
      }
      else
        str1 = "";
      txtRepayAmt.Text = str1;
      this.chkPaymentHistory.Checked = timelineObligationLog.IsPaymentHistory;
      this.chkCollections.Checked = timelineObligationLog.IsCollections;
      this.chkRental.Checked = timelineObligationLog.IsRentalPaymentHistory;
      this.chkDebtOb.Checked = timelineObligationLog.IsDebtObligationsCurrent;
      this.chk2ndLien.Checked = timelineObligationLog.Is2ndLien;
      TextBox txtHowMuch = this.txtHowMuch;
      string str2;
      if (!(timelineObligationLog.Amount2ndLien == 0M))
      {
        num = timelineObligationLog.Amount2ndLien;
        str2 = num.ToString("N2");
      }
      else
        str2 = "";
      txtHowMuch.Text = str2;
      this.chkCreditOther.Checked = timelineObligationLog.IsOtherCreditHistory;
      this.txtCreditOther.Text = timelineObligationLog.OtherCreditHistoryDescription;
      this.checkbox_CheckedChanged((object) this.chkObOther, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chkCreditOther, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chkMortgageLates, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chkHELOC, (EventArgs) null);
      this.checkbox_CheckedChanged((object) this.chk2ndLien, (EventArgs) null);
    }

    public void GetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineObligationLog timelineObligationLog = (VerificationTimelineObligationLog) verificationLog;
      timelineObligationLog.IsInstallmentLoans = this.chkInstallment.Checked;
      timelineObligationLog.IsRealEstateLoans = this.chkRealEstate.Checked;
      timelineObligationLog.IsAlimonyOrMaintenance = this.chkAlimony.Checked;
      timelineObligationLog.IsMonthlyHousingExpense = this.chkHousing.Checked;
      timelineObligationLog.IsRevolvingChargeAccounts = this.chkRevolving.Checked;
      timelineObligationLog.IsSimultaneousLoansOnProperty = this.chkSimultaneous.Checked;
      timelineObligationLog.IsChildSupport = this.chkChildSupport.Checked;
      timelineObligationLog.IsRequiredEscrows = this.chkRequiredEscrow.Checked;
      timelineObligationLog.IsOtherMonthlyObligation = this.chkObOther.Checked;
      timelineObligationLog.OtherMonthlyObligationDescription = this.txtOBOther.Text.Trim();
      timelineObligationLog.IsNoAndAgeOfCreditLines = this.chkAgeCredit.Checked;
      timelineObligationLog.IsJudgements = this.chkJudgements.Checked;
      timelineObligationLog.IsBackruptcies = this.chkBackruptcies.Checked;
      timelineObligationLog.IsMortgageLates = this.chkMortgageLates.Checked;
      timelineObligationLog.NumberOfMortgageLates = this.txtHowMany.Text.Trim() == "" ? 0 : Utils.ParseInt((object) this.txtHowMany.Text.Trim());
      timelineObligationLog.IsHELOC = this.chkHELOC.Checked;
      timelineObligationLog.RepayAmountToHELOC = this.txtRepayAmt.Text.Trim() == "" ? 0M : Utils.ParseDecimal((object) this.txtRepayAmt.Text.Trim(), 0M);
      timelineObligationLog.IsPaymentHistory = this.chkPaymentHistory.Checked;
      timelineObligationLog.IsCollections = this.chkCollections.Checked;
      timelineObligationLog.IsRentalPaymentHistory = this.chkRental.Checked;
      timelineObligationLog.IsDebtObligationsCurrent = this.chkDebtOb.Checked;
      timelineObligationLog.Is2ndLien = this.chk2ndLien.Checked;
      timelineObligationLog.Amount2ndLien = this.txtHowMuch.Text.Trim() == "" ? 0M : Utils.ParseDecimal((object) this.txtHowMuch.Text.Trim(), 0M);
      timelineObligationLog.IsOtherCreditHistory = this.chkCreditOther.Checked;
      timelineObligationLog.OtherCreditHistoryDescription = this.txtCreditOther.Text.Trim();
    }

    private void onNumericFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
        return;
      e.Handled = true;
    }

    private void onDecimalFieldKeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || e.KeyChar.Equals('.'))
        return;
      e.Handled = true;
    }

    private void checkbox_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      CheckBox checkBox = (CheckBox) sender;
      if (checkBox.Checked)
      {
        if (checkBox.Name != this.chkInstallment.Name)
          this.chkInstallment.Checked = false;
        if (checkBox.Name != this.chkRealEstate.Name)
          this.chkRealEstate.Checked = false;
        if (checkBox.Name != this.chkAlimony.Name)
          this.chkAlimony.Checked = false;
        if (checkBox.Name != this.chkHousing.Name)
          this.chkHousing.Checked = false;
        if (checkBox.Name != this.chkRevolving.Name)
          this.chkRevolving.Checked = false;
        if (checkBox.Name != this.chkSimultaneous.Name)
          this.chkSimultaneous.Checked = false;
        if (checkBox.Name != this.chkChildSupport.Name)
          this.chkChildSupport.Checked = false;
        if (checkBox.Name != this.chkRequiredEscrow.Name)
          this.chkRequiredEscrow.Checked = false;
        if (checkBox.Name != this.chkObOther.Name)
          this.chkObOther.Checked = false;
        if (checkBox.Name != this.chkAgeCredit.Name)
          this.chkAgeCredit.Checked = false;
        if (checkBox.Name != this.chkJudgements.Name)
          this.chkJudgements.Checked = false;
        if (checkBox.Name != this.chkBackruptcies.Name)
          this.chkBackruptcies.Checked = false;
        if (checkBox.Name != this.chkMortgageLates.Name)
          this.chkMortgageLates.Checked = false;
        if (checkBox.Name != this.chkHELOC.Name)
          this.chkHELOC.Checked = false;
        if (checkBox.Name != this.chkPaymentHistory.Name)
          this.chkPaymentHistory.Checked = false;
        if (checkBox.Name != this.chkCollections.Name)
          this.chkCollections.Checked = false;
        if (checkBox.Name != this.chkRental.Name)
          this.chkRental.Checked = false;
        if (checkBox.Name != this.chkDebtOb.Name)
          this.chkDebtOb.Checked = false;
        if (checkBox.Name != this.chk2ndLien.Name)
          this.chk2ndLien.Checked = false;
        if (checkBox.Name != this.chkCreditOther.Name)
          this.chkCreditOther.Checked = false;
      }
      if (checkBox.Name == "chkObOther")
      {
        this.txtOBOther.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtOBOther.Text = string.Empty;
      }
      else if (checkBox.Name == "chkCreditOther")
      {
        this.txtCreditOther.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtCreditOther.Text = string.Empty;
      }
      else if (checkBox.Name == "chkMortgageLates")
      {
        this.txtHowMany.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtHowMany.Text = string.Empty;
      }
      else if (checkBox.Name == "chkHELOC")
      {
        this.txtRepayAmt.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtRepayAmt.Text = string.Empty;
      }
      else if (checkBox.Name == "chk2ndLien")
      {
        this.txtHowMuch.ReadOnly = !checkBox.Checked;
        if (!checkBox.Checked)
          this.txtHowMuch.Text = string.Empty;
      }
      if (this.OnStatusChanged == null)
        return;
      this.OnStatusChanged(sender, new EventArgs());
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.chkInstallment.Checked)
        str += "Installment Loans";
      if (this.chkRealEstate.Checked)
        str = str + (str != string.Empty ? "," : "") + "Real Estate Loans";
      if (this.chkAlimony.Checked)
        str = str + (str != string.Empty ? "," : "") + "Alimony/Maintenance";
      if (this.chkHousing.Checked)
        str = str + (str != string.Empty ? "," : "") + "Monthly Housing Expense [P & I]";
      if (this.chkRevolving.Checked)
        str = str + (str != string.Empty ? "," : "") + "Revolving Charge Accounts";
      if (this.chkSimultaneous.Checked)
        str = str + (str != string.Empty ? "," : "") + "Simultaneous Loans on Property";
      if (this.chkChildSupport.Checked)
        str = str + (str != string.Empty ? "," : "") + "Child Support";
      if (this.chkRequiredEscrow.Checked)
        str = str + (str != string.Empty ? "," : "") + "Required Escrows";
      if (this.chkObOther.Checked)
        str = str + (str != string.Empty ? "," : "") + "Other Monthly Obligation";
      if (this.chkAgeCredit.Checked)
        str = str + (str != string.Empty ? "," : "") + "No. and Age of Credit Lines";
      if (this.chkJudgements.Checked)
        str = str + (str != string.Empty ? "," : "") + "Judgements";
      if (this.chkBackruptcies.Checked)
        str = str + (str != string.Empty ? "," : "") + "Backruptcies";
      if (this.chkMortgageLates.Checked)
        str = str + (str != string.Empty ? "," : "") + "Mortgage Lates";
      if (this.chkHELOC.Checked)
        str = str + (str != string.Empty ? "," : "") + "HELOC";
      if (this.chkPaymentHistory.Checked)
        str = str + (str != string.Empty ? "," : "") + "Payment History";
      if (this.chkCollections.Checked)
        str = str + (str != string.Empty ? "," : "") + "Collections";
      if (this.chkRental.Checked)
        str = str + (str != string.Empty ? "," : "") + "Rental Payment History";
      if (this.chkDebtOb.Checked)
        str = str + (str != string.Empty ? "," : "") + "Debt Obligations Current";
      if (this.chk2ndLien.Checked)
        str = str + (str != string.Empty ? "," : "") + "2nd Lien";
      if (this.chkCreditOther.Checked)
        str = str + (str != string.Empty ? "," : "") + "Other Credit History";
      return str;
    }

    private void onNumericFieldKeyUp(object sender, KeyEventArgs e)
    {
      FieldFormat dataFormat = FieldFormat.DECIMAL_2;
      TextBox textBox = (TextBox) sender;
      bool needsUpdate = false;
      string str = Utils.FormatInput(textBox.Text, dataFormat, ref needsUpdate);
      if (!needsUpdate)
        return;
      textBox.Text = str;
      textBox.SelectionStart = str.Length;
    }

    private void onNumericFieldLeave(object sender, EventArgs e)
    {
      TextBox textBox = (TextBox) sender;
      if (textBox.Name == "txtHowMany")
        return;
      if (Utils.ParseDouble((object) textBox.Text.Trim()) == 0.0)
        textBox.Text = "";
      else
        textBox.Text = Utils.ApplyFieldFormatting(textBox.Text.Trim(), FieldFormat.DECIMAL_2);
    }

    public void SetReadOnly()
    {
      this.chkInstallment.Enabled = false;
      this.chkRealEstate.Enabled = false;
      this.chkAlimony.Enabled = false;
      this.chkHousing.Enabled = false;
      this.chkRevolving.Enabled = false;
      this.chkSimultaneous.Enabled = false;
      this.chkChildSupport.Enabled = false;
      this.chkRequiredEscrow.Enabled = false;
      this.chkObOther.Enabled = false;
      this.txtOBOther.ReadOnly = true;
      this.chkAgeCredit.Enabled = false;
      this.chkJudgements.Enabled = false;
      this.chkBackruptcies.Enabled = false;
      this.chkMortgageLates.Enabled = false;
      this.txtHowMany.ReadOnly = true;
      this.chkHELOC.Enabled = false;
      this.txtRepayAmt.ReadOnly = true;
      this.chkPaymentHistory.Enabled = false;
      this.chkCollections.Enabled = false;
      this.chkRental.Enabled = false;
      this.chkDebtOb.Enabled = false;
      this.chk2ndLien.Enabled = false;
      this.txtHowMuch.ReadOnly = true;
      this.chkCreditOther.Enabled = false;
      this.txtCreditOther.ReadOnly = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.groupContainerBor = new GroupContainer();
      this.panelNonEmployment = new Panel();
      this.txtHowMany = new TextBox();
      this.txtHowMuch = new TextBox();
      this.chkHELOC = new CheckBox();
      this.chkMortgageLates = new CheckBox();
      this.label8 = new Label();
      this.chk2ndLien = new CheckBox();
      this.chkDebtOb = new CheckBox();
      this.txtRepayAmt = new TextBox();
      this.label6 = new Label();
      this.label4 = new Label();
      this.chkRental = new CheckBox();
      this.chkAgeCredit = new CheckBox();
      this.chkCollections = new CheckBox();
      this.chkJudgements = new CheckBox();
      this.chkCreditOther = new CheckBox();
      this.txtCreditOther = new TextBox();
      this.chkPaymentHistory = new CheckBox();
      this.chkBackruptcies = new CheckBox();
      this.label2 = new Label();
      this.panelEmployment = new Panel();
      this.label1 = new Label();
      this.chkChildSupport = new CheckBox();
      this.chkInstallment = new CheckBox();
      this.chkRequiredEscrow = new CheckBox();
      this.chkSimultaneous = new CheckBox();
      this.chkRealEstate = new CheckBox();
      this.chkObOther = new CheckBox();
      this.txtOBOther = new TextBox();
      this.chkRevolving = new CheckBox();
      this.chkAlimony = new CheckBox();
      this.chkHousing = new CheckBox();
      this.groupContainerBor.SuspendLayout();
      this.panelNonEmployment.SuspendLayout();
      this.panelEmployment.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerBor.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerBor.Controls.Add((Control) this.panelNonEmployment);
      this.groupContainerBor.Controls.Add((Control) this.panelEmployment);
      this.groupContainerBor.Dock = DockStyle.Fill;
      this.groupContainerBor.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerBor.Location = new Point(0, 0);
      this.groupContainerBor.Name = "groupContainerBor";
      this.groupContainerBor.Size = new Size(650, 314);
      this.groupContainerBor.TabIndex = 1;
      this.groupContainerBor.Text = "Borrower";
      this.panelNonEmployment.BackColor = Color.White;
      this.panelNonEmployment.Controls.Add((Control) this.txtHowMany);
      this.panelNonEmployment.Controls.Add((Control) this.txtHowMuch);
      this.panelNonEmployment.Controls.Add((Control) this.chkHELOC);
      this.panelNonEmployment.Controls.Add((Control) this.chkMortgageLates);
      this.panelNonEmployment.Controls.Add((Control) this.label8);
      this.panelNonEmployment.Controls.Add((Control) this.chk2ndLien);
      this.panelNonEmployment.Controls.Add((Control) this.chkDebtOb);
      this.panelNonEmployment.Controls.Add((Control) this.txtRepayAmt);
      this.panelNonEmployment.Controls.Add((Control) this.label6);
      this.panelNonEmployment.Controls.Add((Control) this.label4);
      this.panelNonEmployment.Controls.Add((Control) this.chkRental);
      this.panelNonEmployment.Controls.Add((Control) this.chkAgeCredit);
      this.panelNonEmployment.Controls.Add((Control) this.chkCollections);
      this.panelNonEmployment.Controls.Add((Control) this.chkJudgements);
      this.panelNonEmployment.Controls.Add((Control) this.chkCreditOther);
      this.panelNonEmployment.Controls.Add((Control) this.txtCreditOther);
      this.panelNonEmployment.Controls.Add((Control) this.chkPaymentHistory);
      this.panelNonEmployment.Controls.Add((Control) this.chkBackruptcies);
      this.panelNonEmployment.Controls.Add((Control) this.label2);
      this.panelNonEmployment.Dock = DockStyle.Fill;
      this.panelNonEmployment.Location = new Point(1, 140);
      this.panelNonEmployment.Name = "panelNonEmployment";
      this.panelNonEmployment.Size = new Size(648, 173);
      this.panelNonEmployment.TabIndex = 14;
      this.txtHowMany.BorderStyle = BorderStyle.FixedSingle;
      this.txtHowMany.Location = new Point(88, 108);
      this.txtHowMany.MaxLength = 3;
      this.txtHowMany.Name = "txtHowMany";
      this.txtHowMany.Size = new Size(35, 20);
      this.txtHowMany.TabIndex = 17;
      this.txtHowMany.TextAlign = HorizontalAlignment.Right;
      this.txtHowMany.KeyPress += new KeyPressEventHandler(this.onNumericFieldKeyPress);
      this.txtHowMany.KeyUp += new KeyEventHandler(this.onNumericFieldKeyUp);
      this.txtHowMany.Leave += new EventHandler(this.onNumericFieldLeave);
      this.txtHowMuch.BorderStyle = BorderStyle.FixedSingle;
      this.txtHowMuch.Location = new Point(301, 109);
      this.txtHowMuch.MaxLength = 10;
      this.txtHowMuch.Name = "txtHowMuch";
      this.txtHowMuch.Size = new Size(99, 20);
      this.txtHowMuch.TabIndex = 25;
      this.txtHowMuch.TextAlign = HorizontalAlignment.Right;
      this.txtHowMuch.KeyPress += new KeyPressEventHandler(this.onDecimalFieldKeyPress);
      this.txtHowMuch.KeyUp += new KeyEventHandler(this.onNumericFieldKeyUp);
      this.txtHowMuch.Leave += new EventHandler(this.onNumericFieldLeave);
      this.chkHELOC.AutoSize = true;
      this.chkHELOC.Location = new Point(10, 128);
      this.chkHELOC.Name = "chkHELOC";
      this.chkHELOC.Size = new Size(62, 17);
      this.chkHELOC.TabIndex = 18;
      this.chkHELOC.Text = "HELOC";
      this.chkHELOC.UseVisualStyleBackColor = true;
      this.chkHELOC.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkMortgageLates.AutoSize = true;
      this.chkMortgageLates.Location = new Point(10, 93);
      this.chkMortgageLates.Name = "chkMortgageLates";
      this.chkMortgageLates.Size = new Size(100, 17);
      this.chkMortgageLates.TabIndex = 16;
      this.chkMortgageLates.Text = "Mortgage Lates";
      this.chkMortgageLates.UseVisualStyleBackColor = true;
      this.chkMortgageLates.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.label8.AutoSize = true;
      this.label8.Location = new Point(231, 112);
      this.label8.Name = "label8";
      this.label8.Size = new Size(68, 13);
      this.label8.TabIndex = 38;
      this.label8.Text = "How Much $";
      this.chk2ndLien.AutoSize = true;
      this.chk2ndLien.Location = new Point(215, 95);
      this.chk2ndLien.Name = "chk2ndLien";
      this.chk2ndLien.Size = new Size(67, 17);
      this.chk2ndLien.TabIndex = 24;
      this.chk2ndLien.Text = "2nd Lien";
      this.chk2ndLien.UseVisualStyleBackColor = true;
      this.chk2ndLien.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkDebtOb.AutoSize = true;
      this.chkDebtOb.Location = new Point(215, 76);
      this.chkDebtOb.Name = "chkDebtOb";
      this.chkDebtOb.Size = new Size(141, 17);
      this.chkDebtOb.TabIndex = 23;
      this.chkDebtOb.Text = "Debt Obligations Current";
      this.chkDebtOb.UseVisualStyleBackColor = true;
      this.chkDebtOb.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.txtRepayAmt.BorderStyle = BorderStyle.FixedSingle;
      this.txtRepayAmt.Location = new Point(102, 142);
      this.txtRepayAmt.MaxLength = 10;
      this.txtRepayAmt.Name = "txtRepayAmt";
      this.txtRepayAmt.Size = new Size(96, 20);
      this.txtRepayAmt.TabIndex = 19;
      this.txtRepayAmt.TextAlign = HorizontalAlignment.Right;
      this.txtRepayAmt.KeyPress += new KeyPressEventHandler(this.onDecimalFieldKeyPress);
      this.txtRepayAmt.KeyUp += new KeyEventHandler(this.onNumericFieldKeyUp);
      this.txtRepayAmt.Leave += new EventHandler(this.onNumericFieldLeave);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(26, 145);
      this.label6.Name = "label6";
      this.label6.Size = new Size(71, 13);
      this.label6.TabIndex = 31;
      this.label6.Text = "Repay Amt. $";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(26, 110);
      this.label4.Name = "label4";
      this.label4.Size = new Size(57, 13);
      this.label4.TabIndex = 26;
      this.label4.Text = "How many";
      this.chkRental.AutoSize = true;
      this.chkRental.Location = new Point(215, 57);
      this.chkRental.Name = "chkRental";
      this.chkRental.Size = new Size(136, 17);
      this.chkRental.TabIndex = 22;
      this.chkRental.Text = "Rental Payment History";
      this.chkRental.UseVisualStyleBackColor = true;
      this.chkRental.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkAgeCredit.AutoSize = true;
      this.chkAgeCredit.Location = new Point(10, 21);
      this.chkAgeCredit.Name = "chkAgeCredit";
      this.chkAgeCredit.Size = new Size(156, 17);
      this.chkAgeCredit.TabIndex = 11;
      this.chkAgeCredit.Text = "No. and Age of Credit Lines";
      this.chkAgeCredit.UseVisualStyleBackColor = true;
      this.chkAgeCredit.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkCollections.AutoSize = true;
      this.chkCollections.Location = new Point(215, 39);
      this.chkCollections.Name = "chkCollections";
      this.chkCollections.Size = new Size(77, 17);
      this.chkCollections.TabIndex = 21;
      this.chkCollections.Text = "Collections";
      this.chkCollections.UseVisualStyleBackColor = true;
      this.chkCollections.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkJudgements.AutoSize = true;
      this.chkJudgements.Location = new Point(10, 39);
      this.chkJudgements.Name = "chkJudgements";
      this.chkJudgements.Size = new Size(83, 17);
      this.chkJudgements.TabIndex = 12;
      this.chkJudgements.Text = "Judgements";
      this.chkJudgements.UseVisualStyleBackColor = true;
      this.chkJudgements.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkCreditOther.AutoSize = true;
      this.chkCreditOther.Location = new Point(10, 75);
      this.chkCreditOther.Name = "chkCreditOther";
      this.chkCreditOther.Size = new Size(55, 17);
      this.chkCreditOther.TabIndex = 14;
      this.chkCreditOther.Text = "Other:";
      this.chkCreditOther.UseVisualStyleBackColor = true;
      this.chkCreditOther.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.txtCreditOther.BorderStyle = BorderStyle.FixedSingle;
      this.txtCreditOther.Location = new Point(67, 73);
      this.txtCreditOther.Name = "txtCreditOther";
      this.txtCreditOther.Size = new Size(133, 20);
      this.txtCreditOther.TabIndex = 15;
      this.chkPaymentHistory.AutoSize = true;
      this.chkPaymentHistory.Location = new Point(215, 21);
      this.chkPaymentHistory.Name = "chkPaymentHistory";
      this.chkPaymentHistory.Size = new Size(102, 17);
      this.chkPaymentHistory.TabIndex = 20;
      this.chkPaymentHistory.Text = "Payment History";
      this.chkPaymentHistory.UseVisualStyleBackColor = true;
      this.chkPaymentHistory.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkBackruptcies.AutoSize = true;
      this.chkBackruptcies.Location = new Point(10, 57);
      this.chkBackruptcies.Name = "chkBackruptcies";
      this.chkBackruptcies.Size = new Size(88, 17);
      this.chkBackruptcies.TabIndex = 13;
      this.chkBackruptcies.Text = "Backruptcies";
      this.chkBackruptcies.UseVisualStyleBackColor = true;
      this.chkBackruptcies.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(7, 5);
      this.label2.Name = "label2";
      this.label2.Size = new Size(83, 13);
      this.label2.TabIndex = 10;
      this.label2.Text = "Credit History";
      this.panelEmployment.BackColor = Color.White;
      this.panelEmployment.Controls.Add((Control) this.label1);
      this.panelEmployment.Controls.Add((Control) this.chkChildSupport);
      this.panelEmployment.Controls.Add((Control) this.chkInstallment);
      this.panelEmployment.Controls.Add((Control) this.chkRequiredEscrow);
      this.panelEmployment.Controls.Add((Control) this.chkSimultaneous);
      this.panelEmployment.Controls.Add((Control) this.chkRealEstate);
      this.panelEmployment.Controls.Add((Control) this.chkObOther);
      this.panelEmployment.Controls.Add((Control) this.txtOBOther);
      this.panelEmployment.Controls.Add((Control) this.chkRevolving);
      this.panelEmployment.Controls.Add((Control) this.chkAlimony);
      this.panelEmployment.Controls.Add((Control) this.chkHousing);
      this.panelEmployment.Dock = DockStyle.Top;
      this.panelEmployment.Location = new Point(1, 25);
      this.panelEmployment.Name = "panelEmployment";
      this.panelEmployment.Size = new Size(648, 115);
      this.panelEmployment.TabIndex = 13;
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(7, 5);
      this.label1.Name = "label1";
      this.label1.Size = new Size(118, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Monthly Obligations";
      this.chkChildSupport.AutoSize = true;
      this.chkChildSupport.Location = new Point(215, 57);
      this.chkChildSupport.Name = "chkChildSupport";
      this.chkChildSupport.Size = new Size(89, 17);
      this.chkChildSupport.TabIndex = 9;
      this.chkChildSupport.Text = "Child Support";
      this.chkChildSupport.UseVisualStyleBackColor = true;
      this.chkChildSupport.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkInstallment.AutoSize = true;
      this.chkInstallment.Location = new Point(10, 21);
      this.chkInstallment.Name = "chkInstallment";
      this.chkInstallment.Size = new Size(108, 17);
      this.chkInstallment.TabIndex = 1;
      this.chkInstallment.Text = "Installment Loans";
      this.chkInstallment.UseVisualStyleBackColor = true;
      this.chkInstallment.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkRequiredEscrow.AutoSize = true;
      this.chkRequiredEscrow.Location = new Point(215, 75);
      this.chkRequiredEscrow.Name = "chkRequiredEscrow";
      this.chkRequiredEscrow.Size = new Size(262, 17);
      this.chkRequiredEscrow.TabIndex = 10;
      this.chkRequiredEscrow.Text = "Required Escrows (i.e. Taxes, Ins, MI, HOA Dues)";
      this.chkRequiredEscrow.UseVisualStyleBackColor = true;
      this.chkRequiredEscrow.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkSimultaneous.AutoSize = true;
      this.chkSimultaneous.Location = new Point(215, 39);
      this.chkSimultaneous.Name = "chkSimultaneous";
      this.chkSimultaneous.Size = new Size(178, 17);
      this.chkSimultaneous.TabIndex = 8;
      this.chkSimultaneous.Text = "Simultaneous Loans on Property";
      this.chkSimultaneous.UseVisualStyleBackColor = true;
      this.chkSimultaneous.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkRealEstate.AutoSize = true;
      this.chkRealEstate.Location = new Point(10, 39);
      this.chkRealEstate.Name = "chkRealEstate";
      this.chkRealEstate.Size = new Size(113, 17);
      this.chkRealEstate.TabIndex = 2;
      this.chkRealEstate.Text = "Real Estate Loans";
      this.chkRealEstate.UseVisualStyleBackColor = true;
      this.chkRealEstate.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkObOther.AutoSize = true;
      this.chkObOther.Location = new Point(10, 93);
      this.chkObOther.Name = "chkObOther";
      this.chkObOther.Size = new Size(55, 17);
      this.chkObOther.TabIndex = 5;
      this.chkObOther.Text = "Other:";
      this.chkObOther.UseVisualStyleBackColor = true;
      this.chkObOther.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.txtOBOther.BorderStyle = BorderStyle.FixedSingle;
      this.txtOBOther.Location = new Point(68, 91);
      this.txtOBOther.Name = "txtOBOther";
      this.txtOBOther.Size = new Size(133, 20);
      this.txtOBOther.TabIndex = 6;
      this.chkRevolving.AutoSize = true;
      this.chkRevolving.Location = new Point(215, 21);
      this.chkRevolving.Name = "chkRevolving";
      this.chkRevolving.Size = new Size(159, 17);
      this.chkRevolving.TabIndex = 7;
      this.chkRevolving.Text = "Revolving Charge Accounts";
      this.chkRevolving.UseVisualStyleBackColor = true;
      this.chkRevolving.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkAlimony.AutoSize = true;
      this.chkAlimony.Location = new Point(10, 57);
      this.chkAlimony.Name = "chkAlimony";
      this.chkAlimony.Size = new Size(129, 17);
      this.chkAlimony.TabIndex = 3;
      this.chkAlimony.Text = "Alimony/Maintenance";
      this.chkAlimony.UseVisualStyleBackColor = true;
      this.chkAlimony.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkHousing.AutoSize = true;
      this.chkHousing.Location = new Point(10, 75);
      this.chkHousing.Name = "chkHousing";
      this.chkHousing.Size = new Size(180, 17);
      this.chkHousing.TabIndex = 4;
      this.chkHousing.Text = "Monthly Housing Expense [P & I]";
      this.chkHousing.UseMnemonic = false;
      this.chkHousing.UseVisualStyleBackColor = true;
      this.chkHousing.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerBor);
      this.Name = nameof (VerificationObligationControl);
      this.Size = new Size(650, 314);
      this.groupContainerBor.ResumeLayout(false);
      this.panelNonEmployment.ResumeLayout(false);
      this.panelNonEmployment.PerformLayout();
      this.panelEmployment.ResumeLayout(false);
      this.panelEmployment.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
