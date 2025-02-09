// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.WebServices.IConsentServiceController
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.WebServices
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "EllieMae.EMLite.WebServices.Security.IConsentServiceController")]
  public interface IConsentServiceController
  {
    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveResponse")]
    ClientConsentDataSaveResponse ClientConsentDataSave(ClientConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveResponse")]
    Task<ClientConsentDataSaveResponse> ClientConsentDataSaveAsync(
      ClientConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataGetResponse")]
    ConsentClientDataGetResponse ClientConsentDataGet(ConsentClientDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataGetResponse")]
    Task<ConsentClientDataGetResponse> ClientConsentDataGetAsync(ConsentClientDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentGetHtml", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentGetHtmlResponse")]
    ConsentGetHtmlResponse ConsentGetHtml(ConsentGetHtmlRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentGetHtml", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentGetHtmlResponse")]
    Task<ConsentGetHtmlResponse> ConsentGetHtmlAsync(ConsentGetHtmlRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/IsConsentRequired", ReplyAction = "http://tempuri.org/IConsentServiceController/IsConsentRequiredResponse")]
    IsConsentRequiredResponse IsConsentRequired(IsConsentRequiredRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/IsConsentRequired", ReplyAction = "http://tempuri.org/IConsentServiceController/IsConsentRequiredResponse")]
    Task<IsConsentRequiredResponse> IsConsentRequiredAsync(IsConsentRequiredRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataSaveResponse")]
    UserConsentDataSaveResponse UserConsentDataSave(UserConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataSaveResponse")]
    Task<UserConsentDataSaveResponse> UserConsentDataSaveAsync(UserConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataGetResponse")]
    UserConsentDataGetResponse UserConsentDataGet(UserConsentDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataGetResponse")]
    Task<UserConsentDataGetResponse> UserConsentDataGetAsync(UserConsentDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentPDFGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentPDFGetResponse")]
    ConsentPDFGetResponse ConsentPDFGet(ConsentPDFGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentPDFGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentPDFGetResponse")]
    Task<ConsentPDFGetResponse> ConsentPDFGetAsync(ConsentPDFGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigration", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigrationResponse")]
    ClientConsentDataSaveResponse ClientConsentDataSaveVersionMigration(
      ClientConsentDataSaveRequestVersionMigration request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigration", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigrationResponse")]
    Task<ClientConsentDataSaveResponse> ClientConsentDataSaveVersionMigrationAsync(
      ClientConsentDataSaveRequestVersionMigration request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentSaveExternal", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentSaveExternalResponse")]
    ConsentSaveExternalResponse ConsentSaveExternal(ConsentSaveExternalRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentSaveExternal", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentSaveExternalResponse")]
    Task<ConsentSaveExternalResponse> ConsentSaveExternalAsync(ConsentSaveExternalRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTracking", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTrackingResponse")]
    string GetLoanLevelConsentTracking(ConsentModelInput input);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTracking", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTrackingResponse")]
    Task<string> GetLoanLevelConsentTrackingAsync(ConsentModelInput input);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentPdf", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentPdfResponse")]
    string GetLoanLevelConsentPdf(ConsentModelInput input);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentPdf", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentPdfResponse")]
    Task<string> GetLoanLevelConsentPdfAsync(ConsentModelInput input);
  }
}
