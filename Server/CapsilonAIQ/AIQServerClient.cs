// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.CapsilonAIQ.AIQServerClient
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Server.CapsilonAIQ.S3Signers;
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
namespace EllieMae.EMLite.Server.CapsilonAIQ
{
  public class AIQServerClient
  {
    private const string className = "AIQServerClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private static HttpClient client = new HttpClient();
    private AIQapiConfigurations AIQConfig;
    private AIQAuth AIQAuth;
    private static string DataStoreSite = "";
    private const string scope = "sc�";
    private const string AIQsessionType = "SITE_USER�";
    private string LaunchURl_WEB = "https://{aiq-site-address}/?application=folderExplorer&launchConfigurationId={decrypted-launch-config-id}&sessionId={aiq-session-id}";
    private string LaunchURL_Desktop = "capsilon://?command=openfolder&siteaddress={aiq-site-address}&userticketid={sessionId}&launchConfigurationId={decrypted-launch-configuration-id}";
    private string LaunchAnalyzer_WEB = "https://{aiq-site-address}/bac/?launchConfiguration={decrypted-launch-config-id}&viewMode=browser";
    private const string VerifySiteAddress_URL = "/v1/encompass-configurations/connection-configuration?site-address={{siteAddress}}�";
    private const string UpdateAIQEncompassInfo_URL = "/v1/encompass-configurations/connection-configuration?site-id={{siteId}}�";
    private const string GetAIQSiteID_URL = "/v1/aiq-sites/aiq-site?site-address={siteAddress}�";
    private const string GetAIQSiteID_URL_V2 = "/v2/aiq-sites/aiq-site?site-address={siteAddress}�";
    private const string GetAIQSession_URL = "/services/v1/sessions/session?site-id={siteID}�";
    private const string GetLaunchConfigurations_URL = "/services/v1/launch-configurations/launch-configuration�";
    private const string GetAIQAppTicketID_URL = "/services/v1/sessions/{session_id}/application-tickets�";
    private const string GetIncomeAnalyzerPayloadID_URL = "/services/filestore-service/v1/folders/{folder_id}/file-sources?source_file_type=INCOME_ANALYZER_RESULT&latest=true�";
    private const string GetIncomeAnalyzerPayloadData_URL = "/services/filestore-service/v1/folders/{folder_id}/file-sources/download?payload_id={payload_id}�";
    private bool isAIQAuthenticationReqired;
    private string ContentType_Json = "application/json";
    private string ContentType_urlencoded = "application/x-www-form-urlencoded";

    public AIQServerClient(Dictionary<string, string> AIQConfigDetails)
    {
      this.AIQConfig = new AIQapiConfigurations();
      this.AIQAuth = new AIQAuth();
      foreach (KeyValuePair<string, string> aiqConfigDetail in AIQConfigDetails)
      {
        switch (aiqConfigDetail.Key)
        {
          case "AIQ.ApplicationDataMap":
            this.AIQConfig.ApplicationDataMaps = aiqConfigDetail.Value;
            continue;
          case "AIQ.ApplicationFlags":
            this.AIQConfig.ApplicationFlags = aiqConfigDetail.Value;
            continue;
          case "AIQ.GetSiteID.URL":
            this.AIQConfig.AIQGetSiteIDURL = aiqConfigDetail.Value;
            continue;
          case "AIQ.LOS.URL":
            this.AIQConfig.LOSURL = aiqConfigDetail.Value;
            continue;
          case "AIQcredential":
            this.AIQConfig.AIQcredential = aiqConfigDetail.Value;
            continue;
          case "AppName":
            this.AIQConfig.AppName = aiqConfigDetail.Value;
            continue;
          case "AppSecret":
            this.AIQConfig.AppSecret = aiqConfigDetail.Value;
            continue;
          case "TERMINATOR":
            this.AIQAuth.TERMINATOR = aiqConfigDetail.Value;
            continue;
          case "regionName":
            this.AIQAuth.regionName = aiqConfigDetail.Value;
            continue;
          case "secretKey":
            this.AIQAuth.secretKey = aiqConfigDetail.Value;
            continue;
          case "secretKeyId":
            this.AIQAuth.secretKeyId = aiqConfigDetail.Value;
            continue;
          case "serviceName_LOS":
            this.AIQAuth.serviceName_LOS = aiqConfigDetail.Value;
            continue;
          case "serviceName_SiteID":
            this.AIQAuth.serviceName_SiteID = aiqConfigDetail.Value;
            continue;
          default:
            continue;
        }
      }
    }

