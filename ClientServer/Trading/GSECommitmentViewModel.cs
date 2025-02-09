// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GSECommitmentViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class GSECommitmentViewModel : TradeViewModel
  {
    protected override string dataTableName => "GseCommitmentDetails";

    public int LoanCount
    {
      get => (int) this.getField("TradeMbsPoolSummary.LoanCount", (object) 0);
      set => this.setField("TradeMbsPoolSummary.LoanCount", (object) value);
    }

    public string ContractNumber
    {
      get => this.getField(this.dataTableName + ".ContractNumber", (object) "") as string;
      set => this.setField(this.dataTableName + ".ContractNumber", (object) value);
    }

    public string TradeDescription
    {
      get => this.getField(this.dataTableName + ".TradeDescription", (object) "") as string;
      set => this.setField(this.dataTableName + ".TradeDescription", (object) value);
    }

    public DateTime CommitmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".CommitmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".CommitmentDate", (object) value);
    }

    public string SellerNumber
    {
      get => this.getField(this.dataTableName + ".SellerNumber", (object) "") as string;
      set => this.setField(this.dataTableName + ".SellerNumber", (object) value);
    }

    public Decimal CommitmentAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".CommitmentAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".CommitmentAmount", (object) value);
    }

    public Decimal OutstandingBalance
    {
      get => (Decimal) this.getField(this.dataTableName + ".OutstandingBalance", (object) 0M);
      set => this.setField(this.dataTableName + ".OutstandingBalance", (object) value);
    }

    public DateTime IssueMonth
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".IssueMonth", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".IssueMonth", (object) value);
    }

    public Decimal MinDeliveryAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinDeliveryAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MinDeliveryAmount", (object) value);
    }

    public Decimal MaxDeliveryAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxDeliveryAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxDeliveryAmount", (object) value);
    }

    public Decimal FulfilledAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".FulfilledAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".FulfilledAmount", (object) value);
    }

    public Decimal PendingAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".PendingAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".PendingAmount", (object) value);
    }

    public Decimal MinRemainingAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MinRemainingAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MinRemainingAmount", (object) value);
    }

    public Decimal MaxRemainingAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxRemainingAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".MaxRemainingAmount", (object) value);
    }

    public Decimal PairOffFeeFactor
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffFeeFactor", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffFeeFactor", (object) value);
    }

    public Decimal RollFeeFactor
    {
      get => (Decimal) this.getField(this.dataTableName + ".RollFeeFactor", (object) 0M);
      set => this.setField(this.dataTableName + ".RollFeeFactor", (object) value);
    }

    public string RemittanceCycle
    {
      get => (string) this.getField(this.dataTableName + ".RemittanceCycle", (object) "");
      set => this.setField(this.dataTableName + ".RemittanceCycle", (object) value);
    }

    public int RemittanceCycleMonth
    {
      get => (int) this.getField(this.dataTableName + ".RemittanceDayOfMonth");
      set => this.setField(this.dataTableName + ".RemittanceDayOfMonth", (object) value);
    }

    public string ServicingOption
    {
      get => (string) this.getField(this.dataTableName + ".ServicingOption", (object) "");
      set => this.setField(this.dataTableName + ".ServicingOption", (object) value);
    }

    public Decimal ParticipationPercent
    {
      get => (Decimal) this.getField(this.dataTableName + ".ParticipationPercent");
      set => this.setField(this.dataTableName + ".ParticipationPercent", (object) value);
    }

    public string BuyupBuydownGrid
    {
      get => this.getField(this.dataTableName + ".BuyupBuydownGrid", (object) "") as string;
      set => this.setField(this.dataTableName + ".BuyupBuydownGrid", (object) value);
    }

    public Decimal MaxBuyupAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".MaxBuyupAmount");
      set => this.setField(this.dataTableName + ".MaxBuyupAmount", (object) value);
    }

    public DateTime LastModified
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".LastModified", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".LastModified", (object) value);
    }

    private TradeStatus tradeStatus
    {
      get => (TradeStatus) this.getField(this.dataTableName + ".Status", (object) -1);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public override TradeStatus Status
    {
      get => (TradeStatus) this.getField(this.dataTableName + ".Status", (object) TradeStatus.None);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public bool Archived
    {
      get => this.Status == TradeStatus.Archived;
      set => this.tradeStatus = value ? TradeStatus.Archived : TradeStatus.Open;
    }

    public new bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public override int TradeID
    {
      get => (int) this.getField(this.dataTableName + ".TradeID", (object) -1);
      set => this.setField(this.dataTableName + ".TradeID", (object) value);
    }

    public override string Guid
    {
      get => this.getField(this.dataTableName + ".Guid", (object) "") as string;
      set => this.setField(this.dataTableName + ".Guid", (object) value);
    }

    public override string Name
    {
      get => this.getField(this.dataTableName + ".Name", (object) "") as string;
      set => this.setField(this.dataTableName + ".Name", (object) value);
    }

    public override Decimal CompletionPercent
    {
      get => (Decimal) this.getField("GseCommitmentSummary.CompletionPercent", (object) 0M);
      set => this.setField("GseCommitmentSummary.CompletionPercent", (object) value);
    }

    public Decimal OpenAmount
    {
      get => (Decimal) this.getField("GseCommitmentDetails.OpenAmount", (object) 0M);
      set => this.setField("GseCommitmentDetails.OpenAmount", (object) value);
    }

    public string RolledTo
    {
      get => this.getField(this.dataTableName + ".RolledTo", (object) "") as string;
      set => this.setField(this.dataTableName + ".RolledTo", (object) value);
    }

    public string RolledFrom
    {
      get => this.getField(this.dataTableName + ".RolledFrom", (object) "") as string;
      set => this.setField(this.dataTableName + ".RolledFrom", (object) value);
    }

    public string BondType
    {
      get => this.getField(this.dataTableName + ".BondType", (object) "") as string;
      set => this.setField(this.dataTableName + ".BondType", (object) value);
    }

    public Decimal MinGFeeAfterBuydown
    {
      get => (Decimal) this.getField("GseCommitmentDetails.MinGFeeAfterBuydown", (object) 0M);
      set => this.setField("GseCommitmentDetails.MinGFeeAfterBuydown", (object) value);
    }

    public Decimal FalloutAmount
    {
      get => (Decimal) this.getField("GseCommitmentDetails.FalloutAmount", (object) 0M);
      set => this.setField("GseCommitmentDetails.FalloutAmount", (object) value);
    }

    public Decimal Fees
    {
      get => (Decimal) this.getField("GseCommitmentDetails.Fees", (object) 0M);
      set => this.setField("GseCommitmentDetails.Fees", (object) value);
    }

    public Decimal RolledAmount
    {
      get => (Decimal) this.getField("GseCommitmentDetails.RolledAmount", (object) 0M);
      set => this.setField("GseCommitmentDetails.RolledAmount", (object) value);
    }

    public override string ToString() => this.Name;

    public override object this[string propertyName]
    {
      get
      {
        return propertyName == this.dataTableName + ".Status" ? (object) (int) this.Status : base[propertyName];
      }
      set => base[propertyName] = value;
    }
  }
}
