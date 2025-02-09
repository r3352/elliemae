// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.Log.OptionDefinition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Xml;
using System;
using System.Xml;

#nullable disable
namespace EllieMae.EMLite.DataEngine.Log
{
  [Serializable]
  public class OptionDefinition
  {
    private string _name;

    public OptionDefinition(string name) => this._name = name;

    public OptionDefinition(XmlElement e)
    {
      this._name = new AttributeReader(e).GetString(nameof (Name));
    }

    public string Name => this._name;

    public override string ToString() => this.Name;

    public void ToXml(XmlElement e) => new AttributeWriter(e).Write("Name", (object) this._name);
  }
}
