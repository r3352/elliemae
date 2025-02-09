// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.WebServices.LoanCenterServiceProxy
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
  [WebServiceBinding(Name = "LoanCenterServiceSoap", Namespace = "http://loancenter.elliemae.com/service/")]
  public class LoanCenterServiceProxy : SoapHttpClientProtocol
  {
    private SendOrPostCallback SendEmailOperationCompleted;

    public LoanCenterServiceProxy()
    {
      this.Url = "https://loancenter.elliemae.com/service/loancenter.asmx";
    }

    public LoanCenterServiceProxy(string loancenterServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(loancenterServiceUrl) || !Uri.IsWellFormedUriString(loancenterServiceUrl, UriKind.Absolute))
        this.Url = "https://loancenter.elliemae.com/service/loancenter.asmx";
      else
        this.Url = loancenterServiceUrl;
    }

    public event SendEmailCompletedEventHandler SendEmailCompleted;

    [SoapDocumentMethod("http://loancenter.elliemae.com/service/SendEmail", RequestNamespace = "http://loancenter.elliemae.com/service/", ResponseNamespace = "http://loancenter.elliemae.com/service/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public void SendEmail(
      string from,
      string to,
      string cc,
      string bcc,
      string subject,
      string body)
    {
      this.Invoke(nameof (SendEmail), new object[6]
      {
        (object) from,
        (object) to,
        (object) cc,
        (object) bcc,
        (object) subject,
        (object) body
      });
    }

    public IAsyncResult BeginSendEmail(
      string from,
      string to,
      string cc,
      string bcc,
      string subject,
      string body,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("SendEmail", new object[6]
      {
        (object) from,
        (object) to,
        (object) cc,
        (object) bcc,
        (object) subject,
        (object) body
      }, callback, asyncState);
    }

    public void EndSendEmail(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);

    public void SendEmailAsync(
      string from,
      string to,
      string cc,
      string bcc,
      string subject,
      string body)
    {
      this.SendEmailAsync(from, to, cc, bcc, subject, body, (object) null);
    }

    public void SendEmailAsync(
      string from,
      string to,
      string cc,
      string bcc,
      string subject,
      string body,
      object userState)
    {
      if (this.SendEmailOperationCompleted == null)
        this.SendEmailOperationCompleted = new SendOrPostCallback(this.OnSendEmailOperationCompleted);
      this.InvokeAsync("SendEmail", new object[6]
      {
        (object) from,
        (object) to,
        (object) cc,
        (object) bcc,
        (object) subject,
        (object) body
      }, this.SendEmailOperationCompleted, userState);
    }

    private void OnSendEmailOperationCompleted(object arg)
    {
      if (this.SendEmailCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.SendEmailCompleted((object) this, new AsyncCompletedEventArgs(completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
