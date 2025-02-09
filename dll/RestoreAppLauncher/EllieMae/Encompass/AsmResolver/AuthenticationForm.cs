// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.AuthenticationForm
// Assembly: RestoreAppLauncher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DF703729-AA3A-440A-B03B-08F970F67A28
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\RestoreAppLauncher.exe

using EllieMae.Encompass.AsmResolver.Utils;
using Microsoft.Win32;
using RestoreAppLauncher;
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
    private static string defaultAuthServerURL = "https://hosted.elliemae.com";
    private static readonly string appSuiteName = "EncompassBE";
    public static AuthenticationForm LastAuthentication = (AuthenticationForm) null;
    private string appNameWExt;
    private AuthenticationResult authResult;
    private string _userid;
    private IContainer components;
    private Label label1;
    private Label label2;
    private TextBox txtBoxServerURL;
    private Button btnLogin;
    private Button btnCancel;
    private ComboBox cmbBoxUserid;
    private ComboBox cmbBoxServerURL;

    private AuthenticationForm(
      string authServerURL,
      string userid,
      string password,
      string appNameWExt,
      string[] clientIDs)
    {
      this.InitializeComponent();
      this.Text = "Encompass - Restore Applauncher";
      this._userid = userid;
      this.appNameWExt = appNameWExt;
      this.ActiveControl = (Control) this.cmbBoxUserid;
      this.txtBoxServerURL.Text = (authServerURL ?? "").Trim();
      if (this.txtBoxServerURL.Text == "")
        this.txtBoxServerURL.Text = AuthenticationForm.defaultAuthServerURL;
      this.populateSmartClientIDList(clientIDs, userid);
      this.populateCmbBoxServerURL();
    }

    private void populateCmbBoxServerURL()
    {
      try
      {
        this.cmbBoxServerURL.Items.Clear();
        this.cmbBoxServerURL.SelectedIndexChanged -= new EventHandler(this.cmbBoxServerURL_SelectedIndexChanged);
        string scRegKeyPath = StartupObject.Instance.ScRegKeyPath;
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(scRegKeyPath, false))
        {
          if (registryKey == null)
            return;
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
          if (items == null || items.Length <= 0)
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
        if (BasicUtils.RegistryDebugLevel < 1)
          return;
        EventLog.WriteEntry(Consts.EventLogSource, "Error populating auth server url combobox: " + ex.Message, EventLogEntryType.Warning);
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
      this.txtBoxServerURL.Text = this.cmbBoxServerURL.Text;
    }

    private void populateSmartClientIDList(string[] clientIDs, string userid)
    {
      this.cmbBoxUserid.Items.Clear();
      if (clientIDs != null && clientIDs.Length > 0)
      {
        foreach (object clientId in clientIDs)
          this.cmbBoxUserid.Items.Add(clientId);
      }
      this.cmbBoxUserid.Text = (userid ?? "").Trim();
      if (!(this.cmbBoxUserid.Text == "") || this.cmbBoxUserid.Items.Count <= 0)
        return;
      this.cmbBoxUserid.SelectedIndex = 0;
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

    private void btnLogin_Click(object sender, EventArgs e)
    {
      if (this.cmbBoxUserid.Text.Trim() == "")
      {
        int num = (int) MessageBox.Show("Please enter a valid SmartClient ID.", "SmartClient", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        this._userid = this.cmbBoxUserid.Text.Trim();
        this.login(this._userid);
      }
    }

    private void login(string userid)
    {
      this.DialogResult = DialogResult.None;
      try
      {
        this.authResult = AuthenticationForm.authenticate(this.AuthServerURL, userid, this.Password, this.appNameWExt);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Login failed with the following error:\r\n" + ex.Message);
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

    public static AuthenticationResult Authenticate(string appNameWExt)
    {
      string str1 = (string) null;
      List<string> stringList = new List<string>();
      string userid1 = (string) null;
      string password1 = (string) null;
      AuthResultCode authResultCode = AuthResultCode.Success;
      RegistryKey registryKey = (RegistryKey) null;
      string scRegKeyPath = StartupObject.Instance.ScRegKeyPath;
      try
      {
        registryKey = Registry.LocalMachine.OpenSubKey(scRegKeyPath);
        if (registryKey != null)
        {
          str1 = (string) registryKey.GetValue("AuthServerURL");
          bool flag = ((string) registryKey.GetValue("AutoSignOn") ?? "").Trim() != "0";
          string str2 = (string) registryKey.GetValue("SmartClientIDs");
          if (str2 != null)
          {
            string[] collection = str2.Split(new char[3]
            {
              ',',
              ';',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
            stringList.AddRange((IEnumerable<string>) collection);
          }
          string dd = (string) registryKey.GetValue("Credentials");
          if (flag && !string.IsNullOrWhiteSpace(str1) && dd != null)
          {
            string[] strArray = XT.DSB64(dd, Consts.KB64).Split('\n');
            userid1 = strArray[0];
            password1 = strArray[1];
            AuthenticationResult authenticationResult;
            try
            {
              authenticationResult = AuthenticationForm.authenticate(str1, userid1, password1, appNameWExt);
            }
            catch (Exception ex)
            {
              authenticationResult = new AuthenticationResult(AuthResultCode.UnhandledException, ex.Message, (NameValueCollection) null, (NameValueCollection) null);
            }
            if (authenticationResult.ResultCode == AuthResultCode.Success)
            {
              AuthenticationForm.LastAuthentication = new AuthenticationForm(str1, userid1, password1, appNameWExt, stringList.ToArray());
              return authenticationResult;
            }
          }
        }
        if (registryKey != null)
          userid1 = (string) registryKey.GetValue("LastSmartClientID");
        AuthenticationForm.LastAuthentication = new AuthenticationForm(str1, userid1, password1, appNameWExt, stringList.ToArray());
        if (AuthenticationForm.LastAuthentication.ShowDialog() == DialogResult.Cancel)
          Process.GetCurrentProcess().Kill();
        string authServerUrl = AuthenticationForm.LastAuthentication.AuthServerURL;
        string userid2 = AuthenticationForm.LastAuthentication.Userid;
        string password2 = AuthenticationForm.LastAuthentication.Password;
        authResultCode = AuthenticationForm.LastAuthentication.authResultCode;
        return AuthenticationForm.LastAuthentication.AuthResult;
      }
      finally
      {
        registryKey?.Close();
      }
    }

    private static AuthenticationResult authenticate(
      string serverURL,
      string userid,
      string password,
      string appNameWExt)
    {
      if ((userid ?? "").Trim() == "")
        return new AuthenticationResult(AuthResultCode.UnhandledException, "SmartClient ID cannot be null or empty", (NameValueCollection) null, (NameValueCollection) null);
      string str1 = serverURL + "/EncompassSC/Default.aspx";
      string str2 = string.Format("?uid={0}&pwd={1}&company={2}&suite={3}&app={4}", (object) HttpUtility.UrlEncode(userid), (object) HttpUtility.UrlEncode(password), (object) HttpUtility.UrlEncode(Consts.AppCompanyName), (object) HttpUtility.UrlEncode(AuthenticationForm.appSuiteName), (object) HttpUtility.UrlEncode(appNameWExt));
      if (BasicUtils.RegistryDebugLevel >= 3)
        EventLog.WriteEntry(Consts.EventLogSource, Path.GetFileName(Application.ExecutablePath) + ": HTTP GET " + str1, EventLogEntryType.Information);
      return AuthenticationResult.ToAuthResult(WebUtil.HttpGet(str1 + str2, "&"));
    }

    private string[] getRegistrySmartClientIDs()
    {
      List<string> stringList = new List<string>();
      string scRegKeyPath = StartupObject.Instance.ScRegKeyPath;
      try
      {
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(scRegKeyPath))
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
        int num = (int) MessageBox.Show("Error getting client IDs from registry: " + ex.Message, Consts.EncompassSmartClient, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return stringList.ToArray();
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (AuthenticationForm));
      this.label1 = new Label();
      this.label2 = new Label();
      this.txtBoxServerURL = new TextBox();
      this.btnLogin = new Button();
      this.btnCancel = new Button();
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
      this.cmbBoxUserid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.cmbBoxUserid.FormattingEnabled = true;
      this.cmbBoxUserid.Location = new Point(110, 51);
      this.cmbBoxUserid.Name = "cmbBoxUserid";
      this.cmbBoxUserid.Size = new Size(294, 21);
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
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnLogin);
      this.Controls.Add((Control) this.txtBoxServerURL);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.cmbBoxServerURL);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = nameof (AuthenticationForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SmartClient Authentication";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
