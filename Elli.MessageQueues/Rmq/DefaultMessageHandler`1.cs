// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.DefaultMessageHandler`1
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class DefaultMessageHandler<T> : IMessageHandler
  {
    protected readonly string _typeName = Global.DefaultTypeNameSerializer.Serialize(typeof (T));
    protected readonly string _subscriptionName;
    protected readonly Action<T, MessageDeliverEventArgs> _msgHandlingAction;
    protected readonly IConsumerErrorHandler _consumerErrorHandler;
    protected readonly PollyCircuitBreakerConfig _circuitBreakerPolicyConfig;
    protected readonly ISerializer _messageSerializer;

    public event MessageHandlingEvent HandlingComplete;

    public event MessageWasNotHandledEvent MessageWasNotHandled;

    public DefaultMessageHandler(
      string subscriptionName,
      Action<T, MessageDeliverEventArgs> msgHandlingAction,
      IConsumerErrorHandler consumerErrorHandler,
      PollyCircuitBreakerConfig circuitBreakerPolicy,
      ISerializer messageSerializer)
    {
      if (msgHandlingAction == null)
        throw new ArgumentNullException("DefaultMessageHandler(msgHandlingAction)");
      if (consumerErrorHandler == null)
        throw new ArgumentNullException("DefaultMessageHandler(consumerErrorHandler)");
      if (messageSerializer == null)
        throw new ArgumentNullException("DefaultMessageHandler(messageSerializer)");
      this._subscriptionName = subscriptionName;
      this._consumerErrorHandler = consumerErrorHandler;
      this._circuitBreakerPolicyConfig = circuitBreakerPolicy;
      this._messageSerializer = messageSerializer;
      this._msgHandlingAction = msgHandlingAction;
      this._typeName = Global.DefaultTypeNameSerializer.Serialize(typeof (T));
    }

    protected virtual void BeforeHandlingMessage(BasicDeliverEventArgs eventArg)
    {
    }

    protected virtual void AfterHandlingMessage(BasicDeliverEventArgs eventArg)
    {
    }

    public virtual void HandleError(BasicDeliverEventArgs eventArg, Exception exception)
    {
      Global.FaultHandler.HandleFault(this.BuildErrorLogMessage(eventArg, exception));
      this._consumerErrorHandler.HandleError(eventArg, exception);
    }

    [ExcludeFromCodeCoverage]
    protected virtual string BuildErrorLogMessage(
      BasicDeliverEventArgs basicDeliverEventArgs,
      Exception exception)
    {
      Encoding.UTF8.GetString(basicDeliverEventArgs.Body);
      (basicDeliverEventArgs.BasicProperties as BasicProperties)?.AppendPropertyDebugStringTo(new StringBuilder());
      return "Exception thrown by subscription calback.\n\tExchange:    '{basicDeliverEventArgs.Exchange}'\n\tRouting Key: '{basicDeliverEventArgs.RoutingKey}'\n\tRedelivered: '{basicDeliverEventArgs.Redelivered}'\n Message:\n{message}\n BasicProperties:\n{propertiesMessage}\n Exception:\n{exception}\n";
    }

    public void HandleMessage(BasicDeliverEventArgs eventArgs)
    {
      bool msgHandled = false;
      try
      {
        this.BeforeHandlingMessage(eventArgs);
        if (this._circuitBreakerPolicyConfig != null && this._circuitBreakerPolicyConfig.CircuitBreakerPolicy != null)
        {
          if (this._circuitBreakerPolicyConfig.CircuitBreakerPolicy.CircuitState != CircuitState.Open)
          {
            try
            {
              this._circuitBreakerPolicyConfig.CircuitBreakerPolicy.Execute((Action) (() => this.HandleMessage(eventArgs, out msgHandled)));
              return;
            }
            catch (BrokenCircuitException ex)
            {
              Global.FaultHandler.HandleFault("BrokenCircuitException occured. {0}, details: {1}", (object) ex.Message, (object) ex.StackTrace);
              MessageUnhandledEventArgs unhandledEventArgs = new MessageUnhandledEventArgs();
              unhandledEventArgs.ConsumerTag = eventArgs.ConsumerTag;
              unhandledEventArgs.DeliveryTag = eventArgs.DeliveryTag;
              unhandledEventArgs.SubscriptionName = this._subscriptionName;
              unhandledEventArgs.Exchange = eventArgs.Exchange;
              unhandledEventArgs.RoutingKey = eventArgs.RoutingKey;
              unhandledEventArgs.Redelivered = eventArgs.Redelivered;
              unhandledEventArgs.DeathCount = MessageUtility.GetDeathCount(eventArgs);
              unhandledEventArgs.Headers = eventArgs.BasicProperties.IsHeadersPresent() ? eventArgs.BasicProperties.Headers : (IDictionary<string, object>) new Dictionary<string, object>();
              unhandledEventArgs.Message = (object) this._messageSerializer.Deserialize<T>(eventArgs.Body);
              MessageUnhandledEventArgs messageUnhandledEventArgs = unhandledEventArgs;
              this._circuitBreakerPolicyConfig.TriggerOnBrokenCircuitException(new PollyBrokenCircuitEventArgs(msgHandled, messageUnhandledEventArgs, ex));
              return;
            }
          }
        }
        this.HandleMessage(eventArgs, out msgHandled);
      }
      catch (Exception ex1)
      {
        Global.FaultHandler.HandleFault(ex1);
        if (!(ex1 is CircuitBreakerPolicyException))
        {
          try
          {
            this.HandleError(eventArgs, ex1);
          }
          catch (Exception ex2)
          {
            Global.FaultHandler.HandleFault("Failed to handle the exception: {0} because of {1}", (object) ex1.Message, (object) ex2.StackTrace);
          }
        }
        else
          msgHandled = true;
      }
      finally
      {
        this.CleanUp(eventArgs, msgHandled);
      }
    }

    protected virtual void HandleMessage(BasicDeliverEventArgs eventArgs, out bool msgHandled)
    {
      this.CheckMessageType(eventArgs.BasicProperties);
      this._msgHandlingAction(this._messageSerializer.Deserialize<T>(eventArgs.Body), new MessageDeliverEventArgs()
      {
        ConsumerTag = eventArgs.ConsumerTag,
        DeliveryTag = eventArgs.DeliveryTag,
        SubscriptionName = this._subscriptionName,
        Exchange = eventArgs.Exchange,
        RoutingKey = eventArgs.RoutingKey,
        Redelivered = eventArgs.Redelivered,
        DeathCount = MessageUtility.GetDeathCount(eventArgs),
        Headers = eventArgs.BasicProperties.IsHeadersPresent() ? eventArgs.BasicProperties.Headers : (IDictionary<string, object>) new Dictionary<string, object>()
      });
      msgHandled = true;
    }

    internal void CleanUp(BasicDeliverEventArgs eventArgs, bool msgHandled)
    {
      if (!msgHandled)
      {
        if (this.MessageWasNotHandled != null)
        {
          try
          {
            this.MessageWasNotHandled(eventArgs);
          }
          catch (Exception ex)
          {
            Global.FaultHandler.HandleFault("There is an error when trying to fire MessageWasNotDelivered event");
            Global.Logger.Error(ex);
          }
        }
      }
      try
      {
        this.AfterHandlingMessage(eventArgs);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault("There is an error when trying to call AfterHandlingMessage method - {0}", (object) ex.Message);
      }
      if (this.HandlingComplete == null)
        return;
      try
      {
        this.HandlingComplete(eventArgs);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault("There is an error when trying to fire HandlingComplete event {0}", (object) ex.Message);
      }
    }

    protected void CheckMessageType(IBasicProperties properties)
    {
      if (properties.Type != this._typeName)
      {
        string str = string.Format("Message type is incorrect. Expected '{0}', but was '{1}'", (object) this._typeName, (object) properties.Type);
        Global.FaultHandler.HandleFault(str);
        throw new Exception(str);
      }
    }
  }
}
