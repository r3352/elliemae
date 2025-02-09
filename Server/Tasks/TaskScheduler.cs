// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Tasks.TaskScheduler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.Cache;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.Common;
using System;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.Server.Tasks
{
  public class TaskScheduler
  {
    private const string className = "TaskScheduler�";
    private static Timer scheduleTimer = (Timer) null;
    private static int checkStuckedJobCounter = 60;
    private static readonly object obj = new object();

    public static void Start()
    {
      if (TaskScheduler.scheduleTimer != null)
        return;
      TaskScheduler.scheduleTimer = new Timer(new TimerCallback(TaskScheduler.checkAllSchedules), (object) null, ServerGlobals.TaskSchedulerInterval, ServerGlobals.TaskSchedulerInterval);
    }

    private static void checkAllSchedules(object notUsed)
    {
      try
      {
        lock (TaskScheduler.obj)
        {
          if (TaskScheduler.checkStuckedJobCounter <= 0)
            TaskScheduler.checkStuckedJobCounter = 60;
          if (EncompassServer.IsRunningInProcess)
          {
            TaskScheduler.checkInstanceTaskSchedule(EnConfigurationSettings.InstanceName);
          }
          else
          {
            foreach (string allInstanceName in EnGlobalSettings.GetAllInstanceNames(true))
              TaskScheduler.checkInstanceTaskSchedule(allInstanceName);
          }
          --TaskScheduler.checkStuckedJobCounter;
        }
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (TaskScheduler), "Error processing server task schedules: " + (object) ex);
      }
    }

    private static void checkInstanceTaskSchedule(string instanceName)
    {
      try
      {
        ClientContext clientContext = ClientContext.Open(instanceName);
        if (clientContext.Settings.TaskSchedulerDisabled)
          return;
        using (clientContext.MakeCurrent((IDataCache) null, (string) null, new Guid?(), new bool?()))
          TaskScheduler.checkTaskSchedule();
      }
      catch (Exception ex)
      {
        ServerGlobals.TraceLog.WriteError(nameof (TaskScheduler), "Error processing server task schedule for instance '" + instanceName + "': " + (object) ex);
      }
    }

    private static void checkTaskSchedule()
    {
      ClientContext current = ClientContext.GetCurrent();
      current.TaskQueue.DeleteStuckTasks();
      bool flag = current.TaskQueue.RequeueOrphanedTasks();
      foreach (ServerTaskScheduleInfo taskSchedule in TaskScheduleAccessor.GetTaskSchedules(true, true))
      {
        if (TaskScheduler.checkStuckedJobCounter == 60 && taskSchedule.Status == ServerTaskStatus.Running && (DateTime.Now - (taskSchedule.FaultTolerance ? taskSchedule.LastExecutionTime : taskSchedule.LastSuccessfulExecution)).Hours >= 4)
          TraceLog.WriteWarning(nameof (TaskScheduler), "EncompassScheduledTask - " + taskSchedule.Description + " Schedule task is running for more than 4 Hours. ScheduleID: " + (object) taskSchedule.ScheduleID);
        if (taskSchedule.Status == ServerTaskStatus.None && taskSchedule.GetNextExecutionTime(taskSchedule.FaultTolerance ? taskSchedule.LastExecutionTime : taskSchedule.LastSuccessfulExecution) < DateTime.Now)
        {
          if (taskSchedule != null && !string.IsNullOrWhiteSpace(taskSchedule.Data))
          {
            string[] strArray = taskSchedule.Data.Split('|');
            if (strArray.Length == 2 && !TaskScheduler.IsTimeOfDayBetween(DateTime.Now, new TimeSpan(Convert.ToInt32(strArray[0]), 0, 0), new TimeSpan(Convert.ToInt32(strArray[1]), 59, 59)))
              continue;
          }
          int taskForSchedule = TaskScheduleAccessor.CreateTaskForSchedule(taskSchedule.ScheduleID);
          if (taskForSchedule >= 0)
          {
            flag = true;
            TraceLog.WriteInfo(nameof (TaskScheduler), "Schedule task '" + taskSchedule.Description + "' activated. Queue task " + (object) taskForSchedule + " for processing.");
          }
        }
      }
      if (!flag)
        return;
      current.TaskQueue.CheckForQueuedItems();
    }

    private static bool IsTimeOfDayBetween(DateTime time, TimeSpan startTime, TimeSpan endTime)
    {
      if (endTime == startTime)
        return true;
      return endTime < startTime ? time.TimeOfDay <= endTime || time.TimeOfDay >= startTime : time.TimeOfDay >= startTime && time.TimeOfDay <= endTime;
    }
  }
}
