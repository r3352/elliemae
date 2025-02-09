// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.TradeManagement.MbsPoolSecurityTrade
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.Trading;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.TradeManagement
{
  public class MbsPoolSecurityTrade
  {
    public DateTime CommitmentDate { get; set; }

    public string SecurityID { get; set; }

    public string SecurityType { get; set; }

    public Decimal Coupon { get; set; }

    public DateTime SettlementDate { get; set; }

    public string Dealer { get; set; }

    public Decimal TradeAmount { get; set; }

    public Decimal MinAmount { get; set; }

    public Decimal MaxAmount { get; set; }

    public Decimal Price { get; set; }

    public int Id { get; private set; }

    public string Guid { get; private set; }

    public Decimal AssignedPoolAmount { get; private set; }

    public Decimal AssignedLoanTradeAmount { get; private set; }

    public Decimal OpenAmount { get; private set; }

    public Decimal CompletionPercentage { get; private set; }

    internal MbsPoolSecurityTrade(SecurityTradeViewModel info, Decimal assignedPoolAmount)
    {
      this.AssignedPoolAmount = assignedPoolAmount;
      this.Id = ((TradeBase) info).TradeID;
      this.CommitmentDate = info.CommitmentDate;
      this.SecurityID = ((TradeBase) info).Name;
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
