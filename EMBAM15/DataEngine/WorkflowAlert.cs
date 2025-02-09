// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.WorkflowAlert
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common.Licensing;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine
{
  [Serializable]
  public class WorkflowAlert : StandardAlert
  {
    private string triggerDescription = "";

    public WorkflowAlert(
      StandardAlertID alertId,
      string name,
      EncompassEdition appliesToEdition,
      AlertTiming timing,
      AlertNotificationType notificationType,
      int displayOrder,
      string triggerDescription)
      : base(alertId, name, appliesToEdition, timing, notificationType, displayOrder)
    {
      this.triggerDescription = triggerDescription;
    }

    public string TriggerDescription => this.triggerDescription;

    public override AlertCategory Category => AlertCategory.Workflow;

    public override bool SupportsCustomMessage => false;
  }
}
