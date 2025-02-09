// Decompiled with JetBrains decompiler
// Type: ProtoBuf.DiscriminatedUnionObject
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

#nullable disable
namespace ProtoBuf
{
  /// <summary>Represent multiple types as a union; this is used as part of OneOf -
  /// note that it is the caller's responsbility to only read/write the value as the same type</summary>
  /// <summary>Create a new discriminated union value</summary>
  public readonly struct DiscriminatedUnionObject(int discriminator, object value)
  {
    private readonly int _discriminator = discriminator;
    /// <summary>The value typed as Object</summary>
    public readonly object Object = value;

    /// <summary>Indicates whether the specified discriminator is assigned</summary>
    public bool Is(int discriminator) => this._discriminator == discriminator;

    /// <summary>Reset a value if the specified discriminator is assigned</summary>
    public static void Reset(ref DiscriminatedUnionObject value, int discriminator)
    {
      if (value.Discriminator != discriminator)
        return;
      value = new DiscriminatedUnionObject();
    }

    /// <summary>The discriminator value</summary>
    public int Discriminator => this._discriminator;
  }
}
