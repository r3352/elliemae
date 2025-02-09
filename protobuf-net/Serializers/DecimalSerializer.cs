// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.DecimalSerializer
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
  internal sealed class DecimalSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (Decimal);

    public DecimalSerializer(TypeModel model)
    {
    }

    public Type ExpectedType => DecimalSerializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source) => (object) BclHelpers.ReadDecimal(source);

    public void Write(object value, ProtoWriter dest)
    {
      BclHelpers.WriteDecimal((Decimal) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), "WriteDecimal", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), "ReadDecimal", this.ExpectedType);
    }
  }
}
