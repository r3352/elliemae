// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.EVerifyLoginInfoStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class EVerifyLoginInfoStore
  {
    private const string className = "EVerifyLoginInfoStore�";

    private static string getXmlFilePath(string userid)
    {
      return ClientContext.GetCurrent().Settings.GetUserDataFilePath(userid, "EVerifyLoginInfo.xml");
    }

    public static EVerifyLoginInfoItem Get(string userid, string vendorName)
    {
      string xmlFilePath = EVerifyLoginInfoStore.getXmlFilePath(userid);
      foreach (EVerifyLoginInfoItem everifyLoginInfoItem in ((EVerifyLoginInfo) XmlDataStore.Deserialize(typeof (EVerifyLoginInfo), xmlFilePath)).Items)
      {
        if (everifyLoginInfoItem.vendorName == vendorName)
          return everifyLoginInfoItem;
      }
      return (EVerifyLoginInfoItem) null;
    }

    public static void Set(string userid, EVerifyLoginInfoItem loginInfoItem)
    {
      string xmlFilePath = EVerifyLoginInfoStore.getXmlFilePath(userid);
      EVerifyLoginInfo everifyLoginInfo = (EVerifyLoginInfo) XmlDataStore.Deserialize(typeof (EVerifyLoginInfo), xmlFilePath);
      foreach (EVerifyLoginInfoItem everifyLoginInfoItem in everifyLoginInfo.Items)
      {
        if (everifyLoginInfoItem.vendorName == loginInfoItem.vendorName)
        {
          everifyLoginInfoItem.bSave = loginInfoItem.bSave;
          everifyLoginInfoItem.accountName = loginInfoItem.accountName;
          everifyLoginInfoItem.pwd = loginInfoItem.pwd;
          XmlDataStore.Serialize((object) everifyLoginInfo, xmlFilePath);
          return;
        }
      }
      everifyLoginInfo.Items = (EVerifyLoginInfoItem[]) new ArrayList((ICollection) everifyLoginInfo.Items)
      {
        (object) loginInfoItem
      }.ToArray(typeof (EVerifyLoginInfoItem));
      XmlDataStore.Serialize((object) everifyLoginInfo, xmlFilePath);
    }
  }
}
