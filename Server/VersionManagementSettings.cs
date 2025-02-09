// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.VersionManagementSettings
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class VersionManagementSettings
  {
    private const string className = "VersionManagementSettings�";

    public static VersionManagementGroup[] GetVersionManagementGroups()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("VersionMgmtGroups");
      DbTableInfo table2 = DbAccessManager.GetTable("VersionMgmtGroupUsers");
      dbQueryBuilder.SelectFrom(table1);
      dbQueryBuilder.SelectFrom(table2);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table3 = dataSet.Tables[0];
      DataTable table4 = dataSet.Tables[1];
      List<VersionManagementGroup> versionManagementGroupList = new List<VersionManagementGroup>();
      foreach (DataRow row in (InternalDataCollectionBase) table3.Rows)
        versionManagementGroupList.Add(VersionManagementSettings.dataRowToManagementGroup(row, table4));
      return versionManagementGroupList.ToArray();
    }

    public static VersionManagementGroup GetVersionManagementGroup(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("VersionMgmtGroups");
      DbTableInfo table2 = DbAccessManager.GetTable("VersionMgmtGroupUsers");
      DbValue key = new DbValue("GroupID", (object) groupId);
      dbQueryBuilder.SelectFrom(table1, key);
      dbQueryBuilder.SelectFrom(table2, key);
      DataSet dataSet = dbQueryBuilder.ExecuteSetQuery();
      DataTable table3 = dataSet.Tables[0];
      DataTable table4 = dataSet.Tables[1];
      return table3.Rows.Count == 0 ? (VersionManagementGroup) null : VersionManagementSettings.dataRowToManagementGroup(table3.Rows[0], table4);
    }

    [PgReady]
    public static VersionManagementGroup GetDefaultVersionManagementGroup()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("VersionMgmtGroups");
        DbValue key = new DbValue("IsDefault", (object) 1);
        pgDbQueryBuilder.SelectFrom(table, key);
        DataTable dataTable = pgDbQueryBuilder.ExecuteTableQuery();
        return dataTable.Rows.Count == 0 ? (VersionManagementGroup) null : VersionManagementSettings.dataRowToManagementGroup(dataTable.Rows[0], (DataTable) null);
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("VersionMgmtGroups");
      DbValue key1 = new DbValue("IsDefault", (object) 1);
      dbQueryBuilder.SelectFrom(table1, key1);
      DataTable dataTable1 = dbQueryBuilder.ExecuteTableQuery();
      return dataTable1.Rows.Count == 0 ? (VersionManagementGroup) null : VersionManagementSettings.dataRowToManagementGroup(dataTable1.Rows[0], (DataTable) null);
    }

    private static VersionManagementGroup dataRowToManagementGroup(
      DataRow groupRow,
      DataTable usersTable)
    {
      int groupId = SQL.DecodeInt(groupRow["GroupID"]);
      bool isDefault = SQL.DecodeBoolean(groupRow["IsDefault"]);
      string groupName = SQL.DecodeString(groupRow["GroupName"]);
      ClientAppVersion appVersion = VersionManagementSettings.convertToAppVersion(SQL.DecodeString(groupRow["AuthorizedVersion"]));
      List<string> stringList = new List<string>();
      if (usersTable != null)
      {
        foreach (DataRow dataRow in usersTable.Select("GroupID = " + (object) groupId))
          stringList.Add(string.Concat(dataRow["UserID"]));
      }
      return new VersionManagementGroup(groupId, groupName, isDefault, appVersion, stringList.ToArray());
    }

    public static void UpdateVersionManagementGroup(VersionManagementGroup group)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("VersionMgmtGroups");
      DbTableInfo table2 = DbAccessManager.GetTable("VersionMgmtGroupUsers");
      DbValue key = new DbValue("GroupID", (object) group.GroupID);
      dbQueryBuilder.Update(table1, VersionManagementSettings.createGroupDbValueList(group), key);
      dbQueryBuilder.DeleteFrom(table2, key);
      if (group.GroupUserIDs != null)
      {
        foreach (string groupUserId in group.GroupUserIDs)
          dbQueryBuilder.InsertInto(table2, new DbValueList()
          {
            key,
            {
              "UserID",
              (object) groupUserId
            }
          }, true, false);
      }
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static int CreateVersionManagementGroup(VersionManagementGroup group)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table1 = DbAccessManager.GetTable("VersionMgmtGroups");
      DbTableInfo table2 = DbAccessManager.GetTable("VersionMgmtGroupUsers");
      DbValue dbValue = new DbValue("GroupID", (object) "@groupId", (IDbEncoder) DbEncoding.None);
      dbQueryBuilder.InsertInto(table1, VersionManagementSettings.createGroupDbValueList(group), true, false);
      dbQueryBuilder.SelectIdentity("@groupId");
      if (group.GroupUserIDs != null)
      {
        foreach (string groupUserId in group.GroupUserIDs)
          dbQueryBuilder.InsertInto(table2, new DbValueList()
          {
            dbValue,
            {
              "UserID",
              (object) groupUserId
            }
          }, true, false);
      }
      dbQueryBuilder.Select("@groupId");
      return (int) dbQueryBuilder.ExecuteScalar();
    }

    public static void DeleteVersionManagementGroup(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("VersionMgmtGroups");
      DbValue key = new DbValue("GroupID", (object) groupId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static UserInfoSummary[] GetVersionManagementGroupUsers(int groupId)
    {
      VersionManagementGroup versionManagementGroup = VersionManagementSettings.GetVersionManagementGroup(groupId);
      if (versionManagementGroup == null)
        throw new ObjectNotFoundException("Invalid User Management Group", ObjectType.VersionManagementGroup, (object) groupId);
      if (versionManagementGroup.GroupUserIDs == null)
        return new UserInfoSummary[0];
      return versionManagementGroup.GroupUserIDs.Length == 0 ? new UserInfoSummary[0] : User.ConvertToUserInfoSummaries(User.GetUsers(versionManagementGroup.GroupUserIDs));
    }

    private static DbValueList createGroupDbValueList(VersionManagementGroup group)
    {
      return new DbValueList()
      {
        {
          "GroupName",
          (object) group.GroupName
        },
        {
          "AuthorizedVersion",
          group.AuthorizedVersion == null ? (object) (string) null : (object) group.AuthorizedVersion.NormalizedVersion
        }
      };
    }

    [PgReady]
    public static ClientAppVersion GetAuthorizedVersion(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select MAX(COALESCE(AuthorizedVersion, '" + ClientAppVersion.MaxVersion.NormalizedVersion + "')) from VersionMgmtGroups");
        pgDbQueryBuilder.AppendLine("where IsDefault = 1");
        pgDbQueryBuilder.AppendLine("  or GroupID in (select GroupID from VersionMgmtGroupUsers where userid = @userid)");
        DbCommandParameter parameter = new DbCommandParameter("userid", (object) userId.TrimEnd(), DbType.AnsiString);
        string versionStr = string.Concat(pgDbQueryBuilder.ExecuteScalar(parameter));
        if (string.IsNullOrEmpty(versionStr))
          return (ClientAppVersion) null;
        ClientAppVersion appVersion = VersionManagementSettings.convertToAppVersion(versionStr);
        return appVersion.Equals((object) ClientAppVersion.MaxVersion) ? (ClientAppVersion) null : appVersion;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select max(IsNull(AuthorizedVersion, '" + ClientAppVersion.MaxVersion.NormalizedVersion + "')) from VersionMgmtGroups");
      dbQueryBuilder.AppendLine("where IsDefault = 1");
      dbQueryBuilder.AppendLine("  or GroupID in (select GroupID from VersionMgmtGroupUsers where userid = " + SQL.Encode((object) userId) + ")");
      string versionStr1 = string.Concat(dbQueryBuilder.ExecuteScalar());
      if ((versionStr1 ?? "") == "")
        return (ClientAppVersion) null;
      ClientAppVersion appVersion1 = VersionManagementSettings.convertToAppVersion(versionStr1);
      return appVersion1.Equals((object) ClientAppVersion.MaxVersion) ? (ClientAppVersion) null : appVersion1;
    }

    private static ClientAppVersion convertToAppVersion(string versionStr)
    {
      try
      {
        if ((versionStr ?? "") == "")
          return (ClientAppVersion) null;
        ClientAppVersion appVersion = ClientAppVersion.Parse(versionStr);
        if (appVersion.MajorVersion < VersionInformation.CurrentVersion.Version)
          appVersion = new ClientAppVersion(VersionInformation.CurrentVersion.Version, 0, (string) null);
        return appVersion;
      }
      catch
      {
        return new ClientAppVersion(VersionInformation.CurrentVersion.Version, 0, (string) null);
      }
    }
  }
}
