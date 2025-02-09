// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerContactSearchUtil
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.Server
{
  public class ServerContactSearchUtil : ContactSearchUtilBase
  {
    public override ContactCustomFieldInfoCollection getCustomFields(ContactType contactType)
    {
      return Contact.GetCustomFieldInfo(contactType);
    }

    public override Hashtable getCategoryIdToNameTable()
    {
      Hashtable categoryIdToNameTable = new Hashtable();
      foreach (BizCategory bizCategory in BizPartnerContact.GetBizCategories())
        categoryIdToNameTable.Add((object) bizCategory.CategoryID, (object) bizCategory.Name);
      return categoryIdToNameTable;
    }

    public override UserInfo getContactOwner(string userId)
    {
      User latestVersion = UserStore.GetLatestVersion(userId);
      UserInfo contactOwner = (UserInfo) null;
      if (latestVersion.Exists)
        contactOwner = latestVersion.UserInfo;
      return contactOwner;
    }

    public ServerContactSearchUtil()
    {
    }

    public ServerContactSearchUtil(RelatedLoanMatchType loanMatchType, ContactType contactType)
      : base(loanMatchType, contactType)
    {
    }
  }
}
