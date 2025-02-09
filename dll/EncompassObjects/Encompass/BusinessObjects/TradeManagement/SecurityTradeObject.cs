// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class SecurityTradeObject
  {
    private DateTime commitmentDate;
    private string securityId;
    private string securityType;
    private Decimal coupon;
    private DateTime settlementDate;
    private string dealerName;
    private Decimal tradeAmount;
    private Decimal minAmount;
    private Decimal maxAmount;
    private Decimal assignedLoanTradeAmount;
    private Decimal completionPercentage;
    private string commitmentType;
    private DateTime confirmationDate;
    private DateTime lastModified;
    private bool locked;
    private DateTime notificationDate;
    private Decimal openAmount;
    private Decimal price;
    private string programType;
    private int termFrom;
    private int termTo;
    private Decimal tolerance;
    private Decimal totalGainLoss;
    private Decimal totalProfit;
    private string tradeDescription;
    private string tradeGUID;

    public DateTime CommitmentDate
    {
      get => this.commitmentDate;
      set => this.commitmentDate = value;
    }

    public string SecurityId
    {
      get => this.securityId;
      set => this.securityId = value;
    }

    public string SecurityType
    {
      get => this.securityType;
      set => this.securityType = value;
    }

    public Decimal Coupon
    {
      get => this.coupon;
      set => this.coupon = value;
    }

    public DateTime SettlementDate
    {
      get => this.settlementDate;
      set => this.settlementDate = value;
    }

    public string DealerName
    {
      get => this.dealerName;
      set => this.dealerName = value;
    }

    public Decimal TradeAmount
    {
      get => this.tradeAmount;
      set => this.tradeAmount = value;
    }

    public Decimal MinAmount
    {
      get => this.minAmount;
      set => this.minAmount = value;
    }

    public Decimal MaxAmount
    {
      get => this.maxAmount;
      set => this.maxAmount = value;
    }

    public Decimal AssignedLoanTradeAmount => this.assignedLoanTradeAmount;

    public Decimal CompletionPercentage => this.completionPercentage;

    public string CommitmentType
    {
      get => this.commitmentType;
      set => this.commitmentType = value;
    }

    public DateTime ConfirmationDate
    {
      get => this.confirmationDate;
      set => this.confirmationDate = value;
    }

    public DateTime LastModified => this.lastModified;

    public bool Locked => this.locked;

    public DateTime NotificationDate
    {
      get => this.notificationDate;
      set => this.notificationDate = value;
    }

    public Decimal OpenAmount => this.openAmount;

    public Decimal Price
    {
      get => this.price;
      set => this.price = value;
    }

    public string ProgramType
    {
      get => this.programType;
      set => this.programType = value;
    }

    public int TermFrom
    {
      get => this.termFrom;
      set => this.termFrom = value;
    }

    public int TermTo
    {
      get => this.termTo;
      set => this.termTo = value;
    }

    public Decimal Tolerance
    {
      get => this.tolerance;
      set => this.tolerance = value;
    }

    public Decimal TotalGainLoss => this.totalGainLoss;

    public Decimal TotalProfit => this.totalProfit;

    public string TradeDescription
    {
      get => this.tradeDescription;
      set => this.tradeDescription = value;
    }

    public string TradeGUID => this.tradeGUID;

    public string DealerAddress { get; set; }

    public string DealerCity { get; set; }

    public string DealerState { get; set; }

    public string DealerZip { get; set; }

    public string DealerPhone { get; set; }

    public string DealerFax { get; set; }

    public string DealerEmail { get; set; }

    public string DealerWebsite { get; set; }

    public string DealerContact { get; set; }

    internal SecurityTradeObject(SecurityTradeViewModel info)
    {
      this.assignedLoanTradeAmount = info.AssignedAmount;
      this.commitmentDate = info.CommitmentDate;
      this.commitmentType = info.CommitmentType;
      this.completionPercentage = Math.Round(info.CompletionPercent, 0);
      this.confirmationDate = info.ConfirmDate;
      this.coupon = info.Coupon;
      this.dealerName = info.DealerName;
      this.lastModified = info.LastModified;
      this.locked = info.Locked;
      this.maxAmount = Math.Round(info.MaxAmount, 0);
      this.minAmount = Math.Round(info.MinAmount, 0);
      this.notificationDate = info.NotificationDate;
      this.openAmount = info.OpenAmount;
      this.price = info.Price;
      this.programType = info.ProgramType;
      this.securityId = ((TradeBase) info).Name;
      this.securityType = info.SecurityType;
      this.settlementDate = info.SettlementDate;
      this.termFrom = info.Term1;
      this.termTo = info.Term2;
      this.tolerance = info.Tolerance;
      this.totalGainLoss = info.TotalPairOffGainLoss;
      this.totalProfit = info.AssignedProfit;
      this.tradeAmount = info.TradeAmount;
      this.tradeDescription = info.TradeDescription;
      this.tradeGUID = ((TradeBase) info).Guid;
    }

    public SecurityTradeObject()
    {
    }

    internal SecurityTradeInfo GetSecurityTradeInfo()
    {
      SecurityTradeInfo securityTradeInfo = new SecurityTradeInfo();
      ((TradeInfoObj) securityTradeInfo).Archived = false;
      ((TradeInfoObj) securityTradeInfo).CommitmentDate = this.CommitmentDate;
      ((TradeInfoObj) securityTradeInfo).CommitmentType = this.CommitmentType;
      securityTradeInfo.ConfirmDate = this.ConfirmationDate;
      ((TradeInfoObj) securityTradeInfo).Coupon = this.Coupon;
      ((TradeInfoObj) securityTradeInfo).Dealer = new ContactInformation()
      {
        EntityName = this.DealerName,
        Address = new Address(this.DealerAddress, "", this.DealerCity, this.DealerState, this.DealerZip),
        PhoneNumber = this.DealerPhone,
        FaxNumber = this.DealerFax,
        EmailAddress = this.DealerEmail,
        WebSite = this.DealerWebsite,
        ContactName = this.DealerContact
      };
      ((TradeInfoObj) securityTradeInfo).MaxAmount = this.MaxAmount;
      ((TradeInfoObj) securityTradeInfo).MinAmount = this.MinAmount;
      securityTradeInfo.NotificationDate = this.NotificationDate;
      securityTradeInfo.Price = this.Price;
      securityTradeInfo.ProgramType = this.ProgramType;
      ((TradeBase) securityTradeInfo).Name = this.SecurityId;
      securityTradeInfo.SecurityType = this.SecurityType;
      securityTradeInfo.SettlementDate = this.SettlementDate;
      securityTradeInfo.Term1 = this.TermFrom;
      securityTradeInfo.Term2 = this.TermTo;
      ((TradeInfoObj) securityTradeInfo).Tolerance = this.Tolerance;
      ((TradeInfoObj) securityTradeInfo).TradeAmount = this.TradeAmount;
      ((TradeInfoObj) securityTradeInfo).TradeDescription = this.TradeDescription;
      return securityTradeInfo;
    }
  }
}
