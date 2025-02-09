// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.SafeTopicHandle
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using Confluent.Kafka.Internal;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal sealed class SafeTopicHandle : SafeHandleZeroIsInvalid
  {
    private const int RD_KAFKA_PARTITION_UA = -1;
    internal SafeKafkaHandle kafkaHandle;

    private SafeTopicHandle()
      : base("kafka topic")
    {
    }

    protected override bool ReleaseHandle()
    {
      Librdkafka.topic_destroy(this.handle);
      this.kafkaHandle.DangerousRelease();
      return true;
    }

    internal string GetName() => Util.Marshal.PtrToStringUTF8(Librdkafka.topic_name(this.handle));

    internal bool PartitionAvailable(int partition)
    {
      return Librdkafka.topic_partition_available(this.handle, partition);
    }
  }
}
