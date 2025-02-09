// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.LoanProgramConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class LoanProgramConfiguration
  {
    public static string[] GetAdditionalFields()
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("select FieldID from LoanProgramAdditionalFields order by FieldOrder");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      string[] additionalFields = new string[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        additionalFields[index] = SQL.DecodeString(dataRowCollection[index]["FieldID"]);
      return additionalFields;
    }

    public static void SetAdditionalFields(string[] fieldIds)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      DbTableInfo table = DbAccessManager.GetTable("LoanProgramAdditionalFields");
      dbQueryBuilder.AppendLine("delete from LoanProgramAdditionalFields");
      for (int index = 0; index < fieldIds.Length; ++index)
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "FieldID",
            (object) fieldIds[index]
          },
          {
            "FieldOrder",
            (object) index
          }
        }, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
