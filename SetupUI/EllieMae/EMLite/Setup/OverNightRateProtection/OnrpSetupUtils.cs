// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.OnrpSetupUtils
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public static class OnrpSetupUtils
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

    public static string TimeStampSameErrorMessage(
      DateTime startTime,
      DateTime endTime,
      bool isGlobal)
    {
      if (!(startTime.ToString("hh:mm tt") == endTime.ToString("hh:mm tt")))
        return (string) null;
      return isGlobal ? "Lock Desk Start Time and Lock Desk End Time may not be the same. Please correct the time(s) and resave." : "ONRP Start Time and ONRP End Time may not be the same. Please correct the time(s) and resave.";
    }
  }
}
