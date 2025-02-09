// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.SiteRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Domain.Site;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class SiteRepository : ISiteRepository
  {
    static SiteRepository() => MongoInitializer.Initialize();

    public bool CreateAdminRule(string siteId, Guid ruleId, string AdminRuleData)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      if (string.IsNullOrEmpty(AdminRuleData))
        throw new ApplicationException("Invalid input, you need to pass request body.");
      IMongoCollection<BsonDocument> collection = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules);
      BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(AdminRuleData);
      document.InsertAt(0, new BsonElement(Constants.SiteIdField, (BsonValue) siteId));
      document.InsertAt(1, new BsonElement(Constants.RuleId, (BsonValue) ruleId.ToString()));
      FilterDefinitionBuilder<BsonDocument> filter1 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter2 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId) & filter1.Eq<bool>((FieldDefinition<BsonDocument, bool>) Constants.IsDefault, true) & !filter1.Eq<Guid>((FieldDefinition<BsonDocument, Guid>) Constants.RuleId, ruleId);
      List<BsonDocument> list1 = collection.Find<BsonDocument>(filter2).Sort(Builders<BsonDocument>.Sort.Ascending((FieldDefinition<BsonDocument>) Constants.OrderNumber)).ToList<BsonDocument>();
      if (document.Contains(Constants.IsDefault) && document[Constants.IsDefault] == (BsonValue) true)
      {
        foreach (BsonDocument replacement in list1)
        {
          if ((BsonDocument) null != replacement)
          {
            if (replacement.Contains(Constants.IsDefault))
              replacement[Constants.IsDefault] = (BsonValue) false;
            else
              replacement.InsertAt(8, new BsonElement(Constants.IsDefault, (BsonValue) true));
          }
          collection.ReplaceOne(filter2, replacement);
        }
      }
      FilterDefinition<BsonDocument> filter3 = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
      List<BsonDocument> list2 = collection.Find<BsonDocument>(filter3).Sort(Builders<BsonDocument>.Sort.Ascending((FieldDefinition<BsonDocument>) Constants.OrderNumber)).ToList<BsonDocument>();
      int result = 0;
      if (list2.Count > 0 && list2[list2.Count - 1].Contains(Constants.OrderNumber))
        int.TryParse(list2[list2.Count - 1][Constants.OrderNumber].ToString(), out result);
      document.InsertAt(2, new BsonElement(Constants.OrderNumber, (BsonValue) (result + 1)));
      collection.InsertOne(document);
      return true;
    }

    public bool UpdateAdminRule(string siteId, Guid ruleId, string AdminRuleData)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      if (string.IsNullOrEmpty(AdminRuleData))
        throw new ApplicationException("Invalid input, you need to pass request body.");
      IMongoCollection<BsonDocument> collection = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules);
      FilterDefinitionBuilder<BsonDocument> filter1 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter2 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RuleId, ruleId.ToString()) & filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
      if ((BsonDocument) null == collection.Find<BsonDocument>(filter2).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>())
        return false;
      BsonDocument bsonDocument = BsonSerializer.Deserialize<BsonDocument>(AdminRuleData);
      foreach (BsonElement bsonElement in bsonDocument)
      {
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set<BsonValue>((FieldDefinition<BsonDocument, BsonValue>) bsonElement.Name, bsonElement.Value);
        IMongoCollectionExtensions.FindOneAndUpdate<BsonDocument>(collection, filter2, update);
      }
      FilterDefinitionBuilder<BsonDocument> filter3 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter4 = filter3.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId) & filter3.Eq<bool>((FieldDefinition<BsonDocument, bool>) Constants.IsDefault, true) & !filter3.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RuleId, ruleId.ToString());
      List<BsonDocument> list = collection.Find<BsonDocument>(filter4).Sort(Builders<BsonDocument>.Sort.Ascending((FieldDefinition<BsonDocument>) Constants.OrderNumber)).ToList<BsonDocument>();
      if (bsonDocument.Contains(Constants.IsDefault) && bsonDocument[Constants.IsDefault] == (BsonValue) true)
      {
        foreach (BsonDocument replacement in list)
        {
          if ((BsonDocument) null != replacement)
          {
            if (replacement.Contains(Constants.IsDefault))
              replacement[Constants.IsDefault] = (BsonValue) false;
            else
              replacement.InsertAt(8, new BsonElement(Constants.IsDefault, (BsonValue) true));
          }
          collection.ReplaceOne(filter4, replacement);
        }
      }
      return true;
    }

    public string GetAdminRule(string siteId, Guid ruleId)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      IMongoCollection<BsonDocument> collection = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules);
      FilterDefinitionBuilder<BsonDocument> filter1 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter2 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RuleId, ruleId.ToString()) & filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
      List<BsonDocument> list = collection.Find<BsonDocument>(filter2).ToList<BsonDocument>();
      if (list.Count == 0)
        return string.Empty;
      List<string> stringList = new List<string>();
      foreach (BsonDocument bsonDocument in list)
      {
        bsonDocument.Add("Id", (BsonValue) bsonDocument[Constants._id].ToString());
        bsonDocument.Remove(Constants._id);
        bsonDocument.Remove(Constants.RuleId);
        stringList.Add(bsonDocument.ToString());
      }
      return stringList[0];
    }

    public List<string> GetAdminRules(string siteId)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      List<BsonDocument> list = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Sort(Builders<BsonDocument>.Sort.Ascending((FieldDefinition<BsonDocument>) Constants.OrderNumber)).ToList<BsonDocument>();
      if (list.Count == 0)
        return new List<string>();
      List<string> adminRules = new List<string>();
      foreach (BsonDocument bsonDocument in list)
      {
        bsonDocument.Add("Id", (BsonValue) bsonDocument[Constants._id].ToString());
        bsonDocument.Remove(Constants._id);
        adminRules.Add(bsonDocument.ToString());
      }
      return adminRules;
    }

    public bool DeleteAdminRule(string siteId, Guid ruleId)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      IMongoCollection<BsonDocument> collection = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules);
      FilterDefinitionBuilder<BsonDocument> filter1 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter2 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RuleId, ruleId.ToString()) & filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
      if (collection.Find<BsonDocument>(filter2).ToList<BsonDocument>().Count != 1)
        return false;
      collection.DeleteOne(filter2);
      return true;
    }

    public List<string> GetAdminRuleOrder(string siteId)
    {
      if (siteId == null)
        throw new ApplicationException("Invalid input, you need to pass a SiteId.");
      List<BsonDocument> list = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId)).Sort(Builders<BsonDocument>.Sort.Ascending((FieldDefinition<BsonDocument>) Constants.OrderNumber)).ToList<BsonDocument>();
      List<string> adminRuleOrder = new List<string>();
      foreach (BsonDocument bsonDocument in list)
      {
        if (bsonDocument.Contains(Constants.RuleId))
          adminRuleOrder.Add(bsonDocument[Constants.RuleId].ToString());
      }
      return adminRuleOrder;
    }

    public bool UpdateAdminRuleOrder(string siteId, List<Guid> AdminRuleOrderIds)
    {
      if (siteId == null)
        throw new ApplicationException(string.Format(SiteRepository.ErrorMessage.InvalidSiteId));
      if (AdminRuleOrderIds.Count == 0)
        throw new ApplicationException(string.Format(SiteRepository.ErrorMessage.EmptyInput));
      IMongoCollection<BsonDocument> collection = SiteRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.AdminRules);
      FilterDefinitionBuilder<BsonDocument> filter1 = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> filter2 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
      List<BsonDocument> list = collection.Find<BsonDocument>(filter2).ToList<BsonDocument>();
      if (list.Count != AdminRuleOrderIds.Count)
        throw new ApplicationException(string.Format(SiteRepository.ErrorMessage.NumberOfRulesMismatch, (object) siteId));
      List<Guid> guidList = new List<Guid>();
      foreach (Guid adminRuleOrderId in AdminRuleOrderIds)
        guidList.Add(adminRuleOrderId);
      foreach (BsonDocument bsonDocument in list)
      {
        if (guidList.Contains(new Guid(bsonDocument[Constants.RuleId].ToString())))
          guidList.Remove(new Guid(bsonDocument[Constants.RuleId].ToString()));
      }
      if (guidList.Count > 0)
        throw new ApplicationException(string.Format(SiteRepository.ErrorMessage.CheckGuids, (object) siteId));
      int num = 1;
      foreach (Guid adminRuleOrderId in AdminRuleOrderIds)
      {
        FilterDefinition<BsonDocument> filter3 = filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.RuleId, adminRuleOrderId.ToString()) & filter1.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, siteId);
        BsonDocument replacement = collection.Find<BsonDocument>(filter3).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
        if ((BsonDocument) null != replacement)
        {
          if (replacement.Contains(Constants.OrderNumber))
            replacement[Constants.OrderNumber] = (BsonValue) num;
          else
            replacement.InsertAt(0, new BsonElement(Constants.OrderNumber, (BsonValue) num));
          collection.ReplaceOne(filter3, replacement);
        }
        ++num;
      }
      return false;
    }

    private static IMongoDatabase Connect() => new MongoUnitOfWork().ActiveConnection();

    internal class ErrorMessage
    {
      public static string InvalidSiteId = "Invalid input, you need to pass a SiteId.";
      public static string EmptyInput = "Invalid input, you need to pass request body.";
      public static string NumberOfRulesMismatch = "The number of rules for SiteId '{0}' provided in request does not match with the number rules in Database.";
      public static string CheckGuids = "The RuleIds in request do not match with the RuleIds in DB for SiteId '{0}'.";
    }
  }
}
