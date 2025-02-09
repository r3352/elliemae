// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SecurityTradeInfo : TradeInfoObj
  {
    private string securityType = string.Empty;
    private string programType = string.Empty;
    private int term1 = -1;
    private int term2 = -1;
    private Decimal price;
    private DateTime confirmDate = DateTime.MinValue;
    private DateTime settlementDate = DateTime.MinValue;
    private DateTime notificationDate = DateTime.MinValue;
    private bool isHidden;

    public SecurityTradeInfo() => this.TradeType = TradeType.SecurityTrade;

    public SecurityTradeInfo(SecurityTradeInfo source)
      : base((TradeInfoObj) source)
    {
      this.CommitmentType = source.CommitmentType;
      this.TradeDescription = source.TradeDescription;
      this.SecurityType = source.SecurityType;
      this.ProgramType = source.ProgramType;
      this.Term1 = source.Term1;
      this.Term2 = source.Term2;
      this.Coupon = source.Coupon;
      this.DealerName = source.DealerName;
      this.Dealer = new ContactInformation(source.Dealer);
      this.TradeAmount = source.TradeAmount;
      this.Tolerance = source.Tolerance;
      this.MinAmount = source.MinAmount;
      this.MaxAmount = source.MaxAmount;
      this.OptionPremium = source.OptionPremium;
      this.TradeType = TradeType.SecurityTrade;
    }

    public string SecurityType
    {
      get => this.securityType;
      set => this.securityType = value;
    }

    public string ProgramType
    {
      get => this.programType;
      set => this.programType = value;
    }

    public int Term1
    {
      get => this.term1;
      set => this.term1 = value;
    }

    public int Term2
    {
      get => this.term2;
      set => this.term2 = value;
    }

    public Decimal Price
    {
      get => this.price;
      set => this.price = value;
    }

    public DateTime ConfirmDate
    {
      get => this.confirmDate;
      set => this.confirmDate = value;
    }

    public DateTime SettlementDate
    {
      get => this.settlementDate;
      set => this.settlementDate = value;
    }

    public DateTime NotificationDate
    {
      get => this.notificationDate;
      set => this.notificationDate = value;
    }

    public bool IsHidden
    {
      get => this.isHidden;
      set => this.isHidden = value;
    }

    public Decimal OptionPremium { get; set; }

    public SecurityTradeInfo Duplicate()
    {
      SecurityTradeInfo securityTradeInfo = new SecurityTradeInfo(this);
      securityTradeInfo.IsCloned = true;
      return securityTradeInfo;
    }

    public SecurityTradeCalculation Calculation
    {
      get
      {
        return base.Calculation != null ? (SecurityTradeCalculation) base.Calculation : new SecurityTradeCalculation((ITradeInfoObject) this);
      }
    }
  }
}
