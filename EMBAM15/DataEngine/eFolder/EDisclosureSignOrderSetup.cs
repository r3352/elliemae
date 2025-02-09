// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.EDisclosureSignOrderSetup
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class EDisclosureSignOrderSetup : IXmlSerializable
  {
    private string attributeName;
    private string signorderEnabled;
    private Dictionary<string, bool> states;

    public EDisclosureSignOrderSetup()
    {
      this.attributeName = "DisclosureSigningOrder";
      this.signorderEnabled = "False";
      this.states = new Dictionary<string, bool>();
    }

    public EDisclosureSignOrderSetup(XmlSerializationInfo info)
    {
      this.attributeName = info.GetString(nameof (AttributeName));
      this.signorderEnabled = info.GetString("SignorderEnabled");
    }

    public string AttributeName => this.attributeName;

    public string SignOrderEnabled
    {
      get => this.signorderEnabled;
      set => this.signorderEnabled = value;
    }

    public Dictionary<string, bool> States
    {
      get => this.states;
      set => this.states = value;
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("AttributeName", (object) this.attributeName);
      info.AddValue("SignorderEnabled", (object) this.signorderEnabled);
    }
  }
}
