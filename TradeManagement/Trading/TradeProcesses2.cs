// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeProcesses2
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
  public class TradeProcesses2
  {
    private const string className = "TradeProcesses2";
    private static string sw = Tracing.SwOutsideLoan;
    private TradeProcesses2.ActionType updateAction;
    private TradeAssignmentManager assignmentMgr;
    private int[] tradeIds;
    private LoanTradeInfo loanTrade;
    private bool forceUpdateOfAllLoans;
    private Decimal securityPrice;
    private Queue<AssignmentItem> assignmentQueue = new Queue<AssignmentItem>();
    private bool applySkipListToAllLoans;
    private List<string> skipFieldList = new List<string>();
    private Dictionary<LoanTradeInfo, LoanTradeAssignment[]> loanAssignments = new Dictionary<LoanTradeInfo, LoanTradeAssignment[]>();
    private readonly ITradeEditor editor;
    private object logLockObject = new object();
    private static string smartClientMultiThreadingSetting = (string) null;

    public TradeProcesses2()
    {
    }

    public TradeProcesses2(ITradeEditor editor) => this.editor = editor;

    public void Execute(
      TradeProcesses2.ActionType updateActionType,
      LoanTradeInfo loanTrade,
      TradeAssignmentManager assignmentManager,
      bool forceUpdateOfAllLoans,
      Decimal securityPrice)
    {
      this.updateAction = updateActionType;
      this.loanTrade = loanTrade;
      this.assignmentMgr = assignmentManager;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.securityPrice = securityPrice;
      this.PrepareTradeAssignment();
      this.Process();
    }

    public void Execute(
      TradeProcesses2.ActionType updateActionType,
      int[] tradeIdsObj,
      bool forceUpdateOfAllLoans)
    {
      this.updateAction = updateActionType;
      this.tradeIds = tradeIdsObj;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.PrepareTradeAssignment();
      this.Process();
    }

    private void ProcessInKafka()
    {
      string str1 = string.Empty;
      int num = 0;
      string pendingTradesNames = string.Empty;
      int pendingTradesCount = 0;
      foreach (LoanTradeInfo key in this.loanAssignments.Keys)
      {
        LoanTradeAssignment[] loanAssignment = this.loanAssignments[key];
        if (loanAssignment != null)
        {
          if (((IEnumerable<LoanTradeAssignment>) loanAssignment).Count<LoanTradeAssignment>() > 0)
          {
            try
            {
              TradeSync.LoanTradeSync((TradeInfoObj) key, loanAssignment, Session.SessionObjects, this.skipFieldList);
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
        this.loanTrade.Status = TradeStatus.Pending;
        ((TradeEditor) this.editor).MakePending(true);
      }
      string elligibleMesg = "";
      if (num > 0)
      {
        string str2 = str1.Substring(0, str1.Length - 2);
        elligibleMesg = num <= 1 ? "Loan Trade " + str2 + " has been successfully submitted for update and is in pending status" : "Loan Trade " + str2 + " have been successfully submitted for update and is in pending status";
      }
      this.displayActionResults(pendingTradesCount, pendingTradesNames, "Trade", elligibleMesg);
    }

    private void ProcessInQueue()
    {
      if (this.loanTrade != null)
      {
        TradeUpdateLoansDialog.Insert(this.updateAction, this.assignmentQueue.First<AssignmentItem>().LoanTrade, this.forceUpdateOfAllLoans, this.securityPrice, this.assignmentQueue.Select<AssignmentItem, LoanTradeAssignment>((Func<AssignmentItem, LoanTradeAssignment>) (q => q.Assignment)).ToArray<LoanTradeAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
        if (this.editor != null)
        {
          this.loanTrade.Status = TradeStatus.Pending;
          ((TradeEditor) this.editor).MakePending(true);
        }
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "Trade " + this.assignmentQueue.First<AssignmentItem>().LoanTrade.Name + " has been successfully submitted for update and is in pending status", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
          IEnumerable<AssignmentItem> source = this.assignmentQueue.Where<AssignmentItem>((Func<AssignmentItem, bool>) (a => a.LoanTrade.TradeID == tradeId));
          if (source != null)
          {
            if (source.Count<AssignmentItem>() > 0)
            {
              try
              {
                TradeUpdateLoansDialog.Insert(this.updateAction, source.First<AssignmentItem>().LoanTrade, this.forceUpdateOfAllLoans, this.securityPrice, source.Select<AssignmentItem, LoanTradeAssignment>((Func<AssignmentItem, LoanTradeAssignment>) (q => q.Assignment)).ToArray<LoanTradeAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
                str1 = str1 + source.First<AssignmentItem>().LoanTrade.Name + ", ";
                ++num;
              }
              catch (TradeNotUpdateException ex)
              {
                ++pendingTradesCount;
                pendingTradesNames = pendingTradesNames + source.First<AssignmentItem>().LoanTrade.Name + ", ";
              }
            }
          }
        }
        string elligibleMesg = "";
        if (num > 0)
        {
          string str2 = str1.Substring(0, str1.Length - 2);
          elligibleMesg = num <= 1 ? "Trade " + str2 + " has been successfully submitted for update and is in pending status" : "Trade " + str2 + " have been successfully submitted for update and is in pending status";
        }
        this.displayActionResults(pendingTradesCount, pendingTradesNames, "Trade", elligibleMesg);
      }
    }

    private void Process()
    {
      if (this.assignmentQueue != null && this.assignmentQueue.Count == 0)
        return;
      TradeLoanSyncDlg tradeLoanSyncDlg = new TradeLoanSyncDlg(this.assignmentQueue.First<AssignmentItem>().LoanTrade.PriceAdjustments, this.assignmentQueue.First<AssignmentItem>().LoanTrade);
      this.skipFieldList = tradeLoanSyncDlg.SkipFieldList;
      bool boolean = Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]);
      if (Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.LoanTradeApplyImmediately"]))
      {
        if (boolean)
          this.ProcessInKafka();
        else
          this.ProcessInQueue();
      }
      else
      {
        int num1 = (int) tradeLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
        if (tradeLoanSyncDlg.DialogResult == DialogResult.OK)
        {
          this.skipFieldList = tradeLoanSyncDlg.SkipFieldList;
          this.applySkipListToAllLoans = tradeLoanSyncDlg.ApplyToAll;
          if (this.applySkipListToAllLoans)
          {
            if (boolean)
              this.ProcessInKafka();
            else
              this.ProcessInQueue();
          }
          else
          {
            if (this.assignmentMgr != null)
            {
              LoanTradeInfo trade = Session.LoanTradeManager.GetTrade(this.loanTrade.TradeID);
              if (trade.Status == TradeStatus.Pending)
              {
                this.displayActionResults(1, trade.Name + ", ", "Trade");
                return;
              }
            }
            else if (this.tradeIds != null)
            {
              string pendingTradesNames = string.Empty;
              int pendingTradesCount = 0;
              Queue<AssignmentItem> source = new Queue<AssignmentItem>();
              foreach (int tradeId1 in this.tradeIds)
              {
                int tradeId = tradeId1;
                LoanTradeInfo loanTradeInfo = (LoanTradeInfo) null;
                if (this.assignmentQueue.Any<AssignmentItem>((Func<AssignmentItem, bool>) (a => a.LoanTrade.TradeID == tradeId)))
                  loanTradeInfo = this.assignmentQueue.Where<AssignmentItem>((Func<AssignmentItem, bool>) (a => a.LoanTrade.TradeID == tradeId)).First<AssignmentItem>().LoanTrade;
                if (loanTradeInfo.Status == TradeStatus.Pending || loanTradeInfo.Status == TradeStatus.Archived)
                {
                  ++pendingTradesCount;
                  pendingTradesNames = pendingTradesNames + loanTradeInfo.Name + ", ";
                }
                else
                {
                  IEnumerable<AssignmentItem> assignmentItems = this.assignmentQueue.Where<AssignmentItem>((Func<AssignmentItem, bool>) (a => a.LoanTrade.TradeID == tradeId));
                  if (assignmentItems != null)
                  {
                    foreach (AssignmentItem assignmentItem in assignmentItems)
                      source.Enqueue(assignmentItem);
                  }
                }
              }
              if (pendingTradesCount > 0)
              {
                this.displayActionResults(pendingTradesCount, pendingTradesNames, "Trade");
                if (pendingTradesCount == this.assignmentQueue.Count)
                  return;
                this.assignmentQueue.Clear();
                foreach (AssignmentItem assignmentItem in source.ToList<AssignmentItem>())
                  this.assignmentQueue.Enqueue(assignmentItem);
              }
            }
            using (TradeLoanUpdateDialog progressForm = new TradeLoanUpdateDialog(this.assignmentQueue, this.applySkipListToAllLoans, this.skipFieldList, this.securityPrice, this.forceUpdateOfAllLoans))
            {
              progressForm.syncDlg = tradeLoanSyncDlg;
              int num2 = (int) progressForm.ShowDialog();
              this.executeComplete(progressForm);
            }
          }
        }
      }
    }

    private void PrepareTradeAssignment()
    {
      if (this.assignmentMgr != null)
      {
        LoanTradeAssignment[] loanTradeAssignmentArray = !this.forceUpdateOfAllLoans ? this.assignmentMgr.GetPendingLoans() : this.assignmentMgr.GetPendingAndAssignedLoans();
        foreach (LoanTradeAssignment assignment in loanTradeAssignmentArray)
          this.assignmentQueue.Enqueue(new AssignmentItem(this.loanTrade, assignment, this.securityPrice));
        this.loanAssignments.Add(this.loanTrade, loanTradeAssignmentArray);
      }
      else
      {
        if (this.tradeIds == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int tradeId in this.tradeIds)
        {
          LoanTradeInfo trade = Session.LoanTradeManager.GetTrade(tradeId);
          if (trade != null)
          {
            SecurityTradeInfo securityTradeInfo = (SecurityTradeInfo) null;
            if (trade.SecurityTradeID >= 0)
              securityTradeInfo = Session.SecurityTradeManager.GetTrade(trade.SecurityTradeID);
            Decimal price = securityTradeInfo == null ? 0M : securityTradeInfo.Price;
            TradeAssignmentManager source = new TradeAssignmentManager(Session.SessionObjects, trade.TradeID, false);
            foreach (LoanTradeAssignment assignment in source)
              this.assignmentQueue.Enqueue(new AssignmentItem(trade, assignment, price));
            this.loanAssignments.Add(trade, source.ToArray<LoanTradeAssignment>());
          }
        }
      }
    }

    private void executeComplete(TradeLoanUpdateDialog progressForm)
    {
      string detailMessage = this.buildCompletionMessage(progressForm.Errors, progressForm.LoanCount, progressForm.TotalCompletedCount, progressForm.CancelRequested);
      int num = (int) new TradeLoanUpdateCompleteDialog(progressForm.CompletedSuccessfullyCount, progressForm.CompletedWithErrorsCount, detailMessage).ShowDialog();
      if (progressForm.LoanCount == progressForm.TotalCompletedCount || progressForm.CancelRequested)
        return;
      this.ProcessInQueue();
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
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice)
    {
      try
      {
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, Loan: " + assignment.PipelineInfo.LoanNumber);
        if (Session.LoanData != null && assignment.Guid == Session.LoanData.GUID)
        {
          if (assignment.PendingStatus == LoanTradeStatus.Unassigned)
          {
            this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling CommitPendingLoanTradeStatus");
            Session.LoanTradeManager.CommitPendingTradeStatus(trade.TradeID, assignment.Guid, assignment.PendingStatus, assignment.Rejected);
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
        Tracing.Log(TradeProcesses2.sw, nameof (TradeProcesses2), TraceLevel.Error, "Error in WorkOneLoan for " + assignment.Guid + ": " + ex.ToString());
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, "An error occurred while updating."));
      }
    }

    private bool commitTradeAssignment(
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment.");
      if (assignment.PendingStatus == LoanTradeStatus.None && !forceRefresh)
        return false;
      Dictionary<string, string> updateFieldList = new Dictionary<string, string>();
      if (trade.IsBulkDelivery)
        updateFieldList.Add("TotalPrice", assignment.TotalPrice.ToString());
      if (assignment.PendingStatus == LoanTradeStatus.None)
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
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      List<string> skipFields,
      Dictionary<string, string> updateFieldList = null)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus.");
      string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      if (assignment.PendingStatus == LoanTradeStatus.None)
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
          assignment.ApplyPendingStatusToLoan(loanMgr, trade, skipFields, updateFieldList);
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling closeLoan A.");
          this.closeLoan(loanMgr, true);
        }
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling assignment.CommitPendingStatusToLoan.");
        assignment.CommitPendingStatusToLoan(trade);
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
      LoanTradeInfo trade,
      List<string> skipFields,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList = null)
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
        Tracing.Log(TradeProcesses2.sw, nameof (TradeProcesses2), traceLevel, msg);
    }

    public static bool IsTradeProcesses2Enabled()
    {
      if (TradeProcesses2.smartClientMultiThreadingSetting == null)
      {
        try
        {
          TradeProcesses2.smartClientMultiThreadingSetting = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "TradeAssignmentUseMultithreading");
        }
        catch (Exception ex)
        {
        }
      }
      if (TradeProcesses2.smartClientMultiThreadingSetting == "0")
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
      string prefixMesg = "The trade",
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
