// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.LoanTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represents a loan trade.</summary>
  /// <remarks>Loan trade allows correspondent sellers to keep track of individual commitments
  /// from their correspondent buyers.</remarks>
  public class LoanTrade
  {
    private List<string> loanPrograms = new List<string>();
    private List<string> mileStones = new List<string>();
    private List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus> occupancyStatus = new List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus>();
    private List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus> investorStatus = new List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus>();
    private bool isWeightedAvgBulkPriceLocked;
    private Decimal weightedAvgBulkPrice;

    /// <summary>Gets or sets commitment number of the loan trade</summary>
    public string CommitmentNumber { get; set; }

    /// <summary>Gets or sets commitment date of the loan trade</summary>
    public DateTime CommitmentDate { get; set; }

    /// <summary>Gets or sets commitment type of the loan trade</summary>
    public string CommitmentType { get; set; }

    /// <summary>Gets or sets trade description of the loan trade</summary>
    public string TradeDescription { get; set; }

    /// <summary>Gets or sets the master contract number</summary>
    public string MasterContractNumber { get; set; }

    /// <summary>Gets or sets commitement amount of the loan trade</summary>
    public Decimal TradeAmount { get; set; }

    /// <summary>
    /// Gets or sets the target delivery date of the loan trade
    /// </summary>
    public DateTime TargetDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the actual delivery date of the loan trade
    /// </summary>
    public DateTime ActualDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the early delivery date of the loan trade
    /// </summary>
    public DateTime EarlyDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the total pair-off amount of the loan trade
    /// </summary>
    public Decimal TotalPairoffAmount { get; set; }

    /// <summary>
    /// Gets or sets the tolerance percentage of the loan trade
    /// </summary>
    public Decimal Tolerance { get; set; }

    /// <summary>Gets or sets a flag whether the loan trade is locked</summary>
    public bool Locked { get; set; }

    /// <summary>Gets or sets investor name of the loan trade</summary>
    public string InvestorName { get; set; }

    /// <summary>
    /// Gets or sets investor commitment number of the loan trade
    /// </summary>
    public string InvestorCommitmentNumber { get; set; }

    /// <summary>
    /// Gets or sets the investor delivery date of the loan trade
    /// </summary>
    public DateTime InvestorDeliveryDate { get; set; }

    /// <summary>Gets or sets investor trade number of the loan trade</summary>
    public string InvestorTradeNumber { get; set; }

    /// <summary>
    /// Gets or sets last modified date/time of the loan trade
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Gets or sets count of assigned loans of the loan trade
    /// </summary>
    public int LoanCount { get; set; }

    /// <summary>Gets or sets Misc. fee of the loan trade</summary>
    public Decimal MiscFee { get; set; }

    /// <summary>Gets or sets net profit of the loan trade</summary>
    public Decimal NetProfit { get; set; }

    /// <summary>Gets or sets purchase date of the loan trade</summary>
    public DateTime PurchaseDate { get; set; }

    /// <summary>Gets or sets rate adjustment of the loan trade</summary>
    public Decimal RateAdjustment { get; set; }

    /// <summary>Gets or sets buy up of the loan trade</summary>
    public Decimal BuyUp { get; set; }

    /// <summary>Gets or sets buy down of the loan trade</summary>
    public Decimal BuyDown { get; set; }

    /// <summary>Gets or sets servicer of the loan trade</summary>
    public string Servicer { get; set; }

    /// <summary>Gets or sets servicing type of the loan trade</summary>
    public ServicingType ServicingType { get; set; }

    /// <summary>
    /// Gets or sets the total profit amount of the loan trade
    /// </summary>
    public Decimal TotalProfit { get; set; }

    /// <summary>Gets or sets actual delivery date of the loan trade</summary>
    public DateTime ShipmentDate { get; set; }

    /// <summary>
    /// Gets or sets date when the loan trade is assigned to a security trade
    /// </summary>
    public DateTime AssignedDate { get; set; }

    /// <summary>
    /// Gets or sets security trade ID which the loan trade is assigned to
    /// </summary>
    public string AssignedSecurityId { get; set; }

    /// <summary>Gets or sets pair off amount 1</summary>
    public Decimal PairOffAmount1 { get; set; }

    /// <summary>
    /// /// <summary>
    /// Gets or sets pair off fee 1
    /// </summary>
    /// </summary>
    public Decimal PairOffFee1 { get; set; }

    /// <summary>Gets or sets pair off gain loss 1</summary>
    public Decimal PairOffGainLoss1 { get; set; }

    /// <summary>Gets or sets pair off date 1</summary>
    public DateTime PairOffDate1 { get; set; }

    /// <summary>Gets or sets pair off amount 2</summary>
    public Decimal PairOffAmount2 { get; set; }

    /// <summary>Gets or sets pair off fee 2</summary>
    public Decimal PairOffFee2 { get; set; }

    /// <summary>Gets or sets pair off gain loss 2</summary>
    public Decimal PairOffGainLoss2 { get; set; }

    /// <summary>Gets or sets pair off date 2</summary>
    public DateTime PairOffDate2 { get; set; }

    /// <summary>Gets or sets pair off amount 3</summary>
    public Decimal PairOffAmount3 { get; set; }

    /// <summary>Gets or sets pair off fee 3</summary>
    public Decimal PairOffFee3 { get; set; }

    /// <summary>Gets or sets pair off gain loss</summary>
    public Decimal PairOffGainLoss3 { get; set; }

    /// <summary>Gets or sets pair off date 3</summary>
    public DateTime PairOffDate3 { get; set; }

    /// <summary>Gets or sets pair off amount 4</summary>
    public Decimal PairOffAmount4 { get; set; }

    /// <summary>Gets or sets pair off fee 4</summary>
    public Decimal PairOffFee4 { get; set; }

    /// <summary>Gets or sets pair off date 4</summary>
    public DateTime PairOffDate4 { get; set; }

    /// <summary>Gets or sets pair off gain loss 4</summary>
    public Decimal PairOffGainLoss4 { get; set; }

    /// <summary>Gets or sets a list of Base Price settings</summary>
    public Dictionary<Decimal, Decimal> BasePriceItems { get; set; }

    /// <summary>
    /// Gets or sets weighted avg bulk price locked indicator of the loan trade
    /// </summary>
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

    /// <summary>
    /// Gets or sets weighted avg bulk price of the loan trade
    /// </summary>
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

    /// <summary>
    /// Gets or sets bulk delivery indicator of the loan trade
    /// </summary>
    public bool IsBulkDelivery { get; set; }

    /// <summary>Gets or sets loan programs.</summary>
    public List<string> LoanPrograms
    {
      get => this.loanPrograms;
      set => this.loanPrograms = value;
    }

    /// <summary>Gets or sets milestones.</summary>
    public List<string> MileStones
    {
      get => this.mileStones;
      set => this.mileStones = value;
    }

    /// <summary>Gets or sets Min Note Rate value</summary>
    public Decimal? MinNoteRateRange { get; set; }

    /// <summary>Gets or sets Max Note Rate value</summary>
    public Decimal? MaxNoteRateRange { get; set; }

    /// <summary>Gets or sets Min Term value</summary>
    public int? MinTermRange { get; set; }

    /// <summary>Gets or sets Max Term Value</summary>
    public int? MaxTermRange { get; set; }

    /// <summary>Gets or sets Min Term value</summary>
    public Decimal? MinLTVRange { get; set; }

    /// <summary>Gets or sets Max Term Value</summary>
    public Decimal? MaxLTVRange { get; set; }

    /// <summary>Gets or sets occupancy status</summary>
    public List<EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus> OccupancyStatus
    {
      get => this.occupancyStatus;
      set => this.occupancyStatus = value;
    }

    /// <summary>Gets or sets investor status</summary>
    public List<EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus> InvestorStatus
    {
      get => this.investorStatus;
      set => this.investorStatus = value;
    }

    /// <summary>Gets the identifier of the loan trade</summary>
    public int Id { get; private set; }

    /// <summary>Gets the Guid of the loan trade</summary>
    public string Guid { get; private set; }

    /// <summary>Gets the open amount of the loan trade</summary>
    public Decimal OpenAmount { get; private set; }

    /// <summary>
    /// Gets how many percent of the loan trade has been allocated by assigned loans
    /// </summary>
    public Decimal CompletionPercentage { get; private set; }

    /// <summary>Gets the maximum amount of the loan trade</summary>
    public Decimal MaxAmount { get; private set; }

    /// <summary>Gets the minimum amount of the loan trade</summary>
    public Decimal MinAmount { get; private set; }

    /// <summary>Gets the amount of assigned loans of the loan trade</summary>
    public Decimal AssignedAmount { get; private set; }

    /// <summary>Gets the total gain loss amount of the loan trade</summary>
    public Decimal GainLossAmount { get; private set; }

    /// <summary>
    /// Gets the flag that if the loan trade has pending loans
    /// </summary>
    public bool HasPendingLoan { get; private set; }

    /// <summary>Gets the status of the loan trade</summary>
    public TradeStatus Status { get; private set; }

    /// <summary>Gets the text for status</summary>
    public string StatusText => this.Status.ToDescription();

    /// <summary>Gets the text for servicing type</summary>
    public string ServicingTypeText => this.ServicingType.ToDescription();

    internal LoanTrade(LoanTradeInfo info)
    {
      this.Id = info.TradeID;
      this.Locked = info.Locked;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = info.CommitmentType;
      this.CommitmentNumber = info.Name;
      this.MasterContractNumber = info.ContractNumber;
      this.LastModified = info.LastModified;
      this.Status = (TradeStatus) info.Status;
      this.TradeDescription = info.TradeDescription;
      this.HasPendingLoan = info.HasPendingLoan();
      this.LoanCount = info.LoanCount;
      this.InvestorName = info.Investor.Name;
      this.InvestorCommitmentNumber = info.InvestorCommitmentNumber;
      this.InvestorTradeNumber = info.InvestorTradeNumber;
      this.TargetDeliveryDate = info.TargetDeliveryDate;
      this.EarlyDeliveryDate = info.EarlyDeliveryDate;
      this.InvestorDeliveryDate = info.InvestorDeliveryDate;
      this.PurchaseDate = info.PurchaseDate;
      this.ShipmentDate = info.ShipmentDate;
      this.Servicer = info.Servicer;
      this.ServicingType = (ServicingType) info.ServicingType;
      this.BuyDown = info.BuyDownAmount;
      this.BuyUp = info.BuyUpAmount;
      this.RateAdjustment = info.RateAdjustment;
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
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.MinAmount = (Decimal) (int) Math.Round(info.MinAmount, 0);
      this.MaxAmount = (Decimal) (int) Math.Round(info.MaxAmount, 0);
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = info.PairOffAmount;
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
      if (info.Filter.FilterType != TradeFilterType.Simple)
        return;
      SimpleTradeFilter simpleFilter = info.Filter.GetSimpleFilter();
      this.loanPrograms = simpleFilter.LoanPrograms.ToList<string>();
      this.mileStones = simpleFilter.Milestones.ToList<string>();
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
      loanTradeInfo.Archived = false;
      loanTradeInfo.BuyDownAmount = this.BuyDown;
      loanTradeInfo.BuyUpAmount = this.BuyUp;
      loanTradeInfo.CommitmentDate = this.CommitmentDate;
      loanTradeInfo.CommitmentType = this.CommitmentType;
      loanTradeInfo.Name = this.CommitmentNumber;
      loanTradeInfo.EarlyDeliveryDate = this.EarlyDeliveryDate;
      loanTradeInfo.InvestorCommitmentNumber = this.InvestorCommitmentNumber;
      loanTradeInfo.InvestorDeliveryDate = this.InvestorDeliveryDate;
      loanTradeInfo.InvestorName = this.InvestorName;
      loanTradeInfo.InvestorTradeNumber = this.InvestorTradeNumber;
      loanTradeInfo.IsBulkDelivery = this.IsBulkDelivery;
      loanTradeInfo.IsWeightedAvgBulkPriceLocked = this.IsWeightedAvgBulkPriceLocked;
      loanTradeInfo.LastModified = this.LastModified;
      loanTradeInfo.ContractNumber = this.MasterContractNumber;
      loanTradeInfo.MiscAdjustment = this.MiscFee;
      loanTradeInfo.NetProfit = this.NetProfit;
      loanTradeInfo.RateAdjustment = this.RateAdjustment;
      loanTradeInfo.Servicer = this.Servicer;
      loanTradeInfo.ServicingType = (EllieMae.EMLite.Trading.ServicingType) this.ServicingType;
      loanTradeInfo.ShipmentDate = this.ActualDeliveryDate;
      loanTradeInfo.Status = (EllieMae.EMLite.Trading.TradeStatus) this.Status;
      loanTradeInfo.TargetDeliveryDate = this.TargetDeliveryDate;
      loanTradeInfo.Tolerance = this.Tolerance;
      loanTradeInfo.PairOffAmount = this.TotalPairoffAmount;
      loanTradeInfo.PurchaseDate = this.PurchaseDate;
      loanTradeInfo.TotalPairOffGainLoss = this.TotalProfit;
      loanTradeInfo.TradeAmount = this.TradeAmount;
      loanTradeInfo.TradeDescription = this.TradeDescription;
      loanTradeInfo.WeightedAvgBulkPrice = this.WeightedAvgBulkPrice;
      SimpleTradeFilter simpleFilter = new SimpleTradeFilter();
      foreach (string loanProgram in this.LoanPrograms)
        simpleFilter.LoanPrograms.Add(loanProgram);
      foreach (string mileStone in this.MileStones)
        simpleFilter.Milestones.Add(mileStone);
      foreach (EllieMae.Encompass.BusinessObjects.TradeManagement.OccupancyStatus occupancyStatu in this.OccupancyStatus)
      {
        if (!simpleFilter.OccupancyStatuses.Contains(occupancyStatu.ToDescription()))
          simpleFilter.OccupancyStatuses.Add(occupancyStatu.ToDescription());
      }
      foreach (EllieMae.Encompass.BusinessObjects.TradeManagement.InvestorStatus investorStatu in this.InvestorStatus)
      {
        if (!simpleFilter.InvestorStatuses.Contains(investorStatu.ToDescription()))
          simpleFilter.InvestorStatuses.Add(investorStatu.ToDescription());
      }
      Decimal? nullable1;
      if (this.MinNoteRateRange.HasValue || this.MaxNoteRateRange.HasValue)
      {
        SimpleTradeFilter simpleTradeFilter = simpleFilter;
        nullable1 = this.MinNoteRateRange;
        string minText;
        if (nullable1.HasValue)
        {
          nullable1 = this.MinNoteRateRange;
          minText = nullable1.ToString();
        }
        else
          minText = "";
        nullable1 = this.MaxNoteRateRange;
        string maxText;
        if (nullable1.HasValue)
        {
          nullable1 = this.MaxNoteRateRange;
          maxText = nullable1.ToString();
        }
        else
          maxText = "";
        Range<Decimal> range = Range<Decimal>.Parse(minText, maxText, Decimal.MinValue, Decimal.MaxValue);
        simpleTradeFilter.NoteRateRange = range;
      }
      if (this.MinTermRange.HasValue || this.MaxTermRange.HasValue)
      {
        SimpleTradeFilter simpleTradeFilter = simpleFilter;
        int? nullable2 = this.MinTermRange;
        string minText;
        if (nullable2.HasValue)
        {
          nullable2 = this.MinTermRange;
          minText = nullable2.ToString();
        }
        else
          minText = "";
        nullable2 = this.MaxTermRange;
        string maxText;
        if (nullable2.HasValue)
        {
          nullable2 = this.MaxTermRange;
          maxText = nullable2.ToString();
        }
        else
          maxText = "";
        Range<int> range = Range<int>.Parse(minText, maxText, int.MinValue, int.MaxValue);
        simpleTradeFilter.TermRange = range;
      }
      nullable1 = this.MinLTVRange;
      if (!nullable1.HasValue)
      {
        nullable1 = this.MaxLTVRange;
        if (!nullable1.HasValue)
          goto label_47;
      }
      SimpleTradeFilter simpleTradeFilter1 = simpleFilter;
      nullable1 = this.MinLTVRange;
      string minText1;
      if (nullable1.HasValue)
      {
        nullable1 = this.MinLTVRange;
        minText1 = nullable1.ToString();
      }
      else
        minText1 = "";
      nullable1 = this.MaxLTVRange;
      string maxText1;
      if (nullable1.HasValue)
      {
        nullable1 = this.MaxLTVRange;
        maxText1 = nullable1.ToString();
      }
      else
        maxText1 = "";
      Range<Decimal> range1 = Range<Decimal>.Parse(minText1, maxText1, -2147483648M, 2147483647M);
      simpleTradeFilter1.LTVRange = range1;
label_47:
      loanTradeInfo.Filter = new TradeFilter(simpleFilter);
      if (this.PairOffDate1 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(1, this.PairOffDate1, this.PairOffAmount1, this.PairOffFee1));
      if (this.PairOffDate2 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(2, this.PairOffDate2, this.PairOffAmount2, this.PairOffFee2));
      if (this.PairOffDate3 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(3, this.PairOffDate3, this.PairOffAmount3, this.PairOffFee3));
      if (this.PairOffDate4 != DateTime.MinValue)
        loanTradeInfo.LoanTradePairOffs.Add(new LoanTradePairOff(4, this.PairOffDate4, this.PairOffAmount4, this.PairOffFee4));
      foreach (KeyValuePair<Decimal, Decimal> basePriceItem in this.BasePriceItems)
        loanTradeInfo.Pricing.SimplePricingItems.Add(new TradePricingItem(basePriceItem.Key, basePriceItem.Value));
      return loanTradeInfo;
    }

    /// <summary>Default constructor.</summary>
    public LoanTrade() => this.BasePriceItems = new Dictionary<Decimal, Decimal>();

    /// <summary>Constructor of LoanTrade</summary>
    internal LoanTrade(LoanTradeViewModel info)
    {
      this.Id = info.TradeID;
      this.Locked = info.Locked;
      this.Guid = info.Guid;
      this.CommitmentDate = info.CommitmentDate;
      this.CommitmentType = info.CommitmentType;
      this.CommitmentNumber = info.Name;
      this.MasterContractNumber = info.ContractNumber;
      this.LastModified = info.LastModified;
      this.Status = (TradeStatus) info.Status;
      this.TradeDescription = info.TradeDescription;
      this.HasPendingLoan = info.PendingLoanCount > 0;
      this.LoanCount = info.LoanCount;
      this.InvestorName = info.InvestorName;
      this.InvestorCommitmentNumber = info.InvestorCommitmentNumber;
      this.InvestorTradeNumber = info.InvestorTradeNumber;
      this.TargetDeliveryDate = info.TargetDeliveryDate;
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
      this.TradeAmount = info.TradeAmount;
      this.Tolerance = info.Tolerance;
      this.MinAmount = (Decimal) (int) Math.Round(info.MinAmount, 0);
      this.MaxAmount = (Decimal) (int) Math.Round(info.MaxAmount, 0);
      this.AssignedAmount = info.AssignedAmount;
      this.OpenAmount = info.OpenAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.TotalPairoffAmount = info.PairOffAmount;
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
