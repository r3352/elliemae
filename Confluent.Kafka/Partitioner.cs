// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Partitioner
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Partitioner enum values</summary>
  public enum Partitioner
  {
    /// <summary>Random</summary>
    Random,
    /// <summary>Consistent</summary>
    Consistent,
    /// <summary>ConsistentRandom</summary>
    ConsistentRandom,
    /// <summary>Murmur2</summary>
    Murmur2,
    /// <summary>Murmur2Random</summary>
    Murmur2Random,
  }
}
