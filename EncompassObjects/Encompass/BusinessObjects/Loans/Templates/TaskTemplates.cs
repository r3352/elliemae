// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplates
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.Client;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> objects defined in the system settings.
  /// </summary>
  public class TaskTemplates : SessionBoundObject, ITaskTemplates, IEnumerable
  {
    private List<TaskTemplate> templates;

    internal TaskTemplates(Session session)
      : base(session)
    {
      this.Refresh();
    }

    /// <summary>
    /// Gets the number of <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> objects in the collection.
    /// </summary>
    public int Count => this.templates.Count;

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> at a specified index in the collection.
    /// </summary>
    /// <param name="index">The index of the requested template.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> at the specified index.</returns>
    public TaskTemplate this[int index] => this.templates[index];

    /// <summary>
    /// Retrieves a template from the collection using its unique ID.
    /// </summary>
    /// <param name="templateId">The ID of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> or, if no template matches
    /// the specified ID, returns <c>null</c>.</returns>
    public TaskTemplate GetTemplateByID(string templateId)
    {
      foreach (TaskTemplate templateById in this)
      {
        if (templateById.ID == templateId)
          return templateById;
      }
      return (TaskTemplate) null;
    }

    /// <summary>Retrieves a template from the collection by name.</summary>
    /// <param name="taskName">The title of the template to be retrieved.</param>
    /// <returns>Returns the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate" /> or, if no template matches
    /// the specified title, returns <c>null</c>. The compaison for the purposes of string matching
    /// is case insensitive.</returns>
    public TaskTemplate GetTemplateByName(string taskName)
    {
      foreach (TaskTemplate templateByName in this)
      {
        if (string.Compare(templateByName.Name, taskName, true) == 0)
          return templateByName;
      }
      return (TaskTemplate) null;
    }

    /// <summary>Refreshes the Task Template list from the server.</summary>
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

    /// <summary>Provides an enumerator for the collection.</summary>
    public IEnumerator GetEnumerator() => (IEnumerator) this.templates.GetEnumerator();
  }
}
