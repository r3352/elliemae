// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BorrowerContact
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.ReportingDbUtils.Query;
using EllieMae.EMLite.Server.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class BorrowerContact : EllieMae.EMLite.Server.Contact
  {
    private const string className = "BorrowerContact�";
    private static readonly string[] DefaultSummaryFields = new string[10]
    {
      "Contact.ContactID",
      "Contact.FirstName",
      "Contact.LastName",
      "Contact.HomePhone",
      "Contact.PersonalEmail",
      "Contact.OwnerID",
      "Contact.ContactType",
      "Contact.Status",
      "Contact.NoSpam",
      "Contact.LastModified"
    };
    private BorrowerInfo info;

    public BorrowerContact(ICacheLock<bool?> innerLock)
      : base(innerLock)
    {
    }

    public BorrowerContact(ContactStore.ContactIdentity id)
      : base(id)
    {
    }

    public override string FullName => this.GetContactInfo().FullName;

    protected override string ContactTable => "Borrower";

    protected override string HistoryTable => "BorrowerHistory";

    protected override string NotesTable => "BorrowerNotes";

    protected override string CustomFieldTable => "BorCustomField";

    protected override string CampaignActivityTable => "BorrowerCampaignActivity";

    public BorrowerInfo GetContactInfo()
    {
      this.validateExists();
      if (this.info == null)
        this.info = this.getContactInfoFromDatabase();
      return this.info;
    }

    public void CheckIn(BorrowerInfo info) => this.CheckIn(info, false);

    public void CheckIn(BorrowerInfo info, bool keepCheckedOut)
    {
      this.validateInstance();
      this.saveContactInfoToDatabase(info);
      this.info = info;
    }

    public static BorrowerInfo[] GetBorrowersByOwner(string ownerId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from Borrower where OwnerID = '" + ownerId + "'");
        object[] objArray = BorrowerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery(), false);
        List<BorrowerInfo> borrowerInfoList = new List<BorrowerInfo>();
        foreach (object obj in objArray)
          borrowerInfoList.Add((BorrowerInfo) obj);
        return borrowerInfoList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo[]) null;
      }
    }

    public static int GetBorrowerIDFromGuid(string borrowerGuid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select distinct ContactID from Borrower where Guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(borrowerGuid));
        object obj = dbQueryBuilder.ExecuteScalar();
        return obj != null ? (int) obj : -1;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return -1;
      }
    }

    public static BorrowerInfo[] QueryBorrowersConflict(QueryCriterion[] criteria)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select distinct Contact.* from Borrower Contact ");
        dbQueryBuilder.AppendLine(" where (1 = 1)");
        ICriterionTranslator fieldTranslator = (ICriterionTranslator) new ContactCriterionTranslator("BorCustomField", BorrowerCustomFields.Get());
        for (int index = 0; index < criteria.Length; ++index)
          dbQueryBuilder.AppendLine("   and (" + criteria[index].ToSQLClause(fieldTranslator) + ")");
        object[] objArray = BorrowerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery(), false);
        List<BorrowerInfo> borrowerInfoList = new List<BorrowerInfo>();
        foreach (object obj in objArray)
          borrowerInfoList.Add((BorrowerInfo) obj);
        return borrowerInfoList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo[]) null;
      }
    }

    public static string GenerateQueryResultQueryString(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      string[] fields = fieldList.Replace(" ", "").Split(',');
      if (fields == null || fields.Length == 0)
        fields = BorrowerContact.DefaultSummaryFields;
      QueryCriterion filter = (QueryCriterion) null;
      if (criteria != null)
      {
        foreach (QueryCriterion criterion in criteria)
          filter = filter != null ? filter.And(criterion) : criterion;
      }
      DataQuery query = new DataQuery((IEnumerable) fields, filter);
      if (sortFields != null)
        query.SortFields.AddRange((IEnumerable<SortField>) sortFields);
      return new BorrowerQuery(user, matchType).GetSqlQueryForDataQuery(query, false);
    }

    public static QueryResult GenerateQueryResult(
      string[] fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      return BorrowerContact.generateQueryResult(fieldList == null || fieldList.Length == 0 ? "*" : string.Join(", ", fieldList), user, criteria, matchType, sortFields);
    }

    private static QueryResult generateQueryResult(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      string[] fields = fieldList.Replace(" ", "").Split(',');
      if (fields == null || fields.Length == 0)
        fields = BorrowerContact.DefaultSummaryFields;
      QueryCriterion filter = (QueryCriterion) null;
      if (criteria != null)
      {
        foreach (QueryCriterion criterion in criteria)
          filter = filter != null ? filter.And(criterion) : criterion;
      }
      DataQuery query = new DataQuery((IEnumerable) fields, filter);
      if (sortFields != null)
        query.SortFields.AddRange((IEnumerable<SortField>) sortFields);
      return new BorrowerQuery(user, matchType).Execute(query, false);
    }

    private static DbQueryBuilder generateQuerySql(
      string fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      DbQueryBuilder querySql = new DbQueryBuilder();
      querySql.AppendLine("select " + fieldList + " ");
      IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
      QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
      BorrowerQuery borrowerQuery = new BorrowerQuery(user, matchType);
      querySql.Append(borrowerQuery.GetTableSelectionClause(fields, filter, sortFields, true, true, false));
      querySql.Append(borrowerQuery.GetOrderByClause(sortFields));
      return querySql;
    }

    private static int[] getGroupIDsOfUser(string userId)
    {
      AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userId);
      int[] groupIdsOfUser = new int[groupsOfUser.Length];
      for (int index = 0; index < groupsOfUser.Length; ++index)
        groupIdsOfUser[index] = groupsOfUser[index].ID;
      return groupIdsOfUser;
    }

    public static string getQueryBorrowerIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BorrowerContact.generateQuerySql("Contact.ContactID", user, criteria, matchType, sortOrder).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (string) null;
      }
    }

    public static string getQueryBorrowerSummariesSql(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BorrowerContact.generateQuerySql("Contact.ContactID, Contact.FirstName, Contact.LastName, Contact.HomePhone, Contact.PersonalEmail, Contact.OwnerID, Contact.ContactType, Contact.Status, Contact.NoSpam, Contact.LastModified, Owner.Last_Name, Owner.First_Name", user, criteria, matchType, sortOrder).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (string) null;
      }
    }

    public static int[] QueryBorrowerIds(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      return BorrowerContact.QueryBorrowerIds(user, criteria, matchType, new SortField[1]
      {
        new SortField("Contact.ContactID", FieldSortOrder.Ascending)
      });
    }

    public static int[] QueryBorrowerIds(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      StringBuilder stringBuilder = new StringBuilder(BorrowerContact.getQueryBorrowerIdsSql(user, criteria, matchType, sortOrder));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["ContactID"]);
      return numArray;
    }

    public static BorrowerSummaryInfo[] QueryBorrowerSummaries(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        string fieldList = "Contact.ContactID, Contact.FirstName, Contact.LastName, Contact.HomePhone, Contact.PersonalEmail, Contact.OwnerID, Contact.ContactType, Contact.Status, Contact.NoSpam, Contact.LastModified, Owner.Last_Name, Owner.First_Name";
        if (sortOrder != null)
        {
          foreach (SortField sortField in sortOrder)
          {
            if (fieldList.IndexOf(sortField.Term.FieldName) < 0)
              fieldList = fieldList + ", " + sortField.Term.FieldName;
          }
        }
        return BorrowerContact.executeListSummaryQuery(BorrowerContact.generateQueryResult(fieldList, user, criteria, matchType, sortOrder).ToDataTable());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public static BorrowerSummaryInfo[] GetBorrowerSummaries(int[] contactIds)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < contactIds.Length; ++index)
          stringBuilder.Append((index > 0 ? "," : "") + contactIds[index].ToString());
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select " + string.Join(", ", BorrowerContact.DefaultSummaryFields));
        dbQueryBuilder.AppendLine("from   Borrower Contact");
        dbQueryBuilder.AppendLine("where  Contact.ContactID in (" + stringBuilder.ToString() + ")");
        BorrowerSummaryInfo[] borrowerSummaryInfoArray = BorrowerContact.executeListSummaryQuery(dbQueryBuilder.ExecuteTableQuery());
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < borrowerSummaryInfoArray.Length; ++index)
          hashtable.Add((object) borrowerSummaryInfoArray[index].ContactID, (object) borrowerSummaryInfoArray[index]);
        BorrowerSummaryInfo[] borrowerSummaries = new BorrowerSummaryInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          borrowerSummaries[index] = (BorrowerSummaryInfo) hashtable[(object) contactIds[index]];
        return borrowerSummaries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public static BorrowerInfo[] QueryBorrowers(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        object[] objArray = BorrowerContact.executeListQuery(BorrowerContact.generateQuerySql("Contact.*", user, criteria, matchType, sortOrder).ExecuteTableQuery(), false);
        List<BorrowerInfo> borrowerInfoList = new List<BorrowerInfo>();
        foreach (object obj in objArray)
          borrowerInfoList.Add((BorrowerInfo) obj);
        return borrowerInfoList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo[]) null;
      }
    }

    public static BorrowerInfo[] QueryBorrowers(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder,
      string fieldList,
      int offset,
      int recordCount,
      out int totalRecords)
    {
      totalRecords = 0;
      List<SortColumn> sortColumnList = (List<SortColumn>) null;
      try
      {
        if (string.IsNullOrEmpty(fieldList))
          fieldList = "Contact.*";
        else if (sortOrder != null && ((IEnumerable<SortField>) sortOrder).Any<SortField>())
        {
          List<string> list = ((IEnumerable<SortField>) sortOrder).Where<SortField>((System.Func<SortField, bool>) (x => !fieldList.ToLower().Contains(x.Term.ToString().ToLower()))).Select<SortField, string>((System.Func<SortField, string>) (y => y.Term.ToString())).ToList<string>();
          if (list != null && list.Any<string>())
            fieldList = fieldList + ", " + string.Join(", ", (IEnumerable<string>) list);
        }
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        IQueryTerm[] fields = (IQueryTerm[]) DataField.CreateFields(fieldList.Replace(" ", "").Split(','));
        QueryCriterion filter = QueryCriterion.Join(criteria, BinaryOperator.And);
        BorrowerQuery borrowerQuery = new BorrowerQuery(user, matchType);
        string fieldSelectionList = borrowerQuery.GetFieldSelectionList(fields);
        dbQueryBuilder.AppendLine("select " + fieldSelectionList);
        dbQueryBuilder.Append(borrowerQuery.GetTableSelectionClause(fields, filter, sortOrder, true, true, false));
        int endRecordNumber = offset + recordCount - 1;
        if (sortOrder != null && ((IEnumerable<SortField>) sortOrder).Any<SortField>())
        {
          sortColumnList = new List<SortColumn>();
          for (int index = 0; index < sortOrder.Length; ++index)
          {
            string columnName = QueryEngine.CriterionNameToColumnName(sortOrder[index].Term.FieldName.Replace(" ", ""));
            sortColumnList.Add(new SortColumn(columnName, sortOrder[index].SortOrder == FieldSortOrder.Descending ? SortOrder.Descending : SortOrder.Ascending));
          }
        }
        DataTable paginatedRecords = new DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), offset, endRecordNumber, sortColumnList);
        object[] objArray = BorrowerContact.executeListQuery(paginatedRecords, false);
        List<BorrowerInfo> source = new List<BorrowerInfo>();
        foreach (object obj in objArray)
          source.Add((BorrowerInfo) obj);
        totalRecords = source.Any<BorrowerInfo>() ? Convert.ToInt32(paginatedRecords.Rows[0]["TotalRowCount"]) : 0;
        return source.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo[]) null;
      }
    }

    public static BorrowerInfo[] GetBorrowers(int[] contactIds)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < contactIds.Length; ++index)
          stringBuilder.Append((index > 0 ? "," : "") + contactIds[index].ToString());
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select Contact.*");
        dbQueryBuilder.AppendLine("from   Borrower Contact");
        dbQueryBuilder.AppendLine("where  Contact.ContactID in (" + stringBuilder.ToString() + ")");
        object[] objArray = BorrowerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery(), false);
        List<BorrowerInfo> borrowerInfoList = new List<BorrowerInfo>();
        foreach (object obj in objArray)
          borrowerInfoList.Add((BorrowerInfo) obj);
        BorrowerInfo[] array = borrowerInfoList.ToArray();
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < array.Length; ++index)
          hashtable.Add((object) array[index].ContactID, (object) array[index]);
        BorrowerInfo[] borrowers = new BorrowerInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          borrowers[index] = (BorrowerInfo) hashtable[(object) contactIds[index]];
        return borrowers;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo[]) null;
      }
    }

    public static BorrowerSummaryInfo[] GetBorrowerSummaries(
      UserInfo user,
      int[] contactIds,
      string[] fields)
    {
      try
      {
        string fieldList = string.Join(", ", fields);
        QueryCriterion queryCriterion = (QueryCriterion) new ListValueCriterion("Contact.ContactID", (Array) contactIds);
        object[] objArray = BorrowerContact.executeListQuery(BorrowerContact.generateQueryResult(fieldList, user, new QueryCriterion[1]
        {
          queryCriterion
        }, RelatedLoanMatchType.None, new SortField[0]).ToDataTable(), true);
        List<BorrowerSummaryInfo> borrowerSummaryInfoList = new List<BorrowerSummaryInfo>();
        foreach (object obj in objArray)
          borrowerSummaryInfoList.Add((BorrowerSummaryInfo) obj);
        BorrowerSummaryInfo[] array = borrowerSummaryInfoList.ToArray();
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < array.Length; ++index)
          hashtable.Add((object) array[index].ContactID, (object) array[index]);
        BorrowerSummaryInfo[] borrowerSummaries = new BorrowerSummaryInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          borrowerSummaries[index] = (BorrowerSummaryInfo) hashtable[(object) contactIds[index]];
        return borrowerSummaries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public static BorrowerSummaryInfo[] GetBorrowerSummaries(
      UserInfo user,
      int[] contactIds,
      string[] fields,
      int maxContactsPerQuery = 1000)
    {
      List<BorrowerSummaryInfo> borrowerSummaryInfoList = new List<BorrowerSummaryInfo>();
      try
      {
        string fieldList = string.Join(", ", fields);
        for (int sourceIndex = 0; sourceIndex < contactIds.Length; sourceIndex += maxContactsPerQuery)
        {
          int length = Math.Min(maxContactsPerQuery, contactIds.Length - sourceIndex);
          int[] numArray = new int[length];
          Array.Copy((Array) contactIds, sourceIndex, (Array) numArray, 0, length);
          ListValueCriterion listValueCriterion = new ListValueCriterion("Contact.ContactID", (Array) numArray);
          QueryResult queryResult = BorrowerContact.generateQueryResult(fieldList, user, new QueryCriterion[1]
          {
            (QueryCriterion) listValueCriterion
          }, RelatedLoanMatchType.None, new SortField[0]);
          borrowerSummaryInfoList.AddRange((IEnumerable<BorrowerSummaryInfo>) BorrowerContact.executeListSummaryQuery(queryResult.ToDataTable()));
        }
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < borrowerSummaryInfoList.Count; ++index)
          hashtable.Add((object) borrowerSummaryInfoList[index].ContactID, (object) borrowerSummaryInfoList[index]);
        BorrowerSummaryInfo[] borrowerSummaries = new BorrowerSummaryInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          borrowerSummaries[index] = (BorrowerSummaryInfo) hashtable[(object) contactIds[index]];
        return borrowerSummaries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerSummaryInfo[]) null;
      }
    }

    public static int CreateNew(
      BorrowerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Declare("@contactId", "int");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("Borrower"), BorrowerContact.createDbValueList(info), true, false);
        dbQueryBuilder.SelectIdentity("@contactId");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BorrowerHistory"), new DbValueList()
        {
          {
            "ContactID",
            (object) "@contactId",
            (IDbEncoder) DbEncoding.None
          },
          {
            "TimeOfHistory",
            (object) firstContactDate
          },
          {
            "EventType",
            (object) "First Contact"
          },
          {
            "ContactSource",
            (object) contactSource.ToString()
          }
        }, true, false);
        dbQueryBuilder.Select("@contactId");
        return (int) dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return -1;
      }
    }

    private static DbValueList createDbValueList(BorrowerInfo info)
    {
      return new DbValueList()
      {
        {
          "FirstName",
          (object) info.FirstName
        },
        {
          "MiddleName",
          (object) info.MiddleName
        },
        {
          "LastName",
          (object) info.LastName
        },
        {
          "SuffixName",
          (object) info.SuffixName
        },
        {
          "Salutation",
          (object) info.Salutation
        },
        {
          "HomeAddress1",
          (object) info.HomeAddress.Street1
        },
        {
          "HomeAddress2",
          (object) info.HomeAddress.Street2
        },
        {
          "HomeCity",
          (object) info.HomeAddress.City
        },
        {
          "HomeState",
          (object) info.HomeAddress.State
        },
        {
          "HomeZip",
          (object) info.HomeAddress.Zip
        },
        {
          "BizAddress1",
          (object) info.BizAddress.Street1
        },
        {
          "BizAddress2",
          (object) info.BizAddress.Street2
        },
        {
          "BizCity",
          (object) info.BizAddress.City
        },
        {
          "BizState",
          (object) info.BizAddress.State
        },
        {
          "BizZip",
          (object) info.BizAddress.Zip
        },
        {
          "BizWebUrl",
          (object) info.BizWebUrl
        },
        {
          "EmployerName",
          (object) info.EmployerName
        },
        {
          "JobTitle",
          (object) info.JobTitle
        },
        {
          "WorkPhone",
          (object) info.WorkPhone
        },
        {
          "HomePhone",
          (object) info.HomePhone
        },
        {
          "MobilePhone",
          (object) info.MobilePhone
        },
        {
          "FaxNumber",
          (object) info.FaxNumber
        },
        {
          "PersonalEmail",
          (object) info.PersonalEmail
        },
        {
          "BizEmail",
          (object) info.BizEmail
        },
        {
          "Birthdate",
          (object) info.Birthdate,
          (IDbEncoder) DbEncoding.ShortDateTime
        },
        {
          "Married",
          (object) info.Married,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "PrimaryContact",
          (object) info.PrimaryContact,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "SpouseContactID",
          (object) info.SpouseContactID,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "SpouseName",
          (object) info.SpouseName
        },
        {
          "Anniversary",
          (object) info.Anniversary,
          (IDbEncoder) DbEncoding.ShortDateTime
        },
        {
          "CustField1",
          (object) info.CustField1
        },
        {
          "CustField2",
          (object) info.CustField2
        },
        {
          "CustField3",
          (object) info.CustField3
        },
        {
          "CustField4",
          (object) info.CustField4
        },
        {
          "PrimaryEmail",
          (object) info.PrimaryEmail
        },
        {
          "PrimaryPhone",
          (object) info.PrimaryPhone
        },
        {
          "NoSpam",
          (object) info.NoSpam,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "OwnerID",
          (object) info.OwnerID,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "IsPublic",
          (object) (info.AccessLevel == ContactAccess.Public ? 1 : 0)
        },
        {
          "ContactType",
          (object) BorrowerTypeEnumUtil.ValueToName(info.ContactType)
        },
        {
          "Status",
          (object) info.Status
        },
        {
          "NoCall",
          (object) info.NoCall,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "NoFax",
          (object) info.NoFax,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "SSN",
          (object) info.SSN
        },
        {
          "Referral",
          (object) info.Referral
        },
        {
          "Income",
          (object) info.Income
        },
        {
          "LeadSource",
          (object) info.LeadSource
        },
        {
          "LeadTxnID",
          (object) info.LeadTxnID
        },
        {
          "LastModified",
          (object) DateTime.Now
        }
      };
    }

    private BorrowerInfo getContactInfoFromDatabase()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.ContactTable), new DbValue("ContactID", (object) this.id.ContactID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Warning, nameof (BorrowerContact), (ServerException) new ObjectNotFoundException("Contact not found with ID " + this.id.ToString(), ObjectType.Contact, (object) this.id.ToString()));
        return BorrowerContact.dataRowToBorrowerInfo(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (BorrowerInfo) null;
      }
    }

    private void saveContactInfoToDatabase(BorrowerInfo info)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable(this.ContactTable), BorrowerContact.createDbValueList(info), new DbValue("ContactID", (object) this.id.ContactID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
      }
    }

    private static BorrowerSummaryInfo[] executeListSummaryQuery(DataTable dt)
    {
      DataRowCollection rows = dt.Rows;
      BorrowerSummaryInfo[] borrowerSummaryInfoArray = new BorrowerSummaryInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        borrowerSummaryInfoArray[index] = BorrowerContact.dataRowToBorrowerSummaryInfo(rows[index]);
      return borrowerSummaryInfoArray;
    }

    private static object[] executeListQuery(DataTable dt, bool borrowerSummaryInfo)
    {
      DataRowCollection rows = dt.Rows;
      List<object> objectList = new List<object>();
      if (borrowerSummaryInfo)
      {
        for (int index = 0; index < rows.Count; ++index)
          objectList.Add((object) BorrowerContact.dataRowToBorrowerSummaryInfo(rows[index]));
      }
      else
      {
        for (int index = 0; index < rows.Count; ++index)
          objectList.Add((object) BorrowerContact.dataRowToBorrowerInfo(rows[index]));
      }
      return objectList.ToArray();
    }

    private static BorrowerSummaryInfo dataRowToBorrowerSummaryInfo(DataRow r)
    {
      Hashtable data = BorrowerContact.constructHashtable(r);
      return data.Count > 0 ? new BorrowerSummaryInfo(data) : new BorrowerSummaryInfo((int) r["ContactID"], EllieMae.EMLite.DataAccess.SQL.Decode(r["OwnerID"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LastName"], (object) "").ToString(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoSpam"], (object) false), EllieMae.EMLite.DataAccess.SQL.Decode(r["FirstName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomePhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PersonalEmail"], (object) "").ToString(), BorrowerTypeEnumUtil.NameToValue(EllieMae.EMLite.DataAccess.SQL.Decode(r["ContactType"], (object) "").ToString()), EllieMae.EMLite.DataAccess.SQL.Decode(r["Status"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["LastModified"], (object) DateTime.MinValue));
    }

    private static Hashtable constructHashtable(DataRow row)
    {
      Hashtable hashtable1 = new Hashtable();
      foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
      {
        switch (column.ColumnName)
        {
          case "Contact.AccessLevel":
            hashtable1[(object) column.ColumnName] = (object) (ContactAccess) Enum.Parse(typeof (ContactAccess), EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName], (object) ContactAccess.Private.ToString()).ToString(), true);
            continue;
          case "Contact.Birthdate":
          case "Contact.LastModified":
          case "ContactOpportunity.PurchaseDate":
            if (string.Concat(row[column.ColumnName]) == "")
            {
              hashtable1[(object) column.ColumnName] = (object) DateTime.MinValue;
              continue;
            }
            hashtable1[(object) column.ColumnName] = (object) DateTime.Parse(string.Concat(row[column.ColumnName]));
            continue;
          case "Contact.ContactID":
          case "ContactOpportunity.CreditRating":
          case "ContactOpportunity.Term":
            if (string.Concat(row[column.ColumnName]) == "")
            {
              hashtable1[(object) column.ColumnName] = (object) 0;
              continue;
            }
            hashtable1[(object) column.ColumnName] = (object) int.Parse(string.Concat(row[column.ColumnName]));
            continue;
          case "Contact.ContactType":
            object obj1 = row[column.ColumnName];
            BorrowerType borrowerType = BorrowerType.Blank;
            string defaultValue1 = borrowerType.ToString();
            if (string.Concat(EllieMae.EMLite.DataAccess.SQL.Decode(obj1, (object) defaultValue1)) == "")
            {
              Hashtable hashtable2 = hashtable1;
              string columnName = column.ColumnName;
              Type enumType = typeof (BorrowerType);
              borrowerType = BorrowerType.Blank;
              string str = borrowerType.ToString();
              // ISSUE: variable of a boxed type
              __Boxed<BorrowerType> local = (Enum) (BorrowerType) Enum.Parse(enumType, str, true);
              hashtable2[(object) columnName] = (object) local;
              continue;
            }
            Hashtable hashtable3 = hashtable1;
            string columnName1 = column.ColumnName;
            Type enumType1 = typeof (BorrowerType);
            object obj2 = row[column.ColumnName];
            borrowerType = BorrowerType.Blank;
            string defaultValue2 = borrowerType.ToString();
            string str1 = EllieMae.EMLite.DataAccess.SQL.Decode(obj2, (object) defaultValue2).ToString();
            // ISSUE: variable of a boxed type
            __Boxed<BorrowerType> local1 = (Enum) (BorrowerType) Enum.Parse(enumType1, str1, true);
            hashtable3[(object) columnName1] = (object) local1;
            continue;
          case "Contact.Income":
          case "Opportunity.CashOut":
          case "Opportunity.DownPayment":
          case "Opportunity.HousingPayment":
          case "Opportunity.LoanAmount":
          case "Opportunity.MortgageBalance":
          case "Opportunity.MortgageRate":
          case "Opportunity.NonHousingPayment":
          case "Opportunity.PropertyValue":
            if (string.Concat(row[column.ColumnName]) == "")
            {
              hashtable1[(object) column.ColumnName] = (object) 0;
              continue;
            }
            hashtable1[(object) column.ColumnName] = (object) Decimal.Parse(string.Concat(row[column.ColumnName]));
            continue;
          case "ContactOpportunity.Bankruptcy":
            hashtable1[(object) column.ColumnName] = string.Concat(row[column.ColumnName]) == "1" ? (object) "Y" : (object) "N";
            continue;
          case "Opportunity.Amortization":
            hashtable1[(object) column.ColumnName] = (object) string.Concat(row[column.ColumnName]);
            continue;
          case "Opportunity.Employment":
            hashtable1[(object) column.ColumnName] = (object) EmploymentStatusEnumUtil.NameToValue(string.Concat(row[column.ColumnName]));
            continue;
          case "Opportunity.PropertyType":
            hashtable1[(object) column.ColumnName] = (object) PropertyTypeEnumUtil.NameToValue(string.Concat(row[column.ColumnName]));
            continue;
          case "Opportunity.PropertyUse":
            hashtable1[(object) column.ColumnName] = (object) PropertyUseEnumUtil.NameToValue(string.Concat(row[column.ColumnName]));
            continue;
          case "Opportunity.Purpose":
            hashtable1[(object) column.ColumnName] = (object) LoanPurposeEnumUtil.NameToValue(string.Concat(row[column.ColumnName]));
            continue;
          default:
            hashtable1[(object) column.ColumnName] = (object) string.Concat(row[column.ColumnName]);
            continue;
        }
      }
      return hashtable1;
    }

    public static void InsertBorrowerContactLoan(int contactID, string borrowerID, string loanGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue dbValue1 = new DbValue(nameof (contactID), (object) contactID);
      DbValue dbValue2 = new DbValue("randomID", (object) borrowerID);
      DbValue dbValue3 = new DbValue("Guid", (object) loanGuid);
      dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BorrowerLoans"), new DbValueList(new DbValue[3]
      {
        dbValue1,
        dbValue2,
        dbValue3
      }), true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteBorrowerContactLoan(int contactID, string borrowerID, string loanGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue dbValue1 = new DbValue(nameof (contactID), (object) contactID);
      DbValue dbValue2 = new DbValue("randomID", (object) borrowerID);
      DbValue dbValue3 = new DbValue("Guid", (object) loanGuid);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("BorrowerLoans"), new DbValueList(new DbValue[3]
      {
        dbValue1,
        dbValue2,
        dbValue3
      }));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteBorrowerContactLoan(string loanGuid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue dbValue = new DbValue("Guid", (object) loanGuid);
      dbQueryBuilder.DeleteFrom(DbAccessManager.GetTable("BorrowerLoans"), new DbValueList(new DbValue[1]
      {
        dbValue
      }));
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static BorrowerInfo dataRowToBorrowerInfo(DataRow r)
    {
      ContactAccess accessLevel = ContactAccess.Private;
      try
      {
        accessLevel = (ContactAccess) Enum.Parse(typeof (ContactAccess), EllieMae.EMLite.DataAccess.SQL.Decode(r["AccessLevel"], (object) ContactAccess.Private.ToString()).ToString(), true);
      }
      catch
      {
      }
      Hashtable data = BorrowerContact.constructHashtable(r);
      return data.Count > 0 ? new BorrowerInfo(data) : new BorrowerInfo((int) r["ContactID"], EllieMae.EMLite.DataAccess.SQL.Decode(r["FirstName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["MiddleName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LastName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["SuffixName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["OwnerID"], (object) "").ToString(), accessLevel, new EllieMae.EMLite.ClientServer.Address(EllieMae.EMLite.DataAccess.SQL.Decode(r["HomeAddress1"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomeAddress2"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomeCity"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomeState"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomeZip"], (object) "").ToString()), new EllieMae.EMLite.ClientServer.Address(EllieMae.EMLite.DataAccess.SQL.Decode(r["BizAddress1"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizAddress2"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizCity"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizState"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizZip"], (object) "").ToString()), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizWebUrl"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["EmployerName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["JobTitle"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["WorkPhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomePhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["MobilePhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["FaxNumber"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PersonalEmail"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizEmail"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["Birthdate"], (object) DateTime.MinValue), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["Married"], (object) false), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["SpouseContactID"], (object) 0), EllieMae.EMLite.DataAccess.SQL.Decode(r["SpouseName"], (object) "").ToString(), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["Anniversary"], (object) DateTime.MinValue), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField1"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField2"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField3"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField4"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PrimaryEmail"], (object) "").ToString().Trim(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PrimaryPhone"], (object) "").ToString().Trim(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoSpam"], (object) false), BorrowerTypeEnumUtil.NameToValue(EllieMae.EMLite.DataAccess.SQL.Decode(r["ContactType"], (object) "").ToString()), EllieMae.EMLite.DataAccess.SQL.Decode(r["Status"], (object) "").ToString(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoCall"], (object) false), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoFax"], (object) false), EllieMae.EMLite.DataAccess.SQL.Decode(r["SSN"], (object) "").ToString(), Convert.ToDecimal(EllieMae.EMLite.DataAccess.SQL.Decode(r["Income"], (object) 0)), EllieMae.EMLite.DataAccess.SQL.Decode(r["Referral"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LeadSource"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LeadTxnID"], (object) "").ToString(), (DateTime) r["LastModified"], (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["PrimaryContact"], (object) false), EllieMae.EMLite.DataAccess.SQL.Decode(r["Salutation"], (object) "").ToString(), (Guid) EllieMae.EMLite.DataAccess.SQL.Decode(r["Guid"], (object) Guid.NewGuid()));
    }

    public static void RenameStatus(string oldStatus, string newStatus)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("Status", (object) newStatus);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable("Borrower"), values, new DbValue("Status", (object) oldStatus));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
      }
    }

    public static DataTable GetUnSyncBorrowerContacts(string loanFolder)
    {
      string text = "Select LoanBorrowers.ID, LoanBorrowers.Guid, LoanBorrowers.PairIndex, LoanBorrowers.BorrowerType from LoanBorrowers where LoanBorrowers.ID not in (Select A.ID from LoanBorrowers A inner join LoanSummary B on A.Guid = B.Guid inner join (select contactGuid, loanRefId, RoleType+1 as RoleType, pairIndex+1 as PairIndex from BorrowerLoans) C on C.loanrefid = B.xrefId where A.pairIndex = C.PairIndex and A.borrowertype = C.roleType) and LoanBorrowers.ID in (Select D.ID from LoanBorrowers D inner join LoanSummary E on D.Guid = E.Guid where E.LoanFolder = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(loanFolder) + ") Order by LoanBorrowers.Guid";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append(text);
      return dbQueryBuilder.ExecuteTableQuery();
    }

    public static string[] GetCRMLoanGuids(int contactID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select distinct A.loanRefId from BorrowerLoans A inner join Borrower B on A.contactGuid = B.Guid where B.ContactID = " + (object) contactID);
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable == null || dataTable.Rows.Count <= 0)
          return (string[]) null;
        List<string> stringList = new List<string>();
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
          stringList.Add(string.Concat(row[0]));
        return stringList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BorrowerContact), ex);
        return (string[]) null;
      }
    }

    public static ContactLoanPair[] GetRelatedLoansForBorrower(int contactId)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BorrowerLoans.*, Borrower.ContactID, Loan.Guid as LoanGuid from BorrowerLoans");
      dbQueryBuilder.AppendLine("  inner join Borrower on Borrower.Guid = BorrowerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BorrowerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where Borrower.ContactID = " + (object) contactId);
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BorrowerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair[] GetRelatedLoansForBorrower(string contactGuid)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BorrowerLoans.*, Borrower.ContactID, Loan.Guid as LoanGuid from BorrowerLoans");
      dbQueryBuilder.AppendLine("  inner join Borrower on BizPartner.Guid = BorrowerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BorrowerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where Borrower.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) contactGuid));
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BorrowerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair[] GetBorrowersForLoan(string loanGuid)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BorrowerLoans.*, Borrower.ContactID, Loan.Guid as LoanGuid from BorrowerLoans");
      dbQueryBuilder.AppendLine("  inner join Borrower on BizPartner.Guid = BorrowerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BorrowerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BorrowerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair dataRowToContactLoanPair(DataRow r)
    {
      return new ContactLoanPair(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), string.Concat(r["ContactGuid"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContactID"]), ContactType.BizPartner, (CRMRoleType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["RoleType"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["PairIndex"]));
    }
  }
}
