// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.PrintFormRuleValidators
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class PrintFormRuleValidators : RuleValidators
  {
    private string formID = string.Empty;
    private string exceptionCodes = string.Empty;
    private ExecutionContext context;

    public string FormID => this.formID;

    public string ExceptionCodes => this.exceptionCodes;

    public PrintFormRuleValidators(PrintFormRule[] rules, string formID)
      : base((BusinessRule[]) rules)
    {
      this.formID = formID;
      this.exceptionCodes = string.Empty;
    }

    public bool ValidateAll(ExecutionContext context)
    {
      this.context = context;
      foreach (RuleValidator ruleValidator in (RuleValidators) this)
      {
        try
        {
          PrintFormRule rule = (PrintFormRule) ruleValidator.Rule;
          ruleValidator.Validate(context, (object) this.formID);
        }
        catch (ValidationException ex)
        {
          ex.Rule = ruleValidator.Rule;
          this.exceptionCodes = ex.Message;
          try
          {
            Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, context.Loan.GUID + " - Print Auto Selection Rule - PrintFormRuleValidators - " + (object) ex.Rule + " - " + ex.Message);
          }
          catch
          {
          }
          return false;
        }
      }
      return true;
    }

    public bool RevalidateAll() => this.context == null || this.ValidateAll(this.context);
  }
}
