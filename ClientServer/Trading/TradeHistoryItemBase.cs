// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeHistoryItemBase
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public abstract class TradeHistoryItemBase
  {
    public const string TradeNameProperty = "TradeName�";
    public const string UserNameProperty = "UserName�";
    public const string CommentProperty = "Comment�";
    private int historyId = -1;
    private int tradeId = -1;
    private string userId = "";
    protected TradeHistoryAction action;
    protected int status = -1;
    private DateTime timestamp = DateTime.MinValue;
    private XmlDictionary<string> data = new XmlDictionary<string>();
    private string dataXml = string.Empty;

    public TradeHistoryItemBase() => this.data = new XmlDictionary<string>();

    public TradeHistoryItemBase(string dataXml)
    {
      this.dataXml = dataXml;
      try
      {
        this.data = XmlDictionary<string>.Parse(this.dataXml);
      }
      catch
      {
      }
      if (this.data != null)
        return;
      this.data = new XmlDictionary<string>();
    }

    public TradeHistoryItemBase(
      int historyId,
      int tradeId,
      TradeHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml)
      : this(dataXml)
    {
      this.historyId = historyId;
      this.tradeId = tradeId;
      this.action = action;
      this.status = status;
      this.timestamp = timestamp;
      this.userId = userId;
    }

    public TradeHistoryItemBase(int tradeId, TradeHistoryAction action, UserInfo user)
    {
      this.tradeId = tradeId;
      this.status = -1;
      this.action = action;
      if (!(user != (UserInfo) null))
        return;
      this.userId = user.Userid;
      this.data[nameof (UserName)] = user.FullName;
    }

    public TradeHistoryItemBase(int tradeId, TradeStatus status, UserInfo user)
    {
      this.tradeId = tradeId;
      this.action = TradeHistoryAction.TradeStatusChanged;
      this.status = (int) status;
      if (!(user != (UserInfo) null))
        return;
      this.userId = user.Userid;
      this.data[nameof (UserName)] = user.FullName;
    }

    public int HistoryID => this.historyId;

    public int TradeID
    {
      get => this.tradeId;
      set => this.tradeId = value;
    }

    public string UserID
    {
      get => this.userId;
      set => this.userId = value;
    }

    public string TradeName
    {
      get => this.data[nameof (TradeName)];
      set => this.data[nameof (TradeName)] = value;
    }

    public string Comment
    {
      get
      {
        try
        {
          return this.data[nameof (Comment)];
        }
        catch
        {
          return "";
        }
      }
      set => this.data[nameof (Comment)] = value;
    }

    [CLSCompliant(false)]
    public TradeHistoryAction Action
    {
      get => this.action;
      set => this.action = value;
    }

    [CLSCompliant(false)]
    public int Status
    {
      get => this.status;
      set => this.status = value;
    }

    public DateTime Timestamp
    {
      get => this.timestamp;
      set => this.timestamp = value;
    }

    public string UserName
    {
      get => this.data[nameof (UserName)];
      set => this.data[nameof (UserName)] = value;
    }

    public XmlDictionary<string> Data => this.data;

    public virtual string Description
    {
      get
      {
        string subDescription = this.getSubDescription();
        if (!string.IsNullOrEmpty(subDescription))
          return subDescription;
        switch (this.action)
        {
          case TradeHistoryAction.TradeCreated:
            return "Trade created";
          case TradeHistoryAction.TradeLocked:
            return "Trade locked";
          case TradeHistoryAction.TradeUnlocked:
            return "Trade unlocked";
          case TradeHistoryAction.TradeArchived:
            return "Trade archived";
          case TradeHistoryAction.TradeActivated:
            return "Trade activated";
          case TradeHistoryAction.TradeStatusChanged:
            return "Trade marked as " + new TradeStatusEnumNameProvider().GetName((object) (TradeStatus) this.status);
          case TradeHistoryAction.UnlockPendingTrade:
            return "Pending Trade is unlocked.";
          case TradeHistoryAction.TradeVoided:
            return "Trade voided.";
          case TradeHistoryAction.AssigneeAssigned:
            return "Trade assignee assigned";
          case TradeHistoryAction.AssigneeUnassigned:
            return "Trade assignee unassigned";
          case TradeHistoryAction.AssigneeChanged:
            return "Trade assignee changed";
          case TradeHistoryAction.AssignedAmountChanged:
            return "Trade assignee amount changed";
          case TradeHistoryAction.LoanUpdateErrors:
            return "Trade loan updated";
          case TradeHistoryAction.LoanUpdateCancelled:
            return "Trade Loan Update cancelled by " + this.Data["Comment"];
          case TradeHistoryAction.LoanUpdatePending:
            return "Loan Update Pending";
          case TradeHistoryAction.LoanUpdateCompleted:
            return "Completed Result : " + this.Comment;
          case TradeHistoryAction.LoanUpdateCleared:
            return "Trade Loan Update was cleared by " + this.Comment;
          case TradeHistoryAction.TradePublished:
            return "Trade published.";
          case TradeHistoryAction.TradeUpdated:
            return "Trade updated.";
          case TradeHistoryAction.PairOffCreated:
            return "Pair off created.";
          case TradeHistoryAction.PairOffUpdated:
            return "Pair off updated.";
          case TradeHistoryAction.PairOffReversed:
            return "Pair off reversed.";
          case TradeHistoryAction.PairOffDeleted:
            return "Pair off deleted.";
          case TradeHistoryAction.StatusChangedManually:
            return "Status changed manually as " + new TradeStatusEnumNameProvider().GetName((object) (TradeStatus) this.status);
          default:
            return "No description available";
        }
      }
    }

    protected virtual string getSubDescription() => string.Empty;

    public string StatusDescription
    {
      get
      {
        switch (this.action)
        {
          case TradeHistoryAction.TradeCreated:
            return "Created";
          case TradeHistoryAction.TradeLocked:
            return "Locked";
          case TradeHistoryAction.TradeUnlocked:
            return "Unlocked";
          case TradeHistoryAction.TradeArchived:
            return "Archived";
          case TradeHistoryAction.TradeActivated:
            return "Activated";
          case TradeHistoryAction.TradeStatusChanged:
          case TradeHistoryAction.StatusChangedManually:
            return new TradeStatusEnumNameProvider().GetName((object) (TradeStatus) this.status);
          case TradeHistoryAction.LoanAssigned:
          case TradeHistoryAction.ContractAssigned:
            return "Assigned";
          case TradeHistoryAction.LoanRemoved:
          case TradeHistoryAction.ContractUnassigned:
            return "Removed";
          case TradeHistoryAction.LoanStatusChanged:
            return new LoanTradeStatusEnumNameProvider().GetName((object) (LoanTradeStatus) this.status);
          case TradeHistoryAction.LoanRejected:
            return "Rejected";
          default:
            return "No description available";
        }
      }
    }

    public override string ToString() => this.Description;
  }
}
