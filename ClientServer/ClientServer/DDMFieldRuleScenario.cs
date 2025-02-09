// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMFieldRuleScenario
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMFieldRuleScenario : 
    BizRuleInfo,
    IFieldSearchable,
    IComparable<DDMFieldRuleScenario>
  {
    public int FieldRuleID { get; set; }

    public int Order { get; set; }

    public override BizRuleType RuleType => BizRuleType.DDMFieldScenarios;

    public bool OrderChanged { get; set; }

    public bool ContentChanged { get; set; }

    public bool Deleted { get; set; }

    public bool Dirty => this.OrderChanged || this.ContentChanged || this.Deleted;

    public string EffectiveDateInfo { get; set; }

    public bool FeeNotAllowed { get; set; }

    public bool EditMode { get; set; }

    public List<DDMFeeRuleValue> FieldRuleValues { get; set; }

    public string ParentRuleName { get; set; }

    public DDMFieldRuleScenario(string ruleName)
      : base(ruleName)
    {
      this.EditMode = true;
    }

    public DDMFieldRuleScenario(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
      this.EditMode = true;
    }

    public DDMFieldRuleScenario(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml)
      : base(ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this.EditMode = true;
    }

    public DDMFieldRuleScenario(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      string effectiveDateInfo)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.EffectiveDateInfo = effectiveDateInfo;
      this.EditMode = true;
    }

    public DDMFieldRuleScenario(DataRow row)
      : base(row)
    {
      if (row["feeNotAllowed"] != DBNull.Value)
        this.FeeNotAllowed = Convert.ToBoolean(row["feeNotAllowed"]);
      this.Order = Convert.ToInt32(row["sortOrder"]);
      this.FieldRuleID = Convert.ToInt32(row["fieldRuleID"]);
      this.EffectiveDateInfo = Convert.ToString(row["effectiveDateInfo"]);
      if (row.Table.Columns.Contains(nameof (ParentRuleName)))
        this.ParentRuleName = Convert.ToString(row[nameof (ParentRuleName)]);
      this.EditMode = true;
    }

    public override object Clone()
    {
      return (object) new DDMFieldRuleScenario(0, "Copy of " + this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.EffectiveDateInfo)
      {
        OrderChanged = this.OrderChanged,
        ContentChanged = this.ContentChanged,
        EditMode = this.EditMode,
        Order = this.Order,
        FieldRuleID = this.FieldRuleID,
        FieldRuleValues = this.CopyFeeRuleValueList()
      };
    }

    public DDMFieldRuleScenario CloneForDuplicate(int FieldRuleID)
    {
      return new DDMFieldRuleScenario(0, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.EffectiveDateInfo)
      {
        OrderChanged = this.OrderChanged,
        ContentChanged = true,
        EditMode = this.EditMode,
        Order = this.Order,
        FieldRuleID = FieldRuleID,
        FieldRuleValues = this.CopyFeeRuleValueList()
      };
    }

    public DDMFieldRuleScenario CloneExact()
    {
      return new DDMFieldRuleScenario(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.EffectiveDateInfo)
      {
        OrderChanged = this.OrderChanged,
        ContentChanged = this.ContentChanged,
        EditMode = this.EditMode,
        Order = this.Order,
        FieldRuleID = this.FieldRuleID,
        FieldRuleValues = this.FieldRuleValues
      };
    }

    public List<DDMFeeRuleValue> CopyFeeRuleValueList()
    {
      List<DDMFeeRuleValue> ddmFeeRuleValueList = new List<DDMFeeRuleValue>();
      foreach (DDMFeeRuleValue fieldRuleValue in this.FieldRuleValues)
        ddmFeeRuleValueList.Add(new DDMFeeRuleValue()
        {
          Field_Name = fieldRuleValue.Field_Name,
          Field_Type = fieldRuleValue.Field_Type,
          Field_Value = fieldRuleValue.Field_Value,
          Field_Value_Type = fieldRuleValue.Field_Value_Type,
          FieldID = fieldRuleValue.FieldID,
          RuleScenarioID = this.RuleID
        });
      return ddmFeeRuleValueList;
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      DDMFieldRuleScenario fieldRuleScenario = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in fieldRuleScenario.GetBaseFields())
        yield return baseField;
      if (!string.IsNullOrEmpty(fieldRuleScenario.EffectiveDateInfo))
      {
        string str = fieldRuleScenario.EffectiveDateInfo.Split('|')[0];
        if (!string.IsNullOrEmpty(str))
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, str);
      }
      if (fieldRuleScenario.FieldRuleValues != null && fieldRuleScenario.FieldRuleValues.Count > 0)
      {
        foreach (DDMFeeRuleValue fieldValue1 in fieldRuleScenario.FieldRuleValues)
        {
          if (fieldValue1.Field_Value_Type != fieldValueType.ValueNotSet)
          {
            if (fieldValue1.Field_Value_Type == fieldValueType.Table)
            {
              string fieldValue2 = fieldValue1.Field_Value;
              char[] chArray = new char[1]{ '|' };
              yield return new KeyValuePair<RelationshipType, string>(RelationshipType.RefersRule, fieldValue1.FieldID + "|DDMDataTables|" + fieldValue2.Split(chArray)[1]);
            }
            else
              yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, fieldValue1.FieldID);
            if (fieldValue1.Field_Value_Type == fieldValueType.Calculation)
            {
              string[] strArray = FieldReplacementRegex.ParseDependentFields(fieldValue1.Field_Value);
              for (int index = 0; index < strArray.Length; ++index)
                yield return new KeyValuePair<RelationshipType, string>(RelationshipType.AffectsValueOf, strArray[index]);
              strArray = (string[]) null;
            }
          }
        }
      }
    }

    public int CompareTo(DDMFieldRuleScenario other)
    {
      return other == null ? 1 : this.Order.CompareTo(other.Order);
    }

    public void MarkNotDirty() => this.OrderChanged = this.ContentChanged = this.Deleted = false;
  }
}
