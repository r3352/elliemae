// Decompiled with JetBrains decompiler
// Type: Elli.Server.Remoting.SessionObjects.OrganizationManager
// Assembly: Elli.Server.Remoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D137973E-0067-435D-9623-8CEE2207CDBE
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Server.Remoting.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Exceptions;
using EllieMae.EMLite.ClientServer.SystemAuditTrail;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataAccess;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Server.ServerObjects;
using EllieMae.EMLite.Server.ServerObjects.Bpm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace Elli.Server.Remoting.SessionObjects
{
  public class OrganizationManager : SessionBoundObject, IOrganizationManager
  {
    private const string className = "OrganizationManager";
    private bool showAdditionalTerritories;
    private const string getScopedUsersCacheKey = "GetScopedUsers";
    private const string OrganizationsCacheKey = "GetAllOrganizationsCacheKey";

    public OrganizationManager Initialize(ISession session)
    {
      this.InitializeInternal(session, nameof (OrganizationManager).ToLower());
      this.showAdditionalTerritories = string.Equals(((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanySetting("Policies", "ShowAdditionalTerritories"), "True", StringComparison.CurrentCultureIgnoreCase);
      return this;
    }

    public virtual UserInfoSummary[] GetScopedUserInfos()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetScopedUserInfos), Array.Empty<object>());
      try
      {
        UserInfo[] scopedUsers = this.GetScopedUsers();
        List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
        if (scopedUsers == null)
          return userInfoSummaryList.ToArray();
        foreach (UserInfo userInfo in scopedUsers)
          userInfoSummaryList.Add(new UserInfoSummary(userInfo.Userid, userInfo.LastName, userInfo.FirstName));
        return userInfoSummaryList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfoSummary[]) null;
      }
    }

    public virtual UserInfoSummary[] GetScopedUsersWithRoles(int[] roleIds)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetScopedUsersWithRoles), new object[1]
      {
        (object) roleIds
      });
      try
      {
        List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
        foreach (int roleId in roleIds)
        {
          UserInfo[] scopedUsersWithRole = User.GetScopedUsersWithRole(this.Session.UserID, roleId);
          if (scopedUsersWithRole != null)
          {
            foreach (UserInfo userInfo in scopedUsersWithRole)
            {
              UserInfoSummary userInfoSummary = new UserInfoSummary(userInfo.Userid, userInfo.LastName, userInfo.FirstName);
              if (!userInfoSummaryList.Contains(userInfoSummary))
                userInfoSummaryList.Add(userInfoSummary);
            }
          }
        }
        return userInfoSummaryList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfoSummary[]) null;
      }
    }

    public virtual UserInfoSummary[] GetAllUserInfoSummary()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllUserInfoSummary), Array.Empty<object>());
      List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
      try
      {
        UserInfo[] allUsers = User.GetAllUsers(this.Session.UserID);
        if (allUsers == null || allUsers.Length == 0)
          return userInfoSummaryList.ToArray();
        foreach (UserInfo userInfo in allUsers)
          userInfoSummaryList.Add(new UserInfoSummary(userInfo.Userid, userInfo.LastName, userInfo.FirstName));
        return userInfoSummaryList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return userInfoSummaryList.ToArray();
      }
    }

    public virtual void CreateNewUser(UserInfo info)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (CreateNewUser), new object[1]
      {
        (object) info
      });
      if (info == (UserInfo) null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("UserInfo cannot be null", nameof (info), this.Session.SessionInfo));
      if ((info.Userid ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (info), this.Session.SessionInfo));
      try
      {
        using (this.Session.Context.Cache.Lock(Organization.SyncRootKey))
        {
          using (Organization latestVersion = OrganizationStore.GetLatestVersion(info.OrgId))
          {
            if (!latestVersion.Exists)
              Err.Raise(TraceLevel.Error, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invalid organization ID '" + (object) info.OrgId + "'", ObjectType.Organization, (object) info.OrgId));
            using (User user = UserStore.CheckOut(info.Userid))
            {
              if (user.Exists)
                Err.Raise(TraceLevel.Error, nameof (OrganizationManager), (ServerException) new DuplicateObjectException("User with ID \"" + info.Userid + "\" already exists.", ObjectType.User, (object) info.Userid));
              user.CreateNew(info, this.Session.GetUserInfo());
              SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserProfileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserCreated, DateTime.Now, info.Userid, info.FullName));
              if (info.RequirePasswordChange)
                SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserPwdChangeForcedAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserPasswordChangeForced, DateTime.Now, info.Userid, info.FullName));
            }
          }
        }
        TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Created new user \"" + info.Userid + "\""));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteUser(
      string userId,
      UserAssignedContactsBehaviorEnums? assignedContactsBehavior = null,
      string reassignContactsToUser = null)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to delete a user", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (DeleteUser), new object[1]
        {
          (object) userId
        });
        if ((userId ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
        try
        {
          using (User user = UserStore.CheckOut(userId))
          {
            if (!user.Exists)
              return;
            string fullName = user.UserInfo.FullName;
            user.Delete(this.Session.GetUserInfo(), assignedContactsBehavior, reassignContactsToUser);
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserProfileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserDeleted, DateTime.Now, userId, fullName));
            TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Deleted user \"" + userId + "\""));
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void EnableUser(string userId)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to enable a user", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (EnableUser), new object[1]
        {
          (object) userId
        });
        if ((userId ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
        try
        {
          using (User user = UserStore.CheckOut(userId))
          {
            if (!user.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("User not found", ObjectType.User, (object) userId));
            user.UserInfo.Status = UserInfo.UserStatusEnum.Enabled;
            user.FailedLoginAttempts = 0;
            user.CheckIn(this.Session.UserID, false);
          }
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Enabled user \"" + userId + "\""));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void DisableUser(string userId)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to disable a user", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (DisableUser), new object[1]
        {
          (object) userId
        });
        if ((userId ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
        try
        {
          using (User user = UserStore.CheckOut(userId))
          {
            if (!user.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("User not found", ObjectType.User, (object) userId));
            user.UserInfo.Status = UserInfo.UserStatusEnum.Disabled;
            user.CheckIn(this.Session.UserID, false);
          }
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Disabled user \"" + userId + "\""));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual UserInfo GetUser(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUser), new object[1]
      {
        (object) userId
      });
      if ((userId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
      try
      {
        User latestVersion = UserStore.GetLatestVersion(userId);
        return latestVersion.Exists ? latestVersion.UserInfo : (UserInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo) null;
      }
    }

    public virtual UserInfo GetUser(string userId, bool fromDb)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUser), new object[2]
      {
        (object) userId,
        (object) this.dbReadReplicaLog(this.Session.Context, DBReadReplicaFeature.Login)
      });
      if ((userId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
      try
      {
        User latestVersion = UserStore.GetLatestVersion(userId, fromDb);
        return latestVersion.Exists ? latestVersion.UserInfo : (UserInfo) null;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo) null;
      }
    }

    public virtual void CreateCCSiteInfo(CCSiteInfo siteInfo, int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), "createCCSiteInfo", new object[1]
      {
        (object) siteInfo
      });
      CCSiteInfoAccessor.createCCSiteInfo(orgId, siteInfo.SiteId, siteInfo.Url);
    }

    public virtual void CreateUserCCSiteInfo(CCSiteInfo siteInfo, string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), "createCCSiteInfo", new object[1]
      {
        (object) siteInfo
      });
      CCSiteInfoAccessor.createUserCCSiteInfo(userId, siteInfo.SiteId, siteInfo.Url);
    }

    public virtual void updateCCSiteInfo(CCSiteInfo siteInfo, int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (updateCCSiteInfo), new object[1]
      {
        (object) siteInfo
      });
      CCSiteInfoAccessor.updateCCSiteInfo(siteInfo, orgId);
    }

    public virtual void UpdateUserCCSiteInfo(CCSiteInfo siteInfo, string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), "updateUserCCSiteInfo", new object[1]
      {
        (object) siteInfo
      });
      CCSiteInfoAccessor.updateUserCCSiteInfo(siteInfo, userId);
    }

    public virtual void UpdateUser(UserInfo info)
    {
      UserInfo userInfo = this.Session.GetUserInfo();
      IFeaturesAclManager aclManager = (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features);
      if (!SecurityManagerUtil.HasFeatureAccess(userInfo, aclManager, AclFeature.SettingsTab_OrganizationsUser) && !SecurityManagerUtil.HasFeatureAccess(userInfo, aclManager, AclFeature.SettingsTab_AllUserInformation))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to update a user", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (UpdateUser), new object[1]
        {
          (object) info
        });
        if (info == (UserInfo) null)
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("UserInfo cannot be null", nameof (info), this.Session.SessionInfo));
        if ((info.Userid ?? "") == "")
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", "userId", this.Session.SessionInfo));
        try
        {
          bool flag = false;
          using (User user = UserStore.CheckOut(info.Userid, this.Session.UserID))
          {
            if (!user.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("User not found with ID \"" + info.Userid + "\".", ObjectType.User, (object) info.Userid));
            if (!user.UserInfo.PasswordExists && !info.SSOOnly && user.IsInvalidPassword(info))
              Err.Raise(TraceLevel.Error, nameof (OrganizationManager), (ServerException) new SecurityException("The password specified does not meet the requirements for this system."));
            if (user.UserInfo.Status == UserInfo.UserStatusEnum.Disabled && info.Status == UserInfo.UserStatusEnum.Enabled)
              user.FailedLoginAttempts = 0;
            else if (user.UserInfo.Locked && !info.Locked)
              user.FailedLoginAttempts = 0;
            if (info.RequirePasswordChange && !user.UserInfo.RequirePasswordChange)
              flag = true;
            user.CheckIn(info, userInfo.Userid);
          }
          SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserProfileAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserModified, DateTime.Now, info.Userid, info.FullName));
          if (flag)
            SystemAuditTrailAccessor.InsertAuditRecord((SystemAuditRecord) new UserPwdChangeForcedAuditRecord(this.Session.UserID, this.Session.GetUserInfo().FullName, ActionType.UserPasswordChangeForced, DateTime.Now, info.Userid, info.FullName));
          if (info.Userid == this.Session.UserID)
            this.Security.RefreshUser();
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Updated user \"" + info.Userid + "\""));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual bool UserExists(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (UserExists), new object[1]
      {
        (object) userId
      });
      if ((userId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
      try
      {
        return UserStore.GetLatestVersion(userId).Exists;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual bool OAuthClientIdExists(string oAuthClientId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (OAuthClientIdExists), new object[1]
      {
        (object) oAuthClientId
      });
      if ((oAuthClientId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("OAuth Client ID cannot be blank or null", nameof (oAuthClientId), this.Session.SessionInfo));
      try
      {
        EllieMae.EMLite.Server.DbQueryBuilder dbQueryBuilder = new EllieMae.EMLite.Server.DbQueryBuilder();
        string str = SQL.Encode((object) oAuthClientId);
        dbQueryBuilder.AppendFormat("select count(*) from [users] where oAuthClientId = {0}", (object) str);
        return Convert.ToInt32(dbQueryBuilder.ExecuteScalar()) > 0;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual int[] GetDescendentsOfOrg(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetDescendentsOfOrg), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetDescendentsOfOrg(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (int[]) null;
      }
    }

    public virtual bool IsDescendentOfOrg(int parentOrgId, int childOrgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (IsDescendentOfOrg), new object[2]
      {
        (object) parentOrgId,
        (object) childOrgId
      });
      try
      {
        return OrganizationStore.IsDescendentOfOrg(parentOrgId, childOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual UserInfo[] GetAllUsers()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllUsers), Array.Empty<object>());
      try
      {
        return User.GetAllUsers(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetAllIntAndExtUsers()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllIntAndExtUsers), Array.Empty<object>());
      try
      {
        return User.GetAllIntAndExtUsers(this.Session.UserID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetUsersInOrgWithRole(int orgID, int roleId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersInOrgWithRole), new object[2]
      {
        (object) orgID,
        (object) roleId
      });
      try
      {
        return User.GetUsersWithInOrgWithRole(orgID, roleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfoSummary[] GetUserInfoSummaryInOrgWithRole(int orgID, int roleId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserInfoSummaryInOrgWithRole), new object[2]
      {
        (object) orgID,
        (object) roleId
      });
      try
      {
        UserInfo[] withInOrgWithRole = User.GetUsersWithInOrgWithRole(orgID, roleId);
        List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
        foreach (UserInfo userInfo in withInOrgWithRole)
          userInfoSummaryList.Add(new UserInfoSummary(userInfo));
        return userInfoSummaryList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return new UserInfoSummary[0];
      }
    }

    public virtual UserInfo[] GetUsersWithRole(int roleId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersWithRole), new object[1]
      {
        (object) roleId
      });
      try
      {
        return User.GetUsersWithRole(roleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual List<string> GetUsersWithRoles(int[] roleIds)
    {
      this.onApiCalled(nameof (OrganizationManager), "GetUsersWithRole", new object[1]
      {
        (object) roleIds
      });
      try
      {
        return User.GetUsersWithRoles(roleIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (List<string>) null;
      }
    }

    public virtual UserInfoSummary[] GetUserInfoSummaryWithRole(int roleId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserInfoSummaryWithRole), new object[1]
      {
        (object) roleId
      });
      try
      {
        UserInfo[] usersWithRole = User.GetUsersWithRole(roleId);
        List<UserInfoSummary> userInfoSummaryList = new List<UserInfoSummary>();
        foreach (UserInfo userInfo in usersWithRole)
          userInfoSummaryList.Add(new UserInfoSummary(userInfo));
        return userInfoSummaryList.ToArray();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return new UserInfoSummary[0];
      }
    }

    public virtual Hashtable GetUsers(string[] userIds) => this.GetUsers(userIds, false);

    public virtual Hashtable GetUsers(string[] userIds, bool summariesOnly)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsers), new object[2]
      {
        (object) userIds,
        (object) summariesOnly
      });
      if (userIds == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("ID list cannot be null", nameof (userIds), this.Session.SessionInfo));
      try
      {
        return User.GetUsers(userIds, summariesOnly);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (Hashtable) null;
      }
    }

    public virtual UserInfo[] GetUsersByName(string firstName, string lastName)
    {
      return this.GetUsersByName(firstName, "", lastName, "");
    }

    public virtual UserInfo[] GetUsersByName(
      string firstName,
      string middleName,
      string lastName,
      string suffixName)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersByName), new object[4]
      {
        (object) firstName,
        (object) middleName,
        (object) lastName,
        (object) suffixName
      });
      if ((firstName ?? "") == "" && (lastName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("First and last names cannot both be blank or null", nameof (firstName), this.Session.SessionInfo));
      try
      {
        return User.GetUsersByName(firstName ?? "", middleName ?? "", lastName ?? "", suffixName ?? "");
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetUsersInOrganization(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersInOrganization), new object[1]
      {
        (object) orgId
      });
      try
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          return User.GetUsersInOrganization(orgId);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetUsersUnderOrganization(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersUnderOrganization), new object[1]
      {
        (object) orgId
      });
      try
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          return User.GetUsersUnderOrganization(orgId);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetOrganizationSSOUsers(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrganizationSSOUsers), new object[1]
      {
        (object) orgId
      });
      try
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          return User.GetOrganizationSSOUsers(orgId);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    private void compareUsers(UserInfo[] users0, UserInfo[] users1)
    {
      if (users0.Length != users1.Length)
        throw new Exception("GetScopedUser: result lengths mismatch");
      foreach (UserInfo userInfo1 in users0)
      {
        bool flag = false;
        foreach (UserInfo userInfo2 in users1)
        {
          if (userInfo1.Userid == userInfo2.Userid)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          throw new Exception("GetScopedUser: user not found: " + userInfo1.Userid);
      }
    }

    public virtual UserInfo[] GetScopedUsers()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetScopedUsers), Array.Empty<object>());
      object serverSetting = ClientContext.GetCurrent().Cache.Get(nameof (GetScopedUsers));
      if (serverSetting == null)
      {
        serverSetting = ClientContext.GetCurrent().Settings.GetServerSetting("Internal.GetScopedUsers", true);
        ClientContext.GetCurrent().Cache.Put(nameof (GetScopedUsers), serverSetting);
      }
      UserInfo[] users1;
      switch ((int) serverSetting)
      {
        case 1:
          users1 = this.GetScopedUsersOld();
          break;
        case 2:
          UserInfo[] scopedUsersOld = this.GetScopedUsersOld();
          users1 = this.GetScopedUsersOpt();
          this.compareUsers(scopedUsersOld, users1);
          break;
        default:
          users1 = this.GetScopedUsersOpt();
          break;
      }
      return users1;
    }

    public virtual UserInfo[] GetScopedUsersOld()
    {
      try
      {
        UserInfo[] scopedUsersOld = new UserInfo[0];
        RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
        if (allRoleFunctions != null)
        {
          Hashtable hashtable = new Hashtable();
          foreach (RoleSummaryInfo roleSummaryInfo in allRoleFunctions)
          {
            UserInfo[] scopedUsersWithRole = User.GetScopedUsersWithRole(this.Session.UserID, roleSummaryInfo.RoleID);
            if (scopedUsersWithRole != null && scopedUsersWithRole.Length != 0)
            {
              foreach (UserInfo userInfo in scopedUsersWithRole)
              {
                if (!hashtable.Contains((object) userInfo.Userid))
                  hashtable.Add((object) userInfo.Userid, (object) userInfo);
              }
            }
          }
          scopedUsersOld = new UserInfo[hashtable.Count];
          hashtable.Values.CopyTo((Array) scopedUsersOld, 0);
        }
        return scopedUsersOld;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetScopedUsersOpt()
    {
      try
      {
        UserInfo[] scopedUsersOpt = new UserInfo[0];
        RoleInfo[] allRoleFunctions = WorkflowBpmDbAccessor.GetAllRoleFunctions();
        if (allRoleFunctions == null || allRoleFunctions.Length == 0)
          return scopedUsersOpt;
        int[] roleIDs = new int[allRoleFunctions.Length];
        for (int index = 0; index < roleIDs.Length; ++index)
          roleIDs[index] = allRoleFunctions[index].RoleID;
        UserInfo[] scopedUsersWithRoles = User.GetScopedUsersWithRoles(this.Session.GetUserInfo(), roleIDs);
        if (scopedUsersWithRoles != null && scopedUsersWithRoles.Length != 0)
        {
          Hashtable hashtable = new Hashtable();
          foreach (UserInfo userInfo in scopedUsersWithRoles)
          {
            if (!hashtable.Contains((object) userInfo.Userid))
              hashtable.Add((object) userInfo.Userid, (object) userInfo);
          }
          scopedUsersOpt = new UserInfo[hashtable.Count];
          hashtable.Values.CopyTo((Array) scopedUsersOpt, 0);
        }
        return scopedUsersOpt;
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetScopedUsersWithRole(int roleId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetScopedUsersWithRole), new object[1]
      {
        (object) roleId
      });
      try
      {
        return User.GetScopedUsersWithRole(this.Session.UserID, roleId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual Dictionary<string, UserLoginInfo> GetUserLoginInfos(
      string[] userIds,
      bool isTPOMVP = false)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserLoginInfos), (object[]) userIds);
      try
      {
        return User.GetUserLoginInfos(userIds, this.Session.UserID, isTPOMVP);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (Dictionary<string, UserLoginInfo>) null;
      }
    }

    public virtual UserInfo[] GetAllAccessibleSalesRepUsers()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllAccessibleSalesRepUsers), Array.Empty<object>());
      try
      {
        return User.GetAllAccessibleSalesRepUsers();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfo[] GetAllAccessibleUsers()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllAccessibleUsers), Array.Empty<object>());
      try
      {
        return User.GetAllAccessibleUsers(UserStore.GetLatestVersion(this.Session.UserID));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual int GetEnabledUserCount()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetEnabledUserCount), Array.Empty<object>());
      try
      {
        return User.GetEnabledUserCount((IClientContext) ClientContext.GetCurrent());
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return 0;
      }
    }

    public virtual LOLicenseInfo[] GetLOLicenses(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetLOLicenses), new object[1]
      {
        (object) userId
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          LOLicenseInfo[] source = latestVersion.GetLOLicenses();
          if (!this.showAdditionalTerritories && source != null && ((IEnumerable<LOLicenseInfo>) source).Count<LOLicenseInfo>() > 0)
          {
            string[] territories = Utils.GetAdditionalTerritories();
            source = ((IEnumerable<LOLicenseInfo>) source).Where<LOLicenseInfo>((Func<LOLicenseInfo, bool>) (val => !((IEnumerable<string>) territories).Contains<string>(val.StateAbbr))).ToArray<LOLicenseInfo>();
          }
          return source;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (LOLicenseInfo[]) null;
      }
    }

    public virtual LOLicenseInfo GetLOLicense(string userId, string state)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetLOLicense), new object[2]
      {
        (object) userId,
        (object) state
      });
      if ((userId ?? "") == "")
        Err.Raise(nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
      if ((state ?? "") == "")
        Err.Raise(nameof (OrganizationManager), (ServerException) new ServerArgumentException("State abbreviation cannot be blank or null", nameof (state), this.Session.SessionInfo));
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          LOLicenseInfo loLicense = latestVersion.GetLOLicense(state);
          return !this.showAdditionalTerritories && loLicense != null && ((IEnumerable<string>) Utils.GetAdditionalTerritories()).Contains<string>(loLicense.StateAbbr) ? (LOLicenseInfo) null : loLicense;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (LOLicenseInfo) null;
      }
    }

    public virtual string[] GetLOLicensedStates(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetLOLicensedStates), new object[1]
      {
        (object) userId
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          string[] source = latestVersion.GetAllLicensedStates();
          if (!this.showAdditionalTerritories && source != null && ((IEnumerable<string>) source).Count<string>() > 0)
          {
            string[] territories = Utils.GetAdditionalTerritories();
            source = ((IEnumerable<string>) source).Where<string>((Func<string, bool>) (val => !((IEnumerable<string>) territories).Contains<string>(val))).ToArray<string>();
          }
          return source;
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (string[]) null;
      }
    }

    public virtual void AddLOLicense(LOLicenseInfo license)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (AddLOLicense), new object[1]
      {
        (object) license
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(license.UserID))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) license.UserID));
          latestVersion.AddLOLicense(license);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void SetLOLicenses(string userId, LOLicenseInfo[] licenses)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (SetLOLicenses), new object[2]
      {
        (object) userId,
        (object) licenses
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          latestVersion.DeleteAllLOLicenses();
          foreach (LOLicenseInfo license in licenses)
            latestVersion.AddLOLicense(license);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteLOLicense(string userId, string state)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (DeleteLOLicense), new object[2]
      {
        (object) userId,
        (object) state
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          latestVersion.DeleteLOLicense(state);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteAllLOLicenses(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (DeleteAllLOLicenses), new object[1]
      {
        (object) userId
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          latestVersion.DeleteAllLOLicenses();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual string GetSettingsRptXML(string userId, string reportID)
    {
      this.onApiCalled(nameof (OrganizationManager), "GetXMLSettingsRpt", new object[1]
      {
        (object) userId
      });
      try
      {
        using (User user = UserStore.CheckOut(userId))
        {
          if (!user.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          return user.getXMLSettingsRpt(reportID);
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual SettingsRptJobInfo[] GetSettingsRptJobs(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetSettingsRptJobs), new object[1]
      {
        (object) userId
      });
      try
      {
        using (User latestVersion = UserStore.GetLatestVersion(userId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("The specified user does not exist", ObjectType.User, (object) userId));
          return SettingsReportAccessor.GetSettingsRptJobs();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (SettingsRptJobInfo[]) null;
      }
    }

    public virtual OrgInfo GetRootOrganization()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetRootOrganization), Array.Empty<object>());
      try
      {
        return OrganizationStore.GetLatestVersion(OrganizationStore.RootOrganizationID).GetOrganizationInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual CCSiteInfo getCCSiteInfo(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), "createCCSiteInfo", new object[1]
      {
        (object) orgId
      });
      try
      {
        return CCSiteInfoAccessor.getCCSiteInfo(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
      return (CCSiteInfo) null;
    }

    public virtual CCSiteInfo GetUserCCSiteInfo(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserCCSiteInfo), new object[1]
      {
        (object) userId
      });
      try
      {
        return CCSiteInfoAccessor.getUserCCSiteInfo(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
      return (CCSiteInfo) null;
    }

    public virtual OrgInfo GetOrganization(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrganization), new object[1]
      {
        (object) orgId
      });
      try
      {
        using (Organization latestVersion = OrganizationStore.GetLatestVersion(orgId))
        {
          if (!latestVersion.Exists)
            Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invalid organization ID", ObjectType.Organization, (object) orgId));
          return latestVersion.GetOrganizationInfo();
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo[] GetOrganizations(int[] orgIds)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrganizations), new object[1]
      {
        (object) orgIds
      });
      if (orgIds == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("Org ID List cannot be null", nameof (orgIds)));
      try
      {
        return Organization.LoadOrganizations(orgIds);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo[]) null;
      }
    }

    public virtual OrgInfo[] GetAllOrganizations()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllOrganizations), Array.Empty<object>());
      try
      {
        return Organization.GetAllOrganizationInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo[]) null;
      }
    }

    public virtual OrgInfo[] GetAllIntAndExtOrganizations()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAllIntAndExtOrganizations), Array.Empty<object>());
      try
      {
        return Organization.GetAllIntAndExtOrganizationInfo();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo[]) null;
      }
    }

    public virtual OrgInfo[] GetOrganizationsByName(string orgName)
    {
      this.onApiCalled(nameof (OrganizationManager), "GetOrganizationByName", new object[1]
      {
        (object) orgName
      });
      if (orgName == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("Org name cannot be null", nameof (orgName)));
      try
      {
        return OrganizationStore.GetOrganizationsByName(orgName);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo[]) null;
      }
    }

    public virtual OrgInfo GetOrganizationForClosingVendorInformation(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrganizationForClosingVendorInformation), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetOrganizationForClosingVendorInformation(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstAvaliableOrganization(int orgId)
    {
      return this.GetFirstAvaliableOrganization(orgId, false);
    }

    public virtual OrgInfo GetFirstAvaliableOrganization(int orgId, bool getInstalledInfoIfNotFound)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstAvaliableOrganization), new object[2]
      {
        (object) orgId,
        (object) getInstalledInfoIfNotFound
      });
      try
      {
        return OrganizationStore.GetFirstAvaliableOrganization(orgId, getInstalledInfoIfNotFound);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithNMLS(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithNMLS), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithNMLS(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithLOSearch(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithLOSearch), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithLOSearch(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithCCSiteId(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), "GetFirstCCSiteIdh", new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithCCSiteId(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithLEI(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithLEI), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithLEI(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithMERSMIN(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithMERSMIN), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithMERSMIN(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithLOComp(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithLOComp), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithLOComp(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationForSSO(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), "GetFirstOrganizationWithLOComp", new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationForSSO(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual bool GetParentOrganizationSSOSetting(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetParentOrganizationSSOSetting), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetParentOrganizationSsoSetting(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithStateLicensing(
      string companyName,
      string streetAddress,
      string loID)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithStateLicensing), new object[3]
      {
        (object) companyName,
        (object) streetAddress,
        (object) loID
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithStateLicensing(companyName, streetAddress, loID);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithStateLicensing(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithStateLicensing), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithStateLicensing(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual OrgInfo GetFirstOrganizationWithONRP(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetFirstOrganizationWithONRP), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.GetFirstOrganizationWithONRP(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (OrgInfo) null;
      }
    }

    public virtual int CreateOrganization(OrgInfo info)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to create an organization", "accessright"));
        return -1;
      }
      this.onApiCalled(nameof (OrganizationManager), nameof (CreateOrganization), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("Organization info cannot be null", nameof (info), this.Session.SessionInfo));
      if ((info.OrgName ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("Organization name cannot be blank or null", nameof (info), this.Session.SessionInfo));
      try
      {
        using (this.Session.Context.Cache.Lock(Organization.SyncRootKey))
        {
          using (Organization organization = OrganizationStore.CheckOut(info.Parent))
          {
            if (!organization.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid parent organization ID", ObjectType.Organization, (object) info.Parent));
            int suborganization = organization.CreateSuborganization(info);
            TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Created organization \"" + info.CompanyName + "\" with ID = " + (object) suborganization));
            return suborganization;
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return -1;
      }
    }

    public virtual void UpdateOrganization(OrgInfo info)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to update an organization", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (UpdateOrganization), new object[1]
        {
          (object) info
        });
        if (info == null)
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("OrgInfo cannot be null", nameof (info), this.Session.SessionInfo));
        try
        {
          using (Organization organization = OrganizationStore.CheckOut(info.Oid))
          {
            bool uncheckParentInfo = info.LOCompHistoryList != null && info.LOCompHistoryList.UncheckParentInfo;
            OrgInfo organizationInfo = organization.GetOrganizationInfo();
            if (!organization.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) info.Oid));
            organization.CheckIn(info);
            this.updateOrganizationChildrenPlans(info.LOCompHistoryList, info.Oid, uncheckParentInfo);
            OrganizationManager.updateOrganizationChildrenCCSite(info.CCSiteSettings, info.Oid);
            if (organizationInfo.MERSMINCode != info.MERSMINCode && info.MERSMINCode != "")
              MersNumberGenerator.SaveBranchMERSNumberingInfo(new BranchMERSMINNumberingInfo(info.MERSMINCode));
            if (info.SSOSettings != null)
              organization.UpdateSSOInheritedOrgsSSOSetting(info.Oid, info.SSOSettings.LoginAccess);
          }
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Updated organization \"" + info.CompanyName + "\" (" + (object) info.Oid + ")"));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    private void updateOrganizationChildrenPlans(
      LoanCompHistoryList parentHistoryList,
      int orgID,
      bool uncheckParentInfo)
    {
      TraceLog.WriteVerbose(nameof (OrganizationManager), "updateOrganizationChildrenPlans: Update child branch and users for LO Comp");
      bool flag = false;
      try
      {
        int[] ofOrgUsingLoComp = OrganizationStore.GetDescendentsOfOrgUsingLOComp(orgID);
        if (ofOrgUsingLoComp == null || ofOrgUsingLoComp.Length == 0)
          flag = true;
        int[] numArray;
        if (flag)
        {
          numArray = new int[1]{ orgID };
        }
        else
        {
          List<int> intList = new List<int>();
          intList.AddRange((IEnumerable<int>) ofOrgUsingLoComp);
          intList.Insert(0, orgID);
          numArray = intList.ToArray();
        }
        if (uncheckParentInfo)
        {
          for (int index1 = 0; index1 < numArray.Length; ++index1)
          {
            using (Organization organization = OrganizationStore.CheckOut(numArray[index1]))
            {
              OrgInfo organizationInfo = organization.GetOrganizationInfo();
              if (!organization.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) numArray[index1]));
              if (orgID != numArray[index1])
              {
                if (organizationInfo.LOCompHistoryList != null)
                {
                  if (!organizationInfo.LOCompHistoryList.UseParentInfo)
                    continue;
                }
                else
                  continue;
              }
              if (orgID != numArray[index1])
              {
                organizationInfo.LOCompHistoryList.UseParentInfo = false;
                organization.CheckIn(organizationInfo);
              }
              string[] idsInOrgForLoComp = User.GetUserIDsInOrgForLOComp(numArray[index1]);
              for (int index2 = 0; index2 < idsInOrgForLoComp.Length; ++index2)
              {
                using (User user = UserStore.CheckOut(idsInOrgForLoComp[index2]))
                {
                  if (!user.Exists)
                    Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid user ID", ObjectType.User, (object) idsInOrgForLoComp[index2]));
                  if (user.UserInfo.InheritParentCompPlan)
                  {
                    user.UserInfo.InheritParentCompPlan = false;
                    user.CheckIn(this.Session.UserID, false);
                  }
                }
              }
            }
          }
          if (flag)
            return;
          for (int index = 1; index < numArray.Length; ++index)
            this.updateOrganizationChildrenPlans((LoanCompHistoryList) null, numArray[index], uncheckParentInfo);
        }
        else
        {
          List<LoanCompHistory> currentAndFuturePlans = parentHistoryList?.GetCurrentAndFuturePlans(DateTime.Today.Date);
          for (int index3 = 0; index3 < numArray.Length; ++index3)
          {
            using (Organization organization = OrganizationStore.CheckOut(numArray[index3]))
            {
              OrgInfo organizationInfo = organization.GetOrganizationInfo();
              if (!organization.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) numArray[index3]));
              string[] idsInOrgForLoComp = User.GetUserIDsInOrgForLOComp(numArray[index3]);
              if (idsInOrgForLoComp != null && idsInOrgForLoComp.Length != 0)
              {
                for (int index4 = 0; index4 < idsInOrgForLoComp.Length; ++index4)
                {
                  LoanCompHistoryList planHistoryforUser = LOCompAccessor.GetComPlanHistoryforUser(idsInOrgForLoComp[index4], false, false);
                  planHistoryforUser.UseParentInfo = true;
                  planHistoryforUser.AddParentPlans(currentAndFuturePlans, DateTime.Today.Date);
                  LOCompAccessor.CreateHistoryCompPlans(planHistoryforUser, numArray[index3], idsInOrgForLoComp[index4], false, false);
                  using (User user = UserStore.CheckOut(idsInOrgForLoComp[index4]))
                  {
                    if (!user.Exists)
                      Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid user ID", ObjectType.User, (object) idsInOrgForLoComp[index4]));
                    user.UserInfo.InheritParentCompPlan = true;
                    user.CheckIn(this.Session.UserID, false);
                  }
                }
              }
              if (numArray[index3] != orgID)
              {
                if (organizationInfo.LOCompHistoryList != null)
                {
                  organizationInfo.LOCompHistoryList.SetID(numArray[index3].ToString());
                  organizationInfo.LOCompHistoryList.UseParentInfo = true;
                  organizationInfo.LOCompHistoryList.AddParentPlans(currentAndFuturePlans, DateTime.Today.Date);
                }
                LOCompAccessor.CreateHistoryCompPlans(organizationInfo.LOCompHistoryList, numArray[index3], (string) null, false, false);
                organization.CheckIn(organizationInfo);
                this.updateOrganizationChildrenPlans(organizationInfo.LOCompHistoryList, numArray[index3], uncheckParentInfo);
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex);
      }
    }

    private static void updateOrganizationChildrenCCSite(CCSiteInfo ccSiteInfo, int orgID)
    {
      if (ccSiteInfo == null)
        return;
      TraceLog.WriteVerbose(nameof (OrganizationManager), "updateOrganizationCCSite: Update child branch and users CCSite");
      bool flag = false;
      try
      {
        int[] usingCcSiteParenInfo = OrganizationStore.GetDescendentsOfOrgUsingCCSiteParenInfo(orgID);
        if (usingCcSiteParenInfo == null || usingCcSiteParenInfo.Length == 0)
          flag = true;
        int[] numArray;
        if (flag)
        {
          numArray = new int[1]{ orgID };
        }
        else
        {
          List<int> intList = new List<int>();
          intList.AddRange((IEnumerable<int>) usingCcSiteParenInfo);
          intList.Insert(0, orgID);
          numArray = intList.ToArray();
        }
        for (int index1 = 0; index1 < numArray.Length; ++index1)
        {
          using (Organization organization = OrganizationStore.CheckOut(numArray[index1]))
          {
            organization.GetOrganizationInfo();
            if (!organization.Exists)
              Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) numArray[index1]));
            if (orgID == numArray[index1])
            {
              string[] idsInOrgForCcSite = User.GetUserIDsInOrgForCCSite(numArray[index1]);
              CCSiteInfoAccessor.updateCCSiteId(ccSiteInfo.SiteId, numArray[index1], ccSiteInfo.Url);
              for (int index2 = 0; index2 < idsInOrgForCcSite.Length; ++index2)
              {
                using (User user = UserStore.CheckOut(idsInOrgForCcSite[index2]))
                {
                  if (!user.Exists)
                    Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid user ID", ObjectType.User, (object) idsInOrgForCcSite[index2]));
                  CCSiteInfoAccessor.updateUserCCSiteInfo(ccSiteInfo, idsInOrgForCcSite[index2]);
                }
              }
            }
            else
              CCSiteInfoAccessor.updateCCSiteId(ccSiteInfo.SiteId, numArray[index1], ccSiteInfo.Url);
          }
        }
        if (flag)
          return;
        for (int index = 1; index < numArray.Length; ++index)
          OrganizationManager.updateOrganizationChildrenCCSite(ccSiteInfo, numArray[index]);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex);
      }
    }

    public virtual void MoveOrganization(int orgId, int newParentId)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to move an organization", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (MoveOrganization), new object[2]
        {
          (object) orgId,
          (object) newParentId
        });
        try
        {
          using (this.Session.Context.Cache.Lock(Organization.SyncRootKey))
          {
            using (Organization organization = OrganizationStore.CheckOut(orgId))
            {
              if (!organization.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) orgId));
              using (Organization newOrg = OrganizationStore.CheckOut(newParentId))
              {
                if (!organization.Exists)
                  Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid parent organization ID", ObjectType.Organization, (object) newParentId));
                using (Organization priorOrg = OrganizationStore.CheckOut(organization.ParentOrganizationID))
                  organization.Move(priorOrg, newOrg);
                TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Moved organization \"" + organization.Name + "\" (" + (object) orgId + ") to new parent \"" + newOrg.Name + "\" (" + (object) newOrg.OrganizationID + ")"));
              }
            }
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void UpdateOrgSSOUsers(int orgId, bool loginAccess, bool applyToAll)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to move an organization", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (UpdateOrgSSOUsers), new object[1]
        {
          (object) orgId
        });
        try
        {
          List<string> list = ((IEnumerable<UserInfo>) this.GetOrganizationSSOUsers(orgId)).Select<UserInfo, string>((Func<UserInfo, string>) (ui => ui.Userid)).ToList<string>();
          if (list.Count <= 0)
            return;
          foreach (string userId in list)
            UserStore.RemoveCache(userId);
          User.UpdateUsersSSOSetting(orgId, loginAccess, applyToAll);
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("UpdateUsersSSOSetting to organization id: " + (object) orgId));
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void CreateSettingsRptJob(
      SettingsRptJobInfo.jobType jobType,
      string reportName,
      DateTime requestDate,
      Dictionary<string, string> reportParameters,
      List<string> reportFilters)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (CreateSettingsRptJob), new object[1]
      {
        (object) reportName
      });
      try
      {
        using (this.Session.Context.Cache.Lock(Organization.SyncRootKey))
        {
          string createdBy = this.Session.GetUserInfo().ToString() + " (" + this.Session.UserID + ")";
          string settingsRptJob = SettingsReportAccessor.CreateSettingsRptJob(new SettingsRptJobInfo(jobType, reportName, SettingsRptJobInfo.jobStatus.Submitted, createdBy, requestDate.ToString()));
          SettingsReportAccessor.saveReportParameters(settingsRptJob, reportParameters);
          SettingsReportAccessor.saveReportFilters(settingsRptJob, reportFilters, jobType);
          TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Created Job for \"" + reportName + "\" (" + settingsRptJob + ")"));
        }
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void DeleteOrganization(int orgId)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to delete an organization", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (DeleteOrganization), new object[1]
        {
          (object) orgId
        });
        try
        {
          using (this.Session.Context.Cache.Lock(Organization.SyncRootKey))
          {
            using (Organization organization = OrganizationStore.CheckOut(orgId))
            {
              if (!organization.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("Invaid organization ID", ObjectType.Organization, (object) orgId));
              if (organization.Children.Length != 0)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), new ServerException("Organization cannot be deleted because it contains suborganizations."));
              if (User.GetUsersInOrganization(orgId).Length != 0)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), new ServerException("Organization cannot be deleted because it contains users"));
              string name = organization.Name;
              using (Organization parentOrg = OrganizationStore.CheckOut(organization.ParentOrganizationID))
                organization.Delete(parentOrg);
              TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Deleted organization \"" + name + "\" (" + (object) orgId + ")"));
            }
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void MoveUsersIntoOrganization(
      string[] userIds,
      int orgId,
      List<string> coonectedUsers = null,
      bool enableSSO = false)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_OrganizationsUser))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to move user(s) into an organization", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (MoveUsersIntoOrganization), new object[2]
        {
          (object) userIds,
          (object) orgId
        });
        if (userIds == null)
          Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID list cannot be null", nameof (userIds), this.Session.SessionInfo));
        try
        {
          User.MoveUsersIntoOrganization(userIds, orgId, this.formatMsg(""), this.GetFirstOrganizationForSSO(orgId).SSOSettings.LoginAccess, enableSSO, coonectedUsers, this.Session.GetUserInfo().Userid);
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual void MoveUserIntoOrganization(string userId, int orgId)
    {
      this.MoveUsersIntoOrganization(new string[1]{ userId }, orgId, (List<string>) null, false);
    }

    public virtual PwdRuleValidator GetPasswordValidator()
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetPasswordValidator), Array.Empty<object>());
      try
      {
        return this.Session.Context.Settings.GetPasswordValidator();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (PwdRuleValidator) null;
      }
    }

    public virtual byte[] GetUserPasswordHash(string userId)
    {
      return this.GetUserPasswordHash(userId, false, false);
    }

    public virtual byte[] GetUserPasswordHash(
      string userId,
      bool isOrganizationUserSetting,
      bool isFromOldTable)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserPasswordHash), new object[1]
      {
        (object) userId
      });
      try
      {
        if (userId != this.Session.UserID && !isOrganizationUserSetting)
          this.Security.DemandAdministrator();
        using (User latestVersion = UserStore.GetLatestVersion(userId))
          return latestVersion.Exists ? latestVersion.GetPasswordHash(isFromOldTable) : throw new ObjectNotFoundException("Invalid User ID", ObjectType.User, (object) userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (byte[]) null;
      }
    }

    public virtual UserInfo[] GetAccessibleUsersWithPersona(int personaID, bool exactMatch)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAccessibleUsersWithPersona), new object[2]
      {
        (object) personaID,
        (object) exactMatch
      });
      try
      {
        UserInfo userById = User.GetUserById(this.Session.UserID);
        int orgId = userById.OrgId;
        bool inclusive = false;
        if (userById.IsSuperAdministrator())
          inclusive = true;
        return User.GetUsersWithPersona(personaID, orgId, exactMatch, inclusive);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfoSummary[] GetAccessibleUserInfoSummariesWithPersona(
      int personaID,
      bool exactMatch)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetAccessibleUserInfoSummariesWithPersona), new object[2]
      {
        (object) personaID,
        (object) exactMatch
      });
      try
      {
        UserInfo userById = User.GetUserById(this.Session.UserID);
        int orgId = userById.OrgId;
        bool inclusive = false;
        if (userById.IsSuperAdministrator())
          inclusive = true;
        return User.ConvertToUserInfoSummaries(User.GetUsersWithPersona(personaID, orgId, exactMatch, inclusive));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfoSummary[]) null;
      }
    }

    public virtual UserInfo[] GetUsersWithPersona(int personaID, bool exactMatch)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUsersWithPersona), new object[2]
      {
        (object) personaID,
        (object) exactMatch
      });
      try
      {
        return User.GetUsersWithPersona(personaID, -1, exactMatch, true);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfo[]) null;
      }
    }

    public virtual UserInfoSummary[] GetUserInfoSummariesWithPersona(int personaID, bool exactMatch)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserInfoSummariesWithPersona), new object[2]
      {
        (object) personaID,
        (object) exactMatch
      });
      try
      {
        return User.ConvertToUserInfoSummaries(User.GetUsersWithPersona(personaID, -1, exactMatch, true));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserInfoSummary[]) null;
      }
    }

    public virtual BinaryObject GetUserCustomDataObject(string userId, string name)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserCustomDataObject), new object[2]
      {
        (object) userId,
        (object) name
      });
      try
      {
        return BinaryObject.Marshal(User.GetCustomDataObjectFile(userId, name));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (BinaryObject) null;
      }
    }

    public virtual void SaveUserCustomDataObject(string userId, string name, BinaryObject data)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (SaveUserCustomDataObject), new object[3]
      {
        (object) userId,
        (object) name,
        (object) data
      });
      data?.Download();
      try
      {
        User.SaveCustomDataObjectFile(userId, name, data);
        data?.DisposeDeserialized();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void AppendToUserCustomDataObject(string userId, string name, BinaryObject data)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (AppendToUserCustomDataObject), new object[3]
      {
        (object) userId,
        (object) name,
        (object) data
      });
      if (data == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("Data value cannot be null", nameof (data), this.Session.SessionInfo));
      data.Download();
      try
      {
        User.AppendToCustomDataObjectFile(userId, name, data);
        data.DisposeDeserialized();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public virtual void UpdatePersonalStatusOnlineUsers(
      string[] enableUserList,
      string[] disableUserList)
    {
      if (!SecurityManagerUtil.HasFeatureAccess(this.Session.GetUserInfo(), (IFeaturesAclManager) this.Session.GetAclManager(AclCategory.Features), AclFeature.SettingsTab_CompanyStatusOnline))
      {
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User does not have the access right to enable/disable a user to use the Status Online Feature", "accessright"));
      }
      else
      {
        this.onApiCalled(nameof (OrganizationManager), nameof (UpdatePersonalStatusOnlineUsers), new object[2]
        {
          (object) enableUserList,
          (object) disableUserList
        });
        try
        {
          foreach (string enableUser in enableUserList)
          {
            using (User user = UserStore.CheckOut(enableUser, this.Session.UserID))
            {
              if (!user.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("User not found with ID \"" + enableUser + "\".", ObjectType.User, (object) enableUser));
              user.UserInfo.PersonalStatusOnline = true;
              user.CheckIn(this.Session.UserID, false);
            }
          }
          foreach (string disableUser in disableUserList)
          {
            using (User user = UserStore.CheckOut(disableUser, this.Session.UserID))
            {
              if (!user.Exists)
                Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ObjectNotFoundException("User not found with ID \"" + disableUser + "\".", ObjectType.User, (object) disableUser));
              user.UserInfo.PersonalStatusOnline = false;
              user.CheckIn(this.Session.UserID, false);
            }
          }
        }
        catch (Exception ex)
        {
          Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        }
      }
    }

    public virtual string GetOrgPath(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrgPath), new object[1]
      {
        (object) orgId
      });
      try
      {
        return Organization.GetOrgPath(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (string) null;
      }
    }

    public virtual UserProfileInfo GetUserProfile(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetUserProfile), new object[1]
      {
        (object) userId
      });
      if ((userId ?? "") == "")
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("User ID cannot be blank or null", nameof (userId), this.Session.SessionInfo));
      try
      {
        return UserStore.GetLatestVersion(userId).GetUserProfile();
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return (UserProfileInfo) null;
      }
    }

    public virtual void UpdateUserProfile(UserProfileInfo info, bool isEdit)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (UpdateUserProfile), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("UserProfileInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        User latestVersion = UserStore.GetLatestVersion(info.UserId);
        if (!isEdit)
          latestVersion.InsertUserProfile(info);
        else
          latestVersion.UpdateUserProfile(info);
        TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Updated User Profile for User  \"" + info.FullName + "\" (" + info.UserId + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public void UpsertUserProfile(UserProfileInfo info)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (UpsertUserProfile), new object[1]
      {
        (object) info
      });
      if (info == null)
        Err.Raise(TraceLevel.Warning, nameof (OrganizationManager), (ServerException) new ServerArgumentException("UserProfileInfo cannot be null", nameof (info), this.Session.SessionInfo));
      try
      {
        UserStore.GetLatestVersion(info.UserId).UpsertUserProfile(info);
        TraceLog.WriteInfo(nameof (OrganizationManager), this.formatMsg("Upserted User Profile for User  \"" + info.FullName + "\" (" + info.UserId + ")"));
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
      }
    }

    public bool OrganizationExists(int orgId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (OrganizationExists), new object[1]
      {
        (object) orgId
      });
      try
      {
        return OrganizationStore.OrganizationExists(orgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public bool IsMfaEnabledForExternalOrg(int extOrgId)
    {
      this.onApiCalled(nameof (OrganizationManager), "IsMfaEnabled", new object[1]
      {
        (object) extOrgId
      });
      try
      {
        return ExternalOrgManagementAccessor.IsMfaEnabledForExternalOrg(extOrgId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex, this.Session.SessionInfo);
        return false;
      }
    }

    public bool CheckIfLoanExistsForInternalUser(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (CheckIfLoanExistsForInternalUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return User.CheckIfLoanExistsForInternalUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex);
        return false;
      }
    }

    public bool CheckIfContactsExistsForInternalUser(string userId)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (CheckIfContactsExistsForInternalUser), new object[1]
      {
        (object) userId
      });
      try
      {
        return User.CheckIfContactsExistsForInternalUser(userId);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex);
        return false;
      }
    }

    public List<OrgHierarchyInfo> GetOrgHierarchy(int oid)
    {
      this.onApiCalled(nameof (OrganizationManager), nameof (GetOrgHierarchy), new object[1]
      {
        (object) oid
      });
      try
      {
        return Organization.GetOrgHierarchy(oid);
      }
      catch (Exception ex)
      {
        Err.Reraise(nameof (OrganizationManager), ex);
        return (List<OrgHierarchyInfo>) null;
      }
    }
  }
}
