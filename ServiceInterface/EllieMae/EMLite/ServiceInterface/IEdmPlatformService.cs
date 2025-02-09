// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IEdmPlatformService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using Elli.Domain.Mortgage;
using EllieMae.EMLite.ClientServer.eFolder;
using System;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IEdmPlatformService : IContextBoundObject
  {
    byte[] GetAttachment(Guid loanId, string attachmentId, out FileAttachment file);

    DocumentLogAttachments[] GetDocumentLogList(Guid loanId, string fileFilterText);

    DocumentLogAttachments[] GetDocumentLogAttachmentsList(Guid loanId);

    DocumentLogAttachments GetDocumentLogAttachments(Guid loanId, Guid docLogId);

    string CreateAttachment(
      Guid encompassLoanId,
      string fileName,
      Guid docId,
      byte[] content,
      string sessionId);

    string CreateAttachment(
      Guid encompassLoanId,
      string fileName,
      byte[] content,
      string documentContainerTitle,
      string requestedFrom,
      string borrowerPairId,
      string dateRequested,
      string dateExpected,
      string dateReceived,
      string comments,
      string sessionId);

    void SaveSupportingData(Guid encompassLoanId, string key, byte[] data);

    byte[] GetSupportingData(Guid encompassLoanId, string key);

    Guid CreateDocumentLog(
      Guid encompassLoanId,
      DocumentLog docLog,
      string fileName,
      byte[] content,
      string sessionId);
  }
}
