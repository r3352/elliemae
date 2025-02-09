// Decompiled with JetBrains decompiler
// Type: Elli.Service.Common.ClientConfiguration
// Assembly: Elli.Service.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: C15A70DC-1690-48C1-BCA7-AD8CA880845A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Service.Common.dll

using Elli.Service.Common.Caching;
using Elli.Service.Common.Caching.Timers;
using Elli.Service.Common.InversionOfControl;
using Elli.Service.Common.WCF;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Elli.Service.Common
{
  public class ClientConfiguration
  {
    private readonly List<Assembly> _requestsAndResponseAssemblies = new List<Assembly>();
    private readonly Container _container;

    public Type RequestDispatcherImplementation { get; set; }

    public Type RequestDispatcherFactoryImplementation { get; set; }

    public Type RequestProcessorImplementation { get; set; }

    public Type AsyncRequestDispatcherImplementation { get; set; }

    public Type AsyncRequestDispatcherFactoryImplementation { get; set; }

    public Type AsyncRequestProcessorImplementation { get; set; }

    public Type ContainerImplementation { get; private set; }

    public Type CacheProviderImplementation { get; set; }

    public Type CacheManagerImplementation { get; set; }

    public ClientConfiguration(Container container)
    {
      this._container = container;
      this.SetDefaultImplementations();
    }

    public ClientConfiguration(Type containerImplementation)
    {
      this.ContainerImplementation = containerImplementation;
      this.SetDefaultImplementations();
    }

    public ClientConfiguration(Assembly requestsAndResponsesAssembly, Container container)
      : this(container)
    {
      this.AddRequestAndResponseAssembly(requestsAndResponsesAssembly);
    }

    public ClientConfiguration(Assembly requestsAndResponsesAssembly, Type containerImplementation)
      : this(containerImplementation)
    {
      this.AddRequestAndResponseAssembly(requestsAndResponsesAssembly);
    }

    public ClientConfiguration AddRequestAndResponseAssembly(Assembly assembly)
    {
      this._requestsAndResponseAssemblies.Add(assembly);
      return this;
    }

    private void SetDefaultImplementations()
    {
      this.RequestDispatcherImplementation = typeof (RequestDispatcher);
      this.RequestDispatcherFactoryImplementation = typeof (RequestDispatcherFactory);
      this.RequestProcessorImplementation = typeof (RequestProcessorProxy);
      this.AsyncRequestDispatcherImplementation = typeof (AsyncRequestDispatcher);
      this.AsyncRequestDispatcherFactoryImplementation = typeof (AsyncRequestDispatcherFactory);
      this.AsyncRequestProcessorImplementation = typeof (AsyncRequestProcessorProxy);
      this.CacheManagerImplementation = typeof (CacheManager);
      this.CacheProviderImplementation = typeof (InMemoryCacheProvider);
    }

    public void Initialize()
    {
      if (IoC.Container == null)
        IoC.Container = this._container ?? (Container) Activator.CreateInstance(this.ContainerImplementation);
      IoC.Container.Register(typeof (IRequestProcessor), this.RequestProcessorImplementation, Lifestyle.Transient);
      IoC.Container.Register(typeof (IRequestDispatcher), this.RequestDispatcherImplementation, Lifestyle.Transient);
      IoC.Container.Register(typeof (IRequestDispatcherFactory), this.RequestDispatcherFactoryImplementation, Lifestyle.Singleton);
      IoC.Container.Register(typeof (IAsyncRequestProcessor), this.AsyncRequestProcessorImplementation, Lifestyle.Transient);
      IoC.Container.Register(typeof (IAsyncRequestDispatcher), this.AsyncRequestDispatcherImplementation, Lifestyle.Transient);
      IoC.Container.Register(typeof (IAsyncRequestDispatcherFactory), this.AsyncRequestDispatcherFactoryImplementation, Lifestyle.Singleton);
      IoC.Container.Register(typeof (ICacheProvider), this.CacheProviderImplementation, Lifestyle.Singleton);
      IoC.Container.Register(typeof (ICacheManager), this.CacheManagerImplementation, Lifestyle.Singleton);
      IoC.Container.Register<ITimerProvider, TimerProvider>(Lifestyle.Singleton);
      this.RegisterRequestAndResponseTypes();
      this.ConfigureCachingLayer();
    }

    private void ConfigureCachingLayer()
    {
      IoC.Container.RegisterSingle<CacheConfiguration>((CacheConfiguration) new ClientCacheConfiguration(this._requestsAndResponseAssemblies.SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (a => (IEnumerable<Type>) a.GetTypes())).Where<Type>((Func<Type, bool>) (t => !t.IsAbstract && t.IsSubclassOf(typeof (Request))))));
    }

    private void RegisterRequestAndResponseTypes()
    {
      foreach (Assembly responseAssembly in this._requestsAndResponseAssemblies)
      {
        KnownTypeProvider.RegisterDerivedTypesOf<Request>(responseAssembly);
        KnownTypeProvider.RegisterDerivedTypesOf<Response>(responseAssembly);
      }
    }
  }
}
