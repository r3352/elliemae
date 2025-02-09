// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TitleFeeCredentialsAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public static class TitleFeeCredentialsAccessor
  {
    private static readonly string className = nameof (TitleFeeCredentialsAccessor);

    public static bool ValidateCredentials(string orderUID, string loanGUID, string credentials)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select [credentials] from [TitleFeeCredentials] where [orderUID] = " + SQL.Encode((object) orderUID) + " and [loanGUID] = " + SQL.Encode((object) loanGUID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection.Count == 0)
        return false;
      string strB = XT.DSB64x2((string) dataRowCollection[0][nameof (credentials)], TitleFeeCredentialsAccessor.className);
      return string.Compare(credentials, strB, true) == 0;
    }

    public static void SaveTitleFeeCredentials(
      string orderUID,
      string loanGUID,
      string credentials)
    {
      credentials = XT.ESB64x2(credentials, TitleFeeCredentialsAccessor.className);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("if exists (select * from [TitleFeeCredentials] where [orderUID] = " + SQL.Encode((object) orderUID) + " and [loanGUID] = " + SQL.Encode((object) loanGUID) + ")");
      dbQueryBuilder.AppendLine("    update [TitleFeeCredentials] set [credentials] = " + SQL.Encode((object) credentials) + " where [orderUID] = " + SQL.Encode((object) orderUID) + " and [loanGUID] = " + SQL.Encode((object) loanGUID));
      dbQueryBuilder.AppendLine("else");
      dbQueryBuilder.AppendLine("    insert into [TitleFeeCredentials] ([orderUID], [loanGUID], [credentials]) values (" + SQL.Encode((object) orderUID) + ", " + SQL.Encode((object) loanGUID) + ", " + SQL.Encode((object) credentials) + ")");
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static string[] GetTitleFeeCredentials(string[] orderUIDs, string loanGUID)
    {
      string[] strArray = new string[orderUIDs.Length];
      for (int index = 0; index < orderUIDs.Length; ++index)
        strArray[index] = SQL.Encode((object) orderUIDs[index]);
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from [TitleFeeCredentials] where [orderUID] in (" + string.Join(", ", strArray) + ") and [loanGUID] = " + SQL.Encode((object) loanGUID));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
      {
        string str = XT.DSB64x2((string) dataRow["credentials"], TitleFeeCredentialsAccessor.className);
        dictionary.Add(((Guid) dataRow["orderUID"]).ToString(), str);
      }
      dbQueryBuilder.Reset();
      bool flag = false;
      string[] titleFeeCredentials = new string[orderUIDs.Length];
      for (int index = 0; index < orderUIDs.Length; ++index)
      {
        if (dictionary.ContainsKey(orderUIDs[index]))
        {
          titleFeeCredentials[index] = dictionary[orderUIDs[index]];
        }
        else
        {
          titleFeeCredentials[index] = Guid.NewGuid().ToString();
          string str = XT.ESB64x2(titleFeeCredentials[index], TitleFeeCredentialsAccessor.className);
          dbQueryBuilder.AppendLine("insert into [TitleFeeCredentials] ([orderUID], [loanGUID], [credentials]) values (" + SQL.Encode((object) orderUIDs[index]) + ", " + SQL.Encode((object) loanGUID) + ", " + SQL.Encode((object) str) + ")");
          flag = true;
        }
      }
      if (flag)
        dbQueryBuilder.ExecuteNonQuery();
      return titleFeeCredentials;
    }
  }
}
