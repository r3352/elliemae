// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference.DocClassificationControllerClient
// Assembly: eFolder, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 15B8DCD4-2F94-422C-B40A-C852937E3CDE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\eFolder.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace EllieMae.EMLite.eFolder.DocClassificationControllerServiceReference
{
  [DebuggerStepThrough]
  [GeneratedCode("System.ServiceModel", "4.0.0.0")]
  public class DocClassificationControllerClient : 
    ClientBase<IDocClassificationController>,
    IDocClassificationController
  {
    public DocClassificationControllerClient()
    {
    }

    public DocClassificationControllerClient(string endpointConfigurationName)
      : base(endpointConfigurationName)
    {
    }

    public DocClassificationControllerClient(string endpointConfigurationName, string remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public DocClassificationControllerClient(
      string endpointConfigurationName,
      EndpointAddress remoteAddress)
      : base(endpointConfigurationName, remoteAddress)
    {
    }

    public DocClassificationControllerClient(Binding binding, EndpointAddress remoteAddress)
      : base(binding, remoteAddress)
    {
    }

    public PageClass SuggestTraining(
      string clientId,
      string userId,
      string password,
      string fileId,
      string documentTitle,
      string suggestedBy,
      byte[] content)
    {
      return this.Channel.SuggestTraining(clientId, userId, password, fileId, documentTitle, suggestedBy, content);
    }

    public PageClass SuggestTrainingFromTemplate(
      string clientId,
      string userId,
      string password,
      string fileId,
      string documentTitle,
      string suggestedBy,
      string templateTitle)
    {
      return this.Channel.SuggestTrainingFromTemplate(clientId, userId, password, fileId, documentTitle, suggestedBy, templateTitle);
    }

    public DocumentClass[] GetDocumentClassList(string clientId, string userId, string password)
    {
      return this.Channel.GetDocumentClassList(clientId, userId, password);
    }

    public DocumentClass AddDocumentClass(
      string clientId,
      string userId,
      string password,
      string documentTitle,
      string addedBy)
    {
      return this.Channel.AddDocumentClass(clientId, userId, password, documentTitle, addedBy);
    }

    public DocumentClass DeleteDocumentClass(
      string clientId,
      string userId,
      string password,
      string documentGuid,
      string deletedBy)
    {
      return this.Channel.DeleteDocumentClass(clientId, userId, password, documentGuid, deletedBy);
    }

    public PageClass[] GetPageClassList(
      string clientId,
      string userId,
      string password,
      string documentGuid)
    {
      return this.Channel.GetPageClassList(clientId, userId, password, documentGuid);
    }

    public DocumentClass[] GetPageTemplateList(string clientId, string userId, string password)
    {
      return this.Channel.GetPageTemplateList(clientId, userId, password);
    }

    public PageClassData GetPageTemplateData(
      string clientId,
      string userId,
      string password,
      string templateTitle)
    {
      return this.Channel.GetPageTemplateData(clientId, userId, password, templateTitle);
    }

    public PageClassData GetPageClassData(
      string clientId,
      string userId,
      string password,
      string pageGuid)
    {
      return this.Channel.GetPageClassData(clientId, userId, password, pageGuid);
    }

    public PageClass ApproveTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string approvedBy)
    {
      return this.Channel.ApproveTraining(clientId, userId, password, pageGuid, approvedBy);
    }

    public PageClass RejectTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string rejectedBy)
    {
      return this.Channel.RejectTraining(clientId, userId, password, pageGuid, rejectedBy);
    }

    public PageClass DeleteTraining(
      string clientId,
      string userId,
      string password,
      string pageGuid,
      string deletedBy)
    {
      return this.Channel.DeleteTraining(clientId, userId, password, pageGuid, deletedBy);
    }
  }
}
