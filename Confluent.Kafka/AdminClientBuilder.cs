// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.AdminClientBuilder
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     A builder for <see cref="T:Confluent.Kafka.IAdminClient" />.
  /// </summary>
  public class AdminClientBuilder
  {
    /// <summary>The config dictionary.</summary>
    protected internal IEnumerable<KeyValuePair<string, string>> Config { get; set; }

    /// <summary>The configured error handler.</summary>
    protected internal Action<IAdminClient, Error> ErrorHandler { get; set; }

    /// <summary>The configured log handler.</summary>
    protected internal Action<IAdminClient, LogMessage> LogHandler { get; set; }

    /// <summary>The configured statistics handler.</summary>
    protected internal Action<IAdminClient, string> StatisticsHandler { get; set; }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.AdminClientBuilder" /> instance.
    /// </summary>
    /// <param name="config">
    ///     A collection of librdkafka configuration parameters
    ///     (refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md)
    ///     and parameters specific to this client (refer to:
    ///     <see cref="T:Confluent.Kafka.ConfigPropertyNames" />).
    ///     At a minimum, 'bootstrap.servers' must be specified.
    /// </param>
    public AdminClientBuilder(IEnumerable<KeyValuePair<string, string>> config)
    {
      this.Config = config;
    }

    /// <summary>
    ///     Set the handler to call on statistics events. Statistics are provided
    ///     as a JSON formatted string as defined here:
    ///     https://github.com/edenhill/librdkafka/blob/master/STATISTICS.md
    /// </summary>
    /// <remarks>
    ///     You can enable statistics by setting the statistics interval
    ///     using the statistics.interval.ms configuration parameter
    ///     (disabled by default).
    /// 
    ///     Executes on the poll thread (a background thread managed by
    ///     the admin client).
    /// </remarks>
    public AdminClientBuilder SetStatisticsHandler(Action<IAdminClient, string> statisticsHandler)
    {
      this.StatisticsHandler = this.StatisticsHandler == null ? statisticsHandler : throw new InvalidOperationException("Statistics handler may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     Set the handler to call on error events e.g. connection failures or all
    ///     brokers down. Note that the client will try to automatically recover from
    ///     errors that are not marked as fatal. Non-fatal errors should be interpreted
    ///     as informational rather than catastrophic.
    /// </summary>
    /// <remarks>
    ///     Executes on the poll thread (a background thread managed by the admin
    ///     client).
    /// </remarks>
    public AdminClientBuilder SetErrorHandler(Action<IAdminClient, Error> errorHandler)
    {
      this.ErrorHandler = this.ErrorHandler == null ? errorHandler : throw new InvalidOperationException("Error handler may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     Set the handler to call when there is information available
    ///     to be logged. If not specified, a default callback that writes
    ///     to stderr will be used.
    /// </summary>
    /// <remarks>
    ///     By default not many log messages are generated.
    /// 
    ///     For more verbose logging, specify one or more debug contexts
    ///     using the 'debug' configuration property.
    /// 
    ///     Warning: Log handlers are called spontaneously from internal
    ///     librdkafka threads and the application must not call any
    ///     Confluent.Kafka APIs from within a log handler or perform any
    ///     prolonged operations.
    /// </remarks>
    public AdminClientBuilder SetLogHandler(Action<IAdminClient, LogMessage> logHandler)
    {
      this.LogHandler = this.LogHandler == null ? logHandler : throw new InvalidOperationException("Log handler may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     Build the <see cref="T:Confluent.Kafka.AdminClient" /> instance.
    /// </summary>
    public virtual IAdminClient Build() => (IAdminClient) new AdminClient(this);
  }
}
