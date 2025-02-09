// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ONRPUtils
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public static class ONRPUtils
  {
    public static bool LockDeskHourHasOverlap(
      DateTime lockDeskStartTime,
      DateTime lockDeskEndTime,
      DateTime onrpStartTime,
      DateTime onrpEndTime)
    {
      DateTime dateTime = new DateTime();
      TimeSpan timeSpan = dateTime.AddDays(1.0) - dateTime;
      return (lockDeskEndTime.TimeOfDay >= lockDeskStartTime.TimeOfDay ? lockDeskEndTime.TimeOfDay - lockDeskStartTime.TimeOfDay : timeSpan - (lockDeskStartTime.TimeOfDay - lockDeskEndTime.TimeOfDay)) + (onrpEndTime.TimeOfDay >= onrpStartTime.TimeOfDay ? onrpEndTime.TimeOfDay - onrpStartTime.TimeOfDay : timeSpan - (onrpStartTime.TimeOfDay - onrpEndTime.TimeOfDay)) > timeSpan;
    }

    public static bool ValidateLDWithWeekendOverlap(
      DateTime startTime1,
      DateTime endTime1,
      DateTime startTime2)
    {
      return !(startTime1.TimeOfDay >= endTime1.TimeOfDay) || !(endTime1.TimeOfDay > startTime2.TimeOfDay);
    }

    public static bool ValidateSettings(
      IONRPRuleHandler messageHandler,
      ONRPEntitySettings setting,
      out bool resetEndTime)
    {
      resetEndTime = false;
      if (setting.UseChannelDefault || !setting.EnableONRP)
        return true;
      if (!setting.ContinuousCoverage)
      {
        if (string.IsNullOrEmpty(setting.ONRPEndTime) && !setting.rules.TwentyfourhrWeekday)
        {
          messageHandler.MessageHandler("The ONRP End Time must be selected before the ONRP Settings may be Saved.");
          if (setting.ONRPEndTime != string.Empty)
            resetEndTime = true;
          return false;
        }
        if (setting.EnableSatONRP && string.IsNullOrEmpty(setting.ONRPSatEndTime) && !setting.rules.TwentyfourhrSat)
        {
          messageHandler.MessageHandler("The ONRP Saturday End Time must be selected before the ONRP Settings may be Saved.");
          if (setting.ONRPSatEndTime != string.Empty)
            resetEndTime = false;
          return false;
        }
        if (setting.EnableSunONRP && string.IsNullOrEmpty(setting.ONRPSunEndTime) && !setting.rules.TwentyfourhrSun)
        {
          messageHandler.MessageHandler("The ONRP Sunday End Time must be selected before the ONRP Settings may be Saved.");
          if (setting.ONRPSatEndTime != string.Empty)
            resetEndTime = false;
          return false;
        }
      }
      if (setting.rules.NoMaxLimit && !setting.MaximumLimit && setting.DollarLimit == 0.0)
      {
        messageHandler.MessageHandler("An ONRP Dollar Limit must be entered if the No Maximum Limit is disabled.");
        return false;
      }
      if (ONRPUtils.LockDeskHourHasOverlap(ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskStartTime), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTime), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTime), ONRPEntitySettings.ConverToDateTime(setting.ONRPEndTime)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Weekday Lock Desk Hours plus Weekday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
        resetEndTime = true;
        return false;
      }
      if (ONRPUtils.LockDeskHourHasOverlap(ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskStartTimeSat), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTimeSat), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTimeSat), ONRPEntitySettings.ConverToDateTime(setting.ONRPSatEndTime)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Saturday Lock Desk Hours plus Saturday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
        resetEndTime = false;
        return false;
      }
      if (ONRPUtils.LockDeskHourHasOverlap(ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskStartTimeSun), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTimeSun), ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTimeSun), ONRPEntitySettings.ConverToDateTime(setting.ONRPSunEndTime)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Sunday Lock Desk Hours plus Sunday ONRP Hours exceed 24 hours and must be corrected before these settings can be saved.");
        resetEndTime = false;
        return false;
      }
      if (setting.EnableONRP && !string.IsNullOrEmpty(setting.GlobalSettings.LockDeskStartTimeSat) && !string.IsNullOrEmpty(setting.ONRPStartTime) && !string.IsNullOrEmpty(setting.ONRPEndTime) && !ONRPUtils.ValidateLDWithWeekendOverlap(Utils.ToDate(setting.ONRPStartTime), Utils.ToDate(setting.ONRPEndTime), Utils.ToDate(setting.GlobalSettings.LockDeskStartTimeSat)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Weekday ONRP End Time overlaps Lock Desk Saturday Hours and must be corrected before these settings can be saved. ");
        resetEndTime = true;
        return false;
      }
      if (setting.EnableONRP && setting.EnableSatONRP && !string.IsNullOrEmpty(setting.GlobalSettings.LockDeskStartTimeSun) && !string.IsNullOrEmpty(setting.ONRPSatStartTime) && !string.IsNullOrEmpty(setting.ONRPSatEndTime) && !ONRPUtils.ValidateLDWithWeekendOverlap(Utils.ToDate(setting.ONRPSatStartTime), Utils.ToDate(setting.ONRPSatEndTime), Utils.ToDate(setting.GlobalSettings.LockDeskStartTimeSun)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Saturday ONRP End Time overlaps Lock Desk Sunday Hours and must be corrected before these settings can be saved.");
        resetEndTime = false;
        return false;
      }
      if (setting.EnableONRP && setting.EnableSunONRP && !string.IsNullOrEmpty(setting.GlobalSettings.LockDeskStartTime) && !string.IsNullOrEmpty(setting.ONRPSunStartTime) && !string.IsNullOrEmpty(setting.ONRPSunEndTime) && !ONRPUtils.ValidateLDWithWeekendOverlap(Utils.ToDate(setting.ONRPSunStartTime), Utils.ToDate(setting.ONRPSunEndTime), Utils.ToDate(setting.GlobalSettings.LockDeskStartTime)))
      {
        messageHandler.MessageHandler(setting.GlobalSettings.BranchChannel + " Sunday ONRP Hours overlap Weekday Lock Desk Hours and must be corrected before these settings may be saved.");
        resetEndTime = false;
        return false;
      }
      TimeSpan timeOfDay = ONRPEntitySettings.ConverToDateTime(setting.ONRPEndTime).TimeOfDay;
      string msg = ONRPUtils.TimeStampSameErrorMessage(ONRPEntitySettings.ConverToDateTime(setting.GlobalSettings.LockDeskEndTime), ONRPEntitySettings.ConverToDateTime(setting.ONRPEndTime), false, "", "");
      if (string.IsNullOrEmpty(msg))
        return true;
      messageHandler.MessageHandler(msg);
      return false;
    }

    public static string TimeStampSameErrorMessage(
      DateTime startTime,
      DateTime endTime,
      bool isLockDeskHours,
      string hourType = "�",
      string channel = "�")
    {
      if (startTime == DateTime.MinValue || endTime == DateTime.MinValue)
        return (string) null;
      if (!(startTime.ToString("hh:mm tt") == endTime.ToString("hh:mm tt")))
        return (string) null;
      if (isLockDeskHours)
        return "Lock Desk Start Time and Lock Desk End Time may not be the same. Please correct the time(s) and resave.";
      hourType = !(hourType != "") ? " " : " " + hourType + " ";
      channel = !(channel != "") ? " " : " " + channel + " ";
      return "ONRP" + hourType + "Start Time and ONRP" + hourType + "End Time for" + channel + "channel may not be the same. Please correct the time(s) and resave.";
    }

    public static DateTime ONRPEndTime(
      DateTime ldStartTime,
      DateTime ldEndTime,
      DateTime onrpEndTime,
      bool isContinuousCvrg,
      bool enableBranch)
    {
      return ldStartTime == ldEndTime || string.IsNullOrEmpty(onrpEndTime.ToString()) || isContinuousCvrg ? DateTime.MinValue : onrpEndTime;
    }

    public static DateTime ONRPStartTime(DateTime ldStartTime, DateTime ldEndTime)
    {
      return ldStartTime == ldEndTime ? DateTime.MinValue : ldEndTime;
    }
  }
}
