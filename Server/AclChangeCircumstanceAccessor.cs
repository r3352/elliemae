// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclChangeCircumstanceAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclChangeCircumstanceAccessor
  {
    private const string className = "AclChangeCircumstanceAccessor�";
    private const string tableName = "[AclGroupChangeCircumstanceOptions]�";

    [PgReady]
    public static string[] GetAclGroupChangeCircumstanceOptions(int groupId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.Append("select OptionId from [AclGroupChangeCircumstanceOptions] where GroupID = " + SQL.Encode((object) groupId) + ";");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        string[] circumstanceOptions = new string[dataRowCollection.Count];
        for (int index = 0; index < circumstanceOptions.Length; ++index)
          circumstanceOptions[index] = SQL.DecodeString(dataRowCollection[index]["OptionId"]);
        return circumstanceOptions;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select OptionId from [AclGroupChangeCircumstanceOptions] where GroupID = " + SQL.Encode((object) groupId));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      string[] circumstanceOptions1 = new string[dataRowCollection1.Count];
      for (int index = 0; index < circumstanceOptions1.Length; ++index)
        circumstanceOptions1[index] = SQL.DecodeString(dataRowCollection1[index]["OptionId"]);
      return circumstanceOptions1;
    }

    [PgReady]
    public static string[] GetUsersChangeCircumstanceOptions(string userId)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("select distinct agcco.OptionId from [AclGroupChangeCircumstanceOptions] agcco");
        pgDbQueryBuilder.AppendLine(" inner join AclGroupMembers agm on agm.GroupID = agcco.GroupID");
        pgDbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) userId) + ";");
        DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute();
        string[] circumstanceOptions = new string[dataRowCollection.Count];
        for (int index = 0; index < circumstanceOptions.Length; ++index)
          circumstanceOptions[index] = SQL.DecodeString(dataRowCollection[index]["OptionId"]);
        return circumstanceOptions;
      }
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct agcco.OptionId from [AclGroupChangeCircumstanceOptions] agcco");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on agm.GroupID = agcco.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) userId));
      DataRowCollection dataRowCollection1 = dbQueryBuilder.Execute();
      string[] circumstanceOptions1 = new string[dataRowCollection1.Count];
      for (int index = 0; index < circumstanceOptions1.Length; ++index)
        circumstanceOptions1[index] = SQL.DecodeString(dataRowCollection1[index]["OptionId"]);
      return circumstanceOptions1;
    }

    [PgReady]
    public static void ResetAclGroupChangeCircumstanceOptions(int groupId, string[] optionIDs)
    {
      try
      {
        if (optionIDs == null)
          return;
        if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          pgDbQueryBuilder.AppendLine("delete from [AclGroupChangeCircumstanceOptions] where GroupID = " + SQL.Encode((object) groupId) + ";");
          pgDbQueryBuilder.Append("insert into [AclGroupChangeCircumstanceOptions] (GroupID, OptionId) values ");
          pgDbQueryBuilder.AppendLine(string.Join(", ", ((IEnumerable<string>) optionIDs).Select<string, string>((System.Func<string, string>) (optionId => "(" + SQL.Encode((object) groupId) + "," + SQL.Encode((object) optionId) + ")"))) + "; ");
          pgDbQueryBuilder.ExecuteNonQuery();
        }
        else
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("delete from  [AclGroupChangeCircumstanceOptions] where GroupID = " + SQL.Encode((object) groupId));
          for (int index = 0; index < optionIDs.Length; ++index)
            dbQueryBuilder.AppendLine("insert into [AclGroupChangeCircumstanceOptions] (GroupID, OptionId) values (" + SQL.Encode((object) groupId) + ", " + SQL.Encode((object) optionIDs[index]) + ")");
          dbQueryBuilder.ExecuteNonQuery();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclChangeCircumstanceAccessor), ex);
      }
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupChangeCircumstanceOptions] where GroupID = " + (object) sourceGroupID);
        DataTable sourceTable = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable == null || sourceTable.Rows.Count <= 0)
          return;
        AclChangeCircumstanceAccessor.CloneStatementHelper(sourceTable, sql, "[AclGroupChangeCircumstanceOptions]", "GroupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclChangeCircumstanceAccessor), ex);
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
