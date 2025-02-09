// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.IExternalUser
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Interface for IExternalUser class to support External User Management
  /// </summary>
  /// <exclude />
  [Guid("681791A9-457E-4221-965F-F926AD1711F4")]
  public interface IExternalUser
  {
    string ID { get; }

    string FirstName { get; set; }

    string MiddleName { get; set; }

    string LastName { get; set; }

    string Suffix { get; set; }

    string Title { get; set; }

    string Address { get; set; }

    string City { get; set; }

    string State { get; set; }

    string Zipcode { get; set; }

    string Phone { get; set; }

    string CellPhone { get; set; }

    string Fax { get; set; }

    string SSN { get; set; }

    string Email { get; set; }

    string EmailForRateSheet { get; set; }

    string FaxForRateSheet { get; set; }

    string EmailForLockInfo { get; set; }

    string FaxForLockInfo { get; set; }

    string EmailForLogin { get; set; }

    bool OnWatchlist { get; set; }

    User SalesRep { get; set; }

    string NmlsId { get; set; }

    bool IsLoanOfficer { get; }

    bool IsLoanProcessor { get; }

    bool IsManager { get; }

    bool IsAdministrator { get; }

    string ContactID { get; }

    DateTime ApprovalDate { get; set; }

    DateTime ApprovalCurrentStatusDate { get; set; }

    DateTime LastLogin { get; }

    ExternalUserList AccessibleUserList { get; }

    int LoginAttempts { get; }

    int ExternalOrganizationId { get; }

    bool Enabled { get; }

    DateTime PasswordChangedDate { get; }

    bool RequirePasswordChange { get; }

    bool UseCompanyAddress { get; set; }

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch { get; }

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company { get; }

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization ParentOrganization { get; }

    string UpdatedBy { get; }

    string UpdatedByFirstName { get; }

    string UpdatedByLastName { get; }

    string UpdatedByEmail { get; }

    DateTime UpdatedDateTime { get; }

    ExternalUrlList AssignedUrls { get; }

    /// <summary>Set teh updatedBy information to an external userID</summary>
    ExternalUser UpdatingExternalUser { set; }

    PersonaList GetPersonas();

    void Enable();

    void Disable();

    void Commit();

    void Delete();

    string ToString();

    string ResetPassword();

    bool SetPassword(string newPassword);

    ExternalLicensing Licensing { get; set; }

    ExternalUserList GetAccessibleUsersBySite(int urlID);

    ExternalUserList GetAccessibleUsersBySite(string siteID);

    void AddRole(ExternalUserRoles role);

    void RemoveRole(ExternalUserRoles role);

    void AddPersona(Persona persona);

    void RemovePersona(Persona persona);

    PersonaList GetAllExternalPersonas();

    void AddUrl(ExternalUrl url);

    void RemoveUrl(ExternalUrl url);

    void UpdateUrls(ExternalUrlList urlList);

    List<CurrentContactStatus> GetContactStatus();
  }
}
