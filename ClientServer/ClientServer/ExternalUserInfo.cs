// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalUserInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [CLSCompliant(true)]
  [Serializable]
  public class ExternalUserInfo : UserInfo, IPropertyDictionary
  {
    private bool useCompanyAddress;
    private string suffix = string.Empty;
    private string title = string.Empty;
    private string address = string.Empty;
    private string city = string.Empty;
    private string state = string.Empty;
    private string zipcode = string.Empty;
    private string phone = string.Empty;
    private string nmlsID = string.Empty;
    private string ssn = string.Empty;
    private bool useParentInfoForRateLock;
    private string emailForRateSheet = string.Empty;
    private string faxForRateSheet = string.Empty;
    private string emailForLockInfo = string.Empty;
    private string faxForLockInfo = string.Empty;
    private bool disabledLogin;
    private string emailForLogin = string.Empty;
    private string welcomeEmailUserName = string.Empty;
    private DateTime welcomeEmailDate;
    private int approvalCurrentStatus;
    private bool addToWatchlist;
    private DateTime approvalDate = DateTime.MinValue;
    private string salesRepID = string.Empty;
    private int roles;
    private RoleSummaryInfo[] roleSummaries;
    private RoleMappingsDetails[] roleMappings;
    private List<StateLicenseExtType> licenses = new List<StateLicenseExtType>();
    private List<string> roleTypes = new List<string>();
    private string notes = "";
    private bool nMLSCurrent;
    private List<ExternalUserURL> siteURLs = new List<ExternalUserURL>();
    private List<AclGroup> groups = new List<AclGroup>();

    public ExternalUserInfo()
    {
    }

    public ExternalUserInfo(UserInfo userInfo)
      : base(userInfo)
    {
    }

    public string ExternalUserID { get; set; }

    public int ExternalOrgID { get; set; }

    public int RootExternalOrgID { get; set; }

    public string ContactID { get; set; }

    public bool UseCompanyAddress
    {
      get => this.useCompanyAddress;
      set => this.useCompanyAddress = value;
    }

    public string Suffix
    {
      get => this.suffix;
      set => this.suffix = value;
    }

    public string Title
    {
      get => this.title;
      set => this.title = value;
    }

    public string Address
    {
      get => this.address;
      set => this.address = value;
    }

    public string City
    {
      get => this.city;
      set => this.city = value;
    }

    public string State
    {
      get => this.state;
      set => this.state = value;
    }

    public string Zipcode
    {
      get => this.zipcode;
      set => this.zipcode = value;
    }

    public new string Phone
    {
      get => this.phone;
      set => this.phone = value;
    }

    public string NmlsID
    {
      get => this.nmlsID;
      set => this.nmlsID = value;
    }

    public string SSN
    {
      get => this.ssn;
      set => this.ssn = value;
    }

    public bool UseParentInfoForRateLock
    {
      get => this.useParentInfoForRateLock;
      set => this.useParentInfoForRateLock = value;
    }

    public string EmailForRateSheet
    {
      get => this.emailForRateSheet;
      set => this.emailForRateSheet = value;
    }

    public string FaxForRateSheet
    {
      get => this.faxForRateSheet;
      set => this.faxForRateSheet = value;
    }

    public string EmailForLockInfo
    {
      get => this.emailForLockInfo;
      set => this.emailForLockInfo = value;
    }

    public string FaxForLockInfo
    {
      get => this.faxForLockInfo;
      set => this.faxForLockInfo = value;
    }

    public bool DisabledLogin
    {
      get => this.disabledLogin;
      set => this.disabledLogin = value;
    }

    public string EmailForLogin
    {
      get => this.emailForLogin;
      set => this.emailForLogin = value;
    }

    public string WelcomeEmailUserName
    {
      get => this.welcomeEmailUserName;
      set => this.welcomeEmailUserName = value;
    }

    public DateTime WelcomeEmailDate
    {
      get => this.welcomeEmailDate;
      set => this.welcomeEmailDate = value;
    }

    public int ApprovalCurrentStatus
    {
      get => this.approvalCurrentStatus;
      set => this.approvalCurrentStatus = value;
    }

    public bool AddToWatchlist
    {
      get => this.addToWatchlist;
      set => this.addToWatchlist = value;
    }

    public DateTime ApprovalCurrentStatusDate { get; set; }

    public DateTime ApprovalDate
    {
      get => this.approvalDate;
      set => this.approvalDate = value;
    }

    public string SalesRepID
    {
      get => this.salesRepID;
      set => this.salesRepID = value;
    }

    public string GenerateNewPassword() => Guid.NewGuid().ToString().Split('-')[0];

    public int Roles
    {
      get => this.roles;
      set => this.roles = value;
    }

    public RoleSummaryInfo[] RoleSummaries
    {
      get => this.roleSummaries;
      set => this.roleSummaries = value;
    }

    public RoleMappingsDetails[] RoleMappings
    {
      get => this.roleMappings;
      set => this.roleMappings = value;
    }

    public List<StateLicenseExtType> Licenses
    {
      get => this.licenses;
      set => this.licenses = value;
    }

    public List<string> RoleTypes
    {
      get => this.roleTypes;
      set => this.roleTypes = value;
    }

    public string Notes
    {
      get => this.notes;
      set => this.notes = value;
    }

    public bool NMLSCurrent
    {
      get => this.nMLSCurrent;
      set => this.nMLSCurrent = value;
    }

    public string UpdatedBy { get; set; }

    public bool UpdatedByExternal { get; set; }

    public DateTime UpdatedDateTime { get; set; }

    public string UpdatedByExternalAdmin { get; set; }

    public ExternalOriginatorOrgType OrganizationType { get; set; }

    public bool OrgVisibleOnTpowcSite { get; set; }

    public List<ExternalUserURL> SiteURLs
    {
      get => this.siteURLs;
      set => this.siteURLs = value;
    }

    public List<AclGroup> Groups
    {
      get => this.groups;
      set => this.groups = value;
    }

    public static long NewContactID(List<long> allContactId)
    {
      int num1 = 0;
      int num2 = 0;
      long num3;
      do
      {
        using (CryptoRandom cryptoRandom = new CryptoRandom())
        {
          num1 = cryptoRandom.Next(10000000, 99999999);
          num2 = cryptoRandom.Next(10, 99);
        }
        num3 = long.Parse(num1.ToString() + num2.ToString());
      }
      while (allContactId != null && allContactId.Contains(num3));
      return num3;
    }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "address1":
            return (object) this.address;
          case "approval_date":
            return (object) this.approvalDate;
          case "approval_status":
            return (object) this.approvalCurrentStatus;
          case "approval_status_date":
            return (object) this.ApprovalCurrentStatusDate;
          case "approval_status_watchlist":
            return (object) this.addToWatchlist;
          case "cell_phone":
            return (object) this.CellPhone;
          case "city":
            return (object) this.city;
          case "disabledlogin":
            return (object) this.disabledLogin;
          case "email":
            return (object) this.Email;
          case "fax":
            return (object) this.Fax;
          case "first_name":
            return (object) this.FirstName;
          case "inheritparentratesheet":
            return (object) this.useParentInfoForRateLock;
          case "last_name":
            return (object) this.LastName;
          case "lock_info_email":
            return (object) this.emailForLockInfo;
          case "lock_info_fax":
            return (object) this.faxForLockInfo;
          case "login_email":
            return (object) this.emailForLogin;
          case "middle_name":
            return (object) this.MiddleName;
          case "nmlscurrent":
            return (object) this.nMLSCurrent;
          case "nmlsoriginatorid":
            return (object) this.nmlsID;
          case "note":
            return (object) this.notes;
          case "phone":
            return (object) this.phone;
          case "rate_sheet_email":
            return (object) this.emailForRateSheet;
          case "rate_sheet_fax":
            return (object) this.faxForRateSheet;
          case "roles":
            return (object) this.roles;
          case "sales_rep_userid":
            return (object) this.salesRepID;
          case "ssn":
            return (object) this.ssn;
          case "state":
            return (object) this.state;
          case "suffix_name":
            return (object) this.suffix;
          case "title":
            return (object) this.title;
          case "usecompanyaddress":
            return (object) this.useCompanyAddress;
          case "zip":
            return (object) this.zipcode;
          default:
            return (object) null;
        }
      }
      set
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "address1":
            this.address = string.Concat(value);
            break;
          case "approval_date":
            this.approvalDate = Utils.ParseDate((object) value.ToString());
            break;
          case "approval_status":
            this.approvalCurrentStatus = (int) value;
            break;
          case "approval_status_date":
            this.ApprovalCurrentStatusDate = Utils.ParseDate((object) value.ToString());
            break;
          case "approval_status_watchlist":
            this.addToWatchlist = (bool) value;
            break;
          case "cell_phone":
            this.CellPhone = string.Concat(value);
            break;
          case "city":
            this.city = string.Concat(value);
            break;
          case "disabledlogin":
            this.disabledLogin = (bool) value;
            break;
          case "email":
            this.Email = string.Concat(value);
            break;
          case "fax":
            this.Fax = string.Concat(value);
            break;
          case "first_name":
            this.FirstName = string.Concat(value);
            break;
          case "inheritparentratesheet":
            this.useParentInfoForRateLock = (bool) value;
            break;
          case "last_name":
            this.LastName = string.Concat(value);
            break;
          case "lock_info_email":
            this.emailForLockInfo = string.Concat(value);
            break;
          case "lock_info_fax":
            this.faxForLockInfo = string.Concat(value);
            break;
          case "login_email":
            this.emailForLogin = string.Concat(value);
            break;
          case "middle_name":
            this.MiddleName = string.Concat(value);
            break;
          case "nmlscurrent":
            this.nMLSCurrent = (bool) value;
            break;
          case "nmlsoriginatorid":
            this.nmlsID = string.Concat(value);
            break;
          case "note":
            this.notes = string.Concat(value);
            break;
          case "phone":
            this.phone = string.Concat(value);
            break;
          case "rate_sheet_email":
            this.emailForRateSheet = string.Concat(value);
            break;
          case "rate_sheet_fax":
            this.faxForRateSheet = string.Concat(value);
            break;
          case "roles":
            this.roles = (int) value;
            break;
          case "sales_rep_userid":
            this.salesRepID = string.Concat(value);
            break;
          case "ssn":
            this.ssn = string.Concat(value);
            break;
          case "state":
            this.state = string.Concat(value);
            break;
          case "suffix_name":
            this.suffix = string.Concat(value);
            break;
          case "title":
            this.title = string.Concat(value);
            break;
          case "usecompanyaddress":
            this.useCompanyAddress = (bool) value;
            break;
          case "zip":
            this.zipcode = string.Concat(value);
            break;
          default:
            throw new ArgumentException("Invalid field name \"" + columnName + "\"");
        }
      }
    }

    public enum ContactRoles : byte
    {
      None = 0,
      LoanOfficer = 1,
      LoanProcessor = 2,
      Manager = 4,
      Administrator = 8,
    }

    public enum ExternalUserInfoSetting
    {
      UpdatingUser,
      AccessibleUserList,
      License,
      AssignedUrls,
      ContactStatus,
      SalesRep,
    }
  }
}
