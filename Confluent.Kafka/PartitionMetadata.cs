// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.PartitionMetadata
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
  /// <summary>
  ///     Metadata pertaining to a single Kafka topic partition.
  /// </summary>
  public class PartitionMetadata
  {
    /// <summary>Initializes a new PartitionMetadata instance.</summary>
    /// <param name="partitionId">
    ///     The id of the partition this metadata relates to.
    /// </param>
    /// <param name="leader">
    ///     The id of the broker that is the leader for the partition.
    /// </param>
    /// <param name="replicas">
    ///     The ids of all brokers that contain replicas of the partition.
    /// </param>
    /// <param name="inSyncReplicas">
    ///     The ids of all brokers that contain in-sync replicas of the partition.
    ///     Note: this value is cached by the broker and is consequently not guaranteed to be up-to-date.
    /// </param>
    /// <param name="error">
    ///     A rich <see cref="P:Confluent.Kafka.PartitionMetadata.Error" /> object associated with the request for this partition metadata.
    /// </param>
    public PartitionMetadata(
      int partitionId,
      int leader,
      int[] replicas,
      int[] inSyncReplicas,
      Error error)
    {
      this.PartitionId = partitionId;
      this.Leader = leader;
      this.Replicas = replicas;
      this.InSyncReplicas = inSyncReplicas;
      this.Error = error;
    }

    /// <summary>
    ///     Gets ths id of the partition this metadata relates to.
    /// </summary>
    public int PartitionId { get; }

    /// <summary>
    ///     Gets the id of the broker that is the leader for the partition.
    /// </summary>
    public int Leader { get; }

    /// <summary>
    ///     Gets the ids of all brokers that contain replicas of the partition.
    /// </summary>
    public int[] Replicas { get; }

    /// <summary>
    ///     Gets the ids of all brokers that contain in-sync replicas of the partition.
    /// </summary>
    public int[] InSyncReplicas { get; }

    /// <summary>
    ///     Gets a rich <see cref="P:Confluent.Kafka.PartitionMetadata.Error" /> object associated with the request for this partition metadata.
    ///     Note: this value is cached by the broker and is consequently not guaranteed to be up-to-date.
    /// </summary>
    public Error Error { get; }

    /// <summary>
    ///     Returns a JSON representation of the PartitionMetadata object.
    /// </summary>
    /// <returns>
    ///     A JSON representation the PartitionMetadata object.
    /// </returns>
    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{{ \"PartitionId\": {0}, \"Leader\": {1}, \"Replicas\": [", (object) this.PartitionId, (object) this.Leader));
      stringBuilder.Append(string.Join(",", ((IEnumerable<int>) this.Replicas).Select<int, string>((Func<int, string>) (r => " " + r.ToString()))));
      stringBuilder.Append(" ], \"InSyncReplicas\": [");
      stringBuilder.Append(string.Join(",", ((IEnumerable<int>) this.InSyncReplicas).Select<int, string>((Func<int, string>) (r => " " + r.ToString()))));
      stringBuilder.Append(" ], \"Error\": \"" + this.Error.Code.ToString() + "\" }");
      return stringBuilder.ToString();
    }
  }
}
