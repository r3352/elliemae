// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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

    public int ID => this.externalOrg.oid;

    public int ParentOrganizationID => this.externalOrg.Parent;

    public string OrganizationName
    {
      get => this.externalOrg.OrganizationName;
      set => this.externalOrg.OrganizationName = value;
    }

    public bool DisabledLogin
    {
      get => this.externalOrg.DisabledLogin;
      set => this.externalOrg.DisabledLogin = value;
    }

    public bool MultiFactorAuthentication
    {
      get => this.externalOrg.MultiFactorAuthentication;
      set => this.externalOrg.MultiFactorAuthentication = value;
    }

    public bool NoAfterHourWires
    {
      get => this.externalOrg.NoAfterHourWires;
      set => this.externalOrg.NoAfterHourWires = value;
    }

    public string Timezone
    {
      get => this.externalOrg.Timezone;
      set => this.externalOrg.Timezone = value;
    }

    public ManageFeeLEDisclosures RequestLEDisclosures
    {
      get => this.externalOrg.GenerateDisclosures;
      set => this.externalOrg.GenerateDisclosures = value;
    }

    public ExternalOriginationOrgType OrganizationType
    {
      get
      {
        switch ((int) this.externalOrg.OrganizationType)
        {
          case 0:
            return ExternalOriginationOrgType.Company;
          case 2:
            return ExternalOriginationOrgType.Branch;
          case 3:
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
            this.externalOrg.OrganizationType = (ExternalOriginatorOrgType) 0;
            break;
          case ExternalOriginationOrgType.Branch:
            this.externalOrg.OrganizationType = (ExternalOriginatorOrgType) 2;
            break;
          case ExternalOriginationOrgType.BranchExtension:
            this.externalOrg.OrganizationType = (ExternalOriginatorOrgType) 3;
            break;
          default:
            this.externalOrg.OrganizationType = (ExternalOriginatorOrgType) 1;
            break;
        }
      }
    }

    public ExternalOrganizationEntityType EntityType
    {
      get
      {
        switch (this.externalOrg.entityType - 1)
        {
          case 0:
            return ExternalOrganizationEntityType.Broker;
          case 1:
            return ExternalOrganizationEntityType.Correspondent;
          case 2:
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
            this.externalOrg.entityType = (ExternalOriginatorEntityType) 1;
            break;
          case ExternalOrganizationEntityType.Correspondent:
            this.externalOrg.entityType = (ExternalOriginatorEntityType) 2;
            break;
          case ExternalOrganizationEntityType.Both:
            this.externalOrg.entityType = (ExternalOriginatorEntityType) 3;
            break;
          default:
            this.externalOrg.entityType = (ExternalOriginatorEntityType) 0;
            break;
        }
      }
    }

    public ExternalOrgUnderwritingType UnderwritingType
    {
      get
      {
        switch (this.externalOrg.UnderwritingType - 1)
        {
          case 0:
            return ExternalOrgUnderwritingType.Delegated;
          case 1:
            return ExternalOrgUnderwritingType.NonDelegated;
          case 2:
            return ExternalOrgUnderwritingType.Both;
          default:
            return ExternalOrgUnderwritingType.None;
        }
      }
      set
      {
        if (value != ExternalOrgUnderwritingType.None && (this.externalOrg.entityType & 3 | 2) == null)
          throw new Exception("Entity Type must be Correspondent to set Under Writing type to Delegated or Non-Delegated");
        this.externalOrg.UnderwritingType = (ExternalOriginatorUnderwritingType) Enum.Parse(typeof (ExternalOriginatorUnderwritingType), value.ToString());
      }
    }

    public string ExternalID
    {
      get
      {
        return this.externalOrg.ExternalID.Length > 10 ? this.externalOrg.ExternalID.Substring(1) : this.externalOrg.ExternalID;
      }
      set => this.externalOrg.ExternalID = value;
    }

    public string OrganizationID
    {
      get => this.externalOrg.OrgID;
      set => this.externalOrg.OrgID = value;
    }

    public string OwnerName
    {
      get => this.externalOrg.OwnerName;
      set => this.externalOrg.OwnerName = value;
    }

    public string CompanyLegalName
    {
      get => this.externalOrg.CompanyLegalName;
      set => this.externalOrg.CompanyLegalName = value;
    }

    public string CompanyDBAName
    {
      get => this.externalOrg.CompanyDBAName;
      set => this.externalOrg.CompanyDBAName = value;
    }

    public string Address
    {
      get => this.externalOrg.Address;
      set => this.externalOrg.Address = value;
    }

    public string City
    {
      get => this.externalOrg.City;
      set => this.externalOrg.City = value;
    }

    public string State
    {
      get => this.externalOrg.State;
      set => this.externalOrg.State = value;
    }

    public string Zip
    {
      get => this.externalOrg.Zip;
      set => this.externalOrg.Zip = value;
    }

    public string PhoneNumber
    {
      get => this.externalOrg.PhoneNumber;
      set => this.externalOrg.PhoneNumber = value;
    }

    public string FaxNumber
    {
      get => this.externalOrg.FaxNumber;
      set => this.externalOrg.FaxNumber = value;
    }

    public string Email
    {
      get => this.externalOrg.Email;
      set => this.externalOrg.Email = value;
    }

    public string Website
    {
      get => this.externalOrg.Website;
      set => this.externalOrg.Website = value;
    }

    public DateTime LastLoanSubmitted
    {
      get => this.externalOrg.LastLoanSubmitted;
      set => this.externalOrg.LastLoanSubmitted = value;
    }

    public bool CanAcceptFirstPayments
    {
      get => this.externalOrg.CanAcceptFirstPayments;
      set => this.externalOrg.CanAcceptFirstPayments = value;
    }

    public string EmailForRateSheet
    {
      get => this.externalOrg.EmailForRateSheet;
      set => this.externalOrg.EmailForRateSheet = value;
    }

    public string FaxForRateSheet
    {
      get => this.externalOrg.FaxForRateSheet;
      set => this.externalOrg.FaxForRateSheet = value;
    }

    public string EmailForLockInfo
    {
      get => this.externalOrg.EmailForLockInfo;
      set => this.externalOrg.EmailForLockInfo = value;
    }

    public string FaxForLockInfo
    {
      get => this.externalOrg.FaxForLockInfo;
      set => this.externalOrg.FaxForLockInfo = value;
    }

    public bool InheritProductAndPricing
    {
      get => this.externalOrg.UseParentInfoForEPPS;
      set => this.externalOrg.UseParentInfoForEPPS = value;
    }

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
          if (this.externalOrg.entityType == 2 || this.externalOrg.entityType == null)
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
          if (this.externalOrg.entityType == 1 || this.externalOrg.entityType == null)
            throw new Exception(message1);
          string message2 = "Underwriting Type should be set to Delegated to set Delegated Price Group.";
          if (this.externalOrg.UnderwritingType == null || this.externalOrg.UnderwritingType == 2)
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
          if (this.externalOrg.entityType == 1 || this.externalOrg.entityType == null)
            throw new Exception(message1);
          string message2 = "Underwriting Type should be set to Non-Delegated to set Non-Delegated Price Group.";
          if (this.externalOrg.UnderwritingType == null || this.externalOrg.UnderwritingType == 1)
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
      if (extOrg != null && extOrg.UseParentInfoForEPPS && extOrg.OrganizationType != null && extOrg.Parent != 0)
        this.getProductandPricingData(this.companyOrgs.First<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (item => item.oid == extOrg.Parent)));
      else
        this.pricingOrg = extOrg;
    }

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

    public bool AddToWatchlist
    {
      get => this.externalOrg.AddToWatchlist;
      set => this.externalOrg.AddToWatchlist = value;
    }

    public DateTime CurrentStatusDate
    {
      get => this.externalOrg.CurrentStatusDate;
      set => this.externalOrg.CurrentStatusDate = value;
    }

    public DateTime ApprovedDate
    {
      get => this.externalOrg.ApprovedDate;
      set => this.externalOrg.ApprovedDate = value;
    }

    public DateTime ApplicationDate
    {
      get => this.externalOrg.ApplicationDate;
      set => this.externalOrg.ApplicationDate = value;
    }

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

    public bool Incorporated
    {
      get => this.externalOrg.Incorporated;
      set => this.externalOrg.Incorporated = value;
    }

    public string StateIncorp
    {
      get => this.externalOrg.StateIncorp;
      set => this.externalOrg.StateIncorp = value;
    }

    public DateTime DateOfIncorporation
    {
      get => this.externalOrg.DateOfIncorporation;
      set => this.externalOrg.DateOfIncorporation = value;
    }

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

    public int TypeOfEntity
    {
      get => this.externalOrg.TypeOfEntity;
      set => this.externalOrg.TypeOfEntity = value;
    }

    public string OtherEntityDescription
    {
      get => this.externalOrg.OtherEntityDescription;
      set => this.externalOrg.OtherEntityDescription = value;
    }

    public string TaxID
    {
      get => this.externalOrg.TaxID;
      set => this.externalOrg.TaxID = value;
    }

    public bool UseSSNFormat
    {
      get => this.externalOrg.UseSSNFormat;
      set => this.externalOrg.UseSSNFormat = value;
    }

    public string NmlsId
    {
      get => this.externalOrg.NmlsId;
      set => this.externalOrg.NmlsId = value;
    }

    public string FinancialsPeriod
    {
      get => this.externalOrg.FinancialsPeriod;
      set => this.externalOrg.FinancialsPeriod = value;
    }

    public DateTime FinancialsLastUpdate
    {
      get => this.externalOrg.FinancialsLastUpdate;
      set => this.externalOrg.FinancialsLastUpdate = value;
    }

    public Decimal CompanyNetWorth
    {
      get
      {
        return !this.externalOrg.CompanyNetWorth.HasValue ? 0M : this.externalOrg.CompanyNetWorth.Value;
      }
      set => this.externalOrg.CompanyNetWorth = new Decimal?(value);
    }

    public DateTime EOExpirationDate
    {
      get => this.externalOrg.EOExpirationDate;
      set => this.externalOrg.EOExpirationDate = value;
    }

    public string EOCompany
    {
      get => this.externalOrg.EOCompany;
      set => this.externalOrg.EOCompany = value;
    }

    public string EOPolicyNumber
    {
      get => this.externalOrg.EOPolicyNumber;
      set => this.externalOrg.EOPolicyNumber = value;
    }

    public string MERSOriginatingORGID
    {
      get => this.externalOrg.MERSOriginatingORGID;
      set => this.externalOrg.MERSOriginatingORGID = value;
    }

    public bool DUSponsored
    {
      get => this.externalOrg.DUSponsored == 1;
      set => this.externalOrg.DUSponsored = value ? 1 : 0;
    }

    public bool CanFundInOwnName
    {
      get => this.externalOrg.CanFundInOwnName == 1;
      set => this.externalOrg.CanFundInOwnName = value ? 1 : 0;
    }

    public bool IsTestAccount
    {
      get => this.externalOrg.IsTestAccount;
      set => this.externalOrg.IsTestAccount = value;
    }

    public bool CanCloseInOwnName
    {
      get => this.externalOrg.CanCloseInOwnName == 1;
      set => this.externalOrg.CanCloseInOwnName = value ? 1 : 0;
    }

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

    public bool CommitmentUseBestEffort
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.CommitmentUseBestEffort : this.externalOrg.CommitmentUseBestEffort;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentUseBestEffort = value;
      }
    }

    public bool CommitmentUseBestEffortLimited
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.CommitmentUseBestEffortLimited : this.externalOrg.CommitmentUseBestEffortLimited;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentUseBestEffortLimited = value;
      }
    }

    public Decimal MaxCommitmentAuthority
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.MaxCommitmentAuthority : this.externalOrg.MaxCommitmentAuthority;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.MaxCommitmentAuthority = value;
      }
    }

    public Decimal CommitmentBestEffortsAvailableAmount
    {
      get
      {
        if (!this.CommitmentUseBestEffort)
          return 0M;
        if (!this.CommitmentUseBestEffortLimited)
          return Decimal.MaxValue;
        Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommentments = this.GetOutstandingCommentments();
        return outstandingCommentments.ContainsKey((CorrespondentMasterDeliveryType) 15) ? this.MaxCommitmentAuthority - outstandingCommentments[(CorrespondentMasterDeliveryType) 15] : this.MaxCommitmentAuthority;
      }
    }

    public bool CommitmentMandatory
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.CommitmentMandatory : this.externalOrg.CommitmentMandatory;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentMandatory = value;
      }
    }

    public Decimal MaxCommitmentAmount
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.MaxCommitmentAmount : this.externalOrg.MaxCommitmentAmount;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.MaxCommitmentAmount = value;
      }
    }

    public bool IsCommitmentDeliveryIndividual
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryIndividual : this.externalOrg.IsCommitmentDeliveryIndividual;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryIndividual = value;
      }
    }

    public bool IsCommitmentDeliveryBulk
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryBulk : this.externalOrg.IsCommitmentDeliveryBulk;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryBulk = value;
      }
    }

    public bool IsCommitmentDeliveryAOT
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryAOT : this.externalOrg.IsCommitmentDeliveryAOT;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryAOT = value;
      }
    }

    public bool IsCommitmentDeliveryBulkAOT
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryBulkAOT : this.externalOrg.IsCommitmentDeliveryBulkAOT;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryBulkAOT = value;
      }
    }

    public bool IsCommitmentDeliveryLiveTrade
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryLiveTrade : this.externalOrg.IsCommitmentDeliveryLiveTrade;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryLiveTrade = value;
      }
    }

    public bool IsCommitmentDeliveryCoIssue
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryCoIssue : this.externalOrg.IsCommitmentDeliveryCoIssue;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryCoIssue = value;
      }
    }

    public bool IsCommitmentDeliveryForward
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.IsCommitmentDeliveryForward : this.externalOrg.IsCommitmentDeliveryForward;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.IsCommitmentDeliveryForward = value;
      }
    }

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
          if (keyValuePair.Key == 20 && this.IsCommitmentDeliveryIndividual)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 5 && this.IsCommitmentDeliveryAOT)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 30 && this.IsCommitmentDeliveryBulk)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 35 && this.IsCommitmentDeliveryBulkAOT)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 40 && this.IsCommitmentDeliveryCoIssue)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 10 && this.IsCommitmentDeliveryForward)
            num += keyValuePair.Value;
          if (keyValuePair.Key == 25 && this.IsCommitmentDeliveryLiveTrade)
            num += keyValuePair.Value;
        }
        return this.MaxCommitmentAmount - num;
      }
    }

    public ExternalOriginationCommitmentPolicy CommitmentPolicy
    {
      get
      {
        if (this.externalOrg.OrganizationType != null)
        {
          ExternalOriginatorCommitmentPolicy commitmentPolicy = this.companyOrg.CommitmentPolicy;
          if (commitmentPolicy == null)
            return ExternalOriginationCommitmentPolicy.NoRestriction;
          return commitmentPolicy == 1 ? ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit : ExternalOriginationCommitmentPolicy.DontAllowLoanCreation;
        }
        ExternalOriginatorCommitmentPolicy commitmentPolicy1 = this.externalOrg.CommitmentPolicy;
        if (commitmentPolicy1 == null)
          return ExternalOriginationCommitmentPolicy.NoRestriction;
        return commitmentPolicy1 == 1 ? ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit : ExternalOriginationCommitmentPolicy.DontAllowLoanCreation;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        if (value != ExternalOriginationCommitmentPolicy.DontAllowLockorSubmit)
        {
          if (value == ExternalOriginationCommitmentPolicy.DontAllowLoanCreation)
            this.externalOrg.CommitmentPolicy = (ExternalOriginatorCommitmentPolicy) 2;
          else
            this.externalOrg.CommitmentPolicy = (ExternalOriginatorCommitmentPolicy) 0;
        }
        else
          this.externalOrg.CommitmentPolicy = (ExternalOriginatorCommitmentPolicy) 1;
      }
    }

    public ExternalOriginationCorrespondentTradeCreationPolicy CorrespondentTradePolicy
    {
      get
      {
        if (this.externalOrg.OrganizationType != null)
        {
          ExternalOriginatorCommitmentTradePolicy commitmentTradePolicy = this.companyOrg.CommitmentTradePolicy;
          return commitmentTradePolicy == null || commitmentTradePolicy != 1 ? ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction : ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation;
        }
        ExternalOriginatorCommitmentTradePolicy commitmentTradePolicy1 = this.externalOrg.CommitmentTradePolicy;
        return commitmentTradePolicy1 == null || commitmentTradePolicy1 != 1 ? ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction : ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        if (value != ExternalOriginationCorrespondentTradeCreationPolicy.NoRestriction)
        {
          if (value != ExternalOriginationCorrespondentTradeCreationPolicy.DontAllowTradeCreation)
            return;
          this.externalOrg.CommitmentTradePolicy = (ExternalOriginatorCommitmentTradePolicy) 1;
        }
        else
          this.externalOrg.CommitmentTradePolicy = (ExternalOriginatorCommitmentTradePolicy) 0;
      }
    }

    public string CommitmentMessage
    {
      get
      {
        return this.externalOrg.OrganizationType != null ? this.companyOrg.CommitmentMessage : this.externalOrg.CommitmentMessage;
      }
      set
      {
        if (this.externalOrg.OrganizationType != null)
          throw new Exception("This property can only be set at the Company level.");
        this.externalOrg.CommitmentMessage = value;
      }
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company
    {
      get
      {
        return new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, this.companyOrg.oid, false, this.hasPerformancePatch);
      }
    }

    public EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch
    {
      get
      {
        return this.branchOrg == null ? (EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization) null : new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, this.branchOrg.oid, false, this.hasPerformancePatch);
      }
    }

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
        return UserInfo.op_Equality(info, (UserInfo) null) ? (User) null : new User(this.Session, info, false);
      }
    }

    public DateTime PrimarySalesRepAssignedDate
    {
      get => this.externalOrg.PrimarySalesRepAssignedDate;
      set => this.externalOrg.PrimarySalesRepAssignedDate = value;
    }

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

    public ExternalUser PrimaryManager
    {
      get
      {
        if (UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null))
          this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
          {
            ExternalOrganizationSetting.PrimaryManager
          });
        return UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null) ? (ExternalUser) null : new ExternalUser(this.Session, this.primaryManager, (ExternalUserInfo) null, this.companyOrgs, false, false, this.hasPerformancePatch);
      }
      set
      {
        this.externalOrg.Manager = value.IsManager ? value.ID : throw new Exception("The assigned contact is not a manager.");
        this.primaryManager = value.info;
      }
    }

    public string PrimaryManagerExternalUserID => this.externalOrg.Manager;

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
      ExternalUserInfo externalUserInfo1 = new ExternalUserInfo();
      externalUserInfo1.EmailForLogin = loginEmail;
      externalUserInfo1.ExternalOrgID = this.ID;
      ((UserInfo) externalUserInfo1).FirstName = firstName;
      ((UserInfo) externalUserInfo1).LastName = lastName;
      externalUserInfo1.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      externalUserInfo1.SalesRepID = this.externalOrg.PrimarySalesRepUserId;
      ExternalUserInfo externalUserInfo2 = externalUserInfo1;
      externalUserInfo2.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(externalUserInfo2).ExternalUserID, password, DateTime.Now, true);
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
      ExternalUserInfo externalUserInfo1 = new ExternalUserInfo();
      externalUserInfo1.EmailForLogin = loginEmail;
      externalUserInfo1.ExternalOrgID = this.ID;
      ((UserInfo) externalUserInfo1).FirstName = firstName;
      ((UserInfo) externalUserInfo1).LastName = lastName;
      externalUserInfo1.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      ExternalUserInfo externalUserInfo2 = externalUserInfo1;
      if (primaySalesRep != null)
        externalUserInfo2.SalesRepID = primaySalesRep.ID;
      externalUserInfo2.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(externalUserInfo2).ExternalUserID, password, DateTime.Now, true);
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
      ExternalUserInfo externalUserInfo1 = new ExternalUserInfo();
      externalUserInfo1.EmailForLogin = loginEmail;
      externalUserInfo1.ExternalOrgID = this.ID;
      ((UserInfo) externalUserInfo1).FirstName = firstName;
      ((UserInfo) externalUserInfo1).LastName = lastName;
      externalUserInfo1.ContactID = string.Concat((object) ExternalUserInfo.NewContactID(allContactId));
      externalUserInfo1.SalesRepID = this.externalOrg.PrimarySalesRepUserId;
      ExternalUserInfo externalUserInfo2 = externalUserInfo1;
      externalUserInfo2.UpdatedDateTime = DateTime.Now;
      ExternalUserInfo info = this.mngr.ResetExternalUserInfoPassword(this.mngr.SaveExternalUserInfo(externalUserInfo2).ExternalUserID, password, DateTime.Now, true);
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

    public ExternalUserList GetUsers()
    {
      this.ensureExists();
      ExternalUserInfo[] externalUserInfos = this.mngr.GetAllExternalUserInfos(this.ExternalID);
      return externalUserInfos != null ? ExternalUser.ToList(this.Session, externalUserInfos, this.companyOrgs, this.hasPerformancePatch) : (ExternalUserList) null;
    }

    public ExternalUrlList GetUrlList()
    {
      if (this.urlList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.UrlList
        });
      return this.urlList;
    }

    public ExternalUrl AddExternalUrl(
      ExternalSiteUrl Url,
      ExternalOrganizationEntityType entityType)
    {
      this.ensureExists();
      if (this.EntityType != ExternalOrganizationEntityType.Both && this.EntityType != entityType)
        throw new ExternalOrganizationUrlException(OrganizationUrlViolationType.InvalidOrganizationUrlCreation, "Channel Type of the Url has to be same as the company organization.");
      return new ExternalUrl(this.mngr.AssociateExternalOrganisationUrl(this.externalOrg.oid, Url.SiteId, Convert.ToInt32((object) entityType)));
    }

    public ExternalUrl AddExternalUrl(ExternalUrl Url)
    {
      this.ensureExists();
      if (this.EntityType != ExternalOrganizationEntityType.Both && this.EntityType != Url.EntityType)
        throw new ExternalOrganizationUrlException(OrganizationUrlViolationType.InvalidOrganizationUrlCreation, "Channel Type of the Url has to be same as the company organization.");
      return new ExternalUrl(this.mngr.AssociateExternalOrganisationUrl(this.externalOrg.oid, Url.SiteId, Convert.ToInt32((object) Url.EntityType)));
    }

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

    public void DeleteExternalUrl(ExternalUrl url)
    {
      this.mngr.DeleteExternalOrgSelectedUrl(this.externalOrg.oid, url.URLID);
    }

    public void UpdateStateLicense()
    {
      this.ensureExists();
      BranchExtLicensing branchExtLicensing = this.licensing != null && this.licensing.BranchExternalLicensing != null ? new BranchExtLicensing((BranchLicensing) this.licensing.BranchExternalLicensing) : throw new InvalidOperationException("This operation is not valid until object is not null");
      this.mngr.UpdateExternalLicence(this.licensing.BranchExternalLicensing, this.externalOrg.oid);
    }

    public void UpdateStateLicense(List<StateLicenseExtType> stateLicenses)
    {
      this.ensureExists();
      if (this.licensing == null || this.licensing.BranchExternalLicensing == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.License
        });
      this.licensing.BranchExternalLicensing.StateLicenseExtTypes = new List<StateLicenseExtType>();
      for (int index = 0; index < stateLicenses.Count; ++index)
        this.licensing.BranchExternalLicensing.StateLicenseExtTypes.Add(new StateLicenseExtType(stateLicenses[index].StateAbbrevation, stateLicenses[index].LicenseType, stateLicenses[index].LicenseNo, stateLicenses[index].IssueDate, stateLicenses[index].StartDate, stateLicenses[index].EndDate, stateLicenses[index].LicenseStatus, stateLicenses[index].StatusDate, stateLicenses[index].Approved, stateLicenses[index].Exempt, stateLicenses[index].LastChecked));
      ((BranchLicensing) this.licensing.BranchExternalLicensing).UseParentInfo = false;
      this.mngr.UpdateExternalLicence(this.licensing.BranchExternalLicensing, this.externalOrg.oid);
    }

    public ExternalNotesList GetAllNotes()
    {
      if (this.notesList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.Note
        });
      return this.notesList;
    }

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

    public void DeleteExternalNote(ExternalNotesList notes)
    {
      List<int> intList = new List<int>();
      foreach (ExternalNote note in (CollectionBase) notes)
        intList.Add(note.NoteID);
      this.mngr.DeleteExternalOrganizationNotes(this.externalOrg.oid, intList);
      this.notesList = (ExternalNotesList) null;
    }

    public int GetNumberOfNotes()
    {
      if (this.notesList == null)
        this.GetAllNotes();
      return this.notesList.Count;
    }

    public List<ExternalAttachment> GetAllAttachments()
    {
      if (this.attachmentsList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.Attachment
        });
      return this.attachmentsList;
    }

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
      int num = -1;
      if (category != null)
        num = category.ID;
      ExternalOrgAttachments externalOrgAttachment = new ExternalOrgAttachments(Guid.NewGuid(), this.externalOrg.oid, fileName, description, dateAdded, num, fileDate, this.Session.UserID, expirationDate, daysToExpire, fileLocation);
      this.mngr.InsertExternalAttachment(externalOrgAttachment);
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) externalOrgAttachment.Guid;
      string str = ((IEnumerable<string>) fileLocation.Split('.')).Last<string>();
      this.mngr.AddAttachment(new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), (FileSystemEntry.Types) 2, (string) null), file.Unwrap());
      this.attachmentsList = (List<ExternalAttachment>) null;
      return new ExternalAttachment(externalOrgAttachment);
    }

    public void UpdateExternalAttachment(ExternalAttachment attachment, DataObject file)
    {
      string message = this.validateAttachment(attachment.RealFileName, file.Unwrap());
      if (message != "")
        throw new Exception(message);
      this.mngr.UpdateExternalAttachment(new ExternalOrgAttachments(attachment.Guid, attachment.ExternalOrgID, attachment.FileName, attachment.Description, attachment.DateAdded, attachment.Category, attachment.FileDate, attachment.UserWhoAdded, attachment.ExpirationDate, attachment.DaysToExpire, attachment.RealFileName));
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      this.mngr.AddAttachment(new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), (FileSystemEntry.Types) 2, (string) null), file.Unwrap());
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

    public void DeleteExternalAttachment(ExternalAttachment attachment)
    {
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + (guid.ToString() + "." + str), (FileSystemEntry.Types) 2, (string) null);
      this.mngr.DeleteExternalAttachment(attachment.Guid, fileSystemEntry);
      this.attachmentsList = (List<ExternalAttachment>) null;
    }

    public bool GetExternalAttachmentIsExpired()
    {
      return this.mngr.GetExternalAttachmentIsExpired(this.externalOrg.oid);
    }

    public int GetNumberOfAttachments()
    {
      if (this.attachmentsList == null)
        this.GetAllAttachments();
      return this.attachmentsList.Count;
    }

    public DataObject ReadAttachment(ExternalAttachment attachment)
    {
      // ISSUE: variable of a boxed type
      __Boxed<Guid> guid = (ValueType) attachment.Guid;
      string str = ((IEnumerable<string>) attachment.RealFileName.Split('.')).Last<string>();
      return new DataObject(this.mngr.ReadAttachment(guid.ToString() + "." + str));
    }

    public ExternalLoanCompPlanList GetAllLOCompPlans()
    {
      if (this.externalLOCompPlanList == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.LOComp
        });
      return this.externalLOCompPlanList;
    }

    public int GetNumberOfLOCompPlans()
    {
      if (this.externalLOCompPlanList == null)
        this.GetAllLOCompPlans();
      return this.externalLOCompPlanList.Count;
    }

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

    public int GetNumberOfLOCompHistory()
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return this.externalLOCompHistoryList.Count;
    }

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

    public ExternalLoanCompHistory GetCurrentPlan(DateTime triggerDateTime)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      LoanCompHistory currentPlan = this.loanCompHistoryList.GetCurrentPlan(triggerDateTime);
      return currentPlan == null ? (ExternalLoanCompHistory) null : new ExternalLoanCompHistory(currentPlan);
    }

    public ExternalLoanCompHistoryList GetCurrentAndFuturePlans(DateTime todayDate)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return ExternalLoanCompHistory.ToList(this.loanCompHistoryList.GetCurrentAndFuturePlans(todayDate));
    }

    public ExternalLoanCompHistoryList GetFuturePlans(DateTime todayDate)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      return ExternalLoanCompHistory.ToList(this.loanCompHistoryList.GetFuturePlans(todayDate));
    }

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

    public ExternalLoanCompHistory GetLoCompHistoryByPlanId(int compPlanId)
    {
      if (this.externalLOCompHistoryList == null)
        this.GetLOCompHistory();
      LoanCompHistory historyByCompPlanId = this.loanCompHistoryList.getLoanCompHistoryByCompPlanId(compPlanId);
      return historyByCompPlanId != null ? new ExternalLoanCompHistory(historyByCompPlanId) : (ExternalLoanCompHistory) null;
    }

    public bool DeleteLoCompHistory(ExternalLoanCompHistoryList compPlans)
    {
      List<int> intList = new List<int>();
      foreach (ExternalLoanCompHistory compPlan in (CollectionBase) compPlans)
        intList.Add(compPlan.CompPlanId);
      this.mngr.DeleteExternalCompPlans(false, this.externalOrg.oid, intList.ToArray());
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return true;
    }

    public bool DeleteLoCompHistory(ExternalLoanCompHistory compPlan)
    {
      this.mngr.DeleteExternalCompPlans(false, this.externalOrg.oid, compPlan.CompPlanId);
      this.externalLOCompHistoryList = (ExternalLoanCompHistoryList) null;
      return true;
    }

    public bool SetSalesRepAsPrimary(User user)
    {
      if (this.externalSalesRepListForOrg == null)
        this.GetExternalOrgSalesRepsForCurrentOrg();
      ExternalSalesRep externalSalesRep = this.externalSalesRepListForOrg.Find((Predicate<ExternalSalesRep>) (e1 => e1.userId.Equals(user.ID)));
      if (externalSalesRep == null)
        throw new Exception("Not a valid user in the current organization");
      this.mngr.SetSalesRepAsPrimary(user.ID, this.externalOrg.oid, new DateTime());
      this.primarySalesRepUser = externalSalesRep;
      return true;
    }

    public ExternalSalesRep GetPrimarySalesRep()
    {
      if (this.primarySalesRepUser == null || this.externalSalesRepListForOrg == null || this.primarySalesRepUser == null)
        this.GetExternalOrgSalesRepsForCurrentOrg();
      return this.primarySalesRepUser;
    }

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

    public List<ExternalSalesRep> GetExternalOrgSalesRepsForCurrentOrg()
    {
      if (this.externalSalesRepListForOrg == null)
        this.BatchLoadSettings(new List<ExternalOrganizationSetting>()
        {
          ExternalOrganizationSetting.ExternalSalesRepListForOrg
        });
      return this.externalSalesRepListForOrg;
    }

    public List<User> GetExternalOrgSalesRepUsersForCurrentOrg()
    {
      List<string> stringList = new List<string>();
      List<User> usersForCurrentOrg = new List<User>();
      this.GetExternalOrgSalesRepsForCurrentOrg();
      foreach (ExternalSalesRep externalSalesRep in this.externalSalesRepListForOrg)
        stringList.Add(externalSalesRep.userId);
      foreach (UserInfo info in (IEnumerable) this.orgMngr.GetUsers(stringList.ToArray()).Values)
      {
        if (UserInfo.op_Inequality(info, (UserInfo) null))
          usersForCurrentOrg.Add(new User(this.Session, info, false));
      }
      return usersForCurrentOrg;
    }

    public List<ExternalSalesRep> GetExternalOrgSalesRepsForCompany()
    {
      if (this.externalSalesRepListForCompany == null)
        this.externalSalesRepListForCompany = ExternalSalesRep.ToList(this.mngr.GetExternalOrgSalesRepsForCompany(this.companyOrg.oid), this.externalOrg.oid);
      return this.externalSalesRepListForCompany;
    }

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

    public void AddInternalUserAsSalesRep(User[] user)
    {
      this.GetAllInternalUsers();
      for (int i = 0; i < user.Length; ++i)
      {
        if (this.assignableUsersAsSalesRep.Find((Predicate<User>) (e1 => e1.ID.Equals(user[i].ID))) == null)
          throw new Exception("User " + user[i].FirstName + " " + user[i].LastName + " cannot be added");
      }
      List<ExternalOrgSalesRep> reps = new List<ExternalOrgSalesRep>();
      ((IEnumerable<User>) user).ToList<User>().ForEach((Action<User>) (item => reps.Add(new ExternalOrgSalesRep(0, this.externalOrg.oid, item.ID, this.externalOrg.CompanyLegalName, this.externalOrg.OrganizationName, string.Format("{0} {1}", (object) item.FirstName, (object) item.LastName), string.Empty, item.Phone, item.Email, false, false, false, false))));
      this.mngr.AddExternalOrganizationSalesReps(reps.ToArray());
      this.externalSalesRepListForOrg = (List<ExternalSalesRep>) null;
      this.assignableSalesReps = (IEnumerable<UserInfo>) null;
    }

    public ExternalFeesList GetAllExternalFees()
    {
      if (this.feesList == null)
        this.feesList = ExternalFees.ToList(this.mngr.GetFeeManagement(this.externalOrg.oid));
      return this.feesList;
    }

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
      ExternalFeeManagement externalFeeManagement = new ExternalFeeManagement(this.externalOrg.oid, fee.FeeName, fee.Code, (ExternalOriginatorEntityType) (int) fee.Channel, fee.StartDate, fee.EndDate, 0, "", "", fee.FeePercent, fee.FeeAmount, fee.FeeBasedOn, (ExternalOriginatorStatus) 2)
      {
        Description = fee.Description
      };
      externalFeeManagement.DateUpdated = externalFeeManagement.DateCreated = DateTime.Now;
      externalFeeManagement.UpdatedBy = externalFeeManagement.CreatedBy = this.Session.UserID;
      this.mngr.InsertFeeManagementSettings(externalFeeManagement, this.externalOrg.oid);
      this.feesList = (ExternalFeesList) null;
      return this.GetAllExternalFees();
    }

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
      this.mngr.UpdateFeeManagementSettings(new ExternalFeeManagement(this.externalOrg.oid, fee.FeeName, fee.Code, (ExternalOriginatorEntityType) (int) fee.Channel, fee.StartDate, fee.EndDate, 0, "", "", fee.FeePercent, fee.FeeAmount, fee.FeeBasedOn, (ExternalOriginatorStatus) 2)
      {
        FeeManagementID = fee.FeeManagementID,
        Description = fee.Description,
        DateUpdated = DateTime.Now,
        UpdatedBy = this.Session.UserID
      });
      this.feesList = (ExternalFeesList) null;
    }

    public void DeleteExternalFees(ExternalFees fee)
    {
      this.mngr.DeleteFeeManagementSettings(new List<int>()
      {
        fee.FeeManagementID
      });
      this.feesList = (ExternalFeesList) null;
    }

    public LateFeeSettings GetExternalLateFeeSettings()
    {
      return new LateFeeSettings(this.mngr.GetExternalOrgLateFeeSettings(this.externalOrg.oid, false));
    }

    public ExternalDBAList GetAllDBANames()
    {
      if (this.DBAList == null)
        this.DBAList = ExternalDBAName.ToList(this.mngr.GetDBANames(this.externalOrg.oid));
      return this.DBAList;
    }

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

    public void SetDefaultDBAName(ExternalDBAName dba)
    {
      this.mngr.SetDBANameAsDefault(this.externalOrg.oid, dba.DBAID);
      this.DBAList = (ExternalDBAList) null;
    }

    public void AddDBAName(string name, bool setDefault)
    {
      int num = this.mngr.InsertDBANames(new ExternalOrgDBAName(this.externalOrg.oid, name, setDefault), this.externalOrg.oid);
      if (setDefault)
        this.mngr.SetDBANameAsDefault(this.externalOrg.oid, num);
      this.DBAList = (ExternalDBAList) null;
    }

    public void EditDBAName(ExternalDBAName dba)
    {
      this.mngr.UpdateDBANames(dba.Original);
      this.DBAList = (ExternalDBAList) null;
    }

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

    public void ChangeSortIndexDBANames(Dictionary<int, ExternalDBAName> dbas)
    {
      if (this.DBAList == null)
        this.DBAList = ExternalDBAName.ToList(this.mngr.GetDBANames(this.externalOrg.oid));
      foreach (ExternalDBAName dba in (CollectionBase) this.DBAList)
      {
        if (!dbas.Values.Contains<ExternalDBAName>(dba))
          throw new Exception("Make sure all the DBA Names of this external organization are included in the Dictionary object with their sort order as key");
      }
      Dictionary<int, int> dictionary = new Dictionary<int, int>();
      foreach (KeyValuePair<int, ExternalDBAName> dba in dbas)
        dictionary.Add(dba.Key, dba.Value.DBAID);
      this.mngr.SetDBANamesSortIndex(dictionary, this.externalOrg.oid);
      this.DBAList = (ExternalDBAList) null;
    }

    public Dictionary<int, ExternalDocumentList> GetAllExternalOrgDocuments()
    {
      return ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, -1, false));
    }

    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel)
    {
      return channel == ExternalOrganizationEntityType.Both ? ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, -1, false)) : ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, (int) channel, -1, false));
    }

    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrgOriginatorStatus status)
    {
      return ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, (int) status, false));
    }

    public Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel,
      ExternalOrgOriginatorStatus status,
      bool disableGlobalDocs)
    {
      return channel == ExternalOrganizationEntityType.Both ? ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, -1, (int) status, disableGlobalDocs)) : ExternalDocumentsSettings.ToDictionary(this.mngr.GetExternalOrgDocuments(this.externalOrg.oid, (int) channel, (int) status, disableGlobalDocs));
    }

    public ExternalDocumentList GetAllArchivedDocuments()
    {
      return ExternalDocumentsSettings.ToList(this.mngr.GetAllArchiveDocuments(this.externalOrg.oid));
    }

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
      BinaryObject binaryObject = new BinaryObject(fileObject.Data);
      this.mngr.CreateDocumentInDataFolder(new FileSystemEntry("\\\\" + document.FileName, (FileSystemEntry.Types) 2, (string) null), binaryObject);
      document.AddedBy = this.Session.GetUserInfo().FullName;
      document.DateAdded = DateTime.Now;
      document.IsArchive = false;
      document.FileSize = Utils.FormatByteSize((long) fileObject.Data.Length);
      this.mngr.AddDocument(this.externalOrg.oid, ExternalDocumentsSettings.ToDocumentSettingInfoObj(document), isTopOfCategory);
    }

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

    public void DeleteExternalDocument(ExternalDocumentsSettings document)
    {
      string str = Path.GetExtension(document.FileName).ToLower().Trim();
      FileSystemEntry fileSystemEntry = new FileSystemEntry("\\\\" + (document.Guid.ToString() + str), (FileSystemEntry.Types) 2, (string) null);
      this.mngr.DeleteDocument(this.externalOrg.oid, document.Guid, fileSystemEntry);
    }

    public void ArchiveExternalDocument(string guid)
    {
      this.mngr.ArchiveDocuments(this.externalOrg.oid, new List<string>()
      {
        guid
      });
    }

    public void UnArchiveExternalDocuments(List<string> guids)
    {
      this.mngr.UnArchiveDocuments(this.externalOrg.oid, guids);
    }

    public void ChangeActiveCheckedExternalDocument(
      ExternalDocumentsSettings document,
      bool activeChecked)
    {
      this.mngr.UpdateActiveStatus(this.externalOrg.oid, activeChecked, document.IsDefault, document.Guid);
    }

    public void ChangeActiveCheckedExternalDocument(int category, bool activeChecked)
    {
      this.mngr.UpdateActiveStatusAllDocsInCategory(this.externalOrg.oid, category, activeChecked);
    }

    public ExternalDocumentList GetGlobalExternalDocumentsToAssign()
    {
      return ExternalDocumentsSettings.ToList(this.mngr.GetExternalDocumentsForOrgAssignment(this.externalOrg.oid));
    }

    public void AssignGlobalDocumentToOrg(ExternalDocumentsSettings document, bool isTopOfCategory)
    {
      if (!document.IsDefault)
        throw new ExternalDocumentSettingException(ExternalDocumentSettingViolationType.InvalidInputArgument, "This document is not from Global Settings.");
      document.ExternalOrgId = this.externalOrg.oid;
      this.mngr.AssignDocumentToOrg(ExternalDocumentsSettings.ToDocumentSettingInfoObj(document), isTopOfCategory);
    }

    public void SwapSortOrderOfDocuments(
      ExternalDocumentsSettings firstDocument,
      ExternalDocumentsSettings secondDocument)
    {
      this.mngr.SwapDocumentSortIds(this.externalOrg.oid, ExternalDocumentsSettings.ToDocumentSettingInfoObj(firstDocument), ExternalDocumentsSettings.ToDocumentSettingInfoObj(secondDocument));
    }

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
      if (UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null))
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

    public void BatchLoadSettings(List<ExternalOrganizationSetting> settings)
    {
      List<ExternalOriginatorOrgSetting> originatorOrgSettingList = new List<ExternalOriginatorOrgSetting>();
      if (settings.Contains(ExternalOrganizationSetting.AssignableSalesReps) && this.assignableSalesReps == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 2);
      if (settings.Contains(ExternalOrganizationSetting.AttachmentCategory) && this.attachmentCategoryList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 6);
      if (settings.Contains(ExternalOrganizationSetting.CompanyRating) && this.companyRatingList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 5);
      if (settings.Contains(ExternalOrganizationSetting.CompanyStatus) && this.companyStatusList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 3);
      if (settings.Contains(ExternalOrganizationSetting.ContactStatus) && this.contactStatusList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 4);
      if (settings.Contains(ExternalOrganizationSetting.License) && this.licensing == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 0);
      if (settings.Contains(ExternalOrganizationSetting.LoanTypes) && this.loanTypes == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 1);
      if (settings.Contains(ExternalOrganizationSetting.PriceGroup) && this.priceGroupList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 7);
      if (settings.Contains(ExternalOrganizationSetting.UrlList) && this.urlList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 8);
      if (settings.Contains(ExternalOrganizationSetting.PrimaryManager) && UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null))
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 9);
      if (settings.Contains(ExternalOrganizationSetting.Note) && this.notesList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 10);
      if (settings.Contains(ExternalOrganizationSetting.Attachment) && this.attachmentsList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 11);
      if (settings.Contains(ExternalOrganizationSetting.LOComp) && this.externalLOCompPlanList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 12);
      if (this.loanCompHistoryList == null)
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 13);
      if (settings.Contains(ExternalOrganizationSetting.ExternalSalesRepListForOrg) && (this.externalSalesRepListForOrg == null || this.primarySalesRepUser == null))
        originatorOrgSettingList.Add((ExternalOriginatorOrgSetting) 14);
      Dictionary<ExternalOriginatorOrgSetting, object> dictionary = new Dictionary<ExternalOriginatorOrgSetting, object>();
      if (this.hasPerformancePatch)
      {
        dictionary = this.mngr.GetExternalAdditionalDetails(this.ID, originatorOrgSettingList);
      }
      else
      {
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 2) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 6) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 5) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 3) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 4) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 0) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 1) || originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 7))
        {
          List<object> additionalDetails = this.mngr.GetExternalAdditionalDetails(this.ID);
          dictionary.Add((ExternalOriginatorOrgSetting) 2, additionalDetails[2]);
          dictionary.Add((ExternalOriginatorOrgSetting) 6, additionalDetails[6]);
          dictionary.Add((ExternalOriginatorOrgSetting) 5, additionalDetails[5]);
          dictionary.Add((ExternalOriginatorOrgSetting) 3, additionalDetails[3]);
          dictionary.Add((ExternalOriginatorOrgSetting) 4, additionalDetails[4]);
          dictionary.Add((ExternalOriginatorOrgSetting) 0, additionalDetails[0]);
          dictionary.Add((ExternalOriginatorOrgSetting) 1, additionalDetails[1]);
          dictionary.Add((ExternalOriginatorOrgSetting) 7, additionalDetails[7]);
        }
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 8) && this.urlList == null)
          dictionary.Add((ExternalOriginatorOrgSetting) 8, (object) this.mngr.GetSelectedOrgUrls(this.externalOrg.oid));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 9) && UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null))
          dictionary.Add((ExternalOriginatorOrgSetting) 9, (object) this.mngr.GetExternalUserInfo(this.externalOrg.Manager));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 10) && this.notesList == null)
          dictionary.Add((ExternalOriginatorOrgSetting) 10, (object) this.mngr.GetExternalOrganizationNotes(this.externalOrg.oid));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 11) && this.attachmentsList == null)
          dictionary.Add((ExternalOriginatorOrgSetting) 11, (object) this.mngr.GetExternalAttachmentsByOid(this.externalOrg.oid));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 12) && this.externalLOCompPlanList == null)
          dictionary.Add((ExternalOriginatorOrgSetting) 12, (object) this.mngr.GetAllCompPlans(true, 2));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 13) && this.loanCompHistoryList == null)
          dictionary.Add((ExternalOriginatorOrgSetting) 13, (object) this.mngr.GetCompPlansByoid(false, this.externalOrg.oid));
        if (originatorOrgSettingList.Contains((ExternalOriginatorOrgSetting) 14) && this.primarySalesRepUser == null)
        {
          List<object> objectList = new List<object>();
          ExternalOriginatorManagementData externalOrganization = this.mngr.GetExternalOrganization(false, this.externalOrg.oid);
          objectList.Add(externalOrganization != null ? (object) externalOrganization.PrimarySalesRepUserId : (object) string.Empty);
          objectList.Add((object) this.mngr.GetExternalOrgSalesRepsForCurrentOrg(this.externalOrg.oid));
          objectList.Add(externalOrganization == null || !(externalOrganization.PrimarySalesRepAssignedDate != DateTime.MinValue) ? (object) string.Empty : (object) externalOrganization.PrimarySalesRepAssignedDate.ToString());
          dictionary.Add((ExternalOriginatorOrgSetting) 14, (object) objectList);
        }
      }
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 2) && this.assignableSalesReps == null)
        this.assignableSalesReps = (IEnumerable<UserInfo>) dictionary[(ExternalOriginatorOrgSetting) 2];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 6) && this.attachmentCategoryList == null)
        this.attachmentCategoryList = (List<ExternalSettingValue>) dictionary[(ExternalOriginatorOrgSetting) 6];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 5) && this.companyRatingList == null)
        this.companyRatingList = (List<ExternalSettingValue>) dictionary[(ExternalOriginatorOrgSetting) 5];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 3) && this.companyStatusList == null)
        this.companyStatusList = (List<ExternalSettingValue>) dictionary[(ExternalOriginatorOrgSetting) 3];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 4) && this.contactStatusList == null)
        this.contactStatusList = (List<ExternalSettingValue>) dictionary[(ExternalOriginatorOrgSetting) 4];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 0) && this.licensing == null)
        this.licensing = new ExternalLicensing((BranchExtLicensing) dictionary[(ExternalOriginatorOrgSetting) 0]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 1) && this.loanTypes == null)
        this.loanTypes = new ExternalLoanTypes((ExternalOrgLoanTypes) dictionary[(ExternalOriginatorOrgSetting) 1]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 7) && this.priceGroupList == null)
        this.priceGroupList = (List<ExternalSettingValue>) dictionary[(ExternalOriginatorOrgSetting) 7];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 8) && this.urlList == null)
        this.urlList = ExternalUrl.ToList(((List<ExternalOrgURL>) dictionary[(ExternalOriginatorOrgSetting) 8]).ToArray());
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 9) && UserInfo.op_Equality((UserInfo) this.primaryManager, (UserInfo) null))
        this.primaryManager = (ExternalUserInfo) dictionary[(ExternalOriginatorOrgSetting) 9];
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 10) && this.notesList == null)
        this.notesList = ExternalNote.ToList((ExternalOrgNotes) dictionary[(ExternalOriginatorOrgSetting) 10]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 11) && this.attachmentsList == null)
        this.attachmentsList = ExternalAttachment.ToList((List<ExternalOrgAttachments>) dictionary[(ExternalOriginatorOrgSetting) 11]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 12) && this.externalLOCompPlanList == null)
        this.externalLOCompPlanList = ExternalLoanCompPlan.ToList((List<LoanCompPlan>) dictionary[(ExternalOriginatorOrgSetting) 12]);
      if (dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 13) && this.loanCompHistoryList == null)
      {
        this.loanCompHistoryList = (LoanCompHistoryList) dictionary[(ExternalOriginatorOrgSetting) 13];
        this.externalLOCompHistoryList = ExternalLoanCompHistory.ToList(this.loanCompHistoryList);
      }
      if (!dictionary.Keys.Contains<ExternalOriginatorOrgSetting>((ExternalOriginatorOrgSetting) 14))
        return;
      List<object> objectList1 = (List<object>) dictionary[(ExternalOriginatorOrgSetting) 14];
      this.primarySalesRepUserId = string.Concat(objectList1[0]);
      if (objectList1.Count > 2)
        this.primarySalesRepAssignedDate = objectList1[2].ToString();
      this.externalSalesRepListForOrg = ExternalSalesRep.ToList((List<ExternalOrgSalesRep>) objectList1[1], this.ID);
      this.primarySalesRepUser = this.externalSalesRepListForOrg.Find((Predicate<ExternalSalesRep>) (e1 => e1.userId.Equals(this.primarySalesRepUserId)));
    }

    private Dictionary<CorrespondentMasterDeliveryType, Decimal> GetOutstandingCommentments()
    {
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommentments = new Dictionary<CorrespondentMasterDeliveryType, Decimal>();
      ICorrespondentTradeManager icorrespondentTradeManager = (ICorrespondentTradeManager) this.Session.GetObject("CorrespondentTradeManager");
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      Dictionary<CorrespondentMasterDeliveryType, Decimal> standingCommitments;
      Dictionary<CorrespondentMasterDeliveryType, Decimal> outstandingCommitments;
      if (this.externalOrg.OrganizationType == null)
      {
        standingCommitments = icorrespondentTradeManager.GetOutStandingCommitments(this.externalOrg.oid);
        outstandingCommitments = iconfigurationManager.GetNonAllocatedOutstandingCommitments(this.externalOrg.ExternalID);
      }
      else
      {
        standingCommitments = icorrespondentTradeManager.GetOutStandingCommitments(this.companyOrg.oid);
        outstandingCommitments = iconfigurationManager.GetNonAllocatedOutstandingCommitments(this.companyOrg.ExternalID);
      }
      foreach (int num1 in (CorrespondentMasterDeliveryType[]) Enum.GetValues(typeof (CorrespondentMasterDeliveryType)))
      {
        CorrespondentMasterDeliveryType key = (CorrespondentMasterDeliveryType) num1;
        Decimal num2 = 0M;
        if (standingCommitments.ContainsKey(key))
          num2 += standingCommitments[key];
        if (outstandingCommitments.ContainsKey(key))
          num2 += outstandingCommitments[key];
        if (outstandingCommentments.ContainsKey(key))
          outstandingCommentments[key] = num2;
        else
          outstandingCommentments.Add(key, num2);
      }
      return outstandingCommentments;
    }

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
          ExternalOriginatorOrgType organizationType = x.OrganizationType;
          if (organizationType != 1 && organizationType != 3)
            return;
          result.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, x.oid, false, this.hasPerformancePatch));
        }
        else
        {
          if (!(x.ExternalID == this.externalOrg.ExternalID))
            return;
          ExternalOriginatorOrgType organizationType = x.OrganizationType;
          if (organizationType != 1 && organizationType != 3)
            return;
          result.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, x.oid, false, this.hasPerformancePatch));
        }
      }));
      return result;
    }

    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetAllBranches()
    {
      IEnumerable<ExternalOriginatorManagementData> originatorManagementDatas = this.companyOrgs.Where<ExternalOriginatorManagementData>((Func<ExternalOriginatorManagementData, bool>) (x => x.OrganizationType == 2));
      List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> allBranches = new List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization>();
      if (originatorManagementDatas != null)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in originatorManagementDatas)
          allBranches.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch));
      }
      return allBranches;
    }

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
      this.committed((object) this, new PersistentObjectEventArgs(this.OrganizationID));
    }

    private string ValidateExternalOrg()
    {
      if (this.EntityType == ExternalOrganizationEntityType.None)
        return "Channel/Entity type can not be blank.";
      if ((this.EntityType == ExternalOrganizationEntityType.Both || this.EntityType == ExternalOrganizationEntityType.Correspondent) && this.UnderwritingType == ExternalOrgUnderwritingType.None)
        return "Underwriting Type is required if Channel/Entity type is set to Correspondent.";
      foreach (StateLicenseExtType stateLicenseExtType in this.Licensing.StateLicenseExtTypes)
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
        ((BranchLicensing) extLicenseDetails).UseParentInfo = true;
        return extLicenseDetails;
      }
      BranchLicensing.ATRExemptCreditors atrExemptCreditor = (BranchLicensing.ATRExemptCreditors) licensing.ATRExemptCreditor;
      BranchLicensing.ATRSmallCreditors atrSmallCreditor = (BranchLicensing.ATRSmallCreditors) licensing.ATRSmallCreditor;
      return new BranchExtLicensing(licensing.UseParentInfo, licensing.AllowLoansWithIssues, licensing.MsgUploadNonApprovedLoans, licensing.LenderType, licensing.HomeState, licensing.OptOut, licensing.StatutoryElectionInMaryland, licensing.StatutoryElectionInMaryland2, licensing.StatutoryElectionInKansas, this.GetStateLicenseExtType(licensing.StateLicenseExtTypes), licensing.UseCustomLenderProfile, atrSmallCreditor, atrExemptCreditor);
    }

    private List<StateLicenseExtType> GetStateLicenseExtType(List<StateLicenseExtType> extType)
    {
      List<StateLicenseExtType> stateLicenseExtType1 = new List<StateLicenseExtType>();
      foreach (StateLicenseExtType stateLicenseExtType2 in extType)
      {
        List<StateLicenseExtType> stateLicenseExtTypeList = stateLicenseExtType1;
        StateLicenseExtType stateLicenseExtType3 = new StateLicenseExtType(stateLicenseExtType2.StateAbbrevation, stateLicenseExtType2.LicenseType, stateLicenseExtType2.LicenseNo, stateLicenseExtType2.IssueDate, stateLicenseExtType2.StartDate, stateLicenseExtType2.EndDate, stateLicenseExtType2.LicenseStatus, stateLicenseExtType2.StatusDate, stateLicenseExtType2.Approved, stateLicenseExtType2.Exempt, stateLicenseExtType2.LastChecked, stateLicenseExtType2.SortIndex);
        ((StateLicenseType) stateLicenseExtType3).Selected = stateLicenseExtType2.Selected;
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

    public List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetALLExternalOrganizationByType(
      ExternalOriginationOrgType organizationType)
    {
      List<int> intList = new List<int>();
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
      intList.Add(Convert.ToInt32((object) organizationType));
      List<ExternalOriginatorManagementData> organizationByType2 = this.mngr.GetChildExternalOrganizationByType(this.externalOrg.oid, intList);
      if (organizationByType2 != null)
      {
        foreach (ExternalOriginatorManagementData originatorManagementData in organizationByType2)
          organizationByType1.Add(new EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization(this.Session, this.companyOrgs, originatorManagementData.oid, false, this.hasPerformancePatch));
      }
      return organizationByType1;
    }

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

    public List<RateSheet> GetRateSheets()
    {
      List<ExternalSettingValue> orgSettingsByName = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettingsByName("ICE PPE Rate Sheet");
      List<RateSheet> result = new List<RateSheet>();
      orgSettingsByName.ForEach((Action<ExternalSettingValue>) (x => result.Add(new RateSheet(this.Session, x.settingValue, x.settingId))));
      return result;
    }

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

    public List<DocumentCategory> GetDocumentCategories()
    {
      List<ExternalSettingValue> orgSettingsByName = ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalOrgSettingsByName("Document Category");
      List<DocumentCategory> result = new List<DocumentCategory>();
      orgSettingsByName.ForEach((Action<ExternalSettingValue>) (x => result.Add(new DocumentCategory(this.Session, x.settingValue, x.settingId))));
      return result;
    }

    public ExternalBanksList GetAllExternalBanks()
    {
      if (this.bankList == null)
        this.bankList = ExternalBanks.ToList(((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).GetExternalBanks());
      return this.bankList;
    }

    public ExternalWarehouseList GetAllExternalOrgWarehouses()
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      if (this.warehouseList == null)
        this.warehouseList = ExternalWarehouse.ToList(iconfigurationManager.GetExternalOrgWarehouses(this.externalOrg.oid));
      return this.warehouseList;
    }

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

    public void UpdateExternalOrgWarehouse(ExternalWarehouse obj)
    {
      IConfigurationManager iconfigurationManager = (IConfigurationManager) this.Session.GetObject("ConfigurationManager");
      ExternalOrgWarehouse externalOrgWarehouse = new ExternalOrgWarehouse()
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
      externalOrgWarehouse.TimeZone = obj.TimeZone;
      externalOrgWarehouse.StatusDate = obj.StatusDate;
      iconfigurationManager.UpdateExternalOrgWarehouse(obj.WarehouseID, externalOrgWarehouse);
      this.warehouseList = (ExternalWarehouseList) null;
    }

    public void DeleteExternalOrgWarehouse(ExternalWarehouse obj)
    {
      ((IConfigurationManager) this.Session.GetObject("ConfigurationManager")).DeleteExternalOrgWarehouse(obj.WarehouseID, this.externalOrg.oid);
      this.warehouseList = (ExternalWarehouseList) null;
    }
  }
}
