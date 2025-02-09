// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.DefaultMessageHandlerFactory
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using System;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class DefaultMessageHandlerFactory : 
    IMessageHandlerFactory,
    IDisposable,
    IObserver<ISerializer>
  {
    protected readonly IConsumerErrorHandler _consumerErrorHandler;
    protected ISerializer _messageSerializer;

    public DefaultMessageHandlerFactory(
      IConsumerErrorHandler consumerErrorHandler,
      ISerializer messageSerializer)
    {
      if (consumerErrorHandler == null)
        throw new ArgumentNullException("DefaultMessageHandlerFactory(consumerErrorHandler)");
      if (messageSerializer == null)
        throw new ArgumentNullException("DefaultMessageHandlerFactory(messageSerializer)");
      this._consumerErrorHandler = consumerErrorHandler;
      this._messageSerializer = messageSerializer;
    }

    public virtual IMessageHandler Create<T>(
      string subscriptionName,
      Action<T, MessageDeliverEventArgs> msgHandlingAction,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig)
    {
      return (IMessageHandler) new DefaultMessageHandler<T>(subscriptionName, msgHandlingAction, this._consumerErrorHandler, circuitBreakerPolicyConfig, this._messageSerializer);
    }

    public void Dispose() => this._consumerErrorHandler.Dispose();

    [ExcludeFromCodeCoverage]
    public void OnNext(ISerializer value) => this._messageSerializer = value;

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
