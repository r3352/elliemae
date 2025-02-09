// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.FieldBasedConditionMonitor
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class FieldBasedConditionMonitor : LoanConditionMonitor
  {
    private string[] dependentFields;

    public FieldBasedConditionMonitor(ConditionEvaluator evaluator, ExecutionContext context)
      : base(evaluator, context)
    {
      if (!(BizRuleTranslator.GetRuleCondition(evaluator.Rule) is IFieldComposition ruleCondition))
        throw new ArgumentException("Evaluator does not provide field-composition condition");
      this.dependentFields = ruleCondition.GetDependentFields();
      foreach (string dependentField in this.dependentFields)
        context.Loan.RegisterCustomFieldValueChangeEventHandler(dependentField, new Routine(this.onLoanFieldChanged));
    }

    private void onLoanFieldChanged(string fieldId, string value) => this.Reevaluate();

    public override void Dispose()
    {
      try
      {
        foreach (string dependentField in this.dependentFields)
          this.Context.Loan.UnregisterCustomFieldValueChangeEventHandler(dependentField, new Routine(this.onLoanFieldChanged));
      }
      catch
      {
      }
      base.Dispose();
    }
  }
}
