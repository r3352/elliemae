// Decompiled with JetBrains decompiler
// Type: ProtoBuf.DiscriminatedUnion128Object
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace ProtoBuf
{
  /// <summary>Represent multiple types as a union; this is used as part of OneOf -
  /// note that it is the caller's responsbility to only read/write the value as the same type</summary>
  [StructLayout(LayoutKind.Explicit)]
  public readonly struct DiscriminatedUnion128Object
  {
    [FieldOffset(0)]
    private readonly int _discriminator;
    /// <summary>The value typed as Int64</summary>
    [FieldOffset(8)]
    public readonly long Int64;
    /// <summary>The value typed as UInt64</summary>
    [FieldOffset(8)]
    public readonly ulong UInt64;
    /// <summary>The value typed as Int32</summary>
    [FieldOffset(8)]
    public readonly int Int32;
    /// <summary>The value typed as UInt32</summary>
    [FieldOffset(8)]
    public readonly uint UInt32;
    /// <summary>The value typed as Boolean</summary>
    [FieldOffset(8)]
    public readonly bool Boolean;
    /// <summary>The value typed as Single</summary>
    [FieldOffset(8)]
    public readonly float Single;
    /// <summary>The value typed as Double</summary>
    [FieldOffset(8)]
    public readonly double Double;
    /// <summary>The value typed as DateTime</summary>
    [FieldOffset(8)]
    public readonly DateTime DateTime;
    /// <summary>The value typed as TimeSpan</summary>
    [FieldOffset(8)]
    public readonly TimeSpan TimeSpan;
    /// <summary>The value typed as Guid</summary>
    [FieldOffset(8)]
    public readonly Guid Guid;
    /// <summary>The value typed as Object</summary>
    [FieldOffset(24)]
    public readonly object Object;

    static DiscriminatedUnion128Object()
    {
      if (sizeof (DateTime) > 16)
        throw new InvalidOperationException("DateTime was unexpectedly too big for DiscriminatedUnion128Object");
      if (sizeof (TimeSpan) > 16)
        throw new InvalidOperationException("TimeSpan was unexpectedly too big for DiscriminatedUnion128Object");
      if (sizeof (Guid) > 16)
        throw new InvalidOperationException("Guid was unexpectedly too big for DiscriminatedUnion128Object");
    }

    private DiscriminatedUnion128Object(int discriminator)
      : this()
    {
      this._discriminator = discriminator;
    }

    /// <summary>Indicates whether the specified discriminator is assigned</summary>
    public bool Is(int discriminator) => this._discriminator == discriminator;

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, long value)
      : this(discriminator)
    {
      this.Int64 = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, int value)
      : this(discriminator)
    {
      this.Int32 = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, ulong value)
      : this(discriminator)
    {
      this.UInt64 = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, uint value)
      : this(discriminator)
    {
      this.UInt32 = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, float value)
      : this(discriminator)
    {
      this.Single = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, double value)
      : this(discriminator)
    {
      this.Double = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, bool value)
      : this(discriminator)
    {
      this.Boolean = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, object value)
      : this(value != null ? discriminator : 0)
    {
      this.Object = value;
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, DateTime? value)
      : this(value.HasValue ? discriminator : 0)
    {
      this.DateTime = value.GetValueOrDefault();
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, TimeSpan? value)
      : this(value.HasValue ? discriminator : 0)
    {
      this.TimeSpan = value.GetValueOrDefault();
    }

    /// <summary>Create a new discriminated union value</summary>
    public DiscriminatedUnion128Object(int discriminator, Guid? value)
      : this(value.HasValue ? discriminator : 0)
    {
      this.Guid = value.GetValueOrDefault();
    }

    /// <summary>Reset a value if the specified discriminator is assigned</summary>
    public static void Reset(ref DiscriminatedUnion128Object value, int discriminator)
    {
      if (value.Discriminator != discriminator)
        return;
      value = new DiscriminatedUnion128Object();
    }

    /// <summary>The discriminator value</summary>
    public int Discriminator => this._discriminator;
  }
}
