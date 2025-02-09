// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoMemberAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Declares a member to be used in protocol-buffer serialization, using
  /// the given Tag. A DataFormat may be used to optimise the serialization
  /// format (for instance, using zigzag encoding for negative numbers, or
  /// fixed-length encoding for large values.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class ProtoMemberAttribute : Attribute, IComparable, IComparable<ProtoMemberAttribute>
  {
    internal MemberInfo Member;
    internal MemberInfo BackingMember;
    internal bool TagIsPinned;
    private string name;
    private DataFormat dataFormat;
    private int tag;
    private MemberSerializationOptions options;

    /// <summary>
    /// Compare with another ProtoMemberAttribute for sorting purposes
    /// </summary>
    public int CompareTo(object other) => this.CompareTo(other as ProtoMemberAttribute);

    /// <summary>
    /// Compare with another ProtoMemberAttribute for sorting purposes
    /// </summary>
    public int CompareTo(ProtoMemberAttribute other)
    {
      if (other == null)
        return -1;
      if (this == other)
        return 0;
      int num = this.tag.CompareTo(other.tag);
      if (num == 0)
        num = string.CompareOrdinal(this.name, other.name);
      return num;
    }

    /// <summary>Creates a new ProtoMemberAttribute instance.</summary>
    /// <param name="tag">Specifies the unique tag used to identify this member within the type.</param>
    public ProtoMemberAttribute(int tag)
      : this(tag, false)
    {
    }

    internal ProtoMemberAttribute(int tag, bool forced)
    {
      this.tag = tag > 0 || forced ? tag : throw new ArgumentOutOfRangeException(nameof (tag));
    }

    /// <summary>
    /// Gets or sets the original name defined in the .proto; not used
    /// during serialization.
    /// </summary>
    public string Name
    {
      get => this.name;
      set => this.name = value;
    }

    /// <summary>
    /// Gets or sets the data-format to be used when encoding this value.
    /// </summary>
    public DataFormat DataFormat
    {
      get => this.dataFormat;
      set => this.dataFormat = value;
    }

    /// <summary>
    /// Gets the unique tag used to identify this member within the type.
    /// </summary>
    public int Tag => this.tag;

    internal void Rebase(int tag) => this.tag = tag;

    /// <summary>
    /// Gets or sets a value indicating whether this member is mandatory.
    /// </summary>
    public bool IsRequired
    {
      get
      {
        return (this.options & MemberSerializationOptions.Required) == MemberSerializationOptions.Required;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.Required;
        else
          this.options &= ~MemberSerializationOptions.Required;
      }
    }

    /// <summary>
    /// Gets a value indicating whether this member is packed.
    /// This option only applies to list/array data of primitive types (int, double, etc).
    /// </summary>
    public bool IsPacked
    {
      get
      {
        return (this.options & MemberSerializationOptions.Packed) == MemberSerializationOptions.Packed;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.Packed;
        else
          this.options &= ~MemberSerializationOptions.Packed;
      }
    }

    /// <summary>
    /// Indicates whether this field should *repace* existing values (the default is false, meaning *append*).
    /// This option only applies to list/array data.
    /// </summary>
    public bool OverwriteList
    {
      get
      {
        return (this.options & MemberSerializationOptions.OverwriteList) == MemberSerializationOptions.OverwriteList;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.OverwriteList;
        else
          this.options &= ~MemberSerializationOptions.OverwriteList;
      }
    }

    /// <summary>Enables full object-tracking/full-graph support.</summary>
    public bool AsReference
    {
      get
      {
        return (this.options & MemberSerializationOptions.AsReference) == MemberSerializationOptions.AsReference;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.AsReference;
        else
          this.options &= ~MemberSerializationOptions.AsReference;
        this.options |= MemberSerializationOptions.AsReferenceHasValue;
      }
    }

    internal bool AsReferenceHasValue
    {
      get
      {
        return (this.options & MemberSerializationOptions.AsReferenceHasValue) == MemberSerializationOptions.AsReferenceHasValue;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.AsReferenceHasValue;
        else
          this.options &= ~MemberSerializationOptions.AsReferenceHasValue;
      }
    }

    /// <summary>
    /// Embeds the type information into the stream, allowing usage with types not known in advance.
    /// </summary>
    public bool DynamicType
    {
      get
      {
        return (this.options & MemberSerializationOptions.DynamicType) == MemberSerializationOptions.DynamicType;
      }
      set
      {
        if (value)
          this.options |= MemberSerializationOptions.DynamicType;
        else
          this.options &= ~MemberSerializationOptions.DynamicType;
      }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this member is packed (lists/arrays).
    /// </summary>
    public MemberSerializationOptions Options
    {
      get => this.options;
      set => this.options = value;
    }
  }
}
