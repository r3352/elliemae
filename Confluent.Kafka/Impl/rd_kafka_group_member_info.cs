// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.rd_kafka_group_member_info
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal struct rd_kafka_group_member_info
  {
    internal string member_id;
    internal string client_id;
    internal string client_host;
    internal IntPtr member_metadata;
    internal IntPtr member_metadata_size;
    internal IntPtr member_assignment;
    internal IntPtr member_assignment_size;
  }
}
