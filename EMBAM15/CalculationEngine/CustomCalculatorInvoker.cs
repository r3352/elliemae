// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.CustomCalculatorInvoker
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.DataEngine;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class CustomCalculatorInvoker
  {
    private CustomFieldCalculator calc;
    private CustomCalculationContext context;

    public CustomCalculatorInvoker(CustomFieldCalculator calc, CustomCalculationContext context)
    {
      this.calc = calc;
      this.context = context;
      foreach (string dependentField in calc.Calculation.DependentFields)
      {
        if (string.Compare(dependentField, calc.Field.FieldID, true) != 0)
          context.Loan.RegisterCustomFieldValueChangeEventHandler(dependentField, new Routine(this.onFieldChanged));
      }
    }

    public CustomFieldCalculator Calculator => this.calc;

    public CustomCalculationContext Context => this.context;

    public void Invoke()
    {
      if (this.calc.Field.IsAuditField())
        return;
      this.invokeInternal();
    }

    private void onFieldChanged(string fieldId, string val) => this.invokeInternal();

    private void invokeInternal()
    {
      try
      {
        using (CustomCalculationContext context = (CustomCalculationContext) this.context.Clone())
        {
          Stopwatch stopwatch = (Stopwatch) null;
          bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
          if (calculationDiagnostic)
          {
            stopwatch = new Stopwatch();
            stopwatch.Start();
          }
          this.calc.Invoke(context);
          if (!calculationDiagnostic)
            return;
          stopwatch.Stop();
          if (this.Context == null || this.Context.Loan == null || this.Context.Loan.Calculator == null)
            return;
          this.Context.Loan.Calculator.IncrementTriggerCounter(this.calc.Calculation.ToString(), stopwatch.ElapsedMilliseconds);
        }
      }
      catch
      {
      }
    }
  }
}
