// Decompiled with JetBrains decompiler
// Type: RestApiProxy.WebhookService.WebhookEventHelper
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

#nullable disable
namespace RestApiProxy.WebhookService
{
  public class WebhookEventHelper
  {
    private string _instanceName;
    private string _sessionId;
    private WebhookResource _eventResource;

    public WebhookEventHelper(string instanceName, string sessionId, WebhookResource eventResource)
    {
      this._instanceName = instanceName;
      this._sessionId = sessionId;
      this._eventResource = eventResource;
    }

    public string SessionId => this._instanceName + "_" + this._sessionId;

    public void Publish(WebhookEventContract eventContract)
    {
      WebhookEventConsts webhookEventConsts = new WebhookEventConsts(this._eventResource);
      eventContract.ResourceType = webhookEventConsts.EventResourceName;
      eventContract.ResourceRef = webhookEventConsts.ResourceRef + eventContract.ResourceId;
      eventContract.Source = "urn:elli:encompass:" + eventContract.InstanceId;
      HttpClient restApiWebhookProxy = RestApiProxyFactory.CreateOAPIGatewayRestApiWebhookProxy(this.SessionId, "application/json", Session.StartupInfo?.WebhookEventXapiKey, "x-api-key");
      StringContent stringContent = new StringContent(JsonConvert.SerializeObject((object) eventContract, new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver(),
        NullValueHandling = NullValueHandling.Ignore
      }), Encoding.UTF8, "application/json");
      string eventEndPoint = webhookEventConsts.EventEndPoint;
      StringContent content = stringContent;
      HttpResponseMessage result = restApiWebhookProxy.PostAsync(eventEndPoint, (HttpContent) content).Result;
      RemoteLogger.Write(TraceLevel.Info, string.Format("Publishing {1} {2} webhook event for the contract - {0}", (object) JsonConvert.SerializeObject((object) eventContract), (object) webhookEventConsts.EventResourceName, (object) eventContract.EventType));
      if (result != null && (result == null || result.IsSuccessStatusCode))
        return;
      RemoteLogger.Write(TraceLevel.Error, string.Format("Publishing {3} {4} webhook event failed with a responsecode - {0} response message - {1} for the contract - {2}", (object) result.StatusCode, (object) result.ReasonPhrase, (object) JsonConvert.SerializeObject((object) eventContract), (object) webhookEventConsts.EventResourceName, (object) eventContract.EventType));
    }
  }
}
