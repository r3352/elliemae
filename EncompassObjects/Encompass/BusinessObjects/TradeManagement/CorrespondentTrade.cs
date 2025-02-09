// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents a correspondent trade.</summary>
  /// <remarks>Correspondent trade allows correspondent buyers to keep track of individual commitments
  /// from their correspondent sellers.</remarks>
  public class CorrespondentTrade
  {
    /// <summary>Gets or sets EPPS Loan Programs</summary>
    public Dictionary<string, string> EppsLoanProgramList = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets commitment number of the correspondent trade
    /// </summary>
    public string CommitmentNumber { get; set; }

    /// <summary>
    /// Gets or sets commitment date of the correspondent trade
    /// </summary>
    public DateTime CommitmentDate { get; set; }

    /// <summary>
    /// Gets or sets the correspondent master commitment number
    /// </summary>
    public string CorrespondentMasterCommitmentNumber { get; set; }

    /// <summary>
    /// Gets or sets commitment type of the correspondent trade
    /// </summary>
    public CorrespondentTradeCommitmentType CommitmentType { get; set; }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.TradeManagement.CorrespondentMasterDeliveryType" /> of the correspondent trade
    /// </summary>
    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    /// <summary>Gets or sets TPO company name</summary>
    public string CompanyName { get; set; }

    /// <summary>Gets or sets the identifier of TPO company</summary>
    public string TPOID { get; set; }

    /// <summary>Gets or sets organization ID of TPO company</summary>
    public string OrganizationID { get; set; }

    /// <summary>
    /// Gets or sets commitement amount of the correspondent trade
    /// </summary>
    public Decimal TradeAmount { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the correspondent trade
    /// </summary>
    public DateTime ExpirationDate { get; set; }

    /// <summary>Gets or sets the date when the loan must be delivered</summary>
    public DateTime DeliveryExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the total pair-off amount of the correspondent trade
    /// </summary>
    public Decimal TotalPairoffAmount { get; set; }

    /// <summary>
    /// Gets or sets the tolerance percentage of the correspondent trade
    /// </summary>
    public Decimal Tolerance { get; set; }

    /// <summary>
    /// Gets or sets a flag whether the correspondent trade is locked
    /// </summary>
    public bool Locked { get; set; }

    /// <summary>Gets or sets security type of AOT information</summary>
    public string AOTSecurityType { get; set; }

    /// <summary>Gets or sets security term of AOT information</summary>
    public string AOTSecurityTerm { get; set; }

    /// <summary>Gets or sets security coupon of AOT information</summary>
    public Decimal AOTSecurityCoupon { get; set; }

    /// <summary>Gets or sets security price of AOT information</summary>
    public Decimal AOTSecurityPrice { get; set; }

    /// <summary>Gets or sets settlement date of AOT information</summary>
    public DateTime AOTSettlementDate { get; set; }

    /// <summary>Gets or sets original trade date of AOT information</summary>
    public DateTime AOTOriginalTradeDate { get; set; }

    /// <summary>Gets or sets original trade dealer of AOT information</summary>
    public string AOTOriginalTradeDealer { get; set; }

    /// <summary>Gets or sets Min Note Rate value</summary>
    public Decimal? MinNoteRateRange { get; set; }

    /// <summary>Gets or sets Max Note Rate value</summary>
    public Decimal? MaxNoteRateRange { get; set; }

    /// <summary>Gets or sets a list of Base Price settings</summary>
    public Dictionary<Decimal, Decimal> BasePriceItems { get; set; }

    /// <summary>Gets or sets a list of Base Price settings</summary>
    public Dictionary<string, Decimal> PriceAdjustments { get; set; }

    /// <summary>Gets or sets a list of Base Price settings</summary>
    public List<SrpTableItem> SrpTable { get; set; }

    /// <summary>Gets or sets a Adjustment from PPE flag</summary>
    public bool AdjustmentFromProductPricingEngine { get; set; }

    /// <summary>Gets or sets a SRP from PPE flag</summary>
    public bool SRPFromProductPricingEngine { get; set; }

    /// <summary>
    /// Gets and Sets Co-Issue Fund Type of the correspondent trade
    /// </summary>
    public string FundType { get; set; }

    /// <summary>
    /// Gets and Sets Co-Issue Rep Warrant Type of the correspondent trade
    /// </summary>
    public string OriginationRepWarrantType { get; set; }

    /// <summary>
    /// Gets and Sets Co-Issue Agency Name of the correspondent trade
    /// </summary>
    public string AgencyName { get; set; }

    /// <summary>
    /// Gets and Sets Co-Issue Agency Delivery Type of the correspondent trade
    /// </summary>
    public string AgencyDeliveryType { get; set; }

    /// <summary>
    /// Gets and Sets Co-Issue Doc Custodia of the correspondent trade
    /// </summary>
    public string DocCustodian { get; set; }

    /// <summary>Gets or sets Authorized Trader User Id</summary>
    public string AuthorizedTraderUserId { get; set; }

    /// <summary>Gets the identifier of the correspondent trade</summary>
    public int Id { get; set; }

    /// <summary>Gets the Guid of the correspondent trade</summary>
    public string Guid { get; private set; }

    /// <summary>Gets the assigned amount of the correspondent trade</summary>
    public Decimal AssignedAmount { get; private set; }

    /// <summary>Gets the open amount of the correspondent trade</summary>
    public Decimal OpenAmount { get; private set; }

    /// <summary>
    /// Gets how many percent of the correspondent trade has been allocated by assigned loans
    /// </summary>
    public Decimal CompletionPercentage { get; private set; }

    /// <summary>Gets the maximum amount of the correspondent trade</summary>
    public Decimal MaxAmount => this.TradeAmount + this.TradeAmount * this.Tolerance / 100M;

    /// <summary>Gets the minimum amount of the correspondent trade</summary>
    public Decimal MinAmount => this.TradeAmount - this.TradeAmount * this.Tolerance / 100M;

    /// <summary>
    /// Gets the total gain loss amount of the correspondent trade
    /// </summary>
    public Decimal GainLossAmount { get; private set; }

    /// <summary>
    /// Gets the flag that if the correspondent trade has pending loans
    /// </summary>
    public bool HasPendingLoan { get; private set; }

    /// <summary>Gets the status of the correspondent trade</summary>
    public TradeStatus Status { get; private set; }

    /// <summary>Gets the trade description of the correspondent trade</summary>
    public string TradeDescription { get; private set; }

    /// <summary>
    /// Gets the weighted average bulk price of the correspondent trade
    /// </summary>
    public Decimal WeightedAvgBulkPrice { get; private set; }

    /// <summary>
    /// Gets the weighted average bulk price locked of the correspondent trade
    /// </summary>
    public bool IsWeightedAvgBulkPriceLocked { get; private set; }

    /// <summary>Gets the tolerance locked of the correspondent trade</summary>
    public bool IsToleranceLocked { get; set; }

    /// <summary>Gets the text for commitment type</summary>
    public string CommitmentTypeText => this.CommitmentType.ToDescription();

    /// <summary>Gets the text for delivery type</summary>
    public string DeliveryTypeText => this.DeliveryType.ToDescription();

    /// <summary>Gets the text for status</summary>
    public string StatusText => this.Status.ToDescription();

    internal CorrespondentTrade(CorrespondentTradeInfo info)
    {
      this.Id = info.TradeID;
      this.Locked = info.Locked;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = (CorrespondentTradeCommitmentType) info.CommitmentType;
      this.CommitmentNumber = info.Name;
      this.CorrespondentMasterCommitmentNumber = info.CorrespondentMasterCommitmentNumber;
      this.DeliveryExpirationDate = info.DeliveryExpirationDate;
      this.DeliveryType = (CorrespondentMasterDeliveryType) info.DeliveryType;
      this.ExpirationDate = info.ExpirationDate;
      this.HasPendingLoan = info.PendingLoanList.Count<KeyValuePair<EllieMae.EMLite.Trading.CorrespondentTradeLoanStatus, int>>() > 0;
      this.Status = (TradeStatus) info.Status;
      this.TradeDescription = info.TradeDescription;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.IsToleranceLocked = info.IsToleranceLocked;
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.AssignedAmount = info.AssignedLoanList.Where<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (x => x.CorrespondentTradeId > 0)).Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount));
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
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
      this.EppsLoanProgramList = info.EPPSLoanProgramsFilter.ToDictionary<EPPSLoanProgramFilter, string, string>((Func<EPPSLoanProgramFilter, string>) (mc => mc.ProgramId), (Func<EPPSLoanProgramFilter, string>) (mc => mc.ProgramName), (IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
      this.BasePriceItems = new Dictionary<Decimal, Decimal>();
      TradePricingInfo pricing = info.Pricing;
      if (pricing != null && pricing.SimplePricingItems != null && pricing.SimplePricingItems.Count<TradePricingItem>() > 0)
      {
        foreach (TradePricingItem simplePricingItem in pricing.SimplePricingItems)
        {
          if (!this.BasePriceItems.Keys.Contains<Decimal>(simplePricingItem.Rate))
            this.BasePriceItems.Add(simplePricingItem.Rate, simplePricingItem.Price);
        }
      }
      this.PriceAdjustments = new Dictionary<string, Decimal>();
      TradePriceAdjustments priceAdjustments = info.PriceAdjustments;
      if (priceAdjustments != null && priceAdjustments.Count<TradePriceAdjustment>() > 0)
      {
        foreach (TradePriceAdjustment tradePriceAdjustment in (List<TradePriceAdjustment>) priceAdjustments)
        {
          if (!this.PriceAdjustments.Keys.Contains<string>(tradePriceAdjustment.CriterionList.ToString()))
            this.PriceAdjustments.Add(tradePriceAdjustment.CriterionList.ToString(), tradePriceAdjustment.PriceAdjustment);
        }
      }
      this.SrpTable = new List<SrpTableItem>();
      SRPTable srpTable = info.SRPTable;
      if (srpTable == null || srpTable.PricingItems == null || srpTable.PricingItems.Count<SRPTable.PricingItem>() <= 0)
        return;
      foreach (SRPTable.PricingItem pricingItem in srpTable.PricingItems)
      {
        List<SrpStateAdjustment> srpStateAdjustmentList = new List<SrpStateAdjustment>();
        if (pricingItem.StateAdjustments != null && pricingItem.StateAdjustments.Count<SRPTable.StateAdjustment>() > 0)
          srpStateAdjustmentList = pricingItem.StateAdjustments.Select<SRPTable.StateAdjustment, SrpStateAdjustment>((Func<SRPTable.StateAdjustment, SrpStateAdjustment>) (s => new SrpStateAdjustment()
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

    /// <summary>Constructor for the class</summary>
    public CorrespondentTrade() => this.BasePriceItems = new Dictionary<Decimal, Decimal>();

    /// <summary>Constructor of CorrespondentTrade</summary>
    internal CorrespondentTrade(CorrespondentTradeViewModel info)
    {
      this.Id = info.TradeID;
      this.Locked = info.Locked;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = (CorrespondentTradeCommitmentType) Utils.ParseInt((object) info.CommitmentType);
      this.CommitmentNumber = info.Name;
      this.CorrespondentMasterCommitmentNumber = info.CorrespondentMasterCommitmentNumber;
      this.DeliveryExpirationDate = info.DeliveryExpirationDate;
      this.DeliveryType = (CorrespondentMasterDeliveryType) Utils.ParseInt((object) info.DeliveryType);
      this.ExpirationDate = info.ExpirationDate;
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.Status = (TradeStatus) info.Status;
      this.TradeDescription = info.TradeDescription;
      this.WeightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.IsWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.IsToleranceLocked = info.IsToleranceLocked;
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.AssignedAmount = info.AssignedAmount;
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = info.PairOffAmount;
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
      tradeInfo.TradeID = this.Id;
      tradeInfo.Locked = this.Locked;
      tradeInfo.Guid = this.Guid;
      tradeInfo.CommitmentDate = this.CommitmentDate;
      tradeInfo.CommitmentType = (EllieMae.EMLite.Trading.CorrespondentTradeCommitmentType) this.CommitmentType;
      tradeInfo.Name = this.CommitmentNumber;
      tradeInfo.CorrespondentMasterCommitmentNumber = this.CorrespondentMasterCommitmentNumber;
      tradeInfo.DeliveryExpirationDate = this.DeliveryExpirationDate;
      tradeInfo.DeliveryType = (EllieMae.EMLite.Trading.CorrespondentMasterDeliveryType) this.DeliveryType;
      tradeInfo.ExpirationDate = this.ExpirationDate;
      tradeInfo.Status = (EllieMae.EMLite.Trading.TradeStatus) this.Status;
      tradeInfo.TradeDescription = this.TradeDescription;
      tradeInfo.WeightedAvgBulkPrice = this.WeightedAvgBulkPrice;
      tradeInfo.IsWeightedAvgBulkPriceLocked = this.IsWeightedAvgBulkPriceLocked;
      tradeInfo.IsToleranceLocked = this.IsToleranceLocked;
      tradeInfo.TradeAmount = this.TradeAmount;
      tradeInfo.Tolerance = this.Tolerance;
      tradeInfo.MaxAmount = this.MaxAmount;
      tradeInfo.MinAmount = this.MinAmount;
      tradeInfo.OpenAmount = this.OpenAmount;
      tradeInfo.CompletionPercent = this.CompletionPercentage;
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
        tradeInfo.EPPSLoanProgramsFilter.Add(new EPPSLoanProgramFilter(eppsLoanProgram.Key, eppsLoanProgram.Value));
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in this.BasePriceItems)
        tradeInfo.Pricing.SimplePricingItems.Add(new TradePricingItem(basePriceItem.Key, basePriceItem.Value));
      if (this.MinNoteRateRange.HasValue || this.MaxNoteRateRange.HasValue)
        tradeInfo.Filter = new TradeFilter(new SimpleTradeFilter(false)
        {
          NoteRateRange = Range<Decimal>.Parse(!this.MinNoteRateRange.HasValue ? "" : this.MinNoteRateRange.ToString(), !this.MaxNoteRateRange.HasValue ? "" : this.MaxNoteRateRange.ToString(), Decimal.MinValue, Decimal.MaxValue)
        });
      return tradeInfo;
    }
  }
}
