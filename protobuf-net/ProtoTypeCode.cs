// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoTypeCode
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Intended to be a direct map to regular TypeCode, but:
  /// - with missing types
  /// - existing on WinRT
  /// </summary>
  internal enum ProtoTypeCode
  {
    Empty = 0,
    Unknown = 1,
    Boolean = 3,
    Char = 4,
    SByte = 5,
    Byte = 6,
    Int16 = 7,
    UInt16 = 8,
    Int32 = 9,
    UInt32 = 10, // 0x0000000A
    Int64 = 11, // 0x0000000B
    UInt64 = 12, // 0x0000000C
    Single = 13, // 0x0000000D
    Double = 14, // 0x0000000E
    Decimal = 15, // 0x0000000F
    DateTime = 16, // 0x00000010
    String = 18, // 0x00000012
    TimeSpan = 100, // 0x00000064
    ByteArray = 101, // 0x00000065
    Guid = 102, // 0x00000066
    Uri = 103, // 0x00000067
    Type = 104, // 0x00000068
  }
}
