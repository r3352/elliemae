// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.WebServices.StatusOnlineProxy
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.ContactUI.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ePackageSoap", Namespace = "http://loancenter.elliemae.com/ePackageWS/")]
  public class StatusOnlineProxy : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetSecurityFieldListOperationCompleted;
    private SendOrPostCallback RegisterRequestOperationCompleted;
    private SendOrPostCallback RegisterStatusOnlineRequestOperationCompleted;
    private SendOrPostCallback UploadStatusOperationCompleted;
    private SendOrPostCallback SendLoanStatusEmailOperationCompleted;
    private SendOrPostCallback GetLoanStatusPreviewOperationCompleted;
    private SendOrPostCallback UploadAttachmentOperationCompleted;
    private SendOrPostCallback ProcessRequestOperationCompleted;
    private SendOrPostCallback GetDisclosureTrackingDetailsOperationCompleted;

    public StatusOnlineProxy()
    {
      this.Url = "https://loancenter.elliemae.com/ePackageWS/ePackage.asmx";
    }

    public event GetSecurityFieldListCompletedEventHandler GetSecurityFieldListCompleted;

    public event RegisterRequestCompletedEventHandler RegisterRequestCompleted;

    public event RegisterStatusOnlineRequestCompletedEventHandler RegisterStatusOnlineRequestCompleted;

    public event UploadStatusCompletedEventHandler UploadStatusCompleted;

    public event SendLoanStatusEmailCompletedEventHandler SendLoanStatusEmailCompleted;

    public event GetLoanStatusPreviewCompletedEventHandler GetLoanStatusPreviewCompleted;

    public event UploadAttachmentCompletedEventHandler UploadAttachmentCompleted;

    public event ProcessRequestCompletedEventHandler ProcessRequestCompleted;

    public event GetDisclosureTrackingDetailsCompletedEventHandler GetDisclosureTrackingDetailsCompleted;

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/GetSecurityFieldList", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetSecurityFieldList()
    {
      return (string) this.Invoke(nameof (GetSecurityFieldList), new object[0])[0];
    }

    public IAsyncResult BeginGetSecurityFieldList(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("GetSecurityFieldList", new object[0], callback, asyncState);
    }

    public string EndGetSecurityFieldList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetSecurityFieldListAsync() => this.GetSecurityFieldListAsync((object) null);

    public void GetSecurityFieldListAsync(object userState)
    {
      if (this.GetSecurityFieldListOperationCompleted == null)
        this.GetSecurityFieldListOperationCompleted = new SendOrPostCallback(this.OnGetSecurityFieldListOperationCompleted);
      this.InvokeAsync("GetSecurityFieldList", new object[0], this.GetSecurityFieldListOperationCompleted, userState);
    }

    private void OnGetSecurityFieldListOperationCompleted(object arg)
    {
      if (this.GetSecurityFieldListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetSecurityFieldListCompleted((object) this, new GetSecurityFieldListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/RegisterRequest", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string RegisterRequest(string xmlRequest)
    {
      return (string) this.Invoke(nameof (RegisterRequest), new object[1]
      {
        (object) xmlRequest
      })[0];
    }

    public IAsyncResult BeginRegisterRequest(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RegisterRequest", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public string EndRegisterRequest(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void RegisterRequestAsync(string xmlRequest)
    {
      this.RegisterRequestAsync(xmlRequest, (object) null);
    }

    public void RegisterRequestAsync(string xmlRequest, object userState)
    {
      if (this.RegisterRequestOperationCompleted == null)
        this.RegisterRequestOperationCompleted = new SendOrPostCallback(this.OnRegisterRequestOperationCompleted);
      this.InvokeAsync("RegisterRequest", new object[1]
      {
        (object) xmlRequest
      }, this.RegisterRequestOperationCompleted, userState);
    }

    private void OnRegisterRequestOperationCompleted(object arg)
    {
      if (this.RegisterRequestCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.RegisterRequestCompleted((object) this, new RegisterRequestCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/RegisterStatusOnlineRequest", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string RegisterStatusOnlineRequest(string xmlRequest)
    {
      return (string) this.Invoke(nameof (RegisterStatusOnlineRequest), new object[1]
      {
        (object) xmlRequest
      })[0];
    }

    public IAsyncResult BeginRegisterStatusOnlineRequest(
      string xmlRequest,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("RegisterStatusOnlineRequest", new object[1]
      {
        (object) xmlRequest
      }, callback, asyncState);
    }

    public string EndRegisterStatusOnlineRequest(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void RegisterStatusOnlineRequestAsync(string xmlRequest)
    {
      this.RegisterStatusOnlineRequestAsync(xmlRequest, (object) null);
    }

    public void RegisterStatusOnlineRequestAsync(string xmlRequest, object userState)
    {
      if (this.RegisterStatusOnlineRequestOperationCompleted == null)
        this.RegisterStatusOnlineRequestOperationCompleted = new SendOrPostCallback(this.OnRegisterStatusOnlineRequestOperationCompleted);
      this.InvokeAsync("RegisterStatusOnlineRequest", new object[1]
      {
        (object) xmlRequest
      }, this.RegisterStatusOnlineRequestOperationCompleted, userState);
    }

    private void OnRegisterStatusOnlineRequestOperationCompleted(object arg)
    {
      if (this.RegisterStatusOnlineRequestCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.RegisterStatusOnlineRequestCompleted((object) this, new RegisterStatusOnlineRequestCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/UploadStatus", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UploadStatus(string serverLoanGUID, string statusXML)
    {
      this.Invoke(nameof (UploadStatus), new object[2]
      {
        (object) serverLoanGUID,
        (object) statusXML
      });
    }

    public IAsyncResult BeginUploadStatus(
      string serverLoanGUID,
      string statusXML,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadStatus", new object[2]
      {
        (object) serverLoanGUID,
        (object) statusXML
      }, callback, asyncState);
    }

    public void EndUploadStatus(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void UploadStatusAsync(string serverLoanGUID, string statusXML)
    {
      this.UploadStatusAsync(serverLoanGUID, statusXML, (object) null);
    }

    public void UploadStatusAsync(string serverLoanGUID, string statusXML, object userState)
    {
      if (this.UploadStatusOperationCompleted == null)
        this.UploadStatusOperationCompleted = new SendOrPostCallback(this.OnUploadStatusOperationCompleted);
      this.InvokeAsync("UploadStatus", new object[2]
      {
        (object) serverLoanGUID,
        (object) statusXML
      }, this.UploadStatusOperationCompleted, userState);
    }

    private void OnUploadStatusOperationCompleted(object arg)
    {
      if (this.UploadStatusCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadStatusCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/SendLoanStatusEmail", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void SendLoanStatusEmail(
      string serverLoanGUID,
      string from,
      string to,
      string cc,
      string subject,
      string body)
    {
      this.Invoke(nameof (SendLoanStatusEmail), new object[6]
      {
        (object) serverLoanGUID,
        (object) from,
        (object) to,
        (object) cc,
        (object) subject,
        (object) body
      });
    }

    public IAsyncResult BeginSendLoanStatusEmail(
      string serverLoanGUID,
      string from,
      string to,
      string cc,
      string subject,
      string body,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SendLoanStatusEmail", new object[6]
      {
        (object) serverLoanGUID,
        (object) from,
        (object) to,
        (object) cc,
        (object) subject,
        (object) body
      }, callback, asyncState);
    }

    public void EndSendLoanStatusEmail(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void SendLoanStatusEmailAsync(
      string serverLoanGUID,
      string from,
      string to,
      string cc,
      string subject,
      string body)
    {
      this.SendLoanStatusEmailAsync(serverLoanGUID, from, to, cc, subject, body, (object) null);
    }

    public void SendLoanStatusEmailAsync(
      string serverLoanGUID,
      string from,
      string to,
      string cc,
      string subject,
      string body,
      object userState)
    {
      if (this.SendLoanStatusEmailOperationCompleted == null)
        this.SendLoanStatusEmailOperationCompleted = new SendOrPostCallback(this.OnSendLoanStatusEmailOperationCompleted);
      this.InvokeAsync("SendLoanStatusEmail", new object[6]
      {
        (object) serverLoanGUID,
        (object) from,
        (object) to,
        (object) cc,
        (object) subject,
        (object) body
      }, this.SendLoanStatusEmailOperationCompleted, userState);
    }

    private void OnSendLoanStatusEmailOperationCompleted(object arg)
    {
      if (this.SendLoanStatusEmailCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SendLoanStatusEmailCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/GetLoanStatusPreview", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetLoanStatusPreview(string svrguid)
    {
      return (string) this.Invoke(nameof (GetLoanStatusPreview), new object[1]
      {
        (object) svrguid
      })[0];
    }

    public IAsyncResult BeginGetLoanStatusPreview(
      string svrguid,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetLoanStatusPreview", new object[1]
      {
        (object) svrguid
      }, callback, asyncState);
    }

    public string EndGetLoanStatusPreview(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetLoanStatusPreviewAsync(string svrguid)
    {
      this.GetLoanStatusPreviewAsync(svrguid, (object) null);
    }

    public void GetLoanStatusPreviewAsync(string svrguid, object userState)
    {
      if (this.GetLoanStatusPreviewOperationCompleted == null)
        this.GetLoanStatusPreviewOperationCompleted = new SendOrPostCallback(this.OnGetLoanStatusPreviewOperationCompleted);
      this.InvokeAsync("GetLoanStatusPreview", new object[1]
      {
        (object) svrguid
      }, this.GetLoanStatusPreviewOperationCompleted, userState);
    }

    private void OnGetLoanStatusPreviewOperationCompleted(object arg)
    {
      if (this.GetLoanStatusPreviewCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetLoanStatusPreviewCompleted((object) this, new GetLoanStatusPreviewCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/UploadAttachment", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void UploadAttachment(string packageXML)
    {
      this.Invoke(nameof (UploadAttachment), new object[1]
      {
        (object) packageXML
      });
    }

    public IAsyncResult BeginUploadAttachment(
      string packageXML,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("UploadAttachment", new object[1]
      {
        (object) packageXML
      }, callback, asyncState);
    }

    public void EndUploadAttachment(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void UploadAttachmentAsync(string packageXML)
    {
      this.UploadAttachmentAsync(packageXML, (object) null);
    }

    public void UploadAttachmentAsync(string packageXML, object userState)
    {
      if (this.UploadAttachmentOperationCompleted == null)
        this.UploadAttachmentOperationCompleted = new SendOrPostCallback(this.OnUploadAttachmentOperationCompleted);
      this.InvokeAsync("UploadAttachment", new object[1]
      {
        (object) packageXML
      }, this.UploadAttachmentOperationCompleted, userState);
    }

    private void OnUploadAttachmentOperationCompleted(object arg)
    {
      if (this.UploadAttachmentCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.UploadAttachmentCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/ProcessRequest", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void ProcessRequest(string packageXML)
    {
      this.Invoke(nameof (ProcessRequest), new object[1]
      {
        (object) packageXML
      });
    }

    public IAsyncResult BeginProcessRequest(
      string packageXML,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ProcessRequest", new object[1]
      {
        (object) packageXML
      }, callback, asyncState);
    }

    public void EndProcessRequest(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void ProcessRequestAsync(string packageXML)
    {
      this.ProcessRequestAsync(packageXML, (object) null);
    }

    public void ProcessRequestAsync(string packageXML, object userState)
    {
      if (this.ProcessRequestOperationCompleted == null)
        this.ProcessRequestOperationCompleted = new SendOrPostCallback(this.OnProcessRequestOperationCompleted);
      this.InvokeAsync("ProcessRequest", new object[1]
      {
        (object) packageXML
      }, this.ProcessRequestOperationCompleted, userState);
    }

    private void OnProcessRequestOperationCompleted(object arg)
    {
      if (this.ProcessRequestCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ProcessRequestCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://loancenter.elliemae.com/ePackageWS/GetDisclosureTrackingDetails", RequestNamespace = "http://loancenter.elliemae.com/ePackageWS/", ResponseNamespace = "http://loancenter.elliemae.com/ePackageWS/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetDisclosureTrackingDetails(string packageInfo)
    {
      return (string) this.Invoke(nameof (GetDisclosureTrackingDetails), new object[1]
      {
        (object) packageInfo
      })[0];
    }

    public IAsyncResult BeginGetDisclosureTrackingDetails(
      string packageInfo,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetDisclosureTrackingDetails", new object[1]
      {
        (object) packageInfo
      }, callback, asyncState);
    }

    public string EndGetDisclosureTrackingDetails(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetDisclosureTrackingDetailsAsync(string packageInfo)
    {
      this.GetDisclosureTrackingDetailsAsync(packageInfo, (object) null);
    }

    public void GetDisclosureTrackingDetailsAsync(string packageInfo, object userState)
    {
      if (this.GetDisclosureTrackingDetailsOperationCompleted == null)
        this.GetDisclosureTrackingDetailsOperationCompleted = new SendOrPostCallback(this.OnGetDisclosureTrackingDetailsOperationCompleted);
      this.InvokeAsync("GetDisclosureTrackingDetails", new object[1]
      {
        (object) packageInfo
      }, this.GetDisclosureTrackingDetailsOperationCompleted, userState);
    }

    private void OnGetDisclosureTrackingDetailsOperationCompleted(object arg)
    {
      if (this.GetDisclosureTrackingDetailsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetDisclosureTrackingDetailsCompleted((object) this, new GetDisclosureTrackingDetailsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
