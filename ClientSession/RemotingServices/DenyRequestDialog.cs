// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.DenyRequestDialog
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.ClientServer;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  public class DenyRequestDialog : Form
  {
    private string requester;
    private Label label1;
    private RichTextBox rtBoxMsg;
    private Button btnSend;
    private System.ComponentModel.Container components;

    public DenyRequestDialog(string requester)
    {
      this.InitializeComponent();
      this.requester = requester;
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
      this.btnSend = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(384, 40);
      this.label1.TabIndex = 0;
      this.label1.Text = "Enter a message describing why you denied the request to add you to his/her Messenger List (optional)";
      this.rtBoxMsg.Location = new Point(8, 48);
      this.rtBoxMsg.Name = "rtBoxMsg";
      this.rtBoxMsg.Size = new Size(384, 96);
      this.rtBoxMsg.TabIndex = 1;
      this.rtBoxMsg.Text = "";
      this.btnSend.DialogResult = DialogResult.OK;
      this.btnSend.Location = new Point(160, 152);
      this.btnSend.Name = "btnSend";
      this.btnSend.Size = new Size(75, 23);
      this.btnSend.TabIndex = 2;
      this.btnSend.Text = "Send";
      this.btnSend.Click += new EventHandler(this.btnSend_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(400, 184);
      this.Controls.Add((Control) this.btnSend);
      this.Controls.Add((Control) this.rtBoxMsg);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (DenyRequestDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Deny Request";
      this.ResumeLayout(false);
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new IMControlMessage(Session.UserID, this.requester, IMMessage.MessageType.DenyAddToList, this.rtBoxMsg.Text));
    }
  }
}
