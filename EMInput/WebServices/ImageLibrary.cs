// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ImageLibrary
// Assembly: EMInput, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: ED3FE5F8-B05D-4E0B-8366-E502FB568694
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMInput.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ImageLibrarySoap", Namespace = "http://resources.elliemae.com/ImageLibraryService")]
  public class ImageLibrary : SoapHttpClientProtocol
  {
    public ImageLibrary()
    {
      this.Url = "https://loancenter.elliemae.com/ImageLibraryService/ImageLibrary.asmx";
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
  }
}
