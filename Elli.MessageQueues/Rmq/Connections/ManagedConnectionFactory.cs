// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.Connections.ManagedConnectionFactory
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

#nullable disable
namespace Elli.MessageQueues.Rmq.Connections
{
  public class ManagedConnectionFactory : ConnectionFactory
  {
    internal static readonly object SyncConnection = new object();
    internal static volatile Dictionary<string, IConnection> SharedConnections = new Dictionary<string, IConnection>();

    public static ConnectionEstablished ConnectionEstablished { get; set; }

    public static ManagedConnectionFactory CreateFromConnectionFactory(
      ConnectionFactory connectionFactory)
    {
      return !(connectionFactory is ManagedConnectionFactory) ? new ManagedConnectionFactory(connectionFactory) : (ManagedConnectionFactory) connectionFactory;
    }

    public ManagedConnectionFactory()
    {
    }

    public ManagedConnectionFactory(ConnectionString connectionString)
    {
      this.HostName = connectionString != null ? connectionString.Host : throw new ArgumentNullException("ManagedConnectionFactory(connectionString)");
      this.Port = connectionString.Port;
      this.VirtualHost = connectionString.VirtualHost;
      this.UserName = connectionString.UserName;
      this.Password = connectionString.Password;
    }

    public ManagedConnectionFactory(ConnectionFactory factory)
    {
      this.AuthMechanisms = factory.AuthMechanisms;
      this.ClientProperties = factory.ClientProperties;
      this.Endpoint = factory.Endpoint;
      this.HostName = factory.HostName;
      this.Password = factory.Password;
      this.Port = factory.Port;
      this.Protocol = factory.Protocol;
      this.RequestedChannelMax = factory.RequestedChannelMax;
      this.RequestedConnectionTimeout = factory.RequestedConnectionTimeout;
      this.RequestedFrameMax = factory.RequestedFrameMax;
      this.RequestedHeartbeat = factory.RequestedHeartbeat;
      this.SocketFactory = factory.SocketFactory;
      this.Ssl = factory.Ssl;
      this.UserName = factory.UserName;
      this.VirtualHost = factory.VirtualHost;
    }

    public override sealed IConnection CreateConnection()
    {
      IConnection connection = this.EstablishConnection();
      this.SaveConnection(connection);
      return connection;
    }

    [ExcludeFromCodeCoverage]
    protected internal virtual IConnection EstablishConnection() => base.CreateConnection();

    private void SaveConnection(IConnection connection)
    {
      if (connection == null || !connection.IsOpen)
        return;
      string key = this.Endpoint.ToString() + this.VirtualHost;
      ManagedConnectionFactory.SharedConnections[key] = connection;
      connection.ConnectionShutdown += new EventHandler<ShutdownEventArgs>(this.ConnectionShutdown);
      if (ManagedConnectionFactory.ConnectionEstablished == null)
        return;
      ManagedConnectionFactory.ConnectionEstablished(this.Endpoint, this.VirtualHost);
    }

    private void ConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
      foreach (KeyValuePair<string, IConnection> sharedConnection in ManagedConnectionFactory.SharedConnections)
      {
        if (sharedConnection.Key == this.Endpoint.ToString() + this.VirtualHost)
        {
          ManagedConnectionFactory.SharedConnections.Remove(sharedConnection.Key);
          break;
        }
      }
    }

    internal static void CloseAllConnections()
    {
      ManagedConnectionFactory.SharedConnections.Values.ToList<IConnection>().ForEach((Action<IConnection>) (c =>
      {
        c.Close((ushort) 200, "Connection disposed by application");
        c.Dispose();
      }));
      ManagedConnectionFactory.SharedConnections.Clear();
      ChannelManager.GetInstance().DisposeConnection();
    }
  }
}
