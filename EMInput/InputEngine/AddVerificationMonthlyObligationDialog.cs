// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationMonthlyObligationDialog
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
  public class AddVerificationMonthlyObligationDialog : Form
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
    private CheckBox chkChargeAccountsBorrower;
    private CheckBox chkChildSupportBorrower;
    private CheckBox chkSimultaneousBorrower;
    private CheckBox chkHousingBorrower;
    private CheckBox chkAlimonyBorrower;
    private CheckBox chkRealEstateBorrower;
    private CheckBox chkInstallmentBorrower;
    private CheckBox chkChargeAccountsCoBorrower;
    private CheckBox chkChildSupportCoBorrower;
    private CheckBox chkSimultaneousCoBorrower;
    private CheckBox chkHousingCoBorrower;
    private CheckBox chkAlimonyCoBorrower;
    private CheckBox chkRealEstateCoBorrower;
    private CheckBox chkInstallmentCoBorrower;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private EMHelpLink helpLink;
    private Label label10;
    private Label label11;
    private CheckBox chkOtherCoBorrower;
    private CheckBox chkEscrowsCoBorrower;
    private CheckBox chkOtherBorrower;
    private CheckBox chkEscrowsBorrower;
    private TextBox txtOther;

    public AddVerificationMonthlyObligationDialog() => this.InitializeComponent();

    public DocumentVerificationObligation[] Verifications => this.verificationList.ToArray();

    private void chkOther_CheckedChanged(object sender, EventArgs e)
    {
      this.txtOther.Enabled = this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked;
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
      if ((this.chkOtherBorrower.Checked || this.chkOtherCoBorrower.Checked) && string.IsNullOrEmpty(this.txtOther.Text))
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "You must enter a description for 'Other'.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        this.tryAddVerification(ObligationType.InstallmentLoan, this.chkInstallmentBorrower.Checked, this.chkInstallmentCoBorrower.Checked);
        this.tryAddVerification(ObligationType.RealEstateLoan, this.chkRealEstateBorrower.Checked, this.chkRealEstateCoBorrower.Checked);
        this.tryAddVerification(ObligationType.AlimonyOrMaintenance, this.chkAlimonyBorrower.Checked, this.chkAlimonyCoBorrower.Checked);
        this.tryAddVerification(ObligationType.MonthlyHousingExpense, this.chkHousingBorrower.Checked, this.chkHousingCoBorrower.Checked);
        this.tryAddVerification(ObligationType.RevolvingChargeAccount, this.chkChargeAccountsBorrower.Checked, this.chkChargeAccountsCoBorrower.Checked);
        this.tryAddVerification(ObligationType.SimultaneousLoansOnProperty, this.chkSimultaneousBorrower.Checked, this.chkSimultaneousCoBorrower.Checked);
        this.tryAddVerification(ObligationType.ChildSupport, this.chkChildSupportBorrower.Checked, this.chkChildSupportCoBorrower.Checked);
        this.tryAddVerification(ObligationType.RequiredEscrow, this.chkEscrowsBorrower.Checked, this.chkEscrowsCoBorrower.Checked);
        this.tryAddVerification(ObligationType.OtherMonthlyObligation, this.chkOtherBorrower.Checked, this.chkOtherCoBorrower.Checked);
        this.DialogResult = DialogResult.OK;
      }
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
      if (obligationType == ObligationType.OtherMonthlyObligation)
        verificationObligation.OtherDescription = this.txtOther.Text;
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
      this.chkChargeAccountsBorrower = new CheckBox();
      this.chkChildSupportBorrower = new CheckBox();
      this.chkSimultaneousBorrower = new CheckBox();
      this.chkHousingBorrower = new CheckBox();
      this.chkAlimonyBorrower = new CheckBox();
      this.chkRealEstateBorrower = new CheckBox();
      this.chkInstallmentBorrower = new CheckBox();
      this.chkChargeAccountsCoBorrower = new CheckBox();
      this.chkChildSupportCoBorrower = new CheckBox();
      this.chkSimultaneousCoBorrower = new CheckBox();
      this.chkHousingCoBorrower = new CheckBox();
      this.chkAlimonyCoBorrower = new CheckBox();
      this.chkRealEstateCoBorrower = new CheckBox();
      this.chkInstallmentCoBorrower = new CheckBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.label10 = new Label();
      this.label11 = new Label();
      this.chkOtherCoBorrower = new CheckBox();
      this.chkEscrowsCoBorrower = new CheckBox();
      this.chkOtherBorrower = new CheckBox();
      this.chkEscrowsBorrower = new CheckBox();
      this.txtOther = new TextBox();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 265);
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
      this.lblType.Size = new Size(116, 14);
      this.lblType.TabIndex = 34;
      this.lblType.Text = "Monthly Obligations";
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
      this.label3.Size = new Size(90, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "Installment Loans";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 66);
      this.label4.Name = "label4";
      this.label4.Size = new Size(94, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Real Estate Loans";
      this.chkChargeAccountsBorrower.AutoSize = true;
      this.chkChargeAccountsBorrower.Location = new Point(277, 138);
      this.chkChargeAccountsBorrower.Name = "chkChargeAccountsBorrower";
      this.chkChargeAccountsBorrower.Size = new Size(15, 14);
      this.chkChargeAccountsBorrower.TabIndex = 120;
      this.chkChargeAccountsBorrower.UseVisualStyleBackColor = true;
      this.chkChildSupportBorrower.AutoSize = true;
      this.chkChildSupportBorrower.Location = new Point(277, 186);
      this.chkChildSupportBorrower.Name = "chkChildSupportBorrower";
      this.chkChildSupportBorrower.Size = new Size(15, 14);
      this.chkChildSupportBorrower.TabIndex = 119;
      this.chkChildSupportBorrower.UseVisualStyleBackColor = true;
      this.chkSimultaneousBorrower.AutoSize = true;
      this.chkSimultaneousBorrower.Location = new Point(277, 162);
      this.chkSimultaneousBorrower.Name = "chkSimultaneousBorrower";
      this.chkSimultaneousBorrower.Size = new Size(15, 14);
      this.chkSimultaneousBorrower.TabIndex = 118;
      this.chkSimultaneousBorrower.UseVisualStyleBackColor = true;
      this.chkHousingBorrower.AutoSize = true;
      this.chkHousingBorrower.Location = new Point(277, 114);
      this.chkHousingBorrower.Name = "chkHousingBorrower";
      this.chkHousingBorrower.Size = new Size(15, 14);
      this.chkHousingBorrower.TabIndex = 117;
      this.chkHousingBorrower.UseVisualStyleBackColor = true;
      this.chkAlimonyBorrower.AutoSize = true;
      this.chkAlimonyBorrower.Location = new Point(277, 90);
      this.chkAlimonyBorrower.Name = "chkAlimonyBorrower";
      this.chkAlimonyBorrower.Size = new Size(15, 14);
      this.chkAlimonyBorrower.TabIndex = 116;
      this.chkAlimonyBorrower.UseVisualStyleBackColor = true;
      this.chkRealEstateBorrower.AutoSize = true;
      this.chkRealEstateBorrower.Location = new Point(277, 66);
      this.chkRealEstateBorrower.Name = "chkRealEstateBorrower";
      this.chkRealEstateBorrower.Size = new Size(15, 14);
      this.chkRealEstateBorrower.TabIndex = 115;
      this.chkRealEstateBorrower.UseVisualStyleBackColor = true;
      this.chkInstallmentBorrower.AutoSize = true;
      this.chkInstallmentBorrower.Location = new Point(277, 42);
      this.chkInstallmentBorrower.Name = "chkInstallmentBorrower";
      this.chkInstallmentBorrower.Size = new Size(15, 14);
      this.chkInstallmentBorrower.TabIndex = 114;
      this.chkInstallmentBorrower.UseVisualStyleBackColor = true;
      this.chkChargeAccountsCoBorrower.AutoSize = true;
      this.chkChargeAccountsCoBorrower.Location = new Point(383, 139);
      this.chkChargeAccountsCoBorrower.Name = "chkChargeAccountsCoBorrower";
      this.chkChargeAccountsCoBorrower.Size = new Size(15, 14);
      this.chkChargeAccountsCoBorrower.TabIndex = (int) sbyte.MaxValue;
      this.chkChargeAccountsCoBorrower.UseVisualStyleBackColor = true;
      this.chkChildSupportCoBorrower.AutoSize = true;
      this.chkChildSupportCoBorrower.Location = new Point(383, 187);
      this.chkChildSupportCoBorrower.Name = "chkChildSupportCoBorrower";
      this.chkChildSupportCoBorrower.Size = new Size(15, 14);
      this.chkChildSupportCoBorrower.TabIndex = 126;
      this.chkChildSupportCoBorrower.UseVisualStyleBackColor = true;
      this.chkSimultaneousCoBorrower.AutoSize = true;
      this.chkSimultaneousCoBorrower.Location = new Point(383, 163);
      this.chkSimultaneousCoBorrower.Name = "chkSimultaneousCoBorrower";
      this.chkSimultaneousCoBorrower.Size = new Size(15, 14);
      this.chkSimultaneousCoBorrower.TabIndex = 125;
      this.chkSimultaneousCoBorrower.UseVisualStyleBackColor = true;
      this.chkHousingCoBorrower.AutoSize = true;
      this.chkHousingCoBorrower.Location = new Point(383, 115);
      this.chkHousingCoBorrower.Name = "chkHousingCoBorrower";
      this.chkHousingCoBorrower.Size = new Size(15, 14);
      this.chkHousingCoBorrower.TabIndex = 124;
      this.chkHousingCoBorrower.UseVisualStyleBackColor = true;
      this.chkAlimonyCoBorrower.AutoSize = true;
      this.chkAlimonyCoBorrower.Location = new Point(383, 91);
      this.chkAlimonyCoBorrower.Name = "chkAlimonyCoBorrower";
      this.chkAlimonyCoBorrower.Size = new Size(15, 14);
      this.chkAlimonyCoBorrower.TabIndex = 123;
      this.chkAlimonyCoBorrower.UseVisualStyleBackColor = true;
      this.chkRealEstateCoBorrower.AutoSize = true;
      this.chkRealEstateCoBorrower.Location = new Point(383, 67);
      this.chkRealEstateCoBorrower.Name = "chkRealEstateCoBorrower";
      this.chkRealEstateCoBorrower.Size = new Size(15, 14);
      this.chkRealEstateCoBorrower.TabIndex = 122;
      this.chkRealEstateCoBorrower.UseVisualStyleBackColor = true;
      this.chkInstallmentCoBorrower.AutoSize = true;
      this.chkInstallmentCoBorrower.Location = new Point(383, 43);
      this.chkInstallmentCoBorrower.Name = "chkInstallmentCoBorrower";
      this.chkInstallmentCoBorrower.Size = new Size(15, 14);
      this.chkInstallmentCoBorrower.TabIndex = 121;
      this.chkInstallmentCoBorrower.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 115);
      this.label5.Name = "label5";
      this.label5.Size = new Size(161, 14);
      this.label5.TabIndex = 129;
      this.label5.Text = "Monthly Housing Expense [P && I]";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 91);
      this.label6.Name = "label6";
      this.label6.Size = new Size(109, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "Alimony Maintenance";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 187);
      this.label7.Name = "label7";
      this.label7.Size = new Size(71, 14);
      this.label7.TabIndex = 132;
      this.label7.Text = "Child Support";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 163);
      this.label8.Name = "label8";
      this.label8.Size = new Size(165, 14);
      this.label8.TabIndex = 131;
      this.label8.Text = "Simultaneous Loans On Property";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(12, 138);
      this.label9.Name = "label9";
      this.label9.Size = new Size(141, 14);
      this.label9.TabIndex = 130;
      this.label9.Text = "Revolving Charge Accounts";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 234);
      this.label10.Name = "label10";
      this.label10.Size = new Size(34, 14);
      this.label10.TabIndex = 138;
      this.label10.Text = "Other";
      this.label11.AutoSize = true;
      this.label11.Location = new Point(12, 210);
      this.label11.Name = "label11";
      this.label11.Size = new Size(250, 14);
      this.label11.TabIndex = 137;
      this.label11.Text = "Required Escrows (i.e. Taxes, Ins, MI, HOA, Dues)";
      this.chkOtherCoBorrower.AutoSize = true;
      this.chkOtherCoBorrower.Location = new Point(383, 234);
      this.chkOtherCoBorrower.Name = "chkOtherCoBorrower";
      this.chkOtherCoBorrower.Size = new Size(15, 14);
      this.chkOtherCoBorrower.TabIndex = 136;
      this.chkOtherCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherCoBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkEscrowsCoBorrower.AutoSize = true;
      this.chkEscrowsCoBorrower.Location = new Point(383, 210);
      this.chkEscrowsCoBorrower.Name = "chkEscrowsCoBorrower";
      this.chkEscrowsCoBorrower.Size = new Size(15, 14);
      this.chkEscrowsCoBorrower.TabIndex = 135;
      this.chkEscrowsCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.AutoSize = true;
      this.chkOtherBorrower.Location = new Point(277, 233);
      this.chkOtherBorrower.Name = "chkOtherBorrower";
      this.chkOtherBorrower.Size = new Size(15, 14);
      this.chkOtherBorrower.TabIndex = 134;
      this.chkOtherBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkEscrowsBorrower.AutoSize = true;
      this.chkEscrowsBorrower.Location = new Point(277, 209);
      this.chkEscrowsBorrower.Name = "chkEscrowsBorrower";
      this.chkEscrowsBorrower.Size = new Size(15, 14);
      this.chkEscrowsBorrower.TabIndex = 133;
      this.chkEscrowsBorrower.UseVisualStyleBackColor = true;
      this.txtOther.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOther.Enabled = false;
      this.txtOther.Location = new Point(52, 231);
      this.txtOther.Name = "txtOther";
      this.txtOther.Size = new Size(185, 20);
      this.txtOther.TabIndex = 139;
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(457, 316);
      this.Controls.Add((Control) this.txtOther);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.label11);
      this.Controls.Add((Control) this.chkOtherCoBorrower);
      this.Controls.Add((Control) this.chkEscrowsCoBorrower);
      this.Controls.Add((Control) this.chkOtherBorrower);
      this.Controls.Add((Control) this.chkEscrowsBorrower);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkChargeAccountsCoBorrower);
      this.Controls.Add((Control) this.chkChildSupportCoBorrower);
      this.Controls.Add((Control) this.chkSimultaneousCoBorrower);
      this.Controls.Add((Control) this.chkHousingCoBorrower);
      this.Controls.Add((Control) this.chkAlimonyCoBorrower);
      this.Controls.Add((Control) this.chkRealEstateCoBorrower);
      this.Controls.Add((Control) this.chkInstallmentCoBorrower);
      this.Controls.Add((Control) this.chkChargeAccountsBorrower);
      this.Controls.Add((Control) this.chkChildSupportBorrower);
      this.Controls.Add((Control) this.chkSimultaneousBorrower);
      this.Controls.Add((Control) this.chkHousingBorrower);
      this.Controls.Add((Control) this.chkAlimonyBorrower);
      this.Controls.Add((Control) this.chkRealEstateBorrower);
      this.Controls.Add((Control) this.chkInstallmentBorrower);
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
      this.Name = nameof (AddVerificationMonthlyObligationDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Monthly Obligation Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
