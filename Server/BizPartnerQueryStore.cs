// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.BizPartnerQueryStore
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using System;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public sealed class BizPartnerQueryStore
  {
    private const string className = "BizPartnerQueryStore�";

    public static string getXmlFilePath(string userid)
    {
      return ClientContext.GetCurrent().Settings.GetDataFilePath("Users\\" + userid + "\\BizPartnerQueries.xml");
    }

    public static ContactQueries Get(string userid)
    {
      ContactQueries contactQueries = (ContactQueries) XmlDataStore.Deserialize(typeof (ContactQueries), BizPartnerQueryStore.getXmlFilePath(userid));
      if (contactQueries.Items == null)
        contactQueries.Items = new ContactQuery[0];
      return contactQueries;
    }

    public static void Set(string userid, ContactQueries queries)
    {
      string xmlFilePath = BizPartnerQueryStore.getXmlFilePath(userid);
      XmlDataStore.Serialize((object) queries, xmlFilePath);
    }

    public static void Delete(string userid, ContactQuery query)
    {
      ContactQueries contactQueries = BizPartnerQueryStore.Get(userid);
      contactQueries.Items = (ContactQuery[]) ArrayUtil.Remove((Array) contactQueries.Items, (object) query);
      string xmlFilePath = BizPartnerQueryStore.getXmlFilePath(userid);
      XmlDataStore.Serialize((object) contactQueries, xmlFilePath);
    }

    public static void Update(string userid, ContactQuery query)
    {
      ContactQueries contactQueries = BizPartnerQueryStore.Get(userid);
      contactQueries.Items = (ContactQuery[]) ArrayUtil.Update((Array) contactQueries.Items, (object) query);
      string xmlFilePath = BizPartnerQueryStore.getXmlFilePath(userid);
      XmlDataStore.Serialize((object) contactQueries, xmlFilePath);
    }

    public static void Add(string userid, ContactQuery query)
    {
      ContactQueries contactQueries = BizPartnerQueryStore.Get(userid);
      contactQueries.Items = (ContactQuery[]) ArrayUtil.Add((Array) contactQueries.Items, (object) query);
      string xmlFilePath = BizPartnerQueryStore.getXmlFilePath(userid);
      XmlDataStore.Serialize((object) contactQueries, xmlFilePath);
    }
  }
}
