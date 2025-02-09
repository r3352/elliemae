// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IConfigurationManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.BidTapeManagement;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.ExternalOriginatorManagement;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.LockComparison;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.ServerTasks;
using EllieMae.EMLite.ClientServer.SkyDrive;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Common.Contact;
using EllieMae.EMLite.Common.Licensing;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.DocEngine;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Server;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IConfigurationManager
  {
    string GetSsoTokenExpirationTimeForEdm();

    CompanyInfo GetCompanyInfo();

    void UpdateCompanyInfo(CompanyInfo info, AclFeature feature);

    string GetCompanySetting(string section, string key);

    Hashtable GetCompanySettings(string section);

    void SetCompanySetting(string section, string key, string str);

    HMDAInformation GetHMDAInformation();

    void UpdateHMDAInformation(HMDAInformation hmdaInformation);

    List<HMDAProfile> GetHMDAProfile();

    HMDAProfile GetHMDAProfileById(int profileId);

    void UpdateHMDAProfile(HMDAProfile hmdaProfile);

    bool DoesProfileNameExist(string profileName);

    void DeleteProfileName(string profileName);

    bool IsAssociateToOrg(int profileID);

    string GetOrgNameByHMDAProfile(int profileID);

    List<IRS4506TTemplate> GetIRS4506TTemplates();

    (List<IRS4506TTemplate> Templates, int TotalCount) GetPagedIRS4506TTemplates(
      string[] formVersions = null,
      int start = 0,
      int limit = 0);

    List<IRS4506TTemplate> GetIRS4506TTemplates(bool listOnly);

    IRS4506TTemplate GetIRS4506TTemplate(int templateID);

    IRS4506TTemplate GetIRS4506TTemplate(string templateName);

    int CreateIRS4506TTemplate(IRS4506TTemplate template);

    void UpdateIRS4506TTemplate(IRS4506TTemplate template);

    void DeleteIRS4506TTemplate(string templateName);

    LicenseInfo GetServerLicense();

    void UpdateServerLicense(LicenseInfo license);

    UserLicenseInfo GetUserLicense(string userId);

    void UpdateUserLicense(UserLicenseInfo license);

    void MigrateLoanCustomFields();

    CustomFieldsInfo GetLoanCustomFields(bool forceToPrimaryDb = false);

    CustomFieldInfo GetLoanCustomField(string fieldID);

    void UpdateLoanCustomField(CustomFieldInfo fieldInfo);

    void UpdateLoanCustomFields(CustomFieldsInfo fieldsInfo, bool cacheCustomFieldsInfo);

    void DeleteLoanCustomField(string fieldID);

    void DeleteLoanCustomFields(string[] fieldIDs);

    DefaultFieldsInfo GetDefaultFields(string root);

    void UpdateDefaultFields(DefaultFieldsInfo fieldInfo, string root);

    EVerifyLoginInfoItem GetEVerifyLoginInfo(string userid, string vendorName);

    void SetEVerifyLoginInfo(string userid, EVerifyLoginInfoItem loginInfoItem);

    LoanNumberingInfo GetLoanNumberingInfo();

    void UpdateLoanNumberingInfo(LoanNumberingInfo info);

    BranchLoanNumberingInfo GetBranchLoanNumberingInfo(string orgCode);

    BranchLoanNumberingInfo[] GetAllBranchLoanNumberingInfo(bool onlyInUse);

    void SaveBranchLoanNumberingInfo(BranchLoanNumberingInfo info);

    MersNumberingInfo GetMersNumberingInfo();

    void UpdateMersNumberingInfo(MersNumberingInfo info);

    BranchMERSMINNumberingInfo GetBranchMERSNumberingInfo(string mersminCode);

    BranchMERSMINNumberingInfo[] GetAllBranchMERSNumberingInfo(bool onlyInUse);

    void SaveBranchMERSNumberingInfo(BranchMERSMINNumberingInfo info);

    List<LoanDuplicateChecker> GetLoanDuplicateInfo(
      string guid,
      List<Dictionary<LoanDuplicateChecker.CheckField, string>> borrowerInfo,
      Address address,
      string loanFolder);

    void SaveDuplicate(string loanGuid, string duplicateGuid);

    string GetDuplicates(string loanGuid);

    DuplicateScreenSetting GetDuplicateScreenSetting(string loanFolder, string loanName);

    void SaveDuplicateScreenSetting(
      DuplicateScreenSetting setting,
      string loanFolder,
      string loanName);

    string[] GetSystemSettingsNames();

    BinaryObject GetSystemSettings(string name);

    Hashtable GetSystemSettings(string[] names);

    Dictionary<string, BinaryObject> GetMultipleSystemSettings(List<string> names);

    void SaveSystemSettings(string name, BinaryObject data);

    BinaryObject GetCustomDataObject(string name);

    void SaveCustomDataObject(string name, BinaryObject data);

    void AppendToCustomDataObject(string name, BinaryObject data);

    string[] GetCustomDataObjectNames();

    DocumentTrackingSetup GetDocumentTrackingSetup();

    void SaveDocumentTrackingSetup(DocumentTrackingSetup setup);

    void UpsertDocumentTrackingTemplate(DocumentTrackingSetup setup, string guid);

    ConditionTrackingSetup GetConditionTrackingSetup(ConditionType conditionType);

    void MigrateUnderwritingAndPostClosingConditionsFromXmlToDb();

    void AddConditionTrackingSetup(ConditionTemplate conditionTemplate);

    void UpdateConditionTrackingSetup(ConditionTemplate conditionTemplate);

    void DeleteConditionTrackingSetup(List<ConditionTemplate> conditionTemplates);

    DocumentGroupSetup GetDocumentGroupSetup();

    void SaveDocumentGroupSetup(DocumentGroupSetup setup);

    EDisclosureSetup GetEDisclosureSetup();

    void SaveEDisclosureSetup(EDisclosureSetup setup);

    EDisclosureSignOrderSetup GetEDisclosureSignOrderSetup();

    void SaveEDisclosureSignOrderSetup(EDisclosureSignOrderSetup setup);

    DataTable GetAllEdisclosureElementAttributes();

    string GetEDisclosureElementAttributeId(int loanChannelType, int edisclosurePackageType);

    string[] GetMilestoneTemplatesByChannelId(int channelId);

    string[] GetMilestoneNamesByChannelId(int channelId);

    string GetFormConfigFile(FormConfigFile fileType);

    FileSystemEntry[] GetAllTemplateSettingsFileEntries(TemplateSettingsType type, string userId);

    FileSystemEntry[] GetAllTemplateSettingsFileEntries(
      TemplateSettingsType type,
      string userId,
      bool includeProperties);

    FileSystemEntry[] GetAllPublicTemplateSettingsFileEntries(
      TemplateSettingsType type,
      bool includeProperties);

    FileSystemEntry[] GetAllPublicTemplateSettingsSystemEntries(
      TemplateSettingsType type,
      bool includeProperties);

    FileSystemEntry[] GetTemplateDirEntries(TemplateSettingsType type, FileSystemEntry parentFolder);

    FileSystemEntry[] GetFilteredTemplateDirEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder);

    FileSystemEntry[] GetFilteredTemplateFileEntries(
      TemplateSettingsType type,
      bool includeProperties);

    FileSystemEntry[] GetFilteredTemplateFileEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      bool recurse,
      bool includeProperties);

    FileSystemEntry GetTemplatePropertiesAndRights(
      TemplateSettingsType type,
      FileSystemEntry fsEntries,
      bool includeProperties,
      bool includeRights);

    FileSystemEntry[] GetTemplatePropertiesAndRights(
      TemplateSettingsType type,
      FileSystemEntry[] fsEntries,
      bool includeProperties,
      bool includeRights);

    FileSystemEntry[] FindTemplateFileEntries(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string propertyName,
      object propertyValue,
      bool recurse);

    FileSystemEntry FindTemplateByGuid(TemplateSettingsType type, string guid);

    FileSystemEntry FindTemplateByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string guid,
      bool recurse);

    BinaryObject GetTemplateSettings(TemplateSettingsType type, FileSystemEntry fileEntry);

    Dictionary<FileSystemEntry, BinaryObject> GetTemplateSettings(
      TemplateSettingsType type,
      FileSystemEntry[] entryList);

    BinaryObject GetTemplateByGuid(TemplateSettingsType type, string guid);

    BinaryObject GetTemplateByGuid(
      TemplateSettingsType type,
      FileSystemEntry parentFolder,
      string guid,
      bool recurse);

    void SaveTemplateSettings(TemplateSettingsType type, FileSystemEntry entry, BinaryObject data);

    bool TemplateSettingsObjectExists(TemplateSettingsType type, FileSystemEntry fileEntry);

    bool TemplateSettingsObjectExistsOfAnyType(TemplateSettingsType type, FileSystemEntry fileEntry);

    void DeleteTemplateSettingsObject(TemplateSettingsType type, FileSystemEntry fileEntry);

    void CreateTemplateSettingsFolder(TemplateSettingsType type, FileSystemEntry folderEntry);

    void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target);

    void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      bool forNewRESPA);

    void CopyTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target,
      RespaVersions respaVersion);

    void MoveTemplateSettingsObject(
      TemplateSettingsType type,
      FileSystemEntry source,
      FileSystemEntry target);

    FileSystemEntry[] GetTemplateSettingsXRefs(TemplateSettingsType type, FileSystemEntry fileEntry);

    BinaryObject GetOutputFormFile(string fileName);

    DateTime GetOutputFormFileLastWriteTime(string fileName);

    Hashtable GetLoanTemplateComponents(FileSystemEntry template);

    FileSystemEntry[] GetCustomLettersRecursively(
      CustomLetterType type,
      FileSystemEntry parentFolder);

    FileSystemEntry[] GetAllCustomLetterFileEntries(CustomLetterType type, string userId);

    FileSystemEntry[] GetAllPublicCustomLetterFileEntries(CustomLetterType type);

    FileSystemEntry[] GetCustomLetterDirEntries(CustomLetterType type, FileSystemEntry parentFolder);

    FileSystemEntry[] GetFilteredCustomLetterDirEntries(
      CustomLetterType type,
      FileSystemEntry parentFolder);

    void NewCustomLetter(CustomLetterType type, FileSystemEntry entry, BinaryObject data);

    BinaryObject GetCustomLetter(CustomLetterType type, FileSystemEntry entry);

    void SaveCustomLetter(CustomLetterType type, FileSystemEntry entry, BinaryObject data);

    void DeleteCustomLetterObject(CustomLetterType type, FileSystemEntry entry);

    void CreateCustomLetterFolder(CustomLetterType type, FileSystemEntry entry);

    bool CustomLetterObjectExists(CustomLetterType type, FileSystemEntry entry);

    bool CustomLetterObjectExistsOfAnyType(CustomLetterType type, FileSystemEntry entry);

    void MoveCustomLetterObject(
      CustomLetterType contactType,
      FileSystemEntry source,
      FileSystemEntry target);

    void CopyCustomLetterObject(
      CustomLetterType contactType,
      FileSystemEntry source,
      FileSystemEntry target);

    CustomFormDetail[] GetCustomFormDetails();

    void SaveCustomFormDetail(CustomFormDetail formDetail);

    void RenameCustomFormDetailSource(string currentSource, string newSource);

    void CopyCustomFormDetail(string currentSource, string targetSource);

    void DeleteCustomFormDetail(string source);

    FileSystemEntry[] GetHelocTableDirEntries();

    FileSystemEntry[] GetHelocTableDirEntries(bool useNewHELOCHistoricTable);

    BinaryObject GetHelocTable(string name);

    BinaryObject GetHelocTable(string name, bool useNewHELOCHistoricTable);

    bool HelocTableObjectExists(string name);

    bool SaveHelocTable(string name, BinaryObject data);

    bool SaveHelocTable(string name, BinaryObject data, bool useNewHELOCHistoricTable);

    bool DeleteHelocTable(string name);

    bool DeleteHelocTable(string name, bool useNewHELOCHistoricTable);

    FileSystemEntry[] GetAllFormGroupFileEntries(string userID);

    FileSystemEntry[] GetFormGroupDirEntries(FileSystemEntry parentFolder);

    FileSystemEntry[] GetAllPublicFormGroupDirEntries(bool includeProperties);

    FileSystemEntry[] GetFilteredFormGroupDirEntries(FileSystemEntry parentFolder);

    FormInfo[] GetForms(FileSystemEntry entry);

    void SaveForms(FileSystemEntry entry, FormInfo[] forms);

    bool FormGroupObjectExists(FileSystemEntry entry);

    bool FormGroupObjectExistsOfAnyType(FileSystemEntry entry);

    void CreateFormGroupFolder(FileSystemEntry entry);

    void MoveFormGroupObject(FileSystemEntry source, FileSystemEntry target);

    void NewFormGroup(FileSystemEntry entry, FormInfo[] forms);

    void CopyFormGroupObject(FileSystemEntry source, FileSystemEntry target);

    void DeleteFormGroupObject(FileSystemEntry entry);

    FileSystemEntry[] GetFormGroupXRefs(FileSystemEntry fileEntry);

    string[] GetNotePhrases();

    void AddNotePhrase(string phrase);

    void AddNotePhrases(string[] phrases);

    void RemoveNotePhrase(string phrase);

    void RemoveNotePhrases(string[] phrases);

    EncompassSystemInfo GetEncompassSystemInfo();

    BinaryObject GetPluginAssembly(string assemblyName);

    Version GetPluginVersion(string assemblyName);

    string[] GetPluginAssemblyNames();

    void InstallPlugin(string assemblyName, BinaryObject pluginAssembly);

    void UninstallPlugin(string assemblyName);

    ArrayList GetSecondaryFields(SecondaryFieldTypes type);

    object[] GetAllSecondaryFields();

    void SetSecondaryFields(ArrayList list, SecondaryFieldTypes type);

    void SetSecondarySecurityTypes(DataTable table);

    DataTable GetSecondarySecurityTypes();

    void SetFannieMaeProductNames(DataTable table);

    DataTable GetFannieMaeProductNames();

    DataTable GetLoanSynchronizationFields();

    void SetLoanSynchronizationFields(DataTable table);

    HtmlEmailTemplate[] GetHtmlEmailTemplates(string ownerID, HtmlEmailTemplateType type);

    HtmlEmailTemplate GetHtmlEmailTemplate(string ownerID, string guid);

    void SaveHtmlEmailTemplate(HtmlEmailTemplate template);

    void DeleteHtmlEmailTemplate(HtmlEmailTemplate template);

    bool DataServicesOptOut();

    void UpdateCompanyDataServicesOpt(string key);

    void UpdateUserDataServicesOpt(string userID, string key);

    Hashtable GetServicingLateCharge(string stateAbbr);

    void UpdateServicingLateCharge(Hashtable states);

    Hashtable GetServicingMortgageStatements();

    void UpdateServicingMortgageStatements(Hashtable formNames);

    CountyLimit[] GetCountyLimits();

    void UpdateCountyLimits(CountyLimit[] countyLimits);

    void ResetCountyLimits(CountyLimit[] countyLimits);

    void ResetConventionalCountyLimits(ConventionalCountyLimit[] countyLimits);

    int GetCountyLimit(string stateAbb, string countyName, int numOfUnits);

    int GetCountyLimitByFips(string stateAbb, string fips, int numOfUnits);

    ConventionalCountyLimit[] GetConventionalCountyLimits();

    ConventionalCountyLimit[] GetConventionalCountyLimits(string state, int year);

    ConventionalCountyLimit[] GetConventionalCountyLimits(string state);

    FedTresholdAdjustment[] GetFedTresholdAdjustments();

    FedTresholdAdjustment[] GetFedTresholdAdjustments(int year);

    void ResetFedTresholdAdjustments(FedTresholdAdjustment[] adjustments);

    AMILimit[] GetAMILimits();

    AMILimit[] GetAMILimits(int year);

    AMILimit[] GetAMILimits(string fipsCode);

    void ResetAMILimits(AMILimit[] adjustments);

    MFILimit[] GetMFILimits();

    MFILimit[] GetMFILimits(string msaCode, string subjectState);

    void ResetMFILimits(MFILimit[] mFILimit);

    int AddMIRecord(MIRecord miRecord, LoanTypeEnum miType, string tabName, bool isForDownload);

    void DeleteMIRecord(int id, LoanTypeEnum miType, string tabName, bool isForDownload);

    MIRecord UpdateMIRecord(
      MIRecord miRecord,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload);

    void UpdateMITable(
      MIRecord[] miRecords,
      LoanTypeEnum miType,
      string tabName,
      bool isForDownload);

    MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName);

    MIRecord[] GetMIRecords(LoanTypeEnum miType, string tabName, bool isForDownload);

    MIRecord[] GetMIRecordsDetail(LoanTypeEnum miType, string tabName, bool isForDownload);

    MIRecordXML ExportMIRecord(LoanTypeEnum miType, string tabName, bool isForDownload);

    bool HasDuplicateMIRecord(int id, string scenarioKey, LoanTypeEnum miType, string tabName);

    void AddMITab(string tabName);

    void DeleteMITab(string tabName);

    void UpdateMITab(string oldTabName, string newTabName);

    string[] GetMITabNames();

    bool HadDuplicateMITab(string tabName);

    void UpdateMIOrder(List<int[]> ids, LoanTypeEnum miType, string tabName, bool isForDownload);

    int AddTrusteeRecord(TrusteeRecord trusteeRecord);

    void DeleteTrusteeRecord(int[] ids);

    bool HasDuplicateTrusteeRecord(int id, string contactName);

    void UpdateTrusteeRecord(TrusteeRecord trusteeRecord);

    TrusteeRecord[] GetTrusteeRecords();

    FileSystemEntry[] GetUserAccessibleViews(string userId);

    ServerTaskScheduleInfo[] GetTaskSchedules(bool activeOnly, bool includeSystemTasks);

    ServerTaskScheduleInfo[] GetUserTaskSchedules(string userId);

    ServerTaskScheduleInfo GetTaskSchedule(int scheduleId);

    ServerTaskScheduleInfo CreateTaskSchedule(ServerTaskScheduleInfo schedule);

    void UpdateTaskSchedule(ServerTaskScheduleInfo schedule);

    void DeleteTaskSchedule(int scheduleId);

    ServerTaskHistoryInfo[] GetTaskScheduleHistory(int scheduleId, DateTime startTime);

    EPassMessageInfo GetEPassMessage(string messageId);

    EPassMessageInfo[] GetEPassMessages(string[] messageTypes);

    int GetEPassMessageCount(string[] messageTypes);

    EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid);

    EPassMessageInfo[] GetEPassMessagesForLoan(string loanGuid, string userId);

    EPassMessageInfo[] GetEPassMessagesForUser(string userId);

    EPassMessageInfo[] GetEPassMessagesForUser(string userId, string[] messageTypes);

    int GetEPassMessageCountForUser(string userId, string[] messageTypes);

    EPassMessageInfo[] GetLoanMailboxMessagesForUser(string userId);

    int GetLoanMailboxMessageCountForUser(string userId);

    void DeleteEPassMessages(string[] msgIds);

    void UpsertEPassMessage(EPassMessageInfo ePassMessage);

    void ResetEPassMessages();

    string AddMilestoneTask(MilestoneTaskDefinition milestoneTask);

    void UpdateMilestoneTask(MilestoneTaskDefinition milestoneTask);

    void DeleteMilestoneTasks(string[] taskGUID);

    bool IsDuplicatedMilestoneTasks(string newTaskName);

    MilestoneTaskDefinition[] GetMilestoneTasks(string[] taskGUIDs);

    Hashtable GetMilestoneTasks();

    void UpdateLRAdditionalFields(LRAdditionalFields fields);

    LRAdditionalFields GetLRAdditionalFields();

    bool IsReferencedAsDefaultViewTemplatePath(FileSystemEntry source);

    void RemoveAllDefaultViewReference(FileSystemEntry source);

    int CreateBusinessCalendar(BusinessCalendar calendar, HolidaySchedule defaultHolidays);

    List<BusinessCalendar> GetAllBusinessCalendars();

    BusinessCalendar GetBusinessCalendar(CalendarType calendarType);

    BusinessCalendar GetBusinessCalendar(CalendarType calendarType, DateTime startDate);

    BusinessCalendar GetBusinessCalendar(
      CalendarType calendarType,
      DateTime startDate,
      DateTime endDate);

    BusinessCalendar GetFullCalendar(
      CalendarType calendarType,
      DateTime startDate,
      DateTime endDate);

    void SaveBusinessCalendar(BusinessCalendar calendar);

    DateTime AddBusinessDays(
      CalendarType calendarType,
      DateTime date,
      int daysToAdd,
      bool startFromClosestBusinessDay);

    DateTime GetClosestBusinessDay(CalendarType calendarType, DateTime date);

    Dictionary<string, DisclosureTrackingFormItem.FormType> GetAllDisclosureFroms();

    void UpdateDisclosureTrackingFroms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList);

    Dictionary<string, DisclosureTrackingFormItem.FormType> GetAllDisclosure2015Forms();

    void UpdateDisclosureTracking2015Forms(
      Dictionary<string, DisclosureTrackingFormItem.FormType> formList);

    List<ProductPricingSetting> GetProductPricingSettings();

    List<ProductPricingSetting> UpdateProductPricingSettings(List<ProductPricingSetting> settings);

    void ProductPricingExportUser(string providerId);

    ProductPricingSetting GetActiveProductPricingPartner();

    ProductPricingSetting GetProductPricingPartner(string partnerID);

    bool IsProductAndPricingExpired(DateTime pricingDate);

    void UpdateFeeManagement(FeeManagementSetting feeManagementSetting);

    FeeManagementSetting GetFeeManagement();

    ePassCredentialSetting UpdateePassCredentialSetting(ePassCredentialSetting newSetting);

    List<ePassCredentialSetting> GetAllePassCredentialSettings();

    List<string> GetUserIDListByePassCredentialID(int credentialID);

    List<ePassCredentialSetting> GetUserePassCredentialSettings(string userID);

    void UpdateePassCredentialUserList(int credentialID, string providerName, string[] userList);

    void DeleteePassCredentialSetting(int credentialID);

    List<string> GetDuplicateUsers(int currentCredentialID, string providerName, string[] userList);

    string[] GetLoanProgramAdditionalFields();

    void SetLoanProgramAdditionalFields(string[] fieldIds);

    LoanProgramFieldSettings GetLoanProgramFieldSettings();

    ZipcodeInfoUserDefined[] GetZipcodeUserDefined(string zipcode);

    ZipcodeUserDefinedList GetZipcodeUserDefinedList();

    void DeleteZipcodeUserDefineds(ZipcodeInfoUserDefined[] zipcodeInfoUserDefineds);

    void UpdateZipcodeUserDefined(
      ZipcodeInfoUserDefined newZipcodeInfoUserDefined,
      ZipcodeInfoUserDefined oldZipcodeInfoUserDefined);

    bool IsZipcodeUserDefinedDuplicated(
      string zipcode,
      string zipcodeExt,
      string state,
      string city);

    IPRange[] GetAllowedIPRanges();

    void DeleteAllowedIPRange(int oid);

    void DeleteAllowedIPRanges(int[] oids);

    void AddAllowedIPRange(IPRange ipRange);

    void UpdateAllowedIPRange(IPRange ipRange);

    LockExtensionPriceAdjustment[] GetLockExtensionPriceAdjustments();

    bool UpdateLockExtensionPriceAdjustments(LockExtensionPriceAdjustment[] newAdjustment);

    LockExtensionPriceAdjustment[] GetLockExtPriceAdjustPerOccurrence();

    bool UpdateLockExtPriceAdjustPerOccurrence(LockExtensionPriceAdjustment[] newAdjustment);

    StatusOnlineSetup GetStatusOnlineSetup(string owner);

    void SaveStatusOnlineSetup(string owner, StatusOnlineSetup setup);

    string[] GetMilestonesByStatusOnlineTriggerGUIDs(string[] selectedGUIDs);

    ImageAttachmentSettings GetImageAttachmentSettings();

    void SaveImageAttachmentSettings(ImageAttachmentSettings settings);

    WebCenterSettings GetWebCenterSettings();

    void SaveWebCenterSettings(WebCenterSettings settings);

    PlanCodeInfo[] GetCompanyPlanCodes(DocumentOrderType orderType);

    PlanCodeInfo[] GetCompanyPlanCodes(DocumentOrderType orderType, bool activeOnly);

    void AddCompanyPlanCodes(DocumentOrderType orderType, PlanCodeInfo[] newCodes);

    void RemoveCompanyPlanCodes(DocumentOrderType orderType, string[] codesToRemove);

    DocEngineStackingOrderInfo[] GetDocEngineStackingOrders(DocumentOrderType orderType);

    DocEngineStackingOrder GetDocEngineStackingOrder(string orderID);

    DocEngineStackingOrder GetDocEngineStackingOrder(DocumentOrderType orderType, string name);

    void SetDefaultDocEngineStackingOrder(string orderID);

    void SaveDocEngineStackingOrder(DocEngineStackingOrder stackingOrder);

    void DeleteDocEngineStackingOrder(string orderID);

    void AddKnownDocEngineStackingElements(DocumentOrderType orderType, StackingElement[] elements);

    StackingElement[] GetKnownDocEngineStackingElements(DocumentOrderType orderType);

    List<CustomPlanCode> GetCompanyCustomPlanCodes(DocumentOrderType orderType);

    CustomPlanCode GetCompanyCustomPlanCode(DocumentOrderType orderType, string planCode);

    void AddCompanyCustomPlanCode(CustomPlanCode newPlanCode);

    void RemoveCompanyCustomPlanCodes(
      DocumentOrderType orderType,
      List<CustomPlanCode> planCodeList);

    List<string> GetHighCostStates();

    void UpdateHighCostStates(List<string> stateList);

    List<string[]> GetChangeCircumstanceOptions();

    void SetChangeCircumstanceOptions(List<string[]> options);

    List<ChangeCircumstanceSettings> GetAllChangeCircumstanceSettings();

    bool UpdateChangeCircumstance(List<ChangeCircumstanceSettings> changeCoCSettings);

    void DeleteChangeCircumstanceSetting(List<ChangeCircumstanceSettings> cocSettings);

    List<LoanCompPlan> GetAllCompPlans(bool activatedOnly, int compType);

    int AddCompPlan(LoanCompPlan newLoanCompPlan);

    int UpdateCompPlan(LoanCompPlan loanCompPlan);

    List<int> RemoveCompPlans(int[] IDplansToRemove);

    List<OrgInfo> GetOrganizationsUsingCompPlan(int loCompID);

    List<object[]> GetUsersUsingCompPlan(int loCompID);

    LoanCompHistoryList GetComPlanHistoryforOrg(int orgID, bool forExternal);

    LoanCompHistoryList GetComPlanHistoryforUser(string userID);

    List<LoanCompHistoryList> GetComPlanHistoryforAllOriginators();

    LoanCompPlan GetLoanCompPlanByID(int loCompID);

    LoanCompPlan GetCurrentComPlanforUser(string userID, DateTime triggerDateTime);

    LoanCompDefaultPlan GetDefaultLoanCompPlan();

    void SetLoanCompDefaultPlan(LoanCompDefaultPlan defaultPlan);

    void CreateHistoryCompPlansForUser(LoanCompHistoryList loanCompHistoryList, string userid);

    int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact);

    int AddExternalContacts(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      LoanCompHistoryList loanCompHistoryList);

    int AddBusinessContact(
      bool forLender,
      int businessID,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      int parent,
      int depth,
      string hierarchyPath);

    int AddManualContact(
      bool forLender,
      ExternalOriginatorEntityType entityType,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList);

    int AddManualContact(
      bool forLender,
      ExternalOriginatorManagementData newExternalContact,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList);

    int AddTPOContact(
      bool forLender,
      string id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      ExternalOriginatorEntityType entityType,
      string address,
      string city,
      string state,
      string zip,
      int parent,
      int depth,
      string hierarchyPath);

    int ImportExternalContact(
      ExternalOriginatorManagementData newExternalContact);

    void UpdateExternalContact(bool forLender, ExternalOriginatorManagementData externalContact);

    void UpdateExternalContact(
      bool forLender,
      ExternalOriginatorManagementData externalContact,
      LoanCompHistoryList loanCompHistoryList);

    void UpdateTPOContact(
      bool forLender,
      string id,
      ExternalOriginatorManagementData externalContact,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList);

    void OverwriteTPOContact(
      bool forLender,
      int oid,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      int parent,
      int depth,
      string hierarchyPath);

    void UpdateBusinessContact(
      bool forLender,
      int id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList);

    void UpdateManualContact(
      bool forLender,
      int id,
      string organizationName,
      string companyDBAName,
      string companyLegalName,
      string address,
      string city,
      string state,
      string zip,
      ExternalOriginatorEntityType entityType,
      bool useParentInfo,
      int parent,
      int depth,
      string hierarchyPath,
      LoanCompHistoryList loanCompHistoryList);

    void UpdateExternalOrgLOCompPlans(
      bool forLender,
      int oid,
      LoanCompHistoryList loanCompHistoryList);

    void UpdateLOCompUseParentInfo(bool useParentInfo, int oid);

    void UpdateExternalOrgHeirarchyPath(int oid, string heirarchyPath);

    void DeleteExternalContact(bool forLender, int oid);

    void DeleteExternalCompPlans(bool forLender, int oid, int CompPlanId);

    void DeleteExternalCompPlans(bool forLender, int oid, int[] CompPlanId);

    List<ExternalOriginatorManagementData>[] GetAllExternalOrganizations();

    List<ExternalOriginatorManagementData> GetAllExternalOrganizations(bool forLender);

    List<ExternalOriginatorManagementData> GetAllExternalParentOrganizations(bool forLender);

    ExternalOriginatorManagementData GetExternalOrganization(bool forLender, int oid);

    List<ExternalOriginatorManagementData> GetExternalOrganizationBranches(bool forLender, int oid);

    List<ExternalOriginatorManagementData> GetChildExternalOrganizationByType(
      int oid,
      List<int> orgType);

    List<ExternalOriginatorManagementData> GetOldTPOExternalOrganizations();

    ExternalOriginatorManagementData GetOldExternalOrganizationByDBAName(string dbaName);

    ExternalOriginatorManagementData GetOldExternalOrganizationByTPOID(string tpoid);

    List<ExternalOriginatorManagementData> GetExternalOrganizationByTPOID(string tpoId);

    int GetExternalRootOrgIdFromOrgChart();

    Dictionary<CorrespondentTradeCommitmentType, Decimal> GetCommitmentAvailableAmounts(
      ExternalOriginatorManagementData orgData);

    Dictionary<CorrespondentTradeCommitmentType, Decimal> GetCommitmentAvailableAmounts(
      ExternalOriginatorManagementData orgData,
      ICorrespondentTradeManager correspondentTradeManager);

    bool IsInOutstandingCommitments(string loanGuid);

    ExternalOriginatorManagementData GetRootOrganisation(bool forLender, int oid);

    List<int> GetExternalOrganizationParents(int oid);

    List<int> GetExternalOrganizationDesendents(int oid);

    List<string> GetExternalOrganizationDesendentsTPOID(int oid);

    void UpdateOrgTypeAndTpoID(List<int> children, int orgType, string TpoID);

    void UpdateExternalLicence(BranchExtLicensing license, int oid);

    BranchExtLicensing GetExtLicenseDetails(int oid);

    BranchExtLicensing GetExtLicenseDetails(string externalUserID);

    BranchExtLicensing GetExportLicensesDetails(int oid);

    string GetExtUserNotes(string externalUserID);

    List<ExternalOriginatorManagementData> GetExternalOrganizations(bool forLender, List<int> oids);

    List<ExternalOriginatorManagementData> GetExternalOrganizationDesendentEntities(
      int oid,
      bool recursive);

    List<ExternalOriginatorManagementData> GetExternalOrganizations(
      bool forLender,
      ExternalOriginatorEntityType entityType,
      string legalName,
      string dbaName,
      string cityName,
      string stateName,
      bool exactMatch,
      string sortedColumnName,
      bool sortedDescending,
      string currentUserID);

    ExternalOriginatorManagementData GetByoid(bool forLender, int oid);

    LoanCompHistoryList GetCompPlansByoid(bool forLender, int oid);

    List<ExternalOriginatorManagementData> GetContactByName(
      bool forLender,
      string ContactName,
      bool searchLegalName);

    bool CheckIfOrgNameExists(bool forLender, string orgName, int externalOrgId);

    bool CheckIfOrgExists(bool forLender, int externalOrgId);

    bool CheckIfOrgExistsWithTpoId(bool forLender, string TPOId);

    bool CheckIfOrgExistsWithTpoIdAndSalesRep(string TPOId, string userId);

    ExternalOriginatorManagementData GetExternalCompanyByTPOID(bool forLender, string TPOId);

    List<long> GetAllTpoID();

    List<string> GetAllLoginEmailID(string contactID);

    List<ExternalUserInfo> GetAllContactsByLoginEmailId(string loginEmailID, string externalUserId);

    List<ExternalUserInfoURL> GetExternalUserInfoUrlsByContactIds(string externalUserIds);

    void DisableContactsLogin(string externalUserIds);

    List<long> GetAllContactID();

    int GetOidByTPOId(bool forLender, string ExternalID);

    int GetOidByBusinessId(bool forLender, int Id);

    List<int> GetOidByParentId(bool forLender, int ParentID);

    List<HierarchySummary>[] GetAllHierarchy();

    List<HierarchySummary> GetHierarchy(bool forLender);

    List<HierarchySummary> GetHierarchy(bool forLender, int parentId);

    HierarchySummary GetOrgDetails(bool forLender, int oid);

    bool GetUseParentInfoValue(bool forLender, int oid);

    LoanCompPlan GetCurrentComPlanforBrokerByName(
      bool forLender,
      string brokerName,
      DateTime triggerDateTime);

    LoanCompPlan GetCurrentComPlanforBrokerByID(
      bool forLender,
      string brokerID,
      DateTime triggerDateTime);

    LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(string TPOWebID, DateTime triggerDateTime);

    LoanCompPlan GetCurrentComPlanforBrokerByTPOWebID(
      bool forLender,
      string TPOWebID,
      DateTime triggerDateTime);

    List<ExternalOriginatorManagementData> GetExternalOrganizationsUsingCompPlan(int loCompID);

    void UpdateExternalOrgIsTestAccount(int oid, bool isTestAccount);

    void UpdateExternalOrgManager(int oid, string manager);

    List<object> GetExternalAdditionalDetails(int oid);

    List<object> GetExternalAdditionalDetails(string externalUserID);

    List<ExternalOriginatorManagementData> GetExternalOrganizationBranchesBySite(
      bool forLender,
      int oid,
      string siteID);

    Dictionary<ExternalOriginatorOrgSetting, object> GetExternalAdditionalDetails(
      int oid,
      List<ExternalOriginatorOrgSetting> requested);

    Dictionary<ExternalUserInfo.ExternalUserInfoSetting, object> GetExternalAdditionalDetails(
      string externalUserID,
      List<ExternalUserInfo.ExternalUserInfoSetting> requested);

    List<object> GetTPOInformationToolSettings(
      string companyExteralID,
      string companyName,
      string branchExteralID,
      string BranchName,
      string siteID,
      string currentUserID);

    List<ExternalOriginatorManagementData> GetExternalOrganizationsWithoutExtension(
      string currentUserID,
      string externalOrgID);

    ExternalOriginatorManagementData[] QueryExternalOrganizations(QueryCriterion[] criteria);

    List<ExternalOriginatorManagementData> GetCompanyOrganizations(int oid);

    List<ExternalOriginatorManagementData> GetCompanyOrganizations(string externalOrgID);

    ExternalOrgNotes GetExternalOrganizationNotes(int oid);

    int AddExternalOrganizationNotes(ExternalOrgNote newExtOrgNote);

    bool DeleteExternalOrganizationNotes(int oid, ExternalOrgNotes deletedExtOrgNotes);

    bool DeleteExternalOrganizationNotes(int oid, List<int> notesID);

    List<ExternalOrgAttachments> GetExternalAttachmentsByOid(int oid);

    List<string> GetExternalAttachmentFileNames();

    bool GetExternalAttachmentIsExpired(int oid);

    void InsertExternalAttachment(ExternalOrgAttachments attachment);

    void UpdateExternalAttachment(ExternalOrgAttachments attachment);

    void DeleteExternalAttachment(Guid guid, FileSystemEntry entry);

    void AddAttachment(FileSystemEntry entry, BinaryObject data);

    BinaryObject ReadAttachment(string fileName);

    ExternalOrgURL[] GetExternalOrganizationURLs();

    ExternalOrgURL GetExternalOrganizationURLbySiteID(string siteID);

    bool DeleteExternalOrganizationURL(string siteId);

    ExternalOrgURL AddExternalOrganizationURL(string siteId, string url);

    void UpdateExternalOrganizationURL(ExternalOrgURL orgUrl);

    void UpdateExternalOrganizationURLTPOAccessStatus(ExternalOrgURL orgUrl);

    void UpdateExternalOrganizationSelectedURLs(int oid, List<ExternalOrgURL> orgUrls, int root);

    void UpdateInheritWebCenterSetupFlag(int oid, bool inheritWebCenterSetup);

    void UpdateInheritCustomFieldsFlag(int oid, bool inheritCustomFields);

    ExternalOrgURL AssociateExternalOrganisationUrl(int oid, string siteId, int entityType);

    void DeleteExternalOrgSelectedUrl(int oid, int urlid);

    List<ExternalOrgURL> GetSelectedOrgUrls(int oid);

    bool CheckIfAnyTPOSiteExists();

    bool CheckIfNewTPOSiteExists(string siteID);

    bool CheckIfTPOWebCenterProvisioned();

    List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCurrentOrg(int oid);

    List<ExternalOrgSalesRep> GetExternalOrgSalesRepsForCompany(int oid);

    List<ExternalOrgLenderContact> GetExternalOrgSalesRepContactsForCompany(int oid);

    List<ExternalSettingType> GetExternalOrgSettingTypes();

    List<ExternalSettingValue> GetExternalOrgSettingsByType(int settingTypeId);

    List<ExternalSettingValue> GetExternalOrgSettingsByName(string settingName);

    ExternalSettingValue GetExternalOrgSettingsByID(int settingId);

    Dictionary<string, List<ExternalSettingValue>> GetExternalOrgSettings();

    int AddExternalOrgSettingValue(ExternalSettingValue externalSettingValue);

    bool DeleteExternalOrgSettingValues(string settingIds);

    void UpdateExternalOrgSettingValue(ExternalSettingValue externalSettingValue);

    void ChangeSettingSortId(ExternalSettingValue OldSetting, ExternalSettingValue NewSetting);

    List<ExternalOriginatorManagementData> GetExternalOrganizationsBySettingId(int settingId);

    List<ExternalUserInfo> GetExternalContactsBySettingId(int settingId);

    void AssignNewSettingValueToExternalOrg(int settingId, int settingTypeId, string oids);

    void AssignNewSettingValueToContact(int settingId, string contactIds);

    void AssignNewSettingValueToAttachments(int settingId);

    bool CheckIfAttachmentWithCategoryExists(int settingId);

    int GetOrgIdForExternalOrgID(int externalOrgId);

    ExternalOrgManagementDataCount GetExternalOrganizationDataWithCountByOid(int orgId);

    Dictionary<string, string> GetOrgTree(int orgId);

    bool AddExternalOrganizationSalesReps(ExternalOrgSalesRep[] newReps);

    bool AddExternalOrganizationSalesReps(int oid, string[] userIds);

    bool UpdateExternalOrganizationSalesRep(ExternalOrgSalesRep salesRep);

    bool DeleteExternalOrganizationSalesReps(int oid, string[] userIDs);

    List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRep(string userid);

    int GetExternalOrganizationCountForManagerID(string managerID);

    Dictionary<string, int> GetExternalOrganizationCountForManagerIDs(string[] managerIDs);

    List<int> GetExternalOrganizationsForManagerID(string managerID);

    void SetSalesRepAsPrimary(
      string userId,
      int externalOrgId,
      DateTime primarySalesRepAssignedDate = default (DateTime));

    string GetPrimarySalesRep(int externalOrgId);

    bool CheckIfSalesRepHasAnyContacts(string userId, int externalOrgId);

    bool CheckIfSalesRepIsPrimary(string userId, int externalOrgId);

    void ChangeSalesRepForContacts(
      string existingSalesRepUserId,
      string newSalesRepUserId,
      int externalOrgId,
      DateTime primaryAeSalesRepAssignedDate = default (DateTime));

    ExternalOrgLoanTypes GetExternalOrganizationLoanTypes(int oid);

    bool UpdateExternalOrganizationLoanTypes(int oid, ExternalOrgLoanTypes loanTypes);

    FieldRuleInfo GetExternalUnderwritingConditions(int oid);

    ExternalUserInfo[] GetExternalUserInfos(int externalOrgID, bool isKeyContact);

    ExternalUserInfo[] GetExternalUserInfosSummary(int externalOrgID, bool isKeyContact);

    ExternalUserInfo[] GetExternalUserInfosSummary(
      int externalOrgID,
      bool isKeyContact,
      bool getPersona);

    ExternalUserInfo[] GetAllExternalUserInfos(int oid);

    ExternalUserInfo[] GetAllExternalUserInfos(string tpoID);

    ExternalUserInfo GetExternalUserInfo(string externalUserID);

    List<ExternalUserInfo> GetExternalUserInfoList(List<string> externalUserIDs);

    List<ExternalUserInfo> GetExternalUserInfoListByContactId(List<string> contactIDs);

    List<ExternalUserInfo> GetExternalUserInfoList(List<string> contactIDs, int urlID);

    Persona[] GetExternalUserInfoUserPersonas(string contactID);

    ExternalUserInfo GetExternalUserInfoByContactId(string contactId);

    List<ExternalUserInfo> GetExternalUserInfoFromEmailandURLID(string loginEmail, string URLID);

    ExternalUserInfo SaveExternalUserInfo(ExternalUserInfo newExternalUserInfo);

    ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      int[] Urls = null);

    ExternalUserInfo SaveExternalUserInfo(
      ExternalUserInfo newExternalUserInfo,
      bool cleanUpStateLicenseBeforeUpdateLicense,
      bool setDefaultURL,
      int[] Urls = null);

    List<ExternalUserInfo> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool isCreate);

    List<string> UpsertExternalUserInfos(
      List<Tuple<ExternalUserInfo, int[]>> extUserInfos,
      bool returnUpdatedResult,
      out List<ExternalUserInfo> updatedExtUsers,
      bool isCreate);

    ExternalUserInfo[] QueryExternalUsers(QueryCriterion[] criteria);

    bool DeleteExternalUserInfo(
      int externalOrgID,
      ExternalUserInfo deletedExternalUserInfo,
      UserInfo userInfo);

    bool DeleteExternalUserInfos(
      int externalOrgID,
      List<ExternalUserInfo> extUsers,
      UserInfo userInfo);

    bool UpdateExternalUserLastLogin(ExternalUserInfo newExternalUserInfo);

    ExternalUserInfo ResetExternalUserInfoPassword(
      string externaluserID,
      string newPassword,
      DateTime date,
      bool requirePasswordChange);

    void SendWelcomeEmailUserInfo(string externaluserID, DateTime date, string userName);

    ExternalUserInfo ResetExternalUserInfoUpdatedDate(
      string externaluserID,
      DateTime date,
      string updatedBy,
      bool updatedByExternal);

    ExternalUserURL[] GetExternalUserInfoURLs(string externalUserID);

    List<ExternalUserURL> GetExternalUserURLs(
      string loginEmail,
      List<string> siteIds,
      string excludeExtUserID = null,
      bool activeUsersOnly = true);

    HashSet<string> GetMultipleExternalUserInfoURLs(string externalUserIDs);

    string GetUrlLink(int urlID);

    void SaveExternalUserInfoURLs(string externalUserID, int[] urlIDs);

    void SaveExternalUserInfoURLs(string externalUserID, string[] urls);

    List<ExternalUserInfo> ValidateExternalUserPasswordBySiteID(
      string loginEmail,
      string password,
      string urlID);

    bool DuplicateExternalUserLoginEmail(int orgID, string loginEmail);

    List<ExternalUserInfo> GetExternalUserInfoBySalesRep(string userid);

    List<ExternalUserInfo> GetAllCompanyManagers(int extID);

    bool ReassignSalesRep(
      List<string> extUserId,
      List<int> extOrgId,
      string salesRepId,
      string currSalesRepId);

    bool DoesTpoContactExists(ExternalUserInfo user, int destOid, List<int> urlsTobeAdded);

    void MoveExternalCompany(bool forLender, HierarchySummary summary);

    void MoveExternalUser(List<ExternalUserInfo> extUserList, int oId);

    LoanImportRequirement GetLoanImportRequirements();

    void SetLoanImportRequirements(
      LoanImportRequirement.LoanImportRequirementType fannieMaeImportRequirementType,
      string templateForFannieMaeImport,
      LoanImportRequirement.LoanImportRequirementType webCenterImportRequirementType,
      string templateForWebCenterImport);

    void SetLoanImportRequirements(LoanImportRequirement loanImportRequirement);

    List<ExternalOrgContact> GetExternalOrgContacts(int externalOrgID);

    int AddExternalOrgManualContact(ExternalOrgContact obj);

    bool UpdateExternalOrgManualContact(ExternalOrgContact obj);

    bool DeleteExternalOrgContact(List<ExternalOrgContact> externalOrgContact);

    bool AddTpoUserToExtOrgContact(string[] external_userid, int externalOrgID);

    List<UserInfo[]> GetAllSalesRepUsers(string currentUserID);

    List<ExternalUserInfo> GetAllAuthorizedDealers(int externalOrgID);

    ContactCustomFieldInfoCollection GetCustomFieldInfo();

    void UpdateCustomFieldInfo(
      ContactCustomFieldInfoCollection customFields,
      ArrayList invalidFieldIds);

    void UpdateCustomFieldValues(int orgID, ContactCustomField[] fields);

    ContactCustomField[] GetCustomFieldValues(int orgID);

    ExternalOrgCustomFields GetTpoCustomFields(int orgId);

    Dictionary<string, string> IsTpoOnWatchList(
      string companyId,
      string branchId,
      string lnProcessorId,
      string lnOfficerId);

    ArrayList GetExternalUserAndOrgBySalesRep(string userid);

    ArrayList GetExternalAndInternalUserAndOrgBySalesRep(string userid, int orgId);

    IList<ExternalOrgLenderContact> GetGlobalLenderContacts();

    IList<ExternalOrgLenderContact> GetGlobalLenderContacts(int externalOrgId);

    IList<ExternalOrgLenderContact> GetLenderContacts(int externalOrgId);

    int AddLenderContact(ExternalOrgLenderContact newContact);

    bool UpdateLenderContact(ExternalOrgLenderContact contact);

    bool UpdateLenderContacts(params ExternalOrgLenderContact[] contacts);

    bool DeleteLenderContact(int contactId);

    IList<ExternalOrgLenderContact> GetTPOCompanyLenderContacts(int orgId);

    void AddTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder);

    void UpdateTPOCompanyLenderContact(
      int externalOrgId,
      int contactID,
      ExternalOrgCompanyContactSourceTable source,
      int hide,
      int displayOrder);

    void UpdateTPOCompanyLenderContacts(
      int externalOrgId,
      params ExternalOrgLenderContact[] contacts);

    List<object> GetAllExternalOrganizationNames();

    List<object> GetTPOForClosingVendorInformation(string tpoOrgID, string tpoLOID);

    List<object> GetAccessibleExternalUserInfoList(string userid);

    List<ExternalUserInfo> GetAccessibleExternalUserInfos(string userid);

    bool UpdateHomeCounselingCodes(
      List<KeyValuePair<string, string>> services,
      List<KeyValuePair<string, string>> languages);

    List<KeyValuePair<string, string>>[] GetHomeCounselingServiceLanguageSupported();

    List<KeyValuePair<string, string>> GetHomeCounselingServiceSupported();

    List<KeyValuePair<string, string>> GetHomeCounselingLanguageSupported();

    List<ExternalUserInfo> GetAllLOLPUsers();

    List<ExternalUserInfo> GetAllTPOLOLPUsers(List<int> personaIds);

    bool ReassignTPOLoans(ExternalUserInfo oldUser, ExternalUserInfo newUser, bool isTPOMVP = false);

    bool ReassignTPOLoanSalesRep(List<int> orgList, List<string> userList);

    string BackupTPOData();

    bool RestoreTPOData(string restoreData);

    void RebuildExternalOrgs();

    Dictionary<string, List<ExternalOriginatorManagementData>> GetCompanyAncestors(int oid);

    List<string> GetAEAccessibleExternalUser(string[] userids);

    List<ExternalFeeManagement> GetFeeManagement(int externalOrgID);

    List<ExternalFeeManagement> GetFeeManagementListByChannel(ExternalOriginatorEntityType channel);

    ExternalLateFeeSettings GetGlobalLateFeeSettings();

    ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      int externalOrgID,
      bool getGlobalIfNotFound);

    ExternalLateFeeSettings GetExternalOrgLateFeeSettings(
      string externalTPOCompanyID,
      bool getGlobalIfNotFound);

    int GetGlobalOrSpecificTPOSetting(int externalOrgID);

    void InsertFeeManagementSettings(ExternalFeeManagement feeManagement, int externalOrgID);

    void UpdateFeeManagementSettings(ExternalFeeManagement feeManagement);

    void SetDefaultFeeManagementListByChannel(
      int externalOrgID,
      ExternalOriginatorEntityType channel);

    void SetSelectedFeeManagementList(int externalOrgID, List<ExternalFeeManagement> fees);

    void DeleteFeeManagementSettings(List<int> feeManagementIDs);

    void DeleteTPOFeeManagementSettings(int externalOrgID);

    void InsertLateFeeSettings(ExternalLateFeeSettings lateFeeSettings, int externalOrgID);

    void UpdateOrgLateFeeSettings(ExternalLateFeeSettings lateFeeSettings, int externalOrgID);

    void UpdateGlobalLateFeeSettings(ExternalLateFeeSettings lateFeeSettings);

    void DeleteLateFeeSettings(int lateFeeSettingID);

    void UpdateGlobalOrSpecificTPOSetting(int externalOrgID, int globalOrSpecificTPO);

    List<ExternalOrgDBAName> GetDBANames(int externalOrgID);

    ExternalOrgDBAName GetDefaultDBAName(int externalOrgID);

    bool GetInheritDBANameSetting(int externalOrgID);

    int InsertDBANames(ExternalOrgDBAName name, int externalOrgID);

    void UpdateDBANames(ExternalOrgDBAName name);

    void DeleteDBANames(List<ExternalOrgDBAName> names);

    void SetDBANamesSortIndex(Dictionary<int, int> dbas, int externalOrgID);

    void SetDBANameAsDefault(int externalOrgID, int DBAID);

    void UpdateInheritDBANameSetting(int externalOrgID, bool useParentInfo);

    void UpdateAllChildrenDBANameSetting(int parent);

    List<DocumentSettingInfo> GetAllArchiveDocuments(int externalOrgID);

    void UnArchiveDocuments(int externalOrgID, List<string> guids);

    void ArchiveDocuments(int externalOrgID, List<string> guids);

    void DeleteDocument(int externalOrgID, Guid guid, FileSystemEntry entry);

    void AddDocument(int externalOrgID, DocumentSettingInfo document, bool isTopOfCategory);

    void UpdateDocument(int externalOrgID, DocumentSettingInfo document);

    void CreateDocumentInDataFolder(FileSystemEntry entry, BinaryObject data);

    BinaryObject ReadDocumentFromDataFolder(string fileName);

    List<DocumentSettingInfo> GetExternalDocuments(int externalOrgID, int channel, int status);

    List<DocumentSettingInfo> GetExternalDocumentsForOrgAssignment(int externalOrgID);

    Dictionary<int, List<DocumentSettingInfo>> GetExternalOrgDocuments(
      int externalOrgID,
      int channel,
      int status,
      bool disableGlobalDocs);

    void UpdateActiveStatus(int externalOrgID, bool activeChecked, bool isDefault, Guid guid);

    void UpdateDocumentCategory(int oldCategory, int newCategory);

    void AssignDefaultDocumentToAll(DocumentSettingInfo document);

    void RemoveDefaultDocumentFromAll(DocumentSettingInfo document);

    void RemoveAssignedDocFromTPO(string guid, int externalOrgId);

    void RemoveAssignedDocFromTPOs(string guid, List<int> externalOrgIds);

    void AssignDocumentToOrg(DocumentSettingInfo document, bool isTopOfCategory);

    void MoveDocumentSortOrder(int externalOrgId, DocumentSettingInfo document, int startSortId);

    int GetDocumentSortId(int externalOrgId, DocumentSettingInfo document);

    void AssignDocumentsToTposByRelatedDoc(
      List<int> externalOrgIds,
      DocumentSettingInfo document,
      DocumentSettingInfo relatedDocument,
      bool IsTopOfCategoryorDoc);

    int GetDocumentMaxSortId(int externalOrgid, DocumentSettingInfo document);

    void AssignDocumentToTpos(List<int> externalOrgIds, DocumentSettingInfo document);

    List<int> GetExternalOrgsByDocumentGuid(Guid guid);

    void UpdateActiveStatusAllDocsInCategory(int externalOrgID, int category, bool activeChecked);

    void SwapDocumentSortIds(
      int externalOrgID,
      DocumentSettingInfo firstDoc,
      DocumentSettingInfo secondDoc);

    void AssignAllDefaultDocumentsToTOPOrg(int externalOrgID);

    Dictionary<string, bool> GetDocAssignedTPOs(Guid guid);

    List<ExternalBank> GetExternalBankByName(string bankName);

    int AddExternalBank(ExternalBank bank);

    void UpdateExternalBank(int id, ExternalBank bank);

    ExternalBank GetExternalBankById(int id);

    void DeleteExternalBank(int oid);

    List<HierarchySummary> GetBankHierarchy();

    List<ExternalBank> GetExternalBanks();

    List<ExternalBank> GetPaginatedExternalBanks(int start, int limit, out int totalRecords);

    List<ExternalBank> GetSelectedExternalBanks(int[] bankIds);

    bool AnyWarehousesUsingThisBank(int oid);

    List<ExternalOrgWarehouse> GetExternalOrgWarehouses(int oid);

    List<ExternalOrgWarehouse> GetExternalOrgWarehousesbyCompanies(int oid);

    ExternalOrgWarehouse AddExternalOrgWarehouse(ExternalOrgWarehouse warehouse);

    void UpdateExternalOrgWarehouse(int id, ExternalOrgWarehouse warehouse);

    void DeleteExternalOrgWarehouse(int id, int externalOrgId);

    bool GetInheritWarehouses(int externalOrgID);

    void UpdateInheritWarehouses(int externalOrgID, bool useParentInfo);

    List<ExternalOriginatorManagementData> GetExternalOrganizationBySalesRepWithPrimarySalesRep(
      string userid);

    List<List<HierarchySummary>> SearchOrganization(string type, string keyword);

    Dictionary<CorrespondentMasterDeliveryType, Decimal> GetNonAllocatedOutstandingCommitments(
      string externalID);

    ArrayList GetTPOWCAEView(string userid, int orgId, int urlID);

    BinaryObject GetTemplateSettings(TemplateSettingsType templateSettingsType, int templateID);

    string[] GetStackingOrderTemplateNamesByExportTemplates(string[] documentExportTemplateNames);

    string GetDrysdaleHostForInstance(string instanceName);

    string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table1,
      string table2,
      string table1PrimaryKeyColumn,
      string table2ForeignKeyColumn,
      string whereClausColumn,
      string[] templates);

    string[] GetCustomFieldsByTemplates(
      string customFieldColumn,
      string table,
      string whereClausColumn,
      string[] templates);

    ExternalOrgOnrpSettings GetExternalOrgOnrpSettings(int externalOrgId);

    ExternalOrgOnrpSettings GetExternalOrgOnrpSettingsByTPOId(string tpoId);

    void InsertOnrpSettings(ExternalOrgOnrpSettings onrpSettings, int externalOrgId);

    void UpdateOnrpSettings(
      ExternalOrgOnrpSettings onrpSettings,
      int externalOrgId,
      ExternalOrgOnrpSettings onrpSettingsOld);

    ONRPEntitySettings GetOnrpRetailSettingsByOrgId(int orgId);

    bool IsRetailBranchExist(int oid);

    List<SyncTemplate> GetAllSyncTemplates();

    List<int> RemoveSyncTemplates(List<int> ids);

    int UpdateSyncTemplate(SyncTemplate syncTemplate);

    bool SyncTemplateNameExist(string templateName);

    void SaveEPPSLoanProgramsSettings(List<EPPSLoanProgram> programs);

    List<EPPSLoanProgram> GetEPPSLoanProgramsSettings();

    string getOAPIURL();

    PurchaseConditionCategory GetCategory(string categoryName);

    List<PurchaseConditionCategory> GetAllCategories();

    List<PurchaseConditionCategory> GetSubCategories(int parentCategoryId);

    PurchaseConditionStatus GetStatus(string statusName);

    List<PurchaseConditionStatus> GetAllStatuses();

    int CreatePurchaseConditionCategory(
      PurchaseConditionCategory purchaseConditionCategory);

    string GetFieldsUsedInRules(FieldSearchRuleType[] ruleTypes);

    DDMAffectedFieldsandDataTableNames GetFieldsAndDataTableNamesUsedInRules(
      FieldSearchRuleType[] ruleTypes);

    bool ExistsConcurrentUserLogin(string userID, string appName, string sessionID = "�");

    void SaveTitleFeeCredentials(string orderUID, string loanGUID, string credentials);

    string[] GetTitleFeeCredentials(string[] orderUIDs, string loanGUID);

    string GetThinClientURL();

    string GetBidTapeThinClientURL();

    IList<BidTapeField> GetBidTapeFields();

    void UpdateBidTapeFields(IList<BidTapeField> fields);

    Dictionary<string, string> GetTpoLoanOfficer(string orgId, int realWorldRoleId);

    Hashtable GetExternalOrganizationsAvailableCommitment(int[] oids);

    void UpdateBestEffortDailyLimit(string entityId, DateTime lockDate, double loanAmount);

    void UpdateBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount,
      string loanGuid,
      string rateSheetId);

    double GetBestEffortDailyLimit(string entityId, DateTime lockDate);

    double GetBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      string rateSheetId,
      string loanGuid);

    void ResetBestEffortDailyLimit(
      string entityId,
      DateTime lockDate,
      double loanAmount,
      string loanGuid);

    void ChangeTradeStatus(bool isAllowPublishChecked);

    List<EnhancedConditionSet> GetEnhancedConditionSets();

    void UpdateEnhancedConditionSet(EnhancedConditionSet conditionset);

    DataSet GetEnhanceConditionTemplateNameAndType(Guid setid);

    List<string> GetTrackingDefinitionsOfConditionType(string type);

    List<string> GetEnhancedConditionTypeList(bool active);

    EnhancedConditionSet GetEnhancedConditionSetDetail(
      Guid setid,
      bool active,
      bool detailincluded);

    bool IsUniqueSetName(string setname, Guid id);

    HashSet<string> GetOptionsFromConditionType(string attribute);

    string GetServicesManagementThinClientURL(string url);

    List<ePassCredential> GetePassCredential(string category);

    SkyDriveUrl GetSkyDriveUrl(string attachmentId);

    HashSet<string> CheckIfTPOUsersHaveLoansAssigned(List<string> contactIds);

    List<AutoRetrieveSettings> GetAutoRetrieveSettings();

    IList<LockComparisonField> GetLockComparisonFields();
  }
}
