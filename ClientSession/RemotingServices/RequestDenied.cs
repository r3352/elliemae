// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.RequestDenied
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
  public class RequestDenied : Form
  {
    private string contactUserid;
    private Label label1;
    private Button btnOK;
    private Label label2;
    private RichTextBox rtBoxMsg;
    private System.ComponentModel.Container components;

    public RequestDenied(string contactUserid, string denyMsg)
    {
      this.InitializeComponent();
      this.contactUserid = contactUserid;
      this.label1.Text = this.contactUserid + this.label1.Text;
      this.label2.Text = this.contactUserid + this.label2.Text;
      this.rtBoxMsg.Text = denyMsg == null ? "" : denyMsg;
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
      this.btnOK = new Button();
      this.label2 = new Label();
      this.rtBoxMsg = new RichTextBox();
      this.SuspendLayout();
      this.label1.Location = new Point(8, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(384, 40);
      this.label1.TabIndex = 0;
      this.label1.Text = " has denied your request to add him/her to your Messenger List, and sent the following message";
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(168, 152);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.label2.Location = new Point(8, 128);
      this.label2.Name = "label2";
      this.label2.Size = new Size(384, 23);
      this.label2.TabIndex = 2;
      this.label2.Text = " has been removed from your Messenger List.";
      this.rtBoxMsg.Location = new Point(8, 48);
      this.rtBoxMsg.Name = "rtBoxMsg";
      this.rtBoxMsg.ReadOnly = true;
      this.rtBoxMsg.Size = new Size(384, 72);
      this.rtBoxMsg.TabIndex = 3;
      this.rtBoxMsg.Text = "";
      this.AutoScaleBaseSize = new Size(5, 13);
      this.ClientSize = new Size(400, 182);
      this.Controls.Add((Control) this.rtBoxMsg);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequestDenied);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (RequestDenied);
      this.ResumeLayout(false);
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) new IMControlMessage(Session.UserID, this.contactUserid, IMMessage.MessageType.DenyAck, ""));
    }
  }
}
