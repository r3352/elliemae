// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.DelayedCompiledTrigger
// Assembly: EMLoanUtils15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 127DBDC4-524E-4934-8841-1513BEA889CD
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMLoanUtils15.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.Common;
using EllieMae.EMLite.Customization;
using EllieMae.EMLite.DataEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public class DelayedCompiledTrigger : DelayedTrigger
  {
    private const string className = "DelayedTriggerTracker�";
    private static string sw = Tracing.SwDataEngine;
    private CompiledTrigger trigger;
    private Dictionary<string, object> originalFieldValues;
    private Dictionary<string, object> modifiedFieldValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);

    public DelayedCompiledTrigger(CompiledTrigger trigger, ExecutionContext Context)
      : base(Context)
    {
      this.trigger = trigger;
      foreach (string triggerField in trigger.Definition.TriggerFields)
        Context.Loan.RegisterCustomFieldValueChangeEventHandler(triggerField, new Routine(this.onTriggerFieldValueChanged));
      this.Reset();
    }

    public TriggerEvent TriggerEvent => ((TriggerImplDef) this.trigger.Definition).Event;

    public override bool SupportsDirectExecution => false;

    public override void Reset()
    {
      Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Resetting delayed trigger '" + this.trigger.Definition.Description + "'");
      this.originalFieldValues = new Dictionary<string, object>((IEqualityComparer<string>) StringComparer.CurrentCultureIgnoreCase);
      foreach (string triggerField in this.trigger.Definition.TriggerFields)
      {
        this.originalFieldValues[triggerField] = Utils.ConvertToNativeValue(this.Context.Loan.GetSimpleField(triggerField), this.Context.Loan.GetFormat(triggerField), (object) null);
        Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Trigger field '" + triggerField + "' has original value = '" + this.originalFieldValues[triggerField] + "'");
      }
    }

    public override void ResetFieldValue(string fieldId, string val)
    {
      Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Resetting trigger field value for field '" + fieldId + "' to be '" + val + "' for delayed trigger '" + this.trigger.Definition.Description + "'");
      if (!this.originalFieldValues.ContainsKey(fieldId))
        return;
      this.originalFieldValues[fieldId] = Utils.ConvertToNativeValue(val, this.Context.Loan.GetFormat(fieldId), (object) null);
    }

    public override void ResubscribeToFieldEvents()
    {
      Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Resubscribing to field events for delayed trigger '" + this.trigger.Definition.Description + "'");
      foreach (string triggerField in this.trigger.Definition.TriggerFields)
      {
        this.Context.Loan.RegisterCustomFieldValueChangeEventHandler(triggerField, new Routine(this.onTriggerFieldValueChanged));
        object nativeValue = Utils.ConvertToNativeValue(this.Context.Loan.GetSimpleField(triggerField), this.Context.Loan.GetFormat(triggerField), (object) null);
        if (this.Context.Loan.GetFieldDefinition(triggerField) is VirtualField)
          Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Field '" + triggerField + "' is virtual. Maintaining original value of '" + this.originalFieldValues[triggerField] + "'");
        else if (!this.modifiedFieldValues.ContainsKey(triggerField))
        {
          Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "   -> Field '" + triggerField + "' was not modified. Replacing original value '" + this.originalFieldValues[triggerField] + "' with '" + nativeValue + "'.");
          this.originalFieldValues[triggerField] = nativeValue;
        }
        else if (!object.Equals(this.modifiedFieldValues[triggerField], nativeValue))
        {
          Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "   -> Field '" + triggerField + "' was modified but new value merged in. Replacing original value '" + this.originalFieldValues[triggerField] + "' with '" + nativeValue + "' and clearing modified value.");
          this.originalFieldValues[triggerField] = nativeValue;
          this.modifiedFieldValues.Remove(triggerField);
        }
        else
          Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "   -> Field '" + triggerField + "' was modified and the modification will be preserved. Original value = '" + this.originalFieldValues[triggerField] + "', modified value = '" + this.modifiedFieldValues[triggerField] + "'");
      }
    }

    public override bool IsActivated()
    {
      Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Checking activation state for delayed trigger '" + this.trigger.Definition.Description + "'");
      foreach (string triggerField in this.trigger.Definition.TriggerFields)
      {
        object nativeValue = Utils.ConvertToNativeValue(this.Context.Loan.GetSimpleField(triggerField), this.Context.Loan.GetFormat(triggerField), (object) null);
        object obj = (object) null;
        if (this.originalFieldValues.ContainsKey(triggerField))
          obj = this.originalFieldValues[triggerField];
        Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Delayed trigger field '" + triggerField + "': Prior Value = '" + obj + "', New Value = '" + nativeValue + "'");
        if (!object.Equals(nativeValue, obj))
        {
          Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Delayed trigger field change detected, evaluating trigger condition...");
          if (this.trigger.Definition.Condition is PredefinedCondition)
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Delayed trigger condition type = " + this.trigger.Definition.Condition.GetType().Name);
          else if (this.trigger.Definition.Condition is CodedCondition)
          {
            CodedCondition condition = (CodedCondition) this.trigger.Definition.Condition;
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, " -> Delayed trigger condition code = " + condition.ToSourceCode());
            foreach (string dependentField in condition.GetDependentFields())
              Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "    -- Field '" + dependentField + "' = '" + this.Context.Loan.GetSimpleField(dependentField) + "'");
          }
          else
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Delayed trigger condition is an unknown type");
          if (this.trigger.ExecuteCondition(this.Context))
          {
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Delayed trigger condition met -- executing trigger.");
            TriggerExecutionResult triggerExecutionResult = this.trigger.Execute(this.Context, triggerField, obj, nativeValue);
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Delayed trigger executed -- result = " + (object) triggerExecutionResult);
            if (triggerExecutionResult == TriggerExecutionResult.Success)
              return true;
          }
          else
            Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Delayed trigger condition not met.");
        }
      }
      return false;
    }

    public override bool Execute(LoanDataMgr dataMgr)
    {
      throw new NotSupportedException("This trigger does not support direct execution");
    }

    private void onTriggerFieldValueChanged(string fieldId, string value)
    {
      Tracing.Log(DelayedCompiledTrigger.sw, "DelayedTriggerTracker", TraceLevel.Verbose, "Delayed trigger field change detected for field '" + fieldId + "' to value '" + value + "' for trigger '" + this.trigger.Definition.Description + "'");
      this.modifiedFieldValues[fieldId] = Utils.ConvertToNativeValue(value, this.Context.Loan.GetFormat(fieldId), (object) null);
    }
  }
}
