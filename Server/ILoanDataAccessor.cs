// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ILoanDataAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.FileFormats;
using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public interface ILoanDataAccessor : IDisposable
  {
    Elli.Domain.Mortgage.Loan SaveLoan(
      string loanXml,
      UserInfo currentUser,
      string folderName,
      string loanName,
      LoanFileFormatType fileFormatType);

    string GetLoan(Guid loanId, LoanFileFormatType fileFormatType);

    void DeleteLoan(Guid loanId);

    void MoveLoanToFolder(Guid encompassGuid, string destinationFolderName, string userId);

    void SaveLoanSnapshotObject(
      Guid loanId,
      Guid snapshotGuid,
      LogSnapshotType type,
      SnapshotObject data);

    SnapshotObject GetLoanSnapshotObject(Guid loanId, Guid snapshotGuid, LogSnapshotType type);

    void SaveLoanEventLogXml(Guid loanId, string xml);

    string GetLoanEventLogXml(Guid loanId);

    void SaveLoanAttachments(Guid loanId, FileAttachment[] attachments);

    FileAttachment[] GetLoanAttachments(
      Guid loanId,
      Func<XmlDocument, FileAttachment[]> deserializer);
  }
}
