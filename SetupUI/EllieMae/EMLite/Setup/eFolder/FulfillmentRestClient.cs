// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.eFolder.FulfillmentRestClient
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using RestApiProxy;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Setup.eFolder
{
  public class FulfillmentRestClient
  {
    private const string className = "FulfillmentRestClient";
    private static readonly string sw = Tracing.SwEFolder;
    private string baseURL = string.Empty;
    private const string FulfillmentBasePath = "/fulfillment/";
    private const string FulfillmentSettingsGet = "/fulfillment/v1/settings";

    private static string SessionId
    {
      get
      {
        return (Session.DefaultInstance.ServerIdentity != null ? Session.DefaultInstance.ServerIdentity.InstanceName : "LOCALHOST") + "_" + Session.DefaultInstance.SessionID;
      }
    }

    public FulfillmentRestClient(RetrySettings settings = null)
    {
      this.baseURL = string.IsNullOrEmpty(Session.StartupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : Session.StartupInfo.OAPIGatewayBaseUri;
    }

    public async Task<string> GetFulFillmentSettingIntegrationType()
    {
      Tracing.Log(FulfillmentRestClient.sw, TraceLevel.Verbose, nameof (FulfillmentRestClient), "GetFulFillmentSetting");
      try
      {
        ApiResponse apiCall = await RestApiProxyFactory.GetApiCall(FulfillmentRestClient.SessionId, this.baseURL + "/fulfillment/v1/settings");
        return apiCall.StatusCode != HttpStatusCode.OK ? "Invalid request" : JsonConvert.DeserializeObject<FulfillmentSetting>(apiCall.ResponseString).Integration.Type;
      }
      catch (Exception ex)
      {
        Tracing.Log(FulfillmentRestClient.sw, nameof (FulfillmentRestClient), TraceLevel.Error, ex.ToString());
        return !(ex.InnerException?.GetType() == typeof (HttpException)) || ((HttpException) ex.InnerException).GetHttpCode() != 404 ? "Invalid request:" + ex.Message : (string) null;
      }
    }
  }
}
