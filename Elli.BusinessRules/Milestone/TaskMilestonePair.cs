// Decompiled with JetBrains decompiler
// Type: Elli.BusinessRules.Milestone.TaskMilestonePair
// Assembly: Elli.BusinessRules, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0A206AB-C2DC-4F02-BBE4-A037D1140EF4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.BusinessRules.dll

using Elli.ElliEnum;

#nullable disable
namespace Elli.BusinessRules.Milestone
{
  public class TaskMilestonePair : MilestoneBase
  {
    private readonly string _taskGUID;
    private readonly bool _isRequired;

    public string TaskName { get; set; }

    public string TaskDescription { get; set; }

    public int DaysToComplete { get; set; }

    public int TaskPriority { get; set; }

    public MilestoneTaskType TaskType { get; set; }

    public TaskMilestonePair(
      string taskGuid,
      string milestoneId,
      bool isRequired,
      MilestoneTaskType taskType = MilestoneTaskType.MilestoneRequired)
      : base(milestoneId)
    {
      this._taskGUID = taskGuid;
      this._isRequired = isRequired;
      this.TaskType = taskType;
    }

    public TaskMilestonePair(
      string taskGuid,
      string milestoneId,
      bool isRequired,
      string taskName,
      string taskDescription,
      int daysToComplete,
      int taskPriority,
      MilestoneTaskType taskType = MilestoneTaskType.MilestoneRequired)
      : base(milestoneId)
    {
      this._taskGUID = taskGuid;
      this._isRequired = isRequired;
      this.TaskType = taskType;
      this.TaskName = taskName;
      this.TaskDescription = taskDescription;
      this.DaysToComplete = daysToComplete;
      this.TaskPriority = taskPriority;
    }

    public string TaskGUID => this._taskGUID;

    public bool IsRequired => this._isRequired;
  }
}
