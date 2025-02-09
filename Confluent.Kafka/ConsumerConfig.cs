// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ConsumerConfig
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Collections.Generic;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Consumer configuration properties</summary>
  public class ConsumerConfig : ClientConfig
  {
    /// <summary>
    ///     Initialize a new empty <see cref="T:Confluent.Kafka.ConsumerConfig" /> instance.
    /// </summary>
    public ConsumerConfig()
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ConsumerConfig" /> instance wrapping
    ///     an existing <see cref="T:Confluent.Kafka.ClientConfig" /> instance.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ConsumerConfig(ClientConfig config)
      : base(config)
    {
    }

    /// <summary>
    ///     Initialize a new <see cref="T:Confluent.Kafka.ConsumerConfig" /> instance wrapping
    ///     an existing key/value pair collection.
    ///     This will change the values "in-place" i.e. operations on this class WILL modify the provided collection
    /// </summary>
    public ConsumerConfig(IDictionary<string, string> config)
      : base(config)
    {
    }

    /// <summary>
    ///     A comma separated list of fields that may be optionally set
    ///     in <see cref="T:Confluent.Kafka.ConsumeResult`2" />
    ///     objects returned by the
    ///     <see cref="M:Confluent.Kafka.Consumer`2.Consume(System.TimeSpan)" />
    ///     method. Disabling fields that you do not require will improve
    ///     throughput and reduce memory consumption. Allowed values:
    ///     headers, timestamp, topic, all, none
    /// 
    ///     default: all
    ///     importance: low
    /// </summary>
    public string ConsumeResultFields
    {
      set => this.SetObject("dotnet.consumer.consume.result.fields", (object) value);
    }

    /// <summary>
    ///     Action to take when there is no initial offset in offset store or the desired offset is out of range: 'smallest','earliest' - automatically reset the offset to the smallest offset, 'largest','latest' - automatically reset the offset to the largest offset, 'error' - trigger an error which is retrieved by consuming messages and checking 'message-&gt;err'.
    /// 
    ///     default: largest
    ///     importance: high
    /// </summary>
    public Confluent.Kafka.AutoOffsetReset? AutoOffsetReset
    {
      get => (Confluent.Kafka.AutoOffsetReset?) this.GetEnum(typeof (Confluent.Kafka.AutoOffsetReset), "auto.offset.reset");
      set => this.SetObject("auto.offset.reset", (object) value);
    }

    /// <summary>
    ///     Client group id string. All clients sharing the same group.id belong to the same group.
    /// 
    ///     default: ''
    ///     importance: high
    /// </summary>
    public string GroupId
    {
      get => this.Get("group.id");
      set => this.SetObject("group.id", (object) value);
    }

    /// <summary>
    ///     Enable static group membership. Static group members are able to leave and rejoin a group within the configured `session.timeout.ms` without prompting a group rebalance. This should be used in combination with a larger `session.timeout.ms` to avoid group rebalances caused by transient unavailability (e.g. process restarts). Requires broker version &gt;= 2.3.0.
    /// 
    ///     default: ''
    ///     importance: medium
    /// </summary>
    public string GroupInstanceId
    {
      get => this.Get("group.instance.id");
      set => this.SetObject("group.instance.id", (object) value);
    }

    /// <summary>
    ///     Name of partition assignment strategy to use when elected group leader assigns partitions to group members.
    /// 
    ///     default: range,roundrobin
    ///     importance: medium
    /// </summary>
    public Confluent.Kafka.PartitionAssignmentStrategy? PartitionAssignmentStrategy
    {
      get
      {
        return (Confluent.Kafka.PartitionAssignmentStrategy?) this.GetEnum(typeof (Confluent.Kafka.PartitionAssignmentStrategy), "partition.assignment.strategy");
      }
      set => this.SetObject("partition.assignment.strategy", (object) value);
    }

    /// <summary>
    ///     Client group session and failure detection timeout. The consumer sends periodic heartbeats (heartbeat.interval.ms) to indicate its liveness to the broker. If no hearts are received by the broker for a group member within the session timeout, the broker will remove the consumer from the group and trigger a rebalance. The allowed range is configured with the **broker** configuration properties `group.min.session.timeout.ms` and `group.max.session.timeout.ms`. Also see `max.poll.interval.ms`.
    /// 
    ///     default: 10000
    ///     importance: high
    /// </summary>
    public int? SessionTimeoutMs
    {
      get => this.GetInt("session.timeout.ms");
      set => this.SetObject("session.timeout.ms", (object) value);
    }

    /// <summary>
    ///     Group session keepalive heartbeat interval.
    /// 
    ///     default: 3000
    ///     importance: low
    /// </summary>
    public int? HeartbeatIntervalMs
    {
      get => this.GetInt("heartbeat.interval.ms");
      set => this.SetObject("heartbeat.interval.ms", (object) value);
    }

    /// <summary>
    ///     Group protocol type
    /// 
    ///     default: consumer
    ///     importance: low
    /// </summary>
    public string GroupProtocolType
    {
      get => this.Get("group.protocol.type");
      set => this.SetObject("group.protocol.type", (object) value);
    }

    /// <summary>
    ///     How often to query for the current client group coordinator. If the currently assigned coordinator is down the configured query interval will be divided by ten to more quickly recover in case of coordinator reassignment.
    /// 
    ///     default: 600000
    ///     importance: low
    /// </summary>
    public int? CoordinatorQueryIntervalMs
    {
      get => this.GetInt("coordinator.query.interval.ms");
      set => this.SetObject("coordinator.query.interval.ms", (object) value);
    }

    /// <summary>
    ///     Maximum allowed time between calls to consume messages (e.g., rd_kafka_consumer_poll()) for high-level consumers. If this interval is exceeded the consumer is considered failed and the group will rebalance in order to reassign the partitions to another consumer group member. Warning: Offset commits may be not possible at this point. Note: It is recommended to set `enable.auto.offset.store=false` for long-time processing applications and then explicitly store offsets (using offsets_store()) *after* message processing, to make sure offsets are not auto-committed prior to processing has finished. The interval is checked two times per second. See KIP-62 for more information.
    /// 
    ///     default: 300000
    ///     importance: high
    /// </summary>
    public int? MaxPollIntervalMs
    {
      get => this.GetInt("max.poll.interval.ms");
      set => this.SetObject("max.poll.interval.ms", (object) value);
    }

    /// <summary>
    ///     Automatically and periodically commit offsets in the background. Note: setting this to false does not prevent the consumer from fetching previously committed start offsets. To circumvent this behaviour set specific start offsets per partition in the call to assign().
    /// 
    ///     default: true
    ///     importance: high
    /// </summary>
    public bool? EnableAutoCommit
    {
      get => this.GetBool("enable.auto.commit");
      set => this.SetObject("enable.auto.commit", (object) value);
    }

    /// <summary>
    ///     The frequency in milliseconds that the consumer offsets are committed (written) to offset storage. (0 = disable). This setting is used by the high-level consumer.
    /// 
    ///     default: 5000
    ///     importance: medium
    /// </summary>
    public int? AutoCommitIntervalMs
    {
      get => this.GetInt("auto.commit.interval.ms");
      set => this.SetObject("auto.commit.interval.ms", (object) value);
    }

    /// <summary>
    ///     Automatically store offset of last message provided to application. The offset store is an in-memory store of the next offset to (auto-)commit for each partition.
    /// 
    ///     default: true
    ///     importance: high
    /// </summary>
    public bool? EnableAutoOffsetStore
    {
      get => this.GetBool("enable.auto.offset.store");
      set => this.SetObject("enable.auto.offset.store", (object) value);
    }

    /// <summary>
    ///     Minimum number of messages per topic+partition librdkafka tries to maintain in the local consumer queue.
    /// 
    ///     default: 100000
    ///     importance: medium
    /// </summary>
    public int? QueuedMinMessages
    {
      get => this.GetInt("queued.min.messages");
      set => this.SetObject("queued.min.messages", (object) value);
    }

    /// <summary>
    ///     Maximum number of kilobytes per topic+partition in the local consumer queue. This value may be overshot by fetch.message.max.bytes. This property has higher priority than queued.min.messages.
    /// 
    ///     default: 1048576
    ///     importance: medium
    /// </summary>
    public int? QueuedMaxMessagesKbytes
    {
      get => this.GetInt("queued.max.messages.kbytes");
      set => this.SetObject("queued.max.messages.kbytes", (object) value);
    }

    /// <summary>
    ///     Maximum time the broker may wait to fill the response with fetch.min.bytes.
    /// 
    ///     default: 100
    ///     importance: low
    /// </summary>
    public int? FetchWaitMaxMs
    {
      get => this.GetInt("fetch.wait.max.ms");
      set => this.SetObject("fetch.wait.max.ms", (object) value);
    }

    /// <summary>
    ///     Initial maximum number of bytes per topic+partition to request when fetching messages from the broker. If the client encounters a message larger than this value it will gradually try to increase it until the entire message can be fetched.
    /// 
    ///     default: 1048576
    ///     importance: medium
    /// </summary>
    public int? MaxPartitionFetchBytes
    {
      get => this.GetInt("max.partition.fetch.bytes");
      set => this.SetObject("max.partition.fetch.bytes", (object) value);
    }

    /// <summary>
    ///     Maximum amount of data the broker shall return for a Fetch request. Messages are fetched in batches by the consumer and if the first message batch in the first non-empty partition of the Fetch request is larger than this value, then the message batch will still be returned to ensure the consumer can make progress. The maximum message batch size accepted by the broker is defined via `message.max.bytes` (broker config) or `max.message.bytes` (broker topic config). `fetch.max.bytes` is automatically adjusted upwards to be at least `message.max.bytes` (consumer config).
    /// 
    ///     default: 52428800
    ///     importance: medium
    /// </summary>
    public int? FetchMaxBytes
    {
      get => this.GetInt("fetch.max.bytes");
      set => this.SetObject("fetch.max.bytes", (object) value);
    }

    /// <summary>
    ///     Minimum number of bytes the broker responds with. If fetch.wait.max.ms expires the accumulated data will be sent to the client regardless of this setting.
    /// 
    ///     default: 1
    ///     importance: low
    /// </summary>
    public int? FetchMinBytes
    {
      get => this.GetInt("fetch.min.bytes");
      set => this.SetObject("fetch.min.bytes", (object) value);
    }

    /// <summary>
    ///     How long to postpone the next fetch request for a topic+partition in case of a fetch error.
    /// 
    ///     default: 500
    ///     importance: medium
    /// </summary>
    public int? FetchErrorBackoffMs
    {
      get => this.GetInt("fetch.error.backoff.ms");
      set => this.SetObject("fetch.error.backoff.ms", (object) value);
    }

    /// <summary>
    ///     Controls how to read messages written transactionally: `read_committed` - only return transactional messages which have been committed. `read_uncommitted` - return all messages, even transactional messages which have been aborted.
    /// 
    ///     default: read_committed
    ///     importance: high
    /// </summary>
    public Confluent.Kafka.IsolationLevel? IsolationLevel
    {
      get => (Confluent.Kafka.IsolationLevel?) this.GetEnum(typeof (Confluent.Kafka.IsolationLevel), "isolation.level");
      set => this.SetObject("isolation.level", (object) value);
    }

    /// <summary>
    ///     Emit RD_KAFKA_RESP_ERR__PARTITION_EOF event whenever the consumer reaches the end of a partition.
    /// 
    ///     default: false
    ///     importance: low
    /// </summary>
    public bool? EnablePartitionEof
    {
      get => this.GetBool("enable.partition.eof");
      set => this.SetObject("enable.partition.eof", (object) value);
    }

    /// <summary>
    ///     Verify CRC32 of consumed messages, ensuring no on-the-wire or on-disk corruption to the messages occurred. This check comes at slightly increased CPU usage.
    /// 
    ///     default: false
    ///     importance: medium
    /// </summary>
    public bool? CheckCrcs
    {
      get => this.GetBool("check.crcs");
      set => this.SetObject("check.crcs", (object) value);
    }
  }
}
