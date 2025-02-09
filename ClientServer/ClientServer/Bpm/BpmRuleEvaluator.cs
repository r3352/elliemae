// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.Bpm.BpmRuleEvaluator
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer.Bpm
{
  public abstract class BpmRuleEvaluator
  {
    private BizRuleInfo[] rules;
    private Dictionary<LoanConditions, object> ruleEvalCache = new Dictionary<LoanConditions, object>();

    protected BpmRuleEvaluator(BizRuleInfo[] rules) => this.rules = rules;

    protected BizRuleInfo[] GetRules() => (BizRuleInfo[]) this.rules.Clone();

    public BizRuleInfo[] GetRules(BizRule.Condition condition)
    {
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      foreach (BizRuleInfo rule in this.GetRules())
      {
        if (rule.Condition == condition)
          bizRuleInfoList.Add(rule);
      }
      return bizRuleInfoList.ToArray();
    }

    public BizRuleInfo[] GetRule(
      BizRule.Condition condition,
      string condition2,
      string conditionState,
      string conditionState2)
    {
      ArrayList arrayList = new ArrayList();
      foreach (BizRuleInfo rule in this.GetRules())
      {
        if (rule.Condition == condition && rule.Condition2.IndexOf(condition2) > -1 && condition == BizRule.Condition.Null)
          arrayList.Add((object) rule);
        else if (rule.Condition == condition && rule.Condition2.IndexOf(condition2) > -1 && rule.ConditionState == conditionState && (rule.ConditionState2 ?? "") == (conditionState2 ?? ""))
          arrayList.Add((object) rule);
      }
      return arrayList.Count > 0 ? (BizRuleInfo[]) arrayList.ToArray(typeof (BizRuleInfo)) : (BizRuleInfo[]) null;
    }

    public BizRuleInfo[] GetRule(
      BizRule.Condition condition,
      string condition2,
      int conditionState,
      string milestoneID,
      string conditionState2)
    {
      string conditionState1 = BizRule.ConditionStateAsString(condition, conditionState, milestoneID);
      return this.GetRule(condition, condition2, conditionState1, conditionState2);
    }

    public object Evaluate(Persona[] personas, LoanConditions conditions)
    {
      if (this.ruleEvalCache.ContainsKey(conditions))
        return this.ruleEvalCache[conditions];
      object conditions1 = this.EvaluateConditions(personas, conditions);
      this.ruleEvalCache[conditions] = conditions1;
      return conditions1;
    }

    public object Evaluate(Persona[] personas) => this.EvaluateNullConditions(personas);

    protected abstract object EvaluateConditions(Persona[] personas, LoanConditions conditions);

    protected abstract object EvaluateNullConditions(Persona[] personas);
  }
}
