// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicMetadata
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Metadata pertaining to a single Kafka topic.</summary>
  public class TopicMetadata
  {
    /// <summary>Initializes a new TopicMetadata class instance.</summary>
    /// <param name="topic">The topic name.</param>
    /// <param name="partitions">
    ///     Metadata for each of the topic's partitions.
    /// </param>
    /// <param name="error">
    ///     A rich <see cref="P:Confluent.Kafka.TopicMetadata.Error" /> object associated with the request for this topic metadata.
    /// </param>
    public TopicMetadata(string topic, List<PartitionMetadata> partitions, Error error)
    {
      this.Topic = topic;
      this.Error = error;
      this.Partitions = partitions;
    }

    /// <summary>Gets the topic name.</summary>
    public string Topic { get; }

    /// <summary>Gets metadata for each of the topics partitions.</summary>
    public List<PartitionMetadata> Partitions { get; }

    /// <summary>
    ///     A rich <see cref="P:Confluent.Kafka.TopicMetadata.Error" /> object associated with the request for this topic metadata.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Returns a JSON representation of the TopicMetadata object.
    /// </summary>
    /// <returns>A JSON representation the TopicMetadata object.</returns>
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{ \"Topic\": \"" + this.Topic + "\", \"Partitions\": [");
      stringBuilder.Append(string.Join(",", this.Partitions.Select<PartitionMetadata, string>((Func<PartitionMetadata, string>) (p => " " + p.ToString()))));
      stringBuilder.Append(" ], \"Error\": \"" + this.Error.Code.ToString() + "\" }");
      return stringBuilder.ToString();
    }
  }
}
