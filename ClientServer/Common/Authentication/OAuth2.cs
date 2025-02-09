// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Authentication.OAuth2
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.Common.Authentication
{
  public class OAuth2
  {
    private string ClassName = nameof (OAuth2);
    private readonly string _sw = Tracing.SwCommon;
    private readonly string AppClientSecret = EnConfigurationSettings.AppSettings["oAuth.ClientSecret"];
    private readonly string AppClientID = EnConfigurationSettings.AppSettings["oAuth.ClientId"];
    private readonly string ClientID = "smartclient";
    private readonly string ClientSecret = "nJWEYegDJk1iRIWvK9BL9Nfk6o5t7criMvCUgVDqHqNP9TgbL6tHhmgjr5St7Sgm";
    private readonly string GrantTypeBearer = "urn:elli:params:oauth:grant-type:encompass-bearer";
    private readonly string GrantTypeS2S = "client_credentials";
    private readonly string GrantTypeAuthCode = "authorization_code";
    private readonly string GrantTypePassword = "password";
    private readonly string GrantTypeTokenExchange = "urn:ietf:params:oauth:grant-type:token-exchange";
    private readonly string SubjectTokenTypeAccessToken = "urn:ietf:params:oauth:token-type:access_token";
    private readonly string RequestedTokenTypeAuthCode = "urn:elli:params:oauth:token-type:authorization_code";
    private readonly string oAuthBaseUrl = string.Empty;
    private CacheItemRetentionPolicy CacheItemRetentionPolicy;
    private readonly RetrySettings RetrySettings;

    public OAuth2(string oauthBaseUrl, CacheItemRetentionPolicy cacheItemRetentionPolicy = null)
      : this(oauthBaseUrl, new RetrySettings(), cacheItemRetentionPolicy)
    {
    }

    public OAuth2(
      string oauthBaseUrl,
      RetrySettings retrySettings,
      CacheItemRetentionPolicy cacheItemRetentionPolicy = null)
    {
      this.oAuthBaseUrl = oauthBaseUrl;
      this.RetrySettings = retrySettings;
      this.CacheItemRetentionPolicy = cacheItemRetentionPolicy ?? CacheItemRetentionPolicy.ExpireIn2Hours;
    }

    public OAuth2(
      string oauthBaseUrl,
      string clientId,
      string clientSecret,
      CacheItemRetentionPolicy cacheItemRetentionPolicy = null)
      : this(oauthBaseUrl, clientId, clientSecret, new RetrySettings(), cacheItemRetentionPolicy)
    {
    }

    public OAuth2(
      string oauthBaseUrl,
      string clientId,
      string clientSecret,
      RetrySettings retrySettings,
      CacheItemRetentionPolicy cacheItemRetentionPolicy = null)
    {
      this.oAuthBaseUrl = oauthBaseUrl;
      this.ClientID = clientId;
      this.ClientSecret = clientSecret;
      this.RetrySettings = retrySettings;
      this.CacheItemRetentionPolicy = cacheItemRetentionPolicy ?? CacheItemRetentionPolicy.ExpireIn2Hours;
    }

    public string oAuthUrl => this.oAuthBaseUrl + "/oauth2/v1/token";

    public OAuth2.AuthToken GetAccessToken(
      string instanceName,
      string sessionId,
      string scope,
      bool noCache = false)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GetAccessToken");
      try
      {
        ICache simpleCache = CacheManager.GetSimpleCache("AccessTokenCache");
        string key = string.Format("{0}_{1}_{2}", (object) instanceName, (object) sessionId, (object) scope);
        if (!noCache)
        {
          OAuth2.AuthToken accessToken = (OAuth2.AuthToken) simpleCache.Get(key);
          if (accessToken != null)
          {
            Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Reading the AccessToken from cache. Key: " + key + " Cached Value: " + accessToken.TypeAndToken);
            return accessToken;
          }
        }
        OAuth2.AuthToken accessToken1 = JsonConvert.DeserializeObject<OAuth2.AuthToken>(this.submitRequest("client_id=" + this.ClientID + "&client_secret=" + this.ClientSecret + "&grant_type=" + this.GrantTypeBearer + "&session=" + (string.IsNullOrEmpty(instanceName) ? "Local" : instanceName + "_" + sessionId) + "&scope=" + scope));
        accessToken1.host_name = new UriBuilder(this.oAuthUrl).Host;
        Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Fetched new access token. Key: " + key + " Cached Value: " + accessToken1.TypeAndToken + " noCache : " + (noCache ? "True" : "False"));
        simpleCache.Put(key, new CacheItem((object) accessToken1, this.CacheItemRetentionPolicy, (ICacheItemExpirationBehavior) new AccessTokenCacheItemExpirationBehavior(this.ClientSecret, this.oAuthBaseUrl)));
        return accessToken1;
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in GetAccessToken. Ex: " + (object) ex);
        throw;
      }
    }

    public OAuth2.AuthToken GetAccessTokenForS2S(
      string instanceName,
      string audience,
      string scope,
      string companyId,
      string userId,
      bool noCache = false)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GetAccessTokenForS2S");
      try
      {
        ICache simpleCache = CacheManager.GetSimpleCache("AccessTokenCache");
        string key = string.Format("{0}_{1}_{2}_{3}_{4}", (object) instanceName, (object) audience, (object) scope, (object) companyId, (object) userId);
        if (!noCache)
        {
          OAuth2.AuthToken accessTokenForS2S = (OAuth2.AuthToken) simpleCache.Get(key);
          if (accessTokenForS2S != null)
          {
            Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Reading the AccessToken from cache. Key :  " + key + "Cached Value : " + accessTokenForS2S.TypeAndToken);
            return accessTokenForS2S;
          }
        }
        string postData = "client_id=" + this.ClientID + "&client_secret=" + this.ClientSecret + "&grant_type=" + this.GrantTypeS2S + "&scope=" + scope + "&aud=" + audience;
        if (!string.IsNullOrEmpty(instanceName))
          postData = postData + "&elli_iid=" + instanceName;
        if (!string.IsNullOrEmpty(companyId))
          postData = postData + "&elli_cid=" + companyId;
        if (!string.IsNullOrEmpty(userId))
          postData = postData + "&elli_uid=" + HttpUtility.UrlEncode("Encompass\\" + instanceName + "\\" + userId);
        OAuth2.AuthToken accessTokenForS2S1 = JsonConvert.DeserializeObject<OAuth2.AuthToken>(this.submitRequest(postData));
        accessTokenForS2S1.host_name = new UriBuilder(this.oAuthUrl).Host;
        Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Fetched new access token. Key :  " + key + "Cached Value : " + accessTokenForS2S1.TypeAndToken + "noCache : " + (noCache ? "True" : "False"));
        simpleCache.Put(key, new CacheItem((object) accessTokenForS2S1, this.CacheItemRetentionPolicy, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior()));
        return accessTokenForS2S1;
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in GetAccessTokenForS2S. Ex: " + (object) ex);
        throw;
      }
    }

    public OAuth2.AuthToken GetAccessTokenFromAuthCode(string authCode, string redirectUri)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GetAccessTokenFromAuthCode");
      try
      {
        OAuth2.AuthToken tokenFromAuthCode = JsonConvert.DeserializeObject<OAuth2.AuthToken>(this.submitRequest("grant_type=" + this.GrantTypeAuthCode + "&redirect_uri=" + HttpUtility.UrlEncode(redirectUri) + "&code=" + authCode + "&client_id=" + this.AppClientID + "&client_secret=" + this.AppClientSecret + "&scope=sc"));
        tokenFromAuthCode.host_name = new UriBuilder(this.oAuthUrl).Host;
        return tokenFromAuthCode;
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in GetAccessTokenFromAuthCode. Ex: " + (object) ex);
        throw;
      }
    }

    public string GetAuthCodeFromToken(
      string instanceId,
      string userId,
      string password,
      string scope)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GetAuthCodeFromToken");
      try
      {
        ICache simpleCache = CacheManager.GetSimpleCache("AccessTokenCache");
        string key = string.Format("{0}_{1}_{2}_{3}", (object) userId, (object) instanceId, (object) this.ClientID, (object) scope);
        OAuth2.AuthToken authToken = (OAuth2.AuthToken) simpleCache.Get(key);
        if (authToken == null)
        {
          authToken = JsonConvert.DeserializeObject<OAuth2.AuthToken>(this.submitRequest("client_id=" + this.ClientID + "&client_secret=" + this.ClientSecret + "&grant_type=" + this.GrantTypePassword + "&username=" + string.Format("{0}@encompass:{1}", (object) userId, (object) instanceId) + "&password=" + password + "&scope=" + scope));
          authToken.host_name = new UriBuilder(this.oAuthUrl).Host;
          Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Fetched new access token. Key :  " + key + "Cached Value : " + authToken.TypeAndToken);
          simpleCache.Put(key, new CacheItem((object) authToken, this.CacheItemRetentionPolicy, (ICacheItemExpirationBehavior) new AccessTokenCacheItemExpirationBehavior(this.ClientSecret, this.oAuthBaseUrl)));
        }
        else
          Tracing.Log(this._sw, TraceLevel.Info, this.ClassName, "Reading the AccessToken from cache. Key :  " + key + "Cached Value : " + authToken.TypeAndToken);
        return this.getAuthCodeFromToken(authToken.access_token);
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in GetAccessToken. Ex: " + (object) ex);
        throw;
      }
    }

    public string GenerateGuestApplicationAuthCode(
      string instanceName,
      string sessionId,
      string guestClientID)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GenerateGuestApplicationAuthCode");
      try
      {
        OAuth2.AuthToken accessToken = this.GetAccessToken(instanceName, sessionId, "sc");
        string str = this.submitRequest("client_id=" + guestClientID + "&grant_type=" + this.GrantTypeTokenExchange + "&subject_token_type=" + this.SubjectTokenTypeAccessToken + "&subject_token=" + accessToken.access_token + "&requested_token_type=" + this.RequestedTokenTypeAuthCode);
        if (str != null)
        {
          object obj1 = JsonConvert.DeserializeObject(str);
          // ISSUE: reference to a compiler-generated field
          if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, bool> target1 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, bool>> p4 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__4;
          // ISSUE: reference to a compiler-generated field
          if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.Not, typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target2 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__3.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p3 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__3;
          // ISSUE: reference to a compiler-generated field
          if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__2 = CallSite<Func<CallSite, Type, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "IsNullOrWhiteSpace", (IEnumerable<Type>) null, typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, object, object> target3 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__2.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, object, object>> p2 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__2;
          Type type = typeof (string);
          // ISSUE: reference to a compiler-generated field
          if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__1 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object> target4 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__1.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object>> p1 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__1;
          // ISSUE: reference to a compiler-generated field
          if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__0 == null)
          {
            // ISSUE: reference to a compiler-generated field
            OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "authorization_code", typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__0.Target((CallSite) OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__0, obj1);
          object obj3 = target4((CallSite) p1, obj2);
          object obj4 = target3((CallSite) p2, type, obj3);
          object obj5 = target2((CallSite) p3, obj4);
          if (target1((CallSite) p4, obj5))
          {
            // ISSUE: reference to a compiler-generated field
            if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__7 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (string), typeof (OAuth2)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, string> target5 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__7.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, string>> p7 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__7;
            // ISSUE: reference to a compiler-generated field
            if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__6 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__6 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Value", typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, object> target6 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__6.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, object>> p6 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__6;
            // ISSUE: reference to a compiler-generated field
            if (OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "authorization_code", typeof (OAuth2), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj6 = OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__5.Target((CallSite) OAuth2.\u003C\u003Eo__26.\u003C\u003Ep__5, obj1);
            object obj7 = target6((CallSite) p6, obj6);
            return target5((CallSite) p7, obj7);
          }
        }
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, string.Format("InstanceID - {0} Guest Application AuthCode generated is null or invalid:", (object) instanceName));
        return (string) null;
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, string.Format("InstanceID - {0} - Error in GenerateGuestApplicationAuthCode.Ex: {1} ", (object) instanceName, (object) ex.StackTrace));
        throw;
      }
    }

    public string GetRestrictedSSOEnabledResponse(
      string instanceName,
      string sessionId,
      string scope = "sc�")
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering GetRestrictedSSOEnabledResponse");
      int retries = this.RetrySettings.Retries;
      int currentRetry = 0;
      while (currentRetry++ <= retries)
      {
        Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Trying to submit request: Attempt #" + (object) currentRetry);
        try
        {
          OAuth2.AuthToken accessToken = this.GetAccessToken(instanceName, sessionId, "sc");
          HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this.oAuthBaseUrl + "/saml2/v2/connections?isActive=true&applicationName=sc");
          httpWebRequest.Method = "GET";
          httpWebRequest.Headers.Add("Authorization", accessToken.TypeAndToken);
          httpWebRequest.ContentType = "application/json";
          HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
          if (response != null && response.StatusCode == HttpStatusCode.OK)
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              return streamReader.ReadToEnd();
          }
          else
          {
            Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "GetRestrictedSSOEnabledResponse - api returned with response code - " + (object) response.StatusCode);
            return (string) null;
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in GetRestrictedSSOEnabledResponse submit request. Ex: " + (object) ex);
          if (currentRetry > retries)
            throw new OAuth2Exception(ex.GetType().ToString(), ex.Message);
          Thread.Sleep(this.RetrySettings.GetDelay(currentRetry));
        }
      }
      return (string) null;
    }

    private string getAuthCodeFromToken(string token)
    {
      Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Entering getAuthCodeFromToken");
      try
      {
        return this.submitRequest("client_id=" + this.ClientID + "&client_secret=" + this.ClientSecret + "&grant_type=" + this.GrantTypeTokenExchange + "&subject_token_type=" + this.SubjectTokenTypeAccessToken + "&subject_token=" + token + "&requested_token_type=" + this.RequestedTokenTypeAuthCode);
      }
      catch (Exception ex)
      {
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in getAuthCodeFromToken. Ex: " + (object) ex);
        throw;
      }
    }

    private string submitRequest(string postData)
    {
      int retries = this.RetrySettings.Retries;
      int currentRetry = 0;
      while (currentRetry++ <= retries)
      {
        Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Trying to submit request: Attempt #" + (object) currentRetry);
        try
        {
          byte[] bytes = Encoding.UTF8.GetBytes(postData);
          HttpWebRequest http = WebRequest.CreateHttp(this.oAuthUrl);
          http.Method = "POST";
          http.ContentType = "application/x-www-form-urlencoded";
          http.ContentLength = (long) bytes.Length;
          http.Timeout = this.RetrySettings.TimeOut;
          http.ServicePoint.Expect100Continue = false;
          Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Calling HttpWebRequest.GetRequestStream");
          using (Stream requestStream = http.GetRequestStream())
          {
            requestStream.Write(bytes, 0, bytes.Length);
            Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Calling HttpWebRequest.GetResponse");
            using (HttpWebResponse response = (HttpWebResponse) http.GetResponse())
            {
              Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Calling WebResponse.GetResponse");
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                string end = streamReader.ReadToEnd();
                Tracing.Log(this._sw, TraceLevel.Verbose, this.ClassName, "Response Content: " + end);
                return end;
              }
            }
          }
        }
        catch (Exception ex)
        {
          if (ex.GetType() == typeof (WebException))
            this.LogWebException(ex);
          else
            Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "Error in submitRequest. Ex: " + (object) ex);
          if (currentRetry > retries)
            throw new OAuth2Exception(ex.GetType().ToString(), ex.Message);
          Thread.Sleep(this.RetrySettings.GetDelay(currentRetry));
        }
      }
      return (string) null;
    }

    private void LogWebException(Exception ex)
    {
      try
      {
        WebException webException = ex as WebException;
        Tracing.Log(this._sw, TraceLevel.Error, this.ClassName, "WebException in submitRequest. Ex: " + JsonConvert.DeserializeObject(new StreamReader(webException.Response.GetResponseStream()).ReadToEnd()) + " CorrelationId :" + (webException.Response == null || webException.Response.Headers == null || !webException.Response.Headers.HasKeys() ? string.Empty : webException.Response.Headers["X-Correlation-ID"]));
      }
      catch
      {
      }
    }

    public class AuthToken
    {
      public string access_token { get; set; }

      public string token_type { get; set; }

      public string host_name { get; set; }

      public string TypeAndToken => this.token_type + " " + this.access_token;

      public int expires_in { get; set; }

      public override string ToString() => this.TypeAndToken;
    }

    public class AuthCode
    {
      public string authorization_code { get; set; }

      public string issued_token_type { get; set; }

      public string token_type { get; set; }

      public int expires_in { get; set; }

      public override string ToString() => this.authorization_code;
    }
  }
}
