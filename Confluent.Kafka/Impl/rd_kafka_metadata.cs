// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Impl.rd_kafka_metadata
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Confluent.Kafka.Impl
{
  internal struct rd_kafka_metadata
  {
    internal int broker_cnt;
    internal IntPtr brokers;
    internal int topic_cnt;
    internal IntPtr topics;
    internal int orig_broker_id;
    [MarshalAs(UnmanagedType.LPStr)]
    internal string orig_broker_name;
  }
}
