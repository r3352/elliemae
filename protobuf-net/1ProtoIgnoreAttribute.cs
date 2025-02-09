// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoIgnoreAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Indicates that a member should be excluded from serialization; this
  /// is only normally used when using implict fields.
  /// </summary>
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class ProtoIgnoreAttribute : Attribute
  {
  }
}
