// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.InvestorContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class InvestorContacts : EnumBase, IInvestorContacts
  {
    private Investor investor;

    internal InvestorContacts(Investor investor)
    {
      this.investor = investor;
      int id = 0;
      foreach (string name in Enum.GetNames(typeof (InvestorContactType)))
      {
        ContactInformation contactInformation = this.investor.GetContactInformation(name);
        InvestorContactType contactType = (InvestorContactType) Enum.Parse(typeof (InvestorContactType), name);
        this.AddItem((EnumItem) new InvestorContact(id, contactInformation, contactType));
        ++id;
      }
    }

    public InvestorContact this[InvestorContactType contactType]
    {
      get => (InvestorContact) this.GetItemByName(contactType.ToString());
    }

    public InvestorContact this[int index] => (InvestorContact) this.GetItem(index);
  }
}
