// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ICorrespondentTradeManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ICorrespondentTradeManager
  {
    List<CorrespondentTradeInfo> GetAllTrades();

    CorrespondentTradeInfo GetTrade(int tradeId);

    CorrespondentTradeInfo GetTrade(string tradeName);

    CorrespondentTradeEditorScreenData GetTradeEditorScreenData(
      int tradeId,
      string[] assignedLoanFields,
      bool isExternalOrganization);

    int CreateTrade(CorrespondentTradeInfo correspondentTrade);

    void UpdateTrade(CorrespondentTradeInfo correspondentTrade, bool checkStatus);

    void PublishTrade(CorrespondentTradeInfo correspondentTrade, bool checkStatus);

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
      CorrespondentTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo trade,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    ICursor GetEligibleLoanCursor(
      CorrespondentTradeInfo[] trades,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortOrder,
      string[] guidsToOmit,
      bool isExternalOrganization,
      EligibleLoanFilterOption filterOption);

    void AddTradeHistoryItem(CorrespondentTradeHistoryItem item);

    ICursor OpenTradeCursor(
      QueryCriterion[] criteria,
      SortField[] sortOrder,
      bool isExternalOrganization);

    void DeleteTrade(int tradeId);

    void SetTradeStatus(
      int[] tradeIds,
      TradeStatus status,
      TradeHistoryAction action,
      bool needPendingCheck);

    void UpdateTradeStatus(int tradeId, TradeStatus status, bool needPendingCheck);

    CorrespondentTradeViewModel GetTradeViewForLoan(string loanGuid);

    CorrespondentTradeInfo GetTradeForLoan(string loanGuid);

    CorrespondentTradeInfo GetTradeForRejectedLoan(string loanGuid);

    CorrespondentTradeViewModel[] GetActiveTradeView();

    CorrespondentTradeViewModel[] GetTradeViewsByMasterId(int masterId);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan);

    void SetPendingTradeStatuses(
      int tradeId,
      string[] loanGuids,
      CorrespondentTradeLoanStatus[] statuses,
      string[] comments,
      bool isExternalOrganization,
      bool removePendingLoan,
      Decimal[] totalPrices,
      bool forceUpdateAllLoans);

    PipelineInfo[] GetAssignedOrPendingLoans(
      int tradeId,
      string[] fields,
      bool isExternalOrganization);

    void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status);

    void CommitPendingTradeStatus(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status,
      bool rejected);

    CorrespondentTradeHistoryItem[] GetTradeHistoryForLoan(string loanGuid);

    CorrespondentTradeHistoryItem[] GetTradeHistoryForTrade(int tradeId);

    CorrespondentMasterInfo[] GetCorrespondentMasterInfos(CorrespondentTradeInfo tradeInfo);

    List<CorrespondentTradeInfo> GetTradeInfosByMasterId(int masterId);

    List<CorrespondentTradeInfo> GetTradeInfosByExternalOrgId(int tpoId);

    Dictionary<CorrespondentMasterDeliveryType, Decimal> GetOutStandingCommitments(int externalOrgId);

    bool CheckTradeByName(string name);

    bool CheckExistingTradeByName(string name);

    string GetNextAutoCreateTradeName(string name, string loanDUID);

    string GenerateNextAutoNumber();

    Dictionary<int, string> GetEligibleCorrespondentMastersByTradeId(int tradeId);

    Dictionary<int, string> GetEligibleCorrespondentTradeByLoanInfo(
      string externalId,
      string deliveryType,
      double loanAmount);

    Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      string deliveryType,
      string loanNumber);

    Dictionary<int, string> GetEligibleCorrespondentTradeByLoanNumber(
      string deliveryType,
      string loanNumber,
      string correspondentMasterNumber);

    bool IsTradeAssignmentUpdateCompleted(
      int tradeId,
      string loanGuid,
      CorrespondentTradeLoanStatus status);

    void VoidAssigedPendingLoanAssignment(int tradeId, string[] loanGuids);

    Decimal CalculateTPOAvailableAmount(
      MasterCommitmentType commitmentType,
      ExternalOriginatorManagementData tpoSettings);

    void UpdateAssignmentsWithTradeExtension(
      int tradeId,
      string[] loanGuids,
      string tradeExtensionInfo);

    void PublishKafkaEvent(string eventType, int tradeId, Hashtable eventPayload);

    TradeStatus GetTradeStatus(int tradeId);
  }
}
