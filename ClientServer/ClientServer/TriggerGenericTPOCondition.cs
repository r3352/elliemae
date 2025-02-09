// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerGenericTPOCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerGenericTPOCondition : TriggerCondition
  {
    private TriggerConditionType triggerConditionType = TriggerConditionType.FixedValue;

    public TriggerGenericTPOCondition(TriggerConditionType triggerConditionType)
    {
      this.triggerConditionType = triggerConditionType;
    }

    public TriggerGenericTPOCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.triggerConditionType = (TriggerConditionType) info.GetInteger(nameof (triggerConditionType));
    }

    public override string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      return new string[0];
    }

    public override string ToString() => "Condition : " + (object) (int) this.triggerConditionType;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("triggerConditionType", (object) (int) this.triggerConditionType);
    }

    public override TriggerConditionType ConditionType => this.triggerConditionType;
  }
}
