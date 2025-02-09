// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class CorrespondentTrade
  {
    public Dictionary<string, string> EppsLoanProgramList = new Dictionary<string, string>();

    public string CommitmentNumber { get; set; }

    public DateTime CommitmentDate { get; set; }

    public string CorrespondentMasterCommitmentNumber { get; set; }

    public CorrespondentTradeCommitmentType CommitmentType { get; set; }

    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    public string CompanyName { get; set; }

    public string TPOID { get; set; }

    public string OrganizationID { get; set; }

    public Decimal TradeAmount { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime DeliveryExpirationDate { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public Decimal Tolerance { get; set; }

    public bool Locked { get; set; }

    public string AOTSecurityType { get; set; }

    public string AOTSecurityTerm { get; set; }

    public Decimal AOTSecurityCoupon { get; set; }

    public Decimal AOTSecurityPrice { get; set; }

    public DateTime AOTSettlementDate { get; set; }

    public DateTime AOTOriginalTradeDate { get; set; }

    public string AOTOriginalTradeDealer { get; set; }

    public Decimal? MinNoteRateRange { get; set; }

    public Decimal? MaxNoteRateRange { get; set; }

    public Dictionary<Decimal, Decimal> BasePriceItems { get; set; }

    public Dictionary<string, Decimal> PriceAdjustments { get; set; }

    public List<SrpTableItem> SrpTable { get; set; }

    public bool AdjustmentFromProductPricingEngine { get; set; }

    public bool SRPFromProductPricingEngine { get; set; }

    public string FundType { get; set; }

    public string OriginationRepWarrantType { get; set; }

    public string AgencyName { get; set; }

    public string AgencyDeliveryType { get; set; }

    public string DocCustodian { get; set; }

    public string AuthorizedTraderUserId { get; set; }

    public int Id { get; set; }

    public string Guid { get; private set; }

    public Decimal AssignedAmount { get; private set; }

    public Decimal OpenAmount { get; private set; }

    public Decimal CompletionPercentage { get; private set; }

    public Decimal MaxAmount => this.TradeAmount + this.TradeAmount * this.Tolerance / 100M;

    public Decimal MinAmount => this.TradeAmount - this.TradeAmount * this.Tolerance / 100M;

    public Decimal GainLossAmount { get; private set; }

    public bool HasPendingLoan { get; private set; }

    public TradeStatus Status { get; private set; }

    public string TradeDescription { get; private set; }

    public Decimal WeightedAvgBulkPrice { get; private set; }

    public bool IsWeightedAvgBulkPriceLocked { get; private set; }

    public bool IsToleranceLocked { get; set; }

    public string CommitmentTypeText => this.CommitmentType.ToDescription();

    public string DeliveryTypeText => this.DeliveryType.ToDescription();

    public string StatusText => this.Status.ToDescription();

    internal CorrespondentTrade(CorrespondentTradeInfo info)
    {
      this.Id = ((TradeBase) info).TradeID;
      this.Locked = ((TradeInfoObj) info).Locked;
      this.Guid = ((TradeBase) info).Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = (CorrespondentTradeCommitmentType) info.CommitmentType;
      this.CommitmentNumber = ((TradeBase) info).Name;
      this.CorrespondentMasterCommitmentNumber = info.CorrespondentMasterCommitmentNumber;
      this.DeliveryExpirationDate = info.DeliveryExpirationDate;
      this.DeliveryType = (CorrespondentMasterDeliveryType) info.DeliveryType;
      this.ExpirationDate = info.ExpirationDate;
      this.HasPendingLoan = info.PendingLoanList.Count<KeyValuePair<CorrespondentTradeLoanStatus, int>>() > 0;
      this.Status = (TradeStatus) ((TradeInfoObj) info).Status;
      this.TradeDescription = ((TradeInfoObj) info).TradeDescription;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.IsToleranceLocked = info.IsToleranceLocked;
      this.TradeAmount = ((TradeInfoObj) info).TradeAmount;
      this.Tolerance = ((TradeInfoObj) info).Tolerance;
      this.AssignedAmount = info.AssignedLoanList.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (x => x.CorrespondentTradeId > 0)).Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount));
      this.OpenAmount = ((TradeInfoObj) info).OpenAmount;
      this.CompletionPercentage = ((TradeInfoObj) info).CompletionPercent;
      this.TotalPairoffAmount = info.TotalPairoffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.OrganizationID = info.OrganizationID;
      this.CompanyName = info.CompanyName;
      this.TPOID = info.TPOID;
      this.AOTOriginalTradeDate = info.AOTOriginalTradeDate;
      this.AOTOriginalTradeDealer = info.AOTOriginalTradeDealer;
      this.AOTSecurityCoupon = info.AOTSecurityCoupon;
      this.AOTSecurityPrice = info.AOTSecurityPrice;
      this.AOTSecurityTerm = info.AOTSecurityTerm;
      this.AOTSecurityType = info.AOTSecurityType;
      this.AOTSettlementDate = info.AOTSettlementDate;
      this.FundType = info.FundType;
      this.OriginationRepWarrantType = info.OriginationRepWarrantType;
      this.AgencyName = info.AgencyName;
      this.AgencyDeliveryType = info.AgencyDeliveryType;
      this.DocCustodian = info.DocCustodian;
      this.AuthorizedTraderUserId = info.AuthorizedTraderUserId;
      this.AdjustmentFromProductPricingEngine = info.AdjustmentsfromPPE;
      this.SRPFromProductPricingEngine = info.SRPfromPPE;
      this.EppsLoanProgramList = ((IEnumerable<EPPSLoanProgramFilter>) ((TradeInfoObj) info).EPPSLoanProgramsFilter).ToDictionary<EPPSLoanProgramFilter, string, string>((Func<EPPSLoanProgramFilter, string>) (mc => mc.ProgramId), (Func<EPPSLoanProgramFilter, string>) (mc => mc.ProgramName), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.BasePriceItems = new Dictionary<Decimal, Decimal>();
      TradePricingInfo pricing = ((TradeInfoObj) info).Pricing;
      if (pricing != null && pricing.SimplePricingItems != null && ((IEnumerable<TradePricingItem>) pricing.SimplePricingItems).Count<TradePricingItem>() > 0)
      {
        foreach (TradePricingItem simplePricingItem in pricing.SimplePricingItems)
        {
          if (!this.BasePriceItems.Keys.Contains<Decimal>(simplePricingItem.Rate))
            this.BasePriceItems.Add(simplePricingItem.Rate, simplePricingItem.Price);
        }
      }
      this.PriceAdjustments = new Dictionary<string, Decimal>();
      TradePriceAdjustments priceAdjustments = ((TradeInfoObj) info).PriceAdjustments;
      if (priceAdjustments != null && ((IEnumerable<TradePriceAdjustment>) priceAdjustments).Count<TradePriceAdjustment>() > 0)
      {
        foreach (TradePriceAdjustment tradePriceAdjustment in (List<TradePriceAdjustment>) priceAdjustments)
        {
          if (!this.PriceAdjustments.Keys.Contains<string>(tradePriceAdjustment.CriterionList.ToString()))
            this.PriceAdjustments.Add(tradePriceAdjustment.CriterionList.ToString(), tradePriceAdjustment.PriceAdjustment);
        }
      }
      this.SrpTable = new List<SrpTableItem>();
      SRPTable srpTable = ((TradeInfoObj) info).SRPTable;
      if (srpTable == null || srpTable.PricingItems == null || ((IEnumerable<SRPTable.PricingItem>) srpTable.PricingItems).Count<SRPTable.PricingItem>() <= 0)
        return;
      foreach (SRPTable.PricingItem pricingItem in srpTable.PricingItems)
      {
        List<SrpStateAdjustment> srpStateAdjustmentList = new List<SrpStateAdjustment>();
        if (pricingItem.StateAdjustments != null && ((IEnumerable<SRPTable.StateAdjustment>) pricingItem.StateAdjustments).Count<SRPTable.StateAdjustment>() > 0)
          srpStateAdjustmentList = ((IEnumerable<SRPTable.StateAdjustment>) pricingItem.StateAdjustments).Select<SRPTable.StateAdjustment, SrpStateAdjustment>((Func<SRPTable.StateAdjustment, SrpStateAdjustment>) (s => new SrpStateAdjustment()
          {
            StateFullName = s.State,
            SrpAdjustment = s.Adjustment,
            SrpIfWaived = s.ImpoundAdjustment
          })).ToList<SrpStateAdjustment>();
        this.SrpTable.Add(new SrpTableItem()
        {
          MinLoanAmount = pricingItem.LoanAmount.Minimum,
          MaxLoanAmount = pricingItem.LoanAmount.Maximum,
          BaseSrp = pricingItem.BaseAdjustment,
          BaseSrpIfWaived = pricingItem.ImpoundsAdjustment,
          SrpAdjustmentsByState = srpStateAdjustmentList
        });
      }
    }

    public CorrespondentTrade() => this.BasePriceItems = new Dictionary<Decimal, Decimal>();

    internal CorrespondentTrade(CorrespondentTradeViewModel info)
    {
      this.Id = ((TradeBase) info).TradeID;
      this.Locked = info.Locked;
      this.Guid = ((TradeBase) info).Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = (CorrespondentTradeCommitmentType) Utils.ParseInt((object) info.CommitmentType, false, -1);
      this.CommitmentNumber = ((TradeBase) info).Name;
      this.CorrespondentMasterCommitmentNumber = info.CorrespondentMasterCommitmentNumber;
      this.DeliveryExpirationDate = info.DeliveryExpirationDate;
      this.DeliveryType = (CorrespondentMasterDeliveryType) Utils.ParseInt((object) info.DeliveryType, false, -1);
      this.ExpirationDate = info.ExpirationDate;
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.Status = (TradeStatus) ((TradeViewModel) info).Status;
      this.TradeDescription = info.TradeDescription;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.IsToleranceLocked = info.IsToleranceLocked;
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.AssignedAmount = info.AssignedAmount;
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = ((TradeViewModel) info).PairOffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.FundType = info.FundType;
      this.OriginationRepWarrantType = info.OriginationRepWarrantType;
      this.AgencyName = info.AgencyName;
      this.AgencyDeliveryType = info.AgencyDeliveryType;
      this.DocCustodian = info.DocCustodian;
      this.AuthorizedTraderUserId = info.AuthorizedTraderUserId;
      this.OrganizationID = info.OrganizationID;
      this.CompanyName = info.CompanyName;
      this.TPOID = info.TPOID;
      this.AOTOriginalTradeDate = info.AOTOriginalTradeDate;
      this.AOTOriginalTradeDealer = info.AOTOriginalTradeDealer;
      this.AOTSecurityCoupon = info.AOTSecurityCoupon;
      this.AOTSecurityPrice = info.AOTSecurityPrice;
      this.AOTSecurityTerm = info.AOTSecurityTerm;
      this.AOTSecurityType = info.AOTSecurityType;
      this.AOTSettlementDate = info.AOTSettlementDate;
    }

    internal CorrespondentTradeInfo GetTradeInfo()
    {
      CorrespondentTradeInfo tradeInfo = new CorrespondentTradeInfo();
      ((TradeBase) tradeInfo).TradeID = this.Id;
      ((TradeInfoObj) tradeInfo).Locked = this.Locked;
      ((TradeBase) tradeInfo).Guid = this.Guid;
      tradeInfo.CommitmentDate = this.CommitmentDate;
      tradeInfo.CommitmentType = (CorrespondentTradeCommitmentType) this.CommitmentType;
      ((TradeBase) tradeInfo).Name = this.CommitmentNumber;
      tradeInfo.CorrespondentMasterCommitmentNumber = this.CorrespondentMasterCommitmentNumber;
      tradeInfo.DeliveryExpirationDate = this.DeliveryExpirationDate;
      tradeInfo.DeliveryType = (CorrespondentMasterDeliveryType) this.DeliveryType;
      tradeInfo.ExpirationDate = this.ExpirationDate;
      ((TradeInfoObj) tradeInfo).Status = (TradeStatus) this.Status;
      ((TradeInfoObj) tradeInfo).TradeDescription = this.TradeDescription;
      tradeInfo.WeightedAvgBulkPrice = this.WeightedAvgBulkPrice;
      tradeInfo.IsWeightedAvgBulkPriceLocked = this.IsWeightedAvgBulkPriceLocked;
      tradeInfo.IsToleranceLocked = this.IsToleranceLocked;
      ((TradeInfoObj) tradeInfo).TradeAmount = this.TradeAmount;
      ((TradeInfoObj) tradeInfo).Tolerance = this.Tolerance;
      ((TradeInfoObj) tradeInfo).MaxAmount = this.MaxAmount;
      ((TradeInfoObj) tradeInfo).MinAmount = this.MinAmount;
      ((TradeInfoObj) tradeInfo).OpenAmount = this.OpenAmount;
      ((TradeInfoObj) tradeInfo).CompletionPercent = this.CompletionPercentage;
      tradeInfo.TotalPairoffAmount = this.TotalPairoffAmount;
      tradeInfo.GainLossAmount = this.GainLossAmount;
      tradeInfo.OrganizationID = this.OrganizationID;
      tradeInfo.CompanyName = this.CompanyName;
      tradeInfo.TPOID = this.TPOID;
      tradeInfo.AOTOriginalTradeDate = this.AOTOriginalTradeDate;
      tradeInfo.AOTOriginalTradeDealer = this.AOTOriginalTradeDealer;
      tradeInfo.AOTSecurityCoupon = this.AOTSecurityCoupon;
      tradeInfo.AOTSecurityPrice = this.AOTSecurityPrice;
      tradeInfo.AOTSecurityTerm = this.AOTSecurityTerm;
      tradeInfo.AOTSecurityType = this.AOTSecurityType;
      tradeInfo.AOTSettlementDate = this.AOTSettlementDate;
      tradeInfo.FundType = this.FundType;
      tradeInfo.OriginationRepWarrantType = this.OriginationRepWarrantType;
      tradeInfo.AgencyName = this.AgencyName;
      tradeInfo.AgencyDeliveryType = this.AgencyDeliveryType;
      tradeInfo.DocCustodian = this.DocCustodian;
      tradeInfo.AuthorizedTraderUserId = this.AuthorizedTraderUserId;
      tradeInfo.AdjustmentsfromPPE = this.AdjustmentFromProductPricingEngine;
      tradeInfo.SRPfromPPE = this.SRPFromProductPricingEngine;
      foreach (KeyValuePair<string, string> eppsLoanProgram in this.EppsLoanProgramList)
        ((TradeInfoObj) tradeInfo).EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(eppsLoanProgram.Key, eppsLoanProgram.Value));
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in this.BasePriceItems)
        ((TradeInfoObj) tradeInfo).Pricing.SimplePricingItems.Add(new TradePricingItem(basePriceItem.Key, basePriceItem.Value, 0M));
      if (this.MinNoteRateRange.HasValue || this.MaxNoteRateRange.HasValue)
        ((TradeInfoObj) tradeInfo).Filter = new TradeFilter(new SimpleTradeFilter(false)
        {
          NoteRateRange = Range<Decimal>.Parse(!this.MinNoteRateRange.HasValue ? "" : this.MinNoteRateRange.ToString(), !this.MaxNoteRateRange.HasValue ? "" : this.MaxNoteRateRange.ToString(), Decimal.MinValue, Decimal.MaxValue)
        });
      return tradeInfo;
    }
  }
}
