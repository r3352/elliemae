// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Users
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

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
  /// <summary>
  /// Provides methods for accessing the Encompass users database.
  /// </summary>
  /// <example>
  /// The following code lists all of the currently defined users on the
  /// Encompass server along with their personas.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class LoanReader
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Fetch the list of all users
  ///       UserList users = session.Users.GetAllUsers();
  /// 
  ///       for (int i = 0; i < users.Count; i++)
  ///       {
  ///          Console.WriteLine("User " + users[i].ID + " has personas:");
  ///          foreach (Persona persona in users[i].Personas)
  ///             Console.WriteLine("\t" + persona.Name);
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
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

    /// <summary>Returns a list of all defined users.</summary>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.UserList">UserList</see>
    /// containing the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> objects.</returns>
    /// <example>
    /// The following code lists all of the currently defined users on the
    /// Encompass server along with their personas.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch the list of all users
    ///       UserList users = session.Users.GetAllUsers();
    /// 
    ///       for (int i = 0; i < users.Count; i++)
    ///       {
    ///          Console.WriteLine("User " + users[i].ID + " has personas:");
    ///          foreach (Persona persona in users[i].Personas)
    ///             Console.WriteLine("\t" + persona.Name);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public UserList GetAllUsers()
    {
      return User.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetAllUsers());
    }

    /// <summary>
    /// Returns a list of users who are assigned a specified Persona.
    /// </summary>
    /// <param name="persona">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona">Persona</see> of the desired users.</param>
    /// <param name="exactMatch">A flag to indicate if a user should be returned only
    /// if his assigned persona exactly matches the one specified.</param>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.UserList">UserList</see>
    /// containing the requested <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> obejcts.</returns>
    /// <example>
    /// The following code lists all of the currently defined Loan Officers.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lo = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList users = session.Users.GetUsersWithPersona(lo, false);
    /// 
    ///       for (int i = 0; i < users.Count; i++)
    ///       {
    ///          Console.WriteLine("User " + users[i].ID + " has personas:");
    ///          foreach (Persona persona in users[i].Personas)
    ///             Console.WriteLine("\t" + persona.Name);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public UserList GetUsersWithPersona(Persona persona, bool exactMatch)
    {
      if (persona == (Persona) null)
        throw new ArgumentNullException(nameof (persona));
      return User.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUsersWithPersona(persona.ID, exactMatch));
    }

    /// <summary>
    /// Retrieves the specified user from the Encompass server.
    /// </summary>
    /// <param name="userId">The Encompass Login ID for the desired user.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> object for the requested user, or
    /// null if no such user exists.</returns>
    /// <example>
    /// The following code lists opens the Loan Officer currently associated with
    /// a loan.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Loans;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class LoanReader
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Fetch the loan for the specified GUID
    ///       Loan loan = session.Loans.Open(args[0]);
    /// 
    ///       // Get the LOID field, which contains the User ID of the Loan
    ///       // Officer, if one has been assigned.
    ///       string loid = loan.Fields["LOID"].FormattedValue;
    /// 
    ///       if (loid != "")
    ///       {
    ///          // Retrieve the Loan Officer's user information
    ///          User lo = session.Users.GetUser(loid);
    ///          Console.WriteLine("The Loan Officer is " + lo.FirstName + " " + lo.LastName);
    ///       }
    ///       else
    ///       {
    ///          Console.WriteLine("No Loan Officer has been assigned to this loan.");
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public User GetUser(string userId)
    {
      ICache simpleCache = CacheManager.GetSimpleCache("UserInfoCache");
      string key = this.Session.SessionObjects.StartupInfo.ServerInstanceName + "@" + this.Session.SessionObjects.StartupInfo.ServerID + "_UserStore_" + userId;
      IOrganizationManager mngr = (IOrganizationManager) this.Session.GetObject("OrganizationManager");
      int num = (int) this.Session.SessionObjects.StartupInfo.PolicySettings[(object) "Policies.UsersClientCacheExpirationTime"];
      if (num < 0)
        num = 0;
      if (num > 28800)
        num = 28800;
      UserInfo info = simpleCache.Get<UserInfo>(key, (Func<UserInfo>) (() => mngr.GetUser(userId)), new CacheItemRetentionPolicy(TimeSpan.MinValue, TimeSpan.FromSeconds((double) num)));
      return info == (UserInfo) null ? (User) null : new User(this.Session, info, false);
    }

    /// <summary>
    /// Retrieves the specified user from the Encompass server.
    /// </summary>
    /// <param name="loginEmail">The login email for the desired user.</param>
    /// <param name="SiteID">The site ID for the desired user.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">ExternalUser</see> object for the requested user, or
    /// null if no such user exists.</returns>
    public ExternalUser GetExternalUserByEmailandSiteID(string loginEmail, string SiteID)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<ExternalUserInfo> info = configurationManager.GetExternalUserInfoFromEmailandURLID(loginEmail, SiteID);
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      if (info == null || info.Count == 0)
        return (ExternalUser) null;
      bool hasPerformancePatch = this.HasPerformancePatch();
      foreach (ExternalUserInfo externalUserInfo in info)
      {
        ExternalUserInfo user = externalUserInfo;
        if (!user.DisabledLogin)
        {
          List<ExternalOriginatorManagementData> companyOrganizations = configurationManager.GetCompanyOrganizations(user.ExternalOrgID);
          if (companyOrganizations.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == user.ExternalOrgID)) == null)
            return (ExternalUser) null;
          if (user.UpdatedByExternal && user.UpdatedBy != "")
            updatingExtUser = configurationManager.GetExternalUserInfoByContactId(user.UpdatedBy);
          return new ExternalUser(this.Session, user, updatingExtUser, companyOrganizations, false, false, hasPerformancePatch);
        }
      }
      List<ExternalOriginatorManagementData> companyOrganizations1 = configurationManager.GetCompanyOrganizations(info[0].ExternalOrgID);
      if (companyOrganizations1.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == info[0].ExternalOrgID)) == null)
        return (ExternalUser) null;
      if (info[0].UpdatedByExternal && info[0].UpdatedBy != "")
        updatingExtUser = configurationManager.GetExternalUserInfoByContactId(info[0].UpdatedBy);
      return new ExternalUser(this.Session, info[0], updatingExtUser, companyOrganizations1, false, false, hasPerformancePatch);
    }

    /// <summary>
    /// Retrieves the specified user from the Encompass server.
    /// </summary>
    /// <param name="externalUserID">The externaluserID for the desired user.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">ExternalUser</see> object for the requested user, or
    /// null if no such user exists.</returns>
    public ExternalUser GetExternalUserByExternalID(string externalUserID)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalUserInfo externalUserInfo = configurationManager.GetExternalUserInfo(externalUserID);
      if ((UserInfo) externalUserInfo == (UserInfo) null)
        return (ExternalUser) null;
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      List<ExternalOriginatorManagementData> companyOrganizations = configurationManager.GetCompanyOrganizations(externalUserInfo.ExternalOrgID);
      if (externalUserInfo.UpdatedByExternal && externalUserInfo.UpdatedBy != "")
        updatingExtUser = configurationManager.GetExternalUserInfoByContactId(externalUserInfo.UpdatedBy);
      return new ExternalUser(this.Session, externalUserInfo, updatingExtUser, companyOrganizations, false, false, this.HasPerformancePatch());
    }

    /// <summary>
    /// Retrieves the specified user from the Encompass server.
    /// </summary>
    /// <param name="contactID">The contactID for the desired user.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">ExternalUser</see> object for the requested user, or
    /// null if no such user exists.</returns>
    public ExternalUser GetExternalUserByContactID(string contactID)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalUserInfo userInfoByContactId = configurationManager.GetExternalUserInfoByContactId(contactID);
      if ((UserInfo) userInfoByContactId == (UserInfo) null)
        return (ExternalUser) null;
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      List<ExternalOriginatorManagementData> companyOrganizations = configurationManager.GetCompanyOrganizations(userInfoByContactId.ExternalOrgID);
      if (userInfoByContactId.UpdatedByExternal && userInfoByContactId.UpdatedBy != "")
        updatingExtUser = configurationManager.GetExternalUserInfoByContactId(userInfoByContactId.UpdatedBy);
      return new ExternalUser(this.Session, userInfoByContactId, updatingExtUser, companyOrganizations, false, false, this.HasPerformancePatch());
    }

    /// <summary>
    /// Retrieves the specified user from the Encompass server.
    /// </summary>
    /// <param name="loginEmail">The login email for the desired user.</param>
    /// <param name="password">The login password for the desired user.</param>
    /// <param name="siteID">The login siteID for the desired user.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">ExternalUser</see> object for the requested user, or
    /// null if no such user exists.</returns>
    public ExternalUser ValidateExternalUserBySiteID(
      string loginEmail,
      string password,
      string siteID)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<ExternalUserInfo> externalUserInfoList = configurationManager.ValidateExternalUserPasswordBySiteID(loginEmail, password, siteID);
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
          validationException.LoginAttempts = byEmailandSiteId.info.failed_login_attempts;
        }
        throw validationException;
      }
      bool hasPerformancePatch = this.HasPerformancePatch();
      foreach (ExternalUserInfo externalUserInfo in externalUserInfoList)
      {
        ExternalUserInfo user = externalUserInfo;
        if (!user.DisabledLogin)
        {
          Dictionary<string, List<ExternalOriginatorManagementData>> companyAncestors = configurationManager.GetCompanyAncestors(user.ExternalOrgID);
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
          user.LastLogin = DateTime.Now;
          user.failed_login_attempts = 0;
          configurationManager.UpdateExternalUserLastLogin(user);
          return externalUser;
        }
      }
      throw new ExternalUserValidationException(UserViolationType.AccountDisabled, "User account is disabled.");
    }

    private bool HasPerformancePatch()
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      try
      {
        configurationManager.GetExternalAdditionalDetails(123, new List<ExternalOriginatorOrgSetting>());
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

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.UserGroup">UserGroups</see> which are defined in the
    /// Encompass system.
    /// </summary>
    public UserGroups Groups => this.groups;

    /// <summary>
    /// Gets the collection of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona">Personas</see> which are defined in the
    /// Encompass system.
    /// </summary>
    public Personas Personas => this.personas;

    /// <summary>
    /// Gets data view for TPO. This method is for internal use only.
    /// </summary>
    /// <param name="aeUser">AE user</param>
    /// <param name="urlID">url id</param>
    /// <returns>Array of objects</returns>
    /// <remarks>This method is intended for internal use only and should not be called by external
    /// applications.</remarks>
    public ArrayList GetTPOWCAEView(User aeUser, int urlID)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetTPOWCAEView(aeUser.ID, aeUser.OrganizationID, urlID);
    }
  }
}
