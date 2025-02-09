// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldRuleValidators
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class FieldRuleValidators : RuleValidators
  {
    public FieldRuleValidators(FieldRule[] rules)
      : base((BusinessRule[]) rules)
    {
    }

    public FieldRuleValidators(FieldRule[] rules, bool loadInNewAppDomain)
      : base((BusinessRule[]) rules, loadInNewAppDomain)
    {
    }

    public RuleValidator[] GetValidatorsForField(string fieldId)
    {
      ArrayList arrayList = new ArrayList();
      foreach (RuleValidator ruleValidator in (RuleValidators) this)
      {
        if (string.Compare(((FieldRule) ruleValidator.Rule).FieldID, fieldId, true) == 0)
          arrayList.Add((object) ruleValidator);
      }
      return (RuleValidator[]) arrayList.ToArray(typeof (RuleValidator));
    }

    public void ValidateField(string fieldId, object value, ExecutionContext context)
    {
      foreach (RuleValidator ruleValidator in this.GetValidatorsForField(fieldId))
        ruleValidator.Validate(context, fieldId, value);
    }

    public void ValidateAll(
      ExecutionContext context,
      bool fromImport = false,
      List<string> failedValidationList = null)
    {
      foreach (RuleValidator ruleValidator in (RuleValidators) this)
      {
        FieldRule rule = (FieldRule) ruleValidator.Rule;
        ruleValidator.Validate(context, rule.FieldID, (object) context.Loan.GetSimpleField(rule.FieldID), fromImport, failedValidationList);
      }
    }
  }
}
