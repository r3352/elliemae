// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Contacts.BorrowerInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI;
using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Contacts
{
  [Serializable]
  public class BorrowerInfo : IPropertyDictionary
  {
    private int contactID;
    private string firstName;
    private string lastName;
    private string middleName;
    private string suffixName;
    private Address homeAddress;
    private Address bizAddress;
    private string bizWebUrl;
    private string employerName;
    private string jobTitle;
    private string workPhone;
    private string homePhone;
    private string mobilePhone;
    private string faxNumber;
    private string personalEmail;
    private string bizEmail;
    private DateTime birthdate;
    private bool married;
    private int spouseContactID;
    private string spouseName;
    private DateTime anniversary;
    private string custField1;
    private string custField2;
    private string custField3;
    private string custField4;
    private string primaryEmail;
    private string primaryPhone;
    private bool noSpam;
    private string ownerId;
    private ContactAccess accessLevel;
    private BorrowerType contactType;
    private string status;
    private bool noCall;
    private bool noFax;
    private string ssn;
    private Decimal income;
    private string referral;
    private string leadSource;
    private string leadTxnId;
    private DateTime lastModified;
    private bool primaryContact;
    private string salutation;
    private Guid guid;
    private Hashtable data;

    public BorrowerInfo()
    {
      this.contactID = -1;
      this.firstName = string.Empty;
      this.lastName = string.Empty;
      this.middleName = string.Empty;
      this.suffixName = string.Empty;
      this.ownerId = string.Empty;
      this.accessLevel = ContactAccess.Private;
      this.homeAddress = new Address();
      this.bizAddress = new Address();
      this.bizWebUrl = string.Empty;
      this.employerName = string.Empty;
      this.jobTitle = string.Empty;
      this.workPhone = string.Empty;
      this.homePhone = string.Empty;
      this.mobilePhone = string.Empty;
      this.faxNumber = string.Empty;
      this.personalEmail = string.Empty;
      this.bizEmail = string.Empty;
      this.birthdate = DateTime.MinValue;
      this.married = false;
      this.spouseContactID = -1;
      this.spouseName = string.Empty;
      this.anniversary = DateTime.MinValue;
      this.custField1 = string.Empty;
      this.custField2 = string.Empty;
      this.custField3 = string.Empty;
      this.custField4 = string.Empty;
      this.primaryEmail = string.Empty;
      this.primaryPhone = string.Empty;
      this.noSpam = false;
      this.contactType = BorrowerType.Prospect;
      this.status = string.Empty;
      this.noCall = false;
      this.noFax = false;
      this.ssn = string.Empty;
      this.income = 0M;
      this.referral = string.Empty;
      this.leadSource = string.Empty;
      this.leadTxnId = string.Empty;
      this.lastModified = DateTime.MinValue;
      this.primaryContact = true;
      this.salutation = string.Empty;
    }

    public BorrowerInfo(string ownerId)
      : this()
    {
      this.OwnerID = ownerId;
    }

    public BorrowerInfo(Hashtable data)
      : this()
    {
      this.data = data;
      this.populatePredefinedField();
    }

    public BorrowerInfo(
      int contactID,
      string firstName,
      string middleName,
      string lastName,
      string suffixName,
      string ownerId,
      ContactAccess accessLevel,
      Address homeAddress,
      Address bizAddress,
      string bizWebUrl,
      string employerName,
      string jobTitle,
      string workPhone,
      string homePhone,
      string mobilePhone,
      string faxNumber,
      string personalEmail,
      string bizEmail,
      DateTime birthdate,
      bool married,
      int spouseContactID,
      string spouseName,
      DateTime anniversary,
      string custField1,
      string custField2,
      string custField3,
      string custField4,
      string primaryEmail,
      string primaryPhone,
      bool noSpam,
      BorrowerType contactType,
      string status,
      bool noCall,
      bool noFax,
      string ssn,
      Decimal income,
      string referral,
      string leadSource,
      string leadTxnId,
      DateTime lastModified,
      bool primaryContact,
      string salutation,
      Guid guid)
    {
      this.contactID = contactID;
      this.firstName = firstName;
      this.lastName = lastName;
      this.middleName = middleName;
      this.suffixName = suffixName;
      this.ownerId = ownerId;
      this.accessLevel = accessLevel;
      this.homeAddress = homeAddress == null ? new Address() : homeAddress;
      this.bizAddress = bizAddress == null ? new Address() : bizAddress;
      this.bizWebUrl = bizWebUrl;
      this.employerName = employerName;
      this.jobTitle = jobTitle;
      this.workPhone = workPhone;
      this.homePhone = homePhone;
      this.mobilePhone = mobilePhone;
      this.faxNumber = faxNumber;
      this.personalEmail = personalEmail;
      this.bizEmail = bizEmail;
      this.birthdate = birthdate;
      this.married = married;
      this.spouseContactID = spouseContactID;
      this.spouseName = spouseName;
      this.anniversary = anniversary;
      this.custField1 = custField1;
      this.custField2 = custField2;
      this.custField3 = custField3;
      this.custField4 = custField4;
      this.primaryEmail = primaryEmail;
      this.primaryPhone = primaryPhone;
      this.noSpam = noSpam;
      this.contactType = contactType;
      this.status = status;
      this.noCall = noCall;
      this.noFax = noFax;
      this.ssn = ssn;
      this.income = income;
      this.referral = referral;
      this.leadSource = leadSource;
      this.leadTxnId = leadTxnId;
      this.lastModified = lastModified;
      this.primaryContact = primaryContact;
      this.salutation = salutation;
      this.guid = guid;
    }

    private void populatePredefinedField()
    {
      if (this.getField("Contact.ContactID") != null)
        this.contactID = Utils.ParseInt(this.getField("Contact.ContactID"), -1);
      if (this.getField("Contact.FirstName") != null)
        this.firstName = string.Concat(this.getField("Contact.FirstName"));
      if (this.getField("Contact.LastName") != null)
        this.lastName = string.Concat(this.getField("Contact.LastName"));
      if (this.getField("Contact.MiddleName") != null)
        this.middleName = string.Concat(this.getField("Contact.MiddleName"));
      if (this.getField("Contact.SuffixName") != null)
        this.suffixName = string.Concat(this.getField("Contact.SuffixName"));
      if (this.getField("Contact.OwnerID") != null)
        this.ownerId = string.Concat(this.getField("Contact.OwnerID"));
      this.homeAddress = new Address();
      if (this.getField("Contact.HomeAddress1") != null)
        this.homeAddress.Street1 = string.Concat(this.getField("Contact.HomeAddress1"));
      if (this.getField("Contact.HomeAddress2") != null)
        this.homeAddress.Street2 = string.Concat(this.getField("Contact.HomeAddress2"));
      if (this.getField("Contact.HomeCity") != null)
        this.homeAddress.City = string.Concat(this.getField("Contact.HomeCity"));
      if (this.getField("Contact.HomeState") != null)
        this.homeAddress.State = string.Concat(this.getField("Contact.HomeState"));
      if (this.getField("Contact.HomeZip") != null)
        this.homeAddress.Zip = string.Concat(this.getField("Contact.HomeZip"));
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
      if (this.getField("Contact.EmployerName") != null)
        this.employerName = string.Concat(this.getField("Contact.EmployerName"));
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
      if (this.getField("Contact.Birthdate") != null)
        this.birthdate = Utils.ParseDate(this.getField("Contact.Birthdate"), DateTime.MinValue);
      if (this.getField("Contact.Married") != null)
        this.married = Convert.ToString(string.Concat(this.getField("Contact.Married"))).ToLower() == "true";
      if (this.getField("Contact.SpouseContactID") != null)
        this.spouseContactID = Utils.ParseInt(this.getField("ontact.SpouseContactID"), -1);
      if (this.getField("Contact.SpouseName") != null)
        this.spouseName = string.Concat(this.getField("Contact.SpouseName"));
      if (this.getField("Contact.Anniversary") != null)
        this.anniversary = Utils.ParseDate(this.getField("Contact.Anniversary"));
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
      if (this.getField("Contact.ContactType") != null)
      {
        string str = string.Concat(this.getField("Contact.ContactType"));
        if (str == "")
          str = BorrowerType.Blank.ToString();
        this.contactType = (BorrowerType) Enum.Parse(typeof (BorrowerType), str, true);
      }
      if (this.getField("Contact.Status") != null)
        this.status = string.Concat(this.getField("Contact.Status"));
      if (this.getField("Contact.NoCall") != null)
        this.noCall = Convert.ToString(string.Concat(this.getField("Contact.NoCall"))).ToLower() == "true";
      if (this.getField("Contact.NoFax") != null)
        this.noFax = Convert.ToString(string.Concat(this.getField("Contact.NoFax"))).ToLower() == "true";
      if (this.getField("Contact.SSN") != null)
        this.ssn = string.Concat(this.getField("Contact.SSN"));
      if (this.getField("Contact.Income") != null)
        this.income = Utils.ParseDecimal(this.getField("Contact.Income"), Decimal.MinValue);
      if (this.getField("Contact.Referral") != null)
        this.referral = string.Concat(this.getField("Contact.Referral"));
      if (this.getField("Contact.LeadSource") != null)
        this.leadSource = string.Concat(this.getField("Contact.LeadSource"));
      if (this.getField("Contact.LeadTxnID") != null)
        this.leadTxnId = string.Concat(this.getField("Contact.LeadTxnID"));
      if (this.getField("Contact.LastModified") != null)
        this.lastModified = Utils.ParseDate(this.getField("Contact.LastModified"), DateTime.MinValue);
      if (this.getField("Contact.PrimContact") != null)
        this.primaryContact = Convert.ToString(string.Concat(this.getField("Contact.PrimContact"))).ToLower() == "y";
      if (this.getField("Contact.Salutation") != null)
        this.salutation = string.Concat(this.getField("Contact.Salutation"));
      if (this.getField("Contact.AccessLevel") != null)
        this.accessLevel = (ContactAccess) Enum.Parse(typeof (ContactAccess), string.Concat(this.getField("Contact.AccessLevel")), true);
      if (this.getField("Contact.Guid") == null)
        return;
      this.guid = new Guid(string.Concat(this.getField("Contact.Guid")));
    }

    public object Info(string propertyName) => this.getField(propertyName);

    private object getField(string fieldName)
    {
      if (this.data.ContainsKey((object) fieldName))
        return this.data[(object) fieldName];
      if (this.data.ContainsKey((object) ("Contact." + fieldName)))
        return this.data[(object) ("Contact." + fieldName)];
      string[] strArray = fieldName.Split('.');
      if (strArray == null || strArray.Length < 2)
        return (object) null;
      return this.data.ContainsKey((object) strArray[1]) ? this.data[(object) strArray[1]] : (object) null;
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
          case "anniversary":
            return (object) this.anniversary;
          case "birthdate":
            return (object) this.birthdate;
          case "bizaddress1":
            return (object) this.bizAddress.Street1;
          case "bizaddress2":
            return (object) this.bizAddress.Street2;
          case "bizcity":
            return (object) this.bizAddress.City;
          case "bizemail":
            return (object) this.bizEmail;
          case "bizstate":
            return (object) this.bizAddress.State;
          case "bizweburl":
            return (object) this.bizWebUrl;
          case "bizzip":
            return (object) this.bizAddress.Zip;
          case "contactid":
            return (object) this.contactID;
          case "contacttype":
            return (object) this.contactType;
          case "custfield1":
            return (object) this.custField1;
          case "custfield2":
            return (object) this.custField2;
          case "custfield3":
            return (object) this.custField3;
          case "custfield4":
            return (object) this.custField4;
          case "defaultemail":
            return !((this.personalEmail ?? "") != "") ? (object) this.bizEmail : (object) this.personalEmail;
          case "employername":
            return (object) this.employerName;
          case "faxnumber":
            return (object) this.faxNumber;
          case "firstname":
            return (object) this.firstName;
          case "fullname":
            return (object) this.FullName;
          case "guid":
            return (object) this.guid;
          case "homeaddress1":
            return (object) this.homeAddress.Street1;
          case "homeaddress2":
            return (object) this.homeAddress.Street2;
          case "homecity":
            return (object) this.homeAddress.City;
          case "homephone":
            return (object) this.homePhone;
          case "homestate":
            return (object) this.homeAddress.State;
          case "homezip":
            return (object) this.homeAddress.Zip;
          case "income":
            return (object) this.income;
          case "jobtitle":
            return (object) this.jobTitle;
          case "lastmodified":
            return (object) this.lastModified;
          case "lastname":
            return (object) this.lastName;
          case "leadsource":
            return (object) this.leadSource;
          case "leadtxnid":
            return (object) this.leadTxnId;
          case "married":
            return (object) this.married;
          case "middlename":
            return (object) this.middleName;
          case "mobilephone":
            return (object) this.mobilePhone;
          case "nocall":
            return (object) this.noCall;
          case "nofax":
            return (object) this.noFax;
          case "nospam":
            return (object) this.noSpam;
          case "ownerid":
            return (object) this.ownerId;
          case "personalemail":
            return (object) this.personalEmail;
          case "primarycontact":
            return (object) this.primaryContact;
          case "primaryemail":
            return (object) this.primaryEmail;
          case "primaryphone":
            return (object) this.primaryPhone;
          case "referral":
            return (object) this.referral;
          case "salutation":
            return (object) this.salutation;
          case "spousecontactid":
            return (object) this.spouseContactID;
          case "spousename":
            return (object) this.spouseName;
          case "ssn":
            return (object) this.ssn;
          case "status":
            return (object) this.status;
          case "suffixname":
            return (object) this.suffixName;
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
          case "anniversary":
            this.anniversary = Utils.ParseDate(value);
            break;
          case "birthdate":
            this.birthdate = Utils.ParseDate(value);
            break;
          case "bizaddress1":
            this.BizAddress.Street1 = string.Concat(value);
            break;
          case "bizaddress2":
            this.BizAddress.Street2 = string.Concat(value);
            break;
          case "bizcity":
            this.BizAddress.City = string.Concat(value);
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
          case "contactid":
            this.ContactID = Utils.ParseInt(value);
            break;
          case "contacttype":
            if (value is string)
            {
              this.contactType = BorrowerTypeEnumUtil.NameToValue(string.Concat(value));
              break;
            }
            this.contactType = (BorrowerType) value;
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
          case "employername":
            this.EmployerName = string.Concat(value);
            break;
          case "faxnumber":
            this.FaxNumber = string.Concat(value);
            break;
          case "firstname":
            this.FirstName = string.Concat(value);
            break;
          case "homeaddress1":
            this.HomeAddress.Street1 = string.Concat(value);
            break;
          case "homeaddress2":
            this.HomeAddress.Street2 = string.Concat(value);
            break;
          case "homecity":
            this.HomeAddress.City = string.Concat(value);
            break;
          case "homephone":
            this.HomePhone = string.Concat(value);
            break;
          case "homestate":
            this.HomeAddress.State = string.Concat(value);
            break;
          case "homezip":
            this.HomeAddress.Zip = string.Concat(value);
            break;
          case "income":
            this.Income = Utils.ParseDecimal(value);
            break;
          case "jobtitle":
            this.JobTitle = string.Concat(value);
            break;
          case "lastname":
            this.LastName = string.Concat(value);
            break;
          case "leadsource":
            this.leadSource = string.Concat(value);
            break;
          case "leadtxnid":
            this.leadTxnId = string.Concat(value);
            break;
          case "married":
            this.married = Utils.ParseBoolean(value);
            break;
          case "middlename":
            this.MiddleName = string.Concat(value);
            break;
          case "mobilephone":
            this.MobilePhone = string.Concat(value);
            break;
          case "nocall":
            this.noCall = Utils.ParseBoolean(value);
            break;
          case "nofax":
            this.noFax = Utils.ParseBoolean(value);
            break;
          case "nospam":
            this.noSpam = Utils.ParseBoolean(value);
            break;
          case "ownerid":
            this.OwnerID = string.Concat(value);
            break;
          case "personalemail":
            this.PersonalEmail = string.Concat(value);
            break;
          case "primarycontact":
            this.primaryContact = Utils.ParseBoolean(value);
            break;
          case "referral":
            this.Referral = string.Concat(value);
            break;
          case "salutation":
            this.salutation = string.Concat(value);
            break;
          case "spousename":
            this.SpouseName = string.Concat(value);
            break;
          case "ssn":
            this.SSN = string.Concat(value);
            break;
          case "status":
            this.status = string.Concat(value);
            break;
          case "suffixname":
            this.SuffixName = string.Concat(value);
            break;
          case "workphone":
            this.WorkPhone = string.Concat(value);
            break;
          default:
            throw new ArgumentException("Invalid field name \"" + columnName + "\"");
        }
      }
    }

    public string ColumnValueToString(string columnName)
    {
      columnName = columnName.ToLower();
      switch (columnName)
      {
        case "accesslevel":
          return this.AccessLevelString;
        case "anniversary":
          return this.AnniversaryString;
        case "birthdate":
          return this.BirthdateString;
        case "bizaddress1":
          return this.bizAddress.Street1;
        case "bizaddress2":
          return this.bizAddress.Street2;
        case "bizcity":
          return this.bizAddress.City;
        case "bizemail":
          return this.bizEmail;
        case "bizstate":
          return this.bizAddress.State;
        case "bizweburl":
          return this.bizWebUrl;
        case "bizzip":
          return this.bizAddress.Zip;
        case "contactid":
          return this.ContactIDString;
        case "contacttype":
          return this.ContactTypeString;
        case "custfield1":
          return this.custField1;
        case "custfield2":
          return this.custField2;
        case "custfield3":
          return this.custField3;
        case "custfield4":
          return this.custField4;
        case "employername":
          return this.employerName;
        case "faxnumber":
          return this.faxNumber;
        case "firstname":
          return this.firstName;
        case "guid":
          return this.guid.ToString();
        case "homeaddress1":
          return this.homeAddress.Street1;
        case "homeaddress2":
          return this.homeAddress.Street2;
        case "homecity":
          return this.homeAddress.City;
        case "homephone":
          return this.homePhone;
        case "homestate":
          return this.homeAddress.State;
        case "homezip":
          return this.homeAddress.Zip;
        case "income":
          return this.IncomeString;
        case "jobtitle":
          return this.jobTitle;
        case "lastmodified":
          return this.lastModified.ToString();
        case "lastname":
          return this.lastName;
        case "leadTxnId":
          return this.leadTxnId;
        case "leadsource":
          return this.leadSource;
        case "married":
          return this.MarriedString;
        case "middlename":
          return this.middleName;
        case "mobilephone":
          return this.mobilePhone;
        case "nocall":
          return this.NoCallString;
        case "nofax":
          return this.NoFaxString;
        case "nospam":
          return this.NoSpamString;
        case "ownerid":
          return this.ownerId;
        case "personalemail":
          return this.personalEmail;
        case "primarycontact":
          return this.PrimaryContactString;
        case "primaryemail":
          return this.primaryEmail;
        case "primaryphone":
          return this.primaryPhone;
        case "referral":
          return this.referral;
        case "salutation":
          return this.salutation;
        case "spousecontactid":
          return this.spouseContactID.ToString();
        case "spousename":
          return this.spouseName;
        case "ssn":
          return this.ssn;
        case "status":
          return this.status;
        case "suffixname":
          return this.suffixName;
        case "workphone":
          return this.workPhone;
        default:
          return (string) null;
      }
    }

    public int ContactID
    {
      get => this.contactID;
      set => this.contactID = value;
    }

    public string ContactIDString => this.contactID.ToString();

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value;
    }

    public string MiddleName
    {
      get => this.middleName;
      set => this.middleName = value;
    }

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value;
    }

    public string SuffixName
    {
      get => this.suffixName;
      set => this.suffixName = value;
    }

    public string FullName
    {
      get
      {
        return ((this.FirstName + " " + this.MiddleName).Trim() + " " + this.LastName + " " + this.SuffixName).Trim();
      }
    }

    public string OwnerID
    {
      get => this.ownerId;
      set => this.ownerId = value;
    }

    public ContactAccess AccessLevel
    {
      get => this.accessLevel;
      set => this.accessLevel = value;
    }

    public string AccessLevelString => this.accessLevel.ToString();

    public Address HomeAddress => this.homeAddress;

    public Address BizAddress => this.bizAddress;

    public string BizWebUrl
    {
      get => this.bizWebUrl;
      set => this.bizWebUrl = value;
    }

    public string EmployerName
    {
      get => this.employerName;
      set
      {
        if (value != null && value.Length > 50)
          this.employerName = value.Substring(0, 50);
        else
          this.employerName = value;
      }
    }

    public string JobTitle
    {
      get => this.jobTitle;
      set => this.jobTitle = value;
    }

    public string WorkPhone
    {
      get => this.workPhone;
      set => this.workPhone = value;
    }

    public string HomePhone
    {
      get => this.homePhone;
      set => this.homePhone = value;
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

    public DateTime Birthdate
    {
      get => this.birthdate;
      set => this.birthdate = value;
    }

    public string BirthdateString
    {
      get
      {
        return !(this.birthdate == DateTime.MinValue) ? this.birthdate.ToString("d", (IFormatProvider) DateTimeFormatInfo.InvariantInfo) : string.Empty;
      }
    }

    public bool Married
    {
      get => this.married;
      set => this.married = value;
    }

    public string MarriedString => !this.married ? "" : "X";

    public string PrimaryContactString => !this.primaryContact ? "" : "X";

    public int SpouseContactID
    {
      get => this.spouseContactID;
      set => this.spouseContactID = value;
    }

    public string SpouseName
    {
      get => this.spouseName;
      set => this.spouseName = value;
    }

    public DateTime Anniversary
    {
      get => this.anniversary;
      set => this.anniversary = value;
    }

    public string AnniversaryString
    {
      get
      {
        return !(this.anniversary == DateTime.MinValue) ? this.anniversary.ToString("MM/dd") : string.Empty;
      }
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

    public bool PrimaryContact
    {
      get => this.primaryContact;
      set => this.primaryContact = value;
    }

    public string Salutation
    {
      get => this.salutation;
      set => this.salutation = value;
    }

    public string NoSpamString => !this.noSpam ? "" : "X";

    public BorrowerType ContactType
    {
      get => this.contactType;
      set => this.contactType = value;
    }

    public string ContactTypeString => BorrowerTypeEnumUtil.ValueToName(this.contactType);

    public string Status
    {
      get => this.status;
      set => this.status = value;
    }

    public bool NoCall
    {
      get => this.noCall;
      set => this.noCall = value;
    }

    public string NoCallString => !this.noCall ? "" : "X";

    public bool NoFax
    {
      get => this.noFax;
      set => this.noFax = value;
    }

    public string NoFaxString => !this.noFax ? "" : "X";

    public string SSN
    {
      get => this.ssn;
      set => this.ssn = value;
    }

    public Decimal Income
    {
      get => this.income;
      set => this.income = value;
    }

    public string IncomeString => !(this.income == 0M) ? this.income.ToString("N0") : "";

    public string Referral
    {
      get => this.referral;
      set => this.referral = value;
    }

    public string LeadSource
    {
      get => this.leadSource;
      set => this.leadSource = value;
    }

    public string LeadTxnID
    {
      get => this.leadTxnId;
      set => this.leadTxnId = value;
    }

    public DateTime LastModified => this.lastModified;

    public Guid ContactGuid
    {
      get => this.guid;
      set => this.guid = value;
    }
  }
}
