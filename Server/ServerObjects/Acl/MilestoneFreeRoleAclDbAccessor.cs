// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.MilestoneFreeRoleAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class MilestoneFreeRoleAclDbAccessor
  {
    private const string tableName = "[Acl_Milestones]�";
    private const string tableName_User = "[Acl_Milestones_User]�";

    private MilestoneFreeRoleAclDbAccessor()
    {
    }

    public static Hashtable GetPermissions(int roleID, int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      Hashtable milestoneFreeRole = MilestoneFreeRoleAclDbAccessor.GetCurrentMilestoneFreeRole();
      Hashtable permissions = new Hashtable();
      bool aclFeaturesDefault = MilestoneFreeRoleAclDbAccessor.getAclFeaturesDefault(personaID);
      if (roleID > 0)
        dbQueryBuilder.AppendLine("select * from [Acl_Milestones] where roleID = " + (object) roleID + " and featureId = " + (object) (int) aclMilestone + " and personaID = " + (object) personaID + " and roleID > 0");
      else
        dbQueryBuilder.AppendLine("select * from [Acl_Milestones] where featureId = " + (object) (int) aclMilestone + " and personaID = " + (object) personaID + " and roleID > 0");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      IDictionaryEnumerator enumerator = milestoneFreeRole.GetEnumerator();
      while (enumerator.MoveNext())
      {
        bool flag1 = false;
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          if (string.Concat(enumerator.Key) == string.Concat(dataRow[nameof (roleID)]))
          {
            flag1 = true;
            bool flag2 = (byte) dataRow["access"] == (byte) 1;
            Hashtable hashtable = (Hashtable) milestoneFreeRole[enumerator.Key];
            hashtable[(object) "Permission"] = (object) flag2;
            permissions.Add(enumerator.Key, (object) hashtable);
          }
        }
        if (!flag1)
        {
          MilestoneFreeRoleAclDbAccessor.SetPermission(int.Parse(string.Concat(enumerator.Key)), personaID, aclFeaturesDefault);
          Hashtable hashtable = (Hashtable) milestoneFreeRole[enumerator.Key];
          hashtable[(object) "Permission"] = (object) aclFeaturesDefault;
          permissions.Add(enumerator.Key, (object) hashtable);
        }
      }
      return permissions;
    }

    public static bool GetPermission(int roleID, int[] personaIDs)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      dbQueryBuilder.AppendLine("select access from [Acl_Milestones] where featureId = " + (object) (int) aclMilestone + " and roleID = " + (object) roleID + " and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ") and roleID > 0 Order by access");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection != null && dataRowCollection.Count > 0 && (byte) dataRowCollection[0]["access"] == (byte) 1;
    }

    public static Hashtable GetPermissions(int[] personaIDs)
    {
      foreach (int personaId in personaIDs)
      {
        if (personaId == 1)
        {
          MilestoneFreeRoleAclDbAccessor.MilestoneFreeRoleCleanUp();
          break;
        }
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      Hashtable milestoneFreeRole = MilestoneFreeRoleAclDbAccessor.GetCurrentMilestoneFreeRole();
      Hashtable permissions = new Hashtable();
      IDictionaryEnumerator enumerator = milestoneFreeRole.GetEnumerator();
      while (enumerator.MoveNext())
      {
        string str = string.Concat(enumerator.Key);
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("select access from [Acl_Milestones] where featureId = " + (object) (int) aclMilestone + " and roleID = " + str + " and personaID in (" + SQL.EncodeArray((Array) personaIDs) + ") and roleID > 0 and access > 0 Order by access");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        Hashtable hashtable = (Hashtable) enumerator.Value;
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          bool flag = (byte) dataRowCollection[0]["access"] == (byte) 1;
          hashtable[(object) "Permission"] = (object) flag;
          permissions.Add(enumerator.Key, (object) hashtable);
        }
        else
          permissions.Add(enumerator.Key, (object) hashtable);
      }
      return permissions;
    }

    public static bool GetPermission(int roleID, UserInfo userInfo)
    {
      if (userInfo.IsSuperAdministrator())
        return true;
      int[] personaIds = AclUtils.GetPersonaIDs(userInfo.UserPersonas);
      Hashtable hashtable = (Hashtable) MilestoneFreeRoleAclDbAccessor.GetPersonalPermission(roleID, personaIds, userInfo.Userid)[(object) roleID];
      return hashtable == null || !hashtable.ContainsKey((object) "Permission") || hashtable[(object) "Permission"] == null || (bool) hashtable[(object) "Permission"];
    }

    public static void SetPermission(int roleID, string userid, object access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      dbQueryBuilder.AppendLine("delete from [Acl_Milestones_User] where roleID = " + (object) roleID + " and userid = " + SQL.Encode((object) userid) + " and featureId = " + (object) (int) aclMilestone + " and roleID > 0");
      if (access != null)
        dbQueryBuilder.AppendLine("insert into [Acl_Milestones_User] (milestoneID, featureID, userid, access, roleID) values (1, " + (object) (int) aclMilestone + ", " + SQL.Encode((object) userid) + ", " + ((bool) access ? (object) "1" : (object) "0") + ", " + (object) roleID + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SetPermission(int roleID, int personaID, bool access)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      dbQueryBuilder.AppendLine("delete from [Acl_Milestones] where roleID = " + (object) roleID + " and featureId = " + (object) (int) aclMilestone + " and personaID = " + (object) personaID + " and roleID > 0");
      dbQueryBuilder.AppendLine("insert into [Acl_Milestones] (milestoneID, featureID, personaID, access, roleID) values (1, " + (object) (int) aclMilestone + ", " + (object) personaID + ", " + (access ? (object) "1" : (object) "0") + ", " + (object) roleID + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DuplicateACLMilestonesFreeRole(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [Acl_Milestones] where personaID = " + (object) sourcePersonaID + " and roleID > 0");
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [Acl_Milestones] (";
        for (int index = 0; index < columns.Count; ++index)
        {
          if (index == 0)
          {
            str3 += columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + SQL.Encode((object) desPersonaID) : str2 + SQL.Encode(row[columns[index].ColumnName]);
          }
          else
          {
            str3 = str3 + ", " + columns[index].ColumnName;
            str2 = !(columns[index].ColumnName != "personaID") ? str2 + ", " + SQL.Encode((object) desPersonaID) : str2 + ", " + SQL.Encode(row[columns[index].ColumnName]);
          }
        }
        string text = str3 + " ) Values (" + str2 + ")";
        dbQueryBuilder.AppendLine(text);
        str1 = "";
        str2 = "";
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static Hashtable GetPersonalPermission(int roleID, int[] personaIDs, string userID)
    {
      Hashtable permissions = MilestoneFreeRoleAclDbAccessor.GetPermissions(personaIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      Hashtable personalPermission = new Hashtable();
      if (roleID > 0)
      {
        dbQueryBuilder.AppendLine("Select A.access, B.roleName from [Acl_Milestones_User] A inner join Roles B on A.roleID = B.roleID where A.userid = '" + userID + "' and A.featureID = " + (object) (int) aclMilestone + " and A.roleID=" + (object) roleID + " Order by A.access");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
        Hashtable hashtable1 = new Hashtable();
        if (dataRowCollection != null && dataRowCollection.Count > 0)
        {
          bool flag = (byte) dataRowCollection[0]["access"] == (byte) 1;
          hashtable1.Add((object) "Source", (object) "User");
          hashtable1.Add((object) "RoleID", (object) roleID);
          hashtable1.Add((object) "RoleName", (object) string.Concat(dataRowCollection[0]["roleName"]));
          hashtable1.Add((object) "Permission", (object) flag);
        }
        else
        {
          IDictionaryEnumerator enumerator = permissions.GetEnumerator();
          while (enumerator.MoveNext())
          {
            Hashtable hashtable2 = (Hashtable) enumerator.Value;
            if (string.Concat(hashtable2[(object) "RoleID"]) == string.Concat((object) roleID))
            {
              hashtable1 = hashtable2;
              break;
            }
          }
        }
        personalPermission.Add((object) roleID, (object) hashtable1);
      }
      else
      {
        IDictionaryEnumerator enumerator = permissions.GetEnumerator();
        while (enumerator.MoveNext())
        {
          dbQueryBuilder.Reset();
          dbQueryBuilder.AppendLine("Select access from [Acl_Milestones_User] where userid = '" + userID + "' and featureID = " + (object) (int) aclMilestone + " and roleID=" + enumerator.Key + " Order by access");
          DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
          Hashtable hashtable = (Hashtable) enumerator.Value;
          if (dataRowCollection != null && dataRowCollection.Count > 0)
          {
            bool flag = (byte) dataRowCollection[0]["access"] == (byte) 1;
            hashtable[(object) "Source"] = (object) "User";
            hashtable[(object) "Permission"] = (object) flag;
          }
          personalPermission.Add((object) string.Concat(enumerator.Key), (object) hashtable);
        }
      }
      return personalPermission;
    }

    public static Hashtable GetPersonalPermission(int[] roleIDs, int[] personaIDs, string userID)
    {
      Hashtable permissions = MilestoneFreeRoleAclDbAccessor.GetPermissions(personaIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      AclMilestone aclMilestone = AclMilestone.AssignLoanTeamMembers;
      dbQueryBuilder.AppendLine("Select A.access, A.roleID , B.roleName  from [Acl_Milestones_User] A inner join Roles B on A.roleID = B.roleID where A.userid = '" + userID + "' and A.featureID = " + (object) (int) aclMilestone + " and A.roleID in (" + SQL.EncodeArray((Array) roleIDs) + ") Order by A.access");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Hashtable personalPermission = new Hashtable();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
        {
          Hashtable hashtable = new Hashtable();
          bool flag = (byte) dataRow["access"] == (byte) 1;
          hashtable.Add((object) "Source", (object) "User");
          hashtable.Add((object) "RoleID", dataRow["roleID"]);
          hashtable.Add((object) "RoleName", (object) string.Concat(dataRow["roleName"]));
          hashtable.Add((object) "Permission", (object) flag);
          personalPermission.Add(dataRow["roleID"], (object) hashtable);
        }
      }
      else
      {
        IDictionaryEnumerator enumerator = permissions.GetEnumerator();
        while (enumerator.MoveNext())
        {
          Hashtable hashtable = (Hashtable) enumerator.Value;
          personalPermission.Add(hashtable[(object) "RoleID"], (object) hashtable);
        }
      }
      return personalPermission;
    }

    public static Hashtable GetPermissions(int[] roleIDs, UserInfo userInfo)
    {
      int[] personaIds = AclUtils.GetPersonaIDs(userInfo.UserPersonas);
      Hashtable personalPermission = MilestoneFreeRoleAclDbAccessor.GetPersonalPermission(roleIDs, personaIds, userInfo.Userid);
      IDictionaryEnumerator enumerator = personalPermission.GetEnumerator();
      Hashtable permissions = new Hashtable();
      while (enumerator.MoveNext())
      {
        Hashtable hashtable = (Hashtable) personalPermission[enumerator.Key];
        if (hashtable == null || !hashtable.ContainsKey((object) "Permission") || hashtable[(object) "Permission"] == null)
          permissions.Add((object) Convert.ToInt32(enumerator.Key), (object) true);
        else
          permissions.Add((object) Convert.ToInt32(enumerator.Key), (object) (bool) hashtable[(object) "Permission"]);
      }
      return permissions;
    }

    public static void MilestoneFreeRoleCleanUp()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("DELETE Acl_Milestones WHERE roleID > 0 and roleID Not IN(Select roleID from Roles)");
      dbQueryBuilder.AppendLine("DELETE Acl_Milestones_User WHERE roleID > 0 and roleID Not IN(Select roleID from Roles)");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SynchronizeAdminSetting()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Update Acl_Milestones set access =1 where roleID>0 and personaID = 1");
      dbQueryBuilder.AppendLine("SELECT roleID FROM (SELECT roleID FROM Roles WHERE roleID NOT IN  (SELECT DISTINCT RoleID FROM Milestones)) x  WHERE (roleID NOT IN  (SELECT roleID  FROM  Acl_Milestones WHERE personaID = 1))");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection == null || dataRowCollection.Count <= 0)
        return;
      dbQueryBuilder.Reset();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        dbQueryBuilder.AppendLine("If not exists (select 1 from Acl_Milestones where MilestoneID = 1 and featureID = 3 and personaID = 1 and roleID = " + dataRow["roleID"] + ")");
        dbQueryBuilder.AppendLine("begin");
        dbQueryBuilder.AppendLine("Insert into Acl_Milestones (MilestoneID, featureID, personaID, access, roleID) values(1, 3, 1, 1, " + dataRow["roleID"] + ")");
        dbQueryBuilder.AppendLine("end");
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static Hashtable GetCurrentMilestoneFreeRole()
    {
      Hashtable milestoneFreeRole = new Hashtable();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select roleID, roleName from Roles where roleID not in(select distinct RoleID FROM Milestones where Archived = 0 and roleid is not null)");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          milestoneFreeRole.Add((object) string.Concat(dataRow["RoleID"]), (object) new Hashtable()
          {
            {
              (object) "Source",
              (object) "Persona"
            },
            {
              (object) "RoleName",
              (object) string.Concat(dataRow["roleName"])
            },
            {
              (object) "RoleID",
              (object) string.Concat(dataRow["roleID"])
            },
            {
              (object) "Permission",
              (object) false
            }
          });
      }
      return milestoneFreeRole;
    }

    private static bool getAclFeaturesDefault(int personaID)
    {
      return PersonaAccessor.GetPersonaAclFeaturesDefault(personaID);
    }
  }
}
