// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Loans.Logging.ILogMilestoneEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Loans.Logging
{
  [Guid("660DC670-F447-43d6-86DB-5EA03768026C")]
  public interface ILogMilestoneEvents
  {
    int Count { get; }

    MilestoneEvent this[int index] { get; }

    MilestoneEvent GetEventForMilestone(string msName);

    MilestoneEvent LastCompletedEvent { get; }

    MilestoneEvent NextEvent { get; }

    IEnumerator GetEnumerator();
  }
}
