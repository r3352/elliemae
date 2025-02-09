// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.SubType
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Serializers;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>Represents an inherited type in a type hierarchy.</summary>
  public sealed class SubType
  {
    private readonly int fieldNumber;
    private readonly MetaType derivedType;
    private readonly DataFormat dataFormat;
    private IProtoSerializer serializer;

    /// <summary>
    /// The field-number that is used to encapsulate the data (as a nested
    /// message) for the derived dype.
    /// </summary>
    public int FieldNumber => this.fieldNumber;

    /// <summary>The sub-type to be considered.</summary>
    public MetaType DerivedType => this.derivedType;

    /// <summary>Creates a new SubType instance.</summary>
    /// <param name="fieldNumber">The field-number that is used to encapsulate the data (as a nested
    /// message) for the derived dype.</param>
    /// <param name="derivedType">The sub-type to be considered.</param>
    /// <param name="format">Specific encoding style to use; in particular, Grouped can be used to avoid buffering, but is not the default.</param>
    public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
    {
      if (derivedType == null)
        throw new ArgumentNullException(nameof (derivedType));
      this.fieldNumber = fieldNumber > 0 ? fieldNumber : throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      this.derivedType = derivedType;
      this.dataFormat = format;
    }

    internal IProtoSerializer Serializer
    {
      get => this.serializer ?? (this.serializer = this.BuildSerializer());
    }

    private IProtoSerializer BuildSerializer()
    {
      WireType wireType = WireType.String;
      if (this.dataFormat == DataFormat.Group)
        wireType = WireType.StartGroup;
      IProtoSerializer tail = (IProtoSerializer) new SubItemSerializer(this.derivedType.Type, this.derivedType.GetKey(false, false), (ISerializerProxy) this.derivedType, false);
      return (IProtoSerializer) new TagDecorator(this.fieldNumber, wireType, false, tail);
    }

    internal sealed class Comparer : IComparer, IComparer<SubType>
    {
      public static readonly SubType.Comparer Default = new SubType.Comparer();

      public int Compare(object x, object y) => this.Compare(x as SubType, y as SubType);

      public int Compare(SubType x, SubType y)
      {
        if (x == y)
          return 0;
        if (x == null)
          return -1;
        return y == null ? 1 : x.FieldNumber.CompareTo(y.FieldNumber);
      }
    }
  }
}
