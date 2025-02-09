// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Services.EBSServiceClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Services
{
  public class EBSServiceClient
  {
    private const string className = "EBSServiceClient�";
    private static readonly string sw = Tracing.SwEFolder;
    private string EBSServiceUrl;
    private string EBSServiceUrlv3;
    public OAPIServices.AccessToken AccessToken;
    public string accessToken;
    private static HttpClient client = new HttpClient();

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public EBSServiceClient(string instanceName, string sessionID, string oapiBaseUri)
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(instanceName, sessionID, oapiBaseUri, new RetrySettings());
      this.EBSServiceUrl = "https://{host}/encompass/v0.9";
    }

    public EBSServiceClient(
      string instanceName,
      string sessionID,
      string oapiBaseUri,
      RetrySettings settings)
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(instanceName, sessionID, oapiBaseUri, settings);
      this.EBSServiceUrl = "https://{host}/encompass/v0.9";
      this.EBSServiceUrlv3 = "https://{host}/encompass/v3";
    }

    public async Task<List<ContactDetails>> GetLoanContacts(string loanGuid)
    {
      string status = "Calling GetLoanContacts method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      List<ContactDetails> contacts = (List<ContactDetails>) null;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          string str1 = "{EBSServiceUrl}/loans/{loanGuid}/contacts";
          string str2 = this.EBSServiceUrl.Replace("{host}", AccessToken.HostName);
          UriBuilder uriBuilder = new UriBuilder(str1.Replace("{EBSServiceUrl}", str2).Replace("{loanGuid}", loanGuid.Replace("{", string.Empty).Replace("}", string.Empty)));
          ServicePointManager.FindServicePoint(new Uri(str2)).ConnectionLeaseTimeout = 60000;
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(uriBuilder.Uri);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            contacts = JsonConvert.DeserializeObject<List<ContactDetails>>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            if (!ebsServiceError.DisplayMessage.Contains("No document found with LoanId"))
              throw new Exception(ebsServiceError.DisplayMessage);
            contacts = (List<ContactDetails>) null;
          }
        }));
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get loan Contacts. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return contacts;
    }

    private string createCCLoanErrorUrl(
      string firstName,
      string lastName,
      string loanGuid,
      string startDate,
      string endDate,
      string ebsServiceUrl)
    {
      string ccLoanErrorUrl = ebsServiceUrl + "/errors?filter=startDate::" + startDate + ",endDate::" + endDate;
      if (firstName != "")
        ccLoanErrorUrl = ccLoanErrorUrl + ",borrowerFirstName::" + Uri.EscapeDataString(firstName);
      if (lastName != "")
        ccLoanErrorUrl = ccLoanErrorUrl + ",borrowerLastName::" + Uri.EscapeDataString(lastName);
      if (loanGuid != "")
        ccLoanErrorUrl = ccLoanErrorUrl + ",loanId::" + Uri.EscapeDataString(loanGuid);
      return ccLoanErrorUrl;
    }

    public async Task<List<LoanErrorEntry>> GetCCLoanErrors(
      string firstName,
      string lastName,
      string loanGuid,
      string startDate,
      string endDate)
    {
      string status = "Calling GetCCLoanErrors method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      string responseString = string.Empty;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          string str = this.EBSServiceUrl.Replace("{host}", AccessToken.HostName);
          string ccLoanErrorUrl = this.createCCLoanErrorUrl(firstName, lastName, loanGuid, startDate, endDate, str);
          ServicePointManager.FindServicePoint(new Uri(str)).ConnectionLeaseTimeout = 60000;
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(ccLoanErrorUrl);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            responseString = await response.Content.ReadAsStringAsync();
          }
          else
          {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
              throw new HttpException(401, "Unauthorized access");
            responseString = await response.Content.ReadAsStringAsync();
            throw new Exception(JsonConvert.DeserializeObject<EBSServiceError>(responseString).DisplayMessage);
          }
        }));
      }
      catch (HttpException ex)
      {
        throw ex;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to get consumer connect loan errors. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return JsonConvert.DeserializeObject<LoanErrorEBSResponse>(responseString).errors;
    }

    public async Task<GetRecipientURLResponse> GetRecipientURL(GetRecipientURLRequest request)
    {
      string status = "Calling GetRecipientURL method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      string responseString = string.Empty;
      GetRecipientURLResponse getRecipientURLResponse = (GetRecipientURLResponse) null;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          string str1 = "{EBSServiceUrl}/tokens";
          string str2 = this.EBSServiceUrl.Replace("{host}", AccessToken.HostName);
          UriBuilder uriBuilder = new UriBuilder(str1.Replace("{EBSServiceUrl}", str2));
          ServicePointManager.FindServicePoint(new Uri(str2)).ConnectionLeaseTimeout = 60000;
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
          Task<HttpResponseMessage> task = EBSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (response.IsSuccessStatusCode)
          {
            responseString = await response.Content.ReadAsStringAsync();
            getRecipientURLResponse = JsonConvert.DeserializeObject<GetRecipientURLResponse>(responseString);
          }
          else
          {
            responseString = await response.Content.ReadAsStringAsync();
            throw new Exception(JsonConvert.DeserializeObject<EBSServiceError>(responseString).DisplayMessage);
          }
        }));
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get loan Contacts. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return getRecipientURLResponse;
    }

    public async Task SendNotification(sendNotificationRequest request)
    {
      string msg1 = "Calling SendNotification method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      string responseString = string.Empty;
      EBSServiceError errorDetails = (EBSServiceError) null;
      try
      {
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), msg1);
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
        {
          UriBuilder uriBuilder = new UriBuilder("{EBSServiceUrl}/loans/{loanGuid}/emails".Replace("{EBSServiceUrl}", this.EBSServiceUrl.Replace("{host}", AccessToken.HostName)).Replace("{loanGuid}", request.loanGuid.ToString()));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
          Task<HttpResponseMessage> task = EBSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (!response.IsSuccessStatusCode)
          {
            responseString = await response.Content.ReadAsStringAsync();
            errorDetails = JsonConvert.DeserializeObject<EBSServiceError>(responseString);
            throw new Exception(errorDetails.DisplayMessage);
          }
        }));
      }
      catch (Exception ex)
      {
        string msg2 = "Error in EBS service call to Send Notification. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), msg2);
        throw ex;
      }
    }

    public async Task<GetCCSiteInfoResponse> GetCCSiteInfo(string userId, string loanGuid)
    {
      string status = "Calling GetCCSiteInfo method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      string responseString = string.Empty;
      GetCCSiteInfoResponse getInfoResponse = (GetCCSiteInfoResponse) null;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync((Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async AccessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          string str = "{EBSServiceUrl}/users/{userId}/site?loanId={loanId}";
          loanGuid = loanGuid.Replace("{", string.Empty).Replace("}", string.Empty);
          string newValue = this.EBSServiceUrl.Replace("{host}", AccessToken.HostName);
          UriBuilder uriBuilder = new UriBuilder(str.Replace("{EBSServiceUrl}", newValue).Replace("{userId}", userId).Replace("{loanId}", loanGuid));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", AccessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(uriBuilder.Uri);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            responseString = await response.Content.ReadAsStringAsync();
            getInfoResponse = JsonConvert.DeserializeObject<GetCCSiteInfoResponse>(responseString);
          }
          else
          {
            responseString = await response.Content.ReadAsStringAsync();
            throw new Exception(JsonConvert.DeserializeObject<EBSServiceError>(responseString).DisplayMessage);
          }
        }));
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get CC Site Info. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return getInfoResponse;
    }

    public async Task<Dictionary<string, string>> GetLaonFieldIDValue(
      string loanguid,
      string[] fieldIds)
    {
      string status = "Calling Get loan fields method";
      string GetFieldURL = "{EBSServiceUrlv3}/loans/{loanGuid}/fieldReader";
      HttpResponseMessage response = (HttpResponseMessage) null;
      Dictionary<string, string> laonFieldIdValue;
      try
      {
        Dictionary<string, string> result = (Dictionary<string, string>) null;
        await this.ReauthenticateOnUnauthorised.ExecuteAsync(this.EBSServiceUrlv3, (string) null, (Func<EllieMae.EMLite.ClientServer.Authentication.AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder(GetFieldURL.Replace("{EBSServiceUrlv3}", accessToken.ServiceUrl).Replace("{loanGuid}", loanguid.Replace("{", string.Empty).Replace("}", string.Empty)));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          StringContent content = new StringContent(JsonConvert.SerializeObject((object) fieldIds), Encoding.UTF8, "application/json");
          Task<HttpResponseMessage> task = EBSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) content);
          Task.WaitAll((Task) task);
          response = task.Result;
          result = response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Dictionary<string, string>>(await response.Content.ReadAsStringAsync()) : throw new Exception(JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync()).DisplayMessage);
        })).ConfigureAwait(false);
        laonFieldIdValue = result;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to get loan field value. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return laonFieldIdValue;
    }
  }
}
