// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.InboxServiceWse
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
  [WebServiceBinding(Name = "InboxServiceSoap", Namespace = "http://loancenter.elliemae.com/eFolder")]
  public class InboxServiceWse : WebServicesClientProtocol
  {
    private string _tokenType;
    private string _accessToken;
    public InboxCredentials InboxCredentialsValue;

    public InboxServiceWse(string tokenType, string accessToken)
    {
      this.Url = "https://loancenter.elliemae.com/efolder/inbox.asmx";
      this._tokenType = tokenType;
      this._accessToken = accessToken;
    }

    public InboxServiceWse(string tokenType, string accessToken, string inboxServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(inboxServiceUrl) || !Uri.IsWellFormedUriString(inboxServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/efolder/inbox.asmx";
      else
        this.Url = inboxServiceUrl;
      this._tokenType = tokenType;
      this._accessToken = accessToken;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      FieldInfo field = webRequest.GetType().GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
      CustomWebRequest customWebRequest = new CustomWebRequest(uri);
      customWebRequest.ChunkReceived += new ChunkHandler(this.webRequest_ChunkReceived);
      field.SetValue((object) webRequest, (object) customWebRequest);
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
      if (!string.IsNullOrEmpty(this._tokenType) && !string.IsNullOrEmpty(this._accessToken))
        webRequest.Headers.Add("AccessToken", this._tokenType + " " + this._accessToken);
      return (WebRequest) webRequest;
    }

    public InboxServiceWse() => this.Url = "https://loancenter.elliemae.com/efolder/inbox.asmx";

    public InboxServiceWse(string inboxServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(inboxServiceUrl) || !Uri.IsWellFormedUriString(inboxServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/efolder/inbox.asmx";
      else
        this.Url = inboxServiceUrl;
    }

    public event ChunkHandler ChunkReceived;

    private void webRequest_ChunkReceived(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkReceived == null)
        return;
      this.ChunkReceived((object) this, e);
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/GetFiles", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public InboxFile[] GetFiles(string clientID, string loanID)
    {
      return (InboxFile[]) this.Invoke(nameof (GetFiles), new object[2]
      {
        (object) clientID,
        (object) loanID
      })[0];
    }

    public IAsyncResult BeginGetFiles(
      string clientID,
      string loanID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetFiles", new object[2]
      {
        (object) clientID,
        (object) loanID
      }, callback, asyncState);
    }

    public InboxFile[] EndGetFiles(IAsyncResult asyncResult)
    {
      return (InboxFile[]) this.EndInvoke(asyncResult)[0];
    }

    [SoapHeader("InboxCredentialsValue")]
    [SoapDocumentMethod("http://loancenter.elliemae.com/eFolder/DownloadFile", RequestNamespace = "http://loancenter.elliemae.com/eFolder", ResponseNamespace = "http://loancenter.elliemae.com/eFolder", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public PageIdentity[] DownloadFile(InboxFile file)
    {
      return (PageIdentity[]) this.Invoke(nameof (DownloadFile), new object[1]
      {
        (object) file
      })[0];
    }

    public IAsyncResult BeginDownloadFile(
      InboxFile file,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DownloadFile", new object[1]
      {
        (object) file
      }, callback, asyncState);
    }

    public PageIdentity[] EndDownloadFile(IAsyncResult asyncResult)
    {
      return (PageIdentity[]) this.EndInvoke(asyncResult)[0];
    }
  }
}
