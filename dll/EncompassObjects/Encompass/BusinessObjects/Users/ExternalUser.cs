// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Users.ExternalUser
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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
using System.Diagnostics;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Users
{
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

    public string ID
    {
      get
      {
        this.ensureExists();
        return this.info.ExternalUserID;
      }
    }

    public string FirstName
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).FirstName;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).FirstName = value;
      }
    }

    public string MiddleName
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).MiddleName;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).MiddleName = value;
      }
    }

    public string LastName
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).LastName;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).LastName = value;
      }
    }

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

    public string CellPhone
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).CellPhone;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).CellPhone = value;
      }
    }

    public string Fax
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).Fax;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).Fax = value;
      }
    }

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

    public string Email
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).Email;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).Email = value;
      }
    }

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

    public User SalesRep
    {
      get
      {
        this.ensureExists();
        if (this.info.SalesRepID == "")
          return (User) null;
        if (UserInfo.op_Equality(this.salesRep, (UserInfo) null))
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

    public int LoginAttempts
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).failed_login_attempts;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).failed_login_attempts = value;
      }
    }

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

    public DateTime LastLogin
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).LastLogin;
      }
      set
      {
        this.ensureExists();
        ((UserInfo) this.info).LastLogin = value;
      }
    }

    public int ExternalOrganizationId
    {
      get
      {
        this.ensureExists();
        return this.info.ExternalOrgID;
      }
    }

    public bool Enabled
    {
      get
      {
        this.ensureExists();
        return !this.info.DisabledLogin;
      }
    }

    public DateTime PasswordChangedDate
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).PasswordChangedDate;
      }
    }

    public bool RequirePasswordChange
    {
      get
      {
        this.ensureExists();
        return ((UserInfo) this.info).RequirePasswordChange;
      }
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization ParentOrganization
    {
      get => this.parentOrganization;
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company
    {
      get => this.companyOrganization;
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch
    {
      get => this.branchOrganization;
    }

    public bool IsLoanOfficer => (this.info.Roles & 1) == 1;

    public bool IsLoanProcessor => (this.info.Roles & 2) == 2;

    public bool IsManager => (this.info.Roles & 4) == 4;

    public bool IsAdministrator => (this.info.Roles & 8) == 8;

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

    public string ContactID
    {
      get
      {
        this.ensureExists();
        return this.info.ContactID;
      }
    }

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

    public string UpdatedBy
    {
      get
      {
        this.ensureExists();
        return this.info.UpdatedBy;
      }
    }

    public string UpdatedByFirstName
    {
      get
      {
        return UserInfo.op_Inequality((UserInfo) this.updatingExtUser, (UserInfo) null) ? ((UserInfo) this.updatingExtUser).FirstName : "";
      }
    }

    public string UpdatedByMiddleName
    {
      get
      {
        return UserInfo.op_Inequality((UserInfo) this.updatingExtUser, (UserInfo) null) ? ((UserInfo) this.updatingExtUser).MiddleName : "";
      }
    }

    public string UpdatedByLastName
    {
      get
      {
        return UserInfo.op_Inequality((UserInfo) this.updatingExtUser, (UserInfo) null) ? ((UserInfo) this.updatingExtUser).LastName : "";
      }
    }

    public string UpdatedByEmail
    {
      get
      {
        return UserInfo.op_Inequality((UserInfo) this.updatingExtUser, (UserInfo) null) ? ((UserInfo) this.updatingExtUser).Email : "";
      }
    }

    public DateTime UpdatedDateTime
    {
      get
      {
        this.ensureExists();
        return this.info.UpdatedDateTime;
      }
    }

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

    public void BatchLoadSettings(List<ExternalUserSetting> settings)
    {
      List<ExternalUserInfo.ExternalUserInfoSetting> externalUserInfoSettingList = new List<ExternalUserInfo.ExternalUserInfoSetting>();
      if (settings.Contains(ExternalUserSetting.AccessibleUserList) && this.accessibleUserList == null)
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 1);
      if (settings.Contains(ExternalUserSetting.AssignedUrls) && (this.urlList == null || this.externalUrlList == null))
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 3);
      if (settings.Contains(ExternalUserSetting.License) && this.licensing == null)
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 2);
      if (settings.Contains(ExternalUserSetting.UpdatingUser) && UserInfo.op_Equality((UserInfo) this.updatingExtUser, (UserInfo) null))
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 0);
      if (settings.Contains(ExternalUserSetting.ContactStatus) && this.contactStatus == null)
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 4);
      if (settings.Contains(ExternalUserSetting.SalesRep) && UserInfo.op_Equality(this.salesRep, (UserInfo) null))
        externalUserInfoSettingList.Add((ExternalUserInfo.ExternalUserInfoSetting) 5);
      Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> dictionary = new Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object>();
      if (this.hasPerformancePatch)
      {
        dictionary = this.mngr.GetExternalAdditionalDetails(this.ID, externalUserInfoSettingList);
      }
      else
      {
        if (settings.Contains(ExternalUserSetting.AssignedUrls) || settings.Contains(ExternalUserSetting.License))
        {
          List<object> additionalDetails = this.mngr.GetExternalAdditionalDetails(this.ID);
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 3, (object) new List<object>()
          {
            additionalDetails[1],
            additionalDetails[2]
          });
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 2, additionalDetails[0]);
        }
        if (settings.Contains(ExternalUserSetting.AccessibleUserList))
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 1, (object) new List<object>()
          {
            (object) this.mngr.GetAccessibleExternalUserInfos(this.ID),
            (object) new Dictionary<string, List<string>>(),
            (object) new Dictionary<string, List<string>>()
          });
        if (settings.Contains(ExternalUserSetting.UpdatingUser) && UserInfo.op_Equality((UserInfo) this.updatingExtUser, (UserInfo) null))
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 0, (object) this.mngr.GetExternalUserInfo(this.info.UpdatedBy));
        if (settings.Contains(ExternalUserSetting.ContactStatus) && this.contactStatus == null)
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 4, (object) this.mngr.GetExternalOrgSettingsByName("Current Contact Status"));
        if (settings.Contains(ExternalUserSetting.SalesRep) && UserInfo.op_Equality(this.salesRep, (UserInfo) null))
        {
          IOrganizationManager iorganizationManager = (IOrganizationManager) this.Session.GetObject("OrganizationManager");
          dictionary.Add((ExternalUserInfo.ExternalUserInfoSetting) 5, (object) iorganizationManager.GetUser(this.info.SalesRepID));
        }
      }
      if (settings.Contains(ExternalUserSetting.AccessibleUserList))
      {
        List<object> objectList = (List<object>) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 1];
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
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>((ExternalUserInfo.ExternalUserInfoSetting) 3) && (this.urlList == null || this.externalUrlList == null))
      {
        List<object> objectList = (List<object>) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 3];
        this.urlList = (ExternalUserURL[]) objectList[0];
        this.externalUrlList = (List<ExternalOrgURL>) objectList[1];
      }
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>((ExternalUserInfo.ExternalUserInfoSetting) 2))
      {
        BranchExtLicensing licensing = (BranchExtLicensing) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 2];
        if (licensing != null)
        {
          this.licensing = new ExternalLicensing(licensing);
          if (this.info.Licenses != null)
            this.info.Licenses.Clear();
          this.info.Licenses = licensing.StateLicenseExtTypes;
        }
      }
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>((ExternalUserInfo.ExternalUserInfoSetting) 0) && UserInfo.op_Equality((UserInfo) this.updatingExtUser, (UserInfo) null))
        this.updatingExtUser = (ExternalUserInfo) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 0];
      if (dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>((ExternalUserInfo.ExternalUserInfoSetting) 4) && this.contactStatus == null)
      {
        List<ExternalSettingValue> externalSettingValueList = (List<ExternalSettingValue>) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 4];
        this.contactStatus = new List<CurrentContactStatus>();
        externalSettingValueList.ForEach((Action<ExternalSettingValue>) (x => this.contactStatus.Add(new CurrentContactStatus(this.Session, x.settingValue, x.settingId))));
      }
      if (!dictionary.Keys.Contains<ExternalUserInfo.ExternalUserInfoSetting>((ExternalUserInfo.ExternalUserInfoSetting) 5) || !UserInfo.op_Equality(this.salesRep, (UserInfo) null))
        return;
      this.salesRep = (UserInfo) dictionary[(ExternalUserInfo.ExternalUserInfoSetting) 5];
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

    public ExternalUser UpdatingExternalUser
    {
      set
      {
        this.ensureExists();
        this.updatingExtUser = value.info;
        this.info.UpdatedByExternalAdmin = UserInfo.op_Inequality((UserInfo) this.updatingExtUser, (UserInfo) null) ? this.updatingExtUser.ContactID : "";
      }
    }

    private void ensureExists()
    {
      if (UserInfo.op_Equality((UserInfo) this.info, (UserInfo) null))
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
        this.info.Licenses.Add(new StateLicenseExtType(newStateLicenses[index].StateAbbrevation, newStateLicenses[index].LicenseType, newStateLicenses[index].LicenseNo, newStateLicenses[index].IssueDate, newStateLicenses[index].StartDate, newStateLicenses[index].EndDate, newStateLicenses[index].LicenseStatus, newStateLicenses[index].StatusDate, newStateLicenses[index].Approved, newStateLicenses[index].Exempt, newStateLicenses[index].LastChecked, newStateLicenses[index].SortIndex));
        this.licensing.AddStateLicenseExtType(newStateLicenses[index]);
      }
    }

    public void Commit()
    {
      this.ensureExists();
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("Access denied");
      List<ExternalUserInfo> contactsByLoginEmailId = this.mngr.GetAllContactsByLoginEmailId(this.EmailForLogin, "");
      if (contactsByLoginEmailId != null && !this.info.DisabledLogin)
      {
        ExternalUserInfo externalUserInfo = contactsByLoginEmailId.FirstOrDefault<ExternalUserInfo>((Func<ExternalUserInfo, bool>) (item => item.ExternalUserID == this.ID));
        if (UserInfo.op_Inequality((UserInfo) externalUserInfo, (UserInfo) null))
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
      this.info = this.mngr.SaveExternalUserInfo(this.info, true, (int[]) null);
      this.isNew = false;
      this.committed((object) this, new PersistentObjectEventArgs(this.ID));
    }

    public void Delete()
    {
      this.ensureExists();
      this.mngr.DeleteExternalUserInfo(this.info.ExternalOrgID, this.info, this.Session.GetUserInfo());
    }

    public string ResetPassword()
    {
      this.ensureExists();
      string newPassword = this.info.GenerateNewPassword();
      this.Commit();
      this.info = this.mngr.ResetExternalUserInfoPassword(this.info.ExternalUserID, newPassword, DateTime.Now, true);
      return newPassword;
    }

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

    public void Enable()
    {
      if (!UserInfo.op_Inequality((UserInfo) this.info, (UserInfo) null))
        return;
      this.info.DisabledLogin = false;
      this.Commit();
    }

    public void Disable()
    {
      if (!UserInfo.op_Inequality((UserInfo) this.info, (UserInfo) null))
        return;
      this.info.DisabledLogin = true;
      ((UserInfo) this.info).failed_login_attempts = 0;
      this.Commit();
    }

    public void AddRole(ExternalUserRoles role)
    {
      this.ensureExists();
      ExternalUserInfo.ContactRoles userInfoContactRole = this.ConvertToExternalUserInfoContactRole(role);
      if ((this.info.Roles & userInfoContactRole) != userInfoContactRole)
        this.info.Roles += (int) userInfoContactRole;
      this.Commit();
    }

    public void RemoveRole(ExternalUserRoles role)
    {
      this.ensureExists();
      ExternalUserInfo.ContactRoles userInfoContactRole = this.ConvertToExternalUserInfoContactRole(role);
      if ((this.info.Roles & userInfoContactRole) == userInfoContactRole)
        this.info.Roles -= (int) userInfoContactRole;
      this.Commit();
    }

    public void AddPersona(Persona persona)
    {
      this.ensureExists();
      if (((IEnumerable<Persona>) ((IEnumerable<Persona>) ((UserInfo) this.info).UserPersonas).Where<Persona>((Func<Persona, bool>) (x => x.ID == persona.ID)).ToArray<Persona>()).Count<Persona>() > 0)
        return;
      Persona[] allPersonas = this.personamgr.GetAllPersonas(new PersonaType[2]
      {
        (PersonaType) 2,
        (PersonaType) 3
      });
      if (((IEnumerable<Persona>) allPersonas).Count<Persona>() == 0)
        return;
      Persona[] selectedPersonas = ((IEnumerable<Persona>) allPersonas).Where<Persona>((Func<Persona, bool>) (x => x.ID == persona.ID)).ToArray<Persona>();
      if (((IEnumerable<Persona>) selectedPersonas).Count<Persona>() == 0 || ((IEnumerable<Persona>) ((IEnumerable<Persona>) ((UserInfo) this.info).UserPersonas).Where<Persona>((Func<Persona, bool>) (x => x.ID == selectedPersonas[0].ID)).ToArray<Persona>()).Count<Persona>() > 0)
        return;
      List<Persona> list = ((IEnumerable<Persona>) ((UserInfo) this.info).UserPersonas).ToList<Persona>();
      list.Add(selectedPersonas[0]);
      ((UserInfo) this.info).UserPersonas = list.ToArray();
      this.Commit();
    }

    public void RemovePersona(Persona persona)
    {
      this.ensureExists();
      if (((IEnumerable<Persona>) ((IEnumerable<Persona>) ((UserInfo) this.info).UserPersonas).Where<Persona>((Func<Persona, bool>) (x => x.ID == persona.ID)).ToArray<Persona>()).Count<Persona>() == 0)
        return;
      ((UserInfo) this.info).UserPersonas = ((IEnumerable<Persona>) ((UserInfo) this.info).UserPersonas).Where<Persona>((Func<Persona, bool>) (x => x.ID != persona.ID)).ToArray<Persona>();
      this.Commit();
    }

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
          return (ExternalUserInfo.ContactRoles) 1;
        case ExternalUserRoles.LoanProcessor:
          return (ExternalUserInfo.ContactRoles) 2;
        case ExternalUserRoles.Administrator:
          return (ExternalUserInfo.ContactRoles) 8;
        default:
          return (ExternalUserInfo.ContactRoles) 4;
      }
    }

    public List<CurrentContactStatus> GetContactStatus()
    {
      if (this.contactStatus == null)
        this.getDetailInfo(new List<ExternalUserSetting>()
        {
          ExternalUserSetting.ContactStatus
        });
      return this.contactStatus;
    }

    public PersonaList GetAllExternalPersonas()
    {
      return ExternalUser.GetAllExternalPersonas(this.Session);
    }

    public static PersonaList GetAllExternalPersonas(Session session)
    {
      IPersonaManager ipersonaManager = (IPersonaManager) session.GetObject("PersonaManager");
      PersonaList externalPersonas = new PersonaList();
      Persona[] allPersonas = ipersonaManager.GetAllPersonas(new PersonaType[2]
      {
        (PersonaType) 3,
        (PersonaType) 2
      });
      if (allPersonas == null || allPersonas.Length == 0)
        return externalPersonas;
      foreach (Persona persona in allPersonas)
        externalPersonas.Add(new Persona(session, persona));
      return externalPersonas;
    }

    public PersonaList GetPersonas()
    {
      Persona[] infoUserPersonas = this.mngr.GetExternalUserInfoUserPersonas(this.ContactID);
      PersonaList personas = new PersonaList();
      if (infoUserPersonas == null || infoUserPersonas.Length == 0)
        return personas;
      foreach (Persona persona in infoUserPersonas)
        personas.Add(new Persona(this.Session, persona));
      return personas;
    }
  }
}
