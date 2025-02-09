// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.DuplicateScreenSetting
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  [Serializable]
  public class DuplicateScreenSetting : BinaryConvertibleObject
  {
    private static string versionStr = "V" + VersionInformation.CurrentVersion.OriginalVersion.FullVersion;
    private bool toDisplay = true;

    public DuplicateScreenSetting()
    {
    }

    public DuplicateScreenSetting(bool toDisplay) => this.toDisplay = toDisplay;

    public DuplicateScreenSetting(XmlSerializationInfo info)
    {
      try
      {
        this.toDisplay = (bool) info.GetValue(DuplicateScreenSetting.versionStr, typeof (bool));
      }
      catch
      {
      }
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue(DuplicateScreenSetting.versionStr, (object) this.toDisplay);
    }

    public bool ToDisplay => this.toDisplay;
  }
}
