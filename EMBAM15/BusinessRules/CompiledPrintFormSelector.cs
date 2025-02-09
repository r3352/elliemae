// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CompiledPrintFormSelector
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Compiler;
using EllieMae.EMLite.Customization;
using System;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class CompiledPrintFormSelector
  {
    private const string className = "CompiledPrintFormSelector";
    private static string sw = Tracing.SwDataEngine;
    private PrintFormSelector def;
    private IFormSelectorImpl printFormImpl;
    private IConditionImpl conditionImpl;
    private RuntimeContext context;

    public CompiledPrintFormSelector(
      PrintFormSelector def,
      IFormSelectorImpl printFormImpl,
      IConditionImpl conditionImpl,
      RuntimeContext context)
    {
      this.def = def;
      this.printFormImpl = printFormImpl;
      this.conditionImpl = conditionImpl;
      this.context = context;
    }

    public PrintFormSelector Definition => this.def;

    public bool ExecutePrintFormSelector(ExecutionContext context)
    {
      try
      {
        using (ExecutionContext context1 = (ExecutionContext) context.Clone())
        {
          if (this.conditionImpl != null && !this.conditionImpl.AppliesTo((IExecutionContext) context1))
            return false;
          string selectorField = this.def.SelectorFields[0];
          string field = context1.Loan.GetField(selectorField);
          FieldFormat format = context1.Loan.GetFormat(selectorField);
          object nativeValue = Utils.ConvertToNativeValue(field ?? "", format, (object) null);
          return this.printFormImpl.Execute((IExecutionContext) context1, selectorField, nativeValue);
        }
      }
      catch (Exception ex)
      {
        Tracing.Log(CompiledPrintFormSelector.sw, nameof (CompiledPrintFormSelector), TraceLevel.Error, "ExecutePrintFormSelector: cannot run compiled assembly in Auto Form Selector Event Condition. Error: " + ex.Message);
        try
        {
          string message = context.Loan.GUID + " - Print Auto Selection - CompiledPrintFormSelector - " + this.def.Description + " - " + ex.Message;
          if (this.def is CodedFormSelector)
          {
            CodedFormSelector def = (CodedFormSelector) this.def;
            message = message + " - Advanced Code: " + def.ToSourceCode();
          }
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, message);
        }
        catch
        {
        }
        return false;
      }
    }
  }
}
