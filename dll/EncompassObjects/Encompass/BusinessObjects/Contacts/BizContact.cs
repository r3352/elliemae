// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.BizContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public override int ID => this.contact.ContactID;

    internal override string Guid => this.contact.ContactGuid.ToString();

    public BizCategory Category
    {
      get => this.Session.Contacts.BizCategories.GetItemByID(this.contact.CategoryID);
      set
      {
        this.contact.CategoryID = !((EnumItem) value == (EnumItem) null) ? value.ID : throw new ArgumentNullException(nameof (Category), "Category cannot be null");
      }
    }

    public override string FirstName
    {
      get => this.contact.FirstName;
      set => this.contact.FirstName = value ?? "";
    }

    public override string LastName
    {
      get => this.contact.LastName;
      set => this.contact.LastName = value ?? "";
    }

    public override string Salutation
    {
      get => this.contact.Salutation;
      set => this.contact.Salutation = value ?? "";
    }

    public string CompanyName
    {
      get => this.contact.CompanyName;
      set => this.contact.CompanyName = value ?? "";
    }

    public override string JobTitle
    {
      get => this.contact.JobTitle;
      set => this.contact.JobTitle = value ?? "";
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

    public override string HomePhone
    {
      get => this.contact.HomePhone;
      set => this.contact.HomePhone = value ?? "";
    }

    public override string WorkPhone
    {
      get => this.contact.WorkPhone;
      set => this.contact.WorkPhone = value ?? "";
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

    public string LicenseNumber
    {
      get => this.contact.LicenseNumber;
      set => this.contact.LicenseNumber = value ?? "";
    }

    public int Fees
    {
      get => this.contact.Fees;
      set => this.contact.Fees = value;
    }

    public string Comment
    {
      get => this.contact.Comment;
      set => this.contact.Comment = value ?? "";
    }

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

    public BizCategoryCustomFields BizCategoryCustomFields
    {
      get
      {
        if (this.categoryFields == null)
          this.categoryFields = new BizCategoryCustomFields((Contact) this);
        return this.categoryFields;
      }
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

    public override void Delete()
    {
      if (!this.IsNew)
        this.mngr.DeleteBizPartner(this.contact.ContactID);
      this.contact = (BizPartnerInfo) null;
    }

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

    public override ContactType Type => ContactType.Biz;

    internal override ContactType ContactType => (ContactType) 1;

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
