// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.TradeProcesses
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.UI;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.Trading
{
  public class TradeProcesses
  {
    private const string className = "TradeProcesses";
    private static string sw = Tracing.SwOutsideLoan;
    private bool skipLockedLoans;
    private int successCount;

    public int SuccessCount
    {
      get => this.successCount;
      set => this.successCount = value;
    }

    public DialogResult CommitTradeAssignments(
      int[] tradeIds,
      bool forceRefresh,
      IProgressFeedback feedback)
    {
      try
      {
        for (int index = 0; index < tradeIds.Length; ++index)
        {
          DialogResult dialogResult = this.CommitTradeAssignments(tradeIds[index], forceRefresh, feedback);
          if (dialogResult == DialogResult.Cancel)
            return dialogResult;
        }
        return DialogResult.OK;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error committing assignments: " + (object) ex);
        return DialogResult.Abort;
      }
    }

    public DialogResult CommitTradeAssignments(
      int tradeId,
      bool forceRefresh,
      IProgressFeedback feedback)
    {
      try
      {
        LoanTradeInfo trade = Session.LoanTradeManager.GetTrade(tradeId);
        if (trade == null)
          return DialogResult.Abort;
        SecurityTradeInfo securityTradeInfo = (SecurityTradeInfo) null;
        if (trade.SecurityTradeID >= 0)
          securityTradeInfo = Session.SecurityTradeManager.GetTrade(trade.SecurityTradeID);
        return this.CommitTradeAssignments(new TradeAssignmentManager(Session.SessionObjects, trade.TradeID, false), trade, forceRefresh, feedback, securityTradeInfo == null ? 0M : securityTradeInfo.Price);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error committing assignments for trade " + (object) tradeId + ": " + (object) ex);
        return DialogResult.Abort;
      }
    }

    public DialogResult CommitTradeAssignments(
      TradeAssignmentManager tradeAssignments,
      LoanTradeInfo trade,
      bool forceRefresh,
      IProgressFeedback feedback,
      Decimal securityPrice)
    {
      LoanTradeAssignment[] loanTradeAssignmentArray = !forceRefresh ? tradeAssignments.GetPendingLoans() : tradeAssignments.GetPendingAndAssignedLoans();
      feedback.Status = "Updating loans for trade '" + trade.Name + "'...";
      feedback.ResetCounter(loanTradeAssignmentArray.Length);
      List<string> skipFields = new List<string>();
      bool flag = false;
      foreach (LoanTradeAssignment assignment in loanTradeAssignmentArray)
      {
        if (feedback.Cancel)
          return DialogResult.Cancel;
        if (Session.LoanData != null && assignment.Guid == Session.LoanData.GUID)
        {
          if (assignment.PendingStatus == LoanTradeStatus.Unassigned)
            Session.LoanTradeManager.CommitPendingTradeStatus(trade.TradeID, assignment.Guid, assignment.PendingStatus, assignment.Rejected);
          int num = (int) Utils.Dialog((IWin32Window) Form.ActiveForm, "Loan file: " + Session.LoanData.LoanNumber + " is currently opened.  Please close this loan file first before it can be updated.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        else
        {
          if (!assignment.Rejected)
            assignment.Rejected = Session.LoanTradeManager.GetTradeForRejectedLoan(assignment.Guid) != null;
          if (!flag)
          {
            TradeLoanSyncDlg tradeLoanSyncDlg = new TradeLoanSyncDlg(trade.PriceAdjustments, trade);
            int num = (int) tradeLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
            skipFields = tradeLoanSyncDlg.SkipFieldList;
            flag = tradeLoanSyncDlg.ApplyToAll;
          }
          DialogResult dialogResult = this.CommitTradeAssignment(assignment, trade, forceRefresh, feedback, skipFields, securityPrice);
          switch (dialogResult)
          {
            case DialogResult.OK:
              ++this.successCount;
              break;
            case DialogResult.Cancel:
              return dialogResult;
          }
          feedback.Increment(1);
        }
      }
      return DialogResult.OK;
    }

    public DialogResult CommitTradeAssignment(
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      bool forceRefresh,
      IProgressFeedback feedback,
      List<string> skipFields,
      Decimal securityPrice)
    {
      if (assignment.PendingStatus == LoanTradeStatus.None && !forceRefresh)
        return DialogResult.OK;
      if (assignment.PendingStatus == LoanTradeStatus.None)
        return this.RefreshTradeDataInLoan(assignment.PipelineInfo, trade, feedback, skipFields, securityPrice);
      if (assignment.PendingStatus != assignment.AssignedStatus)
        return this.CommitPendingTradeStatus(assignment, trade, feedback, skipFields);
      assignment.CommitPendingStatusToLoan(trade, false);
      return this.RefreshTradeDataInLoan(assignment.PipelineInfo, trade, feedback, skipFields, securityPrice);
    }

    public DialogResult CommitPendingTradeStatus(
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      IProgressFeedback feedback,
      List<string> skipFields)
    {
      string str = string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      if (assignment.PendingStatus == LoanTradeStatus.None)
        return DialogResult.OK;
      LoanTradeStatus pendingStatus = assignment.PendingStatus;
      switch (pendingStatus)
      {
        case LoanTradeStatus.Unassigned:
          feedback.Details = "Removing loan '" + str + "' from trade...";
          break;
        case LoanTradeStatus.Assigned:
          feedback.Details = "Assigning loan '" + str + "' to trade...";
          break;
        default:
          feedback.Details = "Updating trade status for loan '" + str + "'...";
          break;
      }
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      if (pendingStatus != assignment.AssignedStatus)
      {
        DialogResult dialogResult = this.openLoan(assignment.PipelineInfo, feedback, out loanMgr);
        if (dialogResult != DialogResult.OK)
          return dialogResult;
      }
      try
      {
        if (loanMgr != null)
        {
          assignment.ApplyPendingStatusToLoan(loanMgr, trade, skipFields);
          DialogResult dialogResult = this.closeLoan(loanMgr, true, feedback);
          if (dialogResult != DialogResult.OK)
            return dialogResult;
        }
        assignment.CommitPendingStatusToLoan(trade);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error applying trade status information to loan " + assignment.Guid + ": " + (object) ex);
        int num1 = (int) Utils.Dialog((IWin32Window) feedback, "The loan '" + str + "' could not be updated due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        if (loanMgr != null)
        {
          int num2 = (int) this.closeLoan(loanMgr, false, feedback);
        }
        return DialogResult.Abort;
      }
      return DialogResult.OK;
    }

    public DialogResult RefreshTradeDataInLoans(
      TradeAssignmentManager tradeAssignments,
      LoanTradeInfo trade,
      IProgressFeedback feedback,
      Decimal securityPrice)
    {
      LoanTradeAssignment[] committedAssignedLoans = tradeAssignments.GetCommittedAssignedLoans();
      feedback.ResetCounter(committedAssignedLoans.Length);
      List<string> skipFields = new List<string>();
      bool flag = false;
      foreach (LoanTradeAssignment loanTradeAssignment in committedAssignedLoans)
      {
        if (feedback.Cancel)
          return DialogResult.Cancel;
        if (!flag)
        {
          TradeLoanSyncDlg tradeLoanSyncDlg = new TradeLoanSyncDlg(trade.PriceAdjustments, trade);
          int num = (int) tradeLoanSyncDlg.ShowDialog((IWin32Window) Form.ActiveForm);
          skipFields = tradeLoanSyncDlg.SkipFieldList;
          flag = tradeLoanSyncDlg.ApplyToAll;
        }
        DialogResult dialogResult = this.RefreshTradeDataInLoan(loanTradeAssignment.PipelineInfo, trade, feedback, skipFields, securityPrice);
        switch (dialogResult)
        {
          case DialogResult.OK:
            ++this.successCount;
            break;
          case DialogResult.Cancel:
            return dialogResult;
        }
        feedback.Increment(1);
      }
      return DialogResult.OK;
    }

    public DialogResult RefreshTradeDataInLoan(
      PipelineInfo pinfo,
      LoanTradeInfo trade,
      IProgressFeedback feedback,
      List<string> skipFields,
      Decimal securityPrice)
    {
      string str = string.Concat(pinfo.GetField("LoanNumber"));
      feedback.Details = "Refreshing trade details for loan '" + str + "'...";
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      DialogResult dialogResult = this.openLoan(pinfo, feedback, out loanMgr);
      if (dialogResult != DialogResult.OK)
        return dialogResult;
      loanMgr.RefreshTradeData((TradeInfoObj) trade, skipFields, securityPrice);
      return this.closeLoan(loanMgr, true, feedback);
    }

    private DialogResult closeLoan(LoanDataMgr loanMgr, bool save, IProgressFeedback feedback)
    {
      try
      {
        if (loanMgr.LoanData != null)
        {
          if (save)
            loanMgr.Save(false);
          loanMgr.Close();
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error saving loan " + loanMgr.LoanData.GUID + ": " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) feedback, "The loan '" + loanMgr.LoanData.LoanNumber + "' could not be saved due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        loanMgr.Close();
        return DialogResult.Abort;
      }
      return DialogResult.OK;
    }

    private DialogResult openLoan(
      PipelineInfo pinfo,
      IProgressFeedback feedback,
      out LoanDataMgr loanMgr)
    {
      loanMgr = (LoanDataMgr) null;
      string str = string.Concat(pinfo.GetField("LoanNumber"));
      try
      {
        loanMgr = LoanDataMgr.OpenLoan(Session.SessionObjects, pinfo.GUID, false);
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error opening loan " + pinfo.GUID + ": " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) feedback, "The loan '" + str + "' could not be opened due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return DialogResult.Abort;
      }
label_3:
      LockInfo[] lockInfoList = (LockInfo[]) null;
      if (Session.SessionObjects.AllowConcurrentEditing)
        lockInfoList = loanMgr.GetCurrentLocks();
      try
      {
        loanMgr.Lock(LoanInfo.LockReason.OpenForWork, LockInfo.ExclusiveLock.Exclusive);
        if (lockInfoList != null)
        {
          if (lockInfoList.Length != 0)
            throw new LockException(lockInfoList[0]);
        }
      }
      catch (LockException ex)
      {
        if (this.skipLockedLoans)
          return DialogResult.Ignore;
        using (LockedLoanDialog lockedLoanDialog = new LockedLoanDialog(loanMgr, ex, lockInfoList))
        {
          switch (lockedLoanDialog.ShowDialog((IWin32Window) feedback))
          {
            case DialogResult.Cancel:
              return DialogResult.Cancel;
            case DialogResult.Abort:
              this.skipLockedLoans = true;
              return DialogResult.Ignore;
            case DialogResult.Ignore:
              return DialogResult.Ignore;
            default:
              goto label_3;
          }
        }
      }
      catch (SecurityException ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error locking loan " + pinfo.GUID + ": " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) feedback, "You don't have access to loan '" + str + ".'  Contact your System Administrator.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        loanMgr.Close();
        return DialogResult.Abort;
      }
      catch (Exception ex)
      {
        Tracing.Log(TradeProcesses.sw, nameof (TradeProcesses), TraceLevel.Error, "Error locking loan " + pinfo.GUID + ": " + (object) ex);
        int num = (int) Utils.Dialog((IWin32Window) feedback, "The loan '" + str + "' could not be locked due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        loanMgr.Close();
        return DialogResult.Abort;
      }
      return DialogResult.OK;
    }
  }
}
