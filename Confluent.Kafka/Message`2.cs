// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Message`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Represents a (deserialized) Kafka message.</summary>
  public class Message<TKey, TValue> : MessageMetadata
  {
    /// <summary>Gets the message key value (possibly null).</summary>
    public TKey Key { get; set; }

    /// <summary>Gets the message value (possibly null).</summary>
    public TValue Value { get; set; }
  }
}
