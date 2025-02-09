// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.UI.ChangePasswordDialog
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.UI
{
  public class ChangePasswordDialog : Form
  {
    private Label label1;
    private Label label2;
    private Label label3;
    private TextBox txtPassword;
    private TextBox txtConfirm;
    private Button btnOK;
    private Button btnCancel;
    private System.ComponentModel.Container components;

    public ChangePasswordDialog() => this.InitializeComponent();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.txtPassword = new TextBox();
      this.txtConfirm = new TextBox();
      this.btnOK = new Button();
      this.btnCancel = new Button();
      this.SuspendLayout();
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(308, 34);
      this.label1.TabIndex = 0;
      this.label1.Text = "Your password has expired or is no longer valid. Enter a new password in the spaces below.";
      this.label2.Location = new Point(64, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(62, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password";
      this.label3.Location = new Point(64, 77);
      this.label3.Name = "label3";
      this.label3.Size = new Size(62, 16);
      this.label3.TabIndex = 2;
      this.label3.Text = "Confirm";
      this.txtPassword.Location = new Point(128, 53);
      this.txtPassword.MaxLength = 50;
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(142, 20);
      this.txtPassword.TabIndex = 3;
      this.txtConfirm.Location = new Point(128, 75);
      this.txtConfirm.MaxLength = 50;
      this.txtConfirm.Name = "txtConfirm";
      this.txtConfirm.PasswordChar = '*';
      this.txtConfirm.Size = new Size(142, 20);
      this.txtConfirm.TabIndex = 4;
      this.btnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOK.Location = new Point(171, 113);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new Size(75, 22);
      this.btnOK.TabIndex = 5;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new EventHandler(this.btnOK_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(250, 113);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(75, 22);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "&Cancel";
      this.AcceptButton = (IButtonControl) this.btnOK;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(334, 144);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOK);
      this.Controls.Add((Control) this.txtConfirm);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangePasswordDialog);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Login";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void btnOK_Click(object sender, EventArgs e)
    {
      string password = Session.Password;
      if (this.txtPassword.Text != this.txtConfirm.Text)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The two passwords you entered do not match. Correct this problem and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        this.resetPassword();
      }
      else
      {
        string text = this.txtPassword.Text;
        PwdRuleValidator passwordValidator = Session.OrganizationManager.GetPasswordValidator();
        if (!passwordValidator.CheckMinLength(text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The password must be at least " + (object) passwordValidator.MinimumLength + " characters long.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.resetPassword();
        }
        else if (!passwordValidator.CheckCompositionRule(text))
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "The password must contain the following:" + Environment.NewLine + Environment.NewLine + passwordValidator.GetCompositionRuleDescription(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          this.resetPassword();
        }
        else
        {
          try
          {
            Session.User.ChangePassword(text);
            string empty = string.Empty;
            this.DialogResult = DialogResult.OK;
          }
          catch (SecurityException ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.resetPassword();
          }
          catch (Exception ex)
          {
            int num = (int) Utils.Dialog((IWin32Window) this, "An error has occurred attempting to save your password: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          }
        }
      }
    }

    private void resetPassword()
    {
      this.txtPassword.Text = "";
      this.txtConfirm.Text = "";
      this.txtPassword.Focus();
    }

    public class EPassPasswordChangedEventArgs : EventArgs
    {
      public string Data { get; set; }
    }
  }
}
