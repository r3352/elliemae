// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.LoanAccessRuleInfo
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
  public class LoanAccessRuleInfo : BizRuleInfo, IFieldSearchable
  {
    public PersonaLoanAccessRight[] LoanAccessRights;

    public LoanAccessRuleInfo(string ruleName)
      : base(0, ruleName)
    {
    }

    public LoanAccessRuleInfo(int ruleID, string ruleName)
      : base(ruleID, ruleName)
    {
    }

    public LoanAccessRuleInfo(
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      PersonaLoanAccessRight[] loanAccessRights)
      : this(0, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments, loanAccessRights)
    {
    }

    public LoanAccessRuleInfo(
      int ruleID,
      string ruleName,
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2,
      string advancedCodeXml,
      string comments,
      PersonaLoanAccessRight[] loanAccessRights)
      : base(ruleID, ruleName, condition, condition2, conditionState, conditionState2, advancedCodeXml, comments)
    {
      this.LoanAccessRights = loanAccessRights;
    }

    public LoanAccessRuleInfo(DataRow dataRow, PersonaLoanAccessRight[] loanAccessRights)
      : base(dataRow)
    {
      this.LoanAccessRights = loanAccessRights;
    }

    public LoanAccessRuleInfo(XmlSerializationInfo info)
      : base(info)
    {
      this.LoanAccessRights = info.GetValue<XmlList<PersonaLoanAccessRight>>(nameof (LoanAccessRights)).ToArray();
    }

    public override BizRuleType RuleType => BizRuleType.LoanAccess;

    public EllieMae.EMLite.ClientServer.LoanAccessRights GetLoanAccessRights()
    {
      return new EllieMae.EMLite.ClientServer.LoanAccessRights(this.LoanAccessRights);
    }

    public override object Clone()
    {
      return (object) new LoanAccessRuleInfo(this.RuleID, this.RuleName, this.Condition, this.Condition2, this.ConditionState, this.ConditionState2, this.AdvancedCodeXML, this.CommentsTxt, this.LoanAccessRights);
    }

    public override void GetXmlObjectData(XmlSerializationInfo info)
    {
      base.GetXmlObjectData(info);
      info.AddValue("LoanAccessRights", (object) new XmlList<PersonaLoanAccessRight>((IEnumerable<PersonaLoanAccessRight>) this.LoanAccessRights));
    }

    public IEnumerable<KeyValuePair<RelationshipType, string>> GetFields()
    {
      LoanAccessRuleInfo loanAccessRuleInfo = this;
      foreach (KeyValuePair<RelationshipType, string> baseField in loanAccessRuleInfo.GetBaseFields())
        yield return baseField;
      PersonaLoanAccessRight[] personaLoanAccessRightArray = loanAccessRuleInfo.LoanAccessRights;
      for (int index1 = 0; index1 < personaLoanAccessRightArray.Length; ++index1)
      {
        PersonaLoanAccessRight personaLoanAccessRight = personaLoanAccessRightArray[index1];
        if (personaLoanAccessRight.editableFields != null && ((IEnumerable<string>) personaLoanAccessRight.editableFields).Any<string>())
        {
          string[] strArray = personaLoanAccessRight.editableFields;
          for (int index2 = 0; index2 < strArray.Length; ++index2)
            yield return new KeyValuePair<RelationshipType, string>(RelationshipType.ResultFields, strArray[index2]);
          strArray = (string[]) null;
        }
      }
      personaLoanAccessRightArray = (PersonaLoanAccessRight[]) null;
    }
  }
}
