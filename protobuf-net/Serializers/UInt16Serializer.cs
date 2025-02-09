// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.UInt16Serializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using ProtoBuf.Meta;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal class UInt16Serializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (ushort);

    public UInt16Serializer(TypeModel model)
    {
    }

    public virtual Type ExpectedType => UInt16Serializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public virtual object Read(object value, ProtoReader source) => (object) source.ReadUInt16();

    public virtual void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteUInt16((ushort) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteUInt16", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadUInt16", ctx.MapType(typeof (ushort)));
    }
  }
}
