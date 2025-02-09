// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContact
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  /// <summary>Interface for Contact class.</summary>
  /// <exclude />
  [Guid("B8D4CF53-CC92-45f4-8102-80905BD31A79")]
  public interface IContact
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

    ContactCustomFields CustomFields { get; }

    User Owner { get; set; }

    ContactAccessLevel AccessLevel { get; set; }

    ContactType Type { get; }

    DateTime LastModified { get; }

    string Salutation { get; set; }

    string FullName { get; }

    LoanContactRelationshipList GetLoanRelationships();
  }
}
