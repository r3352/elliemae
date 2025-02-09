// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoMapAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Controls the formatting of elements in a dictionary, and indicates that
  /// "map" rules should be used: duplicates *replace* earlier values, rather
  /// than throwing an exception
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class ProtoMapAttribute : Attribute
  {
    /// <summary>Describes the data-format used to store the key</summary>
    public DataFormat KeyFormat { get; set; }

    /// <summary>Describes the data-format used to store the value</summary>
    public DataFormat ValueFormat { get; set; }

    /// <summary>
    /// Disables "map" handling; dictionaries will use ".Add(key,value)" instead of  "[key] = value",
    /// which means duplicate keys will cause an exception (instead of retaining the final value); if
    /// a proto schema is emitted, it will be produced using "repeated" instead of "map"
    /// </summary>
    public bool DisableMap { get; set; }
  }
}
