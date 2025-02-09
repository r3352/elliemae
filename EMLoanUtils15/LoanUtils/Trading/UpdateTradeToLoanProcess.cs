// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.Trading.UpdateTradeToLoanProcess
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.Trading
{
  public class UpdateTradeToLoanProcess
  {
    private const string className = "UpdateTradeToLoanProcess�";
    protected static string sw = Tracing.SwOutsideLoan;
    private object logLockObject = new object();

    public bool CommitCorrespondentTradeAssignment(
      SessionObjects sessionObjects,
      CorrespondentTradeLoanAssignment assignment,
      CorrespondentTradeInfo trade,
      bool forceRefresh,
      List<string> skipFields,
      Decimal securityPrice,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment.");
      if (assignment.PendingStatus == CorrespondentTradeLoanStatus.None && !forceRefresh)
        return false;
      Dictionary<string, string> updateFieldList = new Dictionary<string, string>();
      if (trade.DeliveryType == CorrespondentMasterDeliveryType.Bulk || trade.DeliveryType == CorrespondentMasterDeliveryType.BulkAOT)
        updateFieldList.Add("TotalPrice", assignment.TotalPrice.ToString());
      updateFieldList.Add("EPPSLoanProgramName", assignment.EPPSLoanProgramName);
      if (assignment.PendingStatus == CorrespondentTradeLoanStatus.None)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling RefreshTradeDataInLoan");
        this.refreshCorrespondentTradeDataInLoan(sessionObjects, assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList, syncOption);
      }
      else if (assignment.PendingStatus != assignment.AssignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling CommitPendingTradeStatus.");
        this.commitPendingCorrespondentTradeStatus(sessionObjects, assignment, trade, skipFields, updateFieldList, syncOption);
      }
      else
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, assignment.CommitPendingStatusToLoan");
        assignment.CommitPendingStatusToLoan(trade, false);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, RefreshTradeDataInLoan");
        this.refreshCorrespondentTradeDataInLoan(sessionObjects, assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList, syncOption);
      }
      return true;
    }

    public bool ExtendLockWithCorrespondentTrade(
      SessionObjects sessionObjects,
      CorrespondentTradeLoanAssignment assignment,
      CorrespondentTradeInfo trade,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In ExtendLockWithCorrespondentTrade.");
      Dictionary<string, string> updateFieldList = new Dictionary<string, string>();
      updateFieldList.Add("TradeExtensionInfo", assignment.TradeExtensionInfo);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In ExtendLockWithCorrespondentTrade.");
      string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In ExtendLockWithCorrespondentTrade, calling openLoan.");
      this.openLoan(sessionObjects, assignment.PipelineInfo, out loanMgr);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In ExtendLockWithCorrespondentTrade, calling loanMgr.RefreshTradeData.");
      loanMgr.ExtendLockWithTrade((TradeInfoObj) trade, updateFieldList, syncOption);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In ExtendLockWithCorrespondentTrade, calling closeLoan.");
      this.closeLoan(loanMgr, true);
      return true;
    }

    private void commitPendingCorrespondentTradeStatus(
      SessionObjects sessionObjects,
      CorrespondentTradeLoanAssignment assignment,
      CorrespondentTradeInfo trade,
      List<string> skipFields,
      Dictionary<string, string> updateFieldList = null,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus.");
      string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      if (assignment.PendingStatus == CorrespondentTradeLoanStatus.None)
        return;
      CorrespondentTradeLoanStatus pendingStatus = assignment.PendingStatus;
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      if (pendingStatus != assignment.AssignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling openLoan A.");
        this.openLoan(sessionObjects, assignment.PipelineInfo, out loanMgr);
      }
      try
      {
        if (loanMgr != null)
        {
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling assignment.ApplyPendingStatusToLoan.");
          assignment.ApplyPendingStatusToLoan(loanMgr, trade, skipFields, updateFieldList, syncOption);
          this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling closeLoan A.");
          this.closeLoan(loanMgr, true);
        }
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling assignment.CommitPendingStatusToLoan.");
        if (this.MockSEC11394(sessionObjects.CompanyInfo.ClientID))
          assignment.CommitPendingStatusToLoan(trade, false);
        else
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

    private bool MockSEC11394(string clientId)
    {
      if (clientId != "3010000024")
        return false;
      try
      {
        return File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QA-SEC-11394-Reproduce.txt"));
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private void refreshCorrespondentTradeDataInLoan(
      SessionObjects sessionObjects,
      PipelineInfo pinfo,
      CorrespondentTradeInfo trade,
      List<string> skipFields,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList = null,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan.");
      string.Concat(pinfo.GetField("LoanNumber"));
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling openLoan.");
      this.openLoan(sessionObjects, pinfo, out loanMgr);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling loanMgr.RefreshTradeData.");
      loanMgr.RefreshTradeData((TradeInfoObj) trade, skipFields, securityPrice, updateFieldList, syncOption);
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In RefreshTradeDataInLoan, calling closeLoan.");
      this.closeLoan(loanMgr, true);
    }

    public bool CommitLoanTradeAssignment(
      SessionObjects sessionObjects,
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
      updateFieldList.Add("EPPSLoanProgramName", assignment.EPPSLoanProgramName);
      if (assignment.PendingStatus == LoanTradeStatus.None)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling RefreshTradeDataInLoan");
        this.refreshLoanTradeDataInLoan(sessionObjects, assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList);
      }
      else if (assignment.PendingStatus != assignment.AssignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, calling CommitPendingTradeStatus.");
        this.commitPendingLoanTradeStatus(sessionObjects, assignment, trade, skipFields, updateFieldList);
      }
      else
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, assignment.CommitPendingStatusToLoan");
        assignment.CommitPendingStatusToLoan(trade, false);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitTradeAssignment, RefreshTradeDataInLoan");
        this.refreshLoanTradeDataInLoan(sessionObjects, assignment.PipelineInfo, trade, skipFields, securityPrice, updateFieldList);
      }
      return true;
    }

    private void commitPendingLoanTradeStatus(
      SessionObjects sessionObjects,
      LoanTradeAssignment assignment,
      LoanTradeInfo trade,
      List<string> skipFields,
      Dictionary<string, string> updateFieldList = null)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus.");
      string.Concat(assignment.PipelineInfo.GetField("LoanNumber"));
      if (assignment.PendingStatus == LoanTradeStatus.None)
        return;
      LoanTradeStatus pendingStatus = assignment.PendingStatus;
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      if (pendingStatus != assignment.AssignedStatus)
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In CommitPendingTradeStatus, calling openLoan A.");
        this.openLoan(sessionObjects, assignment.PipelineInfo, out loanMgr);
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

    private void refreshLoanTradeDataInLoan(
      SessionObjects sessionObjects,
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
      this.openLoan(sessionObjects, pinfo, out loanMgr);
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

    private void openLoan(
      SessionObjects sessionObjects,
      PipelineInfo pinfo,
      out LoanDataMgr loanMgr)
    {
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan.");
      loanMgr = (LoanDataMgr) null;
      string str1 = string.Concat(pinfo.GetField("LoanNumber"));
      try
      {
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, calling LoanDataMgr.OpenLoan.");
        loanMgr = LoanDataMgr.OpenLoan(sessionObjects, pinfo.GUID, false);
        this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, returned from LoanDataMgr.OpenLoan.");
      }
      catch (Exception ex)
      {
        this.log(TraceLevel.Error, "Error opening loan " + pinfo.GUID + ": " + (object) ex);
        throw new TradeLoanUpdateException(new TradeLoanUpdateError(pinfo.GUID, pinfo, "The loan '" + str1 + "' could not be opened due to the following error: " + ex.Message + ". You will need to correct this problem and then apply the changes to this loan at a later time."));
      }
      this.log(TraceLevel.Verbose, "Thread '" + Thread.CurrentThread.Name + "': In openLoan, top of while loop.");
      LockInfo[] lockInfoArray = (LockInfo[]) null;
      if (sessionObjects.AllowConcurrentEditing)
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
        Tracing.Log(UpdateTradeToLoanProcess.sw, nameof (UpdateTradeToLoanProcess), traceLevel, msg);
    }
  }
}
