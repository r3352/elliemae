// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoginService
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "LoginServiceSoap", Namespace = "http://encompass.elliemae.com/")]
  public class LoginService : SoapHttpClientProtocol
  {
    public LoginService()
    {
      this.Url = "https://encompass.elliemae.com/homepageWS/LoginService.asmx";
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
