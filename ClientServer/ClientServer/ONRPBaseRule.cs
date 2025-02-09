// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ONRPBaseRule
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ONRPBaseRule
  {
    protected ONRPEntitySettings settings;

    public ONRPBaseRule()
    {
    }

    public ONRPBaseRule(ONRPEntitySettings settings) => this.settings = settings;

    public void Subject(ONRPEntitySettings settings) => this.settings = settings;

    public virtual bool EnableONRP => true;

    public virtual bool UseChannelDefaultsAndCustomize => true;

    public virtual bool ContinuousCoverageAndSpecifyTime => true;

    public virtual bool AllowONRPForCancelledExpiredLocks => true;

    public virtual bool EndTime => true;

    public virtual bool WeekendHolidayCoverage => true;

    public virtual bool NoMaxLimit => true;

    public virtual bool DollarLimit => true;

    public virtual bool Tolerance => true;

    public virtual bool EnableSatONRP => true;

    public virtual bool EnableSunONRP => true;

    public virtual bool EnableSat => true;

    public virtual bool EnableSun => true;

    public virtual bool SatStartTime => true;

    public virtual bool SatEndTime => true;

    public virtual bool SunStartTime => true;

    public virtual bool SunEndTime => true;

    public virtual bool TwentyfourhrWeekday => false;

    public virtual bool TwentyfourhrSat => false;

    public virtual bool TwentyfourhrSun => false;

    public virtual bool EnableSatLD => false;

    public virtual bool EnableSunLD => false;
  }
}
