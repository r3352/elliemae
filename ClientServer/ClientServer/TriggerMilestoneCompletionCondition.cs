// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerMilestoneCompletionCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Serialization;
using EllieMae.EMLite.Workflow;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerMilestoneCompletionCondition : TriggerCondition
  {
    private string milestoneId;

    public TriggerMilestoneCompletionCondition(string milestoneId)
    {
      this.milestoneId = milestoneId;
    }

    public TriggerMilestoneCompletionCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.milestoneId = info.GetString(nameof (milestoneId));
    }

    public string MilestoneID => this.milestoneId;

    public override string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      Milestone milestone = activationData.MilestonesList.Find((Predicate<Milestone>) (item => item.MilestoneID == this.milestoneId));
      if (milestone == null)
        return new string[0];
      return new string[1]
      {
        "Log.MS.Status." + milestone.Name
      };
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("milestoneId", (object) this.milestoneId);
    }

    public override TriggerConditionType ConditionType => TriggerConditionType.MilestoneCompleted;

    public override string ToString() => "Milestone Completed: " + this.milestoneId;
  }
}
