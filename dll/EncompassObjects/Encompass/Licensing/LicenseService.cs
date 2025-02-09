// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Licensing.LicenseService
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.Encompass.Licensing
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ApiServiceSoap", Namespace = "http://encompass.elliemae.com/jedservices/")]
  internal sealed class LicenseService : SoapHttpClientProtocol
  {
    private SendOrPostCallback RegisterInstallOperationCompleted;
    private SendOrPostCallback RegisterInstall2OperationCompleted;
    private SendOrPostCallback GetUpdateOperationCompleted;
    private SendOrPostCallback ValidateUpdateOperationCompleted;
    private SendOrPostCallback ValidateExportOperationCompleted;
    private SendOrPostCallback AuthorizeSessionOperationCompleted;

    public LicenseService(string JedServicesUrl)
    {
      if (string.IsNullOrWhiteSpace(JedServicesUrl) || !Uri.IsWellFormedUriString(JedServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/jedservices/api.asmx";
      else
        this.Url = JedServicesUrl + "api.asmx";
    }

    public LicenseService() => this.Url = "https://encompass.elliemae.com/jedservices/api.asmx";

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.Proxy.Credentials = CredentialCache.DefaultCredentials;
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    public event RegisterInstallCompletedEventHandler RegisterInstallCompleted;

    public event RegisterInstall2CompletedEventHandler RegisterInstall2Completed;

    public event GetUpdateCompletedEventHandler GetUpdateCompleted;

    public event ValidateUpdateCompletedEventHandler ValidateUpdateCompleted;

    public event ValidateExportCompletedEventHandler ValidateExportCompleted;

    public event AuthorizeSessionCompletedEventHandler AuthorizeSessionCompleted;

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/RegisterInstall", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string RegisterInstall(string cdkey, string hostname, string[] macAddresses)
    {
      return (string) this.Invoke(nameof (RegisterInstall), new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      })[0];
    }

    public IAsyncResult BeginRegisterInstall(
      string cdkey,
      string hostname,
      string[] macAddresses,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RegisterInstall", new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      }, callback, asyncState);
    }

    public string EndRegisterInstall(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void RegisterInstallAsync(string cdkey, string hostname, string[] macAddresses)
    {
      this.RegisterInstallAsync(cdkey, hostname, macAddresses, (object) null);
    }

    public void RegisterInstallAsync(
      string cdkey,
      string hostname,
      string[] macAddresses,
      object userState)
    {
      if (this.RegisterInstallOperationCompleted == null)
        this.RegisterInstallOperationCompleted = new SendOrPostCallback(this.OnRegisterInstallOperationCompleted);
      this.InvokeAsync("RegisterInstall", new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      }, this.RegisterInstallOperationCompleted, userState);
    }

    private void OnRegisterInstallOperationCompleted(object arg)
    {
      if (this.RegisterInstallCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.RegisterInstallCompleted((object) this, new RegisterInstallCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/RegisterInstall2", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ApiRegistrationInfo RegisterInstall2(
      string cdkey,
      string hostname,
      string[] macAddresses)
    {
      return (ApiRegistrationInfo) this.Invoke(nameof (RegisterInstall2), new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      })[0];
    }

    public IAsyncResult BeginRegisterInstall2(
      string cdkey,
      string hostname,
      string[] macAddresses,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RegisterInstall2", new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      }, callback, asyncState);
    }

    public ApiRegistrationInfo EndRegisterInstall2(IAsyncResult asyncResult)
    {
      return (ApiRegistrationInfo) this.EndInvoke(asyncResult)[0];
    }

    public void RegisterInstall2Async(string cdkey, string hostname, string[] macAddresses)
    {
      this.RegisterInstall2Async(cdkey, hostname, macAddresses, (object) null);
    }

    public void RegisterInstall2Async(
      string cdkey,
      string hostname,
      string[] macAddresses,
      object userState)
    {
      if (this.RegisterInstall2OperationCompleted == null)
        this.RegisterInstall2OperationCompleted = new SendOrPostCallback(this.OnRegisterInstall2OperationCompleted);
      this.InvokeAsync("RegisterInstall2", new object[3]
      {
        (object) cdkey,
        (object) hostname,
        (object) macAddresses
      }, this.RegisterInstall2OperationCompleted, userState);
    }

    private void OnRegisterInstall2OperationCompleted(object arg)
    {
      if (this.RegisterInstall2Completed == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.RegisterInstall2Completed((object) this, new RegisterInstall2CompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/GetUpdate", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetUpdate(string clientID, string targetVersion, string sourceVersion)
    {
      return (string) this.Invoke(nameof (GetUpdate), new object[3]
      {
        (object) clientID,
        (object) targetVersion,
        (object) sourceVersion
      })[0];
    }

    public IAsyncResult BeginGetUpdate(
      string clientID,
      string targetVersion,
      string sourceVersion,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetUpdate", new object[3]
      {
        (object) clientID,
        (object) targetVersion,
        (object) sourceVersion
      }, callback, asyncState);
    }

    public string EndGetUpdate(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    public void GetUpdateAsync(string clientID, string targetVersion, string sourceVersion)
    {
      this.GetUpdateAsync(clientID, targetVersion, sourceVersion, (object) null);
    }

    public void GetUpdateAsync(
      string clientID,
      string targetVersion,
      string sourceVersion,
      object userState)
    {
      if (this.GetUpdateOperationCompleted == null)
        this.GetUpdateOperationCompleted = new SendOrPostCallback(this.OnGetUpdateOperationCompleted);
      this.InvokeAsync("GetUpdate", new object[3]
      {
        (object) clientID,
        (object) targetVersion,
        (object) sourceVersion
      }, this.GetUpdateOperationCompleted, userState);
    }

    private void OnGetUpdateOperationCompleted(object arg)
    {
      if (this.GetUpdateCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetUpdateCompleted((object) this, new GetUpdateCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/ValidateUpdate", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void ValidateUpdate(string clientID, string updateVersion, string updateSource)
    {
      this.Invoke(nameof (ValidateUpdate), new object[3]
      {
        (object) clientID,
        (object) updateVersion,
        (object) updateSource
      });
    }

    public IAsyncResult BeginValidateUpdate(
      string clientID,
      string updateVersion,
      string updateSource,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateUpdate", new object[3]
      {
        (object) clientID,
        (object) updateVersion,
        (object) updateSource
      }, callback, asyncState);
    }

    public void EndValidateUpdate(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void ValidateUpdateAsync(string clientID, string updateVersion, string updateSource)
    {
      this.ValidateUpdateAsync(clientID, updateVersion, updateSource, (object) null);
    }

    public void ValidateUpdateAsync(
      string clientID,
      string updateVersion,
      string updateSource,
      object userState)
    {
      if (this.ValidateUpdateOperationCompleted == null)
        this.ValidateUpdateOperationCompleted = new SendOrPostCallback(this.OnValidateUpdateOperationCompleted);
      this.InvokeAsync("ValidateUpdate", new object[3]
      {
        (object) clientID,
        (object) updateVersion,
        (object) updateSource
      }, this.ValidateUpdateOperationCompleted, userState);
    }

    private void OnValidateUpdateOperationCompleted(object arg)
    {
      if (this.ValidateUpdateCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateUpdateCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/ValidateExport", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void ValidateExport(
      string cdkey,
      string clientID,
      string loanGuid,
      string exportFormat,
      string hostname,
      string appname,
      string server,
      string userID)
    {
      this.Invoke(nameof (ValidateExport), new object[8]
      {
        (object) cdkey,
        (object) clientID,
        (object) loanGuid,
        (object) exportFormat,
        (object) hostname,
        (object) appname,
        (object) server,
        (object) userID
      });
    }

    public IAsyncResult BeginValidateExport(
      string cdkey,
      string clientID,
      string loanGuid,
      string exportFormat,
      string hostname,
      string appname,
      string server,
      string userID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateExport", new object[8]
      {
        (object) cdkey,
        (object) clientID,
        (object) loanGuid,
        (object) exportFormat,
        (object) hostname,
        (object) appname,
        (object) server,
        (object) userID
      }, callback, asyncState);
    }

    public void EndValidateExport(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void ValidateExportAsync(
      string cdkey,
      string clientID,
      string loanGuid,
      string exportFormat,
      string hostname,
      string appname,
      string server,
      string userID)
    {
      this.ValidateExportAsync(cdkey, clientID, loanGuid, exportFormat, hostname, appname, server, userID, (object) null);
    }

    public void ValidateExportAsync(
      string cdkey,
      string clientID,
      string loanGuid,
      string exportFormat,
      string hostname,
      string appname,
      string server,
      string userID,
      object userState)
    {
      if (this.ValidateExportOperationCompleted == null)
        this.ValidateExportOperationCompleted = new SendOrPostCallback(this.OnValidateExportOperationCompleted);
      this.InvokeAsync("ValidateExport", new object[8]
      {
        (object) cdkey,
        (object) clientID,
        (object) loanGuid,
        (object) exportFormat,
        (object) hostname,
        (object) appname,
        (object) server,
        (object) userID
      }, this.ValidateExportOperationCompleted, userState);
    }

    private void OnValidateExportOperationCompleted(object arg)
    {
      if (this.ValidateExportCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateExportCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/AuthorizeSession", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string AuthorizeSession(
      string clientID,
      string cdkey,
      string hostname,
      string winUserName,
      string appPath,
      string appCRC,
      string passPhrase)
    {
      return (string) this.Invoke(nameof (AuthorizeSession), new object[7]
      {
        (object) clientID,
        (object) cdkey,
        (object) hostname,
        (object) winUserName,
        (object) appPath,
        (object) appCRC,
        (object) passPhrase
      })[0];
    }

    public IAsyncResult BeginAuthorizeSession(
      string clientID,
      string cdkey,
      string hostname,
      string winUserName,
      string appPath,
      string appCRC,
      string passPhrase,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AuthorizeSession", new object[7]
      {
        (object) clientID,
        (object) cdkey,
        (object) hostname,
        (object) winUserName,
        (object) appPath,
        (object) appCRC,
        (object) passPhrase
      }, callback, asyncState);
    }

    public string EndAuthorizeSession(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void AuthorizeSessionAsync(
      string clientID,
      string cdkey,
      string hostname,
      string winUserName,
      string appPath,
      string appCRC,
      string passPhrase)
    {
      this.AuthorizeSessionAsync(clientID, cdkey, hostname, winUserName, appPath, appCRC, passPhrase, (object) null);
    }

    public void AuthorizeSessionAsync(
      string clientID,
      string cdkey,
      string hostname,
      string winUserName,
      string appPath,
      string appCRC,
      string passPhrase,
      object userState)
    {
      if (this.AuthorizeSessionOperationCompleted == null)
        this.AuthorizeSessionOperationCompleted = new SendOrPostCallback(this.OnAuthorizeSessionOperationCompleted);
      this.InvokeAsync("AuthorizeSession", new object[7]
      {
        (object) clientID,
        (object) cdkey,
        (object) hostname,
        (object) winUserName,
        (object) appPath,
        (object) appCRC,
        (object) passPhrase
      }, this.AuthorizeSessionOperationCompleted, userState);
    }

    private void OnAuthorizeSessionOperationCompleted(object arg)
    {
      if (this.AuthorizeSessionCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AuthorizeSessionCompleted((object) this, new AuthorizeSessionCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
