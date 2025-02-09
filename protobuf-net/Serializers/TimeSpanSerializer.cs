// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.TimeSpanSerializer
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
  internal sealed class TimeSpanSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (TimeSpan);
    private readonly bool wellKnown;

    public TimeSpanSerializer(DataFormat dataFormat, TypeModel model)
    {
      this.wellKnown = dataFormat == DataFormat.WellKnown;
    }

    public Type ExpectedType => TimeSpanSerializer.expectedType;

    bool IProtoSerializer.RequiresOldValue => false;

    bool IProtoSerializer.ReturnsValue => true;

    public object Read(object value, ProtoReader source)
    {
      return this.wellKnown ? (object) BclHelpers.ReadDuration(source) : (object) BclHelpers.ReadTimeSpan(source);
    }

    public void Write(object value, ProtoWriter dest)
    {
      if (this.wellKnown)
        BclHelpers.WriteDuration((TimeSpan) value, dest);
      else
        BclHelpers.WriteTimeSpan((TimeSpan) value, dest);
    }

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitWrite(ctx.MapType(typeof (BclHelpers)), this.wellKnown ? "WriteDuration" : "WriteTimeSpan", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.wellKnown)
        ctx.LoadValue(valueFrom);
      ctx.EmitBasicRead(ctx.MapType(typeof (BclHelpers)), this.wellKnown ? "ReadDuration" : "ReadTimeSpan", this.ExpectedType);
    }
  }
}
