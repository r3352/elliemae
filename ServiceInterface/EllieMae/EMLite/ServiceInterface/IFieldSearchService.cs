// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ServiceInterface.IFieldSearchService
// Assembly: ServiceInterface, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF0C2B89-A027-4FA0-9669-4D2AA36A4D74
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ServiceInterface.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ServiceInterface
{
  public interface IFieldSearchService : IContextBoundObject
  {
    IEnumerable<FieldSearchRule> LookupRules(
      List<string> fieldIds,
      int pageSize,
      int pageNumber,
      List<SortColumn> sortColumnList,
      List<FilterColumn> filterColumnList,
      out int totalRules);

    FieldSearchRule GetFSRule(int FSRuleId);

    BizRuleInfo GetBizRuleInfo(BizRuleType ruleType, int ruleId);

    string GetRuleDetailString(BizRuleInfo bizRuleInfo);

    string BuildConditionString(BizRuleInfo bizInfo);

    string PersonaIDToName(int personaId, bool initCache = false);

    int NameToPersonaID(string name, bool initCache = false);

    string RoleIDToName(int roleId);

    int NameToRoleID(string name);

    string MilestoneIDToName(string msid);

    string NameToMilestoneID(string name);

    string DocumentIDToName(string docId);

    string NameToDocumentID(string name);

    string TaskIDToName(string taskId);

    string NameToTaskID(string name);

    string GetFieldDescription(string fieldId, EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown);

    string GetFieldFormat(string fieldId, EllieMae.EMLite.FieldSearch.FieldType fieldType = EllieMae.EMLite.FieldSearch.FieldType.Unknown);

    CustomFieldInfo GetStandardLoanCustomField(int index);

    CustomFieldsInfo GetAllLoanCustomFields();

    AlertConfig GetAlert(int alertId);

    ContactCustomFieldInfoCollection GetBorrowerCustomFields();

    ContactCustomFieldInfoCollection GetBusinessCustomFields();

    ContactCustomFieldInfoCollection GetTPOCustomFields();

    BizCategory[] GetBizCategories();

    CustomFieldsDefinitionInfo[] GetCustomFieldsDefinitions(CustomFieldsType customFieldsType);

    LRAdditionalFields GetLRAdditionalFields();

    HtmlEmailTemplate GetHtmlEmailTemplate(string guid);

    StatusOnlineSetup GetStatusOnlineSetup();

    Persona[] GetAllPersonas(bool excludeSuperAdmin);

    InputFormInfo[] GetFormInfos(InputFormCategory category);
  }
}
