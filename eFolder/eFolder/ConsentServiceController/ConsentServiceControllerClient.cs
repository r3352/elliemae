// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.ConsentServiceControllerClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.eFolder.ConsentServiceController
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

    public ClientConsentDataSaveResponse ClientConsentDataSave(ClientConsentDataSaveRequest request)
    {
      return this.Channel.ClientConsentDataSave(request);
    }

    public ConsentClientDataGetResponse ClientConsentDataGet(ConsentClientDataGetRequest request)
    {
      return this.Channel.ClientConsentDataGet(request);
    }

    public ConsentGetHtmlResponse ConsentGetHtml(ConsentGetHtmlRequest request)
    {
      return this.Channel.ConsentGetHtml(request);
    }

    public IsConsentRequiredResponse IsConsentRequired(IsConsentRequiredRequest request)
    {
      return this.Channel.IsConsentRequired(request);
    }

    public UserConsentDataSaveResponse UserConsentDataSave(UserConsentDataSaveRequest request)
    {
      return this.Channel.UserConsentDataSave(request);
    }

    public UserConsentDataGetResponse UserConsentDataGet(UserConsentDataGetRequest request)
    {
      return this.Channel.UserConsentDataGet(request);
    }

    public ConsentPDFGetResponse ConsentPDFGet(ConsentPDFGetRequest request)
    {
      return this.Channel.ConsentPDFGet(request);
    }

    public ClientConsentDataSaveResponse ClientConsentDataSaveVersionMigration(
      ClientConsentDataSaveRequestVersionMigration request)
    {
      return this.Channel.ClientConsentDataSaveVersionMigration(request);
    }

    public string GetLoanLevelConsentTracking(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentTracking(input);
    }

    public Task<string> GetLoanLevelConsentTrackingAsync(ConsentModelInput input)
    {
      return this.Channel.GetLoanLevelConsentTrackingAsync(input);
    }
  }
}
