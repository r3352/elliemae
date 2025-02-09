// Decompiled with JetBrains decompiler
// Type: Confluent.Kafka.Header
// Assembly: Confluent.Kafka, Version=1.4.3.0, Culture=neutral, PublicKeyToken=12c514ca49093d1e
// MVID: D64D07A1-80DE-4516-9A12-7428C1CB46D3
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Confluent.Kafka.xml

using System;

#nullable disable
namespace Confluent.Kafka
{
  /// <summary>Represents a kafka message header.</summary>
  /// <remarks>
  ///     Message headers are supported by v0.11 brokers and above.
  /// </remarks>
  public class Header : IHeader
  {
    private byte[] val;

    /// <summary>The header key.</summary>
    public string Key { get; private set; }

    /// <summary>Get the serialized header value data.</summary>
    public byte[] GetValueBytes() => this.val;

    /// <summary>Create a new Header instance.</summary>
    /// <param name="key">The header key.</param>
    /// <param name="value">The header value (may be null).</param>
    public Header(string key, byte[] value)
    {
      this.Key = key != null ? key : throw new ArgumentNullException("Kafka message header key cannot be null.");
      this.val = value;
    }
  }
}
