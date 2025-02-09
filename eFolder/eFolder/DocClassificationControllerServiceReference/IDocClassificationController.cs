// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference.IDocClassificationController
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.ServiceModel;

#nullable disable
namespace EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference
{
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  [ServiceContract(Namespace = "http://www.elliemae.com/edm/platform", ConfigurationName = "DocClassificationControllerServiceReference.IDocClassificationController")]
  public interface IDocClassificationController
  {
    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/SuggestTraining", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/SuggestTrainingResponse")]
    PageClass SuggestTraining(
      string clientId,
      string userId,
      string password,
      string fileId,
      string documentTitle,
      string suggestedBy,
      byte[] content);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/SuggestTrainingFromTemplate", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/SuggestTrainingFromTemplateResponse")]
    PageClass SuggestTrainingFromTemplate(
      string clientId,
      string userId,
      string password,
      string fileId,
      string documentTitle,
      string suggestedBy,
      string templateTitle);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetDocumentClassList", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetDocumentClassListResponse")]
    DocumentClass[] GetDocumentClassList(string clientId, string userId, string password);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/AddDocumentClass", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/AddDocumentClassResponse")]
    DocumentClass AddDocumentClass(
      string clientId,
      string userId,
      string password,
      string documentTitle,
      string addedBy);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/DeleteDocumentClass", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/DeleteDocumentClassResponse")]
    DocumentClass DeleteDocumentClass(
      string clientId,
      string userId,
      string password,
      string documentGuid,
      string deletedBy);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageClassList", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageClassListResponse")]
    PageClass[] GetPageClassList(
      string clientId,
      string userId,
      string password,
      string documentGuid);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageTemplateList", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageTemplateListResponse")]
    DocumentClass[] GetPageTemplateList(string clientId, string userId, string password);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageTemplateData", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageTemplateDataResponse")]
    PageClassData GetPageTemplateData(
      string clientId,
      string userId,
      string password,
      string templateTitle);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageClassData", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/GetPageClassDataResponse")]
    PageClassData GetPageClassData(
      string clientId,
      string userId,
      string password,
      string pageGuid);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/ApproveTraining", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/ApproveTrainingResponse")]
    PageClass ApproveTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string approvedBy);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/RejectTraining", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/RejectTrainingResponse")]
    PageClass RejectTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string rejectedBy);

    [OperationContract(Action = "http://www.elliemae.com/edm/platform/IDocClassificationController/DeleteTraining", ReplyAction = "http://www.elliemae.com/edm/platform/IDocClassificationController/DeleteTrainingResponse")]
    PageClass DeleteTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string deletedBy);
  }
}
