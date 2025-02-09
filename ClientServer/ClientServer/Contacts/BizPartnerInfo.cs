// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BizPartnerInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BizPartnerInfo : IPropertyDictionary
  {
    private int contactID;
    private ContactAccess accessLevel;
    private string ownerID;
    private int categoryID;
    private string firstName;
    private string lastName;
    private string companyName;
    private string jobTitle;
    private Address bizAddress;
    private string bizWebUrl;
    private string homePhone;
    private string workPhone;
    private string mobilePhone;
    private string faxNumber;
    private string personalEmail;
    private string bizEmail;
    private int fees;
    private string comment;
    private string custField1;
    private string custField2;
    private string custField3;
    private string custField4;
    private string primaryEmail;
    private string primaryPhone;
    private bool noSpam;
    private DateTime lastModified;
    private string salutation;
    private Guid guid;
    private BizContactLicenseInfo personalInfoLicense;
    private BizContactLicenseInfo bizContactLicense;
    private Dictionary<string, object> data;

    public BizPartnerInfo()
    {
      this.contactID = -1;
      this.accessLevel = ContactAccess.Private;
      this.ownerID = string.Empty;
      this.categoryID = 0;
      this.firstName = string.Empty;
      this.lastName = string.Empty;
      this.companyName = string.Empty;
      this.jobTitle = string.Empty;
      this.bizAddress = new Address();
      this.bizWebUrl = string.Empty;
      this.homePhone = string.Empty;
      this.workPhone = string.Empty;
      this.mobilePhone = string.Empty;
      this.faxNumber = string.Empty;
      this.personalEmail = string.Empty;
      this.bizEmail = string.Empty;
      this.fees = -1;
      this.comment = string.Empty;
      this.custField1 = string.Empty;
      this.custField2 = string.Empty;
      this.custField3 = string.Empty;
      this.custField4 = string.Empty;
      this.primaryEmail = string.Empty;
      this.primaryPhone = string.Empty;
      this.noSpam = false;
      this.lastModified = DateTime.MinValue;
      this.salutation = string.Empty;
      this.personalInfoLicense = new BizContactLicenseInfo();
      this.bizContactLicense = new BizContactLicenseInfo();
    }

    public BizPartnerInfo(string ownerId)
      : this()
    {
      this.ownerID = ownerId;
    }

    public BizPartnerInfo(Dictionary<string, object> data)
      : this()
    {
      this.data = data;
      this.populatePredefinedField();
    }

    public BizPartnerInfo(
      int contactID,
      ContactAccess accessLevel,
      string ownerID,
      int categoryID,
      string firstName,
      string lastName,
      string companyName,
      string jobTitle,
      Address bizAddress,
      string bizWebUrl,
      string homePhone,
      string workPhone,
      string mobilePhone,
      string faxNumber,
      string personalEmail,
      string bizEmail,
      string licenseNumber,
      int fees,
      string comment,
      string custField1,
      string custField2,
      string custField3,
      string custField4,
      string primaryEmail,
      string primaryPhone,
      bool noSpam,
      DateTime lastModified,
      string salutation,
      Guid guid,
      BizContactLicenseInfo bizContactLicenseInfo,
      BizContactLicenseInfo personalInfoLicenseInfo)
    {
      this.contactID = contactID;
      this.accessLevel = accessLevel;
      this.ownerID = ownerID;
      this.categoryID = categoryID;
      this.firstName = firstName;
      this.lastName = lastName;
      this.companyName = companyName;
      this.jobTitle = jobTitle;
      this.bizAddress = bizAddress == null ? new Address() : bizAddress;
      this.bizWebUrl = bizWebUrl;
      this.homePhone = homePhone;
      this.workPhone = workPhone;
      this.mobilePhone = mobilePhone;
      this.faxNumber = faxNumber;
      this.personalEmail = personalEmail;
      this.bizEmail = bizEmail;
      this.fees = fees;
      this.comment = comment;
      this.custField1 = custField1;
      this.custField2 = custField2;
      this.custField3 = custField3;
      this.custField4 = custField4;
      this.primaryEmail = primaryEmail;
      this.primaryPhone = primaryPhone;
      this.noSpam = noSpam;
      this.lastModified = lastModified;
      this.salutation = salutation;
      this.guid = guid;
      this.bizContactLicense = bizContactLicenseInfo;
      this.personalInfoLicense = personalInfoLicenseInfo;
      this.bizContactLicense.LicenseNumber = licenseNumber;
    }

    private void populatePredefinedField()
    {
      if (this.getField("Contact.ContactID") != null)
        this.contactID = Utils.ParseInt(this.getField("Contact.ContactID"), -1);
      if (this.getField("Contact.CategoryID") != null)
        this.categoryID = Utils.ParseInt(this.getField("Contact.CategoryID"), -1);
      if (this.getField("Contact.FirstName") != null)
        this.firstName = string.Concat(this.getField("Contact.FirstName"));
      if (this.getField("Contact.LastName") != null)
        this.lastName = string.Concat(this.getField("Contact.LastName"));
      if (this.getField("Contact.CompanyName") != null)
        this.lastName = string.Concat(this.getField("Contact.CompanyName"));
      if (this.getField("Contact.JobTitle") != null)
        this.lastName = string.Concat(this.getField("Contact.JobTitle"));
      if (this.getField("Contact.OwnerID") != null)
        this.ownerID = string.Concat(this.getField("Contact.OwnerID"));
      this.bizAddress = new Address();
      if (this.getField("Contact.BizAddress1") != null)
        this.bizAddress.Street1 = string.Concat(this.getField("Contact.BizAddress1"));
      if (this.getField("Contact.BizAddress2") != null)
        this.bizAddress.Street2 = string.Concat(this.getField("Contact.BizAddress2"));
      if (this.getField("Contact.BizCity") != null)
        this.bizAddress.City = string.Concat(this.getField("Contact.BizCity"));
      if (this.getField("Contact.BizState") != null)
        this.bizAddress.State = string.Concat(this.getField("Contact.BizState"));
      if (this.getField("Contact.BizZip") != null)
        this.bizAddress.Zip = string.Concat(this.getField("Contact.BizZip"));
      if (this.getField("Contact.BizWebUrl") != null)
        this.bizWebUrl = string.Concat(this.getField("Contact.BizWebUrl"));
      if (this.getField("Contact.JobTitle") != null)
        this.jobTitle = string.Concat(this.getField("Contact.JobTitle"));
      if (this.getField("Contact.WorkPhone") != null)
        this.workPhone = string.Concat(this.getField("Contact.WorkPhone"));
      if (this.getField("Contact.HomePhone") != null)
        this.homePhone = string.Concat(this.getField("Contact.HomePhone"));
      if (this.getField("Contact.MobilePhone") != null)
        this.mobilePhone = string.Concat(this.getField("Contact.MobilePhone"));
      if (this.getField("Contact.FaxNumber") != null)
        this.faxNumber = string.Concat(this.getField("Contact.FaxNumber"));
      if (this.getField("Contact.PersonalEmail") != null)
        this.personalEmail = string.Concat(this.getField("Contact.PersonalEmail"));
      if (this.getField("Contact.BizEmail") != null)
        this.bizEmail = string.Concat(this.getField("Contact.BizEmail"));
      if (this.getField("Contact.LicenseNumber") != null)
        this.LicenseNumber = string.Concat(this.getField("Contact.LicenseNumber"));
      if (this.getField("Contact.Fees") != null)
        this.fees = Utils.ParseInt(this.getField("Contact.Fees"), -1);
      if (this.getField("Contact.Comment") != null)
        this.comment = string.Concat(this.getField("Contact.Comment"));
      if (this.getField("Contact.CustField1") != null)
        this.custField1 = string.Concat(this.getField("Contact.CustField1"));
      if (this.getField("Contact.CustField2") != null)
        this.custField2 = string.Concat(this.getField("Contact.CustField2"));
      if (this.getField("Contact.CustField3") != null)
        this.custField3 = string.Concat(this.getField("Contact.CustField3"));
      if (this.getField("Contact.CustField4") != null)
        this.custField4 = string.Concat(this.getField("Contact.CustField4"));
      if (this.getField("Contact.PrimaryEmail") != null)
        this.primaryEmail = string.Concat(this.getField("Contact.PrimaryEmail"));
      if (this.getField("Contact.NoSpam") != null)
        this.noSpam = Convert.ToString(string.Concat(this.getField("Contact.NoSpam"))).ToLower() == "true";
      if (this.getField("Contact.Salutation") != null)
        this.salutation = string.Concat(this.getField("Contact.Salutation"));
      if (this.getField("Contact.AccessLevel") != null)
        this.accessLevel = (ContactAccess) Enum.Parse(typeof (ContactAccess), string.Concat(this.getField("Contact.AccessLevel")), true);
      if (this.getField("Contact.Guid") != null)
        this.guid = new Guid(string.Concat(this.getField("Contact.Guid")));
      this.personalInfoLicense = new BizContactLicenseInfo();
      if (this.getField("PersonalInfoLicenseNumber") != null)
        this.personalInfoLicense.LicenseNumber = string.Concat(this.getField("PersonalInfoLicenseNumber"));
      if (this.getField("PersonalInfoLicenseAuthName") != null)
        this.personalInfoLicense.LicenseAuthName = string.Concat(this.getField("PersonalInfoLicenseAuthName"));
      if (this.getField("PersonalInfoLicenseAuthType") != null)
        this.personalInfoLicense.LicenseAuthType = string.Concat(this.getField("PersonalInfoLicenseAuthType"));
      if (this.getField("PersonalInfoLicenseAuthStateCode") != null)
        this.personalInfoLicense.LicenseStateCode = string.Concat(this.getField("PersonalInfoLicenseAuthStateCode"));
      if (this.getField("PersonalInfoLicenseAuthDate") != null)
        this.personalInfoLicense.LicenseNumber = string.Concat(this.getField("PersonalInfoLicenseAuthDate"));
      this.BizContactLicense = new BizContactLicenseInfo();
      if (this.getField("LicenseNumber") != null)
        this.BizContactLicense.LicenseNumber = string.Concat(this.getField("LicenseNumber"));
      if (this.getField("BizLicenseAuthName") != null)
        this.BizContactLicense.LicenseAuthName = string.Concat(this.getField("BizLicenseAuthName"));
      if (this.getField("BizLicenseAuthType") != null)
        this.BizContactLicense.LicenseAuthType = string.Concat(this.getField("BizLicenseAuthType"));
      if (this.getField("BizLicenseAuthStateCode") != null)
        this.BizContactLicense.LicenseStateCode = string.Concat(this.getField("BizLicenseAuthStateCode"));
      if (this.getField("BizLicenseAuthDate") == null)
        return;
      this.BizContactLicense.LicenseNumber = string.Concat(this.getField("BizLicenseAuthDate"));
    }

    public object Info(string propertyName) => this.getField(propertyName);

    private object getField(string fieldName)
    {
      if (this.data.ContainsKey(fieldName))
        return this.data[fieldName];
      if (this.data.ContainsKey("Contact." + fieldName))
        return this.data["Contact." + fieldName];
      string[] strArray = fieldName.Split('.');
      if (strArray == null || strArray.Length < 2)
        return (object) null;
      return this.data.ContainsKey(strArray[1]) ? this.data[strArray[1]] : (object) null;
    }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "accesslevel":
            return (object) this.accessLevel;
          case "bizaddress1":
            return (object) this.bizAddress.Street1;
          case "bizaddress2":
            return (object) this.bizAddress.Street2;
          case "bizcity":
            return (object) this.bizAddress.City;
          case "bizcontactlicense":
            return (object) this.bizContactLicense;
          case "bizemail":
            return (object) this.bizEmail;
          case "bizstate":
            return (object) this.bizAddress.State;
          case "bizweburl":
            return (object) this.bizWebUrl;
          case "bizzip":
            return (object) this.bizAddress.Zip;
          case "categoryid":
            return (object) this.categoryID;
          case "comment":
            return (object) this.comment;
          case "companyname":
            return (object) this.companyName;
          case "contactid":
            return (object) this.contactID;
          case "custfield1":
            return (object) this.custField1;
          case "custfield2":
            return (object) this.custField2;
          case "custfield3":
            return (object) this.custField3;
          case "custfield4":
            return (object) this.custField4;
          case "defaultemail":
            return !((this.bizEmail ?? "") != "") ? (object) this.personalEmail : (object) this.bizEmail;
          case "faxnumber":
            return (object) this.faxNumber;
          case "fees":
            return (object) this.fees;
          case "firstname":
            return (object) this.firstName;
          case "fullname":
            return (object) this.FullName;
          case "guid":
            return (object) this.guid;
          case "homephone":
            return (object) this.homePhone;
          case "jobtitle":
            return (object) this.jobTitle;
          case "lastmodified":
            return (object) this.lastModified;
          case "lastname":
            return (object) this.lastName;
          case "licensenumber":
            return (object) this.bizContactLicense.LicenseNumber;
          case "mobilephone":
            return (object) this.mobilePhone;
          case "nospam":
            return (object) this.noSpam;
          case "ownerid":
            return (object) this.ownerID;
          case "personalemail":
            return (object) this.PersonalEmail;
          case "personalinfolicense":
            return (object) this.personalInfoLicense;
          case "primaryemail":
            return (object) this.primaryEmail;
          case "primaryphone":
            return (object) this.primaryPhone;
          case "salutation":
            return (object) this.salutation;
          case "workphone":
            return (object) this.workPhone;
          default:
            return (object) null;
        }
      }
      set
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "bizaddress1":
            this.BizAddress.Street1 = string.Concat(value);
            break;
          case "bizaddress2":
            this.BizAddress.Street2 = string.Concat(value);
            break;
          case "bizcity":
            this.BizAddress.City = string.Concat(value);
            break;
          case "bizcontactlicense":
            this.bizContactLicense = (BizContactLicenseInfo) value;
            break;
          case "bizemail":
            this.BizEmail = string.Concat(value);
            break;
          case "bizstate":
            this.BizAddress.State = string.Concat(value);
            break;
          case "bizweburl":
            this.BizWebUrl = string.Concat(value);
            break;
          case "bizzip":
            this.BizAddress.Zip = string.Concat(value);
            break;
          case "categoryid":
            this.CategoryID = Utils.ParseInt(value);
            break;
          case "comment":
            this.Comment = string.Concat(value);
            break;
          case "companyname":
            this.CompanyName = string.Concat(value);
            break;
          case "contactid":
            this.ContactID = Utils.ParseInt(value);
            break;
          case "custfield1":
            this.CustField1 = string.Concat(value);
            break;
          case "custfield2":
            this.CustField2 = string.Concat(value);
            break;
          case "custfield3":
            this.CustField3 = string.Concat(value);
            break;
          case "custfield4":
            this.CustField4 = string.Concat(value);
            break;
          case "faxnumber":
            this.FaxNumber = string.Concat(value);
            break;
          case "fees":
            this.Fees = Utils.ParseInt(value);
            break;
          case "firstname":
            this.FirstName = string.Concat(value);
            break;
          case "homephone":
            this.HomePhone = string.Concat(value);
            break;
          case "jobtitle":
            this.JobTitle = string.Concat(value);
            break;
          case "lastname":
            this.LastName = string.Concat(value);
            break;
          case "licensenumber":
            this.bizContactLicense.LicenseNumber = string.Concat(value);
            break;
          case "mobilephone":
            this.MobilePhone = string.Concat(value);
            break;
          case "nospam":
            this.NoSpam = Utils.ParseBoolean(value);
            break;
          case "personalemail":
            this.PersonalEmail = string.Concat(value);
            break;
          case "personalinfolicense":
            this.personalInfoLicense = (BizContactLicenseInfo) value;
            break;
          case "salutation":
            this.salutation = string.Concat(value);
            break;
          case "workphone":
            this.WorkPhone = string.Concat(value);
            break;
          default:
            throw new ArgumentException("Invalid field name \"" + columnName + "\"");
        }
      }
    }

    public int ContactID
    {
      get => this.contactID;
      set => this.contactID = value;
    }

    public ContactAccess AccessLevel
    {
      get => this.accessLevel;
      set => this.accessLevel = value;
    }

    public string OwnerID
    {
      get => this.ownerID;
      set => this.ownerID = value;
    }

    public int CategoryID
    {
      get => this.categoryID;
      set => this.categoryID = value;
    }

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value;
    }

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value;
    }

    public string FullName => this.FirstName + " " + this.LastName;

    public string CompanyName
    {
      get => this.companyName;
      set
      {
        if (value != null && value.Length > 64)
          this.companyName = value.Substring(0, 64);
        else
          this.companyName = value;
      }
    }

    public string JobTitle
    {
      get => this.jobTitle;
      set => this.jobTitle = value;
    }

    public Address BizAddress => this.bizAddress;

    public string BizWebUrl
    {
      get => this.bizWebUrl;
      set => this.bizWebUrl = value;
    }

    public string HomePhone
    {
      get => this.homePhone;
      set => this.homePhone = value;
    }

    public string WorkPhone
    {
      get => this.workPhone;
      set => this.workPhone = value;
    }

    public string MobilePhone
    {
      get => this.mobilePhone;
      set => this.mobilePhone = value;
    }

    public string FaxNumber
    {
      get => this.faxNumber;
      set => this.faxNumber = value;
    }

    public string PersonalEmail
    {
      get => this.personalEmail;
      set => this.personalEmail = value;
    }

    public string BizEmail
    {
      get => this.bizEmail;
      set => this.bizEmail = value;
    }

    public string LicenseNumber
    {
      get => this.bizContactLicense.LicenseNumber;
      set => this.bizContactLicense.LicenseNumber = value;
    }

    public int Fees
    {
      get => this.fees;
      set => this.fees = value;
    }

    public string Comment
    {
      get => this.comment;
      set => this.comment = value;
    }

    public string CustField1
    {
      get => this.custField1;
      set => this.custField1 = value;
    }

    public string CustField2
    {
      get => this.custField2;
      set => this.custField2 = value;
    }

    public string CustField3
    {
      get => this.custField3;
      set => this.custField3 = value;
    }

    public string CustField4
    {
      get => this.custField4;
      set => this.custField4 = value;
    }

    public string PrimaryEmail
    {
      get => this.primaryEmail;
      set => this.primaryEmail = value;
    }

    public string PrimaryPhone
    {
      get => this.primaryPhone;
      set => this.primaryPhone = value;
    }

    public bool NoSpam
    {
      get => this.noSpam;
      set => this.noSpam = value;
    }

    public string Salutation
    {
      get => this.salutation;
      set => this.salutation = value;
    }

    public DateTime LastModified => this.lastModified;

    public Guid ContactGuid
    {
      get => this.guid;
      set => this.guid = value;
    }

    public BizContactLicenseInfo PersonalInfoLicense
    {
      get => this.personalInfoLicense;
      set => this.personalInfoLicense = value;
    }

    public BizContactLicenseInfo BizContactLicense
    {
      get => this.bizContactLicense;
      set => this.bizContactLicense = value;
    }
  }
}
