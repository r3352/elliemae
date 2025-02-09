// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.DDMFeeRuleScenario
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class DDMFeeRuleScenario : BizRuleInfo, IFieldSearchable, IComparable<DDMFeeRuleScenario>
  {
    private XmlList<TriggerEvent> _events = new XmlList<TriggerEvent>();

    public int FeeRuleID { get; set; }

    public int Order { get; set; }

    public override BizRuleType RuleType => BizRuleType.DDMFeeScenarios;

    public bool OrderChanged { get; set; }

    public bool ContentChanged { get; set; }

    public bool Deleted { get; set; }

    public bool Dirty => this.OrderChanged || this.ContentChanged || this.Deleted;

    public string EffectiveDateInfo { get; set; }

    public bool FeeNotAllowed { get; set; }

    public bool EditMode { get; set; }

    public List<DDMFeeRuleValue> FeeRuleValues { get; set; }

    public string ParentRuleName { get; set; }

    public DDMFeeRuleScenario(string ruleName)
      : base(ruleName)
    {
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      TriggerEvent[] events)
      : base(ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this._events.AddRange((IEnumerable<TriggerEvent>) events);
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      string effectiveDateInfo,
      TriggerEvent[] events)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this._events.AddRange((IEnumerable<TriggerEvent>) events);
      this.EffectiveDateInfo = effectiveDateInfo;
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(DataRow r, TriggerEvent[] events)
      : base(r)
    {
      this.Order = Convert.ToInt32(r["sortOrder"]);
      if (r["feeNotAllowed"] != DBNull.Value)
        this.FeeNotAllowed = Convert.ToBoolean(r["feeNotAllowed"]);
      this.FeeRuleID = Convert.ToInt32(r["feeRuleID"]);
      this.EffectiveDateInfo = Convert.ToString(r["effectiveDateInfo"]);
      if (r.Table.Columns.Contains(nameof (ParentRuleName)))
        this.ParentRuleName = Convert.ToString(r[nameof (ParentRuleName)]);
      if (events != null)
        this._events.AddRange((IEnumerable<TriggerEvent>) events);
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(DataRow row)
      : base(row)
    {
      this.Order = Convert.ToInt32(row["sortOrder"]);
      this.FeeRuleID = Convert.ToInt32(row["feeRuleID"]);
      if (row["feeNotAllowed"] != DBNull.Value)
        this.FeeNotAllowed = Convert.ToBoolean(row["feeNotAllowed"]);
      this.EffectiveDateInfo = Convert.ToString(row["effectiveDateInfo"]);
      this.EditMode = true;
    }

    public DDMFeeRuleScenario(XmlSerializationInfo info)
      : base(info)
    {
      this._events = info.GetValue<XmlList<TriggerEvent>>("Events");
      this.EditMode = true;
    }

    public override object Clone()
    {
      return (object) new DDMFeeRuleScenario(0, "Copy of " + this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.EffectiveDateInfo, this._events.ToArray())
      {
        OrderChanged = this.OrderChanged,
        ContentChanged = this.ContentChanged,
        EditMode = this.EditMode,
        Order = this.Order,
        FeeRuleID = this.FeeRuleID,
        FeeRuleValues = this.CopyFeeRuleValueList(),
        FeeNotAllowed = this.FeeNotAllowed
      };
    }

    public DDMFeeRuleScenario CloneExact()
    {
      return new DDMFeeRuleScenario(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.EffectiveDateInfo, this._events.ToArray())
      {
        OrderChanged = this.OrderChanged,
        ContentChanged = this.ContentChanged,
        EditMode = this.EditMode,
        Order = this.Order,
        FeeRuleID = this.FeeRuleID,
        FeeRuleValues = this.FeeRuleValues,
        FeeNotAllowed = this.FeeNotAllowed
      };
    }

    public List<DDMFeeRuleValue> CopyFeeRuleValueList()
    {
      List<DDMFeeRuleValue> ddmFeeRuleValueList = new List<DDMFeeRuleValue>();
      foreach (DDMFeeRuleValue feeRuleValue in this.FeeRuleValues)
        ddmFeeRuleValueList.Add(new DDMFeeRuleValue()
        {
          Field_Name = feeRuleValue.Field_Name,
          Field_Type = feeRuleValue.Field_Type,
          Field_Value = feeRuleValue.Field_Value,
          Field_Value_Type = feeRuleValue.Field_Value_Type,
          FieldID = feeRuleValue.FieldID,
          RuleScenarioID = this.RuleID
        });
      return ddmFeeRuleValueList;
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      DDMFeeRuleScenario ddmFeeRuleScenario = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in ddmFeeRuleScenario.GetBaseFields())
        yield return baseField;
      if (!string.IsNullOrEmpty(ddmFeeRuleScenario.EffectiveDateInfo))
      {
        string str = ddmFeeRuleScenario.EffectiveDateInfo.Split('|')[0];
        if (!string.IsNullOrEmpty(str))
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, str);
      }
      if (ddmFeeRuleScenario.FeeRuleValues != null && ddmFeeRuleScenario.FeeRuleValues.Count > 0)
      {
        foreach (DDMFeeRuleValue fieldValue1 in ddmFeeRuleScenario.FeeRuleValues)
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
                yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, strArray[index]);
              strArray = (string[]) null;
            }
          }
        }
      }
    }

    public int CompareTo(DDMFeeRuleScenario other)
    {
      return other == null ? 1 : this.Order.CompareTo(other.Order);
    }

    public void MarkNotDirty() => this.OrderChanged = this.ContentChanged = this.Deleted = false;
  }
}
