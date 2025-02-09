// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerTasks.ServerTaskScheduleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ServerTasks
{
  [Serializable]
  public class ServerTaskScheduleInfo
  {
    private int scheduleId = -1;
    private string description;
    private string ownerId;
    private ServerTaskType taskType;
    private bool isSystemTask;
    private DateTime startTime = DateTime.Now;
    private TaskFrequencyUnit frequencyUnit;
    private int frequencyCount;
    private TaskDaysOfWeek daysOfWeek = TaskDaysOfWeek.All;
    private bool enabled = true;
    private DateTime lastSuccessTime = DateTime.MinValue;
    private ServerTaskStatus taskStatus;
    private DateTime lastExecutionTime;
    private string data = "";
    private bool faultTolerance;

    public ServerTaskScheduleInfo(string description, ServerTaskType taskType)
    {
      this.description = description;
      this.taskType = taskType;
    }

    public ServerTaskScheduleInfo(string ownerId, string description, ServerTaskType taskType)
    {
      this.ownerId = ownerId;
      this.description = description;
      this.taskType = taskType;
    }

    public ServerTaskScheduleInfo(
      int scheduleId,
      string ownerId,
      string description,
      ServerTaskType taskType,
      bool isSystemTask,
      DateTime startTime,
      TaskFrequencyUnit frequencyUnit,
      int frequencyCount,
      TaskDaysOfWeek daysOfWeek,
      bool enabled,
      DateTime lastSuccessTime,
      ServerTaskStatus taskStatus,
      DateTime lastExecutionTime,
      bool faultTolerance,
      string data)
    {
      this.scheduleId = scheduleId;
      this.ownerId = ownerId;
      this.description = description;
      this.taskType = taskType;
      this.isSystemTask = isSystemTask;
      this.startTime = startTime;
      this.frequencyUnit = frequencyUnit;
      this.frequencyCount = frequencyCount;
      this.daysOfWeek = daysOfWeek;
      this.enabled = enabled;
      this.lastSuccessTime = lastSuccessTime;
      this.taskStatus = taskStatus;
      this.lastExecutionTime = lastExecutionTime;
      this.faultTolerance = faultTolerance;
      this.data = data;
    }

    public int ScheduleID => this.scheduleId;

    public string OwnerID => this.ownerId;

    public ServerTaskType TaskType => this.taskType;

    public ServerTaskStatus Status => this.taskStatus;

    public DateTime LastSuccessfulExecution => this.lastSuccessTime;

    public DateTime LastExecutionTime => this.lastExecutionTime;

    public string Data => this.data;

    public bool FaultTolerance => this.faultTolerance;

    public string Description
    {
      get => this.description;
      set => this.description = value;
    }

    public bool IsSystemTask
    {
      get => this.isSystemTask;
      set => this.isSystemTask = value;
    }

    public DateTime StartTime
    {
      get => this.startTime;
      set => this.startTime = value;
    }

    public TaskFrequencyUnit FrequencyUnit
    {
      get => this.frequencyUnit;
      set => this.frequencyUnit = value;
    }

    public int FrequencyCount
    {
      get => this.frequencyCount;
      set => this.frequencyCount = value;
    }

    public TaskDaysOfWeek DaysOfWeek
    {
      get => this.daysOfWeek;
      set => this.daysOfWeek = value;
    }

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = false;
    }

    public DateTime GetNextExecutionTime(DateTime priorExecutionTime)
    {
      if (this.frequencyCount == 0)
        throw new InvalidOperationException("Cannot compute execution time when Frequency Count = 0");
      if (priorExecutionTime == DateTime.MinValue)
        return this.startTime;
      int frequencyIntervalCount1 = ServerTaskScheduleInfo.GetFrequencyIntervalCount(this.startTime, priorExecutionTime, this.frequencyUnit);
      if (frequencyIntervalCount1 < 0)
        return this.startTime;
      DateTime startTime = ServerTaskScheduleInfo.AddFrequencyInterval(this.startTime, this.frequencyUnit, frequencyIntervalCount1 - frequencyIntervalCount1 % this.frequencyCount);
      if (startTime <= priorExecutionTime)
        startTime = ServerTaskScheduleInfo.AddFrequencyInterval(startTime, this.frequencyUnit, this.frequencyCount);
      DayOfWeek dayOfWeek = startTime.DayOfWeek;
      if (this.frequencyUnit < TaskFrequencyUnit.Week)
      {
        while (!this.isAllowedDay(startTime.DayOfWeek))
        {
          int frequencyIntervalCount2 = ServerTaskScheduleInfo.GetFrequencyIntervalCount(startTime, startTime.Date.AddDays(1.0), this.frequencyUnit);
          startTime = ServerTaskScheduleInfo.AddFrequencyInterval(startTime, this.frequencyUnit, frequencyIntervalCount2 - frequencyIntervalCount2 % this.frequencyCount);
          if (!this.isAllowedDay(startTime.DayOfWeek))
            startTime = ServerTaskScheduleInfo.AddFrequencyInterval(startTime, this.frequencyUnit, this.frequencyCount);
        }
      }
      return startTime;
    }

    private bool isAllowedDay(DayOfWeek dayOfWeek)
    {
      if (this.daysOfWeek == TaskDaysOfWeek.None)
        return true;
      TaskDaysOfWeek taskDaysOfWeek = TaskDaysOfWeekNameProvider.DayOfWeekToTaskDaysOfWeek(dayOfWeek);
      return (this.daysOfWeek & taskDaysOfWeek) == taskDaysOfWeek;
    }

    public static int GetFrequencyIntervalCount(
      DateTime startTime,
      DateTime endTime,
      TaskFrequencyUnit unit)
    {
      TimeSpan timeSpan = endTime - startTime;
      switch (unit)
      {
        case TaskFrequencyUnit.Minute:
          return Convert.ToInt32(Math.Floor(timeSpan.TotalMinutes));
        case TaskFrequencyUnit.Hour:
          return Convert.ToInt32(Math.Floor(timeSpan.TotalHours));
        case TaskFrequencyUnit.Day:
          return Convert.ToInt32(Math.Floor(timeSpan.TotalDays));
        case TaskFrequencyUnit.Week:
          return Convert.ToInt32(Math.Floor(timeSpan.TotalDays)) / 7;
        case TaskFrequencyUnit.Month:
          return 12 * endTime.Year + endTime.Month - (12 * startTime.Year + startTime.Month);
        default:
          throw new Exception("Invalid frequency interval specified");
      }
    }

    public static DateTime AddFrequencyInterval(
      DateTime startTime,
      TaskFrequencyUnit unit,
      int count)
    {
      switch (unit)
      {
        case TaskFrequencyUnit.Minute:
          return startTime.AddMinutes((double) count);
        case TaskFrequencyUnit.Hour:
          return startTime.AddHours((double) count);
        case TaskFrequencyUnit.Day:
          return startTime.AddDays((double) count);
        case TaskFrequencyUnit.Week:
          return startTime.AddDays((double) (count * 7));
        case TaskFrequencyUnit.Month:
          return startTime.AddMonths(count);
        default:
          throw new Exception("Invalid frequency interval specified");
      }
    }
  }
}
