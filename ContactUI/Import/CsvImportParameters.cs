// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ContactUI.Import.CsvImportParameters
// Assembly: ContactUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: A4DFDE69-475A-433E-BCA0-5CD47FD00B4A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ContactUI.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.ContactUI.CustomFields;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.ContactUI.Import
{
  public class CsvImportParameters
  {
    private CsvImportColumn[] importColumns;
    public ContactType ContactType;
    public bool UseQuotedStrings = true;
    public CsvImportColumn[] Columns;
    public bool AutoAssignColumns = true;
    private string importFile = "";
    private string[][] parsedData;
    private int columnCount = -1;
    private int firstRowIndex;
    private List<int> invalidRows;

    public CsvImportParameters(ContactType contactType, bool isTPOMVP = false)
    {
      this.ContactType = contactType;
      switch (contactType)
      {
        case ContactType.BizPartner:
          this.importColumns = CsvImportParameters.getAllBizPartnerColumns();
          break;
        case ContactType.TPO:
          this.importColumns = CsvImportParameters.getAllTPOContactColumns(isTPOMVP);
          break;
        case ContactType.TPOCompany:
          this.importColumns = CsvImportParameters.getAllTPOCompanyColumns();
          break;
        case ContactType.TPOExportLicenses:
          this.importColumns = CsvImportParameters.getAllExportLicensesColumns();
          break;
        default:
          this.importColumns = CsvImportParameters.getAllBorrowerColumns();
          break;
      }
    }

    public int ColumnCount => this.columnCount;

    public int RowCount => this.parsedData.Length - this.firstRowIndex;

    public string ImportFile => this.importFile;

    public bool ExcludeFirstRow
    {
      get => this.firstRowIndex > 0;
      set => this.firstRowIndex = value ? 1 : 0;
    }

    public string[] GetRow(int index) => this.parsedData[index + this.firstRowIndex];

    public string[] GetHeaderRow() => this.ExcludeFirstRow ? this.parsedData[0] : (string[]) null;

    public List<int> InvalidRows
    {
      get => this.invalidRows;
      set => this.invalidRows = value;
    }

    public void Parse(string importFile)
    {
      using (CsvParser parser = this.createParser(importFile))
        this.parsedData = parser.RemainingRows();
      this.Columns = (CsvImportColumn[]) null;
      this.calculateColumnCount();
      this.importFile = importFile;
    }

    public void ReParse() => this.Parse(this.importFile);

    public void Reset()
    {
      this.importFile = "";
      this.parsedData = (string[][]) null;
      this.UseQuotedStrings = true;
      this.firstRowIndex = 0;
      this.Columns = (CsvImportColumn[]) null;
      this.columnCount = -1;
      this.AutoAssignColumns = true;
    }

    private CsvParser createParser(string filePath)
    {
      return new CsvParser((TextReader) new StreamReader(filePath, Encoding.Default), this.UseQuotedStrings);
    }

    public CsvImportColumn[] GetAllAvailableColumns() => this.importColumns;

    private void calculateColumnCount()
    {
      if (this.parsedData == null)
      {
        this.columnCount = -1;
      }
      else
      {
        this.columnCount = 0;
        for (int index = 0; index < this.parsedData.Length; ++index)
        {
          if (this.parsedData[index].Length > this.columnCount)
            this.columnCount = this.parsedData[index].Length;
        }
      }
    }

    public static CsvImportColumn[] getAllBizPartnerColumns()
    {
      Hashtable categoryNameToIdTable = new BizCategoryUtil(Session.SessionObjects).GetCategoryNameToIdTable();
      string defaultValue = "No Category";
      ArrayList cols = new ArrayList();
      cols.Add((object) new CsvMappedImportColumn("CategoryID", "Category", (IDictionary) categoryNameToIdTable, defaultValue));
      cols.Add((object) new CsvImportColumn("FirstName", "First Name", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("LastName", "Last Name", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Salutation", "Salutation", FieldFormat.STRING, 16));
      cols.Add((object) new CsvImportColumn("CompanyName", "Company", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("JobTitle", "Job Title", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizAddress1", "Bus Addr/Street", FieldFormat.STRING, (int) byte.MaxValue));
      cols.Add((object) new CsvImportColumn("BizAddress2", "Bus Addr/Street 2", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizCity", "Bus Addr/City", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizState", "Bus Addr/State", FieldFormat.STATE, 2));
      cols.Add((object) new CsvImportColumn("BizZip", "Bus Addr/Zip", FieldFormat.ZIPCODE, 20));
      cols.Add((object) new CsvImportColumn("BizWebUrl", "Web Site", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("HomePhone", "Home Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("WorkPhone", "Work Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("MobilePhone", "Cell Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("FaxNumber", "Fax Number", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("PersonalEmail", "Home Email", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizEmail", "Work Email", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("NoSpam", "Do Not Email", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("Fees", "Fees", FieldFormat.INTEGER));
      cols.Add((object) new CsvImportColumn("PersonalInfoLicense.LicenseNumber", "Contact License/Number", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("PersonalInfoLicense.LicenseAuthName", "Contact License/Authority", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("PersonalInfoLicense.LicenseAuthType", "Contact License/Authority Type", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("PersonalInfoLicense.LicenseStateCode", "Contact License/State", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("PersonalInfoLicense.LicenseIssueDate", "Contact License/Issue Date", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("LicenseNumber", "Bus License/Number", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizContactLicense.LicenseAuthName", "Bus License/Authority", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizContactLicense.LicenseAuthType", "Bus License/Authority Type", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizContactLicense.LicenseStateCode", "Bus License/State", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizContactLicense.LicenseIssueDate", "Bus License/Issue Date", FieldFormat.STRING, 50));
      CsvImportParameters.addCustomFieldColumns(cols, ContactType.BizPartner);
      CsvImportParameters.addCategoryFieldColumns(cols, categoryNameToIdTable, CustomFieldsType.BizCategoryStandard);
      CsvImportParameters.addCategoryFieldColumns(cols, categoryNameToIdTable, CustomFieldsType.BizCategoryCustom);
      return (CsvImportColumn[]) cols.ToArray(typeof (CsvImportColumn));
    }

    public static CsvImportColumn[] getAllBorrowerColumns()
    {
      ArrayList cols = new ArrayList();
      cols.Add((object) new CsvImportColumn("FirstName", "First Name", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("LastName", "Last Name", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Salutation", "Salutation", FieldFormat.STRING, 16));
      cols.Add((object) new CsvListValuedImportColumn("ContactType", "Contact Type", CsvImportParameters.toStringArray(BorrowerTypeEnumUtil.GetDisplayNames()), ""));
      cols.Add((object) new CsvListValuedImportColumn("Status", "Status", CsvImportParameters.getBorrowerStatusList(), ""));
      cols.Add((object) new CsvImportColumn("EmployerName", "Employer", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("JobTitle", "Job Title", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("SSN", "SSN", FieldFormat.SSN, 12));
      cols.Add((object) new CsvImportColumn("Referral", "Referral", FieldFormat.STRING, 100));
      cols.Add((object) new CsvImportColumn("Income", "Annual Income", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("HomeAddress1", "Home Addr/Street", FieldFormat.STRING, (int) byte.MaxValue));
      cols.Add((object) new CsvImportColumn("HomeAddress2", "Home Addr/Street 2", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("HomeCity", "Home Addr/City", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("HomeState", "Home Addr/State", FieldFormat.STATE, 10));
      cols.Add((object) new CsvImportColumn("HomeZip", "Home Addr/Zip", FieldFormat.ZIPCODE, 20));
      cols.Add((object) new CsvImportColumn("BizAddress1", "Bus Addr/Street", FieldFormat.STRING, (int) byte.MaxValue));
      cols.Add((object) new CsvImportColumn("BizAddress2", "Bus Addr/Street 2", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizCity", "Bus Addr/City", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizState", "Bus Addr/State", FieldFormat.STATE, 20));
      cols.Add((object) new CsvImportColumn("BizZip", "Bus Addr/Zip", FieldFormat.ZIPCODE, 20));
      cols.Add((object) new CsvImportColumn("BizWebUrl", "Web Site", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("HomePhone", "Home Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("WorkPhone", "Work Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("MobilePhone", "Cell Phone", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("FaxNumber", "Fax Number", FieldFormat.PHONE, 30));
      cols.Add((object) new CsvImportColumn("PersonalEmail", "Home Email", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("BizEmail", "Work Email", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("NoSpam", "Do Not Email", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("NoCall", "Do Not Call", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("NoFax", "Do Not Fax", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("Birthdate", "Birthday", FieldFormat.DATE));
      cols.Add((object) new CsvImportColumn("Married", "Married", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("PrimaryContact", "Primary Contact", FieldFormat.X));
      cols.Add((object) new CsvImportColumn("Anniversary", "Anniversary", FieldFormat.DATE));
      cols.Add((object) new CsvImportColumn("SpouseName", "Spouse's Name", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Opportunity.LoanAmount", "Loan Amount", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.Purpose", "Loan Purpose", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Opportunity.Term", "Loan Term", FieldFormat.INTEGER, 3));
      cols.Add((object) new CsvListValuedImportColumn("Opportunity.Amortization", "Amortization", CsvImportParameters.toStringArray(AmortizationTypeEnumUtil.GetDisplayNames()), ""));
      cols.Add((object) new CsvImportColumn("Opportunity.DownPayment", "Available Down Payment", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.PropertyAddress", "Property Addr/Street", FieldFormat.STRING, (int) byte.MaxValue));
      cols.Add((object) new CsvImportColumn("Opportunity.PropertyCity", "Property Addr/City", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Opportunity.PropertyState", "Property Addr/State", FieldFormat.STRING, 2));
      cols.Add((object) new CsvImportColumn("Opportunity.PropertyZip", "Property Addr/Zip", FieldFormat.STRING, 20));
      cols.Add((object) new CsvListValuedImportColumn("Opportunity.PropertyUse", "Property Use", CsvImportParameters.toStringArray(PropertyUseEnumUtil.GetDisplayNames()), ""));
      cols.Add((object) new CsvListValuedImportColumn("Opportunity.PropertyType", "Property Type", CsvImportParameters.toStringArray(PropertyTypeEnumUtil.GetDisplayNames()), ""));
      cols.Add((object) new CsvImportColumn("Opportunity.PropertyValue", "Property Value", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.PurchaseDate", "Purchase Date", FieldFormat.DATE));
      cols.Add((object) new CsvImportColumn("Opportunity.MortgageBalance", "Current Mortgage Balance", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.MortgageRate", "Current Mortgage Rate", FieldFormat.DECIMAL_4));
      cols.Add((object) new CsvImportColumn("Opportunity.HousingPayment", "Monthly Payment (housing)", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.NonhousingPayment", "Monthly Payment (non-housing)", FieldFormat.DECIMAL_2));
      cols.Add((object) new CsvImportColumn("Opportunity.CreditRating", "Credit Rating", FieldFormat.STRING, 50));
      cols.Add((object) new CsvImportColumn("Opportunity.Bankruptcy", "Bankruptcy", FieldFormat.X));
      cols.Add((object) new CsvListValuedImportColumn("Opportunity.Employment", "Employment", CsvImportParameters.toStringArray(EmploymentStatusEnumUtil.GetDisplayNames()), ""));
      CsvImportParameters.addCustomFieldColumns(cols, ContactType.Borrower);
      return (CsvImportColumn[]) cols.ToArray(typeof (CsvImportColumn));
    }

    public static CsvImportColumn[] getAllTPOCompanyColumns()
    {
      return (CsvImportColumn[]) new ArrayList()
      {
        (object) new CsvImportColumn("OrganizationType", "Organization Type", FieldFormat.STRING, 20),
        (object) new CsvImportColumn("ParentOrganizationName", "Parent Organization Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("OrganizationName", "Organization Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("DisabledLogin", "Disable Login", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("OrgID", "Organization ID", FieldFormat.STRING, 26),
        (object) new CsvImportColumn("OwnerName", "Owner's Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("EntityType", "Channel Type", FieldFormat.STRING, 20),
        (object) new CsvImportColumn("CompanyDBAName", "Default DBA Name", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("CompanyLegalName", "Company Legal Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("Address", "Address", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("City", "City", FieldFormat.STRING, 50),
        (object) new CsvImportColumn("State", "State", FieldFormat.STRING, 20),
        (object) new CsvImportColumn("Zip", "Zip", FieldFormat.STRING, 11),
        (object) new CsvImportColumn("PhoneNumber", "Phone Number", FieldFormat.STRING, 25),
        (object) new CsvImportColumn("FaxNumber", "Fax Number", FieldFormat.STRING, 25),
        (object) new CsvImportColumn("Email", "Email", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("Website", "Website", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("ManagerName", "Manager Name", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("EmailForRateSheet", "Rate Sheet Email", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("FaxForRateSheet", "Rate Sheet Fax Number", FieldFormat.STRING, 25),
        (object) new CsvImportColumn("EmailForLockInfo", "Lock Info Email", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("FaxForLockInfo", "Lock Info Fax Number", FieldFormat.STRING, 25),
        (object) new CsvImportColumn("EPPSPriceGroup", "Price Group", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("EPPSUserName", "ICE PPE User Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("EPPSCompModel", "ICE PPE Comp. Model", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("CurrentStatus", "Current Status", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("AddToWatchlist", "Add to Watchlist", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("CurrentStatusDate", "Current Status Date", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("ApprovedDate", "Approved Date", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("ApplicationDate", "Application Date", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("CompanyRating", "Company Rating", FieldFormat.STRING, 20),
        (object) new CsvImportColumn("PrimarySalesRepUserId", "Primary Sales Rep. ID", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("PrimarySalesRepName", "Primary Sales Rep. Name", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("Incorporated", "Incorporated", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("StateIncorp", "State of Incorporated", FieldFormat.STRING, 20),
        (object) new CsvImportColumn("DateOfIncorporation", "Date of Incorporated", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("TypeOfEntity", "Type of Entity", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("OtherEntityDescription", "Other Entity Description", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("TaxID", "Tax ID", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("UseSSNFormat", "Use SSN Format", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("NmlsId", "NMLS ID", FieldFormat.STRING, 64),
        (object) new CsvImportColumn("FinancialsPeriod", "Financials Period", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("FinancialsLastUpdate", "Financials Last Update", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("CompanyNetWorth", "Company Net Worth", FieldFormat.STRING, 17),
        (object) new CsvImportColumn("EOExpirationDate", "E&O Expiration Date", FieldFormat.STRING, 10),
        (object) new CsvImportColumn("EOCompany", "E&O Company", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("EOPolicyNumber", "E&O Policy", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("MERSOriginatingORGID", "MERS Originating Org ID", FieldFormat.STRING, (int) byte.MaxValue),
        (object) new CsvImportColumn("DUSponsored", "DU Sponsored", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("CanFundInOwnName", "Can Fund in Own Name", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("CanCloseInOwnName", "Can Close in Own Name", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("UseParentInfoForCompanyDetails", "Use Parent Info (Co. Details)", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("UseParentInfoForRateLock", "Use Parent Info (Rate Sheet)", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("UseParentInfoForEPPS", "Use Parent Info (Pricing)", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("UseParentInfoForApprovalStatus", "Use Parent Info (Status)", FieldFormat.STRING, 3),
        (object) new CsvImportColumn("UseParentInfoForBusinessInfo", "Use Parent Info (Business Info)", FieldFormat.STRING, 3)
      }.ToArray(typeof (CsvImportColumn));
    }

    public static CsvImportColumn[] getAllTPOContactColumns(bool isTPOMVP = false)
    {
      ArrayList arrayList = new ArrayList();
      arrayList.Add((object) new CsvImportColumn("First_name", "First Name", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Middle_name", "Middle Name", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Last_name", "Last Name", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Suffix_name", "Suffix Name", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Title", "Title", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("UseCompanyAddress", "Use Company Address", FieldFormat.STRING, 3));
      arrayList.Add((object) new CsvImportColumn("Address1", "Address", FieldFormat.STRING, (int) byte.MaxValue));
      arrayList.Add((object) new CsvImportColumn("City", "City", FieldFormat.STRING, 50));
      arrayList.Add((object) new CsvImportColumn("State", "State", FieldFormat.STRING, 20));
      arrayList.Add((object) new CsvImportColumn("Zip", "Zip", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Email", "Email", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Phone", "Phone", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Fax", "Fax Number", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Cell_phone", "Cell Phone", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("SSN", "SSN", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("DisabledLogin", "Disable Login", FieldFormat.STRING, 3));
      arrayList.Add((object) new CsvImportColumn("Login_email", "Login Email", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("InheritParentRateSheet", "Use Parent Info (Rate Sheet and Lock)", FieldFormat.STRING, 3));
      arrayList.Add((object) new CsvImportColumn("Rate_sheet_email", "Rate Sheet Email", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Rate_sheet_fax", "Rate Sheet Fax", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Lock_info_email", "Lock Info Email", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Lock_info_fax", "Lock Info Fax", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("NMLSOriginatorID", "NMLS Originator ID", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("NMLSCurrent", "NMLS Current", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Approval_status", "Current Status", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Approval_status_watchlist", "Watch List", FieldFormat.STRING, 25));
      arrayList.Add((object) new CsvImportColumn("Approval_status_date", "Approval Status Date", FieldFormat.STRING, 10));
      arrayList.Add((object) new CsvImportColumn("Approval_date", "Approval Date", FieldFormat.STRING, 10));
      arrayList.Add((object) new CsvImportColumn("Sales_rep_name", "Sales Rep Name", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Sales_rep_userid", "Sales Rep User ID", FieldFormat.STRING, 16));
      if (isTPOMVP)
        arrayList.Add((object) new CsvImportColumn("UserPersonas", "Personas", FieldFormat.STRING, 64));
      else
        arrayList.Add((object) new CsvImportColumn("Roles", "Role", FieldFormat.STRING, 64));
      arrayList.Add((object) new CsvImportColumn("Note", "Note", FieldFormat.STRING, 4096));
      return (CsvImportColumn[]) arrayList.ToArray(typeof (CsvImportColumn));
    }

    public static CsvImportColumn[] getAllExportLicensesColumns()
    {
      return (CsvImportColumn[]) new ArrayList()
      {
        (object) new CsvImportColumn("LicenseExempt", "Exempt", FieldFormat.YN, 1),
        (object) new CsvImportColumn("State", "State", FieldFormat.STRING, 2),
        (object) new CsvImportColumn("LicenseType", "License Type", FieldFormat.STRING, 100),
        (object) new CsvImportColumn("LicenseNumber", "License #", FieldFormat.STRING, 50),
        (object) new CsvImportColumn("IssueDate", "Issue Date", FieldFormat.DATE),
        (object) new CsvImportColumn("StartDate", "Start Date", FieldFormat.DATE),
        (object) new CsvImportColumn("EndDate", "End Date", FieldFormat.DATE),
        (object) new CsvImportColumn("Status", "Status", FieldFormat.STRING, 50),
        (object) new CsvImportColumn("StatusDate", "Status Date", FieldFormat.DATE),
        (object) new CsvImportColumn("LastCheckedDate", "Last Checked", FieldFormat.DATE)
      }.ToArray(typeof (CsvImportColumn));
    }

    private static void addCustomFieldColumns(ArrayList cols, ContactType contactType)
    {
      foreach (ContactCustomFieldInfo fieldInfo in ((IContactManager) Session.ISession.GetObject("ContactManager")).GetCustomFieldInfo(contactType).Items)
        cols.Add((object) new CvsCustomFieldImportColumn(fieldInfo));
    }

    private static void addCategoryFieldColumns(
      ArrayList cols,
      Hashtable categoryMap,
      CustomFieldsType customFieldsType)
    {
      IDictionaryEnumerator enumerator = categoryMap.GetEnumerator();
      while (enumerator.MoveNext())
      {
        CustomFieldsDefinition fieldsDefinition = CustomFieldsDefinition.GetCustomFieldsDefinition(Session.SessionObjects, customFieldsType, int.Parse(enumerator.Value.ToString()));
        if (fieldsDefinition != null)
        {
          foreach (CustomFieldDefinition customFieldDefinition in (CollectionBase) fieldsDefinition.CustomFieldDefinitions)
            cols.Add((object) new CvsCategoryFieldImportColumn(customFieldsType, enumerator.Key.ToString(), customFieldDefinition));
        }
      }
    }

    private static string[] getBorrowerStatusList()
    {
      BorrowerStatus borrowerStatus = ((IContactManager) Session.ISession.GetObject("ContactManager")).GetBorrowerStatus();
      string[] borrowerStatusList = new string[borrowerStatus.Items.Length + 1];
      borrowerStatusList[0] = "";
      int num = 1;
      foreach (BorrowerStatusItem borrowerStatusItem in borrowerStatus.Items)
        borrowerStatusList[num++] = borrowerStatusItem.name;
      return borrowerStatusList;
    }

    private static string[] toStringArray(object[] data)
    {
      return (string[]) new ArrayList((ICollection) data).ToArray(typeof (string));
    }
  }
}
