// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ConsumerBuilder`2
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
  ///     A builder class for <see cref="T:Confluent.Kafka.IConsumer`2" />.
  /// </summary>
  public class ConsumerBuilder<TKey, TValue>
  {
    /// <summary>The config dictionary.</summary>
    protected internal IEnumerable<KeyValuePair<string, string>> Config { get; set; }

    /// <summary>The configured error handler.</summary>
    protected internal Action<IConsumer<TKey, TValue>, Error> ErrorHandler { get; set; }

    /// <summary>The configured log handler.</summary>
    protected internal Action<IConsumer<TKey, TValue>, LogMessage> LogHandler { get; set; }

    /// <summary>The configured statistics handler.</summary>
    protected internal Action<IConsumer<TKey, TValue>, string> StatisticsHandler { get; set; }

    /// <summary>The configured key deserializer.</summary>
    protected internal IDeserializer<TKey> KeyDeserializer { get; set; }

    /// <summary>The configured value deserializer.</summary>
    protected internal IDeserializer<TValue> ValueDeserializer { get; set; }

    /// <summary>The configured partitions assigned handler.</summary>
    protected internal Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>> PartitionsAssignedHandler { get; set; }

    /// <summary>The configured partitions revoked handler.</summary>
    protected internal Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>> PartitionsRevokedHandler { get; set; }

    /// <summary>The configured offsets committed handler.</summary>
    protected internal Action<IConsumer<TKey, TValue>, CommittedOffsets> OffsetsCommittedHandler { get; set; }

    internal Consumer<TKey, TValue>.Config ConstructBaseConfig(Consumer<TKey, TValue> consumer)
    {
      return new Consumer<TKey, TValue>.Config()
      {
        config = this.Config,
        errorHandler = this.ErrorHandler == null ? (Action<Error>) null : (Action<Error>) (error => this.ErrorHandler((IConsumer<TKey, TValue>) consumer, error)),
        logHandler = this.LogHandler == null ? (Action<LogMessage>) null : (Action<LogMessage>) (logMessage => this.LogHandler((IConsumer<TKey, TValue>) consumer, logMessage)),
        statisticsHandler = this.StatisticsHandler == null ? (Action<string>) null : (Action<string>) (stats => this.StatisticsHandler((IConsumer<TKey, TValue>) consumer, stats)),
        offsetsCommittedHandler = this.OffsetsCommittedHandler == null ? (Action<CommittedOffsets>) null : (Action<CommittedOffsets>) (offsets => this.OffsetsCommittedHandler((IConsumer<TKey, TValue>) consumer, offsets)),
        partitionsAssignedHandler = this.PartitionsAssignedHandler == null ? (Func<List<TopicPartition>, IEnumerable<TopicPartitionOffset>>) null : (Func<List<TopicPartition>, IEnumerable<TopicPartitionOffset>>) (partitions => this.PartitionsAssignedHandler((IConsumer<TKey, TValue>) consumer, partitions)),
        partitionsRevokedHandler = this.PartitionsRevokedHandler == null ? (Func<List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>) null : (Func<List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>) (partitions => this.PartitionsRevokedHandler((IConsumer<TKey, TValue>) consumer, partitions))
      };
    }

    /// <summary>Initialize a new ConsumerBuilder instance.</summary>
    /// <param name="config">
    ///     A collection of librdkafka configuration parameters
    ///     (refer to https://github.com/edenhill/librdkafka/blob/master/CONFIGURATION.md)
    ///     and parameters specific to this client (refer to:
    ///     <see cref="T:Confluent.Kafka.ConfigPropertyNames" />).
    ///     At a minimum, 'bootstrap.servers' and 'group.id' must be
    ///     specified.
    /// </param>
    public ConsumerBuilder(IEnumerable<KeyValuePair<string, string>> config)
    {
      this.Config = config;
    }

    /// <summary>
    ///     Set the handler to call on statistics events. Statistics
    ///     are provided as a JSON formatted string as defined here:
    ///     https://github.com/edenhill/librdkafka/blob/master/STATISTICS.md
    /// </summary>
    /// <remarks>
    ///     You can enable statistics and set the statistics interval
    ///     using the StatisticsIntervalMs configuration property
    ///     (disabled by default).
    /// 
    ///     Executes as a side-effect of the Consume method (on the same
    ///     thread).
    /// 
    ///     Exceptions: Any exception thrown by your statistics handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call
    ///     to Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetStatisticsHandler(
      Action<IConsumer<TKey, TValue>, string> statisticsHandler)
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
    ///     Executes as a side-effect of the Consume method (on the same thread).
    /// 
    ///     Exceptions: Any exception thrown by your error handler will be silently
    ///     ignored.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetErrorHandler(
      Action<IConsumer<TKey, TValue>, Error> errorHandler)
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
    ///     using the 'Debug' configuration property.
    /// 
    ///     Warning: Log handlers are called spontaneously from internal
    ///     librdkafka threads and the application must not call any
    ///     Confluent.Kafka APIs from within a log handler or perform any
    ///     prolonged operations.
    /// 
    ///     Exceptions: Any exception thrown by your log handler will be
    ///     silently ignored.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetLogHandler(
      Action<IConsumer<TKey, TValue>, LogMessage> logHandler)
    {
      this.LogHandler = this.LogHandler == null ? logHandler : throw new InvalidOperationException("Log handler may not be specified more than once.");
      return this;
    }

    /// <summary>Set the deserializer to use to deserialize keys.</summary>
    /// <remarks>
    ///     If your key deserializer throws an exception, this will be
    ///     wrapped in a ConsumeException with ErrorCode
    ///     Local_KeyDeserialization and thrown by the initiating call to
    ///     Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetKeyDeserializer(IDeserializer<TKey> deserializer)
    {
      this.KeyDeserializer = this.KeyDeserializer == null ? deserializer : throw new InvalidOperationException("Key deserializer may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     Set the deserializer to use to deserialize values.
    /// </summary>
    /// <remarks>
    ///     If your value deserializer throws an exception, this will be
    ///     wrapped in a ConsumeException with ErrorCode
    ///     Local_ValueDeserialization and thrown by the initiating call to
    ///     Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetValueDeserializer(IDeserializer<TValue> deserializer)
    {
      this.ValueDeserializer = this.ValueDeserializer == null ? deserializer : throw new InvalidOperationException("Value deserializer may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     This handler is called when a new consumer group partition assignment has been received
    ///     by this consumer.
    /// 
    ///     Note: corresponding to every call to this handler there will be a corresponding call to
    ///     the partitions revoked handler (if one has been set using SetPartitionsRevokedHandler).
    /// 
    ///     The actual partitions to consume from and start offsets are specified by the return value
    ///     of the handler. This set of partitions is not required to match the assignment provided
    ///     by the consumer group, but typically will. Partition offsets may be a specific offset, or
    ///     special value (Beginning, End or Unset). If Unset, consumption will resume from the
    ///     last committed offset for each partition, or if there is no committed offset, in accordance
    ///     with the `auto.offset.reset` configuration property.
    /// </summary>
    /// <remarks>
    ///     May execute as a side-effect of the Consumer.Consume call (on the same thread).
    /// 
    ///     Assign/Unassign must not be called in the handler.
    /// 
    ///     Exceptions: Any exception thrown by your partitions assigned handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call to Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetPartitionsAssignedHandler(
      Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>> partitionsAssignedHandler)
    {
      this.PartitionsAssignedHandler = this.PartitionsAssignedHandler == null ? partitionsAssignedHandler : throw new InvalidOperationException("The partitions assigned handler may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     This handler is called when a new consumer group partition assignment has been received
    ///     by this consumer.
    /// 
    ///     Note: corresponding to every call to this handler there will be a corresponding call to
    ///     the partitions revoked handler (if one has been set using SetPartitionsRevokedHandler").
    /// 
    ///     Consumption will resume from the last committed offset for each partition, or if there is
    ///     no committed offset, in accordance with the `auto.offset.reset` configuration property.
    /// </summary>
    /// <remarks>
    ///     May execute as a side-effect of the Consumer.Consume call (on the same thread).
    /// 
    ///     Assign/Unassign must not be called in the handler.
    /// 
    ///     Exceptions: Any exception thrown by your partitions assigned handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call to Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetPartitionsAssignedHandler(
      Action<IConsumer<TKey, TValue>, List<TopicPartition>> partitionAssignmentHandler)
    {
      if (this.PartitionsAssignedHandler != null)
        throw new InvalidOperationException("The partitions assigned handler may not be specified more than once.");
      this.PartitionsAssignedHandler = (Func<IConsumer<TKey, TValue>, List<TopicPartition>, IEnumerable<TopicPartitionOffset>>) ((consumer, partitions) =>
      {
        partitionAssignmentHandler(consumer, partitions);
        return (IEnumerable<TopicPartitionOffset>) partitions.Select<TopicPartition, TopicPartitionOffset>((Func<TopicPartition, TopicPartitionOffset>) (tp => new TopicPartitionOffset(tp, Offset.Unset))).ToList<TopicPartitionOffset>();
      });
      return this;
    }

    /// <summary>
    ///     This handler is called immediately prior to a group partition assignment being
    ///     revoked. The second parameter provides the set of partitions the consumer is
    ///     currently assigned to, and the current position of the consumer on each of these
    ///     partitions.
    /// </summary>
    /// <remarks>
    ///     May execute as a side-effect of the Consumer.Consume call (on the same thread).
    /// 
    ///     Assign/Unassign must not be called in the handler.
    /// 
    ///     Exceptions: Any exception thrown by your partitions revoked handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call to Consume/Close.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetPartitionsRevokedHandler(
      Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>> partitionsRevokedHandler)
    {
      this.PartitionsRevokedHandler = this.PartitionsRevokedHandler == null ? partitionsRevokedHandler : throw new InvalidOperationException("The partitions revoked handler may not be specified more than once.");
      return this;
    }

    /// <summary>
    ///     This handler is called immediately prior to a group partition assignment being
    ///     revoked. The second parameter provides the set of partitions the consumer is
    ///     currently assigned to, and the current position of the consumer on each of these
    ///     partitions.
    /// 
    ///     The return value of the handler specifies the partitions/offsets the consumer
    ///     should be assigned to following completion of this method (typically empty).
    /// </summary>
    /// <remarks>
    ///     May execute as a side-effect of the Consumer.Consume call (on the same thread).
    /// 
    ///     Assign/Unassign must not be called in the handler.
    /// 
    ///     Exceptions: Any exception thrown by your partitions revoked handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call to Consume.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetPartitionsRevokedHandler(
      Action<IConsumer<TKey, TValue>, List<TopicPartitionOffset>> partitionsRevokedHandler)
    {
      if (this.PartitionsRevokedHandler != null)
        throw new InvalidOperationException("The partitions revoked handler may not be specified more than once.");
      this.PartitionsRevokedHandler = (Func<IConsumer<TKey, TValue>, List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>>) ((consumer, partitions) =>
      {
        partitionsRevokedHandler(consumer, partitions);
        return (IEnumerable<TopicPartitionOffset>) new List<TopicPartitionOffset>();
      });
      return this;
    }

    /// <summary>
    ///     A handler that is called to report the result of (automatic) offset
    ///     commits. It is not called as a result of the use of the Commit method.
    /// </summary>
    /// <remarks>
    ///     Executes as a side-effect of the Consumer.Consume call (on the same thread).
    /// 
    ///     Exceptions: Any exception thrown by your offsets committed handler
    ///     will be wrapped in a ConsumeException with ErrorCode
    ///     ErrorCode.Local_Application and thrown by the initiating call to Consume/Close.
    /// </remarks>
    public ConsumerBuilder<TKey, TValue> SetOffsetsCommittedHandler(
      Action<IConsumer<TKey, TValue>, CommittedOffsets> offsetsCommittedHandler)
    {
      this.OffsetsCommittedHandler = this.OffsetsCommittedHandler == null ? offsetsCommittedHandler : throw new InvalidOperationException("Offsets committed handler may not be specified more than once.");
      return this;
    }

    /// <summary>Build a new IConsumer implementation instance.</summary>
    public virtual IConsumer<TKey, TValue> Build()
    {
      return (IConsumer<TKey, TValue>) new Consumer<TKey, TValue>(this);
    }
  }
}
