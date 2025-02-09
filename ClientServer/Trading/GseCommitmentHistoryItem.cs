// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.GseCommitmentHistoryItem
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
  public class GseCommitmentHistoryItem : TradeHistoryItemBase
  {
    public const string LoanNumberProperty = "LoanNumber�";
    public static readonly string[] RequiredPipelineInfoFields = new string[1]
    {
      "Loan.LoanNumber"
    };
    private string loanGuid;

    public GseCommitmentHistoryItem(
      int historyId,
      int tradeId,
      string loanGuid,
      TradeHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml)
      : base(historyId, tradeId, action, status, timestamp, userId, dataXml)
    {
      this.loanGuid = loanGuid;
    }

    private GseCommitmentHistoryItem(
      GSECommitmentInfo trade,
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
      }
      if (user != (UserInfo) null)
      {
        this.UserID = user.Userid;
        this.Data["UserName"] = user.FullName;
      }
      if (loanInfo == null)
        return;
      this.loanGuid = loanInfo.GUID;
      this.Data[nameof (LoanNumber)] = string.Concat(loanInfo.GetField(nameof (LoanNumber)));
    }

    public GseCommitmentHistoryItem(
      GSECommitmentInfo trade,
      TradeHistoryAction action,
      UserInfo user)
      : this(trade, (PipelineInfo) null, action, -1, user)
    {
    }

    public GseCommitmentHistoryItem(GSECommitmentInfo trade, TradeStatus status, UserInfo user)
      : this(trade, (PipelineInfo) null, TradeHistoryAction.TradeStatusChanged, (int) status, user)
    {
    }

    public GseCommitmentHistoryItem(int tradeId, TradeHistoryAction action, UserInfo user)
      : base(tradeId, action, user)
    {
    }

    public string LoanGuid => this.loanGuid;

    public string LoanNumber => this.Data[nameof (LoanNumber)];

    protected override string getSubDescription()
    {
      return this.Action == TradeHistoryAction.LoanStatusChanged ? "Loan '" + this.LoanNumber + "' marked as " + new GseCommitmentStatusEnumNameProvider().GetName((object) (GseCommitmentLoanStatus) this.Status) : string.Empty;
    }

    public override string Description
    {
      get
      {
        string subDescription = this.getSubDescription();
        if (!string.IsNullOrEmpty(subDescription))
          return subDescription;
        switch (this.action)
        {
          case TradeHistoryAction.TradeCreated:
            return "Commitment created";
          case TradeHistoryAction.TradeLocked:
            return "Commitment locked";
          case TradeHistoryAction.TradeUnlocked:
            return "Trade unlocked";
          case TradeHistoryAction.TradeArchived:
            return "Commitment archived";
          case TradeHistoryAction.TradeActivated:
            return "Commitment activated";
          case TradeHistoryAction.TradeStatusChanged:
            return "Commitment marked as " + new TradeStatusEnumNameProvider().GetName((object) (TradeStatus) this.status);
          case TradeHistoryAction.AssigneeAssigned:
            return "Fannie Mae PE MBS Pool assigned";
          case TradeHistoryAction.AssigneeUnassigned:
            return "Fannie Mae PE MBS Pool unassigned";
          case TradeHistoryAction.AssigneeChanged:
            return "Fannie Mae PE MBS Pool changed";
          case TradeHistoryAction.AssignedAmountChanged:
            return "Trade assignee amount changed";
          case TradeHistoryAction.GSEAssigneeAssigned:
            return "Fannie Mae PE MBS Pool assigned";
          case TradeHistoryAction.GSEAssigneeUnassigned:
            return "Fannie Mae PE MBS Pool unassigned";
          default:
            return "No description available";
        }
      }
    }
  }
}
