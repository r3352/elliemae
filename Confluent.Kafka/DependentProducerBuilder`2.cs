// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.DependentProducerBuilder`2
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>
  ///     A builder class for <see cref="T:Confluent.Kafka.IProducer`2" /> instance
  ///     implementations that leverage an existing client handle.
  /// 
  ///     [API-SUBJECT-TO-CHANGE] - This class may be removed in the future
  ///     in favor of an improved API for this functionality.
  /// </summary>
  public class DependentProducerBuilder<TKey, TValue>
  {
    /// <summary>The configured client handle.</summary>
    public Handle Handle { get; set; }

    /// <summary>The configured key serializer.</summary>
    public ISerializer<TKey> KeySerializer { get; set; }

    /// <summary>The configured value serializer.</summary>
    public ISerializer<TValue> ValueSerializer { get; set; }

    /// <summary>The configured async key serializer.</summary>
    public IAsyncSerializer<TKey> AsyncKeySerializer { get; set; }

    /// <summary>The configured async value serializer.</summary>
    public IAsyncSerializer<TValue> AsyncValueSerializer { get; set; }

    /// <summary>
    ///     An underlying librdkafka client handle that the Producer will use to
    ///     make broker requests. The handle must be from another Producer
    ///     instance (not Consumer or AdminClient).
    /// </summary>
    public DependentProducerBuilder(Handle handle) => this.Handle = handle;

    /// <summary>The serializer to use to serialize keys.</summary>
    public DependentProducerBuilder<TKey, TValue> SetKeySerializer(ISerializer<TKey> serializer)
    {
      this.KeySerializer = serializer;
      return this;
    }

    /// <summary>The serializer to use to serialize values.</summary>
    public DependentProducerBuilder<TKey, TValue> SetValueSerializer(ISerializer<TValue> serializer)
    {
      this.ValueSerializer = serializer;
      return this;
    }

    /// <summary>The async serializer to use to serialize keys.</summary>
    public DependentProducerBuilder<TKey, TValue> SetKeySerializer(IAsyncSerializer<TKey> serializer)
    {
      this.AsyncKeySerializer = serializer;
      return this;
    }

    /// <summary>The async serializer to use to serialize values.</summary>
    public DependentProducerBuilder<TKey, TValue> SetValueSerializer(
      IAsyncSerializer<TValue> serializer)
    {
      this.AsyncValueSerializer = serializer;
      return this;
    }

    /// <summary>Build a new IProducer implementation instance.</summary>
    public virtual IProducer<TKey, TValue> Build()
    {
      return (IProducer<TKey, TValue>) new Producer<TKey, TValue>(this);
    }
  }
}
