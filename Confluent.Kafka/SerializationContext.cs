// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.SerializationContext
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Context relevant to a serialization or deserialization operation.
  /// </summary>
  /// <summary>
  ///     Create a new SerializationContext object instance.
  /// </summary>
  /// <param name="component">
  ///     The component of the message the serialization operation relates to.
  /// </param>
  /// <param name="topic">
  ///     The topic the data is being written to or read from.
  /// </param>
  /// <param name="headers">
  ///     The collection of message headers (or null). Specifying null or an
  ///     empty list are equivalent. The order of headers is maintained, and
  ///     duplicate header keys are allowed.
  /// </param>
  public struct SerializationContext(MessageComponentType component, string topic, Headers headers = null)
  {
    /// <summary>
    ///     The default SerializationContext value (representing no context defined).
    /// </summary>
    public static SerializationContext Empty => new SerializationContext();

    /// <summary>
    ///     The topic the data is being written to or read from.
    /// </summary>
    public string Topic { get; private set; } = topic;

    /// <summary>
    ///     The component of the message the serialization operation relates to.
    /// </summary>
    public MessageComponentType Component { get; private set; } = component;

    /// <summary>
    ///     The collection of message headers (or null). Specifying null or an
    ///     empty list are equivalent. The order of headers is maintained, and
    ///     duplicate header keys are allowed.
    /// </summary>
    public Headers Headers { get; private set; } = headers;
  }
}
