// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.ArchiveRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Data.MongoDB.Mapping;
using Elli.Domain.Mortgage;
using Elli.Metrics;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class ArchiveRepository
  {
    private readonly IMetricsFactory _metricsFactory;
    private ILoanMetricsRecorder _metricsRecorder;

    public ArchiveRepository(IMetricsFactory metricsFactory)
    {
      this._metricsFactory = metricsFactory;
    }

    static ArchiveRepository()
    {
      MongoInitializer.Initialize();
      LoanMap.Register();
      LoanHistoryMap.Register();
    }

    private ILoanMetricsRecorder MetricsRecorder
    {
      get
      {
        if (this._metricsRecorder != null)
          return this._metricsRecorder;
        MongoUnitOfWork mongoUnitOfWork = new MongoUnitOfWork();
        return this._metricsRecorder = this._metricsFactory.CreateLoanMetricsRecorder(mongoUnitOfWork.GetClient(), mongoUnitOfWork.GetInstanceName());
      }
    }

    public LoanHistory LoanHistoryGet(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistoryGet)))
        return ArchiveRepository.Connect().GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).Find<LoanHistory>(Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid)).FirstOrDefault<LoanHistory, LoanHistory>();
    }

    public void LoanHistorySave(LoanHistory loanHistory)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistorySave)))
        ArchiveRepository.Connect().GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).InsertOne(loanHistory);
    }

    public void LoanHistoryRemove(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistoryRemove)))
        ArchiveRepository.Connect().GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).DeleteOne(Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid));
    }

    public Loan LoanArchiveGet(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanArchiveGet)))
        return ArchiveRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).Find<Loan>(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), encompassLoanGuid)).FirstOrDefault<Loan, Loan>();
    }

    public void LoanArchiveSave(Loan loan)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanArchiveSave)))
      {
        IMongoCollection<Loan> collection = ArchiveRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<Loan> filter = Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), loan.EncompassId);
        Loan loan1 = collection.Find<Loan>(filter).Limit(new int?(1)).Single<Loan, Loan>();
        loan.Id = loan1.Id;
        collection.InsertOne(loan);
      }
    }

    public void LoanArchiveRemove(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanArchiveRemove)))
        ArchiveRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), encompassLoanGuid));
    }

    public CompressedLoanFile CompressedLoanFileGet(string id)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileGet)))
        return ArchiveRepository.Connect().GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).Find<CompressedLoanFile>(Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id))).FirstOrDefault<CompressedLoanFile, CompressedLoanFile>();
    }

    public void CompressedLoanFileSave(CompressedLoanFile compressedLoanFile)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileSave)))
        ArchiveRepository.Connect().GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).InsertOne(compressedLoanFile);
    }

    public void CompressedLoanFileRemove(string id)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileRemove)))
        ArchiveRepository.Connect().GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).DeleteOne(Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id)));
    }

    public void LoanSnapshotDataSave(Loan loan)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanSnapshotDataSave)))
      {
        IMongoCollection<SnapshotDocument> collection = ArchiveRepository.Connect().GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
        List<Tuple<Guid, Guid, Type>> existingSnapshots = this.GetExistingSnapshotsGuid(loan.EncompassId);
        IList<SnapshotDocument> list = (IList<SnapshotDocument>) loan.LockRequestLogs.Where<LockRequestLog>((Func<LockRequestLog, bool>) (x => existingSnapshots.Where<Tuple<Guid, Guid, Type>>((Func<Tuple<Guid, Guid, Type>, bool>) (y => y.Item2 != x.Guid)).ToList<Tuple<Guid, Guid, Type>>().Count <= 0)).Select<LockRequestLog, LockRequestSnapshot>((Func<LockRequestLog, LockRequestSnapshot>) (log =>
        {
          return new LockRequestSnapshot()
          {
            EncompassLoanGuid = loan.EncompassId,
            SnapshotId = log.Guid,
            OptimalBlue = log.SnapshotFields.FirstOrDefault<LogSnapshotField>((Func<LogSnapshotField, bool>) (x => x.FieldID == "OPTIMAL.HISTORY")) == null ? (string) null : log.SnapshotFields.FirstOrDefault<LogSnapshotField>((Func<LogSnapshotField, bool>) (x => x.FieldID == "OPTIMAL.HISTORY")).Value
          };
        })).Cast<SnapshotDocument>().ToList<SnapshotDocument>();
        if (existingSnapshots.Where<Tuple<Guid, Guid, Type>>((Func<Tuple<Guid, Guid, Type>, bool>) (x => x.Item3 != typeof (OptimalBlueSnapshot))).ToList<Tuple<Guid, Guid, Type>>().Count <= 0)
        {
          IList<SnapshotDocument> snapshotDocumentList = list;
          OptimalBlueSnapshot optimalBlueSnapshot = new OptimalBlueSnapshot();
          optimalBlueSnapshot.EncompassLoanGuid = loan.EncompassId;
          optimalBlueSnapshot.HistoryData = loan.Miscellaneous.OptimalBlueHistoryData;
          optimalBlueSnapshot.Request = loan.Miscellaneous.OptimalBlueRequest;
          optimalBlueSnapshot.Response = loan.Miscellaneous.OptimalBlueResponse;
          snapshotDocumentList.Add((SnapshotDocument) optimalBlueSnapshot);
        }
        foreach (DisclosureTrackingLog disclosureTrackingLog in loan.DisclosureTrackingLogs)
        {
          DisclosureTrackingLog log = disclosureTrackingLog;
          if (existingSnapshots.Where<Tuple<Guid, Guid, Type>>((Func<Tuple<Guid, Guid, Type>, bool>) (x => x.Item2 != log.Guid)).ToList<Tuple<Guid, Guid, Type>>().Count <= 0)
          {
            IList<SnapshotDocument> snapshotDocumentList = list;
            DisclosureTrackingSnapshot trackingSnapshot = new DisclosureTrackingSnapshot();
            trackingSnapshot.EncompassLoanGuid = loan.EncompassId;
            trackingSnapshot.SnapshotId = log.Guid;
            trackingSnapshot.SnapshotFields = log.SnapshotFields;
            snapshotDocumentList.Add((SnapshotDocument) trackingSnapshot);
          }
        }
        foreach (DocumentOrderLog documentOrderLog in loan.DocumentOrderLogs)
        {
          DocumentOrderLog log = documentOrderLog;
          if (existingSnapshots.Where<Tuple<Guid, Guid, Type>>((Func<Tuple<Guid, Guid, Type>, bool>) (x => x.Item2 != log.Guid)).ToList<Tuple<Guid, Guid, Type>>().Count <= 0)
          {
            IList<SnapshotDocument> snapshotDocumentList = list;
            DocumentOrderSnapshot documentOrderSnapshot = new DocumentOrderSnapshot();
            documentOrderSnapshot.EncompassLoanGuid = loan.EncompassId;
            documentOrderSnapshot.SnapshotId = log.Guid;
            documentOrderSnapshot.FieldsXml = log.DocumentFieldsXml;
            snapshotDocumentList.Add((SnapshotDocument) documentOrderSnapshot);
          }
        }
        if (list.Count <= 0)
          return;
        collection.InsertMany((IEnumerable<SnapshotDocument>) list);
      }
    }

    public void LoanSnapshotDataGet(Loan loan)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanSnapshotDataGet)))
      {
        IMongoCollection<SnapshotDocument> collection = ArchiveRepository.Connect().GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
        FilterDefinition<SnapshotDocument> filter = Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), loan.EncompassId);
        foreach (SnapshotDocument snapshotDocument in collection.Find<SnapshotDocument>(filter).ToListAsync<SnapshotDocument>().Result)
        {
          SnapshotDocument snapshot = snapshotDocument;
          if (snapshot is LockRequestSnapshot)
          {
            LockRequestSnapshot lockRequestSnapshot = snapshot as LockRequestSnapshot;
            loan.LockRequestLogs.Single<LockRequestLog>((Func<LockRequestLog, bool>) (p => p.Guid == snapshot.SnapshotId)).SnapshotXml = lockRequestSnapshot.OptimalBlue;
          }
          else if (snapshot is OptimalBlueSnapshot)
          {
            OptimalBlueSnapshot optimalBlueSnapshot = snapshot as OptimalBlueSnapshot;
            loan.Miscellaneous.OptimalBlueHistoryData = optimalBlueSnapshot.HistoryData;
            loan.Miscellaneous.OptimalBlueRequest = optimalBlueSnapshot.Request;
            loan.Miscellaneous.OptimalBlueResponse = optimalBlueSnapshot.Response;
          }
          else if (snapshot is DisclosureTrackingSnapshot)
          {
            DisclosureTrackingSnapshot trackingSnapshot = snapshot as DisclosureTrackingSnapshot;
            loan.DisclosureTrackingLogs.Single<DisclosureTrackingLog>((Func<DisclosureTrackingLog, bool>) (p => p.Guid == snapshot.SnapshotId)).SetLogSnapshotFields((IList<LogSnapshotField>) trackingSnapshot.SnapshotFields.ToList<LogSnapshotField>());
          }
          else if (snapshot is DocumentOrderSnapshot)
          {
            DocumentOrderSnapshot documentOrderSnapshot = snapshot as DocumentOrderSnapshot;
            loan.DocumentOrderLogs.Single<DocumentOrderLog>((Func<DocumentOrderLog, bool>) (p => p.Guid == snapshot.SnapshotId)).DocumentFieldsXml = documentOrderSnapshot.FieldsXml;
          }
        }
      }
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ArchiveConnection();

    private List<Tuple<Guid, Guid, Type>> GetExistingSnapshotsGuid(Guid encompassLoanGuid)
    {
      List<SnapshotDocument> result = ArchiveRepository.Connect().GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots).Find<SnapshotDocument>(Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid)).ToListAsync<SnapshotDocument>().Result;
      List<Tuple<Guid, Guid, Type>> existingSnapshotsGuid = new List<Tuple<Guid, Guid, Type>>();
      foreach (SnapshotDocument snapshotDocument in result)
        existingSnapshotsGuid.Add(new Tuple<Guid, Guid, Type>(snapshotDocument.Id, snapshotDocument.SnapshotId, snapshotDocument.GetType()));
      return existingSnapshotsGuid;
    }

    public void LoanDeleteEncompassId(Guid encompassId)
    {
      ArchiveRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), encompassId));
    }

    public bool SupportsArchive => true;
  }
}