    public async Task<AIQServerClient.SiteVerifyOutput> VerifySiteAddress(string siteAddress)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Verify Site Address");
      Dictionary<string, string> QueryParam = new Dictionary<string, string>();
      Dictionary<string, string> headers = new Dictionary<string, string>();
      AIQServerClient.MainSiteVerifyOutput response = new AIQServerClient.MainSiteVerifyOutput();
      try
      {
        this.isAIQAuthenticationReqired = true;
        string url = (this.AIQConfig.LOSURL + "/v1/encompass-configurations/connection-configuration?site-address={{siteAddress}}").Replace("{{siteAddress}}", siteAddress);
        QueryParam.Add("site-address", siteAddress);
        response = await this.submitRequest<AIQServerClient.MainSiteVerifyOutput>(url, HttpMethod.Get.Method, string.Empty, this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
        response.encompassConnectionConfiguration.status = "success";
        return response.encompassConnectionConfiguration;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in Verify Site Address", ex);
        response.encompassConnectionConfiguration.status = "error";
        response.encompassConnectionConfiguration.errorMessage = ex.Message;
        return response.encompassConnectionConfiguration;
      }
    }

    public async Task<AIQServerClient.EncompassConnConfig> UpdateAIQEncompassInfo(
      string siteID,
      string content)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Synch/Update Data & Document Automation and Mortgage Analyzers Encompass Info");
      Dictionary<string, string> headers = new Dictionary<string, string>();
      Dictionary<string, string> QueryParam = new Dictionary<string, string>();
      AIQServerClient.MainEncompassConnConfig response = new AIQServerClient.MainEncompassConnConfig();
      try
      {
        string url = (this.AIQConfig.LOSURL + "/v1/encompass-configurations/connection-configuration?site-id={{siteId}}").Replace("{{siteId}}", siteID);
        QueryParam.Add("site-id", siteID);
        this.isAIQAuthenticationReqired = true;
        response = await this.submitRequest<AIQServerClient.MainEncompassConnConfig>(url, HttpMethod.Put.Method, content, this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
        response.encompassConnectionConfiguration.status = "success";
        return response.encompassConnectionConfiguration;
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in Synch/Update Data & Document Automation and Mortgage Analyzers Encompass Info", ex);
        response.encompassConnectionConfiguration.status = "error";
        response.encompassConnectionConfiguration.errorMessage = ex.Message;
        return response.encompassConnectionConfiguration;
      }
    }

    public async Task<AIQServerClient.AIQSiteOutput> GetAIQSiteID(string siteAddress)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Get Data & Document Automation and Mortgage Analyzers Site ID");
      AIQServerClient.AIQSiteOutput aiqSiteId;
      try
      {
        AIQServerClient.GetSiteIDInput getSiteIdInput = new AIQServerClient.GetSiteIDInput();
        Dictionary<string, string> QueryParam = new Dictionary<string, string>();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string url = (this.AIQConfig.AIQGetSiteIDURL + "/v1/aiq-sites/aiq-site?site-address={siteAddress}").Replace("{siteAddress}", siteAddress);
        QueryParam.Add("site-address", siteAddress);
        getSiteIdInput.application_name = this.AIQConfig.AppName;
        getSiteIdInput.application_secret = this.AIQConfig.AppSecret;
        string content = JsonConvert.SerializeObject((object) getSiteIdInput);
        this.isAIQAuthenticationReqired = true;
        aiqSiteId = await this.submitRequest<AIQServerClient.AIQSiteOutput>(url, HttpMethod.Post.Method, content, this.ContentType_urlencoded, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in get Data & Document Automation and Mortgage Analyzers Site Address.", ex);
        throw;
      }
      return aiqSiteId;
    }

    private async Task<AIQServerClient.AIQSiteOutput_V2> getAIQSiteID_V2(string siteAddress)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Get Data & Document Automation and Mortgage Analyzers Site ID V2");
      AIQServerClient.AIQSiteOutput_V2 aiqSiteIdV2;
      try
      {
        AIQServerClient.GetSiteIDInput getSiteIdInput = new AIQServerClient.GetSiteIDInput();
        Dictionary<string, string> QueryParam = new Dictionary<string, string>();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        string url = (this.AIQConfig.AIQGetSiteIDURL + "/v2/aiq-sites/aiq-site?site-address={siteAddress}").Replace("{siteAddress}", siteAddress);
        QueryParam.Add("site-address", siteAddress);
        getSiteIdInput.application_name = this.AIQConfig.AppName;
        getSiteIdInput.application_secret = this.AIQConfig.AppSecret;
        string content = JsonConvert.SerializeObject((object) getSiteIdInput);
        this.isAIQAuthenticationReqired = true;
        aiqSiteIdV2 = await this.submitRequest<AIQServerClient.AIQSiteOutput_V2>(url, HttpMethod.Post.Method, content, this.ContentType_urlencoded, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in get Data & Document Automation and Mortgage Analyzers Site Address.", ex);
        throw;
      }
      return aiqSiteIdV2;
    }

