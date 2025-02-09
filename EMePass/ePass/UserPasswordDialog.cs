// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.UserPasswordDialog
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [ComVisible(false)]
  public class UserPasswordDialog : Form
  {
    private const string className = "UserPasswordDialog";
    private static readonly string sw = Tracing.SwEpass;
    private Sessions.Session session;
    private IContainer components;
    private Label lblContinue;
    private GroupBox grpContinue;
    private TextBox txtUser;
    private Label lblUser;
    private TextBox txtPassword;
    private Button btnContinue;
    private Label lblEmail;
    private GroupBox grpEmail;
    private Label lblInfo;
    private CheckBox chkRemember;
    private Label lblPassword;
    private Button btnOK;
    private Button btnCancel;

    public UserPasswordDialog(bool loginRequired)
      : this(loginRequired, Session.DefaultInstance)
    {
    }

    public UserPasswordDialog(bool loginRequired, Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
      this.txtUser.Text = this.session.UserID;
      if (!loginRequired)
        return;
      this.grpContinue.Visible = false;
      this.lblContinue.Visible = false;
      this.btnContinue.Visible = false;
      this.Height -= this.btnContinue.Bottom - this.lblEmail.Bottom;
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string errorMsg = string.Empty;
      if (EMNetworkPwdMgr.ChangeUserPassword(this.txtPassword.Text, this.session.Password, out errorMsg, this.session) == "Invalid User Password")
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The password you entered is incorrect.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
        this.DialogResult = DialogResult.OK;
    }

    private void btnEmail_Click(object sender, EventArgs e)
    {
      string errorMsg = string.Empty;
      string text = EMNetworkPwdMgr.SendUserEmail(this.session.CompanyInfo, this.session.UserInfo, out errorMsg, this.session);
      if (errorMsg != string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.lblContinue = new Label();
      this.grpContinue = new GroupBox();
      this.txtUser = new TextBox();
      this.lblUser = new Label();
      this.txtPassword = new TextBox();
      this.btnContinue = new Button();
      this.lblEmail = new Label();
      this.grpEmail = new GroupBox();
      this.lblInfo = new Label();
      this.chkRemember = new CheckBox();
      this.lblPassword = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.lblContinue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblContinue.Location = new Point(12, 206);
      this.lblContinue.Name = "lblContinue";
      this.lblContinue.Size = new Size(380, 28);
      this.lblContinue.TabIndex = 12;
      this.lblContinue.Text = "If you would like to continue with the company default settings, you will not be able to edit your providers list.";
      this.grpContinue.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpContinue.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpContinue.Location = new Point(8, 194);
      this.grpContinue.Name = "grpContinue";
      this.grpContinue.Size = new Size(384, 4);
      this.grpContinue.TabIndex = 11;
      this.grpContinue.TabStop = false;
      this.txtUser.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtUser.Location = new Point(104, 48);
      this.txtUser.Name = "txtUser";
      this.txtUser.ReadOnly = true;
      this.txtUser.Size = new Size(284, 20);
      this.txtUser.TabIndex = 2;
      this.txtUser.TabStop = false;
      this.lblUser.Location = new Point(36, 48);
      this.lblUser.Name = "lblUser";
      this.lblUser.Size = new Size(48, 20);
      this.lblUser.TabIndex = 1;
      this.lblUser.Text = "User ID:";
      this.lblUser.TextAlign = ContentAlignment.MiddleLeft;
      this.txtPassword.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtPassword.Location = new Point(104, 72);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(284, 20);
      this.txtPassword.TabIndex = 4;
      this.btnContinue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnContinue.DialogResult = DialogResult.Ignore;
      this.btnContinue.Location = new Point(168, 242);
      this.btnContinue.Name = "btnContinue";
      this.btnContinue.Size = new Size(220, 24);
      this.btnContinue.TabIndex = 13;
      this.btnContinue.Text = "Continue with Company Default Settings";
      this.lblEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblEmail.Location = new Point(12, 164);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(380, 28);
      this.lblEmail.TabIndex = 9;
      this.lblEmail.Text = "If you don't know (or can't remember) your old password, contact your Encompass administrator so they can reset it for you.";
      this.grpEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.grpEmail.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.grpEmail.Location = new Point(8, 152);
      this.grpEmail.Name = "grpEmail";
      this.grpEmail.Size = new Size(384, 4);
      this.grpEmail.TabIndex = 8;
      this.grpEmail.TabStop = false;
      this.lblInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.lblInfo.Location = new Point(12, 12);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new Size(380, 28);
      this.lblInfo.TabIndex = 0;
      this.lblInfo.Text = "Your password is different than the password on the server. Enter the correct password below.";
      this.chkRemember.Checked = true;
      this.chkRemember.CheckState = CheckState.Checked;
      this.chkRemember.Location = new Point(104, 96);
      this.chkRemember.Name = "chkRemember";
      this.chkRemember.Size = new Size(132, 16);
      this.chkRemember.TabIndex = 5;
      this.chkRemember.Text = "Remember Password";
      this.chkRemember.Visible = false;
      this.lblPassword.Location = new Point(3, 72);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new Size(81, 20);
      this.lblPassword.TabIndex = 3;
      this.lblPassword.Text = "Old Password:";
      this.lblPassword.TextAlign = ContentAlignment.MiddleLeft;
      this.btnOK.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnOK.Location = new Point(232, 120);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 24);
      this.btnOK.TabIndex = 6;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(312, 120);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 24);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Cancel";
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(402, 278);
      this.Controls.Add((Control) this.lblContinue);
      this.Controls.Add((Control) this.grpContinue);
      this.Controls.Add((Control) this.txtUser);
      this.Controls.Add((Control) this.lblUser);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.btnContinue);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.grpEmail);
      this.Controls.Add((Control) this.lblInfo);
      this.Controls.Add((Control) this.chkRemember);
      this.Controls.Add((Control) this.lblPassword);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (UserPasswordDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Enter Password";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
