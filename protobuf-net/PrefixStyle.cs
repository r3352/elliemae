// Decompiled with JetBrains decompiler
// Type: ProtoBuf.PrefixStyle
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Specifies the type of prefix that should be applied to messages.
  /// </summary>
  public enum PrefixStyle
  {
    /// <summary>
    /// No length prefix is applied to the data; the data is terminated only be the end of the stream.
    /// </summary>
    None,
    /// <summary>
    /// A base-128 ("varint", the default prefix format in protobuf) length prefix is applied to the data (efficient for short messages).
    /// </summary>
    Base128,
    /// <summary>
    /// A fixed-length (little-endian) length prefix is applied to the data (useful for compatibility).
    /// </summary>
    Fixed32,
    /// <summary>
    /// A fixed-length (big-endian) length prefix is applied to the data (useful for compatibility).
    /// </summary>
    Fixed32BigEndian,
  }
}
