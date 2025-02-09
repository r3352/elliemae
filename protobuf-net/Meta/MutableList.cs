// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.MutableList
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf.Meta
{
  internal sealed class MutableList : BasicList
  {
    public new object this[int index]
    {
      get => this.head[index];
      set => this.head[index] = value;
    }

    public void RemoveLast() => this.head.RemoveLastWithMutate();

    public void Clear() => this.head.Clear();
  }
}
