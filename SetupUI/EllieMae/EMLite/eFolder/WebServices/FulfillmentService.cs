// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WebServices.FulfillmentService
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

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
namespace EllieMae.EMLite.eFolder.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "FulfillmentServiceSoap", Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class FulfillmentService : SoapHttpClientProtocol
  {
    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      int result = 5;
      int.TryParse(Session.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
      string str = (string) null;
      if (Session.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        str = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
        {
          "Elli.Edm"
        }, result);
      if (!string.IsNullOrWhiteSpace(str))
        webRequest.Headers.Add("Authorization", "EMAuth " + str);
      return (WebRequest) webRequest;
    }

    public FulfillmentService()
    {
      this.Url = "https://loancenter.elliemae.com/eFolder/fulfillment.asmx";
    }

    public FulfillmentService(string fulfillmentServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(fulfillmentServiceUrl) || !Uri.IsWellFormedUriString(fulfillmentServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/eFolder/fulfillment.asmx";
      else
        this.Url = fulfillmentServiceUrl;
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/GetServiceStatus", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetServiceStatus(string clientID)
    {
      return (string) this.Invoke(nameof (GetServiceStatus), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetServiceStatus(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetServiceStatus", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public string EndGetServiceStatus(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/CreateOrder", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void CreateOrder(
      string clientID,
      string userID,
      string password,
      string contactEmail,
      string contactName)
    {
      this.Invoke(nameof (CreateOrder), new object[5]
      {
        (object) clientID,
        (object) userID,
        (object) password,
        (object) contactEmail,
        (object) contactName
      });
    }

    public IAsyncResult BeginCreateOrder(
      string clientID,
      string userID,
      string password,
      string contactEmail,
      string contactName,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("CreateOrder", new object[5]
      {
        (object) clientID,
        (object) userID,
        (object) password,
        (object) contactEmail,
        (object) contactName
      }, callback, asyncState);
    }

    public void EndCreateOrder(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);
  }
}
