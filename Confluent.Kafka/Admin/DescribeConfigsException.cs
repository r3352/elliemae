// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Admin.DescribeConfigsException
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
  ///     Represents an error that occured during a describe configs request.
  /// </summary>
  public class DescribeConfigsException : KafkaException
  {
    /// <summary>
    ///     Initializes a new instance of DescribeConfigsException.
    /// </summary>
    /// <param name="results">
    ///     The result corresponding to all ConfigResource in the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </param>
    public DescribeConfigsException(List<DescribeConfigsReport> results)
      : base(new Confluent.Kafka.Error(ErrorCode.Local_Partial, "An error occurred describing the following resources: [" + string.Join<ConfigResource>(", ", results.Where<DescribeConfigsReport>((Func<DescribeConfigsReport, bool>) (r => r.Error.IsError)).Select<DescribeConfigsReport, ConfigResource>((Func<DescribeConfigsReport, ConfigResource>) (r => r.ConfigResource))) + "]: [" + string.Join<Confluent.Kafka.Error>(", ", results.Where<DescribeConfigsReport>((Func<DescribeConfigsReport, bool>) (r => r.Error.IsError)).Select<DescribeConfigsReport, Confluent.Kafka.Error>((Func<DescribeConfigsReport, Confluent.Kafka.Error>) (r => r.Error))) + "]."))
    {
      this.Results = results;
    }

    /// <summary>
    ///     The result corresponding to all ConfigResources in the request
    ///     (whether or not they were in error). At least one of these
    ///     results will be in error.
    /// </summary>
    public List<DescribeConfigsReport> Results { get; }
  }
}
