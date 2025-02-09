// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoanTradeManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ILoanTradeManager
  {
    List<LoanTradeInfo> GetAllTrades();

    LoanTradeInfo GetTrade(int tradeId);

    TradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization);

    TradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization,
      int sqlRead);

    int CreateTrade(LoanTradeInfo loanTrade);

    void UpdateTrade(LoanTradeInfo loanTrade, bool checkStatus);

    void UpdateTrade(LoanTradeInfo loanTrade, bool checkStatus, bool isUpdateStatus = true);

    LoanTradeInfo GetTradeByName(string tradeName);

    ICursor GetEligibleLoanCursor(
      int tradeId,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      int[] tradeIds,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      LoanTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      LoanTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      LoanTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      LoanTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    void AddTradeHistoryItem(LoanTradeHistoryItem item);

    ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization);

    void DeleteTrade(int tradeId);

    void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck);

    LoanTradeViewModel GetTradeViewForLoan(string loanGuid);

    LoanTradeInfo GetTradeForLoan(string loanGuid);

    LoanTradeInfo GetTradeForRejectedLoan(string loanGuid);

    LoanTradeViewModel[] GetActiveTradeView();

    LoanTradeViewModel[] GetTradeViewsByContractID(int contractId);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      LoanTradeStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrices,
      bool forceUpdateAllLoans);

    PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization);

    void CommitPendingTradeStatus(int tradeId, string loanGuid, LoanTradeStatus status);

    void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      LoanTradeStatus status,
      bool rejected);

    LoanTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid);

    LoanTradeInfo GetTrade(string tradeNumber);

    Dictionary<int, string> GetEligibleLoanTradeByLoanInfo();

    bool IsTradeAssignmentUpdateCompleted(int tradeId, string loanGuid, LoanTradeStatus status);

    List<TradeUnlockInfo> GetPendingTrades(List<TradeType> tradeTypes, int timeWait);

    List<TradeInfoObj> GetPendingTrades(List<TradeType> tradeTypes);

    TradeStatus GetTradeStatus(int tradeId);
  }
}
