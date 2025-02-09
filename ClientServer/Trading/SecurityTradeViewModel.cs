// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SecurityTradeViewModel : TradeViewModel
  {
    public SecurityTradeViewModel(
      int tradeID,
      string tradeGuid,
      string name,
      TradeStatus status,
      string commitmentType,
      string tradeDescription,
      string securityType,
      string programType,
      int term1,
      int term2,
      Decimal coupon,
      Decimal price,
      string dealerName,
      Decimal tradeAmount,
      Decimal tolerance,
      Decimal minAmount,
      Decimal maxAmount,
      DateTime commitmentDate,
      DateTime confirmDate,
      DateTime settlementDate,
      DateTime notificationDate,
      bool locked,
      DateTime lastModified,
      bool isHidden,
      Decimal totalProfit,
      Decimal totalAmount,
      Decimal completionPercent,
      Decimal OptionPremium)
      : base(tradeID, tradeGuid, name, 0M)
    {
      this.Status = status;
      this.CommitmentType = commitmentType;
      this.TradeDescription = tradeDescription;
      this.SecurityType = securityType;
      this.ProgramType = programType;
      this.Term1 = term1;
      this.Term2 = term2;
      this.Coupon = coupon;
      this.Price = price;
      this.DealerName = dealerName;
      this.TradeAmount = tradeAmount;
      this.Tolerance = tolerance;
      this.MinAmount = minAmount;
      this.MaxAmount = maxAmount;
      this.CommitmentDate = commitmentDate;
      this.ConfirmDate = confirmDate;
      this.SettlementDate = settlementDate;
      this.NotificationDate = notificationDate;
      this.Locked = locked;
      this.LastModified = lastModified;
      this.AssignedProfit = totalProfit;
      this.AssignedAmount = totalAmount;
      this.CompletionPercent = completionPercent;
      this.OptionPremium = OptionPremium;
    }

    protected override string dataTableName => "SecurityTradeDetails";

    private int tradeStatus
    {
      get => (int) this.getField(this.dataTableName + ".Status", (object) -1);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public string CommitmentType
    {
      get => this.getField(this.dataTableName + ".CommitmentType", (object) "") as string;
      set => this.setField(this.dataTableName + ".CommitmentType", (object) value);
    }

    public string TradeDescription
    {
      get => this.getField(this.dataTableName + ".TradeDescription", (object) "") as string;
      set => this.setField(this.dataTableName + ".TradeDescription", (object) value);
    }

    public string SecurityType
    {
      get => this.getField(this.dataTableName + ".SecurityType", (object) "") as string;
      set => this.setField(this.dataTableName + ".SecurityType", (object) value);
    }

    public string ProgramType
    {
      get => this.getField(this.dataTableName + ".ProgramType", (object) "") as string;
      set => this.setField(this.dataTableName + ".ProgramType", (object) value);
    }

    public int Term1
    {
      get => (int) this.getField(this.dataTableName + ".Term1", (object) 0);
      set => this.setField(this.dataTableName + ".Term1", (object) value);
    }

    public int Term2
    {
      get => (int) this.getField(this.dataTableName + ".Term2", (object) 0);
      set => this.setField(this.dataTableName + ".Term2", (object) value);
    }

    public Decimal Coupon
    {
      get => (Decimal) this.getField(this.dataTableName + ".Coupon", (object) 0M);
      set => this.setField(this.dataTableName + ".Coupon", (object) value);
    }

    public Decimal Price
    {
      get => (Decimal) this.getField(this.dataTableName + ".Price", (object) 0M);
      set => this.setField(this.dataTableName + ".Price", (object) value);
    }

    public string DealerName
    {
      get => this.getField(this.dataTableName + ".DealerName", (object) "") as string;
      set => this.setField(this.dataTableName + ".DealerName", (object) value);
    }

    public new Decimal TradeAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TradeAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".TradeAmount", (object) value);
    }

    public Decimal Tolerance
    {
      get => (Decimal) this.getField(this.dataTableName + ".Tolerance", (object) 0M);
      set => this.setField(this.dataTableName + ".Tolerance", (object) value);
    }

    public Decimal MinAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MinAmount", (object) value);
    }

    public Decimal MaxAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxAmount", (object) value);
    }

    public DateTime CommitmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".CommitmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".CommitmentDate", (object) value);
    }

    public DateTime ConfirmDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".ConfirmDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".ConfirmDate", (object) value);
    }

    public DateTime SettlementDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".SettlementDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".SettlementDate", (object) value);
    }

    public DateTime NotificationDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".NotificationDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".NotificationDate", (object) value);
    }

    public Decimal OptionPremium
    {
      get => (Decimal) this.getField(this.dataTableName + ".OptionPremium", (object) 0M);
      set => this.setField(this.dataTableName + ".OptionPremium", (object) value);
    }

    public new TradeStatus Status
    {
      get
      {
        if ((TradeStatus) this.getField("Trades.Status", (object) TradeStatus.None) == TradeStatus.Archived)
          return TradeStatus.Archived;
        return this.CommitmentDate != DateTime.MinValue ? TradeStatus.Committed : TradeStatus.Open;
      }
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public bool Archived
    {
      get => this.Status == TradeStatus.Archived;
      set => this.tradeStatus = value ? 5 : 1;
    }

    public new bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public DateTime LastModified
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".LastModified", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".LastModified", (object) value);
    }

    public bool IsHidden
    {
      get => (bool) this.getField(this.dataTableName + ".IsHidden", (object) false);
      set => this.setField(this.dataTableName + ".IsHidden", (object) value);
    }

    public Decimal AssignedProfit
    {
      get => (Decimal) this.getField("SecurityTradeSummary.TotalProfit", (object) 0M);
      set => this.setField("SecurityTradeSummary.TotalProfit", (object) value);
    }

    public new Decimal AssignedAmount
    {
      get => (Decimal) this.getField("SecurityTradeSummary.TotalAmount", (object) 0M);
      set => this.setField("SecurityTradeSummary.TotalAmount", (object) value);
    }

    public new Decimal CompletionPercent
    {
      get => (Decimal) this.getField("SecurityTradeSummary.CompletionPercent", (object) 0M);
      set => this.setField("SecurityTradeSummary.CompletionPercent", (object) value);
    }

    public Decimal OpenAmount
    {
      get => (Decimal) this.getField("SecurityTradeDetails.OpenAmount", (object) 0M);
      set => this.setField("SecurityTradeDetails.OpenAmount", (object) value);
    }

    public Decimal TotalPairOffGainLoss
    {
      get => (Decimal) this.getField("SecurityTradeDetails.PairOffGainLoss", (object) 0M);
      set => this.setField("SecurityTradeDetails.PairOffGainLoss", (object) value);
    }

    public override string ToString() => this.Name;
  }
}
