// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ImplicitFields
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Specifies the method used to infer field tags for members of the type
  /// under consideration. Tags are deduced using the invariant alphabetic
  /// sequence of the members' names; this makes implicit field tags very brittle,
  /// and susceptible to changes such as field names (normally an isolated
  /// change).
  /// </summary>
  public enum ImplicitFields
  {
    /// <summary>
    /// No members are serialized implicitly; all members require a suitable
    /// attribute such as [ProtoMember]. This is the recmomended mode for
    /// most scenarios.
    /// </summary>
    None,
    /// <summary>
    /// Public properties and fields are eligible for implicit serialization;
    /// this treats the public API as a contract. Ordering beings from ImplicitFirstTag.
    /// </summary>
    AllPublic,
    /// <summary>
    /// Public and non-public fields are eligible for implicit serialization;
    /// this acts as a state/implementation serializer. Ordering beings from ImplicitFirstTag.
    /// </summary>
    AllFields,
  }
}
