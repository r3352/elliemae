// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.DeliveryResult`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Encapsulates the result of a successful produce request.
  /// </summary>
  public class DeliveryResult<TKey, TValue>
  {
    /// <summary>The topic associated with the message.</summary>
    public string Topic { get; set; }

    /// <summary>The partition associated with the message.</summary>
    public Partition Partition { get; set; }

    /// <summary>The partition offset associated with the message.</summary>
    public Offset Offset { get; set; }

    /// <summary>The TopicPartition associated with the message.</summary>
    public TopicPartition TopicPartition => new TopicPartition(this.Topic, this.Partition);

    /// <summary>
    ///     The TopicPartitionOffset associated with the message.
    /// </summary>
    public TopicPartitionOffset TopicPartitionOffset
    {
      get => new TopicPartitionOffset(this.Topic, this.Partition, this.Offset);
      set
      {
        this.Topic = value.Topic;
        this.Partition = value.Partition;
        this.Offset = value.Offset;
      }
    }

    /// <summary>The persistence status of the message</summary>
    public PersistenceStatus Status { get; set; }

    /// <summary>The Kafka message.</summary>
    public Confluent.Kafka.Message<TKey, TValue> Message { get; set; }

    /// <summary>The Kafka message Key.</summary>
    public TKey Key
    {
      get => this.Message.Key;
      set => this.Message.Key = value;
    }

    /// <summary>The Kafka message Value.</summary>
    public TValue Value
    {
      get => this.Message.Value;
      set => this.Message.Value = value;
    }

    /// <summary>The Kafka message timestamp.</summary>
    public Timestamp Timestamp
    {
      get => this.Message.Timestamp;
      set => this.Message.Timestamp = value;
    }

    /// <summary>The Kafka message headers.</summary>
    public Headers Headers
    {
      get => this.Message.Headers;
      set => this.Message.Headers = value;
    }
  }
}
