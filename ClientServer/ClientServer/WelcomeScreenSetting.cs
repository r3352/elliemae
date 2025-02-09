// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.WelcomeScreenSetting
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class WelcomeScreenSetting : BinaryConvertibleObject
  {
    private static string versionStr = "V" + VersionInformation.CurrentVersion.OriginalVersion.FullVersion;
    private bool toDisplay = true;

    public WelcomeScreenSetting()
    {
    }

    public WelcomeScreenSetting(bool toDisplay) => this.toDisplay = toDisplay;

    public WelcomeScreenSetting(XmlSerializationInfo info)
    {
      try
      {
        this.toDisplay = (bool) info.GetValue(WelcomeScreenSetting.versionStr, typeof (bool));
      }
      catch
      {
      }
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      info.AddValue(WelcomeScreenSetting.versionStr, (object) this.toDisplay);
    }

    public bool ToDisplay => this.toDisplay;
  }
}
