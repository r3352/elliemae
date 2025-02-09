// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IDocumentService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.EncompassPlatform.Common.DataContracts;
using Elli.EncompassPlatform.Common.SoapMessages;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IDocumentService : IContextBoundObject
  {
    DocumentsCreateResponse CreateDocuments(DocumentsCreateRequest request);

    DocumentsGetResponse GetDocuments(DocumentsGetRequest request);

    DocumentsSaveResponse SaveDocuments(DocumentsSaveRequest request);

    DocumentTemplatesGetResponse GetDocumentTemplates(DocumentTemplatesGetRequest request);

    DocumentAttachmentsAssignResponse AssignDocumentAttachments(
      DocumentAttachmentsAssignRequest request);

    DocumentCommentsCreateResponse CreateComments(DocumentCommentsCreateRequest request);

    DocumentLog CreateDocumentLog(
      LoanData loanData,
      DocumentCreateContract docContract,
      bool? checkExisting,
      Hashtable userAclFeatureRights);
  }
}
