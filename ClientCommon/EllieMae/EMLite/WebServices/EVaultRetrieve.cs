// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.EVaultRetrieve
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
  [GeneratedCode("wsdl", "4.0.30319.1")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "EVaultRetrieveSoap", Namespace = "http://loancenter.elliemae.com/eVaultRetrieveService//")]
  public class EVaultRetrieve : SoapHttpClientProtocol
  {
    private string _tokenType;
    private string _accessToken;
    private SendOrPostCallback GetSignedDocumentListOperationCompleted;
    private SendOrPostCallback GetSignedDocumentOperationCompleted;
    private SendOrPostCallback GetAuditDataOperationCompleted;

    public EVaultRetrieve(string tokenType, string accessToken)
    {
      this.Url = "https://loancenter.elliemae.com/EVaultRetrieveService/EVaultRetrieve.asmx";
      this._tokenType = tokenType;
      this._accessToken = accessToken;
    }

    public EVaultRetrieve(string tokenType, string accessToken, string eVaultRetrieveServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(eVaultRetrieveServiceUrl) || !Uri.IsWellFormedUriString(eVaultRetrieveServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/EVaultRetrieveService/EVaultRetrieve.asmx";
      else
        this.Url = eVaultRetrieveServiceUrl;
      this._tokenType = tokenType;
      this._accessToken = accessToken;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
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
      if (!string.IsNullOrEmpty(this._tokenType) && !string.IsNullOrEmpty(this._accessToken))
        webRequest.Headers.Add("AccessToken", this._tokenType + " " + this._accessToken);
      return (WebRequest) webRequest;
    }

    public EVaultRetrieve()
    {
      this.Url = "https://loancenter.elliemae.com/EVaultRetrieveService/EVaultRetrieve.asmx";
    }

    public EVaultRetrieve(string eVaultRetrieveServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(eVaultRetrieveServiceUrl) || !Uri.IsWellFormedUriString(eVaultRetrieveServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/EVaultRetrieveService/EVaultRetrieve.asmx";
      else
        this.Url = eVaultRetrieveServiceUrl;
    }

    public event GetSignedDocumentListCompletedEventHandler GetSignedDocumentListCompleted;

    public event GetSignedDocumentCompletedEventHandler GetSignedDocumentCompleted;

    public event GetAuditDataCompletedEventHandler GetAuditDataCompleted;

    [SoapDocumentMethod("http://loancenter.elliemae.com/eVaultRetrieveService//GetSignedDocumentList", RequestNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", ResponseNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public LoanCenterDocument[] GetSignedDocumentList(
      EVaultRetrieveCredentials clientCredentials,
      string clientID,
      string loanID)
    {
      return (LoanCenterDocument[]) this.Invoke(nameof (GetSignedDocumentList), new object[3]
      {
        (object) clientCredentials,
        (object) clientID,
        (object) loanID
      })[0];
    }

    public IAsyncResult BeginGetSignedDocumentList(
      EVaultRetrieveCredentials clientCredentials,
      string clientID,
      string loanID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetSignedDocumentList", new object[3]
      {
        (object) clientCredentials,
        (object) clientID,
        (object) loanID
      }, callback, asyncState);
    }

    public LoanCenterDocument[] EndGetSignedDocumentList(IAsyncResult asyncResult)
    {
      return (LoanCenterDocument[]) this.EndInvoke(asyncResult)[0];
    }

    public void GetSignedDocumentListAsync(
      EVaultRetrieveCredentials clientCredentials,
      string clientID,
      string loanID)
    {
      this.GetSignedDocumentListAsync(clientCredentials, clientID, loanID, (object) null);
    }

    public void GetSignedDocumentListAsync(
      EVaultRetrieveCredentials clientCredentials,
      string clientID,
      string loanID,
      object userState)
    {
      if (this.GetSignedDocumentListOperationCompleted == null)
        this.GetSignedDocumentListOperationCompleted = new SendOrPostCallback(this.OnGetSignedDocumentListOperationCompleted);
      this.InvokeAsync("GetSignedDocumentList", new object[3]
      {
        (object) clientCredentials,
        (object) clientID,
        (object) loanID
      }, this.GetSignedDocumentListOperationCompleted, userState);
    }

    private void OnGetSignedDocumentListOperationCompleted(object arg)
    {
      if (this.GetSignedDocumentListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSignedDocumentListCompleted((object) this, new GetSignedDocumentListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/eVaultRetrieveService//GetSignedDocument", RequestNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", ResponseNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public SignedDocument GetSignedDocument(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document)
    {
      return (SignedDocument) this.Invoke(nameof (GetSignedDocument), new object[2]
      {
        (object) clientCredentials,
        (object) document
      })[0];
    }

    public IAsyncResult BeginGetSignedDocument(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetSignedDocument", new object[2]
      {
        (object) clientCredentials,
        (object) document
      }, callback, asyncState);
    }

    public SignedDocument EndGetSignedDocument(IAsyncResult asyncResult)
    {
      return (SignedDocument) this.EndInvoke(asyncResult)[0];
    }

    public void GetSignedDocumentAsync(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document)
    {
      this.GetSignedDocumentAsync(clientCredentials, document, (object) null);
    }

    public void GetSignedDocumentAsync(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document,
      object userState)
    {
      if (this.GetSignedDocumentOperationCompleted == null)
        this.GetSignedDocumentOperationCompleted = new SendOrPostCallback(this.OnGetSignedDocumentOperationCompleted);
      this.InvokeAsync("GetSignedDocument", new object[2]
      {
        (object) clientCredentials,
        (object) document
      }, this.GetSignedDocumentOperationCompleted, userState);
    }

    private void OnGetSignedDocumentOperationCompleted(object arg)
    {
      if (this.GetSignedDocumentCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSignedDocumentCompleted((object) this, new GetSignedDocumentCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/eVaultRetrieveService//GetAuditData", RequestNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", ResponseNamespace = "http://loancenter.elliemae.com/eVaultRetrieveService//", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetAuditData(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document)
    {
      return (string) this.Invoke(nameof (GetAuditData), new object[2]
      {
        (object) clientCredentials,
        (object) document
      })[0];
    }

    public IAsyncResult BeginGetAuditData(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetAuditData", new object[2]
      {
        (object) clientCredentials,
        (object) document
      }, callback, asyncState);
    }

    public string EndGetAuditData(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetAuditDataAsync(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document)
    {
      this.GetAuditDataAsync(clientCredentials, document, (object) null);
    }

    public void GetAuditDataAsync(
      EVaultRetrieveCredentials clientCredentials,
      LoanCenterDocument document,
      object userState)
    {
      if (this.GetAuditDataOperationCompleted == null)
        this.GetAuditDataOperationCompleted = new SendOrPostCallback(this.OnGetAuditDataOperationCompleted);
      this.InvokeAsync("GetAuditData", new object[2]
      {
        (object) clientCredentials,
        (object) document
      }, this.GetAuditDataOperationCompleted, userState);
    }

    private void OnGetAuditDataOperationCompleted(object arg)
    {
      if (this.GetAuditDataCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetAuditDataCompleted((object) this, new GetAuditDataCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
