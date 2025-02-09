// Decompiled with JetBrains decompiler
// Type: Elli.Web.Host.SSF.Context.SSFGuestRegistry
// Assembly: Elli.Web.Host, Version=19.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9006AF5-0CBA-41F3-A51F-BE05E37C7972
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Web.Host.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.LoanUtils.Authentication;
using EllieMae.EMLite.RemotingServices;
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
namespace Elli.Web.Host.SSF.Context
{
  internal class SSFGuestRegistry
  {
    private const string className = "SSFGuestRegistry";
    private static readonly string sw = Tracing.SwThinThick;
    private const string _scope = "sc";
    private static HttpClient _client = new HttpClient();
    private static Dictionary<string, SSFGuest> _cachedGuests = new Dictionary<string, SSFGuest>();
    private readonly OAuth2Utils _oauth;

    public SSFGuestRegistry()
    {
      this._oauth = new OAuth2Utils(Session.ISession, Session.StartupInfo);
    }

    internal async Task<SSFGuest> GetGuest(string guestUrl)
    {
      Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "Entering GetGuest: " + guestUrl);
      try
      {
        Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "Checking Cached Guest Info: " + guestUrl);
        if (SSFGuestRegistry._cachedGuests.ContainsKey(guestUrl))
        {
          Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "Returning Cached Guest Info: " + guestUrl);
          return SSFGuestRegistry._cachedGuests[guestUrl];
        }
        List<SSFGuest> ssfGuestList = await this.submitRequest<List<SSFGuest>>("{host}/ssf/v1/guests?uri={uri}&appId={appId}&appVersion={appVersion}".Replace("{host}", "https://www.epassbusinesscenter.com").Replace("{uri}", HttpUtility.UrlEncode(guestUrl)).Replace("{appId}", "sc").Replace("{appVersion}", VersionInformation.CurrentVersion.GetExtendedVersion(EncompassEdition.None)), "GET", (string) null).ConfigureAwait(false);
        SSFGuest guest;
        if (ssfGuestList != null && ssfGuestList.Count > 0)
        {
          guest = ssfGuestList[0];
          Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "Caching Guest Info: " + guestUrl);
          SSFGuestRegistry._cachedGuests[guestUrl] = guest;
        }
        return guest;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SSFGuestRegistry: Error in GetGuestInfo. Ex: " + (object) ex);
        throw;
      }
    }

    private async Task<T> submitRequest<T>(string url, string method, string content)
    {
      T result = default (T);
      await this._oauth.ExecuteAsync(url, "sc", (Func<AccessToken, Task>) (async AccessToken =>
      {
        using (HttpRequestMessage request = new HttpRequestMessage())
        {
          request.Method = new HttpMethod(method);
          request.RequestUri = new Uri(AccessToken.ServiceUrl);
          request.Headers.Add("Authorization", AccessToken.TypeAndToken);
          request.Headers.Add("Accept", "application/json");
          if (!string.IsNullOrEmpty(content))
            request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
          Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await SSFGuestRegistry._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "SendAsync Response Content: " + str);
              result = JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              if (response.StatusCode != HttpStatusCode.NotFound)
              {
                string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
                try
                {
                  string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                  Tracing.Log(SSFGuestRegistry.sw, TraceLevel.Verbose, nameof (SSFGuestRegistry), "SendAsync Response Content: " + str);
                  SSFGuestRegistryError guestRegistryError = JsonConvert.DeserializeObject<SSFGuestRegistryError>(str);
                  message = message + " " + guestRegistryError.code + " " + guestRegistryError.summary + " " + guestRegistryError.details;
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
        }
      })).ConfigureAwait(false);
      return result;
    }
  }
}
