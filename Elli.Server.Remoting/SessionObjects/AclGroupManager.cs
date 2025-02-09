// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.AclGroupManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.MessageServices.Event;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class AclGroupManager : SessionBoundObject, IAclGroupManager
  {
    private const string className = "AclGroupManager";

    public AclGroupManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (AclGroupManager).ToLower());
      return this;
    }

    public virtual void UpdateGroup(AclGroup group, bool isFromVersionMigration = false)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateGroup), new object[1]
      {
        (object) group
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to update a usergroup"));
      }
      else
      {
        try
        {
          AclGroupAccessor.UpdateGroup(group, this.Session.UserID, isFromVersionMigration);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual AclGroup GetGroupByName(string groupName)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetGroupByName), new object[1]
      {
        (object) groupName
      });
      try
      {
        return AclGroupAccessor.GetGroupByName(groupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup) null;
      }
    }

    public virtual AclGroup GetGroupById(int groupId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetGroupById), new object[1]
      {
        (object) groupId
      });
      try
      {
        return AclGroupAccessor.GetGroupById(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup) null;
      }
    }

    public virtual bool GroupNameExists(string groupName)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GroupNameExists), new object[1]
      {
        (object) groupName
      });
      try
      {
        return AclGroupAccessor.CheckIfGroupNameExist(groupName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual AclGroup[] GetAllGroups()
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAllGroups), (object[]) null);
      try
      {
        return AclGroupAccessor.GetAllGroups();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup[]) null;
      }
    }

    public virtual AclGroup CreateGroup(AclGroup group, bool isFromVersionMigration = false)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CreateGroup), new object[1]
      {
        (object) group.Name
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to create a usergroup"));
        return (AclGroup) null;
      }
      try
      {
        AclGroup group1 = AclGroupAccessor.CreateGroup(group, this.Session.UserID, isFromVersionMigration);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupCreated, DateTime.Now, group1.ID, group1.Name));
        return group1;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup) null;
      }
    }

    public virtual AclGroup CreateGroup(string name, bool isFromVersionMigration = false)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CreateGroup), new object[1]
      {
        (object) name
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to create a usergroup"));
        return (AclGroup) null;
      }
      try
      {
        AclGroup group = AclGroupAccessor.CreateGroup(name, this.Session.UserID, isFromVersionMigration);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupCreated, DateTime.Now, group.ID, group.Name));
        return group;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup) null;
      }
    }

    public virtual void DeleteGroup(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteGroup), new object[1]
      {
        (object) groupID
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to delete a usergroup"));
      }
      else
      {
        try
        {
          string name = AclGroupAccessor.GetGroupById(groupID).Name;
          AclGroupAccessor.DeleteGroup(groupID, this.Session.UserID);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupDeleted, DateTime.Now, groupID, name));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void DeleteGroup(AclGroup group) => this.DeleteGroup(group.ID);

    public virtual void RenameGroup(int groupID, string newName)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (RenameGroup), new object[1]
      {
        (object) newName
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to rename a usergroup"));
      }
      else
      {
        try
        {
          string name = AclGroupAccessor.GetGroupById(groupID).Name;
          AclGroupAccessor.RenameGroup(groupID, newName, this.Session.UserID);
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupModified, DateTime.Now, groupID, name));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void RenameGroup(AclGroup group, string newName)
    {
      this.RenameGroup(group.ID, newName);
    }

    public virtual void UpdateMembersInGroup(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroup), new object[6]
      {
        (object) groupID,
        (object) resetUserList,
        (object) resetOrgList,
        (object) newUserList,
        (object) newOrgList,
        (object) newInclusiveOrgList
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to update members in a usergroup"));
      }
      else
      {
        try
        {
          (List<MemberContract> collection1, List<MemberContract> collection2) = AclGroupAccessor.UpdateUsersInGroup(groupID, resetUserList, newUserList, this.Session.UserID);
          (List<MemberContract> source1, List<MemberContract> source2, List<MemberContract> source3) = AclGroupAccessor.UpdateOrgsInGroup(groupID, resetOrgList, newOrgList, newInclusiveOrgList, this.Session.UserID);
          source1.AddRange((IEnumerable<MemberContract>) collection1);
          source3.AddRange((IEnumerable<MemberContract>) collection2);
          if (!source1.Any<MemberContract>() && !source2.Any<MemberContract>() && !source3.Any<MemberContract>())
            return;
          MembersContract members = new MembersContract()
          {
            Added = source1.Any<MemberContract>() ? (IEnumerable<MemberContract>) source1 : (IEnumerable<MemberContract>) null,
            Updated = source2.Any<MemberContract>() ? (IEnumerable<MemberContract>) source2 : (IEnumerable<MemberContract>) null,
            Deleted = source3.Any<MemberContract>() ? (IEnumerable<MemberContract>) source3 : (IEnumerable<MemberContract>) null
          };
          AclGroupAccessor.PublishUserGroupKafkaEvent("update", this.Session.UserID, groupID, members);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void AddUserToGroup(int groupID, string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddUserToGroup), new object[2]
      {
        (object) groupID,
        (object) userid
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to add user to a group"));
      }
      else
      {
        try
        {
          AclGroupAccessor.AddUserToGroup(groupID, userid);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void AddOrgToGroup(int groupID, int orgID, bool recursive)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddOrgToGroup), new object[3]
      {
        (object) groupID,
        (object) orgID,
        (object) recursive
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to add org to a group"));
      }
      else
      {
        try
        {
          AclGroupAccessor.AddOrgToGroup(groupID, orgID, recursive);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void DeleteUserFromGroup(int groupID, string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteUserFromGroup), new object[2]
      {
        (object) groupID,
        (object) userid
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to delete a user from a usergroup"));
      }
      else
      {
        try
        {
          AclGroupAccessor.DeleteUserFromGroup(groupID, userid);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void DeleteOrgFromGroup(int groupID, int orgID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteOrgFromGroup), new object[2]
      {
        (object) groupID,
        (object) orgID
      });
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_UserGroups))
      {
        Err.Raise(TraceLevel.Warning, nameof (AclGroupManager), (ServerException) new SecurityException("User does not have the access right to delete a org from a usergroup"));
      }
      else
      {
        try
        {
          AclGroupAccessor.DeleteOrgFromGroup(groupID, orgID);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual AclGroup[] GetGroupsOfUser(string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetGroupsOfUser), new object[1]
      {
        (object) userid
      });
      try
      {
        return AclGroupAccessor.GetGroupsOfUser(userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup[]) null;
      }
    }

    public virtual AclGroup[] GetGroups(int[] groupIds)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetGroups), new object[1]
      {
        (object) groupIds
      });
      try
      {
        return AclGroupAccessor.GetGroups(groupIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup[]) null;
      }
    }

    public virtual AclGroup[] GetGroupsOfOrganization(int orgId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetGroupsOfOrganization), new object[1]
      {
        (object) orgId
      });
      try
      {
        return AclGroupAccessor.GetGroupsOfOrganization(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroup[]) null;
      }
    }

    public virtual OrgInGroup[] GetOrgsInGroup(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetOrgsInGroup), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupAccessor.GetOrgsInGroup(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (OrgInGroup[]) null;
      }
    }

    public virtual string[] GetUsersInGroup(int groupID, bool includeOrgUsers)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersInGroup), new object[2]
      {
        (object) groupID,
        (object) includeOrgUsers
      });
      try
      {
        return AclGroupAccessor.GetUsersInGroup(groupID, includeOrgUsers);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual Dictionary<string, object> GetMembersInGroup(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetMembersInGroup), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupAccessor.GetMembersInGroup(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return new Dictionary<string, object>();
      }
    }

    public virtual void UpdateMembersInGroupLoan(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupLoan), new object[6]
      {
        (object) groupID,
        (object) resetUserList,
        (object) resetOrgList,
        (object) newUserList,
        (object) newOrgList,
        (object) newInclusiveOrgList
      });
      try
      {
        AclGroupLoanAccessor.UpdateOrgsInGroupLoan(groupID, resetOrgList, newOrgList, newInclusiveOrgList, this.Session.UserID);
        AclGroupLoanAccessor.UpdateUsersInGroupLoan(groupID, resetUserList, newUserList, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddUserToGroupLoan(int groupID, UserInGroupLoan userInGroupLoan)
    {
      this.onApiCalled(nameof (AclGroupManager), "AddUserToGroup", new object[2]
      {
        (object) groupID,
        (object) userInGroupLoan
      });
      try
      {
        AclGroupLoanAccessor.AddUserToGroupLoan(groupID, userInGroupLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddOrgToGroupLoan(int groupID, OrgInGroupLoan orgInGroupLoan)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddOrgToGroupLoan), new object[2]
      {
        (object) groupID,
        (object) orgInGroupLoan
      });
      try
      {
        AclGroupLoanAccessor.AddOrgToGroupLoan(groupID, orgInGroupLoan);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserFromGroupLoan(int groupID, string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteUserFromGroupLoan), new object[2]
      {
        (object) groupID,
        (object) userid
      });
      try
      {
        AclGroupLoanAccessor.DeleteUserFromGroupLoan(groupID, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOrgFromGroupLoan(int groupID, int orgID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteOrgFromGroupLoan), new object[2]
      {
        (object) groupID,
        (object) orgID
      });
      try
      {
        AclGroupLoanAccessor.DeleteOrgFromGroupLoan(groupID, orgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual OrgInGroupLoan[] GetOrgsInGroupLoan(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetOrgsInGroupLoan), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupLoanAccessor.GetOrgsInGroupLoan(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (OrgInGroupLoan[]) null;
      }
    }

    public virtual UserInGroupLoan[] GetUsersInGroupLoan(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersInGroupLoan), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupLoanAccessor.GetUsersInGroupLoan(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (UserInGroupLoan[]) null;
      }
    }

    public virtual AclGroupLoanMembers GetMembersInGroupLoan(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetMembersInGroupLoan), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupLoanAccessor.GetMembersInGroupLoan(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroupLoanMembers) null;
      }
    }

    public virtual void UpdateMembersInGroupLoan(
      int groupID,
      UserInGroupLoan[] users,
      OrgInGroupLoan[] orgs)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupLoan), new object[3]
      {
        (object) groupID,
        (object) users,
        (object) orgs
      });
      try
      {
        foreach (OrgInGroupLoan org in orgs)
          AclGroupLoanAccessor.UpdateOrgInGroupLoan(groupID, org);
        foreach (UserInGroupLoan user in users)
          AclGroupLoanAccessor.UpdateUserInGroupLoan(groupID, user);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateMembersInGroupContact(
      int groupID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupContact), new object[6]
      {
        (object) groupID,
        (object) resetUserList,
        (object) resetOrgList,
        (object) newUserList,
        (object) newOrgList,
        (object) newInclusiveOrgList
      });
      try
      {
        AclGroupContactAccessor.UpdateUsersInGroupContact(groupID, resetUserList, newUserList, this.Session.UserID);
        AclGroupContactAccessor.UpdateOrgsInGroupContact(groupID, resetOrgList, newOrgList, newInclusiveOrgList, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddUserToGroupContact(int groupID, UserInGroupContact userInGroupContact)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddUserToGroupContact), new object[2]
      {
        (object) groupID,
        (object) userInGroupContact
      });
      try
      {
        AclGroupContactAccessor.AddUserToGroupContact(groupID, userInGroupContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddOrgToGroupContact(int groupID, OrgInGroupContact orgInGroupContact)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddOrgToGroupContact), new object[2]
      {
        (object) groupID,
        (object) orgInGroupContact
      });
      try
      {
        AclGroupContactAccessor.AddOrgToGroupContact(groupID, orgInGroupContact);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserFromGroupContact(int groupID, string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteUserFromGroupContact), new object[2]
      {
        (object) groupID,
        (object) userid
      });
      try
      {
        AclGroupContactAccessor.DeleteUserFromGroupContact(groupID, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOrgFromGroupContact(int groupID, int orgID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteOrgFromGroupContact), new object[2]
      {
        (object) groupID,
        (object) orgID
      });
      try
      {
        AclGroupContactAccessor.DeleteOrgFromGroupContact(groupID, orgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual OrgInGroupContact[] GetOrgsInGroupContact(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetOrgsInGroupContact), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupContactAccessor.GetOrgsInGroupContact(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (OrgInGroupContact[]) null;
      }
    }

    public virtual UserInGroupContact[] GetUsersInGroupContact(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersInGroupContact), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupContactAccessor.GetUsersInGroupContact(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (UserInGroupContact[]) null;
      }
    }

    public virtual AclGroupContactMembers GetMembersInGroupContact(int groupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetMembersInGroupContact), new object[1]
      {
        (object) groupID
      });
      try
      {
        return AclGroupContactAccessor.GetMembersInGroupContact(groupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroupContactMembers) null;
      }
    }

    public virtual void UpdateMembersInGroupContact(
      int groupID,
      UserInGroupContact[] users,
      OrgInGroupContact[] orgs)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupContact), new object[3]
      {
        (object) groupID,
        (object) users,
        (object) orgs
      });
      try
      {
        foreach (OrgInGroupContact org in orgs)
          AclGroupContactAccessor.UpdateOrgInGroupContact(groupID, org);
        foreach (UserInGroupContact user in users)
          AclGroupContactAccessor.UpdateUserInGroupContact(groupID, user);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanFolderInGroup GetAclGroupLoanFolder(int groupId, string folderName)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupLoanFolder), new object[2]
      {
        (object) groupId,
        (object) folderName
      });
      try
      {
        return AclGroupLoanAccessor.GetAclGroupLoanFolder(groupId, folderName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (LoanFolderInGroup) null;
      }
    }

    public virtual void UpdateAclGroupLoanFolder(LoanFolderInGroup folderInGroup)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateAclGroupLoanFolder), new object[1]
      {
        (object) folderInGroup
      });
      try
      {
        AclGroupLoanAccessor.UpdateAclGroupLoanFolder(folderInGroup, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual LoanFolderInGroup[] GetAclGroupLoanFolders(int groupId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupLoanFolders), new object[1]
      {
        (object) groupId
      });
      try
      {
        return AclGroupLoanAccessor.GetAclGroupLoanFolders(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (LoanFolderInGroup[]) null;
      }
    }

    public virtual void UpdateAclGroupFileRef(int groupId, FileInGroup fileInGroup)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateAclGroupFileRef), new object[2]
      {
        (object) groupId,
        (object) fileInGroup
      });
      try
      {
        AclGroupFileAccessor.UpdateAclGroupFileRef(groupId, fileInGroup);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileInGroup GetAclGroupFileRef(int groupId, int fileId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupFileRef), new object[2]
      {
        (object) groupId,
        (object) fileId
      });
      try
      {
        return AclGroupFileAccessor.GetAclGroupFileRef(groupId, fileId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (FileInGroup) null;
      }
    }

    public virtual FileInGroup[] GetAclGroupFileRefs(int groupId, AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupFileRefs), new object[2]
      {
        (object) groupId,
        (object) fileType
      });
      try
      {
        return AclGroupFileAccessor.GetAclGroupFileRefs(groupId, fileType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (FileInGroup[]) null;
      }
    }

    public virtual Dictionary<AclFileType, FileInGroup[]> GetAclGroupFileRefs(
      int[] groupIds,
      AclFileType[] fileTypes)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupFileRefs), new object[2]
      {
        (object) groupIds,
        (object) fileTypes
      });
      try
      {
        return AclGroupFileAccessor.GetAclGroupFileRefs(groupIds, fileTypes);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFileType, FileInGroup[]>) null;
      }
    }

    public virtual int[] GetAclGroupFileRefIDs(int groupId, AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupFileRefIDs), new object[1]
      {
        (object) groupId
      });
      try
      {
        return AclGroupFileAccessor.GetAclGroupFileRefIDs(groupId, fileType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual Dictionary<AclFileType, FileSystemEntry> GetRootFileSystemEntry(
      AclFileType[] fileTypes)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetRootFileSystemEntry), new object[1]
      {
        (object) fileTypes
      });
      try
      {
        Dictionary<AclFileType, FileSystemEntry> rootFileSystemEntry = new Dictionary<AclFileType, FileSystemEntry>();
        foreach (AclFileType fileType in fileTypes)
          rootFileSystemEntry.Add(fileType, AclGroupFileAccessor.GetRootFileSystemEntry(fileType));
        return rootFileSystemEntry;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (Dictionary<AclFileType, FileSystemEntry>) null;
      }
    }

    public virtual AclFileResource GetAclFileResource(int fileId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclFileResource), new object[1]
      {
        (object) fileId
      });
      try
      {
        return AclGroupFileAccessor.GetAclFileResource(fileId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclFileResource) null;
      }
    }

    public virtual AclFileResource[] GetAclFileResources(int[] fileIds)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclFileResources), new object[1]
      {
        (object) fileIds
      });
      try
      {
        return AclGroupFileAccessor.GetAclFileResources(fileIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclFileResource[]) null;
      }
    }

    public virtual bool CheckPublicAccessPermission(AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CheckPublicAccessPermission), new object[1]
      {
        (object) fileType
      });
      try
      {
        return this.Session.GetUserInfo().IsSuperAdministrator() || AclGroupFileAccessor.CheckPublicAccessPermission(fileType, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual Hashtable CheckPublicAccessPermissions(AclFileType[] fileTypes)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CheckPublicAccessPermissions), new object[1]
      {
        (object) fileTypes
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        Hashtable hashtable = new Hashtable();
        foreach (AclFileType fileType in fileTypes)
        {
          if (userInfo.IsSuperAdministrator())
            hashtable[(object) fileType] = (object) true;
          else
            hashtable[(object) fileType] = (object) AclGroupFileAccessor.CheckPublicAccessPermission(fileType, this.Session.UserID);
        }
        return hashtable;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual bool CheckPublicAccessPermissionToAny(AclFileType[] fileTypes)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CheckPublicAccessPermissionToAny), new object[1]
      {
        (object) fileTypes
      });
      try
      {
        return this.Session.GetUserInfo().IsSuperAdministrator() || AclGroupFileAccessor.CheckPublicAccessPermissionToAny(fileTypes, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool CheckPublicAccessPermission(AclFileType fileType, string userId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CheckPublicAccessPermission), new object[3]
      {
        (object) fileType,
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (latestVersion.UserInfo.IsSuperAdministrator())
            return true;
        }
        return AclGroupFileAccessor.CheckPublicAccessPermission(fileType, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual AclResourceAccess GetMaxPublicFolderAccess(AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetMaxPublicFolderAccess), new object[1]
      {
        (object) fileType
      });
      try
      {
        return this.Session.GetUserInfo().IsSuperAdministrator() ? AclResourceAccess.ReadWrite : AclGroupFileAccessor.GetMaxPublicFolderAccess(fileType, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclResourceAccess.None;
      }
    }

    public virtual AclResourceAccess GetUserFileFolderAccess(
      AclFileType fileType,
      FileSystemEntry fsEntry)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUserFileFolderAccess), new object[2]
      {
        (object) fileType,
        (object) fsEntry
      });
      try
      {
        return this.Session.GetUserInfo().IsSuperAdministrator() ? AclResourceAccess.ReadWrite : AclGroupFileAccessor.GetUserFileFolderAccess(fileType, fsEntry, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclResourceAccess.None;
      }
    }

    public virtual FileSystemEntry[] GetUserFileFoldersAccess(
      AclFileType fileType,
      FileSystemEntry[] fsEntry)
    {
      this.onApiCalled(nameof (AclGroupManager), "GetUserFileFolderAccess", new object[2]
      {
        (object) fileType,
        (object) fsEntry
      });
      FileSystemEntry[] fileFoldersAccess = new FileSystemEntry[fsEntry.Length];
      fsEntry.CopyTo((Array) fileFoldersAccess, 0);
      bool flag = this.Session.GetUserInfo().IsSuperAdministrator();
      try
      {
        foreach (FileSystemEntry fsEntry1 in fileFoldersAccess)
          fsEntry1.Access = !flag ? AclGroupFileAccessor.GetUserFileFolderAccess(fileType, fsEntry1, this.Session.UserID) : AclResourceAccess.ReadWrite;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return new FileSystemEntry[fsEntry.Length];
      }
      return fileFoldersAccess;
    }

    public virtual AclResourceAccess GetUserFileFolderAccess(
      AclFileType fileType,
      FileSystemEntry fsEntry,
      string userId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUserFileFolderAccess), new object[4]
      {
        (object) fileType,
        (object) fsEntry,
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            throw new ObjectNotFoundException("Cannot located specific user", ObjectType.User, (object) userId);
          if (latestVersion.UserInfo.IsSuperAdministrator())
            return AclResourceAccess.ReadWrite;
        }
        return AclGroupFileAccessor.GetUserFileFolderAccess(fileType, fsEntry, userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclResourceAccess.None;
      }
    }

    public virtual Hashtable GetUserFileFolderAccess(
      AclFileType fileType,
      FileSystemEntry[] fsEntries)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUserFileFolderAccess), new object[2]
      {
        (object) fileType,
        (object) fsEntries
      });
      try
      {
        UserInfo userInfo = this.Session.GetUserInfo();
        Hashtable fileFolderAccess = new Hashtable();
        foreach (FileSystemEntry fsEntry in fsEntries)
        {
          if (userInfo.IsSuperAdministrator())
            fileFolderAccess[(object) fsEntry] = (object) AclResourceAccess.ReadWrite;
          else
            fileFolderAccess[(object) fsEntry] = (object) AclGroupFileAccessor.GetUserFileFolderAccess(fileType, fsEntry, this.Session.UserID);
        }
        return fileFolderAccess;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual void ResetAclGroupFileRefs(
      int groupId,
      FileInGroup[] filesInGroup,
      AclFileType fileType,
      int[] resetFileIDs)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetAclGroupFileRefs), new object[4]
      {
        (object) groupId,
        (object) filesInGroup,
        (object) fileType,
        (object) resetFileIDs
      });
      try
      {
        AclGroupFileAccessor.ResetAclGroupFileRefs(groupId, filesInGroup, fileType, resetFileIDs, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ResetAclGroupFileRefs(
      int groupId,
      FileInGroup[] filesInGroup,
      AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetAclGroupFileRefs), new object[3]
      {
        (object) groupId,
        (object) filesInGroup,
        (object) fileType
      });
      try
      {
        AclGroupFileAccessor.ResetAclGroupFileRefs(groupId, filesInGroup, fileType, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void ResetAclGroupFileRefs(
      int groupId,
      Dictionary<AclFileType, FileInGroup[]> updateList)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetAclGroupFileRefs), new object[2]
      {
        (object) groupId,
        (object) updateList
      });
      try
      {
        foreach (AclFileType key in updateList.Keys)
          AclGroupFileAccessor.ResetAclGroupFileRefs(groupId, updateList[key], key, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateFileResource(
      string oldName,
      string newName,
      int fileType,
      bool isFolder)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateFileResource), new object[4]
      {
        (object) oldName,
        (object) newName,
        (object) fileType,
        (object) isFolder
      });
      try
      {
        AclGroupFileAccessor.UpdateFileResources(oldName, newName, fileType, isFolder);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual Dictionary<int, AclFileResource> AddAclFileResources(
      AclFileResource[] fileResources)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddAclFileResources), (object[]) fileResources);
      try
      {
        return AclGroupFileAccessor.AddAclFileResources(fileResources);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (Dictionary<int, AclFileResource>) null;
      }
    }

    public virtual OrgInGroupRole[] GetOrgsInGroupRole(int groupID, int roleID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetOrgsInGroupRole), new object[2]
      {
        (object) groupID,
        (object) roleID
      });
      try
      {
        return AclGroupRoleAccessor.GetOrgsInGroupRole(groupID, roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (OrgInGroupRole[]) null;
      }
    }

    public virtual UserInGroupRole[] GetUsersInGroupRole(int groupID, int roleID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersInGroupRole), new object[2]
      {
        (object) groupID,
        (object) roleID
      });
      try
      {
        return AclGroupRoleAccessor.GetUsersInGroupRole(groupID, roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (UserInGroupRole[]) null;
      }
    }

    public virtual AclGroupRoleMembers GetMembersInGroupRole(int groupID, int roleID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetMembersInGroupRole), new object[2]
      {
        (object) groupID,
        (object) roleID
      });
      try
      {
        return AclGroupRoleAccessor.GetMembersInGroupRole(groupID, roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return new AclGroupRoleMembers(groupID, roleID);
      }
    }

    public virtual void AddUserToGroupRole(int groupID, UserInGroupRole userInGroupRole)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddUserToGroupRole), new object[2]
      {
        (object) groupID,
        (object) userInGroupRole
      });
      try
      {
        AclGroupRoleAccessor.AddUserToGroupRole(groupID, userInGroupRole);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AddOrgToGroupRole(int groupID, OrgInGroupRole orgInGroupRole)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (AddOrgToGroupRole), new object[2]
      {
        (object) groupID,
        (object) orgInGroupRole
      });
      try
      {
        AclGroupRoleAccessor.AddOrgToGroupRole(groupID, orgInGroupRole);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUserFromGroupRole(int groupID, int roleID, string userid)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteUserFromGroupRole), new object[3]
      {
        (object) groupID,
        (object) roleID,
        (object) userid
      });
      try
      {
        AclGroupRoleAccessor.DeleteUserFromGroupRole(groupID, roleID, userid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOrgFromGroupRole(int groupID, int roleID, int orgID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (DeleteOrgFromGroupRole), new object[3]
      {
        (object) groupID,
        (object) roleID,
        (object) orgID
      });
      try
      {
        AclGroupRoleAccessor.DeleteOrgFromGroupRole(groupID, roleID, orgID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateMembersInGroupRole(
      int groupID,
      int roleID,
      string[] resetUserList,
      int[] resetOrgList,
      string[] newUserList,
      int[] newOrgList,
      int[] newInclusiveOrgList)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupRole), new object[6]
      {
        (object) groupID,
        (object) resetUserList,
        (object) resetOrgList,
        (object) newUserList,
        (object) newOrgList,
        (object) newInclusiveOrgList
      });
      try
      {
        AclGroupRoleAccessor.UpdateUsersInGroupRole(groupID, roleID, resetUserList, newUserList, this.Session.UserID);
        AclGroupRoleAccessor.UpdateOrgsInGroupRole(groupID, roleID, resetOrgList, newOrgList, newInclusiveOrgList, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateMembersInGroupRole(
      int groupID,
      int roleID,
      UserInGroupRole[] users,
      OrgInGroupRole[] orgs)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateMembersInGroupRole), new object[4]
      {
        (object) groupID,
        (object) roleID,
        (object) users,
        (object) orgs
      });
      try
      {
        AclGroupRoleAccessor.ResetOrgsInGroupRole(groupID, roleID, orgs);
        AclGroupRoleAccessor.ResetUsersInGroupRole(groupID, roleID, users);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual AclGroupRoleAccessLevel GetAclGroupRoleAccessLevel(int groupID, int roleID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupRoleAccessLevel), new object[2]
      {
        (object) groupID,
        (object) roleID
      });
      try
      {
        return AclGroupRoleAccessor.GetAclGroupRoleAccessLevel(groupID, roleID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (AclGroupRoleAccessLevel) null;
      }
    }

    public virtual void UpdateAclGroupRoleAccessLevel(AclGroupRoleAccessLevel accessLevel)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateAclGroupRoleAccessLevel), new object[1]
      {
        (object) accessLevel
      });
      try
      {
        AclGroupRoleAccessor.UpdateAclGroupRoleAccessLevel(accessLevel, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetUsersStdPrintForms()
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersStdPrintForms), Array.Empty<object>());
      try
      {
        return AclGroupStdPrintFormAccessor.GetUsersStdPrintForms(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string[] GetAclGroupStdPrintForms(int groupId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupStdPrintForms), new object[1]
      {
        (object) groupId
      });
      try
      {
        return AclGroupStdPrintFormAccessor.GetAclGroupStdPrintForms(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void ResetAclGroupStdPrintForms(int groupId, string[] fileIds)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetAclGroupStdPrintForms), new object[1]
      {
        (object) groupId
      });
      try
      {
        AclGroupStdPrintFormAccessor.ResetAclGroupStdPrintForms(groupId, fileIds, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string[] GetUsersChangeCircumstanceOptions()
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetUsersChangeCircumstanceOptions), Array.Empty<object>());
      try
      {
        return AclChangeCircumstanceAccessor.GetUsersChangeCircumstanceOptions(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual string[] GetAclGroupChangeCircumstanceOptions(int groupId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetAclGroupChangeCircumstanceOptions), new object[1]
      {
        (object) groupId
      });
      try
      {
        return AclChangeCircumstanceAccessor.GetAclGroupChangeCircumstanceOptions(groupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void ResetAclGroupChangeCircumstanceOptions(int groupId, string[] fileIds)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetAclGroupChangeCircumstanceOptions), new object[1]
      {
        (object) groupId
      });
      try
      {
        AclChangeCircumstanceAccessor.ResetAclGroupChangeCircumstanceOptions(groupId, fileIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual BizGroupRef[] GetBizContactGroupRefs(int aclGroupId)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetBizContactGroupRefs), new object[1]
      {
        (object) aclGroupId
      });
      try
      {
        return AclGroupBizGroupAccessor.GetBizContactGroupRefs(aclGroupId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (BizGroupRef[]) null;
      }
    }

    public virtual BizGroupRef[] GetBizContactGroupRefs(string userID, bool editOnly)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetBizContactGroupRefs), new object[2]
      {
        (object) userID,
        (object) editOnly
      });
      try
      {
        AclGroup[] groupsOfUser = this.GetGroupsOfUser(userID);
        ArrayList arrayList = new ArrayList();
        foreach (AclGroup aclGroup in groupsOfUser)
        {
          BizGroupRef[] contactGroupRefs = this.GetBizContactGroupRefs(aclGroup.ID);
          for (int index = 0; index < contactGroupRefs.Length; ++index)
          {
            AclResourceAccess access = contactGroupRefs[index].Access;
            if ((!editOnly || access == AclResourceAccess.ReadWrite) && !arrayList.Contains((object) contactGroupRefs[index]))
              arrayList.Add((object) contactGroupRefs[index]);
          }
        }
        return (BizGroupRef[]) arrayList.ToArray(typeof (BizGroupRef));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (BizGroupRef[]) null;
      }
    }

    public virtual AclTriState GetBizContactAccessRight(UserInfo userObj, int contactID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetBizContactAccessRight), new object[2]
      {
        (object) userObj,
        (object) contactID
      });
      try
      {
        return AclGroupBizGroupAccessor.GetBizContactAccessRight(userObj, contactID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual AclResourceAccess GetBorrowerContactAccessRight(
      UserInfo userObj,
      string contactOwnerID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetBorrowerContactAccessRight), new object[2]
      {
        (object) userObj,
        (object) contactOwnerID
      });
      try
      {
        UserInfo userById = User.GetUserById(contactOwnerID);
        AclTriState contactAccessRight1 = AclGroupAccessor.GetBorrowerContactAccessRight(AclGroupAccessor.GetGroupsOfUser(contactOwnerID), userObj, userById);
        if (contactAccessRight1 == AclTriState.True)
          return AclResourceAccess.ReadWrite;
        AclTriState contactAccessRight2 = AclGroupContactAccessor.GetBorrowerContactAccessRight(AclGroupAccessor.GetGroupsOfUser(userObj.Userid), userById);
        return contactAccessRight1 == AclTriState.True || contactAccessRight2 == AclTriState.True || contactAccessRight1 == AclTriState.Unspecified && contactAccessRight2 == AclTriState.Unspecified ? AclResourceAccess.ReadWrite : AclResourceAccess.ReadOnly;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclResourceAccess.ReadOnly;
      }
    }

    public virtual AclTriState GetBizContactGroupAccessRight(UserInfo userObj, int contactGroupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetBizContactGroupAccessRight), new object[2]
      {
        (object) userObj,
        (object) contactGroupID
      });
      try
      {
        return AclGroupBizGroupAccessor.GetBizContactGroupAccessRight(userObj, contactGroupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return AclTriState.Unspecified;
      }
    }

    public virtual void ResetBizContactGroupRefs(int aclGroupId, BizGroupRef[] bizGroupRefs)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ResetBizContactGroupRefs), new object[1]
      {
        (object) aclGroupId
      });
      try
      {
        AclGroupBizGroupAccessor.ResetBizContactGroupRefs(aclGroupId, bizGroupRefs, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdateBizContactGroupRef(int aclGroupId, BizGroupRef bizGroupRef)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpdateBizContactGroupRef), new object[1]
      {
        (object) aclGroupId
      });
      try
      {
        AclGroupBizGroupAccessor.UpdateBizContactGroupRef(aclGroupId, bizGroupRef, this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void CloneAclGroup(int sourceGroupID, int desGroupID)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CloneAclGroup), new object[2]
      {
        (object) sourceGroupID,
        (object) desGroupID
      });
      try
      {
        AclGroupAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupBizGroupAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupFileAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupLoanAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupContactAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupRoleAccessor.Clone(sourceGroupID, desGroupID);
        AclGroupStdPrintFormAccessor.Clone(sourceGroupID, desGroupID);
        AclChangeCircumstanceAccessor.Clone(sourceGroupID, desGroupID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual FileSystemEntry[] ApplyUserAccessRights(
      UserInfo userInfo,
      FileSystemEntry[] fsEntries,
      AclFileType fileType)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (ApplyUserAccessRights), new object[3]
      {
        (object) userInfo,
        (object) fsEntries,
        (object) fileType
      });
      try
      {
        return AclGroupFileAccessor.ApplyUserAccessRights(userInfo, fsEntries, fileType);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return (FileSystemEntry[]) null;
      }
    }

    public virtual string[] GetConditionalApprovalLetter(UserInfo userObj)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (GetConditionalApprovalLetter), new object[1]
      {
        (object) userObj
      });
      try
      {
        List<string> stringList = new List<string>();
        List<int> intList = new List<int>();
        if (userObj.IsAdministrator())
        {
          FileSystemEntry[] publicFileEntries = TemplateSettingsStore.GetAllPublicFileEntries(TemplateSettingsType.ConditionalLetter, false);
          if (publicFileEntries == null || publicFileEntries.Length == 0)
            return stringList.ToArray();
          foreach (FileSystemEntry fileSystemEntry in publicFileEntries)
          {
            if (!stringList.Contains(fileSystemEntry.Name))
              stringList.Add(fileSystemEntry.Name);
          }
        }
        else
        {
          AclGroup[] groupsOfUser = AclGroupAccessor.GetGroupsOfUser(userObj.Userid);
          if (groupsOfUser == null || groupsOfUser.Length == 0)
            return stringList.ToArray();
          foreach (AclGroup aclGroup in groupsOfUser)
          {
            FileInGroup[] aclGroupFileRefs = AclGroupFileAccessor.GetAclGroupFileRefs(aclGroup.ID, AclFileType.ConditionalApprovalLetter);
            if (aclGroupFileRefs != null && aclGroupFileRefs.Length != 0)
            {
              foreach (FileInGroup fileInGroup in aclGroupFileRefs)
              {
                if (!intList.Contains(fileInGroup.FileID))
                  intList.Add(fileInGroup.FileID);
              }
            }
          }
          AclFileResource[] aclFileResources = AclGroupFileAccessor.GetAclFileResources(intList.ToArray());
          if (aclFileResources == null || aclFileResources.Length == 0)
            return stringList.ToArray();
          foreach (AclFileResource aclFileResource in aclFileResources)
          {
            if (!stringList.Contains(aclFileResource.FileName))
              stringList.Add(aclFileResource.FileName);
          }
        }
        return stringList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        return new string[0];
      }
    }

    public virtual bool CreateUserGroup(
      UserGroupDetails userGroupDetails,
      string loggedInUserId,
      bool returnEntityData,
      out int newGroupId,
      out UserGroupDetails responseData)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (CreateUserGroup), new object[3]
      {
        (object) userGroupDetails,
        (object) loggedInUserId,
        (object) returnEntityData
      });
      try
      {
        int num = AclGroupAccessor.CreateUserGroup(userGroupDetails, loggedInUserId, returnEntityData, out newGroupId, out responseData) ? 1 : 0;
        if (num != 0)
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupCreated, DateTime.Now, responseData.GroupInfo.ID, responseData.GroupInfo.Name));
        return num != 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual (OrgInGroup[], UserGroupMemberUser[]) UpsertMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId,
      bool returnDetails,
      string groupName,
      bool isUpdate)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (UpsertMembers), new object[6]
      {
        (object) groupId,
        (object) orgs,
        (object) userIds,
        (object) userId,
        (object) returnDetails,
        (object) groupName
      });
      try
      {
        (OrgInGroup[] orgInGroupArray, UserGroupMemberUser[] userGroupMemberUserArray) = AclGroupAccessor.UpsertMembers(groupId, orgs, userIds, userId, returnDetails, isUpdate);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupModified, DateTime.Now, groupId, groupName));
        return (orgInGroupArray, userGroupMemberUserArray);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }

    public virtual void RemoveMembers(
      int groupId,
      IEnumerable<OrgInGroup> orgs,
      IEnumerable<string> userIds,
      string userId,
      string groupName)
    {
      this.onApiCalled(nameof (AclGroupManager), nameof (RemoveMembers), new object[5]
      {
        (object) groupId,
        (object) orgs,
        (object) userIds,
        (object) userId,
        (object) groupName
      });
      try
      {
        AclGroupAccessor.RemoveMembers(groupId, orgs, userIds, userId);
        SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserGroupAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserGroupModified, DateTime.Now, groupId, groupName));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (AclGroupManager), ex, this.Session.SessionInfo);
        throw ex;
      }
    }
  }
}
