// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BizContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ContactUI;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>
  /// Represents a single business contact within the system. A business contact may be
  /// an appraiser, lender, etc. and can include information on the licenses/fees associated
  /// with this contact.
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
  public class BizContact : Contact, IBizContact
  {
    private BizPartnerInfo contact;
    private User owner;
    private BizCategoryCustomFields categoryFields;
    private Address bizAddress;

    internal BizContact(Session session, BizPartnerInfo contact)
      : base(session)
    {
      this.contact = contact;
    }

    internal BizContact(Session session)
      : base(session)
    {
      this.contact = new BizPartnerInfo(this.Session.UserID);
    }

    /// <summary>Gets the unique ID for the contact.</summary>
    public override int ID => this.contact.ContactID;

    /// <summary>Gets the GUID for the contact.</summary>
    internal override string Guid => this.contact.ContactGuid.ToString();

    /// <summary>
    /// Gets or sets the <see cref="T:EllieMae.Encompass.BusinessEnums.BizCategory">BizCategory</see> associated with this contact.
    /// </summary>
    public EllieMae.Encompass.BusinessEnums.BizCategory Category
    {
      get => this.Session.Contacts.BizCategories.GetItemByID(this.contact.CategoryID);
      set
      {
        this.contact.CategoryID = !((EnumItem) value == (EnumItem) null) ? value.ID : throw new ArgumentNullException(nameof (Category), "Category cannot be null");
      }
    }

    /// <summary>Gets or sets the contact's first name.</summary>
    public override string FirstName
    {
      get => this.contact.FirstName;
      set => this.contact.FirstName = value ?? "";
    }

    /// <summary>Gets or sets the contact's last name.</summary>
    public override string LastName
    {
      get => this.contact.LastName;
      set => this.contact.LastName = value ?? "";
    }

    /// <summary>Gets or sets the contact's salutation.</summary>
    public override string Salutation
    {
      get => this.contact.Salutation;
      set => this.contact.Salutation = value ?? "";
    }

    /// <summary>
    /// Gets or sets the name of the company for which the contact works.
    /// </summary>
    public string CompanyName
    {
      get => this.contact.CompanyName;
      set => this.contact.CompanyName = value ?? "";
    }

    /// <summary>Gets or sets the contact's job title.</summary>
    public override string JobTitle
    {
      get => this.contact.JobTitle;
      set => this.contact.JobTitle = value ?? "";
    }

    /// <summary>Gets the contact's business address.</summary>
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

    /// <summary>Gets or sets the contact's home phone number.</summary>
    public override string HomePhone
    {
      get => this.contact.HomePhone;
      set => this.contact.HomePhone = value ?? "";
    }

    /// <summary>Gets or sets the contact's work phone number.</summary>
    public override string WorkPhone
    {
      get => this.contact.WorkPhone;
      set => this.contact.WorkPhone = value ?? "";
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

    /// <summary>Gets or sets the contact's business email address.</summary>
    public override string BizEmail
    {
      get => this.contact.BizEmail;
      set => this.contact.BizEmail = value ?? "";
    }

    /// <summary>
    /// Gets or sets the contact's license number, if appropriate.
    /// </summary>
    public string LicenseNumber
    {
      get => this.contact.LicenseNumber;
      set => this.contact.LicenseNumber = value ?? "";
    }

    /// <summary>Gets or sets the contact's fees, in dollars.</summary>
    public int Fees
    {
      get => this.contact.Fees;
      set => this.contact.Fees = value;
    }

    /// <summary>
    /// Gets or sets a comment to be associated with this contact.
    /// </summary>
    public string Comment
    {
      get => this.contact.Comment;
      set => this.contact.Comment = value ?? "";
    }

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
    /// Gets the custom fields collections associated with the defined business categories.
    /// </summary>
    public BizCategoryCustomFields BizCategoryCustomFields
    {
      get
      {
        if (this.categoryFields == null)
          this.categoryFields = new BizCategoryCustomFields((Contact) this);
        return this.categoryFields;
      }
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
        this.contact.ContactID = this.mngr.CreateBizPartner(this.contact);
      else
        this.mngr.UpdateBizPartner(this.contact);
      if (this.customFields != null)
        this.customFields.Commit();
      if (this.categoryFields != null)
        this.categoryFields.Commit();
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
        this.mngr.DeleteBizPartner(this.contact.ContactID);
      this.contact = (BizPartnerInfo) null;
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
      BizPartnerInfo bizPartner = this.mngr.GetBizPartner(this.contact.ContactID);
      if (bizPartner == null)
        return;
      this.contact = bizPartner;
      this.owner = (User) null;
      this.bizAddress = (Address) null;
      if (this.customFields != null)
        this.customFields.Refresh();
      if (this.categoryFields == null)
        return;
      this.categoryFields.Refresh();
    }

    /// <summary>Gets the type of contact represented by this object.</summary>
    /// <remarks>This property will always return <see cref="F:EllieMae.Encompass.BusinessObjects.Contacts.ContactType.Biz" />.</remarks>
    public override ContactType Type => ContactType.Biz;

    internal override EllieMae.EMLite.ContactUI.ContactType ContactType => EllieMae.EMLite.ContactUI.ContactType.BizPartner;

    internal BizPartnerInfo Unwrap() => this.contact;

    internal static ContactList ToList(Session session, BizPartnerInfo[] data)
    {
      ContactList list = new ContactList();
      for (int index = 0; index < data.Length; ++index)
        list.Add((Contact) new BizContact(session, data[index]));
      return list;
    }
  }
}
