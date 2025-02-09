// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TaskSet : Template, ITaskSet
  {
    private TaskSetTemplate template;

    internal TaskSet(Session session, TemplateEntry fsEntry, TaskSetTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    public override TemplateType TemplateType => TemplateType.TaskSet;

    public override string Description => this.template.Description;

    public TaskTemplateList GetTasksForMilestone(Milestone ms)
    {
      TaskTemplateList tasksForMilestone = new TaskTemplateList();
      foreach (string taskName in this.template.GetTasksByMilestone(ms.InternalName))
      {
        TaskTemplate templateByName = this.Session.Loans.Templates.Tasks.GetTemplateByName(taskName);
        if (templateByName != null)
          tasksForMilestone.Add(templateByName);
      }
      return tasksForMilestone;
    }

    public TaskTemplateList GetAllTasks()
    {
      TaskTemplateList allTasks = new TaskTemplateList();
      foreach (ArrayList arrayList in (IEnumerable) this.template.DocList.Values)
      {
        foreach (string taskName in arrayList)
        {
          TaskTemplate templateByName = this.Session.Loans.Templates.Tasks.GetTemplateByName(taskName);
          if (templateByName != null)
            allTasks.Add(templateByName);
        }
      }
      return allTasks;
    }

    internal override object Unwrap() => (object) this.template;
  }
}
