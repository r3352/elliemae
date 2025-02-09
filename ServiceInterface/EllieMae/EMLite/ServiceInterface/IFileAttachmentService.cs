// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IFileAttachmentService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.EncompassPlatform.Common.SoapMessages;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IFileAttachmentService : IContextBoundObject
  {
    FileStreamInfoGetResponse GetFileStreamInfo(FileStreamInfoGetRequest request);

    AttachmentsCreateResponse CreateAttachments(AttachmentsCreateRequest request);

    UnassignedAttachmentsGetResponse GetUnassignedAttachments(
      UnassignedAttachmentsGetRequest request);

    AttachmentsGetResponse GetAttachments(AttachmentsGetRequest request);

    AttachmentsDeleteResponse DeleteAttachments(AttachmentsDeleteRequest request);

    CoversheetCreateResponse CreateCoversheet(CoversheetCreateRequest request);

    string AssociateAttachmentToLoan(
      Guid encompassLoanId,
      string fileId,
      string fileTitle,
      string docId,
      string docTitle,
      string requestedFrom,
      string borrowerPairId,
      string sessionId);

    FileAttachment CreateAttachment(
      LoanData loanData,
      CreateAttachmentsReasonType reason,
      string fileId,
      string fileTitle,
      DocumentLog doc,
      Hashtable userAclFeatureRights);

    FileAttachment CreateAttachment(
      LoanData loanData,
      CreateAttachmentsReasonType reason,
      string fileId,
      string fileTitle,
      string folderName,
      DocumentLog doc,
      Hashtable userAclFeatureRights);

    FileAttachment[] GetAttachments(Guid loanId);

    void SaveAttachments(Guid loanId, FileAttachment[] attachmentList);

    FileAttachment CreateDraftLoanAttachment(Guid draftLoanId, string fileId, string title);
  }
}
