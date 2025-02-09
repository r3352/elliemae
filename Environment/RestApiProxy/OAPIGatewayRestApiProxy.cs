// Decompiled with JetBrains decompiler
// Type: RestApiProxy.OAPIGatewayRestApiProxy
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Authentication;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

#nullable disable
namespace RestApiProxy
{
  internal class OAPIGatewayRestApiProxy : ProxyBase, IRestApiProxy
  {
    private string baseUri;
    private string authToken;
    private string scheme;
    private bool useOAuthURL = true;

    internal OAPIGatewayRestApiProxy(string sessionId, string mediaContentType)
      : base(sessionId, mediaContentType)
    {
      this.SetAccessToken();
    }

    internal OAPIGatewayRestApiProxy(SessionObjects sessionObjects, string mediaContentType)
      : base(sessionObjects.SessionID, mediaContentType)
    {
      this.SetAccessToken(sessionObjects);
    }

    internal OAPIGatewayRestApiProxy(string sessionId, string mediaContentType, bool useOAuthURL)
      : base(sessionId, mediaContentType)
    {
      this.SetAccessToken();
      this.useOAuthURL = useOAuthURL;
    }

    internal OAPIGatewayRestApiProxy(
      string sessionId,
      string mediaContentType,
      string token,
      string scheme)
      : base(sessionId, mediaContentType)
    {
      this.authToken = token;
      this.scheme = scheme;
    }

    internal OAPIGatewayRestApiProxy(string sessionId, string mediaContentType, string scheme)
      : base(sessionId, mediaContentType)
    {
      this.scheme = scheme;
    }

    public HttpClient GetHttpClient()
    {
      this.baseUri = !this.useOAuthURL ? this.GetFsApiBaseUri() : this.GetRestApiBaseUri();
      HttpClient httpClient = this.BaseObject(this.baseUri);
      if (this.scheme.Equals("x-api-key", StringComparison.InvariantCultureIgnoreCase))
        httpClient.DefaultRequestHeaders.Add(this.scheme, this.authToken);
      else if (this.scheme.Equals("elli-session", StringComparison.InvariantCultureIgnoreCase))
        httpClient.DefaultRequestHeaders.Add(this.scheme, this.GetElliSession());
      else
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(this.scheme, this.authToken);
      return httpClient;
    }

    private string GetElliSession()
    {
      return string.IsNullOrEmpty(this.InstanceId) ? this.SessionId : this.InstanceId + "_" + this.SessionId;
    }

    public HttpRequestMessage GetHttpRequestMessage(string url, string httpMethod = "PATCH")
    {
      HttpRequestMessage httpRequestMessage = new HttpRequestMessage(new HttpMethod(httpMethod), this.GetRestApiBaseUri() + url);
      httpRequestMessage.Headers.Add("Authorization", this.scheme + " " + this.authToken);
      httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      return httpRequestMessage;
    }

    private void SetAccessToken(SessionObjects sessionObjects)
    {
      string typeAndToken = new OAuth2(sessionObjects.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(sessionObjects), CacheItemRetentionPolicy.NoRetention).GetAccessToken(sessionObjects.Session.Server.InstanceName, sessionObjects.SessionID, "sc").TypeAndToken;
      this.scheme = typeAndToken.Split(' ')[0];
      this.authToken = typeAndToken.Split(' ')[1];
    }

    private void SetAccessToken()
    {
      string typeAndToken = new OAuth2(Session.DefaultInstance.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects), CacheItemRetentionPolicy.NoRetention).GetAccessToken(this.InstanceId, this.SessionId, "sc").TypeAndToken;
      this.scheme = typeAndToken.Split(' ')[0];
      this.authToken = typeAndToken.Split(' ')[1];
    }

    internal override string GetRestApiBaseUri()
    {
      string restApiBaseUri = EnConfigurationSettings.AppSettings["oAuth.Url"];
      if (string.IsNullOrEmpty(restApiBaseUri))
        restApiBaseUri = "https://api.elliemae.com";
      if (string.IsNullOrEmpty(Session.DefaultInstance?.StartupInfo?.OAPIGatewayBaseUri))
        return restApiBaseUri;
      return Session.DefaultInstance?.StartupInfo?.OAPIGatewayBaseUri;
    }

    internal string GetFsApiBaseUri() => Session.DefaultInstance.StartupInfo.FsApiBaseUriKey;
  }
}
