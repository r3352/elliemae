// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.MbsPoolProcesses
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class MbsPoolProcesses
  {
    private const string className = "MbsPoolProcesses";
    private static string sw = Tracing.SwOutsideLoan;
    private MbsPoolLoanAssignmentManager assignmentMgr;
    private int[] tradeIds;
    private MbsPoolInfo mbsPool;
    private bool forceUpdateOfAllLoans;
    private Decimal securityPrice;
    private Dictionary<int, MbsPoolLoanAssignment[]> _assignmentsByTrade;
    private List<string> skipFieldList = new List<string>();
    private MbsPoolProcesses.ActionType updateAction;
    private Queue<MbsPoolLoanAssignmentItem> assignmentQueue = new Queue<MbsPoolLoanAssignmentItem>();
    private Dictionary<MbsPoolInfo, MbsPoolLoanAssignment[]> corrAssignments = new Dictionary<MbsPoolInfo, MbsPoolLoanAssignment[]>();
    private readonly ITradeEditor editor;
    private object logLockObject = new object();
    private static string smartClientMultiThreadingSetting = (string) null;

    public MbsPoolProcesses()
    {
    }

    public MbsPoolProcesses(ITradeEditor editor) => this.editor = editor;

    public void Execute(
      MbsPoolProcesses.ActionType updateActionType,
      MbsPoolInfo mbsPool,
      MbsPoolLoanAssignmentManager assignmentManager,
      bool forceUpdateOfAllLoans,
      Decimal securityPrice)
    {
      this.updateAction = updateActionType;
      this.mbsPool = mbsPool;
      this.assignmentMgr = assignmentManager;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.securityPrice = securityPrice;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    public void Execute(
      MbsPoolProcesses.ActionType updateActionType,
      int[] tradeIdsObj,
      bool forceUpdateOfAllLoans)
    {
      this.updateAction = updateActionType;
      this.tradeIds = tradeIdsObj;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    public void Execute(
      MbsPoolProcesses.ActionType updateActionType,
      bool forceUpdateOfAllLoans,
      Dictionary<int, MbsPoolLoanAssignment[]> assignmentsByPool)
    {
      this.updateAction = updateActionType;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this._assignmentsByTrade = assignmentsByPool;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    private void PrepareTradeAssignmentKafkaQueue()
    {
      if (this.assignmentMgr != null)
        this.corrAssignments.Add(this.mbsPool, !this.forceUpdateOfAllLoans ? this.assignmentMgr.GetPendingLoans() : this.assignmentMgr.GetPendingAndAssignedLoans());
      else if (this.tradeIds != null)
      {
        foreach (int tradeId in this.tradeIds)
        {
          MbsPoolInfo trade = Session.MbsPoolManager.GetTrade(tradeId);
          if (trade != null)
          {
            MbsPoolLoanAssignmentManager source = new MbsPoolLoanAssignmentManager(Session.SessionObjects, trade.TradeID, false);
            this.corrAssignments.Add(trade, source.ToArray<MbsPoolLoanAssignment>());
          }
        }
      }
      else
      {
        if (this._assignmentsByTrade == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int key in this._assignmentsByTrade.Keys)
        {
          MbsPoolInfo trade = Session.MbsPoolManager.GetTrade(key);
          if (trade != null && this._assignmentsByTrade.ContainsKey(key))
          {
            MbsPoolLoanAssignment[] poolLoanAssignmentArray = this._assignmentsByTrade[key];
            this.corrAssignments.Add(trade, poolLoanAssignmentArray);
          }
        }
      }
      this.ProcessKafka();
    }

    private void ProcessKafka()
    {
      if (this.corrAssignments != null && this.corrAssignments.Count == 0)
        return;
      this.mbsPool = this.corrAssignments.Keys.First<MbsPoolInfo>();
      MbsPoolLoanSyncDlg mbsPoolLoanSyncDlg = new MbsPoolLoanSyncDlg(this.mbsPool.PoolMortgageType);
      this.skipFieldList = mbsPoolLoanSyncDlg.SkipFieldList;
      if (Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.MBSApplyImmediately"]))
      {
        this.ProcessInKafka();
      }
      else
      {
        int num = (int) mbsPoolLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
        if (mbsPoolLoanSyncDlg.DialogResult == DialogResult.OK)
        {
          this.skipFieldList = mbsPoolLoanSyncDlg.SkipFieldList;
          this.ProcessInKafka();
        }
      }
    }

    private void ProcessInKafka()
    {
      string str1 = string.Empty;
      int num = 0;
      string pendingTradesNames = string.Empty;
      int pendingTradesCount = 0;
      foreach (MbsPoolInfo key in this.corrAssignments.Keys)
      {
        MbsPoolLoanAssignment[] corrAssignment = this.corrAssignments[key];
        if (corrAssignment != null)
        {
          if (((IEnumerable<MbsPoolLoanAssignment>) corrAssignment).Count<MbsPoolLoanAssignment>() > 0)
          {
            try
            {
              TradeSync.MbsPoolTradeSync((TradeInfoObj) key, corrAssignment, Session.SessionObjects, this.skipFieldList);
              str1 = str1 + key.Name + ", ";
              ++num;
            }
            catch (TradeNotUpdateException ex)
            {
              ++pendingTradesCount;
              pendingTradesNames = pendingTradesNames + key.Name + ", ";
            }
          }
        }
      }
      if (this.editor != null)
      {
        this.mbsPool.Status = TradeStatus.Pending;
        if (this.editor is MbsPoolEditor)
          ((MbsPoolEditor) this.editor).MakePending(true);
        else
          ((FannieMaePEPoolEditor) this.editor).MakePending(true);
      }
      string elligibleMesg = "";
      if (num > 0)
      {
        string str2 = str1.Substring(0, str1.Length - 2);
        elligibleMesg = num <= 1 ? "MBS Pool " + str2 + " has been successfully submitted for update and is in pending status" : "MBS Pool " + str2 + " have been successfully submitted for update and is in pending status";
      }
      this.displayActionResults(pendingTradesCount, pendingTradesNames, "MBS Pool", elligibleMesg);
    }

    private void ProcessInQueue()
    {
      if (this.mbsPool != null)
      {
        TradeUpdateLoansDialog.Insert(this.updateAction, this.assignmentQueue.First<MbsPoolLoanAssignmentItem>().MbsPool, this.forceUpdateOfAllLoans, this.securityPrice, this.assignmentQueue.Select<MbsPoolLoanAssignmentItem, MbsPoolLoanAssignment>((Func<MbsPoolLoanAssignmentItem, MbsPoolLoanAssignment>) (q => q.Assignment)).ToArray<MbsPoolLoanAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
        if (this.editor != null)
        {
          this.mbsPool.Status = TradeStatus.Pending;
          if (this.editor is MbsPoolEditor)
            ((MbsPoolEditor) this.editor).MakePending(true);
          else
            ((FannieMaePEPoolEditor) this.editor).MakePending(true);
        }
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "MBS Pool " + this.mbsPool.Name + " has been successfully submitted for update and is in pending status", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
      }
      else
      {
        if (this.tradeIds == null)
          return;
        string str1 = string.Empty;
        int num = 0;
        string pendingTradesNames = string.Empty;
        int pendingTradesCount = 0;
        foreach (int tradeId1 in this.tradeIds)
        {
          int tradeId = tradeId1;
          IEnumerable<MbsPoolLoanAssignmentItem> source = this.assignmentQueue.Where<MbsPoolLoanAssignmentItem>((Func<MbsPoolLoanAssignmentItem, bool>) (a => a.MbsPool.TradeID == tradeId));
          if (source != null)
          {
            if (source.Count<MbsPoolLoanAssignmentItem>() > 0)
            {
              try
              {
                TradeUpdateLoansDialog.Insert(this.updateAction, source.First<MbsPoolLoanAssignmentItem>().MbsPool, this.forceUpdateOfAllLoans, this.securityPrice, source.Select<MbsPoolLoanAssignmentItem, MbsPoolLoanAssignment>((Func<MbsPoolLoanAssignmentItem, MbsPoolLoanAssignment>) (q => q.Assignment)).ToArray<MbsPoolLoanAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
                str1 = str1 + source.First<MbsPoolLoanAssignmentItem>().MbsPool.Name + ", ";
                ++num;
              }
              catch (TradeNotUpdateException ex)
              {
                ++pendingTradesCount;
                pendingTradesNames = pendingTradesNames + source.First<MbsPoolLoanAssignmentItem>().MbsPool.Name + ", ";
              }
            }
          }
        }
        string elligibleMesg = "";
        if (num > 0)
        {
          string str2 = str1.Substring(0, str1.Length - 2);
          elligibleMesg = num <= 1 ? "MBS Pool " + str2 + " has been successfully submitted for update and is in pending status" : "MBS Pool " + str2 + " have been successfully submitted for update and is in pending status";
        }
        this.displayActionResults(pendingTradesCount, pendingTradesNames, "MBS Pool", elligibleMesg);
      }
    }

    private void Process()
    {
      if (this.assignmentQueue != null && this.assignmentQueue.Count == 0)
        return;
      MbsPoolLoanSyncDlg mbsPoolLoanSyncDlg = new MbsPoolLoanSyncDlg(this.assignmentQueue.First<MbsPoolLoanAssignmentItem>().MbsPool.PoolMortgageType);
      this.skipFieldList = mbsPoolLoanSyncDlg.SkipFieldList;
      if (Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.MBSApplyImmediately"]))
      {
        this.ProcessInQueue();
      }
      else
      {
        int num1 = (int) mbsPoolLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
        if (mbsPoolLoanSyncDlg.DialogResult == DialogResult.OK)
        {
          this.skipFieldList = mbsPoolLoanSyncDlg.SkipFieldList;
          if (mbsPoolLoanSyncDlg.ApplyToAll)
          {
            this.ProcessInQueue();
          }
          else
          {
            using (MbsPoolLoanUpdateDialog progressForm = new MbsPoolLoanUpdateDialog(this.assignmentQueue, false, this.skipFieldList, this.securityPrice, this.forceUpdateOfAllLoans))
            {
              progressForm.syncDlg = mbsPoolLoanSyncDlg;
              int num2 = (int) progressForm.ShowDialog();
              this.executeComplete(progressForm);
            }
          }
        }
      }
    }

    private void PrepareTradeAssignmentQueue()
    {
      if (this.assignmentMgr != null)
      {
        foreach (MbsPoolLoanAssignment assignment in !this.forceUpdateOfAllLoans ? this.assignmentMgr.GetPendingLoans() : this.assignmentMgr.GetPendingAndAssignedLoans())
          this.assignmentQueue.Enqueue(new MbsPoolLoanAssignmentItem(this.mbsPool, assignment, this.securityPrice));
      }
      else if (this.tradeIds != null)
      {
        foreach (int tradeId in this.tradeIds)
        {
          MbsPoolInfo trade = Session.MbsPoolManager.GetTrade(tradeId);
          if (trade != null)
          {
            foreach (MbsPoolLoanAssignment assignment in new MbsPoolLoanAssignmentManager(Session.SessionObjects, trade.TradeID, false))
              this.assignmentQueue.Enqueue(new MbsPoolLoanAssignmentItem(trade, assignment, trade.WeightedAvgPrice));
          }
        }
      }
      else
      {
        if (this._assignmentsByTrade == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int key in this._assignmentsByTrade.Keys)
        {
          MbsPoolInfo trade = Session.MbsPoolManager.GetTrade(key);
          if (trade != null && this._assignmentsByTrade.ContainsKey(key))
          {
            foreach (MbsPoolLoanAssignment assignment in this._assignmentsByTrade[key])
              this.assignmentQueue.Enqueue(new MbsPoolLoanAssignmentItem(trade, assignment, trade.WeightedAvgPrice));
          }
        }
      }
    }

    private void executeComplete(MbsPoolLoanUpdateDialog progressForm)
    {
      string detailMessage = this.buildCompletionMessage(progressForm.Errors, progressForm.LoanCount, progressForm.TotalCompletedCount, progressForm.CancelRequested);
      int num = (int) new TradeLoanUpdateCompleteDialog(progressForm.CompletedSuccessfullyCount, progressForm.CompletedWithErrorsCount, detailMessage).ShowDialog();
    }

    private string buildCompletionMessage(
      List<TradeLoanUpdateError> errors,
      int loanCount,
      int completedCount,
      bool cancelRequested)
    {
      string str = "";
      if (errors.Count > 0)
      {
        foreach (TradeLoanUpdateError error in errors)
          str = str + "Loan " + (error.LoanInfo == null ? error.LoanGuid : error.LoanInfo.LoanNumber) + ": " + error.Message + Environment.NewLine + Environment.NewLine;
      }
      else
        str = !cancelRequested ? "All loans were updated successfully." : "Update process was cancelled.";
      return str;
    }

    public bool WorkOneLoan(
      MbsPoolLoanAssignment assignment,
      MbsPoolInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice)
    {
      try
      {
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, Loan: " + assignment.PipelineInfo.LoanNumber);
        if (Session.LoanData != null && assignment.Guid == Session.LoanData.GUID)
        {
          if (assignment.PendingStatus == MbsPoolLoanStatus.Unassigned)
          {
            this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling CommitPendingLoanTradeStatus");
            Session.MbsPoolManager.CommitPendingTradeStatus(trade.TradeID, assignment.Guid, assignment.PendingStatus, assignment.Rejected);
          }
          throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, "Loan file is currently opened.  Please close this loan file first before it can be updated."));
        }
        if (!assignment.Rejected)
          assignment.Rejected = Session.LoanTradeManager.GetTradeForRejectedLoan(assignment.Guid) != null;
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling CommitTradeAssignment");
        int num = this.commitTradeAssignment(assignment, trade, forceRefresh, skipFields, securityPrice) ? 1 : 0;
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, returned from CommitTradeAssignment.");
        return num != 0;
      }
      catch (TradeLoanUpdateException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        Tracing.Log(MbsPoolProcesses.sw, nameof (MbsPoolProcesses), TraceLevel.Error, "Error in WorkOneLoan for " + assignment.Guid + ": " + ex.ToString());
        throw ex;
      }
    }

    private bool commitTradeAssignment(
      MbsPoolLoanAssignment assignment,
      MbsPoolInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment.");
      if (assignment.PendingStatus == MbsPoolLoanStatus.None && !forceRefresh)
        return false;
      Dictionary<string, string> updateFieldList = new Dictionary<string, string>();
      if (trade.PoolMortgageType == MbsPoolMortgageType.FannieMaePE)
      {
        if (!string.IsNullOrEmpty(assignment.CommitmentContractNumber))
          updateFieldList.Add("4093", assignment.CommitmentContractNumber);
        if (!string.IsNullOrEmpty(assignment.ProductName))
          updateFieldList.Add("4094", assignment.ProductName);
        if (assignment.GuarantyFee != 0M)
          updateFieldList.Add("3889", assignment.GuarantyFee.ToString());
        if (assignment.CPA != 0M)
          updateFieldList.Add("CPA", assignment.CPA.ToString());
      }
      if (assignment.PendingStatus == MbsPoolLoanStatus.None)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling RefreshTradeDataInLoan");
        this.refreshTradeDataInLoan(assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList);
      }
      else if (assignment.PendingStatus != assignment.AssignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling CommitPendingTradeStatus.");
        this.commitPendingTradeStatus(assignment, trade, skipFields, updateFieldList);
      }
      else
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, assignment.CommitPendingStatusToLoan");
        assignment.CommitPendingStatusToLoan(trade, false);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, RefreshTradeDataInLoan");
        this.refreshTradeDataInLoan(assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList);
      }
      return true;
    }

    private void commitPendingTradeStatus(
      MbsPoolLoanAssignment assignment,
      MbsPoolInfo mbsPool,
      List<string> skipFields,
      Dictionary<string, string> updateFieldList)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus.");
      string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      if (assignment.PendingStatus == MbsPoolLoanStatus.None)
        return;
      int pendingStatus = (int) assignment.PendingStatus;
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      int assignedStatus = (int) assignment.AssignedStatus;
      if (pendingStatus != assignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling openLoan A.");
        this.openLoan(assignment.PipelineInfo, out loanMgr);
      }
      try
      {
        if (loanMgr != null)
        {
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling assignment.ApplyPendingStatusToLoan.");
          assignment.ApplyPendingStatusToLoan(loanMgr, mbsPool, skipFields, updateFieldList);
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling closeLoan A.");
          this.closeLoan(loanMgr, true);
        }
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling assignment.CommitPendingStatusToLoan.");
        assignment.CommitPendingStatusToLoan(mbsPool);
      }
      catch (Exception ex)
      {
        this.log(TraceLevel.Error, "Error applying trade status information to loan " + assignment.Guid + ": " + (object) ex);
        if (loanMgr != null)
        {
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling closeLoan B.");
          this.closeLoan(loanMgr, false);
        }
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, "The loan could not be updated due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time."));
      }
    }

    private void refreshTradeDataInLoan(
      PipelineInfo pinfo,
      MbsPoolInfo trade,
      List<string> skipFields,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan.");
      string.Concat(pinfo.GetField("LoanNumber"));
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling openLoan.");
      this.openLoan(pinfo, out loanMgr);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling loanMgr.RefreshTradeData.");
      loanMgr.RefreshTradeData((TradeInfoObj) trade, skipFields, securityPrice, updateFieldList);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling closeLoan.");
      this.closeLoan(loanMgr, true);
    }

    private void closeLoan(LoanDataMgr loanMgr, bool save)
    {
      try
      {
        if (loanMgr.LoanData == null)
          return;
        if (save)
        {
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In closeLoan, calling loanMgr.Save.");
          loanMgr.Save(false);
        }
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In closeLoan, calling loanMgr.Close A");
        loanMgr.Close();
      }
      catch (Exception ex)
      {
        this.log(TraceLevel.Error, "Error saving loan " + loanMgr.LoanData.GUID + ": " + (object) ex);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In closeLoan, calling loanMgr.Close B.");
        string loanGuid = "";
        if (loanMgr != null && loanMgr.LoanData != null && loanMgr.LoanData.GUID != null)
          loanGuid = loanMgr.LoanData.GUID;
        PipelineInfo loanInfo = (PipelineInfo) null;
        if (loanMgr != null)
          loanInfo = loanMgr.ToPipelineInfo();
        loanMgr.Close();
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(loanGuid, loanInfo, "The loan could not be saved due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time."));
      }
    }

    private void openLoan(PipelineInfo pinfo, out LoanDataMgr loanMgr)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan.");
      loanMgr = (LoanDataMgr) null;
      string str1 = string.Concat(pinfo.GetField("LoanNumber"));
      try
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling LoanDataMgr.OpenLoan.");
        loanMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, pinfo.GUID, false);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, returned from LoanDataMgr.OpenLoan.");
      }
      catch (Exception ex)
      {
        this.log(TraceLevel.Error, "Error opening loan " + pinfo.GUID + ": " + (object) ex);
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, "The loan '" + str1 + "' could not be opened due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time."));
      }
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, top of while loop.");
      LockInfo[] lockInfoArray = (LockInfo[]) null;
      if (Session.SessionObjects.AllowConcurrentEditing)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling loanMgr.GetCurrentLocks.");
        lockInfoArray = loanMgr.GetCurrentLocks();
      }
      try
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling loanMgr.Lock");
        loanMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
        if (lockInfoArray != null && lockInfoArray.Length != 0)
          throw new LockException(lockInfoArray[0]);
      }
      catch (LockException ex)
      {
        if (lockInfoArray == null)
          throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, "The loan is currently locked."));
        string message = "The loan is currently locked by: ";
        foreach (LockInfo lockInfo in lockInfoArray)
        {
          UserInfo user = loanMgr.SessionObjects.OrganizationManager.GetUser(lockInfo.LockedBy);
          if (user != (UserInfo) null)
          {
            string str2 = "";
            if (user.FullName != "")
              str2 += user.FullName;
            if (user.Email != "")
              str2 = str2 + (str2 == "" ? "" : ", ") + user.Email;
            if (user.Phone != "")
              str2 = str2 + (str2 == "" ? "" : ", ") + user.Phone;
            message = !(str2 == "") ? message + Environment.NewLine + str2 : message + Environment.NewLine + "(Unidentified user)";
          }
        }
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, message));
      }
      catch (SecurityException ex)
      {
        this.log(TraceLevel.Error, "Error locking loan " + pinfo.GUID + ": " + (object) ex);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling loanMgr.Close A");
        loanMgr.Close();
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, "You don't have access to loan '" + str1 + ".'  Contact your System Administrator."));
      }
      catch (Exception ex)
      {
        this.log(TraceLevel.Error, "Error locking loan " + pinfo.GUID + ": " + (object) ex);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling loanMgr.Close B");
        loanMgr.Close();
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, "The loan '" + str1 + "' could not be locked due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time."));
      }
    }

    private void log(TraceLevel traceLevel, string msg)
    {
      lock (this.logLockObject)
        Tracing.Log(MbsPoolProcesses.sw, nameof (MbsPoolProcesses), traceLevel, msg);
    }

    public static bool IsTradeProcesses2Enabled()
    {
      if (MbsPoolProcesses.smartClientMultiThreadingSetting == null)
      {
        try
        {
          MbsPoolProcesses.smartClientMultiThreadingSetting = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "TradeAssignmentUseMultithreading");
        }
        catch (Exception ex)
        {
        }
      }
      if (MbsPoolProcesses.smartClientMultiThreadingSetting == "0")
        return false;
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Ellie Mae\\Encompass");
        if (registryKey != null)
        {
          string str = (string) registryKey.GetValue("TradeAssignmentUseMultithreading");
          if (!string.IsNullOrEmpty(str))
          {
            if (str.Trim() == "0")
              return false;
          }
        }
      }
      catch
      {
      }
      return true;
    }

    private void displayActionResults(
      int pendingTradesCount,
      string pendingTradesNames,
      string prefixMesg = "The pool",
      string elligibleMesg = "")
    {
      string str = "";
      if (pendingTradesCount == 1)
        str = prefixMesg + " " + pendingTradesNames.Substring(0, pendingTradesNames.Length - 2) + " is currently pending in the Trade Update Queue and cannot be modified until completed.\n";
      else if (pendingTradesCount > 1)
        str = prefixMesg + "s " + pendingTradesNames.Substring(0, pendingTradesNames.Length - 2) + " are currently pending in the Trade Update Queue and cannot be modified until completed.\n";
      string text = string.IsNullOrEmpty(elligibleMesg) ? str : elligibleMesg + "\n\n" + str;
      if (string.IsNullOrEmpty(text))
        return;
      int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public enum ActionType
    {
      Commit,
      Refresh,
    }
  }
}
