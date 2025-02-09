// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Setup.OverNightRateProtection.OnrpSettings
// Assembly: SetupUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: B5055D69-6F9D-458D-8A91-C87166D995EA
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\SetupUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Setup.OverNightRateProtection
{
  public class OnrpSettings
  {
    public LoanChannel Channel { get; set; }

    public bool EnableONRP { get; set; }

    public bool AllowONRPForCancelledExpiredLocks { get; set; }

    public string ContinuousCoverage { get; set; }

    public bool WeekendHolidayCoverage { get; set; }

    public bool MaximumLimit { get; set; }

    public double DollarLimit { get; set; }

    public int Tolerance { get; set; }

    public bool EnableSatONRP { get; set; }

    public bool EnableSunONRP { get; set; }

    public string ONRPStartTime { get; set; }

    public string ONRPEndTime { get; set; }

    public string ONRPSatStartTime { get; set; }

    public string ONRPSatEndTime { get; set; }

    public string ONRPSunStartTime { get; set; }

    public string ONRPSunEndTime { get; set; }

    public OnrpSettings(IDictionary settings, LoanChannel channel)
    {
      string str = "";
      switch (channel)
      {
        case LoanChannel.BankedRetail:
          str = "RET";
          break;
        case LoanChannel.BankedWholesale:
          str = "BROKER";
          break;
        case LoanChannel.Correspondent:
          str = "COR";
          break;
      }
      this.Channel = channel;
      this.EnableONRP = Utils.ParseBoolean(settings[(object) ("Policies.EnableONRP" + str)]);
      if (channel == LoanChannel.Correspondent)
        this.AllowONRPForCancelledExpiredLocks = Utils.ParseBoolean(settings[(object) ("Policies.ONRPCancelledExpiredLocks" + str)]);
      this.ContinuousCoverage = settings[(object) string.Format("Policies.ONRP{0}Cvrg", (object) str)].ToString();
      this.WeekendHolidayCoverage = Utils.ParseBoolean(settings[(object) string.Format("Policies.ENABLEONRPWH{0}CVRG", (object) str)]);
      this.MaximumLimit = Utils.ParseBoolean(settings[(object) string.Format("Policies.ONRPNOMAXLIMIT{0}", (object) str)]);
      this.DollarLimit = Utils.ParseDouble(settings[(object) string.Format("Policies.ONRP{0}DOLLIMIT", (object) str)]);
      this.Tolerance = Utils.ParseInt(settings[(object) string.Format("Policies.ONRP{0}DOLTOL", (object) str)]);
      this.EnableSatONRP = Utils.ParseBoolean(settings[(object) string.Format("Policies.ENABLEONRP{0}SAT", (object) str)]);
      this.EnableSunONRP = Utils.ParseBoolean(settings[(object) string.Format("Policies.ENABLEONRP{0}Sun", (object) str)]);
      this.ONRPStartTime = settings[(object) string.Format("Policies.{0}LDENDTIME", (object) str)] as string;
      this.ONRPEndTime = settings[(object) string.Format("Policies.ONRP{0}ENDTIME", (object) str)] as string;
      this.ONRPSatStartTime = settings[(object) string.Format("Policies.{0}LDSATENDTIME", (object) str)] as string;
      this.ONRPSatEndTime = settings[(object) string.Format("Policies.ONRP{0}SATENDTIME", (object) str)] as string;
      this.ONRPSunStartTime = settings[(object) string.Format("Policies.{0}LDSUNENDTIME", (object) str)] as string;
      this.ONRPSunEndTime = settings[(object) string.Format("Policies.ONRP{0}SUNENDTIME", (object) str)] as string;
    }

    public bool NeedClearAccruedAmount(LockDeskGlobalSettings settings)
    {
      return this.EnableONRP != Utils.ParseBoolean((object) settings.ONRPEnabled) || this.AllowONRPForCancelledExpiredLocks != Utils.ParseBoolean((object) settings.AllowONRPForCancelledExpiredLocks) || !string.Equals(settings.ContinuousCoverage, this.ContinuousCoverage, StringComparison.CurrentCultureIgnoreCase) || this.WeekendHolidayCoverage != Utils.ParseBoolean((object) settings.WeekendHoliday) || this.MaximumLimit != Utils.ParseBoolean((object) settings.NoMaxLimit) || this.DollarLimit != Utils.ParseDouble((object) settings.DollarLimit) || this.Tolerance != Utils.ParseInt((object) settings.Tolerance) || this.EnableSatONRP != Utils.ParseBoolean((object) settings.ONRPSatEnabled) || this.EnableSunONRP != Utils.ParseBoolean((object) settings.ONRPSunEnabled) || this.ONRPEndTime != settings.ONRPEndTime || this.ONRPSatEndTime != settings.ONRPSatEndTime || this.ONRPSunEndTime != settings.ONRPSunEndTime;
    }
  }
}
