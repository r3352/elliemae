// Decompiled with JetBrains decompiler
// Type: Elli.Data.MongoDB.DraftLoanRepository
// Assembly: Elli.Data.MongoDB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: F1D8D155-58C1-404A-A2A9-D942D1AE4E32
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Data.MongoDB.dll

using Elli.Data.MongoDB.Functions;
using Elli.Data.MongoDB.Mapping;
using Elli.Domain.Mortgage;
using Elli.Metrics;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ServiceInterface;
using Microsoft.IdentityModel.Claims;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

#nullable disable
namespace Elli.Data.MongoDB
{
  public class DraftLoanRepository : IDraftLoanRepository
  {
    private readonly IMetricsFactory _metricsFactory;
    private ILoanMetricsRecorder _metricsRecorder;

    public DraftLoanRepository(IMetricsFactory metricsFactory)
    {
      this._metricsFactory = metricsFactory;
    }

    static DraftLoanRepository()
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

    public ClaimsIdentity BorrowerContext
    {
      get
      {
        if (Thread.CurrentPrincipal == null)
          return (ClaimsIdentity) null;
        return !(Thread.CurrentPrincipal is ClaimsPrincipal) ? (ClaimsIdentity) null : (ClaimsIdentity) ((ClaimsPrincipal) Thread.CurrentPrincipal).Identity;
      }
    }

    public bool CreateDraftLoan(string loanApplicationContract)
    {
      try
      {
        IMongoDatabase mongoDatabase = DraftLoanRepository.Connect();
        BsonDocument document = BsonSerializer.Deserialize<BsonDocument>(loanApplicationContract);
        mongoDatabase.GetCollection<BsonDocument>("LoanApplications").InsertOne(document);
        return true;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(ex.Message);
      }
    }

