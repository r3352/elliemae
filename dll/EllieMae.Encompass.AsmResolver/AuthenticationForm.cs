// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AuthenticationForm
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using EllieMae.Encompass.AsmResolver.MOTD;
using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using SCAppMgr;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.Encompass.AsmResolver
{
  internal class AuthenticationForm : Form
  {
    public static AuthenticationForm LastAuthentication = (AuthenticationForm) null;
    private const string defaultAuthServerURL = "https://hosted.elliemae.com";
    private static string scIDUnknown = "";
    private AuthenticationResult authResult;
    private bool continueInOfflineSelected;
    private string appCompanyName;
    private string appSuiteName;
    private string appNameWExt;
    private bool offlineReady;
    private string uacHashFolder;
    private string _userid;
    private IContainer components;
    private Label label1;
    private Label label2;
    private TextBox txtBoxServerURL;
    private Button btnLogin;
    private Button btnCancel;
    private CheckBox chkBoxAutoSignOn;
    private Button btnGetCID;
    private Button btnManageCIDs;
    private ComboBox cmbBoxUserid;
    private ComboBox cmbBoxServerURL;

    private AuthenticationForm(
      string authServerURL,
      string userid,
      string password,
      string appCompanyName,
      string appSuiteName,
      string appNameWExt,
      bool offlineReady,
      string uacHashFolder,
      bool autoSignOn,
      string[] clientIDs,
      string scid = null)
    {
      this.InitializeComponent();
      this._userid = userid;
      this.Text = (Path.GetFileNameWithoutExtension(appNameWExt) ?? "").Trim();
      if (this.Text.ToLower().StartsWith(ResolverConsts.AppLauncher.ToLower()))
      {
        string[] commandLineArgs = Environment.GetCommandLineArgs();
        if (commandLineArgs.Length < 2)
          this.Text = "Encompass - Launcher";
        else
          this.Text = Path.GetFileNameWithoutExtension(commandLineArgs[1]) + " - Launcher";
      }
      else if (this.Text == "ClickLoanMain")
        this.Text = "Encompass";
      this.txtBoxServerURL.Text = (authServerURL ?? "").Trim();
      if (this.txtBoxServerURL.Text == "")
        this.txtBoxServerURL.Text = "https://hosted.elliemae.com";
      this.populateSmartClientIDList(clientIDs, userid);
      this.appCompanyName = appCompanyName;
      this.appSuiteName = appSuiteName;
      this.appNameWExt = appNameWExt;
      this.offlineReady = offlineReady;
      this.uacHashFolder = uacHashFolder;
      this.chkBoxAutoSignOn.Checked = autoSignOn;
      this.ActiveControl = (Control) this.cmbBoxUserid;
      this.populateCmbBoxServerURL(appCompanyName);
      if (string.IsNullOrWhiteSpace(scid))
        return;
      this.cmbBoxUserid.Text = scid;
      this.cmbBoxUserid.Enabled = false;
    }

    private void populateCmbBoxServerURL(string appCompanyName)
    {
      try
      {
        this.cmbBoxServerURL.Items.Clear();
        this.cmbBoxServerURL.SelectedIndexChanged -= new EventHandler(this.cmbBoxServerURL_SelectedIndexChanged);
        string name = "Software\\" + appCompanyName + "\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive(appCompanyName).OpenSubKey(name, false))
        {
          string str = (string) registryKey.GetValue("AuthServerURLs");
          if (str == null)
            return;
          string[] items = str.Split(new char[4]
          {
            ',',
            ';',
            ' ',
            '\t'
          }, StringSplitOptions.RemoveEmptyEntries);
          if (items == null || items.Length == 0)
            return;
          this.cmbBoxServerURL.Items.Add((object) this.txtBoxServerURL.Text);
          this.cmbBoxServerURL.Items.AddRange((object[]) items);
          this.cmbBoxServerURL.SelectedIndex = 0;
          this.cmbBoxServerURL.Visible = true;
          this.txtBoxServerURL.Visible = false;
          this.cmbBoxServerURL.SelectedIndexChanged += new EventHandler(this.cmbBoxServerURL_SelectedIndexChanged);
        }
      }
      catch (SecurityException ex)
      {
        AssemblyResolver.Instance.WriteToEventLog("Error populating auth server url combobox: " + ex.Message, EventLogEntryType.Warning);
      }
    }

    private void cmbBoxServerURL_TextChanged(object sender, EventArgs e)
    {
      this.serverUrlCmbBoxChanged();
    }

    private void cmbBoxServerURL_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.serverUrlCmbBoxChanged();
    }

    private void serverUrlCmbBoxChanged()
    {
      if (!this.cmbBoxServerURL.Visible)
        return;
      try
      {
        this.txtBoxServerURL.Text = this.cmbBoxServerURL.Text;
      }
      catch (Exception ex)
      {
        AssemblyResolver.Instance.WriteToEventLog("Error in cmbBoxServerURL_SelectedIndexChanged: " + ex.Message, EventLogEntryType.Warning);
      }
    }

    private void populateSmartClientIDList(string[] clientIDs, string userid)
    {
      this.cmbBoxUserid.Items.Clear();
      if (clientIDs != null && clientIDs.Length != 0)
      {
        foreach (object clientId in clientIDs)
          this.cmbBoxUserid.Items.Add(clientId);
      }
      this.cmbBoxUserid.Text = (userid ?? "").Trim();
      if (!(this.cmbBoxUserid.Text == ""))
        return;
      if (this.cmbBoxUserid.Items.Count > 0)
        this.cmbBoxUserid.SelectedIndex = 0;
      else
        this.cmbBoxUserid.Text = AuthenticationForm.scIDUnknown;
    }

    internal AuthenticationResult AuthResult => this.authResult;

    private AuthResultCode authResultCode
    {
      get
      {
        return this.authResult == null ? AuthResultCode.UnhandledException : this.authResult.ResultCode;
      }
    }

    private string installationURL
    {
      get => this.authResult == null ? (string) null : this.authResult.InstallationURL;
    }

    internal NameValueCollection AppCmdLineArgs
    {
      get => this.authResult == null ? (NameValueCollection) null : this.authResult.AppCmdLineArgs;
    }

    internal NameValueCollection Attributes
    {
      get => this.authResult == null ? (NameValueCollection) null : this.authResult.Attributes;
    }

    internal string AuthServerURL
    {
      get
      {
        string authServerUrl = this.txtBoxServerURL.Text.Trim();
        if (authServerUrl.EndsWith("/"))
          authServerUrl = authServerUrl.Substring(0, authServerUrl.Length - 1);
        return authServerUrl;
      }
    }

    internal string Userid => this._userid;

    internal string Password => this._userid;

    internal bool ContinueInOfflineSelected => this.continueInOfflineSelected;

    internal bool AutoSignOn => this.chkBoxAutoSignOn.Checked;

    private static string getDesiredEncompassVersion(string userid)
    {
      string encompassVersion = "";
      string name = "Software\\Ellie Mae\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/") + "\\Encompass\\" + userid;
      BasicUtils.DisplayDebuggingInfo("DesiredVersion", "Getting desired Encompass version from HKCU\\" + name);
      using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name))
      {
        if (registryKey != null)
          encompassVersion = (string) registryKey.GetValue("Version") ?? "";
      }
      BasicUtils.DisplayDebuggingInfo("DesiredVersion", "Got desired Encompass version: " + encompassVersion);
      return encompassVersion;
    }

    private void btnGetCID_Click(object sender, EventArgs e)
    {
      if (new ClientIDInfoForm().ShowDialog() == DialogResult.Cancel)
        return;
      this._userid = "3012345678";
      this.login(this._userid);
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
      if (this.cmbBoxUserid.Text.Trim() == "" || !char.IsDigit(this.cmbBoxUserid.Text[0]) && !char.IsLetter(this.cmbBoxUserid.Text[0]))
      {
        this._userid = "3012345678";
        this.login(this._userid);
      }
      else
      {
        this._userid = this.cmbBoxUserid.Text.Trim();
        this.login(this._userid);
      }
    }

    private void login(string userid)
    {
      this.continueInOfflineSelected = false;
      this.DialogResult = DialogResult.None;
      try
      {
        string encompassVersion = AuthenticationForm.getDesiredEncompassVersion(userid);
        this.authResult = AuthenticationForm.authenticate(this.AuthServerURL, userid, this.Password, this.appCompanyName, this.appSuiteName, this.appNameWExt, encompassVersion);
      }
      catch (Exception ex)
      {
        if (!this.offlineReady)
        {
          int num = (int) MessageBox.Show("Login failed with the following error:\r\n" + ex.Message);
          return;
        }
        if (Application.ExecutablePath.Trim().ToLower().EndsWith(ResolverConsts.AppLauncherExe.ToLower()))
        {
          this.continueInOfflineSelected = true;
          this.DialogResult = this.btnLogin.DialogResult;
        }
        else if (MessageBox.Show("Login failed with the following error. Since the application has been downloaded completely, do you want to continue using the application?\r\n\r\n" + ex.Message, "Authentication", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
        {
          this.continueInOfflineSelected = true;
          this.DialogResult = this.btnLogin.DialogResult;
        }
        if (!this.continueInOfflineSelected)
          return;
        NameValueCollection appCmdLineArgs = new NameValueCollection();
        NameValueCollection attributes = new NameValueCollection();
        MOTDSettings motdSettings = (MOTDSettings) null;
        string name1 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\AppCmdLineArgs";
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(name1))
        {
          if (registryKey != null)
          {
            foreach (string valueName in registryKey.GetValueNames())
              appCmdLineArgs.Add(valueName, (string) registryKey.GetValue(valueName));
          }
        }
        string name2 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\Attributes";
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(name2))
        {
          if (registryKey != null)
          {
            foreach (string valueName in registryKey.GetValueNames())
              attributes.Add(valueName, (string) registryKey.GetValue(valueName));
          }
        }
        this.authResult = new AuthenticationResult(AuthResultCode.OfflineMode, "Authentication exception occurred but continue because of offline-ready", appCmdLineArgs, attributes, motdSettings);
        return;
      }
      if (this.authResult.ResultCode == AuthResultCode.Success)
      {
        this.DialogResult = this.btnLogin.DialogResult;
      }
      else
      {
        int num1 = (int) MessageBox.Show("Login failed with the following error:\r\n" + this.authResult.ResultDescription);
      }
    }

    private static void checkAndSetRegistryPermission(string appCompanyName)
    {
      try
      {
        string name = "Software\\" + appCompanyName + "\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
        using (BasicUtils.GetRegistryHive(appCompanyName).OpenSubKey(name, true))
          ;
      }
      catch
      {
        try
        {
          ((AppMgrRemoteObject) Activator.GetObject(typeof (AppMgrRemoteObject), "ipc://SCAppMgr/AppMgrRemoteObject.rem")).DownloadAndExecute(true, "/install/SCAppMgrApps/SetRegistryPermission.exe", "", true);
          AssemblyResolver.Instance.WriteToEventLog("SCAppMgrApps/SetRegistryPermission.exe executed", EventLogEntryType.Information);
        }
        catch (Exception ex)
        {
          AssemblyResolver.Instance.WriteToEventLog("Error setting registry permission: " + ex.Message, EventLogEntryType.Error);
        }
      }
    }

    public static AuthenticationResult Authenticate(
      string appCompanyName,
      string appSuiteName,
      string appNameWExt,
      bool offlineReady,
      out bool continueInOfflineSelected,
      string uacHashFolder,
      string scid = null)
    {
      continueInOfflineSelected = false;
      string str1 = (string) null;
      bool autoSignOn = true;
      List<string> stringList = new List<string>();
      string str2 = (string) null;
      string password = (string) null;
      AuthResultCode authResultCode = AuthResultCode.Success;
      NameValueCollection nameValueCollection1 = (NameValueCollection) null;
      NameValueCollection nameValueCollection2 = (NameValueCollection) null;
      AuthenticationForm.checkAndSetRegistryPermission(appCompanyName);
      bool flag1 = true;
      RegistryKey registryKey1 = (RegistryKey) null;
      string str3 = "Software\\" + appCompanyName + "\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
      try
      {
        RegistryKey registryHive = BasicUtils.GetRegistryHive(appCompanyName);
        try
        {
          registryKey1 = registryHive.OpenSubKey(str3, true);
        }
        catch (SecurityException ex)
        {
          AssemblyResolver.Instance.WriteToEventLog("[Authentication Form] Error opening registry '" + str3 + "' for write access:" + ex.Message, EventLogEntryType.Warning);
          registryKey1 = registryHive.OpenSubKey(str3);
          flag1 = false;
        }
        if (registryKey1 == null)
        {
          registryKey1 = registryHive.CreateSubKey(str3);
          registryKey1.SetValue("AppStartupPathHash", (object) HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, AssemblyResolver.AppStartupPath));
        }
        else
        {
          if (flag1 && BasicUtils.IsNullOrEmpty((string) registryKey1.GetValue("AppStartupPathHash")))
            registryKey1.SetValue("AppStartupPathHash", (object) HashUtil.ComputeHashB64FilePath(HashAlgorithm.SHA1, AssemblyResolver.AppStartupPath));
          str1 = (string) registryKey1.GetValue("AuthServerURL");
          autoSignOn = ((string) registryKey1.GetValue("AutoSignOn") ?? "").Trim() != "0";
          string str4 = (string) registryKey1.GetValue("SmartClientIDs");
          if (str4 != null)
          {
            string[] collection = str4.Split(new char[3]
            {
              ',',
              ';',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
            stringList.AddRange((IEnumerable<string>) collection);
          }
          string dd = (string) registryKey1.GetValue("Credentials");
          if (!BasicUtils.IsNullOrEmpty(str1) && dd != null)
          {
            string[] strArray = XT.DSB64(dd, ResolverConsts.KB64).Split('\n');
            str2 = strArray[0];
            password = strArray[1];
            if (string.IsNullOrWhiteSpace(scid) || str2 == scid)
            {
              AuthenticationResult authenticationResult;
              try
              {
                string encompassVersion = AuthenticationForm.getDesiredEncompassVersion(str2);
                authenticationResult = AuthenticationForm.authenticate(str1, str2, password, appCompanyName, appSuiteName, appNameWExt, encompassVersion);
              }
              catch (Exception ex)
              {
                authenticationResult = new AuthenticationResult(AuthResultCode.UnhandledException, ex.Message, (NameValueCollection) null, (NameValueCollection) null, (MOTDSettings) null);
              }
              authResultCode = authenticationResult.ResultCode;
              string installationUrl = authenticationResult.InstallationURL;
              nameValueCollection1 = authenticationResult.AppCmdLineArgs;
              nameValueCollection2 = authenticationResult.Attributes;
              int num1;
              switch (authResultCode)
              {
                case AuthResultCode.Success:
label_19:
                  AuthenticationForm.LastAuthentication = new AuthenticationForm(str1, str2, password, appCompanyName, appSuiteName, appNameWExt, offlineReady, uacHashFolder, autoSignOn, stringList.ToArray());
                  return authenticationResult;
                case AuthResultCode.UseridNotFound:
                case AuthResultCode.InvalidPwd:
                  num1 = 0;
                  break;
                default:
                  num1 = Application.ExecutablePath.Trim().ToLower().EndsWith(ResolverConsts.AppLauncherExe.ToLower()) ? 1 : 0;
                  break;
              }
              int num2 = offlineReady ? 1 : 0;
              if ((num1 & num2) != 0)
                goto label_19;
            }
          }
        }
        str2 = !string.IsNullOrWhiteSpace(scid) ? scid : (string) registryKey1.GetValue("LastSmartClientID");
        bool flag2 = false;
        using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\SmartClient\\Encompass"))
        {
          if (registryKey2 != null)
            flag2 = (registryKey2.GetValue("SH2SC")?.ToString() ?? "").Trim() == "1";
        }
        bool flag3 = false;
        AuthenticationForm.LastAuthentication = new AuthenticationForm(str1, str2, password, appCompanyName, appSuiteName, appNameWExt, offlineReady, uacHashFolder, autoSignOn, stringList.ToArray(), scid);
        bool flag4 = string.Compare(scid, "ScApp", true) == 0;
        if (flag4)
        {
          AuthenticationForm.LastAuthentication.cmbBoxUserid.Text = scid;
          AuthenticationForm.LastAuthentication.btnLogin_Click((object) null, (EventArgs) null);
        }
        else if (flag2)
        {
          if (stringList.Count == 0)
          {
            AuthenticationForm.LastAuthentication.cmbBoxUserid.Text = "@#*&$%^!";
            AuthenticationForm.LastAuthentication.btnLogin_Click((object) null, (EventArgs) null);
          }
          else if (stringList.Count == 1)
          {
            AuthenticationForm.LastAuthentication.cmbBoxUserid.Text = stringList[0];
            AuthenticationForm.LastAuthentication.btnLogin_Click((object) null, (EventArgs) null);
          }
          else
            flag3 = true;
        }
        if ((!flag2 | flag3 || !string.IsNullOrWhiteSpace(scid)) && !flag4 && AuthenticationForm.LastAuthentication.ShowDialog() == DialogResult.Cancel)
          Process.GetCurrentProcess().Kill();
        str1 = AuthenticationForm.LastAuthentication.AuthServerURL;
        str2 = AuthenticationForm.LastAuthentication.Userid;
        password = AuthenticationForm.LastAuthentication.Password;
        authResultCode = AuthenticationForm.LastAuthentication.authResultCode;
        string installationUrl1 = AuthenticationForm.LastAuthentication.installationURL;
        nameValueCollection1 = AuthenticationForm.LastAuthentication.AppCmdLineArgs;
        nameValueCollection2 = AuthenticationForm.LastAuthentication.Attributes;
        continueInOfflineSelected = AuthenticationForm.LastAuthentication.ContinueInOfflineSelected;
        return AuthenticationForm.LastAuthentication.AuthResult;
      }
      finally
      {
        if (flag1 && registryKey1 != null && (authResultCode == AuthResultCode.Success || authResultCode == AuthResultCode.NullInstallationURL))
        {
          registryKey1.SetValue("AuthServerURL", (object) str1);
          if (AuthenticationForm.LastAuthentication == null || AuthenticationForm.LastAuthentication.AutoSignOn)
          {
            if (str2 != "3012345678")
            {
              registryKey1.SetValue("AutoSignOn", (object) "1");
              registryKey1.SetValue("Credentials", (object) XT.ESB64(str2 + "\n" + password, ResolverConsts.KB64));
              registryKey1.DeleteValue("LastSmartClientID", false);
            }
          }
          else
          {
            registryKey1.SetValue("AutoSignOn", (object) "0");
            registryKey1.DeleteValue("Credentials", false);
            if (str2 != "3012345678")
              registryKey1.SetValue("LastSmartClientID", (object) str2);
          }
          if (str2 != "3012345678")
          {
            int index1 = -1;
            for (int index2 = 0; index2 < stringList.Count; ++index2)
            {
              if (string.Compare(stringList[index2], str2, true) == 0)
              {
                index1 = index2;
                break;
              }
            }
            if (index1 >= 0)
              stringList.RemoveAt(index1);
            stringList.Insert(0, str2);
            registryKey1.SetValue("SmartClientIDs", (object) string.Join(", ", stringList.ToArray()));
          }
          registryKey1.DeleteSubKey("AppCmdLineArgs", false);
          if (nameValueCollection1 != null && nameValueCollection1.Count > 0)
          {
            RegistryKey subKey = registryKey1.CreateSubKey("AppCmdLineArgs");
            foreach (string key in nameValueCollection1.Keys)
              subKey.SetValue(key, (object) nameValueCollection1[key]);
            subKey.Close();
          }
          registryKey1.DeleteSubKey("Attributes", false);
          if (nameValueCollection2 != null && nameValueCollection2.Count > 0)
          {
            RegistryKey subKey = registryKey1.CreateSubKey("Attributes");
            foreach (string key in nameValueCollection2.Keys)
              subKey.SetValue(key, (object) nameValueCollection2[key]);
            subKey.Close();
          }
        }
        registryKey1?.Close();
      }
    }

    private static AuthenticationResult authenticate(
      string serverURL,
      string userid,
      string password,
      string appCompanyName,
      string appSuiteName,
      string appNameWExt,
      string desiredVersion)
    {
      if ((userid ?? "").Trim() == "")
        return new AuthenticationResult(AuthResultCode.UnhandledException, "SmartClient ID cannot be null or empty", (NameValueCollection) null, (NameValueCollection) null, (MOTDSettings) null);
      ServerUri serverUri = new ServerUri(serverURL);
      string str = string.Format("?uid={0}&pwd={1}&company={2}&suite={3}&app={4}&version={5}", (object) HttpUtility.UrlEncode(userid), (object) HttpUtility.UrlEncode(password), (object) HttpUtility.UrlEncode(appCompanyName), (object) HttpUtility.UrlEncode(appSuiteName), (object) HttpUtility.UrlEncode(appNameWExt), (object) HttpUtility.UrlEncode(desiredVersion));
      if (BasicUtils.GetRegistryDebugLevel(appCompanyName) >= 2)
        AssemblyResolver.Instance.WriteToEventLog(Path.GetFileName(Application.ExecutablePath) + ": HTTP GET " + serverUri.ToString(), EventLogEntryType.Information);
      return AuthenticationResult.ToAuthResult(WebUtil.HttpGet(serverUri.ToString() + str, "&"));
    }

    private string[] getRegistrySmartClientIDs()
    {
      List<string> stringList = new List<string>();
      string name = "Software\\Ellie Mae\\SmartClient\\" + AssemblyResolver.AppStartupPath.Replace("\\", "/");
      try
      {
        using (RegistryKey registryKey = BasicUtils.GetRegistryHive((string) null).OpenSubKey(name))
        {
          if (registryKey == null)
            return (string[]) null;
          string str = (string) registryKey.GetValue("SmartClientIDs");
          if (str != null)
          {
            string[] collection = str.Split(new char[3]
            {
              ',',
              ';',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
            stringList.AddRange((IEnumerable<string>) collection);
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Error getting client IDs from registry: " + ex.Message, "Encompass SmartClient");
      }
      return stringList.ToArray();
    }

    private void btnManageCIDs_Click(object sender, EventArgs e)
    {
      int num = (int) new ClientIDsManagementForm(this.getRegistrySmartClientIDs()).ShowDialog();
      this.populateSmartClientIDList(this.getRegistrySmartClientIDs(), "");
    }

    private void label1_DoubleClick(object sender, EventArgs e)
    {
      this.txtBoxServerURL.ReadOnly = !this.txtBoxServerURL.ReadOnly;
    }

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
      this.txtBoxServerURL = new TextBox();
      this.btnLogin = new Button();
      this.btnCancel = new Button();
      this.chkBoxAutoSignOn = new CheckBox();
      this.btnGetCID = new Button();
      this.btnManageCIDs = new Button();
      this.cmbBoxUserid = new ComboBox();
      this.cmbBoxServerURL = new ComboBox();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(5, 22);
      this.label1.Name = "label1";
      this.label1.Size = new Size(94, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "SmartClient Server";
      this.label1.DoubleClick += new EventHandler(this.label1_DoubleClick);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(5, 55);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "SmartClient ID";
      this.txtBoxServerURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBoxServerURL.Location = new Point(110, 19);
      this.txtBoxServerURL.Name = "txtBoxServerURL";
      this.txtBoxServerURL.ReadOnly = true;
      this.txtBoxServerURL.Size = new Size(294, 20);
      this.txtBoxServerURL.TabIndex = 1;
      this.btnLogin.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnLogin.DialogResult = DialogResult.OK;
      this.btnLogin.Location = new Point(228, 86);
      this.btnLogin.Name = "btnLogin";
      this.btnLogin.Size = new Size(85, 23);
      this.btnLogin.TabIndex = 4;
      this.btnLogin.Text = "Login";
      this.btnLogin.UseVisualStyleBackColor = true;
      this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(319, 86);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(85, 23);
      this.btnCancel.TabIndex = 6;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.chkBoxAutoSignOn.AutoSize = true;
      this.chkBoxAutoSignOn.Checked = true;
      this.chkBoxAutoSignOn.CheckState = CheckState.Checked;
      this.chkBoxAutoSignOn.Location = new Point(110, 90);
      this.chkBoxAutoSignOn.Name = "chkBoxAutoSignOn";
      this.chkBoxAutoSignOn.Size = new Size(89, 17);
      this.chkBoxAutoSignOn.TabIndex = 3;
      this.chkBoxAutoSignOn.Text = "Auto Sign-On";
      this.chkBoxAutoSignOn.UseVisualStyleBackColor = true;
      this.btnGetCID.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.btnGetCID.Location = new Point(8, 86);
      this.btnGetCID.Name = "btnGetCID";
      this.btnGetCID.Size = new Size(85, 23);
      this.btnGetCID.TabIndex = 7;
      this.btnGetCID.Text = "Get Client ID";
      this.btnGetCID.UseVisualStyleBackColor = true;
      this.btnGetCID.Visible = false;
      this.btnGetCID.Click += new EventHandler(this.btnGetCID_Click);
      this.btnManageCIDs.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnManageCIDs.Location = new Point(319, 50);
      this.btnManageCIDs.Name = "btnManageCIDs";
      this.btnManageCIDs.Size = new Size(85, 23);
      this.btnManageCIDs.TabIndex = 5;
      this.btnManageCIDs.Text = "Manage IDs";
      this.btnManageCIDs.UseVisualStyleBackColor = true;
      this.btnManageCIDs.Click += new EventHandler(this.btnManageCIDs_Click);
      this.cmbBoxUserid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxUserid.FormattingEnabled = true;
      this.cmbBoxUserid.Location = new Point(110, 51);
      this.cmbBoxUserid.Name = "cmbBoxUserid";
      this.cmbBoxUserid.Size = new Size(203, 21);
      this.cmbBoxUserid.TabIndex = 2;
      this.cmbBoxServerURL.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxServerURL.FormattingEnabled = true;
      this.cmbBoxServerURL.Location = new Point(110, 19);
      this.cmbBoxServerURL.Name = "cmbBoxServerURL";
      this.cmbBoxServerURL.Size = new Size(294, 21);
      this.cmbBoxServerURL.TabIndex = 8;
      this.cmbBoxServerURL.Visible = false;
      this.cmbBoxServerURL.TextChanged += new EventHandler(this.cmbBoxServerURL_TextChanged);
      this.AcceptButton = (IButtonControl) this.btnLogin;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(416, 121);
      this.Controls.Add((Control) this.cmbBoxUserid);
      this.Controls.Add((Control) this.btnManageCIDs);
      this.Controls.Add((Control) this.btnGetCID);
      this.Controls.Add((Control) this.chkBoxAutoSignOn);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnLogin);
      this.Controls.Add((Control) this.txtBoxServerURL);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cmbBoxServerURL);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Name = nameof (AuthenticationForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SmartClient Authentication";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
