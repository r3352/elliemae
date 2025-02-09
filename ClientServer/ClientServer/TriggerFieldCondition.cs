// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerFieldCondition
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
  public abstract class TriggerFieldCondition : TriggerCondition
  {
    private string fieldId;

    public TriggerFieldCondition(string fieldId) => this.fieldId = fieldId;

    public TriggerFieldCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.fieldId = info.GetString(nameof (fieldId));
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("fieldId", (object) this.fieldId);
    }

    public string FieldID => this.fieldId;

    public override string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      return new string[1]{ this.fieldId };
    }

    public override string ToString() => "Field: " + this.FieldID;
  }
}
