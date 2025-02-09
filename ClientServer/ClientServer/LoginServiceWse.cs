// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoginServiceWse
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Microsoft.Web.Services2;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "LoginServiceSoap", Namespace = "http://encompass.elliemae.com/")]
  public class LoginServiceWse : WebServicesClientProtocol
  {
    private string _ssoToken;

    public LoginServiceWse(string ssoToken = null)
    {
      this.Url = "https://encompass.elliemae.com/homepageWS/LoginService.asmx";
      this._ssoToken = ssoToken;
    }

    public LoginServiceWse(string loginServicesUrl, string ssoToken = null)
    {
      if (string.IsNullOrWhiteSpace(loginServicesUrl) || !Uri.IsWellFormedUriString(loginServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/homepageWS/LoginService.asmx";
      else
        this.Url = loginServicesUrl;
      this._ssoToken = ssoToken;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      WebRequest webRequest = base.GetWebRequest(uri);
      if (!string.IsNullOrWhiteSpace(this._ssoToken))
        webRequest.Headers.Add("Authorization", "EMAuth " + this._ssoToken);
      return webRequest;
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/CheckServer", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string CheckServer() => (string) this.Invoke(nameof (CheckServer), new object[0])[0];

    public IAsyncResult BeginCheckServer(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("CheckServer", new object[0], callback, asyncState);
    }

    public string EndCheckServer(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/AutherticateUser", RequestNamespace = "http://encompass.elliemae.com/", ResponseNamespace = "http://encompass.elliemae.com/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string AutherticateUser(string xmlLoginData)
    {
      return (string) this.Invoke(nameof (AutherticateUser), new object[1]
      {
        (object) xmlLoginData
      })[0];
    }

    public IAsyncResult BeginAutherticateUser(
      string xmlLoginData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AutherticateUser", new object[1]
      {
        (object) xmlLoginData
      }, callback, asyncState);
    }

    public string EndAutherticateUser(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }
  }
}
