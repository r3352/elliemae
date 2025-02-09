// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class LoanTradeHistoryItem : TradeHistoryItemBase, ITradeHistoryItem
  {
    public const string InvestorNameProperty = "InvestorName�";
    public const string ContractNumberProperty = "ContractNumber�";
    public const string LoanNumberProperty = "LoanNumber�";
    public static readonly string[] RequiredPipelineInfoFields = new string[1]
    {
      "Loan.LoanNumber"
    };
    private int contractId = -1;
    private string loanGuid;

    public LoanTradeHistoryItem(
      int historyId,
      int tradeId,
      int contractId,
      string loanGuid,
      TradeHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml)
      : base(historyId, tradeId, action, status, timestamp, userId, dataXml)
    {
      this.contractId = contractId;
      this.loanGuid = loanGuid;
    }

    private LoanTradeHistoryItem(
      LoanTradeInfo trade,
      MasterContractSummaryInfo contract,
      PipelineInfo loanInfo,
      TradeHistoryAction action,
      int status,
      UserInfo user)
    {
      this.Action = action;
      this.Status = status;
      if (trade != null)
      {
        this.TradeID = trade.TradeID;
        this.Data["TradeName"] = trade.Name;
        this.Data[nameof (InvestorName)] = trade.InvestorName;
        this.Data[nameof (ContractNumber)] = trade.ContractNumber;
      }
      if (user != (UserInfo) null)
      {
        this.UserID = user.Userid;
        this.Data["UserName"] = user.FullName;
      }
      if (contract != null)
      {
        this.contractId = contract.ContractID;
        this.Data[nameof (ContractNumber)] = contract.ContractNumber;
      }
      if (loanInfo == null)
        return;
      this.loanGuid = loanInfo.GUID;
      this.Data[nameof (LoanNumber)] = string.Concat(loanInfo.GetField(nameof (LoanNumber)));
    }

    public LoanTradeHistoryItem(LoanTradeInfo trade, TradeHistoryAction action, UserInfo user)
      : this(trade, (MasterContractSummaryInfo) null, (PipelineInfo) null, action, -1, user)
    {
    }

    public LoanTradeHistoryItem(LoanTradeInfo trade, TradeStatus status, UserInfo user)
      : this(trade, (MasterContractSummaryInfo) null, (PipelineInfo) null, TradeHistoryAction.TradeStatusChanged, (int) status, user)
    {
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      PipelineInfo loanInfo,
      LoanTradeStatus status,
      UserInfo user)
      : this(trade, (MasterContractSummaryInfo) null, loanInfo, TradeHistoryAction.LoanStatusChanged, (int) status, user)
    {
      if (status == LoanTradeStatus.Assigned)
      {
        this.Action = TradeHistoryAction.LoanAssigned;
      }
      else
      {
        if (status != LoanTradeStatus.Unassigned)
          return;
        this.Action = TradeHistoryAction.LoanRemoved;
      }
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      PipelineInfo loanInfo,
      LoanTradeStatus status,
      string comment,
      UserInfo user)
      : this(trade, loanInfo, status, user)
    {
      this.Comment = comment;
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      PipelineInfo loanInfo,
      string rejectedByInvestorName,
      UserInfo user)
      : this(trade, (MasterContractSummaryInfo) null, loanInfo, TradeHistoryAction.LoanRejected, -1, user)
    {
      this.Data[nameof (InvestorName)] = rejectedByInvestorName;
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      MasterContractSummaryInfo contract,
      TradeHistoryAction action,
      UserInfo user)
      : this(trade, contract, (PipelineInfo) null, action, -1, user)
    {
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      TradeHistoryAction action,
      string comment,
      UserInfo user)
      : this(trade.TradeID, action, user)
    {
      this.Comment = comment;
    }

    public LoanTradeHistoryItem(
      LoanTradeInfo trade,
      PipelineInfo loanInfo,
      TradeHistoryAction action,
      string comment,
      UserInfo user)
      : this(trade, (MasterContractSummaryInfo) null, loanInfo, action, -1, user)
    {
      this.Comment = comment;
    }

    public LoanTradeHistoryItem(int tradeId, TradeHistoryAction action, UserInfo user)
      : base(tradeId, action, user)
    {
    }

    public string InvestorName => this.Data[nameof (InvestorName)];

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
          return "Loan '" + this.LoanNumber + "' marked as " + new LoanTradeStatusEnumNameProvider().GetName((object) (LoanTradeStatus) this.Status);
        case TradeHistoryAction.LoanRejected:
          return "Loan '" + this.LoanNumber + "' rejected by investor '" + this.InvestorName + "'";
        case TradeHistoryAction.ContractAssigned:
          return "Added to contract '" + this.ContractNumber + "'";
        case TradeHistoryAction.ContractUnassigned:
          return "Removed from contract '" + this.ContractNumber + "'";
        case TradeHistoryAction.LoanUpdateErrors:
          return "Loan '" + this.LoanNumber + "': " + this.Data["Comment"];
        default:
          return string.Empty;
      }
    }
  }
}
