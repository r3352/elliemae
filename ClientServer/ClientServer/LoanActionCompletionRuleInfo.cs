// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanActionCompletionRuleInfo
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
  public class LoanActionCompletionRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public DocLoanActionPair[] Docs;
    public TaskLoanActionPair[] Tasks;
    public FieldLoanActionPair[] Fields;
    public MilestoneLoanActionPair[] Milestones;
    public AdvancedCodeLoanActionPair[] AdvancedCodes;

    public LoanActionCompletionRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public LoanActionCompletionRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public LoanActionCompletionRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      DocLoanActionPair[] docs,
      FieldLoanActionPair[] fields,
      TaskLoanActionPair[] tasks,
      MilestoneLoanActionPair[] milestones,
      AdvancedCodeLoanActionPair[] advCodes)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, docs, fields, tasks, milestones, advCodes)
    {
    }

    public LoanActionCompletionRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      DocLoanActionPair[] docs,
      FieldLoanActionPair[] fields,
      TaskLoanActionPair[] tasks,
      MilestoneLoanActionPair[] milestones,
      AdvancedCodeLoanActionPair[] advCodes)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.Docs = docs;
      this.Fields = fields;
      this.Tasks = tasks;
      this.Milestones = milestones;
      this.AdvancedCodes = advCodes;
    }

    public LoanActionCompletionRuleInfo(
      DataRow dataRow,
      DocLoanActionPair[] docs,
      FieldLoanActionPair[] fields,
      TaskLoanActionPair[] tasks,
      MilestoneLoanActionPair[] milestones,
      AdvancedCodeLoanActionPair[] advCodes)
      : base(dataRow)
    {
      this.Docs = docs;
      this.Fields = fields;
      this.Tasks = tasks;
      this.Milestones = milestones;
      this.AdvancedCodes = advCodes;
    }

    public LoanActionCompletionRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.Docs = info.GetValue<XmlList<DocLoanActionPair>>("Documents").ToArray();
      this.Fields = info.GetValue<XmlList<FieldLoanActionPair>>(nameof (Fields)).ToArray();
      this.Tasks = info.GetValue<XmlList<TaskLoanActionPair>>(nameof (Tasks)).ToArray();
      this.Milestones = info.GetValue<XmlList<MilestoneLoanActionPair>>(nameof (Milestones)).ToArray();
      this.AdvancedCodes = info.GetValue<XmlList<AdvancedCodeLoanActionPair>>(nameof (AdvancedCodes)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.LoanActionCompletionRules;

    public override object Clone()
    {
      return (object) new LoanActionCompletionRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.Docs, this.Fields, this.Tasks, this.Milestones, this.AdvancedCodes);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("Documents", (object) new XmlList<DocLoanActionPair>((IEnumerable<DocLoanActionPair>) this.Docs));
      info.AddValue("Fields", (object) new XmlList<FieldLoanActionPair>((IEnumerable<FieldLoanActionPair>) this.Fields));
      info.AddValue("Tasks", (object) new XmlList<TaskLoanActionPair>((IEnumerable<TaskLoanActionPair>) this.Tasks));
      info.AddValue("Milestones", (object) new XmlList<MilestoneLoanActionPair>((IEnumerable<MilestoneLoanActionPair>) this.Milestones));
      info.AddValue("AdvancedCodes", (object) new XmlList<AdvancedCodeLoanActionPair>((IEnumerable<AdvancedCodeLoanActionPair>) this.AdvancedCodes));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      LoanActionCompletionRuleInfo completionRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in completionRuleInfo.GetBaseFields())
        yield return baseField;
      if (completionRuleInfo.Docs != null || completionRuleInfo.Tasks != null || completionRuleInfo.Fields != null || completionRuleInfo.AdvancedCodes != null)
        yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, "MS.STATUS");
      if (completionRuleInfo.Fields != null)
      {
        FieldLoanActionPair[] fieldLoanActionPairArray = completionRuleInfo.Fields;
        for (int index = 0; index < fieldLoanActionPairArray.Length; ++index)
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, fieldLoanActionPairArray[index].FieldID);
        fieldLoanActionPairArray = (FieldLoanActionPair[]) null;
      }
      if (completionRuleInfo.AdvancedCodes != null)
      {
        foreach (string str in ((IEnumerable<AdvancedCodeLoanActionPair>) completionRuleInfo.AdvancedCodes).Where<AdvancedCodeLoanActionPair>((System.Func<AdvancedCodeLoanActionPair, bool>) (AdvancedCodeLoanActionPair => AdvancedCodeLoanActionPair != null)).SelectMany<AdvancedCodeLoanActionPair, string, string>((System.Func<AdvancedCodeLoanActionPair, IEnumerable<string>>) (AdvancedCodeLoanActionPair => (IEnumerable<string>) FieldReplacementRegex.ParseDependentFields(AdvancedCodeLoanActionPair.SourceCode)), (Func<AdvancedCodeLoanActionPair, string, string>) ((AdvancedCodeLoanActionPair, fieldId) => fieldId)))
          yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, str);
      }
    }
  }
}
