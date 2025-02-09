// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradeInfo : TradeInfoObj
  {
    private int correspondentMasterID = -1;

    public int CorrespondentMasterID
    {
      get => this.correspondentMasterID;
      set => this.correspondentMasterID = value;
    }

    public string CorrespondentMasterCommitmentNumber { get; set; }

    public new DateTime CommitmentDate { get; set; }

    public DateTime CommitmentDateTime
    {
      get
      {
        return !(this.CommitmentDate != DateTime.MinValue) ? DateTime.MinValue : this.CommitmentDate.Date.AddHours(14.0);
      }
    }

    public CorrespondentTradeCommitmentType CommitmentType { get; set; }

    public string CompanyName { get; set; }

    public CorrespondentMasterDeliveryType DeliveryType { get; set; }

    public string TPOID { get; set; }

    public string OrganizationID { get; set; }

    public DateTime ExpirationDate { get; set; }

    public DateTime DeliveryExpirationDate { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public string AOTSecurityType { get; set; }

    public string AOTSecurityTerm { get; set; }

    public Decimal AOTSecurityCoupon { get; set; }

    public Decimal AOTSecurityPrice { get; set; }

    public DateTime AOTSettlementDate { get; set; }

    private Decimal pairOffAmount { get; set; }

    private Decimal completePercent { get; set; }

    public int ExternalOriginatorManagementID { get; set; }

    public DateTime AOTOriginalTradeDate { get; set; }

    public string AOTOriginalTradeDealer { get; set; }

    public Decimal GainLossAmount { get; set; }

    public string AuthorizedTraderUserId { get; set; }

    public string AuthorizedTraderName { get; set; }

    public string AuthorizedTraderEmail { get; set; }

    public DateTime LastPublishedDateTime { get; set; }

    public CorrespondentTradePairOffs CorrespondentTradePairOffs { get; set; }

    public CorrespondentTradePairOffs CopyOfCorrespondentTradePairOffs { get; set; }

    public Dictionary<CorrespondentTradeLoanStatus, int> PendingLoanList { get; set; }

    public List<LoanSummaryExtension> AssignedLoanList { get; set; }

    public bool AutoCreated { get; set; }

    public string AutoCreateLoanGUID { get; set; }

    public bool OverrideTradeName { get; set; }

    public bool SRPfromPPE { get; set; }

    public bool AdjustmentsfromPPE { get; set; }

    public bool IsWeightedAvgBulkPriceLocked { get; set; }

    public Decimal WeightedAvgBulkPrice { get; set; }

    public bool IsToleranceLocked { get; set; }

    public string FundType { get; set; }

    public string OriginationRepWarrantType { get; set; }

    public string AgencyName { get; set; }

    public string AgencyDeliveryType { get; set; }

    public string DocCustodian { get; set; }

    public Decimal DeliveredAmount { get; set; }

    public Decimal RejectedAmount { get; set; }

    public Decimal PurchasedAmount { get; set; }

    public override TradePairOffs PairOffs
    {
      get => throw new NotImplementedException("Please use TradePairOffs objects instead");
    }

    public Decimal GetTotalPairOffAmount()
    {
      return CorrespondentTradeCalculation.CalculatePairOffAmount(this.CorrespondentTradePairOffs);
    }

    public override TradeStatus Status
    {
      get
      {
        if (this.status == TradeStatus.Open)
          return TradeStatus.Open;
        if (base.Status == TradeStatus.Pending)
          return TradeStatus.Pending;
        if (base.Status == TradeStatus.Voided)
          return TradeStatus.Voided;
        if (base.Status == TradeStatus.Archived)
          return TradeStatus.Archived;
        if (base.Status == TradeStatus.Delivered)
          return TradeStatus.Delivered;
        if (base.Status == TradeStatus.Settled)
          return TradeStatus.Settled;
        if (base.Status == TradeStatus.Unpublished)
          return TradeStatus.Unpublished;
        return base.Status == TradeStatus.Committed || this.CommitmentDate != DateTime.MinValue ? TradeStatus.Committed : TradeStatus.Open;
      }
      set => base.Status = value;
    }

    public TradeStatus CopyOfStatus { get; set; }

    public int GetPendingLoanCount(CorrespondentTradeLoanStatus status)
    {
      return this.PendingLoanList == null || !this.PendingLoanList.ContainsKey(status) ? 0 : this.PendingLoanList[status];
    }

    public CorrespondentTradeInfo Duplicate()
    {
      CorrespondentTradeInfo correspondentTradeInfo = new CorrespondentTradeInfo(this);
      correspondentTradeInfo.IsCloned = true;
      return correspondentTradeInfo;
    }

    public bool IsForIndividualLoan()
    {
      return this.DeliveryType == CorrespondentMasterDeliveryType.IndividualBestEfforts || this.DeliveryType == CorrespondentMasterDeliveryType.IndividualMandatory;
    }

    public CorrespondentTradeInfo(int tradeID, string guid, string name)
      : base(tradeID, guid, name)
    {
      this.TradeType = TradeType.CorrespondentTrade;
    }

    public CorrespondentTradeInfo(CorrespondentTradeInfo source)
      : base(-1, "", "")
    {
      this.CorrespondentMasterCommitmentNumber = source.CorrespondentMasterCommitmentNumber;
      this.correspondentMasterID = source.correspondentMasterID;
      this.ExternalOriginatorManagementID = source.ExternalOriginatorManagementID;
      this.TPOID = source.TPOID;
      this.CompanyName = source.CompanyName;
      this.OrganizationID = source.OrganizationID;
      this.DeliveryType = source.DeliveryType;
      this.TradeAmount = source.TradeAmount;
      this.Tolerance = source.Tolerance;
      this.CommitmentType = source.CommitmentType;
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.Filter = source.Filter;
      this.CorrespondentTradePairOffs = new CorrespondentTradePairOffs();
      this.TradeType = TradeType.CorrespondentTrade;
    }

    public CorrespondentTradeInfo()
    {
      this.ParseTradeObjects("", "", "", "", "", "", "", "", "", "", "", "", "");
      this.TradeType = TradeType.CorrespondentTrade;
      this.CorrespondentTradePairOffs = new CorrespondentTradePairOffs();
    }

    public override void ParseTradeObjects(
      string notes,
      string filterQueryXml,
      string pairOffXml,
      string pricingXml,
      string adjustmentsXml,
      string srpTableXml,
      string investorXml,
      string dealerXml,
      string assigneeXml,
      string buyUpDownXml = "�",
      string productXml = "�",
      string guarantyFeesXml = "�",
      string eppsLoanProgramXml = "�")
    {
      base.ParseTradeObjects(notes, filterQueryXml, pairOffXml, pricingXml, adjustmentsXml, srpTableXml, investorXml, dealerXml, assigneeXml, buyUpDownXml, productXml, guarantyFeesXml, eppsLoanProgramXml);
    }

    public CorrespondentTradeCalculation Calculation
    {
      get
      {
        return base.Calculation != null ? (CorrespondentTradeCalculation) base.Calculation : new CorrespondentTradeCalculation((ITradeInfoObject) this);
      }
    }

    public bool IsNoteRateAllowed(PipelineInfo info)
    {
      return this.IsNoteRateAllowed(Utils.ParseDecimal(info.GetField("LoanRate")));
    }

    public bool IsNoteRateAllowed(Decimal noteRate)
    {
      if (this.IsForIndividualLoan())
        return true;
      if (!this.Pricing.IsAdvancedPricing)
      {
        for (int index = 0; index < this.Pricing.SimplePricingItems.Count; ++index)
        {
          if (this.Pricing.SimplePricingItems[index].Rate == noteRate)
            return true;
        }
      }
      return false;
    }

    public Decimal GetBasePriceForNoteRate(Decimal noteRate, Decimal securityPrice)
    {
      if (!this.IsNoteRateAllowed(noteRate) || this.Pricing.IsAdvancedPricing)
        return 0M;
      for (int index = 0; index < this.Pricing.SimplePricingItems.Count; ++index)
      {
        if (this.Pricing.SimplePricingItems[index].Rate == noteRate)
          return this.Pricing.SimplePricingItems[index].Price;
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

    public static bool ComparePricing(CorrespondentTradeInfo trade1, CorrespondentTradeInfo trade2)
    {
      return !(trade1.TradeAmount != trade2.TradeAmount) && !(trade1.PriceAdjustments.ToXml() != trade2.PriceAdjustments.ToXml()) && !(trade1.Pricing.ToXml() != trade2.Pricing.ToXml()) && !(trade1.SRPTable.ToXml() != trade2.SRPTable.ToXml()) && trade1.CorrespondentTradePairOffs != null && trade2.CorrespondentTradePairOffs != null && trade1.CorrespondentTradePairOffs.ToXml() == trade2.CorrespondentTradePairOffs.ToXml();
    }

    public void ResetPairoffAmount() => this.pairOffAmount = this.PairOffAmount;

    public bool LoansArePurchased(List<LoanSummaryExtension> loans)
    {
      return !loans.Any<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (l =>
      {
        DateTime purchaseDate1 = l.PurchaseDate;
        DateTime purchaseDate2 = l.PurchaseDate;
        if (l.PurchaseDate == DateTime.MinValue)
          return true;
        DateTime purchaseDate3 = l.PurchaseDate;
        return l.PurchaseDate.ToString() == string.Empty;
      }));
    }

    public bool CanBeArchive(List<LoanSummaryExtension> loans)
    {
      return CorrespondentTradeCalculation.CalculateOpenAmount(this.TradeAmount, loans.Sum<LoanSummaryExtension>((Func<LoanSummaryExtension, Decimal>) (l => l.LoanAmount)), CorrespondentTradeCalculation.CalculatePairOffAmount(this.CorrespondentTradePairOffs)) <= this.TradeAmount - this.MinAmount && this.LoansArePurchased(loans);
    }

    public bool CanBeVoided(List<LoanSummaryExtension> loans)
    {
      return !loans.Any<LoanSummaryExtension>((Func<LoanSummaryExtension, bool>) (l => l.SubmittedForReviewDate != DateTime.MinValue));
    }
  }
}
