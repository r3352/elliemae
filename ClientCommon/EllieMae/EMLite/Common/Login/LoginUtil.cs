// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Login.LoginUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.SmartClient;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Common.Login
{
  public class LoginUtil
  {
    private string _authCodeFromIDP = string.Empty;
    private string _userNameFromIDP = string.Empty;
    private string _passwordFromIDP = string.Empty;
    private string _instanceFromIDP = string.Empty;
    private string _className = nameof (LoginUtil);
    private AppName _appName;
    private string _server;
    private bool _donotLockServer;
    private string _url;
    private string serverInstance = string.Empty;
    private Func<LoginContext, bool> _loginRoutine;
    private static readonly string sw = Tracing.SwCommon;
    private static string defaultEncompassServerURL = string.Empty;
    private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

    public event LoginUtil.MFALoginFlowCompletedHandler OnMFALoginCompleted;

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(
      IntPtr hInternet,
      int dwOption,
      IntPtr lpBuffer,
      int lpdwBufferLength);

    public LoginUtil()
    {
      if (EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders.ContainsKey("X-ClientType"))
        return;
      EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders["X-ClientType"] = "Encompass";
    }

    public static bool IsTokenLoginOnly
    {
      get
      {
        return (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "TokenLoginOnly") != null ? (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "TokenLoginOnly") == "1" : AssemblyResolver.GetScAttribute("TokenLoginOnly") == "1";
      }
    }

    public static string IdpUrl
    {
      get
      {
        string registryValue = (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", nameof (IdpUrl));
        if (!string.IsNullOrWhiteSpace(registryValue))
          return registryValue;
        string scAttribute = AssemblyResolver.GetScAttribute(nameof (IdpUrl));
        return string.IsNullOrWhiteSpace(scAttribute) ? "https://idp.elliemae.com" : scAttribute;
      }
    }

    private DialogResult Login(LoginContext context)
    {
      if (!LoginUtil.IsTokenLoginOnly)
        throw new InvalidOperationException("This method should be called only when IsTokenLoginOnly is true");
      DialogResult dialogResult = DialogResult.None;
      if (this._loginRoutine(context))
        dialogResult = DialogResult.OK;
      return dialogResult;
    }

    public static string DefaultEncompassServerUrl
    {
      get
      {
        if (!string.IsNullOrEmpty(LoginUtil.defaultEncompassServerURL))
          return LoginUtil.defaultEncompassServerURL;
        string str = string.Empty;
        string name1 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\Attributes";
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name1))
        {
          if (registryKey != null)
            str = (string) registryKey.GetValue("EncompassServerUrl");
        }
        if (str == null)
          str = "";
        string name2 = "Software\\Ellie Mae\\SmartClient\\" + Application.StartupPath.Replace("\\", "/") + "\\AppCmdLineArgs";
        using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name2))
        {
          if (registryKey != null)
            str = (string) registryKey.GetValue("Encompass.exe");
        }
        LoginUtil.defaultEncompassServerURL = str.Replace("-s ", "");
        return LoginUtil.defaultEncompassServerURL;
      }
    }

    public static string DefaultInstanceID
    {
      get
      {
        return LoginUtil.DefaultEncompassServerUrl.Substring(LoginUtil.DefaultEncompassServerUrl.IndexOf('$') + 1);
      }
    }

    public ThinThickForm GetLoginForm(
      AppName appName,
      string server,
      bool donotLockServer,
      Func<LoginContext, bool> loginRoutine,
      string instanceId,
      string Title = null,
      string appClientId = null,
      string appScope = null,
      string redirectUrl = "")
    {
      ThinThickForm loginForm = new ThinThickForm(Title);
      loginForm.AddThinThickPage((UserControl) new EncompassLoginBrowser());
      loginForm.FormBorderStyle = FormBorderStyle.FixedSingle;
      string str1 = string.IsNullOrEmpty(appClientId) ? "n35xg3ze" : appClientId;
      string str2 = string.IsNullOrEmpty(appScope) ? "sc" : appScope;
      string str3 = string.IsNullOrEmpty(redirectUrl) ? "https://encompass.elliemae.com/homepage/atest.asp" : redirectUrl;
      string url = LoginUtil.IdpUrl + "/authorize?client_id=" + str1 + "&response_type=code&redirect_uri=" + str3 + "&scope=" + str2 + "&instance_id=" + instanceId + "&encompass_version=" + VersionInformation.CurrentVersion.DisplayVersion;
      loginForm.Navigate(url);
      loginForm.BeforeNavigate += new ThinThickForm.WebBrowserBeforeNavigateHandler(this.WebPageForm_BeforeNavigate);
      this.OnMFALoginCompleted += new LoginUtil.MFALoginFlowCompletedHandler(this.LoginUtil_OnMFALoginCompleted);
      loginForm.Navigated += new ThinThickForm.WebBrowserNavigatedHandler(this.WebPageForm_Navigated);
      this._server = server;
      this._donotLockServer = donotLockServer;
      this._loginRoutine = loginRoutine;
      this._appName = appName;
      this._url = url;
      return loginForm;
    }

    private void WebPageForm_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      if (sender is WebBrowser webBrowser && e.Url.ToString().Contains("code="))
      {
        this._authCodeFromIDP = HttpUtility.ParseQueryString(e.Url.ToString().Substring(e.Url.ToString().IndexOf('?')).Split('#')[0])["code"];
        if (string.IsNullOrWhiteSpace(this._authCodeFromIDP))
          return;
        Form parentForm = this.getParentForm((Control) sender);
        parentForm.Hide();
        if (this.OnMFALoginCompleted == null)
          return;
        this.OnMFALoginCompleted((object) parentForm, new LoginUtil.EncompassLoginCompletedArgs()
        {
          InstanceID = this._instanceFromIDP,
          AuthCode = this._authCodeFromIDP,
          UserName = this._userNameFromIDP,
          Password = this._passwordFromIDP
        });
      }
      else
      {
        if (!e.Url.ToString().StartsWith("https://encompass.elliemae.com/homepage/atest.asp"))
          return;
        string uriString = this._url + "&" + e.Url.Query.Remove(0, 1);
        LoginUtil.InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);
        webBrowser.Navigate(new Uri(uriString, UriKind.Absolute));
        webBrowser.Refresh(WebBrowserRefreshOption.Completely);
      }
    }

    private void LoginUtil_OnMFALoginCompleted(
      object sender,
      LoginUtil.EncompassLoginCompletedArgs args)
    {
      Form form = sender as Form;
      form.Invoke((Delegate) (() =>
      {
        if (!this._server.ToLower().Contains(args.InstanceID.ToLower()))
          this._server = string.Format("https://{0}.ea.elliemae.net/Encompass${0}", (object) args.InstanceID);
        try
        {
          form.DialogResult = this.Login(new LoginContext()
          {
            AppName = this._appName,
            UserId = this._userNameFromIDP,
            Password = this._passwordFromIDP,
            Server = this._server,
            InstanceID = this._instanceFromIDP,
            AuthCode = this._authCodeFromIDP
          });
        }
        catch (Exception ex)
        {
          Tracing.Log(LoginUtil.sw, TraceLevel.Error, this._className, ex.StackTrace);
        }
        finally
        {
          form.Close();
        }
      }));
    }

    private void WebPageForm_BeforeNavigate(object sender, WebBrowserNavigatingEventArgs e)
    {
      WebBrowser webBrowser = sender as WebBrowser;
      if (webBrowser.Document.GetElementById("instanceId") != (HtmlElement) null)
        this._instanceFromIDP = webBrowser.Document.GetElementById("instanceId").GetAttribute("value");
      if (webBrowser == null || !e.Url.ToString().Contains("authorization.ping"))
        return;
      this._userNameFromIDP = webBrowser.Document.GetElementById("userId")?.GetAttribute("value");
      this._passwordFromIDP = webBrowser.Document.GetElementById("password")?.GetAttribute("value");
    }

    private Form getParentForm(Control sender)
    {
      for (Control parent = sender.Parent; parent != null; parent = parent.Parent)
      {
        if (parent is Form)
          return parent as Form;
      }
      return (Form) null;
    }

    public static ClientInfo getSmartClientSessionInfo(
      Sessions.Session session,
      string scID,
      string server)
    {
      VersionManagementGroup versionManagementGroup = session.ServerManager.GetDefaultVersionManagementGroup();
      return new ClientInfo()
      {
        RuntimeEnvironment = session.StartupInfo.RuntimeEnvironment,
        ClientID = session.CompanyInfo.ClientID,
        Password = session.CompanyInfo.Password,
        EncompassSystemID = session.EncompassSystemID,
        SqlDbID = session.SqlDbID,
        EncompassEdition = session.EncompassEdition,
        IsLegacySettingAutoUpdate = versionManagementGroup.AuthorizedVersion == null,
        SessionInfoValuePair = LoginUtil.GetSessionInfoNameValuePairs(session, scID, server),
        UserId = session.UserID
      };
    }

    private static NameValuePair[] GetSessionInfoNameValuePairs(
      Sessions.Session session,
      string scID,
      string server)
    {
      List<NameValuePair> nameValuePairList = new List<NameValuePair>();
      try
      {
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "SmartClientID",
          Value = (object) (scID ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "Userid",
          Value = (object) session.UserID
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "UserPassword",
          Value = (object) XT.ESB64(session.Password, KB.SC64)
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "UserFirstName",
          Value = (object) (session.UserInfo.FirstName ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "UserLastName",
          Value = (object) (session.UserInfo.LastName ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "UserEmail",
          Value = (object) (session.UserInfo.Email ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "ProgramName",
          Value = (object) Path.GetFileNameWithoutExtension(Application.ExecutablePath)
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "EncompassServer",
          Value = (object) (server ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "EncompassVersion",
          Value = (object) (VersionInformation.CurrentVersion.DisplayVersion ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "DbVersion",
          Value = (object) (session.DbVersion ?? "")
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "ServerVersion",
          Value = (object) VersionInformation.CurrentVersion.GetExtendedVersion(session.EncompassEdition)
        });
        LoginParameters loginParameters = new LoginParameters("", "", (ServerIdentity) null, "", "", false);
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "ClientHostName",
          Value = (object) loginParameters.Hostname
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "Windows",
          Value = (object) loginParameters.OSVersion.Version.ToString()
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "Processor",
          Value = (object) loginParameters.ProcessorID
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "Speed",
          Value = (object) loginParameters.ProcessorSpeed
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "Memory",
          Value = (object) string.Concat((object) ((int) ((long) (loginParameters.TotalMemory / 1024UL) + 500L) / 1000))
        });
        Tuple<string, string> netFxInfo = SmartClientUtils.getNetFxInfo();
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "NetFxRelease",
          Value = (object) netFxInfo?.Item1
        });
        nameValuePairList.Add(new NameValuePair()
        {
          Name = "NetFxVersion",
          Value = (object) netFxInfo?.Item2
        });
      }
      catch (Exception ex)
      {
        Tracing.Log(true, "Error", "LoginHelper", "Error getting data for updating client info: " + ex.Message);
      }
      return nameValuePairList.ToArray();
    }

    public delegate void MFALoginFlowCompletedHandler(
      object sender,
      LoginUtil.EncompassLoginCompletedArgs args);

    public class EncompassLoginCompletedArgs : EventArgs
    {
      public string UserName { get; set; }

      public string Password { get; set; }

      public string InstanceID { get; set; }

      public string AuthCode { get; set; }
    }
  }
}
