// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.FieldSearchRule
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Contacts;
using EllieMae.EMLite.ClientServer.CustomFields;
using EllieMae.EMLite.ClientServer.HtmlEmail;
using EllieMae.EMLite.ClientServer.StatusOnline;
using EllieMae.EMLite.DataEngine;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  [Serializable]
  public class FieldSearchRule
  {
    public const string FS_RULE_ID = "FsRuleId�";
    public const string RULE_ID = "RuleId�";
    public const string RULE_TYPE = "RuleType�";
    public const string RULE_NAME = "RuleName�";
    public const string RULE_STATUS = "Status�";
    public const string FIELD_ID = "FieldId�";
    public const string RELATIONSHPI_TYPE = "RelationshipType�";
    public const string IDENTIFIER = "Identifier�";
    public const string PARENT_RULEID = "ParentFSRuleId�";

    public int FsRuleId { get; set; }

    public int RuleId { get; set; }

    public FieldSearchRuleType RuleType { get; set; }

    public BizRule.RuleStatus Status { get; set; }

    public string RuleName { get; set; }

    [CLSCompliant(false)]
    public string Identifier { get; set; }

    public int? ParentFSRuleId { get; set; }

    public List<FieldSearchField> FieldSearchFields { get; set; }

    public static bool IsValidSortFsRuleColumn(string colName)
    {
      return string.Compare(colName, "RuleType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "RuleName", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "Status", StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsValidFilterFsRuleColumn(string colName)
    {
      return string.Compare(colName, "RuleType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "Status", StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static bool IsValidFsRuleColumn(string colName)
    {
      return string.Compare(colName, "FsRuleId", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "RuleId", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "RuleType", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "RuleName", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "Status", StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static string GetValidSortFsRuleColumns() => "RuleType,RuleName,Status";

    public static string GetValidFilterFsRuleColumns() => "RuleType,Status";

    public static string GetValidFsRuleColumns() => "FsRuleId,RuleId,RuleType,RuleName,Status";

    public static bool IsValidFsFieldColumn(string colName)
    {
      return string.Compare(colName, "FsRuleId", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "FieldId", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(colName, "RelationshipType", StringComparison.OrdinalIgnoreCase) == 0;
    }

    public static string GetValidFsFieldColumns() => "FsRuleId,FieldId,RelationshipType";

    private FieldSearchRule(
      IFieldSearchable rule,
      List<KeyValuePair<RelationshipType, string>> additionalFields = null)
    {
      if (rule == null)
        throw new NotSupportedException("bizRuleInfo does not implement IFieldSearchable.");
      this.FieldSearchFields = new List<FieldSearchField>();
      this.InitFieldSearchFields(rule, additionalFields);
    }

    public FieldSearchRule(DataRow r)
    {
      this.FsRuleId = Convert.ToInt32(r[nameof (FsRuleId)]);
      this.RuleId = Convert.ToInt32(r[nameof (RuleId)]);
      this.RuleType = (FieldSearchRuleType) r[nameof (RuleType)];
      this.Status = (BizRule.RuleStatus) Convert.ToInt16(r[nameof (Status)]);
      this.RuleName = r[nameof (RuleName)].ToString();
      if (r.Table.Columns.Contains(nameof (Identifier)) && !r.IsNull(nameof (Identifier)))
        this.Identifier = Convert.ToString(r[nameof (Identifier)]);
      if (r.Table.Columns.Contains(nameof (ParentFSRuleId)) && !r.IsNull(nameof (ParentFSRuleId)))
        this.ParentFSRuleId = new int?(Convert.ToInt32(r[nameof (ParentFSRuleId)]));
      this.FieldSearchFields = new List<FieldSearchField>();
    }

    public FieldSearchRule(PiggybackFields ruleFields)
      : this((IFieldSearchable) ruleFields)
    {
      this.RuleType = FieldSearchRuleType.PiggyBackingFields;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleName = FieldSearchUtil.Type2Name(this.RuleType);
    }

    public FieldSearchRule(StatusOnlineTrigger fields)
      : this((IFieldSearchable) fields)
    {
      this.RuleType = FieldSearchRuleType.CompanyStatusOnline;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleName = fields.Name;
      this.Identifier = fields.Guid;
    }

    public FieldSearchRule(CustomFieldInfo loanCustomField)
      : this((IFieldSearchable) loanCustomField)
    {
      this.RuleType = FieldSearchRuleType.LoanCustomFields;
      this.Status = BizRule.RuleStatus.Active;
      this.Identifier = this.RuleName = loanCustomField.FieldID;
    }

    public FieldSearchRule(ContactCustomFieldInfoCollection fields, FieldSearchRuleType type)
      : this((IFieldSearchable) fields)
    {
      this.RuleType = type;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleName = FieldSearchUtil.Type2Name(type);
    }

    public FieldSearchRule(CustomFieldsDefinitionInfo fields)
      : this((IFieldSearchable) fields)
    {
      this.RuleType = FieldSearchRuleType.BusinessCustomFields;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleName = FieldSearchUtil.Type2Name(this.RuleType);
    }

    public FieldSearchRule(AlertConfig fields, int id)
      : this((IFieldSearchable) fields)
    {
      this.RuleType = FieldSearchRuleType.Alerts;
      this.RuleName = fields.Definition.Name;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleId = id;
    }

    public FieldSearchRule(LRAdditionalFields fields)
      : this((IFieldSearchable) fields)
    {
      this.RuleType = FieldSearchRuleType.LockRequestAdditionalFields;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleName = FieldSearchUtil.Type2Name(this.RuleType);
    }

    public FieldSearchRule(
      BizRuleInfo bizRuleInfo,
      List<KeyValuePair<RelationshipType, string>> additionalFields = null)
      : this(bizRuleInfo as IFieldSearchable, additionalFields)
    {
      this.RuleId = bizRuleInfo.RuleID;
      FieldSearchRuleType result;
      if (!Enum.TryParse<FieldSearchRuleType>(bizRuleInfo.RuleType.ToString(), true, out result))
        result = FieldSearchRuleType.None;
      this.RuleType = result;
      this.Status = bizRuleInfo.Status;
      if (result == FieldSearchRuleType.DDMFieldScenarios)
        this.RuleName = ((DDMFieldRuleScenario) bizRuleInfo).ParentRuleName;
      else if (result == FieldSearchRuleType.DDMFeeScenarios)
        this.RuleName = ((DDMFeeRuleScenario) bizRuleInfo).ParentRuleName;
      else
        this.RuleName = bizRuleInfo.RuleName;
    }

    public FieldSearchRule(HtmlEmailTemplate fields)
      : this((IFieldSearchable) fields)
    {
      this.Identifier = fields.Guid;
      this.RuleName = fields.Subject;
      this.Status = BizRule.RuleStatus.Active;
      this.RuleType = FieldSearchRuleType.HtmlEmailTemplate;
    }

    private void InitFieldSearchFields(
      IFieldSearchable rule,
      List<KeyValuePair<RelationshipType, string>> additionalFields)
    {
      foreach (KeyValuePair<RelationshipType, string> field in rule.GetFields())
        this.AddNewFieldSearchFieldInfo(field.Value, field.Key);
      if (additionalFields == null)
        return;
      foreach (KeyValuePair<RelationshipType, string> additionalField in additionalFields)
        this.AddNewFieldSearchFieldInfo(additionalField.Value, additionalField.Key);
    }

    private void AddNewFieldSearchFieldInfo(string brFieldId, RelationshipType relationshipType)
    {
      if (this.FieldSearchFields.FindIndex((Predicate<FieldSearchField>) (x => x.FieldId == brFieldId && x.RelationshipType == relationshipType)) != -1)
        return;
      this.FieldSearchFields.Add(new FieldSearchField(brFieldId, relationshipType));
    }

    public Dictionary<string, FieldSearchField> IndexFSFields()
    {
      Dictionary<string, FieldSearchField> dictionary = new Dictionary<string, FieldSearchField>();
      foreach (FieldSearchField fieldSearchField in this.FieldSearchFields)
      {
        if (!dictionary.ContainsKey(fieldSearchField.FieldId))
          dictionary[fieldSearchField.FieldId] = fieldSearchField;
      }
      return dictionary;
    }
  }
}
