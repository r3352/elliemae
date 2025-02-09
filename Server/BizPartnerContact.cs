// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BizPartnerContact
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
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
  public class BizPartnerContact : EllieMae.EMLite.Server.Contact
  {
    private const string className = "BizPartnerContact�";
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
    private BizPartnerInfo info;

    public BizPartnerContact(ICacheLock<bool?> innerLock)
      : base(innerLock)
    {
    }

    public BizPartnerContact(ContactStore.ContactIdentity id)
      : base(id)
    {
    }

    public override string FullName => this.GetContactInfo().FullName;

    protected override string ContactTable => "BizPartner";

    protected override string HistoryTable => "BizPartnerHistory";

    protected override string NotesTable => "BizPartnerNotes";

    protected override string CustomFieldTable => "BizCustomField";

    protected override string CampaignActivityTable => "PartnerCampaignActivity";

    public BizPartnerInfo GetContactInfo()
    {
      this.validateExists();
      if (this.info == null)
        this.info = this.getContactInfoFromDatabase();
      return this.info;
    }

    public void CheckIn(BizPartnerInfo info) => this.CheckIn(info, false);

    public void CheckIn(BizPartnerInfo info, bool keepCheckedOut)
    {
      this.validateInstance();
      this.saveContactInfoToDatabase(info);
      this.info = info;
    }

    public static BizPartnerInfo[] GetBizPartnersByOwner(string ownerId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from BizPartner where OwnerID = '" + ownerId + "'");
        return BizPartnerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo[]) null;
      }
    }

    public static void MakeBizPartnersPublic(int[] contactIds)
    {
      try
      {
        List<string> stringList = new List<string>();
        foreach (int contactId in contactIds)
          stringList.Add(string.Concat((object) contactId));
        string str = string.Join(",", stringList.ToArray());
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("Update BizPartner set OwnerID = Null, IsPublic = 1 where ContactID in (" + str + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
      }
    }

    public static int GetBizPartnerIDFromGuid(string bizPartnerGuid)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select distinct ContactID from BizPartner where Guid = " + EllieMae.EMLite.DataAccess.SQL.EncodeString(bizPartnerGuid));
        object obj = dbQueryBuilder.ExecuteScalar();
        return obj != null ? (int) obj : -1;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return -1;
      }
    }

    public static string getQueryBizPartnerIdsSql(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BizPartnerContact.generateQuerySql("Contact.ContactID", user, criteria, matchType, sortOrder).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (string) null;
      }
    }

    public static string getQueryBizPartnerSummariesSql(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BizPartnerContact.generateQuerySql("Contact.ContactID, Contact.AccessLevel, Contact.CategoryID, Contact.FirstName, Contact.LastName, Contact.CompanyName, Contact.WorkPhone, Contact.BizEmail, Contact.NoSpam, Contact.LastModified, BizCategory.CategoryName", user, criteria, matchType, sortOrder).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (string) null;
      }
    }

    public static string getQueryBizPartnerIdSql(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BizPartnerContact.generateQuerySql("Contact.ContactID, Contact.AccessLevel, Contact.CategoryID, Contact.FirstName, Contact.LastName, Contact.CompanyName, Contact.WorkPhone, Contact.BizEmail, Contact.NoSpam, Contact.LastModified, BizCategory.CategoryName", user, criteria, matchType, sortOrder).ToString();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (string) null;
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
        fields = BizPartnerContact.DefaultSummaryFields;
      QueryCriterion filter = (QueryCriterion) null;
      if (criteria != null)
      {
        foreach (QueryCriterion criterion in criteria)
          filter = filter != null ? filter.And(criterion) : criterion;
      }
      DataQuery query = new DataQuery((IEnumerable) fields, filter);
      if (sortFields != null)
        query.SortFields.AddRange((IEnumerable<SortField>) sortFields);
      return new BizPartnerQuery(user, matchType).GetSqlQueryForDataQuery(query, false);
    }

    public static QueryResult GenerateQueryResult(
      string[] fieldList,
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortFields)
    {
      return BizPartnerContact.generateQueryResult(fieldList == null || fieldList.Length == 0 ? "*" : string.Join(", ", fieldList), user, criteria, matchType, sortFields);
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
        fields = BizPartnerContact.DefaultSummaryFields;
      QueryCriterion filter = (QueryCriterion) null;
      if (criteria != null)
      {
        foreach (QueryCriterion criterion in criteria)
          filter = filter != null ? filter.And(criterion) : criterion;
      }
      DataQuery query = new DataQuery((IEnumerable) fields, filter);
      if (sortFields != null)
        query.SortFields.AddRange((IEnumerable<SortField>) sortFields);
      return new BizPartnerQuery(user, matchType).Execute(query, false);
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
      BizPartnerQuery bizPartnerQuery = new BizPartnerQuery(user, matchType);
      querySql.Append(bizPartnerQuery.GetTableSelectionClause(fields, filter, sortFields, true, true, false));
      querySql.Append(bizPartnerQuery.GetOrderByClause(sortFields));
      return querySql;
    }

    public static int[] GetPublicBizContactWithoutGroupIDList()
    {
      int[] withoutGroupIdList = (int[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select distinct ContactID from BizPartner where IsPublic = 1 AND  contactID not in (select distinct contactID from ContactGroupPartners CGP inner join ContactGroup CG on CGP.groupID = CG.groupID where CG.contactType = 2)");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        withoutGroupIdList = new int[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          withoutGroupIdList[index] = int.Parse(string.Concat(dataRowCollection[index]["contactID"]));
      }
      return withoutGroupIdList;
    }

    private static string GetUserGroupGrantedContactGroupIDList(string userid, bool editOnly)
    {
      ArrayList arrayList = new ArrayList();
      foreach (AclGroup aclGroup in AclGroupAccessor.GetGroupsOfUser(userid))
      {
        BizGroupRef[] contactGroupRefs = AclGroupBizGroupAccessor.GetBizContactGroupRefs(aclGroup.ID);
        if (contactGroupRefs != null)
        {
          foreach (BizGroupRef bizGroupRef in contactGroupRefs)
          {
            if ((!editOnly || bizGroupRef.Access != AclResourceAccess.ReadOnly) && !arrayList.Contains((object) string.Concat((object) bizGroupRef.BizGroupID)))
              arrayList.Add((object) string.Concat((object) bizGroupRef.BizGroupID));
          }
        }
      }
      string[] array = (string[]) arrayList.ToArray(typeof (string));
      return array != null && array.Length != 0 ? string.Join(",", array) : "";
    }

    public static int[] QueryBizPartnerIds(
      UserInfo userInfo,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType)
    {
      return BizPartnerContact.QueryBizPartnerIds(userInfo, criteria, matchType, new SortField[1]
      {
        new SortField("Contact.ContactID", FieldSortOrder.Ascending)
      });
    }

    public static int[] QueryBizPartnerIds(
      UserInfo userInfo,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      StringBuilder stringBuilder = new StringBuilder(BizPartnerContact.getQueryBizPartnerIdsSql(userInfo, criteria, matchType, sortOrder));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(stringBuilder.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["ContactID"]);
      return numArray;
    }

    public static int QueryBizPartnerMortgageClause(int categoryID, string mortgageClauseCompany)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @ID INT");
      dbQueryBuilder.AppendLine("SELECT TOP 1 @ID = CustomFieldValue.ContactId FROM BizCategoryCustomFieldValue AS CustomFieldValue");
      dbQueryBuilder.AppendLine("       Left Join BizCategoryCustomFieldDefinition AS CustomFieldDefinition");
      dbQueryBuilder.AppendLine("       ON CustomFieldDefinition.FieldId = CustomFieldValue.FieldId");
      dbQueryBuilder.AppendLine("       WHERE CustomFieldDefinition.FieldDescription = 'Company Name'");
      dbQueryBuilder.AppendLine("       AND CustomFieldValue.FieldValue = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) mortgageClauseCompany));
      dbQueryBuilder.AppendLine("       AND CustomFieldValue.CategoryId = " + (object) categoryID);
      dbQueryBuilder.AppendLine("SELECT @ID");
      try
      {
        object obj = dbQueryBuilder.ExecuteScalar();
        if (obj != null)
        {
          if (obj is int)
            return Utils.ParseInt(obj);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return -1;
      }
      return -1;
    }

    public static BizPartnerSummaryInfo[] QueryBizPartnerSummaries(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        string fieldList = "Contact.ContactID, Contact.AccessLevel, Contact.CategoryID, Contact.FirstName, Contact.LastName, Contact.CompanyName, Contact.WorkPhone, Contact.BizEmail, Contact.NoSpam, Contact.LastModified, BizCategory.CategoryName";
        if (sortOrder != null)
        {
          foreach (SortField sortField in sortOrder)
          {
            if (fieldList.IndexOf(sortField.Term.FieldName) < 0)
              fieldList = fieldList + ", " + sortField.Term.FieldName;
          }
        }
        return BizPartnerContact.executeListSummaryQuery(BizPartnerContact.generateQueryResult(fieldList, user, criteria, matchType, sortOrder).ToDataTable());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public static BizPartnerSummaryInfo[] QueryPublicBizPartnerSummaries(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BizPartnerContact.executeListSummaryQuery(BizPartnerContact.generateQueryResult("Contact.ContactID, Contact.AccessLevel, Contact.CategoryID, Contact.FirstName, Contact.LastName, Contact.CompanyName, Contact.WorkPhone, Contact.BizEmail, Contact.NoSpam, Contact.LastModified, BizCategory.CategoryName", user, criteria, matchType, sortOrder).ToDataTable());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public static BizPartnerSummaryInfo[] GetBizPartnerSummaries(
      UserInfo userInfo,
      int[] contactIds,
      string[] fields)
    {
      try
      {
        if (fields == null || fields.Length == 0)
          fields = new string[22]
          {
            "Contact.FirstName",
            "Contact.LastName",
            "Contact.HomePhone",
            "Contact.WorkPhone",
            "Contact.MobilePhone",
            "Contact.FaxNumber",
            "Contact.PersonalEmail",
            "Contact.BizEmail",
            "Contact.NoSpam",
            "Contact.CategoryID",
            "Contact.CompanyName",
            "Contact.JobTitle",
            "Contact.LicenseNumber",
            "Contact.Fees",
            "Contact.BizAddress1",
            "Contact.BizAddress2",
            "Contact.BizCity",
            "Contact.BizState",
            "Contact.BizZip",
            "Contact.BizWebUrl",
            "Contact.Salutation",
            "Contact.LastModified"
          };
        string fieldList = string.Join(", ", fields);
        ListValueCriterion listValueCriterion = new ListValueCriterion("Contact.ContactID", (Array) contactIds);
        BizPartnerSummaryInfo[] partnerSummaryInfoArray = BizPartnerContact.executeListSummaryQuery(BizPartnerContact.generateQueryResult(fieldList, userInfo, new QueryCriterion[1]
        {
          (QueryCriterion) listValueCriterion
        }, RelatedLoanMatchType.None, new SortField[0]).ToDataTable());
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < partnerSummaryInfoArray.Length; ++index)
          hashtable.Add((object) partnerSummaryInfoArray[index].ContactID, (object) partnerSummaryInfoArray[index]);
        BizPartnerSummaryInfo[] partnerSummaries = new BizPartnerSummaryInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          partnerSummaries[index] = (BizPartnerSummaryInfo) hashtable[(object) contactIds[index]];
        return partnerSummaries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public static BizPartnerSummaryInfo[] GetBizPartnerSummaries(
      UserInfo userInfo,
      int[] contactIds,
      string[] fields,
      int maxContactsPerQuery = 1000)
    {
      string fieldList = string.Join(", ", fields);
      List<BizPartnerSummaryInfo> partnerSummaryInfoList = new List<BizPartnerSummaryInfo>();
      try
      {
        if (fields == null || fields.Length == 0)
          fields = new string[22]
          {
            "Contact.FirstName",
            "Contact.LastName",
            "Contact.HomePhone",
            "Contact.WorkPhone",
            "Contact.MobilePhone",
            "Contact.FaxNumber",
            "Contact.PersonalEmail",
            "Contact.BizEmail",
            "Contact.NoSpam",
            "Contact.CategoryID",
            "Contact.CompanyName",
            "Contact.JobTitle",
            "Contact.LicenseNumber",
            "Contact.Fees",
            "Contact.BizAddress1",
            "Contact.BizAddress2",
            "Contact.BizCity",
            "Contact.BizState",
            "Contact.BizZip",
            "Contact.BizWebUrl",
            "Contact.Salutation",
            "Contact.LastModified"
          };
        for (int sourceIndex = 0; sourceIndex < contactIds.Length; sourceIndex += maxContactsPerQuery)
        {
          int length = Math.Min(maxContactsPerQuery, contactIds.Length - sourceIndex);
          int[] numArray = new int[length];
          Array.Copy((Array) contactIds, sourceIndex, (Array) numArray, 0, length);
          ListValueCriterion listValueCriterion = new ListValueCriterion("Contact.ContactID", (Array) numArray);
          QueryResult queryResult = BizPartnerContact.generateQueryResult(fieldList, userInfo, new QueryCriterion[1]
          {
            (QueryCriterion) listValueCriterion
          }, RelatedLoanMatchType.None, new SortField[0]);
          partnerSummaryInfoList.AddRange((IEnumerable<BizPartnerSummaryInfo>) BizPartnerContact.executeListSummaryQuery(queryResult.ToDataTable()));
        }
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < partnerSummaryInfoList.Count; ++index)
          hashtable.Add((object) partnerSummaryInfoList[index].ContactID, (object) partnerSummaryInfoList[index]);
        BizPartnerSummaryInfo[] partnerSummaries = new BizPartnerSummaryInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          partnerSummaries[index] = (BizPartnerSummaryInfo) hashtable[(object) contactIds[index]];
        return partnerSummaries;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerSummaryInfo[]) null;
      }
    }

    public static BizPartnerInfo[] QueryBizPartners(
      UserInfo user,
      QueryCriterion[] criteria,
      RelatedLoanMatchType matchType,
      SortField[] sortOrder)
    {
      try
      {
        return BizPartnerContact.executeListQuery(BizPartnerContact.generateQuerySql("Contact.*", user, criteria, matchType, sortOrder).ExecuteTableQuery());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo[]) null;
      }
    }

    public static BizPartnerInfo[] QueryBizPartners(
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
        BizPartnerQuery bizPartnerQuery = new BizPartnerQuery(user, matchType);
        string fieldSelectionList = bizPartnerQuery.GetFieldSelectionList(fields);
        dbQueryBuilder.AppendLine("select " + fieldSelectionList);
        dbQueryBuilder.Append(bizPartnerQuery.GetTableSelectionClause(fields, filter, sortOrder, true, true, false));
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
        BizPartnerInfo[] source = BizPartnerContact.executeFieldListQuery(paginatedRecords);
        totalRecords = ((IEnumerable<BizPartnerInfo>) source).Any<BizPartnerInfo>() ? Convert.ToInt32(paginatedRecords.Rows[0]["TotalRowCount"]) : 0;
        return source;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo[]) null;
      }
    }

    public static BizPartnerInfo[] GetBizPartners(int[] contactIds)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < contactIds.Length; ++index)
          stringBuilder.Append((index > 0 ? "," : "") + contactIds[index].ToString());
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select Contact.*");
        dbQueryBuilder.AppendLine("from   BizPartner Contact");
        dbQueryBuilder.AppendLine("where  Contact.ContactID in (" + stringBuilder.ToString() + ")");
        BizPartnerInfo[] bizPartnerInfoArray = BizPartnerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery());
        Hashtable hashtable = new Hashtable(contactIds.Length);
        for (int index = 0; index < bizPartnerInfoArray.Length; ++index)
          hashtable.Add((object) bizPartnerInfoArray[index].ContactID, (object) bizPartnerInfoArray[index]);
        BizPartnerInfo[] bizPartners = new BizPartnerInfo[contactIds.Length];
        for (int index = 0; index < contactIds.Length; ++index)
          bizPartners[index] = (BizPartnerInfo) hashtable[(object) contactIds[index]];
        return bizPartners;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo[]) null;
      }
    }

    public static BizPartnerInfo[] GetBizPartners(string[] contactGuids)
    {
      try
      {
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < contactGuids.Length; ++index)
          stringBuilder.Append((index > 0 ? "," : "") + EllieMae.EMLite.DataAccess.SQL.EncodeString(contactGuids[index]));
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("select Contact.*");
        dbQueryBuilder.AppendLine("from   BizPartner Contact");
        dbQueryBuilder.AppendLine("where  Contact.Guid in (" + stringBuilder.ToString() + ")");
        return BizPartnerContact.executeListQuery(dbQueryBuilder.ExecuteTableQuery());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo[]) null;
      }
    }

    public static int CreateNew(
      BizPartnerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Declare("@contactId", "int");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BizPartner"), BizPartnerContact.createDbValueList(info), true, false);
        dbQueryBuilder.SelectIdentity("@contactId");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BizPartnerHistory"), new DbValueList()
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
        Err.Reraise(nameof (BizPartnerContact), ex);
        return -1;
      }
    }

    public static int CreateNew(
      BizPartnerInfo info,
      DateTime firstContactDate,
      ContactSource contactSource,
      CustomFieldsValueInfo categoryInfo)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Begin();
        dbQueryBuilder.Declare("@contactId", "int");
        dbQueryBuilder.Declare("@errorMsg", "nvarchar(4000)");
        dbQueryBuilder.Append("BEGIN TRY");
        dbQueryBuilder.Append(" BEGIN TRANSACTION ");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BizPartner"), BizPartnerContact.createDbValueList(info), true, false);
        dbQueryBuilder.SelectIdentity("@contactId");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("BizPartnerHistory"), new DbValueList()
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
        if (categoryInfo != null && categoryInfo.CustomFieldValues != null && categoryInfo.CustomFieldValues.Length != 0)
        {
          dbQueryBuilder.If("@contactId is not null");
          dbQueryBuilder.Begin();
          foreach (CustomFieldValueInfo customFieldValue in categoryInfo.CustomFieldValues)
          {
            if (customFieldValue.IsNew && !customFieldValue.IsDeleted)
              dbQueryBuilder.AppendLine("INSERT INTO BizCategoryCustomFieldValue VALUES (@contactId, " + (object) customFieldValue.FieldId + ", " + (object) categoryInfo.CategoryId + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldValue.FieldValue) + ") ");
          }
          dbQueryBuilder.End();
        }
        dbQueryBuilder.Append("COMMIT ");
        dbQueryBuilder.Append("END TRY ");
        dbQueryBuilder.Append("BEGIN CATCH ");
        dbQueryBuilder.Append("if @@TRANCOUNT > 0 ROLLBACK TRAN ");
        dbQueryBuilder.Append("SET @errorMsg = ERROR_MESSAGE() ");
        dbQueryBuilder.Append("RAISERROR (@errorMsg, 15, 1) ");
        dbQueryBuilder.Append("END CATCH ");
        dbQueryBuilder.End();
        return (int) dbQueryBuilder.ExecuteScalar();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return -1;
      }
    }

    public static void UpdateBizCategory(BizCategory cat)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("CategoryName", (object) cat.Name);
        DbValue key = new DbValue("CategoryID", (object) cat.CategoryID);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("BizCategory");
        dbQueryBuilder.Append("if exists (select 1 from BizCategory where CategoryName like '" + EllieMae.EMLite.DataAccess.SQL.Escape(table["CategoryName"].SizeToFit(cat.Name)) + "' and CategoryID <> " + (object) cat.CategoryID + ")");
        dbQueryBuilder.RaiseError("Category already exists with this name");
        dbQueryBuilder.Update(table, values, key);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
      }
    }

    public static void DeleteBizCategory(BizCategory cat)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Declare("@noCatId", "int");
        dbQueryBuilder.AppendLine("select @noCatId = CategoryID from BizCategory where CategoryName = 'No Category'");
        dbQueryBuilder.AppendLine("update BizPartner set CategoryID = @noCatId where CategoryID = " + (object) cat.CategoryID);
        dbQueryBuilder.AppendLine("delete from BizCategory where CategoryID = " + (object) cat.CategoryID);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
      }
    }

    public static BizCategory AddBizCategory(string categoryName)
    {
      try
      {
        DbValueList values = new DbValueList();
        values.Add("CategoryName", (object) categoryName);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("BizCategory");
        dbQueryBuilder.Declare("@categoryId", "int");
        dbQueryBuilder.SelectVar("@categoryId", (object) ("CategoryID from BizCategory where CategoryName like '" + EllieMae.EMLite.DataAccess.SQL.Escape(table["CategoryName"].SizeToFit(categoryName)) + "'"), (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.If("@categoryId is NULL");
        dbQueryBuilder.Begin();
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.SelectIdentity("@categoryId");
        dbQueryBuilder.End();
        dbQueryBuilder.SelectFrom(table, new DbValue("CategoryID", (object) "@categoryId", (IDbEncoder) DbEncoding.None));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Error, nameof (BizPartnerContact), (ServerException) new ServerDataException("Error addiing business category: no data returned from query"));
        return new BizCategory((int) dataRowCollection[0]["CategoryID"], dataRowCollection[0]["CategoryName"].ToString());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizCategory) null;
      }
    }

    public static BizCategory[] GetBizCategories()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable("BizCategory"));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        BizCategory[] bizCategories = new BizCategory[dataRowCollection.Count];
        for (int index = 0; index < dataRowCollection.Count; ++index)
          bizCategories[index] = BizPartnerContact.dataRowToBizCategory(dataRowCollection[index]);
        return bizCategories;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizCategory[]) null;
      }
    }

    public static CustomFieldsDefinitionInfo[] GetCustomFieldsDefinitions(
      CustomFieldsType customFieldsType)
    {
      if (CustomFieldsType.BizCategoryStandard != customFieldsType && CustomFieldsType.BizCategoryCustom != customFieldsType && (CustomFieldsType.BizCategoryCustom | CustomFieldsType.BizCategoryStandard) != customFieldsType)
        return (CustomFieldsDefinitionInfo[]) null;
      string str = string.Empty;
      if (CustomFieldsType.BizCategoryStandard == customFieldsType || CustomFieldsType.BizCategoryCustom == customFieldsType)
        str = CustomFieldsType.BizCategoryStandard != customFieldsType ? " WHERE f.FieldNumber > 0" : " WHERE f.FieldNumber < 0";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT f.CategoryId FROM BizCategoryCustomFieldDefinition f" + str + " ORDER BY f.CategoryId");
      dbQueryBuilder.AppendLine("SELECT f.* FROM BizCategoryCustomFieldDefinition f" + str + " ORDER BY f.CategoryId, f.FieldNumber");
      dbQueryBuilder.AppendLine("SELECT o.* FROM BizCategoryCustomFieldDefinition f INNER JOIN BizCategoryCustomFieldOption o ON f.FieldId = o.FieldId" + str + " ORDER BY o.FieldId, o.OptionNumber");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      CustomFieldsDefinitionInfo[] fieldsDefinitions = new CustomFieldsDefinitionInfo[count];
      if (count == 0)
        return fieldsDefinitions;
      dataSet.Relations.Add("Fields", dataSet.Tables[0].Columns["CategoryId"], dataSet.Tables[1].Columns["CategoryId"]);
      dataSet.Relations.Add("Options", dataSet.Tables[1].Columns["FieldId"], dataSet.Tables[2].Columns["FieldId"]);
      for (int index1 = 0; index1 < count; ++index1)
      {
        DataRow row = dataSet.Tables[0].Rows[index1];
        fieldsDefinitions[index1] = BizPartnerContact.convertRowToCustomFieldsDefinition(customFieldsType, row);
        DataRow[] childRows1 = row.GetChildRows("Fields");
        int length1 = childRows1.Length;
        if (length1 != 0)
        {
          CustomFieldDefinitionInfo[] fieldDefinitionInfoArray = new CustomFieldDefinitionInfo[length1];
          fieldsDefinitions[index1].CustomFieldDefinitions = fieldDefinitionInfoArray;
          for (int index2 = 0; index2 < length1; ++index2)
          {
            DataRow fieldRow = childRows1[index2];
            fieldDefinitionInfoArray[index2] = BizPartnerContact.convertRowToCustomFieldDefinition(fieldRow);
            DataRow[] childRows2 = fieldRow.GetChildRows("Options");
            int length2 = childRows2.Length;
            if (length2 != 0)
            {
              CustomFieldOptionDefinitionInfo[] optionDefinitionInfoArray = new CustomFieldOptionDefinitionInfo[length2];
              fieldDefinitionInfoArray[index2].CustomFieldOptionDefinitions = optionDefinitionInfoArray;
              for (int index3 = 0; index3 < length2; ++index3)
              {
                DataRow optionRow = childRows2[index3];
                optionDefinitionInfoArray[index3] = BizPartnerContact.convertRowToCustomFieldOptionDefinition(optionRow);
              }
            }
          }
        }
      }
      return fieldsDefinitions;
    }

    public static CustomFieldsDefinitionInfo GetCustomFieldsDefinition(
      CustomFieldsType customFieldsType,
      int recordId)
    {
      if (CustomFieldsType.BizCategoryStandard != customFieldsType && CustomFieldsType.BizCategoryCustom != customFieldsType && (CustomFieldsType.BizCategoryCustom | CustomFieldsType.BizCategoryStandard) != customFieldsType || 0 > recordId)
        return (CustomFieldsDefinitionInfo) null;
      StringBuilder stringBuilder = new StringBuilder();
      if (CustomFieldsType.BizCategoryStandard == customFieldsType || CustomFieldsType.BizCategoryCustom == customFieldsType)
      {
        stringBuilder.Append(" AND f.FieldNumber ");
        if (CustomFieldsType.BizCategoryStandard == customFieldsType)
          stringBuilder.Append("<");
        else
          stringBuilder.Append(">");
        stringBuilder.Append(" 0");
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@CategoryId", "INT");
      dbQueryBuilder.SelectVar("@CategoryId", (object) recordId);
      dbQueryBuilder.AppendLine("SELECT f.* FROM BizCategoryCustomFieldDefinition f WHERE f.CategoryId = @CategoryId" + (object) stringBuilder + " ORDER BY f.FieldNumber");
      dbQueryBuilder.AppendLine("SELECT o.* FROM BizCategoryCustomFieldDefinition f INNER JOIN BizCategoryCustomFieldOption o ON f.FieldId = o.FieldId WHERE f.CategoryId = @CategoryId" + (object) stringBuilder + " ORDER BY o.OptionNumber");
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      int count = dataSet.Tables[0].Rows.Count;
      if (count == 0)
        return (CustomFieldsDefinitionInfo) null;
      CustomFieldDefinitionInfo[] customFieldDefinitions = new CustomFieldDefinitionInfo[count];
      CustomFieldsDefinitionInfo fieldsDefinition = new CustomFieldsDefinitionInfo(customFieldsType, recordId, customFieldDefinitions);
      dataSet.Relations.Add("Options", dataSet.Tables[0].Columns["FieldId"], dataSet.Tables[1].Columns["FieldId"]);
      for (int index1 = 0; index1 < count; ++index1)
      {
        DataRow row = dataSet.Tables[0].Rows[index1];
        customFieldDefinitions[index1] = BizPartnerContact.convertRowToCustomFieldDefinition(row);
        DataRow[] childRows = row.GetChildRows("Options");
        int length = childRows.Length;
        if (length != 0)
        {
          CustomFieldOptionDefinitionInfo[] optionDefinitionInfoArray = new CustomFieldOptionDefinitionInfo[length];
          customFieldDefinitions[index1].CustomFieldOptionDefinitions = optionDefinitionInfoArray;
          for (int index2 = 0; index2 < length; ++index2)
          {
            DataRow optionRow = childRows[index2];
            optionDefinitionInfoArray[index2] = BizPartnerContact.convertRowToCustomFieldOptionDefinition(optionRow);
          }
        }
      }
      return fieldsDefinition;
    }

    public static CustomFieldsDefinitionInfo UpdateCustomFieldsDefinition(
      CustomFieldsDefinitionInfo customFieldsDefinitionInfo)
    {
      if (customFieldsDefinitionInfo == null || customFieldsDefinitionInfo.CustomFieldsType == CustomFieldsType.None || 0 > customFieldsDefinitionInfo.RecordId)
        return customFieldsDefinitionInfo;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      ArrayList arrayList = new ArrayList();
      foreach (CustomFieldDefinitionInfo customFieldDefinition in customFieldsDefinitionInfo.CustomFieldDefinitions)
      {
        if (customFieldDefinition.IsDeleted && !customFieldDefinition.IsNew)
          arrayList.Add((object) customFieldDefinition.FieldId);
      }
      if (0 < arrayList.Count)
        dbQueryBuilder.AppendLine("DELETE FROM BizCategoryCustomFieldDefinition WHERE FieldId IN " + BizPartnerContact.encodeIntArray(arrayList.ToArray(typeof (int))));
      dbQueryBuilder.Declare("@FieldId", "INT");
      foreach (CustomFieldDefinitionInfo customFieldDefinition in customFieldsDefinitionInfo.CustomFieldDefinitions)
      {
        if (customFieldDefinition.IsNew && !customFieldDefinition.IsDeleted)
        {
          dbQueryBuilder.AppendLine("INSERT INTO BizCategoryCustomFieldDefinition VALUES (" + (object) customFieldDefinition.CategoryId + ", " + (object) customFieldDefinition.FieldNumber + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldDefinition.FieldDescription) + ", " + (object) (int) customFieldDefinition.FieldFormat + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldDefinition.LoanFieldId) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(customFieldDefinition.TwoWayTransfer) + ")");
          dbQueryBuilder.SelectIdentity("@FieldId");
          if (FieldFormat.DROPDOWN == customFieldDefinition.FieldFormat || FieldFormat.DROPDOWNLIST == customFieldDefinition.FieldFormat)
          {
            foreach (CustomFieldOptionDefinitionInfo optionDefinition in customFieldDefinition.CustomFieldOptionDefinitions)
              dbQueryBuilder.AppendLine("INSERT INTO BizCategoryCustomFieldOption VALUES (@FieldId, " + (object) optionDefinition.OptionNumber + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) optionDefinition.OptionValue) + ")");
          }
        }
      }
      foreach (CustomFieldDefinitionInfo customFieldDefinition in customFieldsDefinitionInfo.CustomFieldDefinitions)
      {
        if (customFieldDefinition.IsDirty && !customFieldDefinition.IsDeleted && !customFieldDefinition.IsNew)
        {
          dbQueryBuilder.AppendLine("UPDATE BizCategoryCustomFieldDefinition SET FieldDescription = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldDefinition.FieldDescription) + ", FieldFormat = " + (object) (int) customFieldDefinition.FieldFormat + ", LoanFieldId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldDefinition.LoanFieldId) + ", TwoWayTransfer = " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(customFieldDefinition.TwoWayTransfer) + " WHERE FieldId = " + (object) customFieldDefinition.FieldId);
          dbQueryBuilder.AppendLine("DELETE FROM BizCategoryCustomFieldOption WHERE FieldId = " + (object) customFieldDefinition.FieldId);
          if (FieldFormat.DROPDOWN == customFieldDefinition.FieldFormat || FieldFormat.DROPDOWNLIST == customFieldDefinition.FieldFormat)
          {
            foreach (CustomFieldOptionDefinitionInfo optionDefinition in customFieldDefinition.CustomFieldOptionDefinitions)
              dbQueryBuilder.AppendLine("INSERT INTO BizCategoryCustomFieldOption VALUES (" + (object) customFieldDefinition.FieldId + ", " + (object) optionDefinition.OptionNumber + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) optionDefinition.OptionValue) + ")");
          }
        }
      }
      dbQueryBuilder.ExecuteNonQuery();
      return BizPartnerContact.GetCustomFieldsDefinition(customFieldsDefinitionInfo.CustomFieldsType, customFieldsDefinitionInfo.RecordId);
    }

    public static CustomFieldsValueInfo GetCustomFieldsValues(int contactId, int categoryId)
    {
      CustomFieldsValueInfo customFieldsValues = new CustomFieldsValueInfo(contactId, categoryId, new CustomFieldValueInfo[0]);
      if (0 >= contactId || 0 > categoryId)
        return customFieldsValues;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.*, f.FieldFormat FROM BizCategoryCustomFieldValue v INNER JOIN BizCategoryCustomFieldDefinition f ON v.FieldId = f.FieldId WHERE v.ContactId = " + (object) contactId + " AND v.CategoryId = " + (object) categoryId + " ORDER BY v.FieldId");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return customFieldsValues;
      customFieldsValues.CustomFieldValues = new CustomFieldValueInfo[count];
      for (int index = 0; index < count; ++index)
        customFieldsValues.CustomFieldValues[index] = BizPartnerContact.convertRowToCustomFieldValue(dataRowCollection[index]);
      return customFieldsValues;
    }

    public static Dictionary<string, IEnumerable<CustomFieldValueInfo>> GetCustomFieldValueInfosForBizContactIds(
      int[] contactIds)
    {
      Dictionary<string, IEnumerable<CustomFieldValueInfo>> forBizContactIds = new Dictionary<string, IEnumerable<CustomFieldValueInfo>>();
      if (contactIds.Length == 0)
        return forBizContactIds;
      StringBuilder stringBuilder = new StringBuilder(string.Join<int>(",", (IEnumerable<int>) contactIds));
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT v.*, f.FieldFormat FROM BizCategoryCustomFieldValue v INNER JOIN BizCategoryCustomFieldDefinition f ON v.FieldId = f.FieldId");
      dbQueryBuilder.AppendLine(" WHERE v.ContactId in (" + (object) stringBuilder + ")");
      dbQueryBuilder.AppendLine(" ORDER BY v.ContactId, v.CategoryId, v.FieldId");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return forBizContactIds;
      int num1 = -1;
      int num2 = -1;
      List<CustomFieldValueInfo> customFieldValueInfoList = (List<CustomFieldValueInfo>) null;
      for (int index = 0; index < count; ++index)
      {
        DataRow valueRow = dataRowCollection[index];
        if (num1 != (int) valueRow["ContactId"] || num2 != (int) valueRow["CategoryId"])
        {
          num1 = (int) valueRow["ContactId"];
          num2 = (int) valueRow["CategoryId"];
          customFieldValueInfoList = new List<CustomFieldValueInfo>();
          forBizContactIds.Add(num1.ToString() + "|" + (object) num2, (IEnumerable<CustomFieldValueInfo>) customFieldValueInfoList);
        }
        customFieldValueInfoList.Add(BizPartnerContact.convertRowToCustomFieldValue(valueRow));
      }
      return forBizContactIds;
    }

    public static CustomFieldsValueInfo UpdateCustomFieldsValues(
      CustomFieldsValueInfo customFieldsValueInfo)
    {
      if (customFieldsValueInfo == null || 0 >= customFieldsValueInfo.ContactId || 0 > customFieldsValueInfo.CategoryId || customFieldsValueInfo.CustomFieldValues.Length == 0)
        return customFieldsValueInfo;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      ArrayList arrayList = new ArrayList();
      foreach (CustomFieldValueInfo customFieldValue in customFieldsValueInfo.CustomFieldValues)
      {
        if (customFieldValue.IsDeleted && !customFieldValue.IsNew)
          arrayList.Add((object) customFieldValue.FieldId);
      }
      if (0 < arrayList.Count)
        dbQueryBuilder.AppendLine("DELETE FROM BizCategoryCustomFieldValue WHERE ContactId = " + (object) customFieldsValueInfo.ContactId + " AND FieldId IN " + BizPartnerContact.encodeIntArray(arrayList.ToArray(typeof (int))));
      foreach (CustomFieldValueInfo customFieldValue in customFieldsValueInfo.CustomFieldValues)
      {
        if (customFieldValue.IsNew && !customFieldValue.IsDeleted)
        {
          dbQueryBuilder.AppendLine("DELETE FROM BizCategoryCustomFieldValue WHERE ContactId = " + (object) customFieldValue.ContactId + " AND FieldId = " + (object) customFieldValue.FieldId + " AND CategoryID = " + (object) customFieldsValueInfo.CategoryId);
          dbQueryBuilder.AppendLine("INSERT INTO BizCategoryCustomFieldValue VALUES (" + (object) customFieldValue.ContactId + ", " + (object) customFieldValue.FieldId + ", " + (object) customFieldsValueInfo.CategoryId + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldValue.FieldValue) + ")");
        }
      }
      foreach (CustomFieldValueInfo customFieldValue in customFieldsValueInfo.CustomFieldValues)
      {
        if (customFieldValue.IsDirty && !customFieldValue.IsDeleted && !customFieldValue.IsNew)
          dbQueryBuilder.AppendLine("UPDATE BizCategoryCustomFieldValue SET FieldValue = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) customFieldValue.FieldValue) + " WHERE ContactId = " + (object) customFieldValue.ContactId + " AND FieldId = " + (object) customFieldValue.FieldId);
      }
      dbQueryBuilder.ExecuteNonQuery();
      return BizPartnerContact.GetCustomFieldsValues(customFieldsValueInfo.ContactId, customFieldsValueInfo.CategoryId);
    }

    public static CustomFieldsMappingInfo GetCategoryCustomFieldsMapping(
      CustomFieldsType customFieldsType,
      int categoryId,
      bool twoWayTransfersOnly)
    {
      CustomFieldsMappingInfo customFieldsMapping = new CustomFieldsMappingInfo(customFieldsType, new CustomFieldMappingInfo[0]);
      if (CustomFieldsType.BizCategoryCustom != customFieldsType)
        return customFieldsMapping;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT f.* FROM BizCategoryCustomFieldDefinition f WHERE f.LoanFieldId != '' ");
      if (0 <= categoryId)
        dbQueryBuilder.AppendLine(" AND f.CategoryId = " + (object) categoryId);
      if (twoWayTransfersOnly)
        dbQueryBuilder.AppendLine(" AND f.TwoWayTransfer = 1");
      dbQueryBuilder.AppendLine(" ORDER BY f.FieldNumber");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return customFieldsMapping;
      customFieldsMapping.CustomFieldMappings = new CustomFieldMappingInfo[count];
      for (int index = 0; index < count; ++index)
        customFieldsMapping.CustomFieldMappings[index] = BizPartnerContact.convertRowToCustomFieldMapping(dataRowCollection[index]);
      return customFieldsMapping;
    }

    private static CustomFieldMappingInfo convertRowToCustomFieldMapping(DataRow definitionRow)
    {
      int recordId = (int) definitionRow["FieldId"];
      int categoryId = (int) definitionRow["CategoryId"];
      int fieldNumber = (int) definitionRow["FieldNumber"];
      FieldFormat fieldFormat = (FieldFormat) definitionRow["FieldFormat"];
      string loanFieldId = (string) definitionRow["LoanFieldId"];
      bool twoWayTransfer = (bool) definitionRow["TwoWayTransfer"];
      return new CustomFieldMappingInfo(CustomFieldsType.BizCategoryCustom, categoryId, fieldNumber, recordId, fieldFormat, loanFieldId, twoWayTransfer);
    }

    private static string encodeIntArray(Array intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      foreach (int num in intArray)
        stringBuilder.Append(num.ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private static CustomFieldsDefinitionInfo convertRowToCustomFieldsDefinition(
      CustomFieldsType customFieldsType,
      DataRow definitionRow)
    {
      int recordId = (int) definitionRow["CategoryId"];
      return new CustomFieldsDefinitionInfo(customFieldsType, recordId, (CustomFieldDefinitionInfo[]) null);
    }

    private static CustomFieldDefinitionInfo convertRowToCustomFieldDefinition(DataRow fieldRow)
    {
      return new CustomFieldDefinitionInfo((int) fieldRow["FieldId"], (int) fieldRow["CategoryId"], (int) fieldRow["FieldNumber"], (string) fieldRow["FieldDescription"], (FieldFormat) fieldRow["FieldFormat"], (string) fieldRow["LoanFieldId"], (bool) fieldRow["TwoWayTransfer"], (CustomFieldOptionDefinitionInfo[]) null);
    }

    private static CustomFieldOptionDefinitionInfo convertRowToCustomFieldOptionDefinition(
      DataRow optionRow)
    {
      return new CustomFieldOptionDefinitionInfo((int) optionRow["FieldId"], (int) optionRow["OptionNumber"], (string) optionRow["OptionValue"]);
    }

    private static CustomFieldValueInfo convertRowToCustomFieldValue(DataRow valueRow)
    {
      return new CustomFieldValueInfo((int) valueRow["FieldId"], (int) valueRow["ContactId"], (FieldFormat) valueRow["FieldFormat"], (string) valueRow["FieldValue"]);
    }

    private BizPartnerInfo getContactInfoFromDatabase()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.SelectFrom(DbAccessManager.GetTable(this.ContactTable), new DbValue("ContactID", (object) this.id.ContactID));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection.Count == 0)
          Err.Raise(TraceLevel.Warning, nameof (BizPartnerContact), (ServerException) new ObjectNotFoundException("Contact not found with ID " + this.id.ToString(), ObjectType.Contact, (object) this.id.ToString()));
        return BizPartnerContact.dataRowToBizPartnerInfo(dataRowCollection[0]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (BizPartnerInfo) null;
      }
    }

    private void saveContactInfoToDatabase(BizPartnerInfo info)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Update(DbAccessManager.GetTable(this.ContactTable), BizPartnerContact.createDbValueList(info), new DbValue("ContactID", (object) this.id.ContactID));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (BizPartnerContact), ex);
      }
    }

    private static DbValueList createDbValueList(BizPartnerInfo info)
    {
      return new DbValueList()
      {
        {
          "IsPublic",
          (object) (info.AccessLevel == ContactAccess.Private ? 0 : 1)
        },
        {
          "OwnerID",
          (object) info.OwnerID,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "CategoryID",
          (object) info.CategoryID
        },
        {
          "FirstName",
          (object) info.FirstName
        },
        {
          "LastName",
          (object) info.LastName
        },
        {
          "Salutation",
          (object) info.Salutation
        },
        {
          "CompanyName",
          (object) info.CompanyName
        },
        {
          "JobTitle",
          (object) info.JobTitle
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
          "HomePhone",
          (object) info.HomePhone
        },
        {
          "WorkPhone",
          (object) info.WorkPhone
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
          "LicenseNumber",
          (object) info.BizContactLicense.LicenseNumber
        },
        {
          "Fees",
          (object) info.Fees,
          (IDbEncoder) DbEncoding.MinusOneAsNull
        },
        {
          "Comment",
          (object) info.Comment
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
          "LastModified",
          (object) DateTime.Now
        },
        {
          "BizLicenseAuthName",
          (object) info.BizContactLicense.LicenseAuthName
        },
        {
          "bizLicenseAuthType",
          (object) info.BizContactLicense.LicenseAuthType
        },
        {
          "BizLicenseAuthStateCode",
          (object) info.BizContactLicense.LicenseStateCode
        },
        {
          "BizLicenseAuthDate",
          (object) info.BizContactLicense.LicenseIssueDate
        },
        {
          "personalInfoLicenseNumber",
          (object) info.PersonalInfoLicense.LicenseNumber
        },
        {
          "personalInfoLicenseAuthName",
          (object) info.PersonalInfoLicense.LicenseAuthName
        },
        {
          "personalInfoLicenseAuthType",
          (object) info.PersonalInfoLicense.LicenseAuthType
        },
        {
          "PersonalInfoLicenseAuthStateCode",
          (object) info.PersonalInfoLicense.LicenseStateCode
        },
        {
          "PersonalInfoLicenseAuthDate",
          (object) info.PersonalInfoLicense.LicenseIssueDate
        }
      };
    }

    private static BizPartnerInfo[] executeListQuery(DataTable dt)
    {
      DataRowCollection rows = dt.Rows;
      BizPartnerInfo[] bizPartnerInfoArray = new BizPartnerInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        bizPartnerInfoArray[index] = BizPartnerContact.dataRowToBizPartnerInfo(rows[index]);
      return bizPartnerInfoArray;
    }

    private static BizPartnerInfo[] executeFieldListQuery(DataTable dt)
    {
      DataRowCollection rows = dt.Rows;
      BizPartnerInfo[] bizPartnerInfoArray = new BizPartnerInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
      {
        Dictionary<string, object> data = BizPartnerContact.constructBizPartnerInfoHashtable(rows[index]);
        bizPartnerInfoArray[index] = data.Count <= 0 ? BizPartnerContact.dataRowToBizPartnerInfo(rows[index]) : new BizPartnerInfo(data);
      }
      return bizPartnerInfoArray;
    }

    private static BizPartnerSummaryInfo[] executeListSummaryQuery(DataTable dt)
    {
      DataRowCollection rows = dt.Rows;
      BizPartnerSummaryInfo[] partnerSummaryInfoArray = new BizPartnerSummaryInfo[rows.Count];
      for (int index = 0; index < rows.Count; ++index)
        partnerSummaryInfoArray[index] = BizPartnerContact.dataRowToBizPartnerSummaryInfo(rows[index]);
      return partnerSummaryInfoArray;
    }

    private static BizPartnerSummaryInfo dataRowToBizPartnerSummaryInfo(DataRow r)
    {
      Hashtable data = BizPartnerContact.constructHashtable(r);
      return data.Count > 0 ? new BizPartnerSummaryInfo(data) : new BizPartnerSummaryInfo((int) r["ContactID"], (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["CategoryID"], (object) -1), EllieMae.EMLite.DataAccess.SQL.Decode(r["CompanyName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LastName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["FirstName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["WorkPhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizEmail"], (object) "").ToString(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoSpam"], (object) false), (ContactAccess) Enum.Parse(typeof (ContactAccess), r["AccessLevel"].ToString(), true), (DateTime) EllieMae.EMLite.DataAccess.SQL.Decode(r["LastModified"], (object) DateTime.MinValue));
    }

    private static Hashtable constructHashtable(DataRow row)
    {
      Hashtable hashtable = new Hashtable();
      foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
      {
        switch (column.ColumnName)
        {
          case "Contact.ContactID":
          case "Contact.CategoryID":
          case "Contact.Fees":
            if (string.Concat(row[column.ColumnName]) == "")
            {
              hashtable[(object) column.ColumnName] = (object) 0;
              continue;
            }
            hashtable[(object) column.ColumnName] = (object) int.Parse(string.Concat(row[column.ColumnName]));
            continue;
          case "Contact.LastModified":
            if (string.Concat(row[column.ColumnName]) == "")
            {
              hashtable[(object) column.ColumnName] = (object) DateTime.MinValue;
              continue;
            }
            hashtable[(object) column.ColumnName] = (object) DateTime.Parse(string.Concat(row[column.ColumnName]));
            continue;
          case "Contact.AccessLevel":
            hashtable[(object) column.ColumnName] = (object) (ContactAccess) Enum.Parse(typeof (ContactAccess), EllieMae.EMLite.DataAccess.SQL.Decode(row[column.ColumnName], (object) ContactAccess.Private.ToString()).ToString(), true);
            continue;
          default:
            hashtable[(object) column.ColumnName] = (object) string.Concat(row[column.ColumnName]);
            continue;
        }
      }
      return hashtable;
    }

    private static Dictionary<string, object> constructBizPartnerInfoHashtable(DataRow row)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      foreach (DataColumn column in (InternalDataCollectionBase) row.Table.Columns)
        dictionary[column.ColumnName] = (object) string.Concat(row[column.ColumnName]);
      return dictionary;
    }

    private static BizPartnerInfo dataRowToBizPartnerInfo(DataRow r)
    {
      return new BizPartnerInfo((int) r["ContactID"], (ContactAccess) Enum.Parse(typeof (ContactAccess), r["AccessLevel"].ToString(), true), EllieMae.EMLite.DataAccess.SQL.Decode(r["OwnerID"], (object) "").ToString(), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["CategoryID"], (object) -1), EllieMae.EMLite.DataAccess.SQL.Decode(r["FirstName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LastName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CompanyName"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["JobTitle"], (object) "").ToString(), new EllieMae.EMLite.ClientServer.Address(EllieMae.EMLite.DataAccess.SQL.Decode(r["BizAddress1"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizAddress2"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizCity"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizState"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizZip"], (object) "").ToString()), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizWebUrl"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["HomePhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["WorkPhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["MobilePhone"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["FaxNumber"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PersonalEmail"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["BizEmail"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["LicenseNumber"], (object) "").ToString(), (int) EllieMae.EMLite.DataAccess.SQL.Decode(r["Fees"], (object) -1), EllieMae.EMLite.DataAccess.SQL.Decode(r["Comment"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField1"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField2"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField3"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["CustField4"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PrimaryEmail"], (object) "").ToString().Trim(), EllieMae.EMLite.DataAccess.SQL.Decode(r["PrimaryPhone"], (object) "").ToString().Trim(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(r["NoSpam"], (object) false), (DateTime) r["LastModified"], EllieMae.EMLite.DataAccess.SQL.Decode(r["Salutation"], (object) "").ToString(), (Guid) EllieMae.EMLite.DataAccess.SQL.Decode(r["Guid"], (object) Guid.NewGuid()), new BizContactLicenseInfo(r["LicenseNumber"].ToString(), r["BizLicenseAuthName"].ToString(), r["BizLicenseAuthType"].ToString(), r["BizLicenseAuthStateCode"].ToString(), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["BizLicenseAuthDate"])), new BizContactLicenseInfo(r["PersonalInfoLicenseNumber"].ToString(), r["PersonalInfoLicenseAuthName"].ToString(), r["PersonalInfoLicenseAuthType"].ToString(), r["PersonalInfoLicenseAuthStateCode"].ToString(), EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(r["PersonalInfoLicenseAuthDate"])));
    }

    private static BizCategory dataRowToBizCategory(DataRow r)
    {
      return new BizCategory((int) r["CategoryID"], r["CategoryName"].ToString());
    }

    public static string[] GetCRMLoanGuids(int contactID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select distinct A.loanRefId from BizPartnerLoans A inner join BizPartner B on A.contactGuid = B.Guid where B.ContactID = " + (object) contactID);
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
        Err.Reraise(nameof (BizPartnerContact), ex);
        return (string[]) null;
      }
    }

    public static ContactLoanPair[] GetRelatedLoansForBizPartner(int contactId)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BizPartnerLoans.*, BizPartner.ContactID, Loan.Guid as LoanGuid from BizPartnerLoans");
      dbQueryBuilder.AppendLine("  inner join BizPartner on BizPartner.Guid = BizPartnerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BizPartnerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where BizPartner.ContactID = " + (object) contactId);
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BizPartnerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair[] GetRelatedLoansForBizPartner(string contactGuid)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BizPartnerLoans.*, BizPartner.ContactID, Loan.Guid as LoanGuid from BizPartnerLoans");
      dbQueryBuilder.AppendLine("  inner join BizPartner on BizPartner.Guid = BizPartnerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BizPartnerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where BizPartner.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) contactGuid));
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BizPartnerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair[] GetBizPartnersForLoan(string loanGuid)
    {
      List<ContactLoanPair> contactLoanPairList = new List<ContactLoanPair>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select BizPartnerLoans.*, BizPartner.ContactID, Loan.Guid as LoanGuid from BizPartnerLoans");
      dbQueryBuilder.AppendLine("  inner join BizPartner on BizPartner.Guid = BizPartnerLoans.contactGuid");
      dbQueryBuilder.AppendLine("  inner join LoanSummary Loan on Loan.XrefID = BizPartnerLoans.loanRefId");
      dbQueryBuilder.AppendLine("where Loan.Guid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanGuid));
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        contactLoanPairList.Add(BizPartnerContact.dataRowToContactLoanPair(r));
      return contactLoanPairList.ToArray();
    }

    public static ContactLoanPair dataRowToContactLoanPair(DataRow r)
    {
      return new ContactLoanPair(EllieMae.EMLite.DataAccess.SQL.DecodeString(r["LoanGuid"]), string.Concat(r["ContactGuid"]), EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["ContactID"]), ContactType.BizPartner, (CRMRoleType) EllieMae.EMLite.DataAccess.SQL.DecodeInt(r["RoleType"]), -1);
    }
  }
}
