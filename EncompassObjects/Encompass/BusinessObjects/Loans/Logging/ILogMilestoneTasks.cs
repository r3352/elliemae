// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogMilestoneTasks
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  /// <summary>Interface for LogReceivedFaxes class.</summary>
  /// <exclude />
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
