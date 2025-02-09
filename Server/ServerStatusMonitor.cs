// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerStatusMonitor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Net;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class ServerStatusMonitor
  {
    private const string className = "ServerStatusMonitor�";
    private static System.Threading.Timer monitorTimer;

    public static void Start()
    {
      TimeSpan activityTimerInterval = ServerGlobals.ServerActivityTimerInterval;
      ServerStatusMonitor.monitorTimer = new System.Threading.Timer(new TimerCallback(ServerStatusMonitor.updateServerStatus), (object) null, TimeSpan.Zero, activityTimerInterval);
    }

    public static void ChangeTimerInterval(TimeSpan timerInterval)
    {
      if (ServerStatusMonitor.monitorTimer == null)
        return;
      ServerStatusMonitor.monitorTimer.Change(TimeSpan.Zero, timerInterval);
    }

    private static void updateServerStatus(object state)
    {
      foreach (ClientContext context in ClientContext.GetAll())
      {
        try
        {
          if (!context.Settings.Disabled)
            ServerStatusMonitor.NotifyAlive(context);
        }
        catch (Exception ex)
        {
          ServerGlobals.TraceLog.WriteError(nameof (ServerStatusMonitor), "Error attempting to write server status for instance '" + context.InstanceName + "'");
          ServerGlobals.TraceLog.WriteException(nameof (ServerStatusMonitor), ex);
        }
      }
    }

    [PgReady]
    public static void NotifyAlive(ClientContext context)
    {
      using (context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        ServerStatusMonitor.UpdatingserverstatusValidity();
        try
        {
          DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
          dbQueryBuilder.AppendLine("DELETE FROM ServerStatus WHERE ValidUntil < GetDate()");
          dbQueryBuilder.AppendLine("DELETE FROM Sessions WHERE (ServerID is not null) And (ServerID not in (select ServerID from ServerStatus))");
          dbQueryBuilder.AppendLine("DELETE FROM ServerTaskQueue WHERE (ServerID is not null) And (ServerID not in (select ServerID from ServerStatus))");
          dbQueryBuilder.AppendLine("EXEC SessionDBCleanUpProc");
          dbQueryBuilder.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
          TraceLog.WriteError(nameof (ServerStatusMonitor), "Exception in NotifyAlive" + ex.Message);
        }
      }
    }

    private static void UpdatingserverstatusValidity()
    {
      try
      {
        DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
        DbTableInfo table = DbAccessManager.GetTable("ServerStatus");
        DbValue dbValue = new DbValue("ServerID", (object) EncompassServer.ServerID);
        DbValueList values = new DbValueList();
        values.Add("Hostname", (object) Dns.GetHostName());
        values.Add("Mode", (object) EncompassServer.ServerMode.ToString());
        values.Add("LastActivity", (object) "GetDate()", (IDbEncoder) DbEncoding.None);
        values.Add("ValidUntil", (object) ("DateAdd(s, " + ServerGlobals.ServerActivityTimeout.TotalSeconds.ToString("#") + ", GetDate())"), (IDbEncoder) DbEncoding.None);
        dbQueryBuilder.Declare("@serverId", "varchar(38)");
        dbQueryBuilder.SelectVar("@serverId", (object) EncompassServer.ServerID);
        dbQueryBuilder.IfExists(table, dbValue);
        dbQueryBuilder.Update(table, values, dbValue);
        dbQueryBuilder.Else();
        values.Add(dbValue);
        dbQueryBuilder.InsertInto(table, values, true, false);
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        TraceLog.WriteError(nameof (ServerStatusMonitor), "Exception in UpdatingserverstatusValidity" + ex.Message);
      }
    }
  }
}
