// Decompiled with JetBrains decompiler
// Type: ProtoBuf.MemberSerializationOptions
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Additional (optional) settings that control serialization of members
  /// </summary>
  [Flags]
  public enum MemberSerializationOptions
  {
    /// <summary>Default; no additional options</summary>
    None = 0,
    /// <summary>
    /// Indicates that repeated elements should use packed (length-prefixed) encoding
    /// </summary>
    Packed = 1,
    /// <summary>Indicates that the given item is required</summary>
    Required = 2,
    /// <summary>Enables full object-tracking/full-graph support</summary>
    AsReference = 4,
    /// <summary>
    /// Embeds the type information into the stream, allowing usage with types not known in advance
    /// </summary>
    DynamicType = 8,
    /// <summary>
    /// Indicates whether this field should *repace* existing values (the default is false, meaning *append*).
    /// This option only applies to list/array data.
    /// </summary>
    OverwriteList = 16, // 0x00000010
    /// <summary>
    /// Determines whether the types AsReferenceDefault value is used, or whether this member's AsReference should be used
    /// </summary>
    AsReferenceHasValue = 32, // 0x00000020
  }
}
