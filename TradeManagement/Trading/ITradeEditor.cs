// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.ITradeEditor
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public interface ITradeEditor : ITradeEditorBase
  {
    ToolTip ToolTip { get; }

    TabPage LoanToTradeAssignmentTab { get; }

    bool Loading { get; set; }

    bool LoanUpdatesRequired { get; }

    object Assignments { get; }

    bool RemovePendingLoanFromOtherTrades { get; set; }

    void LoadTradeData();

    string[] GetPricingAndEligibilityFields();

    bool IsNoteRateAllowed(PipelineInfo pinfo);

    string GetTradeStatusDescription(LoanToTradeAssignmentBase assignmentInfo);

    string GetLoanStatusDescription(object value);

    void CommitChanges();

    bool SaveTrade(bool forceUpdateOfLoans, bool updatedSelectedLoans);

    Decimal CalculatePriceIndex(PipelineInfo info);

    Decimal CalculatePriceIndex(PipelineInfo info, Decimal securityPrice);

    Decimal CalculateProfit(PipelineInfo info, Decimal securityPrice);

    ICursor GetEligibleLoanCursor(
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      string[] excludedGuids,
      EligibleLoanFilterOption filterOption = EligibleLoanFilterOption.All);

    List<TradeLoanUpdateError> AssignLoanToTrade(PipelineInfo[] pinfos);

    void RemoveLoanFromTrade(string guid, bool rejected);

    string[] GetLoanToTradeAssignmentAllLoanGuids();

    List<LoanToTradeAssignmentBase> GetLoanToTradeAssignments();

    PipelineInfo[] GetLoanToTradeAssignedPipelineData();

    int GetLoanToTradePendingAssignmentCount();

    int GetLoanToTradePendingShipmentCount();

    int GetLoanToTradePendingRemovalCount();

    int GetLoanToTradePendingPurchaseCount();

    bool ValidateLoanToTradeAssignment(LoanToTradeAssignmentBase assignment, out string errMsg);

    string[] GetLoanToTradeAssignedAndRejectedLoanGuids();

    string[] GetLoanToTradeAssignedAndRejectedLoanNumbers();

    void MarkLoanToTradeAssignmentStatusToShipped(string loanGuid);

    void MarkLoanToTradeAssignmentStatusToPurchasedPending(string loanGuid);

    void CommitLoanToTradeAssignments(bool forceUpdateOfAllLoans, bool selectedLoans);

    bool IsLoanToTradeAssignmentPending(LoanToTradeAssignmentBase assignment);

    List<LoanToTradeAssignmentBase> GetLoanToTradeAssignedLoans();

    List<LoanToTradeAssignmentBase> GetAllAssignedPendingLoans();

    Decimal GetOpenAmount();

    void MakePending(bool value);
  }
}
