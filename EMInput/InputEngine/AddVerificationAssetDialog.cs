// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationAssetDialog
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
  public class AddVerificationAssetDialog : Form
  {
    private List<DocumentVerificationAsset> verificationList = new List<DocumentVerificationAsset>();
    private IContainer components;
    private ToolTip tooltip;
    private Panel pnlClose;
    private Button btnAdd;
    private Button btnCancel;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private CheckBox chkRentalBorrower;
    private CheckBox chkMutualFundsBorrower;
    private CheckBox chkBankBorrower;
    private CheckBox chkRentalCoBorrower;
    private CheckBox chkMutualFundsCoBorrower;
    private CheckBox chkBankCoBorrower;
    private Label label6;
    private EMHelpLink helpLink;
    private Label label10;
    private CheckBox chkOtherCoBorrower;
    private CheckBox chkOtherBorrower;
    private TextBox txtOther;
    private Label label12;

    public AddVerificationAssetDialog() => this.InitializeComponent();

    public DocumentVerificationAsset[] Verifications => this.verificationList.ToArray();

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
        this.tryAddVerification(AssetType.BankStatement, this.chkBankBorrower.Checked, this.chkBankCoBorrower.Checked);
        this.tryAddVerification(AssetType.MutualFund, this.chkMutualFundsBorrower.Checked, this.chkMutualFundsCoBorrower.Checked);
        this.tryAddVerification(AssetType.RentalPropertyIncome, this.chkRentalBorrower.Checked, this.chkRentalCoBorrower.Checked);
        this.tryAddVerification(AssetType.Other, this.chkOtherBorrower.Checked, this.chkOtherCoBorrower.Checked);
        this.DialogResult = DialogResult.OK;
      }
    }

    private void tryAddVerification(
      AssetType assetType,
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
      DocumentVerificationAsset verificationAsset = new DocumentVerificationAsset(borrowerType);
      verificationAsset.AssetType = assetType;
      if (assetType == AssetType.Other)
        verificationAsset.OtherDescription = this.txtOther.Text;
      this.verificationList.Add(verificationAsset);
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
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.chkRentalBorrower = new CheckBox();
      this.chkMutualFundsBorrower = new CheckBox();
      this.chkBankBorrower = new CheckBox();
      this.chkRentalCoBorrower = new CheckBox();
      this.chkMutualFundsCoBorrower = new CheckBox();
      this.chkBankCoBorrower = new CheckBox();
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
      this.pnlClose.Location = new Point(0, 161);
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
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(244, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 14);
      this.label1.TabIndex = 35;
      this.label1.Text = "For Borrower";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(337, 22);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 14);
      this.label2.TabIndex = 36;
      this.label2.Text = "For Co-Borrower";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 47);
      this.label3.Name = "label3";
      this.label3.Size = new Size(88, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "Bank Statements";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 71);
      this.label4.Name = "label4";
      this.label4.Size = new Size(71, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Mutual Funds";
      this.chkRentalBorrower.AutoSize = true;
      this.chkRentalBorrower.Location = new Point(277, 95);
      this.chkRentalBorrower.Name = "chkRentalBorrower";
      this.chkRentalBorrower.Size = new Size(15, 14);
      this.chkRentalBorrower.TabIndex = 116;
      this.chkRentalBorrower.UseVisualStyleBackColor = true;
      this.chkMutualFundsBorrower.AutoSize = true;
      this.chkMutualFundsBorrower.Location = new Point(277, 71);
      this.chkMutualFundsBorrower.Name = "chkMutualFundsBorrower";
      this.chkMutualFundsBorrower.Size = new Size(15, 14);
      this.chkMutualFundsBorrower.TabIndex = 115;
      this.chkMutualFundsBorrower.UseVisualStyleBackColor = true;
      this.chkBankBorrower.AutoSize = true;
      this.chkBankBorrower.Location = new Point(277, 47);
      this.chkBankBorrower.Name = "chkBankBorrower";
      this.chkBankBorrower.Size = new Size(15, 14);
      this.chkBankBorrower.TabIndex = 114;
      this.chkBankBorrower.UseVisualStyleBackColor = true;
      this.chkRentalCoBorrower.AutoSize = true;
      this.chkRentalCoBorrower.Location = new Point(383, 96);
      this.chkRentalCoBorrower.Name = "chkRentalCoBorrower";
      this.chkRentalCoBorrower.Size = new Size(15, 14);
      this.chkRentalCoBorrower.TabIndex = 123;
      this.chkRentalCoBorrower.UseVisualStyleBackColor = true;
      this.chkMutualFundsCoBorrower.AutoSize = true;
      this.chkMutualFundsCoBorrower.Location = new Point(383, 72);
      this.chkMutualFundsCoBorrower.Name = "chkMutualFundsCoBorrower";
      this.chkMutualFundsCoBorrower.Size = new Size(15, 14);
      this.chkMutualFundsCoBorrower.TabIndex = 122;
      this.chkMutualFundsCoBorrower.UseVisualStyleBackColor = true;
      this.chkBankCoBorrower.AutoSize = true;
      this.chkBankCoBorrower.Location = new Point(383, 48);
      this.chkBankCoBorrower.Name = "chkBankCoBorrower";
      this.chkBankCoBorrower.Size = new Size(15, 14);
      this.chkBankCoBorrower.TabIndex = 121;
      this.chkBankCoBorrower.UseVisualStyleBackColor = true;
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 96);
      this.label6.Name = "label6";
      this.label6.Size = new Size(182, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "Rental Property Income - Schedule E";
      this.label10.AutoSize = true;
      this.label10.Location = new Point(12, 120);
      this.label10.Name = "label10";
      this.label10.Size = new Size(34, 14);
      this.label10.TabIndex = 138;
      this.label10.Text = "Other";
      this.chkOtherCoBorrower.AutoSize = true;
      this.chkOtherCoBorrower.Location = new Point(383, 120);
      this.chkOtherCoBorrower.Name = "chkOtherCoBorrower";
      this.chkOtherCoBorrower.Size = new Size(15, 14);
      this.chkOtherCoBorrower.TabIndex = 136;
      this.chkOtherCoBorrower.UseVisualStyleBackColor = true;
      this.chkOtherCoBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkOtherBorrower.AutoSize = true;
      this.chkOtherBorrower.Location = new Point(277, 119);
      this.chkOtherBorrower.Name = "chkOtherBorrower";
      this.chkOtherBorrower.Size = new Size(15, 14);
      this.chkOtherBorrower.TabIndex = 134;
      this.chkOtherBorrower.UseVisualStyleBackColor = true;
      this.chkOtherBorrower.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.txtOther.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtOther.Enabled = false;
      this.txtOther.Location = new Point(52, 117);
      this.txtOther.Name = "txtOther";
      this.txtOther.Size = new Size(169, 20);
      this.txtOther.TabIndex = 139;
      this.label12.AutoSize = true;
      this.label12.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label12.Location = new Point(12, 22);
      this.label12.Name = "label12";
      this.label12.Size = new Size(47, 14);
      this.label12.TabIndex = 140;
      this.label12.Text = "Assets";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(457, 212);
      this.Controls.Add((Control) this.label12);
      this.Controls.Add((Control) this.txtOther);
      this.Controls.Add((Control) this.label10);
      this.Controls.Add((Control) this.chkOtherCoBorrower);
      this.Controls.Add((Control) this.chkOtherBorrower);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkRentalCoBorrower);
      this.Controls.Add((Control) this.chkMutualFundsCoBorrower);
      this.Controls.Add((Control) this.chkBankCoBorrower);
      this.Controls.Add((Control) this.chkRentalBorrower);
      this.Controls.Add((Control) this.chkMutualFundsBorrower);
      this.Controls.Add((Control) this.chkBankBorrower);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddVerificationAssetDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Asset Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
