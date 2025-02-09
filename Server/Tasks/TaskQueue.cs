// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TaskQueue
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.DataAccess.Postgres;
using EllieMae.EMLite.Server.ServerTasks;
using EllieMae.EMLite.Server.ServerTasks.PostVersionMigrationTasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class TaskQueue
  {
    private const string className = "TaskQueue�";
    private static List<TaskProcessor> processors = (List<TaskProcessor>) null;
    private static Dictionary<ServerTaskType, ITaskHandler> taskHandlers = new Dictionary<ServerTaskType, ITaskHandler>();
    private ClientContext context;
    private AutoResetEvent queueEvent;
    private readonly object _dequeueLock = new object();

    static TaskQueue()
    {
      TaskQueue.RegisterTaskHandler(ServerTaskType.Demo, (ITaskHandler) new DemoTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.SettingAuditTrail, (ITaskHandler) new SettingAuditTrailTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.EPassMessagePoll, (ITaskHandler) new EPassMessagePollTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.UpdateDbStatistics, (ITaskHandler) new UpdateDbStatisticsTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.PlatformSessionCleanup, (ITaskHandler) new PlatformSessionCleanupTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.ReportGenerator, (ITaskHandler) new ReportingTaskhandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.LoanVersionCleanup, (ITaskHandler) new LoanVersionCleanupTask());
      TaskQueue.RegisterTaskHandler(ServerTaskType.AttachmentMetadataMigration, (ITaskHandler) new AttachmentMetadataMigrationToCIFSTask());
      TaskQueue.RegisterTaskHandler(ServerTaskType.KafkaEventForwarder, (ITaskHandler) new KafkaEventForwarderTask());
      TaskQueue.RegisterTaskHandler(ServerTaskType.RenewServerLicense, (ITaskHandler) new RenewServerLicenseTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.RetryTradeSync, (ITaskHandler) new RetryTradeSyncTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.UpdateCorrTradeStatus, (ITaskHandler) new UpdateCorrTradeStatusTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.RecentLoansPurge, (ITaskHandler) new RecentLoansPurgeTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.SoftLoanArchival, (ITaskHandler) new SoftLoanArchivalTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.PostVersionMigration, (ITaskHandler) new PostVersionMigrationTaskHandler());
      TaskQueue.RegisterTaskHandler(ServerTaskType.AnalyzerFieldMigration, (ITaskHandler) new AnalyzersCleanupFromLoanExternalTableTask());
    }

    public static void Start()
    {
      lock (typeof (TaskQueue))
      {
        if (TaskQueue.processors != null)
          return;
        TaskQueue.processors = new List<TaskProcessor>();
        for (int index = 0; index < EllieMae.EMLite.Server.ServerGlobals.TaskProcessingThreadCount; ++index)
          TaskQueue.processors.Add(new TaskProcessor());
      }
    }

    public static string AddTaskProcessor()
    {
      lock (typeof (TaskQueue))
      {
        if (TaskQueue.processors == null)
          throw new Exception("The TaskQueue has not been started on this server.");
        TaskProcessor taskProcessor = new TaskProcessor();
        TaskQueue.processors.Add(taskProcessor);
        return taskProcessor.ProcessorID;
      }
    }

    public static void ResetTaskProcessors()
    {
      lock (typeof (TaskQueue))
      {
        if (TaskQueue.processors == null)
          return;
        foreach (TaskProcessor processor in TaskQueue.processors)
          processor.RefreshContextList();
      }
    }

    public static void RegisterTaskHandler(ServerTaskType taskType, ITaskHandler handler)
    {
      lock (TaskQueue.taskHandlers)
        TaskQueue.taskHandlers[taskType] = handler;
    }

    public static ITaskHandler GetTaskHandler(ServerTaskType taskType)
    {
      lock (TaskQueue.taskHandlers)
        return TaskQueue.taskHandlers.ContainsKey(taskType) ? TaskQueue.taskHandlers[taskType] : throw new ArgumentException("The task type '" + (object) taskType + "' has no associated handler");
    }

    public TaskQueue(ClientContext context)
    {
      this.context = context;
      this.queueEvent = new AutoResetEvent(true);
    }

    public void Activate() => TaskQueue.ResetTaskProcessors();

    public WaitHandle WaitHandle => (WaitHandle) this.queueEvent;

    public ClientContext Context => this.context;

    public int Enqueue(ServerTask info)
    {
      using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskQueue");
        dbQueryBuilder.InsertInto(table, TaskQueue.createServerTaskValueList(info), true, false);
        dbQueryBuilder.SelectIdentity();
        int num = SQL.DecodeInt(dbQueryBuilder.ExecuteScalar());
        this.queueEvent.Set();
        return num;
      }
    }

    [PgReady]
    public ServerTask Dequeue()
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        try
        {
          lock (this._dequeueLock)
          {
            this.RequeueOrphanedTasks();
            using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
            {
              EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder((IClientContext) this.context);
              pgDbQueryBuilder.AppendLine("UPDATE ServerTaskQueue SET");
              pgDbQueryBuilder.AppendLine("    StartTime = GetDate(), Status = " + (object) 2 + ", ServerID = " + SQL.Encode((object) EncompassServer.ServerID));
              pgDbQueryBuilder.AppendLine("    WHERE TaskID IN (SELECT TaskID FROM ServerTaskQueue WHERE Status = " + (object) 1 + " ORDER BY TaskID LIMIT 1 FOR UPDATE)");
              pgDbQueryBuilder.AppendLine("    RETURNING *;");
              DataRowCollection dataRowCollection = pgDbQueryBuilder.Execute(DbTransactionType.Serialized);
              return dataRowCollection.Count == 0 ? (ServerTask) null : TaskQueue.dataRowToTaskInfo(dataRowCollection[0]);
            }
          }
        }
        catch (Exception ex)
        {
          this.Context.TraceLog.WriteError(nameof (TaskQueue), "Error dequeuing task -- task will will skipped for now: " + (object) ex);
          return (ServerTask) null;
        }
      }
      else
      {
        try
        {
          lock (this)
          {
            this.RequeueOrphanedTasks();
            using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
            {
              EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
              dbQueryBuilder.Declare("@taskId", "int");
              dbQueryBuilder.AppendLine("select @taskId = min(TaskID) from ServerTaskQueue where Status = " + (object) 1);
              dbQueryBuilder.AppendLine("if @taskId is not null");
              dbQueryBuilder.AppendLine("begin");
              dbQueryBuilder.AppendLine("   update ServerTaskQueue set StartTime = GetDate(), Status = " + (object) 2 + ", ServerID = " + SQL.Encode((object) EncompassServer.ServerID) + " where TaskID = @taskId");
              dbQueryBuilder.AppendLine("   select * from ServerTaskQueue where TaskID = @taskId");
              dbQueryBuilder.AppendLine("end");
              dbQueryBuilder.AppendLine("else");
              dbQueryBuilder.AppendLine("   select * from ServerTaskQueue where 0 = 1");
              DataRowCollection dataRowCollection = dbQueryBuilder.Execute(DbTransactionType.Serialized);
              return dataRowCollection.Count == 0 ? (ServerTask) null : TaskQueue.dataRowToTaskInfo(dataRowCollection[0]);
            }
          }
        }
        catch (Exception ex)
        {
          this.Context.TraceLog.WriteError(nameof (TaskQueue), "Error dequeuing task -- task will will skipped for now: " + (object) ex);
          return (ServerTask) null;
        }
      }
    }

    [PgReady]
    public bool RequeueOrphanedTasks()
    {
      if (this.context.Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          EllieMae.EMLite.Server.PgDbQueryBuilder pgDbQueryBuilder = new EllieMae.EMLite.Server.PgDbQueryBuilder((IClientContext) this.context);
          pgDbQueryBuilder.AppendLine("update ServerTaskQueue set status = " + (object) 1 + ", ServerID = NULL");
          pgDbQueryBuilder.AppendLine("   where status = " + (object) 2);
          pgDbQueryBuilder.AppendLine("     and ServerID <> " + SQL.Encode((object) EncompassServer.ServerID));
          pgDbQueryBuilder.AppendLine("     and ServerID not in (select ServerID from ServerStatus);");
          return SQL.DecodeInt(pgDbQueryBuilder.ExecuteNonQueryWithRowCount()) > 0;
        }
      }
      else
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          dbQueryBuilder.AppendLine("update ServerTaskQueue set status = " + (object) 1 + ", ServerID = NULL");
          dbQueryBuilder.AppendLine("   where status = " + (object) 2);
          dbQueryBuilder.AppendLine("     and ServerID <> " + SQL.Encode((object) EncompassServer.ServerID));
          dbQueryBuilder.AppendLine("     and ServerID not in (select ServerID from ServerStatus)");
          dbQueryBuilder.AppendLine("select @@rowcount");
          return SQL.DecodeInt(dbQueryBuilder.ExecuteScalar()) > 0;
        }
      }
    }

    public void CheckForQueuedItems() => this.queueEvent.Set();

    public void DeleteStuckTasks()
    {
      try
      {
        DateTime dateTime = DateTime.Now.Subtract(TimeSpan.FromHours(8.0));
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        dbQueryBuilder.AppendLine(string.Format("delete from ServerTaskQueue where StartTime < {0} and Status = {1} ", (object) SQL.EncodeDateTime(dateTime), (object) SQL.Encode((object) 2)));
        dbQueryBuilder.ExecuteNonQuery();
      }
      catch (Exception ex)
      {
        this.Context.TraceLog.WriteWarning(nameof (TaskQueue), "Error deleting stuck tasks: " + (object) ex);
      }
    }

    [PgReady]
    public void SetTaskOutcome(ServerTask taskInfo, ServerTaskOutcome outcome, string message)
    {
      if (ClientContext.GetCurrent().Settings.DbServerType == DbServerType.Postgres)
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          DateTime now = DateTime.Now;
          EllieMae.EMLite.Server.PgDbQueryBuilder idbqb = new EllieMae.EMLite.Server.PgDbQueryBuilder();
          DbTableInfo table1 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskQueue");
          DbValue key1 = new DbValue("TaskID", (object) taskInfo.TaskID);
          VariableScope container = new VariableScope((EllieMae.EMLite.DataAccess.PgDbQueryBuilder) idbqb, (IList<DbVariable>) new List<DbVariable>()
          {
            new DbVariable("v_scheduleid", DbType.Int32),
            new DbVariable("v_starttime", DbType.DateTime)
          });
          container.EmitOpenScope();
          idbqb.AppendLine("select ScheduleID, StartTime INTO v_scheduleid, v_starttime from ServerTaskQueue where TaskID = " + (object) taskInfo?.TaskID + ";");
          IfThenElseBlock ifThenElseBlock = new IfThenElseBlock((EllieMae.EMLite.DataAccess.Postgres.Scope) container, "v_scheduleid is not null");
          ifThenElseBlock.EmitOpenScope();
          DbTableInfo table2 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerScheduleHistory");
          DbValueList values1 = new DbValueList();
          values1.Add("ScheduleID", (object) "v_scheduleid", (IDbEncoder) DbEncoding.None);
          values1.Add("StartTime", (object) "v_startTime", (IDbEncoder) DbEncoding.None);
          values1.Add("EndTime", (object) now);
          values1.Add("Outcome", (object) (int) outcome);
          values1.Add("Message", (object) message, (IDbEncoder) DbEncoding.EmptyStringAsNull);
          if (outcome == ServerTaskOutcome.Failed)
            idbqb.InsertInto(table2, values1, true, false);
          idbqb.DeleteFrom(table1, key1);
          DbTableInfo table3 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
          DbValue key2 = new DbValue("ScheduleID", (object) "v_scheduleid", (IDbEncoder) DbEncoding.None);
          DbValueList values2 = new DbValueList();
          if (outcome == ServerTaskOutcome.Success)
            values2.Add("LastCompletionTime", (object) now);
          values2.Add("LastExecutionTime", (object) now);
          idbqb.Update(table3, values2, key2);
          ifThenElseBlock.Else();
          if (outcome == ServerTaskOutcome.Success)
            idbqb.DeleteFrom(table1, key1);
          else
            idbqb.Update(table1, new DbValueList()
            {
              {
                "Status",
                (object) (int) outcome
              }
            }, key1);
          ifThenElseBlock.EmitCloseScope();
          container.EmitCloseScope();
          idbqb.AppendLine("delete from ServerScheduleHistory where EndTime < " + SQL.EncodeDateTime(now - this.context.Settings.TaskHistoryExpirationPeriod));
          idbqb.ExecuteNonQuery(DbTransactionType.Serialized);
        }
      }
      else
      {
        using (this.context.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        {
          DateTime now = DateTime.Now;
          EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
          DbTableInfo table4 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskQueue");
          DbValue key3 = new DbValue("TaskID", (object) taskInfo.TaskID);
          dbQueryBuilder.Declare("@scheduleId", "int");
          dbQueryBuilder.Declare("@startTime", "dateTime");
          dbQueryBuilder.AppendLine("select @scheduleId = ScheduleID, @startTime = StartTime from ServerTaskQueue where TaskID = " + (object) taskInfo?.TaskID);
          dbQueryBuilder.If("@scheduleId is not NULL");
          dbQueryBuilder.Begin();
          DbTableInfo table5 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerScheduleHistory");
          DbValueList values3 = new DbValueList();
          values3.Add("ScheduleID", (object) "@scheduleId", (IDbEncoder) DbEncoding.None);
          values3.Add("StartTime", (object) "@startTime", (IDbEncoder) DbEncoding.None);
          values3.Add("EndTime", (object) now);
          values3.Add("Outcome", (object) (int) outcome);
          values3.Add("Message", (object) message, (IDbEncoder) DbEncoding.EmptyStringAsNull);
          if (outcome == ServerTaskOutcome.Failed)
            dbQueryBuilder.InsertInto(table5, values3, true, false);
          dbQueryBuilder.DeleteFrom(table4, key3);
          DbTableInfo table6 = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
          DbValue key4 = new DbValue("ScheduleID", (object) "@scheduleId", (IDbEncoder) DbEncoding.None);
          DbValueList values4 = new DbValueList();
          if (outcome == ServerTaskOutcome.Success)
            values4.Add("LastCompletionTime", (object) now);
          values4.Add("LastExecutionTime", (object) now);
          dbQueryBuilder.Update(table6, values4, key4);
          dbQueryBuilder.End();
          dbQueryBuilder.Else();
          dbQueryBuilder.Begin();
          if (outcome == ServerTaskOutcome.Success)
            dbQueryBuilder.DeleteFrom(table4, key3);
          else
            dbQueryBuilder.Update(table4, new DbValueList()
            {
              {
                "Status",
                (object) (int) outcome
              }
            }, key3);
          dbQueryBuilder.End();
          dbQueryBuilder.AppendLine("delete from ServerScheduleHistory where EndTime < " + SQL.EncodeDateTime(DateTime.Now - this.context.Settings.TaskHistoryExpirationPeriod));
          dbQueryBuilder.ExecuteNonQuery(DbTransactionType.Serialized);
        }
      }
    }

    private static ServerTask dataRowToTaskInfo(DataRow r)
    {
      return new ServerTask(SQL.DecodeInt(r["TaskID"]), SQL.DecodeInt(r["ScheduleID"], -1), SQL.DecodeEnum<ServerTaskType>(r["TaskType"]), SQL.DecodeString(r["Data"], (string) null));
    }

    private static DbValueList createServerTaskValueList(ServerTask info)
    {
      return new DbValueList()
      {
        {
          "TaskType",
          (object) info.TaskType
        },
        {
          "ServerID",
          (object) EncompassServer.ServerID
        },
        {
          "Data",
          (object) info.Data
        }
      };
    }

    public static ServerTaskProcessorStatus[] GetProcessorStatuses()
    {
      List<ServerTaskProcessorStatus> taskProcessorStatusList = new List<ServerTaskProcessorStatus>();
      lock (typeof (TaskQueue))
      {
        if (TaskQueue.processors != null)
        {
          foreach (TaskProcessor processor in TaskQueue.processors)
            taskProcessorStatusList.Add(processor.GetStatus());
        }
      }
      return taskProcessorStatusList.ToArray();
    }
  }
}
