// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.eFolder.WebCenterSettingsStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Configuration;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Serialization;
using System;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.eFolder
{
  public class WebCenterSettingsStore
  {
    private const string className = "WebCenterSettingsStore�";

    private WebCenterSettingsStore()
    {
    }

    public static WebCenterSettings GetWebCenterSettings()
    {
      return ClientContext.GetCurrent().Cache.Get<WebCenterSettings>(nameof (WebCenterSettingsStore), new Func<WebCenterSettings>(WebCenterSettingsStore.GetWebCenterSettingsFromXml), CacheSetting.Low);
    }

    private static WebCenterSettings GetWebCenterSettingsFromXml()
    {
      try
      {
        using (BinaryObject systemSettings = SystemConfiguration.GetSystemSettings("WebCenterSettings"))
        {
          if (systemSettings != null)
            return systemSettings.ToObject<WebCenterSettings>();
        }
      }
      catch
      {
      }
      return new WebCenterSettings();
    }

    public static void SaveWebCenterSettings(WebCenterSettings setup)
    {
      ClientContext.GetCurrent().Cache.Put<WebCenterSettings>(nameof (WebCenterSettingsStore), (Action) (() =>
      {
        using (BinaryObject data = new BinaryObject((IXmlSerializable) setup))
          SystemConfiguration.SaveSystemSettings("WebCenterSettings", data);
      }), new Func<WebCenterSettings>(WebCenterSettingsStore.GetWebCenterSettingsFromXml), CacheSetting.Low);
    }
  }
}
