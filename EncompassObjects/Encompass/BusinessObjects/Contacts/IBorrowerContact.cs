// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IBorrowerContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for BorrowerContact class.</summary>
  /// <exclude />
  [Guid("1ECAD13F-5669-43c6-A464-D8FE21BEF994")]
  public interface IBorrowerContact
  {
    int ID { get; }

    string LastName { get; set; }

    string FirstName { get; set; }

    string JobTitle { get; set; }

    string WorkPhone { get; set; }

    string HomePhone { get; set; }

    string MobilePhone { get; set; }

    string FaxNumber { get; set; }

    string CustField1 { get; set; }

    string CustField2 { get; set; }

    string CustField3 { get; set; }

    string CustField4 { get; set; }

    string PersonalEmail { get; set; }

    string BizEmail { get; set; }

    string BizWebUrl { get; set; }

    Address BizAddress { get; }

    bool NoSpam { get; set; }

    void Commit();

    void Delete();

    void Refresh();

    bool IsNew { get; }

    ContactNotes Notes { get; }

    ContactEvents Events { get; }

    string ToString();

    Address HomeAddress { get; }

    string EmployerName { get; set; }

    object Birthdate { get; set; }

    void SetBirthdate(object newDate);

    bool Married { get; set; }

    int SpouseContactID { get; set; }

    string SpouseName { get; set; }

    object Anniversary { get; set; }

    void SetAnniversary(object newDate);

    User Owner { get; set; }

    BorrowerContactType BorrowerType { get; set; }

    string Status { get; set; }

    bool NoCall { get; set; }

    bool NoFax { get; set; }

    string SocialSecurityNumber { get; set; }

    string Referral { get; set; }

    Decimal Income { [return: MarshalAs(UnmanagedType.Currency)] get; [param: MarshalAs(UnmanagedType.Currency)] set; }

    ContactAccessLevel AccessLevel { get; set; }

    ContactOpportunities Opportunities { get; }

    ContactCustomFields CustomFields { get; }

    ContactType Type { get; }

    DateTime LastModified { get; }

    string Salutation { get; set; }

    bool PrimaryContact { get; set; }

    string FullName { get; }

    string MiddleName { get; set; }

    string Suffix { get; set; }
  }
}
