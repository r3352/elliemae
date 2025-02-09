// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ProduceException`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that occured whilst producing a message.
  /// </summary>
  public class ProduceException<TKey, TValue> : KafkaException
  {
    /// <summary>
    ///     Initialize a new instance of ProduceException based on
    ///     an existing Error value.
    /// </summary>
    /// <param name="error">
    ///     The error associated with the delivery result.
    /// </param>
    /// <param name="deliveryResult">
    ///     The delivery result associated with the produce request.
    /// </param>
    /// <param name="innerException">
    ///     The exception instance that caused this exception.
    /// </param>
    public ProduceException(
      Error error,
      Confluent.Kafka.DeliveryResult<TKey, TValue> deliveryResult,
      Exception innerException)
      : base(error, innerException)
    {
      this.DeliveryResult = deliveryResult;
    }

    /// <summary>
    ///     Initialize a new instance of ProduceException based on
    ///     an existing Error value.
    /// </summary>
    /// <param name="error">
    ///     The error associated with the delivery report.
    /// </param>
    /// <param name="deliveryResult">
    ///     The delivery result associated with the produce request.
    /// </param>
    public ProduceException(Error error, Confluent.Kafka.DeliveryResult<TKey, TValue> deliveryResult)
      : base(error)
    {
      this.DeliveryResult = deliveryResult;
    }

    /// <summary>
    ///     The delivery result associated with the produce request.
    /// </summary>
    public Confluent.Kafka.DeliveryResult<TKey, TValue> DeliveryResult { get; }
  }
}
