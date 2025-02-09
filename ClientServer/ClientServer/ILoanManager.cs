// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.ILoanManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Cache;
using EllieMae.EMLite.ClientServer.Query;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.DataEngine.Log;
using EllieMae.EMLite.RemotingServices;
using EllieMae.EMLite.Trading;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface ILoanManager
  {
    LoanTemplateInfo[] GetLoanTemplates();

    LoanData InstantiateLoanTemplate(string name);

    LoanData InstantiateLoanTemplateFromPlatform(string name);

    string GetNextLoanNumber();

    string GetNextLoanNumber(OrgInfo orgInfo);

    string GetNextMersNumber(bool alwaysGenerate, OrgInfo sourceOrg);

    string GenerateUniqueLoanName(string folderName, string baseLoanName);

    ILoan CreateLoan(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction);

    ILoan CreateLoan(
      string folderName,
      string loanName,
      LoanData data,
      DuplicateLoanAction dupAction,
      bool addLoanNumber);

    ILoan OpenLoan(LoanIdentity id);

    ILoan OpenLoan(LoanIdentity id, LoanInfo.LockReason reason);

    ILoan OpenLoan(string guid);

    ILoan OpenLoan(string guid, LoanInfo.LockReason reason);

    ILoan OpenLoan(string folderName, string loanName);

    ILoan OpenLoan(string folderName, string loanName, LoanInfo.LockReason reason);

    void DeleteLoan(string guid, bool demandAdmin, bool skipSystemLogging);

    void DeleteLoan(string guid, bool demandAdmin);

    void DeleteLoan(string guid);

    ILoanConfigurationInfo GetLoanConfigurationInfo();

    ILoanConfigurationInfo GetLoanConfigurationInfo(LoanConfigurationParameters configParams);

    ILoanConfigurationInfo GetLoanConfigurationInfo(
      LoanConfigurationParameters configParams,
      string loanFolder,
      string loanName,
      string hmdaProfileID = null);

    ILoanConfigurationInfo GetLoanConfigurationInfo(
      LoanConfigurationParameters configParams,
      ILoan loandata,
      string loanFolder,
      string loanName);

    LoanDefaults GetLoanDefaultData();

    ILoanSettings GetLoanSettings();

    ILoanSettings GetLoanSettings(string hmdaProfileID);

    FieldSettings GetFieldSettings();

    LoanIdentity GetLoanIdentity(string guid);

    LoanIdentity GetLoanIdentity(string folderName, string loanName);

    LoanProperties GetLoanProperties(string guid);

    bool LoanExists(string guid);

    bool LoanExists(string folderName, string loanName);

    void CreateLoanFolder(string name);

    void DeleteLoanFolder(string name, bool forceDelete);

    bool DoesLoanFolderExist(string name);

    void MoveLoan(LoanIdentity id, string targetFolder, DuplicateLoanAction dupAction);

    LoanFolderInfo GetLoanFolder(string name);

    string[] GetAllLoanFolderNames(bool includeTrashFolder);

    string[] GetAllLoanFolderNames(bool includeTrashFolder, bool applySecurity);

    LoanFolderInfo[] GetAllLoanFolderInfos(bool includeTrashFolder);

    LoanFolderInfo[] GetAllLoanFolderInfos(bool includeTrashFolder, bool applySecurity);

    void SetLoanFolderType(string folderName, LoanFolderInfo.LoanFolderType folderType);

    LoanIdentity[] GetLoanFolderContents(
      string folderName,
      LoanInfo.Right accessRights,
      bool isExternalOrganization);

    int GetLoanFolderPhysicalSize(string folderName);

    void SetIncludeInDuplicateLoanCheck(string folderName, bool dupLoanCheck);

    bool GetIncludeInDuplicateLoanCheck(string folderName);

    string GetAllIncludeInDuplicateLoanCheck();

    PipelineInfo[] GetPipeline(string loanFolder, bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      string loanFolder,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization);

    PipelineInfo[] GetPipeline(string[] guids, bool isExternalOrganization);

    PipelineInfo[] GetPipeline(string[] guids, bool isExternalOrganization, int sqlRead);

    PipelineInfo[] GetPipeline(
      string[] guids,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      TradeType tradeType = TradeType.None);

    void RebuildPipeline(IServerProgressFeedback feedback, DatabaseToRebuild dbToRebuild);

    void RebuildPipeline(
      IServerProgressFeedback feedback,
      bool fastRebuild,
      DatabaseToRebuild dbToRebuild);

    void RebuildPipeline(
      string folderName,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild);

    void RebuildPipeline(
      string[] folderNames,
      IServerProgressFeedback feedback,
      DatabaseToRebuild dbToRebuild);

    void ClearReportingDatabase(IServerProgressFeedback feedback);

    void RebuildIndex(string[] columnNames, IServerProgressFeedback feedback);

    void UpdateExtendedLoanSummary(IServerProgressFeedback feedback);

    void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      IServerProgressFeedback2 feedback);

    void RebuildReportingDb(
      bool useERDB,
      bool pendingFieldsOnly,
      IServerProgressFeedback2 feedback,
      bool updateAllLoans);

    void RebuildLoan(string loanFolder, string loanName, DatabaseToRebuild dbToRebuild);

    PipelineScreenData GetPipelineScreenData();

    List<UserPipelineView> GetPipelineData();

    List<PersonaPipelineView> GetPersonaPipelineData();

    PipelineScreenData GetUserCustomPipelineScreenData();

    AlertSetupData GetAlertSetupData();

    ICursor OpenPipeline(
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead = 0);

    ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization);

    ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead);

    ICursor OpenPipeline(
      string[] loanFolders,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead);

    ICursor OpenPipeline(
      string[] loanFolders,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      SortField[] sortFields,
      QueryCriterion filter,
      bool isExternalOrganization,
      int sqlRead,
      bool isGlobalSearch,
      bool excludeArchivedLoans);

    ICursor OpenPipelineForReassignment(
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      bool isExternalOrganization,
      string externalOrgID,
      bool filter,
      int sqlRead = 0);

    ICursor OpenPipelineForReassignment(
      string roleID,
      string userID,
      string folderName,
      string[] fields,
      PipelineData dataToInclude,
      QueryCriterion queryCriterion,
      SortField[] sortFields,
      bool isExternalOrganization,
      string externalOrgID = null,
      bool filter = false,
      int sqlRead = 0);

    ICursor UpdatePipelineForReassignment(
      PipelineInfo[] pinfos,
      string[] fields,
      PipelineData dataToInclude);

    ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization);

    ICursor OpenPipeline(
      string loanFolder,
      LoanInfo.Right rights,
      string[] fields,
      PipelineData dataToInclude,
      PipelineSortOrder sortOrder,
      QueryCriterion filter,
      bool isExternalOrganization,
      bool excludeArchivedLoans);

    ICursor OpenContactPipeline(
      string[] fields,
      SortField[] sortFields,
      int contactID,
      CRMLogType logType,
      bool isExternalOrganization);

    LoanIdentity[] QueryLoans(QueryCriterion[] criteria, bool isExternalOrganization);

    QueryResult QueryPipeline(DataQuery query, bool isExternalOrganization);

    QueryResult QueryPipelineFromHomepage(DataQuery query, bool isExternalOrganization);

    QueryCursor OpenQuery(DataQuery query, bool isExternalOrganization);

    QueryCursor OpenQuery(
      DataQuery query,
      bool fromERDB,
      string applicationExe,
      bool isExternalOrganization);

    LoanInfo.Right GetEffectiveLoanAccessRights(string guid);

    LoanInfo.Right[] GetEffectiveLoanAccessRights(string[] guids);

    LoanInfo.Right GetEffectiveLoanAccessRights(string userId, string guid);

    LoanInfo.Right[] GetEffectiveLoanAccessRights(string userId, string[] guids);

    void AddLoanAccessRights(string[] guids, string[] userIds, LoanInfo.Right rights);

    void RemoveLoanAccessRights(string[] guids, string[] userIds, LoanInfo.Right rights);

    string[] GetRecentLoanGuids(int maxItems, bool isExternalOrganization);

    PipelineInfo[] GetRecentLoanInfo(int maxItems, bool isExternalOrganization);

    LockInfo[] GetCurrentLocks(bool includeCrashedSessionLocks, bool isExternalOrganization);

    LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      bool refreshCache,
      bool isExternalOrganization);

    LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string loanFolder,
      bool isExternalOrganization);

    LockInfo[] GetCurrentLocks(
      bool includeCrashedSessionLocks,
      string loanFolder,
      bool refreshCache,
      bool isExternalOrganization);

    ILoan Import(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction);

    ILoan Import(string folderName, string loanName, LoanData data, DuplicateLoanAction dupAction);

    ILoan ImportNew(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction);

    ILoan ImportNew(
      string folderName,
      string loanName,
      BinaryObject encryptedData,
      DuplicateLoanAction dupAction,
      bool transfer);

    StatusOnlineLoanSetup GetStatusOnlineSetup(LoanIdentity loanid);

    StatusOnlineLoanSetup SaveStatusOnlineTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList);

    StatusOnlineLoanSetup DeleteStatusOnlineTriggers(
      LoanIdentity loanid,
      StatusOnlineTrigger[] triggerList);

    StatusOnlineLoanSetup SetStatusOnlinePrompt(LoanIdentity loanid, bool showPrompt);

    [PersistentClientCacheable("LoanXDBTableList-{0}")]
    LoanXDBTableList GetLoanXDBTableList(bool useERDB);

    LoanXDBStatusInfo GetLoanXDBStatus(bool useERDB);

    LoanXDBField[] GetAuditTrailLoanXDBField();

    LoanXDBAuditField[] GetAuditTrailReportingLoanXDBField();

    Dictionary<string, AuditRecord> GetLastAuditRecordsForLoan(string guid);

    Dictionary<string, AuditRecord> GetLastAuditRecordsForLoan(string guid, string[] fieldIds);

    LoanXDBStatusInfo ApplyReportingDatabaseChanges(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey);

    bool ResetReportingDatabase(
      bool useERDB,
      LoanXDBTableList tableList,
      string validationKey,
      bool keepTables,
      IServerProgressFeedback feedback);

    string GetFieldDescriptionFromReportFieldCache(string fieldId);

    string CalculateDescription(string name);

    bool IsTimeToSetLoanNumber(LoanData data);

    int GetLoanCount();

    int GetLoanCount(string loanFolder);

    DateTime GetServerCurrentTime();

    DateTime GetLoanCountLastUpdatedTime();

    TimeSpan GetTimeSpanSinceLoanCountLastUpdated();

    void SetLoanCountLastUpdatedTime();

    ArrayList GetAllLenders(string folderName);

    ArrayList GetAllInvestors(string folderName);

    ArrayList GetAllBrokers(string folderName);

    void LoanReassign(
      int index,
      PipelineInfo pipeLine,
      string userID,
      int roleID,
      IServerProgressFeedback feedback);

    PipelineInfo.Alert[] GetLoanAlerts(string guid);

    void UpdateLoanAlerts(string guid, PipelineInfo.Alert[] alertList);

    AuditRecord[] GetAuditRecords(string guid, string columnID);

    AuditRecord[] GetAuditRecords(string guid, string[] columnIDs);

    void SubmitBatch(LoanBatch batch, bool isExternalOrganization);

    StandardFields GetStandardFields();

    bool AddWebCenterImportID(
      string importID,
      string emSiteID,
      string loanGUID,
      DateTime importDateTime,
      string whoImports);

    WebCenterImpotStatus GetWebCenterImportStatus(string loanGUID);

    void DeleteWebCenterImportID(string importID);

    PipelineInfo[] GetLoansByLoanNumbers(List<string> loanNumbers);

    FieldRuleInfo[] GetAdvancedConditionMilestoneTemplates(
      LoanData loan,
      UserInfo userInfo,
      SessionObjects sessionObj);

    string SyncLoanFolder(string guid, bool cleanup);

    SellConditionTrackingSetup GetUpdatedSellConditionSetup();

    FastLoanAccessResponse GetFastLoanAccess(string guid);

    FastLoanLoadResponse FastLoanLoad(FastLoanLoadRequest request);

    LoanFolderInfo.LoanFolderType GetLoanFolderType(string folderName);
  }
}
