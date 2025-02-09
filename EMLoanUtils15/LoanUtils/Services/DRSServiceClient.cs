// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.DRSServiceClient
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
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class DRSServiceClient
  {
    private const string className = "DRSServiceClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc�";
    private static HttpClient _client = new HttpClient();
    private OAuth2Utils _oauth;
    private string _loanId;

    public DRSServiceClient(LoanDataMgr loanDataMgr)
    {
      this._loanId = loanDataMgr.LoanData.GUID;
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
    }

    public DRSServiceClient(ISession session, ISessionStartupInfo startupInfo)
    {
      this._oauth = new OAuth2Utils(session, startupInfo);
    }

    public async Task<BarcodeIdOutput[]> CreateBarcodes(BarcodeMetadataInput[] inputList)
    {
      Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "Entering CreateBarcodes");
      BarcodeIdOutput[] array;
      try
      {
        array = (await this.submitRequest<List<BarcodeIdOutput>>("{host}/encompass/v2/loans/{loanId}/barcodes".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "POST", JsonConvert.SerializeObject((object) new List<BarcodeMetadataInput>((IEnumerable<BarcodeMetadataInput>) inputList))).ConfigureAwait(false)).ToArray();
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "DRSServiceClient: Error in CreateBarcodes. Ex: " + (object) ex);
        throw;
      }
      return array;
    }

    public async Task<DocumentMetadataOutput> GetBarcode(string barcodeId)
    {
      Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "Entering GetBarcode");
      DocumentMetadataOutput barcode;
      try
      {
        barcode = await this.submitRequest<DocumentMetadataOutput>("{host}/encompass/v2/loans/{loanId}/barcodes/{barcodeId}".Replace("{loanId}", Guid.Parse(this._loanId).ToString()).Replace("{barcodeId}", barcodeId), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "DRSServiceClient: Error in GetBarcode. Ex: " + (object) ex);
        throw;
      }
      return barcode;
    }

    private async Task<T> submitRequest<T>(
      string url,
      string method,
      string content,
      HttpResponseHeaders responseHeaders = null)
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
          Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "Calling SendAsync: " + request.RequestUri.ToString() + " Payload: " + content);
          using (HttpResponseMessage response = await DRSServiceClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            responseHeaders = response.Headers;
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "SendAsync Response Content: " + str);
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
                  Tracing.Log(DRSServiceClient.sw, TraceLevel.Verbose, nameof (DRSServiceClient), "SendAsync Response Content: " + str);
                  DRSServiceError drsServiceError = JsonConvert.DeserializeObject<DRSServiceError>(str);
                  message = drsServiceError.DisplayMessage == null ? message + " " + JsonConvert.DeserializeObject<ServiceError>(str).Code : message + " " + drsServiceError.DisplayMessage;
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
