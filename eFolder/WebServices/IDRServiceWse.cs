// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.IDRServiceWse
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Web.Services2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "IDRServiceSoap", Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class IDRServiceWse : WebServicesClientProtocol
  {
    public IDRCredentials IDRCredentialsValue;

    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      FieldInfo field = webRequest.GetType().GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
      CustomWebRequest customWebRequest = new CustomWebRequest(uri);
      customWebRequest.ChunkSent += new ChunkHandler(this.webRequest_ChunkSent);
      field.SetValue((object) webRequest, (object) customWebRequest);
      if (Session.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
      {
        int result = 5;
        int.TryParse(Session.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
        string ssoToken = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
        if (!string.IsNullOrWhiteSpace(ssoToken))
          webRequest.Headers.Add("Authorization", "EMAuth " + ssoToken);
      }
      return (WebRequest) webRequest;
    }

    public IDRServiceWse() => this.Url = "https://loancenter.elliemae.com/efolder/idr.asmx";

    public IDRServiceWse(string idrServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(idrServiceUrl) || !Uri.IsWellFormedUriString(idrServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/efolder/idr.asmx";
      else
        this.Url = idrServiceUrl;
    }

    public event ChunkHandler ChunkSent;

    private void webRequest_ChunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkSent == null)
        return;
      this.ChunkSent((object) this, e);
    }

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
