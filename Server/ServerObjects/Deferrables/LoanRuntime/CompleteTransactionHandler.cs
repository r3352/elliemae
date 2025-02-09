// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime.CompleteTransactionHandler
// Assembly: Server, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 4B6E360F-802A-47E0-97B9-9D6935EA0DD1
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Server.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace EllieMae.EMLite.Server.ServerObjects.Deferrables.LoanRuntime
{
  internal sealed class CompleteTransactionHandler : TransactionAwareBase
  {
    private readonly List<object> _preDefinedProcessorList = new List<object>();

    public CompleteTransactionHandler(DeferrableLoanTransaction transaction)
      : base(transaction)
    {
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IPreProcessor>());
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IAlertNotificationProcessor>());
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IServiceOrderProcessor>());
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IReportingDbProcessor>());
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IAuditTrailProcessor>());
      this._preDefinedProcessorList.Add((object) new CompleteTransactionHandler.ProcessorCreator<IPostProcessor>());
    }

    public void Handle()
    {
      foreach (object definedProcessor in this._preDefinedProcessorList)
      {
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, DeferrableLoanTransaction, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CreateInstance", (IEnumerable<Type>) null, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__0.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__0, definedProcessor, this.CurrentTransaction);
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__2.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p2 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__2;
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj2 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__1.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__1, obj1, (object) null);
        if (target((CallSite) p2, obj2))
        {
          // ISSUE: reference to a compiler-generated field
          if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__3 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__3.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__3, obj1);
        }
        if (this.CurrentTransaction.SwitchedToRealTimeMode)
          break;
      }
      if (!this.CurrentTransaction.SwitchedToRealTimeMode)
        return;
      foreach (object definedProcessor in this._preDefinedProcessorList)
      {
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__4 = CallSite<Func<CallSite, object, DeferrableLoanTransaction, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "CreateInstance", (IEnumerable<Type>) null, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj3 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__4.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__4, definedProcessor, this.CurrentTransaction);
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__9 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__9.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p9 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__9;
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj4 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__5.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__5, obj3, (object) null);
        // ISSUE: reference to a compiler-generated field
        if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        object obj5;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__8.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__8, obj4))
        {
          // ISSUE: reference to a compiler-generated field
          if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__7 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target2 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__7.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> p7 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__7;
          object obj6 = obj4;
          // ISSUE: reference to a compiler-generated field
          if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__6 = CallSite<Func<CallSite, DeferrableLoanTransaction, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "NeedToRunRealTimeProcessor", (IEnumerable<Type>) null, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj7 = CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__6.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__6, this.CurrentTransaction, obj3);
          obj5 = target2((CallSite) p7, obj6, obj7);
        }
        else
          obj5 = obj4;
        if (target1((CallSite) p9, obj5))
        {
          // ISSUE: reference to a compiler-generated field
          if (CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__10 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__10 = CallSite<Action<CallSite, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Execute", (IEnumerable<Type>) null, typeof (CompleteTransactionHandler), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__10.Target((CallSite) CompleteTransactionHandler.\u003C\u003Eo__2.\u003C\u003Ep__10, obj3);
        }
      }
    }

    private class ProcessorCreator<T> where T : class
    {
      public T CreateInstance(DeferrableLoanTransaction transaction)
      {
        IDeferrableProcessorFactory<T> processorFactory = transaction.GetProcessorFactory<T>();
        return processorFactory == null ? default (T) : processorFactory.CreateInstance();
      }
    }
  }
}
