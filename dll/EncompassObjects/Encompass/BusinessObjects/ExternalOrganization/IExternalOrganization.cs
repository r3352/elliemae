// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.ExternalOrganization.IExternalOrganization
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.ExternalOrganization
{
  [Guid("A77CB3F1-6328-4E39-B185-1C8555F71052")]
  public interface IExternalOrganization
  {
    int ID { get; }

    int ParentOrganizationID { get; }

    bool DisabledLogin { get; set; }

    ExternalOriginationOrgType OrganizationType { get; set; }

    ExternalOrganizationEntityType EntityType { get; set; }

    string ExternalID { get; set; }

    string OrganizationID { get; set; }

    string OwnerName { get; set; }

    string CompanyLegalName { get; set; }

    string CompanyDBAName { get; set; }

    string Address { get; set; }

    string City { get; set; }

    string State { get; set; }

    string Zip { get; set; }

    string PhoneNumber { get; set; }

    string FaxNumber { get; set; }

    string Email { get; set; }

    string Website { get; set; }

    string OrganizationName { get; set; }

    DateTime LastLoanSubmitted { get; set; }

    bool CanAcceptFirstPayments { get; set; }

    string EmailForRateSheet { get; set; }

    string FaxForRateSheet { get; set; }

    string EmailForLockInfo { get; set; }

    string FaxForLockInfo { get; set; }

    bool InheritProductAndPricing { get; set; }

    string EPPSUserName { get; set; }

    string EPPSCompModel { get; set; }

    RateSheet EPPSRateSheet { get; set; }

    PriceGroup PriceGroup { get; set; }

    PriceGroup PriceGroupBroker { get; set; }

    PriceGroup PriceGroupDelegated { get; set; }

    PriceGroup PriceGroupNonDelegated { get; set; }

    string PMLUserName { get; set; }

    string PMLPassword { get; set; }

    string PMLCustomerCode { get; set; }

    CurrentCompanyStatus CurrentStatus { get; set; }

    bool AddToWatchlist { get; set; }

    DateTime CurrentStatusDate { get; set; }

    DateTime ApprovedDate { get; set; }

    DateTime ApplicationDate { get; set; }

    CompanyRating CompanyRating { get; set; }

    ExternalLicensing Licensing { get; set; }

    ExternalLoanTypes LoanTypes { get; set; }

    bool Incorporated { get; set; }

    string StateIncorp { get; set; }

    DateTime DateOfIncorporation { get; set; }

    string YrsInBusiness { get; }

    int TypeOfEntity { get; set; }

    string OtherEntityDescription { get; set; }

    string TaxID { get; set; }

    bool UseSSNFormat { get; set; }

    string NmlsId { get; set; }

    string FinancialsPeriod { get; set; }

    DateTime FinancialsLastUpdate { get; set; }

    Decimal CompanyNetWorth { get; set; }

    DateTime EOExpirationDate { get; set; }

    string EOCompany { get; set; }

    string EOPolicyNumber { get; set; }

    string MERSOriginatingORGID { get; set; }

    bool DUSponsored { get; set; }

    bool CanFundInOwnName { get; set; }

    bool CanCloseInOwnName { get; set; }

    bool IsTestAccount { get; set; }

    bool CommitmentUseBestEffort { get; set; }

    bool CommitmentUseBestEffortLimited { get; set; }

    Decimal MaxCommitmentAuthority { get; set; }

    Decimal CommitmentBestEffortsAvailableAmount { get; }

    bool CommitmentMandatory { get; set; }

    Decimal MaxCommitmentAmount { get; set; }

    Decimal CommitmentMandatoryAvailableAmount { get; }

    bool IsCommitmentDeliveryIndividual { get; set; }

    bool IsCommitmentDeliveryBulk { get; set; }

    bool IsCommitmentDeliveryAOT { get; set; }

    bool IsCommitmentDeliveryLiveTrade { get; set; }

    bool IsCommitmentDeliveryForward { get; set; }

    ExternalOriginationCommitmentPolicy CommitmentPolicy { get; set; }

    string CommitmentMessage { get; set; }

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Company { get; }

    EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization Branch { get; }

    ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls);

    ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls,
      User primarySalesRep);

    ExternalUser CreateUser(
      string firstName,
      string lastName,
      string loginEmail,
      string password,
      ExternalUrlList urls,
      ExternalUser updatedByExtUser);

    ExternalUserList GetUsers();

    ExternalUrlList GetUrlList();

    ExternalUrl AddExternalUrl(ExternalSiteUrl Url, ExternalOrganizationEntityType entityType);

    ExternalUrl AddExternalUrl(ExternalUrl Url);

    void UpdateExternalUrls(ExternalUrlList url);

    void DeleteExternalUrl(ExternalUrl url);

    User PrimarySalesRep { get; }

    UserList AvailableAEs { get; }

    ExternalLoanCompPlanList GetAllLOCompPlans();

    int GetNumberOfLOCompPlans();

    ExternalLoanCompHistoryList GetLOCompHistory();

    int GetNumberOfLOCompHistory();

    ExternalLoanCompHistory AssignLOCompPlan(
      ExternalLoanCompPlan loanCompPlan,
      DateTime startDate,
      DateTime endDate);

    ExternalLoanCompHistory GetCurrentPlan(DateTime triggerDateTime);

    ExternalLoanCompHistoryList GetCurrentAndFuturePlans(DateTime todayDate);

    ExternalLoanCompHistoryList GetFuturePlans(DateTime todayDate);

    bool UpdateStartDateForCompHistory(
      DateTime startDate,
      ExternalLoanCompHistory selectedLoanCompHistory);

    ExternalLoanCompHistory GetLoCompHistoryByPlanId(int compPlanId);

    bool DeleteLoCompHistory(ExternalLoanCompHistoryList compPlans);

    bool DeleteLoCompHistory(ExternalLoanCompHistory compPlanId);

    bool SetSalesRepAsPrimary(User user);

    ExternalSalesRep GetPrimarySalesRep();

    bool DeleteExternalOrganizationSalesReps(ExternalSalesRep[] userids);

    List<ExternalSalesRep> GetExternalOrgSalesRepsForCurrentOrg();

    List<User> GetExternalOrgSalesRepUsersForCurrentOrg();

    List<ExternalSalesRep> GetExternalOrgSalesRepsForCompany();

    UserList GetAllInternalUsers();

    void AddInternalUserAsSalesRep(User[] user);

    ExternalUser PrimaryManager { get; set; }

    string PrimaryManagerExternalUserID { get; }

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetAllBranches();

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetAllBranches(
      string siteID);

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetALLExternalOrganizationByType(
      ExternalOriginationOrgType organizationType);

    void UpdateStateLicense();

    ExternalNotesList GetAllNotes();

    ExternalNote CreateExternalNote(string note);

    void DeleteExternalNote(ExternalNotesList notes);

    int GetNumberOfNotes();

    List<ExternalAttachment> GetAllAttachments();

    ExternalAttachment CreateExternalAttachment(
      string fileName,
      string description,
      DateTime dateAdded,
      string category,
      DateTime fileDate,
      DateTime expirationDate,
      int daysToExpire,
      string fileLocation,
      DataObject file);

    ExternalAttachment CreateExternalAttachment(
      string fileName,
      string description,
      DateTime dateAdded,
      AttachmentCategory category,
      DateTime fileDate,
      DateTime expirationDate,
      int daysToExpire,
      string fileLocation,
      DataObject file);

    void UpdateExternalAttachment(ExternalAttachment attachment, DataObject file);

    void DeleteExternalAttachment(ExternalAttachment attachment);

    bool GetExternalAttachmentIsExpired();

    int GetNumberOfAttachments();

    DataObject ReadAttachment(ExternalAttachment attachment);

    List<AttachmentCategory> GetAttachmentCategories();

    List<CurrentCompanyStatus> GetCurrentCompanyStatus();

    List<CompanyRating> GetCompanyRatings();

    List<PriceGroup> GetPriceGroups();

    List<DocumentCategory> GetDocumentCategories();

    ExternalFeesList GetAllExternalFees();

    ExternalFeesList GetExternalFeesByChannel(ExternalOrganizationEntityType channel);

    ExternalFeesList GetExternalFeesByStatus(ExternalOriginatorFeeStatus status);

    ExternalFeesList AddExternalFees(ExternalFees fee);

    void EditExternalFees(ExternalFees fee);

    void DeleteExternalFees(ExternalFees fee);

    LateFeeSettings GetExternalLateFeeSettings();

    ExternalDBAList GetAllDBANames();

    ExternalDBAName GetDefaultDBAName();

    void SetDefaultDBAName(ExternalDBAName dba);

    void AddDBAName(string name, bool setDefault);

    void EditDBAName(ExternalDBAName dba);

    void DeleteDBAName(ExternalDBAName dba);

    void ChangeSortIndexDBANames(Dictionary<int, ExternalDBAName> dbas);

    Dictionary<int, ExternalDocumentList> GetAllExternalOrgDocuments();

    Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel);

    Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(ExternalOrgOriginatorStatus status);

    Dictionary<int, ExternalDocumentList> GetExternalOrgDocuments(
      ExternalOrganizationEntityType channel,
      ExternalOrgOriginatorStatus status,
      bool disableGlobalDocs);

    ExternalDocumentList GetAllArchivedDocuments();

    void AddExternalDocument(
      ExternalDocumentsSettings document,
      DataObject fileObject,
      bool isTopOfCategory);

    void UpdateExternalDocument(ExternalDocumentsSettings document);

    void DeleteExternalDocument(ExternalDocumentsSettings document);

    void ArchiveExternalDocument(string guid);

    void UnArchiveExternalDocuments(List<string> guids);

    void ChangeActiveCheckedExternalDocument(ExternalDocumentsSettings document, bool activeChecked);

    void ChangeActiveCheckedExternalDocument(int Category, bool activeChecked);

    ExternalDocumentList GetGlobalExternalDocumentsToAssign();

    void AssignGlobalDocumentToOrg(ExternalDocumentsSettings document, bool isTopOfCategory);

    void SwapSortOrderOfDocuments(
      ExternalDocumentsSettings firstDocument,
      ExternalDocumentsSettings secondDocument);

    DataObject ReadDocumentFromDataFolder(ExternalDocumentsSettings document);

    void RemoveAssignedDocFromTPO(string guid);

    ExternalBanksList GetAllExternalBanks();

    ExternalWarehouseList GetAllExternalOrgWarehouses();

    ExternalWarehouseList AddExternalOrgWarehouse(int BankID);

    ExternalWarehouseList AddExternalOrgWarehouse(int BankID, string timeZone);

    void UpdateExternalOrgWarehouse(ExternalWarehouse obj);

    void DeleteExternalOrgWarehouse(ExternalWarehouse obj);

    List<EllieMae.Encompass.BusinessObjects.ExternalOrganization.ExternalOrganization> GetExtensionOrganizations(
      bool immediateChildOnly);
  }
}
