// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.EnhancedConditionsTool.RestApiHelper
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Setup;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestApiProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.AdminTools.EnhancedConditionsTool
{
  public class RestApiHelper
  {
    private string _sessionID;
    private string _instanceName;
    private const string className = "RestApiHelper";

    public RestApiHelper(Sessions.Session session)
    {
      this._instanceName = session.ServerIdentity?.InstanceName ?? "LOCALHOST";
      this._sessionID = this._instanceName + "_" + session.SessionID;
    }

    public async Task<IEnumerable<EnhancedConditionType>> GetEnhancedConditionTypes()
    {
      return (IEnumerable<EnhancedConditionType>) JsonConvert.DeserializeObject<EnhancedConditionType[]>((await RestApiProxyFactory.GetApiCall(this._sessionID, "/encompass/v3/settings/loan/conditions/types?activeOnly=false&view=detail"))?.ResponseString);
    }

    public async Task<IEnumerable<EnhancedConditionTemplate>> GetEnhancedConditionTemplates()
    {
      return (IEnumerable<EnhancedConditionTemplate>) JsonConvert.DeserializeObject<EnhancedConditionTemplate[]>((await RestApiProxyFactory.GetApiCall(this._sessionID, string.Format("/encompass/v3/settings/loan/conditions/templates?activeOnly={0}&view={1}", (object) Convert.ToString(false), (object) "detail")))?.ResponseString);
    }

    public async Task<SyncResult> UpsertEnhancedConditionTemplates(
      IList<EnhancedConditionTemplate> templates,
      bool useInsert,
      CancellationToken cancellationToken)
    {
      SyncResult result = new SyncResult()
      {
        ImportCount = 0
      };
      if (templates.Count > 0)
      {
        string formattedUrl = string.Format("/encompass/v3/settings/loan/conditions/templates?action={0}&view={1}", useInsert ? (object) "add" : (object) "update", (object) "id");
        ApiResponse apiResponse = await RestApiProxyFactory.PatchAPICall((HttpContent) new StringContent(JsonConvert.SerializeObject((object) templates, new JsonSerializerSettings()
        {
          ContractResolver = (IContractResolver) new RestApiHelper.EnhancedConditionTemplateResolver()
        }), Encoding.UTF8, "application/json"), this._sessionID, formattedUrl, cancellationToken);
        if (apiResponse != null)
        {
          RestApiHelper.UpsertResponse upsertResponse = JsonConvert.DeserializeObject<RestApiHelper.UpsertResponse>(apiResponse.ResponseString);
          if (HttpStatusCode.Conflict == apiResponse.StatusCode)
          {
            EnhanceConditionError enhanceConditionError = new EnhanceConditionError()
            {
              summary = upsertResponse.Summary,
              details = upsertResponse.Details
            };
            HttpException httpException = new HttpException();
            httpException.Data.Add((object) "error", (object) enhanceConditionError);
            throw httpException;
          }
          result.Status = SyncStatus.Success;
          SyncResult syncResult = result;
          IList<string> id = upsertResponse.Id;
          int count = id != null ? id.Count : 0;
          syncResult.ImportCount = count;
        }
      }
      return result;
    }

    public async Task<ApiResponse> SendHOIStatus(string status)
    {
      RemoteLogger.Write(TraceLevel.Info, "RestApiHelper Entering SendHOIStatus");
      try
      {
        string formattedUrl = "/platform/v1/beacons";
        StringContent content = new StringContent(JsonConvert.SerializeObject((object) new RestApiHelper.BeaconsData()
        {
          originDate = DateTime.UtcNow.ToString(),
          type = "HOI_ENABLEMENT",
          source = ("urn:elli:encompass:" + this._instanceName),
          payload = new RestApiHelper.BeaconPayload()
          {
            value = status,
            provider = "Geico"
          }
        }), Encoding.UTF8, "application/json");
        content.Headers.ContentType.CharSet = string.Empty;
        return await RestApiProxyFactory.PostAPICall((HttpContent) content, this._sessionID, formattedUrl);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "RestApiHelper: Error in SendHOIStatus. Ex: " + (object) ex);
        return (ApiResponse) null;
      }
    }

    internal class BeaconsData
    {
      public string originDate { get; set; }

      public string type { get; set; }

      public string source { get; set; }

      public RestApiHelper.BeaconPayload payload { get; set; }
    }

    internal class BeaconPayload
    {
      public string value { get; set; }

      public string provider { get; set; }
    }

    internal sealed class UpsertResponse
    {
      public string Summary { get; set; }

      public string Details { get; set; }

      public IList<string> Id { get; set; }
    }

    private class EnhancedConditionTemplateResolver : IgnorePropertySerializer
    {
      public EnhancedConditionTemplateResolver()
      {
        this.IgnoreProperty(typeof (EnhancedConditionTemplate), "CreatedBy", "CreatedDate", "LastModifiedBy", "LastModifiedDate");
      }

      protected override JsonProperty CreateProperty(
        MemberInfo member,
        MemberSerialization memberSerialization)
      {
        JsonProperty property = base.CreateProperty(member, memberSerialization);
        property.NullValueHandling = new NullValueHandling?(member.Name == "DaysToReceive" ? NullValueHandling.Include : NullValueHandling.Ignore);
        return property;
      }
    }
  }
}
