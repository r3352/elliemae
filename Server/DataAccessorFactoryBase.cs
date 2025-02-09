// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DataAccessorFactoryBase
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Elli.Metrics;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Settings;
using EllieMae.EMLite.DataAccess;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public abstract class DataAccessorFactoryBase
  {
    protected IServerSettings Settings => ClientContext.GetCurrent().Settings;

    protected IStorageSettings StorageSettings
    {
      get => ClientContext.GetCurrent().Settings.GetStorageModeSettings();
    }

    protected StorageMode StorageMode
    {
      get => (StorageMode) this.Settings.GetStorageSetting("DataStore.StorageMode");
    }

    protected virtual IDbContext CreateDbContext()
    {
      IDbContext dbContext = (IDbContext) null;
      if (this.StorageMode == StorageMode.MongoOnly || this.StorageMode == StorageMode.MongoFileSystemMaster || this.StorageMode == StorageMode.MongoMaster || this.StorageMode == StorageMode.BothFileSystemMaster || this.StorageMode == StorageMode.BothDatabaseMaster || this.StorageMode == StorageMode.DatabaseOnly)
      {
        UowDbContext uowDbContext = new UowDbContext();
        uowDbContext.ConnectionString = string.Empty;
        uowDbContext.InstanceId = this.StorageSettings.InstanceId;
        dbContext = (IDbContext) uowDbContext;
      }
      else if (this.StorageMode == StorageMode.PostgresOnly || this.StorageMode == StorageMode.BothFileSystemPostgresMaster || this.StorageMode == StorageMode.BothPostgresFileSystemMaster)
      {
        PgDbContext pgDbContext = new PgDbContext();
        pgDbContext.ConnectionString = this.StorageSettings.PostgreSqlConnectionString;
        pgDbContext.InstanceId = this.StorageSettings.InstanceId;
        dbContext = (IDbContext) pgDbContext;
      }
      return dbContext;
    }

    protected virtual ILoanAccessorMetricsRecorder CreateLoanAccessorMetricsRecorder()
    {
      return new MetricsFactory().CreateLoanAccessorMetricsRecorder(this.StorageSettings.ClientId ?? string.Empty, this.StorageSettings.InstanceId ?? string.Empty);
    }

    protected virtual ILoanSerializationMatrixRecorder CreateLoanSerializationMetricsRecorder()
    {
      return new MetricsFactory().CreateLoanSerializationMetricsRecorder(this.StorageSettings.ClientId ?? string.Empty, this.StorageSettings.InstanceId ?? string.Empty);
    }
  }
}
