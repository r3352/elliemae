// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ServerTasks.TaskDaysOfWeekNameProvider
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.ServerTasks
{
  public class TaskDaysOfWeekNameProvider : DefaultEnumNameProvider
  {
    public TaskDaysOfWeekNameProvider()
      : base(typeof (TaskDaysOfWeek))
    {
    }

    public static TaskDaysOfWeek DayOfWeekToTaskDaysOfWeek(DayOfWeek dayOfWeek)
    {
      switch (dayOfWeek)
      {
        case DayOfWeek.Sunday:
          return TaskDaysOfWeek.Sunday;
        case DayOfWeek.Monday:
          return TaskDaysOfWeek.Monday;
        case DayOfWeek.Tuesday:
          return TaskDaysOfWeek.Tuesday;
        case DayOfWeek.Wednesday:
          return TaskDaysOfWeek.Wednesday;
        case DayOfWeek.Thursday:
          return TaskDaysOfWeek.Thursday;
        case DayOfWeek.Friday:
          return TaskDaysOfWeek.Friday;
        case DayOfWeek.Saturday:
          return TaskDaysOfWeek.Saturday;
        default:
          throw new Exception("Invalid DayOfWeek value");
      }
    }
  }
}
