// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupLoanAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupLoanAccessor
  {
    private const string className = "AclGroupLoanAccessor�";
    private const string tableName_AclGroupLoanUserRef = "[AclGroupLoanUserRef]�";
    private const string tableName_AclGroupLoanOrgRef = "[AclGroupLoanOrgRef]�";
    private const string tableName_AclGroupLoanFolderAccess = "[AclGroupLoanFolderAccess]�";

    public static void UpdateUsersInGroupLoan(
      int groupID,
      string[] resetUserList,
      string[] newUserList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetUserList != null && resetUserList.Length != 0)
        dbQueryBuilder.AppendLine("delete from [AclGroupLoanUserRef] where groupID = " + (object) groupID + " and userid in (" + string.Join(",", Array.ConvertAll<string, string>(resetUserList, (Converter<string, string>) (x => "'" + x + "'"))) + ")");
      if (newUserList != null && newUserList.Length != 0)
      {
        foreach (string newUser in newUserList)
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanUserRef] (groupID, userid, access) values (" + (object) groupID + ", " + SQL.Encode((object) newUser) + ", " + (object) 0 + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateOrgsInGroupLoan(
      int groupID,
      int[] resetOrgList,
      int[] newOrgList,
      int[] newInclusiveOrgList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetOrgList != null && resetOrgList.Length != 0)
        dbQueryBuilder.AppendLine("delete from [AclGroupLoanOrgRef] where groupID = " + (object) groupID + " and orgID in (" + string.Join(",", Array.ConvertAll<int, string>(resetOrgList, (Converter<int, string>) (x => x.ToString()))) + ")");
      if (newOrgList != null && newOrgList.Length != 0)
      {
        foreach (int newOrg in newOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) newOrg + ", " + (object) 0 + ", " + (object) 0 + ")");
      }
      if (newInclusiveOrgList != null && newInclusiveOrgList.Length != 0)
      {
        foreach (int newInclusiveOrg in newInclusiveOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupLoanOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) newInclusiveOrg + ", " + (object) 1 + ", " + (object) 0 + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddUserToGroupLoan(int groupID, UserInGroupLoan userInGroupLoan)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupLoanUserRef] where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) userInGroupLoan.UserID));
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupLoanUserRef] (groupID, userid, access) values (" + (object) groupID + ", " + SQL.Encode((object) userInGroupLoan.UserID) + ", " + (object) (int) userInGroupLoan.Access + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddOrgToGroupLoan(int groupID, OrgInGroupLoan orgInGroupLoan)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupLoanOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgInGroupLoan.OrgID);
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupLoanOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) orgInGroupLoan.OrgID + ", " + (object) (orgInGroupLoan.IsInclusive ? 1 : 0) + ", " + (object) (int) orgInGroupLoan.Access + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteUserFromGroupLoan(int groupID, string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupLoanUserRef] where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) userid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteOrgFromGroupLoan(int groupID, int orgID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupLoanOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static OrgInGroupLoan[] GetOrgsInGroupLoan(int groupID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select a.*, b.org_name from AclGroupLoanOrgRef a inner join org_chart b on a.orgID = b.oid where a.groupID = " + (object) groupID);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new OrgInGroupLoan[0];
        OrgInGroupLoan[] orgsInGroupLoan = new OrgInGroupLoan[dataRowCollection.Count];
        for (int index = 0; index < orgsInGroupLoan.Length; ++index)
        {
          AclResourceAccess access = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["access"].ToString());
          orgsInGroupLoan[index] = new OrgInGroupLoan((int) dataRowCollection[index]["orgID"], (byte) dataRowCollection[index]["recursive"] == (byte) 1, access, string.Concat(dataRowCollection[index]["org_name"]));
        }
        return orgsInGroupLoan;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupLoanAccessor), ex);
        return (OrgInGroupLoan[]) null;
      }
    }

    public static UserInGroupLoan[] GetUsersInGroupLoan(int groupID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select userid, access from AclGroupLoanUserRef where groupID = " + (object) groupID);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new UserInGroupLoan[0];
        UserInGroupLoan[] usersInGroupLoan = new UserInGroupLoan[dataRowCollection.Count];
        for (int index = 0; index < usersInGroupLoan.Length; ++index)
        {
          AclResourceAccess access = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["access"].ToString());
          usersInGroupLoan[index] = new UserInGroupLoan((string) dataRowCollection[index]["userid"], access);
        }
        return usersInGroupLoan;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupLoanAccessor), ex);
        return (UserInGroupLoan[]) null;
      }
    }

    public static void UpdateUserInGroupLoan(int groupID, UserInGroupLoan user)
    {
      if (user == (UserInGroupLoan) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [AclGroupLoanUserRef] set access = " + (object) (int) user.Access + " where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) user.UserID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateOrgInGroupLoan(int groupID, OrgInGroupLoan org)
    {
      if ((OrgInGroup) org == (OrgInGroup) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [AclGroupLoanOrgRef] set access = " + (object) (int) org.Access + ", recursive = " + (object) (org.IsInclusive ? 1 : 0) + " where groupID = " + (object) groupID + " and orgid = " + (object) org.OrgID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanFolderInGroup GetAclGroupLoanFolder(int groupId, string folderName)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from AclGroupLoanFolderAccess where groupID = " + (object) groupId + " and folderName like '" + SQL.Escape(folderName) + "'");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        return dataRowCollection == null ? (LoanFolderInGroup) null : new LoanFolderInGroup((int) dataRowCollection[0]["groupID"], (string) dataRowCollection[0][nameof (folderName)], (bool) SQL.Decode((object) ((byte) dataRowCollection[0]["access"] == (byte) 1), (object) false));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupLoanAccessor), ex);
        return (LoanFolderInGroup) null;
      }
    }

    public static void UpdateAclGroupLoanFolder(
      LoanFolderInGroup folderInGroup,
      string loggedInUser = null)
    {
      if (folderInGroup == (LoanFolderInGroup) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from AclGroupLoanFolderAccess where GroupID = " + (object) folderInGroup.GroupID + " and folderName like '" + SQL.Escape(folderInGroup.FolderName) + "'");
      dbQueryBuilder.AppendLine("insert into AclGroupLoanFolderAccess (groupID, folderName, access) values (" + (object) folderInGroup.GroupID + ", " + SQL.Encode((object) folderInGroup.FolderName) + ", " + SQL.EncodeFlag(folderInGroup.Accessible) + ")");
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, folderInGroup.GroupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static LoanFolderInGroup[] GetAclGroupLoanFolders(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select A.*, L.folderType from AclGroupLoanFolderAccess as A left outer join LoanFolder as L on A.folderName = L.folderName where groupID = " + (object) groupId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      LoanFolderInGroup[] groupLoanFolders = new LoanFolderInGroup[dataRowCollection.Count];
      for (int index = 0; index < groupLoanFolders.Length; ++index)
      {
        LoanFolderInfo.LoanFolderType folderType = LoanFolderInfo.LoanFolderType.Regular;
        if (dataRowCollection[index]["folderType"] != DBNull.Value)
          folderType = (LoanFolderInfo.LoanFolderType) dataRowCollection[index]["folderType"];
        groupLoanFolders[index] = new LoanFolderInGroup((int) dataRowCollection[index]["groupID"], (string) dataRowCollection[index]["folderName"], (bool) SQL.Decode((object) ((byte) dataRowCollection[index]["access"] == (byte) 1), (object) false), folderType);
      }
      return groupLoanFolders;
    }

    public static string GetUsersAccessibleLoanFolder(UserInfo user, string folderName)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select distinct aglfa.folderName from AclGroupLoanFolderAccess as aglfa");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on aglfa.GroupID = agm.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) user.Userid));
      dbQueryBuilder.AppendLine("   and aglfa.access = 1");
      dbQueryBuilder.AppendLine("   and aglfa.folderName = " + SQL.Encode((object) folderName));
      dbQueryBuilder.Execute(DbTransactionType.Snapshot);
      return SQL.DecodeString(dbQueryBuilder.ExecuteScalar());
    }

    public static string[] GetUsersAccessibleLoanFolders(UserInfo user)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder(DBReadReplicaFeature.Pipeline);
      dbQueryBuilder.AppendLine("select distinct aglfa.folderName from AclGroupLoanFolderAccess as aglfa");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on aglfa.GroupID = agm.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) user.Userid));
      dbQueryBuilder.AppendLine("   and aglfa.access = 1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.Snapshot);
      string[] accessibleLoanFolders = new string[dataRowCollection.Count];
      for (int index = 0; index < accessibleLoanFolders.Length; ++index)
        accessibleLoanFolders[index] = SQL.DecodeString(dataRowCollection[index]["folderName"]);
      return accessibleLoanFolders;
    }

    public static bool IsFolderAccessibleToUser(UserInfo user, string folderName)
    {
      foreach (string accessibleLoanFolder in AclGroupLoanAccessor.GetUsersAccessibleLoanFolders(user))
      {
        if (string.Compare(accessibleLoanFolder, folderName, true) == 0)
          return true;
      }
      return false;
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupLoanFolderAccess] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable1 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable1 != null && sourceTable1.Rows.Count > 0)
        {
          AclGroupLoanAccessor.CloneStatementHelper(sourceTable1, sql, "[AclGroupLoanFolderAccess]", "groupID", desGroupID);
          sql.ExecuteNonQuery();
          sql.Reset();
        }
        sql.Append("select * from [AclGroupLoanOrgRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable2 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable2 != null && sourceTable2.Rows.Count > 0)
        {
          AclGroupLoanAccessor.CloneStatementHelper(sourceTable2, sql, "[AclGroupLoanOrgRef]", "groupID", desGroupID);
          sql.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
          sql.Reset();
        }
        sql.Append("select * from [AclGroupLoanUserRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable3 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable3 == null || sourceTable3.Rows.Count <= 0)
          return;
        AclGroupLoanAccessor.CloneStatementHelper(sourceTable3, sql, "[AclGroupLoanUserRef]", "groupID", desGroupID);
        sql.ExecuteNonQuery(EnConfigurationSettings.GlobalSettings.AclGroupSQLTimeout, DbTransactionType.None);
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupLoanAccessor), ex);
      }
    }

    private static void CloneStatementHelper(
      DataTable sourceTable,
      DbQueryBuilder sql,
      string tableName,
      string keyColumnName,
      int desKeyIDValue)
    {
      AclGroupAccessor.CloneStatementHelper(sourceTable, sql, tableName, keyColumnName, desKeyIDValue);
    }

    public static AclGroupLoanMembers GetMembersInGroupLoan(int groupID)
    {
      return new AclGroupLoanMembers(groupID)
      {
        UserMembers = AclGroupLoanAccessor.GetUsersInGroupLoan(groupID),
        OrgMembers = AclGroupLoanAccessor.GetOrgsInGroupLoan(groupID)
      };
    }
  }
}
