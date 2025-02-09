// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IUser
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  [Guid("EDD308B5-2A15-4a6e-A484-5E2450848393")]
  public interface IUser
  {
    string ID { get; }

    string Password { get; set; }

    string FirstName { get; set; }

    string LastName { get; set; }

    string EmployeeID { get; set; }

    UserPersonas Personas { get; }

    string Email { get; set; }

    string Phone { get; set; }

    string WorkingFolder { get; set; }

    int OrganizationID { get; }

    void ChangeOrganization(Organization newOrganization);

    StateLicenses StateLicenses { get; }

    SubordinateLoanAccessRight SubordinateLoanAccessRight { get; set; }

    bool Enabled { get; }

    void Enable();

    void Disable();

    void Commit();

    void Delete();

    void Refresh();

    bool IsNew { get; }

    string ToString();

    bool HasAccessTo(Feature featr);

    void GrantAccessTo(Feature featr);

    void RevokeAccessTo(Feature featr);

    PeerLoanAccessRight PeerLoanAccessRight { get; set; }

    bool RequirePasswordChange { get; set; }

    bool AccountLocked { get; set; }

    string FullName { get; }

    DataObject GetCustomDataObject(string filePath);

    void SaveCustomDataObject(string filePath, DataObject dataObj);

    string CellPhone { get; set; }

    string Fax { get; set; }

    string CHUMID { get; set; }

    string NMLSOriginatorID { get; set; }

    DateTime NMLSExpirationDate { get; set; }

    void AppendToCustomDataObject(string filePath, DataObject dataObj);

    void DeleteCustomDataObject(string filePath);

    bool SSOOnly { get; set; }

    bool SSODisconnectedFromOrg { get; set; }
  }
}
