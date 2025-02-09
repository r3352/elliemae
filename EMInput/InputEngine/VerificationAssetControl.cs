// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationAssetControl
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.InputEngine
{
  public class VerificationAssetControl : UserControl, IVerificationDetails
  {
    public EventHandler OnStatusChanged;
    private VerificationDetailsControl.BorrowerEditMode editMode;
    private IContainer components;
    private GroupContainer groupContainerBor;
    private CheckBox chkOther;
    private CheckBox chkMutual;
    private CheckBox chkRental;
    private CheckBox chkBank;
    private Label label1;
    private TextBox txtOther;

    public VerificationAssetControl(
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
      VerificationTimelineAssetLog timelineAssetLog = (VerificationTimelineAssetLog) verificationLog;
      this.chkBank.Checked = timelineAssetLog.IsBankStatements;
      this.chkRental.Checked = timelineAssetLog.IsRentalPropertyIncome;
      this.chkMutual.Checked = timelineAssetLog.IsMutualFunds;
      this.chkOther.Checked = timelineAssetLog.IsOther;
      this.txtOther.Text = this.chkOther.Checked ? timelineAssetLog.OtherDescription : "";
      this.chkOther_CheckedChanged((object) this.chkOther, (EventArgs) null);
    }

    public void GetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineAssetLog timelineAssetLog = (VerificationTimelineAssetLog) verificationLog;
      timelineAssetLog.IsBankStatements = this.chkBank.Checked;
      timelineAssetLog.IsRentalPropertyIncome = this.chkRental.Checked;
      timelineAssetLog.IsMutualFunds = this.chkMutual.Checked;
      timelineAssetLog.IsOther = this.chkOther.Checked;
      timelineAssetLog.OtherDescription = timelineAssetLog.IsOther ? this.txtOther.Text.Trim() : "";
    }

    private void chkOther_CheckedChanged(object sender, EventArgs e)
    {
      this.checkbox_CheckedChanged(sender, e);
    }

    private void checkbox_CheckedChanged(object sender, EventArgs e)
    {
      if (sender == null)
        return;
      CheckBox checkBox = (CheckBox) sender;
      if (checkBox.Checked)
      {
        if (checkBox.Name != this.chkBank.Name)
          this.chkBank.Checked = false;
        if (checkBox.Name != this.chkRental.Name)
          this.chkRental.Checked = false;
        if (checkBox.Name != this.chkMutual.Name)
          this.chkMutual.Checked = false;
        if (checkBox.Name != this.chkOther.Name)
          this.chkOther.Checked = false;
      }
      if (checkBox.Name == "chkOther")
      {
        if (!this.chkOther.Checked)
        {
          this.txtOther.Text = string.Empty;
          this.txtOther.ReadOnly = true;
        }
        else
          this.txtOther.ReadOnly = false;
      }
      if (this.OnStatusChanged == null)
        return;
      this.OnStatusChanged(sender, new EventArgs());
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.chkBank.Checked)
        str += "Bank Statements";
      if (this.chkRental.Checked)
        str = str + (str != string.Empty ? "," : "") + "Rental Property Income - Schedule E";
      if (this.chkMutual.Checked)
        str = str + (str != string.Empty ? "," : "") + "Mutual Funds";
      if (this.chkOther.Checked)
        str = str + (str != string.Empty ? "," : "") + "Other";
      return str;
    }

    public void SetReadOnly()
    {
      this.chkBank.Enabled = false;
      this.chkRental.Enabled = false;
      this.chkMutual.Enabled = false;
      this.chkOther.Enabled = false;
      this.txtOther.ReadOnly = true;
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
      this.txtOther = new TextBox();
      this.chkOther = new CheckBox();
      this.chkMutual = new CheckBox();
      this.chkRental = new CheckBox();
      this.chkBank = new CheckBox();
      this.label1 = new Label();
      this.groupContainerBor.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerBor.BackColor = Color.White;
      this.groupContainerBor.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerBor.Controls.Add((Control) this.txtOther);
      this.groupContainerBor.Controls.Add((Control) this.chkOther);
      this.groupContainerBor.Controls.Add((Control) this.chkMutual);
      this.groupContainerBor.Controls.Add((Control) this.chkRental);
      this.groupContainerBor.Controls.Add((Control) this.chkBank);
      this.groupContainerBor.Controls.Add((Control) this.label1);
      this.groupContainerBor.Dock = DockStyle.Fill;
      this.groupContainerBor.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerBor.Location = new Point(0, 0);
      this.groupContainerBor.Name = "groupContainerBor";
      this.groupContainerBor.Size = new Size(650, 114);
      this.groupContainerBor.TabIndex = 0;
      this.groupContainerBor.Text = "Borrower";
      this.txtOther.BorderStyle = BorderStyle.FixedSingle;
      this.txtOther.Location = new Point(273, 65);
      this.txtOther.Name = "txtOther";
      this.txtOther.Size = new Size(149, 20);
      this.txtOther.TabIndex = 5;
      this.chkOther.AutoSize = true;
      this.chkOther.Location = new Point(218, 66);
      this.chkOther.Name = "chkOther";
      this.chkOther.Size = new Size(55, 17);
      this.chkOther.TabIndex = 4;
      this.chkOther.Text = "Other:";
      this.chkOther.UseVisualStyleBackColor = true;
      this.chkOther.CheckedChanged += new EventHandler(this.chkOther_CheckedChanged);
      this.chkMutual.AutoSize = true;
      this.chkMutual.Location = new Point(218, 48);
      this.chkMutual.Name = "chkMutual";
      this.chkMutual.Size = new Size(90, 17);
      this.chkMutual.TabIndex = 3;
      this.chkMutual.Text = "Mutual Funds";
      this.chkMutual.UseVisualStyleBackColor = true;
      this.chkMutual.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkRental.AutoSize = true;
      this.chkRental.Location = new Point(9, 66);
      this.chkRental.Name = "chkRental";
      this.chkRental.Size = new Size(201, 17);
      this.chkRental.TabIndex = 2;
      this.chkRental.Text = "Rental Property Income - Schedule E";
      this.chkRental.UseVisualStyleBackColor = true;
      this.chkRental.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkBank.AutoSize = true;
      this.chkBank.Location = new Point(9, 48);
      this.chkBank.Name = "chkBank";
      this.chkBank.Size = new Size(107, 17);
      this.chkBank.TabIndex = 1;
      this.chkBank.Text = "Bank Statements";
      this.chkBank.UseVisualStyleBackColor = true;
      this.chkBank.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(6, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(44, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Assets";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerBor);
      this.Name = nameof (VerificationAssetControl);
      this.Size = new Size(650, 114);
      this.groupContainerBor.ResumeLayout(false);
      this.groupContainerBor.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
