// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.EDelivery.EDeliveryRestClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
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
namespace EllieMae.EMLite.LoanUtils.EDelivery
{
  public class EDeliveryRestClient
  {
    private const string className = "EDeliveryRestClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc�";
    private static HttpClient _client = new HttpClient();
    private readonly OAuth2Utils _oauth;
    private string _loanId;

    public EDeliveryRestClient(LoanDataMgr loanDataMgr)
    {
      this._loanId = loanDataMgr.LoanData.GUID;
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
    }

    public async Task<PackageGroup> GetPackageGroup()
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetPackageGroup");
      PackageGroup packageGroup;
      try
      {
        packageGroup = await this.submitRequest<PackageGroup>("{host}/ellisign/v3/loans/{loanId}".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetPackageGroup. Ex: " + (object) ex);
        throw;
      }
      return packageGroup;
    }

    public async Task<string> AddGroupLevelConsent(string recipientID, string name, string email)
    {
      string str;
      try
      {
        str = await this.submitRequest<string>("{host}/delivery/v3/loans/{loanId}/recipients/{recipientID}/consents".Replace("{loanId}", Guid.Parse(this._loanId).ToString()).Replace("{recipientID}", recipientID), "POST", "{\"status\": \"Declined\", \"name\": \"" + name + "\", \"email\": \"" + email + "\"}").ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in AddGroupLevelConsent (loanID: " + Guid.Parse(this._loanId).ToString() + ", recipientID: " + recipientID + "). Exception: " + (object) ex);
        throw;
      }
      return str;
    }

    public async Task<BinaryObject> GetPackageGroupConsentPdf()
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetPackageGroupConsentPdf");
      BinaryObject packageGroupConsentPdf;
      try
      {
        ConsentPdfOutput consentPdfOutput = await this.submitRequest<ConsentPdfOutput>("{host}/ellisign/v3/loans/{loanId}/consentPdf?format=Json".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false);
        packageGroupConsentPdf = consentPdfOutput == null || consentPdfOutput.pdf == null ? (BinaryObject) null : new BinaryObject(Convert.FromBase64String(consentPdfOutput.pdf));
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetPackageGroupConsentPdf. Ex: " + (object) ex);
        throw;
      }
      return packageGroupConsentPdf;
    }

    public async Task<BinaryObject> GetPackageConsentPdf(string packageId)
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetPackageConsentPdf");
      BinaryObject packageConsentPdf;
      try
      {
        ConsentPdfOutput consentPdfOutput = await this.submitRequest<ConsentPdfOutput>("{host}/ellisign/v3/loans/{loanId}/packages/{packageId}/consentPdf?format=Json".Replace("{loanId}", Guid.Parse(this._loanId).ToString()).Replace("{packageId}", Guid.Parse(packageId).ToString()), "GET", (string) null).ConfigureAwait(false);
        packageConsentPdf = consentPdfOutput == null || consentPdfOutput.pdf == null ? (BinaryObject) null : new BinaryObject(Convert.FromBase64String(consentPdfOutput.pdf));
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetPackageConsentPdf. Ex: " + (object) ex);
        throw;
      }
      return packageConsentPdf;
    }

    public async Task<EDeliveryPackageTrackingDetail> GetDisclosureTrackingDetails()
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetDisclosureTrackingDetails");
      EDeliveryPackageTrackingDetail disclosureTrackingDetails;
      try
      {
        disclosureTrackingDetails = await this.submitRequest<EDeliveryPackageTrackingDetail>("{host}/ellisign/v3/loans/{loanId}/packageTracking".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetDisclosureTrackingDetails. Ex: " + (object) ex);
        throw;
      }
      return disclosureTrackingDetails;
    }

    public async Task<EDeliveryConsentTrackingResponse> GetLoanLevelConsentTracking()
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetLoanLevelConsentTracking");
      EDeliveryConsentTrackingResponse levelConsentTracking;
      try
      {
        levelConsentTracking = await this.submitRequest<EDeliveryConsentTrackingResponse>("{host}/ellisign/v3/loans/{loanId}/consentTracking".Replace("{loanId}", Guid.Parse(this._loanId).ToString()), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetLoanLevelConsentTracking. Ex: " + (object) ex);
        throw;
      }
      return levelConsentTracking;
    }

    public async Task<EDeliverySignedDocument[]> GetSignedDocumentList(string loanId)
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetSignedDocumentList");
      EDeliverySignedDocument[] signedDocumentList;
      try
      {
        signedDocumentList = await this.submitRequest<EDeliverySignedDocument[]>("{host}/ellisign/v3/loans/{loanGuid}/signedDocuments".Replace("{loanGuid}", loanId), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetSignedDocumentList. Ex: " + (object) ex);
        throw;
      }
      return signedDocumentList;
    }

    public async Task<string> GetVaultExportData(
      string loanId,
      string packageId,
      string documentId)
    {
      Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Entering GetVaultExportData");
      string dataInXmlFormat;
      try
      {
        dataInXmlFormat = (await this.submitRequest<EVaultAudit>("{host}/ellisign/v3/loans/{loanGuid}/packages/{packageId}/documents/{documentId}/vaultExport".Replace("{loanGuid}", loanId).Replace("{packageId}", packageId).Replace("{documentId}", documentId), "GET", (string) null).ConfigureAwait(false)).GetDataInXMLFormat();
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EDeliveryRestClient: Error in GetVaultExportData. Ex: " + (object) ex);
        throw;
      }
      return dataInXmlFormat;
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
          Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await EDeliveryRestClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "SendAsync Response Content: " + str);
              result = JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              if (response.StatusCode == HttpStatusCode.Conflict)
                throw new HttpException((int) response.StatusCode, "Conflict");
              if (response.StatusCode != HttpStatusCode.NotFound)
              {
                string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
                try
                {
                  string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                  Tracing.Log(EDeliveryRestClient.sw, TraceLevel.Verbose, nameof (EDeliveryRestClient), "SendAsync Response Content: " + str);
                  EDeliveryRestError edeliveryRestError = JsonConvert.DeserializeObject<EDeliveryRestError>(str);
                  message = message + " " + edeliveryRestError.code + " " + edeliveryRestError.summary + " " + edeliveryRestError.details;
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
