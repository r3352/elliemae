// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.PPEservice
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("wsdl", "2.0.50727.42")]
  [DebuggerStepThrough]
  [DesignerCategory("code")]
  [WebServiceBinding(Name = "PPEserviceSoap", Namespace = "http://epass.elliemaeservices.com/PPEservices")]
  public class PPEservice : SoapHttpClientProtocol
  {
    private SendOrPostCallback GetPartnersOperationCompleted;

    public PPEservice()
    {
      this.Url = "https://epass.elliemaeservices.com/ProductPricingEligibility/PPE.asmx";
    }

    public PPEservice(string ppeServiceUrl)
    {
      if (string.IsNullOrWhiteSpace(ppeServiceUrl) || !Uri.IsWellFormedUriString(ppeServiceUrl, UriKind.Absolute))
        this.Url = "https://epass.elliemaeservices.com/ProductPricingEligibility/PPE.asmx";
      else
        this.Url = ppeServiceUrl;
    }

    public event GetPartnersCompletedEventHandler GetPartnersCompleted;

    [SoapDocumentMethod("http://epass.elliemaeservices.com/PPEservices/GetPartners", RequestNamespace = "http://epass.elliemaeservices.com/PPEservices", ResponseNamespace = "http://epass.elliemaeservices.com/PPEservices", Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
    [return: XmlArrayItem(IsNullable = false)]
    public Partner[] GetPartners(string encompassClientID)
    {
      return (Partner[]) this.Invoke(nameof (GetPartners), new object[1]
      {
        (object) encompassClientID
      })[0];
    }

    public IAsyncResult BeginGetPartners(
      string encompassClientID,
      AsyncCallback callback,
      object asyncState)
    {
      return this.BeginInvoke("GetPartners", new object[1]
      {
        (object) encompassClientID
      }, callback, asyncState);
    }

    public Partner[] EndGetPartners(IAsyncResult asyncResult)
    {
      return (Partner[]) this.EndInvoke(asyncResult)[0];
    }

    public void GetPartnersAsync(string encompassClientID)
    {
      this.GetPartnersAsync(encompassClientID, (object) null);
    }

    public void GetPartnersAsync(string encompassClientID, object userState)
    {
      if (this.GetPartnersOperationCompleted == null)
        this.GetPartnersOperationCompleted = new SendOrPostCallback(this.OnGetPartnersOperationCompleted);
      this.InvokeAsync("GetPartners", new object[1]
      {
        (object) encompassClientID
      }, this.GetPartnersOperationCompleted, userState);
    }

    private void OnGetPartnersOperationCompleted(object arg)
    {
      if (this.GetPartnersCompleted == null)
        return;
      InvokeCompletedEventArgs completedEventArgs = (InvokeCompletedEventArgs) arg;
      this.GetPartnersCompleted((object) this, new GetPartnersCompletedEventArgs(completedEventArgs.Results, completedEventArgs.Error, completedEventArgs.Cancelled, completedEventArgs.UserState));
    }

    public new void CancelAsync(object userState) => base.CancelAsync(userState);
  }
}
