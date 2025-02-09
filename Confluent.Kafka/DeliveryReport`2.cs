// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.DeliveryReport`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>The result of a produce request.</summary>
  public class DeliveryReport<TKey, TValue> : DeliveryResult<TKey, TValue>
  {
    /// <summary>
    ///     An error (or NoError) associated with the message.
    /// </summary>
    public Error Error { get; set; }

    /// <summary>
    ///     The TopicPartitionOffsetError associated with the message.
    /// </summary>
    public TopicPartitionOffsetError TopicPartitionOffsetError
    {
      get => new TopicPartitionOffsetError(this.Topic, this.Partition, this.Offset, this.Error);
      set
      {
        this.Topic = value.Topic;
        this.Partition = value.Partition;
        this.Offset = value.Offset;
        this.Error = value.Error;
      }
    }
  }
}
