// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanActionAccessRuleInfo
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
  public class LoanActionAccessRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public EllieMae.EMLite.ClientServer.LoanActionAccessRights[] LoanActionAccessRights;

    public LoanActionAccessRuleInfo(string ruleName)
      : this(0, ruleName)
    {
    }

    public LoanActionAccessRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public LoanActionAccessRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      EllieMae.EMLite.ClientServer.LoanActionAccessRights[] loanActionAccessRights)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, loanActionAccessRights)
    {
    }

    public LoanActionAccessRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      EllieMae.EMLite.ClientServer.LoanActionAccessRights[] loanActionAccessRights)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.LoanActionAccessRights = loanActionAccessRights;
    }

    public LoanActionAccessRuleInfo(
      DataRow dataRow,
      EllieMae.EMLite.ClientServer.LoanActionAccessRights[] loanActionAccessRights)
      : base(dataRow)
    {
      this.LoanActionAccessRights = loanActionAccessRights;
    }

    public LoanActionAccessRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.LoanActionAccessRights = ((List<EllieMae.EMLite.ClientServer.LoanActionAccessRights>) info.GetValue(nameof (LoanActionAccessRights), typeof (XmlList<EllieMae.EMLite.ClientServer.LoanActionAccessRights>))).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.LoanActionAccess;

    public override object Clone()
    {
      return (object) new LoanActionAccessRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.LoanActionAccessRights);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      XmlList<EllieMae.EMLite.ClientServer.LoanActionAccessRights> xmlList = new XmlList<EllieMae.EMLite.ClientServer.LoanActionAccessRights>();
      if (this.LoanActionAccessRights != null)
        xmlList.AddRange((IEnumerable<EllieMae.EMLite.ClientServer.LoanActionAccessRights>) this.LoanActionAccessRights);
      info.AddValue("LoanActionAccessRights", (object) xmlList);
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      foreach (KeyValuePair<RelationshipType, string> baseField in this.GetBaseFields())
        yield return baseField;
    }
  }
}