    public async Task<AIQServerClient.AIQSessionOutput> GetAIQSession(
      AIQServerClient.AIQSessionDetails details,
      string siteID,
      Dictionary<string, string> headers,
      string baseURL)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Get Data & Document Automation and Mortgage Analyzers Session");
      details.authRequest.applicationName = this.AIQConfig.AppName;
      details.authRequest.applicationSecret = this.AIQConfig.AppSecret;
      details.authRequest.credential = this.AIQConfig.AIQcredential;
      details.authRequest.sessionType = "SITE_USER";
      this.isAIQAuthenticationReqired = false;
      AIQServerClient.AIQSessionOutput aiqSession;
      try
      {
        aiqSession = await this.submitRequest<AIQServerClient.AIQSessionOutput>("{baseURL}/services/v1/sessions/session?site-id={siteID}".Replace("{baseURL}", baseURL).Replace("{siteID}", siteID), HttpMethod.Post.Method, JsonConvert.SerializeObject((object) details), this.ContentType_Json, headers, new Dictionary<string, string>()
        {
          {
            nameof (siteID),
            siteID
          }
        }).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in Get Data & Document Automation and Mortgage Analyzers Session", ex);
        throw;
      }
      return aiqSession;
    }

    public async Task<AIQServerClient.LaunchConfigResponse> GetLaunchConfigurations(
      AIQServerClient.LaunchConfigurations details,
      Dictionary<string, string> headers,
      string baseURL)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering create Launch configurations");
      this.isAIQAuthenticationReqired = false;
      AIQServerClient.LaunchConfigResponse launchConfigurations;
      try
      {
        details.launchConfigurationRequest.applicationFlags = new List<string>()
        {
          this.AIQConfig.ApplicationFlags
        };
        Dictionary<string, string> QueryParam = new Dictionary<string, string>();
        launchConfigurations = await this.submitRequest<AIQServerClient.LaunchConfigResponse>("{baseURL}/services/v1/launch-configurations/launch-configuration".Replace("{baseURL}", baseURL), HttpMethod.Post.Method, JsonConvert.SerializeObject((object) details), this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in create Launch configurations", ex);
        throw;
      }
      return launchConfigurations;
    }

    private async Task<AIQServerClient.ApplicationTicketOutput> getAIQAppTicketID(
      AIQServerClient.AIQAppTicketsDetail details,
      string baseURL,
      string sessionID)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering Get Data & Document Automation and Mortgage Analyzers Application Ticket IDs");
      this.isAIQAuthenticationReqired = false;
      AIQServerClient.ApplicationTicketOutput aiqAppTicketId;
      try
      {
        Dictionary<string, string> QueryParam = new Dictionary<string, string>();
        Dictionary<string, string> headers = new Dictionary<string, string>();
        aiqAppTicketId = await this.submitRequest<AIQServerClient.ApplicationTicketOutput>((baseURL + "/services/v1/sessions/{session_id}/application-tickets").Replace("{session_id}", sessionID), HttpMethod.Post.Method, JsonConvert.SerializeObject((object) details), this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in create Launch configurations", ex);
        throw;
      }
      return aiqAppTicketId;
    }

    private async Task<AIQServerClient.IncomePayloadIDOutput> getIncomeAnalyzerPayloadID(
      string baseAddress,
      string aiqFolderID,
      string appTicketID)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering getIncomeAnalyzerPayloadID");
      Dictionary<string, string> QueryParam = new Dictionary<string, string>();
      Dictionary<string, string> headers = new Dictionary<string, string>();
      AIQServerClient.IncomePayloadIDOutput analyzerPayloadId;
      try
      {
        this.isAIQAuthenticationReqired = false;
        string url = (baseAddress + "/services/filestore-service/v1/folders/{folder_id}/file-sources?source_file_type=INCOME_ANALYZER_RESULT&latest=true").Replace("{folder_id}", aiqFolderID);
        headers.Add("application_ticket_id", appTicketID);
        analyzerPayloadId = await this.submitRequest<AIQServerClient.IncomePayloadIDOutput>(url, HttpMethod.Get.Method, string.Empty, this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getIncomeAnalyzerPayloadID", ex);
        throw;
      }
      return analyzerPayloadId;
    }

    private async Task<JAIQIncome> getIncomeAnalyzerPayloadData(
      string dataStoreAddress,
      string aiqFolderID,
      string appTicketID,
      string payloadID)
    {
      Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Entering getIncomeAnalyzerPayloadData");
      Dictionary<string, string> QueryParam = new Dictionary<string, string>();
      Dictionary<string, string> headers = new Dictionary<string, string>();
      JAIQIncome analyzerPayloadData;
      try
      {
        this.isAIQAuthenticationReqired = false;
        string url = (dataStoreAddress + "/services/filestore-service/v1/folders/{folder_id}/file-sources/download?payload_id={payload_id}").Replace("{folder_id}", aiqFolderID).Replace("{payload_id}", payloadID);
        headers.Add("application_ticket_id", appTicketID);
        analyzerPayloadData = await this.submitRequest<JAIQIncome>(url, HttpMethod.Get.Method, string.Empty, this.ContentType_Json, headers, QueryParam).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getIncomeAnalyzerPayloadData", ex);
        throw;
      }
      return analyzerPayloadData;
    }

