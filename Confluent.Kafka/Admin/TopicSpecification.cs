// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.TopicSpecification
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     Specification of a new topic to be created via the CreateTopics
  ///     method. This class is used for the same purpose as NewTopic in
  ///     the Java API.
  /// </summary>
  public class TopicSpecification
  {
    /// <summary>The configuration to use to create the new topic.</summary>
    public Dictionary<string, string> Configs { get; set; }

    /// <summary>The name of the topic to be created (required).</summary>
    public string Name { get; set; }

    /// <summary>
    ///     The number of partitions for the new topic or -1 (the default) if a
    ///     replica assignment is specified.
    /// </summary>
    public int NumPartitions { get; set; } = -1;

    /// <summary>
    ///     A map from partition id to replica ids (i.e., static broker ids) or null
    ///     if the number of partitions and replication factor are specified
    ///     instead.
    /// </summary>
    public Dictionary<int, List<int>> ReplicasAssignments { get; set; }

    /// <summary>
    ///     The replication factor for the new topic or -1 (the default) if a
    ///     replica assignment is specified instead.
    /// </summary>
    public short ReplicationFactor { get; set; } = -1;
  }
}
