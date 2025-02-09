// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.LockExtensionPriceAdjustment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Represents a single price adjustment record in the Lock Extension Price Adjustment settngs table.
  /// </summary>
  public class LockExtensionPriceAdjustment
  {
    private EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment adjustment;

    internal LockExtensionPriceAdjustment(EllieMae.EMLite.ClientServer.LockExtensionPriceAdjustment adjustment)
    {
      this.adjustment = adjustment;
    }

    /// <summary>
    /// Gets the number of days by which to extend the lock request.
    /// </summary>
    public int DaysToExtend => this.adjustment.DaysToExtend;

    /// <summary>
    /// Gets the price adjustment applied to the lock request when the extension is made.
    /// </summary>
    public Decimal PriceAdjustment => this.adjustment.PriceAdjustment;
  }
}
