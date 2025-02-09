// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Contact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.Common;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Provides a base class for both the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see> and
  /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact">BorrowerContact</see> classes.
  /// </summary>
  [ComSourceInterfaces(typeof (IPersistentObjectEvents))]
  public abstract class Contact : SessionBoundObject, IContact
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    protected internal IContactManager mngr;
    protected internal ContactNotes notes;
    protected internal ContactEvents events;
    protected internal ContactCustomFields customFields;

    /// <summary>Event indicating that the object has been committed to the server.</summary>
    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    protected internal Contact(Session session)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Contact), "Committed");
      this.mngr = session.Contacts.ContactManager;
    }

    /// <summary>Gets the unique ID for the contact.</summary>
    public abstract int ID { get; }

    /// <summary>Gets the Guid for the contact.</summary>
    internal abstract string Guid { get; }

    /// <summary>Gets or sets the last name for the contact.</summary>
    public abstract string LastName { get; set; }

    /// <summary>Gets or sets the salutation for the contact.</summary>
    public abstract string Salutation { get; set; }

    /// <summary>Gets or sets the first name for the contact.</summary>
    public abstract string FirstName { get; set; }

    /// <summary>Gets the combined first and last name of the contact.</summary>
    public string FullName => Utils.JoinName(this.FirstName, this.LastName);

    /// <summary>Gets or sets the job title for the contact.</summary>
    public abstract string JobTitle { get; set; }

    /// <summary>Gets or sets the work phone number for the contact.</summary>
    public abstract string WorkPhone { get; set; }

    /// <summary>Gets or sets the home phone number for the contact.</summary>
    public abstract string HomePhone { get; set; }

    /// <summary>Gets or sets the mobile/cell phone for the contact.</summary>
    public abstract string MobilePhone { get; set; }

    /// <summary>Gets or sets the fax number for the contact.</summary>
    public abstract string FaxNumber { get; set; }

    /// <summary>Gets or sets the first custom field for the contact.</summary>
    public abstract string CustField1 { get; set; }

    /// <summary>Gets or sets the second custom field for the contact.</summary>
    public abstract string CustField2 { get; set; }

    /// <summary>Gets or sets the third custom field for the contact.</summary>
    public abstract string CustField3 { get; set; }

    /// <summary>Gets or sets the fourth custom field for the contact.</summary>
    public abstract string CustField4 { get; set; }

    /// <summary>Gets or sets the personal/home email address for the contact.</summary>
    public abstract string PersonalEmail { get; set; }

    /// <summary>Gets or sets the business/work email address for the contact.</summary>
    public abstract string BizEmail { get; set; }

    /// <summary>Gets or sets the business web page URL for the contact.</summary>
    public abstract string BizWebUrl { get; set; }

    /// <summary>Gets or sets the business/employer address for the contact.</summary>
    public abstract Address BizAddress { get; }

    /// <summary>Gets or sets a flag indicating if the user should receive unsolicited email.</summary>
    public abstract bool NoSpam { get; set; }

    /// <summary>Gets the date and time the contact was last saved.</summary>
    public abstract DateTime LastModified { get; }

    /// <summary>Gets or sets the owner of the contact.</summary>
    public abstract User Owner { get; set; }

    /// <summary>Gets or sets the access level for the contact, either Public or Private.</summary>
    public abstract ContactAccessLevel AccessLevel { get; set; }

    /// <summary>Gets the type of contact represented by this object.</summary>
    public abstract ContactType Type { get; }

    /// <summary>Saves pending changes to the current contact.</summary>
    /// <example>
    /// The following code opens an existing Business Contact, modifies its first
    /// and last name and then commits the changes to the Encompass server.
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
    ///       // Get the contact specified on the command line
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
    /// 
    ///       // Update his personal information
    ///       contact.FirstName = "Fred";
    ///       contact.LastName = "Silverman";
    /// 
    ///       // Until Commit is called, the changes are held locally. Commit persists
    ///       // them back to the Encompass Server.
    ///       contact.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public abstract void Commit();

    /// <summary>Deletes the current contact.</summary>
    /// <example>
    /// The following code deletes all Appraisers from the contacts database.
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
    ///       // Create the query criterion to get the Appraisers from the server
    ///       NumericFieldCriterion catCri = new NumericFieldCriterion();
    ///       catCri.FieldName = "Contact.CategoryID";
    ///       catCri.Value = session.Contacts.BizCategories.GetItemByName("Appraiser").ID;
    /// 
    ///       // Retrieve the set of all Business Contacts matching our criteria
    ///       ContactList contacts = session.Contacts.Query(catCri, ContactLoanMatchType.None, ContactType.Biz);
    /// 
    ///       // Delete all of these contacts
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          contacts[i].Delete();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public abstract void Delete();

    /// <summary>Refreshes the contact's information from the server.</summary>
    /// <example>
    /// The following code demonstrates how using the Refresh method will discard
    /// any pending changes to a Contact that haven't been committed.
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
    ///       // Get the contact specified on the command line
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
    /// 
    ///       // Print the original first name of the contact, e.g. "Mary"
    ///       Console.WriteLine(contact.FirstName);
    /// 
    ///       // Modify the first name
    ///       contact.FirstName = "Marilyn";
    /// 
    ///       // The following line will print the new value, "Marilyn"
    ///       Console.WriteLine(contact.FirstName);
    /// 
    ///       // Refresh the contact to reload the fields from the server
    ///       contact.Refresh();
    /// 
    ///       // The first name will now be restored to its prior state (unless
    ///       // it was changed by another user since originally retrieved).
    ///       Console.WriteLine(contact.FirstName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public abstract void Refresh();

    /// <summary>
    /// Gets a flag indicating if the current contact has yet to be saved to the server.
    /// </summary>
    public bool IsNew => this.ID < 0;

    /// <summary>
    /// Gets the set of notes associated with the current contact.
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
    public ContactNotes Notes
    {
      get
      {
        this.ensureExists();
        if (this.notes == null)
          this.notes = new ContactNotes(this);
        return this.notes;
      }
    }

    /// <summary>
    /// Gets the set of events associated with the current contact.
    /// </summary>
    /// <example>
    /// The following code lists all of the events for a Business Contact whose
    /// unique ID is specified on the command line.
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
    ///       // Print all of the events for the contact
    ///       for (int i = 0; i < contact.Events.Count; i++)
    ///       {
    ///          Console.WriteLine("Timestamp: " + contact.Events[i].Timestamp);
    ///          Console.WriteLine("Subject:   " + contact.Events[i].EventType);
    /// 
    ///          // If there's a loan related to this event, output some of its properties
    ///          if (contact.Events[i].RelatedLoan != null)
    ///          {
    ///             ContactLoan loan = contact.Events[i].RelatedLoan;
    ///             Console.WriteLine("Loan Amount:   " + loan.LoanAmount);
    ///             Console.WriteLine("Loan Closed:   " + loan.DateCompleted);
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactEvents Events
    {
      get
      {
        this.ensureExists();
        if (this.events == null)
          this.events = new ContactEvents(this);
        return this.events;
      }
    }

    /// <summary>
    /// Gets the set of custom fields associated with the current contact.
    /// </summary>
    public ContactCustomFields CustomFields
    {
      get
      {
        if (this.customFields == null)
          this.customFields = new ContactCustomFields(this);
        return this.customFields;
      }
    }

    /// <summary>
    /// Returns a collection of the relationships this contact has with the Loans.
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.LoanContactRelationshipList" /> containing an entry
    /// for each loan relationship this contact is assigned to. If a contact is assigned to multiple
    /// relationships within the same loan, multiple records will be returned, one for each
    /// relationship.</returns>
    public LoanContactRelationshipList GetLoanRelationships()
    {
      ContactLoanPair[] relatedLoansForContact = this.Session.SessionObjects.ContactManager.GetRelatedLoansForContact(this.ID, this.ContactType);
      LoanContactRelationshipList loanRelationships = new LoanContactRelationshipList();
      foreach (ContactLoanPair contactLoanPair in relatedLoansForContact)
        loanRelationships.Add(new LoanContactRelationship(this.Session, contactLoanPair.LoanGuid, contactLoanPair.ContactID, (ContactType) contactLoanPair.ContactType, (LoanContactRelationshipType) contactLoanPair.RoleType, contactLoanPair.BorrowerPair));
      return loanRelationships;
    }

    /// <summary>Provides a string representation of the contact.</summary>
    /// <returns>Returns the first and last name of the contact.</returns>
    public override string ToString() => this.FirstName + " " + this.LastName;

    /// <summary>
    /// Provides a hash code implementation for the Contact object.
    /// </summary>
    /// <returns>A hash code usable in a Hashtable object.</returns>
    public override int GetHashCode() => this.ID;

    /// <summary>
    /// Determines if two contact objects represent the same persistent contact.
    /// </summary>
    /// <param name="obj">The contact to which to compare this object.</param>
    /// <returns>Returns <c>true</c> if the two objects represent the same
    /// contact, <c>false</c> otherwise.</returns>
    public override bool Equals(object obj)
    {
      if (object.Equals((object) (obj as Contact), (object) null))
        return false;
      Contact contact = (Contact) obj;
      if (contact.Session != this.Session)
        return false;
      if (contact.ID < 0 || this.ID < 0)
        return this == obj;
      return contact.ContactType == this.ContactType && contact.ID == this.ID;
    }

    /// <summary>Internal property for the contact type</summary>
    internal abstract EllieMae.EMLite.ContactUI.ContactType ContactType { get; }

    /// <summary>
    /// 
    /// </summary>
    protected internal void ensureExists()
    {
      if (this.IsNew)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    protected internal void OnCommitted()
    {
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }
  }
}
