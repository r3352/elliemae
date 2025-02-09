// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ExternalUser
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.RemotingServices;
using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
  /// <summary>Represents a single External Organization user.</summary>
  public class ExternalUser : SessionBoundObject, IExternalUser
  {
    internal ExternalUserInfo info;
    private EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization parentOrganization;
    private EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization companyOrganization;
    private EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization branchOrganization;
    private IConfigurationManager mngr;
    private IPersonaManager personamgr;
    private bool isNew;
    private ExternalUserList accessibleUserList;
    private ExternalUserInfo updatingExtUser;
    private ExternalUserURL[] urlList;
    private List<ExternalOrgURL> externalUrlList;
    private List<ExternalOriginatorManagementData> companyOrgs;
    private List<CurrentContactStatus> contactStatus;
    private UserInfo salesRep;
    private Dictionary<string, List<string>> siteIDMapping;
    private Dictionary<string, List<string>> urlIDMapping;
    private bool hasPerformancePatch;
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private ExternalLicensing licensing;

    /// <summary>Event indicating that the object has been committed to the server.</summary>
    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal ExternalUser(
      Session session,
      ExternalUserInfo info,
      ExternalUserInfo updatingExtUser,
      List<ExternalOriginatorManagementData> companyOrgs,
      bool isNew,
      bool getDetails,
      bool hasPerformancePatch)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (ExternalUser), "Committed");
      this.info = info;
      this.companyOrgs = companyOrgs;
      this.updatingExtUser = updatingExtUser;
      ExternalOriginatorManagementData originatorManagementData = companyOrgs.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == info.ExternalOrgID));
      ExternalOriginatorManagementData externalCompany = Organizations.GetExternalCompany(info.ExternalOrgID, companyOrgs);
      ExternalOriginatorManagementData externalBranch = Organizations.GetExternalBranch(info.ExternalOrgID, companyOrgs);
      this.hasPerformancePatch = hasPerformancePatch;
      this.companyOrganization = new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(session, companyOrgs, externalCompany.oid, false, this.hasPerformancePatch);
      if (externalBranch != null)
        this.branchOrganization = new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(session, companyOrgs, externalBranch.oid, false, this.hasPerformancePatch);
      this.parentOrganization = new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(session, companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch);
      this.mngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      this.personamgr = (IPersonaManager) session.GetObject("PersonaManager");
      if (getDetails)
        this.getAllDetailInfo();
      this.isNew = isNew;
    }

    /// <summary>Gets the external user's ID</summary>
    public string ID
    {
      get
      {
        this.ensureExists();
        return this.info.ExternalUserID;
      }
    }

    /// <summary>Gets or sets the user's first name.</summary>
    public string FirstName
    {
      get
      {
        this.ensureExists();
        return this.info.FirstName;
      }
      set
      {
        this.ensureExists();
        this.info.FirstName = value;
      }
    }

    /// <summary>Gets or sets the user's middle name</summary>
    public string MiddleName
    {
      get
      {
        this.ensureExists();
        return this.info.MiddleName;
      }
      set
      {
        this.ensureExists();
        this.info.MiddleName = value;
      }
    }

    /// <summary>Gets or sets the user's last name</summary>
    public string LastName
    {
      get
      {
        this.ensureExists();
        return this.info.LastName;
      }
      set
      {
        this.ensureExists();
        this.info.LastName = value;
      }
    }

    /// <summary>Gets or sets the user's suffix</summary>
    public string Suffix
    {
      get
      {
        this.ensureExists();
        return this.info.Suffix;
      }
      set
      {
        this.ensureExists();
        this.info.Suffix = value;
      }
    }

    /// <summary>Gets or sets user's Title</summary>
    public string Title
    {
      get
      {
        this.ensureExists();
        return this.info.Title;
      }
      set
      {
        this.ensureExists();
        this.info.Title = value;
      }
    }

    /// <summary>Gets or sets user's address</summary>
    public string Address
    {
      get
      {
        this.ensureExists();
        return this.info.Address;
      }
      set
      {
        this.ensureExists();
        if (!this.UseCompanyAddress)
          this.info.Address = value;
        else if (value != this.info.Address)
          throw new ExternalUserValidationException(UserViolationType.InvalidExternalUserAddress, "Invalid Address");
      }
    }

    /// <summary>Gets or sets user's city</summary>
    public string City
    {
      get
      {
        this.ensureExists();
        return this.info.City;
      }
      set
      {
        this.ensureExists();
        if (!this.UseCompanyAddress)
          this.info.City = value;
        else if (value != this.info.City)
          throw new ExternalUserValidationException(UserViolationType.InvalidExternalUserAddress, "Invalid Address");
      }
    }

    /// <summary>Gets or sets user's state</summary>
    public string State
    {
      get
      {
        this.ensureExists();
        return this.info.State;
      }
      set
      {
        this.ensureExists();
        if (!this.UseCompanyAddress)
          this.info.State = value;
        else if (value != this.info.State)
          throw new ExternalUserValidationException(UserViolationType.InvalidExternalUserAddress, "Invalid Address");
      }
    }

    /// <summary>Gets or sets user's zipcode</summary>
    public string Zipcode
    {
      get
      {
        this.ensureExists();
        return this.info.Zipcode;
      }
      set
      {
        this.ensureExists();
        if (!this.UseCompanyAddress)
          this.info.Zipcode = value;
        else if (value != this.info.Zipcode)
          throw new ExternalUserValidationException(UserViolationType.InvalidExternalUserAddress, "Invalid Address");
      }
    }

    /// <summary>Gets or sets user's phone</summary>
    public string Phone
    {
      get
      {
        this.ensureExists();
        return this.info.Phone;
      }
      set
      {
        this.ensureExists();
        this.info.Phone = value;
      }
    }

    /// <summary>Gets or sets user's cellphone</summary>
    public string CellPhone
    {
      get
      {
        this.ensureExists();
        return this.info.CellPhone;
      }
      set
      {
        this.ensureExists();
        this.info.CellPhone = value;
      }
    }

    /// <summary>Gets or sets user's fax</summary>
    public string Fax
    {
      get
      {
        this.ensureExists();
        return this.info.Fax;
      }
      set
      {
        this.ensureExists();
        this.info.Fax = value;
      }
    }

    /// <summary>Gets or sets user's ssn</summary>
    public string SSN
    {
      get
      {
        this.ensureExists();
        return this.info.SSN;
      }
      set
      {
        this.ensureExists();
        this.info.SSN = value;
      }
    }

    /// <summary>Gets or sets user's email</summary>
    public string Email
    {
      get
      {
        this.ensureExists();
        return this.info.Email;
      }
      set
      {
        this.ensureExists();
        this.info.Email = value;
      }
    }

    /// <summary>Gets or sets user's rate sheet email</summary>
    public string EmailForRateSheet
    {
      get
      {
        this.ensureExists();
        return this.info.EmailForRateSheet;
      }
      set
      {
        this.ensureExists();
        this.info.EmailForRateSheet = value;
      }
    }

    /// <summary>Gets or sets user's Rate sheet fax number</summary>
    public string FaxForRateSheet
    {
      get
      {
        this.ensureExists();
        return this.info.FaxForRateSheet;
      }
      set
      {
        this.ensureExists();
        this.info.FaxForRateSheet = value;
      }
    }

    /// <summary>Gets or sets user's lock info email</summary>
    public string EmailForLockInfo
    {
      get
      {
        this.ensureExists();
        return this.info.EmailForLockInfo;
      }
      set
      {
        this.ensureExists();
        this.info.EmailForLockInfo = value;
      }
    }

    /// <summary>Gets or sets user's lock info fax number</summary>
    public string FaxForLockInfo
    {
      get
      {
        this.ensureExists();
        return this.info.FaxForLockInfo;
      }
      set
      {
        this.ensureExists();
        this.info.FaxForLockInfo = value;
      }
    }

    /// <summary>Gets or sets user's TPO Webcenter login email</summary>
    public string EmailForLogin
    {
      get
      {
        this.ensureExists();
        return this.info.EmailForLogin;
      }
      set
      {
        this.ensureExists();
        this.info.EmailForLogin = value;
      }
    }

    /// <summary>Gets or sets user's notes</summary>
    public string Notes
    {
      get
      {
        this.ensureExists();
        return this.info.Notes;
      }
      set
      {
        this.ensureExists();
        this.info.Notes = value;
      }
    }

    /// <summary>Gets or sets user's add to watchlist setting</summary>
    public bool OnWatchlist
    {
      get
      {
        this.ensureExists();
        return this.info.AddToWatchlist;
      }
      set
      {
        this.ensureExists();
        this.info.AddToWatchlist = value;
      }
    }

    /// <summary>Gets or sets user's approval date field</summary>
    public DateTime ApprovalDate
    {
      get
      {
        this.ensureExists();
        return this.info.ApprovalDate;
      }
      set
      {
        this.ensureExists();
        this.info.ApprovalDate = value;
      }
    }

    /// <summary>Gets or sets the approval status date</summary>
    public DateTime ApprovalCurrentStatusDate
    {
      get
      {
        this.ensureExists();
        return this.info.ApprovalCurrentStatusDate;
      }
      set
      {
        this.ensureExists();
        this.info.ApprovalCurrentStatusDate = value;
      }
    }

    /// <summary>Gets or sets the approval status</summary>
    public CurrentContactStatus ApprovalCurrentStatus
    {
      get
      {
        this.ensureExists();
        if (this.info.ApprovalCurrentStatus < 0)
          return (CurrentContactStatus) null;
        this.contactStatus = this.GetContactStatus();
        return this.contactStatus.FirstOrDefault<CurrentContactStatus>((Func<CurrentContactStatus, bool>) (x => x.ID == this.info.ApprovalCurrentStatus));
      }
      set
      {
        this.ensureExists();
        this.info.ApprovalCurrentStatus = value.ID;
      }
    }

    /// <summary>Gets or sets user's sales rep</summary>
    public User SalesRep
    {
      get
      {
        this.ensureExists();
        if (this.info.SalesRepID == "")
          return (User) null;
        if (this.salesRep == (UserInfo) null)
          this.getDetailInfo(new List<ExternalUserSetting>()
          {
            ExternalUserSetting.SalesRep
          });
        return new User(this.Session, this.salesRep, false);
      }
      set
      {
        if (this.info.SalesRepID == value.ID)
          return;
        this.info.SalesRepID = value.ID;
        this.salesRep = value.info;
      }
    }

    /// <summary>
    /// Get accessible external user list.  These external users object does not contain detail organization information.
    /// </summary>
    public ExternalUserList AccessibleUserList
    {
      get
      {
        this.ensureExists();
        if (this.accessibleUserList == null)
          this.getDetailInfo(new List<ExternalUserSetting>()
          {
            ExternalUserSetting.AccessibleUserList
          });
        return this.accessibleUserList;
      }
    }

    /// <summary>Gets or sets the user's failed login attempts</summary>
    public int LoginAttempts
    {
      get
      {
        this.ensureExists();
        return this.info.failed_login_attempts;
      }
      set
      {
        this.ensureExists();
        this.info.failed_login_attempts = value;
      }
    }

    /// <summary>
    /// Return the accessbile user list based on a particular urlID
    /// </summary>
    /// <param name="urlID"></param>
    /// <returns></returns>
    public ExternalUserList GetAccessibleUsersBySite(int urlID)
    {
      this.ensureExists();
      ExternalUserList result = new ExternalUserList();
      if (this.accessibleUserList == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.AccessibleUserList
        });
      if (this.hasPerformancePatch)
      {
        if (!this.urlIDMapping.ContainsKey(string.Concat((object) urlID)))
          return result;
        List<string> stringList = this.urlIDMapping[string.Concat((object) urlID)];
        foreach (ExternalUser accessibleUser in (CollectionBase) this.accessibleUserList)
        {
          if (stringList.Contains(accessibleUser.ID))
            result.Add(accessibleUser);
        }
      }
      else
      {
        List<string> contactIDs = new List<string>();
        ((IEnumerable<ExternalUser>) this.accessibleUserList.ToArray()).ToList<ExternalUser>().ForEach((Action<ExternalUser>) (x => contactIDs.Add(x.ContactID)));
        this.mngr.GetExternalUserInfoList(contactIDs, urlID).ForEach((Action<ExternalUserInfo>) (x => result.Add(new ExternalUser(this.Session, x, (ExternalUserInfo) null, this.companyOrgs, false, false, this.hasPerformancePatch))));
      }
      return result;
    }

    /// <summary>
    /// Return the accessbile user list based on a particular siteID
    /// </summary>
    /// <param name="siteID"></param>
    /// <returns></returns>
    public ExternalUserList GetAccessibleUsersBySite(string siteID)
    {
      this.ensureExists();
      if (!this.hasPerformancePatch)
        throw new Exception("This API is not available with current Encompass Server version.");
      List<string> stringList1 = new List<string>();
      ExternalUserList accessibleUsersBySite = new ExternalUserList();
      if (this.accessibleUserList == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.AccessibleUserList
        });
      if (!this.siteIDMapping.ContainsKey(siteID ?? ""))
        return accessibleUsersBySite;
      List<string> stringList2 = this.siteIDMapping[siteID ?? ""];
      foreach (ExternalUser accessibleUser in (CollectionBase) this.accessibleUserList)
      {
        if (stringList2.Contains(accessibleUser.ID))
          accessibleUsersBySite.Add(accessibleUser);
      }
      return accessibleUsersBySite;
    }

    /// <summary>
    /// Gets or sets user's last login information on TPO Webcenter
    /// </summary>
    public DateTime LastLogin
    {
      get
      {
        this.ensureExists();
        return this.info.LastLogin;
      }
      set
      {
        this.ensureExists();
        this.info.LastLogin = value;
      }
    }

    /// <summary>Gets user's external organization id</summary>
    public int ExternalOrganizationId
    {
      get
      {
        this.ensureExists();
        return this.info.ExternalOrgID;
      }
    }

    /// <summary>Check if the user if enabled to login TPO webcenter</summary>
    public bool Enabled
    {
      get
      {
        this.ensureExists();
        return !this.info.DisabledLogin;
      }
    }

    /// <summary>Gets the last password change date</summary>
    public DateTime PasswordChangedDate
    {
      get
      {
        this.ensureExists();
        return this.info.PasswordChangedDate;
      }
    }

    /// <summary>Gets the indicator for password change requirement</summary>
    public bool RequirePasswordChange
    {
      get
      {
        this.ensureExists();
        return this.info.RequirePasswordChange;
      }
    }

    /// <summary>Gets the external organization the user belongs to</summary>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization ParentOrganization
    {
      get => this.parentOrganization;
    }

    /// <summary>Gets the external company the user belongs to</summary>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company
    {
      get => this.companyOrganization;
    }

    /// <summary>Gets the external branch the user belongs to</summary>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch
    {
      get => this.branchOrganization;
    }

    /// <summary>A flag indicating if the user is a loan officer</summary>
    public bool IsLoanOfficer => (this.info.Roles & 1) == 1;

    /// <summary>A flag indicating if the user is a loan processor</summary>
    public bool IsLoanProcessor => (this.info.Roles & 2) == 2;

    /// <summary>A flag indicating if the user is a manager</summary>
    public bool IsManager => (this.info.Roles & 4) == 4;

    /// <summary>A flag indicating if the user is administrator</summary>
    public bool IsAdministrator => (this.info.Roles & 8) == 8;

    /// <summary>Gets or sets the flag for using company address</summary>
    public bool UseCompanyAddress
    {
      get
      {
        this.ensureExists();
        return this.info.UseCompanyAddress;
      }
      set
      {
        this.ensureExists();
        this.info.UseCompanyAddress = value;
      }
    }

    /// <summary>Returns the licensing information of the user</summary>
    /// <return>Returns a <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalLicensing" /> containing the licensing information of the user
    /// </return>
    public ExternalLicensing Licensing
    {
      get
      {
        this.ensureExists();
        if (this.licensing == null)
          this.getDetailInfo(new List<ExternalUserSetting>()
          {
            ExternalUserSetting.License
          });
        return this.licensing;
      }
      set
      {
        this.ensureExists();
        this.licensing = value;
      }
    }

    /// <summary>Gets contactid of the user</summary>
    public string ContactID
    {
      get
      {
        this.ensureExists();
        return this.info.ContactID;
      }
    }

    /// <summary>Gets and sets NmlsId of the user</summary>
    public string NmlsId
    {
      get
      {
        this.ensureExists();
        return this.info.NmlsID;
      }
      set
      {
        this.ensureExists();
        this.info.NmlsID = value;
      }
    }

    /// <summary>
    /// Gets the userId of the user account last updating the user account.  It could be external user ID or Encompass user ID.
    /// </summary>
    public string UpdatedBy
    {
      get
      {
        this.ensureExists();
        return this.info.UpdatedBy;
      }
    }

    /// <summary>
    /// Gets the first name of the user account last updating the user account.
    /// </summary>
    public string UpdatedByFirstName
    {
      get
      {
        return (UserInfo) this.updatingExtUser != (UserInfo) null ? this.updatingExtUser.FirstName : "";
      }
    }

    /// <summary>
    /// Gets the middle name of the user account last updating the user account.
    /// </summary>
    public string UpdatedByMiddleName
    {
      get
      {
        return (UserInfo) this.updatingExtUser != (UserInfo) null ? this.updatingExtUser.MiddleName : "";
      }
    }

    /// <summary>
    /// Gets the last name of the user account last updating the user account.
    /// </summary>
    public string UpdatedByLastName
    {
      get
      {
        return (UserInfo) this.updatingExtUser != (UserInfo) null ? this.updatingExtUser.LastName : "";
      }
    }

    /// <summary>
    /// Gets the email of the user account last updating the user account.
    /// </summary>
    public string UpdatedByEmail
    {
      get => (UserInfo) this.updatingExtUser != (UserInfo) null ? this.updatingExtUser.Email : "";
    }

    /// <summary>Gets last updated date and time of the user account.</summary>
    public DateTime UpdatedDateTime
    {
      get
      {
        this.ensureExists();
        return this.info.UpdatedDateTime;
      }
    }

    /// <summary>
    /// Returns the set of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">ExternalUrl</see> to which the user belongs
    /// </summary>
    /// <returns>Returns a <see cref="T:EllieMae.Encompass.Collections.ExternalUrlList" /> containing the set of external urls to which
    /// the user assigned to.
    /// </returns>
    public ExternalUrlList AssignedUrls
    {
      get
      {
        if (this.urlList == null)
          this.getDetailInfo(new List<ExternalUserSetting>()
          {
            ExternalUserSetting.AssignedUrls
          });
        ExternalUrlList result = new ExternalUrlList();
        ((IEnumerable<ExternalUserURL>) this.urlList).ToList<ExternalUserURL>().ForEach((Action<ExternalUserURL>) (x =>
        {
          ExternalOrgURL externalOrgUrl = ((IEnumerable<ExternalOrgURL>) this.externalUrlList.ToArray()).FirstOrDefault<ExternalOrgURL>((Func<ExternalOrgURL, bool>) (y => y.URLID == x.URLID));
          if (externalOrgUrl == null)
            return;
          result.Add(new ExternalUrl(externalOrgUrl));
        }));
        return result;
      }
    }

    /// <summary>Batch Load Settings</summary>
    /// <param name="settings">List of different areas to preload data</param>
    public void BatchLoadSettings(List<ExternalUserSetting> settings)
    {
      List<ExternalUserInfo.ExternalUserInfoSetting> requested = new List<ExternalUserInfo.ExternalUserInfoSetting>();
      if (settings.Contains(ExternalUserSetting.AccessibleUserList) && this.accessibleUserList == null)
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.AccessibleUserList);
      if (settings.Contains(ExternalUserSetting.AssignedUrls) && (this.urlList == null || this.externalUrlList == null))
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls);
      if (settings.Contains(ExternalUserSetting.License) && this.licensing == null)
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.License);
      if (settings.Contains(ExternalUserSetting.UpdatingUser) && (UserInfo) this.updatingExtUser == (UserInfo) null)
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser);
      if (settings.Contains(ExternalUserSetting.ContactStatus) && this.contactStatus == null)
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.ContactStatus);
      if (settings.Contains(ExternalUserSetting.SalesRep) && this.salesRep == (UserInfo) null)
        requested.Add(ExternalUserInfo.ExternalUserInfoSetting.SalesRep);
      Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> dictionary = new Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object>();
      if (this.hasPerformancePatch)
      {
        dictionary = this.mngr.GetExternalAdditionalDetails(this.ID, requested);
      }
      else
      {
        if (settings.Contains(ExternalUserSetting.AssignedUrls) || settings.Contains(ExternalUserSetting.License))
        {
          List<object> additionalDetails = this.mngr.GetExternalAdditionalDetails(this.ID);
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls, (object) new List<object>()
          {
            additionalDetails[1],
            additionalDetails[2]
          });
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.License, additionalDetails[0]);
        }
        if (settings.Contains(ExternalUserSetting.AccessibleUserList))
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.AccessibleUserList, (object) new List<object>()
          {
            (object) this.mngr.GetAccessibleExternalUserInfos(this.ID),
            (object) new Dictionary<string, List<string>>(),
            (object) new Dictionary<string, List<string>>()
          });
        if (settings.Contains(ExternalUserSetting.UpdatingUser) && (UserInfo) this.updatingExtUser == (UserInfo) null)
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser, (object) this.mngr.GetExternalUserInfo(this.info.UpdatedBy));
        if (settings.Contains(ExternalUserSetting.ContactStatus) && this.contactStatus == null)
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.ContactStatus, (object) this.mngr.GetExternalOrgSettingsByName("Current Contact Status"));
        if (settings.Contains(ExternalUserSetting.SalesRep) && this.salesRep == (UserInfo) null)
        {
          IOrganizationManager organizationManager = (IOrganizationManager) this.Session.GetObject("OrganizationManager");
          dictionary.Add(ExternalUserInfo.ExternalUserInfoSetting.SalesRep, (object) organizationManager.GetUser(this.info.SalesRepID));
        }
      }
      if (settings.Contains(ExternalUserSetting.AccessibleUserList))
      {
        List<object> objectList = (List<object>) dictionary[ExternalUserInfo.ExternalUserInfoSetting.AccessibleUserList];
        List<ExternalUserInfo> externalUserInfoList = (List<ExternalUserInfo>) objectList[0];
        this.siteIDMapping = (Dictionary<string, List<string>>) objectList[1];
        this.urlIDMapping = (Dictionary<string, List<string>>) objectList[2];
        this.accessibleUserList = new ExternalUserList();
        if (externalUserInfoList != null && externalUserInfoList.Count > 0)
        {
          Stopwatch.StartNew();
          foreach (ExternalUserInfo info in externalUserInfoList)
            this.accessibleUserList.Add(new ExternalUser(this.Session, info, (ExternalUserInfo) null, this.companyOrgs, false, false, this.hasPerformancePatch));
        }
      }
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>(ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls) && (this.urlList == null || this.externalUrlList == null))
      {
        List<object> objectList = (List<object>) dictionary[ExternalUserInfo.ExternalUserInfoSetting.AssignedUrls];
        this.urlList = (ExternalUserURL[]) objectList[0];
        this.externalUrlList = (List<ExternalOrgURL>) objectList[1];
      }
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>(ExternalUserInfo.ExternalUserInfoSetting.License))
      {
        BranchExtLicensing licensing = (BranchExtLicensing) dictionary[ExternalUserInfo.ExternalUserInfoSetting.License];
        if (licensing != null)
        {
          this.licensing = new ExternalLicensing(licensing);
          if (this.info.Licenses != null)
            this.info.Licenses.Clear();
          this.info.Licenses = licensing.StateLicenseExtTypes;
        }
      }
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>(ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser) && (UserInfo) this.updatingExtUser == (UserInfo) null)
        this.updatingExtUser = (ExternalUserInfo) dictionary[ExternalUserInfo.ExternalUserInfoSetting.UpdatingUser];
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>(ExternalUserInfo.ExternalUserInfoSetting.ContactStatus) && this.contactStatus == null)
      {
        List<ExternalSettingValue> externalSettingValueList = (List<ExternalSettingValue>) dictionary[ExternalUserInfo.ExternalUserInfoSetting.ContactStatus];
        this.contactStatus = new List<CurrentContactStatus>();
        externalSettingValueList.ForEach((Action<ExternalSettingValue>) (x => this.contactStatus.Add(new CurrentContactStatus(this.Session, x.settingValue, x.settingId))));
      }
      if (!dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>(ExternalUserInfo.ExternalUserInfoSetting.SalesRep) || !(this.salesRep == (UserInfo) null))
        return;
      this.salesRep = (UserInfo) dictionary[ExternalUserInfo.ExternalUserInfoSetting.SalesRep];
    }

    private void getDetailInfo(List<ExternalUserSetting> settings)
    {
      if (this.accessibleUserList != null && settings.Contains(ExternalUserSetting.AccessibleUserList))
        settings.Remove(ExternalUserSetting.AccessibleUserList);
      if (this.licensing != null && settings.Contains(ExternalUserSetting.License))
        settings.Remove(ExternalUserSetting.License);
      if (this.urlList != null && this.externalUrlList != null && settings.Contains(ExternalUserSetting.AssignedUrls))
        settings.Remove(ExternalUserSetting.AssignedUrls);
      if (this.contactStatus != null && settings.Contains(ExternalUserSetting.ContactStatus))
        settings.Remove(ExternalUserSetting.ContactStatus);
      this.BatchLoadSettings(settings);
    }

    private void getAllDetailInfo()
    {
      List<ExternalUserSetting> settings = new List<ExternalUserSetting>();
      if (this.accessibleUserList == null)
        settings.Add(ExternalUserSetting.AccessibleUserList);
      if (this.licensing == null)
        settings.Add(ExternalUserSetting.License);
      if (this.urlList == null || this.externalUrlList == null)
        settings.Add(ExternalUserSetting.AssignedUrls);
      if (this.contactStatus == null)
        settings.Add(ExternalUserSetting.ContactStatus);
      this.BatchLoadSettings(settings);
    }

    private ExternalOriginatorManagementData GetExternalCompany(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      ExternalOriginatorManagementData company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgID));
      while (company != null && company.Parent > 0)
        company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == company.Parent));
      return company;
    }

    private ExternalOriginatorManagementData GetExternalBranch(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      ExternalOriginatorManagementData company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgID));
      if (company.Depth < 2)
        return (ExternalOriginatorManagementData) null;
      if (company.Depth == 2)
        return company;
      while (company != null && company.Parent > 0)
      {
        company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == company.Parent));
        if (company.Depth <= 2)
          break;
      }
      return company;
    }

    /// <summary>
    /// Sets external user account which updates this user account
    /// </summary>
    public ExternalUser UpdatingExternalUser
    {
      set
      {
        this.ensureExists();
        this.updatingExtUser = value.info;
        this.info.UpdatedByExternalAdmin = (UserInfo) this.updatingExtUser != (UserInfo) null ? this.updatingExtUser.ContactID : "";
      }
    }

    private void ensureExists()
    {
      if ((UserInfo) this.info == (UserInfo) null)
        throw new InvalidOperationException("This operation is not valid until this user account login successfully.");
    }

    internal static ExternalUserList ToList(
      Session session,
      ExternalUserInfo[] infos,
      List<ExternalOriginatorManagementData> companyOrgs,
      bool hasPerformancePatch)
    {
      ExternalUserList list = new ExternalUserList();
      ((IEnumerable<ExternalUserInfo>) infos).ToList<ExternalUserInfo>().ForEach((Action<ExternalUserInfo>) (x => list.Add(new ExternalUser(session, x, (ExternalUserInfo) null, companyOrgs, false, false, hasPerformancePatch))));
      return list;
    }

    /// <summary>Set state licenses to the user account.</summary>
    /// <param name="newStateLicenses">The list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType" /> which the user
    /// should has.</param>
    /// <remarks>
    /// </remarks>
    public void AddStateLicenseExtType(List<StateLicenseExtType> newStateLicenses)
    {
      if (this.info.Licenses != null)
        this.info.Licenses.Clear();
      if (this.licensing.StateLicenseExtTypes != null)
        this.licensing.StateLicenseExtTypes.Clear();
      if (newStateLicenses == null)
        return;
      for (int index = 0; index < newStateLicenses.Count; ++index)
      {
        this.info.Licenses.Add(new EllieMae.EMLite.RemotingServices.StateLicenseExtType(newStateLicenses[index].StateAbbrevation, newStateLicenses[index].LicenseType, newStateLicenses[index].LicenseNo, newStateLicenses[index].IssueDate, newStateLicenses[index].StartDate, newStateLicenses[index].EndDate, newStateLicenses[index].LicenseStatus, newStateLicenses[index].StatusDate, newStateLicenses[index].Approved, newStateLicenses[index].Exempt, newStateLicenses[index].LastChecked, newStateLicenses[index].SortIndex));
        this.licensing.AddStateLicenseExtType(newStateLicenses[index]);
      }
    }

    /// <summary>Commits the changes to the current externaluser.</summary>
    /// <example>
    ///   The following code retrieves a user from the Encompass Server, modifies
    ///   its name and email address and saves it back to the external user.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //update user's first and last name
    ///       externalUser.FirstName = "NewFirstName";
    ///       externalUser.LastName = "NewLastName";
    /// 
    ///       //save changes to external users
    ///       externalUser.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Commit()
    {
      this.ensureExists();
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
      List<ExternalUserInfo> contactsByLoginEmailId = this.mngr.GetAllContactsByLoginEmailId(this.EmailForLogin, "");
      if (contactsByLoginEmailId != null && !this.info.DisabledLogin)
      {
        ExternalUserInfo externalUserInfo = contactsByLoginEmailId.FirstOrDefault<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (item => item.ExternalUserID == this.ID));
        if ((UserInfo) externalUserInfo != (UserInfo) null)
          contactsByLoginEmailId.Remove(externalUserInfo);
        List<ExternalUserInfoURL> urlsByContactIds = this.mngr.GetExternalUserInfoUrlsByContactIds(string.Join("','", contactsByLoginEmailId.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>()));
        if (urlsByContactIds != null)
        {
          foreach (ExternalUrl assignedUrl in (CollectionBase) this.AssignedUrls)
          {
            ExternalUrl url = assignedUrl;
            if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == url.URLID)) != null)
              throw new Exception("Users with login email '" + this.EmailForLogin + "' and URL '" + url.URL + "' already exist for this organization.");
          }
        }
      }
      if (this.info.UpdatedByExternalAdmin == null || this.info.UpdatedByExternalAdmin == "")
        this.info.UpdatedByExternalAdmin = this.info.UpdatedBy;
      this.info = this.mngr.SaveExternalUserInfo(this.info, true);
      this.isNew = false;
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.ID));
    }

    /// <summary>Deletes the current user.</summary>
    /// <remarks>This method can only be invoked by an Administrator.</remarks>
    /// <example>
    ///   The following code deletes a external LO user account if there are no loans
    ///   currently assigned to that user account.
    ///   <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
    /// using EllieMae.Encompass.BusinessObjects.Users;
    /// using EllieMae.Encompass.Query;
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       if (externalUser.IsLoanOfficer)
    ///       {
    ///           // Create the query criterion that specifies an exact match based on the TPO LO's ID
    ///           StringFieldCriterion lpCri = new StringFieldCriterion();
    ///           lpCri.FieldName = "TPO.X62";
    ///           lpCri.Value = externalUser.ContactID;
    /// 
    ///           // Run the query to get the list of matching loans
    ///           LoanIdentityList assignedLoans = session.Loans.Query(lpCri);
    /// 
    ///           if (assignedLoans.Count == 0)
    ///               externalUser.Delete();
    ///       }
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void Delete()
    {
      this.ensureExists();
      this.mngr.DeleteExternalUserInfo(this.info.ExternalOrgID, this.info, this.Session.GetUserInfo());
    }

    /// <summary>
    /// Reset password of the user account with a randomly generated password.
    /// </summary>
    /// <returns>New temporary password</returns>
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and reset its credential to a random password.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //update user's first and last name
    ///       string newRandomPassword = externalUser.ResetPassword();
    /// 
    ///       Console.WriteLine("New User Password:" + newRandomPassword);
    /// 
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public string ResetPassword()
    {
      this.ensureExists();
      string newPassword = this.info.GenerateNewPassword();
      this.Commit();
      this.info = this.mngr.ResetExternalUserInfoPassword(this.info.ExternalUserID, newPassword, DateTime.Now, true);
      return newPassword;
    }

    /// <summary>Set password of the user account</summary>
    /// <param name="newPassword" />
    /// <returns />
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and update login credential with a given password.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //update user's first and last name
    ///       bool result = externalUser.SetPassword("New Password");
    /// 
    ///       if(result)
    ///         Console.WriteLine("Password has been updated.");
    ///       else
    ///         Console.WriteLine("Failed to update password.");
    /// 
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool SetPassword(string newPassword)
    {
      try
      {
        this.ensureExists();
        this.Commit();
        this.info = this.mngr.ResetExternalUserInfoPassword(this.info.ExternalUserID, newPassword, DateTime.Now, false);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    /// <summary>Enable TPO web center login</summary>
    public void Enable()
    {
      if (!((UserInfo) this.info != (UserInfo) null))
        return;
      this.info.DisabledLogin = false;
      this.Commit();
    }

    /// <summary>Disable TPO web center login</summary>
    public void Disable()
    {
      if (!((UserInfo) this.info != (UserInfo) null))
        return;
      this.info.DisabledLogin = true;
      this.info.failed_login_attempts = 0;
      this.Commit();
    }

    /// <summary>
    /// Assign <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUserRoles" /> to the user account
    /// </summary>
    /// <param name="role">The role to assign.</param>
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and assign loan officer role.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //assign loan officer role
    ///       if(!externalUser.IsLoanOfficer)
    ///         externalUser.AddRole(ExternalUserRoles.LoanOfficer);
    /// 
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void AddRole(ExternalUserRoles role)
    {
      this.ensureExists();
      ExternalUserInfo.ContactRoles userInfoContactRole = this.ConvertToExternalUserInfoContactRole(role);
      if (((ExternalUserInfo.ContactRoles) this.info.Roles & userInfoContactRole) != userInfoContactRole)
        this.info.Roles += (int) userInfoContactRole;
      this.Commit();
    }

    /// <summary>
    /// Remove <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUserRoles" /> from the user account
    /// </summary>
    /// <param name="role">The role to remove.</param>
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and remove loan officer role.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //remove loan officer role
    ///       if(externalUser.IsLoanOfficer)
    ///         externalUser.RemoveRole(ExternalUserRoles.LoanOfficer);
    /// 
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void RemoveRole(ExternalUserRoles role)
    {
      this.ensureExists();
      ExternalUserInfo.ContactRoles userInfoContactRole = this.ConvertToExternalUserInfoContactRole(role);
      if (((ExternalUserInfo.ContactRoles) this.info.Roles & userInfoContactRole) == userInfoContactRole)
        this.info.Roles -= (int) userInfoContactRole;
      this.Commit();
    }

    /// <summary>
    /// Adds <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> to External User
    /// </summary>
    /// <param name="persona">The role to add.</param>
    /// <include file="ExternalUser.xml" path="Examples/Example[@name=&quot;ExternalUser.AddPersona&quot;]/*" />
    public void AddPersona(Persona persona)
    {
      this.ensureExists();
      if (((IEnumerable<EllieMae.EMLite.Common.Persona>) ((IEnumerable<EllieMae.EMLite.Common.Persona>) this.info.UserPersonas).Where<EllieMae.EMLite.Common.Persona>((Func<EllieMae.EMLite.Common.Persona, bool>) (x => x.ID == persona.ID)).ToArray<EllieMae.EMLite.Common.Persona>()).Count<EllieMae.EMLite.Common.Persona>() > 0)
        return;
      EllieMae.EMLite.Common.Persona[] allPersonas = this.personamgr.GetAllPersonas(new PersonaType[2]
      {
        PersonaType.External,
        PersonaType.BothInternalExternal
      });
      if (((IEnumerable<EllieMae.EMLite.Common.Persona>) allPersonas).Count<EllieMae.EMLite.Common.Persona>() == 0)
        return;
      EllieMae.EMLite.Common.Persona[] selectedPersonas = ((IEnumerable<EllieMae.EMLite.Common.Persona>) allPersonas).Where<EllieMae.EMLite.Common.Persona>((Func<EllieMae.EMLite.Common.Persona, bool>) (x => x.ID == persona.ID)).ToArray<EllieMae.EMLite.Common.Persona>();
      if (((IEnumerable<EllieMae.EMLite.Common.Persona>) selectedPersonas).Count<EllieMae.EMLite.Common.Persona>() == 0 || ((IEnumerable<EllieMae.EMLite.Common.Persona>) ((IEnumerable<EllieMae.EMLite.Common.Persona>) this.info.UserPersonas).Where<EllieMae.EMLite.Common.Persona>((Func<EllieMae.EMLite.Common.Persona, bool>) (x => x.ID == selectedPersonas[0].ID)).ToArray<EllieMae.EMLite.Common.Persona>()).Count<EllieMae.EMLite.Common.Persona>() > 0)
        return;
      List<EllieMae.EMLite.Common.Persona> list = ((IEnumerable<EllieMae.EMLite.Common.Persona>) this.info.UserPersonas).ToList<EllieMae.EMLite.Common.Persona>();
      list.Add(selectedPersonas[0]);
      this.info.UserPersonas = list.ToArray();
      this.Commit();
    }

    /// <summary>
    /// Remove <see cref="T:EllieMae.Encompass.BusinessObjects.Users.Persona" /> to External User
    /// </summary>
    /// <param name="persona">The persona to remove.</param>
    /// <include file="ExternalUser.xml" path="Examples/Example[@name=&quot;ExternalUser.RemovePersona&quot;]/*" />
    public void RemovePersona(Persona persona)
    {
      this.ensureExists();
      if (((IEnumerable<EllieMae.EMLite.Common.Persona>) ((IEnumerable<EllieMae.EMLite.Common.Persona>) this.info.UserPersonas).Where<EllieMae.EMLite.Common.Persona>((Func<EllieMae.EMLite.Common.Persona, bool>) (x => x.ID == persona.ID)).ToArray<EllieMae.EMLite.Common.Persona>()).Count<EllieMae.EMLite.Common.Persona>() == 0)
        return;
      this.info.UserPersonas = ((IEnumerable<EllieMae.EMLite.Common.Persona>) this.info.UserPersonas).Where<EllieMae.EMLite.Common.Persona>((Func<EllieMae.EMLite.Common.Persona, bool>) (x => x.ID != persona.ID)).ToArray<EllieMae.EMLite.Common.Persona>();
      this.Commit();
    }

    /// <summary>
    /// Assign <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl" /> to user account
    /// </summary>
    /// <param name="url">The url to assign to the account.</param>
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and assign all available urls to this account.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //Get a list of url curently assigned to the company.
    ///       ExternalUrlList urlList = externalUser.Company.GetUrlList();
    /// 
    ///       //Assign all possible url to the user
    ///       foreach (ExternalUrl extUrl in urlList)
    ///       {
    ///            if (externalUser.AssignedUrls.Contains(extUrl))
    ///               externalUser.AddUrl(extUrl);
    ///       }
    /// 
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void AddUrl(ExternalUrl url)
    {
      this.ensureExists();
      if (this.urlList == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.AssignedUrls
        });
      List<ExternalUserURL> externalUserUrlList = new List<ExternalUserURL>((IEnumerable<ExternalUserURL>) this.urlList);
      List<int> urlIDs = new List<int>();
      ((IEnumerable<ExternalUserURL>) this.urlList).ToList<ExternalUserURL>().ForEach((Action<ExternalUserURL>) (x => urlIDs.Add(x.URLID)));
      if (urlIDs.Contains(url.URLID))
        return;
      urlIDs.Add(url.URLID);
      this.mngr.SaveExternalUserInfoURLs(this.info.ExternalUserID, urlIDs.ToArray());
      externalUserUrlList.Add(new ExternalUserURL()
      {
        DateAdded = DateTime.Now,
        ExternalUserID = this.info.ExternalUserID,
        URL = url.URL,
        URLID = url.URLID,
        isDeleted = false
      });
      this.urlList = externalUserUrlList.ToArray();
    }

    /// <summary>
    /// Remove <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl" /> from user account
    /// </summary>
    /// <param name="url">The url to remove.</param>
    /// <example>
    ///   The following code retrieves an external user from the Encompass Server and remove the first url starts with https.
    ///   <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
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
    ///       //Get external user from external user id
    ///       ExternalUser externalUser = session.Users.GetExternalUserByExternalID("sampleExternalUesrId");
    /// 
    ///       //Assign all possible url to the user
    ///       foreach (ExternalUrl extUrl in externalUser.AssignedUrls)
    ///       {
    ///           if (extUrl.URL.StartsWith("https:"))
    ///           {
    ///               externalUser.RemoveUrl(extUrl);
    ///               break;
    ///           }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void RemoveUrl(ExternalUrl url)
    {
      this.ensureExists();
      if (this.urlList == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.AssignedUrls
        });
      List<ExternalUserURL> source = new List<ExternalUserURL>((IEnumerable<ExternalUserURL>) this.urlList);
      List<int> urlIDs = new List<int>();
      ((IEnumerable<ExternalUserURL>) this.urlList).ToList<ExternalUserURL>().ForEach((Action<ExternalUserURL>) (x => urlIDs.Add(x.URLID)));
      if (!urlIDs.Contains(url.URLID))
        return;
      urlIDs.Remove(url.URLID);
      this.mngr.SaveExternalUserInfoURLs(this.info.ExternalUserID, urlIDs.ToArray());
      this.urlList = source.ToList<ExternalUserURL>().TakeWhile<ExternalUserURL>((Func<ExternalUserURL, bool>) (x => x.URLID != url.URLID)).ToArray<ExternalUserURL>();
    }

    /// <summary>
    /// Assign a set of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">ExternalUrl</see> to user
    /// </summary>
    /// <param name="newUrlList">The list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">ExternalUrl</see></param>
    public void UpdateUrls(ExternalUrlList newUrlList)
    {
      this.ensureExists();
      if (this.urlList == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.AssignedUrls
        });
      List<int> intList = new List<int>();
      List<ExternalUserURL> currentList = new List<ExternalUserURL>((IEnumerable<ExternalUserURL>) this.urlList);
      List<ExternalUserURL> newList = new List<ExternalUserURL>();
      foreach (ExternalUrl newUrl in (CollectionBase) newUrlList)
        intList.Add(newUrl.URLID);
      this.mngr.SaveExternalUserInfoURLs(this.info.ExternalUserID, intList.ToArray());
      intList.ForEach((Action<int>) (x =>
      {
        ExternalUserURL externalUserUrl = currentList.FirstOrDefault<ExternalUserURL>((Func<ExternalUserURL, bool>) (c => c.URLID == x));
        if (externalUserUrl != null)
        {
          newList.Add(externalUserUrl);
        }
        else
        {
          foreach (ExternalOrgURL externalUrl in this.externalUrlList)
          {
            if (externalUrl.URLID == x)
            {
              newList.Add(new ExternalUserURL()
              {
                DateAdded = externalUrl.DateAdded,
                ExternalUserID = this.info.ExternalUserID,
                isDeleted = false,
                URL = externalUrl.URL,
                URLID = externalUrl.URLID
              });
              break;
            }
          }
        }
      }));
      this.urlList = newList.ToArray();
    }

    private ExternalUserInfo.ContactRoles ConvertToExternalUserInfoContactRole(
      ExternalUserRoles role)
    {
      switch (role)
      {
        case ExternalUserRoles.LoanOfficer:
          return ExternalUserInfo.ContactRoles.LoanOfficer;
        case ExternalUserRoles.LoanProcessor:
          return ExternalUserInfo.ContactRoles.LoanProcessor;
        case ExternalUserRoles.Administrator:
          return ExternalUserInfo.ContactRoles.Administrator;
        default:
          return ExternalUserInfo.ContactRoles.Manager;
      }
    }

    /// <summary>Method to get contact status list</summary>
    /// <returns>The list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CurrentContactStatus">CurrentContactStatus</see></returns>
    public List<CurrentContactStatus> GetContactStatus()
    {
      if (this.contactStatus == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.ContactStatus
        });
      return this.contactStatus;
    }

    /// <summary>Method to get all eligible personas for this user</summary>
    /// <returns>The list of <see cref="T:EllieMae.Encompass.Collections.PersonaList">Personas</see></returns>
    public PersonaList GetAllExternalPersonas()
    {
      return ExternalUser.GetAllExternalPersonas(this.Session);
    }

    /// <summary>Returns a list of all the External Personas</summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public static PersonaList GetAllExternalPersonas(Session session)
    {
      IPersonaManager personaManager = (IPersonaManager) session.GetObject("PersonaManager");
      PersonaList externalPersonas = new PersonaList();
      EllieMae.EMLite.Common.Persona[] allPersonas = personaManager.GetAllPersonas(new PersonaType[2]
      {
        PersonaType.BothInternalExternal,
        PersonaType.External
      });
      if (allPersonas == null || allPersonas.Length == 0)
        return externalPersonas;
      foreach (EllieMae.EMLite.Common.Persona persona in allPersonas)
        externalPersonas.Add(new Persona(session, persona));
      return externalPersonas;
    }

    /// <summary>Returns a list of all the personas</summary>
    /// <returns></returns>
    public PersonaList GetPersonas()
    {
      EllieMae.EMLite.Common.Persona[] infoUserPersonas = this.mngr.GetExternalUserInfoUserPersonas(this.ContactID);
      PersonaList personas = new PersonaList();
      if (infoUserPersonas == null || infoUserPersonas.Length == 0)
        return personas;
      foreach (EllieMae.EMLite.Common.Persona persona in infoUserPersonas)
        personas.Add(new Persona(this.Session, persona));
      return personas;
    }
  }
}
