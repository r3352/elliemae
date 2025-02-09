// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionFixedValueCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintSelectionFixedValueCondition : PrintSelectionCondition
  {
    private string value;

    public PrintSelectionFixedValueCondition(string fieldId, string value)
      : base(fieldId)
    {
      this.value = value;
    }

    public PrintSelectionFixedValueCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.value = info.GetString(nameof (value));
    }

    public string Value => this.value;

    public override PrintSelectionConditionType ConditionType
    {
      get => PrintSelectionConditionType.FixedValue;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("value", (object) this.value);
    }
  }
}
