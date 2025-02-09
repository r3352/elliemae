// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.ActivityValidationDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer.Campaign;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class ActivityValidationDialog : Form
  {
    private System.ComponentModel.Container components;
    private Panel pnlButtons;
    private Button btnCancel;
    private Button btnOK;
    private Panel pnlMain;
    private Label lblOptOutContacts;
    private Label lblNoAddressContacts;
    private Label lblOptOutCount;
    private Label lblNoAddressCount;
    private Label lblButtonsSeparator;
    private Panel pnlHeading;
    private Label lblHeadingSeparator;
    private Panel pnlNoAddress;
    private Panel pnlOptOut;
    private RadioButton rbSkipOptOut;
    private RadioButton rbProcessOptOut;
    private RadioButton rbSkipNoAddress;
    private RadioButton rbProcessNoAddress;
    private Label lblFailedValidation;

    public bool SkipOptOutContacts => this.rbSkipOptOut.Checked;

    public bool ProceedOptOutContacts => this.rbProcessOptOut.Checked;

    public bool SkipNoAddressContacts => this.rbSkipNoAddress.Checked;

    public ActivityValidationDialog(
      ActivityType activityType,
      int[] optOutContactIds,
      int[] noAddressContactIds)
    {
      this.InitializeComponent();
      int length1 = optOutContactIds != null ? optOutContactIds.Length : 0;
      int length2 = noAddressContactIds != null ? noAddressContactIds.Length : 0;
      this.lblFailedValidation.Text = string.Format("{0}  Contacts Did Not Pass Validation.", (object) (length1 + length2));
      int num = this.FindForm().Height - this.pnlOptOut.Height - this.pnlNoAddress.Height;
      if (0 < length2)
      {
        this.pnlNoAddress.Visible = true;
        num += this.pnlNoAddress.Height;
        this.lblNoAddressCount.Text = length2.ToString();
      }
      if (0 < length1)
      {
        this.pnlOptOut.Visible = true;
        num += this.pnlOptOut.Height;
        this.lblOptOutCount.Text = length1.ToString();
        this.lblOptOutContacts.Text = "opted out from " + (activityType == ActivityType.Email ? "emails" : (ActivityType.Fax == activityType ? "faxes" : "phone calls"));
      }
      this.FindForm().Height = num;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void InitializeComponent()
    {
      this.pnlButtons = new Panel();
      this.lblButtonsSeparator = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.pnlMain = new Panel();
      this.pnlOptOut = new Panel();
      this.rbSkipOptOut = new RadioButton();
      this.rbProcessOptOut = new RadioButton();
      this.lblOptOutCount = new Label();
      this.lblOptOutContacts = new Label();
      this.pnlNoAddress = new Panel();
      this.rbSkipNoAddress = new RadioButton();
      this.rbProcessNoAddress = new RadioButton();
      this.lblNoAddressContacts = new Label();
      this.lblNoAddressCount = new Label();
      this.pnlHeading = new Panel();
      this.lblHeadingSeparator = new Label();
      this.lblFailedValidation = new Label();
      this.pnlButtons.SuspendLayout();
      this.pnlMain.SuspendLayout();
      this.pnlOptOut.SuspendLayout();
      this.pnlNoAddress.SuspendLayout();
      this.pnlHeading.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.lblButtonsSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnOK);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 166);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(456, 32);
      this.pnlButtons.TabIndex = 1;
      this.lblButtonsSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblButtonsSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblButtonsSeparator.Location = new Point(6, 0);
      this.lblButtonsSeparator.Name = "lblButtonsSeparator";
      this.lblButtonsSeparator.Size = new Size(444, 1);
      this.lblButtonsSeparator.TabIndex = 2;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(375, 4);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(296, 4);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.pnlMain.Controls.Add((Control) this.pnlOptOut);
      this.pnlMain.Controls.Add((Control) this.pnlNoAddress);
      this.pnlMain.Dock = DockStyle.Fill;
      this.pnlMain.Location = new Point(0, 32);
      this.pnlMain.Name = "pnlMain";
      this.pnlMain.Size = new Size(456, 134);
      this.pnlMain.TabIndex = 2;
      this.pnlOptOut.Controls.Add((Control) this.rbSkipOptOut);
      this.pnlOptOut.Controls.Add((Control) this.rbProcessOptOut);
      this.pnlOptOut.Controls.Add((Control) this.lblOptOutCount);
      this.pnlOptOut.Controls.Add((Control) this.lblOptOutContacts);
      this.pnlOptOut.Dock = DockStyle.Top;
      this.pnlOptOut.Location = new Point(0, 67);
      this.pnlOptOut.Name = "pnlOptOut";
      this.pnlOptOut.Size = new Size(456, 67);
      this.pnlOptOut.TabIndex = 12;
      this.pnlOptOut.Visible = false;
      this.rbSkipOptOut.Location = new Point(54, 22);
      this.rbSkipOptOut.Name = "rbSkipOptOut";
      this.rbSkipOptOut.Size = new Size(202, 23);
      this.rbSkipOptOut.TabIndex = 11;
      this.rbSkipOptOut.Text = "Do not process at this time.";
      this.rbProcessOptOut.Checked = true;
      this.rbProcessOptOut.Location = new Point(54, 42);
      this.rbProcessOptOut.Name = "rbProcessOptOut";
      this.rbProcessOptOut.Size = new Size(202, 23);
      this.rbProcessOptOut.TabIndex = 10;
      this.rbProcessOptOut.TabStop = true;
      this.rbProcessOptOut.Text = "Remove from step.";
      this.lblOptOutCount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblOptOutCount.Location = new Point(6, 2);
      this.lblOptOutCount.Name = "lblOptOutCount";
      this.lblOptOutCount.Size = new Size(48, 23);
      this.lblOptOutCount.TabIndex = 5;
      this.lblOptOutCount.Text = "250";
      this.lblOptOutCount.TextAlign = ContentAlignment.MiddleRight;
      this.lblOptOutContacts.Location = new Point(54, 2);
      this.lblOptOutContacts.Name = "lblOptOutContacts";
      this.lblOptOutContacts.Size = new Size(202, 23);
      this.lblOptOutContacts.TabIndex = 2;
      this.lblOptOutContacts.Text = "opted out from emails:";
      this.lblOptOutContacts.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlNoAddress.Controls.Add((Control) this.rbSkipNoAddress);
      this.pnlNoAddress.Controls.Add((Control) this.rbProcessNoAddress);
      this.pnlNoAddress.Controls.Add((Control) this.lblNoAddressContacts);
      this.pnlNoAddress.Controls.Add((Control) this.lblNoAddressCount);
      this.pnlNoAddress.Dock = DockStyle.Top;
      this.pnlNoAddress.Location = new Point(0, 0);
      this.pnlNoAddress.Name = "pnlNoAddress";
      this.pnlNoAddress.Size = new Size(456, 67);
      this.pnlNoAddress.TabIndex = 13;
      this.pnlNoAddress.Visible = false;
      this.rbSkipNoAddress.Checked = true;
      this.rbSkipNoAddress.Location = new Point(54, 22);
      this.rbSkipNoAddress.Name = "rbSkipNoAddress";
      this.rbSkipNoAddress.Size = new Size(202, 23);
      this.rbSkipNoAddress.TabIndex = 13;
      this.rbSkipNoAddress.TabStop = true;
      this.rbSkipNoAddress.Text = "Do not process at this time.";
      this.rbProcessNoAddress.Location = new Point(54, 42);
      this.rbProcessNoAddress.Name = "rbProcessNoAddress";
      this.rbProcessNoAddress.Size = new Size(202, 23);
      this.rbProcessNoAddress.TabIndex = 12;
      this.rbProcessNoAddress.Text = "Remove from step.";
      this.lblNoAddressContacts.Location = new Point(54, 2);
      this.lblNoAddressContacts.Name = "lblNoAddressContacts";
      this.lblNoAddressContacts.Size = new Size(202, 23);
      this.lblNoAddressContacts.TabIndex = 3;
      this.lblNoAddressContacts.Text = "contacts with no email address:";
      this.lblNoAddressContacts.TextAlign = ContentAlignment.MiddleLeft;
      this.lblNoAddressCount.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblNoAddressCount.Location = new Point(6, 2);
      this.lblNoAddressCount.Name = "lblNoAddressCount";
      this.lblNoAddressCount.Size = new Size(48, 23);
      this.lblNoAddressCount.TabIndex = 6;
      this.lblNoAddressCount.Text = "50";
      this.lblNoAddressCount.TextAlign = ContentAlignment.MiddleRight;
      this.pnlHeading.Controls.Add((Control) this.lblHeadingSeparator);
      this.pnlHeading.Controls.Add((Control) this.lblFailedValidation);
      this.pnlHeading.Dock = DockStyle.Top;
      this.pnlHeading.Location = new Point(0, 0);
      this.pnlHeading.Name = "pnlHeading";
      this.pnlHeading.Size = new Size(456, 32);
      this.pnlHeading.TabIndex = 3;
      this.lblHeadingSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblHeadingSeparator.Location = new Point(6, 31);
      this.lblHeadingSeparator.Name = "lblHeadingSeparator";
      this.lblHeadingSeparator.Size = new Size(444, 1);
      this.lblHeadingSeparator.TabIndex = 5;
      this.lblFailedValidation.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.lblFailedValidation.Location = new Point(6, 6);
      this.lblFailedValidation.Name = "lblFailedValidation";
      this.lblFailedValidation.Size = new Size(320, 23);
      this.lblFailedValidation.TabIndex = 4;
      this.lblFailedValidation.Text = "9999  Contacts Did Not Pass Validation.";
      this.lblFailedValidation.TextAlign = ContentAlignment.MiddleLeft;
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(456, 198);
      this.Controls.Add((Control) this.pnlMain);
      this.Controls.Add((Control) this.pnlHeading);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(464, 232);
      this.MinimizeBox = false;
      this.Name = nameof (ActivityValidationDialog);
      this.ShowInTaskbar = false;
      this.Text = "Encompass";
      this.KeyUp += new KeyEventHandler(this.ActivityValidationDialog_KeyUp);
      this.pnlButtons.ResumeLayout(false);
      this.pnlMain.ResumeLayout(false);
      this.pnlOptOut.ResumeLayout(false);
      this.pnlNoAddress.ResumeLayout(false);
      this.pnlHeading.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    private void ActivityValidationDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
