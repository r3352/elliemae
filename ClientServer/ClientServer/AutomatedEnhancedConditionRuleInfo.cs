// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AutomatedEnhancedConditionRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AutomatedEnhancedConditionRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public AutomatedEnhancedCondition[] Conditions;

    public AutomatedEnhancedConditionRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public AutomatedEnhancedConditionRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public AutomatedEnhancedConditionRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      AutomatedEnhancedCondition[] conditions)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, conditions)
    {
    }

    public AutomatedEnhancedConditionRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      AutomatedEnhancedCondition[] conditions)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.Conditions = conditions;
    }

    public AutomatedEnhancedConditionRuleInfo(DataRow ruleInfo, DataRow[] conditions)
      : base(ruleInfo)
    {
      if (conditions == null)
        return;
      string empty1 = string.Empty;
      string empty2 = string.Empty;
      this.Conditions = new AutomatedEnhancedCondition[conditions.Length];
      for (int index = 0; index < conditions.Length; ++index)
      {
        string conditionType = Convert.ToString(conditions[index]["conditionType"]);
        string conditionName = Convert.ToString(conditions[index]["conditionName"]);
        this.Conditions[index] = new AutomatedEnhancedCondition(conditionType, conditionName);
      }
    }

    public AutomatedEnhancedConditionRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      XmlList<AutomatedEnhancedCondition> xmlList = info.GetValue<XmlList<AutomatedEnhancedCondition>>(nameof (Conditions));
      this.Conditions = new AutomatedEnhancedCondition[xmlList.Count];
      for (int index = 0; index < xmlList.Count; ++index)
        this.Conditions[index] = xmlList[index];
    }

    public override BizRuleType RuleType => BizRuleType.AutomatedEnhancedConditions;

    public override object Clone()
    {
      return (object) new AutomatedEnhancedConditionRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.Conditions);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      XmlList<AutomatedEnhancedCondition> xmlList = new XmlList<AutomatedEnhancedCondition>();
      foreach (AutomatedEnhancedCondition condition in this.Conditions)
        xmlList.Add(condition);
      info.AddValue("Conditions", (object) xmlList);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (KeyValuePair<RelationshipType, string> baseField in this.GetBaseFields())
        yield return baseField;
    }
  }
}
