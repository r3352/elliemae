// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.DataEngine.CorrespondentTradeToLoanBase
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.DataEngine
{
  internal class CorrespondentTradeToLoanBase : ITradeToLoan
  {
    private const string className = "CorrespondentTradeToLoanBase�";
    protected static string sw = Tracing.SwOutsideLoan;
    protected DateTime serverTime;
    protected bool isUpdate;

    protected SessionObjects sessionObjects { get; set; }

    protected LoanDataMgr loanDataMgr { get; set; }

    public CorrespondentTradeToLoanBase(SessionObjects sessionObjects, LoanDataMgr loanDataMgr)
    {
      this.sessionObjects = sessionObjects;
      this.loanDataMgr = loanDataMgr;
      this.serverTime = this.sessionObjects.Session.ServerTime;
    }

    public virtual void RemoveFromTrade(TradeInfoObj trade, bool rejected)
    {
      this.RemoveFromTrade(trade, rejected, new List<string>());
    }

    public virtual void RemoveFromTrade(
      TradeInfoObj trade,
      bool rejected,
      List<string> skipFieldList)
    {
      bool flag = ((CorrespondentTradeInfo) trade).DeliveryType == CorrespondentMasterDeliveryType.AOT || ((CorrespondentTradeInfo) trade).DeliveryType == CorrespondentMasterDeliveryType.LiveTrade || ((CorrespondentTradeInfo) trade).DeliveryType == CorrespondentMasterDeliveryType.Forwards || ((CorrespondentTradeInfo) trade).DeliveryType == CorrespondentMasterDeliveryType.BulkAOT;
      IDictionary serverSettings = this.sessionObjects.ServerManager.GetServerSettings("Policies");
      if (this.loanDataMgr.LoanData.GetField("3914") != trade.Guid)
      {
        Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Warning, "The lock is not assigned to the trade " + trade.Name + ".");
      }
      else
      {
        LockRequestLog lastConfirmedLock = this.loanDataMgr.LoanData.GetLogList().GetLastConfirmedLock();
        if (lastConfirmedLock == null)
        {
          Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Warning, "Unable to find a confirmed rate lock.");
        }
        else
        {
          Hashtable lockRequestSnapshot = lastConfirmedLock.GetLockRequestSnapshot();
          if (lockRequestSnapshot == null)
          {
            Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Warning, "Unable to find snapshot for current confirmed rate lock");
          }
          else
          {
            LockRequestLog rec = !(DateTime.Compare(DateTime.Today, ((CorrespondentTradeInfo) trade).ExpirationDate) > 0 & flag) || (bool) serverSettings[(object) "Policies.VoidedFlagEnabled"] ? (rejected || !(trade is CorrespondentTradeInfo) || ((CorrespondentTradeInfo) trade).DeliveryType != CorrespondentMasterDeliveryType.Bulk && ((CorrespondentTradeInfo) trade).DeliveryType != CorrespondentMasterDeliveryType.BulkAOT ? this.loanDataMgr.CreateLockRequestLog(RateLockRequestStatus.NotLocked, false, lastConfirmedLock.Guid) : this.loanDataMgr.CreateLockRequestLog(RateLockRequestStatus.Cancelled, false, lastConfirmedLock.Guid)) : this.loanDataMgr.CreateLockRequestLog(RateLockRequestStatus.Cancelled, false, lastConfirmedLock.Guid);
            LockRequestLog lockRequest = this.loanDataMgr.LoanData.GetLogList().GetLockRequest(lastConfirmedLock.Guid);
            if (lockRequest != null)
            {
              rec.IsLockCancellation = lockRequest.IsLockCancellation;
              rec.IsLockExtension = lockRequest.IsLockExtension;
              rec.IsRelock = lockRequest.IsRelock;
              rec.IsFakeRequest = lockRequest.IsFakeRequest;
            }
            Hashtable hashtable = lockRequestSnapshot;
            if (!((CorrespondentTradeInfo) trade).IsForIndividualLoan())
            {
              foreach (string snapshotField in LockRequestLog.SnapshotFields)
              {
                if (!LockRequestLog.LoanInfoSnapshotFields.Contains(snapshotField) && !LockRequestLog.CompSideFields.Contains(snapshotField) && !LockRequestLog.BuySideFields.Contains(snapshotField) && !LockRequestLog.LockExtensionFields.Contains(snapshotField) && !LockRequestLog.SellSideFields.Contains(snapshotField))
                  hashtable[(object) snapshotField] = (object) string.Empty;
              }
            }
            foreach (string buySideField in LockRequestLog.BuySideFields)
            {
              if (buySideField != "2029")
                hashtable[(object) buySideField] = (object) string.Empty;
            }
            if (rec.IsLockExtension)
            {
              if (!((CorrespondentTradeInfo) trade).IsForIndividualLoan())
              {
                foreach (string lockExtensionField in LockRequestLog.LockExtensionFields)
                  hashtable[(object) lockExtensionField] = (object) string.Empty;
              }
              else
              {
                hashtable[(object) "3358"] = (object) string.Empty;
                hashtable[(object) "3363"] = (object) string.Empty;
                hashtable[(object) "3364"] = (object) string.Empty;
                hashtable[(object) "3365"] = (object) string.Empty;
              }
            }
            rec.AddLockRequestSnapshot(hashtable);
            new LockRequestCalculator(this.sessionObjects, this.loanDataMgr.LoanData).PerformSnapshotCalculations(hashtable);
            this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) rec);
            foreach (LockRequestLog allLockRequest in this.loanDataMgr.LoanData.GetLogList().GetAllLockRequests())
            {
              if (allLockRequest != rec)
              {
                if (allLockRequest.LockRequestStatus == RateLockRequestStatus.RateLocked || allLockRequest.LockRequestStatus == RateLockRequestStatus.Cancelled)
                  allLockRequest.LockRequestStatus = RateLockRequestStatus.OldLock;
                else if (allLockRequest.LockRequestStatus == RateLockRequestStatus.Requested)
                  allLockRequest.LockRequestStatus = RateLockRequestStatus.OldRequest;
              }
            }
            this.loanDataMgr.SyncLockRequestSnapshotToLoan(hashtable, false, false);
            LockRemovedLog lockRemovedLog = this.loanDataMgr.CreateLockRemovedLog(rec.Guid);
            lockRemovedLog.DisplayInLog = true;
            this.loanDataMgr.LoanData.GetLogList().AddRecord((LogRecordBase) lockRemovedLog);
            this.loanDataMgr.LoanData.SetCurrentField("3941", "N");
            this.loanDataMgr.LoanData.SetCurrentField("761", "");
            this.loanDataMgr.LoanData.SetCurrentField("762", "");
            this.loanDataMgr.LoanData.SetCurrentField("432", "");
            this.loanDataMgr.LoanData.SetCurrentField("4105", "");
            this.loanDataMgr.LoanData.SetCurrentField("2400", "");
            this.loanDataMgr.LoanData.SetCurrentField("3907", "");
            this.loanDataMgr.LoanData.SetCurrentField("3908", "");
            this.loanDataMgr.LoanData.SetCurrentField("3578", "");
            this.loanDataMgr.LoanData.SetCurrentField("3926", "");
            this.loanDataMgr.LoanData.SetCurrentField("3924", "");
            this.loanDataMgr.LoanData.SetCurrentField("3923", "");
            this.loanDataMgr.LoanData.SetCurrentField("3967", "");
            this.loanDataMgr.LoanData.SetCurrentField("2203", "");
            this.loanDataMgr.LoanData.SetCurrentField("2205", "");
            this.loanDataMgr.LoanData.SetField("3915", "");
            this.loanDataMgr.LoanData.SetField("3914", "");
            if (DateTime.Compare(DateTime.Today, ((CorrespondentTradeInfo) trade).ExpirationDate) > 0 & flag && !(bool) serverSettings[(object) "Policies.VoidedFlagEnabled"])
            {
              if (this.loanDataMgr.LoanData.GetField("4119") != "//")
              {
                this.loanDataMgr.LoanData.SetField("4120", DateTime.Today.ToString());
                this.loanDataMgr.LoanData.SetField("4207", "");
                this.loanDataMgr.LoanData.SetField("4208", "");
              }
              else
              {
                this.loanDataMgr.LoanData.SetField("4207", DateTime.Today.ToString());
                this.loanDataMgr.LoanData.SetField("4120", "");
                this.loanDataMgr.LoanData.SetField("4208", "");
              }
            }
            DateTime today;
            if (rejected)
            {
              LoanData loanData = this.loanDataMgr.LoanData;
              today = DateTime.Today;
              string val = today.ToString();
              loanData.SetField("4208", val);
            }
            this.loanDataMgr.LoanData.TriggerCalculation("761", this.loanDataMgr.LoanData.GetField("761"));
            if (this.loanDataMgr.LoanData.Use2010RESPA || this.loanDataMgr.LoanData.Use2015RESPA)
              this.loanDataMgr.LoanData.TriggerCalculation("2400", this.loanDataMgr.LoanData.GetField("2400"));
            if (rejected || !(trade is CorrespondentTradeInfo) || ((CorrespondentTradeInfo) trade).DeliveryType != CorrespondentMasterDeliveryType.Bulk && ((CorrespondentTradeInfo) trade).DeliveryType != CorrespondentMasterDeliveryType.BulkAOT)
              return;
            if (this.loanDataMgr.LoanData.GetField("4119") == "//")
            {
              LoanData loanData = this.loanDataMgr.LoanData;
              today = DateTime.Today;
              string val = today.ToString();
              loanData.SetField("4207", val);
              this.loanDataMgr.LoanData.SetField("4120", "");
              this.loanDataMgr.LoanData.SetField("4208", "");
            }
            else
            {
              LoanData loanData = this.loanDataMgr.LoanData;
              today = DateTime.Today;
              string val = today.ToString();
              loanData.SetField("4120", val);
              this.loanDataMgr.LoanData.SetField("4207", "");
              this.loanDataMgr.LoanData.SetField("4208", "");
            }
          }
        }
      }
    }

    public virtual void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice)
    {
      this.AssignToTrade(trade, skipFieldList, securityPrice, new Dictionary<string, string>(), LoanDataMgr.LockLoanSyncOption.syncLockToLoan);
    }

    public virtual void AssignToTrade(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal securityPrice,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Info, "Assigning loan to trade " + (object) trade.TradeID + "..");
      skipFieldList = skipFieldList ?? new List<string>();
      if (!((CorrespondentTradeInfo) trade).IsForIndividualLoan())
        this.loanDataMgr.LoanData.SetField("2029", this.sessionObjects.UserInfo.FullName);
      LockRequestLog rateLockRequest = this.loanDataMgr.CreateRateLockRequest(false, true, true);
      Hashtable lockRequestSnapshot = rateLockRequest.GetLockRequestSnapshot();
      rateLockRequest.DisplayInLog = !((CorrespondentTradeInfo) trade).IsForIndividualLoan() && !this.isUpdate;
      LockRequestLog lastConfirmedLock = this.loanDataMgr.LoanData.GetLogList().GetLastConfirmedLock();
      if (lastConfirmedLock != null)
        this.CopyConfirmedSnapshot(rateLockRequest, lockRequestSnapshot, lastConfirmedLock);
      this.loanDataMgr.LoanData.SetField("3360", "");
      this.loanDataMgr.LoanData.SetField("3361", "");
      this.loanDataMgr.LoanData.SetField("3362", "");
      this.loanDataMgr.LoanData.SetField("3369", "");
      this.loanDataMgr.LoanData.SetField("4120", "");
      this.loanDataMgr.LoanData.SetField("4207", "");
      this.loanDataMgr.LoanData.SetField("4208", "");
      this.loanDataMgr.LoanData.SetField("3914", trade.Guid);
      this.CopyTradeDataToSnapshot(trade, skipFieldList, securityPrice, lockRequestSnapshot, updateFieldList);
      lockRequestSnapshot[(object) "4209"] = (object) LockUtils.GetRequestLockStatus(this.loanDataMgr.LoanData);
      if (!((CorrespondentTradeInfo) trade).IsForIndividualLoan())
        this.ClearNonProductAdjustments(lockRequestSnapshot);
      rateLockRequest.AddLockRequestSnapshot(lockRequestSnapshot);
      LockRequestCalculator requestCalculator = new LockRequestCalculator(this.sessionObjects, this.loanDataMgr.LoanData);
      if (!((CorrespondentTradeInfo) trade).IsForIndividualLoan())
        requestCalculator.PerformSnapshotCalculations(lockRequestSnapshot, true, false, false, false);
      else
        requestCalculator.PerformSnapshotCalculations(lockRequestSnapshot);
      this.loanDataMgr.LockRateRequest(rateLockRequest, lockRequestSnapshot, this.sessionObjects.UserInfo, true, true, syncOption: syncOption);
    }

    private void ClearNonProductAdjustments(Hashtable newData)
    {
      for (int index = 3454; index <= 3473; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 3474; index <= 3493; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 3494; index <= 3513; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 3755; index <= 3774; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4256; index <= 4275; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4276; index <= 4295; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4296; index <= 4315; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4316; index <= 4335; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4336; index <= 4355; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4356; index <= 4375; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4376; index <= 4395; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      for (int index = 4396; index <= 4415; ++index)
      {
        if (newData.ContainsKey((object) index.ToString()))
          newData[(object) index.ToString()] = (object) string.Empty;
      }
      if (newData.ContainsKey((object) "3433"))
        newData[(object) "3433"] = (object) "0";
      if (!newData.ContainsKey((object) "3431"))
        return;
      newData[(object) "3431"] = (object) "0";
    }

    public void CopyPreviousSnapshot(TradeInfoObj trade, bool remove, List<string> skipFieldList = null)
    {
    }

    public virtual void CopyTradeDataToSnapshot(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Hashtable newData,
      Dictionary<string, string> updateFieldList)
    {
    }

    private void CopyConfirmedSnapshot(
      LockRequestLog newLock,
      Hashtable newData,
      LockRequestLog confirmedLog)
    {
      LockRequestLog lockRequestLog = confirmedLog;
      if (lockRequestLog == null)
        return;
      Hashtable lockRequestSnapshot = lockRequestLog.GetLockRequestSnapshot();
      if (lockRequestSnapshot == null)
        return;
      foreach (string currentLockField in LockRequestLog.CurrentLockFields)
        newData[(object) currentLockField] = (object) string.Concat(lockRequestSnapshot[(object) currentLockField]);
      foreach (string snapshotField in LockRequestLog.SnapshotFields)
        newData[(object) snapshotField] = (object) string.Concat(lockRequestSnapshot[(object) snapshotField]);
      foreach (string buySideField in LockRequestLog.BuySideFields)
        newData[(object) buySideField] = (object) string.Concat(lockRequestSnapshot[(object) buySideField]);
      foreach (string compSideField in LockRequestLog.CompSideFields)
        newData[(object) compSideField] = (object) string.Concat(lockRequestSnapshot[(object) compSideField]);
      if (lockRequestSnapshot.ContainsKey((object) "OPTIMAL.HISTORY"))
        newData[(object) "OPTIMAL.HISTORY"] = (object) string.Concat(lockRequestSnapshot[(object) "OPTIMAL.HISTORY"]);
      if (lockRequestSnapshot.ContainsKey((object) "3848"))
        newData[(object) "3848"] = (object) lockRequestSnapshot[(object) "3848"].ToString();
      if (lockRequestSnapshot.ContainsKey((object) "3873"))
        newData[(object) "3873"] = (object) lockRequestSnapshot[(object) "3873"].ToString();
      if (lockRequestSnapshot.ContainsKey((object) "3875"))
        newData[(object) "3875"] = (object) lockRequestSnapshot[(object) "3875"].ToString();
      if (!lockRequestLog.IsLockExtension)
        return;
      foreach (string key in new List<string>()
      {
        "2088",
        "3254",
        "2090",
        "2091"
      })
      {
        if (lockRequestSnapshot.ContainsKey((object) key) && (!(key == "3254") && !(key == "2091") || Utils.IsDate(lockRequestSnapshot[(object) key])))
          newData[(object) key] = (object) lockRequestSnapshot[(object) key].ToString();
      }
      for (int index = 0; index < LockRequestLog.LockExtensionFields.Count; ++index)
      {
        string lockExtensionField = LockRequestLog.LockExtensionFields[index];
        if (lockRequestSnapshot.ContainsKey((object) lockExtensionField))
          newData[(object) lockExtensionField] = (object) lockRequestSnapshot[(object) lockExtensionField].ToString();
      }
      if (lockRequestSnapshot.ContainsKey((object) "3431"))
        newData[(object) "3431"] = (object) lockRequestSnapshot[(object) "3431"].ToString();
      if (!lockRequestSnapshot.ContainsKey((object) "3433"))
        return;
      newData[(object) "3433"] = (object) lockRequestSnapshot[(object) "3433"].ToString();
    }

    public virtual void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList)
    {
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Info, "Setting correspondent trade status to " + status + "..");
      int num = this.loanDataMgr.LoanData.GetField("2819") != trade.Guid ? 1 : 0;
    }

    public virtual void ModifyTradeStatus(
      TradeInfoObj trade,
      object status,
      List<string> skipFieldList,
      Decimal price)
    {
      this.ModifyTradeStatus(trade, status, skipFieldList, price, new Dictionary<string, string>());
    }

    public virtual void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan,
      bool forAssignment = false)
    {
      this.isUpdate = true;
      this.AssignToTrade(trade, skipFieldList, price, updateFieldList, syncOption);
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Info, "Trade data refresh completed");
    }

    public virtual void RefreshTradeData(
      TradeInfoObj trade,
      List<string> skipFieldList,
      Decimal price)
    {
      this.RefreshTradeData(trade, skipFieldList, price, new Dictionary<string, string>(), LoanDataMgr.LockLoanSyncOption.syncLockToLoan, false);
    }

    public virtual void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor)
    {
    }

    public virtual void ApplyInvestorToLoan(
      Investor investor,
      ContactInformation assignee,
      bool updateInvestor,
      List<string> skipFieldList)
    {
    }

    protected void SetDateTimeField(Hashtable data, string fieldId, DateTime value)
    {
      if (value != DateTime.MinValue)
        data[(object) fieldId] = (object) value.ToString("MM/dd/yyyy");
      else
        data[(object) fieldId] = (object) "";
    }

    public virtual void ExtendLockWithTrade(
      TradeInfoObj trade,
      Dictionary<string, string> updateFieldList,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Info, "Extend lock for loan from trade " + (object) trade.TradeID + "..");
      LockUtils lockUtils = new LockUtils(this.sessionObjects);
      if (this.loanDataMgr.LoanData.GetField("3914") != trade.Guid)
      {
        Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Warning, "The lock is not assigned to the trade " + trade.Name + ".");
      }
      else
      {
        LockRequestLog lockRequestLog = (LockRequestLog) null;
        string field = this.loanDataMgr.LoanData.GetField("LOCKRATE.RATESTATUS");
        if (!(field == "Cancelled") && !(field == "Expired"))
        {
          lockRequestLog = this.loanDataMgr.LoanData.GetLogList().GetLastConfirmedLock();
        }
        else
        {
          LockConfirmLog recentConfirmedLock = this.loanDataMgr.LoanData.GetLogList().GetMostRecentConfirmedLock();
          if (recentConfirmedLock != null)
            lockRequestLog = this.loanDataMgr.LoanData.GetLogList().GetLockRequest(recentConfirmedLock.RequestGUID);
        }
        if (lockRequestLog == null)
        {
          Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Error, "No current lock request found for loan " + this.loanDataMgr.LoanData.GUID + " in ExtendLockWithTradeData.");
        }
        else
        {
          Hashtable lockRequestSnapshot = lockRequestLog.GetLockRequestSnapshot();
          if (lockRequestSnapshot == null)
          {
            Tracing.Log(CorrespondentTradeToLoanBase.sw, nameof (CorrespondentTradeToLoanBase), TraceLevel.Error, "No lock request snapshot found for loan " + this.loanDataMgr.LoanData.GUID + " in ExtendLockWithTradeData");
          }
          else
          {
            LockRequestLog lockLog = new LockRequestLog(this.loanDataMgr.LoanData.GetLogList());
            lockLog.Date = this.sessionObjects.Session.ServerTime;
            lockLog.LockRequestStatus = RateLockRequestStatus.Requested;
            lockLog.IsLockExtension = true;
            lockLog.SetRequestingUser(this.sessionObjects.UserID, this.sessionObjects.UserInfo.FullName);
            lockLog.ParentLockGuid = lockRequestLog.Guid;
            lockLog.RateLockAction = RateLockAction.TradeExtension;
            string empty = string.Empty;
            int daysToExtend = 0;
            Decimal priceAdjustment = 0M;
            Decimal reLockFee = 0M;
            Decimal customPriceAdj = 0M;
            string cpaDesc = string.Empty;
            string comment = string.Empty;
            DateTime newExpirationDate = DateTime.Today;
            if (updateFieldList.ContainsKey("TradeExtensionInfo"))
            {
              string updateField = updateFieldList["TradeExtensionInfo"];
              string[] source1 = updateField.Split(',');
              string source2 = ((IEnumerable<string>) source1).Where<string>((Func<string, bool>) (s => s.Contains("DaysToExtend"))).FirstOrDefault<string>();
              if (source2 != null && source2.Contains<char>(':'))
                daysToExtend = Utils.ParseInt((object) source2.Substring(source2.IndexOf(":") + 1), 0);
              string source3 = ((IEnumerable<string>) source1).Where<string>((Func<string, bool>) (s => s.Contains("PriceAdjustment"))).FirstOrDefault<string>();
              if (source3 != null && source3.Contains<char>(':'))
                priceAdjustment = Utils.ParseDecimal((object) source3.Substring(source3.IndexOf(":") + 1), 0M);
              string source4 = ((IEnumerable<string>) source1).Where<string>((Func<string, bool>) (s => s.Contains("ReLockFee"))).FirstOrDefault<string>();
              if (source4 != null && source4.Contains<char>(':'))
                reLockFee = Utils.ParseDecimal((object) source4.Substring(source4.IndexOf(":") + 1), 0M);
              string source5 = ((IEnumerable<string>) source1).Where<string>((Func<string, bool>) (s => s.Contains("CustomPriceAdjustment"))).FirstOrDefault<string>();
              if (source5 != null && source5.Contains<char>(':'))
                customPriceAdj = Utils.ParseDecimal((object) source5.Substring(source5.IndexOf(":") + 1), 0M);
              string source6 = ((IEnumerable<string>) source1).Where<string>((Func<string, bool>) (s => s.Contains("CustomPriceDescription"))).FirstOrDefault<string>();
              if (source6 != null && source6.Contains<char>(':'))
                cpaDesc = source6.Substring(source6.IndexOf(":") + 1);
              if (updateField.IndexOf("Comment:") > 0)
              {
                string[] source7 = updateField.Substring(updateField.IndexOf("Comment:")).Split(':');
                if (source7.Length > 1)
                  comment = string.Join(":", ((IEnumerable<string>) source7).Skip<string>(1));
              }
              LockExtensionUtils lockExtensionUtils = new LockExtensionUtils(this.sessionObjects, this.loanDataMgr.LoanData);
              newExpirationDate = lockRequestSnapshot[(object) "3433"] == null || Utils.ParseInt((object) lockRequestSnapshot[(object) "3433"].ToString(), 0) <= 0 ? lockExtensionUtils.GetExtensionExpirationDate(Utils.ParseDate(lockRequestSnapshot[(object) "2151"]), daysToExtend) : lockExtensionUtils.GetExtensionExpirationDate(Utils.ParseDate(lockRequestSnapshot[(object) "3364"]), daysToExtend);
            }
            this.loanDataMgr.CreateExtendRateLockRequest(lockLog, lockRequestSnapshot, lockRequestLog.IsLockExtension, true, daysToExtend, newExpirationDate, priceAdjustment, comment, reLockFee, customPriceAdj, cpaDesc, syncOption);
          }
        }
      }
    }

    public virtual void ClearUlddFields(MbsPoolMortgageType poolMortgageType)
    {
    }
  }
}
