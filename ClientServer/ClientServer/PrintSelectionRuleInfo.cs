// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PrintSelectionRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

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
  public class PrintSelectionRuleInfo : BizRuleInfo, IFieldSearchable
  {
    private XmlList<PrintSelectionEvent> events = new XmlList<PrintSelectionEvent>();

    public PrintSelectionRuleInfo(string ruleName)
      : base(ruleName)
    {
    }

    public PrintSelectionRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public PrintSelectionRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      PrintSelectionEvent[] events)
      : base(ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml)
    {
    }

    public PrintSelectionRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      PrintSelectionEvent[] events)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.events.AddRange((IEnumerable<PrintSelectionEvent>) events);
    }

    public PrintSelectionRuleInfo(DataRow r, PrintSelectionEvent[] events)
      : base(r)
    {
      this.events.AddRange((IEnumerable<PrintSelectionEvent>) events);
    }

    public PrintSelectionRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.events = info.GetValue<XmlList<PrintSelectionEvent>>(nameof (Events));
    }

    public List<PrintSelectionEvent> Events => (List<PrintSelectionEvent>) this.events;

    public override BizRuleType RuleType => BizRuleType.PrintSelection;

    public string[] GetActivationFields()
    {
      List<string> stringList = new List<string>();
      foreach (PrintSelectionEvent printSelectionEvent in (List<PrintSelectionEvent>) this.events)
      {
        foreach (PrintSelectionCondition condition in printSelectionEvent.Conditions)
        {
          if (!stringList.Contains(condition.FieldID))
            stringList.Add(condition.FieldID);
        }
      }
      return stringList.ToArray();
    }

    public override object Clone()
    {
      return (object) new PrintSelectionRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.events.ToArray());
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Events", (object) this.events);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      PrintSelectionRuleInfo selectionRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in selectionRuleInfo.GetBaseFields())
        yield return baseField;
      foreach (PrintSelectionCondition selectionCondition in selectionRuleInfo.events.Where<PrintSelectionEvent>((System.Func<PrintSelectionEvent, bool>) (printSelectionEvent => printSelectionEvent != null)).SelectMany((System.Func<PrintSelectionEvent, IEnumerable<PrintSelectionCondition>>) (printSelectionEvent => (IEnumerable<PrintSelectionCondition>) printSelectionEvent.Conditions), (printSelectionEvent, printSelectionCondition) => new
      {
        printSelectionEvent = printSelectionEvent,
        printSelectionCondition = printSelectionCondition
      }).Where(_param1 => _param1.printSelectionCondition != null).Select(_param1 => _param1.printSelectionCondition))
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, selectionCondition.FieldID);
    }
  }
}
