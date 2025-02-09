// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.PerformanceContext
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class PerformanceContext : ILogicalThreadAffinative
  {
    public const string Key = "Performance�";

    public PerformanceContext(string app, string assembly, APICallSourceType sourceType)
    {
      this.SourceApp = app;
      this.SourceAssembly = assembly;
      this.SourceType = sourceType;
    }

    public string SourceApp { get; set; }

    public string SourceAssembly { get; set; }

    public APICallSourceType SourceType { get; set; }

    public static void Apply(string app, string assembly, APICallSourceType sourceType)
    {
    }

    [Serializable]
    public sealed class PerformanceAttribute : OnMethodBoundaryAspect
    {
      private readonly string category;
      [NonSerialized]
      private string methodName;

      public PerformanceAttribute()
      {
      }

      public PerformanceAttribute(string category) => this.category = category;

      public virtual void RuntimeInitialize(MethodBase method)
      {
        this.methodName = method.DeclaringType.Name + "." + method.Name;
      }

      [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers | MethodExecutionAdviceOptimizations.IgnoreEventArgs)]
      public override void OnEntry(MethodExecutionArgs args)
      {
        PerformanceMeter.Current.AddCheckpoint("START:" + this.methodName, 80, nameof (OnEntry), "D:\\ws\\24.3.0.0\\EmLite\\ClientServer\\Classes\\PerformanceContext.cs");
      }

      [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers | MethodExecutionAdviceOptimizations.IgnoreEventArgs)]
      public override void OnExit(MethodExecutionArgs args)
      {
        PerformanceMeter.Current.AddCheckpoint("STOPPED:" + this.methodName, 88, nameof (OnExit), "D:\\ws\\24.3.0.0\\EmLite\\ClientServer\\Classes\\PerformanceContext.cs");
      }
    }
  }
}
