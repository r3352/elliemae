// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ProducerBuilder`2
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
  ///     A builder class for <see cref="T:Confluent.Kafka.IProducer`2" />.
  /// </summary>
  public class ProducerBuilder<TKey, TValue>
  {
    /// <summary>The config dictionary.</summary>
    protected internal IEnumerable<KeyValuePair<string, string>> Config { get; set; }

    /// <summary>The configured error handler.</summary>
    protected internal Action<IProducer<TKey, TValue>, Error> ErrorHandler { get; set; }

    /// <summary>The configured log handler.</summary>
    protected internal Action<IProducer<TKey, TValue>, LogMessage> LogHandler { get; set; }

    /// <summary>The configured statistics handler.</summary>
    protected internal Action<IProducer<TKey, TValue>, string> StatisticsHandler { get; set; }

    /// <summary>The configured key serializer.</summary>
    protected internal ISerializer<TKey> KeySerializer { get; set; }

    /// <summary>The configured value serializer.</summary>
    protected internal ISerializer<TValue> ValueSerializer { get; set; }

    /// <summary>The configured async key serializer.</summary>
    protected internal IAsyncSerializer<TKey> AsyncKeySerializer { get; set; }

    /// <summary>The configured async value serializer.</summary>
    protected internal IAsyncSerializer<TValue> AsyncValueSerializer { get; set; }

    internal Producer<TKey, TValue>.Config ConstructBaseConfig(Producer<TKey, TValue> producer)
    {
      return new Producer<TKey, TValue>.Config()
      {
        config = this.Config,
        errorHandler = this.ErrorHandler == null ? (Action<Error>) null : (Action<Error>) (error => this.ErrorHandler((IProducer<TKey, TValue>) producer, error)),
        logHandler = this.LogHandler == null ? (Action<LogMessage>) null : (Action<LogMessage>) (logMessage => this.LogHandler((IProducer<TKey, TValue>) producer, logMessage)),
        statisticsHandler = this.StatisticsHandler == null ? (Action<string>) null : (Action<string>) (stats => this.StatisticsHandler((IProducer<TKey, TValue>) producer, stats))
      };
    }

    /// <summary>
    ///     A collection of librdkafka configuration parameters
    ///     (refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md)
    ///     and parameters specific to this client (refer to:
    ///     <see cref="T:Confluent.Kafka.ConfigPropertyNames" />).
    ///     At a minimum, 'bootstrap.servers' must be specified.
    /// </summary>
    public ProducerBuilder(IEnumerable<KeyValuePair<string, string>> config)
    {
      this.Config = config;
    }

    /// <summary>
    ///     Set the handler to call on statistics events. Statistics are provided as
    ///     a JSON formatted string as defined here:
    ///     https://github.com/edenhill/librdkafka/blob/master/STATISTICS.md
    /// </summary>
    /// <remarks>
    ///     You can enable statistics and set the statistics interval
    ///     using the StatisticsIntervalMs configuration property
    ///     (disabled by default).
    /// 
    ///     Executes on the poll thread (by default, a background thread managed by
    ///     the producer).
    /// 
    ///     Exceptions: Any exception thrown by your statistics handler
    ///     will be devivered to your error handler, if set, else they will be
    ///     silently ignored.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetStatisticsHandler(
      Action<IProducer<TKey, TValue>, string> statisticsHandler)
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
    ///     Executes on the poll thread (by default, a background thread managed by
    ///     the producer).
    /// 
    ///     Exceptions: Any exception thrown by your error handler will be silently
    ///     ignored.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetErrorHandler(
      Action<IProducer<TKey, TValue>, Error> errorHandler)
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
    ///     using the Debug configuration property.
    /// 
    ///     Warning: Log handlers are called spontaneously from internal
    ///     librdkafka threads and the application must not call any
    ///     Confluent.Kafka APIs from within a log handler or perform any
    ///     prolonged operations.
    /// 
    ///     Exceptions: Any exception thrown by your log handler will be
    ///     silently ignored.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetLogHandler(
      Action<IProducer<TKey, TValue>, LogMessage> logHandler)
    {
      this.LogHandler = this.LogHandler == null ? logHandler : throw new InvalidOperationException("Log handler may not be specified more than once.");
      return this;
    }

    /// <summary>The serializer to use to serialize keys.</summary>
    /// <remarks>
    ///     If your key serializer throws an exception, this will be
    ///     wrapped in a ProduceException with ErrorCode
    ///     Local_KeySerialization and thrown by the initiating call to
    ///     Produce or ProduceAsync.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetKeySerializer(ISerializer<TKey> serializer)
    {
      this.KeySerializer = this.KeySerializer == null && this.AsyncKeySerializer == null ? serializer : throw new InvalidOperationException("Key serializer may not be specified more than once.");
      return this;
    }

    /// <summary>The serializer to use to serialize values.</summary>
    /// <remarks>
    ///     If your value serializer throws an exception, this will be
    ///     wrapped in a ProduceException with ErrorCode
    ///     Local_ValueSerialization and thrown by the initiating call to
    ///     Produce or ProduceAsync.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetValueSerializer(ISerializer<TValue> serializer)
    {
      this.ValueSerializer = this.ValueSerializer == null && this.AsyncValueSerializer == null ? serializer : throw new InvalidOperationException("Value serializer may not be specified more than once.");
      return this;
    }

    /// <summary>The serializer to use to serialize keys.</summary>
    /// <remarks>
    ///     If your key serializer throws an exception, this will be
    ///     wrapped in a ProduceException with ErrorCode
    ///     Local_KeySerialization and thrown by the initiating call to
    ///     Produce or ProduceAsync.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetKeySerializer(IAsyncSerializer<TKey> serializer)
    {
      this.AsyncKeySerializer = this.KeySerializer == null && this.AsyncKeySerializer == null ? serializer : throw new InvalidOperationException("Key serializer may not be specified more than once.");
      return this;
    }

    /// <summary>The serializer to use to serialize values.</summary>
    /// <remarks>
    ///     If your value serializer throws an exception, this will be
    ///     wrapped in a ProduceException with ErrorCode
    ///     Local_ValueSerialization and thrown by the initiating call to
    ///     Produce or ProduceAsync.
    /// </remarks>
    public ProducerBuilder<TKey, TValue> SetValueSerializer(IAsyncSerializer<TValue> serializer)
    {
      this.AsyncValueSerializer = this.ValueSerializer == null && this.AsyncValueSerializer == null ? serializer : throw new InvalidOperationException("Value serializer may not be specified more than once.");
      return this;
    }

    /// <summary>Build a new IProducer implementation instance.</summary>
    public virtual IProducer<TKey, TValue> Build()
    {
      return (IProducer<TKey, TValue>) new Producer<TKey, TValue>(this);
    }
  }
}
