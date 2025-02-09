// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IOrganizationManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IOrganizationManager
  {
    void CreateNewUser(UserInfo info);

    void DeleteUser(
      string userId,
      UserAssignedContactsBehaviorEnums? assignedContactsBehavior = null,
      string reassignContactsToUser = null);

    void EnableUser(string userId);

    void DisableUser(string userId);

    UserInfo GetUser(string userId);

    Hashtable GetUsers(string[] userIds);

    Hashtable GetUsers(string[] userIds, bool summariesOnly);

    void UpdateUser(UserInfo info);

    bool UserExists(string userId);

    UserInfo[] GetAllUsers();

    UserInfo[] GetAllIntAndExtUsers();

    List<string> GetUsersWithRoles(int[] roleIds);

    UserInfoSummary[] GetAllUserInfoSummary();

    UserInfo[] GetUsersWithRole(int roleId);

    UserInfoSummary[] GetUserInfoSummaryWithRole(int roleId);

    UserInfo[] GetUsersInOrgWithRole(int orgID, int roleId);

    UserInfoSummary[] GetUserInfoSummaryInOrgWithRole(int orgID, int roleId);

    Dictionary<string, UserLoginInfo> GetUserLoginInfos(string[] userIds, bool isTPOMVP = false);

    UserInfo[] GetUsersWithPersona(int personaID, bool exactMatch);

    UserInfoSummary[] GetUserInfoSummariesWithPersona(int personaID, bool exactMatch);

    UserInfo[] GetAccessibleUsersWithPersona(int personaID, bool exactMatch);

    UserInfoSummary[] GetAccessibleUserInfoSummariesWithPersona(int personaID, bool exactMatch);

    UserInfo[] GetUsersByName(string firstName, string lastName);

    UserInfo[] GetUsersByName(
      string firstName,
      string middleName,
      string lastName,
      string suffixName);

    UserInfo[] GetUsersInOrganization(int orgId);

    UserInfo[] GetUsersUnderOrganization(int orgId);

    UserInfo[] GetOrganizationSSOUsers(int orgId);

    void UpdateOrgSSOUsers(int oid, bool loginAccess, bool applyToAllUsers);

    UserInfo[] GetAllAccessibleUsers();

    UserInfo[] GetAllAccessibleSalesRepUsers();

    int GetEnabledUserCount();

    LOLicenseInfo[] GetLOLicenses(string userId);

    LOLicenseInfo GetLOLicense(string userId, string state);

    string[] GetLOLicensedStates(string userId);

    void AddLOLicense(LOLicenseInfo license);

    void DeleteLOLicense(string userId, string state);

    void DeleteAllLOLicenses(string userId);

    void SetLOLicenses(string userId, LOLicenseInfo[] licenses);

    OrgInfo GetRootOrganization();

    OrgInfo GetOrganization(int orgId);

    OrgInfo[] GetOrganizations(int[] orgIds);

    OrgInfo[] GetOrganizationsByName(string orgName);

    OrgInfo[] GetAllOrganizations();

    OrgInfo[] GetAllIntAndExtOrganizations();

    int[] GetDescendentsOfOrg(int orgId);

    bool IsDescendentOfOrg(int parentOrgId, int childOrgId);

    OrgInfo GetFirstAvaliableOrganization(int orgId);

    OrgInfo GetFirstAvaliableOrganization(int orgId, bool getInstalledInfoIfNotFound);

    OrgInfo GetFirstOrganizationWithNMLS(int orgId);

    OrgInfo GetFirstOrganizationWithLOSearch(int orgId);

    OrgInfo GetFirstOrganizationWithCCSiteId(int orgId);

    OrgInfo GetFirstOrganizationWithLEI(int orgId);

    OrgInfo GetFirstOrganizationWithMERSMIN(int orgId);

    OrgInfo GetFirstOrganizationWithStateLicensing(int orgId);

    OrgInfo GetFirstOrganizationWithStateLicensing(
      string companyName,
      string streetAddress,
      string loID);

    OrgInfo GetFirstOrganizationWithLOComp(int orgId);

    OrgInfo GetFirstOrganizationForSSO(int orgId);

    bool GetParentOrganizationSSOSetting(int orgId);

    OrgInfo GetFirstOrganizationWithONRP(int orgId);

    OrgInfo GetOrganizationForClosingVendorInformation(int orgId);

    string GetOrgPath(int orgId);

    int CreateOrganization(OrgInfo info);

    void UpdateOrganization(OrgInfo info);

    void MoveOrganization(int orgId, int newParentId);

    void DeleteOrganization(int orgId);

    void CreateSettingsRptJob(
      SettingsRptJobInfo.jobType jobtype,
      string reportName,
      DateTime requestDate,
      Dictionary<string, string> reportParameters,
      List<string> reportFilters);

    SettingsRptJobInfo[] GetSettingsRptJobs(string userId);

    string GetSettingsRptXML(string userId, string reportID);

    void MoveUserIntoOrganization(string userId, int orgId);

    void MoveUsersIntoOrganization(
      string[] userIds,
      int orgId,
      List<string> coonectedUsers = null,
      bool enableSSO = false);

    UserInfo[] GetScopedUsersWithRole(int roleId);

    UserInfoSummary[] GetScopedUsersWithRoles(int[] roleIds);

    UserInfo[] GetScopedUsers();

    UserInfoSummary[] GetScopedUserInfos();

    PwdRuleValidator GetPasswordValidator();

    byte[] GetUserPasswordHash(string userId);

    byte[] GetUserPasswordHash(string userId, bool isOrganizationUserSetting, bool isFromOldTable);

    BinaryObject GetUserCustomDataObject(string userId, string name);

    void SaveUserCustomDataObject(string userId, string name, BinaryObject data);

    void AppendToUserCustomDataObject(string userId, string name, BinaryObject data);

    void UpdatePersonalStatusOnlineUsers(string[] enableUserList, string[] disableUserList);

    UserProfileInfo GetUserProfile(string userId);

    void UpdateUserProfile(UserProfileInfo info, bool isEdit);

    void UpsertUserProfile(UserProfileInfo info);

    CCSiteInfo getCCSiteInfo(int orgId);

    void CreateUserCCSiteInfo(CCSiteInfo siteInfo, string userId);

    void UpdateUserCCSiteInfo(CCSiteInfo siteInfo, string userId);

    CCSiteInfo GetUserCCSiteInfo(string userId);

    void CreateCCSiteInfo(CCSiteInfo siteInfo, int orgId);

    bool OAuthClientIdExists(string oAuthClientId);

    UserInfo GetUser(string userId, bool fromDb);

    bool OrganizationExists(int orgId);

    bool IsMfaEnabledForExternalOrg(int extOrgId);

    bool CheckIfLoanExistsForInternalUser(string userId);

    bool CheckIfContactsExistsForInternalUser(string userId);

    List<OrgHierarchyInfo> GetOrgHierarchy(int orgId);
  }
}
