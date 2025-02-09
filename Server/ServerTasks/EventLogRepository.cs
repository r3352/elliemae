// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.EventLogRepository
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks
{
  public class EventLogRepository
  {
    [PgReady]
    public IList<EventLog> GetOutstanding()
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        List<EventLog> outstanding = new List<EventLog>();
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.Append("select * from ServerTaskEventLog where [Status] != 3");
        foreach (DataRow row in (InternalDataCollectionBase) pgDbQueryBuilder.Execute())
          outstanding.Add(this.Extract(row));
        return (IList<EventLog>) outstanding;
      }
      List<EventLog> outstanding1 = new List<EventLog>();
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Append("select * from ServerTaskEventLog where [Status] != 3");
      foreach (DataRow row in (InternalDataCollectionBase) dbQueryBuilder.Execute())
        outstanding1.Add(this.Extract(row));
      return (IList<EventLog>) outstanding1;
    }

    [PgReady]
    public bool Add(EventLog log)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLineFormat("\tinsert ServerTaskEventLog(FilePath, FileOwner, Position, Status, LastModified) values ({0}, {1}, {2}, {3}, {4})", (object) SQL.Encode((object) log.FilePath), (object) SQL.Encode((object) log.FileOwner), (object) log.Position, (object) (byte) log.Status, (object) SQL.Encode((object) log.LastModified));
        pgDbQueryBuilder.AppendLineFormat("where not exists (select 1 from ServerTaskEventLog where FilePath = {0})", (object) SQL.Encode((object) log.FilePath));
        return Convert.ToInt32(pgDbQueryBuilder.ExecuteNonQueryWithRowCount()) > 0;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLineFormat("if not exists (select * from ServerTaskEventLog where FilePath = {0})", (object) SQL.Encode((object) log.FilePath));
      dbQueryBuilder.AppendLineFormat("\tinsert ServerTaskEventLog(FilePath, FileOwner, Position, Status, LastModified) values ({0}, {1}, {2}, {3}, {4})", (object) SQL.Encode((object) log.FilePath), (object) SQL.Encode((object) log.FileOwner), (object) log.Position, (object) (byte) log.Status, (object) SQL.Encode((object) log.LastModified));
      dbQueryBuilder.AppendLine("select @@rowcount");
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
    }

    [PgReady]
    public bool Update(EventLog log)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder();
        pgDbQueryBuilder.AppendLineFormat("update ServerTaskEventLog set FilePath={0}, FileOwner={1}, Position={2}, Status={3}, LastModified={4}", (object) SQL.Encode((object) log.FilePath), (object) SQL.Encode((object) log.FileOwner), (object) log.Position, (object) (int) log.Status, (object) SQL.Encode((object) log.LastModified));
        pgDbQueryBuilder.AppendLineFormat("where FilePath = {0}", (object) SQL.Encode((object) log.FilePath));
        return Convert.ToInt32(pgDbQueryBuilder.ExecuteNonQueryWithRowCount()) > 0;
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLineFormat("update ServerTaskEventLog set FilePath={0}, FileOwner={1}, Position={2}, Status={3}, LastModified={4}", (object) SQL.Encode((object) log.FilePath), (object) SQL.Encode((object) log.FileOwner), (object) log.Position, (object) (byte) log.Status, (object) SQL.Encode((object) log.LastModified));
      dbQueryBuilder.AppendLineFormat("where FilePath = {0}", (object) SQL.Encode((object) log.FilePath));
      dbQueryBuilder.AppendLine("select @@rowcount");
      return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
    }

    private EventLog Extract(DataRow row)
    {
      return new EventLog()
      {
        FileOwner = Convert.ToString(row["FileOwner"]),
        FilePath = Convert.ToString(row["FilePath"]),
        Id = Convert.ToInt32(row["Id"]),
        LastModified = Convert.ToDateTime(row["LastModified"]),
        Status = (EventLogStatus) Convert.ToByte(row["Status"]),
        Position = Convert.ToInt64(row["Position"])
      };
    }
  }
}
