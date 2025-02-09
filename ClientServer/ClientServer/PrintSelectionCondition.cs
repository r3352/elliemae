// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionCondition
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public abstract class PrintSelectionCondition : IXmlSerializable
  {
    private string fieldId;

    public PrintSelectionCondition(string fieldId) => this.fieldId = fieldId;

    public PrintSelectionCondition(XmlSerializationInfo info)
    {
      this.fieldId = info.GetString(nameof (fieldId));
    }

    public virtual void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("fieldId", (object) this.fieldId);
    }

    public string FieldID => this.fieldId;

    public string ToXml() => new XmlSerializer().Serialize((object) this);

    public abstract PrintSelectionConditionType ConditionType { get; }
  }
}
