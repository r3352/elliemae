// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.HomeCounselingAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class HomeCounselingAccessor
  {
    private const string className = "HomeCounselingAccessor�";

    public static bool UpdateHomeCounselingCodes(
      List<KeyValuePair<string, string>> services,
      List<KeyValuePair<string, string>> languages)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      List<string> stringList = (List<string>) null;
      try
      {
        for (int index1 = 1; index1 <= 2; ++index1)
        {
          List<KeyValuePair<string, string>> keyValuePairList = index1 == 1 ? services : languages;
          stringList?.Clear();
          stringList = new List<string>();
          dbQueryBuilder.Reset();
          if (keyValuePairList != null && keyValuePairList.Count > 0)
          {
            for (int index2 = 0; index2 < keyValuePairList.Count; ++index2)
            {
              KeyValuePair<string, string> keyValuePair = keyValuePairList[index2];
              stringList.Add(keyValuePair.Key);
              dbQueryBuilder.AppendLine("IF NOT EXISTS (SELECT 1 FROM [HomeCounselingCodes] WHERE [codeType] = " + (object) index1 + " AND [CodeKey] = " + SQL.Encode((object) keyValuePair.Key) + ")");
              dbQueryBuilder.Begin();
              dbQueryBuilder.AppendLine("    INSERT INTO [HomeCounselingCodes] (codeType, codeKey, codeName) values (" + (object) index1 + ", " + SQL.Encode((object) keyValuePair.Key) + ", " + SQL.Encode((object) keyValuePair.Value) + ")");
              dbQueryBuilder.End();
              dbQueryBuilder.Else();
              dbQueryBuilder.Begin();
              dbQueryBuilder.AppendLine("    UPDATE [HomeCounselingCodes] SET [codeName] = " + SQL.Encode((object) keyValuePair.Value) + " WHERE [codeType] = " + (object) index1 + " AND [CodeKey] = " + SQL.Encode((object) keyValuePair.Key));
              dbQueryBuilder.End();
            }
            dbQueryBuilder.ExecuteNonQuery();
            dbQueryBuilder.Reset();
            string str = string.Empty;
            for (int index3 = 0; index3 < stringList.Count; ++index3)
              str = str + (str != string.Empty ? "," : "") + SQL.Encode((object) stringList[index3]);
            if (str != string.Empty)
            {
              dbQueryBuilder.AppendLine("DELETE FROM [HomeCounselingCodes] WHERE [codeType] = " + (object) index1 + " and [CodeKey] NOT IN (" + str + ")");
              dbQueryBuilder.ExecuteNonQuery();
            }
          }
        }
      }
      catch (Exception ex)
      {
        ClientContext.GetCurrent().TraceLog.WriteException(TraceLevel.Error, nameof (HomeCounselingAccessor), ex);
        throw new Exception("UpdateHomeCounselingCodes: Cannot update HomeCounselingCodes table.\r\n" + ex.Message);
      }
      return true;
    }

    public static List<KeyValuePair<string, string>>[] GetHomeCounselingServiceLanguageSupported()
    {
      return new List<KeyValuePair<string, string>>[2]
      {
        HomeCounselingAccessor.GetHomeCounselingServiceSupported(),
        HomeCounselingAccessor.GetHomeCounselingLanguageSupported()
      };
    }

    public static List<KeyValuePair<string, string>> GetHomeCounselingServiceSupported()
    {
      List<KeyValuePair<string, string>> serviceSupported = new List<KeyValuePair<string, string>>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT codeKey, codeName From [HomeCounselingCodes] WHERE [codeType] = 1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          serviceSupported.Add(new KeyValuePair<string, string>(Convert.ToString(dataRow["codeKey"]), Convert.ToString(dataRow["codeName"])));
      }
      return serviceSupported;
    }

    public static List<KeyValuePair<string, string>> GetHomeCounselingLanguageSupported()
    {
      List<KeyValuePair<string, string>> languageSupported = new List<KeyValuePair<string, string>>();
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendLine("SELECT codeKey, codeName From [HomeCounselingCodes] WHERE [codeType] = 2");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      if (dataRowCollection != null && dataRowCollection.Count > 0)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          languageSupported.Add(new KeyValuePair<string, string>(Convert.ToString(dataRow["codeKey"]), Convert.ToString(dataRow["codeName"])));
      }
      return languageSupported;
    }
  }
}
