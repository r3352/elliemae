// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.Connections.DurableConnection
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues.Rmq.Connections
{
  internal class DurableConnection : IDurableConnection, IDisposable
  {
    protected readonly IRetryPolicy _retryPolicy;
    private Action _unsubscribeEvents = (Action) (() => { });
    private readonly ConnectionFactory _connectionFactory;

    public event Action Connected;

    public event Action Disconnected;

    internal DurableConnection(IRetryPolicy retryPolicy)
    {
      this._retryPolicy = retryPolicy != null ? retryPolicy : throw new ArgumentNullException("DurableConnection(retryPolicy)");
    }

    public DurableConnection(IRetryPolicy retryPolicy, ConnectionFactory connectionFactory)
      : this(retryPolicy)
    {
      this._connectionFactory = connectionFactory != null ? (ConnectionFactory) ManagedConnectionFactory.CreateFromConnectionFactory(connectionFactory) : throw new ArgumentNullException("DurableConnection(connectionFactory)");
      ConnectionEstablished handler = (ConnectionEstablished) ((endpoint, virtualHost) =>
      {
        if (!(this._connectionFactory.Endpoint.ToString() + this._connectionFactory.VirtualHost == endpoint.ToString() + virtualHost))
          return;
        this.FireConnectedEvent();
      });
      ManagedConnectionFactory.ConnectionEstablished += handler;
      this._unsubscribeEvents = (Action) (() => ManagedConnectionFactory.ConnectionEstablished -= handler);
    }

    public virtual void Connect()
    {
      Monitor.Enter(ManagedConnectionFactory.SyncConnection);
      try
      {
        if (this.IsConnected || this._retryPolicy.IsWaiting)
          return;
        Global.MetricsCollector.IncrementConnectionCount();
        Global.Logger.DebugFormat("Trying to connect to endpoint: '{0}'", (object) this.ConnectionFactory.Endpoint);
        this.ConnectionFactory.CreateConnection().ConnectionShutdown += new EventHandler<ShutdownEventArgs>(this.SharedConnectionShutdown);
        this._retryPolicy.Reset();
        Global.Logger.InfoFormat("Connected to RabbitMQ. Broker: '{0}', VHost: '{1}'", (object) this.ConnectionFactory.Endpoint, (object) this.ConnectionFactory.VirtualHost);
      }
      catch (ConnectFailureException ex)
      {
        this.HandleConnectionException((Exception) ex);
      }
      catch (BrokerUnreachableException ex)
      {
        this.HandleConnectionException((Exception) ex);
      }
      finally
      {
        Monitor.Exit(ManagedConnectionFactory.SyncConnection);
      }
    }

    private void HandleConnectionException(Exception ex)
    {
      Global.FaultHandler.HandleFault("Failed to connect to Broker: '{0}', VHost: '{1}'. Retrying in {2} ms\nCheck HostName, VirtualHost, Username and Password.\nExceptionMessage: {3}", (object) this.ConnectionFactory.HostName, (object) this.ConnectionFactory.VirtualHost, (object) this._retryPolicy.DelayTime, (object) ex.Message);
      this._retryPolicy.WaitForNextRetry(new Action(this.Connect));
    }

    protected void FireConnectedEvent()
    {
      if (this.Connected == null)
        return;
      this.Connected();
    }

    protected void FireDisconnectedEvent()
    {
      if (this.Disconnected == null)
        return;
      this.Disconnected();
    }

    protected void SharedConnectionShutdown(object sender, ShutdownEventArgs reason)
    {
      this.FireDisconnectedEvent();
      if (reason != null && reason.ReplyText != "Connection disposed by application" && reason.ReplyText != "Closed by application")
      {
        Global.Logger.WarnFormat("Disconnected from RabbitMQ Broker '{0}': {1}", (object) ((IConnection) sender).Endpoint, reason != null ? (object) reason.ReplyText : (object) "");
        this._retryPolicy.WaitForNextRetry(new Action(this.Connect));
      }
      else
        Global.Logger.InfoFormat("Disconnected from RabbitMQ Broker '{0}': {1}", (object) ((IConnection) sender).Endpoint, reason != null ? (object) reason.ReplyText : (object) "");
    }

    public bool IsConnected
    {
      get
      {
        string key = this.ConnectionFactory.Endpoint.ToString() + this.ConnectionFactory.VirtualHost;
        if (!ManagedConnectionFactory.SharedConnections.ContainsKey(key))
          return false;
        IConnection sharedConnection = ManagedConnectionFactory.SharedConnections[key];
        return sharedConnection.IsOpen && sharedConnection.Endpoint != null && this.ConnectionFactory.Endpoint.ToString().Equals(sharedConnection.Endpoint.ToString());
      }
    }

    public string HostName => this.ConnectionFactory.HostName;

    public string VirtualHost => this.ConnectionFactory.VirtualHost;

    public string UserName => this.ConnectionFactory.UserName;

    public string EndPoint => this.ConnectionFactory.Endpoint.ToString();

    protected internal virtual ConnectionFactory ConnectionFactory => this._connectionFactory;

    public IModel CreateChannel()
    {
      try
      {
        if (!this.IsConnected)
          this.Connect();
        if (!this.IsConnected)
          throw new BrokerUnreachableException(new Exception("Cannot connect to RabbitMQ server."));
        Global.MetricsCollector.IncrementChannelCount();
        return ChannelManager.GetInstance().GetChannel(this.EndPoint, this.VirtualHost);
      }
      catch (ChannelAllocationException ex)
      {
        Global.Logger.ErrorFormat("ClassName={0}, MethodName=CreateChannel, Message=Channel AllocationException while creating channel - {1}", (object) this.GetType().Name, (object) ex.StackTrace);
        throw ex;
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("ClassName={0} MethodName=CreateChannel, Message=Exception while creating channel - {1}", (object) this.GetType().Name, (object) ex.StackTrace);
        throw ex;
      }
    }

    public void DropChannel(IModel model)
    {
      if (model == null)
        return;
      try
      {
        ChannelManager.GetInstance().PutChannel(this.EndPoint, this.VirtualHost, model);
      }
      catch (IOException ex)
      {
        Global.Logger.ErrorFormat("ClassName={0}, MethodName=DropChannel, IOException while dropping channel - {1}", (object) this.GetType().Name, (object) model.CreateBasicProperties().AppId);
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("ClassName={0}, MethodName=DropChannel, Exception while dropping channel - {1}", (object) this.GetType().Name, (object) model.CreateBasicProperties().AppId);
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public virtual void Dispose() => this._unsubscribeEvents();
  }
}
