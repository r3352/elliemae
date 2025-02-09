// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.RuleValidator
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class RuleValidator
  {
    private const string className = "RuleValidator";
    private static string sw = Tracing.SwDataEngine;
    private BusinessRule rule;
    private IValidatorImpl validatorImpl;
    private IConditionImpl conditionImpl;
    private RuntimeContext context;

    public RuleValidator(
      BusinessRule rule,
      IValidatorImpl validatorImpl,
      IConditionImpl conditionImpl,
      RuntimeContext context)
    {
      this.rule = rule;
      this.validatorImpl = validatorImpl;
      this.conditionImpl = conditionImpl;
      this.context = context;
    }

    public BusinessRule Rule => this.rule;

    public void Validate(
      ExecutionContext context,
      string fieldId,
      object value,
      bool fromImport = false,
      List<string> failedValidationList = null)
    {
      try
      {
        using (ExecutionContext context1 = (ExecutionContext) context.Clone())
        {
          if (!this.conditionImpl.AppliesTo((IExecutionContext) context1))
            return;
          this.validatorImpl.Validate((IExecutionContext) context1, fieldId, value);
        }
      }
      catch (ValidationException ex)
      {
        Tracing.Log(RuleValidator.sw, nameof (RuleValidator), TraceLevel.Warning, "Validation exception evaluating rule '" + this.rule.Description + "': " + ex.Message);
        ex.Rule = this.rule;
        failedValidationList?.Add(fieldId);
        if (!fromImport)
          throw ex;
      }
    }

    public void Validate(ExecutionContext context, object value)
    {
      this.Validate(context, (string) null, value);
    }
  }
}
