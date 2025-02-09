// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.CharSerializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class CharSerializer(TypeModel model) : UInt16Serializer(model)
  {
    private static readonly Type expectedType = typeof (char);

    public override Type ExpectedType => CharSerializer.expectedType;

    public override void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteUInt16((ushort) (char) value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      return (object) (char) source.ReadUInt16();
    }
  }
}
