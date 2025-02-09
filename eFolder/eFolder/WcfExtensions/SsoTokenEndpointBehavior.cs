// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.WcfExtensions.SsoTokenEndpointBehavior
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

#nullable disable
namespace EllieMae.EMLite.eFolder.WcfExtensions
{
  public class SsoTokenEndpointBehavior : IEndpointBehavior
  {
    public void AddBindingParameters(
      ServiceEndpoint endpoint,
      BindingParameterCollection bindingParameters)
    {
    }

    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
    {
      clientRuntime.MessageInspectors.Add((IClientMessageInspector) new SsoTokenEndpointBehavior.SsoHeaderMessageInspector());
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
      public void AfterReceiveReply(ref Message reply, object correlationState)
      {
      }

      public object BeforeSendRequest(ref Message request, IClientChannel channel)
      {
        if (Session.DefaultInstance != null && Session.SessionObjects.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
        {
          int result = 5;
          int.TryParse(Session.ConfigurationManager.GetSsoTokenExpirationTimeForEdm(), out result);
          string ssoToken = Session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
          {
            "Elli.Edm"
          }, result);
          if (!string.IsNullOrWhiteSpace(ssoToken))
          {
            string str = "EMAuth " + ssoToken;
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
        }
        return (object) null;
      }
    }
  }
}
