// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.JobService
// Assembly: ClientSession, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: B6063C59-FEBD-476F-AF5D-07F2CE35B702
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientSession.dll

using EllieMae.EMLite.Common;
using System;
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
  [WebServiceBinding(Name = "JobServiceSoap", Namespace = "http://encompass.elliemae.com/EncompassServices/")]
  public class JobService : SoapHttpClientProtocol
  {
    public JobService()
    {
      this.Url = string.Concat(EnConfigurationSettings.GlobalSettings["JobServiceURL"]);
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    [SoapDocumentMethod("http://encompass.elliemae.com/EncompassServices/SubmitJob", RequestNamespace = "http://encompass.elliemae.com/EncompassServices/", ResponseNamespace = "http://encompass.elliemae.com/EncompassServices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int SubmitJob(
      string clientId,
      string userId,
      string jobType,
      int jobWeight,
      string jobData)
    {
      return (int) this.Invoke(nameof (SubmitJob), new object[5]
      {
        (object) clientId,
        (object) userId,
        (object) jobType,
        (object) jobWeight,
        (object) jobData
      })[0];
    }

    public IAsyncResult BeginSubmitJob(
      string clientId,
      string userId,
      string jobType,
      int jobWeight,
      string jobData,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SubmitJob", new object[5]
      {
        (object) clientId,
        (object) userId,
        (object) jobType,
        (object) jobWeight,
        (object) jobData
      }, callback, asyncState);
    }

    public int EndSubmitJob(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];

    [SoapDocumentMethod("http://encompass.elliemae.com/EncompassServices/GetJobStatus", RequestNamespace = "http://encompass.elliemae.com/EncompassServices/", ResponseNamespace = "http://encompass.elliemae.com/EncompassServices/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public int GetJobStatus(int jobId)
    {
      return (int) this.Invoke(nameof (GetJobStatus), new object[1]
      {
        (object) jobId
      })[0];
    }

    public IAsyncResult BeginGetJobStatus(int jobId, AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("GetJobStatus", new object[1]
      {
        (object) jobId
      }, callback, asyncState);
    }

    public int EndGetJobStatus(IAsyncResult asyncResult) => (int) this.EndInvoke(asyncResult)[0];
  }
}
