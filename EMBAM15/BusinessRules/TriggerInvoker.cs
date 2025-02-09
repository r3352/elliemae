// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.TriggerInvoker
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class TriggerInvoker
  {
    private static string className = nameof (TriggerInvoker);
    private static string sw = Tracing.SwDataEngine;
    private CompiledTrigger trigger;
    private ExecutionContext context;
    private Dictionary<string, object> fieldValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
    private bool enabled = true;

    public TriggerInvoker(CompiledTrigger trigger, ExecutionContext context)
    {
      this.trigger = trigger;
      this.context = context;
      this.ResubscribeToFieldEvents();
    }

    public void ResubscribeToFieldEvents()
    {
      foreach (string triggerField in this.trigger.Definition.TriggerFields)
        this.context.Loan.RegisterCustomFieldValueChangeEventHandler(triggerField, new Routine(this.onFieldChanged));
      this.Reset();
    }

    public CompiledTrigger Trigger => this.trigger;

    public bool Enabled
    {
      get => this.enabled;
      set => this.enabled = value;
    }

    public ExecutionContext Context => this.context;

    public void Reset()
    {
      string id = this.context.Loan.CurrentBorrowerPair.Id;
      BorrowerPair pair = (BorrowerPair) null;
      BorrowerPair[] borrowerPairs = this.context.Loan.GetBorrowerPairs();
      for (int index = 0; index < borrowerPairs.Length; ++index)
      {
        this.context.Loan.SetBorrowerPair(borrowerPairs[index]);
        foreach (string triggerField in this.trigger.Definition.TriggerFields)
        {
          FieldDefinition parentField = StandardFields.All[triggerField];
          if (parentField == null)
          {
            StandardField field = StandardFields.GetField(triggerField);
            if (field != null && field.ParentField != null)
              parentField = field.ParentField;
          }
          if (parentField == null || !parentField.RequiresBorrowerPredicate)
          {
            if (index == 0)
              this.fieldValues[triggerField] = Utils.ConvertToNativeValue(this.context.Loan.GetSimpleField(triggerField), this.context.Loan.GetFormat(triggerField), (object) null);
          }
          else
            this.fieldValues[triggerField + "#" + (object) (index + 1)] = Utils.ConvertToNativeValue(this.context.Loan.GetSimpleField(triggerField), this.context.Loan.GetFormat(triggerField), (object) null);
        }
        if (pair == null && id == borrowerPairs[index].Id)
          pair = borrowerPairs[index];
      }
      this.context.Loan.SetBorrowerPair(pair);
    }

    private void onFieldChanged(string fieldId, string val)
    {
      if (!this.enabled)
        return;
      string str = (string) null;
      int length = fieldId.IndexOf("#");
      if (length > -1)
      {
        str = fieldId.Substring(length + 1);
        fieldId = fieldId.Substring(0, length);
      }
      bool flag = false;
      foreach (string triggerField in this.trigger.Definition.TriggerFields)
      {
        if (string.Compare(triggerField, fieldId, true) == 0)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return;
      object nativeValue = Utils.ConvertToNativeValue(val, this.context.Loan.GetFormat(fieldId), (object) null);
      object obj = (object) null;
      if (str != null)
      {
        if (this.fieldValues.ContainsKey(fieldId + "#" + str))
          obj = this.fieldValues[fieldId + "#" + str];
      }
      else if (this.fieldValues.ContainsKey(fieldId))
        obj = this.fieldValues[fieldId];
      Tracing.Log(TriggerInvoker.sw, TriggerInvoker.className, TraceLevel.Verbose, "Trigger field change handler invoked for '" + this.trigger.Definition.Description + "' on field '" + fieldId + "'. Prior Value = " + obj + ", New Value = " + nativeValue);
      if (object.Equals(nativeValue, obj))
        return;
      using (Tracing.StartTimer(TriggerInvoker.sw, TriggerInvoker.className, TraceLevel.Verbose, "Executing trigger '" + this.trigger.Definition.Description + "'..."))
      {
        this.fieldValues[str != null ? fieldId + "#" + str : fieldId] = nativeValue;
        bool enabled = this.context.Loan.Validator.Enabled;
        try
        {
          this.invokeInternal(fieldId, obj, nativeValue);
        }
        finally
        {
          this.context.Loan.Validator.Enabled = enabled;
        }
      }
    }

    private void invokeInternal(string fieldId, object priorValue, object newValue)
    {
      Stopwatch stopwatch = (Stopwatch) null;
      bool calculationDiagnostic = EnConfigurationSettings.GlobalSettings.AllowCalculationDiagnostic;
      if (calculationDiagnostic)
      {
        stopwatch = new Stopwatch();
        stopwatch.Start();
      }
      TriggerExecutionResult triggerExecutionResult = this.trigger.Execute(this.context, fieldId, priorValue, newValue);
      if (calculationDiagnostic)
      {
        stopwatch.Stop();
        if (this.Context != null && this.Context.Loan != null && this.Context.Loan.Calculator != null)
          this.Context.Loan.Calculator.IncrementTriggerCounter(this.Trigger.Definition.Description, stopwatch.ElapsedMilliseconds);
      }
      if (triggerExecutionResult != TriggerExecutionResult.FailedException || !EnConfigurationSettings.GlobalSettings.Debug || !(this.trigger.Definition is CodedTrigger))
        return;
      Tracing.Log(TriggerInvoker.sw, TriggerInvoker.className, TraceLevel.Warning, "Trigger source code:" + Environment.NewLine + ((CodedTrigger) this.trigger.Definition).ToSourceCode());
    }
  }
}
