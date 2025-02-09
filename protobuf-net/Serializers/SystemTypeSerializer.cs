// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.SystemTypeSerializer
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
  internal sealed class SystemTypeSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (Type);

    public SystemTypeSerializer(TypeModel model)
    {
    }

    public Type ExpectedType => SystemTypeSerializer.expectedType;

    void IProtoSerializer.Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteType((Type) value, dest);
    }

    object IProtoSerializer.Read(object value, ProtoReader source) => (object) source.ReadType();

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteType", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead("ReadType", this.ExpectedType);
    }
  }
}
