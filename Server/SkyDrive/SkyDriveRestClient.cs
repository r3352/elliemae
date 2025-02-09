// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.SkyDrive.SkyDriveRestClient
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Server.Authentication;
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
namespace EllieMae.EMLite.Server.SkyDrive
{
  public class SkyDriveRestClient
  {
    private const string className = "SkyDriveRestClient�";
    private const int _expires = 900;
    private const string _supportingPolicyId = "urn:elli:service:smartclient:supportingDocuments�";
    private const string _eFolderPolicyId = "urn:elli:service:efolder:attachment:deferred�";
    private const string _sdceFolderPolicy = "urn:elli:service:sc:attachment:sdc�";
    private const string _SDCeFolderPolicyId = "urn:elli:service:sc:attachment:sdc�";
    private const string _partnerFilePolicyId = "urn:elli:service:sc:attachment:transformation�";
    private const string _audience = "urn:elli:service:skydrive�";
    private const string _scope = "apiplatform�";
    private static readonly HttpClient _client = new HttpClient();
    private readonly IClientContext _context;
    private readonly string _sub;
    private readonly Dictionary<string, object> _claims;

    public SkyDriveRestClient(IClientContext context)
      : this(context, (string) null)
    {
    }

    public SkyDriveRestClient(IClientContext context, string userId)
    {
      this._context = context;
      this._claims = new Dictionary<string, object>();
      this._claims.Add("elli_iid", (object) context.InstanceName);
      this._claims.Add("elli_cid", (object) context.ClientID);
      if (string.IsNullOrEmpty(userId))
        return;
      this._sub = "urn:elli:encompass:" + context.InstanceName + ":user:" + userId;
      this._claims.Add("elli_uid", (object) string.Format("Encompass\\{0}\\{1}", (object) context.InstanceName, (object) userId));
    }

    public async Task<SkyDriveUrl> GetSkyDriveUrlForPut(
      string loanId,
      string fileKey,
      string contentType,
      bool useSkyDriveClassic)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveUrlForPut");
      SkyDriveUrl skyDriveUrlForPut;
      try
      {
        string str = "urn:elli:service:smartclient:supportingDocuments";
        if (fileKey.StartsWith("Attachment-", StringComparison.OrdinalIgnoreCase) || fileKey.StartsWith("Images-", StringComparison.OrdinalIgnoreCase) || fileKey.StartsWith("Thumbnails-", StringComparison.OrdinalIgnoreCase))
          str = "urn:elli:service:efolder:attachment:deferred";
        string url = EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/urlGenerator?action=put";
        GetSkyDriveUrlForPutRequest urlForPutRequest = new GetSkyDriveUrlForPutRequest()
        {
          contentType = contentType,
          expires = 900,
          sub = this._sub,
          meta = new Meta()
          {
            fileKey = fileKey,
            fileName = fileKey,
            owner = new Owner()
            {
              entityId = Guid.Parse(loanId).ToString(),
              entityType = "urn:elli:encompass:loanId"
            },
            policyId = str,
            tags = new string[1]{ "supportingData" }
          }
        };
        if (useSkyDriveClassic)
        {
          urlForPutRequest.meta.policyId = "urn:elli:service:sc:attachment:sdc";
          urlForPutRequest.meta.fileKey = string.Empty;
        }
        string content = JsonConvert.SerializeObject((object) urlForPutRequest);
        List<SkyDriveUrlResponse> driveUrlResponseList = await this.submitRequest<List<SkyDriveUrlResponse>>(url, "POST", content).ConfigureAwait(false);
        SkyDriveUrl skyDriveUrl;
        if (driveUrlResponseList != null)
          skyDriveUrl = this.mapToSkyDriveUrl(driveUrlResponseList[0]);
        skyDriveUrlForPut = skyDriveUrl;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveUrlForPut. Ex: " + (object) ex);
        throw;
      }
      return skyDriveUrlForPut;
    }

