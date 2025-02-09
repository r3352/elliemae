// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ePassAService
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "ePassAServiceSoap", Namespace = "http://tempuri.org/")]
  public class ePassAService : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetePassAServiceOperationCompleted;

    public ePassAService(string ePassAServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(ePassAServiceUrl) || !Uri.IsWellFormedUriString(ePassAServiceUrl, UriKind.Absolute))
        this.Url = "https://www.epassbusinesscenter.com/epassws/ePassAService.asmx";
      else
        this.Url = ePassAServiceUrl;
    }

    public ePassAService()
    {
      this.Url = "https://www.epassbusinesscenter.com/epassws/ePassAService.asmx";
    }

    public event GetePassAServiceCompletedEventHandler GetePassAServiceCompleted;

    [SoapDocumentMethod("http://tempuri.org/GetePassAService", RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    public string GetePassAService(string dParam, string vParam, string tParam)
    {
      return (string) this.Invoke(nameof (GetePassAService), new object[3]
      {
        (object) dParam,
        (object) vParam,
        (object) tParam
      })[0];
    }

    public IAsyncResult BeginGetePassAService(
      string dParam,
      string vParam,
      string tParam,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetePassAService", new object[3]
      {
        (object) dParam,
        (object) vParam,
        (object) tParam
      }, callback, asyncState);
    }

    public string EndGetePassAService(IAsyncResult asyncResult)
    {
      return (string) this.EndInvoke(asyncResult)[0];
    }

    public void GetePassAServiceAsync(string dParam, string vParam, string tParam)
    {
      this.GetePassAServiceAsync(dParam, vParam, tParam, (object) null);
    }

    public void GetePassAServiceAsync(
      string dParam,
      string vParam,
      string tParam,
      object userState)
    {
      if (this.GetePassAServiceOperationCompleted == null)
        this.GetePassAServiceOperationCompleted = new SendOrPostCallback(this.OnGetePassAServiceOperationCompleted);
      this.InvokeAsync("GetePassAService", new object[3]
      {
        (object) dParam,
        (object) vParam,
        (object) tParam
      }, this.GetePassAServiceOperationCompleted, userState);
    }

    private void OnGetePassAServiceOperationCompleted(object arg)
    {
      if (this.GetePassAServiceCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetePassAServiceCompleted((object) this, new GetePassAServiceCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
