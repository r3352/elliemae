// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ONRPEntitySettings
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ONRPEntitySettings : IEquatable<ONRPEntitySettings>
  {
    private bool useParentInfo;
    private bool enableONRP;
    private bool useChannelDefault;
    private bool continuousCoverage;
    private bool allowONRPForCancelledExpiredLocks;
    private bool weekendHolidayCoverage;
    private bool maximumLimit;
    private double dollarLimit;
    private int tolerance;
    private bool enableSatONRP;
    private bool enableSunONRP;
    private string oNRPStartTime;
    private string oNRPEndTime;
    private string oNRPSatStartTime;
    private string oNRPSatEndTime;
    private string oNRPSunStartTime;
    private string oNRPSunEndTime;
    public ONRPBaseRule rules;
    private LockDeskGlobalSettings globalSetting;
    public IONRPRuleHandler MessageHandler;

    public ONRPEntitySettings() => this.rules = new ONRPBaseRule(this);

    public ONRPEntitySettings(
      ONRPBaseRule rules,
      LockDeskGlobalSettings globalSettings,
      bool isNew = true)
      : this((IONRPRuleHandler) null, rules, globalSettings)
    {
      this.rules = rules;
      this.rules.Subject(this);
      if (((this.globalSetting == null ? 0 : (this.rules != null ? 1 : 0)) & (isNew ? 1 : 0)) == 0)
        return;
      this.useChannelDefault = true;
      this.CopyGlobalSettings();
    }

    public ONRPEntitySettings(
      IONRPRuleHandler msgHandler,
      ONRPBaseRule rules,
      LockDeskGlobalSettings globalSettings,
      bool setGlobals = true)
    {
      this.MessageHandler = msgHandler;
      this.rules = rules;
      this.rules.Subject(this);
      this.globalSetting = globalSettings;
      if (((this.globalSetting == null ? 0 : (this.rules != null ? 1 : 0)) & (setGlobals ? 1 : 0)) != 0)
        this.CopyGlobalSettings();
      this.SetValuesFromGlobalSettings();
    }

    public LockDeskGlobalSettings GlobalSettings => this.globalSetting;

    public void SetRules(
      IONRPRuleHandler msgHandler,
      ONRPBaseRule rules,
      LockDeskGlobalSettings globalSettings,
      bool useChannelDefault = true)
    {
      this.rules = rules;
      this.rules.Subject(this);
      this.MessageHandler = msgHandler;
      this.globalSetting = globalSettings;
      if (this.globalSetting != null && this.UseChannelDefault)
        this.CopyGlobalSettings();
      this.SetValuesFromGlobalSettings();
    }

    public bool UseParentInfo
    {
      get => this.useParentInfo;
      set => this.useParentInfo = value;
    }

    public bool EnableONRP
    {
      get => this.enableONRP;
      set
      {
        if (this.rules.EnableONRP)
          this.enableONRP = value;
        else
          this.HandleMessage("Enable ONRP for [BRANCH] is not ON, edits may not be made to field/fields.");
      }
    }

    public bool AllowONRPForCancelledExpiredLocks
    {
      get => this.allowONRPForCancelledExpiredLocks;
      set
      {
        this.CheckGlobalLevelONRP();
        this.allowONRPForCancelledExpiredLocks = value;
      }
    }

    public bool UseChannelDefault
    {
      get => this.useChannelDefault;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.rules.UseChannelDefaultsAndCustomize)
        {
          this.useChannelDefault = value;
          if (this.globalSetting == null || !this.useChannelDefault)
            return;
          this.CopyGlobalSettings();
        }
        else if (value)
          this.HandleMessage("Use Channel Defaults cannot be edited when Branch has not been enabled.");
        else
          this.HandleMessage("Customize Settings cannot be edited when Branch has not been enabled.");
      }
    }

    public bool ContinuousCoverage
    {
      get => this.continuousCoverage;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
        {
          if (value)
            this.HandleMessage("Continuous ONRP Coverage cannot be edited when Use Channel Defaults is ON");
          else
            this.HandleMessage("Specify Times cannot be edited when Use Channel Defaults is ON");
        }
        if (this.rules.ContinuousCoverageAndSpecifyTime)
        {
          this.continuousCoverage = value;
          if (this.continuousCoverage)
          {
            this.oNRPStartTime = "";
            this.oNRPEndTime = "";
            this.enableSatONRP = false;
            this.oNRPSatStartTime = "";
            this.oNRPSatEndTime = "";
            this.enableSunONRP = false;
            this.oNRPSunStartTime = "";
            this.oNRPSunEndTime = "";
            this.weekendHolidayCoverage = false;
          }
          if (this.continuousCoverage || this.GlobalSettings == null)
            return;
          this.oNRPStartTime = this.GlobalSettings.LockDeskEndTime;
        }
        else if (value)
          this.HandleMessage("Continuous ONRP Coverage can only be edited when Customize Settings is ON.");
        else
          this.HandleMessage("Specify Times can only be edited when Customize Settings is ON.");
      }
    }

    public bool WeekendHolidayCoverage
    {
      get => this.weekendHolidayCoverage;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("Weekend/Holiday Coverage cannot be edited when Use Channel Defaults is ON");
        if (this.rules.WeekendHolidayCoverage)
          this.weekendHolidayCoverage = value;
        else
          this.HandleMessage("Weekend/Holiday Coverage can only be edited when Specify Times is ON and Saturday and Sunday lock desk is turned OFF.");
      }
    }

    public bool MaximumLimit
    {
      get => this.maximumLimit;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("No Maximum Limit cannot be edited when Use Channel Defaults is ON");
        if (this.rules.NoMaxLimit)
        {
          this.maximumLimit = value;
          if (!this.maximumLimit)
            return;
          this.tolerance = 0;
          this.dollarLimit = 0.0;
        }
        else
          this.HandleMessage("No Maximum Limit can only be edited when Customize Settings is ON.");
      }
    }

    public string ONRPStartTime
    {
      get => this.continuousCoverage || this.rules.TwentyfourhrWeekday ? "" : this.oNRPStartTime;
      set => this.oNRPStartTime = value;
    }

    public string ONRPEndTime
    {
      get => this.oNRPEndTime;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP End Time cannot be edited when Use Channel Defaults is ON");
        if (this.rules.EndTime)
          this.oNRPEndTime = value;
        else
          this.HandleMessage("ONRP Weekday End Time can only be edited when Specify Settings is ON or lock desk weekday is not set to 24 hours.");
      }
    }

    public string ONRPSatStartTime
    {
      get => this.EnableSatONRP ? this.oNRPSatStartTime : "";
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Saturday Start Time cannot be edited when Use Channel Defaults is ON");
        if (this.rules.SatStartTime)
          this.oNRPSatStartTime = value;
        else
          this.HandleMessage("ONRP Saturday Start Time can only be edited when Specify Settings is ON and Saturday lock desk is turned ON without 24 hours.");
      }
    }

    public string ONRPSatEndTime
    {
      get => this.oNRPSatEndTime;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Saturday End Time cannot be edited when Use Channel Defaults is ON");
        if (this.rules.SatEndTime)
          this.oNRPSatEndTime = value;
        else
          this.HandleMessage("ONRP Saturday End Time can only be edited when Specify Settings is ON and Saturday lock desk is turned ON without 24 hours.");
      }
    }

    public string ONRPSunStartTime
    {
      get => this.EnableSunONRP ? this.oNRPSunStartTime : "";
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Sunday Start Time cannot be edited when Use Channel Defaults is ON");
        if (this.rules.SunStartTime)
          this.oNRPSunStartTime = value;
        else
          this.HandleMessage("ONRP Sunday Start Time can only be edited when Specify Settings is ON and Sunday lock desk is turned ON without 24 hours.");
      }
    }

    public string ONRPSunEndTime
    {
      get => this.oNRPSunEndTime;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Sunday End Time cannot be edited when Use Channel Defaults is ON");
        if (this.rules.SunEndTime)
          this.oNRPSunEndTime = value;
        else
          this.HandleMessage("ONRP Sunday End Time can only be edited when Specify Settings is ON and Sunday lock desk is turned ON without 24 hours.");
      }
    }

    public bool EnableSatONRP
    {
      get => this.enableSatONRP;
      set
      {
        if (this.rules.EnableSat)
        {
          this.enableSatONRP = value;
          if (!this.enableSatONRP)
            this.oNRPSatStartTime = this.oNRPSatEndTime = "";
          if (!this.enableSatONRP || this.GlobalSettings == null)
            return;
          this.oNRPSatStartTime = this.GlobalSettings.LockDeskEndTimeSat;
        }
        else
          this.HandleMessage("Enable Saturday ONRP for [BRANCH] is not ON or is set for 24 hours, edits may not be made to field/fields.");
      }
    }

    public bool EnableSunONRP
    {
      get => this.enableSunONRP;
      set
      {
        if (this.rules.EnableSun)
        {
          this.enableSunONRP = value;
          if (!this.enableSunONRP)
            this.oNRPSunStartTime = this.oNRPSunEndTime = "";
          if (!this.enableSunONRP || this.GlobalSettings == null)
            return;
          this.oNRPSunStartTime = this.GlobalSettings.LockDeskEndTimeSun;
        }
        else
          this.HandleMessage("Enable Sunday ONRP for [BRANCH] is not ON or is set for 24 hours, edits may not be made to field/fields.");
      }
    }

    public double DollarLimit
    {
      get => this.dollarLimit;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Dollar Limit cannot be edited when Use Channel Defaults is ON");
        if (this.rules.DollarLimit)
        {
          this.dollarLimit = value;
          if (this.dollarLimit > 99999999.0)
            this.HandleMessage("ONRP Dollar limit cannot be greater than 99999999.");
          if (this.dollarLimit > 0.0)
            return;
          this.HandleMessage("ORNP Dollar Limit may not be blank or zero when No Maximum Limit is OFF.");
        }
        else
          this.HandleMessage("ONRP Dollar Limit may not be set when No Maximum Limit is ON.");
      }
    }

    public int Tolerance
    {
      get => this.tolerance;
      set
      {
        this.CheckGlobalLevelONRP();
        if (this.UseChannelDefault)
          this.HandleMessage("ONRP Tolerance cannot be edited when Use Channel Defaults is ON");
        if (this.rules.Tolerance)
        {
          this.tolerance = value;
          if (this.tolerance <= 99)
            return;
          this.HandleMessage("ONRP Tolerance cannot be greater than 99.");
        }
        else
          this.HandleMessage("ONRP Tolerance may not be set when No Maximum Limit is ON.");
      }
    }

    public ONRPEntitySettings Clone()
    {
      return new ONRPEntitySettings()
      {
        EnableONRP = this.EnableONRP,
        ContinuousCoverage = this.ContinuousCoverage,
        AllowONRPForCancelledExpiredLocks = this.AllowONRPForCancelledExpiredLocks,
        DollarLimit = this.DollarLimit,
        MaximumLimit = this.MaximumLimit,
        ONRPStartTime = this.ONRPStartTime,
        ONRPEndTime = this.oNRPEndTime,
        EnableSatONRP = this.enableSatONRP,
        EnableSunONRP = this.enableSunONRP,
        ONRPSatStartTime = this.oNRPSatStartTime,
        ONRPSatEndTime = this.oNRPSatEndTime,
        ONRPSunStartTime = this.oNRPSunStartTime,
        ONRPSunEndTime = this.oNRPSunEndTime,
        Tolerance = this.Tolerance,
        UseChannelDefault = this.UseChannelDefault,
        WeekendHolidayCoverage = this.WeekendHolidayCoverage,
        UseParentInfo = this.UseParentInfo
      };
    }

    public ONRPEntitySettings Clone(
      IONRPRuleHandler msgHandler,
      ONRPBaseRule rules,
      LockDeskGlobalSettings globalSettings)
    {
      ONRPEntitySettings onrpEntitySettings = new ONRPEntitySettings()
      {
        EnableONRP = this.EnableONRP,
        ContinuousCoverage = this.ContinuousCoverage,
        AllowONRPForCancelledExpiredLocks = this.AllowONRPForCancelledExpiredLocks,
        DollarLimit = this.DollarLimit,
        MaximumLimit = this.MaximumLimit,
        ONRPStartTime = this.ONRPStartTime,
        ONRPEndTime = this.oNRPEndTime,
        EnableSatONRP = this.enableSatONRP,
        EnableSunONRP = this.enableSunONRP,
        ONRPSatStartTime = this.oNRPSatStartTime,
        ONRPSatEndTime = this.oNRPSatEndTime,
        ONRPSunStartTime = this.oNRPSunStartTime,
        ONRPSunEndTime = this.oNRPSunEndTime,
        Tolerance = this.Tolerance,
        UseChannelDefault = this.UseChannelDefault,
        WeekendHolidayCoverage = this.WeekendHolidayCoverage,
        UseParentInfo = this.UseParentInfo
      };
      onrpEntitySettings.SetRules(msgHandler, rules, globalSettings);
      return onrpEntitySettings;
    }

    public static TimeSpan ConvertToTimeSpan(int hours, int minutes, string AMPM)
    {
      return DateTime.Parse(hours.ToString() + ":" + minutes.ToString() + " " + AMPM).TimeOfDay;
    }

    public static DateTime ConverToDateTime(string time)
    {
      return time == string.Empty ? new DateTime() : Convert.ToDateTime(time);
    }

    private void CheckGlobalLevelONRP()
    {
      if (this.rules.EnableONRP)
        return;
      this.HandleMessage("Enable ONRP for [BRANCH] is not ON, edits may not be made to field/fields.");
    }

    private void HandleMessage(string s)
    {
      if (this.MessageHandler == null)
        return;
      this.MessageHandler.MessageHandler(s);
    }

    public void SetMessageHandler(IONRPRuleHandler handler) => this.MessageHandler = handler;

    private void CopyGlobalSettings()
    {
      this.continuousCoverage = this.globalSetting.ContinuousCoverage != "Specify";
      this.oNRPEndTime = string.IsNullOrEmpty(this.globalSetting.ONRPEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.ONRPEndTime).ToString("HH\\:mm");
      this.oNRPStartTime = string.IsNullOrEmpty(this.globalSetting.LockDeskEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.LockDeskEndTime).ToString("HH\\:mm");
      this.enableSatONRP = this.globalSetting.ONRPSatEnabled == "True";
      this.enableSunONRP = this.globalSetting.ONRPSunEnabled == "True";
      this.oNRPSatStartTime = string.IsNullOrEmpty(this.globalSetting.LockDeskEndTimeSat) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.LockDeskEndTimeSat).ToString("HH\\:mm");
      this.oNRPSatEndTime = string.IsNullOrEmpty(this.globalSetting.ONRPSatEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.ONRPSatEndTime).ToString("HH\\:mm");
      this.oNRPSunStartTime = string.IsNullOrEmpty(this.globalSetting.LockDeskEndTimeSun) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.LockDeskEndTimeSun).ToString("HH\\:mm");
      this.oNRPSunEndTime = string.IsNullOrEmpty(this.globalSetting.ONRPSunEndTime) ? "" : ONRPEntitySettings.ConverToDateTime(this.globalSetting.ONRPSunEndTime).ToString("HH\\:mm");
      this.weekendHolidayCoverage = this.globalSetting.WeekendHoliday == "True";
      this.maximumLimit = this.globalSetting.NoMaxLimit == "True";
      this.dollarLimit = Utils.ParseDouble((object) this.globalSetting.DollarLimit, 0.0);
      this.tolerance = Utils.ParseInt((object) this.globalSetting.Tolerance, 0);
    }

    private void SetValuesFromGlobalSettings()
    {
      if (this.globalSetting == null || this.UseChannelDefault)
        return;
      DateTime dateTime1 = ONRPUtils.ONRPStartTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTime), Utils.ToDate(this.GlobalSettings.LockDeskEndTime));
      this.oNRPStartTime = dateTime1 == DateTime.MinValue || this.ContinuousCoverage ? "" : dateTime1.ToString("HH\\:mm");
      DateTime dateTime2 = ONRPUtils.ONRPEndTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTime), Utils.ToDate(this.GlobalSettings.LockDeskEndTime), Utils.ToDate(this.oNRPEndTime), this.ContinuousCoverage, this.EnableONRP);
      this.oNRPEndTime = !(dateTime2 == DateTime.MinValue) ? dateTime2.ToString("HH\\:mm") : "";
      this.enableSatONRP = bool.Parse(this.globalSetting.EnableLockDeskSat) && this.enableSatONRP;
      DateTime dateTime3 = ONRPUtils.ONRPStartTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTimeSat), Utils.ToDate(this.GlobalSettings.LockDeskEndTimeSat));
      this.oNRPSatStartTime = dateTime3 == DateTime.MinValue || this.GlobalSettings.EnableLockDeskSat.ToLower() != "true" ? "" : dateTime3.ToString("HH\\:mm");
      DateTime dateTime4 = ONRPUtils.ONRPEndTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTimeSat), Utils.ToDate(this.GlobalSettings.LockDeskEndTimeSat), Utils.ToDate(this.oNRPSatEndTime), this.ContinuousCoverage, this.EnableONRP);
      this.oNRPSatEndTime = dateTime4 == DateTime.MinValue || this.GlobalSettings.EnableLockDeskSat.ToLower() != "true" ? "" : dateTime4.ToString("HH\\:mm");
      this.enableSunONRP = bool.Parse(this.globalSetting.EnableLockDeskSun) && this.enableSunONRP;
      DateTime dateTime5 = ONRPUtils.ONRPStartTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTimeSun), Utils.ToDate(this.GlobalSettings.LockDeskEndTimeSun));
      this.oNRPSunStartTime = dateTime5 == DateTime.MinValue || this.GlobalSettings.EnableLockDeskSun.ToLower() != "true" ? "" : dateTime5.ToString("HH\\:mm");
      DateTime dateTime6 = ONRPUtils.ONRPEndTime(Utils.ToDate(this.GlobalSettings.LockDeskStartTimeSun), Utils.ToDate(this.GlobalSettings.LockDeskEndTimeSun), Utils.ToDate(this.oNRPSunEndTime), this.ContinuousCoverage, this.EnableONRP);
      this.oNRPSunEndTime = dateTime6 == DateTime.MinValue || this.GlobalSettings.EnableLockDeskSun.ToLower() != "true" ? "" : dateTime6.ToString("HH\\:mm");
      this.weekendHolidayCoverage = !this.enableSatONRP && !this.enableSunONRP && !(this.globalSetting.ContinuousCoverage.ToLower() == "continuous") && !(this.globalSetting.EnableLockDeskSat.ToLower() == "true") && !(this.globalSetting.EnableLockDeskSun.ToLower() == "true") && this.weekendHolidayCoverage;
    }

    public bool Equals(ONRPEntitySettings other)
    {
      return this.useParentInfo == other.useParentInfo && this.enableONRP == other.enableONRP && this.useChannelDefault == other.useChannelDefault && this.continuousCoverage == other.continuousCoverage && this.AllowONRPForCancelledExpiredLocks == other.AllowONRPForCancelledExpiredLocks && this.weekendHolidayCoverage == other.weekendHolidayCoverage && this.maximumLimit == other.maximumLimit && this.dollarLimit == other.dollarLimit && this.tolerance == other.tolerance && this.enableSatONRP == other.enableSatONRP && this.enableSunONRP == other.enableSunONRP && this.oNRPStartTime == other.oNRPStartTime && this.oNRPEndTime == other.ONRPEndTime && this.oNRPSatStartTime == other.ONRPSatStartTime && this.oNRPSatEndTime == other.ONRPSatEndTime && this.oNRPSunStartTime == other.ONRPSunStartTime && this.oNRPSunEndTime == other.ONRPSunEndTime;
    }

    public bool NeedClearAccruedAmount(ONRPEntitySettings other)
    {
      return this.enableONRP != other.enableONRP || this.continuousCoverage != other.ContinuousCoverage || this.weekendHolidayCoverage != other.WeekendHolidayCoverage || this.AllowONRPForCancelledExpiredLocks != other.AllowONRPForCancelledExpiredLocks || this.maximumLimit != other.MaximumLimit || this.dollarLimit != other.DollarLimit || this.tolerance != other.Tolerance || this.enableSatONRP != other.EnableSatONRP || this.enableSunONRP != other.EnableSunONRP || this.oNRPStartTime != other.ONRPStartTime || this.oNRPEndTime != other.ONRPEndTime || this.oNRPSatStartTime != other.ONRPSatStartTime || this.oNRPSatEndTime != other.ONRPSatEndTime || this.oNRPSunStartTime != other.ONRPSunStartTime || this.oNRPSunEndTime != other.ONRPSunEndTime;
    }
  }
}
