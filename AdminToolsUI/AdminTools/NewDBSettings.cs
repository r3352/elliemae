// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.NewDBSettings
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class NewDBSettings : Form
  {
    private TextBox txtDatabase;
    private TextBox txtUserID;
    private TextBox txtServer;
    private Label label5;
    private Label label4;
    private Label label2;
    private Label label1;
    private Label label3;
    private Button cancelBtn;
    private Button okBtn;
    private TextBox txtPassword;
    private System.ComponentModel.Container components;

    public NewDBSettings()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.txtServer.Text = EnConfigurationSettings.GlobalSettings.DatabaseServer;
      this.txtDatabase.Text = EnConfigurationSettings.GlobalSettings.DatabaseName;
      this.txtUserID.Text = EnConfigurationSettings.GlobalSettings.DatabaseUserID;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.txtDatabase = new TextBox();
      this.txtUserID = new TextBox();
      this.txtServer = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.txtPassword = new TextBox();
      this.label3 = new Label();
      this.cancelBtn = new Button();
      this.okBtn = new Button();
      this.SuspendLayout();
      this.txtDatabase.Location = new Point(124, 72);
      this.txtDatabase.MaxLength = (int) byte.MaxValue;
      this.txtDatabase.Name = "txtDatabase";
      this.txtDatabase.Size = new Size(140, 20);
      this.txtDatabase.TabIndex = 1;
      this.txtUserID.Location = new Point(124, 94);
      this.txtUserID.MaxLength = (int) byte.MaxValue;
      this.txtUserID.Name = "txtUserID";
      this.txtUserID.Size = new Size(140, 20);
      this.txtUserID.TabIndex = 2;
      this.txtServer.Location = new Point(124, 50);
      this.txtServer.MaxLength = (int) byte.MaxValue;
      this.txtServer.Name = "txtServer";
      this.txtServer.Size = new Size(140, 20);
      this.txtServer.TabIndex = 0;
      this.label5.Location = new Point(20, 71);
      this.label5.Name = "label5";
      this.label5.Size = new Size(100, 20);
      this.label5.TabIndex = 27;
      this.label5.Text = "Database:";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.Location = new Point(20, 94);
      this.label4.Name = "label4";
      this.label4.Size = new Size(100, 20);
      this.label4.TabIndex = 26;
      this.label4.Text = "SQL Login ID:";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.Location = new Point(20, 50);
      this.label2.Name = "label2";
      this.label2.Size = new Size(100, 20);
      this.label2.TabIndex = 25;
      this.label2.Text = "Server:";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label1.Location = new Point(20, 8);
      this.label1.Name = "label1";
      this.label1.Size = new Size(244, 36);
      this.label1.TabIndex = 24;
      this.label1.Text = "Enter the new connection information for the SQL Server:";
      this.txtPassword.Location = new Point(124, 116);
      this.txtPassword.MaxLength = (int) byte.MaxValue;
      this.txtPassword.Name = "txtPassword";
      this.txtPassword.PasswordChar = '*';
      this.txtPassword.Size = new Size(140, 20);
      this.txtPassword.TabIndex = 3;
      this.label3.Location = new Point(20, 116);
      this.label3.Name = "label3";
      this.label3.Size = new Size(100, 20);
      this.label3.TabIndex = 31;
      this.label3.Text = "Password:";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(189, 144);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 23);
      this.cancelBtn.TabIndex = 5;
      this.cancelBtn.Text = "Cancel";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.okBtn.Location = new Point(112, 144);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(75, 23);
      this.okBtn.TabIndex = 4;
      this.okBtn.Text = "OK";
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(284, 176);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.okBtn);
      this.Controls.Add((Control) this.txtPassword);
      this.Controls.Add((Control) this.txtDatabase);
      this.Controls.Add((Control) this.txtUserID);
      this.Controls.Add((Control) this.txtServer);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewDBSettings);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "SQL Server Settings";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public string DatabaseServer => this.txtServer.Text;

    public string DatabaseName => this.txtDatabase.Text;

    public string DatabaseUserID => this.txtUserID.Text;

    public string DatabasePassword => this.txtPassword.Text;

    private void okBtn_Click(object sender, EventArgs e)
    {
      if (this.txtServer.Text == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter the name of the SQL Server.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtDatabase.Text == "")
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "Please enter the name of the Encompass database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtUserID.Text == "")
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "Please enter the login ID of the SQL user to use when connecting to the database.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else if (this.txtPassword.Text == "")
      {
        int num4 = (int) Utils.Dialog((IWin32Window) this, "Please enter the password for the specified user.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (!this.testConnection())
          return;
        this.DialogResult = DialogResult.OK;
      }
    }

    private bool testConnection()
    {
      try
      {
        DbManager.TestConnection(new DbAccessManager(this.txtServer.Text, this.txtDatabase.Text, this.txtUserID.Text, this.txtPassword.Text, false, 0, 5, 15));
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "A test connection using the specified parameters has failed. The reason reported was: " + ex.Message + ". Please fix this problem and try again.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
    }

    private void cancelBtn_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
    }
  }
}
