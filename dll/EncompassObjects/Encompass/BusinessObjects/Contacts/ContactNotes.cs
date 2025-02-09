// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactNotes
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
  public class ContactNotes : SessionBoundObject, IContactNotes, IEnumerable
  {
    private IContactManager mngr;
    private Contact contact;
    private ContactNoteList notes;

    internal ContactNotes(Contact contact)
      : base(contact.Session)
    {
      this.mngr = this.Session.Contacts.ContactManager;
      this.contact = contact;
    }

    public ContactNote this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.notes[index];
      }
    }

    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.notes.Count;
      }
    }

    public ContactNote Add(string subject, string details)
    {
      ContactNote contactNote = new ContactNote(this.contact, this.mngr.GetNoteForContact(this.contact.ID, this.contact.ContactType, this.mngr.AddNoteForContact(this.contact.ID, this.contact.ContactType, new ContactNote(subject ?? "", details ?? ""))));
      if (this.notes != null)
        this.notes.Add(contactNote);
      return contactNote;
    }

    public void Remove(ContactNote note)
    {
      this.ensureLoaded();
      if (!this.notes.Contains(note))
        throw new ArgumentException("Specified note not found", nameof (note));
      this.mngr.DeleteNoteForContact(this.contact.ID, this.contact.ContactType, note.ID);
      this.notes.Remove(note);
    }

    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.notes.GetEnumerator();
    }

    public void Refresh()
    {
      this.notes = (ContactNoteList) null;
      this.ensureLoaded();
    }

    private void ensureLoaded()
    {
      if (this.notes != null)
        return;
      this.notes = ContactNote.ToList(this.contact, this.mngr.GetNotesForContact(this.contact.ID, this.contact.ContactType));
    }
  }
}
