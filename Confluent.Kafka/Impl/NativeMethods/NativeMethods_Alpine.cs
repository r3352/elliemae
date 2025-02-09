// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.NativeMethods.NativeMethods_Alpine
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Admin;
using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Confluent.Kafka.Impl.NativeMethods
{
  /// <summary>
  ///     This class should be an exact replica of other NativeMethods classes, except
  ///     for the DllName const.
  /// </summary>
  /// <remarks>
  ///     This copy/pasting is required because DllName must be const.
  ///     TODO: generate the NativeMethods classes at runtime (compile C# code) rather
  ///     than copy/paste.
  /// 
  ///     Alternatively, we could have used dlopen to load the native library, but to
  ///     do that we need to know the absolute path of the native libraries because the
  ///     dlopen call does not know .NET runtime library storage conventions. Unfortunately
  ///     these are relatively complex, so we prefer to go with the copy/paste solution
  ///     which is relatively simple.
  /// </remarks>
  internal class NativeMethods_Alpine
  {
    public const string DllName = "alpine-librdkafka";

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_version();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_version_str();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_get_debug_contexts();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_err2str(ErrorCode err);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_last_error();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_fatal_error(
      IntPtr rk,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_partition_list_new(IntPtr size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_topic_partition_list_destroy(IntPtr rkparlist);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_partition_list_add(
      IntPtr rktparlist,
      string topic,
      int partition);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_headers_new(IntPtr size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_headers_destroy(IntPtr hdrs);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_header_add(
      IntPtr hdrs,
      IntPtr name,
      IntPtr name_size,
      IntPtr value,
      IntPtr value_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_header_get_all(
      IntPtr hdrs,
      IntPtr idx,
      out IntPtr namep,
      out IntPtr valuep,
      out IntPtr sizep);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern long rd_kafka_message_timestamp(IntPtr rkmessage, out IntPtr tstype);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_message_headers(IntPtr rkmessage, out IntPtr hdrs);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern PersistenceStatus rd_kafka_message_status(IntPtr rkmessage);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_message_destroy(IntPtr rkmessage);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SafeConfigHandle rd_kafka_conf_new();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_destroy(IntPtr conf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_conf_dup(IntPtr conf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ConfRes rd_kafka_conf_set(
      IntPtr conf,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      [MarshalAs(UnmanagedType.LPStr)] string value,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_dr_msg_cb(
      IntPtr conf,
      Librdkafka.DeliveryReportDelegate dr_msg_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_rebalance_cb(
      IntPtr conf,
      Librdkafka.RebalanceDelegate rebalance_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_offset_commit_cb(
      IntPtr conf,
      Librdkafka.CommitDelegate commit_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_error_cb(
      IntPtr conf,
      Librdkafka.ErrorDelegate error_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_log_cb(IntPtr conf, Librdkafka.LogDelegate log_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_stats_cb(
      IntPtr conf,
      Librdkafka.StatsDelegate stats_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_set_default_topic_conf(IntPtr conf, IntPtr tconf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ConfRes rd_kafka_conf_get(
      IntPtr conf,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      StringBuilder dest,
      ref UIntPtr dest_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ConfRes rd_kafka_topic_conf_get(
      IntPtr conf,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      StringBuilder dest,
      ref UIntPtr dest_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_conf_dump(IntPtr conf, out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_conf_dump(IntPtr conf, out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_conf_dump_free(IntPtr arr, UIntPtr cnt);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SafeTopicConfigHandle rd_kafka_topic_conf_new();

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_conf_dup(IntPtr conf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_topic_conf_destroy(IntPtr conf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ConfRes rd_kafka_topic_conf_set(
      IntPtr conf,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      [MarshalAs(UnmanagedType.LPStr)] string value,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_topic_conf_set_partitioner_cb(
      IntPtr topic_conf,
      Librdkafka.PartitionerDelegate partitioner_cb);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern bool rd_kafka_topic_partition_available(IntPtr rkt, int partition);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_init_transactions(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_begin_transaction(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_commit_transaction(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_abort_transaction(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_send_offsets_to_transaction(
      IntPtr rk,
      IntPtr offsets,
      IntPtr consumer_group_metadata,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_consumer_group_metadata(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_consumer_group_metadata_destroy(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_consumer_group_metadata_write(
      IntPtr cgmd,
      out IntPtr valuep,
      out IntPtr sizep);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_consumer_group_metadata_read(
      out IntPtr cgmdp,
      byte[] buffer,
      IntPtr size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SafeKafkaHandle rd_kafka_new(
      RdKafkaType type,
      IntPtr conf,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_destroy(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_destroy_flags(IntPtr rk, IntPtr flags);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_name(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_memberid(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern SafeTopicHandle rd_kafka_topic_new(IntPtr rk, [MarshalAs(UnmanagedType.LPStr)] string topic, IntPtr conf);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_topic_destroy(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_name(IntPtr rkt);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_poll_set_consumer(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_poll(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_query_watermark_offsets(
      IntPtr rk,
      [MarshalAs(UnmanagedType.LPStr)] string topic,
      int partition,
      out long low,
      out long high,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_get_watermark_offsets(
      IntPtr rk,
      [MarshalAs(UnmanagedType.LPStr)] string topic,
      int partition,
      out long low,
      out long high);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_offsets_for_times(
      IntPtr rk,
      IntPtr offsets,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_mem_free(IntPtr rk, IntPtr ptr);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_subscribe(IntPtr rk, IntPtr topics);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_unsubscribe(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_subscription(IntPtr rk, out IntPtr topics);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_consumer_poll(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_consumer_close(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_assign(IntPtr rk, IntPtr partitions);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_assignment(IntPtr rk, out IntPtr topics);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_offsets_store(IntPtr rk, IntPtr offsets);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_commit(IntPtr rk, IntPtr offsets, bool async);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_commit_queue(
      IntPtr rk,
      IntPtr offsets,
      IntPtr rkqu,
      Librdkafka.CommitDelegate cb,
      IntPtr opaque);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_pause_partitions(IntPtr rk, IntPtr partitions);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_resume_partitions(IntPtr rk, IntPtr partitions);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_seek(
      IntPtr rkt,
      int partition,
      long offset,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_committed(
      IntPtr rk,
      IntPtr partitions,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_position(IntPtr rk, IntPtr partitions);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_producev(
      IntPtr rk,
      Librdkafka.ProduceVarTag topicType,
      [MarshalAs(UnmanagedType.LPStr)] string topic,
      Librdkafka.ProduceVarTag partitionType,
      int partition,
      Librdkafka.ProduceVarTag vaType,
      IntPtr val,
      UIntPtr len,
      Librdkafka.ProduceVarTag keyType,
      IntPtr key,
      UIntPtr keylen,
      Librdkafka.ProduceVarTag msgflagsType,
      IntPtr msgflags,
      Librdkafka.ProduceVarTag msg_opaqueType,
      IntPtr msg_opaque,
      Librdkafka.ProduceVarTag timestampType,
      long timestamp,
      Librdkafka.ProduceVarTag headersType,
      IntPtr headers,
      Librdkafka.ProduceVarTag endType);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_flush(IntPtr rk, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_metadata(
      IntPtr rk,
      bool all_topics,
      IntPtr only_rkt,
      out IntPtr metadatap,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_metadata_destroy(IntPtr metadata);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_list_groups(
      IntPtr rk,
      string group,
      out IntPtr grplistp,
      IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_group_list_destroy(IntPtr grplist);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_brokers_add(IntPtr rk, [MarshalAs(UnmanagedType.LPStr)] string brokerlist);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern int rd_kafka_outq_len(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_AdminOptions_new(IntPtr rk, Librdkafka.AdminOp op);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_AdminOptions_destroy(IntPtr options);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_AdminOptions_set_request_timeout(
      IntPtr options,
      IntPtr timeout_ms,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_AdminOptions_set_operation_timeout(
      IntPtr options,
      IntPtr timeout_ms,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_AdminOptions_set_validate_only(
      IntPtr options,
      IntPtr true_or_false,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_AdminOptions_set_incremental(
      IntPtr options,
      IntPtr true_or_false,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_AdminOptions_set_broker(
      IntPtr options,
      int broker_id,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_AdminOptions_set_opaque(IntPtr options, IntPtr opaque);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_NewTopic_new(
      [MarshalAs(UnmanagedType.LPStr)] string topic,
      IntPtr num_partitions,
      IntPtr replication_factor,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_NewTopic_destroy(IntPtr new_topic);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_NewTopic_set_replica_assignment(
      IntPtr new_topic,
      int partition,
      int[] broker_ids,
      UIntPtr broker_id_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_NewTopic_set_config(
      IntPtr new_topic,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_CreateTopics(
      IntPtr rk,
      IntPtr[] new_topics,
      UIntPtr new_topic_cnt,
      IntPtr options,
      IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_CreateTopics_result_topics(
      IntPtr result,
      out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_DeleteTopic_new([MarshalAs(UnmanagedType.LPStr)] string topic);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_DeleteTopic_destroy(IntPtr del_topic);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_DeleteTopics(
      IntPtr rk,
      IntPtr[] del_topics,
      UIntPtr del_topic_cnt,
      IntPtr options,
      IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_DeleteTopics_result_topics(
      IntPtr result,
      out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_NewPartitions_new(
      [MarshalAs(UnmanagedType.LPStr)] string topic,
      UIntPtr new_total_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_NewPartitions_destroy(IntPtr new_parts);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_NewPartitions_set_replica_assignment(
      IntPtr new_parts,
      int new_partition_idx,
      int[] broker_ids,
      UIntPtr broker_id_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_CreatePartitions(
      IntPtr rk,
      IntPtr[] new_parts,
      UIntPtr new_parts_cnt,
      IntPtr options,
      IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_CreatePartitions_result_topics(
      IntPtr result,
      out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigSource_name(ConfigSource configsource);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_name(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_value(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ConfigSource rd_kafka_ConfigEntry_source(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_is_read_only(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_is_default(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_is_sensitive(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_is_synonym(IntPtr entry);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigEntry_synonyms(IntPtr entry, out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ResourceType_name(ResourceType restype);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigResource_new(ResourceType restype, [MarshalAs(UnmanagedType.LPStr)] string resname);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_ConfigResource_destroy(IntPtr config);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_ConfigResource_add_config(
      IntPtr config,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_ConfigResource_set_config(
      IntPtr config,
      [MarshalAs(UnmanagedType.LPStr)] string name,
      [MarshalAs(UnmanagedType.LPStr)] string value);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_ConfigResource_delete_config(
      IntPtr config,
      [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigResource_configs(IntPtr config, out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ResourceType rd_kafka_ConfigResource_type(IntPtr config);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigResource_name(IntPtr config);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_ConfigResource_error(IntPtr config);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_ConfigResource_error_string(IntPtr config);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_AlterConfigs(
      IntPtr rk,
      IntPtr[] configs,
      UIntPtr config_cnt,
      IntPtr options,
      IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_AlterConfigs_result_resources(
      IntPtr result,
      out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_DescribeConfigs(
      IntPtr rk,
      IntPtr[] configs,
      UIntPtr config_cnt,
      IntPtr options,
      IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_DescribeConfigs_result_resources(
      IntPtr result,
      out UIntPtr cntp);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_topic_result_error(IntPtr topicres);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_result_error_string(IntPtr topicres);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_topic_result_name(IntPtr topicres);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_queue_new(IntPtr rk);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_queue_destroy(IntPtr rkqu);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_queue_poll(IntPtr rkqu, IntPtr timeout_ms);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_event_destroy(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern Librdkafka.EventType rd_kafka_event_type(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_event_opaque(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_event_error(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_event_error_string(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_event_topic_partition_list(IntPtr rkev);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern ErrorCode rd_kafka_error_code(IntPtr error);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_error_string(IntPtr error);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_error_is_fatal(IntPtr error);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_error_is_retriable(IntPtr error);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr rd_kafka_error_txn_requires_abort(IntPtr error);

    [DllImport("alpine-librdkafka", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void rd_kafka_error_destroy(IntPtr error);
  }
}
