// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartitionOffset
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents a Kafka (topic, partition, offset) tuple.
  /// </summary>
  public class TopicPartitionOffset
  {
    /// <summary>Initializes a new TopicPartitionOffset instance.</summary>
    /// <param name="tp">Kafka topic name and partition.</param>
    /// <param name="offset">A Kafka offset value.</param>
    public TopicPartitionOffset(TopicPartition tp, Offset offset)
      : this(tp.Topic, tp.Partition, offset)
    {
    }

    /// <summary>Initializes a new TopicPartitionOffset instance.</summary>
    /// <param name="topic">A Kafka topic name.</param>
    /// <param name="partition">A Kafka partition.</param>
    /// <param name="offset">A Kafka offset value.</param>
    public TopicPartitionOffset(string topic, Partition partition, Offset offset)
    {
      this.Topic = topic;
      this.Partition = partition;
      this.Offset = offset;
    }

    /// <summary>Gets the Kafka topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets the Kafka partition.</summary>
    public Partition Partition { get; }

    /// <summary>Gets the Kafka partition offset value.</summary>
    public Offset Offset { get; }

    /// <summary>
    ///     Gets the TopicPartition component of this TopicPartitionOffset instance.
    /// </summary>
    public TopicPartition TopicPartition => new TopicPartition(this.Topic, this.Partition);

    /// <summary>
    ///     Tests whether this TopicPartitionOffset instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a TopicPartitionOffset and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as TopicPartitionOffset) == null)
        return false;
      TopicPartitionOffset topicPartitionOffset = (TopicPartitionOffset) obj;
      return topicPartitionOffset.Partition == this.Partition && topicPartitionOffset.Topic == this.Topic && topicPartitionOffset.Offset == this.Offset;
    }

    /// <summary>
    ///     Returns a hash code for this TopicPartitionOffset.
    /// </summary>
    /// <returns>
    ///     An integer that specifies a hash value for this TopicPartitionOffset.
    /// </returns>
    public override int GetHashCode()
    {
      return (this.Partition.GetHashCode() * 251 + this.Topic.GetHashCode()) * 251 + this.Offset.GetHashCode();
    }

    /// <summary>
    ///     Tests whether TopicPartitionOffset instance a is equal to TopicPartitionOffset instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionOffset instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionOffset instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionOffset instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(TopicPartitionOffset a, TopicPartitionOffset b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether TopicPartitionOffset instance a is not equal to TopicPartitionOffset instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionOffset instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionOffset instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionOffset instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(TopicPartitionOffset a, TopicPartitionOffset b) => !(a == b);

    /// <summary>
    ///     Returns a string representation of the TopicPartitionOffset object.
    /// </summary>
    /// <returns>
    ///     A string that represents the TopicPartitionOffset object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} [{1}] @{2}", (object) this.Topic, (object) this.Partition, (object) this.Offset);
    }
  }
}
