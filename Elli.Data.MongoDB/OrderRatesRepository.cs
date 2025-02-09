// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.OrderRatesRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.APR;
using Elli.Domain.Mortgage;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class OrderRatesRepository : IOrderRatesRepository
  {
    static OrderRatesRepository() => MongoInitializer.Initialize();

    public bool SaveEPPSRates(string orderRates, string ratesRequest, string loanId)
    {
      try
      {
        IMongoDatabase mongoDatabase = OrderRatesRepository.Connect();
        BsonArray bsonArray = BsonSerializer.Deserialize<BsonArray>(orderRates);
        BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(ratesRequest);
        BsonDocument document = new BsonDocument();
        document.InsertAt(0, new BsonElement(Constants.EppsLoanId, (BsonValue) loanId));
        document.InsertAt(1, new BsonElement(Constants.EppsRatesRequest, (BsonValue) bsonDocument));
        document.InsertAt(2, new BsonElement(Constants.EppsLoanPrograms, (BsonValue) bsonArray));
        mongoDatabase.GetCollection<BsonDocument>("EPPSRates").InsertOne(document);
        return true;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public object GetEppsRatesRequest(string eppsLoanId)
    {
      if (eppsLoanId == null)
        throw new ApplicationException("Invalid input, you need to pass Loan Id.");
      BsonDocument bsonDocument = OrderRatesRepository.Connect().GetCollection<BsonDocument>(Collections.EppsRates).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.EppsLoanId, eppsLoanId)).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      bsonDocument.Remove("_id");
      return (object) bsonDocument.ToString();
    }

    private static IMongoDatabase Connect(DataStores dataStores = DataStores.ActiveStore)
    {
      MongoUnitOfWork mongoUnitOfWork = new MongoUnitOfWork();
      return dataStores != DataStores.ActiveStore ? mongoUnitOfWork.ArchiveConnection() : mongoUnitOfWork.ActiveConnection();
    }
  }
}
