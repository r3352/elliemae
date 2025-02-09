// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ONRPSettingRules
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ONRPSettingRules : ONRPBaseRule
  {
    public ONRPSettingRules()
    {
    }

    public ONRPSettingRules(ONRPEntitySettings settings)
      : base(settings)
    {
    }

    public override bool EnableONRP
    {
      get
      {
        return this.settings.GlobalSettings != null && this.settings.GlobalSettings.ONRPEnabled == "True" && this.settings.GlobalSettings.IsEncompassLockDeskHoursEnabled == "True";
      }
    }

    public override bool EnableSatONRP
    {
      get
      {
        return this.settings.GlobalSettings != null && this.settings.GlobalSettings.ONRPSatEnabled == "True" && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage;
      }
    }

    public override bool EnableSunONRP
    {
      get
      {
        return this.settings.GlobalSettings != null && this.settings.GlobalSettings.ONRPSunEnabled == "True" && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage;
      }
    }

    public override bool UseChannelDefaultsAndCustomize
    {
      get => this.settings.EnableONRP && this.EnableONRP;
    }

    public override bool ContinuousCoverageAndSpecifyTime
    {
      get => this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault;
    }

    public override bool AllowONRPForCancelledExpiredLocks
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && this.settings.AllowONRPForCancelledExpiredLocks;
      }
    }

    public override bool EndTime
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && !this.TwentyfourhrWeekday;
      }
    }

    public override bool SatStartTime
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && this.settings.EnableSatONRP && !this.TwentyfourhrSat && this.EnableSatLD;
      }
    }

    public override bool SatEndTime
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && this.settings.EnableSatONRP && !this.TwentyfourhrSat && this.EnableSatLD;
      }
    }

    public override bool SunStartTime
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && this.settings.EnableSunONRP && !this.TwentyfourhrSun && this.EnableSunLD;
      }
    }

    public override bool SunEndTime
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && this.settings.EnableSunONRP && !this.TwentyfourhrSun && this.EnableSunLD;
      }
    }

    public override bool WeekendHolidayCoverage
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && !(this.settings.GlobalSettings.EnableLockDeskSat == "True") && !(this.settings.GlobalSettings.EnableLockDeskSun == "True");
      }
    }

    public override bool NoMaxLimit
    {
      get => this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault;
    }

    public override bool DollarLimit
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.MaximumLimit;
      }
    }

    public override bool Tolerance
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.MaximumLimit;
      }
    }

    public override bool TwentyfourhrWeekday
    {
      get
      {
        return this.settings.EnableONRP && this.settings.GlobalSettings.LockDeskStartTime == this.settings.GlobalSettings.LockDeskEndTime;
      }
    }

    public override bool TwentyfourhrSat
    {
      get
      {
        return this.settings.EnableONRP && this.settings.GlobalSettings.LockDeskStartTimeSat == this.settings.GlobalSettings.LockDeskEndTimeSat;
      }
    }

    public override bool TwentyfourhrSun
    {
      get
      {
        return this.settings.EnableONRP && this.settings.GlobalSettings.LockDeskStartTimeSun == this.settings.GlobalSettings.LockDeskEndTimeSun;
      }
    }

    public override bool EnableSat
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && !this.TwentyfourhrSat && this.EnableSatLD;
      }
    }

    public override bool EnableSun
    {
      get
      {
        return this.settings.EnableONRP && this.EnableONRP && !this.settings.UseChannelDefault && !this.settings.ContinuousCoverage && !this.TwentyfourhrSun && this.EnableSunLD;
      }
    }

    public override bool EnableSatLD
    {
      get
      {
        return this.settings.GlobalSettings != null && this.settings.GlobalSettings.EnableLockDeskSat == "True";
      }
    }

    public override bool EnableSunLD
    {
      get
      {
        return this.settings.GlobalSettings != null && this.settings.GlobalSettings.EnableLockDeskSun == "True";
      }
    }
  }
}
