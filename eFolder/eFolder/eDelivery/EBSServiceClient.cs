// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.EBSServiceClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class EBSServiceClient
  {
    private const string className = "EBSServiceClient";
    private static readonly string sw = Tracing.SwEFolder;
    private Sessions.Session session;
    private static HttpClient client = new HttpClient();
    private const string EBSServiceUrl = "https://{host}/encompass/v0.9";

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public EBSServiceClient()
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
    }

    public EBSServiceClient(Sessions.Session session)
    {
      this.session = session;
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.session.ServerIdentity.InstanceName, this.session.SessionID, this.session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
    }

    public async Task<List<ContactDetails>> GetLoanContacts(string loanGuid)
    {
      List<ContactDetails> result = (List<ContactDetails>) null;
      string status = "Calling GetLoanContacts method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      List<ContactDetails> loanContacts;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/encompass/v0.9", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{EBSServiceUrl}/loans/{loanGuid}/contacts".Replace("{EBSServiceUrl}", accessToken.ServiceUrl).Replace("{loanGuid}", loanGuid.Replace("{", string.Empty).Replace("}", string.Empty)));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(uriBuilder.Uri);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<List<ContactDetails>>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            if (!ebsServiceError.DisplayMessage.Contains("No document found with LoanId"))
              throw new HttpException((int) response.StatusCode, ebsServiceError.DisplayMessage);
            result = (List<ContactDetails>) null;
          }
        })).ConfigureAwait(false);
        loanContacts = result;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get loan Contacts. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return loanContacts;
    }

    public async Task<GetRecipientURLResponse> GetRecipientURL(GetRecipientURLRequest request)
    {
      GetRecipientURLResponse result = (GetRecipientURLResponse) null;
      string status = "Calling GetRecipientURL method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      GetRecipientURLResponse recipientUrl;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/encompass/v0.9", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{EBSServiceUrl}/tokens".Replace("{EBSServiceUrl}", accessToken.ServiceUrl));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> task = EBSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GetRecipientURLResponse>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            throw new HttpException((int) response.StatusCode, ebsServiceError.DisplayMessage);
          }
        })).ConfigureAwait(false);
        recipientUrl = result;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get loan Contacts. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return recipientUrl;
    }

    public async Task SendNotification(sendNotificationRequest request)
    {
      string status = "Calling SendNotification method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/encompass/v0.9", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{EBSServiceUrl}/loans/{loanGuid}/emails".Replace("{EBSServiceUrl}", accessToken.ServiceUrl).Replace("{loanGuid}", request.loanGuid.ToString()));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> task = EBSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (!response.IsSuccessStatusCode)
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            throw new HttpException((int) response.StatusCode, ebsServiceError.DisplayMessage);
          }
        })).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Send Notification. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
    }

    public async Task<GetCCSiteInfoResponse> GetCCSiteInfo(string userId, string loanGuid)
    {
      GetCCSiteInfoResponse result = (GetCCSiteInfoResponse) null;
      string status = "Calling GetCCSiteInfo method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      GetCCSiteInfoResponse ccSiteInfo;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/encompass/v0.9", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          string str = "{EBSServiceUrl}/users/{userId}/site?loanId={loanId}";
          loanGuid = loanGuid.Replace("{", string.Empty).Replace("}", string.Empty);
          UriBuilder uriBuilder = new UriBuilder(str.Replace("{EBSServiceUrl}", accessToken.ServiceUrl).Replace("{userId}", userId).Replace("{loanId}", loanGuid));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(uriBuilder.Uri);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GetCCSiteInfoResponse>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            throw new HttpException((int) response.StatusCode, ebsServiceError.DisplayMessage);
          }
        })).ConfigureAwait(false);
        ccSiteInfo = result;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get CC Site Info. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return ccSiteInfo;
    }

    public async Task<List<RecipientDetails>> GetRecipientDetails(string loanGuid)
    {
      List<RecipientDetails> result = (List<RecipientDetails>) null;
      string status = "Calling GetRecipientDetails method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      List<RecipientDetails> recipientDetails;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/encompass/v0.9", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(EBSServiceClient.sw, TraceLevel.Verbose, nameof (EBSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("https://{host}/encompass/v3/loans/{loanGuid}/recipients".Replace("{host}", accessToken.HostName).Replace("{loanGuid}", loanGuid.Replace("{", string.Empty).Replace("}", string.Empty)));
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Clear();
          EBSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          EBSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> async = EBSServiceClient.client.GetAsync(uriBuilder.Uri);
          Task.WaitAll((Task) async);
          response = async.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<List<RecipientDetails>>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            EBSServiceError ebsServiceError = JsonConvert.DeserializeObject<EBSServiceError>(await response.Content.ReadAsStringAsync());
            if (!ebsServiceError.DisplayMessage.Contains("No document found with LoanId"))
              throw new HttpException((int) response.StatusCode, ebsServiceError.DisplayMessage);
            result = (List<RecipientDetails>) null;
          }
        })).ConfigureAwait(false);
        recipientDetails = result;
      }
      catch (Exception ex)
      {
        status = "Error in EBS service call to Get recipients. Ex: " + (object) ex;
        Tracing.Log(EBSServiceClient.sw, TraceLevel.Error, nameof (EBSServiceClient), status);
        throw ex;
      }
      return recipientDetails;
    }
  }
}
