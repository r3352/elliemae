// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoContractAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Indicates that a type is defined for protocol-buffer serialization.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
  public sealed class ProtoContractAttribute : Attribute
  {
    private int implicitFirstTag;
    private ushort flags;
    private const ushort OPTIONS_InferTagFromName = 1;
    private const ushort OPTIONS_InferTagFromNameHasValue = 2;
    private const ushort OPTIONS_UseProtoMembersOnly = 4;
    private const ushort OPTIONS_SkipConstructor = 8;
    private const ushort OPTIONS_IgnoreListHandling = 16;
    private const ushort OPTIONS_AsReferenceDefault = 32;
    private const ushort OPTIONS_EnumPassthru = 64;
    private const ushort OPTIONS_EnumPassthruHasValue = 128;
    private const ushort OPTIONS_IsGroup = 256;

    /// <summary>Gets or sets the defined name of the type.</summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the fist offset to use with implicit field tags;
    /// only uesd if ImplicitFields is set.
    /// </summary>
    public int ImplicitFirstTag
    {
      get => this.implicitFirstTag;
      set
      {
        this.implicitFirstTag = value >= 1 ? value : throw new ArgumentOutOfRangeException(nameof (ImplicitFirstTag));
      }
    }

    /// <summary>
    /// If specified, alternative contract markers (such as markers for XmlSerailizer or DataContractSerializer) are ignored.
    /// </summary>
    public bool UseProtoMembersOnly
    {
      get => this.HasFlag((ushort) 4);
      set => this.SetFlag((ushort) 4, value);
    }

    /// <summary>
    /// If specified, do NOT treat this type as a list, even if it looks like one.
    /// </summary>
    public bool IgnoreListHandling
    {
      get => this.HasFlag((ushort) 16);
      set => this.SetFlag((ushort) 16, value);
    }

    /// <summary>
    /// Gets or sets the mechanism used to automatically infer field tags
    /// for members. This option should be used in advanced scenarios only.
    /// Please review the important notes against the ImplicitFields enumeration.
    /// </summary>
    public ImplicitFields ImplicitFields { get; set; }

    /// <summary>
    /// Enables/disables automatic tag generation based on the existing name / order
    /// of the defined members. This option is not used for members marked
    /// with ProtoMemberAttribute, as intended to provide compatibility with
    /// WCF serialization. WARNING: when adding new fields you must take
    /// care to increase the Order for new elements, otherwise data corruption
    /// may occur.
    /// </summary>
    /// <remarks>If not explicitly specified, the default is assumed from Serializer.GlobalOptions.InferTagFromName.</remarks>
    public bool InferTagFromName
    {
      get => this.HasFlag((ushort) 1);
      set
      {
        this.SetFlag((ushort) 1, value);
        this.SetFlag((ushort) 2, true);
      }
    }

    /// <summary>
    /// Has a InferTagFromName value been explicitly set? if not, the default from the type-model is assumed.
    /// </summary>
    internal bool InferTagFromNameHasValue => this.HasFlag((ushort) 2);

    /// <summary>
    /// Specifies an offset to apply to [DataMember(Order=...)] markers;
    /// this is useful when working with mex-generated classes that have
    /// a different origin (usually 1 vs 0) than the original data-contract.
    /// 
    /// This value is added to the Order of each member.
    /// </summary>
    public int DataMemberOffset { get; set; }

    /// <summary>
    /// If true, the constructor for the type is bypassed during deserialization, meaning any field initializers
    /// or other initialization code is skipped.
    /// </summary>
    public bool SkipConstructor
    {
      get => this.HasFlag((ushort) 8);
      set => this.SetFlag((ushort) 8, value);
    }

    /// <summary>
    /// Should this type be treated as a reference by default? Please also see the implications of this,
    /// as recorded on ProtoMemberAttribute.AsReference
    /// </summary>
    public bool AsReferenceDefault
    {
      get => this.HasFlag((ushort) 32);
      set => this.SetFlag((ushort) 32, value);
    }

    /// <summary>
    /// Indicates whether this type should always be treated as a "group" (rather than a string-prefixed sub-message)
    /// </summary>
    public bool IsGroup
    {
      get => this.HasFlag((ushort) 256);
      set => this.SetFlag((ushort) 256, value);
    }

    private bool HasFlag(ushort flag) => ((int) this.flags & (int) flag) == (int) flag;

    private void SetFlag(ushort flag, bool value)
    {
      if (value)
        this.flags |= flag;
      else
        this.flags &= ~flag;
    }

    /// <summary>
    /// Applies only to enums (not to DTO classes themselves); gets or sets a value indicating that an enum should be treated directly as an int/short/etc, rather
    /// than enforcing .proto enum rules. This is useful *in particul* for [Flags] enums.
    /// </summary>
    public bool EnumPassthru
    {
      get => this.HasFlag((ushort) 64);
      set
      {
        this.SetFlag((ushort) 64, value);
        this.SetFlag((ushort) 128, true);
      }
    }

    /// <summary>
    /// Allows to define a surrogate type used for serialization/deserialization purpose.
    /// </summary>
    public Type Surrogate { get; set; }

    /// <summary>Has a EnumPassthru value been explicitly set?</summary>
    internal bool EnumPassthruHasValue => this.HasFlag((ushort) 128);
  }
}
