// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Compiler.Local
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace ProtoBuf.Compiler
{
  internal sealed class Local : IDisposable
  {
    private LocalBuilder value;
    private readonly Type type;
    private CompilerContext ctx;

    private Local(LocalBuilder value, Type type)
    {
      this.value = value;
      this.type = type;
    }

    internal Local(CompilerContext ctx, Type type)
    {
      this.ctx = ctx;
      if (ctx != null)
        this.value = ctx.GetFromPool(type);
      this.type = type;
    }

    internal LocalBuilder Value
    {
      get => this.value ?? throw new ObjectDisposedException(this.GetType().Name);
    }

    public Type Type => this.type;

    public Local AsCopy() => this.ctx == null ? this : new Local(this.value, this.type);

    public void Dispose()
    {
      if (this.ctx == null)
        return;
      this.ctx.ReleaseToPool(this.value);
      this.value = (LocalBuilder) null;
      this.ctx = (CompilerContext) null;
    }

    internal bool IsSame(Local other)
    {
      if (this == other)
        return true;
      object obj = (object) this.value;
      return other != null && obj == other.value;
    }
  }
}
