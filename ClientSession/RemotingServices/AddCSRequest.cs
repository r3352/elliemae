// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.AddCSRequest
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
  public class AddCSRequest : Form
  {
    private CSControlMessage msg;
    private IContainer components;
    private Label lblSub1;
    private TextBox txtReqMsg;
    private Button btnAllow;
    private Button btnDeny;
    private Label lblQuestion;
    private RadioButton rbtFull;
    private RadioButton rbtPartial;
    private Label lblSub;
    private RadioButton rbtReadOnly;
    private TextBox txtReply;

    public AddCSRequest(CSControlMessage msg)
    {
      this.InitializeComponent();
      this.msg = msg;
      this.InitialPage();
    }

    private void InitialPage()
    {
      UserInfo user = Session.OrganizationManager.GetUser(this.msg.FromUser);
      string str = "";
      if (user != (UserInfo) null)
        str = user.FullName;
      this.Text = "Message from " + str;
      this.lblSub1.Text = "'" + str + "'" + this.lblSub1.Text;
      this.txtReqMsg.Text = this.msg.Text;
      if (this.msg.MsgType == CSMessage.MessageType.RequestReadOnlyCalendarAccess)
        this.rbtReadOnly.Checked = true;
      else if (this.msg.MsgType == CSMessage.MessageType.RequestPartialCalendarAccess)
        this.rbtPartial.Checked = true;
      else
        this.rbtFull.Checked = true;
    }

    private void btnAllow_Click(object sender, EventArgs e)
    {
      CSMessage.AccessLevel accessLevel = CSMessage.AccessLevel.ReadOnly;
      if (this.rbtFull.Checked)
        accessLevel = CSMessage.AccessLevel.Full;
      else if (this.rbtPartial.Checked)
        accessLevel = CSMessage.AccessLevel.Partial;
      CSControlMessage ackMsg = new CSControlMessage(Session.UserID, this.msg.FromUser, CSMessage.MessageType.AllowAddToList, this.txtReply.Text.Trim());
      Session.CalendarManager.UpdateContact(this.msg.ToUser, this.msg.FromUser, accessLevel, ackMsg);
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) ackMsg);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void btnDeny_Click(object sender, EventArgs e)
    {
      CSControlMessage csControlMessage = new CSControlMessage(Session.UserID, this.msg.FromUser, CSMessage.MessageType.DenyAddToList, this.txtReply.Text.Trim());
      Session.CalendarManager.DeleteContact(this.msg.ToUser, this.msg.FromUser);
      Session.ServerManager.SendMessage((EllieMae.EMLite.ClientServer.Message) csControlMessage);
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblSub1 = new Label();
      this.txtReqMsg = new TextBox();
      this.btnAllow = new Button();
      this.btnDeny = new Button();
      this.lblQuestion = new Label();
      this.rbtFull = new RadioButton();
      this.rbtPartial = new RadioButton();
      this.lblSub = new Label();
      this.rbtReadOnly = new RadioButton();
      this.txtReply = new TextBox();
      this.SuspendLayout();
      this.lblSub1.Location = new Point(13, 13);
      this.lblSub1.Name = "lblSub1";
      this.lblSub1.Size = new Size(407, 33);
      this.lblSub1.TabIndex = 0;
      this.lblSub1.Text = " is requesting access to your calendar and has sent the following message:";
      this.txtReqMsg.Location = new Point(13, 50);
      this.txtReqMsg.Multiline = true;
      this.txtReqMsg.Name = "txtReqMsg";
      this.txtReqMsg.ReadOnly = true;
      this.txtReqMsg.Size = new Size(405, 119);
      this.txtReqMsg.TabIndex = 1;
      this.btnAllow.Location = new Point(260, 390);
      this.btnAllow.Name = "btnAllow";
      this.btnAllow.Size = new Size(75, 23);
      this.btnAllow.TabIndex = 2;
      this.btnAllow.Text = "Allow";
      this.btnAllow.UseVisualStyleBackColor = true;
      this.btnAllow.Click += new EventHandler(this.btnAllow_Click);
      this.btnDeny.Location = new Point(345, 390);
      this.btnDeny.Name = "btnDeny";
      this.btnDeny.Size = new Size(75, 23);
      this.btnDeny.TabIndex = 3;
      this.btnDeny.Text = "Deny";
      this.btnDeny.UseVisualStyleBackColor = true;
      this.btnDeny.Click += new EventHandler(this.btnDeny_Click);
      this.lblQuestion.Location = new Point(13, 231);
      this.lblQuestion.Name = "lblQuestion";
      this.lblQuestion.Size = new Size(403, 34);
      this.lblQuestion.TabIndex = 4;
      this.lblQuestion.Text = "Type a message explaining why you denied/accepted this request to grant access to your calendar (optional):";
      this.rbtFull.AutoSize = true;
      this.rbtFull.Location = new Point(184, 200);
      this.rbtFull.Name = "rbtFull";
      this.rbtFull.Size = new Size(92, 17);
      this.rbtFull.TabIndex = 10;
      this.rbtFull.TabStop = true;
      this.rbtFull.Text = "Full (Add/Edit)";
      this.rbtFull.UseVisualStyleBackColor = true;
      this.rbtPartial.AutoSize = true;
      this.rbtPartial.Location = new Point(95, 200);
      this.rbtPartial.Name = "rbtPartial";
      this.rbtPartial.Size = new Size(82, 17);
      this.rbtPartial.TabIndex = 9;
      this.rbtPartial.TabStop = true;
      this.rbtPartial.Text = "Partial (Add)";
      this.rbtPartial.UseVisualStyleBackColor = true;
      this.lblSub.AutoSize = true;
      this.lblSub.Location = new Point(11, 180);
      this.lblSub.Name = "lblSub";
      this.lblSub.Size = new Size(71, 13);
      this.lblSub.TabIndex = 8;
      this.lblSub.Text = "Access Level";
      this.rbtReadOnly.AutoSize = true;
      this.rbtReadOnly.Location = new Point(13, 200);
      this.rbtReadOnly.Name = "rbtReadOnly";
      this.rbtReadOnly.Size = new Size(75, 17);
      this.rbtReadOnly.TabIndex = 7;
      this.rbtReadOnly.TabStop = true;
      this.rbtReadOnly.Text = "Read Only";
      this.rbtReadOnly.UseVisualStyleBackColor = true;
      this.txtReply.Location = new Point(13, 268);
      this.txtReply.MaxLength = 512;
      this.txtReply.Multiline = true;
      this.txtReply.Name = "txtReply";
      this.txtReply.Size = new Size(404, 116);
      this.txtReply.TabIndex = 11;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(432, 421);
      this.Controls.Add((Control) this.txtReply);
      this.Controls.Add((Control) this.rbtFull);
      this.Controls.Add((Control) this.rbtPartial);
      this.Controls.Add((Control) this.lblSub);
      this.Controls.Add((Control) this.rbtReadOnly);
      this.Controls.Add((Control) this.lblQuestion);
      this.Controls.Add((Control) this.btnDeny);
      this.Controls.Add((Control) this.btnAllow);
      this.Controls.Add((Control) this.txtReqMsg);
      this.Controls.Add((Control) this.lblSub1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (AddCSRequest);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Calendar Sharing Request";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
