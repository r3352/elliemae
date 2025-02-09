// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IMbsPoolManager
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
  public interface IMbsPoolManager
  {
    List<MbsPoolInfo> GetAllTrades();

    MbsPoolInfo GetTrade(int tradeId);

    MbsPoolEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization);

    int CreateTrade(MbsPoolInfo loanTrade);

    void UpdateTrade(MbsPoolInfo loanTrade, bool isExternalOrganization, bool checkStatus);

    MbsPoolInfo GetTradeByName(string tradeName);

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
      MbsPoolInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      MbsPoolInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      MbsPoolInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    void AddTradeHistoryItem(MbsPoolHistoryItem item);

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

    MbsPoolViewModel GetTradeViewForLoan(string loanGuid);

    MbsPoolInfo GetTradeForLoan(string loanGuid);

    MbsPoolInfo GetTradeForRejectedLoan(string loanGuid);

    MbsPoolViewModel[] GetActiveTradeView();

    MbsPoolViewModel[] GetTradeViewsByContractID(int contractId);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      MbsPoolLoanStatus[] statuses,
      string[] comments,
      string[] commitmentContractNumber,
      string[] productName,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetCommitmentInfo(
      int tradeId,
      string[] loanGuids,
      string[] commitmentContractNumber,
      string[] productName,
      bool isExternalOrganization);

    PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization);

    void CommitPendingTradeStatus(int tradeId, string loanGuid, MbsPoolLoanStatus status);

    void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      MbsPoolLoanStatus status,
      bool rejected);

    MbsPoolHistoryItem[] GetTradeHistoryForLoan(string loanGuid);

    void AssignSecurityTradeToTrade(int tradeId, int assigneeTradeId);

    void AssignSecurityTradeToTrade(int tradeId, int assigneeTradeId, Decimal assignedAmount);

    void AssignSecurityTradeToTrade(
      int tradeId,
      int assigneeTradeId,
      MbsPoolSecurityTradeStatus status,
      DateTime assignedStatusDate);

    void UnassignSecurityTradeToTrade(int tradeId, int assigneeTradeId);

    void UpdateAssignedAmountToTrade(int tradeId, int assigneeTradeId, Decimal assignedAmount);

    void UnassignGSECommitmentToTrade(int tradeId, int assigneeTradeId);

    void AssignGSECommitmentToTrade(int tradeId, int assigneeTradeId, Decimal assignedAmount);

    MbsPoolAssignment[] GetTradeAssigments(int tradeId);

    MbsPoolAssignment[] GetUnassignedTradeAssigments(int tradeId);

    MbsPoolAssignment[] GetTradeAssigmentsBySecurityTrade(int tradeId);

    MbsPoolAssignment[] GetUnassignedTradeAssigmentsBySecurityTrade(int tradeId);

    MbsPoolAssignment[] GetTradeAssigmentsByGseCommitment(int tradeId);

    MbsPoolAssignment[] GetUnassignedTradeAssigmentsByGseCommitment(int tradeId);

    void UpdateTradeAfterAssignSecurityTrade(MbsPoolInfo tradeInfo);

    bool IsTradeAssignmentUpdateCompleted(int tradeId, string loanGuid, MbsPoolLoanStatus status);

    TradeStatus GetTradeStatus(int tradeId);

    MbsPoolMortgageType GetMbsPoolMortgageType(string tradeGuid);
  }
}
