// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AutomatedConditionRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.DataEngine.eFolder;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AutomatedConditionRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public AutomatedCondition[] Conditions;

    public AutomatedConditionRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public AutomatedConditionRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public AutomatedConditionRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      AutomatedCondition[] conditions)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, conditions)
    {
    }

    public AutomatedConditionRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      AutomatedCondition[] conditions)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.Conditions = conditions;
    }

    public AutomatedConditionRuleInfo(DataRow ruleInfo, DataRow[] conditions)
      : base(ruleInfo)
    {
      if (conditions == null)
        return;
      string empty = string.Empty;
      this.Conditions = new AutomatedCondition[conditions.Length];
      for (int index = 0; index < conditions.Length; ++index)
      {
        ConditionType conditionType = (ConditionType) Enum.Parse(typeof (ConditionType), conditions[index]["conditionType"].ToString(), true);
        string conditionName = conditions[index]["conditionName"].ToString();
        this.Conditions[index] = new AutomatedCondition(conditionType, conditionName);
      }
    }

    public AutomatedConditionRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      XmlList<AutomatedCondition> xmlList = info.GetValue<XmlList<AutomatedCondition>>(nameof (Conditions));
      this.Conditions = new AutomatedCondition[xmlList.Count];
      for (int index = 0; index < xmlList.Count; ++index)
        this.Conditions[index] = xmlList[index];
    }

    public override BizRuleType RuleType => BizRuleType.AutomatedConditions;

    public override object Clone()
    {
      return (object) new AutomatedConditionRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.Conditions);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      XmlList<AutomatedCondition> xmlList = new XmlList<AutomatedCondition>();
      foreach (AutomatedCondition condition in this.Conditions)
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
