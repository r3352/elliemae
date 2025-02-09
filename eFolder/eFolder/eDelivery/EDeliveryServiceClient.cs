// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.EDeliveryServiceClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Authentication;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class EDeliveryServiceClient
  {
    private const string className = "EDeliveryServiceClient";
    private const string UpdatePackageUrl = "{eDeliveryServiceUrl}/loans/{loanGuid}/packages/{packageId}/recipients/{recipientId}/tasks/{taskId}";
    private const string GetPackageDetailsUrl = "{host}/ellisign/v3/loans/{loanId}/packages/{packageId}";
    private const string MediaServerURLForDownload = "{host}/ellisign/v3/loans/{loanGuid}/packages/{packageId}/documents/{documentId}";
    private static readonly string sw = Tracing.SwEFolder;
    private const string _scope = "sc";
    private Sessions.Session session;
    private string eDeliveryServiceUrl;
    private static HttpClient client = new HttpClient();

    public OAuth2Utils _oauth { get; }

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public EDeliveryServiceClient()
      : this(Session.DefaultInstance)
    {
      this.eDeliveryServiceUrl = "https://{host}/ellisign/v2";
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.StartupInfo.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
    }

    public EDeliveryServiceClient(Sessions.Session session) => this.session = session;

    public EDeliveryServiceClient(LoanDataMgr loanDataMgr)
    {
      this._oauth = new OAuth2Utils(loanDataMgr.SessionObjects.Session, loanDataMgr.SessionObjects.StartupInfo);
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.StartupInfo.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
    }

    public async Task<string> CreatePackage(Request request)
    {
      string result = (string) null;
      string status = "Calling CreatePackage";
      string package;
      using (PerformanceMeter.StartNew("EDlvSvcClntCrtPkgOtr", "DOCS EDelivertClient CreatePackage OUTER Asynch Thread", 96, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs"))
      {
        try
        {
          PerformanceMeter.Current.AddCheckpoint("BEGIN", 100, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<AccessToken, Task>) (async AccessToken =>
          {
            using (PerformanceMeter.StartNew("EDlvSvcClntCrtPkgInr", "DOCS EDelivertClient CreatePackage INNER Asynch Thread", 104, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs"))
            {
              try
              {
                PerformanceMeter.Current.AddCheckpoint("BEGIN", 108, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), status);
                UriBuilder eDeliveryUri = new UriBuilder("{eDeliveryServiceUrl}/loans/{loanGuid}/packages".Replace("{eDeliveryServiceUrl}", this.eDeliveryServiceUrl).Replace("{host}", AccessToken.HostName).Replace("{loanGuid}", request.applicationGroupId));
                EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Clear();
                EDeliveryServiceClient.client.DefaultRequestHeaders.Clear();
                EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                EDeliveryServiceClient.client.DefaultRequestHeaders.Add("X-Caller", "name=" + request.caller.name + ";version=" + request.caller.version);
                EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
                HttpResponseMessage response = await EDeliveryServiceClient.client.PostAsync(eDeliveryUri.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                  PerformanceMeter.Current.AddCheckpoint("Create Success", 125, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                  request.packageId = response.Headers.Location.ToString().Substring(response.Headers.Location.ToString().LastIndexOf("/") + 1);
                  result = request.packageId;
                  eDeliveryUri = (UriBuilder) null;
                  response = (HttpResponseMessage) null;
                }
                else
                {
                  PerformanceMeter.Current.AddCheckpoint("Create Failure - response.StatusCode: " + (object) response.StatusCode, 131, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                  if (response.StatusCode == HttpStatusCode.GatewayTimeout)
                  {
                    PerformanceMeter.Current.AddCheckpoint("HttpStatusCode.GatewayTimeout", 134, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                    Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Warning, nameof (EDeliveryServiceClient), "Trying to get package details for create package");
                    Thread.Sleep(30000);
                    PerformanceMeter.Current.AddCheckpoint("Sleep 30 seconds", 138, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                    HttpResponseMessage async = await EDeliveryServiceClient.client.GetAsync(eDeliveryUri.Uri);
                    PerformanceMeter.Current.AddCheckpoint("await async retry", 141, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                    if (async.IsSuccessStatusCode)
                    {
                      string str = this.LookForPackageCreation(await async.Content.ReadAsStringAsync(), request.custom["SynchronizationId"]);
                      if (!string.IsNullOrWhiteSpace(str))
                      {
                        result = str;
                        PerformanceMeter.Current.AddCheckpoint("EXIT RETURN", 151, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                        return;
                      }
                      Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), "Couldn't create a package after retry");
                      PerformanceMeter.Current.AddCheckpoint("FAIL Couldn't create a package after retry", 156, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                    }
                  }
                  EDeliveryServiceError innerException = JsonConvert.DeserializeObject<EDeliveryServiceError>(await response.Content.ReadAsStringAsync());
                  IEnumerable<string> values;
                  response.Headers.TryGetValues("X-Correlation-ID", out values);
                  if (values != null)
                  {
                    EDeliveryServiceError edeliveryServiceError = innerException;
                    edeliveryServiceError.summary = edeliveryServiceError.summary + Environment.NewLine + " CorrelationID = " + values.FirstOrDefault<string>();
                  }
                  PerformanceMeter.Current.AddCheckpoint("EXIT THROW HttpException", 168, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
                  throw new HttpException((int) response.StatusCode, innerException.Message, (Exception) innerException);
                }
              }
              finally
              {
                PerformanceMeter.Current.AddCheckpoint("END", 174, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
              }
            }
          })).ConfigureAwait(false);
          package = result;
        }
        catch (HttpException ex)
        {
          status = "Package creation failed";
          EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW HttpException", 188, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(innerException.GetDisplayErrorMessage());
        }
        catch (Exception ex)
        {
          if (ex is EDeliveryServiceError)
          {
            PerformanceMeter.Current.AddCheckpoint("EXIT THROW EDeliveryServiceError", 196, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
            throw new Exception(ex.Message);
          }
          status = "Package creation failed";
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), status + " Error details: " + ex.Message);
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW", 202, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(status);
        }
        finally
        {
          PerformanceMeter.Current.AddCheckpoint("END", 207, nameof (CreatePackage), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        }
      }
      return package;
    }

    public async void UpdatePackageTask(PackageTask request, bool suppressErrors = false)
    {
      string status = "Calling UpdatePackage";
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 220, nameof (UpdatePackageTask), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{eDeliveryServiceUrl}/loans/{loanGuid}/packages/{packageId}/recipients/{recipientId}/tasks/{taskId}".Replace("{eDeliveryServiceUrl}", this.eDeliveryServiceUrl).Replace("{host}", accessToken.HostName).Replace("{loanGuid}", request.ApplicationGroupId).Replace("{packageId}", request.PackageId).Replace("{recipientId}", request.RecipientId).Replace("{taskId}", request.TaskId));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("X-Caller", "name=" + request.Caller.name + ";version=" + request.Caller.version);
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.Type + " " + accessToken.Token);
          HttpResponseMessage response = await EDeliveryServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          if (!response.IsSuccessStatusCode && !suppressErrors)
          {
            EDeliveryServiceError innerException = JsonConvert.DeserializeObject<EDeliveryServiceError>(await response.Content.ReadAsStringAsync());
            IEnumerable<string> values;
            response.Headers.TryGetValues("X-Correlation-ID", out values);
            if (values != null)
            {
              EDeliveryServiceError edeliveryServiceError = innerException;
              edeliveryServiceError.summary = edeliveryServiceError.summary + Environment.NewLine + " CorrelationID = " + values.FirstOrDefault<string>();
            }
            throw new HttpException((int) response.StatusCode, innerException.Message, (Exception) innerException);
          }
        })).ConfigureAwait(false);
      }
      catch (HttpException ex)
      {
        status = "Error in service call to UpdatePackageTask.";
        EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
        string displayErrorMessage = innerException.GetDisplayErrorMessage();
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW " + displayErrorMessage, 264, nameof (UpdatePackageTask), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(displayErrorMessage);
      }
      catch (Exception ex)
      {
        if (ex is EDeliveryServiceError)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW EDeliveryServiceError", 271, nameof (UpdatePackageTask), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(ex.Message);
        }
        status = "Error in service call to UpdatePackageTask.";
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), status + " Error details: " + ex.Message);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW " + status, 277, nameof (UpdatePackageTask), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(status);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 282, nameof (UpdatePackageTask), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
      }
    }

    public string LookForPackageCreation(string getPackageResponse, string synchronizationId)
    {
      foreach (PackageDetails packageDetails in JsonConvert.DeserializeObject<List<PackageDetails>>(getPackageResponse))
      {
        if (packageDetails.custom != null && packageDetails.custom.SynchronizationId == synchronizationId)
          return packageDetails.id;
      }
      return (string) null;
    }

    public async Task<string> GenerateEnvelopeSignerURL(Request request, string recipientId)
    {
      string result = (string) null;
      string status = "Calling GenerateEnvelopeSignerURL method";
      string envelopeSignerUrl;
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 316, nameof (GenerateEnvelopeSignerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{eDeliveryServiceUrl}/loans/{loanGuid}/packages/{packageId}/recipients/{recipientId}/signingRoom".Replace("{eDeliveryServiceUrl}", this.eDeliveryServiceUrl).Replace("{host}", accessToken.HostName).Replace("{loanGuid}", request.applicationGroupId).Replace("{packageId}", request.packageId).Replace("{recipientId}", recipientId));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("X-Caller", "name=" + request.caller.name + ";version=" + request.caller.version);
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          HttpResponseMessage response = await EDeliveryServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) new GenerateEnvelopeSignerURLRequest()
          {
            returnUrl = "http://www.elliemae.com"
          }), Encoding.UTF8, "application/json"));
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GenerateEnvelopeSignerURLResponse>(await response.Content.ReadAsStringAsync()).signingRoomUrl;
          }
          else
          {
            EDeliveryServiceError innerException = JsonConvert.DeserializeObject<EDeliveryServiceError>(await response.Content.ReadAsStringAsync());
            IEnumerable<string> values;
            response.Headers.TryGetValues("X-Correlation-ID", out values);
            if (values != null)
            {
              EDeliveryServiceError edeliveryServiceError = innerException;
              edeliveryServiceError.summary = edeliveryServiceError.summary + Environment.NewLine + " CorrelationID = " + values.FirstOrDefault<string>();
            }
            throw new HttpException((int) response.StatusCode, innerException.Message, (Exception) innerException);
          }
        })).ConfigureAwait(false);
        envelopeSignerUrl = result;
      }
      catch (HttpException ex)
      {
        status = "Error in service call to GenerateEnvelopeSignerURL.";
        EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
        string displayErrorMessage = innerException.GetDisplayErrorMessage();
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + displayErrorMessage, 365, nameof (GenerateEnvelopeSignerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(displayErrorMessage);
      }
      catch (Exception ex)
      {
        if (ex is EDeliveryServiceError)
          throw new Exception(ex.Message);
        status = "Error in service call to GenerateEnvelopeSignerURL.";
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), status + " Error details: " + ex.Message);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + status, 378, nameof (GenerateEnvelopeSignerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(status);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 383, nameof (GenerateEnvelopeSignerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
      }
      return envelopeSignerUrl;
    }

    public async Task<string> GenerateEnvelopeOriginatorSignerURL(Request request)
    {
      string empty = string.Empty;
      Recipient recipient = request.recipients.Find((Predicate<Recipient>) (x => x.role == "Originator"));
      return recipient == null ? empty : await this.GenerateEnvelopeSignerURL(request, recipient.id);
    }

    public async Task<string> GenerateMediaServerURL(Request request)
    {
      HttpResponseMessage response = (HttpResponseMessage) null;
      string result = (string) null;
      string status = "Calling GenerateMediaServerURL";
      string mediaServerUrl;
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 427, nameof (GenerateMediaServerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{eDeliveryServiceUrl}/loans/{loanGuid}/packages/documents/url".Replace("{eDeliveryServiceUrl}", this.eDeliveryServiceUrl).Replace("{host}", accessToken.HostName).Replace("{loanGuid}", request.applicationGroupId));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Clear();
          EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("X-Caller", "name=" + request.caller.name + ";version=" + request.caller.version);
          EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> task = EDeliveryServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(string.Empty));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GeneratesMediaServerURLResponse>(await response.Content.ReadAsStringAsync()).saveUrl;
          }
          else
          {
            EDeliveryServiceError innerException = JsonConvert.DeserializeObject<EDeliveryServiceError>(await response.Content.ReadAsStringAsync());
            IEnumerable<string> values;
            response.Headers.TryGetValues("X-Correlation-ID", out values);
            if (values != null)
            {
              EDeliveryServiceError edeliveryServiceError = innerException;
              edeliveryServiceError.summary = edeliveryServiceError.summary + Environment.NewLine + " CorrelationID = " + values.FirstOrDefault<string>();
            }
            throw new HttpException((int) response.StatusCode, innerException.Message, (Exception) innerException);
          }
        })).ConfigureAwait(false);
        mediaServerUrl = result;
      }
      catch (HttpException ex)
      {
        status = "Error in service call to GenerateEnvelopeSignerURL.";
        EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
        string displayErrorMessage = innerException.GetDisplayErrorMessage();
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + displayErrorMessage, 475, nameof (GenerateMediaServerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(displayErrorMessage);
      }
      catch (Exception ex)
      {
        if (ex is EDeliveryServiceError)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + ex.Message, 482, nameof (GenerateMediaServerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(ex.Message);
        }
        status = "Error in service call to GenerateEnvelopeSignerURL.";
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), status + " Error details: " + ex.Message);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + status, 488, nameof (GenerateMediaServerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(status);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 493, nameof (GenerateMediaServerURL), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
      }
      return mediaServerUrl;
    }

    public async Task<string> DownloadFilesFromMediaServer(
      Request request,
      string loanGuid,
      string packageId,
      string documentId,
      string basePath)
    {
      string result = (string) null;
      string status = "Calling GenerateMediaServerURLForDownloadFile";
      string url = "{host}/ellisign/v3/loans/{loanGuid}/packages/{packageId}/documents/{documentId}".Replace("{loanGuid}", loanGuid).Replace("{packageId}", packageId).Replace("{documentId}", documentId);
      string str;
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 513, nameof (DownloadFilesFromMediaServer), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        await this._oauth.ExecuteAsync(url, "sc", (Func<AccessToken, Task>) (async AccessToken =>
        {
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder(url.Replace("{host}", AccessToken.HostName));
          HttpResponseMessage responseMessage;
          lock (new object())
          {
            EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Clear();
            EDeliveryServiceClient.client.DefaultRequestHeaders.Clear();
            EDeliveryServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
            responseMessage = EDeliveryServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(string.Empty)).Result;
          }
          if (responseMessage.IsSuccessStatusCode)
          {
            GeneratesMediaServerDownloadURLResponse downloadUrlResponse = JsonConvert.DeserializeObject<GeneratesMediaServerDownloadURLResponse>(await responseMessage.Content.ReadAsStringAsync());
            result = downloadUrlResponse.url;
            if (downloadUrlResponse.mediaStore == "SkyDrive")
            {
              string header = downloadUrlResponse.headers["elli-signature"];
              if (string.IsNullOrEmpty(header))
                throw new Exception("elli-signature not found (for mediaStore SkyDrive).");
              EDeliveryServiceClient.client.DefaultRequestHeaders.Remove("Authorization");
              EDeliveryServiceClient.client.DefaultRequestHeaders.Add("Authorization", "elli-signature " + header);
            }
            Stream result1 = EDeliveryServiceClient.client.GetStreamAsync(downloadUrlResponse.url).Result;
            result = Path.Combine(basePath, documentId);
            using (Stream destination = (Stream) System.IO.File.Open(result, FileMode.Create))
              result1.CopyTo(destination);
          }
          else
          {
            EDeliveryServiceError innerException = JsonConvert.DeserializeObject<EDeliveryServiceError>(await responseMessage.Content.ReadAsStringAsync());
            IEnumerable<string> values;
            responseMessage.Headers.TryGetValues("X-Correlation-ID", out values);
            if (values != null)
            {
              EDeliveryServiceError edeliveryServiceError = innerException;
              edeliveryServiceError.summary = edeliveryServiceError.summary + Environment.NewLine + " CorrelationID = " + values.FirstOrDefault<string>();
            }
            throw new HttpException((int) responseMessage.StatusCode, innerException.Message, (Exception) innerException);
          }
        })).ConfigureAwait(false);
        str = result;
      }
      catch (HttpException ex)
      {
        status = "Error in service call to GenerateMediaServerURLForDownloadFile.";
        EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
        string displayErrorMessage = innerException.GetDisplayErrorMessage();
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + displayErrorMessage, 586, nameof (DownloadFilesFromMediaServer), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(displayErrorMessage);
      }
      catch (Exception ex)
      {
        if (ex is EDeliveryServiceError)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + ex.Message, 593, nameof (DownloadFilesFromMediaServer), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(ex.Message);
        }
        status = "Error in service call to GenerateMediaServerURLForDownloadFile.";
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), status + " Error details: " + ex.Message);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + status, 599, nameof (DownloadFilesFromMediaServer), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(status);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 604, nameof (DownloadFilesFromMediaServer), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
      }
      return str;
    }

    public async Task<string> DownloadServerFile(string url, string filename)
    {
      string empty = string.Empty;
      string msg = "Calling DownloadServerFile";
      string str;
      try
      {
        PerformanceMeter.Current.AddCheckpoint("BEGIN", 621, nameof (DownloadServerFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), msg);
        Stream stream = await EDeliveryServiceClient.client.GetStreamAsync(url).ConfigureAwait(false);
        string tempFileName = SystemSettings.GetTempFileName(filename);
        using (Stream destination = (Stream) System.IO.File.Open(tempFileName, FileMode.Create))
          stream.CopyTo(destination);
        str = tempFileName;
      }
      catch (HttpException ex)
      {
        EDeliveryServiceError innerException = (EDeliveryServiceError) ex.InnerException;
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), innerException.GetDetailedErrorMessageForLogging());
        string displayErrorMessage = innerException.GetDisplayErrorMessage();
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + displayErrorMessage, 645, nameof (DownloadServerFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(displayErrorMessage);
      }
      catch (Exception ex)
      {
        if (ex is EDeliveryServiceError)
        {
          PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + ex.Message, 652, nameof (DownloadServerFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
          throw new Exception(ex.Message);
        }
        string message = "Error in service call to GenerateMediaServerURLForDownloadFile.";
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), message + " Error details: " + ex.Message);
        PerformanceMeter.Current.AddCheckpoint("EXIT THROW - " + message, 658, nameof (DownloadServerFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
        throw new Exception(message);
      }
      finally
      {
        PerformanceMeter.Current.AddCheckpoint("END", 663, nameof (DownloadServerFile), "D:\\ws\\24.3.0.0\\EmLite\\eFolder\\eDelivery\\EDeliveryServiceClient.cs");
      }
      return str;
    }

    public async Task<Request> GetPackageDetails(string loanId, string packageId)
    {
      Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), "Entering GetPackageGroup");
      Request packageDetails;
      try
      {
        packageDetails = await this.submitRequest<Request>("{host}/ellisign/v3/loans/{loanId}/packages/{packageId}".Replace("{loanId}", loanId).Replace("{packageId}", packageId), "GET", (string) null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), " Error in GetPackageGroup. Ex: " + ex.ToString());
        throw;
      }
      return packageDetails;
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
          request.Headers.Add("elli_idt", "Application");
          if (!string.IsNullOrEmpty(content))
            request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, "application/json");
          Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), "Calling SendAsync: " + request.RequestUri.ToString());
          using (HttpResponseMessage response = await EDeliveryServiceClient.client.SendAsync(request).ConfigureAwait(false))
          {
            Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Verbose, nameof (EDeliveryServiceClient), "SendAsync Response Content: " + str);
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
                  Tracing.Log(EDeliveryServiceClient.sw, TraceLevel.Error, nameof (EDeliveryServiceClient), "SendAsync Response Content: " + str);
                  EDeliveryServiceError edeliveryServiceError = JsonConvert.DeserializeObject<EDeliveryServiceError>(str);
                  message = message + " " + edeliveryServiceError.code + " " + edeliveryServiceError.summary + " " + edeliveryServiceError.details;
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
