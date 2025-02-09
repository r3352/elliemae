// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.Librdkafka
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Admin;
using Confluent.Kafka.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal static class Librdkafka
  {
    private const long minVersion = 590847;
    internal const int MaxErrorStringLength = 512;
    private static object loadLockObj = new object();
    private static bool isInitialized = false;
    private static Func<IntPtr> _version;
    private static Func<IntPtr> _version_str;
    private static Func<IntPtr> _get_debug_contexts;
    private static Func<ErrorCode, IntPtr> _err2str;
    private static Func<IntPtr, IntPtr> _topic_partition_list_new;
    private static Action<IntPtr> _topic_partition_list_destroy;
    private static Func<IntPtr, string, int, IntPtr> _topic_partition_list_add;
    private static Func<IntPtr, IntPtr> _headers_new;
    private static Action<IntPtr> _headers_destroy;
    private static Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, ErrorCode> _header_add;
    private static Librdkafka.headerGetAllDelegate _header_get_all;
    private static Func<ErrorCode> _last_error;
    private static Func<IntPtr, StringBuilder, UIntPtr, ErrorCode> _fatal_error;
    private static Librdkafka.messageTimestampDelegate _message_timestamp;
    private static Func<IntPtr, PersistenceStatus> _message_status;
    private static Librdkafka.messageHeadersDelegate _message_headers;
    private static Action<IntPtr> _message_destroy;
    private static Func<SafeConfigHandle> _conf_new;
    private static Action<IntPtr> _conf_destroy;
    private static Func<IntPtr, IntPtr> _conf_dup;
    private static Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes> _conf_set;
    private static Action<IntPtr, Librdkafka.DeliveryReportDelegate> _conf_set_dr_msg_cb;
    private static Action<IntPtr, Librdkafka.RebalanceDelegate> _conf_set_rebalance_cb;
    private static Action<IntPtr, Librdkafka.CommitDelegate> _conf_set_offset_commit_cb;
    private static Action<IntPtr, Librdkafka.ErrorDelegate> _conf_set_error_cb;
    private static Action<IntPtr, Librdkafka.LogDelegate> _conf_set_log_cb;
    private static Action<IntPtr, Librdkafka.StatsDelegate> _conf_set_stats_cb;
    private static Action<IntPtr, IntPtr> _conf_set_default_topic_conf;
    private static Librdkafka.ConfGet _conf_get;
    private static Librdkafka.ConfGet _topic_conf_get;
    private static Librdkafka.ConfDump _conf_dump;
    private static Librdkafka.ConfDump _topic_conf_dump;
    private static Action<IntPtr, UIntPtr> _conf_dump_free;
    private static Func<SafeTopicConfigHandle> _topic_conf_new;
    private static Func<IntPtr, IntPtr> _topic_conf_dup;
    private static Action<IntPtr> _topic_conf_destroy;
    private static Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes> _topic_conf_set;
    private static Action<IntPtr, Librdkafka.PartitionerDelegate> _topic_conf_set_partitioner_cb;
    private static Func<IntPtr, int, bool> _topic_partition_available;
    private static Func<IntPtr, IntPtr, IntPtr> _init_transactions;
    private static Func<IntPtr, IntPtr> _begin_transaction;
    private static Func<IntPtr, IntPtr, IntPtr> _commit_transaction;
    private static Func<IntPtr, IntPtr, IntPtr> _abort_transaction;
    private static Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr> _send_offsets_to_transaction;
    private static Func<IntPtr, IntPtr> _rd_kafka_consumer_group_metadata;
    private static Action<IntPtr> _rd_kafka_consumer_group_metadata_destroy;
    private static Librdkafka.ConsumerGroupMetadataWriteDelegate _rd_kafka_consumer_group_metadata_write;
    private static Librdkafka.ConsumerGroupMetadataReadDelegate _rd_kafka_consumer_group_metadata_read;
    private static Func<RdKafkaType, IntPtr, StringBuilder, UIntPtr, SafeKafkaHandle> _new;
    private static Action<IntPtr> _destroy;
    private static Action<IntPtr, IntPtr> _destroy_flags;
    private static Func<IntPtr, IntPtr> _name;
    private static Func<IntPtr, IntPtr> _memberid;
    private static Func<IntPtr, string, IntPtr, SafeTopicHandle> _topic_new;
    private static Action<IntPtr> _topic_destroy;
    private static Func<IntPtr, IntPtr> _topic_name;
    private static Func<IntPtr, ErrorCode> _poll_set_consumer;
    private static Func<IntPtr, IntPtr, IntPtr> _poll;
    private static Librdkafka.QueryOffsets _query_watermark_offsets;
    private static Librdkafka.GetOffsets _get_watermark_offsets;
    private static Librdkafka.OffsetsForTimes _offsets_for_times;
    private static Action<IntPtr, IntPtr> _mem_free;
    private static Func<IntPtr, IntPtr, ErrorCode> _subscribe;
    private static Func<IntPtr, ErrorCode> _unsubscribe;
    private static Librdkafka.Subscription _subscription;
    private static Func<IntPtr, IntPtr, IntPtr> _consumer_poll;
    private static Func<IntPtr, ErrorCode> _consumer_close;
    private static Func<IntPtr, IntPtr, ErrorCode> _assign;
    private static Librdkafka.Assignment _assignment;
    private static Func<IntPtr, IntPtr, ErrorCode> _offsets_store;
    private static Func<IntPtr, IntPtr, bool, ErrorCode> _commit;
    private static Func<IntPtr, IntPtr, IntPtr, Librdkafka.CommitDelegate, IntPtr, ErrorCode> _commit_queue;
    private static Func<IntPtr, IntPtr, ErrorCode> _pause_partitions;
    private static Func<IntPtr, IntPtr, ErrorCode> _resume_partitions;
    private static Func<IntPtr, int, long, IntPtr, ErrorCode> _seek;
    private static Func<IntPtr, IntPtr, IntPtr, ErrorCode> _committed;
    private static Func<IntPtr, IntPtr, ErrorCode> _position;
    private static Librdkafka.Producev _producev;
    private static Librdkafka.Flush _flush;
    private static Librdkafka.Metadata _metadata;
    private static Action<IntPtr> _metadata_destroy;
    private static Librdkafka.ListGroups _list_groups;
    private static Action<IntPtr> _group_list_destroy;
    private static Func<IntPtr, string, IntPtr> _brokers_add;
    private static Func<IntPtr, int> _outq_len;
    private static Func<IntPtr, Librdkafka.AdminOp, IntPtr> _AdminOptions_new;
    private static Action<IntPtr> _AdminOptions_destroy;
    private static Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode> _AdminOptions_set_request_timeout;
    private static Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode> _AdminOptions_set_operation_timeout;
    private static Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode> _AdminOptions_set_validate_only;
    private static Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode> _AdminOptions_set_incremental;
    private static Func<IntPtr, int, StringBuilder, UIntPtr, ErrorCode> _AdminOptions_set_broker;
    private static Action<IntPtr, IntPtr> _AdminOptions_set_opaque;
    private static Func<string, IntPtr, IntPtr, StringBuilder, UIntPtr, IntPtr> _NewTopic_new;
    private static Action<IntPtr> _NewTopic_destroy;
    private static Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode> _NewTopic_set_replica_assignment;
    private static Func<IntPtr, string, string, ErrorCode> _NewTopic_set_config;
    private static Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr> _CreateTopics;
    private static Librdkafka._CreateTopics_result_topics_delegate _CreateTopics_result_topics;
    private static Func<string, IntPtr> _DeleteTopic_new;
    private static Action<IntPtr> _DeleteTopic_destroy;
    private static Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr> _DeleteTopics;
    private static Librdkafka._DeleteTopics_result_topics_delegate _DeleteTopics_result_topics;
    private static Func<string, UIntPtr, StringBuilder, UIntPtr, IntPtr> _NewPartitions_new;
    private static Action<IntPtr> _NewPartitions_destroy;
    private static Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode> _NewPartitions_set_replica_assignment;
    private static Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr> _CreatePartitions;
    private static Librdkafka._CreatePartitions_result_topics_delegate _CreatePartitions_result_topics;
    private static Func<ConfigSource, IntPtr> _ConfigSource_name;
    private static Func<IntPtr, IntPtr> _ConfigEntry_name;
    private static Func<IntPtr, IntPtr> _ConfigEntry_value;
    private static Func<IntPtr, ConfigSource> _ConfigEntry_source;
    private static Func<IntPtr, IntPtr> _ConfigEntry_is_read_only;
    private static Func<IntPtr, IntPtr> _ConfigEntry_is_default;
    private static Func<IntPtr, IntPtr> _ConfigEntry_is_sensitive;
    private static Func<IntPtr, IntPtr> _ConfigEntry_is_synonym;
    private static Librdkafka._ConfigEntry_synonyms_delegate _ConfigEntry_synonyms;
    private static Func<ResourceType, IntPtr> _ResourceType_name;
    private static Func<ResourceType, string, IntPtr> _ConfigResource_new;
    private static Action<IntPtr> _ConfigResource_destroy;
    private static Func<IntPtr, string, string, ErrorCode> _ConfigResource_add_config;
    private static Func<IntPtr, string, string, ErrorCode> _ConfigResource_set_config;
    private static Func<IntPtr, string, ErrorCode> _ConfigResource_delete_config;
    private static Librdkafka._ConfigResource_configs_delegate _ConfigResource_configs;
    private static Func<IntPtr, ResourceType> _ConfigResource_type;
    private static Func<IntPtr, IntPtr> _ConfigResource_name;
    private static Func<IntPtr, ErrorCode> _ConfigResource_error;
    private static Func<IntPtr, IntPtr> _ConfigResource_error_string;
    private static Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr> _AlterConfigs;
    private static Librdkafka._AlterConfigs_result_resources_delegate _AlterConfigs_result_resources;
    private static Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr> _DescribeConfigs;
    private static Librdkafka._DescribeConfigs_result_resources_delegate _DescribeConfigs_result_resources;
    private static Func<IntPtr, ErrorCode> _topic_result_error;
    private static Func<IntPtr, IntPtr> _topic_result_error_string;
    private static Func<IntPtr, IntPtr> _topic_result_name;
    private static Func<IntPtr, IntPtr> _queue_new;
    private static Action<IntPtr> _queue_destroy;
    private static Func<IntPtr, IntPtr, IntPtr> _queue_poll;
    private static Action<IntPtr> _event_destroy;
    private static Func<IntPtr, IntPtr> _event_opaque;
    private static Func<IntPtr, Librdkafka.EventType> _event_type;
    private static Func<IntPtr, ErrorCode> _event_error;
    private static Func<IntPtr, IntPtr> _event_error_string;
    private static Func<IntPtr, IntPtr> _event_topic_partition_list;
    private static Func<IntPtr, ErrorCode> _error_code;
    private static Func<IntPtr, IntPtr> _error_string;
    private static Func<IntPtr, IntPtr> _error_is_fatal;
    private static Func<IntPtr, IntPtr> _error_is_retriable;
    private static Func<IntPtr, IntPtr> _error_txn_requires_abort;
    private static Action<IntPtr> _error_destroy;

    private static bool SetDelegates(Type nativeMethodsClass)
    {
      MethodInfo[] array = nativeMethodsClass.GetRuntimeMethods().ToArray<MethodInfo>();
      Librdkafka._version = (Func<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_version")).CreateDelegate(typeof (Func<IntPtr>));
      Librdkafka._version_str = (Func<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_version_str")).CreateDelegate(typeof (Func<IntPtr>));
      Librdkafka._get_debug_contexts = (Func<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_get_debug_contexts")).CreateDelegate(typeof (Func<IntPtr>));
      Librdkafka._err2str = (Func<ErrorCode, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_err2str")).CreateDelegate(typeof (Func<ErrorCode, IntPtr>));
      Librdkafka._last_error = (Func<ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_last_error")).CreateDelegate(typeof (Func<ErrorCode>));
      Librdkafka._fatal_error = (Func<IntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_fatal_error")).CreateDelegate(typeof (Func<IntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._topic_partition_list_new = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_partition_list_new")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._topic_partition_list_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_partition_list_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._topic_partition_list_add = (Func<IntPtr, string, int, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_partition_list_add")).CreateDelegate(typeof (Func<IntPtr, string, int, IntPtr>));
      Librdkafka._headers_new = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_headers_new")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._headers_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_headers_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._header_add = (Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_header_add")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr, ErrorCode>));
      Librdkafka._header_get_all = (Librdkafka.headerGetAllDelegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_header_get_all")).CreateDelegate(typeof (Librdkafka.headerGetAllDelegate));
      Librdkafka._message_timestamp = (Librdkafka.messageTimestampDelegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_message_timestamp")).CreateDelegate(typeof (Librdkafka.messageTimestampDelegate));
      Librdkafka._message_headers = (Librdkafka.messageHeadersDelegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_message_headers")).CreateDelegate(typeof (Librdkafka.messageHeadersDelegate));
      Librdkafka._message_status = (Func<IntPtr, PersistenceStatus>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_message_status")).CreateDelegate(typeof (Func<IntPtr, PersistenceStatus>));
      Librdkafka._message_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_message_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._conf_new = (Func<SafeConfigHandle>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_new")).CreateDelegate(typeof (Func<SafeConfigHandle>));
      Librdkafka._conf_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._conf_dup = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_dup")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._conf_set = (Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set")).CreateDelegate(typeof (Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes>));
      Librdkafka._conf_set_dr_msg_cb = (Action<IntPtr, Librdkafka.DeliveryReportDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_dr_msg_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.DeliveryReportDelegate>));
      Librdkafka._conf_set_rebalance_cb = (Action<IntPtr, Librdkafka.RebalanceDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_rebalance_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.RebalanceDelegate>));
      Librdkafka._conf_set_error_cb = (Action<IntPtr, Librdkafka.ErrorDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_error_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.ErrorDelegate>));
      Librdkafka._conf_set_offset_commit_cb = (Action<IntPtr, Librdkafka.CommitDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_offset_commit_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.CommitDelegate>));
      Librdkafka._conf_set_log_cb = (Action<IntPtr, Librdkafka.LogDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_log_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.LogDelegate>));
      Librdkafka._conf_set_stats_cb = (Action<IntPtr, Librdkafka.StatsDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_stats_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.StatsDelegate>));
      Librdkafka._conf_set_default_topic_conf = (Action<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_set_default_topic_conf")).CreateDelegate(typeof (Action<IntPtr, IntPtr>));
      Librdkafka._conf_get = (Librdkafka.ConfGet) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_get")).CreateDelegate(typeof (Librdkafka.ConfGet));
      Librdkafka._topic_conf_get = (Librdkafka.ConfGet) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_get")).CreateDelegate(typeof (Librdkafka.ConfGet));
      Librdkafka._conf_dump = (Librdkafka.ConfDump) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_dump")).CreateDelegate(typeof (Librdkafka.ConfDump));
      Librdkafka._topic_conf_dump = (Librdkafka.ConfDump) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_dump")).CreateDelegate(typeof (Librdkafka.ConfDump));
      Librdkafka._conf_dump_free = (Action<IntPtr, UIntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_conf_dump_free")).CreateDelegate(typeof (Action<IntPtr, UIntPtr>));
      Librdkafka._topic_conf_new = (Func<SafeTopicConfigHandle>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_new")).CreateDelegate(typeof (Func<SafeTopicConfigHandle>));
      Librdkafka._topic_conf_dup = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_dup")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._topic_conf_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._topic_conf_set = (Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_set")).CreateDelegate(typeof (Func<IntPtr, string, string, StringBuilder, UIntPtr, ConfRes>));
      Librdkafka._topic_conf_set_partitioner_cb = (Action<IntPtr, Librdkafka.PartitionerDelegate>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_conf_set_partitioner_cb")).CreateDelegate(typeof (Action<IntPtr, Librdkafka.PartitionerDelegate>));
      Librdkafka._topic_partition_available = (Func<IntPtr, int, bool>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_partition_available")).CreateDelegate(typeof (Func<IntPtr, int, bool>));
      Librdkafka._init_transactions = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_init_transactions")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._begin_transaction = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_begin_transaction")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._commit_transaction = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_commit_transaction")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._abort_transaction = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_abort_transaction")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._send_offsets_to_transaction = (Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_send_offsets_to_transaction")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr, IntPtr, IntPtr>));
      Librdkafka._rd_kafka_consumer_group_metadata = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_group_metadata")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._rd_kafka_consumer_group_metadata_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_group_metadata_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._rd_kafka_consumer_group_metadata_write = (Librdkafka.ConsumerGroupMetadataWriteDelegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_group_metadata_write")).CreateDelegate(typeof (Librdkafka.ConsumerGroupMetadataWriteDelegate));
      Librdkafka._rd_kafka_consumer_group_metadata_read = (Librdkafka.ConsumerGroupMetadataReadDelegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_group_metadata_read")).CreateDelegate(typeof (Librdkafka.ConsumerGroupMetadataReadDelegate));
      Librdkafka._new = (Func<RdKafkaType, IntPtr, StringBuilder, UIntPtr, SafeKafkaHandle>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_new")).CreateDelegate(typeof (Func<RdKafkaType, IntPtr, StringBuilder, UIntPtr, SafeKafkaHandle>));
      Librdkafka._name = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_name")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._memberid = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_memberid")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._topic_new = (Func<IntPtr, string, IntPtr, SafeTopicHandle>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_new")).CreateDelegate(typeof (Func<IntPtr, string, IntPtr, SafeTopicHandle>));
      Librdkafka._topic_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._topic_name = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_name")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._poll = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_poll")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._poll_set_consumer = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_poll_set_consumer")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._query_watermark_offsets = (Librdkafka.QueryOffsets) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_query_watermark_offsets")).CreateDelegate(typeof (Librdkafka.QueryOffsets));
      Librdkafka._get_watermark_offsets = (Librdkafka.GetOffsets) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_get_watermark_offsets")).CreateDelegate(typeof (Librdkafka.GetOffsets));
      Librdkafka._offsets_for_times = (Librdkafka.OffsetsForTimes) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_offsets_for_times")).CreateDelegate(typeof (Librdkafka.OffsetsForTimes));
      Librdkafka._mem_free = (Action<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_mem_free")).CreateDelegate(typeof (Action<IntPtr, IntPtr>));
      Librdkafka._subscribe = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_subscribe")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._unsubscribe = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_unsubscribe")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._subscription = (Librdkafka.Subscription) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_subscription")).CreateDelegate(typeof (Librdkafka.Subscription));
      Librdkafka._consumer_poll = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_poll")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._consumer_close = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_consumer_close")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._assign = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_assign")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._assignment = (Librdkafka.Assignment) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_assignment")).CreateDelegate(typeof (Librdkafka.Assignment));
      Librdkafka._offsets_store = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_offsets_store")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._commit = (Func<IntPtr, IntPtr, bool, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_commit")).CreateDelegate(typeof (Func<IntPtr, IntPtr, bool, ErrorCode>));
      Librdkafka._commit_queue = (Func<IntPtr, IntPtr, IntPtr, Librdkafka.CommitDelegate, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_commit_queue")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr, Librdkafka.CommitDelegate, IntPtr, ErrorCode>));
      Librdkafka._committed = (Func<IntPtr, IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_committed")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr, ErrorCode>));
      Librdkafka._pause_partitions = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_pause_partitions")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._resume_partitions = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_resume_partitions")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._seek = (Func<IntPtr, int, long, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_seek")).CreateDelegate(typeof (Func<IntPtr, int, long, IntPtr, ErrorCode>));
      Librdkafka._position = (Func<IntPtr, IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_position")).CreateDelegate(typeof (Func<IntPtr, IntPtr, ErrorCode>));
      Librdkafka._producev = (Librdkafka.Producev) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_producev")).CreateDelegate(typeof (Librdkafka.Producev));
      Librdkafka._flush = (Librdkafka.Flush) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_flush")).CreateDelegate(typeof (Librdkafka.Flush));
      Librdkafka._metadata = (Librdkafka.Metadata) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_metadata")).CreateDelegate(typeof (Librdkafka.Metadata));
      Librdkafka._metadata_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_metadata_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._list_groups = (Librdkafka.ListGroups) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_list_groups")).CreateDelegate(typeof (Librdkafka.ListGroups));
      Librdkafka._group_list_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_group_list_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._brokers_add = (Func<IntPtr, string, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_brokers_add")).CreateDelegate(typeof (Func<IntPtr, string, IntPtr>));
      Librdkafka._outq_len = (Func<IntPtr, int>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_outq_len")).CreateDelegate(typeof (Func<IntPtr, int>));
      Librdkafka._queue_new = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_queue_new")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._queue_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_queue_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._event_opaque = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_opaque")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._event_type = (Func<IntPtr, Librdkafka.EventType>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_type")).CreateDelegate(typeof (Func<IntPtr, Librdkafka.EventType>));
      Librdkafka._event_error = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_error")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._event_error_string = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_error_string")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._event_topic_partition_list = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_topic_partition_list")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._event_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_event_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._queue_poll = (Func<IntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_queue_poll")).CreateDelegate(typeof (Func<IntPtr, IntPtr, IntPtr>));
      Librdkafka._AdminOptions_new = (Func<IntPtr, Librdkafka.AdminOp, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_new")).CreateDelegate(typeof (Func<IntPtr, Librdkafka.AdminOp, IntPtr>));
      Librdkafka._AdminOptions_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._AdminOptions_set_request_timeout = (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_request_timeout")).CreateDelegate(typeof (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._AdminOptions_set_operation_timeout = (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_operation_timeout")).CreateDelegate(typeof (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._AdminOptions_set_validate_only = (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_validate_only")).CreateDelegate(typeof (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._AdminOptions_set_incremental = (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_incremental")).CreateDelegate(typeof (Func<IntPtr, IntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._AdminOptions_set_broker = (Func<IntPtr, int, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_broker")).CreateDelegate(typeof (Func<IntPtr, int, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._AdminOptions_set_opaque = (Action<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AdminOptions_set_opaque")).CreateDelegate(typeof (Action<IntPtr, IntPtr>));
      Librdkafka._NewTopic_new = (Func<string, IntPtr, IntPtr, StringBuilder, UIntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewTopic_new")).CreateDelegate(typeof (Func<string, IntPtr, IntPtr, StringBuilder, UIntPtr, IntPtr>));
      Librdkafka._NewTopic_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewTopic_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._NewTopic_set_replica_assignment = (Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewTopic_set_replica_assignment")).CreateDelegate(typeof (Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._NewTopic_set_config = (Func<IntPtr, string, string, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewTopic_set_config")).CreateDelegate(typeof (Func<IntPtr, string, string, ErrorCode>));
      Librdkafka._CreateTopics = (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_CreateTopics")).CreateDelegate(typeof (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>));
      Librdkafka._CreateTopics_result_topics = (Librdkafka._CreateTopics_result_topics_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_CreateTopics_result_topics")).CreateDelegate(typeof (Librdkafka._CreateTopics_result_topics_delegate));
      Librdkafka._DeleteTopic_new = (Func<string, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DeleteTopic_new")).CreateDelegate(typeof (Func<string, IntPtr>));
      Librdkafka._DeleteTopic_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DeleteTopic_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._DeleteTopics = (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DeleteTopics")).CreateDelegate(typeof (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>));
      Librdkafka._DeleteTopics_result_topics = (Librdkafka._DeleteTopics_result_topics_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DeleteTopics_result_topics")).CreateDelegate(typeof (Librdkafka._DeleteTopics_result_topics_delegate));
      Librdkafka._NewPartitions_new = (Func<string, UIntPtr, StringBuilder, UIntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewPartitions_new")).CreateDelegate(typeof (Func<string, UIntPtr, StringBuilder, UIntPtr, IntPtr>));
      Librdkafka._NewPartitions_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewPartitions_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._NewPartitions_set_replica_assignment = (Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_NewPartitions_set_replica_assignment")).CreateDelegate(typeof (Func<IntPtr, int, int[], UIntPtr, StringBuilder, UIntPtr, ErrorCode>));
      Librdkafka._CreatePartitions = (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_CreatePartitions")).CreateDelegate(typeof (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>));
      Librdkafka._CreatePartitions_result_topics = (Librdkafka._CreatePartitions_result_topics_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_CreatePartitions_result_topics")).CreateDelegate(typeof (Librdkafka._CreatePartitions_result_topics_delegate));
      Librdkafka._ConfigSource_name = (Func<ConfigSource, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigSource_name")).CreateDelegate(typeof (Func<ConfigSource, IntPtr>));
      Librdkafka._ConfigEntry_name = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_name")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_value = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_value")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_source = (Func<IntPtr, ConfigSource>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_source")).CreateDelegate(typeof (Func<IntPtr, ConfigSource>));
      Librdkafka._ConfigEntry_is_read_only = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_is_read_only")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_is_default = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_is_default")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_is_sensitive = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_is_sensitive")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_is_synonym = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_is_synonym")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigEntry_synonyms = (Librdkafka._ConfigEntry_synonyms_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigEntry_synonyms")).CreateDelegate(typeof (Librdkafka._ConfigEntry_synonyms_delegate));
      Librdkafka._ResourceType_name = (Func<ResourceType, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ResourceType_name")).CreateDelegate(typeof (Func<ResourceType, IntPtr>));
      Librdkafka._ConfigResource_new = (Func<ResourceType, string, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_new")).CreateDelegate(typeof (Func<ResourceType, string, IntPtr>));
      Librdkafka._ConfigResource_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._ConfigResource_add_config = (Func<IntPtr, string, string, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_add_config")).CreateDelegate(typeof (Func<IntPtr, string, string, ErrorCode>));
      Librdkafka._ConfigResource_set_config = (Func<IntPtr, string, string, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_set_config")).CreateDelegate(typeof (Func<IntPtr, string, string, ErrorCode>));
      Librdkafka._ConfigResource_delete_config = (Func<IntPtr, string, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_delete_config")).CreateDelegate(typeof (Func<IntPtr, string, ErrorCode>));
      Librdkafka._ConfigResource_configs = (Librdkafka._ConfigResource_configs_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_configs")).CreateDelegate(typeof (Librdkafka._ConfigResource_configs_delegate));
      Librdkafka._ConfigResource_type = (Func<IntPtr, ResourceType>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_type")).CreateDelegate(typeof (Func<IntPtr, ResourceType>));
      Librdkafka._ConfigResource_name = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_name")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._ConfigResource_error = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_error")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._ConfigResource_error_string = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_ConfigResource_error_string")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._AlterConfigs = (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AlterConfigs")).CreateDelegate(typeof (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>));
      Librdkafka._AlterConfigs_result_resources = (Librdkafka._AlterConfigs_result_resources_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_AlterConfigs_result_resources")).CreateDelegate(typeof (Librdkafka._AlterConfigs_result_resources_delegate));
      Librdkafka._DescribeConfigs = (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DescribeConfigs")).CreateDelegate(typeof (Action<IntPtr, IntPtr[], UIntPtr, IntPtr, IntPtr>));
      Librdkafka._DescribeConfigs_result_resources = (Librdkafka._DescribeConfigs_result_resources_delegate) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_DescribeConfigs_result_resources")).CreateDelegate(typeof (Librdkafka._DescribeConfigs_result_resources_delegate));
      Librdkafka._topic_result_error = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_result_error")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._topic_result_error_string = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_result_error_string")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._topic_result_name = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_topic_result_name")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      Librdkafka._destroy_flags = (Action<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_destroy_flags")).CreateDelegate(typeof (Action<IntPtr, IntPtr>));
      Librdkafka._error_code = (Func<IntPtr, ErrorCode>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_code")).CreateDelegate(typeof (Func<IntPtr, ErrorCode>));
      Librdkafka._error_string = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_string")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._error_is_fatal = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_is_fatal")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._error_is_retriable = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_is_retriable")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._error_txn_requires_abort = (Func<IntPtr, IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_txn_requires_abort")).CreateDelegate(typeof (Func<IntPtr, IntPtr>));
      Librdkafka._error_destroy = (Action<IntPtr>) ((IEnumerable<MethodInfo>) array).Single<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == "rd_kafka_error_destroy")).CreateDelegate(typeof (Action<IntPtr>));
      try
      {
        IntPtr num = Librdkafka._err2str(ErrorCode.NoError);
      }
      catch (Exception ex)
      {
        return false;
      }
      return true;
    }

    public static bool IsInitialized
    {
      get
      {
        lock (Librdkafka.loadLockObj)
          return Librdkafka.isInitialized;
      }
    }

    public static bool Initialize(string userSpecifiedPath)
    {
      lock (Librdkafka.loadLockObj)
      {
        if (Librdkafka.isInitialized)
          return false;
        Librdkafka.isInitialized = false;
        string str = userSpecifiedPath;
        if (str == null)
        {
          bool flag = IntPtr.Size == 8;
          string directoryName = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().EscapedCodeBase).LocalPath);
          str = Path.Combine(Path.Combine(directoryName, flag ? Path.Combine("librdkafka", "x64") : Path.Combine("librdkafka", "x86")), "librdkafka.dll");
          if (!File.Exists(str))
            str = Path.Combine(Path.Combine(directoryName, flag ? "runtimes\\win-x64\\native" : "runtimes\\win-x86\\native"), "librdkafka.dll");
          if (!File.Exists(str))
            str = Path.Combine(Path.Combine(directoryName, flag ? "x64" : "x86"), "librdkafka.dll");
          if (!File.Exists(str))
            str = Path.Combine(directoryName, "librdkafka.dll");
        }
        if (Librdkafka.WindowsNative.LoadLibraryEx(str, IntPtr.Zero, Librdkafka.WindowsNative.LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH) == IntPtr.Zero)
        {
          Win32Exception innerException = new Win32Exception();
          throw new InvalidOperationException("Error while loading librdkafka.dll or its dependencies from " + str + ". Check the directory exists, if not check your deployment process. You can also load the library and its dependencies by yourself before any call to Confluent.Kafka", (Exception) innerException);
        }
        Librdkafka.isInitialized = Librdkafka.SetDelegates(typeof (Confluent.Kafka.Impl.NativeMethods.NativeMethods));
        if (!Librdkafka.isInitialized)
          throw new DllNotFoundException("Failed to load the librdkafka native library.");
        if ((long) Librdkafka.version() < 590847L)
          throw new FileLoadException(string.Format("Invalid librdkafka version {0:x}, expected at least {1:x}", (object) (long) Librdkafka.version(), (object) 590847L));
        Librdkafka.isInitialized = true;
        return Librdkafka.isInitialized;
      }
    }

    internal static IntPtr version() => Librdkafka._version();

    internal static IntPtr version_str() => Librdkafka._version_str();

    internal static IntPtr get_debug_contexts() => Librdkafka._get_debug_contexts();

    internal static IntPtr err2str(ErrorCode err) => Librdkafka._err2str(err);

    internal static IntPtr topic_partition_list_new(IntPtr size)
    {
      return Librdkafka._topic_partition_list_new(size);
    }

    internal static void topic_partition_list_destroy(IntPtr rkparlist)
    {
      Librdkafka._topic_partition_list_destroy(rkparlist);
    }

    internal static IntPtr topic_partition_list_add(IntPtr rktparlist, string topic, int partition)
    {
      return Librdkafka._topic_partition_list_add(rktparlist, topic, partition);
    }

    internal static IntPtr headers_new(IntPtr size) => Librdkafka._headers_new(size);

    internal static void headers_destroy(IntPtr hdrs) => Librdkafka._headers_destroy(hdrs);

    internal static ErrorCode headers_add(
      IntPtr hdrs,
      IntPtr keydata,
      IntPtr keylen,
      IntPtr valdata,
      IntPtr vallen)
    {
      return Librdkafka._header_add(hdrs, keydata, keylen, valdata, vallen);
    }

    internal static ErrorCode header_get_all(
      IntPtr hdrs,
      IntPtr idx,
      out IntPtr namep,
      out IntPtr valuep,
      out IntPtr sizep)
    {
      return Librdkafka._header_get_all(hdrs, idx, out namep, out valuep, out sizep);
    }

    internal static ErrorCode last_error() => Librdkafka._last_error();

    internal static ErrorCode fatal_error(IntPtr rk, StringBuilder sb, UIntPtr len)
    {
      return Librdkafka._fatal_error(rk, sb, len);
    }

    internal static long message_timestamp(IntPtr rkmessage, out IntPtr tstype)
    {
      return Librdkafka._message_timestamp(rkmessage, out tstype);
    }

    internal static PersistenceStatus message_status(IntPtr rkmessage)
    {
      return Librdkafka._message_status(rkmessage);
    }

    internal static ErrorCode message_headers(IntPtr rkmessage, out IntPtr hdrs)
    {
      return Librdkafka._message_headers(rkmessage, out hdrs);
    }

    internal static void message_destroy(IntPtr rkmessage)
    {
      Librdkafka._message_destroy(rkmessage);
    }

    internal static SafeConfigHandle conf_new() => Librdkafka._conf_new();

    internal static void conf_destroy(IntPtr conf) => Librdkafka._conf_destroy(conf);

    internal static IntPtr conf_dup(IntPtr conf) => Librdkafka._conf_dup(conf);

    internal static ConfRes conf_set(
      IntPtr conf,
      string name,
      string value,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._conf_set(conf, name, value, errstr, errstr_size);
    }

    internal static void conf_set_dr_msg_cb(
      IntPtr conf,
      Librdkafka.DeliveryReportDelegate dr_msg_cb)
    {
      Librdkafka._conf_set_dr_msg_cb(conf, dr_msg_cb);
    }

    internal static void conf_set_rebalance_cb(
      IntPtr conf,
      Librdkafka.RebalanceDelegate rebalance_cb)
    {
      Librdkafka._conf_set_rebalance_cb(conf, rebalance_cb);
    }

    internal static void conf_set_offset_commit_cb(IntPtr conf, Librdkafka.CommitDelegate commit_cb)
    {
      Librdkafka._conf_set_offset_commit_cb(conf, commit_cb);
    }

    internal static void conf_set_error_cb(IntPtr conf, Librdkafka.ErrorDelegate error_cb)
    {
      Librdkafka._conf_set_error_cb(conf, error_cb);
    }

    internal static void conf_set_log_cb(IntPtr conf, Librdkafka.LogDelegate log_cb)
    {
      Librdkafka._conf_set_log_cb(conf, log_cb);
    }

    internal static void conf_set_stats_cb(IntPtr conf, Librdkafka.StatsDelegate stats_cb)
    {
      Librdkafka._conf_set_stats_cb(conf, stats_cb);
    }

    internal static void conf_set_default_topic_conf(IntPtr conf, IntPtr tconf)
    {
      Librdkafka._conf_set_default_topic_conf(conf, tconf);
    }

    internal static ConfRes conf_get(
      IntPtr conf,
      string name,
      StringBuilder dest,
      ref UIntPtr dest_size)
    {
      return Librdkafka._conf_get(conf, name, dest, ref dest_size);
    }

    internal static ConfRes topic_conf_get(
      IntPtr conf,
      string name,
      StringBuilder dest,
      ref UIntPtr dest_size)
    {
      return Librdkafka._topic_conf_get(conf, name, dest, ref dest_size);
    }

    internal static IntPtr conf_dump(IntPtr conf, out UIntPtr cntp)
    {
      return Librdkafka._conf_dump(conf, out cntp);
    }

    internal static IntPtr topic_conf_dump(IntPtr conf, out UIntPtr cntp)
    {
      return Librdkafka._topic_conf_dump(conf, out cntp);
    }

    internal static void conf_dump_free(IntPtr arr, UIntPtr cnt)
    {
      Librdkafka._conf_dump_free(arr, cnt);
    }

    internal static SafeTopicConfigHandle topic_conf_new() => Librdkafka._topic_conf_new();

    internal static IntPtr topic_conf_dup(IntPtr conf) => Librdkafka._topic_conf_dup(conf);

    internal static void topic_conf_destroy(IntPtr conf) => Librdkafka._topic_conf_destroy(conf);

    internal static ConfRes topic_conf_set(
      IntPtr conf,
      string name,
      string value,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._topic_conf_set(conf, name, value, errstr, errstr_size);
    }

    internal static void topic_conf_set_partitioner_cb(
      IntPtr topic_conf,
      Librdkafka.PartitionerDelegate partitioner_cb)
    {
      Librdkafka._topic_conf_set_partitioner_cb(topic_conf, partitioner_cb);
    }

    internal static bool topic_partition_available(IntPtr rkt, int partition)
    {
      return Librdkafka._topic_partition_available(rkt, partition);
    }

    internal static IntPtr init_transactions(IntPtr rk, IntPtr timeout)
    {
      return Librdkafka._init_transactions(rk, timeout);
    }

    internal static IntPtr begin_transaction(IntPtr rk) => Librdkafka._begin_transaction(rk);

    internal static IntPtr commit_transaction(IntPtr rk, IntPtr timeout)
    {
      return Librdkafka._commit_transaction(rk, timeout);
    }

    internal static IntPtr abort_transaction(IntPtr rk, IntPtr timeout)
    {
      return Librdkafka._abort_transaction(rk, timeout);
    }

    internal static IntPtr send_offsets_to_transaction(
      IntPtr rk,
      IntPtr offsets,
      IntPtr consumer_group_metadata,
      IntPtr timeout_ms)
    {
      return Librdkafka._send_offsets_to_transaction(rk, offsets, consumer_group_metadata, timeout_ms);
    }

    internal static IntPtr consumer_group_metadata(IntPtr rk)
    {
      return Librdkafka._rd_kafka_consumer_group_metadata(rk);
    }

    internal static void consumer_group_metadata_destroy(IntPtr rk)
    {
      Librdkafka._rd_kafka_consumer_group_metadata_destroy(rk);
    }

    internal static IntPtr consumer_group_metadata_write(
      IntPtr cgmd,
      out IntPtr data,
      out IntPtr dataSize)
    {
      return Librdkafka._rd_kafka_consumer_group_metadata_write(cgmd, out data, out dataSize);
    }

    internal static IntPtr consumer_group_metadata_read(
      out IntPtr cgmd,
      byte[] data,
      IntPtr dataSize)
    {
      return Librdkafka._rd_kafka_consumer_group_metadata_read(out cgmd, data, dataSize);
    }

    internal static SafeKafkaHandle kafka_new(
      RdKafkaType type,
      IntPtr conf,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._new(type, conf, errstr, errstr_size);
    }

    internal static void destroy(IntPtr rk) => Librdkafka._destroy(rk);

    internal static void destroy_flags(IntPtr rk, IntPtr flags)
    {
      Librdkafka._destroy_flags(rk, flags);
    }

    internal static IntPtr name(IntPtr rk) => Librdkafka._name(rk);

    internal static IntPtr memberid(IntPtr rk) => Librdkafka._memberid(rk);

    internal static SafeTopicHandle topic_new(IntPtr rk, string topic, IntPtr conf)
    {
      return Librdkafka._topic_new(rk, topic, conf);
    }

    internal static void topic_destroy(IntPtr rk) => Librdkafka._topic_destroy(rk);

    internal static IntPtr topic_name(IntPtr rkt) => Librdkafka._topic_name(rkt);

    internal static ErrorCode poll_set_consumer(IntPtr rk) => Librdkafka._poll_set_consumer(rk);

    internal static IntPtr poll(IntPtr rk, IntPtr timeout_ms) => Librdkafka._poll(rk, timeout_ms);

    internal static ErrorCode query_watermark_offsets(
      IntPtr rk,
      string topic,
      int partition,
      out long low,
      out long high,
      IntPtr timeout_ms)
    {
      return Librdkafka._query_watermark_offsets(rk, topic, partition, out low, out high, timeout_ms);
    }

    internal static ErrorCode get_watermark_offsets(
      IntPtr rk,
      string topic,
      int partition,
      out long low,
      out long high)
    {
      return Librdkafka._get_watermark_offsets(rk, topic, partition, out low, out high);
    }

    internal static ErrorCode offsets_for_times(IntPtr rk, IntPtr offsets, IntPtr timeout_ms)
    {
      return Librdkafka._offsets_for_times(rk, offsets, timeout_ms);
    }

    internal static void mem_free(IntPtr rk, IntPtr ptr) => Librdkafka._mem_free(rk, ptr);

    internal static ErrorCode subscribe(IntPtr rk, IntPtr topics)
    {
      return Librdkafka._subscribe(rk, topics);
    }

    internal static ErrorCode unsubscribe(IntPtr rk) => Librdkafka._unsubscribe(rk);

    internal static ErrorCode subscription(IntPtr rk, out IntPtr topics)
    {
      return Librdkafka._subscription(rk, out topics);
    }

    internal static IntPtr consumer_poll(IntPtr rk, IntPtr timeout_ms)
    {
      return Librdkafka._consumer_poll(rk, timeout_ms);
    }

    internal static ErrorCode consumer_close(IntPtr rk) => Librdkafka._consumer_close(rk);

    internal static ErrorCode assign(IntPtr rk, IntPtr partitions)
    {
      return Librdkafka._assign(rk, partitions);
    }

    internal static ErrorCode assignment(IntPtr rk, out IntPtr topics)
    {
      return Librdkafka._assignment(rk, out topics);
    }

    internal static ErrorCode offsets_store(IntPtr rk, IntPtr offsets)
    {
      return Librdkafka._offsets_store(rk, offsets);
    }

    internal static ErrorCode commit(IntPtr rk, IntPtr offsets, bool async)
    {
      return Librdkafka._commit(rk, offsets, async);
    }

    internal static ErrorCode commit_queue(
      IntPtr rk,
      IntPtr offsets,
      IntPtr rkqu,
      Librdkafka.CommitDelegate cb,
      IntPtr opaque)
    {
      return Librdkafka._commit_queue(rk, offsets, rkqu, cb, opaque);
    }

    internal static ErrorCode pause_partitions(IntPtr rk, IntPtr partitions)
    {
      return Librdkafka._pause_partitions(rk, partitions);
    }

    internal static ErrorCode resume_partitions(IntPtr rk, IntPtr partitions)
    {
      return Librdkafka._resume_partitions(rk, partitions);
    }

    internal static ErrorCode seek(IntPtr rkt, int partition, long offset, IntPtr timeout_ms)
    {
      return Librdkafka._seek(rkt, partition, offset, timeout_ms);
    }

    internal static ErrorCode committed(IntPtr rk, IntPtr partitions, IntPtr timeout_ms)
    {
      return Librdkafka._committed(rk, partitions, timeout_ms);
    }

    internal static ErrorCode position(IntPtr rk, IntPtr partitions)
    {
      return Librdkafka._position(rk, partitions);
    }

    internal static ErrorCode producev(
      IntPtr rk,
      string topic,
      int partition,
      IntPtr msgflags,
      IntPtr val,
      UIntPtr len,
      IntPtr key,
      UIntPtr keylen,
      long timestamp,
      IntPtr headers,
      IntPtr msg_opaque)
    {
      return Librdkafka._producev(rk, Librdkafka.ProduceVarTag.Topic, topic, Librdkafka.ProduceVarTag.Partition, partition, Librdkafka.ProduceVarTag.Value, val, len, Librdkafka.ProduceVarTag.Key, key, keylen, Librdkafka.ProduceVarTag.Opaque, msg_opaque, Librdkafka.ProduceVarTag.MsgFlags, msgflags, Librdkafka.ProduceVarTag.Timestamp, timestamp, Librdkafka.ProduceVarTag.Headers, headers, Librdkafka.ProduceVarTag.End);
    }

    internal static ErrorCode flush(IntPtr rk, IntPtr timeout_ms)
    {
      return Librdkafka._flush(rk, timeout_ms);
    }

    internal static ErrorCode metadata(
      IntPtr rk,
      bool all_topics,
      IntPtr only_rkt,
      out IntPtr metadatap,
      IntPtr timeout_ms)
    {
      return Librdkafka._metadata(rk, all_topics, only_rkt, out metadatap, timeout_ms);
    }

    internal static void metadata_destroy(IntPtr metadata)
    {
      Librdkafka._metadata_destroy(metadata);
    }

    internal static ErrorCode list_groups(
      IntPtr rk,
      string group,
      out IntPtr grplistp,
      IntPtr timeout_ms)
    {
      return Librdkafka._list_groups(rk, group, out grplistp, timeout_ms);
    }

    internal static void group_list_destroy(IntPtr grplist)
    {
      Librdkafka._group_list_destroy(grplist);
    }

    internal static IntPtr brokers_add(IntPtr rk, string brokerlist)
    {
      return Librdkafka._brokers_add(rk, brokerlist);
    }

    internal static int outq_len(IntPtr rk) => Librdkafka._outq_len(rk);

    internal static IntPtr AdminOptions_new(IntPtr rk, Librdkafka.AdminOp op)
    {
      return Librdkafka._AdminOptions_new(rk, op);
    }

    internal static void AdminOptions_destroy(IntPtr options)
    {
      Librdkafka._AdminOptions_destroy(options);
    }

    internal static ErrorCode AdminOptions_set_request_timeout(
      IntPtr options,
      IntPtr timeout_ms,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._AdminOptions_set_request_timeout(options, timeout_ms, errstr, errstr_size);
    }

    internal static ErrorCode AdminOptions_set_operation_timeout(
      IntPtr options,
      IntPtr timeout_ms,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._AdminOptions_set_operation_timeout(options, timeout_ms, errstr, errstr_size);
    }

    internal static ErrorCode AdminOptions_set_validate_only(
      IntPtr options,
      IntPtr true_or_false,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._AdminOptions_set_validate_only(options, true_or_false, errstr, errstr_size);
    }

    internal static ErrorCode AdminOptions_set_incremental(
      IntPtr options,
      IntPtr true_or_false,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._AdminOptions_set_incremental(options, true_or_false, errstr, errstr_size);
    }

    internal static ErrorCode AdminOptions_set_broker(
      IntPtr options,
      int broker_id,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._AdminOptions_set_broker(options, broker_id, errstr, errstr_size);
    }

    internal static void AdminOptions_set_opaque(IntPtr options, IntPtr opaque)
    {
      Librdkafka._AdminOptions_set_opaque(options, opaque);
    }

    internal static IntPtr NewTopic_new(
      string topic,
      IntPtr num_partitions,
      IntPtr replication_factor,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._NewTopic_new(topic, num_partitions, replication_factor, errstr, errstr_size);
    }

    internal static void NewTopic_destroy(IntPtr new_topic)
    {
      Librdkafka._NewTopic_destroy(new_topic);
    }

    internal static ErrorCode NewTopic_set_replica_assignment(
      IntPtr new_topic,
      int partition,
      int[] broker_ids,
      UIntPtr broker_id_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._NewTopic_set_replica_assignment(new_topic, partition, broker_ids, broker_id_cnt, errstr, errstr_size);
    }

    internal static ErrorCode NewTopic_set_config(IntPtr new_topic, string name, string value)
    {
      return Librdkafka._NewTopic_set_config(new_topic, name, value);
    }

    internal static void CreateTopics(
      IntPtr rk,
      IntPtr[] new_topics,
      UIntPtr new_topic_cnt,
      IntPtr options,
      IntPtr rkqu)
    {
      Librdkafka._CreateTopics(rk, new_topics, new_topic_cnt, options, rkqu);
    }

    internal static IntPtr CreateTopics_result_topics(IntPtr result, out UIntPtr cntp)
    {
      return Librdkafka._CreateTopics_result_topics(result, out cntp);
    }

    internal static IntPtr DeleteTopic_new(string topic) => Librdkafka._DeleteTopic_new(topic);

    internal static void DeleteTopic_destroy(IntPtr del_topic)
    {
      Librdkafka._DeleteTopic_destroy(del_topic);
    }

    internal static void DeleteTopics(
      IntPtr rk,
      IntPtr[] del_topics,
      UIntPtr del_topic_cnt,
      IntPtr options,
      IntPtr rkqu)
    {
      Librdkafka._DeleteTopics(rk, del_topics, del_topic_cnt, options, rkqu);
    }

    internal static IntPtr DeleteTopics_result_topics(IntPtr result, out UIntPtr cntp)
    {
      return Librdkafka._DeleteTopics_result_topics(result, out cntp);
    }

    internal static IntPtr NewPartitions_new(
      string topic,
      UIntPtr new_total_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._NewPartitions_new(topic, new_total_cnt, errstr, errstr_size);
    }

    internal static void NewPartitions_destroy(IntPtr new_parts)
    {
      Librdkafka._NewPartitions_destroy(new_parts);
    }

    internal static ErrorCode NewPartitions_set_replica_assignment(
      IntPtr new_parts,
      int new_partition_idx,
      int[] broker_ids,
      UIntPtr broker_id_cnt,
      StringBuilder errstr,
      UIntPtr errstr_size)
    {
      return Librdkafka._NewPartitions_set_replica_assignment(new_parts, new_partition_idx, broker_ids, broker_id_cnt, errstr, errstr_size);
    }

    internal static void CreatePartitions(
      IntPtr rk,
      IntPtr[] new_parts,
      UIntPtr new_parts_cnt,
      IntPtr options,
      IntPtr rkqu)
    {
      Librdkafka._CreatePartitions(rk, new_parts, new_parts_cnt, options, rkqu);
    }

    internal static IntPtr CreatePartitions_result_topics(IntPtr result, out UIntPtr cntp)
    {
      return Librdkafka._CreatePartitions_result_topics(result, out cntp);
    }

    internal static IntPtr ConfigSource_name(ConfigSource configsource)
    {
      return Librdkafka._ConfigSource_name(configsource);
    }

    internal static IntPtr ConfigEntry_name(IntPtr entry) => Librdkafka._ConfigEntry_name(entry);

    internal static IntPtr ConfigEntry_value(IntPtr entry) => Librdkafka._ConfigEntry_value(entry);

    internal static ConfigSource ConfigEntry_source(IntPtr entry)
    {
      return Librdkafka._ConfigEntry_source(entry);
    }

    internal static IntPtr ConfigEntry_is_read_only(IntPtr entry)
    {
      return Librdkafka._ConfigEntry_is_read_only(entry);
    }

    internal static IntPtr ConfigEntry_is_default(IntPtr entry)
    {
      return Librdkafka._ConfigEntry_is_default(entry);
    }

    internal static IntPtr ConfigEntry_is_sensitive(IntPtr entry)
    {
      return Librdkafka._ConfigEntry_is_sensitive(entry);
    }

    internal static IntPtr ConfigEntry_is_synonym(IntPtr entry)
    {
      return Librdkafka._ConfigEntry_is_synonym(entry);
    }

    internal static IntPtr ConfigEntry_synonyms(IntPtr entry, out UIntPtr cntp)
    {
      return Librdkafka._ConfigEntry_synonyms(entry, out cntp);
    }

    internal static IntPtr ResourceType_name(ResourceType restype)
    {
      return Librdkafka._ResourceType_name(restype);
    }

    internal static IntPtr ConfigResource_new(ResourceType restype, string resname)
    {
      return Librdkafka._ConfigResource_new(restype, resname);
    }

    internal static void ConfigResource_destroy(IntPtr config)
    {
      Librdkafka._ConfigResource_destroy(config);
    }

    internal static ErrorCode ConfigResource_add_config(IntPtr config, string name, string value)
    {
      return Librdkafka._ConfigResource_add_config(config, name, value);
    }

    internal static ErrorCode ConfigResource_set_config(IntPtr config, string name, string value)
    {
      return Librdkafka._ConfigResource_set_config(config, name, value);
    }

    internal static ErrorCode ConfigResource_delete_config(IntPtr config, string name)
    {
      return Librdkafka._ConfigResource_delete_config(config, name);
    }

    internal static IntPtr ConfigResource_configs(IntPtr config, out UIntPtr cntp)
    {
      return Librdkafka._ConfigResource_configs(config, out cntp);
    }

    internal static ResourceType ConfigResource_type(IntPtr config)
    {
      return Librdkafka._ConfigResource_type(config);
    }

    internal static IntPtr ConfigResource_name(IntPtr config)
    {
      return Librdkafka._ConfigResource_name(config);
    }

    internal static ErrorCode ConfigResource_error(IntPtr config)
    {
      return Librdkafka._ConfigResource_error(config);
    }

    internal static IntPtr ConfigResource_error_string(IntPtr config)
    {
      return Librdkafka._ConfigResource_error_string(config);
    }

    internal static void AlterConfigs(
      IntPtr rk,
      IntPtr[] configs,
      UIntPtr config_cnt,
      IntPtr options,
      IntPtr rkqu)
    {
      Librdkafka._AlterConfigs(rk, configs, config_cnt, options, rkqu);
    }

    internal static IntPtr AlterConfigs_result_resources(IntPtr result, out UIntPtr cntp)
    {
      return Librdkafka._AlterConfigs_result_resources(result, out cntp);
    }

    internal static void DescribeConfigs(
      IntPtr rk,
      IntPtr[] configs,
      UIntPtr config_cnt,
      IntPtr options,
      IntPtr rkqu)
    {
      Librdkafka._DescribeConfigs(rk, configs, config_cnt, options, rkqu);
    }

    internal static IntPtr DescribeConfigs_result_resources(IntPtr result, out UIntPtr cntp)
    {
      return Librdkafka._DescribeConfigs_result_resources(result, out cntp);
    }

    internal static ErrorCode topic_result_error(IntPtr topicres)
    {
      return Librdkafka._topic_result_error(topicres);
    }

    internal static IntPtr topic_result_error_string(IntPtr topicres)
    {
      return Librdkafka._topic_result_error_string(topicres);
    }

    internal static IntPtr topic_result_name(IntPtr topicres)
    {
      return Librdkafka._topic_result_name(topicres);
    }

    internal static IntPtr queue_new(IntPtr rk) => Librdkafka._queue_new(rk);

    internal static void queue_destroy(IntPtr rkqu) => Librdkafka._queue_destroy(rkqu);

    internal static IntPtr queue_poll(IntPtr rkqu, int timeout_ms)
    {
      return Librdkafka._queue_poll(rkqu, (IntPtr) timeout_ms);
    }

    internal static void event_destroy(IntPtr rkev) => Librdkafka._event_destroy(rkev);

    internal static IntPtr event_opaque(IntPtr rkev) => Librdkafka._event_opaque(rkev);

    internal static Librdkafka.EventType event_type(IntPtr rkev) => Librdkafka._event_type(rkev);

    internal static ErrorCode event_error(IntPtr rkev) => Librdkafka._event_error(rkev);

    internal static string event_error_string(IntPtr rkev)
    {
      return Util.Marshal.PtrToStringUTF8(Librdkafka._event_error_string(rkev));
    }

    internal static IntPtr event_topic_partition_list(IntPtr rkev)
    {
      return Librdkafka._event_topic_partition_list(rkev);
    }

    internal static ErrorCode error_code(IntPtr error) => Librdkafka._error_code(error);

    internal static string error_string(IntPtr error)
    {
      return Util.Marshal.PtrToStringUTF8(Librdkafka._error_string(error));
    }

    internal static bool error_is_fatal(IntPtr error)
    {
      return Librdkafka._error_is_fatal(error) != IntPtr.Zero;
    }

    internal static bool error_is_retriable(IntPtr error)
    {
      return Librdkafka._error_is_retriable(error) != IntPtr.Zero;
    }

    internal static bool error_txn_requires_abort(IntPtr error)
    {
      return Librdkafka._error_txn_requires_abort(error) != IntPtr.Zero;
    }

    internal static void error_destroy(IntPtr error) => Librdkafka.error_destroy(error);

    internal enum DestroyFlags
    {
      RD_KAFKA_DESTROY_F_NO_CONSUMER_CLOSE = 8,
    }

    internal enum AdminOp
    {
      Any,
      CreateTopics,
      DeleteTopics,
      CreatePartitions,
      AlterConfigs,
      DescribeConfigs,
    }

    public enum EventType
    {
      None = 0,
      DR = 1,
      Fetch = 2,
      Log = 4,
      Error = 8,
      Rebalance = 16, // 0x00000010
      Offset_Commit = 32, // 0x00000020
      Stats = 64, // 0x00000040
      CreateTopics_Result = 100, // 0x00000064
      DeleteTopics_Result = 101, // 0x00000065
      CreatePartitions_Result = 102, // 0x00000066
      AlterConfigs_Result = 103, // 0x00000067
      DescribeConfigs_Result = 104, // 0x00000068
    }

    private static class WindowsNative
    {
      [DllImport("kernel32", SetLastError = true)]
      public static extern IntPtr LoadLibraryEx(
        string lpFileName,
        IntPtr hReservedNull,
        Librdkafka.WindowsNative.LoadLibraryFlags dwFlags);

      [DllImport("kernel32", SetLastError = true)]
      public static extern IntPtr GetModuleHandle(string lpFileName);

      [DllImport("kernel32", SetLastError = true)]
      public static extern IntPtr GetProcAddress(IntPtr hModule, string procname);

      [Flags]
      public enum LoadLibraryFlags : uint
      {
        DONT_RESOLVE_DLL_REFERENCES = 1,
        LOAD_IGNORE_CODE_AUTHZ_LEVEL = 16, // 0x00000010
        LOAD_LIBRARY_AS_DATAFILE = 2,
        LOAD_LIBRARY_AS_DATAFILE_EXCLUSIVE = 64, // 0x00000040
        LOAD_LIBRARY_AS_IMAGE_RESOURCE = 32, // 0x00000020
        LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 512, // 0x00000200
        LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 4096, // 0x00001000
        LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 256, // 0x00000100
        LOAD_LIBRARY_SEARCH_SYSTEM32 = 2048, // 0x00000800
        LOAD_LIBRARY_SEARCH_USER_DIRS = 1024, // 0x00000400
        LOAD_WITH_ALTERED_SEARCH_PATH = 8,
      }
    }

    private static class PosixNative
    {
      [DllImport("libdl")]
      public static extern IntPtr dlopen(string fileName, int flags);

      [DllImport("libdl")]
      public static extern IntPtr dlerror();

      [DllImport("libdl")]
      public static extern IntPtr dlsym(IntPtr handle, string symbol);

      public static string LastError
      {
        get
        {
          IntPtr ptr = Librdkafka.PosixNative.dlerror();
          return ptr == IntPtr.Zero ? "" : System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ptr);
        }
      }
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DeliveryReportDelegate(IntPtr rk, IntPtr rkmessage, IntPtr opaque);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CommitDelegate(IntPtr rk, ErrorCode err, IntPtr offsets, IntPtr opaque);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void ErrorDelegate(IntPtr rk, ErrorCode err, string reason, IntPtr opaque);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void RebalanceDelegate(
      IntPtr rk,
      ErrorCode err,
      IntPtr partitions,
      IntPtr opaque);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void LogDelegate(IntPtr rk, SyslogLevel level, string fac, string buf);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int StatsDelegate(IntPtr rk, IntPtr json, UIntPtr json_len, IntPtr opaque);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate int PartitionerDelegate(
      IntPtr rkt,
      IntPtr keydata,
      UIntPtr keylen,
      int partition_cnt,
      IntPtr rkt_opaque,
      IntPtr msg_opaque);

    internal delegate ErrorCode headerGetAllDelegate(
      IntPtr hdrs,
      IntPtr idx,
      out IntPtr namep,
      out IntPtr valuep,
      out IntPtr sizep);

    internal delegate long messageTimestampDelegate(IntPtr rkmessage, out IntPtr tstype);

    internal delegate ErrorCode messageHeadersDelegate(IntPtr rkmessage, out IntPtr hdrsType);

    private delegate ConfRes ConfGet(
      IntPtr conf,
      string name,
      StringBuilder dest,
      ref UIntPtr dest_size);

    private delegate IntPtr ConfDump(IntPtr conf, out UIntPtr cntp);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr ConsumerGroupMetadataWriteDelegate(
      IntPtr cgmd,
      out IntPtr data,
      out IntPtr dataSize);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate IntPtr ConsumerGroupMetadataReadDelegate(
      out IntPtr cgmd,
      byte[] data,
      IntPtr dataSize);

    private delegate ErrorCode QueryOffsets(
      IntPtr rk,
      string topic,
      int partition,
      out long low,
      out long high,
      IntPtr timeout_ms);

    private delegate ErrorCode GetOffsets(
      IntPtr rk,
      string topic,
      int partition,
      out long low,
      out long high);

    private delegate ErrorCode OffsetsForTimes(IntPtr rk, IntPtr offsets, IntPtr timeout_ms);

    private delegate ErrorCode Subscription(IntPtr rk, out IntPtr topics);

    private delegate ErrorCode Assignment(IntPtr rk, out IntPtr topics);

    /// <summary>Var-arg tag types, used in producev</summary>
    internal enum ProduceVarTag
    {
      End,
      Topic,
      Rkt,
      Partition,
      Value,
      Key,
      Opaque,
      MsgFlags,
      Timestamp,
      Header,
      Headers,
    }

    private delegate ErrorCode Producev(
      IntPtr rk,
      Librdkafka.ProduceVarTag topicTag,
      string topic,
      Librdkafka.ProduceVarTag partitionTag,
      int partition,
      Librdkafka.ProduceVarTag vaTag,
      IntPtr val,
      UIntPtr len,
      Librdkafka.ProduceVarTag keyTag,
      IntPtr key,
      UIntPtr keylen,
      Librdkafka.ProduceVarTag msgflagsTag,
      IntPtr msgflags,
      Librdkafka.ProduceVarTag msg_opaqueTag,
      IntPtr msg_opaque,
      Librdkafka.ProduceVarTag timestampTag,
      long timestamp,
      Librdkafka.ProduceVarTag headersTag,
      IntPtr headers,
      Librdkafka.ProduceVarTag endTag);

    private delegate ErrorCode Flush(IntPtr rk, IntPtr timeout_ms);

    private delegate ErrorCode Metadata(
      IntPtr rk,
      bool all_topics,
      IntPtr only_rkt,
      out IntPtr metadatap,
      IntPtr timeout_ms);

    private delegate ErrorCode ListGroups(
      IntPtr rk,
      string group,
      out IntPtr grplistp,
      IntPtr timeout_ms);

    private delegate IntPtr _CreateTopics_result_topics_delegate(IntPtr result, out UIntPtr cntp);

    private delegate IntPtr _DeleteTopics_result_topics_delegate(IntPtr result, out UIntPtr cntp);

    private delegate IntPtr _CreatePartitions_result_topics_delegate(
      IntPtr result,
      out UIntPtr cntp);

    private delegate IntPtr _ConfigEntry_synonyms_delegate(IntPtr entry, out UIntPtr cntp);

    private delegate IntPtr _ConfigResource_configs_delegate(IntPtr config, out UIntPtr cntp);

    private delegate IntPtr _AlterConfigs_result_resources_delegate(IntPtr result, out UIntPtr cntp);

    private delegate IntPtr _DescribeConfigs_result_resources_delegate(
      IntPtr result,
      out UIntPtr cntp);
  }
}
