// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.Organizations
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>
  /// Provides methods for accessing the Encompass organization hierarchy.
  /// </summary>
  /// <remarks>The properties and methods of this class are restricted to users
  /// who have the Administrator persona.</remarks>
  /// <example>
  /// The following code demonstrates how to display the entire organization
  /// hierarchy.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Users;
  /// 
  /// class UserManager
  /// {
  ///    public static void Main()
  ///    {
  ///       // Open the session to the remote server. We will need to be logged
  ///       // in as an Administrator to modify the user accounts.
  ///       Session session = new Session();
  ///       session.Start("myserver", "admin", "adminpwd");
  /// 
  ///       // Fetch the root organization
  ///       Organization root = session.Organizations.GetTopMostOrganization();
  /// 
  ///       // Recursively display this organization and its children
  ///       displayOrgHierarchy(root, 0);
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// 
  ///    // Used to recursively generate the organization hierarchy
  ///    private static void displayOrgHierarchy(Organization parentOrg, int depth)
  ///    {
  ///       // Write out the parent organization's name
  ///       Console.WriteLine(new string('-', depth * 2) + parentOrg.Name);
  /// 
  ///       // Iterate over the organization's children, recursively calling this function
  ///       foreach (Organization suborg in parentOrg.GetSuborganizations())
  ///          displayOrgHierarchy(suborg, depth + 1);
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class Organizations : SessionBoundObject, IOrganizations
  {
    internal Organizations(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Returns the top-most organization in the organization hierarchy.
    /// </summary>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization" /> object representing the top
    /// organization in the hierarchy.</returns>
    /// <example>
    /// The following code demonstrates how to display the entire organization
    /// hierarchy.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the root organization
    ///       Organization root = session.Organizations.GetTopMostOrganization();
    /// 
    ///       // Recursively display this organization and its children
    ///       displayOrgHierarchy(root, 0);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// 
    ///    // Used to recursively generate the organization hierarchy
    ///    private static void displayOrgHierarchy(Organization parentOrg, int depth)
    ///    {
    ///       // Write out the parent organization's name
    ///       Console.WriteLine(new string('-', depth * 2) + parentOrg.Name);
    /// 
    ///       // Iterate over the organization's children, recursively calling this function
    ///       foreach (Organization suborg in parentOrg.GetSuborganizations())
    ///          displayOrgHierarchy(suborg, depth + 1);
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Organization GetTopMostOrganization()
    {
      return new Organization(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetRootOrganization());
    }

    /// <summary>
    /// Returns the top-most organization in the organization hierarchy.
    /// </summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrg</see></returns>
    internal ExternalOrganizationList GetTopMostExternalOrganizations()
    {
      return EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.ToList(this.Session, ((IEnumerable<ExternalOriginatorManagementData>) ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllExternalOrganizations(false).ToArray()).TakeWhile<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.Parent == 0)).ToArray<ExternalOriginatorManagementData>(), this.HasPerformancePatch());
    }

    /// <summary>Returns a list of all defined Organizations.</summary>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.OrganizationList">OrganizationList</see>
    /// containing all of the <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization">Organization</see> objects
    /// defined on the Encompass Server.</returns>
    /// <example>
    /// The following code generates a complete roster of the users defined in
    /// Encompass, broken down by the organization in which they belong.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch all of the organizations
    ///       OrganizationList orgs = session.Organizations.GetAllOrganizations();
    /// 
    ///       foreach (Organization org in orgs)
    ///       {
    ///          // Display the organization name
    ///          Console.WriteLine(org.Name);
    /// 
    ///          // Display the name of each user in the organization
    ///          foreach (User user in org.GetUsers())
    ///             Console.WriteLine("+ " + user.ToString());
    /// 
    ///          Console.WriteLine();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public OrganizationList GetAllOrganizations()
    {
      return Organization.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetAllOrganizations());
    }

    /// <summary>
    /// Returns a list of all defined External Organizations - could have performance issue with large number of organizations
    /// </summary>
    /// <returns>A <see cref="T:EllieMae.Encompass.Collections.ExternalOrganizationList">ExternalOrganizationList</see>
    /// containing all of the <see cref="T:EllieMae.EMLite.ClientServer.ExternalOriginatorManagementData">ExternalOriginatorManagementData</see> objects
    /// defined on the Encompass Server.</returns>
    /// <example>
    /// The following code generates a complete roster of the users defined in
    /// Encompass, broken down by the external organization in which they belong.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch all of the organizations
    ///       ExternalOrganizationList orgs = session.Organizations.GetAllExternalOrganizations();
    /// 
    ///       foreach (ExternalOrganization org in orgs)
    ///       {
    ///         // Display the organization name
    ///         Console.WriteLine(org.CompanyLegalName);
    /// 
    ///         // Display the name of each user in the organization
    ///         foreach (ExternalUser user in org.GetUsers())
    ///           Console.WriteLine("+ " + user.ToString());
    /// 
    ///         Console.WriteLine();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalOrganizationList GetAllExternalOrganizations()
    {
      return EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.ToList(this.Session, ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllExternalOrganizations(false).ToArray(), this.HasPerformancePatch());
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

    /// <summary>
    /// Returns a list of organization whose names match the specified value.
    /// </summary>
    /// <param name="orgName">The name of the organization(s) to be found.</param>
    /// <returns>Returns a list of matching organizations. A organization's name must
    /// match the specified value exactly to be returned.</returns>
    /// <example>
    /// The following code enables all of the user accounts in the
    /// "Loan Officers" organization.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Fetch the "Loan Officers" organization(s)
    ///       OrganizationList orgs = session.Organizations.GetOrganizationsByName("Loan Officers");
    /// 
    ///       foreach (Organization org in orgs)
    ///       {
    ///          // Fetch all of the users from the organization
    ///          UserList users = org.GetUsers();
    /// 
    ///          // For each LO, move the user into the new organization
    ///          foreach (User user in users)
    ///          {
    ///             if (user.Enabled == false)
    ///                user.Enable();
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public OrganizationList GetOrganizationsByName(string orgName)
    {
      return Organization.ToList(this.Session, ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetOrganizationsByName(orgName));
    }

    /// <summary>
    /// Retrieves the specified Organization from the Encompass server.
    /// </summary>
    /// <param name="orgId">The unique ID for the desired Organization.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Organization">Organization</see> object for the requested Organization, or
    /// null if no such Organization exists.</returns>
    /// <example>
    /// The following code displays a roster of all Loan Officers in the system
    /// along with the organizations to which they belong.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// 
    /// class UserManager
    /// {
    ///    public static void Main()
    ///    {
    ///       // Open the session to the remote server. We will need to be logged
    ///       // in as an Administrator to modify the user accounts.
    ///       Session session = new Session();
    ///       session.Start("myserver", "admin", "adminpwd");
    /// 
    ///       // Get the "Loan Officer" Persona
    ///       Persona lop = session.Users.Personas.GetPersonaByName("Loan Officer");
    /// 
    ///       // Fetch the list of Loan Officers from the server
    ///       UserList los = session.Users.GetUsersWithPersona(lop, false);
    /// 
    ///       foreach (User lo in los)
    ///       {
    ///          // Fetch the user's organization
    ///          Organization org = session.Organizations.GetOrganization(lo.OrganizationID);
    /// 
    ///          // Display the user's name and organization
    ///          Console.WriteLine(lo + ", " + org);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Organization GetOrganization(int orgId)
    {
      OrgInfo organization = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetOrganization(orgId);
      return organization == null ? (Organization) null : new Organization(this.Session, organization);
    }

    /// <summary>
    /// Retrieves the specified External Organization without details from the Encompass server.
    /// </summary>
    /// <param name="orgId">The unique ID for the desired External Organization.</param>
    /// <returns>The <see cref="N:EllieMae.Encompass.BusinessObjects.ExternalOrganization">ExternalOrganization</see> object for the requested External Organization, or
    /// null if no such External Organization exists.</returns>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId)
    {
      return this.GetExternalOrganization(orgId, false);
    }

    /// <summary>
    /// Retrieves the specified External Organization with all details from the Encompass server.
    /// </summary>
    /// <param name="orgId">The unique ID for the desired External Organization.</param>
    /// <param name="getAllDetails" />
    /// <returns>The <see cref="N:EllieMae.Encompass.BusinessObjects.ExternalOrganization">ExternalOrganization</see> object for the requested External Organization, or
    /// null if no such External Organization exists.</returns>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      int orgId,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(orgId);
      return companyOrganizations.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgId)) == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, orgId, getAllDetails, this.HasPerformancePatch());
    }

    /// <summary>
    /// Retrieves the specified External Organization without details from the Encompass server.
    /// </summary>
    /// <param name="externalID">The externalID of the organization.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrg</see> object for the corresponding externalID.</returns>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID)
    {
      return this.GetExternalOrganization(externalID, false);
    }

    /// <summary>Returns the list of Price Groups</summary>
    /// <returns></returns>
    public List<PriceGroup> GetPriceGroups()
    {
      Dictionary<string, List<ExternalSettingValue>> externalOrgSettings = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettings();
      if (!externalOrgSettings.ContainsKey("Price Group") || externalOrgSettings["Price Group"] == null)
        externalOrgSettings["Price Group"] = new List<ExternalSettingValue>();
      List<PriceGroup> result = new List<PriceGroup>();
      externalOrgSettings["Price Group"].ForEach((Action<ExternalSettingValue>) (x => result.Add(new PriceGroup(this.Session, x.settingValue, x.settingCode, x.settingId))));
      return result;
    }

    /// <summary>
    /// Retrieves the specified External Organizations for a given externalID without details from the Encompass server.
    /// </summary>
    /// <param name="externalID">The externalID of the organization.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrg</see> list object for the corresponding externalID.</returns>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID)
    {
      return this.GetExternalOrganizationsWithExtension(externalID, false);
    }

    /// <summary>
    /// Retrieves the specified External Organization with all details from the Encompass server.
    /// </summary>
    /// <param name="externalID">External ID of the External Organization</param>
    /// <param name="getAllDetails">Flag to indicate whether detail information of the ExternalOrg will be populated by default.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrg</see> object for the corresponding externalID.</returns>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization GetExternalOrganization(
      string externalID,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(externalID);
      ExternalOriginatorManagementData originatorManagementData1 = (ExternalOriginatorManagementData) null;
      foreach (ExternalOriginatorManagementData originatorManagementData2 in companyOrganizations.ToArray())
      {
        string str = originatorManagementData2.ExternalID;
        if (originatorManagementData2.ExternalID.Length > 10)
          str = originatorManagementData2.ExternalID.Substring(1);
        if (str == externalID && (originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Company || originatorManagementData2.OrganizationType == ExternalOriginatorOrgType.Branch))
        {
          originatorManagementData1 = originatorManagementData2;
          break;
        }
      }
      return originatorManagementData1 == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, originatorManagementData1.oid, getAllDetails, this.HasPerformancePatch());
    }

    /// <summary>
    /// Retrieves the specified External Organizations for a given externalID with all details from the Encompass server.
    /// </summary>
    /// <param name="externalID">External ID of the External Organization</param>
    /// <param name="getAllDetails">Flag to indicate whether detail information of the ExternalOrg will be populated by default.</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrg</see> list object for the corresponding externalID.</returns>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExternalOrganizationsWithExtension(
      string externalID,
      bool getAllDetails)
    {
      List<ExternalOriginatorManagementData> companyOrganizations = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCompanyOrganizations(externalID);
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> organizationsWithExtension = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      foreach (ExternalOriginatorManagementData originatorManagementData in companyOrganizations.ToArray())
      {
        string str = originatorManagementData.ExternalID;
        if (originatorManagementData.ExternalID.Length > 10)
          str = originatorManagementData.ExternalID.Substring(1);
        if (str == externalID)
          organizationsWithExtension.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, companyOrganizations, originatorManagementData.oid, getAllDetails, this.HasPerformancePatch()));
      }
      return organizationsWithExtension;
    }

    internal List<object> GetExternalAdditionalDetails(int oid)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalAdditionalDetails(oid);
    }

    internal List<object> GetExternalAdditionalDetails(string externalUserID)
    {
      return ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalAdditionalDetails(externalUserID);
    }

    /// <summary>
    /// Retrieves organizations and users having aeUserId assigned as sales reps
    /// </summary>
    /// <param name="aeUserId">Encompass userid</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.AEExternalAccessibleEntity">AEExternalAccessibleEntity</see> object that contains the accessible organizations and users.</returns>
    public AEExternalAccessibleEntity GetAccessibleExternalOrganizations(string aeUserId)
    {
      IConfigurationManager mngr = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      mngr.GetAllExternalOrganizations(false);
      List<ExternalOriginatorManagementData> organizationBySalesRep = mngr.GetExternalOrganizationBySalesRep(aeUserId);
      List<ExternalUserInfo> userInfoBySalesRep = mngr.GetExternalUserInfoBySalesRep(aeUserId);
      List<string> companyList = new List<string>();
      Dictionary<string, string[]> branchList = new Dictionary<string, string[]>();
      List<string> userList = new List<string>();
      organizationBySalesRep.ForEach((Action<ExternalOriginatorManagementData>) (x =>
      {
        if (x.OrganizationType == ExternalOriginatorOrgType.Company && !companyList.Contains(x.ExternalID))
        {
          companyList.Add(x.ExternalID);
        }
        else
        {
          if (branchList.ContainsKey(x.ExternalID))
            return;
          List<string> organizationDesendentsTpoid = mngr.GetExternalOrganizationDesendentsTPOID(x.oid);
          if (organizationDesendentsTpoid.Count > 0)
            branchList.Add(x.ExternalID, organizationDesendentsTpoid.ToArray());
          else
            branchList.Add(x.ExternalID, new string[0]);
        }
      }));
      userInfoBySalesRep.ForEach((Action<ExternalUserInfo>) (x => userList.Add(x.ExternalUserID)));
      return new AEExternalAccessibleEntity(companyList, branchList, userList);
    }

    /// <summary>Retrieves visible organizations and users</summary>
    /// <param name="aeUserId">Encompass userid</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.AEExternalAccessibleEntity">AEExternalAccessibleEntity</see> object that contains the accessible organizations and users.</returns>
    public AEExternalAccessibleEntity GetVisibleExternalOrganizations(string aeUserId)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      UserInfo user = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUser(aeUserId);
      ArrayList andOrgBySalesRep = configurationManager.GetExternalAndInternalUserAndOrgBySalesRep(user.Userid, user.OrgId);
      List<string> companyList = new List<string>();
      Dictionary<string, string[]> accessibleBranchies = new Dictionary<string, string[]>();
      List<string> stringList = new List<string>();
      ((List<ExternalOriginatorManagementData>) andOrgBySalesRep[1]).ForEach((Action<ExternalOriginatorManagementData>) (x => companyList.Add(x.ExternalID)));
      List<string> accessibleContacts = (List<string>) andOrgBySalesRep[4];
      return new AEExternalAccessibleEntity(companyList, accessibleBranchies, accessibleContacts);
    }

    internal static ExternalOriginatorManagementData GetExternalCompany(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      return orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.Depth == 1));
    }

    internal static ExternalOriginatorManagementData GetExternalBranch(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      ExternalOriginatorManagementData company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgID));
      if (company.Depth < 2)
        return (ExternalOriginatorManagementData) null;
      if (company.OrganizationType == ExternalOriginatorOrgType.CompanyExtension)
        return (ExternalOriginatorManagementData) null;
      if (company.OrganizationType == ExternalOriginatorOrgType.Branch)
        return company;
      while (company != null && company.Parent > 0)
      {
        company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == company.Parent));
        if (company.OrganizationType == ExternalOriginatorOrgType.Branch)
          break;
      }
      return company;
    }

    /// <summary>Method to add site URL</summary>
    /// <param name="url">The url to add</param>
    /// <param name="siteId">The corresponding siteid for the url</param>
    public void AddSiteUrl(string url, string siteId)
    {
      if (siteId == null || siteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      if (url == null || url == string.Empty)
        throw new Exception("URL cannot be null or empty.");
      if (!SystemUtil.IsValidURL(url))
        throw new Exception("Not a valid URL.");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgURL[] organizationUrLs = configurationManager.GetExternalOrganizationURLs();
      ExternalOrgURL externalOrgUrl1 = ((IEnumerable<ExternalOrgURL>) organizationUrLs).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.siteId == siteId));
      ExternalOrgURL externalOrgUrl2 = ((IEnumerable<ExternalOrgURL>) organizationUrLs).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == url));
      if (externalOrgUrl1 != null)
        throw new Exception("Site Id already exists.");
      if (externalOrgUrl2 != null)
        throw new Exception("URL already exists.");
      configurationManager.AddExternalOrganizationURL(siteId, url);
    }

    /// <summary>Method to delete url</summary>
    /// <param name="url">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrl">ExternalSiteUrl</see> object to delete.</param>
    public void DeleteSiteUrl(ExternalSiteUrl url)
    {
      if (url == null || url.SiteId == null || url.SiteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).DeleteExternalOrganizationURL(url.SiteId);
    }

    /// <summary>Method to update url</summary>
    /// <param name="url">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrl">ExternalSiteUrl</see> object to update.</param>
    public void UpdateSiteUrl(ExternalSiteUrl url)
    {
      if (url == null || url.SiteId == null || url.SiteId == string.Empty)
        throw new Exception("Site Id cannot be null or empty.");
      if (url.URL == null || url.URL == string.Empty)
        throw new Exception("URL cannot be null or empty.");
      if (!SystemUtil.IsValidURL(url.URL))
        throw new Exception("Not a valid URL.");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgURL externalOrgUrl = ((IEnumerable<ExternalOrgURL>) configurationManager.GetExternalOrganizationURLs()).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (a => a.URL == url.URL));
      if (externalOrgUrl != null && externalOrgUrl.siteId != url.SiteId)
        throw new Exception("URL already exists.");
      configurationManager.UpdateExternalOrganizationURL(new ExternalOrgURL()
      {
        siteId = url.SiteId,
        URL = url.URL
      });
    }

    /// <summary>Retrieves all site urls</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSiteUrl">ExternalSiteUrl</see></returns>
    public List<ExternalSiteUrl> GetSiteUrls()
    {
      ExternalOrgURL[] organizationUrLs = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrganizationURLs();
      List<ExternalSiteUrl> siteUrls = new List<ExternalSiteUrl>();
      if (organizationUrLs != null)
      {
        foreach (ExternalOrgURL externalOrgUrl in organizationUrLs)
        {
          if (!externalOrgUrl.isDeleted)
            siteUrls.Add(new ExternalSiteUrl(externalOrgUrl));
        }
      }
      return siteUrls;
    }

    /// <summary>Archive documents</summary>
    /// <param name="settingObjs"></param>
    public void ArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> guids = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
        guids.Add(settingObj.Guid.ToString());
      configurationManager.ArchiveDocuments(-1, guids);
    }

    /// <summary>Deletes documents</summary>
    /// <param name="settingObjs"></param>
    public void DeleteExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> stringList = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
      {
        FileSystemEntry entry = new FileSystemEntry("\\\\" + (settingObj.Guid.ToString() + "." + ((IEnumerable<string>) settingObj.FileName.Split('.')).Last<string>()), FileSystemEntry.Types.File, (string) null);
        configurationManager.DeleteDocument(-1, settingObj.Guid, entry);
      }
    }

    /// <summary>Unarchive the external documents</summary>
    /// <param name="settingObjs"></param>
    public void UnArchiveExternalDocumentSetting(List<ExternalDocumentsSettings> settingObjs)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (settingObjs == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      List<string> guids = new List<string>();
      foreach (ExternalDocumentsSettings settingObj in settingObjs)
        guids.Add(settingObj.Guid.ToString());
      configurationManager.UnArchiveDocuments(-1, guids);
    }

    /// <summary>Gets All archived documents</summary>
    /// <returns></returns>
    public List<ExternalDocumentsSettings> GetAllArchivedDocuments()
    {
      List<DocumentSettingInfo> archiveDocuments = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetAllArchiveDocuments(-1);
      List<ExternalDocumentsSettings> archivedDocuments = new List<ExternalDocumentsSettings>();
      foreach (DocumentSettingInfo docSetting in archiveDocuments)
        archivedDocuments.Add(new ExternalDocumentsSettings(docSetting));
      return archivedDocuments;
    }

    /// <summary>Gets Non Archived documents</summary>
    /// <returns></returns>
    public List<ExternalDocumentsSettings> GetUnArchivedDocuments()
    {
      List<DocumentSettingInfo> externalDocuments = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalDocuments(-1, -1, -1);
      List<ExternalDocumentsSettings> archivedDocuments = new List<ExternalDocumentsSettings>();
      foreach (DocumentSettingInfo docSetting in externalDocuments)
        archivedDocuments.Add(new ExternalDocumentsSettings(docSetting));
      return archivedDocuments;
    }

    /// <summary>Reetrieve Document</summary>
    /// <param name="docObject"></param>
    /// <returns></returns>
    public DataObject RetrieveDocument(ExternalDocumentsSettings docObject)
    {
      if (docObject == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      try
      {
        IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
        // ISSUE: variable of a boxed type
        __Boxed<Guid> guid = (ValueType) docObject.Guid;
        string str = ((IEnumerable<string>) docObject.FileName.Split('.')).Last<string>();
        string fileName = guid.ToString() + "." + str;
        ((IEnumerable<string>) docObject.FileName.Split('\\')).Last<string>();
        return new DataObject(configurationManager.ReadDocumentFromDataFolder(fileName));
      }
      catch (Exception ex)
      {
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentDoesNotExist, "Document not found.");
      }
    }

    /// <summary>Edit the properties of the uploaded document</summary>
    /// <param name="docObject"></param>
    public void EditDocument(ExternalDocumentsSettings docObject)
    {
      if (docObject == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Null input argument");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      List<DocumentSettingInfo> archiveDocuments = configurationManager.GetAllArchiveDocuments(-1);
      List<DocumentSettingInfo> externalDocuments = configurationManager.GetExternalDocuments(-1, -1, -1);
      if (archiveDocuments.Find((Predicate<DocumentSettingInfo>) (a => a.Guid == docObject.Guid)) == null && externalDocuments.Find((Predicate<DocumentSettingInfo>) (a => a.Guid == docObject.Guid)) == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentDoesNotExist, "Document does not exists");
      DocumentSettingInfo document = new DocumentSettingInfo();
      document.Active = docObject.Active;
      document.Guid = docObject.Guid;
      document.FileName = docObject.FileName;
      document.DisplayName = docObject.DisplayName;
      document.StartDate = docObject.StartDate;
      document.EndDate = docObject.EndDate;
      document.Category = docObject.Category;
      document.Channel = (ExternalOriginatorEntityType) Convert.ToInt32((object) docObject.Channel);
      document.DateAdded = docObject.DateAdded;
      document.AvailbleAllTPO = docObject.AvailbleAllTPO;
      document.AddedBy = docObject.AddedBy;
      document.Status = (ExternalOriginatorStatus) Convert.ToInt32((object) docObject.Status);
      document.IsArchive = docObject.IsArchive;
      configurationManager.UpdateDocument(-1, document);
      if (document.AvailbleAllTPO)
        configurationManager.AssignDefaultDocumentToAll(document);
      else
        configurationManager.RemoveDefaultDocumentFromAll(document);
    }

    /// <summary>Uploads dodument attachment</summary>
    /// <param name="fileObject"></param>
    /// <param name="filename"></param>
    /// <param name="displayName"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="Category"></param>
    /// <param name="channel"></param>
    /// <param name="availbleAllTPO"></param>
    public void UploadDocument(
      DataObject fileObject,
      string filename,
      string displayName,
      DateTime startDate,
      DateTime endDate,
      DocumentCategory Category,
      ExternalOrganizationEntityType channel,
      bool availbleAllTPO)
    {
      string[] array = new string[12]
      {
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".txt",
        ".tif",
        ".jpg",
        ".jpeg",
        ".jpe",
        ".csv",
        ".xml"
      };
      string str = Path.GetExtension(filename).ToLower().Trim();
      if (str == string.Empty || Array.IndexOf<string>(array, str) < 0)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidDocumentExtension, "The '" + str + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.csv', and '.xml'.");
      if (fileObject.Data.Length > 25000000)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentExceedSize, "File attachments cannot exceed 25 MB. Please select another file.");
      Guid guid = Guid.NewGuid();
      DocumentSettingInfo document = new DocumentSettingInfo();
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      document.Active = true;
      document.Guid = guid;
      document.FileName = filename;
      document.DisplayName = displayName;
      document.StartDate = startDate;
      document.EndDate = endDate;
      document.Category = Category.ID;
      document.Channel = (ExternalOriginatorEntityType) Convert.ToInt32((object) channel);
      document.DateAdded = DateTime.Now;
      document.AvailbleAllTPO = availbleAllTPO;
      document.AddedBy = this.Session.GetUserInfo().FullName;
      document.IsArchive = false;
      document.FileSize = Utils.FormatByteSize((long) fileObject.Data.Length);
      configurationManager.AddDocument(-1, document, false);
      FileSystemEntry entry = new FileSystemEntry("\\\\" + guid.ToString() + str, FileSystemEntry.Types.File, (string) null);
      BinaryObject data = new BinaryObject(fileObject.Data);
      configurationManager.CreateDocumentInDataFolder(entry, data);
      if (!availbleAllTPO)
        return;
      configurationManager.AssignDefaultDocumentToAll(document);
    }
  }
}
