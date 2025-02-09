// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IGseCommitmentManager
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
  public interface IGseCommitmentManager
  {
    List<GSECommitmentInfo> GetAllTrades();

    GSECommitmentInfo GetTrade(int tradeId);

    GseCommitmentEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization);

    GseCommitmentEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization,
      int sqlRead);

    int CreateTrade(GSECommitmentInfo loanTrade);

    void UpdateTrade(GSECommitmentInfo loanTrade, bool isExternalOrganization);

    GSECommitmentInfo GetTradeByName(string tradeName);

    GSECommitmentInfo GetTradeByContractNumber(string contractNumber);

    void AddTradeHistoryItem(GseCommitmentHistoryItem item);

    ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      string[] fields,
      bool summaryOnly,
      bool isExternalOrganization);

    void DeleteTrade(int tradeId);

    void SetTradeStatus(int tradeId, TradeStatus status);

    void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck);

    GSECommitmentViewModel GetTradeViewForLoan(string loanGuid);

    GSECommitmentInfo GetTradeForLoan(string loanGuid);

    GSECommitmentInfo GetTradeForRejectedLoan(string loanGuid);

    GSECommitmentViewModel[] GetActiveTradeView();

    GSECommitmentViewModel[] GetTradeViewsByContractID(int contractId);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      GseCommitmentLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      GseCommitmentLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan);

    PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization);

    PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization,
      int sqlRead);

    void CommitPendingTradeStatus(int tradeId, string loanGuid, GseCommitmentLoanStatus status);

    void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      GseCommitmentLoanStatus status,
      bool rejected);

    GseCommitmentHistoryItem[] GetTradeHistoryForLoan(string loanGuid);

    void UpdateAssignedAmountToTrade(int tradeId, int assigneeTradeId, Decimal assignedAmount);

    GSECommitmentAssignment[] GetTradeAssigmentsByMbsPool(int tradeId);

    GSECommitmentAssignment[] GetUnassignedTradeAssigmentsByMbsPool(int tradeId);

    void UpdateTradeAfterAssignSecurityTrade(GSECommitmentInfo tradeInfo);

    List<GSECommitmentInfo> ValidateContractNumbers(List<string> commitmentContractNums);
  }
}
