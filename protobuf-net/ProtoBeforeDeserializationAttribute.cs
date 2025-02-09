// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoBeforeDeserializationAttribute
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.ComponentModel;

#nullable disable
namespace ProtoBuf
{
  /// <summary>Specifies a method on the root-contract in an hierarchy to be invoked before deserialization.</summary>
  [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
  [ImmutableObject(true)]
  public sealed class ProtoBeforeDeserializationAttribute : Attribute
  {
  }
}
