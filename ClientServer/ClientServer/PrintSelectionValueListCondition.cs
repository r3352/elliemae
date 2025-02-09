// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionValueListCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PrintSelectionValueListCondition : PrintSelectionCondition
  {
    private string[] values;

    public PrintSelectionValueListCondition(string fieldId, string[] values)
      : base(fieldId)
    {
      this.values = values;
    }

    public PrintSelectionValueListCondition(XmlSerializationInfo info)
      : base(info)
    {
      this.values = ((List<string>) info.GetValue(nameof (values), typeof (XmlList<string>))).ToArray();
    }

    public string[] Values => this.values;

    public override PrintSelectionConditionType ConditionType
    {
      get => PrintSelectionConditionType.ValueList;
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("values", (object) new XmlList<string>((IEnumerable<string>) this.values));
    }
  }
}
