// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.RmqSetup
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Rmq.Connections;
using Elli.MessageQueues.Setup;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  [ExcludeFromCodeCoverage]
  internal class RmqSetup
  {
    private readonly object _syncObj = new object();
    private readonly string _connectionString;
    private IDurableConnection _connection;

    protected IDurableConnection DurableConnection
    {
      get
      {
        if (this._connection == null)
          this._connection = this.CreateConnection();
        return this._connection;
      }
      set => this._connection = value;
    }

    public RmqSetup(string connectionString) => this._connectionString = connectionString;

    private IDurableConnection CreateConnection()
    {
      lock (this._syncObj)
      {
        if (this._connection == null || !this._connection.IsConnected)
        {
          List<ManagedConnectionFactory> list = ((IEnumerable<string>) this._connectionString.Split(new char[1]
          {
            '|'
          }, StringSplitOptions.RemoveEmptyEntries)).Select<string, ManagedConnectionFactory>((Func<string, ManagedConnectionFactory>) (x => new ManagedConnectionFactory(new ConnectionString(x)))).ToList<ManagedConnectionFactory>();
          this._connection = (IDurableConnection) new HaConnection(MessageQueueFramework.Runtime.CreateService<IRetryPolicy>(), (IList<ManagedConnectionFactory>) list);
          this._connection.Connect();
        }
        return this._connection;
      }
    }

    public void CreateRoute<T>(RouteSetupData routeSetupData)
    {
      string queueName = routeSetupData.RouteFinder.FindQueueName<T>(routeSetupData.SubscriptionName);
      string exchangeName = routeSetupData.RouteFinder.FindExchangeName<T>();
      string routingKey = routeSetupData.RouteFinder.FindRoutingKey<T>();
      string toQueueRoutingKey1 = routeSetupData.BindExchangeToQueueRoutingKey;
      string deadLetterExchange = routeSetupData.QueueSetupData.DeadLetterExchange;
      string deadLetterQueue = routeSetupData.QueueSetupData.DeadLetterQueue;
      string letterRoutingKey = routeSetupData.QueueSetupData.DeadLetterRoutingKey;
      int messageTimeToLive = routeSetupData.QueueSetupData.DeadLetterMessageTimeToLive;
      if (!string.IsNullOrWhiteSpace(deadLetterExchange) && !string.IsNullOrWhiteSpace(deadLetterQueue))
        this.CreateDeadLetterRoute<T>(queueName, exchangeName, routeSetupData.ExchangeSetupData.ExchangeType, routingKey, routeSetupData.QueueSetupData.DeadLetterRoutingKeyForOriginal, deadLetterExchange, deadLetterQueue, letterRoutingKey, messageTimeToLive);
      using (IModel channel = this.CreateConnection().CreateChannel())
      {
        this.DeclareExchange(routeSetupData.ExchangeSetupData, channel, exchangeName);
        this.DeclareQueue<T>(routeSetupData.QueueSetupData, queueName, channel);
        this.BindQueue<T>(channel, routeSetupData.QueueSetupData, exchangeName, queueName, !string.IsNullOrWhiteSpace(toQueueRoutingKey1) ? toQueueRoutingKey1 : routingKey, routeSetupData.OptionalBindingData);
        if (routeSetupData.AdditionalBindExchangeToQueueRoutingKeys == null)
          return;
        foreach (string toQueueRoutingKey2 in routeSetupData.AdditionalBindExchangeToQueueRoutingKeys)
        {
          if (!string.IsNullOrWhiteSpace(toQueueRoutingKey2))
            this.BindQueue<T>(channel, routeSetupData.QueueSetupData, exchangeName, queueName, toQueueRoutingKey2, routeSetupData.OptionalBindingData);
        }
      }
    }

    protected virtual void CreateDeadLetterRoute<T>(
      string queueName,
      string exchangeName,
      string exchangeType,
      string routingKey,
      string routingKeyOriginal,
      string deadLetterExchangeName,
      string deadLetterQueueName,
      string deadLetterRoutingKey,
      int deadLetterMessageTimeToLive)
    {
      ExchangeSetupData exchange = new ExchangeSetupData()
      {
        Durable = true
      };
      if (string.Compare(exchangeType, "topic", true) == 0)
        routingKey = !string.IsNullOrWhiteSpace(routingKeyOriginal) ? routingKeyOriginal : routingKey + ".Original";
      QueueSetupData queue = new QueueSetupData()
      {
        MessageTimeToLive = deadLetterMessageTimeToLive,
        AutoDelete = false,
        DeadLetterExchange = exchangeName,
        DeadLetterRoutingKey = routingKey
      };
      using (IModel channel = this.CreateConnection().CreateChannel())
      {
        this.DeclareExchange(exchange, channel, deadLetterExchangeName);
        this.DeclareQueue<T>(queue, deadLetterQueueName, channel);
        this.BindQueue<T>(channel, (QueueSetupData) null, deadLetterExchangeName, deadLetterQueueName, deadLetterRoutingKey);
      }
    }

    protected virtual void BindQueue<T>(
      IModel model,
      QueueSetupData queue,
      string exchangeName,
      string queueName,
      string routingKey,
      IDictionary<string, object> bindingData = null)
    {
      if (string.IsNullOrEmpty(exchangeName))
      {
        Global.Logger.WarnFormat("Attempt to bind queue {0} to a empty name Exchange, that's the default built-in exchange so the action will be ignored", (object) queueName);
      }
      else
      {
        try
        {
          model.QueueBind(queueName, exchangeName, routingKey, bindingData);
        }
        catch (Exception ex)
        {
          Global.FaultHandler.HandleFault(ex);
        }
      }
    }

    protected virtual void DeclareQueue<T>(QueueSetupData queue, string queueName, IModel model)
    {
      try
      {
        this.SetQueueSetupArguments(queue);
        model.QueueDeclare(queueName, queue.Durable, false, queue.AutoDelete, queue.Arguments);
      }
      catch (OperationInterruptedException ex)
      {
        if (ex.ShutdownReason.ReplyText.StartsWith("PRECONDITION_FAILED - "))
          Global.FaultHandler.HandleFault(ex.ShutdownReason.ReplyText);
        else
          Global.FaultHandler.HandleFault((Exception) ex);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    protected void SetQueueSetupArguments(QueueSetupData queue)
    {
      if (queue.MessageTimeToLive > 0)
        queue.Arguments["x-message-ttl"] = (object) queue.MessageTimeToLive;
      if (queue.AutoExpire > 0)
        queue.Arguments["x-expires"] = (object) queue.AutoExpire;
      if (queue.DeadLetterExchange != null)
        queue.Arguments["x-dead-letter-exchange"] = (object) queue.DeadLetterExchange;
      if (queue.DeadLetterRoutingKey == null)
        return;
      queue.Arguments["x-dead-letter-routing-key"] = (object) queue.DeadLetterRoutingKey;
    }

    protected virtual void DeclareExchange(
      ExchangeSetupData exchange,
      IModel model,
      string exchangeName)
    {
      if (string.IsNullOrEmpty(exchangeName))
      {
        Global.Logger.WarnFormat("Attempt to declare a Exchange with empty string, that's the default built-in exchange so the action will be ignored");
      }
      else
      {
        try
        {
          model.ExchangeDeclare(exchangeName, exchange.ExchangeType, exchange.Durable, exchange.AutoDelete, exchange.Arguments);
        }
        catch (OperationInterruptedException ex)
        {
          if (ex.ShutdownReason.ReplyText.StartsWith("PRECONDITION_FAILED - "))
            Global.FaultHandler.HandleFault(ex.ShutdownReason.ReplyText);
          else
            Global.FaultHandler.HandleFault((Exception) ex);
        }
        catch (Exception ex)
        {
          Global.FaultHandler.HandleFault(ex);
        }
      }
    }

    public virtual void DestroyRoute<T>(RouteSetupData routeSetupData)
    {
      string queueName = routeSetupData.RouteFinder.FindQueueName<T>(routeSetupData.SubscriptionName);
      string exchangeName = routeSetupData.RouteFinder.FindExchangeName<T>();
      string deadLetterExchange = routeSetupData.QueueSetupData.DeadLetterExchange;
      string deadLetterQueue = routeSetupData.QueueSetupData.DeadLetterQueue;
      using (IModel channel = this.CreateConnection().CreateChannel())
      {
        this.DeleteQueue<T>(channel, routeSetupData.QueueSetupData, queueName);
        if (!string.IsNullOrWhiteSpace(deadLetterQueue))
          this.DeleteQueue<T>(channel, routeSetupData.QueueSetupData, deadLetterQueue);
        this.DeleteExchange<T>(channel, routeSetupData.ExchangeSetupData, exchangeName);
        if (string.IsNullOrWhiteSpace(deadLetterExchange))
          return;
        this.DeleteExchange<T>(channel, routeSetupData.ExchangeSetupData, deadLetterExchange);
      }
    }

    protected virtual void DeleteExchange<T>(
      IModel model,
      ExchangeSetupData exchange,
      string exchangeName)
    {
      try
      {
        model.ExchangeDelete(exchangeName);
      }
      catch (OperationInterruptedException ex)
      {
        if (ex.ShutdownReason.ReplyText.StartsWith("NOT_FOUND - no exchange "))
          Global.Logger.WarnFormat(ex.ShutdownReason.ReplyText);
        else
          Global.FaultHandler.HandleFault((Exception) ex);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    protected virtual void DeleteQueue<T>(IModel model, QueueSetupData queue, string queueName)
    {
      try
      {
        int num = (int) model.QueueDelete(queueName);
      }
      catch (OperationInterruptedException ex)
      {
        if (ex.ShutdownReason.ReplyText.StartsWith("NOT_FOUND - no queue "))
          Global.Logger.WarnFormat(ex.ShutdownReason.ReplyText);
        else
          Global.FaultHandler.HandleFault((Exception) ex);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }
  }
}