    public AIQServerClient.LaunchResponce CallingAIQLaunchAPI(
      ref string SiteId,
      ref string AIQBaseAddress,
      string SiteAddress,
      AIQServerClient.AIQSessionDetails inputSessionAPI,
      AIQServerClient.LaunchConfigurations LaunchConfigAPI_input)
    {
      string baseURL = (string) null;
      string siteID = (string) null;
      string str1 = (string) null;
      string str2 = (string) null;
      string str3 = (string) null;
      AIQServerClient.LaunchResponce launchResponce = new AIQServerClient.LaunchResponce();
      try
      {
        if (string.IsNullOrWhiteSpace(SiteId))
        {
          Task<AIQServerClient.AIQSiteOutput> aiqSiteId = this.GetAIQSiteID(SiteAddress);
          Task.WaitAll((Task) aiqSiteId);
          if (aiqSiteId.Result != null)
          {
            siteID = aiqSiteId.Result.aiqsite.siteId.ToString();
            baseURL = aiqSiteId.Result.aiqsite.urls.value;
            SiteId = siteID;
            AIQBaseAddress = baseURL;
          }
        }
        else
        {
          siteID = SiteId.ToString();
          baseURL = AIQBaseAddress;
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in Get Data & Document Automation and Mortgage Analyzers SiteID", ex);
        launchResponce.status = "error";
        launchResponce.errorMessage = ex.Message;
        return launchResponce;
      }
      if (!string.IsNullOrWhiteSpace(siteID))
      {
        try
        {
          Dictionary<string, string> headers = new Dictionary<string, string>();
          Task<AIQServerClient.AIQSessionOutput> aiqSession = this.GetAIQSession(inputSessionAPI, siteID, headers, baseURL);
          Task.WaitAll((Task) aiqSession);
          if (aiqSession.Result != null)
            str1 = aiqSession.Result.aiqSession.sessionId.ToString();
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in Get Data & Document Automation and Mortgage Analyzers Session", ex);
          launchResponce.status = "error";
          launchResponce.errorMessage = ex.Message;
          return launchResponce;
        }
      }
      if (!string.IsNullOrWhiteSpace(str1))
      {
        try
        {
          Task<AIQServerClient.LaunchConfigResponse> launchConfigurations = this.GetLaunchConfigurations(LaunchConfigAPI_input, new Dictionary<string, string>()
          {
            {
              "session-id",
              str1
            }
          }, baseURL);
          Task.WaitAll((Task) launchConfigurations);
          if (launchConfigurations.Result != null)
          {
            str2 = launchConfigurations.Result.launchConfigurationResponse.id.ToString();
            str3 = launchConfigurations.Result.launchConfigurationResponse.sessionId.ToString();
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in get launch Configuration", ex);
          launchResponce.status = "error";
          launchResponce.errorMessage = ex.Message;
          return launchResponce;
        }
      }
      launchResponce.status = "success";
      launchResponce.LaunchAIQSession = str3;
      launchResponce.launchConfigurationCode = str2;
      return launchResponce;
    }

    public JAIQIncome CallingAIQIncomeAnalyzerAPI(
      ref string SiteId,
      ref string AIQBaseAddress,
      string SiteAddress,
      AIQServerClient.AIQSessionDetails inputSessionAPI,
      string aiqFolderID)
    {
      string str = (string) null;
      string siteID = (string) null;
      string sessionID = (string) null;
      string appTicketID = (string) null;
      string payloadID = (string) null;
      JAIQIncome jaiqIncome = (JAIQIncome) null;
      try
      {
        if (string.IsNullOrWhiteSpace(SiteId) || string.IsNullOrWhiteSpace(AIQServerClient.DataStoreSite))
        {
          Task<AIQServerClient.AIQSiteOutput_V2> aiqSiteIdV2 = this.getAIQSiteID_V2(SiteAddress);
          Task.WaitAll((Task) aiqSiteIdV2);
          if (aiqSiteIdV2.Result != null)
          {
            siteID = aiqSiteIdV2.Result.aiqSiteAndUrls.siteId.ToString();
            foreach (AIQServerClient.URLS url in (IEnumerable<AIQServerClient.URLS>) aiqSiteIdV2.Result.aiqSiteAndUrls.urls)
            {
              if (url.key == "LisGatewayURL")
                str = url.value;
              else if (url.key == "datastoreBaseURL")
                AIQServerClient.DataStoreSite = url.value;
            }
            SiteId = siteID;
            AIQBaseAddress = str;
          }
        }
        else
        {
          siteID = SiteId.ToString();
          str = AIQBaseAddress;
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getting Data & Document Automation and Mortgage Analyzers SiteID (V2)", ex);
        throw new Exception("Error in getting Data & Document Automation and Mortgage Analyzers SiteID (V2): [" + ex.Message + "]");
      }
      if (!string.IsNullOrWhiteSpace(siteID))
      {
        try
        {
          Dictionary<string, string> headers = new Dictionary<string, string>();
          Task<AIQServerClient.AIQSessionOutput> aiqSession = this.GetAIQSession(inputSessionAPI, siteID, headers, str);
          Task.WaitAll((Task) aiqSession);
          if (aiqSession.Result != null)
            sessionID = aiqSession.Result.aiqSession.sessionId.ToString();
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getting Data & Document Automation and Mortgage Analyzers Session", ex);
          throw new Exception("Error in getting Data & Document Automation and Mortgage Analyzers Session: [" + ex.Message + "]");
        }
      }
      if (!string.IsNullOrWhiteSpace(sessionID))
      {
        try
        {
          Task<AIQServerClient.ApplicationTicketOutput> aiqAppTicketId = this.getAIQAppTicketID(new AIQServerClient.AIQAppTicketsDetail(new AIQServerClient.ApplicationTicketsRequest(this.AIQConfig.AppName, this.AIQConfig.AppSecret, 1)), str, sessionID);
          Task.WaitAll((Task) aiqAppTicketId);
          if (aiqAppTicketId.Result != null)
          {
            IList<AIQServerClient.ApplicationTicket> applicationTickets = aiqAppTicketId.Result.applicationTicketCollection.applicationTickets;
            if (applicationTickets != null)
            {
              if (applicationTickets.Count > 0)
                appTicketID = aiqAppTicketId.Result.applicationTicketCollection.applicationTickets[0].id;
            }
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getting application ticket ID", ex);
          throw new Exception("Error in getting application ticket ID: [" + ex.Message + "]");
        }
      }
      if (!string.IsNullOrWhiteSpace(appTicketID))
      {
        try
        {
          Task<AIQServerClient.IncomePayloadIDOutput> analyzerPayloadId = this.getIncomeAnalyzerPayloadID(str, aiqFolderID, appTicketID);
          Task.WaitAll((Task) analyzerPayloadId);
          if (analyzerPayloadId.Result != null)
          {
            IList<AIQServerClient.FileSource> fileSources = analyzerPayloadId.Result.fileSourceCollection.fileSources;
            if (fileSources != null)
            {
              if (fileSources.Count > 0)
                payloadID = fileSources[0].payloadId;
            }
          }
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getting income payload ID", ex);
          throw new Exception("Error in getting income payload ID: [" + ex.Message + "]");
        }
      }
      if (!string.IsNullOrWhiteSpace(payloadID))
      {
        try
        {
          Task<JAIQIncome> analyzerPayloadData = this.getIncomeAnalyzerPayloadData(AIQServerClient.DataStoreSite, aiqFolderID, appTicketID, payloadID);
          Task.WaitAll((Task) analyzerPayloadData);
          if (analyzerPayloadData.Result != null)
            jaiqIncome = analyzerPayloadData.Result;
        }
        catch (Exception ex)
        {
          RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in getting income payload data", ex);
          throw new Exception("Error in getting income payload data: [" + ex.Message + "]");
        }
        return jaiqIncome;
      }
      RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Got no payload ID for this loan from Data & Document Automation and Mortgage Analyzers side.", (Exception) null);
      throw new Exception("Got no Income Analyzer payload ID from Data & Document Automation and Mortgage Analyzers side.");
    }

    private async Task<T> submitRequest<T>(
      string url,
      string method,
      string content,
      string Content_Type,
      Dictionary<string, string> headers,
      Dictionary<string, string> QueryParam)
    {
      T result = default (T);
      try
      {
        using (HttpRequestMessage request = new HttpRequestMessage())
        {
          Uri uri = new Uri(url);
          if (this.isAIQAuthenticationReqired)
          {
            string Qparam = AIQServerClient.ConcatQueryParam(QueryParam);
            this.ComputeAuthorization(method, Content_Type, ref headers, Qparam, request, uri, content);
          }
          request.Method = new HttpMethod(method);
          request.RequestUri = uri;
          if (Content_Type == this.ContentType_urlencoded && !string.IsNullOrEmpty(content))
          {
            if (headers.Count > 0)
            {
              foreach (KeyValuePair<string, string> header in headers)
              {
                if (header.Key == "host")
                  request.Headers.Host = header.Value;
                else
                  request.Headers.TryAddWithoutValidation(header.Key, header.Value);
              }
            }
          }
          else
          {
            if (!string.IsNullOrWhiteSpace(content))
            {
              request.Content = (HttpContent) new StringContent(content, Encoding.UTF8, Content_Type);
              request.Content.Headers.ContentType = new MediaTypeHeaderValue(Content_Type);
            }
            if (headers.Count > 0)
            {
              foreach (KeyValuePair<string, string> header in headers)
              {
                if (header.Key == "host")
                  request.Headers.Host = header.Value;
                else
                  request.Headers.TryAddWithoutValidation(header.Key, header.Value);
              }
            }
          }
          result = await this.SendAPIRequest<T>(content, result, request).ConfigureAwait(false);
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in API calling", ex);
        throw;
      }
      return result;
    }

    private void ComputeAuthorization(
      string method,
      string Content_Type,
      ref Dictionary<string, string> headers,
      string Qparam,
      HttpRequestMessage request,
      Uri uri,
      string content)
    {
      string str = (string) null;
      if (method == HttpMethod.Post.Method)
      {
        FormUrlEncodedContent urlEncodedContent = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>) JsonConvert.DeserializeObject<Dictionary<string, string>>(content));
        request.Content = (HttpContent) urlEncodedContent;
        string hexString = SignerBase.ToHexString(SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(urlEncodedContent.ReadAsStringAsync().Result)), true);
        SignerForAuthorizationHeader authorizationHeader1 = new SignerForAuthorizationHeader();
        authorizationHeader1.EndpointUri = uri;
        authorizationHeader1.HttpMethod = method;
        authorizationHeader1.Service = this.AIQAuth.serviceName_SiteID;
        authorizationHeader1.Region = this.AIQAuth.regionName;
        authorizationHeader1.TERMINATOR = this.AIQAuth.TERMINATOR;
        SignerForAuthorizationHeader authorizationHeader2 = authorizationHeader1;
        headers.Add("x-aiq-content-sha256", hexString);
        headers.Add("Content-Length", urlEncodedContent.ReadAsStringAsync().Result.Length.ToString());
        headers.Add("Content-Type", Content_Type);
        str = authorizationHeader2.ComputeSignature((IDictionary<string, string>) headers, Qparam, hexString, this.AIQAuth.secretKeyId, this.AIQAuth.secretKey);
      }
      else if (method == HttpMethod.Put.Method)
      {
        string hexString = SignerBase.ToHexString(SignerBase.CanonicalRequestHashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(content)), true);
        SignerForAuthorizationHeader authorizationHeader3 = new SignerForAuthorizationHeader();
        authorizationHeader3.EndpointUri = uri;
        authorizationHeader3.HttpMethod = method;
        authorizationHeader3.Service = this.AIQAuth.serviceName_LOS;
        authorizationHeader3.Region = this.AIQAuth.regionName;
        authorizationHeader3.TERMINATOR = this.AIQAuth.TERMINATOR;
        SignerForAuthorizationHeader authorizationHeader4 = authorizationHeader3;
        headers.Add("x-aiq-content-sha256", hexString);
        headers.Add("Content-Length", content.Length.ToString());
        headers.Add("Content-Type", Content_Type);
        str = authorizationHeader4.ComputeSignature((IDictionary<string, string>) headers, Qparam, hexString, this.AIQAuth.secretKeyId, this.AIQAuth.secretKey);
      }
      else if (method == HttpMethod.Get.Method)
      {
        headers.Add("x-aiq-content-sha256", "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855");
        SignerForAuthorizationHeader authorizationHeader = new SignerForAuthorizationHeader();
        authorizationHeader.EndpointUri = uri;
        authorizationHeader.HttpMethod = method;
        authorizationHeader.Service = this.AIQAuth.serviceName_LOS;
        authorizationHeader.Region = this.AIQAuth.regionName;
        authorizationHeader.TERMINATOR = this.AIQAuth.TERMINATOR;
        str = authorizationHeader.ComputeSignature((IDictionary<string, string>) headers, Qparam, "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", this.AIQAuth.secretKeyId, this.AIQAuth.secretKey);
      }
      headers.Add("Authorization", str);
    }

    private async Task<T> SendAPIRequest<T>(string content, T result, HttpRequestMessage request)
    {
      try
      {
        Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "Calling SendAsync: " + request.RequestUri.ToString() + " Payload: " + content);
        using (HttpResponseMessage response = await AIQServerClient.client.SendAsync(request).ConfigureAwait(false))
        {
          Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "SendAsync Response StatusCode: " + (object) response.StatusCode);
          if (response.IsSuccessStatusCode)
          {
            string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "SendAsync Response Content: " + str);
            result = JsonConvert.DeserializeObject<T>(str);
          }
          else
          {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
              throw new HttpException((int) response.StatusCode, "Unauthorized Request");
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
              string message = "Response: " + (object) (int) response.StatusCode + " " + (object) response.StatusCode;
              string str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
              Tracing.Log(AIQServerClient.sw, TraceLevel.Verbose, nameof (AIQServerClient), "SendAsync Response Content: " + str);
              AIQServerClient.AIQServiceError aiqServiceError = JsonConvert.DeserializeObject<AIQServerClient.AIQServiceError>(str);
              message = aiqServiceError.DisplayMessage == null ? message + " " + JsonConvert.DeserializeObject<AIQServerClient.ServiceError>(str).Code : message + " " + aiqServiceError.DisplayMessage;
              IEnumerable<string> values;
              response.Headers.TryGetValues("X-Correlation-ID", out values);
              if (values != null)
                message = message + " CorrelationID=" + values.FirstOrDefault<string>();
              throw new HttpException((int) response.StatusCode, message);
            }
          }
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (AIQServerClient), "Error in API responce", ex);
        throw;
      }
      return (T) result;
    }

    public string CreateLaunchURL(
      string LaunchType,
      string SiteAddress,
      string DecrptedLaunchCode,
      string LaunchAIQSession)
    {
      string launchUrl = (string) null;
      switch (LaunchType)
      {
        case "WEB":
          this.LaunchURl_WEB = this.LaunchURl_WEB.Replace("{aiq-site-address}", SiteAddress);
          this.LaunchURl_WEB = this.LaunchURl_WEB.Replace("{decrypted-launch-config-id}", DecrptedLaunchCode);
          this.LaunchURl_WEB = this.LaunchURl_WEB.Replace("{aiq-session-id}", LaunchAIQSession);
          launchUrl = this.LaunchURl_WEB;
          break;
        case "DESKTOP":
          string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(LaunchAIQSession));
          this.LaunchURL_Desktop = this.LaunchURL_Desktop.Replace("{aiq-site-address}", SiteAddress);
          this.LaunchURL_Desktop = this.LaunchURL_Desktop.Replace("{sessionId}", base64String);
          this.LaunchURL_Desktop = this.LaunchURL_Desktop.Replace("{decrypted-launch-configuration-id}", DecrptedLaunchCode);
          launchUrl = this.LaunchURL_Desktop;
          break;
        case "Analyzer_WEB":
          this.LaunchAnalyzer_WEB = this.LaunchAnalyzer_WEB.Replace("{aiq-site-address}", SiteAddress);
          this.LaunchAnalyzer_WEB = this.LaunchAnalyzer_WEB.Replace("{decrypted-launch-config-id}", DecrptedLaunchCode);
          launchUrl = this.LaunchAnalyzer_WEB;
          break;
      }
      return launchUrl;
    }

    private static string ConcatQueryParam(Dictionary<string, string> QueryParam)
    {
      string str = "";
      int num = 0;
      if (QueryParam.Count > 0)
      {
        foreach (KeyValuePair<string, string> keyValuePair in QueryParam)
        {
          if (num > 0)
            str += "&";
          str = str + keyValuePair.Key + "=" + keyValuePair.Value;
          ++num;
        }
      }
      return str;
    }

    public class AIQServiceError
    {
      public string DisplayMessage { get; set; }

      public string FaultCode { get; set; }

      public string FaultID { get; set; }

      public string InternalMessage { get; set; }

      public int Source { get; set; }
    }

    public class ServiceError
    {
      public string Message { get; set; }

      public string Code { get; set; }
    }

    public class GetSiteIDInput
    {
      [JsonProperty(PropertyName = "application-name")]
      public string application_name { get; set; }

      [JsonProperty(PropertyName = "application-secret")]
      public string application_secret { get; set; }
    }

    public class URLS
    {
      public string key { get; set; }

      public string value { get; set; }
    }

    public class AIQSITE
    {
      public string siteId { get; set; }

      public AIQServerClient.URLS urls { get; set; }
    }

    public class AIQSiteOutput
    {
      public AIQServerClient.AIQSITE aiqsite { get; set; }
    }

    public class AIQSITE_V2
    {
      public string siteId { get; set; }

      public IList<AIQServerClient.URLS> urls { get; set; }
    }

    public class AIQSiteOutput_V2
    {
      public AIQServerClient.AIQSITE_V2 aiqSiteAndUrls { get; set; }
    }

    public class UserDetail
    {
      public UserDetail(string uId, string fName, string lName, string eId, string pNames)
      {
        this.userId = uId;
        this.firstName = fName;
        this.lastName = lName;
        this.emailId = eId;
        this.roleName = pNames;
      }

      public string userId { get; set; }

      public string firstName { get; set; }

      public string lastName { get; set; }

      public string emailId { get; set; }

      public string roleName { get; set; }
    }

    public class AuthRequest
    {
      public AuthRequest(
        string appName,
        string appSecret,
        string cred,
        string sessType,
        AIQServerClient.UserDetail user)
      {
        this.applicationName = appName;
        this.applicationSecret = appSecret;
        this.credential = cred;
        this.sessionType = sessType;
        this.userDetail = user;
      }

      public string applicationName { get; set; }

      public string applicationSecret { get; set; }

      public string credential { get; set; }

      public string sessionType { get; set; }

      public AIQServerClient.UserDetail userDetail { get; set; }
    }

    public class AIQSessionDetails
    {
      public AIQSessionDetails(AIQServerClient.AuthRequest autReq) => this.authRequest = autReq;

      public AIQServerClient.AuthRequest authRequest { get; set; }
    }

    public class ApplicationTicketsRequest
    {
      public ApplicationTicketsRequest(string appName, string appSecret, int noTickets)
      {
        this.applicationName = appName;
        this.applicationSecret = appSecret;
        this.noOfTickets = noTickets;
      }

      public string applicationName { get; set; }

      public string applicationSecret { get; set; }

      public int noOfTickets { get; set; }
    }

    public class AIQAppTicketsDetail
    {
      public AIQAppTicketsDetail(AIQServerClient.ApplicationTicketsRequest appReq)
      {
        this.applicationTicketsRequest = appReq;
      }

      public AIQServerClient.ApplicationTicketsRequest applicationTicketsRequest { get; set; }
    }

    public class AIQSession
    {
      public string sessionId { get; set; }
    }

    public class AIQSessionOutput
    {
      public AIQServerClient.AIQSession aiqSession { get; set; }
    }

    public class ApplicationTicket
    {
      public string id { get; set; }

      public string userTicketId { get; set; }

      public string applicationName { get; set; }
    }

    public class ApplicationTicketCollection
    {
      public IList<AIQServerClient.ApplicationTicket> applicationTickets { get; set; }
    }

    public class ApplicationTicketOutput
    {
      public AIQServerClient.ApplicationTicketCollection applicationTicketCollection { get; set; }
    }

    public class ApplicationDataMap
    {
      public ApplicationDataMap(
        List<AIQServerClient.ApplicationDataKeyValuePair> value)
      {
        this.entry = value;
      }

      public List<AIQServerClient.ApplicationDataKeyValuePair> entry { get; set; }
    }

    public struct ApplicationDataKeyValuePair(string k, string v)
    {
      public string key = k;
      public string value = v;
    }

    public class LaunchConfigurationRequest
    {
      public LaunchConfigurationRequest(
        string ctextId,
        List<string> rIDs,
        AIQServerClient.ApplicationDataMap appDataMap,
        List<string> appFlags)
      {
        this.contextId = ctextId;
        this.resourceIDs = rIDs;
        this.applicationDataMap = appDataMap;
        this.applicationFlags = appFlags;
      }

      public string contextId { get; set; }

      public List<string> resourceIDs { get; set; }

      public List<string> applicationFlags { get; set; }

      public AIQServerClient.ApplicationDataMap applicationDataMap { get; set; }
    }

    public class LaunchConfigurations
    {
      public LaunchConfigurations(
        AIQServerClient.LaunchConfigurationRequest LaunchConfig)
      {
        this.launchConfigurationRequest = LaunchConfig;
      }

      public AIQServerClient.LaunchConfigurationRequest launchConfigurationRequest { get; set; }
    }

    public class LaunchConfigurationResponse
    {
      public string id { get; set; }

      public string sessionId { get; set; }
    }

    public class LaunchConfigResponse
    {
      public AIQServerClient.LaunchConfigurationResponse launchConfigurationResponse { get; set; }
    }

    public class MainSiteVerifyOutput
    {
      public AIQServerClient.SiteVerifyOutput encompassConnectionConfiguration { get; set; }
    }

    public class SiteVerifyOutput
    {
      public string siteId { get; set; }

      public string clientId { get; set; }

      public string instanceId { get; set; }

      public string devConnectApiUserId { get; set; }

      public string encompassUserId { get; set; }

      public string encompassPassword { get; set; }

      public string APIClientId { get; set; }

      public string APIClientSecret { get; set; }

      public string environment { get; set; }

      public string deploymentStatus { get; set; }

      public string status { get; set; }

      public string errorMessage { get; set; }
    }

    public class FileSource
    {
      public string id { get; set; }

      public string friendlyId { get; set; }

      public string folderId { get; set; }

      public string sourceFileType { get; set; }

      public string dateCreated { get; set; }

      public string payloadId { get; set; }
    }

    public class FileSourceCollection
    {
      public bool available { get; set; }

      public string id { get; set; }

      public IList<AIQServerClient.FileSource> fileSources { get; set; }
    }

    public class IncomePayloadIDOutput
    {
      public AIQServerClient.FileSourceCollection fileSourceCollection { get; set; }
    }

    public class EncompassConfigRequest
    {
      public string clientId { get; set; }

      public string instanceId { get; set; }

      public string devConnectApiUserId { get; set; }

      public string encompassUserId { get; set; }

      public string encompassPassword { get; set; }

      public string apiClientId { get; set; }

      public string apiClientSecret { get; set; }

      public string environment { get; set; }

      public EncompassConfigRequest()
      {
        this.clientId = string.Empty;
        this.instanceId = string.Empty;
        this.devConnectApiUserId = string.Empty;
        this.encompassUserId = string.Empty;
        this.encompassPassword = string.Empty;
        this.apiClientId = string.Empty;
        this.apiClientSecret = string.Empty;
        this.environment = string.Empty;
      }
    }

    public class MainEncompassConnConfig
    {
      public AIQServerClient.EncompassConnConfig encompassConnectionConfiguration { get; set; }
    }

    public class EncompassConnConfig
    {
      public string siteId { get; set; }

      public string clientId { get; set; }

      public string instanceId { get; set; }

      public string devConnectApiUserId { get; set; }

      public string encompassUserId { get; set; }

      public string encompassPassword { get; set; }

      public string apiClientId { get; set; }

      public string apiClientSecret { get; set; }

      public string environment { get; set; }

      public string deploymentStatus { get; set; }

      public string status { get; set; }

      public string errorMessage { get; set; }

      public EncompassConnConfig()
      {
        this.siteId = string.Empty;
        this.clientId = string.Empty;
        this.instanceId = string.Empty;
        this.devConnectApiUserId = string.Empty;
        this.encompassUserId = string.Empty;
        this.encompassPassword = string.Empty;
        this.apiClientId = string.Empty;
        this.apiClientSecret = string.Empty;
        this.environment = string.Empty;
        this.deploymentStatus = string.Empty;
        this.status = string.Empty;
        this.errorMessage = string.Empty;
      }
    }

    public class LaunchResponce
    {
      public string status { get; set; }

      public string launchConfigurationCode { get; set; }

      public string LaunchAIQSession { get; set; }

      public string errorMessage { get; set; }
    }
  }
}
