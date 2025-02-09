// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.AutoLockExclusionRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer.Bpm;
using EllieMae.EMLite.ClientServer.Classes;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.FieldSearch;
using EllieMae.EMLite.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class AutoLockExclusionRuleInfo : BizRuleInfo, IFieldSearchable
  {
    private XmlList<TriggerEvent> events = new XmlList<TriggerEvent>();

    public AutoLockExclusionRuleInfo(string ruleName)
      : base(ruleName)
    {
      this.Condition = BizRule.Condition.AdvancedCoding;
    }

    public AutoLockExclusionRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
      this.Condition = BizRule.Condition.AdvancedCoding;
    }

    public AutoLockExclusionRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this.Condition = BizRule.Condition.AdvancedCoding;
    }

    public AutoLockExclusionRuleInfo(DataRow dataRow)
      : base(dataRow)
    {
      this.Condition = BizRule.Condition.AdvancedCoding;
    }

    public FieldSearchField[] GetAdvancedCodeFields(string advCode)
    {
      return ((IEnumerable<string>) FieldReplacementRegex.ParseDependentFields(advCode)).Select<string, FieldSearchField>((System.Func<string, FieldSearchField>) (fieldId => new FieldSearchField(fieldId, RelationshipType.ResultFields))).ToArray<FieldSearchField>();
    }

    public override BizRuleType RuleType => BizRuleType.AutoLockExclusionRules;

    public string[] GetActivationFields(ConfigInfoForTriggers activationData)
    {
      List<string> stringList = new List<string>();
      foreach (TriggerEvent triggerEvent in (List<TriggerEvent>) this.events)
      {
        foreach (string activationField in triggerEvent.GetActivationFields(activationData))
        {
          if (!stringList.Contains(activationField))
            stringList.Add(activationField);
        }
      }
      return stringList.ToArray();
    }

    public override object Clone()
    {
      return (object) new AutoLockExclusionRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info) => base.GetXmlObjectData(info);

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (KeyValuePair<RelationshipType, string> baseField in this.GetBaseFields())
        yield return baseField;
    }
  }
}
