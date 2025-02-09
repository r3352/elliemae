// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactNote
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Represents a single note made for a contact.</summary>
  /// <example>
  /// The following code demonstrates retrieving, updating and adding
  /// notes for a contact.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Create a new Business Contact in the database
  ///       BizContact contact = (BizContact) session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
  /// 
  ///       // Print all of the notes for the contact
  ///       for (int i = 0; i < contact.Notes.Count; i++)
  ///       {
  ///          Console.WriteLine("Timestamp: " + contact.Notes[i].Timestamp);
  ///          Console.WriteLine("Subject:   " + contact.Notes[i].Subject);
  ///          Console.WriteLine(contact.Notes[i].Details);
  ///       }
  /// 
  ///       // Now add a new note. We do not have to call Commit() to save to database --
  ///       // the act of adding it saves it to the server.
  ///       ContactNote newNote = contact.Notes.Add("The subject of the note", "The body of the note");
  ///       Console.WriteLine("New note created with ID " + newNote.ID);
  /// 
  ///       // Modify the note and save it
  ///       newNote.Subject = "The corrected subject of the note";
  ///       newNote.Commit();
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class ContactNote : SessionBoundObject, IContactNote
  {
    private ScopedEventHandler<EventArgs> committed;
    private IContactManager mngr;
    private Contact contact;
    private EllieMae.EMLite.ClientServer.Contacts.ContactNote note;

    /// <summary>Event indicating that the object has been committed to the server.</summary>
    public event EventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<EventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal ContactNote(Contact contact, EllieMae.EMLite.ClientServer.Contacts.ContactNote note)
      : base(contact.Session)
    {
      this.committed = new ScopedEventHandler<EventArgs>(nameof (ContactNote), "Committed");
      this.contact = contact;
      this.mngr = this.Session.Contacts.ContactManager;
      this.note = note;
    }

    /// <summary>Gets the unique identifier for the note.</summary>
    public int ID => this.note.NoteID;

    /// <summary>Gets or sets the subject of the note.</summary>
    public string Subject
    {
      get => this.note.Subject;
      set => this.note.Subject = value ?? "";
    }

    /// <summary>
    /// Gets or sets the date and time associated with the note.
    /// </summary>
    public DateTime Timestamp
    {
      get => this.note.Timestamp;
      set => this.note.Timestamp = value;
    }

    /// <summary>Gets or sets the body or details of the note.</summary>
    public string Details
    {
      get => this.note.Details;
      set => this.note.Details = value ?? "";
    }

    /// <summary>
    /// Commits any pending changes to the object to the server.
    /// </summary>
    /// <remarks>This function saves any changes made to an existing object
    /// or commits a new object if not already saved. Failing to call commit
    /// will result in the loss of any changes made.</remarks>
    /// <example>
    /// The following code demonstrates retrieving, updating and adding
    /// notes for a contact.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create a new Business Contact in the database
    ///       BizContact contact = (BizContact) session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
    /// 
    ///       // Print all of the notes for the contact
    ///       for (int i = 0; i < contact.Notes.Count; i++)
    ///       {
    ///          Console.WriteLine("Timestamp: " + contact.Notes[i].Timestamp);
    ///          Console.WriteLine("Subject:   " + contact.Notes[i].Subject);
    ///          Console.WriteLine(contact.Notes[i].Details);
    ///       }
    /// 
    ///       // Now add a new note. We do not have to call Commit() to save to database --
    ///       // the act of adding it saves it to the server.
    ///       ContactNote newNote = contact.Notes.Add("The subject of the note", "The body of the note");
    ///       Console.WriteLine("New note created with ID " + newNote.ID);
    /// 
    ///       // Modify the note and save it
    ///       newNote.Subject = "The corrected subject of the note";
    ///       newNote.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Commit()
    {
      this.mngr.UpdateNoteForContact(this.contact.ID, this.contact.ContactType, this.note);
    }

    /// <summary>Refreshes the current note from the server.</summary>
    /// <remarks>Any changes made by other users will become visible once refreshed.
    /// However, any pending changes that weren't committed will be lost.</remarks>
    /// <example>
    /// The following code demonstrates how pending changes to a note can be discarded
    /// using the Refresh() method.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open a Business Contact using the contact's unique ID
    ///       BizContact contact = (BizContact) session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
    /// 
    ///       // Fetch the first note for this contact
    ///       ContactNote note = contact.Notes[0];
    /// 
    ///       // Modify the note
    ///       note.Subject = "This is the new subject";
    ///       note.Details = "This is the new body of the note";
    /// 
    ///       // Discard the changes by refreshing the note
    ///       note.Refresh();
    /// 
    ///       // Verify that the previous values have been restored
    ///       Console.WriteLine(note.Subject);
    ///       Console.WriteLine(note.Details);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Refresh()
    {
      this.note = this.mngr.GetNoteForContact(this.contact.ID, this.contact.ContactType, this.note.NoteID);
    }

    internal EllieMae.EMLite.ClientServer.Contacts.ContactNote Unwrap() => this.note;

    internal static ContactNoteList ToList(Contact contact, EllieMae.EMLite.ClientServer.Contacts.ContactNote[] data)
    {
      ContactNoteList list = new ContactNoteList();
      for (int index = 0; index < data.Length; ++index)
        list.Add(new ContactNote(contact, data[index]));
      return list;
    }

    /// <summary>Raises the Committed event</summary>
    protected void OnCommitted() => this.committed.Invoke((object) this, EventArgs.Empty);
  }
}
