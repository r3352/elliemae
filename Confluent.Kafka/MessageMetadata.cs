// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.MessageMetadata
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     All components of <see cref="T:Confluent.Kafka.Message`2" /> except Key and Value.
  /// </summary>
  public class MessageMetadata
  {
    /// <summary>
    ///     The message timestamp. The timestamp type must be set to CreateTime.
    ///     Specify Timestamp.Default to set the message timestamp to the time
    ///     of this function call.
    /// </summary>
    public Timestamp Timestamp { get; set; }

    /// <summary>
    ///     The collection of message headers (or null). Specifying null or an
    ///      empty list are equivalent. The order of headers is maintained, and
    ///     duplicate header keys are allowed.
    /// </summary>
    public Headers Headers { get; set; }
  }
}
