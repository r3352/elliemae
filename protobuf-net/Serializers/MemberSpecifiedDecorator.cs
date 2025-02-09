// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.MemberSpecifiedDecorator
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using System;
using System.Reflection;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal sealed class MemberSpecifiedDecorator : ProtoDecoratorBase
  {
    private readonly MethodInfo getSpecified;
    private readonly MethodInfo setSpecified;

    public override Type ExpectedType => this.Tail.ExpectedType;

    public override bool RequiresOldValue => this.Tail.RequiresOldValue;

    public override bool ReturnsValue => this.Tail.ReturnsValue;

    public MemberSpecifiedDecorator(
      MethodInfo getSpecified,
      MethodInfo setSpecified,
      IProtoSerializer tail)
      : base(tail)
    {
      this.getSpecified = !(getSpecified == (MethodInfo) null) || !(setSpecified == (MethodInfo) null) ? getSpecified : throw new InvalidOperationException();
      this.setSpecified = setSpecified;
    }

    public override void Write(object value, ProtoWriter dest)
    {
      if (!(this.getSpecified == (MethodInfo) null) && !(bool) this.getSpecified.Invoke(value, (object[]) null))
        return;
      this.Tail.Write(value, dest);
    }

    public override object Read(object value, ProtoReader source)
    {
      object obj = this.Tail.Read(value, source);
      if (this.setSpecified != (MethodInfo) null)
        this.setSpecified.Invoke(value, new object[1]
        {
          (object) true
        });
      return obj;
    }

    protected override void EmitWrite(CompilerContext ctx, Local valueFrom)
    {
      if (this.getSpecified == (MethodInfo) null)
      {
        this.Tail.EmitWrite(ctx, valueFrom);
      }
      else
      {
        using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
        {
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.EmitCall(this.getSpecified);
          CodeLabel label = ctx.DefineLabel();
          ctx.BranchIfFalse(label, false);
          this.Tail.EmitWrite(ctx, localWithValue);
          ctx.MarkLabel(label);
        }
      }
    }

    protected override void EmitRead(CompilerContext ctx, Local valueFrom)
    {
      if (this.setSpecified == (MethodInfo) null)
      {
        this.Tail.EmitRead(ctx, valueFrom);
      }
      else
      {
        using (Local localWithValue = ctx.GetLocalWithValue(this.ExpectedType, valueFrom))
        {
          this.Tail.EmitRead(ctx, localWithValue);
          ctx.LoadAddress(localWithValue, this.ExpectedType);
          ctx.LoadValue(1);
          ctx.EmitCall(this.setSpecified);
        }
      }
    }
  }
}
