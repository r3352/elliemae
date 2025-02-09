// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationCreditHistoryDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI.Controls;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddVerificationCreditHistoryDialog : Form
  {
    private List<DocumentVerificationObligation> verificationList = new List<DocumentVerificationObligation>();
    private IContainer components;
    private ToolTip tooltip;
    private Panel pnlClose;
    private Button btnAdd;
    private Button btnCancel;
    private Label lblType;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private CheckBox chkCollectionsBorrower;
    private CheckBox chkDebtBorrower;
    private CheckBox chkRentalPaymentsBorrower;
    private CheckBox chkPaymentsBorrower;
    private CheckBox chkBankruptciesBorrower;
    private CheckBox chkJudgementsBorrower;
    private CheckBox chkCreditLinesBorrower;
    private CheckBox chkCollectionsCoBorrower;
    private CheckBox chkDebtCoBorrower;
    private CheckBox chkRentalPaymentsCoBorrower;
    private CheckBox chkPaymentsCoBorrower;
    private CheckBox chkBankruptciesCoBorrower;
    private CheckBox chkJudgementsCoBorrower;
    private CheckBox chkCreditLinesCoBorrower;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private EMHelpLink helpLink;
    private Label label10;
    private Label label11;
    private CheckBox chkLatesCoBorrower;
    private CheckBox chkOtherCoBorrower;
    private CheckBox chkLatesBorrower;
    private CheckBox chkOtherBorrower;
    private TextBox txtOther;
    private TextBox txtLates;
    private TextBox txtHELOC;
    private TextBox txt2ndLien;
    private Label label12;
    private Label label13;
    private CheckBox chkHELOCCoBorrower;
    private CheckBox chk2ndLienCoBorrower;
    private CheckBox chkHELOCBorrower;
    private CheckBox chk2ndLienBorrower;

    public AddVerificationCreditHistoryDialog() => this.InitializeComponent();

    public DocumentVerificationObligation[] Verifications => this.verificationList.ToArray();

    private void chkOther_CheckedChanged(object sender, EventArgs e)
    {
      this.txtOther.Enabled = this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked;
    }

    private void chkLates_CheckedChanged(object sender, EventArgs e)
    {
      this.txtLates.Enabled = this.chkLatesBorrower.Checked || this.chkLatesCoBorrower.Checked;
    }

    private void chk2ndLien_CheckedChanged(object sender, EventArgs e)
    {
      this.txt2ndLien.Enabled = this.chk2ndLienBorrower.Checked || this.chk2ndLienCoBorrower.Checked;
    }

    private void chkHELOC_CheckedChanged(object sender, EventArgs e)
    {
      this.txtHELOC.Enabled = this.chkHELOCBorrower.Checked || this.chkHELOCCoBorrower.Checked;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!this.validate())
        return;
      this.tryAddVerification(ObligationType.NoAndAgeOfCreditLine, this.chkCreditLinesBorrower.Checked, this.chkCreditLinesCoBorrower.Checked);
      this.tryAddVerification(ObligationType.Judgement, this.chkJudgementsBorrower.Checked, this.chkJudgementsCoBorrower.Checked);
      this.tryAddVerification(ObligationType.Backruptcy, this.chkBankruptciesBorrower.Checked, this.chkBankruptciesCoBorrower.Checked);
      this.tryAddVerification(ObligationType.PaymentHistory, this.chkPaymentsBorrower.Checked, this.chkPaymentsCoBorrower.Checked);
      this.tryAddVerification(ObligationType.Collection, this.chkCollectionsBorrower.Checked, this.chkCollectionsCoBorrower.Checked);
      this.tryAddVerification(ObligationType.RentalPaymentHistory, this.chkRentalPaymentsBorrower.Checked, this.chkRentalPaymentsCoBorrower.Checked);
      this.tryAddVerification(ObligationType.DebtObligationCurrent, this.chkDebtBorrower.Checked, this.chkDebtCoBorrower.Checked);
      this.tryAddVerification(ObligationType.OtherCreditHistory, this.chkOtherBorrower.Checked, this.chkOtherCoBorrower.Checked);
      this.tryAddVerification(ObligationType.MortgageLate, this.chkLatesBorrower.Checked, this.chkLatesCoBorrower.Checked);
      this.tryAddVerification(ObligationType.SecondLien, this.chk2ndLienBorrower.Checked, this.chk2ndLienCoBorrower.Checked);
      this.tryAddVerification(ObligationType.HELOC, this.chkHELOCBorrower.Checked, this.chkHELOCCoBorrower.Checked);
      this.DialogResult = DialogResult.OK;
    }

    private bool validate()
    {
      if ((this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked) && string.IsNullOrEmpty(this.txtOther.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a description for 'Other'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      if ((this.chkLatesBorrower.Checked || this.chkLatesCoBorrower.Checked) && string.IsNullOrEmpty(this.txtLates.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must specify How Many 'Mortgage Lates'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        return false;
      }
      int result;
      if (this.chk2ndLienBorrower.Checked || this.chk2ndLienCoBorrower.Checked)
      {
        if (string.IsNullOrEmpty(this.txt2ndLien.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter How Much for '2nd Lien'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (!int.TryParse(this.txt2ndLien.Text, out result))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "How Much for '2nd Lien' must be a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      if (this.chkHELOCBorrower.Checked || this.chkHELOCCoBorrower.Checked)
      {
        if (string.IsNullOrEmpty(this.txtHELOC.Text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a HELOC Repay Amt.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
        if (!int.TryParse(this.txtHELOC.Text, out result))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "HELOC Repay Amt. must be a valid number.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return false;
        }
      }
      return true;
    }

    private void tryAddVerification(
      ObligationType obligationType,
      bool borrowerChecked,
      bool coBorrowerChecked)
    {
      if (!borrowerChecked && !coBorrowerChecked)
        return;
      LoanBorrowerType borrowerType = LoanBorrowerType.None;
      if (borrowerChecked & coBorrowerChecked)
      {
        borrowerType = LoanBorrowerType.Both;
      }
      else
      {
        if (borrowerChecked)
          borrowerType = LoanBorrowerType.Borrower;
        if (coBorrowerChecked)
          borrowerType = LoanBorrowerType.Coborrower;
      }
      if (borrowerType == LoanBorrowerType.None)
        return;
      DocumentVerificationObligation verificationObligation = new DocumentVerificationObligation(borrowerType);
      verificationObligation.ObligationType = obligationType;
      if (obligationType == ObligationType.OtherCreditHistory)
        verificationObligation.OtherDescription = this.txtOther.Text;
      if (obligationType == ObligationType.MortgageLate)
        verificationObligation.MortageLateCount = this.txtLates.Text;
      if (obligationType == ObligationType.SecondLien)
        verificationObligation.Amount = (Decimal) Convert.ToInt32(this.txt2ndLien.Text);
      if (obligationType == ObligationType.HELOC)
        verificationObligation.Amount = (Decimal) Convert.ToInt32(this.txtHELOC.Text);
      this.verificationList.Add(verificationObligation);
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
      this.tooltip = new ToolTip(this.components);
      this.pnlClose = new Panel();
      this.helpLink = new EMHelpLink();
      this.btnAdd = new Button();
      this.btnCancel = new Button();
      this.lblType = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.chkCollectionsBorrower = new CheckBox();
      this.chkDebtBorrower = new CheckBox();
      this.chkRentalPaymentsBorrower = new CheckBox();
      this.chkPaymentsBorrower = new CheckBox();
      this.chkBankruptciesBorrower = new CheckBox();
      this.chkJudgementsBorrower = new CheckBox();
      this.chkCreditLinesBorrower = new CheckBox();
      this.chkCollectionsCoBorrower = new CheckBox();
      this.chkDebtCoBorrower = new CheckBox();
      this.chkRentalPaymentsCoBorrower = new CheckBox();
      this.chkPaymentsCoBorrower = new CheckBox();
      this.chkBankruptciesCoBorrower = new CheckBox();
      this.chkJudgementsCoBorrower = new CheckBox();
      this.chkCreditLinesCoBorrower = new CheckBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.chkLatesCoBorrower = new CheckBox();
      this.chkOtherCoBorrower = new CheckBox();
      this.chkLatesBorrower = new CheckBox();
      this.chkOtherBorrower = new CheckBox();
      this.txtOther = new TextBox();
      this.txtLates = new TextBox();
      this.txtHELOC = new TextBox();
      this.txt2ndLien = new TextBox();
      this.label12 = new Label();
      this.label13 = new Label();
      this.chkHELOCCoBorrower = new CheckBox();
      this.chk2ndLienCoBorrower = new CheckBox();
      this.chkHELOCBorrower = new CheckBox();
      this.chk2ndLienBorrower = new CheckBox();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 320);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(457, 51);
      this.pnlClose.TabIndex = 4;
      this.helpLink.BackColor = Color.Transparent;
      this.helpLink.Cursor = Cursors.Hand;
      this.helpLink.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.helpLink.HelpTag = "Document Manager";
      this.helpLink.Location = new Point(12, 21);
      this.helpLink.Name = "helpLink";
      this.helpLink.Size = new Size(90, 18);
      this.helpLink.TabIndex = 15;
      this.helpLink.TabStop = false;
      this.btnAdd.Location = new Point(269, 21);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 5;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(350, 21);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblType.AutoSize = true;
      this.lblType.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblType.Location = new Point(12, 18);
      this.lblType.Name = "lblType";
      this.lblType.Size = new Size(83, 14);
      this.lblType.TabIndex = 34;
      this.lblType.Text = "Credit History";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(242, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 14);
      this.label1.TabIndex = 35;
      this.label1.Text = "For Borrower";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(335, 18);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 14);
      this.label2.TabIndex = 36;
      this.label2.Text = "For Co-Borrower";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 42);
      this.label3.Name = "label3";
      this.label3.Size = new Size(139, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "No. and Age of Credit Lines";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 66);
      this.label4.Name = "label4";
      this.label4.Size = new Size(65, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Judgements";
      this.chkCollectionsBorrower.AutoSize = true;
      this.chkCollectionsBorrower.Location = new Point(277, 138);
      this.chkCollectionsBorrower.Name = "chkCollectionsBorrower";
      this.chkCollectionsBorrower.Size = new Size(15, 14);
      this.chkCollectionsBorrower.TabIndex = 120;
      this.chkCollectionsBorrower.UseVisualStyleBackColor = true;
      this.chkDebtBorrower.AutoSize = true;
      this.chkDebtBorrower.Location = new Point(277, 186);
      this.chkDebtBorrower.Name = "chkDebtBorrower";
      this.chkDebtBorrower.Size = new Size(15, 14);
      this.chkDebtBorrower.TabIndex = 119;
      this.chkDebtBorrower.UseVisualStyleBackColor = true;
      this.chkRentalPaymentsBorrower.AutoSize = true;
      this.chkRentalPaymentsBorrower.Location = new Point(277, 162);
      this.chkRentalPaymentsBorrower.Name = "chkRentalPaymentsBorrower";
      this.chkRentalPaymentsBorrower.Size = new Size(15, 14);
      this.chkRentalPaymentsBorrower.TabIndex = 118;
      this.chkRentalPaymentsBorrower.UseVisualStyleBackColor = true;
      this.chkPaymentsBorrower.AutoSize = true;
      this.chkPaymentsBorrower.Location = new Point(277, 114);
      this.chkPaymentsBorrower.Name = "chkPaymentsBorrower";
      this.chkPaymentsBorrower.Size = new Size(15, 14);
      this.chkPaymentsBorrower.TabIndex = 117;
      this.chkPaymentsBorrower.UseVisualStyleBackColor = true;
      this.chkBankruptciesBorrower.AutoSize = true;
      this.chkBankruptciesBorrower.Location = new Point(277, 90);
      this.chkBankruptciesBorrower.Name = "chkBankruptciesBorrower";
      this.chkBankruptciesBorrower.Size = new Size(15, 14);
      this.chkBankruptciesBorrower.TabIndex = 116;
      this.chkBankruptciesBorrower.UseVisualStyleBackColor = true;
      this.chkJudgementsBorrower.AutoSize = true;
      this.chkJudgementsBorrower.Location = new Point(277, 66);
      this.chkJudgementsBorrower.Name = "chkJudgementsBorrower";
      this.chkJudgementsBorrower.Size = new Size(15, 14);
      this.chkJudgementsBorrower.TabIndex = 115;
      this.chkJudgementsBorrower.UseVisualStyleBackColor = true;
      this.chkCreditLinesBorrower.AutoSize = true;
      this.chkCreditLinesBorrower.Location = new Point(277, 42);
      this.chkCreditLinesBorrower.Name = "chkCreditLinesBorrower";
      this.chkCreditLinesBorrower.Size = new Size(15, 14);
      this.chkCreditLinesBorrower.TabIndex = 114;
      this.chkCreditLinesBorrower.UseVisualStyleBackColor = true;
      this.chkCollectionsCoBorrower.AutoSize = true;
      this.chkCollectionsCoBorrower.Location = new Point(383, 139);
      this.chkCollectionsCoBorrower.Name = "chkCollectionsCoBorrower";
      this.chkCollectionsCoBorrower.Size = new Size(15, 14);
      this.chkCollectionsCoBorrower.TabIndex = (int) sbyte.MaxValue;
      this.chkCollectionsCoBorrower.UseVisualStyleBackColor = true;
      this.chkDebtCoBorrower.AutoSize = true;
      this.chkDebtCoBorrower.Location = new Point(383, 187);
      this.chkDebtCoBorrower.Name = "chkDebtCoBorrower";
      this.chkDebtCoBorrower.Size = new Size(15, 14);
      this.chkDebtCoBorrower.TabIndex = 126;
      this.chkDebtCoBorrower.UseVisualStyleBackColor = true;
      this.chkRentalPaymentsCoBorrower.AutoSize = true;
      this.chkRentalPaymentsCoBorrower.Location = new Point(383, 163);
      this.chkRentalPaymentsCoBorrower.Name = "chkRentalPaymentsCoBorrower";
      this.chkRentalPaymentsCoBorrower.Size = new Size(15, 14);
      this.chkRentalPaymentsCoBorrower.TabIndex = 125;
      this.chkRentalPaymentsCoBorrower.UseVisualStyleBackColor = true;
      this.chkPaymentsCoBorrower.AutoSize = true;
      this.chkPaymentsCoBorrower.Location = new Point(383, 115);
      this.chkPaymentsCoBorrower.Name = "chkPaymentsCoBorrower";
      this.chkPaymentsCoBorrower.Size = new Size(15, 14);
      this.chkPaymentsCoBorrower.TabIndex = 124;
      this.chkPaymentsCoBorrower.UseVisualStyleBackColor = true;
      this.chkBankruptciesCoBorrower.AutoSize = true;
      this.chkBankruptciesCoBorrower.Location = new Point(383, 91);
      this.chkBankruptciesCoBorrower.Name = "chkBankruptciesCoBorrower";
      this.chkBankruptciesCoBorrower.Size = new Size(15, 14);
      this.chkBankruptciesCoBorrower.TabIndex = 123;
      this.chkBankruptciesCoBorrower.UseVisualStyleBackColor = true;
      this.chkJudgementsCoBorrower.AutoSize = true;
      this.chkJudgementsCoBorrower.Location = new Point(383, 67);
      this.chkJudgementsCoBorrower.Name = "chkJudgementsCoBorrower";
      this.chkJudgementsCoBorrower.Size = new Size(15, 14);
      this.chkJudgementsCoBorrower.TabIndex = 122;
      this.chkJudgementsCoBorrower.UseVisualStyleBackColor = true;
      this.chkCreditLinesCoBorrower.AutoSize = true;
      this.chkCreditLinesCoBorrower.Location = new Point(383, 43);
      this.chkCreditLinesCoBorrower.Name = "chkCreditLinesCoBorrower";
      this.chkCreditLinesCoBorrower.Size = new Size(15, 14);
      this.chkCreditLinesCoBorrower.TabIndex = 121;
      this.chkCreditLinesCoBorrower.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 115);
      this.label5.Name = "label5";
      this.label5.Size = new Size(85, 14);
      this.label5.TabIndex = 129;
      this.label5.Text = "Payment History";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 91);
      this.label6.Name = "label6";
      this.label6.Size = new Size(70, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "Bankruptcies";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 187);
      this.label7.Name = "label7";
      this.label7.Size = new Size(124, 14);
      this.label7.TabIndex = 132;
      this.label7.Text = "Debt Obligations Current";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 163);
      this.label8.Name = "label8";
      this.label8.Size = new Size(118, 14);
      this.label8.TabIndex = 131;
      this.label8.Text = "Rental Payment History";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(12, 138);
      this.label9.Name = "label9";
      this.label9.Size = new Size(59, 14);
      this.label9.TabIndex = 130;
      this.label9.Text = "Collections";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 234);
      this.label10.Name = "label10";
      this.label10.Size = new Size(140, 14);
      this.label10.TabIndex = 138;
      this.label10.Text = "Mortgage Lates: How Many";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 210);
      this.label11.Name = "label11";
      this.label11.Size = new Size(34, 14);
      this.label11.TabIndex = 137;
      this.label11.Text = "Other";
      this.chkLatesCoBorrower.AutoSize = true;
      this.chkLatesCoBorrower.Location = new Point(383, 234);
      this.chkLatesCoBorrower.Name = "chkLatesCoBorrower";
      this.chkLatesCoBorrower.Size = new Size(15, 14);
      this.chkLatesCoBorrower.TabIndex = 136;
      this.chkLatesCoBorrower.UseVisualStyleBackColor = true;
      this.chkLatesCoBorrower.CheckedChanged += new EventHandler(this.chkLates_CheckedChanged);
      this.chkOtherCoBorrower.AutoSize = true;
      this.chkOtherCoBorrower.Location = new Point(383, 210);
      this.chkOtherCoBorrower.Name = "chkOtherCoBorrower";
      this.chkOtherCoBorrower.Size = new Size(15, 14);
      this.chkOtherCoBorrower.TabIndex = 135;
      this.chkOtherCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherCoBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkLatesBorrower.AutoSize = true;
      this.chkLatesBorrower.Location = new Point(277, 233);
      this.chkLatesBorrower.Name = "chkLatesBorrower";
      this.chkLatesBorrower.Size = new Size(15, 14);
      this.chkLatesBorrower.TabIndex = 134;
      this.chkLatesBorrower.UseVisualStyleBackColor = true;
      this.chkLatesBorrower.CheckedChanged += new EventHandler(this.chkLates_CheckedChanged);
      this.chkOtherBorrower.AutoSize = true;
      this.chkOtherBorrower.Location = new Point(277, 209);
      this.chkOtherBorrower.Name = "chkOtherBorrower";
      this.chkOtherBorrower.Size = new Size(15, 14);
      this.chkOtherBorrower.TabIndex = 133;
      this.chkOtherBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.txtOther.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOther.Enabled = false;
      this.txtOther.Location = new Point(52, 207);
      this.txtOther.Name = "txtOther";
      this.txtOther.Size = new Size(205, 20);
      this.txtOther.TabIndex = 139;
      this.txtLates.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtLates.Enabled = false;
      this.txtLates.Location = new Point(158, 231);
      this.txtLates.Name = "txtLates";
      this.txtLates.Size = new Size(99, 20);
      this.txtLates.TabIndex = 140;
      this.txtHELOC.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtHELOC.Enabled = false;
      this.txtHELOC.Location = new Point(133, 281);
      this.txtHELOC.Name = "txtHELOC";
      this.txtHELOC.Size = new Size(124, 20);
      this.txtHELOC.TabIndex = 148;
      this.txt2ndLien.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txt2ndLien.Enabled = false;
      this.txt2ndLien.Location = new Point(133, 257);
      this.txt2ndLien.Name = "txt2ndLien";
      this.txt2ndLien.Size = new Size(124, 20);
      this.txt2ndLien.TabIndex = 147;
      this.label12.AutoSize = true;
      this.label12.Location = new Point(12, 284);
      this.label12.Name = "label12";
      this.label12.Size = new Size(111, 14);
      this.label12.TabIndex = 146;
      this.label12.Text = "HELOC: Repay Amt. $";
      this.label13.AutoSize = true;
      this.label13.Location = new Point(12, 260);
      this.label13.Name = "label13";
      this.label13.Size = new Size(115, 14);
      this.label13.TabIndex = 145;
      this.label13.Text = "2nd Lien: How Much $";
      this.chkHELOCCoBorrower.AutoSize = true;
      this.chkHELOCCoBorrower.Location = new Point(383, 284);
      this.chkHELOCCoBorrower.Name = "chkHELOCCoBorrower";
      this.chkHELOCCoBorrower.Size = new Size(15, 14);
      this.chkHELOCCoBorrower.TabIndex = 144;
      this.chkHELOCCoBorrower.UseVisualStyleBackColor = true;
      this.chkHELOCCoBorrower.CheckStateChanged += new EventHandler(this.chkHELOC_CheckedChanged);
      this.chk2ndLienCoBorrower.AutoSize = true;
      this.chk2ndLienCoBorrower.Location = new Point(383, 260);
      this.chk2ndLienCoBorrower.Name = "chk2ndLienCoBorrower";
      this.chk2ndLienCoBorrower.Size = new Size(15, 14);
      this.chk2ndLienCoBorrower.TabIndex = 143;
      this.chk2ndLienCoBorrower.UseVisualStyleBackColor = true;
      this.chk2ndLienCoBorrower.CheckedChanged += new EventHandler(this.chk2ndLien_CheckedChanged);
      this.chkHELOCBorrower.AutoSize = true;
      this.chkHELOCBorrower.Location = new Point(277, 283);
      this.chkHELOCBorrower.Name = "chkHELOCBorrower";
      this.chkHELOCBorrower.Size = new Size(15, 14);
      this.chkHELOCBorrower.TabIndex = 142;
      this.chkHELOCBorrower.UseVisualStyleBackColor = true;
      this.chkHELOCBorrower.CheckedChanged += new EventHandler(this.chkHELOC_CheckedChanged);
      this.chk2ndLienBorrower.AutoSize = true;
      this.chk2ndLienBorrower.Location = new Point(277, 259);
      this.chk2ndLienBorrower.Name = "chk2ndLienBorrower";
      this.chk2ndLienBorrower.Size = new Size(15, 14);
      this.chk2ndLienBorrower.TabIndex = 141;
      this.chk2ndLienBorrower.UseVisualStyleBackColor = true;
      this.chk2ndLienBorrower.CheckedChanged += new EventHandler(this.chk2ndLien_CheckedChanged);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(457, 371);
      this.Controls.Add((Control) this.txtHELOC);
      this.Controls.Add((Control) this.txt2ndLien);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.label13);
      this.Controls.Add((Control) this.chkHELOCCoBorrower);
      this.Controls.Add((Control) this.chk2ndLienCoBorrower);
      this.Controls.Add((Control) this.chkHELOCBorrower);
      this.Controls.Add((Control) this.chk2ndLienBorrower);
      this.Controls.Add((Control) this.txtLates);
      this.Controls.Add((Control) this.txtOther);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.chkLatesCoBorrower);
      this.Controls.Add((Control) this.chkOtherCoBorrower);
      this.Controls.Add((Control) this.chkLatesBorrower);
      this.Controls.Add((Control) this.chkOtherBorrower);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkCollectionsCoBorrower);
      this.Controls.Add((Control) this.chkDebtCoBorrower);
      this.Controls.Add((Control) this.chkRentalPaymentsCoBorrower);
      this.Controls.Add((Control) this.chkPaymentsCoBorrower);
      this.Controls.Add((Control) this.chkBankruptciesCoBorrower);
      this.Controls.Add((Control) this.chkJudgementsCoBorrower);
      this.Controls.Add((Control) this.chkCreditLinesCoBorrower);
      this.Controls.Add((Control) this.chkCollectionsBorrower);
      this.Controls.Add((Control) this.chkDebtBorrower);
      this.Controls.Add((Control) this.chkRentalPaymentsBorrower);
      this.Controls.Add((Control) this.chkPaymentsBorrower);
      this.Controls.Add((Control) this.chkBankruptciesBorrower);
      this.Controls.Add((Control) this.chkJudgementsBorrower);
      this.Controls.Add((Control) this.chkCreditLinesBorrower);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblType);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddVerificationCreditHistoryDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Monthly Obligation Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
