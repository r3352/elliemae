// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Consumer`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Impl;
using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Implements a high-level Apache Kafka consumer with
  ///     deserialization capability.
  /// </summary>
  internal class Consumer<TKey, TValue> : IConsumer<TKey, TValue>, IClient, IDisposable
  {
    private IDeserializer<TKey> keyDeserializer;
    private IDeserializer<TValue> valueDeserializer;
    private Dictionary<Type, object> defaultDeserializers = new Dictionary<Type, object>()
    {
      {
        typeof (Null),
        (object) Deserializers.Null
      },
      {
        typeof (Ignore),
        (object) Deserializers.Ignore
      },
      {
        typeof (int),
        (object) Deserializers.Int32
      },
      {
        typeof (long),
        (object) Deserializers.Int64
      },
      {
        typeof (string),
        (object) Deserializers.Utf8
      },
      {
        typeof (float),
        (object) Deserializers.Single
      },
      {
        typeof (double),
        (object) Deserializers.Double
      },
      {
        typeof (byte[]),
        (object) Deserializers.ByteArray
      }
    };
    private int cancellationDelayMaxMs;
    private bool disposeHasBeenCalled;
    private object disposeHasBeenCalledLockObj = new object();
    /// <summary>
    ///     keeps track of whether or not assign has been called during
    ///     invocation of a rebalance callback event.
    /// </summary>
    private int assignCallCount;
    private object assignCallCountLockObj = new object();
    private bool enableHeaderMarshaling = true;
    private bool enableTimestampMarshaling = true;
    private bool enableTopicNameMarshaling = true;
    private SafeKafkaHandle kafkaHandle;
    private Exception handlerException;
    private Action<Error> errorHandler;
    private Librdkafka.ErrorDelegate errorCallbackDelegate;
    private Action<string> statisticsHandler;
    private Librdkafka.StatsDelegate statisticsCallbackDelegate;
    private Action<LogMessage> logHandler;
    private object loggerLockObj = new object();
    private Librdkafka.LogDelegate logCallbackDelegate;
    private Func<List<TopicPartition>, IEnumerable<TopicPartitionOffset>> partitionsAssignedHandler;
    private Func<List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>> partitionsRevokedHandler;
    private Librdkafka.RebalanceDelegate rebalanceDelegate;
    private Action<CommittedOffsets> offsetsCommittedHandler;
    private Librdkafka.CommitDelegate commitDelegate;

    private void ErrorCallback(IntPtr rk, ErrorCode err, string reason, IntPtr opaque)
    {
      if (this.kafkaHandle.IsClosed)
        return;
      try
      {
        Action<Error> errorHandler = this.errorHandler;
        if (errorHandler == null)
          return;
        errorHandler(this.kafkaHandle.CreatePossiblyFatalError(err, reason));
      }
      catch (Exception ex)
      {
      }
    }

    private int StatisticsCallback(IntPtr rk, IntPtr json, UIntPtr json_len, IntPtr opaque)
    {
      if (this.kafkaHandle.IsClosed)
        return 0;
      try
      {
        Action<string> statisticsHandler = this.statisticsHandler;
        if (statisticsHandler != null)
          statisticsHandler(Util.Marshal.PtrToStringUTF8(json));
      }
      catch (Exception ex)
      {
        this.handlerException = ex;
      }
      return 0;
    }

    private void LogCallback(IntPtr rk, SyslogLevel level, string fac, string buf)
    {
      if (this.kafkaHandle != null && this.kafkaHandle.IsClosed)
        return;
      try
      {
        Action<LogMessage> logHandler = this.logHandler;
        if (logHandler == null)
          return;
        logHandler(new LogMessage(Util.Marshal.PtrToStringUTF8(Librdkafka.name(rk)), level, fac, buf));
      }
      catch (Exception ex)
      {
      }
    }

    private void RebalanceCallback(IntPtr rk, ErrorCode err, IntPtr partitions, IntPtr opaque)
    {
      try
      {
        List<TopicPartition> list = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(partitions).Select<TopicPartitionOffsetError, TopicPartition>((Func<TopicPartitionOffsetError, TopicPartition>) (p => p.TopicPartition)).ToList<TopicPartition>();
        if (this.kafkaHandle.IsClosed)
          throw new Exception("Unexpected rebalance callback on disposed kafkaHandle");
        if (err == ErrorCode.Local_AssignPartitions)
        {
          if (this.partitionsAssignedHandler == null)
          {
            this.Assign(list.Select<TopicPartition, TopicPartitionOffset>((Func<TopicPartition, TopicPartitionOffset>) (p => new TopicPartitionOffset(p, Offset.Unset))));
          }
          else
          {
            lock (this.assignCallCountLockObj)
              this.assignCallCount = 0;
            IEnumerable<TopicPartitionOffset> partitions1 = this.partitionsAssignedHandler(list);
            lock (this.assignCallCountLockObj)
            {
              if (this.assignCallCount > 0)
                throw new InvalidOperationException("Assign/Unassign must not be called in the partitions assigned handler.");
            }
            this.Assign(partitions1);
          }
        }
        else
        {
          if (err != ErrorCode.Local_RevokePartitions)
            throw new KafkaException(this.kafkaHandle.CreatePossiblyFatalError(err, (string) null));
          if (this.partitionsRevokedHandler == null)
          {
            this.Unassign();
          }
          else
          {
            List<TopicPartitionOffset> topicPartitionOffsetList = new List<TopicPartitionOffset>();
            foreach (TopicPartition topicPartition in list)
            {
              try
              {
                topicPartitionOffsetList.Add(new TopicPartitionOffset(topicPartition, this.Position(topicPartition)));
              }
              catch
              {
                topicPartitionOffsetList.Add(new TopicPartitionOffset(topicPartition, Offset.Unset));
              }
            }
            lock (this.assignCallCountLockObj)
              this.assignCallCount = 0;
            IEnumerable<TopicPartitionOffset> topicPartitionOffsets = this.partitionsRevokedHandler(topicPartitionOffsetList);
            lock (this.assignCallCountLockObj)
            {
              if (this.assignCallCount > 0)
                throw new InvalidOperationException("Assign/Unassign must not be called in the partitions revoked handler.");
            }
            if (topicPartitionOffsets.Count<TopicPartitionOffset>() > 0)
              this.Assign(topicPartitionOffsets);
            else
              this.Unassign();
          }
        }
      }
      catch (Exception ex)
      {
        this.handlerException = ex;
      }
    }

    private void CommitCallback(IntPtr rk, ErrorCode err, IntPtr offsets, IntPtr opaque)
    {
      if (this.kafkaHandle.IsClosed)
        return;
      try
      {
        Action<CommittedOffsets> committedHandler = this.offsetsCommittedHandler;
        if (committedHandler == null)
          return;
        committedHandler(new CommittedOffsets((IList<TopicPartitionOffsetError>) SafeKafkaHandle.GetTopicPartitionOffsetErrorList(offsets), this.kafkaHandle.CreatePossiblyFatalError(err, (string) null)));
      }
      catch (Exception ex)
      {
        this.handlerException = ex;
      }
    }

    private static byte[] KeyAsByteArray(rd_kafka_message msg)
    {
      byte[] destination = (byte[]) null;
      if (msg.key != IntPtr.Zero)
      {
        destination = new byte[(int) (uint) msg.key_len];
        System.Runtime.InteropServices.Marshal.Copy(msg.key, destination, 0, (int) (uint) msg.key_len);
      }
      return destination;
    }

    private static byte[] ValueAsByteArray(rd_kafka_message msg)
    {
      byte[] destination = (byte[]) null;
      if (msg.val != IntPtr.Zero)
      {
        destination = new byte[(int) (uint) msg.len];
        System.Runtime.InteropServices.Marshal.Copy(msg.val, destination, 0, (int) (uint) msg.len);
      }
      return destination;
    }

    /// <inheritdoc />
    public List<TopicPartition> Assignment => this.kafkaHandle.GetAssignment();

    /// <inheritdoc />
    public List<string> Subscription => this.kafkaHandle.GetSubscription();

    /// <inheritdoc />
    public void Subscribe(IEnumerable<string> topics) => this.kafkaHandle.Subscribe(topics);

    /// <inheritdoc />
    public void Subscribe(string topic)
    {
      this.Subscribe((IEnumerable<string>) new string[1]
      {
        topic
      });
    }

    /// <inheritdoc />
    public void Unsubscribe() => this.kafkaHandle.Unsubscribe();

    /// <inheritdoc />
    public void Assign(TopicPartition partition)
    {
      this.Assign((IEnumerable<TopicPartition>) new List<TopicPartition>()
      {
        partition
      });
    }

    /// <inheritdoc />
    public void Assign(TopicPartitionOffset partition)
    {
      this.Assign((IEnumerable<TopicPartitionOffset>) new List<TopicPartitionOffset>()
      {
        partition
      });
    }

    /// <inheritdoc />
    public void Assign(IEnumerable<TopicPartitionOffset> partitions)
    {
      lock (this.assignCallCountLockObj)
        ++this.assignCallCount;
      this.kafkaHandle.Assign((IEnumerable<TopicPartitionOffset>) partitions.ToList<TopicPartitionOffset>());
    }

    /// <inheritdoc />
    public void Assign(IEnumerable<TopicPartition> partitions)
    {
      lock (this.assignCallCountLockObj)
        ++this.assignCallCount;
      this.kafkaHandle.Assign((IEnumerable<TopicPartitionOffset>) partitions.Select<TopicPartition, TopicPartitionOffset>((Func<TopicPartition, TopicPartitionOffset>) (p => new TopicPartitionOffset(p, Offset.Unset))).ToList<TopicPartitionOffset>());
    }

    /// <inheritdoc />
    public void Unassign()
    {
      lock (this.assignCallCountLockObj)
        ++this.assignCallCount;
      this.kafkaHandle.Assign((IEnumerable<TopicPartitionOffset>) null);
    }

    /// <inheritdoc />
    public void StoreOffset(ConsumeResult<TKey, TValue> result)
    {
      this.StoreOffset(new TopicPartitionOffset(result.TopicPartition, result.Offset + 1));
    }

    /// <inheritdoc />
    public void StoreOffset(TopicPartitionOffset offset)
    {
      try
      {
        this.kafkaHandle.StoreOffsets((IEnumerable<TopicPartitionOffset>) new TopicPartitionOffset[1]
        {
          offset
        });
      }
      catch (TopicPartitionOffsetException ex)
      {
        throw new KafkaException(ex.Results[0].Error);
      }
    }

    /// <inheritdoc />
    public List<TopicPartitionOffset> Commit()
    {
      return this.kafkaHandle.Commit((IEnumerable<TopicPartitionOffset>) null);
    }

    /// <inheritdoc />
    public void Commit(IEnumerable<TopicPartitionOffset> offsets)
    {
      this.kafkaHandle.Commit(offsets);
    }

    /// <inheritdoc />
    public void Commit(ConsumeResult<TKey, TValue> result)
    {
      if (result.Message == null)
        throw new InvalidOperationException("Attempt was made to commit offset corresponding to an empty consume result");
      this.Commit((IEnumerable<TopicPartitionOffset>) new TopicPartitionOffset[1]
      {
        new TopicPartitionOffset(result.TopicPartition, result.Offset + 1)
      });
    }

    /// <inheritdoc />
    public void Seek(TopicPartitionOffset tpo)
    {
      this.kafkaHandle.Seek(tpo.Topic, tpo.Partition, tpo.Offset, -1);
    }

    /// <inheritdoc />
    public void Pause(IEnumerable<TopicPartition> partitions) => this.kafkaHandle.Pause(partitions);

    /// <inheritdoc />
    public void Resume(IEnumerable<TopicPartition> partitions)
    {
      this.kafkaHandle.Resume(partitions);
    }

    /// <inheritdoc />
    public List<TopicPartitionOffset> Committed(TimeSpan timeout)
    {
      return this.kafkaHandle.Committed((IEnumerable<TopicPartition>) this.Assignment, (IntPtr) timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public List<TopicPartitionOffset> Committed(
      IEnumerable<TopicPartition> partitions,
      TimeSpan timeout)
    {
      return this.kafkaHandle.Committed(partitions, (IntPtr) timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public Offset Position(TopicPartition partition)
    {
      try
      {
        return this.kafkaHandle.Position((IEnumerable<TopicPartition>) new List<TopicPartition>()
        {
          partition
        }).First<TopicPartitionOffset>().Offset;
      }
      catch (TopicPartitionOffsetException ex)
      {
        throw new KafkaException(ex.Results[0].Error);
      }
    }

    /// <inheritdoc />
    public List<TopicPartitionOffset> OffsetsForTimes(
      IEnumerable<TopicPartitionTimestamp> timestampsToSearch,
      TimeSpan timeout)
    {
      return this.kafkaHandle.OffsetsForTimes(timestampsToSearch, timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public WatermarkOffsets GetWatermarkOffsets(TopicPartition topicPartition)
    {
      return this.kafkaHandle.GetWatermarkOffsets(topicPartition.Topic, (int) topicPartition.Partition);
    }

    /// <inheritdoc />
    public WatermarkOffsets QueryWatermarkOffsets(TopicPartition topicPartition, TimeSpan timeout)
    {
      return this.kafkaHandle.QueryWatermarkOffsets(topicPartition.Topic, (int) topicPartition.Partition, timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public string MemberId => this.kafkaHandle.MemberId;

    /// <inheritdoc />
    public int AddBrokers(string brokers) => this.kafkaHandle.AddBrokers(brokers);

    /// <inheritdoc />
    public string Name => this.kafkaHandle.Name;

    /// <inheritdoc />
    public Handle Handle
    {
      get
      {
        return new Handle()
        {
          Owner = (IClient) this,
          LibrdkafkaHandle = this.kafkaHandle
        };
      }
    }

    /// <inheritdoc />
    public void Close()
    {
      this.kafkaHandle.ConsumerClose();
      if (this.handlerException != null)
      {
        Exception handlerException = this.handlerException;
        this.handlerException = (Exception) null;
        throw handlerException;
      }
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///     Releases all resources used by this Consumer without
    ///     committing offsets and without alerting the group coordinator
    ///     that the consumer is exiting the group. If you do not call
    ///     <see cref="M:Confluent.Kafka.Consumer`2.Close" /> or
    ///     <see cref="M:Confluent.Kafka.Consumer`2.Unsubscribe" />
    ///     prior to Dispose, the group will rebalance after a timeout
    ///     specified by group's `session.timeout.ms`.
    ///     You should commit offsets / unsubscribe from the group before
    ///     calling this method (typically by calling
    ///     <see cref="M:Confluent.Kafka.Consumer`2.Close" />).
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the
    ///     <see cref="T:Confluent.Kafka.Consumer`2" />
    ///     and optionally disposes the managed resources.
    /// </summary>
    /// <param name="disposing">
    ///     true to release both managed and unmanaged resources;
    ///     false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      lock (this.disposeHasBeenCalledLockObj)
      {
        if (this.disposeHasBeenCalled)
          return;
        this.disposeHasBeenCalled = true;
      }
      if (!disposing)
        return;
      this.kafkaHandle.Dispose();
    }

    internal Consumer(ConsumerBuilder<TKey, TValue> builder)
    {
      Consumer<TKey, TValue>.Config config = builder.ConstructBaseConfig(this);
      this.statisticsHandler = config.statisticsHandler;
      this.logHandler = config.logHandler;
      this.errorHandler = config.errorHandler;
      this.partitionsAssignedHandler = config.partitionsAssignedHandler;
      this.partitionsRevokedHandler = config.partitionsRevokedHandler;
      this.offsetsCommittedHandler = config.offsetsCommittedHandler;
      Librdkafka.Initialize((string) null);
      IEnumerable<KeyValuePair<string, string>> cancellationDelayMaxMs = Confluent.Kafka.Config.ExtractCancellationDelayMaxMs(config.config, out this.cancellationDelayMaxMs);
      KeyValuePair<string, string> keyValuePair = cancellationDelayMaxMs.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => string.Equals(prop.Key, "group.id", StringComparison.Ordinal)));
      if (keyValuePair.Value == null)
        throw new ArgumentException("'group.id' configuration parameter is required and was not specified.");
      List<KeyValuePair<string, string>> list = Library.NameAndVersionConfig.Concat<KeyValuePair<string, string>>(cancellationDelayMaxMs.Where<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key != "dotnet.consumer.consume.result.fields"))).ToList<KeyValuePair<string, string>>();
      keyValuePair = cancellationDelayMaxMs.FirstOrDefault<KeyValuePair<string, string>>((Func<KeyValuePair<string, string>, bool>) (prop => prop.Key == "dotnet.consumer.consume.result.fields"));
      string str1 = keyValuePair.Value;
      if (str1 != null)
      {
        string str2 = str1.Replace(" ", "");
        if (str2 != "all")
        {
          this.enableHeaderMarshaling = false;
          this.enableTimestampMarshaling = false;
          this.enableTopicNameMarshaling = false;
          if (str2 != "none")
          {
            string str3 = str2;
            char[] chArray = new char[1]{ ',' };
            foreach (string str4 in str3.Split(chArray))
            {
              switch (str4)
              {
                case "headers":
                  this.enableHeaderMarshaling = true;
                  break;
                case "timestamp":
                  this.enableTimestampMarshaling = true;
                  break;
                case "topic":
                  this.enableTopicNameMarshaling = true;
                  break;
                default:
                  throw new ArgumentException("Unexpected consume result field name '" + str4 + "' in config value 'dotnet.consumer.consume.result.fields'.");
              }
            }
          }
        }
      }
      SafeConfigHandle configHandle = SafeConfigHandle.Create();
      list.ForEach((Action<KeyValuePair<string, string>>) (kvp =>
      {
        if (kvp.Value == null)
          throw new ArgumentNullException("'" + kvp.Key + "' configuration parameter must not be null.");
        configHandle.Set(kvp.Key, kvp.Value);
      }));
      this.rebalanceDelegate = new Librdkafka.RebalanceDelegate(this.RebalanceCallback);
      this.commitDelegate = new Librdkafka.CommitDelegate(this.CommitCallback);
      this.errorCallbackDelegate = new Librdkafka.ErrorDelegate(this.ErrorCallback);
      this.logCallbackDelegate = new Librdkafka.LogDelegate(this.LogCallback);
      this.statisticsCallbackDelegate = new Librdkafka.StatsDelegate(this.StatisticsCallback);
      IntPtr handle = configHandle.DangerousGetHandle();
      if (this.partitionsAssignedHandler != null || this.partitionsRevokedHandler != null)
        Librdkafka.conf_set_rebalance_cb(handle, this.rebalanceDelegate);
      if (this.offsetsCommittedHandler != null)
        Librdkafka.conf_set_offset_commit_cb(handle, this.commitDelegate);
      if (this.errorHandler != null)
        Librdkafka.conf_set_error_cb(handle, this.errorCallbackDelegate);
      if (this.logHandler != null)
        Librdkafka.conf_set_log_cb(handle, this.logCallbackDelegate);
      if (this.statisticsHandler != null)
        Librdkafka.conf_set_stats_cb(handle, this.statisticsCallbackDelegate);
      this.kafkaHandle = SafeKafkaHandle.Create(RdKafkaType.Consumer, handle, (IClient) this);
      configHandle.SetHandleAsInvalid();
      ErrorCode code = this.kafkaHandle.PollSetConsumer();
      if (code != ErrorCode.NoError)
        throw new KafkaException(new Error(code, "Failed to redirect the poll queue to consumer_poll queue: " + code.GetReason()));
      if (builder.KeyDeserializer == null)
      {
        object obj;
        if (!this.defaultDeserializers.TryGetValue(typeof (TKey), out obj))
          throw new InvalidOperationException("Key deserializer was not specified and there is no default deserializer defined for type " + typeof (TKey).Name + ".");
        this.keyDeserializer = (IDeserializer<TKey>) obj;
      }
      else
        this.keyDeserializer = builder.KeyDeserializer;
      if (builder.ValueDeserializer == null)
      {
        object obj;
        if (!this.defaultDeserializers.TryGetValue(typeof (TValue), out obj))
          throw new InvalidOperationException("Value deserializer was not specified and there is no default deserializer defined for type " + typeof (TValue).Name + ".");
        this.valueDeserializer = (IDeserializer<TValue>) obj;
      }
      else
        this.valueDeserializer = builder.ValueDeserializer;
    }

    /// <summary>
    ///     Refer to <see cref="M:Confluent.Kafka.IConsumer`2.Consume(System.Int32)" />
    /// </summary>
    public unsafe ConsumeResult<TKey, TValue> Consume(int millisecondsTimeout)
    {
      IntPtr num1 = this.kafkaHandle.ConsumerPoll((IntPtr) millisecondsTimeout);
      if (this.handlerException != null)
      {
        Exception handlerException = this.handlerException;
        this.handlerException = (Exception) null;
        if (num1 != IntPtr.Zero)
          Librdkafka.message_destroy(num1);
        throw handlerException;
      }
      if (num1 == IntPtr.Zero)
        return (ConsumeResult<TKey, TValue>) null;
      try
      {
        rd_kafka_message structure = Util.Marshal.PtrToStructure<rd_kafka_message>(num1);
        string topic = (string) null;
        if (this.enableTopicNameMarshaling && structure.rkt != IntPtr.Zero)
          topic = Util.Marshal.PtrToStringUTF8(Librdkafka.topic_name(structure.rkt));
        if (structure.err == ErrorCode.Local_PartitionEOF)
          return new ConsumeResult<TKey, TValue>()
          {
            TopicPartitionOffset = new TopicPartitionOffset(topic, (Partition) structure.partition, (Offset) structure.offset),
            Message = (Message<TKey, TValue>) null,
            IsPartitionEOF = true
          };
        long unixTimestampMs = 0;
        IntPtr tstype = (IntPtr) 0;
        if (this.enableTimestampMarshaling)
          unixTimestampMs = Librdkafka.message_timestamp(num1, out tstype);
        Timestamp timestamp = new Timestamp(unixTimestampMs, (TimestampType) (int) tstype);
        Headers headers = (Headers) null;
        if (this.enableHeaderMarshaling)
        {
          headers = new Headers();
          IntPtr hdrs;
          int num2 = (int) Librdkafka.message_headers(num1, out hdrs);
          if (hdrs != IntPtr.Zero)
          {
            IntPtr namep;
            IntPtr valuep;
            IntPtr sizep;
            for (int idx = 0; Librdkafka.header_get_all(hdrs, (IntPtr) idx, out namep, out valuep, out sizep) == ErrorCode.NoError; ++idx)
            {
              string stringUtF8 = Util.Marshal.PtrToStringUTF8(namep);
              byte[] numArray = (byte[]) null;
              if (valuep != IntPtr.Zero)
              {
                numArray = new byte[(int) sizep];
                System.Runtime.InteropServices.Marshal.Copy(valuep, numArray, 0, (int) sizep);
              }
              headers.Add(stringUtF8, numArray);
            }
          }
        }
        if (structure.err != ErrorCode.NoError)
        {
          ConsumeResult<byte[], byte[]> consumerRecord = new ConsumeResult<byte[], byte[]>();
          consumerRecord.TopicPartitionOffset = new TopicPartitionOffset(topic, (Partition) structure.partition, (Offset) structure.offset);
          Message<byte[], byte[]> message = new Message<byte[], byte[]>();
          message.Timestamp = timestamp;
          message.Headers = headers;
          message.Key = Consumer<TKey, TValue>.KeyAsByteArray(structure);
          message.Value = Consumer<TKey, TValue>.ValueAsByteArray(structure);
          consumerRecord.Message = message;
          consumerRecord.IsPartitionEOF = false;
          throw new ConsumeException(consumerRecord, this.kafkaHandle.CreatePossiblyFatalError(structure.err, (string) null));
        }
        TKey key;
        try
        {
          key = this.keyDeserializer.Deserialize(structure.key == IntPtr.Zero ? ReadOnlySpan<byte>.Empty : new ReadOnlySpan<byte>(structure.key.ToPointer(), (int) (uint) structure.key_len), structure.key == IntPtr.Zero, new SerializationContext(MessageComponentType.Key, topic, headers));
        }
        catch (Exception ex)
        {
          ConsumeResult<byte[], byte[]> consumerRecord = new ConsumeResult<byte[], byte[]>();
          consumerRecord.TopicPartitionOffset = new TopicPartitionOffset(topic, (Partition) structure.partition, (Offset) structure.offset);
          Message<byte[], byte[]> message = new Message<byte[], byte[]>();
          message.Timestamp = timestamp;
          message.Headers = headers;
          message.Key = Consumer<TKey, TValue>.KeyAsByteArray(structure);
          message.Value = Consumer<TKey, TValue>.ValueAsByteArray(structure);
          consumerRecord.Message = message;
          consumerRecord.IsPartitionEOF = false;
          throw new ConsumeException(consumerRecord, new Error(ErrorCode.Local_KeyDeserialization), ex);
        }
        TValue obj;
        try
        {
          obj = this.valueDeserializer.Deserialize(structure.val == IntPtr.Zero ? ReadOnlySpan<byte>.Empty : new ReadOnlySpan<byte>(structure.val.ToPointer(), (int) (uint) structure.len), structure.val == IntPtr.Zero, new SerializationContext(MessageComponentType.Value, topic, headers));
        }
        catch (Exception ex)
        {
          ConsumeResult<byte[], byte[]> consumerRecord = new ConsumeResult<byte[], byte[]>();
          consumerRecord.TopicPartitionOffset = new TopicPartitionOffset(topic, (Partition) structure.partition, (Offset) structure.offset);
          Message<byte[], byte[]> message = new Message<byte[], byte[]>();
          message.Timestamp = timestamp;
          message.Headers = headers;
          message.Key = Consumer<TKey, TValue>.KeyAsByteArray(structure);
          message.Value = Consumer<TKey, TValue>.ValueAsByteArray(structure);
          consumerRecord.Message = message;
          consumerRecord.IsPartitionEOF = false;
          throw new ConsumeException(consumerRecord, new Error(ErrorCode.Local_ValueDeserialization), ex);
        }
        ConsumeResult<TKey, TValue> consumeResult = new ConsumeResult<TKey, TValue>();
        consumeResult.TopicPartitionOffset = new TopicPartitionOffset(topic, (Partition) structure.partition, (Offset) structure.offset);
        Message<TKey, TValue> message1 = new Message<TKey, TValue>();
        message1.Timestamp = timestamp;
        message1.Headers = headers;
        message1.Key = key;
        message1.Value = obj;
        consumeResult.Message = message1;
        consumeResult.IsPartitionEOF = false;
        return consumeResult;
      }
      finally
      {
        Librdkafka.message_destroy(num1);
      }
    }

    /// <inheritdoc />
    public ConsumeResult<TKey, TValue> Consume(CancellationToken cancellationToken = default (CancellationToken))
    {
      ConsumeResult<TKey, TValue> consumeResult;
      do
      {
        cancellationToken.ThrowIfCancellationRequested();
        consumeResult = this.Consume(this.cancellationDelayMaxMs);
      }
      while (consumeResult == null);
      return consumeResult;
    }

    /// <inheritdoc />
    public ConsumeResult<TKey, TValue> Consume(TimeSpan timeout)
    {
      return this.Consume(timeout.TotalMillisecondsAsInt());
    }

    /// <inheritdoc />
    public IConsumerGroupMetadata ConsumerGroupMetadata
    {
      get
      {
        IntPtr consumerGroupMetadata = this.kafkaHandle.GetConsumerGroupMetadata();
        try
        {
          return (IConsumerGroupMetadata) new Confluent.Kafka.ConsumerGroupMetadata()
          {
            serializedMetadata = this.kafkaHandle.SerializeConsumerGroupMetadata(consumerGroupMetadata)
          };
        }
        finally
        {
          this.kafkaHandle.DestroyConsumerGroupMetadata(consumerGroupMetadata);
        }
      }
    }

    internal class Config
    {
      internal IEnumerable<KeyValuePair<string, string>> config;
      internal Action<Error> errorHandler;
      internal Action<LogMessage> logHandler;
      internal Action<string> statisticsHandler;
      internal Action<CommittedOffsets> offsetsCommittedHandler;
      internal Func<List<TopicPartition>, IEnumerable<TopicPartitionOffset>> partitionsAssignedHandler;
      internal Func<List<TopicPartitionOffset>, IEnumerable<TopicPartitionOffset>> partitionsRevokedHandler;
    }
  }
}
