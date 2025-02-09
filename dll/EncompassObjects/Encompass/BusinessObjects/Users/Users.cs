// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Users
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class Users : SessionBoundObject, IUsers
  {
    private UserGroups groups;
    private Personas personas;

    internal Users(Session session)
      : base(session)
    {
      this.groups = new UserGroups(session);
      this.personas = new Personas(session);
    }

    public UserList GetAllUsers()
    {
      return User.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetAllUsers());
    }

    public UserList GetUsersWithPersona(Persona persona, bool exactMatch)
    {
      if (persona == (Persona) null)
        throw new ArgumentNullException(nameof (persona));
      return User.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUsersWithPersona(persona.ID, exactMatch));
    }

    public User GetUser(string userId)
    {
      ICache simpleCache = CacheManager.GetSimpleCache("UserInfoCache");
      string str = this.Session.SessionObjects.StartupInfo.ServerInstanceName + "@" + this.Session.SessionObjects.StartupInfo.ServerID + "_UserStore_" + userId;
      IOrganizationManager mngr = (IOrganizationManager) this.Session.GetObject("OrganizationManager");
      int num = (int) this.Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.UsersClientCacheExpirationTime"];
      if (num < 0)
        num = 0;
      if (num > 28800)
        num = 28800;
      UserInfo info = simpleCache.Get<UserInfo>(str, (Func<UserInfo>) (() => mngr.GetUser(userId)), new CacheItemRetentionPolicy(TimeSpan.MinValue, TimeSpan.FromSeconds((double) num)));
      return UserInfo.op_Equality(info, (UserInfo) null) ? (User) null : new User(this.Session, info, false);
    }

    public ExternalUser GetExternalUserByEmailandSiteID(string loginEmail, string SiteID)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<ExternalUserInfo> info = iconfigurationManager.GetExternalUserInfoFromEmailandURLID(loginEmail, SiteID);
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      if (info == null || info.Count == 0)
        return (ExternalUser) null;
      bool hasPerformancePatch = this.HasPerformancePatch();
      foreach (ExternalUserInfo externalUserInfo in info)
      {
        ExternalUserInfo user = externalUserInfo;
        if (!user.DisabledLogin)
        {
          List<ExternalOriginatorManagementData> companyOrganizations = iconfigurationManager.GetCompanyOrganizations(user.ExternalOrgID);
          if (companyOrganizations.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == user.ExternalOrgID)) == null)
            return (ExternalUser) null;
          if (user.UpdatedByExternal && user.UpdatedBy != "")
            updatingExtUser = iconfigurationManager.GetExternalUserInfoByContactId(user.UpdatedBy);
          return new ExternalUser(this.Session, user, updatingExtUser, companyOrganizations, false, false, hasPerformancePatch);
        }
      }
      List<ExternalOriginatorManagementData> companyOrganizations1 = iconfigurationManager.GetCompanyOrganizations(info[0].ExternalOrgID);
      if (companyOrganizations1.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == info[0].ExternalOrgID)) == null)
        return (ExternalUser) null;
      if (info[0].UpdatedByExternal && info[0].UpdatedBy != "")
        updatingExtUser = iconfigurationManager.GetExternalUserInfoByContactId(info[0].UpdatedBy);
      return new ExternalUser(this.Session, info[0], updatingExtUser, companyOrganizations1, false, false, hasPerformancePatch);
    }

    public ExternalUser GetExternalUserByExternalID(string externalUserID)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalUserInfo externalUserInfo = iconfigurationManager.GetExternalUserInfo(externalUserID);
      if (UserInfo.op_Equality((UserInfo) externalUserInfo, (UserInfo) null))
        return (ExternalUser) null;
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      List<ExternalOriginatorManagementData> companyOrganizations = iconfigurationManager.GetCompanyOrganizations(externalUserInfo.ExternalOrgID);
      if (externalUserInfo.UpdatedByExternal && externalUserInfo.UpdatedBy != "")
        updatingExtUser = iconfigurationManager.GetExternalUserInfoByContactId(externalUserInfo.UpdatedBy);
      return new ExternalUser(this.Session, externalUserInfo, updatingExtUser, companyOrganizations, false, false, this.HasPerformancePatch());
    }

    public ExternalUser GetExternalUserByContactID(string contactID)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalUserInfo userInfoByContactId = iconfigurationManager.GetExternalUserInfoByContactId(contactID);
      if (UserInfo.op_Equality((UserInfo) userInfoByContactId, (UserInfo) null))
        return (ExternalUser) null;
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      List<ExternalOriginatorManagementData> companyOrganizations = iconfigurationManager.GetCompanyOrganizations(userInfoByContactId.ExternalOrgID);
      if (userInfoByContactId.UpdatedByExternal && userInfoByContactId.UpdatedBy != "")
        updatingExtUser = iconfigurationManager.GetExternalUserInfoByContactId(userInfoByContactId.UpdatedBy);
      return new ExternalUser(this.Session, userInfoByContactId, updatingExtUser, companyOrganizations, false, false, this.HasPerformancePatch());
    }

    public ExternalUser ValidateExternalUserBySiteID(
      string loginEmail,
      string password,
      string siteID)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<ExternalUserInfo> externalUserInfoList = iconfigurationManager.ValidateExternalUserPasswordBySiteID(loginEmail, password, siteID);
      if (externalUserInfoList == null || externalUserInfoList.Count == 0)
      {
        ExternalUserValidationException validationException = new ExternalUserValidationException(UserViolationType.LoginCredentialNotFound, "Invalid combination of login credential.");
        ExternalUser byEmailandSiteId = this.GetExternalUserByEmailandSiteID(loginEmail, siteID);
        if (byEmailandSiteId == null)
        {
          validationException.UserInfo = (ExternalUserInfo) null;
          validationException.LoginAttempts = 0;
        }
        else
        {
          validationException.UserInfo = byEmailandSiteId.info;
          validationException.LoginAttempts = ((UserInfo) byEmailandSiteId.info).failed_login_attempts;
        }
        throw validationException;
      }
      bool hasPerformancePatch = this.HasPerformancePatch();
      foreach (ExternalUserInfo externalUserInfo in externalUserInfoList)
      {
        ExternalUserInfo user = externalUserInfo;
        if (!user.DisabledLogin)
        {
          Dictionary<string, List<ExternalOriginatorManagementData>> companyAncestors = iconfigurationManager.GetCompanyAncestors(user.ExternalOrgID);
          List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
          List<ExternalOriginatorManagementData> list = new List<ExternalOriginatorManagementData>();
          companyAncestors.TryGetValue("CompanyOrgs", out originatorManagementDataList);
          companyAncestors.TryGetValue("CompanyAncestors", out list);
          if (originatorManagementDataList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == user.ExternalOrgID)) == null)
            return (ExternalUser) null;
          ExternalUser externalUser = new ExternalUser(this.Session, user, (ExternalUserInfo) null, originatorManagementDataList, false, false, hasPerformancePatch);
          if (this.isOrganizationDisabled(list))
            throw new ExternalUserValidationException(UserViolationType.OrganizationDisabled, "Organization is disabled.");
          if (!externalUser.Enabled)
            throw new ExternalUserValidationException(UserViolationType.AccountDisabled, "User account is disabled.");
          externalUser.LastLogin = DateTime.Now;
          ((UserInfo) user).LastLogin = DateTime.Now;
          ((UserInfo) user).failed_login_attempts = 0;
          iconfigurationManager.UpdateExternalUserLastLogin(user);
          return externalUser;
        }
      }
      throw new ExternalUserValidationException(UserViolationType.AccountDisabled, "User account is disabled.");
    }

    private bool HasPerformancePatch()
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      try
      {
        iconfigurationManager.GetExternalAdditionalDetails(123, new List<ExternalOriginatorOrgSetting>());
      }
      catch
      {
        return false;
      }
      return true;
    }

    private bool isOrganizationDisabled(List<ExternalOriginatorManagementData> list)
    {
      foreach (ExternalOriginatorManagementData originatorManagementData in list)
      {
        if (originatorManagementData.DisabledLogin)
          return true;
      }
      return false;
    }

    public UserGroups Groups => this.groups;

    public Personas Personas => this.personas;

    public ArrayList GetTPOWCAEView(User aeUser, int urlID)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetTPOWCAEView(aeUser.ID, aeUser.OrganizationID, urlID);
    }
  }
}
