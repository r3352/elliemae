// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ExternalOriginatorManagementData
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class ExternalOriginatorManagementData : IPropertyDictionary
  {
    private string commitmentMsg;
    private string dailyLimitMsg;
    private string dbaName = "";

    public virtual int oid { get; set; }

    public virtual ExternalOriginatorContactType contactType { get; set; }

    public virtual int Parent { get; set; }

    public virtual string ExternalID { get; set; }

    public virtual string OldExternalID { get; set; }

    public virtual string OrganizationName { get; set; }

    public virtual string CompanyDBAName
    {
      get => this.dbaName;
      set => this.dbaName = value;
    }

    public virtual string CompanyLegalName { get; set; }

    public virtual ExternalOriginatorEntityType entityType { get; set; }

    public virtual ExternalOriginatorUnderwritingType UnderwritingType { get; set; }

    public virtual ManageFeeLEDisclosures GenerateDisclosures { get; set; }

    public virtual string Address { get; set; }

    public virtual string City { get; set; }

    public virtual string State { get; set; }

    public virtual string Zip { get; set; }

    public virtual bool UseParentInfo { get; set; }

    public virtual int Depth { get; set; }

    public virtual string HierarchyPath { get; set; }

    public virtual bool DisabledLogin { get; set; }

    public virtual bool NoAfterHourWires { get; set; }

    public virtual bool UseParentInfoForCompanyDetails { get; set; }

    public virtual string Timezone { get; set; }

    public virtual ExternalOriginatorOrgType OrganizationType { get; set; }

    public virtual bool AutoAssignOrgID { get; set; }

    public virtual string OrgID { get; set; }

    public virtual string OwnerName { get; set; }

    public virtual string PhoneNumber { get; set; }

    public virtual string FaxNumber { get; set; }

    public virtual string Email { get; set; }

    public virtual string Website { get; set; }

    public virtual string Manager { get; set; }

    public virtual DateTime LastLoanSubmitted { get; set; }

    public virtual bool? VisibleOnTPOWCSite { get; set; }

    public virtual bool MultiFactorAuthentication { get; set; }

    public virtual bool UseParentInfoForRateLock { get; set; }

    public virtual string EmailForRateSheet { get; set; }

    public virtual string FaxForRateSheet { get; set; }

    public virtual string EmailForLockInfo { get; set; }

    public virtual string FaxForLockInfo { get; set; }

    public virtual bool UseParentInfoForEPPS { get; set; }

    public virtual string EPPSUserName { get; set; }

    public virtual string EPPSCompModel { get; set; }

    public virtual string EPPSPriceGroup { get; set; }

    public virtual string EPPSPriceGroupBroker { get; set; }

    public virtual string EPPSPriceGroupDelegated { get; set; }

    public virtual string EPPSPriceGroupNonDelegated { get; set; }

    public virtual string EPPSRateSheet { get; set; }

    public virtual string PMLUserName { get; set; }

    public virtual string PMLPassword { get; set; }

    public virtual string PMLCustomerCode { get; set; }

    public virtual bool UseParentInfoForApprovalStatus { get; set; }

    public virtual int CurrentStatus { get; set; }

    public virtual bool AddToWatchlist { get; set; }

    public virtual DateTime CurrentStatusDate { get; set; }

    public virtual DateTime ApprovedDate { get; set; }

    public virtual DateTime ApplicationDate { get; set; }

    public virtual int CompanyRating { get; set; }

    public virtual string PrimarySalesRepUserId { get; set; }

    public virtual DateTime PrimarySalesRepAssignedDate { get; set; }

    public virtual bool UseParentInfoForBusinessInfo { get; set; }

    public virtual bool Incorporated { get; set; }

    public virtual string StateIncorp { get; set; }

    public virtual DateTime DateOfIncorporation { get; set; }

    public virtual bool ResetLimitForRatesheetId { get; set; }

    public virtual string YrsInBusiness
    {
      get
      {
        string yrsInBusiness = string.Empty;
        if ((ValueType) this.DateOfIncorporation != null && !string.IsNullOrWhiteSpace(this.DateOfIncorporation.ToString()) && this.DateOfIncorporation != DateTime.MinValue)
        {
          if (this.DateOfIncorporation > DateTime.Now)
          {
            yrsInBusiness = "0 Year 0 Months";
          }
          else
          {
            DateTime date1 = DateTime.Now.Date;
            DateTime date2 = this.DateOfIncorporation.Date;
            int num1 = DateTime.DaysInMonth(date2.Year, date2.Month);
            int num2 = date1.Day + (num1 - date2.Day);
            int num3;
            int num4;
            if (date1.Month > date2.Month)
            {
              num3 = date1.Year - date2.Year;
              num4 = date1.Month - (date2.Month + 1) + Math.Abs(num2 / num1);
            }
            else if (date1.Month == date2.Month)
            {
              if (date1.Day >= date2.Day)
              {
                num3 = date1.Year - date2.Year;
                num4 = 0;
              }
              else
              {
                num3 = date1.Year - 1 - date2.Year;
                num4 = 11;
              }
            }
            else
            {
              num3 = date1.Year - 1 - date2.Year;
              num4 = date1.Month + (11 - date2.Month) + Math.Abs(num2 / num1);
            }
            string str = num3 > 1 ? " Years " : " Year ";
            yrsInBusiness = num3.ToString() + str + (object) num4 + " Months";
          }
        }
        return yrsInBusiness;
      }
    }

    public virtual int TypeOfEntity { get; set; }

    public virtual string OtherEntityDescription { get; set; }

    public virtual string TaxID { get; set; }

    public virtual bool UseSSNFormat { get; set; }

    public virtual string NmlsId { get; set; }

    public virtual string FinancialsPeriod { get; set; }

    public virtual DateTime FinancialsLastUpdate { get; set; }

    public virtual Decimal? CompanyNetWorth { get; set; }

    public virtual DateTime EOExpirationDate { get; set; }

    public virtual string EOCompany { get; set; }

    public virtual string EOPolicyNumber { get; set; }

    public virtual string MERSOriginatingORGID { get; set; }

    public virtual int DUSponsored { get; set; }

    public virtual int CanFundInOwnName { get; set; }

    public virtual int CanCloseInOwnName { get; set; }

    public virtual bool InheritWebCenterSetup { get; set; }

    public virtual bool InheritCustomFields { get; set; }

    public virtual bool InheritDBANames { get; set; }

    public virtual bool InheritWarehouses { get; set; }

    public virtual bool InheritParentLicense { get; set; }

    public virtual bool IsTestAccount { get; set; }

    public virtual int LPASponsored { get; set; }

    public virtual string LPASponsorTPONumber { get; set; }

    public virtual string LPASponsorLPAPassword { get; set; }

    public virtual string LEINumber { get; set; }

    public virtual bool CommitmentUseBestEffort { get; set; }

    public virtual bool CommitmentUseBestEffortLimited { get; set; }

    public virtual Decimal MaxCommitmentAuthority { get; set; }

    public virtual Decimal BestEffortDailyVolumeLimit { get; set; }

    public virtual ExternalOriginatorCommitmentTolerancePolicy BestEffortTolerencePolicy { get; set; }

    public virtual Decimal BestEffortTolerancePct { get; set; }

    public virtual Decimal BestEffortToleranceAmt { get; set; }

    public virtual ExternalOriginatorBestEffortDailyLimitPolicy BestEfforDailyLimitPolicy { get; set; }

    public virtual string DailyLimitWarningMsg
    {
      get
      {
        return string.IsNullOrWhiteSpace(this.dailyLimitMsg) ? "The loan amount exceeds the available amount of the daily volume limit." : this.dailyLimitMsg;
      }
      set => this.dailyLimitMsg = value;
    }

    public virtual string MaxCommitmentAuthorityDisplayValue
    {
      get => this.MaxCommitmentAuthority.ToString("###,###");
    }

    public virtual bool CommitmentMandatory { get; set; }

    public virtual Decimal MaxCommitmentAmount { get; set; }

    public virtual string MaxCommitmentAmountDisplayValue
    {
      get => this.MaxCommitmentAmount.ToString("###,###");
    }

    public virtual ExternalOriginatorCommitmentTolerancePolicy MandatoryTolerencePolicy { get; set; }

    public virtual Decimal MandatoryTolerancePct { get; set; }

    public virtual Decimal MandatoryToleranceAmt { get; set; }

    public virtual bool IsCommitmentDeliveryIndividual { get; set; }

    public virtual bool IsCommitmentDeliveryBulk { get; set; }

    public virtual bool IsCommitmentDeliveryAOT { get; set; }

    public virtual bool IsCommitmentDeliveryLiveTrade { get; set; }

    public virtual bool IsCommitmentDeliveryCoIssue { get; set; }

    public virtual bool IsCommitmentDeliveryForward { get; set; }

    public virtual bool IsCommitmentDeliveryBulkAOT { get; set; }

    public virtual ExternalOriginatorCommitmentPolicy CommitmentPolicy { get; set; }

    public virtual ExternalOriginatorCommitmentTradePolicy CommitmentTradePolicy { get; set; }

    public virtual string CommitmentMessage
    {
      get
      {
        return string.IsNullOrEmpty(this.commitmentMsg) ? "The loan amount exceeds the available amount of the commitment authority for this delivery type." : this.commitmentMsg;
      }
      set => this.commitmentMsg = value;
    }

    public virtual bool TradeMgmtEnableTPOTradeManagementForTPOClient { get; set; }

    public virtual bool TradeMgmtUseCompanyTPOTradeManagementSettings { get; set; }

    public virtual bool TradeMgmtViewCorrespondentTrade { get; set; }

    public virtual bool TradeMgmtViewCorrespondentMasterCommitment { get; set; }

    public virtual bool TradeMgmtLoanEligibilityToCorrespondentTrade { get; set; }

    public virtual bool TradeMgmtEPPSLoanProgramEligibilityPricing { get; set; }

    public virtual bool TradeMgmtLoanAssignmentToCorrespondentTrade { get; set; }

    public virtual bool TradeMgmtLoanDeletionFromCorrespondentTrade { get; set; }

    public virtual bool TradeMgmtRequestPairOff { get; set; }

    public virtual bool TradeMgmtReceiveCommitmentConfirmation { get; set; }

    public virtual bool CanAcceptFirstPayments { get; set; }

    public virtual string BillingAddress { get; set; }

    public virtual string BillingCity { get; set; }

    public virtual string BillingState { get; set; }

    public virtual string BillingZip { get; set; }

    public virtual List<string> RootOrgBySalesRep { get; set; }

    public virtual bool GlobalOrSpecificTPO { get; set; }

    public object this[string columnName]
    {
      get
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "address":
            return (object) this.Address;
          case "addtowatchlist":
            return !this.AddToWatchlist ? (object) "N" : (object) "Y";
          case "applicationdate":
            return (object) this.ApplicationDate;
          case "approveddate":
            return (object) this.ApprovedDate;
          case "cancloseinownname":
            return (object) this.CanCloseInOwnName;
          case "canfundinownname":
            return (object) this.CanFundInOwnName;
          case "city":
            return (object) this.City;
          case "companydbaname":
            return (object) this.CompanyDBAName;
          case "companylegalname":
            return (object) this.CompanyLegalName;
          case "companynetworth":
            return (object) this.CompanyNetWorth;
          case "companyrating":
            return (object) this.CompanyRating;
          case "currentstatus":
            return (object) this.CurrentStatus;
          case "currentstatusdate":
            return (object) this.CurrentStatusDate;
          case "dateofincorporation":
            return (object) this.DateOfIncorporation;
          case "disabledlogin":
            return (object) this.DisabledLogin;
          case "dusponsored":
            return (object) this.DUSponsored;
          case "email":
            return (object) this.Email;
          case "emailforlockinfo":
            return (object) this.EmailForLockInfo;
          case "emailforratesheet":
            return (object) this.EmailForRateSheet;
          case "entitytype":
            return (object) this.entityType;
          case "eocompany":
            return (object) this.EOCompany;
          case "eoexpirationdate":
            return (object) this.EOExpirationDate;
          case "eopolicynumber":
            return (object) this.EOPolicyNumber;
          case "eppscompModel":
            return (object) this.EPPSCompModel;
          case "eppspricegroup":
            return (object) this.EPPSPriceGroup;
          case "eppspricegroupbroker":
            return (object) this.EPPSPriceGroupBroker;
          case "eppspricegroupdel":
            return (object) this.EPPSPriceGroupDelegated;
          case "eppspricegroupnondel":
            return (object) this.EPPSPriceGroupNonDelegated;
          case "eppsratesheet":
            return (object) this.EPPSRateSheet;
          case "eppsusername":
            return (object) this.EPPSUserName;
          case "faxforlockinfo":
            return (object) this.FaxForLockInfo;
          case "faxforratesheet":
            return (object) this.FaxForRateSheet;
          case "faxnumber":
            return (object) this.FaxNumber;
          case "financialslastupdate":
            return (object) this.FinancialsLastUpdate;
          case "financialsperiod":
            return (object) this.FinancialsPeriod;
          case "hierarchypath":
            return (object) this.HierarchyPath;
          case "incorporated":
            return (object) this.Incorporated;
          case "managername":
            return (object) this.Manager;
          case "mersoriginatingorgid":
            return (object) this.MERSOriginatingORGID;
          case "nmlsid":
            return (object) this.NmlsId;
          case "organizationname":
            return (object) this.OrganizationName;
          case "organizationtype":
            return (object) this.OrganizationType.ToString();
          case "orgid":
            return (object) this.OrgID;
          case "otherentitydescription":
            return (object) this.OtherEntityDescription;
          case "ownername":
            return (object) this.OwnerName;
          case "phonenumber":
            return (object) this.PhoneNumber;
          case "primarysalesrepuserid":
            return (object) this.PrimarySalesRepUserId;
          case "state":
            return (object) this.State;
          case "stateincorp":
            return (object) this.StateIncorp;
          case "taxid":
            return (object) this.TaxID;
          case "typeofentity":
            return (object) this.TypeOfEntity;
          case "useparentinfoforapprovalstatus":
            return (object) this.UseParentInfoForApprovalStatus;
          case "useparentinfoforbusinessinfo":
            return (object) this.UseParentInfoForBusinessInfo;
          case "useparentinfoforcompanydetails":
            return (object) this.UseParentInfoForCompanyDetails;
          case "useparentinfoforepps":
            return (object) this.UseParentInfoForEPPS;
          case "useparentinfoforratelock":
            return (object) this.UseParentInfoForRateLock;
          case "usessnformat":
            return (object) this.UseSSNFormat;
          case "website":
            return (object) this.Website;
          case "zip":
            return (object) this.Zip;
          default:
            return (object) null;
        }
      }
      set
      {
        columnName = columnName.ToLower();
        switch (columnName)
        {
          case "address":
            this.Address = string.Concat(value);
            break;
          case "addtowatchlist":
            this.AddToWatchlist = (bool) value;
            break;
          case "applicationdate":
            if (!(value.ToString() != string.Empty))
              break;
            this.ApplicationDate = Utils.ParseDate(value);
            break;
          case "approveddate":
            if (!(value.ToString() != string.Empty))
              break;
            this.ApprovedDate = Utils.ParseDate(value);
            break;
          case "cancloseinownname":
            this.CanCloseInOwnName = Utils.ParseInt(value);
            break;
          case "canfundinownname":
            this.CanFundInOwnName = Utils.ParseInt(value);
            break;
          case "city":
            this.City = string.Concat(value);
            break;
          case "companydbaname":
            this.CompanyDBAName = string.Concat(value);
            break;
          case "companylegalname":
            this.CompanyLegalName = string.Concat(value);
            break;
          case "companynetworth":
            if (!(value.ToString() != string.Empty))
              break;
            this.CompanyNetWorth = new Decimal?(Utils.ParseDecimal(value));
            break;
          case "companyrating":
            if (!(value.ToString() != string.Empty))
              break;
            this.CompanyRating = Utils.ParseInt(value);
            break;
          case "currentstatus":
            this.CurrentStatus = Utils.ParseInt(value);
            break;
          case "currentstatusdate":
            if (!(value.ToString() != string.Empty))
              break;
            this.CurrentStatusDate = Utils.ParseDate(value);
            break;
          case "dateofincorporation":
            if (!(value.ToString() != string.Empty))
              break;
            this.DateOfIncorporation = Utils.ParseDate(value);
            break;
          case "disabledlogin":
            this.DisabledLogin = (bool) value;
            break;
          case "dusponsored":
            this.DUSponsored = Utils.ParseInt(value);
            break;
          case "email":
            this.Email = string.Concat(value);
            break;
          case "emailforlockinfo":
            this.EmailForLockInfo = string.Concat(value);
            break;
          case "emailforratesheet":
            this.EmailForRateSheet = string.Concat(value);
            break;
          case "entitytype":
            this.entityType = value.ToString() == "Broker" ? ExternalOriginatorEntityType.Broker : (value.ToString() == "Correspondent" ? ExternalOriginatorEntityType.Correspondent : (value.ToString() == "Both" ? ExternalOriginatorEntityType.Both : ExternalOriginatorEntityType.None));
            break;
          case "eocompany":
            this.EOCompany = string.Concat(value);
            break;
          case "eoexpirationdate":
            if (!(value.ToString() != string.Empty))
              break;
            this.EOExpirationDate = Utils.ParseDate(value);
            break;
          case "eopolicynumber":
            this.EOPolicyNumber = string.Concat(value);
            break;
          case "eppscompmodel":
            this.EPPSCompModel = string.Concat(value);
            break;
          case "eppspricegroup":
            this.EPPSPriceGroup = string.Concat(value);
            break;
          case "eppspricegroupbroker":
            this.EPPSPriceGroupBroker = string.Concat(value);
            break;
          case "eppspricegroupdel":
            this.EPPSPriceGroupDelegated = string.Concat(value);
            break;
          case "eppspricegroupnondel":
            this.EPPSPriceGroupNonDelegated = string.Concat(value);
            break;
          case "eppsratesheet":
            this.EPPSRateSheet = string.Concat(value);
            break;
          case "eppsusername":
            this.EPPSUserName = string.Concat(value);
            break;
          case "faxforlockinfo":
            this.FaxForLockInfo = string.Concat(value);
            break;
          case "faxforratesheet":
            this.FaxForRateSheet = string.Concat(value);
            break;
          case "faxnumber":
            this.FaxNumber = string.Concat(value);
            break;
          case "financialslastupdate":
            if (!(value.ToString() != string.Empty))
              break;
            this.FinancialsLastUpdate = Utils.ParseDate(value);
            break;
          case "financialsperiod":
            this.FinancialsPeriod = string.Concat(value);
            break;
          case "hierarchypath":
            this.HierarchyPath = string.Concat(value);
            break;
          case "incorporated":
            this.Incorporated = (bool) value;
            break;
          case "managername":
            this.Manager = string.Concat(value);
            break;
          case "mersoriginatingorgid":
            this.MERSOriginatingORGID = string.Concat(value);
            break;
          case "nmlsid":
            this.NmlsId = string.Concat(value);
            break;
          case "organizationname":
            this.OrganizationName = string.Concat(value);
            break;
          case "organizationtype":
            this.OrganizationType = value.ToString() == "Branch" ? ExternalOriginatorOrgType.Branch : ExternalOriginatorOrgType.Company;
            break;
          case "orgid":
            this.OrgID = string.Concat(value);
            break;
          case "otherentitydescription":
            this.OtherEntityDescription = string.Concat(value);
            break;
          case "ownername":
            this.OwnerName = string.Concat(value);
            break;
          case "phonenumber":
            this.PhoneNumber = string.Concat(value);
            break;
          case "primarysalesrepname":
            this.PrimarySalesRepUserId = string.Concat(value);
            break;
          case "primarysalesrepuserid":
            this.PrimarySalesRepUserId = string.Concat(value);
            break;
          case "state":
            this.State = string.Concat(value);
            break;
          case "stateincorp":
            this.StateIncorp = string.Concat(value);
            break;
          case "taxid":
            this.TaxID = string.Concat(value);
            break;
          case "typeofentity":
            this.TypeOfEntity = Utils.ParseInt(value);
            break;
          case "useparentinfoforapprovalstatus":
            this.UseParentInfoForApprovalStatus = (bool) value;
            break;
          case "useparentinfoforbusinessinfo":
            this.UseParentInfoForBusinessInfo = (bool) value;
            break;
          case "useparentinfoforcompanydetails":
            this.UseParentInfoForCompanyDetails = (bool) value;
            break;
          case "useparentinfoforepps":
            this.UseParentInfoForEPPS = (bool) value;
            break;
          case "useparentinfoforratelock":
            this.UseParentInfoForRateLock = (bool) value;
            break;
          case "usessnformat":
            this.UseSSNFormat = (bool) value;
            break;
          case "website":
            this.Website = string.Concat(value);
            break;
          case "zip":
            this.Zip = string.Concat(value);
            break;
          default:
            throw new ArgumentException("Invalid field name \"" + columnName + "\"");
        }
      }
    }
  }
}
