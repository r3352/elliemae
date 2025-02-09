// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogMilestoneTasks
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("576A57CF-59B6-47bd-A615-4E0543339E76")]
  public interface ILogMilestoneTasks
  {
    int Count { get; }

    MilestoneTask this[int index] { get; }

    MilestoneTask Add(string taskName, MilestoneEvent msEvent);

    void Remove(MilestoneTask taskToRemove);

    IEnumerator GetEnumerator();

    LogEntryList GetTasksForMilestone(MilestoneEvent msEvent);

    LogEntryList GetTasksByName(string taskName);
  }
}
