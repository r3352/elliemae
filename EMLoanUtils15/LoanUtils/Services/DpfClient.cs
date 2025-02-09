// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.DpfClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Authentication;
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
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class DpfClient
  {
    private const string className = "DpfClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc�";
    private static HttpClient _client = new HttpClient();
    private static string _loanId = (string) null;
    private static ResolveLoanSettingsResponseList _resolveLoanSettingsResponseList = (ResolveLoanSettingsResponseList) null;
    private readonly OAuth2Utils _oauth;

    public DpfClient(LoanDataMgr loanDataMgr)
    {
      if (DpfClient._loanId != loanDataMgr.LoanData.GUID)
        DpfClient._resolveLoanSettingsResponseList = (ResolveLoanSettingsResponseList) null;
      DpfClient._loanId = loanDataMgr.LoanData.GUID;
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
    }

    public async Task<ResolveLoanSettingsResponseList> ResolveLoanSettings()
    {
      Tracing.Log(DpfClient.sw, TraceLevel.Verbose, nameof (DpfClient), "Entering ResolveLoanSettings");
      try
      {
        if (DpfClient._resolveLoanSettingsResponseList == null)
          DpfClient._resolveLoanSettingsResponseList = await this.submitRequest<ResolveLoanSettingsResponseList>("{host}/deliverypartners/v1/partners/resolve/loans/{groupId}?type=POS".Replace("{groupId}", Guid.Parse(DpfClient._loanId).ToString()), "POST", (string) null).ConfigureAwait(false);
      }
      catch (HttpException ex)
      {
        if (ex.GetHttpCode() == 404)
        {
          DpfClient._resolveLoanSettingsResponseList = new ResolveLoanSettingsResponseList();
        }
        else
        {
          RemoteLogger.Write(TraceLevel.Error, "DpfClient: Error in ResolveLoanSettings. Ex: " + (object) ex);
          throw;
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "DpfClient: Error in ResolveLoanSettings. Ex: " + (object) ex);
        throw;
      }
      return DpfClient._resolveLoanSettingsResponseList;
    }

    private async Task<T> submitRequest<T>(string url, string method, string content)
    {
      T result = default (T);
      await this._oauth.ExecuteAsync(url, "sc", (Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
      {
        using (HttpRequestMessage request = new HttpRequestMessage())
        {
          request.Method = new HttpMethod(method);
          request.RequestUri = new Uri(AccessToken.ServiceUrl);
          request.Headers.Add("Authorization", AccessToken.TypeAndToken);
          request.Headers.Add("Accept", "application/json");
          if (!string.IsNullOrEmpty(content))
            request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
          Tracing.Log(DpfClient.sw, TraceLevel.Verbose, nameof (DpfClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await DpfClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(DpfClient.sw, TraceLevel.Verbose, nameof (DpfClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(DpfClient.sw, TraceLevel.Verbose, nameof (DpfClient), "SendAsync Response Content: " + str);
              try
              {
                result = JsonConvert.DeserializeObject<T>(str);
              }
              catch (Exception ex)
              {
                RemoteLogger.Write(TraceLevel.Error, "DpfClient: Error in DeserializeObject. Ex: " + (object) ex);
                throw;
              }
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
              try
              {
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Tracing.Log(DpfClient.sw, TraceLevel.Verbose, nameof (DpfClient), "SendAsync Response Content: " + str);
                DPFError dpfError = JsonConvert.DeserializeObject<DPFError>(str);
                message = message + " " + dpfError.code + " " + dpfError.summary + " " + dpfError.details;
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
      })).ConfigureAwait(false);
      return result;
    }
  }
}
