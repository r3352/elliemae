// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.DocService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
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
  [GeneratedCode("wsdl", "2.0.50727.3038")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "WSSoap", Namespace = "http://www.odi.com/EncompassIntegration/EncompassWS")]
  public class DocService : SoapHttpClientProtocol
  {
    private SendOrPostCallback HelloOperationCompleted;
    private SendOrPostCallback ValidateCredentialsOperationCompleted;
    private SendOrPostCallback VersionInfoOperationCompleted;
    private SendOrPostCallback VersionStackOperationCompleted;
    private SendOrPostCallback MergeOnDemandOperationCompleted;
    private SendOrPostCallback AuditLoanOperationCompleted;
    private SendOrPostCallback GetDocDataOperationCompleted;
    private SendOrPostCallback GetInvestorPlancodeListOperationCompleted;
    private SendOrPostCallback GetPlancodeDetailsOperationCompleted;
    private SendOrPostCallback GetInvestorListOperationCompleted;
    private SendOrPostCallback GetAltLenderListOperationCompleted;
    private SendOrPostCallback BuildDocsetOperationCompleted;
    private SendOrPostCallback PickupDocsetOperationCompleted;
    private SendOrPostCallback DeleteDocsetOperationCompleted;
    private SendOrPostCallback PerformSettingsLookupOperationCompleted;
    private SendOrPostCallback GetLateFeesOperationCompleted;
    private SendOrPostCallback GetClientClosingDocsStatusOperationCompleted;
    private SendOrPostCallback GetFormsListOperationCompleted;
    private SendOrPostCallback GetFormOperationCompleted;
    private SendOrPostCallback FormsLibraryAuthenticationOperationCompleted;
    private SendOrPostCallback LogEventOperationCompleted;
    private SendOrPostCallback MergeNativeOperationCompleted;
    private string _ssoToken;

    public DocService(string ssoToken, string docServiceUrl)
      : this(ssoToken)
    {
      if (string.IsNullOrWhiteSpace(docServiceUrl) || !Uri.IsWellFormedUriString(docServiceUrl, UriKind.Absolute))
        this.Url = "https://docsengine.elliemae.com/emdocs/ws.asmx";
      else
        this.Url = docServiceUrl;
    }

    public DocService(string ssoToken = null)
    {
      this.Url = "https://docsengine.elliemae.com/emdocs/ws.asmx";
      this._ssoToken = ssoToken;
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      WebRequest webRequest = base.GetWebRequest(uri);
      if (!string.IsNullOrWhiteSpace(this._ssoToken))
        webRequest.Headers.Add("Authorization", "EMAuth " + this._ssoToken);
      return webRequest;
    }

    public event HelloCompletedEventHandler HelloCompleted;

    public event ValidateCredentialsCompletedEventHandler ValidateCredentialsCompleted;

    public event VersionInfoCompletedEventHandler VersionInfoCompleted;

    public event VersionStackCompletedEventHandler VersionStackCompleted;

    public event MergeOnDemandCompletedEventHandler MergeOnDemandCompleted;

    public event AuditLoanCompletedEventHandler AuditLoanCompleted;

    public event GetDocDataCompletedEventHandler GetDocDataCompleted;

    public event GetInvestorPlancodeListCompletedEventHandler GetInvestorPlancodeListCompleted;

    public event GetPlancodeDetailsCompletedEventHandler GetPlancodeDetailsCompleted;

    public event GetInvestorListCompletedEventHandler GetInvestorListCompleted;

    public event GetAltLenderListCompletedEventHandler GetAltLenderListCompleted;

    public event BuildDocsetCompletedEventHandler BuildDocsetCompleted;

    public event PickupDocsetCompletedEventHandler PickupDocsetCompleted;

    public event DeleteDocsetCompletedEventHandler DeleteDocsetCompleted;

    public event PerformSettingsLookupCompletedEventHandler PerformSettingsLookupCompleted;

    public event GetLateFeesCompletedEventHandler GetLateFeesCompleted;

    public event GetClientClosingDocsStatusCompletedEventHandler GetClientClosingDocsStatusCompleted;

    public event GetFormsListCompletedEventHandler GetFormsListCompleted;

    public event GetFormCompletedEventHandler GetFormCompleted;

    public event FormsLibraryAuthenticationCompletedEventHandler FormsLibraryAuthenticationCompleted;

    public event LogEventCompletedEventHandler LogEventCompleted;

    public event MergeNativeCompletedEventHandler MergeNativeCompleted;

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/Hello", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string Hello() => (string) this.Invoke(nameof (Hello), new object[0])[0];

    public IAsyncResult BeginHello(AsyncCallback callback, object asyncState)
    {
      return this.BeginInvoke("Hello", new object[0], callback, asyncState);
    }

    public string EndHello(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    public void HelloAsync() => this.HelloAsync((object) null);

    public void HelloAsync(object userState)
    {
      if (this.HelloOperationCompleted == null)
        this.HelloOperationCompleted = new SendOrPostCallback(this.OnHelloOperationCompleted);
      this.InvokeAsync("Hello", new object[0], this.HelloOperationCompleted, userState);
    }

    private void OnHelloOperationCompleted(object arg)
    {
      if (this.HelloCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.HelloCompleted((object) this, new HelloCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/ValidateCredentials", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string ValidateCredentials(string requestXmlString)
    {
      return (string) this.Invoke(nameof (ValidateCredentials), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginValidateCredentials(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("ValidateCredentials", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndValidateCredentials(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void ValidateCredentialsAsync(string requestXmlString)
    {
      this.ValidateCredentialsAsync(requestXmlString, (object) null);
    }

    public void ValidateCredentialsAsync(string requestXmlString, object userState)
    {
      if (this.ValidateCredentialsOperationCompleted == null)
        this.ValidateCredentialsOperationCompleted = new SendOrPostCallback(this.OnValidateCredentialsOperationCompleted);
      this.InvokeAsync("ValidateCredentials", new object[1]
      {
        (object) requestXmlString
      }, this.ValidateCredentialsOperationCompleted, userState);
    }

    private void OnValidateCredentialsOperationCompleted(object arg)
    {
      if (this.ValidateCredentialsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.ValidateCredentialsCompleted((object) this, new ValidateCredentialsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/VersionInfo", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string VersionInfo(string requestXmlString)
    {
      return (string) this.Invoke(nameof (VersionInfo), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginVersionInfo(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("VersionInfo", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndVersionInfo(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void VersionInfoAsync(string requestXmlString)
    {
      this.VersionInfoAsync(requestXmlString, (object) null);
    }

    public void VersionInfoAsync(string requestXmlString, object userState)
    {
      if (this.VersionInfoOperationCompleted == null)
        this.VersionInfoOperationCompleted = new SendOrPostCallback(this.OnVersionInfoOperationCompleted);
      this.InvokeAsync("VersionInfo", new object[1]
      {
        (object) requestXmlString
      }, this.VersionInfoOperationCompleted, userState);
    }

    private void OnVersionInfoOperationCompleted(object arg)
    {
      if (this.VersionInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.VersionInfoCompleted((object) this, new VersionInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/VersionStack", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string VersionStack(string requestXmlString)
    {
      return (string) this.Invoke(nameof (VersionStack), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginVersionStack(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("VersionStack", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndVersionStack(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void VersionStackAsync(string requestXmlString)
    {
      this.VersionStackAsync(requestXmlString, (object) null);
    }

    public void VersionStackAsync(string requestXmlString, object userState)
    {
      if (this.VersionStackOperationCompleted == null)
        this.VersionStackOperationCompleted = new SendOrPostCallback(this.OnVersionStackOperationCompleted);
      this.InvokeAsync("VersionStack", new object[1]
      {
        (object) requestXmlString
      }, this.VersionStackOperationCompleted, userState);
    }

    private void OnVersionStackOperationCompleted(object arg)
    {
      if (this.VersionStackCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.VersionStackCompleted((object) this, new VersionStackCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/MergeOnDemand", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string MergeOnDemand(string requestXmlString)
    {
      return (string) this.Invoke(nameof (MergeOnDemand), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginMergeOnDemand(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("MergeOnDemand", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndMergeOnDemand(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void MergeOnDemandAsync(string requestXmlString)
    {
      this.MergeOnDemandAsync(requestXmlString, (object) null);
    }

    public void MergeOnDemandAsync(string requestXmlString, object userState)
    {
      if (this.MergeOnDemandOperationCompleted == null)
        this.MergeOnDemandOperationCompleted = new SendOrPostCallback(this.OnMergeOnDemandOperationCompleted);
      this.InvokeAsync("MergeOnDemand", new object[1]
      {
        (object) requestXmlString
      }, this.MergeOnDemandOperationCompleted, userState);
    }

    private void OnMergeOnDemandOperationCompleted(object arg)
    {
      if (this.MergeOnDemandCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.MergeOnDemandCompleted((object) this, new MergeOnDemandCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/AuditLoan", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string AuditLoan(string requestXmlString)
    {
      return (string) this.Invoke(nameof (AuditLoan), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginAuditLoan(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("AuditLoan", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndAuditLoan(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    public void AuditLoanAsync(string requestXmlString)
    {
      this.AuditLoanAsync(requestXmlString, (object) null);
    }

    public void AuditLoanAsync(string requestXmlString, object userState)
    {
      if (this.AuditLoanOperationCompleted == null)
        this.AuditLoanOperationCompleted = new SendOrPostCallback(this.OnAuditLoanOperationCompleted);
      this.InvokeAsync("AuditLoan", new object[1]
      {
        (object) requestXmlString
      }, this.AuditLoanOperationCompleted, userState);
    }

    private void OnAuditLoanOperationCompleted(object arg)
    {
      if (this.AuditLoanCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.AuditLoanCompleted((object) this, new AuditLoanCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetDocData", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetDocData(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetDocData), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetDocData(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetDocData", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetDocData(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetDocDataAsync(string requestXmlString)
    {
      this.GetDocDataAsync(requestXmlString, (object) null);
    }

    public void GetDocDataAsync(string requestXmlString, object userState)
    {
      if (this.GetDocDataOperationCompleted == null)
        this.GetDocDataOperationCompleted = new SendOrPostCallback(this.OnGetDocDataOperationCompleted);
      this.InvokeAsync("GetDocData", new object[1]
      {
        (object) requestXmlString
      }, this.GetDocDataOperationCompleted, userState);
    }

    private void OnGetDocDataOperationCompleted(object arg)
    {
      if (this.GetDocDataCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetDocDataCompleted((object) this, new GetDocDataCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetInvestorPlancodeList", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetInvestorPlancodeList(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetInvestorPlancodeList), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetInvestorPlancodeList(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetInvestorPlancodeList", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetInvestorPlancodeList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetInvestorPlancodeListAsync(string requestXmlString)
    {
      this.GetInvestorPlancodeListAsync(requestXmlString, (object) null);
    }

    public void GetInvestorPlancodeListAsync(string requestXmlString, object userState)
    {
      if (this.GetInvestorPlancodeListOperationCompleted == null)
        this.GetInvestorPlancodeListOperationCompleted = new SendOrPostCallback(this.OnGetInvestorPlancodeListOperationCompleted);
      this.InvokeAsync("GetInvestorPlancodeList", new object[1]
      {
        (object) requestXmlString
      }, this.GetInvestorPlancodeListOperationCompleted, userState);
    }

    private void OnGetInvestorPlancodeListOperationCompleted(object arg)
    {
      if (this.GetInvestorPlancodeListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetInvestorPlancodeListCompleted((object) this, new GetInvestorPlancodeListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetPlancodeDetails", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetPlancodeDetails(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetPlancodeDetails), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetPlancodeDetails(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetPlancodeDetails", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetPlancodeDetails(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetPlancodeDetailsAsync(string requestXmlString)
    {
      this.GetPlancodeDetailsAsync(requestXmlString, (object) null);
    }

    public void GetPlancodeDetailsAsync(string requestXmlString, object userState)
    {
      if (this.GetPlancodeDetailsOperationCompleted == null)
        this.GetPlancodeDetailsOperationCompleted = new SendOrPostCallback(this.OnGetPlancodeDetailsOperationCompleted);
      this.InvokeAsync("GetPlancodeDetails", new object[1]
      {
        (object) requestXmlString
      }, this.GetPlancodeDetailsOperationCompleted, userState);
    }

    private void OnGetPlancodeDetailsOperationCompleted(object arg)
    {
      if (this.GetPlancodeDetailsCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetPlancodeDetailsCompleted((object) this, new GetPlancodeDetailsCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetInvestorList", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetInvestorList(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetInvestorList), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetInvestorList(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetInvestorList", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetInvestorList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetInvestorListAsync(string requestXmlString)
    {
      this.GetInvestorListAsync(requestXmlString, (object) null);
    }

    public void GetInvestorListAsync(string requestXmlString, object userState)
    {
      if (this.GetInvestorListOperationCompleted == null)
        this.GetInvestorListOperationCompleted = new SendOrPostCallback(this.OnGetInvestorListOperationCompleted);
      this.InvokeAsync("GetInvestorList", new object[1]
      {
        (object) requestXmlString
      }, this.GetInvestorListOperationCompleted, userState);
    }

    private void OnGetInvestorListOperationCompleted(object arg)
    {
      if (this.GetInvestorListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetInvestorListCompleted((object) this, new GetInvestorListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetAltLenderList", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetAltLenderList(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetAltLenderList), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetAltLenderList(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetAltLenderList", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetAltLenderList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetAltLenderListAsync(string requestXmlString)
    {
      this.GetAltLenderListAsync(requestXmlString, (object) null);
    }

    public void GetAltLenderListAsync(string requestXmlString, object userState)
    {
      if (this.GetAltLenderListOperationCompleted == null)
        this.GetAltLenderListOperationCompleted = new SendOrPostCallback(this.OnGetAltLenderListOperationCompleted);
      this.InvokeAsync("GetAltLenderList", new object[1]
      {
        (object) requestXmlString
      }, this.GetAltLenderListOperationCompleted, userState);
    }

    private void OnGetAltLenderListOperationCompleted(object arg)
    {
      if (this.GetAltLenderListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetAltLenderListCompleted((object) this, new GetAltLenderListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/BuildDocset", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string BuildDocset(string requestXmlString)
    {
      return (string) this.Invoke(nameof (BuildDocset), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginBuildDocset(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("BuildDocset", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndBuildDocset(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void BuildDocsetAsync(string requestXmlString)
    {
      this.BuildDocsetAsync(requestXmlString, (object) null);
    }

    public void BuildDocsetAsync(string requestXmlString, object userState)
    {
      if (this.BuildDocsetOperationCompleted == null)
        this.BuildDocsetOperationCompleted = new SendOrPostCallback(this.OnBuildDocsetOperationCompleted);
      this.InvokeAsync("BuildDocset", new object[1]
      {
        (object) requestXmlString
      }, this.BuildDocsetOperationCompleted, userState);
    }

    private void OnBuildDocsetOperationCompleted(object arg)
    {
      if (this.BuildDocsetCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.BuildDocsetCompleted((object) this, new BuildDocsetCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/PickupDocset", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string PickupDocset(string requestXmlString)
    {
      return (string) this.Invoke(nameof (PickupDocset), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginPickupDocset(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("PickupDocset", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndPickupDocset(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void PickupDocsetAsync(string requestXmlString)
    {
      this.PickupDocsetAsync(requestXmlString, (object) null);
    }

    public void PickupDocsetAsync(string requestXmlString, object userState)
    {
      if (this.PickupDocsetOperationCompleted == null)
        this.PickupDocsetOperationCompleted = new SendOrPostCallback(this.OnPickupDocsetOperationCompleted);
      this.InvokeAsync("PickupDocset", new object[1]
      {
        (object) requestXmlString
      }, this.PickupDocsetOperationCompleted, userState);
    }

    private void OnPickupDocsetOperationCompleted(object arg)
    {
      if (this.PickupDocsetCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.PickupDocsetCompleted((object) this, new PickupDocsetCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/DeleteDocset", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string DeleteDocset(string requestXmlString)
    {
      return (string) this.Invoke(nameof (DeleteDocset), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginDeleteDocset(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("DeleteDocset", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndDeleteDocset(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void DeleteDocsetAsync(string requestXmlString)
    {
      this.DeleteDocsetAsync(requestXmlString, (object) null);
    }

    public void DeleteDocsetAsync(string requestXmlString, object userState)
    {
      if (this.DeleteDocsetOperationCompleted == null)
        this.DeleteDocsetOperationCompleted = new SendOrPostCallback(this.OnDeleteDocsetOperationCompleted);
      this.InvokeAsync("DeleteDocset", new object[1]
      {
        (object) requestXmlString
      }, this.DeleteDocsetOperationCompleted, userState);
    }

    private void OnDeleteDocsetOperationCompleted(object arg)
    {
      if (this.DeleteDocsetCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.DeleteDocsetCompleted((object) this, new DeleteDocsetCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/PerformSettingsLookup", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string PerformSettingsLookup(string requestXmlString)
    {
      return (string) this.Invoke(nameof (PerformSettingsLookup), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginPerformSettingsLookup(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("PerformSettingsLookup", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndPerformSettingsLookup(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void PerformSettingsLookupAsync(string requestXmlString)
    {
      this.PerformSettingsLookupAsync(requestXmlString, (object) null);
    }

    public void PerformSettingsLookupAsync(string requestXmlString, object userState)
    {
      if (this.PerformSettingsLookupOperationCompleted == null)
        this.PerformSettingsLookupOperationCompleted = new SendOrPostCallback(this.OnPerformSettingsLookupOperationCompleted);
      this.InvokeAsync("PerformSettingsLookup", new object[1]
      {
        (object) requestXmlString
      }, this.PerformSettingsLookupOperationCompleted, userState);
    }

    private void OnPerformSettingsLookupOperationCompleted(object arg)
    {
      if (this.PerformSettingsLookupCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.PerformSettingsLookupCompleted((object) this, new PerformSettingsLookupCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetLateFees", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetLateFees(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetLateFees), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetLateFees(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetLateFees", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetLateFees(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetLateFeesAsync(string requestXmlString)
    {
      this.GetLateFeesAsync(requestXmlString, (object) null);
    }

    public void GetLateFeesAsync(string requestXmlString, object userState)
    {
      if (this.GetLateFeesOperationCompleted == null)
        this.GetLateFeesOperationCompleted = new SendOrPostCallback(this.OnGetLateFeesOperationCompleted);
      this.InvokeAsync("GetLateFees", new object[1]
      {
        (object) requestXmlString
      }, this.GetLateFeesOperationCompleted, userState);
    }

    private void OnGetLateFeesOperationCompleted(object arg)
    {
      if (this.GetLateFeesCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetLateFeesCompleted((object) this, new GetLateFeesCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetClientClosingDocsStatus", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetClientClosingDocsStatus(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetClientClosingDocsStatus), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetClientClosingDocsStatus(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetClientClosingDocsStatus", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetClientClosingDocsStatus(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetClientClosingDocsStatusAsync(string requestXmlString)
    {
      this.GetClientClosingDocsStatusAsync(requestXmlString, (object) null);
    }

    public void GetClientClosingDocsStatusAsync(string requestXmlString, object userState)
    {
      if (this.GetClientClosingDocsStatusOperationCompleted == null)
        this.GetClientClosingDocsStatusOperationCompleted = new SendOrPostCallback(this.OnGetClientClosingDocsStatusOperationCompleted);
      this.InvokeAsync("GetClientClosingDocsStatus", new object[1]
      {
        (object) requestXmlString
      }, this.GetClientClosingDocsStatusOperationCompleted, userState);
    }

    private void OnGetClientClosingDocsStatusOperationCompleted(object arg)
    {
      if (this.GetClientClosingDocsStatusCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetClientClosingDocsStatusCompleted((object) this, new GetClientClosingDocsStatusCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetFormsList", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetFormsList(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetFormsList), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetFormsList(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetFormsList", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetFormsList(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetFormsListAsync(string requestXmlString)
    {
      this.GetFormsListAsync(requestXmlString, (object) null);
    }

    public void GetFormsListAsync(string requestXmlString, object userState)
    {
      if (this.GetFormsListOperationCompleted == null)
        this.GetFormsListOperationCompleted = new SendOrPostCallback(this.OnGetFormsListOperationCompleted);
      this.InvokeAsync("GetFormsList", new object[1]
      {
        (object) requestXmlString
      }, this.GetFormsListOperationCompleted, userState);
    }

    private void OnGetFormsListOperationCompleted(object arg)
    {
      if (this.GetFormsListCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetFormsListCompleted((object) this, new GetFormsListCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/GetForm", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetForm(string requestXmlString)
    {
      return (string) this.Invoke(nameof (GetForm), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginGetForm(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetForm", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndGetForm(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    public void GetFormAsync(string requestXmlString)
    {
      this.GetFormAsync(requestXmlString, (object) null);
    }

    public void GetFormAsync(string requestXmlString, object userState)
    {
      if (this.GetFormOperationCompleted == null)
        this.GetFormOperationCompleted = new SendOrPostCallback(this.OnGetFormOperationCompleted);
      this.InvokeAsync("GetForm", new object[1]
      {
        (object) requestXmlString
      }, this.GetFormOperationCompleted, userState);
    }

    private void OnGetFormOperationCompleted(object arg)
    {
      if (this.GetFormCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetFormCompleted((object) this, new GetFormCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/FormsLibraryAuthentication", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string FormsLibraryAuthentication(string requestXmlString)
    {
      return (string) this.Invoke(nameof (FormsLibraryAuthentication), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginFormsLibraryAuthentication(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("FormsLibraryAuthentication", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndFormsLibraryAuthentication(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void FormsLibraryAuthenticationAsync(string requestXmlString)
    {
      this.FormsLibraryAuthenticationAsync(requestXmlString, (object) null);
    }

    public void FormsLibraryAuthenticationAsync(string requestXmlString, object userState)
    {
      if (this.FormsLibraryAuthenticationOperationCompleted == null)
        this.FormsLibraryAuthenticationOperationCompleted = new SendOrPostCallback(this.OnFormsLibraryAuthenticationOperationCompleted);
      this.InvokeAsync("FormsLibraryAuthentication", new object[1]
      {
        (object) requestXmlString
      }, this.FormsLibraryAuthenticationOperationCompleted, userState);
    }

    private void OnFormsLibraryAuthenticationOperationCompleted(object arg)
    {
      if (this.FormsLibraryAuthenticationCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.FormsLibraryAuthenticationCompleted((object) this, new FormsLibraryAuthenticationCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/LogEvent", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string LogEvent(string requestXmlString)
    {
      return (string) this.Invoke(nameof (LogEvent), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginLogEvent(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("LogEvent", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndLogEvent(IAsyncResult asyncResult) => (string) this.EndInvoke(asyncResult)[0];

    public void LogEventAsync(string requestXmlString)
    {
      this.LogEventAsync(requestXmlString, (object) null);
    }

    public void LogEventAsync(string requestXmlString, object userState)
    {
      if (this.LogEventOperationCompleted == null)
        this.LogEventOperationCompleted = new SendOrPostCallback(this.OnLogEventOperationCompleted);
      this.InvokeAsync("LogEvent", new object[1]
      {
        (object) requestXmlString
      }, this.LogEventOperationCompleted, userState);
    }

    private void OnLogEventOperationCompleted(object arg)
    {
      if (this.LogEventCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.LogEventCompleted((object) this, new LogEventCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    [SoapDocumentMethod("http://www.odi.com/EncompassIntegration/EncompassWS/MergeNative", RequestNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", ResponseNamespace = "http://www.odi.com/EncompassIntegration/EncompassWS", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string MergeNative(string requestXmlString)
    {
      return (string) this.Invoke(nameof (MergeNative), new object[1]
      {
        (object) requestXmlString
      })[0];
    }

    public IAsyncResult BeginMergeNative(
      string requestXmlString,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("MergeNative", new object[1]
      {
        (object) requestXmlString
      }, callback, asyncState);
    }

    public string EndMergeNative(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void MergeNativeAsync(string requestXmlString)
    {
      this.MergeNativeAsync(requestXmlString, (object) null);
    }

    public void MergeNativeAsync(string requestXmlString, object userState)
    {
      if (this.MergeNativeOperationCompleted == null)
        this.MergeNativeOperationCompleted = new SendOrPostCallback(this.OnMergeNativeOperationCompleted);
      this.InvokeAsync("MergeNative", new object[1]
      {
        (object) requestXmlString
      }, this.MergeNativeOperationCompleted, userState);
    }

    private void OnMergeNativeOperationCompleted(object arg)
    {
      if (this.MergeNativeCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.MergeNativeCompleted((object) this, new MergeNativeCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
