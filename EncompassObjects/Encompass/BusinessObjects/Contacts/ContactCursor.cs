// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Provides a server-side cursor for iterating over a large set of
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> objects.
  /// </summary>
  public class ContactCursor : Cursor, IContactCursor
  {
    private ContactType contactType;

    internal ContactCursor(Session session, ICursor cursor, ContactType contactType)
      : base(session, cursor)
    {
      this.contactType = contactType;
    }

    /// <summary>
    /// Retrieves the item from the cursor at the specified index.
    /// </summary>
    /// <param name="index">Index of the item to be retrieved (with 0 as the first
    /// index).</param>
    /// <returns>Returns the specified <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> object.</returns>
    public Contact GetItem(int index) => (Contact) base.GetItem(index);

    /// <summary>
    /// Retrieves a subset of the cursor items starting at a specified index.
    /// </summary>
    /// <param name="startIndex">The index at which to start the subset.</param>
    /// <param name="count">The number of items to retrieve</param>
    /// <returns>Returns an array containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact" /> objects
    /// within the specified range</returns>
    public ContactList GetItems(int startIndex, int count)
    {
      return (ContactList) base.GetItems(startIndex, count);
    }

    internal override object ConvertToItemType(object item)
    {
      return this.contactType == ContactType.Borrower ? (object) new BorrowerContact(this.Session, (BorrowerInfo) item) : (object) new BizContact(this.Session, (BizPartnerInfo) item);
    }

    internal override ListBase ConvertToItemList(object[] items)
    {
      return this.contactType == ContactType.Borrower ? (ListBase) BorrowerContact.ToList(this.Session, (BorrowerInfo[]) items) : (ListBase) BizContact.ToList(this.Session, (BizPartnerInfo[]) items);
    }
  }
}
