// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.Subscription
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  internal class Subscription : ISubscription, IDisposable
  {
    internal const string CloseByApplication = "Closed by application";
    private const string FailedToAckMessage = "Basic ack/nack failed because chanel was closed with message {0}. Message remains on RabbitMQ and will be retried.";
    internal static ConcurrentDictionary<string, List<ulong>> OutstandingDeliveryTags = new ConcurrentDictionary<string, List<ulong>>();
    private IModel _channel;
    private DistributedMutex _mutex;

    public string QueueName { get; set; }

    public string SubscriptionName { get; set; }

    public string ConsumerTag { get; set; }

    public bool Exclusive { get; set; }

    public CancellationToken CancellationToken { get; set; }

    protected internal Subscription()
    {
    }

    protected internal Subscription(IModel channel)
      : this()
    {
      this.SetChannel(channel);
    }

    public void SetChannel(IModel channel)
    {
      this._channel = channel != null ? channel : throw new ArgumentNullException("Subscription.SetChannel(channel)");
    }

    public void Cancel()
    {
      this.TryCancel((Action<IModel>) (x => x.BasicCancel(this.ConsumerTag)), this._channel);
    }

    public void Ack(ulong deliveryTag) => this.TryAck(this._channel, deliveryTag, false);

    public void AckAllUpTo(ulong deliveryTag) => this.TryAck(this._channel, deliveryTag, true);

    public void Ack(IEnumerable<ulong> deliveryTags)
    {
      List<ulong> ulongList = deliveryTags != null ? deliveryTags.ToList<ulong>() : throw new ArgumentNullException("Subscription.Ack(deliveryTags)");
      if (ulongList.Count == 0)
        return;
      lock (Subscription.OutstandingDeliveryTags)
      {
        ulong num = ulongList.Max<ulong>();
        if (Subscription.OutstandingDeliveryTags.ContainsKey(this.ConsumerTag) && this.CanAckNackAll(Subscription.OutstandingDeliveryTags[this.ConsumerTag], ulongList, num))
          this.TryAck(this._channel, num, true);
        else
          ulongList.ForEach((Action<ulong>) (t => this.TryAck(this._channel, t, false)));
      }
    }

    private bool CanAckNackAll(List<ulong> outstandingList, List<ulong> list, ulong maxTag)
    {
      Dictionary<ulong, bool> dictionary = list.ToDictionary<ulong, ulong, bool>((Func<ulong, ulong>) (x => x), (Func<ulong, bool>) (x => true));
      foreach (ulong outstanding in outstandingList)
      {
        if (!dictionary.ContainsKey(outstanding) && outstanding < maxTag)
          return false;
      }
      return true;
    }

    public void AckAllOutstandingMessages() => this.TryAck(this._channel, 0UL, true);

    public void Nack(ulong deliveryTag, bool requeue)
    {
      this.TryNack(this._channel, deliveryTag, false, requeue);
    }

    public void NackAllUpTo(ulong deliveryTag, bool requeue)
    {
      this.TryNack(this._channel, deliveryTag, true, requeue);
    }

    public void Nack(IEnumerable<ulong> deliveryTags, bool requeue)
    {
      List<ulong> ulongList = deliveryTags != null ? deliveryTags.ToList<ulong>() : throw new ArgumentNullException("Subscription.Nack(deliveryTags)");
      if (ulongList.Count == 0)
        return;
      lock (Subscription.OutstandingDeliveryTags)
      {
        ulong num = ulongList.Max<ulong>();
        if (Subscription.OutstandingDeliveryTags.ContainsKey(this.ConsumerTag) && this.CanAckNackAll(Subscription.OutstandingDeliveryTags[this.ConsumerTag], ulongList, num))
          this.TryNack(this._channel, num, true, requeue);
        else
          ulongList.ForEach((Action<ulong>) (t => this.TryNack(this._channel, t, false, requeue)));
      }
    }

    public void NackAllOutstandingMessages(bool requeue)
    {
      this.TryNack(this._channel, 0UL, true, requeue);
    }

    internal static void TryAckOrNack(
      string consumerTag,
      bool ack,
      IModel channel,
      ulong deliveryTag,
      bool multiple,
      bool requeue)
    {
      try
      {
        if (channel == null)
          Global.Logger.WarnFormat("Trying ack/nack msg but the Channel is null, will not do anything");
        else if (!channel.IsOpen)
        {
          Global.Logger.WarnFormat("Trying ack/nack msg but the Channel is not open, will not do anything");
        }
        else
        {
          if (ack)
            channel.BasicAck(deliveryTag, multiple);
          else
            channel.BasicNack(deliveryTag, multiple, requeue);
          lock (Subscription.OutstandingDeliveryTags)
          {
            if (!Subscription.OutstandingDeliveryTags.ContainsKey(consumerTag))
              return;
            if (deliveryTag == 0UL)
              Subscription.OutstandingDeliveryTags[consumerTag].Clear();
            else if (multiple)
              Subscription.OutstandingDeliveryTags[consumerTag].RemoveAll((Predicate<ulong>) (x => x <= deliveryTag));
            else
              Subscription.OutstandingDeliveryTags[consumerTag].Remove(deliveryTag);
          }
        }
      }
      catch (AlreadyClosedException ex)
      {
        Global.Logger.WarnFormat("Basic ack/nack failed because chanel was closed with message {0}. Message remains on RabbitMQ and will be retried.", (object) ex.Message);
      }
      catch (IOException ex)
      {
        Global.Logger.WarnFormat("Basic ack/nack failed because chanel was closed with message {0}. Message remains on RabbitMQ and will be retried.", (object) ex.Message);
      }
    }

    private void TryAck(IModel channel, ulong deliveryTag, bool multiple)
    {
      using (Global.MetricsCollector.RecordMessageAckTime())
        Subscription.TryAckOrNack(this.ConsumerTag, true, channel, deliveryTag, multiple, false);
    }

    private void TryNack(IModel channel, ulong deliveryTag, bool multiple, bool requeue)
    {
      Subscription.TryAckOrNack(this.ConsumerTag, false, channel, deliveryTag, multiple, requeue);
    }

    internal void TryCancel(Action<IModel> action, IModel channel)
    {
      try
      {
        Global.Logger.InfoFormat("Cancelling subscription {0} on queue {1}", (object) this.SubscriptionName, (object) this.QueueName);
        action(channel);
        Global.Logger.InfoFormat("Subscription {0}  on queue {1} cancelled", (object) this.SubscriptionName, (object) this.QueueName);
      }
      catch (AlreadyClosedException ex)
      {
        Global.Logger.WarnFormat("Action failed because chanel was closed with message {0}. ", (object) ex.Message);
      }
      catch (IOException ex)
      {
        Global.Logger.WarnFormat("Action failed because chanel was closed with message {0}. ", (object) ex.Message);
      }
      finally
      {
        this.ReleaseExclusivity();
      }
    }

    internal void EnsureExclusivity(IDurableConnection connection)
    {
      if (!this.Exclusive)
        return;
      string str = "HostName:" + connection.HostName + "VirtualHost:" + connection.VirtualHost + "Queue:" + this.QueueName;
      this._mutex = new DistributedMutex(Convert.ToBase64String(Encoding.UTF8.GetBytes("[StartSubscription]" + str + "[EndSubscription]")));
      if (!this._mutex.WaitOne(Global.DistributedMutexPolicy.GetMutexTimeout()))
        throw new Exception("Timeout waiting to obtain mutex on queue '" + str + "'");
    }

    internal void ReleaseExclusivity()
    {
      try
      {
        if (this._mutex == null)
          return;
        this._mutex.Dispose();
      }
      catch
      {
      }
    }

    public void Dispose() => this.ReleaseExclusivity();
  }
}
