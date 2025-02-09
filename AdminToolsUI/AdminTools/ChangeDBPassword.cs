// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.ChangeDBPassword
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class ChangeDBPassword : Form
  {
    private System.ComponentModel.Container components;
    private Button cancelBtn;
    private Button okBtn;
    private TextBox txtConfirmPassword;
    private Label label3;
    private TextBox txtOldPassword;
    private TextBox txtNewPassword;
    private TextBox txtUserID;
    private Label label5;
    private Label label4;
    private Label label2;
    private Label label1;

    public ChangeDBPassword()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.txtUserID.Text = EnConfigurationSettings.GlobalSettings.DatabaseUserID;
      if (!EnConfigurationSettings.GlobalSettings.DatabasePassword.StartsWith("EMPW"))
        return;
      this.txtOldPassword.Text = EnConfigurationSettings.GlobalSettings.DatabasePassword;
      this.txtOldPassword.Enabled = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.txtConfirmPassword = new TextBox();
      this.label3 = new Label();
      this.txtOldPassword = new TextBox();
      this.txtNewPassword = new TextBox();
      this.txtUserID = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.SuspendLayout();
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(188, 150);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 4;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.okBtn.Location = new Point(112, 150);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 3;
      this.okBtn.Text = "&OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.txtConfirmPassword.Location = new Point(124, 122);
      this.txtConfirmPassword.MaxLength = (int) byte.MaxValue;
      this.txtConfirmPassword.Name = "txtConfirmPassword";
      this.txtConfirmPassword.PasswordChar = '*';
      this.txtConfirmPassword.Size = new Size(140, 20);
      this.txtConfirmPassword.TabIndex = 2;
      this.label3.Location = new Point(20, 122);
      this.label3.Name = "label3";
      this.label3.Size = new Size(100, 20);
      this.label3.TabIndex = 42;
      this.label3.Text = "Confirm Password:";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.txtOldPassword.Location = new Point(124, 78);
      this.txtOldPassword.MaxLength = (int) byte.MaxValue;
      this.txtOldPassword.Name = "txtOldPassword";
      this.txtOldPassword.PasswordChar = '*';
      this.txtOldPassword.Size = new Size(140, 20);
      this.txtOldPassword.TabIndex = 0;
      this.txtNewPassword.Location = new Point(124, 100);
      this.txtNewPassword.MaxLength = (int) byte.MaxValue;
      this.txtNewPassword.Name = "txtNewPassword";
      this.txtNewPassword.PasswordChar = '*';
      this.txtNewPassword.Size = new Size(140, 20);
      this.txtNewPassword.TabIndex = 1;
      this.txtUserID.Location = new Point(124, 56);
      this.txtUserID.MaxLength = (int) byte.MaxValue;
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.ReadOnly = true;
      this.txtUserID.Size = new Size(140, 20);
      this.txtUserID.TabIndex = 51;
      this.label5.Location = new Point(20, 78);
      this.label5.Name = "label5";
      this.label5.Size = new Size(100, 20);
      this.label5.TabIndex = 38;
      this.label5.Text = "Old Password:";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.Location = new Point(20, 100);
      this.label4.Name = "label4";
      this.label4.Size = new Size(100, 20);
      this.label4.TabIndex = 37;
      this.label4.Text = "New Password:";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(20, 56);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 20);
      this.label2.TabIndex = 36;
      this.label2.Text = "SQL Login ID:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(20, 14);
      this.label1.Name = "label1";
      this.label1.Size = new Size(244, 36);
      this.label1.TabIndex = 50;
      this.label1.Text = "Enter the user's current and new password in the form below:";
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(286, 184);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.txtConfirmPassword);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.txtOldPassword);
      this.Controls.Add((Control) this.txtNewPassword);
      this.Controls.Add((Control) this.txtUserID);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (ChangeDBPassword);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Change Database Password";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public string Password => this.txtNewPassword.Text;

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.txtOldPassword.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter the current password for this user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtNewPassword.Text == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter the new password for this user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtConfirmPassword.Text == "")
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Please confirm the new password for this user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtConfirmPassword.Text != this.txtNewPassword.Text)
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "The new password and the confirmation do not match. Please re-type these values and try again.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        string databaseUserId = EnConfigurationSettings.GlobalSettings.DatabaseUserID;
        try
        {
          this.changePassword(databaseUserId, this.txtOldPassword.Text, this.txtNewPassword.Text);
          try
          {
            this.changePassword("sa", this.txtOldPassword.Text, this.txtNewPassword.Text);
          }
          catch
          {
          }
          this.DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "Error while attemping to change password for user \"" + databaseUserId + "\": " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
      }
    }

    private bool changePassword(string userId, string oldPassword, string newPassword)
    {
      DbAccessManager dbAccessManager = new DbAccessManager(EnConfigurationSettings.GlobalSettings.DatabaseServer, EnConfigurationSettings.GlobalSettings.DatabaseName, userId, oldPassword, false, 0, 5, 15);
      dbAccessManager.Open();
      try
      {
        string sql = "use master\nexec sp_password @OldPassword, @NewPassword, @LoginName";
        dbAccessManager.ExecuteNonQuery(sql, new Hashtable()
        {
          {
            (object) "@OldPassword",
            (object) oldPassword
          },
          {
            (object) "@NewPassword",
            (object) newPassword
          },
          {
            (object) "@LoginName",
            (object) null
          }
        });
        return true;
      }
      finally
      {
        dbAccessManager.Close();
      }
    }
  }
}
