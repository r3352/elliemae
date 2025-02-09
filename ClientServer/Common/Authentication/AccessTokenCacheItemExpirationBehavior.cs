// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Authentication.AccessTokenCacheItemExpirationBehavior
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.SimpleCache;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Authentication
{
  public class AccessTokenCacheItemExpirationBehavior : ICacheItemExpirationBehavior
  {
    private const string className = "AccessTokenCacheItemExpirationBehavior�";
    private static readonly string sw = Tracing.SwCommon;
    private readonly string ClientSecret;
    private readonly string oAuthBaseUrl;

    public AccessTokenCacheItemExpirationBehavior(string clientSecret, string oauthBaseUrl)
    {
      this.ClientSecret = clientSecret;
      this.oAuthBaseUrl = oauthBaseUrl;
    }

    public string oAuthUrl => this.oAuthBaseUrl + "/oauth2/v1/revocation";

    public void OnExpiration(CacheItem cacheItem)
    {
      try
      {
        string accessToken = ((OAuth2.AuthToken) cacheItem.Value).access_token;
        WebRequest webRequest = WebRequest.Create(this.oAuthUrl);
        webRequest.Method = "POST";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        byte[] bytes = Encoding.UTF8.GetBytes("token=" + accessToken);
        webRequest.ContentLength = (long) bytes.Length;
        Stream requestStream = webRequest.GetRequestStream();
        requestStream.Write(bytes, 0, bytes.Length);
        requestStream.Close();
        using (StreamReader streamReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
          streamReader.ReadToEnd();
      }
      catch (Exception ex)
      {
        Tracing.Log(AccessTokenCacheItemExpirationBehavior.sw, nameof (AccessTokenCacheItemExpirationBehavior), TraceLevel.Warning, ex.ToString());
      }
    }
  }
}
