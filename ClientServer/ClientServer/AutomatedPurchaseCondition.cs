// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AutomatedPurchaseCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AutomatedPurchaseCondition : IXmlSerializable
  {
    public readonly string ConditionName;
    public readonly ConditionType ConditionType;

    public AutomatedPurchaseCondition(ConditionType conditionType, string conditionName)
    {
      this.ConditionType = conditionType;
      this.ConditionName = conditionName;
    }

    public AutomatedPurchaseCondition(XmlSerializationInfo info)
    {
      this.ConditionType = (ConditionType) Enum.Parse(typeof (ConditionType), info.GetString(nameof (ConditionType)));
      this.ConditionName = info.GetString(nameof (ConditionName));
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("ConditionType", (object) this.ConditionType.ToString());
      info.AddValue("ConditionName", (object) this.ConditionName);
    }
  }
}
