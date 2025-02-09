// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.IAsyncSerializer`1
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System.Threading.Tasks;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     Defines a serializer for use with <see cref="T:Confluent.Kafka.Producer`2" />.
  /// </summary>
  public interface IAsyncSerializer<T>
  {
    /// <summary>
    ///     Serialize the key or value of a <see cref="T:Confluent.Kafka.Message`2" />
    ///     instance.
    /// </summary>
    /// <param name="data">The value to serialize.</param>
    /// <param name="context">
    ///     Context relevant to the serialize operation.
    /// </param>
    /// <returns>
    ///     A <see cref="T:System.Threading.Tasks.Task" /> that
    ///     completes with the serialized data.
    /// </returns>
    Task<byte[]> SerializeAsync(T data, SerializationContext context);
  }
}
