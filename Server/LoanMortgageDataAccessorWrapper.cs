// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanMortgageDataAccessorWrapper
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Domain.FileFormats;
using Elli.Domain.Mortgage;
using Elli.ElliEnum;
using Elli.Service;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanMortgageDataAccessorWrapper : ILoanDataAccessor, IDisposable
  {
    private IStorageSettings _storageSettings;
    private StorageMode _storageMode;

    public LoanMortgageDataAccessorWrapper(IServerSettings serverSettings)
    {
      this._storageSettings = serverSettings.GetStorageModeSettings();
      this._storageMode = (StorageMode) serverSettings.GetStorageSetting("DataStore.StorageMode");
    }

    public string GetLoan(Guid guid, LoanFileFormatType fileFormatType)
    {
      return IocContainer.CreateMortgageService(this._storageSettings, this._storageMode).LoanFileExportEncompassId(guid, fileFormatType);
    }

    public Elli.Domain.Mortgage.Loan SaveLoan(
      string data,
      UserInfo currentUser,
      string loanFolder,
      string loanName,
      LoanFileFormatType fileFormatType)
    {
      return IocContainer.CreateMortgageService(this._storageSettings, this._storageMode).LoanFileImport(data, fileFormatType, loanFolder, currentUser.Userid, LoanHistoryActionType.Save);
    }

    public void DeleteLoan(Guid guid)
    {
      IocContainer.CreateMortgageService(this._storageSettings, this._storageMode).LoanDeleteEncompassIdComplete(guid);
    }

    public string GetLoanEventLogXml(Guid loanId) => throw new NotImplementedException();

    public void SaveLoanEventLogXml(Guid loanId, string xml) => throw new NotImplementedException();

    public SnapshotObject GetLoanSnapshotObject(
      Guid loanId,
      Guid snapshotGuid,
      LogSnapshotType type)
    {
      throw new NotImplementedException();
    }

    public void SaveLoanSnapshotObject(
      Guid loanId,
      Guid snapshotGuid,
      LogSnapshotType type,
      SnapshotObject data)
    {
      throw new NotImplementedException();
    }

    public void MoveLoanToFolder(Guid encompassGuid, string destinationFolderName, string userId)
    {
      IocContainer.CreateMortgageService(this._storageSettings, this._storageMode).MoveLoanToAnotherFolder(encompassGuid, destinationFolderName, userId);
    }

    public void SaveLoanAttachments(Guid loanId, FileAttachment[] attachments)
    {
      throw new NotImplementedException();
    }

    public FileAttachment[] GetLoanAttachments(
      Guid loanId,
      Func<XmlDocument, FileAttachment[]> deserializer)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
    }
  }
}
