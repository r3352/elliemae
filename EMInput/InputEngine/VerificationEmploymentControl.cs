// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.InputEngine.VerificationEmploymentControl
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
  public class VerificationEmploymentControl : UserControl, IVerificationDetails
  {
    public EventHandler OnStatusChanged;
    private VerificationDetailsControl.BorrowerEditMode editMode;
    private IContainer components;
    private GroupContainer groupContainerBor;
    private CheckBox chkMilitary;
    private CheckBox chkIrregular;
    private CheckBox chkSelfEmployed;
    private CheckBox chkPartTime;
    private CheckBox chkRetired;
    private CheckBox chkSeasonal;
    private CheckBox chkFullTime;
    private Label label1;

    public VerificationEmploymentControl(
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
      VerificationTimelineEmploymentLog timelineEmploymentLog = (VerificationTimelineEmploymentLog) verificationLog;
      this.chkFullTime.Checked = timelineEmploymentLog.IsFullTime;
      this.chkSeasonal.Checked = timelineEmploymentLog.IsSeasonal;
      this.chkRetired.Checked = timelineEmploymentLog.IsRetired;
      this.chkPartTime.Checked = timelineEmploymentLog.IsPartTime;
      this.chkSelfEmployed.Checked = timelineEmploymentLog.IsSelfEmployed;
      this.chkIrregular.Checked = timelineEmploymentLog.IsIrregular;
      this.chkMilitary.Checked = timelineEmploymentLog.IsMilitary;
    }

    public void GetDetails(VerificationTimelineLog verificationLog)
    {
      VerificationTimelineEmploymentLog timelineEmploymentLog = (VerificationTimelineEmploymentLog) verificationLog;
      timelineEmploymentLog.IsFullTime = this.chkFullTime.Checked;
      timelineEmploymentLog.IsSeasonal = this.chkSeasonal.Checked;
      timelineEmploymentLog.IsRetired = this.chkRetired.Checked;
      timelineEmploymentLog.IsPartTime = this.chkPartTime.Checked;
      timelineEmploymentLog.IsSelfEmployed = this.chkSelfEmployed.Checked;
      timelineEmploymentLog.IsIrregular = this.chkIrregular.Checked;
      timelineEmploymentLog.IsMilitary = this.chkMilitary.Checked;
    }

    private void checkbox_CheckedChanged(object sender, EventArgs e)
    {
      if (this.OnStatusChanged != null)
        this.OnStatusChanged(sender, new EventArgs());
      if (sender == null)
        return;
      CheckBox checkBox = (CheckBox) sender;
      if (!checkBox.Checked)
        return;
      if (checkBox.Name != this.chkFullTime.Name)
        this.chkFullTime.Checked = false;
      if (checkBox.Name != this.chkSeasonal.Name)
        this.chkSeasonal.Checked = false;
      if (checkBox.Name != this.chkRetired.Name)
        this.chkRetired.Checked = false;
      if (checkBox.Name != this.chkPartTime.Name)
        this.chkPartTime.Checked = false;
      if (checkBox.Name != this.chkSelfEmployed.Name)
        this.chkSelfEmployed.Checked = false;
      if (checkBox.Name != this.chkIrregular.Name)
        this.chkIrregular.Checked = false;
      if (!(checkBox.Name != this.chkMilitary.Name))
        return;
      this.chkMilitary.Checked = false;
    }

    public string BuildWhatVerified()
    {
      string str = string.Empty;
      if (this.chkFullTime.Checked)
        str += "Full Time";
      if (this.chkSeasonal.Checked)
        str = str + (str != string.Empty ? "," : "") + "Seasonal";
      if (this.chkRetired.Checked)
        str = str + (str != string.Empty ? "," : "") + "Retired";
      if (this.chkPartTime.Checked)
        str = str + (str != string.Empty ? "," : "") + "Part-Time";
      if (this.chkSelfEmployed.Checked)
        str = str + (str != string.Empty ? "," : "") + "Self-Employed";
      if (this.chkIrregular.Checked)
        str = str + (str != string.Empty ? "," : "") + "Irregular";
      if (this.chkMilitary.Checked)
        str = str + (str != string.Empty ? "," : "") + "Military";
      return str;
    }

    public void SetReadOnly()
    {
      this.chkFullTime.Enabled = false;
      this.chkSeasonal.Enabled = false;
      this.chkRetired.Enabled = false;
      this.chkPartTime.Enabled = false;
      this.chkSelfEmployed.Enabled = false;
      this.chkIrregular.Enabled = false;
      this.chkMilitary.Enabled = false;
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
      this.chkMilitary = new CheckBox();
      this.chkIrregular = new CheckBox();
      this.chkSelfEmployed = new CheckBox();
      this.chkPartTime = new CheckBox();
      this.chkRetired = new CheckBox();
      this.chkSeasonal = new CheckBox();
      this.chkFullTime = new CheckBox();
      this.label1 = new Label();
      this.groupContainerBor.SuspendLayout();
      this.SuspendLayout();
      this.groupContainerBor.BackColor = Color.White;
      this.groupContainerBor.Borders = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.groupContainerBor.Controls.Add((Control) this.chkMilitary);
      this.groupContainerBor.Controls.Add((Control) this.chkIrregular);
      this.groupContainerBor.Controls.Add((Control) this.chkSelfEmployed);
      this.groupContainerBor.Controls.Add((Control) this.chkPartTime);
      this.groupContainerBor.Controls.Add((Control) this.chkRetired);
      this.groupContainerBor.Controls.Add((Control) this.chkSeasonal);
      this.groupContainerBor.Controls.Add((Control) this.chkFullTime);
      this.groupContainerBor.Controls.Add((Control) this.label1);
      this.groupContainerBor.Dock = DockStyle.Fill;
      this.groupContainerBor.HeaderForeColor = SystemColors.ControlText;
      this.groupContainerBor.Location = new Point(0, 0);
      this.groupContainerBor.Name = "groupContainerBor";
      this.groupContainerBor.Size = new Size(650, 114);
      this.groupContainerBor.TabIndex = 0;
      this.groupContainerBor.Text = "Borrower";
      this.chkMilitary.AutoSize = true;
      this.chkMilitary.Location = new Point(243, 48);
      this.chkMilitary.Name = "chkMilitary";
      this.chkMilitary.Size = new Size(58, 17);
      this.chkMilitary.TabIndex = 7;
      this.chkMilitary.Text = "Military";
      this.chkMilitary.UseVisualStyleBackColor = true;
      this.chkMilitary.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkIrregular.AutoSize = true;
      this.chkIrregular.Location = new Point(130, 84);
      this.chkIrregular.Name = "chkIrregular";
      this.chkIrregular.Size = new Size(139, 17);
      this.chkIrregular.TabIndex = 6;
      this.chkIrregular.Text = "Irregular (i.e. Contractor)";
      this.chkIrregular.UseVisualStyleBackColor = true;
      this.chkIrregular.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkSelfEmployed.AutoSize = true;
      this.chkSelfEmployed.Location = new Point(130, 66);
      this.chkSelfEmployed.Name = "chkSelfEmployed";
      this.chkSelfEmployed.Size = new Size(93, 17);
      this.chkSelfEmployed.TabIndex = 5;
      this.chkSelfEmployed.Text = "Self-Employed";
      this.chkSelfEmployed.UseVisualStyleBackColor = true;
      this.chkSelfEmployed.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkPartTime.AutoSize = true;
      this.chkPartTime.Location = new Point(130, 48);
      this.chkPartTime.Name = "chkPartTime";
      this.chkPartTime.Size = new Size(71, 17);
      this.chkPartTime.TabIndex = 4;
      this.chkPartTime.Text = "Part-Time";
      this.chkPartTime.UseVisualStyleBackColor = true;
      this.chkPartTime.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkRetired.AutoSize = true;
      this.chkRetired.Location = new Point(9, 84);
      this.chkRetired.Name = "chkRetired";
      this.chkRetired.Size = new Size(60, 17);
      this.chkRetired.TabIndex = 3;
      this.chkRetired.Text = "Retired";
      this.chkRetired.UseVisualStyleBackColor = true;
      this.chkRetired.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkSeasonal.AutoSize = true;
      this.chkSeasonal.Location = new Point(9, 66);
      this.chkSeasonal.Name = "chkSeasonal";
      this.chkSeasonal.Size = new Size(70, 17);
      this.chkSeasonal.TabIndex = 2;
      this.chkSeasonal.Text = "Seasonal";
      this.chkSeasonal.UseVisualStyleBackColor = true;
      this.chkSeasonal.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.chkFullTime.AutoSize = true;
      this.chkFullTime.Location = new Point(9, 48);
      this.chkFullTime.Name = "chkFullTime";
      this.chkFullTime.Size = new Size(68, 17);
      this.chkFullTime.TabIndex = 1;
      this.chkFullTime.Text = "Full-Time";
      this.chkFullTime.UseVisualStyleBackColor = true;
      this.chkFullTime.CheckedChanged += new EventHandler(this.checkbox_CheckedChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label1.Location = new Point(6, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(114, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Employment Status";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add((Control) this.groupContainerBor);
      this.Name = nameof (VerificationEmploymentControl);
      this.Size = new Size(650, 114);
      this.groupContainerBor.ResumeLayout(false);
      this.groupContainerBor.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
