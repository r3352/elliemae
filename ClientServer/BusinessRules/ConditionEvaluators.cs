// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.ConditionEvaluators
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class ConditionEvaluators : IEnumerable<ConditionEvaluator>, IEnumerable
  {
    private const string className = "ConditionEvaluators�";
    private static readonly string sw = Tracing.SwDataEngine;
    private List<ConditionEvaluator> evaluators = new List<ConditionEvaluator>();
    public bool exceptionErr;

    public ConditionEvaluators(BizRuleInfo[] rules)
      : this(rules, true)
    {
    }

    public ConditionEvaluators(BizRuleInfo[] rules, bool loadInNewAppDomain)
    {
      try
      {
        if (rules.Length == 0)
          return;
        RuleBuilder ruleBuilder = new RuleBuilder();
        Dictionary<int, RuleCondition> dictionary1 = new Dictionary<int, RuleCondition>();
        Dictionary<int, TypeIdentifier> dictionary2 = new Dictionary<int, TypeIdentifier>();
        List<string> stringList = new List<string>();
        foreach (BizRuleInfo rule in rules)
        {
          RuleCondition ruleCondition = BizRuleTranslator.GetRuleCondition(rule);
          dictionary1[rule.RuleID] = ruleCondition;
          if (ruleCondition is ICodedObject)
          {
            TypeIdentifier typeIdentifier = ruleBuilder.AddCondition(string.Concat((object) rule.RuleID), ruleCondition as ICodedObject);
            dictionary2[rule.RuleID] = typeIdentifier;
          }
        }
        RuntimeContext context = (RuntimeContext) null;
        if (dictionary2.Count > 0)
        {
          context = loadInNewAppDomain ? RuntimeContext.Create() : RuntimeContext.Current;
          try
          {
            ruleBuilder.Compile(context);
          }
          catch (Exception ex)
          {
            RemoteLogger.Write(TraceLevel.Error, nameof (ConditionEvaluators), "Error compiling custom business rule conditions. ", ex);
            this.exceptionErr = true;
          }
        }
        foreach (BizRuleInfo rule in rules)
        {
          RuleCondition ruleCondition = dictionary1[rule.RuleID];
          switch (ruleCondition)
          {
            case ICodedObject _:
              try
              {
                IConditionImpl instance = (IConditionImpl) context.CreateInstance(dictionary2[rule.RuleID], typeof (IConditionImpl));
                this.evaluators.Add((ConditionEvaluator) new ConditionEvaluators.AdvancedConditionEvaluator(rule, instance, context));
                break;
              }
              catch
              {
                Tracing.Log(ConditionEvaluators.sw, nameof (ConditionEvaluators), TraceLevel.Error, "Unable to load condition implementation for rule " + (object) rule.RuleID);
                RemoteLogger.Write(TraceLevel.Error, "Unable to load condition implementation for rule " + (object) rule.RuleID);
                this.exceptionErr = true;
                break;
              }
            case IConditionImpl _:
              this.evaluators.Add(new ConditionEvaluator(rule, ruleCondition as IConditionImpl));
              break;
          }
        }
      }
      catch (Exception ex)
      {
        RemoteLogger.Write(TraceLevel.Error, nameof (ConditionEvaluators), "Error building custom condition evaluators. ", ex);
        this.exceptionErr = true;
      }
    }

    public int Count => this.evaluators.Count;

    public ConditionEvaluator this[int index] => this.evaluators[index];

    public BizRuleInfo[] GetApplicableRules(ExecutionContext context)
    {
      List<BizRuleInfo> bizRuleInfoList = new List<BizRuleInfo>();
      foreach (ConditionEvaluators.AdvancedConditionEvaluator evaluator in this.evaluators)
      {
        if (evaluator.AppliesTo(context))
          bizRuleInfoList.Add(evaluator.Rule);
      }
      return bizRuleInfoList.ToArray();
    }

    public IEnumerator<ConditionEvaluator> GetEnumerator()
    {
      return (IEnumerator<ConditionEvaluator>) this.evaluators.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.evaluators.GetEnumerator();

    private class AdvancedConditionEvaluator : ConditionEvaluator
    {
      private RuntimeContext context;

      public AdvancedConditionEvaluator(
        BizRuleInfo rule,
        IConditionImpl conditionImpl,
        RuntimeContext context)
        : base(rule, conditionImpl)
      {
        this.context = context;
      }
    }
  }
}
