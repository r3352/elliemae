// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.EDisclosureSignOrderConfiguration
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataEngine.eFolder;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public static class EDisclosureSignOrderConfiguration
  {
    private const string E_DISCLOSURE_SETTINGS = "eDisclosureSettings�";
    private const string E_DISCLOSURE_STATE_SETTINGS = "eDisclosureStateSettings�";

    [PgReady]
    public static EDisclosureSignOrderSetup GetEDisclosureSignOrderSettings()
    {
      EDisclosureSignOrderSetup signOrderSettings = new EDisclosureSignOrderSetup();
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("fn_getedisclosuresettings");
        DbValueList parameters = new DbValueList()
        {
          {
            "v_attr_name",
            (object) signOrderSettings.AttributeName
          }
        };
        DataRowCollection dataRowCollection = pgDbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, parameters);
        if (dataRowCollection != null)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            if (dataRow != null)
            {
              signOrderSettings.SignOrderEnabled = SQL.DecodeString(dataRow["Value"]);
              break;
            }
          }
        }
        Dictionary<string, bool> edisclosureStateSettings = EDisclosureSignOrderConfiguration.GetEDisclosureStateSettings();
        if (edisclosureStateSettings.Count > 0)
          signOrderSettings.States = edisclosureStateSettings;
        return signOrderSettings;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("");
      dbQueryBuilder.AppendLine("GetEDisclosureSettings");
      DataRowCollection dataRowCollection1 = dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, new DbValueList()
      {
        {
          "@attr_name",
          (object) signOrderSettings.AttributeName
        }
      });
      if (dataRowCollection1 != null)
      {
        foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection1)
        {
          if (dataRow != null)
          {
            signOrderSettings.SignOrderEnabled = SQL.DecodeString(dataRow["Value"]);
            break;
          }
        }
      }
      Dictionary<string, bool> edisclosureStateSettings1 = EDisclosureSignOrderConfiguration.GetEDisclosureStateSettings();
      if (edisclosureStateSettings1.Count > 0)
        signOrderSettings.States = edisclosureStateSettings1;
      return signOrderSettings;
    }

    public static void SaveEDisclosureSignOrderSettings(EDisclosureSignOrderSetup objSetup)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Reset();
      dbQueryBuilder.AppendLine("");
      dbQueryBuilder.AppendLine("SetEDisclosureSettings");
      dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, new DbValueList()
      {
        {
          "@attr_name",
          (object) objSetup.AttributeName
        },
        {
          "@attr_val",
          (object) objSetup.SignOrderEnabled
        }
      });
      EDisclosureSignOrderConfiguration.SaveEDisclosureStateSettings(objSetup.States);
    }

    [PgReady]
    private static Dictionary<string, bool> GetEDisclosureStateSettings()
    {
      Dictionary<string, bool> edisclosureStateSettings = new Dictionary<string, bool>();
      if (ClientContext.GetCurrent(false).Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLine("fn_getedisclosurestatesettings");
        DbValueList parameters = new DbValueList();
        DataRowCollection dataRowCollection = pgDbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, parameters);
        if (dataRowCollection != null)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            string key = dataRow[0].ToString();
            bool flag = SQL.DecodeBoolean(dataRow[1], false);
            edisclosureStateSettings.Add(key, flag);
          }
        }
      }
      else
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("");
        dbQueryBuilder.AppendLine(nameof (GetEDisclosureStateSettings));
        DbValueList parameters = new DbValueList();
        DataRowCollection dataRowCollection = dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, parameters);
        if (dataRowCollection != null)
        {
          foreach (DataRow dataRow in (InternalDataCollectionBase) dataRowCollection)
          {
            string key = (string) dataRow[0];
            bool flag = (bool) dataRow[1];
            edisclosureStateSettings.Add(key, flag);
          }
        }
      }
      return edisclosureStateSettings;
    }

    private static void SaveEDisclosureStateSettings(Dictionary<string, bool> dataDict)
    {
      foreach (KeyValuePair<string, bool> keyValuePair in dataDict)
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendLine("");
        dbQueryBuilder.AppendLine("SetEDisclosureStateSettings");
        dbQueryBuilder.ExecuteStoredProc(DbTransactionType.Snapshot, new DbValueList()
        {
          {
            "@state",
            (object) keyValuePair.Key
          },
          {
            "@signing_order",
            (object) keyValuePair.Value
          }
        });
      }
    }
  }
}
