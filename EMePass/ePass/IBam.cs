// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ePass.IBam
// Assembly: EMePass, Version=3.5.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A610697F-A1EC-4CC3-A30A-403E37B2B276
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMePass.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.eFolder;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.ePass.BamObjects;
using EllieMae.EMLite.ePass.Browser;
using EllieMae.EMLite.RemotingServices;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

#nullable disable
namespace EllieMae.EMLite.ePass
{
  [Guid("8410CA24-1952-391D-8FC1-710897FC560D")]
  public interface IBam
  {
    string GetLoginID();

    string GetUserID();

    string GetUserFirstName();

    string GetUserLastName();

    string GetUserPassword();

    string GetUserPersona();

    string GetUserPhone();

    string GetUserEmail();

    bool GetUserMyEpassCustomRight();

    void SetUserSetting(string section, string key, string str);

    string GetUserSetting(string section, string key);

    void SetCompanySetting(string section, string key, string str);

    string GetCompanySetting(string section, string key);

    string[] GetLoanFolders();

    string[] GetLoanNames(string folder);

    void AddePASSLog(
      string title,
      string company,
      string orderDateStr,
      string receiveDateStr,
      string ePASSSignature,
      string comments);

    string GetLoanName();

    int GetNumberOfDeposits();

    int GetNumberOfEmployer(bool borrower);

    int GetNumberOfLiabilitesExlcudingAlimonyJobExp();

    int GetNumberOfMortgages();

    int GetNumberOfResidence(bool borrower);

    int GetNumberOfBorrowerPairs();

    string GetPairID();

    string GetSimpleField(string id);

    string GetSimpleField(string id, int pairIndex);

    void LoadLoan(string loanFolder, string loanName, bool readOnly);

    int NewLiability();

    void SaveLoan();

    void SetBorrowerPair(int pairIndex);

    void SetCurrentField(string id, string val);

    void RemoveLiabilityAt(int block);

    string ExportData(string format);

    bool ValidateData(string format, bool allowContinue);

    string LoadFile(string filePath);

    void SaveFile(string filePath, string fileContent);

    string LoadDo();

    void SaveDo(string xmlStr);

    string LoadXdt(string vendor);

    void SaveXdt(string vendor, string xmlStr);

    string LoadLiability();

    void SaveLiability(string xmlStr);

    string LoadFannieLiability(string vendor);

    void SaveFannieLiability(string vendor, string xmlStr);

    string LoadMbaLiability(string vendor);

    void SaveMbaLiability(string vendor, string xmlStr);

    string LoadReport(string vendor, Bam.FileType fileType);

    void SaveReport(string vendor, Bam.FileType fileType, string fileContent);

    string GetReportFileTypes(string vendor);

    void Navigate(string url);

    void ProcessURL(string url);

    bool SendMessage(string msgFile);

    string[] GetGlobalFiles();

    string LoadGlobalFile(string filePath);

    void SaveGlobalFile(string filePath, string fileContent);

    string[] GetUserFiles();

    string LoadUserFile(string filePath);

    void SaveUserFile(string filePath, string fileContent);

    void CloseLoan();

    bool LoadLoan(string loanFolder, string loanName);

    bool LockLoan();

    void AddLifeInsuranceLog(
      string title,
      string status,
      string followUpDate,
      bool alertLO,
      bool alertLP,
      string comments);

    void CreateLead(object properties);

    bool LeadExists(string firstName, string lastName, string leadSource, string txnId);

    string GetTempFolder();

    void LogVerbose(string source, string message);

    void LogInfo(string source, string message);

    void LogWarning(string source, string message);

    void LogError(string source, string message);

    byte[] LoadBinary(string filename);

    void SaveBinary(string filename, byte[] content);

    bool GetUserPlanCodeRight();

    bool GetUserAltLenderRight();

    void GoToField(string fieldID);

    void GoToField(string fieldID, bool findNext);

    bool SendPDF(string pdfFile);

    string GetCompanyClientID();

    string GetCompanyName();

