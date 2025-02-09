// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.LoginForm
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.AdminTools.Properties;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.VersionInterface15;
using EllieMae.Encompass.AsmResolver;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  public class LoginForm : Form
  {
    private const string applicationName = "AdminTools";
    private System.ComponentModel.Container components;
    private Panel panel2;
    private Panel pnlServer;
    private Label label3;
    private ComboBox serverBox;
    private Button okBtn;
    private Panel pnlConnection;
    private ComboBox connectionBox;
    private Label label4;
    private Panel pnlUsername;
    private TextBox passwordBox;
    private Label label1;
    private Label label2;
    private TextBox loginNameBox;
    private Button cancelBtn;
    private LoginType allowedLoginTypes = LoginType.Server;
    private bool isLoading = true;
    private bool showConnectionWarning = true;

    public LoginForm(LoginType allowedLoginTypes, string requiredServer, bool donotLockServer)
    {
      this.allowedLoginTypes = allowedLoginTypes;
      this.InitializeComponent();
      this.serverBox.Items.Clear();
      this.serverBox.Items.AddRange((object[]) SystemSettings.Servers);
      if ((requiredServer ?? "") != "")
      {
        this.connectionBox.SelectedIndex = 1;
        this.pnlConnection.Visible = false;
        this.serverBox.Enabled = donotLockServer;
        if (!this.serverBox.Enabled || this.serverBox.Enabled && this.serverBox.Items.Count == 0)
        {
          this.serverBox.SelectedIndex = -1;
          this.serverBox.Text = requiredServer;
        }
      }
      else
      {
        switch (SystemSettings.InstallationMode)
        {
          case InstallationMode.Client:
            this.connectionBox.SelectedIndex = 1;
            this.pnlConnection.Visible = false;
            break;
          case InstallationMode.Local:
            this.connectionBox.SelectedIndex = 0;
            break;
          default:
            this.connectionBox.SelectedIndex = 1;
            break;
        }
        if ((allowedLoginTypes & LoginType.Offline) == LoginType.None)
        {
          this.connectionBox.SelectedIndex = 1;
          this.pnlConnection.Visible = false;
        }
        if ((allowedLoginTypes & LoginType.Server) == LoginType.None)
        {
          this.connectionBox.SelectedIndex = 0;
          this.connectionBox.Enabled = false;
        }
      }
      this.loginNameBox.Text = "admin";
      this.isLoading = false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoginForm));
      this.panel2 = new Panel();
      this.pnlServer = new Panel();
      this.cancelBtn = new Button();
      this.label3 = new Label();
      this.serverBox = new ComboBox();
      this.okBtn = new Button();
      this.pnlConnection = new Panel();
      this.connectionBox = new ComboBox();
      this.label4 = new Label();
      this.pnlUsername = new Panel();
      this.passwordBox = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.loginNameBox = new TextBox();
      this.panel2.SuspendLayout();
      this.pnlServer.SuspendLayout();
      this.pnlConnection.SuspendLayout();
      this.pnlUsername.SuspendLayout();
      this.SuspendLayout();
      this.panel2.BackColor = Color.Transparent;
      this.panel2.Controls.Add((Control) this.pnlServer);
      this.panel2.Controls.Add((Control) this.pnlConnection);
      this.panel2.Controls.Add((Control) this.pnlUsername);
      this.panel2.Location = new Point(54, 146);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(286, 196);
      this.panel2.TabIndex = 16;
      this.pnlServer.Controls.Add((Control) this.cancelBtn);
      this.pnlServer.Controls.Add((Control) this.label3);
      this.pnlServer.Controls.Add((Control) this.serverBox);
      this.pnlServer.Controls.Add((Control) this.okBtn);
      this.pnlServer.Dock = DockStyle.Fill;
      this.pnlServer.Location = new Point(0, 113);
      this.pnlServer.Name = "pnlServer";
      this.pnlServer.Size = new Size(286, 83);
      this.pnlServer.TabIndex = 2;
      this.cancelBtn.BackColor = SystemColors.Control;
      this.cancelBtn.DialogResult = DialogResult.Cancel;
      this.cancelBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.cancelBtn.Location = new Point(172, 42);
      this.cancelBtn.Name = "cancelBtn";
      this.cancelBtn.Size = new Size(71, 22);
      this.cancelBtn.TabIndex = 14;
      this.cancelBtn.Text = "&Cancel";
      this.cancelBtn.UseVisualStyleBackColor = true;
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.Transparent;
      this.label3.Location = new Point(20, 1);
      this.label3.Name = "label3";
      this.label3.Size = new Size(42, 15);
      this.label3.TabIndex = 12;
      this.label3.Text = "Server";
      this.label3.TextAlign = ContentAlignment.MiddleLeft;
      this.label3.DoubleClick += new EventHandler(this.label3_DoubleClick);
      this.serverBox.Location = new Point(94, 0);
      this.serverBox.MaxLength = 100;
      this.serverBox.Name = "serverBox";
      this.serverBox.Size = new Size(169, 23);
      this.serverBox.TabIndex = 4;
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.okBtn.Location = new Point(93, 42);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(71, 22);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&Log In";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.pnlConnection.Controls.Add((Control) this.connectionBox);
      this.pnlConnection.Controls.Add((Control) this.label4);
      this.pnlConnection.Dock = DockStyle.Top;
      this.pnlConnection.Location = new Point(0, 80);
      this.pnlConnection.Name = "pnlConnection";
      this.pnlConnection.Size = new Size(286, 33);
      this.pnlConnection.TabIndex = 1;
      this.connectionBox.DropDownStyle = ComboBoxStyle.DropDownList;
      this.connectionBox.FormattingEnabled = true;
      this.connectionBox.Items.AddRange(new object[2]
      {
        (object) "Local",
        (object) "Networked"
      });
      this.connectionBox.Location = new Point(94, 0);
      this.connectionBox.Name = "connectionBox";
      this.connectionBox.Size = new Size(169, 23);
      this.connectionBox.TabIndex = 14;
      this.connectionBox.SelectedIndexChanged += new EventHandler(this.connectionBox_SelectedIndexChanged);
      this.label4.AutoSize = true;
      this.label4.BackColor = Color.Transparent;
      this.label4.Location = new Point(20, 2);
      this.label4.Name = "label4";
      this.label4.Size = new Size(70, 15);
      this.label4.TabIndex = 13;
      this.label4.Text = "Connection";
      this.label4.TextAlign = ContentAlignment.MiddleLeft;
      this.pnlUsername.Controls.Add((Control) this.passwordBox);
      this.pnlUsername.Controls.Add((Control) this.label1);
      this.pnlUsername.Controls.Add((Control) this.label2);
      this.pnlUsername.Controls.Add((Control) this.loginNameBox);
      this.pnlUsername.Dock = DockStyle.Top;
      this.pnlUsername.Location = new Point(0, 0);
      this.pnlUsername.Name = "pnlUsername";
      this.pnlUsername.Size = new Size(286, 80);
      this.pnlUsername.TabIndex = 0;
      this.passwordBox.Location = new Point(94, 49);
      this.passwordBox.Name = "passwordBox";
      this.passwordBox.PasswordChar = '*';
      this.passwordBox.Size = new Size(169, 21);
      this.passwordBox.TabIndex = 1;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Location = new Point(20, 19);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 15);
      this.label1.TabIndex = 0;
      this.label1.Text = "User ID";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.Transparent;
      this.label2.Location = new Point(20, 50);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.loginNameBox.CharacterCasing = CharacterCasing.Lower;
      this.loginNameBox.Location = new Point(94, 18);
      this.loginNameBox.Name = "loginNameBox";
      this.loginNameBox.Size = new Size(169, 21);
      this.loginNameBox.TabIndex = 0;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(7f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.WhiteSmoke;
      this.BackgroundImage = (Image) Resources.encompass_login_admin_tools;
      this.CancelButton = (IButtonControl) this.cancelBtn;
      this.ClientSize = new Size(394, 396);
      this.Controls.Add((Control) this.panel2);
      this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoginForm);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Encompass Admin Tools Log In";
      this.Shown += new EventHandler(this.LoginForm_Shown);
      this.KeyUp += new KeyEventHandler(this.LoginForm_KeyUp);
      this.panel2.ResumeLayout(false);
      this.pnlServer.ResumeLayout(false);
      this.pnlServer.PerformLayout();
      this.pnlConnection.ResumeLayout(false);
      this.pnlConnection.PerformLayout();
      this.pnlUsername.ResumeLayout(false);
      this.pnlUsername.PerformLayout();
      this.ResumeLayout(false);
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      string text1 = this.serverBox.Text;
      string text2 = this.loginNameBox.Text;
      string text3 = this.passwordBox.Text;
      bool flag = this.connectionBox.SelectedIndex == 0;
      if (text2.Trim() == "")
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "Please enter a User ID.");
      }
      try
      {
        if (!flag && text1 == "")
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, "Please type in the server name or IP address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          Cursor.Current = Cursors.WaitCursor;
          if (flag)
          {
            Session.Start(text2, text3, "AdminTools", false);
            SystemSettings.DefaultApplicationMode = ApplicationMode.Local;
          }
          else
          {
            Session.Start(text1, text2, text3, "AdminTools", false);
            SystemSettings.DefaultApplicationMode = ApplicationMode.Server;
          }
          Session.ExitOnDisconnect = false;
          OrgInfo rootOrganization = Session.OrganizationManager.GetRootOrganization();
          if (!Session.UserInfo.IsAdministrator() || Session.UserInfo.OrgId != rootOrganization.Oid)
          {
            Session.End();
            if (Utils.Dialog((IWin32Window) this, "The specified user does not have root administrative rights on the Encompass Server. Please log in as a different user.", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
              this.DialogResult = DialogResult.No;
          }
          else
          {
            if (!Session.Connection.IsServerInProcess)
              this.updateServerSettings(text1);
            this.DialogResult = DialogResult.OK;
          }
          if (!AssemblyResolver.IsSmartClient || !SmartClientUtils.IsUpdateClientInfo2Required())
            return;
          SmartClientUtils.UpdateClientInfo(LoginUtil.getSmartClientSessionInfo(Session.DefaultInstance, AssemblyResolver.SCClientID, text1), AssemblyResolver.SCClientID, VersionInformation.CurrentVersion.Version.FullVersion, text1, true);
        }
      }
      catch (FormatException ex)
      {
        int num3 = (int) Utils.Dialog((IWin32Window) this, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (LoginException ex)
      {
        if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "The username and password entered do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          this.passwordBox.Text = "";
          this.passwordBox.Focus();
        }
        else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "This user account has been disabled.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
        {
          int num6 = (int) Utils.Dialog((IWin32Window) this, "Login has been disabled. Only the \"admin\" user can login to the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.IP_Blocked)
        {
          int num7 = (int) Utils.Dialog((IWin32Window) this, "Remote access from IP address " + (object) ex.ClientIPAddress + " is not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (ex.LoginReturnCode == LoginReturnCode.SERVER_BUSY)
        {
          int num8 = (int) Utils.Dialog((IWin32Window) this, "Server is currently busy. Please try again later.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          int num9 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error.");
        }
      }
      catch (ServerDataException ex)
      {
        int num10 = (int) Utils.Dialog((IWin32Window) this, "Make sure that the SQL database server (MSDE) is up and running and the data in the database is not corrupted.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (VersionMismatchException ex)
      {
        if (AssemblyResolver.IsSmartClient)
        {
          JedVersion jedVersion = ex.ClientVersion;
          string fullVersion1 = jedVersion.FullVersion;
          jedVersion = ex.ServerVersionControl.Version;
          string fullVersion2 = jedVersion.FullVersion;
          SmartClientUtils.HandleVersionMismatch(fullVersion1, fullVersion2);
        }
        if (VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          Application.Exit();
        this.DialogResult = DialogResult.No;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LicenseException ex)
      {
        int num11 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (ServerConnectionException ex)
      {
        int num12 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        int num13 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      finally
      {
        Cursor.Current = Cursors.Default;
      }
    }

    private void updateServerSettings(string serverName)
    {
      this.serverBox.Items.Remove((object) serverName);
      this.serverBox.Items.Insert(0, (object) serverName);
      this.serverBox.SelectedIndex = 0;
      string[] destination = new string[this.serverBox.Items.Count];
      this.serverBox.Items.CopyTo((object[]) destination, 0);
      SystemSettings.Servers = destination;
    }

    private void connectionBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (this.connectionBox.SelectedIndex == 0)
      {
        this.serverBox.Text = "(local)";
        this.serverBox.Enabled = false;
        if (this.isLoading || !this.showConnectionWarning)
          return;
        this.showConnectionWarning = false;
        if (Utils.Dialog((IWin32Window) this, "Warning: Modifying your Encompass Server's configuration using Local mode can cause unpredictable results. Use Network mode to ensure your changes are applied properly." + Environment.NewLine + Environment.NewLine + "Would you like to switch to Network mode for this login?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
          return;
        this.connectionBox.SelectedIndex = 1;
      }
      else
      {
        if (this.serverBox.Items.Count > 0)
          this.serverBox.Text = this.serverBox.Items[0].ToString();
        else
          this.serverBox.Text = "";
        this.serverBox.Enabled = true;
      }
    }

    private void LoginForm_Shown(object sender, EventArgs e)
    {
      if (!(this.loginNameBox.Text != string.Empty))
        return;
      this.passwordBox.Focus();
    }

    private void LoginForm_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void label3_DoubleClick(object sender, EventArgs e)
    {
      if (this.serverBox.Text == "(local)")
        return;
      this.serverBox.Enabled = !this.serverBox.Enabled;
    }
  }
}
