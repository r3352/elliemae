// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.eDelivery.DMOSServiceClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.eFolder.eDelivery.HelperMethods;
using EllieMae.EMLite.RemotingServices;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#nullable disable
namespace EllieMae.EMLite.eFolder.eDelivery
{
  public class DMOSServiceClient
  {
    private const string className = "DMOSServiceClient";
    private static readonly string sw = Tracing.SwEFolder;
    private static HttpClient client = new HttpClient();
    private const string DMOSServiceUrl = "https://{host}/pos/v1";

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public DMOSServiceClient()
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(Session.ServerIdentity.InstanceName, Session.SessionObjects.SessionID, Session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(Session.SessionObjects));
    }

    public async Task<GetDMOSRecipientURLResponse> GetRecipientURL(
      GetDMOSRecipientURLRequest request)
    {
      foreach (Party party in request.parties)
      {
        if (!party.contact.phoneNumber.StartsWith("+1"))
          party.contact.phoneNumber = "+1" + party.contact.phoneNumber;
      }
      GetDMOSRecipientURLResponse result = (GetDMOSRecipientURLResponse) null;
      string status = "Calling DMOS GetRecipientURL method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      GetDMOSRecipientURLResponse recipientUrl;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/pos/v1", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(DMOSServiceClient.sw, TraceLevel.Verbose, nameof (DMOSServiceClient), status);
          UriBuilder uriBuilder = new UriBuilder("{DMOSServiceUrl}/loans/{loanGuid}/recipients?othersUseLoanConnect=true&lockId={lockId}".Replace("{DMOSServiceUrl}", accessToken.ServiceUrl).Replace("{loanGuid}", request.loanGuid).Replace("{lockId}", Session.SessionObjects.SessionID));
          DMOSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          DMOSServiceClient.client.DefaultRequestHeaders.Clear();
          DMOSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          DMOSServiceClient.client.DefaultRequestHeaders.Add("X-Caller", "name=" + request.caller.name + ";version=" + request.caller.version);
          DMOSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> task = DMOSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) request), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GetDMOSRecipientURLResponse>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            DMOSServiceError dmosServiceError = JsonConvert.DeserializeObject<DMOSServiceError>(await response.Content.ReadAsStringAsync());
            throw new HttpException((int) response.StatusCode, dmosServiceError.details);
          }
        })).ConfigureAwait(false);
        recipientUrl = result;
      }
      catch (Exception ex)
      {
        status = "Error in DMOS service call to create recipients. Ex: " + (object) ex;
        Tracing.Log(DMOSServiceClient.sw, TraceLevel.Error, nameof (DMOSServiceClient), status);
        throw ex;
      }
      return recipientUrl;
    }

    public Party SetPartyDetails(OTPRecipient recipient)
    {
      Party party = (Party) null;
      if (recipient != null)
      {
        party = new Party()
        {
          id = recipient.PartyId,
          fullName = recipient.fullName,
          contact = new DMOSContact()
        };
        party.contact.emailAddress = recipient.email;
        if (!string.IsNullOrEmpty(recipient.PhoneNumber))
        {
          party.contact.phoneNumber = recipient.PhoneNumber;
          party.contact.phoneType = recipient.PhoneType.ToString();
        }
        DMOSRequestHelper.SetPartyEntityType(party, recipient.role, recipient.borrowerId, recipient.userId);
      }
      return party;
    }

    public async Task<GetDMOSAuditTrailResponse> GetAuditTrail(string loanGuid, string packageId)
    {
      GetDMOSAuditTrailResponse result = (GetDMOSAuditTrailResponse) null;
      string status = "Calling DMOS GetAuditTrail method";
      HttpResponseMessage response = (HttpResponseMessage) null;
      GetDMOSAuditTrailResponse auditTrail;
      try
      {
        await this.ReauthenticateOnUnauthorised.ExecuteAsync("https://{host}/pos/v1", (string) null, (Func<AccessToken, Task>) (async accessToken =>
        {
          Tracing.Log(DMOSServiceClient.sw, TraceLevel.Verbose, nameof (DMOSServiceClient), status);
          string str = "{DMOSServiceUrl}/loans/{loanGuid}/packages/{packageId}/auditGenerator";
          loanGuid = loanGuid.Replace("{", "").Replace("}", "");
          UriBuilder uriBuilder = new UriBuilder(str.Replace("{DMOSServiceUrl}", accessToken.ServiceUrl).Replace("{loanGuid}", loanGuid).Replace("{packageId}", packageId));
          DMOSServiceClient.client.DefaultRequestHeaders.Accept.Clear();
          DMOSServiceClient.client.DefaultRequestHeaders.Clear();
          DMOSServiceClient.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
          DMOSServiceClient.client.DefaultRequestHeaders.Add("Authorization", accessToken.TypeAndToken);
          Task<HttpResponseMessage> task = DMOSServiceClient.client.PostAsync(uriBuilder.Uri, (HttpContent) new StringContent(JsonConvert.SerializeObject((object) string.Empty), Encoding.UTF8, "application/json"));
          Task.WaitAll((Task) task);
          response = task.Result;
          if (response.IsSuccessStatusCode)
          {
            result = JsonConvert.DeserializeObject<GetDMOSAuditTrailResponse>(await response.Content.ReadAsStringAsync());
          }
          else
          {
            DMOSServiceError dmosServiceError = JsonConvert.DeserializeObject<DMOSServiceError>(await response.Content.ReadAsStringAsync());
            throw new HttpException((int) response.StatusCode, dmosServiceError.details);
          }
        })).ConfigureAwait(false);
        auditTrail = result;
      }
      catch (Exception ex)
      {
        status = "Error in DMOS service call to get audit trail. Ex: " + (object) ex;
        Tracing.Log(DMOSServiceClient.sw, TraceLevel.Error, nameof (DMOSServiceClient), status);
        throw ex;
      }
      return auditTrail;
    }
  }
}
