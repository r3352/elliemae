// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.CampaignUI.CopyCampaignDialog
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ContactUI.CampaignUI
{
  public class CopyCampaignDialog : Form
  {
    private Panel pnlButtons;
    private Label lblSeparator;
    private Button btnCancel;
    private Button btnOK;
    protected Panel pnlBrokenRules;
    private Label lblBrokenRulesSeparator;
    private Label lblBrokenRules;
    protected ListBox lstBrokenRules;
    private Panel pnlMainContent;
    private TextBox txtName;
    private Label lblName;
    private ErrorProvider errBrokenRules;
    private IContainer components;
    private EllieMae.EMLite.Campaign.Campaign campaign;
    private string newCampaignName;
    private TextBox txtDesc;
    private Label lblDesc;
    private CheckBox chkCopyContacts;
    private string newCampaignDesc;

    public CopyCampaignDialog(EllieMae.EMLite.Campaign.Campaign campaign)
    {
      this.InitializeComponent();
      this.campaign = campaign;
      this.txtName.Text = "Copy of " + campaign.CampaignName;
      this.txtDesc.Text = campaign.CampaignDesc;
      this.FindForm().Height -= this.pnlBrokenRules.Height;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      this.newCampaignName = this.txtName.Text.Trim();
      this.newCampaignDesc = this.txtDesc.Text.Trim();
      if (!this.isValid())
        return;
      try
      {
        EllieMae.EMLite.Campaign.Campaign.CopyCampaign(this.campaign.CampaignId, this.chkCopyContacts.Checked, this.newCampaignName, this.newCampaignDesc, Session.SessionObjects);
        this.clearBrokenRules();
        int num = (int) Utils.Dialog((IWin32Window) this, "Campaign was successfully duplicated.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        if (0 <= ex.Message.IndexOf("Violation of UNIQUE KEY constraint 'UK_Campaign_UserId_CampaignName'"))
        {
          this.displayBrokenRules(new string[1]
          {
            "This Campaign name already exists"
          });
          return;
        }
        throw;
      }
      this.DialogResult = DialogResult.OK;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private bool isValid()
    {
      ArrayList arrayList = new ArrayList();
      if (this.newCampaignName.Length < 1)
        arrayList.Add((object) "Campaign Name is a required field");
      if (this.newCampaignName.Length > 64)
        arrayList.Add((object) "Campaign Name exceeds 64 characters");
      if (this.newCampaignDesc.Length > 250)
        arrayList.Add((object) "Campaign Description exceeds 250 characters");
      if (0 < arrayList.Count)
        this.displayBrokenRules((string[]) arrayList.ToArray(typeof (string)));
      return arrayList.Count == 0;
    }

    private void displayBrokenRules(string[] brokenRules)
    {
      if (brokenRules == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string brokenRule in brokenRules)
        stringBuilder.Append(brokenRule + ".\n");
      int num = (int) Utils.Dialog((IWin32Window) this, stringBuilder.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void clearBrokenRules()
    {
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
      this.pnlButtons = new Panel();
      this.lblSeparator = new Label();
      this.btnCancel = new Button();
      this.btnOK = new Button();
      this.pnlBrokenRules = new Panel();
      this.lblBrokenRulesSeparator = new Label();
      this.lblBrokenRules = new Label();
      this.lstBrokenRules = new ListBox();
      this.errBrokenRules = new ErrorProvider(this.components);
      this.pnlMainContent = new Panel();
      this.chkCopyContacts = new CheckBox();
      this.txtDesc = new TextBox();
      this.txtName = new TextBox();
      this.lblDesc = new Label();
      this.lblName = new Label();
      this.pnlButtons.SuspendLayout();
      this.pnlBrokenRules.SuspendLayout();
      ((ISupportInitialize) this.errBrokenRules).BeginInit();
      this.pnlMainContent.SuspendLayout();
      this.SuspendLayout();
      this.pnlButtons.Controls.Add((Control) this.lblSeparator);
      this.pnlButtons.Controls.Add((Control) this.btnCancel);
      this.pnlButtons.Controls.Add((Control) this.btnOK);
      this.pnlButtons.Dock = DockStyle.Bottom;
      this.pnlButtons.Location = new Point(0, 186);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new Size(580, 32);
      this.pnlButtons.TabIndex = 11;
      this.lblSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblSeparator.Location = new Point(7, 0);
      this.lblSeparator.Name = "lblSeparator";
      this.lblSeparator.Size = new Size(566, 1);
      this.lblSeparator.TabIndex = 2;
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(498, 5);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(421, 5);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 0;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.pnlBrokenRules.Controls.Add((Control) this.lblBrokenRulesSeparator);
      this.pnlBrokenRules.Controls.Add((Control) this.lblBrokenRules);
      this.pnlBrokenRules.Controls.Add((Control) this.lstBrokenRules);
      this.pnlBrokenRules.Dock = DockStyle.Bottom;
      this.pnlBrokenRules.Location = new Point(0, 110);
      this.pnlBrokenRules.Name = "pnlBrokenRules";
      this.pnlBrokenRules.Size = new Size(580, 76);
      this.pnlBrokenRules.TabIndex = 12;
      this.pnlBrokenRules.Visible = false;
      this.lblBrokenRulesSeparator.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblBrokenRulesSeparator.BorderStyle = BorderStyle.FixedSingle;
      this.lblBrokenRulesSeparator.Location = new Point(7, 0);
      this.lblBrokenRulesSeparator.Name = "lblBrokenRulesSeparator";
      this.lblBrokenRulesSeparator.Size = new Size(566, 1);
      this.lblBrokenRulesSeparator.TabIndex = 13;
      this.lblBrokenRules.Location = new Point(7, 5);
      this.lblBrokenRules.Name = "lblBrokenRules";
      this.lblBrokenRules.Size = new Size(204, 23);
      this.lblBrokenRules.TabIndex = 12;
      this.lblBrokenRules.Text = "The following problems were identified:";
      this.lblBrokenRules.TextAlign = ContentAlignment.MiddleLeft;
      this.lstBrokenRules.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.lstBrokenRules.Location = new Point(7, 29);
      this.lstBrokenRules.Name = "lstBrokenRules";
      this.lstBrokenRules.Size = new Size(566, 43);
      this.lstBrokenRules.TabIndex = 11;
      this.errBrokenRules.ContainerControl = (ContainerControl) this;
      this.pnlMainContent.Controls.Add((Control) this.chkCopyContacts);
      this.pnlMainContent.Controls.Add((Control) this.txtDesc);
      this.pnlMainContent.Controls.Add((Control) this.txtName);
      this.pnlMainContent.Controls.Add((Control) this.lblDesc);
      this.pnlMainContent.Controls.Add((Control) this.lblName);
      this.pnlMainContent.Dock = DockStyle.Fill;
      this.pnlMainContent.Location = new Point(0, 0);
      this.pnlMainContent.Name = "pnlMainContent";
      this.pnlMainContent.Size = new Size(580, 110);
      this.pnlMainContent.TabIndex = 13;
      this.chkCopyContacts.Location = new Point(73, 83);
      this.chkCopyContacts.Name = "chkCopyContacts";
      this.chkCopyContacts.Size = new Size(191, 24);
      this.chkCopyContacts.TabIndex = 10;
      this.chkCopyContacts.Text = "Copy Campaign Contacts.";
      this.txtDesc.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtDesc.Location = new Point(73, 32);
      this.txtDesc.Multiline = true;
      this.txtDesc.Name = "txtDesc";
      this.txtDesc.Size = new Size(500, 47);
      this.txtDesc.TabIndex = 9;
      this.txtName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtName.Location = new Point(73, 5);
      this.txtName.Name = "txtName";
      this.txtName.Size = new Size(500, 20);
      this.txtName.TabIndex = 8;
      this.lblDesc.Location = new Point(7, 31);
      this.lblDesc.Name = "lblDesc";
      this.lblDesc.Size = new Size(66, 23);
      this.lblDesc.TabIndex = 7;
      this.lblDesc.Text = "Description:";
      this.lblDesc.TextAlign = ContentAlignment.MiddleLeft;
      this.lblName.Location = new Point(7, 4);
      this.lblName.Name = "lblName";
      this.lblName.Size = new Size(66, 23);
      this.lblName.TabIndex = 6;
      this.lblName.Text = "Name:";
      this.lblName.TextAlign = ContentAlignment.MiddleLeft;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(580, 218);
      this.Controls.Add((Control) this.pnlMainContent);
      this.Controls.Add((Control) this.pnlBrokenRules);
      this.Controls.Add((Control) this.pnlButtons);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(586, 250);
      this.MinimizeBox = false;
      this.Name = nameof (CopyCampaignDialog);
      this.ShowInTaskbar = false;
      this.Text = "Duplicate Campaign";
      this.KeyUp += new KeyEventHandler(this.CopyCampaignDialog_KeyUp);
      this.pnlButtons.ResumeLayout(false);
      this.pnlBrokenRules.ResumeLayout(false);
      ((ISupportInitialize) this.errBrokenRules).EndInit();
      this.pnlMainContent.ResumeLayout(false);
      this.pnlMainContent.PerformLayout();
      this.ResumeLayout(false);
    }

    private void CopyCampaignDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }
  }
}
