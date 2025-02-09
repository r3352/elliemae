// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterDelivery
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>CorrespondentMasterDelivery Class</summary>
  public class CorrespondentMasterDelivery
  {
    /// <summary>Gets the Delivery Type.</summary>
    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    /// <summary>Gets the Delivery Days.</summary>
    public int DeliveryDays { get; set; }

    /// <summary>Gets the Tolerance.</summary>
    public Decimal Tolerance { get; set; }

    /// <summary>Gets the Delivery Effective Date.</summary>
    public DateTime EffectiveDate { get; set; }

    /// <summary>Gets the Delivery Expire Date.</summary>
    public DateTime ExpireDate { get; set; }
  }
}
