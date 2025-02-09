// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ConsumeResult`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents a message consumed from a Kafka cluster.
  /// </summary>
  public class ConsumeResult<TKey, TValue>
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

    /// <summary>
    ///     The Kafka message, or null if this ConsumeResult
    ///     instance represents an end of partition event.
    /// </summary>
    public Confluent.Kafka.Message<TKey, TValue> Message { get; set; }

    /// <summary>The Kafka message Key.</summary>
    [Obsolete("Please access the message Key via .Message.Key.")]
    public TKey Key => this.Message != null ? this.Message.Key : throw new MessageNullException();

    /// <summary>The Kafka message Value.</summary>
    [Obsolete("Please access the message Value via .Message.Value.")]
    public TValue Value
    {
      get => this.Message != null ? this.Message.Value : throw new MessageNullException();
    }

    /// <summary>The Kafka message timestamp.</summary>
    [Obsolete("Please access the message Timestamp via .Message.Timestamp.")]
    public Timestamp Timestamp
    {
      get => this.Message != null ? this.Message.Timestamp : throw new MessageNullException();
    }

    /// <summary>The Kafka message headers.</summary>
    [Obsolete("Please access the message Headers via .Message.Headers.")]
    public Headers Headers
    {
      get => this.Message != null ? this.Message.Headers : throw new MessageNullException();
    }

    /// <summary>
    ///     True if this instance represents an end of partition
    ///     event, false if it represents a message in kafka.
    /// </summary>
    public bool IsPartitionEOF { get; set; }
  }
}
