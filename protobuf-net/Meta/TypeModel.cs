// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Meta.TypeModel
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf.Meta
{
  /// <summary>
  /// Provides protobuf serialization support for a number of types
  /// </summary>
  public abstract class TypeModel
  {
    private static readonly Type ilist = typeof (IList);
    private readonly Dictionary<Type, TypeModel.KnownTypeKey> knownKeys = new Dictionary<Type, TypeModel.KnownTypeKey>();

    /// <summary>
    /// Should the <c>Kind</c> be included on date/time values?
    /// </summary>
    protected internal virtual bool SerializeDateTimeKind() => false;

    /// <summary>Resolve a System.Type to the compiler-specific type</summary>
    protected internal Type MapType(Type type) => this.MapType(type, true);

    /// <summary>Resolve a System.Type to the compiler-specific type</summary>
    protected internal virtual Type MapType(Type type, bool demand) => type;

    private WireType GetWireType(
      ProtoTypeCode code,
      DataFormat format,
      ref Type type,
      out int modelKey)
    {
      modelKey = -1;
      if (Helpers.IsEnum(type))
      {
        modelKey = this.GetKey(ref type);
        return WireType.Variant;
      }
      switch (code)
      {
        case ProtoTypeCode.Boolean:
        case ProtoTypeCode.Char:
        case ProtoTypeCode.SByte:
        case ProtoTypeCode.Byte:
        case ProtoTypeCode.Int16:
        case ProtoTypeCode.UInt16:
        case ProtoTypeCode.Int32:
        case ProtoTypeCode.UInt32:
          return format != DataFormat.FixedSize ? WireType.Variant : WireType.Fixed32;
        case ProtoTypeCode.Int64:
        case ProtoTypeCode.UInt64:
          return format != DataFormat.FixedSize ? WireType.Variant : WireType.Fixed64;
        case ProtoTypeCode.Single:
          return WireType.Fixed32;
        case ProtoTypeCode.Double:
          return WireType.Fixed64;
        case ProtoTypeCode.Decimal:
        case ProtoTypeCode.DateTime:
        case ProtoTypeCode.String:
        case ProtoTypeCode.TimeSpan:
        case ProtoTypeCode.ByteArray:
        case ProtoTypeCode.Guid:
        case ProtoTypeCode.Uri:
          return WireType.String;
        default:
          return (modelKey = this.GetKey(ref type)) >= 0 ? WireType.String : WireType.None;
      }
    }

    /// <summary>
    /// This is the more "complete" version of Serialize, which handles single instances of mapped types.
    /// The value is written as a complete field, including field-header and (for sub-objects) a
    /// length-prefix
    /// In addition to that, this provides support for:
    ///  - basic values; individual int / string / Guid / etc
    ///  - IEnumerable sequences of any type handled by TrySerializeAuxiliaryType
    /// 
    /// </summary>
    internal bool TrySerializeAuxiliaryType(
      ProtoWriter writer,
      Type type,
      DataFormat format,
      int tag,
      object value,
      bool isInsideList,
      object parentList)
    {
      if (type == (Type) null)
        type = value.GetType();
      ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
      int modelKey;
      WireType wireType = this.GetWireType(typeCode, format, ref type, out modelKey);
      if (modelKey >= 0)
      {
        if (Helpers.IsEnum(type))
        {
          this.Serialize(modelKey, value, writer);
          return true;
        }
        ProtoWriter.WriteFieldHeader(tag, wireType, writer);
        switch (wireType)
        {
          case WireType.None:
            throw ProtoWriter.CreateException(writer);
          case WireType.String:
          case WireType.StartGroup:
            SubItemToken token = ProtoWriter.StartSubItem(value, writer);
            this.Serialize(modelKey, value, writer);
            ProtoWriter.EndSubItem(token, writer);
            return true;
          default:
            this.Serialize(modelKey, value, writer);
            return true;
        }
      }
      else
      {
        if (wireType != WireType.None)
          ProtoWriter.WriteFieldHeader(tag, wireType, writer);
        switch (typeCode)
        {
          case ProtoTypeCode.Boolean:
            ProtoWriter.WriteBoolean((bool) value, writer);
            return true;
          case ProtoTypeCode.Char:
            ProtoWriter.WriteUInt16((ushort) (char) value, writer);
            return true;
          case ProtoTypeCode.SByte:
            ProtoWriter.WriteSByte((sbyte) value, writer);
            return true;
          case ProtoTypeCode.Byte:
            ProtoWriter.WriteByte((byte) value, writer);
            return true;
          case ProtoTypeCode.Int16:
            ProtoWriter.WriteInt16((short) value, writer);
            return true;
          case ProtoTypeCode.UInt16:
            ProtoWriter.WriteUInt16((ushort) value, writer);
            return true;
          case ProtoTypeCode.Int32:
            ProtoWriter.WriteInt32((int) value, writer);
            return true;
          case ProtoTypeCode.UInt32:
            ProtoWriter.WriteUInt32((uint) value, writer);
            return true;
          case ProtoTypeCode.Int64:
            ProtoWriter.WriteInt64((long) value, writer);
            return true;
          case ProtoTypeCode.UInt64:
            ProtoWriter.WriteUInt64((ulong) value, writer);
            return true;
          case ProtoTypeCode.Single:
            ProtoWriter.WriteSingle((float) value, writer);
            return true;
          case ProtoTypeCode.Double:
            ProtoWriter.WriteDouble((double) value, writer);
            return true;
          case ProtoTypeCode.Decimal:
            BclHelpers.WriteDecimal((Decimal) value, writer);
            return true;
          case ProtoTypeCode.DateTime:
            if (this.SerializeDateTimeKind())
              BclHelpers.WriteDateTimeWithKind((DateTime) value, writer);
            else
              BclHelpers.WriteDateTime((DateTime) value, writer);
            return true;
          case ProtoTypeCode.String:
            ProtoWriter.WriteString((string) value, writer);
            return true;
          case ProtoTypeCode.TimeSpan:
            BclHelpers.WriteTimeSpan((TimeSpan) value, writer);
            return true;
          case ProtoTypeCode.ByteArray:
            ProtoWriter.WriteBytes((byte[]) value, writer);
            return true;
          case ProtoTypeCode.Guid:
            BclHelpers.WriteGuid((Guid) value, writer);
            return true;
          case ProtoTypeCode.Uri:
            ProtoWriter.WriteString(((Uri) value).OriginalString, writer);
            return true;
          default:
            if (!(value is IEnumerable parentList1))
              return false;
            if (isInsideList)
              throw TypeModel.CreateNestedListsNotSupported(parentList?.GetType());
            foreach (object obj in parentList1)
            {
              if (obj == null)
                throw new NullReferenceException();
              if (!this.TrySerializeAuxiliaryType(writer, (Type) null, format, tag, obj, true, (object) parentList1))
                TypeModel.ThrowUnexpectedType(obj.GetType());
            }
            return true;
        }
      }
    }

    private void SerializeCore(ProtoWriter writer, object value)
    {
      Type type = value != null ? value.GetType() : throw new ArgumentNullException(nameof (value));
      int key = this.GetKey(ref type);
      if (key >= 0)
      {
        this.Serialize(key, value, writer);
      }
      else
      {
        if (this.TrySerializeAuxiliaryType(writer, type, DataFormat.Default, 1, value, false, (object) null))
          return;
        TypeModel.ThrowUnexpectedType(type);
      }
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream.
    /// </summary>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="dest">The destination stream to write to.</param>
    public void Serialize(Stream dest, object value)
    {
      this.Serialize(dest, value, (SerializationContext) null);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream.
    /// </summary>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="dest">The destination stream to write to.</param>
    /// <param name="context">Additional information about this serialization operation.</param>
    public void Serialize(Stream dest, object value, SerializationContext context)
    {
      using (ProtoWriter writer = ProtoWriter.Create(dest, this, context))
      {
        writer.SetRootObject(value);
        this.SerializeCore(writer, value);
        writer.Close();
      }
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied writer.
    /// </summary>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="dest">The destination writer to write to.</param>
    public void Serialize(ProtoWriter dest, object value)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      dest.CheckDepthFlushlock();
      dest.SetRootObject(value);
      this.SerializeCore(dest, value);
      dest.CheckDepthFlushlock();
      ProtoWriter.Flush(dest);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (or null), using length-prefixed
    /// data - useful with network IO.
    /// </summary>
    /// <param name="type">The type being merged.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="fieldNumber">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, fieldNumber, (Serializer.TypeResolver) null, out long _);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (or null), using length-prefixed
    /// data - useful with network IO.
    /// </summary>
    /// <param name="type">The type being merged.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="expectedField">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    /// <param name="resolver">Used to resolve types on a per-field basis.</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out long _);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (or null), using length-prefixed
    /// data - useful with network IO.
    /// </summary>
    /// <param name="type">The type being merged.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="expectedField">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    /// <param name="resolver">Used to resolve types on a per-field basis.</param>
    /// <param name="bytesRead">Returns the number of bytes consumed by this operation (includes length-prefix overheads and any skipped data).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      out int bytesRead)
    {
      long bytesRead1;
      object obj = this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out bytesRead1, out bool _, (SerializationContext) null);
      bytesRead = checked ((int) bytesRead1);
      return obj;
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (or null), using length-prefixed
    /// data - useful with network IO.
    /// </summary>
    /// <param name="type">The type being merged.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="expectedField">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    /// <param name="resolver">Used to resolve types on a per-field basis.</param>
    /// <param name="bytesRead">Returns the number of bytes consumed by this operation (includes length-prefix overheads and any skipped data).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      out long bytesRead)
    {
      return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out bytesRead, out bool _, (SerializationContext) null);
    }

    private object DeserializeWithLengthPrefix(
      Stream source,
      object value,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      out long bytesRead,
      out bool haveObject,
      SerializationContext context)
    {
      haveObject = false;
      bytesRead = 0L;
      if (type == (Type) null && (style != PrefixStyle.Base128 || resolver == null))
        throw new InvalidOperationException("A type must be provided unless base-128 prefixing is being used in combination with a resolver");
      long num;
      bool flag;
      do
      {
        bool expectHeader = expectedField > 0 || resolver != null;
        int fieldNumber;
        int bytesRead1;
        num = ProtoReader.ReadLongLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead1);
        if (bytesRead1 == 0)
          return value;
        bytesRead += (long) bytesRead1;
        if (num < 0L)
          return value;
        if (style == PrefixStyle.Base128)
        {
          if (expectHeader && expectedField == 0 && type == (Type) null && resolver != null)
          {
            type = resolver(fieldNumber);
            flag = type == (Type) null;
          }
          else
            flag = expectedField != fieldNumber;
        }
        else
          flag = false;
        if (flag)
        {
          if (num == long.MaxValue)
            throw new InvalidOperationException();
          ProtoReader.Seek(source, num, (byte[]) null);
          bytesRead += num;
        }
      }
      while (flag);
      ProtoReader protoReader = (ProtoReader) null;
      try
      {
        protoReader = ProtoReader.Create(source, this, context, num);
        int key = this.GetKey(ref type);
        if (key >= 0 && !Helpers.IsEnum(type))
          value = this.Deserialize(key, value, protoReader);
        else if (!this.TryDeserializeAuxiliaryType(protoReader, DataFormat.Default, 1, type, ref value, true, false, true, false, (object) null) && num != 0L)
          TypeModel.ThrowUnexpectedType(type);
        bytesRead += protoReader.LongPosition;
        haveObject = true;
        return value;
      }
      finally
      {
        ProtoReader.Recycle(protoReader);
      }
    }

    /// <summary>
    /// Reads a sequence of consecutive length-prefixed items from a stream, using
    /// either base-128 or fixed-length prefixes. Base-128 prefixes with a tag
    /// are directly comparable to serializing multiple items in succession
    /// (use the <see cref="F:ProtoBuf.Serializer.ListItemTag" /> tag to emulate the implicit behavior
    /// when serializing a list/array). When a tag is
    /// specified, any records with different tags are silently omitted. The
    /// tag is ignored. The tag is ignores for fixed-length prefixes.
    /// </summary>
    /// <param name="source">The binary stream containing the serialized records.</param>
    /// <param name="style">The prefix style used in the data.</param>
    /// <param name="expectedField">The tag of records to return (if non-positive, then no tag is
    /// expected and all records are returned).</param>
    /// <param name="resolver">On a field-by-field basis, the type of object to deserialize (can be null if "type" is specified). </param>
    /// <param name="type">The type of object to deserialize (can be null if "resolver" is specified).</param>
    /// <returns>The sequence of deserialized objects.</returns>
    public IEnumerable DeserializeItems(
      Stream source,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver)
    {
      return this.DeserializeItems(source, type, style, expectedField, resolver, (SerializationContext) null);
    }

    /// <summary>
    /// Reads a sequence of consecutive length-prefixed items from a stream, using
    /// either base-128 or fixed-length prefixes. Base-128 prefixes with a tag
    /// are directly comparable to serializing multiple items in succession
    /// (use the <see cref="F:ProtoBuf.Serializer.ListItemTag" /> tag to emulate the implicit behavior
    /// when serializing a list/array). When a tag is
    /// specified, any records with different tags are silently omitted. The
    /// tag is ignored. The tag is ignores for fixed-length prefixes.
    /// </summary>
    /// <param name="source">The binary stream containing the serialized records.</param>
    /// <param name="style">The prefix style used in the data.</param>
    /// <param name="expectedField">The tag of records to return (if non-positive, then no tag is
    /// expected and all records are returned).</param>
    /// <param name="resolver">On a field-by-field basis, the type of object to deserialize (can be null if "type" is specified). </param>
    /// <param name="type">The type of object to deserialize (can be null if "resolver" is specified).</param>
    /// <returns>The sequence of deserialized objects.</returns>
    /// <param name="context">Additional information about this serialization operation.</param>
    public IEnumerable DeserializeItems(
      Stream source,
      Type type,
      PrefixStyle style,
      int expectedField,
      Serializer.TypeResolver resolver,
      SerializationContext context)
    {
      return (IEnumerable) new TypeModel.DeserializeItemsIterator(this, source, type, style, expectedField, resolver, context);
    }

    /// <summary>
    /// Reads a sequence of consecutive length-prefixed items from a stream, using
    /// either base-128 or fixed-length prefixes. Base-128 prefixes with a tag
    /// are directly comparable to serializing multiple items in succession
    /// (use the <see cref="F:ProtoBuf.Serializer.ListItemTag" /> tag to emulate the implicit behavior
    /// when serializing a list/array). When a tag is
    /// specified, any records with different tags are silently omitted. The
    /// tag is ignored. The tag is ignores for fixed-length prefixes.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="source">The binary stream containing the serialized records.</param>
    /// <param name="style">The prefix style used in the data.</param>
    /// <param name="expectedField">The tag of records to return (if non-positive, then no tag is
    /// expected and all records are returned).</param>
    /// <returns>The sequence of deserialized objects.</returns>
    public IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int expectedField)
    {
      return this.DeserializeItems<T>(source, style, expectedField, (SerializationContext) null);
    }

    /// <summary>
    /// Reads a sequence of consecutive length-prefixed items from a stream, using
    /// either base-128 or fixed-length prefixes. Base-128 prefixes with a tag
    /// are directly comparable to serializing multiple items in succession
    /// (use the <see cref="F:ProtoBuf.Serializer.ListItemTag" /> tag to emulate the implicit behavior
    /// when serializing a list/array). When a tag is
    /// specified, any records with different tags are silently omitted. The
    /// tag is ignored. The tag is ignores for fixed-length prefixes.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="source">The binary stream containing the serialized records.</param>
    /// <param name="style">The prefix style used in the data.</param>
    /// <param name="expectedField">The tag of records to return (if non-positive, then no tag is
    /// expected and all records are returned).</param>
    /// <returns>The sequence of deserialized objects.</returns>
    /// <param name="context">Additional information about this serialization operation.</param>
    public IEnumerable<T> DeserializeItems<T>(
      Stream source,
      PrefixStyle style,
      int expectedField,
      SerializationContext context)
    {
      return (IEnumerable<T>) new TypeModel.DeserializeItemsIterator<T>(this, source, style, expectedField, context);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream,
    /// with a length-prefix. This is useful for socket programming,
    /// as DeserializeWithLengthPrefix can be used to read the single object back
    /// from an ongoing stream.
    /// </summary>
    /// <param name="type">The type being serialized.</param>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="dest">The destination stream to write to.</param>
    /// <param name="fieldNumber">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    public void SerializeWithLengthPrefix(
      Stream dest,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber)
    {
      this.SerializeWithLengthPrefix(dest, value, type, style, fieldNumber, (SerializationContext) null);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream,
    /// with a length-prefix. This is useful for socket programming,
    /// as DeserializeWithLengthPrefix can be used to read the single object back
    /// from an ongoing stream.
    /// </summary>
    /// <param name="type">The type being serialized.</param>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="dest">The destination stream to write to.</param>
    /// <param name="fieldNumber">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    /// <param name="context">Additional information about this serialization operation.</param>
    public void SerializeWithLengthPrefix(
      Stream dest,
      object value,
      Type type,
      PrefixStyle style,
      int fieldNumber,
      SerializationContext context)
    {
      if (type == (Type) null)
        type = value != null ? this.MapType(value.GetType()) : throw new ArgumentNullException(nameof (value));
      int key = this.GetKey(ref type);
      using (ProtoWriter protoWriter = ProtoWriter.Create(dest, this, context))
      {
        switch (style)
        {
          case PrefixStyle.None:
            this.Serialize(key, value, protoWriter);
            break;
          case PrefixStyle.Base128:
          case PrefixStyle.Fixed32:
          case PrefixStyle.Fixed32BigEndian:
            ProtoWriter.WriteObject(value, key, protoWriter, style, fieldNumber);
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (style));
        }
        protoWriter.Close();
      }
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object Deserialize(Stream source, object value, Type type)
    {
      return this.Deserialize(source, value, type, (SerializationContext) null);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    /// <param name="context">Additional information about this serialization operation.</param>
    public object Deserialize(
      Stream source,
      object value,
      Type type,
      SerializationContext context)
    {
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      ProtoReader reader = (ProtoReader) null;
      try
      {
        reader = ProtoReader.Create(source, this, context);
        if (value != null)
          reader.SetRootObject(value);
        object obj = this.DeserializeCore(reader, type, value, noAutoCreate);
        reader.CheckFullyConsumed();
        return obj;
      }
      finally
      {
        ProtoReader.Recycle(reader);
      }
    }

    private bool PrepareDeserialize(object value, ref Type type)
    {
      if (type == (Type) null)
        type = value != null ? this.MapType(value.GetType()) : throw new ArgumentNullException(nameof (type));
      bool flag = true;
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
      {
        type = underlyingType;
        flag = false;
      }
      return flag;
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="length">The number of bytes to consume.</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object Deserialize(Stream source, object value, Type type, int length)
    {
      return this.Deserialize(source, value, type, length, (SerializationContext) null);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="length">The number of bytes to consume.</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object Deserialize(Stream source, object value, Type type, long length)
    {
      return this.Deserialize(source, value, type, length, (SerializationContext) null);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="length">The number of bytes to consume (or -1 to read to the end of the stream).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    /// <param name="context">Additional information about this serialization operation.</param>
    public object Deserialize(
      Stream source,
      object value,
      Type type,
      int length,
      SerializationContext context)
    {
      return this.Deserialize(source, value, type, length == int.MaxValue ? long.MaxValue : (long) length, context);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="length">The number of bytes to consume (or -1 to read to the end of the stream).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    /// <param name="context">Additional information about this serialization operation.</param>
    public object Deserialize(
      Stream source,
      object value,
      Type type,
      long length,
      SerializationContext context)
    {
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      ProtoReader reader = (ProtoReader) null;
      try
      {
        reader = ProtoReader.Create(source, this, context, length);
        if (value != null)
          reader.SetRootObject(value);
        object obj = this.DeserializeCore(reader, type, value, noAutoCreate);
        reader.CheckFullyConsumed();
        return obj;
      }
      finally
      {
        ProtoReader.Recycle(reader);
      }
    }

    /// <summary>
    /// Applies a protocol-buffer reader to an existing instance (which may be null).
    /// </summary>
    /// <param name="type">The type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The reader to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public object Deserialize(ProtoReader source, object value, Type type)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      bool noAutoCreate = this.PrepareDeserialize(value, ref type);
      if (value != null)
        source.SetRootObject(value);
      object obj = this.DeserializeCore(source, type, value, noAutoCreate);
      source.CheckFullyConsumed();
      return obj;
    }

    private object DeserializeCore(ProtoReader reader, Type type, object value, bool noAutoCreate)
    {
      int key = this.GetKey(ref type);
      if (key >= 0 && !Helpers.IsEnum(type))
        return this.Deserialize(key, value, reader);
      this.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, noAutoCreate, false, (object) null);
      return value;
    }

    internal static MethodInfo ResolveListAdd(
      TypeModel model,
      Type listType,
      Type itemType,
      out bool isList)
    {
      Type type = listType;
      isList = model.MapType(TypeModel.ilist).IsAssignableFrom(type);
      Type[] types = new Type[1]{ itemType };
      MethodInfo instanceMethod = Helpers.GetInstanceMethod(type, "Add", types);
      if (instanceMethod == (MethodInfo) null)
      {
        bool flag = type.IsInterface && model.MapType(typeof (IEnumerable<>)).MakeGenericType(types).IsAssignableFrom(type);
        Type declaringType = model.MapType(typeof (ICollection<>)).MakeGenericType(types);
        if (flag || declaringType.IsAssignableFrom(type))
          instanceMethod = Helpers.GetInstanceMethod(declaringType, "Add", types);
      }
      if (instanceMethod == (MethodInfo) null)
      {
        foreach (Type declaringType in type.GetInterfaces())
        {
          if (declaringType.Name == "IProducerConsumerCollection`1" && declaringType.IsGenericType && declaringType.GetGenericTypeDefinition().FullName == "System.Collections.Concurrent.IProducerConsumerCollection`1")
          {
            instanceMethod = Helpers.GetInstanceMethod(declaringType, "TryAdd", types);
            if (instanceMethod != (MethodInfo) null)
              break;
          }
        }
      }
      if (instanceMethod == (MethodInfo) null)
      {
        types[0] = model.MapType(typeof (object));
        instanceMethod = Helpers.GetInstanceMethod(type, "Add", types);
      }
      if (instanceMethod == (MethodInfo) null & isList)
        instanceMethod = Helpers.GetInstanceMethod(model.MapType(TypeModel.ilist), "Add", types);
      return instanceMethod;
    }

    internal static Type GetListItemType(TypeModel model, Type listType)
    {
      if (listType == model.MapType(typeof (string)) || listType.IsArray || !model.MapType(typeof (IEnumerable)).IsAssignableFrom(listType))
        return (Type) null;
      BasicList candidates = new BasicList();
      foreach (MethodInfo method in listType.GetMethods())
      {
        if (!method.IsStatic && !(method.Name != "Add"))
        {
          ParameterInfo[] parameters = method.GetParameters();
          Type parameterType;
          if (parameters.Length == 1 && !candidates.Contains((object) (parameterType = parameters[0].ParameterType)))
            candidates.Add((object) parameterType);
        }
      }
      string name = listType.Name;
      if (name == null || name.IndexOf("Queue") < 0 && name.IndexOf("Stack") < 0)
      {
        TypeModel.TestEnumerableListPatterns(model, candidates, listType);
        foreach (Type iType in listType.GetInterfaces())
          TypeModel.TestEnumerableListPatterns(model, candidates, iType);
      }
      foreach (PropertyInfo property in listType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
      {
        if (!(property.Name != "Item") && !candidates.Contains((object) property.PropertyType))
        {
          ParameterInfo[] indexParameters = property.GetIndexParameters();
          if (indexParameters.Length == 1 && !(indexParameters[0].ParameterType != model.MapType(typeof (int))))
            candidates.Add((object) property.PropertyType);
        }
      }
      switch (candidates.Count)
      {
        case 0:
          return (Type) null;
        case 1:
          return (Type) candidates[0] == listType ? (Type) null : (Type) candidates[0];
        case 2:
          if ((Type) candidates[0] != listType && TypeModel.CheckDictionaryAccessors(model, (Type) candidates[0], (Type) candidates[1]))
            return (Type) candidates[0];
          if ((Type) candidates[1] != listType && TypeModel.CheckDictionaryAccessors(model, (Type) candidates[1], (Type) candidates[0]))
            return (Type) candidates[1];
          break;
      }
      return (Type) null;
    }

    private static void TestEnumerableListPatterns(
      TypeModel model,
      BasicList candidates,
      Type iType)
    {
      if (!iType.IsGenericType)
        return;
      Type genericTypeDefinition = iType.GetGenericTypeDefinition();
      if (!(genericTypeDefinition == model.MapType(typeof (IEnumerable<>))) && !(genericTypeDefinition == model.MapType(typeof (ICollection<>))) && !(genericTypeDefinition.FullName == "System.Collections.Concurrent.IProducerConsumerCollection`1"))
        return;
      Type[] genericArguments = iType.GetGenericArguments();
      if (candidates.Contains((object) genericArguments[0]))
        return;
      candidates.Add((object) genericArguments[0]);
    }

    private static bool CheckDictionaryAccessors(TypeModel model, Type pair, Type value)
    {
      return pair.IsGenericType && pair.GetGenericTypeDefinition() == model.MapType(typeof (KeyValuePair<,>)) && pair.GetGenericArguments()[1] == value;
    }

    private bool TryDeserializeList(
      TypeModel model,
      ProtoReader reader,
      DataFormat format,
      int tag,
      Type listType,
      Type itemType,
      ref object value)
    {
      bool isList;
      MethodInfo methodInfo = TypeModel.ResolveListAdd(model, listType, itemType, out isList);
      if (methodInfo == (MethodInfo) null)
        throw new NotSupportedException("Unknown list variant: " + listType.FullName);
      bool flag = false;
      object obj = (object) null;
      IList list = value as IList;
      object[] parameters = isList ? (object[]) null : new object[1];
      BasicList basicList = listType.IsArray ? new BasicList() : (BasicList) null;
      for (; this.TryDeserializeAuxiliaryType(reader, format, tag, itemType, ref obj, true, true, true, true, value ?? (object) listType); obj = (object) null)
      {
        flag = true;
        if (value == null && basicList == null)
        {
          value = TypeModel.CreateListInstance(listType, itemType);
          list = value as IList;
        }
        if (list != null)
          list.Add(obj);
        else if (basicList != null)
        {
          basicList.Add(obj);
        }
        else
        {
          parameters[0] = obj;
          methodInfo.Invoke(value, parameters);
        }
      }
      if (basicList != null)
      {
        if (value != null)
        {
          if (basicList.Count != 0)
          {
            Array sourceArray = (Array) value;
            Array instance = Array.CreateInstance(itemType, sourceArray.Length + basicList.Count);
            Array.Copy(sourceArray, instance, sourceArray.Length);
            basicList.CopyTo(instance, sourceArray.Length);
            value = (object) instance;
          }
        }
        else
        {
          Array instance = Array.CreateInstance(itemType, basicList.Count);
          basicList.CopyTo(instance, 0);
          value = (object) instance;
        }
      }
      return flag;
    }

    private static object CreateListInstance(Type listType, Type itemType)
    {
      Type type = listType;
      if (listType.IsArray)
        return (object) Array.CreateInstance(itemType, 0);
      if (!listType.IsClass || listType.IsAbstract || Helpers.GetConstructor(listType, Helpers.EmptyTypes, true) == (ConstructorInfo) null)
      {
        bool flag = false;
        string fullName;
        if (listType.IsInterface && (fullName = listType.FullName) != null && fullName.IndexOf("Dictionary") >= 0)
        {
          if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof (IDictionary<,>))
          {
            type = typeof (Dictionary<,>).MakeGenericType(listType.GetGenericArguments());
            flag = true;
          }
          if (!flag && listType == typeof (IDictionary))
          {
            type = typeof (Hashtable);
            flag = true;
          }
        }
        if (!flag)
        {
          type = typeof (List<>).MakeGenericType(itemType);
          flag = true;
        }
        if (!flag)
          type = typeof (ArrayList);
      }
      return Activator.CreateInstance(type);
    }

    /// <summary>
    /// This is the more "complete" version of Deserialize, which handles single instances of mapped types.
    /// The value is read as a complete field, including field-header and (for sub-objects) a
    /// length-prefix..kmc
    /// 
    /// In addition to that, this provides support for:
    ///  - basic values; individual int / string / Guid / etc
    ///  - IList sets of any type handled by TryDeserializeAuxiliaryType
    /// </summary>
    internal bool TryDeserializeAuxiliaryType(
      ProtoReader reader,
      DataFormat format,
      int tag,
      Type type,
      ref object value,
      bool skipOtherFields,
      bool asListItem,
      bool autoCreate,
      bool insideList,
      object parentListOrType)
    {
      ProtoTypeCode code = !(type == (Type) null) ? Helpers.GetTypeCode(type) : throw new ArgumentNullException(nameof (type));
      int modelKey;
      WireType wireType = this.GetWireType(code, format, ref type, out modelKey);
      bool flag1 = false;
      if (wireType == WireType.None)
      {
        Type itemType = TypeModel.GetListItemType(this, type);
        if (itemType == (Type) null && type.IsArray && type.GetArrayRank() == 1 && type != typeof (byte[]))
          itemType = type.GetElementType();
        if (itemType != (Type) null)
        {
          if (insideList)
          {
            Type type1 = parentListOrType as Type;
            if ((object) type1 == null)
              type1 = parentListOrType?.GetType();
            throw TypeModel.CreateNestedListsNotSupported(type1);
          }
          bool flag2 = this.TryDeserializeList(this, reader, format, tag, type, itemType, ref value);
          if (!flag2 & autoCreate)
            value = TypeModel.CreateListInstance(type, itemType);
          return flag2;
        }
        TypeModel.ThrowUnexpectedType(type);
      }
      while (!(flag1 & asListItem))
      {
        int num = reader.ReadFieldHeader();
        if (num > 0)
        {
          if (num != tag)
          {
            if (!skipOtherFields)
              throw ProtoReader.AddErrorData((Exception) new InvalidOperationException("Expected field " + tag.ToString() + ", but found " + num.ToString()), reader);
            reader.SkipField();
          }
          else
          {
            flag1 = true;
            reader.Hint(wireType);
            if (modelKey >= 0)
            {
              switch (wireType)
              {
                case WireType.String:
                case WireType.StartGroup:
                  SubItemToken token = ProtoReader.StartSubItem(reader);
                  value = this.Deserialize(modelKey, value, reader);
                  ProtoReader.EndSubItem(token, reader);
                  continue;
                default:
                  value = this.Deserialize(modelKey, value, reader);
                  continue;
              }
            }
            else
            {
              switch (code)
              {
                case ProtoTypeCode.Boolean:
                  value = (object) reader.ReadBoolean();
                  continue;
                case ProtoTypeCode.Char:
                  value = (object) (char) reader.ReadUInt16();
                  continue;
                case ProtoTypeCode.SByte:
                  value = (object) reader.ReadSByte();
                  continue;
                case ProtoTypeCode.Byte:
                  value = (object) reader.ReadByte();
                  continue;
                case ProtoTypeCode.Int16:
                  value = (object) reader.ReadInt16();
                  continue;
                case ProtoTypeCode.UInt16:
                  value = (object) reader.ReadUInt16();
                  continue;
                case ProtoTypeCode.Int32:
                  value = (object) reader.ReadInt32();
                  continue;
                case ProtoTypeCode.UInt32:
                  value = (object) reader.ReadUInt32();
                  continue;
                case ProtoTypeCode.Int64:
                  value = (object) reader.ReadInt64();
                  continue;
                case ProtoTypeCode.UInt64:
                  value = (object) reader.ReadUInt64();
                  continue;
                case ProtoTypeCode.Single:
                  value = (object) reader.ReadSingle();
                  continue;
                case ProtoTypeCode.Double:
                  value = (object) reader.ReadDouble();
                  continue;
                case ProtoTypeCode.Decimal:
                  value = (object) BclHelpers.ReadDecimal(reader);
                  continue;
                case ProtoTypeCode.DateTime:
                  value = (object) BclHelpers.ReadDateTime(reader);
                  continue;
                case ProtoTypeCode.String:
                  value = (object) reader.ReadString();
                  continue;
                case ProtoTypeCode.TimeSpan:
                  value = (object) BclHelpers.ReadTimeSpan(reader);
                  continue;
                case ProtoTypeCode.ByteArray:
                  value = (object) ProtoReader.AppendBytes((byte[]) value, reader);
                  continue;
                case ProtoTypeCode.Guid:
                  value = (object) BclHelpers.ReadGuid(reader);
                  continue;
                case ProtoTypeCode.Uri:
                  value = (object) new Uri(reader.ReadString(), UriKind.RelativeOrAbsolute);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        else
          break;
      }
      if (((flag1 ? 0 : (!asListItem ? 1 : 0)) & (autoCreate ? 1 : 0)) != 0 && type != typeof (string))
        value = Activator.CreateInstance(type);
      return flag1;
    }

    /// <summary>
    /// Creates a new runtime model, to which the caller
    /// can add support for a range of types. A model
    /// can be used "as is", or can be compiled for
    /// optimal performance.
    /// </summary>
    public static RuntimeTypeModel Create() => new RuntimeTypeModel(false);

    /// <summary>
    /// Applies common proxy scenarios, resolving the actual type to consider
    /// </summary>
    protected internal static Type ResolveProxies(Type type)
    {
      if (type == (Type) null)
        return (Type) null;
      if (type.IsGenericParameter)
        return (Type) null;
      Type underlyingType = Helpers.GetUnderlyingType(type);
      if (underlyingType != (Type) null)
        return underlyingType;
      string fullName1 = type.FullName;
      if (fullName1 != null && fullName1.StartsWith("System.Data.Entity.DynamicProxies."))
        return type.BaseType;
      foreach (Type type1 in type.GetInterfaces())
      {
        string fullName2 = type1.FullName;
        if (fullName2 == "NHibernate.Proxy.INHibernateProxy" || fullName2 == "NHibernate.Proxy.DynamicProxy.IProxy" || fullName2 == "NHibernate.Intercept.IFieldInterceptorAccessor")
          return type.BaseType;
      }
      return (Type) null;
    }

    /// <summary>
    /// Indicates whether the supplied type is explicitly modelled by the model
    /// </summary>
    public bool IsDefined(Type type) => this.GetKey(ref type) >= 0;

    /// <summary>
    /// Provides the key that represents a given type in the current model.
    /// The type is also normalized for proxies at the same time.
    /// </summary>
    protected internal int GetKey(ref Type type)
    {
      if (type == (Type) null)
        return -1;
      lock (this.knownKeys)
      {
        TypeModel.KnownTypeKey knownTypeKey;
        if (this.knownKeys.TryGetValue(type, out knownTypeKey))
        {
          type = knownTypeKey.Type;
          return knownTypeKey.Key;
        }
      }
      int keyImpl = this.GetKeyImpl(type);
      Type key = type;
      if (keyImpl < 0)
      {
        Type type1 = TypeModel.ResolveProxies(type);
        if (type1 != (Type) null && type1 != type)
        {
          type = type1;
          keyImpl = this.GetKeyImpl(type);
        }
      }
      lock (this.knownKeys)
        this.knownKeys[key] = new TypeModel.KnownTypeKey(type, keyImpl);
      return keyImpl;
    }

    /// <summary>Advertise that a type's key can have changed</summary>
    internal void ResetKeyCache()
    {
      lock (this.knownKeys)
        this.knownKeys.Clear();
    }

    /// <summary>
    /// Provides the key that represents a given type in the current model.
    /// </summary>
    protected abstract int GetKeyImpl(Type type);

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream.
    /// </summary>
    /// <param name="key">Represents the type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be serialized (cannot be null).</param>
    /// <param name="dest">The destination stream to write to.</param>
    protected internal abstract void Serialize(int key, object value, ProtoWriter dest);

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance (which may be null).
    /// </summary>
    /// <param name="key">Represents the type (including inheritance) to consider.</param>
    /// <param name="value">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    protected internal abstract object Deserialize(int key, object value, ProtoReader source);

    /// <summary>
    /// Create a deep clone of the supplied instance; any sub-items are also cloned.
    /// </summary>
    public object DeepClone(object value)
    {
      if (value == null)
        return (object) null;
      Type type = value.GetType();
      int key = this.GetKey(ref type);
      if (key >= 0 && !Helpers.IsEnum(type))
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ProtoWriter dest = ProtoWriter.Create((Stream) memoryStream, this))
          {
            dest.SetRootObject(value);
            this.Serialize(key, value, dest);
            dest.Close();
          }
          memoryStream.Position = 0L;
          ProtoReader protoReader = (ProtoReader) null;
          try
          {
            protoReader = ProtoReader.Create((Stream) memoryStream, this);
            return this.Deserialize(key, (object) null, protoReader);
          }
          finally
          {
            ProtoReader.Recycle(protoReader);
          }
        }
      }
      else
      {
        if (type == typeof (byte[]))
        {
          byte[] src = (byte[]) value;
          byte[] dst = new byte[src.Length];
          Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, src.Length);
          return (object) dst;
        }
        int modelKey;
        if (this.GetWireType(Helpers.GetTypeCode(type), DataFormat.Default, ref type, out modelKey) != WireType.None && modelKey < 0)
          return value;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ProtoWriter writer = ProtoWriter.Create((Stream) memoryStream, this))
          {
            if (!this.TrySerializeAuxiliaryType(writer, type, DataFormat.Default, 1, value, false, (object) null))
              TypeModel.ThrowUnexpectedType(type);
            writer.Close();
          }
          memoryStream.Position = 0L;
          ProtoReader reader = (ProtoReader) null;
          try
          {
            reader = ProtoReader.Create((Stream) memoryStream, this);
            value = (object) null;
            this.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, true, false, (object) null);
            return value;
          }
          finally
          {
            ProtoReader.Recycle(reader);
          }
        }
      }
    }

    /// <summary>
    /// Indicates that while an inheritance tree exists, the exact type encountered was not
    /// specified in that hierarchy and cannot be processed.
    /// </summary>
    protected internal static void ThrowUnexpectedSubtype(Type expected, Type actual)
    {
      if (expected != TypeModel.ResolveProxies(actual))
        throw new InvalidOperationException("Unexpected sub-type: " + actual.FullName);
    }

    /// <summary>
    /// Indicates that the given type was not expected, and cannot be processed.
    /// </summary>
    protected internal static void ThrowUnexpectedType(Type type)
    {
      string str = type == (Type) null ? "(unknown)" : type.FullName;
      Type type1 = type != (Type) null ? type.BaseType : throw new InvalidOperationException("Type is not expected, and no contract can be inferred: " + str);
      if (type1 != (Type) null && type1.IsGenericType && type1.GetGenericTypeDefinition().Name == "GeneratedMessage`2")
        throw new InvalidOperationException("Are you mixing protobuf-net and protobuf-csharp-port? See https://stackoverflow.com/q/11564914/23354; type: " + str);
    }

    internal static Exception CreateNestedListsNotSupported(Type type)
    {
      return (Exception) new NotSupportedException("Nested or jagged lists and arrays are not supported: " + (type?.FullName ?? "(null)"));
    }

    /// <summary>
    /// Indicates that the given type cannot be constructed; it may still be possible to
    /// deserialize into existing instances.
    /// </summary>
    public static void ThrowCannotCreateInstance(Type type)
    {
      throw new ProtoException("No parameterless constructor found for " + (type?.FullName ?? "(null)"));
    }

    internal static string SerializeType(TypeModel model, Type type)
    {
      if (model != null)
      {
        TypeFormatEventHandler dynamicTypeFormatting = model.DynamicTypeFormatting;
        if (dynamicTypeFormatting != null)
        {
          TypeFormatEventArgs args = new TypeFormatEventArgs(type);
          dynamicTypeFormatting((object) model, args);
          if (!string.IsNullOrEmpty(args.FormattedName))
            return args.FormattedName;
        }
      }
      return type.AssemblyQualifiedName;
    }

    internal static Type DeserializeType(TypeModel model, string value)
    {
      if (model != null)
      {
        TypeFormatEventHandler dynamicTypeFormatting = model.DynamicTypeFormatting;
        if (dynamicTypeFormatting != null)
        {
          TypeFormatEventArgs args = new TypeFormatEventArgs(value);
          dynamicTypeFormatting((object) model, args);
          if (args.Type != (Type) null)
            return args.Type;
        }
      }
      return Type.GetType(value);
    }

    /// <summary>
    /// Returns true if the type supplied is either a recognised contract type,
    /// or a *list* of a recognised contract type.
    /// </summary>
    /// <remarks>Note that primitives always return false, even though the engine
    /// will, if forced, try to serialize such</remarks>
    /// <returns>True if this type is recognised as a serializable entity, else false</returns>
    public bool CanSerializeContractType(Type type) => this.CanSerialize(type, false, true, true);

    /// <summary>
    /// Returns true if the type supplied is a basic type with inbuilt handling,
    /// a recognised contract type, or a *list* of a basic / contract type.
    /// </summary>
    public bool CanSerialize(Type type) => this.CanSerialize(type, true, true, true);

    /// <summary>
    /// Returns true if the type supplied is a basic type with inbuilt handling,
    /// or a *list* of a basic type with inbuilt handling
    /// </summary>
    public bool CanSerializeBasicType(Type type) => this.CanSerialize(type, true, false, true);

    private bool CanSerialize(Type type, bool allowBasic, bool allowContract, bool allowLists)
    {
      Type type1 = !(type == (Type) null) ? Helpers.GetUnderlyingType(type) : throw new ArgumentNullException(nameof (type));
      if (type1 != (Type) null)
        type = type1;
      switch (Helpers.GetTypeCode(type))
      {
        case ProtoTypeCode.Empty:
        case ProtoTypeCode.Unknown:
          if (this.GetKey(ref type) >= 0)
            return allowContract;
          if (allowLists)
          {
            Type type2 = (Type) null;
            if (type.IsArray)
            {
              if (type.GetArrayRank() == 1)
                type2 = type.GetElementType();
            }
            else
              type2 = TypeModel.GetListItemType(this, type);
            if (type2 != (Type) null)
              return this.CanSerialize(type2, allowBasic, allowContract, false);
          }
          return false;
        default:
          return allowBasic;
      }
    }

    /// <summary>Suggest a .proto definition for the given type</summary>
    /// <param name="type">The type to generate a .proto definition for, or <c>null</c> to generate a .proto that represents the entire model</param>
    /// <returns>The .proto definition as a string</returns>
    public virtual string GetSchema(Type type) => this.GetSchema(type, ProtoSyntax.Proto2);

    /// <summary>Suggest a .proto definition for the given type</summary>
    /// <param name="type">The type to generate a .proto definition for, or <c>null</c> to generate a .proto that represents the entire model</param>
    /// <returns>The .proto definition as a string</returns>
    /// <param name="syntax">The .proto syntax to use for the operation</param>
    public virtual string GetSchema(Type type, ProtoSyntax syntax)
    {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Used to provide custom services for writing and parsing type names when using dynamic types. Both parsing and formatting
    /// are provided on a single API as it is essential that both are mapped identically at all times.
    /// </summary>
    public event TypeFormatEventHandler DynamicTypeFormatting;

    /// <summary>
    /// Creates a new IFormatter that uses protocol-buffer [de]serialization.
    /// </summary>
    /// <returns>A new IFormatter to be used during [de]serialization.</returns>
    /// <param name="type">The type of object to be [de]deserialized by the formatter.</param>
    public IFormatter CreateFormatter(Type type)
    {
      return (IFormatter) new TypeModel.Formatter(this, type);
    }

    internal virtual Type GetType(string fullName, Assembly context)
    {
      return TypeModel.ResolveKnownType(fullName, this, context);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static Type ResolveKnownType(string name, TypeModel model, Assembly assembly)
    {
      if (string.IsNullOrEmpty(name))
        return (Type) null;
      try
      {
        Type type = Type.GetType(name);
        if (type != (Type) null)
          return type;
      }
      catch
      {
      }
      try
      {
        int length = name.IndexOf(',');
        string name1 = (length > 0 ? name.Substring(0, length) : name).Trim();
        if (assembly == (Assembly) null)
          assembly = Assembly.GetCallingAssembly();
        Type type = assembly?.GetType(name1);
        if (type != (Type) null)
          return type;
      }
      catch
      {
      }
      return (Type) null;
    }

    private sealed class DeserializeItemsIterator<T>(
      TypeModel model,
      Stream source,
      PrefixStyle style,
      int expectedField,
      SerializationContext context) : 
      TypeModel.DeserializeItemsIterator(model, source, model.MapType(typeof (T)), style, expectedField, (Serializer.TypeResolver) null, context),
      IEnumerator<T>,
      IDisposable,
      IEnumerator,
      IEnumerable<T>,
      IEnumerable
    {
      IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) this;

      public T Current => (T) base.Current;

      void IDisposable.Dispose()
      {
      }
    }

    private class DeserializeItemsIterator : IEnumerator, IEnumerable
    {
      private bool haveObject;
      private object current;
      private readonly Stream source;
      private readonly Type type;
      private readonly PrefixStyle style;
      private readonly int expectedField;
      private readonly Serializer.TypeResolver resolver;
      private readonly TypeModel model;
      private readonly SerializationContext context;

      IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this;

      public bool MoveNext()
      {
        if (this.haveObject)
          this.current = this.model.DeserializeWithLengthPrefix(this.source, (object) null, this.type, this.style, this.expectedField, this.resolver, out long _, out this.haveObject, this.context);
        return this.haveObject;
      }

      void IEnumerator.Reset() => throw new NotSupportedException();

      public object Current => this.current;

      public DeserializeItemsIterator(
        TypeModel model,
        Stream source,
        Type type,
        PrefixStyle style,
        int expectedField,
        Serializer.TypeResolver resolver,
        SerializationContext context)
      {
        this.haveObject = true;
        this.source = source;
        this.type = type;
        this.style = style;
        this.expectedField = expectedField;
        this.resolver = resolver;
        this.model = model;
        this.context = context;
      }
    }

    private readonly struct KnownTypeKey(Type type, int key)
    {
      public int Key { get; } = key;

      public Type Type { get; } = type;
    }

    /// <summary>Indicates the type of callback to be used</summary>
    protected internal enum CallbackType
    {
      /// <summary>Invoked before an object is serialized</summary>
      BeforeSerialize,
      /// <summary>Invoked after an object is serialized</summary>
      AfterSerialize,
      /// <summary>
      /// Invoked before an object is deserialized (or when a new instance is created)
      /// </summary>
      BeforeDeserialize,
      /// <summary>Invoked after an object is deserialized</summary>
      AfterDeserialize,
    }

    internal sealed class Formatter : IFormatter
    {
      private readonly TypeModel model;
      private readonly Type type;
      private SerializationBinder binder;
      private StreamingContext context;
      private ISurrogateSelector surrogateSelector;

      internal Formatter(TypeModel model, Type type)
      {
        this.model = model ?? throw new ArgumentNullException(nameof (model));
        this.type = type ?? throw new ArgumentNullException(nameof (type));
      }

      public SerializationBinder Binder
      {
        get => this.binder;
        set => this.binder = value;
      }

      public StreamingContext Context
      {
        get => this.context;
        set => this.context = value;
      }

      public object Deserialize(Stream source)
      {
        return this.model.Deserialize(source, (object) null, this.type, -1L, (SerializationContext) this.Context);
      }

      public void Serialize(Stream destination, object graph)
      {
        this.model.Serialize(destination, graph, (SerializationContext) this.Context);
      }

      public ISurrogateSelector SurrogateSelector
      {
        get => this.surrogateSelector;
        set => this.surrogateSelector = value;
      }
    }
  }
}
