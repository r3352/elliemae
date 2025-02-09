// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeLoanAssignment
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class CorrespondentTradeLoanAssignment : LoanToTradeAssignmentBase
  {
    private CorrespondentTradeLoanStatus assignedStatus;
    private CorrespondentTradeLoanStatus initialPendingStatus;
    private CorrespondentTradeLoanStatus pendingStatus;

    public CorrespondentTradeLoanAssignment(
      SessionObjects sessionObjects,
      int tradeId,
      PipelineInfo pinfo)
      : base(sessionObjects, tradeId, pinfo)
    {
      this.assignedStatus = CorrespondentTradeLoanStatus.Unassigned;
      this.pendingStatus = CorrespondentTradeLoanStatus.None;
      this.rejected = false;
      if (tradeId > 0)
      {
        foreach (PipelineInfo.TradeInfo tradeAssignment in pinfo.TradeAssignments)
        {
          if (tradeAssignment.TradeID == tradeId)
          {
            this.assignedStatus = (CorrespondentTradeLoanStatus) tradeAssignment.AssignedStatus;
            this.pendingStatus = (CorrespondentTradeLoanStatus) tradeAssignment.PendingStatus;
            this.TotalPrice = tradeAssignment.TotalPrice;
            this.EPPSLoanProgramName = tradeAssignment.EPPSLoanProgramName;
            break;
          }
        }
      }
      this.initialPendingStatus = this.pendingStatus;
    }

    public CorrespondentTradeLoanStatus AssignedStatus => this.assignedStatus;

    public CorrespondentTradeLoanStatus PendingStatus
    {
      get => this.pendingStatus;
      set
      {
        this.pendingStatus = value != this.assignedStatus || this.initialPendingStatus != CorrespondentTradeLoanStatus.None ? value : CorrespondentTradeLoanStatus.None;
        if (this.Status == CorrespondentTradeLoanStatus.Unassigned)
          return;
        this.rejected = false;
      }
    }

    public CorrespondentTradeLoanStatus Status
    {
      get
      {
        return this.pendingStatus != CorrespondentTradeLoanStatus.None ? this.pendingStatus : this.assignedStatus;
      }
    }

    public bool Assigned => this.Status != CorrespondentTradeLoanStatus.Unassigned;

    public bool Removed => this.Pending && this.Status == CorrespondentTradeLoanStatus.Unassigned;

    public bool Modified => this.Pending && this.pendingStatus != this.initialPendingStatus;

    public bool Pending => this.pendingStatus != 0;

    public bool PendingOrAssigned => this.Pending || this.Assigned;

    public bool Rejected
    {
      get => this.rejected;
      set
      {
        this.rejected = value;
        if (!this.rejected)
          return;
        this.pendingStatus = CorrespondentTradeLoanStatus.Unassigned;
      }
    }

    public Decimal TotalPrice { get; set; }

    public string EPPSLoanProgramName { get; set; }

    public string TradeExtensionInfo { get; set; }

    public void ApplyPendingStatusToLoan(
      LoanDataMgr loanMgr,
      CorrespondentTradeInfo trade,
      List<string> skipFieldList,
      Dictionary<string, string> updateFiedList = null,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      if (this.Guid != loanMgr.LoanData.GUID)
        throw new Exception("The specified LoanDataMgr does not have the correct GUID");
      if (this.PendingStatus == CorrespondentTradeLoanStatus.None || this.pendingStatus == this.assignedStatus)
        return;
      if (this.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
      {
        if (!this.rejected)
          this.rejected = this.sessionObjects.CorrespondentTradeManager.GetTradeForRejectedLoan(loanMgr.LoanData.GUID) != null;
        loanMgr.RemoveFromTrade((TradeInfoObj) trade, this.rejected, skipFieldList);
      }
      else if (this.PendingStatus == CorrespondentTradeLoanStatus.Assigned)
      {
        loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, 0M, updateFiedList, syncOption);
      }
      else
      {
        if (this.AssignedStatus == CorrespondentTradeLoanStatus.Unassigned)
          loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, 0M);
        loanMgr.ModifyTradeStatus((TradeInfoObj) trade, (object) this.PendingStatus, skipFieldList, 0M);
      }
    }

    public void CommitPendingStatusToLoan(CorrespondentTradeInfo trade, bool checkCompletion = true)
    {
      if (this.PendingStatus == CorrespondentTradeLoanStatus.None)
        return;
      if (checkCompletion && !this.IsUpdateCompleted(this.pendingStatus))
        throw new Exception("Unexpected error during loan update");
      this.sessionObjects.CorrespondentTradeManager.CommitPendingTradeStatus(this.tradeId, this.Guid, this.pendingStatus, this.rejected);
      if (this.pendingStatus != this.assignedStatus && this.rejected)
        this.sessionObjects.CorrespondentTradeManager.AddTradeHistoryItem(this.createRejectedCorrespondentTradeHistoryItem(trade));
      if (this.sessionObjects.CorrespondentTradeManager.GetTradeForRejectedLoan(this.Guid) != null)
        return;
      this.CommitPendingStatus();
    }

    internal void ClearModifications() => this.initialPendingStatus = this.pendingStatus;

    internal void CommitPendingStatus()
    {
      this.assignedStatus = this.Status;
      this.pendingStatus = CorrespondentTradeLoanStatus.None;
      this.initialPendingStatus = CorrespondentTradeLoanStatus.None;
      this.rejected = false;
    }

    internal void ApplyNewTradeID(int tradeId) => this.tradeId = tradeId;

    public bool IsUpdateCompleted(CorrespondentTradeLoanStatus status)
    {
      return this.sessionObjects.CorrespondentTradeManager.IsTradeAssignmentUpdateCompleted(this.tradeId, this.Guid, status);
    }

    private CorrespondentTradeHistoryItem createRejectedCorrespondentTradeHistoryItem(
      CorrespondentTradeInfo trade)
    {
      return new CorrespondentTradeHistoryItem(trade, this.PipelineInfo, CorrespondentTradeLoanStatus.Unassigned, this.sessionObjects.UserInfo, true);
    }
  }
}
