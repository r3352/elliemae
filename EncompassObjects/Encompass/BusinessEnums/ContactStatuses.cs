// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessEnums.ContactStatuses
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;

#nullable disable
namespace EllieMae.Encompass.BusinessEnums
{
  /// <summary>
  /// The ContactStatuses class represents the set of all Business Categories defined
  /// for the system. Business Categories are used to classify <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see>s
  /// into Appraisers, Lenders, etc.
  /// </summary>
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

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.ContactStatus">ContactStatus</see> with the specified index.</summary>
    public ContactStatus this[int index] => (ContactStatus) this.GetItem(index);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.ContactStatus">ContactStatus</see> with the specified ID value.</summary>
    public ContactStatus GetItemByID(int itemId) => (ContactStatus) base.GetItemByID(itemId);

    /// <summary>Provides access to the <see cref="T:EllieMae.Encompass.BusinessEnums.ContactStatus">ContactStatus</see> with the specified name.</summary>
    /// <param name="name">The name of the item being retrieved (case insensitive).</param>
    public ContactStatus GetItemByName(string name) => (ContactStatus) base.GetItemByName(name);
  }
}
