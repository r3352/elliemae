// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.KafkaTxnRequiresAbortException
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that caused the current transaction
  ///     to fail and enter the abortable state.
  /// </summary>
  /// <summary>
  ///     Initialize a new instance of KafkaTxnRequiresAbortException
  ///     based on an existing Error instance.
  /// </summary>
  /// <param name="error">The Error instance.</param>
  public class KafkaTxnRequiresAbortException(Error error) : KafkaException(error)
  {
  }
}
