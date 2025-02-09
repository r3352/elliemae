// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.BusinessRules.TriggerImplBase
// Assembly: EMBAM15, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 3F88DC24-E168-47B4-9B32-B34D72387BF6
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMBAM15.dll

using EllieMae.EMLite.Customization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace EllieMae.EMLite.BusinessRules
{
  public abstract class TriggerImplBase : ContextBoundCodeImplBase, ITriggerImpl
  {
    private ConcurrentDictionary<int, Stack<TriggerImplBase.ExecutionContextHolder>> threadContextStack = new ConcurrentDictionary<int, Stack<TriggerImplBase.ExecutionContextHolder>>();

    public bool Execute(
      IExecutionContext context,
      string fieldId,
      object priorValue,
      object newValue)
    {
      Stack<TriggerImplBase.ExecutionContextHolder> orAdd = this.threadContextStack.GetOrAdd(Thread.CurrentThread.ManagedThreadId, (Func<int, Stack<TriggerImplBase.ExecutionContextHolder>>) (t => new Stack<TriggerImplBase.ExecutionContextHolder>()));
      TriggerImplBase.ExecutionContextHolder executionContextHolder1 = !this.isFieldInContextStack(fieldId, orAdd) ? new TriggerImplBase.ExecutionContextHolder(context, fieldId) : throw new RecursiveExecutionException();
      orAdd.Push(executionContextHolder1);
      try
      {
        this.EstablishContext(executionContextHolder1.Context);
        executionContextHolder1.Context.Fields.EnableRules();
        return this.ExecuteTrigger(fieldId, priorValue, newValue);
      }
      catch (RecursiveExecutionException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new ExecutionException("Trigger execution error for field '" + fieldId + "'.", ex);
      }
      finally
      {
        orAdd.Pop();
        if (orAdd.Count == 0)
        {
          this.ReleaseContext();
        }
        else
        {
          TriggerImplBase.ExecutionContextHolder executionContextHolder2 = orAdd.Peek();
          this.EstablishContext(executionContextHolder2 == null ? (IExecutionContext) null : executionContextHolder2.Context);
          if (executionContextHolder2 != null)
          {
            if (executionContextHolder2.IgnoreValidationErrors)
              executionContextHolder2.Context.Fields.DisableRules();
            else
              executionContextHolder2.Context.Fields.EnableRules();
          }
        }
      }
    }

    private TriggerImplBase.ExecutionContextHolder CurrentContext
    {
      get => this.threadContextStack[Thread.CurrentThread.ManagedThreadId].Peek();
    }

    protected abstract bool ExecuteTrigger(string fieldId, object priorValue, object newValue);

    protected object XType(object value)
    {
      return this.Fields.XType(value, this.CurrentContext.CurrentFieldID);
    }

    protected void IgnoreValidationErrors()
    {
      this.CurrentContext.IgnoreValidationErrors = true;
      this.CurrentContext.Context.Fields.DisableRules();
    }

    private bool isFieldInContextStack(
      string fieldId,
      Stack<TriggerImplBase.ExecutionContextHolder> contextStack)
    {
      foreach (TriggerImplBase.ExecutionContextHolder context in contextStack)
      {
        if (string.Compare(context.CurrentFieldID, fieldId, true) == 0)
          return true;
      }
      return false;
    }

    private class ExecutionContextHolder
    {
      public IExecutionContext Context;
      public string CurrentFieldID;
      public bool IgnoreValidationErrors;

      public ExecutionContextHolder(IExecutionContext context, string currentFieldId)
      {
        this.Context = context;
        this.CurrentFieldID = currentFieldId;
      }
    }
  }
}
