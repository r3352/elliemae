// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.SecurityTradeHistoryItem
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.RemotingServices;
using System;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  [Serializable]
  public class SecurityTradeHistoryItem : TradeHistoryItemBase, ITradeHistoryItem
  {
    public const string DealerNameProperty = "DealerName�";
    private int assigneeTradeId = -1;

    public string InvestorName => string.Empty;

    public SecurityTradeHistoryItem(
      int historyId,
      int tradeId,
      TradeHistoryAction action,
      int status,
      DateTime timestamp,
      string userId,
      string dataXml)
      : base(historyId, tradeId, action, status, timestamp, userId, dataXml)
    {
    }

    public SecurityTradeHistoryItem(SecurityTradeInfo trade, int status, UserInfo user)
      : this(trade, TradeHistoryAction.TradeStatusChanged, status, user)
    {
    }

    public SecurityTradeHistoryItem(
      SecurityTradeInfo trade,
      TradeHistoryAction action,
      int status,
      UserInfo user)
      : base(trade.TradeID, action, user)
    {
      if (trade == null)
        return;
      this.TradeID = trade.TradeID;
      this.Data["TradeName"] = trade.Name;
      this.Data["DealerName"] = trade.DealerName;
    }

    public SecurityTradeHistoryItem(
      SecurityTradeInfo trade,
      TradeHistoryAction action,
      UserInfo user)
      : base(trade.TradeID, action, user)
    {
      if (trade == null)
        return;
      this.TradeID = trade.TradeID;
      this.Data["TradeName"] = trade.Name;
      this.Data["DealerName"] = trade.DealerName;
    }

    public SecurityTradeHistoryItem(int tradeId, TradeHistoryAction action, UserInfo user)
      : base(tradeId, action, user)
    {
    }

    public SecurityTradeHistoryItem(SecurityTradeInfo trade, TradeStatus action, UserInfo user)
      : base(trade.TradeID, action, user)
    {
      if (trade == null)
        return;
      this.TradeID = trade.TradeID;
      this.Data["TradeName"] = trade.Name;
      this.Data["DealerName"] = trade.DealerName;
    }

    public SecurityTradeHistoryItem(int tradeId, TradeStatus status, UserInfo user)
      : base(tradeId, status, user)
    {
    }

    public int AssigneeTradeID
    {
      get => this.assigneeTradeId;
      set => this.assigneeTradeId = value;
    }

    protected override string getSubDescription()
    {
      switch (this.Action)
      {
        case TradeHistoryAction.AssigneeAssigned:
          return "Loan trade assigned";
        case TradeHistoryAction.AssigneeUnassigned:
          return "Loan trade unassigned";
        default:
          return "";
      }
    }
  }
}
