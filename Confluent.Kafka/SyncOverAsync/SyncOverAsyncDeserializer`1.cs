// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.SyncOverAsync.SyncOverAsyncDeserializer`1
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka.SyncOverAsync
{
  /// <summary>
  ///     An adapter that allows an async deserializer
  ///     to be used where a sync deserializer is required.
  ///     In using this adapter, there are two potential
  ///     issues you should be aware of:
  /// 
  ///     1. If you are working in a single threaded
  ///        SynchronizationContext (for example, a
  ///        WindowsForms application), you must ensure
  ///        that all methods awaited by your deserializer
  ///        (at all levels) are configured to NOT
  ///        continue on the captured context, otherwise
  ///        your application will deadlock. You do this
  ///        by calling .ConfigureAwait(false) on every
  ///        method awaited in your deserializer
  ///        implementation. If your deserializer makes use
  ///        of a library that does not do this, you
  ///        can get around this by calling await
  ///        Task.Run(() =&gt; ...) to force the library
  ///        method to execute in a SynchronizationContext
  ///        that is not single threaded. Note: all
  ///        Confluent async deserializers comply with the
  ///        above.
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
  ///        typical async deserializer to wait on an async
  ///        result (i.e. most deserializers will only
  ///        infrequently need to execute asynchronously),
  ///        this scenario should not commonly occur in
  ///        practice.
  /// </summary>
  public class SyncOverAsyncDeserializer<T> : IDeserializer<T>
  {
    private IAsyncDeserializer<T> asyncDeserializer { get; }

    /// <summary>Initializes a new SyncOverAsyncDeserializer.</summary>
    public SyncOverAsyncDeserializer(IAsyncDeserializer<T> asyncDeserializer)
    {
      this.asyncDeserializer = asyncDeserializer;
    }

    /// <summary>Deserialize a message key or value.</summary>
    /// <param name="data">The data to deserialize.</param>
    /// <param name="isNull">Whether or not the value is null.</param>
    /// <param name="context">
    ///     Context relevant to the deserialize
    ///     operation.
    /// </param>
    /// <returns>The deserialized value.</returns>
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
      return this.asyncDeserializer.DeserializeAsync(new ReadOnlyMemory<byte>(data.ToArray()), isNull, context).ConfigureAwait(false).GetAwaiter().GetResult();
    }
  }
}
