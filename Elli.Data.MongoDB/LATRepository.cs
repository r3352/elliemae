// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.LATRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Common.Data;
using Elli.Domain.LAT;
using EllieMae.EMLite.Common.Settings;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class LATRepository : ILATRepository
  {
    private StorageSettings storageSettings;

    public LATRepository()
    {
    }

    static LATRepository() => MongoInitializer.Initialize();

    public LATRepository(StorageSettings storageSettings) => this.storageSettings = storageSettings;

    public bool IsPackageGroupCreated(string loanId)
    {
      return this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId) & Builders<BsonDocument>.Filter.Ne<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, (string) null) & Builders<BsonDocument>.Filter.Ne<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, string.Empty)).FirstOrDefault<BsonDocument, BsonDocument>() != (BsonDocument) null;
    }

    public string CreateLAT(LoanContactDocument loanContactDoc)
    {
      IMongoCollection<BsonDocument> collection = this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts);
      FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RecipientId, loanContactDoc.RecipientId);
      BsonDocument replacement = collection.Find<BsonDocument>(filter1).FirstOrDefault<BsonDocument, BsonDocument>();
      if (replacement == (BsonDocument) null && loanContactDoc.ContactType == "Borrower")
      {
        FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanContactDoc.LoanId) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.BorrowerId, loanContactDoc.BorrowerId);
        replacement = collection.Find<BsonDocument>(filter2).FirstOrDefault<BsonDocument, BsonDocument>();
        if (replacement != (BsonDocument) null)
        {
          replacement[Constants.LAT] = (BsonValue) Guid.NewGuid().ToString();
          replacement[Constants.RecipientId] = (BsonValue) loanContactDoc.RecipientId;
        }
      }
      if (replacement != (BsonDocument) null)
      {
        replacement[Constants.AuthCode] = (BsonValue) loanContactDoc.AuthCode;
        replacement[Constants.AuthType] = (BsonValue) loanContactDoc.AuthType;
        replacement[Constants.Name] = (BsonValue) loanContactDoc.Name;
        replacement[Constants.Email] = (BsonValue) loanContactDoc.Email;
        replacement[Constants.Attempts] = (BsonValue) 0;
        replacement[Constants.Enabled] = (BsonValue) true;
        collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
        return replacement[Constants.LAT].ToString();
      }
      loanContactDoc.LAT = Guid.NewGuid().ToString();
      BsonDocument document = BsonDocument.Parse(JsonConvert.SerializeObject((object) loanContactDoc));
      collection.InsertOne(document);
      return loanContactDoc.LAT;
    }

    private void CreateAuthLog(LatAuthenticationLog authLog, LATAuthenticationStatus status)
    {
      IMongoCollection<BsonDocument> collection = this.Connect().GetCollection<BsonDocument>(Collections.LatAuthenticationLogs);
      authLog.Result = status.ToString();
      BsonDocument document = BsonDocument.Parse(JsonConvert.SerializeObject((object) authLog));
      CancellationToken cancellationToken = new CancellationToken();
      collection.InsertOne(document, cancellationToken: cancellationToken);
    }

    public LATAuthenticationStatus AuthenticateWithLAT(
      string lat,
      string authCode,
      string uuid,
      ref string loanId,
      ref string recipientId,
      string ipAddress)
    {
      LatAuthenticationLog authLog = new LatAuthenticationLog()
      {
        UUID = uuid,
        LAT = lat,
        IpAddress = ipAddress
      };
      IMongoCollection<BsonDocument> collection = this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts);
      FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, lat);
      BsonDocument replacement = collection.Find<BsonDocument>(filter1).FirstOrDefault<BsonDocument, BsonDocument>();
      if (replacement == (BsonDocument) null)
      {
        this.CreateAuthLog(authLog, LATAuthenticationStatus.NotFound);
        return LATAuthenticationStatus.NotFound;
      }
      authLog.LoanId = replacement[Constants.LoanId].ToString();
      if (!(bool) replacement[Constants.Enabled])
      {
        this.CreateAuthLog(authLog, LATAuthenticationStatus.Locked);
        return LATAuthenticationStatus.Locked;
      }
      if (!string.IsNullOrEmpty(replacement[Constants.UUID].ToString()) && replacement[Constants.UUID] != (BsonValue) uuid)
      {
        this.CreateAuthLog(authLog, LATAuthenticationStatus.Conflict);
        return LATAuthenticationStatus.Conflict;
      }
      if (authCode == replacement[Constants.AuthCode].ToString())
      {
        FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, replacement[Constants.LoanId].ToString()) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.UUID, uuid);
        BsonDocument bsonDocument = collection.Find<BsonDocument>(filter2).FirstOrDefault<BsonDocument, BsonDocument>();
        if (bsonDocument != (BsonDocument) null && bsonDocument != replacement)
        {
          this.CreateAuthLog(authLog, LATAuthenticationStatus.HasBeenAssociated);
          return LATAuthenticationStatus.HasBeenAssociated;
        }
        this.CreateAuthLog(authLog, LATAuthenticationStatus.Successful);
        loanId = replacement[Constants.LoanId].ToString();
        recipientId = replacement[Constants.RecipientId].ToString();
        replacement[Constants.UUID] = (BsonValue) uuid;
        replacement[Constants.IsAuthenticated] = (BsonValue) true;
        replacement[Constants.Attempts] = (BsonValue) 0;
        collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
        return LATAuthenticationStatus.Successful;
      }
      this.CreateAuthLog(authLog, LATAuthenticationStatus.Mismatched);
      replacement[Constants.Attempts] = (BsonValue) ((int) replacement[Constants.Attempts] + 1);
      if ((int) replacement[Constants.Attempts] == 5)
        replacement[Constants.Enabled] = (BsonValue) false;
      collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
      return LATAuthenticationStatus.Mismatched;
    }

    public LATAuthenticationStatus AuthenticateWithLATNoAuthCode(
      string lat,
      string uuid,
      ref string loanId)
    {
      BsonDocument bsonDocument = this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, lat)).FirstOrDefault<BsonDocument, BsonDocument>();
      if (bsonDocument == (BsonDocument) null)
        return LATAuthenticationStatus.NotFound;
      if (!(bool) bsonDocument[Constants.Enabled])
        return LATAuthenticationStatus.Locked;
      if ((string.IsNullOrEmpty(bsonDocument[Constants.UUID].ToString()) ? 0 : (bsonDocument[Constants.UUID] != (BsonValue) uuid ? 1 : 0)) != 0)
        return LATAuthenticationStatus.Conflict;
      if (!(bool) bsonDocument[Constants.IsAuthenticated])
        return LATAuthenticationStatus.NeedAuthentication;
      BsonValue bsonValue = (BsonValue) null;
      if (bsonDocument.TryGetValue(Constants.LoanId, out bsonValue))
      {
        loanId = bsonValue.ToString();
        return LATAuthenticationStatus.Successful;
      }
      loanId = "-1";
      return LATAuthenticationStatus.NotFound;
    }

    public LATAuthenticationStatus AuthenticateWithLoanId(
      string loanId,
      string authCode,
      string uuid,
      ref string lat,
      ref string recipientId,
      string ipAddress)
    {
      LatAuthenticationLog authLog = new LatAuthenticationLog()
      {
        UUID = uuid,
        LoanId = loanId,
        IpAddress = ipAddress
      };
      IMongoCollection<BsonDocument> collection = this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts);
      FilterDefinition<BsonDocument> filter1 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.UUID, uuid) & Builders<BsonDocument>.Filter.Ne<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, (string) null) & Builders<BsonDocument>.Filter.Ne<string>((FieldDefinition<BsonDocument, string>) Constants.LAT, string.Empty);
      BsonDocument replacement = collection.Find<BsonDocument>(filter1).FirstOrDefault<BsonDocument, BsonDocument>();
      if (replacement == (BsonDocument) null)
      {
        this.CreateAuthLog(authLog, LATAuthenticationStatus.NotFound);
        return LATAuthenticationStatus.NotFound;
      }
      authLog.LAT = replacement[Constants.LAT].ToString();
      lat = authLog.LAT;
      if (!(bool) replacement[Constants.Enabled])
      {
        this.CreateAuthLog(authLog, LATAuthenticationStatus.Locked);
        return LATAuthenticationStatus.Locked;
      }
      if (authCode == replacement[Constants.AuthCode].ToString())
      {
        FilterDefinition<BsonDocument> filter2 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.UUID, uuid);
        BsonDocument bsonDocument = collection.Find<BsonDocument>(filter2).FirstOrDefault<BsonDocument, BsonDocument>();
        if (bsonDocument != (BsonDocument) null && bsonDocument != replacement)
        {
          this.CreateAuthLog(authLog, LATAuthenticationStatus.HasBeenAssociated);
          return LATAuthenticationStatus.HasBeenAssociated;
        }
        this.CreateAuthLog(authLog, LATAuthenticationStatus.Successful);
        recipientId = replacement[Constants.RecipientId].ToString();
        replacement[Constants.Attempts] = (BsonValue) 0;
        replacement[Constants.IsAuthenticated] = (BsonValue) true;
        collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
        return LATAuthenticationStatus.Successful;
      }
      this.CreateAuthLog(authLog, LATAuthenticationStatus.Mismatched);
      replacement[Constants.Attempts] = (BsonValue) ((int) replacement[Constants.Attempts] + 1);
      if ((int) replacement[Constants.Attempts] == 5)
        replacement[Constants.Enabled] = (BsonValue) false;
      collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
      return LATAuthenticationStatus.Mismatched;
    }

    public void WipeOutBorrowerUUIDs(string loanId)
    {
      IMongoCollection<BsonDocument> collection = this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts);
      FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId) & (Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.ContactType, "Borrower") | Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.ContactType, "CoBorrower"));
      foreach (BsonDocument replacement in collection.Find<BsonDocument>(filter).ToList<BsonDocument>())
      {
        replacement[Constants.UUID] = (BsonValue) string.Empty;
        collection.ReplaceOne(Builders<BsonDocument>.Filter.Eq<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) Constants._id, replacement[Constants._id]), replacement);
      }
    }

    public bool HasAccessToLoan(string uuid, string loanId)
    {
      return this.GetLoanContact(uuid, loanId) != (BsonDocument) null;
    }

    public bool GetContactInfo(
      string uuid,
      string loanId,
      ref string contactType,
      ref string borrowerId,
      ref string recipientId)
    {
      BsonDocument loanContact = this.GetLoanContact(uuid, loanId);
      if (loanContact == (BsonDocument) null)
      {
        recipientId = (string) null;
        borrowerId = (string) null;
        contactType = (string) null;
        return false;
      }
      recipientId = loanContact[Constants.RecipientId].ToString();
      borrowerId = loanContact[Constants.BorrowerId].ToString();
      contactType = loanContact[Constants.ContactType].ToString();
      return true;
    }

    private BsonDocument GetLoanContact(string uuid, string loanId)
    {
      return this.Connect().GetCollection<BsonDocument>(Collections.LoanContacts).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LoanId, loanId) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.UUID, uuid)).FirstOrDefault<BsonDocument, BsonDocument>();
    }

    private IMongoDatabase Connect()
    {
      MongoUnitOfWork mongoUnitOfWork = new MongoUnitOfWork();
      mongoUnitOfWork.Begin((IStorageSettings) this.storageSettings);
      return mongoUnitOfWork.ActiveConnection();
    }
  }
}
