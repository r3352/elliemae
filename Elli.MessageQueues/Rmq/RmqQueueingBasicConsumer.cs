// Decompiled with JetBrains decompiler
// Type: Elli.MessageQueues.Rmq.RmqQueueingBasicConsumer
// Assembly: Elli.MessageQueues, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 24211DB4-B81B-430D-BE95-0449CADCF25D
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.MessageQueues.dll

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Util;
using System.Diagnostics.CodeAnalysis;

#nullable disable
namespace Elli.MessageQueues.Rmq
{
  public class RmqQueueingBasicConsumer : DefaultBasicConsumer, IQueueingBasicConsumer
  {
    public RmqQueueingBasicConsumer()
      : this((IModel) null)
    {
    }

    public RmqQueueingBasicConsumer(IModel model)
      : this(model, new SharedQueue<BasicDeliverEventArgs>())
    {
    }

    public RmqQueueingBasicConsumer(IModel model, SharedQueue<BasicDeliverEventArgs> queue)
      : base(model)
    {
      this.Queue = queue;
    }

    public SharedQueue<BasicDeliverEventArgs> Queue { get; protected set; }

    [ExcludeFromCodeCoverage]
    public override void HandleBasicDeliver(
      string consumerTag,
      ulong deliveryTag,
      bool redelivered,
      string exchange,
      string routingKey,
      IBasicProperties properties,
      byte[] body)
    {
      this.Queue.Enqueue(new BasicDeliverEventArgs()
      {
        ConsumerTag = consumerTag,
        DeliveryTag = deliveryTag,
        Redelivered = redelivered,
        Exchange = exchange,
        RoutingKey = routingKey,
        BasicProperties = properties,
        Body = body
      });
    }

    [ExcludeFromCodeCoverage]
    public override void OnCancel()
    {
      base.OnCancel();
      this.Queue.Close();
    }
  }
}
