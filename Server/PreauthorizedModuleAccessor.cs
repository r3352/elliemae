// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.PreauthorizedModuleAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataAccess;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class PreauthorizedModuleAccessor
  {
    private const string className = "PreauthorizedModuleAccessor�";

    [PgReady]
    public static PreauthorizedModule[] GetPreauthorizedModules()
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        List<PreauthorizedModule> preauthorizedModuleList = new List<PreauthorizedModule>();
        PreauthorizedModule[] preauthorizedModules = (PreauthorizedModule[]) current.Cache.Get(nameof (PreauthorizedModuleAccessor));
        if (preauthorizedModules != null)
          return preauthorizedModules;
        using (current.Cache.Lock(nameof (PreauthorizedModuleAccessor)))
        {
          PgDbQueryBuilder pgDbQueryBuilder = new PgDbQueryBuilder();
          DbTableInfo table = DbAccessManager.GetTable("PreauthorizedModules");
          pgDbQueryBuilder.SelectFrom(table);
          foreach (DataRow row in (InternalDataCollectionBase) pgDbQueryBuilder.ExecuteTableQuery().Rows)
            preauthorizedModuleList.Add(PreauthorizedModuleAccessor.dataRowToPreauthorizedModule(row));
          current.Cache.Put(nameof (PreauthorizedModuleAccessor), (object) preauthorizedModuleList.ToArray(), CacheSetting.Low);
        }
        return preauthorizedModuleList.ToArray();
      }
      List<PreauthorizedModule> preauthorizedModuleList1 = new List<PreauthorizedModule>();
      PreauthorizedModule[] preauthorizedModules1 = (PreauthorizedModule[]) current.Cache.Get(nameof (PreauthorizedModuleAccessor));
      if (preauthorizedModules1 != null)
        return preauthorizedModules1;
      using (current.Cache.Lock(nameof (PreauthorizedModuleAccessor)))
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("PreauthorizedModules");
        dbQueryBuilder.SelectFrom(table);
        foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.ExecuteTableQuery().Rows)
          preauthorizedModuleList1.Add(PreauthorizedModuleAccessor.dataRowToPreauthorizedModule(row));
        current.Cache.Put(nameof (PreauthorizedModuleAccessor), (object) preauthorizedModuleList1.ToArray(), CacheSetting.Low);
      }
      return preauthorizedModuleList1.ToArray();
    }

    public static void AddPreauthorizedModule(PreauthorizedModule module)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (PreauthorizedModuleAccessor)))
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("PreauthorizedModules");
        dbQueryBuilder.InsertInto(table, new DbValueList()
        {
          {
            "AuthKey",
            (object) module.AuthorizationKey
          },
          {
            "ModuleType",
            (object) (int) module.ModuleType
          },
          {
            "Description",
            (object) (module.Description ?? "")
          }
        }, true, false);
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove(nameof (PreauthorizedModuleAccessor));
      }
    }

    public static void RemovePreauthorizedModule(PreauthorizedModule module)
    {
      ClientContext current = ClientContext.GetCurrent();
      using (current.Cache.Lock(nameof (PreauthorizedModuleAccessor)))
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("PreauthorizedModules");
        dbQueryBuilder.DeleteFrom(table, new DbValueList()
        {
          {
            "AuthKey",
            (object) module.AuthorizationKey
          },
          {
            "ModuleType",
            (object) (int) module.ModuleType
          }
        });
        dbQueryBuilder.ExecuteNonQuery();
        current.Cache.Remove(nameof (PreauthorizedModuleAccessor));
      }
    }

    private static PreauthorizedModule dataRowToPreauthorizedModule(DataRow r)
    {
      return new PreauthorizedModule(string.Concat(r["AuthKey"]), SQL.DecodeEnum<PreauthorizedModuleType>(r["ModuleType"]), SQL.DecodeString(r["Description"]));
    }
  }
}
