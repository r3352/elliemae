// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.Contacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Query;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Provides access to the contacts database within Encompass.
  /// </summary>
  /// <example>
  /// The following code opens a Business Contact using the contact's ID. The
  /// contact is then modified and saved back to the server.
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
  ///       // Open the contact specified on the command line
  ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
  /// 
  ///       if (contact != null)
  ///       {
  ///          // Modify the contact's e-mail address and save back to the server
  ///          contact.BizAddress.Street1 = "409 Kensington Ave.";
  ///          contact.BizAddress.City = "Bethesda";
  ///          contact.BizAddress.State = "MD";
  ///          contact.BizAddress.Zip = "21107";
  ///          contact.Commit();
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class Contacts : SessionBoundObject, IContacts
  {
    private BizCategories bizCategories;
    private Hashtable customFields = new Hashtable();

    internal Contacts(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Opens an existing <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see> or
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact">BorrowerContact</see> object.
    /// </summary>
    /// <param name="contactId">The ID of the contact to open.</param>
    /// <param name="type">The type of contact being retrieved.</param>
    /// <returns>The requested <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact">Contact</see> object or
    /// null if the ID specified is invalid.</returns>
    /// <example>
    /// The following code opens a Business Contact using the contact's ID. The
    /// contact is then modified and saved back to the server.
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
    ///       // Open the contact specified on the command line
    ///       Contact contact = session.Contacts.Open(int.Parse(args[0]), ContactType.Biz);
    /// 
    ///       if (contact != null)
    ///       {
    ///          // Modify the contact's e-mail address and save back to the server
    ///          contact.BizAddress.Street1 = "409 Kensington Ave.";
    ///          contact.BizAddress.City = "Bethesda";
    ///          contact.BizAddress.State = "MD";
    ///          contact.BizAddress.Zip = "21107";
    ///          contact.Commit();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Contact Open(int contactId, ContactType type)
    {
      ContactList contactList = this.Query((EllieMae.Encompass.Query.QueryCriterion) new NumericFieldCriterion()
      {
        FieldName = "Contact.ContactID",
        Value = (double) contactId,
        MatchType = OrdinalFieldMatchType.Equals
      }, ContactLoanMatchType.None, type);
      return contactList.Count > 0 ? contactList[0] : (Contact) null;
    }

    /// <summary>
    /// Creates a new <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see> or
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact">BorrowerContact</see> object.
    /// </summary>
    /// <param name="type">The type of contact being created.</param>
    /// <remarks>The returned object is not yet committed to the database.
    /// To save the contact, update the properties as necessary and call <c>Commit()</c></remarks>
    /// <returns>A new <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact">Contact</see> object.</returns>
    /// <example>
    /// The following code creates a new Business Contact, populates it contact
    /// information and commits it to the server.
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
    ///       // Open the contact specified on the command line
    ///       Contact contact = session.Contacts.CreateNew(ContactType.Biz);
    /// 
    ///       // Set the contact's personal infomation
    ///       contact.FirstName = "Allison";
    ///       contact.LastName = "Meriwether";
    ///       contact.BizEmail = "allison@somecompany.com";
    /// 
    ///       // Set the Business Category
    ///       BizContact biz = (contact as BizContact);
    ///       biz.Category = session.Contacts.BizCategories.GetItemByName("Appraiser");
    /// 
    ///       // Save to the server
    ///       contact.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Contact CreateNew(ContactType type)
    {
      if (type == ContactType.Biz)
        return (Contact) new BizContact(this.Session);
      if (type == ContactType.Borrower)
        return (Contact) new BorrowerContact(this.Session);
      throw new ArgumentException("Invalid contact type specified");
    }

    /// <summary>
    /// Deletes an existing <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see> or
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact">BorrowerContact</see> object.
    /// </summary>
    /// <param name="type">The type of contact being deleted.</param>
    /// <param name="contactId">The ID of the contact to delete.</param>
    /// <remarks>In order to delete a BorrowerContact, the currently logged in
    /// user must be an administrator or must own the specified contact.</remarks>
    /// <example>
    /// The following code deletes a Business Contact from the contact database
    /// by specifying his contact ID directly.
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
    ///       // Parse the contact ID
    ///       int contactId = int.Parse(args[0]);
    /// 
    ///       // Open the contact specified on the command line
    ///       session.Contacts.Delete(contactId, ContactType.Biz);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Delete(int contactId, ContactType type) => this.Open(contactId, type)?.Delete();

    /// <summary>
    /// Executes a query to select a subset of the business contacts that match
    /// the specified citeria.
    /// </summary>
    /// <param name="criterion">The criterion to use to perform the query. This
    /// can be a single criterion or a logical combination of objects which
    /// implement the IQueryCriterion interface.</param>
    /// <param name="loanMatchType">The types of related loans that should
    /// be considered when performing this query. This parameters allows you
    /// to locate all contacts who satisfy certain loan-related criteria.</param>
    /// <param name="type">The type of contacts being queried.</param>
    /// <returns>The list of <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact">Contact</see> objects that
    /// matched the specified criteria.
    /// </returns>
    /// <remarks>When querying for BorrowerContacts, a non-administrative user will
    /// only be able to see those contacts which are assigned to him. An administrative user,
    /// however, is able to query against the set of all contacts.
    /// </remarks>
    /// <example>
    /// The following code queries the Borrower Contacts for all borrowers who
    /// have closed a loan exceeding $200,000 within the current year.
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
    ///       // Create the criterion for the loan amount (>= $200,000)
    ///       NumericFieldCriterion amtCriterion = new NumericFieldCriterion();
    ///       amtCriterion.FieldName = "RelatedLoan.LoanAmount";
    ///       amtCriterion.Value = 200000;
    ///       amtCriterion.MatchType = OrdinalFieldMatchType.GreaterThanOrEquals;
    /// 
    ///       // Create the criterion for the loan completion date in the current year
    ///       DateFieldCriterion closedCriterion = new DateFieldCriterion();
    ///       closedCriterion.FieldName = "RelatedLoan.DateCompleted";
    ///       closedCriterion.Value = DateTime.Now;
    ///       closedCriterion.MatchType = OrdinalFieldMatchType.Equals;
    ///       closedCriterion.Precision = DateFieldMatchPrecision.Year;
    /// 
    ///       // Join the two criteria together with an AND
    ///       QueryCriterion jointCriterion = amtCriterion.And(closedCriterion);
    /// 
    ///       // Execute the query agains the Borrowers
    ///       ContactList contacts = session.Contacts.Query(jointCriterion, ContactLoanMatchType.Any, ContactType.Borrower);
    /// 
    ///       // Dump all of the contacts that matched
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          Console.WriteLine(contacts[i].FirstName + " " + contacts[i].LastName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactList Query(
      EllieMae.Encompass.Query.QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      ContactType type)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (type == ContactType.Biz)
        return BizContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBizPartners(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[1]
        {
          criterion.Unwrap()
        }, (RelatedLoanMatchType) loanMatchType));
      if (type != ContactType.Borrower)
        throw new ArgumentException("Invalid contact type specified");
      return BorrowerContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBorrowers(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[1]
      {
        criterion.Unwrap()
      }, (RelatedLoanMatchType) loanMatchType));
    }

    /// <summary>
    /// Executes a query and returns the result in a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactCursor" /> for
    /// optimal performance.
    /// </summary>
    /// <param name="criterion">The criterion to use to perform the query. This
    /// can be a single criterion or a logical combination of objects which
    /// implement the IQueryCriterion interface.</param>
    /// <param name="loanMatchType">The types of related loans that should
    /// be considered when performing this query. This parameters allows you
    /// to locate all contacts who satisfy certain loan-related criteria.</param>
    /// <param name="sortCriteria">The sort criteria to be used to sort the
    /// contacts. A <c>null</c> value (<c>Nothing</c> in Visual Basic) can be passed
    /// to specify that no sort be applied.</param>
    /// <param name="type">The type of contacts being queried.</param>
    /// <returns>The list of <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact">Contact</see> objects that
    /// matched the specified criteria.
    /// </returns>
    /// <remarks>A cursor represents a server-side resource that consumes memory
    /// on the Encompass Server. To release this memory, be sure to call the cursor's
    /// Close() method when the cursor is no longer needed.</remarks>
    public ContactCursor QueryCursor(
      EllieMae.Encompass.Query.QueryCriterion criterion,
      ContactLoanMatchType loanMatchType,
      SortCriterionList sortCriteria,
      ContactType type)
    {
      if (criterion == null)
        throw new ArgumentNullException(nameof (criterion));
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (type == ContactType.Biz)
        return new ContactCursor(this.Session, this.ContactManager.OpenBizPartnerCursor(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[1]
        {
          criterion.Unwrap()
        }, (RelatedLoanMatchType) loanMatchType, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Biz);
      if (type != ContactType.Borrower)
        throw new ArgumentException("Invalid contact type specified");
      return new ContactCursor(this.Session, this.ContactManager.OpenBorrowerCursor(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[1]
      {
        criterion.Unwrap()
      }, (RelatedLoanMatchType) loanMatchType, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Borrower);
    }

    /// <summary>
    /// Returns a list of all <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BizContact">BizContact</see> or
    /// <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact">BorrowerContact</see> objects defined
    /// in the Contact Database.
    /// </summary>
    /// <param name="type">The type of contacts being retrieved.</param>
    /// <returns>When the <c>type</c> parameter is Borrower and the currently logged in
    /// user is not an administrator, this function returns the list of only those business contacts
    /// assigned to that user. In all other cases, this function will return all available
    /// contacts of the specified type.</returns>
    /// <example>
    /// The following code retrieves a list of all of the Business Contacts in
    /// the contacts database.
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
    ///       // Fetch the list of Business contacts from the server
    ///       ContactList contacts = session.Contacts.GetAll(ContactType.Biz);
    /// 
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          Console.WriteLine(contacts[i].FirstName + " " + contacts[i].LastName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ContactList GetAll(ContactType type)
    {
      if (type == ContactType.Biz)
        return BizContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBizPartners(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[0], RelatedLoanMatchType.None));
      if (type == ContactType.Borrower)
        return BorrowerContact.ToList(this.Session, this.ContactManager.QueryMaxAccessibleBorrowers(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[0], RelatedLoanMatchType.None));
      throw new ArgumentException("Invalid contact type specified");
    }

    /// <summary>
    /// Opens a <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.ContactCursor" /> that contains all contacts for the current
    /// user.
    /// </summary>
    /// <param name="sortCriteria">The sort criteria to be used to sort the
    /// contacts. A <c>null</c> value (<c>Nothing</c> in Visual Basic) can be passed
    /// to specify that no sort be applied.</param>
    /// <param name="type">The type of contacts being queried.</param>
    /// <returns>A cursor containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Contacts.Contact">Contact</see> objects
    /// to which the user has access.
    /// </returns>
    /// <remarks>A cursor represents a server-side resource that consumes memory
    /// on the Encompass Server. To release this memory, be sure to call the cursor's
    /// Close() method when the cursor is no longer needed.</remarks>
    public ContactCursor OpenCursor(SortCriterionList sortCriteria, ContactType type)
    {
      if (sortCriteria == null)
        sortCriteria = new SortCriterionList();
      if (type == ContactType.Biz)
        return new ContactCursor(this.Session, this.ContactManager.OpenMaxAccessibleBizPartnerCursor(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[0], RelatedLoanMatchType.None, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Biz);
      if (type == ContactType.Borrower)
        return new ContactCursor(this.Session, this.ContactManager.OpenMaxAccessibleBorrowerCursor(new EllieMae.EMLite.ClientServer.Query.QueryCriterion[0], RelatedLoanMatchType.None, sortCriteria.ToSortFieldList(), (string[]) null, false), ContactType.Borrower);
      throw new ArgumentException("Invalid contact type specified");
    }

    /// <summary>
    /// Returns the enumeration of all defined <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see>
    /// objects.
    /// </summary>
    /// <example>
    /// The following code creates a new Business Contact, populates it contact
    /// information and commits it to the server.
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
    ///       // Open the contact specified on the command line
    ///       Contact contact = session.Contacts.CreateNew(ContactType.Biz);
    /// 
    ///       // Set the contact's personal infomation
    ///       contact.FirstName = "Allison";
    ///       contact.LastName = "Meriwether";
    ///       contact.BizEmail = "allison@somecompany.com";
    /// 
    ///       // Set the Business Category
    ///       BizContact biz = (contact as BizContact);
    ///       biz.Category = session.Contacts.BizCategories.GetItemByName("Appraiser");
    /// 
    ///       // Save to the server
    ///       contact.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public BizCategories BizCategories
    {
      get
      {
        if (this.bizCategories == null)
          this.bizCategories = new BizCategories(this.ContactManager);
        return this.bizCategories;
      }
    }

    internal IContactManager ContactManager
    {
      get => (IContactManager) this.Session.GetObject(nameof (ContactManager));
    }

    internal ContactCustomFieldInfoCollection GetCustomFieldDefinitions(EllieMae.EMLite.ContactUI.ContactType contactType)
    {
      lock (this)
      {
        ContactCustomFieldInfoCollection customField = (ContactCustomFieldInfoCollection) this.customFields[(object) contactType];
        if (customField != null)
          return customField;
        ContactCustomFieldInfoCollection customFieldInfo = this.Session.Contacts.ContactManager.GetCustomFieldInfo(contactType);
        this.customFields.Add((object) contactType, (object) customFieldInfo);
        return customFieldInfo;
      }
    }
  }
}