    string GetCompanyAddressLine1();

    string GetCompanyAddressCity();

    string GetCompanyAddressState();

    string GetCompanyAddressZip();

    string GetCompanyFax();

    void UpdateVerifLog(
      string verifID,
      bool ordered,
      bool received,
      string ePASSSignature,
      string comments);

    void PrintFaxCover(string orderNo, string faxNumber, string trackingNo, string misc2);

    void ShowWebPage(string title, string url, int width, int height);

    void ShowPdfFile(string fileName);

    void SetField(string id, string val);

    string GetVersion();

    void ExportLoan(string zipFile);

    void ImportLoan(string zipFile);

    bool LoanExists(string guid);

    bool LoadLoan(string guid);

    void LoadLoan(string guid, bool readOnly);

    void CreateLoan(string loanName);

    string GetLoanFolder();

    string ShowRolodex(string category, bool multiSelect);

    void AddCondition(
      string pairID,
      string docName,
      string condSource,
      string condInfo,
      bool eFolder);

    void AddComplianceLog(
      string company,
      string riskIndicator,
      string riskExplanation,
      bool freeReport);

    string[] SelectDocuments(bool multiSelect);

    string GetDocumentTitle(string docID);

    string GetDocumentCompany(string docID);

    string GetDocumentPairID(string docID);

    string ExportDocument(string docID);

    string GetEdition();

    void AddEDMLog(string description, string action, string[] docList, string comments);

    string[] GetOffers(string companyID);

    string GetOfferCategory(string offerID);

    string GetOfferCompanyID(string offerID);

    string GetOfferCompanyName(string offerID);

    string GetOfferCompanyProgram(string offerID);

    string GetOfferDescription(string offerID);

    void DeliverDisclosures(string zipFile, bool premiumDisclosures);

    void ShowHelp(string helpPageName);

    bool IsDocumentCollector();

    string AddAttachment(string filepath);

    bool GetUserMyEpassCustomRight(string service);

    void SetTab(string tabName);

    bool SendPDF(string pdfFile, string[] docList);

    int[] GetUserPersonaList();

    int[] GetPersonaList();

    string GetPersonaName(int personaID);

    string[] GetEncompassCloserForms();

    string[] SelectEncompassForms(bool selectBorrowerPair);

    string GetEncompassFormTitle(string formID);

    string GetEncompassFormType(string formID);

    string ExportEncompassForm(string formID);

    void AddPreliminaryConditionLog(
      string source,
      string title,
      string description,
      string details,
      string pairID,
      string category,
      string priorTo);

    string AddAttachment(string filepath, string title);

    void SaveBinary(string filename, string title, byte[] content);

    void SaveFile(string filename, string title, string fileContent);

    bool CreateLoanFromDUFile(string filepath);

    string[] GetEncompassDisclosureForms();

    int GetBorrowerPair();

    string AddDisclosureDocument(string docTitle);

    string SelectAnEmailAddress();

    string GetCompanyPhone();

    string GetUserFax();

    void AddLockRequest();

    object[][] QueryLoans(string[] fieldsToRetrieve, string queryXml);

    string[] QueryLoanGuids(string queryXml);

    string[] QueryLoanNames(string queryXml);

    string[] GetDocumentAttachmentList(string docID);

    string GetAttachment(string attachmentID);

    int GetAttachmentRotation(string attachmentID);

    string GetAttachmentTitle(string attachmentID);

    string ExportAttachment(string attachmentID);

    bool ShowDisclosureEntities();

    bool ShowDisclosurePackages();

    string[] GetCompanyDisclosureEntities();

    string[] GetCompanyDisclosurePackages();

    string[] GetEncompassDisclosureForms(string[] entityList, string[] packageList);

    bool GetUserDeselectDisclosureRight();

    bool PreviewDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList);

