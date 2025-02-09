// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PredefinedCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class PredefinedCondition : RuleCondition, IConditionImpl
  {
    public static readonly PredefinedCondition Empty = (PredefinedCondition) new PredefinedCondition.EmptyRuleCondition();

    public override sealed IConditionImpl CreateImplementation(RuntimeContext context)
    {
      return (IConditionImpl) this;
    }

    public abstract bool AppliesTo(IExecutionContext context);

    public PredefinedCondition And(PredefinedCondition cond)
    {
      return (PredefinedCondition) new PredefinedCondition.BinaryCondition(this, cond, PredefinedCondition.BinaryCondition.Operator.And);
    }

    public PredefinedCondition Or(PredefinedCondition cond)
    {
      return (PredefinedCondition) new PredefinedCondition.BinaryCondition(this, cond, PredefinedCondition.BinaryCondition.Operator.Or);
    }

    [Serializable]
    private class EmptyRuleCondition : PredefinedCondition
    {
      public override bool AppliesTo(IExecutionContext context) => true;
    }

    [Serializable]
    private class BinaryCondition : PredefinedCondition, IFieldComposition
    {
      private PredefinedCondition condA;
      private PredefinedCondition condB;
      private PredefinedCondition.BinaryCondition.Operator op;

      public BinaryCondition(
        PredefinedCondition condA,
        PredefinedCondition condB,
        PredefinedCondition.BinaryCondition.Operator op)
      {
        this.condA = condA;
        this.condB = condB;
        this.op = op;
      }

      public override bool AppliesTo(IExecutionContext context)
      {
        if (this.op == PredefinedCondition.BinaryCondition.Operator.And)
          return this.condA.AppliesTo(context) && this.condB.AppliesTo(context);
        if (this.op != PredefinedCondition.BinaryCondition.Operator.Or)
          return false;
        return this.condA.AppliesTo(context) || this.condB.AppliesTo(context);
      }

      public string[] GetDependentFields()
      {
        Dictionary<string, bool> dictionary = new Dictionary<string, bool>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
        if (this.condA is IFieldComposition)
        {
          foreach (string dependentField in ((IFieldComposition) this.condA).GetDependentFields())
            dictionary[dependentField] = true;
        }
        if (this.condB is IFieldComposition)
        {
          foreach (string dependentField in ((IFieldComposition) this.condB).GetDependentFields())
            dictionary[dependentField] = true;
        }
        return new List<string>((IEnumerable<string>) dictionary.Keys).ToArray();
      }

      public enum Operator
      {
        And,
        Or,
      }
    }
  }
}
