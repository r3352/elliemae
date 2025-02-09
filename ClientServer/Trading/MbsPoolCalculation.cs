// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolCalculation
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolCalculation : TradeCalculation
  {
    internal MbsPoolCalculation(ITradeInfoObject trade)
      : base(trade)
    {
    }

    private MbsPoolInfo Trade => base.Trade != null ? (MbsPoolInfo) base.Trade : (MbsPoolInfo) null;

    public static Decimal CalculateAssignedAmount(MbsPoolAssignment[] assignments)
    {
      Decimal num = 0M;
      return assignments == null ? num : ((IEnumerable<MbsPoolAssignment>) assignments).Sum<MbsPoolAssignment>((Func<MbsPoolAssignment, Decimal>) (a => a.AssigneeTrade.TradeAmount));
    }

    public static Decimal CalculateAllocatedAmount(MbsPoolAssignment[] assignments)
    {
      Decimal num = 0M;
      return assignments == null ? num : ((IEnumerable<MbsPoolAssignment>) assignments).Sum<MbsPoolAssignment>((Func<MbsPoolAssignment, Decimal>) (a => a.AssignedAmount));
    }

    public Decimal CalculateOpenAmount(MbsPoolAssignment[] assignments)
    {
      return MbsPoolCalculation.CalculateOpenAmount(this.Trade.TradeAmount, assignments);
    }

    public static Decimal CalculateOpenAmount(Decimal tradeAmount, MbsPoolAssignment[] assignments)
    {
      Decimal allocatedAmount = MbsPoolCalculation.CalculateAllocatedAmount(assignments);
      return MbsPoolCalculation.CalculateOpenAmount(tradeAmount, allocatedAmount);
    }

    public static Decimal CalculateOpenAmount(Decimal tradeAmount, Decimal assignedAmount)
    {
      return tradeAmount - assignedAmount;
    }

    public static Decimal CalculateWeightedAveragePrice(MbsPoolAssignment[] assignments)
    {
      Decimal allocatedAmount = MbsPoolCalculation.CalculateAllocatedAmount(assignments);
      if (allocatedAmount == 0M)
        return 0M;
      Decimal weightedAveragePrice = 0M;
      foreach (MbsPoolAssignment assignment in assignments)
        weightedAveragePrice += assignment.AssignedAmount / allocatedAmount * ((SecurityTradeInfo) assignment.AssigneeTrade).Price;
      return weightedAveragePrice;
    }

    public static Decimal CalculateTotalPrice(
      Decimal weightedAvgPrice,
      Decimal noteRate,
      TradeAdvancedPricingInfo pricingInfo,
      Decimal cpa = 0M)
    {
      return noteRate != 0M && pricingInfo != null && pricingInfo.PricingItems != null && pricingInfo.PricingItems.Count > 0 ? pricingInfo.PricingItems.Where<TradeAdvancedPricingItem>((Func<TradeAdvancedPricingItem, bool>) (i => i.NoteRate == noteRate)).Select<TradeAdvancedPricingItem, Decimal>((Func<TradeAdvancedPricingItem, Decimal>) (i => weightedAvgPrice + i.BuyUp + i.BuyDown + cpa)).FirstOrDefault<Decimal>() : 0M;
    }

    public static Decimal CalculateFannieMaePEPoolTotalPrice(
      Decimal weightedAvgPrice,
      Decimal noteRate,
      string commitmentContractNumber,
      string productName,
      TradeAdvancedPricingInfo pricingInfo,
      Decimal cpa = 0M)
    {
      return noteRate != 0M && pricingInfo != null && pricingInfo.PricingItems != null && pricingInfo.PricingItems.Count > 0 ? pricingInfo.PricingItems.Where<TradeAdvancedPricingItem>((Func<TradeAdvancedPricingItem, bool>) (i => i.NoteRate == noteRate && i.GSEContractNumber == commitmentContractNumber && i.ProductName == productName)).Select<TradeAdvancedPricingItem, Decimal>((Func<TradeAdvancedPricingItem, Decimal>) (i => weightedAvgPrice + i.BuyUp + i.BuyDown + cpa)).FirstOrDefault<Decimal>() : 0M;
    }

    public static Decimal CalculateTotalPrice(
      Decimal weightedAvgPrice,
      TradeAdvancedPricingItem item,
      Decimal cpa = 0M)
    {
      return item != null ? weightedAvgPrice + item.BuyUp + item.BuyDown + cpa : 0M;
    }

    public static Decimal CalculateFannieMaePEExcessServicing(
      Decimal coupon,
      Decimal guaranteeFee,
      Decimal minServicing,
      Decimal maxBU,
      TradeAdvancedPricingItem item)
    {
      return item != null ? item.NoteRate - coupon - guaranteeFee - minServicing - maxBU : 0M;
    }

    public static Tuple<Decimal, Decimal> CalculateBuyUpDownAmount(
      MbsPoolBuyUpDownItems buyUpDownItems,
      TradeAdvancedPricingItem item)
    {
      bool isBuyUp = item.GNMAIIExcess > 0M;
      if (buyUpDownItems != null && buyUpDownItems.Count > 0)
      {
        foreach (MbsPoolBuyUpDownItem poolBuyUpDownItem in buyUpDownItems.Where<MbsPoolBuyUpDownItem>((Func<MbsPoolBuyUpDownItem, bool>) (i => i.IsBuyUp == isBuyUp)))
        {
          if (poolBuyUpDownItem.GnrMin <= item.NoteRate && item.NoteRate <= poolBuyUpDownItem.GnrMax)
            return isBuyUp ? Tuple.Create<Decimal, Decimal>(item.GNMAIIExcess * poolBuyUpDownItem.Ratio, 0M) : Tuple.Create<Decimal, Decimal>(0M, item.GNMAIIExcess * poolBuyUpDownItem.Ratio);
        }
      }
      return Tuple.Create<Decimal, Decimal>(0M, 0M);
    }

    public static TradeAdvancedPricingInfo CalculateTradeAdvancedPricing(
      Decimal coupon,
      Decimal guaranteeFee,
      Decimal serviceFee,
      Decimal minServicingFee,
      Decimal maxBU,
      MbsPoolBuyUpDownItems buyUpDownItems,
      TradeAdvancedPricingInfo pricing,
      MbsPoolMortgageType poolType)
    {
      TradeAdvancedPricingInfo tradeAdvancedPricing = pricing;
      if (tradeAdvancedPricing != null && tradeAdvancedPricing.PricingItems != null && tradeAdvancedPricing.PricingItems.Count > 0)
      {
        List<TradeAdvancedPricingItem> advancedPricingItemList = new List<TradeAdvancedPricingItem>();
        foreach (TradeAdvancedPricingItem pricingItem in tradeAdvancedPricing.PricingItems)
          advancedPricingItemList.Add(MbsPoolCalculation.CalculateTradeAdvancedPricing(coupon, guaranteeFee, serviceFee, minServicingFee, maxBU, buyUpDownItems, pricingItem, poolType));
        tradeAdvancedPricing.PricingItems.Clear();
        foreach (TradeAdvancedPricingItem advancedPricingItem in advancedPricingItemList)
          tradeAdvancedPricing.PricingItems.Add(advancedPricingItem);
      }
      return tradeAdvancedPricing;
    }

    public static TradeAdvancedPricingItem CalculateTradeAdvancedPricing(
      Decimal coupon,
      Decimal guaranteeFee,
      Decimal serviceFee,
      Decimal minServicingFee,
      Decimal maxBU,
      MbsPoolBuyUpDownItems buyUpDownItems,
      TradeAdvancedPricingItem item,
      MbsPoolMortgageType poolType)
    {
      TradeAdvancedPricingItem tradeAdvancedPricing = item;
      if (tradeAdvancedPricing == null)
        return (TradeAdvancedPricingItem) null;
      tradeAdvancedPricing.GNMAIIExcess = 0M;
      tradeAdvancedPricing.BuyUp = 0M;
      tradeAdvancedPricing.BuyDown = 0M;
      if (poolType == MbsPoolMortgageType.GinnieMae)
      {
        tradeAdvancedPricing.GNMAIIExcess = tradeAdvancedPricing.NoteRate - coupon - guaranteeFee;
      }
      else
      {
        tradeAdvancedPricing.GuarantyFee = tradeAdvancedPricing.NoteRate - coupon - tradeAdvancedPricing.ServicingFee;
        tradeAdvancedPricing.ServicingRetained = tradeAdvancedPricing.NoteRate - coupon - guaranteeFee - tradeAdvancedPricing.ServicingFee;
        tradeAdvancedPricing.GNMAIIExcess = tradeAdvancedPricing.NoteRate - coupon - guaranteeFee - minServicingFee - maxBU;
        Tuple<Decimal, Decimal> peBuyUpDownAmount = MbsPoolCalculation.CalculateFannieMaePEBuyUpDownAmount(buyUpDownItems, tradeAdvancedPricing, maxBU);
        tradeAdvancedPricing.BuyUp = peBuyUpDownAmount.Item1;
        tradeAdvancedPricing.BuyDown = peBuyUpDownAmount.Item2;
      }
      return tradeAdvancedPricing;
    }

    public static Tuple<Decimal, Decimal> CalculateFannieMaePEBuyUpDownAmount(
      MbsPoolBuyUpDownItems buyUpDownItems,
      TradeAdvancedPricingItem item,
      Decimal maxBU)
    {
      if (item == null)
        return Tuple.Create<Decimal, Decimal>(0M, 0M);
      bool isBuyUp = item.ServicingRetained > 0M;
      if (buyUpDownItems != null && buyUpDownItems.Count > 0)
      {
        foreach (MbsPoolBuyUpDownItem poolBuyUpDownItem in buyUpDownItems.Where<MbsPoolBuyUpDownItem>((Func<MbsPoolBuyUpDownItem, bool>) (i => i.IsBuyUp == isBuyUp)))
        {
          if (poolBuyUpDownItem.GnrMin <= item.NoteRate && item.NoteRate <= poolBuyUpDownItem.GnrMax)
          {
            if (!isBuyUp)
              return Tuple.Create<Decimal, Decimal>(0M, item.ServicingRetained * poolBuyUpDownItem.Ratio);
            return item.ServicingRetained <= maxBU ? Tuple.Create<Decimal, Decimal>(item.ServicingRetained * poolBuyUpDownItem.Ratio, 0M) : Tuple.Create<Decimal, Decimal>(maxBU * poolBuyUpDownItem.Ratio, 0M);
          }
        }
      }
      return Tuple.Create<Decimal, Decimal>(0M, 0M);
    }

    public static Tuple<Decimal, Decimal> GetServicingGuarantyFeeForNoteRate(
      Decimal noteRate,
      MbsPoolInfo poolInfo,
      string commitmentContracNumber,
      string productName)
    {
      if (noteRate != 0M && poolInfo.Pricing.AdvancedPricingInfo != null && poolInfo.Pricing.AdvancedPricingInfo.PricingItems != null && poolInfo.Pricing.AdvancedPricingInfo.PricingItems.Count > 0)
      {
        if (poolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMae || poolInfo.PoolMortgageType == MbsPoolMortgageType.FreddieMac)
        {
          TradeAdvancedPricingItem advancedPricingItem = poolInfo.Pricing.AdvancedPricingInfo.PricingItems.Where<TradeAdvancedPricingItem>((Func<TradeAdvancedPricingItem, bool>) (i => i.NoteRate == noteRate)).FirstOrDefault<TradeAdvancedPricingItem>();
          return advancedPricingItem == null ? (Tuple<Decimal, Decimal>) null : Tuple.Create<Decimal, Decimal>(advancedPricingItem.ServicingFee, advancedPricingItem.GuarantyFee);
        }
        if (poolInfo.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
        {
          TradeAdvancedPricingItem advancedPricingItem = poolInfo.Pricing.AdvancedPricingInfo.PricingItems.Where<TradeAdvancedPricingItem>((Func<TradeAdvancedPricingItem, bool>) (i => i.NoteRate == noteRate && i.GSEContractNumber == commitmentContracNumber && i.ProductName == productName)).FirstOrDefault<TradeAdvancedPricingItem>();
          return advancedPricingItem == null ? (Tuple<Decimal, Decimal>) null : Tuple.Create<Decimal, Decimal>(advancedPricingItem.ServicingFee, advancedPricingItem.GuarantyFee);
        }
      }
      return (Tuple<Decimal, Decimal>) null;
    }
  }
}
