// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TaskScheduleAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using System;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public static class TaskScheduleAccessor
  {
    [PgReady]
    public static ServerTaskScheduleInfo[] GetTaskSchedules(
      bool activeOnly,
      bool includeSystemTasks)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("select sts.*, stq.Status");
        pgDbQueryBuilder.AppendLine("from ServerTaskSchedule sts left outer join ServerTaskQueue stq on sts.ScheduleID = stq.ScheduleID");
        pgDbQueryBuilder.AppendLine("where (1 = 1)");
        if (!includeSystemTasks)
          pgDbQueryBuilder.AppendLine("and sts.SystemTask = 0");
        if (activeOnly)
          pgDbQueryBuilder.AppendLine("and sts.Enabled = 1");
        return pgDbQueryBuilder.Execute().Cast<DataRow>().Select<DataRow, ServerTaskScheduleInfo>((System.Func<DataRow, ServerTaskScheduleInfo>) (row => TaskScheduleAccessor.dataRowToServerTaskScheduleInfo(row))).ToArray<ServerTaskScheduleInfo>();
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select sts.*, stq.Status");
      dbQueryBuilder.AppendLine("from ServerTaskSchedule sts left outer join ServerTaskQueue stq on sts.ScheduleID = stq.ScheduleID");
      dbQueryBuilder.AppendLine("where stq.ScheduleID is null and IntervalUnit <> 'OnlyOnce'");
      if (!includeSystemTasks)
        dbQueryBuilder.AppendLine("and sts.SystemTask = 0");
      if (activeOnly)
        dbQueryBuilder.AppendLine("and sts.Enabled = 1");
      dbQueryBuilder.AppendLine("UNION ALL");
      dbQueryBuilder.AppendLine("select sts.*, stq.Status");
      dbQueryBuilder.AppendLine("from ServerTaskSchedule sts left outer join ServerTaskQueue stq on sts.ScheduleID = stq.ScheduleID");
      dbQueryBuilder.AppendLine("where stq.ScheduleID is null and IntervalUnit = 'OnlyOnce' AND LastCompletionTime is null AND LastExecutionTime is null");
      if (!includeSystemTasks)
        dbQueryBuilder.AppendLine("and sts.SystemTask = 0");
      if (activeOnly)
        dbQueryBuilder.AppendLine("and sts.Enabled = 1");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ServerTaskScheduleInfo[] taskSchedules = new ServerTaskScheduleInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        taskSchedules[index] = TaskScheduleAccessor.dataRowToServerTaskScheduleInfo(dataRowCollection[index]);
      return taskSchedules;
    }

    public static ServerTaskScheduleInfo[] GetUserTaskSchedules(string userId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select sts.*, stq.Status");
      dbQueryBuilder.AppendLine("from ServerTaskSchedule sts left outer join ServerTaskQueue stq on sts.ScheduleID = stq.ScheduleID");
      dbQueryBuilder.AppendLine("where OwnerID = " + SQL.Encode((object) userId));
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ServerTaskScheduleInfo[] userTaskSchedules = new ServerTaskScheduleInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        userTaskSchedules[index] = TaskScheduleAccessor.dataRowToServerTaskScheduleInfo(dataRowCollection[index]);
      return userTaskSchedules;
    }

    public static ServerTaskScheduleInfo GetTaskSchedule(int scheduleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select sts.*, stq.Status");
      dbQueryBuilder.AppendLine("from ServerTaskSchedule sts left outer join ServerTaskQueue stq on sts.ScheduleID = stq.ScheduleID");
      dbQueryBuilder.AppendLine("where sts.ScheduleID = " + (object) scheduleId);
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      return dataRowCollection.Count == 0 ? (ServerTaskScheduleInfo) null : TaskScheduleAccessor.dataRowToServerTaskScheduleInfo(dataRowCollection[0]);
    }

    public static ServerTaskScheduleInfo CreateTaskSchedule(ServerTaskScheduleInfo schedule)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
      DbValueList scheduleDbValueList = TaskScheduleAccessor.createScheduleDbValueList(schedule);
      dbQueryBuilder.InsertInto(table, scheduleDbValueList, true, false);
      dbQueryBuilder.SelectIdentity();
      return TaskScheduleAccessor.GetTaskSchedule(SQL.DecodeInt(dbQueryBuilder.ExecuteScalar()));
    }

    public static void UpdateTaskSchedule(ServerTaskScheduleInfo schedule)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
      DbValue key = new DbValue("ScheduleID", (object) schedule.ScheduleID);
      DbValueList scheduleDbValueList = TaskScheduleAccessor.createScheduleDbValueList(schedule);
      dbQueryBuilder.Update(table, scheduleDbValueList, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteTaskSchedule(int scheduleId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
      DbValue key = new DbValue("ScheduleID", (object) scheduleId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static void DeleteTaskFromQueue(int taskId)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskQueue");
      DbValue key = new DbValue("TaskID", (object) taskId);
      dbQueryBuilder.DeleteFrom(table, key);
      dbQueryBuilder.ExecuteNonQuery();
    }

    public static ServerTaskHistoryInfo[] GetTaskScheduleHistory(int scheduleId, DateTime startTime)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.AppendLine("select * from ServerTaskHistory");
      dbQueryBuilder.AppendLine("where ScheduleID = " + (object) scheduleId);
      if (startTime > DateTime.MinValue)
        dbQueryBuilder.AppendLine("   and StartTime >= " + SQL.Encode((object) startTime));
      dbQueryBuilder.AppendLine("order by EndTime desc");
      DataRowCollection dataRowCollection = dbQueryBuilder.Execute();
      ServerTaskHistoryInfo[] taskScheduleHistory = new ServerTaskHistoryInfo[dataRowCollection.Count];
      for (int index = 0; index < dataRowCollection.Count; ++index)
        taskScheduleHistory[index] = TaskScheduleAccessor.dataRowToTaskHistoryInfo(dataRowCollection[index]);
      return taskScheduleHistory;
    }

    private static ServerTaskHistoryInfo dataRowToTaskHistoryInfo(DataRow r)
    {
      return new ServerTaskHistoryInfo(SQL.DecodeInt(r["HistoryID"]), SQL.DecodeInt(r["ScheduleID"]), SQL.DecodeDateTime(r["StartTime"]), SQL.DecodeDateTime(r["EndTime"]), (ServerTaskOutcome) SQL.DecodeInt(r["Outcome"]));
    }

    private static DbValueList createScheduleDbValueList(ServerTaskScheduleInfo schedule)
    {
      return new DbValueList()
      {
        {
          "OwnerID",
          (object) schedule.OwnerID,
          (IDbEncoder) DbEncoding.EmptyStringAsNull
        },
        {
          "Description",
          (object) schedule.Description
        },
        {
          "TaskType",
          (object) schedule.TaskType.ToString()
        },
        {
          "SystemTask",
          (object) schedule.IsSystemTask,
          (IDbEncoder) DbEncoding.Flag
        },
        {
          "StartTime",
          (object) schedule.StartTime
        },
        {
          "IntervalUnit",
          (object) schedule.FrequencyUnit.ToString()
        },
        {
          "IntervalCount",
          (object) schedule.FrequencyCount
        },
        {
          "DaysOfWeek",
          (object) TaskScheduleAccessor.encodeDaysOfWeek(schedule.DaysOfWeek)
        },
        {
          "Enabled",
          (object) schedule.Enabled,
          (IDbEncoder) DbEncoding.Flag
        }
      };
    }

    [PgReady]
    public static int CreateTaskForSchedule(int scheduleId)
    {
      ClientContext current = ClientContext.GetCurrent();
      if (current.Settings.DbServerType == DbServerType.Postgres)
      {
        EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder((IClientContext) current);
        pgDbQueryBuilder.AppendLine("insert into ServerTaskQueue (tasktype, status, serverid, starttime, data, scheduleid)");
        pgDbQueryBuilder.AppendLine("    select TaskType, " + (object) 1 + ", NULL, NULL, Data, ScheduleID");
        pgDbQueryBuilder.AppendLine("    from ServerTaskSchedule");
        pgDbQueryBuilder.AppendLine("    where ScheduleID = " + (object) scheduleId);
        pgDbQueryBuilder.AppendLine("        and not exists (select 1 from ServerTaskQueue where ScheduleID = " + (object) scheduleId + ")");
        pgDbQueryBuilder.AppendLine("returning TaskID;");
        return SQL.DecodeInt(pgDbQueryBuilder.ExecuteScalar(DbTransactionType.Serialized), -1);
      }
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      dbQueryBuilder.Declare("@taskId", "int");
      dbQueryBuilder.AppendLine("select @taskId = TaskID from ServerTaskQueue where ScheduleID = " + (object) scheduleId);
      dbQueryBuilder.If("@taskId is NULL");
      dbQueryBuilder.Begin();
      dbQueryBuilder.AppendLine("insert into ServerTaskQueue select TaskType, " + (object) 1 + ", NULL, NULL, Data, ScheduleID from ServerTaskSchedule where ScheduleID = " + (object) scheduleId);
      dbQueryBuilder.SelectIdentity();
      dbQueryBuilder.End();
      return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar(DbTransactionType.Serialized), -1);
    }

    private static ServerTaskScheduleInfo dataRowToServerTaskScheduleInfo(DataRow r)
    {
      return new ServerTaskScheduleInfo((int) r["ScheduleID"], SQL.DecodeString(r["OwnerID"], (string) null), SQL.DecodeString(r["Description"]), SQL.DecodeEnum<ServerTaskType>(r["TaskType"]), SQL.DecodeBoolean(r["SystemTask"]), SQL.DecodeDateTime(r["StartTime"]), SQL.DecodeEnum<TaskFrequencyUnit>(r["IntervalUnit"]), SQL.DecodeInt(r["IntervalCount"]), TaskScheduleAccessor.decodeDaysOfWeek(SQL.DecodeString(r["DaysOfWeek"])), SQL.DecodeBoolean(r["Enabled"]), SQL.DecodeDateTime(r["LastCompletionTime"]), SQL.DecodeEnum<ServerTaskStatus>(r["Status"], ServerTaskStatus.None), SQL.DecodeDateTime(r["LastExecutionTime"]), SQL.DecodeBoolean(r["FaultTolerance"]), SQL.DecodeString(r["Data"]));
    }

    private static TaskDaysOfWeek decodeDaysOfWeek(string text)
    {
      TaskDaysOfWeek taskDaysOfWeek = TaskDaysOfWeek.None;
      for (int index = 0; index < text.Length; ++index)
      {
        switch (text[index])
        {
          case 'A':
            taskDaysOfWeek |= TaskDaysOfWeek.Saturday;
            break;
          case 'F':
            taskDaysOfWeek |= TaskDaysOfWeek.Friday;
            break;
          case 'H':
            taskDaysOfWeek |= TaskDaysOfWeek.Thursday;
            break;
          case 'M':
            taskDaysOfWeek |= TaskDaysOfWeek.Monday;
            break;
          case 'S':
            taskDaysOfWeek |= TaskDaysOfWeek.Sunday;
            break;
          case 'T':
            taskDaysOfWeek |= TaskDaysOfWeek.Tuesday;
            break;
          case 'W':
            taskDaysOfWeek |= TaskDaysOfWeek.Wednesday;
            break;
        }
      }
      return taskDaysOfWeek;
    }

    private static string encodeDaysOfWeek(TaskDaysOfWeek days)
    {
      string str = "";
      Bitmask bitmask = new Bitmask((object) days);
      if (bitmask.Contains((object) TaskDaysOfWeek.Sunday))
        str += "S";
      if (bitmask.Contains((object) TaskDaysOfWeek.Monday))
        str += "M";
      if (bitmask.Contains((object) TaskDaysOfWeek.Tuesday))
        str += "T";
      if (bitmask.Contains((object) TaskDaysOfWeek.Wednesday))
        str += "W";
      if (bitmask.Contains((object) TaskDaysOfWeek.Thursday))
        str += "H";
      if (bitmask.Contains((object) TaskDaysOfWeek.Friday))
        str += "F";
      if (bitmask.Contains((object) TaskDaysOfWeek.Saturday))
        str += "A";
      return str;
    }
  }
}
