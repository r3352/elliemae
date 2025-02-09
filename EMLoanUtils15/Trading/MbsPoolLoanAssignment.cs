// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolLoanAssignment
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
  public class MbsPoolLoanAssignment : LoanToTradeAssignmentBase
  {
    private MbsPoolLoanStatus assignedStatus;
    private MbsPoolLoanStatus initialPendingStatus;
    private MbsPoolLoanStatus pendingStatus;

    public MbsPoolLoanAssignment(SessionObjects sessionObjects, int tradeId, PipelineInfo pinfo)
      : base(sessionObjects, tradeId, pinfo)
    {
      this.assignedStatus = MbsPoolLoanStatus.Unassigned;
      this.pendingStatus = MbsPoolLoanStatus.None;
      this.rejected = false;
      if (tradeId > 0)
      {
        foreach (PipelineInfo.TradeInfo tradeAssignment in pinfo.TradeAssignments)
        {
          if (tradeAssignment.TradeID == tradeId || tradeAssignment.GseCommitmentId == tradeId)
          {
            this.assignedStatus = (MbsPoolLoanStatus) tradeAssignment.AssignedStatus;
            this.pendingStatus = (MbsPoolLoanStatus) tradeAssignment.PendingStatus;
            this.CommitmentContractNumber = tradeAssignment.CommitmentContractNumber;
            this.ProductName = tradeAssignment.ProductName;
            if (tradeAssignment.GseCommitmentId == tradeId)
            {
              this.tradeId = tradeAssignment.TradeID;
              break;
            }
            break;
          }
        }
      }
      this.initialPendingStatus = this.pendingStatus;
    }

    public MbsPoolLoanStatus AssignedStatus => this.assignedStatus;

    public MbsPoolLoanStatus PendingStatus
    {
      get => this.pendingStatus;
      set
      {
        this.pendingStatus = value != this.assignedStatus || this.initialPendingStatus != MbsPoolLoanStatus.None ? value : MbsPoolLoanStatus.None;
        if (this.Status == MbsPoolLoanStatus.Unassigned)
          return;
        this.rejected = false;
      }
    }

    public MbsPoolLoanStatus Status
    {
      get
      {
        return this.pendingStatus != MbsPoolLoanStatus.None ? this.pendingStatus : this.assignedStatus;
      }
    }

    public bool Assigned => this.Status != MbsPoolLoanStatus.Unassigned;

    public bool Removed => this.Pending && this.Status == MbsPoolLoanStatus.Unassigned;

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
        this.pendingStatus = MbsPoolLoanStatus.Unassigned;
      }
    }

    public string ProductName { get; set; }

    public string CommitmentContractNumber { get; set; }

    public int CommitmentTradeId { get; set; }

    public Decimal GuarantyFee { get; set; }

    public Decimal CPA { get; set; }

    public void ApplyPendingStatusToLoan(
      LoanDataMgr loanMgr,
      MbsPoolInfo trade,
      List<string> skipFieldList,
      Dictionary<string, string> updateFieldList)
    {
      if (this.Guid != loanMgr.LoanData.GUID)
        throw new Exception("The specified LoanDataMgr does not have the correct GUID");
      if (this.PendingStatus == MbsPoolLoanStatus.None || this.pendingStatus == this.assignedStatus)
        return;
      if (this.PendingStatus == MbsPoolLoanStatus.Unassigned)
      {
        if (!this.rejected)
          this.rejected = this.sessionObjects.MbsPoolManager.GetTradeForRejectedLoan(loanMgr.LoanData.GUID) != null;
        loanMgr.RemoveFromTrade((TradeInfoObj) trade, this.rejected, skipFieldList);
      }
      else if (this.PendingStatus == MbsPoolLoanStatus.Assigned)
      {
        loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, trade.WeightedAvgPrice, updateFieldList);
      }
      else
      {
        if (this.AssignedStatus == MbsPoolLoanStatus.Unassigned)
          loanMgr.AssignToTrade((TradeInfoObj) trade, skipFieldList, trade.WeightedAvgPrice, updateFieldList);
        loanMgr.ModifyTradeStatus((TradeInfoObj) trade, (object) this.PendingStatus, skipFieldList, trade.WeightedAvgPrice, updateFieldList);
      }
    }

    public void CommitPendingStatusToLoan(MbsPoolInfo trade, bool checkCompletion = true)
    {
      if (this.PendingStatus == MbsPoolLoanStatus.None)
        return;
      if (checkCompletion && !this.IsUpdateCompleted(this.pendingStatus))
        throw new Exception("Unexpected error during loan update");
      this.sessionObjects.MbsPoolManager.CommitPendingTradeStatus(this.tradeId, this.Guid, this.pendingStatus, this.rejected);
      if (this.pendingStatus != this.assignedStatus && this.rejected && trade.InvestorName != "")
        this.sessionObjects.MbsPoolManager.AddTradeHistoryItem(this.createRejectedLoanTradeHistoryItem(trade));
      if (this.sessionObjects.MbsPoolManager.GetTradeForRejectedLoan(this.Guid) != null)
        return;
      this.CommitPendingStatus();
    }

    internal void ClearModifications() => this.initialPendingStatus = this.pendingStatus;

    internal void CommitPendingStatus()
    {
      this.assignedStatus = this.Status;
      this.pendingStatus = MbsPoolLoanStatus.None;
      this.initialPendingStatus = MbsPoolLoanStatus.None;
      this.rejected = false;
    }

    internal void ApplyNewTradeID(int tradeId) => this.tradeId = tradeId;

    private MbsPoolHistoryItem createRejectedLoanTradeHistoryItem(MbsPoolInfo trade)
    {
      return new MbsPoolHistoryItem(trade, this.PipelineInfo, trade.InvestorName, this.sessionObjects.UserInfo);
    }

    public bool IsUpdateCompleted(MbsPoolLoanStatus status)
    {
      return this.sessionObjects.MbsPoolManager.IsTradeAssignmentUpdateCompleted(this.tradeId, this.Guid, status);
    }
  }
}
