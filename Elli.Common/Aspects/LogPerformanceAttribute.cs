// Decompiled with JetBrains decompiler
// Type: Elli.Common.Aspects.LogPerformanceAttribute
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using Elli.Common.Diagnostics;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Elli.Common.Aspects
{
  [AttributeUsage(AttributeTargets.Method)]
  [Serializable]
  public sealed class LogPerformanceAttribute : OnMethodBoundaryAspect
  {
    private DateTime _startTime;

    [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers)]
    public override void OnEntry(MethodExecutionArgs args)
    {
      this._startTime = DateTime.Now;
      base.OnEntry(args);
    }

    [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreMethodExecutionTag | MethodExecutionAdviceOptimizations.IgnoreSetFlowBehavior | MethodExecutionAdviceOptimizations.IgnoreSetArguments | MethodExecutionAdviceOptimizations.IgnoreGetInstance | MethodExecutionAdviceOptimizations.IgnoreSetInstance | MethodExecutionAdviceOptimizations.IgnoreGetException | MethodExecutionAdviceOptimizations.IgnoreGetReturnValue | MethodExecutionAdviceOptimizations.IgnoreSetReturnValue | MethodExecutionAdviceOptimizations.IgnoreGetYieldValue | MethodExecutionAdviceOptimizations.IgnoreSetYieldValue | MethodExecutionAdviceOptimizations.IgnoreGetDeclarationIdentifier)]
    public override void OnExit(MethodExecutionArgs args)
    {
      double totalMilliseconds = (DateTime.Now - this._startTime).TotalMilliseconds;
      string str1 = args.Method.DeclaringType != (Type) null ? args.Method.DeclaringType.Name : string.Empty;
      string str2 = "";
      bool flag = true;
      foreach (object obj in args.Arguments)
      {
        if (!flag)
          str2 += ",";
        else
          flag = false;
        if (obj != null && obj.GetType() == typeof (string))
          str2 = str2 + "'" + (string) obj + "'";
      }
      MethodPerformanceLogger.Log(string.Format("Method [{0}.{1}] ~~ {2}ms on Process {3} Thread {4}: Total {5} Args, Display String Value Only - ({6})", (object) str1, (object) args.Method.Name, (object) totalMilliseconds, (object) Process.GetCurrentProcess().Id, (object) Thread.CurrentThread.ManagedThreadId, (object) args.Arguments.Count, (object) str2));
      base.OnExit(args);
    }
  }
}
