// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent">ContactEvent</see> objects associated
  /// with a single contact.
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

    /// <summary>
    /// Gets the ContactEvent from the set with the specified index.
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
    public ContactEvent this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.events[index];
      }
    }

    /// <summary>Gets the number of events in the set.</summary>
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
    public int Count
    {
      get
      {
        this.ensureLoaded();
        return this.events.Count;
      }
    }

    /// <summary>
    /// Adds a new <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent">ContactEvent</see> to the set of events for this Contact.
    /// </summary>
    /// <param name="eventType">The type of event to create.</param>
    /// <returns>Returns the new <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent">ContactEvent</see> object.</returns>
    /// <remarks>The Timestamp for the new Event is set to the current date and time.</remarks>
    /// <example>
    /// The following code creates a new custom event on every Borrower Contact to
    /// indicate that a marketing email was sent to them.
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
    ///       // Get all of the Borrower Contacts
    ///       ContactList contacts = session.Contacts.GetAll(ContactType.Borrower);
    /// 
    ///       // For each contact, add a new event to indicate the mailing was sent
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          contacts[i].Events.Add("Marketing mail sent");
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactEvent Add(string eventType) => this.AddForDate(eventType, DateTime.Now);

    /// <summary>
    /// Adds a new <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent" /> to the set of events for this Contact
    /// using the specified date as the event's timestamp.
    /// </summary>
    /// <param name="eventType">The type of event to create.</param>
    /// <param name="eventDate">The date/time at which the event occurred.</param>
    /// <returns></returns>
    public ContactEvent AddForDate(string eventType, DateTime eventDate)
    {
      ContactEvent contactEvent = new ContactEvent(this.contact, this.mngr.GetHistoryItemForContact(this.contact.ID, this.contact.ContactType, this.mngr.AddHistoryItemForContact(this.contact.ID, this.contact.ContactType, new ContactHistoryItem(eventType ?? "", eventDate))));
      if (this.events != null)
        this.events.Add(contactEvent);
      return contactEvent;
    }

    /// <summary>Deletes a event from the server.</summary>
    /// <param name="evnt">The event to delete. This event must belong to the current contact.</param>
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
    ///          for (int i = contact.Events.Count - 1; i >= 0; i--)
    ///             contact.Events.Remove(contact.Events[i]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(ContactEvent evnt)
    {
      this.ensureLoaded();
      if (!this.events.Contains(evnt))
        throw new ArgumentException("Specified note not found", "note");
      this.mngr.DeleteNoteForContact(this.contact.ID, this.contact.ContactType, evnt.ID);
      this.events.Remove(evnt);
    }

    /// <summary>
    /// Allows for enumeration over the set of events for this contact.
    /// </summary>
    /// <returns>An enumerator for the set of events.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.events.GetEnumerator();
    }

    /// <summary>Refreshes the set of events from the server.</summary>
    /// <remarks>Any changes to the set of events made by other users since the
    /// contact was originaly load can be seen by refreshing the list.</remarks>
    /// <example>
    /// The following code waits until a "Closed Loan" event has occurred for
    /// a specific Business Partner.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.Collections;
    /// using System.IO;
    /// using System.Threading;
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
    ///       // Save the event IDs of the events that we've reported on
    ///       ArrayList eventIds = new ArrayList();
    /// 
    ///       for (;;)
    ///       {
    ///          // Iterate over all of the events for the contact
    ///          foreach (ContactEvent evnt in contact.Events)
    ///          {
    ///             // Ensure this is a "Closed Loan" event which we haven't reported on yet
    ///             if ((evnt.EventType == "Closed Loan") && (!eventIds.Contains(evnt.ID)))
    ///             {
    ///                Console.WriteLine("A new Closed Loan event has occured!");
    /// 
    ///                // Add the event ID to our list so we don't report on it again.
    ///                eventIds.Add(evnt.ID);
    ///             }
    ///          }
    /// 
    ///          // Sleep for 1 minute before checking again
    ///          Thread.Sleep(60000);
    /// 
    ///          // Refresh the list of events so we get any new events that have occurred
    ///          contact.Events.Refresh();
    ///       }
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
