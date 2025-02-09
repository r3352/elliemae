// Decompiled with JetBrains decompiler
// Type: Encompass.Diagnostics.Config.ConfigDataSection
// Assembly: Encompass.Diagnostics, Version=1.0.0.1, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: E8A3B074-7BF0-4187-B0D2-083265232A16
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Diagnostics.dll

using System;
using System.Collections.Generic;
using Unity;
using Unity.Injection;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.TransparentProxyInterception;
using Unity.Interception.PolicyInjection;

#nullable disable
namespace Encompass.Diagnostics.Config
{
  public class ConfigDataSection : MarshalByRefObject, IConfigDataSection
  {
    private static readonly UnityContainer _container;
    private static readonly InjectionMember[] _injectionMembers = new InjectionMember[2]
    {
      (InjectionMember) new Interceptor<TransparentProxyInterceptor>(),
      (InjectionMember) new InterceptionBehavior<PolicyInjectionBehavior>()
    };
    private readonly Dictionary<string, object> values = new Dictionary<string, object>();

    static ConfigDataSection()
    {
      ConfigDataSection._container = new UnityContainer();
      ConfigDataSection._container.AddNewExtension<Unity.Interception.Interception>();
    }

    public static T NewInstance<T>() where T : ConfigDataSection
    {
      if (!ConfigDataSection._container.IsRegistered<T>())
        ConfigDataSection._container.RegisterType<T>(ConfigDataSection._injectionMembers);
      return ConfigDataSection._container.Resolve<T>();
    }

    bool IConfigDataSection.TryGetConfigValue(string propertyName, out object value)
    {
      return this.values.TryGetValue(propertyName, out value);
    }

    void IConfigDataSection.SetConfigValue(string propertyName, object value)
    {
      this.values[propertyName] = value;
    }
  }
}
