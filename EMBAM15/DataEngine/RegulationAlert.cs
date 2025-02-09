// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RegulationAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class RegulationAlert : StandardAlert
  {
    protected AlertTriggerFieldCollection triggerFields = new AlertTriggerFieldCollection();
    protected bool allowUserDefinedTriggerFields;

    public RegulationAlert(
      StandardAlertID alertId,
      string name,
      EncompassEdition appliesToEdition,
      AlertTiming timing,
      AlertNotificationType notificationType,
      bool allowUserDefinedTriggerFields,
      int displayOrder)
      : base(alertId, name, appliesToEdition, timing, notificationType, displayOrder)
    {
      this.allowUserDefinedTriggerFields = allowUserDefinedTriggerFields;
    }

    public override AlertCategory Category => AlertCategory.Regulation;

    public AlertTriggerFieldCollection TriggerFields => this.triggerFields;

    public bool AllowUserDefinedTriggerFields => this.allowUserDefinedTriggerFields;
  }
}
