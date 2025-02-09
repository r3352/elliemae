// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.Connections.HaConnection
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues.Rmq.Connections
{
  internal class HaConnection : DurableConnection
  {
    private readonly IRoundRobinList<ConnectionFactory> _connectionFactories;
    private int _nodeTried;
    private Action _unsubscribeEvents = (Action) (() => { });

    public HaConnection(
      IRetryPolicy retryPolicy,
      IList<ManagedConnectionFactory> connectionFactories)
      : base(retryPolicy)
    {
      this._connectionFactories = !(MessageQueueFramework.Runtime.CreateService<IHaConnectionRoundRobinPolicy>() ?? (IHaConnectionRoundRobinPolicy) new HaConnectionRoundRobinPolicy()).UseSingleRoundRobinListPerApplicationDomain ? (IRoundRobinList<ConnectionFactory>) new RoundRobinList<ConnectionFactory>((IEnumerable<ConnectionFactory>) connectionFactories) : (IRoundRobinList<ConnectionFactory>) new HaRoundRobinList((IEnumerable<ConnectionFactory>) connectionFactories);
      ConnectionEstablished handler = (ConnectionEstablished) ((endpoint, virtualHost) =>
      {
        if (!this._connectionFactories.All.Any<ConnectionFactory>((Func<ConnectionFactory, bool>) (f => f.Endpoint.ToString() + f.VirtualHost == endpoint.ToString() + virtualHost)))
          return;
        if (!this.IsConnected)
        {
          while (this._connectionFactories.Current.Endpoint.ToString() + this._connectionFactories.Current.VirtualHost != endpoint.ToString() + virtualHost)
            this._connectionFactories.GetNext();
        }
        this.FireConnectedEvent();
      });
      ManagedConnectionFactory.ConnectionEstablished += handler;
      this._unsubscribeEvents = (Action) (() => ManagedConnectionFactory.ConnectionEstablished -= handler);
    }

    protected internal override ConnectionFactory ConnectionFactory
    {
      get => this._connectionFactories.Current;
    }

    public override void Connect()
    {
      Monitor.Enter(ManagedConnectionFactory.SyncConnection);
      try
      {
        if (this.IsConnected || this._retryPolicy.IsWaiting)
          return;
        Global.Logger.DebugFormat("Trying to connect to endpoint: '{0}'", (object) this.ConnectionFactory.Endpoint);
        this.ConnectionFactory.CreateConnection().ConnectionShutdown += new EventHandler<ShutdownEventArgs>(((DurableConnection) this).SharedConnectionShutdown);
        this._retryPolicy.Reset();
        this._nodeTried = 0;
        Global.MetricsCollector.IncrementConnectionCount();
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
      catch (OperationInterruptedException ex)
      {
        this.HandleConnectionException((Exception) ex);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault("Received Exception : {0}  during connecting  to RabbitMQ. Broker: '{1}:{2}', VHost: '{3}'.", (object) ex.Message, (object) this.ConnectionFactory.HostName, (object) this.ConnectionFactory.Port, (object) this.ConnectionFactory.VirtualHost);
      }
      finally
      {
        Monitor.Exit(ManagedConnectionFactory.SyncConnection);
      }
    }

    private void HandleConnectionException(Exception ex)
    {
      ConnectionFactory connectionFactory = this.ConnectionFactory;
      ConnectionFactory next = this._connectionFactories.GetNext();
      if (++this._nodeTried < this._connectionFactories.All.Count<ConnectionFactory>())
      {
        Global.FaultHandler.HandleFault("Failed to connect to Broker: '{0}:{1}', VHost: '{2}'. Failing over to '{3}:{4}'\nExceptionMessage: {5}", (object) connectionFactory.HostName, (object) connectionFactory.Port, (object) connectionFactory.VirtualHost, (object) next.HostName, (object) next.Port, (object) ex.Message);
        this.Connect();
      }
      else
      {
        Global.FaultHandler.HandleFault("Failed to connect to Broker: '{0}:{1}', VHost: '{2}'. Failing over to '{3}{4}' after {5}ms\nExceptionMessage: {6}", (object) connectionFactory.HostName, (object) connectionFactory.Port, (object) connectionFactory.VirtualHost, (object) next.HostName, (object) next.Port, (object) this._retryPolicy.DelayTime, (object) ex.Message);
        this._retryPolicy.WaitForNextRetry(new Action(((DurableConnection) this).Connect));
      }
    }

    internal IRoundRobinList<ConnectionFactory> ConnectionFactories => this._connectionFactories;

    public override void Dispose()
    {
      this._unsubscribeEvents();
      base.Dispose();
    }
  }
}
