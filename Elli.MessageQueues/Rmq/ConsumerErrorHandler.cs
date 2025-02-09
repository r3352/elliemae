// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.ConsumerErrorHandler
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class ConsumerErrorHandler : IConsumerErrorHandler, IDisposable, IObserver<ISerializer>
  {
    private readonly string _errorQueue;
    private readonly string _errorExchange;
    private readonly IDurableConnection _durableConnection;
    private readonly object _channelGate = new object();
    private ISerializer _serializer;
    private bool _errorQueueDeclared;
    private bool _errorQueueBound;

    public ConsumerErrorHandler(IDurableConnection durableConnection, ISerializer serializer)
    {
      if (durableConnection == null)
        throw new ArgumentNullException("ConsumerErrorHandler(durableConnection)");
      if (serializer == null)
        throw new ArgumentNullException("ConsumerErrorHandler(serializer)");
      this._durableConnection = durableConnection;
      this._serializer = serializer;
      this._errorQueue = Global.DefaultErrorQueueName;
      this._errorExchange = Global.DefaultErrorExchangeName;
    }

    private void DeclareErrorQueue(IModel model)
    {
      if (this._errorQueueDeclared)
        return;
      model.QueueDeclare(this._errorQueue, true, false, false, (IDictionary<string, object>) null);
      this._errorQueueDeclared = true;
    }

    private void DeclareErrorExchangeAndBindToErrorQueue(IModel model)
    {
      if (this._errorQueueBound)
        return;
      model.ExchangeDeclare(this._errorExchange, "direct", true);
      model.QueueBind(this._errorQueue, this._errorExchange, string.Empty);
      this._errorQueueBound = true;
    }

    protected void InitializeErrorExchangeAndQueue(IModel model)
    {
      this.DeclareErrorQueue(model);
      this.DeclareErrorExchangeAndBindToErrorQueue(model);
    }

    protected virtual byte[] CreateErrorMessage(
      BasicDeliverEventArgs devliverArgs,
      Exception exception)
    {
      string str = Encoding.UTF8.GetString(devliverArgs.Body);
      return this._serializer.Serialize<RmqError>(new RmqError()
      {
        RoutingKey = devliverArgs.RoutingKey,
        Exchange = devliverArgs.Exchange,
        Exception = exception.ToString(),
        Message = str,
        DateTime = DateTime.Now,
        BasicProperties = new BasicPropertiesWrapper(devliverArgs.BasicProperties)
      });
    }

    private string CreateConnectionCheckMessage(IDurableConnection durableConnection)
    {
      return "Please check connection string and that the RabbitMQ Service is running at the specified endpoint.\n\tHostname: '{durableConnection.HostName}'\n\tVirtualHost: '{durableConnection.VirtualHost}'\n\tUserName: '{durableConnection.UserName}'\nFailed to write error message to error queue";
    }

    public void Dispose()
    {
    }

    public virtual void HandleError(BasicDeliverEventArgs deliverEventArgs, Exception exception)
    {
      try
      {
        using (IModel channel = this._durableConnection.CreateChannel())
        {
          lock (this._channelGate)
            this.InitializeErrorExchangeAndQueue(channel);
          byte[] errorMessage = this.CreateErrorMessage(deliverEventArgs, exception);
          IBasicProperties basicProperties = channel.CreateBasicProperties();
          basicProperties.Persistent = true;
          basicProperties.Expiration = "86400000";
          channel.BasicPublish(this._errorExchange, string.Empty, basicProperties, errorMessage);
        }
      }
      catch (ConnectFailureException ex)
      {
        Global.FaultHandler.HandleFault("ConsumerErrorHandler: cannot connect to Broker.\n" + this.CreateConnectionCheckMessage(this._durableConnection));
      }
      catch (BrokerUnreachableException ex)
      {
        Global.FaultHandler.HandleFault("ConsumerErrorHandler: cannot connect to Broker.\n" + this.CreateConnectionCheckMessage(this._durableConnection));
      }
      catch (OperationInterruptedException ex)
      {
        Global.FaultHandler.HandleFault("ConsumerErrorHandler: Broker connection was closed while attempting to publish Error message.\nMessage was: '" + ex.Message + "'\n" + this.CreateConnectionCheckMessage(this._durableConnection));
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault("ConsumerErrorHandler: Failed to publish error message\nException is:\n" + (object) ex);
      }
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
