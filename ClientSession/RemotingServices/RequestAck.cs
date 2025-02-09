// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.RequestAck
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer.Calendar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class RequestAck : Form
  {
    private CSControlMessage ackMsg;
    private IContainer components;
    private RichTextBox rtBoxMsg;
    private Label lblSub2;
    private Button btnOK;
    private Label lblSub1;
    private Panel panel1;

    public RequestAck(CSControlMessage ackMsg)
    {
      this.InitializeComponent();
      this.ackMsg = ackMsg;
      this.InitialPage();
    }

    private void InitialPage()
    {
      UserInfo user = Session.OrganizationManager.GetUser(this.ackMsg.FromUser);
      string str = "";
      if (user != (UserInfo) null)
        str = user.FullName;
      this.Text = "Message from " + str;
      switch (this.ackMsg.MsgType)
      {
        case CSMessage.MessageType.AllowAddToList:
          this.lblSub1.Text = "'" + str + "' has accepted your request to access their calendar and has sent the following message:";
          this.lblSub2.Text = "Your Access Level to '" + str + "'s' has been updated on your Contact list.";
          break;
        case CSMessage.MessageType.DenyAddToList:
          this.lblSub1.Text = "'" + str + "' has denied your request to access their calendar and has sent the following message:";
          this.lblSub2.Visible = false;
          break;
        case CSMessage.MessageType.ModifyAccess:
          this.lblSub1.Text = "'" + str + "' has modified your access level to their calendar and has sent the following message:";
          break;
        case CSMessage.MessageType.DeleteAccess:
          this.lblSub1.Text = "'" + str + "' has removed your access to their calendar and has sent the following message:";
          this.lblSub2.Visible = false;
          break;
        case CSMessage.MessageType.WithdrawAccess:
          this.lblSub1.Text = "'" + str + "' has removed their access to your calendar.  ";
          Label lblSub1 = this.lblSub1;
          lblSub1.Text = lblSub1.Text + "'" + str + "' will be removed from your Viewer list";
          this.rtBoxMsg.Visible = false;
          this.lblSub2.Visible = false;
          break;
      }
      this.rtBoxMsg.Text = this.ackMsg.Text;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      CSControlMessage csControlMessage = (CSControlMessage) null;
      switch (this.ackMsg.MsgType)
      {
        case CSMessage.MessageType.AllowAddToList:
          csControlMessage = new CSControlMessage(Session.UserID, this.ackMsg.FromUser, CSMessage.MessageType.AcceptAck, "");
          break;
        case CSMessage.MessageType.DenyAddToList:
          csControlMessage = new CSControlMessage(Session.UserID, this.ackMsg.FromUser, CSMessage.MessageType.DenyAck, "");
          break;
        case CSMessage.MessageType.ModifyAccess:
        case CSMessage.MessageType.DeleteAccess:
        case CSMessage.MessageType.WithdrawAccess:
          csControlMessage = new CSControlMessage(Session.UserID, this.ackMsg.FromUser, CSMessage.MessageType.ModifyAck, "");
          break;
      }
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) csControlMessage);
      this.DialogResult = DialogResult.OK;
    }

    private void RequestAck_FormClosing(object sender, FormClosingEventArgs e)
    {
      this.btnOK_Click((object) null, (EventArgs) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.rtBoxMsg = new RichTextBox();
      this.lblSub2 = new Label();
      this.btnOK = new Button();
      this.lblSub1 = new Label();
      this.panel1 = new Panel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.rtBoxMsg.Dock = DockStyle.Top;
      this.rtBoxMsg.Location = new Point(0, 34);
      this.rtBoxMsg.Name = "rtBoxMsg";
      this.rtBoxMsg.ReadOnly = true;
      this.rtBoxMsg.Size = new Size(381, 72);
      this.rtBoxMsg.TabIndex = 7;
      this.rtBoxMsg.Text = "";
      this.lblSub2.Dock = DockStyle.Top;
      this.lblSub2.Location = new Point(0, 106);
      this.lblSub2.Name = "lblSub2";
      this.lblSub2.Size = new Size(381, 40);
      this.lblSub2.TabIndex = 6;
      this.lblSub2.Text = " ";
      this.btnOK.Location = new Point(164, 161);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblSub1.Dock = DockStyle.Top;
      this.lblSub1.Location = new Point(0, 0);
      this.lblSub1.Name = "lblSub1";
      this.lblSub1.Size = new Size(381, 34);
      this.lblSub1.TabIndex = 4;
      this.panel1.Controls.Add((Control) this.lblSub2);
      this.panel1.Controls.Add((Control) this.rtBoxMsg);
      this.panel1.Controls.Add((Control) this.lblSub1);
      this.panel1.Location = new Point(12, 12);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(381, 140);
      this.panel1.TabIndex = 8;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(406, 188);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.btnOK);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequestAck);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calendar Sharing Request Acknowledgement";
      this.FormClosing += new FormClosingEventHandler(this.RequestAck_FormClosing);
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
