// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Authentication.ReauthenticateOnUnauthorised
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Authentication
{
  public class ReauthenticateOnUnauthorised
  {
    private static readonly string sw = Tracing.SwEFolder;
    private const string ClassName = "ReauthenticateOnUnauthorised�";
    private readonly string _oapiGatewayBaseUri;
    private readonly string _instanceName;
    private readonly string _sessionID;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _audience;
    private readonly string _companyId;
    private readonly string _userId;
    private readonly RetrySettings _retrySettings;

    private ReauthenticateOnUnauthorised()
    {
    }

    public ReauthenticateOnUnauthorised(
      string instanceName,
      string sessionID,
      string oapiGatewayBaseUri,
      RetrySettings settings)
    {
      this._oapiGatewayBaseUri = oapiGatewayBaseUri;
      this._instanceName = instanceName;
      this._sessionID = sessionID;
      this._retrySettings = settings;
    }

    public ReauthenticateOnUnauthorised(
      string oapiGatewayBaseUri,
      string audience,
      string instanceName,
      string companyId,
      string userId,
      RetrySettings settings)
    {
      this._instanceName = instanceName;
      this._oapiGatewayBaseUri = oapiGatewayBaseUri;
      this._audience = audience;
      this._companyId = companyId;
      this._retrySettings = settings;
    }

    public ReauthenticateOnUnauthorised(
      string oapiGatewayBaseUri,
      string clientId,
      string clientSecret,
      string audience,
      string instanceName,
      string companyId,
      string userId,
      RetrySettings settings)
    {
      this._oapiGatewayBaseUri = oapiGatewayBaseUri;
      this._clientId = clientId;
      this._clientSecret = clientSecret;
      this._audience = audience;
      this._instanceName = instanceName;
      this._companyId = companyId;
      this._userId = userId;
      this._retrySettings = settings;
    }

    public void Execute(Action<AccessToken> work)
    {
      this.Execute((string) null, (string) null, work);
    }

    public void Execute(string scope, Action<AccessToken> work)
    {
      this.Execute((string) null, scope, work);
    }

    public void Execute(string ServiceUrl, string scope, Action<AccessToken> work)
    {
      bool reauthoriseAttempt = false;
      while (true)
      {
        try
        {
          AccessToken accessToken = this.PerformReauthorizationWork((string) null, scope, reauthoriseAttempt);
          work(accessToken);
          break;
        }
        catch (WebException ex)
        {
          if (!reauthoriseAttempt && ((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized)
          {
            reauthoriseAttempt = true;
            Tracing.Log(ReauthenticateOnUnauthorised.sw, TraceLevel.Error, nameof (ReauthenticateOnUnauthorised), "Token is expired. Getting the fresh token. " + (object) ex);
          }
          else
            throw;
        }
        catch (HttpException ex)
        {
          if (!reauthoriseAttempt && ex.GetHttpCode() == 401)
          {
            reauthoriseAttempt = true;
            Tracing.Log(ReauthenticateOnUnauthorised.sw, TraceLevel.Error, nameof (ReauthenticateOnUnauthorised), "Token is expired. Getting the fresh token. " + (object) ex);
          }
          else
            throw;
        }
      }
    }

    public async Task ExecuteAsync(Func<AccessToken, Task> work)
    {
      await this.ExecuteAsync((string) null, (string) null, work);
    }

    public async Task ExecuteAsync(string serviceUrl, string scope, Func<AccessToken, Task> work)
    {
      bool reauthoriseAttempt = false;
      while (true)
      {
        try
        {
          await work(this.PerformReauthorizationWork(serviceUrl, scope, reauthoriseAttempt)).ConfigureAwait(false);
          break;
        }
        catch (WebException ex)
        {
          if (!reauthoriseAttempt && ((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.Unauthorized)
          {
            reauthoriseAttempt = true;
            Tracing.Log(ReauthenticateOnUnauthorised.sw, TraceLevel.Error, nameof (ReauthenticateOnUnauthorised), "Token is expired. Getting the fresh token. " + (object) ex);
          }
          else
            throw;
        }
        catch (HttpException ex)
        {
          if (!reauthoriseAttempt && ex.GetHttpCode() == 401)
          {
            reauthoriseAttempt = true;
            Tracing.Log(ReauthenticateOnUnauthorised.sw, TraceLevel.Error, nameof (ReauthenticateOnUnauthorised), "Token is expired. Getting the fresh token. " + (object) ex);
          }
          else
            throw;
        }
      }
    }

    private AccessToken PerformReauthorizationWork(
      string serviceUrl,
      string scope,
      bool reauthoriseAttempt)
    {
      CacheItemRetentionPolicy cacheItemRetentionPolicy = string.IsNullOrEmpty(this._sessionID) ? CacheItemRetentionPolicy.ExpireIn5Mins : CacheItemRetentionPolicy.ExpireIn2Hours;
      OAuth2 oauth2 = string.IsNullOrEmpty(this._clientId) || string.IsNullOrEmpty(this._clientSecret) ? new OAuth2(this._oapiGatewayBaseUri, this._retrySettings, cacheItemRetentionPolicy) : new OAuth2(this._oapiGatewayBaseUri, this._clientId, this._clientSecret, this._retrySettings, cacheItemRetentionPolicy);
      OAuth2.AuthToken authToken = string.IsNullOrEmpty(this._sessionID) ? oauth2.GetAccessTokenForS2S(this._instanceName, this._audience, scope, this._companyId, this._userId, reauthoriseAttempt) : oauth2.GetAccessToken(this._instanceName, this._sessionID, scope, reauthoriseAttempt);
      AccessToken accessToken = new AccessToken()
      {
        Token = authToken.access_token,
        Type = authToken.token_type,
        HostName = authToken.host_name
      };
      if (!string.IsNullOrEmpty(serviceUrl))
      {
        serviceUrl = serviceUrl.Replace("{host}", accessToken.HostName);
        ServicePointManager.FindServicePoint(new Uri(serviceUrl)).ConnectionLeaseTimeout = 60000;
        accessToken.ServiceUrl = serviceUrl;
      }
      return accessToken;
    }
  }
}
