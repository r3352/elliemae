// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents a single borrower contact within the system. A borrower contact may be
  /// an individual who has previously closed a loan or could simply be a current or
  /// prospective client.
  /// </summary>
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
  public class BorrowerContact : EllieMae.Encompass.BusinessObjects.Contacts.Contact, IBorrowerContact
  {
    private BorrowerInfo contact;
    private User owner;
    private ContactOpportunities opportunities;
    private Address homeAddress;
    private Address bizAddress;

    internal BorrowerContact(Session session, BorrowerInfo contact)
      : base(session)
    {
      this.contact = contact;
    }

    internal BorrowerContact(Session session)
      : base(session)
    {
      this.contact = new BorrowerInfo(session.UserID);
    }

    /// <summary>Gets the unique ID for the contact.</summary>
    public override int ID => this.contact.ContactID;

    /// <summary>Gets the GUID for the contact.</summary>
    internal override string Guid => this.contact.ContactGuid.ToString();

    /// <summary>Gets or sets the contact's first name.</summary>
    public override string FirstName
    {
      get => this.contact.FirstName;
      set => this.contact.FirstName = value ?? "";
    }

    /// <summary>Gets or sets the contact's middle name.</summary>
    public string MiddleName
    {
      get => this.contact.MiddleName;
      set => this.contact.MiddleName = value ?? "";
    }

    /// <summary>Gets or sets the contact's last name.</summary>
    public override string LastName
    {
      get => this.contact.LastName;
      set => this.contact.LastName = value ?? "";
    }

    /// <summary>Gets or sets the suffix for the contact's name.</summary>
    public string Suffix
    {
      get => this.contact.SuffixName;
      set => this.contact.SuffixName = value ?? "";
    }

    /// <summary>Gets or sets the contact's salutation.</summary>
    public override string Salutation
    {
      get => this.contact.Salutation;
      set => this.contact.Salutation = value ?? "";
    }

    /// <summary>Gets the contact's home address.</summary>
    public Address HomeAddress
    {
      get
      {
        if (this.homeAddress == null)
          this.homeAddress = new Address(this.contact.HomeAddress);
        return this.homeAddress;
      }
    }

    /// <summary>Gets or sets the contact's work address.</summary>
    public override Address BizAddress
    {
      get
      {
        if (this.bizAddress == null)
          this.bizAddress = new Address(this.contact.BizAddress);
        return this.bizAddress;
      }
    }

    /// <summary>
    /// Gets or sets the URL of the contact's business web site.
    /// </summary>
    public override string BizWebUrl
    {
      get => this.contact.BizWebUrl;
      set => this.contact.BizWebUrl = value ?? "";
    }

    /// <summary>
    /// Gets or sets the name of the contact's current employer.
    /// </summary>
    public string EmployerName
    {
      get => this.contact.EmployerName;
      set => this.contact.EmployerName = value ?? "";
    }

    /// <summary>Gets or sets the contact's job title.</summary>
    public override string JobTitle
    {
      get => this.contact.JobTitle;
      set => this.contact.JobTitle = value ?? "";
    }

    /// <summary>
    /// Gets or sets the access level for the contact, which determines which users
    /// can view/modify this object.
    /// </summary>
    public override ContactAccessLevel AccessLevel
    {
      get => (ContactAccessLevel) this.contact.AccessLevel;
      set
      {
        UserInfo userInfo = this.Session.Unwrap().GetUser().GetUserInfo();
        if (userInfo.Userid != this.contact.OwnerID && !userInfo.IsAdministrator())
          throw new InvalidOperationException("Property or method can only be invoked by an administrative user of the owner of the contact.");
        this.contact.AccessLevel = (ContactAccess) value;
      }
    }

    /// <summary>Gets or sets the borrower's social security number.</summary>
    public string SocialSecurityNumber
    {
      get => this.contact.SSN;
      set => this.contact.SSN = value ?? "";
    }

    /// <summary>
    /// Gets or sets the type of borrower this contact represents.
    /// </summary>
    public BorrowerContactType BorrowerType
    {
      get => (BorrowerContactType) this.contact.ContactType;
      set => this.contact.ContactType = (EllieMae.EMLite.Common.Contact.BorrowerType) value;
    }

    /// <summary>Gets or sets the borrower's status.</summary>
    public string Status
    {
      get => this.contact.Status;
      set => this.contact.Status = value ?? "";
    }

    /// <summary>
    /// Gets or sets a flag indicating if this contact can be called.
    /// </summary>
    public bool NoCall
    {
      get => this.contact.NoCall;
      set => this.contact.NoCall = value;
    }

    /// <summary>
    /// Gets or sets a flag indicating if this contact can be faxed.
    /// </summary>
    public bool NoFax
    {
      get => this.contact.NoFax;
      set => this.contact.NoFax = value;
    }

    /// <summary>
    /// Gets or sets a field which provides information regarding how this borrower was referred.
    /// </summary>
    public string Referral
    {
      get => this.contact.Referral;
      set => this.contact.Referral = value ?? "";
    }

    /// <summary>
    /// Gets or sets a field indicating the borrower's income.
    /// </summary>
    public Decimal Income
    {
      get => this.contact.Income;
      set => this.contact.Income = value;
    }

    /// <summary>Gets or sets the contact's work phone number.</summary>
    public override string WorkPhone
    {
      get => this.contact.WorkPhone;
      set => this.contact.WorkPhone = value ?? "";
    }

    /// <summary>Gets or sets the contact's home phone number.</summary>
    public override string HomePhone
    {
      get => this.contact.HomePhone;
      set => this.contact.HomePhone = value ?? "";
    }

    /// <summary>Gets or sets the contact's mobile phone number.</summary>
    public override string MobilePhone
    {
      get => this.contact.MobilePhone;
      set => this.contact.MobilePhone = value ?? "";
    }

    /// <summary>Gets or sets the contact's fax number.</summary>
    public override string FaxNumber
    {
      get => this.contact.FaxNumber;
      set => this.contact.FaxNumber = value ?? "";
    }

    /// <summary>Gets or sets the contact's personal email address.</summary>
    public override string PersonalEmail
    {
      get => this.contact.PersonalEmail;
      set => this.contact.PersonalEmail = value ?? "";
    }

    /// <summary>Gets or sets the contact's work email address.</summary>
    public override string BizEmail
    {
      get => this.contact.BizEmail;
      set => this.contact.BizEmail = value ?? "";
    }

    /// <summary>Gets or sets the contact's birthdate.</summary>
    /// <remarks>
    /// <p>Note to COM clients: Because of a limitation in the way COM marshals the date values
    /// when exposed as a VARIANT, you should call the <c>SetBirthdate()</c> method
    /// to set this property.</p>
    /// </remarks>
    public object Birthdate
    {
      get
      {
        return !(this.contact.Birthdate == DateTime.MinValue) ? (object) this.contact.Birthdate : (object) null;
      }
      set => this.contact.Birthdate = value == null ? DateTime.MinValue : (DateTime) value;
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the Birthdate property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void IBorrowerContact.SetBirthdate(object value) => this.Birthdate = value;

    /// <summary>
    /// Gets or sets a flag indicating if the contact is married.
    /// </summary>
    public bool Married
    {
      get => this.contact.Married;
      set => this.contact.Married = value;
    }

    /// <summary>
    /// Gets or sets a flag indicating if the contact is a primary contact.
    /// </summary>
    public bool PrimaryContact
    {
      get => this.contact.PrimaryContact;
      set => this.contact.PrimaryContact = value;
    }

    /// <summary>
    /// Gets or sets the ID of the contact which is this contact's spouse.
    /// </summary>
    public int SpouseContactID
    {
      get => this.contact.SpouseContactID;
      set => this.contact.SpouseContactID = value;
    }

    /// <summary>Gets or sets the name of the contact's spouse.</summary>
    public string SpouseName
    {
      get => this.contact.SpouseName;
      set => this.contact.SpouseName = value ?? "";
    }

    /// <summary>
    /// Gets or sets the contact's anniversary date. Only the month and day value of the
    /// returned DateTime object should be considered valid.
    /// </summary>
    /// <remarks>
    /// <p>Note to COM clients: Because of a limitation in the way COM marshals the date values
    /// when exposed as a VARIANT, you should call the <c>SetAnniversary()</c> method
    /// to set this property.</p>
    /// </remarks>
    public object Anniversary
    {
      get
      {
        return !(this.contact.Anniversary == DateTime.MinValue) ? (object) this.contact.Anniversary : (object) null;
      }
      set => this.contact.Anniversary = value == null ? DateTime.MinValue : (DateTime) value;
    }

    /// <summary>
    /// Interface method for COM components that cannot set the date directly by using
    /// the Anniversary property.
    /// </summary>
    /// <param name="value">The new date or the Empty variant to clear the date.</param>
    void IBorrowerContact.SetAnniversary(object value) => this.Anniversary = value;

    /// <summary>
    /// Gets or sets the first custom field value for the contact.
    /// </summary>
    public override string CustField1
    {
      get => this.contact.CustField1;
      set => this.contact.CustField1 = value ?? "";
    }

    /// <summary>
    /// Gets or sets the second custom field value for the contact.
    /// </summary>
    public override string CustField2
    {
      get => this.contact.CustField2;
      set => this.contact.CustField2 = value ?? "";
    }

    /// <summary>
    /// Gets or sets the third custom field value for the contact.
    /// </summary>
    public override string CustField3
    {
      get => this.contact.CustField3;
      set => this.contact.CustField3 = value ?? "";
    }

    /// <summary>
    /// Gets or sets the fourth custom field value for the contact.
    /// </summary>
    public override string CustField4
    {
      get => this.contact.CustField4;
      set => this.contact.CustField4 = value ?? "";
    }

    /// <summary>
    /// Gets or sets a flag indicating if the user should receive mass e-mailings.
    /// </summary>
    public override bool NoSpam
    {
      get => this.contact.NoSpam;
      set => this.contact.NoSpam = value;
    }

    /// <summary>
    /// Gets the last modification date and time for this contact.
    /// </summary>
    public override DateTime LastModified => this.contact.LastModified;

    /// <summary>
    /// Gets or sets the Encompass User to whom the contact belongs.
    /// </summary>
    /// <remarks>The currently logged in user must have the Administrator
    /// persona in order to modify the owner of a contact.</remarks>
    public override User Owner
    {
      get
      {
        if (this.owner == null && this.contact.OwnerID != "")
          this.owner = this.Session.Users.GetUser(this.contact.OwnerID);
        return this.owner;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException(nameof (value), "The Owner cannot be set to null.");
        if (!this.Session.Unwrap().GetUser().GetUserInfo().IsAdministrator())
          throw new InvalidOperationException("Property or method can only be invoked by an administrative user");
        this.owner = value;
        this.contact.OwnerID = value.ID;
      }
    }

    /// <summary>
    /// Gets the set of opportunities associated with this contact.
    /// </summary>
    /// <remarks>This property is not accessible until the contact has been
    /// committed to the server.</remarks>
    public ContactOpportunities Opportunities
    {
      get
      {
        this.ensureExists();
        if (this.opportunities == null)
          this.opportunities = new ContactOpportunities((EllieMae.Encompass.BusinessObjects.Contacts.Contact) this);
        return this.opportunities;
      }
    }

    /// <summary>
    /// Saves any pending changes to the current contact to the server or creates a new
    /// contact if not previously committed.
    /// </summary>
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
    public override void Commit()
    {
      if (this.contact.ContactID < 0)
        this.contact.ContactID = this.mngr.CreateBorrower(this.contact);
      else
        this.mngr.UpdateBorrower(this.contact);
      if (this.customFields != null)
        this.customFields.Commit();
      this.Refresh();
      this.OnCommitted();
    }

    /// <summary>
    /// Deletes the current contact from the contact database.
    /// </summary>
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
    public override void Delete()
    {
      if (!this.IsNew)
        this.mngr.DeleteBorrower(this.contact.ContactID);
      this.contact = (BorrowerInfo) null;
    }

    /// <summary>
    /// Refreshes the object from the server. Any uncommitted changes made to the object
    /// are lost.
    /// </summary>
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
    public override void Refresh()
    {
      this.ensureExists();
      BorrowerInfo borrower = this.mngr.GetBorrower(this.contact.ContactID);
      if (borrower == null)
        return;
      this.contact = borrower;
      this.owner = (User) null;
      this.homeAddress = (Address) null;
      this.bizAddress = (Address) null;
      if (this.customFields == null)
        return;
      this.customFields.Refresh();
    }

    /// <summary>Gets the type of contact represented by this object.</summary>
    /// <remarks>This property will always return <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.ContactType.Borrower" />.</remarks>
    public override ContactType Type => ContactType.Borrower;

    internal override EllieMae.EMLite.ContactUI.ContactType ContactType => EllieMae.EMLite.ContactUI.ContactType.Borrower;

    internal BorrowerInfo Unwrap() => this.contact;

    internal static ContactList ToList(Session session, BorrowerInfo[] data)
    {
      ContactList list = new ContactList();
      for (int index = 0; index < data.Length; ++index)
        list.Add((EllieMae.Encompass.BusinessObjects.Contacts.Contact) new BorrowerContact(session, data[index]));
      return list;
    }
  }
}