    public object GetDraftLoan(Guid applicationId, bool blnIsCCAdmin)
    {
      BsonDocument bsonDocument = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, applicationId.ToString())).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      if (bsonDocument == (BsonDocument) null)
        return (object) string.Empty;
      if (!blnIsCCAdmin && this.BorrowerContext.Claims != null)
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        ClaimCollection claims = this.BorrowerContext.Claims;
        int num1 = ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Consumer")).Count<Claim>();
        int num2 = ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Enterprise")).Count<Claim>();
        ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Application")).Count<Claim>();
        if (num1 > 0)
        {
          string str1 = ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "sub")).Select<Claim, string>((Func<Claim, string>) (x => x.Value)).First<string>().ToString();
          string empty3 = string.Empty;
          if (bsonDocument.Contains(Constants.BorrowerProfileId))
            empty3 = bsonDocument[Constants.BorrowerProfileId].ToString();
          string str2 = empty3;
          if (str1 != str2)
            throw new UnauthorizedAccessException("You do not have the privileges to perform this operation.");
        }
        if (num2 > 0)
        {
          UserIdentity userIdentity = UserIdentity.Parse(((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "elli_uid")).Select<Claim, string>((Func<Claim, string>) (x => x.Value)).First<string>().ToString().ToString());
          userIdentity.ToString();
          string str = string.Empty;
          foreach (BsonElement bsonElement in bsonDocument)
          {
            if (bsonElement.Name == "LenderId")
              str = !bsonElement.Value.ToString().Contains("\\") ? bsonElement.Value.ToString() : UserIdentity.Parse(bsonElement.Value.ToString()).UserID.ToString();
          }
          if (userIdentity.UserID != str)
            throw new UnauthorizedAccessException("You do not have the privileges to perform this operation.");
        }
      }
      bsonDocument.Add("Id", (BsonValue) bsonDocument["_id"].ToString());
      bsonDocument.Remove("_id");
      return (object) bsonDocument.ToString();
    }

    public object UpdateDraftLoan(Guid applicationId, string loanApplicationContract)
    {
      IMongoCollection<BsonDocument> collection = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications);
      FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, applicationId.ToString());
      BsonDocument Target = collection.Find<BsonDocument>(filter).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
      if ((BsonDocument) null == Target)
        return (object) false;
      BsonDocument Source = BsonSerializer.Deserialize<BsonDocument>(loanApplicationContract);
      if (this.BorrowerContext.Claims == null)
        return (object) false;
      string empty1 = string.Empty;
      ClaimCollection claims = this.BorrowerContext.Claims;
      if (((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "elli_idt" && x.Value == "Consumer")).Count<Claim>() > 0)
      {
        string str1 = ((IEnumerable<Claim>) claims).Where<Claim>((Func<Claim, bool>) (x => x.ClaimType == "sub")).Select<Claim, string>((Func<Claim, string>) (x => x.Value)).First<string>().ToString();
        string empty2 = string.Empty;
        if (Target.Contains(Constants.BorrowerProfileId))
          empty2 = Target[Constants.BorrowerProfileId].ToString();
        string str2 = empty2;
        if (str1 != str2)
          throw new UnauthorizedAccessException("You do not have the privileges to perform this operation.");
      }
      BsonDocument replacement = PartialUpdate.MergeDocument(Source, Target);
      collection.ReplaceOne(filter, replacement);
      return (object) true;
    }

    public List<string> GetDraftLoanSummary(
      UserInfo currentUser,
      int start,
      int limit,
      string sort,
      string filters,
      string status,
      bool blnIsCCAdmin,
      out long xTotalCount)
    {
      List<string> draftLoanSummary = new List<string>();
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
          empty2 = strArray[strArray.Length - 1].ToString();
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
      SortDefinitionBuilder<BsonDocument> sort1 = Builders<BsonDocument>.Sort;
      FilterDefinitionBuilder<BsonDocument> filter = Builders<BsonDocument>.Filter;
      FilterDefinition<BsonDocument> FilterDef = filter.Empty;
      if (empty5 == "Enterprise" && !blnIsCCAdmin)
        FilterDef = filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.LenderId, empty2);
      else if (empty5 == "Consumer")
        FilterDef = filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.BorrowerProfileId, empty4);
      FilterDefinition<BsonDocument> applicationStatusFilter = DraftLoanRepository.getApplicationStatusFilter(status);
      FilterDefinition<BsonDocument> filters1 = DraftLoanRepository.getFilters(filters, FilterDef);
      SortDefinition<BsonDocument> sort2 = DraftLoanRepository.getSorts(sort).Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants._id);
      IMongoCollection<BsonDocument> collection = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications);
      List<BsonDocument> list = collection.Find<BsonDocument>(applicationStatusFilter & filters1).Sort(sort2).Skip(new int?(start - 1)).Limit(new int?(limit)).ToList<BsonDocument>();
      xTotalCount = collection.Count(applicationStatusFilter & filters1);
      foreach (BsonDocument bsonDocument in list)
      {
        bsonDocument.Remove("_id");
        draftLoanSummary.Add(bsonDocument.ToString());
      }
      return draftLoanSummary;
    }

    private static FilterDefinition<BsonDocument> getApplicationStatusFilter(string status)
    {
      FilterDefinitionBuilder<BsonDocument> definitionBuilder = new FilterDefinitionBuilder<BsonDocument>();
      BsonDocument bsonDocument = new BsonDocument();
      FilterDefinition<BsonDocument> applicationStatusFilter = definitionBuilder.Empty;
      int num = 0;
      foreach (string str in status.ToLower().Split(",".ToCharArray()))
      {
        switch (str)
        {
          case "draft":
            if (num == 0)
              applicationStatusFilter = definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "Draft");
            else
              applicationStatusFilter |= definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "Draft");
            ++num;
            break;
          case "error":
            if (num == 0)
              applicationStatusFilter = definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "Error");
            else
              applicationStatusFilter |= definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "Error");
            ++num;
            break;
          case "inprocess":
            if (num == 0)
              applicationStatusFilter = definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "InProcess");
            else
              applicationStatusFilter |= definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "InProcess");
            ++num;
            break;
        }
      }
      if (num == 0)
        applicationStatusFilter = definitionBuilder.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Status, "Draft");
      return applicationStatusFilter;
    }

    private static SortDefinition<BsonDocument> getSorts(string sorts)
    {
      SortDefinitionBuilder<BsonDocument> sort1 = Builders<BsonDocument>.Sort;
      SortDefinition<BsonDocument> sort2 = (SortDefinition<BsonDocument>) new BsonDocument();
      if (string.IsNullOrEmpty(sorts))
        return sort2;
      foreach (string str1 in sorts.Split(",".ToCharArray()))
      {
        if (!string.IsNullOrEmpty(str1))
        {
          bool flag = false;
          string empty = string.Empty;
          string str2;
          if (str1[0].ToString() == "-")
          {
            flag = true;
            str2 = str1.Replace("-", "");
          }
          else
            str2 = str1;
          switch (str2.ToLower())
          {
            case "borrower.emailaddresstext":
            case "borroweremailaddresstext":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerEmail) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerEmail);
              continue;
            case "borrower.firstname":
            case "borrowerfirstname":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerFirstName) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerFirstName);
              continue;
            case "borrower.lastname":
            case "borrowerlastname":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerLastName) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.BorrowerLastName);
              continue;
            case "coborrower.emailaddresstext":
            case "coborroweremailaddresstext":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerEmail) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerEmail);
              continue;
            case "coborrower.firstname":
            case "coborrowerfirstname":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerFirstName) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerFirstName);
              continue;
            case "coborrower.lastname":
            case "coborrowerlastname":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerLastName) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.CoBorrowerLastName);
              continue;
            case "lastmodified":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.LastModified) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.LastModified);
              continue;
            case "respa6":
              sort2 = !flag ? sort2.Ascending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.Respa6) : sort2.Descending<BsonDocument>((FieldDefinition<BsonDocument>) Constants.Respa6);
              continue;
            default:
              continue;
          }
        }
      }
      return sort2;
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
        switch (strArray[0].ToLower())
        {
          case "borrower.emailaddresstext":
          case "borroweremailaddresstext":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.BorrowerEmail, strArray[2].ToString());
            break;
          case "borrower.firstname":
          case "borrowerfirstname":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.BorrowerFirstName, strArray[2].ToString());
            break;
          case "borrower.lastname":
          case "borrowerlastname":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.BorrowerLastName, strArray[2].ToString());
            break;
          case "coborrower.emailaddresstext":
          case "coborroweremailaddresstext":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.CoBorrowerEmail, strArray[2].ToString());
            break;
          case "coborrower.firstname":
          case "coborrowerfirstname":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.CoBorrowerFirstName, strArray[2].ToString());
            break;
          case "coborrower.lastname":
          case "coborrowerlastname":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.CoBorrowerLastName, strArray[2].ToString());
            break;
          case "name":
            FilterDefinition<BsonDocument> filterDefinition = filter.Regex((FieldDefinition<BsonDocument>) Constants.BorrowerFirstName, new BsonRegularExpression(strArray[2].ToString())) | filter.Regex((FieldDefinition<BsonDocument>) Constants.BorrowerLastName, new BsonRegularExpression(strArray[2].ToString())) | filter.Regex((FieldDefinition<BsonDocument>) Constants.CoBorrowerFirstName, new BsonRegularExpression(strArray[2].ToString())) | filter.Regex((FieldDefinition<BsonDocument>) Constants.CoBorrowerLastName, new BsonRegularExpression(strArray[2].ToString()));
            FilterDef &= filterDefinition;
            break;
          case "siteid":
            FilterDef &= filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.SiteIdField, strArray[2].ToString());
            break;
        }
      }
      return FilterDef;
    }

    private static StringBuilder CompareObjects(JObject source, JObject target)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (KeyValuePair<string, JToken> keyValuePair in source)
      {
        if (keyValuePair.Value.Type == JTokenType.Object)
          stringBuilder.Append((object) DraftLoanRepository.CompareObjects(keyValuePair.Value.ToObject<JObject>(), target.GetValue(keyValuePair.Key).ToObject<JObject>()));
        else if (keyValuePair.Value.Type == JTokenType.Array)
        {
          stringBuilder.Append((object) DraftLoanRepository.CompareArrays(keyValuePair.Value.ToObject<JArray>(), target.GetValue(keyValuePair.Key).ToObject<JArray>(), keyValuePair.Key));
        }
        else
        {
          JToken t1 = keyValuePair.Value;
          JToken t2 = target.SelectToken(keyValuePair.Key);
          if (t2 == null)
            stringBuilder.Append("Key " + keyValuePair.Key + " not found" + Environment.NewLine);
          else if (!JToken.DeepEquals(t1, t2))
            stringBuilder.Append(keyValuePair.Key + ": " + (object) keyValuePair.Value + " !=  " + (object) target.Property(keyValuePair.Key).Value + Environment.NewLine);
        }
      }
      return stringBuilder;
    }

    private static StringBuilder CompareArrays(JArray source, JArray target, string arrayName = "")
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < source.Count; ++index)
      {
        JToken t1 = source[index];
        if (t1.Type == JTokenType.Object)
        {
          JToken jtoken = index >= target.Count ? (JToken) new JObject() : target[index];
          stringBuilder.Append((object) DraftLoanRepository.CompareObjects(t1.ToObject<JObject>(), jtoken.ToObject<JObject>()));
        }
        else
        {
          JToken t2 = index >= target.Count ? (JToken) "" : target[index];
          if (!JToken.DeepEquals(t1, t2))
          {
            if (string.IsNullOrEmpty(arrayName))
              stringBuilder.Append("Index " + (object) index + ": " + (object) t1 + " != " + (object) t2 + Environment.NewLine);
            else
              stringBuilder.Append("Key " + arrayName + "[" + (object) index + "]: " + (object) t1 + " != " + (object) t2 + Environment.NewLine);
          }
        }
      }
      return stringBuilder;
    }

    public bool SupportsArchive => true;

    private static IMongoDatabase Connect(DataStores dataStores = DataStores.ActiveStore)
    {
      MongoUnitOfWork mongoUnitOfWork = new MongoUnitOfWork();
      return dataStores != DataStores.ActiveStore ? mongoUnitOfWork.ArchiveConnection() : mongoUnitOfWork.ActiveConnection();
    }

    public bool UpdateDraftLoanStatusForPublisher(
      Guid loanGuid,
      out string userId,
      bool blnIsCCAdmin)
    {
      IMongoCollection<BsonDocument> collection = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications);
      bool flag = false;
      userId = (string) null;
      FilterDefinition<BsonDocument> filter;
      if (!blnIsCCAdmin)
      {
        if (this.BorrowerContext == null || this.BorrowerContext.Claims == null)
          throw new ApplicationException("No Valid JWT Token found with LoanId, '" + (object) loanGuid + "'");
        if (((IEnumerable<Claim>) this.BorrowerContext.Claims).Count<Claim>((Func<Claim, bool>) (v => v.ClaimType == "elli_idt" && v.Value == "Consumer")) > 0)
        {
          Claim claim = ((IEnumerable<Claim>) this.BorrowerContext.Claims).FirstOrDefault<Claim>((Func<Claim, bool>) (v => v.ClaimType == "sub"));
          if (claim == null || string.IsNullOrEmpty(claim.Value))
            throw new ApplicationException("No Valid JWT Token found with LoanId, '" + (object) loanGuid + "'");
          filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString()) & Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) "BorrowerProfileId", claim.Value);
          flag = true;
        }
        else if (((IEnumerable<Claim>) this.BorrowerContext.Claims).Count<Claim>((Func<Claim, bool>) (v => v.ClaimType == "elli_idt" && v.Value == "Enterprise")) > 0)
        {
          Claim claim = ((IEnumerable<Claim>) this.BorrowerContext.Claims).FirstOrDefault<Claim>((Func<Claim, bool>) (v => v.ClaimType == "elli_uid"));
          string str = claim != null && !string.IsNullOrEmpty(claim.Value) ? claim.Value : throw new ApplicationException("No Valid JWT Token found with LoanId, '" + (object) loanGuid + "'");
          if (str.StartsWith("Encompass\\"))
          {
            List<string> list = ((IEnumerable<string>) str.Split('\\')).ToList<string>();
            str = list.Count == 3 ? list[2] : list[1];
          }
          Dictionary<string, string> dictionary = new Dictionary<string, string>()
          {
            {
              "ContactType",
              "LOAN_OFFICER"
            },
            {
              "LoginId",
              str
            }
          };
          filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString()) & Builders<BsonDocument>.Filter.ElemMatch<BsonDocument>((FieldDefinition<BsonDocument>) "Contacts", (FilterDefinition<BsonDocument>) new BsonDocumentFilterDefinition<BsonDocument>(new BsonDocument((IDictionary) dictionary)));
        }
        else
        {
          if (((IEnumerable<Claim>) this.BorrowerContext.Claims).Count<Claim>((Func<Claim, bool>) (v => v.ClaimType == "elli_idt" && v.Value == "Application")) <= 0)
            throw new ApplicationException("No Valid JWT Token found with LoanId, '" + (object) loanGuid + "'");
          filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString());
          flag = true;
        }
      }
      else
        filter = Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString());
      UpdateDefinition<BsonDocument> update = (UpdateDefinition<BsonDocument>) new BsonDocument("$set", (BsonValue) new BsonDocument("Status", (BsonValue) "InProcess"));
      UpdateResult updateResult = collection.UpdateOne(filter, update);
      if (updateResult.MatchedCount > 0L && updateResult.ModifiedCount > 0L && flag | blnIsCCAdmin)
      {
        BsonDocument[] pipeline = new BsonDocument[3]
        {
          new BsonDocument()
          {
            {
              "$unwind",
              (BsonValue) "$Contacts"
            }
          },
          new BsonDocument()
          {
            {
              "$match",
              (BsonValue) new BsonDocument()
              {
                {
                  "Guid",
                  (BsonValue) loanGuid.ToString()
                },
                {
                  "Contacts.ContactType",
                  (BsonValue) "LOAN_OFFICER"
                }
              }
            }
          },
          new BsonDocument()
          {
            {
              "$project",
              (BsonValue) new BsonDocument()
              {
                {
                  "_id",
                  (BsonValue) 0
                },
                {
                  "Contacts.LoginId",
                  (BsonValue) 1
                }
              }
            }
          }
        };
        BsonDocument bsonDocument = collection.Aggregate<BsonDocument>((PipelineDefinition<BsonDocument, BsonDocument>) pipeline).FirstOrDefault<BsonDocument>();
        if (bsonDocument != (BsonDocument) null)
        {
          List<BsonElement> list = bsonDocument.Elements.ToList<BsonElement>();
          if (list.Count > 0)
            userId = list[0].Value[0].ToString();
        }
      }
      return updateResult.MatchedCount > 0L && updateResult.ModifiedCount > 0L;
    }

    public BsonDocument GetDraftLoanForSubscriber(Guid loanGuid)
    {
      return DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString()) & !Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) "Status", "Submit")).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>();
    }

    public bool UpdateDraftLoanStatusForSubscriber(
      Guid loanGuid,
      string status,
      DateTime submDateTime,
      string errorMessage,
      bool isReSubmitAllowed,
      bool isFailed,
      bool isBorrowerActionRequired,
      bool isLenderActionRequired,
      bool isEncompassLevelActionRequired,
      string errorDetails)
    {
      if (!isFailed)
      {
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set<string>((FieldDefinition<BsonDocument, string>) "Status", status).Set<BsonDocument, string>((FieldDefinition<BsonDocument, string>) "SubmitDate", submDateTime.ToString());
        UpdateResult updateResult = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications).UpdateOne(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString()), update);
        return updateResult.MatchedCount > 0L && updateResult.ModifiedCount > 0L;
      }
      BsonDocument bsonDocument = new BsonDocument()
      {
        {
          "ErrorMessage",
          (BsonValue) errorMessage
        },
        {
          "ReSubmitAllowed",
          (BsonValue) isReSubmitAllowed
        },
        {
          "BorrowerActionRequired",
          (BsonValue) isBorrowerActionRequired
        },
        {
          "LenderActionRequired",
          (BsonValue) isLenderActionRequired
        },
        {
          "EncompassLevelActionRequired",
          (BsonValue) isEncompassLevelActionRequired
        },
        {
          "ErrorDetails",
          (BsonValue) errorDetails
        }
      };
      UpdateDefinition<BsonDocument> update1 = Builders<BsonDocument>.Update.Set<string>((FieldDefinition<BsonDocument, string>) "Status", status).Set<BsonDocument, string>((FieldDefinition<BsonDocument, string>) "SubmitDate", "").Set<BsonDocument, BsonDocument>((FieldDefinition<BsonDocument, BsonDocument>) "Error", bsonDocument);
      UpdateResult updateResult1 = DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications).UpdateOne(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, loanGuid.ToString()), update1);
      return updateResult1.MatchedCount > 0L && updateResult1.ModifiedCount > 0L;
    }

    public bool CheckDraftLoanExist(Guid applicationId)
    {
      return !(DraftLoanRepository.Connect().GetCollection<BsonDocument>(Elli.Data.MongoDB.Collections.LoanApplications).Find<BsonDocument>(Builders<BsonDocument>.Filter.Eq<string>((FieldDefinition<BsonDocument, string>) Constants.Guid, applicationId.ToString())).Limit(new int?(1)).FirstOrDefault<BsonDocument, BsonDocument>() == (BsonDocument) null);
    }
  }
}
