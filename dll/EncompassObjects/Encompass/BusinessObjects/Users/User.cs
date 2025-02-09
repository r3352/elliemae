// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.User
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using Elli.ElliEnum;
using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.Common.SimpleCache;
using EllieMae.EMLite.Common.Version;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Xml;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  public class User : SessionBoundObject, IUser
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private IOrganizationManager mngr;
    private IServicesAclManager servicesAclManager;
    internal UserInfo info;
    private bool isNew;
    private StateLicenses licenses;
    private UserPersonas personas;
    private string currentPassword = string.Empty;
    private const string LOGINURL = "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp�";
    private const string DEFAULTURL = "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp�";
    private const string SECONDARYLOGINURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp�";
    private const string SECONDARYDEFAULTURL = "https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp�";

    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    [DllImport("wininet.dll")]
    private static extern bool InternetSetCookieEx(
      string url,
      string name,
      string data,
      int flags,
      string p3p);

    internal User(Session session, UserInfo info, bool isNew)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (User), "Committed");
      this.info = info;
      this.mngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.isNew = isNew;
      this.currentPassword = this.Session.SessionObjects.UserPassword;
    }

    public string ID => this.info.Userid;

    public string Password
    {
      get => this.info.Password ?? "";
      set => this.info.Password = value ?? "";
    }

    public string FirstName
    {
      get => this.info.FirstName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.FirstName = value ?? "";
      }
    }

    public string MiddleName
    {
      get => this.info.MiddleName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.MiddleName = value ?? "";
      }
    }

    public string Suffix
    {
      get => this.info.SuffixName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.SuffixName = value ?? "";
      }
    }

    public string LastName
    {
      get => this.info.LastName;
      set
      {
        this.EnsureAdminOrUser();
        this.info.LastName = value ?? "";
      }
    }

    public string FullName => this.info.FullName;

    public string EmployeeID
    {
      get => this.info.EmployeeID;
      set
      {
        this.EnsureAdminOrUser();
        this.info.EmployeeID = value ?? "";
      }
    }

    public UserPersonas Personas
    {
      get
      {
        if (this.personas == null)
          this.personas = new UserPersonas(this, this.info.UserPersonas);
        return this.personas;
      }
    }

    public string JobTitle
    {
      get => this.info.JobTitle;
      set
      {
        this.EnsureAdminOrUser();
        this.info.JobTitle = value ?? "";
      }
    }

    public string Email
    {
      get => this.info.Email;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Email = value ?? "";
      }
    }

    public string Phone
    {
      get => this.info.Phone;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Phone = value ?? "";
      }
    }

    public string CellPhone
    {
      get => this.info.CellPhone;
      set
      {
        this.EnsureAdminOrUser();
        this.info.CellPhone = value ?? "";
      }
    }

    public string Fax
    {
      get => this.info.Fax;
      set
      {
        this.EnsureAdminOrUser();
        this.info.Fax = value ?? "";
      }
    }

    public string WorkingFolder
    {
      get => this.info.WorkingFolder;
      set
      {
        this.EnsureAdmin();
        this.info.WorkingFolder = value ?? "";
      }
    }

    public bool RequirePasswordChange
    {
      get => this.info.RequirePasswordChange;
      set
      {
        this.EnsureAdmin();
        this.info.RequirePasswordChange = value;
      }
    }

    public DateTime PasswordChangedDate => this.info.PasswordChangedDate;

    public bool AccountLocked
    {
      get => this.info.Locked;
      set
      {
        this.EnsureAdmin();
        this.info.Locked = value;
      }
    }

    public string CHUMID
    {
      get => this.info.CHUMId;
      set
      {
        this.EnsureAdmin();
        this.info.CHUMId = value ?? "";
      }
    }

    public string NMLSOriginatorID
    {
      get => this.info.NMLSOriginatorID;
      set
      {
        this.EnsureAdmin();
        this.info.NMLSOriginatorID = value ?? "";
      }
    }

    public DateTime NMLSExpirationDate
    {
      get => this.info.NMLSExpirationDate;
      set
      {
        this.EnsureAdmin();
        this.info.NMLSExpirationDate = value;
      }
    }

    public SubordinateLoanAccessRight SubordinateLoanAccessRight
    {
      get => (SubordinateLoanAccessRight) this.info.AccessMode;
      set
      {
        this.EnsureAdmin();
        this.info.AccessMode = (UserInfo.AccessModeEnum) value;
      }
    }

    public PeerLoanAccessRight PeerLoanAccessRight
    {
      get => (PeerLoanAccessRight) this.info.PeerView;
      set
      {
        this.EnsureAdmin();
        this.info.PeerView = (UserInfo.UserPeerView) value;
      }
    }

    public StateLicenses StateLicenses
    {
      get
      {
        if (this.licenses == null)
          this.licenses = new StateLicenses(this);
        return this.licenses;
      }
    }

    public int OrganizationID => this.info.OrgId;

    public int FailedLoginAttempts => this.info.failed_login_attempts;

    public void ChangeOrganization(Organization newOrganization)
    {
      this.ensureExists();
      this.EnsureAdmin();
      if (newOrganization.IsNew)
        throw new InvalidOperationException("The target organization must be committed before users can be added");
      this.mngr.MoveUserIntoOrganization(this.info.Userid, newOrganization.ID);
      string password = this.info.Password;
      this.info = new UserInfo(this.info, newOrganization.ID, newOrganization.IsTopMostOrganization);
      this.info.Password = password;
    }

    public bool Enabled => this.info.Status == 0;

    public void Enable()
    {
      this.EnsureAdmin();
      if (!this.isNew)
        this.mngr.EnableUser(this.info.Userid);
      this.info.Status = (UserInfo.UserStatusEnum) 0;
    }

    public void Disable()
    {
      this.EnsureAdmin();
      if (this.info.Userid == "admin")
        throw new InvalidOperationException("The 'admin' cannot be disabled");
      if (!this.isNew)
        this.mngr.DisableUser(this.info.Userid);
      this.info.Status = (UserInfo.UserStatusEnum) 1;
    }

    public bool HasAccessTo(Feature feature)
    {
      this.ensureExists();
      return this.FeaturesAclManager.CheckPermission((AclFeature) feature, this.info);
    }

    public void GrantAccessTo(Feature feature)
    {
      this.ensureExists();
      this.EnsureAdmin();
      this.FeaturesAclManager.SetPermission((AclFeature) feature, this.ID, (AclTriState) 1);
    }

    public void RevokeAccessTo(Feature feature)
    {
      this.ensureExists();
      this.EnsureAdmin();
      this.FeaturesAclManager.SetPermission((AclFeature) feature, this.ID, (AclTriState) 2);
    }

    public DataObject GetCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      BinaryObject customDataObject = ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetUserCustomDataObject(this.info.Userid, fileName);
      return customDataObject == null ? (DataObject) null : new DataObject(customDataObject);
    }

    public void SaveCustomDataObject(string fileName, DataObject dataObj)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (dataObj == null)
        throw new ArgumentNullException(nameof (dataObj));
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).SaveUserCustomDataObject(this.info.Userid, fileName, dataObj.Unwrap());
    }

    public void AppendToCustomDataObject(string fileName, DataObject dataObj)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      if (dataObj == null)
        throw new ArgumentNullException(nameof (dataObj));
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).AppendToUserCustomDataObject(this.info.Userid, fileName, dataObj.Unwrap());
    }

    public void DeleteCustomDataObject(string fileName)
    {
      if ((fileName ?? "") == "")
        throw new ArgumentException(nameof (fileName), "File name cannot be blank or null");
      ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).SaveUserCustomDataObject(this.info.Userid, fileName, (BinaryObject) null);
    }

    public void Commit()
    {
      if (this.Personas.Count == 0)
        throw new InvalidOperationException("Cannot commit a user without at least one assigned persona");
      this.info.UserPersonas = this.personas.Unwrap();
      bool isNew = this.isNew;
      if (this.isNew)
      {
        this.mngr.CreateNewUser(this.info);
        this.isNew = false;
      }
      else if (this.Session.GetUserInfo().IsAdministrator())
      {
        if (!this.info.SSOOnly && !this.info.PasswordExists && string.IsNullOrEmpty(this.info.Password))
          throw new InvalidOperationException("Password field is empty. Password is required for Full Access, please add password.");
        this.mngr.UpdateUser(this.info);
        CacheManager.GetSimpleCache("UserInfoCache").Remove(this.Session.SessionObjects.StartupInfo.ServerInstanceName + "@" + this.Session.SessionObjects.StartupInfo.ServerID + "_UserStore_" + Convert.ToString(this.info.Userid));
      }
      else
      {
        if (!(this.Session.UserID == this.info.Userid))
          throw new InvalidOperationException("Access denied");
        ICurrentUser user = this.Session.Unwrap().GetUser();
        user.UpdatePersonalInfo(this.info.FirstName, this.info.LastName, this.info.Email, this.info.Phone, this.info.CellPhone, this.info.Fax);
        if (!string.IsNullOrEmpty(this.info.Password))
        {
          user.ChangePassword(this.info.Password);
          this.currentPassword = this.info.Password;
        }
      }
      if (isNew)
      {
        bool isAdmin = !(this.Session.UserID == this.info.Userid) && this.Session.GetUserInfo().IsAdministrator();
        this.updateEMNPassword(isNew, isAdmin);
      }
      if (this.licenses != null)
        this.licenses.Commit();
      this.committed((object) this, new PersistentObjectEventArgs(this.ID));
    }

    private string addEMNUser(string url, string password)
    {
      SessionObjects sessionObjects = this.Session.SessionObjects;
      CompanyInfo companyInfo = sessionObjects.CompanyInfo;
      OrgInfo branchInfo = sessionObjects.StartupInfo.BranchInfo;
      if (branchInfo != null)
        companyInfo = new CompanyInfo(companyInfo.ClientID, branchInfo.CompanyName, branchInfo.CompanyAddress.Street1, branchInfo.CompanyAddress.City, branchInfo.CompanyAddress.State, branchInfo.CompanyAddress.Zip, branchInfo.CompanyPhone, branchInfo.CompanyFax, companyInfo.Password, new string[4]
        {
          branchInfo.DBAName1,
          branchInfo.DBAName2,
          branchInfo.DBAName3,
          branchInfo.DBAName4
        }, (BranchExtLicensing) ((BranchLicensing) branchInfo.OrgBranchLicensing).Clone());
      string str1 = string.Empty;
      RuntimeEnvironment runtimeEnvironment = sessionObjects.StartupInfo.RuntimeEnvironment;
      if (runtimeEnvironment != null)
      {
        if (runtimeEnvironment == 1)
          str1 = "Hosted";
      }
      else
        str1 = "Default";
      if (this.servicesAclManager == null)
        this.servicesAclManager = (IServicesAclManager) sessionObjects.Session.GetAclManager((AclCategory) 9);
      ServiceAclInfo.ServicesDefaultSetting servicesDefaultSetting = this.servicesAclManager.GetServicesDefaultSetting((AclFeature) 1023, sessionObjects.UserID, sessionObjects.UserInfo.UserPersonas);
      string str2 = string.Empty;
      string str3 = (string) null;
      switch ((int) servicesDefaultSetting)
      {
        case 0:
          str3 = "n";
          break;
        case 1:
          str3 = "c";
          str2 = this.getCompanyServices(sessionObjects);
          break;
        case 2:
          str3 = "y";
          break;
      }
      EncompassEdition encompassEdition = (EncompassEdition) 0;
      switch (this.Session.EncompassEdition)
      {
        case EncompassEdition.Broker:
          encompassEdition = (EncompassEdition) 3;
          break;
        case EncompassEdition.Banker:
          encompassEdition = (EncompassEdition) 5;
          break;
      }
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("clientid=" + HttpUtility.UrlEncode(companyInfo.ClientID));
      stringBuilder.Append("&clientname=" + HttpUtility.UrlEncode(companyInfo.Name));
      stringBuilder.Append("&clientaddress=" + HttpUtility.UrlEncode(companyInfo.Address));
      stringBuilder.Append("&clientcity=" + HttpUtility.UrlEncode(companyInfo.City));
      stringBuilder.Append("&clientstate=" + HttpUtility.UrlEncode(companyInfo.State));
      stringBuilder.Append("&clientzip=" + HttpUtility.UrlEncode(companyInfo.Zip));
      stringBuilder.Append("&clientphone=" + HttpUtility.UrlEncode(companyInfo.Phone));
      stringBuilder.Append("&clientfax=" + HttpUtility.UrlEncode(companyInfo.Fax));
      stringBuilder.Append("&clientpassword=" + HttpUtility.UrlEncode(companyInfo.Password));
      stringBuilder.Append("&userid=" + HttpUtility.UrlEncode(this.info.Userid));
      stringBuilder.Append("&userfirstname=" + HttpUtility.UrlEncode(this.info.FirstName));
      stringBuilder.Append("&userlastname=" + HttpUtility.UrlEncode(this.info.LastName));
      stringBuilder.Append("&userpersona=" + HttpUtility.UrlEncode(sessionObjects.GetEPassPersonaDescriptor()));
      stringBuilder.Append("&useremail=" + HttpUtility.UrlEncode(this.info.Email));
      stringBuilder.Append("&userphone=" + HttpUtility.UrlEncode(this.info.Phone));
      stringBuilder.Append("&userpassword=" + HttpUtility.UrlEncode(password));
      stringBuilder.Append("&personal=" + HttpUtility.UrlEncode(str3));
      stringBuilder.Append("&version=" + HttpUtility.UrlEncode(VersionInformation.CurrentVersion.GetExtendedVersion(encompassEdition)));
      stringBuilder.Append("&environment=" + HttpUtility.UrlEncode(str1));
      stringBuilder.Append("&companyservices=" + HttpUtility.UrlEncode(str2));
      stringBuilder.Append("&NMLSOriginatorID=" + HttpUtility.UrlEncode(this.info.NMLSOriginatorID));
      stringBuilder.Append("&NMLSExpirationDate=" + HttpUtility.UrlEncode(this.info.NMLSExpirationDate == DateTime.MaxValue ? "" : this.info.NMLSExpirationDate.ToString("MM/dd/yyyy")));
      DateTime dateTime;
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml("<LOLICENSES></LOLICENSES>");
        XmlElement documentElement = xmlDocument.DocumentElement;
        foreach (LOLicenseInfo loLicense in ((IOrganizationManager) this.Session.GetObject("OrganizationManager")).GetLOLicenses(sessionObjects.UserInfo.Userid))
        {
          if (loLicense.Enabled && loLicense.License != string.Empty)
          {
            XmlElement element = xmlDocument.CreateElement("LIC");
            documentElement.AppendChild((XmlNode) element);
            element.SetAttribute("ST", loLicense.StateAbbr.Trim());
            element.SetAttribute("LN", loLicense.License.Trim());
            XmlElement xmlElement = element;
            string str4;
            if (!(loLicense.ExpirationDate == DateTime.MaxValue))
            {
              dateTime = loLicense.ExpirationDate;
              str4 = dateTime.ToString("MM/dd/yyyy");
            }
            else
              str4 = "";
            xmlElement.SetAttribute("ED", str4);
          }
        }
        stringBuilder.Append("&lolicensesxml=" + HttpUtility.UrlEncode(xmlDocument.OuterXml));
      }
      catch
      {
        return (string) null;
      }
      string header1;
      string header2;
      string end;
      try
      {
        HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp");
        httpWebRequest.KeepAlive = false;
        httpWebRequest.Method = "POST";
        httpWebRequest.ContentType = "application/x-www-form-urlencoded";
        httpWebRequest.ContentLength = (long) stringBuilder.Length;
        httpWebRequest.Timeout = 5000;
        StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
        streamWriter.Write(stringBuilder.ToString());
        streamWriter.Close();
        HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse();
        header1 = response.Headers["Set-Cookie"];
        header2 = response.Headers["P3P"];
        StreamReader streamReader = new StreamReader(response.GetResponseStream());
        end = streamReader.ReadToEnd();
        streamReader.Close();
      }
      catch
      {
        switch (url)
        {
          case "https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp":
            return this.addEMNUser("https://core.elliemaeservices.com/epassutils/ePASSLoginHA.asp", password);
          case "https://www.epassbusinesscenter.com/epassutils/jedlogindefault.asp":
            return this.addEMNUser("https://core.elliemaeservices.com/epassutils/ePASSLoginDefaultHA.asp", password);
          default:
            return (string) null;
        }
      }
      if (header1 == null)
        return (string) null;
      Uri uri = new Uri("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp");
      CookieContainer cookieContainer = new CookieContainer();
      cookieContainer.SetCookies(uri, header1);
      foreach (Cookie cookie in cookieContainer.GetCookies(uri))
      {
        string str5 = cookie.ToString();
        if (cookie.Expires != DateTime.MinValue)
        {
          string str6 = str5;
          dateTime = cookie.Expires;
          string str7 = dateTime.ToString("r");
          str5 = str6 + "; expires=" + str7;
        }
        string data = str5 + "; path=" + cookie.Path;
        User.InternetSetCookieEx(uri.AbsoluteUri, (string) null, data, 64, header2);
      }
      return end;
    }

    private string[] getServices(SessionObjects sessionObjects, bool hasPermission)
    {
      ServiceAclInfo[] permissions = this.servicesAclManager.GetPermissions((AclFeature) 1023, sessionObjects.UserID, sessionObjects.UserInfo.UserPersonas);
      List<string> stringList = new List<string>();
      foreach (ServiceAclInfo serviceAclInfo in permissions)
      {
        if (serviceAclInfo.Access == hasPermission)
          stringList.Add(serviceAclInfo.ServiceTitle);
      }
      return stringList.ToArray();
    }

    private string getCompanyServices(SessionObjects sessionObjects)
    {
      string[] services = this.getServices(sessionObjects, false);
      XmlDocument xmlDocument = new XmlDocument();
      xmlDocument.LoadXml("<SERVICES/>");
      foreach (string str in services)
      {
        XmlElement element = xmlDocument.CreateElement("CATEGORY");
        element.SetAttribute("Title", str);
        xmlDocument.DocumentElement.AppendChild((XmlNode) element);
      }
      return xmlDocument.OuterXml;
    }

    private void updateEMNPassword(bool isNew, bool isAdmin)
    {
      if (!isNew)
        return;
      this.addEMNUser("https://www.epassbusinesscenter.com/epassutils/jedloginuser.asp", this.Password);
    }

    public void Delete()
    {
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
      if (this.ID == this.Session.UserID)
        throw new InvalidOperationException("Cannot delete currently logged in user");
      if (this.info.Userid == "admin")
        throw new InvalidOperationException("The admin user account cannot be deleted or disabled");
      this.mngr.DeleteUser(this.info.Userid, new UserAssignedContactsBehaviorEnums?(), (string) null);
    }

    public void Refresh()
    {
      if (this.IsNew)
        throw new InvalidOperationException("Method not valid for new objects");
      this.info = this.mngr.GetUser(this.info.Userid);
      if (this.licenses == null)
        return;
      this.licenses.Refresh();
    }

    public UserGroupList GetUserGroups()
    {
      AclGroup[] groupsOfUser = this.Session.SessionObjects.AclGroupManager.GetGroupsOfUser(this.ID);
      UserGroupList userGroups = new UserGroupList();
      foreach (AclGroup aclGroup in groupsOfUser)
      {
        UserGroup groupById = this.Session.Users.Groups.GetGroupByID(aclGroup.ID);
        if (groupById != null)
          userGroups.Add(groupById);
      }
      return userGroups;
    }

    public bool IsNew => this.isNew;

    public override string ToString() => this.FullName;

    public override int GetHashCode() => this.ID.GetHashCode();

    public override bool Equals(object obj)
    {
      return !object.Equals((object) (obj as User), (object) null) && ((SessionBoundObject) obj).Session == this.Session && ((User) obj).ID == this.ID;
    }

    internal void EnsureAdmin()
    {
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
    }

    internal void EnsureAdminOrUser()
    {
      if (!(this.Session.UserID != this.info.Userid))
        return;
      this.EnsureAdmin();
    }

    private void ensureExists()
    {
      if (this.IsNew)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    private IFeaturesAclManager FeaturesAclManager
    {
      get => (IFeaturesAclManager) this.Session.GetAclManager((AclCategory) 0);
    }

    internal UserInfo Unwrap() => this.info;

    internal static UserList ToList(Session session, UserInfo[] infos)
    {
      UserList list = new UserList();
      for (int index = 0; index < infos.Length; ++index)
        list.Add(new User(session, infos[index], false));
      return list;
    }

    public static User Wrap(Session session, UserInfo userInfo)
    {
      return new User(session, userInfo, false);
    }

    public bool SSOOnly
    {
      get => this.info.SSOOnly;
      set
      {
        this.EnsureAdmin();
        this.info.SSOOnly = value;
        OrgInfo organizationForSso = this.mngr.GetFirstOrganizationForSSO(this.info.OrgId);
        this.info.SSODisconnectedFromOrg = value != organizationForSso.SSOSettings.LoginAccess;
      }
    }

    public bool SSODisconnectedFromOrg
    {
      get => this.info.SSODisconnectedFromOrg;
      set
      {
        this.EnsureAdmin();
        this.info.SSODisconnectedFromOrg = value;
        OrgInfo organizationForSso = this.mngr.GetFirstOrganizationForSSO(this.info.OrgId);
        if (value)
          return;
        this.info.SSOOnly = organizationForSso.SSOSettings.LoginAccess;
      }
    }
  }
}
