// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoIncludeAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.ComponentModel;
using System.Reflection;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Indicates the known-types to support for an individual
  /// message. This serializes each level in the hierarchy as
  /// a nested message to retain wire-compatibility with
  /// other protocol-buffer implementations.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
  public sealed class ProtoIncludeAttribute : Attribute
  {
    /// <summary>
    /// Creates a new instance of the ProtoIncludeAttribute.
    /// </summary>
    /// <param name="tag">The unique index (within the type) that will identify this data.</param>
    /// <param name="knownType">The additional type to serialize/deserialize.</param>
    public ProtoIncludeAttribute(int tag, Type knownType)
      : this(tag, knownType == (Type) null ? "" : knownType.AssemblyQualifiedName)
    {
    }

    /// <summary>Creates a new instance of the ProtoIncludeAttribute.</summary>
    /// <param name="tag">The unique index (within the type) that will identify this data.</param>
    /// <param name="knownTypeName">The additional type to serialize/deserialize.</param>
    public ProtoIncludeAttribute(int tag, string knownTypeName)
    {
      if (tag <= 0)
        throw new ArgumentOutOfRangeException(nameof (tag), "Tags must be positive integers");
      if (string.IsNullOrEmpty(knownTypeName))
        throw new ArgumentNullException(nameof (knownTypeName), "Known type cannot be blank");
      this.Tag = tag;
      this.KnownTypeName = knownTypeName;
    }

    /// <summary>
    /// Gets the unique index (within the type) that will identify this data.
    /// </summary>
    public int Tag { get; }

    /// <summary>Gets the additional type to serialize/deserialize.</summary>
    public string KnownTypeName { get; }

    /// <summary>Gets the additional type to serialize/deserialize.</summary>
    public Type KnownType
    {
      get => TypeModel.ResolveKnownType(this.KnownTypeName, (TypeModel) null, (Assembly) null);
    }

    /// <summary>
    /// Specifies whether the inherited sype's sub-message should be
    /// written with a length-prefix (default), or with group markers.
    /// </summary>
    [DefaultValue(DataFormat.Default)]
    public DataFormat DataFormat { get; set; }
  }
}
