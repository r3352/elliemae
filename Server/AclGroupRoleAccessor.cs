// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupRoleAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupRoleAccessor
  {
    private const string className = "AclGroupRoleAccessor�";
    private const string tableName_AclGroupRoleOrgRef = "[AclGroupRoleOrgRef]�";
    private const string tableName_AclGroupRoleUserRef = "[AclGroupRoleUserRef]�";
    private const string tableName_AclGroupRoleAccessLevel = "[AclGroupRoleAccessLevel]�";

    public static OrgInGroupRole[] GetOrgsInGroupRole(int groupId, int roleId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select a.*, b.org_name from AclGroupRoleOrgRef a inner join org_chart b on a.orgID = b.oid where a.groupID = " + (object) groupId + " and roleID = " + (object) roleId);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new OrgInGroupRole[0];
        OrgInGroupRole[] orgsInGroupRole = new OrgInGroupRole[dataRowCollection.Count];
        for (int index = 0; index < orgsInGroupRole.Length; ++index)
          orgsInGroupRole[index] = new OrgInGroupRole(roleId, (int) dataRowCollection[index]["orgID"], (byte) dataRowCollection[index]["inclusive"] == (byte) 1, string.Concat(dataRowCollection[index]["org_name"]));
        return orgsInGroupRole;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupRoleAccessor), ex);
        return (OrgInGroupRole[]) null;
      }
    }

    public static UserInGroupRole[] GetUsersInGroupRole(int groupId, int roleId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select userid from AclGroupRoleUserRef where groupID = " + (object) groupId + " and roleID = " + (object) roleId);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new UserInGroupRole[0];
        UserInGroupRole[] usersInGroupRole = new UserInGroupRole[dataRowCollection.Count];
        for (int index = 0; index < usersInGroupRole.Length; ++index)
          usersInGroupRole[index] = new UserInGroupRole((string) dataRowCollection[index]["userid"], roleId);
        return usersInGroupRole;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupRoleAccessor), ex);
        return (UserInGroupRole[]) null;
      }
    }

    public static AclGroupRoleMembers GetMembersInGroupRole(int groupID, int roleID)
    {
      return new AclGroupRoleMembers(groupID, roleID)
      {
        OrgMembers = AclGroupRoleAccessor.GetOrgsInGroupRole(groupID, roleID),
        UserMembers = AclGroupRoleAccessor.GetUsersInGroupRole(groupID, roleID)
      };
    }

    public static void AddUserToGroupRole(int groupId, UserInGroupRole userInGroup)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupRoleUserRef] where groupID = " + (object) groupId + " and roleID = " + (object) userInGroup.RoleID + " and userid = " + SQL.Encode((object) userInGroup.UserID));
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupRoleUserRef] (groupID, roleID, userid) values (" + (object) groupId + ", " + (object) userInGroup.RoleID + ", " + SQL.Encode((object) userInGroup.UserID) + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddOrgToGroupRole(int groupID, OrgInGroupRole orgInGroup)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupRoleOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgInGroup.OrgID + " and roleID = " + (object) orgInGroup.RoleID);
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupRoleOrgRef] (groupID, roleID, orgID, inclusive) values (" + (object) groupID + ", " + (object) orgInGroup.RoleID + ", " + (object) orgInGroup.OrgID + ", " + (object) (orgInGroup.IsInclusive ? 1 : 0) + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteUserFromGroupRole(int groupID, int roleId, string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupRoleUserRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId + " and userid = " + SQL.Encode((object) userid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteOrgFromGroupRole(int groupID, int roleId, int orgID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupRoleOrgRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId + " and orgID = " + (object) orgID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ResetUsersInGroupRole(int groupID, int roleId, UserInGroupRole[] users)
    {
      if (users == null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupRoleUserRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId);
      for (int index = 0; index < users.Length; ++index)
        dbQueryBuilder.AppendLine("insert into [AclGroupRoleUserRef] (groupID, roleId, userid) values (" + (object) groupID + ", " + (object) roleId + ", " + SQL.Encode((object) users[index].UserID) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void ResetOrgsInGroupRole(int groupID, int roleId, OrgInGroupRole[] orgs)
    {
      if (orgs == null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupRoleOrgRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId);
      for (int index = 0; index < orgs.Length; ++index)
        dbQueryBuilder.AppendLine("insert into [AclGroupRoleOrgRef] (groupID, roleId, orgID, inclusive) values (" + (object) groupID + ", " + (object) roleId + ", " + (object) orgs[index].OrgID + ", " + (object) (orgs[index].IsInclusive ? 1 : 0) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateUsersInGroupRole(
      int groupID,
      int roleId,
      string[] resetUserList,
      string[] newUserList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetUserList != null && resetUserList.Length != 0)
      {
        foreach (string resetUser in resetUserList)
          dbQueryBuilder.AppendLine("delete from [AclGroupRoleUserRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId + " and userid = " + SQL.EncodeString(resetUser));
      }
      if (newUserList != null && newUserList.Length != 0)
      {
        foreach (string newUser in newUserList)
          dbQueryBuilder.AppendLine("insert into [AclGroupRoleUserRef] (groupID, roleId, userid) values (" + (object) groupID + ", " + (object) roleId + ", " + SQL.Encode((object) newUser) + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateOrgsInGroupRole(
      int groupID,
      int roleId,
      int[] resetOrgList,
      int[] newOrgList,
      int[] newInclusiveOrgList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetOrgList != null && resetOrgList.Length != 0)
      {
        foreach (int resetOrg in resetOrgList)
          dbQueryBuilder.AppendLine("delete from [AclGroupRoleOrgRef] where groupID = " + (object) groupID + " and roleID = " + (object) roleId + " and orgID=" + (object) resetOrg);
      }
      if (newOrgList != null && newOrgList.Length != 0)
      {
        foreach (int newOrg in newOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupRoleOrgRef] (groupID, roleId, orgID, inclusive) values (" + (object) groupID + ", " + (object) roleId + ", " + (object) newOrg + ", " + (object) 0 + ")");
      }
      if (newInclusiveOrgList != null && newInclusiveOrgList.Length != 0)
      {
        foreach (int newInclusiveOrg in newInclusiveOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupRoleOrgRef] (groupID, roleId, orgID, inclusive) values (" + (object) groupID + ", " + (object) roleId + ", " + (object) newInclusiveOrg + ", " + (object) 1 + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static AclGroupRoleAccessLevel GetAclGroupRoleAccessLevel(int groupId, int roleId)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from AclGroupRoleAccessLevel where groupID = " + (object) groupId + " and roleID = " + (object) roleId);
        AclGroupRoleAccessLevel groupRoleAccessLevel = new AclGroupRoleAccessLevel(groupId, roleId, AclGroupRoleAccessEnum.All, true);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null || dataRowCollection.Count == 0)
          return groupRoleAccessLevel;
        groupRoleAccessLevel.Access = (AclGroupRoleAccessEnum) Enum.Parse(typeof (AclGroupRoleAccessEnum), dataRowCollection[0]["access"].ToString());
        groupRoleAccessLevel.HideDisabledAccount = dataRowCollection[0]["hideDisabled"].ToString() == "1";
        return groupRoleAccessLevel;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupRoleAccessor), ex);
        return (AclGroupRoleAccessLevel) null;
      }
    }

    public static int[] GetAclGroupIDsByUser(string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select [GroupID] from [AclGroupMembers] where [UserID] = " + SQL.Encode((object) userid));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      int[] aclGroupIdsByUser = new int[dataRowCollection.Count];
      for (int index = 0; index < aclGroupIdsByUser.Length; ++index)
        aclGroupIdsByUser[index] = (int) dataRowCollection[index]["GroupID"];
      return aclGroupIdsByUser;
    }

    public static Dictionary<int, Dictionary<int, AclGroupRoleAccessLevel>> GetAclGroupRoleAccessLevels(
      string userid,
      int[] roleIDs,
      int[] groupIDs)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select * from AclGroupRoleAccessLevel where groupID in (" + string.Join<int>(", ", (IEnumerable<int>) groupIDs) + ") and roleID in (" + string.Join<int>(", ", (IEnumerable<int>) roleIDs) + ")");
        Dictionary<int, Dictionary<int, AclGroupRoleAccessLevel>> roleAccessLevels = new Dictionary<int, Dictionary<int, AclGroupRoleAccessLevel>>();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        {
          int num1 = (int) dataRow["roleID"];
          int num2 = (int) dataRow["groupID"];
          AclGroupRoleAccessEnum access = (AclGroupRoleAccessEnum) Enum.Parse(typeof (AclGroupRoleAccessEnum), dataRow["access"].ToString());
          bool hideDisabledAccount = dataRow["hideDisabled"].ToString() == "1";
          AclGroupRoleAccessLevel groupRoleAccessLevel = new AclGroupRoleAccessLevel(num2, num1, access, hideDisabledAccount);
          if (!roleAccessLevels.ContainsKey(num1))
            roleAccessLevels.Add(num1, new Dictionary<int, AclGroupRoleAccessLevel>());
          roleAccessLevels[num1].Add(num2, groupRoleAccessLevel);
        }
        foreach (int roleId in roleIDs)
        {
          if (!roleAccessLevels.ContainsKey(roleId))
            roleAccessLevels.Add(roleId, new Dictionary<int, AclGroupRoleAccessLevel>());
          Dictionary<int, AclGroupRoleAccessLevel> dictionary = roleAccessLevels[roleId];
          foreach (int groupId in groupIDs)
          {
            if (!dictionary.ContainsKey(groupId))
              dictionary.Add(groupId, new AclGroupRoleAccessLevel(groupId, roleId, AclGroupRoleAccessEnum.All, true));
          }
        }
        return roleAccessLevels;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupRoleAccessor), ex);
        return (Dictionary<int, Dictionary<int, AclGroupRoleAccessLevel>>) null;
      }
    }

    public static void UpdateAclGroupRoleAccessLevel(
      AclGroupRoleAccessLevel accessLevel,
      string loggedInUser)
    {
      if (accessLevel == (AclGroupRoleAccessLevel) null)
        return;
      int num = !accessLevel.HideDisabledAccount ? 0 : 1;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from AclGroupRoleAccessLevel where GroupID = " + (object) accessLevel.GroupID + " and RoleID = " + (object) accessLevel.RoleID);
      dbQueryBuilder.AppendLine("insert into AclGroupRoleAccessLevel (groupID, roleID, access, hideDisabled) values (" + (object) accessLevel.GroupID + ", " + (object) accessLevel.RoleID + ", " + (object) (int) accessLevel.Access + ", " + (object) num + ")");
      dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, accessLevel.GroupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupRoleAccessLevel] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable1 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable1 != null && sourceTable1.Rows.Count > 0)
        {
          AclGroupRoleAccessor.CloneStatementHelper(sourceTable1, sql, "[AclGroupRoleAccessLevel]", "groupID", desGroupID);
          sql.ExecuteNonQuery();
          sql.Reset();
        }
        sql.Append("select * from [AclGroupRoleOrgRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable2 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable2 != null && sourceTable2.Rows.Count > 0)
        {
          AclGroupRoleAccessor.CloneStatementHelper(sourceTable2, sql, "[AclGroupRoleOrgRef]", "groupID", desGroupID);
          sql.ExecuteNonQuery();
          sql.Reset();
        }
        sql.Append("select * from [AclGroupRoleUserRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable3 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable3 == null || sourceTable3.Rows.Count <= 0)
          return;
        AclGroupRoleAccessor.CloneStatementHelper(sourceTable3, sql, "[AclGroupRoleUserRef]", "groupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupRoleAccessor), ex);
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
  }
}
