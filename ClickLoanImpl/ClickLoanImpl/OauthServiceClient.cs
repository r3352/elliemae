// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClickLoanImpl.OauthServiceClient
// Assembly: ClickLoanImpl, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 9549E162-7E74-49E9-BCDA-CB0A69B5F0B5
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClickLoanImpl.dll

using EllieMae.EMLite.ClickLoanWrapperUtil;
using EllieMae.EMLite.ClientServer.Authentication;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.ClickLoanImpl
{
  public class OauthServiceClient
  {
    private static HttpClient client = new HttpClient();
    private const string className = "OauthServiceClient";
    private static TraceSwitch sw = new TraceSwitch("ClickLoanImpl", "ClickLoan oauthservice client class");
    private readonly RetrySettings RetrySettings;

    public OauthServiceClient() => this.RetrySettings = new RetrySettings();

    public LoginContextResponse GenerateToken(
      string authCode,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl)
    {
      int retries = this.RetrySettings.Retries;
      int currentRetry = 0;
      while (currentRetry++ <= retries)
      {
        try
        {
          using (HttpRequestMessage request = new HttpRequestMessage())
          {
            request.Method = new HttpMethod("Post");
            request.RequestUri = new Uri(oauthUrl + "/oauth2/v1/token");
            request.Headers.Add("Accept", "application/x-www-form-urlencoded");
            string content = "grant_type=authorization_code&code=" + authCode + "&client_id=" + clientId + "&redirect_uri=" + redirectUrl + "&scope=" + scope;
            if (!string.IsNullOrEmpty(content))
              request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            using (HttpResponseMessage result = OauthServiceClient.client.SendAsync(request).Result)
            {
              if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<LoginContextResponse>(result.Content.ReadAsStringAsync().Result);
              throw new Exception(result.Content.ReadAsStringAsync().Result);
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(OauthServiceClient.sw, nameof (OauthServiceClient), TraceLevel.Error, "Error while generating token from authode: " + ex.Message);
          if (currentRetry > retries)
            throw ex;
          Thread.Sleep(this.RetrySettings.GetDelay(currentRetry));
        }
      }
      return (LoginContextResponse) null;
    }

    public LoginContextResponse GenerateAuthCodeFromToken(
      string accessToken,
      string clientId,
      string scope,
      string oauthUrl,
      string redirectUrl)
    {
      int retries = this.RetrySettings.Retries;
      int currentRetry = 0;
      while (currentRetry++ <= retries)
      {
        try
        {
          using (HttpRequestMessage request = new HttpRequestMessage())
          {
            request.Method = new HttpMethod("Post");
            request.RequestUri = new Uri(oauthUrl + "/oauth2/v1/token");
            request.Headers.Add("Accept", "application/x-www-form-urlencoded");
            string content = "grant_type=urn:ietf:params:oauth:grant-type:token-exchange&subject_token_type=urn:ietf:params:oauth:token-type:access_token&subject_token=" + accessToken + "&client_id=" + clientId + "&requested_token_type=urn:elli:params:oauth:token-type:authorization_code";
            if (!string.IsNullOrEmpty(content))
              request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            using (HttpResponseMessage result = OauthServiceClient.client.SendAsync(request).Result)
            {
              if (result.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<LoginContextResponse>(result.Content.ReadAsStringAsync().Result);
              throw new Exception(result.Content.ReadAsStringAsync().Result);
            }
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(OauthServiceClient.sw, nameof (OauthServiceClient), TraceLevel.Error, "Error while generating authCode from token: " + ex.Message);
          if (currentRetry > retries)
            throw ex;
          Thread.Sleep(this.RetrySettings.GetDelay(currentRetry));
        }
      }
      return (LoginContextResponse) null;
    }
  }
}
