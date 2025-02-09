// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.RuleValidators
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

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
  public class RuleValidators : IEnumerable, IDisposable
  {
    private static string className = nameof (RuleValidators);
    private static readonly string sw = Tracing.SwDataEngine;
    private RuntimeContext context;
    private List<RuleValidator> validators;

    public RuleValidators(BusinessRule[] rules)
      : this(rules, true)
    {
    }

    public RuleValidators(BusinessRule[] rules, bool loadInNewAppDomain)
    {
      this.validators = new List<RuleValidator>();
      RuleBuilder ruleBuilder = new RuleBuilder();
      Hashtable hashtable1 = new Hashtable();
      Hashtable hashtable2 = new Hashtable();
      Hashtable hashtable3 = new Hashtable();
      foreach (BusinessRule rule in rules)
      {
        try
        {
          if (rule is ICodedObject)
            hashtable1[(object) rule.RuleID] = (object) ruleBuilder.AddRule(rule.RuleID, rule as ICodedObject);
          if (rule.Condition is ICodedObject)
            hashtable3[(object) rule.RuleID] = (object) ruleBuilder.AddCondition(rule.RuleID, rule.Condition as ICodedObject);
          hashtable2[(object) rule.RuleID] = (object) rule;
        }
        catch (Exception ex)
        {
          Tracing.Log(RuleValidators.sw, RuleValidators.className, TraceLevel.Error, "Error building validation function for rule '" + (object) rule + "': " + (object) ex);
        }
      }
      if (hashtable2.Count == 0)
        return;
      if (hashtable1.Count + hashtable3.Count > 0)
        this.context = loadInNewAppDomain ? RuntimeContext.Create() : RuntimeContext.Current;
      try
      {
        if (this.context != null)
          ruleBuilder.Compile(this.context);
        foreach (DictionaryEntry dictionaryEntry in hashtable2)
        {
          BusinessRule rule = (BusinessRule) dictionaryEntry.Value;
          IValidatorImpl validatorImpl = rule as IValidatorImpl;
          IConditionImpl conditionImpl = rule.Condition as IConditionImpl;
          if (validatorImpl == null)
            validatorImpl = (IValidatorImpl) this.context.CreateInstance((TypeIdentifier) hashtable1[dictionaryEntry.Key], typeof (IValidatorImpl));
          if (conditionImpl == null)
            conditionImpl = (IConditionImpl) this.context.CreateInstance((TypeIdentifier) hashtable3[dictionaryEntry.Key], typeof (IConditionImpl));
          this.validators.Add(this.CreateValidator(rule, validatorImpl, conditionImpl, this.context));
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(RuleValidators.sw, RuleValidators.className, TraceLevel.Error, "Error compiling field rules: " + (object) ex);
      }
    }

    public int Count => this.validators.Count;

    public RuleValidator this[int index] => this.validators[index];

    public IEnumerator GetEnumerator() => (IEnumerator) this.validators.GetEnumerator();

    public void Dispose()
    {
      if (this.context == null)
        return;
      this.context.Dispose();
      this.context = (RuntimeContext) null;
    }

    protected virtual RuleValidator CreateValidator(
      BusinessRule rule,
      IValidatorImpl validatorImpl,
      IConditionImpl conditionImpl,
      RuntimeContext context)
    {
      return new RuleValidator(rule, validatorImpl, conditionImpl, context);
    }
  }
}
