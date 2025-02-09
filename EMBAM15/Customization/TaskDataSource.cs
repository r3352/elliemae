// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Customization.TaskDataSource
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Customization
{
  public class TaskDataSource(LoanData loan, UserInfo currentUser, bool readOnly) : 
    LoanDataSource(loan, currentUser, readOnly),
    ITaskDataSource
  {
    public bool Exists(string taskName) => this.getTaskByName(taskName) != null;

    public bool IsComplete(string taskName)
    {
      MilestoneTaskLog taskByName = this.getTaskByName(taskName);
      return taskByName != null && taskByName.Completed;
    }

    public bool SetComplete(string taskName) => this.SetComplete(taskName, true);

    public bool SetComplete(string taskName, bool isComplete)
    {
      this.EnsureWritable();
      MilestoneTaskLog taskByName = this.getTaskByName(taskName);
      if (taskByName == null)
      {
        this.Loan.RefreshRequiredTaskRule(taskName);
        taskByName = this.getTaskByName(taskName);
        if (taskByName == null)
          return false;
      }
      if (taskByName.Completed && !isComplete)
        taskByName.UnmarkAsCompleted();
      else if (!taskByName.Completed & isComplete)
        taskByName.MarkAsCompleted(DateTime.Now, this.CurrentUser);
      return true;
    }

    private MilestoneTaskLog getTaskByName(string taskName)
    {
      LogList logList = this.Loan.GetLogList();
      MilestoneLog currentMilestone = logList.GetCurrentMilestone();
      if (currentMilestone != null)
      {
        foreach (MilestoneTaskLog milestoneTaskLog in logList.GetAllMilestoneTaskLogs(currentMilestone.Stage))
        {
          if (string.Compare(milestoneTaskLog.TaskName, taskName, true) == 0)
            return milestoneTaskLog;
        }
      }
      foreach (MilestoneTaskLog taskByName in logList.GetAllRecordsOfType(typeof (MilestoneTaskLog)))
      {
        if (string.Compare(taskByName.TaskName, taskName, true) == 0)
          return taskByName;
      }
      return (MilestoneTaskLog) null;
    }
  }
}