    public async Task<SkyDriveUrl> GetSkyDriveUrlForMeta(string objectId, string fileName = null)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveUrlForMeta");
      SkyDriveUrl skyDriveUrlForMeta;
      try
      {
        string str = string.IsNullOrEmpty(fileName) || !fileName.Equals("document.json") ? "urn:elli:service:sc:attachment:transformation" : (string) null;
        List<SkyDriveUrlResponse> driveUrlResponseList = await this.submitRequest<List<SkyDriveUrlResponse>>(EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/urlGenerator?action=meta", "POST", JsonConvert.SerializeObject((object) new GetSkyDriveUrlForMetaRequest()
        {
          expires = 900,
          meta = new Meta() { policyId = str },
          objectId = objectId
        })).ConfigureAwait(false);
        SkyDriveUrl skyDriveUrl;
        if (driveUrlResponseList != null)
          skyDriveUrl = this.mapToSkyDriveUrl(driveUrlResponseList[0]);
        skyDriveUrlForMeta = skyDriveUrl;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveUrlForMeta. Ex: " + (object) ex);
        throw;
      }
      return skyDriveUrlForMeta;
    }

    public async Task<SkyDriveUrl> GetSkyDriveUrlForGet(string objectId)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveUrlForGet");
      SkyDriveUrl skyDriveUrlForGet;
      try
      {
        List<SkyDriveUrlResponse> driveUrlResponseList = await this.submitRequest<List<SkyDriveUrlResponse>>(EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/urlGenerator?action=get", "POST", JsonConvert.SerializeObject((object) new GetSkyDriveUrlForGetRequest()
        {
          expires = 900,
          sub = this._sub,
          objectIds = new string[1]{ objectId }
        })).ConfigureAwait(false);
        SkyDriveUrl skyDriveUrl;
        if (driveUrlResponseList != null)
          skyDriveUrl = this.mapToSkyDriveUrl(driveUrlResponseList[0]);
        skyDriveUrlForGet = skyDriveUrl;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveUrlForGet. Ex: " + (object) ex);
        throw;
      }
      return skyDriveUrlForGet;
    }

    public async Task<List<SkyDriveUrl>> GetSkyDriveUrlsForGet(string[] objectIds)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveUrlForGet");
      List<SkyDriveUrl> skyDriveUrlsForGet;
      try
      {
        List<SkyDriveUrlResponse> driveUrlResponseList = await this.submitRequest<List<SkyDriveUrlResponse>>(EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/urlGenerator?action=get", "POST", JsonConvert.SerializeObject((object) new GetSkyDriveUrlForGetRequest()
        {
          expires = 900,
          sub = this._sub,
          objectIds = objectIds
        })).ConfigureAwait(false);
        List<SkyDriveUrl> skyDriveUrlList = new List<SkyDriveUrl>();
        if (driveUrlResponseList != null)
        {
          foreach (SkyDriveUrlResponse response in driveUrlResponseList)
            skyDriveUrlList.Add(this.mapToSkyDriveUrl(response));
        }
        skyDriveUrlsForGet = skyDriveUrlList;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveUrlForGet. Ex: " + (object) ex);
        throw;
      }
      return skyDriveUrlsForGet;
    }

    public async Task<SkyDriveUrl> GetSkyDriveUrlForGet(string loanId, string fileKey)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveUrlForGet");
      SkyDriveUrl skyDriveUrlForGet;
      try
      {
        string url = EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/urlGenerator?action=get";
        GetSkyDriveUrlForGetRequest urlForGetRequest = new GetSkyDriveUrlForGetRequest();
        urlForGetRequest.expires = 900;
        urlForGetRequest.sub = this._sub;
        urlForGetRequest.files = new SkyDriveFile[1]
        {
          new SkyDriveFile()
          {
            fileKey = fileKey,
            owner = new Owner()
            {
              entityId = Guid.Parse(loanId).ToString(),
              entityType = "urn:elli:encompass:loanId"
            }
          }
        };
        string content = JsonConvert.SerializeObject((object) urlForGetRequest);
        List<SkyDriveUrlResponse> driveUrlResponseList = await this.submitRequest<List<SkyDriveUrlResponse>>(url, "POST", content).ConfigureAwait(false);
        SkyDriveUrl skyDriveUrl;
        if (driveUrlResponseList != null)
          skyDriveUrl = this.mapToSkyDriveUrl(driveUrlResponseList[0]);
        skyDriveUrlForGet = skyDriveUrl;
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveUrlForGet. Ex: " + (object) ex);
        throw;
      }
      return skyDriveUrlForGet;
    }

    private SkyDriveUrl mapToSkyDriveUrl(SkyDriveUrlResponse response)
    {
      return new SkyDriveUrl(response.id, response.url, "elli-signature " + response.elliSignature);
    }

    public async Task<string[]> GetSkyDriveFileKeys(string loanId)
    {
      this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Entering GetSkyDriveFileKeys");
      string[] array;
      try
      {
        List<SkyDriveFileKey> skyDriveFileKeyList = await this.submitRequest<List<SkyDriveFileKey>>(EnConfigurationSettings.AppSettings["SkyDrive.Url"] + "/v1/operators/search", "POST", JsonConvert.SerializeObject((object) new GetSkyDriveFileKeysRequest()
        {
          meta = new Meta()
          {
            owner = new Owner()
            {
              entityId = Guid.Parse(loanId).ToString(),
              entityType = "urn:elli:encompass:loanId"
            }
          }
        })).ConfigureAwait(false);
        List<string> stringList = new List<string>();
        if (skyDriveFileKeyList != null)
        {
          foreach (SkyDriveFileKey skyDriveFileKey in skyDriveFileKeyList)
          {
            if (!string.IsNullOrEmpty(skyDriveFileKey.fileKey))
              stringList.Add(skyDriveFileKey.fileKey);
          }
        }
        array = stringList.ToArray();
      }
      catch (Exception ex)
      {
        this._context.TraceLog.Write(TraceLevel.Error, nameof (SkyDriveRestClient), "Error in GetSkyDriveFileKeys. Ex: " + (object) ex);
        throw;
      }
      return array;
    }

    private async Task<T> submitRequest<T>(string url, string method, string content)
    {
      T result = default (T);
      AccessToken jwt = await new OAuth2Client(this._context, url).GetJWT("urn:elli:service:skydrive", "apiplatform", this._claims);
      using (HttpRequestMessage request = new HttpRequestMessage())
      {
        request.Method = new HttpMethod(method);
        request.RequestUri = new Uri(jwt.ServiceUrl);
        request.Headers.Add("Authorization", jwt.TypeAndToken);
        request.Headers.Add("Accept", "application/json");
        if (!string.IsNullOrEmpty(content))
          request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
        this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "Calling SendAsync: " + request.RequestUri.ToString() + " Payload: " + content);
        using (HttpResponseMessage response = await SkyDriveRestClient._client.SendAsync(request).ConfigureAwait(false))
        {
          this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
          if (response.IsSuccessStatusCode)
          {
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "SendAsync Response Content: " + str);
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
                this._context.TraceLog.Write(TraceLevel.Verbose, nameof (SkyDriveRestClient), "SendAsync Response Content: " + str);
                SkyDriveError skyDriveError = JsonConvert.DeserializeObject<SkyDriveError>(str);
                message = message + " " + skyDriveError.Code + " " + skyDriveError.Summary + " " + skyDriveError.Details;
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
      return result;
    }
  }
}
