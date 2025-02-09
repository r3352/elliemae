// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using com.elliemae.services.eventbus.models;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.RemotingServices;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server.ServiceObjects.KafkaEvent;
using EllieMae.EMLite.ServiceInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupAccessor
  {
    private const string className = "AclGroupAccessor�";
    private const string tableName_AclGroups = "[AclGroups]�";
    private const string tableName_AclGroupOrgRef = "[AclGroupOrgRef]�";
    private const string tableName_AclGroupUserRef = "[AclGroupUserRef]�";

    private AclGroupAccessor()
    {
    }

    public static AclGroup GetGroupByName(string name)
    {
      if (name == null || name.Trim() == "")
        throw new Exception("Group name cannot be null or empty");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from [AclGroups] where groupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name.Trim()));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count == 0)
        return (AclGroup) null;
      bool viewSubordContacts = false;
      if (dataRowCollection[0]["viewSubordContacts"] != DBNull.Value)
        viewSubordContacts = (byte) dataRowCollection[0]["viewSubordContacts"] == (byte) 1;
      AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
      if (dataRowCollection[0]["contactAccess"] != DBNull.Value)
        contactAccess = (byte) dataRowCollection[0]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
      return new AclGroup((int) dataRowCollection[0]["groupID"], dataRowCollection[0]["groupName"].ToString(), viewSubordContacts, contactAccess, (int) dataRowCollection[0]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["LastModifiedBy"]));
    }

    public static AclGroup GetGroupById(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from [AclGroups] where groupID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupId.ToString()));
      return AclGroupAccessor.PopulateUserGroupInfo(AclGroupAccessor.CastDataRowCollection(dbQueryBuilder.Execute()));
    }

    public static bool CheckIfGroupNameExist(string groupName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT TOP 1 groupName FROM [AclGroups] WHERE groupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupName.Trim()));
      return !string.IsNullOrEmpty(Convert.ToString(dbQueryBuilder.ExecuteScalar()));
    }

    private static AclGroup PopulateUserGroupInfo(DataRow[] usersRows)
    {
      if (usersRows == null || usersRows.Length == 0)
        return (AclGroup) null;
      bool viewSubordContacts = false;
      if (usersRows[0].Table.Columns.Contains("viewSubordContacts") && usersRows[0]["viewSubordContacts"] != DBNull.Value)
        viewSubordContacts = (byte) usersRows[0]["viewSubordContacts"] == (byte) 1;
      AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
      if (usersRows[0].Table.Columns.Contains("contactAccess") && usersRows[0]["contactAccess"] != DBNull.Value)
        contactAccess = (byte) usersRows[0]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
      return new AclGroup((int) usersRows[0]["groupID"], usersRows[0]["groupName"].ToString(), viewSubordContacts, contactAccess, (int) usersRows[0]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(usersRows[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(usersRows[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(usersRows[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(usersRows[0]["LastModifiedBy"]));
    }

    public static UserGroupDetails GetUserGroupDetails(int groupID, UserGroupCoreEntity entities = UserGroupCoreEntity.Summary)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      int[] numArray = new int[1]{ groupID };
      dbQueryBuilder.AppendLine(AclGroupAccessor.GetUserGroupDetailsQuery(entities, (string) null, numArray));
      List<UserGroupDetails> source = AclGroupAccessor.PopulateUserGroupDetailsData(dbQueryBuilder.ExecuteSetQuery(), ((IEnumerable<int>) numArray).ToList<int>());
      return source != null && source.Any<UserGroupDetails>() && source[0].GroupInfo != (AclGroup) null ? source.FirstOrDefault<UserGroupDetails>() : (UserGroupDetails) null;
    }

    public static bool CreateUserGroup(
      UserGroupDetails userGroupDetails,
      string loggedInUserId,
      bool returnEntityData,
      out int newGroupId,
      out UserGroupDetails responseData)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(AclGroupAccessor.CreateUserGroupQuery(userGroupDetails, loggedInUserId));
      int groupId;
      bool userGroup;
      if (returnEntityData)
      {
        dbQueryBuilder.AppendLine(AclGroupAccessor.GetUserGroupDetailsQuery(UserGroupCoreEntity.All, "@groupID", (int[]) null));
        DataSet ds = dbQueryBuilder.ExecuteSetQuery();
        dbQueryBuilder.Reset();
        if (ds != null && ds.Tables.Count > 0)
        {
          groupId = Utils.ParseInt((object) ds.Tables[0].Rows[0][nameof (newGroupId)].ToString());
          ds.Tables.RemoveAt(0);
          int[] source1 = new int[1]{ groupId };
          List<UserGroupDetails> source2 = AclGroupAccessor.PopulateUserGroupDetailsData(ds, ((IEnumerable<int>) source1).ToList<int>());
          responseData = source2 == null || !source2.Any<UserGroupDetails>() || !(source2[0].GroupInfo != (AclGroup) null) ? (UserGroupDetails) null : source2.FirstOrDefault<UserGroupDetails>();
          newGroupId = groupId;
          userGroup = true;
        }
        else
        {
          responseData = (UserGroupDetails) null;
          newGroupId = -1;
          return false;
        }
      }
      else
      {
        groupId = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        newGroupId = groupId;
        responseData = (UserGroupDetails) null;
        userGroup = true;
      }
      List<MemberContract> memberContractList1 = new List<MemberContract>();
      foreach (OrgInGroup memberOrganization in userGroupDetails.MemberOrganizations)
      {
        List<MemberContract> memberContractList2 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = memberOrganization.OrgID.ToString();
        memberContract.EntityType = EntityRefTypeContract.Organization;
        memberContract.IsRecursive = memberOrganization.IsInclusive;
        memberContractList2.Add(memberContract);
      }
      foreach (UserGroupMemberUser memberUser in userGroupDetails.MemberUsers)
      {
        List<MemberContract> memberContractList3 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = memberUser.UserId;
        memberContract.EntityType = EntityRefTypeContract.User;
        memberContractList3.Add(memberContract);
      }
      AclGroupAccessor.PublishUserGroupKafkaEvent("create", loggedInUserId, groupId, new MembersContract()
      {
        Added = (IEnumerable<MemberContract>) memberContractList1
      });
      return userGroup;
    }

    private static string CreateUserGroupQuery(
      UserGroupDetails userGroupDetails,
      string loggedInUserId)
    {
      string str = (userGroupDetails.GroupInfo.Name ?? "").Trim();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DateTime utcNow = DateTime.UtcNow;
      dbQueryBuilder.AppendLine("DECLARE @groupID INT;");
      dbQueryBuilder.AppendLine("DECLARE @organizationId INT;");
      dbQueryBuilder.AppendLine("DECLARE @fileId INT;");
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO [AclGroups] (groupName, viewSubordContacts, contactAccess, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy) VALUES({0}, 1, 0, {1}, {2}, {3}, {4})", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) str), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(utcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUserId), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(utcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUserId)));
      dbQueryBuilder.SelectIdentity("@groupId");
      foreach (UserGroupMemberUser memberUser in userGroupDetails.MemberUsers)
        dbQueryBuilder.AppendLine("INSERT INTO [AclGroupUserRef] (groupID, userid) VALUES(@groupId, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) memberUser.UserId) + ")");
      foreach (OrgInGroup memberOrganization in userGroupDetails.MemberOrganizations)
      {
        dbQueryBuilder.AppendLine(AclGroupAccessor.GetOrgIdQuery(memberOrganization));
        dbQueryBuilder.AppendLine("IF(@organizationId IS NOT NULL)");
        dbQueryBuilder.Begin();
        dbQueryBuilder.AppendLine("INSERT INTO [AclGroupOrgRef] (groupID, orgID, recursive) VALUES(@groupId, @organizationId, " + (object) (memberOrganization.IsInclusive ? 1 : 0) + ")");
        dbQueryBuilder.End();
      }
      if (userGroupDetails.LoanOrganizations != null)
      {
        foreach (OrgInGroupLoan loanOrganization in userGroupDetails.LoanOrganizations)
        {
          dbQueryBuilder.AppendLine(AclGroupAccessor.GetOrgIdQuery((OrgInGroup) loanOrganization));
          dbQueryBuilder.AppendLine("IF(@organizationId IS NOT NULL)");
          dbQueryBuilder.Begin();
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanOrgRef] (groupID, orgID, recursive, access) values (@groupId, @organizationId, " + (object) (loanOrganization.IsInclusive ? 1 : 0) + ", " + (object) (int) loanOrganization.Access + ")");
          dbQueryBuilder.End();
        }
      }
      if (userGroupDetails.LoanUsers != null)
      {
        foreach (UserGroupLoanUser loanUser in userGroupDetails.LoanUsers)
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanUserRef] (groupID, userid, access) values (@groupId, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) loanUser.UserId) + ", " + (object) (int) loanUser.Access + ")");
      }
      if (userGroupDetails.AccessibleFolders != null)
      {
        foreach (string accessibleFolder in userGroupDetails.AccessibleFolders)
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanFolderAccess] (groupID, folderName, access) values (@groupId, " + EllieMae.EMLite.DataAccess.SQL.Encode((object) accessibleFolder) + ", " + EllieMae.EMLite.DataAccess.SQL.EncodeFlag(true) + ")");
      }
      if (userGroupDetails.LoanTemplates != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForAllLoanTemplates(userGroupDetails.LoanTemplates));
      dbQueryBuilder.AppendLine("SELECT @groupId AS newGroupId");
      return dbQueryBuilder.ToString();
    }

    private static UserGroupLoanTemplates PopulateAclGroupLoanTemplates(DataRow[] rows)
    {
      if (rows == null)
        return new UserGroupLoanTemplates();
      Dictionary<int, IEnumerable<DataRow>> dictionary = new Dictionary<int, IEnumerable<DataRow>>();
      UserGroupLoanTemplates groupLoanTemplates = new UserGroupLoanTemplates();
      Dictionary<int, IEnumerable<DataRow>> dataRows = AclGroupAccessor.GetDataRows(rows);
      if (!dataRows.Any<KeyValuePair<int, IEnumerable<DataRow>>>())
        return new UserGroupLoanTemplates();
      IEnumerable<DataRow> rows1;
      if (dataRows.TryGetValue(1, out rows1))
        groupLoanTemplates.LoanPrograms = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(2, out rows1))
        groupLoanTemplates.ClosingCosts = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(5, out rows1))
        groupLoanTemplates.DocumentSets = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(4, out rows1))
        groupLoanTemplates.InputFormSets = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(3, out rows1))
        groupLoanTemplates.DataTemplates = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(6, out rows1))
        groupLoanTemplates.LoanTemplateSets = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(17, out rows1))
        groupLoanTemplates.TaskSets = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(19, out rows1))
        groupLoanTemplates.SettlementServiceProviders = AclGroupAccessor.GetUserResourceDetail(rows1);
      if (dataRows.TryGetValue(20, out rows1))
        groupLoanTemplates.AffiliatedBusinessArrangements = AclGroupAccessor.GetUserResourceDetail(rows1);
      return groupLoanTemplates;
    }

    private static Dictionary<int, IEnumerable<DataRow>> GetDataRows(DataRow[] rowCollection)
    {
      return rowCollection != null ? ((IEnumerable<DataRow>) rowCollection).GroupBy<DataRow, int>((System.Func<DataRow, int>) (d => Convert.ToInt32(d["FileType"]))).ToDictionary<IGrouping<int, DataRow>, int, IEnumerable<DataRow>>((System.Func<IGrouping<int, DataRow>, int>) (g => g.Key), (System.Func<IGrouping<int, DataRow>, IEnumerable<DataRow>>) (g => g.AsEnumerable<DataRow>())) : new Dictionary<int, IEnumerable<DataRow>>();
    }

    private static UserGroupResourceDetail[] GetUserResourceDetail(IEnumerable<DataRow> rows)
    {
      List<UserGroupResourceDetail> groupResourceDetailList = new List<UserGroupResourceDetail>();
      foreach (DataRow row in rows)
      {
        UserGroupResourceDetail groupResourceDetail1 = new UserGroupResourceDetail();
        groupResourceDetail1.FileID = (int) row["FileID"];
        groupResourceDetail1.FileType = (AclFileType) row["FileType"];
        groupResourceDetail1.FilePath = (string) row["FilePath"];
        groupResourceDetail1.IsFolder = (bool) row["IsFolder"];
        groupResourceDetail1.Owner = row["Owner"] != null ? (string) row["Owner"] : (string) null;
        groupResourceDetail1.Access = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), row["access"].ToString());
        UserGroupResourceDetail groupResourceDetail2 = groupResourceDetail1;
        groupResourceDetailList.Add(groupResourceDetail2);
      }
      return groupResourceDetailList.ToArray();
    }

    private static string CreateQueryForAllLoanTemplates(
      UserGroupLoanTemplates userGroupLoanTemplates)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (userGroupLoanTemplates.LoanPrograms != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.LoanPrograms, AclFileType.LoanProgram));
      if (userGroupLoanTemplates.ClosingCosts != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.ClosingCosts, AclFileType.ClosingCost));
      if (userGroupLoanTemplates.DocumentSets != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.DocumentSets, AclFileType.DocumentSet));
      if (userGroupLoanTemplates.InputFormSets != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.InputFormSets, AclFileType.FormList));
      if (userGroupLoanTemplates.DataTemplates != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.DataTemplates, AclFileType.MiscData));
      if (userGroupLoanTemplates.LoanTemplateSets != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.LoanTemplateSets, AclFileType.LoanTemplate));
      if (userGroupLoanTemplates.TaskSets != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.TaskSets, AclFileType.TaskSet));
      if (userGroupLoanTemplates.SettlementServiceProviders != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.SettlementServiceProviders, AclFileType.SettlementServiceProviders));
      if (userGroupLoanTemplates.AffiliatedBusinessArrangements != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.CreateQueryForLoanTemplates(userGroupLoanTemplates.AffiliatedBusinessArrangements, AclFileType.AffiliatedBusinessArrangements));
      return dbQueryBuilder.ToString();
    }

    private static string CreateQueryForLoanTemplates(
      UserGroupResourceDetail[] userGroupResources,
      AclFileType type)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      foreach (UserGroupResourceDetail userGroupResource in userGroupResources)
      {
        string str = AclGroupAccessor.CheckFileResourceExists(userGroupResource.FilePath, (int) type);
        if (str == null)
        {
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO [FileResource] (FileType, IsFolder,Owner,FilePath) VALUES ({0},{1},{2},{3})", (object) (int) type, userGroupResource.IsFolder ? (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(true) : (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(false), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userGroupResource.Owner), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) userGroupResource.FilePath)));
          dbQueryBuilder.SelectIdentity("@fileId");
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO [AclGroupFileRef] (groupID, fileID, inclusive,Access) VALUES(@groupId, @fileId, {0}, {1} )", userGroupResource.IsFolder ? (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(true) : (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(false), (object) (int) userGroupResource.Access));
        }
        else
          dbQueryBuilder.AppendLine(string.Format("INSERT INTO [AclGroupFileRef] (groupID, fileID, inclusive,Access) VALUES(@groupId, {0}, {1}, {2} )", (object) str, userGroupResource.IsFolder ? (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(true) : (object) EllieMae.EMLite.DataAccess.SQL.EncodeFlag(false), (object) (int) userGroupResource.Access));
      }
      return dbQueryBuilder.ToString();
    }

    private static string CheckFileResourceExists(string path, int fileType)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("Select fileID from FileResource where FilePath = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) path) + "and FileType =" + (object) fileType);
      dbQueryBuilder.End();
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count > 0 ? string.Concat(dataRowCollection[0][0]) : (string) null;
    }

    private static string GetOrgIdQuery(OrgInGroup org)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SET @organizationId = NULL");
      dbQueryBuilder.Begin();
      if (string.Equals(org.Type, "External", StringComparison.InvariantCultureIgnoreCase))
        dbQueryBuilder.AppendLine("SELECT @organizationId = org_chart_id FROM ExternalOriginatorManagement WHERE oid = " + (object) org.OrgID);
      else
        dbQueryBuilder.AppendLine("SET @organizationId = " + (object) org.OrgID);
      dbQueryBuilder.End();
      return dbQueryBuilder.ToString();
    }

    public static AclGroup[] GetAllGroups()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from [AclGroups] order by displayOrder");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (AclGroup[]) null;
      AclGroup[] allGroups = new AclGroup[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        bool viewSubordContacts = false;
        if (dataRowCollection[0]["viewSubordContacts"] != DBNull.Value)
          viewSubordContacts = (byte) dataRowCollection[0]["viewSubordContacts"] == (byte) 1;
        AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
        if (dataRowCollection[0]["contactAccess"] != DBNull.Value)
          contactAccess = (byte) dataRowCollection[0]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        allGroups[index] = new AclGroup((int) dataRowCollection[index]["groupID"], dataRowCollection[index]["groupName"].ToString(), viewSubordContacts, contactAccess, (int) dataRowCollection[index]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["LastModifiedBy"]));
      }
      return allGroups;
    }

    public static AclGroup[] GetGroups(int[] groupIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from [AclGroups] where GroupID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) groupIds) + ") order by displayOrder");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (AclGroup[]) null;
      AclGroup[] groups = new AclGroup[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
      {
        bool viewSubordContacts = false;
        if (dataRowCollection[0]["viewSubordContacts"] != DBNull.Value)
          viewSubordContacts = (byte) dataRowCollection[0]["viewSubordContacts"] == (byte) 1;
        AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
        if (dataRowCollection[0]["contactAccess"] != DBNull.Value)
          contactAccess = (byte) dataRowCollection[0]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        groups[index] = new AclGroup((int) dataRowCollection[index]["groupID"], dataRowCollection[index]["groupName"].ToString(), viewSubordContacts, contactAccess, (int) dataRowCollection[index]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["LastModifiedBy"]));
      }
      return groups;
    }

    public static AclGroup CreateGroup(
      AclGroup group,
      string loggedInUserId,
      bool isFromVersionMigration = false)
    {
      group.Name = (group.Name ?? "").Trim();
      if (AclGroupAccessor.CheckIfGroupNameExist(group.Name))
        throw new Exception(group.Name + ": group already exists");
      DbTableInfo table = DbAccessManager.GetTable("[AclGroups]");
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbValue key = new DbValue("groupName", (object) group.Name);
      group.CreatedBy = string.IsNullOrWhiteSpace(group.CreatedBy) ? loggedInUserId : group.CreatedBy;
      group.CreatedDate = new DateTime?(group.CreatedDate ?? DateTime.UtcNow);
      DbValueList values = new DbValueList()
      {
        key,
        {
          "viewSubordContacts",
          (object) (group.ViewSubordinatesContacts ? 1 : 0)
        },
        {
          "displayOrder",
          (object) group.DisplayOrder
        },
        {
          "contactAccess",
          (object) (int) group.ContactAccess
        },
        {
          "CreatedDate",
          (object) group.CreatedDate
        },
        {
          "CreatedBy",
          (object) group.CreatedBy
        },
        {
          "LastModifiedDate",
          (object) group.CreatedDate
        },
        {
          "LastModifiedBy",
          (object) group.CreatedBy
        }
      };
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.SelectFrom(table, key);
      DataRowCollection source = dbQueryBuilder.Execute(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
      if (!isFromVersionMigration)
        AclGroupAccessor.PublishUserGroupKafkaEvent("create", group.CreatedBy, (int) source[0]["groupID"], (MembersContract) null);
      return AclGroupAccessor.DatabaseRowsToUserGroups(source.Cast<DataRow>().ToArray<DataRow>())[0];
    }

    public static AclGroup CreateGroup(
      string name,
      string loggedInUserId,
      bool isFromVersionMigration = false)
    {
      name = (name ?? "").Trim();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select groupID from [AclGroups] where groupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
        throw new Exception(name + ": group already exists");
      dbQueryBuilder.Reset();
      DateTime utcNow = DateTime.UtcNow;
      dbQueryBuilder.AppendLine(string.Format("INSERT INTO [AclGroups] (groupName, viewSubordContacts, contactAccess, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy) VALUES({0}, 0, 0, {1}, {2}, {3}, {4})", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) name), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(utcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUserId), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(utcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUserId)));
      dbQueryBuilder.AppendLine("select * from [AclGroups] where groupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) name));
      DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
      if (!isFromVersionMigration)
        AclGroupAccessor.PublishUserGroupKafkaEvent("create", loggedInUserId, (int) dataRowCollection2[0]["groupID"], (MembersContract) null);
      return new AclGroup((int) dataRowCollection2[0]["groupID"], name, false, AclResourceAccess.ReadOnly, (int) dataRowCollection2[0]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection2[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection2[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection2[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection2[0]["LastModifiedBy"]));
    }

    public static void DeleteGroup(int groupID, string loggedInUserId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select * from [AclGroupOrgRef] where groupId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupID));
      dbQueryBuilder.Append("select * from [AclGroupUserRef] where groupId = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupID));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      List<MemberContract> source = new List<MemberContract>();
      if (dataSet != null)
      {
        int? count = dataSet.Tables?.Count;
        int num = 0;
        if (count.GetValueOrDefault() > num & count.HasValue)
        {
          foreach (DataRow dataRow in dataSet.Tables[0].Rows.Cast<DataRow>())
          {
            List<MemberContract> memberContractList = source;
            MemberContract memberContract = new MemberContract();
            memberContract.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["orgID"]);
            memberContract.EntityType = EntityRefTypeContract.Organization;
            memberContract.IsRecursive = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["recursive"]);
            memberContractList.Add(memberContract);
          }
          DataTable table = dataSet.Tables[1];
          foreach (DataRow dataRow in table != null ? table.Rows.Cast<DataRow>() : (IEnumerable<DataRow>) null)
          {
            List<MemberContract> memberContractList = source;
            MemberContract memberContract = new MemberContract();
            memberContract.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["userid"]);
            memberContract.EntityType = EntityRefTypeContract.User;
            memberContractList.Add(memberContract);
          }
        }
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.Append("delete from [AclGroups] where groupID = " + (object) groupID);
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
      MembersContract members = new MembersContract()
      {
        Deleted = source.Any<MemberContract>() ? (IEnumerable<MemberContract>) source : (IEnumerable<MemberContract>) null
      };
      AclGroupAccessor.PublishUserGroupKafkaEvent("delete", loggedInUserId, groupID, members);
    }

    public static void RenameGroup(int groupID, string newName, string loggedInUserId)
    {
      newName = (newName ?? "").Trim();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(string.Format("update[AclGroups] set groupName = {0}, LastModifiedDate = {1}, LastModifiedBy = {2}", (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) newName), (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.UtcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUserId)));
      dbQueryBuilder.AppendLine("\twhere groupID = " + (object) groupID);
      dbQueryBuilder.ExecuteNonQuery();
      AclGroupAccessor.PublishUserGroupKafkaEvent("update", loggedInUserId, groupID, (MembersContract) null);
    }

    public static void UpdateGroup(
      AclGroup group,
      string loggedInUserId,
      bool isFromVersionMigration = false)
    {
      if (group == (AclGroup) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [AclGroups] set GroupName = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) group.Name) + ", viewSubordContacts = " + (object) (group.ViewSubordinatesContacts ? 1 : 0) + ", displayOrder = " + (object) group.DisplayOrder + ", contactAccess = " + (group.ContactAccess == AclResourceAccess.ReadWrite ? (object) "1" : (object) "0") + " where groupID = " + (object) group.ID);
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.Default);
      if (isFromVersionMigration)
        return;
      AclGroupAccessor.PublishUserGroupKafkaEvent("update", loggedInUserId, group.ID, (MembersContract) null);
    }

    public static (List<MemberContract>, List<MemberContract>) UpdateUsersInGroup(
      int groupID,
      string[] resetUserList,
      string[] newUserList,
      string loggedInUser)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      if (newUserList != null)
      {
        foreach (string newUser in newUserList)
          dictionary[newUser] = newUser;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("[AclGroupUserRef]");
      List<MemberContract> memberContractList1 = new List<MemberContract>();
      List<MemberContract> memberContractList2 = new List<MemberContract>();
      if (resetUserList != null && resetUserList.Length != 0)
      {
        foreach (string resetUser in resetUserList)
        {
          if (!dictionary.ContainsKey(resetUser))
          {
            dbQueryBuilder.DeleteFrom(table, new DbValueList()
            {
              {
                nameof (groupID),
                (object) groupID
              },
              {
                "userid",
                (object) resetUser
              }
            });
            List<MemberContract> memberContractList3 = memberContractList1;
            MemberContract memberContract = new MemberContract();
            memberContract.EntityId = resetUser.ToString();
            memberContract.EntityType = EntityRefTypeContract.User;
            memberContractList3.Add(memberContract);
          }
        }
      }
      if (newUserList != null && newUserList.Length != 0)
      {
        dbQueryBuilder.AppendLine("Create table #Temptable");
        dbQueryBuilder.AppendLine("(");
        dbQueryBuilder.AppendLine("    groupId int,");
        dbQueryBuilder.AppendLine("    userId varchar(16),");
        dbQueryBuilder.AppendLine("    operation varchar(15)");
        dbQueryBuilder.AppendLine(")");
        foreach (string newUser in newUserList)
        {
          DbValueList dbValueList = new DbValueList();
          dbValueList.Add(nameof (groupID), (object) groupID);
          dbValueList.Add("userid", (object) newUser);
          dbQueryBuilder.IfNotExists(table, dbValueList);
          dbQueryBuilder.Begin();
          dbQueryBuilder.InsertInto(table, dbValueList, true, false);
          dbQueryBuilder.AppendLine(string.Format("Insert into #Temptable values ({0}, '{1}', 'insert')", (object) groupID, (object) newUser));
          dbQueryBuilder.End();
        }
      }
      if (dbQueryBuilder.ToString() != "")
      {
        if (loggedInUser != null)
          dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
        if (newUserList != null && newUserList.Length != 0)
        {
          dbQueryBuilder.AppendLine("select * from #Temptable");
          dbQueryBuilder.AppendLine("drop table #Temptable");
        }
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          int? count = dataTable.Rows?.Count;
          int num = 0;
          if (count.GetValueOrDefault() > num & count.HasValue)
          {
            foreach (DataRow dataRow in dataTable.Rows.Cast<DataRow>())
            {
              if (EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["operation"]) == "insert")
              {
                List<MemberContract> memberContractList4 = memberContractList2;
                MemberContract memberContract = new MemberContract();
                memberContract.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["userId"]);
                memberContract.EntityType = EntityRefTypeContract.User;
                memberContractList4.Add(memberContract);
              }
            }
          }
        }
      }
      return (memberContractList2, memberContractList1);
    }

    public static (List<MemberContract>, List<MemberContract>, List<MemberContract>) UpdateOrgsInGroup(
      int groupID,
      int[] resetOrgList,
      int[] newOrgList,
      int[] newInclusiveOrgList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("[AclGroupOrgRef]");
      Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
      if (newOrgList != null)
      {
        foreach (int newOrg in newOrgList)
          dictionary[newOrg] = false;
      }
      if (newInclusiveOrgList != null)
      {
        foreach (int newInclusiveOrg in newInclusiveOrgList)
          dictionary[newInclusiveOrg] = true;
      }
      if (dictionary.Count > 0 || resetOrgList.Length != 0)
      {
        dbQueryBuilder.AppendLine("Create table #Temptable");
        dbQueryBuilder.AppendLine("(");
        dbQueryBuilder.AppendLine("    groupId int,");
        dbQueryBuilder.AppendLine("    orgId int,");
        dbQueryBuilder.AppendLine("    operation varchar(15),");
        dbQueryBuilder.AppendLine("    isRecursive bit");
        dbQueryBuilder.AppendLine(")");
      }
      if (resetOrgList != null && resetOrgList.Length != 0)
      {
        foreach (int resetOrg in resetOrgList)
        {
          if (!dictionary.ContainsKey(resetOrg))
          {
            dbQueryBuilder.AppendLine(string.Format("Insert into #Temptable values({0}, {1}, 'delete', (select recursive from [AclGroupOrgRef] where groupId = {2} and orgId = {3}))", (object) groupID, (object) resetOrg, (object) groupID, (object) resetOrg));
            dbQueryBuilder.DeleteFrom(table, new DbValueList()
            {
              {
                nameof (groupID),
                (object) groupID
              },
              {
                "orgID",
                (object) resetOrg
              }
            });
          }
        }
      }
      List<MemberContract> memberContractList1 = new List<MemberContract>();
      List<MemberContract> memberContractList2 = new List<MemberContract>();
      List<MemberContract> memberContractList3 = new List<MemberContract>();
      foreach (int key in dictionary.Keys)
      {
        DbValueList dbValueList1 = new DbValueList();
        dbValueList1.Add(nameof (groupID), (object) groupID);
        dbValueList1.Add("orgID", (object) key);
        DbValue dbValue = new DbValue("recursive", (object) dictionary[key], (IDbEncoder) DbEncoding.Flag);
        DbValueList dbValueList2 = new DbValueList();
        dbValueList2.Add(dbValueList1);
        dbValueList2.Add(dbValue);
        dbQueryBuilder.IfNotExists(table, dbValueList1);
        dbQueryBuilder.Begin();
        dbQueryBuilder.InsertInto(table, dbValueList2, true, false);
        dbQueryBuilder.AppendLine(string.Format("Insert into #Temptable values({0}, {1}, 'insert', '{2}')", (object) groupID, (object) key, (object) dictionary[key]));
        dbQueryBuilder.End();
        dbQueryBuilder.Else();
        dbQueryBuilder.Begin();
        dbQueryBuilder.IfNotExists(table, dbValueList2);
        dbQueryBuilder.Begin();
        dbQueryBuilder.Update(table, new DbValueList(new DbValue[1]
        {
          dbValue
        }), dbValueList1);
        dbQueryBuilder.AppendLine(string.Format("Insert into #Temptable values({0}, {1}, 'update', '{2}')", (object) groupID, (object) key, (object) dictionary[key]));
        dbQueryBuilder.End();
        dbQueryBuilder.End();
      }
      if (dbQueryBuilder.ToString() != "")
      {
        if (loggedInUser != null)
          dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
        if (dictionary.Count > 0 || resetOrgList.Length != 0)
        {
          dbQueryBuilder.AppendLine("select * from #Temptable");
          dbQueryBuilder.AppendLine("drop table #Temptable");
        }
        DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
        if (dataTable != null)
        {
          int? count = dataTable.Rows?.Count;
          int num = 0;
          if (count.GetValueOrDefault() > num & count.HasValue)
          {
            foreach (DataRow dataRow in dataTable.Rows.Cast<DataRow>())
            {
              switch (EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["operation"]))
              {
                case "insert":
                  List<MemberContract> memberContractList4 = memberContractList2;
                  MemberContract memberContract1 = new MemberContract();
                  memberContract1.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["orgId"]);
                  memberContract1.EntityType = EntityRefTypeContract.Organization;
                  memberContract1.IsRecursive = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isRecursive"]);
                  memberContractList4.Add(memberContract1);
                  continue;
                case "update":
                  List<MemberContract> memberContractList5 = memberContractList1;
                  MemberContract memberContract2 = new MemberContract();
                  memberContract2.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["orgId"]);
                  memberContract2.EntityType = EntityRefTypeContract.Organization;
                  memberContract2.IsRecursive = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isRecursive"]);
                  memberContractList5.Add(memberContract2);
                  continue;
                case "delete":
                  List<MemberContract> memberContractList6 = memberContractList3;
                  MemberContract memberContract3 = new MemberContract();
                  memberContract3.EntityId = EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRow["orgId"]);
                  memberContract3.EntityType = EntityRefTypeContract.Organization;
                  memberContract3.IsRecursive = EllieMae.EMLite.DataAccess.SQL.DecodeBoolean(dataRow["isRecursive"]);
                  memberContractList6.Add(memberContract3);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
      }
      return (memberContractList2, memberContractList1, memberContractList3);
    }

    public static void AddUserToGroup(int groupID, string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupUserRef] where groupID = " + (object) groupID + " and userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupUserRef] (groupID, userid) values (" + (object) groupID + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid) + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
    }

    public static void AddOrgToGroup(int groupID, int orgID, bool recursive)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgID);
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupOrgRef] (groupID, orgID, recursive) values (" + (object) groupID + ", " + (object) orgID + ", " + (object) (recursive ? 1 : 0) + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
    }

    public static void DeleteUserFromGroup(int groupID, string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupUserRef] where groupID = " + (object) groupID + " and userid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
    }

    public static void DeleteOrgFromGroup(int groupID, int orgID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgID);
      dbQueryBuilder.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
    }

    [PgReady]
    public static AclGroup[] GetGroupsOfUser(string userid)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select AclGroups.* from AclGroups");
        pgDbQueryBuilder.AppendLine("   inner join AclGroupMembers on AclGroups.GroupID = AclGroupMembers.GroupID");
        pgDbQueryBuilder.AppendLine("where AclGroupMembers.UserID = @userid");
        DbCommandParameter parameter = new DbCommandParameter(nameof (userid), (object) userid.TrimEnd(), DbType.AnsiString);
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(parameter);
        if (dataRowCollection == null)
          return (AclGroup[]) null;
        AclGroup[] groupsOfUser = new AclGroup[dataRowCollection.Count];
        for (int index = 0; index < groupsOfUser.Length; ++index)
        {
          bool viewSubordContacts = false;
          if (dataRowCollection[0]["viewSubordContacts"] != DBNull.Value)
            viewSubordContacts = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["viewSubordContacts"]) == 1;
          AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
          if (dataRowCollection[0]["contactAccess"] != DBNull.Value)
            contactAccess = EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[index]["contactAccess"]) == 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
          groupsOfUser[index] = new AclGroup((int) dataRowCollection[index]["groupID"], (string) dataRowCollection[index]["groupName"], viewSubordContacts, contactAccess, (int) dataRowCollection[index]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["LastModifiedBy"]));
        }
        return groupsOfUser;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select AclGroups.* from AclGroups");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers on AclGroups.GroupID = AclGroupMembers.GroupID");
      dbQueryBuilder.AppendLine("where AclGroupMembers.UserID = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) userid));
      return AclGroupAccessor.DatabaseRowsToUserGroups(dbQueryBuilder.Execute().Cast<DataRow>().ToArray<DataRow>());
    }

    public static AclGroup[] DatabaseRowsToUserGroups(DataRow[] rows)
    {
      if (rows == null)
        return (AclGroup[]) null;
      AclGroup[] userGroups = new AclGroup[rows.Length];
      for (int index = 0; index < userGroups.Length; ++index)
      {
        bool viewSubordContacts = false;
        if (rows[0]["viewSubordContacts"] != DBNull.Value)
          viewSubordContacts = (byte) rows[index]["viewSubordContacts"] == (byte) 1;
        AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
        if (rows[0]["contactAccess"] != DBNull.Value)
          contactAccess = (byte) rows[index]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        userGroups[index] = new AclGroup((int) rows[index]["groupID"], (string) rows[index]["groupName"], viewSubordContacts, contactAccess, (int) rows[index]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(rows[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(rows[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(rows[0]["LastModifiedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(rows[0]["LastModifiedBy"]));
      }
      return userGroups;
    }

    public static AclGroup[] GetGroupsOfOrganization(int orgID)
    {
      int[] ancestorsOfOrg = OrganizationStore.GetAncestorsOfOrg(orgID, true);
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [AclGroups] where groupID in");
      dbQueryBuilder.AppendLine("(");
      dbQueryBuilder.AppendLine("\t(select groupID from [AclGroupOrgRef] where groupid = 1  ");
      if (orgID == 0)
      {
        dbQueryBuilder.AppendLine("\t\tor (orgID = 0 ) ");
      }
      else
      {
        for (int index = 0; index < ancestorsOfOrg.Length; ++index)
          dbQueryBuilder.AppendLine("\t\tor (orgID = " + (object) ancestorsOfOrg[index] + " and recursive = 1)");
      }
      dbQueryBuilder.AppendLine("\tgroup by groupID)");
      dbQueryBuilder.AppendLine(") ");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null)
        return (AclGroup[]) null;
      AclGroup[] groupsOfOrganization = new AclGroup[dataRowCollection.Count];
      for (int index = 0; index < groupsOfOrganization.Length; ++index)
      {
        bool viewSubordContacts = false;
        if (dataRowCollection[0]["viewSubordContacts"] != DBNull.Value)
          viewSubordContacts = (byte) dataRowCollection[0]["viewSubordContacts"] == (byte) 1;
        AclResourceAccess contactAccess = AclResourceAccess.ReadOnly;
        if (dataRowCollection[0]["contactAccess"] != DBNull.Value)
          contactAccess = (byte) dataRowCollection[0]["contactAccess"] == (byte) 1 ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
        groupsOfOrganization[index] = new AclGroup((int) dataRowCollection[index]["groupID"], (string) dataRowCollection[index]["groupName"], viewSubordContacts, contactAccess, (int) dataRowCollection[index]["displayOrder"], new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["CreatedDate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["CreatedBy"]), new DateTime?(EllieMae.EMLite.DataAccess.SQL.DecodeDateTime(dataRowCollection[0]["LastModifieddate"])), EllieMae.EMLite.DataAccess.SQL.DecodeString(dataRowCollection[0]["LastModifiedBy"]));
      }
      return groupsOfOrganization;
    }

    public static OrgInGroup[] GetOrgsInGroup(int groupID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select a.*, b.org_name, b.org_type from AclGroupOrgRef a inner join org_chart b on a.orgID = b.oid where a.groupID = " + (object) groupID);
        return AclGroupAccessor.PopulateOrgsInGroup(AclGroupAccessor.CastDataRowCollection(dbQueryBuilder.Execute()));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
        return (OrgInGroup[]) null;
      }
    }

    public static (OrgInGroup[], UserGroupMemberUser[]) UpsertMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId,
      bool returnDetails,
      bool isUpdate)
    {
      UserGroupMemberUser[] userGroupMemberUserArray = new UserGroupMemberUser[0];
      OrgInGroup[] orgInGroupArray = new OrgInGroup[0];
      if (groupId <= 0)
        return (orgInGroupArray, userGroupMemberUserArray);
      DbQueryBuilder sql = new DbQueryBuilder();
      List<SqlParameter> sqlParameterList = new List<SqlParameter>();
      if (orgs.Any<OrgInGroup>() || userIds.Any<string>())
      {
        DataSet upsertMembersDataSet = AclGroupAccessor.GetUpsertMembersDataSet(groupId, orgs, userIds, sql);
        sql.AppendLine("EXEC UpsertUserGroupMembers @orgs, @users");
        sqlParameterList.Add(new SqlParameter("@orgs", (object) upsertMembersDataSet.Tables[0])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_UserGroupMembersOrgs"
        });
        sqlParameterList.Add(new SqlParameter("@users", (object) upsertMembersDataSet.Tables[1])
        {
          SqlDbType = SqlDbType.Structured,
          TypeName = "typ_UserGroupMembersUsers"
        });
      }
      using (SqlCommand sqlCmd = new SqlCommand())
      {
        if (returnDetails)
        {
          (List<int> orgIdList, List<int> extOrgIdList) = AclGroupAccessor.GetExternalAndInternalOrgs(orgs);
          sql.Append(AclGroupAccessor.GetMembersQuery(string.Format("({0})", (object) groupId), (IEnumerable<int>) orgIdList, (IEnumerable<int>) extOrgIdList, userIds));
        }
        sql.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(userId, groupId));
        sqlCmd.CommandText = sql.ToString();
        sqlCmd.Parameters.AddRange(sqlParameterList.ToArray());
        if (returnDetails)
        {
          DataSet dataSet = new DbAccessManager().ExecuteSetQuery((IDbCommand) sqlCmd);
          orgInGroupArray = orgs.Any<OrgInGroup>() ? AclGroupAccessor.PopulateOrgsInGroup(dataSet.Tables[0].Rows.Cast<DataRow>().ToArray<DataRow>()) : new OrgInGroup[0];
          userGroupMemberUserArray = userIds.Any<string>() ? AclGroupAccessor.PopulateUsersInGroup(dataSet.Tables[dataSet.Tables.Count - 1].Rows.Cast<DataRow>().ToArray<DataRow>()) : new UserGroupMemberUser[0];
        }
        else
          new DbAccessManager().ExecuteNonQuery((IDbCommand) sqlCmd);
      }
      List<MemberContract> memberContractList1 = new List<MemberContract>();
      foreach (OrgInGroup org in orgs)
      {
        List<MemberContract> memberContractList2 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = org.OrgID.ToString();
        memberContract.EntityType = EntityRefTypeContract.Organization;
        memberContract.IsRecursive = org.IsInclusive;
        memberContractList2.Add(memberContract);
      }
      foreach (string userId1 in userIds)
      {
        List<MemberContract> memberContractList3 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = userId1;
        memberContract.EntityType = EntityRefTypeContract.User;
        memberContractList3.Add(memberContract);
      }
      MembersContract members = new MembersContract();
      if (isUpdate)
        members.Updated = (IEnumerable<MemberContract>) memberContractList1;
      else
        members.Added = (IEnumerable<MemberContract>) memberContractList1;
      AclGroupAccessor.PublishUserGroupKafkaEvent("update", userId, groupId, members);
      return (orgInGroupArray, userGroupMemberUserArray);
    }

    public static void RemoveMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId)
    {
      if (groupId <= 0 || !orgs.Any<OrgInGroup>() && !userIds.Any<string>())
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (orgs.Any<OrgInGroup>())
      {
        (List<int> orgIdList, List<int> extOrgIdList) = AclGroupAccessor.GetExternalAndInternalOrgs(orgs);
        dbQueryBuilder.AppendLine("DELETE [AclGroupOrgRef]");
        dbQueryBuilder.AppendLine("FROM [AclGroupOrgRef] groupOrgs");
        dbQueryBuilder.AppendLine("INNER JOIN org_chart b ON b.oid = groupOrgs.orgID");
        dbQueryBuilder.AppendLine("LEFT JOIN ExternalOriginatorManagement e ON b.oid = e.org_chart_id");
        dbQueryBuilder.AppendLine(string.Format("WHERE groupOrgs.groupID = {0}{1}", (object) groupId, (object) AclGroupAccessor.GetOrgMemberConditionQuery((IEnumerable<int>) orgIdList, (IEnumerable<int>) extOrgIdList)));
      }
      if (userIds.Any<string>())
        dbQueryBuilder.AppendLine(string.Format("DELETE FROM {0} WHERE groupID = {1} AND userid in ({2})", (object) "[AclGroupUserRef]", (object) groupId, (object) string.Join(", ", userIds.Select<string, string>((System.Func<string, string>) (x => EllieMae.EMLite.DataAccess.SQL.Encode((object) x))))));
      dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(userId, groupId));
      dbQueryBuilder.ExecuteNonQuery();
      List<MemberContract> memberContractList1 = new List<MemberContract>();
      foreach (OrgInGroup org in orgs)
      {
        List<MemberContract> memberContractList2 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = org.OrgID.ToString();
        memberContract.EntityType = EntityRefTypeContract.Organization;
        memberContract.IsRecursive = org.IsInclusive;
        memberContractList2.Add(memberContract);
      }
      foreach (string userId1 in userIds)
      {
        List<MemberContract> memberContractList3 = memberContractList1;
        MemberContract memberContract = new MemberContract();
        memberContract.EntityId = userId1;
        memberContract.EntityType = EntityRefTypeContract.User;
        memberContractList3.Add(memberContract);
      }
      MembersContract members = new MembersContract()
      {
        Deleted = (IEnumerable<MemberContract>) memberContractList1
      };
      AclGroupAccessor.PublishUserGroupKafkaEvent("update", userId, groupId, members);
    }

    private static DataSet GetUpsertMembersDataSet(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      DbQueryBuilder sql)
    {
      DataSet upsertMembersDataSet = new DataSet();
      DataTable dataTable = new DataTable();
      dataTable.Columns.AddRange(new DataColumn[4]
      {
        new DataColumn("groupID", typeof (int)),
        new DataColumn("orgID", typeof (int)),
        new DataColumn("isExternal", typeof (bool)),
        new DataColumn("recursive", typeof (byte))
      });
      foreach (OrgInGroup org in orgs)
        dataTable.Rows.Add((object) groupId, (object) org.OrgID, (object) AclGroupAccessor.IsOrgTypeExternal(org.Type), (object) (org.IsInclusive ? 1 : 0));
      DataTable dtTable = new DataTable();
      dtTable.Columns.AddRange(new DataColumn[2]
      {
        new DataColumn("groupID", typeof (int)),
        new DataColumn("userid", typeof (string))
      });
      DbValueList values = new DbValueList();
      foreach (string userId in userIds)
      {
        values.Add(dtTable.Columns[0].ColumnName, (object) groupId);
        values.Add(dtTable.Columns[1].ColumnName, (object) userId);
        sql.InsertIntoDataTable(dtTable, DbAccessManager.GetTable("[AclGroupUserRef]"), values);
        values.Clear();
      }
      upsertMembersDataSet.Tables.AddRange(new DataTable[2]
      {
        dataTable,
        dtTable
      });
      return upsertMembersDataSet;
    }

    private static bool IsOrgTypeExternal(string type)
    {
      return string.Equals(type, "External", StringComparison.InvariantCultureIgnoreCase);
    }

    private static OrgInGroup[] PopulateOrgsInGroup(DataRow[] orgRows)
    {
      if (orgRows == null)
        return new OrgInGroup[0];
      OrgInGroup[] orgInGroupArray = new OrgInGroup[orgRows.Length];
      for (int index = 0; index < orgInGroupArray.Length; ++index)
      {
        orgInGroupArray[index] = new OrgInGroup();
        orgInGroupArray[index].OrgID = (int) orgRows[index]["orgID"];
        orgInGroupArray[index].IsInclusive = (byte) orgRows[index]["recursive"] == (byte) 1;
        orgInGroupArray[index].OrgName = (string) orgRows[index]["org_name"];
        orgInGroupArray[index].Type = orgRows[index]["org_type"] != DBNull.Value ? "External" : "Internal";
      }
      return orgInGroupArray;
    }

    private static UserGroupMemberUser[] PopulateUsersInGroup(DataRow[] userRows)
    {
      if (userRows == null)
        return new UserGroupMemberUser[0];
      UserGroupMemberUser[] userGroupMemberUserArray = new UserGroupMemberUser[userRows.Length];
      for (int index = 0; index < userGroupMemberUserArray.Length; ++index)
      {
        userGroupMemberUserArray[index] = new UserGroupMemberUser();
        userGroupMemberUserArray[index].UserId = (string) userRows[index]["userid"];
        userGroupMemberUserArray[index].FirstName = (string) userRows[index]["first_name"];
        userGroupMemberUserArray[index].MiddleName = userRows[index]["middle_name"] != DBNull.Value ? (string) userRows[index]["middle_name"] : string.Empty;
        userGroupMemberUserArray[index].LastName = (string) userRows[index]["last_name"];
        userGroupMemberUserArray[index].SuffixName = userRows[index]["suffix_name"] != DBNull.Value ? (string) userRows[index]["suffix_name"] : string.Empty;
        userGroupMemberUserArray[index].Login = !(bool) userRows[index]["locked"] ? UserInfo.UserStatusEnum.Enabled : UserInfo.UserStatusEnum.Disabled;
        userGroupMemberUserArray[index].Status = (int) userRows[index]["status"] == 0 ? UserInfo.UserStatusEnum.Enabled : UserInfo.UserStatusEnum.Disabled;
        userGroupMemberUserArray[index].UserType = userRows[index]["user_type"] == DBNull.Value || !string.Equals(userRows[index]["user_type"].ToString(), "External", StringComparison.CurrentCultureIgnoreCase) ? "Internal" : "External";
      }
      return userGroupMemberUserArray;
    }

    private static OrgInGroupLoan[] PopulateLoanOrgsInGroup(DataRow[] orgRows)
    {
      if (orgRows == null)
        return Enumerable.Empty<OrgInGroupLoan>().ToArray<OrgInGroupLoan>();
      OrgInGroupLoan[] orgInGroupLoanArray = new OrgInGroupLoan[orgRows.Length];
      for (int index = 0; index < orgInGroupLoanArray.Length; ++index)
      {
        AclResourceAccess aclResourceAccess = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), orgRows[index]["access"].ToString());
        orgInGroupLoanArray[index] = new OrgInGroupLoan();
        orgInGroupLoanArray[index].OrgID = orgRows[index]["orgID"] != DBNull.Value ? (int) orgRows[index]["orgID"] : 0;
        orgInGroupLoanArray[index].IsInclusive = (byte) orgRows[index]["recursive"] == (byte) 1;
        orgInGroupLoanArray[index].OrgName = (string) orgRows[index]["org_name"];
        orgInGroupLoanArray[index].Type = orgRows[index]["org_type"] != DBNull.Value ? "External" : "Internal";
        orgInGroupLoanArray[index].Access = aclResourceAccess;
      }
      return orgInGroupLoanArray;
    }

    private static UserGroupLoanUser[] PopulateLoanUsersInGroup(DataRow[] userRows)
    {
      if (userRows == null)
        return Enumerable.Empty<UserGroupLoanUser>().ToArray<UserGroupLoanUser>();
      UserGroupLoanUser[] userGroupLoanUserArray1 = new UserGroupLoanUser[userRows.Length];
      for (int index1 = 0; index1 < userGroupLoanUserArray1.Length; ++index1)
      {
        AclResourceAccess aclResourceAccess = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), userRows[index1]["access"].ToString());
        UserGroupLoanUser[] userGroupLoanUserArray2 = userGroupLoanUserArray1;
        int index2 = index1;
        UserGroupLoanUser userGroupLoanUser = new UserGroupLoanUser();
        userGroupLoanUser.UserId = (string) userRows[index1]["userid"];
        userGroupLoanUser.FirstName = (string) userRows[index1]["first_name"];
        userGroupLoanUser.MiddleName = userRows[index1]["middle_name"] != DBNull.Value ? (string) userRows[index1]["middle_name"] : string.Empty;
        userGroupLoanUser.LastName = (string) userRows[index1]["last_name"];
        userGroupLoanUser.SuffixName = userRows[index1]["suffix_name"] != DBNull.Value ? (string) userRows[index1]["suffix_name"] : string.Empty;
        userGroupLoanUser.UserType = userRows[index1]["user_type"] == DBNull.Value || !string.Equals(userRows[index1]["user_type"].ToString(), "External", StringComparison.CurrentCultureIgnoreCase) ? "Internal" : "External";
        userGroupLoanUser.Access = aclResourceAccess;
        userGroupLoanUserArray2[index2] = userGroupLoanUser;
      }
      return userGroupLoanUserArray1;
    }

    private static List<string> PopulateAclGroupLoanFolders(DataRow[] folderRows)
    {
      List<string> stringList = new List<string>(folderRows.Length);
      for (int index = 0; index < folderRows.Length; ++index)
        stringList.Add(EllieMae.EMLite.DataAccess.SQL.DecodeString(folderRows[index]["folderName"]));
      return stringList;
    }

    public static string[] GetUsersInGroup(int groupID, bool includeOrgUsers)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (includeOrgUsers)
          dbQueryBuilder.Append("select userid from AclGroupMembers where groupID = " + (object) groupID);
        else
          dbQueryBuilder.Append("select userid from AclGroupUserRef where groupID = " + (object) groupID);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new string[0];
        string[] usersInGroup = new string[dataRowCollection.Count];
        for (int index = 0; index < usersInGroup.Length; ++index)
          usersInGroup[index] = (string) dataRowCollection[index]["userid"];
        return usersInGroup;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
        return (string[]) null;
      }
    }

    public static Dictionary<int, string[]> GetUsersInGroups(int[] groupIds, bool includeOrgUsers)
    {
      Dictionary<int, string[]> usersInGroups = new Dictionary<int, string[]>();
      try
      {
        if (groupIds == null || groupIds.Length == 0)
          return usersInGroups;
        string str = string.Join<int>(",", ((IEnumerable<int>) groupIds).Distinct<int>());
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        if (includeOrgUsers)
          dbQueryBuilder.Append("select groupid, userid from AclGroupMembers where groupID in (" + str + ")");
        else
          dbQueryBuilder.Append("select groupid, userid from AclGroupUserRef where groupID in (" + str + ")");
        DataRowCollection source1 = dbQueryBuilder.Execute();
        if (source1 == null)
          return usersInGroups;
        foreach (int groupId in groupIds)
        {
          int gId = groupId;
          IEnumerable<string> source2 = source1.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>) (r => (int) r["groupid"] == gId)).Select<DataRow, string>((System.Func<DataRow, string>) (r => (string) r["userid"]));
          usersInGroups.Add(gId, source2.ToArray<string>());
        }
        return usersInGroups;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
        return usersInGroups;
      }
    }

    public static List<SimpleUserInfo> GetUserInfoByUsersAndGroups(
      string[] users,
      int[] groups,
      bool includeOrgUsers = true)
    {
      List<SimpleUserInfo> byUsersAndGroups = new List<SimpleUserInfo>();
      if (users.Length == 0 && groups.Length == 0)
        return byUsersAndGroups;
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("-- Create a temp table for user Ids");
        dbQueryBuilder.AppendLine("declare @user_ids table (user_id varchar(38))");
        dbQueryBuilder.AppendLine("declare @group_ids table (group_id int)");
        foreach (string user in users)
          dbQueryBuilder.AppendLine("insert into @user_ids (user_id) values ('" + user.Trim() + "')");
        if (groups.Length != 0)
        {
          foreach (int group in groups)
            dbQueryBuilder.AppendLine("insert into @group_ids (group_id) values (" + (object) group + ")");
          if (includeOrgUsers)
            dbQueryBuilder.AppendLine("insert into @user_ids select userid from AclGroupMembers a inner join @group_ids g on a.groupID = g.group_id");
          else
            dbQueryBuilder.AppendLine("insert into @user_ids select userid from AclGroupUserRef a inner join @group_ids g on a.groupID = g.group_id");
        }
        dbQueryBuilder.AppendLine("select distinct userid, first_name, last_name, first_name + ' ' + last_name full_name, email from users u inner join @user_ids t on u.userId = t.user_id where u.email != ''");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count <= 0)
          return byUsersAndGroups;
        string[] strArray = new string[dataRowCollection.Count];
        for (int index = 0; index < strArray.Length; ++index)
        {
          SimpleUserInfo simpleUserInfo = new SimpleUserInfo((string) dataRowCollection[index]["userid"], (string) dataRowCollection[index]["full_name"], (string) dataRowCollection[index]["first_name"], (string) dataRowCollection[index]["last_name"], (string) dataRowCollection[index]["email"]);
          byUsersAndGroups.Add(simpleUserInfo);
        }
        return byUsersAndGroups;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
        return byUsersAndGroups;
      }
    }

    public static AclTriState GetBorrowerContactAccessRight(
      AclGroup[] groupList,
      UserInfo viewer,
      UserInfo contactOwner)
    {
      try
      {
        int[] ancestorsOfOrg = OrganizationStore.GetAncestorsOfOrg(contactOwner.OrgId, false);
        if (ancestorsOfOrg == null || ancestorsOfOrg.Length == 0)
          return AclTriState.Unspecified;
        bool flag = false;
        foreach (int num in ancestorsOfOrg)
        {
          if (num == viewer.OrgId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return AclTriState.Unspecified;
        AclTriState contactAccessRight = AclTriState.Unspecified;
        foreach (AclGroup group in groupList)
        {
          if (group.ViewSubordinatesContacts)
          {
            if (group.ContactAccess == AclResourceAccess.ReadWrite)
              return AclTriState.True;
            contactAccessRight = AclTriState.False;
          }
        }
        return contactAccessRight;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
        return AclTriState.Unspecified;
      }
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupOrgRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable1 = sql.ExecuteTableQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
        sql.Reset();
        if (sourceTable1 != null && sourceTable1.Rows.Count > 0)
        {
          AclGroupAccessor.CloneStatementHelper(sourceTable1, sql, "[AclGroupOrgRef]", "groupID", desGroupID);
          sql.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
          sql.Reset();
        }
        sql.Append("select * from [AclGroupUserRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable2 = sql.ExecuteTableQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout);
        sql.Reset();
        if (sourceTable2 == null || sourceTable2.Rows.Count <= 0)
          return;
        AclGroupAccessor.CloneStatementHelper(sourceTable2, sql, "[AclGroupUserRef]", "groupID", desGroupID);
        sql.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupAccessor), ex);
      }
    }

    public static Dictionary<string, object> GetMembersInGroup(int groupID)
    {
      Dictionary<string, object> membersInGroup = new Dictionary<string, object>();
      membersInGroup.Add("OrgList", (object) AclGroupAccessor.GetOrgsInGroup(groupID));
      string[] usersInGroup = AclGroupAccessor.GetUsersInGroup(groupID, false);
      if (usersInGroup != null && usersInGroup.Length != 0)
      {
        Hashtable users = User.GetUsers(usersInGroup, false);
        if (users != null && users.Count > 0)
        {
          ArrayList arrayList = new ArrayList(users.Values);
          membersInGroup.Add("UserList", (object) (UserInfo[]) arrayList.ToArray(typeof (UserInfo)));
        }
      }
      return membersInGroup;
    }

    private static void CloneStatementHelperForSQL2005orEarlier(
      DataTable sourceTable,
      DbQueryBuilder sql,
      string tableName,
      string keyColumnName,
      int desKeyIDValue)
    {
      string str1 = "";
      string str2 = "";
      foreach (DataRow row in (InternalDataCollectionBase) sourceTable.Rows)
      {
        DataColumnCollection columns = sourceTable.Columns;
        string str3 = "Insert into " + tableName + " (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != keyColumnName) ? str2 + EllieMae.EMLite.DataAccess.SQL.Encode((object) desKeyIDValue) : str2 + EllieMae.EMLite.DataAccess.SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != keyColumnName) ? str2 + ", " + EllieMae.EMLite.DataAccess.SQL.Encode((object) desKeyIDValue) : str2 + ", " + EllieMae.EMLite.DataAccess.SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        sql.AppendLine(text);
        str1 = "";
        str2 = "";
      }
    }

    internal static void CloneStatementHelper(
      DataTable sourceTable,
      DbQueryBuilder sql,
      string tableName,
      string keyColumnName,
      int desKeyIDValue)
    {
      using (DbAccessManager dbAccessManager = new DbAccessManager())
      {
        DbVersion dbVersion = dbAccessManager.GetDbVersion();
        if (DbVersion.SQL2000 != dbVersion && DbVersion.SQL2005 != dbVersion)
        {
          if (dbVersion != DbVersion.None)
            goto label_7;
        }
        AclGroupAccessor.CloneStatementHelperForSQL2005orEarlier(sourceTable, sql, tableName, keyColumnName, desKeyIDValue);
        return;
      }
label_7:
      string text = "";
      string str1 = "";
      int num = 0;
      foreach (DataRow row in (InternalDataCollectionBase) sourceTable.Rows)
      {
        if (num == 0)
          text = "Insert into " + tableName + " (";
        DataColumnCollection columns = sourceTable.Columns;
        for (int index = 0; index < columns.Count; ++index)
        {
          string str2 = index == 0 ? "" : ", ";
          if (num == 0)
            text = text + str2 + columns[index].ColumnName;
          str1 = !(columns[index].ColumnName != keyColumnName) ? str1 + str2 + EllieMae.EMLite.DataAccess.SQL.Encode((object) desKeyIDValue) : str1 + str2 + EllieMae.EMLite.DataAccess.SQL.Encode(row[columns[index].ColumnName]);
        }
        text = text + (num == 0 ? " ) Values" : ",") + " (" + str1 + ")";
        str1 = "";
        ++num;
      }
      sql.AppendLine(text);
    }

    public static List<UserGroupDetails> GetAllUserGroupsInfo(
      out int totalCount,
      UserGroupCoreEntity entities = UserGroupCoreEntity.Summary,
      int start = 0,
      int limit = 10)
    {
      totalCount = 0;
      bool flag = start >= 0 && limit > 0;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * FROM [AclGroups]");
      DataRowCollection dataRowCollection = (DataRowCollection) null;
      if (flag)
      {
        DataTable paginatedRecords = new DbQueryBuilder().GetPaginatedRecords(dbQueryBuilder.ToString(), start + 1, start + limit, (List<SortColumn>) null);
        if (paginatedRecords != null && paginatedRecords.Rows != null)
          dataRowCollection = paginatedRecords.Rows;
      }
      else
        dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return new List<UserGroupDetails>();
      totalCount = !flag ? dataRowCollection.Count : EllieMae.EMLite.DataAccess.SQL.DecodeInt(dataRowCollection[0]["TotalRowCount"]);
      int[] numArray = new int[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        numArray[index] = (int) dataRowCollection[index]["groupID"];
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine(AclGroupAccessor.GetUserGroupDetailsQuery(entities, (string) null, numArray));
      return AclGroupAccessor.PopulateUserGroupDetailsData(dbQueryBuilder.ExecuteSetQuery(), ((IEnumerable<int>) numArray).ToList<int>());
    }

    public static bool CheckIfGroupIdExist(string groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 1 FROM [AclGroups] WHERE groupid = " + EllieMae.EMLite.DataAccess.SQL.Encode((object) groupId));
      return !string.IsNullOrWhiteSpace(dbQueryBuilder.ExecuteScalar()?.ToString());
    }

    private static string GetUserGroupDetailsQuery(
      UserGroupCoreEntity entities,
      string gidParamName,
      int[] groupIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      string str = string.IsNullOrWhiteSpace(gidParamName) ? "(" + string.Join<int>(",", ((IEnumerable<int>) groupIds).Select<int, int>((System.Func<int, int>) (x => x))) + ")" : "(" + gidParamName + ")";
      dbQueryBuilder.AppendLine("SELECT * FROM [AclGroups] where groupID in " + str);
      if (entities.HasFlag((Enum) UserGroupCoreEntity.Members))
      {
        dbQueryBuilder.AppendLine("SELECT 'Members' as TableName, a.groupID,a.recursive, b.org_name, b.org_type, (case when b.org_type = 'External' then e.oid else b.oid end) as orgID FROM AclGroupOrgRef a INNER JOIN org_chart b on a.orgID = b.oid LEFT JOIN ExternalOriginatorManagement e on b.oid = e.org_chart_id where a.groupID in " + str);
        dbQueryBuilder.AppendLine("SELECT 'MemberOrgs' as TableName, ag.groupID, ag.userid, u.first_name, u.middle_name, u.last_name, u.suffix_name, u.status,u.locked, u.user_type FROM AclGroupUserRef ag INNER JOIN users u on ag.UserID = u.userid where ag.groupID in " + str);
      }
      if (entities.HasFlag((Enum) UserGroupCoreEntity.Loans))
      {
        dbQueryBuilder.AppendLine("SELECT 'LoanUsers' as TableName, uref.groupID,uref.userid, u.first_name, u.middle_name, u.last_name, u.suffix_name, u.user_type, uref.access FROM AclGroupLoanUserRef uref INNER JOIN users u ON uref.UserID = u.userid where uref.groupID in " + str);
        dbQueryBuilder.AppendLine("SELECT 'LoanOrgs' as TableName, a.groupID,a.recursive, a.access, b.org_name, b.org_type, (case when b.org_type = 'External' then e.oid else b.oid end) as orgID FROM AclGroupLoanOrgRef a INNER JOIN org_chart b on a.orgID = b.oid LEFT JOIN ExternalOriginatorManagement e on b.oid = e.org_chart_id where a.groupID in " + str);
        dbQueryBuilder.AppendLine("SELECT 'AccessibleFolders' as TableName, A.groupID,A.folderName FROM AclGroupLoanFolderAccess as A LEFT OUTER JOIN LoanFolder as L ON A.folderName = L.folderName WHERE A.access = 1 and A.groupID in " + str);
      }
      if (entities.HasFlag((Enum) UserGroupCoreEntity.LoanTemplates))
        dbQueryBuilder.AppendLine("select 'LoanTemplates' as TableName, agf.groupID,agf.FileID, agf.Inclusive, agf.Access, fr.FilePath, fr.FileType, fr.IsFolder, fr.FileTypePath, fr.Owner from AclGroupFileRef agf inner join FileResource fr on agf.FileID = fr.FileID where agf.groupID in " + str);
      return dbQueryBuilder.ToString();
    }

    private static List<UserGroupDetails> PopulateUserGroupDetailsData(
      DataSet ds,
      List<int> groupIds)
    {
      List<UserGroupDetails> userGroupDetailsList = new List<UserGroupDetails>();
      for (int index1 = 0; index1 < groupIds.Count && ds.Tables.Count > 0; ++index1)
      {
        UserGroupDetails userGroupDetails = new UserGroupDetails();
        DataRow[] usersRows = ds.Tables[0].Select("groupID =" + (object) groupIds[index1]);
        userGroupDetails.GroupInfo = AclGroupAccessor.PopulateUserGroupInfo(usersRows);
        for (int index2 = 1; index2 < ds.Tables.Count; ++index2)
        {
          if (ds.Tables[index2].Rows.Count > 0)
          {
            string str = ds.Tables[index2].Rows[0]["TableName"] != DBNull.Value ? Convert.ToString(ds.Tables[index2].Rows[0]["TableName"]) : (string) null;
            if (!string.IsNullOrWhiteSpace(str))
            {
              DataRow[] dataRowArray = ds.Tables[index2].Select("groupID =" + (object) groupIds[index1]);
              if (str.Equals("Members", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.MemberOrganizations = AclGroupAccessor.PopulateOrgsInGroup(dataRowArray);
              if (str.Equals("MemberOrgs", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.MemberUsers = AclGroupAccessor.PopulateUsersInGroup(dataRowArray);
              if (str.Equals("LoanUsers", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.LoanUsers = AclGroupAccessor.PopulateLoanUsersInGroup(dataRowArray);
              if (str.Equals("LoanOrgs", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.LoanOrganizations = AclGroupAccessor.PopulateLoanOrgsInGroup(dataRowArray);
              if (str.Equals("AccessibleFolders", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.AccessibleFolders = AclGroupAccessor.PopulateAclGroupLoanFolders(dataRowArray);
              if (str.Equals("LoanTemplates", StringComparison.CurrentCultureIgnoreCase))
                userGroupDetails.LoanTemplates = AclGroupAccessor.PopulateAclGroupLoanTemplates(dataRowArray);
            }
          }
        }
        userGroupDetailsList.Add(userGroupDetails);
      }
      return userGroupDetailsList;
    }

    private static DataRow[] CastDataRowCollection(DataRowCollection rows)
    {
      return rows != null ? rows.Cast<DataRow>().ToArray<DataRow>() : (DataRow[]) null;
    }

    private static string GetMembersQuery(
      string gids,
      IEnumerable<int> orgIdList,
      IEnumerable<int> extOrgIdList,
      IEnumerable<string> userIdList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (orgIdList.Count<int>() > 0 || extOrgIdList.Count<int>() > 0)
        dbQueryBuilder.AppendLine(AclGroupAccessor.GetOrgMembersQuery(gids) + AclGroupAccessor.GetOrgMemberConditionQuery(orgIdList, extOrgIdList));
      if (userIdList.Count<string>() > 0)
        dbQueryBuilder.AppendLine(AclGroupAccessor.GetUserMembersQuery(gids) + AclGroupAccessor.GetUserMemberConditionQuery(userIdList));
      return dbQueryBuilder.ToString();
    }

    private static string GetMembersQuery(string gids)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine(AclGroupAccessor.GetOrgMembersQuery(gids));
      dbQueryBuilder.AppendLine(AclGroupAccessor.GetUserMembersQuery(gids));
      return dbQueryBuilder.ToString();
    }

    private static string GetOrgMembersQuery(string gids)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 'Members' as TableName, a.groupID,a.recursive, b.org_name, b.org_type, (case when b.org_type = 'External' then e.oid else b.oid end) as orgID");
      dbQueryBuilder.AppendLine("FROM AclGroupOrgRef a INNER JOIN org_chart b on a.orgID = b.oid LEFT JOIN ExternalOriginatorManagement e on b.oid = e.org_chart_id");
      dbQueryBuilder.AppendLine("where a.groupID in " + gids);
      return dbQueryBuilder.ToString();
    }

    private static string GetUserMembersQuery(string gids)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT 'MemberOrgs' as TableName, ag.groupID, ag.userid, u.first_name, u.middle_name, u.last_name, u.suffix_name, u.status,u.locked, u.user_type");
      dbQueryBuilder.AppendLine("FROM AclGroupUserRef ag INNER JOIN users u on ag.UserID = u.userid");
      dbQueryBuilder.AppendLine("where ag.groupID in " + gids);
      return dbQueryBuilder.ToString();
    }

    private static string GetOrgMemberConditionQuery(
      IEnumerable<int> orgIdList,
      IEnumerable<int> extOrgIdList)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (orgIdList.Any<int>())
        dbQueryBuilder.Append("b.oid in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) orgIdList.ToArray<int>()) + ")");
      if (extOrgIdList.Any<int>())
      {
        if (dbQueryBuilder.Length > 0)
          dbQueryBuilder.Append(" OR ");
        dbQueryBuilder.Append("e.oid IN(" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) extOrgIdList.ToArray<int>()) + ")");
      }
      return dbQueryBuilder.Length > 0 ? string.Format(" AND ({0})", (object) dbQueryBuilder) : string.Empty;
    }

    private static string GetUserMemberConditionQuery(IEnumerable<string> userIdList)
    {
      return !userIdList.Any<string>() ? string.Empty : " AND ag.UserID in (" + EllieMae.EMLite.DataAccess.SQL.EncodeArray((Array) userIdList.ToArray<string>()) + ")";
    }

    private static (List<int>, List<int>) GetExternalAndInternalOrgs(IEnumerable<OrgInGroup> orgs)
    {
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      foreach (OrgInGroup org in orgs)
      {
        if (AclGroupAccessor.IsOrgTypeExternal(org.Type))
          intList2.Add(org.OrgID);
        else
          intList1.Add(org.OrgID);
      }
      return (intList1, intList2);
    }

    public static string UpdateUserGroupMetadata(string loggedInUser, int groupId)
    {
      return string.Format("update [AclGroups] set LastModifiedDate = {0}, LastModifiedBy = {1} where groupID = {2}", (object) EllieMae.EMLite.DataAccess.SQL.EncodeDateTime(DateTime.UtcNow), (object) EllieMae.EMLite.DataAccess.SQL.Encode((object) loggedInUser), (object) groupId);
    }

    public static bool PublishUserGroupKafkaEvent(
      string eventType,
      string loggedInUser,
      int groupId,
      MembersContract members)
    {
      ClientContext current = ClientContext.GetCurrent();
      bool flag = false;
      try
      {
        bool isSourceEncompass = EncompassServer.ServerMode != EncompassServerMode.Service;
        UserGroupWebhookEvent queueEvent = new UserGroupWebhookEvent(current.InstanceName, loggedInUser, isSourceEncompass ? Enums.Source.URN_ELLI_SERVICE_ENCOMPASS : Enums.Source.URN_ELLI_SERVICE_EBS);
        queueEvent.CreateUserGroupMessage(ClientContext.CurrentRequest.CorrelationId, current.InstanceName, loggedInUser, eventType, current.ClientID, isSourceEncompass, groupId, members);
        if (queueEvent.QueueMessages.Count > 0)
        {
          IMessageQueueEventService queueEventService = (IMessageQueueEventService) new MessageQueueEventService();
          IMessageQueueProcessor processor = (IMessageQueueProcessor) new KafkaProcessor();
          queueEventService.MessageQueueProducer((MessageQueueEvent) queueEvent, processor);
          flag = true;
        }
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (AclGroupAccessor), string.Format("Exception publishing userEvent to kafka for userId - {0}. Exception details {1}", (object) loggedInUser, (object) ex.StackTrace));
      }
      return flag;
    }
  }
}
