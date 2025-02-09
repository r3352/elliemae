// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.EBSRestClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
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
  public class EBSRestClient
  {
    private const string className = "EBSRestClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc�";
    private static HttpClient _client = new HttpClient();
    private readonly OAuth2Utils _oauth;
    private string _loanId;

    public EBSRestClient(LoanDataMgr loanDataMgr)
    {
      this._loanId = loanDataMgr.LoanData.GUID;
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
    }

    public EBSRestClient(LoanDataMgr loanDataMgr, EllieMae.EMLite.ClientServer.Authentication.AccessToken accessToken)
    {
      this._loanId = loanDataMgr.LoanData.GUID;
      this._oauth = new OAuth2Utils(accessToken);
    }

    public EBSRestClient(EllieMae.EMLite.ClientServer.Authentication.AccessToken accessToken)
    {
      this._oauth = new OAuth2Utils(accessToken);
    }

    public async Task<GetLoanErrorsResponse> GetCCLoanErrors(
      string firstName,
      string lastName,
      string loanGuid,
      string startDate,
      string endDate)
    {
      Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Entering GetCCLoanErrors");
      GetLoanErrorsResponse ccLoanErrors;
      try
      {
        string url = "{host}/encompass/v0.9/errors?filter=startDate::{startDate},endDate::{endDate}".Replace("{startDate}", startDate).Replace("{endDate}", endDate);
        if (!string.IsNullOrEmpty(firstName))
          url = url + ",borrowerFirstName::" + Uri.EscapeDataString(firstName);
        if (!string.IsNullOrEmpty(lastName))
          url = url + ",borrowerLastName::" + Uri.EscapeDataString(lastName);
        if (!string.IsNullOrEmpty(loanGuid))
          url = url + ",loanId::" + Uri.EscapeDataString(loanGuid);
        ccLoanErrors = await this.submitRequest<GetLoanErrorsResponse>(url, "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EBSRestClient: Error in GetCCLoanErrors. Ex: " + (object) ex);
        throw;
      }
      return ccLoanErrors;
    }

    public async Task<string> GetCCSiteId(string userId)
    {
      Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Entering GetCCSiteId");
      string id;
      try
      {
        id = (await this.submitRequest<GetCCSiteIdResponse>("{host}/encompass/v0.9/users/{userId}/site?loanId={loanId}".Replace("{userId}", userId).Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false))?.id;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EBSRestClient: Error in GetCCSiteId. Ex: " + (object) ex);
        throw;
      }
      return id;
    }

    public async Task<bool> SetAttachmentTitle(CloudAttachment attachment, string title)
    {
      Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Entering SetAttachmentTitle");
      bool flag;
      try
      {
        string str = await this.submitRequest<string>("{host}/encompass/v3/loans/{loanId}/attachments?action=Update".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "PATCH", JsonConvert.SerializeObject((object) new EBSAttachment[1]
        {
          new EBSAttachment()
          {
            id = attachment.ID,
            title = title,
            type = "Cloud"
          }
        })).ConfigureAwait(false);
        attachment.Title = title;
        flag = true;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EBSRestClient: Error in SetAttachmentTitle. Ex: " + (object) ex);
        throw;
      }
      return flag;
    }

    public async Task<string> GetLoan3()
    {
      Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Entering GetLoan3");
      string loan3;
      try
      {
        loan3 = await this.submitRequest<string>("{host}/encompass/v3/loans/{loanId}".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EBSRestClient: Error in GetLoan3. Ex: " + (object) ex);
        throw;
      }
      return loan3;
    }

    public async Task<string> GetEffectiveRightsV3(string userId)
    {
      Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Entering GetEffectiveRightsV3");
      string effectiveRightsV3;
      try
      {
        effectiveRightsV3 = await this.submitRequest<string>("{host}/encompass/v3/users/{userId}/effectiverights".Replace("{userId}", userId), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EBSRestClient: Error in GetEffectiveRightsV3. Ex: " + (object) ex);
        throw;
      }
      return effectiveRightsV3;
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
          Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await EBSRestClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "SendAsync Response Content: " + str);
              result = !(typeof (T) == typeof (string)) ? JsonConvert.DeserializeObject<T>(str) : (T) Convert.ChangeType((object) str, typeof (T));
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
                  Tracing.Log(EBSRestClient.sw, TraceLevel.Verbose, nameof (EBSRestClient), "SendAsync Response Content: " + str);
                  message = message + " " + JsonConvert.DeserializeObject<EBSRestError>(str).DisplayMessage;
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
