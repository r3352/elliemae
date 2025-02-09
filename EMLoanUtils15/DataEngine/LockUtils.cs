// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.LockUtils
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.EPC2;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.LoanUtils.RateLocks;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  public class LockUtils
  {
    private SessionObjects sessionObjects;

    public LockUtils(SessionObjects sessionObjects) => this.sessionObjects = sessionObjects;

    public string FormatSecondaryDecimalDigits(string fieldID, string origValue)
    {
      bool needsUpdate = false;
      FieldDefinition field = EncompassFields.GetField(fieldID);
      return field == null ? origValue : (!this.sessionObjects.Use10DecimalDigitInLockRequestSecondaryTradeAreas || !LockRequestUtils.IsLockRequestSecondary10DigitFormatingFields(fieldID) ? Utils.FormatInput(origValue, field.Format, ref needsUpdate) : Utils.FormatInput(origValue, FieldFormat.DECIMAL_10, ref needsUpdate));
    }

    public string FormatSecondaryDecimalDigits(string fieldID, Decimal origValue)
    {
      return this.FormatSecondaryDecimalDigits(fieldID, origValue.ToString());
    }

    public static LockRequestLog GetAssignToTradePostConfirmationLock(LoanDataMgr loanMgr)
    {
      if (loanMgr == null)
        return (LockRequestLog) null;
      LockRequestLog currentLockRequest = loanMgr.LoanData.GetLogList().GetCurrentLockRequest();
      if (currentLockRequest == null)
        return (LockRequestLog) null;
      if (currentLockRequest.RequestedStatus != RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
        return (LockRequestLog) null;
      return string.IsNullOrEmpty((string) currentLockRequest.GetLockRequestSnapshot()[(object) "3839"]) ? (LockRequestLog) null : currentLockRequest;
    }

    public static LockRequestLog GetLastLockRequestForVoid(LoanDataMgr loanDatamgr)
    {
      if (loanDatamgr == null || loanDatamgr.LoanData.GetLogList() == null)
        return (LockRequestLog) null;
      List<LockRequestLog> list = ((IEnumerable<LockRequestLog>) loanDatamgr.LoanData.GetLogList().GetAllLockRequests(true)).Where<LockRequestLog>((Func<LockRequestLog, bool>) (log =>
      {
        if (log.Voided)
          return false;
        return log.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked) || log.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.OldLock) || log.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Cancelled) || log.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RequestDenied);
      })).OrderByDescending<LockRequestLog, DateTime>((Func<LockRequestLog, DateTime>) (log => log.Date)).ToList<LockRequestLog>();
      LockRequestLog lockRequestForVoid = list.Count<LockRequestLog>() > 0 ? list.First<LockRequestLog>() : (LockRequestLog) null;
      if (lockRequestForVoid == null)
      {
        lockRequestForVoid = LockUtils.GetAssignToTradePostConfirmationLock(loanDatamgr);
        if (lockRequestForVoid != null && lockRequestForVoid.Voided)
          lockRequestForVoid = (LockRequestLog) null;
      }
      else if (lockRequestForVoid.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
      {
        DateTime t1 = lockRequestForVoid.IsLockExtension ? lockRequestForVoid.BuySideNewLockExtensionDate : lockRequestForVoid.BuySideExpirationDate;
        if (t1 != DateTime.MinValue && DateTime.Compare(t1, DateTime.Today) < 0)
          lockRequestForVoid = (LockRequestLog) null;
      }
      return lockRequestForVoid;
    }

    public static bool IsLockVoidable(LoanDataMgr loanDatamgr, LockRequestLog lockRequestLog = null)
    {
      LogList logList = loanDatamgr?.LoanData.GetLogList();
      if (logList == null || ((IEnumerable<LockRequestLog>) logList.GetAllLockRequests(true)).Where<LockRequestLog>((Func<LockRequestLog, bool>) (log =>
      {
        if (!(log.RequestedStatus == RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.Requested)))
          return false;
        return log.IsRelock || log.IsLockExtension || log.IsLockCancellation || log.ReLockSequenceNumberForInactiveLock > 0;
      })).Any<LockRequestLog>())
        return false;
      LockRequestLog lockRequestForVoid = LockUtils.GetLastLockRequestForVoid(loanDatamgr);
      return lockRequestForVoid != null && (lockRequestLog == null || lockRequestForVoid.Guid.Equals(lockRequestLog.Guid)) && (!string.Equals(loanDatamgr.LoanData.GetField("2626"), "Correspondent", StringComparison.InvariantCultureIgnoreCase) || string.IsNullOrEmpty(loanDatamgr.LoanData.GetField("3907")) && LockUtils.IsBestEffortsSelected(lockRequestForVoid.GetLockRequestSnapshot()));
    }

    public static bool IsLockCancellable(LoanDataMgr loanMgr, LockRequestLog lockRequestLog = null)
    {
      if (loanMgr.LoanData == null || loanMgr.LoanData.IsTemplate)
        return false;
      LockRequestLog lockRequestLog1 = loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr);
      return lockRequestLog1 != null && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-NoRequest") && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-Request") && (lockRequestLog == null || lockRequestLog1.Guid.Equals(lockRequestLog.Guid));
    }

    public static bool IsLockExtendable(LoanDataMgr loanMgr, LockRequestLog lockRequestLog = null)
    {
      if (loanMgr.LoanData == null || loanMgr.LoanData.IsTemplate)
        return false;
      LockRequestLog lockRequestLog1 = loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr);
      return lockRequestLog1 != null && (lockRequestLog == null || lockRequestLog1.Guid.Equals(lockRequestLog.Guid)) && (!lockRequestLog1.IsLockExtension ? lockRequestLog1.BuySideExpirationDate : lockRequestLog1.BuySideNewLockExtensionDate) >= DateTime.Today;
    }

    private static bool IsBestEffortsSelected(Hashtable snapshotData)
    {
      string[] strArray = new string[2]{ "4187", "3910" };
      foreach (string key in strArray)
      {
        if (snapshotData[(object) key] != null && snapshotData[(object) key].ToString().Equals("Best Efforts"))
          return true;
      }
      return false;
    }

    public static LockRequestLog GetAssignToTradePostConfirmationLock(LoanData loanData)
    {
      if (loanData == null)
        return (LockRequestLog) null;
      LockRequestLog currentLockRequest = loanData.GetLogList().GetCurrentLockRequest();
      if (currentLockRequest == null)
        return (LockRequestLog) null;
      if (currentLockRequest.RequestedStatus != RateLockEnum.GetRateLockStatusString(RateLockRequestStatus.RateLocked))
        return (LockRequestLog) null;
      return string.IsNullOrEmpty((string) currentLockRequest.GetLockRequestSnapshot()[(object) "3839"]) ? (LockRequestLog) null : currentLockRequest;
    }

    public static LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser,
      bool historyFromCurrentLock,
      LoanData loanData,
      DateTime serverTime)
    {
      return LockUtils.CreateRateLockRequest(requestingUser, historyFromCurrentLock, loanData, serverTime, (SessionObjects) null);
    }

    public static LockRequestLog CreateRateLockRequest(
      UserInfo requestingUser,
      bool historyFromCurrentLock,
      LoanData loanData,
      DateTime serverTime,
      SessionObjects sessionObjects,
      bool supressLockDeskHours = false,
      bool isPersistent = true,
      bool isRelock = false,
      RateLockAction rateLockAction = RateLockAction.UnKnown,
      bool enforceCountyLoanLimit = true)
    {
      OnrpCalcInfo onrpCalcInfo = (OnrpCalcInfo) null;
      LoanDataMgr loanDataMgr = (LoanDataMgr) null;
      if (sessionObjects != null)
        loanDataMgr = LoanDataMgr.GetLoanDataMgr(sessionObjects, loanData, true, enforceCountyLimit: enforceCountyLoanLimit);
      if (loanDataMgr.CheckProviderAndFlag(isRelock: false) && loanDataMgr.GetChannel().Contains("Retail"))
        supressLockDeskHours = true;
      if (loanDataMgr != null && !supressLockDeskHours && !loanDataMgr.AllowNewLockOutsideLDHours())
      {
        LockDeskHoursManager.ValidateLockRequestTimeThickClient((IClientSession) sessionObjects.Session, sessionObjects, loanDataMgr, new bool?(), out onrpCalcInfo, false, true);
        if (onrpCalcInfo != null & isPersistent)
          LockDeskHoursManager.PerformOnrpRegistrationThickClient(sessionObjects, loanDataMgr, onrpCalcInfo);
      }
      string field1 = loanData.GetField("2088");
      loanDataMgr.ValidateBestEffortDailyLimit(ref field1, loanData.GetField("2089"), loanData.GetField("3965"), loanData.FltVal("2965"), out bool _);
      if (isRelock)
        loanDataMgr.ValidateLockPeriodRestriction();
      LockRequestLog lockRequestLog = new LockRequestLog(loanData.GetLogList());
      lockRequestLog.Date = serverTime;
      lockRequestLog.LockRequestStatus = RateLockRequestStatus.Requested;
      lockRequestLog.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
      if (!string.IsNullOrEmpty(loanData.GetField("4789")))
        lockRequestLog.PriceConcessionIndicator = loanData.GetField("4789");
      if (!string.IsNullOrEmpty(loanData.GetField("4790")))
        lockRequestLog.LockExtensionIndicator = loanData.GetField("4790");
      if (!string.IsNullOrEmpty(loanData.GetField("4791")))
        lockRequestLog.PriceConcessionRequestStatus = loanData.GetField("4791");
      lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(loanData);
      Hashtable hashtable1 = loanData.PrepareLockRequestData();
      if (onrpCalcInfo != null && onrpCalcInfo.OnrpLockTime != null && onrpCalcInfo.OnrpLockDate != null)
      {
        hashtable1[(object) "4060"] = (object) onrpCalcInfo.OnrpLockTime;
        hashtable1[(object) "4069"] = (object) onrpCalcInfo.OnrpLockDate;
        hashtable1[(object) "4061"] = onrpCalcInfo.IsONRPEligible ? (object) "Y" : (object) "N";
        if (!hashtable1.ContainsKey((object) "4058"))
          hashtable1.Add((object) "4058", hashtable1[(object) "4060"]);
        else
          hashtable1[(object) "4058"] = hashtable1[(object) "4060"];
        if (!hashtable1.ContainsKey((object) "4059"))
          hashtable1.Add((object) "4059", hashtable1[(object) "4061"]);
        else
          hashtable1[(object) "4059"] = hashtable1[(object) "4061"];
        if (!hashtable1.Contains((object) "4070"))
          hashtable1.Add((object) "4070", hashtable1[(object) "4069"]);
        else
          hashtable1[(object) "4070"] = hashtable1[(object) "4069"];
        if (onrpCalcInfo.IsONRPEligible && hashtable1.Contains((object) "4069") && hashtable1.Contains((object) "4060"))
        {
          DateTime dateTime;
          if (hashtable1.Contains((object) "2089"))
          {
            Hashtable hashtable2 = hashtable1;
            dateTime = LockDeskHoursManager.GetLockDateForOnrp((IClientSession) sessionObjects.Session, sessionObjects, loanDataMgr, Utils.ParseDate((object) (hashtable1[(object) "4069"].ToString() + " " + hashtable1[(object) "4060"])));
            string shortDateString = dateTime.ToShortDateString();
            hashtable2[(object) "2089"] = (object) shortDateString;
          }
          else
          {
            Hashtable hashtable3 = hashtable1;
            dateTime = LockDeskHoursManager.GetLockDateForOnrp((IClientSession) sessionObjects.Session, sessionObjects, loanDataMgr, Utils.ParseDate((object) (hashtable1[(object) "4069"].ToString() + " " + hashtable1[(object) "4060"])));
            string shortDateString = dateTime.ToShortDateString();
            hashtable3.Add((object) "2089", (object) shortDateString);
          }
          if (hashtable1.Contains((object) "2090"))
          {
            LockRequestCalculator requestCalculator = new LockRequestCalculator(sessionObjects, loanData);
            if (hashtable1.Contains((object) "2091"))
            {
              Hashtable hashtable4 = hashtable1;
              dateTime = requestCalculator.CalculateLockExpirationDate(Utils.ParseDate(hashtable1[(object) "2089"]), Utils.ParseInt(hashtable1[(object) "2090"], 0));
              string str = dateTime.ToString("MM/dd/yyyy");
              hashtable4[(object) "2091"] = (object) str;
            }
            else
            {
              Hashtable hashtable5 = hashtable1;
              dateTime = requestCalculator.CalculateLockExpirationDate(Utils.ParseDate(hashtable1[(object) "2089"]), Utils.ParseInt(hashtable1[(object) "2090"], 0));
              string str = dateTime.ToString("MM/dd/yyyy");
              hashtable5.Add((object) "2091", (object) str);
            }
          }
        }
      }
      else
      {
        hashtable1[(object) "4060"] = hashtable1[(object) "4069"] = (object) "";
        hashtable1[(object) "4061"] = (object) "N";
        if (hashtable1.ContainsKey((object) "4058"))
          hashtable1[(object) "4058"] = (object) "";
        else
          hashtable1.Add((object) "4058", (object) "");
        if (hashtable1.ContainsKey((object) "4059"))
          hashtable1[(object) "4059"] = (object) "N";
        else
          hashtable1.Add((object) "4059", (object) "N");
        if (hashtable1.Contains((object) "4070"))
          hashtable1[(object) "4070"] = (object) "";
        else
          hashtable1.Add((object) "4070", (object) "");
      }
      if (historyFromCurrentLock && loanData.GetLogList().GetCurrentLockRequest() != null)
      {
        Hashtable lockRequestSnapshot = loanData.GetLogList().GetCurrentLockRequest().GetLockRequestSnapshot();
        if (lockRequestSnapshot.ContainsKey((object) "OPTIMAL.HISTORY"))
          hashtable1[(object) "OPTIMAL.HISTORY"] = (object) string.Concat(lockRequestSnapshot[(object) "OPTIMAL.HISTORY"]);
        lockRequestLog.ParentLockGuid = loanData.GetLogList().GetCurrentLockRequest().Guid;
      }
      string field2 = loanData.GetField("LOCKRATE.RATESTATUS");
      if ((field2 == "Cancelled" || field2 == "Expired") & isRelock)
      {
        lockRequestLog.IsRelock = true;
        if (rateLockAction != RateLockAction.WcpcCurrent && rateLockAction != RateLockAction.WcpcHistorical)
        {
          if (sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RELOCKFEEALLOWED"] != null && (bool) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RELOCKFEEALLOWED"])
          {
            hashtable1[(object) "4256"] = (object) "Re-Lock #1";
            hashtable1[(object) "4257"] = (object) LockUtils.GetRelockFee(sessionObjects, loanData);
          }
          lockRequestLog.RateLockAction = RateLockAction.ReLockInactiveCurrentPricing;
          new LockRequestCalculator(sessionObjects, loanData).PerformLockRequestCalculations(hashtable1);
        }
        else
          lockRequestLog.RateLockAction = rateLockAction;
        int numberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(loanData);
        lockRequestLog.ReLockSequenceNumberForInactiveLock = numberForInactiveLock + 1;
        switch (field2)
        {
          case "Cancelled":
            lockRequestLog.ParentLockGuid = loanData.GetLogList().GetMostRecentLockCancellation().Guid;
            break;
          case "Expired":
            LockConfirmLog lockConfirmation = loanData.GetLogList().GetCurrentLockConfirmation();
            if (lockConfirmation != null)
            {
              lockRequestLog.ParentLockGuid = lockConfirmation.Guid;
              break;
            }
            break;
        }
      }
      else
        lockRequestLog.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(loanData);
      lockRequestLog.AddLockRequestSnapshot(hashtable1);
      foreach (LockRequestLog allLockRequest in loanData.GetLogList().GetAllLockRequests())
      {
        if (allLockRequest.LockRequestStatus == RateLockRequestStatus.Requested)
          allLockRequest.LockRequestStatus = RateLockRequestStatus.OldRequest;
      }
      loanData.GetLogList().AddRecord((LogRecordBase) lockRequestLog);
      if (sessionObjects.GetPolicySetting("NotAllowPricingChange"))
      {
        string field3 = loanData.GetField("3039");
        if (!string.IsNullOrWhiteSpace(field3) && field3 != "//" && Utils.IsDate((object) field3))
          loanData.SetFieldFromCal("3039", "");
      }
      loanData.TriggerCalculation("761", loanData.GetField("761"));
      return loanDataMgr != null && loanDataMgr.AllowAutoLock(false, isRelock, false, false, isRelock) ? LockUtils.PerformAutoLock(sessionObjects, lockRequestLog, loanDataMgr, isPersistent) : lockRequestLog;
    }

    public static bool isApplyReLockFee(
      SessionObjects sessionObjects,
      LoanData loanData,
      bool isRelock)
    {
      bool flag = sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RELOCKFEEALLOWED"] != null && (bool) sessionObjects.StartupInfo.PolicySettings[(object) "Policies.RELOCKFEEALLOWED"];
      if (!flag)
        return false;
      string a = Convert.ToString(sessionObjects.StartupInfo.PolicySettings[(object) "Policies.EPPS_EPC2_SHIP_DARK_SR"]);
      if (string.IsNullOrEmpty(a) || string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase))
        return flag;
      ProductPricingSetting productPricingPartner = sessionObjects.StartupInfo.ProductPricingPartner;
      if ((productPricingPartner == null || productPricingPartner != null && productPricingPartner.IsEPPS && productPricingPartner.VendorPlatform == VendorPlatform.EPC2) && LockUtils.GetWaiveRelockFeeState(sessionObjects, loanData, isRelock) == LockUtils.WaiveRelockFeeState.WaiveRelockFeeAfterDaysCap)
        flag = false;
      return flag;
    }

    public static bool IfShipDark(SessionObjects session, string shipDarkSetting = "EPPS_EPC2_SHIP_DARK_SR�")
    {
      bool flag = false;
      string a = Convert.ToString(session.StartupInfo.PolicySettings[(object) string.Format("Policies.{0}", (object) shipDarkSetting)]);
      if (string.IsNullOrEmpty(a) || string.Equals(a, "true", StringComparison.CurrentCultureIgnoreCase))
        flag = true;
      return flag;
    }

    public static LockUtils.WaiveRelockFeeState GetWaiveRelockFeeState(
      SessionObjects session,
      LoanData loanData,
      bool isRelock)
    {
      if (!isRelock || LockUtils.IsActiveRelock(loanData) || !(bool) session.ServerManager.GetServerSetting("Policies.WAIVEFEE"))
        return LockUtils.WaiveRelockFeeState.NotAvailable;
      DateTime inactiveDate = LockUtils.AddDaysToInactiveDate(session, loanData, "Policies.WAIVEFEEAFTERDAYS");
      return inactiveDate != DateTime.MinValue && DateTime.Today < inactiveDate ? LockUtils.WaiveRelockFeeState.WaiveRelockFeeWithinDaysCap : LockUtils.WaiveRelockFeeState.WaiveRelockFeeAfterDaysCap;
    }

    private static DateTime AddDaysToInactiveDate(
      SessionObjects session,
      LoanData loanData,
      string setting)
    {
      string field = loanData.GetField("LOCKRATE.RATESTATUS");
      DateTime inactiveDate = DateTime.MinValue;
      int num = Utils.ParseInt(session.ServerManager.GetServerSetting(setting));
      switch (field)
      {
        case "Cancelled":
          inactiveDate = loanData.GetLogList().GetMostRecentLockCancellation().DateTimeCancelled.AddDays((double) num);
          break;
        case "Expired":
          inactiveDate = Utils.ParseDate((object) loanData.GetField("762")).AddDays((double) num);
          break;
      }
      return inactiveDate;
    }

    public static LockRequestLog PerformAutoLock(
      SessionObjects sessionObjects,
      LockRequestLog lockLog,
      LoanDataMgr loanDataMgr,
      bool isPersistent = true,
      LoanDataMgr.LockLoanSyncOption syncOption = LoanDataMgr.LockLoanSyncOption.syncLockToLoan)
    {
      Hashtable lockRequestSnapshot = lockLog.GetLockRequestSnapshot();
      if (!LockUtils.IsPricingTxFromEPPS(lockRequestSnapshot))
        return lockLog;
      LockUtils.CopySnapshotRequestSideToBuySide(lockRequestSnapshot, sessionObjects.UserInfo.FullName, false);
      LockUtils.AutoLockLoadBuySidePriceAdjustments(loanDataMgr, lockRequestSnapshot);
      new LockRequestCalculator(sessionObjects, loanDataMgr.LoanData).PerformSnapshotCalculations(lockRequestSnapshot);
      loanDataMgr.SetNumFieldDecimal(lockRequestSnapshot);
      loanDataMgr.LockRateRequest(lockLog, lockRequestSnapshot, sessionObjects.UserInfo, true, isPersistent: isPersistent, syncOption: syncOption);
      LockUtils.AutoLockEpassLockAndConfirmUpdate(loanDataMgr, lockRequestSnapshot, sessionObjects.StartupInfo.ProductPricingPartner);
      return loanDataMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? lockLog;
    }

    public static LockRequestLog CreateExtendedRateLockRequest(
      SessionObjects sessionObjects,
      UserInfo requestingUser,
      bool historyFromCurrentLock,
      LoanData loanData,
      DateTime serverTime,
      int daysToExtend,
      DateTime newExpirationDate,
      Decimal priceAdjustment,
      string comment,
      Dictionary<string, string> customFields,
      bool enforceCountyLoanLimit = true)
    {
      bool flag = false;
      LockConfirmLog confirmLockLog = loanData.GetLogList().GetConfirmLockLog();
      LoanDataMgr loanDataMgr = (LoanDataMgr) null;
      if (confirmLockLog == null)
        return (LockRequestLog) null;
      LockRequestLog confirmedLockRequest = loanData.GetLogList().GetCurrentConfirmedLockRequest();
      LockRequestLog confirmationLock = LockUtils.GetAssignToTradePostConfirmationLock(loanData);
      if (confirmedLockRequest != null)
        flag = confirmedLockRequest.IsLockExtension;
      else if (confirmationLock != null)
        flag = confirmationLock.IsLockExtension;
      string requestGUID = confirmLockLog.RequestGUID;
      if (confirmationLock != null)
        requestGUID = confirmationLock.Guid;
      LockRequestLog extendedRateLockRequest = new LockRequestLog(loanData.GetLogList());
      extendedRateLockRequest.Date = serverTime;
      extendedRateLockRequest.LockRequestStatus = RateLockRequestStatus.Requested;
      extendedRateLockRequest.IsLockExtension = true;
      extendedRateLockRequest.SetRequestingUser(requestingUser.Userid, requestingUser.FullName);
      extendedRateLockRequest.ParentLockGuid = requestGUID;
      extendedRateLockRequest.ReLockSequenceNumberForInactiveLock = LockUtils.GetReLockSequenceNumberForInactiveLock(loanData);
      Hashtable lockRequestSnapshot1 = loanData.GetLogList().GetLockRequest(requestGUID).GetLockRequestSnapshot();
      if (customFields != null)
      {
        foreach (KeyValuePair<string, string> customField in customFields)
          LockUtils.setSnapshotField(lockRequestSnapshot1, (object) customField.Key, (object) customField.Value);
      }
      for (int index = 2148; index <= 2203; ++index)
      {
        if (index != 2161)
        {
          string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) index.ToString()));
          int num = index - 60;
          LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num.ToString(), (object) str);
        }
      }
      if (flag)
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "2091", LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3358"));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "2101", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3420")));
      for (int index = 2448; index <= 2481; ++index)
      {
        string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) index.ToString()));
        int num = index - 34;
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num.ToString(), (object) str);
      }
      for (int index = 2733; index <= 2775; ++index)
      {
        string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) index.ToString()));
        int num = index - 86;
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num.ToString(), (object) str);
      }
      for (int index = 3474; index <= 3493; ++index)
      {
        string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) index.ToString()));
        int num = index - 20;
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num.ToString(), (object) str);
      }
      for (int index = 0; index < 20; ++index)
      {
        int num1 = 4276 + index;
        int num2 = 4256 + index;
        string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) num1.ToString()));
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num2.ToString(), (object) str);
      }
      for (int index = 0; index < 20; ++index)
      {
        int num3 = 4356 + index;
        int num4 = 4336 + index;
        string str = string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) num3.ToString()));
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) num4.ToString(), (object) str);
      }
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "2848", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "2205")));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3254", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3256")));
      if (string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")) != "" && string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")) != "//")
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3369", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")));
      else if (string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "2151")) != "")
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3369", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "2151")));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3360", (object) daysToExtend);
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3361", (object) newExpirationDate.ToString("MM/dd/yyyy"));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3362", (object) priceAdjustment);
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "2144", (object) comment);
      if (string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3358")) == "" || string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3358")) == "//")
      {
        if (string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")) != "" && string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")) != "//")
          LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3358", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3364")));
        else if (string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "2151")) != "")
          LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3358", (object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "2151")));
      }
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3363", (object) daysToExtend);
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3364", (object) newExpirationDate.ToString("MM/dd/yyyy"));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3365", (object) priceAdjustment);
      int num5 = Utils.ParseInt((object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3433")), 0) + 1;
      if (num5 > 10)
        throw new Exception("Failed to create lock extension: Lock extension limit has been reached.");
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3433", (object) num5);
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) (3454 + 2 * (num5 - 1)).ToString(), (object) ("(" + (object) daysToExtend + (daysToExtend == 1 ? (object) " day" : (object) " days") + ")"));
      Hashtable snapshot1 = lockRequestSnapshot1;
      int num6 = 3454 + 2 * (num5 - 1) + 1;
      string key1 = num6.ToString();
      string str1 = priceAdjustment.ToString("N3");
      LockUtils.setSnapshotField(snapshot1, (object) key1, (object) str1);
      Hashtable snapshot2 = lockRequestSnapshot1;
      num6 = 3474 + 2 * (num5 - 1);
      string key2 = num6.ToString();
      string str2 = "(" + (object) daysToExtend + (daysToExtend == 1 ? (object) " day" : (object) " days") + ")";
      LockUtils.setSnapshotField(snapshot2, (object) key2, (object) str2);
      Hashtable snapshot3 = lockRequestSnapshot1;
      num6 = 3474 + 2 * (num5 - 1) + 1;
      string key3 = num6.ToString();
      string str3 = priceAdjustment.ToString("N3");
      LockUtils.setSnapshotField(snapshot3, (object) key3, (object) str3);
      if (Utils.ParseInt((object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3431")), 0) > 0)
      {
        int num7 = Utils.ParseInt((object) string.Concat(LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3431")), 0);
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3431", (object) (num7 + daysToExtend));
      }
      else
        LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "3431", (object) daysToExtend);
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "2151", LockUtils.getSnapshotField(lockRequestSnapshot1, (object) "3369"));
      LockUtils.setSnapshotField(lockRequestSnapshot1, (object) "4209", (object) LockUtils.GetRequestLockStatus(loanData));
      new LockRequestCalculator(sessionObjects, loanData).PerformSnapshotCalculations(lockRequestSnapshot1);
      extendedRateLockRequest.AddLockRequestSnapshot(lockRequestSnapshot1);
      foreach (LockRequestLog allLockRequest in loanData.GetLogList().GetAllLockRequests())
      {
        if (allLockRequest.LockRequestStatus == RateLockRequestStatus.Requested)
          allLockRequest.LockRequestStatus = RateLockRequestStatus.OldRequest;
      }
      loanData.GetLogList().AddRecord((LogRecordBase) extendedRateLockRequest);
      loanData.TriggerCalculation("761", loanData.GetField("761"));
      loanData.SetField("3360", "");
      loanData.SetField("3361", "");
      loanData.SetField("3362", "");
      loanData.SetField("3370", "");
      if (sessionObjects != null)
        loanDataMgr = LoanDataMgr.GetLoanDataMgr(sessionObjects, loanData, true, enforceCountyLimit: enforceCountyLoanLimit);
      if (loanDataMgr != null && loanDataMgr.AllowAutoLock(false, false, true, false, false))
      {
        Hashtable lockRequestSnapshot2 = extendedRateLockRequest.GetLockRequestSnapshot();
        new LockRequestCalculator(sessionObjects, loanData).PerformSnapshotCalculations(lockRequestSnapshot2);
        loanDataMgr.LockRateRequest(extendedRateLockRequest, lockRequestSnapshot2, sessionObjects.UserInfo, true);
      }
      return extendedRateLockRequest;
    }

    private static void setSnapshotField(Hashtable snapshot, object key, object value)
    {
      if (snapshot.Contains(key))
        snapshot[key] = value;
      else
        snapshot.Add(key, value);
    }

    private static object getSnapshotField(Hashtable snapshot, object key)
    {
      return snapshot.Contains(key) ? snapshot[key] : (object) null;
    }

    public static void CopySnapshotRequestSideToBuySide(
      Hashtable snapshot,
      string userFullName,
      bool isLockExtension)
    {
      for (int index = 2088; index <= 2143; ++index)
      {
        if (!isLockExtension || index != 2091)
        {
          string buySideGetField = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, index.ToString());
          int num = index + 60;
          LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num.ToString(), buySideGetField);
        }
      }
      if (isLockExtension)
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "2151", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3369"));
      for (int index = 2414; index <= 2447; ++index)
      {
        string buySideGetField = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, index.ToString());
        int num = index + 34;
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num.ToString(), buySideGetField);
      }
      for (int index = 2647; index <= 2689; ++index)
      {
        string buySideGetField = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, index.ToString());
        int num = index + 86;
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num.ToString(), buySideGetField);
      }
      for (int index = 3454; index <= 3473; ++index)
      {
        string buySideGetField = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, index.ToString());
        int num = index + 20;
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num.ToString(), buySideGetField);
      }
      for (int index = 0; index < 20; ++index)
      {
        int num1 = 4256 + index;
        int num2 = 4276 + index;
        string val = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, num1.ToString()) ?? "";
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num2.ToString(), val);
      }
      for (int index = 0; index < 20; ++index)
      {
        int num3 = 4336 + index;
        int num4 = 4356 + index;
        string val = LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, num3.ToString()) ?? "";
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, num4.ToString(), val);
      }
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "2029", userFullName);
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "2205", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "2848"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3256", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3254"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "2161", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "2101"));
      for (int index = 3380; index <= 3420; ++index)
        LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, index.ToString(), "");
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3363", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3360"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3364", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3361"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3365", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3362"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3848", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3847"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3873", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3872"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "3875", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "3874"));
      LockUtils.copySnapshotRequestSideToBuySide_setField(snapshot, "2205", LockUtils.copySnapshotRequestSideToBuySide_getField(snapshot, "4201"));
    }

    private static string copySnapshotRequestSideToBuySide_getField(Hashtable data, string id)
    {
      return data == null || !data.ContainsKey((object) id) ? "" : data[(object) id].ToString();
    }

    private static void copySnapshotRequestSideToBuySide_setField(
      Hashtable data,
      string id,
      string val)
    {
      if (data == null)
        return;
      if (!data.ContainsKey((object) id))
        data.Add((object) id, (object) val);
      else
        data[(object) id] = (object) val;
    }

    public static void RefreshLockRequestMapFieldsFromLoan(LoanDataMgr loanMgr)
    {
      if (loanMgr == null || loanMgr.LoanData == null)
        return;
      for (int index = 0; index < LockRequestLog.RequestFieldMap.Count; ++index)
      {
        LoanData loanData1 = loanMgr.LoanData;
        KeyValuePair<string, string> requestField = LockRequestLog.RequestFieldMap[index];
        string key = requestField.Key;
        LoanData loanData2 = loanMgr.LoanData;
        requestField = LockRequestLog.RequestFieldMap[index];
        string id = requestField.Value;
        string field = loanData2.GetField(id);
        loanData1.SetField(key, field);
      }
    }

    public static void SyncAdditionalLockFieldsToRequest(LoanDataMgr loanMgr)
    {
      if (loanMgr == null || loanMgr.LoanData == null || loanMgr.LoanData.Settings == null || loanMgr.LoanData.Settings.FieldSettings == null || loanMgr.LoanData.Settings.FieldSettings.LockRequestAdditionalFields == null)
        return;
      string[] fields = loanMgr.LoanData.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      for (int index = 0; index < fields.Length; ++index)
        loanMgr.LoanData.SetField(LockRequestCustomField.GenerateCustomFieldID(fields[index]), loanMgr.LoanData.GetField(fields[index]));
    }

    public static bool IsPricingTxFromEPPS(Hashtable rateLockSnapshotData)
    {
      return rateLockSnapshotData.Contains((object) "OPTIMAL.HISTORY") && !string.IsNullOrEmpty(rateLockSnapshotData[(object) "OPTIMAL.HISTORY"].ToString()) && rateLockSnapshotData[(object) "OPTIMAL.HISTORY"].ToString().Contains("<MPS_envelope");
    }

    public static void AutoLockLoadBuySidePriceAdjustments(LoanDataMgr loanMgr, Hashtable snapshot)
    {
      if (!snapshot.Contains((object) "OPTIMAL.HISTORY"))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.XmlResolver = (XmlResolver) null;
      try
      {
        xmlDocument.LoadXml((string) snapshot[(object) "OPTIMAL.HISTORY"]);
      }
      catch (Exception ex)
      {
        return;
      }
      List<KeyValuePair<string, string>> keyValuePairList1 = new List<KeyValuePair<string, string>>();
      List<KeyValuePair<string, string>> keyValuePairList2 = new List<KeyValuePair<string, string>>();
      Decimal num1 = 0M;
      foreach (XmlNode selectNode in xmlDocument.DocumentElement.SelectNodes("/RespDoc/LoBuyResp/MPS_envelope/body/MPSExportData/LienData/ElevatedAdjustmentList/ElevatedAdjustment"))
      {
        if (selectNode.Attributes["Type"] != null)
        {
          string str = selectNode.Attributes["Type"].Value;
          XmlNode xmlNode1 = selectNode.SelectSingleNode("Description");
          if (xmlNode1 != null)
          {
            string innerText1 = xmlNode1.InnerText;
            XmlNode xmlNode2 = selectNode.SelectSingleNode("Price");
            if (xmlNode2 != null)
            {
              string innerText2 = xmlNode2.InnerText;
              switch (str.ToLower())
              {
                case "base":
                  snapshot[(object) "2161"] = (object) innerText2;
                  continue;
                case "final":
                  continue;
                case "profit":
                  keyValuePairList1.Add(new KeyValuePair<string, string>(innerText1, innerText2));
                  continue;
                case "srp":
                  num1 += Utils.ParseDecimal((object) innerText2, 0M);
                  continue;
                default:
                  keyValuePairList2.Add(new KeyValuePair<string, string>(innerText1, innerText2));
                  continue;
              }
            }
          }
        }
      }
      if (num1 != 0M)
      {
        if (loanMgr.LoanData.GetField("2626") == "Correspondent")
        {
          snapshot[(object) "2205"] = (object) num1.ToString();
        }
        else
        {
          Decimal num2 = Utils.ParseDecimal(snapshot[(object) "2161"], 0M);
          snapshot[(object) "2161"] = (object) (num2 + num1).ToString();
        }
      }
      int num3 = 3380;
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList1)
      {
        Hashtable hashtable1 = snapshot;
        int num4 = num3;
        int num5 = num4 + 1;
        int num6 = num4;
        string key1 = num6.ToString();
        string key2 = keyValuePair.Key;
        hashtable1[(object) key1] = (object) key2;
        Hashtable hashtable2 = snapshot;
        int num7 = num5;
        num3 = num7 + 1;
        num6 = num7;
        string key3 = num6.ToString();
        string str = keyValuePair.Value;
        hashtable2[(object) key3] = (object) str;
      }
      for (int index = 2162; index <= 2201; ++index)
        snapshot[(object) index.ToString()] = (object) "";
      int num8 = 2162;
      foreach (KeyValuePair<string, string> keyValuePair in keyValuePairList2)
      {
        Hashtable hashtable3 = snapshot;
        int num9 = num8;
        int num10 = num9 + 1;
        int num11 = num9;
        string key4 = num11.ToString();
        string key5 = keyValuePair.Key;
        hashtable3[(object) key4] = (object) key5;
        Hashtable hashtable4 = snapshot;
        int num12 = num10;
        num8 = num12 + 1;
        num11 = num12;
        string key6 = num11.ToString();
        string str = keyValuePair.Value;
        hashtable4[(object) key6] = (object) str;
      }
    }

    public static void AutoLockEpassLockAndConfirmUpdate(
      LoanDataMgr loanMgr,
      Hashtable snapshot,
      ProductPricingSetting pricingSetting)
    {
      if (loanMgr == null || snapshot == null || pricingSetting == null || !snapshot.Contains((object) "OPTIMAL.HISTORY"))
        return;
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.XmlResolver = (XmlResolver) null;
      try
      {
        xmlDocument.LoadXml((string) snapshot[(object) "OPTIMAL.HISTORY"]);
      }
      catch (Exception ex)
      {
        return;
      }
      XmlNode xmlNode1 = xmlDocument.DocumentElement.SelectSingleNode("LoBuyResp/MPS_envelope/body");
      if (xmlNode1 == null)
        return;
      XmlNode xmlNode2 = xmlNode1.SelectSingleNode("MPSExportData/LienData/UserLender");
      string innerText1 = xmlNode2 == null ? "" : xmlNode2.InnerText;
      XmlNode xmlNode3 = xmlNode1.SelectSingleNode("MPSExportData/LienData/Relationship");
      string innerText2 = xmlNode3 == null ? "" : xmlNode3.InnerText;
      if (pricingSetting.UseCustomizedInvestorName)
      {
        bool onlyInvestorName = pricingSetting.UseOnlyInvestorName;
        bool useOnlyLenderName = pricingSetting.UseOnlyLenderName;
        bool investorAndLenderName = pricingSetting.UseInvestorAndLenderName;
        if (onlyInvestorName)
        {
          if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("VEND.X263"))
            loanMgr.LoanData.SetField("VEND.X263", innerText1);
        }
        else if (useOnlyLenderName)
        {
          if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("1264"))
            loanMgr.LoanData.SetField("1264", innerText1);
        }
        else if (string.IsNullOrEmpty(innerText2))
        {
          if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("VEND.X263"))
            loanMgr.LoanData.SetField("VEND.X263", innerText1);
        }
        else if (innerText2.Trim().ToLower() == "correspondent")
        {
          if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("VEND.X263"))
            loanMgr.LoanData.SetField("VEND.X263", innerText1);
        }
        else if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("1264"))
          loanMgr.LoanData.SetField("1264", innerText1);
      }
      else if (innerText1 != "" && innerText1 != loanMgr.LoanData.GetField("VEND.X263"))
        loanMgr.LoanData.SetField("VEND.X263", innerText1);
      XmlNode xmlNode4 = xmlNode1.SelectSingleNode("MPSExportData/LienData/QualRate");
      string innerText3 = xmlNode4 == null ? "" : xmlNode4.InnerText;
      if (innerText3 != "" && innerText3 != loanMgr.LoanData.GetField("1014"))
        loanMgr.LoanData.SetField("1014", innerText3);
      XmlNode xmlNode5 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMIndex");
      string innerText4 = xmlNode5 == null ? "" : xmlNode5.InnerText;
      if (innerText4 != "")
      {
        string val;
        switch (innerText4)
        {
          case "1 Month LIBOR":
            val = "LIBOR1M";
            break;
          case "1 Year LIBOR":
            val = "LIBOR12M";
            break;
          case "11th District Cost of Funds (COFI)":
            val = "FHLBSFCOFI";
            break;
          case "12 Month Treasury Average":
            val = "MTA";
            break;
          case "6 Month CD":
            val = "6MCDW";
            break;
          case "6 Month LIBOR":
            val = "LIBOR6M";
            break;
          case "Constant Maturity Treasury":
            val = "UST1YW";
            break;
          case "One Year Treasury Bill":
            val = "UST1YW";
            break;
          case "Prime":
            val = "WSJPrime";
            break;
          default:
            val = "";
            break;
        }
        if (val != "" && val != loanMgr.LoanData.GetField("1959"))
          loanMgr.LoanData.SetField("1959", val);
      }
      XmlNode xmlNode6 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMIndexValue");
      string innerText5 = xmlNode6 == null ? "" : xmlNode6.InnerText;
      if (innerText5 != "" && Utils.ToDouble(innerText5) != Utils.ToDouble(loanMgr.LoanData.GetField("688")))
        loanMgr.LoanData.SetField("688", innerText5);
      XmlNode xmlNode7 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMMargin");
      string innerText6 = xmlNode7 == null ? "" : xmlNode7.InnerText;
      if (innerText6 != "" && Utils.ToDouble(innerText6) != Utils.ToDouble(loanMgr.LoanData.GetField("689")))
      {
        loanMgr.LoanData.SetField("689", innerText6);
        loanMgr.LoanData.SetField("KBYO.XD689", Utils.RemoveEndingZeros(innerText6));
      }
      XmlNode xmlNode8 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMFixedTerm");
      string innerText7 = xmlNode8 == null ? "" : xmlNode8.InnerText;
      if (innerText7 != "" && innerText7 != loanMgr.LoanData.GetField("696"))
        loanMgr.LoanData.SetField("696", innerText7);
      XmlNode xmlNode9 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMAdjPeriod");
      string innerText8 = xmlNode9 == null ? "" : xmlNode9.InnerText;
      if (innerText8 != "" && innerText8 != loanMgr.LoanData.GetField("694"))
        loanMgr.LoanData.SetField("694", innerText8);
      XmlNode xmlNode10 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMLifetimeCap");
      string innerText9 = xmlNode10 == null ? "" : xmlNode10.InnerText;
      if (innerText9 != "" && Utils.ToDouble(innerText9) != Utils.ToDouble(loanMgr.LoanData.GetField("247")))
        loanMgr.LoanData.SetField("247", innerText9);
      XmlNode xmlNode11 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMFirstCap");
      string innerText10 = xmlNode11 == null ? "" : xmlNode11.InnerText;
      if (innerText10 != "" && Utils.ToDouble(innerText10) != Utils.ToDouble(loanMgr.LoanData.GetField("697")))
      {
        loanMgr.LoanData.SetField("697", innerText10);
        loanMgr.LoanData.SetField("KBYO.XD697", Utils.RemoveEndingZeros(innerText10));
      }
      XmlNode xmlNode12 = xmlNode1.SelectSingleNode("MPSExportData/LienData/ARMPeriodicCap");
      string innerText11 = xmlNode12 == null ? "" : xmlNode12.InnerText;
      if (!(innerText11 != "") || !(Convert.ToDecimal(innerText11).ToString("0.000") != loanMgr.LoanData.GetField("695")))
        return;
      loanMgr.LoanData.SetField("695", innerText11);
      loanMgr.LoanData.SetField("KBYO.XD695", Utils.RemoveEndingZeros(innerText11));
    }

    public string ValidateDaysToExtend(
      LoanData loanData,
      int extLockDays,
      int oriLockDays,
      Hashtable snapshot,
      int original3363Value)
    {
      string empty = string.Empty;
      IDictionary serverSettings = this.sessionObjects.ServerManager.GetServerSettings("Policies");
      int extCumulatedDays = loanData.GetLogList().GetCurrentExtCumulatedDays(snapshot);
      int currentExtNumber = loanData.GetLogList().GetCurrentExtNumber(snapshot);
      if (snapshot != null)
      {
        if (original3363Value > 0)
          extCumulatedDays -= original3363Value;
        else
          extCumulatedDays -= extLockDays;
      }
      else
        ++currentExtNumber;
      if (!Utils.ParseBoolean(serverSettings[(object) "POLICIES.EnableLockExtension"]))
        return empty;
      if (Utils.ParseBoolean(serverSettings[(object) "Policies.LockExtensionAllowTotalCap"]))
      {
        int num = Utils.ParseInt(serverSettings[(object) "Policies.LockExtensionAllowTotalCapDays"], 0);
        if (oriLockDays + extLockDays + extCumulatedDays > num)
          return "This loan has reached the maximum number of total lock days.";
      }
      if ((int) serverSettings[(object) "Policies.LockExtensionCompanyControlled"] == 2)
      {
        int extensionDays = new LockExtensionUtils(this.sessionObjects, loanData).GetExtensionDays(currentExtNumber);
        if (extLockDays != extensionDays)
          return "This loan is not matched with the number of days allowed to be extended.";
      }
      else
      {
        if (Utils.ParseInt(serverSettings[(object) "Policies.LOCKEXTENSION_CAP_TYPE"], 0) == 0)
          return empty;
        if (Utils.ParseInt(serverSettings[(object) "Policies.LOCKEXTENSION_CAP_TYPE"], 0) == 1 && extCumulatedDays + extLockDays > oriLockDays || Utils.ParseInt(serverSettings[(object) "Policies.LOCKEXTENSION_CAP_TYPE"], 0) == 2 && extCumulatedDays + extLockDays > Utils.ParseInt(serverSettings[(object) "Policies.LOCKEXTENSION_CAP_DAYS"], 0))
          return "This loan has reached the maximum number of days allowed to be extended.";
        if (Utils.ParseInt(serverSettings[(object) "Policies.LockExtensionCompanyControlled"], 0) != 2 && Utils.ParseBoolean(serverSettings[(object) "Policies.LockExtAllowTotalTimesCapEnabled"]) && currentExtNumber > Utils.ParseInt(serverSettings[(object) "Policies.LockExtAllowTotalTimesCap"]))
          return "This loan has reached the maximum number of times allowed to be extended.";
      }
      return empty;
    }

    public static string GetQualifiedAsOfDateFromEppsTxHistory(string transactionHistory)
    {
      if (string.IsNullOrEmpty(transactionHistory))
        return "";
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.XmlResolver = (XmlResolver) null;
        xmlDocument.LoadXml(transactionHistory);
        XmlNode xmlNode = xmlDocument.SelectSingleNode("RespDoc/LoBuyResp/MPS_envelope/body/MPSExportData/LienData/QualifiedAsOf");
        return xmlNode == null ? "" : xmlNode.InnerText;
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    public static Decimal GetSRPFromEppsTxHistory(string transactionHistory, bool isLoBuyResp)
    {
      if (string.IsNullOrEmpty(transactionHistory))
        return 0M;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.XmlResolver = (XmlResolver) null;
        xmlDocument.LoadXml(transactionHistory);
        Decimal fromEppsTxHistory = 0M;
        foreach (XmlNode xmlNode1 in isLoBuyResp ? xmlDocument.DocumentElement.SelectNodes("/RespDoc/LoBuyResp/MPS_envelope/body/MPSExportData/LienData/ElevatedAdjustmentList/ElevatedAdjustment") : xmlDocument.DocumentElement.SelectNodes("/RespDoc/SecBuyResp/MPS_envelope/body/MPSExportData/LienData/ElevatedAdjustmentList/ElevatedAdjustment"))
        {
          if (xmlNode1.Attributes["Type"] != null)
          {
            string str = xmlNode1.Attributes["Type"].Value;
            XmlNode xmlNode2 = xmlNode1.SelectSingleNode("Price");
            if (str.ToLower() == "srp" && xmlNode2 != null)
              fromEppsTxHistory += Utils.ParseDecimal((object) xmlNode2.InnerText, 0M);
          }
        }
        return fromEppsTxHistory;
      }
      catch (Exception ex)
      {
        return 0M;
      }
    }

    public static bool IsValidateRelockTrans(string transactionHistory)
    {
      if (string.IsNullOrEmpty(transactionHistory))
        return false;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.XmlResolver = (XmlResolver) null;
        xmlDocument.LoadXml(transactionHistory);
        return xmlDocument.SelectSingleNode("RespDoc/LoBuyResp/MPS_envelope").Attributes["action"].Value == "EllieMaeValidateRelock";
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static bool IsWorstCaseHistoricalPricingTrans(string transactionHistory)
    {
      if (string.IsNullOrEmpty(transactionHistory))
        return false;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.XmlResolver = (XmlResolver) null;
        xmlDocument.LoadXml(transactionHistory);
        return xmlDocument.SelectSingleNode("RespDoc/LoBuyResp/MPS_envelope").Attributes["action"].Value == "WorstCaseHistorical";
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    public static LockRequestLog CancelRateLockRequest(
      string lockId,
      UserInfo cancellingUser,
      LoanData loanData,
      SessionObjects sessionObjects,
      DateTime cancellationDate,
      string comment,
      bool enforceCountyLoanLimit = true)
    {
      LoanDataMgr loanMgr = (LoanDataMgr) null;
      if (sessionObjects != null)
        loanMgr = LoanDataMgr.GetLoanDataMgr(sessionObjects, loanData, true, enforceCountyLimit: enforceCountyLoanLimit);
      if (loanMgr == null || !loanMgr.AllowAutoLock(false, false, false, true, false))
        return loanMgr.CreateRateLockCancellationRequest(cancellingUser, cancellationDate, comment);
      if (!LockUtils.IsConfirmedLockExists(loanMgr, lockId))
        throw new ApplicationException(string.Format("Can't find a confirmed lock with Id {0}.", (object) lockId));
      if (!LockUtils.IsLockConfirmedNotExpired(loanMgr))
        throw new ApplicationException("Lock can't be cancelled. A lock can be cancelled if and only if it has been confirmed and has not expired");
      string field = loanData.GetField("3907");
      if (!string.IsNullOrEmpty(field))
        throw new ApplicationException(string.Format("This loan \"{0}\" must be removed from the Correspondent Trade \"{1}\" before the lock can be cancelled.", (object) loanData.LoanNumber, (object) field));
      return loanMgr.CancelRateLockWithoutRequest(cancellingUser, cancellationDate, comment, true);
    }

    public static bool IsConfirmedLockExists(LoanDataMgr loanMgr, string lockId)
    {
      foreach (LockConfirmLog lockConfirmLog in (LockConfirmLog[]) loanMgr.LoanData.GetLogList().GetAllRecordsOfType(typeof (LockConfirmLog)))
      {
        if (lockConfirmLog.RequestGUID.Equals(lockId))
          return true;
      }
      return false;
    }

    public static bool IsLockConfirmedNotExpired(LoanDataMgr loanMgr)
    {
      bool flag = false;
      if (loanMgr.LoanData == null || loanMgr.LoanData.IsTemplate)
        return flag;
      return (loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr)) != null && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-NoRequest") && !(loanMgr.LoanData.GetField("LOCKRATE.RATEREQUESTSTATUS") == "Expired-Request");
    }

    public static bool ValidateLockExtension(
      SessionObjects sessionObjects,
      LoanDataMgr loanMgr,
      int daysToExtend,
      Decimal priceAdjustment)
    {
      if (!Utils.ParseBoolean(sessionObjects.ServerManager.GetServerSettings("Policies")[(object) "POLICIES.EnableLockExtension"]))
        throw new Exception("Lock extension request is not enabled.");
      if (daysToExtend < 1 || daysToExtend > 999)
        throw new Exception("Data out of range: 1 - 999.");
      bool flag = false;
      if (loanMgr.LoanData != null && !loanMgr.LoanData.IsTemplate)
      {
        LockRequestLog lockRequestLog = loanMgr.LoanData.GetLogList().GetCurrentConfirmedLockRequest() ?? LockUtils.GetAssignToTradePostConfirmationLock(loanMgr);
        if (lockRequestLog != null && (!lockRequestLog.IsLockExtension ? lockRequestLog.BuySideExpirationDate : lockRequestLog.BuySideNewLockExtensionDate) >= DateTime.Today)
          flag = true;
        if (!flag)
          throw new Exception("The lock must be confirmed before it can be extended.");
        Hashtable lockRequestSnapshot = lockRequestLog.GetLockRequestSnapshot();
        int oriLockDays = Utils.ParseInt(lockRequestSnapshot[(object) "2150"], 0);
        int extLockDays = Utils.ParseInt(lockRequestSnapshot[(object) "3363"], 0);
        string extend = new LockUtils(sessionObjects).ValidateDaysToExtend(loanMgr.LoanData, extLockDays, oriLockDays, (Hashtable) null, 0);
        if (extend != string.Empty)
          throw new Exception(extend);
      }
      return flag;
    }

    public static string GetRequestLockStatus(LoanData loanData)
    {
      LockConfirmLog confirmForCurrentLock = loanData.GetLogList().GetMostRecentConfirmForCurrentLock();
      bool flag = false;
      if (confirmForCurrentLock != null)
        flag = confirmForCurrentLock.CommitmentTermEnabled;
      DateTime dateTime = string.Equals(loanData.GetField("2626"), "correspondent", StringComparison.InvariantCultureIgnoreCase) & flag ? Utils.ParseDate((object) loanData.GetField("4529")) : Utils.ParseDate((object) loanData.GetField("762"));
      string field1 = loanData.GetField("LOCKRATE.RATESTATUS");
      string field2 = loanData.GetField("LOCKRATE.REQUESTED");
      loanData.GetField("LOCKRATE.RATEREQUESTSTATUS");
      if (field1 == "Cancelled")
        return "Cancelled Lock";
      if (field1 != "NotLocked")
      {
        int totalDays = (int) (dateTime - DateTime.Today).TotalDays;
        return dateTime == DateTime.MinValue || totalDays >= 0 ? "Active Lock" : "Expired Lock";
      }
      int num = field2 == "Y" ? 1 : 0;
      return "Not Locked";
    }

    public static LockUtils.RelockState GetRelockState(SessionObjects session, LoanData loan)
    {
      if (!LockUtils.IsRelock(loan.GetField("3841")) || LockUtils.IsActiveRelock(loan) || !(bool) session.ServerManager.GetServerSetting("Policies.GetCurrentPricing"))
        return LockUtils.RelockState.NotAvailable;
      DateTime inactiveDate = LockUtils.AddDaysToInactiveDate(session, loan, "Policies.LockExpDays");
      return inactiveDate != DateTime.MinValue && inactiveDate < DateTime.Today || session.StartupInfo.ProductPricingPartner != null && session.StartupInfo.ProductPricingPartner.IsEPPS && session.StartupInfo.ProductPricingPartner.VendorPlatform == VendorPlatform.EPC2 && Utils.ParseBoolean(session.ServerManager.GetServerSetting("Policies.WORSTCASEPRICE")) ? LockUtils.RelockState.RelockExpiredExceedDaysCap : LockUtils.RelockState.RelockExpiredWithinDaysCap;
    }

    public static LockUtils.WorstCasePriceState GetWcpStateForRelock(
      SessionObjects session,
      LoanData loanData,
      bool isRelock)
    {
      loanData.GetField("LOCKRATE.RATESTATUS");
      if (!isRelock || LockUtils.IsActiveRelock(loanData) || !(bool) session.ServerManager.GetServerSetting("Policies.WORSTCASEPRICE"))
        return LockUtils.WorstCasePriceState.NotAvailable;
      DateTime inactiveDate = LockUtils.AddDaysToInactiveDate(session, loanData, "Policies.WORSTCASEPRICEEQUALDAYS");
      return inactiveDate != DateTime.MinValue && DateTime.Today <= inactiveDate ? LockUtils.WorstCasePriceState.WorstCasePriceWithinDaysCap : LockUtils.WorstCasePriceState.WorstCasePriceAfterDaysCap;
    }

    public static bool IsActiveRelock(LoanData loanData)
    {
      string field = loanData.GetField("LOCKRATE.RATESTATUS");
      return field != "Cancelled" && field != "Expired";
    }

    public static string GetRelockFee(SessionObjects session, LoanData loanData)
    {
      return loanData != null && (loanData.GetField("2626") == "Banked - Retail" && (bool) session.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"] || loanData.GetField("2626") == "Banked - Wholesale" && (bool) session.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]) ? Decimal.Negate((Decimal) session.ServerManager.GetServerSetting("Policies.RelockFee")).ToString() : session.ServerManager.GetServerSetting("Policies.RelockFee").ToString();
    }

    public static int GetReLockSequenceNumberForInactiveLock(LoanData loanData)
    {
      int numberForInactiveLock = 0;
      LockConfirmLog recentConfirmedLock = loanData.GetLogList().GetMostRecentConfirmedLock();
      if (recentConfirmedLock != null)
        numberForInactiveLock = loanData.GetLogList().GetLockRequest(recentConfirmedLock.RequestGUID).ReLockSequenceNumberForInactiveLock;
      return numberForInactiveLock;
    }

    public static bool IsInactiveReLockExceededAllowed(SessionObjects session, LoanData loan)
    {
      int num1 = 10;
      if (session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCap"] != null && (bool) session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCap"])
      {
        int num2 = Utils.ParseInt(session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCapTimes"] != null ? (object) session.StartupInfo.PolicySettings[(object) "Policies.RelockAllowTotalCapTimes"].ToString() : (object) "");
        num1 = num2 < num1 ? num2 : num1;
      }
      return LockUtils.GetReLockSequenceNumberForInactiveLock(loan) != num1;
    }

    public static void AddReLockFee(
      SessionObjects sessionObject,
      Hashtable dataTable,
      LoanData loanData)
    {
      int? availableReLockFieldId = LockUtils.GetNextAvailableReLockFieldId(sessionObject, dataTable);
      if (!availableReLockFieldId.HasValue)
        return;
      int num1 = availableReLockFieldId.Value;
      int num2 = (num1 - 4256) / 2 + 1;
      dataTable[(object) num1.ToString()] = (object) ("Re-Lock #" + num2.ToString());
      dataTable[(object) (num1 + 1).ToString()] = (object) LockUtils.GetRelockFee(sessionObject, loanData);
    }

    public static int? GetNextAvailableReLockFieldId(
      SessionObjects sessionObject,
      Hashtable dataTable)
    {
      int? availableReLockFieldId1 = new int?(4256);
      int? availableReLockFieldId2;
      while (true)
      {
        availableReLockFieldId2 = availableReLockFieldId1;
        int num = 4275;
        if (availableReLockFieldId2.GetValueOrDefault() <= num & availableReLockFieldId2.HasValue)
        {
          object obj1 = dataTable[(object) availableReLockFieldId1.ToString()];
          Hashtable hashtable = dataTable;
          availableReLockFieldId2 = availableReLockFieldId1;
          string key = (availableReLockFieldId2.HasValue ? new int?(availableReLockFieldId2.GetValueOrDefault() + 1) : new int?()).ToString();
          object obj2 = hashtable[(object) key];
          if (obj1 != null && !string.IsNullOrEmpty(obj1.ToString()) || obj2 != null && !string.IsNullOrEmpty(obj2.ToString()))
          {
            availableReLockFieldId2 = availableReLockFieldId1;
            availableReLockFieldId2 = availableReLockFieldId2.HasValue ? new int?(availableReLockFieldId2.GetValueOrDefault() + 1) : new int?();
            availableReLockFieldId1 = availableReLockFieldId2.HasValue ? new int?(availableReLockFieldId2.GetValueOrDefault() + 1) : new int?();
          }
          else
            break;
        }
        else
          break;
      }
      availableReLockFieldId2 = availableReLockFieldId1;
      int num1 = 4275;
      if (!(availableReLockFieldId2.GetValueOrDefault() > num1 & availableReLockFieldId2.HasValue))
        return availableReLockFieldId1;
      availableReLockFieldId2 = new int?();
      return availableReLockFieldId2;
    }

    public static bool IsRelock(string field3841)
    {
      return field3841 == "ReLock" || field3841.Contains("Update");
    }

    public static string GetRequestType(string lockStatus)
    {
      return lockStatus == "Active Lock" ? "Lock Update" : "ReLock";
    }

    public static string GetRelockRequestType(bool isRelock, bool isCancelOrExpired)
    {
      return isCancelOrExpired ? "Re-Lock" : "Update";
    }

    public static void CopyLoanToLock(
      SessionObjects sessionObj,
      LoanData loanData,
      Hashtable lockSnapshot,
      bool excludeInterestRateOnCopy)
    {
      Hashtable diffTable = new Hashtable();
      bool parPricingSetting = LockUtils.GetZeroBasedParPricingSetting(sessionObj, loanData);
      for (int index = 0; index < LockRequestLog.RequestFieldMap.Count; ++index)
      {
        string loanId = LockRequestLog.RequestFieldMap[index].Value;
        string key = LockRequestLog.RequestFieldMap[index].Key;
        if (!(key == "4463") || lockSnapshot[(object) "4463"] != null && !string.IsNullOrEmpty((string) lockSnapshot[(object) "4463"]))
          LockUtils.CompareLoanLockValues(parPricingSetting, loanId, key, loanData, lockSnapshot, diffTable);
      }
      string[] fields = loanData.Settings.FieldSettings.LockRequestAdditionalFields.GetFields(true);
      if (fields != null)
      {
        for (int index = 0; index < fields.Length; ++index)
        {
          string customFieldId = LockRequestCustomField.GenerateCustomFieldID(fields[index]);
          LockUtils.CompareLoanLockValues(parPricingSetting, fields[index], customFieldId, loanData, lockSnapshot, diffTable);
        }
      }
      LockUtils.CompareLoanLockValues(parPricingSetting, "NEWHUD.X1720", "3873", loanData, lockSnapshot, diffTable);
      LockUtils.CompareLoanLockValues(parPricingSetting, "NEWHUD.X1721", "3875", loanData, lockSnapshot, diffTable);
      LockUtils.updateLockData(parPricingSetting, diffTable, loanData, lockSnapshot, excludeInterestRateOnCopy, sessionObj);
    }

    private static void CompareLoanLockValues(
      bool zeroBasedParPricing,
      string loanId,
      string lockId,
      LoanData loanData,
      Hashtable lockSnapshot,
      Hashtable diffTable)
    {
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      string strA = loanData.GetField(loanId);
      string strB = lockSnapshot.ContainsKey((object) lockId) ? lockSnapshot[(object) lockId].ToString() : string.Empty;
      FieldDefinition field = EncompassFields.GetField(loanId);
      if (field != null)
      {
        strA = field.FormatValue(strA);
        strB = field.FormatValue(strB);
      }
      if (string.Compare(lockId, "3875", true) == 0 && !string.IsNullOrWhiteSpace(strB) && !string.IsNullOrWhiteSpace(strA) && !zeroBasedParPricing)
      {
        if (Utils.ParseDecimal((object) strB) == 100M - Utils.ParseDecimal((object) strA))
          return;
      }
      else if (string.Compare(strA, strB, true) == 0)
        return;
      if (diffTable.ContainsKey((object) lockId))
        return;
      diffTable.Add((object) lockId, (object) strA);
    }

    public static bool GetZeroBasedParPricingSetting(SessionObjects sessionObj, LoanData loanData)
    {
      bool parPricingSetting = false;
      string field = loanData.GetField("2626");
      if (string.Equals(field, "Banked - Retail", StringComparison.InvariantCultureIgnoreCase))
        parPricingSetting = Utils.ParseBoolean(sessionObj.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingRetail"]);
      else if (string.Equals(field, "Banked - Wholesale", StringComparison.InvariantCultureIgnoreCase))
        parPricingSetting = Utils.ParseBoolean(sessionObj.StartupInfo.PolicySettings[(object) "Policies.EnableZeroParPricingWholesale"]);
      return parPricingSetting;
    }

    private static void updateLockData(
      bool zeroBasedParPricing,
      Hashtable diffTable,
      LoanData loanData,
      Hashtable lockSnapshot,
      bool excludeInterestRateOnCopy,
      SessionObjects sessionObjects)
    {
      foreach (DictionaryEntry d in diffTable)
      {
        string lockId = d.Key.ToString();
        if (!(lockId == "2092" & excludeInterestRateOnCopy))
        {
          if (lockId == "4632")
            LockUtils.setBuydownFields(d, loanData, lockSnapshot);
          LockUtils.setSnapshotField(lockId, d.Value.ToString(), lockSnapshot);
          if (lockId == "2092")
            new LockRequestCalculator(sessionObjects, loanData).CalcTotalBaseRateAdjAndNetBuyRate(lockSnapshot);
          if (lockId == "3875" && !string.IsNullOrWhiteSpace(d.Value.ToString()) && !zeroBasedParPricing)
          {
            Decimal num = Utils.ParseDecimal(d.Value, 0M);
            LockUtils.setSnapshotField(lockId, (100M - num).ToString(), lockSnapshot);
          }
          if (lockId.StartsWith("LR.", StringComparison.InvariantCultureIgnoreCase))
            LockUtils.setSnapshotField(lockId.Substring(3), d.Value.ToString(), lockSnapshot);
        }
      }
    }

    private static void setBuydownFields(
      DictionaryEntry d,
      LoanData loanData,
      Hashtable lockSnapshot)
    {
      LockUtils.setSnapshotField(d.Key.ToString(), d.Value.ToString(), lockSnapshot);
      if (lockSnapshot.ContainsKey((object) "4631") && lockSnapshot[(object) "4631"].ToString() == "Borrower")
      {
        int num1 = 4633;
        int num2 = 1269;
        while (num1 < 4639)
        {
          LockUtils.setSnapshotField(num1.ToString(), loanData.GetField(num2.ToString()), lockSnapshot);
          ++num1;
          ++num2;
        }
        int num3 = 4639;
        int num4 = 1613;
        while (num3 < 4645)
        {
          LockUtils.setSnapshotField(num3.ToString(), loanData.GetField(num4.ToString()), lockSnapshot);
          ++num3;
          ++num4;
        }
      }
      else
      {
        int num5 = 4633;
        int num6 = 4535;
        while (num5 < 4645)
        {
          LockUtils.setSnapshotField(num5.ToString(), loanData.GetField(num6.ToString()), lockSnapshot);
          ++num5;
          ++num6;
        }
      }
    }

    private static void setSnapshotField(string lockId, string value, Hashtable lockSnapshot)
    {
      if (!lockSnapshot.ContainsKey((object) lockId))
        lockSnapshot.Add((object) lockId, (object) value);
      else
        lockSnapshot[(object) lockId] = (object) value;
    }

    public enum WaiveRelockFeeState
    {
      NotAvailable,
      WaiveRelockFeeWithinDaysCap,
      WaiveRelockFeeAfterDaysCap,
    }

    public enum RelockState
    {
      NotAvailable,
      RelockExpiredWithinDaysCap,
      RelockExpiredExceedDaysCap,
    }

    public enum WorstCasePriceState
    {
      NotAvailable,
      WorstCasePriceWithinDaysCap,
      WorstCasePriceAfterDaysCap,
    }
  }
}
