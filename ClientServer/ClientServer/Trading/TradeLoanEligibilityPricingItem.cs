// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Trading.TradeLoanEligibilityPricingItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Trading
{
  [Serializable]
  public class TradeLoanEligibilityPricingItem
  {
    public int EligibilityPricingItemId { get; set; }

    public int EligibilityPricingId { get; set; }

    public int Order { get; set; }

    public string Description { get; set; }

    public Decimal Rate { get; set; }

    public Decimal Price { get; set; }

    public int Admin { get; set; }

    public TradeLoanEligibilityType Type { get; set; }

    public string TypeText { get; set; }
  }
}
