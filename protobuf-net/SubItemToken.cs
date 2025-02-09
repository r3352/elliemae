// Decompiled with JetBrains decompiler
// Type: ProtoBuf.SubItemToken
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Used to hold particulars relating to nested objects. This is opaque to the caller - simply
  /// give back the token you are given at the end of an object.
  /// </summary>
  public readonly struct SubItemToken
  {
    internal readonly long value64;

    internal SubItemToken(int value) => this.value64 = (long) value;

    internal SubItemToken(long value) => this.value64 = value;
  }
}
