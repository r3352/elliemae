// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.WcfExtensions.SsoTokenEndpointBehavior
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.WcfExtensions
{
  public class SsoTokenEndpointBehavior : IEndpointBehavior
  {
    private string _ssoToken;

    public SsoTokenEndpointBehavior(string ssoToken) => this._ssoToken = ssoToken;

    public void AddBindingParameters(
      ServiceEndpoint endpoint,
      BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
      clientRuntime.MessageInspectors.Add((IClientMessageInspector) new SsoTokenEndpointBehavior.SsoHeaderMessageInspector(this._ssoToken));
    }

    public void ApplyDispatchBehavior(
      ServiceEndpoint endpoint,
      EndpointDispatcher endpointDispatcher)
    {
    }

    public void Validate(ServiceEndpoint endpoint)
    {
    }

    private class SsoHeaderMessageInspector : IClientMessageInspector
    {
      private string _ssoToken;

      public SsoHeaderMessageInspector(string ssoToken) => this._ssoToken = ssoToken;

      public void AfterReceiveReply(ref Message reply, object correlationState)
      {
      }

      public object BeforeSendRequest(ref Message request, IClientChannel channel)
      {
        if (!string.IsNullOrWhiteSpace(this._ssoToken))
        {
          string str = "EMAuth " + this._ssoToken;
          object obj;
          if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out obj))
          {
            HttpRequestMessageProperty requestMessageProperty = obj as HttpRequestMessageProperty;
            if (string.IsNullOrEmpty(requestMessageProperty.Headers["Authorization"]))
              requestMessageProperty.Headers["Authorization"] = str;
          }
          else
          {
            HttpRequestMessageProperty property = new HttpRequestMessageProperty();
            property.Headers.Add("Authorization", str);
            request.Properties.Add(HttpRequestMessageProperty.Name, (object) property);
          }
        }
        return (object) null;
      }
    }
  }
}
