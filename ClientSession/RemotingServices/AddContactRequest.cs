// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AddContactRequest
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class AddContactRequest : Form
  {
    private string requester;
    private Label label1;
    private RichTextBox rtBoxMsg;
    private Label label2;
    private Button btnAllowAndAdd;
    private Button btnAllow;
    private Button btnDeny;
    private Button btnViewProfile;
    private System.ComponentModel.Container components;

    public AddContactRequest(string fromUser, string message)
    {
      this.InitializeComponent();
      this.requester = fromUser;
      this.label1.Text = this.requester + this.label1.Text;
      this.rtBoxMsg.Text = message == null ? "" : message;
      this.btnViewProfile.Text = "View " + this.requester + "'s profile";
      if (!Session.MessengerListManager.ContainsContact(fromUser))
        return;
      this.btnAllowAndAdd.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.rtBoxMsg = new RichTextBox();
      this.label2 = new Label();
      this.btnAllowAndAdd = new Button();
      this.btnAllow = new Button();
      this.btnDeny = new Button();
      this.btnViewProfile = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(384, 40);
      this.label1.TabIndex = 0;
      this.label1.Text = " would like to add you to his/her Encompass Messenger List and has sent you the following request message";
      this.rtBoxMsg.Location = new Point(8, 56);
      this.rtBoxMsg.Name = "rtBoxMsg";
      this.rtBoxMsg.ReadOnly = true;
      this.rtBoxMsg.Size = new Size(384, 64);
      this.rtBoxMsg.TabIndex = 1;
      this.rtBoxMsg.Text = "";
      this.label2.Location = new Point(8, 128);
      this.label2.Name = "label2";
      this.label2.Size = new Size(384, 23);
      this.label2.TabIndex = 2;
      this.label2.Text = "Do you want to allow this person to add you to his/her Messenger List?";
      this.btnAllowAndAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAllowAndAdd.DialogResult = DialogResult.OK;
      this.btnAllowAndAdd.Location = new Point(50, 146);
      this.btnAllowAndAdd.Name = "btnAllowAndAdd";
      this.btnAllowAndAdd.Size = new Size(184, 23);
      this.btnAllowAndAdd.TabIndex = 3;
      this.btnAllowAndAdd.Text = "Allow && add to my Messenger List";
      this.btnAllowAndAdd.Click += new EventHandler(this.btnAllowAndAdd_Click);
      this.btnAllow.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnAllow.DialogResult = DialogResult.Yes;
      this.btnAllow.Location = new Point(242, 146);
      this.btnAllow.Name = "btnAllow";
      this.btnAllow.Size = new Size(72, 23);
      this.btnAllow.TabIndex = 4;
      this.btnAllow.Text = "Allow";
      this.btnAllow.Click += new EventHandler(this.btnAllow_Click);
      this.btnDeny.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnDeny.DialogResult = DialogResult.No;
      this.btnDeny.Location = new Point(322, 146);
      this.btnDeny.Name = "btnDeny";
      this.btnDeny.Size = new Size(72, 23);
      this.btnDeny.TabIndex = 5;
      this.btnDeny.Text = "Deny";
      this.btnDeny.Click += new EventHandler(this.btnDeny_Click);
      this.btnViewProfile.Location = new Point(248, 24);
      this.btnViewProfile.Name = "btnViewProfile";
      this.btnViewProfile.Size = new Size(144, 23);
      this.btnViewProfile.TabIndex = 6;
      this.btnViewProfile.Text = "View (user)'s profile";
      this.btnViewProfile.Click += new EventHandler(this.btnViewProfile_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(402, 176);
      this.Controls.Add((Control) this.btnViewProfile);
      this.Controls.Add((Control) this.btnDeny);
      this.Controls.Add((Control) this.btnAllow);
      this.Controls.Add((Control) this.btnAllowAndAdd);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.rtBoxMsg);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddContactRequest);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Add Contact Request";
      this.ResumeLayout(false);
    }

    private void btnDeny_Click(object sender, EventArgs e)
    {
      this.TopMost = false;
      DenyRequestDialog denyRequestDialog = new DenyRequestDialog(this.requester);
      denyRequestDialog.TopMost = true;
      int num = (int) denyRequestDialog.ShowDialog();
      denyRequestDialog.TopMost = false;
    }

    private void btnAllow_Click(object sender, EventArgs e)
    {
      this.TopMost = false;
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new IMControlMessage(Session.UserID, this.requester, IMMessage.MessageType.AllowAddToList, ""));
    }

    private void btnAllowAndAdd_Click(object sender, EventArgs e)
    {
      this.TopMost = false;
      this.btnAllow_Click((object) this.btnAllowAndAdd, e);
      EncompassMessenger.Instance.addContact(this.requester);
    }

    private void btnViewProfile_Click(object sender, EventArgs e)
    {
      UserInfo user = Session.OrganizationManager.GetUser(this.requester);
      if (user == (UserInfo) null)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "There is no contact profile for contact " + this.requester + ".", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else
      {
        int num2 = (int) new ContactMessengerProfile(user).ShowDialog((IWin32Window) this);
      }
    }
  }
}
