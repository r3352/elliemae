// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.IAsyncDeserializer`1
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;
using System.Threading.Tasks;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     A deserializer for use with <see cref="T:Confluent.Kafka.Consumer`2" />.
  /// </summary>
  public interface IAsyncDeserializer<T>
  {
    /// <summary>Deserialize a message key or value.</summary>
    /// <param name="data">The raw byte data to deserialize.</param>
    /// <param name="isNull">True if this is a null value.</param>
    /// <param name="context">
    ///     Context relevant to the deserialize operation.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.Threading.Tasks.Task" /> that completes
    ///     with the deserialized value.
    /// </returns>
    Task<T> DeserializeAsync(ReadOnlyMemory<byte> data, bool isNull, SerializationContext context);
  }
}
