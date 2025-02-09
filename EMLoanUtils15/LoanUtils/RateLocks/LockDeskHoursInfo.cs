// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.LockDeskHoursInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer.Configuration;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public class LockDeskHoursInfo
  {
    public LockExpCalendarSetting LockExpirationCalendar { get; set; }

    public bool IsEncompassLockDeskHoursEnabled { get; set; }

    public TimeSpan LockDeskStartTime { get; set; }

    public TimeSpan LockDeskEndTime { get; set; }

    public bool LockDeskSatHoursEnabled { get; set; }

    public TimeSpan LockDeskStartTimeSat { get; set; }

    public TimeSpan LockDeskEndTimeSat { get; set; }

    public bool LockDeskSunHoursEnabled { get; set; }

    public TimeSpan LockDeskStartTimeSun { get; set; }

    public TimeSpan LockDeskEndTimeSun { get; set; }

    public string LockDeskHoursMessage { get; set; }

    public bool IsLockDeskShutdown { get; set; }

    public string LockDeskShutdownMessage { get; set; }

    public bool AllowActiveRelockRequests { get; set; }

    public LockDeskHoursInfo Clone() => (LockDeskHoursInfo) this.MemberwiseClone();
  }
}
