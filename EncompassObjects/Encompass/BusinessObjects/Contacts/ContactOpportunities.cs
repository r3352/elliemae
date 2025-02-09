// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunities
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
  /// Represents the collection of <see cref="T:EllieMae.EMLite.ClientServer.Contacts.Opportunity" /> objects associated
  /// with a single contact.
  /// </summary>
  /// <remarks>Encompass 2.0 supports only a single Opportnity for a contact. Future
  /// versions may support multiple opportunities.</remarks>
  /// <example>
  /// The following code queries for all Borrower Contacts that have loan opportunities
  /// with amounts greater than $200,000. It then prints these contacts along with
  /// the accompanying loan amount.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Contacts;
  /// using EllieMae.Encompass.Query;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Create the query criterion to find all borrower with loan opportunities
  ///       // of at least $200K.
  ///       NumericFieldCriterion cri = new NumericFieldCriterion("Opp.LoanAmount", 200000,
  ///          OrdinalFieldMatchType.GreaterThanOrEquals);
  /// 
  ///       // Perform the query
  ///       ContactList contacts = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Borrower);
  /// 
  ///       // Display the loan amount for each opportunity along with the contact's name
  ///       foreach (BorrowerContact contact in contacts)
  ///       {
  ///          Console.WriteLine(contact.FirstName + " " + contact.LastName + ":");
  /// 
  ///          for (int i = 0; i < contact.Opportunities.Count; i++)
  ///          if (contact.Opportunities[i].LoanAmount >= 200000)
  ///             Console.WriteLine("   Loan Amount: " + contact.Opportunities[i].LoanAmount);
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class ContactOpportunities : SessionBoundObject, IContactOpportunities, IEnumerable
  {
    private IContactManager mngr;
    private Contact contact;
    private ContactOpportunityList opps;

    internal ContactOpportunities(Contact contact)
      : base(contact.Session)
    {
      this.mngr = this.Session.Contacts.ContactManager;
      this.contact = contact;
    }

    /// <summary>
    /// Gets the <see cref="T:EllieMae.EMLite.ClientServer.Contacts.Opportunity" /> from the set with the specified index.
    /// </summary>
    /// <example>
    /// The following code queries for all Borrower Contacts that have loan opportunities
    /// with amounts greater than $200,000. It then prints these contacts along with
    /// the accompanying loan amount.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the query criterion to find all borrower with loan opportunities
    ///       // of at least $200K.
    ///       NumericFieldCriterion cri = new NumericFieldCriterion("Opp.LoanAmount", 200000,
    ///          OrdinalFieldMatchType.GreaterThanOrEquals);
    /// 
    ///       // Perform the query
    ///       ContactList contacts = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Borrower);
    /// 
    ///       // Display the loan amount for each opportunity along with the contact's name
    ///       foreach (BorrowerContact contact in contacts)
    ///       {
    ///          Console.WriteLine(contact.FirstName + " " + contact.LastName + ":");
    /// 
    ///          for (int i = 0; i < contact.Opportunities.Count; i++)
    ///          if (contact.Opportunities[i].LoanAmount >= 200000)
    ///             Console.WriteLine("   Loan Amount: " + contact.Opportunities[i].LoanAmount);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactOpportunity this[int index]
    {
      get
      {
        this.ensureLoaded();
        return this.opps[index];
      }
    }

    /// <summary>Gets the number of opportunities in the set.</summary>
    /// <remarks>The present version of Encompass supports at most one Opportunity per
    /// Borrower Contact. Thus, this property will always return either 0 or 1.
    /// </remarks>
    /// <example>
    /// The following code queries for all Borrower Contacts that have loan opportunities
    /// with amounts greater than $200,000. It then prints these contacts along with
    /// the accompanying loan amount.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create the query criterion to find all borrower with loan opportunities
    ///       // of at least $200K.
    ///       NumericFieldCriterion cri = new NumericFieldCriterion("Opp.LoanAmount", 200000,
    ///          OrdinalFieldMatchType.GreaterThanOrEquals);
    /// 
    ///       // Perform the query
    ///       ContactList contacts = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Borrower);
    /// 
    ///       // Display the loan amount for each opportunity along with the contact's name
    ///       foreach (BorrowerContact contact in contacts)
    ///       {
    ///          Console.WriteLine(contact.FirstName + " " + contact.LastName + ":");
    /// 
    ///          for (int i = 0; i < contact.Opportunities.Count; i++)
    ///          if (contact.Opportunities[i].LoanAmount >= 200000)
    ///             Console.WriteLine("   Loan Amount: " + contact.Opportunities[i].LoanAmount);
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
        return this.opps.Count;
      }
    }

    /// <summary>Adds a new opportunity to the set.</summary>
    /// <remarks>Attempting to add more than one Opportunity to the collection
    /// will result in an exception.</remarks>
    /// <returns>The newly added <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity" /> object.</returns>
    /// <example>
    /// The following code adds a new ContactOpportunity to a Borrower Contact.
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
    ///       // Create a new contact
    ///       BorrowerContact contact = (BorrowerContact) session.Contacts.CreateNew(ContactType.Borrower);
    /// 
    ///       // Set some basic properties
    ///       contact.FirstName = "Mary";
    ///       contact.LastName = "Jones";
    ///       contact.HomePhone = "(555) 555-5555";
    ///       contact.BorrowerType = BorrowerContactType.Propspect;
    /// 
    ///       // Save the contact before we attempt to create the opportunity
    ///       contact.Commit();
    /// 
    ///       // Create an opportunity for the contact
    ///       ContactOpportunity opp = contact.Opportunities.Add();
    /// 
    ///       opp.LoanAmount = 245000;
    ///       opp.MortgageBalance = 255000;
    ///       opp.MortgageRate = 6.75f;
    ///       opp.EmploymentStatus = EmploymentStatus.Employed;
    ///       opp.PropertyUse = PropertyUse.Primary;
    ///       opp.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactOpportunity Add()
    {
      this.ensureLoaded();
      if (this.opps.Count >= 1)
        throw new InvalidOperationException("The current Encompass version supports at most one Opportunity per contact.");
      ContactOpportunity contactOpportunity = new ContactOpportunity(this.contact, this.Session.Contacts.ContactManager.GetBorrowerOpportunity(this.Session.Contacts.ContactManager.CreateBorrowerOpportunity(new Opportunity()
      {
        ContactID = this.contact.ID
      })));
      this.opps.Add(contactOpportunity);
      return contactOpportunity;
    }

    /// <summary>
    /// Removes an opportunity from the current borrower contact.
    /// </summary>
    /// <param name="opp">The <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactOpportunity" /> object to remove.</param>
    /// <example>
    /// The following code removes the opportunity information from every Borrower Contact
    /// for which the current mortgage rate is less than 4%.
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
    ///       // Create the query criterion to find all borrowers with an opportunity who
    ///       // have a current mortgage rate of less than 4%.
    ///       NumericFieldCriterion cri = new NumericFieldCriterion("Opp.MortRate", 4,
    ///          OrdinalFieldMatchType.LessThan);
    /// 
    ///       // Perform the query
    ///       ContactList contacts = session.Contacts.Query(cri, ContactLoanMatchType.None, ContactType.Borrower);
    /// 
    ///       // For each of these contacts, delete the associate opportunity
    ///       foreach (BorrowerContact contact in contacts)
    ///          contact.Opportunities.Remove(contact.Opportunities[0]);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Remove(ContactOpportunity opp)
    {
      this.ensureLoaded();
      if (!this.opps.Contains(opp))
        throw new ArgumentException("Specified Opportunity not found", nameof (opp));
      this.Session.Contacts.ContactManager.DeleteBorrowerOpportunity(opp.ID);
      this.opps.Remove(opp);
    }

    /// <summary>
    /// Allows for enumeration over the set of events for this contact.
    /// </summary>
    /// <returns>An enumerator for the set of opps.</returns>
    public IEnumerator GetEnumerator()
    {
      this.ensureLoaded();
      return this.opps.GetEnumerator();
    }

    /// <summary>Refreshes the set of events from the server.</summary>
    /// <remarks>Any changes to the set of events made by other users since the
    /// contact was originaly load can be seen by refreshing the list.</remarks>
    /// <include file="ContactOpportunities.xml" path="Examples/Example[@name=&quot;ContactOpportunities.Refresh&quot;]/*" />
    public void Refresh() => this.opps = (ContactOpportunityList) null;

    private void ensureLoaded()
    {
      if (this.opps != null)
        return;
      this.opps = new ContactOpportunityList();
      Opportunity opportunityByBorrowerId = this.mngr.GetOpportunityByBorrowerId(this.contact.ID);
      if (opportunityByBorrowerId == null)
        return;
      this.opps.Add(new ContactOpportunity(this.contact, opportunityByBorrowerId));
    }
  }
}
