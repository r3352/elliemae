// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartitionTimestamp
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents a Kafka (topic, partition, timestamp) tuple.
  /// </summary>
  public class TopicPartitionTimestamp
  {
    /// <summary>
    ///     Initializes a new TopicPartitionTimestamp instance.
    /// </summary>
    /// <param name="tp">Kafka topic name and partition.</param>
    /// <param name="timestamp">A Kafka timestamp value.</param>
    public TopicPartitionTimestamp(TopicPartition tp, Timestamp timestamp)
      : this(tp.Topic, tp.Partition, timestamp)
    {
    }

    /// <summary>
    ///     Initializes a new TopicPartitionTimestamp instance.
    /// </summary>
    /// <param name="topic">A Kafka topic name.</param>
    /// <param name="partition">A Kafka partition.</param>
    /// <param name="timestamp">A Kafka timestamp value.</param>
    public TopicPartitionTimestamp(string topic, Partition partition, Timestamp timestamp)
    {
      this.Topic = topic;
      this.Partition = partition;
      this.Timestamp = timestamp;
    }

    /// <summary>Gets the Kafka topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets the Kafka partition.</summary>
    public Partition Partition { get; }

    /// <summary>Gets the Kafka timestamp.</summary>
    public Timestamp Timestamp { get; }

    /// <summary>
    ///     Gets the TopicPartition component of this TopicPartitionTimestamp instance.
    /// </summary>
    public TopicPartition TopicPartition => new TopicPartition(this.Topic, this.Partition);

    /// <summary>
    ///     Tests whether this TopicPartitionTimestamp instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a TopicPartitionTimestamp and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as TopicPartitionTimestamp) == null)
        return false;
      TopicPartitionTimestamp partitionTimestamp = (TopicPartitionTimestamp) obj;
      return partitionTimestamp.Partition == this.Partition && partitionTimestamp.Topic == this.Topic && partitionTimestamp.Timestamp == this.Timestamp;
    }

    /// <summary>
    ///     Returns a hash code for this TopicPartitionTimestamp.
    /// </summary>
    /// <returns>
    ///     An integer that specifies a hash value for this TopicPartitionTimestamp.
    /// </returns>
    public override int GetHashCode()
    {
      return (this.Partition.GetHashCode() * 251 + this.Topic.GetHashCode()) * 251 + this.Timestamp.GetHashCode();
    }

    /// <summary>
    ///     Tests whether TopicPartitionTimestamp instance a is equal to TopicPartitionTimestamp instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionTimestamp instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionTimestamp instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionTimestamp instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(TopicPartitionTimestamp a, TopicPartitionTimestamp b)
    {
      return a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether TopicPartitionTimestamp instance a is not equal to TopicPartitionTimestamp instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionTimestamp instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionTimestamp instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionTimestamp instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(TopicPartitionTimestamp a, TopicPartitionTimestamp b)
    {
      return !(a == b);
    }

    /// <summary>
    ///     Returns a string representation of the TopicPartitionTimestamp object.
    /// </summary>
    /// <returns>
    ///     A string that represents the TopicPartitionTimestamp object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} [{1}] @{2}", (object) this.Topic, (object) this.Partition, (object) this.Timestamp);
    }
  }
}
