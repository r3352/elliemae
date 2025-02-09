// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.RmqTunnel
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class RmqTunnel : IRmqTunnel, ITunnel, IDisposable
  {
    protected readonly IConsumerManager _consumerManager;
    protected readonly IDurableConnection _connection;
    private readonly ICorrelationIdGenerator _correlationIdGenerator;
    protected readonly List<IModel> _createdChannels = new List<IModel>();
    private ConcurrentDictionary<Guid, Action> _subscribeActions;
    private ConcurrentBag<IObserver<ISerializer>> _observers;
    protected static readonly object _tunnelGate = new object();
    private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
    protected ISerializer _serializer;
    protected IRouteFinder _routeFinder;
    private ITopicExchangeRouteFinder _topicExchangeRouteFinder;
    private bool _setPersistent;
    protected volatile bool _disposed;
    private volatile IModel _dedicatedPublishingChannel;
    private volatile List<Timer> _createdTimer = new List<Timer>();

    public event Action OnOpened;

    public event Action OnClosed;

    public event Action<ISubscription> ConsumerDisconnected;

    public static TunnelFactory Factory { get; internal set; }

    static RmqTunnel()
    {
      if (RmqTunnel.Factory != null)
        return;
      TunnelFactory tunnelFactory = new TunnelFactory();
    }

    public RmqTunnel(IRouteFinder routeFinder, IDurableConnection connection)
      : this((IConsumerManager) new ConsumerManager((IMessageHandlerFactory) new DefaultMessageHandlerFactory((IConsumerErrorHandler) new ConsumerErrorHandler(connection, Global.Serializer), Global.Serializer), Global.Serializer), routeFinder, connection, Global.Serializer, Global.DefaultCorrelationIdGenerator, Global.DefaultPersistentMode)
    {
    }

    public RmqTunnel(
      IConsumerManager consumerManager,
      IRouteFinder routeFinder,
      IDurableConnection connection,
      ISerializer serializer,
      ICorrelationIdGenerator correlationIdGenerator,
      bool setPersistent)
    {
      if (consumerManager == null)
        throw new ArgumentNullException(nameof (consumerManager));
      if (connection == null)
        throw new ArgumentNullException(nameof (connection));
      if (correlationIdGenerator == null)
        throw new ArgumentNullException(nameof (correlationIdGenerator));
      this._consumerManager = consumerManager;
      this._connection = connection;
      this._correlationIdGenerator = correlationIdGenerator;
      this._observers = new ConcurrentBag<IObserver<ISerializer>>();
      this.SetRouteFinder(routeFinder);
      this.SetSerializer(serializer);
      this.SetPersistentMode(setPersistent);
      this._connection.Connected += new Action(this.OpenTunnel);
      this._connection.Disconnected += new Action(this.CloseTunnel);
      this._subscribeActions = new ConcurrentDictionary<Guid, Action>();
    }

    private void CloseTunnel()
    {
      if (this._disposed)
        return;
      this._autoResetEvent.WaitOne();
      try
      {
        this._consumerManager.ClearConsumers();
        Task.Factory.StartNew((Action) (() => this.DisposeAllChannels()), Global.DefaultTaskCreationOptionsProvider()).ContinueWith((Action<Task>) (t => this._createdChannels.Clear()), Global.DefaultTaskContinuationOptionsProvider());
        if (this.OnClosed == null)
          return;
        this.OnClosed();
      }
      finally
      {
        this._autoResetEvent.Set();
      }
    }

    private void OpenTunnel()
    {
      this._autoResetEvent.WaitOne();
      try
      {
        this.CreatePublishChannel();
        if (this._subscribeActions.Count > 0)
          Global.Logger.InfoFormat("Subscribe to queues");
        foreach (Action subscription in (IEnumerable<Action>) this._subscribeActions.Values)
          this.TrySubscribe(subscription);
        if (this.OnOpened == null)
          return;
        this.OnOpened();
      }
      finally
      {
        this._autoResetEvent.Set();
      }
    }

    protected virtual void CreatePublishChannel()
    {
      if (this._dedicatedPublishingChannel != null && this._dedicatedPublishingChannel.IsOpen)
        return;
      Global.Logger.InfoFormat("Creating dedicated publishing channel to Broker: '{0}', VHost: '{1}'", (object) this._connection.HostName, (object) this._connection.VirtualHost);
      this._dedicatedPublishingChannel = this._connection.CreateChannel();
      if (this._dedicatedPublishingChannel == null || !this._dedicatedPublishingChannel.IsOpen)
        throw new Exception("No channel to RabbitMQ server established.");
      this._createdChannels.Add(this._dedicatedPublishingChannel);
      Global.Logger.InfoFormat("Dedicated publishing channel to Broker: '{0}', VHost: '{1}' established", (object) this._connection.HostName, (object) this._connection.VirtualHost);
    }

    public bool IsOpened => this._connection != null && this._connection.IsConnected;

    public IModel DedicatedPublishingChannel
    {
      get
      {
        lock (RmqTunnel._tunnelGate)
        {
          this.EnsurePublishChannelIsCreated();
          return this._dedicatedPublishingChannel;
        }
      }
    }

    public void Publish<T>(T message)
    {
      if (this._topicExchangeRouteFinder != null)
        this.Publish<T>(message, this._topicExchangeRouteFinder.FindRoutingKey<T>(message), (IDictionary<string, object>) null);
      else
        this.Publish<T>(message, this._routeFinder.FindRoutingKey<T>(), (IDictionary<string, object>) null);
    }

    public virtual void Publish<T>(T message, string routingKey)
    {
      this.Publish<T>(message, routingKey, (IDictionary<string, object>) null);
    }

    public void Publish<T>(T message, IDictionary<string, object> customHeaders)
    {
      if (this._topicExchangeRouteFinder != null)
        this.Publish<T>(message, this._topicExchangeRouteFinder.FindRoutingKey<T>(message), (IDictionary<string, object>) null);
      else
        this.Publish<T>(message, this._routeFinder.FindRoutingKey<T>(), customHeaders);
    }

    private void Publish<T>(
      T message,
      string routingKey,
      IDictionary<string, object> customHeaders)
    {
      try
      {
        using (Global.MetricsCollector.RecordPublishTime(typeof (T).Name))
        {
          byte[] body = this._serializer.Serialize<T>(message);
          IBasicProperties propertiesForPublishing = this.CreateBasicPropertiesForPublishing<T>();
          if (customHeaders != null)
          {
            propertiesForPublishing.Headers = (IDictionary<string, object>) new Dictionary<string, object>();
            foreach (string key in (IEnumerable<string>) customHeaders.Keys)
            {
              if (key != null && customHeaders[key] != null)
                propertiesForPublishing.Headers.Add(key, customHeaders[key]);
            }
          }
          if (propertiesForPublishing.Headers == null)
            propertiesForPublishing.Headers = (IDictionary<string, object>) new Dictionary<string, object>();
          if (!propertiesForPublishing.Headers.ContainsKey("PublishedDate"))
            propertiesForPublishing.Headers.Add("PublishedDate", (object) DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", (IFormatProvider) CultureInfo.InvariantCulture));
          string exchangeName = this._routeFinder.FindExchangeName<T>();
          lock (RmqTunnel._tunnelGate)
            this.DedicatedPublishingChannel.BasicPublish(exchangeName, routingKey, propertiesForPublishing, body);
          if (!Global.Logger.IsDebugEnable)
            return;
          Global.Logger.DebugFormat("Published to {0}, CorrelationId {1}", (object) exchangeName, (object) propertiesForPublishing.CorrelationId);
        }
      }
      catch (Exception ex)
      {
        throw new Exception(string.Format("Publish failed: '{0}'", (object) ex.Message), ex);
      }
    }

    public void PublishingChannelConfirmSelect() => this.DedicatedPublishingChannel.ConfirmSelect();

    public void PublishingChannelWaitForConfirmsOrDie(int timeout = 0)
    {
      if (timeout <= 0)
        timeout = -1;
      this.DedicatedPublishingChannel.WaitForConfirmsOrDie(TimeSpan.FromMilliseconds((double) timeout));
    }

    protected void EnsurePublishChannelIsCreated()
    {
      if (!this.IsOpened)
        this._connection.Connect();
      if (this._dedicatedPublishingChannel != null && this._dedicatedPublishingChannel.IsOpen)
        return;
      this.CreatePublishChannel();
    }

    protected virtual IBasicProperties CreateBasicPropertiesForPublishing<T>()
    {
      IBasicProperties basicProperties = this.DedicatedPublishingChannel.CreateBasicProperties();
      basicProperties.Persistent = this._setPersistent;
      basicProperties.Type = Global.DefaultTypeNameSerializer.Serialize(typeof (T));
      basicProperties.CorrelationId = this._correlationIdGenerator.GenerateCorrelationId();
      return basicProperties;
    }

    private ushort GetProperPrefetchCount(uint prefetchCount)
    {
      if (prefetchCount <= 0U)
        return (ushort) Global.PreFetchCount;
      if (prefetchCount > (uint) ushort.MaxValue)
        Global.Logger.WarnFormat("The prefetch count is too high {0}, the queue will prefetch the maximum {1} msgs", (object) prefetchCount, (object) ushort.MaxValue);
      return (ushort) Math.Min((uint) ushort.MaxValue, prefetchCount);
    }

    public ISubscription Subscribe<T>(SubscriptionOption<T> subscriptionOption)
    {
      string queueName = (subscriptionOption.RouteFinder ?? this._routeFinder).FindQueueName<T>(subscriptionOption.SubscriptionName);
      ushort properPrefetchCount = this.GetProperPrefetchCount(subscriptionOption.QueuePrefetchCount);
      this.TryConnectBeforeSubscribing();
      Func<IModel, IBasicConsumer> createConsumer = (Func<IModel, IBasicConsumer>) (channel => this._consumerManager.CreateConsumer<T>(channel, subscriptionOption.SubscriptionName, queueName, subscriptionOption.MessageHandler, subscriptionOption.CircuitBreakerPolicyConfig, new ushort?(subscriptionOption.BatchSize <= (ushort) 0 ? (ushort) 1 : subscriptionOption.BatchSize)));
      return (ISubscription) this.CreateSubscription(subscriptionOption.SubscriptionName, subscriptionOption.Exclusive, queueName, createConsumer, properPrefetchCount, CancellationToken.None);
    }

    public ISubscription SubscribeAsync<T>(AsyncSubscriptionOption<T> subscriptionOption)
    {
      string queueName = (subscriptionOption.RouteFinder ?? this._routeFinder).FindQueueName<T>(subscriptionOption.SubscriptionName);
      ushort properPrefetchCount = this.GetProperPrefetchCount(subscriptionOption.QueuePrefetchCount);
      this.TryConnectBeforeSubscribing();
      Func<IModel, IBasicConsumer> createConsumer = (Func<IModel, IBasicConsumer>) (channel => this._consumerManager.CreateAsyncConsumer<T>(channel, subscriptionOption.SubscriptionName, queueName, subscriptionOption.MessageHandler, subscriptionOption.CircuitBreakerPolicy, new ushort?(subscriptionOption.BatchSize <= (ushort) 0 ? (ushort) 1 : subscriptionOption.BatchSize)));
      return (ISubscription) this.CreateSubscription(subscriptionOption.SubscriptionName, subscriptionOption.Exclusive, queueName, createConsumer, properPrefetchCount, subscriptionOption.CancellationToken);
    }

    public ISubscription Subscribe<T>(string subscriptionName, Action<T> onReceiveMessage)
    {
      return this.Subscribe<T>(new SubscriptionOption<T>()
      {
        SubscriptionName = subscriptionName,
        MessageHandler = onReceiveMessage,
        BatchSize = (ushort) 1,
        QueuePrefetchCount = Global.PreFetchCount
      });
    }

    public ISubscription Subscribe<T>(
      string subscriptionName,
      Action<T, MessageDeliverEventArgs> onReceiveMessage)
    {
      return this.SubscribeAsync<T>(new AsyncSubscriptionOption<T>()
      {
        SubscriptionName = subscriptionName,
        MessageHandler = onReceiveMessage,
        BatchSize = (ushort) 1,
        QueuePrefetchCount = Global.PreFetchCount
      });
    }

    public ISubscription SubscribeAsync<T>(
      string subscriptionName,
      Action<T> onReceiveMessage,
      ushort? batchSize)
    {
      return this.Subscribe<T>(new SubscriptionOption<T>()
      {
        SubscriptionName = subscriptionName,
        MessageHandler = onReceiveMessage,
        BatchSize = (ushort) ((int) batchSize ?? (int) Global.DefaultConsumerBatchSize),
        QueuePrefetchCount = Global.PreFetchCount
      });
    }

    public ISubscription SubscribeAsync<T>(
      string subscriptionName,
      Action<T, MessageDeliverEventArgs> onReceiveMessage,
      ushort? batchSize)
    {
      return this.SubscribeAsync<T>(new AsyncSubscriptionOption<T>()
      {
        SubscriptionName = subscriptionName,
        MessageHandler = onReceiveMessage,
        BatchSize = (ushort) ((int) batchSize ?? (int) Global.DefaultConsumerBatchSize),
        QueuePrefetchCount = Global.PreFetchCount
      });
    }

    protected void TryConnectBeforeSubscribing()
    {
      lock (RmqTunnel._tunnelGate)
      {
        if (this.IsOpened)
          return;
        this._connection.Connect();
      }
    }

    protected virtual void TryReconnect(
      IModel disconnectedChannel,
      Guid id,
      ShutdownEventArgs eventArgs)
    {
      if (this._disposed)
        return;
      this._createdChannels.Remove(disconnectedChannel);
      if (eventArgs.ReplyCode != (ushort) 406 || !eventArgs.ReplyText.StartsWith("PRECONDITION_FAILED - unknown delivery tag "))
        return;
      Global.Logger.InfoFormat("Trying to re-subscribe to queue after 2 seconds ...");
      RmqTunnel.TimeSubscription state = new RmqTunnel.TimeSubscription();
      Timer timer = new Timer((TimerCallback) (o => this.ExecuteSubscription((RmqTunnel.TimeSubscription) o)), (object) state, 2000, -1);
      state.SubscriptionId = id;
      state.Timer = timer;
      this._createdTimer.Add(timer);
    }

    internal void ExecuteSubscription(RmqTunnel.TimeSubscription state)
    {
      try
      {
        this._subscribeActions[state.SubscriptionId]();
        if (state.Timer == null)
          return;
        state.Timer.Dispose();
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    private Subscription CreateSubscription(
      string subscriptionName,
      bool exclusive,
      string queueName,
      Func<IModel, IBasicConsumer> createConsumer,
      ushort prefetchCount,
      CancellationToken cancellationToken)
    {
      Subscription subscription = new Subscription()
      {
        SubscriptionName = subscriptionName,
        CancellationToken = cancellationToken
      };
      Guid id = Guid.NewGuid();
      Action subscription1 = (Action) (() =>
      {
        try
        {
          subscription.QueueName = queueName;
          subscription.ConsumerTag = string.Format("{0}-{1}", (object) subscriptionName, (object) Guid.NewGuid());
          subscription.Exclusive = exclusive;
          IModel channel = this._connection.CreateChannel();
          channel.ModelShutdown += (EventHandler<ShutdownEventArgs>) ((c, reason) =>
          {
            if (this._disposed)
              return;
            this.RaiseConsumerDisconnectedEvent(subscription);
            this.TryReconnect((IModel) c, id, reason);
          });
          channel.BasicQos(0U, prefetchCount, false);
          this._createdChannels.Add(channel);
          IBasicConsumer consumer = createConsumer(channel);
          if (consumer is DefaultBasicConsumer)
            (consumer as DefaultBasicConsumer).ConsumerTag = subscription.ConsumerTag;
          Subscription.OutstandingDeliveryTags[subscription.ConsumerTag] = new List<ulong>();
          subscription.SetChannel(channel);
          subscription.EnsureExclusivity(this._connection);
          channel.BasicConsume(subscription.QueueName, false, subscription.ConsumerTag, consumer);
          Global.MetricsCollector.IncrementSubscriptionCount();
          Global.Logger.InfoFormat("Subscribed to: {0} with subscriptionName: {1}", (object) subscription.QueueName, (object) subscription.SubscriptionName);
        }
        catch
        {
          subscription.ReleaseExclusivity();
          throw;
        }
      });
      this._subscribeActions[id] = subscription1;
      this.TrySubscribe(subscription1);
      return subscription;
    }

    protected void RaiseConsumerDisconnectedEvent(Subscription subscription)
    {
      if (this.ConsumerDisconnected == null)
        return;
      this.ConsumerDisconnected((ISubscription) subscription);
    }

    protected void TrySubscribe(Action subscription)
    {
      try
      {
        subscription();
      }
      catch (OperationInterruptedException ex)
      {
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    public void SetRouteFinder(IRouteFinder routeFinder)
    {
      this._routeFinder = routeFinder != null ? routeFinder : throw new ArgumentNullException(nameof (routeFinder));
      this._topicExchangeRouteFinder = this._routeFinder as ITopicExchangeRouteFinder;
    }

    public void SetSerializer(ISerializer serializer)
    {
      this._serializer = serializer != null ? serializer : throw new ArgumentNullException(nameof (serializer));
      foreach (IObserver<ISerializer> observer in this._observers)
        observer.OnNext(serializer);
    }

    public void SetPersistentMode(bool persistentMode) => this._setPersistent = persistentMode;

    public uint GetMessageCount<T>(SubscriptionOption<T> subscriptionOption)
    {
      return this.GetMessageCount((subscriptionOption.RouteFinder ?? this._routeFinder).FindQueueName<T>(subscriptionOption.SubscriptionName));
    }

    public uint GetMessageCount(string queueName)
    {
      lock (RmqTunnel._tunnelGate)
      {
        QueueDeclareOk queueDeclareOk = this.DedicatedPublishingChannel.QueueDeclarePassive(queueName);
        return queueDeclareOk != null ? queueDeclareOk.MessageCount : 0U;
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing)
        this.DisposeConsumerManager();
      Task.Factory.StartNew((Action) (() => this.DisposeAllChannels()), Global.DefaultTaskCreationOptionsProvider()).ContinueWith((Action<Task>) (t => this._createdChannels.Clear()), Global.DefaultTaskContinuationOptionsProvider()).Wait(Global.ConsumerDisposeTimeoutPolicy.TimeoutInSeconds * 1000);
      if (this._connection.IsConnected)
        this._connection.Dispose();
      this._observers = (ConcurrentBag<IObserver<ISerializer>>) null;
      this._subscribeActions = (ConcurrentDictionary<Guid, Action>) null;
      this._disposed = true;
    }

    ~RmqTunnel()
    {
      try
      {
        Global.Logger.WarnFormat("Caller forgot to dispose the tunnel object and it is being disposed by Finalizer");
        this.Dispose(false);
      }
      catch (Exception ex)
      {
        Global.Logger.ErrorFormat("Error : {0}  in finalizer during disposing the tunnel object" + ex.Message);
      }
      finally
      {
        // ISSUE: explicit finalizer call
        base.Finalize();
      }
    }

    protected virtual void DisposeConsumerManager() => this._consumerManager.Dispose();

    private void DisposeChannel(IModel model) => this._connection.DropChannel(model);

    [ExcludeFromCodeCoverage]
    internal void AddSerializerObserver(IObserver<ISerializer> observer)
    {
      if (this._observers.Contains<IObserver<ISerializer>>(observer))
        return;
      this._observers.Add(observer);
    }

    private void DisposeAllChannels()
    {
      try
      {
        this._createdChannels.ForEach(new Action<IModel>(this.DisposeChannel));
      }
      catch (Exception ex)
      {
      }
    }

    protected ConcurrentDictionary<Guid, Action> getSubscribeActions() => this._subscribeActions;

    internal class TimeSubscription
    {
      public Timer Timer { get; set; }

      public Guid SubscriptionId { get; set; }
    }
  }
}
