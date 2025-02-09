// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.WelcomeScreenSettingStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects
{
  public class WelcomeScreenSettingStore
  {
    private WelcomeScreenSettingStore()
    {
    }

    private static string getXmlFilePath(string userid)
    {
      return ClientContext.GetCurrent().Settings.GetUserDataFilePath(userid, "WelcomeScreenSetting.xml");
    }

    public static WelcomeScreenSetting Get(string userid)
    {
      return (WelcomeScreenSetting) XmlDataStore.Deserialize(typeof (WelcomeScreenSetting), WelcomeScreenSettingStore.getXmlFilePath(userid));
    }

    public static void Save(string userid, WelcomeScreenSetting setting)
    {
      string xmlFilePath = WelcomeScreenSettingStore.getXmlFilePath(userid);
      XmlDataStore.Serialize((object) setting, xmlFilePath);
    }
  }
}
