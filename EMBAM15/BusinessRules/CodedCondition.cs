// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CodedCondition
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  [Serializable]
  public abstract class CodedCondition : RuleCondition, ICodedObject, IFieldComposition
  {
    public override IConditionImpl CreateImplementation(RuntimeContext context)
    {
      return new RuleBuilder().CreateConditionImpl(this, context);
    }

    public string ToSourceCode()
    {
      return FieldReplacementRegex.Replace(this.GetConditionDefinition()).Replace("\r", " ").Replace("\n", " ");
    }

    public string[] GetDependentFields()
    {
      return FieldReplacementRegex.ParseDependentFields(this.GetConditionDefinition());
    }

    public CodedCondition And(CodedCondition cond)
    {
      return (CodedCondition) new CodedCondition.BinaryCondition(this, cond, CodedCondition.BinaryCondition.Operator.And);
    }

    public CodedCondition Or(CodedCondition cond)
    {
      return (CodedCondition) new CodedCondition.BinaryCondition(this, cond, CodedCondition.BinaryCondition.Operator.Or);
    }

    protected abstract string GetConditionDefinition();

    [Serializable]
    private class BinaryCondition : CodedCondition
    {
      private CodedCondition condA;
      private CodedCondition condB;
      private CodedCondition.BinaryCondition.Operator op;

      public BinaryCondition(
        CodedCondition condA,
        CodedCondition condB,
        CodedCondition.BinaryCondition.Operator op)
      {
        this.condA = condA;
        this.condB = condB;
        this.op = op;
      }

      protected override string GetConditionDefinition()
      {
        if (this.op == CodedCondition.BinaryCondition.Operator.And)
          return "(" + this.condA.GetConditionDefinition() + ") And (" + this.condB.GetConditionDefinition() + ")";
        if (this.op != CodedCondition.BinaryCondition.Operator.Or)
          return "False";
        return "(" + this.condA.GetConditionDefinition() + ") Or (" + this.condB.GetConditionDefinition() + ")";
      }

      public enum Operator
      {
        And,
        Or,
      }
    }
  }
}
