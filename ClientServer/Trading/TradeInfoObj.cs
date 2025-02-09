// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeInfoObj
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class TradeInfoObj : TradeBase, ITradeInfoObject
  {
    public const string TradeEligibilityDataKeyPrefix = "TradeEligibility�";
    private int tradeID = -1;
    private string guid = string.Empty;
    private string name = string.Empty;
    private Decimal pairOffAmount;
    private Decimal pairOffFee;
    private string commitmentType = string.Empty;
    private DateTime commitmentDate = DateTime.MinValue;
    private string tradeDescription = string.Empty;
    private Decimal coupon;
    private string dealerName = string.Empty;
    public TradeStatus status;
    private bool locked;
    private int contractId = -1;
    private Decimal tradeAmount;
    private Decimal tolerance;
    private Decimal minAmount;
    private Decimal maxAmount;
    private string notes = string.Empty;
    private TradeFilter filter;
    private TradePairOffs pairOffs;
    private FannieMaeProducts productNames;
    private GuarantyFeeItems guarantyFees;
    private GuarantyFeeItems cpa;
    private MbsPoolBuyUpDownItems buyUpDownItems;
    private TradePricingInfo pricingInfo;
    private TradePriceAdjustments priceAdjustments;
    private SRPTable srpTable;
    private Investor investor;
    private ContactInformation dealer;
    private ContactInformation assignee;
    private EPPSLoanProgramFilters eppsLoanPrograms;
    private DateTime investorDeliveryDate;
    private DateTime earlyDeliveryDate;
    private DateTime targetDeliveryDate;
    private DateTime shipmentDate;
    private DateTime purchaseDate;
    private Decimal completionPercent;
    private bool isCloned;
    private Decimal openAmount;
    private Decimal totalPairOffGainLoss;
    private string pendingBy;
    private TradeCalculation calculation;

    public TradeInfoObj()
    {
    }

    public TradeInfoObj(TradeInfoObj trade)
    {
    }

    public TradeInfoObj(int tradeID, string guid, string name)
    {
      this.tradeID = tradeID;
      this.guid = guid;
      this.name = name;
    }

    public override int TradeID
    {
      get => this.tradeID;
      set => this.tradeID = value;
    }

    public override string Guid
    {
      get => string.IsNullOrEmpty(this.guid) ? System.Guid.NewGuid().ToString() : this.guid;
      set => this.guid = value;
    }

    public override string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public virtual Decimal PairOffAmount
    {
      get => this.pairOffAmount;
      set => this.pairOffAmount = value;
    }

    public virtual Decimal PairOffFee
    {
      get => this.pairOffFee;
      set => this.pairOffFee = value;
    }

    public virtual string CommitmentType
    {
      get => this.commitmentType;
      set => this.commitmentType = value;
    }

    public virtual DateTime CommitmentDate
    {
      get => this.commitmentDate;
      set => this.commitmentDate = value;
    }

    public virtual string TradeDescription
    {
      get => this.tradeDescription;
      set => this.tradeDescription = value;
    }

    public virtual Decimal Coupon
    {
      get => this.coupon;
      set => this.coupon = value;
    }

    public virtual string DealerName
    {
      get => this.dealerName;
      set => this.dealerName = value;
    }

    public virtual TradeStatus Status
    {
      get
      {
        if (this.status == TradeStatus.Pending)
          return TradeStatus.Pending;
        if (this.status == TradeStatus.Voided)
          return TradeStatus.Voided;
        if (this.status == TradeStatus.Archived)
          return TradeStatus.Archived;
        if (this.status == TradeStatus.Delivered)
          return TradeStatus.Delivered;
        if (this.status == TradeStatus.Settled)
          return TradeStatus.Settled;
        if (this.status == TradeStatus.Unpublished)
          return TradeStatus.Unpublished;
        return this.status == TradeStatus.Committed || this.CommitmentDate != DateTime.MinValue ? TradeStatus.Committed : TradeStatus.Open;
      }
      set => this.status = value;
    }

    public virtual bool Archived
    {
      get => this.Status == TradeStatus.Archived;
      set
      {
        if (!value)
          return;
        this.Status = TradeStatus.Archived;
      }
    }

    public virtual bool Locked
    {
      get => this.Status == TradeStatus.Archived || this.locked;
      set => this.locked = value;
    }

    public int ContractID
    {
      get => this.contractId;
      set => this.contractId = value;
    }

    public virtual Decimal TradeAmount
    {
      get => this.tradeAmount;
      set => this.tradeAmount = value;
    }

    public virtual Decimal Tolerance
    {
      get => this.tolerance;
      set => this.tolerance = value;
    }

    public virtual Decimal MinAmount
    {
      get => this.minAmount;
      set => this.minAmount = value;
    }

    public virtual Decimal MaxAmount
    {
      get => this.maxAmount;
      set => this.maxAmount = value;
    }

    public string Notes
    {
      get => this.notes;
      set => this.notes = value;
    }

    public TradeFilter Filter
    {
      get => this.filter;
      set => this.filter = value;
    }

    public virtual TradePairOffs PairOffs => this.pairOffs;

    public virtual FannieMaeProducts ProductNames
    {
      set => this.productNames = value;
      get => this.productNames;
    }

    public virtual GuarantyFeeItems GuarantyFees
    {
      set => this.guarantyFees = value;
      get => this.guarantyFees;
    }

    public virtual GuarantyFeeItems CPA
    {
      set => this.cpa = value;
      get => this.cpa;
    }

    public MbsPoolBuyUpDownItems BuyUpDownItems
    {
      get => this.buyUpDownItems;
      set => this.buyUpDownItems = value;
    }

    public TradePricingInfo Pricing
    {
      get => this.pricingInfo;
      set => this.pricingInfo = value;
    }

    public TradePriceAdjustments PriceAdjustments
    {
      get => this.priceAdjustments;
      set => this.priceAdjustments = value;
    }

    public SRPTable SRPTable
    {
      get => this.srpTable;
      set => this.srpTable = value;
    }

    public Investor Investor
    {
      get => this.investor;
      set => this.investor = value;
    }

    public string InvestorName
    {
      get => this.Investor == null ? string.Empty : this.Investor.Name;
      set => this.Investor.Name = value;
    }

    public ContactInformation Dealer
    {
      get => this.dealer;
      set => this.dealer = value;
    }

    public ContactInformation Assignee
    {
      get => this.assignee;
      set => this.assignee = value;
    }

    public virtual EPPSLoanProgramFilters EPPSLoanProgramsFilter
    {
      set => this.eppsLoanPrograms = value;
      get => this.eppsLoanPrograms;
    }

    public DateTime InvestorDeliveryDate
    {
      get => this.investorDeliveryDate;
      set => this.investorDeliveryDate = value;
    }

    public DateTime EarlyDeliveryDate
    {
      get => this.earlyDeliveryDate;
      set => this.earlyDeliveryDate = value;
    }

    public DateTime TargetDeliveryDate
    {
      get => this.targetDeliveryDate;
      set => this.targetDeliveryDate = value;
    }

    public DateTime ShipmentDate
    {
      get => this.shipmentDate;
      set => this.shipmentDate = value;
    }

    public DateTime PurchaseDate
    {
      get => this.purchaseDate;
      set => this.purchaseDate = value;
    }

    public Decimal CompletionPercent
    {
      get => this.completionPercent;
      set => this.completionPercent = value;
    }

    public bool IsCloned
    {
      get => this.isCloned;
      set => this.isCloned = value;
    }

    public Decimal OpenAmount
    {
      get => this.openAmount;
      set => this.openAmount = value;
    }

    public Decimal TotalPairOffGainLoss
    {
      get => this.totalPairOffGainLoss;
      set => this.totalPairOffGainLoss = value;
    }

    public string PendingBy
    {
      get => this.pendingBy;
      set => this.pendingBy = value;
    }

    public virtual string CreateEligiblityDataKey(string elementName)
    {
      return "TradeEligibility." + (object) this.TradeID + "." + elementName;
    }

    public virtual void ParseTradeObjects(
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
      this.notes = notes;
      this.filter = BinaryConvertible<TradeFilter>.Parse(filterQueryXml);
      this.pairOffs = BinaryConvertible<TradePairOffs>.Parse(pairOffXml);
      this.pricingInfo = BinaryConvertible<TradePricingInfo>.Parse(pricingXml);
      this.priceAdjustments = TradePriceAdjustments.Parse(adjustmentsXml);
      this.buyUpDownItems = BinaryConvertible<MbsPoolBuyUpDownItems>.Parse(buyUpDownXml);
      this.srpTable = BinaryConvertible<SRPTable>.Parse(srpTableXml);
      this.investor = BinaryConvertible<Investor>.Parse(investorXml);
      this.dealer = BinaryConvertible<ContactInformation>.Parse(dealerXml);
      this.assignee = BinaryConvertible<ContactInformation>.Parse(assigneeXml);
      this.productNames = BinaryConvertible<FannieMaeProducts>.Parse(productXml);
      this.guarantyFees = BinaryConvertible<GuarantyFeeItems>.Parse(guarantyFeesXml);
      this.eppsLoanPrograms = BinaryConvertible<EPPSLoanProgramFilters>.Parse(eppsLoanProgramXml);
      this.InitTradeObjects();
    }

    public virtual void InitTradeObjects()
    {
      if (this.pairOffs == null)
        this.pairOffs = new TradePairOffs();
      if (this.pricingInfo == null)
        this.pricingInfo = new TradePricingInfo();
      if (this.priceAdjustments == null)
        this.priceAdjustments = new TradePriceAdjustments();
      if (this.buyUpDownItems == null)
        this.buyUpDownItems = new MbsPoolBuyUpDownItems();
      if (this.srpTable == null)
        this.srpTable = new SRPTable();
      if (this.investor == null)
        this.investor = new Investor();
      if (this.dealer == null)
        this.dealer = new ContactInformation();
      if (this.assignee == null)
        this.assignee = new ContactInformation();
      if (this.productNames == null)
        this.productNames = new FannieMaeProducts();
      if (this.guarantyFees == null)
        this.guarantyFees = new GuarantyFeeItems();
      if (this.eppsLoanPrograms != null)
        return;
      this.eppsLoanPrograms = new EPPSLoanProgramFilters();
    }

    public virtual TradeCalculation Calculation
    {
      get
      {
        if (this.calculation != null)
          return this.calculation;
        if (this.TradeType == TradeType.LoanTrade)
          this.calculation = (TradeCalculation) new LoanTradeCalculation((ITradeInfoObject) this);
        else if (this.TradeType == TradeType.SecurityTrade)
          this.calculation = (TradeCalculation) new SecurityTradeCalculation((ITradeInfoObject) this);
        else if (this.TradeType == TradeType.CorrespondentTrade)
          this.calculation = (TradeCalculation) new CorrespondentTradeCalculation((ITradeInfoObject) this);
        return this.calculation;
      }
    }

    public virtual Decimal GetMSRValueForNoteRate(Decimal noteRate)
    {
      for (int index = 0; index < this.Pricing.MSRPricingItems.Count; ++index)
      {
        if (this.Pricing.MSRPricingItems[index].Rate == noteRate)
          return this.Pricing.MSRPricingItems[index].Price;
      }
      return 0M;
    }
  }
}
