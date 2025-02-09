// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IAclGroupManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IAclGroupManager
  {
    AclGroup GetGroupById(int groupId);

    AclGroup GetGroupByName(string groupName);

    bool GroupNameExists(string groupName);

    AclGroup[] GetAllGroups();

    AclGroup[] GetGroups(int[] groupIds);

    AclGroup CreateGroup(AclGroup group, bool isFromVersionMigration = false);

    AclGroup CreateGroup(string name, bool isFromVersionMigration = false);

    void DeleteGroup(int groupID);

    void DeleteGroup(AclGroup group);

    void RenameGroup(int groupID, string newName);

    void RenameGroup(AclGroup group, string newName);

    AclGroup[] GetGroupsOfUser(string userid);

    void UpdateGroup(AclGroup group, bool isFromVersionMigration = false);

    void UpdateMembersInGroup(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList);

    void AddUserToGroup(int groupID, string userid);

    void AddOrgToGroup(int groupID, int orgID, bool recursive);

    void DeleteUserFromGroup(int groupID, string userid);

    void DeleteOrgFromGroup(int groupID, int orgID);

    OrgInGroup[] GetOrgsInGroup(int groupID);

    string[] GetUsersInGroup(int groupID, bool includeOrgUsers);

    Dictionary<string, object> GetMembersInGroup(int groupID);

    AclGroup[] GetGroupsOfOrganization(int orgId);

    void UpdateMembersInGroupLoan(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList);

    void AddUserToGroupLoan(int groupID, UserInGroupLoan userInGroupLoan);

    void AddOrgToGroupLoan(int groupID, OrgInGroupLoan orgInGroupLoan);

    void DeleteUserFromGroupLoan(int groupID, string userid);

    void DeleteOrgFromGroupLoan(int groupID, int orgID);

    OrgInGroupLoan[] GetOrgsInGroupLoan(int groupID);

    UserInGroupLoan[] GetUsersInGroupLoan(int groupID);

    AclGroupLoanMembers GetMembersInGroupLoan(int groupID);

    void UpdateMembersInGroupLoan(int groupID, UserInGroupLoan[] users, OrgInGroupLoan[] orgs);

    void UpdateMembersInGroupContact(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList);

    void AddUserToGroupContact(int groupID, UserInGroupContact userInGroupLoan);

    void AddOrgToGroupContact(int groupID, OrgInGroupContact orgInGroupLoan);

    void DeleteUserFromGroupContact(int groupID, string userid);

    void DeleteOrgFromGroupContact(int groupID, int orgID);

    OrgInGroupContact[] GetOrgsInGroupContact(int groupID);

    UserInGroupContact[] GetUsersInGroupContact(int groupID);

    AclGroupContactMembers GetMembersInGroupContact(int groupID);

    void UpdateMembersInGroupContact(
      int groupID,
      UserInGroupContact[] users,
      OrgInGroupContact[] orgs);

    void UpdateAclGroupFileRef(int groupId, FileInGroup fileInGroup);

    void UpdateFileResource(string oldName, string newName, int fileType, bool isFolder);

    FileInGroup GetAclGroupFileRef(int groupId, int fileId);

    FileInGroup[] GetAclGroupFileRefs(int groupId, AclFileType fileType);

    Dictionary<AclFileType, FileInGroup[]> GetAclGroupFileRefs(
      int[] groupIds,
      AclFileType[] fileTypes);

    int[] GetAclGroupFileRefIDs(int groupId, AclFileType fileType);

    AclFileResource GetAclFileResource(int fileId);

    AclFileResource[] GetAclFileResources(int[] fileIds);

    void ResetAclGroupFileRefs(
      int groupId,
      FileInGroup[] filesInGroup,
      AclFileType fileType,
      int[] resetFileIDs);

    void ResetAclGroupFileRefs(int groupId, FileInGroup[] filesInGroup, AclFileType fileType);

    Dictionary<int, AclFileResource> AddAclFileResources(AclFileResource[] fileResources);

    string[] GetAclGroupChangeCircumstanceOptions(int groupId);

    string[] GetUsersChangeCircumstanceOptions();

    void ResetAclGroupChangeCircumstanceOptions(int groupId, string[] optionIDs);

    bool CheckPublicAccessPermission(AclFileType fileType);

    Hashtable CheckPublicAccessPermissions(AclFileType[] fileTypes);

    bool CheckPublicAccessPermissionToAny(AclFileType[] fileTypes);

    bool CheckPublicAccessPermission(AclFileType fileType, string userId);

    AclResourceAccess GetMaxPublicFolderAccess(AclFileType fileType);

    AclResourceAccess GetUserFileFolderAccess(AclFileType fileType, FileSystemEntry fsEntry);

    Hashtable GetUserFileFolderAccess(AclFileType fileType, FileSystemEntry[] fsEntries);

    AclResourceAccess GetUserFileFolderAccess(
      AclFileType fileType,
      FileSystemEntry fsEntry,
      string userId);

    FileSystemEntry[] GetUserFileFoldersAccess(AclFileType fileType, FileSystemEntry[] fsEntry);

    LoanFolderInGroup GetAclGroupLoanFolder(int groupId, string folderName);

    AclTriState GetBizContactAccessRight(UserInfo userObj, int contactID);

    AclTriState GetBizContactGroupAccessRight(UserInfo userObj, int contactGroupID);

    AclResourceAccess GetBorrowerContactAccessRight(UserInfo userObj, string contactOwnerID);

    void UpdateAclGroupLoanFolder(LoanFolderInGroup folderInGroup);

    LoanFolderInGroup[] GetAclGroupLoanFolders(int groupId);

    OrgInGroupRole[] GetOrgsInGroupRole(int groupID, int roleID);

    UserInGroupRole[] GetUsersInGroupRole(int groupID, int roleID);

    AclGroupRoleMembers GetMembersInGroupRole(int groupID, int roleID);

    void AddUserToGroupRole(int groupID, UserInGroupRole userInGroupRole);

    void AddOrgToGroupRole(int groupID, OrgInGroupRole orgInGroupRole);

    void DeleteUserFromGroupRole(int groupID, int roleID, string userid);

    void DeleteOrgFromGroupRole(int groupID, int roleID, int orgID);

    void UpdateMembersInGroupRole(
      int groupID,
      int roleID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList);

    void UpdateMembersInGroupRole(
      int groupID,
      int roleID,
      UserInGroupRole[] users,
      OrgInGroupRole[] orgs);

    AclGroupRoleAccessLevel GetAclGroupRoleAccessLevel(int groupID, int roleID);

    void UpdateAclGroupRoleAccessLevel(AclGroupRoleAccessLevel accessLevel);

    string[] GetUsersStdPrintForms();

    string[] GetAclGroupStdPrintForms(int groupId);

    void ResetAclGroupStdPrintForms(int groupId, string[] fileIds);

    BizGroupRef[] GetBizContactGroupRefs(int aclGroupId);

    BizGroupRef[] GetBizContactGroupRefs(string userID, bool editOnly);

    void ResetBizContactGroupRefs(int aclGroupId, BizGroupRef[] bizGroupRefs);

    void UpdateBizContactGroupRef(int aclGroupId, BizGroupRef bizGroupRef);

    void CloneAclGroup(int sourceGroupID, int desGroupID);

    string[] GetConditionalApprovalLetter(UserInfo userObj);

    FileSystemEntry[] ApplyUserAccessRights(
      UserInfo userInfo,
      FileSystemEntry[] fsEntries,
      AclFileType fileType);

    Dictionary<AclFileType, FileSystemEntry> GetRootFileSystemEntry(AclFileType[] fileTypes);

    void ResetAclGroupFileRefs(int groupId, Dictionary<AclFileType, FileInGroup[]> updateList);

    bool CreateUserGroup(
      UserGroupDetails userGroupDetails,
      string loggedInUserId,
      bool returnEntityData,
      out int newGroupId,
      out UserGroupDetails responseData);

    (OrgInGroup[], UserGroupMemberUser[]) UpsertMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId,
      bool returnDetails,
      string groupName,
      bool isUpdate);

    void RemoveMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId,
      string groupName);
  }
}
