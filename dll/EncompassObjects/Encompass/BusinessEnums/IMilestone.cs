// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.IMilestone
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  [Guid("AE5B37C2-54E2-4b41-94AE-9B77B5DBFC0B")]
  public interface IMilestone
  {
    int ID { get; }

    string Name { get; }

    Milestone Next { get; }

    Milestone Previous { get; }

    string ToString();

    bool Equals(object o);

    bool OccursBefore(Milestone ms);

    bool OccursOnOrBefore(Milestone ms);

    bool OccursAfter(Milestone ms);

    bool OccursOnOrAfter(Milestone ms);

    bool IsCustom { get; }

    Milestone CoreMilestone { get; }
  }
}
