// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.ClientContactSearchUtil
// Assembly: ClientCommon, Version=2.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 228D3734-C6F5-495E-AE35-6FE8CA02C59D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientCommon.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.RemotingServices;
using System.Collections;

#nullable disable
namespace EllieMae.EMLite.ContactUI
{
  public class ClientContactSearchUtil : ContactSearchUtilBase
  {
    public override ContactCustomFieldInfoCollection getCustomFields(ContactType contactType)
    {
      return Session.ContactManager.GetCustomFieldInfo(contactType);
    }

    public override Hashtable getCategoryIdToNameTable()
    {
      return new BizCategoryUtil(Session.SessionObjects).GetCategoryIdToNameTable();
    }

    public static string getSearchDescriptionHeading(ContactQuery query, ContactType contactType)
    {
      ContactQueryItem[] items = query.Items;
      int num1 = 0;
      int num2 = 0;
      for (int index = 0; index < items.Length; ++index)
      {
        if (items[index].GroupName == "LoanInfo")
          ++num1;
      }
      num2 = items.Length - num1;
      string str = "borrowers";
      if (contactType == ContactType.BizPartner)
        str = "business contacts";
      string descriptionHeading;
      switch (query.LoanMatchType)
      {
        case RelatedLoanMatchType.AnyClosed:
          descriptionHeading = num1 != 0 ? "Showing " + str + " whose closed loans have\n" : "Showing " + str + " with at least one closed loan\n";
          break;
        case RelatedLoanMatchType.LastClosed:
          descriptionHeading = num1 != 0 ? "Showing " + str + " whose last closed loan has\n" : "Showing " + str + " with at least one closed loan\n";
          break;
        case RelatedLoanMatchType.AnyOriginated:
          descriptionHeading = num1 != 0 ? "Showing " + str + " whose originated loans have\n" : "Showing " + str + " with at least one originated loan\n";
          break;
        case RelatedLoanMatchType.LastOriginated:
          descriptionHeading = num1 != 0 ? "Showing " + str + " whose last originated loan has\n" : "Showing " + str + " with at least one originated loan\n";
          break;
        default:
          descriptionHeading = items.Length != 0 ? "Showing " + str + " whose\n" : "Showing all " + str;
          break;
      }
      return descriptionHeading;
    }

    public override UserInfo getContactOwner(string userId)
    {
      return Session.OrganizationManager.GetUser(userId);
    }

    public ClientContactSearchUtil()
    {
    }

    public ClientContactSearchUtil(RelatedLoanMatchType loanMatchType, ContactType contactType)
      : base(loanMatchType, contactType)
    {
    }
  }
}
