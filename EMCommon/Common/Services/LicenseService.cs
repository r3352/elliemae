// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Services.LicenseService
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

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
namespace EllieMae.EMLite.Common.Services
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "LicenseServiceSoap", Namespace = "http://tempuri.org/")]
  public class LicenseService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetClosedLoanBillingInfoOperationCompleted;

    public LicenseService() => this.Url = "https://encompass.elliemae.com/jedservices/License.asmx";

    public LicenseService(string jedServicesUrl)
    {
      if (string.IsNullOrWhiteSpace(jedServicesUrl) || !Uri.IsWellFormedUriString(jedServicesUrl, UriKind.Absolute))
        this.Url = "https://encompass.elliemae.com/jedservices/License.asmx";
      else
        this.Url = jedServicesUrl + "License.asmx";
    }

    protected override WebRequest GetWebRequest(Uri uri)
    {
      HttpWebRequest webRequest = (HttpWebRequest) base.GetWebRequest(uri);
      webRequest.KeepAlive = false;
      return (WebRequest) webRequest;
    }

    public event GetClosedLoanBillingInfoCompletedEventHandler GetClosedLoanBillingInfoCompleted;

    [SoapDocumentMethod("http://tempuri.org/GetClosedLoanBillingInfo", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public ClosedLoanBillingInfo GetClosedLoanBillingInfo(string clientID)
    {
      return (ClosedLoanBillingInfo) this.Invoke(nameof (GetClosedLoanBillingInfo), new object[1]
      {
        (object) clientID
      })[0];
    }

    public IAsyncResult BeginGetClosedLoanBillingInfo(
      string clientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetClosedLoanBillingInfo", new object[1]
      {
        (object) clientID
      }, callback, asyncState);
    }

    public ClosedLoanBillingInfo EndGetClosedLoanBillingInfo(IAsyncResult asyncResult)
    {
      return (ClosedLoanBillingInfo) this.EndInvoke(asyncResult)[0];
    }

    public void GetClosedLoanBillingInfoAsync(string clientID)
    {
      this.GetClosedLoanBillingInfoAsync(clientID, (object) null);
    }

    public void GetClosedLoanBillingInfoAsync(string clientID, object userState)
    {
      if (this.GetClosedLoanBillingInfoOperationCompleted == null)
        this.GetClosedLoanBillingInfoOperationCompleted = new SendOrPostCallback(this.OnGetClosedLoanBillingInfoOperationCompleted);
      this.InvokeAsync("GetClosedLoanBillingInfo", new object[1]
      {
        (object) clientID
      }, this.GetClosedLoanBillingInfoOperationCompleted, userState);
    }

    private void OnGetClosedLoanBillingInfoOperationCompleted(object arg)
    {
      if (this.GetClosedLoanBillingInfoCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetClosedLoanBillingInfoCompleted((object) this, new GetClosedLoanBillingInfoCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
