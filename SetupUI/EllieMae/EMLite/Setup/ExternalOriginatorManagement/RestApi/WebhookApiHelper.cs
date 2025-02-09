// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi.WebhookApiHelper
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestApiProxy;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Setup.ExternalOriginatorManagement.RestApi
{
  public static class WebhookApiHelper
  {
    private const string className = "WebhookApiHelper";
    private const string WEBHOOK_EVENT_XAPI_KEY_NAME = "x-api-key";
    public const string WEBHOOK_EVENT_RESOURCENAME_EXTERNALORG = "externalOrganization";
    public const string WEBHOOK_EXTERNALORGANIZATION_RESOURCENAME = "/platform/v1/events/externalorganization/";
    public const string WEBHOOK_EVENT_SOURCE = "urn:elli:encompass:";

    public static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public static void PublishExternalOrgWebhookEvent(
      string userID,
      string modifiedEntityType,
      int organizationId,
      bool lender = false,
      ExternalCompanyEventType eventType = ExternalCompanyEventType.Update)
    {
      try
      {
        ExternalOriginatorManagementData byoid = Session.DefaultInstance.ConfigurationManager.GetByoid(lender, organizationId);
        if (byoid == null)
        {
          RemoteLogger.Write(TraceLevel.Warning, string.Format("No external organization found for the id - {0} userId -{1}. Skipping externalOrganization settings webhook event publishing.", (object) organizationId, (object) userID));
        }
        else
        {
          WebookEventContract eventContract = new WebookEventContract(userID, eventType.ToString(), organizationId.ToString());
          eventContract.AddExpoObject("organizationName", (object) byoid.OrganizationName);
          eventContract.AddExpoObject("settingsGroup", (object) modifiedEntityType);
          eventContract.AddExpoObject("tpoId", (object) byoid.ExternalID);
          WebhookApiHelper.Publish(eventContract);
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (WebhookApiHelper), "Exception while publishing externalOrganization settings webhook event message", ex);
      }
    }

    public static void Publish(WebookEventContract eventContract)
    {
      HttpResponseMessage result = RestApiProxyFactory.CreateOAPIGatewayRestApiWebhookProxy(WebhookApiHelper.SessionId, "application/json", Session.StartupInfo?.WebhookEventXapiKey, "x-api-key").PostAsync("/platform/v1/events/externalorganization/", (HttpContent) new StringContent(JsonConvert.SerializeObject((object) eventContract, new JsonSerializerSettings()
      {
        ContractResolver = (IContractResolver) new CamelCasePropertyNamesContractResolver()
      }), Encoding.UTF8, "application/json")).Result;
      if (result != null && (result == null || result.IsSuccessStatusCode))
        return;
      RemoteLogger.Write(TraceLevel.Error, string.Format("Publishing externalOrganization settings webhook event failed with a responsecode - {0} response message - {1} for the contract - {2}", (object) result.StatusCode, (object) result.ReasonPhrase, (object) JsonConvert.SerializeObject((object) eventContract)));
    }
  }
}
