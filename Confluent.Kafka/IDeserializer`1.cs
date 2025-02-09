// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.IDeserializer`1
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Defines a deserializer for use with <see cref="T:Confluent.Kafka.Consumer`2" />.
  /// </summary>
  public interface IDeserializer<T>
  {
    /// <summary>Deserialize a message key or value.</summary>
    /// <param name="data">The data to deserialize.</param>
    /// <param name="isNull">Whether or not the value is null.</param>
    /// <param name="context">
    ///     Context relevant to the deserialize operation.
    /// </param>
    /// <returns>The deserialized value.</returns>
    T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context);
  }
}
