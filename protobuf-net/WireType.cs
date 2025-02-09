// Decompiled with JetBrains decompiler
// Type: ProtoBuf.WireType
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Indicates the encoding used to represent an individual value in a protobuf stream
  /// </summary>
  public enum WireType
  {
    /// <summary>Represents an error condition</summary>
    None = -1, // 0xFFFFFFFF
    /// <summary>Base-128 variant-length encoding</summary>
    Variant = 0,
    /// <summary>Fixed-length 8-byte encoding</summary>
    Fixed64 = 1,
    /// <summary>Length-variant-prefixed encoding</summary>
    String = 2,
    /// <summary>Indicates the start of a group</summary>
    StartGroup = 3,
    /// <summary>Indicates the end of a group</summary>
    EndGroup = 4,
    /// <summary>Fixed-length 4-byte encoding</summary>
    /// 10
    Fixed32 = 5,
    /// <summary>
    /// This is not a formal wire-type in the "protocol buffers" spec, but
    /// denotes a variant integer that should be interpreted using
    /// zig-zag semantics (so -ve numbers aren't a significant overhead)
    /// </summary>
    SignedVariant = 8,
  }
}
