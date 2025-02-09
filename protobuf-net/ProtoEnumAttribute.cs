// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoEnumAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Used to define protocol-buffer specific behavior for
  /// enumerated values.
  /// </summary>
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public sealed class ProtoEnumAttribute : Attribute
  {
    private bool hasValue;
    private int enumValue;

    /// <summary>
    /// Gets or sets the specific value to use for this enum during serialization.
    /// </summary>
    public int Value
    {
      get => this.enumValue;
      set
      {
        this.enumValue = value;
        this.hasValue = true;
      }
    }

    /// <summary>
    /// Indicates whether this instance has a customised value mapping
    /// </summary>
    /// <returns>true if a specific value is set</returns>
    public bool HasValue() => this.hasValue;

    /// <summary>
    /// Gets or sets the defined name of the enum, as used in .proto
    /// (this name is not used during serialization).
    /// </summary>
    public string Name { get; set; }
  }
}
