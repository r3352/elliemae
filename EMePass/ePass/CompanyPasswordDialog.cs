// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.CompanyPasswordDialog
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
  public class CompanyPasswordDialog : Form
  {
    private const string className = "CompanyPasswordDialog";
    private static readonly string sw = Tracing.SwEpass;
    private Sessions.Session session;
    private IContainer components;
    private Label lblEmail;
    private GroupBox group;
    private TextBox txtPassword;
    private Label lblPassword;
    private Button btnOK;
    private Button btnCancel;
    private Label lblInfo;
    private Button btnEmail;

    public CompanyPasswordDialog()
      : this(Session.DefaultInstance)
    {
    }

    public CompanyPasswordDialog(Sessions.Session session)
    {
      this.session = session;
      this.InitializeComponent();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string errorMsg = string.Empty;
      string str = EMNetworkPwdMgr.ValidateCompanyPassword(this.session.CompanyInfo.ClientID, this.txtPassword.Text, out errorMsg);
      if (errorMsg != string.Empty)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, errorMsg, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (str == "Invalid Company Password")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "The password you entered is incorrect. Please contact your administrator for the company password.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        this.session.ConfigurationManager.SetCompanySetting("CLIENT", "CLIENTPASSWORD", str);
        this.session.RecacheCompanyInfo();
        this.DialogResult = DialogResult.OK;
      }
    }

    private void btnEmail_Click(object sender, EventArgs e)
    {
      CompanyInfo companyInfo = this.session.CompanyInfo;
      UserInfo userInfo = this.session.UserInfo;
      string errorMsg = string.Empty;
      string text = EMNetworkPwdMgr.SendCompanyEmail(this.session.CompanyInfo, this.session.UserInfo, out errorMsg, this.session);
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
      this.lblEmail = new Label();
      this.group = new GroupBox();
      this.txtPassword = new TextBox();
      this.lblPassword = new Label();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.lblInfo = new Label();
      this.btnEmail = new Button();
      this.SuspendLayout();
      this.lblEmail.Location = new Point(9, 132);
      this.lblEmail.Name = "lblEmail";
      this.lblEmail.Size = new Size(413, 30);
      this.lblEmail.TabIndex = 6;
      this.lblEmail.Text = "If you do not know the company password, contact your Encompass administrator so they can reset it for you.";
      this.group.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.group.Location = new Point(9, 120);
      this.group.Name = "group";
      this.group.Size = new Size(416, 4);
      this.group.TabIndex = 5;
      this.group.TabStop = false;
      this.txtPassword.Location = new Point(124, 60);
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(300, 20);
      this.txtPassword.TabIndex = 2;
      this.lblPassword.Location = new Point(12, 64);
      this.lblPassword.Name = "lblPassword";
      this.lblPassword.Size = new Size(112, 16);
      this.lblPassword.TabIndex = 1;
      this.lblPassword.Text = "Company Password:";
      this.btnOK.Location = new Point(268, 88);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(76, 24);
      this.btnOK.TabIndex = 3;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(348, 88);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(76, 24);
      this.btnCancel.TabIndex = 4;
      this.btnCancel.Text = "Cancel";
      this.lblInfo.Location = new Point(12, 12);
      this.lblInfo.Name = "lblInfo";
      this.lblInfo.Size = new Size(416, 36);
      this.lblInfo.TabIndex = 0;
      this.lblInfo.Text = "Your company password is incorrect or has been changed by your administrator.  Please enter the current company password.";
      this.btnEmail.Location = new Point(268, 156);
      this.btnEmail.Name = "btnEmail";
      this.btnEmail.Size = new Size(156, 24);
      this.btnEmail.TabIndex = 7;
      this.btnEmail.Text = "Send Company Password";
      this.btnEmail.Visible = false;
      this.btnEmail.Click += new EventHandler(this.btnEmail_Click);
      this.AutoScaleDimensions = new SizeF(6f, 14f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(434, 168);
      this.Controls.Add((Control) this.lblEmail);
      this.Controls.Add((Control) this.group);
      this.Controls.Add((Control) this.btnEmail);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.lblPassword);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.lblInfo);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (CompanyPasswordDialog);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Enter Company Password";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
