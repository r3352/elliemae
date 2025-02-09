// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeInfo : TradeInfoObj
  {
    private string contractNumber;
    private string dealerName;
    private string assigneeName;
    private string investorTradeNum;
    private string investorCommitmentNum;
    private Decimal rateAdjustment;
    private Decimal buyUpAmount;
    private Decimal buyDownAmount;
    private Decimal miscAdjustment;
    private DateTime lastModified;
    private int loanCount;
    private Dictionary<LoanTradeStatus, int> pendingLoanList;
    private Decimal assignedProfit;
    private Decimal assignedAmount;
    private Decimal pairOffAmount;
    private Decimal completePercent;
    private int pendingLoanCount;
    private int securityTradeID;
    private Decimal gainLossAmount;
    private Decimal netProfit;
    private LoanTradePairOffs loanTradePairOffs;
    private string servicer;
    private ServicingType servicingType;
    private bool isBulkDelivery;
    private Decimal weightedAvgBulkPrice;
    private bool isWeightedAvgBulkPriceLocked;

    public LoanTradeInfo(
      int tradeID,
      string guid,
      string name,
      int contractId,
      string contractNumber,
      TradeStatus status,
      bool locked,
      string dealerName,
      string assigneeName,
      string investorName,
      string investorTradeNum,
      string investorCommitmentNum,
      DateTime investorDeliveryDate,
      DateTime earlyDeliveryDate,
      DateTime targetDeliveryDate,
      Decimal tradeAmount,
      Decimal tolerance,
      Decimal pairOffFee,
      DateTime commitmentDate,
      DateTime shipmentDate,
      DateTime purchaseDate,
      Decimal rateAdjustment,
      Decimal buyUpAmount,
      Decimal buyDownAmount,
      Decimal miscAdjustment,
      DateTime lastModified,
      int loanCount,
      Decimal assignedProfit,
      Decimal assignedAmount,
      Decimal pairOffAmount,
      string notes,
      string filterQueryXml,
      string srpTableXml,
      string investorXml,
      string dealerXml,
      string assigneeXml,
      string pricingXml,
      string adjustmentsXml,
      string pairOffXml,
      Decimal completePercent,
      int pendingLoanCount,
      Decimal minAmount,
      Decimal maxAmount,
      Dictionary<LoanTradeStatus, int> pendingLoanList,
      string commitmentType,
      string tradeDescription,
      int securityTradeID,
      Decimal gainLossAmount,
      Decimal netProfit,
      string servicer,
      ServicingType servicingType,
      bool isWeightedAvgBulkPriceLocked,
      Decimal weightedAvgBulkPrice,
      bool isBulkDelivery)
      : base(tradeID, guid, name)
    {
      this.ContractID = contractId;
      this.ParseTradeObjects(notes, filterQueryXml, "", pricingXml, adjustmentsXml, srpTableXml, investorXml, dealerXml, assigneeXml, "", "", "", "");
      this.loanTradePairOffs = BinaryConvertible<LoanTradePairOffs>.Parse(pairOffXml);
      if (this.loanTradePairOffs == null)
        this.loanTradePairOffs = new LoanTradePairOffs();
      this.contractNumber = contractNumber;
      this.Status = status;
      this.Locked = locked;
      this.dealerName = dealerName;
      this.assigneeName = assigneeName;
      this.InvestorName = investorName;
      this.investorTradeNum = investorTradeNum;
      this.investorCommitmentNum = investorCommitmentNum;
      this.InvestorDeliveryDate = investorDeliveryDate;
      this.EarlyDeliveryDate = earlyDeliveryDate;
      this.TargetDeliveryDate = targetDeliveryDate;
      this.TradeAmount = tradeAmount;
      this.Tolerance = tolerance;
      this.PairOffFee = pairOffFee;
      this.CommitmentDate = commitmentDate;
      this.ShipmentDate = shipmentDate;
      this.PurchaseDate = purchaseDate;
      this.rateAdjustment = rateAdjustment;
      this.buyUpAmount = buyUpAmount;
      this.buyDownAmount = buyDownAmount;
      this.miscAdjustment = miscAdjustment;
      this.lastModified = lastModified;
      this.loanCount = loanCount;
      this.completePercent = completePercent;
      this.pendingLoanCount = pendingLoanCount;
      this.MaxAmount = maxAmount;
      this.MinAmount = minAmount;
      this.pendingLoanCount = pendingLoanCount;
      this.pendingLoanList = pendingLoanList;
      this.assignedProfit = assignedProfit;
      this.assignedAmount = assignedAmount;
      this.pairOffAmount = pairOffAmount;
      this.CommitmentType = commitmentType;
      this.TradeDescription = tradeDescription;
      this.securityTradeID = securityTradeID;
      this.gainLossAmount = gainLossAmount;
      this.netProfit = netProfit;
      this.TradeType = TradeType.LoanTrade;
      this.servicer = servicer;
      this.servicingType = servicingType;
      this.isBulkDelivery = isBulkDelivery;
      this.IsWeightedAvgBulkPriceLocked = isWeightedAvgBulkPriceLocked;
      this.weightedAvgBulkPrice = weightedAvgBulkPrice;
    }

    public LoanTradeInfo(LoanTradeInfo source)
      : base(-1, "", "")
    {
      this.TradeDescription = source.TradeDescription;
      this.TradeAmount = source.TradeAmount;
      this.Tolerance = source.Tolerance;
      this.MinAmount = source.MinAmount;
      this.MaxAmount = source.MaxAmount;
      this.rateAdjustment = source.rateAdjustment;
      this.buyUpAmount = source.buyUpAmount;
      this.buyDownAmount = source.buyDownAmount;
      this.ParseTradeObjects("", source.Filter.ToXml(), "", source.Pricing.ToXml(), source.PriceAdjustments.ToXml(), source.SRPTable.ToXml(), source.Investor.ToXml(), source.Dealer.ToXml(), "", "", "", "", "");
      this.loanTradePairOffs = new LoanTradePairOffs();
      this.TradeType = TradeType.LoanTrade;
      this.servicer = source.servicer;
      this.servicingType = source.servicingType;
    }

    public LoanTradeInfo()
    {
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.loanTradePairOffs = new LoanTradePairOffs();
      this.TradeType = TradeType.LoanTrade;
      this.servicingType = ServicingType.None;
    }

    public int SecurityTradeID
    {
      get => this.securityTradeID;
      set => this.securityTradeID = value;
    }

    public Decimal GainLossAmount
    {
      get => this.gainLossAmount;
      set => this.gainLossAmount = value;
    }

    public Decimal NetProfit
    {
      get => this.netProfit;
      set => this.netProfit = value;
    }

    public LoanTradePairOffs LoanTradePairOffs
    {
      get => this.loanTradePairOffs;
      set => this.loanTradePairOffs = value;
    }

    public string Servicer
    {
      get => this.servicer;
      set => this.servicer = value;
    }

    public ServicingType ServicingType
    {
      get => this.servicingType;
      set => this.servicingType = value;
    }

    public override TradePairOffs PairOffs
    {
      get => throw new NotImplementedException("Please use LoanTradePairOffs objects instead");
    }

    public override Decimal PairOffAmount
    {
      get
      {
        Decimal num = 0M;
        foreach (LoanTradePairOff loanTradePairOff in this.loanTradePairOffs)
          num += loanTradePairOff.TradeAmount;
        return !(num > 0M) ? num : num * -1M;
      }
    }

    public string AssigneeName
    {
      get => this.Assignee.EntityName;
      set => this.Assignee.EntityName = value;
    }

    public override Decimal PairOffFee
    {
      get => this.Investor.PairOffFee;
      set => this.Investor.PairOffFee = value;
    }

    public Decimal RateAdjustment
    {
      get => this.rateAdjustment;
      set => this.rateAdjustment = value;
    }

    public Decimal BuyUpAmount
    {
      get => this.buyUpAmount;
      set => this.buyUpAmount = value;
    }

    public Decimal BuyDownAmount
    {
      get => this.buyDownAmount;
      set => this.buyDownAmount = value;
    }

    public override TradeStatus Status
    {
      get
      {
        if (base.Status == TradeStatus.Pending)
          return TradeStatus.Pending;
        if (base.Status == TradeStatus.Archived)
          return TradeStatus.Archived;
        if (this.PurchaseDate != DateTime.MinValue)
          return TradeStatus.Purchased;
        if (this.ShipmentDate != DateTime.MinValue)
          return TradeStatus.Shipped;
        return this.CommitmentDate != DateTime.MinValue ? TradeStatus.Committed : TradeStatus.Open;
      }
      set => base.Status = value;
    }

    public string InvestorTradeNumber
    {
      get => this.investorTradeNum;
      set => this.investorTradeNum = value;
    }

    public string InvestorCommitmentNumber
    {
      get => this.investorCommitmentNum;
      set => this.investorCommitmentNum = value;
    }

    public Decimal MiscAdjustment
    {
      get => this.miscAdjustment;
      set => this.miscAdjustment = value;
    }

    public string ContractNumber
    {
      get => this.contractNumber;
      set => this.contractNumber = value;
    }

    public bool IsBulkDelivery
    {
      get => this.isBulkDelivery;
      set => this.isBulkDelivery = value;
    }

    public Decimal WeightedAvgBulkPrice
    {
      get => this.weightedAvgBulkPrice;
      set => this.weightedAvgBulkPrice = value;
    }

    public bool IsWeightedAvgBulkPriceLocked
    {
      get => this.isWeightedAvgBulkPriceLocked;
      set => this.isWeightedAvgBulkPriceLocked = value;
    }

    public int GetPendingLoanCount(LoanTradeStatus status)
    {
      return this.pendingLoanList == null || !this.pendingLoanList.ContainsKey(status) ? 0 : this.pendingLoanList[status];
    }

    public bool HasPendingLoan() => this.pendingLoanList != null && this.pendingLoanList.Count > 0;

    public DateTime LastModified
    {
      get => this.lastModified;
      set => this.lastModified = value;
    }

    public int LoanCount
    {
      get => this.loanCount;
      set => this.loanCount = value;
    }

    public LoanTradeInfo Duplicate()
    {
      LoanTradeInfo loanTradeInfo = new LoanTradeInfo(this);
      loanTradeInfo.IsCloned = true;
      return loanTradeInfo;
    }

    public Decimal CalculatePriceIndex(PipelineInfo info)
    {
      return this.CalculatePriceIndex(info, true, 0M);
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice)
    {
      return this.CalculatePriceIndex(info, true, securityPrice);
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, bool includeSrp, Decimal securityPrice)
    {
      Decimal noteRate = Utils.ParseDecimal(info.GetField("LoanRate"));
      if (this.isBulkDelivery)
        return securityPrice;
      Decimal priceForNoteRate = this.GetBasePriceForNoteRate(noteRate, securityPrice);
      if (priceForNoteRate == 0M)
        return 0M;
      foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) this.PriceAdjustments)
      {
        if (priceAdjustment.CriterionList.CreateEvaluator().Evaluate(info, FilterEvaluationOption.None))
          priceForNoteRate += priceAdjustment.PriceAdjustment;
      }
      if (includeSrp)
        priceForNoteRate += this.SRPTable.GetAdjustmentForLoan(info);
      return priceForNoteRate;
    }

    public Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice)
    {
      Decimal num1 = Utils.ParseDecimal(info.GetField("TotalLoanAmount"));
      Decimal num2 = Utils.ParseDecimal(info.GetField("TotalBuyPrice"));
      if (num2 <= 0M || num1 <= 0M)
        return 0M;
      Decimal priceIndex = this.CalculatePriceIndex(info, securityPrice);
      return priceIndex == 0M ? 0M : Math.Round((priceIndex - num2) * num1 / 100M, 2);
    }

    public Decimal CalculateGainLoss(IEnumerable<PipelineInfo> loans)
    {
      return this.CalculateGainLoss(loans, (SecurityTradeInfo) null);
    }

    public Decimal CalculateGainLoss(PipelineInfo[] loanList)
    {
      return this.CalculateGainLoss(loanList, (SecurityTradeInfo) null);
    }

    public Decimal CalculateGainLoss(
      IEnumerable<PipelineInfo> loans,
      SecurityTradeInfo secTradeInfo)
    {
      return loans == null ? 0M : this.CalculateGainLoss(new List<PipelineInfo>(loans).ToArray(), secTradeInfo);
    }

    public Decimal CalculateGainLoss(PipelineInfo[] loanList, SecurityTradeInfo secTradeInfo)
    {
      Decimal gainLoss = 0M;
      foreach (PipelineInfo loan in loanList)
      {
        if (!this.Pricing.IsAdvancedPricing)
          gainLoss += this.CalculateProfit(loan, 0M);
        else if (secTradeInfo == null)
          gainLoss += this.CalculateProfit(loan, 0M);
        else
          gainLoss += this.CalculateProfit(loan, secTradeInfo.Price);
      }
      return gainLoss;
    }

    public Decimal CalculateNetProfit(PipelineInfo[] loanList)
    {
      return this.CalculateNetProfit(loanList, (SecurityTradeInfo) null);
    }

    public Decimal CalculateNetProfit(PipelineInfo[] loanList, SecurityTradeInfo secTradeInfo)
    {
      return this.CalculateGainLoss(loanList, secTradeInfo) - this.MiscAdjustment;
    }

    public void ResetCalculatedField(PipelineInfo[] pipelineList)
    {
      this.gainLossAmount = this.CalculateGainLoss(pipelineList);
      this.netProfit = this.CalculateNetProfit(pipelineList);
    }

    public bool IsNoteRateAllowed(PipelineInfo info)
    {
      return this.IsNoteRateAllowed(Utils.ParseDecimal(info.GetField("LoanRate")));
    }

    public bool IsNoteRateAllowed(Decimal noteRate)
    {
      if (!this.Pricing.IsAdvancedPricing)
      {
        if (this.Pricing.SimplePricingItems.Count == 0)
          return false;
        if (noteRate < this.Pricing.SimplePricingItems[0].Rate)
        {
          if (this.rateAdjustment == 0M || this.buyDownAmount <= 0M)
            return false;
          Decimal d = (this.Pricing.SimplePricingItems[0].Rate - noteRate) / this.rateAdjustment;
          return Math.Floor(d) == d;
        }
        if (noteRate > this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate)
        {
          if (this.rateAdjustment == 0M || this.buyDownAmount <= 0M)
            return false;
          Decimal d = (this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate - noteRate) / this.rateAdjustment;
          return Math.Floor(d) == d;
        }
        for (int index = 0; index < this.Pricing.SimplePricingItems.Count; ++index)
        {
          if (this.Pricing.SimplePricingItems[index].Rate == noteRate)
            return true;
        }
      }
      else
      {
        if (this.Pricing.AdvancedPricingInfo.PricingItems.Count == 0)
          return false;
        for (int index = 0; index < this.Pricing.AdvancedPricingInfo.PricingItems.Count; ++index)
        {
          if (this.Pricing.AdvancedPricingInfo.PricingItems[index].NoteRate == noteRate)
            return true;
        }
      }
      return false;
    }

    public Decimal GetBasePriceForNoteRate(Decimal noteRate, Decimal securityPrice)
    {
      if (!this.IsNoteRateAllowed(noteRate))
        return 0M;
      if (!this.Pricing.IsAdvancedPricing)
      {
        if (noteRate < this.Pricing.SimplePricingItems[0].Rate)
          return this.Pricing.SimplePricingItems[0].Price - Math.Floor((this.Pricing.SimplePricingItems[0].Rate - noteRate) / this.rateAdjustment) * this.buyDownAmount;
        if (noteRate > this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate)
          return this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Price + Math.Floor((noteRate - this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate) / this.rateAdjustment) * this.buyUpAmount;
        for (int index = 0; index < this.Pricing.SimplePricingItems.Count; ++index)
        {
          if (this.Pricing.SimplePricingItems[index].Rate == noteRate)
            return this.Pricing.SimplePricingItems[index].Price;
        }
        return 0M;
      }
      return this.Pricing.AdvancedPricingInfo.HasPricingSetting(noteRate) ? securityPrice + this.Pricing.AdvancedPricingInfo.GetPricingForNoteRate(noteRate) : 0M;
    }

    public Decimal GetServiceForNoteRate(Decimal noteRate)
    {
      if (!this.IsNoteRateAllowed(noteRate) || this.Pricing.IsAdvancedPricing)
        return 0M;
      for (int index = 0; index < this.Pricing.SimplePricingItems.Count; ++index)
      {
        if (this.Pricing.SimplePricingItems[index].Rate == noteRate)
          return this.Pricing.SimplePricingItems[index].ServiceFee;
      }
      return 0M;
    }

    public string[] GetPricingFields()
    {
      List<string> stringList = new List<string>();
      stringList.Add("Loan.TotalLoanAmount");
      stringList.Add("Loan.State");
      stringList.Add("Loan.LoanRate");
      stringList.Add("Loan.EscrowWaived");
      stringList.Add("Loan.TotalBuyPrice");
      foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) this.PriceAdjustments)
      {
        foreach (string field in priceAdjustment.CriterionList.GetFieldList())
        {
          if (!stringList.Contains(field))
            stringList.Add(field);
        }
      }
      return stringList.ToArray();
    }

    public string[] GetPricingAndEligibilityFields()
    {
      List<string> stringList = new List<string>((IEnumerable<string>) this.GetPricingFields());
      if (this.Filter != null)
      {
        string[] fieldList = this.Filter.GetFieldList();
        for (int index = 0; index < fieldList.Length; ++index)
        {
          if (!stringList.Contains(fieldList[index]))
            stringList.Add(fieldList[index]);
        }
      }
      return stringList.ToArray();
    }

    public static bool ComparePricing(LoanTradeInfo trade1, LoanTradeInfo trade2)
    {
      return !(trade1.BuyDownAmount != trade2.BuyDownAmount) && !(trade1.BuyUpAmount != trade2.BuyUpAmount) && !(trade1.RateAdjustment != trade2.RateAdjustment) && !(trade1.PriceAdjustments.ToXml() != trade2.PriceAdjustments.ToXml()) && !(trade1.Pricing.ToXml() != trade2.Pricing.ToXml()) && !(trade1.SRPTable.ToXml() != trade2.SRPTable.ToXml()) && !(trade1.LoanTradePairOffs.ToXml() != trade2.LoanTradePairOffs.ToXml());
    }

    public void ResetPairoffAmount() => this.pairOffAmount = this.PairOffAmount;
  }
}
