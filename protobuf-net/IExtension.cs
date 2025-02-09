// Decompiled with JetBrains decompiler
// Type: ProtoBuf.IExtension
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System.IO;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Provides addition capability for supporting unexpected fields during
  /// protocol-buffer serialization/deserialization. This allows for loss-less
  /// round-trip/merge, even when the data is not fully understood.
  /// </summary>
  public interface IExtension
  {
    /// <summary>
    /// Requests a stream into which any unexpected fields can be persisted.
    /// </summary>
    /// <returns>A new stream suitable for storing data.</returns>
    Stream BeginAppend();

    /// <summary>
    /// Indicates that all unexpected fields have now been stored. The
    /// implementing class is responsible for closing the stream. If
    /// "commit" is not true the data may be discarded.
    /// </summary>
    /// <param name="stream">The stream originally obtained by BeginAppend.</param>
    /// <param name="commit">True if the append operation completed successfully.</param>
    void EndAppend(Stream stream, bool commit);

    /// <summary>
    /// Requests a stream of the unexpected fields previously stored.
    /// </summary>
    /// <returns>A prepared stream of the unexpected fields.</returns>
    Stream BeginQuery();

    /// <summary>
    /// Indicates that all unexpected fields have now been read. The
    /// implementing class is responsible for closing the stream.
    /// </summary>
    /// <param name="stream">The stream originally obtained by BeginQuery.</param>
    void EndQuery(Stream stream);

    /// <summary>
    /// Requests the length of the raw binary stream; this is used
    /// when serializing sub-entities to indicate the expected size.
    /// </summary>
    /// <returns>The length of the binary stream representing unexpected data.</returns>
    int GetLength();
  }
}
