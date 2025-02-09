// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.TimestampType
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Enumerates the different meanings of a message timestamp value.
  /// </summary>
  public enum TimestampType
  {
    /// <summary>Timestamp type is unknown.</summary>
    NotAvailable,
    /// <summary>
    ///     Timestamp relates to message creation time as set by a Kafka client.
    /// </summary>
    CreateTime,
    /// <summary>
    ///     Timestamp relates to the time a message was appended to a Kafka log.
    /// </summary>
    LogAppendTime,
  }
}
