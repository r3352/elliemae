// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.TunnelFactory
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Elli.MessageQueues.Rmq.Connections;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class TunnelFactory
  {
    public TunnelFactory()
      : this(true)
    {
    }

    internal TunnelFactory(bool setAsDefault)
    {
      if (!setAsDefault)
        return;
      RmqTunnel.Factory = this;
    }

    public void CloseAllConnections() => ManagedConnectionFactory.CloseAllConnections();

    [ExcludeFromCodeCoverage]
    public virtual IRmqTunnel Create()
    {
      return this.Create((ConfigurationManager.ConnectionStrings["RabbitMQ"] ?? throw new Exception("Could not find a connection string for RabbitMQ. Please add a connection string in the <ConnectionStrings> secionof the application's configuration file. For example: <add name=\"RabbitMQ\" connectionString=\"host=localhost\" />")).ConnectionString);
    }

    public virtual IRmqTunnel Create(string connectionString)
    {
      string[] source = connectionString.Split(new char[1]
      {
        '|'
      }, StringSplitOptions.RemoveEmptyEntries);
      if (source.Length <= 1)
        return this.Create((ConnectionFactory) new ManagedConnectionFactory(new ConnectionString(connectionString)));
      List<ManagedConnectionFactory> list = ((IEnumerable<string>) source).Select<string, ManagedConnectionFactory>((Func<string, ManagedConnectionFactory>) (x => new ManagedConnectionFactory(new ConnectionString(x)))).ToList<ManagedConnectionFactory>();
      return this.Create((IDurableConnection) new HaConnection(MessageQueueFramework.Runtime.CreateService<IRetryPolicy>(), (IList<ManagedConnectionFactory>) list));
    }

    public virtual IRmqTunnel Create(
      string hostName,
      int port,
      string virtualHost,
      string username,
      string password)
    {
      ManagedConnectionFactory connectionFactory = new ManagedConnectionFactory();
      connectionFactory.HostName = hostName;
      connectionFactory.Port = port;
      connectionFactory.VirtualHost = virtualHost;
      connectionFactory.UserName = username;
      connectionFactory.Password = password;
      return this.Create((ConnectionFactory) connectionFactory);
    }

    private IRmqTunnel Create(ConnectionFactory connectionFactory)
    {
      return this.Create((IDurableConnection) new DurableConnection(MessageQueueFramework.Runtime.CreateService<IRetryPolicy>(), connectionFactory));
    }

    private IRmqTunnel Create(IDurableConnection durableConnection)
    {
      ConsumerErrorHandler consumerErrorHandler = new ConsumerErrorHandler(durableConnection, Global.Serializer);
      DefaultMessageHandlerFactory messageHandlerFactory = new DefaultMessageHandlerFactory((IConsumerErrorHandler) consumerErrorHandler, Global.Serializer);
      ConsumerManager consumerManager = new ConsumerManager((IMessageHandlerFactory) messageHandlerFactory, Global.Serializer);
      RmqTunnel rmqTunnel = new RmqTunnel((IConsumerManager) consumerManager, (IRouteFinder) new DefaultRouteFinder(), durableConnection, Global.Serializer, Global.DefaultCorrelationIdGenerator, Global.DefaultPersistentMode);
      rmqTunnel.AddSerializerObserver((IObserver<ISerializer>) consumerErrorHandler);
      rmqTunnel.AddSerializerObserver((IObserver<ISerializer>) messageHandlerFactory);
      rmqTunnel.AddSerializerObserver((IObserver<ISerializer>) consumerManager);
      return (IRmqTunnel) rmqTunnel;
    }
  }
}
