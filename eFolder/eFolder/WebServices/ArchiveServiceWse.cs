// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WebServices.ArchiveServiceWse
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
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.eFolder.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ArchiveServiceSoap", Namespace = "https://archive.elliemae.com/ArchiveWS")]
  public class ArchiveServiceWse : WebServicesClientProtocol
  {
    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      ((HttpWebRequest) webRequest.Request).KeepAlive = false;
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

    public ArchiveServiceWse()
    {
      this.Url = "https://archive.elliemae.com/ArchiveWS/ArchiveService.asmx";
    }

    [SoapDocumentMethod("https://archive.elliemae.com/ArchiveWS/RegisterLoan", RequestNamespace = "https://archive.elliemae.com/ArchiveWS", ResponseNamespace = "https://archive.elliemae.com/ArchiveWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int RegisterLoan(string clientID, string loanDataXml)
    {
      return (int) this.Invoke(nameof (RegisterLoan), new object[2]
      {
        (object) clientID,
        (object) loanDataXml
      })[0];
    }

    public IAsyncResult BeginRegisterLoan(
      string clientID,
      string loanDataXml,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RegisterLoan", new object[2]
      {
        (object) clientID,
        (object) loanDataXml
      }, callback, asyncState);
    }

    public int EndRegisterLoan(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    [SoapDocumentMethod("https://archive.elliemae.com/ArchiveWS/UploadDocument", RequestNamespace = "https://archive.elliemae.com/ArchiveWS", ResponseNamespace = "https://archive.elliemae.com/ArchiveWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int UploadDocument(int loanID, string docInfoXml, int numPages, [XmlElement(DataType = "base64Binary")] byte[] docData)
    {
      return (int) this.Invoke(nameof (UploadDocument), new object[4]
      {
        (object) loanID,
        (object) docInfoXml,
        (object) numPages,
        (object) docData
      })[0];
    }

    public IAsyncResult BeginUploadDocument(
      int loanID,
      string docInfoXml,
      int numPages,
      byte[] docData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadDocument", new object[4]
      {
        (object) loanID,
        (object) docInfoXml,
        (object) numPages,
        (object) docData
      }, callback, asyncState);
    }

    public int EndUploadDocument(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    [SoapDocumentMethod("https://archive.elliemae.com/ArchiveWS/UploadDocument2", RequestNamespace = "https://archive.elliemae.com/ArchiveWS", ResponseNamespace = "https://archive.elliemae.com/ArchiveWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int UploadDocument2(int loanID, string docInfoXml, int numPages)
    {
      return (int) this.Invoke(nameof (UploadDocument2), new object[3]
      {
        (object) loanID,
        (object) docInfoXml,
        (object) numPages
      })[0];
    }

    public IAsyncResult BeginUploadDocument2(
      int loanID,
      string docInfoXml,
      int numPages,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadDocument2", new object[3]
      {
        (object) loanID,
        (object) docInfoXml,
        (object) numPages
      }, callback, asyncState);
    }

    public int EndUploadDocument2(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];
  }
}
