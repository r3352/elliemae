// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PluginService
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

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
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "PluginServiceSoap", Namespace = "http://encompass.elliemae.com/jedservices/")]
  public class PluginService : SoapHttpClientProtocol
  {
    private SendOrPostCallback AuthorizeFormOperationCompleted;
    private SendOrPostCallback AuthorizePluginOperationCompleted;

    public PluginService() => this.Url = "https://encompass.elliemae.com/jedservices/plugins.asmx";

    public PluginService(string jedServicesUrl)
    {
      if (string.IsNullOrWhiteSpace(jedServicesUrl) || !Uri.IsWellFormedUriString(jedServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/jedservices/plugins.asmx";
      else
        this.Url = jedServicesUrl + "plugins.asmx";
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    public event AuthorizeFormCompletedEventHandler AuthorizeFormCompleted;

    public event AuthorizePluginCompletedEventHandler AuthorizePluginCompleted;

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/AuthorizeForm", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string AuthorizeForm(
      string clientID,
      string hostname,
      string winUserName,
      string formName,
      string formCRC,
      string assemblyName,
      string className,
      string assemblyCrc,
      string passPhrase)
    {
      return (string) this.Invoke(nameof (AuthorizeForm), new object[9]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) formName,
        (object) formCRC,
        (object) assemblyName,
        (object) className,
        (object) assemblyCrc,
        (object) passPhrase
      })[0];
    }

    public IAsyncResult BeginAuthorizeForm(
      string clientID,
      string hostname,
      string winUserName,
      string formName,
      string formCRC,
      string assemblyName,
      string className,
      string assemblyCrc,
      string passPhrase,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AuthorizeForm", new object[9]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) formName,
        (object) formCRC,
        (object) assemblyName,
        (object) className,
        (object) assemblyCrc,
        (object) passPhrase
      }, callback, asyncState);
    }

    public string EndAuthorizeForm(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void AuthorizeFormAsync(
      string clientID,
      string hostname,
      string winUserName,
      string formName,
      string formCRC,
      string assemblyName,
      string className,
      string assemblyCrc,
      string passPhrase)
    {
      this.AuthorizeFormAsync(clientID, hostname, winUserName, formName, formCRC, assemblyName, className, assemblyCrc, passPhrase, (object) null);
    }

    public void AuthorizeFormAsync(
      string clientID,
      string hostname,
      string winUserName,
      string formName,
      string formCRC,
      string assemblyName,
      string className,
      string assemblyCrc,
      string passPhrase,
      object userState)
    {
      if (this.AuthorizeFormOperationCompleted == null)
        this.AuthorizeFormOperationCompleted = new SendOrPostCallback(this.OnAuthorizeFormOperationCompleted);
      this.InvokeAsync("AuthorizeForm", new object[9]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) formName,
        (object) formCRC,
        (object) assemblyName,
        (object) className,
        (object) assemblyCrc,
        (object) passPhrase
      }, this.AuthorizeFormOperationCompleted, userState);
    }

    private void OnAuthorizeFormOperationCompleted(object arg)
    {
      if (this.AuthorizeFormCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AuthorizeFormCompleted((object) this, new AuthorizeFormCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/jedservices/AuthorizePlugin", RequestNamespace = "http://encompass.elliemae.com/jedservices/", ResponseNamespace = "http://encompass.elliemae.com/jedservices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string AuthorizePlugin(
      string clientID,
      string hostname,
      string winUserName,
      string assemblyName,
      string className,
      string assemblyCRC,
      string passPhrase)
    {
      return (string) this.Invoke(nameof (AuthorizePlugin), new object[7]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) assemblyName,
        (object) className,
        (object) assemblyCRC,
        (object) passPhrase
      })[0];
    }

    public IAsyncResult BeginAuthorizePlugin(
      string clientID,
      string hostname,
      string winUserName,
      string assemblyName,
      string className,
      string assemblyCRC,
      string passPhrase,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AuthorizePlugin", new object[7]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) assemblyName,
        (object) className,
        (object) assemblyCRC,
        (object) passPhrase
      }, callback, asyncState);
    }

    public string EndAuthorizePlugin(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void AuthorizePluginAsync(
      string clientID,
      string hostname,
      string winUserName,
      string assemblyName,
      string className,
      string assemblyCRC,
      string passPhrase)
    {
      this.AuthorizePluginAsync(clientID, hostname, winUserName, assemblyName, className, assemblyCRC, passPhrase, (object) null);
    }

    public void AuthorizePluginAsync(
      string clientID,
      string hostname,
      string winUserName,
      string assemblyName,
      string className,
      string assemblyCRC,
      string passPhrase,
      object userState)
    {
      if (this.AuthorizePluginOperationCompleted == null)
        this.AuthorizePluginOperationCompleted = new SendOrPostCallback(this.OnAuthorizePluginOperationCompleted);
      this.InvokeAsync("AuthorizePlugin", new object[7]
      {
        (object) clientID,
        (object) hostname,
        (object) winUserName,
        (object) assemblyName,
        (object) className,
        (object) assemblyCRC,
        (object) passPhrase
      }, this.AuthorizePluginOperationCompleted, userState);
    }

    private void OnAuthorizePluginOperationCompleted(object arg)
    {
      if (this.AuthorizePluginCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AuthorizePluginCompleted((object) this, new AuthorizePluginCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
