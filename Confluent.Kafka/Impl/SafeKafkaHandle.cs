// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.SafeKafkaHandle
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Admin;
using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal sealed class SafeKafkaHandle : SafeHandleZeroIsInvalid
  {
    private const int RD_KAFKA_PARTITION_UA = -1;
    public volatile IClient owner;
    public RdKafkaType type;
    private object topicHandlesLock = new object();
    private Dictionary<string, SafeTopicHandle> topicHandles = new Dictionary<string, SafeTopicHandle>((IEqualityComparer<string>) StringComparer.Ordinal);
    private string name;

    internal SafeTopicHandle getKafkaTopicHandle(string topic)
    {
      lock (this.topicHandlesLock)
      {
        if (this.topicHandles.ContainsKey(topic))
          return this.topicHandles[topic];
        SafeTopicHandle kafkaTopicHandle = this.NewTopic(topic, IntPtr.Zero);
        this.topicHandles.Add(topic, kafkaTopicHandle);
        return kafkaTopicHandle;
      }
    }

    public SafeKafkaHandle()
      : base("kafka")
    {
    }

    /// <summary>
    ///     This object is tightly coupled to the referencing Producer /
    ///     Consumer via callback objects passed into the librdkafka
    ///     config. These are not tracked by the CLR, so we need to
    ///     maintain an explicit reference to the containing object here
    ///     so the delegates - which may get called by librdkafka during
    ///     destroy - are guaranteed to exist during finalization.
    ///     Note: objects referenced by this handle (recursively) will
    ///     not be GC'd at the time of finalization as the freachable
    ///     list is a GC root. Also, the delegates are ok to use since they
    ///     don't have finalizers.
    /// 
    ///     this is a useful reference:
    ///     https://stackoverflow.com/questions/6964270/which-objects-can-i-use-in-a-finalizer-method
    /// </summary>
    internal void SetOwner(IClient owner) => this.owner = owner;

    public static SafeKafkaHandle Create(RdKafkaType type, IntPtr config, IClient owner)
    {
      StringBuilder errstr = new StringBuilder(512);
      SafeKafkaHandle safeKafkaHandle = Librdkafka.kafka_new(type, config, errstr, (UIntPtr) (ulong) errstr.Capacity);
      if (safeKafkaHandle.IsInvalid)
      {
        Librdkafka.conf_destroy(config);
        throw new InvalidOperationException(errstr.ToString());
      }
      safeKafkaHandle.SetOwner(owner);
      safeKafkaHandle.type = type;
      Library.IncrementKafkaHandleCreateCount();
      return safeKafkaHandle;
    }

    /// <summary>
    ///     Prevent AccessViolationException when handle has already been closed.
    ///     Should be called at start of every function using the handle,
    ///     except in ReleaseHandle.
    /// </summary>
    public void ThrowIfHandleClosed()
    {
      if (this.IsClosed)
        throw new ObjectDisposedException("handle is destroyed", (Exception) null);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        foreach (KeyValuePair<string, SafeTopicHandle> topicHandle in this.topicHandles)
          topicHandle.Value.Dispose();
      }
      base.Dispose(disposing);
    }

    protected override bool ReleaseHandle()
    {
      Library.IncrementKafkaHandleDestroyCount();
      if (this.type == RdKafkaType.Producer)
        Librdkafka.destroy(this.handle);
      else
        Librdkafka.destroy_flags(this.handle, (IntPtr) 8);
      return true;
    }

    internal Confluent.Kafka.Error CreatePossiblyFatalError(ErrorCode err, string reason)
    {
      if (err != ErrorCode.Local_Fatal)
        return new Confluent.Kafka.Error(err, reason);
      StringBuilder sb = new StringBuilder(512);
      err = Librdkafka.fatal_error(this.handle, sb, (UIntPtr) (ulong) sb.Capacity);
      return new Confluent.Kafka.Error(err, sb.ToString(), true);
    }

    internal string Name
    {
      get
      {
        if (this.name == null)
        {
          this.ThrowIfHandleClosed();
          this.name = Util.Marshal.PtrToStringUTF8(Librdkafka.name(this.handle));
        }
        return this.name;
      }
    }

    private int OutQueueLength
    {
      get
      {
        this.ThrowIfHandleClosed();
        return Librdkafka.outq_len(this.handle);
      }
    }

    internal int Flush(int millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      int num = (int) Librdkafka.flush(this.handle, new IntPtr(millisecondsTimeout));
      return this.OutQueueLength;
    }

    internal int AddBrokers(string brokers)
    {
      this.ThrowIfHandleClosed();
      return brokers != null ? (int) Librdkafka.brokers_add(this.handle, brokers) : throw new ArgumentNullException(nameof (brokers), "Argument 'brokers' must not be null.");
    }

    internal int Poll(IntPtr millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      return (int) Librdkafka.poll(this.handle, millisecondsTimeout);
    }

    /// <summary>
    ///     Setting the config parameter to IntPtr.Zero returns the handle of an
    ///     existing topic, or an invalid handle if a topic with name <paramref name="topic" />
    ///     does not exist. Note: Only the first applied configuration for a specific
    ///     topic will be used.
    /// </summary>
    internal SafeTopicHandle NewTopic(string topic, IntPtr config)
    {
      this.ThrowIfHandleClosed();
      bool success = false;
      this.DangerousAddRef(ref success);
      if (!success)
      {
        Librdkafka.topic_conf_destroy(config);
        throw new Exception("Failed to create topic (DangerousAddRef failed)");
      }
      SafeTopicHandle safeTopicHandle = Librdkafka.topic_new(this.handle, topic, config);
      if (safeTopicHandle.IsInvalid)
      {
        this.DangerousRelease();
        throw new KafkaException(this.CreatePossiblyFatalError(Librdkafka.last_error(), (string) null));
      }
      safeTopicHandle.kafkaHandle = this;
      return safeTopicHandle;
    }

    private IntPtr marshalHeaders(IEnumerable<IHeader> headers)
    {
      IntPtr hdrs = IntPtr.Zero;
      if (headers != null && headers.Any<IHeader>())
      {
        hdrs = Librdkafka.headers_new((IntPtr) headers.Count<IHeader>());
        if (hdrs == IntPtr.Zero)
          throw new Exception("Failed to create headers list.");
        foreach (IHeader header in headers)
        {
          if (header.Key == null)
            throw new ArgumentNullException("Message header keys must not be null.");
          byte[] bytes = Encoding.UTF8.GetBytes(header.Key);
          GCHandle gcHandle1 = GCHandle.Alloc((object) bytes, GCHandleType.Pinned);
          IntPtr keydata = gcHandle1.AddrOfPinnedObject();
          IntPtr valdata = IntPtr.Zero;
          GCHandle gcHandle2 = new GCHandle();
          if (header.GetValueBytes() != null)
          {
            gcHandle2 = GCHandle.Alloc((object) header.GetValueBytes(), GCHandleType.Pinned);
            valdata = gcHandle2.AddrOfPinnedObject();
          }
          ErrorCode err = Librdkafka.headers_add(hdrs, keydata, (IntPtr) bytes.Length, valdata, (IntPtr) (header.GetValueBytes() == null ? 0 : header.GetValueBytes().Length));
          gcHandle1.Free();
          if (header.GetValueBytes() != null)
            gcHandle2.Free();
          if (err != ErrorCode.NoError)
            throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
        }
      }
      return hdrs;
    }

    internal ErrorCode Produce(
      string topic,
      byte[] val,
      int valOffset,
      int valLength,
      byte[] key,
      int keyOffset,
      int keyLength,
      int partition,
      long timestamp,
      IEnumerable<IHeader> headers,
      IntPtr opaque)
    {
      IntPtr val1 = IntPtr.Zero;
      IntPtr key1 = IntPtr.Zero;
      GCHandle gcHandle1 = new GCHandle();
      GCHandle gcHandle2 = new GCHandle();
      if (val == null)
      {
        if (valOffset != 0 || valLength != 0)
          throw new ArgumentException("valOffset and valLength parameters must be 0 when producing null values.");
      }
      else
      {
        gcHandle1 = GCHandle.Alloc((object) val, GCHandleType.Pinned);
        val1 = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement((Array) val, valOffset);
      }
      if (key == null)
      {
        if (keyOffset != 0 || keyLength != 0)
          throw new ArgumentException("keyOffset and keyLength parameters must be 0 when producing null key values.");
      }
      else
      {
        gcHandle2 = GCHandle.Alloc((object) key, GCHandleType.Pinned);
        key1 = System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement((Array) key, keyOffset);
      }
      IntPtr num1 = this.marshalHeaders(headers);
      try
      {
        int num2 = (int) Librdkafka.producev(this.handle, topic, partition, (IntPtr) 2, val1, (UIntPtr) (ulong) valLength, key1, (UIntPtr) (ulong) keyLength, timestamp, num1, opaque);
        if (num2 != 0 && num1 != IntPtr.Zero)
          Librdkafka.headers_destroy(num1);
        return (ErrorCode) num2;
      }
      catch
      {
        if (num1 != IntPtr.Zero)
          Librdkafka.headers_destroy(num1);
        throw;
      }
      finally
      {
        if (val != null)
          gcHandle1.Free();
        if (key != null)
          gcHandle2.Free();
      }
    }

    private static int[] MarshalCopy(IntPtr source, int length)
    {
      int[] destination = new int[length];
      System.Runtime.InteropServices.Marshal.Copy(source, destination, 0, length);
      return destination;
    }

    /// <summary>
    ///     - allTopics=true - request all topics from cluster
    ///     - allTopics=false, topic=null - request only locally known topics (topic_new():ed topics or otherwise locally referenced once, such as consumed topics)
    ///     - allTopics=false, topic=valid - request specific topic
    /// </summary>
    internal Confluent.Kafka.Metadata GetMetadata(
      bool allTopics,
      SafeTopicHandle topic,
      int millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      IntPtr metadatap;
      ErrorCode err = Librdkafka.metadata(this.handle, allTopics, topic != null ? topic.DangerousGetHandle() : IntPtr.Zero, out metadatap, (IntPtr) millisecondsTimeout);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      try
      {
        rd_kafka_metadata meta = Util.Marshal.PtrToStructure<rd_kafka_metadata>(metadatap);
        return new Confluent.Kafka.Metadata(Enumerable.Range(0, meta.broker_cnt).Select<int, rd_kafka_metadata_broker>((Func<int, rd_kafka_metadata_broker>) (i => Util.Marshal.PtrToStructure<rd_kafka_metadata_broker>(meta.brokers + i * Util.Marshal.SizeOf<rd_kafka_metadata_broker>()))).Select<rd_kafka_metadata_broker, BrokerMetadata>((Func<rd_kafka_metadata_broker, BrokerMetadata>) (b => new BrokerMetadata(b.id, b.host, b.port))).ToList<BrokerMetadata>(), Enumerable.Range(0, meta.topic_cnt).Select<int, rd_kafka_metadata_topic>((Func<int, rd_kafka_metadata_topic>) (i => Util.Marshal.PtrToStructure<rd_kafka_metadata_topic>(meta.topics + i * Util.Marshal.SizeOf<rd_kafka_metadata_topic>()))).Select<rd_kafka_metadata_topic, TopicMetadata>((Func<rd_kafka_metadata_topic, TopicMetadata>) (t => new TopicMetadata(t.topic, Enumerable.Range(0, t.partition_cnt).Select<int, rd_kafka_metadata_partition>((Func<int, rd_kafka_metadata_partition>) (j => Util.Marshal.PtrToStructure<rd_kafka_metadata_partition>(t.partitions + j * Util.Marshal.SizeOf<rd_kafka_metadata_partition>()))).Select<rd_kafka_metadata_partition, PartitionMetadata>((Func<rd_kafka_metadata_partition, PartitionMetadata>) (p => new PartitionMetadata(p.id, p.leader, SafeKafkaHandle.MarshalCopy(p.replicas, p.replica_cnt), SafeKafkaHandle.MarshalCopy(p.isrs, p.isr_cnt), (Confluent.Kafka.Error) p.err))).ToList<PartitionMetadata>(), (Confluent.Kafka.Error) t.err))).ToList<TopicMetadata>(), meta.orig_broker_id, meta.orig_broker_name);
      }
      finally
      {
        Librdkafka.metadata_destroy(metadatap);
      }
    }

    internal ErrorCode PollSetConsumer()
    {
      this.ThrowIfHandleClosed();
      return Librdkafka.poll_set_consumer(this.handle);
    }

    internal void InitTransactions(int millisecondsTimeout)
    {
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.init_transactions(this.handle, (IntPtr) millisecondsTimeout));
      if (error.Code == ErrorCode.NoError)
        return;
      if (error.IsRetriable)
        throw new KafkaRetriableException(error);
      throw new KafkaException(error);
    }

    internal void BeginTransaction()
    {
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.begin_transaction(this.handle));
      if (error.Code != ErrorCode.NoError)
        throw new KafkaException(error);
    }

    internal void CommitTransaction(int millisecondsTimeout)
    {
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.commit_transaction(this.handle, (IntPtr) millisecondsTimeout));
      if (error.Code == ErrorCode.NoError)
        return;
      if (error.TxnRequiresAbort)
        throw new KafkaTxnRequiresAbortException(error);
      if (error.IsRetriable)
        throw new KafkaRetriableException(error);
      throw new KafkaException(error);
    }

    internal void AbortTransaction(int millisecondsTimeout)
    {
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.abort_transaction(this.handle, (IntPtr) millisecondsTimeout));
      if (error.Code == ErrorCode.NoError)
        return;
      if (error.IsRetriable)
        throw new KafkaRetriableException(error);
      throw new KafkaException(error);
    }

    internal void SendOffsetsToTransaction(
      IEnumerable<TopicPartitionOffset> offsets,
      IConsumerGroupMetadata groupMetadata,
      int millisecondsTimeout)
    {
      IntPtr ctopicPartitionList = SafeKafkaHandle.GetCTopicPartitionList(offsets);
      IntPtr num = groupMetadata is ConsumerGroupMetadata ? this.DeserializeConsumerGroupMetadata(((ConsumerGroupMetadata) groupMetadata).serializedMetadata) : throw new ArgumentException("groupMetadata object must be a value acquired via Consumer.ConsumerGroupMetadata.");
      try
      {
        Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.send_offsets_to_transaction(this.handle, ctopicPartitionList, num, (IntPtr) millisecondsTimeout));
        if (error.Code == ErrorCode.NoError)
          return;
        if (error.IsRetriable)
          throw new KafkaRetriableException(error);
        if (error.TxnRequiresAbort)
          throw new KafkaTxnRequiresAbortException(error);
        throw new KafkaException(error);
      }
      finally
      {
        this.DestroyConsumerGroupMetadata(num);
      }
    }

    internal IntPtr GetConsumerGroupMetadata() => Librdkafka.consumer_group_metadata(this.handle);

    internal void DestroyConsumerGroupMetadata(IntPtr consumerGroupMetadata)
    {
      Librdkafka.consumer_group_metadata_destroy(consumerGroupMetadata);
    }

    internal unsafe byte[] SerializeConsumerGroupMetadata(IntPtr consumerGroupMetadata)
    {
      IntPtr data;
      IntPtr dataSize;
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.consumer_group_metadata_write(consumerGroupMetadata, out data, out dataSize));
      if (error.Code != ErrorCode.NoError)
        throw new KafkaException(error);
      byte[] numArray = new byte[(int) dataSize];
      byte* numPtr = (byte*) (void*) data;
      for (int index = 0; index < (int) dataSize; ++index)
        numArray[index] = *numPtr++;
      Librdkafka.mem_free(IntPtr.Zero, data);
      return numArray;
    }

    internal IntPtr DeserializeConsumerGroupMetadata(byte[] buffer)
    {
      IntPtr cgmd;
      Confluent.Kafka.Error error = new Confluent.Kafka.Error(Librdkafka.consumer_group_metadata_read(out cgmd, buffer, (IntPtr) buffer.Length));
      if (error.Code != ErrorCode.NoError)
        throw new KafkaException(error);
      return cgmd;
    }

    internal WatermarkOffsets QueryWatermarkOffsets(
      string topic,
      int partition,
      int millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      long low;
      long high;
      ErrorCode err = Librdkafka.query_watermark_offsets(this.handle, topic, partition, out low, out high, (IntPtr) millisecondsTimeout);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      return new WatermarkOffsets((Confluent.Kafka.Offset) low, (Confluent.Kafka.Offset) high);
    }

    internal WatermarkOffsets GetWatermarkOffsets(string topic, int partition)
    {
      this.ThrowIfHandleClosed();
      long low;
      long high;
      ErrorCode watermarkOffsets = Librdkafka.get_watermark_offsets(this.handle, topic, partition, out low, out high);
      if (watermarkOffsets != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(watermarkOffsets, (string) null));
      return new WatermarkOffsets((Confluent.Kafka.Offset) low, (Confluent.Kafka.Offset) high);
    }

    internal List<TopicPartitionOffset> OffsetsForTimes(
      IEnumerable<TopicPartitionTimestamp> timestampsToSearch,
      int millisecondsTimeout)
    {
      if (timestampsToSearch.Count<TopicPartitionTimestamp>() == 0)
        return new List<TopicPartitionOffset>();
      IntPtr ctopicPartitionList = SafeKafkaHandle.GetCTopicPartitionList((IEnumerable<TopicPartitionOffset>) timestampsToSearch.Select<TopicPartitionTimestamp, TopicPartitionOffset>((Func<TopicPartitionTimestamp, TopicPartitionOffset>) (t => new TopicPartitionOffset(t.TopicPartition, (Confluent.Kafka.Offset) t.Timestamp.UnixTimestampMs))).ToList<TopicPartitionOffset>());
      try
      {
        ErrorCode err = Librdkafka.offsets_for_times(this.handle, ctopicPartitionList, (IntPtr) millisecondsTimeout);
        if (err != ErrorCode.NoError)
          throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
        List<TopicPartitionOffsetError> partitionOffsetErrorList = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(ctopicPartitionList);
        if (partitionOffsetErrorList.Where<TopicPartitionOffsetError>((Func<TopicPartitionOffsetError, bool>) (tpoe => tpoe.Error.Code != 0)).Count<TopicPartitionOffsetError>() > 0)
          throw new TopicPartitionOffsetException(partitionOffsetErrorList);
        return partitionOffsetErrorList.Select<TopicPartitionOffsetError, TopicPartitionOffset>((Func<TopicPartitionOffsetError, TopicPartitionOffset>) (r => r.TopicPartitionOffset)).ToList<TopicPartitionOffset>();
      }
      finally
      {
        Librdkafka.topic_partition_list_destroy(ctopicPartitionList);
      }
    }

    internal void Subscribe(IEnumerable<string> topics)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = Librdkafka.topic_partition_list_new((IntPtr) topics.Count<string>());
      if (num == IntPtr.Zero)
        throw new Exception("Failed to create topic partition list");
      foreach (string topic in topics)
        Librdkafka.topic_partition_list_add(num, topic, -1);
      ErrorCode err = Librdkafka.subscribe(this.handle, num);
      Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
    }

    internal void Unsubscribe()
    {
      this.ThrowIfHandleClosed();
      ErrorCode err = Librdkafka.unsubscribe(this.handle);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
    }

    internal IntPtr ConsumerPoll(IntPtr millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      return Librdkafka.consumer_poll(this.handle, millisecondsTimeout);
    }

    internal void ConsumerClose()
    {
      this.ThrowIfHandleClosed();
      ErrorCode err = Librdkafka.consumer_close(this.handle);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
    }

    internal List<TopicPartition> GetAssignment()
    {
      this.ThrowIfHandleClosed();
      IntPtr topics = IntPtr.Zero;
      ErrorCode err = Librdkafka.assignment(this.handle, out topics);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      List<TopicPartition> list = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(topics).Select<TopicPartitionOffsetError, TopicPartition>((Func<TopicPartitionOffsetError, TopicPartition>) (a => a.TopicPartition)).ToList<TopicPartition>();
      Librdkafka.topic_partition_list_destroy(topics);
      return list;
    }

    internal List<string> GetSubscription()
    {
      this.ThrowIfHandleClosed();
      IntPtr topics = IntPtr.Zero;
      ErrorCode err = Librdkafka.subscription(this.handle, out topics);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      List<string> list = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(topics).Select<TopicPartitionOffsetError, string>((Func<TopicPartitionOffsetError, string>) (a => a.Topic)).ToList<string>();
      Librdkafka.topic_partition_list_destroy(topics);
      return list;
    }

    internal void Assign(IEnumerable<TopicPartitionOffset> partitions)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = IntPtr.Zero;
      if (partitions != null)
      {
        num = Librdkafka.topic_partition_list_new((IntPtr) partitions.Count<TopicPartitionOffset>());
        if (num == IntPtr.Zero)
          throw new Exception("Failed to create topic partition list");
        foreach (TopicPartitionOffset partition in partitions)
        {
          if (partition.Topic == null)
          {
            Librdkafka.topic_partition_list_destroy(num);
            throw new ArgumentException("Cannot assign partitions because one or more have a null topic.");
          }
          System.Runtime.InteropServices.Marshal.WriteInt64(Librdkafka.topic_partition_list_add(num, partition.Topic, (int) partition.Partition), (int) Util.Marshal.OffsetOf<rd_kafka_topic_partition>("offset"), (long) partition.Offset);
        }
      }
      ErrorCode err = Librdkafka.assign(this.handle, num);
      if (num != IntPtr.Zero)
        Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
    }

    internal void StoreOffsets(IEnumerable<TopicPartitionOffset> offsets)
    {
      this.ThrowIfHandleClosed();
      IntPtr ctopicPartitionList = SafeKafkaHandle.GetCTopicPartitionList(offsets);
      ErrorCode err = Librdkafka.offsets_store(this.handle, ctopicPartitionList);
      List<TopicPartitionOffsetError> partitionOffsetErrorList = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(ctopicPartitionList);
      Librdkafka.topic_partition_list_destroy(ctopicPartitionList);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      if (partitionOffsetErrorList.Where<TopicPartitionOffsetError>((Func<TopicPartitionOffsetError, bool>) (tpoe => tpoe.Error.Code != 0)).Count<TopicPartitionOffsetError>() > 0)
        throw new TopicPartitionOffsetException(partitionOffsetErrorList);
    }

    /// <summary>
    ///     Dummy commit callback that does nothing but prohibits
    ///     triggering the global offset_commit_cb.
    ///     Used by manual commits.
    /// </summary>
    private static void DummyOffsetCommitCb(
      IntPtr rk,
      ErrorCode err,
      IntPtr offsets,
      IntPtr opaque)
    {
    }

    internal List<TopicPartitionOffset> Commit(IEnumerable<TopicPartitionOffset> offsets)
    {
      this.ThrowIfHandleClosed();
      if (offsets != null && offsets.Count<TopicPartitionOffset>() == 0)
        return new List<TopicPartitionOffset>();
      IntPtr rkqu = Librdkafka.queue_new(this.handle);
      IntPtr ctopicPartitionList = SafeKafkaHandle.GetCTopicPartitionList(offsets);
      ErrorCode err1 = Librdkafka.commit_queue(this.handle, ctopicPartitionList, rkqu, new Librdkafka.CommitDelegate(SafeKafkaHandle.DummyOffsetCommitCb), IntPtr.Zero);
      if (ctopicPartitionList != IntPtr.Zero)
        Librdkafka.topic_partition_list_destroy(ctopicPartitionList);
      if (err1 != ErrorCode.NoError)
      {
        Librdkafka.queue_destroy(rkqu);
        throw new KafkaException(this.CreatePossiblyFatalError(err1, (string) null));
      }
      IntPtr rkev = Librdkafka.queue_poll(rkqu, -1);
      Librdkafka.queue_destroy(rkqu);
      ErrorCode err2 = !(rkev == IntPtr.Zero) ? Librdkafka.event_error(rkev) : throw new KafkaException(ErrorCode.Local_TimedOut);
      if (err2 != ErrorCode.NoError)
      {
        string reason = Librdkafka.event_error_string(rkev);
        Librdkafka.event_destroy(rkev);
        throw new KafkaException(this.CreatePossiblyFatalError(err2, reason));
      }
      List<TopicPartitionOffsetError> partitionOffsetErrorList = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(Librdkafka.event_topic_partition_list(rkev));
      Librdkafka.event_destroy(rkev);
      if (partitionOffsetErrorList.Where<TopicPartitionOffsetError>((Func<TopicPartitionOffsetError, bool>) (tpoe => tpoe.Error.Code != 0)).Count<TopicPartitionOffsetError>() > 0)
        throw new TopicPartitionOffsetException(partitionOffsetErrorList);
      return partitionOffsetErrorList.Select<TopicPartitionOffsetError, TopicPartitionOffset>((Func<TopicPartitionOffsetError, TopicPartitionOffset>) (r => r.TopicPartitionOffset)).ToList<TopicPartitionOffset>();
    }

    internal void Seek(string topic, Partition partition, Confluent.Kafka.Offset offset, int millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      SafeTopicHandle kafkaTopicHandle = this.getKafkaTopicHandle(topic);
      bool success = false;
      kafkaTopicHandle.DangerousAddRef(ref success);
      if (!success)
        throw new Exception("Seek failed (DangerousAddRef failed)");
      ErrorCode err;
      try
      {
        err = Librdkafka.seek(kafkaTopicHandle.DangerousGetHandle(), (int) partition, (long) offset, (IntPtr) millisecondsTimeout);
      }
      finally
      {
        kafkaTopicHandle.DangerousRelease();
      }
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
    }

    internal List<TopicPartitionError> Pause(IEnumerable<TopicPartition> partitions)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = Librdkafka.topic_partition_list_new((IntPtr) partitions.Count<TopicPartition>());
      if (num == IntPtr.Zero)
        throw new Exception("Failed to create pause partition list");
      foreach (TopicPartition partition in partitions)
        Librdkafka.topic_partition_list_add(num, partition.Topic, (int) partition.Partition);
      ErrorCode err = Librdkafka.pause_partitions(this.handle, num);
      List<TopicPartitionError> partitionErrorList = SafeKafkaHandle.GetTopicPartitionErrorList(num);
      Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      if (partitionErrorList.Where<TopicPartitionError>((Func<TopicPartitionError, bool>) (tpe => tpe.Error.Code != 0)).Count<TopicPartitionError>() > 0)
        throw new TopicPartitionException(partitionErrorList);
      return partitionErrorList;
    }

    internal List<TopicPartitionError> Resume(IEnumerable<TopicPartition> partitions)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = Librdkafka.topic_partition_list_new((IntPtr) partitions.Count<TopicPartition>());
      if (num == IntPtr.Zero)
        throw new Exception("Failed to create resume partition list");
      foreach (TopicPartition partition in partitions)
        Librdkafka.topic_partition_list_add(num, partition.Topic, (int) partition.Partition);
      ErrorCode err = Librdkafka.resume_partitions(this.handle, num);
      List<TopicPartitionError> partitionErrorList = SafeKafkaHandle.GetTopicPartitionErrorList(num);
      Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      if (partitionErrorList.Where<TopicPartitionError>((Func<TopicPartitionError, bool>) (tpe => tpe.Error.Code != 0)).Count<TopicPartitionError>() > 0)
        throw new TopicPartitionException(partitionErrorList);
      return partitionErrorList;
    }

    internal List<TopicPartitionOffset> Committed(
      IEnumerable<TopicPartition> partitions,
      IntPtr timeout_ms)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = Librdkafka.topic_partition_list_new((IntPtr) partitions.Count<TopicPartition>());
      if (num == IntPtr.Zero)
        throw new Exception("Failed to create committed partition list");
      foreach (TopicPartition partition in partitions)
        Librdkafka.topic_partition_list_add(num, partition.Topic, (int) partition.Partition);
      ErrorCode err = Librdkafka.committed(this.handle, num, timeout_ms);
      List<TopicPartitionOffsetError> partitionOffsetErrorList = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(num);
      Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      if (partitionOffsetErrorList.Where<TopicPartitionOffsetError>((Func<TopicPartitionOffsetError, bool>) (tpoe => tpoe.Error.Code != 0)).Count<TopicPartitionOffsetError>() > 0)
        throw new TopicPartitionOffsetException(partitionOffsetErrorList);
      return partitionOffsetErrorList.Select<TopicPartitionOffsetError, TopicPartitionOffset>((Func<TopicPartitionOffsetError, TopicPartitionOffset>) (r => r.TopicPartitionOffset)).ToList<TopicPartitionOffset>();
    }

    internal List<TopicPartitionOffset> Position(IEnumerable<TopicPartition> partitions)
    {
      this.ThrowIfHandleClosed();
      IntPtr num = Librdkafka.topic_partition_list_new((IntPtr) partitions.Count<TopicPartition>());
      if (num == IntPtr.Zero)
        throw new Exception("Failed to create position list");
      foreach (TopicPartition partition in partitions)
        Librdkafka.topic_partition_list_add(num, partition.Topic, (int) partition.Partition);
      ErrorCode err = Librdkafka.position(this.handle, num);
      List<TopicPartitionOffsetError> partitionOffsetErrorList = SafeKafkaHandle.GetTopicPartitionOffsetErrorList(num);
      Librdkafka.topic_partition_list_destroy(num);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      if (partitionOffsetErrorList.Where<TopicPartitionOffsetError>((Func<TopicPartitionOffsetError, bool>) (tpoe => tpoe.Error.Code != 0)).Count<TopicPartitionOffsetError>() > 0)
        throw new TopicPartitionOffsetException(partitionOffsetErrorList);
      return partitionOffsetErrorList.Select<TopicPartitionOffsetError, TopicPartitionOffset>((Func<TopicPartitionOffsetError, TopicPartitionOffset>) (r => r.TopicPartitionOffset)).ToList<TopicPartitionOffset>();
    }

    internal string MemberId
    {
      get
      {
        this.ThrowIfHandleClosed();
        IntPtr num = Librdkafka.memberid(this.handle);
        if (num == IntPtr.Zero)
          return (string) null;
        string stringUtF8 = Util.Marshal.PtrToStringUTF8(num);
        Librdkafka.mem_free(this.handle, num);
        return stringUtF8;
      }
    }

    internal static List<TopicPartitionError> GetTopicPartitionErrorList(IntPtr listPtr)
    {
      rd_kafka_topic_partition_list list = !(listPtr == IntPtr.Zero) ? Util.Marshal.PtrToStructure<rd_kafka_topic_partition_list>(listPtr) : throw new InvalidOperationException("FATAL: Cannot marshal from a NULL ptr.");
      return Enumerable.Range(0, list.cnt).Select<int, rd_kafka_topic_partition>((Func<int, rd_kafka_topic_partition>) (i => Util.Marshal.PtrToStructure<rd_kafka_topic_partition>(list.elems + i * Util.Marshal.SizeOf<rd_kafka_topic_partition>()))).Select<rd_kafka_topic_partition, TopicPartitionError>((Func<rd_kafka_topic_partition, TopicPartitionError>) (ktp => new TopicPartitionError(ktp.topic, (Partition) ktp.partition, (Confluent.Kafka.Error) ktp.err))).ToList<TopicPartitionError>();
    }

    internal static List<TopicPartitionOffsetError> GetTopicPartitionOffsetErrorList(IntPtr listPtr)
    {
      rd_kafka_topic_partition_list list = !(listPtr == IntPtr.Zero) ? Util.Marshal.PtrToStructure<rd_kafka_topic_partition_list>(listPtr) : throw new InvalidOperationException("FATAL: Cannot marshal from a NULL ptr.");
      return Enumerable.Range(0, list.cnt).Select<int, rd_kafka_topic_partition>((Func<int, rd_kafka_topic_partition>) (i => Util.Marshal.PtrToStructure<rd_kafka_topic_partition>(list.elems + i * Util.Marshal.SizeOf<rd_kafka_topic_partition>()))).Select<rd_kafka_topic_partition, TopicPartitionOffsetError>((Func<rd_kafka_topic_partition, TopicPartitionOffsetError>) (ktp => new TopicPartitionOffsetError(ktp.topic, (Partition) ktp.partition, (Confluent.Kafka.Offset) ktp.offset, (Confluent.Kafka.Error) ktp.err))).ToList<TopicPartitionOffsetError>();
    }

    /// <summary>
    ///     Creates and returns a C rd_kafka_topic_partition_list_t * populated by offsets.
    /// </summary>
    /// <returns>
    ///     If offsets is null a null IntPtr will be returned, else a IntPtr
    ///     which must destroyed with LibRdKafka.topic_partition_list_destroy()
    /// </returns>
    internal static IntPtr GetCTopicPartitionList(IEnumerable<TopicPartitionOffset> offsets)
    {
      if (offsets == null)
        return IntPtr.Zero;
      IntPtr ctopicPartitionList = Librdkafka.topic_partition_list_new((IntPtr) offsets.Count<TopicPartitionOffset>());
      if (ctopicPartitionList == IntPtr.Zero)
        throw new Exception("Failed to create topic partition list");
      foreach (TopicPartitionOffset offset in offsets)
      {
        if (offset.Topic == null)
        {
          Librdkafka.topic_partition_list_destroy(ctopicPartitionList);
          throw new ArgumentException("Cannot create offsets list because one or more topics is null.");
        }
        System.Runtime.InteropServices.Marshal.WriteInt64(Librdkafka.topic_partition_list_add(ctopicPartitionList, offset.Topic, (int) offset.Partition), (int) Util.Marshal.OffsetOf<rd_kafka_topic_partition>("offset"), (long) offset.Offset);
      }
      return ctopicPartitionList;
    }

    private static byte[] CopyBytes(IntPtr ptr, IntPtr len)
    {
      byte[] destination = (byte[]) null;
      if (ptr != IntPtr.Zero)
      {
        destination = new byte[(int) len];
        System.Runtime.InteropServices.Marshal.Copy(ptr, destination, 0, (int) len);
      }
      return destination;
    }

    internal GroupInfo ListGroup(string group, int millisecondsTimeout)
    {
      return group != null ? this.ListGroupsImpl(group, millisecondsTimeout).FirstOrDefault<GroupInfo>() : throw new ArgumentNullException(nameof (group), "Argument 'group' must not be null.");
    }

    internal List<GroupInfo> ListGroups(int millisecondsTimeout)
    {
      return this.ListGroupsImpl((string) null, millisecondsTimeout);
    }

    private List<GroupInfo> ListGroupsImpl(string group, int millisecondsTimeout)
    {
      this.ThrowIfHandleClosed();
      IntPtr grplistp;
      ErrorCode err = Librdkafka.list_groups(this.handle, group, out grplistp, (IntPtr) millisecondsTimeout);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
      rd_kafka_group_list list = Util.Marshal.PtrToStructure<rd_kafka_group_list>(grplistp);
      List<GroupInfo> list1 = Enumerable.Range(0, list.group_cnt).Select<int, rd_kafka_group_info>((Func<int, rd_kafka_group_info>) (i => Util.Marshal.PtrToStructure<rd_kafka_group_info>(list.groups + i * Util.Marshal.SizeOf<rd_kafka_group_info>()))).Select<rd_kafka_group_info, GroupInfo>((Func<rd_kafka_group_info, GroupInfo>) (gi => new GroupInfo(new BrokerMetadata(gi.broker.id, gi.broker.host, gi.broker.port), gi.group, (Confluent.Kafka.Error) gi.err, gi.state, gi.protocol_type, gi.protocol, Enumerable.Range(0, gi.member_cnt).Select<int, rd_kafka_group_member_info>((Func<int, rd_kafka_group_member_info>) (j => Util.Marshal.PtrToStructure<rd_kafka_group_member_info>(gi.members + j * Util.Marshal.SizeOf<rd_kafka_group_member_info>()))).Select<rd_kafka_group_member_info, GroupMemberInfo>((Func<rd_kafka_group_member_info, GroupMemberInfo>) (mi => new GroupMemberInfo(mi.member_id, mi.client_id, mi.client_host, SafeKafkaHandle.CopyBytes(mi.member_metadata, mi.member_metadata_size), SafeKafkaHandle.CopyBytes(mi.member_assignment, mi.member_assignment_size)))).ToList<GroupMemberInfo>()))).ToList<GroupInfo>();
      Librdkafka.group_list_destroy(grplistp);
      return list1;
    }

    internal IntPtr CreateQueue() => Librdkafka.queue_new(this.handle);

    internal void DestroyQueue(IntPtr queue) => Librdkafka.queue_destroy(queue);

    internal IntPtr QueuePoll(IntPtr queue, int millisecondsTimeout)
    {
      return Librdkafka.queue_poll(queue, millisecondsTimeout);
    }

    private void setOption_ValidatOnly(IntPtr optionsPtr, bool validateOnly)
    {
      StringBuilder errstr = new StringBuilder(512);
      ErrorCode err = Librdkafka.AdminOptions_set_validate_only(optionsPtr, (IntPtr) (validateOnly ? 1 : 0), errstr, (UIntPtr) (ulong) errstr.Capacity);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, errstr.ToString()));
    }

    private void setOption_RequestTimeout(IntPtr optionsPtr, TimeSpan? timeout)
    {
      if (!timeout.HasValue)
        return;
      StringBuilder errstr = new StringBuilder(512);
      ErrorCode err = Librdkafka.AdminOptions_set_request_timeout(optionsPtr, (IntPtr) (int) timeout.Value.TotalMilliseconds, errstr, (UIntPtr) (ulong) errstr.Capacity);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, errstr.ToString()));
    }

    private void setOption_OperationTimeout(IntPtr optionsPtr, TimeSpan? timeout)
    {
      if (!timeout.HasValue)
        return;
      StringBuilder errstr = new StringBuilder(512);
      ErrorCode err = Librdkafka.AdminOptions_set_operation_timeout(optionsPtr, (IntPtr) (int) timeout.Value.TotalMilliseconds, errstr, (UIntPtr) (ulong) errstr.Capacity);
      if (err != ErrorCode.NoError)
        throw new KafkaException(this.CreatePossiblyFatalError(err, errstr.ToString()));
    }

    private void setOption_completionSource(IntPtr optionsPtr, IntPtr completionSourcePtr)
    {
      Librdkafka.AdminOptions_set_opaque(optionsPtr, completionSourcePtr);
    }

    internal void AlterConfigs(
      IDictionary<ConfigResource, List<ConfigEntry>> configs,
      AlterConfigsOptions options,
      IntPtr resultQueuePtr,
      IntPtr completionSourcePtr)
    {
      this.ThrowIfHandleClosed();
      options = options == null ? new AlterConfigsOptions() : options;
      IntPtr num1 = Librdkafka.AdminOptions_new(this.handle, Librdkafka.AdminOp.AlterConfigs);
      this.setOption_ValidatOnly(num1, options.ValidateOnly);
      this.setOption_RequestTimeout(num1, options.RequestTimeout);
      this.setOption_completionSource(num1, completionSourcePtr);
      IntPtr[] configs1 = new IntPtr[configs.Count<KeyValuePair<ConfigResource, List<ConfigEntry>>>()];
      int num2 = 0;
      foreach (KeyValuePair<ConfigResource, List<ConfigEntry>> config1 in (IEnumerable<KeyValuePair<ConfigResource, List<ConfigEntry>>>) configs)
      {
        ConfigResource key = config1.Key;
        List<ConfigEntry> configEntryList = config1.Value;
        if (string.IsNullOrEmpty(key.Name))
          throw new ArgumentException("Resource must be specified.");
        IntPtr config2 = Librdkafka.ConfigResource_new(key.Type, key.Name);
        foreach (ConfigEntry configEntry in configEntryList)
        {
          if (string.IsNullOrEmpty(configEntry.Name))
            throw new ArgumentException(string.Format("config name must be specified for {0}", (object) key));
          ErrorCode err = Librdkafka.ConfigResource_set_config(config2, configEntry.Name, configEntry.Value);
          if (err != ErrorCode.NoError)
            throw new KafkaException(this.CreatePossiblyFatalError(err, (string) null));
        }
        configs1[num2++] = config2;
      }
      Librdkafka.AlterConfigs(this.handle, configs1, (UIntPtr) (ulong) configs1.Length, num1, resultQueuePtr);
      for (int index = 0; index < configs1.Length; ++index)
        Librdkafka.ConfigResource_destroy(configs1[index]);
      Librdkafka.AdminOptions_destroy(num1);
    }

    internal void DescribeConfigs(
      IEnumerable<ConfigResource> resources,
      DescribeConfigsOptions options,
      IntPtr resultQueuePtr,
      IntPtr completionSourcePtr)
    {
      this.ThrowIfHandleClosed();
      options = options == null ? new DescribeConfigsOptions() : options;
      IntPtr num1 = Librdkafka.AdminOptions_new(this.handle, Librdkafka.AdminOp.DescribeConfigs);
      this.setOption_RequestTimeout(num1, options.RequestTimeout);
      this.setOption_completionSource(num1, completionSourcePtr);
      IntPtr[] configs = new IntPtr[resources.Count<ConfigResource>()];
      int num2 = 0;
      foreach (ConfigResource resource in resources)
      {
        if (string.IsNullOrEmpty(resource.Name))
          throw new ArgumentException("Resource must be specified.");
        IntPtr num3 = Librdkafka.ConfigResource_new(resource.Type, resource.Name);
        configs[num2++] = num3;
      }
      Librdkafka.DescribeConfigs(this.handle, configs, (UIntPtr) (ulong) configs.Length, num1, resultQueuePtr);
      for (int index = 0; index < configs.Length; ++index)
        Librdkafka.ConfigResource_destroy(configs[index]);
      Librdkafka.AdminOptions_destroy(num1);
    }

    internal void CreatePartitions(
      IEnumerable<PartitionsSpecification> newPartitions,
      CreatePartitionsOptions options,
      IntPtr resultQueuePtr,
      IntPtr completionSourcePtr)
    {
      this.ThrowIfHandleClosed();
      StringBuilder errstr = new StringBuilder(512);
      options = options == null ? new CreatePartitionsOptions() : options;
      IntPtr num1 = Librdkafka.AdminOptions_new(this.handle, Librdkafka.AdminOp.CreatePartitions);
      this.setOption_ValidatOnly(num1, options.ValidateOnly);
      this.setOption_RequestTimeout(num1, options.RequestTimeout);
      this.setOption_OperationTimeout(num1, options.OperationTimeout);
      this.setOption_completionSource(num1, completionSourcePtr);
      IntPtr[] new_parts1 = new IntPtr[newPartitions.Count<PartitionsSpecification>()];
      try
      {
        int index = 0;
        foreach (PartitionsSpecification newPartition in newPartitions)
        {
          string topic = newPartition.Topic;
          int increaseTo = newPartition.IncreaseTo;
          List<List<int>> replicaAssignments = newPartition.ReplicaAssignments;
          if (newPartition.Topic == null)
            throw new ArgumentException("Cannot add partitions to a null topic.");
          IntPtr new_parts2 = Librdkafka.NewPartitions_new(topic, (UIntPtr) (ulong) increaseTo, errstr, (UIntPtr) (ulong) errstr.Capacity);
          if (new_parts2 == IntPtr.Zero)
            throw new KafkaException(new Confluent.Kafka.Error(ErrorCode.Unknown, errstr.ToString()));
          if (replicaAssignments != null)
          {
            int num2 = 0;
            foreach (List<int> intList in replicaAssignments)
            {
              errstr = new StringBuilder(512);
              int[] array = replicaAssignments[num2].ToArray();
              ErrorCode err = Librdkafka.NewPartitions_set_replica_assignment(new_parts2, num2, array, (UIntPtr) (ulong) array.Length, errstr, (UIntPtr) (ulong) errstr.Capacity);
              if (err != ErrorCode.NoError)
                throw new KafkaException(this.CreatePossiblyFatalError(err, errstr.ToString()));
              ++num2;
            }
          }
          new_parts1[index] = new_parts2;
          ++index;
        }
        Librdkafka.CreatePartitions(this.handle, new_parts1, (UIntPtr) (ulong) new_parts1.Length, num1, resultQueuePtr);
      }
      finally
      {
        foreach (IntPtr new_parts3 in new_parts1)
        {
          if (new_parts3 != IntPtr.Zero)
            Librdkafka.NewPartitions_destroy(new_parts3);
        }
      }
      Librdkafka.AdminOptions_destroy(num1);
    }

    internal void DeleteTopics(
      IEnumerable<string> deleteTopics,
      DeleteTopicsOptions options,
      IntPtr resultQueuePtr,
      IntPtr completionSourcePtr)
    {
      this.ThrowIfHandleClosed();
      options = options == null ? new DeleteTopicsOptions() : options;
      IntPtr num1 = Librdkafka.AdminOptions_new(this.handle, Librdkafka.AdminOp.DeleteTopics);
      this.setOption_RequestTimeout(num1, options.RequestTimeout);
      this.setOption_OperationTimeout(num1, options.OperationTimeout);
      this.setOption_completionSource(num1, completionSourcePtr);
      IntPtr[] del_topics = new IntPtr[deleteTopics.Count<string>()];
      try
      {
        int index = 0;
        foreach (string deleteTopic in deleteTopics)
        {
          IntPtr num2 = deleteTopic != null ? Librdkafka.DeleteTopic_new(deleteTopic) : throw new ArgumentException("Cannot delete topics because one or more topics were specified as null.");
          del_topics[index] = num2;
          ++index;
        }
        Librdkafka.DeleteTopics(this.handle, del_topics, (UIntPtr) (ulong) del_topics.Length, num1, resultQueuePtr);
      }
      finally
      {
        foreach (IntPtr del_topic in del_topics)
        {
          if (del_topic != IntPtr.Zero)
            Librdkafka.DeleteTopic_destroy(del_topic);
        }
      }
      Librdkafka.AdminOptions_destroy(num1);
    }

    internal void CreateTopics(
      IEnumerable<TopicSpecification> newTopics,
      CreateTopicsOptions options,
      IntPtr resultQueuePtr,
      IntPtr completionSourcePtr)
    {
      this.ThrowIfHandleClosed();
      StringBuilder errstr = new StringBuilder(512);
      options = options == null ? new CreateTopicsOptions() : options;
      IntPtr num1 = Librdkafka.AdminOptions_new(this.handle, Librdkafka.AdminOp.CreateTopics);
      this.setOption_ValidatOnly(num1, options.ValidateOnly);
      this.setOption_RequestTimeout(num1, options.RequestTimeout);
      this.setOption_OperationTimeout(num1, options.OperationTimeout);
      this.setOption_completionSource(num1, completionSourcePtr);
      IntPtr[] new_topics = new IntPtr[newTopics.Count<TopicSpecification>()];
      try
      {
        int index = 0;
        foreach (TopicSpecification newTopic in newTopics)
        {
          if (newTopic.ReplicationFactor != (short) -1 && newTopic.ReplicasAssignments != null)
            throw new ArgumentException("ReplicationFactor must be -1 when ReplicasAssignments are specified.");
          if (newTopic.Name == null)
            throw new ArgumentException("Cannot create a topic with a name of null.");
          IntPtr new_topic = Librdkafka.NewTopic_new(newTopic.Name, (IntPtr) newTopic.NumPartitions, (IntPtr) (int) newTopic.ReplicationFactor, errstr, (UIntPtr) (ulong) errstr.Capacity);
          if (new_topic == IntPtr.Zero)
            throw new KafkaException(new Confluent.Kafka.Error(ErrorCode.Unknown, errstr.ToString()));
          if (newTopic.ReplicasAssignments != null)
          {
            foreach (KeyValuePair<int, List<int>> replicasAssignment in newTopic.ReplicasAssignments)
            {
              int key = replicasAssignment.Key;
              int[] array = replicasAssignment.Value.ToArray();
              ErrorCode err = Librdkafka.NewTopic_set_replica_assignment(new_topic, key, array, (UIntPtr) (ulong) array.Length, errstr, (UIntPtr) (ulong) errstr.Capacity);
              if (err != ErrorCode.NoError)
                throw new KafkaException(this.CreatePossiblyFatalError(err, errstr.ToString()));
            }
          }
          if (newTopic.Configs != null)
          {
            foreach (KeyValuePair<string, string> config in newTopic.Configs)
            {
              int num2 = (int) Librdkafka.NewTopic_set_config(new_topic, config.Key, config.Value);
            }
          }
          new_topics[index] = new_topic;
          ++index;
        }
        Librdkafka.CreateTopics(this.handle, new_topics, (UIntPtr) (ulong) new_topics.Length, num1, resultQueuePtr);
      }
      finally
      {
        foreach (IntPtr new_topic in new_topics)
        {
          if (new_topic != IntPtr.Zero)
            Librdkafka.NewTopic_destroy(new_topic);
        }
      }
      Librdkafka.AdminOptions_destroy(num1);
    }
  }
}
