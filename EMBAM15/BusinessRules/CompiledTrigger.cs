// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.CompiledTrigger
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
  public class CompiledTrigger
  {
    private const string className = "CompiledTrigger";
    private static string sw = Tracing.SwDataEngine;
    private Trigger def;
    private ITriggerImpl triggerImpl;
    private IConditionImpl conditionImpl;
    private RuntimeContext context;

    public CompiledTrigger(
      Trigger def,
      ITriggerImpl triggerImpl,
      IConditionImpl conditionImpl,
      RuntimeContext context)
    {
      this.def = def;
      this.triggerImpl = triggerImpl;
      this.conditionImpl = conditionImpl;
      this.context = context;
    }

    public Trigger Definition => this.def;

    public bool ExecuteCondition(ExecutionContext context)
    {
      if (this.conditionImpl == null)
        return true;
      using (ExecutionContext context1 = (ExecutionContext) context.Clone())
        return this.conditionImpl.AppliesTo((IExecutionContext) context1);
    }

    public TriggerExecutionResult Execute(
      ExecutionContext context,
      string modifiedFieldId,
      object priorValue,
      object newValue)
    {
      using (ExecutionContext context1 = (ExecutionContext) context.Clone())
      {
        try
        {
          if (this.conditionImpl != null)
          {
            if (!this.conditionImpl.AppliesTo((IExecutionContext) context1))
              return TriggerExecutionResult.ConditionsNotMet;
          }
        }
        catch (Exception ex)
        {
          Tracing.Log(CompiledTrigger.sw, nameof (CompiledTrigger), TraceLevel.Warning, "Error executing condition for trigger '" + this.def.Description + "': " + ex.Message);
          try
          {
            string message = context.Loan.GUID + " - Field Trigger Rule - CompiledTrigger - Runtime error executing condition for trigger'" + this.def.Description + "': " + ex.Message;
            if (this.def.Condition is CodedCondition)
            {
              CodedCondition condition = (CodedCondition) this.def.Condition;
              Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, message + "- Advanced code: " + condition.ToSourceCode());
            }
            else
              Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, message);
          }
          catch
          {
          }
          return TriggerExecutionResult.FailedConditionException;
        }
        try
        {
          if (this.triggerImpl.Execute((IExecutionContext) context1, modifiedFieldId, priorValue, newValue))
          {
            Tracing.Log(CompiledTrigger.sw, nameof (CompiledTrigger), TraceLevel.Info, "Trigger '" + this.def.Description + "' executed successfully.");
            return TriggerExecutionResult.Success;
          }
          string msg = "Trigger '" + this.def.Description + "' not executed -- required conditions not met.";
          Tracing.Log(CompiledTrigger.sw, nameof (CompiledTrigger), TraceLevel.Verbose, msg);
          return TriggerExecutionResult.ConditionsNotMet;
        }
        catch (RecursiveExecutionException ex)
        {
          string msg = "Trigger '" + this.def.Description + "' not executed -- recursive execution detected.";
          Tracing.Log(CompiledTrigger.sw, nameof (CompiledTrigger), TraceLevel.Verbose, msg);
          this.sendTriggerInfoWithMessage(context.Loan.GUID + " - Field Trigger Rule - CompiledTrigger - " + msg);
          return TriggerExecutionResult.FailedRecursive;
        }
        catch (Exception ex)
        {
          Exception exception = ex;
          string str = exception.Message;
          for (; exception.InnerException != null; exception = exception.InnerException)
            str = str + ": " + exception.InnerException.Message;
          string msg = context.Loan.GUID + " - Field Trigger Rule - CompiledTrigger - Runtime error executing trigger '" + this.def.Description + "': " + str;
          Tracing.Log(CompiledTrigger.sw, nameof (CompiledTrigger), TraceLevel.Warning, msg);
          this.sendTriggerInfoWithMessage(msg);
          return TriggerExecutionResult.FailedException;
        }
      }
    }

    private void sendTriggerInfoWithMessage(string msg)
    {
      try
      {
        if (this.def is CodedTrigger)
        {
          CodedTrigger def = (CodedTrigger) this.def;
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, msg + "- Advanced code: " + def.ToSourceCode());
        }
        else
          Tracing.SendBusinessRuleErrorToServer(TraceLevel.Error, msg);
      }
      catch
      {
      }
    }
  }
}
