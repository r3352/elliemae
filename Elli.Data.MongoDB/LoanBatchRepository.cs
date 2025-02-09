// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.LoanBatchRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Exceptions;
using Elli.Domain.LoanBatch;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class LoanBatchRepository : ILoanBatchRepository
  {
    static LoanBatchRepository() => MongoInitializer.Initialize();

    public string CreateLoanBatchRequest(ILoanBatchRequest request)
    {
      try
      {
        IMongoCollection<BsonDocument> collection = LoanBatchRepository.Connect().GetCollection<BsonDocument>(Collections.LoanBatchRequests);
        BsonDocument bsonDocument = BsonDocument.Parse(JsonConvert.SerializeObject((object) request));
        BsonDocument document = bsonDocument;
        CancellationToken cancellationToken = new CancellationToken();
        collection.InsertOne(document, cancellationToken: cancellationToken);
        return bsonDocument["_id"].ToString();
      }
      catch (Exception ex)
      {
        throw new InternalServerErrorException(ex.Message);
      }
    }

    public ILoanBatchRequest GetLoanBatchUpdateRequest(string requestId)
    {
      try
      {
        BsonDocument bsonDocument = LoanBatchRepository.Connect().GetCollection<BsonDocument>(Collections.LoanBatchRequests).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(requestId))).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if (bsonDocument == (BsonDocument) null)
          throw new NotFoundException("The specified loan batch update request with requestId " + requestId + " cannot be found.");
        if (bsonDocument["Id"] == (BsonValue) null)
          bsonDocument.InsertAt(0, new BsonElement("Id", (BsonValue) bsonDocument["_id"].ToString()));
        else
          bsonDocument["Id"] = (BsonValue) bsonDocument["_id"].ToString();
        bsonDocument.Remove("_id");
        return (ILoanBatchRequest) JsonConvert.DeserializeObject<LoanBatchUpdateRequest>(bsonDocument.ToString());
      }
      catch (NotFoundException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new InternalServerErrorException(ex.Message);
      }
    }

    public void UpdateLoanBatchRequest(string requestId, ILoanBatchRequest request)
    {
      try
      {
        IMongoCollection<BsonDocument> collection = LoanBatchRepository.Connect().GetCollection<BsonDocument>(Collections.LoanBatchRequests);
        FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq<ObjectId>((FieldDefinition<BsonDocument, ObjectId>) "_id", ObjectId.Parse(requestId));
        BsonDocument bsonDocument = BsonDocument.Parse(JsonConvert.SerializeObject((object) request));
        if (bsonDocument == (BsonDocument) null)
          throw new NotFoundException("The specified loan batch request with requestId " + requestId + " cannot be found.");
        FilterDefinition<BsonDocument> filter = filterDefinition;
        BsonDocument replacement = bsonDocument;
        CancellationToken cancellationToken = new CancellationToken();
        collection.ReplaceOne(filter, replacement, cancellationToken: cancellationToken);
      }
      catch (Exception ex)
      {
        throw new InternalServerErrorException(ex.Message);
      }
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();
  }
}
