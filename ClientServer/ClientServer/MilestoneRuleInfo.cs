// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.MilestoneRuleInfo
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

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
  public class MilestoneRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public DocMilestonePair[] Docs;
    public TaskMilestonePair[] Tasks;
    public FieldMilestonePair[] Fields;
    public AdvancedCodeMilestonePair[] AdvancedCodes;

    public MilestoneRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public MilestoneRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public MilestoneRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      DocMilestonePair[] docs,
      FieldMilestonePair[] fields,
      TaskMilestonePair[] tasks,
      AdvancedCodeMilestonePair[] advCodes)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, docs, fields, tasks, advCodes)
    {
    }

    public MilestoneRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      DocMilestonePair[] docs,
      FieldMilestonePair[] fields,
      TaskMilestonePair[] tasks,
      AdvancedCodeMilestonePair[] advCodes)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.Docs = docs;
      this.Fields = fields;
      this.Tasks = tasks;
      this.AdvancedCodes = advCodes;
    }

    public MilestoneRuleInfo(
      DataRow dataRow,
      DocMilestonePair[] docs,
      FieldMilestonePair[] fields,
      TaskMilestonePair[] tasks,
      AdvancedCodeMilestonePair[] advCodes)
      : base(dataRow)
    {
      this.Docs = docs;
      this.Fields = fields;
      this.Tasks = tasks;
      this.AdvancedCodes = advCodes;
    }

    public MilestoneRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.Docs = info.GetValue<XmlList<DocMilestonePair>>("Documents").ToArray();
      this.Fields = info.GetValue<XmlList<FieldMilestonePair>>(nameof (Fields)).ToArray();
      this.Tasks = info.GetValue<XmlList<TaskMilestonePair>>(nameof (Tasks)).ToArray();
      this.AdvancedCodes = info.GetValue<XmlList<AdvancedCodeMilestonePair>>(nameof (AdvancedCodes)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.MilestoneRules;

    public override object Clone()
    {
      return (object) new MilestoneRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.Docs, this.Fields, this.Tasks, this.AdvancedCodes);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Documents", (object) new XmlList<DocMilestonePair>((IEnumerable<DocMilestonePair>) this.Docs));
      info.AddValue("Fields", (object) new XmlList<FieldMilestonePair>((IEnumerable<FieldMilestonePair>) this.Fields));
      info.AddValue("Tasks", (object) new XmlList<TaskMilestonePair>((IEnumerable<TaskMilestonePair>) this.Tasks));
      info.AddValue("AdvancedCodes", (object) new XmlList<AdvancedCodeMilestonePair>((IEnumerable<AdvancedCodeMilestonePair>) this.AdvancedCodes));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      MilestoneRuleInfo milestoneRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in milestoneRuleInfo.GetBaseFields())
        yield return baseField;
      if (milestoneRuleInfo.Docs != null || milestoneRuleInfo.Tasks != null || milestoneRuleInfo.Fields != null || milestoneRuleInfo.AdvancedCodes != null)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, "MS.STATUS");
      if (milestoneRuleInfo.Fields != null)
      {
        FieldMilestonePair[] fieldMilestonePairArray = milestoneRuleInfo.Fields;
        for (int index = 0; index < fieldMilestonePairArray.Length; ++index)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, fieldMilestonePairArray[index].FieldID);
        fieldMilestonePairArray = (FieldMilestonePair[]) null;
      }
      if (milestoneRuleInfo.AdvancedCodes != null)
      {
        foreach (string str in ((IEnumerable<AdvancedCodeMilestonePair>) milestoneRuleInfo.AdvancedCodes).Where<AdvancedCodeMilestonePair>((System.Func<AdvancedCodeMilestonePair, bool>) (advancedCodeMilestonePair => advancedCodeMilestonePair != null)).SelectMany<AdvancedCodeMilestonePair, string, string>((System.Func<AdvancedCodeMilestonePair, IEnumerable<string>>) (advancedCodeMilestonePair => (IEnumerable<string>) FieldReplacementRegex.ParseDependentFields(advancedCodeMilestonePair.SourceCode)), (Func<AdvancedCodeMilestonePair, string, string>) ((advancedCodeMilestonePair, fieldId) => fieldId)))
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, str);
      }
    }
  }
}
