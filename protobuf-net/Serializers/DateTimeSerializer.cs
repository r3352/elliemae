// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.DateTimeSerializer
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
  internal sealed class DateTimeSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (DateTime);
    private readonly bool includeKind;
    private readonly bool wellKnown;

    public Type ExpectedType => DateTimeSerializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public DateTimeSerializer(DataFormat dataFormat, TypeModel model)
    {
      this.wellKnown = dataFormat == DataFormat.WellKnown;
      this.includeKind = model != null && model.SerializeDateTimeKind();
    }

    public object Read(object value, ProtoReader source)
    {
      return this.wellKnown ? (object) BclHelpers.ReadTimestamp(source) : (object) BclHelpers.ReadDateTime(source);
    }

    public void Write(object value, ProtoWriter dest)
    {
      if (this.wellKnown)
        BclHelpers.WriteTimestamp((DateTime) value, dest);
      else if (this.includeKind)
        BclHelpers.WriteDateTimeWithKind((DateTime) value, dest);
      else
        BclHelpers.WriteDateTime((DateTime) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), this.wellKnown ? "WriteTimestamp" : (this.includeKind ? "WriteDateTimeWithKind" : "WriteDateTime"), valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local entity)
    {
      if (this.wellKnown)
        ctx.LoadValue(entity);
      ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), this.wellKnown ? "ReadTimestamp" : "ReadDateTime", this.ExpectedType);
    }
  }
}
