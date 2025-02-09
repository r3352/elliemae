// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BorrowerContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public override int ID => this.contact.ContactID;

    internal override string Guid => this.contact.ContactGuid.ToString();

    public override string FirstName
    {
      get => this.contact.FirstName;
      set => this.contact.FirstName = value ?? "";
    }

    public string MiddleName
    {
      get => this.contact.MiddleName;
      set => this.contact.MiddleName = value ?? "";
    }

    public override string LastName
    {
      get => this.contact.LastName;
      set => this.contact.LastName = value ?? "";
    }

    public string Suffix
    {
      get => this.contact.SuffixName;
      set => this.contact.SuffixName = value ?? "";
    }

    public override string Salutation
    {
      get => this.contact.Salutation;
      set => this.contact.Salutation = value ?? "";
    }

    public Address HomeAddress
    {
      get
      {
        if (this.homeAddress == null)
          this.homeAddress = new Address(this.contact.HomeAddress);
        return this.homeAddress;
      }
    }

    public override Address BizAddress
    {
      get
      {
        if (this.bizAddress == null)
          this.bizAddress = new Address(this.contact.BizAddress);
        return this.bizAddress;
      }
    }

    public override string BizWebUrl
    {
      get => this.contact.BizWebUrl;
      set => this.contact.BizWebUrl = value ?? "";
    }

    public string EmployerName
    {
      get => this.contact.EmployerName;
      set => this.contact.EmployerName = value ?? "";
    }

    public override string JobTitle
    {
      get => this.contact.JobTitle;
      set => this.contact.JobTitle = value ?? "";
    }

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

    public string SocialSecurityNumber
    {
      get => this.contact.SSN;
      set => this.contact.SSN = value ?? "";
    }

    public BorrowerContactType BorrowerType
    {
      get => (BorrowerContactType) this.contact.ContactType;
      set => this.contact.ContactType = (EllieMae.EMLite.Common.Contact.BorrowerType) value;
    }

    public string Status
    {
      get => this.contact.Status;
      set => this.contact.Status = value ?? "";
    }

    public bool NoCall
    {
      get => this.contact.NoCall;
      set => this.contact.NoCall = value;
    }

    public bool NoFax
    {
      get => this.contact.NoFax;
      set => this.contact.NoFax = value;
    }

    public string Referral
    {
      get => this.contact.Referral;
      set => this.contact.Referral = value ?? "";
    }

    public Decimal Income
    {
      get => this.contact.Income;
      set => this.contact.Income = value;
    }

    public override string WorkPhone
    {
      get => this.contact.WorkPhone;
      set => this.contact.WorkPhone = value ?? "";
    }

    public override string HomePhone
    {
      get => this.contact.HomePhone;
      set => this.contact.HomePhone = value ?? "";
    }

    public override string MobilePhone
    {
      get => this.contact.MobilePhone;
      set => this.contact.MobilePhone = value ?? "";
    }

    public override string FaxNumber
    {
      get => this.contact.FaxNumber;
      set => this.contact.FaxNumber = value ?? "";
    }

    public override string PersonalEmail
    {
      get => this.contact.PersonalEmail;
      set => this.contact.PersonalEmail = value ?? "";
    }

    public override string BizEmail
    {
      get => this.contact.BizEmail;
      set => this.contact.BizEmail = value ?? "";
    }

    public object Birthdate
    {
      get
      {
        return !(this.contact.Birthdate == DateTime.MinValue) ? (object) this.contact.Birthdate : (object) null;
      }
      set => this.contact.Birthdate = value == null ? DateTime.MinValue : (DateTime) value;
    }

    void IBorrowerContact.SetBirthdate(object value) => this.Birthdate = value;

    public bool Married
    {
      get => this.contact.Married;
      set => this.contact.Married = value;
    }

    public bool PrimaryContact
    {
      get => this.contact.PrimaryContact;
      set => this.contact.PrimaryContact = value;
    }

    public int SpouseContactID
    {
      get => this.contact.SpouseContactID;
      set => this.contact.SpouseContactID = value;
    }

    public string SpouseName
    {
      get => this.contact.SpouseName;
      set => this.contact.SpouseName = value ?? "";
    }

    public object Anniversary
    {
      get
      {
        return !(this.contact.Anniversary == DateTime.MinValue) ? (object) this.contact.Anniversary : (object) null;
      }
      set => this.contact.Anniversary = value == null ? DateTime.MinValue : (DateTime) value;
    }

    void IBorrowerContact.SetAnniversary(object value) => this.Anniversary = value;

    public override string CustField1
    {
      get => this.contact.CustField1;
      set => this.contact.CustField1 = value ?? "";
    }

    public override string CustField2
    {
      get => this.contact.CustField2;
      set => this.contact.CustField2 = value ?? "";
    }

    public override string CustField3
    {
      get => this.contact.CustField3;
      set => this.contact.CustField3 = value ?? "";
    }

    public override string CustField4
    {
      get => this.contact.CustField4;
      set => this.contact.CustField4 = value ?? "";
    }

    public override bool NoSpam
    {
      get => this.contact.NoSpam;
      set => this.contact.NoSpam = value;
    }

    public override DateTime LastModified => this.contact.LastModified;

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

    public override void Delete()
    {
      if (!this.IsNew)
        this.mngr.DeleteBorrower(this.contact.ContactID);
      this.contact = (BorrowerInfo) null;
    }

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

    public override ContactType Type => ContactType.Borrower;

    internal override ContactType ContactType => (ContactType) 0;

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
