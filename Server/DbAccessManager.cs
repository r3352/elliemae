// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DbAccessManager
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class DbAccessManager(ClientContext context) : EllieMae.EMLite.DataAccess.DbAccessManager(context.Settings.ConnectionString, context.Settings.DbServerType), IDisposable
  {
    public DbAccessManager()
      : this(ClientContext.GetCurrent())
    {
    }

    public new static void RemoveTableFromCache(string tableName)
    {
      EllieMae.EMLite.DataAccess.DbAccessManager.RemoveTableFromCache(tableName);
    }

    public static DbTableInfo GetTable(string tableName)
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetTable(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, tableName);
    }

    public static DbTableInfo GetTable(IClientContext context, string tableName)
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetTable(context.Settings.ConnectionString, context.Settings.DbServerType, tableName);
    }

    public static DbTableInfo GetDynamicTable(string tableName)
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetDynamicTable(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType, tableName);
    }

    public static DbTableInfo GetDynamicTable(ClientContext context, string tableName)
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetDynamicTable(context.Settings.ConnectionString, context.Settings.DbServerType, tableName);
    }

    public static void DefragmentDatabase(IServerProgressFeedback feedback)
    {
      EllieMae.EMLite.DataAccess.DbAccessManager.DefragmentDatabase(ClientContext.GetCurrent().Settings.ConnectionString, feedback);
    }

    public static int GetAvgFragmentationLevel()
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetAvgFragmentationLevel(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType);
    }

    [PgReady]
    public static void UpdateDatabaseStatistics(IServerProgressFeedback feedback)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
        return;
      EllieMae.EMLite.DataAccess.DbAccessManager.UpdateDatabaseStatistics(ClientContext.GetCurrent().Settings.ConnectionString, feedback);
    }

    public static int GetConsistencyErrorCount()
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetConsistencyErrorCount(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType);
    }

    public static DbSize GetDatabaseSize()
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetDatabaseSize(ClientContext.GetCurrent().Settings.ConnectionString);
    }

    public static DbUsageInfo GetUsageInfo()
    {
      return EllieMae.EMLite.DataAccess.DbAccessManager.GetUsageInfo(ClientContext.GetCurrent().Settings.ConnectionString, ClientContext.GetCurrent().Settings.DbServerType);
    }
  }
}
