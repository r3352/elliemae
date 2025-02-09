// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerAssignment
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class TriggerAssignment : IXmlSerializable
  {
    private string fieldId;
    private string expression;
    private bool evaluate;

    public TriggerAssignment(string fieldId, string expression, bool evaluate)
    {
      this.fieldId = fieldId;
      this.expression = expression;
      this.evaluate = evaluate;
    }

    public TriggerAssignment(XmlSerializationInfo info)
    {
      this.fieldId = info.GetString(nameof (fieldId));
      this.expression = info.GetString("expr");
      this.evaluate = info.GetBoolean("eval");
    }

    public string FieldID => this.fieldId;

    public string Expression => this.expression;

    public bool Evaluate => this.evaluate;

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("fieldId", (object) this.fieldId);
      info.AddValue("expr", (object) this.expression);
      info.AddValue("eval", (object) this.evaluate);
    }
  }
}
