// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.RegulationAlertWithDataCompletionFields
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public sealed class RegulationAlertWithDataCompletionFields : RegulationAlert
  {
    private bool allowCustomDataCompletionFields;

    public bool AllowCustomDataCompletionFields => this.allowCustomDataCompletionFields;

    public RegulationAlertWithDataCompletionFields(
      StandardAlertID alertId,
      string name,
      EncompassEdition appliesToEdition,
      AlertTiming timing,
      AlertNotificationType notificationType,
      bool allowUserDefinedTriggerFields,
      int displayOrder,
      bool allowCustomDataCompletionFields)
      : base(alertId, name, appliesToEdition, timing, notificationType, allowUserDefinedTriggerFields, displayOrder)
    {
      this.allowCustomDataCompletionFields = allowCustomDataCompletionFields;
    }

    public override object Clone()
    {
      RegulationAlertWithDataCompletionFields completionFields = new RegulationAlertWithDataCompletionFields(this.StandardAlertID, this.Name, this.AppliesToEncompassEdition, this.GetAlertTiming((AlertConfig) null), this.NotificationType, this.AllowUserDefinedTriggerFields, this.DisplayOrder, this.AllowCustomDataCompletionFields);
      completionFields.triggerFields = this.TriggerFields.Clone();
      return (object) completionFields;
    }
  }
}
