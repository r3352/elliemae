// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.UserInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.Common;
using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [CLSCompliant(true)]
  [Serializable]
  public class UserInfo : ICloneable, IComparable
  {
    public const string TrustedUserID = "(trusted)";
    public const string TrustedAppID = "trustedEnc";
    public const string AdminUserID = "admin";
    public const string RemoteMgmtUserID = "(rmi)";
    public const string TPOWebCenterAdminUserID = "tpowcadmin";
    public const int MinUserIDLength = 1;
    public const int MaxUserIDLength = 16;
    private string userid;
    private string runAsUserId;
    private string lastName;
    private string firstName;
    private string middleName;
    private string suffixName;
    private string jobtitle;
    private string employeeID;
    private string password;
    private string profileURL;
    private Persona[] personas;
    private string workingFolder;
    private int orgId;
    private bool isInRootOrg;
    private UserInfo.AccessModeEnum accessMode;
    private UserInfo.UserStatusEnum status;
    private string email;
    private string phone;
    private string cellPhone;
    private string fax;
    private string chumID;
    private string nmlsOriginatorID;
    private DateTime nmlsExpirationDate = DateTime.MaxValue;
    private bool pwdNeverExpires;
    private bool requirePwdChange;
    private DateTime passwordChangedDate = DateTime.MinValue;
    private bool locked;
    private UserInfo.UserPeerView peerView;
    private string dataServicesOptOutKey = "";
    private bool delegateTasksRight;
    private DateTime lastLogin = DateTime.MinValue;
    private string encompassVersion;
    private string emailSignature = string.Empty;
    private bool personalStatusOnline;
    private string personaAccessComments = string.Empty;
    private bool inheritParentCompPlan;
    private string userType = "";
    private bool apiUser;
    private string oAuthClientId;
    private bool allowImpersonation;
    private bool ssoOnly;
    private bool ssoDisconnectedFromOrg;
    private bool passwordRequired;
    private bool passwordExists;
    public string file_transfer_right = string.Empty;
    public string tmp_table_right = string.Empty;
    public string tracking_setup_right = string.Empty;
    public string reports_right = string.Empty;
    public string myepass_custom_right = string.Empty;
    public string offline_right = string.Empty;
    public int failed_login_attempts;
    public string lo_license = string.Empty;
    public string contact_export_right = string.Empty;
    public string plan_code_right = string.Empty;
    public string alt_lender_right = string.Empty;
    public string scope_lo = string.Empty;
    public string scope_lp = string.Empty;
    public string scope_closer = string.Empty;
    public string pipelineView = string.Empty;
    public string enc_version = string.Empty;
    public string firstLastName = string.Empty;
    public string userName = string.Empty;
    private bool inheritParentCCSite;
    private DateTime? lastLockedOutDateTime;
    public DateTime? createdDate;
    public string createdBy = string.Empty;
    public DateTime? lastModifiedDate;
    public string lastModifiedBy = string.Empty;
    public Dictionary<UserInfo.ContextDataId, object> ContextData = new Dictionary<UserInfo.ContextDataId, object>();

    public ClaimsIdentity BorrowerContext
    {
      get
      {
        if (Thread.CurrentPrincipal == null)
          return (ClaimsIdentity) null;
        return !(Thread.CurrentPrincipal is ClaimsPrincipal) ? (ClaimsIdentity) null : (ClaimsIdentity) ((ClaimsPrincipal) Thread.CurrentPrincipal).Identity;
      }
    }

    public UserInfo(
      string userid,
      string password,
      string lastName,
      string suffixName,
      string firstName,
      string middleName,
      string employeeID,
      string profileURL,
      Persona[] personas,
      string workingFolder,
      int orgId,
      bool isInRootOrg,
      UserInfo.AccessModeEnum accessMode,
      UserInfo.UserStatusEnum status,
      string email,
      string phone,
      string cellPhone,
      string fax,
      bool pwdNeverExpires,
      bool requirePwdChange,
      bool locked,
      UserInfo.UserPeerView peerView,
      string dataServicesOptOutKey,
      bool delegateTasksRight,
      DateTime lastLogin,
      string chumID,
      string nmlsOriginatorID,
      DateTime nmlsExpirationDate,
      string encompassVersion,
      string emailSignature,
      bool personalStatusOnline,
      bool inheritParentCompPlan,
      bool apiUser,
      string oAuthClientId,
      bool allowImpersonation,
      bool inheritParentCCSite,
      DateTime password_Change = default (DateTime),
      string personaAccessComments = "",
      DateTime? lastLockedOutDateTime = null,
      bool ssoOnly = false,
      bool ssoDisconnectedFromOrg = false,
      bool passwordRequired = false,
      bool passwordExists = true,
      DateTime? createdDate = null,
      string createdBy = "",
      DateTime? lastModifiedDate = null,
      string lastModifiedBy = "")
    {
      this.userid = (userid ?? "").ToLower().Trim();
      this.runAsUserId = this.IsVirtualUser() ? "admin" : this.userid;
      this.password = password;
      this.lastName = lastName ?? "";
      this.suffixName = suffixName ?? "";
      this.firstName = firstName ?? "";
      this.middleName = middleName ?? "";
      this.employeeID = employeeID ?? "";
      this.profileURL = profileURL ?? "";
      this.workingFolder = workingFolder ?? "";
      this.orgId = orgId;
      this.isInRootOrg = isInRootOrg;
      this.accessMode = accessMode;
      this.status = status;
      this.email = email ?? "";
      this.phone = phone ?? "";
      this.cellPhone = cellPhone ?? "";
      this.fax = fax ?? "";
      this.pwdNeverExpires = pwdNeverExpires;
      this.requirePwdChange = requirePwdChange;
      this.locked = locked;
      this.peerView = peerView;
      this.personas = personas;
      this.dataServicesOptOutKey = dataServicesOptOutKey;
      this.delegateTasksRight = delegateTasksRight;
      this.lastLogin = lastLogin;
      this.chumID = chumID ?? "";
      this.nmlsOriginatorID = nmlsOriginatorID ?? "";
      this.nmlsExpirationDate = nmlsExpirationDate;
      this.encompassVersion = encompassVersion ?? "";
      this.emailSignature = emailSignature ?? "";
      this.personalStatusOnline = personalStatusOnline;
      this.inheritParentCompPlan = inheritParentCompPlan;
      this.apiUser = apiUser;
      this.oAuthClientId = oAuthClientId;
      this.allowImpersonation = allowImpersonation;
      this.inheritParentCCSite = inheritParentCCSite;
      this.passwordChangedDate = password_Change;
      this.PersonaAccessComments = personaAccessComments;
      this.lastLockedOutDateTime = lastLockedOutDateTime;
      this.ssoOnly = ssoOnly;
      this.ssoDisconnectedFromOrg = ssoDisconnectedFromOrg;
      this.passwordRequired = passwordRequired;
      this.passwordExists = passwordExists;
      this.createdDate = createdDate;
      this.createdBy = createdBy;
      this.lastModifiedDate = lastModifiedDate;
      this.lastModifiedBy = lastModifiedBy;
    }

    public UserInfo(
      string userid,
      string password,
      string lastName,
      string suffixName,
      string firstName,
      string middleName,
      string employeeID,
      string profileURL,
      Persona[] personas,
      string workingFolder,
      int orgId,
      bool isInRootOrg,
      UserInfo.AccessModeEnum accessMode,
      UserInfo.UserStatusEnum status,
      string email,
      string phone,
      string cellPhone,
      string fax,
      bool pwdNeverExpires,
      bool requirePwdChange,
      bool locked,
      UserInfo.UserPeerView peerView,
      string dataServicesOptOutKey,
      bool delegateTasksRight,
      DateTime lastLogin,
      string chumID,
      string nmlsOriginatorID,
      DateTime nmlsExpirationDate,
      string encompassVersion,
      string emailSignature,
      bool personalStatusOnline,
      bool inheritParentCompPlan,
      bool apiUser,
      string oAuthClientId,
      bool allowImpersonation,
      bool inheritParentCCSite,
      string file_transfer_right,
      string tmp_table_right,
      string tracking_setup_right,
      string reports_right,
      string myepass_custom_right,
      string offline_right,
      int failed_login_attempts,
      string lo_license,
      string contact_export_right,
      string plan_code_right,
      string alt_lender_right,
      string scope_lo,
      string scope_lp,
      string scope_closer,
      string pipelineView,
      string enc_version,
      string firstLastName,
      string userName,
      DateTime password_Change = default (DateTime),
      string personaAccessComments = "",
      DateTime? lastLockedOutDateTime = null,
      bool ssoOnly = false,
      bool ssoDisconnectedFromOrg = false,
      bool passwordRequired = false,
      bool passwordExists = true,
      DateTime? createdDate = null,
      string createdBy = "",
      DateTime? lastModifiedDate = null,
      string lastModifiedBy = "")
    {
      this.userid = (userid ?? "").ToLower().Trim();
      this.runAsUserId = this.IsVirtualUser() ? "admin" : this.userid;
      this.password = password;
      this.lastName = lastName ?? "";
      this.suffixName = suffixName ?? "";
      this.firstName = firstName ?? "";
      this.middleName = middleName ?? "";
      this.employeeID = employeeID ?? "";
      this.profileURL = profileURL ?? "";
      this.workingFolder = workingFolder ?? "";
      this.orgId = orgId;
      this.isInRootOrg = isInRootOrg;
      this.accessMode = accessMode;
      this.status = status;
      this.email = email ?? "";
      this.phone = phone ?? "";
      this.cellPhone = cellPhone ?? "";
      this.fax = fax ?? "";
      this.pwdNeverExpires = pwdNeverExpires;
      this.requirePwdChange = requirePwdChange;
      this.locked = locked;
      this.peerView = peerView;
      this.personas = personas;
      this.dataServicesOptOutKey = dataServicesOptOutKey;
      this.delegateTasksRight = delegateTasksRight;
      this.lastLogin = lastLogin;
      this.chumID = chumID ?? "";
      this.nmlsOriginatorID = nmlsOriginatorID ?? "";
      this.nmlsExpirationDate = nmlsExpirationDate;
      this.encompassVersion = encompassVersion ?? "";
      this.emailSignature = emailSignature ?? "";
      this.personalStatusOnline = personalStatusOnline;
      this.inheritParentCompPlan = inheritParentCompPlan;
      this.apiUser = apiUser;
      this.oAuthClientId = oAuthClientId;
      this.allowImpersonation = allowImpersonation;
      this.inheritParentCCSite = inheritParentCCSite;
      this.passwordChangedDate = password_Change;
      this.PersonaAccessComments = personaAccessComments;
      this.file_transfer_right = file_transfer_right;
      this.tmp_table_right = tmp_table_right;
      this.tracking_setup_right = tracking_setup_right;
      this.reports_right = reports_right;
      this.myepass_custom_right = myepass_custom_right;
      this.offline_right = offline_right;
      this.failed_login_attempts = failed_login_attempts;
      this.lo_license = lo_license;
      this.contact_export_right = contact_export_right;
      this.plan_code_right = plan_code_right;
      this.alt_lender_right = alt_lender_right;
      this.scope_lo = scope_lo;
      this.scope_lp = scope_lp;
      this.scope_closer = scope_closer;
      this.pipelineView = pipelineView;
      this.enc_version = enc_version;
      this.firstLastName = firstLastName;
      this.userName = userName;
      this.lastLockedOutDateTime = lastLockedOutDateTime;
      this.ssoOnly = ssoOnly;
      this.ssoDisconnectedFromOrg = ssoDisconnectedFromOrg;
      this.passwordRequired = passwordRequired;
      this.passwordExists = passwordExists;
      this.createdDate = createdDate;
      this.createdBy = createdBy;
      this.lastModifiedDate = lastModifiedDate;
      this.lastModifiedBy = lastModifiedBy;
    }

    public UserInfo(string userId, string password, int orgId, Persona[] personas)
    {
      this.userid = (userId ?? "").ToLower().Trim();
      this.runAsUserId = this.IsVirtualUser() ? "admin" : this.userid;
      this.orgId = orgId;
      this.isInRootOrg = false;
      this.password = password;
      this.lastName = "";
      this.suffixName = "";
      this.firstName = "";
      this.middleName = "";
      this.workingFolder = "";
      this.accessMode = UserInfo.AccessModeEnum.ReadOnly;
      this.status = UserInfo.UserStatusEnum.Enabled;
      this.email = "";
      this.phone = "";
      this.cellPhone = "";
      this.fax = "";
      this.profileURL = "";
      this.peerView = UserInfo.UserPeerView.Disabled;
      this.personas = personas;
      this.nmlsOriginatorID = "";
      this.nmlsExpirationDate = DateTime.MaxValue;
      this.encompassVersion = "";
      this.emailSignature = "";
      this.personalStatusOnline = false;
      this.inheritParentCompPlan = false;
      this.apiUser = false;
      this.oAuthClientId = (string) null;
      this.allowImpersonation = false;
      this.inheritParentCCSite = true;
      this.ssoOnly = false;
      this.ssoDisconnectedFromOrg = false;
      this.passwordRequired = false;
      this.passwordExists = true;
      this.createdDate = new DateTime?();
      this.createdBy = "";
      this.lastModifiedDate = new DateTime?();
      this.lastModifiedBy = "";
    }

    public UserInfo(UserInfo source)
    {
      this.userid = source.userid;
      this.runAsUserId = source.IsVirtualUser() ? "admin" : source.userid;
      this.lastName = source.lastName;
      this.suffixName = source.suffixName;
      this.firstName = source.firstName;
      this.middleName = source.middleName;
      this.employeeID = source.employeeID;
      this.personas = source.personas;
      this.workingFolder = source.workingFolder;
      this.orgId = source.orgId;
      this.isInRootOrg = source.isInRootOrg;
      this.accessMode = source.accessMode;
      this.status = source.status;
      this.email = source.email;
      this.phone = source.phone;
      this.cellPhone = source.cellPhone;
      this.fax = source.fax;
      this.pwdNeverExpires = source.pwdNeverExpires;
      this.requirePwdChange = source.requirePwdChange;
      this.locked = source.locked;
      this.peerView = source.peerView;
      this.dataServicesOptOutKey = source.dataServicesOptOutKey;
      this.delegateTasksRight = source.delegateTasksRight;
      this.lastLogin = source.lastLogin;
      this.chumID = source.chumID;
      this.nmlsOriginatorID = source.nmlsOriginatorID;
      this.nmlsExpirationDate = source.nmlsExpirationDate;
      this.encompassVersion = source.encompassVersion;
      this.emailSignature = source.emailSignature;
      this.personalStatusOnline = source.personalStatusOnline;
      this.inheritParentCompPlan = source.InheritParentCompPlan;
      this.apiUser = source.apiUser;
      this.oAuthClientId = source.oAuthClientId;
      this.allowImpersonation = source.allowImpersonation;
      this.inheritParentCCSite = source.inheritParentCCSite;
      this.passwordChangedDate = source.passwordChangedDate;
      this.failed_login_attempts = source.failed_login_attempts;
      this.profileURL = source.ProfileURL;
      this.personaAccessComments = source.personaAccessComments;
      this.lastLockedOutDateTime = source.lastLockedOutDateTime;
      this.ssoOnly = source.ssoOnly;
      this.ssoDisconnectedFromOrg = source.ssoDisconnectedFromOrg;
      this.passwordRequired = source.passwordRequired;
      this.passwordExists = source.passwordExists;
      this.createdDate = source.createdDate;
      this.createdBy = source.createdBy;
      this.lastModifiedDate = source.lastModifiedDate;
      this.lastModifiedBy = source.lastModifiedBy;
      this.jobtitle = source.jobtitle;
    }

    public UserInfo(UserInfo source, int newOrgId, bool isInRootOrg)
      : this(source)
    {
      this.orgId = newOrgId;
      this.isInRootOrg = isInRootOrg;
    }

    public UserInfo()
    {
    }

    public override int GetHashCode() => this.userid.GetHashCode();

    public override bool Equals(object obj)
    {
      UserInfo userInfo = obj as UserInfo;
      return !(userInfo == (UserInfo) null) && userInfo.userid == this.userid;
    }

    public static bool operator ==(UserInfo o1, UserInfo o2)
    {
      return object.Equals((object) o1, (object) o2);
    }

    public static bool operator !=(UserInfo o1, UserInfo o2) => !(o1 == o2);

    public override string ToString()
    {
      string str = string.Empty;
      if (this.firstName != null && this.firstName != string.Empty)
        str = this.firstName + " ";
      if ((this.middleName ?? "") != string.Empty)
        str = str + this.middleName + " ";
      if (this.lastName != null)
        str += this.lastName;
      if ((this.suffixName ?? "") != string.Empty)
        str = str + " " + this.suffixName;
      return str;
    }

    public string Userid => this.userid;

    public string RunAsUserId => this.runAsUserId;

    public string Password
    {
      get => this.password;
      set => this.password = value;
    }

    public string LastName
    {
      get => this.lastName;
      set => this.lastName = value ?? "";
    }

    public string JobTitle
    {
      get => this.jobtitle;
      set => this.jobtitle = value ?? "";
    }

    public string ProfileURL
    {
      get => this.profileURL;
      set => this.profileURL = value;
    }

    public string FirstName
    {
      get => this.firstName;
      set => this.firstName = value ?? "";
    }

    public string MiddleName
    {
      get => this.middleName;
      set => this.middleName = value ?? "";
    }

    public string SuffixName
    {
      get => this.suffixName;
      set => this.suffixName = value ?? "";
    }

    public string FullName
    {
      get
      {
        return ((!string.IsNullOrWhiteSpace(this.firstName) ? this.firstName : string.Empty) + (!string.IsNullOrWhiteSpace(this.middleName) ? " " + this.middleName : string.Empty) + (!string.IsNullOrWhiteSpace(this.lastName) ? " " + this.lastName : string.Empty) + (!string.IsNullOrWhiteSpace(this.suffixName) ? " " + this.suffixName : string.Empty)).Trim();
      }
    }

    public string EmployeeID
    {
      get => this.employeeID;
      set => this.employeeID = value ?? "";
    }

    public Persona[] UserPersonas
    {
      get => this.personas;
      set => this.personas = value;
    }

    public string PersonaAccessComments
    {
      get => this.personaAccessComments;
      set => this.personaAccessComments = value;
    }

    public string WorkingFolder
    {
      get => this.workingFolder;
      set => this.workingFolder = value ?? "";
    }

    public int OrgId => this.orgId;

    public string OrgName { get; set; }

    public UserInfo.AccessModeEnum AccessMode
    {
      get => this.accessMode;
      set => this.accessMode = value;
    }

    public bool LegacyDelegateTasksRight
    {
      get => this.delegateTasksRight;
      set => this.delegateTasksRight = value;
    }

    public UserInfo.UserStatusEnum Status
    {
      get => this.status;
      set => this.status = value;
    }

    public string Email
    {
      get => this.email;
      set => this.email = value ?? "";
    }

    public string Phone
    {
      get => this.phone;
      set => this.phone = value ?? "";
    }

    public string CellPhone
    {
      get => this.cellPhone;
      set => this.cellPhone = value ?? "";
    }

    public string Fax
    {
      get => this.fax;
      set => this.fax = value ?? "";
    }

    public bool PasswordNeverExpires
    {
      get => this.pwdNeverExpires;
      set => this.pwdNeverExpires = value;
    }

    public bool RequirePasswordChange
    {
      get => this.requirePwdChange;
      set => this.requirePwdChange = value;
    }

    public DateTime PasswordChangedDate
    {
      get => this.passwordChangedDate;
      set => this.passwordChangedDate = value;
    }

    public bool Locked
    {
      get => this.locked;
      set => this.locked = value;
    }

    public DateTime? LastLockedOutDateTime
    {
      get => this.lastLockedOutDateTime;
      set => this.lastLockedOutDateTime = value;
    }

    public bool SSOOnly
    {
      get => this.ssoOnly;
      set => this.ssoOnly = value;
    }

    public UserInfo.UserPeerView PeerView
    {
      get => this.peerView;
      set => this.peerView = value;
    }

    public bool DataServicesOptOut => this.dataServicesOptOutKey != "";

    public string DataServicesOptOutKey
    {
      get => this.dataServicesOptOutKey;
      set => this.dataServicesOptOutKey = value;
    }

    public DateTime LastLogin
    {
      get => this.lastLogin;
      set => this.lastLogin = value;
    }

    public string EncompassVersion
    {
      get => this.encompassVersion;
      set => this.encompassVersion = value;
    }

    public string CHUMId
    {
      get => this.chumID;
      set => this.chumID = value;
    }

    public string NMLSOriginatorID
    {
      get => this.nmlsOriginatorID;
      set => this.nmlsOriginatorID = value;
    }

    public DateTime NMLSExpirationDate
    {
      get => this.nmlsExpirationDate;
      set => this.nmlsExpirationDate = value;
    }

    public bool IsTopLevelUser
    {
      get => this.isInRootOrg;
      set => this.isInRootOrg = value;
    }

    public string EmailSignature
    {
      get => this.emailSignature;
      set => this.emailSignature = value;
    }

    public bool PersonalStatusOnline
    {
      get => this.personalStatusOnline;
      set => this.personalStatusOnline = value;
    }

    public bool InheritParentCompPlan
    {
      get => this.inheritParentCompPlan;
      set => this.inheritParentCompPlan = value;
    }

    public bool ApiUser
    {
      get => this.apiUser;
      set => this.apiUser = value;
    }

    public string OAuthClientId
    {
      get => this.oAuthClientId;
      set => this.oAuthClientId = value;
    }

    public bool AllowImpersonation
    {
      get => this.allowImpersonation;
      set => this.allowImpersonation = value;
    }

    public bool InheritParentCCSite
    {
      get => this.inheritParentCCSite;
      set => this.inheritParentCCSite = value;
    }

    public string UserType
    {
      get => this.userType;
      set => this.userType = value;
    }

    public bool SSODisconnectedFromOrg
    {
      get => this.ssoDisconnectedFromOrg;
      set => this.ssoDisconnectedFromOrg = value;
    }

    public bool PasswordRequired => this.passwordRequired;

    public bool PasswordExists => this.passwordExists;

    public int[] GetPersonaIDs()
    {
      List<int> intList = new List<int>();
      foreach (Persona persona in this.personas)
        intList.Add(persona.ID);
      return intList.ToArray();
    }

    public static bool IsSuperAdministrator(string userId, Persona[] personaList)
    {
      if (userId == "admin" || userId == "(trusted)")
        return true;
      if (personaList != null)
      {
        foreach (object persona in personaList)
        {
          if (persona.Equals((object) Persona.SuperAdministrator))
            return true;
        }
      }
      return false;
    }

    public static bool IsAdministrator(string userId, Persona[] personaList)
    {
      if (UserInfo.IsSuperAdministrator(userId, personaList))
        return true;
      if (personaList != null)
      {
        foreach (object persona in personaList)
        {
          if (persona.Equals((object) Persona.Administrator))
            return true;
        }
      }
      return false;
    }

    public bool IsTopLevelAdministrator()
    {
      if (this.IsSuperAdministrator())
        return true;
      return this.isInRootOrg && this.IsAdministrator();
    }

    public bool IsSuperAdministrator() => UserInfo.IsSuperAdministrator(this.userid, this.personas);

    public bool IsAdministrator() => UserInfo.IsAdministrator(this.userid, this.personas);

    public bool IsVirtualUser()
    {
      return !string.IsNullOrEmpty(this.userid) && this.userid.StartsWith("<") && this.userid.EndsWith(">");
    }

    public object Clone() => (object) new UserInfo(this);

    public int CompareTo(object obj) => this.FullName.CompareTo(((UserInfo) obj).FullName);

    public static bool IsValidUserID(string userId)
    {
      return !((userId ?? "") == "") && userId.Length >= 1 && userId.Length <= 16 && Regex.IsMatch(userId, "^[a-zA-Z0-9._\\-@]*$");
    }

    public enum AccessModeEnum
    {
      ReadWrite,
      ReadOnly,
    }

    public enum UserStatusEnum
    {
      Enabled,
      Disabled,
    }

    public enum UserPeerView
    {
      Disabled,
      ViewOnly,
      Edit,
    }

    [Flags]
    public enum TemplateAccessEnum
    {
      None = 0,
      Public = 1,
      Personal = 2,
      All = Personal | Public, // 0x00000003
    }

    public enum ContextDataId
    {
    }
  }
}
