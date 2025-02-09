// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  public class ContactEvents : SessionBoundObject, IContactEvents, IEnumerable
  {
    private IContactManager mngr;
    private Contact contact;
    private ContactEventList events;

    internal ContactEvents(Contact contact)
      : base(contact.Session)
    {
      this.mngr = this.Session.Contacts.ContactManager;
      this.contact = contact;
    }

    public ContactEvent this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.events[index];
      }
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.events.Count;
      }
    }

    public ContactEvent Add(string eventType) => this.AddForDate(eventType, DateTime.Now);

    public ContactEvent AddForDate(string eventType, DateTime eventDate)
    {
      ContactEvent contactEvent = new ContactEvent(this.contact, this.mngr.GetHistoryItemForContact(this.contact.ID, this.contact.ContactType, this.mngr.AddHistoryItemForContact(this.contact.ID, this.contact.ContactType, new ContactHistoryItem(eventType ?? "", eventDate))));
      if (this.events != null)
        this.events.Add(contactEvent);
      return contactEvent;
    }

    public void Remove(ContactEvent evnt)
    {
      this.ensureLoaded();
      if (!this.events.Contains(evnt))
        throw new ArgumentException("Specified note not found", "note");
      this.mngr.DeleteNoteForContact(this.contact.ID, this.contact.ContactType, evnt.ID);
      this.events.Remove(evnt);
    }

    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.events.GetEnumerator();
    }

    public void Refresh()
    {
      this.events = (ContactEventList) null;
      this.ensureLoaded();
    }

    private void ensureLoaded()
    {
      if (this.events != null)
        return;
      this.events = ContactEvent.ToList(this.contact, this.mngr.GetHistoryForContact(this.contact.ID, this.contact.ContactType));
    }
  }
}
