// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.EOSClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.Files;
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
  public class EOSClient
  {
    private const string className = "EOSClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc�";
    private static HttpClient _client = new HttpClient();
    private readonly OAuth2Utils _oauth;
    private readonly string _loanId;
    private readonly string _lockId;

    public bool IsClassificationFailed { get; set; }

    public EOSClient(LoanDataMgr loanDataMgr)
    {
      this._loanId = loanDataMgr.LoanData.GUID;
      this._lockId = loanDataMgr.SessionObjects.SessionID;
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
    }

    public async Task<CreateAttachmentUrlResponse> CreateAttachmentUrl(
      AddReasonType reason,
      string fileName,
      string title,
      long fileSize,
      string contentType)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CreateAttachmentUrl");
      CreateAttachmentUrlResponse attachmentUrl;
      try
      {
        switch (reason)
        {
          case AddReasonType.SDKImage:
            reason = AddReasonType.SDK;
            break;
          case AddReasonType.ScanDoc:
            reason = AddReasonType.Scan;
            break;
        }
        string url = "{host}/encompass/v3/loans/{loanId}/attachmentUploadUrl?context={context}".Replace("{loanId}", Guid.Parse(this._loanId).ToString()).Replace("{context}", reason.ToString());
        if (!string.IsNullOrEmpty(this._lockId))
          url = url + "&lockId=" + Guid.Parse(this._lockId).ToString();
        string content = JsonConvert.SerializeObject((object) new CreateAttachmentUrlRequest()
        {
          title = title,
          file = new FileDetails()
          {
            name = fileName,
            size = fileSize,
            contentType = contentType
          }
        });
        attachmentUrl = await this.submitRequest<CreateAttachmentUrlResponse>(url, "POST", content).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateAttachmentUrl. Ex: " + (object) ex);
        throw;
      }
      return attachmentUrl;
    }

    public async Task<DragDropJobStatusResponse> CheckDragDropJobStatus(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CheckDragDropJobStatus");
      DragDropJobStatusResponse jobStatusResponse1;
      try
      {
        DragDropJobStatusResponse jobStatusResponse2 = await this.submitRequest<DragDropJobStatusResponse>("{host}/efolder/v1/dragdropjobs/{jobId}?presign=true".Replace("{jobId}", jobId), "GET", (string) null).ConfigureAwait(false);
        if (string.Equals(jobStatusResponse2.status, "Failed", StringComparison.InvariantCultureIgnoreCase))
          RemoteLogger.Write(TraceLevel.Error, "EOSClient: CheckDragDropJobStatus returned Failed status.");
        jobStatusResponse1 = jobStatusResponse2;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateExportJob. Ex: " + (object) ex);
        throw;
      }
      return jobStatusResponse1;
    }

    public async Task<string> CreateExportJob(
      FileAttachment[] fileAttachments,
      AnnotationExportType annotationExportType)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CreateExportJob");
      string jobId;
      try
      {
        string url = "{host}/efolder/v1/exportjobs";
        List<Entity> entityList = new List<Entity>();
        foreach (FileAttachment fileAttachment in fileAttachments)
          entityList.Add(new Entity()
          {
            entityId = fileAttachment.ID,
            entityType = "Attachment"
          });
        CreateExportJobRequest exportJobRequest = new CreateExportJobRequest()
        {
          source = new Entity()
          {
            entityId = Guid.Parse(this._loanId).ToString(),
            entityType = "loan"
          },
          entities = entityList.ToArray()
        };
        if (annotationExportType != AnnotationExportType.None)
        {
          string[] strArray = new string[1]{ string.Empty };
          switch (annotationExportType)
          {
            case AnnotationExportType.All:
              strArray = new string[3]
              {
                "Private",
                "Internal",
                "Public"
              };
              break;
            case AnnotationExportType.Personal:
              strArray = new string[1]{ "Private" };
              break;
            case AnnotationExportType.Public:
              strArray = new string[1]{ "Public" };
              break;
          }
          exportJobRequest.annotationSettings = new AnnotationSettings()
          {
            visibility = strArray
          };
        }
        string content = JsonConvert.SerializeObject((object) exportJobRequest);
        jobId = (await this.submitRequest<CreateExportJobResponse>(url, "POST", content).ConfigureAwait(false)).jobId;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateExportJob. Ex: " + (object) ex);
        throw;
      }
      return jobId;
    }

    public async Task<SkyDriveUrl> CheckExportJobStatus(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CheckExportJobStatus");
      try
      {
        ExportJobStatusResponse jobStatusResponse = await this.submitRequest<ExportJobStatusResponse>("{host}/efolder/v1/exportjobs/{jobId}".Replace("{jobId}", jobId), "GET", (string) null).ConfigureAwait(false);
        if (string.Equals(jobStatusResponse.status, "Success", StringComparison.InvariantCultureIgnoreCase))
          return new SkyDriveUrl(jobStatusResponse.file.entityId, jobStatusResponse.file.entityUri, jobStatusResponse.file.authorizationHeader);
        if (string.Equals(jobStatusResponse.status, "Failed", StringComparison.InvariantCultureIgnoreCase))
          throw new Exception("Export Job failed with error code: " + jobStatusResponse.error.code);
        return (SkyDriveUrl) null;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateExportJob. Ex: " + (object) ex);
        throw;
      }
    }

    public async Task<CreateMergeJobResponse> CreateMergeJob(FileAttachment[] fileAttachments)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CreateMergeJob");
      CreateMergeJobResponse mergeJob;
      try
      {
        string url = "{host}/efolder/v2/mergejobs";
        List<string> stringList = new List<string>();
        foreach (FileAttachment fileAttachment in fileAttachments)
          stringList.Add(fileAttachment.ID);
        List<attachments> attachmentsList = new List<attachments>();
        foreach (FileAttachment fileAttachment in fileAttachments)
          attachmentsList.Add(new attachments(fileAttachment.ID));
        string content = JsonConvert.SerializeObject((object) new CreateMergeJobRequest()
        {
          attachments = attachmentsList,
          source = new Entity()
          {
            entityId = Guid.Parse(this._loanId).ToString(),
            entityType = "loan"
          }
        });
        mergeJob = await this.submitRequest<CreateMergeJobResponse>(url, "POST", content).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateMergeJob. Ex: " + (object) ex);
        throw;
      }
      return mergeJob;
    }

    public async Task<SkyDriveUrl> CheckMergeJobStatus(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CheckMergeJobStatus");
      try
      {
        MergeJobStatusResponse jobStatusResponse = await this.submitRequest<MergeJobStatusResponse>("{host}/efolder/v2/mergejobs/{jobId}".Replace("{jobId}", jobId), "GET", (string) null).ConfigureAwait(false);
        if (string.Equals(jobStatusResponse.status, "Success", StringComparison.InvariantCultureIgnoreCase))
          return new SkyDriveUrl(jobStatusResponse.file.entityId, jobStatusResponse.file.entityUri, jobStatusResponse.file.authorizationHeader);
        if (string.Equals(jobStatusResponse.status, "Failed", StringComparison.InvariantCultureIgnoreCase))
          throw new Exception("Merge Job failed with error code: " + jobStatusResponse.error.code);
        return (SkyDriveUrl) null;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CheckMergeJobStatus. Ex: " + (object) ex);
        throw;
      }
    }

    public async Task<string> CommitMergeJob(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CommitMergeJob");
      string attachmentId;
      try
      {
        string url = "{host}/efolder/v2/mergejobs/{jobId}?action=commit".Replace("{jobId}", jobId);
        if (!string.IsNullOrEmpty(this._lockId))
          url = url + "&lockId=" + Guid.Parse(this._lockId).ToString();
        string content = JsonConvert.SerializeObject((object) new CommitMergeAttachmentRequest()
        {
          create = new create() { title = "Merged File" }
        });
        attachmentId = (await this.submitRequest<CommitMergeJobResponse>(url, "PATCH", content).ConfigureAwait(false)).attachmentId;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CommitMergeJob. Ex: " + (object) ex);
        throw;
      }
      return attachmentId;
    }

    public async Task<string> CreateSplitJob(string attachmentId, SplitFile[] files)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CreateSplitJob");
      string jobId;
      try
      {
        jobId = (await this.submitRequest<CreateSplitJobResponse>("{host}/efolder/v1/splitjobs", "POST", JsonConvert.SerializeObject((object) new CreateSplitJobRequest()
        {
          source = new Entity()
          {
            entityId = Guid.Parse(this._loanId).ToString(),
            entityType = "loan"
          },
          attachmentId = attachmentId,
          files = files
        })).ConfigureAwait(false)).jobId;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateSplitJob. Ex: " + (object) ex);
        throw;
      }
      return jobId;
    }

    public async Task<SplitJobStatusResponse> CheckSplitJobStatus(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CheckSplitJobStatus");
      try
      {
        SplitJobStatusResponse jobStatusResponse = await this.submitRequest<SplitJobStatusResponse>("{host}/efolder/v1/splitjobs/{jobId}".Replace("{jobId}", jobId), "GET", (string) null).ConfigureAwait(false);
        if (string.Equals(jobStatusResponse.status, "Success", StringComparison.InvariantCultureIgnoreCase))
          return jobStatusResponse;
        if (string.Equals(jobStatusResponse.status, "Failed", StringComparison.InvariantCultureIgnoreCase))
          throw new Exception("Split Job failed with error code: " + jobStatusResponse.error.code);
        return (SplitJobStatusResponse) null;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CheckSplitJobStatus. Ex: " + (object) ex);
        throw;
      }
    }

    public async Task<CommitSplitJobResponse> CommitSplitJob(
      string jobId,
      CommitAttachmentRequest[] commitList)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CommitSplitJob");
      CommitSplitJobResponse splitJobResponse;
      try
      {
        string url = "{host}/efolder/v1/splitjobs/{jobId}?action=commit".Replace("{jobId}", jobId);
        if (!string.IsNullOrEmpty(this._lockId))
          url = url + "&lockId=" + Guid.Parse(this._lockId).ToString();
        string content = JsonConvert.SerializeObject((object) new CommitSplitJobRequest()
        {
          attachments = commitList
        });
        splitJobResponse = await this.submitRequest<CommitSplitJobResponse>(url, "PATCH", content).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CommitSplitJob. Ex: " + (object) ex);
        throw;
      }
      return splitJobResponse;
    }

    public async Task<string> CreateDocClassificationJob(string[] attachmentIdLst)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CreateDocClassificationJob");
      string classificationJob;
      try
      {
        string url = "{host}/efolder/v1/classificationJobs?lockId={lockId}".Replace("{lockId}", Convert.ToString((object) Guid.Parse(this._lockId)));
        List<FileEntity> fileEntityList = new List<FileEntity>();
        foreach (string str in attachmentIdLst)
          fileEntityList.Add(new FileEntity() { id = str });
        string content = JsonConvert.SerializeObject((object) new CreateClassificationJobRequest()
        {
          loan = new LoanEntity()
          {
            id = Guid.Parse(this._loanId).ToString()
          },
          files = fileEntityList,
          autoAssign = true,
          options = new Engines()
          {
            useBarcode = true,
            useTextIndex = true,
            useADR = false
          }
        });
        string str1 = await this.CreateClassifcationJob(url, "POST", content).ConfigureAwait(false);
        string str2;
        if (string.IsNullOrEmpty(str1))
          str2 = string.Empty;
        else
          str2 = ((IEnumerable<string>) str1.Split('/')).Last<string>();
        string str3 = str2;
        Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateDocClassificationJob :: Job Id : " + str3);
        classificationJob = str3;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CreateDocClassificationJob. Ex: " + (object) ex);
        this.IsClassificationFailed = true;
        throw;
      }
      return classificationJob;
    }

    public async Task<GetClassificationJobResponse> CheckDocClassificationJobStatus(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CheckDocClassificationJobStatus :: Entering CheckDocClassificationJobStatus");
      try
      {
        GetClassificationJobResponse classificationJobResponse = await this.submitRequest<GetClassificationJobResponse>("{host}/efolder/v1/classificationJobs/{jobId}".Replace("{jobId}", jobId), "GET", (string) null).ConfigureAwait(false);
        return string.Equals(classificationJobResponse.status, "Success", StringComparison.InvariantCultureIgnoreCase) || !string.Equals(classificationJobResponse.status, "Failed", StringComparison.InvariantCultureIgnoreCase) ? classificationJobResponse : throw new Exception("Classification Job failed with error code: " + classificationJobResponse?.error?.code);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CheckDocClassificationJobStatus. Ex: " + (object) ex);
        this.IsClassificationFailed = true;
        throw;
      }
    }

    public async Task CancelDocClassificationJob(string jobId)
    {
      Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Entering CancelDocClassificationJob");
      try
      {
        string str = await this.submitRequest<string>("{host}/efolder/v1/classificationJobs/{jobId}".Replace("{jobId}", jobId), "PATCH", JsonConvert.SerializeObject((object) new CancelClassificationJobRequest()
        {
          status = "Cancelled"
        })).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "EOSClient: Error in CancelDocClassificationJob. Ex: " + (object) ex);
        throw;
      }
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
          Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await EOSClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              IEnumerable<string> values;
              response.Headers.TryGetValues("X-Correlation-ID", out values);
              if (values != null)
                Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "Co-relation Id EOS Client: " + values.FirstOrDefault<string>());
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "SendAsync Response Content at: " + DateTime.Now.ToString() + ":" + str);
              result = JsonConvert.DeserializeObject<T>(str);
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
              try
              {
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "SendAsync Response Content: " + str);
                EOSError eosError = JsonConvert.DeserializeObject<EOSError>(str);
                message = message + " " + eosError.code + " " + eosError.summary + " " + eosError.details;
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

    private async Task<string> CreateClassifcationJob(string url, string method, string content)
    {
      string result = string.Empty;
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
          Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateClassifcationJob :: Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await EOSClient._client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateClassifcationJob :: SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              IEnumerable<string> values1;
              response.Headers.TryGetValues("X-Correlation-ID", out values1);
              if (values1 != null)
                Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateClassifcationJob :: Co-relation Id: " + values1.FirstOrDefault<string>());
              IEnumerable<string> values2;
              response.Headers.TryGetValues("Location", out values2);
              if (values2 != null)
                result = values2.FirstOrDefault<string>();
              Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateClassifcationJob :: Location Header: " + result);
            }
            else
            {
              if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new HttpException((int) response.StatusCode, "Unauthorized Request");
              string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
              try
              {
                string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Tracing.Log(EOSClient.sw, TraceLevel.Verbose, nameof (EOSClient), "CreateClassifcationJob :: SendAsync Response Content: " + str);
                EOSError eosError = JsonConvert.DeserializeObject<EOSError>(str);
                message = message + " " + eosError.code + " " + eosError.summary + " " + eosError.details;
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
