// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.AclGroupStdPrintFormAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using System;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class AclGroupStdPrintFormAccessor
  {
    private const string className = "AclGroupStdPrintFormAccessor�";
    private const string tableName_AclGroupStdPrintForm = "[AclGroupStdPrintForm]�";

    public static string[] GetAclGroupStdPrintForms(int groupId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.Append("select FileID from AclGroupStdPrintForm where GroupID = " + (object) groupId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      string[] groupStdPrintForms = new string[dataRowCollection.Count];
      for (int index = 0; index < groupStdPrintForms.Length; ++index)
        groupStdPrintForms[index] = (string) dataRowCollection[index]["FileID"];
      return groupStdPrintForms;
    }

    public static string[] GetUsersStdPrintForms(string userId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select distinct agspf.FileID from AclGroupStdPrintForm agspf");
      dbQueryBuilder.AppendLine("   inner join AclGroupMembers agm on agm.GroupID = agspf.GroupID");
      dbQueryBuilder.AppendLine("where agm.UserID = " + SQL.Encode((object) userId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      string[] usersStdPrintForms = new string[dataRowCollection.Count];
      for (int index = 0; index < usersStdPrintForms.Length; ++index)
        usersStdPrintForms[index] = (string) dataRowCollection[index]["FileID"];
      return usersStdPrintForms;
    }

    public static void ResetAclGroupStdPrintForms(
      int groupId,
      string[] fileIds,
      string loggedInUser)
    {
      try
      {
        if (fileIds == null)
          return;
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        dbQueryBuilder.AppendLine("delete from AclGroupStdPrintForm where GroupID = " + (object) groupId);
        for (int index = 0; index < fileIds.Length; ++index)
          dbQueryBuilder.AppendLine("insert into [AclGroupStdPrintForm] (GroupID, FileID) values (" + (object) groupId + ", " + SQL.Encode((object) fileIds[index]) + ")");
        dbQueryBuilder.AppendLine(AclGroupAccessor.UpdateUserGroupMetadata(loggedInUser, groupId));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupStdPrintFormAccessor), ex);
      }
    }

    public static void Clone(int sourceGroupID, int desGroupID)
    {
      try
      {
        DbQueryBuilder sql = new DbQueryBuilder();
        sql.Append("select * from [AclGroupStdPrintForm] where GroupID = " + (object) sourceGroupID);
        DataTable sourceTable = sql.ExecuteTableQuery();
        sql.Reset();
        if (sourceTable == null || sourceTable.Rows.Count <= 0)
          return;
        AclGroupStdPrintFormAccessor.CloneStatementHelper(sourceTable, sql, "[AclGroupStdPrintForm]", "GroupID", desGroupID);
        sql.ExecuteNonQuery();
        sql.Reset();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupStdPrintFormAccessor), ex);
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
