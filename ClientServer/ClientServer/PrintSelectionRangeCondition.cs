// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionRangeCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintSelectionRangeCondition : PrintSelectionCondition
  {
    private string minValue;
    private string maxValue;

    public PrintSelectionRangeCondition(string fieldId, string minValue, string maxValue)
      : base(fieldId)
    {
      this.minValue = minValue;
      this.maxValue = maxValue;
    }

    public PrintSelectionRangeCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.minValue = info.GetString("min");
      this.maxValue = info.GetString("max");
    }

    public string Minimum => this.minValue;

    public string Maximum => this.maxValue;

    public override PrintSelectionConditionType ConditionType => PrintSelectionConditionType.Range;

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("min", (object) this.minValue);
      info.AddValue("max", (object) this.maxValue);
    }
  }
}
