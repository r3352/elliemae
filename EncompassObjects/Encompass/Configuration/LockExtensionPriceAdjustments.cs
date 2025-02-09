// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LockExtensionPriceAdjustments
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Represents the collection of Price Adjustment options in the lock extension settings
  /// </summary>
  public class LockExtensionPriceAdjustments : IEnumerable
  {
    private List<LockExtensionPriceAdjustment> adjustments = new List<LockExtensionPriceAdjustment>();

    internal LockExtensionPriceAdjustments(EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment[] adjustmentObjects)
    {
      foreach (EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment adjustmentObject in adjustmentObjects)
        this.adjustments.Add(new LockExtensionPriceAdjustment(adjustmentObject));
    }

    /// <summary>Gets the number of adjustments in the collection</summary>
    public int Count => this.adjustments.Count;

    /// <summary>
    /// Returns the <see cref="T:EllieMae.Encompass.Configuration.LockExtensionPriceAdjustment" /> at the specified index.
    /// </summary>
    /// <param name="index">The index of the specified item in the collection</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.Configuration.LockExtensionPriceAdjustment" /> object at the
    /// specified index.</returns>
    public LockExtensionPriceAdjustment this[int index] => this.adjustments[index];

    /// <summary>
    /// Provides an enumeration of the PriceAdjustments in the collection
    /// </summary>
    public IEnumerator GetEnumerator() => (IEnumerator) this.adjustments.GetEnumerator();
  }
}
