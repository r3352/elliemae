// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The InvestorContacts class represents the set of all Investor Contacts defined
  /// for the current <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorTemplate">InvestorTemplate</see>.
  /// </summary>
  public class InvestorContacts : EnumBase, IInvestorContacts
  {
    private Investor investor;

    internal InvestorContacts(Investor investor)
    {
      this.investor = investor;
      int id = 0;
      foreach (string name in Enum.GetNames(typeof (EllieMae.EMLite.ClientServer.InvestorContactType)))
      {
        ContactInformation contactInformation = this.investor.GetContactInformation(name);
        InvestorContactType contactType = (InvestorContactType) Enum.Parse(typeof (EllieMae.EMLite.ClientServer.InvestorContactType), name);
        this.AddItem((EnumItem) new InvestorContact(id, contactInformation, contactType));
        ++id;
      }
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see> by it's <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContactType">InvestorContactType</see>.
    /// </summary>
    /// <param name="contactType">The index of the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContactType">InvestorContactType</see> enum value for the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see></returns>
    public InvestorContact this[InvestorContactType contactType]
    {
      get => (InvestorContact) this.GetItemByName(contactType.ToString());
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see> by it's index.
    /// </summary>
    /// <param name="index">The index of the <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see> in the list.</param>
    /// <returns>The selected <see cref="T:EllieMae.Encompass.BusinessEnums.InvestorContact">InvestorContact</see></returns>
    public InvestorContact this[int index] => (InvestorContact) this.GetItem(index);
  }
}
