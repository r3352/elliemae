// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerCondition
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
  public abstract class TriggerCondition : IXmlSerializable
  {
    public TriggerCondition()
    {
    }

    public TriggerCondition(XmlSerializationInfo info)
    {
    }

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
    }

    public abstract string[] GetActivationFields(ConfigInfoForTriggers activationData);

    public abstract TriggerConditionType ConditionType { get; }
  }
}
