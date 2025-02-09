// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ConditionEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class ConditionEvaluator
  {
    private const string className = "ConditionEvaluator�";
    private static string sw = Tracing.SwDataEngine;
    private BizRuleInfo rule;
    private IConditionImpl conditionImpl;

    public ConditionEvaluator(BizRuleInfo rule, IConditionImpl conditionImpl)
    {
      this.rule = rule;
      this.conditionImpl = conditionImpl;
    }

    public BizRuleInfo Rule => this.rule;

    public bool AppliesTo(ExecutionContext context)
    {
      try
      {
        using (ExecutionContext context1 = (ExecutionContext) context.Clone())
          return this.conditionImpl.AppliesTo((IExecutionContext) context1);
      }
      catch (Exception ex)
      {
        Tracing.Log(ConditionEvaluator.sw, nameof (ConditionEvaluator), TraceLevel.Warning, "Runtime error evaluating condition for rule '" + this.rule.RuleName + "': " + ex.Message);
        try
        {
          string message = context.Loan.GUID + " - " + BizRule.Type2Name(this.rule.RuleType) + " - ConditionEvaluator - Runtime error  evaluating condition for rule '" + this.rule.RuleName + "': " + ex.Message;
          if (this.rule.Condition == BizRule.Condition.AdvancedCoding)
            message = message + " - Advanced Code XML: " + this.rule.ConditionState;
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, message);
        }
        catch
        {
        }
        return false;
      }
    }
  }
}
