// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupContactAccessor
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
  public class AclGroupContactAccessor
  {
    private const string className = "AclGroupContactAccessor�";
    private const string tableName_AclGroupContactUserRef = "[AclGroupContactUserRef]�";
    private const string tableName_AclGroupContactOrgRef = "[AclGroupContactOrgRef]�";

    public static void UpdateUsersInGroupContact(
      int groupID,
      string[] resetUserList,
      string[] newUserList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetUserList != null && resetUserList.Length != 0)
      {
        foreach (string resetUser in resetUserList)
          dbQueryBuilder.AppendLine("delete from [AclGroupContactUserRef] where groupID = " + (object) groupID + " and userid =" + SQL.EncodeString(resetUser));
      }
      if (newUserList != null && newUserList.Length != 0)
      {
        foreach (string newUser in newUserList)
          dbQueryBuilder.AppendLine("insert into [AclGroupContactUserRef] (groupID, userid, access) values (" + (object) groupID + ", " + SQL.Encode((object) newUser) + ", " + (object) 0 + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateOrgsInGroupContact(
      int groupID,
      int[] resetOrgList,
      int[] newOrgList,
      int[] newInclusiveOrgList,
      string loggedInUser)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      if (resetOrgList != null && resetOrgList.Length != 0)
      {
        foreach (int resetOrg in resetOrgList)
          dbQueryBuilder.AppendLine("delete from [AclGroupContactOrgRef] where groupID = " + (object) groupID + " and orgID= " + (object) resetOrg);
      }
      if (newOrgList != null && newOrgList.Length != 0)
      {
        foreach (int newOrg in newOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupContactOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) newOrg + ", " + (object) 0 + ", " + (object) 0 + ")");
      }
      if (newInclusiveOrgList != null && newInclusiveOrgList.Length != 0)
      {
        foreach (int newInclusiveOrg in newInclusiveOrgList)
          dbQueryBuilder.AppendLine("insert into [AclGroupContactOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) newInclusiveOrg + ", " + (object) 1 + ", " + (object) 0 + ")");
      }
      if (!(dbQueryBuilder.ToString() != ""))
        return;
      if (loggedInUser != null)
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddUserToGroupContact(int groupID, UserInGroupContact userInGroupContact)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupContactUserRef] where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) userInGroupContact.UserID));
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupContactUserRef] (groupID, userid, access) values (" + (object) groupID + ", " + SQL.Encode((object) userInGroupContact.UserID) + ", " + (object) (int) userInGroupContact.Access + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void AddOrgToGroupContact(int groupID, OrgInGroupContact orgInGroupContact)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("Declare @id int");
      dbQueryBuilder.AppendLine("select @id = groupID from [AclGroupContactOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgInGroupContact.OrgID);
      dbQueryBuilder.AppendLine("if @id is NULL");
      dbQueryBuilder.AppendLine("begin");
      dbQueryBuilder.AppendLine("\tinsert into [AclGroupContactOrgRef] (groupID, orgID, recursive, access) values (" + (object) groupID + ", " + (object) orgInGroupContact.OrgID + ", " + (object) (orgInGroupContact.IsInclusive ? 1 : 0) + ", " + (object) (int) orgInGroupContact.Access + ")");
      dbQueryBuilder.AppendLine("end");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteUserFromGroupContact(int groupID, string userid)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupContactUserRef] where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) userid));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteOrgFromGroupContact(int groupID, int orgID)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from [AclGroupContactOrgRef] where groupID = " + (object) groupID + " and orgID = " + (object) orgID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static OrgInGroupContact[] GetOrgsInGroupContact(int groupID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select a.*, b.org_name from [AclGroupContactOrgRef] a inner join org_chart b on a.orgID = b.oid where a.groupID = " + (object) groupID);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new OrgInGroupContact[0];
        OrgInGroupContact[] orgsInGroupContact = new OrgInGroupContact[dataRowCollection.Count];
        for (int index = 0; index < orgsInGroupContact.Length; ++index)
        {
          AclResourceAccess access = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["access"].ToString());
          orgsInGroupContact[index] = new OrgInGroupContact((int) dataRowCollection[index]["orgID"], (byte) dataRowCollection[index]["recursive"] == (byte) 1, access, string.Concat(dataRowCollection[index]["org_name"]));
        }
        return orgsInGroupContact;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupContactAccessor), ex);
        return (OrgInGroupContact[]) null;
      }
    }

    public static UserInGroupContact[] GetUsersInGroupContact(int groupID)
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.Append("select userid, access from [AclGroupContactUserRef] where groupID = " + (object) groupID);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return new UserInGroupContact[0];
        UserInGroupContact[] usersInGroupContact = new UserInGroupContact[dataRowCollection.Count];
        for (int index = 0; index < usersInGroupContact.Length; ++index)
        {
          AclResourceAccess access = (AclResourceAccess) Enum.Parse(typeof (AclResourceAccess), dataRowCollection[index]["access"].ToString());
          usersInGroupContact[index] = new UserInGroupContact((string) dataRowCollection[index]["userid"], access);
        }
        return usersInGroupContact;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupContactAccessor), ex);
        return (UserInGroupContact[]) null;
      }
    }

    public static AclGroupContactMembers GetMembersInGroupContact(int groupID)
    {
      try
      {
        return new AclGroupContactMembers(groupID)
        {
          UserMembers = AclGroupContactAccessor.GetUsersInGroupContact(groupID),
          OrgMembers = AclGroupContactAccessor.GetOrgsInGroupContact(groupID)
        };
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupContactAccessor), ex);
        return (AclGroupContactMembers) null;
      }
    }

    public static void UpdateUserInGroupContact(int groupID, UserInGroupContact user)
    {
      if (user == (UserInGroupContact) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [AclGroupContactUserRef] set access = " + (object) (int) user.Access + " where groupID = " + (object) groupID + " and userid = " + SQL.Encode((object) user.UserID));
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdateOrgInGroupContact(int groupID, OrgInGroupContact org)
    {
      if ((OrgInGroup) org == (OrgInGroup) null)
        return;
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("update [AclGroupContactOrgRef] set access = " + (object) (int) org.Access + ", recursive = " + (object) (org.IsInclusive ? 1 : 0) + " where groupID = " + (object) groupID + " and orgid = " + (object) org.OrgID);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static AclTriState GetBorrowerContactAccessRight(
      AclGroup[] groupList,
      UserInfo contactOwner)
    {
      try
      {
        int[] data = new int[groupList.Length];
        for (int index = 0; index < groupList.Length; ++index)
          data[index] = groupList[index].ID;
        int[] ancestorsOfOrg = OrganizationStore.GetAncestorsOfOrg(contactOwner.OrgId, true);
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        string text = "select AGCU.access from AclGroups AG inner join AclGroupContactUserRef AGCU on AG.groupID = AGCU.groupID where AG.groupID in ( " + SQL.EncodeArray((Array) data) + ") AND AGCU.userid = '" + contactOwner.Userid + "' union select AGCO.access from AclGroups AG inner join AclGroupContactOrgRef AGCO on AG.groupID = AGCO.groupID where AG.groupID in (" + SQL.EncodeArray((Array) data) + ") AND (AGCO.orgID = '" + (object) contactOwner.OrgId + "' or (AGCO.orgID in (" + SQL.EncodeArray((Array) ancestorsOfOrg) + ") and recursive = 1))";
        dbQueryBuilder.Append(text);
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        if (dataRowCollection == null)
          return AclTriState.Unspecified;
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          if (dataRowCollection[index]["access"].ToString() == "1")
            return AclTriState.True;
        }
        return AclTriState.False;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupContactAccessor), ex);
        return AclTriState.Unspecified;
      }
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupContactOrgRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable1 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable1 != null && sourceTable1.Rows.Count > 0)
        {
          AclGroupContactAccessor.CloneStatementHelper(sourceTable1, sql, "[AclGroupContactOrgRef]", "groupID", desGroupID);
          sql.ExecuteNonQuery();
          sql.Reset();
        }
        sql.Append("select * from [AclGroupContactUserRef] where groupID = " + (object) sourceGroupID);
        DataTable sourceTable2 = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable2 == null || sourceTable2.Rows.Count <= 0)
          return;
        AclGroupContactAccessor.CloneStatementHelper(sourceTable2, sql, "[AclGroupContactUserRef]", "groupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupContactAccessor), ex);
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
