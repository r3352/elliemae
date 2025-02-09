// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Cursors;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactCursor : Cursor, IContactCursor
  {
    private ContactType contactType;

    internal ContactCursor(Session session, ICursor cursor, ContactType contactType)
      : base(session, cursor)
    {
      this.contactType = contactType;
    }

    public Contact GetItem(int index) => (Contact) base.GetItem(index);

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
