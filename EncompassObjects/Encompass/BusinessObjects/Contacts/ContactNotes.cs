// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactNotes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents the set of all <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactNote">ContactNote</see> objects
  /// associated with a contact.
  /// </summary>
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

    /// <summary>
    /// Gets the ContactNote object from the list with the specified index.
    /// </summary>
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
    public ContactNote this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.notes[index];
      }
    }

    /// <summary>Gets the number of notes contained in the set.</summary>
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
    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.notes.Count;
      }
    }

    /// <summary>Creates a new Note for the current contact.</summary>
    /// <param name="subject">The subject of the note.</param>
    /// <param name="details">The body or details of the note</param>
    /// <returns>The newly created <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactNote">ContactNote</see> object.</returns>
    /// <remarks>The Timestamp for the new note is set to the current date and time.</remarks>
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
    public ContactNote Add(string subject, string details)
    {
      ContactNote contactNote = new ContactNote(this.contact, this.mngr.GetNoteForContact(this.contact.ID, this.contact.ContactType, this.mngr.AddNoteForContact(this.contact.ID, this.contact.ContactType, new EllieMae.EMLite.ClientServer.Contacts.ContactNote(subject ?? "", details ?? ""))));
      if (this.notes != null)
        this.notes.Add(contactNote);
      return contactNote;
    }

    /// <summary>Deletes the specified note from the server.</summary>
    /// <param name="note">The note to delete. This note must belong to the current contact.</param>
    /// <example>
    /// The following code deletes all of the notes associated with a specific Borrower.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.Query;
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
    ///       // Create the query criteria to do a search by first and last name
    ///       StringFieldCriterion fnCri = new StringFieldCriterion("Contact.FirstName", "George", StringFieldMatchType.Exact, true);
    ///       StringFieldCriterion lnCri = new StringFieldCriterion("Contact.LastName", "Donahue", StringFieldMatchType.Exact, true);
    /// 
    ///       // Perform the query for any matching Borrower contacts
    ///       ContactList contacts = session.Contacts.Query(fnCri.And(lnCri), ContactLoanMatchType.None, ContactType.Borrower);
    /// 
    ///       // Delete all the notes for these borrowers
    ///       foreach (BorrowerContact contact in contacts)
    ///       {
    ///          for (int i = contact.Notes.Count - 1; i >= 0; i--)
    ///             contact.Notes.Remove(contact.Notes[i]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(ContactNote note)
    {
      this.ensureLoaded();
      if (!this.notes.Contains(note))
        throw new ArgumentException("Specified note not found", nameof (note));
      this.mngr.DeleteNoteForContact(this.contact.ID, this.contact.ContactType, note.ID);
      this.notes.Remove(note);
    }

    /// <summary>
    /// Allows for enumeration over the set of notes for this contact.
    /// </summary>
    /// <returns>An enumerator for the set of notes.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.notes.GetEnumerator();
    }

    /// <summary>Refreshes the set of notes from the server.</summary>
    /// <remarks>Any changes to the set of notes made by other users since the
    /// contact was last retrieved will be retrieved.</remarks>
    /// <example>
    /// The following code modifies several of the notes associated with a contact
    /// and then discards those changes by calling Refresh().
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
    ///       // Modify the subject of all of the contact's notes
    ///       foreach (ContactNote note in contact.Notes)
    ///          note.Subject = "This is the new subject";
    /// 
    ///       // Refresh the note set, which results in all pending changes to be lost
    ///       contact.Notes.Refresh();
    /// 
    ///       // Output the subject lines of the notes to verify the've been restored
    ///       foreach (ContactNote note in contact.Notes)
    ///          Console.WriteLine(note.Subject);
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