    bool PrintDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList);

    bool SendDisclosures(
      string coversheetFile,
      string[] entityList,
      string[] packageList,
      string[] titleList,
      string[] pdfList);

    string ExportFaxCoversheet();

    string GetLoanDataXML();

    string GetCustomFieldSettings();

    string GetEncompassSignatureType(string formID);

    string GetLoanPaymentSchedule();

    string ProductPricingPartnerSetting(string partnerID, string property);

    string GetDefaultProductPricingPartnerID();

    void ProductPricingCreateLockRequest();

    void ProductPricingCreateLockRequestWithAction(string productAndPricingData);

    string AddDocumentLog(
      string title,
      string company,
      string dateRequested,
      string dateExpected,
      string dateReceived,
      string comments,
      string epassSignature);

    string[] GetDocumentList();

    bool CanAddAttachment(string docID);

    string AddAttachment(string filepath, string title, string docID);

    string GetDocumentListXml();

    bool PreviewClosing(string[] titleList, string[] pdfList);

    bool PrintClosing(string[] titleList, string[] pdfList);

    bool SendClosing(string[] titleList, string[] pdfList);

    string GetUserPaymentToken(string settingType);

    string GetUserPaymentType(string settingType);

    void ViewDocument(string docID);

    void RetrieveDocument(string docID);

    string[] GetPersonaUsers(int personaID);

    int[] GetOrganizationList();

    string GetOrganizationName(int orgID);

    string GetOrganizationCode(int orgID);

    string[] GetUserList(bool accessibleOnly);

    string GetUserFirstName(string userID);

    string GetUserLastName(string userID);

    string GetUserEmail(string userID);

    int[] GetUserPersonaList(string userID);

    int GetUserOrganization(string userID);

    int[] GetUserGroupList();

    string GetUserGroupName(int groupID);

    string[] GetUserGroupUsers(int groupID);

    void ShowLockRequestProcessDialog();

    bool AreInformationalOnlyDisclosures();

    bool DoNotUseEMeDisclosures();

    void LogTransaction(string vendorUrl, string transID, string miscData);

    Form GetEncompassApplicationWindow();

    string AddPartnerDocumentLog(
      string title,
      string company,
      string dateRequested,
      string dateExpected,
      string comments,
      string epassSignature);

    string GetDocumentID(string title, string company);

    bool DoesDocumentLogExist(string docID);

    bool MarkDocumentLogAsReceived(string docID, string dateReceivedStr, string comments);

    string[] GetCompanyDocumentList(string company);

    DateTime GetDocumentDateRequested(string docID);

    string GetAttachmentID(string docID, string title);

    bool DoesAttachmentExist(string attachmentID);

    string GetDisplayVersion();

    string GetLicenseEdition();

    void SaveCustomBinary(string key, byte[] data);

    void AppendCustomBinary(string key, byte[] data);

    int GetPairIndex();

    void SetBorrowerPair(string pairId);

    object GetNativeField(string fieldId);

    object GetNativeField(string fieldId, int pairIndex);

    void SetNativeField(string fieldId, object value);

    void SelectPlanCode();

    void SelectAltLender();

    void AuditDisclosures();

    void OrderDisclosures();

    void AuditClosingDocs();

    void OrderClosingDocs();

    void ViewClosingDocs();

    bool IsExportServiceAccessible(string exportName);

    bool ExportServiceProcessLoans(string exportName, string[] loanGuids);

    bool ExportServiceValidateLoan(string exportName, string loanGuid);

    void ExportServiceProcessLoan(string exportName, string loanGuid);

    bool ExportServiceExportData(string exportName, string[] loanGuids);

    bool GetUserRequestEllieMaeNetworkServicesRight();

    bool GetUserOrderEncompassClosingDocsRight();

    bool IsEncompassClosingDocsSolutionAvailable();

    bool IsEncompassOpeningDocsSolutionAvailable();

    byte[] GetCompanyCustomDataObject(string name);

    void SetCompanyCustomDataObject(string name, byte[] data);

    void AppendToCompanyCustomDataObject(string name, byte[] data);

    void DeleteCompanyCustomDataObject(string name);

    void Recalculate();

    void ExecCalculation(string calcName);

    int GetNumberOf4506();

    int GetNumberOf4506T();

    string SelectEVaultDocument();

    string[] SelectDocuments2(bool multiSelect, bool allowContinue);

    string ImportAUSTrackingHistory(
      string xmlString,
      string submissionDate,
      bool copyDefaultLoanDataToLog,
      bool forDU);

    string GetAUSTrackingHistory(string historyGUID);

    string[] GetAUSTrackingHistoryGUIDs(bool includeManualEntry);

    bool ProductPricingIsHistoricalPricingEnabled();

    void ExecuteCalculation(string calcName);

    bool IsHosted();

    string GetSsoToken(IEnumerable<string> services, int expirationInMinutes);

    bool CanSendConsent();

    int GetNumberOfSettlementServiceProviders();

    bool GetUserAppraisalMgtRight();

    string QueryLoansForXml(string fieldsToRetrieveXml, string queryXml);

    string FieldsNotInRDB(string fieldsXml);

    bool RDBUpdatePendingStatus();

    string GetFullVersion();

    bool RemoveSettlementServiceProviderAt(int i);

    int NewSettlementServiceProvider();

    List<KeyValuePair<string, string>> GetPreliminaryConditionLogs();

    void SaveLoan(bool triggerCalcAll);

    void SaveLoan(bool triggerCalcAll, bool throwException);

    DisclosureTrackingRecord2015[] GetDisclosureTracking2015Records();

    List<KeyValuePair<string, string>> GetPostClosingConditionLogs();

    void AddPostClosingConditionLog(
      string source,
      string title,
      string description,
      string details,
      string pairID,
      string recipient);

    string GetTpoNumber(string userId);

    FeeManagementSetting GetFeeManagementSetting();

    string GetCurrentFormOrTool();

    void SaveEPPSClientLoanPrograms(string xmlResponse);

    List<KeyValuePair<string, string>> GetEpassCredentials(string userId, string partnerSection);

    void refreshLoanFromServer();

    string GetAccessToken(string scope);

    List<KeyValuePair<string, string>> GetEpassCredentialsForAll(
      string userId,
      string partnerSection);

    string EncompassServerUrl { get; }

    void SaveTitleFeeCredentials(string orderID, string loanGUID, string credentials);

    string[] GetTitleFeeCredentials(string[] orderIDs, string loanGUID);

    void RefreshContents();

    Dictionary<DocumentLogInfo, FileAttachment[]> GetDocumentAttachmentsForCLO();

    string ExportDocumentFile(string docID, FileAttachment fileAttachment);

    BizPartnerInfo ShowRolodex(string category);

    RoleInfo GetRealWorldRoleMapping(RealWorldRoleID realWorldRoleID);

    UserInfo GetRealWorldRoleContact(RealWorldRoleID realWorldRoleID);

    void AddFieldLock(string fieldID);

    void RemoveFieldLock(string fieldID);

    bool IsFieldLocked(string fieldID);

    int AddGSERepWarrantTracker();

    int GetNumberOfGSERepWarrentTrackers();

    bool RemoveGSERepWarrentTrackers();

    int RemoveGSERepWarrentBlock(int blockID);

    void RetrieveDocument2(string docID, bool showDocumentDetailsDialog);

    int GetNumberOfAdditionalLoans();

    int GetNumberOfGiftsAndGrants();

    int GetNumberOfOtherIncomeSources();

    int GetNumberOfOtherLiabilities();

    int GetNumberOfOtherAssets();

    int GetNumberOfAlternateNames(bool borrower);

    void Recalculate(bool skipLockRequestSync);

    EnhancedDisclosureTrackingRecord2015[] GetDisclosureTracking2015EnhancedRecords();

    string GetOAPIGatewayBaseUri();

    string GetEpc2HostAdapterUrl();

    IEncompassBrowser CreateBrowser();

    IEncompassBrowser CreateBrowser(IBrowserParams parameters);

    string GetServiceUrl(string urlKey);

    Dictionary<string, string> GetServiceUrls();
  }
}
