// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.PersistenceStatus
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Enumeration of possible message persistence states.
  /// </summary>
  public enum PersistenceStatus
  {
    /// <summary>
    ///     Message was never transmitted to the broker, or failed with
    ///     an error indicating it was not written to the log.
    ///     Application retry risks ordering, but not duplication.
    /// </summary>
    NotPersisted,
    /// <summary>
    ///     Message was transmitted to broker, but no acknowledgement was
    ///     received. Application retry risks ordering and duplication.
    /// </summary>
    PossiblyPersisted,
    /// <summary>
    ///     Message was written to the log and acknowledged by the broker.
    ///     Note: acks='all' should be used for this to be fully trusted
    ///     in case of a broker failover.
    /// </summary>
    Persisted,
  }
}
