// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.IAttachmentAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.eFolder;
using System;
using System.Collections.Generic;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public interface IAttachmentAccessor : IDisposable
  {
    FileAttachment[] GetLoanAttachmentsFromDatabase(
      int loanXRefId,
      IEnumerable<string> attachmentIds = null,
      bool includeRemoved = true);

    IEnumerable<Tuple<XmlElement, bool>> BuildAttachmentXmlFromDbForMigration(
      Guid loanId,
      int loanXrefId,
      bool isDataRequired,
      out string loanPropertiesFlag,
      out string instanceLevelFlag);

    void InsertLoanAttachmentsToDB(
      Guid loanId,
      int loanXRefId,
      FileAttachment[] attachments,
      DbQueryBuilder sql = null);

    void ReInsertData(Guid loanId, int loanXRefId, FileAttachment[] attachments);

    void UpdateLoanAttachmentsToDB(int loanXRefId, FileAttachment[] attachments);

    void DeleteAttachments(int loanXRefId, string attachmentId);

    void DeleteAttachments(int loanXRefId);
  }
}
