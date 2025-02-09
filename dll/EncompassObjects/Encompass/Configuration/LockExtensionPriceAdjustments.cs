// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LockExtensionPriceAdjustments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  public class LockExtensionPriceAdjustments : IEnumerable
  {
    private List<LockExtensionPriceAdjustment> adjustments = new List<LockExtensionPriceAdjustment>();

    internal LockExtensionPriceAdjustments(LockExtensionPriceAdjustment[] adjustmentObjects)
    {
      foreach (LockExtensionPriceAdjustment adjustmentObject in adjustmentObjects)
        this.adjustments.Add(new LockExtensionPriceAdjustment(adjustmentObject));
    }

    public int Count => this.adjustments.Count;

    public LockExtensionPriceAdjustment this[int index] => this.adjustments[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.adjustments.GetEnumerator();
  }
}
