// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataAccess.DbServerTypeHelpers
// Assembly: DataAccess, Version=6.5.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: A079574B-67E2-4BE9-A7E2-5764B684A9D9
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\DataAccess.dll

using EllieMae.EMLite.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.DataAccess
{
  public class DbServerTypeHelpers
  {
    private static Dictionary<DbServerType, string> s_databaseTypeDbProviderMap = new Dictionary<DbServerType, string>()
    {
      {
        DbServerType.None,
        ""
      },
      {
        DbServerType.SqlServer,
        "System.Data.SqlClient"
      },
      {
        DbServerType.Postgres,
        "Npgsql"
      }
    };

    public static string GetDbProvider(DbServerType databaseType)
    {
      return DbServerTypeHelpers.s_databaseTypeDbProviderMap.FirstOrDefault<KeyValuePair<DbServerType, string>>((Func<KeyValuePair<DbServerType, string>, bool>) (kvp => kvp.Key == databaseType)).Value;
    }

    public static DbProviderFactory GetFactory(DbServerType databaseType)
    {
      switch (databaseType)
      {
        case DbServerType.None:
          throw new ArgumentOutOfRangeException("DatabaseType", (object) databaseType, "error reading 'DatabaseType' key.  Please contact the administrator with the following information:");
        case DbServerType.SqlServer:
          return (DbProviderFactory) SqlClientFactory.Instance;
        case DbServerType.Postgres:
          return (DbProviderFactory) NpgsqlFactory.Instance;
        default:
          throw new ArgumentOutOfRangeException("DatabaseType", (object) databaseType, "error reading 'DatabaseType' key.  Please contact the administrator with the following information:");
      }
    }
  }
}
