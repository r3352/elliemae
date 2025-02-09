// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.BizRuleTranslator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public static class BizRuleTranslator
  {
    public static RuleCondition GetRuleCondition(BizRuleInfo rule)
    {
      RuleCondition cond = (RuleCondition) null;
      if (rule.Condition == BizRule.Condition.LoanPurpose)
        cond = (RuleCondition) new LoanPurposeCondition((BizRule.LoanPurpose) rule.ConditionStateInt);
      else if (rule.Condition == BizRule.Condition.LoanType)
        cond = (RuleCondition) new LoanTypeCondition((BizRule.LoanType) rule.ConditionStateInt);
      else if (rule.Condition == BizRule.Condition.LoanStatus)
        cond = (RuleCondition) new LoanStatusCondition((BizRule.LoanStatus) rule.ConditionStateInt);
      else if (rule.Condition == BizRule.Condition.CurrentLoanAssocMS)
        cond = (RuleCondition) new UserCondition(rule.ConditionState, rule.ConditionState2);
      else if (rule.Condition == BizRule.Condition.RateLock)
        cond = (RuleCondition) new FieldStateCondition("2400", rule.ConditionState == "0" ? FieldState.Empty : FieldState.NonEmpty);
      else if (rule.Condition == BizRule.Condition.LoanDocType)
        cond = (RuleCondition) new LoanDocTypeCondition((LoanDocTypeMap.Code) BizRuleTranslator.toEnum(rule.ConditionState, typeof (LoanDocTypeMap.Code)));
      else if (rule.Condition == BizRule.Condition.PropertyState)
        cond = (RuleCondition) new PropertyStateCondition((USPS.StateCode) BizRuleTranslator.toEnum(rule.ConditionState, typeof (USPS.StateCode)));
      else if (rule.Condition == BizRule.Condition.AdvancedCoding)
      {
        string definition = rule.ConditionState;
        if ((definition.Contains("[420]") || definition.Contains("[2958]") || definition.ToUpper().Contains("[AUS.X56]")) && (definition.ToLower().Contains("\"first lien\"") || definition.ToLower().Contains("\"second lien\"")))
          definition = definition.Replace("\"First Lien\"", "\"FirstLien\"").Replace("\"Second Lien\"", "\"SecondLien\"").Replace("\"first lien\"", "\"firstlien\"").Replace("\"second lien\"", "\"secondlien\"");
        if (definition.Contains("[1393]") && definition.ToLower().Contains("\"active loan\""))
          definition = definition.Replace("\"Active Loan\"", "\"\"");
        cond = (RuleCondition) new AdvancedCodeCondition(definition);
      }
      else if (rule.Condition == BizRule.Condition.LoanProgram)
        cond = (RuleCondition) new LoanProgramCondition(rule.ConditionState);
      else if (rule.Condition == BizRule.Condition.TPOActions)
        return (RuleCondition) null;
      RuleCondition ruleCondition;
      switch (cond)
      {
        case PredefinedCondition _:
          ruleCondition = (RuleCondition) new LoanChannelCondition(rule.Condition2).And(cond as PredefinedCondition);
          break;
        case CodedCondition _:
          ruleCondition = (RuleCondition) new LoanChannelCodedCondition(rule.Condition2).And(cond as CodedCondition);
          break;
        default:
          ruleCondition = (RuleCondition) new LoanChannelCondition(rule.Condition2);
          break;
      }
      return ruleCondition;
    }

    private static object toEnum(string value, Type enumType)
    {
      try
      {
        return Enum.Parse(enumType, value, true);
      }
      catch
      {
        return (object) 0;
      }
    }
  }
}
