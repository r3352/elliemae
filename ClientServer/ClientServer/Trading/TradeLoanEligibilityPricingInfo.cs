// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Trading.TradeLoanEligibilityPricingInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Trading
{
  [Serializable]
  public class TradeLoanEligibilityPricingInfo
  {
    public int EligibilityPricingId { get; set; }

    public int TradeId { get; set; }

    public string LoanGuid { get; set; }

    public int ProgramId { get; set; }

    public string LoanNumber { get; set; }

    public Decimal LoanAmount { get; set; }

    public Decimal BasePriceNoteRate { get; set; }

    public Decimal BasePrice { get; set; }

    public Decimal TotalBuyPrice { get; set; }

    public Decimal ExtendedTotalBuyPrice { get; set; }

    public bool IsEligible { get; set; }

    public string IneligiblityReason { get; set; }

    public List<TradeLoanEligibilityPricingItem> TradeLoanEligibilityPricingItems { get; set; }
  }
}
