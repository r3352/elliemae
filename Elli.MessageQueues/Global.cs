// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Global
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Loggers;
using Elli.MessageQueues.Metrics;
using Elli.MessageQueues.Serializers;
using Elli.MessageQueues.Utils;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

#nullable disable
namespace Elli.MessageQueues
{
  [ExcludeFromCodeCoverage]
  public static class Global
  {
    private static readonly DefaultTaskCreationOptionProvider GlobalDefaultTaskCreationOptionProvider = new DefaultTaskCreationOptionProvider();
    private static ISerializer _serializer;
    private static ILogger _logger;
    private static IConsumerDisposeTimeoutPolicy _consumerDisposeTimeoutPolicy;
    private static ISignalFxPolicy _signalFxPolicy;
    private static IMetricsCollector _metricsCollector;
    private static IDistributedMutexPolicy _distributedMutexPolicy;
    private static IFaultHandler _faultHandler;
    private static IPasswordPolicy _passwordPolicy;
    public static ICorrelationIdGenerator DefaultCorrelationIdGenerator = (ICorrelationIdGenerator) new SimpleCorrelationIdGenerator();
    public static ITypeNameSerializer DefaultTypeNameSerializer = (ITypeNameSerializer) new TypeNameSerializer();
    public static Func<TaskCreationOptions> DefaultTaskCreationOptionsProvider = (Func<TaskCreationOptions>) (() => Global.GlobalDefaultTaskCreationOptionProvider.GetOptions());
    public static ushort DefaultConsumerBatchSize = 4;
    public static uint PreFetchCount = 128;
    public static bool DefaultPersistentMode = true;
    public static string DefaultErrorQueueName = "Elli.Queue.Error";
    public static string DefaultErrorExchangeName = "Elli.Exchange.Error";
    internal static Func<TaskContinuationOptions> DefaultTaskContinuationOptionsProvider = (Func<TaskContinuationOptions>) (() => (Global.DefaultTaskCreationOptionsProvider() & TaskCreationOptions.LongRunning) <= TaskCreationOptions.None ? TaskContinuationOptions.PreferFairness : TaskContinuationOptions.LongRunning);

    public static ISerializer Serializer
    {
      get
      {
        if (Global._serializer != null)
          return Global._serializer;
        Global._serializer = MessageQueueFramework.Runtime.CreateService<ISerializer>();
        return Global._serializer;
      }
      internal set => Global._serializer = value;
    }

    public static ILogger Logger
    {
      get
      {
        if (Global._logger != null)
          return Global._logger;
        Global._logger = MessageQueueFramework.Runtime.CreateService<ILogger>();
        return Global._logger;
      }
      internal set => Global._logger = value;
    }

    public static IConsumerDisposeTimeoutPolicy ConsumerDisposeTimeoutPolicy
    {
      get
      {
        if (Global._consumerDisposeTimeoutPolicy != null)
          return Global._consumerDisposeTimeoutPolicy;
        Global._consumerDisposeTimeoutPolicy = MessageQueueFramework.Runtime.CreateService<IConsumerDisposeTimeoutPolicy>();
        return Global._consumerDisposeTimeoutPolicy;
      }
      internal set => Global._consumerDisposeTimeoutPolicy = value;
    }

    public static ISignalFxPolicy SignalFxPolicy
    {
      get
      {
        if (Global._signalFxPolicy != null)
          return Global._signalFxPolicy;
        Global._signalFxPolicy = MessageQueueFramework.Runtime.CreateService<ISignalFxPolicy>();
        return Global._signalFxPolicy;
      }
      internal set => Global._signalFxPolicy = value;
    }

    public static IMetricsCollector MetricsCollector
    {
      get
      {
        if (Global._metricsCollector != null)
          return Global._metricsCollector;
        Global._metricsCollector = MessageQueueFramework.Runtime.CreateService<IMetricsCollector>();
        return Global._metricsCollector;
      }
      internal set => Global._metricsCollector = value;
    }

    public static IDistributedMutexPolicy DistributedMutexPolicy
    {
      get
      {
        if (Global._distributedMutexPolicy != null)
          return Global._distributedMutexPolicy;
        Global._distributedMutexPolicy = MessageQueueFramework.Runtime.CreateService<IDistributedMutexPolicy>();
        return Global._distributedMutexPolicy;
      }
      internal set => Global._distributedMutexPolicy = value;
    }

    public static IFaultHandler FaultHandler
    {
      get
      {
        if (Global._faultHandler != null)
          return Global._faultHandler;
        Global._faultHandler = MessageQueueFramework.Runtime.CreateService<IFaultHandler>();
        return Global._faultHandler;
      }
      internal set => Global._faultHandler = value;
    }

    public static IPasswordPolicy PasswordPolicy
    {
      get
      {
        if (Global._passwordPolicy != null)
          return Global._passwordPolicy;
        Global._passwordPolicy = MessageQueueFramework.Runtime.CreateService<IPasswordPolicy>();
        return Global._passwordPolicy;
      }
      internal set => Global._passwordPolicy = value;
    }

    static Global()
    {
      TaskScheduler.UnobservedTaskException += new EventHandler<UnobservedTaskExceptionEventArgs>(Global.TaskSchedulerUnobservedTaskException);
    }

    private static void TaskSchedulerUnobservedTaskException(
      object sender,
      UnobservedTaskExceptionEventArgs e)
    {
      foreach (Exception innerException in e.Exception.InnerExceptions)
        Global.Logger.Error(innerException);
    }
  }
}
