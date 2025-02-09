// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.ConsentServiceControllerClient
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class ConsentServiceControllerClient : 
    ClientBase<IConsentServiceController>,
    IConsentServiceController
  {
    public ConsentServiceControllerClient()
    {
    }

    public ConsentServiceControllerClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public ConsentServiceControllerClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public ConsentServiceControllerClient(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public ConsentServiceControllerClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ClientConsentDataSaveResponse IConsentServiceController.ClientConsentDataSave(
      ClientConsentDataSaveRequest request)
    {
      return this.Channel.ClientConsentDataSave(request);
    }

    public Security.ClientConsentDataSaveResponseClientConsentDataSaveResponseBody ClientConsentDataSave(
      Security Security,
      Security.ClientConsentDataSaveRequestClientConsentDataSaveRequestBody ClientConsentDataSaveRequest1)
    {
      return ((IConsentServiceController) this).ClientConsentDataSave(new ClientConsentDataSaveRequest()
      {
        Security = Security,
        ClientConsentDataSaveRequest1 = ClientConsentDataSaveRequest1
      }).ClientConsentDataSaveResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ClientConsentDataSaveResponse> IConsentServiceController.ClientConsentDataSaveAsync(
      ClientConsentDataSaveRequest request)
    {
      return this.Channel.ClientConsentDataSaveAsync(request);
    }

    public Task<ClientConsentDataSaveResponse> ClientConsentDataSaveAsync(
      Security Security,
      Security.ClientConsentDataSaveRequestClientConsentDataSaveRequestBody ClientConsentDataSaveRequest1)
    {
      return ((IConsentServiceController) this).ClientConsentDataSaveAsync(new ClientConsentDataSaveRequest()
      {
        Security = Security,
        ClientConsentDataSaveRequest1 = ClientConsentDataSaveRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ConsentClientDataGetResponse IConsentServiceController.ClientConsentDataGet(
      ConsentClientDataGetRequest request)
    {
      return this.Channel.ClientConsentDataGet(request);
    }

    public Security.ConsentClientDataGetResponseConsentClientDataGetResponseBody ClientConsentDataGet(
      Security Security,
      Security.ConsentClientDataGetRequestConsentClientDataGetRequestBody ConsentClientDataGetRequest1)
    {
      return ((IConsentServiceController) this).ClientConsentDataGet(new ConsentClientDataGetRequest()
      {
        Security = Security,
        ConsentClientDataGetRequest1 = ConsentClientDataGetRequest1
      }).ConsentClientDataGetResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ConsentClientDataGetResponse> IConsentServiceController.ClientConsentDataGetAsync(
      ConsentClientDataGetRequest request)
    {
      return this.Channel.ClientConsentDataGetAsync(request);
    }

    public Task<ConsentClientDataGetResponse> ClientConsentDataGetAsync(
      Security Security,
      Security.ConsentClientDataGetRequestConsentClientDataGetRequestBody ConsentClientDataGetRequest1)
    {
      return ((IConsentServiceController) this).ClientConsentDataGetAsync(new ConsentClientDataGetRequest()
      {
        Security = Security,
        ConsentClientDataGetRequest1 = ConsentClientDataGetRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ConsentGetHtmlResponse IConsentServiceController.ConsentGetHtml(ConsentGetHtmlRequest request)
    {
      return this.Channel.ConsentGetHtml(request);
    }

    public Security.ConsentGetHtmlResponseConsentGetHtmlResponseBody ConsentGetHtml(
      Security Security,
      Security.ConsentGetHtmlRequestConsentGetHtmlRequestBody ConsentGetHtmlRequest1)
    {
      return ((IConsentServiceController) this).ConsentGetHtml(new ConsentGetHtmlRequest()
      {
        Security = Security,
        ConsentGetHtmlRequest1 = ConsentGetHtmlRequest1
      }).ConsentGetHtmlResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ConsentGetHtmlResponse> IConsentServiceController.ConsentGetHtmlAsync(
      ConsentGetHtmlRequest request)
    {
      return this.Channel.ConsentGetHtmlAsync(request);
    }

    public Task<ConsentGetHtmlResponse> ConsentGetHtmlAsync(
      Security Security,
      Security.ConsentGetHtmlRequestConsentGetHtmlRequestBody ConsentGetHtmlRequest1)
    {
      return ((IConsentServiceController) this).ConsentGetHtmlAsync(new ConsentGetHtmlRequest()
      {
        Security = Security,
        ConsentGetHtmlRequest1 = ConsentGetHtmlRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    IsConsentRequiredResponse IConsentServiceController.IsConsentRequired(
      IsConsentRequiredRequest request)
    {
      return this.Channel.IsConsentRequired(request);
    }

    public Security.IsConsentRequiredResponseIsConsentRequiredResponseBody IsConsentRequired(
      Security Security,
      Security.IsConsentRequiredRequestIsConsentRequiredRequestBody IsConsentRequiredRequest1)
    {
      return ((IConsentServiceController) this).IsConsentRequired(new IsConsentRequiredRequest()
      {
        Security = Security,
        IsConsentRequiredRequest1 = IsConsentRequiredRequest1
      }).IsConsentRequiredResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<IsConsentRequiredResponse> IConsentServiceController.IsConsentRequiredAsync(
      IsConsentRequiredRequest request)
    {
      return this.Channel.IsConsentRequiredAsync(request);
    }

    public Task<IsConsentRequiredResponse> IsConsentRequiredAsync(
      Security Security,
      Security.IsConsentRequiredRequestIsConsentRequiredRequestBody IsConsentRequiredRequest1)
    {
      return ((IConsentServiceController) this).IsConsentRequiredAsync(new IsConsentRequiredRequest()
      {
        Security = Security,
        IsConsentRequiredRequest1 = IsConsentRequiredRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UserConsentDataSaveResponse IConsentServiceController.UserConsentDataSave(
      UserConsentDataSaveRequest request)
    {
      return this.Channel.UserConsentDataSave(request);
    }

    public Security.UserConsentDataSaveResponseUserConsentDataSaveResponseBody UserConsentDataSave(
      Security Security,
      Security.UserConsentDataSaveRequestUserConsentDataSaveRequestBody UserConsentDataSaveRequest1)
    {
      return ((IConsentServiceController) this).UserConsentDataSave(new UserConsentDataSaveRequest()
      {
        Security = Security,
        UserConsentDataSaveRequest1 = UserConsentDataSaveRequest1
      }).UserConsentDataSaveResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<UserConsentDataSaveResponse> IConsentServiceController.UserConsentDataSaveAsync(
      UserConsentDataSaveRequest request)
    {
      return this.Channel.UserConsentDataSaveAsync(request);
    }

    public Task<UserConsentDataSaveResponse> UserConsentDataSaveAsync(
      Security Security,
      Security.UserConsentDataSaveRequestUserConsentDataSaveRequestBody UserConsentDataSaveRequest1)
    {
      return ((IConsentServiceController) this).UserConsentDataSaveAsync(new UserConsentDataSaveRequest()
      {
        Security = Security,
        UserConsentDataSaveRequest1 = UserConsentDataSaveRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    UserConsentDataGetResponse IConsentServiceController.UserConsentDataGet(
      UserConsentDataGetRequest request)
    {
      return this.Channel.UserConsentDataGet(request);
    }

    public Security.UserConsentDataGetResponseUserConsentDataGetResponseBody[] UserConsentDataGet(
      Security Security,
      Security.UserConsentDataGetRequestUserConsentDataGetRequestBody UserConsentDataGetRequest1)
    {
      return ((IConsentServiceController) this).UserConsentDataGet(new UserConsentDataGetRequest()
      {
        Security = Security,
        UserConsentDataGetRequest1 = UserConsentDataGetRequest1
      }).UserConsentDataGetResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<UserConsentDataGetResponse> IConsentServiceController.UserConsentDataGetAsync(
      UserConsentDataGetRequest request)
    {
      return this.Channel.UserConsentDataGetAsync(request);
    }

    public Task<UserConsentDataGetResponse> UserConsentDataGetAsync(
      Security Security,
      Security.UserConsentDataGetRequestUserConsentDataGetRequestBody UserConsentDataGetRequest1)
    {
      return ((IConsentServiceController) this).UserConsentDataGetAsync(new UserConsentDataGetRequest()
      {
        Security = Security,
        UserConsentDataGetRequest1 = UserConsentDataGetRequest1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ConsentPDFGetResponse IConsentServiceController.ConsentPDFGet(ConsentPDFGetRequest request)
    {
      return this.Channel.ConsentPDFGet(request);
    }

    public Security.ConsentPdfResponse[] ConsentPDFGet(
      Security Security,
      Security.ConsentPDFGetRequestConsentPDFGetRequestBody ConsentPDFGetRequest1)
    {
      return ((IConsentServiceController) this).ConsentPDFGet(new ConsentPDFGetRequest()
      {
        Security = Security,
        ConsentPDFGetRequest1 = ConsentPDFGetRequest1
      }).ConsentPDFGetResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ConsentPDFGetResponse> IConsentServiceController.ConsentPDFGetAsync(
      ConsentPDFGetRequest request)
    {
      return this.Channel.ConsentPDFGetAsync(request);
    }

    public Task<ConsentPDFGetResponse> ConsentPDFGetAsync(
      Security Security,
      Security.ConsentPDFGetRequestConsentPDFGetRequestBody ConsentPDFGetRequest1)
    {
      return ((IConsentServiceController) this).ConsentPDFGetAsync(new ConsentPDFGetRequest()
      {
        Security = Security,
        ConsentPDFGetRequest1 = ConsentPDFGetRequest1
      });
    }

    public UserConsentDataGetResponse UserConsentDataGet(UserConsentDataGetRequest request)
    {
      return this.Channel.UserConsentDataGet(request);
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ClientConsentDataSaveResponse IConsentServiceController.ClientConsentDataSaveVersionMigration(
      ClientConsentDataSaveRequestVersionMigration request)
    {
      return this.Channel.ClientConsentDataSaveVersionMigration(request);
    }

    public Security.ClientConsentDataSaveResponseClientConsentDataSaveResponseBody ClientConsentDataSaveVersionMigration(
      Security.ClientSecurity ClientSecurity,
      Security.Client ClientConsentDataSaveRequestVersionMigration1)
    {
      return ((IConsentServiceController) this).ClientConsentDataSaveVersionMigration(new ClientConsentDataSaveRequestVersionMigration()
      {
        ClientSecurity = ClientSecurity,
        ClientConsentDataSaveRequestVersionMigration1 = ClientConsentDataSaveRequestVersionMigration1
      }).ClientConsentDataSaveResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ClientConsentDataSaveResponse> IConsentServiceController.ClientConsentDataSaveVersionMigrationAsync(
      ClientConsentDataSaveRequestVersionMigration request)
    {
      return this.Channel.ClientConsentDataSaveVersionMigrationAsync(request);
    }

    public Task<ClientConsentDataSaveResponse> ClientConsentDataSaveVersionMigrationAsync(
      Security.ClientSecurity ClientSecurity,
      Security.Client ClientConsentDataSaveRequestVersionMigration1)
    {
      return ((IConsentServiceController) this).ClientConsentDataSaveVersionMigrationAsync(new ClientConsentDataSaveRequestVersionMigration()
      {
        ClientSecurity = ClientSecurity,
        ClientConsentDataSaveRequestVersionMigration1 = ClientConsentDataSaveRequestVersionMigration1
      });
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    ConsentSaveExternalResponse IConsentServiceController.ConsentSaveExternal(
      ConsentSaveExternalRequest request)
    {
      return this.Channel.ConsentSaveExternal(request);
    }

    public Security.ConsentSaveExternalResponseConsentSaveExternalResponseBody ConsentSaveExternal(
      Security.ClientSecurity ClientSecurity,
      Security.ConsentSaveExternalRequestConsentSaveExternalRequestBody ConsentSaveExternalRequest1)
    {
      return ((IConsentServiceController) this).ConsentSaveExternal(new ConsentSaveExternalRequest()
      {
        ClientSecurity = ClientSecurity,
        ConsentSaveExternalRequest1 = ConsentSaveExternalRequest1
      }).ConsentSaveExternalResponse1;
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    Task<ConsentSaveExternalResponse> IConsentServiceController.ConsentSaveExternalAsync(
      ConsentSaveExternalRequest request)
    {
      return this.Channel.ConsentSaveExternalAsync(request);
    }

    public Task<ConsentSaveExternalResponse> ConsentSaveExternalAsync(
      Security.ClientSecurity ClientSecurity,
      Security.ConsentSaveExternalRequestConsentSaveExternalRequestBody ConsentSaveExternalRequest1)
    {
      return ((IConsentServiceController) this).ConsentSaveExternalAsync(new ConsentSaveExternalRequest()
      {
        ClientSecurity = ClientSecurity,
        ConsentSaveExternalRequest1 = ConsentSaveExternalRequest1
      });
    }

    public string GetLoanLevelConsentTracking(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentTracking(input);
    }

    public Task<string> GetLoanLevelConsentTrackingAsync(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentTrackingAsync(input);
    }

    public string GetLoanLevelConsentPdf(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentPdf(input);
    }

    public Task<string> GetLoanLevelConsentPdfAsync(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentPdfAsync(input);
    }
  }
}
