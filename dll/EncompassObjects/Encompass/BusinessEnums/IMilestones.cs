// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IMilestones
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("268EC9D6-E5F6-4fb8-9D44-AF24D687BB0F")]
  public interface IMilestones
  {
    Milestone this[int index] { get; }

    Milestone GetItemByID(int itemId);

    Milestone GetItemByName(string itemName);

    int Count { get; }

    IEnumerator GetEnumerator();

    Milestone First { get; }

    Milestone Started { get; }

    Milestone Processing { get; }

    Milestone Submittal { get; }

    Milestone Approval { get; }

    Milestone DocsSigning { get; }

    Milestone Funding { get; }

    Milestone Completion { get; }
  }
}
