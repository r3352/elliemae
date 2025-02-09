// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactEvent : SessionBoundObject, IContactEvent
  {
    private IContactManager mngr;
    private Contact contact;
    private ContactHistoryItem item;
    private ContactLoan relatedLoan;

    internal ContactEvent(Contact contact, ContactHistoryItem item)
      : base(contact.Session)
    {
      this.mngr = this.Session.Contacts.ContactManager;
      this.contact = contact;
      this.item = item;
    }

    public int ID => this.item.HistoryItemID;

    public string EventType => this.item.EventType;

    public DateTime Timestamp => this.item.Timestamp;

    public ContactLoan RelatedLoan
    {
      get
      {
        this.ensureLoanLoaded();
        return this.relatedLoan;
      }
    }

    internal ContactHistoryItem Unwrap() => this.item;

    internal static ContactEventList ToList(Contact contact, ContactHistoryItem[] data)
    {
      ContactEventList list = new ContactEventList();
      for (int index = 0; index < data.Length; ++index)
        list.Add(new ContactEvent(contact, data[index]));
      return list;
    }

    private void ensureLoanLoaded()
    {
      if (this.item.LoanID < 0 || this.relatedLoan != null)
        return;
      this.relatedLoan = new ContactLoan(this.Session, this.mngr.GetContactLoan(this.item.LoanID));
    }
  }
}
