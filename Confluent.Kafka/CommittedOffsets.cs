// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.CommittedOffsets
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Encapsulates information provided to a Consumer's OnOffsetsCommitted
  ///     event - per-partition offsets and success/error together with overall
  ///     success/error of the commit operation.
  /// </summary>
  /// <remarks>
  ///     Possible error conditions:
  ///     - Entire request failed: Error is set, but not per-partition errors.
  ///     - All partitions failed: Error is set to the value of the last failed partition, but each partition may have different errors.
  ///     - Some partitions failed: global error is success.
  /// </remarks>
  public class CommittedOffsets
  {
    /// <summary>Initializes a new instance of CommittedOffsets.</summary>
    /// <param name="offsets">
    ///     per-partition offsets and success/error.
    /// </param>
    /// <param name="error">overall operation success/error.</param>
    public CommittedOffsets(IList<TopicPartitionOffsetError> offsets, Error error)
    {
      this.Offsets = offsets;
      this.Error = error;
    }

    /// <summary>Gets the overall operation success/error.</summary>
    public Error Error { get; }

    /// <summary>Gets the per-partition offsets and success/error.</summary>
    public IList<TopicPartitionOffsetError> Offsets { get; }
  }
}
