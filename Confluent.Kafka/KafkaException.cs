// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.KafkaException
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Represents an error that occured during an interaction with Kafka.
  /// </summary>
  public class KafkaException : Exception
  {
    /// <summary>
    ///     Initialize a new instance of KafkaException based on
    ///     an existing Error instance.
    /// </summary>
    /// <param name="error">The Kafka Error.</param>
    public KafkaException(Error error)
      : base(error.ToString())
    {
      this.Error = error;
    }

    /// <summary>
    ///     Initialize a new instance of KafkaException based on
    ///     an existing Error instance and inner exception.
    /// </summary>
    /// <param name="error">The Kafka Error.</param>
    /// <param name="innerException">
    ///     The exception instance that caused this exception.
    /// </param>
    public KafkaException(Error error, Exception innerException)
      : base(error.Reason, innerException)
    {
      this.Error = error;
    }

    /// <summary>
    ///     Initialize a new instance of KafkaException based on
    ///     an existing ErrorCode value.
    /// </summary>
    /// <param name="code">The Kafka ErrorCode.</param>
    public KafkaException(ErrorCode code)
      : base(code.GetReason())
    {
      this.Error = new Error(code);
    }

    /// <summary>
    ///     Gets the Error associated with this KafkaException.
    /// </summary>
    public Error Error { get; }
  }
}
