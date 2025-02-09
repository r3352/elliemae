// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.CalculationEngine.FieldCalculator
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.DataEngine;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.CalculationEngine
{
  public class FieldCalculator
  {
    private static string className = nameof (FieldCalculator);
    private static string sw = Tracing.SwDataEngine;
    private FieldDefinition field;
    private CustomCalculation calc;
    private ICustomCalculationImpl impl;
    private RuntimeContext context;

    public FieldCalculator(FieldDefinition field, CompiledCalculation calc)
    {
      this.field = field;
      this.calc = calc.Calculation;
      this.impl = (ICustomCalculationImpl) calc;
      this.context = (RuntimeContext) null;
    }

    public FieldCalculator(
      FieldDefinition field,
      CustomCalculation calc,
      ICustomCalculationImpl impl)
    {
      this.field = field;
      this.calc = calc;
      this.impl = impl;
      this.context = (RuntimeContext) null;
    }

    public FieldCalculator(
      FieldDefinition field,
      CustomCalculation calc,
      TypeIdentifier typeId,
      RuntimeContext context)
    {
      this.field = field;
      this.calc = calc;
      this.impl = (ICustomCalculationImpl) context.CreateInstance(typeId, typeof (ICustomCalculationImpl));
      this.context = context;
    }

    public FieldDefinition FieldDefinition => this.field;

    public CustomCalculation Calculation => this.calc;

    public void Invoke(CustomCalculationContext context)
    {
      try
      {
        object obj = this.impl.Calculate((ICalculationContext) context);
        string val = context.Loan.FormatValue(string.Concat(obj), this.field.Format);
        if (val != context.Loan.GetField(this.field.FieldID))
        {
          if (this.field.Format == FieldFormat.AUDIT && context.Loan.IsTemplate)
            return;
          context.Loan.SetFieldFromCal(this.field.FieldID, val);
        }
        Tracing.Log(FieldCalculator.sw, FieldCalculator.className, TraceLevel.Verbose, "Custom calculation for field '" + this.field.FieldID + "' invoked successfully. Result = " + val);
      }
      catch (Exception ex)
      {
        if (Utils.UnformatValue(context.Loan.GetSimpleField(this.field.FieldID), this.field.Format) != "")
          context.Loan.SetFieldFromCal(this.field.FieldID, "");
        Tracing.Log(FieldCalculator.sw, FieldCalculator.className, TraceLevel.Verbose, "Custom calculation for field '" + this.field.FieldID + "' threw exception (" + ex.Message + "). Field was cleared.");
      }
    }
  }
}
