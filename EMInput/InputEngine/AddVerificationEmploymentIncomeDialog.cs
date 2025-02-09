// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationEmploymentIncomeDialog
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class AddVerificationEmploymentIncomeDialog : Form
  {
    private List<DocumentVerificationIncome> verificationList = new List<DocumentVerificationIncome>();
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
    private CheckBox chkSocialSecurityBorrower;
    private CheckBox chkPensionBorrower;
    private CheckBox chk401KBorrower;
    private CheckBox chk1099Borrower;
    private CheckBox chkW2Borrower;
    private CheckBox chkTaxReturnBorrower;
    private CheckBox chkPaystubsBorrower;
    private CheckBox chkSocialSecurityCoBorrower;
    private CheckBox chkPensionCoBorrower;
    private CheckBox chk401KCoBorrower;
    private CheckBox chk1099CoBorrower;
    private CheckBox chkW2CoBorrower;
    private CheckBox chkTaxReturnCoBorrower;
    private CheckBox chkPaystubsCoBorrower;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private EMHelpLink helpLink;
    private Label label10;
    private Label label11;
    private CheckBox chkOtherCoBorrower;
    private CheckBox chkMilitaryCoBorrower;
    private CheckBox chkOtherBorrower;
    private CheckBox chkMilitaryBorrower;
    private TextBox txtOther;
    private Label label12;
    private TextBox txtTaxReturnYear;

    public AddVerificationEmploymentIncomeDialog() => this.InitializeComponent();

    public DocumentVerificationIncome[] Verifications => this.verificationList.ToArray();

    private void chkTaxReturnYear_CheckedChanged(object sender, EventArgs e)
    {
      this.txtTaxReturnYear.Enabled = this.chkTaxReturnBorrower.Checked || this.chkTaxReturnCoBorrower.Checked;
    }

    private void chkOther_CheckedChanged(object sender, EventArgs e)
    {
      this.txtOther.Enabled = this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (this.chkTaxReturnBorrower.Checked || this.chkTaxReturnCoBorrower.Checked)
      {
        if (this.txtTaxReturnYear.Text.Trim().Length != 4)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a valid 'Tax Return Year' (YYYY).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
        if (!Regex.IsMatch(this.txtTaxReturnYear.Text, "^(19|20)[0-9][0-9]"))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a valid 'Tax Return Year' (YYYY).", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
          return;
        }
      }
      if ((this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked) && string.IsNullOrEmpty(this.txtOther.Text))
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "You must enter a description for 'Other'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.tryAddVerification(IncomeType.Paystub, this.chkPaystubsBorrower.Checked, this.chkPaystubsCoBorrower.Checked);
        this.tryAddVerification(IncomeType.TaxReturn, this.chkTaxReturnBorrower.Checked, this.chkTaxReturnCoBorrower.Checked);
        this.tryAddVerification(IncomeType.W2, this.chkW2Borrower.Checked, this.chkW2CoBorrower.Checked);
        this.tryAddVerification(IncomeType.Ten99, this.chk1099Borrower.Checked, this.chk1099CoBorrower.Checked);
        this.tryAddVerification(IncomeType.SocialSecurity, this.chkSocialSecurityBorrower.Checked, this.chkSocialSecurityCoBorrower.Checked);
        this.tryAddVerification(IncomeType.Four01K, this.chk401KBorrower.Checked, this.chk401KCoBorrower.Checked);
        this.tryAddVerification(IncomeType.Pension, this.chkPensionBorrower.Checked, this.chkPensionCoBorrower.Checked);
        this.tryAddVerification(IncomeType.Military, this.chkMilitaryBorrower.Checked, this.chkMilitaryCoBorrower.Checked);
        this.tryAddVerification(IncomeType.OtherEmployment, this.chkOtherBorrower.Checked, this.chkOtherCoBorrower.Checked);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void tryAddVerification(
      IncomeType incomeType,
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
      DocumentVerificationIncome verificationIncome = new DocumentVerificationIncome(borrowerType);
      verificationIncome.IncomeType = incomeType;
      if (incomeType == IncomeType.TaxReturn)
        verificationIncome.TaxReturnYear = Convert.ToInt32(this.txtTaxReturnYear.Text);
      if (incomeType == IncomeType.OtherEmployment)
        verificationIncome.OtherDescription = this.txtOther.Text;
      this.verificationList.Add(verificationIncome);
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
      this.chkSocialSecurityBorrower = new CheckBox();
      this.chkPensionBorrower = new CheckBox();
      this.chk401KBorrower = new CheckBox();
      this.chk1099Borrower = new CheckBox();
      this.chkW2Borrower = new CheckBox();
      this.chkTaxReturnBorrower = new CheckBox();
      this.chkPaystubsBorrower = new CheckBox();
      this.chkSocialSecurityCoBorrower = new CheckBox();
      this.chkPensionCoBorrower = new CheckBox();
      this.chk401KCoBorrower = new CheckBox();
      this.chk1099CoBorrower = new CheckBox();
      this.chkW2CoBorrower = new CheckBox();
      this.chkTaxReturnCoBorrower = new CheckBox();
      this.chkPaystubsCoBorrower = new CheckBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.chkOtherCoBorrower = new CheckBox();
      this.chkMilitaryCoBorrower = new CheckBox();
      this.chkOtherBorrower = new CheckBox();
      this.chkMilitaryBorrower = new CheckBox();
      this.txtOther = new TextBox();
      this.label12 = new Label();
      this.txtTaxReturnYear = new TextBox();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 296);
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
      this.lblType.Size = new Size(76, 14);
      this.lblType.TabIndex = 34;
      this.lblType.Text = "Employment";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(244, 42);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 14);
      this.label1.TabIndex = 35;
      this.label1.Text = "For Borrower";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(337, 42);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 14);
      this.label2.TabIndex = 36;
      this.label2.Text = "For Co-Borrower";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 67);
      this.label3.Name = "label3";
      this.label3.Size = new Size(52, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "Paystubs";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 91);
      this.label4.Name = "label4";
      this.label4.Size = new Size(85, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Tax Return Year";
      this.chkSocialSecurityBorrower.AutoSize = true;
      this.chkSocialSecurityBorrower.Location = new Point(277, 163);
      this.chkSocialSecurityBorrower.Name = "chkSocialSecurityBorrower";
      this.chkSocialSecurityBorrower.Size = new Size(15, 14);
      this.chkSocialSecurityBorrower.TabIndex = 120;
      this.chkSocialSecurityBorrower.UseVisualStyleBackColor = true;
      this.chkPensionBorrower.AutoSize = true;
      this.chkPensionBorrower.Location = new Point(277, 211);
      this.chkPensionBorrower.Name = "chkPensionBorrower";
      this.chkPensionBorrower.Size = new Size(15, 14);
      this.chkPensionBorrower.TabIndex = 119;
      this.chkPensionBorrower.UseVisualStyleBackColor = true;
      this.chk401KBorrower.AutoSize = true;
      this.chk401KBorrower.Location = new Point(277, 187);
      this.chk401KBorrower.Name = "chk401KBorrower";
      this.chk401KBorrower.Size = new Size(15, 14);
      this.chk401KBorrower.TabIndex = 118;
      this.chk401KBorrower.UseVisualStyleBackColor = true;
      this.chk1099Borrower.AutoSize = true;
      this.chk1099Borrower.Location = new Point(277, 139);
      this.chk1099Borrower.Name = "chk1099Borrower";
      this.chk1099Borrower.Size = new Size(15, 14);
      this.chk1099Borrower.TabIndex = 117;
      this.chk1099Borrower.UseVisualStyleBackColor = true;
      this.chkW2Borrower.AutoSize = true;
      this.chkW2Borrower.Location = new Point(277, 115);
      this.chkW2Borrower.Name = "chkW2Borrower";
      this.chkW2Borrower.Size = new Size(15, 14);
      this.chkW2Borrower.TabIndex = 116;
      this.chkW2Borrower.UseVisualStyleBackColor = true;
      this.chkTaxReturnBorrower.AutoSize = true;
      this.chkTaxReturnBorrower.Location = new Point(277, 91);
      this.chkTaxReturnBorrower.Name = "chkTaxReturnBorrower";
      this.chkTaxReturnBorrower.Size = new Size(15, 14);
      this.chkTaxReturnBorrower.TabIndex = 115;
      this.chkTaxReturnBorrower.UseVisualStyleBackColor = true;
      this.chkTaxReturnBorrower.CheckedChanged += new EventHandler(this.chkTaxReturnYear_CheckedChanged);
      this.chkPaystubsBorrower.AutoSize = true;
      this.chkPaystubsBorrower.Location = new Point(277, 67);
      this.chkPaystubsBorrower.Name = "chkPaystubsBorrower";
      this.chkPaystubsBorrower.Size = new Size(15, 14);
      this.chkPaystubsBorrower.TabIndex = 114;
      this.chkPaystubsBorrower.UseVisualStyleBackColor = true;
      this.chkSocialSecurityCoBorrower.AutoSize = true;
      this.chkSocialSecurityCoBorrower.Location = new Point(383, 164);
      this.chkSocialSecurityCoBorrower.Name = "chkSocialSecurityCoBorrower";
      this.chkSocialSecurityCoBorrower.Size = new Size(15, 14);
      this.chkSocialSecurityCoBorrower.TabIndex = (int) sbyte.MaxValue;
      this.chkSocialSecurityCoBorrower.UseVisualStyleBackColor = true;
      this.chkPensionCoBorrower.AutoSize = true;
      this.chkPensionCoBorrower.Location = new Point(383, 212);
      this.chkPensionCoBorrower.Name = "chkPensionCoBorrower";
      this.chkPensionCoBorrower.Size = new Size(15, 14);
      this.chkPensionCoBorrower.TabIndex = 126;
      this.chkPensionCoBorrower.UseVisualStyleBackColor = true;
      this.chk401KCoBorrower.AutoSize = true;
      this.chk401KCoBorrower.Location = new Point(383, 188);
      this.chk401KCoBorrower.Name = "chk401KCoBorrower";
      this.chk401KCoBorrower.Size = new Size(15, 14);
      this.chk401KCoBorrower.TabIndex = 125;
      this.chk401KCoBorrower.UseVisualStyleBackColor = true;
      this.chk1099CoBorrower.AutoSize = true;
      this.chk1099CoBorrower.Location = new Point(383, 140);
      this.chk1099CoBorrower.Name = "chk1099CoBorrower";
      this.chk1099CoBorrower.Size = new Size(15, 14);
      this.chk1099CoBorrower.TabIndex = 124;
      this.chk1099CoBorrower.UseVisualStyleBackColor = true;
      this.chkW2CoBorrower.AutoSize = true;
      this.chkW2CoBorrower.Location = new Point(383, 116);
      this.chkW2CoBorrower.Name = "chkW2CoBorrower";
      this.chkW2CoBorrower.Size = new Size(15, 14);
      this.chkW2CoBorrower.TabIndex = 123;
      this.chkW2CoBorrower.UseVisualStyleBackColor = true;
      this.chkTaxReturnCoBorrower.AutoSize = true;
      this.chkTaxReturnCoBorrower.Location = new Point(383, 92);
      this.chkTaxReturnCoBorrower.Name = "chkTaxReturnCoBorrower";
      this.chkTaxReturnCoBorrower.Size = new Size(15, 14);
      this.chkTaxReturnCoBorrower.TabIndex = 122;
      this.chkTaxReturnCoBorrower.UseVisualStyleBackColor = true;
      this.chkTaxReturnCoBorrower.CheckedChanged += new EventHandler(this.chkTaxReturnYear_CheckedChanged);
      this.chkPaystubsCoBorrower.AutoSize = true;
      this.chkPaystubsCoBorrower.Location = new Point(383, 68);
      this.chkPaystubsCoBorrower.Name = "chkPaystubsCoBorrower";
      this.chkPaystubsCoBorrower.Size = new Size(15, 14);
      this.chkPaystubsCoBorrower.TabIndex = 121;
      this.chkPaystubsCoBorrower.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 140);
      this.label5.Name = "label5";
      this.label5.Size = new Size(31, 14);
      this.label5.TabIndex = 129;
      this.label5.Text = "1099";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 116);
      this.label6.Name = "label6";
      this.label6.Size = new Size(23, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "W2";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 212);
      this.label7.Name = "label7";
      this.label7.Size = new Size(45, 14);
      this.label7.TabIndex = 132;
      this.label7.Text = "Pension";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 188);
      this.label8.Name = "label8";
      this.label8.Size = new Size(32, 14);
      this.label8.TabIndex = 131;
      this.label8.Text = "401K";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(12, 163);
      this.label9.Name = "label9";
      this.label9.Size = new Size(79, 14);
      this.label9.TabIndex = 130;
      this.label9.Text = "Social Security";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 259);
      this.label10.Name = "label10";
      this.label10.Size = new Size(34, 14);
      this.label10.TabIndex = 138;
      this.label10.Text = "Other";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 235);
      this.label11.Name = "label11";
      this.label11.Size = new Size(40, 14);
      this.label11.TabIndex = 137;
      this.label11.Text = "Military";
      this.chkOtherCoBorrower.AutoSize = true;
      this.chkOtherCoBorrower.Location = new Point(383, 259);
      this.chkOtherCoBorrower.Name = "chkOtherCoBorrower";
      this.chkOtherCoBorrower.Size = new Size(15, 14);
      this.chkOtherCoBorrower.TabIndex = 136;
      this.chkOtherCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherCoBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkMilitaryCoBorrower.AutoSize = true;
      this.chkMilitaryCoBorrower.Location = new Point(383, 235);
      this.chkMilitaryCoBorrower.Name = "chkMilitaryCoBorrower";
      this.chkMilitaryCoBorrower.Size = new Size(15, 14);
      this.chkMilitaryCoBorrower.TabIndex = 135;
      this.chkMilitaryCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.AutoSize = true;
      this.chkOtherBorrower.Location = new Point(277, 258);
      this.chkOtherBorrower.Name = "chkOtherBorrower";
      this.chkOtherBorrower.Size = new Size(15, 14);
      this.chkOtherBorrower.TabIndex = 134;
      this.chkOtherBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkMilitaryBorrower.AutoSize = true;
      this.chkMilitaryBorrower.Location = new Point(277, 234);
      this.chkMilitaryBorrower.Name = "chkMilitaryBorrower";
      this.chkMilitaryBorrower.Size = new Size(15, 14);
      this.chkMilitaryBorrower.TabIndex = 133;
      this.chkMilitaryBorrower.UseVisualStyleBackColor = true;
      this.txtOther.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOther.Enabled = false;
      this.txtOther.Location = new Point(52, 256);
      this.txtOther.Name = "txtOther";
      this.txtOther.Size = new Size(169, 20);
      this.txtOther.TabIndex = 139;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(12, 42);
      this.label12.Name = "label12";
      this.label12.Size = new Size(179, 14);
      this.label12.TabIndex = 140;
      this.label12.Text = "Income Supporting Information";
      this.txtTaxReturnYear.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtTaxReturnYear.Enabled = false;
      this.txtTaxReturnYear.Location = new Point(103, 88);
      this.txtTaxReturnYear.Name = "txtTaxReturnYear";
      this.txtTaxReturnYear.Size = new Size(118, 20);
      this.txtTaxReturnYear.TabIndex = 141;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(457, 347);
      this.Controls.Add((Control) this.txtTaxReturnYear);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.txtOther);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.chkOtherCoBorrower);
      this.Controls.Add((Control) this.chkMilitaryCoBorrower);
      this.Controls.Add((Control) this.chkOtherBorrower);
      this.Controls.Add((Control) this.chkMilitaryBorrower);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkSocialSecurityCoBorrower);
      this.Controls.Add((Control) this.chkPensionCoBorrower);
      this.Controls.Add((Control) this.chk401KCoBorrower);
      this.Controls.Add((Control) this.chk1099CoBorrower);
      this.Controls.Add((Control) this.chkW2CoBorrower);
      this.Controls.Add((Control) this.chkTaxReturnCoBorrower);
      this.Controls.Add((Control) this.chkPaystubsCoBorrower);
      this.Controls.Add((Control) this.chkSocialSecurityBorrower);
      this.Controls.Add((Control) this.chkPensionBorrower);
      this.Controls.Add((Control) this.chk401KBorrower);
      this.Controls.Add((Control) this.chk1099Borrower);
      this.Controls.Add((Control) this.chkW2Borrower);
      this.Controls.Add((Control) this.chkTaxReturnBorrower);
      this.Controls.Add((Control) this.chkPaystubsBorrower);
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
      this.Name = nameof (AddVerificationEmploymentIncomeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Income Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
