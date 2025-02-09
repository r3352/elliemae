// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  public class TaskTemplates : SessionBoundObject, ITaskTemplates, IEnumerable
  {
    private List<TaskTemplate> templates;

    internal TaskTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    public int Count => this.templates.Count;

    public TaskTemplate this[int index] => this.templates[index];

    public TaskTemplate GetTemplateByID(string templateId)
    {
      foreach (TaskTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (TaskTemplate) null;
    }

    public TaskTemplate GetTemplateByName(string taskName)
    {
      foreach (TaskTemplate templateByName in this)
      {
        if (string.Compare(templateByName.Name, taskName, true) == 0)
          return templateByName;
      }
      return (TaskTemplate) null;
    }

    public void Refresh()
    {
      lock (this)
      {
        MilestoneTaskDefinition[] milestoneTasks = this.Session.SessionObjects.ConfigurationManager.GetMilestoneTasks((string[]) null);
        this.templates = new List<TaskTemplate>();
        foreach (MilestoneTaskDefinition taskDef in milestoneTasks)
          this.templates.Add(new TaskTemplate(taskDef));
      }
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
