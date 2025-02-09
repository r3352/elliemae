// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EnhancedConditions.EnhancedConditionsRestClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.eFolder;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.EnhancedConditions
{
  public class EnhancedConditionsRestClient
  {
    private const string className = "EnhancedConditionsRestClient�";
    private static readonly string sw = Tracing.SwEFolder;
    public const string EnhancedConditionsBasePath = "/encompass/v3/settings/loan/conditions/�";
    public const string EnhancedConditionsTypeGet = "/encompass/v3/settings/loan/conditions/types?context=loanConditions�";
    public const string EnhancedConditionsTypeGetById = "/encompass/v3/settings/loan/conditions/types/ID?context=loanConditions�";
    public const string EnhancedConditionTemplateGet = "/encompass/v3/settings/loan/conditions/templates?context=loanConditions&activeOnly=true&view={0}�";
    private const string scope = "sc�";
    private static HttpClient _httpClient;
    private static string _sessionId;
    private string _oapiBaseUri;
    private string authToken;
    private string scheme;
    private LoanDataMgr loanDataMgr;

    public EnhancedConditionsRestClient(LoanDataMgr loanDataMgr, RetrySettings settings = null)
    {
      this.loanDataMgr = loanDataMgr;
      EnhancedConditionsRestClient._sessionId = loanDataMgr.SessionObjects.SessionID;
      this._oapiBaseUri = loanDataMgr.SessionObjects.StartupInfo.OAPIGatewayBaseUri;
      this.SetAccessToken(loanDataMgr.SessionObjects, loanDataMgr.SessionObjects.StartupInfo.ServerInstanceName);
      EnhancedConditionsRestClient._httpClient = this.GetHttpClient();
    }

    public EnhancedConditionType[] GetEnhancedConditionTypes(bool activeOnly, bool includeDetail)
    {
      EnhancedConditionType[] enhancedConditionTypes1 = (EnhancedConditionType[]) null;
      try
      {
        Task<ApiResponse> enhancedConditionTypes2 = this.getEnhancedConditionTypes(activeOnly, includeDetail ? "detail" : "summary");
        Task.WaitAll((Task) enhancedConditionTypes2);
        enhancedConditionTypes1 = JsonConvert.DeserializeObject<EnhancedConditionType[]>(enhancedConditionTypes2.Result.ResponseString);
      }
      catch (Exception ex)
      {
        Tracing.Log(EnhancedConditionsRestClient.sw, nameof (EnhancedConditionsRestClient), TraceLevel.Error, ex.ToString());
      }
      return enhancedConditionTypes1;
    }

    public EnhancedConditionTemplate[] GetEnhancedConditionTemplates(bool includeDetail)
    {
      EnhancedConditionTemplate[] conditionTemplates1 = (EnhancedConditionTemplate[]) null;
      try
      {
        Task<ApiResponse> conditionTemplates2 = this.getEnhancedConditionTemplates(includeDetail ? "detail" : "summary");
        Task.WaitAll((Task) conditionTemplates2);
        conditionTemplates1 = JsonConvert.DeserializeObject<EnhancedConditionTemplate[]>(conditionTemplates2.Result.ResponseString);
      }
      catch (Exception ex)
      {
        Tracing.Log(EnhancedConditionsRestClient.sw, nameof (EnhancedConditionsRestClient), TraceLevel.Error, ex.ToString());
      }
      return conditionTemplates1;
    }

    public EnhancedConditionType GetEnhancedConditionTypeDetails(string conditionId)
    {
      try
      {
        Task<ApiResponse> conditionTypeById = this.getConditionTypeById(conditionId);
        Task.WaitAll((Task) conditionTypeById);
        return JsonConvert.DeserializeObject<EnhancedConditionType>(conditionTypeById.Result.ResponseString);
      }
      catch (Exception ex)
      {
        Tracing.Log(EnhancedConditionsRestClient.sw, nameof (EnhancedConditionsRestClient), TraceLevel.Error, ex.ToString());
        return (EnhancedConditionType) null;
      }
    }

    private async Task<ApiResponse> getEnhancedConditionTypes(bool activeOnly, string view = "summary�")
    {
      Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "Entering GetEnhancedConditionTypes");
      string str = "/encompass/v3/settings/loan/conditions/types?context=loanConditions";
      string requestUri = !activeOnly ? str + "&activeOnly=false&view=" + view : str + "&activeOnly=true&view=" + view;
      return await this.ValidateResponseMessage(await EnhancedConditionsRestClient._httpClient.GetAsync(requestUri).ConfigureAwait(false));
    }

    private async Task<ApiResponse> getConditionTypeById(string Id)
    {
      Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "Entering GetConditionTypeId");
      string requestUri = "/encompass/v3/settings/loan/conditions/types/ID?context=loanConditions".Replace("ID", Id);
      return await this.ValidateResponseMessage(await EnhancedConditionsRestClient._httpClient.GetAsync(requestUri).ConfigureAwait(false));
    }

    private async Task<ApiResponse> getEnhancedConditionTemplates(string view = "summary�")
    {
      Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "Entering GetEnhancedConditionTemplates");
      HttpResponseMessage response = (HttpResponseMessage) null;
      try
      {
        string requestUri = string.Format("/encompass/v3/settings/loan/conditions/templates?context=loanConditions&activeOnly=true&view={0}", (object) view);
        response = await EnhancedConditionsRestClient._httpClient.GetAsync(requestUri).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EnhancedConditionsRestClient: Error in GetEnhancedConditionTemplates. Ex: " + (object) ex);
      }
      return await this.ValidateResponseMessage(response);
    }

    private HttpClient GetHttpClient()
    {
      HttpClient httpClient = new HttpClient()
      {
        BaseAddress = new Uri(this._oapiBaseUri)
      };
      httpClient.DefaultRequestHeaders.Accept.Clear();
      httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.scheme, this.authToken);
      return httpClient;
    }

    private void SetAccessToken(SessionObjects sessionObjects, string instanceId)
    {
      string typeAndToken = new OAuth2(this._oapiBaseUri, new RetrySettings(sessionObjects), CacheItemRetentionPolicy.NoRetention).GetAccessToken(instanceId, EnhancedConditionsRestClient._sessionId, "sc").TypeAndToken;
      this.scheme = typeAndToken.Split(' ')[0];
      this.authToken = typeAndToken.Split(' ')[1];
    }

    private async Task<ApiResponse> ValidateResponseMessage(HttpResponseMessage response)
    {
      Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
      ApiResponse enhanceConditionApiResponse = new ApiResponse();
      enhanceConditionApiResponse.StatusCode = response.StatusCode;
      if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
      {
        string str = await response.Content.ReadAsStringAsync();
        Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "SendAsync Response Content: " + str);
        enhanceConditionApiResponse.ResponseString = str;
        return enhanceConditionApiResponse;
      }
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        throw new HttpException((int) response.StatusCode, "Unauthorized Request");
      string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
      string str1 = await response.Content.ReadAsStringAsync();
      Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "SendAsync Response Content: " + str1);
      try
      {
        EnhanceConditionError enhanceConditionError = JsonConvert.DeserializeObject<EnhanceConditionError>(str1);
        message = message + " " + enhanceConditionError.code + " " + enhanceConditionError.summary + " " + enhanceConditionError.details;
      }
      catch (Exception ex)
      {
        Tracing.Log(EnhancedConditionsRestClient.sw, TraceLevel.Verbose, nameof (EnhancedConditionsRestClient), "failed in DeserializeObject Enhance Condition Response Content: " + ex.Message);
      }
      IEnumerable<string> values;
      response.Headers.TryGetValues("X-Correlation-ID", out values);
      if (values != null)
        message = message + " CorrelationID=" + values.FirstOrDefault<string>();
      throw new HttpException((int) response.StatusCode, message);
    }

    private HttpRequestMessage getHttpRequestMessage(string url, string method)
    {
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod(method), this._oapiBaseUri + url);
      httpRequestMessage.Headers.Add("Authorization", this.scheme + " " + this.authToken);
      httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      return httpRequestMessage;
    }
  }
}
