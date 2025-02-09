// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolSecurityTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  /// <summary>Represent Mbs pool security trade.</summary>
  public class MbsPoolSecurityTrade
  {
    /// <summary>
    /// Gets or sets commitment date of the correspondent trade
    /// </summary>
    public DateTime CommitmentDate { get; set; }

    /// <summary>Gets or sets security ID</summary>
    public string SecurityID { get; set; }

    /// <summary>Gets or sets security Type</summary>
    public string SecurityType { get; set; }

    /// <summary>Gets or sets security Type</summary>
    public Decimal Coupon { get; set; }

    /// <summary>Gets or sets settlement date</summary>
    public DateTime SettlementDate { get; set; }

    /// <summary>Gets or sets original trade dealer</summary>
    public string Dealer { get; set; }

    /// <summary>Gets or sets trade amount of the Security trade</summary>
    public Decimal TradeAmount { get; set; }

    /// <summary>Gets or sets minimum amount of the Security trade</summary>
    public Decimal MinAmount { get; set; }

    /// <summary>Gets or sets maximum amount of the Security trade</summary>
    public Decimal MaxAmount { get; set; }

    /// <summary>Gets or sets price of security trade</summary>
    public Decimal Price { get; set; }

    /// <summary>Gets the identifier of the security trade</summary>
    public int Id { get; private set; }

    /// <summary>Gets the Guid of the security trade</summary>
    public string Guid { get; private set; }

    /// <summary>Gets the assigned amount of the mbs pool</summary>
    public Decimal AssignedPoolAmount { get; private set; }

    /// <summary>
    /// Gets or sets assigned loan trade amount of the Security trade
    /// </summary>
    public Decimal AssignedLoanTradeAmount { get; private set; }

    /// <summary>Gets the open amount of the security trade</summary>
    public Decimal OpenAmount { get; private set; }

    /// <summary>
    /// Gets how many percent of the security trade has been allocated by assigned loans
    /// </summary>
    public Decimal CompletionPercentage { get; private set; }

    internal MbsPoolSecurityTrade(SecurityTradeViewModel info, Decimal assignedPoolAmount)
    {
      this.AssignedPoolAmount = assignedPoolAmount;
      this.Id = info.TradeID;
      this.CommitmentDate = info.CommitmentDate;
      this.SecurityID = info.Name;
      this.SecurityType = info.SecurityType;
      this.Coupon = info.Coupon;
      this.SettlementDate = info.SettlementDate;
      this.Price = info.Price;
      this.Dealer = info.DealerName;
      this.TradeAmount = info.TradeAmount;
      this.MinAmount = info.MinAmount;
      this.MaxAmount = info.MaxAmount;
      this.CompletionPercentage = info.CompletionPercent;
      this.OpenAmount = info.OpenAmount;
      this.AssignedLoanTradeAmount = info.AssignedAmount;
    }
  }
}
