// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeCalculation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SecurityTradeCalculation : TradeCalculation
  {
    internal SecurityTradeCalculation(ITradeInfoObject trade)
      : base(trade)
    {
    }

    private SecurityTradeInfo Trade
    {
      get => base.Trade != null ? (SecurityTradeInfo) base.Trade : (SecurityTradeInfo) null;
    }

    public Decimal CalculatePairOffAmount()
    {
      return SecurityTradeCalculation.CalculatePairOffAmount(this.Trade.PairOffs);
    }

    public static Decimal CalculatePairOffAmount(TradePairOffs pairOffs)
    {
      Decimal num = 0M;
      foreach (PairOff pairOff in pairOffs)
        num += pairOff.UndeliveredAmount;
      return num * -1M;
    }

    public Decimal CalculatePairOffGainLossAmount()
    {
      return SecurityTradeCalculation.CalculatePairOffGainLossAmount(this.Trade.Price, this.Trade.PairOffs);
    }

    public static Decimal CalculatePairOffGainLossAmount(Decimal tradePrice, TradePairOffs pairOffs)
    {
      Decimal offGainLossAmount = 0M;
      foreach (PairOff pairOff in pairOffs)
        offGainLossAmount += SecurityTradeCalculation.CalculatePairOffGainLoss(tradePrice, pairOff.Fee, pairOff.UndeliveredAmount);
      return offGainLossAmount;
    }

    public static Decimal CalculateAssignedAmount(SecurityTradeAssignment[] assignments)
    {
      Decimal assignedAmount = 0M;
      if (assignments == null)
        return assignedAmount;
      foreach (SecurityTradeAssignment assignment in assignments)
        assignedAmount += assignment.AssigneeTrade.TradeAmount;
      return assignedAmount;
    }

    public static Decimal CalculateNetProfitAmount(SecurityTradeAssignment[] assignments)
    {
      Decimal netProfitAmount = 0M;
      if (assignments == null)
        return netProfitAmount;
      foreach (SecurityTradeAssignment assignment in assignments)
        netProfitAmount += assignment.AssigneeTrade.NetProfit;
      return netProfitAmount;
    }

    public Decimal CalculateOpenAmount(
      SecurityTradeAssignment[] assignments,
      MbsPoolAssignment[] mbsPoolAssignments)
    {
      return SecurityTradeCalculation.CalculateOpenAmount(this.Trade.TradeAmount, this.Trade.PairOffs, assignments, mbsPoolAssignments);
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      TradePairOffs pairOffs,
      SecurityTradeAssignment[] assignments,
      MbsPoolAssignment[] mbsPoolAssignments)
    {
      Decimal assignedAmount = SecurityTradeCalculation.CalculateAssignedAmount(assignments);
      Decimal pairOffAmount = SecurityTradeCalculation.CalculatePairOffAmount(pairOffs);
      Decimal allocatedAmount = SecurityTradeCalculation.CalculateAllocatedAmount(mbsPoolAssignments);
      return SecurityTradeCalculation.CalculateOpenAmount(tradeAmount, assignedAmount, pairOffAmount, allocatedAmount);
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      Decimal assignedAmount,
      Decimal pairAmount,
      Decimal mbsAllocatedPoolAmount)
    {
      return !(pairAmount > 0M) ? tradeAmount - assignedAmount + pairAmount - mbsAllocatedPoolAmount : tradeAmount - assignedAmount - pairAmount - mbsAllocatedPoolAmount;
    }

    public Decimal CalculateTotalGainLossAmount(SecurityTradeAssignment[] assignments)
    {
      return SecurityTradeCalculation.CalculateTotalGainLossAmount(this.Trade.Price, this.Trade.PairOffs, assignments);
    }

    public static Decimal CalculateTotalGainLossAmount(
      Decimal tradePrice,
      TradePairOffs pairOffs,
      SecurityTradeAssignment[] assignments)
    {
      Decimal offGainLossAmount = SecurityTradeCalculation.CalculatePairOffGainLossAmount(tradePrice, pairOffs);
      return SecurityTradeCalculation.CalculateTotalGainLossAmount(SecurityTradeCalculation.CalculateNetProfitAmount(assignments), offGainLossAmount);
    }

    public static Decimal CalculateTotalGainLossAmount(
      Decimal netProfitAmount,
      Decimal pairOffGainLossAmount)
    {
      return pairOffGainLossAmount;
    }

    public static Decimal CalculatePairOffGainLoss(
      Decimal securityTradePrice,
      Decimal pairOffBuyPrice,
      Decimal pairOffAmount)
    {
      return (securityTradePrice - pairOffBuyPrice) * pairOffAmount / 100M;
    }

    public static Decimal CalculateAllocatedAmount(MbsPoolAssignment[] assignments)
    {
      return MbsPoolCalculation.CalculateAllocatedAmount(assignments);
    }

    public Decimal CalculateOpenAmountFromPoolAssignment(MbsPoolAssignment[] assignments)
    {
      return SecurityTradeCalculation.CalculateOpenAmountFromPoolAssignment(this.Trade.TradeAmount, assignments);
    }

    public static Decimal CalculateOpenAmountFromPoolAssignment(
      Decimal tradeAmount,
      MbsPoolAssignment[] assignments)
    {
      Decimal allocatedAmount = SecurityTradeCalculation.CalculateAllocatedAmount(assignments);
      return SecurityTradeCalculation.CalculateOpenAmountFromPoolAssignment(tradeAmount, allocatedAmount);
    }

    public static Decimal CalculateOpenAmountFromPoolAssignment(
      Decimal tradeAmount,
      Decimal assignedAmount)
    {
      return tradeAmount - assignedAmount;
    }

    public static Decimal CalculateCompletionPercentage(Decimal tradeAmount, Decimal openAmount)
    {
      return tradeAmount != 0M ? (tradeAmount - openAmount) / tradeAmount * 100M : 0M;
    }
  }
}
