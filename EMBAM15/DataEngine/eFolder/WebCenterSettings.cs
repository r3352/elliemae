// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.DataEngine.eFolder.WebCenterSettings
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.DataEngine.eFolder
{
  [Serializable]
  public class WebCenterSettings : IXmlSerializable
  {
    private bool useSameWebcenter;

    public WebCenterSettings() => this.useSameWebcenter = false;

    public WebCenterSettings(XmlSerializationInfo info)
    {
      this.useSameWebcenter = info.GetBoolean(nameof (UseSameWebcenter), false);
    }

    public void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue("UseSameWebcenter", (object) this.useSameWebcenter);
    }

    public bool UseSameWebcenter
    {
      get => this.useSameWebcenter;
      set => this.useSameWebcenter = value;
    }
  }
}
