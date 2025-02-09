// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IBpmManager
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.Workflow;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  public interface IBpmManager
  {
    BizRuleInfo CreateNewRule(BizRuleInfo rule, bool isGlobalSetting = false);

    BizRuleInfo[] GetRules();

    BizRuleInfo[] GetRules(bool activeOnly);

    BizRuleInfo[] GetRules(BizRule.Condition condition, bool activeOnly);

    BizRuleInfo[] GetRules(BizRule.Condition[] conditions, bool activeOnly);

    BizRuleInfo[] GetRules(BizRuleType ruleType);

    BizRuleInfo[] GetRules(BizRuleType ruleType, bool activeOnly, bool forceToPrimaryDb = false);

    BizRuleInfo[] GetRules(BizRuleType ruleType, BizRule.Condition condition, bool activeOnly);

    BizRuleInfo[] GetRules(
      BizRuleType ruleType,
      BizRule.Condition[] conditions,
      bool activeOnly,
      bool forceToPrimaryDb = false);

    BizRuleInfo[] GetRules(BizRuleType[] ruleTypes);

    BizRuleInfo[] GetRules(BizRuleType[] ruleTypes, bool activeOnly);

    BizRuleInfo[] GetRules(BizRuleType[] ruleTypes, BizRule.Condition condition, bool activeOnly);

    BizRuleInfo[] GetRules(
      BizRuleType[] ruleTypes,
      BizRule.Condition[] conditions,
      bool activeOnly);

    BizRuleInfo GetRule(BizRuleType ruleType, int ruleID);

    void InvalidateCache(BizRuleType ruleType);

    void UpdateRule(BizRuleInfo rule, bool isGlobalSetting = false);

    void DeleteRule(BizRuleType ruleType, int ruleID, bool isGlobalSetting = false, bool forceToPrimaryDb = false);

    DateTime GetLastRuleModificationTime();

    DateTime GetLastRuleModificationTime(BizRuleType ruleType);

    FieldAccessRuleInfo[] GetActiveFieldAccessRulesByPersonas(int[] personaIds);

    BizRule.ActivationReturnCode ActivateRule(
      BizRuleType ruleType,
      int ruleID,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false);

    BizRule.ActivationReturnCode DeactivateRule(
      BizRuleType ruleType,
      int ruleID,
      bool isGlobalSetting = false,
      bool forceToPrimaryDb = false);

    BizRuleInfo GetActiveRule(
      BizRuleType ruleType,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2);

    FieldRuleFieldIDAndType[] GetInconsistentRuleFields(int ruleId);

    DocumentDefaultAccessRuleInfo[] GetDocumentDefaultAccessRules();

    DocumentDefaultAccessRuleInfo GetDocumentDefaultAccessRule(int roleAddedBy);

    void SaveDocumentDefaultAccessRules(DocumentDefaultAccessRuleInfo[] rules);

    void SetLoanFolderAccessRule(LoanFolderRuleInfo rule);

    LoanFolderRuleInfo[] GetLoanFolderAccessRules();

    LoanFolderRuleInfo GetLoanFolderAccessRule(string loanFolder);

    void DeleteLoanFolderAccessRule(string loanFolder);

    string[] GetAllowedOriginationLoanFolders();

    void SetMsWorksheetInfo(WorksheetInfo worksheetInfo);

    void UpdateMsWorksheetAlertMessages(Dictionary<string, string> alertMsgsToUpdate);

    void SetOrUpdateMsWorksheetAlertMessages(
      Dictionary<WorksheetInfo, string> alertMsgsToUpdate);

    WorksheetInfo[] GetMsWorksheetInfos();

    WorksheetInfo GetMsWorksheetInfo(int coreMilestoneID);

    WorksheetInfo GetMsWorksheetInfo(string milestoneID);

    string[] GetMilestoneIDsByRoleID(int roleID);

    IEnumerable<EllieMae.EMLite.Workflow.Milestone> GetMilestones(bool activeOnly);

    EllieMae.EMLite.Workflow.Milestone GetMilestone(string milestoneId);

    EllieMae.EMLite.Workflow.Milestone GetMilestoneByName(string milestoneName);

    void CreateMilestone(EllieMae.EMLite.Workflow.Milestone ms);

    void UpdateMilestone(EllieMae.EMLite.Workflow.Milestone ms);

    void SetMilestoneArchiveFlag(string milestoneId, bool archived);

    void DeleteMilestone(string milestoneId);

    void SetMilestoneOrder(string[] milestoneIds);

    void ChangeMilestoneSortIndex(string milestoneId, int sortIndex);

    void ChangeMilestoneSortIndex(EllieMae.EMLite.Workflow.Milestone OldMilestone, EllieMae.EMLite.Workflow.Milestone NewMilestone);

    IEnumerable<MilestoneTemplate> GetMilestoneTemplates(bool activeOnly);

    MilestoneTemplate GetMilestoneTemplate(string templateId);

    MilestoneTemplate GetMilestoneTemplateByGuid(string templateGuid);

    List<FieldRuleInfo> GetMilestoneTemplateConditions();

    MilestoneTemplate GetMilestoneTemplateByName(string templateName);

    MilestoneTemplate GetDefaultMilestoneTemplate();

    string GetMilestoneTemplatebyMilestoneID(string milestoneId);

    void CreateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule);

    void UpdateMilestoneTemplate(MilestoneTemplate template, BizRuleInfo rule);

    void SetMilestoneTemplateActiveFlag(string templateId, bool active);

    void DeleteMilestoneTemplate(string templateId);

    void SetMilestoneTemplateOrder(string[] templateIds);

    void ChangeMilestoneTemplateSortIndex(string templateId, int sortIndex);

    void ChangeMilestoneTemplateSortIndex(
      MilestoneTemplate OldTemplate,
      MilestoneTemplate NewTemplate);

    FieldRuleInfo GetMilestoneTemplateConditions(string templateId);

    IEnumerable<MilestoneTemplate> UpdateMilestoneTemplateImpactedAreaSettings(
      Dictionary<MilestoneTemplate, string> newSettings,
      string impactedArea);

    void UpdateMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, Dictionary<MilestoneTemplate, string>> exceptionsList,
      bool remove);

    void UpdateMilestoneTemplateEDisclosureExceptions(EDisclosureSetup eDisclosureSetup);

    void RemoveMilestoneTemplateEDisclosureExceptions(
      Dictionary<string, List<string>> exceptionsList);

    Dictionary<string, List<string>> GetAllMilestoneTemplateEDisclosureExceptions();

    Hashtable GetMilestoneTemplateDefaultSettings();

    RoleInfo[] GetAllRoleFunctions();

    RoleInfo GetRoleFunction(int roleID);

    RoleInfo[] GetRoleFunctionsByUserID(string userID);

    RoleSummaryInfo[] GetUserEligibleRoles(string userID);

    RoleInfo[] GetAllRoleFunctionsByPersonaID(int personaID);

    int SetRoleFunction(RoleInfo roleInfo);

    void DeleteRoleFunction(int roleID);

    RolesMappingInfo[] GetAllRoleMappingInfos();

    RolesMappingInfo GetRoleMappingInfo(RealWorldRoleID realWorldRoleID);

    void UpdateRoleMappingInfos(RolesMappingInfo[] rolesMappingInfos);

    RolesMappingInfo[] GetUsersRoleMapping(string userID);

    RolesMappingInfo GetUsersRoleMapping(string userID, RealWorldRoleID realWorldRoleID);

    Hashtable GetAllMilestoneAlertMessages();

    void UpdateMilestoneCache();

    void CreateFieldSearchInfo();

    List<int> UpdateFieldSearchInfo(List<FieldSearchRuleType> fsRuleTypes, bool forceToPrimaryDb = false);

    List<int> UpdateFieldSearchInfo(bool forceToPrimaryDb = false);

    List<SearchableRuleType> GetSearchableRuleTypes();

    FieldSearchRuleType GetRuleType(int fsRuleId);

    int GetRuleId(int fsRuleId);

    string GetRuleIdentifier(int fsRuleId);

    string[] GetMilestoneNamesByRuleIds(int[] ruleIDs);

    int CreateDDMFeeRule(DDMFeeRule ddmFeeRule, bool forceToPrimaryDb = false);

    int CreateDDMFieldRule(DDMFieldRule DDMfieldrule, bool forceToPrimaryDb = false);

    int UpdateDDMFieldRule(
      DDMFieldRule DDMfieldrule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0);

    DDMFeeRule[] GetAllDDMFeeRules(bool forceToPrimaryDb = false);

    DDMFeeRule[] GetAllDDMFeeRulesAndScenarios(bool activeOnly, bool forceToPrimaryDb = false);

    DDMFeeRule GetDDMFeeRuleAndScenarioByID(int ruleID, bool forceToPrimaryDb = false);

    DDMFeeRule[] GetDDMFeeRuleByDataTable(string dataTableName, bool forceToPrimaryDb = false);

    void UpdateDDMFeeRuleByID(
      int ruleID,
      DDMFeeRule feeRule,
      bool statusUpdate = false,
      bool isGlobalSetting = false,
      int activeDeActiveScenarioId = 0);

    DDMFieldRule[] GetAllDDMFieldRules(bool forceToPrimaryDb = false);

    DDMFieldRule[] GetAllDDMFieldRulesAndScenarios(
      bool activeOnly,
      List<int> fieldRuleIds = null,
      bool forceToPrimaryDb = false);

    Dictionary<int, HashSet<string>> GetAllDdmFieldRulesAndFieldsList(bool forceToPrimaryDb = false);

    bool DDMFeeRuleExist(string ruleName, bool forceToPrimaryDb = false);

    bool DDMFeeLineExist(string feeLine, bool forceToPrimaryDb = false);

    void DeleteDDMFeeRuleByID(int ruleID, bool forceToPrimaryDb = false);

    bool DDMFieldRuleExist(string ruleName, bool forceToPrimaryDb = false);

    bool DDMFieldsExistInFieldRules(string fields, bool forceToPrimaryDb = false);

    void DeleteDDMFieldRuleByID(int ruleID, bool forceToPrimaryDb = false);

    DDMFieldRule[] GetDDMFieldRuleByDataTable(string dataTableName, bool forceToPrimaryDb = false);

    void UpdateDDMDataPopulationTiming(
      DDMDataPopulationTiming ddmDataPopTiming,
      bool forceToPrimaryDb = false);

    DDMDataPopulationTiming GetDDMDataPopulationTiming(bool forceToPrimaryDb = false);

    List<DDMDataPopTimingField> UpdateDDMDataPopulationTimingNumberOfReferences();

    int GetNumberReferences(string fieldId, bool forceToPrimaryDb = false);

    int CreateDDMDataTable(DDMDataTable ddmDataTable, bool importMode = false, bool forceToPrimaryDb = false);

    int UpdateDDMDataTable(DDMDataTable ddmDataTable, bool forceToPrimaryDb = false);

    void DeleteDDMDataTable(DDMDataTable ddmDataTable);

    DDMDataTable[] GetAllDDMDataTables(bool forceToPrimaryDb = false);

    DDMDataTable[] GetAllDDMDataTablesWithFieldValues(bool forceToPrimaryDb = false);

    DDMDataTable[] GetAllReferencedDDMDataTablesWithFieldValues(bool forceToPrimaryDb = false);

    DDMDataTable[] GetDDMDataTablesAndFieldValuesForDataTableNames(
      List<string> dataTableNames,
      bool forceToPrimaryDb = false);

    DDMDataTable GetDDMDataTableAndFieldIdsForDataTableName(
      string dataTableName,
      bool forceToPrimaryDb = false);

    void AddFieldValuesForDataTable(
      DDMDataTable dataTable,
      Dictionary<string, string> fieldValues,
      bool forceToPrimaryDb = false);

    DDMDataTable GetDDMDataTableAndFieldValues(int dataTableId, bool forceToPrimaryDb = false);

    DDMDataTable GetDDMDataTableAndFieldValuesByName(string dataTableName, bool forceToPrimaryDb = false);

    bool DDMDataTableExists(string tablename, bool forceToPrimaryDb = false);

    List<DDMDataTableReference> GetDDMDataTableReferences(string dataTable, bool forceToPrimaryDb = false);

    int CreateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField);

    int UpdateDDMDataTableField(DDMDataTableFieldValue ddmDataTableField);

    void DeleteDDMDataTableField(DDMDataTableFieldValue ddmDataTableField);

    bool IsTableUsedByFeeOrFieldRules(DDMDataTable ddmDataTable, bool forceToPrimaryDb = false);

    void ResetDataTableFeeRuleFieldRuleValue(DDMDataTable ddmDataTable);

    DDMDataTableExportLog GetDataTableExportLog(string dataTableExportLogID, bool forceToPrimaryDb = false);

    void SaveDataTableExportLog(DDMDataTableExportLog dataTableExportLog);

    int[] AtomicDataTableChange(
      List<DDMDataTableFieldValue> batchCreationList,
      List<DDMDataTableFieldValue> batchUpdateList,
      List<DDMDataTableFieldValue> batchDeletionList);

    List<TemporaryBuydown> GetAllTemporaryBuydowns();

    void CreateTemporaryBuydownType(TemporaryBuydown buydown);

    void UpdateTemporaryBuydownType(TemporaryBuydown buydown);

    void DeleteTemporaryBuydownType(TemporaryBuydown buydown);
  }
}
