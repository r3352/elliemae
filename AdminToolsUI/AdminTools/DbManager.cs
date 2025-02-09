// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.DbManager
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class DbManager : Form
  {
    private Button cancelBtn;
    private Label label1;
    private Label label2;
    private Label label4;
    private Label label5;
    private Button btnPassword;
    private Button btnTest;
    private Label label3;
    private Label label6;
    private Button btnNewDB;
    private TextBox lblServer;
    private TextBox lblDatabase;
    private TextBox lblUserID;
    private Button btnDiagnostics;
    private System.ComponentModel.Container components;

    public DbManager()
    {
      this.InitializeComponent();
      this.BackColor = EllieMae.EMLite.AdminTools.AdminTools.FormBackgroundColor;
      this.loadSettings();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DbManager));
      this.cancelBtn = new Button();
      this.label1 = new Label();
      this.label2 = new Label();
      this.label4 = new Label();
      this.label5 = new Label();
      this.btnPassword = new Button();
      this.btnTest = new Button();
      this.label3 = new Label();
      this.label6 = new Label();
      this.btnNewDB = new Button();
      this.lblServer = new TextBox();
      this.lblDatabase = new TextBox();
      this.lblUserID = new TextBox();
      this.btnDiagnostics = new Button();
      this.SuspendLayout();
      this.cancelBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Location = new Point(214, 222);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(75, 22);
      this.cancelBtn.TabIndex = 13;
      this.cancelBtn.Text = "Close";
      this.cancelBtn.Click += new EventHandler(this.cancelBtn_Click);
      this.label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label1.Location = new Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new Size(278, 32);
      this.label1.TabIndex = 14;
      this.label1.Text = "Your current Encompass SQL Server configuration settings:";
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label2.Location = new Point(12, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(44, 13);
      this.label2.TabIndex = 15;
      this.label2.Text = "Server";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label4.Location = new Point(12, 99);
      this.label4.Name = "label4";
      this.label4.Size = new Size(50, 13);
      this.label4.TabIndex = 17;
      this.label4.Text = "User ID";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.label5.Location = new Point(12, 77);
      this.label5.Name = "label5";
      this.label5.Size = new Size(61, 13);
      this.label5.TabIndex = 18;
      this.label5.Text = "Database";
      this.label5.TextAlign = ContentAlignment.MiddleLeft;
      this.btnPassword.Location = new Point(181, (int) sbyte.MaxValue);
      this.btnPassword.Name = "btnPassword";
      this.btnPassword.Size = new Size(107, 22);
      this.btnPassword.TabIndex = 22;
      this.btnPassword.Text = "Change Password";
      this.btnPassword.Click += new EventHandler(this.btnPassword_Click);
      this.btnTest.Location = new Point(96, (int) sbyte.MaxValue);
      this.btnTest.Name = "btnTest";
      this.btnTest.Size = new Size(81, 22);
      this.btnTest.TabIndex = 23;
      this.btnTest.Text = "Test";
      this.btnTest.Click += new EventHandler(this.btnTest_Click);
      this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label3.BackColor = Color.Black;
      this.label3.Location = new Point(6, 156);
      this.label3.Name = "label3";
      this.label3.Size = new Size(287, 2);
      this.label3.TabIndex = 24;
      this.label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.label6.Location = new Point(12, 164);
      this.label6.Name = "label6";
      this.label6.Size = new Size(276, 48);
      this.label6.TabIndex = 25;
      this.label6.Text = "To perform a basic diagnostic check on your SQL database, click the Run Diagnostics button. You may then elect to take corrective action if required.";
      this.btnNewDB.Location = new Point(12, (int) sbyte.MaxValue);
      this.btnNewDB.Name = "btnNewDB";
      this.btnNewDB.Size = new Size(81, 22);
      this.btnNewDB.TabIndex = 26;
      this.btnNewDB.Text = "Edit";
      this.btnNewDB.Click += new EventHandler(this.btnNewDB_Click);
      this.lblServer.Location = new Point(82, 52);
      this.lblServer.Name = "lblServer";
      this.lblServer.ReadOnly = true;
      this.lblServer.Size = new Size(206, 20);
      this.lblServer.TabIndex = 27;
      this.lblServer.TabStop = false;
      this.lblDatabase.Location = new Point(82, 74);
      this.lblDatabase.Name = "lblDatabase";
      this.lblDatabase.ReadOnly = true;
      this.lblDatabase.Size = new Size(206, 20);
      this.lblDatabase.TabIndex = 28;
      this.lblDatabase.TabStop = false;
      this.lblUserID.Location = new Point(82, 96);
      this.lblUserID.Name = "lblUserID";
      this.lblUserID.ReadOnly = true;
      this.lblUserID.Size = new Size(206, 20);
      this.lblUserID.TabIndex = 29;
      this.lblUserID.TabStop = false;
      this.btnDiagnostics.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnDiagnostics.Location = new Point(12, 222);
      this.btnDiagnostics.Name = "btnDiagnostics";
      this.btnDiagnostics.Size = new Size(128, 22);
      this.btnDiagnostics.TabIndex = 30;
      this.btnDiagnostics.Text = "Run Diagnostics";
      this.btnDiagnostics.Click += new EventHandler(this.btnDiagnostics_Click);
      this.AutoScaleBaseSize = new Size(5, 13);
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(298, 252);
      this.Controls.Add((Control) this.btnDiagnostics);
      this.Controls.Add((Control) this.lblUserID);
      this.Controls.Add((Control) this.lblDatabase);
      this.Controls.Add((Control) this.lblServer);
      this.Controls.Add((Control) this.btnNewDB);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.btnTest);
      this.Controls.Add((Control) this.btnPassword);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cancelBtn);
      this.Controls.Add((Control) this.label6);
      this.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = nameof (DbManager);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SQL Server Setup";
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void loadSettings()
    {
      this.lblServer.Text = EnConfigurationSettings.GlobalSettings.DatabaseServer;
      this.lblDatabase.Text = EnConfigurationSettings.GlobalSettings.DatabaseName;
      this.lblUserID.Text = EnConfigurationSettings.GlobalSettings.DatabaseUserID;
      this.btnPassword.Enabled = this.lblUserID.Text != "";
    }

    private void cancelBtn_Click(object sender, EventArgs e) => this.Close();

    private void btnTest_Click(object sender, EventArgs e)
    {
      try
      {
        DbManager.TestConnection(new DbAccessManager());
        int num = (int) Utils.Dialog((IWin32Window) this, "Test completed successfully.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Test failed! The reason for failure was reported as: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
    }

    public static void TestConnection(DbAccessManager dbAccess)
    {
      dbAccess.Open();
      try
      {
        dbAccess.ExecuteNonQuery("select * from users");
      }
      finally
      {
        dbAccess.Close();
      }
    }

    private void btnNewDB_Click(object sender, EventArgs e)
    {
      NewDBSettings newDbSettings = new NewDBSettings();
      if (newDbSettings.ShowDialog((IWin32Window) this) != DialogResult.OK)
        return;
      bool flag = false;
      if (newDbSettings.DatabaseServer != EnConfigurationSettings.GlobalSettings.DatabaseServer)
      {
        EnConfigurationSettings.GlobalSettings.DatabaseServer = newDbSettings.DatabaseServer;
        flag = true;
      }
      if (newDbSettings.DatabaseName != EnConfigurationSettings.GlobalSettings.DatabaseName)
      {
        EnConfigurationSettings.GlobalSettings.DatabaseName = newDbSettings.DatabaseName;
        flag = true;
      }
      if (newDbSettings.DatabaseUserID != EnConfigurationSettings.GlobalSettings.DatabaseUserID)
      {
        EnConfigurationSettings.GlobalSettings.DatabaseUserID = newDbSettings.DatabaseUserID;
        flag = true;
      }
      if (newDbSettings.DatabasePassword != EnConfigurationSettings.GlobalSettings.DatabasePassword)
      {
        EnConfigurationSettings.GlobalSettings.DatabasePassword = newDbSettings.DatabasePassword;
        flag = true;
      }
      if (!flag)
        return;
      Win64.SyncEncompass64To32();
      this.loadSettings();
      int num = (int) Utils.Dialog((IWin32Window) this, "You must stop and restart the Encompass Server for these settings to take effect.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void btnPassword_Click(object sender, EventArgs e)
    {
      if (EnConfigurationSettings.GlobalSettings.InstallationMode == InstallationMode.Server && Utils.Dialog((IWin32Window) this, "Warning: If you modify the password while your server is running, your server will become unable to access the database until restarted.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.Cancel)
        return;
      try
      {
        ChangeDBPassword changeDbPassword = new ChangeDBPassword();
        if (changeDbPassword.ShowDialog((IWin32Window) this) != DialogResult.OK)
          return;
        EnConfigurationSettings.GlobalSettings.DatabasePassword = changeDbPassword.Password;
        Win64.SyncEncompass64To32();
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Unable to modify database password: " + ex.Message);
      }
    }

    private void btnDiagnostics_Click(object sender, EventArgs e)
    {
      if (Utils.Dialog((IWin32Window) this, "The database diagnostics can take several minutes to run. During that time, your SQL Server's performance will be negatively impacted and Encompass users may notice decreased responsiveness." + Environment.NewLine + Environment.NewLine + "Are you sure you want to proceed with the diagnostics at this time?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
        return;
      new DbDiagnosticsDialog().Show();
    }
  }
}
