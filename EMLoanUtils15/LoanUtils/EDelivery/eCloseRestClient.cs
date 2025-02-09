// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.eCloseRestClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.LoanUtils.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class eCloseRestClient
  {
    private const string className = "eCloseRestClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private string baseURL = string.Empty;
    private const string _scope = "sc�";
    private const string simplifileOrgIdGetUrl = "/collaboration/v1/spaces/simplifile/organization�";
    private static HttpClient _client = new HttpClient();
    private readonly OAuth2Utils _oauth;

    public eCloseRestClient(ISession session, ISessionStartupInfo startupInfo)
    {
      this._oauth = new OAuth2Utils(session, startupInfo);
      this.baseURL = string.IsNullOrEmpty(startupInfo.OAPIGatewayBaseUri) ? EnConfigurationSettings.AppSettings["oAuth.Url"] : startupInfo.OAPIGatewayBaseUri;
    }

    public async Task<eCloseRestClient.SimplifileOrganizationResponse> GetSimplifileOrganization()
    {
      Tracing.Log(eCloseRestClient.sw, TraceLevel.Info, nameof (eCloseRestClient), "Entering GetSimplifileOrganization");
      eCloseRestClient.SimplifileOrganizationResponse simplifileOrganization;
      try
      {
        simplifileOrganization = await this.submitRequest<eCloseRestClient.SimplifileOrganizationResponse>(string.Format(this.baseURL + "/collaboration/v1/spaces/simplifile/organization"), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "eCloseRestClient: Error in GetSimplifileOrganization. Ex: " + (object) ex);
        throw;
      }
      return simplifileOrganization;
    }

    private async Task<T> submitRequest<T>(string url, string method, string content)
    {
      T result = default (T);
      await this._oauth.ExecuteAsync(url, "sc", (Func<AccessToken, Task>) (async AccessToken =>
      {
        using (HttpRequestMessage request = new HttpRequestMessage())
        {
          request.Method = new HttpMethod(method);
          request.RequestUri = new Uri(AccessToken.ServiceUrl);
          request.Headers.Add("Authorization", AccessToken.TypeAndToken);
          request.Headers.Add("Accept", "application/json");
          if (!string.IsNullOrEmpty(content))
            request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
          Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await eCloseRestClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), "SendAsync Response Content: " + str);
              result = JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              if (response.StatusCode == HttpStatusCode.Conflict)
                throw new HttpException((int) response.StatusCode, "Conflict");
              if (response.StatusCode != HttpStatusCode.NotFound)
              {
                string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
                try
                {
                  string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                  Tracing.Log(eCloseRestClient.sw, TraceLevel.Verbose, nameof (eCloseRestClient), "SendAsync Response Content: " + str);
                  EDeliveryRestError edeliveryRestError = JsonConvert.DeserializeObject<EDeliveryRestError>(str);
                  message = message + " " + edeliveryRestError.code + " " + edeliveryRestError.summary + " " + edeliveryRestError.details;
                }
                catch
                {
                }
                IEnumerable<string> values;
                response.Headers.TryGetValues("X-Correlation-ID", out values);
                if (values != null)
                  message = message + " CorrelationID=" + values.FirstOrDefault<string>();
                throw new HttpException((int) response.StatusCode, message);
              }
            }
          }
        }
      })).ConfigureAwait(false);
      return result;
    }

    [Serializable]
    public class SimplifileOrganizationResponse
    {
      public string id { get; set; }

      public string spaceId { get; set; }

      public string name { get; set; }
    }
  }
}
