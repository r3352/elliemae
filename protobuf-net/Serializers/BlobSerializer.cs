// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.BlobSerializer
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
  internal sealed class BlobSerializer : IProtoSerializer
  {
    private static readonly Type expectedType = typeof (byte[]);
    private readonly bool overwriteList;

    public Type ExpectedType => BlobSerializer.expectedType;

    public BlobSerializer(TypeModel model, bool overwriteList)
    {
      this.overwriteList = overwriteList;
    }

    public object Read(object value, ProtoReader source)
    {
      return (object) ProtoReader.AppendBytes(this.overwriteList ? (byte[]) null : (byte[]) value, source);
    }

    public void Write(object value, ProtoWriter dest)
    {
      ProtoWriter.WriteBytes((byte[]) value, dest);
    }

    bool IProtoSerializer.RequiresOldValue => !this.overwriteList;

    bool IProtoSerializer.ReturnsValue => true;

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.EmitBasicWrite("WriteBytes", valueFrom);
    }

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.overwriteList)
        ctx.LoadNullRef();
      else
        ctx.LoadValue(valueFrom);
      ctx.LoadReaderWriter();
      ctx.EmitCall(ctx.MapType(typeof (ProtoReader)).GetMethod("AppendBytes"));
    }
  }
}
