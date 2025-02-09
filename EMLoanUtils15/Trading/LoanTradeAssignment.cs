// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.LoanTradeAssignment
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
  public class LoanTradeAssignment : LoanToTradeAssignmentBase
  {
    private LoanTradeStatus assignedStatus;
    private LoanTradeStatus initialPendingStatus;
    private LoanTradeStatus pendingStatus;

    public LoanTradeAssignment(SessionObjects sessionObjects, int tradeId, PipelineInfo pinfo)
      : base(sessionObjects, tradeId, pinfo)
    {
      this.assignedStatus = LoanTradeStatus.Unassigned;
      this.pendingStatus = LoanTradeStatus.None;
      this.rejected = false;
      if (tradeId > 0)
      {
        foreach (PipelineInfo.TradeInfo tradeAssignment in pinfo.TradeAssignments)
        {
          if (tradeAssignment.TradeID == tradeId)
          {
            this.assignedStatus = (LoanTradeStatus) tradeAssignment.AssignedStatus;
            this.pendingStatus = (LoanTradeStatus) tradeAssignment.PendingStatus;
            this.TotalPrice = tradeAssignment.TotalPrice;
            break;
          }
        }
      }
      this.initialPendingStatus = this.pendingStatus;
    }

    public LoanTradeStatus AssignedStatus => this.assignedStatus;

    public LoanTradeStatus PendingStatus
    {
      get => this.pendingStatus;
      set
      {
        this.pendingStatus = value != this.assignedStatus || this.initialPendingStatus != LoanTradeStatus.None ? value : LoanTradeStatus.None;
        if (this.Status == LoanTradeStatus.Unassigned)
          return;
        this.rejected = false;
      }
    }

    public LoanTradeStatus Status
    {
      get => this.pendingStatus != LoanTradeStatus.None ? this.pendingStatus : this.assignedStatus;
    }

    public bool Assigned => this.Status != LoanTradeStatus.Unassigned;

    public bool Removed => this.Pending && this.Status == LoanTradeStatus.Unassigned;

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
        this.pendingStatus = LoanTradeStatus.Unassigned;
      }
    }

    public void ApplyPendingStatusToLoan(
      LoanDataMgr loanMgr,
      LoanTradeInfo trade,
      List<string> skipFieldList,
      Dictionary<string, string> updateFiedList = null)
    {
      if (this.Guid != loanMgr.LoanData.GUID)
        throw new Exception("The specified LoanDataMgr does not have the correct GUID");
      if (this.PendingStatus == LoanTradeStatus.None || this.pendingStatus == this.assignedStatus)
        return;
      SecurityTradeInfo securityTradeInfo = (SecurityTradeInfo) null;
      if (trade.SecurityTradeID >= 0)
        securityTradeInfo = this.sessionObjects.SecurityTradeManager.GetTrade(trade.SecurityTradeID);
      if (this.PendingStatus == LoanTradeStatus.Unassigned)
      {
        if (!this.rejected)
          this.rejected = this.sessionObjects.LoanTradeManager.GetTradeForRejectedLoan(loanMgr.LoanData.GUID) != null;
        loanMgr.RemoveFromTrade((TradeInfoObj) trade, this.rejected, skipFieldList);
      }
      else if (this.PendingStatus == LoanTradeStatus.Assigned)
      {
        loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, securityTradeInfo == null ? 0M : securityTradeInfo.Price, updateFiedList);
      }
      else
      {
        if (this.AssignedStatus == LoanTradeStatus.Unassigned)
          loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, securityTradeInfo == null ? 0M : securityTradeInfo.Price, updateFiedList);
        loanMgr.ModifyTradeStatus((TradeInfoObj) trade, (object) this.PendingStatus, skipFieldList, securityTradeInfo == null ? 0M : securityTradeInfo.Price, updateFiedList);
      }
    }

    public void CommitPendingStatusToLoan(LoanTradeInfo trade, bool checkCompletion = true)
    {
      if (this.PendingStatus == LoanTradeStatus.None)
        return;
      if (checkCompletion && !this.IsUpdateCompleted(this.pendingStatus))
        throw new Exception("Unexpected error during loan update");
      this.sessionObjects.LoanTradeManager.CommitPendingTradeStatus(this.tradeId, this.Guid, this.pendingStatus, this.rejected);
      if (this.pendingStatus != this.assignedStatus && this.rejected && trade.InvestorName != "")
        this.sessionObjects.LoanTradeManager.AddTradeHistoryItem(this.createRejectedLoanTradeHistoryItem(trade));
      if (this.sessionObjects.LoanTradeManager.GetTradeForRejectedLoan(this.Guid) != null)
        return;
      this.CommitPendingStatus();
    }

    internal void ClearModifications() => this.initialPendingStatus = this.pendingStatus;

    internal void CommitPendingStatus()
    {
      this.assignedStatus = this.Status;
      this.pendingStatus = LoanTradeStatus.None;
      this.initialPendingStatus = LoanTradeStatus.None;
      this.rejected = false;
    }

    internal void ApplyNewTradeID(int tradeId) => this.tradeId = tradeId;

    private LoanTradeHistoryItem createRejectedLoanTradeHistoryItem(LoanTradeInfo trade)
    {
      return new LoanTradeHistoryItem(trade, this.PipelineInfo, trade.InvestorName, this.sessionObjects.UserInfo);
    }

    public Decimal TotalPrice { get; set; }

    public string EPPSLoanProgramName { get; set; }

    public bool IsUpdateCompleted(LoanTradeStatus status)
    {
      return this.sessionObjects.LoanTradeManager.IsTradeAssignmentUpdateCompleted(this.tradeId, this.Guid, status);
    }
  }
}
