// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DbQueryBuilder
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using Encompass.Diagnostics;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class DbQueryBuilder : EllieMae.EMLite.DataAccess.DbQueryBuilder
  {
    public DbQueryBuilder()
      : this((IClientContext) ClientContext.GetCurrent())
    {
    }

    public DbQueryBuilder(IClientContext context)
      : base(context.Settings.ConnectionString)
    {
    }

    public DbQueryBuilder(bool useReadReplica)
      : base(ClientContext.GetCurrent().Settings.GetSqlConnectionString(useReadReplica))
    {
    }

    public DbQueryBuilder(int sqlRead)
      : base(ClientContext.GetCurrent().Settings.GetConnectionString(sqlRead))
    {
    }

    public DbQueryBuilder(DBReadReplicaFeature feature)
      : base(DbQueryBuilder.getConnectionString(feature))
    {
    }

    public static string getConnectionString(DBReadReplicaFeature feature)
    {
      string name = Enum.GetName(typeof (DBReadReplicaFeature), (object) feature);
      IRequestContext currentRequest = ClientContext.CurrentRequest;
      ClientContext current = ClientContext.GetCurrent();
      EnGlobalSettings enGlobalSettings = current.GetEnGlobalSettings();
      if (!current.Settings.IsReadReplicaUseAGListener || currentRequest.ForcePrimaryDB)
        return enGlobalSettings.DatabaseConnectionString;
      bool? replicaOverrideFor = currentRequest.GetDisableReadReplicaOverrideFor(feature);
      int num1 = replicaOverrideFor.HasValue ? (replicaOverrideFor.Value ? 2 : 1) : 0;
      string companySetting = Company.GetCompanySetting((IClientContext) current, "DisableReadReplica", name);
      int num2 = 0;
      DBReadReplicaGlobalFlags result;
      if (Enum.TryParse<DBReadReplicaGlobalFlags>(companySetting, out result))
        num2 = (int) result;
      bool flag = (num2 >> num1 & 1) == 0;
      string connectionString = flag ? enGlobalSettings.ReadReplicaConnectionString : enGlobalSettings.DatabaseConnectionString;
      string message = "DBReadReplicaEnabledFeature:" + name + " UseReadReplica:" + flag.ToString() + " DB:" + (connectionString.Contains("ApplicationIntent=ReadOnly") ? "ReadReplica" : "Primary");
      DiagUtility.LogManager.GetLogger("SQL.ReadReplica").Write(Encompass.Diagnostics.Logging.LogLevel.INFO, "DBQueryBuilder:", message);
      return connectionString;
    }
  }
}
