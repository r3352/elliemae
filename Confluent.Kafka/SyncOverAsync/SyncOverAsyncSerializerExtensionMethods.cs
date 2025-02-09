// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.SyncOverAsync.SyncOverAsyncSerializerExtensionMethods
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.SyncOverAsync
{
  /// <summary>
  ///     Extension methods related to SyncOverAsyncSerializer.
  /// </summary>
  public static class SyncOverAsyncSerializerExtensionMethods
  {
    /// <summary>
    ///     Create a sync serializer by wrapping an async
    ///     one. For more information on the potential
    ///     pitfalls in doing this, refer to <see cref="T:Confluent.Kafka.SyncOverAsync.SyncOverAsyncSerializer`1" />.
    /// </summary>
    public static ISerializer<T> AsSyncOverAsync<T>(this IAsyncSerializer<T> asyncSerializer)
    {
      return (ISerializer<T>) new SyncOverAsyncSerializer<T>(asyncSerializer);
    }
  }
}
