// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  /// <summary>Represents a single external organization</summary>
  public class ExternalOrganization : SessionBoundObject, IExternalOrganization
  {
    private List<ExternalOriginatorManagementData> companyOrgs;
    private ExternalOriginatorManagementData externalOrg;
    private ExternalOriginatorManagementData companyOrg;
    private ExternalOriginatorManagementData branchOrg;
    private ExternalUrlList urlList;
    private ExternalNotesList notesList;
    private List<ExternalAttachment> attachmentsList;
    private IConfigurationManager mngr;
    private IEnumerable<UserInfo> assignableSalesReps;
    private IOrganizationManager orgMngr;
    private ExternalLoanCompHistoryList externalLOCompHistoryList;
    private LoanCompHistoryList loanCompHistoryList;
    private ExternalLoanCompPlanList externalLOCompPlanList;
    private List<ExternalSalesRep> externalSalesRepListForOrg;
    private List<ExternalSalesRep> externalSalesRepListForCompany;
    private ExternalSalesRep primarySalesRepUser;
    private List<User> assignableUsersAsSalesRep;
    private List<ExternalSettingValue> companyStatusList;
    private List<ExternalSettingValue> contactStatusList;
    private List<ExternalSettingValue> companyRatingList;
    private List<ExternalSettingValue> attachmentCategoryList;
    private List<ExternalSettingValue> priceGroupList;
    private ExternalUserInfo primaryManager;
    private CustomFields customFields;
    private string primarySalesRepUserId = "";
    private bool hasPerformancePatch;
    private string primarySalesRepAssignedDate = "";
    private ExternalFeesList feesList;
    private ExternalWarehouseList warehouseList;
    private ExternalBanksList bankList;
    private ExternalDBAList DBAList;
    private List<ExternalSettingValue> rateSheetList;
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private DateTime minValue = DateTime.Parse("1/1/1900");
    private DateTime maxValue = DateTime.Parse("01/01/2079");
    private ExternalOriginatorManagementData pricingOrg;
    private ExternalLicensing licensing;
    private ExternalLoanTypes loanTypes;

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

    internal ExternalOrganization(
      Session session,
      List<ExternalOriginatorManagementData> companyOrgs,
      int oid,
      bool getDetails,
      bool hasPerformancePatch)
      : base(session)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (ExternalOrganization), "Committed");
      this.companyOrgs = companyOrgs;
      this.externalOrg = this.companyOrgs.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == oid));
      this.companyOrg = Organizations.GetExternalCompany(this.externalOrg.oid, this.companyOrgs);
      this.branchOrg = Organizations.GetExternalBranch(this.externalOrg.oid, this.companyOrgs);
      this.mngr = (IConfigurationManager) session.GetObject("ConfigurationManager");
      this.orgMngr = (IOrganizationManager) session.GetObject("OrganizationManager");
      this.hasPerformancePatch = hasPerformancePatch;
      if (!getDetails)
        return;
      this.getAllDetailInfo();
    }

    /// <summary>Gets external organization ID</summary>
    public int ID => this.externalOrg.oid;

    /// <summary>Gets parent external organization ID</summary>
    public int ParentOrganizationID => this.externalOrg.Parent;

    /// <summary>Gets or sets organization name</summary>
    public string OrganizationName
    {
      get => this.externalOrg.OrganizationName;
      set => this.externalOrg.OrganizationName = value;
    }

    /// <summary>Gets or sets a flag to disable login</summary>
    public bool DisabledLogin
    {
      get => this.externalOrg.DisabledLogin;
      set => this.externalOrg.DisabledLogin = value;
    }

    /// <summary>
    /// Gets or sets a flag to use multi-factor authentication
    /// </summary>
    public bool MultiFactorAuthentication
    {
      get => this.externalOrg.MultiFactorAuthentication;
      set => this.externalOrg.MultiFactorAuthentication = value;
    }

    /// <summary>
    /// Gets or sets a flag to use whether NoAfterHourWires is checked or not
    /// </summary>
    public bool NoAfterHourWires
    {
      get => this.externalOrg.NoAfterHourWires;
      set => this.externalOrg.NoAfterHourWires = value;
    }

    /// <summary>Gets or sets a flag to use Timezone</summary>
    public string Timezone
    {
      get => this.externalOrg.Timezone;
      set => this.externalOrg.Timezone = value;
    }

    /// <summary>
    /// Gets or sets a flag to use multi-factor authentication
    /// </summary>
    public ManageFeeLEDisclosures RequestLEDisclosures
    {
      get => this.externalOrg.GenerateDisclosures;
      set => this.externalOrg.GenerateDisclosures = value;
    }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOriginationOrgType">organization</see> type
    /// </summary>
    public ExternalOriginationOrgType OrganizationType
    {
      get
      {
        switch (this.externalOrg.OrganizationType)
        {
          case ExternalOriginatorOrgType.Company:
            return ExternalOriginationOrgType.Company;
          case ExternalOriginatorOrgType.Branch:
            return ExternalOriginationOrgType.Branch;
          case ExternalOriginatorOrgType.BranchExtension:
            return ExternalOriginationOrgType.BranchExtension;
          default:
            return ExternalOriginationOrgType.CompanyExtension;
        }
      }
      set
      {
        switch (value)
        {
          case ExternalOriginationOrgType.Company:
            this.externalOrg.OrganizationType = ExternalOriginatorOrgType.Company;
            break;
          case ExternalOriginationOrgType.Branch:
            this.externalOrg.OrganizationType = ExternalOriginatorOrgType.Branch;
            break;
          case ExternalOriginationOrgType.BranchExtension:
            this.externalOrg.OrganizationType = ExternalOriginatorOrgType.BranchExtension;
            break;
          default:
            this.externalOrg.OrganizationType = ExternalOriginatorOrgType.CompanyExtension;
            break;
        }
      }
    }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganizationEntityType">entity</see> type
    /// </summary>
    public ExternalOrganizationEntityType EntityType
    {
      get
      {
        switch (this.externalOrg.entityType)
        {
          case ExternalOriginatorEntityType.Broker:
            return ExternalOrganizationEntityType.Broker;
          case ExternalOriginatorEntityType.Correspondent:
            return ExternalOrganizationEntityType.Correspondent;
          case ExternalOriginatorEntityType.Both:
            return ExternalOrganizationEntityType.Both;
          default:
            return ExternalOrganizationEntityType.None;
        }
      }
      set
      {
        switch (value)
        {
          case ExternalOrganizationEntityType.Broker:
            this.externalOrg.entityType = ExternalOriginatorEntityType.Broker;
            break;
          case ExternalOrganizationEntityType.Correspondent:
            this.externalOrg.entityType = ExternalOriginatorEntityType.Correspondent;
            break;
          case ExternalOrganizationEntityType.Both:
            this.externalOrg.entityType = ExternalOriginatorEntityType.Both;
            break;
          default:
            this.externalOrg.entityType = ExternalOriginatorEntityType.None;
            break;
        }
      }
    }

    /// <summary>Gets or Sets the underwriting type</summary>
    public ExternalOrgUnderwritingType UnderwritingType
    {
      get
      {
        switch (this.externalOrg.UnderwritingType)
        {
          case ExternalOriginatorUnderwritingType.Delegated:
            return ExternalOrgUnderwritingType.Delegated;
          case ExternalOriginatorUnderwritingType.NonDelegated:
            return ExternalOrgUnderwritingType.NonDelegated;
          case ExternalOriginatorUnderwritingType.Both:
            return ExternalOrgUnderwritingType.Both;
          default:
            return ExternalOrgUnderwritingType.None;
        }
      }
      set
      {
        if (value != ExternalOrgUnderwritingType.None && (this.externalOrg.entityType & ExternalOriginatorEntityType.Both | ExternalOriginatorEntityType.Correspondent) == ExternalOriginatorEntityType.None)
          throw new Exception("Entity Type must be Correspondent to set Under Writing type to Delegated or Non-Delegated");
        this.externalOrg.UnderwritingType = (ExternalOriginatorUnderwritingType) Enum.Parse(typeof (ExternalOriginatorUnderwritingType), value.ToString());
      }
    }

    /// <summary>Gets or sets externalID</summary>
    public string ExternalID
    {
      get
      {
        return this.externalOrg.ExternalID.Length > 10 ? this.externalOrg.ExternalID.Substring(1) : this.externalOrg.ExternalID;
      }
      set => this.externalOrg.ExternalID = value;
    }

    /// <summary>Gets or sets organizationID</summary>
    public string OrganizationID
    {
      get => this.externalOrg.OrgID;
      set => this.externalOrg.OrgID = value;
    }

    /// <summary>Gets or sets owner name</summary>
    public string OwnerName
    {
      get => this.externalOrg.OwnerName;
      set => this.externalOrg.OwnerName = value;
    }

    /// <summary>Gets or sets company legal name</summary>
    public string CompanyLegalName
    {
      get => this.externalOrg.CompanyLegalName;
      set => this.externalOrg.CompanyLegalName = value;
    }

    /// <summary>Gets or sets company DBA Name</summary>
    public string CompanyDBAName
    {
      get => this.externalOrg.CompanyDBAName;
      set => this.externalOrg.CompanyDBAName = value;
    }

    /// <summary>Gets or sets address</summary>
    public string Address
    {
      get => this.externalOrg.Address;
      set => this.externalOrg.Address = value;
    }

    /// <summary>Gets or sets city</summary>
    public string City
    {
      get => this.externalOrg.City;
      set => this.externalOrg.City = value;
    }

    /// <summary>Gets or sets State</summary>
    public string State
    {
      get => this.externalOrg.State;
      set => this.externalOrg.State = value;
    }

    /// <summary>Gets or sets Zip</summary>
    public string Zip
    {
      get => this.externalOrg.Zip;
      set => this.externalOrg.Zip = value;
    }

    /// <summary>Gets or sets phone number</summary>
    public string PhoneNumber
    {
      get => this.externalOrg.PhoneNumber;
      set => this.externalOrg.PhoneNumber = value;
    }

    /// <summary>Gets or sets fax number</summary>
    public string FaxNumber
    {
      get => this.externalOrg.FaxNumber;
      set => this.externalOrg.FaxNumber = value;
    }

    /// <summary>Gets or sets email</summary>
    public string Email
    {
      get => this.externalOrg.Email;
      set => this.externalOrg.Email = value;
    }

    /// <summary>Gets or sets web site</summary>
    public string Website
    {
      get => this.externalOrg.Website;
      set => this.externalOrg.Website = value;
    }

    /// <summary>
    /// Gets or sets last loan submitted datetime from the organization
    /// </summary>
    public DateTime LastLoanSubmitted
    {
      get => this.externalOrg.LastLoanSubmitted;
      set => this.externalOrg.LastLoanSubmitted = value;
    }

    /// <summary>
    /// Gets or sets the value for the field Company can Accept First Payments
    /// </summary>
    public bool CanAcceptFirstPayments
    {
      get => this.externalOrg.CanAcceptFirstPayments;
      set => this.externalOrg.CanAcceptFirstPayments = value;
    }

    /// <summary>Gets or sets Rate sheet email information</summary>
    public string EmailForRateSheet
    {
      get => this.externalOrg.EmailForRateSheet;
      set => this.externalOrg.EmailForRateSheet = value;
    }

    /// <summary>Gets or sets Rate sheet Fax number</summary>
    public string FaxForRateSheet
    {
      get => this.externalOrg.FaxForRateSheet;
      set => this.externalOrg.FaxForRateSheet = value;
    }

    /// <summary>Gets or sets Lock email information</summary>
    public string EmailForLockInfo
    {
      get => this.externalOrg.EmailForLockInfo;
      set => this.externalOrg.EmailForLockInfo = value;
    }

    /// <summary>Gets or sets Lock Fax number</summary>
    public string FaxForLockInfo
    {
      get => this.externalOrg.FaxForLockInfo;
      set => this.externalOrg.FaxForLockInfo = value;
    }

    /// <summary>Gets or sets EPPS user name</summary>
    public bool InheritProductAndPricing
    {
      get => this.externalOrg.UseParentInfoForEPPS;
      set => this.externalOrg.UseParentInfoForEPPS = value;
    }

    /// <summary>Gets or sets EPPS user name</summary>
    public string EPPSUserName
    {
      get
      {
        if (this.pricingOrg == null)
        {
          this.pricingOrg = this.externalOrg;
          this.getProductandPricingData(this.externalOrg);
        }
        return this.pricingOrg.EPPSUserName;
      }
      set => this.externalOrg.EPPSUserName = value;
    }

    /// <summary>Gets or sets EPPS Comp Model</summary>
    public string EPPSCompModel
    {
      get
      {
        if (this.pricingOrg == null)
        {
          this.pricingOrg = this.externalOrg;
          this.getProductandPricingData(this.externalOrg);
        }
        return this.pricingOrg.EPPSCompModel;
      }
      set => this.externalOrg.EPPSCompModel = value;
    }

    /// <summary>Gets or sets Price Group</summary>
    public PriceGroup PriceGroup
    {
      get
      {
        if (this.priceGroupList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PriceGroup
          });
        ExternalSettingValue externalSettingValue = this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => string.Concat((object) x.settingId) == this.externalOrg.EPPSPriceGroup));
        return externalSettingValue == null ? (PriceGroup) null : new PriceGroup(this.Session, externalSettingValue.settingValue, externalSettingValue.settingCode, externalSettingValue.settingId);
      }
      set
      {
        throw new Exception("Price Group property is obsolete. Please use PriceGroupBroker, PriceGroupDelegated and PriceGroupNonDelegated instead.");
      }
    }

    /// <summary>Gets or Sets the EPPSPriceGroupBroker</summary>
    public PriceGroup PriceGroupBroker
    {
      get
      {
        if (this.priceGroupList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PriceGroup
          });
        ExternalSettingValue externalSettingValue = this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => string.Concat((object) x.settingId) == this.externalOrg.EPPSPriceGroupBroker));
        return externalSettingValue == null ? (PriceGroup) null : new PriceGroup(this.Session, externalSettingValue.settingValue, externalSettingValue.settingCode, externalSettingValue.settingId);
      }
      set
      {
        if (value == null)
        {
          this.externalOrg.EPPSPriceGroupBroker = "";
        }
        else
        {
          string message = "Entity Type should be set to Broker to set Broker Price Group.";
          if (this.externalOrg.entityType == ExternalOriginatorEntityType.Correspondent || this.externalOrg.entityType == ExternalOriginatorEntityType.None)
            throw new Exception(message);
          if (this.priceGroupList == null)
            this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
            {
              ExternalOrganizationSetting.PriceGroup
            });
          this.externalOrg.EPPSPriceGroupBroker = string.Concat((object) (this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingCode == value.StatusCode)) ?? throw new Exception("Invalid Status")).settingId);
        }
      }
    }

    /// <summary>Gets or Sets the EPPSPriceGroupDelegated</summary>
    public PriceGroup PriceGroupDelegated
    {
      get
      {
        if (this.priceGroupList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PriceGroup
          });
        ExternalSettingValue externalSettingValue = this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => string.Concat((object) x.settingId) == this.externalOrg.EPPSPriceGroupDelegated));
        return externalSettingValue == null ? (PriceGroup) null : new PriceGroup(this.Session, externalSettingValue.settingValue, externalSettingValue.settingCode, externalSettingValue.settingId);
      }
      set
      {
        if (value == null)
        {
          this.externalOrg.EPPSPriceGroupDelegated = "";
        }
        else
        {
          string message1 = "Entity Type should be set to Correspondent to set Delegated Price Group.";
          if (this.externalOrg.entityType == ExternalOriginatorEntityType.Broker || this.externalOrg.entityType == ExternalOriginatorEntityType.None)
            throw new Exception(message1);
          string message2 = "Underwriting Type should be set to Delegated to set Delegated Price Group.";
          if (this.externalOrg.UnderwritingType == ExternalOriginatorUnderwritingType.None || this.externalOrg.UnderwritingType == ExternalOriginatorUnderwritingType.NonDelegated)
            throw new Exception(message2);
          if (this.priceGroupList == null)
            this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
            {
              ExternalOrganizationSetting.PriceGroup
            });
          this.externalOrg.EPPSPriceGroupDelegated = string.Concat((object) (this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingCode == value.StatusCode)) ?? throw new Exception("Invalid Status")).settingId);
        }
      }
    }

    /// <summary>Gets or Sets the EPPS Price Group Non Delegated</summary>
    public PriceGroup PriceGroupNonDelegated
    {
      get
      {
        if (this.priceGroupList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PriceGroup
          });
        ExternalSettingValue externalSettingValue = this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => string.Concat((object) x.settingId) == this.externalOrg.EPPSPriceGroupNonDelegated));
        return externalSettingValue == null ? (PriceGroup) null : new PriceGroup(this.Session, externalSettingValue.settingValue, externalSettingValue.settingCode, externalSettingValue.settingId);
      }
      set
      {
        if (value == null)
        {
          this.externalOrg.EPPSPriceGroupNonDelegated = "";
        }
        else
        {
          string message1 = "Entity Type should be set to Correspondent to set Non-Delegated Price Group.";
          if (this.externalOrg.entityType == ExternalOriginatorEntityType.Broker || this.externalOrg.entityType == ExternalOriginatorEntityType.None)
            throw new Exception(message1);
          string message2 = "Underwriting Type should be set to Non-Delegated to set Non-Delegated Price Group.";
          if (this.externalOrg.UnderwritingType == ExternalOriginatorUnderwritingType.None || this.externalOrg.UnderwritingType == ExternalOriginatorUnderwritingType.Delegated)
            throw new Exception(message2);
          if (this.priceGroupList == null)
            this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
            {
              ExternalOrganizationSetting.PriceGroup
            });
          this.externalOrg.EPPSPriceGroupNonDelegated = string.Concat((object) (this.priceGroupList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingCode == value.StatusCode)) ?? throw new Exception("Invalid Status")).settingId);
        }
      }
    }

    private void getProductandPricingData(ExternalOriginatorManagementData extOrg)
    {
      if (extOrg != null && extOrg.UseParentInfoForEPPS && extOrg.OrganizationType != ExternalOriginatorOrgType.Company && extOrg.Parent != 0)
        this.getProductandPricingData(this.companyOrgs.First<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (item => item.oid == extOrg.Parent)));
      else
        this.pricingOrg = extOrg;
    }

    /// <summary>Gets or sets PML user name</summary>
    public string PMLUserName
    {
      get
      {
        if (this.pricingOrg == null)
        {
          this.pricingOrg = this.externalOrg;
          this.getProductandPricingData(this.externalOrg);
        }
        return this.pricingOrg.PMLUserName;
      }
      set => this.externalOrg.PMLUserName = value;
    }

    /// <summary>Sets PML password</summary>
    public string PMLPassword
    {
      get
      {
        if (this.pricingOrg == null)
        {
          this.pricingOrg = this.externalOrg;
          this.getProductandPricingData(this.externalOrg);
        }
        return this.pricingOrg.PMLPassword;
      }
      set => this.externalOrg.PMLPassword = value;
    }

    /// <summary>Gets or sets PML Customer Code</summary>
    public string PMLCustomerCode
    {
      get
      {
        if (this.pricingOrg == null)
        {
          this.pricingOrg = this.externalOrg;
          this.getProductandPricingData(this.externalOrg);
        }
        return this.pricingOrg.PMLCustomerCode;
      }
      set => this.externalOrg.PMLCustomerCode = value;
    }

    /// <summary>Gets or sets Current Approval Status</summary>
    public CurrentCompanyStatus CurrentStatus
    {
      get
      {
        if (this.companyStatusList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.CompanyStatus
          });
        ExternalSettingValue externalSettingValue = this.companyStatusList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingId == this.externalOrg.CurrentStatus));
        return externalSettingValue == null ? (CurrentCompanyStatus) null : new CurrentCompanyStatus(this.Session, externalSettingValue.settingValue, externalSettingValue.settingId);
      }
      set
      {
        if (this.companyStatusList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.CompanyStatus
          });
        this.externalOrg.CurrentStatus = (this.companyStatusList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingValue == value.StatusName)) ?? throw new Exception("Invalid Status")).settingId;
      }
    }

    /// <summary>Gets or sets EPPS raet sheet</summary>
    public RateSheet EPPSRateSheet
    {
      get
      {
        if (this.rateSheetList == null)
          this.getAllDetailInfo();
        ExternalSettingValue externalSettingValue = this.rateSheetList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => string.Concat((object) x.settingId) == this.externalOrg.EPPSRateSheet));
        return externalSettingValue == null ? (RateSheet) null : new RateSheet(this.Session, externalSettingValue.settingValue, externalSettingValue.settingId);
      }
      set
      {
        if (this.rateSheetList == null)
          this.getAllDetailInfo();
        this.externalOrg.EPPSRateSheet = string.Concat((object) (this.rateSheetList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingValue == value.StatusName)) ?? throw new Exception("Invalid rate sheet")).settingId);
      }
    }

    /// <summary>Gets or sets Add to watch list flag</summary>
    public bool AddToWatchlist
    {
      get => this.externalOrg.AddToWatchlist;
      set => this.externalOrg.AddToWatchlist = value;
    }

    /// <summary>Gets or sets Current Status Date</summary>
    public DateTime CurrentStatusDate
    {
      get => this.externalOrg.CurrentStatusDate;
      set => this.externalOrg.CurrentStatusDate = value;
    }

    /// <summary>Gets or sets Approved Date</summary>
    public DateTime ApprovedDate
    {
      get => this.externalOrg.ApprovedDate;
      set => this.externalOrg.ApprovedDate = value;
    }

    /// <summary>Gets or sets Applicaiton date</summary>
    public DateTime ApplicationDate
    {
      get => this.externalOrg.ApplicationDate;
      set => this.externalOrg.ApplicationDate = value;
    }

    /// <summary>Gets or sets company rating information</summary>
    public CompanyRating CompanyRating
    {
      get
      {
        if (this.companyRatingList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.CompanyRating
          });
        ExternalSettingValue externalSettingValue = this.companyRatingList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingId == this.externalOrg.CompanyRating));
        return externalSettingValue == null ? (CompanyRating) null : new CompanyRating(this.Session, externalSettingValue.settingValue, externalSettingValue.settingId);
      }
      set
      {
        if (this.companyRatingList == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.CompanyRating
          });
        this.externalOrg.CompanyRating = (this.companyRatingList.FirstOrDefault<ExternalSettingValue>((Func<ExternalSettingValue, bool>) (x => x.settingValue == value.StatusName)) ?? throw new Exception("Invalid Rating")).settingId;
      }
    }

    /// <summary>Gets or sets flag for incorporated</summary>
    public bool Incorporated
    {
      get => this.externalOrg.Incorporated;
      set => this.externalOrg.Incorporated = value;
    }

    /// <summary>Gets or sets State incorporated information</summary>
    public string StateIncorp
    {
      get => this.externalOrg.StateIncorp;
      set => this.externalOrg.StateIncorp = value;
    }

    /// <summary>Gets or sets Date of incorporation</summary>
    public DateTime DateOfIncorporation
    {
      get => this.externalOrg.DateOfIncorporation;
      set => this.externalOrg.DateOfIncorporation = value;
    }

    /// <summary>Gets Years of Business</summary>
    public string YrsInBusiness
    {
      get
      {
        string empty = string.Empty;
        if ((ValueType) this.DateOfIncorporation != null && !string.IsNullOrWhiteSpace(this.DateOfIncorporation.ToString()) && this.DateOfIncorporation != DateTime.MinValue)
        {
          int num = DateTime.Now.Year - this.DateOfIncorporation.Year;
          if (this.DateOfIncorporation > DateTime.Now.AddYears(-num))
            --num;
          empty = num.ToString();
        }
        return empty;
      }
    }

    /// <summary>Gets or sets type of entity</summary>
    public int TypeOfEntity
    {
      get => this.externalOrg.TypeOfEntity;
      set => this.externalOrg.TypeOfEntity = value;
    }

    /// <summary>Gets or sets Other entity description</summary>
    public string OtherEntityDescription
    {
      get => this.externalOrg.OtherEntityDescription;
      set => this.externalOrg.OtherEntityDescription = value;
    }

    /// <summary>Gets or sets tax id</summary>
    public string TaxID
    {
      get => this.externalOrg.TaxID;
      set => this.externalOrg.TaxID = value;
    }

    /// <summary>Gets or sets flag to use SSN format</summary>
    public bool UseSSNFormat
    {
      get => this.externalOrg.UseSSNFormat;
      set => this.externalOrg.UseSSNFormat = value;
    }

    /// <summary>Gets or sets NmlsID</summary>
    public string NmlsId
    {
      get => this.externalOrg.NmlsId;
      set => this.externalOrg.NmlsId = value;
    }

    /// <summary>Gets or sets Financial Period</summary>
    public string FinancialsPeriod
    {
      get => this.externalOrg.FinancialsPeriod;
      set => this.externalOrg.FinancialsPeriod = value;
    }

    /// <summary>Gets or sets date financials last updated</summary>
    public DateTime FinancialsLastUpdate
    {
      get => this.externalOrg.FinancialsLastUpdate;
      set => this.externalOrg.FinancialsLastUpdate = value;
    }

    /// <summary>Gets or sets company net worth</summary>
    public Decimal CompanyNetWorth
    {
      get
      {
        return !this.externalOrg.CompanyNetWorth.HasValue ? 0M : this.externalOrg.CompanyNetWorth.Value;
      }
      set => this.externalOrg.CompanyNetWorth = new Decimal?(value);
    }

    /// <summary>Gets or sets EO expiration date</summary>
    public DateTime EOExpirationDate
    {
      get => this.externalOrg.EOExpirationDate;
      set => this.externalOrg.EOExpirationDate = value;
    }

    /// <summary>Gets or sets EO Company</summary>
    public string EOCompany
    {
      get => this.externalOrg.EOCompany;
      set => this.externalOrg.EOCompany = value;
    }

    /// <summary>Gets or sets EO policy number</summary>
    public string EOPolicyNumber
    {
      get => this.externalOrg.EOPolicyNumber;
      set => this.externalOrg.EOPolicyNumber = value;
    }

    /// <summary>Gets or sets Mers Originating organization ID</summary>
    public string MERSOriginatingORGID
    {
      get => this.externalOrg.MERSOriginatingORGID;
      set => this.externalOrg.MERSOriginatingORGID = value;
    }

    /// <summary>Gets or sets DU sponsored flag</summary>
    public bool DUSponsored
    {
      get => this.externalOrg.DUSponsored == 1;
      set => this.externalOrg.DUSponsored = value ? 1 : 0;
    }

    /// <summary>Gets or sets Can fund in own name flag</summary>
    public bool CanFundInOwnName
    {
      get => this.externalOrg.CanFundInOwnName == 1;
      set => this.externalOrg.CanFundInOwnName = value ? 1 : 0;
    }

    /// <summary>Gets or sets Is Test Account flag</summary>
    public bool IsTestAccount
    {
      get => this.externalOrg.IsTestAccount;
      set => this.externalOrg.IsTestAccount = value;
    }

    /// <summary>Gets or sets Can close in own name flag</summary>
    public bool CanCloseInOwnName
    {
      get => this.externalOrg.CanCloseInOwnName == 1;
      set => this.externalOrg.CanCloseInOwnName = value ? 1 : 0;
    }

    /// <summary>
    /// Gets or sets <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalLicensing">Licensing</see> information
    /// </summary>
    public ExternalLicensing Licensing
    {
      get
      {
        if (this.licensing == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.License
          });
        return this.licensing;
      }
      set => this.licensing = value;
    }

    /// <summary>Gets or sets loan types</summary>
    public ExternalLoanTypes LoanTypes
    {
      get
      {
        if (this.loanTypes == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.LoanTypes
          });
        return this.loanTypes;
      }
      set => this.loanTypes = value;
    }

    /// <summary>
    /// Gets or sets whether to use Best Efforts for Commitment Authority - can only be set at Company Level
    /// </summary>
    public bool CommitmentUseBestEffort
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.CommitmentUseBestEffort : this.externalOrg.CommitmentUseBestEffort;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentUseBestEffort = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Best Efforts is Limited - can only be set at Company Level
    /// </summary>
    public bool CommitmentUseBestEffortLimited
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.CommitmentUseBestEffortLimited : this.externalOrg.CommitmentUseBestEffortLimited;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentUseBestEffortLimited = value;
      }
    }

    /// <summary>
    /// Gets or sets the Best Efforts Maximum Commitment Authority - can only be set at Company Level
    /// </summary>
    public Decimal MaxCommitmentAuthority
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.MaxCommitmentAuthority : this.externalOrg.MaxCommitmentAuthority;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.MaxCommitmentAuthority = value;
      }
    }

    /// <summary>Gets the Best Efforts Maximum Available Amount</summary>
    public Decimal CommitmentBestEffortsAvailableAmount
    {
      get
      {
        if (!this.CommitmentUseBestEffort)
          return 0M;
        if (!this.CommitmentUseBestEffortLimited)
          return Decimal.MaxValue;
        Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommentments = this.GetOutstandingCommentments();
        return outstandingCommentments.ContainsKey(CorrespondentMasterDeliveryType.IndividualBestEfforts) ? this.MaxCommitmentAuthority - outstandingCommentments[CorrespondentMasterDeliveryType.IndividualBestEfforts] : this.MaxCommitmentAuthority;
      }
    }

    /// <summary>
    /// Gets or sets whether Commitment Authority is Mandatory - can only be set at Company Level
    /// </summary>
    public bool CommitmentMandatory
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.CommitmentMandatory : this.externalOrg.CommitmentMandatory;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentMandatory = value;
      }
    }

    /// <summary>
    /// Gets or sets Max Commitment Amount for Mandatory - can only be set at Company Level
    /// </summary>
    public Decimal MaxCommitmentAmount
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.MaxCommitmentAmount : this.externalOrg.MaxCommitmentAmount;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.MaxCommitmentAmount = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Individual is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryIndividual
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryIndividual : this.externalOrg.IsCommitmentDeliveryIndividual;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryIndividual = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Bulk is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryBulk
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryBulk : this.externalOrg.IsCommitmentDeliveryBulk;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryBulk = value;
      }
    }

    /// <summary>
    /// Gets or sets whether AOT is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryAOT
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryAOT : this.externalOrg.IsCommitmentDeliveryAOT;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryAOT = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Bulk AOT is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryBulkAOT
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryBulkAOT : this.externalOrg.IsCommitmentDeliveryBulkAOT;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryBulkAOT = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Direct Trade is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryLiveTrade
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryLiveTrade : this.externalOrg.IsCommitmentDeliveryLiveTrade;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryLiveTrade = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Co-Issue is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryCoIssue
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryCoIssue : this.externalOrg.IsCommitmentDeliveryCoIssue;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryCoIssue = value;
      }
    }

    /// <summary>
    /// Gets or sets whether Forward is an Accepted Delivery Type - can only be set at Company Level
    /// </summary>
    public bool IsCommitmentDeliveryForward
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.IsCommitmentDeliveryForward : this.externalOrg.IsCommitmentDeliveryForward;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryForward = value;
      }
    }

    /// <summary>Gets the Mandatory Available Amount</summary>
    public Decimal CommitmentMandatoryAvailableAmount
    {
      get
      {
        if (!this.CommitmentMandatory)
          return 0M;
        Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommentments = this.GetOutstandingCommentments();
        Decimal num = 0M;
        foreach (KeyValuePair<CorrespondentMasterDeliveryType, Decimal> keyValuePair in outstandingCommentments)
        {
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.IndividualMandatory && this.IsCommitmentDeliveryIndividual)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.AOT && this.IsCommitmentDeliveryAOT)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.Bulk && this.IsCommitmentDeliveryBulk)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.BulkAOT && this.IsCommitmentDeliveryBulkAOT)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.CoIssue && this.IsCommitmentDeliveryCoIssue)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.Forwards && this.IsCommitmentDeliveryForward)
            num += keyValuePair.Value;
          if (keyValuePair.Key == CorrespondentMasterDeliveryType.LiveTrade && this.IsCommitmentDeliveryLiveTrade)
            num += keyValuePair.Value;
        }
        return this.MaxCommitmentAmount - num;
      }
    }

    /// <summary>
    /// Gets or sets Commitment Policy for loans that exceed Max Commitment Authority - can only be set at Company Level
    /// </summary>
    public ExternalOriginationCommitmentPolicy CommitmentPolicy
    {
      get
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
        {
          switch (this.companyOrg.CommitmentPolicy)
          {
            case ExternalOriginatorCommitmentPolicy.NoRestriction:
              return ExternalOriginationCommitmentPolicy.NoRestriction;
            case ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit:
              return ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit;
            default:
              return ExternalOriginationCommitmentPolicy.DontAllowLoanCreation;
          }
        }
        else
        {
          switch (this.externalOrg.CommitmentPolicy)
          {
            case ExternalOriginatorCommitmentPolicy.NoRestriction:
              return ExternalOriginationCommitmentPolicy.NoRestriction;
            case ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit:
              return ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit;
            default:
              return ExternalOriginationCommitmentPolicy.DontAllowLoanCreation;
          }
        }
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        if (value != ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit)
        {
          if (value == ExternalOriginationCommitmentPolicy.DontAllowLoanCreation)
            this.externalOrg.CommitmentPolicy = ExternalOriginatorCommitmentPolicy.DontAllowLoanCreation;
          else
            this.externalOrg.CommitmentPolicy = ExternalOriginatorCommitmentPolicy.NoRestriction;
        }
        else
          this.externalOrg.CommitmentPolicy = ExternalOriginatorCommitmentPolicy.DontAllowLockorSubmit;
      }
    }

    /// <summary>
    /// Gets or sets Trade Creation Policy for Correspondent Trades
    /// </summary>
    public ExternalOriginationCorrespondentTradeCreationPolicy CorrespondentTradePolicy
    {
      get
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
        {
          switch (this.companyOrg.CommitmentTradePolicy)
          {
            case ExternalOriginatorCommitmentTradePolicy.NoRestriction:
              return ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction;
            case ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation:
              return ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation;
            default:
              return ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction;
          }
        }
        else
        {
          switch (this.externalOrg.CommitmentTradePolicy)
          {
            case ExternalOriginatorCommitmentTradePolicy.NoRestriction:
              return ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction;
            case ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation:
              return ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation;
            default:
              return ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction;
          }
        }
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        if (value != ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction)
        {
          if (value != ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation)
            return;
          this.externalOrg.CommitmentTradePolicy = ExternalOriginatorCommitmentTradePolicy.DontAllowTradeCreation;
        }
        else
          this.externalOrg.CommitmentTradePolicy = ExternalOriginatorCommitmentTradePolicy.NoRestriction;
      }
    }

    /// <summary>
    /// Gets or sets the warning message for restricted loans - can only be set at Company Level
    /// </summary>
    public string CommitmentMessage
    {
      get
      {
        return this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company ? this.companyOrg.CommitmentMessage : this.externalOrg.CommitmentMessage;
      }
      set
      {
        if (this.externalOrg.OrganizationType != ExternalOriginatorOrgType.Company)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentMessage = value;
      }
    }

    /// <summary>
    /// Gets external company object this organizaiton belongs to
    /// </summary>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company
    {
      get
      {
        return new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, this.companyOrg.oid, false, this.hasPerformancePatch);
      }
    }

    /// <summary>
    /// Gets Branch organization object this organization belongs to
    /// </summary>
    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch
    {
      get
      {
        return this.branchOrg == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, this.branchOrg.oid, false, this.hasPerformancePatch);
      }
    }

    /// <summary>Gets custom fields for the External Organization</summary>
    public CustomFields CustomFields
    {
      get
      {
        if (this.customFields == null)
          this.customFields = new CustomFields((IConfigurationManager) this.Session.GetObject("ConfigurationManager"), this.externalOrg);
        return this.customFields;
      }
      set
      {
        if (value == null)
          return;
        ContactCustomFieldInfo[] items = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetCustomFieldInfo().Items;
        ContactCustomField[] fieldsCollection = this.customFields.FieldsCollection;
        foreach (ContactCustomFieldInfo contactCustomFieldInfo in items)
        {
          ContactCustomFieldInfo cf = contactCustomFieldInfo;
          if (((IEnumerable<ContactCustomField>) fieldsCollection).Select<ContactCustomField, bool>((Func<ContactCustomField, bool>) (x => x.FieldID.ToString() == cf.LabelID.ToString())).Count<bool>() == 0)
            throw new Exception("Field : " + cf.Label + " is not configured in settings.");
        }
      }
    }

    /// <summary>Gets primary sales rep of the organization</summary>
    public User PrimarySalesRep
    {
      get
      {
        if (this.assignableSalesReps == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.AssignableSalesReps
          });
        UserInfo info = this.assignableSalesReps.FirstOrDefault<UserInfo>((Func<UserInfo, bool>) (x => x.Userid == this.externalOrg.PrimarySalesRepUserId));
        return info == (UserInfo) null ? (User) null : new User(this.Session, info, false);
      }
    }

    /// <summary>get and set primary sales rep assigned date</summary>
    public DateTime PrimarySalesRepAssignedDate
    {
      get => this.externalOrg.PrimarySalesRepAssignedDate;
      set => this.externalOrg.PrimarySalesRepAssignedDate = value;
    }

    /// <summary>Gets a list of available sales reps</summary>
    public UserList AvailableAEs
    {
      get
      {
        if (this.assignableSalesReps == null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.AssignableSalesReps
          });
        List<User> userObj = new List<User>();
        this.assignableSalesReps.ToList<UserInfo>().ForEach((Action<UserInfo>) (x => userObj.Add(new User(this.Session, x, false))));
        return new UserList((IList) userObj);
      }
    }

    /// <summary>Gets or sets primary manager of the organization</summary>
    public ExternalUser PrimaryManager
    {
      get
      {
        if ((UserInfo) this.primaryManager == (UserInfo) null)
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PrimaryManager
          });
        return (UserInfo) this.primaryManager == (UserInfo) null ? (ExternalUser) null : new ExternalUser(this.Session, this.primaryManager, (ExternalUserInfo) null, this.companyOrgs, false, false, this.hasPerformancePatch);
      }
      set
      {
        this.externalOrg.Manager = value.IsManager ? value.ID : throw new Exception("The assigned contact is not a manager.");
        this.primaryManager = value.info;
      }
    }

    /// <summary>
    /// Gets external userid of primary manager of the organization
    /// </summary>
    public string PrimaryManagerExternalUserID => this.externalOrg.Manager;

    /// <summary>
    /// Method to create new external user under the organization
    /// </summary>
    /// <param name="firstName">user's first name</param>
    /// <param name="lastName">user's last name</param>
    /// <param name="loginEmail">user's TPO web center login email</param>
    /// <param name="password">user's TPO web center login password</param>
    /// <param name="urls">a list of accessible urls</param>
    /// <returns>a new <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">external</see> user object.</returns>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server and create a new external user.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       ExternalUser newUser = externalOrg.CreateUser("FirstName", "LastName", "login@EllieMae.com", "password", externalOrg.GetUrlList());
    /// 
    ///       if(newUser != null)
    ///         Console.WriteLine("New contact created: " + newUser.ID);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls)
    {
      List<ExternalUserInfo> contactsByLoginEmailId = this.mngr.GetAllContactsByLoginEmailId(loginEmail, "");
      if (contactsByLoginEmailId != null)
      {
        List<ExternalUserInfoURL> urlsByContactIds = this.mngr.GetExternalUserInfoUrlsByContactIds(string.Join("','", contactsByLoginEmailId.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>()));
        if (urlsByContactIds != null)
        {
          foreach (ExternalUrl url1 in (CollectionBase) urls)
          {
            ExternalUrl url = url1;
            if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == url.URLID)) != null)
              throw new Exception("Users with login email '" + loginEmail + "' and URL '" + url.URL + "' already exist for this organization.");
          }
        }
      }
      List<long> allContactId = this.mngr.GetAllContactID();
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      externalUserInfo.EmailForLogin = loginEmail;
      externalUserInfo.ExternalOrgID = this.ID;
      externalUserInfo.FirstName = firstName;
      externalUserInfo.LastName = lastName;
      externalUserInfo.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      externalUserInfo.SalesRepID = this.externalOrg.PrimarySalesRepUserId;
      ExternalUserInfo newExternalUserInfo = externalUserInfo;
      newExternalUserInfo.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(newExternalUserInfo).ExternalUserID, password, DateTime.Now, true);
      List<int> intList = new List<int>();
      if (urls != null && urls.Count > 0)
      {
        foreach (ExternalUrl url in (CollectionBase) urls)
          intList.Add(url.URLID);
      }
      this.mngr.SaveExternalUserInfoURLs(info.ExternalUserID, intList.ToArray());
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      if (info.UpdatedByExternal && info.UpdatedBy != "")
        updatingExtUser = this.mngr.GetExternalUserInfoByContactId(info.UpdatedBy);
      return new ExternalUser(this.Session, info, updatingExtUser, this.companyOrgs, false, false, this.hasPerformancePatch);
    }

    /// <summary>
    /// Method to create new external user under the organization
    /// </summary>
    /// <param name="firstName">user's first name</param>
    /// <param name="lastName">user's last name</param>
    /// <param name="loginEmail">user's TPO web center login email</param>
    /// <param name="password">user's TPO web center login password</param>
    /// <param name="urls">a list of accessible urls</param>
    /// <param name="primaySalesRep">the primary sales rep to assign to the new external user</param>
    /// <returns>a new <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">external</see> user object.</returns>
    public ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls,
      User primaySalesRep)
    {
      List<ExternalUserInfo> contactsByLoginEmailId = this.mngr.GetAllContactsByLoginEmailId(loginEmail, "");
      if (contactsByLoginEmailId != null)
      {
        List<ExternalUserInfoURL> urlsByContactIds = this.mngr.GetExternalUserInfoUrlsByContactIds(string.Join("','", contactsByLoginEmailId.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>()));
        if (urlsByContactIds != null)
        {
          foreach (ExternalUrl url1 in (CollectionBase) urls)
          {
            ExternalUrl url = url1;
            if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == url.URLID)) != null)
              throw new Exception("Users with login email '" + loginEmail + "' and URL '" + url.URL + "' already exist for this organization.");
          }
        }
      }
      List<long> allContactId = this.mngr.GetAllContactID();
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      externalUserInfo.EmailForLogin = loginEmail;
      externalUserInfo.ExternalOrgID = this.ID;
      externalUserInfo.FirstName = firstName;
      externalUserInfo.LastName = lastName;
      externalUserInfo.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      ExternalUserInfo newExternalUserInfo = externalUserInfo;
      if (primaySalesRep != null)
        newExternalUserInfo.SalesRepID = primaySalesRep.ID;
      newExternalUserInfo.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(newExternalUserInfo).ExternalUserID, password, DateTime.Now, true);
      List<int> intList = new List<int>();
      if (urls != null && urls.Count > 0)
      {
        foreach (ExternalUrl url in (CollectionBase) urls)
          intList.Add(url.URLID);
      }
      this.mngr.SaveExternalUserInfoURLs(info.ExternalUserID, intList.ToArray());
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      if (info.UpdatedByExternal && info.UpdatedBy != "")
        updatingExtUser = this.mngr.GetExternalUserInfoByContactId(info.UpdatedBy);
      return new ExternalUser(this.Session, info, updatingExtUser, this.companyOrgs, false, false, this.hasPerformancePatch);
    }

    /// <summary>
    /// Method to create new external user under the organization
    /// </summary>
    /// <param name="firstName">user's first name</param>
    /// <param name="lastName">user's last name</param>
    /// <param name="loginEmail">user's TPO web center login email</param>
    /// <param name="password">user's TPO web center login password</param>
    /// <param name="urls">a list of accessible urls</param>
    /// <param name="updatedByExtUser">UpdatedBy External User</param>
    /// <returns>a new <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">external</see> user object.</returns>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server and create a new external user.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       ExternalUser newUser = externalOrg.CreateUser("FirstName", "LastName", "login@EllieMae.com", "password", externalOrg.GetUrlList());
    /// 
    ///       if(newUser != null)
    ///         Console.WriteLine("New contact created: " + newUser.ID);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls,
      ExternalUser updatedByExtUser)
    {
      List<ExternalUserInfo> contactsByLoginEmailId = this.mngr.GetAllContactsByLoginEmailId(loginEmail, "");
      if (contactsByLoginEmailId != null)
      {
        List<ExternalUserInfoURL> urlsByContactIds = this.mngr.GetExternalUserInfoUrlsByContactIds(string.Join("','", contactsByLoginEmailId.Select<ExternalUserInfo, string>((Func<ExternalUserInfo, string>) (e => e.ExternalUserID)).ToArray<string>()));
        if (urlsByContactIds != null)
        {
          foreach (ExternalUrl url1 in (CollectionBase) urls)
          {
            ExternalUrl url = url1;
            if (urlsByContactIds.FirstOrDefault<ExternalUserInfoURL>((Func<ExternalUserInfoURL, bool>) (item => item.URLID == url.URLID)) != null)
              throw new Exception("Users with login email '" + loginEmail + "' and URL '" + url.URL + "' already exist for this organization.");
          }
        }
      }
      List<long> allContactId = this.mngr.GetAllContactID();
      ExternalUserInfo externalUserInfo = new ExternalUserInfo();
      externalUserInfo.EmailForLogin = loginEmail;
      externalUserInfo.ExternalOrgID = this.ID;
      externalUserInfo.FirstName = firstName;
      externalUserInfo.LastName = lastName;
      externalUserInfo.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      externalUserInfo.SalesRepID = this.externalOrg.PrimarySalesRepUserId;
      ExternalUserInfo newExternalUserInfo = externalUserInfo;
      newExternalUserInfo.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(newExternalUserInfo).ExternalUserID, password, DateTime.Now, true);
      info.UpdatedBy = updatedByExtUser.ID;
      info.UpdatedByExternal = true;
      List<int> intList = new List<int>();
      if (urls != null && urls.Count > 0)
      {
        foreach (ExternalUrl url in (CollectionBase) urls)
          intList.Add(url.URLID);
      }
      this.mngr.SaveExternalUserInfoURLs(info.ExternalUserID, intList.ToArray());
      ExternalUserInfo updatingExtUser = (ExternalUserInfo) null;
      if (info.UpdatedByExternal && info.UpdatedBy != "")
        updatingExtUser = this.mngr.GetExternalUserInfoByContactId(info.UpdatedBy);
      return new ExternalUser(this.Session, info, updatingExtUser, this.companyOrgs, false, false, this.hasPerformancePatch);
    }

    /// <summary>
    /// Method to retrieve all external users under the organization
    /// </summary>
    /// <returns>a list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.ExternalUser">users</see></returns>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server and retrieves all the external users under the organization.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       ExternalUserList externalUsers = externalOrg.GetUsers();
    /// 
    ///       Console.WriteLine("Users under organization " + externalOrg.CompanyLegalName + Environment.NewLine);
    ///       foreach (ExternalUser externalUser in externalUsers)
    ///          Console.WriteLine(externalUser.FirstName + " " + externalUser.LastName + Environment.NewLine);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalUserList GetUsers()
    {
      this.ensureExists();
      ExternalUserInfo[] externalUserInfos = this.mngr.GetAllExternalUserInfos(this.ExternalID);
      return externalUserInfos != null ? ExternalUser.ToList(this.Session, externalUserInfos, this.companyOrgs, this.hasPerformancePatch) : (ExternalUserList) null;
    }

    /// <summary>Method to retrieve selected urls for the organization</summary>
    /// <returns>a list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">external urls</see></returns>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server and retrieves all the assigned urls.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       ExternalUrlList externalUrls = externalOrg.GetUrlList();
    /// 
    ///       Console.WriteLine("Urls assigned to organization " + externalOrg.CompanyLegalName + Environment.NewLine);
    ///       foreach (ExternalUrl externalUrl in externalUrls)
    ///          Console.WriteLine(externalUrl.URL + " " + Environment.NewLine);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalUrlList GetUrlList()
    {
      if (this.urlList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.UrlList
        });
      return this.urlList;
    }

    /// <summary>Method to retrieve selected urls for the organization</summary>
    /// <param name="Url">an ExternalSiteUrl object</param>
    /// <param name="entityType">ExternalOrganizationEntityType</param>
    /// <returns>an ExternalUrl object</returns>
    /// <example>
    ///   The following code retrieves an organization from the Encompass Server.  A list of all available urls is retrieved.
    ///   The code removes all currently assigned site and adds all previously not assigned urls.
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //All available sites
    ///       List<ExternalSiteUrl> availableSiteUrls = session.Organizations.GetSiteUrls();
    /// 
    ///       //Currently assigned urls
    ///       List<ExternalUrl> externalUrls = externalOrg.GetUrlList().ToArray().ToList();
    /// 
    ///       //Remove currently assigned url and add previously not assigned urls
    ///       foreach (ExternalSiteUrl externalSiteUrl in availableSiteUrls)
    ///       {
    ///           ExternalUrl externalUrl = externalUrls.FirstOrDefault(x => x.SiteId == externalSiteUrl.SiteId);
    ///           if (externalUrl != null)
    ///              externalOrg.DeleteExternalUrl(externalUrl);
    ///           else
    ///              externalOrg.AddExternalUrl(externalSiteUrl, externalOrg.EntityType);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalUrl AddExternalUrl(
      ExternalSiteUrl Url,
      ExternalOrganizationEntityType entityType)
    {
      this.ensureExists();
      if (this.EntityType != ExternalOrganizationEntityType.Both && this.EntityType != entityType)
        throw new ExternalOrganizationUrlException(OrganizationUrlViolationType.InvalidOrganizationUrlCreation, "Channel Type of the Url has to be same as the company organization.");
      return new ExternalUrl(this.mngr.AssociateExternalOrganisationUrl(this.externalOrg.oid, Url.SiteId, Convert.ToInt32((object) entityType)));
    }

    /// <summary>Method to add url to the organization</summary>
    /// <param name="Url">an <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">EXternalUrl</see> object</param>
    /// <returns>Returned the saved <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">EXternalUrl</see> object</returns>
    public ExternalUrl AddExternalUrl(ExternalUrl Url)
    {
      this.ensureExists();
      if (this.EntityType != ExternalOrganizationEntityType.Both && this.EntityType != Url.EntityType)
        throw new ExternalOrganizationUrlException(OrganizationUrlViolationType.InvalidOrganizationUrlCreation, "Channel Type of the Url has to be same as the company organization.");
      return new ExternalUrl(this.mngr.AssociateExternalOrganisationUrl(this.externalOrg.oid, Url.SiteId, Convert.ToInt32((object) Url.EntityType)));
    }

    /// <summary>
    /// Method to overwrite organization's existing url setting with the updated setting
    /// </summary>
    /// <param name="orgUrls">A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">ExternalUrl</see> to update</param>
    public void UpdateExternalUrls(ExternalUrlList orgUrls)
    {
      List<ExternalOrgURL> list = ExternalUrl.ToList(orgUrls);
      foreach (ExternalOrgURL externalOrgUrl in list)
      {
        if (this.EntityType != ExternalOrganizationEntityType.Both && Convert.ToInt32((object) this.externalOrg.entityType) != externalOrgUrl.EntityType)
          throw new ExternalOrganizationUrlException(OrganizationUrlViolationType.InvalidOrganizationUrlCreation, "Channel Type of the Url has to be same as the company organization.");
      }
      this.mngr.UpdateExternalOrganizationSelectedURLs(this.externalOrg.oid, list, this.companyOrg.oid);
      this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
      {
        ExternalOrganizationSetting.UrlList
      });
    }

    /// <summary>Delete a particular url setting for the organization</summary>
    /// <param name="url">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalUrl">ExternalUrl</see> to delete.</param>
    /// <example>
    ///   The following code retrieves an organization from the Encompass Server.  A list of all available urls is retrieved.
    ///   The code removes all currently assigned site and adds all previously not assigned urls.
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //All available sites
    ///       List<ExternalSiteUrl> availableSiteUrls = session.Organizations.GetSiteUrls();
    /// 
    ///       //Currently assigned urls
    ///       List<ExternalUrl> externalUrls = externalOrg.GetUrlList().ToArray().ToList();
    /// 
    ///       //Remove currently assigned url and add previously not assigned urls
    ///       foreach (ExternalSiteUrl externalSiteUrl in availableSiteUrls)
    ///       {
    ///           ExternalUrl externalUrl = externalUrls.FirstOrDefault(x => x.SiteId == externalSiteUrl.SiteId);
    ///           if (externalUrl != null)
    ///              externalOrg.DeleteExternalUrl(externalUrl);
    ///           else
    ///              externalOrg.AddExternalUrl(externalSiteUrl, externalOrg.EntityType);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void DeleteExternalUrl(ExternalUrl url)
    {
      this.mngr.DeleteExternalOrgSelectedUrl(this.externalOrg.oid, url.URLID);
    }

    /// <summary>
    /// Commits the license change to the external organization to the Encompass Server.
    /// </summary>
    public void UpdateStateLicense()
    {
      this.ensureExists();
      BranchExtLicensing branchExtLicensing = this.licensing != null && this.licensing.BranchExternalLicensing != null ? new BranchExtLicensing((BranchLicensing) this.licensing.BranchExternalLicensing) : throw new InvalidOperationException("This operation is not valid until object is not null");
      this.mngr.UpdateExternalLicence(this.licensing.BranchExternalLicensing, this.externalOrg.oid);
    }

    /// <summary>
    /// Replace existing state license settings with the new settings
    /// </summary>
    /// <param name="stateLicenses">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType">StateLicenseExtType</see> to update.</param>
    public void UpdateStateLicense(List<EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType> stateLicenses)
    {
      this.ensureExists();
      if (this.licensing == null || this.licensing.BranchExternalLicensing == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.License
        });
      this.licensing.BranchExternalLicensing.StateLicenseExtTypes = new List<EllieMae.EMLite.RemotingServices.StateLicenseExtType>();
      for (int index = 0; index < stateLicenses.Count; ++index)
        this.licensing.BranchExternalLicensing.StateLicenseExtTypes.Add(new EllieMae.EMLite.RemotingServices.StateLicenseExtType(stateLicenses[index].StateAbbrevation, stateLicenses[index].LicenseType, stateLicenses[index].LicenseNo, stateLicenses[index].IssueDate, stateLicenses[index].StartDate, stateLicenses[index].EndDate, stateLicenses[index].LicenseStatus, stateLicenses[index].StatusDate, stateLicenses[index].Approved, stateLicenses[index].Exempt, stateLicenses[index].LastChecked));
      this.licensing.BranchExternalLicensing.UseParentInfo = false;
      this.mngr.UpdateExternalLicence(this.licensing.BranchExternalLicensing, this.externalOrg.oid);
    }

    /// <summary>Method to get all organization notes</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalNote">ExternalNote</see></returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete notes.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of notes
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //print out all notes
    ///       ExternalNotesList noteList = externalOrg.GetAllNotes();
    ///       foreach (ExternalNote note in noteList)
    ///           Console.WriteLine("Note added by " + note.WhoAdded + ":" + note.NotesDetails);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalNote(new ExternalNotesList() { noteList[0] });
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalNote("New note");
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalNotesList GetAllNotes()
    {
      if (this.notesList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.Note
        });
      return this.notesList;
    }

    /// <summary>Method to create new organization note</summary>
    /// <param name="note">note to create</param>
    /// <returns>Created <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalNote">ExternalNote</see> object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete notes.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of notes
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //print out all notes
    ///       ExternalNotesList noteList = externalOrg.GetAllNotes();
    ///       foreach (ExternalNote note in noteList)
    ///           Console.WriteLine("Note added by " + note.WhoAdded + ":" + note.NotesDetails);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalNote(new ExternalNotesList() { noteList[0] });
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalNote("New note");
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalNote CreateExternalNote(string note)
    {
      ExternalOrgNote externalOrgNote = new ExternalOrgNote(this.Session.UserID, note);
      externalOrgNote.ExternalCompanyID = this.externalOrg.oid;
      externalOrgNote.AddedDateTime = this.Session.GetServerTime();
      int num = this.mngr.AddExternalOrganizationNotes(externalOrgNote);
      externalOrgNote.NoteID = num;
      this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
      {
        ExternalOrganizationSetting.Note
      });
      return new ExternalNote(externalOrgNote);
    }

    /// <summary>Method to delete organization note</summary>
    /// <param name="notes">The list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalNote">ExternalNote</see> to delete.</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete notes.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of notes
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //print out all notes
    ///       ExternalNotesList noteList = externalOrg.GetAllNotes();
    ///       foreach (ExternalNote note in noteList)
    ///           Console.WriteLine("Note added by " + note.WhoAdded + ":" + note.NotesDetails);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalNote(new ExternalNotesList() { noteList[0] });
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalNote("New note");
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void DeleteExternalNote(ExternalNotesList notes)
    {
      List<int> notesID = new List<int>();
      foreach (ExternalNote note in (CollectionBase) notes)
        notesID.Add(note.NoteID);
      this.mngr.DeleteExternalOrganizationNotes(this.externalOrg.oid, notesID);
      this.notesList = (ExternalNotesList) null;
    }

    /// <summary>Method to get number of notes</summary>
    /// <returns>number of notes</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete notes.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of notes
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //print out all notes
    ///       ExternalNotesList noteList = externalOrg.GetAllNotes();
    ///       foreach (ExternalNote note in noteList)
    ///           Console.WriteLine("Note added by " + note.WhoAdded + ":" + note.NotesDetails);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalNote(new ExternalNotesList() { noteList[0] });
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalNote("New note");
    ///       Console.WriteLine("Number of notes:" + externalOrg.GetNumberOfNotes());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int GetNumberOfNotes()
    {
      if (this.notesList == null)
        this.GetAllNotes();
      return this.notesList.Count;
    }

    /// <summary>Method to get all attachments</summary>
    /// <returns>a list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment">ExternalAttachment</see></returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public List<ExternalAttachment> GetAllAttachments()
    {
      if (this.attachmentsList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.Attachment
        });
      return this.attachmentsList;
    }

    /// <summary>Method to create new attachment</summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="description">Description of the attachment</param>
    /// <param name="dateAdded">Date added</param>
    /// <param name="category">Category</param>
    /// <param name="fileDate">File Date</param>
    /// <param name="expirationDate">Expiration Date</param>
    /// <param name="daysToExpire">Days to expire</param>
    /// <param name="fileLocation">location of the file</param>
    /// <param name="file">Binary object of the file</param>
    /// <returns>an ExternalAttachment object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalAttachment CreateExternalAttachment(
      string fileName,
      string description,
      DateTime dateAdded,
      string category,
      DateTime fileDate,
      DateTime expirationDate,
      int daysToExpire,
      string fileLocation,
      DataObject file)
    {
      AttachmentCategory category1 = (AttachmentCategory) null;
      if (this.attachmentCategoryList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.AttachmentCategory
        });
      if (this.attachmentCategoryList.Count != 0)
      {
        foreach (ExternalSettingValue attachmentCategory in this.attachmentCategoryList)
        {
          if (attachmentCategory.settingValue == category)
          {
            category1 = new AttachmentCategory(this.Session, attachmentCategory.settingValue, attachmentCategory.settingId);
            break;
          }
        }
      }
      return this.CreateExternalAttachment(fileName, description, dateAdded, category1, fileDate, expirationDate, daysToExpire, fileLocation, file);
    }

    /// <summary>Method to create new attachment</summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="description">Description of the attachment</param>
    /// <param name="dateAdded">Date added</param>
    /// <param name="category">Attachment Category</param>
    /// <param name="fileDate">File Date</param>
    /// <param name="expirationDate">Expiration Date</param>
    /// <param name="daysToExpire">Days to expire</param>
    /// <param name="fileLocation">location of the file</param>
    /// <param name="file">Binary object of the file</param>
    /// <returns>The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment">ExternalAttachment</see> created.</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalAttachment CreateExternalAttachment(
      string fileName,
      string description,
      DateTime dateAdded,
      AttachmentCategory category,
      DateTime fileDate,
      DateTime expirationDate,
      int daysToExpire,
      string fileLocation,
      DataObject file)
    {
      string message = this.validateAttachment(fileLocation, file.Unwrap());
      if (message != "")
        throw new Exception(message);
      int category1 = -1;
      if (category != null)
        category1 = category.ID;
      ExternalOrgAttachments externalOrgAttachments = new ExternalOrgAttachments(Guid.NewGuid(), this.externalOrg.oid, fileName, description, dateAdded, category1, fileDate, this.Session.UserID, expirationDate, daysToExpire, fileLocation);
      this.mngr.InsertExternalAttachment(externalOrgAttachments);
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) externalOrgAttachments.Guid;
      string str = ((IEnumerable<string>) fileLocation.Split('.')).Last<string>();
      this.mngr.AddAttachment(new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null), file.Unwrap());
      this.attachmentsList = (List<ExternalAttachment>) null;
      return new ExternalAttachment(externalOrgAttachments);
    }

    /// <summary>Method to update an existing external attachment</summary>
    /// <param name="attachment">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment">ExternalAttachment</see> to update.</param>
    /// <param name="file">The <see cref="T:EllieMae.Encompass.BusinessObjects.DataObject">DataObject</see> object to update</param>
    public void UpdateExternalAttachment(ExternalAttachment attachment, DataObject file)
    {
      string message = this.validateAttachment(attachment.RealFileName, file.Unwrap());
      if (message != "")
        throw new Exception(message);
      this.mngr.UpdateExternalAttachment(new ExternalOrgAttachments(attachment.Guid, attachment.ExternalOrgID, attachment.FileName, attachment.Description, attachment.DateAdded, attachment.Category, attachment.FileDate, attachment.UserWhoAdded, attachment.ExpirationDate, attachment.DaysToExpire, attachment.RealFileName));
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      this.mngr.AddAttachment(new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null), file.Unwrap());
      this.attachmentsList = (List<ExternalAttachment>) null;
    }

    private string validateAttachment(string fileLocation, BinaryObject file)
    {
      if (fileLocation.LastIndexOf('.') < 0)
        return "This file does not have an extension. Please select another file.";
      string str = "";
      try
      {
        str = fileLocation.Substring(fileLocation.LastIndexOf('.') + 1, 3);
      }
      catch
      {
      }
      if (str != "" && (str == "exe" || str == "dll" || str == "msi"))
        return "This file extension is not supported. Please select another file.";
      return file.Length > 25000000L ? "File attachments cannot exceed 25 MB. Please select another file." : "";
    }

    /// <summary>Method to delete an attachment</summary>
    /// <param name="attachment">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment">ExternalAttachment</see> to delete.</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void DeleteExternalAttachment(ExternalAttachment attachment)
    {
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      FileSystemEntry entry = new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), FileSystemEntry.Types.File, (string) null);
      this.mngr.DeleteExternalAttachment(attachment.Guid, entry);
      this.attachmentsList = (List<ExternalAttachment>) null;
    }

    /// <summary>
    /// Get a flag indicating if any of the attachment is expired
    /// </summary>
    /// <returns>flag indicating if any of the attachment is expired</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool GetExternalAttachmentIsExpired()
    {
      return this.mngr.GetExternalAttachmentIsExpired(this.externalOrg.oid);
    }

    /// <summary>Gets number of attachments</summary>
    /// <returns>Number of attachments</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of attachments
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    ///       Console.WriteLine("Any expired attachments:" + (externalOrg.GetExternalAttachmentIsExpired() ? "Yes" : "No"));
    /// 
    ///       //print out all attachments
    ///       List<ExternalAttachment> attachmentList = externalOrg.GetAllAttachments();
    ///       foreach (ExternalAttachment attachment in attachmentList)
    ///          Console.WriteLine("Attachment added by " + attachment.UserWhoAdded + ":" + attachment.FileName);
    /// 
    ///       //delete first note in the list
    ///       externalOrg.DeleteExternalAttachment(attachmentList[0]);
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       //add a new note
    ///       externalOrg.CreateExternalAttachment("SampleFileName.pdf", "Sample attachment description", DateTime.Now, 0,
    ///           DateTime.Now, DateTime.Now.AddYears(1), 365, "C:\\SampleFileName.pdf", new EllieMae.Encompass.BusinessObjects.DataObject("C:\\SampleFileName.pdf"));
    ///       Console.WriteLine("Number of attachments:" + externalOrg.GetNumberOfAttachments());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int GetNumberOfAttachments()
    {
      if (this.attachmentsList == null)
        this.GetAllAttachments();
      return this.attachmentsList.Count;
    }

    /// <summary>Gets existing attachment in binary format</summary>
    /// <param name="attachment">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalAttachment">ExternalAttachment</see> to retrieve data.</param>
    /// <returns><see cref="T:EllieMae.Encompass.BusinessObjects.DataObject">DataObject</see> object</returns>
    public DataObject ReadAttachment(ExternalAttachment attachment)
    {
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      return new DataObject(this.mngr.ReadAttachment(guid.ToString() + "." + str));
    }

    /// <summary>Get all LO comp plans</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompPlan">ExternalLoanCompPlan</see></returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of LO Comp History
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //print out all LO Comp History
    ///       ExternalLoanCompHistoryList loCompHistoryList = externalOrg.GetLOCompHistory();
    ///       foreach (ExternalLoanCompHistory loCompHistory in loCompHistoryList)
    ///           Console.WriteLine("LO Comp History - " + loCompHistory.PlanName + ":" + loCompHistory.StartDate.ToShortDateString() +
    ///                 " to " + loCompHistory.EndDate.ToShortDateString());
    /// 
    ///       //delete first LO Comp History in the list
    ///       externalOrg.DeleteLoCompHistory(loCompHistoryList[0]);
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //add a new LO Comp History
    ///       ExternalLoanCompPlanList loCompPlanList = externalOrg.GetAllLOCompPlans();
    ///       externalOrg.AssignLOCompPlan(loCompPlanList[0], DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(12));
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalLoanCompPlanList GetAllLOCompPlans()
    {
      if (this.externalLOCompPlanList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.LOComp
        });
      return this.externalLOCompPlanList;
    }

    /// <summary>Gets number of LO Comp Plans</summary>
    /// <returns>number of LO Comp Plans</returns>
    public int GetNumberOfLOCompPlans()
    {
      if (this.externalLOCompPlanList == null)
        this.GetAllLOCompPlans();
      return this.externalLOCompPlanList.Count;
    }

    /// <summary>Gets LO Comp History</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see></returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of LO Comp History
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //print out all LO Comp History
    ///       ExternalLoanCompHistoryList loCompHistoryList = externalOrg.GetLOCompHistory();
    ///       foreach (ExternalLoanCompHistory loCompHistory in loCompHistoryList)
    ///           Console.WriteLine("LO Comp History - " + loCompHistory.PlanName + ":" + loCompHistory.StartDate.ToShortDateString() +
    ///                 " to " + loCompHistory.EndDate.ToShortDateString());
    /// 
    ///       //delete first LO Comp History in the list
    ///       externalOrg.DeleteLoCompHistory(loCompHistoryList[0]);
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //add a new LO Comp History
    ///       ExternalLoanCompPlanList loCompPlanList = externalOrg.GetAllLOCompPlans();
    ///       externalOrg.AssignLOCompPlan(loCompPlanList[0], DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(12));
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalLoanCompHistoryList GetLOCompHistory()
    {
      if (this.externalLOCompHistoryList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.LOCompHistory
        });
      this.SortPlans(true);
      return this.externalLOCompHistoryList;
    }

    private void SortPlans(bool changeEndDate)
    {
      if (this.externalLOCompHistoryList == null || this.externalLOCompHistoryList.Count == 0)
        return;
      DateTime startDate;
      for (int index1 = 0; index1 < this.externalLOCompHistoryList.Count - 1; ++index1)
      {
        for (int index2 = index1 + 1; index2 < this.externalLOCompHistoryList.Count; ++index2)
        {
          startDate = this.externalLOCompHistoryList[index1].StartDate;
          DateTime date1 = startDate.Date;
          startDate = this.externalLOCompHistoryList[index2].StartDate;
          DateTime date2 = startDate.Date;
          if (date1 > date2)
          {
            ExternalLoanCompHistory externalLoCompHistory = this.externalLOCompHistoryList[index1];
            this.externalLOCompHistoryList[index1] = this.externalLOCompHistoryList[index2];
            this.externalLOCompHistoryList[index2] = externalLoCompHistory;
          }
        }
      }
      if (!changeEndDate)
        return;
      for (int index = 0; index < this.externalLOCompHistoryList.Count - 1; ++index)
      {
        ExternalLoanCompHistory externalLoCompHistory = this.externalLOCompHistoryList[index];
        startDate = this.externalLOCompHistoryList[index + 1].StartDate;
        DateTime dateTime = startDate.AddDays(-1.0);
        externalLoCompHistory.EndDate = dateTime;
      }
      this.externalLOCompHistoryList[this.externalLOCompHistoryList.Count - 1].EndDate = DateTime.MaxValue;
    }

    /// <summary>Gets number of LO Comp History</summary>
    /// <returns>number of LO Comp History</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of LO Comp History
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //print out all LO Comp History
    ///       ExternalLoanCompHistoryList loCompHistoryList = externalOrg.GetLOCompHistory();
    ///       foreach (ExternalLoanCompHistory loCompHistory in loCompHistoryList)
    ///           Console.WriteLine("LO Comp History - " + loCompHistory.PlanName + ":" + loCompHistory.StartDate.ToShortDateString() +
    ///                 " to " + loCompHistory.EndDate.ToShortDateString());
    /// 
    ///       //delete first LO Comp History in the list
    ///       externalOrg.DeleteLoCompHistory(loCompHistoryList[0]);
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //add a new LO Comp History
    ///       ExternalLoanCompPlanList loCompPlanList = externalOrg.GetAllLOCompPlans();
    ///       externalOrg.AssignLOCompPlan(loCompPlanList[0], DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(12));
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int GetNumberOfLOCompHistory()
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return this.externalLOCompHistoryList.Count;
    }

    /// <summary>Method to Add LO Comp Plan</summary>
    /// <param name="loanCompPlan">external loan comp plan</param>
    /// <param name="startDate">date started</param>
    /// <param name="endDate">end started</param>
    /// <returns>The assigned <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see></returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of LO Comp History
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //print out all LO Comp History
    ///       ExternalLoanCompHistoryList loCompHistoryList = externalOrg.GetLOCompHistory();
    ///       foreach (ExternalLoanCompHistory loCompHistory in loCompHistoryList)
    ///           Console.WriteLine("LO Comp History - " + loCompHistory.PlanName + ":" + loCompHistory.StartDate.ToShortDateString() +
    ///                 " to " + loCompHistory.EndDate.ToShortDateString());
    /// 
    ///       //delete first LO Comp History in the list
    ///       externalOrg.DeleteLoCompHistory(loCompHistoryList[0]);
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //add a new LO Comp History
    ///       ExternalLoanCompPlanList loCompPlanList = externalOrg.GetAllLOCompPlans();
    ///       externalOrg.AssignLOCompPlan(loCompPlanList[0], DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(12));
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalLoanCompHistory AssignLOCompPlan(
      ExternalLoanCompPlan loanCompPlan,
      DateTime startDate,
      DateTime endDate)
    {
      if (endDate < DateTime.Now)
        throw new ExternalOrganizationLOCompPlanException(LOCompPlanViolationType.InvalidStartDate, "The Start Date cannot be earlier than today's date.");
      LoanCompHistory loanCompHistory = new LoanCompHistory(this.externalOrg.oid.ToString(), loanCompPlan.Name, loanCompPlan.Id, startDate, endDate);
      loanCompHistory.NewRecord = true;
      loanCompHistory.MinTermDays = loanCompPlan.MinTermDays;
      loanCompHistory.PercentAmt = loanCompPlan.PercentAmt;
      loanCompHistory.DollarAmount = loanCompPlan.DollarAmount;
      loanCompHistory.MinDollarAmount = loanCompPlan.MinDollarAmount;
      loanCompHistory.MaxDollarAmount = loanCompPlan.MaxDollarAmount;
      loanCompHistory.PercentAmtBase = loanCompPlan.PercentAmtBase;
      loanCompHistory.RoundingMethod = loanCompPlan.RoundingMethod;
      if (this.loanCompHistoryList != null && this.loanCompHistoryList.Count > 0)
      {
        if (!this.loanCompHistoryList.IsNewPlanStartDateValid(loanCompHistory))
          throw new ExternalOrganizationLOCompPlanException(LOCompPlanViolationType.InvalidStartDate, "The Start Date is invalid.");
        if (!this.loanCompHistoryList.IsNewPlanStartDateValidWithMinimumTermDays(loanCompHistory, loanCompHistory.StartDate.Date))
          throw new ExternalOrganizationLOCompPlanException(LOCompPlanViolationType.InvalidStartDate, "The Start Date entered is prior to the Earliest Change Date for the previous plan. Would you like to proceed?");
      }
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      this.loanCompHistoryList.AddHistory(loanCompHistory);
      this.mngr.UpdateExternalOrgLOCompPlans(false, this.externalOrg.oid, this.loanCompHistoryList);
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return new ExternalLoanCompHistory(loanCompHistory);
    }

    /// <summary>Method to get current plan based on the trigger date</summary>
    /// <param name="triggerDateTime">The trigger date to get the plan</param>
    /// <returns>The active <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> for the specified date.</returns>
    public ExternalLoanCompHistory GetCurrentPlan(DateTime triggerDateTime)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      LoanCompHistory currentPlan = this.loanCompHistoryList.GetCurrentPlan(triggerDateTime);
      return currentPlan == null ? (ExternalLoanCompHistory) null : new ExternalLoanCompHistory(currentPlan);
    }

    /// <summary>
    /// Method to get current and future plans from a particular date
    /// </summary>
    /// <param name="todayDate">The started date</param>
    /// <returns>A list of active and future <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> for the specified date.</returns>
    public ExternalLoanCompHistoryList GetCurrentAndFuturePlans(DateTime todayDate)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return ExternalLoanCompHistory.ToList(this.loanCompHistoryList.GetCurrentAndFuturePlans(todayDate));
    }

    /// <summary>Gets future plans from a particular date</summary>
    /// <param name="todayDate">The start date</param>
    /// <returns>A list of future <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> for the specified date.</returns>
    public ExternalLoanCompHistoryList GetFuturePlans(DateTime todayDate)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return ExternalLoanCompHistory.ToList(this.loanCompHistoryList.GetFuturePlans(todayDate));
    }

    /// <summary>
    /// Method to update the start date of a loan comp history
    /// </summary>
    /// <param name="startDate">new start date</param>
    /// <param name="selectedLoanCompHistory">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> to update.</param>
    /// <returns>True if successfully updated.</returns>
    public bool UpdateStartDateForCompHistory(
      DateTime startDate,
      ExternalLoanCompHistory selectedLoanCompHistory)
    {
      LoanCompHistory loanCompHistory = ExternalLoanCompHistory.ToLoanCompHistory(selectedLoanCompHistory);
      if (startDate < DateTime.Today.Date)
        throw new ExternalOrganizationLOCompPlanException(LOCompPlanViolationType.InvalidStartDate, "The Start Date cannot be earlier than today's date.");
      if (!this.loanCompHistoryList.IsNewPlanStartDateValid(loanCompHistory, startDate))
        throw new ExternalOrganizationLOCompPlanException(LOCompPlanViolationType.InvalidStartDate, "The Start Date is invalid in current Assigned Comp Plan list.");
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      LoanCompHistory historyByCompPlanId = this.loanCompHistoryList.getLoanCompHistoryByCompPlanId(loanCompHistory.CompPlanId);
      if (historyByCompPlanId == null)
        return false;
      historyByCompPlanId.StartDate = startDate;
      this.mngr.UpdateExternalOrgLOCompPlans(false, this.externalOrg.oid, this.loanCompHistoryList);
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return true;
    }

    /// <summary>Method to get LoComp History by plan id</summary>
    /// <param name="compPlanId">the comp plan id</param>
    /// <returns>The desired <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> object</returns>
    public ExternalLoanCompHistory GetLoCompHistoryByPlanId(int compPlanId)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      LoanCompHistory historyByCompPlanId = this.loanCompHistoryList.getLoanCompHistoryByCompPlanId(compPlanId);
      return historyByCompPlanId != null ? new ExternalLoanCompHistory(historyByCompPlanId) : (ExternalLoanCompHistory) null;
    }

    /// <summary>Method to delete a list of LO Comp histories</summary>
    /// <param name="compPlans">A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> to delete.</param>
    /// <returns>Result of the transaction.</returns>
    public bool DeleteLoCompHistory(ExternalLoanCompHistoryList compPlans)
    {
      List<int> intList = new List<int>();
      foreach (ExternalLoanCompHistory compPlan in (CollectionBase) compPlans)
        intList.Add(compPlan.CompPlanId);
      this.mngr.DeleteExternalCompPlans(false, this.externalOrg.oid, intList.ToArray());
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return true;
    }

    /// <summary>Method to delete one LO Comp history</summary>
    /// <param name="compPlan">The <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalLoanCompHistory">ExternalLoanCompHistory</see> to delete.</param>
    /// <returns>Result of the transaction.</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and perform add/delete attachments.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Display number of LO Comp History
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //print out all LO Comp History
    ///       ExternalLoanCompHistoryList loCompHistoryList = externalOrg.GetLOCompHistory();
    ///       foreach (ExternalLoanCompHistory loCompHistory in loCompHistoryList)
    ///           Console.WriteLine("LO Comp History - " + loCompHistory.PlanName + ":" + loCompHistory.StartDate.ToShortDateString() +
    ///                 " to " + loCompHistory.EndDate.ToShortDateString());
    /// 
    ///       //delete first LO Comp History in the list
    ///       externalOrg.DeleteLoCompHistory(loCompHistoryList[0]);
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       //add a new LO Comp History
    ///       ExternalLoanCompPlanList loCompPlanList = externalOrg.GetAllLOCompPlans();
    ///       externalOrg.AssignLOCompPlan(loCompPlanList[0], DateTime.Now.AddMonths(1), DateTime.Now.AddMonths(12));
    ///       Console.WriteLine("Number of LO Comp History:" + externalOrg.GetNumberOfLOCompHistory());
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool DeleteLoCompHistory(ExternalLoanCompHistory compPlan)
    {
      this.mngr.DeleteExternalCompPlans(false, this.externalOrg.oid, compPlan.CompPlanId);
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return true;
    }

    /// <summary>Method to set an Encompass user as sales rep</summary>
    /// <param name="user">The <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> to set as sales rep.</param>
    /// <returns>result of the transaction.</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and assign a new sales rep.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       User newSalesRep = session.Users.GetUser("SampleSalesRep");
    /// 
    ///       ExternalSalesRep currentPrimaySalesRep = externalOrg.GetPrimarySalesRep();
    /// 
    ///       //If current primary sales rep is not the same as SampleSalesRep and it is deletable,
    ///       //remove the existing primary sales rep from the sales rep list of the organization.
    ///       if (currentPrimaySalesRep != null && currentPrimaySalesRep.userId != newSalesRep.ID && currentPrimaySalesRep.isDeletable)
    ///       {
    ///           externalOrg.DeleteExternalOrganizationSalesReps(new ExternalSalesRep[] { currentPrimaySalesRep });
    ///       }
    /// 
    ///       //Set SampleSalesRep as the new primary sales rep of the organization.
    ///       externalOrg.SetSalesRepAsPrimary(newSalesRep);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool SetSalesRepAsPrimary(User user)
    {
      if (this.externalSalesRepListForOrg == null)
        this.GetExternalOrgSalesRepsForCurrentOrg();
      ExternalSalesRep externalSalesRep = this.externalSalesRepListForOrg.Find((Predicate<ExternalSalesRep>) (e1 => e1.userId.Equals(user.ID)));
      if (externalSalesRep == null)
        throw new Exception("Not a valid user in the current organization");
      this.mngr.SetSalesRepAsPrimary(user.ID, this.externalOrg.oid);
      this.primarySalesRepUser = externalSalesRep;
      return true;
    }

    /// <summary>Method to retrieve Primary sales rep</summary>
    /// <returns>The primary <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep">sales rep</see>.</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and assign a new sales rep.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       User newSalesRep = session.Users.GetUser("SampleSalesRep");
    /// 
    ///       ExternalSalesRep currentPrimaySalesRep = externalOrg.GetPrimarySalesRep();
    /// 
    ///       //If current primary sales rep is not the same as SampleSalesRep and it is deletable,
    ///       //remove the existing primary sales rep from the sales rep list of the organization.
    ///       if (currentPrimaySalesRep != null && currentPrimaySalesRep.userId != newSalesRep.ID && currentPrimaySalesRep.isDeletable)
    ///       {
    ///           externalOrg.DeleteExternalOrganizationSalesReps(new ExternalSalesRep[] { currentPrimaySalesRep });
    ///       }
    /// 
    ///       //Set SampleSalesRep as the new primary sales rep of the organization.
    ///       externalOrg.SetSalesRepAsPrimary(newSalesRep);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public ExternalSalesRep GetPrimarySalesRep()
    {
      if (this.primarySalesRepUser == null || this.externalSalesRepListForOrg == null || this.primarySalesRepUser == null)
        this.GetExternalOrgSalesRepsForCurrentOrg();
      return this.primarySalesRepUser;
    }

    /// <summary>
    /// Method to remove a list of Encompass users as sales reps
    /// </summary>
    /// <param name="users">The list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep">sales reps</see> to delete.</param>
    /// <returns>result of the transaction.</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and assign a new sales rep.
    /// <code>
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       User newSalesRep = session.Users.GetUser("SampleSalesRep");
    /// 
    ///       ExternalSalesRep currentPrimaySalesRep = externalOrg.GetPrimarySalesRep();
    /// 
    ///       //If current primary sales rep is not the same as SampleSalesRep and it is deletable,
    ///       //remove the existing primary sales rep from the sales rep list of the organization.
    ///       if (currentPrimaySalesRep != null && currentPrimaySalesRep.userId != newSalesRep.ID && currentPrimaySalesRep.isDeletable)
    ///       {
    ///           externalOrg.DeleteExternalOrganizationSalesReps(new ExternalSalesRep[] { currentPrimaySalesRep });
    ///       }
    /// 
    ///       //Set SampleSalesRep as the new primary sales rep of the organization.
    ///       externalOrg.SetSalesRepAsPrimary(newSalesRep);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool DeleteExternalOrganizationSalesReps(ExternalSalesRep[] users)
    {
      List<string> userids = new List<string>();
      ((IEnumerable<ExternalSalesRep>) users).ToList<ExternalSalesRep>().ForEach((Action<ExternalSalesRep>) (item => userids.Add(item.userId)));
      if (this.primarySalesRepUser == null)
        this.GetPrimarySalesRep();
      if (this.primarySalesRepUser != null)
      {
        string userId = this.primarySalesRepUser.userId;
        if (userids.Contains(userId))
          throw new Exception("Cannot delete primary sales rep user.");
      }
      this.mngr.DeleteExternalOrganizationSalesReps(this.externalOrg.oid, userids.ToArray());
      this.externalSalesRepListForOrg = (List<ExternalSalesRep>) null;
      return true;
    }

    /// <summary>Method to get sales reps assigned to the organization</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep">ExternalSalesRep</see></returns>
    public List<ExternalSalesRep> GetExternalOrgSalesRepsForCurrentOrg()
    {
      if (this.externalSalesRepListForOrg == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.ExternalSalesRepListForOrg
        });
      return this.externalSalesRepListForOrg;
    }

    /// <summary>
    /// Method to get internal users who are assigned to the external organization as sales rep
    /// </summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see></returns>
    public List<User> GetExternalOrgSalesRepUsersForCurrentOrg()
    {
      List<string> stringList = new List<string>();
      List<User> usersForCurrentOrg = new List<User>();
      this.GetExternalOrgSalesRepsForCurrentOrg();
      foreach (ExternalSalesRep externalSalesRep in this.externalSalesRepListForOrg)
        stringList.Add(externalSalesRep.userId);
      foreach (UserInfo info in (IEnumerable) this.orgMngr.GetUsers(stringList.ToArray()).Values)
      {
        if (info != (UserInfo) null)
          usersForCurrentOrg.Add(new User(this.Session, info, false));
      }
      return usersForCurrentOrg;
    }

    /// <summary>
    /// Method to get sales reps from the company of the organization
    /// </summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalSalesRep">ExternalSalesRep</see></returns>
    public List<ExternalSalesRep> GetExternalOrgSalesRepsForCompany()
    {
      if (this.externalSalesRepListForCompany == null)
        this.externalSalesRepListForCompany = ExternalSalesRep.ToList(this.mngr.GetExternalOrgSalesRepsForCompany(this.companyOrg.oid), this.externalOrg.oid);
      return this.externalSalesRepListForCompany;
    }

    /// <summary>
    /// Method to get Encompass users eligible to be sales rep based on the persona setting.
    /// </summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see></returns>
    public UserList GetAllInternalUsers()
    {
      if (this.assignableSalesReps == null)
      {
        this.assignableUsersAsSalesRep = new List<User>();
        UserInfo[] allInternalUsers = this.orgMngr.GetAllAccessibleSalesRepUsers();
        if (this.externalSalesRepListForOrg == null)
          this.GetExternalOrgSalesRepsForCurrentOrg();
        for (int i = 0; i < allInternalUsers.Length; ++i)
        {
          if (this.externalSalesRepListForOrg.Find((Predicate<ExternalSalesRep>) (e1 => e1.userId.Equals(allInternalUsers[i].Userid))) == null)
            this.assignableUsersAsSalesRep.Add(new User(this.Session, allInternalUsers[i], false));
        }
      }
      return new UserList((IList) this.assignableUsersAsSalesRep);
    }

    /// <summary>
    /// Method to add a list of Encompass users as sales rep of the organization
    /// </summary>
    /// <param name="user">A list of <see cref="T:EllieMae.Encompass.BusinessObjects.Users.User">User</see> to add.</param>
    public void AddInternalUserAsSalesRep(User[] user)
    {
      this.GetAllInternalUsers();
      for (int i = 0; i < user.Length; ++i)
      {
        if (this.assignableUsersAsSalesRep.Find((Predicate<User>) (e1 => e1.ID.Equals(user[i].ID))) == null)
          throw new Exception("User " + user[i].FirstName + " " + user[i].LastName + " cannot be added");
      }
      List<ExternalOrgSalesRep> reps = new List<ExternalOrgSalesRep>();
      ((IEnumerable<User>) user).ToList<User>().ForEach((Action<User>) (item => reps.Add(new ExternalOrgSalesRep(0, this.externalOrg.oid, item.ID, this.externalOrg.CompanyLegalName, this.externalOrg.OrganizationName, string.Format("{0} {1}", (object) item.FirstName, (object) item.LastName), string.Empty, item.Phone, item.Email))));
      this.mngr.AddExternalOrganizationSalesReps(reps.ToArray());
      this.externalSalesRepListForOrg = (List<ExternalSalesRep>) null;
      this.assignableSalesReps = (IEnumerable<UserInfo>) null;
    }

    /// <summary>Method to get all external Fees</summary>
    /// <returns>an ExternalFeesList object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retrieve all
    ///       All External Org Fees
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all external fees for the organization
    ///       ExternalFeesList feesList = externalOrg.GetAllExternalFees();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalFeesList GetAllExternalFees()
    {
      if (this.feesList == null)
        this.feesList = ExternalFees.ToList(this.mngr.GetFeeManagement(this.externalOrg.oid));
      return this.feesList;
    }

    /// <summary>Method to get all external Fees by Channel</summary>
    /// <param name="channel">ExternalOrganizationEntityType</param>
    /// <returns>an ExternalFeesList object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retrieve all
    ///       All External Org Fees by Channel
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all external fees for the organization by channel
    ///       ExternalFeesList feesList = externalOrg.GetExternalFeesByChannel(ExternalOrganizationEntityType.Broker);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalFeesList GetExternalFeesByChannel(ExternalOrganizationEntityType channel)
    {
      if (channel == ExternalOrganizationEntityType.None)
        return (ExternalFeesList) null;
      if (this.feesList == null)
        this.feesList = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      if (channel == ExternalOrganizationEntityType.Both)
        return this.feesList;
      ExternalFeesList externalFeesByChannel = new ExternalFeesList();
      foreach (ExternalFees fees in (CollectionBase) this.feesList)
      {
        if (fees.Channel == channel || fees.Channel == ExternalOrganizationEntityType.Both)
          externalFeesByChannel.Add(fees);
      }
      return externalFeesByChannel;
    }

    /// <summary>Method to get all external Fees by Status</summary>
    /// <param name="status">ExternalOriginatorFeeStatus</param>
    /// <returns>an ExternalFeesList object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retrieve all
    ///       All External Org Fees by Status
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all external fees for the organization by channel
    ///       ExternalFeesList feesList = externalOrg.GetExternalFeesByStatus(ExternalOriginatorFeeStatus.Active);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalFeesList GetExternalFeesByStatus(ExternalOriginatorFeeStatus status)
    {
      if (this.feesList == null)
        this.feesList = ExternalFees.ToList(this.mngr.GetFeeManagement(-1));
      ExternalFeesList externalFeesByStatus = new ExternalFeesList();
      foreach (ExternalFees fees in (CollectionBase) this.feesList)
      {
        if (fees.Status == status)
          externalFeesByStatus.Add(fees);
      }
      return externalFeesByStatus;
    }

    /// <summary>Method to add an external Fee</summary>
    /// <param name="fee">ExternalFees</param>
    /// <returns>an ExternalFeesList object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and adds a manual External Fee
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Create an External Fee
    ///       ExternalFees fee = new ExternalFees();
    ///       fee.FeeName = "SampleFee";
    ///       fee.Channel = ExternalOrganizationEntityType.Correspondent;
    ///       fee.FeeAmount = 100;
    ///       fee.Code = "SampleCode";
    ///       fee.StartDate = DateTime.Now;
    ///       fee.EndDate = DateTime.Now.AddDays(1);
    ///       //Add new external fees for the organization
    ///       ExternalFeesList feesList = externalOrg.AddExternalFees(fee);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalFeesList AddExternalFees(ExternalFees fee)
    {
      if (fee.FeeName == "")
        throw new Exception("Name cannot be blank.");
      if (fee.Channel == ExternalOrganizationEntityType.None)
        throw new Exception("Please select a channel.");
      if (fee.FeeAmount == 0.0)
        throw new Exception("Fee Amount cannot be blank.");
      if (fee.Code == "")
        throw new Exception("Code cannot be blank.");
      if (fee.EndDate < fee.StartDate && fee.StartDate != DateTime.MinValue && fee.EndDate != DateTime.MinValue)
        throw new Exception("Start Date cannot be greater than End Date.");
      ExternalFeeManagement feeManagement = new ExternalFeeManagement(this.externalOrg.oid, fee.FeeName, fee.Code, (ExternalOriginatorEntityType) fee.Channel, fee.StartDate, fee.EndDate, 0, "", "", fee.FeePercent, fee.FeeAmount, fee.FeeBasedOn, ExternalOriginatorStatus.Active)
      {
        Description = fee.Description
      };
      feeManagement.DateUpdated = feeManagement.DateCreated = DateTime.Now;
      feeManagement.UpdatedBy = feeManagement.CreatedBy = this.Session.UserID;
      this.mngr.InsertFeeManagementSettings(feeManagement, this.externalOrg.oid);
      this.feesList = (ExternalFeesList) null;
      return this.GetAllExternalFees();
    }

    /// <summary>Method to edit an external Fee</summary>
    /// <param name="fee">ExternalFees</param>
    /// <returns />
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all External Fees and edit an External Fee
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all external fees for the organization by channel
    ///       ExternalFeesList feesList = externalOrg.GetExternalFeesByStatus(ExternalOriginatorFeeStatus.Active);
    /// 
    ///       //Edit an External Fee
    ///       if (feesList.Count > 0)
    ///       {
    ///         ExternalFees fee = feesList[0];
    ///         fee.FeeAmount = 500;
    ///         fee.EndDate = fee.EndDate.AddDays(1);
    ///         //Edit an external fee
    ///         externalOrg.EditExternalFees(fee);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void EditExternalFees(ExternalFees fee)
    {
      if (fee.FeeName == "")
        throw new Exception("Name cannot be blank.");
      if (fee.Channel == ExternalOrganizationEntityType.None)
        throw new Exception("Please select a channel.");
      if (fee.FeeAmount == 0.0)
        throw new Exception("Fee Amount cannot be blank.");
      if (fee.Code == "")
        throw new Exception("Code cannot be blank.");
      if (fee.EndDate < fee.StartDate && fee.StartDate != DateTime.MinValue && fee.EndDate != DateTime.MinValue)
        throw new Exception("Start Date cannot be greater than End Date.");
      this.mngr.UpdateFeeManagementSettings(new ExternalFeeManagement(this.externalOrg.oid, fee.FeeName, fee.Code, (ExternalOriginatorEntityType) fee.Channel, fee.StartDate, fee.EndDate, 0, "", "", fee.FeePercent, fee.FeeAmount, fee.FeeBasedOn, ExternalOriginatorStatus.Active)
      {
        FeeManagementID = fee.FeeManagementID,
        Description = fee.Description,
        DateUpdated = DateTime.Now,
        UpdatedBy = this.Session.UserID
      });
      this.feesList = (ExternalFeesList) null;
    }

    /// <summary>Method to delete an external Fee</summary>
    /// <param name="fee">ExternalFees</param>
    /// <returns />
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all External Fees and delete an External Fee
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all external fees for the organization by channel
    ///       ExternalFeesList feesList = externalOrg.GetExternalFeesByStatus(ExternalOriginatorFeeStatus.Expired);
    /// 
    ///       //Edit an External Fee
    ///       if (feesList.Count > 0)
    ///       {
    ///         ExternalFees fee = feesList[0];
    /// 
    ///         //Delete an external fee
    ///         externalOrg.DeleteExternalFees(fee);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void DeleteExternalFees(ExternalFees fee)
    {
      this.mngr.DeleteFeeManagementSettings(new List<int>()
      {
        fee.FeeManagementID
      });
      this.feesList = (ExternalFeesList) null;
    }

    /// <summary>Method to get the Late Fee Settings</summary>
    /// <returns>LateFeeSettings</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives the Late Fee Settings
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get Late Fee Settings
    ///       LateFeeSettings settings = externalOrg.GetExternalLateFeeSettings();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public LateFeeSettings GetExternalLateFeeSettings()
    {
      return new LateFeeSettings(this.mngr.GetExternalOrgLateFeeSettings(this.externalOrg.oid, false));
    }

    /// <summary>Method to get all DBA Names</summary>
    /// <returns>an ExternalDBAList object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all DBA names
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all DBA Names
    ///       ExternalDBAList dbaList = externalOrg.GetAllDBANames();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalDBAList GetAllDBANames()
    {
      if (this.DBAList == null)
        this.DBAList = ExternalDBAName.ToList(this.mngr.GetDBANames(this.externalOrg.oid));
      return this.DBAList;
    }

    /// <summary>Method to get the default DBA Name</summary>
    /// <returns>an ExternalDBAName object</returns>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreive the default DBA name
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get the default DBA Name
    ///       ExternalDBAName defaultDBA = externalOrg.GetDefaultDBAName();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public ExternalDBAName GetDefaultDBAName()
    {
      if (this.DBAList == null)
        this.DBAList = ExternalDBAName.ToList(this.mngr.GetDBANames(this.externalOrg.oid));
      foreach (ExternalDBAName dba in (CollectionBase) this.DBAList)
      {
        if (dba.SetAsDefault)
          return dba;
      }
      return (ExternalDBAName) null;
    }

    /// <summary>Method to set the default DBA Name</summary>
    /// <param name="dba">an ExternalDBAName object</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all DBA names and set the default DBA name
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all DBA Names
    ///       ExternalDBAList dbaList = externalOrg.GetAllDBANames();
    /// 
    ///       if (dbaList.Count > 0)
    ///       {
    ///           //Set the default DBA Name
    ///           externalOrg.SetDefaultDBAName(dbaList[0]);
    ///       }
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void SetDefaultDBAName(ExternalDBAName dba)
    {
      this.mngr.SetDBANameAsDefault(this.externalOrg.oid, dba.DBAID);
      this.DBAList = (ExternalDBAList) null;
    }

    /// <summary>Method to add a DBA Name</summary>
    /// <param name="name">DBA Name</param>
    /// <param name="setDefault">Is Default</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and add a DBA name
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organization based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Add a DBA Name and set to default
    ///       externalOrg.AddDBAName("sampleName", true);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void AddDBAName(string name, bool setDefault)
    {
      int DBAID = this.mngr.InsertDBANames(new ExternalOrgDBAName(this.externalOrg.oid, name, setDefault), this.externalOrg.oid);
      if (setDefault)
        this.mngr.SetDBANameAsDefault(this.externalOrg.oid, DBAID);
      this.DBAList = (ExternalDBAList) null;
    }

    /// <summary>Method to edit a DBA Name or IsDefault property</summary>
    /// <param name="dba">an ExternalDBAName object</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all DBA names and edit a DBA name and IsDefault property
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organization based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all DBA Names
    ///       ExternalDBAList dbaList = externalOrg.GetAllDBANames();
    /// 
    ///       if (dbaList.Count > 0)
    ///       {
    ///           ExternalDBAName dba = dbaList[0];
    ///           dba.Name = "NewSample";
    ///           dba.SetAsDefault = true;
    ///           //Edit the DBA Name
    ///           externalOrg.EditDBAName(dba);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void EditDBAName(ExternalDBAName dba)
    {
      this.mngr.UpdateDBANames(dba.Original);
      this.DBAList = (ExternalDBAList) null;
    }

    /// <summary>Method to delete a DBA Name</summary>
    /// <param name="dba">an ExternalDBAName object</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all DBA names and delete a DBA name
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organization based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all DBA Names
    ///       ExternalDBAList dbaList = externalOrg.GetAllDBANames();
    /// 
    ///       if (dbaList.Count > 0)
    ///       {
    ///           externalOrg.DeleteDBAName(dbaList[0]);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void DeleteDBAName(ExternalDBAName dba)
    {
      this.mngr.DeleteDBANames(new List<ExternalOrgDBAName>()
      {
        dba.Original
      });
      Dictionary<int, ExternalDBAName> dbas = new Dictionary<int, ExternalDBAName>();
      int key = 0;
      this.DBAList = (ExternalDBAList) null;
      foreach (ExternalDBAName allDbaName in (CollectionBase) this.GetAllDBANames())
      {
        dbas.Add(key, allDbaName);
        ++key;
      }
      this.ChangeSortIndexDBANames(dbas);
    }

    /// <summary>Method to change the sort Index of the DBA Names</summary>
    /// <param name="dbas">a Dictionary with the sortIndex of the DBA name as Key and the ExternalDBAName object as Value</param>
    /// <example>
    ///       The following code retrieves an organization from the Encompass Server and retreives all DBA names and reverse the sort Index
    ///       <code>
    ///         <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organization based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all DBA Names
    ///       ExternalDBAList dbaList = externalOrg.GetAllDBANames();
    /// 
    ///       if (dbaList.Count > 0)
    ///       {
    ///           Dictionary<int, ExternalDBAName> sortList = new Dictionary<int, ExternalDBAName>();
    ///           int count = dbaList.Count - 1;
    ///           foreach(ExternalDBAName dba in dbaList)
    ///           {
    ///               sortList.Add(count, dba);
    ///               count--;
    ///           }
    ///           externalOrg.ChangeSortIndexDBANames(sortList);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    ///       </code>
    ///     </example>
    public void ChangeSortIndexDBANames(Dictionary<int, ExternalDBAName> dbas)
    {
      if (this.DBAList == null)
        this.DBAList = ExternalDBAName.ToList(this.mngr.GetDBANames(this.externalOrg.oid));
      foreach (ExternalDBAName dba in (CollectionBase) this.DBAList)
      {
        if (!dbas.Values.Contains<ExternalDBAName>(dba))
          throw new Exception("Make sure all the DBA Names of this external organization are included in the Dictionary object with their sort order as key");
      }
      Dictionary<int, int> dbas1 = new Dictionary<int, int>();
      foreach (KeyValuePair<int, ExternalDBAName> dba in dbas)
        dbas1.Add(dba.Key, dba.Value.DBAID);
      this.mngr.SetDBANamesSortIndex(dbas1, this.externalOrg.oid);
      this.DBAList = (ExternalDBAList) null;
    }

    /// <summary>Get All External Org Documents</summary>
    /// <returns></returns>
    public Dictionary<int, ExternalDocumentList> GetAllExternalOrgDocuments()
    {
      return ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, -1, false));
    }

    /// <summary>Get All External Org Documents by Channel</summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel)
    {
      return channel == ExternalOrganizationEntityType.Both ? ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, -1, false)) : ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, (int) channel, -1, false));
    }

    /// <summary>Get All External Org Documents by Status</summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrgOriginatorStatus status)
    {
      return ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, (int) status, false));
    }

    /// <summary>Get All External Org Documents by Channel, Status</summary>
    /// <param name="channel"></param>
    /// <param name="status"></param>
    /// <param name="disableGlobalDocs"></param>
    /// <returns></returns>
    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel,
      ExternalOrgOriginatorStatus status,
      bool disableGlobalDocs)
    {
      return channel == ExternalOrganizationEntityType.Both ? ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, (int) status, disableGlobalDocs)) : ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, (int) channel, (int) status, disableGlobalDocs));
    }

    /// <summary>Get All Archived Org Documents</summary>
    /// <returns></returns>
    public ExternalDocumentList GetAllArchivedDocuments()
    {
      return ExternalDocumentsSettings.ToList(this.mngr.GetAllArchiveDocuments(this.externalOrg.oid));
    }

    /// <summary>Add External Document to Org</summary>
    /// <param name="document"></param>
    /// <param name="fileObject"></param>
    /// <param name="isTopOfCategory"></param>
    public void AddExternalDocument(
      ExternalDocumentsSettings document,
      DataObject fileObject,
      bool isTopOfCategory)
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
      if (document.FileName.Trim() == "")
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "The file has not been specified.");
      if (document.DisplayName.Trim() == "")
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Document Display Name cannot be blank.");
      if (document.StartDate < DateTime.MinValue || document.StartDate > DateTime.MaxValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Invalid data for Start Date.");
      if (document.EndDate < DateTime.MinValue || document.EndDate > DateTime.MaxValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Invalid data for End Date.");
      if (document.EndDate < document.StartDate && document.StartDate != DateTime.MinValue && document.EndDate != DateTime.MinValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Start Date cannot be greater than End Date.");
      if (fileObject == null)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "File object cannot be null.");
      if (fileObject.Data.Length > 25000000)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentExceedSize, "File attachments cannot exceed 25 MB. Please select another file.");
      string str1 = Path.GetExtension(document.FileName).ToLower().Trim();
      if (Array.IndexOf<string>(array, str1) < 0)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "The '" + str1 + "' file type is not supported. The allowed file types are '.pdf', '.doc', '.docx', '.xls', '.xlsx', '.txt', '.tif', '.jpg', '.jpeg', '.jpe', '.csv', and '.xml'.");
      if (fileObject == null)
        return;
      string str2 = document.Guid.ToString() + str1;
      BinaryObject data = new BinaryObject(fileObject.Data);
      this.mngr.CreateDocumentInDataFolder(new FileSystemEntry("\\\\" + document.FileName, FileSystemEntry.Types.File, (string) null), data);
      document.AddedBy = this.Session.GetUserInfo().FullName;
      document.DateAdded = DateTime.Now;
      document.IsArchive = false;
      document.FileSize = Utils.FormatByteSize((long) fileObject.Data.Length);
      this.mngr.AddDocument(this.externalOrg.oid, ExternalDocumentsSettings.ToDocumentSettingInfoObj(document), isTopOfCategory);
    }

    /// <summary>Update External Document Settings</summary>
    /// <param name="document"></param>
    public void UpdateExternalDocument(ExternalDocumentsSettings document)
    {
      string[] strArray = new string[12]
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
      if (document.FileName.Trim() == "")
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "The file has not been specified.");
      if (document.DisplayName.Trim() == "")
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Document Display Name cannot be blank.");
      if (document.StartDate < DateTime.MinValue || document.StartDate > DateTime.MaxValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Invalid data for Start Date.");
      if (document.EndDate < DateTime.MinValue || document.EndDate > DateTime.MaxValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Invalid data for End Date.");
      if (document.EndDate < document.StartDate && document.StartDate != DateTime.MinValue && document.EndDate != DateTime.MinValue)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "Start Date cannot be greater than End Date.");
      this.mngr.UpdateDocument(this.externalOrg.oid, ExternalDocumentsSettings.ToDocumentSettingInfoObj(document));
    }

    /// <summary>Delete External Document</summary>
    /// <param name="document"></param>
    public void DeleteExternalDocument(ExternalDocumentsSettings document)
    {
      string str = Path.GetExtension(document.FileName).ToLower().Trim();
      FileSystemEntry entry = new FileSystemEntry("\\\\" + (document.Guid.ToString() + str), FileSystemEntry.Types.File, (string) null);
      this.mngr.DeleteDocument(this.externalOrg.oid, document.Guid, entry);
    }

    /// <summary>Archive External Document</summary>
    /// <param name="guid"></param>
    public void ArchiveExternalDocument(string guid)
    {
      this.mngr.ArchiveDocuments(this.externalOrg.oid, new List<string>()
      {
        guid
      });
    }

    /// <summary>UnArchive External Documents</summary>
    /// <param name="guids"></param>
    public void UnArchiveExternalDocuments(List<string> guids)
    {
      this.mngr.UnArchiveDocuments(this.externalOrg.oid, guids);
    }

    /// <summary>Change Active Checked External Document</summary>
    /// <param name="document"></param>
    /// <param name="activeChecked"></param>
    public void ChangeActiveCheckedExternalDocument(
      ExternalDocumentsSettings document,
      bool activeChecked)
    {
      this.mngr.UpdateActiveStatus(this.externalOrg.oid, activeChecked, document.IsDefault, document.Guid);
    }

    /// <summary>
    /// Change Active Checked for External Documents within selected category
    /// </summary>
    /// <param name="category"></param>
    /// <param name="activeChecked"></param>
    public void ChangeActiveCheckedExternalDocument(int category, bool activeChecked)
    {
      this.mngr.UpdateActiveStatusAllDocsInCategory(this.externalOrg.oid, category, activeChecked);
    }

    /// <summary>Get Global External Documents to Assign</summary>
    /// <returns></returns>
    public ExternalDocumentList GetGlobalExternalDocumentsToAssign()
    {
      return ExternalDocumentsSettings.ToList(this.mngr.GetExternalDocumentsForOrgAssignment(this.externalOrg.oid));
    }

    /// <summary>Assign Global Document to Org</summary>
    /// <param name="document"></param>
    /// <param name="isTopOfCategory"></param>
    public void AssignGlobalDocumentToOrg(ExternalDocumentsSettings document, bool isTopOfCategory)
    {
      if (!document.IsDefault)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "This document is not from Global Settings.");
      document.ExternalOrgId = this.externalOrg.oid;
      this.mngr.AssignDocumentToOrg(ExternalDocumentsSettings.ToDocumentSettingInfoObj(document), isTopOfCategory);
    }

    /// <summary>Swap Sort Order of Documents</summary>
    /// <param name="firstDocument"></param>
    /// <param name="secondDocument"></param>
    public void SwapSortOrderOfDocuments(
      ExternalDocumentsSettings firstDocument,
      ExternalDocumentsSettings secondDocument)
    {
      this.mngr.SwapDocumentSortIds(this.externalOrg.oid, ExternalDocumentsSettings.ToDocumentSettingInfoObj(firstDocument), ExternalDocumentsSettings.ToDocumentSettingInfoObj(secondDocument));
    }

    /// <summary>Read Document from Data Folder</summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public DataObject ReadDocumentFromDataFolder(ExternalDocumentsSettings document)
    {
      try
      {
        string str = Path.GetExtension(document.FileName).ToLower().Trim();
        return new DataObject(this.mngr.ReadDocumentFromDataFolder(document.Guid.ToString() + str));
      }
      catch (Exception ex)
      {
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.DocumentDoesNotExist, "Document doesn't exist.");
      }
    }

    /// <summary>Remove Assigned Doc From TPO</summary>
    /// <param name="guid"></param>
    public void RemoveAssignedDocFromTPO(string guid)
    {
      this.mngr.RemoveAssignedDocFromTPO(guid, this.externalOrg.oid);
    }

    private void ensureExists()
    {
      if (this.externalOrg == null)
        throw new InvalidOperationException("This operation is not valid until object is commited");
    }

    internal static ExternalOrganizationList ToList(
      Session session,
      ExternalOriginatorManagementData[] externalOrgs,
      bool hasPerformancepatch)
    {
      ExternalOrganizationList list = new ExternalOrganizationList();
      List<ExternalOriginatorManagementData> companyOrgs = new List<ExternalOriginatorManagementData>((IEnumerable<ExternalOriginatorManagementData>) externalOrgs);
      for (int index = 0; index < externalOrgs.Length; ++index)
        list.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(session, companyOrgs, externalOrgs[index].oid, false, hasPerformancepatch));
      return list;
    }

    private static ExternalOriginatorManagementData GetExternalCompany(
      int orgID,
      List<ExternalOriginatorManagementData> orgList)
    {
      ExternalOriginatorManagementData company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == orgID));
      while (company != null && company.Parent > 0)
        company = orgList.FirstOrDefault<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.oid == company.Parent));
      return company;
    }

    private static ExternalOriginatorManagementData GetExternalBranch(
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

    private void getAllDetailInfo()
    {
      List<ExternalOrganizationSetting> settings = new List<ExternalOrganizationSetting>();
      if (this.assignableSalesReps == null)
        settings.Add(ExternalOrganizationSetting.AssignableSalesReps);
      if (this.attachmentCategoryList == null)
        settings.Add(ExternalOrganizationSetting.AttachmentCategory);
      if (this.companyRatingList == null)
        settings.Add(ExternalOrganizationSetting.CompanyRating);
      if (this.companyStatusList == null)
        settings.Add(ExternalOrganizationSetting.CompanyStatus);
      if (this.contactStatusList == null)
        settings.Add(ExternalOrganizationSetting.ContactStatus);
      if (this.licensing == null)
        settings.Add(ExternalOrganizationSetting.License);
      if (this.loanTypes == null)
        settings.Add(ExternalOrganizationSetting.LoanTypes);
      if (this.priceGroupList == null)
        settings.Add(ExternalOrganizationSetting.PriceGroup);
      if (this.urlList == null)
        settings.Add(ExternalOrganizationSetting.UrlList);
      if ((UserInfo) this.primaryManager == (UserInfo) null)
        settings.Add(ExternalOrganizationSetting.PrimaryManager);
      if (this.notesList == null)
        settings.Add(ExternalOrganizationSetting.Note);
      if (this.attachmentsList == null)
        settings.Add(ExternalOrganizationSetting.Attachment);
      if (this.externalLOCompPlanList == null)
        settings.Add(ExternalOrganizationSetting.LOComp);
      if (this.loanCompHistoryList == null)
        settings.Add(ExternalOrganizationSetting.LOCompHistory);
      this.BatchLoadSettings(settings);
    }

    /// <summary>Batch Load Settings</summary>
    /// <param name="settings">List of different areas to preload data</param>
    public void BatchLoadSettings(List<ExternalOrganizationSetting> settings)
    {
      List<ExternalOriginatorOrgSetting> requested = new List<ExternalOriginatorOrgSetting>();
      if (settings.Contains(ExternalOrganizationSetting.AssignableSalesReps) && this.assignableSalesReps == null)
        requested.Add(ExternalOriginatorOrgSetting.AssignableSalesReps);
      if (settings.Contains(ExternalOrganizationSetting.AttachmentCategory) && this.attachmentCategoryList == null)
        requested.Add(ExternalOriginatorOrgSetting.AttachmentCategory);
      if (settings.Contains(ExternalOrganizationSetting.CompanyRating) && this.companyRatingList == null)
        requested.Add(ExternalOriginatorOrgSetting.CompanyRating);
      if (settings.Contains(ExternalOrganizationSetting.CompanyStatus) && this.companyStatusList == null)
        requested.Add(ExternalOriginatorOrgSetting.CompanyStatus);
      if (settings.Contains(ExternalOrganizationSetting.ContactStatus) && this.contactStatusList == null)
        requested.Add(ExternalOriginatorOrgSetting.ContactStatus);
      if (settings.Contains(ExternalOrganizationSetting.License) && this.licensing == null)
        requested.Add(ExternalOriginatorOrgSetting.License);
      if (settings.Contains(ExternalOrganizationSetting.LoanTypes) && this.loanTypes == null)
        requested.Add(ExternalOriginatorOrgSetting.LoanTypes);
      if (settings.Contains(ExternalOrganizationSetting.PriceGroup) && this.priceGroupList == null)
        requested.Add(ExternalOriginatorOrgSetting.PriceGroup);
      if (settings.Contains(ExternalOrganizationSetting.UrlList) && this.urlList == null)
        requested.Add(ExternalOriginatorOrgSetting.UrlList);
      if (settings.Contains(ExternalOrganizationSetting.PrimaryManager) && (UserInfo) this.primaryManager == (UserInfo) null)
        requested.Add(ExternalOriginatorOrgSetting.PrimaryManager);
      if (settings.Contains(ExternalOrganizationSetting.Note) && this.notesList == null)
        requested.Add(ExternalOriginatorOrgSetting.Note);
      if (settings.Contains(ExternalOrganizationSetting.Attachment) && this.attachmentsList == null)
        requested.Add(ExternalOriginatorOrgSetting.Attachment);
      if (settings.Contains(ExternalOrganizationSetting.LOComp) && this.externalLOCompPlanList == null)
        requested.Add(ExternalOriginatorOrgSetting.LOComp);
      if (this.loanCompHistoryList == null)
        requested.Add(ExternalOriginatorOrgSetting.LoCompHistory);
      if (settings.Contains(ExternalOrganizationSetting.ExternalSalesRepListForOrg) && (this.externalSalesRepListForOrg == null || this.primarySalesRepUser == null))
        requested.Add(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg);
      Dictionary<ExternalOriginatorOrgSetting, object> dictionary = new Dictionary<ExternalOriginatorOrgSetting, object>();
      if (this.hasPerformancePatch)
      {
        dictionary = this.mngr.GetExternalAdditionalDetails(this.ID, requested);
      }
      else
      {
        if (requested.Contains(ExternalOriginatorOrgSetting.AssignableSalesReps) || requested.Contains(ExternalOriginatorOrgSetting.AttachmentCategory) || requested.Contains(ExternalOriginatorOrgSetting.CompanyRating) || requested.Contains(ExternalOriginatorOrgSetting.CompanyStatus) || requested.Contains(ExternalOriginatorOrgSetting.ContactStatus) || requested.Contains(ExternalOriginatorOrgSetting.License) || requested.Contains(ExternalOriginatorOrgSetting.LoanTypes) || requested.Contains(ExternalOriginatorOrgSetting.PriceGroup))
        {
          List<object> additionalDetails = this.mngr.GetExternalAdditionalDetails(this.ID);
          dictionary.Add(ExternalOriginatorOrgSetting.AssignableSalesReps, additionalDetails[2]);
          dictionary.Add(ExternalOriginatorOrgSetting.AttachmentCategory, additionalDetails[6]);
          dictionary.Add(ExternalOriginatorOrgSetting.CompanyRating, additionalDetails[5]);
          dictionary.Add(ExternalOriginatorOrgSetting.CompanyStatus, additionalDetails[3]);
          dictionary.Add(ExternalOriginatorOrgSetting.ContactStatus, additionalDetails[4]);
          dictionary.Add(ExternalOriginatorOrgSetting.License, additionalDetails[0]);
          dictionary.Add(ExternalOriginatorOrgSetting.LoanTypes, additionalDetails[1]);
          dictionary.Add(ExternalOriginatorOrgSetting.PriceGroup, additionalDetails[7]);
        }
        if (requested.Contains(ExternalOriginatorOrgSetting.UrlList) && this.urlList == null)
          dictionary.Add(ExternalOriginatorOrgSetting.UrlList, (object) this.mngr.GetSelectedOrgUrls(this.externalOrg.oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.PrimaryManager) && (UserInfo) this.primaryManager == (UserInfo) null)
          dictionary.Add(ExternalOriginatorOrgSetting.PrimaryManager, (object) this.mngr.GetExternalUserInfo(this.externalOrg.Manager));
        if (requested.Contains(ExternalOriginatorOrgSetting.Note) && this.notesList == null)
          dictionary.Add(ExternalOriginatorOrgSetting.Note, (object) this.mngr.GetExternalOrganizationNotes(this.externalOrg.oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.Attachment) && this.attachmentsList == null)
          dictionary.Add(ExternalOriginatorOrgSetting.Attachment, (object) this.mngr.GetExternalAttachmentsByOid(this.externalOrg.oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.LOComp) && this.externalLOCompPlanList == null)
          dictionary.Add(ExternalOriginatorOrgSetting.LOComp, (object) this.mngr.GetAllCompPlans(true, 2));
        if (requested.Contains(ExternalOriginatorOrgSetting.LoCompHistory) && this.loanCompHistoryList == null)
          dictionary.Add(ExternalOriginatorOrgSetting.LoCompHistory, (object) this.mngr.GetCompPlansByoid(false, this.externalOrg.oid));
        if (requested.Contains(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg) && this.primarySalesRepUser == null)
        {
          List<object> objectList = new List<object>();
          ExternalOriginatorManagementData externalOrganization = this.mngr.GetExternalOrganization(false, this.externalOrg.oid);
          objectList.Add(externalOrganization != null ? (object) externalOrganization.PrimarySalesRepUserId : (object) string.Empty);
          objectList.Add((object) this.mngr.GetExternalOrgSalesRepsForCurrentOrg(this.externalOrg.oid));
          objectList.Add(externalOrganization == null || !(externalOrganization.PrimarySalesRepAssignedDate != DateTime.MinValue) ? (object) string.Empty : (object) externalOrganization.PrimarySalesRepAssignedDate.ToString());
          dictionary.Add(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg, (object) objectList);
        }
      }
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.AssignableSalesReps) && this.assignableSalesReps == null)
        this.assignableSalesReps = (IEnumerable<UserInfo>) dictionary[ExternalOriginatorOrgSetting.AssignableSalesReps];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.AttachmentCategory) && this.attachmentCategoryList == null)
        this.attachmentCategoryList = (List<ExternalSettingValue>) dictionary[ExternalOriginatorOrgSetting.AttachmentCategory];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.CompanyRating) && this.companyRatingList == null)
        this.companyRatingList = (List<ExternalSettingValue>) dictionary[ExternalOriginatorOrgSetting.CompanyRating];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.CompanyStatus) && this.companyStatusList == null)
        this.companyStatusList = (List<ExternalSettingValue>) dictionary[ExternalOriginatorOrgSetting.CompanyStatus];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.ContactStatus) && this.contactStatusList == null)
        this.contactStatusList = (List<ExternalSettingValue>) dictionary[ExternalOriginatorOrgSetting.ContactStatus];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.License) && this.licensing == null)
        this.licensing = new ExternalLicensing((BranchExtLicensing) dictionary[ExternalOriginatorOrgSetting.License]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.LoanTypes) && this.loanTypes == null)
        this.loanTypes = new ExternalLoanTypes((ExternalOrgLoanTypes) dictionary[ExternalOriginatorOrgSetting.LoanTypes]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.PriceGroup) && this.priceGroupList == null)
        this.priceGroupList = (List<ExternalSettingValue>) dictionary[ExternalOriginatorOrgSetting.PriceGroup];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.UrlList) && this.urlList == null)
        this.urlList = ExternalUrl.ToList(((List<ExternalOrgURL>) dictionary[ExternalOriginatorOrgSetting.UrlList]).ToArray());
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.PrimaryManager) && (UserInfo) this.primaryManager == (UserInfo) null)
        this.primaryManager = (ExternalUserInfo) dictionary[ExternalOriginatorOrgSetting.PrimaryManager];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.Note) && this.notesList == null)
        this.notesList = ExternalNote.ToList((ExternalOrgNotes) dictionary[ExternalOriginatorOrgSetting.Note]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.Attachment) && this.attachmentsList == null)
        this.attachmentsList = ExternalAttachment.ToList((List<ExternalOrgAttachments>) dictionary[ExternalOriginatorOrgSetting.Attachment]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.LOComp) && this.externalLOCompPlanList == null)
        this.externalLOCompPlanList = ExternalLoanCompPlan.ToList((List<LoanCompPlan>) dictionary[ExternalOriginatorOrgSetting.LOComp]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.LoCompHistory) && this.loanCompHistoryList == null)
      {
        this.loanCompHistoryList = (LoanCompHistoryList) dictionary[ExternalOriginatorOrgSetting.LoCompHistory];
        this.externalLOCompHistoryList = ExternalLoanCompHistory.ToList(this.loanCompHistoryList);
      }
      if (!dictionary.Keys.Contains<ExternalOriginatorOrgSetting>(ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg))
        return;
      List<object> objectList1 = (List<object>) dictionary[ExternalOriginatorOrgSetting.ExternalSalesRepListForOrg];
      this.primarySalesRepUserId = string.Concat(objectList1[0]);
      if (objectList1.Count > 2)
        this.primarySalesRepAssignedDate = objectList1[2].ToString();
      this.externalSalesRepListForOrg = ExternalSalesRep.ToList((List<ExternalOrgSalesRep>) objectList1[1], this.ID);
      this.primarySalesRepUser = this.externalSalesRepListForOrg.Find((Predicate<ExternalSalesRep>) (e1 => e1.userId.Equals(this.primarySalesRepUserId)));
    }

    /// <summary>Get outstanding commitments</summary>
    /// <returns></returns>
    private Dictionary<CorrespondentMasterDeliveryType, Decimal> GetOutstandingCommentments()
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommentments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      ICorrespondentTradeManager correspondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments;
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments;
      if (this.externalOrg.OrganizationType == ExternalOriginatorOrgType.Company)
      {
        standingCommitments = correspondentTradeManager.GetOutStandingCommitments(this.externalOrg.oid);
        outstandingCommitments = configurationManager.GetNonAllocatedOutstandingCommitments(this.externalOrg.ExternalID);
      }
      else
      {
        standingCommitments = correspondentTradeManager.GetOutStandingCommitments(this.companyOrg.oid);
        outstandingCommitments = configurationManager.GetNonAllocatedOutstandingCommitments(this.companyOrg.ExternalID);
      }
      foreach (CorrespondentMasterDeliveryType key in (CorrespondentMasterDeliveryType[]) Enum.GetValues(typeof (CorrespondentMasterDeliveryType)))
      {
        Decimal num = 0M;
        if (standingCommitments.ContainsKey(key))
          num += standingCommitments[key];
        if (outstandingCommitments.ContainsKey(key))
          num += outstandingCommitments[key];
        if (outstandingCommentments.ContainsKey(key))
          outstandingCommentments[key] = num;
        else
          outstandingCommentments.Add(key, num);
      }
      return outstandingCommentments;
    }

    /// <summary>
    /// Gets list of extension organizations with the same external ID from the immediate child nodes or the whole company
    /// </summary>
    /// <param name="immediateChildOnly">indicates whether to return external organization only from the immediate child.</param>
    /// <returns>a list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see></returns>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExtensionOrganizations(
      bool immediateChildOnly)
    {
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> result = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      this.companyOrgs.ForEach((Action<ExternalOriginatorManagementData>) (x =>
      {
        if (immediateChildOnly)
        {
          if (x.Parent != this.externalOrg.oid)
            return;
          switch (x.OrganizationType)
          {
            case ExternalOriginatorOrgType.CompanyExtension:
            case ExternalOriginatorOrgType.BranchExtension:
              result.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, x.oid, false, this.hasPerformancePatch));
              break;
          }
        }
        else
        {
          if (!(x.ExternalID == this.externalOrg.ExternalID))
            return;
          switch (x.OrganizationType)
          {
            case ExternalOriginatorOrgType.CompanyExtension:
            case ExternalOriginatorOrgType.BranchExtension:
              result.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, x.oid, false, this.hasPerformancePatch));
              break;
          }
        }
      }));
      return result;
    }

    /// <summary>Method to get all branches of the organization</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see></returns>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetAllBranches()
    {
      IEnumerable<ExternalOriginatorManagementData> originatorManagementDatas = this.companyOrgs.Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.OrganizationType == ExternalOriginatorOrgType.Branch));
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> allBranches = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      if (originatorManagementDatas != null)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in originatorManagementDatas)
          allBranches.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch));
      }
      return allBranches;
    }

    /// <summary>Get All branches assigned to a particular site</summary>
    /// <param name="siteID"></param>
    /// <returns></returns>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetAllBranches(
      string siteID)
    {
      if (!this.hasPerformancePatch)
        throw new Exception("This API is not available with current Encompass Server version.");
      List<ExternalOriginatorManagementData> organizationBranchesBySite = this.mngr.GetExternalOrganizationBranchesBySite(false, this.ID, siteID);
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> allBranches = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      if (organizationBranchesBySite != null)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in organizationBranchesBySite)
          allBranches.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch));
      }
      return allBranches;
    }

    /// <summary>
    /// Commits the changes to the current ExternalOrganization.
    /// </summary>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server, modifies
    /// its company legal name and save back to database.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       externalOrg.CompanyLegalName = "New legal name";
    ///       externalOrg.Commit();
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
      string message = this.ValidateExternalOrg();
      if (!string.IsNullOrEmpty(message))
        throw new Exception(message);
      if (this.loanCompHistoryList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.LOCompHistory
        });
      this.mngr.UpdateExternalContact(false, this.externalOrg, this.loanCompHistoryList);
      ExternalOrgLoanTypes externalOrgLoanType = this.GetExternalOrgLoanType(this.LoanTypes);
      if (externalOrgLoanType == null)
        throw new Exception("No Parent information found for Lon Type.");
      BranchExtLicensing externalOrgLicensing = this.GetExternalOrgLicensing(this.licensing);
      if (externalOrgLicensing == null)
        throw new Exception("No Parent information found for Licensing Criteria.");
      this.mngr.UpdateExternalOrganizationLoanTypes(this.externalOrg.oid, externalOrgLoanType);
      this.mngr.UpdateExternalLicence(externalOrgLicensing, this.externalOrg.oid);
      if (this.customFields != null)
        this.mngr.UpdateCustomFieldValues(this.externalOrg.oid, this.customFields.FieldsCollection);
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.OrganizationID));
    }

    private string ValidateExternalOrg()
    {
      if (this.EntityType == ExternalOrganizationEntityType.None)
        return "Channel/Entity type can not be blank.";
      if ((this.EntityType == ExternalOrganizationEntityType.Both || this.EntityType == ExternalOrganizationEntityType.Correspondent) && this.UnderwritingType == ExternalOrgUnderwritingType.None)
        return "Underwriting Type is required if Channel/Entity type is set to Correspondent.";
      foreach (EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType stateLicenseExtType in this.Licensing.StateLicenseExtTypes)
      {
        if (!this.IsDateInRange(stateLicenseExtType.IssueDate))
          return "The value of Issue Date '" + stateLicenseExtType.IssueDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
        if (!this.IsDateInRange(stateLicenseExtType.StartDate))
          return "The value of Start Date '" + stateLicenseExtType.StartDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
        if (!this.IsDateInRange(stateLicenseExtType.EndDate))
          return "The value of End Date '" + stateLicenseExtType.EndDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
        if (!this.IsDateInRange(stateLicenseExtType.StatusDate))
          return "The value of Status Date '" + stateLicenseExtType.StatusDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
        if (!this.IsDateInRange(stateLicenseExtType.LastChecked))
          return "The value of Last Checked Date '" + stateLicenseExtType.LastChecked.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
      }
      if (!this.IsDateInRange(this.LoanTypes.FHAApprovedDate))
        return "The value of FHA Approved Date '" + this.LoanTypes.FHAApprovedDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
      if (!this.IsDateInRange(this.LoanTypes.FHAExpirationDate))
        return "The value of FHA Expiration Date '" + this.LoanTypes.FHAExpirationDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
      if (!this.IsDateInRange(this.LoanTypes.VAApprovedDate))
        return "The value of VA Approved Date '" + this.LoanTypes.VAApprovedDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
      if (this.IsDateInRange(this.LoanTypes.VAExpirationDate))
        return "";
      return "The value of VA Expiration Date '" + this.LoanTypes.VAExpirationDate.ToString("d") + "' is outside the date range of " + this.minValue.ToString("MM/dd/yyyy") + " - " + this.maxValue.ToString("MM/dd/yyyy") + ".";
    }

    private bool IsDateInRange(DateTime date)
    {
      DateTime dateTime = Convert.ToDateTime(date);
      return !(dateTime > this.maxValue) && !(dateTime < this.minValue) || !(date != DateTime.MinValue);
    }

    private BranchExtLicensing GetExternalOrgLicensing(ExternalLicensing licensing)
    {
      if (licensing.UseParentInfo)
      {
        BranchExtLicensing extLicenseDetails = this.mngr.GetExtLicenseDetails(this.ParentOrganizationID);
        if (extLicenseDetails == null)
          return (BranchExtLicensing) null;
        extLicenseDetails.UseParentInfo = true;
        return extLicenseDetails;
      }
      BranchLicensing.ATRExemptCreditors atrExemptCreditor = (BranchLicensing.ATRExemptCreditors) licensing.ATRExemptCreditor;
      BranchLicensing.ATRSmallCreditors atrSmallCreditor = (BranchLicensing.ATRSmallCreditors) licensing.ATRSmallCreditor;
      return new BranchExtLicensing(licensing.UseParentInfo, licensing.AllowLoansWithIssues, licensing.MsgUploadNonApprovedLoans, licensing.LenderType, licensing.HomeState, licensing.OptOut, licensing.StatutoryElectionInMaryland, licensing.StatutoryElectionInMaryland2, licensing.StatutoryElectionInKansas, this.GetStateLicenseExtType(licensing.StateLicenseExtTypes), licensing.UseCustomLenderProfile, atrSmallCreditor, atrExemptCreditor);
    }

    private List<EllieMae.EMLite.RemotingServices.StateLicenseExtType> GetStateLicenseExtType(
      List<EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType> extType)
    {
      List<EllieMae.EMLite.RemotingServices.StateLicenseExtType> stateLicenseExtType1 = new List<EllieMae.EMLite.RemotingServices.StateLicenseExtType>();
      foreach (EllieMae.Encompass.BusinessObjects.Users.StateLicenseExtType stateLicenseExtType2 in extType)
      {
        List<EllieMae.EMLite.RemotingServices.StateLicenseExtType> stateLicenseExtTypeList = stateLicenseExtType1;
        EllieMae.EMLite.RemotingServices.StateLicenseExtType stateLicenseExtType3 = new EllieMae.EMLite.RemotingServices.StateLicenseExtType(stateLicenseExtType2.StateAbbrevation, stateLicenseExtType2.LicenseType, stateLicenseExtType2.LicenseNo, stateLicenseExtType2.IssueDate, stateLicenseExtType2.StartDate, stateLicenseExtType2.EndDate, stateLicenseExtType2.LicenseStatus, stateLicenseExtType2.StatusDate, stateLicenseExtType2.Approved, stateLicenseExtType2.Exempt, stateLicenseExtType2.LastChecked, stateLicenseExtType2.SortIndex);
        stateLicenseExtType3.Selected = stateLicenseExtType2.Selected;
        stateLicenseExtTypeList.Add(stateLicenseExtType3);
      }
      return stateLicenseExtType1;
    }

    private ExternalOrgLoanTypes GetExternalOrgLoanType(ExternalLoanTypes loanType)
    {
      if (loanType.UseParentInfoFhaVa)
      {
        ExternalOrgLoanTypes organizationLoanTypes = this.mngr.GetExternalOrganizationLoanTypes(this.ParentOrganizationID);
        if (organizationLoanTypes == null)
          return (ExternalOrgLoanTypes) null;
        organizationLoanTypes.UseParentInfoFhaVa = true;
        return organizationLoanTypes;
      }
      return new ExternalOrgLoanTypes()
      {
        AdvancedCode = loanType.AdvancedCode,
        AdvancedCodeXml = loanType.AdvancedCodeXml,
        Broker = this.GetExternalOrgChannelLoanType(loanType.Broker),
        CorrespondentDelegated = this.GetExternalOrgChannelLoanType(loanType.CorrespondentDelegated),
        CorrespondentNonDelegated = this.GetExternalOrgChannelLoanType(loanType.CorrespondentNonDelegated),
        ExternalOrgID = loanType.ExternalOrgID,
        FHAApprovedDate = loanType.FHAApprovedDate,
        FHACompareRatio = loanType.FHACompareRatio,
        FHAExpirationDate = loanType.FHAExpirationDate,
        FHAId = loanType.FHAId,
        FHASonsorId = loanType.FHASponsorId,
        FHAStatus = loanType.FHAStatus,
        Id = loanType.Id,
        Underwriting = (int) loanType.Underwriting,
        UseParentInfoFhaVa = loanType.UseParentInfoFhaVa,
        VAApprovedDate = loanType.VAApprovedDate,
        VAExpirationDate = loanType.VAExpirationDate,
        VAId = loanType.VAId,
        VAStatus = loanType.VAStatus,
        FHADirectEndorsement = this.loanTypes.FHADirectEndorsement,
        VASponsorID = this.loanTypes.VASponsorID,
        FHMLCApproved = this.loanTypes.FHMLCApproved,
        FNMAApproved = this.loanTypes.FNMAApproved,
        FannieMaeID = this.loanTypes.FannieMaeID,
        FreddieMacID = this.loanTypes.FreddieMacID,
        AUSMethod = this.loanTypes.AUSMethod
      };
    }

    private ExternalOrgLoanTypes.ExternalOrgChannelLoanType GetExternalOrgChannelLoanType(
      ExternalChannelLoanType channelLoanType)
    {
      return new ExternalOrgLoanTypes.ExternalOrgChannelLoanType()
      {
        AllowLoansWithIssues = channelLoanType.AllowLoansWithIssues,
        ChannelType = channelLoanType.ChannelType,
        ExternalOrgID = channelLoanType.ExternalOrgID,
        LoanPurpose = channelLoanType.GetLoanPurposeValue(),
        LoanTypes = channelLoanType.GetLoanTypeValue(),
        MsgUploadNonApprovedLoans = channelLoanType.MsgUploadNonApprovedLoans
      };
    }

    /// <summary>
    /// Method to get a list of sub organizations based on a specific type
    /// </summary>
    /// <param name="organizationType">The organization type to get</param>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization">ExternalOrganization</see></returns>
    /// <example>
    /// The following code retrieves an organization from the Encompass Server and retrieve all
    /// the branch organizations below that organization.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       //Get all branch organizations under the organization
    ///       List<ExternalOrganization> organizations = externalOrg.GetALLExternalOrganizationByType(ExternalOriginationOrgType.Branch);
    /// 
    ///       foreach(ExternalOrganization organization in organizations)
    ///          Console.WriteLine("Organization: " + organization.OrganizationName);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetALLExternalOrganizationByType(
      ExternalOriginationOrgType organizationType)
    {
      List<int> orgType = new List<int>();
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> organizationByType1 = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      List<ExternalOriginatorManagementData> originatorManagementDataList = new List<ExternalOriginatorManagementData>();
      if (this.OrganizationType == ExternalOriginationOrgType.Company)
      {
        if (organizationType != ExternalOriginationOrgType.Branch && organizationType != ExternalOriginationOrgType.CompanyExtension)
          throw new Exception("Company can return only Branch or Company extension");
      }
      else if (this.OrganizationType == ExternalOriginationOrgType.Branch)
      {
        if (organizationType != ExternalOriginationOrgType.BranchExtension)
          throw new Exception("Branch can return only branch extension");
      }
      else if (this.OrganizationType == ExternalOriginationOrgType.CompanyExtension)
      {
        if (organizationType != ExternalOriginationOrgType.CompanyExtension)
          throw new Exception("Company Extension can return only Company extension");
      }
      else if (this.OrganizationType == ExternalOriginationOrgType.BranchExtension && organizationType != ExternalOriginationOrgType.BranchExtension)
        throw new Exception("Branch Extension can return only branch extension");
      orgType.Add(Convert.ToInt32((object) organizationType));
      List<ExternalOriginatorManagementData> organizationByType2 = this.mngr.GetChildExternalOrganizationByType(this.externalOrg.oid, orgType);
      if (organizationByType2 != null)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in organizationByType2)
          organizationByType1.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch));
      }
      return organizationByType1;
    }

    /// <summary>Method to get a list of AttachmentCategory</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.AttachmentCategory">AttachmentCategory</see></returns>
    public List<AttachmentCategory> GetAttachmentCategories()
    {
      if (this.attachmentCategoryList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.AttachmentCategory
        });
      List<AttachmentCategory> result = new List<AttachmentCategory>();
      this.attachmentCategoryList.ForEach((Action<ExternalSettingValue>) (x => result.Add(new AttachmentCategory(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    /// <summary>Method to get a list of CompanyStatus</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.CurrentCompanyStatus">CurrentCompanyStatus</see></returns>
    public List<CurrentCompanyStatus> GetCurrentCompanyStatus()
    {
      if (this.companyStatusList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.CompanyStatus
        });
      List<CurrentCompanyStatus> result = new List<CurrentCompanyStatus>();
      this.companyStatusList.ForEach((Action<ExternalSettingValue>) (x => result.Add(new CurrentCompanyStatus(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    /// <summary>Method to get a list of CompanyRatings</summary>
    /// <returns>A list of <see cref="P:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.CompanyRating">CompanyRating</see></returns>
    /// <example>
    /// The following code retrieves a list of available company ratings from the Encompass Server (TPO Settings) and assign it to the current organization. Ratings are based on your company’s own scale and requirements.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// 
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.Collections;
    /// using EllieMae.Encompass.BusinessObjects.ExternalOrganization;
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
    ///       //Get external organizaiton based on an external Id
    ///       ExternalOrganization externalOrg = session.Organizations.GetExternalOrganization("SampleExternalID");
    /// 
    ///       // Retrieve the current company rating for this organization
    ///       CompanyRating current = externalOrg.CompanyRating;
    /// 
    ///       //Get all possible company ratings from TPO Settings/Company Rating
    ///       List<CompanyRating> ratings = externalOrg.GetCompanyRatings();
    /// 
    ///       //set a different rating to the current organization
    ///       if (current != ratings[0])
    ///            externalOrg.CompanyRating = ratings[0];
    ///       externalOrg.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public List<CompanyRating> GetCompanyRatings()
    {
      if (this.companyRatingList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.CompanyRating
        });
      List<CompanyRating> result = new List<CompanyRating>();
      this.companyRatingList.ForEach((Action<ExternalSettingValue>) (x => result.Add(new CompanyRating(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    /// <summary>Method to get a list of RateSheets</summary>
    /// <returns>A list of <see cref="T:EllieMae.Encompass.BusinessObjects.ExternalOrganization.RateSheet">RateSheet</see></returns>
    public List<RateSheet> GetRateSheets()
    {
      List<ExternalSettingValue> orgSettingsByName = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettingsByName("ICE PPE Rate Sheet");
      List<RateSheet> result = new List<RateSheet>();
      orgSettingsByName.ForEach((Action<ExternalSettingValue>) (x => result.Add(new RateSheet(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    /// <summary>Method to get a list of PriceGroups</summary>
    /// <returns>A list of <see cref="P:EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization.PriceGroup">PriceGroup</see></returns>
    public List<PriceGroup> GetPriceGroups()
    {
      if (this.priceGroupList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.PriceGroup
        });
      List<PriceGroup> result = new List<PriceGroup>();
      this.priceGroupList.ForEach((Action<ExternalSettingValue>) (x => result.Add(new PriceGroup(this.Session, x.settingValue, x.settingCode, x.settingId))));
      return result;
    }

    /// <summary>Get Document Categories</summary>
    /// <returns></returns>
    public List<DocumentCategory> GetDocumentCategories()
    {
      List<ExternalSettingValue> orgSettingsByName = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettingsByName("Document Category");
      List<DocumentCategory> result = new List<DocumentCategory>();
      orgSettingsByName.ForEach((Action<ExternalSettingValue>) (x => result.Add(new DocumentCategory(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    /// <summary>Method to get a list of External Banks</summary>
    /// <returns></returns>
    public ExternalBanksList GetAllExternalBanks()
    {
      if (this.bankList == null)
        this.bankList = ExternalBanks.ToList(((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalBanks());
      return this.bankList;
    }

    /// <summary>Method to get a list of External Warehouses</summary>
    /// <returns></returns>
    public ExternalWarehouseList GetAllExternalOrgWarehouses()
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (this.warehouseList == null)
        this.warehouseList = ExternalWarehouse.ToList(configurationManager.GetExternalOrgWarehouses(this.externalOrg.oid));
      return this.warehouseList;
    }

    /// <summary>Method to add External Warehouse</summary>
    /// <param name="BankID"></param>
    /// <returns></returns>
    public ExternalWarehouseList AddExternalOrgWarehouse(int BankID)
    {
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).AddExternalOrgWarehouse(new ExternalOrgWarehouse()
      {
        BankID = BankID,
        ExternalOrgID = this.externalOrg.oid
      });
      this.warehouseList = (ExternalWarehouseList) null;
      return this.GetAllExternalOrgWarehouses();
    }

    /// <summary>Method to add External Warehouse</summary>
    /// <param name="BankID"></param>
    /// <param name="timeZone"></param>
    /// <returns></returns>
    public ExternalWarehouseList AddExternalOrgWarehouse(int BankID, string timeZone)
    {
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).AddExternalOrgWarehouse(new ExternalOrgWarehouse()
      {
        BankID = BankID,
        ExternalOrgID = this.externalOrg.oid,
        TimeZone = timeZone
      });
      this.warehouseList = (ExternalWarehouseList) null;
      return this.GetAllExternalOrgWarehouses();
    }

    /// <summary>Method to update External Warehouse</summary>
    /// <param name="obj"></param>
    public void UpdateExternalOrgWarehouse(ExternalWarehouse obj)
    {
      IConfigurationManager configurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgWarehouse warehouse = new ExternalOrgWarehouse()
      {
        UseBankContact = obj.UseBankContact,
        ContactName = obj.ContactName,
        ContactEmail = obj.ContactEmail,
        ContactPhone = obj.ContactPhone,
        ContactFax = obj.ContactFax,
        Notes = obj.Notes,
        AcctNumber = obj.AcctNumber,
        Description = obj.Description,
        Expiration = obj.Expiration,
        SelfFunder = obj.SelfFunder,
        BaileeReq = obj.BaileeReq,
        TriParty = obj.TriParty,
        TimeZone = obj.TimeZone,
        Approved = obj.Approved,
        AcctName = obj.AcctName,
        CreditAcctName = obj.CreditAcctName,
        CreditAcctNumber = obj.CreditAcctNumber
      };
      warehouse.TimeZone = obj.TimeZone;
      warehouse.StatusDate = obj.StatusDate;
      configurationManager.UpdateExternalOrgWarehouse(obj.WarehouseID, warehouse);
      this.warehouseList = (ExternalWarehouseList) null;
    }

    /// <summary>Method to delete External Warehouse</summary>
    /// <param name="obj"></param>
    public void DeleteExternalOrgWarehouse(ExternalWarehouse obj)
    {
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).DeleteExternalOrgWarehouse(obj.WarehouseID, this.externalOrg.oid);
      this.warehouseList = (ExternalWarehouseList) null;
    }
  }
}
