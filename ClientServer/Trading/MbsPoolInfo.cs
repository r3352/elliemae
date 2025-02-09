// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Reporting;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class MbsPoolInfo : TradeInfoObj
  {
    public MbsPoolInfo(
      int tradeID,
      string guid,
      string name,
      string notes,
      string filterQueryXml,
      string srpTableXml,
      string investorXml,
      string dealerXml,
      string assigneeXml,
      string pricingXml,
      string adjustmentsXml,
      string buyUpDownXml,
      string productNameXml,
      Dictionary<MbsPoolLoanStatus, int> pendingLoanList)
      : base(tradeID, guid, name)
    {
      this.ParseTradeObjects(notes, filterQueryXml, "", pricingXml, adjustmentsXml, "", investorXml, dealerXml, assigneeXml, buyUpDownXml, productNameXml, "", "");
      this.TradeType = TradeType.MbsPool;
      this.PendingLoanList = pendingLoanList;
    }

    public MbsPoolInfo(MbsPoolInfo source)
      : base(-1, "", "")
    {
      this.PoolMortgageType = source.PoolMortgageType;
      this.PoolNumber = source.PoolNumber;
      this.SuffixID = source.SuffixID;
      this.CUSIP = source.CUSIP;
      this.TradeDescription = source.TradeDescription;
      this.TradeAmount = source.TradeAmount;
      this.Tolerance = source.Tolerance;
      this.RateAdjustment = source.RateAdjustment;
      this.BuyUpAmount = source.BuyUpAmount;
      this.BuyDownAmount = source.BuyDownAmount;
      this.ParseTradeObjects("", source.Filter.ToXml(), "", source.Pricing.ToXml(), source.PriceAdjustments.ToXml(), "", source.Investor.ToXml(), source.Dealer.ToXml(), "", source.BuyUpDownItems.ToXml(), source.ProductNames.ToXml(), "", "");
      this.TradeType = TradeType.MbsPool;
      this.ServicingType = source.ServicingType;
      this.Servicer = source.Servicer;
      this.Coupon = source.Coupon;
      this.SettlementDate = source.SettlementDate;
      this.SettlementMonth = source.SettlementMonth;
      this.NotificationDate = source.NotificationDate;
      this.GuaranteeFee = source.GuaranteeFee;
      this.FixedServicingFeePercent = source.FixedServicingFeePercent;
      this.DocCustodianID = source.DocCustodianID;
      this.ServicerID = source.ServicerID;
      this.InvestorProductPlanID = source.InvestorProductPlanID;
      this.InvestorFeatureID = source.InvestorFeatureID;
      this.LoanDefaultLossParty = source.LoanDefaultLossParty;
      this.ReoMarketingParty = source.ReoMarketingParty;
      this.BaseGuarantyFee = source.BaseGuarantyFee;
      this.GFeeAfterAltPaymentMethod = source.GFeeAfterAltPaymentMethod;
      this.GuaranteeFee = source.GuaranteeFee;
      this.InvestorRemittanceDay = source.InvestorRemittanceDay;
      this.ContractNum = source.ContractNum;
      this.IssueDate = source.IssueDate;
      this.OwnershipPercent = source.OwnershipPercent;
      this.StructureType = source.StructureType;
      this.AccrualRateStructureType = source.AccrualRateStructureType;
      this.SecurityIssueDateIntRate = source.SecurityIssueDateIntRate;
      this.MinAccuralRate = source.MinAccuralRate;
      this.MaxAccuralRate = source.MaxAccuralRate;
      this.MarginRate = source.MarginRate;
      this.IntRateRoundingType = source.IntRateRoundingType;
      this.IntRateRoundingPercent = source.IntRateRoundingPercent;
      this.IsInterestOnly = source.IsInterestOnly;
      this.IntPaymentAdjIndexLeadDays = source.IntPaymentAdjIndexLeadDays;
      this.IsAssumability = source.IsAssumability;
      this.IsBalloon = source.IsBalloon;
      this.FixedServicingFeePercent = source.FixedServicingFeePercent;
      this.ScheduledRemittancePaymentDay = source.ScheduledRemittancePaymentDay;
      this.SecurityTradeBookEntryDate = source.SecurityTradeBookEntryDate;
      this.PayeeCode = source.PayeeCode;
      this.CommitmentPeriod = source.CommitmentPeriod;
      this.SubmissionType = source.SubmissionType;
      this.PlanNum = source.PlanNum;
      this.PassThruRate = source.PassThruRate;
      this.ForecloseRiskCode = source.ForecloseRiskCode;
      this.ContractType = source.ContractType;
      this.DeliveryRegion = source.DeliveryRegion;
      this.InterestOnlyEndDate = source.InterestOnlyEndDate;
      this.IsMultiFamily = source.IsMultiFamily;
      this.NoteCustodian = source.NoteCustodian;
      this.FinancialInstNum = source.FinancialInstNum;
      this.StandardLookback = source.StandardLookback;
      this.IsGuarantyFeeAddOn = source.IsGuarantyFeeAddOn;
      this.InvestorRemittanceType = source.InvestorRemittanceType;
      this.SellerId = source.SellerId;
      this.IssuerNum = source.IssuerNum;
      this.IssueType = source.IssueType;
      this.ARMIndex = source.ARMIndex;
      this.CertAgreement = source.CertAgreement;
      this.PnICustodialABA = source.PnICustodialABA;
      this.SubscriberRecordABA = source.SubscriberRecordABA;
      this.SubscriberRecordFRBPosDesc = source.SubscriberRecordFRBPosDesc;
      this.SubscriberRecordFRBAcctDesc = source.SubscriberRecordFRBAcctDesc;
      this.MasterTnIABA = source.MasterTnIABA;
      this.NewTransferIssuerNum = source.NewTransferIssuerNum;
      this.MaturityDate = source.MaturityDate;
      this.PoolTaxID = source.PoolTaxID;
      this.MbsMargin = source.MbsMargin;
      this.ChangeDate = source.ChangeDate;
      this.IsBondFinancePool = source.IsBondFinancePool;
      this.IsSent1711ToCustodian = source.IsSent1711ToCustodian;
      this.PnICustodialAcctNum = source.PnICustodialAcctNum;
      this.SubscriberRecordAcctNum = source.SubscriberRecordAcctNum;
      this.MasterTnIAcctNum = source.MasterTnIAcctNum;
      this.SubservicerIssuerNum = source.SubservicerIssuerNum;
      this.GinniePoolType = source.GinniePoolType;
      this.InitialDate = source.InitialDate;
      this.LastPaidInstallmentDate = source.LastPaidInstallmentDate;
      this.UnpaidBalanceDate = source.UnpaidBalanceDate;
      this.ACHBankAccountPurposeType = source.ACHBankAccountPurposeType;
      this.ACHABARoutingAndTransitNum = source.ACHABARoutingAndTransitNum;
      this.ACHABARoutingAndTransitId = source.ACHABARoutingAndTransitId;
      this.ACHInsitutionTelegraphicName = source.ACHInsitutionTelegraphicName;
      this.ACHReceiverSubaccountName = source.ACHReceiverSubaccountName;
      this.ACHBankAccountDescription = source.ACHBankAccountDescription;
      this.GinniePoolIndexType = source.GinniePoolIndexType;
      this.PoolIssuerTransferee = source.PoolIssuerTransferee;
      this.PoolCertificatePaymentDate = source.PoolCertificatePaymentDate;
      this.BondFinanceProgramName = source.BondFinanceProgramName;
      this.BondFinanceProgramType = source.BondFinanceProgramType;
      this.GinniePoolClassType = source.GinniePoolClassType;
      this.GinniePoolConcurrentTransferIndicator = source.GinniePoolConcurrentTransferIndicator;
      this.PoolCurrentLoanCount = source.PoolCurrentLoanCount;
      this.PoolCurrentPrincipalBalAmt = source.PoolCurrentPrincipalBalAmt;
      this.PoolingMethodType = source.PoolingMethodType;
      this.PoolInterestAdjustmentEffectiveDate = source.PoolInterestAdjustmentEffectiveDate;
      this.PoolMaturityPeriodCount = source.PoolMaturityPeriodCount;
      this.DocSubmissionIndicator = source.DocSubmissionIndicator;
      this.DocReqIndicator = source.DocReqIndicator;
      this.SecurityOrigSubscriptionAmt = source.SecurityOrigSubscriptionAmt;
      this.MinServicingFee = source.MinServicingFee;
      this.MaxBU = source.MaxBU;
    }

    public MbsPoolInfo(MbsPoolMortgageType poolMortgageType)
      : this()
    {
      this.PoolMortgageType = poolMortgageType;
    }

    public MbsPoolInfo()
    {
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.TradeType = TradeType.MbsPool;
      this.ServicingType = ServicingType.None;
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

    public MbsPoolMortgageType PoolMortgageType { get; set; }

    public string PoolNumber { get; set; }

    public string SuffixID { get; set; }

    public string CUSIP { get; set; }

    public string MortgageType { get; set; }

    public string AmortizationType { get; set; }

    public int Term { get; set; }

    public ServicingType ServicingType { get; set; }

    public Decimal WeightedAvgPrice { get; set; }

    public bool WeightedAvgPriceLocked { get; set; }

    public DateTime SettlementDate { get; set; }

    public string SettlementMonth { get; set; }

    public DateTime NotificationDate { get; set; }

    public string ContractNumber { get; set; }

    public string Servicer { get; set; }

    public Decimal TBAOpenAmount { get; set; }

    public Decimal RateAdjustment { get; set; }

    public Decimal BuyUpAmount { get; set; }

    public Decimal BuyDownAmount { get; set; }

    public Decimal MiscAdjustment { get; set; }

    public Dictionary<MbsPoolLoanStatus, int> PendingLoanList { get; private set; }

    public string DocCustodianID { get; set; }

    public string ServicerID { get; set; }

    public string InvestorProductPlanID { get; set; }

    public string InvestorFeatureID { get; set; }

    public string LoanDefaultLossParty { get; set; }

    public string ReoMarketingParty { get; set; }

    public Decimal BaseGuarantyFee { get; set; }

    public Decimal GFeeAfterAltPaymentMethod { get; set; }

    public Decimal GuaranteeFee { get; set; }

    public Decimal CPA { get; set; }

    public int InvestorRemittanceDay { get; set; }

    public string ContractNum { get; set; }

    public DateTime IssueDate { get; set; }

    public Decimal OwnershipPercent { get; set; }

    public string StructureType { get; set; }

    public string AccrualRateStructureType { get; set; }

    public Decimal SecurityIssueDateIntRate { get; set; }

    public Decimal MinAccuralRate { get; set; }

    public Decimal MaxAccuralRate { get; set; }

    public Decimal MarginRate { get; set; }

    public string IntRateRoundingType { get; set; }

    public Decimal IntRateRoundingPercent { get; set; }

    public bool IsInterestOnly { get; set; }

    public int IntPaymentAdjIndexLeadDays { get; set; }

    public bool IsAssumability { get; set; }

    public bool IsBalloon { get; set; }

    public Decimal FixedServicingFeePercent { get; set; }

    public int ScheduledRemittancePaymentDay { get; set; }

    public DateTime SecurityTradeBookEntryDate { get; set; }

    public string PayeeCode { get; set; }

    public string CommitmentPeriod { get; set; }

    public string SubmissionType { get; set; }

    public string PlanNum { get; set; }

    public Decimal PassThruRate { get; set; }

    public string ForecloseRiskCode { get; set; }

    public string ContractType { get; set; }

    public string DeliveryRegion { get; set; }

    public DateTime InterestOnlyEndDate { get; set; }

    public bool IsMultiFamily { get; set; }

    public string NoteCustodian { get; set; }

    public string FinancialInstNum { get; set; }

    public string StandardLookback { get; set; }

    public bool IsGuarantyFeeAddOn { get; set; }

    public string InvestorRemittanceType { get; set; }

    public string SellerId { get; set; }

    public string IssuerNum { get; set; }

    public string IssueType { get; set; }

    public Decimal ARMIndex { get; set; }

    public string CertAgreement { get; set; }

    public string PnICustodialABA { get; set; }

    public string SubscriberRecordABA { get; set; }

    public string SubscriberRecordFRBPosDesc { get; set; }

    public string SubscriberRecordFRBAcctDesc { get; set; }

    public string MasterTnIABA { get; set; }

    public string NewTransferIssuerNum { get; set; }

    public DateTime MaturityDate { get; set; }

    public string PoolTaxID { get; set; }

    public Decimal MbsMargin { get; set; }

    public DateTime ChangeDate { get; set; }

    public bool IsBondFinancePool { get; set; }

    public bool IsSent1711ToCustodian { get; set; }

    public string PnICustodialAcctNum { get; set; }

    public string SubscriberRecordAcctNum { get; set; }

    public string MasterTnIAcctNum { get; set; }

    public string SubservicerIssuerNum { get; set; }

    public string GinniePoolType { get; set; }

    public DateTime InitialDate { get; set; }

    public DateTime LastPaidInstallmentDate { get; set; }

    public DateTime UnpaidBalanceDate { get; set; }

    public string ACHBankAccountPurposeType { get; set; }

    public string ACHABARoutingAndTransitNum { get; set; }

    public string ACHABARoutingAndTransitId { get; set; }

    public string ACHInsitutionTelegraphicName { get; set; }

    public string ACHReceiverSubaccountName { get; set; }

    public string ACHBankAccountDescription { get; set; }

    public string GinniePoolIndexType { get; set; }

    public string PoolIssuerTransferee { get; set; }

    public DateTime PoolCertificatePaymentDate { get; set; }

    public string BondFinanceProgramType { get; set; }

    public string BondFinanceProgramName { get; set; }

    public string GinniePoolClassType { get; set; }

    public bool GinniePoolConcurrentTransferIndicator { get; set; }

    public int PoolCurrentLoanCount { get; set; }

    public long PoolCurrentPrincipalBalAmt { get; set; }

    public string PoolingMethodType { get; set; }

    public DateTime PoolInterestAdjustmentEffectiveDate { get; set; }

    public int PoolMaturityPeriodCount { get; set; }

    public bool DocSubmissionIndicator { get; set; }

    public bool DocReqIndicator { get; set; }

    public long SecurityOrigSubscriptionAmt { get; set; }

    public Decimal MinServicingFee { get; set; }

    public Decimal MaxBU { get; set; }

    public int GetPendingLoanCount(MbsPoolLoanStatus status)
    {
      return this.PendingLoanList == null || !this.PendingLoanList.ContainsKey(status) ? 0 : this.PendingLoanList[status];
    }

    public MbsPoolInfo Duplicate()
    {
      MbsPoolInfo mbsPoolInfo = new MbsPoolInfo(this);
      mbsPoolInfo.IsCloned = true;
      return mbsPoolInfo;
    }

    public void CalcAllPricingDetails(List<GSECommitmentInfo> gseCommitments)
    {
      List<TradeAdvancedPricingItem> advancedPricingItemList = new List<TradeAdvancedPricingItem>();
      foreach (TradeAdvancedPricingItem pricingItem1 in this.Pricing.AdvancedPricingInfo.PricingItems)
      {
        TradeAdvancedPricingItem pricingItem = pricingItem1;
        if (gseCommitments.Any<GSECommitmentInfo>((Func<GSECommitmentInfo, bool>) (c => c.ContractNumber == pricingItem.GSEContractNumber)))
        {
          GSECommitmentInfo gseCommitmentInfo = gseCommitments.Where<GSECommitmentInfo>((Func<GSECommitmentInfo, bool>) (c => c.ContractNumber == pricingItem.GSEContractNumber)).FirstOrDefault<GSECommitmentInfo>();
          if (gseCommitmentInfo.ProductNames.Any<FannieMaeProduct>((Func<FannieMaeProduct, bool>) (p => p.ProductName == pricingItem.ProductName)))
            advancedPricingItemList.Add(MbsPoolCalculation.CalculateTradeAdvancedPricing(this.Coupon, this.GetGuaranteeFee(gseCommitmentInfo, pricingItem.ProductName), this.FixedServicingFeePercent, this.MinServicingFee, this.MaxBU, gseCommitmentInfo.ProductNames.Where<FannieMaeProduct>((Func<FannieMaeProduct, bool>) (p => p.ProductName == pricingItem.ProductName)).First<FannieMaeProduct>().BuyUpDownItems, pricingItem, this.PoolMortgageType));
        }
      }
      this.Pricing.AdvancedPricingInfo.PricingItems.Clear();
      foreach (TradeAdvancedPricingItem advancedPricingItem in advancedPricingItemList)
        this.Pricing.AdvancedPricingInfo.PricingItems.Add(advancedPricingItem);
    }

    public Decimal GetGuaranteeFee(GSECommitmentInfo gseCommitmentInfo, string productName)
    {
      return gseCommitmentInfo == null || gseCommitmentInfo.GuarantyFees == null || !gseCommitmentInfo.GuarantyFees.Any<GuarantyFeeItem>((Func<GuarantyFeeItem, bool>) (p =>
      {
        if (p.ProductName == productName)
        {
          Decimal? couponMax = p.CouponMax;
          Decimal coupon = this.Coupon;
          if (couponMax.GetValueOrDefault() >= coupon & couponMax.HasValue)
            return p.CouponMin <= this.Coupon;
        }
        return false;
      })) ? this.GuaranteeFee : gseCommitmentInfo.GuarantyFees.Where<GuarantyFeeItem>((Func<GuarantyFeeItem, bool>) (p =>
      {
        if (p.ProductName == productName)
        {
          Decimal? couponMax = p.CouponMax;
          Decimal coupon = this.Coupon;
          if (couponMax.GetValueOrDefault() >= coupon & couponMax.HasValue)
            return p.CouponMin <= this.Coupon;
        }
        return false;
      })).First<GuarantyFeeItem>().GuarantyFee;
    }

    public Tuple<Decimal, Decimal> GetServicingGuarantyFeeForNoteRate(
      Decimal noteRate,
      MbsPoolInfo poolInfo,
      string commitmentContracNumber,
      string productName)
    {
      return !this.IsNoteRateAllowed(noteRate) ? (Tuple<Decimal, Decimal>) null : MbsPoolCalculation.GetServicingGuarantyFeeForNoteRate(noteRate, poolInfo, commitmentContracNumber, productName);
    }

    public Decimal GetCPA(GSECommitmentInfo gseCommitmentInfo, string productName)
    {
      return gseCommitmentInfo == null || gseCommitmentInfo.GuarantyFees == null || !gseCommitmentInfo.GuarantyFees.Any<GuarantyFeeItem>((Func<GuarantyFeeItem, bool>) (p =>
      {
        if (p.ProductName == productName)
        {
          Decimal? couponMax = p.CouponMax;
          Decimal coupon = this.Coupon;
          if (couponMax.GetValueOrDefault() >= coupon & couponMax.HasValue)
            return p.CouponMin <= this.Coupon;
        }
        return false;
      })) ? this.CPA : gseCommitmentInfo.GuarantyFees.Where<GuarantyFeeItem>((Func<GuarantyFeeItem, bool>) (p =>
      {
        if (p.ProductName == productName)
        {
          Decimal? couponMax = p.CouponMax;
          Decimal coupon = this.Coupon;
          if (couponMax.GetValueOrDefault() >= coupon & couponMax.HasValue)
            return p.CouponMin <= this.Coupon;
        }
        return false;
      })).First<GuarantyFeeItem>().CPA;
    }

    public Decimal CalculatePriceIndex(PipelineInfo info)
    {
      return this.CalculatePriceIndex(info, true, 0M, "", "");
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice)
    {
      return this.CalculatePriceIndex(info, true, securityPrice, "", "");
    }

    public Decimal CalculatePriceIndex(PipelineInfo info, bool includeSrp, Decimal securityPrice)
    {
      return this.CalculatePriceIndex(info, includeSrp, securityPrice, "", "");
    }

    public Decimal CalculatePriceIndex(
      PipelineInfo info,
      bool includeSrp,
      Decimal securityPrice,
      string commitmentContracNumber,
      string productName,
      Decimal cpa = 0M)
    {
      Decimal noteRate = Utils.ParseDecimal(info.GetField("LoanRate"));
      Decimal priceIndex = this.PoolMortgageType != MbsPoolMortgageType.FannieMaePE ? MbsPoolCalculation.CalculateTotalPrice(securityPrice, noteRate, this.Pricing.AdvancedPricingInfo) : MbsPoolCalculation.CalculateFannieMaePEPoolTotalPrice(securityPrice, noteRate, commitmentContracNumber, productName, this.Pricing.AdvancedPricingInfo, cpa);
      if (priceIndex == 0M)
        return 0M;
      foreach (TradePriceAdjustment priceAdjustment in (List<TradePriceAdjustment>) this.PriceAdjustments)
      {
        if (priceAdjustment.CriterionList.CreateEvaluator().Evaluate(info, FilterEvaluationOption.None))
          priceIndex += priceAdjustment.PriceAdjustment;
      }
      return priceIndex;
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
      return loans == null ? 0M : this.CalculateGainLoss(new List<PipelineInfo>(loans).ToArray());
    }

    public Decimal CalculateGainLoss(PipelineInfo[] loanList)
    {
      Decimal gainLoss = 0M;
      foreach (PipelineInfo loan in loanList)
        gainLoss += this.CalculateProfit(loan, this.WeightedAvgPrice);
      return gainLoss;
    }

    public void ResetCalculatedField(PipelineInfo[] pipelineList)
    {
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
          if (this.RateAdjustment == 0M || this.BuyDownAmount <= 0M)
            return false;
          Decimal d = (this.Pricing.SimplePricingItems[0].Rate - noteRate) / this.RateAdjustment;
          return Math.Floor(d) == d;
        }
        if (noteRate > this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate)
        {
          if (this.RateAdjustment == 0M || this.BuyDownAmount <= 0M)
            return false;
          Decimal d = (this.Pricing.SimplePricingItems[this.Pricing.SimplePricingItems.Count - 1].Rate - noteRate) / this.RateAdjustment;
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

    public Decimal GetBasePriceForNoteRate(
      Decimal noteRate,
      Decimal weightedAvgPrice,
      Decimal cpa = 0M,
      string commitmentContracNumber = null,
      string productName = null)
    {
      if (!this.IsNoteRateAllowed(noteRate))
        return 0M;
      return this.PoolMortgageType == MbsPoolMortgageType.FannieMaePE ? MbsPoolCalculation.CalculateFannieMaePEPoolTotalPrice(weightedAvgPrice, noteRate, commitmentContracNumber, productName, this.Pricing.AdvancedPricingInfo, cpa) : MbsPoolCalculation.CalculateTotalPrice(weightedAvgPrice, noteRate, this.Pricing.AdvancedPricingInfo);
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

    public static bool ComparePricing(MbsPoolInfo trade1, MbsPoolInfo trade2)
    {
      return !(trade1.BuyDownAmount != trade2.BuyDownAmount) && !(trade1.BuyUpAmount != trade2.BuyUpAmount) && !(trade1.RateAdjustment != trade2.RateAdjustment) && !(trade1.PriceAdjustments.ToXml() != trade2.PriceAdjustments.ToXml()) && !(trade1.Pricing.ToXml() != trade2.Pricing.ToXml());
    }

    public MbsPoolCalculation Calculation
    {
      get
      {
        return base.Calculation != null ? (MbsPoolCalculation) base.Calculation : new MbsPoolCalculation((ITradeInfoObject) this);
      }
    }
  }
}
