// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.SecurityTradeObject
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>SecurityTradeObject</summary>
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

    /// <summary>Gets/Sets commitment date</summary>
    public DateTime CommitmentDate
    {
      get => this.commitmentDate;
      set => this.commitmentDate = value;
    }

    /// <summary>Gets/Sets security trade ID</summary>
    public string SecurityId
    {
      get => this.securityId;
      set => this.securityId = value;
    }

    /// <summary>Gets/Sets security type</summary>
    public string SecurityType
    {
      get => this.securityType;
      set => this.securityType = value;
    }

    /// <summary>Gets/Sets coupon</summary>
    public Decimal Coupon
    {
      get => this.coupon;
      set => this.coupon = value;
    }

    /// <summary>Gets/Sets settlement date</summary>
    public DateTime SettlementDate
    {
      get => this.settlementDate;
      set => this.settlementDate = value;
    }

    /// <summary>Gets/Sets dealer name</summary>
    public string DealerName
    {
      get => this.dealerName;
      set => this.dealerName = value;
    }

    /// <summary>Gets/Sets trade amount</summary>
    public Decimal TradeAmount
    {
      get => this.tradeAmount;
      set => this.tradeAmount = value;
    }

    /// <summary>Gets/Sets minumum trade amount</summary>
    public Decimal MinAmount
    {
      get => this.minAmount;
      set => this.minAmount = value;
    }

    /// <summary>Gets/Sets maxiumum trade amount</summary>
    public Decimal MaxAmount
    {
      get => this.maxAmount;
      set => this.maxAmount = value;
    }

    /// <summary>Gets assinged loan trade amount</summary>
    public Decimal AssignedLoanTradeAmount => this.assignedLoanTradeAmount;

    /// <summary>Gets completion percentage</summary>
    public Decimal CompletionPercentage => this.completionPercentage;

    /// <summary>Gets/Sets commitment type</summary>
    public string CommitmentType
    {
      get => this.commitmentType;
      set => this.commitmentType = value;
    }

    /// <summary>Gets/Sets commitment type</summary>
    public DateTime ConfirmationDate
    {
      get => this.confirmationDate;
      set => this.confirmationDate = value;
    }

    /// <summary>Gets last modified date</summary>
    public DateTime LastModified => this.lastModified;

    /// <summary>Gets lock status</summary>
    public bool Locked => this.locked;

    /// <summary>Gets/Sets notification date</summary>
    public DateTime NotificationDate
    {
      get => this.notificationDate;
      set => this.notificationDate = value;
    }

    /// <summary>Gets open amount</summary>
    public Decimal OpenAmount => this.openAmount;

    /// <summary>Gets/Sets price</summary>
    public Decimal Price
    {
      get => this.price;
      set => this.price = value;
    }

    /// <summary>Gets program type</summary>
    public string ProgramType
    {
      get => this.programType;
      set => this.programType = value;
    }

    /// <summary>Gets/Sets term from</summary>
    public int TermFrom
    {
      get => this.termFrom;
      set => this.termFrom = value;
    }

    /// <summary>Gets/Sets term to</summary>
    public int TermTo
    {
      get => this.termTo;
      set => this.termTo = value;
    }

    /// <summary>Gets/Sets tolerance</summary>
    public Decimal Tolerance
    {
      get => this.tolerance;
      set => this.tolerance = value;
    }

    /// <summary>Gets total gain/loss</summary>
    public Decimal TotalGainLoss => this.totalGainLoss;

    /// <summary>Gets total profit</summary>
    public Decimal TotalProfit => this.totalProfit;

    /// <summary>Gets/Sets trade description</summary>
    public string TradeDescription
    {
      get => this.tradeDescription;
      set => this.tradeDescription = value;
    }

    /// <summary>Gets trade GUID</summary>
    public string TradeGUID => this.tradeGUID;

    /// <summary>Gets/Sets Dealer Address</summary>
    public string DealerAddress { get; set; }

    /// <summary>Gets/Sets Dealer City</summary>
    public string DealerCity { get; set; }

    /// <summary>Gets/Sets Dealer State</summary>
    public string DealerState { get; set; }

    /// <summary>Gets/Sets Dealer Zip</summary>
    public string DealerZip { get; set; }

    /// <summary>Gets/Sets Dealer Phone</summary>
    public string DealerPhone { get; set; }

    /// <summary>Gets/Sets Dealer Fax</summary>
    public string DealerFax { get; set; }

    /// <summary>Gets/Sets Dealer Emaile Address</summary>
    public string DealerEmail { get; set; }

    /// <summary>Gets/Sets Dealer Website</summary>
    public string DealerWebsite { get; set; }

    /// <summary>Gets/Sets Dealer Contact</summary>
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
      this.securityId = info.Name;
      this.securityType = info.SecurityType;
      this.settlementDate = info.SettlementDate;
      this.termFrom = info.Term1;
      this.termTo = info.Term2;
      this.tolerance = info.Tolerance;
      this.totalGainLoss = info.TotalPairOffGainLoss;
      this.totalProfit = info.AssignedProfit;
      this.tradeAmount = info.TradeAmount;
      this.tradeDescription = info.TradeDescription;
      this.tradeGUID = info.Guid;
    }

    /// <summary>SecurityTradeObject</summary>
    public SecurityTradeObject()
    {
    }

    internal SecurityTradeInfo GetSecurityTradeInfo()
    {
      SecurityTradeInfo securityTradeInfo = new SecurityTradeInfo();
      securityTradeInfo.Archived = false;
      securityTradeInfo.CommitmentDate = this.CommitmentDate;
      securityTradeInfo.CommitmentType = this.CommitmentType;
      securityTradeInfo.ConfirmDate = this.ConfirmationDate;
      securityTradeInfo.Coupon = this.Coupon;
      securityTradeInfo.Dealer = new ContactInformation()
      {
        EntityName = this.DealerName,
        Address = new Address(this.DealerAddress, "", this.DealerCity, this.DealerState, this.DealerZip),
        PhoneNumber = this.DealerPhone,
        FaxNumber = this.DealerFax,
        EmailAddress = this.DealerEmail,
        WebSite = this.DealerWebsite,
        ContactName = this.DealerContact
      };
      securityTradeInfo.MaxAmount = this.MaxAmount;
      securityTradeInfo.MinAmount = this.MinAmount;
      securityTradeInfo.NotificationDate = this.NotificationDate;
      securityTradeInfo.Price = this.Price;
      securityTradeInfo.ProgramType = this.ProgramType;
      securityTradeInfo.Name = this.SecurityId;
      securityTradeInfo.SecurityType = this.SecurityType;
      securityTradeInfo.SettlementDate = this.SettlementDate;
      securityTradeInfo.Term1 = this.TermFrom;
      securityTradeInfo.Term2 = this.TermTo;
      securityTradeInfo.Tolerance = this.Tolerance;
      securityTradeInfo.TradeAmount = this.TradeAmount;
      securityTradeInfo.TradeDescription = this.TradeDescription;
      return securityTradeInfo;
    }
  }
}
