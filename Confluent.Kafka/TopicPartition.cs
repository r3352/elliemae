// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartition
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Represents a Kafka (topic, partition) tuple.</summary>
  public class TopicPartition
  {
    /// <summary>Initializes a new TopicPartition instance.</summary>
    /// <param name="topic">A Kafka topic name.</param>
    /// <param name="partition">A Kafka partition.</param>
    public TopicPartition(string topic, Partition partition)
    {
      this.Topic = topic;
      this.Partition = partition;
    }

    /// <summary>Gets the Kafka topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets the Kafka partition.</summary>
    public Partition Partition { get; }

    /// <summary>
    ///     Tests whether this TopicPartition instance is equal to the specified object.
    /// </summary>
    /// <param name="obj">The object to test.</param>
    /// <returns>
    ///     true if obj is a TopicPartition and all properties are equal. false otherwise.
    /// </returns>
    public override bool Equals(object obj)
    {
      if ((object) (obj as TopicPartition) == null)
        return false;
      TopicPartition topicPartition = (TopicPartition) obj;
      return topicPartition.Partition == this.Partition && topicPartition.Topic == this.Topic;
    }

    /// <summary>Returns a hash code for this TopicPartition.</summary>
    /// <returns>
    ///     An integer that specifies a hash value for this TopicPartition.
    /// </returns>
    public override int GetHashCode()
    {
      return this.Partition.GetHashCode() * 251 + this.Topic.GetHashCode();
    }

    /// <summary>
    ///     Tests whether TopicPartition instance a is equal to TopicPartition instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartition instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartition instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartition instances a and b are equal. false otherwise.
    /// </returns>
    public static bool operator ==(TopicPartition a, TopicPartition b)
    {
      return (object) a == null ? (object) b == null : a.Equals((object) b);
    }

    /// <summary>
    ///     Tests whether TopicPartition instance a is not equal to TopicPartition instance b.
    /// </summary>
    /// <param name="a">
    ///     The first TopicPartition instance to compare.
    /// </param>
    /// <param name="b">
    ///     The second TopicPartition instance to compare.
    /// </param>
    /// <returns>
    ///     true if TopicPartition instances a and b are not equal. false otherwise.
    /// </returns>
    public static bool operator !=(TopicPartition a, TopicPartition b) => !(a == b);

    /// <summary>
    ///     Returns a string representation of the TopicPartition object.
    /// </summary>
    /// <returns>
    ///     A string that represents the TopicPartition object.
    /// </returns>
    public override string ToString()
    {
      return string.Format("{0} [{1}]", (object) this.Topic, (object) this.Partition);
    }
  }
}
