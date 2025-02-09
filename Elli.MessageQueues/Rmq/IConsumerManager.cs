// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.IConsumerManager
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using System;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  public interface IConsumerManager : IDisposable
  {
    IMessageHandlerFactory MessageHandlerFactory { get; }

    IBasicConsumer CreateConsumer<T>(
      IModel channel,
      string subscriptionName,
      string queueName,
      Action<T> onReceiveMessage,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      ushort? consumerThreadCount = null);

    IBasicConsumer CreateAsyncConsumer<T>(
      IModel channel,
      string subscriptionName,
      string queueName,
      Action<T, MessageDeliverEventArgs> onReceiveMessage,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      ushort? consumerThreadCount = null);

    void ClearConsumers();
  }
}
