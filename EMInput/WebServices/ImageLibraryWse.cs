// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ImageLibraryWse
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

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
  [WebServiceBinding(Name = "ImageLibrarySoap", Namespace = "http://resources.elliemae.com/ImageLibraryService")]
  public class ImageLibraryWse : WebServicesClientProtocol
  {
    public ImageLibraryWse()
    {
      this.Url = "https://loancenter.elliemae.com/ImageLibraryService/ImageLibrary.asmx";
    }

    public ImageLibraryWse(string imageLibraryServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(imageLibraryServiceUrl) || !Uri.IsWellFormedUriString(imageLibraryServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/ImageLibraryService/ImageLibrary.asmx";
      else
        this.Url = imageLibraryServiceUrl;
    }

    public event ChunkHandler ChunkSent;

    private void webRequest_ChunkSent(object sender, ChunkHandlerEventArgs e)
    {
      if (this.ChunkSent == null)
        return;
      this.ChunkSent((object) this, e);
    }

    [SoapDocumentMethod("http://resources.elliemae.com/ImageLibraryService/GetImageList", RequestNamespace = "http://resources.elliemae.com/ImageLibraryService", ResponseNamespace = "http://resources.elliemae.com/ImageLibraryService", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetImageList(string xmlRequest)
    {
      return (string) this.Invoke(nameof (GetImageList), new object[1]
      {
        (object) xmlRequest
      })[0];
    }

    public IAsyncResult BeginGetImageList(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetImageList", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public string EndGetImageList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://resources.elliemae.com/ImageLibraryService/UploadImage", RequestNamespace = "http://resources.elliemae.com/ImageLibraryService", ResponseNamespace = "http://resources.elliemae.com/ImageLibraryService", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string UploadImage(string xmlRequest)
    {
      return (string) this.Invoke(nameof (UploadImage), new object[1]
      {
        (object) xmlRequest
      })[0];
    }

    public IAsyncResult BeginUploadImage(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadImage", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public string EndUploadImage(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    [SoapDocumentMethod("http://resources.elliemae.com/ImageLibraryService/DeleteImage", RequestNamespace = "http://resources.elliemae.com/ImageLibraryService", ResponseNamespace = "http://resources.elliemae.com/ImageLibraryService", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void DeleteImage(string xmlRequest)
    {
      this.Invoke(nameof (DeleteImage), new object[1]
      {
        (object) xmlRequest
      });
    }

    public IAsyncResult BeginDeleteImage(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DeleteImage", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public void EndDeleteImage(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    protected override WebRequest GetWebRequest(Uri uri)
    {
      SoapWebRequest webRequest = (SoapWebRequest) base.GetWebRequest(uri);
      FieldInfo field = webRequest.GetType().GetField("_request", BindingFlags.Instance | BindingFlags.NonPublic);
      CustomWebRequest customWebRequest = new CustomWebRequest(uri);
      customWebRequest.ChunkSent += new ChunkHandler(this.webRequest_ChunkSent);
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
      return (WebRequest) webRequest;
    }
  }
}
