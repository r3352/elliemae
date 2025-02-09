// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationNonEmploymentIncomeDialog
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
  public class AddVerificationNonEmploymentIncomeDialog : Form
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
    private CheckBox chkRentalBorrower;
    private CheckBox chkChildSupportBorrower;
    private CheckBox chkAlimonyBorrower;
    private CheckBox chkRentalCoBorrower;
    private CheckBox chkChildSupportCoBorrower;
    private CheckBox chkAlimonyCoBorrower;
    private Label label6;
    private EMHelpLink helpLink;
    private Label label10;
    private CheckBox chkOtherCoBorrower;
    private CheckBox chkOtherBorrower;
    private TextBox txtOther;
    private Label label12;

    public AddVerificationNonEmploymentIncomeDialog() => this.InitializeComponent();

    public DocumentVerificationIncome[] Verifications => this.verificationList.ToArray();

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
        this.tryAddVerification(IncomeType.AlimonyOrMaintenance, this.chkAlimonyBorrower.Checked, this.chkAlimonyCoBorrower.Checked);
        this.tryAddVerification(IncomeType.ChildSupport, this.chkChildSupportBorrower.Checked, this.chkChildSupportCoBorrower.Checked);
        this.tryAddVerification(IncomeType.RentalIncome, this.chkRentalBorrower.Checked, this.chkRentalCoBorrower.Checked);
        this.tryAddVerification(IncomeType.OtherNonEmployment, this.chkOtherBorrower.Checked, this.chkOtherCoBorrower.Checked);
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
      if (incomeType == IncomeType.OtherNonEmployment)
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
      this.chkRentalBorrower = new CheckBox();
      this.chkChildSupportBorrower = new CheckBox();
      this.chkAlimonyBorrower = new CheckBox();
      this.chkRentalCoBorrower = new CheckBox();
      this.chkChildSupportCoBorrower = new CheckBox();
      this.chkAlimonyCoBorrower = new CheckBox();
      this.label6 = new Label();
      this.label10 = new Label();
      this.chkOtherCoBorrower = new CheckBox();
      this.chkOtherBorrower = new CheckBox();
      this.txtOther = new TextBox();
      this.label12 = new Label();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 180);
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
      this.lblType.Size = new Size(101, 14);
      this.lblType.TabIndex = 34;
      this.lblType.Text = "Non-Employment";
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
      this.label3.Size = new Size(115, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "Alimony / Maintenance";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 91);
      this.label4.Name = "label4";
      this.label4.Size = new Size(71, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Child Support";
      this.chkRentalBorrower.AutoSize = true;
      this.chkRentalBorrower.Location = new Point(277, 115);
      this.chkRentalBorrower.Name = "chkRentalBorrower";
      this.chkRentalBorrower.Size = new Size(15, 14);
      this.chkRentalBorrower.TabIndex = 116;
      this.chkRentalBorrower.UseVisualStyleBackColor = true;
      this.chkChildSupportBorrower.AutoSize = true;
      this.chkChildSupportBorrower.Location = new Point(277, 91);
      this.chkChildSupportBorrower.Name = "chkChildSupportBorrower";
      this.chkChildSupportBorrower.Size = new Size(15, 14);
      this.chkChildSupportBorrower.TabIndex = 115;
      this.chkChildSupportBorrower.UseVisualStyleBackColor = true;
      this.chkAlimonyBorrower.AutoSize = true;
      this.chkAlimonyBorrower.Location = new Point(277, 67);
      this.chkAlimonyBorrower.Name = "chkAlimonyBorrower";
      this.chkAlimonyBorrower.Size = new Size(15, 14);
      this.chkAlimonyBorrower.TabIndex = 114;
      this.chkAlimonyBorrower.UseVisualStyleBackColor = true;
      this.chkRentalCoBorrower.AutoSize = true;
      this.chkRentalCoBorrower.Location = new Point(383, 116);
      this.chkRentalCoBorrower.Name = "chkRentalCoBorrower";
      this.chkRentalCoBorrower.Size = new Size(15, 14);
      this.chkRentalCoBorrower.TabIndex = 123;
      this.chkRentalCoBorrower.UseVisualStyleBackColor = true;
      this.chkChildSupportCoBorrower.AutoSize = true;
      this.chkChildSupportCoBorrower.Location = new Point(383, 92);
      this.chkChildSupportCoBorrower.Name = "chkChildSupportCoBorrower";
      this.chkChildSupportCoBorrower.Size = new Size(15, 14);
      this.chkChildSupportCoBorrower.TabIndex = 122;
      this.chkChildSupportCoBorrower.UseVisualStyleBackColor = true;
      this.chkAlimonyCoBorrower.AutoSize = true;
      this.chkAlimonyCoBorrower.Location = new Point(383, 68);
      this.chkAlimonyCoBorrower.Name = "chkAlimonyCoBorrower";
      this.chkAlimonyCoBorrower.Size = new Size(15, 14);
      this.chkAlimonyCoBorrower.TabIndex = 121;
      this.chkAlimonyCoBorrower.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 116);
      this.label6.Name = "label6";
      this.label6.Size = new Size(74, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "Rental Income";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 140);
      this.label10.Name = "label10";
      this.label10.Size = new Size(34, 14);
      this.label10.TabIndex = 138;
      this.label10.Text = "Other";
      this.chkOtherCoBorrower.AutoSize = true;
      this.chkOtherCoBorrower.Location = new Point(383, 140);
      this.chkOtherCoBorrower.Name = "chkOtherCoBorrower";
      this.chkOtherCoBorrower.Size = new Size(15, 14);
      this.chkOtherCoBorrower.TabIndex = 136;
      this.chkOtherCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherCoBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkOtherBorrower.AutoSize = true;
      this.chkOtherBorrower.Location = new Point(277, 139);
      this.chkOtherBorrower.Name = "chkOtherBorrower";
      this.chkOtherBorrower.Size = new Size(15, 14);
      this.chkOtherBorrower.TabIndex = 134;
      this.chkOtherBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.txtOther.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOther.Enabled = false;
      this.txtOther.Location = new Point(52, 137);
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
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(457, 231);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.txtOther);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.chkOtherCoBorrower);
      this.Controls.Add((Control) this.chkOtherBorrower);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkRentalCoBorrower);
      this.Controls.Add((Control) this.chkChildSupportCoBorrower);
      this.Controls.Add((Control) this.chkAlimonyCoBorrower);
      this.Controls.Add((Control) this.chkRentalBorrower);
      this.Controls.Add((Control) this.chkChildSupportBorrower);
      this.Controls.Add((Control) this.chkAlimonyBorrower);
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
      this.Name = nameof (AddVerificationNonEmploymentIncomeDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Income Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
