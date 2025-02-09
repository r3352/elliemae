// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.MessageQueueFramework
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Loggers;
using Elli.MessageQueues.Metrics;
using Elli.MessageQueues.Provision;
using Elli.MessageQueues.Rmq;
using Elli.MessageQueues.Serializers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public sealed class MessageQueueFramework
  {
    private Dictionary<Type, object> _serviceFactories = new Dictionary<Type, object>();
    private MessageQueueType _messageQueueType;

    static MessageQueueFramework() => MessageQueueFramework.Runtime = new MessageQueueFramework();

    public static MessageQueueFramework Runtime { get; private set; }

    private MessageQueueFramework()
    {
    }

    public bool Initialized { get; private set; }

    public void Initialize()
    {
      if (this.Initialized)
        throw new Exception("The MessageQueueFramework has already been initialized");
      if (Global.Serializer == null)
        throw new Exception("Serializer is null");
      if (Global.ConsumerDisposeTimeoutPolicy == null)
        throw new Exception("ConsumerDisposeTimeoutPolicy is null");
      if (Global.SignalFxPolicy == null)
        throw new Exception("SignalFxPolicy is null");
      if (Global.MetricsCollector == null)
        throw new Exception("MetricsCollector is null");
      if (Global.FaultHandler == null)
        throw new Exception("FaultHandler is null");
      this.Initialized = true;
    }

    internal void InitializeForUnitTest()
    {
      this.Initialized = !this.Initialized ? true : throw new Exception("The MessageQueueFramework has already been initialized");
    }

    public MessageQueueFramework UseRmq()
    {
      this._messageQueueType = MessageQueueType.RabbitMQ;
      this.InjectRmqRouteProvisioner();
      return this;
    }

    public MessageQueueFramework Inject<T>(IServiceFactory<T> serviceFactory) where T : class
    {
      if (this.Initialized)
        throw new Exception("All services must be injected prior to calling Initialize");
      this._serviceFactories[typeof (T)] = (object) serviceFactory;
      return this;
    }

    private MessageQueueFramework InjectRmqRouteProvisioner()
    {
      return this.Inject<IRouteProvisioner>((IServiceFactory<IRouteProvisioner>) new RouteProvisionerFactory());
    }

    public MessageQueueFramework InjectConsoleLogger()
    {
      return this.Inject<ILogger>((IServiceFactory<ILogger>) new ConsoleLoggerFactory());
    }

    public MessageQueueFramework InjectJsonMessageSerializer()
    {
      return this.Inject<ISerializer>((IServiceFactory<ISerializer>) new JsonMessageSerializerFactory());
    }

    public MessageQueueFramework InjectDefaultRetryPolicy()
    {
      return this.Inject<IRetryPolicy>((IServiceFactory<IRetryPolicy>) new DefaultRetryPolicyFactory());
    }

    public MessageQueueFramework InjectPlainTextPasswordPolicy()
    {
      return this.Inject<IPasswordPolicy>((IServiceFactory<IPasswordPolicy>) new PlainTextPasswordPolicyFactory());
    }

    public MessageQueueFramework InjectConsumerDisposeTimeoutPolicy(int timeoutInSeconds)
    {
      return this.Inject<IConsumerDisposeTimeoutPolicy>((IServiceFactory<IConsumerDisposeTimeoutPolicy>) new ConsumerDisposeTimeoutPolicyFactory(timeoutInSeconds));
    }

    public MessageQueueFramework InjectDefaultFaultHandler()
    {
      return this.Inject<IFaultHandler>((IServiceFactory<IFaultHandler>) new DefaultFaultHandlerFactory());
    }

    public MessageQueueFramework InjectSignalFxPolicy(bool enable = false)
    {
      return this.Inject<ISignalFxPolicy>((IServiceFactory<ISignalFxPolicy>) new SignalFxPolicyFactory(enable));
    }

    public MessageQueueFramework InjectSignalFxMetrics(
      string contextName,
      bool initializeFromAppConfig = false)
    {
      return this.Inject<IMetricsCollector>((IServiceFactory<IMetricsCollector>) new SignalFxCollectorFactory(contextName, initializeFromAppConfig));
    }

    public MessageQueueFramework InjectDefaultDistributedMutexPolicy()
    {
      return this.Inject<IDistributedMutexPolicy>((IServiceFactory<IDistributedMutexPolicy>) new DefaultDistributedMutexPolicyFactory());
    }

    public MessageQueueFramework InjectHaConnectionRoundRobinPolicy(
      bool useSingleRoundRobinListPerApplicationDomain = false)
    {
      return this.Inject<IHaConnectionRoundRobinPolicy>((IServiceFactory<IHaConnectionRoundRobinPolicy>) new HaConnectionRoundRobinPolicyFactory(useSingleRoundRobinListPerApplicationDomain));
    }

    public T CreateService<T>() where T : class
    {
      IServiceFactory<T> serviceFactory = this.GetServiceFactory<T>();
      return serviceFactory == null ? default (T) : serviceFactory.CreateInstance();
    }

    public IServiceFactory<T> GetServiceFactory<T>() where T : class
    {
      object obj = (object) null;
      return !this._serviceFactories.TryGetValue(typeof (T), out obj) ? (IServiceFactory<T>) null : (IServiceFactory<T>) obj;
    }

    public ITunnel CreateTunnel(string connectionString)
    {
      if (this._messageQueueType == MessageQueueType.RabbitMQ)
        return (ITunnel) RmqTunnel.Factory.Create(connectionString);
      throw new Exception("Unknown Message Queue Type.");
    }

    public void CloseAllConnections()
    {
      if (this._messageQueueType != MessageQueueType.RabbitMQ)
        return;
      RmqTunnel.Factory.CloseAllConnections();
    }

    internal void Dispose()
    {
      this._serviceFactories.Clear();
      this.Initialized = false;
    }
  }
}
