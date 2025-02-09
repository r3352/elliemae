// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Templates.TaskTemplate
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.DataEngine.Log;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Templates
{
  /// <summary>
  /// Represents a configured task template which can be used to create a
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Loans.Logging.MilestoneTask" />.
  /// </summary>
  public class TaskTemplate : ITaskTemplate
  {
    private MilestoneTaskDefinition taskDef;

    internal TaskTemplate(MilestoneTaskDefinition taskDef) => this.taskDef = taskDef;

    /// <summary>Returns the unique identifier for the task template.</summary>
    public string ID => this.taskDef.TaskGUID;

    /// <summary>Gets the name of the task.</summary>
    public string Name => this.taskDef.TaskName;

    /// <summary>
    /// Gets the number of days from the date started until the task should be completed.
    /// </summary>
    public int DaysToComplete => this.taskDef.DaysToComplete;

    /// <summary>Gets the description of the task.</summary>
    public string Description => this.taskDef.TaskDescription;

    /// <summary>
    /// Gets the type of document represented by the template.
    /// </summary>
    public TaskPriority Priority
    {
      get
      {
        return (TaskPriority) Enum.Parse(typeof (TaskPriority), this.taskDef.TaskPriority.ToString(), true);
      }
    }

    /// <summary>Provides an equality comparer for two templates.</summary>
    /// <returns>Returns <c>true</c> if the IDs of the two templates are the same, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      return obj is TaskTemplate taskTemplate && taskTemplate.ID == this.ID;
    }

    /// <summary>Provides a hash code for the object based on the ID.</summary>
    public override int GetHashCode() => this.ID.GetHashCode();
  }
}
