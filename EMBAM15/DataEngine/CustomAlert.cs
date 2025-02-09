// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.CustomAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class CustomAlert : AlertDefinition
  {
    private string alertGuid;
    private DayType dayType = DayType.Calendar;
    private int dateAdjustment;
    private bool allowToClear;
    private string conditionXml;

    public CustomAlert(
      int alertId,
      string alertGuid,
      string name,
      int dateAdjustment,
      DayType adjustmentType,
      bool allowToClear,
      string conditionXml)
      : base(alertId, name)
    {
      this.alertGuid = alertGuid;
      this.conditionXml = conditionXml;
      this.dateAdjustment = dateAdjustment;
      this.dayType = adjustmentType;
      this.allowToClear = allowToClear;
    }

    public CustomAlert()
      : base(-1, "")
    {
      this.alertGuid = System.Guid.NewGuid().ToString();
    }

    public override AlertCategory Category => AlertCategory.Custom;

    public override AlertTiming GetAlertTiming(AlertConfig config)
    {
      return config.TriggerFieldList.Count <= 0 ? AlertTiming.Immediate : AlertTiming.DaysBefore;
    }

    public string Guid => this.alertGuid;

    public int DateAdjustment
    {
      get => this.dateAdjustment;
      set => this.dateAdjustment = value;
    }

    public DayType AdjustmentDayType
    {
      get => this.dayType;
      set => this.dayType = value;
    }

    public bool AllowToClear
    {
      get => string.IsNullOrEmpty(this.conditionXml) || this.allowToClear;
      set => this.allowToClear = value;
    }

    public string ConditionXml
    {
      get => this.conditionXml;
      set => this.conditionXml = value;
    }
  }
}
