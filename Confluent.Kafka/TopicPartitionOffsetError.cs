// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartitionOffsetError
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents a Kafka (topic, partition, offset, error) tuple.
  /// </summary>
  public class TopicPartitionOffsetError
  {
    /// <summary>
    ///     Initializes a new TopicPartitionOffsetError instance.
    /// </summary>
    /// <param name="tp">Kafka topic name and partition values.</param>
    /// <param name="offset">A Kafka offset value.</param>
    /// <param name="error">A Kafka error.</param>
    public TopicPartitionOffsetError(TopicPartition tp, Offset offset, Error error)
      : this(tp.Topic, tp.Partition, offset, error)
    {
    }

    /// <summary>
    ///     Initializes a new TopicPartitionOffsetError instance.
    /// </summary>
    /// <param name="tpo">
    ///     Kafka topic name, partition and offset values.
    /// </param>
    /// <param name="error">A Kafka error.</param>
    public TopicPartitionOffsetError(TopicPartitionOffset tpo, Error error)
      : this(tpo.Topic, tpo.Partition, tpo.Offset, error)
    {
    }

    /// <summary>
    ///     Initializes a new TopicPartitionOffsetError instance.
    /// </summary>
    /// <param name="topic">A Kafka topic name.</param>
    /// <param name="partition">A Kafka partition value.</param>
    /// <param name="offset">A Kafka offset value.</param>
    /// <param name="error">A Kafka error.</param>
    public TopicPartitionOffsetError(
      string topic,
      Partition partition,
      Offset offset,
      Error error)
    {
      this.Topic = topic;
      this.Partition = partition;
      this.Offset = offset;
      this.Error = error;
    }

    /// <summary>Gets the Kafka topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets the Kafka partition.</summary>
    public Partition Partition { get; }

    /// <summary>Gets the Kafka partition offset value.</summary>
    public Offset Offset { get; }

    /// <summary>Gets the Kafka error.</summary>
    public Error Error { get; }

    /// <summary>
    ///     Gets the TopicPartition component of this TopicPartitionOffsetError instance.
    /// </summary>
    public TopicPartition TopicPartition => new TopicPartition(this.Topic, this.Partition);

    /// <summary>
    ///     Gets the TopicPartitionOffset component of this TopicPartitionOffsetError instance.
    /// </summary>
    /// &gt;
    public TopicPartitionOffset TopicPartitionOffset
    {
      get => new TopicPartitionOffset(this.Topic, this.Partition, this.Offset);
    }

    /// <summary>
    ///     Tests whether this TopicPartitionOffsetError instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a TopicPartitionOffsetError and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as TopicPartitionOffsetError) == null)
        return false;
      TopicPartitionOffsetError partitionOffsetError = (TopicPartitionOffsetError) obj;
      return partitionOffsetError.Partition == this.Partition && partitionOffsetError.Topic == this.Topic && partitionOffsetError.Offset == this.Offset && partitionOffsetError.Error == this.Error;
    }

    /// <summary>
    ///     Returns a hash code for this TopicPartitionOffsetError.
    /// </summary>
    /// <returns>
    ///     An integer that specifies a hash value for this TopicPartitionOffsetError.
    /// </returns>
    public override int GetHashCode()
    {
      return ((this.Partition.GetHashCode() * 251 + this.Topic.GetHashCode()) * 251 + this.Offset.GetHashCode()) * 251 + this.Error.GetHashCode();
    }

    /// <summary>
    ///     Tests whether TopicPartitionOffsetError instance a is equal to TopicPartitionOffsetError instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionOffsetError instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionOffsetError instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionOffsetError instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(TopicPartitionOffsetError a, TopicPartitionOffsetError b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether TopicPartitionOffsetError instance a is not equal to TopicPartitionOffsetError instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartitionOffsetError instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartitionOffsetError instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartitionOffsetError instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(TopicPartitionOffsetError a, TopicPartitionOffsetError b)
    {
      return !(a == b);
    }

    /// <summary>
    ///     Converts TopicPartitionOffsetError instance to TopicPartitionOffset instance.
    ///     NOTE: Throws KafkaException if Error.Code != ErrorCode.NoError
    /// </summary>
    /// <param name="tpoe">
    ///     The TopicPartitionOffsetError instance to convert.
    /// </param>
    /// <returns>
    ///     TopicPartitionOffset instance converted from TopicPartitionOffsetError instance
    /// </returns>
    public static explicit operator TopicPartitionOffset(TopicPartitionOffsetError tpoe)
    {
      if (tpoe.Error.IsError)
        throw new KafkaException(tpoe.Error);
      return tpoe.TopicPartitionOffset;
    }

    /// <summary>
    ///     Returns a string representation of the TopicPartitionOffsetError object.
    /// </summary>
    /// <returns>
    ///     A string representation of the TopicPartitionOffsetError object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} [{1}] @{2}: {3}", (object) this.Topic, (object) this.Partition, (object) this.Offset, (object) this.Error);
    }
  }
}
