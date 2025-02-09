// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.AppointmentContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.Encompass.BusinessObjects.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  public class AppointmentContacts : SessionBoundObject, IAppointmentContacts, IEnumerable
  {
    private Appointment appt;
    private ContactList contacts;

    internal AppointmentContacts(Appointment appt)
      : base(appt.Session)
    {
      this.appt = appt;
      if (!appt.IsNew())
        return;
      this.contacts = new ContactList();
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.contacts.Count;
      }
    }

    public Contact this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.contacts[index];
      }
    }

    public void Add(Contact contact)
    {
      this.ensureLoaded();
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      if (!contact.Session.Equals((object) this.Session))
        throw new ArgumentException("Specific contact must come from same Session");
      for (int index = 0; index < this.contacts.Count; ++index)
      {
        if (contact.Equals((object) this.contacts[index]))
          throw new ArgumentException("Contact already exists in collection");
      }
      this.contacts.Add(contact);
    }

    public void Remove(Contact contact)
    {
      this.ensureLoaded();
      if (contact == null)
        throw new ArgumentNullException(nameof (contact));
      this.contacts.Remove(contact);
    }

    public void Clear() => this.contacts = new ContactList();

    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.contacts.GetEnumerator();
    }

    internal ContactInfo[] GetContactInfoList()
    {
      if (this.contacts == null)
        return (ContactInfo[]) null;
      ContactInfo[] contactInfoList = new ContactInfo[this.contacts.Count];
      for (int index = 0; index < this.contacts.Count; ++index)
      {
        Contact contact = this.contacts[index];
        contactInfoList[index] = new ContactInfo(contact.ID.ToString(), (CategoryType) contact.ContactType);
      }
      return contactInfoList;
    }

    internal void Refresh() => this.contacts = (ContactList) null;

    private void ensureLoaded()
    {
      if (this.contacts != null)
        return;
      this.contacts = new ContactList();
      ContactInfo[] contactInfo = this.Session.Calendar.CalendarManager.GetContactInfo(this.appt.ID.ToString());
      for (int index = 0; index < contactInfo.Length; ++index)
      {
        Contact contact = this.Session.Contacts.Open(int.Parse(contactInfo[index].ContactID), (ContactType) contactInfo[index].ContactType);
        if (contact != null)
          this.contacts.Add(contact);
      }
    }
  }
}
