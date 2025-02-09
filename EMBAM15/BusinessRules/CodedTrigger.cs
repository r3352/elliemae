// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CodedTrigger
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class CodedTrigger : Trigger, ICodedObject, IFieldComposition
  {
    public CodedTrigger(string description, string[] triggerFields, RuleCondition condition)
      : base(description, triggerFields, condition)
    {
    }

    public CodedTrigger(
      string ruleId,
      string description,
      string[] triggerFields,
      RuleCondition condition)
      : base(ruleId, description, triggerFields, condition)
    {
    }

    public override ITriggerImpl CreateImplementation(RuntimeContext context)
    {
      return new RuleBuilder().CreateTriggerImpl(this, context);
    }

    public string ToSourceCode() => FieldReplacementRegex.Replace(this.GetRuleDefinition());

    public string[] GetDependentFields()
    {
      return FieldReplacementRegex.ParseDependentFields(this.GetRuleDefinition());
    }

    protected abstract string GetRuleDefinition();
  }
}
