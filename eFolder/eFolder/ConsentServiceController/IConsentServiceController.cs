// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.ConsentServiceController.IConsentServiceController
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.eFolder.ConsentServiceController
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(ConfigurationName = "ConsentServiceController.IConsentServiceController")]
  public interface IConsentServiceController
  {
    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveResponse")]
    ClientConsentDataSaveResponse ClientConsentDataSave(ClientConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataGetResponse")]
    ConsentClientDataGetResponse ClientConsentDataGet(ConsentClientDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentGetHtml", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentGetHtmlResponse")]
    ConsentGetHtmlResponse ConsentGetHtml(ConsentGetHtmlRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/IsConsentRequired", ReplyAction = "http://tempuri.org/IConsentServiceController/IsConsentRequiredResponse")]
    IsConsentRequiredResponse IsConsentRequired(IsConsentRequiredRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataSave", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataSaveResponse")]
    UserConsentDataSaveResponse UserConsentDataSave(UserConsentDataSaveRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/UserConsentDataGet", ReplyAction = "http://tempuri.org/IConsentServiceController/UserConsentDataGetResponse")]
    UserConsentDataGetResponse UserConsentDataGet(UserConsentDataGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ConsentPDFGet", ReplyAction = "http://tempuri.org/IConsentServiceController/ConsentPDFGetResponse")]
    ConsentPDFGetResponse ConsentPDFGet(ConsentPDFGetRequest request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigration", ReplyAction = "http://tempuri.org/IConsentServiceController/ClientConsentDataSaveVersionMigrationResponse")]
    ClientConsentDataSaveResponse ClientConsentDataSaveVersionMigration(
      ClientConsentDataSaveRequestVersionMigration request);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTracking", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTrackingResponse")]
    string GetLoanLevelConsentTracking(ConsentModelInput input);

    [OperationContract(Action = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTracking", ReplyAction = "http://tempuri.org/IConsentServiceController/GetLoanLevelConsentTrackingResponse")]
    Task<string> GetLoanLevelConsentTrackingAsync(ConsentModelInput input);
  }
}
