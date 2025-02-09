// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskSet
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a saved Task Set template which can be applied to a loan.
  /// </summary>
  public class TaskSet : Template, ITaskSet
  {
    private TaskSetTemplate template;

    internal TaskSet(Session session, TemplateEntry fsEntry, TaskSetTemplate template)
      : base(session, fsEntry)
    {
      this.template = template;
    }

    /// <summary>
    /// Returns the type of template represented by the object.
    /// </summary>
    public override TemplateType TemplateType => TemplateType.TaskSet;

    /// <summary>Gets the description of the template.</summary>
    public override string Description => this.template.Description;

    /// <summary>Retrieves all documents for the specified milestone.</summary>
    /// <param name="ms">The <see cref="T:EllieMae.EMLite.Common.Milestone" /></param>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.DocumentTemplateList" /> containing the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects which are assigned to the specified milestone.</returns>
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

    /// <summary>
    /// Retrieves all documents which are part of the Document Set.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.DocumentTemplateList" /> containing the
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.DocumentTemplate" /> objects which are part of the document set.</returns>
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

    /// <summary>
    /// Unwraps the template object to return the internal data type.
    /// </summary>
    internal override object Unwrap() => (object) this.template;
  }
}
