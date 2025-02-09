// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IBizContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessEnums;
using EllieMae.Encompass.BusinessObjects.Users;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("DCC3DBB3-4BD8-47bf-A6BB-66F0E3276DB5")]
  public interface IBizContact
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

    BizCategory Category { get; set; }

    string CompanyName { get; set; }

    string LicenseNumber { get; set; }

    int Fees { get; set; }

    string Comment { get; set; }

    ContactCustomFields CustomFields { get; }

    User Owner { get; set; }

    ContactAccessLevel AccessLevel { get; set; }

    ContactType Type { get; }

    DateTime LastModified { get; }

    string Salutation { get; set; }

    BizCategoryCustomFields BizCategoryCustomFields { get; }

    string FullName { get; }
  }
}
