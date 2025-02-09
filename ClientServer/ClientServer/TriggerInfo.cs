// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.TriggerInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using Elli.ElliEnum.Triggers;
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
  public class TriggerInfo : BizRuleInfo, IFieldSearchable
  {
    private XmlList<TriggerEvent> events = new XmlList<TriggerEvent>();

    public TriggerInfo(string ruleName)
      : base(ruleName)
    {
    }

    public TriggerInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public TriggerInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      TriggerEvent[] events)
      : base(ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
      this.events.AddRange((IEnumerable<TriggerEvent>) events);
    }

    public TriggerInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      TriggerEvent[] events)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.events.AddRange((IEnumerable<TriggerEvent>) events);
    }

    public TriggerInfo(DataRow r, TriggerEvent[] events)
      : base(r)
    {
      this.events.AddRange((IEnumerable<TriggerEvent>) events);
    }

    public TriggerInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.events = info.GetValue<XmlList<TriggerEvent>>(nameof (Events));
    }

    public FieldSearchField[] GetAdvancedCodeFields(string advCode)
    {
      return ((IEnumerable<string>) FieldReplacementRegex.ParseDependentFields(advCode)).Select<string, FieldSearchField>((System.Func<string, FieldSearchField>) (fieldId => new FieldSearchField(fieldId, RelationshipType.ResultFields))).ToArray<FieldSearchField>();
    }

    public List<TriggerEvent> Events => (List<TriggerEvent>) this.events;

    public override BizRuleType RuleType => BizRuleType.Triggers;

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
      return (object) new TriggerInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.events.ToArray());
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Events", (object) this.events);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      TriggerInfo triggerInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in triggerInfo.GetBaseFields())
        yield return baseField;
      foreach (TriggerEvent evnt in triggerInfo.Events)
      {
        TriggerCondition condition = evnt.Conditions[0];
        if (condition is TriggerFieldCondition)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ConditionOf, ((TriggerFieldCondition) condition).FieldID);
        int index;
        string[] strArray;
        if (evnt.Action.ActionType == TriggerActionType.Assign)
        {
          TriggerAssignment[] triggerAssignmentArray = ((TriggerAssignmentAction) evnt.Action).Assignments;
          for (index = 0; index < triggerAssignmentArray.Length; ++index)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, triggerAssignmentArray[index].FieldID);
          triggerAssignmentArray = (TriggerAssignment[]) null;
        }
        else if (evnt.Action.ActionType == TriggerActionType.Copy)
        {
          strArray = ((TriggerCopyAction) evnt.Action).TargetFieldIDs;
          for (index = 0; index < strArray.Length; ++index)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index]);
          strArray = (string[]) null;
        }
        else if (evnt.Action.ActionType == TriggerActionType.AdvancedCode)
        {
          strArray = FieldReplacementRegex.ParseDependentFields(((TriggerAdvancedCodeAction) evnt.Action).SourceCode);
          for (index = 0; index < strArray.Length; ++index)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index]);
          strArray = (string[]) null;
        }
      }
    }
  }
}
