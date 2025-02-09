// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeViewModel
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradeViewModel : TradeViewModel
  {
    private Dictionary<CorrespondentTradeLoanStatus, int> pendingLoanCounts;

    protected override string dataTableName => "CorrespondentTradeDetails";

    public int CommitmentID
    {
      get => (int) this.getField(this.dataTableName + ".CorrespondentMasterID", (object) -1);
      set => this.setField(this.dataTableName + ".CorrespondentMasterID", (object) value);
    }

    public int ExternalOriginatorManagementID
    {
      get
      {
        return (int) this.getField(this.dataTableName + ".ExternalOriginatorManagementID", (object) -1);
      }
      set => this.setField(this.dataTableName + ".ExternalOriginatorManagementID", (object) value);
    }

    public string CorrespondentMasterCommitmentNumber
    {
      get => this.getField(this.dataTableName + ".ContractNumber", (object) "") as string;
      set => this.setField(this.dataTableName + ".ContractNumber", (object) value);
    }

    public string DeliveryType
    {
      get => (string) this.getField(this.dataTableName + ".DeliveryType");
      set => this.setField(this.dataTableName + ".DeliveryType", (object) value);
    }

    public DateTime ExpirationDate
    {
      get => (DateTime) this.getField(this.dataTableName + ".ExpirationDate");
      set => this.setField(this.dataTableName + ".ExpirationDate", (object) value);
    }

    public DateTime DeliveryExpirationDate
    {
      get => (DateTime) this.getField(this.dataTableName + ".DeliveryExpirationDate");
      set => this.setField(this.dataTableName + ".DeliveryExpirationDate", (object) value);
    }

    public string AOTSecurityType
    {
      get => (string) this.getField(this.dataTableName + ".AOTSecurityType");
      set => this.setField(this.dataTableName + ".AOTSecurityType", (object) value);
    }

    public string AOTSecurityTerm
    {
      get => (string) this.getField(this.dataTableName + ".AOTSecurityTerm");
      set => this.setField(this.dataTableName + ".AOTSecurityTerm", (object) value);
    }

    public Decimal AOTSecurityCoupon
    {
      get => (Decimal) this.getField(this.dataTableName + ".AOTSecurityCoupon");
      set => this.setField(this.dataTableName + ".AOTSecurityCoupon", (object) value);
    }

    public Decimal AOTSecurityPrice
    {
      get => (Decimal) this.getField(this.dataTableName + ".AOTSecurityPrice");
      set => this.setField(this.dataTableName + ".AOTSecurityPrice", (object) value);
    }

    public DateTime AOTSettlementDate
    {
      get => (DateTime) this.getField(this.dataTableName + ".AOTSettlementDate");
      set => this.setField(this.dataTableName + ".AOTSettlementDate", (object) value);
    }

    public string CompanyName
    {
      get => (string) this.getField(this.dataTableName + ".CompanyName");
      set => this.setField(this.dataTableName + ".CompanyName", (object) value);
    }

    public string TPOID
    {
      get => (string) this.getField(this.dataTableName + ".ExternalID");
      set => this.setField(this.dataTableName + ".ExternalID", (object) value);
    }

    public string OrganizationID
    {
      get => (string) this.getField(this.dataTableName + ".OrganizationID");
      set => this.setField(this.dataTableName + ".OrganizationID", (object) value);
    }

    public DateTime AOTOriginalTradeDate
    {
      get => (DateTime) this.getField(this.dataTableName + ".AOTOriginalTradeDate");
      set => this.setField(this.dataTableName + ".AOTOriginalTradeDate", (object) value);
    }

    public string AOTOriginalTradeDealer
    {
      get => (string) this.getField(this.dataTableName + ".AOTOriginalTradeDealer");
      set => this.setField(this.dataTableName + ".AOTOriginalTradeDealer", (object) value);
    }

    private TradeStatus tradeStatus
    {
      get => (TradeStatus) this.getField(this.dataTableName + ".Status", (object) -1);
      set => this.setField(this.dataTableName + ".Status", (object) value);
    }

    public string CommitmentType
    {
      get => (string) this.getField(this.dataTableName + ".CommitmentType");
      set => this.setField(this.dataTableName + ".CommitmentType", (object) value);
    }

    public DateTime LastModified
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".LastModified", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".LastModified", (object) value);
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

    public bool Voided
    {
      get => this.Status == TradeStatus.Voided;
      set => this.tradeStatus = value ? TradeStatus.Voided : TradeStatus.Committed;
    }

    public new bool Locked
    {
      get
      {
        return this.Status == TradeStatus.Archived || (bool) this.getField(this.dataTableName + ".Locked", (object) false);
      }
      set => this.setField(this.dataTableName + ".Locked", (object) value);
    }

    public new Decimal TradeAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".TradeAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".TradeAmount", (object) value);
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

    public Decimal Tolerance
    {
      get => (Decimal) this.getField(this.dataTableName + ".Tolerance", (object) 0M);
      set => this.setField(this.dataTableName + ".Tolerance", (object) value);
    }

    public virtual Decimal PairOffFee
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffFee", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffFee", (object) value);
    }

    public DateTime CommitmentDate
    {
      get
      {
        return (DateTime) this.getField(this.dataTableName + ".CommitmentDate", (object) this.DEFAULT_DATETIME);
      }
      set => this.setField(this.dataTableName + ".CommitmentDate", (object) value);
    }

    public int LoanCount
    {
      get => (int) this.getField("TradeCorrespondentTradeSummary.LoanCount", (object) 0);
      set => this.setField("TradeCorrespondentTradeSummary.LoanCount", (object) value);
    }

    public Decimal AssignedProfit
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.TotalProfit", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.TotalProfit", (object) value);
    }

    public new Decimal AssignedAmount
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.TotalAmount", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.TotalAmount", (object) value);
    }

    public int PendingLoanCount
    {
      get => (int) this.getField("TradeCorrespondentTradeSummary.PendingLoanCount", (object) 0);
      set => this.setField("TradeCorrespondentTradeSummary.PendingLoanCount", (object) value);
    }

    public Dictionary<CorrespondentTradeLoanStatus, int> PendingLoanCounts
    {
      get => this.pendingLoanCounts;
      set => this.pendingLoanCounts = value;
    }

    public new Decimal CompletionPercent
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.CompletionPercent", (object) 0M);
      }
      set => this.setField("TradeCorrespondentTradeSummary.CompletionPercent", (object) value);
    }

    public override Decimal PairOffAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".PairOffAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".PairOffAmount", (object) value);
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

    public Decimal OpenAmount
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.OpenAmount", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.OpenAmount", (object) value);
    }

    public Decimal GainLossAmount
    {
      get => (Decimal) this.getField(this.dataTableName + ".GainLossAmount", (object) 0M);
      set => this.setField(this.dataTableName + ".GainLossAmount", (object) value);
    }

    public bool AutoCreated
    {
      get => (bool) this.getField(this.dataTableName + ".AutoCreated", (object) false);
      set => this.setField(this.dataTableName + ".AutoCreated", (object) value);
    }

    public string TradeDescription
    {
      get => (string) this.getField(this.dataTableName + ".TradeDescription");
      set => this.setField(this.dataTableName + ".TradeDescription", (object) value);
    }

    public bool IsWeightedAvgBulkPriceLocked
    {
      get
      {
        return (bool) this.getField(this.dataTableName + ".IsWeightedAvgBulkPriceLocked", (object) false);
      }
      set => this.setField(this.dataTableName + ".IsWeightedAvgBulkPriceLocked", (object) value);
    }

    public Decimal WeightedAvgBulkPrice
    {
      get => (Decimal) this.getField(this.dataTableName + ".WeightedAvgBulkPrice", (object) 0M);
      set => this.setField(this.dataTableName + ".WeightedAvgBulkPrice", (object) value);
    }

    public int WithdrawnLoanCount => this.LoanCount - this.NotWithdrawnLoanCount;

    public int NotWithdrawnLoanCount
    {
      get
      {
        return (int) this.getField("TradeCorrespondentTradeSummary.NotWithdrawnLoanCount", (object) 0);
      }
      set => this.setField("TradeCorrespondentTradeSummary.NotWithdrawnLoanCount", (object) value);
    }

    public bool IsToleranceLocked
    {
      get => (bool) this.getField(this.dataTableName + ".IsToleranceLocked", (object) false);
      set => this.setField(this.dataTableName + ".IsToleranceLocked", (object) value);
    }

    public string FundType
    {
      get => (string) this.getField(this.dataTableName + ".FundType");
      set => this.setField(this.dataTableName + ".FundType", (object) value);
    }

    public string OriginationRepWarrantType
    {
      get => (string) this.getField(this.dataTableName + ".OriginationRepWarrantType");
      set => this.setField(this.dataTableName + ".OriginationRepWarrantType", (object) value);
    }

    public string AgencyName
    {
      get => (string) this.getField(this.dataTableName + ".AgencyName");
      set => this.setField(this.dataTableName + ".AgencyName", (object) value);
    }

    public string AgencyDeliveryType
    {
      get => (string) this.getField(this.dataTableName + ".AgencyDeliveryType");
      set => this.setField(this.dataTableName + ".AgencyDeliveryType", (object) value);
    }

    public string DocCustodian
    {
      get => (string) this.getField(this.dataTableName + ".DocCustodian");
      set => this.setField(this.dataTableName + ".DocCustodian", (object) value);
    }

    public string AuthorizedTraderUserId
    {
      get => (string) this.getField(this.dataTableName + ".AuthorizedTraderUserId");
      set => this.setField(this.dataTableName + ".AuthorizedTraderUserId", (object) value);
    }

    public string AuthorizedTraderName
    {
      get => (string) this.getField(this.dataTableName + ".AuthorizedTraderName");
      set => this.setField(this.dataTableName + ".AuthorizedTraderName", (object) value);
    }

    public string AuthorizedTraderEmail
    {
      get => (string) this.getField(this.dataTableName + ".AuthorizedTraderEmail");
      set => this.setField(this.dataTableName + ".AuthorizedTraderEmail", (object) value);
    }

    public string LastPublishedDateTime
    {
      get => (string) this.getField(this.dataTableName + ".LastPublishedDateTime");
      set => this.setField(this.dataTableName + ".LastPublishedDateTime", (object) value);
    }

    public Decimal DeliveredAmount
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.DeliveredAmount", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.DeliveredAmount", (object) value);
    }

    public Decimal DeliveredPercentage
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.DeliveredPercentage", (object) 0M);
      }
      set => this.setField("TradeCorrespondentTradeSummary.DeliveredPercentage", (object) value);
    }

    public Decimal PurchasedAmount
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.PurchasedAmount", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.PurchasedAmount", (object) value);
    }

    public Decimal PurchasedPercentage
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.PurchasedPercentage", (object) 0M);
      }
      set => this.setField("TradeCorrespondentTradeSummary.PurchasedPercentage", (object) value);
    }

    public Decimal RejectedAmount
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.RejectedAmount", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.RejectedAmount", (object) value);
    }

    public Decimal RejectedPercentage
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.RejectedPercentage", (object) 0M);
      }
      set => this.setField("TradeCorrespondentTradeSummary.RejectedPercentage", (object) value);
    }

    public Decimal MinNetOpen
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.MinNetOpen", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.MinNetOpen", (object) value);
    }

    public Decimal MaxNetOpen
    {
      get => (Decimal) this.getField("TradeCorrespondentTradeSummary.MaxNetOpen", (object) 0M);
      set => this.setField("TradeCorrespondentTradeSummary.MaxNetOpen", (object) value);
    }

    public Decimal PurchasedLoanAmount
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.PurchasedLoanAmount", (object) 0M);
      }
      set => this.setField("TradeCorrespondentTradeSummary.PurchasedLoanAmount", (object) value);
    }

    public Decimal PurchasedLoanAmountPercentage
    {
      get
      {
        return (Decimal) this.getField("TradeCorrespondentTradeSummary.PurchasedLoanAmountPercentage", (object) 0M);
      }
      set
      {
        this.setField("TradeCorrespondentTradeSummary.PurchasedLoanAmountPercentage", (object) value);
      }
    }

    public int GetPendingLoanCount(CorrespondentTradeLoanStatus status)
    {
      return this.tradeStatus == TradeStatus.Pending || this.pendingLoanCounts == null || !this.pendingLoanCounts.ContainsKey(status) ? 0 : this.pendingLoanCounts[status];
    }

    public int GetPendingLoanCount()
    {
      if (this.pendingLoanCounts == null)
        return 0;
      int pendingLoanCount = 0;
      foreach (int num in this.pendingLoanCounts.Values)
        pendingLoanCount += num;
      return pendingLoanCount;
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
