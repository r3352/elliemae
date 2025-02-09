// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Metadata
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
  /// <summary>Kafka cluster metadata.</summary>
  public class Metadata
  {
    /// <summary>Instantiates a new Metadata class instance.</summary>
    /// <param name="brokers">
    ///     Information about each constituent broker of the cluster.
    /// </param>
    /// <param name="topics">
    ///     Information about requested topics in the cluster.
    /// </param>
    /// <param name="originatingBrokerId">
    ///     The id of the broker that provided this metadata.
    /// </param>
    /// <param name="originatingBrokerName">
    ///     The name of the broker that provided this metadata.
    /// </param>
    public Metadata(
      List<BrokerMetadata> brokers,
      List<TopicMetadata> topics,
      int originatingBrokerId,
      string originatingBrokerName)
    {
      this.Brokers = brokers;
      this.Topics = topics;
      this.OriginatingBrokerId = originatingBrokerId;
      this.OriginatingBrokerName = originatingBrokerName;
    }

    /// <summary>
    ///     Gets information about each constituent broker of the cluster.
    /// </summary>
    public List<BrokerMetadata> Brokers { get; }

    /// <summary>
    ///     Gets information about requested topics in the cluster.
    /// </summary>
    public List<TopicMetadata> Topics { get; }

    /// <summary>
    ///     Gets the id of the broker that provided this metadata.
    /// </summary>
    public int OriginatingBrokerId { get; }

    /// <summary>
    ///     Gets the name of the broker that provided this metadata.
    /// </summary>
    public string OriginatingBrokerName { get; }

    /// <summary>
    ///     Returns a JSON representation of the Metadata object.
    /// </summary>
    /// <returns>A JSON representation of the Metadata object.</returns>
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{{ \"OriginatingBrokerId\": {0}, \"OriginatingBrokerName\": \"{1}\", \"Brokers\": [", (object) this.OriginatingBrokerId, (object) this.OriginatingBrokerName));
      stringBuilder.Append(string.Join(",", this.Brokers.Select<BrokerMetadata, string>((Func<BrokerMetadata, string>) (b => " " + b.ToString()))));
      stringBuilder.Append(" ], \"Topics\": [");
      stringBuilder.Append(string.Join(",", this.Topics.Select<TopicMetadata, string>((Func<TopicMetadata, string>) (t => " " + t.ToString()))));
      stringBuilder.Append("] }");
      return stringBuilder.ToString();
    }
  }
}
