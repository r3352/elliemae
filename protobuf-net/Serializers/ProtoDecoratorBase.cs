// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.ProtoDecoratorBase
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal abstract class ProtoDecoratorBase : IProtoSerializer
  {
    protected readonly IProtoSerializer Tail;

    public abstract Type ExpectedType { get; }

    protected ProtoDecoratorBase(IProtoSerializer tail) => this.Tail = tail;

    public abstract bool ReturnsValue { get; }

    public abstract bool RequiresOldValue { get; }

    public abstract void Write(object value, ProtoWriter dest);

    public abstract object Read(object value, ProtoReader source);

    void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      this.EmitWrite(ctx, valueFrom);
    }

    protected abstract void EmitWrite(CompilerContext ctx, Local valueFrom);

    void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
    {
      this.EmitRead(ctx, valueFrom);
    }

    protected abstract void EmitRead(CompilerContext ctx, Local valueFrom);
  }
}
