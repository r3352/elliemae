// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.CreatePartitionsException
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Confluent.Kafka.Admin
{
  /// <summary>
  ///     Represents an error that occured during a create partitions request.
  /// </summary>
  public class CreatePartitionsException : KafkaException
  {
    /// <summary>
    ///     Initialize a new instance of CreatePartitionsException.
    /// </summary>
    /// <param name="results">
    ///     The result corresponding to all topics in the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </param>
    public CreatePartitionsException(List<CreatePartitionsReport> results)
      : base(new Confluent.Kafka.Error(ErrorCode.Local_Partial, "An error occurred creating partitions for topics: [" + string.Join(", ", results.Where<CreatePartitionsReport>((Func<CreatePartitionsReport, bool>) (r => r.Error.IsError)).Select<CreatePartitionsReport, string>((Func<CreatePartitionsReport, string>) (r => r.Topic))) + "]: [" + string.Join<Confluent.Kafka.Error>(", ", results.Where<CreatePartitionsReport>((Func<CreatePartitionsReport, bool>) (r => r.Error.IsError)).Select<CreatePartitionsReport, Confluent.Kafka.Error>((Func<CreatePartitionsReport, Confluent.Kafka.Error>) (r => r.Error))) + "]."))
    {
      this.Results = results;
    }

    /// <summary>
    ///     The result corresponding to all topics in the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </summary>
    public List<CreatePartitionsReport> Results { get; }
  }
}
