// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.UriDecorator
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
  internal sealed class UriDecorator(TypeModel model, IProtoSerializer tail) : ProtoDecoratorBase(tail)
  {
    private static readonly Type expectedType = typeof (Uri);

    public override Type ExpectedType => UriDecorator.expectedType;

    public override bool RequiresOldValue => false;

    public override bool ReturnsValue => true;

    public override void Write(object value, ProtoWriter dest)
    {
      this.Tail.Write((object) ((Uri) value).OriginalString, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      string uriString = (string) this.Tail.Read((object) null, source);
      return uriString.Length != 0 ? (object) new Uri(uriString, UriKind.RelativeOrAbsolute) : (object) null;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      ctx.LoadValue(valueFrom);
      ctx.LoadValue(typeof (Uri).GetProperty("OriginalString"));
      this.Tail.EmitWrite(ctx, (Local) null);
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      this.Tail.EmitRead(ctx, valueFrom);
      ctx.CopyValue();
      CodeLabel label1 = ctx.DefineLabel();
      CodeLabel label2 = ctx.DefineLabel();
      ctx.LoadValue(typeof (string).GetProperty("Length"));
      ctx.BranchIfTrue(label1, true);
      ctx.DiscardValue();
      ctx.LoadNullRef();
      ctx.Branch(label2, true);
      ctx.MarkLabel(label1);
      ctx.LoadValue(0);
      ctx.EmitCtor(ctx.MapType(typeof (Uri)), ctx.MapType(typeof (string)), ctx.MapType(typeof (UriKind)));
      ctx.MarkLabel(label2);
    }
  }
}
