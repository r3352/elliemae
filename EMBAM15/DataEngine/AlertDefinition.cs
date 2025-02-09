// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.AlertDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public abstract class AlertDefinition : IComparable<AlertDefinition>, ICloneable
  {
    private int alertId;
    private string name;

    public AlertDefinition(int alertId, string name)
    {
      this.alertId = alertId;
      this.name = name;
    }

    public int AlertID => this.alertId;

    public virtual string Name
    {
      get => this.name;
      set => this.name = value;
    }

    public virtual bool SupportsCustomMessage => true;

    public abstract AlertCategory Category { get; }

    public abstract AlertTiming GetAlertTiming(AlertConfig config);

    public virtual AlertNotificationType NotificationType => AlertNotificationType.None;

    protected virtual int DisplayOrder => -1;

    public virtual bool AppliesToEdition(EncompassEdition edition) => true;

    public virtual string GetCriterionName() => "Alert." + this.name + ".AlertCount";

    public int CompareTo(AlertDefinition other)
    {
      if (other == null)
        return 1;
      if (this.Category != other.Category)
        return this.Category - other.Category;
      return this.DisplayOrder != other.DisplayOrder ? this.DisplayOrder - other.DisplayOrder : string.Compare(this.Name, other.Name);
    }

    public virtual object Clone() => this.MemberwiseClone();
  }
}
