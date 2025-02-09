// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.SrpStateAdjustment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents a SRP Adjustments Based on Geography.</summary>
  /// <remarks>SrpStateAdjustment allows correspondent buyers to setup SRP adjustments based on state.</remarks>
  public class SrpStateAdjustment
  {
    /// <summary>Gets or sets State full name.</summary>
    /// <remarks>If SRP adjustments are applied to all states, set this property to "All States".</remarks>
    public string StateFullName { get; set; }

    /// <summary>Gets or sets SRP Adjustment</summary>
    public Decimal SrpAdjustment { get; set; }

    /// <summary>Gets or sets SRP Adjustment, If impounds are Waived</summary>
    public Decimal SrpIfWaived { get; set; }
  }
}
