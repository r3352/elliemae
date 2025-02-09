// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.StandardAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public abstract class StandardAlert : AlertDefinition
  {
    private int displayOrder = -1;
    private AlertTiming timing;
    private AlertNotificationType notificationType;
    private EncompassEdition appliesToEdition;

    public StandardAlert(
      StandardAlertID alertId,
      string name,
      EncompassEdition appliesToEdition,
      AlertTiming timing,
      AlertNotificationType notificationType,
      int displayOrder)
      : base((int) alertId, name)
    {
      this.timing = timing;
      this.displayOrder = displayOrder;
      this.notificationType = notificationType;
      this.appliesToEdition = appliesToEdition;
    }

    public StandardAlertID StandardAlertID => (StandardAlertID) this.AlertID;

    public override string Name
    {
      get => base.Name;
      set => throw new NotSupportedException("Name cannot be changed for standard alert");
    }

    public override AlertTiming GetAlertTiming(AlertConfig config) => this.timing;

    public override AlertNotificationType NotificationType => this.notificationType;

    public EncompassEdition AppliesToEncompassEdition => this.appliesToEdition;

    protected override int DisplayOrder => this.displayOrder;

    public override bool AppliesToEdition(EncompassEdition edition)
    {
      return this.appliesToEdition == EncompassEdition.None || this.appliesToEdition == edition;
    }

    public override string GetCriterionName()
    {
      return "Alert." + this.StandardAlertID.ToString() + ".AlertCount";
    }
  }
}
