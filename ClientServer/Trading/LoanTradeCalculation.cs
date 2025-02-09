// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeCalculation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeCalculation(ITradeInfoObject trade) : TradeCalculation(trade)
  {
    private LoanTradeInfo Trade
    {
      get => base.Trade != null ? (LoanTradeInfo) base.Trade : (LoanTradeInfo) null;
    }

    public static Decimal CalculateWeightedAvgBulkPrice(
      Decimal totalPrice,
      Decimal totalAmount,
      LoanTradeInfo tradeInfo)
    {
      Decimal weightedAvgBulkPrice = 0M;
      if (totalAmount > 0M && tradeInfo.IsBulkDelivery && !tradeInfo.IsWeightedAvgBulkPriceLocked)
        weightedAvgBulkPrice = totalPrice / totalAmount;
      return weightedAvgBulkPrice;
    }
  }
}
