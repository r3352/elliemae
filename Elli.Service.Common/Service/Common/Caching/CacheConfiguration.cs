// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.Caching.CacheConfiguration
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Elli.Service.Common.Caching
{
  public abstract class CacheConfiguration
  {
    private Dictionary<Type, RequestCacheConfiguration> requestCacheConfigurations;
    private readonly Type enableResponseCachingAttributeType;

    protected CacheConfiguration(
      IEnumerable<Type> knownRequestTypes,
      Type enableResponseCachingAttributeType)
    {
      this.enableResponseCachingAttributeType = enableResponseCachingAttributeType;
      this.BuildMapOfConfigurationsForRequestsThatEnabledResponseCaching(knownRequestTypes);
    }

    private void BuildMapOfConfigurationsForRequestsThatEnabledResponseCaching(
      IEnumerable<Type> knownRequestTypes)
    {
      this.requestCacheConfigurations = new Dictionary<Type, RequestCacheConfiguration>();
      foreach (Type knownRequestType in knownRequestTypes)
      {
        object customAttribute = (object) Attribute.GetCustomAttribute((MemberInfo) knownRequestType, this.enableResponseCachingAttributeType);
        // ISSUE: reference to a compiler-generated field
        if (CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (CacheConfiguration), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, bool> target1 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__1.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, bool>> p1 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__1;
        // ISSUE: reference to a compiler-generated field
        if (CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__0 == null)
        {
          // ISSUE: reference to a compiler-generated field
          CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__0 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (CacheConfiguration), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj1 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__0.Target((CallSite) CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__0, customAttribute, (object) null);
        if (target1((CallSite) p1, obj1))
        {
          Dictionary<Type, RequestCacheConfiguration> cacheConfigurations = this.requestCacheConfigurations;
          Type key = knownRequestType;
          // ISSUE: reference to a compiler-generated field
          if (CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__4 = CallSite<Func<CallSite, Type, Type, object, object, RequestCacheConfiguration>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeConstructor(CSharpBinderFlags.None, typeof (CacheConfiguration), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, Type, Type, object, object, RequestCacheConfiguration> target2 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__4.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, Type, Type, object, object, RequestCacheConfiguration>> p4 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__4;
          Type type1 = typeof (RequestCacheConfiguration);
          Type type2 = knownRequestType;
          // ISSUE: reference to a compiler-generated field
          if (CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__2 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__2 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Expiration", typeof (CacheConfiguration), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj2 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__2.Target((CallSite) CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__2, customAttribute);
          // ISSUE: reference to a compiler-generated field
          if (CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__3 == null)
          {
            // ISSUE: reference to a compiler-generated field
            CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "Region", typeof (CacheConfiguration), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__3.Target((CallSite) CacheConfiguration.\u003C\u003Eo__3.\u003C\u003Ep__3, customAttribute);
          RequestCacheConfiguration cacheConfiguration = target2((CallSite) p4, type1, type2, obj2, obj3);
          cacheConfigurations.Add(key, cacheConfiguration);
        }
      }
    }

    public bool IsCachingEnabledFor(Type requestType)
    {
      return this.requestCacheConfigurations.ContainsKey(requestType);
    }

    public RequestCacheConfiguration GetConfigurationFor(Type requestType)
    {
      return this.requestCacheConfigurations[requestType];
    }

    public IEnumerable<string> GetRegionNames()
    {
      return (IEnumerable<string>) new HashSet<string>(this.requestCacheConfigurations.Values.Where<RequestCacheConfiguration>((Func<RequestCacheConfiguration, bool>) (v => !string.IsNullOrEmpty(v.Region))).Select<RequestCacheConfiguration, string>((Func<RequestCacheConfiguration, string>) (v => v.Region)));
    }

    public string GetRegionNameFor(Type requestType)
    {
      return this.requestCacheConfigurations[requestType].Region;
    }
  }
}
