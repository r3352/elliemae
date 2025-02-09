// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactEvent
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents a single event in the history of a contact.
  /// </summary>
  /// <remarks>Encompass predefines two events:
  /// <list type="bullet"><item>First Contact, which is automatically
  /// generated when the contact is first created</item>
  /// <item>Completed Loan, when a loan associated with this contact (either as a
  /// borrower or as a business partner) is completed</item></list></remarks>
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

    /// <summary>Gets the unique ID of this event in the log.</summary>
    public int ID => this.item.HistoryItemID;

    /// <summary>Gets the type of event that occurred.</summary>
    public string EventType => this.item.EventType;

    /// <summary>Gets the date and time at which the event occurred.</summary>
    public DateTime Timestamp => this.item.Timestamp;

    /// <summary>Gets the loan with which this event is related.</summary>
    /// <remarks>For events that are not related to a particular loan,
    /// this value should be set to null.</remarks>
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
