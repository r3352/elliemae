// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.ConsumerManager
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class ConsumerManager : IConsumerManager, IDisposable, IObserver<ISerializer>
  {
    protected ISerializer _serializer;
    protected readonly List<IBasicConsumer> _createdConsumers;
    internal readonly Action<ISerializer> UpdateSerializer;
    private bool _disposed;

    public virtual IMessageHandlerFactory MessageHandlerFactory { get; private set; }

    public ConsumerManager(IMessageHandlerFactory messageHandlerFactory, ISerializer serializer)
    {
      if (messageHandlerFactory == null)
        throw new ArgumentNullException("ConsumerManager(messageHandlerFactory)");
      if (serializer == null)
        throw new ArgumentNullException("ConsumerManager(serializer)");
      this.MessageHandlerFactory = messageHandlerFactory;
      this._serializer = serializer;
      this._createdConsumers = new List<IBasicConsumer>();
      this.UpdateSerializer = (Action<ISerializer>) (s => this._serializer = s);
    }

    public virtual IBasicConsumer CreateConsumer<T>(
      IModel channel,
      string subscriptionName,
      string queueName,
      Action<T> onReceiveMessage,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      ushort? consumerThreadCount)
    {
      IMessageHandler messageHandler1 = this.MessageHandlerFactory.Create<T>(subscriptionName, (Action<T, MessageDeliverEventArgs>) ((msg, evt) => onReceiveMessage(msg)), circuitBreakerPolicyConfig);
      IModel channel1 = channel;
      string queueName1 = queueName;
      IMessageHandler messageHandler2 = messageHandler1;
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig1 = circuitBreakerPolicyConfig;
      ushort? nullable = consumerThreadCount;
      int batchSize = (nullable.GetValueOrDefault() <= (ushort) 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? (int) consumerThreadCount.Value : (int) Global.DefaultConsumerBatchSize;
      RmqConsumer consumer = new RmqConsumer(channel1, queueName1, messageHandler2, circuitBreakerPolicyConfig1, true, batchSize);
      this._createdConsumers.Add((IBasicConsumer) consumer);
      return (IBasicConsumer) consumer;
    }

    public IBasicConsumer CreateAsyncConsumer<T>(
      IModel channel,
      string subscriptionName,
      string queueName,
      Action<T, MessageDeliverEventArgs> onReceiveMessage,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      ushort? consumerThreadCount = null)
    {
      IMessageHandler messageHandler1 = this.MessageHandlerFactory.Create<T>(subscriptionName, onReceiveMessage, circuitBreakerPolicyConfig);
      IModel channel1 = channel;
      string queueName1 = queueName;
      IMessageHandler messageHandler2 = messageHandler1;
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig1 = circuitBreakerPolicyConfig;
      ushort? nullable = consumerThreadCount;
      int batchSize = (nullable.GetValueOrDefault() <= (ushort) 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0 ? (int) consumerThreadCount.Value : (int) Global.DefaultConsumerBatchSize;
      RmqConsumer asyncConsumer = new RmqConsumer(channel1, queueName1, messageHandler2, circuitBreakerPolicyConfig1, false, batchSize);
      this._createdConsumers.Add((IBasicConsumer) asyncConsumer);
      return (IBasicConsumer) asyncConsumer;
    }

    public void ClearConsumers()
    {
      Global.Logger.DebugFormat("Clearing consumer subscriptions");
      this._createdConsumers.OfType<IDisposable>().ToList<IDisposable>().AsParallel<IDisposable>().ForAll<IDisposable>((Action<IDisposable>) (c =>
      {
        try
        {
          c.Dispose();
        }
        catch (Exception ex)
        {
          Global.FaultHandler.HandleFault(ex);
        }
      }));
      this._createdConsumers.Clear();
    }

    public void Dispose()
    {
      if (!this._disposed)
      {
        this.ClearConsumers();
        this.MessageHandlerFactory.Dispose();
      }
      this._disposed = true;
    }

    [ExcludeFromCodeCoverage]
    public void OnNext(ISerializer value) => this._serializer = value;

    [ExcludeFromCodeCoverage]
    public void OnError(Exception error)
    {
    }

    [ExcludeFromCodeCoverage]
    public void OnCompleted()
    {
    }
  }
}
