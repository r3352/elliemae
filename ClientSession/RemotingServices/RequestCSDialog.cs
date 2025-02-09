// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.RequestCSDialog
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
  public class RequestCSDialog : Form
  {
    private IContainer components;
    private TextBox txtMessage;
    private Button btnOK;
    private Label lblSub1;
    private RadioButton rbtReadOnly;
    private Label lblSub;
    private RadioButton rbtPartial;
    private RadioButton rbtFull;
    private Button btnCancel;
    private Label lblUser;
    private Label label1;
    private Label label2;

    public RequestCSDialog(string userName)
    {
      this.InitializeComponent();
      this.rbtReadOnly.Checked = true;
      this.lblUser.Text = userName;
    }

    public string Message => this.txtMessage.Text.Trim();

    public CSMessage.MessageType MessageType
    {
      get
      {
        if (this.rbtReadOnly.Checked)
          return CSMessage.MessageType.RequestReadOnlyCalendarAccess;
        return this.rbtPartial.Checked ? CSMessage.MessageType.RequestPartialCalendarAccess : CSMessage.MessageType.RequestFullCalendarAccess;
      }
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

    private void RequestCSDialog_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.btnCancel.PerformClick();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtMessage = new TextBox();
      this.btnOK = new Button();
      this.lblSub1 = new Label();
      this.rbtReadOnly = new RadioButton();
      this.lblSub = new Label();
      this.rbtPartial = new RadioButton();
      this.rbtFull = new RadioButton();
      this.btnCancel = new Button();
      this.lblUser = new Label();
      this.label1 = new Label();
      this.label2 = new Label();
      this.SuspendLayout();
      this.txtMessage.Location = new Point(12, 104);
      this.txtMessage.MaxLength = 512;
      this.txtMessage.Multiline = true;
      this.txtMessage.Name = "txtMessage";
      this.txtMessage.Size = new Size(407, 119);
      this.txtMessage.TabIndex = 0;
      this.btnOK.DialogResult = DialogResult.OK;
      this.btnOK.Location = new Point(265, 257);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.lblSub1.Location = new Point(9, 80);
      this.lblSub1.Name = "lblSub1";
      this.lblSub1.Size = new Size(410, 21);
      this.lblSub1.TabIndex = 2;
      this.lblSub1.Text = "Type a message (optional):";
      this.rbtReadOnly.AutoSize = true;
      this.rbtReadOnly.Location = new Point(13, 51);
      this.rbtReadOnly.Name = "rbtReadOnly";
      this.rbtReadOnly.Size = new Size(75, 17);
      this.rbtReadOnly.TabIndex = 3;
      this.rbtReadOnly.TabStop = true;
      this.rbtReadOnly.Text = "Read Only";
      this.rbtReadOnly.UseVisualStyleBackColor = true;
      this.lblSub.AutoSize = true;
      this.lblSub.Location = new Point(11, 31);
      this.lblSub.Name = "lblSub";
      this.lblSub.Size = new Size(114, 13);
      this.lblSub.TabIndex = 4;
      this.lblSub.Text = "Request Access Level";
      this.rbtPartial.AutoSize = true;
      this.rbtPartial.Location = new Point(95, 51);
      this.rbtPartial.Name = "rbtPartial";
      this.rbtPartial.Size = new Size(82, 17);
      this.rbtPartial.TabIndex = 5;
      this.rbtPartial.TabStop = true;
      this.rbtPartial.Text = "Partial (Add)";
      this.rbtPartial.UseVisualStyleBackColor = true;
      this.rbtFull.AutoSize = true;
      this.rbtFull.Location = new Point(184, 51);
      this.rbtFull.Name = "rbtFull";
      this.rbtFull.Size = new Size(92, 17);
      this.rbtFull.TabIndex = 6;
      this.rbtFull.TabStop = true;
      this.rbtFull.Text = "Full (Add/Edit)";
      this.rbtFull.UseVisualStyleBackColor = true;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(346, 257);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 23);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.lblUser.AutoSize = true;
      this.lblUser.ForeColor = Color.Red;
      this.lblUser.Location = new Point(48, 9);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(55, 13);
      this.lblUser.TabIndex = 8;
      this.lblUser.Text = "userName";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(10, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(32, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "User:";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 226);
      this.label2.Name = "label2";
      this.label2.Size = new Size(223, 13);
      this.label2.TabIndex = 11;
      this.label2.Text = "Your request will be sent to the specified user.";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(431, 292);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.lblUser);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.rbtFull);
      this.Controls.Add((Control) this.rbtPartial);
      this.Controls.Add((Control) this.lblSub);
      this.Controls.Add((Control) this.rbtReadOnly);
      this.Controls.Add((Control) this.lblSub1);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtMessage);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (RequestCSDialog);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Request Dialog";
      this.KeyUp += new KeyEventHandler(this.RequestCSDialog_KeyUp);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
