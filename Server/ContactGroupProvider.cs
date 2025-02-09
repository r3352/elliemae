// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ContactGroupProvider
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Campaign;
using EllieMae.EMLite.ClientServer.ContactGroup;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ContactGroupProvider
  {
    private const string className = "ContactGroupProvider�";

    public static ContactGroupInfo[] GetContactGroupsForUser(
      string userId,
      ContactGroupCollectionCriteria criteria)
    {
      if (criteria == null || !(typeof (ContactGroupCollectionCriteria) == criteria.GetType()) || criteria.UserId == null || !Enum.IsDefined(typeof (ContactType), (object) criteria.ContactType))
        return (ContactGroupInfo[]) null;
      UserInfo userInfo = UserStore.GetLatestVersion(userId).UserInfo;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (criteria.ContactType.Equals((object) ContactType.PublicBiz) && !userInfo.IsAdministrator())
        dbQueryBuilder.AppendLine("SELECT distinct ContactGroup.* FROM ContactGroup  inner join AclGroupPublicBizGroupRef  on  ContactGroup.GroupId =AclGroupPublicBizGroupRef.bizGroupID inner join AclGroupMembers on AclGroupMembers.GroupID= AclGroupPublicBizGroupRef.aclGroupID where AclGroupMembers.UserID =" + EllieMae.EMLite.DataAccess.SQL.Encode((object) criteria.UserId) + " and ContactGroup.ContactType =  " + Enum.Format(typeof (ContactType), (object) criteria.ContactType, "D"));
      else
        dbQueryBuilder.AppendLine("SELECT ContactGroup.* FROM ContactGroup  WHERE ContactGroup.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) criteria.UserId) + " AND ContactGroup.ContactType = " + Enum.Format(typeof (ContactType), (object) criteria.ContactType, "D"));
      if (criteria.GroupTypes != null)
        dbQueryBuilder.AppendLine(" AND ContactGroup.GroupType IN (" + ContactGroupProvider.encodeEnumArray(typeof (ContactGroupType), (Array) criteria.GroupTypes) + ")");
      dbQueryBuilder.AppendLine("ORDER BY ContactGroup.GroupName");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return new ContactGroupInfo[0];
      ContactGroupInfo[] contactGroupsForUser = new ContactGroupInfo[count];
      for (int index = 0; index < count; ++index)
      {
        DataRow groupRow = dataRowCollection[index];
        contactGroupsForUser[index] = ContactGroupProvider.convertRowToGroup(groupRow);
        contactGroupsForUser[index].ContactIds = contactGroupsForUser[index].ContactType != ContactType.Borrower ? ContactGroupProvider.getMemberIdsForPartnerGroup(contactGroupsForUser[index].GroupId) : ContactGroupProvider.getMemberIdsForBorrowerGroup(contactGroupsForUser[index].GroupId);
      }
      return contactGroupsForUser;
    }

    public static ContactGroupInfo[] GetContactGroupsForContact(
      string userId,
      ContactType contactType,
      int contactId)
    {
      string str = contactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (contactType == ContactType.Borrower)
        dbQueryBuilder.AppendLine("SELECT g.* FROM ContactGroup g, " + str + " r WHERE g.GroupId = r.GroupId and g.UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userId) + " AND g.ContactType = " + Enum.Format(typeof (ContactType), (object) contactType, "D"));
      else
        dbQueryBuilder.AppendLine("SELECT g.* FROM ContactGroup g, " + str + " r WHERE g.GroupId = r.GroupId AND g.ContactType = " + Enum.Format(typeof (ContactType), (object) contactType, "D"));
      dbQueryBuilder.AppendLine(" AND g.GroupType IN (" + ContactGroupProvider.encodeEnumArray(typeof (ContactGroupType), (Array) new ContactGroupType[1]) + ")");
      dbQueryBuilder.AppendLine(" and r.ContactId = " + contactId.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return new ContactGroupInfo[0];
      ContactGroupInfo[] groupsForContact = new ContactGroupInfo[count];
      for (int index = 0; index < count; ++index)
      {
        DataRow groupRow = dataRowCollection[index];
        groupsForContact[index] = ContactGroupProvider.convertRowToGroup(groupRow);
      }
      return groupsForContact;
    }

    public static ContactGroupInfo GetContactGroup(int groupId, string userId)
    {
      if (0 >= groupId)
        return (ContactGroupInfo) null;
      ContactGroupInfo contactGroupInfo = ContactGroupProvider.getContactGroupInfo(groupId);
      if (!((ContactGroupInfo) null != contactGroupInfo))
        return (ContactGroupInfo) null;
      UserInfo userInfo = UserStore.GetLatestVersion(userId).UserInfo;
      if (contactGroupInfo.ContactType == ContactType.Borrower)
      {
        contactGroupInfo.BorrowerMembers = ContactGroupProvider.GetMembersForBorrowerGroup(contactGroupInfo, userInfo);
        contactGroupInfo.ContactIds = new int[contactGroupInfo.BorrowerMembers.Length];
        for (int index = 0; index < contactGroupInfo.BorrowerMembers.Length; ++index)
          contactGroupInfo.ContactIds[index] = contactGroupInfo.BorrowerMembers[index].ContactID;
      }
      else
      {
        contactGroupInfo.PartnerMembers = ContactGroupProvider.GetMembersForPartnerGroup(contactGroupInfo, userInfo);
        contactGroupInfo.ContactIds = new int[contactGroupInfo.PartnerMembers.Length];
        for (int index = 0; index < contactGroupInfo.PartnerMembers.Length; ++index)
          contactGroupInfo.ContactIds[index] = contactGroupInfo.PartnerMembers[index].ContactID;
      }
      return contactGroupInfo;
    }

    private static ContactGroupInfo getContactGroupInfo(int groupId)
    {
      if (0 >= groupId)
        return (ContactGroupInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT g.* FROM ContactGroup g WHERE g.GroupId = " + (object) groupId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ContactGroupInfo) null : ContactGroupProvider.convertRowToGroup(dataRowCollection[0]);
    }

    public static void AddContactsToGroup(
      int[] newContactIds,
      int groupId,
      ContactType contactType)
    {
      string str1 = contactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
      string str2 = contactType == ContactType.Borrower ? "Borrower" : "BizPartner";
      int[] oldInts = contactType == ContactType.Borrower ? ContactGroupProvider.getMemberIdsForBorrowerGroup(groupId) : ContactGroupProvider.getMemberIdsForPartnerGroup(groupId);
      if (oldInts.Length != 0)
        ContactGroupProvider.arrayDifference(ref newContactIds, oldInts);
      if (newContactIds.Length == 0)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      ContactGroupProvider.IncrementalIntArray intArray = new ContactGroupProvider.IncrementalIntArray(100, newContactIds);
      do
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.Append("INSERT INTO " + str1 + " SELECT " + groupId.ToString() + " AS GroupId, b.ContactId AS ContactId, GETDATE() AS CreatedDate FROM " + str2 + " b WHERE b.ContactId IN " + ContactGroupProvider.encodeIntArray(intArray) + " AND b.ContactId NOT IN (SELECT g.ContactId FROM " + str1 + " g WHERE g.GroupId = " + groupId.ToString() + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      while (intArray.Increment());
    }

    private static void arrayDifference(ref int[] newInts, int[] oldInts)
    {
      ArrayList arrayList1 = new ArrayList((ICollection) newInts);
      ArrayList arrayList2 = new ArrayList((ICollection) oldInts);
      if (arrayList1.Count <= arrayList2.Count)
      {
        arrayList2.Sort();
        for (int index1 = arrayList1.Count - 1; index1 >= 0; --index1)
        {
          int index2 = arrayList2.BinarySearch(arrayList1[index1]);
          if (0 <= index2)
            arrayList1.Remove(arrayList2[index2]);
        }
      }
      else
      {
        arrayList1.Sort();
        for (int index3 = arrayList2.Count - 1; index3 >= 0; --index3)
        {
          int index4 = arrayList1.BinarySearch(arrayList2[index3]);
          if (0 <= index4)
            arrayList1.RemoveAt(index4);
        }
      }
      newInts = (int[]) arrayList1.ToArray(typeof (int));
    }

    public static ContactGroupInfo SaveContactGroup(
      ContactGroupInfo groupInfo,
      string userId,
      bool getOnlyGroup = false)
    {
      if (!((ContactGroupInfo) null != groupInfo) || !(typeof (ContactGroupInfo) == groupInfo.GetType()))
        return (ContactGroupInfo) null;
      int groupId = ContactGroupProvider.SaveContactGroup(groupInfo);
      return getOnlyGroup ? ContactGroupProvider.getContactGroupInfo(groupId) : ContactGroupProvider.GetContactGroup(groupId, userId);
    }

    public static void DeleteContactGroup(int groupId)
    {
      if (0 >= groupId)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("DELETE FROM ContactGroup WHERE GroupId = " + (object) groupId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static bool CheckForDuplicateContactGroup(ContactGroupInfo groupInfo)
    {
      if (!((ContactGroupInfo) null != groupInfo) || !(typeof (ContactGroupInfo) == groupInfo.GetType()))
        return true;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT GroupId FROM ContactGroup WHERE UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupInfo.UserId) + " AND ContactType = " + (object) (int) groupInfo.ContactType + " AND GroupType = " + (object) (int) groupInfo.GroupType + " AND GroupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupInfo.GroupName));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return false;
      return count > 1 || groupInfo.GroupId == 0 || groupInfo.GroupId != (int) dataRowCollection[0]["GroupId"];
    }

    public static ContactGroupInfo[] GetPublicBizContactGroups()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT g.* FROM ContactGroup g where g.ContactType = " + Enum.Format(typeof (ContactType), (object) ContactType.PublicBiz, "D"));
      dbQueryBuilder.AppendLine("ORDER BY g.GroupName");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return new ContactGroupInfo[0];
      ContactGroupInfo[] bizContactGroups = new ContactGroupInfo[count];
      for (int index = 0; index < count; ++index)
      {
        DataRow groupRow = dataRowCollection[index];
        bizContactGroups[index] = ContactGroupProvider.convertRowToGroup(groupRow);
        bizContactGroups[index].ContactIds = ContactGroupProvider.getMemberIdsForPartnerGroup(bizContactGroups[index].GroupId);
      }
      return bizContactGroups;
    }

    public static BorrowerSummaryInfo[] GetMembersForBorrowerGroup(int groupId, string userId)
    {
      if (0 >= groupId)
        return (BorrowerSummaryInfo[]) null;
      ContactGroupInfo contactGroupInfo = ContactGroupProvider.getContactGroupInfo(groupId);
      if (!((ContactGroupInfo) null != contactGroupInfo))
        return (BorrowerSummaryInfo[]) null;
      UserInfo userInfo = UserStore.GetLatestVersion(userId).UserInfo;
      return ContactGroupProvider.GetMembersForBorrowerGroup(contactGroupInfo, userInfo);
    }

    public static BorrowerSummaryInfo[] GetMembersForBorrowerGroup(
      ContactGroupInfo contactGroup,
      UserInfo userInfo)
    {
      if (!((ContactGroupInfo) null != contactGroup))
        return (BorrowerSummaryInfo[]) null;
      QueryCriterion[] criteria = new QueryCriterion[0];
      if (PsuedoGroupId.MyContacts == contactGroup.GroupId)
      {
        QueryCriterion[] queryCriterionArray = new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("ContactGroup.UserID", contactGroup.UserId, StringMatchType.Exact)
        };
        criteria = new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("Contact.OwnerID", userInfo.Userid, StringMatchType.Exact)
        };
      }
      else if (PsuedoGroupId.AllContacts != contactGroup.GroupId)
        criteria = new QueryCriterion[1]
        {
          (QueryCriterion) new OrdinalValueCriterion("ContactGroup.GroupID", (object) contactGroup.GroupId, OrdinalMatchType.Equals)
        };
      return BorrowerContact.QueryBorrowerSummaries(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null);
    }

    private static int[] getMemberIdsForBorrowerGroup(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select ContactId as ContactId from ContactGroupBorrowers where GroupId = " + groupId.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return new int[0];
      int[] forBorrowerGroup = new int[count];
      for (int index = 0; index < count; ++index)
        forBorrowerGroup[index] = (int) dataRowCollection[index]["ContactId"];
      return forBorrowerGroup;
    }

    public static BizPartnerSummaryInfo[] GetMembersForPartnerGroup(int groupId, string userId)
    {
      if (0 >= groupId)
        return (BizPartnerSummaryInfo[]) null;
      ContactGroupInfo contactGroupInfo = ContactGroupProvider.getContactGroupInfo(groupId);
      if (!((ContactGroupInfo) null != contactGroupInfo))
        return (BizPartnerSummaryInfo[]) null;
      UserInfo userInfo = UserStore.GetLatestVersion(userId).UserInfo;
      return ContactGroupProvider.GetMembersForPartnerGroup(contactGroupInfo, userInfo);
    }

    public static BizPartnerSummaryInfo[] GetMembersForPartnerGroup(
      ContactGroupInfo contactGroup,
      UserInfo userInfo)
    {
      if ((ContactGroupInfo) null == contactGroup || (UserInfo) null == userInfo)
        return (BizPartnerSummaryInfo[]) null;
      QueryCriterion[] criteria = new QueryCriterion[0];
      if (PsuedoGroupId.MyContacts == contactGroup.GroupId)
        criteria = new QueryCriterion[1]
        {
          (QueryCriterion) new StringValueCriterion("Contact.OwnerID", userInfo.Userid, StringMatchType.Exact)
        };
      else if (PsuedoGroupId.AllContacts != contactGroup.GroupId)
        criteria = new QueryCriterion[1]
        {
          (QueryCriterion) new OrdinalValueCriterion("ContactGroup.GroupID", (object) contactGroup.GroupId, OrdinalMatchType.Equals)
        };
      return BizPartnerContact.QueryBizPartnerSummaries(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null);
    }

    private static int[] getMemberIdsForPartnerGroup(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select ContactId as ContactId from ContactGroupPartners where GroupId = " + groupId.ToString());
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return new int[0];
      int[] idsForPartnerGroup = new int[count];
      for (int index = 0; index < count; ++index)
        idsForPartnerGroup[index] = (int) dataRowCollection[index]["ContactId"];
      return idsForPartnerGroup;
    }

    public static int[] GetContactQueryMemberIds(
      User user,
      ContactQueryInfo queryInfo,
      SortField[] sortFields)
    {
      if (queryInfo == null)
        return (int[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DataRowCollection dataRowCollection = (DataRowCollection) null;
      if ((ContactQueryType.CampaignPredefinedQuery & queryInfo.QueryType) == ContactQueryType.CampaignPredefinedQuery)
      {
        dbQueryBuilder.Append(ContactGroupProvider.BuildPredefinedQueryString(user, queryInfo, false));
        dbQueryBuilder.AppendLine(" ORDER BY " + ContactGroupProvider.getContactGroupMembersSortField(sortFields));
        dataRowCollection = dbQueryBuilder.Execute();
      }
      else
      {
        DataTable dataTable = ContactGroupProvider.BuildAdvancedQueryResult(user, queryInfo, true).ToDataTable();
        if (dataTable != null)
          dataRowCollection = dataTable.Rows;
      }
      int count = dataRowCollection.Count;
      if (count == 0)
        return (int[]) null;
      int[] contactQueryMemberIds = new int[count];
      for (int index = 0; index < count; ++index)
      {
        try
        {
          contactQueryMemberIds[index] = (int) dataRowCollection[index]["ContactId"];
        }
        catch
        {
          if (string.Concat(dataRowCollection[index]["Contact.ContactId"]) != "")
            contactQueryMemberIds[index] = int.Parse(string.Concat(dataRowCollection[index]["Contact.ContactId"]));
        }
      }
      return contactQueryMemberIds;
    }

    public static ContactQueryInfo[] GetContactQueries(ContactQueryCollectionCriteria criteria)
    {
      if (criteria == null || !(typeof (ContactQueryCollectionCriteria) == criteria.GetType()) || !Enum.IsDefined(typeof (ContactType), (object) criteria.ContactType))
        return (ContactQueryInfo[]) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT q.* FROM ContactQuery q WHERE q.ContactType = " + Enum.Format(typeof (ContactType), (object) criteria.ContactType, "D") + " AND q.QueryType IN (" + ContactGroupProvider.encodeEnumArray(typeof (ContactQueryType), (Array) criteria.QueryTypes) + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int count = dataRowCollection.Count;
      if (count == 0)
        return (ContactQueryInfo[]) null;
      ContactQueryInfo[] contactQueries = new ContactQueryInfo[count];
      for (int index = 0; index < count; ++index)
      {
        DataRow queryRow = dataRowCollection[index];
        contactQueries[index] = ContactGroupProvider.convertRowToQuery(queryRow);
      }
      return contactQueries;
    }

    public static ContactQueryInfo GetContactQuery(int queryId)
    {
      if (0 >= queryId)
        return (ContactQueryInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT q.* FROM ContactQuery q WHERE q.QueryId = " + (object) queryId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return 1 != dataRowCollection.Count ? (ContactQueryInfo) null : ContactGroupProvider.convertRowToQuery(dataRowCollection[0]);
    }

    public static ContactQueryInfo SaveContactQuery(ContactQueryInfo queryInfo)
    {
      if (queryInfo == null || !(typeof (ContactQueryInfo) == queryInfo.GetType()))
        return (ContactQueryInfo) null;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@queryId", "int");
      if (queryInfo.QueryId == 0)
      {
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("ContactQuery"), ContactGroupProvider.createDbValueList(queryInfo), true, false);
        dbQueryBuilder.SelectIdentity("@queryId");
      }
      else
      {
        DbValue key = new DbValue("QueryId", (object) queryInfo.QueryId);
        dbQueryBuilder.Update(DbAccessManager.GetTable("ContactQuery"), ContactGroupProvider.createDbValueList(queryInfo), key);
        dbQueryBuilder.SelectVar("@queryId", (object) queryInfo.QueryId);
      }
      dbQueryBuilder.Select("@queryId");
      return ContactGroupProvider.GetContactQuery((int) dbQueryBuilder.ExecuteScalar());
    }

    public static void DeleteContactQuery(int queryId)
    {
      if (0 >= queryId)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE FROM ContactQuery WHERE QueryId = " + (object) queryId);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static string BuildPredefinedQueryString(
      User user,
      ContactQueryInfo queryInfo,
      bool idsOnly)
    {
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml(queryInfo.XmlQueryString);
      XmlElement documentElement = xmlDocument.DocumentElement;
      string attribute = documentElement.GetAttribute("Name");
      string[] queryParameters = new string[documentElement.ChildNodes.Count];
      for (int i = 0; i < documentElement.ChildNodes.Count; ++i)
        queryParameters[i] = ((XmlElement) documentElement.ChildNodes[i]).GetAttribute("Value");
      return queryInfo.ContactType != ContactType.Borrower ? ContactGroupProvider.buildPredefinedPartnerString(user, attribute, queryParameters, idsOnly) : ContactGroupProvider.buildPredefinedBorrowerString(user, attribute, queryParameters, queryInfo.PrimaryOnly, queryInfo.OwnerOnly, queryInfo.NotOwner, idsOnly);
    }

    public static string BuildAdvancedQueryString(
      User user,
      ContactQueryInfo queryInfo,
      bool idsOnly)
    {
      QueryCriterion queryCriterion = ((FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), queryInfo.XmlQueryString)).CreateEvaluator().ToQueryCriterion();
      if (queryInfo.PrimaryOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals)) : (QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals);
      if (queryInfo.OwnerOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact)) : (QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact);
      UserInfo userInfo = UserStore.GetLatestVersion(queryInfo.UserId).UserInfo;
      QueryCriterion[] criteria = new QueryCriterion[1]
      {
        queryCriterion
      };
      return queryInfo.ContactType == ContactType.Borrower ? (idsOnly ? BorrowerContact.getQueryBorrowerIdsSql(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null) : BorrowerContact.getQueryBorrowerSummariesSql(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null)) : (idsOnly ? BizPartnerContact.getQueryBizPartnerIdsSql(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null) : BizPartnerContact.getQueryBizPartnerSummariesSql(userInfo, criteria, RelatedLoanMatchType.None, (SortField[]) null));
    }

    public static string BuildAdvancedQueryResultString(
      User user,
      ContactQueryInfo queryInfo,
      bool idsOnly)
    {
      FieldFilterList fieldFilterList = (FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), queryInfo.XmlQueryString);
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (fieldFilterList != null)
        queryCriterion = fieldFilterList.CreateEvaluator().ToQueryCriterion();
      if (queryInfo.PrimaryOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals)) : (QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals);
      if (queryInfo.OwnerOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact)) : (QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact);
      QueryCriterion[] criteria = new QueryCriterion[1]
      {
        queryCriterion
      };
      UserInfo userInfo = UserStore.GetLatestVersion(queryInfo.UserId).UserInfo;
      return queryInfo.ContactType == ContactType.Borrower ? (idsOnly ? BorrowerContact.GenerateQueryResultQueryString("Contact.ContactID", userInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null) : BorrowerContact.GenerateQueryResultQueryString("Borrower.*", userInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null)) : (idsOnly ? BizPartnerContact.GenerateQueryResultQueryString("Contact.ContactID", userInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null) : BizPartnerContact.GenerateQueryResultQueryString("BizPartner.*", userInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null));
    }

    public static QueryResult BuildAdvancedQueryResult(
      User user,
      ContactQueryInfo queryInfo,
      bool idsOnly)
    {
      FieldFilterList fieldFilterList = (FieldFilterList) QueryXmlConverter.Deserialize(typeof (FieldFilterList), queryInfo.XmlQueryString);
      QueryCriterion queryCriterion = (QueryCriterion) null;
      if (fieldFilterList != null)
        queryCriterion = fieldFilterList.CreateEvaluator().ToQueryCriterion();
      if (queryInfo.PrimaryOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals)) : (QueryCriterion) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals);
      if (queryInfo.OwnerOnly)
        queryCriterion = queryCriterion != null ? queryCriterion.And((QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact)) : (QueryCriterion) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact);
      QueryCriterion[] criteria = new QueryCriterion[1]
      {
        queryCriterion
      };
      return queryInfo.ContactType == ContactType.Borrower ? (idsOnly ? BorrowerContact.GenerateQueryResult(new string[1]
      {
        "Contact.ContactID"
      }, user.UserInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null) : BorrowerContact.GenerateQueryResult(new string[1]
      {
        "Borrower.*"
      }, user.UserInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null)) : (idsOnly ? BizPartnerContact.GenerateQueryResult(new string[1]
      {
        "Contact.ContactID"
      }, user.UserInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null) : BizPartnerContact.GenerateQueryResult(new string[1]
      {
        "BizPartner.*"
      }, user.UserInfo, criteria, fieldFilterList.RelatedLoanMatchType, (SortField[]) null));
    }

    internal static int SaveContactGroup(ContactGroupInfo groupInfo)
    {
      if (!((ContactGroupInfo) null != groupInfo) || !(typeof (ContactGroupInfo) == groupInfo.GetType()))
        return 0;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Declare("@contactGroupId", "int");
      if (groupInfo.GroupId == 0)
      {
        dbQueryBuilder.AppendLine("IF NOT EXISTS(SELECT 1 FROM ContactGroup WHERE UserId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupInfo.UserId) + " AND ContactType = " + (object) (int) groupInfo.ContactType + " AND GroupType = " + (object) (int) groupInfo.GroupType + " AND GroupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupInfo.GroupName) + ") BEGIN");
        dbQueryBuilder.InsertInto(DbAccessManager.GetTable("ContactGroup"), ContactGroupProvider.createDbValueList(groupInfo), true, false);
        dbQueryBuilder.SelectIdentity("@contactGroupId");
        dbQueryBuilder.AppendLine("END ELSE BEGIN SELECT @contactGroupId = -1 END");
      }
      else
      {
        DbValue key = new DbValue("GroupId", (object) groupInfo.GroupId);
        dbQueryBuilder.Update(DbAccessManager.GetTable("ContactGroup"), ContactGroupProvider.createDbValueList(groupInfo), key);
        dbQueryBuilder.SelectVar("@contactGroupId", (object) groupInfo.GroupId);
      }
      dbQueryBuilder.Select("@contactGroupId");
      int num = (int) dbQueryBuilder.ExecuteScalar();
      if (-1 == num)
        Err.Raise(TraceLevel.Warning, nameof (ContactGroupProvider), (ServerException) new DuplicateObjectException("A Contact Group already exists with this Group Name.", ObjectType.ContactGroup, (object) groupInfo.GroupId));
      string str1 = groupInfo.ContactType == ContactType.Borrower ? "ContactGroupBorrowers" : "ContactGroupPartners";
      if (groupInfo.DeletedContactIds != null && groupInfo.DeletedContactIds.Length != 0)
      {
        ContactGroupProvider.IncrementalIntArray intArray = new ContactGroupProvider.IncrementalIntArray(100, groupInfo.DeletedContactIds);
        do
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("DELETE FROM " + str1 + " WHERE GroupId = " + (object) num + "AND ContactId IN " + ContactGroupProvider.encodeIntArray(intArray));
          dbQueryBuilder.ExecuteNonQuery();
        }
        while (intArray.Increment());
      }
      if (groupInfo.AddedContactIds != null && groupInfo.AddedContactIds.Length != 0)
      {
        string str2 = groupInfo.ContactType == ContactType.Borrower ? "Borrower" : "BizPartner";
        ContactGroupProvider.IncrementalIntArray intArray = new ContactGroupProvider.IncrementalIntArray(100, groupInfo.AddedContactIds);
        do
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.Append("INSERT INTO " + str1 + " SELECT " + num.ToString() + " AS GroupId, b.ContactId, GETDATE() AS CreatedDate FROM " + str2 + " b WHERE b.ContactId IN " + ContactGroupProvider.encodeIntArray(intArray) + " AND b.ContactId NOT IN (SELECT g.ContactId FROM " + str1 + " g WHERE g.GroupId = " + num.ToString() + ")");
          dbQueryBuilder.ExecuteNonQuery();
        }
        while (intArray.Increment());
      }
      return num;
    }

    internal static void BuildSaveQueryStatement(ContactQueryInfo queryInfo, DbQueryBuilder sqlStmt)
    {
      if (queryInfo == null || !(typeof (ContactQueryInfo) == queryInfo.GetType()))
        return;
      string varName = "@contactQueryId";
      if ((ContactQueryType.CampaignAddQuery & queryInfo.QueryType) == ContactQueryType.CampaignAddQuery)
        varName = "@addQueryId";
      else if ((ContactQueryType.CampaignDeleteQuery & queryInfo.QueryType) == ContactQueryType.CampaignDeleteQuery)
        varName = "@deleteQueryId";
      sqlStmt.Declare(varName, "int");
      if (queryInfo.QueryId == 0)
      {
        sqlStmt.InsertInto(DbAccessManager.GetTable("ContactQuery"), ContactGroupProvider.createDbValueList(queryInfo), true, false);
        sqlStmt.SelectIdentity(varName);
      }
      else
      {
        DbValue key = new DbValue("QueryId", (object) queryInfo.QueryId);
        sqlStmt.Update(DbAccessManager.GetTable("ContactQuery"), ContactGroupProvider.createDbValueList(queryInfo), key);
        sqlStmt.SelectVar(varName, (object) queryInfo.QueryId);
      }
    }

    private static string encodeIntArray(Array intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      foreach (int num in intArray)
        stringBuilder.Append(num.ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private static string encodeIntArray(ContactGroupProvider.IncrementalIntArray intArray)
    {
      StringBuilder stringBuilder = new StringBuilder("(");
      for (int start = intArray.Start; start < intArray.Start + intArray.Count; ++start)
        stringBuilder.Append(intArray.IntArray[start].ToString() + ",");
      stringBuilder.Replace(",", ")", stringBuilder.Length - 1, 1);
      return stringBuilder.ToString();
    }

    private static string getContactGroupMembersSortField(SortField[] sortFields)
    {
      if (sortFields == null || sortFields.Length == 0)
        return "Contact.LastName ASC";
      StringBuilder stringBuilder = new StringBuilder();
      switch (sortFields[0].Term.FieldName)
      {
        case "AccessLevel":
          stringBuilder.Append("Contact.AccessLevel");
          break;
        case "BizEmail":
          stringBuilder.Append("Contact.BizEmail");
          break;
        case "CategoryId":
          stringBuilder.Append("BizCategory.CategoryName");
          break;
        case "CompanyName":
          stringBuilder.Append("Contact.CompanyName");
          break;
        case "ContactType":
          stringBuilder.Append("Contact.ContactType");
          break;
        case "FirstName":
          stringBuilder.Append("Contact.FirstName");
          break;
        case "HomePhone":
          stringBuilder.Append("Contact.HomePhone");
          break;
        case "LastName":
          stringBuilder.Append("Contact.LastName");
          break;
        case "OwnerId":
          stringBuilder.Append("Contact.OwnerId");
          break;
        case "PersonalEmail":
          stringBuilder.Append("Contact.PersonalEmail");
          break;
        case "Status":
          stringBuilder.Append("Contact.Status");
          break;
        case "WorkPhone":
          stringBuilder.Append("Contact.WorkPhone");
          break;
        default:
          stringBuilder.Append("Contact.LastName");
          break;
      }
      if (FieldSortOrder.Descending == sortFields[0].SortOrder)
        stringBuilder.Append(" DESC");
      return stringBuilder.ToString();
    }

    private static string buildPredefinedBorrowerString(
      User user,
      string queryName,
      string[] queryParameters,
      bool primaryOnly,
      bool ownerOnly,
      bool notOwner,
      bool idsOnly)
    {
      ArrayList arrayList = new ArrayList();
      RelatedLoanMatchType matchType = RelatedLoanMatchType.None;
      switch (queryName)
      {
        case "Anniversary":
          ContactQueryItem contactQueryItem1 = new ContactQueryItem();
          contactQueryItem1.FieldDisplayName = "Anniversary";
          contactQueryItem1.FieldName = "Contact.Anniversary";
          contactQueryItem1.GroupName = "ContactInfo";
          contactQueryItem1.Condition = "Is";
          contactQueryItem1.Value1 = queryParameters[0];
          contactQueryItem1.ValueType = "System.DateTime";
          ContactQuery query1 = new ContactQuery();
          query1.Items = new ContactQueryItem[1]
          {
            contactQueryItem1
          };
          query1.LoanMatchType = RelatedLoanMatchType.None;
          ServerContactSearchUtil contactSearchUtil1 = new ServerContactSearchUtil(query1.LoanMatchType, ContactType.Borrower);
          contactSearchUtil1.FlushSearchObjectsToSql(query1, ContactType.Borrower);
          QueryCriterion[] array1 = (QueryCriterion[]) contactSearchUtil1.Criteria.ToArray(typeof (QueryCriterion));
          arrayList.AddRange((ICollection) array1);
          break;
        case "Birthday":
          ContactQueryItem contactQueryItem2 = new ContactQueryItem();
          contactQueryItem2.FieldDisplayName = "Birthday";
          contactQueryItem2.FieldName = "Contact.Birthdate";
          contactQueryItem2.GroupName = "ContactInfo";
          contactQueryItem2.Condition = "Is";
          contactQueryItem2.Value1 = queryParameters[0];
          contactQueryItem2.ValueType = "System.DateTime";
          ContactQuery query2 = new ContactQuery();
          query2.Items = new ContactQueryItem[1]
          {
            contactQueryItem2
          };
          query2.LoanMatchType = RelatedLoanMatchType.None;
          ServerContactSearchUtil contactSearchUtil2 = new ServerContactSearchUtil(query2.LoanMatchType, ContactType.Borrower);
          contactSearchUtil2.FlushSearchObjectsToSql(query2, ContactType.Borrower);
          QueryCriterion[] array2 = (QueryCriterion[]) contactSearchUtil2.Criteria.ToArray(typeof (QueryCriterion));
          arrayList.AddRange((ICollection) array2);
          break;
        case "Borrower Contact Status":
          string fieldName1 = "Contact.Status";
          string queryParameter1 = queryParameters[0];
          arrayList.Add((object) new StringValueCriterion(fieldName1, queryParameter1, StringMatchType.Exact));
          break;
        case "Borrower Contact Type":
          string fieldName2 = "Contact.ContactType";
          string queryParameter2 = queryParameters[0];
          arrayList.Add((object) new StringValueCriterion(fieldName2, queryParameter2, StringMatchType.Exact));
          break;
        case "Imported or Newly Created Borrower Contacts":
          QueryCriterion[] createTimeCriteria = ContactGroupProvider.getBorrowerCreateTimeCriteria(Convert.ToInt32(queryParameters[0]));
          arrayList.AddRange((ICollection) createTimeCriteria);
          break;
        case "Last Loan Closed":
          matchType = RelatedLoanMatchType.LastClosed;
          QueryCriterion[] loanClosedCriteria = ContactGroupProvider.getBorrowerLastLoanClosedCriteria(queryParameters);
          arrayList.AddRange((ICollection) loanClosedCriteria);
          break;
        case "Last Loan Originated":
          matchType = RelatedLoanMatchType.LastOriginated;
          int int32 = Convert.ToInt32(queryParameters[0]);
          string fieldName3 = "Loan.DateFileOpened";
          DateTime today = DateTime.Today;
          DateTime dateTime = DateTime.Today.AddDays((double) -int32);
          DateValueCriterion dateValueCriterion1 = new DateValueCriterion(fieldName3, dateTime, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day);
          DateValueCriterion dateValueCriterion2 = new DateValueCriterion(fieldName3, today, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day);
          arrayList.Add((object) dateValueCriterion1);
          arrayList.Add((object) dateValueCriterion2);
          break;
      }
      if (primaryOnly)
        arrayList.Add((object) new OrdinalValueCriterion("Contact.PrimaryContact", (object) 1, OrdinalMatchType.Equals));
      if (ownerOnly)
        arrayList.Add((object) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact));
      if (notOwner)
        arrayList.Add((object) new StringValueCriterion("Contact.OwnerID", user.UserID, StringMatchType.Exact, false));
      QueryCriterion[] array3 = (QueryCriterion[]) arrayList.ToArray(typeof (QueryCriterion));
      return idsOnly ? BorrowerContact.getQueryBorrowerIdsSql(user.UserInfo, array3, matchType, (SortField[]) null) : BorrowerContact.getQueryBorrowerSummariesSql(user.UserInfo, array3, matchType, (SortField[]) null);
    }

    private static QueryCriterion[] getBorrowerCreateTimeCriteria(int days)
    {
      StringValueCriterion stringValueCriterion = new StringValueCriterion("History.EventType", "First Contact", StringMatchType.Exact);
      string fieldName = "History.TimeOfHistory";
      DateTime today = DateTime.Today;
      DateTime dateTime = DateTime.Today.AddDays((double) -days);
      DateValueCriterion dateValueCriterion1 = new DateValueCriterion(fieldName, dateTime, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day);
      DateValueCriterion dateValueCriterion2 = new DateValueCriterion(fieldName, today, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day);
      return new QueryCriterion[3]
      {
        (QueryCriterion) stringValueCriterion,
        (QueryCriterion) dateValueCriterion1,
        (QueryCriterion) dateValueCriterion2
      };
    }

    private static QueryCriterion[] getBorrowerLastLoanClosedCriteria(string[] queryParameters)
    {
      switch (queryParameters[0])
      {
        case "Completion Date":
          int int32 = Convert.ToInt32(queryParameters[1]);
          string fieldName = "Loan.DateCompleted";
          DateTime today = DateTime.Today;
          DateTime dateTime = DateTime.Today.AddDays((double) -int32);
          return new QueryCriterion[2]
          {
            (QueryCriterion) new DateValueCriterion(fieldName, dateTime, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day),
            (QueryCriterion) new DateValueCriterion(fieldName, today, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day)
          };
        case "Loan Amount":
          OrdinalMatchType matchType1 = "less than" == queryParameters[1] ? OrdinalMatchType.LessThan : ("greater than" == queryParameters[1] ? OrdinalMatchType.GreaterThan : OrdinalMatchType.Equals);
          return new QueryCriterion[1]
          {
            (QueryCriterion) new OrdinalValueCriterion("Loan.LoanAmount", (object) Convert.ToDouble(queryParameters[2]), matchType1)
          };
        case "Interest Rate":
          OrdinalMatchType matchType2 = "less than" == queryParameters[1] ? OrdinalMatchType.LessThan : ("greater than" == queryParameters[1] ? OrdinalMatchType.GreaterThan : OrdinalMatchType.Equals);
          return new QueryCriterion[1]
          {
            (QueryCriterion) new OrdinalValueCriterion("Loan.LoanRate", (object) Convert.ToDouble(queryParameters[2]), matchType2)
          };
        case "Loan Type":
          return new QueryCriterion[1]
          {
            (QueryCriterion) new StringValueCriterion("Loan.LoanType", queryParameters[1], StringMatchType.Exact)
          };
        case "Loan Purpose":
          return new QueryCriterion[1]
          {
            (QueryCriterion) new StringValueCriterion("Loan.LoanPurpose", LoanPurposeEnumUtil.ValueToNameInLoan(LoanPurposeEnumUtil.NameToValue(queryParameters[1])), StringMatchType.Exact)
          };
        default:
          return new QueryCriterion[0];
      }
    }

    private static string buildPredefinedPartnerString(
      User user,
      string queryName,
      string[] queryParameters,
      bool idsOnly)
    {
      RelatedLoanMatchType matchType = RelatedLoanMatchType.None;
      QueryCriterion[] criteria;
      switch (queryName)
      {
        case "Imported or Newly Created Business Contacts":
          StringValueCriterion stringValueCriterion = new StringValueCriterion("History.EventType", "First Contact", StringMatchType.Exact);
          int int32 = Convert.ToInt32(queryParameters[0]);
          string fieldName = "History.TimeOfHistory";
          DateTime today = DateTime.Today;
          DateTime dateTime = DateTime.Today.AddDays((double) -int32);
          DateValueCriterion dateValueCriterion1 = new DateValueCriterion(fieldName, dateTime, OrdinalMatchType.GreaterThanOrEquals, DateMatchPrecision.Day);
          DateValueCriterion dateValueCriterion2 = new DateValueCriterion(fieldName, today, OrdinalMatchType.LessThanOrEquals, DateMatchPrecision.Day);
          criteria = new QueryCriterion[3]
          {
            (QueryCriterion) stringValueCriterion,
            (QueryCriterion) dateValueCriterion1,
            (QueryCriterion) dateValueCriterion2
          };
          break;
        case "Business Contact Category":
          criteria = new QueryCriterion[1]
          {
            (QueryCriterion) new OrdinalValueCriterion("Contact.CategoryID", (object) Convert.ToInt32(queryParameters[0]), OrdinalMatchType.Equals)
          };
          break;
        default:
          return string.Empty;
      }
      return idsOnly ? BizPartnerContact.getQueryBizPartnerIdsSql(user.UserInfo, criteria, matchType, (SortField[]) null) : BizPartnerContact.getQueryBizPartnerSummariesSql(user.UserInfo, criteria, matchType, (SortField[]) null);
    }

    private static string encodeEnumArray(Type enumType, Array enumArray)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < enumArray.Length; ++index)
        stringBuilder.Append(Enum.Format(enumType, enumArray.GetValue(index), "D") + ",");
      --stringBuilder.Length;
      return stringBuilder.ToString();
    }

    private static ContactGroupInfo convertRowToGroup(DataRow groupRow)
    {
      return new ContactGroupInfo((int) groupRow["GroupId"], (string) groupRow["UserId"], (ContactType) groupRow["ContactType"], (ContactGroupType) groupRow["GroupType"], (string) groupRow["GroupName"], EllieMae.EMLite.DataAccess.SQL.Decode(groupRow["GroupDesc"], (object) "").ToString(), (DateTime) groupRow["CreationTime"], (int[]) null);
    }

    private static ContactQueryInfo convertRowToQuery(DataRow queryRow)
    {
      return new ContactQueryInfo((int) queryRow["QueryId"], (string) queryRow["UserId"], (ContactType) queryRow["ContactType"], (ContactQueryType) queryRow["QueryType"], (string) queryRow["QueryName"], EllieMae.EMLite.DataAccess.SQL.Decode(queryRow["QueryDesc"], (object) "").ToString(), EllieMae.EMLite.DataAccess.SQL.Decode(queryRow["XmlQueryString"], (object) "").ToString(), (bool) EllieMae.EMLite.DataAccess.SQL.Decode(queryRow["PrimaryOnly"], (object) false));
    }

    private static DbValueList createDbValueList(ContactGroupInfo groupInfo)
    {
      return new DbValueList()
      {
        {
          "UserId",
          (object) groupInfo.UserId,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "ContactType",
          (object) (int) groupInfo.ContactType
        },
        {
          "GroupType",
          (object) (int) groupInfo.GroupType
        },
        {
          "GroupName",
          (object) groupInfo.GroupName,
          (IDbEncoder) DbEncoding.Default
        },
        {
          "GroupDesc",
          (object) groupInfo.GroupDesc
        }
      };
    }

    private static DbValueList createDbValueList(ContactQueryInfo queryInfo)
    {
      return new DbValueList()
      {
        {
          "UserId",
          (object) queryInfo.UserId,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "ContactType",
          (object) (int) queryInfo.ContactType
        },
        {
          "QueryType",
          (object) (int) queryInfo.QueryType
        },
        {
          "QueryName",
          (object) queryInfo.QueryName,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "QueryDesc",
          (object) queryInfo.QueryDesc
        },
        {
          "XmlQueryString",
          (object) queryInfo.XmlQueryString,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "PrimaryOnly",
          (object) queryInfo.PrimaryOnly,
          (IDbEncoder) DbEncoding.Flag
        }
      };
    }

    private static DbValueList createDbValueList(int contactId)
    {
      return new DbValueList()
      {
        {
          "GroupId",
          (object) "@contactGroupId",
          (IDbEncoder) DbEncoding.None
        },
        {
          "ContactId",
          (object) contactId
        }
      };
    }

    public class IncrementalIntArray
    {
      private int increment;
      public int Start;
      public int Count;
      public int[] IntArray;

      public IncrementalIntArray(int increment, int[] intArray)
      {
        this.increment = increment;
        this.IntArray = intArray;
        this.Reset();
      }

      public void Reset()
      {
        this.Start = 0;
        this.Count = Math.Min(this.increment, this.IntArray.Length - this.Start);
      }

      public bool Increment()
      {
        this.Start += this.Count;
        this.Count = Math.Min(this.increment, this.IntArray.Length - this.Start);
        return this.Count != 0;
      }
    }
  }
}
