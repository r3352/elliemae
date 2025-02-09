// Decompiled with JetBrains decompiler
// Type: Elli.Service.MortgageService
// Assembly: Elli.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 23E1ED92-4ABE-46FE-97AA-A4B97F8619DF
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.dll

using Elli.Domain;
using Elli.Domain.FileFormats;
using Elli.Domain.Mortgage;
using Elli.Domain.Mortgage.Enums;
using Elli.Metrics;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using EllieMae.EMLite.RemotingServices;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Elli.Service
{
  public class MortgageService : IMortgageService, IDisposable
  {
    private readonly ILoanRepository _loanRepository;
    private IStorageSettings _storgaeSettings;
    private StorageMode _storageMode;
    private IUnitOfWork _unitOfWork;
    private readonly IMetricsFactory _metricsFactory;
    private ILoanMetricsRecorder _metricsRecorder;

    public MortgageService(ILoanRepository repository, IMetricsFactory metricsFactory)
    {
      this._loanRepository = repository;
      this._metricsFactory = metricsFactory;
    }

    internal void ApplyConfiguration(IStorageSettings storageSettings, StorageMode storageMode)
    {
      this._storgaeSettings = storageSettings;
      this._storageMode = storageMode;
      this._unitOfWork = IocContainer.CreateUnitOfWork(this._storgaeSettings, this._storageMode);
    }

    private ILoanMetricsRecorder MetricsRecorder
    {
      get
      {
        return this._metricsRecorder != null ? this._metricsRecorder : (this._metricsRecorder = this._metricsFactory.CreateLoanMetricsRecorder(this._storgaeSettings.ClientId ?? string.Empty, this._storgaeSettings.InstanceId ?? string.Empty));
      }
    }

    public void LoanSave(Loan loan)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime("LoanSave (deprecated)"))
        this.LoanFileImport(loan);
    }

    [Obsolete("This method is deprecated, Please use overloaded method with additonal folderName parameter, userName and LoanHistoryAction")]
    public Loan LoanFileImport(string fileData, LoanFileFormatType fileFormatType)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime("LoanFileImport (deprecated)"))
        return this.LoanFileImport(fileData, fileFormatType, (string) null);
    }

    [Obsolete("This method is deprecated, Please use overloaded method with additonal folderName parameter, userName and LoanHistoryAction")]
    public Loan LoanFileImport(
      string fileData,
      LoanFileFormatType fileFormatType,
      string folderName)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime("LoanFileImport (deprecated)"))
        return this.LoanFileImport(fileData, fileFormatType, folderName, (string) null, LoanHistoryActionType.Save);
    }

    public Loan LoanFileImport(
      string fileData,
      LoanFileFormatType fileFormatType,
      string folderName,
      string userName,
      LoanHistoryActionType actionType)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanFileImport)))
      {
        IFileFormat fileFormat = FileFormat.Create(fileFormatType, fileData);
        Loan completeEncompassId = this._loanRepository.LoanGetCompleteEncompassId(fileFormat.FileId);
        Loan loan = fileFormat.ReadFile(completeEncompassId);
        if (!string.IsNullOrEmpty(folderName))
          loan.AddLoanTag(new TagItem()
          {
            TagType = Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanFolder),
            TagName = folderName
          });
        if (loan.EncompassId == Guid.Empty)
          throw new ApplicationException(string.Format("Loan request does not contains valid EncompassId, please verify request and retry"));
        try
        {
          foreach (CustomField customField in loan.CustomFields)
          {
            if (customField.StringValue == null)
              customField.StringValue = string.Empty;
          }
        }
        catch
        {
        }
        this._loanRepository.LoanSave(loan);
        if (this._storageMode != StorageMode.MongoFileSystemMaster && this._storageMode != StorageMode.MongoMaster && this._storageMode != StorageMode.MongoOnly)
          return loan;
        Guid encompassId = loan.EncompassId;
        this._loanRepository.LoanSnapshotDataSave(loan);
        LoanHistory loanHistory1 = this._loanRepository.LoanHistoryGetComplete(encompassId);
        if (loanHistory1 == null)
          loanHistory1 = new LoanHistory()
          {
            EncompassLoanGuid = encompassId
          };
        LoanHistory loanHistory2 = loanHistory1;
        CompressedLoanFile compressedLoanFile = CompressedLoanFile.Create(encompassId, loan.ToJson<Loan>());
        DateTime utcNow = DateTime.UtcNow;
        compressedLoanFile.Consolidate(loanHistory2, userName, utcNow);
        this._loanRepository.CompressedLoanFileSave(compressedLoanFile);
        LoanHistoryEntry entry = new LoanHistoryEntry()
        {
          Action = actionType,
          CompressedLoanFileId = compressedLoanFile.Id,
          Occurred = utcNow,
          UserId = userName
        };
        loanHistory2.Add(entry);
        this._loanRepository.LoanHistorySave(loanHistory2);
        return loan;
      }
    }

    public Loan LoanFileImportToDataStore(
      string fileData,
      LoanFileFormatType fileFormatType,
      DataStores dataStores = DataStores.ActiveStore)
    {
      IFileFormat fileFormat = FileFormat.Create(fileFormatType, fileData);
      Loan encompassId = this._loanRepository.LoanGetEncompassId(fileFormat.FileId, dataStores);
      Loan loan = fileFormat.ReadFile(encompassId);
      if (dataStores == DataStores.ActiveStore)
        this._loanRepository.LoanSave(loan);
      else
        this._loanRepository.LoanArchiveSave(loan);
      return loan;
    }

    public Loan LoanFileImport(Loan loan)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime("LoanFileImport (deprecated)"))
      {
        if (loan.EncompassId == Guid.Empty)
          throw new ApplicationException(string.Format("Loan request does not contains valid EncompassId, please verify request and retry"));
        IFileFormat fileFormat = FileFormat.Create(LoanFileFormatType.Encompass360, (string) null);
        string folderName = "My Pipleline";
        Loan loan1 = loan;
        string fileData = fileFormat.WriteFile(loan1);
        if (loan.LoanTags != null && loan.LoanTags.Count<TagItem>() > 0)
          folderName = loan.LoanTags.FirstOrDefault<TagItem>((Func<TagItem, bool>) (x => x.TagType == Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanFolder))).TagName;
        return this.LoanFileImport(fileData, LoanFileFormatType.Encompass360, folderName, (string) null, LoanHistoryActionType.Save);
      }
    }

    public Loan LoanGet(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanGet)))
        return this._loanRepository.LoanGet(loanId);
    }

    public Loan LoanGetArchive(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanGetArchive)))
        return this._loanRepository.LoanGet(loanId, DataStores.ArchiveStore);
    }

    public Loan LoanGetComplete(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanGetComplete)))
        return this._loanRepository.LoanGetComplete(loanId);
    }

    public Loan LoanGetCompleteEncompassId(Guid encompassId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanGetCompleteEncompassId)))
        return this._loanRepository.LoanGetCompleteEncompassId(encompassId);
    }

    public string LoanFileExportEncompassId(Guid encompassGuid, LoanFileFormatType fileFormatType)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanFileExportEncompassId)))
      {
        Loan completeEncompassId = this._loanRepository.LoanGetCompleteEncompassId(encompassGuid);
        return completeEncompassId == null ? (string) null : FileFormat.Create(fileFormatType, (string) null).WriteFile(completeEncompassId);
      }
    }

    public string LoanFileExport(Guid loanId, LoanFileFormatType fileFormatType)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanFileExport)))
      {
        Loan complete = this._loanRepository.LoanGetComplete(loanId);
        return complete == null ? (string) null : FileFormat.Create(fileFormatType, (string) null).WriteFile(complete);
      }
    }

    public void LoanDelete(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDelete)))
        this._loanRepository.LoanDelete(loanId);
    }

    public void LoanDeleteArchive(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDeleteArchive)))
        this._loanRepository.LoanDelete(loanId, DataStores.ArchiveStore);
    }

    public void LoanDeleteComplete(Guid loanId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDeleteComplete)))
        this._loanRepository.LoanDeleteComplete(loanId);
    }

    public void LoanDeleteEncompassId(Guid encompassGuid)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDeleteEncompassId)))
        this._loanRepository.LoanDeleteEncompassId(encompassGuid);
    }

    public void LoanDeleteArchiveEncompassId(Guid encompassGuid)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDeleteArchiveEncompassId)))
        this._loanRepository.LoanDeleteEncompassId(encompassGuid, DataStores.ArchiveStore);
    }

    public void LoanDeleteEncompassIdComplete(Guid encompassGuid)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanDeleteEncompassIdComplete)))
      {
        this._loanRepository.LoanDeleteEncompassIdComplete(encompassGuid);
        this._loanRepository.LoanSnapshotDataDeleteComplete(encompassGuid);
        this._loanRepository.LoanHistoryRemoveComplete(encompassGuid);
        this._loanRepository.CompressedLoanFileRemoveComplete(encompassGuid.ToString());
      }
    }

    public Loan MoveLoanToAnotherFolder(
      Guid encompassGuid,
      string destinationFolderName,
      string userId)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (MoveLoanToAnotherFolder)))
      {
        Loan completeEncompassId = this.LoanGetCompleteEncompassId(encompassGuid);
        if (completeEncompassId == null)
          throw new ApplicationException(string.Format("Loan Move operation is failed due to missing loan {0}", (object) encompassGuid));
        completeEncompassId.MoveLoanToAnotherFolder(destinationFolderName);
        this.LoanSave(completeEncompassId);
        return completeEncompassId;
      }
    }

    public void LoanFileArchive(
      string userId,
      string fileData,
      LoanHistoryActionType action,
      DateTime archivedUtc,
      string folderName)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanFileArchive)))
      {
        if (!this._loanRepository.SupportsArchive)
          return;
        IFileFormat fileFormat = FileFormat.Create(LoanFileFormatType.Encompass360, fileData);
        Guid fileId = fileFormat.FileId;
        Loan loan = fileFormat.ReadFile(this._loanRepository.LoanGetEncompassId(fileId, DataStores.ArchiveStore));
        if (!string.IsNullOrEmpty(folderName))
          loan.AddLoanTag(new TagItem()
          {
            TagType = Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanFolder),
            TagName = folderName
          });
        this._loanRepository.LoanArchiveSave(loan);
        this._loanRepository.LoanSnapshotDataSave(loan, DataStores.ArchiveStore);
        LoanHistory loanHistory1 = this._loanRepository.LoanHistoryGet(fileId, DataStores.ArchiveStore);
        if (loanHistory1 == null)
          loanHistory1 = new LoanHistory()
          {
            EncompassLoanGuid = fileId
          };
        LoanHistory loanHistory2 = loanHistory1;
        CompressedLoanFile compressedLoanFile = CompressedLoanFile.Create(fileId, loan.ToJson<Loan>());
        compressedLoanFile.Consolidate(loanHistory2, userId, archivedUtc);
        this._loanRepository.CompressedLoanFileSave(compressedLoanFile, DataStores.ArchiveStore);
        LoanHistoryEntry entry = new LoanHistoryEntry()
        {
          Action = action,
          CompressedLoanFileId = compressedLoanFile.Id,
          Occurred = archivedUtc,
          UserId = userId
        };
        loanHistory2.Add(entry);
        this._loanRepository.LoanHistorySave(loanHistory2, DataStores.ArchiveStore);
      }
    }

    public void LoanFileArchive(
      string userId,
      Loan loan,
      LoanHistoryActionType action,
      DateTime archivedUtc,
      string folderName)
    {
      using (this.MetricsRecorder.RecordMortgageServiceTime(nameof (LoanFileArchive)))
      {
        using (IocContainer.CreateUnitOfWork(this._storgaeSettings, this._storageMode))
        {
          if (!string.IsNullOrEmpty(folderName))
            loan.AddLoanTag(new TagItem()
            {
              TagType = Enum.GetName(typeof (LoanTagType), (object) LoanTagType.LoanFolder),
              TagName = folderName
            });
          this._loanRepository.LoanArchiveSave(loan);
          this._loanRepository.LoanSnapshotDataSave(loan, DataStores.ArchiveStore);
          LoanHistory loanHistory1 = this._loanRepository.LoanHistoryGet(loan.EncompassId, DataStores.ArchiveStore);
          if (loanHistory1 == null)
            loanHistory1 = new LoanHistory()
            {
              EncompassLoanGuid = loan.EncompassId
            };
          LoanHistory loanHistory2 = loanHistory1;
          CompressedLoanFile compressedLoanFile = CompressedLoanFile.Create(loan.EncompassId, loan.ToJson<Loan>());
          compressedLoanFile.Consolidate(loanHistory2, userId, archivedUtc);
          this._loanRepository.CompressedLoanFileSave(compressedLoanFile, DataStores.ArchiveStore);
          LoanHistoryEntry entry = new LoanHistoryEntry()
          {
            Action = action,
            CompressedLoanFileId = compressedLoanFile.Id,
            Occurred = archivedUtc,
            UserId = userId
          };
          loanHistory2.Add(entry);
          this._loanRepository.LoanHistorySave(loanHistory2, DataStores.ArchiveStore);
        }
      }
    }

    private void LoanFileArchiveWithoutSnapshotData(
      Guid encompassGuid,
      string destinationFolderName,
      string userId)
    {
      Loan encompassId = this._loanRepository.LoanGetEncompassId(encompassGuid, DataStores.ArchiveStore);
      encompassId.MoveLoanToAnotherFolder(destinationFolderName);
      this._loanRepository.LoanArchiveSave(encompassId);
      LoanHistory loanHistory1 = this._loanRepository.LoanHistoryGet(encompassGuid, DataStores.ArchiveStore);
      if (loanHistory1 == null)
        loanHistory1 = new LoanHistory()
        {
          EncompassLoanGuid = encompassGuid
        };
      LoanHistory loanHistory2 = loanHistory1;
      CompressedLoanFile compressedLoanFile = CompressedLoanFile.Create(encompassId.EncompassId, encompassId.ToJson<Loan>());
      DateTime utcNow = DateTime.UtcNow;
      compressedLoanFile.Consolidate(loanHistory2, userId, utcNow);
      this._loanRepository.CompressedLoanFileSave(compressedLoanFile, DataStores.ArchiveStore);
      LoanHistoryEntry entry = new LoanHistoryEntry()
      {
        Action = LoanHistoryActionType.Move,
        CompressedLoanFileId = compressedLoanFile.Id,
        Occurred = utcNow,
        UserId = userId
      };
      loanHistory2.Add(entry);
      this._loanRepository.LoanHistorySave(loanHistory2, DataStores.ArchiveStore);
    }

    public void LoanFileArchive(
      string userId,
      string fileData,
      LoanHistoryActionType action,
      DateTime archivedUtc)
    {
      this.LoanFileArchive(userId, fileData, action, archivedUtc, (string) null);
    }

    public void LoanFileArchive(
      string userId,
      Loan loan,
      LoanHistoryActionType action,
      DateTime archivedUtc)
    {
      this.LoanFileArchive(userId, loan, action, archivedUtc, (string) null);
    }

    public object GetLoanContacts(Guid loanId) => this._loanRepository.LoanContactsGet(loanId);

    public List<string> GetSubmitLoanGuid(
      UserInfo loanId,
      int start,
      int limit,
      string sort,
      string filter)
    {
      return this._loanRepository.GetSubmitLoanGuid(loanId, start, limit, sort, filter);
    }

    public void Dispose()
    {
      GC.SuppressFinalize((object) this);
      this._unitOfWork.Dispose();
    }
  }
}
