// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Authentication.OAuth2Utils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.SimpleCache;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Authentication
{
  public class OAuth2Utils
  {
    private const string className = "OAuth2Utils�";
    private static readonly string sw = Tracing.SwEFolder;
    private readonly ISession _session;
    private readonly ISessionStartupInfo _startupInfo;
    private readonly AccessToken _accessToken;

    public OAuth2Utils(ISession session, ISessionStartupInfo startupInfo)
    {
      this._session = session;
      this._startupInfo = startupInfo;
    }

    public OAuth2Utils(AccessToken accessToken) => this._accessToken = accessToken;

    public AccessToken GetAccessToken(string scope, bool noCache = false)
    {
      Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Entering GetAccessToken");
      try
      {
        if (this._accessToken != null)
          return this._accessToken;
        ICache simpleCache = CacheManager.GetSimpleCache("AccessTokenCache");
        string key = string.Format("{0}_{1}_{2}", (object) this._startupInfo.ServerInstanceName, (object) this._startupInfo.UserInfo.Userid, (object) scope);
        if (!noCache)
        {
          AccessToken accessToken = (AccessToken) simpleCache.Get(key);
          if (accessToken != null)
          {
            Tracing.Log(OAuth2Utils.sw, TraceLevel.Info, nameof (OAuth2Utils), string.Format("Using Cached AccessToken. Key:{0} Token:{1}", (object) key, (object) accessToken.TypeAndToken));
            return accessToken;
          }
        }
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Calling CurrentUser.GetAccessToken: " + scope);
        AccessToken accessToken1 = this._session.GetUser().GetAccessToken(scope);
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), string.Format("Caching AccessToken. Key:{0} Token:{1}", (object) key, (object) accessToken1.TypeAndToken));
        simpleCache.Put(key, new CacheItem((object) accessToken1, CacheItemRetentionPolicy.ExpireIn2Hours));
        return accessToken1;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "OAuth2Utils: Error in GetAccessToken. Ex: " + (object) ex);
        throw;
      }
    }

    public AuthorizationCode GetAuthCodeForGuest(string clientId)
    {
      Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Entering GetAuthCodeForGuest");
      try
      {
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Calling CurrentUser.GetAuthCodeForGuest: " + clientId);
        AuthorizationCode authCodeForGuest = this._session.GetUser().GetAuthCodeForGuest(clientId);
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "CurrentUser.GetAuthCodeForGuest Response: " + (object) authCodeForGuest);
        return authCodeForGuest;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "OAuth2Utils: Error in GetAuthCodeForGuest. Ex: " + (object) ex);
        throw;
      }
    }

    public AuthorizationCode GetAuthCodeFromAuthToken()
    {
      Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Entering GetAuthCodeFromAuthToken");
      try
      {
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "Calling CurrentUser.GetAuthCodeFromAuthToken");
        AuthorizationCode codeFromAuthToken = this._session.GetUser().GetAuthCodeFromAuthToken();
        Tracing.Log(OAuth2Utils.sw, TraceLevel.Verbose, nameof (OAuth2Utils), "CurrentUser.GetAuthCodeFromAuthToken Response: " + (object) codeFromAuthToken);
        return codeFromAuthToken;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "OAuth2Utils: Error in GetAuthCodeFromAuthToken. Ex: " + (object) ex);
        throw;
      }
    }

    public void Execute(string scope, Action<AccessToken> work)
    {
      this.Execute((string) null, scope, work);
    }

    public void Execute(string serviceUrl, string scope, Action<AccessToken> work)
    {
      AccessToken accessToken1 = this.getAccessToken(serviceUrl, scope, false);
      try
      {
        work(accessToken1);
        return;
      }
      catch (WebException ex)
      {
        if (((HttpWebResponse) ex.Response).StatusCode != HttpStatusCode.Unauthorized)
          throw;
      }
      catch (HttpException ex)
      {
        if (ex.GetHttpCode() != 401)
          throw;
      }
      AccessToken accessToken2 = this.getAccessToken(serviceUrl, scope, true);
      work(accessToken2);
    }

    public async Task ExecuteAsync(string scope, Func<AccessToken, Task> work)
    {
      await this.ExecuteAsync((string) null, scope, work).ConfigureAwait(false);
    }

    public async Task ExecuteAsync(string serviceUrl, string scope, Func<AccessToken, Task> work)
    {
      AccessToken accessToken = this.getAccessToken(serviceUrl, scope, false);
      try
      {
        await work(accessToken).ConfigureAwait(false);
        return;
      }
      catch (WebException ex)
      {
        if (((HttpWebResponse) ex.Response).StatusCode != HttpStatusCode.Unauthorized)
          throw;
      }
      catch (HttpException ex)
      {
        if (ex.GetHttpCode() != 401)
          throw;
      }
      await work(this.getAccessToken(serviceUrl, scope, true)).ConfigureAwait(false);
    }

    private AccessToken getAccessToken(string serviceUrl, string scope, bool noCache)
    {
      AccessToken accessToken1 = this.GetAccessToken(scope, noCache);
      if (string.IsNullOrEmpty(serviceUrl))
        return accessToken1;
      AccessToken accessToken2 = new AccessToken()
      {
        Type = accessToken1.Type,
        Token = accessToken1.Token,
        HostName = accessToken1.HostName,
        ServiceUrl = serviceUrl.Replace("{host}", accessToken1.HostName)
      };
      ServicePointManager.FindServicePoint(new Uri(accessToken2.ServiceUrl)).ConnectionLeaseTimeout = 60000;
      return accessToken2;
    }
  }
}
