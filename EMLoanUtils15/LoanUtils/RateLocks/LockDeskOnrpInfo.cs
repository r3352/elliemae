// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.LoanUtils.RateLocks.LockDeskOnrpInfo
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.LoanUtils.RateLocks
{
  public class LockDeskOnrpInfo
  {
    public string EntityId;

    public bool IsONRPEnabled { get; set; }

    public bool AllowONRPForCancelledExpiredLocks { get; set; }

    public bool IsUseChannelDefaults { get; set; }

    public bool IsContinuousONRPCoverage { get; set; }

    public TimeSpan ONRPEndTime { get; set; }

    public bool IsONRPSatEnabled { get; set; }

    public TimeSpan ONRPSatEndTime { get; set; }

    public bool IsONRPSunEnabled { get; set; }

    public TimeSpan ONRPSunEndTime { get; set; }

    public bool IsWeekendHolidayCoverage { get; set; }

    public bool IsNoMaxLimit { get; set; }

    public double ONRPLimitAmount { get; set; }

    public int ONRPTolerance { get; set; }

    public string ONRPOverLimitMessage { get; set; }

    public string ONRPMessageAddendum { get; set; }

    public LoanChannel Channel { get; set; }

    public LockDeskOnrpInfo Clone() => (LockDeskOnrpInfo) this.MemberwiseClone();
  }
}
