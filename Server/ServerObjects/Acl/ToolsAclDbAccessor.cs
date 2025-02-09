// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Acl.ToolsAclDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Acl
{
  public class ToolsAclDbAccessor
  {
    private const string tableName = "[AclF_ToolsConfig]�";
    private const string tableName_User = "[AclF_ToolsConfig_User]�";

    private ToolsAclDbAccessor()
    {
    }

    public static void UpdateUserToolsConfiguration(ToolsAclInfo[] toolsAclInfoList, string userID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "DELETE [AclF_ToolsConfig_User] WHERE userID = '" + userID + "'";
      dbQueryBuilder.Append(text1);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      str1 = "";
      if (toolsAclInfoList == null)
        return;
      foreach (ToolsAclInfo toolsAclInfo in toolsAclInfoList)
      {
        str2 = "";
        string text2 = "INSERT INTO [AclF_ToolsConfig_User] (featureID, userID, roleID, access, milestoneID) VALUES (" + (object) toolsAclInfo.FeatureID + ", '" + userID + "', " + (object) toolsAclInfo.RoleID + ", " + (object) toolsAclInfo.Access + ", " + SQL.Encode((object) toolsAclInfo.MilestoneID) + ") ";
        dbQueryBuilder.AppendLine(text2);
        str2 = "";
      }
      if (toolsAclInfoList.Length == 0)
        return;
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void UpdatePersonaToolsConfiguration(
      ToolsAclInfo[] toolsAclInfoList,
      string personaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text1 = "DELETE [AclF_ToolsConfig] WHERE personaID = " + personaID;
      dbQueryBuilder.Append(text1);
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      str1 = "";
      foreach (ToolsAclInfo toolsAclInfo in toolsAclInfoList)
      {
        str2 = "";
        string text2 = "INSERT INTO [AclF_ToolsConfig] (featureID, personaID, roleID, access, milestoneID) VALUES (" + (object) toolsAclInfo.FeatureID + ", " + personaID + ", " + (object) toolsAclInfo.RoleID + ", " + (object) toolsAclInfo.Access + ", " + SQL.Encode((object) toolsAclInfo.MilestoneID) + ") ";
        dbQueryBuilder.AppendLine(text2);
        str2 = "";
      }
      if (toolsAclInfoList.Length == 0)
        return;
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static ToolsAclInfo[] GetPersonaToolsConfiguration(
      string milestoneID,
      int roleID,
      string personaID)
    {
      RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
      bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(int.Parse(personaID));
      return ToolsAclDbAccessor.GetPersonaToolsConfiguration(milestoneID, roleID, personaID, allRoleFunctions, aclFeaturesDefault);
    }

    public static ToolsAclInfo[] GetPersonaToolsConfiguration(
      string milestoneID,
      int roleID,
      string personaID,
      RoleInfo[] roleList,
      bool defaultAccess)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("INSERT into [AclF_ToolsConfig] (featureID, personaID, MilestoneID, roleID, access) SELECT 321, " + personaID + ", m.milestoneID, -1, 0 FROM milestones m WHERE NOT EXISTS(SELECT 1 FROM [AclF_ToolsConfig] x with(updlock, holdlock) WHERE x.MilestoneID = m.milestoneID AND x.featureID = 321 AND x.personaID = " + personaID + " AND x.roleID = -1)");
      dbQueryBuilder.ExecuteNonQuery();
      dbQueryBuilder.Reset();
      ToolsAclInfo[] toolsAclInfoList = (ToolsAclInfo[]) null;
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      bool flag = false;
      string str1 = "SELECT * FROM [AclF_ToolsConfig] ";
      string str2 = "WHERE ";
      string str3;
      if (milestoneID == "" && roleID < 0)
        str3 = str2 + "personaID = " + personaID + " and ((roleID not in (select distinct roleID from Milestones where roleID is not NULL)) or (roleID = -1))";
      else if (milestoneID == "")
        str3 = str2 + "roleID = " + (object) roleID + " AND personaID = " + personaID;
      else
        str3 = str2 + "milestoneID = '" + milestoneID + "' AND personaID = " + personaID;
      string text = str1 + str3;
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          if (string.Concat(row[nameof (roleID)]) != "-1")
            hashtable1.Add((object) string.Concat(row[nameof (roleID)]), (object) ToolsAclDbAccessor.ConvertDataRowToObject(row));
          else
            hashtable2.Add((object) string.Concat(row[nameof (milestoneID)]), (object) ToolsAclDbAccessor.ConvertDataRowToObject(row));
        }
      }
      foreach (RoleInfo role in roleList)
      {
        if (hashtable1.ContainsKey((object) string.Concat((object) role.RoleID)))
          ((ToolsAclInfo) hashtable1[(object) string.Concat((object) role.RoleID)]).RoleName = role.RoleName;
      }
      ArrayList arrayList1 = new ArrayList();
      ArrayList milestoneRecords = ToolsAclDbAccessor.GetMissingMilestoneRecords(personaID, defaultAccess);
      ArrayList arrayList2 = new ArrayList();
      ArrayList milestoneFreeRoleRecord = ToolsAclDbAccessor.GetMissingMilestoneFreeRoleRecord(personaID, defaultAccess);
      if (hashtable1.Count > 0 || hashtable2.Count > 0 || milestoneRecords.Count > 0 || milestoneFreeRoleRecord.Count > 0)
      {
        ArrayList arrayList3 = new ArrayList();
        if (hashtable1.Count > 0)
        {
          IDictionaryEnumerator enumerator = hashtable1.GetEnumerator();
          while (enumerator.MoveNext())
            arrayList3.Add(enumerator.Value);
        }
        if (hashtable2.Count > 0)
        {
          IDictionaryEnumerator enumerator = hashtable2.GetEnumerator();
          while (enumerator.MoveNext())
            arrayList3.Add(enumerator.Value);
        }
        if (milestoneRecords.Count > 0)
        {
          arrayList3.AddRange((ICollection) milestoneRecords);
          flag = true;
        }
        if (milestoneFreeRoleRecord.Count > 0)
        {
          arrayList3.AddRange((ICollection) milestoneFreeRoleRecord);
          flag = true;
        }
        toolsAclInfoList = (ToolsAclInfo[]) arrayList3.ToArray(typeof (ToolsAclInfo));
      }
      if (flag && toolsAclInfoList.Length != 0)
        ToolsAclDbAccessor.UpdatePersonaToolsConfiguration(toolsAclInfoList, personaID);
      return toolsAclInfoList;
    }

    public static ToolsAclInfo GetPersonaToolsConfiguration(
      string milestoneID,
      int roleID,
      int[] personaIDList)
    {
      FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIDList);
      string str = SQL.Encode((object) personaIDList);
      DataTable dataTable1 = new DataTable();
      ToolsAclInfo toolsConfiguration = new ToolsAclInfo(321, roleID, 0, "", "-1");
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string text;
      if (roleID > 0)
        text = "SELECT A.access, B.roleName FROM AclF_ToolsConfig A, Roles B WHERE A.roleID = B.roleID AND A.personaID in (" + str + ") AND A.roleID = " + (object) roleID + " ORDER BY access DESC";
      else
        text = "SELECT A.access, '' as roleName FROM AclF_ToolsConfig A WHERE A.personaID in (" + str + ") AND A.milestoneID = " + SQL.Encode((object) milestoneID) + " ORDER BY access DESC";
      dbQueryBuilder.Reset();
      dbQueryBuilder.Append(text);
      DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable2 != null && dataTable2.Rows.Count > 0)
      {
        toolsConfiguration.Access = int.Parse(string.Concat(dataTable2.Rows[0]["access"]));
        toolsConfiguration.RoleName = string.Concat(dataTable2.Rows[0]["roleName"]);
      }
      return toolsConfiguration;
    }

    public static ToolsAclInfo[] GetUserToolsConfiguration(
      string milestoneID,
      int roleID,
      string userId)
    {
      ToolsAclInfo[] toolsConfiguration = (ToolsAclInfo[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str1 = "SELECT A.*, B.roleName FROM AclF_ToolsConfig_User A left outer join Roles B on A.roleiD = B.roleID ";
      string str2 = "WHERE A.userID = '" + userId + "'";
      if (!(milestoneID == "") || roleID >= 0)
        str2 = roleID <= -1 ? str2 + " AND A.milestoneID = '" + milestoneID + "'" : str2 + " AND A.roleID = " + (object) roleID;
      string text = str1 + str2;
      dbQueryBuilder.Append(text);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      if (dataTable != null && dataTable.Rows.Count > 0)
      {
        ArrayList arrayList = new ArrayList(dataTable.Rows.Count);
        foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
        {
          ToolsAclInfo toolsAclInfo = ToolsAclDbAccessor.ConvertDataRowToObject(row);
          toolsAclInfo.RoleName = string.Concat(row["roleName"]);
          arrayList.Add((object) toolsAclInfo);
        }
        toolsConfiguration = (ToolsAclInfo[]) arrayList.ToArray(typeof (ToolsAclInfo));
      }
      return toolsConfiguration;
    }

    public static ToolsAclInfo[] GetUserToolsAccessibility(
      string milestoneID,
      int roleID,
      string userId,
      int[] personaIDList,
      RoleInfo[] roleList,
      bool defaultAccess)
    {
      ArrayList arrayList = new ArrayList();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      string str = SQL.Encode((object) personaIDList);
      DataTable dataTable1 = new DataTable();
      if (!(milestoneID == "") || roleID >= 0)
      {
        if (roleID > -1)
        {
          bool flag = false;
          foreach (RoleInfo role in roleList)
          {
            if (role.RoleID == roleID)
            {
              roleList = new RoleInfo[1]{ role };
              flag = true;
              break;
            }
          }
          if (!flag)
            return new ToolsAclInfo[1]
            {
              new ToolsAclInfo(321, roleID, 0, "", "")
            };
          foreach (RoleInfo role in roleList)
          {
            ToolsAclInfo toolsAclInfo = new ToolsAclInfo(321, role.RoleID, -1, role.RoleName, "-1");
            ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetUserToolsConfiguration("", role.RoleID, userId);
            if (toolsConfiguration != null && toolsConfiguration.Length != 0)
            {
              toolsAclInfo.Access = toolsConfiguration[0].Access;
              toolsAclInfo.CustomAccess = true;
            }
            if (toolsAclInfo.Access == -1)
            {
              string text = "SELECT access FROM [AclF_ToolsConfig] WHERE personaID in (" + str + ") AND roleID = " + (object) role.RoleID + " ORDER BY access DESC";
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append(text);
              DataTable dataTable2 = dbQueryBuilder.ExecuteTableQuery();
              if (dataTable2 != null && dataTable2.Rows.Count > 0)
                toolsAclInfo.Access = int.Parse(string.Concat(dataTable2.Rows[0]["access"]));
              else if (toolsAclInfo.Access == -1)
                toolsAclInfo.Access = defaultAccess ? 1 : 0;
            }
            arrayList.Add((object) toolsAclInfo);
          }
        }
        else
        {
          ToolsAclInfo toolsAclInfo = new ToolsAclInfo(321, -1, -1, "", milestoneID);
          ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetUserToolsConfiguration(milestoneID, -1, userId);
          if (toolsConfiguration != null && toolsConfiguration.Length != 0)
          {
            toolsAclInfo.Access = toolsConfiguration[0].Access;
            toolsAclInfo.CustomAccess = true;
          }
          if (toolsAclInfo.Access == -1)
          {
            string text = "SELECT access FROM [AclF_ToolsConfig] WHERE personaID in (" + str + ") AND milestoneID = " + SQL.Encode((object) milestoneID) + " ORDER BY access DESC";
            dbQueryBuilder.Reset();
            dbQueryBuilder.Append(text);
            DataTable dataTable3 = dbQueryBuilder.ExecuteTableQuery();
            if (dataTable3 != null && dataTable3.Rows.Count > 0)
              toolsAclInfo.Access = int.Parse(string.Concat(dataTable3.Rows[0]["access"]));
            else if (toolsAclInfo.Access == -1)
              toolsAclInfo.Access = defaultAccess ? 1 : 0;
          }
          arrayList.Add((object) toolsAclInfo);
        }
      }
      else
      {
        foreach (ToolsAclInfo toolsAclInfo in ToolsAclDbAccessor.GetPersonaToolsConfiguration(milestoneID, roleID, string.Concat((object) personaIDList[0]), roleList, defaultAccess))
        {
          if (toolsAclInfo.RoleID == -1)
          {
            ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetUserToolsConfiguration(toolsAclInfo.MilestoneID, -1, userId);
            if (toolsConfiguration != null && toolsConfiguration.Length != 0)
            {
              toolsAclInfo.Access = toolsConfiguration[0].Access;
              toolsAclInfo.CustomAccess = true;
            }
            else if (toolsAclInfo.Access == 0)
            {
              string text = "SELECT access FROM [AclF_ToolsConfig] WHERE personaID in (" + str + ") AND milestoneID = " + SQL.Encode((object) toolsAclInfo.MilestoneID) + " ORDER BY access DESC";
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append(text);
              DataTable dataTable4 = dbQueryBuilder.ExecuteTableQuery();
              if (dataTable4 != null && dataTable4.Rows.Count > 0)
                toolsAclInfo.Access = int.Parse(string.Concat(dataTable4.Rows[0]["access"]));
              else if (toolsAclInfo.Access == -1)
                toolsAclInfo.Access = defaultAccess ? 1 : 0;
            }
          }
          else
          {
            ToolsAclInfo[] toolsConfiguration = ToolsAclDbAccessor.GetUserToolsConfiguration("", toolsAclInfo.RoleID, userId);
            if (toolsConfiguration != null && toolsConfiguration.Length != 0)
            {
              toolsAclInfo.Access = toolsConfiguration[0].Access;
              toolsAclInfo.CustomAccess = true;
            }
            else if (toolsAclInfo.Access == 0)
            {
              string text = "SELECT access FROM [AclF_ToolsConfig] WHERE personaID in (" + str + ") AND roleID = " + (object) toolsAclInfo.RoleID + " ORDER BY access DESC";
              dbQueryBuilder.Reset();
              dbQueryBuilder.Append(text);
              DataTable dataTable5 = dbQueryBuilder.ExecuteTableQuery();
              if (dataTable5 != null && dataTable5.Rows.Count > 0)
                toolsAclInfo.Access = int.Parse(string.Concat(dataTable5.Rows[0]["access"]));
              else if (toolsAclInfo.Access == -1)
                toolsAclInfo.Access = defaultAccess ? 1 : 0;
            }
          }
          arrayList.Add((object) toolsAclInfo);
        }
      }
      return (ToolsAclInfo[]) arrayList.ToArray(typeof (ToolsAclInfo));
    }

    public static ToolsAclInfo[] GetUserApplicationToolsSetting(
      string milestoneID,
      int roleID,
      string userID,
      Persona[] personaList)
    {
      int[] personaIds = AclUtils.GetPersonaIDs(personaList);
      RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
      bool aclFeaturesDefault = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIds);
      return ToolsAclDbAccessor.GetUserToolsAccessibility(milestoneID, roleID, userID, personaIds, allRoleFunctions, aclFeaturesDefault);
    }

    public static void DuplicateACLTool(int sourcePersonaID, int desPersonaID)
    {
      string str1 = "";
      string str2 = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from [AclF_ToolsConfig] where personaID = " + (object) sourcePersonaID);
      DataTable dataTable = dbQueryBuilder.ExecuteTableQuery();
      dbQueryBuilder.Reset();
      DataColumnCollection columns = dataTable.Columns;
      if (dataTable == null || dataTable.Rows.Count <= 0)
        return;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      {
        string str3 = "Insert into [AclF_ToolsConfig] (";
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

    public static ToolsAclInfo[] GetFileContactsGrantWriteAccessRights(
      string userID,
      Persona[] personaList,
      string[] loanMilestoneIDs,
      int[] loanFreeRoleIDs)
    {
      List<ToolsAclInfo> result = new List<ToolsAclInfo>();
      bool flag1 = loanMilestoneIDs != null && loanMilestoneIDs.Length != 0;
      bool flag2 = loanFreeRoleIDs != null && loanFreeRoleIDs.Length != 0;
      if (!flag1 && !flag2)
        return result.ToArray();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      int[] personaIds = AclUtils.GetPersonaIDs(personaList);
      bool defaultAccess = FeaturesAclDbAccessor.GetAclFeaturesDefault(personaIds);
      string str1 = SQL.Encode((object) personaIds);
      int num = 1;
      dbQueryBuilder.AppendLine(string.Format("SELECT * FROM {0} WHERE userID = '{1}' ", (object) "[AclF_ToolsConfig_User]", (object) userID));
      if (flag1)
      {
        string str2 = SQL.Encode((object) loanMilestoneIDs);
        dbQueryBuilder.AppendLine(string.Format("SELECT milestoneID,roleID,personaID,access FROM {0} WHERE personaID in ({1}) AND milestoneID in ({2}) AND roleID = -1 ORDER BY milestoneID ", (object) "[AclF_ToolsConfig]", (object) str1, (object) str2));
        ++num;
      }
      if (flag2)
      {
        string str3 = SQL.Encode((object) loanFreeRoleIDs);
        dbQueryBuilder.AppendLine(string.Format("SELECT roleid,milestoneID,personaID,access FROM {0} WHERE personaID in ({1}) AND roleid in ({2}) AND roleID >= 0 ORDER BY roleID ", (object) "[AclF_ToolsConfig]", (object) str1, (object) str3));
        ++num;
      }
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      if (dataSet == null || dataSet.Tables.Count < num)
        throw new Exception("No enough tables loaded for tools file contact grant write access settings");
      List<ToolsAclInfo> userAcls = ToolsAclDbAccessor.CollectToolsAclInfo(dataSet.Tables[0]);
      if (flag1)
      {
        List<ToolsAclInfo> milestoneAcls = ToolsAclDbAccessor.CollectToolsAclInfo(dataSet.Tables[1]);
        Array.ForEach<string>(loanMilestoneIDs, (Action<string>) (milestoneID =>
        {
          ToolsAclInfo toolsAclInfo1 = userAcls.Find((Predicate<ToolsAclInfo>) (userAcl => userAcl.MilestoneID == milestoneID));
          if (toolsAclInfo1 != null)
          {
            result.Add(toolsAclInfo1);
          }
          else
          {
            List<ToolsAclInfo> all = milestoneAcls.FindAll((Predicate<ToolsAclInfo>) (milestoneAcl => milestoneAcl.MilestoneID == milestoneID));
            if (all != null && all.Count > 0)
            {
              ToolsAclInfo toolsAclInfo2 = all.Find((Predicate<ToolsAclInfo>) (acl => acl.Access == 1));
              if (toolsAclInfo2 != null)
                result.Add(toolsAclInfo2);
              else
                result.Add(all[0]);
            }
            else
              result.Add(new ToolsAclInfo(231, -1, defaultAccess ? 1 : 0, "", milestoneID));
          }
        }));
      }
      if (flag2)
      {
        List<ToolsAclInfo> roleAcls = ToolsAclDbAccessor.CollectToolsAclInfo(dataSet.Tables[num - 1]);
        Array.ForEach<int>(loanFreeRoleIDs, (Action<int>) (freeRoleID =>
        {
          ToolsAclInfo toolsAclInfo3 = userAcls.Find((Predicate<ToolsAclInfo>) (userAcl => userAcl.RoleID == freeRoleID));
          if (toolsAclInfo3 != null)
          {
            result.Add(toolsAclInfo3);
          }
          else
          {
            List<ToolsAclInfo> all = roleAcls.FindAll((Predicate<ToolsAclInfo>) (roleAcl => roleAcl.RoleID == freeRoleID));
            if (all != null && all.Count > 0)
            {
              ToolsAclInfo toolsAclInfo4 = all.Find((Predicate<ToolsAclInfo>) (acl => acl.Access == 1));
              if (toolsAclInfo4 != null)
                result.Add(toolsAclInfo4);
              else
                result.Add(all[0]);
            }
            else
              result.Add(new ToolsAclInfo(231, freeRoleID, defaultAccess ? 1 : 0, "", "-1"));
          }
        }));
      }
      return result.ToArray();
    }

    private static List<ToolsAclInfo> CollectToolsAclInfo(DataTable table)
    {
      List<ToolsAclInfo> toolsAclInfoList = new List<ToolsAclInfo>();
      for (int index = 0; index < table.Rows.Count; ++index)
      {
        string milestoneID = table.Rows[index]["MilestoneID"] == DBNull.Value ? "-1" : (string) table.Rows[index]["MilestoneID"];
        ToolsAclInfo toolsAclInfo = new ToolsAclInfo(231, table.Rows[index]["roleID"] == DBNull.Value ? -1 : (int) table.Rows[index]["roleID"], (int) table.Rows[index]["access"], "", milestoneID);
        toolsAclInfoList.Add(toolsAclInfo);
      }
      return toolsAclInfoList;
    }

    private static ToolsAclInfo ConvertDataRowToObject(DataRow dr)
    {
      return new ToolsAclInfo(int.Parse(string.Concat(dr["featureID"])), int.Parse(string.Concat(dr["roleID"])), int.Parse(string.Concat(dr["access"])), "", string.Concat(dr["milestoneID"]));
    }

    public static void SynchronizeAdminSetting()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where roleID >-1 and roleID not in (select distinct roleID from Roles)");
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where roleID > -1 and roleID in (select distinct roleID from MilestoneWorksheets where roleID is not NULL)");
      dbQueryBuilder.AppendLine("Update [AclF_ToolsConfig] set access = 1 where personaID = 1");
      dbQueryBuilder.AppendLine("SELECT distinct R.roleID FROM Roles R, MilestoneWorksheets MW  WHERE R.roleID Not IN (select distinct roleID from MilestoneWorksheets where roleID is not NULL ) and R.roleID NOT IN  (SELECT roleID FROM [AclF_ToolsConfig] WHERE personaID = 1)");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
      {
        dbQueryBuilder.Reset();
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
          dbQueryBuilder.AppendLine("Insert into [AclF_ToolsConfig] (featureID, personaID, access, roleID) values(321, 1, 1, " + dataRow["roleID"] + ")");
        dbQueryBuilder.ExecuteNonQuery();
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where milestoneID in (select distinct guid from CustomMilestones where status = 1)");
      dbQueryBuilder.AppendLine("SELECT CM.Guid FROM CustomMilestones CM  WHERE status = 0 AND CM.Guid Not IN (SELECT distinct milestoneID FROM [AclF_ToolsConfig] WHERE personaID = 1)");
      DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
      if (dataRowCollection2 == null || dataRowCollection2.Count <= 0)
        return;
      dbQueryBuilder.Reset();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
        dbQueryBuilder.AppendLine("Insert into [AclF_ToolsConfig] (featureID, personaID, access, milestoneID) values(321, 1, 1, " + SQL.Encode(dataRow["guid"]) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void SynchronizeBrokerSetting(string baseMilestoneID, string currentMilestoneID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("INSERT INTO [AclF_ToolsConfig]");
      dbQueryBuilder.AppendLine("SELECT Distinct A.featureID, A.personaID, " + SQL.EncodeString(currentMilestoneID) + ", A.roleID, A.access");
      dbQueryBuilder.AppendLine("FROM [AclF_ToolsConfig] A WHERE A.MilestoneID = " + SQL.EncodeString(baseMilestoneID) + " AND A.featureID = '321' AND A.personaID!=1");
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static ArrayList GetMissingMilestoneRecords(string personaID, bool defaultAccess)
    {
      int access = 0;
      if (defaultAccess)
        access = 1;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      ArrayList milestoneRecords = new ArrayList();
      dbQueryBuilder.AppendLine("Select MilestoneID from Milestone where convert(varchar, MilestoneID) not in (select distinct milestoneID from [AclF_ToolsConfig] where personaID = " + personaID + ")");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      if (dataRowCollection1 != null && dataRowCollection1.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
          milestoneRecords.Add((object) new ToolsAclInfo(321, -1, access, "", string.Concat(dataRow["milestoneID"])));
      }
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where milestoneID in (select distinct guid from CustomMilestones where status = 1)");
      dbQueryBuilder.AppendLine("Select Guid from CustomMilestones where status = 0 and Guid not in (select distinct milestoneID from [AclF_ToolsConfig] where personaID = " + personaID + ")");
      DataRowCollection dataRowCollection2 = dbQueryBuilder.Execute();
      if (dataRowCollection2 != null && dataRowCollection2.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection2)
          milestoneRecords.Add((object) new ToolsAclInfo(321, -1, access, "", string.Concat(dataRow["Guid"])));
      }
      return milestoneRecords;
    }

    private static ArrayList GetMissingMilestoneFreeRoleRecord(string personaID, bool defaultAccess)
    {
      ArrayList milestoneFreeRoleRecord = new ArrayList();
      int access = 0;
      if (defaultAccess)
        access = 1;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where roleID >-1 and roleID not in (select distinct roleID from Roles)");
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig_User] where roleID >-1 and roleID not in (select distinct roleID from Roles)");
      dbQueryBuilder.AppendLine("delete [AclF_ToolsConfig] where roleID > -1 and roleID in (select distinct roleID from Milestones where roleID is not NULL)");
      dbQueryBuilder.AppendLine("SELECT distinct R.roleID, R.roleName FROM Roles R, Milestones M  WHERE R.roleID Not IN (select distinct roleID from Milestones where roleID is not NULL ) and R.roleID NOT IN  (SELECT distinct roleID FROM [AclF_ToolsConfig] WHERE roleID >0 and personaID = " + personaID + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          milestoneFreeRoleRecord.Add((object) new ToolsAclInfo(321, int.Parse(string.Concat(dataRow["roleID"])), access, string.Concat(dataRow["roleName"]), "-1"));
      }
      return milestoneFreeRoleRecord;
    }
  }
}
