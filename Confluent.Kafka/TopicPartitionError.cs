// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartitionError
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents a Kafka (topic, partition, error) tuple.
  /// </summary>
  public class TopicPartitionError
  {
    /// <summary>Initializes a new TopicPartitionError instance.</summary>
    /// <param name="tp">Kafka topic name and partition values.</param>
    /// <param name="error">A Kafka error.</param>
    public TopicPartitionError(TopicPartition tp, Error error)
      : this(tp.Topic, tp.Partition, error)
    {
    }

    /// <summary>Initializes a new TopicPartitionError instance.</summary>
    /// <param name="topic">A Kafka topic name.</param>
    /// <param name="partition">A Kafka partition value.</param>
    /// <param name="error">A Kafka error.</param>
    public TopicPartitionError(string topic, Partition partition, Error error)
    {
      this.Topic = topic;
      this.Partition = partition;
      this.Error = error;
    }

    /// <summary>Gets the Kafka topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets the Kafka partition.</summary>
    public Partition Partition { get; }

    /// <summary>Gets the Kafka error.</summary>
    public Error Error { get; }

    /// <summary>
    ///     Gets the TopicPartition component of this TopicPartitionError instance.
    /// </summary>
    public TopicPartition TopicPartition => new TopicPartition(this.Topic, this.Partition);

    /// <summary>
    ///     Tests whether this TopicPartitionError instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a TopicPartitionError and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as TopicPartitionError) == null)
        return false;
      TopicPartitionError topicPartitionError = (TopicPartitionError) obj;
      return topicPartitionError.Partition == this.Partition && topicPartitionError.Topic == this.Topic && topicPartitionError.Error == this.Error;
    }

    /// <summary>Returns a hash code for this TopicPartitionError.</summary>
    /// <returns>
    ///     An integer that specifies a hash value for this TopicPartitionError.
    /// </returns>
    public override int GetHashCode()
    {
      return (this.Partition.GetHashCode() * 251 + this.Topic.GetHashCode()) * 251 + this.Error.GetHashCode();
    }

    /// <summary>
    ///     Tests whether TopicPartitionError instance a is equal to TopicPartitionError instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionError instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionError instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionError instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(TopicPartitionError a, TopicPartitionError b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether TopicPartitionError instance a is not equal to TopicPartitionError instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionError instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionError instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionError instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(TopicPartitionError a, TopicPartitionError b) => !(a == b);

    /// <summary>
    ///     Returns a string representation of the TopicPartitionError object.
    /// </summary>
    /// <returns>
    ///     A string representation of the TopicPartitionError object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} [{1}]: {2}", (object) this.Topic, (object) this.Partition, (object) this.Error);
    }
  }
}
