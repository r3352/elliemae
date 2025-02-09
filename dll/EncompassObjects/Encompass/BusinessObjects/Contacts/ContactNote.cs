// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactNote
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
  public class ContactNote : SessionBoundObject, IContactNote
  {
    private ScopedEventHandler<EventArgs> committed;
    private IContactManager mngr;
    private Contact contact;
    private ContactNote note;

    public event EventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal ContactNote(Contact contact, ContactNote note)
      : base(contact.Session)
    {
      this.committed = new ScopedEventHandler<EventArgs>(nameof (ContactNote), "Committed");
      this.contact = contact;
      this.mngr = this.Session.Contacts.ContactManager;
      this.note = note;
    }

    public int ID => this.note.NoteID;

    public string Subject
    {
      get => this.note.Subject;
      set => this.note.Subject = value ?? "";
    }

    public DateTime Timestamp
    {
      get => this.note.Timestamp;
      set => this.note.Timestamp = value;
    }

    public string Details
    {
      get => this.note.Details;
      set => this.note.Details = value ?? "";
    }

    public void Commit()
    {
      this.mngr.UpdateNoteForContact(this.contact.ID, this.contact.ContactType, this.note);
    }

    public void Refresh()
    {
      this.note = this.mngr.GetNoteForContact(this.contact.ID, this.contact.ContactType, this.note.NoteID);
    }

    internal ContactNote Unwrap() => this.note;

    internal static ContactNoteList ToList(Contact contact, ContactNote[] data)
    {
      ContactNoteList list = new ContactNoteList();
      for (int index = 0; index < data.Length; ++index)
        list.Add(new ContactNote(contact, data[index]));
      return list;
    }

    protected void OnCommitted() => this.committed((object) this, EventArgs.Empty);
  }
}
