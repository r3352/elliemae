// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DbContextUtil
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using EllieMae.EMLite.DataAccess;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class DbContextUtil
  {
    public static IDbContext GetLoanDbContext(ClientContext context)
    {
      IServerSettings settings = context.Settings;
      IStorageSettings storageModeSettings = settings.GetStorageModeSettings();
      switch ((StorageMode) settings.GetStorageSetting("DataStore.StorageMode"))
      {
        case StorageMode.PostgresOnly:
        case StorageMode.BothPostgresFileSystemMaster:
        case StorageMode.BothFileSystemPostgresMaster:
          PgDbContext loanDbContext1 = new PgDbContext();
          loanDbContext1.ConnectionString = storageModeSettings.PostgreSqlConnectionString;
          loanDbContext1.InstanceId = storageModeSettings.InstanceId;
          return (IDbContext) loanDbContext1;
        default:
          UowDbContext loanDbContext2 = new UowDbContext();
          loanDbContext2.ConnectionString = storageModeSettings.PostgreSqlConnectionString;
          loanDbContext2.InstanceId = storageModeSettings.InstanceId;
          return (IDbContext) loanDbContext2;
      }
    }
  }
}
