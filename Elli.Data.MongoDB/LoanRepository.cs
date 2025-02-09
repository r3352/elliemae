// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.LoanRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Data.MongoDB.Mapping;
using Elli.Data.MongoDB.MigrationScript;
using Elli.Domain.Mortgage;
using Elli.Metrics;
using EllieMae.EMLite.RemotingServices;
using Microsoft.IdentityModel.Claims;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class LoanRepository : ILoanRepository
  {
    private readonly IMetricsFactory _metricsFactory;
    private ILoanMetricsRecorder _metricsRecorder;

    public LoanRepository(IMetricsFactory metricsFactory) => this._metricsFactory = metricsFactory;

    static LoanRepository()
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

    private void TrackLoanDocumentLength(Loan loan)
    {
      if (loan == null || !this._metricsFactory.MetricsCollectionEnabled)
        return;
      this.MetricsRecorder.RecordLoanDocumentLength(loan.ToJson<Loan>().Length);
    }

    public void LoanSave(Loan loan)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanSave)))
      {
        this.TrackLoanDocumentLength(loan);
        IMongoCollection<Loan> collection = LoanRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans);
        if (loan.Id == Guid.Empty || loan.CurrentDataStore == DataStores.ArchiveStore)
        {
          ++loan.LoanVersionId;
          collection.InsertOne(loan);
        }
        else
        {
          int loanVersionId = loan.LoanVersionId;
          FilterDefinitionBuilder<Loan> filter1 = Builders<Loan>.Filter;
          FilterDefinition<Loan> filter2 = filter1.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), loan.EncompassId) & filter1.Eq<int>((Expression<Func<Loan, int>>) (x => x.LoanVersionId), loanVersionId);
          ++loan.LoanVersionId;
          if (collection.ReplaceOne(filter2, loan).ModifiedCount == 0L)
            throw new ApplicationException("Your data is out of sync, Please refresh your data before proceed.");
        }
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanArchiveSave(Loan loan)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanArchiveSave)))
      {
        IMongoCollection<Loan> collection = LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<Loan> filter = Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), loan.EncompassId);
        Loan loan1 = collection.Find<Loan>(filter).Limit(new int?(1)).Single<Loan, Loan>();
        loan.Id = loan1.Id;
        ++loan.LoanVersionId;
        collection.InsertOne(loan);
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanHistorySave(LoanHistory loanHistory, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistorySave)))
      {
        IMongoCollection<LoanHistory> collection = LoanRepository.Connect(dataStores).GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory);
        FilterDefinition<LoanHistory> filter = Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), loanHistory.EncompassLoanGuid);
        if (collection.Find<LoanHistory>(filter).ToListAsync<LoanHistory>().Result.FirstOrDefault<LoanHistory>() == null)
          collection.InsertOne(loanHistory);
        else
          collection.ReplaceOne(filter, loanHistory);
      }
    }

    public void CompressedLoanFileSave(CompressedLoanFile compressedLoanFile, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileSave)))
      {
        IMongoCollection<CompressedLoanFile> collection = LoanRepository.Connect(dataStores).GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles);
        FilterDefinition<CompressedLoanFile> filter = Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), compressedLoanFile.EncompassLoanGuid);
        if (collection.Find<CompressedLoanFile>(filter).FirstOrDefault<CompressedLoanFile, CompressedLoanFile>() == null)
          collection.InsertOne(compressedLoanFile);
        else
          collection.ReplaceOne(filter, compressedLoanFile);
      }
    }

    public void LoanSnapshotDataSave(Loan loan, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanSnapshotDataSave)))
      {
        IMongoCollection<SnapshotDocument> collection = LoanRepository.Connect(dataStores).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
        List<Tuple<Guid, Guid, Type>> existingSnapshots = LoanRepository.GetExistingSnapshotsGuid(loan.EncompassId, dataStores);
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
        if (list.Count > 0)
          collection.InsertMany((IEnumerable<SnapshotDocument>) list);
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public Loan LoanGet(Guid id, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanGet)))
      {
        Loan loan = LoanRepository.Connect(dataStores).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).Find<Loan>(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.Id), id)).FirstOrDefault<Loan, Loan>();
        loan.CurrentDataStore = dataStores;
        this.MetricsRecorder.IncrementLoanOperationCount();
        this.TrackLoanDocumentLength(loan);
        return loan;
      }
    }

    public Loan LoanGetComplete(Guid loanId)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanGetComplete)))
      {
        DataStores dataStores = DataStores.ActiveStore;
        IMongoCollection<BsonDocument> collection = LoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq<Guid>((FieldDefinition<BsonDocument, Guid>) "_id", loanId);
        FilterDefinition<BsonDocument> filter2 = filter1;
        BsonDocument sourceDocument = collection.Find<BsonDocument>(filter2).FirstOrDefault<BsonDocument, BsonDocument>();
        if (sourceDocument == (BsonDocument) null)
        {
          sourceDocument = LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.Loans).Find<BsonDocument>(filter1).FirstOrDefault<BsonDocument, BsonDocument>();
          dataStores = DataStores.ArchiveStore;
        }
        BsonDocument document = new ScriptProcessor().Migrate(sourceDocument);
        Loan loan = document != (BsonDocument) null ? BsonSerializer.Deserialize<Loan>(document) : (Loan) null;
        if (loan != null)
          loan.CurrentDataStore = dataStores;
        this.TrackLoanDocumentLength(loan);
        this.MetricsRecorder.IncrementLoanOperationCount();
        return loan;
      }
    }

    public Loan LoanGetCompleteEncompassId(Guid encompassId)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanGetCompleteEncompassId)))
      {
        DataStores dataStores = DataStores.ActiveStore;
        IMongoCollection<BsonDocument> collection = LoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq<Guid>((FieldDefinition<BsonDocument, Guid>) "EncompassId", encompassId);
        FilterDefinition<BsonDocument> filter2 = filter1;
        BsonDocument sourceDocument = collection.Find<BsonDocument>(filter2).FirstOrDefault<BsonDocument, BsonDocument>();
        if (sourceDocument == (BsonDocument) null)
        {
          sourceDocument = LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.Loans).Find<BsonDocument>(filter1).FirstOrDefault<BsonDocument, BsonDocument>();
          dataStores = DataStores.ArchiveStore;
        }
        BsonDocument document = new ScriptProcessor().Migrate(sourceDocument);
        Loan loan = document != (BsonDocument) null ? BsonSerializer.Deserialize<Loan>(document) : (Loan) null;
        if (loan != null)
          loan.CurrentDataStore = dataStores;
        this.TrackLoanDocumentLength(loan);
        this.MetricsRecorder.IncrementLoanOperationCount();
        return loan;
      }
    }

    public Loan LoanGetEncompassId(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanGetEncompassId)))
      {
        BsonDocument document = new ScriptProcessor().Migrate(LoanRepository.Connect(dataStores).GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.Loans).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<Guid>((FieldDefinition<BsonDocument, Guid>) "EncompassId", encompassId)).FirstOrDefault<BsonDocument, BsonDocument>());
        Loan loan = document != (BsonDocument) null ? BsonSerializer.Deserialize<Loan>(document) : (Loan) null;
        if (loan != null)
          loan.CurrentDataStore = dataStores;
        this.TrackLoanDocumentLength(loan);
        this.MetricsRecorder.IncrementLoanOperationCount();
        return loan;
      }
    }

    public LoanHistory LoanHistoryGet(Guid encompassLoanGuid, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistoryGet)))
        return LoanRepository.Connect(dataStores).GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).Find<LoanHistory>(Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid)).FirstOrDefault<LoanHistory, LoanHistory>();
    }

    public LoanHistory LoanHistoryGetComplete(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistoryGetComplete)))
      {
        IMongoCollection<LoanHistory> collection = LoanRepository.Connect().GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory);
        FilterDefinition<LoanHistory> filter = Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid);
        return collection.Find<LoanHistory>(filter).FirstOrDefault<LoanHistory, LoanHistory>() ?? LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).Find<LoanHistory>(filter).FirstOrDefault<LoanHistory, LoanHistory>();
      }
    }

    public CompressedLoanFile CompressedLoanFileGet(string id, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileGet)))
        return LoanRepository.Connect(dataStores).GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).Find<CompressedLoanFile>(Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id))).FirstOrDefault<CompressedLoanFile, CompressedLoanFile>();
    }

    public CompressedLoanFile CompressedLoanFileGetComplete(string id)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime("CompressedLoanFileGet"))
      {
        IMongoCollection<CompressedLoanFile> collection = LoanRepository.Connect().GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles);
        FilterDefinition<CompressedLoanFile> filter = Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id));
        return collection.Find<CompressedLoanFile>(filter).FirstOrDefault<CompressedLoanFile, CompressedLoanFile>() ?? LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).Find<CompressedLoanFile>(filter).FirstOrDefault<CompressedLoanFile, CompressedLoanFile>();
      }
    }

    public void LoanSnapshotDataGet(Loan loan, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanSnapshotDataGet)))
      {
        IMongoCollection<SnapshotDocument> collection = LoanRepository.Connect(dataStores).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
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
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanSnapshotDataGetComplete(Loan loan)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanSnapshotDataGetComplete)))
      {
        IMongoCollection<SnapshotDocument> collection = LoanRepository.Connect().GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
        FilterDefinition<SnapshotDocument> filter = Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), loan.EncompassId);
        IFindFluent<SnapshotDocument, SnapshotDocument> source = collection.Find<SnapshotDocument>(filter);
        if (source == null || !source.Any<SnapshotDocument>())
          source = LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots).Find<SnapshotDocument>(filter);
        foreach (SnapshotDocument snapshotDocument in source.ToList<SnapshotDocument>())
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
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public SnapshotDocument LoanSnapshotDataGet(Guid snapshotGuid, DataStores dataStores = DataStores.ActiveStore)
    {
      throw new NotImplementedException();
    }

    public SnapshotDocument LoanSnapshotDataGetComplete(Guid snapshotGuid)
    {
      throw new NotImplementedException();
    }

    public object LoanContactsGet(Guid loanId)
    {
      List<BsonDocument> list = LoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanContacts).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId.ToString())).ToList<BsonDocument>();
      if (list.Count == 0)
        throw new ApplicationException("No document found with LoanId '" + loanId.ToString() + "'");
      List<string> stringList = new List<string>();
      foreach (BsonDocument bsonDocument in list)
      {
        bsonDocument.Add("Id", (BsonValue) bsonDocument["_id"].ToString());
        bsonDocument.Remove("_id");
        stringList.Add(bsonDocument.ToString());
      }
      return (object) stringList;
    }

    public void LoanDelete(Guid loanId, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanDelete)))
      {
        LoanRepository.Connect(dataStores).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.Id), loanId));
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanDeleteComplete(Guid loanId)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime("LoanDelete"))
      {
        IMongoCollection<Loan> collection = LoanRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<Loan> filter = Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.Id), loanId);
        if (collection.DeleteOne(filter).DeletedCount == 0L)
          LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(filter);
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanDeleteEncompassId(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanDeleteEncompassId)))
      {
        LoanRepository.Connect(dataStores).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), encompassId));
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanDeleteEncompassIdComplete(Guid encompassId)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime("LoanDeleteComplete"))
      {
        IMongoCollection<Loan> collection = LoanRepository.Connect().GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans);
        FilterDefinition<Loan> filter = Builders<Loan>.Filter.Eq<Guid>((Expression<Func<Loan, Guid>>) (x => x.EncompassId), encompassId);
        if (collection.DeleteOne(filter).DeletedCount == 0L)
          LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<Loan>(Elli.Data.MongoDB.Collections.Loans).DeleteOne(filter);
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public void LoanHistoryRemove(Guid encompassLoanGuid, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (LoanHistoryRemove)))
        LoanRepository.Connect(dataStores).GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).DeleteOne(Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid));
    }

    public void LoanHistoryRemoveComplete(Guid encompassLoanGuid)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime("LoanHistoryRemove"))
      {
        IMongoCollection<LoanHistory> collection = LoanRepository.Connect().GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory);
        FilterDefinition<LoanHistory> filter = Builders<LoanHistory>.Filter.Eq<Guid>((Expression<Func<LoanHistory, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid);
        if (collection.DeleteOne(filter).DeletedCount != 0L)
          return;
        LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<LoanHistory>(Elli.Data.MongoDB.Collections.LoanHistory).DeleteOne(filter);
      }
    }

    public void CompressedLoanFileRemove(string id, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileRemove)))
        LoanRepository.Connect(dataStores).GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).DeleteOne(Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id)));
    }

    public void CompressedLoanFileRemoveComplete(string id)
    {
      using (this.MetricsRecorder.RecordArchiveRepositoryTime(nameof (CompressedLoanFileRemoveComplete)))
      {
        IMongoCollection<CompressedLoanFile> collection = LoanRepository.Connect().GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles);
        FilterDefinition<CompressedLoanFile> filter = Builders<CompressedLoanFile>.Filter.Eq<Guid>((Expression<Func<CompressedLoanFile, Guid>>) (x => x.EncompassLoanGuid), new Guid(id));
        if (collection.DeleteOne(filter).DeletedCount != 0L)
          return;
        LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<CompressedLoanFile>(Elli.Data.MongoDB.Collections.CompressedLoanFiles).DeleteOne(filter);
      }
    }

    public void LoanSnapshotDataDelete(Guid encompassId, DataStores dataStores = DataStores.ActiveStore)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanSnapshotDataDelete)))
        LoanRepository.Connect(dataStores).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots).DeleteMany(Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), encompassId));
    }

    public void LoanSnapshotDataDeleteComplete(Guid encompassId)
    {
      using (this.MetricsRecorder.RecordLoanRepositoryTime(nameof (LoanSnapshotDataDeleteComplete)))
      {
        IMongoCollection<SnapshotDocument> collection = LoanRepository.Connect().GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots);
        FilterDefinition<SnapshotDocument> filter = Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), encompassId);
        if (collection.DeleteMany(filter).DeletedCount == 0L)
          LoanRepository.Connect(DataStores.ArchiveStore).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots).DeleteMany(filter);
        this.MetricsRecorder.IncrementLoanOperationCount();
      }
    }

    public List<string> GetSubmitLoanGuid(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filter)
    {
      if (currentUser == (UserInfo) null)
        throw new ApplicationException("User Info not available in the context.");
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      string empty5 = string.Empty;
      foreach (Claim claim in currentUser.BorrowerContext.Claims)
      {
        if (claim.ClaimType == "elli_sid")
        {
          string str1 = claim.Value;
        }
        else if (claim.ClaimType == "elli_uid")
        {
          string[] strArray = claim.Value.Split("\\".ToCharArray());
          strArray[strArray.Length - 1].ToString();
        }
        else if (claim.ClaimType == "elli_inst")
        {
          string str2 = claim.Value;
        }
        else if (claim.ClaimType == "sub")
          empty4 = claim.Value;
        else if (claim.ClaimType == "elli_idt")
          empty5 = claim.Value;
      }
      if (empty5 != "Consumer")
        throw new ApplicationException("Only a borrower can view the submitted loans pipeline.");
      IMongoCollection<BsonDocument> collection = LoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanContacts);
      BsonDocument bsonDocument1 = new BsonDocument()
      {
        {
          "$match",
          (BsonValue) new BsonDocument()
          {
            {
              Constants.UUID,
              (BsonValue) empty4
            }
          }
        }
      };
      BsonDocument bsonDocument2 = new BsonDocument()
      {
        {
          "$group",
          (BsonValue) new BsonDocument()
          {
            {
              "_id",
              (BsonValue) new BsonDocument()
              {
                {
                  Constants.LoanId,
                  (BsonValue) ("$" + Constants.LoanId)
                }
              }
            },
            {
              "sorter",
              (BsonValue) new BsonDocument()
              {
                {
                  "$first",
                  (BsonValue) "$_id"
                }
              }
            }
          }
        }
      };
      BsonDocument bsonDocument3 = new BsonDocument()
      {
        {
          "$sort",
          (BsonValue) new BsonDocument()
          {
            {
              "sorter",
              (BsonValue) -1
            }
          }
        }
      };
      BsonDocument bsonDocument4 = new BsonDocument()
      {
        {
          "$skip",
          (BsonValue) (start - 1)
        }
      };
      BsonDocument bsonDocument5 = new BsonDocument()
      {
        {
          "$limit",
          (BsonValue) limit
        }
      };
      List<BsonDocument> bsonDocumentList;
      if (string.IsNullOrWhiteSpace(filter) && string.IsNullOrWhiteSpace(sort))
        bsonDocumentList = new List<BsonDocument>()
        {
          bsonDocument1,
          bsonDocument2,
          bsonDocument3,
          bsonDocument4,
          bsonDocument5
        };
      else
        bsonDocumentList = new List<BsonDocument>()
        {
          bsonDocument1,
          bsonDocument2
        };
      PipelineDefinition<BsonDocument, BsonDocument> pipeline = (PipelineDefinition<BsonDocument, BsonDocument>) bsonDocumentList;
      CancellationToken cancellationToken = new CancellationToken();
      List<BsonDocument> list = collection.Aggregate<BsonDocument>(pipeline, cancellationToken: cancellationToken).ToList<BsonDocument>();
      List<string> submitLoanGuid = new List<string>();
      foreach (BsonDocument bsonDocument6 in list)
        submitLoanGuid.Add(bsonDocument6[0][0].ToString());
      return submitLoanGuid;
    }

    private static FilterDefinition<BsonDocument> getFilters(
      string filters,
      FilterDefinition<BsonDocument> FilterDef)
    {
      FilterDefinitionBuilder<BsonDocument> filter = Builders<BsonDocument>.Filter;
      if (string.IsNullOrEmpty(filters))
        return FilterDef;
      foreach (string str in filters.Split(",".ToCharArray()))
      {
        string[] strArray = str.Split("::".ToCharArray());
        if (strArray[0].ToLower() == "siteid")
          FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, strArray[2].ToString());
      }
      return FilterDef;
    }

    private static List<Tuple<Guid, Guid, Type>> GetExistingSnapshotsGuid(
      Guid encompassLoanGuid,
      DataStores dataStores = DataStores.ActiveStore)
    {
      return LoanRepository.Connect(dataStores).GetCollection<SnapshotDocument>(Elli.Data.MongoDB.Collections.Snapshots).Find<SnapshotDocument>(Builders<SnapshotDocument>.Filter.Eq<Guid>((Expression<Func<SnapshotDocument, Guid>>) (x => x.EncompassLoanGuid), encompassLoanGuid)).ToListAsync<SnapshotDocument>().Result.Select<SnapshotDocument, Tuple<Guid, Guid, Type>>((Func<SnapshotDocument, Tuple<Guid, Guid, Type>>) (snapshot => new Tuple<Guid, Guid, Type>(snapshot.Id, snapshot.SnapshotId, snapshot.GetType()))).ToList<Tuple<Guid, Guid, Type>>();
    }

    public bool SupportsArchive => true;

    private static IMongoDatabase Connect(DataStores dataStores = DataStores.ActiveStore)
    {
      MongoUnitOfWork mongoUnitOfWork = new MongoUnitOfWork();
      return dataStores != DataStores.ActiveStore ? mongoUnitOfWork.ArchiveConnection() : mongoUnitOfWork.ActiveConnection();
    }
  }
}
