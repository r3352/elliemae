// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.LockDeskHoursManager
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public static class LockDeskHoursManager
  {
    private const string className = "LockDeskHoursManager�";
    private static string sw = Tracing.SwOutsideLoan;
    private static LoanDataMgr loanDataManager = (LoanDataMgr) null;

    public static void ValidateLockRequestTimeThickClient(
      IClientSession clientSession,
      SessionObjects sessionObjs,
      LoanDataMgr loanDataMgr,
      bool? overrideOnrpCheck,
      out OnrpCalcInfo onrpCalcInfo,
      bool isRelock,
      bool fromPlatform = false)
    {
      LockDeskHoursManager.loanDataManager = loanDataMgr;
      EncompassLockDeskHoursHelper lockDeskHoursHelper = new EncompassLockDeskHoursHelper(clientSession, sessionObjs, loanDataMgr);
      string field1 = loanDataMgr.LoanData.GetField("4060");
      onrpCalcInfo = (OnrpCalcInfo) null;
      bool isActiveLock = loanDataMgr.LoanData.GetField("4209").Equals("Active Lock");
      if (!string.IsNullOrEmpty(field1))
      {
        onrpCalcInfo = new OnrpCalcInfo()
        {
          IsONRPEligible = true,
          OnrpLockDate = loanDataMgr.LoanData.GetField("4069"),
          OnrpLockTime = field1,
          IsManual = true
        };
      }
      else
      {
        DateTime serverEasternTime = lockDeskHoursHelper.GetServerEasternTime();
        BusinessCalendar expirationCalendar = LockDeskHoursUtils.GetLockExpirationCalendar(sessionObjs.StartupInfo, sessionObjs);
        bool serverSetting = (bool) sessionObjs.ServerManager.GetServerSetting("POLICIES.EnableRelockOutsideLockDeskHours");
        string field2 = loanDataMgr.LoanData.GetField("364");
        double loanAmount = Utils.ParseDouble((object) loanDataMgr.LoanData.GetField("2965"));
        LoanChannel loanChannel = lockDeskHoursHelper.GetLoanChannel();
        string field3 = loanDataMgr.LoanData.GetField("TPO.X15");
        string field4 = loanDataMgr.LoanData.GetField("LOCKRATE.RATESTATUS");
        bool flag = field4 == "Cancelled" || field4 == "Expired";
        LockDeskHoursManager.ValidatLockRequest(serverEasternTime, expirationCalendar, (ILockDeskHoursHelper) lockDeskHoursHelper, loanChannel, field3, field2, loanAmount, overrideOnrpCheck, out onrpCalcInfo, !flag & isRelock & serverSetting, isActiveLock, isRelock, fromPlatform);
      }
    }

    public static void ValidatLockRequest(
      DateTime requestDateTime,
      BusinessCalendar calendar,
      ILockDeskHoursHelper lockDeskHoursHelper,
      LoanChannel loanChannel,
      string tpoId,
      string loanNumber,
      double loanAmount,
      bool? overrideOnrpCheck,
      out OnrpCalcInfo onrpCalcInfo,
      bool allowRelockOutSideLDHours,
      bool isActiveLock,
      bool isRelock,
      bool fromPlatform = false)
    {
      if (lockDeskHoursHelper == null)
        throw new ArgumentNullException("lockDeskHoursHelper is required");
      onrpCalcInfo = (OnrpCalcInfo) null;
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Getting lock desk hours settings...");
      LockDeskHoursInfo deskHoursSettings = lockDeskHoursHelper.GetLockDeskHoursSettings(loanChannel);
      DateTime lockDeskEnd = LockDeskHoursManager.ValidateLockDeskSettings(deskHoursSettings, lockDeskHoursHelper, requestDateTime, calendar, loanChannel, allowRelockOutSideLDHours, isActiveLock, isRelock);
      if (loanChannel == LoanChannel.None && deskHoursSettings.LockDeskHoursMessage != null && deskHoursSettings.LockDeskHoursMessage.Trim() != string.Empty)
        loanChannel = LoanChannel.BankedRetail;
      if (!(lockDeskEnd != DateTime.MinValue))
        return;
      onrpCalcInfo = LockDeskHoursManager.validateOnrp(requestDateTime, deskHoursSettings, lockDeskEnd, calendar, lockDeskHoursHelper, loanChannel, tpoId, loanNumber, loanAmount, overrideOnrpCheck, fromPlatform);
      if (onrpCalcInfo == null)
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Lock desk closed");
        throw new LockDeskClosedException(deskHoursSettings.LockDeskHoursMessage);
      }
    }

    private static DateTime ValidateLockDeskSettings(
      LockDeskHoursInfo lockDeskHours,
      ILockDeskHoursHelper lockDeskHoursHelper,
      DateTime requestDateTime,
      BusinessCalendar calendar,
      LoanChannel loanChannel,
      bool allowRelockOutSideLDHours,
      bool isActiveLock,
      bool isRelock)
    {
      if (!lockDeskHours.IsEncompassLockDeskHoursEnabled)
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Lock desk hours disabled - lock desk always open");
        return DateTime.MinValue;
      }
      if (loanChannel == LoanChannel.None && lockDeskHours.LockDeskHoursMessage != null && lockDeskHours.LockDeskHoursMessage.Trim() != string.Empty)
        loanChannel = LoanChannel.BankedRetail;
      if (loanChannel != LoanChannel.BankedWholesale && loanChannel != LoanChannel.BankedRetail && loanChannel != LoanChannel.Correspondent)
        throw new LockDeskClosedException("Lock Desk Hours are controlled by Loan Channel, however, no channel has been selected for this loan. Please select the Loan Channel prior to Lock Request submission.");
      if (lockDeskHours.IsLockDeskShutdown && !(lockDeskHours.AllowActiveRelockRequests & isActiveLock & isRelock))
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Lock desk is shutdown");
        throw new LockDeskClosedException(lockDeskHours.LockDeskShutdownMessage);
      }
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Calling GetMostRecentLockDeskStartTime()");
      DateTime lockDeskStartTime = LockDeskHoursManager.GetMostRecentLockDeskStartTime(requestDateTime, lockDeskHours, calendar);
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): lockDeskStart: " + lockDeskStartTime.ToString("g"));
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Calling GetNextLockDeskEndTime()");
      DateTime nextLockDeskEndTime = LockDeskHoursManager.GetNextLockDeskEndTime(lockDeskStartTime, lockDeskHours);
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): lockDeskEnd: " + nextLockDeskEndTime.ToString("g"));
      if (requestDateTime < lockDeskStartTime)
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Error, "ValidateLockRequestTime(): requestTime was earlier than lockDeskStart");
        return DateTime.MinValue;
      }
      if (requestDateTime >= lockDeskStartTime && requestDateTime <= nextLockDeskEndTime)
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Within lock desk hours");
        return DateTime.MinValue;
      }
      return allowRelockOutSideLDHours ? DateTime.MinValue : nextLockDeskEndTime;
    }

    public static DateTime? GetONRPStartTime(
      DateTime lockDeskEndTime,
      LockDeskHoursInfo lockDeskHours,
      LockDeskOnrpInfo onrpInfo,
      BusinessCalendar calendarForLockDesk)
    {
      DateTime? onrpStartTime = new DateTime?(lockDeskEndTime);
      if (onrpInfo.IsContinuousONRPCoverage || onrpInfo.IsWeekendHolidayCoverage)
        return onrpStartTime;
      if (lockDeskEndTime.DayOfWeek == DayOfWeek.Saturday)
      {
        if (!onrpInfo.IsONRPSatEnabled)
          onrpStartTime = new DateTime?();
      }
      else if (lockDeskEndTime.DayOfWeek == DayOfWeek.Sunday)
      {
        if (!onrpInfo.IsONRPSunEnabled)
          onrpStartTime = new DateTime?();
      }
      else if (calendarForLockDesk != null && calendarForLockDesk.GetDayType(lockDeskEndTime) != CalendarDayType.BusinessDay && !onrpInfo.IsWeekendHolidayCoverage)
        onrpStartTime = new DateTime?();
      return onrpStartTime;
    }

    private static OnrpCalcInfo validateOnrp(
      DateTime requestDateTime,
      LockDeskHoursInfo lockDeskHours,
      DateTime lockDeskEnd,
      BusinessCalendar calendar,
      ILockDeskHoursHelper lockDeskHoursHelper,
      LoanChannel loanChannel,
      string tpoId,
      string loanNumber,
      double loanAmount,
      bool? overrideOnrpCheck,
      bool fromPlatform = false)
    {
      bool? nullable1 = overrideOnrpCheck;
      bool flag1 = false;
      if (nullable1.GetValueOrDefault() == flag1 & nullable1.HasValue)
      {
        lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateOnrp(): suppressing ONRP checks");
        return (OnrpCalcInfo) null;
      }
      if (loanChannel != LoanChannel.BankedWholesale && loanChannel != LoanChannel.BankedRetail && loanChannel != LoanChannel.Correspondent)
        throw new LockDeskONRPException("Lock Request submitted outside of Lock Desk Hours. Channel must be set to Banked Retail, Banked Wholesale or Correspondent to be validated for Overnight Rate Protection. Properly select the Channel and resubmit, or submit the Lock Request during Lock Desk Hours.");
      if ((loanChannel == LoanChannel.Correspondent || loanChannel == LoanChannel.BankedWholesale) && string.IsNullOrEmpty(tpoId))
        throw new LockDeskONRPException("Lock Request submitted outside of Lock Desk Hours.  TPO Company in TPO Information tool must be selected in order to be validated for Overnight Rate Protection.  Select the TPO Company and resubmit, or submit the Lock Request during Lock Desk Hours.");
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateOnrp(): Getting effective ONRP info...");
      LockDeskOnrpInfo lockDeskOnrpInfo = LockDeskHoursManager.GetEffectiveLockDeskOnrpInfo(lockDeskHoursHelper, loanChannel, tpoId);
      bool? nullable2 = overrideOnrpCheck;
      bool flag2 = true;
      if (nullable2.GetValueOrDefault() == flag2 & nullable2.HasValue)
        return new OnrpCalcInfo()
        {
          EffectiveEntityId = lockDeskOnrpInfo.EntityId,
          OnrpStartDate = lockDeskEnd.Date,
          IsONRPEligible = true,
          OnrpLockDate = requestDateTime.ToString("MM/dd/yyyy"),
          OnrpLockTime = requestDateTime.ToString("h:mm:ss tt")
        };
      if (!lockDeskOnrpInfo.IsONRPEnabled)
        return (OnrpCalcInfo) null;
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateOnrp(): Getting ONRP start time...");
      DateTime? onrpStartTime = LockDeskHoursManager.GetONRPStartTime(lockDeskEnd, lockDeskHours, lockDeskOnrpInfo, calendar);
      ILockDeskHoursHelper lockDeskHoursHelper1 = lockDeskHoursHelper;
      string sw1 = LockDeskHoursManager.sw;
      DateTime dateTime;
      string str1;
      if (onrpStartTime.HasValue)
      {
        dateTime = onrpStartTime.Value;
        str1 = dateTime.ToString("g");
      }
      else
        str1 = "none";
      string msg1 = "ValidateOnrp(): ONRP start time: " + str1;
      lockDeskHoursHelper1.Log(sw1, nameof (LockDeskHoursManager), TraceLevel.Verbose, msg1);
      if (!onrpStartTime.HasValue)
        return (OnrpCalcInfo) null;
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateOnrp(): Getting ONRP end time...");
      DateTime? onrpEndTime = LockDeskHoursManager.GetONRPEndTime(onrpStartTime.Value, lockDeskHours, lockDeskOnrpInfo, calendar, requestDateTime);
      ILockDeskHoursHelper lockDeskHoursHelper2 = lockDeskHoursHelper;
      string sw2 = LockDeskHoursManager.sw;
      string str2;
      if (onrpEndTime.HasValue)
      {
        dateTime = onrpEndTime.Value;
        str2 = dateTime.ToString("g");
      }
      else
        str2 = "none";
      string msg2 = "ValidateOnrp(): ONRP end time: " + str2;
      lockDeskHoursHelper2.Log(sw2, nameof (LockDeskHoursManager), TraceLevel.Verbose, msg2);
      if (!onrpEndTime.HasValue)
        return (OnrpCalcInfo) null;
      dateTime = requestDateTime;
      DateTime? nullable3 = onrpEndTime;
      if ((nullable3.HasValue ? (dateTime < nullable3.GetValueOrDefault() ? 1 : 0) : 0) == 0)
        throw new LockDeskONRPException("Lock Request submitted outside of Overnight Rate Protection window. Lock Request was not accepted. Please reprice and resubmit your Lock Request during normal Lock Desk Hours.");
      loanNumber = loanNumber == null ? "" : loanNumber;
      bool flag3 = lockDeskOnrpInfo.AllowONRPForCancelledExpiredLocks && LockDeskHoursManager.loanDataManager != null && !Utils.IsDate((object) LockDeskHoursManager.loanDataManager.LoanData.GetField("3917")) && !Utils.IsDate((object) LockDeskHoursManager.loanDataManager.LoanData.GetField("4119")) && (LockUtils.IsRelock(LockDeskHoursManager.loanDataManager.LoanData.GetField("3841")) || LockDeskHoursManager.loanDataManager.LoanData.GetField("3841") == "NewLock");
      if (!lockDeskHoursHelper.IsFirstLockRequest() && !flag3)
        throw new LockDeskONRPException("Only lock requests for the initial lock are eligible for Overnight Rate Protection. You will need to submit the lock request during normal Lock Desk Hours.");
      lockDeskHoursHelper.Log(LockDeskHoursManager.sw, nameof (LockDeskHoursManager), TraceLevel.Verbose, "ValidateLockRequestTime(): Calling ValidateOnrpDollarLimit");
      string message = LockDeskHoursManager.ValidateOnrpDollarLimit(lockDeskHoursHelper, lockDeskOnrpInfo, lockDeskEnd.Date, loanAmount, loanNumber);
      if (message != string.Empty)
        throw new LockDeskONRPException(message);
      return new OnrpCalcInfo()
      {
        EffectiveEntityId = lockDeskOnrpInfo.EntityId,
        OnrpStartDate = lockDeskEnd.Date,
        OnrpLockDate = requestDateTime.ToString("MM/dd/yyyy"),
        OnrpLockTime = requestDateTime.ToString("h:mm:ss tt"),
        IsONRPEligible = true
      };
    }

    public static LockDeskOnrpInfo GetEffectiveLockDeskOnrpInfo(
      ILockDeskHoursHelper lockDeskHourHelper,
      LoanChannel channel,
      string tpoId)
    {
      LockDeskOnrpInfo globalRetailSettings;
      LockDeskOnrpInfo globalWholesaleSettings;
      LockDeskOnrpInfo globalCorrespondentSettings;
      lockDeskHourHelper.GetONRPGlobalSettings(out globalRetailSettings, out globalWholesaleSettings, out globalCorrespondentSettings);
      LockDeskOnrpInfo lockDeskOnrpInfo = new LockDeskOnrpInfo();
      switch (channel)
      {
        case LoanChannel.BankedRetail:
          LockDeskOnrpInfo branchONRPSettings;
          lockDeskHourHelper.GetONRPRetailSettings(out branchONRPSettings);
          lockDeskOnrpInfo = LockDeskHoursManager.MergeWithGlobalSettings(globalRetailSettings, branchONRPSettings);
          break;
        case LoanChannel.BankedWholesale:
        case LoanChannel.Correspondent:
          LockDeskOnrpInfo brokerONRPSettings;
          LockDeskOnrpInfo correspondentONRPSettings;
          lockDeskHourHelper.GetONRPTPOSettings(tpoId, out brokerONRPSettings, out correspondentONRPSettings);
          if (channel == LoanChannel.Correspondent)
          {
            lockDeskOnrpInfo = LockDeskHoursManager.MergeWithGlobalSettings(globalCorrespondentSettings, correspondentONRPSettings);
            break;
          }
          if (channel == LoanChannel.BankedWholesale)
          {
            lockDeskOnrpInfo = LockDeskHoursManager.MergeWithGlobalSettings(globalWholesaleSettings, brokerONRPSettings);
            break;
          }
          break;
      }
      return lockDeskOnrpInfo;
    }

    public static LockDeskOnrpInfo MergeWithGlobalSettings(
      LockDeskOnrpInfo global,
      LockDeskOnrpInfo branch)
    {
      LockDeskOnrpInfo lockDeskOnrpInfo = global.Clone();
      if (branch != null)
      {
        if (lockDeskOnrpInfo.IsONRPEnabled && branch.IsONRPEnabled)
        {
          if (!branch.IsUseChannelDefaults)
          {
            lockDeskOnrpInfo.IsContinuousONRPCoverage = branch.IsContinuousONRPCoverage;
            lockDeskOnrpInfo.ONRPLimitAmount = branch.ONRPLimitAmount;
            lockDeskOnrpInfo.IsONRPEnabled = branch.IsONRPEnabled;
            lockDeskOnrpInfo.ONRPEndTime = branch.ONRPEndTime;
            lockDeskOnrpInfo.ONRPTolerance = branch.ONRPTolerance;
            lockDeskOnrpInfo.IsUseChannelDefaults = branch.IsUseChannelDefaults;
            lockDeskOnrpInfo.IsWeekendHolidayCoverage = branch.IsWeekendHolidayCoverage;
            lockDeskOnrpInfo.IsNoMaxLimit = branch.IsNoMaxLimit;
            lockDeskOnrpInfo.IsONRPSatEnabled = branch.IsONRPSatEnabled;
            lockDeskOnrpInfo.IsONRPSunEnabled = branch.IsONRPSunEnabled;
            lockDeskOnrpInfo.ONRPSatEndTime = branch.ONRPSatEndTime;
            lockDeskOnrpInfo.ONRPSunEndTime = branch.ONRPSunEndTime;
          }
          else
            lockDeskOnrpInfo.IsUseChannelDefaults = true;
        }
        else if (!branch.IsONRPEnabled)
          lockDeskOnrpInfo.IsONRPEnabled = false;
        if (branch.IsONRPEnabled)
          lockDeskOnrpInfo.AllowONRPForCancelledExpiredLocks = branch.AllowONRPForCancelledExpiredLocks;
        lockDeskOnrpInfo.Channel = branch.Channel;
        lockDeskOnrpInfo.EntityId = branch.EntityId;
      }
      else
        lockDeskOnrpInfo.IsONRPEnabled = false;
      return lockDeskOnrpInfo;
    }

    public static DateTime GetMostRecentLockDeskStartTime(
      DateTime requestTimeET,
      LockDeskHoursInfo lockDeskInfo,
      BusinessCalendar calendarForLockDesk,
      bool needCheckTime = true)
    {
      if (lockDeskInfo == null)
        throw new ArgumentNullException("lockDeskInfo cannot be null");
      DateTime dateTime = requestTimeET;
      TimeSpan timeSpan = lockDeskInfo.LockDeskStartTime;
      if (requestTimeET.DayOfWeek == DayOfWeek.Saturday)
      {
        if (lockDeskInfo.LockDeskSatHoursEnabled)
          timeSpan = lockDeskInfo.LockDeskStartTimeSat;
        else
          needCheckTime = false;
      }
      if (requestTimeET.DayOfWeek == DayOfWeek.Sunday)
      {
        if (lockDeskInfo.LockDeskSunHoursEnabled)
          timeSpan = lockDeskInfo.LockDeskStartTimeSun;
        else
          needCheckTime = false;
      }
      if (needCheckTime && requestTimeET.TimeOfDay < timeSpan)
        dateTime = requestTimeET.AddDays(-1.0);
      if (calendarForLockDesk != null && calendarForLockDesk.GetDayType(dateTime) == CalendarDayType.Holiday)
        dateTime = calendarForLockDesk.GetPreviousClosestNotHolidayDay(dateTime);
      if (dateTime.DayOfWeek == DayOfWeek.Saturday)
      {
        if (!lockDeskInfo.LockDeskSatHoursEnabled)
          dateTime = dateTime.AddDays(-1.0);
      }
      else if (dateTime.DayOfWeek == DayOfWeek.Sunday)
      {
        if (!lockDeskInfo.LockDeskSunHoursEnabled && !lockDeskInfo.LockDeskSatHoursEnabled)
          dateTime = dateTime.AddDays(-2.0);
        if (!lockDeskInfo.LockDeskSunHoursEnabled && lockDeskInfo.LockDeskSatHoursEnabled)
          dateTime = dateTime.AddDays(-1.0);
      }
      if (calendarForLockDesk != null && calendarForLockDesk.GetDayType(dateTime) == CalendarDayType.Holiday)
        dateTime = LockDeskHoursManager.GetMostRecentLockDeskStartTime(dateTime, lockDeskInfo, calendarForLockDesk, false);
      if (dateTime.DayOfWeek == DayOfWeek.Saturday)
        return dateTime.Date.Add(lockDeskInfo.LockDeskStartTimeSat);
      return dateTime.DayOfWeek == DayOfWeek.Sunday ? dateTime.Date.Add(lockDeskInfo.LockDeskStartTimeSun) : dateTime.Date.Add(lockDeskInfo.LockDeskStartTime);
    }

    private static bool isBusinessDay(DateTime requestDateTime, BusinessCalendar calendar)
    {
      if (calendar != null)
        return calendar.GetDayType(requestDateTime) == CalendarDayType.BusinessDay;
      return requestDateTime.DayOfWeek != DayOfWeek.Saturday && requestDateTime.DayOfWeek != 0;
    }

    private static DateTime getNextLockDeskDay(
      DateTime currentDay,
      BusinessCalendar calendar,
      LockDeskHoursInfo lockDeskInfo)
    {
      DateTime nextLockDeskDay = currentDay.Date.AddDays(1.0);
      if (nextLockDeskDay.DayOfWeek == DayOfWeek.Saturday && !lockDeskInfo.LockDeskSatHoursEnabled || nextLockDeskDay.DayOfWeek == DayOfWeek.Sunday && !lockDeskInfo.LockDeskSunHoursEnabled)
        nextLockDeskDay = LockDeskHoursManager.getNextLockDeskDay(nextLockDeskDay, calendar, lockDeskInfo);
      if (calendar != null && calendar.GetDayType(nextLockDeskDay) == CalendarDayType.Holiday)
        nextLockDeskDay = LockDeskHoursManager.getNextLockDeskDay(nextLockDeskDay, calendar, lockDeskInfo);
      return nextLockDeskDay;
    }

    public static DateTime GetNextLockDeskEndTime(
      DateTime lockDeskStartTime,
      LockDeskHoursInfo lockDeskInfo)
    {
      if (lockDeskInfo == null)
        throw new ArgumentNullException("lockDeskInfo cannot be null");
      return lockDeskStartTime.DayOfWeek == DayOfWeek.Saturday ? (lockDeskStartTime.TimeOfDay < lockDeskInfo.LockDeskEndTimeSat ? lockDeskStartTime.Date.Add(lockDeskInfo.LockDeskEndTimeSat) : lockDeskStartTime.Date.AddDays(1.0).Add(lockDeskInfo.LockDeskEndTimeSat)) : (lockDeskStartTime.DayOfWeek == DayOfWeek.Sunday ? (lockDeskStartTime.TimeOfDay < lockDeskInfo.LockDeskEndTimeSun ? lockDeskStartTime.Date.Add(lockDeskInfo.LockDeskEndTimeSun) : lockDeskStartTime.Date.AddDays(1.0).Add(lockDeskInfo.LockDeskEndTimeSun)) : (lockDeskStartTime.TimeOfDay < lockDeskInfo.LockDeskEndTime ? lockDeskStartTime.Date.Add(lockDeskInfo.LockDeskEndTime) : lockDeskStartTime.Date.AddDays(1.0).Add(lockDeskInfo.LockDeskEndTime)));
    }

    public static DateTime? GetONRPEndTime(
      DateTime onrpStartTime,
      LockDeskHoursInfo lockDeskHours,
      LockDeskOnrpInfo onrpInfo,
      BusinessCalendar calendarForLockDesk,
      DateTime requestDateTime)
    {
      return !onrpInfo.IsContinuousONRPCoverage ? (!onrpInfo.IsWeekendHolidayCoverage ? new DateTime?(LockDeskHoursManager.GetOnrpEndTime_SpecifyEndTime(onrpStartTime, lockDeskHours, onrpInfo, calendarForLockDesk)) : new DateTime?(LockDeskHoursManager.GetOnrpEndTime_WeekendHolidayCoverage(onrpStartTime, lockDeskHours, onrpInfo, calendarForLockDesk, requestDateTime))) : new DateTime?(LockDeskHoursManager.GetOnrpEndTime_ContinuousCoverage(onrpStartTime, lockDeskHours, onrpInfo, calendarForLockDesk));
    }

    private static DateTime GetOnrpEndTime_ContinuousCoverage(
      DateTime onrpStartTime,
      LockDeskHoursInfo lockDeskHours,
      LockDeskOnrpInfo onrpInfo,
      BusinessCalendar calendarForLockDesk)
    {
      DateTime dateTime = onrpStartTime.Date;
      switch (onrpStartTime.DayOfWeek)
      {
        case DayOfWeek.Sunday:
          dateTime = !(onrpStartTime.TimeOfDay < lockDeskHours.LockDeskStartTimeSun) ? LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours) : onrpStartTime.Date;
          break;
        case DayOfWeek.Monday:
        case DayOfWeek.Tuesday:
        case DayOfWeek.Wednesday:
        case DayOfWeek.Thursday:
        case DayOfWeek.Friday:
          dateTime = !(onrpStartTime.TimeOfDay < lockDeskHours.LockDeskStartTime) ? LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours) : (calendarForLockDesk == null || calendarForLockDesk.GetDayType(onrpStartTime) != CalendarDayType.Holiday ? onrpStartTime.Date : LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours));
          break;
        case DayOfWeek.Saturday:
          dateTime = !(onrpStartTime.TimeOfDay < lockDeskHours.LockDeskStartTimeSat) ? LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours) : onrpStartTime.Date;
          break;
      }
      return dateTime.DayOfWeek != DayOfWeek.Saturday ? (dateTime.DayOfWeek != DayOfWeek.Sunday ? dateTime.Date.Add(lockDeskHours.LockDeskStartTime) : dateTime.Date.Add(lockDeskHours.LockDeskStartTimeSun)) : dateTime.Date.Add(lockDeskHours.LockDeskStartTimeSat);
    }

    private static DateTime GetOnrpEndTime_WeekendHolidayCoverage(
      DateTime onrpStartTime,
      LockDeskHoursInfo lockDeskHours,
      LockDeskOnrpInfo onrpInfo,
      BusinessCalendar calendarForLockDesk,
      DateTime requestDateTime)
    {
      DateTime date = onrpStartTime.Date;
      DateTime dateTime1 = requestDateTime.DayOfWeek == DayOfWeek.Saturday || requestDateTime.DayOfWeek == DayOfWeek.Sunday || calendarForLockDesk != null && calendarForLockDesk.GetDayType(requestDateTime) == CalendarDayType.Holiday || onrpStartTime.DayOfWeek == DayOfWeek.Saturday || onrpStartTime.DayOfWeek == DayOfWeek.Sunday || calendarForLockDesk != null && calendarForLockDesk.GetDayType(onrpStartTime) == CalendarDayType.Holiday ? LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours) : (!(onrpStartTime.TimeOfDay < onrpInfo.ONRPEndTime) ? LockDeskHoursManager.getNextLockDeskDay(onrpStartTime, calendarForLockDesk, lockDeskHours) : onrpStartTime.Date);
      DateTime dateTime2 = dateTime1.Date;
      if (onrpInfo.ONRPEndTime > lockDeskHours.LockDeskStartTime && onrpStartTime.Date < dateTime1.Date)
        dateTime2 = dateTime2.AddDays(-1.0);
      return dateTime2.Date.Add(onrpInfo.ONRPEndTime);
    }

    private static DateTime GetOnrpEndTime_SpecifyEndTime(
      DateTime onrpStartTime,
      LockDeskHoursInfo lockDeskHours,
      LockDeskOnrpInfo onrpInfo,
      BusinessCalendar calendarForLockDesk)
    {
      DateTime timeSpecifyEndTime;
      if (onrpStartTime.DayOfWeek == DayOfWeek.Saturday)
      {
        if (onrpInfo.ONRPSatEndTime > onrpStartTime.TimeOfDay)
        {
          timeSpecifyEndTime = onrpStartTime.Date.Add(onrpInfo.ONRPSatEndTime);
        }
        else
        {
          DateTime dateTime = onrpStartTime.Date;
          dateTime = dateTime.AddDays(1.0);
          timeSpecifyEndTime = dateTime.Add(onrpInfo.ONRPSatEndTime);
        }
      }
      else if (onrpStartTime.DayOfWeek == DayOfWeek.Sunday)
      {
        if (onrpInfo.ONRPSunEndTime > onrpStartTime.TimeOfDay)
        {
          timeSpecifyEndTime = onrpStartTime.Date.Add(onrpInfo.ONRPSunEndTime);
        }
        else
        {
          DateTime dateTime = onrpStartTime.Date;
          dateTime = dateTime.AddDays(1.0);
          timeSpecifyEndTime = dateTime.Add(onrpInfo.ONRPSunEndTime);
        }
      }
      else if (onrpInfo.ONRPEndTime > onrpStartTime.TimeOfDay)
      {
        timeSpecifyEndTime = onrpStartTime.Date.Add(onrpInfo.ONRPEndTime);
      }
      else
      {
        DateTime dateTime = onrpStartTime.Date;
        dateTime = dateTime.AddDays(1.0);
        timeSpecifyEndTime = dateTime.Add(onrpInfo.ONRPEndTime);
      }
      return timeSpecifyEndTime;
    }

    public static string ValidateOnrpDollarLimit(
      ILockDeskHoursHelper lockDeskHoursHelper,
      LockDeskOnrpInfo effectiveOnrpInfo,
      DateTime onrpStartDate,
      double loanAmount,
      string loanNumber)
    {
      if (effectiveOnrpInfo.IsNoMaxLimit)
        return string.Empty;
      double onrpLimitAmount = effectiveOnrpInfo.ONRPLimitAmount;
      if (effectiveOnrpInfo.ONRPTolerance > 0)
        onrpLimitAmount *= 1.0 + (double) effectiveOnrpInfo.ONRPTolerance / 100.0;
      double num = lockDeskHoursHelper.GetOnrpAccruedDollarAmount(effectiveOnrpInfo.Channel, effectiveOnrpInfo.EntityId, onrpStartDate) + loanAmount;
      if (onrpLimitAmount >= num)
        return string.Empty;
      string empty = string.Empty;
      string newValue = (num - onrpLimitAmount).ToString("#,##0");
      string str = !string.IsNullOrEmpty(effectiveOnrpInfo.ONRPOverLimitMessage) ? effectiveOnrpInfo.ONRPOverLimitMessage.Replace("<Loan Number>", loanNumber).Replace("<Dollar Amount>", newValue) : string.Format("Overnight Rate Protection for Loan {0} exceeded Company limit by ${1}.", (object) loanNumber, (object) newValue);
      if (!string.IsNullOrEmpty(effectiveOnrpInfo.ONRPMessageAddendum))
        str = str + " " + effectiveOnrpInfo.ONRPMessageAddendum;
      return str;
    }

    public static void PerformOnrpRegistrationThickClient(
      SessionObjects sessionObjs,
      LoanDataMgr loanDataMgr,
      OnrpCalcInfo onrpCalcInfo)
    {
      if (onrpCalcInfo.IsManual)
        return;
      double loanAmount = Utils.ParseDouble((object) loanDataMgr.LoanData.GetField("2965"));
      string field = loanDataMgr.LoanData.GetField("2626");
      sessionObjs.OverNightRateProtectionManager.UpdateOnrpPeriodAccruedAmount(LockDeskHoursUtils.GetEnumChannel(field), onrpCalcInfo.EffectiveEntityId, onrpCalcInfo.OnrpStartDate, loanAmount);
    }

    public static DateTime GetLockDateForOnrp(
      IClientSession clientSession,
      SessionObjects sessionObjs,
      LoanDataMgr loanDataMgr,
      DateTime requested)
    {
      try
      {
        EncompassLockDeskHoursHelper lockDeskHoursHelper = new EncompassLockDeskHoursHelper(clientSession, sessionObjs, loanDataMgr);
        BusinessCalendar expirationCalendar = LockDeskHoursUtils.GetLockExpirationCalendar(sessionObjs.StartupInfo, sessionObjs);
        LockDeskHoursInfo deskHoursSettings = lockDeskHoursHelper.GetLockDeskHoursSettings(lockDeskHoursHelper.GetLoanChannel());
        return LockDeskHoursManager.GetMostRecentLockDeskStartTime(requested, deskHoursSettings, expirationCalendar);
      }
      catch (Exception ex)
      {
        return DateTime.Today;
      }
    }
  }
}
