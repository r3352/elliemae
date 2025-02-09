// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Trading.CorrespondentTradeProcesses
// Assembly: TradeManagement, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: 30F9401A-5385-498E-9C5B-D147722AC94C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\TradeManagement.dll

using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.LoanUtils.Trading;
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
  public class CorrespondentTradeProcesses
  {
    private const string className = "CorrespondentTradeProcess";
    private static string sw = Tracing.SwOutsideLoan;
    private CorrespondentTradeProcesses.ActionType updateAction;
    private CorrespondentTradeLoanAssignmentManager assignmentMgr;
    private int[] tradeIds;
    private CorrespondentTradeInfo correspondentTrade;
    private bool forceUpdateOfAllLoans;
    private Decimal securityPrice;
    private Queue<CorrespondentTradeAssignmentItem> assignmentQueue = new Queue<CorrespondentTradeAssignmentItem>();
    private List<string> skipFieldList = new List<string>();
    private Dictionary<CorrespondentTradeInfo, CorrespondentTradeLoanAssignmentManager> tradeAssignments;
    private Dictionary<CorrespondentTradeInfo, CorrespondentTradeLoanAssignment[]> corrAssignments = new Dictionary<CorrespondentTradeInfo, CorrespondentTradeLoanAssignment[]>();
    private bool skipSyncDialog = true;
    private string tradeExtensionInfo;
    private readonly ITradeEditor editor;
    private static string smartClientMultiThreadingSetting = (string) null;
    private object logLockObject = new object();

    public CorrespondentTradeProcesses()
    {
    }

    public CorrespondentTradeProcesses(ITradeEditor editor) => this.editor = editor;

    public void Execute(
      CorrespondentTradeProcesses.ActionType updateAction,
      CorrespondentTradeInfo trade,
      CorrespondentTradeLoanAssignmentManager assignmentManager,
      bool forceUpdateOfAllLoans,
      Decimal securityPrice,
      bool skipSyncDialog = true)
    {
      this.correspondentTrade = trade;
      this.assignmentMgr = assignmentManager;
      this.updateAction = updateAction;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.securityPrice = securityPrice;
      this.skipSyncDialog = skipSyncDialog;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    public void Execute(
      CorrespondentTradeProcesses.ActionType updateAction,
      int[] tradeIdsObj,
      bool forceUpdateOfAllLoans,
      bool skipSyncDialog = true)
    {
      this.tradeIds = tradeIdsObj;
      this.updateAction = updateAction;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.skipSyncDialog = skipSyncDialog;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    public void Execute(
      CorrespondentTradeProcesses.ActionType updateAction,
      Dictionary<CorrespondentTradeInfo, CorrespondentTradeLoanAssignmentManager> tradeAssignments,
      int[] tradeIds,
      bool forceUpdateOfAllLoans,
      Decimal securityPrice,
      bool skipSyncDialog = true,
      string tradeExtensionInfo = null)
    {
      if (tradeAssignments.Count > 1)
      {
        this.tradeAssignments = tradeAssignments;
        this.tradeIds = tradeIds;
      }
      else
      {
        this.correspondentTrade = tradeAssignments.First<KeyValuePair<CorrespondentTradeInfo, CorrespondentTradeLoanAssignmentManager>>().Key;
        this.assignmentMgr = tradeAssignments.First<KeyValuePair<CorrespondentTradeInfo, CorrespondentTradeLoanAssignmentManager>>().Value;
      }
      this.updateAction = updateAction;
      this.forceUpdateOfAllLoans = forceUpdateOfAllLoans;
      this.securityPrice = securityPrice;
      this.skipSyncDialog = skipSyncDialog;
      this.tradeExtensionInfo = tradeExtensionInfo;
      if (!Utils.ParseBoolean(Session.StartupInfo.TradeSettings[(object) "Trade.TradeLoanUpdateKafka"]))
      {
        this.PrepareTradeAssignmentQueue();
        this.Process();
      }
      else
        this.PrepareTradeAssignmentKafkaQueue();
    }

    private void ProcessKafka()
    {
      if (this.corrAssignments != null && this.corrAssignments.Count == 0)
        return;
      string str1 = string.Empty;
      int num = 0;
      string pendingTradesNames = string.Empty;
      int pendingTradesCount = 0;
      foreach (CorrespondentTradeInfo key in this.corrAssignments.Keys)
      {
        CorrespondentTradeLoanAssignment[] corrAssignment = this.corrAssignments[key];
        if (corrAssignment != null)
        {
          if (((IEnumerable<CorrespondentTradeLoanAssignment>) corrAssignment).Count<CorrespondentTradeLoanAssignment>() > 0)
          {
            try
            {
              if (this.updateAction == CorrespondentTradeProcesses.ActionType.Void)
                key.Status = TradeStatus.Voided;
              TradeSync.CorrespondentTradeSync((TradeInfoObj) key, corrAssignment, Session.SessionObjects, this.updateAction == CorrespondentTradeProcesses.ActionType.ExtendLock, this.tradeExtensionInfo);
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
        this.correspondentTrade.Status = TradeStatus.Pending;
        ((CorrespondentTradeEditor) this.editor).MakePending(true);
      }
      string elligibleMesg = "";
      if (num > 0)
      {
        string str2 = str1.Substring(0, str1.Length - 2);
        elligibleMesg = num <= 1 ? "Correspondent Trade " + str2 + " has been successfully submitted for update and is in pending status" : "Correspondent Trade " + str2 + " have been successfully submitted for update and is in pending status";
      }
      this.displayActionResults(pendingTradesCount, pendingTradesNames, "Correspondent Trade", elligibleMesg);
    }

    private void PrepareTradeAssignmentKafkaQueue()
    {
      if (this.tradeAssignments != null)
      {
        List<int> intList = new List<int>();
        foreach (CorrespondentTradeInfo key in this.tradeAssignments.Keys)
        {
          CorrespondentTradeLoanAssignment[] modifiedLoans = this.tradeAssignments[key].GetModifiedLoans();
          this.corrAssignments.Add(key, modifiedLoans);
        }
      }
      else if (this.assignmentMgr != null)
      {
        this.corrAssignments.Add(this.correspondentTrade, this.forceUpdateOfAllLoans || this.updateAction == CorrespondentTradeProcesses.ActionType.ExtendLock ? this.assignmentMgr.GetPendingAndAssignedLoans() : this.assignmentMgr.GetPendingLoans());
      }
      else
      {
        if (this.tradeIds == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int tradeId in this.tradeIds)
        {
          CorrespondentTradeInfo trade = Session.CorrespondentTradeManager.GetTrade(tradeId);
          if (trade != null)
          {
            CorrespondentTradeLoanAssignmentManager source = new CorrespondentTradeLoanAssignmentManager(Session.SessionObjects, trade.TradeID, false);
            this.corrAssignments.Add(trade, source.ToArray<CorrespondentTradeLoanAssignment>());
          }
        }
      }
      this.ProcessKafka();
    }

    private void Process()
    {
      if (this.assignmentQueue != null && this.assignmentQueue.Count == 0)
        return;
      if (this.correspondentTrade != null)
      {
        TradeUpdateLoansDialog.Insert(this.updateAction, this.assignmentQueue.First<CorrespondentTradeAssignmentItem>().Trade, this.forceUpdateOfAllLoans, this.securityPrice, this.assignmentQueue.Select<CorrespondentTradeAssignmentItem, CorrespondentTradeLoanAssignment>((Func<CorrespondentTradeAssignmentItem, CorrespondentTradeLoanAssignment>) (q => q.Assignment)).ToArray<CorrespondentTradeLoanAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
        if (this.editor != null)
        {
          this.correspondentTrade.Status = TradeStatus.Pending;
          ((CorrespondentTradeEditor) this.editor).MakePending(true);
        }
        int num = (int) Utils.Dialog((IWin32Window) Session.MainScreen, "Correspondent Trade " + this.assignmentQueue.First<CorrespondentTradeAssignmentItem>().Trade.Name + " has been successfully submitted for update and is in pending status", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
          IEnumerable<CorrespondentTradeAssignmentItem> source = this.assignmentQueue.Where<CorrespondentTradeAssignmentItem>((Func<CorrespondentTradeAssignmentItem, bool>) (a => a.Trade.TradeID == tradeId));
          if (source != null)
          {
            if (source.Count<CorrespondentTradeAssignmentItem>() > 0)
            {
              try
              {
                TradeUpdateLoansDialog.Insert(this.updateAction, source.First<CorrespondentTradeAssignmentItem>().Trade, this.forceUpdateOfAllLoans, this.securityPrice, source.Select<CorrespondentTradeAssignmentItem, CorrespondentTradeLoanAssignment>((Func<CorrespondentTradeAssignmentItem, CorrespondentTradeLoanAssignment>) (q => q.Assignment)).ToArray<CorrespondentTradeLoanAssignment>(), this.skipFieldList, this.forceUpdateOfAllLoans);
                str1 = str1 + source.First<CorrespondentTradeAssignmentItem>().Trade.Name + ", ";
                ++num;
              }
              catch (TradeNotUpdateException ex)
              {
                ++pendingTradesCount;
                pendingTradesNames = pendingTradesNames + source.First<CorrespondentTradeAssignmentItem>().Trade.Name + ", ";
              }
            }
          }
        }
        string elligibleMesg = "";
        if (num > 0)
        {
          string str2 = str1.Substring(0, str1.Length - 2);
          elligibleMesg = num <= 1 ? "Correspondent Trade " + str2 + " has been successfully submitted for update and is in pending status" : "Correspondent Trade " + str2 + " have been successfully submitted for update and is in pending status";
        }
        this.displayActionResults(pendingTradesCount, pendingTradesNames, "Correspondent Trade", elligibleMesg);
      }
    }

    private void PrepareTradeAssignmentQueue()
    {
      if (this.tradeAssignments != null)
      {
        List<int> intList = new List<int>();
        foreach (CorrespondentTradeInfo key in this.tradeAssignments.Keys)
        {
          foreach (CorrespondentTradeLoanAssignment modifiedLoan in this.tradeAssignments[key].GetModifiedLoans())
            this.assignmentQueue.Enqueue(new CorrespondentTradeAssignmentItem(key, modifiedLoan, this.securityPrice));
        }
      }
      else if (this.assignmentMgr != null)
      {
        foreach (CorrespondentTradeLoanAssignment assignment in this.forceUpdateOfAllLoans || this.updateAction == CorrespondentTradeProcesses.ActionType.ExtendLock ? this.assignmentMgr.GetPendingAndAssignedLoans() : this.assignmentMgr.GetPendingLoans())
          this.assignmentQueue.Enqueue(new CorrespondentTradeAssignmentItem(this.correspondentTrade, assignment, this.securityPrice));
      }
      else
      {
        if (this.tradeIds == null)
          throw new InvalidOperationException("Either assignmentMgr or tradeIds must have a value.");
        foreach (int tradeId in this.tradeIds)
        {
          CorrespondentTradeInfo trade = Session.CorrespondentTradeManager.GetTrade(tradeId);
          if (trade != null)
          {
            foreach (CorrespondentTradeLoanAssignment assignment in new CorrespondentTradeLoanAssignmentManager(Session.SessionObjects, trade.TradeID, false))
              this.assignmentQueue.Enqueue(new CorrespondentTradeAssignmentItem(trade, assignment, this.securityPrice));
          }
        }
      }
    }

    private void executeComplete(CorrespondentLoanUpdateDialog progressForm)
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
      CorrespondentTradeLoanAssignment assignment,
      CorrespondentTradeInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice)
    {
      try
      {
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, Loan: " + assignment.PipelineInfo.LoanNumber);
        UpdateTradeToLoanProcess tradeToLoanProcess = new UpdateTradeToLoanProcess();
        string companySetting = Session.ConfigurationManager.GetCompanySetting("POLICIES", "LockRequestToLoanDataSyncOption");
        if (skipFields != null && skipFields.Any<string>((Func<string, bool>) (s => s == "ExtendLock")))
        {
          this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling ExtendLockWithCorrespondentTrade.");
          bool flag = string.IsNullOrEmpty(companySetting) || (LoanDataMgr.LockLoanSyncOption) Enum.Parse(typeof (LoanDataMgr.LockLoanSyncOption), companySetting) != LoanDataMgr.LockLoanSyncOption.noSync ? tradeToLoanProcess.ExtendLockWithCorrespondentTrade(Session.SessionObjects, assignment, trade) : tradeToLoanProcess.ExtendLockWithCorrespondentTrade(Session.SessionObjects, assignment, trade, LoanDataMgr.LockLoanSyncOption.noSync);
          this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, returned from ExtendLockWithCorrespondentTrade.");
          return flag;
        }
        if (Session.LoanData != null && assignment.Guid == Session.LoanData.GUID)
        {
          if (assignment.PendingStatus == CorrespondentTradeLoanStatus.Unassigned)
          {
            this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling CommitPendingLoanTradeStatus");
            Session.CorrespondentTradeManager.CommitPendingTradeStatus(trade.TradeID, assignment.Guid, assignment.PendingStatus, assignment.Rejected);
          }
          throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, "Loan file is currently opened.  Please close this loan file first before it can be updated."));
        }
        if (!assignment.Rejected)
          assignment.Rejected = Session.CorrespondentTradeManager.GetTradeForRejectedLoan(assignment.Guid) != null;
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, calling CommitTradeAssignment");
        bool flag1 = string.IsNullOrEmpty(companySetting) || (LoanDataMgr.LockLoanSyncOption) Enum.Parse(typeof (LoanDataMgr.LockLoanSyncOption), companySetting) != LoanDataMgr.LockLoanSyncOption.noSync ? tradeToLoanProcess.CommitCorrespondentTradeAssignment(Session.SessionObjects, assignment, trade, forceRefresh, skipFields, securityPrice) : tradeToLoanProcess.CommitCorrespondentTradeAssignment(Session.SessionObjects, assignment, trade, forceRefresh, skipFields, securityPrice, LoanDataMgr.LockLoanSyncOption.noSync);
        this.log(TraceLevel.Verbose, "TradeMgmt, thread'" + Thread.CurrentThread.Name + "': In WorkOneLoan, returned from CommitTradeAssignment.");
        return flag1;
      }
      catch (TradeLoanUpdateException ex)
      {
        throw;
      }
      catch (EllieMae.EMLite.ClientServer.Exceptions.TradeLoanUpdateException ex)
      {
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, ex.Error.Message));
      }
      catch (Exception ex)
      {
        Tracing.Log(CorrespondentTradeProcesses.sw, "CorrespondentTradeProcess", TraceLevel.Error, "Error in WorkOneLoan for " + assignment.Guid + ": " + ex.ToString());
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(assignment.Guid, assignment.PipelineInfo, ex.Message));
      }
    }

    public static bool IsTradeProcesses2Enabled()
    {
      if (CorrespondentTradeProcesses.smartClientMultiThreadingSetting == null)
      {
        try
        {
          CorrespondentTradeProcesses.smartClientMultiThreadingSetting = SmartClientUtils.GetAttribute(Session.CompanyInfo.ClientID, "Encompass.exe", "TradeAssignmentUseMultithreading");
        }
        catch (Exception ex)
        {
        }
      }
      if (CorrespondentTradeProcesses.smartClientMultiThreadingSetting == "0")
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

    private void log(TraceLevel traceLevel, string msg)
    {
      lock (this.logLockObject)
        Tracing.Log(CorrespondentTradeProcesses.sw, "CorrespondentTradeProcess", traceLevel, msg);
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
      Void,
      ExtendLock,
    }
  }
}
