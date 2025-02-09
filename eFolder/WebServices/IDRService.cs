// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.IDRService
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "IDRServiceSoap", Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class IDRService : SoapHttpClientProtocol
  {
    public IDRCredentials IDRCredentialsValue;

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      int expirationInMinutes = 5;
      string str = (string) null;
      if (Session.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        str = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, expirationInMinutes);
      if (!string.IsNullOrWhiteSpace(str))
        webRequest.Headers.Add("Authorization", "EMAuth " + str);
      return (WebRequest) webRequest;
    }

    public IDRService() => this.Url = "https://loancenter.elliemae.com/efolder/idr.asmx";

    [SoapHeader("IDRCredentialsValue")]
    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/IdentifyForm", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public FormIdentity IdentifyForm()
    {
      return (FormIdentity) this.Invoke(nameof (IdentifyForm), new object[0])[0];
    }

    public IAsyncResult BeginIdentifyForm(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("IdentifyForm", new object[0], callback, asyncState);
    }

    public FormIdentity EndIdentifyForm(IAsyncResult asyncResult)
    {
      return (FormIdentity) this.EndInvoke(asyncResult)[0];
    }

    [SoapHeader("IDRCredentialsValue")]
    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/IdentifyForms", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public FormIdentity[] IdentifyForms(int matchLimit)
    {
      return (FormIdentity[]) this.Invoke(nameof (IdentifyForms), new object[1]
      {
        (object) matchLimit
      })[0];
    }

    public IAsyncResult BeginIdentifyForms(
      int matchLimit,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("IdentifyForms", new object[1]
      {
        (object) matchLimit
      }, callback, asyncState);
    }

    public FormIdentity[] EndIdentifyForms(IAsyncResult asyncResult)
    {
      return (FormIdentity[]) this.EndInvoke(asyncResult)[0];
    }
  }
}
