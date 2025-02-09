// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.Login.WebLoginUtil
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using Elli.Web.Host.BrowserControls;
using Elli.Web.Host.EventObjects;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Login;
using EllieMae.EMLite.Common.Version;
using EllieMae.Encompass.AsmResolver;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Web;
using System.Windows.Forms;

#nullable disable
namespace Elli.Web.Host.Login
{
  public class WebLoginUtil
  {
    private string _authCodeFromIDP = string.Empty;
    private string _userNameFromIDP = string.Empty;
    private string _passwordFromIDP = string.Empty;
    private string _instanceFromIDP = string.Empty;
    private AppName _appName;
    private string _server;
    private bool _donotLockServer;
    private string _url;
    private string serverInstance = string.Empty;
    private Func<LoginContext, bool> _loginRoutine;
    protected static string sw = Tracing.SwOutsideLoan;
    private const string ChromiumWebBrowserAttr = "UseChromiumForWebLogin";
    private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;

    public event WebLoginUtil.MFALoginFlowCompletedHandler OnMFALoginCompleted;

    [DllImport("wininet.dll", SetLastError = true)]
    private static extern bool InternetSetOption(
      IntPtr hInternet,
      int dwOption,
      IntPtr lpBuffer,
      int lpdwBufferLength);

    public WebLoginUtil()
    {
      if (EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders.ContainsKey("X-ClientType"))
        return;
      EnConfigurationSettings.GlobalSettings.HttpClientCustomHeaders["X-ClientType"] = "Encompass";
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

    public LoginThinThickForm GetLoginForm(
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
      LoginThinThickForm loginForm = new LoginThinThickForm(Title);
      loginForm.FormBorderStyle = FormBorderStyle.FixedSingle;
      string str1 = string.IsNullOrEmpty(appClientId) ? "n35xg3ze" : appClientId;
      string str2 = string.IsNullOrEmpty(appScope) ? "sc" : appScope;
      string str3 = string.IsNullOrEmpty(redirectUrl) ? "https://encompass.elliemae.com/homepage/atest.asp" : redirectUrl;
      string url = WebLoginUtil.IdpUrl + "/authorize?client_id=" + str1 + "&response_type=code&redirect_uri=" + str3 + "&scope=" + str2 + "&instance_id=" + instanceId + "&encompass_version=" + VersionInformation.CurrentVersion.DisplayVersion;
      loginForm.Navigate(url);
      loginForm.OnStartNavigation += new LoginThinThickForm.BeforeNavigation(this.WebPageForm_OnStartNavigation);
      this.OnMFALoginCompleted += new WebLoginUtil.MFALoginFlowCompletedHandler(this.LoginUtil_OnMFALoginCompleted);
      loginForm.OnBrowserLoadComplete += new LoginThinThickForm.BrowserLoadComplete(this.WebPageForm_OnBrowserLoadComplete);
      this._server = server;
      this._donotLockServer = donotLockServer;
      this._loginRoutine = loginRoutine;
      this._appName = appName;
      this._url = url;
      return loginForm;
    }

    private void WebPageForm_OnBrowserLoadComplete(object sender, NavigationCompletedEventArgs e)
    {
      if (sender != null && e.Url.ToString().Contains("instance_id"))
        this._instanceFromIDP = HttpUtility.ParseQueryString(e.Url.ToString().Substring(e.Url.ToString().IndexOf('?')).Split('#')[0])["instance_id"];
      if (sender != null && e.Url.ToString().Contains("code="))
      {
        this._authCodeFromIDP = HttpUtility.ParseQueryString(e.Url.ToString().Substring(e.Url.ToString().IndexOf('?')).Split('#')[0])["code"];
        if (string.IsNullOrWhiteSpace(this._authCodeFromIDP))
          return;
        Form parentForm = this.getParentForm((Control) sender);
        if (parentForm is LoginThinThickForm)
          ((LoginThinThickForm) parentForm).HideForm();
        if (this.OnMFALoginCompleted == null)
          return;
        this.OnMFALoginCompleted((object) parentForm, new LoginCompletedEventArgs()
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
        string url = this._url + "&" + e.Url.Remove(0, 1);
        WebLoginUtil.InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);
        ((EncWebFormBrowserControl) sender).Navigate(url);
        ((Control) sender).Refresh();
      }
    }

    private void LoginUtil_OnMFALoginCompleted(object sender, LoginCompletedEventArgs args)
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
          Tracing.Log(Tracing.SwCommon, TraceLevel.Error, nameof (WebLoginUtil), ex.StackTrace);
        }
        finally
        {
          form.Close();
        }
      }));
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

    public static bool IsTokenLoginOnly
    {
      get
      {
        return (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "TokenLoginOnly") != null ? (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "TokenLoginOnly") == "1" : AssemblyResolver.GetScAttribute("TokenLoginOnly") == "1";
      }
    }

    private DialogResult Login(LoginContext context)
    {
      if (!WebLoginUtil.IsTokenLoginOnly)
        throw new InvalidOperationException("This method should be called only when IsTokenLoginOnly is true");
      DialogResult dialogResult = DialogResult.None;
      if (this._loginRoutine(context))
        dialogResult = DialogResult.OK;
      return dialogResult;
    }

    private void WebPageForm_OnStartNavigation(object sender, StartLoadingEventArgs e)
    {
      WebHost webHost = sender as WebHost;
      string browserAttributeValue = webHost.GetBrowserAttributeValue("instanceId");
      if (!string.IsNullOrWhiteSpace(browserAttributeValue))
        this._instanceFromIDP = browserAttributeValue;
      if (webHost == null || !e.ValidatedURL.ToString().Contains("authorization.ping"))
        return;
      this._userNameFromIDP = webHost.GetBrowserAttributeValue("userId");
      this._passwordFromIDP = webHost.GetBrowserAttributeValue("password");
    }

    public static bool StopBrowserEngine(string appName)
    {
      bool flag = false;
      try
      {
        BrowserEngine.StopEngine();
        flag = true;
      }
      catch (Exception ex)
      {
        Tracing.Log(WebLoginUtil.sw, appName.ToString(), TraceLevel.Error, "Exception while stopping browser engine: " + ex.Message);
      }
      return flag;
    }

    public static bool IsChromiumForWebLoginEnabled
    {
      get
      {
        bool forWebLoginEnabled = true;
        if ((string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "UseChromiumForWebLogin") != null)
          forWebLoginEnabled = (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "UseChromiumForWebLogin") == "1";
        else if (!string.IsNullOrEmpty(AssemblyResolver.GetScAttribute("UseChromiumForWebLogin")))
          forWebLoginEnabled = AssemblyResolver.GetScAttribute("UseChromiumForWebLogin") == "1";
        return forWebLoginEnabled;
      }
    }

    public static bool IsEnableChromeDebugMode
    {
      get => EnConfigurationSettings.GlobalSettings.Debug || WebLoginUtil.IsAutomationDebugEnabled;
    }

    private static bool IsAutomationDebugEnabled
    {
      get
      {
        return (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "EnableChromeDebugForAutomation") != null && (string) RegistryUtil.GetRegistryValue(Registry.LocalMachine, "Software\\Ellie Mae\\Encompass", "EnableChromeDebugForAutomation") == "1";
      }
    }

    public delegate void MFALoginFlowCompletedHandler(object sender, LoginCompletedEventArgs args);
  }
}
