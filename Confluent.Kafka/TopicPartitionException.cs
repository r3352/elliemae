// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TopicPartitionException
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that occured during a Consumer.Position request.
  /// </summary>
  public class TopicPartitionException : KafkaException
  {
    /// <summary>
    ///     Initializes a new instance of OffsetsRequestExceptoion.
    /// </summary>
    /// <param name="results">
    ///     The result corresponding to all topic partitions of the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </param>
    public TopicPartitionException(List<TopicPartitionError> results)
      : base(new Error(ErrorCode.Local_Partial, "An error occurred for topic partitions: [" + string.Join<TopicPartition>(", ", results.Where<TopicPartitionError>((Func<TopicPartitionError, bool>) (r => r.Error.IsError)).Select<TopicPartitionError, TopicPartition>((Func<TopicPartitionError, TopicPartition>) (r => r.TopicPartition))) + "]."))
    {
      this.Results = results;
    }

    /// <summary>
    ///     The result corresponding to all ConfigResources in the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </summary>
    public List<TopicPartitionError> Results { get; }
  }
}
