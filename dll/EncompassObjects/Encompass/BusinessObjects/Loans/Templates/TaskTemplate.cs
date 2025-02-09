// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TaskTemplate : ITaskTemplate
  {
    private MilestoneTaskDefinition taskDef;

    internal TaskTemplate(MilestoneTaskDefinition taskDef) => this.taskDef = taskDef;

    public string ID => this.taskDef.TaskGUID;

    public string Name => this.taskDef.TaskName;

    public int DaysToComplete => this.taskDef.DaysToComplete;

    public string Description => this.taskDef.TaskDescription;

    public TaskPriority Priority
    {
      get
      {
        return (TaskPriority) Enum.Parse(typeof (TaskPriority), this.taskDef.TaskPriority.ToString(), true);
      }
    }

    public override bool Equals(object obj)
    {
      return obj is TaskTemplate taskTemplate && taskTemplate.ID == this.ID;
    }

    public override int GetHashCode() => this.ID.GetHashCode();
  }
}
