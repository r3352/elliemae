// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerTasks.PostVersionMigrationTasks.PostVersionMigrationTaskHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.Server.Tasks;
using Encompass.Diagnostics;
using Encompass.Diagnostics.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.Server.ServerTasks.PostVersionMigrationTasks
{
  internal class PostVersionMigrationTaskHandler : ITaskHandler
  {
    private readonly Dictionary<PostVersionMigrationTaskHandler.PostVersionMigrationTaskType, IPostVersionMigrationTaskHandler> _taskHandlersMap;
    private const int MaxExecutionTimeInMins = 180;
    private ILogger pvmTaskTraceLog;

    public PostVersionMigrationTaskHandler()
    {
      this._taskHandlersMap = new Dictionary<PostVersionMigrationTaskHandler.PostVersionMigrationTaskType, IPostVersionMigrationTaskHandler>()
      {
        {
          PostVersionMigrationTaskHandler.PostVersionMigrationTaskType.EFolderDocTrackViewFilesToDbMigration,
          (IPostVersionMigrationTaskHandler) new EFolderDocTrackViewFilesToDbMigrationTask()
        },
        {
          PostVersionMigrationTaskHandler.PostVersionMigrationTaskType.PipelineViewXmlToDbMigration,
          (IPostVersionMigrationTaskHandler) new PipelineViewXmlToDbMigrationTask()
        }
      };
      this.pvmTaskTraceLog = DiagUtility.LogManager.GetLogger("PostVersionMigrationTask");
    }

    public void ProcessTask(ServerTask serverTask)
    {
      ClientContext clientContext = (ClientContext) null;
      PostVersionMigrationTaskHandler.PostVersionMigrationTaskData pvmData = (PostVersionMigrationTaskHandler.PostVersionMigrationTaskData) null;
      try
      {
        clientContext = ClientContext.GetCurrent();
        Stopwatch stopwatch = Stopwatch.StartNew();
        pvmData = JsonConvert.DeserializeObject<PostVersionMigrationTaskHandler.PostVersionMigrationTaskData>(serverTask?.Data);
        if (!this._taskHandlersMap.ContainsKey(pvmData.Type))
          throw new NotSupportedException(string.Format("This type {0} does not have a valid handler mapping", (object) pvmData.Type.ToString()));
        IPostVersionMigrationTaskHandler taskHandlers = this._taskHandlersMap[pvmData.Type];
        DateTime now = DateTime.Now;
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, string.Format("Started : {0} PostVersionMigrationTask processing for instance {1} with {2} minute timeout period at:{3}", (object) pvmData?.Type, (object) clientContext?.InstanceName, (object) pvmData?.TimeOut?.PeriodInMins, (object) now));
        double num1;
        if (pvmData != null)
        {
          PostVersionMigrationTaskHandler.TimeOutSettings timeOut = pvmData.TimeOut;
          int num2;
          if (timeOut == null)
          {
            num2 = 0;
          }
          else
          {
            int periodInMins = timeOut.PeriodInMins;
            num2 = 1;
          }
          if (num2 != 0)
          {
            num1 = TimeSpan.FromMinutes((double) pvmData.TimeOut.PeriodInMins).TotalMinutes;
            goto label_10;
          }
        }
        num1 = 180.0;
label_10:
        double num3 = num1;
        while (stopwatch.Elapsed.TotalMinutes <= num3 && taskHandlers.HasPendingBatches())
          taskHandlers.ExecuteBatch(pvmData?.Arguments?.ToString());
        if (taskHandlers.HasPendingBatches())
        {
          PostVersionMigrationTaskHandler.RescheduleTask(serverTask.TaskType, pvmData, now.Date);
          this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, string.Format("Timeout : {0} PostVersionMigrationTask for instance {1} in {2}", (object) pvmData?.Type, (object) clientContext?.InstanceName, (object) stopwatch.Elapsed));
        }
        this.Log(Encompass.Diagnostics.Logging.LogLevel.INFO, string.Format("Finished : {0} PostVersionMigrationTask completed successfully for instance {1}", (object) pvmData?.Type, (object) clientContext?.InstanceName));
      }
      catch (Exception ex)
      {
        this.Log(Encompass.Diagnostics.Logging.LogLevel.ERROR, string.Format("Error : {0} PostVersionMigrationTask failed for instance {1} : Stack Trace : {2}", (object) pvmData?.Type, (object) clientContext?.InstanceName, (object) ex));
        throw ex;
      }
    }

    private static void RescheduleTask(
      ServerTaskType serverTaskType,
      PostVersionMigrationTaskHandler.PostVersionMigrationTaskData pvmData,
      DateTime taskStartDate)
    {
      EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
      DbTableInfo table = EllieMae.EMLite.Server.DbAccessManager.GetTable("ServerTaskSchedule");
      DbValueList values = new DbValueList()
      {
        new DbValue("Description", (object) ("Post Version Migration | " + (object) pvmData.Type)),
        new DbValue("TaskType", (object) serverTaskType.ToString()),
        new DbValue("SystemTask", (object) 1),
        new DbValue("StartTime", (object) PostVersionMigrationTaskHandler.getNextScheduleForTimeoutTask(taskStartDate, pvmData?.TimeOut?.OnTimeOut)),
        new DbValue("IntervalUnit", (object) "OnlyOnce"),
        new DbValue("IntervalCount", (object) 1),
        new DbValue("Enabled", (object) 1),
        new DbValue("Data", (object) JsonConvert.SerializeObject((object) pvmData))
      };
      dbQueryBuilder.InsertInto(table, values, true, false);
      dbQueryBuilder.ExecuteNonQuery();
    }

    private static DateTime getNextScheduleForTimeoutTask(
      DateTime taskStartDate,
      PostVersionMigrationTaskHandler.OnTimeOut onTimeOut)
    {
      DateTime dateTime = onTimeOut == null || onTimeOut.RescheduleAfterDays <= 0 ? taskStartDate.AddDays(1.0) : taskStartDate.AddDays((double) onTimeOut.RescheduleAfterDays);
      TimeSpan? rescheduleAtTime = onTimeOut?.RescheduleAtTime;
      TimeSpan zero = TimeSpan.Zero;
      return (rescheduleAtTime.HasValue ? (rescheduleAtTime.GetValueOrDefault() > zero ? 1 : 0) : 0) == 0 ? dateTime.Add(new TimeSpan(22, 0, 0)) : dateTime.Add(onTimeOut.RescheduleAtTime);
    }

    public void Log(Encompass.Diagnostics.Logging.LogLevel LogLevel, string message)
    {
      using (ClientContext.GetCurrent().MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
        this.pvmTaskTraceLog.Write(LogLevel, nameof (PostVersionMigrationTaskHandler), message);
    }

    internal class PostVersionMigrationTaskData
    {
      [JsonRequired]
      [JsonConverter(typeof (StringEnumConverter))]
      public PostVersionMigrationTaskHandler.PostVersionMigrationTaskType Type { get; set; }

      public PostVersionMigrationTaskHandler.TimeOutSettings TimeOut { get; set; }

      public JRaw Arguments { get; set; }
    }

    internal class TimeOutSettings
    {
      public int PeriodInMins { get; set; }

      public PostVersionMigrationTaskHandler.OnTimeOut OnTimeOut { get; set; }
    }

    internal class OnTimeOut
    {
      public int RescheduleAfterDays { get; set; }

      public TimeSpan RescheduleAtTime { get; set; }
    }

    internal enum PostVersionMigrationTaskType
    {
      EFolderDocTrackViewFilesToDbMigration,
      PipelineViewXmlToDbMigration,
    }
  }
}
