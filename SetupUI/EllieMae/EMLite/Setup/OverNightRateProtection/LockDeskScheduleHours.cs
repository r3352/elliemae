// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.LockDeskScheduleHours
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public class LockDeskScheduleHours
  {
    private string centralChannel = "";

    public string CentralChannel
    {
      get => this.centralChannel;
      set => this.centralChannel = value;
    }

    public string WeekdayStart { set; get; }

    public string WeekdayEnd { get; set; }

    public string SaturdayHoursEnabled { set; get; }

    public string SaturdayStart { set; get; }

    public string SaturdayEnd { set; get; }

    public string SundayHoursEnabled { set; get; }

    public string SundayStart { set; get; }

    public string SundayEnd { set; get; }

    public string LockDeskHoursMessage { get; set; }

    public string ShutDownLockDesk { get; set; }

    public string LockDeskShutDownMessage { get; set; }

    public string AllowActiveRelockRequests { get; set; }

    public bool NeedClearAccruedAmount(LockDeskScheduleHours other)
    {
      return this.WeekdayStart != other.WeekdayStart || this.WeekdayEnd != other.WeekdayEnd || this.SaturdayHoursEnabled != other.SaturdayHoursEnabled || this.SaturdayStart != other.SaturdayStart || this.SaturdayEnd != other.SaturdayEnd || this.SundayHoursEnabled != other.SundayHoursEnabled || this.SundayStart != other.SundayStart || this.SundayEnd != other.SundayEnd;
    }
  }
}
