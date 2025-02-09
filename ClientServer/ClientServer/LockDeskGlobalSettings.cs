// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LockDeskGlobalSettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class LockDeskGlobalSettings
  {
    private IDictionary policySettings;
    private string channelString = string.Empty;
    private string channel = string.Empty;
    private LoanChannel loanChannel;

    public LockDeskGlobalSettings(IDictionary settings, LoanChannel loanChannel)
    {
      this.policySettings = settings;
      this.Channel = loanChannel;
    }

    public LoanChannel Channel
    {
      get => this.loanChannel;
      set
      {
        this.loanChannel = value;
        switch (this.loanChannel)
        {
          case LoanChannel.BankedRetail:
            this.channelString = "RET";
            this.channel = "Retail";
            break;
          case LoanChannel.BankedWholesale:
            this.channelString = "BROKER";
            this.channel = "Broker";
            break;
          case LoanChannel.Correspondent:
            this.channelString = "COR";
            this.channel = "Correspondent";
            break;
        }
      }
    }

    public IDictionary PolicySettings => this.policySettings;

    public string ChannelString => this.channelString;

    public string BranchChannel => this.channel;

    public string IsEncompassLockDeskHoursEnabled
    {
      get => this.GetPropertySetting("EnableLockDeskSCHEDULE");
    }

    public string ONRPEnabled
    {
      get => this.GetPropertySetting("EnableONRP" + this.channelString);
      set => this.SetPropertySetting("EnableONRP" + this.channelString, value);
    }

    public string AllowONRPForCancelledExpiredLocks
    {
      get => this.GetPropertySetting("ONRPCancelledExpiredLocks" + this.channelString);
      set => this.SetPropertySetting("ONRPCancelledExpiredLocks" + this.channelString, value);
    }

    public string ContinuousCoverage
    {
      get => this.GetPropertySetting(string.Format("ONRP{0}Cvrg", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}Cvrg", (object) this.channelString), value);
      }
    }

    public string LockDeskStartTime
    {
      get => this.GetPropertySetting(string.Format("{0}LDSTRTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSTRTIME", (object) this.channelString), value);
      }
    }

    public string LockDeskEndTime
    {
      get => this.GetPropertySetting(string.Format("{0}LDENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDENDTIME", (object) this.channelString), value);
      }
    }

    public string LockDeskStartTimeSat
    {
      get => this.GetPropertySetting(string.Format("{0}LDSATSTRTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSATSTRTIME", (object) this.channelString), value);
      }
    }

    public string LockDeskEndTimeSat
    {
      get => this.GetPropertySetting(string.Format("{0}LDSATENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSATENDTIME", (object) this.channelString), value);
      }
    }

    public string LockDeskStartTimeSun
    {
      get => this.GetPropertySetting(string.Format("{0}LDSUNSTRTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSUNSTRTIME", (object) this.channelString), value);
      }
    }

    public string LockDeskEndTimeSun
    {
      get => this.GetPropertySetting(string.Format("{0}LDSUNENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSUNENDTIME", (object) this.channelString), value);
      }
    }

    public string EnableLockDeskSat
    {
      get => this.GetPropertySetting(string.Format("ENABLELD{0}SAT", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ENABLELD{0}SAT", (object) this.channelString), value);
      }
    }

    public string EnableLockDeskSun
    {
      get => this.GetPropertySetting(string.Format("ENABLELD{0}SUN", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ENABLELD{0}SUN", (object) this.channelString), value);
      }
    }

    public string ONRPStartTime
    {
      get => this.GetPropertySetting(string.Format("{0}LDENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDENDTIME", (object) this.channelString), value);
      }
    }

    public string ONRPSatStartTime
    {
      get => this.GetPropertySetting(string.Format("{0}LDSATSTRTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSATSTRTIME", (object) this.channelString), value);
      }
    }

    public string ONRPSunStartTime
    {
      get => this.GetPropertySetting(string.Format("{0}LDSUNENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("{0}LDSUNENDTIME", (object) this.channelString), value);
      }
    }

    public string ONRPEndTime
    {
      get => this.GetPropertySetting(string.Format("ONRP{0}ENDTIME", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}ENDTIME", (object) this.channelString), value);
      }
    }

    public string ONRPSatEndTime
    {
      get
      {
        return this.GetPropertySetting(string.Format("ONRP{0}SATENDTIME", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}SATENDTIME", (object) this.channelString), value);
      }
    }

    public string ONRPSunEndTime
    {
      get
      {
        return this.GetPropertySetting(string.Format("ONRP{0}SUNENDTIME", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}SUNENDTIME", (object) this.channelString), value);
      }
    }

    public string ONRPSatEnabled
    {
      get
      {
        return this.GetPropertySetting(string.Format("ENABLEONRP{0}SAT", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ENABLEONRP{0}SAT", (object) this.channelString), value);
      }
    }

    public string ONRPSunEnabled
    {
      get
      {
        return this.GetPropertySetting(string.Format("ENABLEONRP{0}Sun", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ENABLEONRP{0}Sun", (object) this.channelString), value);
      }
    }

    public string WeekendHoliday
    {
      get
      {
        return this.GetPropertySetting(string.Format("ENABLEONRPWH{0}CVRG", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ENABLEONRPWH{0}CVRG", (object) this.channelString), value);
      }
    }

    public string NoMaxLimit
    {
      get
      {
        return this.GetPropertySetting(string.Format("ONRPNOMAXLIMIT{0}", (object) this.channelString));
      }
      set
      {
        this.SetPropertySetting(string.Format("ONRPNOMAXLIMIT{0}", (object) this.channelString), value);
      }
    }

    public string DollarLimit
    {
      get => this.GetPropertySetting(string.Format("ONRP{0}DOLLIMIT", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}DOLLIMIT", (object) this.channelString), value);
      }
    }

    public string Tolerance
    {
      get => this.GetPropertySetting(string.Format("ONRP{0}DOLTOL", (object) this.channelString));
      set
      {
        this.SetPropertySetting(string.Format("ONRP{0}DOLTOL", (object) this.channelString), value);
      }
    }

    private string GetPropertySetting(string propName)
    {
      return this.policySettings == null || !this.policySettings.Contains((object) ("Policies." + propName)) ? string.Empty : this.policySettings[(object) ("Policies." + propName)].ToString();
    }

    private void SetPropertySetting(string propName, string propValue)
    {
      if (this.policySettings == null)
        return;
      if (this.policySettings.Contains((object) ("Policies." + propName)))
        this.policySettings[(object) ("Policies." + propName)] = (object) propValue;
      else
        this.policySettings.Add((object) ("Policies." + propName), (object) propValue);
    }
  }
}
