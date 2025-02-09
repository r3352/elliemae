// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeCalculation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradeCalculation : TradeCalculation
  {
    internal CorrespondentTradeCalculation(ITradeInfoObject trade)
      : base(trade)
    {
    }

    private CorrespondentTradeInfo Trade
    {
      get
      {
        return base.Trade != null ? (CorrespondentTradeInfo) base.Trade : (CorrespondentTradeInfo) null;
      }
    }

    public static Decimal CalculatePriceIndex(PipelineInfo info, CorrespondentTradeInfo tradeInfo)
    {
      return CorrespondentTradeCalculation.CalculatePriceIndex(info, tradeInfo, 0M, true);
    }

    public static Decimal CalculatePriceIndex(
      PipelineInfo info,
      CorrespondentTradeInfo tradeInfo,
      Decimal securityPrice)
    {
      return CorrespondentTradeCalculation.CalculatePriceIndex(info, tradeInfo, securityPrice, true);
    }

    public static Decimal CalculatePriceIndex(
      PipelineInfo info,
      CorrespondentTradeInfo tradeInfo,
      Decimal securityPrice,
      bool includeSrp)
    {
      if (tradeInfo.IsForIndividualLoan())
        return Utils.ParseDecimal(info.GetField("Loan.TotalBuyPrice"));
      if (tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk || tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT)
        return securityPrice;
      Decimal priceForNoteRate = tradeInfo.GetBasePriceForNoteRate(Utils.ParseDecimal(info.GetField("Loan.LoanRate")), securityPrice);
      if (priceForNoteRate == 0M)
        return 0M;
      foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) tradeInfo.PriceAdjustments)
      {
        if (priceAdjustment.CriterionList.CreateEvaluator().Evaluate(info, FilterEvaluationOption.None))
          priceForNoteRate += priceAdjustment.PriceAdjustment;
      }
      if (includeSrp)
        priceForNoteRate += tradeInfo.SRPTable.GetAdjustmentForLoan(info);
      return priceForNoteRate;
    }

    public static Decimal CalculateProfit(
      PipelineInfo info,
      CorrespondentTradeInfo tradeInfo,
      Decimal securityPrice)
    {
      Decimal num1 = Utils.ParseDecimal(info.GetField("TotalLoanAmount"));
      Decimal num2 = Utils.ParseDecimal(info.GetField("TotalBuyPrice"));
      Decimal num3 = Utils.ParseDecimal(info.GetField("TotalSellPrice"));
      return num2 <= 0M || num1 <= 0M || num3 == 0M ? 0M : Math.Round((num3 - num2) * num1 / 100M, 2);
    }

    public static Decimal CalculateGainLoss(
      IEnumerable<PipelineInfo> loans,
      CorrespondentTradeInfo tradeInfo)
    {
      return loans.Sum<PipelineInfo>((Func<PipelineInfo, Decimal>) (pinfo => CorrespondentTradeCalculation.CalculateProfit(pinfo, tradeInfo, 0M)));
    }

    public static Decimal CalculateGainLoss(
      PipelineInfo[] loanList,
      CorrespondentTradeInfo tradeInfo)
    {
      return ((IEnumerable<PipelineInfo>) loanList).Sum<PipelineInfo>((Func<PipelineInfo, Decimal>) (pinfo => CorrespondentTradeCalculation.CalculateProfit(pinfo, tradeInfo, 0M)));
    }

    public static Decimal CalculateNetProfit(
      PipelineInfo[] loanList,
      CorrespondentTradeInfo tradeInfo,
      Decimal miscAdjustment)
    {
      return CorrespondentTradeCalculation.CalculateGainLoss(loanList, tradeInfo) - miscAdjustment;
    }

    public Decimal CalculateOpenAmount(PipelineInfo[] assignments)
    {
      return CorrespondentTradeCalculation.CalculateOpenAmount(this.Trade.TradeAmount, this.Trade.CorrespondentTradePairOffs, assignments);
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      CorrespondentTradePairOffs pairOffs,
      PipelineInfo[] assignments)
    {
      return CorrespondentTradeCalculation.CalculateOpenAmount(tradeAmount, pairOffs, ((IEnumerable<PipelineInfo>) assignments).Select<PipelineInfo, Hashtable>((Func<PipelineInfo, Hashtable>) (a => a.Info)));
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      CorrespondentTradePairOffs pairOffs,
      IEnumerable<Hashtable> assignments)
    {
      Decimal assignedAmount = CorrespondentTradeCalculation.CalculateAssignedAmount(assignments);
      Decimal pairOffAmount = CorrespondentTradeCalculation.CalculatePairOffAmount(pairOffs);
      return CorrespondentTradeCalculation.CalculateOpenAmount(tradeAmount, assignedAmount, pairOffAmount);
    }

    public static Decimal CalculateOpenAmount(
      Decimal tradeAmount,
      Decimal assignedAmount,
      Decimal pairAmount)
    {
      return !(pairAmount > 0M) ? tradeAmount - assignedAmount + pairAmount : tradeAmount - assignedAmount - pairAmount;
    }

    public Decimal CalculatePairOffAmount()
    {
      return CorrespondentTradeCalculation.CalculatePairOffAmount(this.Trade.CorrespondentTradePairOffs);
    }

    public static Decimal CalculatePairOffAmount(CorrespondentTradePairOffs pairOffs)
    {
      Decimal num = 0M;
      foreach (CorrespondentTradePairOff pairOff in pairOffs)
        num += pairOff.TradeAmount;
      return !(num > 0M) ? num : num * -1M;
    }

    public static Decimal CalculateAssignedAmount(PipelineInfo[] assignments)
    {
      return CorrespondentTradeCalculation.CalculateAssignedAmount(((IEnumerable<PipelineInfo>) assignments).Select<PipelineInfo, Hashtable>((Func<PipelineInfo, Hashtable>) (a => a.Info)));
    }

    public static Decimal CalculateAssignedAmount(IEnumerable<Hashtable> assignments)
    {
      Decimal assignedAmount = 0M;
      if (assignments == null)
        return assignedAmount;
      foreach (Hashtable assignment in assignments)
        assignedAmount += Utils.ParseDecimal(assignment[(object) "Loan.TotalLoanAmount"]);
      return assignedAmount;
    }

    public static Dictionary<CorrespondentMasterDeliveryType, Decimal> CalculateOutStandingCommitments(
      List<CorrespondentTradeInfo> trades,
      bool isCommitmentUseBestEffortLimited,
      bool ignoreBestEffortsLimtedCheck = false)
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      foreach (CorrespondentMasterDeliveryType key in Enum.GetValues(typeof (CorrespondentMasterDeliveryType)).Cast<CorrespondentMasterDeliveryType>())
      {
        if (key != CorrespondentMasterDeliveryType.None)
          standingCommitments.Add(key, 0M);
      }
      if (trades == null)
        return standingCommitments;
      trades = trades.Where<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, bool>) (t => t.Status != TradeStatus.Archived)).ToList<CorrespondentTradeInfo>();
      if (!ignoreBestEffortsLimtedCheck && !isCommitmentUseBestEffortLimited)
        trades = trades.Where<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, bool>) (t => t.CommitmentType == CorrespondentTradeCommitmentType.Mandatory)).ToList<CorrespondentTradeInfo>();
      foreach (IGrouping<CorrespondentMasterDeliveryType, CorrespondentTradeInfo> source1 in trades.GroupBy<CorrespondentTradeInfo, CorrespondentMasterDeliveryType>((Func<CorrespondentTradeInfo, CorrespondentMasterDeliveryType>) (t => t.DeliveryType)))
      {
        CorrespondentMasterDeliveryType key = source1.Key;
        Decimal num1 = source1.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.TradeAmount));
        Decimal num2 = source1.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.PairOffAmount));
        List<LoanSummaryExtension> source2 = new List<LoanSummaryExtension>();
        foreach (CorrespondentTradeInfo correspondentTradeInfo in (IEnumerable<CorrespondentTradeInfo>) source1)
          source2.AddRange(correspondentTradeInfo.AssignedLoanList.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (x => x.CorrespondentTradeId > 0)));
        Decimal num3 = source2.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (l =>
        {
          DateTime receivedDate = l.ReceivedDate;
          return l.ReceivedDate > DateTime.MinValue;
        })).Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount));
        Decimal num4 = source2.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (l =>
        {
          DateTime receivedDate = l.ReceivedDate;
          if (!(l.ReceivedDate > DateTime.MinValue))
            return false;
          DateTime rejectedDate = l.RejectedDate;
          return l.RejectedDate > DateTime.MinValue;
        })).Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount));
        Decimal num5 = num1 + num2 - num3 + num4;
        if (standingCommitments.ContainsKey(key))
          standingCommitments.Remove(key);
        standingCommitments.Add(key, num5);
      }
      return standingCommitments;
    }

    public static Dictionary<CorrespondentMasterDeliveryType, Decimal> CalculateOutStandingCommitmentsBySummary(
      List<CorrespondentTradeInfo> trades,
      bool isCommitmentUseBestEffortLimited,
      bool ignoreBestEffortsLimtedCheck = false)
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> commitmentsBySummary = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      foreach (CorrespondentMasterDeliveryType key in Enum.GetValues(typeof (CorrespondentMasterDeliveryType)).Cast<CorrespondentMasterDeliveryType>())
      {
        if (key != CorrespondentMasterDeliveryType.None)
          commitmentsBySummary.Add(key, 0M);
      }
      if (trades == null)
        return commitmentsBySummary;
      trades = trades.Where<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, bool>) (t => t.Status != TradeStatus.Archived)).ToList<CorrespondentTradeInfo>();
      if (!ignoreBestEffortsLimtedCheck && !isCommitmentUseBestEffortLimited)
        trades = trades.Where<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, bool>) (t => t.CommitmentType == CorrespondentTradeCommitmentType.Mandatory)).ToList<CorrespondentTradeInfo>();
      foreach (IGrouping<CorrespondentMasterDeliveryType, CorrespondentTradeInfo> source in trades.GroupBy<CorrespondentTradeInfo, CorrespondentMasterDeliveryType>((Func<CorrespondentTradeInfo, CorrespondentMasterDeliveryType>) (t => t.DeliveryType)))
      {
        CorrespondentMasterDeliveryType key = source.Key;
        Decimal num = source.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.TradeAmount)) + source.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.PairOffAmount)) - source.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.DeliveredAmount)) + source.Sum<CorrespondentTradeInfo>((Func<CorrespondentTradeInfo, Decimal>) (t => t.RejectedAmount));
        if (commitmentsBySummary.ContainsKey(key))
          commitmentsBySummary.Remove(key);
        commitmentsBySummary.Add(key, num);
      }
      return commitmentsBySummary;
    }

    public static Decimal CalculateWeightedAvgBulkPrice(
      Decimal totalPrice,
      Decimal totalAmount,
      CorrespondentTradeInfo tradeInfo,
      bool isCalculated)
    {
      Decimal weightedAvgBulkPrice = 0M;
      if (((!(totalAmount > 0M) ? 0 : (tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.Bulk ? 1 : (tradeInfo.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT ? 1 : 0))) & (isCalculated ? 1 : 0)) != 0)
        weightedAvgBulkPrice = totalPrice / totalAmount;
      return weightedAvgBulkPrice;
    }
  }
}
