// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.RmqConsumer
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class RmqConsumer : RmqQueueingBasicConsumer, IDisposable
  {
    private readonly bool _autoAck;
    private bool _channelShutdown;
    private readonly object _sharedQueueLock = new object();
    private readonly string _queueName;
    private readonly IMessageHandler _messageHandler;
    private readonly PollyCircuitBreakerConfig _circuitBreakerPolicyConfig;
    protected volatile ConsumerStatus _status;

    public int BatchSize { get; private set; }

    protected SafeSemaphore _pool { get; set; }

    public RmqConsumer(
      IModel channel,
      string queueName,
      IMessageHandler messageHandler,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      bool autoAck,
      int batchSize)
      : this(channel, queueName, messageHandler, circuitBreakerPolicyConfig, autoAck, batchSize, true)
    {
    }

    protected RmqConsumer(
      IModel channel,
      string queueName,
      IMessageHandler messageHandler,
      PollyCircuitBreakerConfig circuitBreakerPolicyConfig,
      bool autoAck,
      int batchSize,
      bool startThread)
      : base(channel, new SharedQueue<BasicDeliverEventArgs>())
    {
      if (channel == null)
        throw new ArgumentNullException("RmqConsumer(channel)");
      if (queueName == null)
        throw new ArgumentNullException("RmqConsumer(queueName)");
      if (messageHandler == null)
        throw new ArgumentNullException("RmqConsumer(messageHandler)");
      if (batchSize < 1)
        throw new ArgumentException("batchSize must be greater than or equal 1", "RmqConsumer(batchSize)");
      this.Model.ModelShutdown += new EventHandler<ShutdownEventArgs>(this.WhenChannelShutdown);
      this.Model.BasicRecoverAsync(true);
      this.BatchSize = batchSize;
      this._pool = new SafeSemaphore(this.BatchSize, this.BatchSize);
      this._autoAck = autoAck;
      this._queueName = queueName;
      this._messageHandler = messageHandler;
      this._circuitBreakerPolicyConfig = circuitBreakerPolicyConfig;
      this._messageHandler.HandlingComplete += new MessageHandlingEvent(this.MessageHandlerHandlingComplete);
      this._messageHandler.MessageWasNotHandled += new MessageWasNotHandledEvent(this.MessageWasNotHandled);
      if (!startThread)
        return;
      this.StartConsumerThread(string.Format("Consumer thread: {0}", (object) this.ConsumerTag));
    }

    protected void StartConsumerThread(string threadName)
    {
      new Thread((ThreadStart) (() =>
      {
        try
        {
          Global.Logger.InfoFormat("<<Batch size is {0}>>", (object) this.BatchSize);
          Action<BasicDeliverEventArgs> handler1 = new Action<BasicDeliverEventArgs>(this.HandleMessageDeliveryInSameThread);
          Action<BasicDeliverEventArgs> handler2 = new Action<BasicDeliverEventArgs>(this.HandleMessageDeliveryInSeperatedThread);
          while (this._status == ConsumerStatus.Active && !this._channelShutdown)
          {
            if (this._circuitBreakerPolicyConfig != null && this._circuitBreakerPolicyConfig.CircuitBreakerPolicy != null)
            {
              if (this._circuitBreakerPolicyConfig.CircuitBreakerPolicy.CircuitState == CircuitState.Open)
              {
                TimeSpan circuitBreakerIsOpen = this._circuitBreakerPolicyConfig.WaitTimeWhenCircuitBreakerIsOpen;
                Global.Logger.InfoFormat("Thread paused in 'RmqConsumer.StartConsumerThread()' thread will sleep for [{0}] milliseconds as circuit breaker is OPEN.", (object) this._circuitBreakerPolicyConfig.WaitTimeWhenCircuitBreakerIsOpen.TotalMilliseconds);
                Thread.Sleep(this._circuitBreakerPolicyConfig.WaitTimeWhenCircuitBreakerIsOpen);
                Global.Logger.InfoFormat("Resuming thread in 'RmqConsumer.StartConsumerThread()' after waiting [{0}] milliseconds.", (object) this._circuitBreakerPolicyConfig.WaitTimeWhenCircuitBreakerIsOpen.TotalMilliseconds);
              }
              else if (this._circuitBreakerPolicyConfig.CircuitBreakerPolicy.CircuitState == CircuitState.HalfOpen)
                this.WaitAndHandleMessageDelivery(handler1);
              else if (this.BatchSize > 1)
                this.WaitAndHandleMessageDelivery(handler2);
              else
                this.WaitAndHandleMessageDelivery(handler1);
            }
            else if (this.BatchSize > 1)
              this.WaitAndHandleMessageDelivery(handler2);
            else
              this.WaitAndHandleMessageDelivery(handler1);
          }
        }
        catch (ThreadStateException ex)
        {
          Global.Logger.WarnFormat("The consumer thread {0} got a ThreadStateException: {1}, {2}", (object) this.ConsumerTag, (object) ex.Message, (object) ex.StackTrace);
        }
        catch (ThreadInterruptedException ex)
        {
          Global.Logger.WarnFormat("The consumer thread {0} is interrupted", (object) this.ConsumerTag);
        }
        catch (ThreadAbortException ex)
        {
          Global.Logger.WarnFormat("The consumer thread {0} is aborted", (object) this.ConsumerTag);
        }
      }))
      {
        Name = threadName,
        Priority = ThreadPriority.AboveNormal,
        IsBackground = true
      }.Start();
    }

    protected virtual BasicDeliverEventArgs Dequeue() => this.Queue.Dequeue();

    protected virtual void CloseQueue() => this.Queue.Close();

    internal void WaitAndHandleMessageDelivery(Action<BasicDeliverEventArgs> handler)
    {
      try
      {
        BasicDeliverEventArgs eventArgs = (BasicDeliverEventArgs) null;
        lock (this._sharedQueueLock)
        {
          this._pool.WaitOne();
          if (this._status == ConsumerStatus.Active)
          {
            if (this._circuitBreakerPolicyConfig != null && this._circuitBreakerPolicyConfig.CircuitBreakerPolicy != null && this._circuitBreakerPolicyConfig.CircuitBreakerPolicy.CircuitState == CircuitState.Open)
            {
              Global.Logger.InfoFormat("2.1 Circuit Breaker is OPEN, message will not be Dequeued, releasing the semaphore and returning...");
              this._pool.Release();
              return;
            }
            eventArgs = this.Dequeue();
          }
        }
        if (eventArgs != null)
        {
          Global.MetricsCollector.RecordMessageWaitInQueueTime(this._queueName, MessageUtility.GetMessageWaitInQueueTime(eventArgs));
          lock (Subscription.OutstandingDeliveryTags)
          {
            if (Subscription.OutstandingDeliveryTags.ContainsKey(eventArgs.ConsumerTag))
              Subscription.OutstandingDeliveryTags[eventArgs.ConsumerTag].Add(eventArgs.DeliveryTag);
            else
              Global.Logger.InfoFormat("Consumer Tag cannot be located. It is either Consumer status became inactive \r\nor Channel has shut down. A message with DTag: {0} by CTag: {1} has been received and will proceed to handle as the last one.", (object) eventArgs.DeliveryTag, (object) eventArgs.ConsumerTag);
          }
          handler(eventArgs);
        }
        else
        {
          Global.FaultHandler.HandleFault("Message arrived but it's null for some reason, properly a serious BUG :D, contact author asap, release semaphore for other messages");
          this._pool.Release();
        }
      }
      catch (EndOfStreamException ex)
      {
        Thread.Sleep(100);
        this._pool.Release();
      }
      catch (Exception ex)
      {
        Global.Logger.Error(ex);
      }
    }

    private void MessageWasNotHandled(BasicDeliverEventArgs eventArgs)
    {
      try
      {
        if (this._autoAck || this.IsDisposed)
          return;
        this.DoAck(eventArgs, (IBasicConsumer) this);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
    }

    private void MessageHandlerHandlingComplete(BasicDeliverEventArgs eventArgs)
    {
      try
      {
        if (!this._autoAck || this.IsDisposed)
          return;
        this.DoAck(eventArgs, (IBasicConsumer) this);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
      }
      finally
      {
        this._pool.Release();
      }
    }

    protected virtual void WhenChannelShutdown(object sender, ShutdownEventArgs reason)
    {
      lock (Subscription.OutstandingDeliveryTags)
      {
        if (Subscription.OutstandingDeliveryTags.ContainsKey(this.ConsumerTag))
          Subscription.OutstandingDeliveryTags.TryRemove(this.ConsumerTag, out List<ulong> _);
      }
      this.CloseQueue();
      this._channelShutdown = true;
      Global.Logger.InfoFormat("Channel on queue {0} is shutdown: {1}", (object) this.ConsumerTag, (object) reason.ReplyText);
    }

    private void HandleMessageDeliveryInSeperatedThread(BasicDeliverEventArgs basicDeliverEventArgs)
    {
      Task.Factory.StartNew((Action) (() =>
      {
        try
        {
          using (Global.MetricsCollector.RecordHandleMessageDeliveryTime(this._messageHandler.GetType().Name))
            this._messageHandler.HandleMessage(basicDeliverEventArgs);
        }
        catch (Exception ex)
        {
          Global.FaultHandler.HandleFault(ex);
          this.Dispose();
        }
      }), Global.DefaultTaskCreationOptionsProvider());
    }

    private void HandleMessageDeliveryInSameThread(BasicDeliverEventArgs basicDeliverEventArgs)
    {
      try
      {
        using (Global.MetricsCollector.RecordHandleMessageDeliveryTime(this._messageHandler.GetType().Name))
          this._messageHandler.HandleMessage(basicDeliverEventArgs);
      }
      catch (Exception ex)
      {
        Global.FaultHandler.HandleFault(ex);
        this.Dispose();
      }
    }

    protected internal virtual void DoAck(
      BasicDeliverEventArgs basicDeliverEventArgs,
      IBasicConsumer subscriptionInfo)
    {
      if (this.IsDisposed)
        return;
      Subscription.TryAckOrNack(basicDeliverEventArgs.ConsumerTag, true, subscriptionInfo.Model, basicDeliverEventArgs.DeliveryTag, false, false);
    }

    public ConsumerStatus Status
    {
      get => this._status;
      set => this._status = value;
    }

    protected bool IsDisposed => this._status == ConsumerStatus.Disposed;

    public virtual void Dispose()
    {
      if (this.IsDisposed)
        return;
      this._status = ConsumerStatus.Disposing;
      DateTime dateTime = DateTime.Now.AddSeconds((double) Global.ConsumerDisposeTimeoutPolicy.TimeoutInSeconds);
      while (this.MessageInProgressCount() > 0 && DateTime.Now <= dateTime)
      {
        Global.Logger.InfoFormat("Wait for {0} messages in progress", (object) this.MessageInProgressCount());
        Thread.Sleep(1000);
      }
      this._status = ConsumerStatus.Disposed;
      this._pool.Dispose();
      this.CloseQueue();
    }

    private int MessageInProgressCount()
    {
      lock (Subscription.OutstandingDeliveryTags)
        return Subscription.OutstandingDeliveryTags.ContainsKey(this.ConsumerTag) ? Subscription.OutstandingDeliveryTags[this.ConsumerTag].Count : 0;
    }
  }
}
