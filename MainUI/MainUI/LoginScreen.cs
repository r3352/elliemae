// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.MainUI.LoginScreen
// Assembly: MainUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 08E50E3F-BC8C-4CB4-A2C3-E44E8DFB9C64
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\MainUI.dll

using Elli.Server.Remoting;
using EllieMae.EMLite.Client;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.Diagnostics;
using EllieMae.EMLite.ePass;
using EllieMae.EMLite.ePass.PartnerAPI;
using EllieMae.EMLite.Properties;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.MainUI
{
  public class LoginScreen : Form
  {
    private const string appName = "Encompass";
    private const string className = "LoginScreen";
    private static readonly string sw = Tracing.SwOutsideLoan;
    private static DateTime loginStartTime;
    private string serverForCRMTool;
    private string pwdForCRMTool;
    private Panel panel1;
    private Label label1;
    private Label label2;
    private TextBox loginNameBox;
    private Button okBtn;
    private Label label3;
    private ComboBox serverBox;
    private TextBox passwordBox;
    private string requiredServerName;
    private bool donotLockServer;
    private Label label4;
    private Panel panel2;
    private Panel pnlServer;
    private Panel pnlConnection;
    private ComboBox connectionBox;
    private ContextMenuStrip contextMenuStrip1;
    private IContainer components;
    private ToolStripMenuItem tsMenuItemServer;
    private Button cancelBtn;
    private Label lblPartnerAPIDebug;
    private ContextMenuStrip contextMenuStrip2;
    private ToolStripMenuItem tsMenuItemServerTime;
    private Panel pnlUsername;
    private string loginName;
    private string password;
    private string server;
    private int _connectionMode = -1;
    private static bool channelInitialized = false;

    public LoginScreen()
    {
      this.InitializeComponent();
      this.serverBox.Items.Clear();
      this.serverBox.Items.AddRange((object[]) SystemSettings.Servers);
      if (SystemSettings.InstallationMode == InstallationMode.Client)
      {
        this.connectionBox.SelectedIndex = 1;
        this.pnlConnection.Visible = false;
      }
      else if (SystemSettings.DefaultApplicationMode == ApplicationMode.Local)
        this.connectionBox.SelectedIndex = 0;
      else
        this.connectionBox.SelectedIndex = 1;
      if (PapiSettings.Debug)
        this.lblPartnerAPIDebug.Visible = true;
      this.loginNameBox.Text = SystemSettings.LastLoginName;
      if (EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders.ContainsKey("X-ClientType"))
        return;
      EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders["X-ClientType"] = "Encompass".Replace(":", "").Replace(";", "");
    }

    public LoginScreen(string loginName, string password, string server, bool donotLockServer)
      : this()
    {
      this.loginName = loginName;
      this.password = password;
      this.server = server;
      this.donotLockServer = donotLockServer;
    }

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (LoginScreen));
      this.contextMenuStrip1 = new ContextMenuStrip(this.components);
      this.tsMenuItemServer = new ToolStripMenuItem();
      this.okBtn = new Button();
      this.panel1 = new Panel();
      this.lblPartnerAPIDebug = new Label();
      this.panel2 = new Panel();
      this.pnlServer = new Panel();
      this.cancelBtn = new Button();
      this.label3 = new Label();
      this.serverBox = new ComboBox();
      this.pnlConnection = new Panel();
      this.connectionBox = new ComboBox();
      this.label4 = new Label();
      this.pnlUsername = new Panel();
      this.passwordBox = new TextBox();
      this.label1 = new Label();
      this.label2 = new Label();
      this.contextMenuStrip2 = new ContextMenuStrip(this.components);
      this.tsMenuItemServerTime = new ToolStripMenuItem();
      this.loginNameBox = new TextBox();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel2.SuspendLayout();
      this.pnlServer.SuspendLayout();
      this.pnlConnection.SuspendLayout();
      this.pnlUsername.SuspendLayout();
      this.contextMenuStrip2.SuspendLayout();
      this.SuspendLayout();
      this.contextMenuStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemServer
      });
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new Size(123, 26);
      this.contextMenuStrip1.Opening += new CancelEventHandler(this.contextMenuStrip1_Opening);
      this.tsMenuItemServer.Name = "tsMenuItemServer";
      this.tsMenuItemServer.Size = new Size(122, 22);
      this.tsMenuItemServer.Text = "<Server>";
      this.tsMenuItemServer.Click += new EventHandler(this.okBtn_Click);
      this.okBtn.BackColor = SystemColors.Control;
      this.okBtn.Font = new Font("Arial", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.okBtn.Location = new Point(93, 42);
      this.okBtn.Name = "okBtn";
      this.okBtn.Size = new Size(71, 22);
      this.okBtn.TabIndex = 5;
      this.okBtn.Text = "&Log In";
      this.okBtn.UseVisualStyleBackColor = true;
      this.okBtn.Click += new EventHandler(this.okBtn_Click);
      this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
      this.panel1.BackgroundImage = (Image) Resources.enc_16_1_login_bg_image;
      this.panel1.Controls.Add((Control) this.lblPartnerAPIDebug);
      this.panel1.Controls.Add((Control) this.panel2);
      this.panel1.Dock = DockStyle.Fill;
      this.panel1.Location = new Point(0, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(394, 396);
      this.panel1.TabIndex = 0;
      this.panel1.DoubleClick += new EventHandler(this.panel1_DoubleClick);
      this.lblPartnerAPIDebug.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.lblPartnerAPIDebug.AutoSize = true;
      this.lblPartnerAPIDebug.BackColor = System.Drawing.Color.Transparent;
      this.lblPartnerAPIDebug.Font = new Font("Arial", 9f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, (byte) 0);
      this.lblPartnerAPIDebug.Location = new Point(4, 376);
      this.lblPartnerAPIDebug.Name = "lblPartnerAPIDebug";
      this.lblPartnerAPIDebug.Size = new Size(197, 15);
      this.lblPartnerAPIDebug.TabIndex = 15;
      this.lblPartnerAPIDebug.Text = "Partner API Debug Mode Enabled";
      this.lblPartnerAPIDebug.Visible = false;
      this.panel2.BackColor = System.Drawing.Color.Transparent;
      this.panel2.Controls.Add((Control) this.pnlServer);
      this.panel2.Controls.Add((Control) this.pnlConnection);
      this.panel2.Controls.Add((Control) this.pnlUsername);
      this.panel2.Location = new Point(54, 146);
      this.panel2.Name = "panel2";
      this.panel2.Size = new Size(286, 196);
      this.panel2.TabIndex = 14;
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
      this.label3.BackColor = System.Drawing.Color.Transparent;
      this.label3.ContextMenuStrip = this.contextMenuStrip1;
      this.label3.Location = new Point(19, 3);
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
      this.label4.BackColor = System.Drawing.Color.Transparent;
      this.label4.Location = new Point(19, 4);
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
      this.label1.BackColor = System.Drawing.Color.Transparent;
      this.label1.Location = new Point(19, 21);
      this.label1.Name = "label1";
      this.label1.Size = new Size(49, 15);
      this.label1.TabIndex = 0;
      this.label1.Text = "User ID";
      this.label1.TextAlign = ContentAlignment.MiddleLeft;
      this.label2.AutoSize = true;
      this.label2.BackColor = System.Drawing.Color.Transparent;
      this.label2.ContextMenuStrip = this.contextMenuStrip2;
      this.label2.Location = new Point(19, 52);
      this.label2.Name = "label2";
      this.label2.Size = new Size(63, 15);
      this.label2.TabIndex = 1;
      this.label2.Text = "Password";
      this.label2.TextAlign = ContentAlignment.MiddleLeft;
      this.contextMenuStrip2.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.tsMenuItemServerTime
      });
      this.contextMenuStrip2.Name = "contextMenuStrip1";
      this.contextMenuStrip2.Size = new Size(153, 26);
      this.contextMenuStrip2.Opening += new CancelEventHandler(this.contextMenuStrip2_Opening);
      this.tsMenuItemServerTime.Name = "tsMenuItemServerTime";
      this.tsMenuItemServerTime.Size = new Size(152, 22);
      this.tsMenuItemServerTime.Text = "<Server Time>";
      this.loginNameBox.CharacterCasing = CharacterCasing.Lower;
      this.loginNameBox.Location = new Point(94, 18);
      this.loginNameBox.Name = "loginNameBox";
      this.loginNameBox.Size = new Size(169, 21);
      this.loginNameBox.TabIndex = 0;
      this.AcceptButton = (IButtonControl) this.okBtn;
      this.AutoScaleDimensions = new SizeF(7f, 15f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(394, 396);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (LoginScreen);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Encompass Log In";
      this.Load += new EventHandler(this.LoginScreen_Load);
      this.Shown += new EventHandler(this.LoginScreen_Shown);
      this.KeyUp += new KeyEventHandler(this.LoginScreen_KeyUp);
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel2.ResumeLayout(false);
      this.pnlServer.ResumeLayout(false);
      this.pnlServer.PerformLayout();
      this.pnlConnection.ResumeLayout(false);
      this.pnlConnection.PerformLayout();
      this.pnlUsername.ResumeLayout(false);
      this.pnlUsername.PerformLayout();
      this.contextMenuStrip2.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    public DialogResult Login()
    {
      return this.Login(this.loginName, this.password, this.server, this.donotLockServer);
    }

    public DialogResult Login(
      string loginName,
      string password,
      string server,
      bool donotLockServer)
    {
      this.donotLockServer = donotLockServer;
      string str = "(Local)";
      if ((server ?? "") != "")
        str = server;
      if ((loginName ?? "") != "")
      {
        try
        {
          return this.performLogin(loginName, password, server) ? DialogResult.OK : DialogResult.Cancel;
        }
        catch
        {
          this.loginNameBox.Text = (loginName ?? "").Trim();
        }
      }
      if ((server ?? "") != "")
        this.requiredServerName = server;
      if (!AssemblyResolver.IsSmartClient)
      {
        if (this.connectionMode == 1)
        {
          this.connectionBox.SelectedIndex = 0;
          this.connectionBox.Enabled = false;
          this.serverBox.SelectedIndex = -1;
          this.serverBox.Text = "(Local)";
          this.requiredServerName = (string) null;
          this.serverBox.Enabled = false;
        }
        else if (this.connectionMode == 2)
        {
          this.connectionBox.SelectedIndex = 1;
          this.pnlConnection.Visible = false;
        }
        else if (this.connectionMode == 0 && Utils.Dialog((IWin32Window) this, "This program is no longer supported. Please use the SmartClient version of Encompass. If you continue with this program, you may not get future hot updates.  Do you still want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
          Process.GetCurrentProcess().Kill();
      }
      return this.ShowDialog();
    }

    private string registryClientID
    {
      get
      {
        using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Ellie Mae\\Encompass"))
          return registryKey == null ? (string) null : (string) registryKey.GetValue("ClientID");
      }
    }

    private int connectionMode
    {
      get
      {
        if (this._connectionMode < 0)
        {
          this._connectionMode = 3;
          string attribute = SmartClientUtils.GetAttribute(this.registryClientID ?? "*", "Encompass.exe", "ConnectionMode");
          if (attribute != null)
          {
            try
            {
              this._connectionMode = int.Parse(attribute);
            }
            catch (Exception ex)
            {
              string str = "Error parsing attribute 'ConnectionMode': " + ex.Message;
              Tracing.Log(LoginScreen.sw, nameof (LoginScreen), TraceLevel.Warning, str);
              if (EnConfigurationSettings.GlobalSettings.Debug)
              {
                int num = (int) Utils.Dialog((IWin32Window) this, str, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
            }
          }
        }
        return this._connectionMode;
      }
    }

    private void okBtn_Click(object sender, EventArgs e)
    {
      LoginScreen.loginStartTime = DateTime.Now;
      string loginName = this.loginNameBox.Text.Trim();
      string text = this.passwordBox.Text;
      bool flag1 = this.connectionBox.SelectedIndex == 0;
      string server = "";
      if (!flag1)
      {
        string str = this.serverBox.Text.Trim();
        server = str;
        if (str == null || str == string.Empty)
        {
          int num = (int) Utils.Dialog((IWin32Window) this, "Please type in the server name or IP address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return;
        }
      }
      PerformanceMeter performanceMeter = PerformanceMeter.StartNew("Encompass.Login", "Login process from login screen to UI display", true, 561, nameof (okBtn_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
      try
      {
        Cursor.Current = Cursors.WaitCursor;
        if (!this.performLogin(loginName, text, server))
          return;
        PerformanceMeter.Current.AddCheckpoint("Perform Login Logistic", 572, nameof (okBtn_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
        if (AssemblyResolver.IsSmartClient)
        {
          Tracing.Log(true, "Info", nameof (LoginScreen), "SCID = " + AssemblyResolver.SCClientID);
          if (SmartClientUtils.IsUpdateClientInfo2Required())
            SmartClientUtils.UpdateClientInfo(LoginUtil.getSmartClientSessionInfo(Session.DefaultInstance, AssemblyResolver.SCClientID, server), AssemblyResolver.SCClientID, VersionInformation.CurrentVersion.Version.FullVersion, server, Session.UserInfo.IsSuperAdministrator());
          if (AssemblyResolver.SCClientID.EndsWith(SmartClientUtils.SCTestCIDSuffix, StringComparison.CurrentCultureIgnoreCase))
          {
            List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
            VersionManagementGroup[] managementGroups = Session.ServerManager.GetVersionManagementGroups();
            if (managementGroups != null)
            {
              foreach (VersionManagementGroup versionManagementGroup in managementGroups)
              {
                if (!versionManagementGroup.IsDefaultGroup)
                  userInfoSummaryList.AddRange((IEnumerable<UserInfoSummary>) Session.ServerManager.GetVersionManagementGroupUsers(versionManagementGroup.GroupID));
              }
            }
            bool flag2 = true;
            if (userInfoSummaryList.Count > 0)
            {
              flag2 = false;
              foreach (UserInfoSummary userInfoSummary in userInfoSummaryList)
              {
                if (string.Compare(Session.UserID, userInfoSummary.UserID, true) == 0)
                {
                  flag2 = true;
                  break;
                }
              }
            }
            if (!flag2)
            {
              int num = (int) Utils.Dialog((IWin32Window) this, "You are not allowed to log in using Encompass SmartClient test CID.  Please use your production SmartClient CID.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
              Application.Exit();
            }
          }
        }
        PerformanceMeter.Current.AddCheckpoint("Perform Smart Client Registration", 620, nameof (okBtn_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
        if (!this.validatePassword())
        {
          this.DialogResult = DialogResult.Cancel;
        }
        else
        {
          this.checkCRMStatus(flag1 ? "(Local)" : server, text);
          MainForm instance = MainForm.Instance;
          instance.Text = instance.Text + " - " + Session.StartupInfo.UserInfo.Userid + " - " + Session.CompanyInfo.ClientID;
          try
          {
            new Thread(new ThreadStart(this.WriteDTToServer)).Start();
          }
          catch
          {
          }
          this.DialogResult = DialogResult.OK;
        }
      }
      catch (LoginException ex)
      {
        this.handleLoginException(ex);
      }
      catch (ServerDataException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "Make sure that the database server is up and running and the data in the database is not corrupt.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      catch (VersionMismatchException ex)
      {
        if (AssemblyResolver.IsSmartClient)
          SmartClientUtils.HandleVersionMismatch(ex.ClientVersion.FullVersion, ex.ServerVersionControl.Version.FullVersion);
        if (!EllieMae.EMLite.Common.VersionControl.QueryInstallVersionUpdate(ex.ServerVersionControl))
          return;
        Application.Exit();
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.LicenseException ex)
      {
        if (ex.Cause == LicenseExceptionType.TrialExpired & flag1)
        {
          int num1 = (int) new TrialExpiredForm(ex.License).ShowDialog((IWin32Window) this);
        }
        else
        {
          int num2 = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        Application.Exit();
      }
      catch (ServerConnectionException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (FormatException ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The Server name format you have entered is invalid.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      catch (Exception ex)
      {
        if (ex.InnerException != null && ex.InnerException is LoginException)
          this.handleLoginException((LoginException) ex.InnerException);
        else if (EnConfigurationSettings.GlobalSettings.Debug)
        {
          int num3 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error: " + (object) ex, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      finally
      {
        if (!Session.IsConnected)
          performanceMeter.Abort();
        else
          PerformanceMeter.Current.AddCheckpoint("Complete Login", 697, nameof (okBtn_Click), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
        Cursor.Current = Cursors.Default;
      }
    }

    private void WriteDTToServer()
    {
      try
      {
        string message = DTErrorLogger.ReadLog();
        if (!(message != ""))
          return;
        RemoteLogger.Write(TraceLevel.Warning, message);
        DTErrorLogger.Rename();
      }
      catch
      {
      }
    }

    private void handleLoginException(LoginException ex)
    {
      if (ex.LoginReturnCode == LoginReturnCode.USERID_NOT_FOUND || ex.LoginReturnCode == LoginReturnCode.PASSWORD_MISMATCH)
      {
        int num = (int) Utils.Dialog((IWin32Window) this, "The username and password entered do not match. The password is case-sensitive. Please try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.passwordBox.Text = "";
        this.passwordBox.Focus();
      }
      else if (ex.LoginReturnCode == LoginReturnCode.USER_DISABLED)
      {
        int num1 = (int) Utils.Dialog((IWin32Window) this, "This user account has been disabled. Your System Administrator must enable your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.LOGIN_DISABLED)
      {
        int num2 = (int) Utils.Dialog((IWin32Window) this, "All Encompass logins have been disabled by your System Administrator. Only the \"admin\" user can currently log into the system.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.USER_LOCKED)
      {
        string str = string.Empty;
        if (!string.IsNullOrWhiteSpace(ex.Message))
        {
          int num3 = ex.Message.IndexOf("USER_LOCKED");
          if (num3 > 0 && ex.Message.Length >= num3 + 12)
            str = ex.Message.Substring(ex.Message.IndexOf("USER_LOCKED") + 12);
        }
        if (string.IsNullOrEmpty(str))
        {
          int num4 = (int) Utils.Dialog((IWin32Window) this, "Your user account has been locked. Your System Administrator must unlock your account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
        {
          int num5 = (int) Utils.Dialog((IWin32Window) this, "Your account has been locked due to multiple failed login attempts. Please wait " + str + " to try again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
      }
      else if (ex.LoginReturnCode == LoginReturnCode.PERSONA_NOT_FOUND)
      {
        int num6 = (int) Utils.Dialog((IWin32Window) this, "This user account does not have a persona assigned to it. Contact your System Administrator to have this corrected.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.Concurrent_Editing_Offline_Not_Allowed)
      {
        int num7 = (int) Utils.Dialog((IWin32Window) this, "Multi-User Editing is enabled on this system. Please login to Encompass server using \"Networked\" connection mode.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.IP_Blocked)
      {
        int num8 = (int) Utils.Dialog((IWin32Window) this, "Remote access from IP address " + (object) ex.ClientIPAddress + " is not allowed.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.TPO_LOGIN_RESTRICTED)
      {
        int num9 = (int) Utils.Dialog((IWin32Window) this, "You are not authorized to log into Encompass.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.SERVER_BUSY)
      {
        int num10 = (int) Utils.Dialog((IWin32Window) this, "Server is currently busy. Please try again later.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.API_USER_RESTRICTED)
      {
        int num11 = (int) Utils.Dialog((IWin32Window) this, "You do not have sufficient privileges to log into Encompass, please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.SSO_USER_PASSWORD_NOT_ALLOWED)
      {
        int num12 = (int) Utils.Dialog((IWin32Window) MainForm.Instance, "Your login method has been changed to use your company credentials, please contact your administrator.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else if (ex.LoginReturnCode == LoginReturnCode.USER_LOGIN_DISABLED)
      {
        int num13 = (int) Utils.Dialog((IWin32Window) this, "This user login has been disabled. Your System Administrator must enable user account in order for you to log in.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      else
      {
        int num14 = (int) Utils.Dialog((IWin32Window) this, "Unknown login error.");
      }
    }

    private void checkCRMStatus(string server, string password)
    {
      if (Session.UserID != "admin")
        return;
      string companySetting = Session.ConfigurationManager.GetCompanySetting("Migration", "CRMTool");
      if (!(companySetting == "0") && !(companySetting == ""))
        return;
      CRMToolPrompt crmToolPrompt = new CRMToolPrompt();
      int num = (int) crmToolPrompt.ShowDialog((IWin32Window) this);
      if (crmToolPrompt.SelectedOption == CRMToolPrompt.CRMOption.DonotShow)
        Session.ConfigurationManager.SetCompanySetting("Migration", "CRMTool", "2");
      else if (crmToolPrompt.SelectedOption == CRMToolPrompt.CRMOption.Later)
      {
        Session.ConfigurationManager.SetCompanySetting("Migration", "CRMTool", "0");
      }
      else
      {
        this.serverForCRMTool = server;
        this.pwdForCRMTool = password;
        new Thread(new ThreadStart(this.LaunchCRMTool))
        {
          Priority = ThreadPriority.Normal,
          IsBackground = false
        }.Start();
      }
    }

    private void LaunchCRMTool()
    {
      Process process = new Process();
      process.StartInfo.CreateNoWindow = true;
      process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
      string str = (this.serverForCRMTool == null ? "" : " -s " + this.serverForCRMTool) + (this.pwdForCRMTool == null ? "" : " -p " + this.pwdForCRMTool);
      if (AssemblyResolver.IsSmartClient)
      {
        process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppLauncher.exe");
        process.StartInfo.Arguments = "CRMTool.exe" + str;
      }
      else
      {
        process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CRMTool.exe");
        process.StartInfo.Arguments = str.TrimStart();
      }
      process.StartInfo.UseShellExecute = false;
      process.Start();
    }

    private bool validatePassword()
    {
      if (!Session.StartupInfo.IsPasswordChangeRequired)
        return true;
      using (ChangePasswordDialog changePasswordDialog = new ChangePasswordDialog())
        return changePasswordDialog.ShowDialog((IWin32Window) this) == DialogResult.OK;
    }

    private void startAsyncEpassLoginTask()
    {
      EpassLogin.silentEpassLoginTask = Task.Run<bool>((Func<bool>) (() => EpassLogin.LoginUser(false)));
    }

    private bool performLogin(string loginName, string password, string server)
    {
      PerformanceMeter performanceMeter = PerformanceMeter.Get("Encompass.Login");
      bool flag = (server ?? "") == "";
      string displayVersionString = VersionInformation.CurrentVersion.DisplayVersionString;
      ThreadPool.SetMinThreads(15, 10);
      SystemSettings.LastLoginName = loginName;
      if ((server ?? "") == "")
      {
        SystemSettings.DefaultApplicationMode = ApplicationMode.Local;
        SystemSettings.ApplicationMode = ApplicationMode.Local;
        Session.Start(loginName, password, "Encompass");
        this.startAsyncEpassLoginTask();
      }
      else
      {
        SystemSettings.DefaultApplicationMode = ApplicationMode.Server;
        SystemSettings.ApplicationMode = ApplicationMode.Server;
        Session.Start(server, loginName, password, "Encompass", true, (string) null, (string) null);
        this.startAsyncEpassLoginTask();
        performanceMeter.AddCheckpoint("Perform Login - Start Session", 880, nameof (performLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
        this.updateServerSettings(server);
        performanceMeter.AddCheckpoint("Perform Login - Update Server Settings", 885, nameof (performLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
        if (EnConfigurationSettings.GlobalSettings.InstallationMode == InstallationMode.Local)
          this.syncCompanySettings();
        performanceMeter.AddCheckpoint("Perform Login - Sync Company Settings", 891, nameof (performLogin), "D:\\ws\\24.3.0.0\\EmLite\\EMLite\\MainUI\\LoginScreen.cs");
      }
      DiagnosticSession.InitializeSessionVariables();
      if (!this.performLicenseChecks())
        return false;
      if (EnConfigurationSettings.GlobalSettings.Debug)
        Tracing.Log(true, "Init", nameof (LoginScreen), "Server time is " + (object) Session.ServerTime);
      MainForm instance1 = MainForm.Instance;
      instance1.Text = instance1.Text + " - Build " + displayVersionString;
      if ((server ?? "") == "")
      {
        MainForm.Instance.Text += " - (Local)";
      }
      else
      {
        MainForm instance2 = MainForm.Instance;
        instance2.Text = instance2.Text + " - " + server;
      }
      return true;
    }

    private bool performLicenseChecks()
    {
      LicenseInfo serverLicense = Session.ServerLicense;
      if (serverLicense.IsTrialVersion && !this.showTrialLicensePrompt(serverLicense))
        return false;
      if (Session.UserInfo.IsAdministrator())
        this.performUserLimitCheck(serverLicense);
      return true;
    }

    private bool showTrialLicensePrompt(LicenseInfo license)
    {
      if (new TrialUpgradeForm(license).ShowDialog((IWin32Window) this) != DialogResult.OK)
        return true;
      Application.Exit();
      return false;
    }

    private void performUserLimitCheck(LicenseInfo license)
    {
      int enabledUserCount = Session.OrganizationManager.GetEnabledUserCount();
      if (license.UserLimit <= 0 || enabledUserCount <= license.UserLimit)
        return;
      int num = (int) Utils.Dialog((IWin32Window) this, "Your Encompass system is in violation of the Encompass licensing agreement." + Environment.NewLine + Environment.NewLine + "Your license permits " + (object) license.UserLimit + " enabled user(s), and you currently have " + (object) enabledUserCount + " enabled users. You will be unable to create or enable user accounts until you purchase additional user licenses or delete/disable existing users.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
    }

    private void syncCompanySettings()
    {
      try
      {
        using (InProcConnection inProcConnection = new InProcConnection())
        {
          inProcConnection.OpenTrusted();
          IConfigurationManager configurationManager = (IConfigurationManager) inProcConnection.Session.GetObject("ConfigurationManager");
          CompanyInfo companyInfo = configurationManager.GetCompanyInfo();
          if (companyInfo.ClientID == Session.CompanyInfo.ClientID && companyInfo.Password != Session.CompanyInfo.Password && Session.CompanyInfo.Password != "")
            configurationManager.SetCompanySetting("CLIENT", "CLIENTPASSWORD", Session.CompanyInfo.Password);
          if (!(companyInfo.ClientID == Session.CompanyInfo.ClientID))
            return;
          configurationManager.UpdateCompanyDataServicesOpt(Session.ConfigurationManager.GetEncompassSystemInfo().DataServicesOptKey);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(LoginScreen.sw, nameof (LoginScreen), TraceLevel.Warning, "Company password sync failed due to error: " + (object) ex);
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

    private void downloadCompanyPassword()
    {
      try
      {
        Session.RecacheCompanyInfo();
      }
      catch (Exception ex)
      {
        Tracing.Log(LoginScreen.sw, TraceLevel.Warning, nameof (LoginScreen), "Unable to set local password from server: " + ex.Message);
      }
    }

    private void LoginScreen_Load(object sender, EventArgs e)
    {
      if (this.requiredServerName == null)
        return;
      this.connectionBox.SelectedIndex = 1;
      this.connectionBox.Enabled = false;
      this.pnlConnection.Visible = false;
      this.serverBox.Enabled = this.donotLockServer;
      if (this.serverBox.Enabled && (!this.serverBox.Enabled || this.serverBox.Items.Count != 0))
        return;
      this.serverBox.SelectedIndex = -1;
      this.serverBox.Text = this.requiredServerName;
    }

    private void LoginScreen_Shown(object sender, EventArgs e)
    {
      if (!(this.loginNameBox.Text != string.Empty))
        return;
      this.passwordBox.Focus();
    }

    private void LoginScreen_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Escape)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
    {
      this.tsMenuItemServer.Text = this.serverBox.Text;
    }

    private void label3_DoubleClick(object sender, EventArgs e)
    {
      if (this.serverBox.Text == "(local)")
        return;
      this.serverBox.Enabled = !this.serverBox.Enabled;
    }

    private void panel1_DoubleClick(object sender, EventArgs e)
    {
      if (this.serverBox.Text == "(local)")
        return;
      this.serverBox.Enabled = true;
    }

    private DateTime getServerTime(string server)
    {
      if (!LoginScreen.channelInitialized)
      {
        Connection connection = new Connection();
        LoginScreen.channelInitialized = true;
      }
      return ((ILoginManager) Activator.GetObject(typeof (ILoginManager), ServerIdentity.Parse(server).Uri.AbsoluteUri + "LoginManager.rem", (object) null)).ServerTime();
    }

    private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
    {
      try
      {
        this.tsMenuItemServerTime.Text = this.getServerTime(this.serverBox.Text.Trim()).ToString("yyyy-MM-dd HH:mm:ss");
      }
      catch (Exception ex)
      {
        Tracing.Log(LoginScreen.sw, nameof (LoginScreen), TraceLevel.Error, "Error getting server time: " + ex.Message);
      }
    }

    public static DateTime GetLoginStartTime() => LoginScreen.loginStartTime;
  }
}
