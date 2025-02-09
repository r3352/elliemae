// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.ConsumeException
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that occured during message consumption.
  /// </summary>
  public class ConsumeException : KafkaException
  {
    /// <summary>Initialize a new instance of ConsumeException</summary>
    /// <param name="consumerRecord">
    ///     An object that provides information know about the consumer
    ///     record for which the error occured.
    /// </param>
    /// <param name="error">The error that occured.</param>
    /// <param name="innerException">
    ///     The exception instance that caused this exception.
    /// </param>
    public ConsumeException(
      ConsumeResult<byte[], byte[]> consumerRecord,
      Error error,
      Exception innerException)
      : base(error, innerException)
    {
      this.ConsumerRecord = consumerRecord;
    }

    /// <summary>Initialize a new instance of ConsumeException</summary>
    /// <param name="consumerRecord">
    ///     An object that provides information know about the consumer
    ///     record for which the error occured.
    /// </param>
    /// <param name="error">The error that occured.</param>
    public ConsumeException(ConsumeResult<byte[], byte[]> consumerRecord, Error error)
      : base(error)
    {
      this.ConsumerRecord = consumerRecord;
    }

    /// <summary>
    ///     An object that provides information known about the consumer
    ///     record for which the error occured.
    /// </summary>
    public ConsumeResult<byte[], byte[]> ConsumerRecord { get; private set; }
  }
}
