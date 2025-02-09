// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.SkyDrive.SkyDriveStreamingClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Services;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.SkyDrive
{
  public class SkyDriveStreamingClient
  {
    private const string className = "SkyDriveStreamingClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private const int _DefaultCopyBufferSize = 81920;
    private readonly int _DownloadBufferSize;
    private readonly int _UploadBufferSize;
    private ILoan _Loan;
    private static HttpClient client = new HttpClient();

    public event DownloadProgressEventHandler DownloadProgress;

    public event DownloadProgressEventHandler UploadProgress;

    static SkyDriveStreamingClient()
    {
      SkyDriveStreamingClient.client.Timeout = TimeSpan.FromMinutes(15.0);
    }

    public SkyDriveStreamingClient(ILoan loan)
    {
      this._Loan = loan;
      this._DownloadBufferSize = 81920;
      this._UploadBufferSize = 81920;
    }

    public SkyDriveStreamingClient(ILoan loan, ISessionStartupInfo startupInfo)
      : this(loan)
    {
      IDictionary efolderSettings = startupInfo.EFolderSettings;
      if (efolderSettings == null)
        return;
      if (efolderSettings.Contains((object) "eFolder.DownloadBufferSize"))
        this._DownloadBufferSize = Convert.ToInt32(efolderSettings[(object) "eFolder.DownloadBufferSize"]);
      if (!efolderSettings.Contains((object) "eFolder.UploadBufferSize"))
        return;
      this._UploadBufferSize = Convert.ToInt32(efolderSettings[(object) "eFolder.UploadBufferSize"]);
    }

    public SkyDriveStreamingClient(LoanDataMgr loanDataMgr)
      : this(loanDataMgr.LoanObject, loanDataMgr.SessionObjects.StartupInfo)
    {
    }

    public async Task<BinaryObject> GetObject(string objectId)
    {
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Calling GetSkyDriveUrlForObject: " + objectId);
      SkyDriveUrl driveUrlForObject = this._Loan.GetSkyDriveUrlForObject(objectId);
      if (driveUrlForObject == null)
      {
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "File Not Found: " + objectId);
        return (BinaryObject) null;
      }
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Downloading File: " + objectId);
      string path = await this.DownloadFile(driveUrlForObject, objectId).ConfigureAwait(false);
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Returning BinaryObject: " + objectId);
      return new BinaryObject(path, false);
    }

    public async Task<BinaryObject> GetSupportingData(string fileKey)
    {
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Calling GetSkyDriveUrlForGet: " + fileKey);
      SkyDriveUrl skyDriveUrlForGet = this._Loan.GetSkyDriveUrlForGet(fileKey);
      if (skyDriveUrlForGet == null)
      {
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "File Not Found: " + fileKey);
        return (BinaryObject) null;
      }
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Downloading File: " + fileKey);
      string path = await this.DownloadFile(skyDriveUrlForGet, fileKey).ConfigureAwait(false);
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Returning BinaryObject: " + fileKey);
      return new BinaryObject(path, false);
    }

    public async Task<string> SaveSupportingData(
      string fileKey,
      BinaryObject data,
      bool useSkyDriveClassic = false)
    {
      string contentType = FileContentTypes.GetContentType(fileKey);
      if (contentType == "application/pdf")
      {
        try
        {
          Convert.FromBase64String(data.ToString());
          contentType = "base64/pdf";
        }
        catch
        {
        }
      }
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Calling GetSkyDriveUrlForPut: " + fileKey);
      SkyDriveUrl presigned = this._Loan.GetSkyDriveUrlForPut(fileKey, contentType, useSkyDriveClassic);
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Uploading File: " + fileKey);
      string str = await this.uploadFile(presigned, data, contentType, 0L, 0L).ConfigureAwait(false);
      return presigned.id;
    }

    public SkyDriveUrl GetPresignedUrlForPartnerFilesUpload(string objectId, string fileName = null)
    {
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: Calling GetSkyDriveUrlForMeta: " + objectId);
      return this._Loan.GetSkyDriveUrlForMeta(objectId, fileName);
    }

    public SkyDriveUrl GetPresignedUrlForPartnerFilesDownload(string objectId)
    {
      try
      {
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: Calling GetSkyDriveUrlForObject for partner files with ObjectId: " + objectId);
        SkyDriveUrl driveUrlForObject = this._Loan.GetSkyDriveUrlForObject(objectId);
        if (driveUrlForObject != null)
          return driveUrlForObject;
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: File Not Found for ObjectId: " + objectId);
        return (SkyDriveUrl) null;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClientSkyDriveClassic: Error in GetPresignedUrlForPartnerFilesDownload. Ex: " + (object) ex);
        throw;
      }
    }

    public async Task<BinaryObject> GetPartnerFile(SkyDriveUrl presigned, string fileKey)
    {
      BinaryObject partnerFile;
      try
      {
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: Downloading Partner File: " + fileKey);
        string path = await this.DownloadFile(presigned, fileKey).ConfigureAwait(false);
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: Returning BinaryObject: " + fileKey);
        partnerFile = new BinaryObject(path, false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClientSkyDriveClassic: Error in GetPartnerFile. Ex: " + (object) ex);
        throw;
      }
      return partnerFile;
    }

    public async Task<string> SavePartnerFile(
      SkyDriveUrl presigned,
      string contentType,
      BinaryObject data)
    {
      string id;
      try
      {
        if (contentType == "application/pdf")
        {
          try
          {
            Convert.FromBase64String(data.ToString());
            contentType = "base64/pdf";
          }
          catch
          {
          }
        }
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SkyDriveClassic: Uploading Partner File with Object Id: " + presigned.id);
        string str = await this.uploadFile(presigned, data, contentType, 0L, 0L).ConfigureAwait(false);
        id = presigned.id;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClientSkyDriveClassic: Error in SavePartnerFile. Ex: " + (object) ex);
        throw;
      }
      return id;
    }

    public async Task<string> UploadAttachment(
      CreateAttachmentUrlResponse eosResponse,
      BinaryObject data,
      string contentType)
    {
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Entering UploadAttachment");
      try
      {
        if (eosResponse.multiChunkRequired)
        {
          Stream dataStream = data.AsStream();
          long sentOffset = 0;
          long totalBytes = data.Length;
          Chunk[] chunkArray = eosResponse.multiChunk.chunkList;
          for (int index = 0; index < chunkArray.Length; ++index)
          {
            Chunk chunk = chunkArray[index];
            SkyDriveUrl presigned = new SkyDriveUrl(eosResponse.attachmentId, chunk.uploadUrl, string.Empty);
            byte[] buffer = new byte[chunk.size];
            Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Populating Buffer: " + (object) chunk.size);
            int num = await dataStream.ReadAsync(buffer, 0, chunk.size).ConfigureAwait(false);
            Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Creating Chunk");
            using (BinaryObject chunkData = new BinaryObject(buffer))
            {
              Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Uploading Chunk");
              string str = await this.uploadFile(presigned, chunkData, contentType, sentOffset, totalBytes).ConfigureAwait(false);
            }
            sentOffset += (long) chunk.size;
            presigned = (SkyDriveUrl) null;
            buffer = (byte[]) null;
            chunk = (Chunk) null;
          }
          chunkArray = (Chunk[]) null;
          using (await this.submitRequest(eosResponse.multiChunk.commitUrl, (string) null, "POST", "application/json", (HttpContent) null).ConfigureAwait(false))
            return eosResponse.attachmentId;
        }
        else
        {
          SkyDriveUrl presigned = new SkyDriveUrl(eosResponse.attachmentId, eosResponse.uploadUrl, eosResponse.authorizationHeader);
          Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Uploading File");
          string str = await this.uploadFile(presigned, data, contentType, 0L, 0L).ConfigureAwait(false);
          return eosResponse.attachmentId;
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClient: Error in UploadAttachment. Ex: " + (object) ex);
        throw;
      }
    }

    private async Task<string> uploadFile(
      SkyDriveUrl presigned,
      BinaryObject data,
      string contentType,
      long sentOffset,
      long totalBytes)
    {
      SkyDriveStreamingClient driveStreamingClient = this;
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Entering uploadFile");
      string id;
      try
      {
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Creating BinaryObjectContent");
        using (BinaryObjectContent content = new BinaryObjectContent(data, driveStreamingClient._UploadBufferSize, sentOffset, totalBytes))
        {
          content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
          content.UploadProgress += new DownloadProgressEventHandler(driveStreamingClient.Content_UploadProgress);
          try
          {
            long startTicks = DateTime.Now.Ticks;
            using (await driveStreamingClient.submitRequest(presigned.url, presigned.authorizationHeader, "PUT", "application/json", (HttpContent) content).ConfigureAwait(false))
            {
              TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - startTicks);
              RemoteLogger.Write(TraceLevel.Info, "SkyDriveStreamingClient: File Uploaded:" + presigned.id + " Size:" + (object) data.Length + " Time:" + (object) Math.Round(timeSpan.TotalMilliseconds));
              id = presigned.id;
            }
          }
          finally
          {
            content.UploadProgress -= new DownloadProgressEventHandler(driveStreamingClient.Content_UploadProgress);
          }
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClient: Error in uploadFile. Ex: " + (object) ex);
        throw;
      }
      return id;
    }

    public async Task<string> DownloadFile(
      SkyDriveUrl url,
      string fileKey,
      bool useResponseFileInfo = false)
    {
      SkyDriveStreamingClient source = this;
      Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Entering DownloadFile");
      string str;
      try
      {
        long startTicks = DateTime.Now.Ticks;
        using (HttpResponseMessage response = await source.submitRequest(url.url, url.authorizationHeader, "GET", (string) null, (HttpContent) null).ConfigureAwait(false))
        {
          if (useResponseFileInfo)
          {
            MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
            if (contentType != null && !string.IsNullOrEmpty(contentType.MediaType))
              fileKey = Path.ChangeExtension(fileKey, FileContentTypes.GetExtension(contentType.MediaType));
          }
          Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Getting ContentLength");
          long? totalBytes = response.Content.Headers.ContentLength;
          Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Calling Content.ReadAsStreamAsync");
          using (Stream httpStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
          {
            string filePath = SystemSettings.GetTempFileName(fileKey);
            Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Creating FileStream: " + filePath);
            using (FileStream fileStream = System.IO.File.Create(filePath, source._DownloadBufferSize, FileOptions.Asynchronous))
            {
              byte[] buffer = new byte[source._DownloadBufferSize];
              int bytesRead = 0;
              long bytesWritten = 0;
              do
              {
                bytesRead = await httpStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                if (bytesRead > 0)
                {
                  await fileStream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                  bytesWritten += (long) bytesRead;
                  if (source.DownloadProgress != null && totalBytes.HasValue)
                  {
                    DownloadProgressEventArgs e = new DownloadProgressEventArgs(bytesWritten, totalBytes.Value);
                    source.DownloadProgress((object) source, e);
                    if (e.Cancel)
                      throw new CanceledOperationException();
                  }
                }
              }
              while (bytesRead > 0);
              if (string.IsNullOrEmpty(url.id))
                url.id = fileKey;
              TimeSpan timeSpan = new TimeSpan(DateTime.Now.Ticks - startTicks);
              RemoteLogger.Write(TraceLevel.Info, "SkyDriveStreamingClient: File Downloaded:" + url.id + " Size:" + (object) bytesWritten + " Time:" + (object) Math.Round(timeSpan.TotalMilliseconds));
              buffer = (byte[]) null;
            }
            str = filePath;
          }
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, "SkyDriveStreamingClient: Error in DownloadFile. Ex: " + (object) ex);
        throw;
      }
      return str;
    }

    private async Task<HttpResponseMessage> submitRequest(
      string url,
      string authorizationHeader,
      string method,
      string acceptHeader,
      HttpContent content)
    {
      HttpResponseMessage httpResponseMessage;
      using (HttpRequestMessage request = new HttpRequestMessage())
      {
        request.Method = new HttpMethod(method);
        request.RequestUri = new Uri(url);
        if (!string.IsNullOrEmpty(authorizationHeader))
          request.Headers.Add("Authorization", authorizationHeader);
        if (!string.IsNullOrEmpty(acceptHeader))
          request.Headers.Add("Accept", acceptHeader);
        if (content != null)
          request.Content = content;
        ServicePoint servicePoint = ServicePointManager.FindServicePoint(request.RequestUri);
        servicePoint.ConnectionLeaseTimeout = 60000;
        if (servicePoint.ConnectionLimit < 25)
          servicePoint.ConnectionLimit = 25;
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "Calling SendAsync: " + request.RequestUri.ToString());
        HttpResponseMessage response = await SkyDriveStreamingClient.client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
        Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
          httpResponseMessage = response;
        }
        else
        {
          if (response.StatusCode == HttpStatusCode.NotFound)
            throw new HttpException((int) response.StatusCode, "Not Found");
          if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new HttpException((int) response.StatusCode, "Unauthorized Request");
          string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
          string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
          Tracing.Log(SkyDriveStreamingClient.sw, TraceLevel.Verbose, nameof (SkyDriveStreamingClient), "SendAsync Response Content: " + str);
          try
          {
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
      return httpResponseMessage;
    }

    private void Content_UploadProgress(object sender, DownloadProgressEventArgs e)
    {
      if (this.UploadProgress == null)
        return;
      this.UploadProgress((object) this, e);
    }
  }
}
