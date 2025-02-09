// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.AddVerificationEmploymentDialog
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

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
  public class AddVerificationEmploymentDialog : Form
  {
    private List<DocumentVerificationEmployment> verificationList = new List<DocumentVerificationEmployment>();
    private IContainer components;
    private ToolTip tooltip;
    private Panel pnlClose;
    private Button btnAdd;
    private Button btnCancel;
    private Label lblEmploymentStatus;
    private Label label1;
    private Label label2;
    private Label label3;
    private Label label4;
    private CheckBox chkSelfEmployedBorrower;
    private CheckBox chkIrregularBorrower;
    private CheckBox chkRetiredBorrower;
    private CheckBox chkSeasonalBorrower;
    private CheckBox chkMilitaryBorrower;
    private CheckBox chkPartTimeBorrower;
    private CheckBox chkFullTimeBorrower;
    private CheckBox chkSelfEmployedCoBorrower;
    private CheckBox chkIrregularCoBorrower;
    private CheckBox chkRetiredCoBorrower;
    private CheckBox chkSeasonalCoBorrower;
    private CheckBox chkMilitaryCoBorrower;
    private CheckBox chkPartTimeCoBorrower;
    private CheckBox chkFullTimeCoBorrower;
    private Label label5;
    private Label label6;
    private Label label7;
    private Label label8;
    private Label label9;
    private EMHelpLink helpLink;

    public AddVerificationEmploymentDialog() => this.InitializeComponent();

    public DocumentVerificationEmployment[] Verifications => this.verificationList.ToArray();

    private void btnAdd_Click(object sender, EventArgs e)
    {
      this.tryAddVerification(EmploymentType.FullTime, this.chkFullTimeBorrower.Checked, this.chkFullTimeCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.PartTime, this.chkPartTimeBorrower.Checked, this.chkPartTimeCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.Military, this.chkMilitaryBorrower.Checked, this.chkMilitaryCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.Seasonal, this.chkSeasonalBorrower.Checked, this.chkSeasonalCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.SelfEmployed, this.chkSelfEmployedBorrower.Checked, this.chkSelfEmployedCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.Retired, this.chkRetiredBorrower.Checked, this.chkRetiredCoBorrower.Checked);
      this.tryAddVerification(EmploymentType.Irregular, this.chkIrregularBorrower.Checked, this.chkIrregularCoBorrower.Checked);
      this.DialogResult = DialogResult.OK;
    }

    private void tryAddVerification(
      EmploymentType employmentType,
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
      this.verificationList.Add(new DocumentVerificationEmployment(borrowerType)
      {
        EmploymentType = employmentType
      });
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
      this.lblEmploymentStatus = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.label4 = new Label();
      this.chkSelfEmployedBorrower = new CheckBox();
      this.chkIrregularBorrower = new CheckBox();
      this.chkRetiredBorrower = new CheckBox();
      this.chkSeasonalBorrower = new CheckBox();
      this.chkMilitaryBorrower = new CheckBox();
      this.chkPartTimeBorrower = new CheckBox();
      this.chkFullTimeBorrower = new CheckBox();
      this.chkSelfEmployedCoBorrower = new CheckBox();
      this.chkIrregularCoBorrower = new CheckBox();
      this.chkRetiredCoBorrower = new CheckBox();
      this.chkSeasonalCoBorrower = new CheckBox();
      this.chkMilitaryCoBorrower = new CheckBox();
      this.chkPartTimeCoBorrower = new CheckBox();
      this.chkFullTimeCoBorrower = new CheckBox();
      this.label5 = new Label();
      this.label6 = new Label();
      this.label7 = new Label();
      this.label8 = new Label();
      this.label9 = new Label();
      this.pnlClose.SuspendLayout();
      this.SuspendLayout();
      this.pnlClose.Controls.Add((Control) this.helpLink);
      this.pnlClose.Controls.Add((Control) this.btnAdd);
      this.pnlClose.Controls.Add((Control) this.btnCancel);
      this.pnlClose.Dock = DockStyle.Bottom;
      this.pnlClose.Location = new Point(0, 221);
      this.pnlClose.Name = "pnlClose";
      this.pnlClose.Size = new Size(359, 51);
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
      this.btnAdd.Location = new Point(186, 17);
      this.btnAdd.Name = "btnAdd";
      this.btnAdd.Size = new Size(75, 22);
      this.btnAdd.TabIndex = 5;
      this.btnAdd.Text = "Add";
      this.btnAdd.UseVisualStyleBackColor = true;
      this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(267, 17);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.lblEmploymentStatus.AutoSize = true;
      this.lblEmploymentStatus.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblEmploymentStatus.Location = new Point(12, 18);
      this.lblEmploymentStatus.Name = "lblEmploymentStatus";
      this.lblEmploymentStatus.Size = new Size(114, 14);
      this.lblEmploymentStatus.TabIndex = 34;
      this.lblEmploymentStatus.Text = "Employment Status";
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(150, 18);
      this.label1.Name = "label1";
      this.label1.Size = new Size(81, 14);
      this.label1.TabIndex = 35;
      this.label1.Text = "For Borrower";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point((int) byte.MaxValue, 18);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 14);
      this.label2.TabIndex = 36;
      this.label2.Text = "For Co-Borrower";
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 42);
      this.label3.Name = "label3";
      this.label3.Size = new Size(49, 14);
      this.label3.TabIndex = 37;
      this.label3.Text = "Full-Time";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 66);
      this.label4.Name = "label4";
      this.label4.Size = new Size(52, 14);
      this.label4.TabIndex = 38;
      this.label4.Text = "Part-Time";
      this.chkSelfEmployedBorrower.AutoSize = true;
      this.chkSelfEmployedBorrower.Location = new Point(185, 138);
      this.chkSelfEmployedBorrower.Name = "chkSelfEmployedBorrower";
      this.chkSelfEmployedBorrower.Size = new Size(15, 14);
      this.chkSelfEmployedBorrower.TabIndex = 120;
      this.chkSelfEmployedBorrower.UseVisualStyleBackColor = true;
      this.chkIrregularBorrower.AutoSize = true;
      this.chkIrregularBorrower.Location = new Point(185, 186);
      this.chkIrregularBorrower.Name = "chkIrregularBorrower";
      this.chkIrregularBorrower.Size = new Size(15, 14);
      this.chkIrregularBorrower.TabIndex = 119;
      this.chkIrregularBorrower.UseVisualStyleBackColor = true;
      this.chkRetiredBorrower.AutoSize = true;
      this.chkRetiredBorrower.Location = new Point(185, 162);
      this.chkRetiredBorrower.Name = "chkRetiredBorrower";
      this.chkRetiredBorrower.Size = new Size(15, 14);
      this.chkRetiredBorrower.TabIndex = 118;
      this.chkRetiredBorrower.UseVisualStyleBackColor = true;
      this.chkSeasonalBorrower.AutoSize = true;
      this.chkSeasonalBorrower.Location = new Point(185, 114);
      this.chkSeasonalBorrower.Name = "chkSeasonalBorrower";
      this.chkSeasonalBorrower.Size = new Size(15, 14);
      this.chkSeasonalBorrower.TabIndex = 117;
      this.chkSeasonalBorrower.UseVisualStyleBackColor = true;
      this.chkMilitaryBorrower.AutoSize = true;
      this.chkMilitaryBorrower.Location = new Point(185, 90);
      this.chkMilitaryBorrower.Name = "chkMilitaryBorrower";
      this.chkMilitaryBorrower.Size = new Size(15, 14);
      this.chkMilitaryBorrower.TabIndex = 116;
      this.chkMilitaryBorrower.UseVisualStyleBackColor = true;
      this.chkPartTimeBorrower.AutoSize = true;
      this.chkPartTimeBorrower.Location = new Point(185, 66);
      this.chkPartTimeBorrower.Name = "chkPartTimeBorrower";
      this.chkPartTimeBorrower.Size = new Size(15, 14);
      this.chkPartTimeBorrower.TabIndex = 115;
      this.chkPartTimeBorrower.UseVisualStyleBackColor = true;
      this.chkFullTimeBorrower.AutoSize = true;
      this.chkFullTimeBorrower.Location = new Point(185, 42);
      this.chkFullTimeBorrower.Name = "chkFullTimeBorrower";
      this.chkFullTimeBorrower.Size = new Size(15, 14);
      this.chkFullTimeBorrower.TabIndex = 114;
      this.chkFullTimeBorrower.UseVisualStyleBackColor = true;
      this.chkSelfEmployedCoBorrower.AutoSize = true;
      this.chkSelfEmployedCoBorrower.Location = new Point(303, 139);
      this.chkSelfEmployedCoBorrower.Name = "chkSelfEmployedCoBorrower";
      this.chkSelfEmployedCoBorrower.Size = new Size(15, 14);
      this.chkSelfEmployedCoBorrower.TabIndex = (int) sbyte.MaxValue;
      this.chkSelfEmployedCoBorrower.UseVisualStyleBackColor = true;
      this.chkIrregularCoBorrower.AutoSize = true;
      this.chkIrregularCoBorrower.Location = new Point(303, 187);
      this.chkIrregularCoBorrower.Name = "chkIrregularCoBorrower";
      this.chkIrregularCoBorrower.Size = new Size(15, 14);
      this.chkIrregularCoBorrower.TabIndex = 126;
      this.chkIrregularCoBorrower.UseVisualStyleBackColor = true;
      this.chkRetiredCoBorrower.AutoSize = true;
      this.chkRetiredCoBorrower.Location = new Point(303, 163);
      this.chkRetiredCoBorrower.Name = "chkRetiredCoBorrower";
      this.chkRetiredCoBorrower.Size = new Size(15, 14);
      this.chkRetiredCoBorrower.TabIndex = 125;
      this.chkRetiredCoBorrower.UseVisualStyleBackColor = true;
      this.chkSeasonalCoBorrower.AutoSize = true;
      this.chkSeasonalCoBorrower.Location = new Point(303, 115);
      this.chkSeasonalCoBorrower.Name = "chkSeasonalCoBorrower";
      this.chkSeasonalCoBorrower.Size = new Size(15, 14);
      this.chkSeasonalCoBorrower.TabIndex = 124;
      this.chkSeasonalCoBorrower.UseVisualStyleBackColor = true;
      this.chkMilitaryCoBorrower.AutoSize = true;
      this.chkMilitaryCoBorrower.Location = new Point(303, 91);
      this.chkMilitaryCoBorrower.Name = "chkMilitaryCoBorrower";
      this.chkMilitaryCoBorrower.Size = new Size(15, 14);
      this.chkMilitaryCoBorrower.TabIndex = 123;
      this.chkMilitaryCoBorrower.UseVisualStyleBackColor = true;
      this.chkPartTimeCoBorrower.AutoSize = true;
      this.chkPartTimeCoBorrower.Location = new Point(303, 67);
      this.chkPartTimeCoBorrower.Name = "chkPartTimeCoBorrower";
      this.chkPartTimeCoBorrower.Size = new Size(15, 14);
      this.chkPartTimeCoBorrower.TabIndex = 122;
      this.chkPartTimeCoBorrower.UseVisualStyleBackColor = true;
      this.chkFullTimeCoBorrower.AutoSize = true;
      this.chkFullTimeCoBorrower.Location = new Point(303, 43);
      this.chkFullTimeCoBorrower.Name = "chkFullTimeCoBorrower";
      this.chkFullTimeCoBorrower.Size = new Size(15, 14);
      this.chkFullTimeCoBorrower.TabIndex = 121;
      this.chkFullTimeCoBorrower.UseVisualStyleBackColor = true;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 115);
      this.label5.Name = "label5";
      this.label5.Size = new Size(52, 14);
      this.label5.TabIndex = 129;
      this.label5.Text = "Seasonal";
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 91);
      this.label6.Name = "label6";
      this.label6.Size = new Size(40, 14);
      this.label6.TabIndex = 128;
      this.label6.Text = "Military";
      this.label7.AutoSize = true;
      this.label7.Location = new Point(12, 187);
      this.label7.Name = "label7";
      this.label7.Size = new Size(126, 14);
      this.label7.TabIndex = 132;
      this.label7.Text = "Irregular (i.e. Contractor)";
      this.label8.AutoSize = true;
      this.label8.Location = new Point(12, 163);
      this.label8.Name = "label8";
      this.label8.Size = new Size(41, 14);
      this.label8.TabIndex = 131;
      this.label8.Text = "Retired";
      this.label9.AutoSize = true;
      this.label9.Location = new Point(12, 138);
      this.label9.Name = "label9";
      this.label9.Size = new Size(76, 14);
      this.label9.TabIndex = 130;
      this.label9.Text = "Self-Employed";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.ClientSize = new Size(359, 272);
      this.Controls.Add((Control) this.label7);
      this.Controls.Add((Control) this.label8);
      this.Controls.Add((Control) this.label9);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label6);
      this.Controls.Add((Control) this.chkSelfEmployedCoBorrower);
      this.Controls.Add((Control) this.chkIrregularCoBorrower);
      this.Controls.Add((Control) this.chkRetiredCoBorrower);
      this.Controls.Add((Control) this.chkSeasonalCoBorrower);
      this.Controls.Add((Control) this.chkMilitaryCoBorrower);
      this.Controls.Add((Control) this.chkPartTimeCoBorrower);
      this.Controls.Add((Control) this.chkFullTimeCoBorrower);
      this.Controls.Add((Control) this.chkSelfEmployedBorrower);
      this.Controls.Add((Control) this.chkIrregularBorrower);
      this.Controls.Add((Control) this.chkRetiredBorrower);
      this.Controls.Add((Control) this.chkSeasonalBorrower);
      this.Controls.Add((Control) this.chkMilitaryBorrower);
      this.Controls.Add((Control) this.chkPartTimeBorrower);
      this.Controls.Add((Control) this.chkFullTimeBorrower);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblEmploymentStatus);
      this.Controls.Add((Control) this.pnlClose);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddVerificationEmploymentDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "ATR/QM - Employment Verification";
      this.pnlClose.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
