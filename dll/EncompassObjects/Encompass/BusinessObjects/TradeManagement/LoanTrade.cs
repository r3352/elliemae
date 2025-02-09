// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class LoanTrade
  {
    private List<string> loanPrograms = new List<string>();
    private List<string> mileStones = new List<string>();
    private List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus> occupancyStatus = new List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus>();
    private List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus> investorStatus = new List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus>();
    private bool isWeightedAvgBulkPriceLocked;
    private Decimal weightedAvgBulkPrice;

    public string CommitmentNumber { get; set; }

    public DateTime CommitmentDate { get; set; }

    public string CommitmentType { get; set; }

    public string TradeDescription { get; set; }

    public string MasterContractNumber { get; set; }

    public Decimal TradeAmount { get; set; }

    public DateTime TargetDeliveryDate { get; set; }

    public DateTime ActualDeliveryDate { get; set; }

    public DateTime EarlyDeliveryDate { get; set; }

    public Decimal TotalPairoffAmount { get; set; }

    public Decimal Tolerance { get; set; }

    public bool Locked { get; set; }

    public string InvestorName { get; set; }

    public string InvestorCommitmentNumber { get; set; }

    public DateTime InvestorDeliveryDate { get; set; }

    public string InvestorTradeNumber { get; set; }

    public DateTime LastModified { get; set; }

    public int LoanCount { get; set; }

    public Decimal MiscFee { get; set; }

    public Decimal NetProfit { get; set; }

    public DateTime PurchaseDate { get; set; }

    public Decimal RateAdjustment { get; set; }

    public Decimal BuyUp { get; set; }

    public Decimal BuyDown { get; set; }

    public string Servicer { get; set; }

    public ServicingType ServicingType { get; set; }

    public Decimal TotalProfit { get; set; }

    public DateTime ShipmentDate { get; set; }

    public DateTime AssignedDate { get; set; }

    public string AssignedSecurityId { get; set; }

    public Decimal PairOffAmount1 { get; set; }

    public Decimal PairOffFee1 { get; set; }

    public Decimal PairOffGainLoss1 { get; set; }

    public DateTime PairOffDate1 { get; set; }

    public Decimal PairOffAmount2 { get; set; }

    public Decimal PairOffFee2 { get; set; }

    public Decimal PairOffGainLoss2 { get; set; }

    public DateTime PairOffDate2 { get; set; }

    public Decimal PairOffAmount3 { get; set; }

    public Decimal PairOffFee3 { get; set; }

    public Decimal PairOffGainLoss3 { get; set; }

    public DateTime PairOffDate3 { get; set; }

    public Decimal PairOffAmount4 { get; set; }

    public Decimal PairOffFee4 { get; set; }

    public DateTime PairOffDate4 { get; set; }

    public Decimal PairOffGainLoss4 { get; set; }

    public Dictionary<Decimal, Decimal> BasePriceItems { get; set; }

    public bool IsWeightedAvgBulkPriceLocked
    {
      get => this.isWeightedAvgBulkPriceLocked;
      set
      {
        if (this.IsBulkDelivery)
          this.isWeightedAvgBulkPriceLocked = value;
        else if (this.IsBulkDelivery & value)
          throw new Exception("Weighted Average Bulk Price lock can not be unlocked unless IsBulkDelivery is set to true.");
      }
    }

    public Decimal WeightedAvgBulkPrice
    {
      get => this.weightedAvgBulkPrice;
      set
      {
        if (!this.IsBulkDelivery || !this.isWeightedAvgBulkPriceLocked)
          throw new Exception("WeightedAvgBulkPrice can only be set to value if IsBulkDelivery and WeightedAvgBulkPrice are set to true.");
        this.weightedAvgBulkPrice = value;
      }
    }

    public bool IsBulkDelivery { get; set; }

    public List<string> LoanPrograms
    {
      get => this.loanPrograms;
      set => this.loanPrograms = value;
    }

    public List<string> MileStones
    {
      get => this.mileStones;
      set => this.mileStones = value;
    }

    public Decimal? MinNoteRateRange { get; set; }

    public Decimal? MaxNoteRateRange { get; set; }

    public int? MinTermRange { get; set; }

    public int? MaxTermRange { get; set; }

    public Decimal? MinLTVRange { get; set; }

    public Decimal? MaxLTVRange { get; set; }

    public List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus> OccupancyStatus
    {
      get => this.occupancyStatus;
      set => this.occupancyStatus = value;
    }

    public List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus> InvestorStatus
    {
      get => this.investorStatus;
      set => this.investorStatus = value;
    }

    public int Id { get; private set; }

    public string Guid { get; private set; }

    public Decimal OpenAmount { get; private set; }

    public Decimal CompletionPercentage { get; private set; }

    public Decimal MaxAmount { get; private set; }

    public Decimal MinAmount { get; private set; }

    public Decimal AssignedAmount { get; private set; }

    public Decimal GainLossAmount { get; private set; }

    public bool HasPendingLoan { get; private set; }

    public TradeStatus Status { get; private set; }

    public string StatusText => this.Status.ToDescription();

    public string ServicingTypeText => this.ServicingType.ToDescription();

    internal LoanTrade(LoanTradeInfo info)
    {
      this.Id = ((TradeBase) info).TradeID;
      this.Locked = ((TradeInfoObj) info).Locked;
      this.Guid = ((TradeBase) info).Guid;
      this.CommitmentDate = ((TradeInfoObj) info).CommitmentDate;
      this.CommitmentType = ((TradeInfoObj) info).CommitmentType;
      this.CommitmentNumber = ((TradeBase) info).Name;
      this.MasterContractNumber = info.ContractNumber;
      this.LastModified = info.LastModified;
      this.Status = (TradeStatus) ((TradeInfoObj) info).Status;
      this.TradeDescription = ((TradeInfoObj) info).TradeDescription;
      this.HasPendingLoan = info.HasPendingLoan();
      this.LoanCount = info.LoanCount;
      this.InvestorName = ((TradeInfoObj) info).Investor.Name;
      this.InvestorCommitmentNumber = info.InvestorCommitmentNumber;
      this.InvestorTradeNumber = info.InvestorTradeNumber;
      this.TargetDeliveryDate = ((TradeInfoObj) info).TargetDeliveryDate;
      this.EarlyDeliveryDate = ((TradeInfoObj) info).EarlyDeliveryDate;
      this.InvestorDeliveryDate = ((TradeInfoObj) info).InvestorDeliveryDate;
      this.PurchaseDate = ((TradeInfoObj) info).PurchaseDate;
      this.ShipmentDate = ((TradeInfoObj) info).ShipmentDate;
      this.Servicer = info.Servicer;
      this.ServicingType = (ServicingType) info.ServicingType;
      this.BuyDown = info.BuyDownAmount;
      this.BuyUp = info.BuyUpAmount;
      this.RateAdjustment = info.RateAdjustment;
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
      this.TradeAmount = ((TradeInfoObj) info).TradeAmount;
      this.Tolerance = ((TradeInfoObj) info).Tolerance;
      this.MinAmount = (Decimal) (int) Math.Round(((TradeInfoObj) info).MinAmount, 0);
      this.MaxAmount = (Decimal) (int) Math.Round(((TradeInfoObj) info).MaxAmount, 0);
      this.OpenAmount = ((TradeInfoObj) info).OpenAmount;
      this.CompletionPercentage = ((TradeInfoObj) info).CompletionPercent;
      this.TotalPairoffAmount = ((TradeInfoObj) info).PairOffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.MiscFee = info.MiscAdjustment;
      this.NetProfit = info.NetProfit;
      this.PairOffAmount1 = info.LoanTradePairOffs[0].TradeAmount;
      this.PairOffDate1 = info.LoanTradePairOffs[0].Date;
      this.PairOffFee1 = info.LoanTradePairOffs[0].PairOffFeePercentage;
      this.PairOffGainLoss1 = info.LoanTradePairOffs[0].CalculatedPairOffFee;
      this.PairOffAmount2 = info.LoanTradePairOffs[1].TradeAmount;
      this.PairOffDate2 = info.LoanTradePairOffs[1].Date;
      this.PairOffFee2 = info.LoanTradePairOffs[1].PairOffFeePercentage;
      this.PairOffGainLoss2 = info.LoanTradePairOffs[1].CalculatedPairOffFee;
      this.PairOffAmount3 = info.LoanTradePairOffs[2].TradeAmount;
      this.PairOffDate3 = info.LoanTradePairOffs[2].Date;
      this.PairOffFee3 = info.LoanTradePairOffs[2].PairOffFeePercentage;
      this.PairOffGainLoss3 = info.LoanTradePairOffs[2].CalculatedPairOffFee;
      this.PairOffAmount4 = info.LoanTradePairOffs[3].TradeAmount;
      this.PairOffDate4 = info.LoanTradePairOffs[3].Date;
      this.PairOffFee4 = info.LoanTradePairOffs[3].PairOffFeePercentage;
      this.PairOffGainLoss4 = info.LoanTradePairOffs[3].CalculatedPairOffFee;
      this.IsBulkDelivery = info.IsBulkDelivery;
      this.weightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.isWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      if (((TradeInfoObj) info).Filter.FilterType != 1)
        return;
      SimpleTradeFilter simpleFilter = ((TradeInfoObj) info).Filter.GetSimpleFilter();
      this.loanPrograms = ((IEnumerable<string>) simpleFilter.LoanPrograms).ToList<string>();
      this.mileStones = ((IEnumerable<string>) simpleFilter.Milestones).ToList<string>();
      if (simpleFilter.NoteRateRange != null)
      {
        this.MinNoteRateRange = new Decimal?(simpleFilter.NoteRateRange.Minimum);
        this.MaxNoteRateRange = new Decimal?(simpleFilter.NoteRateRange.Maximum);
      }
      if (simpleFilter.TermRange != null)
      {
        this.MinTermRange = new int?(simpleFilter.TermRange.Minimum);
        this.MaxTermRange = new int?(simpleFilter.TermRange.Maximum);
      }
      if (simpleFilter.LTVRange != null)
      {
        this.MinLTVRange = new Decimal?(simpleFilter.LTVRange.Minimum);
        this.MaxLTVRange = new Decimal?(simpleFilter.LTVRange.Maximum);
      }
      if (simpleFilter.occupancyStatuses != null)
      {
        foreach (string occupancyStatuse in (List<string>) simpleFilter.OccupancyStatuses)
        {
          if (occupancyStatuse.ToLower() == "primaryresidence")
            this.occupancyStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus.Primary);
          if (occupancyStatuse.ToLower() == "secondhome")
            this.occupancyStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus.Secondary);
          if (occupancyStatuse.ToLower() == "investor")
            this.occupancyStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus.Investoment);
        }
      }
      if (simpleFilter.InvestorStatuses == null)
        return;
      foreach (string investorStatuse in (List<string>) simpleFilter.InvestorStatuses)
      {
        if (investorStatuse.ToLower() == "assignedflow")
          this.InvestorStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus.AssignedFlow);
        if (investorStatuse.ToLower() == "assignedbulk")
          this.InvestorStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus.AssignedBulk);
        if (investorStatuse.ToLower() == "rejected")
          this.InvestorStatus.Add(EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus.Unassigned);
      }
    }

    internal LoanTradeInfo GetLoanTradeInfo()
    {
      LoanTradeInfo loanTradeInfo = new LoanTradeInfo();
      ((TradeInfoObj) loanTradeInfo).Archived = false;
      loanTradeInfo.BuyDownAmount = this.BuyDown;
      loanTradeInfo.BuyUpAmount = this.BuyUp;
      ((TradeInfoObj) loanTradeInfo).CommitmentDate = this.CommitmentDate;
      ((TradeInfoObj) loanTradeInfo).CommitmentType = this.CommitmentType;
      ((TradeBase) loanTradeInfo).Name = this.CommitmentNumber;
      ((TradeInfoObj) loanTradeInfo).EarlyDeliveryDate = this.EarlyDeliveryDate;
      loanTradeInfo.InvestorCommitmentNumber = this.InvestorCommitmentNumber;
      ((TradeInfoObj) loanTradeInfo).InvestorDeliveryDate = this.InvestorDeliveryDate;
      ((TradeInfoObj) loanTradeInfo).InvestorName = this.InvestorName;
      loanTradeInfo.InvestorTradeNumber = this.InvestorTradeNumber;
      loanTradeInfo.IsBulkDelivery = this.IsBulkDelivery;
      loanTradeInfo.IsWeightedAvgBulkPriceLocked = this.IsWeightedAvgBulkPriceLocked;
      loanTradeInfo.LastModified = this.LastModified;
      loanTradeInfo.ContractNumber = this.MasterContractNumber;
      loanTradeInfo.MiscAdjustment = this.MiscFee;
      loanTradeInfo.NetProfit = this.NetProfit;
      loanTradeInfo.RateAdjustment = this.RateAdjustment;
      loanTradeInfo.Servicer = this.Servicer;
      loanTradeInfo.ServicingType = (ServicingType) this.ServicingType;
      ((TradeInfoObj) loanTradeInfo).ShipmentDate = this.ActualDeliveryDate;
      ((TradeInfoObj) loanTradeInfo).Status = (TradeStatus) this.Status;
      ((TradeInfoObj) loanTradeInfo).TargetDeliveryDate = this.TargetDeliveryDate;
      ((TradeInfoObj) loanTradeInfo).Tolerance = this.Tolerance;
      ((TradeInfoObj) loanTradeInfo).PairOffAmount = this.TotalPairoffAmount;
      ((TradeInfoObj) loanTradeInfo).PurchaseDate = this.PurchaseDate;
      ((TradeInfoObj) loanTradeInfo).TotalPairOffGainLoss = this.TotalProfit;
      ((TradeInfoObj) loanTradeInfo).TradeAmount = this.TradeAmount;
      ((TradeInfoObj) loanTradeInfo).TradeDescription = this.TradeDescription;
      loanTradeInfo.WeightedAvgBulkPrice = this.WeightedAvgBulkPrice;
      SimpleTradeFilter simpleTradeFilter1 = new SimpleTradeFilter(true);
      foreach (string loanProgram in this.LoanPrograms)
        ((List<string>) simpleTradeFilter1.LoanPrograms).Add(loanProgram);
      foreach (string mileStone in this.MileStones)
        ((List<string>) simpleTradeFilter1.Milestones).Add(mileStone);
      foreach (EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus occupancyStatu in this.OccupancyStatus)
      {
        if (!((List<string>) simpleTradeFilter1.OccupancyStatuses).Contains(occupancyStatu.ToDescription()))
          ((List<string>) simpleTradeFilter1.OccupancyStatuses).Add(occupancyStatu.ToDescription());
      }
      foreach (EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus investorStatu in this.InvestorStatus)
      {
        if (!((List<string>) simpleTradeFilter1.InvestorStatuses).Contains(investorStatu.ToDescription()))
          ((List<string>) simpleTradeFilter1.InvestorStatuses).Add(investorStatu.ToDescription());
      }
      Decimal? nullable1;
      if (this.MinNoteRateRange.HasValue || this.MaxNoteRateRange.HasValue)
      {
        SimpleTradeFilter simpleTradeFilter2 = simpleTradeFilter1;
        nullable1 = this.MinNoteRateRange;
        string str1;
        if (nullable1.HasValue)
        {
          nullable1 = this.MinNoteRateRange;
          str1 = nullable1.ToString();
        }
        else
          str1 = "";
        nullable1 = this.MaxNoteRateRange;
        string str2;
        if (nullable1.HasValue)
        {
          nullable1 = this.MaxNoteRateRange;
          str2 = nullable1.ToString();
        }
        else
          str2 = "";
        Range<Decimal> range = Range<Decimal>.Parse(str1, str2, Decimal.MinValue, Decimal.MaxValue);
        simpleTradeFilter2.NoteRateRange = range;
      }
      if (this.MinTermRange.HasValue || this.MaxTermRange.HasValue)
      {
        SimpleTradeFilter simpleTradeFilter3 = simpleTradeFilter1;
        int? nullable2 = this.MinTermRange;
        string str3;
        if (nullable2.HasValue)
        {
          nullable2 = this.MinTermRange;
          str3 = nullable2.ToString();
        }
        else
          str3 = "";
        nullable2 = this.MaxTermRange;
        string str4;
        if (nullable2.HasValue)
        {
          nullable2 = this.MaxTermRange;
          str4 = nullable2.ToString();
        }
        else
          str4 = "";
        Range<int> range = Range<int>.Parse(str3, str4, int.MinValue, int.MaxValue);
        simpleTradeFilter3.TermRange = range;
      }
      nullable1 = this.MinLTVRange;
      if (!nullable1.HasValue)
      {
        nullable1 = this.MaxLTVRange;
        if (!nullable1.HasValue)
          goto label_47;
      }
      SimpleTradeFilter simpleTradeFilter4 = simpleTradeFilter1;
      nullable1 = this.MinLTVRange;
      string str5;
      if (nullable1.HasValue)
      {
        nullable1 = this.MinLTVRange;
        str5 = nullable1.ToString();
      }
      else
        str5 = "";
      nullable1 = this.MaxLTVRange;
      string str6;
      if (nullable1.HasValue)
      {
        nullable1 = this.MaxLTVRange;
        str6 = nullable1.ToString();
      }
      else
        str6 = "";
      Range<Decimal> range1 = Range<Decimal>.Parse(str5, str6, -2147483648M, 2147483647M);
      simpleTradeFilter4.LTVRange = range1;
label_47:
      ((TradeInfoObj) loanTradeInfo).Filter = new TradeFilter(simpleTradeFilter1);
      if (this.PairOffDate1 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(1, this.PairOffDate1, this.PairOffAmount1, this.PairOffFee1));
      if (this.PairOffDate2 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(2, this.PairOffDate2, this.PairOffAmount2, this.PairOffFee2));
      if (this.PairOffDate3 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(3, this.PairOffDate3, this.PairOffAmount3, this.PairOffFee3));
      if (this.PairOffDate4 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(4, this.PairOffDate4, this.PairOffAmount4, this.PairOffFee4));
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in this.BasePriceItems)
        ((TradeInfoObj) loanTradeInfo).Pricing.SimplePricingItems.Add(new TradePricingItem(basePriceItem.Key, basePriceItem.Value, 0M));
      return loanTradeInfo;
    }

    public LoanTrade() => this.BasePriceItems = new Dictionary<Decimal, Decimal>();

    internal LoanTrade(LoanTradeViewModel info)
    {
      this.Id = ((TradeBase) info).TradeID;
      this.Locked = ((TradeViewModel) info).Locked;
      this.Guid = ((TradeBase) info).Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = info.CommitmentType;
      this.CommitmentNumber = ((TradeBase) info).Name;
      this.MasterContractNumber = info.ContractNumber;
      this.LastModified = info.LastModified;
      this.Status = (TradeStatus) ((TradeViewModel) info).Status;
      this.TradeDescription = info.TradeDescription;
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.LoanCount = info.LoanCount;
      this.InvestorName = ((TradeViewModel) info).InvestorName;
      this.InvestorCommitmentNumber = info.InvestorCommitmentNumber;
      this.InvestorTradeNumber = info.InvestorTradeNumber;
      this.TargetDeliveryDate = ((TradeViewModel) info).TargetDeliveryDate;
      this.EarlyDeliveryDate = info.EarlyDeliveryDate;
      this.InvestorDeliveryDate = info.InvestorDeliveryDate;
      this.PurchaseDate = info.PurchaseDate;
      this.ShipmentDate = info.ShipmentDate;
      this.Servicer = info.Servicer;
      this.ServicingType = (ServicingType) info.ServicingType;
      this.AssignedDate = info.AssignedStatusDate;
      this.AssignedSecurityId = info.AssignedSecurityID;
      this.BuyDown = info.BuyDownAmount;
      this.BuyUp = info.BuyUpAmount;
      this.RateAdjustment = info.RateAdjustment;
      this.TradeAmount = ((TradeViewModel) info).TradeAmount;
      this.Tolerance = info.Tolerance;
      this.MinAmount = (Decimal) (int) Math.Round(info.MinAmount, 0);
      this.MaxAmount = (Decimal) (int) Math.Round(info.MaxAmount, 0);
      this.AssignedAmount = ((TradeViewModel) info).AssignedAmount;
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = ((TradeViewModel) info).CompletionPercent;
      this.TotalPairoffAmount = ((TradeViewModel) info).PairOffAmount;
      this.GainLossAmount = info.GainLossAmount;
      this.MiscFee = info.MiscAdjustment;
      this.NetProfit = info.NetProfit;
      this.TotalProfit = info.AssignedProfit;
      this.IsBulkDelivery = info.IsBulkDelivery;
      this.isWeightedAvgBulkPriceLocked = info.IsWeightedAvgBulkPriceLocked;
      this.weightedAvgBulkPrice = info.WeightedAvgBulkPrice;
      this.PairOffAmount1 = info.PairOffAmount1;
      this.PairOffAmount2 = info.PairOffAmount2;
      this.PairOffAmount3 = info.PairOffAmount3;
      this.PairOffAmount4 = info.PairOffAmount4;
      this.PairOffDate1 = info.PairOffDate1;
      this.PairOffDate2 = info.PairOffDate2;
      this.PairOffDate3 = info.PairOffDate3;
      this.PairOffDate4 = info.PairOffDate4;
      this.PairOffGainLoss1 = info.PairOffGainLoss1;
      this.PairOffGainLoss2 = info.PairOffGainLoss2;
      this.PairOffGainLoss3 = info.PairOffGainLoss3;
      this.PairOffGainLoss4 = info.PairOffGainLoss4;
      this.PairOffFee1 = info.PairOffBuyPrice1;
      this.PairOffFee2 = info.PairOffBuyPrice2;
      this.PairOffFee3 = info.PairOffBuyPrice3;
      this.PairOffFee4 = info.PairOffBuyPrice4;
    }
  }
}
