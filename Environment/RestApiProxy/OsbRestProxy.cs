// Decompiled with JetBrains decompiler
// Type: RestApiProxy.OsbRestProxy
// Assembly: Environment, Version=17.1.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 54BC7282-2405-4166-B8F8-72E1EF543E16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Environment.dll

using EllieMae.EMLite.RemotingServices;
using System.Net.Http;

#nullable disable
namespace RestApiProxy
{
  internal class OsbRestProxy : ProxyBase, IRestApiProxy
  {
    internal OsbRestProxy(string sessionId, string mediaContentType)
      : base(sessionId, mediaContentType)
    {
    }

    public HttpClient GetHttpClient()
    {
      string restApiBaseUri = this.GetRestApiBaseUri();
      string accessToken = this.GetAccessToken();
      HttpClient httpClient = this.BaseObject(restApiBaseUri);
      httpClient.DefaultRequestHeaders.Add("elli-session", accessToken);
      return httpClient;
    }

    internal override string GetRestApiBaseUri()
    {
      return !string.IsNullOrEmpty(Session.DefaultInstance.StartupInfo.RestApiBaseUriKey) ? Session.DefaultInstance.StartupInfo.RestApiBaseUriKey : this.GetConfigValue<string>("restApiBaseUriKey") ?? string.Empty;
    }

    internal override string GetAccessToken()
    {
      return string.IsNullOrEmpty(this.InstanceId) ? this.SessionId : this.InstanceId + "_" + this.SessionId;
    }
  }
}
