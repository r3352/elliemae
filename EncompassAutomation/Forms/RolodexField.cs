// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Forms.RolodexField
// Assembly: EncompassAutomation, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: D6A34E1A-9881-4DDD-B85D-11A8A4C40EF4
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassAutomation.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassAutomation.xml

#nullable disable
namespace EllieMae.Encompass.Forms
{
  /// <summary>
  /// Enumeration of the fields which can be pulled from the Rolodex.
  /// </summary>
  public enum RolodexField
  {
    /// <summary>No field specified</summary>
    None = 0,
    /// <summary>Company name</summary>
    Company = 1,
    /// <summary>Contact name</summary>
    Name = 2,
    /// <summary>Contact address, line 1</summary>
    AddressLine1 = 3,
    /// <summary>Contact address, line 1</summary>
    AddressLine2 = 4,
    /// <summary>Contact address, lines 1 and 2 combined</summary>
    FullAddress = 5,
    /// <summary>Contact address, city</summary>
    City = 6,
    /// <summary>Contact address, state</summary>
    State = 7,
    /// <summary>Contact address, ZIP code</summary>
    ZipCode = 8,
    /// <summary>Contact phone number</summary>
    Phone = 9,
    /// <summary>Contact fax number</summary>
    Fax = 10, // 0x0000000A
    /// <summary>Cell Phone</summary>
    CellPhone = 11, // 0x0000000B
    /// <summary>Contact email address</summary>
    Email = 12, // 0x0000000C
    /// <summary>Contact/Company license number</summary>
    CompanyLicenseNo = 13, // 0x0000000D
    /// <summary>Business category</summary>
    Category = 14, // 0x0000000E
    /// <summary>Contact/business routing number</summary>
    ABA = 15, // 0x0000000F
    /// <summary>Contact/business routing account name</summary>
    AccountName = 16, // 0x00000010
    /// <summary>Buesiness web site URL</summary>
    WebSite = 17, // 0x00000011
    /// <summary>Mortgagee Clause Company Name</summary>
    MortgageeClauseCompany = 18, // 0x00000012
    /// <summary>Mortgagee Clause Contact Name</summary>
    MortgageeClauseName = 19, // 0x00000013
    /// <summary>Mortgagee Clause Address</summary>
    MortgageeClauseAddressLine = 20, // 0x00000014
    /// <summary>Mortgagee Clause City</summary>
    MortgageeClauseCity = 21, // 0x00000015
    /// <summary>Mortgagee Clause State</summary>
    MortgageeClauseState = 22, // 0x00000016
    /// <summary>Mortgagee Clause Zip Code</summary>
    MortgageeClauseZipCode = 23, // 0x00000017
    /// <summary>Mortgagee Clause Phone Number</summary>
    MortgageeClausePhone = 24, // 0x00000018
    /// <summary>Mortgagee Clause Fax Number</summary>
    MortgageeClauseFax = 25, // 0x00000019
    /// <summary>Mortgagee Clause Text</summary>
    MortgageeClauseText = 26, // 0x0000001A
    /// <summary>Contact Home Phone</summary>
    HomePhone = 28, // 0x0000001C
    /// <summary>Organization Identifier</summary>
    OrganizationID = 29, // 0x0000001D
    /// <summary>ContactLicenseNo</summary>
    ContactLicenseNo = 30, // 0x0000001E
    /// <summary>ContactLicenseIssuingAuthorityName</summary>
    ContactLicenseIssuingAuthorityName = 31, // 0x0000001F
    /// <summary>ContactLicenseAuthorityType</summary>
    ContactLicenseAuthorityType = 32, // 0x00000020
    /// <summary>ContactLicenseAuthorityStateCode</summary>
    ContactLicenseAuthorityStateCode = 33, // 0x00000021
    /// <summary>ContactLicenseIssueDate</summary>
    ContactLicenseIssueDate = 34, // 0x00000022
    /// <summary>CompanyLicenseIssuingAuthorityName</summary>
    CompanyLicenseIssuingAuthorityName = 36, // 0x00000024
    /// <summary>CompanyLicenseAuthorityType</summary>
    CompanyLicenseAuthorityType = 37, // 0x00000025
    /// <summary>CompanyLicenseAuthorityStateCode</summary>
    CompanyLicenseAuthorityStateCode = 38, // 0x00000026
    /// <summary>CompanyLicenseIssueDate</summary>
    CompanyLicenseIssueDate = 39, // 0x00000027
    /// <summary>Morgagee Location Code</summary>
    MortgageeClauseLocationCode = 40, // 0x00000028
    /// <summary>Mortgagee Investor Code</summary>
    MortgageeClauseInvestorCode = 41, // 0x00000029
  }
}
