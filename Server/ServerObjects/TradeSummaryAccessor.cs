// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.TradeSummaryAccessor
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.TradeSummary;
using EllieMae.EMLite.Trading;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class TradeSummaryAccessor
  {
    public static TradeSummaryInfo GetTradeSummary(string loanGuid)
    {
      TradeSummaryInfo tradeSummary = new TradeSummaryInfo();
      LoanTradeViewModel tradeViewForLoan = LoanTrades.GetTradeViewForLoan(loanGuid);
      MbsPoolViewModel mbsPoolViewModel = (MbsPoolViewModel) null;
      if (tradeViewForLoan == null)
        mbsPoolViewModel = MbsPools.GetTradeViewForLoan(loanGuid);
      if (tradeViewForLoan != null)
      {
        tradeSummary.TradeId = tradeViewForLoan.Guid;
        tradeSummary.TradeType = TradeType.LoanTrade.ToString();
        tradeSummary.TradeName = tradeViewForLoan.Name;
        tradeSummary.InvestorName = tradeViewForLoan.InvestorName;
        tradeSummary.MasterContract = tradeViewForLoan.ContractNumber;
        tradeSummary.InvestorTradeNumber = tradeViewForLoan.InvestorTradeNumber;
        tradeSummary.InvestorCommitmentNumber = tradeViewForLoan.InvestorCommitmentNumber;
        tradeSummary.TradeAmount = tradeViewForLoan.TradeAmount;
        tradeSummary.Tolerance = tradeViewForLoan.Tolerance;
        tradeSummary.TotalPairOffAmount = tradeViewForLoan.PairOffAmount;
        tradeSummary.CommitmentDate = tradeViewForLoan.CommitmentDate;
        tradeSummary.InvestorDeliveryDate = tradeViewForLoan.InvestorDeliveryDate;
        tradeSummary.EarlyDeliveryDate = tradeViewForLoan.EarlyDeliveryDate;
        tradeSummary.TargetDeliveryDate = tradeViewForLoan.TargetDeliveryDate;
        tradeSummary.ActualDeliveryDate = tradeViewForLoan.ShipmentDate;
        tradeSummary.PurchaseDate = tradeViewForLoan.PurchaseDate;
      }
      else if (mbsPoolViewModel != null)
      {
        tradeSummary.TradeId = mbsPoolViewModel.Guid;
        tradeSummary.TradeType = TradeType.MbsPool.ToString();
        tradeSummary.TradeName = mbsPoolViewModel.Name;
        tradeSummary.InvestorName = mbsPoolViewModel.InvestorName;
        tradeSummary.MasterContract = mbsPoolViewModel.ContractNumber;
        tradeSummary.TradeAmount = mbsPoolViewModel.TradeAmount;
        tradeSummary.CommitmentDate = mbsPoolViewModel.CommitmentDate;
        tradeSummary.InvestorDeliveryDate = mbsPoolViewModel.InvestorDeliveryDate;
        tradeSummary.EarlyDeliveryDate = mbsPoolViewModel.EarlyDeliveryDate;
        tradeSummary.TargetDeliveryDate = mbsPoolViewModel.TargetDeliveryDate;
        tradeSummary.ActualDeliveryDate = mbsPoolViewModel.ShipmentDate;
        tradeSummary.PurchaseDate = mbsPoolViewModel.PurchaseDate;
      }
      tradeSummary.LoanHistory = TradeSummaryAccessor.getLoanHistory(loanGuid);
      return tradeSummary;
    }

    private static List<LoanHistory> getLoanHistory(string loanGuid)
    {
      List<LoanHistory> loanHistory = new List<LoanHistory>();
      ITradeHistoryItem[] tradeHistoryForLoan = (ITradeHistoryItem[]) LoanTrades.GetTradeHistoryForLoan(loanGuid);
      if (tradeHistoryForLoan == null || tradeHistoryForLoan.Length == 0)
        tradeHistoryForLoan = (ITradeHistoryItem[]) MbsPools.GetTradeHistoryForLoan(loanGuid);
      if (tradeHistoryForLoan != null && tradeHistoryForLoan.Length != 0)
      {
        foreach (ITradeHistoryItem tradeHistoryItem in tradeHistoryForLoan)
          loanHistory.Add(new LoanHistory()
          {
            EventTime = tradeHistoryItem.Timestamp,
            Event = tradeHistoryItem.StatusDescription,
            TradeNumber = tradeHistoryItem.TradeName,
            InvestorName = tradeHistoryItem.InvestorName,
            Comments = tradeHistoryItem.Comment
          });
      }
      return loanHistory;
    }
  }
}
