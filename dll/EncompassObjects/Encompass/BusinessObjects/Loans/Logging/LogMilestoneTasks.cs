// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.LogMilestoneTasks
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Templates;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  public class LogMilestoneTasks : LoanLogEntryCollection, ILogMilestoneTasks, IEnumerable
  {
    internal LogMilestoneTasks(Loan loan)
      : base(loan, typeof (MilestoneTaskLog))
    {
    }

    public MilestoneTask this[int index] => (MilestoneTask) this.LogEntries[index];

    public MilestoneTask Add(string taskName, MilestoneEvent msEvent)
    {
      if ((taskName ?? "") == "")
        throw new ArgumentException(nameof (taskName));
      if (msEvent == null)
        throw new ArgumentNullException(nameof (msEvent));
      TaskTemplate templateByName = this.Loan.Session.Loans.Templates.Tasks.GetTemplateByName(taskName);
      MilestoneTaskLog record = new MilestoneTaskLog(this.Loan.Session.GetCurrentUser().Unwrap(), taskName, "");
      record.Stage = msEvent.InternalName;
      if (templateByName != null)
        record.TaskGUID = templateByName.ID;
      return (MilestoneTask) this.CreateEntry((LogRecordBase) record);
    }

    public void Remove(MilestoneTask taskToRemove) => this.RemoveEntry((LogEntry) taskToRemove);

    public LogEntryList GetTasksForMilestone(MilestoneEvent msEvent)
    {
      if (msEvent == null)
        throw new ArgumentNullException(nameof (msEvent));
      LogEntryList tasksForMilestone = new LogEntryList();
      foreach (MilestoneTask milestoneTask in (LoanLogEntryCollection) this)
      {
        if (milestoneTask.MilestoneEvent != null && milestoneTask.MilestoneEvent.InternalName == msEvent.InternalName)
          tasksForMilestone.Add((LogEntry) milestoneTask);
      }
      return tasksForMilestone;
    }

    public LogEntryList GetTasksByName(string taskName)
    {
      if ((taskName ?? "") == "")
        throw new ArgumentException(nameof (taskName));
      LogEntryList tasksByName = new LogEntryList();
      foreach (MilestoneTask milestoneTask in (LoanLogEntryCollection) this)
      {
        if (string.Compare(milestoneTask.Name, taskName, true) == 0)
          tasksByName.Add((LogEntry) milestoneTask);
      }
      return tasksByName;
    }

    internal override LogEntry Wrap(LogRecordBase logRecord)
    {
      return (LogEntry) new MilestoneTask(this.Loan, (MilestoneTaskLog) logRecord);
    }
  }
}
