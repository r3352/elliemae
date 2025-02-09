// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.Trades
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public static class Trades
  {
    public static string CreateTrade(
      TradeType tradeType,
      object tradeContract,
      UserInfo currentUser)
    {
      switch (tradeType)
      {
        case TradeType.SecurityTrade:
        case TradeType.LoanTrade:
        case TradeType.MbsPool:
        case TradeType.CorrespondentMaster:
        case TradeType.MasterContract:
        case TradeType.GSECommitment:
          throw new NotImplementedException(string.Format("CreateTrade is not implemented for trades of type {0}.", (object) tradeType.ToString()));
        case TradeType.CorrespondentTrade:
          return CorrespondentTrades.CreateTrade((CorrespondentTradeInfo) tradeContract, currentUser).ToString();
        default:
          throw new ApplicationException("Error fetching Trade Information from database.");
      }
    }

    public static KeyValuePair<TradeType, object> GetTrade(int tradeId)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          return new KeyValuePair<TradeType, object>(TradeType.SecurityTrade, (object) SecurityTrades.GetTrade(tradeId));
        case TradeType.LoanTrade:
          return new KeyValuePair<TradeType, object>(TradeType.LoanTrade, (object) LoanTrades.GetTrade(tradeId));
        case TradeType.MbsPool:
          return new KeyValuePair<TradeType, object>(TradeType.MbsPool, (object) MbsPools.GetTrade(tradeId));
        case TradeType.CorrespondentTrade:
          return new KeyValuePair<TradeType, object>(TradeType.CorrespondentTrade, (object) CorrespondentTrades.GetTrade(tradeId));
        case TradeType.CorrespondentMaster:
          return new KeyValuePair<TradeType, object>(TradeType.CorrespondentMaster, (object) CorrespondentMasters.GetCorrespondentMaster(tradeId));
        case TradeType.MasterContract:
          return new KeyValuePair<TradeType, object>(TradeType.MasterContract, (object) MasterContracts.GetContract(tradeId));
        case TradeType.GSECommitment:
          return new KeyValuePair<TradeType, object>(TradeType.GSECommitment, (object) GseCommitments.GetTrade(tradeId));
        default:
          throw new ApplicationException("Error fetching Trade Information from database.");
      }
    }

    public static List<string> GetAssignedLoanGuids(int tradeId)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          throw new NotImplementedException();
        case TradeType.MbsPool:
          throw new NotImplementedException();
        case TradeType.CorrespondentTrade:
          return CorrespondentTrades.GetAssignedLoans(tradeId);
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }

    public static List<string> GetEligibleLoanGuids(UserInfo user, int tradeId, int? maxCount = null)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          throw new NotImplementedException();
        case TradeType.MbsPool:
          throw new NotImplementedException();
        case TradeType.CorrespondentTrade:
          return CorrespondentTrades.GetEligibleLoanGuids(user, tradeId, maxCount);
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }

    public static void AssignLoan(SessionObjects sessionObjects, int tradeId, string loanGuid)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          throw new NotImplementedException();
        case TradeType.MbsPool:
          throw new NotImplementedException();
        case TradeType.CorrespondentTrade:
          throw new NotImplementedException();
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }

    public static void RemoveLoan(SessionObjects sessionObjects, int tradeId, string loanGuid)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          throw new NotImplementedException();
        case TradeType.MbsPool:
          throw new NotImplementedException();
        case TradeType.CorrespondentTrade:
          throw new NotImplementedException();
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }

    public static TradeType GetTradeType(int tradeId)
    {
      DbQueryBuilder dbQueryBuilder = new DbQueryBuilder();
      dbQueryBuilder.AppendFormat("SELECT TradeType FROM Trades WHERE TradeID = {0}", (object) tradeId);
      object tradeType = dbQueryBuilder.ExecuteScalar();
      if (tradeType == null)
      {
        dbQueryBuilder.Reset();
        dbQueryBuilder.AppendFormat("SELECT Count(*) FROM CorrespondentMaster WHERE CorrespondentMasterID = {0}", (object) tradeId);
        object obj = dbQueryBuilder.ExecuteScalar();
        if (obj != null && int.Parse(obj.ToString()) == 1)
          return TradeType.CorrespondentMaster;
      }
      else if (Enum.IsDefined(typeof (TradeType), tradeType))
        return (TradeType) tradeType;
      return TradeType.None;
    }

    public static void UpdateTradeStatus(
      int tradeId,
      TradeStatus status,
      UserInfo currentUser,
      bool needPendingCheck)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          LoanTrades.UpdateTradeStatus(tradeId, status, currentUser, needPendingCheck);
          break;
        case TradeType.MbsPool:
          MbsPools.UpdateTradeStatus(tradeId, status, currentUser, needPendingCheck);
          break;
        case TradeType.CorrespondentTrade:
          CorrespondentTrades.UpdateTradeStatus(tradeId, status, currentUser, needPendingCheck);
          break;
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }

    public static void AddTradeHistoryItem(
      int tradeId,
      TradeHistoryAction tradeHistoryAction,
      string message,
      UserInfo currentUser)
    {
      switch (Trades.GetTradeType(tradeId))
      {
        case TradeType.SecurityTrade:
          throw new NotImplementedException();
        case TradeType.LoanTrade:
          LoanTradeInfo trade1 = new LoanTradeInfo();
          trade1.TradeID = tradeId;
          LoanTrades.AddTradeHistoryItem(new LoanTradeHistoryItem(trade1, tradeHistoryAction, message, currentUser));
          break;
        case TradeType.MbsPool:
          MbsPoolInfo trade2 = new MbsPoolInfo();
          trade2.TradeID = tradeId;
          MbsPools.AddTradeHistoryItem(new MbsPoolHistoryItem(trade2, tradeHistoryAction, message, currentUser));
          break;
        case TradeType.CorrespondentTrade:
          CorrespondentTradeInfo trade3 = new CorrespondentTradeInfo();
          trade3.TradeID = tradeId;
          CorrespondentTrades.AddTradeHistoryItem(new CorrespondentTradeHistoryItem(trade3, tradeHistoryAction, message, currentUser));
          break;
        case TradeType.CorrespondentMaster:
          throw new NotImplementedException();
        case TradeType.MasterContract:
          throw new NotImplementedException();
        case TradeType.GSECommitment:
          throw new NotImplementedException();
        default:
          throw new NotImplementedException();
      }
    }
  }
}
