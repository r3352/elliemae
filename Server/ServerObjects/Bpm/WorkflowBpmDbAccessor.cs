// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Bpm.WorkflowBpmDbAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Server.ServerObjects.eFolder;
using EllieMae.EMLite.Workflow;
using PostSharp.Aspects;
using PostSharp.ImplementationDetails_93a154f1;
using PostSharp.Reflection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Bpm
{
  public static class WorkflowBpmDbAccessor
  {
    private const string worksheetsTableName = "MilestoneWorksheets�";
    private const string rolesTableName = "Roles�";
    private const string roleMappingTableName = "RolesMapping�";
    private const string milestoneCacheName = "WorkflowMilestones�";
    private const string milestoneTemplateCacheName = "WorkflowMilestoneTemplates�";
    private const string defaultSettingsCacheName = "MilestoneTemplateDefaultSettings�";
    private static ConcurrentDictionary<string, TimedStruct<Hashtable>> milestoneRoleCache = new ConcurrentDictionary<string, TimedStruct<Hashtable>>();

    public static WorksheetInfo[] GetAllMsWorksheetInfos()
    {
      return WorkflowBpmDbAccessor.getMsWorksheetInfoFromDatabase();
    }

    public static string[] GetMilestoneIDsByRoleID(int roleID)
    {
      return WorkflowBpmDbAccessor.getMilestoneIDsByRoleIDFromDatabase(roleID);
    }

    public static WorksheetInfo GetMsWorksheetInfo(string milestoneID)
    {
      return WorkflowBpmDbAccessor.getMsWorksheetInfoFromDatabase(milestoneID);
    }

    public static Hashtable GetMilestoneRoles()
    {
      return WorkflowBpmDbAccessor.getMilestoneRolesFromDatabase();
    }

    public static void SetMsWorksheetInfo(WorksheetInfo wsInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("MilestoneWorksheets"))
      {
        WorkflowBpmDbAccessor.saveWorksheetInfoToDatabase(wsInfo);
        current.Cache.Remove("MilestoneWorksheets");
      }
      WorkflowBpmDbAccessor.raiseCacheControlEvent();
    }

    public static void UpdateMsWorksheetAlertMessages(Dictionary<string, string> alertMsgsToUpdate)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("MilestoneWorksheets"))
      {
        WorkflowBpmDbAccessor.updateAlertMessagesInDatabase(alertMsgsToUpdate);
        current.Cache.Remove("MilestoneWorksheets");
      }
      WorkflowBpmDbAccessor.raiseCacheControlEvent();
    }

    public static void SetOrUpdateMsWorksheetAlertMessages(
      Dictionary<WorksheetInfo, string> alertMsgsToUpdate)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("MilestoneWorksheets"))
      {
        if (alertMsgsToUpdate == null || alertMsgsToUpdate.Count == 0)
          return;
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        foreach (WorksheetInfo key in alertMsgsToUpdate.Keys)
        {
          string str = alertMsgsToUpdate[key];
          dbQueryBuilder.Append("select * from MilestoneWorksheets where milestoneId = '" + key.MilestoneID + "'");
          if (dbQueryBuilder.Execute().Count > 0)
          {
            dbQueryBuilder.Reset();
            dbQueryBuilder.AppendLine("update MilestoneWorksheets set [alertMessage] = " + (str == null ? "null" : SQL.Encode((object) str.Trim())) + " where [milestoneID] = " + SQL.Encode((object) key.MilestoneID));
            dbQueryBuilder.ExecuteNonQuery();
          }
          else
            WorkflowBpmDbAccessor.saveWorksheetInfoToDatabase(key);
        }
        current.Cache.Remove("MilestoneWorksheets");
      }
      WorkflowBpmDbAccessor.raiseCacheControlEvent();
    }

    public static Hashtable GetAllMilestoneAlertMessages()
    {
      return WorkflowBpmDbAccessor.getAllMilestoneAlertMessagesFromDatabase();
    }

    public static RoleInfo[] GetAllRoleFunctions()
    {
      return ClientContext.GetCurrent().Cache.Get<RoleInfo[]>("Roles", new Func<RoleInfo[]>(WorkflowBpmDbAccessor.getAllRoleFunctionsFromDatabase), CacheSetting.Low);
    }

    public static RoleInfo GetRoleFunction(int roleID)
    {
      return ClientContext.GetCurrent().Settings.CacheSetting == CacheSetting.Disabled ? WorkflowBpmDbAccessor.getRoleFunctionFromDatabase(roleID) : ((IEnumerable<RoleInfo>) WorkflowBpmDbAccessor.GetAllRoleFunctions()).ToList<RoleInfo>().FirstOrDefault<RoleInfo>((System.Func<RoleInfo, bool>) (role => role.RoleID == roleID));
    }

    public static RoleInfo[] GetRoleFunctionsByPersonaID(int personaID)
    {
      return ClientContext.GetCurrent().Settings.CacheSetting == CacheSetting.Disabled ? WorkflowBpmDbAccessor.getRoleFunctionsByPersonaIDFromDatabase(personaID) : ((IEnumerable<RoleInfo>) WorkflowBpmDbAccessor.GetAllRoleFunctions()).Where<RoleInfo>((System.Func<RoleInfo, bool>) (role => role.ContainsPersona(personaID))).ToList<RoleInfo>().ToArray();
    }

    public static RoleInfo[] GetRoleFunctionsByUserID(string userID)
    {
      return WorkflowBpmDbAccessor.getRoleFunctionsByUserIDFromDatabase(userID);
    }

    public static int SetRoleFunction(RoleSummaryInfo roleInfo)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("Roles"))
      {
        int database = WorkflowBpmDbAccessor.saveRoleFunctionToDatabase(roleInfo);
        current.Cache.Remove("Roles");
        return database;
      }
    }

    public static void DeleteRoleFunction(int roleId)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("Roles"))
      {
        WorkflowBpmDbAccessor.deleteRoleFunctionFromDatabase(roleId);
        current.Cache.Remove("Roles");
        current.Cache.Remove("RolesMapping");
      }
    }

    public static RolesMappingInfo[] GetAllRoleMappingInfos()
    {
      return ClientContext.GetCurrent().Cache.Get<RolesMappingInfo[]>("RolesMapping", new Func<RolesMappingInfo[]>(WorkflowBpmDbAccessor.getAllRoleMappingInfosFromDatabase), CacheSetting.Low);
    }

    public static RoleMappingsDetails[] GetStandardRoleMapping()
    {
      return WorkflowBpmDbAccessor.getStandardRoleMappingFromDatabase();
    }

    public static RolesMappingInfo GetRoleMappingInfo(RealWorldRoleID realWorldRoleID)
    {
      return ((IEnumerable<RolesMappingInfo>) WorkflowBpmDbAccessor.GetAllRoleMappingInfos()).FirstOrDefault<RolesMappingInfo>((System.Func<RolesMappingInfo, bool>) (mapInfo => mapInfo.RealWorldRoleID == realWorldRoleID));
    }

    public static void UpdateRoleMappingInfos(RolesMappingInfo[] rolesMappingInfos)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("RolesMapping"))
      {
        WorkflowBpmDbAccessor.saveRoleMappingInfoToDatabase(rolesMappingInfos);
        current.Cache.Remove("RolesMapping");
      }
    }

    public static RolesMappingInfo[] GetUsersRoleMapping(string userID)
    {
      return WorkflowBpmDbAccessor.getUsersRoleMappingFromDatabase(userID);
    }

    public static RolesMappingInfo GetUsersRoleMapping(
      string userID,
      RealWorldRoleID realWorldRoleID)
    {
      return WorkflowBpmDbAccessor.getUsersRoleMappingFromDatabase(userID, realWorldRoleID);
    }

    public static RoleInfo[] GetUsersAllowedRoles(string userID)
    {
      return WorkflowBpmDbAccessor.getUsersAllowedRolesFromDatabase(userID);
    }

    public static RoleSummaryInfo[] GetUserEligibleRoles(string userID)
    {
      return WorkflowBpmDbAccessor.getUserEligibleRoles(userID);
    }

    public static void InvalidateRoleCache() => ClientContext.GetCurrent().Cache.Remove("Roles");

    public static string[] GetPersonaNamesFromRoleNames(string[] roles)
    {
      string str = string.Empty;
      for (int index = 0; index < roles.Length; ++index)
        str = str + ("'" + roles[index]) + (index == roles.Length - 1 ? "'" : "',");
      if (string.IsNullOrWhiteSpace(str))
        return (string[]) null;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct personaName from RolePersonas rp inner join  Personas p on rp.personaID= p.personaID");
      dbQueryBuilder.AppendLine(" inner join Roles r on r.roleID=rp.roleID");
      dbQueryBuilder.AppendLine(" where r.roleName in (" + str + ")");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      string[] namesFromRoleNames = new string[dataRowCollection.Count];
      int index1 = 0;
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        namesFromRoleNames[index1] = dataRow["personaName"].ToString();
        ++index1;
      }
      return namesFromRoleNames;
    }

    private static WorksheetInfo[] getMsWorksheetInfoFromDatabase()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT M.MilestoneID, W.SetRoleFirst, W.AlertMessage, W.lastModTime, W.fieldSummaryFormID, F.Name as FormName, R.RoleID, R.roleName, R.roleAbbr, R.Protected, M.Archived FROM Milestones M");
      dbQueryBuilder.AppendLine("inner join MilestoneWorksheets W on M.MilestoneID = W.MilestoneID");
      dbQueryBuilder.AppendLine("Left join Roles R On W.roleID = R.roleID");
      dbQueryBuilder.AppendLine("Left join InputForms F On W.fieldSummaryFormID = F.FormID");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ArrayList arrayList = new ArrayList();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        RoleSummaryInfo role = (RoleSummaryInfo) null;
        if (string.Concat(dataRow["roleName"]) != "")
          role = new RoleSummaryInfo((int) dataRow["RoleID"], string.Concat(dataRow["RoleName"]), string.Concat(dataRow["RoleAbbr"]), (bool) dataRow["Protected"]);
        bool isMsArchived = false;
        if (dataRow["archived"] != DBNull.Value)
          isMsArchived = (byte) dataRow["archived"] == (byte) 1;
        InputFormInfo fieldSummaryForm = (InputFormInfo) null;
        string mname = (string) SQL.Decode(dataRow["FormName"]);
        string formId = (string) SQL.Decode(dataRow["fieldSummaryFormID"]);
        if (mname != null)
          fieldSummaryForm = new InputFormInfo(formId, mname);
        WorksheetInfo worksheetInfo = new WorksheetInfo(string.Concat(dataRow["milestoneID"]), role, Convert.ToInt32(SQL.Decode(dataRow["setRoleFirst"], (object) 0)).Equals(1), string.Concat(dataRow["alertMessage"]), (DateTime) dataRow["lastModTime"], isMsArchived, fieldSummaryForm);
        arrayList.Add((object) worksheetInfo);
      }
      return (WorksheetInfo[]) arrayList.ToArray(typeof (WorksheetInfo));
    }

    private static WorksheetInfo getMsWorksheetInfoFromDatabase(string milestoneID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT W.*, F.Name as FormName, R.roleName, R.roleAbbr, R.Protected, C.status FROM MilestoneWorksheets W");
      dbQueryBuilder.AppendLine("Left join Roles R On W.roleID = R.roleID");
      dbQueryBuilder.AppendLine("Left join CustomMilestones C On W.milestoneID = C.Guid");
      dbQueryBuilder.AppendLine("Left join InputForms F On W.fieldSummaryFormID = F.FormID");
      dbQueryBuilder.AppendLine("Where W.milestoneID = " + SQL.Encode((object) milestoneID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return (WorksheetInfo) null;
      DataRow dataRow = dataRowCollection[0];
      RoleSummaryInfo role = (RoleSummaryInfo) null;
      int roleID = (int) SQL.Decode(dataRow["roleID"], (object) -1);
      if ((string) SQL.Decode(dataRow["roleName"]) != null)
        role = new RoleSummaryInfo(roleID, string.Concat(dataRow["roleName"]), string.Concat(dataRow["roleAbbr"]), (bool) dataRow["protected"]);
      bool isMsArchived = false;
      if (dataRow["status"] != DBNull.Value)
        isMsArchived = (byte) dataRow["status"] == (byte) 1;
      InputFormInfo fieldSummaryForm = (InputFormInfo) null;
      string mname = (string) SQL.Decode(dataRow["FormName"]);
      string formId = (string) SQL.Decode(dataRow["fieldSummaryFormID"]);
      if (mname != null)
        fieldSummaryForm = new InputFormInfo(formId, mname);
      return new WorksheetInfo(milestoneID, role, Convert.ToInt32(SQL.Decode(dataRow["setRoleFirst"], (object) 0)).Equals(1), string.Concat(dataRow["alertMessage"]), (DateTime) dataRow["lastModTime"], isMsArchived, fieldSummaryForm);
    }

    private static string[] getMilestoneIDsByRoleIDFromDatabase(int roleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneWorksheets");
      DbValue key = new DbValue(nameof (roleID), (object) roleID);
      dbQueryBuilder.SelectFrom(table, new string[1]
      {
        "milestoneID"
      }, key);
      return dbQueryBuilder.Execute(DbTransactionType.None).Cast<DataRow>().Select<DataRow, string>((System.Func<DataRow, string>) (r => r["milestoneID"].ToString())).ToList<string>().ToArray();
    }

    [PgReady]
    private static Hashtable getMilestoneRolesFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        string key = (string) null;
        try
        {
          ClientContext current = ClientContext.GetCurrent();
          key = current.ClientID + "_" + current.InstanceName;
          if (WorkflowBpmDbAccessor.milestoneRoleCache.ContainsKey(key))
          {
            if (WorkflowBpmDbAccessor.milestoneRoleCache[key].StillExists())
              return WorkflowBpmDbAccessor.milestoneRoleCache[key].data;
          }
        }
        catch (Exception ex)
        {
        }
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * from MilestoneRole;");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None);
        Hashtable d = new Hashtable();
        for (int index = 0; index < dataRowCollection.Count; ++index)
          d[dataRowCollection[index]["MilestoneID"]] = (object) new RoleSummaryInfo((int) dataRowCollection[index]["RoleID"], (string) dataRowCollection[index]["RoleName"], (string) dataRowCollection[index]["RoleAbbr"], false);
        if (key != null)
          WorkflowBpmDbAccessor.milestoneRoleCache[key] = new TimedStruct<Hashtable>(d, 5);
        return d;
      }
      string key1 = (string) null;
      try
      {
        ClientContext current = ClientContext.GetCurrent();
        key1 = current.ClientID + "_" + current.InstanceName;
        if (WorkflowBpmDbAccessor.milestoneRoleCache.ContainsKey(key1))
        {
          if (WorkflowBpmDbAccessor.milestoneRoleCache[key1].StillExists())
            return WorkflowBpmDbAccessor.milestoneRoleCache[key1].data;
        }
      }
      catch (Exception ex)
      {
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from MilestoneRole");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute(DbTransactionType.None);
      Hashtable d1 = new Hashtable();
      for (int index = 0; index < dataRowCollection1.Count; ++index)
        d1[dataRowCollection1[index]["MilestoneID"]] = (object) new RoleSummaryInfo((int) dataRowCollection1[index]["RoleID"], (string) dataRowCollection1[index]["RoleName"], (string) dataRowCollection1[index]["RoleAbbr"], false);
      if (key1 != null)
        WorkflowBpmDbAccessor.milestoneRoleCache[key1] = new TimedStruct<Hashtable>(d1, 5);
      return d1;
    }

    [PgReady]
    private static Hashtable getAllMilestoneAlertMessagesFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        Hashtable messagesFromDatabase = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT cm.Name, mw.alertMessage");
        pgDbQueryBuilder.AppendLine("\tFROM MilestoneWorksheets AS mw INNER JOIN Milestones AS cm ON cm.MilestoneID = mw.milestoneID");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.None);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        for (int index = 0; index < dataRowCollection.Count; ++index)
        {
          string key = (string) SQL.Decode(dataRowCollection[index]["Name"], (object) "");
          string str = string.Empty;
          object obj = dataRowCollection[index]["alertMessage"];
          if (obj != DBNull.Value)
            str = (string) obj;
          if (!messagesFromDatabase.ContainsKey((object) key) && str != string.Empty)
            messagesFromDatabase.Add((object) key, (object) str);
        }
        return messagesFromDatabase;
      }
      Hashtable messagesFromDatabase1 = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("SELECT cm.Name, mw.alertMessage");
      dbQueryBuilder1.AppendLine("\tFROM MilestoneWorksheets AS mw INNER JOIN Milestones AS cm ON cm.MilestoneID = mw.milestoneID");
      DataRowCollection dataRowCollection1 = dbQueryBuilder1.Execute(DbTransactionType.None);
      string empty3 = string.Empty;
      string empty4 = string.Empty;
      for (int index = 0; index < dataRowCollection1.Count; ++index)
      {
        string key = (string) SQL.Decode(dataRowCollection1[index]["Name"], (object) "");
        string str = string.Empty;
        object obj = dataRowCollection1[index]["alertMessage"];
        if (obj != DBNull.Value)
          str = (string) obj;
        if (!messagesFromDatabase1.ContainsKey((object) key) && str != string.Empty)
          messagesFromDatabase1.Add((object) key, (object) str);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder2.AppendLine("SELECT m.Name, mw.alertMessage");
      dbQueryBuilder2.AppendLine("\tFROM MilestoneWorksheets AS mw INNER JOIN Milestones AS m ON m.MilestoneId = mw.milestoneID");
      DataRowCollection dataRowCollection2 = dbQueryBuilder2.Execute();
      if (dataRowCollection2.Count > 0)
      {
        for (int index = 0; index < dataRowCollection2.Count; ++index)
        {
          string key = (string) SQL.Decode(dataRowCollection2[index]["Name"], (object) "");
          string str = string.Empty;
          object obj = dataRowCollection2[index]["alertMessage"];
          if (obj != DBNull.Value)
            str = (string) obj;
          if (!messagesFromDatabase1.ContainsKey((object) key) && str != string.Empty)
            messagesFromDatabase1.Add((object) key, (object) str);
        }
      }
      return messagesFromDatabase1;
    }

    private static void updateAlertMessagesInDatabase(Dictionary<string, string> alertMsgsToUpdate)
    {
      if (alertMsgsToUpdate == null || alertMsgsToUpdate.Count == 0)
        return;
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      foreach (string key in alertMsgsToUpdate.Keys)
      {
        string str = alertMsgsToUpdate[key];
        dbQueryBuilder.AppendLine("update MilestoneWorksheets set [alertMessage] = " + (str == null ? "null" : SQL.Encode((object) str.Trim())) + " where [milestoneID] = " + SQL.Encode((object) key));
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static void saveWorksheetInfoToDatabase(WorksheetInfo wsInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneWorksheets");
      DbValue key = new DbValue("milestoneID", (object) wsInfo.MilestoneID);
      dbQueryBuilder.DeleteFrom(table, key);
      DbValueList values = new DbValueList();
      values.Add(key);
      if (wsInfo.Role != null && wsInfo.Role.RoleID > 0)
      {
        values.Add("roleID", (object) wsInfo.Role.RoleID);
        values.Add("setRoleFirst", (object) wsInfo.SetRoleFirst, (IDbEncoder) DbEncoding.Flag);
      }
      if (wsInfo.FieldSummaryForm != (InputFormInfo) null)
        values.Add("fieldSummaryFormID", (object) wsInfo.FieldSummaryForm.FormID);
      values.Add("alertMessage", (object) wsInfo.AlertMessage);
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    private static RoleInfo[] getAllRoleFunctionsFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * from Roles;");
        pgDbQueryBuilder.AppendLine("SELECT * from RolePersonas order by personaID;");
        pgDbQueryBuilder.AppendLine("SELECT * from RoleUserGroups order by groupID;");
        return WorkflowBpmDbAccessor.dataSetToRoleInfos(pgDbQueryBuilder.ExecuteSetQuery());
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from Roles");
      dbQueryBuilder.AppendLine("SELECT * from RolePersonas order by personaID");
      dbQueryBuilder.AppendLine("SELECT * from RoleUserGroups order by groupID");
      return WorkflowBpmDbAccessor.dataSetToRoleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static RoleInfo getRoleFunctionFromDatabase(int roleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from Roles where roleID = " + (object) roleID);
      dbQueryBuilder.AppendLine("SELECT * from RolePersonas where roleID = " + (object) roleID + " order by personaID");
      dbQueryBuilder.AppendLine("SELECT * from RoleUserGroups where roleID = " + (object) roleID + " order by groupID");
      RoleInfo[] roleInfos = WorkflowBpmDbAccessor.dataSetToRoleInfos(dbQueryBuilder.ExecuteSetQuery());
      return roleInfos.Length == 0 ? (RoleInfo) null : roleInfos[0];
    }

    private static RoleInfo[] getRoleFunctionsByPersonaIDFromDatabase(int personaID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from Roles where roleID in (select distinct roleID from RolePersonas where personaId = " + (object) personaID + ")");
      dbQueryBuilder.AppendLine("SELECT * from RolePersonas where roleID in (select distinct roleID from RolePersonas where personaId = " + (object) personaID + ") order by personaID");
      dbQueryBuilder.AppendLine("SELECT * from RoleUserGroups where roleID in (select distinct roleID from RolePersonas where personaId = " + (object) personaID + ") order by groupID");
      return WorkflowBpmDbAccessor.dataSetToRoleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static RoleInfo[] getRoleFunctionsByUserIDFromDatabase(string userID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from Roles where roleID in");
      dbQueryBuilder.AppendLine("   (select distinct rp.roleID from RolePersonas rp inner join UserPersona up on rp.personaID = up.personaID where up.userID = " + SQL.Encode((object) userID) + ")");
      dbQueryBuilder.AppendLine("SELECT * from RolePersonas where roleID in");
      dbQueryBuilder.AppendLine("   (select distinct rp.roleID from RolePersonas rp inner join UserPersona up on rp.personaID = up.personaID where up.userID = " + SQL.Encode((object) userID) + ")");
      dbQueryBuilder.AppendLine("SELECT * from RoleUserGroups where roleID in");
      dbQueryBuilder.AppendLine("   (select distinct rp.roleID from RolePersonas rp inner join UserPersona up on rp.personaID = up.personaID where up.userID = " + SQL.Encode((object) userID) + ")");
      return WorkflowBpmDbAccessor.dataSetToRoleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    [PgReady]
    private static RoleInfo[] dataSetToRoleInfos(DataSet data)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        DataTable table1 = data.Tables[0];
        DataTable table2 = data.Tables[1];
        DataTable table3 = data.Tables[2];
        DataRelation relation1 = data.Relations.Add(table1.Columns["roleID"], table2.Columns["roleID"]);
        DataRelation relation2 = data.Relations.Add(table1.Columns["roleID"], table3.Columns["roleID"]);
        RoleInfo[] roleInfos = new RoleInfo[table1.Rows.Count];
        for (int index1 = 0; index1 < roleInfos.Length; ++index1)
        {
          DataRow row = table1.Rows[index1];
          DataRow[] childRows1 = row.GetChildRows(relation1);
          int[] personaIDs = new int[childRows1.Length];
          for (int index2 = 0; index2 < personaIDs.Length; ++index2)
            personaIDs[index2] = (int) childRows1[index2]["personaID"];
          DataRow[] childRows2 = row.GetChildRows(relation2);
          int[] groupIDs = new int[childRows2.Length];
          for (int index3 = 0; index3 < groupIDs.Length; ++index3)
            groupIDs[index3] = (int) childRows2[index3]["groupID"];
          roleInfos[index1] = new RoleInfo((int) row["roleID"], string.Concat(row["roleName"]), string.Concat(row["roleAbbr"]), SQL.DecodeBoolean(row["protected"]), personaIDs, groupIDs);
        }
        return roleInfos;
      }
      DataTable table4 = data.Tables[0];
      DataTable table5 = data.Tables[1];
      DataTable table6 = data.Tables[2];
      DataRelation relation3 = data.Relations.Add(table4.Columns["roleID"], table5.Columns["roleID"]);
      DataRelation relation4 = data.Relations.Add(table4.Columns["roleID"], table6.Columns["roleID"]);
      RoleInfo[] roleInfos1 = new RoleInfo[table4.Rows.Count];
      for (int index4 = 0; index4 < roleInfos1.Length; ++index4)
      {
        DataRow row = table4.Rows[index4];
        DataRow[] childRows3 = row.GetChildRows(relation3);
        int[] personaIDs = new int[childRows3.Length];
        for (int index5 = 0; index5 < personaIDs.Length; ++index5)
          personaIDs[index5] = (int) childRows3[index5]["personaID"];
        DataRow[] childRows4 = row.GetChildRows(relation4);
        int[] groupIDs = new int[childRows4.Length];
        for (int index6 = 0; index6 < groupIDs.Length; ++index6)
          groupIDs[index6] = (int) childRows4[index6]["groupID"];
        roleInfos1[index4] = new RoleInfo((int) row["roleID"], string.Concat(row["roleName"]), string.Concat(row["roleAbbr"]), (bool) row["protected"], personaIDs, groupIDs);
      }
      return roleInfos1;
    }

    private static RoleSummaryInfo[] dataSetToRoleInfosForUserEligibleRoles(DataSet data)
    {
      ClientContext.GetCurrent();
      DataTable table = data.Tables[0];
      RoleSummaryInfo[] userEligibleRoles = new RoleSummaryInfo[table.Rows.Count];
      for (int index = 0; index < userEligibleRoles.Length; ++index)
      {
        DataRow row = table.Rows[index];
        userEligibleRoles[index] = new RoleSummaryInfo((int) row["roleID"], string.Concat(row["roleName"]), string.Concat(row["roleAbbr"]), (bool) row["protected"]);
      }
      return userEligibleRoles;
    }

    private static int saveRoleFunctionToDatabase(RoleSummaryInfo roleInfo)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("Roles");
      DbValueList values = new DbValueList();
      values.Add("roleName", (object) roleInfo.Name);
      values.Add("roleAbbr", (object) roleInfo.RoleAbbr);
      values.Add("protected", (object) roleInfo.Protected, (IDbEncoder) DbEncoding.Flag);
      dbQueryBuilder.Declare("@roleId", "int");
      DbValue key = new DbValue("roleID", (object) "@roleId", (IDbEncoder) DbEncoding.None);
      if (roleInfo.RoleID <= 0)
      {
        dbQueryBuilder.InsertInto(table1, values, true, false);
        dbQueryBuilder.SelectIdentity("@roleId");
      }
      else
      {
        dbQueryBuilder.SelectVar("@roleId", (object) roleInfo.RoleID);
        dbQueryBuilder.Update(table1, values, key);
      }
      if (roleInfo is RoleInfo)
      {
        RoleInfo roleInfo1 = (RoleInfo) roleInfo;
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("RolePersonas");
        dbQueryBuilder.DeleteFrom(table2, key);
        for (int index = 0; index < roleInfo1.PersonaIDs.Length; ++index)
          dbQueryBuilder.InsertInto(table2, new DbValueList()
          {
            key,
            {
              "personaID",
              (object) roleInfo1.PersonaIDs[index]
            }
          }, true, false);
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("RoleUserGroups");
        dbQueryBuilder.DeleteFrom(table3, key);
        for (int index = 0; index < roleInfo1.UserGroupIDs.Length; ++index)
          dbQueryBuilder.InsertInto(table3, new DbValueList()
          {
            key,
            {
              "groupID",
              (object) roleInfo1.UserGroupIDs[index]
            }
          }, true, false);
      }
      dbQueryBuilder.Select("@roleId");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    [PgReady]
    private static void deleteRoleFunctionFromDatabase(int roleID)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        DbValue key = new DbValue(nameof (roleID), (object) roleID);
        pgDbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RolePersonas"), key);
        pgDbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RoleUserGroups"), key);
        pgDbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("Roles"), key);
        pgDbQueryBuilder.ExecuteNonQuery();
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbValue key = new DbValue(nameof (roleID), (object) roleID);
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RolePersonas"), key);
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RoleUserGroups"), key);
        dbQueryBuilder.DeleteFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("Roles"), key);
        dbQueryBuilder.ExecuteNonQuery();
      }
    }

    [PgReady]
    private static RolesMappingInfo[] getAllRoleMappingInfosFromDatabase()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RolesMapping"));
        return WorkflowBpmDbAccessor.dataRowsToRolesMappingInfos((ICollection) pgDbQueryBuilder.Execute(DbTransactionType.None));
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.SelectFrom(EllieMae.EMLite.Server.DbAccessManager.GetTable("RolesMapping"));
      return WorkflowBpmDbAccessor.dataRowsToRolesMappingInfos((ICollection) dbQueryBuilder.Execute(DbTransactionType.None));
    }

    private static RoleMappingsDetails[] getStandardRoleMappingFromDatabase()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select rm.roleID,rm.realWorldRoleID,r.roleName from RolesMapping rm inner join Roles r on rm.roleID=r.roleID");
      return WorkflowBpmDbAccessor.dataRowsToStandardRoleMapping((ICollection) dbQueryBuilder.Execute(DbTransactionType.None));
    }

    private static void saveRoleMappingInfoToDatabase(RolesMappingInfo[] rolesMappingInfos)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("RolesMapping");
      foreach (RolesMappingInfo rolesMappingInfo in rolesMappingInfos)
      {
        DbValue key = new DbValue("realWorldRoleID", (object) (int) rolesMappingInfo.RealWorldRoleID);
        dbQueryBuilder.DeleteFrom(table, key);
        int[] roleIdList = rolesMappingInfo.RoleIDList;
        if (roleIdList != null && roleIdList.Length != 0)
        {
          foreach (int num in roleIdList)
            dbQueryBuilder.InsertInto(table, new DbValueList()
            {
              key,
              {
                "roleID",
                (object) num
              }
            }, true, false);
        }
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    [PgReady]
    private static RolesMappingInfo[] getUsersRoleMappingFromDatabase(string userID)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("SELECT DISTINCT RM.* FROM Users U INNER JOIN UserPersona UP on U.userid = UP.userid");
        pgDbQueryBuilder.AppendLine("   INNER JOIN RolePersonas RP on UP.personaID = RP.personaID");
        pgDbQueryBuilder.AppendLine("   INNER JOIN RolesMapping RM on RP.roleID = RM.roleID");
        pgDbQueryBuilder.AppendLine("   WHERE U.userid = " + SQL.Encode((object) userID));
        return WorkflowBpmDbAccessor.dataRowsToRolesMappingInfos((ICollection) pgDbQueryBuilder.Execute(DbTransactionType.None));
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT RM.* FROM Users U INNER JOIN UserPersona UP on U.userid = UP.userid");
      dbQueryBuilder.AppendLine("   INNER JOIN RolePersonas RP on UP.personaID = RP.personaID");
      dbQueryBuilder.AppendLine("   INNER JOIN RolesMapping RM on RP.roleID = RM.roleID");
      dbQueryBuilder.AppendLine("   WHERE U.userid = " + SQL.Encode((object) userID));
      return WorkflowBpmDbAccessor.dataRowsToRolesMappingInfos((ICollection) dbQueryBuilder.Execute(DbTransactionType.None));
    }

    public static Dictionary<int, List<string>> GetAllPersonaRoleMappings()
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct  r.roleName, rp.personaId from RolePersonas rp inner join Roles r on r.roleID = rp.roleID inner join  RolesMapping rm on rm.roleID = rp.roleId");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
      Dictionary<int, List<string>> personaRoleMappings = new Dictionary<int, List<string>>();
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string str = SQL.DecodeString(dataRow["roleName"]);
        int key = SQL.DecodeInt(dataRow["personaId"]);
        if (personaRoleMappings.ContainsKey(key))
          personaRoleMappings[key].Add(str);
        else
          personaRoleMappings.Add(key, new List<string>()
          {
            str
          });
      }
      return personaRoleMappings;
    }

    private static RolesMappingInfo getUsersRoleMappingFromDatabase(
      string userID,
      RealWorldRoleID realWorldRoleID)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT DISTINCT RM.* FROM Users U INNER JOIN UserPersona UP on U.userid = UP.userid");
      dbQueryBuilder.AppendLine("   INNER JOIN RolePersonas RP on UP.personaID = RP.personaID");
      dbQueryBuilder.AppendLine("   INNER JOIN RolesMapping RM on RP.roleID = RM.roleID");
      dbQueryBuilder.AppendLine("   WHERE U.userid = " + SQL.Encode((object) userID));
      dbQueryBuilder.AppendLine("   AND RM.realWorldRoleID = " + SQL.Encode((object) (int) realWorldRoleID));
      RolesMappingInfo[] rolesMappingInfos = WorkflowBpmDbAccessor.dataRowsToRolesMappingInfos((ICollection) dbQueryBuilder.Execute(DbTransactionType.None));
      return rolesMappingInfos.Length != 0 ? rolesMappingInfos[0] : (RolesMappingInfo) null;
    }

    [PgReady]
    private static RoleInfo[] getUsersAllowedRolesFromDatabase(string userID)
    {
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        string str = "SELECT DISTINCT RP.RoleID FROM Users U INNER JOIN UserPersona UP on U.userid = UP.userid INNER JOIN RolePersonas RP on UP.personaID = RP.personaID WHERE U.userid = @userid";
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * from Roles where roleID in (" + str + ");");
        pgDbQueryBuilder.AppendLine("SELECT * from RolePersonas where roleID in (" + str + ") order by personaID;");
        pgDbQueryBuilder.AppendLine("SELECT * from RoleUserGroups where roleID in (" + str + ") order by groupID;");
        DbCommandParameter[] array = new DbCommandParameter("userid", (object) userID.TrimEnd(), DbType.AnsiString).ToArray();
        return WorkflowBpmDbAccessor.dataSetToRoleInfos(pgDbQueryBuilder.ExecuteSetQuery(DbTransactionType.Default, array));
      }
      string str1 = "SELECT DISTINCT RP.RoleID FROM Users U INNER JOIN UserPersona UP on U.userid = UP.userid INNER JOIN RolePersonas RP on UP.personaID = RP.personaID WHERE U.userid = " + SQL.Encode((object) userID);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT * from Roles where roleID in (" + str1 + ")");
      dbQueryBuilder.AppendLine("SELECT * from RolePersonas where roleID in (" + str1 + ") order by personaID");
      dbQueryBuilder.AppendLine("SELECT * from RoleUserGroups where roleID in (" + str1 + ") order by groupID");
      return WorkflowBpmDbAccessor.dataSetToRoleInfos(dbQueryBuilder.ExecuteSetQuery());
    }

    private static RoleSummaryInfo[] getUserEligibleRoles(string userID)
    {
      ClientContext.GetCurrent(false);
      string text = "SELECT r.roleid, r.roleName,r.protected,r.roleAbbr FROM RolePersonas rp INNER JOIN UserPersona up ON rp.personaID=up.personaID INNER JOIN Roles r ON rp.roleID=r.roleID WHERE up.userID = " + SQL.Encode((object) userID) + " Union SELECT  r.roleid, r.roleName,r.protected,r.roleAbbr FROM AclGroupMembers agm  INNER JOIN RoleUserGroups rg ON rg.groupID=AGM.GroupID INNER JOIN Roles r ON rg.roleID=r.roleID WHERE UserID = " + SQL.Encode((object) userID);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(text);
      return WorkflowBpmDbAccessor.dataSetToRoleInfosForUserEligibleRoles(dbQueryBuilder.ExecuteSetQuery());
    }

    public static string GetUserEligibleRolesAndRoleMappingsQuery(List<string> contactIDs)
    {
      string str = string.Join("','", (IEnumerable<string>) contactIDs);
      return "SELECT r.roleid, r.roleName,r.protected,r.roleAbbr, rm.realWorldRoleID, up.userid FROM RolePersonas rp INNER JOIN UserPersona up ON rp.personaID=up.personaID INNER JOIN Roles r ON rp.roleID=r.roleID LEFT JOIN RolesMapping rm on r.roleID = rm.roleID WHERE up.userID in ( '" + str + "' )  Union SELECT  r.roleid, r.roleName,r.protected,r.roleAbbr, rm.realWorldRoleID, agm.UserID FROM AclGroupMembers agm  INNER JOIN RoleUserGroups rg ON rg.groupID=AGM.GroupID INNER JOIN Roles r ON rg.roleID=r.roleID LEFT JOIN RolesMapping rm on r.roleID = rm.roleID WHERE UserID in ( '" + str + "' ) ";
    }

    public static RoleSummaryInfo DataRowToRoleSummaryInfo(DataRow row)
    {
      return new RoleSummaryInfo(SQL.DecodeInt(row["roleID"]), SQL.DecodeString(row["roleName"]) ?? "", SQL.DecodeString(row["roleAbbr"]) ?? "", SQL.DecodeBoolean(row["protected"]));
    }

    public static RoleMappingsDetails DataRowToRoleMappingDetails(DataRow row)
    {
      return new RoleMappingsDetails()
      {
        RealWorldRoleId = SQL.DecodeString(row["realWorldRoleID"]),
        RealWorldRoleName = Enum.ToObject(typeof (RealWorldRoleID), SQL.DecodeInt(row["realWorldRoleID"])).ToString(),
        UserDefinedRoleId = SQL.DecodeString(row["roleID"]),
        UserDefinedRoleName = SQL.DecodeString(row["roleName"])
      };
    }

    public static Dictionary<string, Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>> GetUsersRolesAndRoleMappings(
      List<string> contactIDs)
    {
      Dictionary<string, Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>> rolesAndRoleMappings = new Dictionary<string, Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>>();
      string roleMappingsQuery = WorkflowBpmDbAccessor.GetUserEligibleRolesAndRoleMappingsQuery(contactIDs);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine(roleMappingsQuery);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count > 0)
      {
        foreach (DataRow row in (InternalDataCollectionBase) dataRowCollection)
        {
          string key = SQL.DecodeString(row["userid"]);
          Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>> tuple;
          if (!rolesAndRoleMappings.TryGetValue(key, out tuple))
          {
            List<RoleSummaryInfo> roleSummaryInfoList = new List<RoleSummaryInfo>();
            List<RoleMappingsDetails> roleMappingsDetailsList = new List<RoleMappingsDetails>();
            roleSummaryInfoList.Add(WorkflowBpmDbAccessor.DataRowToRoleSummaryInfo(row));
            if (!Convert.IsDBNull(row["realWorldRoleID"]))
              roleMappingsDetailsList.Add(WorkflowBpmDbAccessor.DataRowToRoleMappingDetails(row));
            rolesAndRoleMappings.Add(key, new Tuple<List<RoleSummaryInfo>, List<RoleMappingsDetails>>(roleSummaryInfoList, roleMappingsDetailsList));
          }
          else
          {
            tuple.Item1.Add(WorkflowBpmDbAccessor.DataRowToRoleSummaryInfo(row));
            if (!Convert.IsDBNull(row["realWorldRoleID"]))
              tuple.Item2.Add(WorkflowBpmDbAccessor.DataRowToRoleMappingDetails(row));
          }
        }
      }
      return rolesAndRoleMappings;
    }

    [PgReady]
    private static RolesMappingInfo[] dataRowsToRolesMappingInfos(ICollection rows)
    {
      Hashtable hashtable = new Hashtable();
      foreach (DataRow row in (IEnumerable) rows)
      {
        RealWorldRoleID realWorldRoleId = (RealWorldRoleID) Enum.ToObject(typeof (RealWorldRoleID), (int) row["realWorldRoleID"]);
        RolesMappingInfo rolesMappingInfo = (RolesMappingInfo) hashtable[(object) realWorldRoleId];
        if (rolesMappingInfo == null)
        {
          rolesMappingInfo = new RolesMappingInfo(realWorldRoleId);
          hashtable[(object) realWorldRoleId] = (object) rolesMappingInfo;
        }
        rolesMappingInfo.AddRoleID((int) row["roleID"]);
      }
      RolesMappingInfo[] rolesMappingInfos = new RolesMappingInfo[hashtable.Count];
      if (hashtable.Count > 0)
        hashtable.Values.CopyTo((Array) rolesMappingInfos, 0);
      return rolesMappingInfos;
    }

    private static RoleMappingsDetails[] dataRowsToStandardRoleMapping(ICollection rows)
    {
      RoleMappingsDetails[] standardRoleMapping = new RoleMappingsDetails[rows.Count];
      int index = 0;
      foreach (DataRow row in (IEnumerable) rows)
      {
        RealWorldRoleID realWorldRoleId = (RealWorldRoleID) Enum.ToObject(typeof (RealWorldRoleID), (int) row["realWorldRoleID"]);
        RoleMappingsDetails roleMappingsDetails = new RoleMappingsDetails()
        {
          RealWorldRoleId = row["realWorldRoleID"].ToString(),
          RealWorldRoleName = realWorldRoleId.ToString(),
          UserDefinedRoleId = row["roleID"].ToString(),
          UserDefinedRoleName = row["roleName"].ToString()
        };
        standardRoleMapping[index] = roleMappingsDetails;
        ++index;
      }
      return standardRoleMapping;
    }

    private static void raiseCacheControlEvent()
    {
      ClientContext.GetCurrent().Sessions.BroadcastMessage((Message) new CacheControlMessage(ClientSessionCacheID.Workflow), false);
    }

    public static void UpdateMilestoneCache()
    {
      ClientContext current = ClientContext.GetCurrent();
      if ((EllieMae.EMLite.Workflow.Milestone[]) current.Cache.Get("WorkflowMilestones") != null)
      {
        using (current.Cache.Lock("WorkflowMilestones", EllieMae.EMLite.ClientServer.LockType.ReadOnly))
          current.Cache.Remove("WorkflowMilestones");
      }
      WorkflowBpmDbAccessor.GetMilestones(false);
    }

    public static List<EllieMae.EMLite.Workflow.Milestone> GetMilestones(bool activeOnly)
    {
      List<EllieMae.EMLite.Workflow.Milestone> list = ((IEnumerable<EllieMae.EMLite.Workflow.Milestone>) ClientContext.GetCurrent().Cache.Get<EllieMae.EMLite.Workflow.Milestone[]>("WorkflowMilestones", (Func<EllieMae.EMLite.Workflow.Milestone[]>) (() => WorkflowBpmDbAccessor.GetMilestonesFromDB(true)), CacheSetting.Low)).ToList<EllieMae.EMLite.Workflow.Milestone>();
      return activeOnly ? WorkflowBpmDbAccessor.getActiveMilestones(list) : list;
    }

    [PgReady]
    private static EllieMae.EMLite.Workflow.Milestone[] GetMilestonesFromReadReplica()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from Milestones where Archived = 0 order by SortIndex");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          milestoneList.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.AppendLine("select * from Milestones where Archived = 1 order by SortIndex DESC");
        foreach (DataRow r in (InternalDataCollectionBase) pgDbQueryBuilder.Execute())
          milestoneList.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
        return milestoneList.ToArray();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      dbQueryBuilder.AppendLine("select * from Milestones where Archived = 0 order by SortIndex");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList1 = new List<EllieMae.EMLite.Workflow.Milestone>();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection1)
        milestoneList1.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("select * from Milestones where Archived = 1 order by SortIndex DESC");
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        milestoneList1.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
      return milestoneList1.ToArray();
    }

    [PgReady]
    private static EllieMae.EMLite.Workflow.Milestone[] GetMilestonesFromDB(
      bool requireAdditionalDetails = false)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select * from Milestones where Archived = 0 order by SortIndex");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        List<EllieMae.EMLite.Workflow.Milestone> milestoneList = new List<EllieMae.EMLite.Workflow.Milestone>();
        foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection)
          milestoneList.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
        pgDbQueryBuilder.Reset();
        pgDbQueryBuilder.AppendLine("select * from Milestones where Archived = 1 order by SortIndex DESC");
        foreach (DataRow r in (InternalDataCollectionBase) pgDbQueryBuilder.Execute())
          milestoneList.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r));
        return milestoneList.ToArray();
      }
      EllieMae.EMLite.Server.DbQueryBuilder milestoneSqlQuery = WorkflowBpmDbAccessor.CreateMilestoneSqlQuery(new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login), new int?(0), requireAdditionalDetails: requireAdditionalDetails);
      DataRowCollection dataRowCollection1 = milestoneSqlQuery.Execute();
      List<EllieMae.EMLite.Workflow.Milestone> milestoneList1 = new List<EllieMae.EMLite.Workflow.Milestone>();
      foreach (DataRow r in (InternalDataCollectionBase) dataRowCollection1)
        milestoneList1.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r, requireAdditionalDetails));
      milestoneSqlQuery.Reset();
      foreach (DataRow r in (InternalDataCollectionBase) WorkflowBpmDbAccessor.CreateMilestoneSqlQuery(milestoneSqlQuery, new int?(1), requireAdditionalDetails: requireAdditionalDetails).Execute())
        milestoneList1.Add(WorkflowBpmDbAccessor.dataRowToMilestone(r, requireAdditionalDetails));
      return milestoneList1.ToArray();
    }

    private static List<EllieMae.EMLite.Workflow.Milestone> getActiveMilestones(
      List<EllieMae.EMLite.Workflow.Milestone> msList)
    {
      List<EllieMae.EMLite.Workflow.Milestone> msActive = new List<EllieMae.EMLite.Workflow.Milestone>();
      msList.ForEach((Action<EllieMae.EMLite.Workflow.Milestone>) (item =>
      {
        if (item.Archived)
          return;
        msActive.Add(item);
      }));
      return msActive;
    }

    public static Hashtable GetCompleteMilestoneNameToGUID()
    {
      List<EllieMae.EMLite.Workflow.Milestone> list = WorkflowBpmDbAccessor.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      Hashtable milestoneNameToGuid = new Hashtable();
      for (int index = 0; index < list.Count; ++index)
      {
        if (!milestoneNameToGuid.ContainsKey((object) list[index].Name))
          milestoneNameToGuid.Add((object) list[index].Name, (object) list[index].MilestoneID);
      }
      return milestoneNameToGuid;
    }

    public static Hashtable GetCompleteMilestoneGUIDToName()
    {
      List<EllieMae.EMLite.Workflow.Milestone> list = WorkflowBpmDbAccessor.GetMilestones(false).ToList<EllieMae.EMLite.Workflow.Milestone>();
      Hashtable milestoneGuidToName = new Hashtable();
      for (int index = 0; index < list.Count; ++index)
      {
        if (!milestoneGuidToName.ContainsKey((object) list[index].MilestoneID))
          milestoneGuidToName.Add((object) list[index].MilestoneID, (object) list[index].Name);
      }
      return milestoneGuidToName;
    }

    public static EllieMae.EMLite.Workflow.Milestone GetMilestone(
      string milestoneId,
      bool requireAdditionalDetails = false)
    {
      DataRowCollection dataRowCollection = WorkflowBpmDbAccessor.CreateMilestoneSqlQuery(new EllieMae.EMLite.Server.DbQueryBuilder(), milestoneId: milestoneId, requireAdditionalDetails: requireAdditionalDetails).Execute();
      return dataRowCollection.Count == 0 ? (EllieMae.EMLite.Workflow.Milestone) null : WorkflowBpmDbAccessor.dataRowToMilestone(dataRowCollection[0], requireAdditionalDetails);
    }

    private static EllieMae.EMLite.Server.DbQueryBuilder CreateMilestoneSqlQuery(
      EllieMae.EMLite.Server.DbQueryBuilder sql,
      int? archived = null,
      string milestoneId = null,
      bool requireAdditionalDetails = false)
    {
      if (requireAdditionalDetails)
      {
        sql.AppendLine("select m.*, r.roleName, f.Name formName from Milestones m");
        sql.AppendLine("left join Roles r on m.RoleID = r.roleID");
        sql.AppendLine("left Join InputForms f on m.SummaryFormID = f.FormID");
      }
      else
        sql.AppendLine("select * from Milestones m");
      int? nullable = archived;
      int num1 = 0;
      if (nullable.GetValueOrDefault() == num1 & nullable.HasValue)
      {
        sql.AppendLine("where m.Archived = 0 order by SortIndex");
      }
      else
      {
        nullable = archived;
        int num2 = 1;
        if (nullable.GetValueOrDefault() == num2 & nullable.HasValue)
          sql.AppendLine("where m.Archived = 1 order by SortIndex DESC");
        else if (!string.IsNullOrEmpty(milestoneId))
          sql.AppendLine("where m.MilestoneID = " + SQL.Encode((object) milestoneId) ?? "");
      }
      return sql;
    }

    public static EllieMae.EMLite.Workflow.Milestone GetMilestoneByName(string milestoneName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Milestones");
      DbValue key = new DbValue("Name", (object) milestoneName);
      dbQueryBuilder.SelectFrom(table, key);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (EllieMae.EMLite.Workflow.Milestone) null : WorkflowBpmDbAccessor.dataRowToMilestone(dataRowCollection[0]);
    }

    public static void CreateMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select MAX(SortIndex)+1 from Milestones where SortIndex < (Select SortIndex from Milestones where Name = 'Completion')");
        int int32 = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Milestones");
        DbValueList dbValueList = WorkflowBpmDbAccessor.createDbValueList(ms);
        dbValueList.Add("MilestoneID", (object) ms.MilestoneID);
        dbValueList.Add("SortIndex", (object) int32);
        dbValueList.Add("Archived", (object) ms.Archived, (IDbEncoder) DbEncoding.Flag);
        dbQueryBuilder.InsertInto(table, dbValueList, true, false);
        dbQueryBuilder.ExecuteNonQuery();
        WorkflowBpmDbAccessor.resetSortIndex();
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    public static void UpdateMilestone(EllieMae.EMLite.Workflow.Milestone ms)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        foreach (MilestoneTemplate milestoneTemplate in WorkflowBpmDbAccessor.GetMilestoneTemplates(false))
        {
          if (milestoneTemplate.Contains(ms.MilestoneID))
            dbQueryBuilder.AppendLine("DELETE FROM MilestoneTemplateFreeRoles where RoleID = '" + (object) ms.RoleID + "' and TemplateID = '" + milestoneTemplate.TemplateID + "'");
        }
        if (dbQueryBuilder.ToString() != "")
        {
          dbQueryBuilder.ExecuteNonQuery();
          dbQueryBuilder.Reset();
        }
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Milestones");
        DbValueList dbValueList = WorkflowBpmDbAccessor.createDbValueList(ms);
        DbValue key = new DbValue("MilestoneID", (object) ms.MilestoneID);
        dbQueryBuilder.Update(table, dbValueList, key);
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove("WorkflowMilestones");
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    private static void resetSortIndex()
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, int> source = new Dictionary<string, int>();
      sql.AppendLine("SELECT MilestoneID, SortIndex FROM Milestones WHERE Archived = 0 AND name <> 'Completion' ORDER BY sortIndex ASC");
      foreach (DataRow dataRow in (InternalDataCollectionBase) sql.Execute())
        source.Add(string.Concat(dataRow["MilestoneID"]), SQL.DecodeInt(dataRow["SortIndex"]));
      sql.Reset();
      int counter = 1;
      source.ToList<KeyValuePair<string, int>>().ForEach((Action<KeyValuePair<string, int>>) (item =>
      {
        sql.AppendLine("UPDATE Milestones SET SortIndex = " + (object) counter + "WHERE MilestoneID = '" + item.Key + "'");
        ++counter;
      }));
      sql.AppendLine("UPDATE Milestones SET SortIndex = 99999999 WHERE Name = 'Completion'");
      sql.ExecuteNonQuery();
    }

    private static void resetArchivedSortIndex()
    {
      EllieMae.EMLite.Server.DbQueryBuilder sql = new EllieMae.EMLite.Server.DbQueryBuilder();
      Dictionary<string, int> source = new Dictionary<string, int>();
      sql.AppendLine("SELECT MilestoneID, SortIndex FROM Milestones WHERE Archived = 1 ORDER BY sortIndex DESC");
      foreach (DataRow dataRow in (InternalDataCollectionBase) sql.Execute())
        source.Add(string.Concat(dataRow["MilestoneID"]), SQL.DecodeInt(dataRow["SortIndex"]));
      sql.Reset();
      int counter = -1;
      source.ToList<KeyValuePair<string, int>>().ForEach((Action<KeyValuePair<string, int>>) (item =>
      {
        sql.AppendLine("UPDATE Milestones SET SortIndex = " + (object) counter + "WHERE MilestoneID = '" + item.Key + "'");
        --counter;
      }));
      if (!(sql.ToString() != string.Empty))
        return;
      sql.ExecuteNonQuery();
    }

    private static void removeMilestoneFromTemplates(string milestoneID)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("DELETE FROM MilestoneTemplateMilestones WHERE MilestoneID = '" + milestoneID + "' and 1 = (select archived from milestones where milestoneID = '" + milestoneID + "')");
        dbQueryBuilder.ExecuteNonQuery();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("UPDATE MilestoneTemplates SET AutoLoanNumberingMilestoneID = null WHERE AutoLoanNumberingMilestoneID = '" + milestoneID + "'and 1 = (select archived from milestones where milestoneID = '" + milestoneID + "')");
        dbQueryBuilder.AppendLine("DELETE FROM eDisclosureMSTemplateSettings WHERE MilestoneID = '" + milestoneID + "'and 1 = (select archived from milestones where milestoneID = '" + milestoneID + "')");
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void SetMilestoneArchiveFlag(string milestoneId, bool archived)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        int num;
        if (archived)
        {
          dbQueryBuilder.AppendLine("SELECT MIN(SortIndex) - 1 FROM Milestones");
          int int32 = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
          num = int32 >= 0 ? -1 : int32;
          dbQueryBuilder.Reset();
        }
        else
        {
          dbQueryBuilder.AppendLine("SELECT MAX(SortIndex) + 1 FROM Milestones WHERE SortIndex < (Select SortIndex from Milestones where Name = 'Completion')");
          num = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
          dbQueryBuilder.Reset();
        }
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Milestones");
        DbValue key = new DbValue("MilestoneID", (object) milestoneId);
        dbQueryBuilder.Update(table, new DbValueList()
        {
          {
            "Archived",
            (object) archived,
            (IDbEncoder) DbEncoding.Flag
          },
          {
            "SortIndex",
            (object) num
          }
        }, key);
        dbQueryBuilder.ExecuteNonQuery();
        if (archived)
        {
          WorkflowBpmDbAccessor.resetSortIndex();
          WorkflowBpmDbAccessor.removeMilestoneFromTemplates(milestoneId);
        }
        else
          WorkflowBpmDbAccessor.resetArchivedSortIndex();
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    public static void DeleteMilestone(string milestoneId)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("Milestones");
        DbValue key = new DbValue("MilestoneID", (object) milestoneId);
        dbQueryBuilder.DeleteFrom(table, key);
        dbQueryBuilder.AppendLine("exec ReindexMilestones");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    public static void SetMilestoneOrder(string[] milestoneIds)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        for (int index = 0; index < milestoneIds.Length; ++index)
          dbQueryBuilder.AppendLine("update Milestones set SortIndex = " + (object) (index + 1) + " where MilestoneID = " + SQL.Encode((object) milestoneIds[index]));
        dbQueryBuilder.AppendLine("exec ReindexMilestones");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    public static void ChangeMilestoneSortIndex(string milestoneId, int sortIndex)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Declare("@priorIndex", "int");
        dbQueryBuilder.Declare("@newIndex", "int");
        dbQueryBuilder.AppendLine("select @priorIndex = SortIndex, @newIndex = " + (object) sortIndex + " from Milestones where MilestoneID = " + SQL.Encode((object) milestoneId));
        dbQueryBuilder.If("@newIndex > @priorIndex");
        dbQueryBuilder.AppendLine("update Milestones set SortIndex = SortIndex - 1 where SortIndex <= @newIndex");
        dbQueryBuilder.If("@newIndex < @priorIndex");
        dbQueryBuilder.AppendLine("update Milestones set SortIndex = SortIndex + 1 where SortIndex >= @newIndex");
        dbQueryBuilder.AppendLine("update Milestones set SortIndex = @newIndex where MilestoneID = " + SQL.Encode((object) milestoneId));
        dbQueryBuilder.AppendLine("exec ReindexMilestones");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    public static void ChangeMilestoneSortIndex(EllieMae.EMLite.Workflow.Milestone OldMilestone, EllieMae.EMLite.Workflow.Milestone NewMilestone)
    {
      ClientContext current = ClientContext.GetCurrent();
      List<string> stringList = new List<string>();
      List<int> intList = new List<int>();
      using (current.Cache.Lock("WorkflowMilestones"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (OldMilestone.SortIndex > NewMilestone.SortIndex)
        {
          dbQueryBuilder.AppendLine("Select MilestoneID, SortIndex from Milestones where Archived = 0 AND SortIndex BETWEEN " + (object) NewMilestone.SortIndex + " AND " + (object) OldMilestone.SortIndex + " ORDER BY SortIndex");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            stringList.Add(string.Concat(dataRow["MilestoneID"]));
            intList.Add(SQL.DecodeInt(dataRow["SortIndex"]));
          }
          dbQueryBuilder.Reset();
          for (int index = 0; index < stringList.Count - 1; ++index)
            dbQueryBuilder.AppendLine("UPDATE Milestones SET SortIndex = " + (object) intList[index + 1] + " WHERE MilestoneID = '" + stringList[index] + "'");
          dbQueryBuilder.AppendLine("UPDATE Milestones SET SortIndex = " + (object) intList[0] + " WHERE MilestoneID = '" + stringList[stringList.Count - 1] + "'");
        }
        else if (NewMilestone.SortIndex > OldMilestone.SortIndex)
        {
          dbQueryBuilder.AppendLine("Select MilestoneID, SortIndex from Milestones where Archived = 0 AND SortIndex BETWEEN " + (object) OldMilestone.SortIndex + " AND " + (object) NewMilestone.SortIndex + " ORDER BY SortIndex");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            stringList.Add(string.Concat(dataRow["MilestoneID"]));
            intList.Add(SQL.DecodeInt(dataRow["SortIndex"]));
          }
          dbQueryBuilder.Reset();
          for (int index = 0; index < stringList.Count - 1; ++index)
            dbQueryBuilder.AppendLine("UPDATE Milestones SET SortIndex = " + (object) intList[index] + " WHERE MilestoneID = '" + stringList[index + 1] + "'");
          dbQueryBuilder.AppendLine("UPDATE Milestones SET SortIndex = " + (object) intList[intList.Count - 1] + " WHERE MilestoneID = '" + stringList[0] + "'");
        }
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestones");
      }
    }

    private static DbValueList createDbValueList(EllieMae.EMLite.Workflow.Milestone ms)
    {
      DbValueList dbValueList1 = new DbValueList();
      dbValueList1.Add("Name", (object) ms.Name);
      dbValueList1.Add("TPOConnectStatus", (object) ms.TPOConnectStatus);
      dbValueList1.Add("ConsumerStatus", (object) ms.ConsumerStatus);
      DbValueList dbValueList2 = dbValueList1;
      byte num = ms.DisplayColor.R;
      string str1 = num.ToString("X2");
      num = ms.DisplayColor.G;
      string str2 = num.ToString("X2");
      num = ms.DisplayColor.B;
      string str3 = num.ToString("X2");
      string str4 = str1 + str2 + str3;
      dbValueList2.Add("DisplayColor", (object) str4);
      dbValueList1.Add("RoleID", (object) ms.RoleID, (IDbEncoder) DbEncoding.MinusOneAsNull);
      dbValueList1.Add("SummaryFormID", (object) ms.SummaryFormID, (IDbEncoder) DbEncoding.EmptyStringAsNull);
      dbValueList1.Add("RoleMemberRequired", (object) ms.RoleRequired, (IDbEncoder) DbEncoding.Flag);
      dbValueList1.Add("DescTextBefore", ms.DescTextBefore == "" ? (object) (ms.Name + " Expected") : (object) ms.DescTextBefore);
      dbValueList1.Add("DescTextAfter", ms.DescTextAfter == "" ? (object) (ms.Name + " Finished") : (object) ms.DescTextAfter);
      dbValueList1.Add("DefaultDays", (object) ms.DefaultDays);
      return dbValueList1;
    }

    private static EllieMae.EMLite.Workflow.Milestone dataRowToMilestone(
      DataRow r,
      bool requireAdditionalDetails = false)
    {
      string str = string.Concat(r["DisplayColor"]);
      return new EllieMae.EMLite.Workflow.Milestone(string.Concat(r["MilestoneID"]), SQL.DecodeInt(r["SortIndex"]), SQL.DecodeInt(r["RoleID"], -1))
      {
        Name = string.Concat(r["Name"]),
        TPOConnectStatus = string.Concat(r["TPOConnectStatus"]),
        ConsumerStatus = string.Concat(r["ConsumerStatus"]),
        Archived = SQL.DecodeInt(r["Archived"]) != 0,
        DisplayColor = str.Equals("") ? Color.FromArgb(0) : Color.FromArgb((int) byte.MaxValue, Color.FromArgb(int.Parse(string.Concat(r["DisplayColor"]), NumberStyles.AllowHexSpecifier))),
        SummaryFormID = SQL.DecodeString(r["SummaryFormID"], (string) null),
        RoleRequired = SQL.DecodeBoolean(r["RoleMemberRequired"], false),
        DescTextBefore = SQL.DecodeString(r["DescTextBefore"], ""),
        DescTextAfter = SQL.DecodeString(r["DescTextAfter"], ""),
        DefaultDays = SQL.DecodeInt(r["DefaultDays"], 0),
        RoleName = requireAdditionalDetails ? SQL.DecodeString((object) string.Concat(r["RoleName"])) : "",
        FormName = requireAdditionalDetails ? SQL.DecodeString((object) string.Concat(r["FormName"])) : ""
      };
    }

    public static IEnumerable<MilestoneTemplate> GetMilestoneTemplates(bool activeOnly)
    {
      ClientContext current = ClientContext.GetCurrent();
      MilestoneTemplate[] milestoneTemplateArray = (MilestoneTemplate[]) current.Cache.Get("WorkflowMilestoneTemplates");
      if (milestoneTemplateArray != null)
        return activeOnly ? (IEnumerable<MilestoneTemplate>) WorkflowBpmDbAccessor.getActiveMilestoneTemplates(((IEnumerable<MilestoneTemplate>) milestoneTemplateArray).ToList<MilestoneTemplate>()) : (IEnumerable<MilestoneTemplate>) new List<MilestoneTemplate>((IEnumerable<MilestoneTemplate>) milestoneTemplateArray);
      using (current.Cache.Lock("WorkflowMilestoneTemplates", EllieMae.EMLite.ClientServer.LockType.ReadOnly))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select * from MilestoneTemplates");
        dbQueryBuilder.AppendLine("order by SortIndex DESC");
        dbQueryBuilder.AppendLine("select mtm.*, m.Name, m.Archived from MilestoneTemplateMilestones mtm");
        dbQueryBuilder.AppendLine(" inner join Milestones m on m.MilestoneID = mtm.MilestoneID  order by TemplateID, SortIndex");
        dbQueryBuilder.AppendLine("select * from MilestoneTemplateFreeRoles order by TemplateID");
        dbQueryBuilder.AppendLine("select * from eDisclosureMSTemplateSettings order by TemplateID");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataTable table4 = dataSet.Tables[3];
        List<MilestoneTemplate> templateList = new List<MilestoneTemplate>();
        foreach (DataRow row in (InternalDataCollectionBase) table1.Rows)
          templateList.Add(WorkflowBpmDbAccessor.dataRowToMilestoneTemplate(row, (IEnumerable) table2.Select("TemplateID = " + SQL.Encode(row["TemplateID"])), (IEnumerable) table3.Select("TemplateID = " + SQL.Encode(row["TemplateID"])), (IEnumerable) table4.Select("templateID = " + SQL.Encode(row["templateID"]))));
        MilestoneTemplate[] array = templateList.ToArray();
        current.Cache.Put("WorkflowMilestoneTemplates", (object) array, CacheSetting.Low);
        return activeOnly ? (IEnumerable<MilestoneTemplate>) WorkflowBpmDbAccessor.getActiveMilestoneTemplates(templateList) : (IEnumerable<MilestoneTemplate>) templateList;
      }
    }

    private static List<MilestoneTemplate> getActiveMilestoneTemplates(
      List<MilestoneTemplate> templateList)
    {
      List<MilestoneTemplate> activeOnly = new List<MilestoneTemplate>();
      templateList.ForEach((Action<MilestoneTemplate>) (item =>
      {
        if (!item.Active)
          return;
        activeOnly.Add(item);
      }));
      return activeOnly;
    }

    [PgReady]
    public static List<FieldRuleInfo> GetMilestoneTemplate()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        List<FieldRuleInfo> milestoneTemplate = new List<FieldRuleInfo>();
        pgDbQueryBuilder.AppendLine("SELECT TemplateID, Name, condition, conditionState, conditionState2, ChannelCondition, advancedCode, advancedCodeXml from MilestoneTemplates");
        foreach (DataRow r in (InternalDataCollectionBase) pgDbQueryBuilder.Execute(DbTransactionType.None))
          milestoneTemplate.Add(WorkflowBpmDbAccessor.dataRowToFieldRuleInfo(r));
        return milestoneTemplate;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder(DBReadReplicaFeature.Login);
      List<FieldRuleInfo> milestoneTemplate1 = new List<FieldRuleInfo>();
      dbQueryBuilder.AppendLine("SELECT TemplateID, Name, condition, conditionState,conditionState2,ChannelCondition,advancedCode, advancedCodeXml from MilestoneTemplates");
      foreach (DataRow r in (InternalDataCollectionBase) dbQueryBuilder.Execute(DbTransactionType.None))
        milestoneTemplate1.Add(WorkflowBpmDbAccessor.dataRowToFieldRuleInfo(r));
      return milestoneTemplate1;
    }

    public static MilestoneTemplate GetMilestoneTemplate(string templateId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
      DbValue key = new DbValue("TemplateID", (object) templateId);
      dbQueryBuilder.SelectFrom(table1, key);
      dbQueryBuilder.AppendLine("select mtm.*, m.Name, m.Archived from MilestoneTemplateMilestones mtm");
      dbQueryBuilder.AppendLine(" inner join Milestones m on m.MilestoneID = mtm.MilestoneID where mtm.TemplateID = " + SQL.Encode((object) templateId) + " order by SortIndex");
      dbQueryBuilder.AppendLine("select * from MilestoneTemplateFreeRoles where TemplateID = " + SQL.Encode((object) templateId));
      dbQueryBuilder.AppendLine("select * from eDisclosureMSTemplateSettings where TemplateID = " + SQL.Encode((object) templateId));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table2 = dataSet.Tables[0];
      DataTable table3 = dataSet.Tables[1];
      DataTable table4 = dataSet.Tables[2];
      DataTable table5 = dataSet.Tables[3];
      return table2.Rows.Count == 0 ? (MilestoneTemplate) null : WorkflowBpmDbAccessor.dataRowToMilestoneTemplate(table2.Rows[0], (IEnumerable) table3.Rows, (IEnumerable) table4.Rows, (IEnumerable) table5.Rows);
    }

    public static MilestoneTemplate GetMilestoneTemplateByGuid(string templateGuid)
    {
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("select TemplateID from Milestonetemplates where TemplateGuid = " + SQL.Encode((object) templateGuid));
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        if (dataRowCollection != null)
        {
          if (dataRowCollection.Count > 0)
            return WorkflowBpmDbAccessor.GetMilestoneTemplate(string.Concat(dataRowCollection[0][0]));
        }
      }
      catch
      {
      }
      return (MilestoneTemplate) null;
    }

    public static string GetMilestoneTemplatebyMilestoneID(string milestoneId)
    {
      string templatebyMilestoneId = "";
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select Name from Milestonetemplates where TemplateID IN(Select TemplateID from MilestoneTemplateMilestones where MilestoneID = " + SQL.Encode((object) milestoneId) + ")");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute(DbTransactionType.None))
        templatebyMilestoneId = templatebyMilestoneId + dataRow["Name"] + ",";
      return templatebyMilestoneId;
    }

    public static FieldRuleInfo GetMilestoneTemplateConditions(string templateId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT TemplateID, Name, condition, conditionState,conditionState2,ChannelCondition,advancedCode, advancedCodeXml from MilestoneTemplates where TemplateID = " + templateId);
      return WorkflowBpmDbAccessor.dataRowToFieldRuleInfo(dbQueryBuilder.Execute(DbTransactionType.None)[0]);
    }

    public static MilestoneTemplate GetMilestoneTemplateByName(string templateName)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
      DbValue key = new DbValue("Name", (object) templateName);
      dbQueryBuilder.SelectFrom(table1, key);
      dbQueryBuilder.AppendLine("select mtm.*, m.Name, m.Archived from MilestoneTemplateMilestones mtm");
      dbQueryBuilder.AppendLine(" inner join MilestoneTemplates mt on mt.TemplateID = mtm.TemplateID");
      dbQueryBuilder.AppendLine(" inner join Milestones m ON m.MilestoneID = mtm.MilestoneID");
      dbQueryBuilder.AppendLine("where mt.Name = " + SQL.Encode((object) templateName) + " order by mtm.SortIndex");
      dbQueryBuilder.AppendLine("select mtfr.* from MilestoneTemplateFreeRoles mtfr");
      dbQueryBuilder.AppendLine("  inner join MilestoneTemplates mt on mt.TemplateID = mtfr.TemplateID");
      dbQueryBuilder.AppendLine("where mt.Name = " + SQL.Encode((object) templateName));
      dbQueryBuilder.AppendLine("select edis.* from eDisclosureMSTemplateSettings edis");
      dbQueryBuilder.AppendLine("  inner join MilestoneTemplates mt on mt.TemplateID = edis.TemplateID");
      dbQueryBuilder.AppendLine("where mt.Name = " + SQL.Encode((object) templateName));
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table2 = dataSet.Tables[0];
      DataTable table3 = dataSet.Tables[1];
      DataTable table4 = dataSet.Tables[2];
      DataTable table5 = dataSet.Tables[3];
      return table2.Rows.Count == 0 ? (MilestoneTemplate) null : WorkflowBpmDbAccessor.dataRowToMilestoneTemplate(table2.Rows[0], (IEnumerable) table3.Select("TemplateID = " + SQL.Encode(table2.Rows[0]["TemplateID"])), (IEnumerable) table4.Select("TemplateID = " + SQL.Encode(table2.Rows[0]["TemplateID"])), (IEnumerable) table5.Select("TemplateID = " + SQL.Encode(table2.Rows[0]["TemplateID"])));
    }

    [PgReady]
    public static MilestoneTemplate GetDefaultMilestoneTemplate()
    {
      MethodExecutionArgs args = new MethodExecutionArgs((object) null, Arguments.Empty);
      args.DeclarationIdentifier = new DeclarationIdentifier(-7808866884781735852L);
      // ISSUE: reference to a compiler-generated field
      args.Method = \u003C\u003Ez__a_1._2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: explicit non-virtual call
      __nonvirtual (\u003C\u003Ez__a_1.a0.OnEntry(args));
      if (args.FlowBehavior == FlowBehavior.Return)
        return (MilestoneTemplate) args.ReturnValue;
      MilestoneTemplate milestoneTemplate;
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("SELECT * from MilestoneTemplates where Name = 'Default Template';");
        pgDbQueryBuilder.AppendLine("SELECT mtm.*, m.Name, m.Archived FROM MilestoneTemplateMilestones mtm");
        pgDbQueryBuilder.AppendLine(" inner join MilestoneTemplates mt ON mt.TemplateID = mtm.TemplateID");
        pgDbQueryBuilder.AppendLine(" inner join Milestones m ON m.MilestoneID = mtm.MilestoneID");
        pgDbQueryBuilder.AppendLine("where mt.Name = 'Default Template' ORDER BY mtm.SortIndex;");
        pgDbQueryBuilder.AppendLine("SELECT mtfr.* FROM MilestoneTemplateFreeRoles mtfr");
        pgDbQueryBuilder.AppendLine("inner join MilestoneTemplates mt ON mt.TemplateID = mtfr.TemplateID");
        pgDbQueryBuilder.AppendLine("where mt.Name = 'Default Template';");
        pgDbQueryBuilder.AppendLine("select edis.* from eDisclosureMSTemplateSettings edis");
        pgDbQueryBuilder.AppendLine("  inner join MilestoneTemplates mt on mt.TemplateID = edis.TemplateID");
        pgDbQueryBuilder.AppendLine("where  mt.Name = 'Default Template';");
        DataSet dataSet = pgDbQueryBuilder.ExecuteSetQuery();
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataTable table4 = dataSet.Tables[3];
        milestoneTemplate = table1.Rows.Count != 0 ? WorkflowBpmDbAccessor.dataRowToMilestoneTemplate(table1.Rows[0], (IEnumerable) table2.Select("TemplateID = " + SQL.Encode(table1.Rows[0]["TemplateID"])), (IEnumerable) table3.Select("TemplateID = " + SQL.Encode(table1.Rows[0]["TemplateID"])), (IEnumerable) table4.Select("TemplateID = " + SQL.Encode(table1.Rows[0]["TemplateID"]))) : (MilestoneTemplate) null;
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT * from MilestoneTemplates where Name = 'Default Template'");
        dbQueryBuilder.AppendLine("SELECT mtm.*, m.Name, m.Archived FROM MilestoneTemplateMilestones mtm");
        dbQueryBuilder.AppendLine(" inner join MilestoneTemplates mt ON mt.TemplateID = mtm.TemplateID");
        dbQueryBuilder.AppendLine(" inner join Milestones m ON m.MilestoneID = mtm.MilestoneID");
        dbQueryBuilder.AppendLine("where mt.Name = 'Default Template' ORDER BY mtm.SortIndex");
        dbQueryBuilder.AppendLine("SELECT mtfr.* FROM MilestoneTemplateFreeRoles mtfr");
        dbQueryBuilder.AppendLine("inner join MilestoneTemplates mt ON mt.TemplateID = mtfr.TemplateID");
        dbQueryBuilder.AppendLine("where mt.Name = 'Default Template'");
        dbQueryBuilder.AppendLine("select edis.* from eDisclosureMSTemplateSettings edis");
        dbQueryBuilder.AppendLine("  inner join MilestoneTemplates mt on mt.TemplateID = edis.TemplateID");
        dbQueryBuilder.AppendLine("where  mt.Name = 'Default Template'");
        DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
        DataTable table5 = dataSet.Tables[0];
        DataTable table6 = dataSet.Tables[1];
        DataTable table7 = dataSet.Tables[2];
        DataTable table8 = dataSet.Tables[3];
        milestoneTemplate = table5.Rows.Count != 0 ? WorkflowBpmDbAccessor.dataRowToMilestoneTemplate(table5.Rows[0], (IEnumerable) table6.Select("TemplateID = " + SQL.Encode(table5.Rows[0]["TemplateID"])), (IEnumerable) table7.Select("TemplateID = " + SQL.Encode(table5.Rows[0]["TemplateID"])), (IEnumerable) table8.Select("TemplateID = " + SQL.Encode(table5.Rows[0]["TemplateID"]))) : (MilestoneTemplate) null;
      }
      args.ReturnValue = (object) milestoneTemplate;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: explicit non-virtual call
      __nonvirtual (\u003C\u003Ez__a_1.a0.OnSuccess(args));
      return (MilestoneTemplate) args.ReturnValue;
    }

    public static void CreateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT MAX(SortIndex) + 1 FROM MilestoneTemplates");
        int int32 = Convert.ToInt32(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        dbQueryBuilder.Declare("@TemplateID", "int");
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
        DbValueList dbValueList1 = WorkflowBpmDbAccessor.createDbValueList(template, rule);
        dbValueList1.Add("SortIndex", (object) int32);
        dbQueryBuilder.InsertInto(table1, dbValueList1, true, false);
        dbQueryBuilder.SelectIdentity("@TemplateID");
        dbQueryBuilder.Select("@TemplateID");
        int num = Utils.ParseInt(dbQueryBuilder.ExecuteScalar());
        dbQueryBuilder.Reset();
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplateMilestones");
        foreach (TemplateMilestone ms in template)
        {
          DbValueList dbValueList2 = WorkflowBpmDbAccessor.createDbValueList(ms);
          dbValueList2.Add("TemplateID", (object) num);
          dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
        }
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplateFreeRoles");
        foreach (TemplateFreeRole freeRole in template.FreeRoles)
        {
          DbValueList dbValueList3 = WorkflowBpmDbAccessor.createDbValueList(freeRole);
          dbValueList3.Add("TemplateID", (object) num);
          dbQueryBuilder.InsertInto(table3, dbValueList3, true, false);
        }
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void UpdateMilestoneTemplateImpactedAreaSettings(
      Dictionary<MilestoneTemplate, string> newSettings,
      string impactedArea)
    {
      ClientContext current = ClientContext.GetCurrent();
      string str = "";
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        if (impactedArea == "LoanNumbering")
          str = "AutoLoanNumberingMilestoneID";
        if (str != "")
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("Update MilestoneTemplates set " + str + " = NULL");
          foreach (MilestoneTemplate key in newSettings.Keys)
            dbQueryBuilder.AppendLine("Update MilestoneTemplates set " + str + " = '" + newSettings[key] + "' where TemplateID=" + key.TemplateID);
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
      current.Cache.Remove("WorkflowMilestoneTemplates");
    }

    public static Dictionary<string, List<string>> GetAllMilestoneTemplateEDisclosureExceptions()
    {
      Dictionary<string, List<string>> edisclosureExceptions = new Dictionary<string, List<string>>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("Select * from eDisclosureMSTemplateSettings");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
      {
        if (edisclosureExceptions.ContainsKey(dataRow["Channel"].ToString() + "_" + dataRow["Category"] + "_" + dataRow["eDisclosureElementAttributeID"].ToString()))
          edisclosureExceptions[dataRow["Channel"].ToString() + "_" + dataRow["Category"] + "_" + dataRow["eDisclosureElementAttributeID"].ToString()].Add(SQL.DecodeString(dataRow["TemplateID"]));
        else
          edisclosureExceptions.Add(dataRow["Channel"].ToString() + "_" + dataRow["Category"] + "_" + dataRow["eDisclosureElementAttributeID"].ToString(), new List<string>()
          {
            SQL.DecodeString(dataRow["TemplateID"])
          });
      }
      return edisclosureExceptions;
    }

    public static void UpdateMilestoneTemplateEDisclosureExceptions(
      EDisclosureSetup eDisclosureSetup)
    {
      Dictionary<string, Dictionary<MilestoneTemplate, string>> exceptionsList = new Dictionary<string, Dictionary<MilestoneTemplate, string>>();
      DataTable elementAttributes = EDisclosureConfigurationAccessor.GetAllEdisclosureElementAttributes();
      if (elementAttributes == null)
        return;
      DataRow[] array = elementAttributes.Rows.Cast<DataRow>().ToArray<DataRow>();
      if (eDisclosureSetup.RetailChannel.ConditionalApplication.UpdatedExceptionsList != null)
        exceptionsList.Add("RetailChannel_AtApplication_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 1, 1), eDisclosureSetup.RetailChannel.ConditionalApplication.UpdatedExceptionsList);
      if (eDisclosureSetup.RetailChannel.ConditionalThreeDay.UpdatedExceptionsList != null)
        exceptionsList.Add("RetailChannel_ThreeDay_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 1, 2), eDisclosureSetup.RetailChannel.ConditionalThreeDay.UpdatedExceptionsList);
      if (eDisclosureSetup.RetailChannel.ConditionalLock.UpdatedExceptionsList != null)
        exceptionsList.Add("RetailChannel_AtLock_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 1, 3), eDisclosureSetup.RetailChannel.ConditionalLock.UpdatedExceptionsList);
      if (eDisclosureSetup.RetailChannel.ConditionalApproval.UpdatedExceptionsList != null)
        exceptionsList.Add("RetailChannel_Approval_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 1, 4), eDisclosureSetup.RetailChannel.ConditionalApproval.UpdatedExceptionsList);
      if (eDisclosureSetup.WholesaleChannel.ConditionalApplication.UpdatedExceptionsList != null)
        exceptionsList.Add("WholesaleChannel_AtApplication_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 2, 1), eDisclosureSetup.WholesaleChannel.ConditionalApplication.UpdatedExceptionsList);
      if (eDisclosureSetup.WholesaleChannel.ConditionalThreeDay.UpdatedExceptionsList != null)
        exceptionsList.Add("WholesaleChannel_ThreeDay_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 2, 2), eDisclosureSetup.WholesaleChannel.ConditionalThreeDay.UpdatedExceptionsList);
      if (eDisclosureSetup.WholesaleChannel.ConditionalLock.UpdatedExceptionsList != null)
        exceptionsList.Add("WholesaleChannel_AtLock_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 2, 3), eDisclosureSetup.WholesaleChannel.ConditionalLock.UpdatedExceptionsList);
      if (eDisclosureSetup.WholesaleChannel.ConditionalApproval.UpdatedExceptionsList != null)
        exceptionsList.Add("WholesaleChannel_Approval_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 2, 4), eDisclosureSetup.WholesaleChannel.ConditionalApproval.UpdatedExceptionsList);
      if (eDisclosureSetup.BrokerChannel.ConditionalApplication.UpdatedExceptionsList != null)
        exceptionsList.Add("BrokerChannel_AtApplication_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 3, 1), eDisclosureSetup.BrokerChannel.ConditionalApplication.UpdatedExceptionsList);
      if (eDisclosureSetup.BrokerChannel.ConditionalThreeDay.UpdatedExceptionsList != null)
        exceptionsList.Add("BrokerChannel_ThreeDay_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 3, 2), eDisclosureSetup.BrokerChannel.ConditionalThreeDay.UpdatedExceptionsList);
      if (eDisclosureSetup.BrokerChannel.ConditionalLock.UpdatedExceptionsList != null)
        exceptionsList.Add("BrokerChannel_AtLock_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 3, 3), eDisclosureSetup.BrokerChannel.ConditionalLock.UpdatedExceptionsList);
      if (eDisclosureSetup.BrokerChannel.ConditionalApproval.UpdatedExceptionsList != null)
        exceptionsList.Add("BrokerChannel_Approval_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 3, 4), eDisclosureSetup.BrokerChannel.ConditionalApproval.UpdatedExceptionsList);
      if (eDisclosureSetup.CorrespondentChannel.ConditionalApplication.UpdatedExceptionsList != null)
        exceptionsList.Add("CorrespondentChannel_AtApplication_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 4, 1), eDisclosureSetup.CorrespondentChannel.ConditionalApplication.UpdatedExceptionsList);
      if (eDisclosureSetup.CorrespondentChannel.ConditionalThreeDay.UpdatedExceptionsList != null)
        exceptionsList.Add("CorrespondentChannel_ThreeDay_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 4, 2), eDisclosureSetup.CorrespondentChannel.ConditionalThreeDay.UpdatedExceptionsList);
      if (eDisclosureSetup.CorrespondentChannel.ConditionalLock.UpdatedExceptionsList != null)
        exceptionsList.Add("CorrespondentChannel_AtLock_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 4, 3), eDisclosureSetup.CorrespondentChannel.ConditionalLock.UpdatedExceptionsList);
      if (eDisclosureSetup.CorrespondentChannel.ConditionalApproval.UpdatedExceptionsList != null)
        exceptionsList.Add("CorrespondentChannel_Approval_" + WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, 4, 4), eDisclosureSetup.CorrespondentChannel.ConditionalApproval.UpdatedExceptionsList);
      WorkflowBpmDbAccessor.UpdateMilestoneTemplateEDisclosureExceptions(exceptionsList, true);
    }

    private static string GetEdisclosureElementAttributeID(
      DataRow[] rows,
      int channelID,
      int channelTypeID)
    {
      return Enumerable.Cast<DataRow>(rows).Where<DataRow>((System.Func<DataRow, bool>) (row => row["ChannelID"].Equals((object) channelID) && row["PackageTypeID"].ToString().Equals(channelTypeID.ToString()))).Select<DataRow, int>((System.Func<DataRow, int>) (row => int.Parse(row["eDisclosureElementAttributeID"].ToString()))).FirstOrDefault<int>().ToString();
    }

    public static void UpdateMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, Dictionary<MilestoneTemplate, string>> exceptionsList,
      bool remove)
    {
      Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder1 = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder1.AppendLine("Select * from eDisclosureMSTemplateSettings");
      foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder1.Execute())
      {
        if (exceptionsList.ContainsKey(SQL.DecodeString(dataRow["Channel"]) + "_" + SQL.DecodeString(dataRow["Category"]) + "_" + SQL.DecodeString(dataRow["eDisclosureElementAttributeID"])))
          dictionary.Add(SQL.DecodeString(dataRow["ID"]), false);
      }
      DataRow[] rows = (DataRow[]) null;
      ClientContext current = ClientContext.GetCurrent();
      foreach (KeyValuePair<string, Dictionary<MilestoneTemplate, string>> exceptions in exceptionsList)
      {
        string[] strArray = exceptions.Key.Split('_');
        string str1 = strArray[0];
        string str2 = strArray[1];
        string s = string.Empty;
        if (strArray.Length > 2)
        {
          s = strArray[2];
        }
        else
        {
          if (rows == null)
          {
            DataTable elementAttributes = EDisclosureConfigurationAccessor.GetAllEdisclosureElementAttributes();
            if (elementAttributes == null)
              return;
            rows = elementAttributes.Rows.Cast<DataRow>().ToArray<DataRow>();
          }
          if (exceptions.Key.Equals("RetailChannel_AtApplication", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 1, 1);
          if (exceptions.Key.Equals("RetailChannel_ThreeDay", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 1, 2);
          if (exceptions.Key.Equals("RetailChannel_AtLock", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 1, 3);
          if (exceptions.Key.Equals("RetailChannel_Approval", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 1, 4);
          if (exceptions.Key.Equals("WholesaleChannel_AtApplication", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 2, 1);
          if (exceptions.Key.Equals("WholesaleChannel_ThreeDay", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 2, 2);
          if (exceptions.Key.Equals("WholesaleChannel_AtLock", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 2, 3);
          if (exceptions.Key.Equals("WholesaleChannel_Approval", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 2, 4);
          if (exceptions.Key.Equals("BrokerChannel_AtApplication", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 3, 1);
          if (exceptions.Key.Equals("BrokerChannel_ThreeDay", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 3, 2);
          if (exceptions.Key.Equals("BrokerChannel_AtLock", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 3, 3);
          if (exceptions.Key.Equals("BrokerChannel_Approval", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 3, 4);
          if (exceptions.Key.Equals("CorrespondentChannel_AtApplication", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 4, 1);
          if (exceptions.Key.Equals("CorrespondentChannel_ThreeDay", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 4, 2);
          if (exceptions.Key.Equals("CorrespondentChannel_AtLock", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 4, 3);
          if (exceptions.Key.Equals("CorrespondentChannel_Approval", StringComparison.OrdinalIgnoreCase))
            s = WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(rows, 4, 4);
        }
        int num = int.Parse(s);
        foreach (KeyValuePair<MilestoneTemplate, string> keyValuePair in exceptions.Value)
        {
          if (keyValuePair.Value != null)
          {
            EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder2 = new EllieMae.EMLite.Server.DbQueryBuilder();
            dbQueryBuilder2.AppendLine("Select * from eDisclosureMSTemplateSettings where TemplateID = '" + keyValuePair.Key.TemplateID + "' and Channel = '" + str1 + "' and Category = '" + str2 + "' and eDisclosureElementAttributeID = '" + (object) num + "'");
            DataRowCollection dataRowCollection = dbQueryBuilder2.Execute();
            DataRow dataRow = dataRowCollection == null || dataRowCollection.Count == 0 ? (DataRow) null : dataRowCollection[0];
            if (dataRow != null)
            {
              dictionary[SQL.DecodeString(dataRow["ID"])] = true;
              dbQueryBuilder2.Reset();
              dbQueryBuilder2.AppendLine("update eDisclosureMSTemplateSettings set milestoneID = '" + keyValuePair.Value + "' where templateID = '" + keyValuePair.Key.TemplateID + "' and channel = '" + str1 + "' and category = '" + str2 + "' and eDisclosureElementAttributeID = '" + (object) num + "'");
              dbQueryBuilder2.ExecuteNonQuery();
            }
            else
            {
              dbQueryBuilder2.Reset();
              dbQueryBuilder2.AppendLine(string.Format("insert into eDisclosureMSTemplateSettings (TemplateID, eDisclosureElementAttributeID, MilestoneID, Channel, Category) values ({0},{1},{2},{3},{4})", (object) keyValuePair.Key.TemplateID, (object) num, (object) SQL.Encode((object) keyValuePair.Value), (object) SQL.Encode((object) str1), (object) SQL.Encode((object) str2)));
              dbQueryBuilder2.ExecuteNonQuery();
            }
          }
        }
      }
      if (remove)
      {
        foreach (KeyValuePair<string, bool> keyValuePair in dictionary)
        {
          if (!keyValuePair.Value)
          {
            EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder3 = new EllieMae.EMLite.Server.DbQueryBuilder();
            dbQueryBuilder3.AppendLine("delete from eDisclosureMSTemplateSettings where ID = '" + keyValuePair.Key + "'");
            dbQueryBuilder3.ExecuteNonQuery();
          }
        }
      }
      current.Cache.Remove("WorkflowMilestoneTemplates");
    }

    public static void RemoveMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, List<string>> exceptionsList)
    {
      ClientContext current = ClientContext.GetCurrent();
      foreach (KeyValuePair<string, List<string>> exceptions in exceptionsList)
      {
        foreach (string str in exceptions.Value)
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("delete from eDisclosureMSTemplateSettings where milestoneID = '" + str + "' and templateID = '" + exceptions.Key + "'");
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
      current.Cache.Remove("WorkflowMilestoneTemplates");
    }

    public static void UpdateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
        DbValueList dbValueList1 = WorkflowBpmDbAccessor.createDbValueList(template, rule);
        DbValue key = new DbValue("TemplateID", (object) template.TemplateID);
        dbQueryBuilder.Update(table1, dbValueList1, key);
        DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplateMilestones");
        dbQueryBuilder.DeleteFrom(table2, key);
        foreach (TemplateMilestone ms in template)
        {
          DbValueList dbValueList2 = WorkflowBpmDbAccessor.createDbValueList(ms);
          dbValueList2.Add("TemplateID", (object) template.TemplateID);
          dbQueryBuilder.InsertInto(table2, dbValueList2, true, false);
        }
        DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplateFreeRoles");
        dbQueryBuilder.DeleteFrom(table3, key);
        foreach (TemplateFreeRole freeRole in template.FreeRoles)
        {
          DbValueList dbValueList3 = WorkflowBpmDbAccessor.createDbValueList(freeRole);
          dbValueList3.Add("TemplateID", (object) template.TemplateID);
          dbQueryBuilder.InsertInto(table3, dbValueList3, true, false);
        }
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void SetMilestoneTemplateActiveFlag(string templateId, bool active)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
        DbValue key = new DbValue("TemplateID", (object) templateId);
        dbQueryBuilder.Update(table, new DbValueList()
        {
          {
            "Active",
            (object) active,
            (IDbEncoder) DbEncoding.Flag
          }
        }, key);
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void DeleteMilestoneTemplate(string templateId)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("MilestoneTemplates");
        DbValue key = new DbValue("TemplateID", (object) templateId);
        dbQueryBuilder.DeleteFrom(table, key);
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void SetMilestoneTemplateOrder(string[] templateIds)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        for (int index = 0; index < templateIds.Length; ++index)
          dbQueryBuilder.AppendLine("update MilestoneTemplates set SortIndex = " + (object) (index + 1) + " where TemplateID = " + SQL.Encode((object) templateIds[index]));
        dbQueryBuilder.AppendLine("exec ReindexMilestoneTemplates");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void ChangeMilestoneTemplateSortIndex(string templateId, int sortIndex)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Declare("@priorIndex", "int");
        dbQueryBuilder.Declare("@newIndex", "int");
        dbQueryBuilder.AppendLine("select @priorIndex = SortIndex, @newIndex = " + (object) sortIndex + " from MilestoneTemplates where TemplateID = " + SQL.Encode((object) templateId));
        dbQueryBuilder.If("@newIndex > @priorIndex");
        dbQueryBuilder.AppendLine("update MilestoneTemplates set SortIndex = SortIndex - 1 where SortIndex <= @newIndex");
        dbQueryBuilder.If("@newIndex < @priorIndex");
        dbQueryBuilder.AppendLine("update MilestoneTemplates set SortIndex = SortIndex + 1 where SortIndex >= @newIndex");
        dbQueryBuilder.AppendLine("update MilestoneTemplates set SortIndex = @newIndex where TemplateID = " + SQL.Encode((object) templateId));
        dbQueryBuilder.AppendLine("exec ReindexMilestoneTemplates");
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static void ChangeMilestoneTemplateSortIndex(
      MilestoneTemplate OldTemplate,
      MilestoneTemplate NewTemplate)
    {
      ClientContext current = ClientContext.GetCurrent();
      List<string> stringList = new List<string>();
      List<int> intList = new List<int>();
      using (current.Cache.Lock("WorkflowMilestoneTemplates"))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        if (OldTemplate.SortIndex > NewTemplate.SortIndex)
        {
          dbQueryBuilder.AppendLine("Select TemplateID, SortIndex from MilestoneTemplates where SortIndex BETWEEN " + (object) NewTemplate.SortIndex + " AND " + (object) OldTemplate.SortIndex + " ORDER BY SortIndex DESC");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            stringList.Add(string.Concat(dataRow["TemplateID"]));
            intList.Add(SQL.DecodeInt(dataRow["SortIndex"]));
          }
          dbQueryBuilder.Reset();
          for (int index = 0; index < stringList.Count - 1; ++index)
            dbQueryBuilder.AppendLine("UPDATE MilestoneTemplates SET SortIndex = " + (object) intList[index] + " WHERE TemplateID = '" + stringList[index + 1] + "'");
          dbQueryBuilder.AppendLine("UPDATE MilestoneTemplates SET SortIndex = " + (object) intList[stringList.Count - 1] + " WHERE TemplateID = '" + stringList[0] + "'");
        }
        else if (NewTemplate.SortIndex > OldTemplate.SortIndex)
        {
          dbQueryBuilder.AppendLine("Select TemplateID, SortIndex from MilestoneTemplates where SortIndex BETWEEN " + (object) OldTemplate.SortIndex + " AND " + (object) NewTemplate.SortIndex + " ORDER BY SortIndex DESC");
          foreach (DataRow dataRow in (InternalDataCollectionBase) dbQueryBuilder.Execute())
          {
            stringList.Add(string.Concat(dataRow["TemplateID"]));
            intList.Add(SQL.DecodeInt(dataRow["SortIndex"]));
          }
          dbQueryBuilder.Reset();
          for (int index = 0; index < stringList.Count - 1; ++index)
            dbQueryBuilder.AppendLine("UPDATE MilestoneTemplates SET SortIndex = " + (object) intList[index + 1] + " WHERE TemplateID = '" + stringList[index] + "'");
          dbQueryBuilder.AppendLine("UPDATE MilestoneTemplates SET SortIndex = " + (object) intList[0] + " WHERE TemplateID = '" + stringList[intList.Count - 1] + "'");
        }
        dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        current.Cache.Remove("WorkflowMilestoneTemplates");
      }
    }

    public static Hashtable GetMilestoneTemplateDefaultSettings()
    {
      ClientContext current = ClientContext.GetCurrent();
      Hashtable d1 = (Hashtable) current.Cache.Get("MilestoneTemplateDefaultSettings");
      if (d1 != null)
        return new Hashtable((IDictionary) d1);
      using (current.Cache.Lock("MilestoneTemplateDefaultSettings", EllieMae.EMLite.ClientServer.LockType.ReadOnly))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine("SELECT * from MilestoneTemplateDefaultSettings");
        DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.None);
        Hashtable d2 = new Hashtable();
        for (int index = 0; index < dataRowCollection.Count; ++index)
          d2[dataRowCollection[index]["SettingsName"]] = (object) (string) dataRowCollection[index]["MilestoneID"];
        Hashtable o = d2;
        current.Cache.Put("MilestoneTemplateDefaultSettings", (object) o, CacheSetting.Low);
        return new Hashtable((IDictionary) d2);
      }
    }

    private static DbValueList createDbValueList(MilestoneTemplate template, BizRuleInfo rule)
    {
      DbValueList dbValueList = new DbValueList();
      dbValueList.Add("Name", (object) template.Name);
      dbValueList.Add("Active", (object) (template.Active ? 1 : 0));
      dbValueList.Add("Comment", (object) template.Comment);
      dbValueList.Add("UpdateBorrowerContactMilestoneID", (object) template.UpdateBorrowerContactMilestoneID);
      dbValueList.Add("AutoLoanNumberingMilestoneID", (object) template.AutoLoanNumberingMilestoneID);
      if (rule != null)
      {
        dbValueList.Add("condition", (object) (int) rule.Condition);
        dbValueList.Add("ChannelCondition", (object) rule.Condition2);
        dbValueList.Add("conditionState", rule.Condition == BizRule.Condition.AdvancedCoding ? (object) "" : (object) rule.ConditionState);
        dbValueList.Add("conditionState2", (object) rule.ConditionState2);
        dbValueList.Add("advancedCode", rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.ConditionState : (object) (string) null);
        dbValueList.Add("advancedCodeXml", rule.Condition == BizRule.Condition.AdvancedCoding ? (object) rule.AdvancedCodeXML : (object) (string) null);
      }
      return dbValueList;
    }

    private static DbValueList createDbValueList(TemplateMilestone ms)
    {
      return new DbValueList()
      {
        {
          "MilestoneID",
          (object) ms.MilestoneID
        },
        {
          "SortIndex",
          (object) ms.SortIndex
        },
        {
          "DaysToComplete",
          (object) ms.DaysToComplete
        }
      };
    }

    private static DbValueList createDbValueList(TemplateFreeRole role)
    {
      return new DbValueList()
      {
        {
          "RoleID",
          (object) role.RoleID
        }
      };
    }

    [PgReady]
    private static MilestoneTemplate dataRowToMilestoneTemplate(
      DataRow r,
      IEnumerable milestoneRows,
      IEnumerable roleRows,
      IEnumerable eDisclosureRows)
    {
      if (eDisclosureRows.Cast<DataRow>().Where<DataRow>((System.Func<DataRow, bool>) (row => row["eDisclosureElementAttributeID"] == DBNull.Value)).Select<DataRow, object>((System.Func<DataRow, object>) (row => row["eDisclosureElementAttributeID"])).Count<object>() >= 1)
      {
        EDisclosureConfigurationAccessor.GetEDisclosurePackageSetup();
        DataTable elementAttributes = EDisclosureConfigurationAccessor.GetAllEdisclosureElementAttributes();
        if (elementAttributes != null)
        {
          DataRow[] array = elementAttributes.Rows.Cast<DataRow>().ToArray<DataRow>();
          int channelID = 0;
          int channelTypeID = 0;
          foreach (DataRow eDisclosureRow in eDisclosureRows)
          {
            if (eDisclosureRow["eDisclosureElementAttributeID"] == DBNull.Value)
            {
              switch (eDisclosureRow["Channel"].ToString())
              {
                case "RetailChannel":
                  channelID = 1;
                  break;
                case "WholesaleChannel":
                  channelID = 2;
                  break;
                case "BrokerChannel":
                  channelID = 3;
                  break;
                case "CorrespondentChannel":
                  channelID = 4;
                  break;
              }
              switch (eDisclosureRow["Category"].ToString())
              {
                case "AtApplication":
                  channelTypeID = 1;
                  break;
                case "ThreeDay":
                  channelTypeID = 2;
                  break;
                case "AtLock":
                  channelTypeID = 3;
                  break;
                case "Approval":
                  channelTypeID = 4;
                  break;
              }
              eDisclosureRow["eDisclosureElementAttributeID"] = (object) WorkflowBpmDbAccessor.GetEdisclosureElementAttributeID(array, channelID, channelTypeID);
            }
          }
        }
      }
      Dictionary<string, string> eDisclosureMilestoneSettings = new Dictionary<string, string>();
      foreach (DataRow eDisclosureRow in eDisclosureRows)
        eDisclosureMilestoneSettings.Add(string.Format("{0}_{1}_{2}", (object) eDisclosureRow["Channel"].ToString(), (object) eDisclosureRow["Category"].ToString(), (object) eDisclosureRow["eDisclosureElementAttributeID"].ToString()), SQL.DecodeString(eDisclosureRow["MilestoneID"]));
      MilestoneTemplate milestoneTemplate = new MilestoneTemplate(string.Concat(r["TemplateID"]), string.Concat(r["Name"]), SQL.DecodeInt(r["SortIndex"]), SQL.DecodeInt(r["Active"]) != 0, string.Concat(r["Comment"]), string.Concat(r["UpdateBorrowerContactMilestoneID"]), string.Concat(r["AutoLoanNumberingMilestoneID"]), eDisclosureMilestoneSettings, new Guid(string.Concat(r["TemplateGuid"])));
      IEnumerable<EllieMae.EMLite.Workflow.Milestone> milestones = (IEnumerable<EllieMae.EMLite.Workflow.Milestone>) WorkflowBpmDbAccessor.GetMilestones(true);
      foreach (DataRow milestoneRow in milestoneRows)
      {
        DataRow msRow = milestoneRow;
        EllieMae.EMLite.Workflow.Milestone milestone = milestones.ToList<EllieMae.EMLite.Workflow.Milestone>().FirstOrDefault<EllieMae.EMLite.Workflow.Milestone>((System.Func<EllieMae.EMLite.Workflow.Milestone, bool>) (item => item.MilestoneID == string.Concat(msRow["MilestoneID"])));
        int roleID = milestone != null ? milestone.RoleID : -1;
        TemplateMilestone msTemplate = new TemplateMilestone(string.Concat(msRow["MilestoneID"]), SQL.DecodeInt(msRow["SortIndex"]), SQL.DecodeInt(msRow["DaysToComplete"]), roleID, string.Concat(msRow["Name"]), SQL.DecodeBoolean(msRow["Archived"]));
        milestoneTemplate.Add(msTemplate);
      }
      foreach (DataRow roleRow in roleRows)
      {
        TemplateFreeRole freeRole = new TemplateFreeRole(SQL.DecodeInt(roleRow["RoleID"]));
        milestoneTemplate.Add(freeRole);
      }
      return milestoneTemplate;
    }

    private static FieldRuleInfo dataRowToFieldRuleInfo(DataRow r)
    {
      FieldRuleInfo fieldRuleInfo = new FieldRuleInfo(SQL.DecodeInt(r["TemplateID"]), string.Concat(r["Name"]));
      fieldRuleInfo.Condition = (BizRule.Condition) SQL.DecodeInt(r["condition"]);
      fieldRuleInfo.Condition2 = string.Concat(r["ChannelCondition"]);
      fieldRuleInfo.ConditionState = fieldRuleInfo.Condition == BizRule.Condition.AdvancedCoding ? string.Concat(r["advancedCode"]) : string.Concat(r["conditionState"]);
      fieldRuleInfo.ConditionState2 = string.Concat(r["conditionState2"]);
      fieldRuleInfo.AdvancedCodeXML = string.Concat(r["advancedCodeXml"]);
      return fieldRuleInfo;
    }
  }
}
