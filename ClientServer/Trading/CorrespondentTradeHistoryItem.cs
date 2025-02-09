// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class CorrespondentTradeHistoryItem : TradeHistoryItemBase, ITradeHistoryItem
  {
    public const string ContractNumberProperty = "ContractNumber�";
    public const string LoanNumberProperty = "LoanNumber�";
    public static readonly string[] RequiredPipelineInfoFields = new string[1]
    {
      "Loan.LoanNumber"
    };
    private int contractId = -1;
    private string loanGuid;
    private XmlDictionary<string> priorTradeValues = new XmlDictionary<string>();
    private string priorTradeXml;

    public XmlDictionary<string> PriorTradeValues => this.priorTradeValues;

    public string InvestorName => string.Empty;

    public CorrespondentTradeHistoryItem(
      int historyId,
      int tradeId,
      int contractId,
      string loanGuid,
      TradeHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml,
      string priorDataXml)
      : base(historyId, tradeId, action, status, timestamp, userId, dataXml)
    {
      this.contractId = contractId;
      this.loanGuid = loanGuid;
      if (string.IsNullOrEmpty(priorDataXml))
        return;
      this.priorTradeXml = priorDataXml;
      try
      {
        this.priorTradeValues = XmlDictionary<string>.Parse(this.priorTradeXml);
      }
      catch
      {
      }
    }

    private CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      CorrespondentMasterInfo correspondentMasterInfo,
      PipelineInfo loanInfo,
      TradeHistoryAction action,
      int status,
      UserInfo user,
      CorrespondentTradeInfo priorTrade = null)
    {
      this.Action = action;
      this.Status = status;
      if (trade != null)
      {
        this.TradeCreatedHistoryData(trade);
        if (priorTrade != null)
        {
          this.priorTradeValues = new XmlDictionary<string>();
          this.TradeUpdatedHistoryData(trade, priorTrade);
        }
      }
      if (user != (UserInfo) null)
      {
        this.UserID = user.Userid;
        this.Data["UserName"] = user.FullName;
      }
      if (correspondentMasterInfo != null)
      {
        this.contractId = correspondentMasterInfo.ID;
        this.Data[nameof (ContractNumber)] = correspondentMasterInfo.Name;
      }
      if (loanInfo == null)
        return;
      this.loanGuid = loanInfo.GUID;
      this.Data[nameof (LoanNumber)] = string.Concat(loanInfo.GetField(nameof (LoanNumber)));
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      TradeHistoryAction action,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, (CorrespondentMasterInfo) null, (PipelineInfo) null, action, -1, user, priorValue)
    {
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      TradeHistoryAction action,
      TradeStatus status,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, (CorrespondentMasterInfo) null, (PipelineInfo) null, action, (int) status, user, priorValue)
    {
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      TradeStatus status,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, (CorrespondentMasterInfo) null, (PipelineInfo) null, TradeHistoryAction.TradeStatusChanged, (int) status, user, priorValue)
    {
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      PipelineInfo loanInfo,
      CorrespondentTradeLoanStatus status,
      UserInfo user,
      bool rejected = false,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, (CorrespondentMasterInfo) null, loanInfo, TradeHistoryAction.LoanStatusChanged, (int) status, user, priorValue)
    {
      if (rejected)
        this.Action = TradeHistoryAction.LoanRejected;
      else if (status == CorrespondentTradeLoanStatus.Assigned)
      {
        this.Action = TradeHistoryAction.LoanAssigned;
      }
      else
      {
        if (status != CorrespondentTradeLoanStatus.Unassigned)
          return;
        this.Action = TradeHistoryAction.LoanRemoved;
      }
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      PipelineInfo loanInfo,
      CorrespondentTradeLoanStatus status,
      string comment,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, loanInfo, status, user, priorValue: priorValue)
    {
      this.Comment = comment;
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      CorrespondentMasterInfo correspondentMasterInfo,
      TradeHistoryAction action,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, correspondentMasterInfo, (PipelineInfo) null, action, -1, user, priorValue)
    {
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      TradeHistoryAction action,
      string comment,
      UserInfo user)
      : this(trade.TradeID, action, user)
    {
      this.Comment = comment;
    }

    public CorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade,
      PipelineInfo loanInfo,
      TradeHistoryAction action,
      string comment,
      UserInfo user,
      CorrespondentTradeInfo priorValue = null)
      : this(trade, (CorrespondentMasterInfo) null, loanInfo, action, -1, user, priorValue)
    {
      this.Comment = comment;
    }

    public CorrespondentTradeHistoryItem(int tradeId, TradeHistoryAction action, UserInfo user)
      : base(tradeId, action, user)
    {
    }

    public CorrespondentTradeHistoryItem(
      int tradeId,
      TradeHistoryAction action,
      DateTime lastPublishDateTime,
      TradeStatus tradeStatus,
      UserInfo user)
      : base(tradeId, action, user)
    {
      this.Status = (int) tradeStatus;
      this.TradeID = tradeId;
      this.Data["LastPublishedDateTime"] = lastPublishDateTime.ToString() + " PST";
      this.Data["TradeStatus"] = tradeStatus.ToDescription();
    }

    private void TradeCreatedHistoryData(CorrespondentTradeInfo trade)
    {
      this.TradeID = trade.TradeID;
      this.Data["TradeName"] = trade.Name;
      this.Data["CommitmentDate"] = trade.CommitmentDate.ToString("MM/dd/yyyy");
      this.Data["TradeDescription"] = trade.TradeDescription;
      this.Data["ContractNumber"] = trade.CorrespondentMasterCommitmentNumber;
      this.Data["DeliveryType"] = trade.DeliveryType.ToString();
      this.Data["TradeAmount"] = trade.TradeAmount.ToString("#,0.00#");
      this.Data["Tolerance"] = trade.Tolerance.ToString("N8");
      this.Data["MinAmount"] = trade.MinAmount.ToString("#,0.00#");
      this.Data["MaxAmount"] = trade.MaxAmount.ToString("#,0.00#");
      this.Data["WeightedAvgBulkPrice"] = trade.WeightedAvgBulkPrice.ToString("#,0.00#");
      this.Data["ExpirationDate"] = trade.ExpirationDate.ToString("MM/dd/yyyy");
      this.Data["DeliveryExpirationDate"] = trade.DeliveryExpirationDate.ToString("MM/dd/yyyy");
      this.Data["TotalPairOffAmount"] = trade.PairOffAmount.ToString("#,0.00#");
      this.Data["TotalPairOffGainLoss"] = trade.GainLossAmount.ToString("#,0.00#");
      this.Data["AuthorizedTraderName"] = trade.AuthorizedTraderName;
      XmlDictionary<string> data1 = this.Data;
      DateTime dateTime;
      string empty1;
      if (!(trade.AOTOriginalTradeDate == DateTime.MinValue))
      {
        dateTime = trade.AOTOriginalTradeDate;
        empty1 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        empty1 = string.Empty;
      data1["OriginalTradeDate"] = empty1;
      this.Data["OriginalTradeDealer"] = trade.AOTOriginalTradeDealer;
      this.Data["SecurityType"] = trade.AOTSecurityType;
      this.Data["SecurityTerm"] = trade.AOTSecurityTerm;
      XmlDictionary<string> data2 = this.Data;
      Decimal num;
      string empty2;
      if (!(trade.AOTSecurityCoupon == 0M))
      {
        num = trade.AOTSecurityCoupon;
        empty2 = num.ToString("#,0.00000#");
      }
      else
        empty2 = string.Empty;
      data2["SecurityCoupon"] = empty2;
      XmlDictionary<string> data3 = this.Data;
      string empty3;
      if (!(trade.AOTSecurityPrice == 0M))
      {
        num = trade.AOTSecurityPrice;
        empty3 = num.ToString("#,0.0000000#");
      }
      else
        empty3 = string.Empty;
      data3["SecurityPrice"] = empty3;
      XmlDictionary<string> data4 = this.Data;
      string empty4;
      if (!(trade.AOTSettlementDate == DateTime.MinValue))
      {
        dateTime = trade.AOTSettlementDate;
        empty4 = dateTime.ToString("MM/dd/yyyy");
      }
      else
        empty4 = string.Empty;
      data4["SettlementDate"] = empty4;
      this.Data["FundType"] = trade.FundType;
      this.Data["OriginationRepWarrantType"] = trade.OriginationRepWarrantType;
      this.Data["AgencyName"] = trade.AgencyName;
      this.Data["AgencyDeliveryType"] = trade.AgencyDeliveryType;
      this.Data["DocCustodian"] = trade.DocCustodian;
      if (trade.Filter == null || trade.Filter.FilterType != TradeFilterType.Simple || trade.Filter.GetSimpleFilter() == null || trade.Filter.GetSimpleFilter().NoteRateRange == null)
        return;
      Decimal minimum = trade.Filter.GetSimpleFilter().NoteRateRange.Minimum;
      Decimal maximum = trade.Filter.GetSimpleFilter().NoteRateRange.Maximum;
      this.Data["NoteRateRange"] = (minimum == Decimal.MinValue ? "N/A" : minimum.ToString("N4")) + " - " + (maximum == Decimal.MaxValue ? "N/A" : maximum.ToString("N4"));
    }

    private void TradeUpdatedHistoryData(
      CorrespondentTradeInfo trade,
      CorrespondentTradeInfo priorValue)
    {
      this.priorTradeValues["TradeName"] = priorValue.Name;
      this.priorTradeValues["CommitmentDate"] = priorValue.CommitmentDate.ToString("MM/dd/yyyy");
      this.priorTradeValues["TradeDescription"] = priorValue.TradeDescription;
      this.priorTradeValues["ContractNumber"] = priorValue.CorrespondentMasterCommitmentNumber;
      this.priorTradeValues["DeliveryType"] = priorValue.DeliveryType.ToString();
      this.priorTradeValues["TradeAmount"] = priorValue.TradeAmount.ToString("#,0.00#");
      this.priorTradeValues["Tolerance"] = priorValue.Tolerance.ToString("N8");
      this.priorTradeValues["MinAmount"] = priorValue.MinAmount.ToString("#,0.00#");
      this.priorTradeValues["MaxAmount"] = priorValue.MaxAmount.ToString("#,0.00#");
      this.priorTradeValues["WeightedAvgBulkPrice"] = priorValue.WeightedAvgBulkPrice.ToString("#,0.00#");
      this.priorTradeValues["ExpirationDate"] = priorValue.ExpirationDate.ToString("MM/dd/yyyy");
      this.priorTradeValues["DeliveryExpirationDate"] = priorValue.DeliveryExpirationDate.ToString("MM/dd/yyyy");
      this.priorTradeValues["TotalPairOffAmount"] = priorValue.PairOffAmount.ToString("#,0.00#");
      this.priorTradeValues["TotalPairOffGainLoss"] = priorValue.GainLossAmount.ToString("#,0.00#");
      this.priorTradeValues["AuthorizedTraderName"] = priorValue.AuthorizedTraderName;
      this.priorTradeValues["OriginalTradeDate"] = priorValue.AOTOriginalTradeDate == DateTime.MinValue ? string.Empty : priorValue.AOTOriginalTradeDate.ToString("MM/dd/yyyy");
      this.priorTradeValues["OriginalTradeDealer"] = priorValue.AOTOriginalTradeDealer;
      this.priorTradeValues["SecurityType"] = priorValue.AOTSecurityType;
      this.priorTradeValues["SecurityTerm"] = priorValue.AOTSecurityTerm;
      this.priorTradeValues["SecurityCoupon"] = priorValue.AOTSecurityCoupon == 0M ? string.Empty : priorValue.AOTSecurityCoupon.ToString("#,0.00000#");
      this.priorTradeValues["SecurityPrice"] = priorValue.AOTSecurityPrice == 0M ? string.Empty : priorValue.AOTSecurityPrice.ToString("#,0.0000000#");
      this.priorTradeValues["SettlementDate"] = priorValue.AOTSettlementDate == DateTime.MinValue ? string.Empty : priorValue.AOTSettlementDate.ToString("MM/dd/yyyy");
      this.priorTradeValues["FundType"] = priorValue.FundType;
      this.priorTradeValues["OriginationRepWarrantType"] = priorValue.OriginationRepWarrantType;
      this.priorTradeValues["AgencyName"] = priorValue.AgencyName;
      this.priorTradeValues["AgencyDeliveryType"] = priorValue.AgencyDeliveryType;
      this.priorTradeValues["DocCustodian"] = priorValue.DocCustodian;
      if (priorValue.Filter == null || priorValue.Filter.FilterType != TradeFilterType.Simple || priorValue.Filter.GetSimpleFilter() == null || priorValue.Filter.GetSimpleFilter().NoteRateRange == null)
        return;
      Decimal minimum = priorValue.Filter.GetSimpleFilter().NoteRateRange.Minimum;
      Decimal maximum = priorValue.Filter.GetSimpleFilter().NoteRateRange.Maximum;
      this.priorTradeValues["NoteRateRange"] = (minimum == Decimal.MinValue ? "N/A" : minimum.ToString("N4")) + " - " + (maximum == Decimal.MaxValue ? "N/A" : maximum.ToString("N4"));
    }

    public string LoanGuid => this.loanGuid;

    public string LoanNumber => this.Data[nameof (LoanNumber)];

    public int ContractID => this.contractId;

    public string ContractNumber => this.Data[nameof (ContractNumber)];

    protected override string getSubDescription()
    {
      switch (this.Action)
      {
        case TradeHistoryAction.LoanAssigned:
          return "Added loan '" + this.LoanNumber + "'";
        case TradeHistoryAction.LoanRemoved:
          return "Removed loan '" + this.LoanNumber + "'";
        case TradeHistoryAction.LoanStatusChanged:
          return "Loan '" + this.LoanNumber + "' marked as " + new CorrespondentTradeStatusEnumNameProvider().GetName((object) (CorrespondentTradeLoanStatus) this.Status);
        case TradeHistoryAction.LoanRejected:
          return "Loan '" + this.LoanNumber + "' is flagged as \"voided\".";
        case TradeHistoryAction.ContractAssigned:
          return "Added to master commitment '" + this.ContractNumber + "'";
        case TradeHistoryAction.ContractUnassigned:
          return "Removed from master commitment '" + this.ContractNumber + "'";
        case TradeHistoryAction.LoanUpdateErrors:
          return "Loan '" + this.LoanNumber + "': " + this.Data["Comment"];
        default:
          return string.Empty;
      }
    }
  }
}
