// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CodedFormSelector
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class CodedFormSelector : PrintFormSelector, ICodedObject, IFieldComposition
  {
    public CodedFormSelector(string description, string[] selectorFields, RuleCondition condition)
      : base(description, selectorFields, condition)
    {
    }

    public CodedFormSelector(
      string ruleId,
      string description,
      string[] selectorFields,
      RuleCondition condition)
      : base(ruleId, description, selectorFields, condition)
    {
    }

    public override IFormSelectorImpl CreateImplementation(RuntimeContext context)
    {
      return new RuleBuilder().CreatePrintFormSelectorImpl(this, context);
    }

    public string ToSourceCode() => FieldReplacementRegex.Replace(this.GetRuleDefinition());

    public string[] GetDependentFields()
    {
      return FieldReplacementRegex.ParseDependentFields(this.GetRuleDefinition());
    }

    protected abstract string GetRuleDefinition();
  }
}
