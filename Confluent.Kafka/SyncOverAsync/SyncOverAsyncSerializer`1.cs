// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.SyncOverAsync.SyncOverAsyncSerializer`1
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka.SyncOverAsync
{
  /// <summary>
  ///     An adapter that allows an async serializer
  ///     to be used where a sync serializer is required.
  ///     In using this adapter, there are two potential
  ///     issues you should be aware of:
  /// 
  ///     1. If you are working in a single threaded
  ///        SynchronizationContext (for example, a
  ///        WindowsForms application), you must ensure
  ///        that all methods awaited by your serializer
  ///        (at all levels) are configured to NOT
  ///        continue on the captured context, otherwise
  ///        your application will deadlock. You do this
  ///        by calling .ConfigureAwait(false) on every
  ///        method awaited in your serializer
  ///        implementation. If your serializer makes use
  ///        of a library that does not do this, you
  ///        can get around this by calling await
  ///        Task.Run(() =&gt; ...) to force the library
  ///        method to execute in a SynchronizationContext
  ///        that is not single threaded. Note: all
  ///        Confluent async serializers are safe to use
  ///        with this adapter.
  /// 
  ///     2. In any application, there is potential
  ///        for a deadlock due to thread pool exhaustion.
  ///        This can happen because in order for an async
  ///        method to complete, a thread pool thread is
  ///        typically required. However, if all available
  ///        thread pool threads are in use waiting for the
  ///        async methods to complete, there will be
  ///        no threads available to complete the tasks
  ///        (deadlock). Due to (a) the large default
  ///        number of thread pool threads in the modern
  ///        runtime and (b) the infrequent need for a
  ///        typical async serializer to wait on an async
  ///        result (i.e. most serializers will only
  ///        infrequently need to execute asynchronously),
  ///        this scenario should not commonly occur in
  ///        practice.
  /// </summary>
  public class SyncOverAsyncSerializer<T> : ISerializer<T>
  {
    private IAsyncSerializer<T> asyncSerializer { get; }

    /// <summary>
    ///     Initializes a new SyncOverAsyncSerializer
    ///     instance.
    /// </summary>
    public SyncOverAsyncSerializer(IAsyncSerializer<T> asyncSerializer)
    {
      this.asyncSerializer = asyncSerializer;
    }

    /// <summary>
    ///     Serialize the key or value of a <see cref="T:Confluent.Kafka.Message`2" />
    ///     instance.
    /// </summary>
    /// <param name="data">The value to serialize.</param>
    /// <param name="context">
    ///     Context relevant to the serialize operation.
    /// </param>
    /// <returns>the serialized data.</returns>
    public byte[] Serialize(T data, SerializationContext context)
    {
      return this.asyncSerializer.SerializeAsync(data, context).ConfigureAwait(false).GetAwaiter().GetResult();
    }
  }
}
