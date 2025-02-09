// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Authentication.OAuth2Client
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.SimpleCache;
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
namespace EllieMae.EMLite.Server.Authentication
{
  public class OAuth2Client
  {
    private const string className = "OAuth2Client�";
    private static readonly HttpClient _client = new HttpClient();
    private readonly IClientContext _context;
    private readonly string _serviceUrl;
    private readonly string _smartClientId;
    private readonly string _smartClientSecret;
    private readonly string _oAuthClientId;
    private readonly string _oAuthClientSecret;
    private const string _jwtIssuer = "urn:elli:service:ids�";
    private const string _jwtSubject = "urn:elli:service:smartclient�";
    private const string GrantTypeTokenExchange = "urn:ietf:params:oauth:grant-type:token-exchange�";
    private const string SubjectTokenTypeAccessToken = "urn:ietf:params:oauth:token-type:access_token�";

    public OAuth2Client(IClientContext context)
      : this(context, (string) null)
    {
    }

    public OAuth2Client(IClientContext context, string serviceUrl)
    {
      this._context = context;
      this._serviceUrl = serviceUrl;
      this._smartClientId = "smartclient";
      this._smartClientSecret = EnConfigurationSettings.AppSettings["SmartClientSecret"];
      this._oAuthClientId = EnConfigurationSettings.AppSettings["oAuth.ClientId"];
      this._oAuthClientSecret = EnConfigurationSettings.AppSettings["oAuth.ClientSecret"];
    }

    public async Task<AccessToken> GetJWT(
      string audience,
      string scope,
      Dictionary<string, object> claims)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetJWT");
      AccessToken accessToken;
      try
      {
        accessToken = this.createAccessToken("Bearer", new JwtBuilder(await this.GetJWTKey(), "urn:elli:service:ids", "urn:elli:service:smartclient").Create(audience, scope, claims));
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetJWT. Ex: " + (object) ex);
        throw;
      }
      return accessToken;
    }

