// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.LockComparisonFields
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.DataAccess;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class LockComparisonFields
  {
    public static IList<LockComparisonField> GetLockComparisonFields()
    {
      IList<LockComparisonField> comparisonFields = (IList<LockComparisonField>) new List<LockComparisonField>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from LockComparisonFields");
      foreach (DataRow dataRow in (IEnumerable) dbQueryBuilder.Execute())
        comparisonFields.Add(new LockComparisonField()
        {
          LoanFieldId = SQL.DecodeString(dataRow["LoanFieldId"]),
          LockRequestFieldId = SQL.DecodeString(dataRow["LockFieldId"])
        });
      return comparisonFields;
    }

    public static void InsertLockComparisonFields(IList<LockComparisonField> lockComparisonFields)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("delete from LockComparisonFields");
      foreach (LockComparisonField lockComparisonField in (IEnumerable<LockComparisonField>) lockComparisonFields)
        dbQueryBuilder.AppendFormat("insert into LockComparisonFields (loanFieldId, lockFieldId) values ({0}, {1})\n", (object) SQL.Encode((object) lockComparisonField.LoanFieldId), (object) SQL.Encode((object) lockComparisonField.LockRequestFieldId));
      dbQueryBuilder.ExecuteNonQuery();
    }
  }
}
