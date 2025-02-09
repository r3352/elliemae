// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.LoanCenter.ConsentServiceClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Authentication;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.eFolder.ConsentServiceController;
using EllieMae.EMLite.eFolder.WcfExtensions;
using EllieMae.EMLite.RemotingServices;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.eFolder.LoanCenter
{
  public class ConsentServiceClient
  {
    private const string className = "ConsentServiceClient";
    private readonly ILogger logger;
    private Sessions.Session session;

    private ReauthenticateOnUnauthorised ReauthenticateOnUnauthorised { get; set; }

    public ConsentServiceClient()
      : this(Session.DefaultInstance)
    {
    }

    public ConsentServiceClient(Sessions.Session session)
    {
      this.session = session;
      this.logger = DiagUtility.LogManager.GetLogger(Tracing.SwEFolder);
    }

    public bool ClientConsentDataGet(AclFeature feature)
    {
      ConsentServiceControllerClient consentServiceClient = this.getConsentServiceClient();
      ConsentClientDataGetRequest request = new ConsentClientDataGetRequest();
      request.Security = this.getSecurityHeader();
      request.ConsentClientDataGetRequest1 = new ConsentClientDataGetRequestConsentClientDataGetRequestBody()
      {
        ClientId = this.session.CompanyInfo.ClientID
      };
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "Calling ClientConsentDataGet of ConsentService");
      ConsentClientDataGetResponse clientDataGetResponse = consentServiceClient.ClientConsentDataGet(request);
      if (clientDataGetResponse == null)
      {
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), "ClientConsentDataGet returned a null response. Possible authentication failure.");
        return false;
      }
      ConsentClientDataGetResponseConsentClientDataGetResponseBody dataGetResponse1 = clientDataGetResponse.ConsentClientDataGetResponse1;
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "ClientConsentDataGet response.UserName: " + dataGetResponse1.UserName);
      if (!string.IsNullOrEmpty(dataGetResponse1.StreetAddress) && !string.IsNullOrEmpty(dataGetResponse1.StreetAddress))
      {
        CompanyInfo companyInfo = this.session.ConfigurationManager.GetCompanyInfo();
        this.session.ConfigurationManager.UpdateCompanyInfo(new CompanyInfo((string) null, dataGetResponse1.UserName == null ? "" : dataGetResponse1.UserName, dataGetResponse1.StreetAddress == null ? "" : dataGetResponse1.StreetAddress, dataGetResponse1.City == null ? "" : dataGetResponse1.City, dataGetResponse1.State == null ? "" : dataGetResponse1.State, dataGetResponse1.Zip == null ? "" : dataGetResponse1.Zip, dataGetResponse1.Phone == null ? "" : dataGetResponse1.Phone, dataGetResponse1.Fax == null ? "" : dataGetResponse1.Fax, (string) null, companyInfo.GetDBANames(), companyInfo.StateBranchLicensing), feature);
        this.session.ConfigurationManager.SetCompanySetting("eDisclosures", "FulfillmentFee", dataGetResponse1.FulfillmentFee == null ? "" : dataGetResponse1.FulfillmentFee);
        EDisclosureSetup edisclosureSetup = this.session.ConfigurationManager.GetEDisclosureSetup();
        edisclosureSetup.ConsentModelType = "Loan level consent";
        int? consentModel = dataGetResponse1.ConsentModel;
        int num = 2;
        if (consentModel.GetValueOrDefault() == num & consentModel.HasValue)
          edisclosureSetup.ConsentModelType = "Package level consent";
        this.session.ConfigurationManager.SaveEDisclosureSetup(edisclosureSetup);
      }
      return true;
    }

    public bool ClientConsentDataSave()
    {
      ConsentServiceControllerClient consentServiceClient = this.getConsentServiceClient();
      ClientConsentDataSaveRequest request = new ClientConsentDataSaveRequest();
      request.Security = this.getSecurityHeader();
      CompanyInfo companyInfo = this.session.ConfigurationManager.GetCompanyInfo();
      if (companyInfo == null)
      {
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), "CompanyInfo object not set. Cannot call ClientConsentDataSave.");
        return false;
      }
      if (string.IsNullOrEmpty(companyInfo.ClientID))
      {
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), "Client ID not set. Cannot call ClientConsentDataSave.");
        return false;
      }
      request.ClientConsentDataSaveRequest1 = new ClientConsentDataSaveRequestClientConsentDataSaveRequestBody()
      {
        ClientId = companyInfo.ClientID,
        StreetAddress = companyInfo.Address,
        City = companyInfo.City,
        State = companyInfo.State,
        Zip = companyInfo.Zip,
        UserName = companyInfo.Name,
        Phone = companyInfo.Phone,
        Fax = companyInfo.Fax,
        FulfillmentFee = this.session.ConfigurationManager.GetCompanySetting("eDisclosures", "FulfillmentFee"),
        ConsentModel = !(this.session.ConfigurationManager.GetEDisclosureSetup().ConsentModelType == "Package level consent") ? new int?(1) : new int?(2)
      };
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "Calling ClientConsentDataSave of ConsentService");
      ClientConsentDataSaveResponse dataSaveResponse = consentServiceClient.ClientConsentDataSave(request);
      if (dataSaveResponse == null)
      {
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), "ClientConsentDataSave returned a null response. Possible authentication failure.");
        return false;
      }
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "ClientConsentDataSave response: " + dataSaveResponse.ClientConsentDataSaveResponse1.Success.ToString());
      return dataSaveResponse.ClientConsentDataSaveResponse1.Success;
    }

    public string ClientConsentDataSaveVersionMigration(CompanyInfo company, string fulfillmentFee)
    {
      string message1 = "Entering ClientConsentDataSaveVersionMigration.";
      try
      {
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        message1 = "Get Consent Service client.";
        ConsentServiceControllerClient consentServiceClient = this.getConsentServiceClient();
        if (consentServiceClient == null)
        {
          message1 = "Consent Service Client object is null.";
          this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), message1);
          return message1;
        }
        ClientConsentDataSaveRequestVersionMigration request = new ClientConsentDataSaveRequestVersionMigration();
        message1 = "Set Client security.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        request.ClientSecurity = new ClientSecurity()
        {
          ClientAuthCode = company.Password,
          SecurityClientId = company.ClientID
        };
        message1 = "Create request body.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        Client client = new Client();
        client.ClientId = company.ClientID;
        message1 = "Set company address.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        client.ClientStreetAddress = company.Address == null ? "" : company.Address;
        client.ClientCity = company.City == null ? "" : company.City;
        client.ClientState = company.State == null ? "" : company.State;
        client.ClientZip = company.Zip == null ? "" : company.Zip;
        message1 = "Set company contact info.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        client.ClientUserName = company.Name == null ? "" : company.Name;
        client.ClientFax = company.Fax == null ? "" : company.Fax;
        client.ClientPhone = company.Phone == null ? "" : company.Phone;
        message1 = "Set fulfillment fee.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        client.ClientFulfillmentFee = fulfillmentFee == null ? "" : fulfillmentFee;
        request.ClientConsentDataSaveRequestVersionMigration1 = client;
        message1 = "Calling ClientConsentDataSaveVersionMigration of ConsentService.";
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        ClientConsentDataSaveResponse dataSaveResponse = consentServiceClient.ClientConsentDataSaveVersionMigration(request);
        if (dataSaveResponse == null)
        {
          message1 = "ClientConsentDataSaveVersionMigration returned a null response. Possible authentication failure.";
          this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), message1);
          return message1;
        }
        message1 = "ClientConsentDataSaveVersionMigration response: " + dataSaveResponse.ClientConsentDataSaveResponse1.Success.ToString();
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), message1);
        return dataSaveResponse.ClientConsentDataSaveResponse1.Success ? string.Empty : message1;
      }
      catch (Exception ex)
      {
        string message2 = "Error in ClientConsentDataSaveVersionMigration. " + message1;
        this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.ERROR, nameof (ConsentServiceClient), message2, ex);
        return message2 + (object) ex;
      }
    }

    public UserConsentDataGetResponse UserConsentDataGet(string loanGuid)
    {
      ConsentServiceControllerClient consentServiceClient = this.getConsentServiceClient();
      UserConsentDataGetRequest request = new UserConsentDataGetRequest();
      request.Security = this.getSecurityHeader();
      request.UserConsentDataGetRequest1 = new UserConsentDataGetRequestUserConsentDataGetRequestBody()
      {
        ClientId = this.session.CompanyInfo.ClientID,
        LoanGuid = loanGuid
      };
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "Calling UserConsentDataGet of ConsentService");
      return consentServiceClient.UserConsentDataGet(request);
    }

    public string GetLoanLevelConsentTracking(string loanGuid)
    {
      this.ReauthenticateOnUnauthorised = new ReauthenticateOnUnauthorised(this.session.ServerIdentity.InstanceName, this.session.StartupInfo.SessionID, this.session.StartupInfo.OAPIGatewayBaseUri, new RetrySettings(this.session.SessionObjects));
      ConsentServiceControllerClient svcClient = this.getConsentServiceClient();
      string result = (string) null;
      this.ReauthenticateOnUnauthorised.Execute((Action<AccessToken>) (accessToken =>
      {
        using (new OperationContextScope((IContextChannel) svcClient.InnerChannel))
        {
          OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader("AccessToken", "http://tempuri.org", (object) accessToken.TypeAndToken));
          ConsentModelInput input = new ConsentModelInput();
          input.LoanGuid = loanGuid.Replace("{", string.Empty).Replace("}", string.Empty);
          this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "Calling UserConsentDataGet of ConsentService");
          result = svcClient.GetLoanLevelConsentTracking(input);
        }
      }));
      return result;
    }

    private ConsentServiceControllerClient getConsentServiceClient()
    {
      this.logger.Write(Encompass.Diagnostics.Logging.LogLevel.DEBUG, nameof (ConsentServiceClient), "Initializing Service Client and Endpoint");
      BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
      basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
      string str = this.session.SessionObjects?.StartupInfo?.ServiceUrls?.ConsentServiceUrl;
      if (string.IsNullOrWhiteSpace(str) || !Uri.IsWellFormedUriString(str, UriKind.Absolute))
        str = "https://loancenter.elliemae.com/ConsentService/ConsentServiceController.svc";
      EndpointAddress remoteAddress = new EndpointAddress(str);
      ConsentServiceControllerClient consentServiceClient = new ConsentServiceControllerClient((System.ServiceModel.Channels.Binding) basicHttpBinding, remoteAddress);
      consentServiceClient.ChannelFactory.Endpoint.Behaviors.Add((IEndpointBehavior) new SsoTokenEndpointBehavior());
      return consentServiceClient;
    }

    private Security getSecurityHeader()
    {
      return new Security()
      {
        SecurityClientId = this.session.CompanyInfo.ClientID,
        UserId = this.session.UserID,
        Password = this.session.Password
      };
    }

    public string createNewPassword(Sessions.Session session)
    {
      string newPassword = (string) null;
      try
      {
        if (session.StartupInfo.RuntimeEnvironment == RuntimeEnvironment.Hosted)
          newPassword = session.IdentityManager.GetSsoToken((IEnumerable<string>) new string[1]
          {
            "Elli.Emn"
          }, 5);
      }
      catch (Exception ex)
      {
        int num = (int) Utils.Dialog((IWin32Window) null, "Failed to generate security token for access to ICE Mortgage Technology Network. User account creation in ICE Mortgage Technology Network failed.");
        return "";
      }
      return newPassword;
    }
  }
}
