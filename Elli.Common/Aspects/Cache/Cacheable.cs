// Decompiled with JetBrains decompiler
// Type: Elli.Common.Aspects.Cache.Cacheable
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Cache;
using PostSharp.Aspects;
using PostSharp.Aspects.Internals;
using System;
using System.Reflection;

#nullable disable
namespace Elli.Common.Aspects.Cache
{
  [Serializable]
  public class Cacheable : OnMethodBoundaryAspect
  {
    private readonly Lazy<KeyBuilder> _keyBuilder = new Lazy<KeyBuilder>((Func<KeyBuilder>) (() => new KeyBuilder()));

    public KeyBuilder KeyBuilder => this._keyBuilder.Value;

    public Cacheable(string groupName, CacheSettings settings, string parameterProperty)
    {
      this.KeyBuilder.GroupName = groupName;
      this.KeyBuilder.Settings = settings;
      this.KeyBuilder.ParameterProperty = parameterProperty;
    }

    public Cacheable(string groupName, CacheSettings settings)
      : this(groupName, settings, string.Empty)
    {
    }

    public Cacheable(string groupName)
      : this(groupName, CacheSettings.Default)
    {
    }

    public Cacheable()
      : this(string.Empty)
    {
    }

    public virtual void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
    {
      this.KeyBuilder.MethodParameters = method.GetParameters();
      this.KeyBuilder.MethodName = string.Format("{0}.{1}", method.DeclaringType != (Type) null ? (object) method.DeclaringType.FullName : (object) string.Empty, (object) method.Name);
    }

    [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.None)]
    public override sealed void OnEntry(MethodExecutionArgs args)
    {
      IDataCache dataCache = this.CreateDataCache(args);
      if (dataCache == null)
        return;
      string key = this.KeyBuilder.BuildCacheKey(args.Instance, args.Arguments);
      DateWrapper<object> dateWrapper = dataCache.Get<DateWrapper<object>>(key);
      if (dateWrapper != null)
      {
        args.ReturnValue = dateWrapper.Object;
        args.FlowBehavior = FlowBehavior.Return;
      }
      else
        args.MethodExecutionTag = (object) key;
    }

    [MethodExecutionAdviceOptimization(MethodExecutionAdviceOptimizations.None)]
    public override sealed void OnSuccess(MethodExecutionArgs args)
    {
      IDataCache dataCache = this.CreateDataCache(args);
      if (dataCache == null)
        return;
      string methodExecutionTag = (string) args.MethodExecutionTag;
      DateWrapper<object> dateWrapper = new DateWrapper<object>()
      {
        Object = args.ReturnValue,
        Timestamp = DateTime.UtcNow
      };
      dataCache.Set<DateWrapper<object>>(methodExecutionTag, dateWrapper);
    }

    protected virtual IDataCache CreateDataCache(MethodExecutionArgs args)
    {
      return ((IDataCacheable) args.Instance)?.DataCache;
    }
  }
}