    private async Task<JWTKey> GetJWTKey()
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetJWTKey");
      try
      {
        ICache cache = CacheManager.GetSimpleCache("OAuth2Cache");
        string cacheKey = "JWTKey";
        JWTKey jwtKey1 = (JWTKey) cache.Get(cacheKey);
        if (jwtKey1 != null)
        {
          this._context.TraceLog.Write(TraceLevel.Info, nameof (OAuth2Client), "Reading the JWTKey from cache. Key :  " + cacheKey + "Cached Value : " + jwtKey1.kid);
          return jwtKey1;
        }
        GetJWTSResponse getJwtsResponse = await this.submitRequest<GetJWTSResponse>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v2/jwks", "GET", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(this._smartClientId + ":" + this._smartClientSecret)), (Dictionary<string, string>) null).ConfigureAwait(false);
        JWTKey jwtKey2;
        if (getJwtsResponse == null)
        {
          jwtKey2 = (JWTKey) null;
        }
        else
        {
          JWTKey[] keys = getJwtsResponse.keys;
          jwtKey2 = keys != null ? ((IEnumerable<JWTKey>) keys).OrderByDescending<JWTKey, string>((Func<JWTKey, string>) (x => x.kid)).FirstOrDefault<JWTKey>() : (JWTKey) null;
        }
        JWTKey jwtKey3 = jwtKey2;
        if (jwtKey3 == null)
          throw new KeyNotFoundException();
        this._context.TraceLog.Write(TraceLevel.Info, nameof (OAuth2Client), "Fetched new JWT key. Key :  " + cacheKey + "Cached Value : " + jwtKey3.kid);
        cache.Put(cacheKey, new CacheItem((object) jwtKey3, CacheItemRetentionPolicy.ExpireIn2Hours, (ICacheItemExpirationBehavior) new SimpleCacheItemExpirationBehavior()));
        return jwtKey3;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetJWTS. Ex: " + (object) ex);
        throw;
      }
    }

    public async Task<AccessToken> GetAccessToken(string sessionId, string scope)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAccessToken");
      AccessToken accessToken;
      try
      {
        OAuth2Token oauth2Token = await this.submitRequest<OAuth2Token>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._smartClientId
          },
          {
            "client_secret",
            this._smartClientSecret
          },
          {
            "grant_type",
            "urn:elli:params:oauth:grant-type:encompass-bearer"
          },
          {
            "session",
            !string.IsNullOrEmpty(this._context.InstanceName) ? this._context.InstanceName + "_" + sessionId : "Local_" + sessionId
          },
          {
            nameof (scope),
            scope
          }
        }).ConfigureAwait(false);
        accessToken = this.createAccessToken(oauth2Token.token_type, oauth2Token.access_token);
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAccessToken. Ex: " + (object) ex);
        throw;
      }
      return accessToken;
    }

    private AccessToken createAccessToken(string type, string token)
    {
      AccessToken accessToken = new AccessToken()
      {
        Type = type,
        Token = token,
        HostName = EnConfigurationSettings.AppSettings["oAuth.Url"]
      };
      if (!string.IsNullOrEmpty(this._serviceUrl))
      {
        accessToken.ServiceUrl = this._serviceUrl.Replace("{host}", accessToken.HostName);
        ServicePointManager.FindServicePoint(new Uri(accessToken.ServiceUrl)).ConnectionLeaseTimeout = 60000;
      }
      return accessToken;
    }

    public async Task<AuthorizationCode> GetAuthCodeForMFA(string sessionId, string scope)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAuthCodeForMFA");
      AuthorizationCode authCodeForMfa;
      try
      {
        OAuth2Code oauth2Code = await this.submitRequest<OAuth2Code>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._oAuthClientId
          },
          {
            "client_secret",
            this._oAuthClientSecret
          },
          {
            "grant_type",
            "urn:ietf:params:oauth:grant-type:token-exchange"
          },
          {
            "subject_token",
            (await this.GetAccessToken(sessionId, scope)).Token
          },
          {
            "subject_token_type",
            "urn:ietf:params:oauth:token-type:access_token"
          },
          {
            "requested_token_type",
            "urn:elli:params:oauth:token-type:authorization_code"
          }
        }).ConfigureAwait(false);
        authCodeForMfa = new AuthorizationCode()
        {
          Code = oauth2Code.authorization_code,
          IssuedType = oauth2Code.issued_token_type,
          TokenType = oauth2Code.token_type,
          ExpiresIn = oauth2Code.expires_in,
          HostName = EnConfigurationSettings.AppSettings["oAuth.Url"]
        };
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAuthCodeForMFA. Ex: " + (object) ex);
        throw;
      }
      return authCodeForMfa;
    }

    public async Task<AccessToken> GetAccessTokenForMFA(string authCode, string redirectUri)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAccessTokenForMFA");
      AccessToken accessToken;
      try
      {
        OAuth2Token oauth2Token = await this.submitRequest<OAuth2Token>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._oAuthClientId
          },
          {
            "client_secret",
            this._oAuthClientSecret
          },
          {
            "grant_type",
            "authorization_code"
          },
          {
            "code",
            authCode
          },
          {
            "redirect_uri",
            HttpUtility.UrlEncode(redirectUri)
          }
        }).ConfigureAwait(false);
        accessToken = this.createAccessToken(oauth2Token.token_type, oauth2Token.access_token);
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAccessTokenForMFA. Ex: " + (object) ex);
        throw;
      }
      return accessToken;
    }

    public async Task<OAuth2TokenIntrospection> InspectAccessToken(string accessToken)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering InspectAccessToken");
      OAuth2TokenIntrospection tokenIntrospection;
      try
      {
        tokenIntrospection = await this.submitRequest<OAuth2TokenIntrospection>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token/introspection", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._oAuthClientId
          },
          {
            "client_secret",
            this._oAuthClientSecret
          },
          {
            "token",
            accessToken
          }
        }).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in InspectAccessToken. Ex: " + (object) ex);
        throw;
      }
      return tokenIntrospection;
    }

    public async Task<AuthorizationCode> GetAuthCodeForGuest(string accessToken, string clientId)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAuthCodeForGuest");
      AuthorizationCode authCodeForGuest;
      try
      {
        OAuth2Code oauth2Code = await this.submitRequest<OAuth2Code>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            clientId
          },
          {
            "grant_type",
            "urn:ietf:params:oauth:grant-type:token-exchange"
          },
          {
            "subject_token",
            accessToken
          },
          {
            "subject_token_type",
            "urn:ietf:params:oauth:token-type:access_token"
          },
          {
            "requested_token_type",
            "urn:elli:params:oauth:token-type:authorization_code"
          }
        }).ConfigureAwait(false);
        authCodeForGuest = new AuthorizationCode()
        {
          Code = oauth2Code.authorization_code,
          IssuedType = oauth2Code.issued_token_type,
          TokenType = oauth2Code.token_type,
          ExpiresIn = oauth2Code.expires_in,
          HostName = EnConfigurationSettings.AppSettings["oAuth.Url"]
        };
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAuthCodeForGuest. Ex: " + (object) ex);
        throw;
      }
      return authCodeForGuest;
    }

    public async Task<AuthorizationCode> GetAuthCodeForGuest(
      string accessToken,
      string clientId,
      string scope,
      string redirectUri)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAuthCodeForGuest");
      AuthorizationCode authCodeForGuest;
      try
      {
        OAuth2Code oauth2Code = await this.submitRequest<OAuth2Code>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            clientId
          },
          {
            "grant_type",
            "urn:ietf:params:oauth:grant-type:token-exchange"
          },
          {
            "subject_token",
            accessToken
          },
          {
            "subject_token_type",
            "urn:ietf:params:oauth:token-type:access_token"
          },
          {
            "requested_token_type",
            "urn:elli:params:oauth:token-type:authorization_code"
          },
          {
            nameof (scope),
            scope
          },
          {
            "redirect_uri",
            redirectUri
          }
        }).ConfigureAwait(false);
        authCodeForGuest = new AuthorizationCode()
        {
          Code = oauth2Code.authorization_code,
          IssuedType = oauth2Code.issued_token_type,
          TokenType = oauth2Code.token_type,
          ExpiresIn = oauth2Code.expires_in,
          HostName = EnConfigurationSettings.AppSettings["oAuth.Url"]
        };
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAuthCodeForGuest. Ex: " + (object) ex);
        throw;
      }
      return authCodeForGuest;
    }

    public async Task<AccessToken> ExchangeAuthCodeForAccessToken(
      string authCode,
      string clientId,
      string scope,
      string redirectUri)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering ExchangeAuthCodeForAccessToken");
      AccessToken accessToken;
      try
      {
        OAuth2Token oauth2Token = await this.submitRequest<OAuth2Token>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            clientId
          },
          {
            "redirect_uri",
            redirectUri
          },
          {
            "grant_type",
            "authorization_code"
          },
          {
            "code",
            authCode
          },
          {
            nameof (scope),
            scope
          }
        }).ConfigureAwait(false);
        accessToken = this.createAccessToken(oauth2Token.token_type, oauth2Token.access_token);
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in ExchangeAuthCodeForAccessToken. Ex: " + (object) ex);
        throw;
      }
      return accessToken;
    }

    public async Task<AccessToken> GetAccessTokenForRfc(string sessionId)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAccessTokenForRfc");
      AccessToken accessToken;
      try
      {
        OAuth2Token oauth2Token = await this.submitRequest<OAuth2Token>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._oAuthClientId
          },
          {
            "client_secret",
            this._oAuthClientSecret
          },
          {
            "grant_type",
            "urn:ietf:params:oauth:grant-type:token-exchange"
          },
          {
            "subject_token",
            !string.IsNullOrEmpty(this._context.InstanceName) ? this._context.InstanceName + "_" + sessionId : "Local_" + sessionId
          },
          {
            "scope",
            "sc"
          },
          {
            "requested_token_type",
            "urn:ietf:params:oauth:token-type:access_token"
          },
          {
            "subject_token_type",
            "urn:elli:params:oauth:token-type:encompass_session"
          }
        }).ConfigureAwait(false);
        accessToken = this.createAccessToken(oauth2Token.token_type, oauth2Token.access_token);
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAccessTokenForRfc. Ex: " + (object) ex);
        throw;
      }
      return accessToken;
    }

    public async Task<AuthorizationCode> GetAuthCodeFromAuthToken(string accessToken)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Entering GetAuthCodeFromAuthToken");
      AuthorizationCode codeFromAuthToken;
      try
      {
        OAuth2Code oauth2Code = await this.submitRequest<OAuth2Code>(EnConfigurationSettings.AppSettings["oAuth.Url"] + "/oauth2/v1/token", "POST", (string) null, new Dictionary<string, string>()
        {
          {
            "client_id",
            this._oAuthClientId
          },
          {
            "client_secret",
            this._oAuthClientSecret
          },
          {
            "grant_type",
            "urn:ietf:params:oauth:grant-type:token-exchange"
          },
          {
            "subject_token_type",
            "urn:ietf:params:oauth:token-type:access_token"
          },
          {
            "subject_token",
            accessToken
          },
          {
            "requested_token_type",
            "urn:elli:params:oauth:token-type:authorization_code"
          }
        }).ConfigureAwait(false);
        codeFromAuthToken = new AuthorizationCode()
        {
          Code = oauth2Code.authorization_code,
          IssuedType = oauth2Code.issued_token_type,
          TokenType = oauth2Code.token_type,
          ExpiresIn = oauth2Code.expires_in,
          HostName = EnConfigurationSettings.AppSettings["oAuth.Url"]
        };
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (OAuth2Client), "Error in GetAuthCodeFromAuthToken. Ex: " + (object) ex);
        throw;
      }
      return codeFromAuthToken;
    }

    private async Task<T> submitRequest<T>(
      string url,
      string method,
      string authorization,
      Dictionary<string, string> content)
    {
      T obj = default (T);
      using (HttpRequestMessage request = new HttpRequestMessage())
      {
        request.Method = new HttpMethod(method);
        request.RequestUri = new Uri(url);
        request.Headers.Add("Accept", "application/json");
        if (authorization != null)
          request.Headers.Add("Authorization", authorization);
        if (content != null)
          request.Content = (HttpContent) new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) content);
        this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "Calling SendAsync: " + request.RequestUri.ToString());
        using (HttpResponseMessage response = await OAuth2Client._client.SendAsync(request).ConfigureAwait(false))
        {
          this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "SendAsync Response StatusCode: " + (object) response.StatusCode);
          if (response.IsSuccessStatusCode)
          {
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "SendAsync Response Content: " + str);
            obj = JsonConvert.DeserializeObject<T>(str);
          }
          else
          {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
              throw new HttpException((int) response.StatusCode, "Unauthorized Request");
            if (response.StatusCode == HttpStatusCode.NotFound)
              throw new HttpException((int) response.StatusCode, "Not Found");
            string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
            try
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              this._context.TraceLog.Write(TraceLevel.Verbose, nameof (OAuth2Client), "SendAsync Response Content: " + str);
              OAuth2Error oauth2Error = JsonConvert.DeserializeObject<OAuth2Error>(str);
              message = message + " " + oauth2Error.error + " " + oauth2Error.error_description;
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
      return obj;
    }
  }
}
