// Decompiled with JetBrains decompiler
// Type: Elli.Common.Aspects.Cache.Cache
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using System;
using System.Reflection;

#nullable disable
namespace Elli.Common.Aspects.Cache
{
  public static class Cache
  {
    [Serializable]
    public class TriggerInvalidation : OnMethodBoundaryAspect
    {
      private KeyBuilder _keyBuilder;

      public KeyBuilder KeyBuilder => this._keyBuilder ?? (this._keyBuilder = new KeyBuilder());

      public TriggerInvalidation(
        string groupName,
        CacheSettings settings,
        string parameterProperty)
      {
        this.KeyBuilder.GroupName = groupName;
        this.KeyBuilder.Settings = settings;
        this.KeyBuilder.ParameterProperty = parameterProperty;
      }

      public TriggerInvalidation(string groupName, CacheSettings settings)
        : this(groupName, settings, string.Empty)
      {
      }

      public TriggerInvalidation(string groupName)
        : this(groupName, CacheSettings.Default, string.Empty)
      {
      }

      public TriggerInvalidation()
        : this(string.Empty)
      {
      }

      public virtual void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
      {
        this.KeyBuilder.MethodParameters = method.GetParameters();
        this.KeyBuilder.MethodName = string.Format("{0}.{1}", (object) method.DeclaringType.FullName, (object) method.Name);
      }

      [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.IgnoreAllEventArgsMembers)]
      public override sealed void OnExit(MethodExecutionArgs args) => base.OnExit(args);
    }
  }
}
