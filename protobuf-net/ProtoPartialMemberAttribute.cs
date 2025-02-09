// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoPartialMemberAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Declares a member to be used in protocol-buffer serialization, using
  /// the given Tag and MemberName. This allows ProtoMemberAttribute usage
  /// even for partial classes where the individual members are not
  /// under direct control.
  /// A DataFormat may be used to optimise the serialization
  /// format (for instance, using zigzag encoding for negative numbers, or
  /// fixed-length encoding for large values.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
  public sealed class ProtoPartialMemberAttribute : ProtoMemberAttribute
  {
    /// <summary>Creates a new ProtoMemberAttribute instance.</summary>
    /// <param name="tag">Specifies the unique tag used to identify this member within the type.</param>
    /// <param name="memberName">Specifies the member to be serialized.</param>
    public ProtoPartialMemberAttribute(int tag, string memberName)
      : base(tag)
    {
      this.MemberName = !string.IsNullOrEmpty(memberName) ? memberName : throw new ArgumentNullException(nameof (memberName));
    }

    /// <summary>The name of the member to be serialized.</summary>
    public string MemberName { get; private set; }
  }
}
