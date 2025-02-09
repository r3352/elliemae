// Decompiled with JetBrains decompiler
// Type: RestApiProxy.RestApiProxyFactory
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using EllieMae.EMLite;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.eFolder;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace RestApiProxy
{
  public class RestApiProxyFactory
  {
    private static readonly string sw = Tracing.SwEFolder;

    public static HttpClient CreateRestProxy(
      string sessionId,
      string userName,
      string mediaConentType = "json/application")
    {
      return new DirectApiCallRestProxy(sessionId, userName, mediaConentType).GetHttpClient();
    }

    public static HttpClient CreateOsbHttpClientProxy(string sessionId, string mediaConentType = "json/application")
    {
      return new OsbRestProxy(sessionId, mediaConentType).GetHttpClient();
    }

    public static HttpClient CreateGatewayApiHttpClientProxy(
      string sessionId,
      string mediaConentType = "json/application")
    {
      return new GatewayApiRestProxy(sessionId, mediaConentType).GetHttpClient();
    }

    public static HttpClient CreateOAPIGatewayRestApiProxy(
      string sessionId,
      string mediaContentType = "json/application")
    {
      return new OAPIGatewayRestApiProxy(sessionId, mediaContentType).GetHttpClient();
    }

    public static HttpClient CreateOAPIGatewayRestApiProxy(
      SessionObjects sessionObjects,
      string mediaContentType = "json/application")
    {
      return new OAPIGatewayRestApiProxy(sessionObjects, mediaContentType).GetHttpClient();
    }

    public static HttpClient CreateRestApiProxy(
      string sessionId,
      bool useOAuthUrl,
      string mediaContentType = "json/application")
    {
      return new OAPIGatewayRestApiProxy(sessionId, mediaContentType, useOAuthUrl).GetHttpClient();
    }

    public static HttpClient CreateOAPIGatewayRestApiWebhookProxy(
      string sessionId,
      string mediaContentType,
      string token,
      string scheme)
    {
      return new OAPIGatewayRestApiProxy(sessionId, mediaContentType, token, scheme).GetHttpClient();
    }

    public static HttpClient CreateOAPIGatewayRestApiProxy(
      string sessionId,
      string mediaContentType,
      string scheme)
    {
      return new OAPIGatewayRestApiProxy(sessionId, mediaContentType, scheme).GetHttpClient();
    }

    public static HttpRequestMessage GetHttpRequestMessage(
      string sessionId,
      string url,
      string httpMethod = "PATCH",
      string mediaConentType = "json/application")
    {
      return new OAPIGatewayRestApiProxy(sessionId, mediaConentType).GetHttpRequestMessage(url, httpMethod);
    }

    public async Task<string> PatchCallToAPIAsync(
      string sessionId,
      string url,
      object conditionType)
    {
      string empty = string.Empty;
      HttpClient httpClient = new HttpClient();
      string apiAsync;
      using (HttpRequestMessage request = RestApiProxyFactory.GetHttpRequestMessage(sessionId, url))
      {
        // ISSUE: reference to a compiler-generated field
        if (RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (RestApiProxyFactory)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, JsonSerializerSettings, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (RestApiProxyFactory), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__0.Target((CallSite) RestApiProxyFactory.\u003C\u003Eo__11.\u003C\u003Ep__0, typeof (JsonConvert), conditionType, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore
        });
        string content = target((CallSite) p1, obj);
        if (!string.IsNullOrEmpty(content))
        {
          if (!content.Trim().StartsWith("["))
            content = "[" + content + "]";
          request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
        }
        Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "Calling SendAsync: " + request.RequestUri.ToString());
        using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
        {
          Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response StatusCode: " + (object) response.StatusCode);
          if (response.IsSuccessStatusCode)
          {
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response Content: " + str);
            apiAsync = str;
          }
          else
          {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
              throw new HttpException((int) response.StatusCode, "Unauthorized Request");
            string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response Content: " + str);
            try
            {
              EnhanceConditionError enhanceConditionError = JsonConvert.DeserializeObject<EnhanceConditionError>(str);
              message = message + " " + enhanceConditionError.code + " " + enhanceConditionError.summary + " " + enhanceConditionError.details;
            }
            catch (Exception ex)
            {
              Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "failed in DeserializeObject Enhance Condition Response Content: " + ex.Message);
            }
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("X-Correlation-ID", out values))
              message = message + " CorrelationID=" + values.FirstOrDefault<string>();
            throw new HttpException((int) response.StatusCode, message);
          }
        }
      }
      return apiAsync;
    }

    public async Task<string> PutCallToAPIAsync(string sessionId, string url, object conditionType)
    {
      string empty = string.Empty;
      HttpClient httpClient = new HttpClient();
      string apiAsync;
      using (HttpRequestMessage request = RestApiProxyFactory.GetHttpRequestMessage(sessionId, url, "PUT"))
      {
        // ISSUE: reference to a compiler-generated field
        if (RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (RestApiProxyFactory)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, string> target = RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, string>> p1 = RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__0 = CallSite<Func<CallSite, Type, object, JsonSerializerSettings, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "SerializeObject", (IEnumerable<Type>) null, typeof (RestApiProxyFactory), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj = RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__0.Target((CallSite) RestApiProxyFactory.\u003C\u003Eo__12.\u003C\u003Ep__0, typeof (JsonConvert), conditionType, new JsonSerializerSettings()
        {
          NullValueHandling = NullValueHandling.Ignore
        });
        string content = target((CallSite) p1, obj);
        if (!string.IsNullOrEmpty(content))
          request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
        Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "Calling SendAsync: " + request.RequestUri.ToString());
        using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
        {
          Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response StatusCode: " + (object) response.StatusCode);
          if (response.IsSuccessStatusCode)
          {
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response Content: " + str);
            apiAsync = str;
          }
          else
          {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
              throw new HttpException((int) response.StatusCode, "Unauthorized Request");
            string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "SendAsync Response Content: " + str);
            try
            {
              EnhanceConditionError enhanceConditionError = JsonConvert.DeserializeObject<EnhanceConditionError>(str);
              message = message + " " + enhanceConditionError.code + " " + enhanceConditionError.summary + " " + enhanceConditionError.details;
            }
            catch (Exception ex)
            {
              Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "failed in DeserializeObject Enhance Condition Response Content: " + ex.Message);
            }
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("X-Correlation-ID", out values))
              message = message + " CorrelationID=" + values.FirstOrDefault<string>();
            throw new HttpException((int) response.StatusCode, message);
          }
        }
      }
      return apiAsync;
    }

    public static Task<ApiResponse> GetApiCall(string sessionId, string formattedUrl)
    {
      return RestApiProxyFactory.GetApiCall(RestApiProxyFactory.CreateOAPIGatewayRestApiProxy(sessionId, "application/json"), formattedUrl);
    }

    public static async Task<ApiResponse> GetApiCall(HttpClient httpClient, string formattedUrl)
    {
      HttpResponseMessage response = await httpClient.GetAsync(formattedUrl).ConfigureAwait(false);
      IEnumerable<string> values;
      if (response.Headers.TryGetValues("X-Correlation-ID", out values))
        Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "Response correlationID = " + string.Join(", ", values));
      return await RestApiProxyFactory.ValidateResponseMessage(response);
    }

    public static async Task<ApiResponse> PatchAPICall(
      HttpContent upatedContent,
      string sessionId,
      string formattedUrl)
    {
      HttpClient httpClient = new HttpClient();
      HttpRequestMessage httpRequestMessage = RestApiProxyFactory.GetHttpRequestMessage(sessionId, formattedUrl);
      httpRequestMessage.Content = upatedContent;
      HttpRequestMessage request = httpRequestMessage;
      ApiResponse apiResponse;
      using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
        apiResponse = await RestApiProxyFactory.ValidateResponseMessage(response);
      return apiResponse;
    }

    public static async Task<ApiResponse> PatchAPICall(
      HttpContent requestContent,
      string sessionId,
      string formattedUrl,
      CancellationToken cancellationToken)
    {
      HttpClient httpClient = new HttpClient();
      HttpRequestMessage httpRequestMessage = RestApiProxyFactory.GetHttpRequestMessage(sessionId, formattedUrl);
      httpRequestMessage.Content = requestContent;
      HttpRequestMessage request = httpRequestMessage;
      CancellationToken cancellationToken1 = cancellationToken;
      ApiResponse apiResponse;
      using (HttpResponseMessage response = await httpClient.SendAsync(request, cancellationToken1).ConfigureAwait(false))
      {
        cancellationToken.ThrowIfCancellationRequested();
        apiResponse = await RestApiProxyFactory.ValidateResponseMessage(response);
      }
      return apiResponse;
    }

    public static async Task<ApiResponse> PostAPICall(
      HttpContent content,
      string sessionId,
      string formattedUrl)
    {
      HttpClient httpClient = new HttpClient();
      HttpRequestMessage httpRequestMessage = RestApiProxyFactory.GetHttpRequestMessage(sessionId, formattedUrl, "POST");
      httpRequestMessage.Content = content;
      HttpRequestMessage request = httpRequestMessage;
      ApiResponse apiResponse;
      using (HttpResponseMessage response = await httpClient.SendAsync(request).ConfigureAwait(false))
        apiResponse = await RestApiProxyFactory.ValidateResponseMessageWithoutThrowingException(response);
      return apiResponse;
    }

    private static async Task<ApiResponse> ValidateResponseMessage(HttpResponseMessage response)
    {
      string sw1 = RestApiProxyFactory.sw;
      RestApiProxyFactory.LoggerName loggerName = RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper;
      string className1 = loggerName.ToString();
      string msg1 = "SendAsync Response StatusCode: " + (object) response.StatusCode;
      Tracing.Log(sw1, TraceLevel.Verbose, className1, msg1);
      ApiResponse enhanceConditionApiResponse = new ApiResponse();
      enhanceConditionApiResponse.StatusCode = response.StatusCode;
      if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Conflict)
      {
        string str = await response.Content.ReadAsStringAsync();
        string sw2 = RestApiProxyFactory.sw;
        loggerName = RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper;
        string className2 = loggerName.ToString();
        string msg2 = "SendAsync Response Content: " + str;
        Tracing.Log(sw2, TraceLevel.Verbose, className2, msg2);
        enhanceConditionApiResponse.ResponseString = str;
        return enhanceConditionApiResponse;
      }
      if (response.StatusCode == HttpStatusCode.Unauthorized)
        throw new HttpException((int) response.StatusCode, "Unauthorized Request");
      EnhanceConditionError error = (EnhanceConditionError) null;
      string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
      string str1 = await response.Content.ReadAsStringAsync();
      string sw3 = RestApiProxyFactory.sw;
      loggerName = RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper;
      string className3 = loggerName.ToString();
      string msg3 = "SendAsync Response Content: " + str1;
      Tracing.Log(sw3, TraceLevel.Verbose, className3, msg3);
      try
      {
        error = JsonConvert.DeserializeObject<EnhanceConditionError>(str1);
        message = message + " " + error.code + " " + error.summary + " " + error.details;
      }
      catch (Exception ex)
      {
        Tracing.Log(RestApiProxyFactory.sw, TraceLevel.Verbose, RestApiProxyFactory.LoggerName.EnhancedConditionsRestAPIHelper.ToString(), "failed in DeserializeObject Enhance Condition Response Content: " + ex.Message);
      }
      IEnumerable<string> values;
      if (response.Headers.TryGetValues("X-Correlation-ID", out values))
        message = message + " CorrelationID=" + values.FirstOrDefault<string>();
      HttpException httpException = new HttpException((int) response.StatusCode, message);
      httpException.Data.Add((object) "error", (object) error);
      throw httpException;
    }

    private static async Task<ApiResponse> ValidateResponseMessageWithoutThrowingException(
      HttpResponseMessage response)
    {
      ApiResponse apiResponse = new ApiResponse();
      apiResponse.StatusCode = response.StatusCode;
      IEnumerable<string> values;
      if (response.Headers.TryGetValues("X-Correlation-ID", out values))
        apiResponse.CorrelationId = values.FirstOrDefault<string>();
      if (response.IsSuccessStatusCode)
      {
        apiResponse.ResponseString = await response.Content.ReadAsStringAsync();
        return apiResponse;
      }
      if (response.StatusCode == HttpStatusCode.Unauthorized)
      {
        apiResponse.ResponseString = "Unauthorized Request";
      }
      else
      {
        string str = await response.Content.ReadAsStringAsync();
        try
        {
          apiResponse.ResponseString = JsonConvert.DeserializeObject<ParserError>(str).ToString();
        }
        catch (Exception ex)
        {
          apiResponse.ResponseString = "Error in DeserializeObject Response Content: " + ex.Message;
        }
      }
      return apiResponse;
    }

    public enum LoggerName
    {
      EnhancedConditionsRestAPIHelper,
      RestAPIHelper,
    }
  }
}
