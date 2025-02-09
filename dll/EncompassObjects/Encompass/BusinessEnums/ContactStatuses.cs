// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ContactStatuses
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  public class ContactStatuses : EnumBase, IContactStatuses
  {
    internal ContactStatuses(IContactManager mngr)
    {
      BorrowerStatusItem[] items = mngr.GetBorrowerStatus().Items;
      if (items == null)
        return;
      for (int index = 0; index < items.Length; ++index)
        this.AddItem((EnumItem) new ContactStatus(items[index]));
    }

    public ContactStatus this[int index] => (ContactStatus) this.GetItem(index);

    public ContactStatus GetItemByID(int itemId) => (ContactStatus) base.GetItemByID(itemId);

    public ContactStatus GetItemByName(string name) => (ContactStatus) base.GetItemByName(name);
  }
}
