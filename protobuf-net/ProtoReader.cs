// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoReader
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// A stateful reader, used to read a protobuf stream. Typical usage would be (sequentially) to call
  /// ReadFieldHeader and (after matching the field) an appropriate Read* method.
  /// </summary>
  public sealed class ProtoReader : IDisposable
  {
    private Stream source;
    private byte[] ioBuffer;
    private TypeModel model;
    private int fieldNumber;
    private int depth;
    private int ioIndex;
    private int available;
    private long position64;
    private long blockEnd64;
    private long dataRemaining64;
    private WireType wireType;
    private bool isFixedLength;
    private bool internStrings;
    private NetObjectCache netCache;
    private uint trapCount;
    internal const long TO_EOF = -1;
    private SerializationContext context;
    private const long Int64Msb = -9223372036854775808;
    private const int Int32Msb = -2147483648;
    private Dictionary<string, string> stringInterner;
    private static readonly UTF8Encoding encoding = new UTF8Encoding();
    private static readonly byte[] EmptyBlob = new byte[0];
    [ThreadStatic]
    private static ProtoReader lastReader;

    /// <summary>Gets the number of the field being processed.</summary>
    public int FieldNumber => this.fieldNumber;

    /// <summary>
    /// Indicates the underlying proto serialization format on the wire.
    /// </summary>
    public WireType WireType => this.wireType;

    /// <summary>Creates a new reader against a stream</summary>
    /// <param name="source">The source stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    [Obsolete("Please use ProtoReader.Create; this API may be removed in a future version", false)]
    public ProtoReader(Stream source, TypeModel model, SerializationContext context)
    {
      ProtoReader.Init(this, source, model, context, -1L);
    }

    /// <summary>
    /// Gets / sets a flag indicating whether strings should be checked for repetition; if
    /// true, any repeated UTF-8 byte sequence will result in the same String instance, rather
    /// than a second instance of the same string. Enabled by default. Note that this uses
    /// a <i>custom</i> interner - the system-wide string interner is not used.
    /// </summary>
    public bool InternStrings
    {
      get => this.internStrings;
      set => this.internStrings = value;
    }

    /// <summary>Creates a new reader against a stream</summary>
    /// <param name="source">The source stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    /// <param name="length">The number of bytes to read, or -1 to read until the end of the stream</param>
    [Obsolete("Please use ProtoReader.Create; this API may be removed in a future version", false)]
    public ProtoReader(Stream source, TypeModel model, SerializationContext context, int length)
    {
      ProtoReader.Init(this, source, model, context, (long) length);
    }

    /// <summary>Creates a new reader against a stream</summary>
    /// <param name="source">The source stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    /// <param name="length">The number of bytes to read, or -1 to read until the end of the stream</param>
    [Obsolete("Please use ProtoReader.Create; this API may be removed in a future version", false)]
    public ProtoReader(Stream source, TypeModel model, SerializationContext context, long length)
    {
      ProtoReader.Init(this, source, model, context, length);
    }

    private static void Init(
      ProtoReader reader,
      Stream source,
      TypeModel model,
      SerializationContext context,
      long length)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      reader.source = source.CanRead ? source : throw new ArgumentException("Cannot read from stream", nameof (source));
      reader.ioBuffer = BufferPool.GetBuffer();
      reader.model = model;
      bool flag = length >= 0L;
      reader.isFixedLength = flag;
      reader.dataRemaining64 = flag ? length : 0L;
      if (context == null)
        context = SerializationContext.Default;
      else
        context.Freeze();
      reader.context = context;
      reader.position64 = 0L;
      reader.available = reader.depth = reader.fieldNumber = reader.ioIndex = 0;
      reader.blockEnd64 = long.MaxValue;
      reader.internStrings = RuntimeTypeModel.Default.InternStrings;
      reader.wireType = WireType.None;
      reader.trapCount = 1U;
      if (reader.netCache != null)
        return;
      reader.netCache = new NetObjectCache();
    }

    /// <summary>
    /// Addition information about this deserialization operation.
    /// </summary>
    public SerializationContext Context => this.context;

    /// <summary>
    /// Releases resources used by the reader, but importantly <b>does not</b> Dispose the
    /// underlying stream; in many typical use-cases the stream is used for different
    /// processes, so it is assumed that the consumer will Dispose their stream separately.
    /// </summary>
    public void Dispose()
    {
      this.source = (Stream) null;
      this.model = (TypeModel) null;
      BufferPool.ReleaseBufferToPool(ref this.ioBuffer);
      if (this.stringInterner != null)
      {
        this.stringInterner.Clear();
        this.stringInterner = (Dictionary<string, string>) null;
      }
      if (this.netCache == null)
        return;
      this.netCache.Clear();
    }

    internal int TryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
    {
      if (this.available < 10)
        this.Ensure(10, false);
      if (this.available == 0)
      {
        value = 0U;
        return 0;
      }
      int ioIndex = this.ioIndex;
      ref uint local = ref value;
      byte[] ioBuffer1 = this.ioBuffer;
      int index1 = ioIndex;
      int num1 = index1 + 1;
      int num2 = (int) ioBuffer1[index1];
      local = (uint) num2;
      if (((int) value & 128) == 0)
        return 1;
      value &= (uint) sbyte.MaxValue;
      if (this.available == 1)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer2 = this.ioBuffer;
      int index2 = num1;
      int num3 = index2 + 1;
      uint num4 = (uint) ioBuffer2[index2];
      value |= (uint) (((int) num4 & (int) sbyte.MaxValue) << 7);
      if (((int) num4 & 128) == 0)
        return 2;
      if (this.available == 2)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer3 = this.ioBuffer;
      int index3 = num3;
      int num5 = index3 + 1;
      uint num6 = (uint) ioBuffer3[index3];
      value |= (uint) (((int) num6 & (int) sbyte.MaxValue) << 14);
      if (((int) num6 & 128) == 0)
        return 3;
      if (this.available == 3)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer4 = this.ioBuffer;
      int index4 = num5;
      int index5 = index4 + 1;
      uint num7 = (uint) ioBuffer4[index4];
      value |= (uint) (((int) num7 & (int) sbyte.MaxValue) << 21);
      if (((int) num7 & 128) == 0)
        return 4;
      if (this.available == 4)
        throw ProtoReader.EoF(this);
      uint num8 = (uint) this.ioBuffer[index5];
      value |= num8 << 28;
      if (((int) num8 & 240) == 0)
        return 5;
      int num9;
      int num10;
      int num11;
      int num12;
      int num13;
      if (trimNegative && ((int) num8 & 240) == 240 && this.available >= 10 && this.ioBuffer[num9 = index5 + 1] == byte.MaxValue && this.ioBuffer[num10 = num9 + 1] == byte.MaxValue && this.ioBuffer[num11 = num10 + 1] == byte.MaxValue && this.ioBuffer[num12 = num11 + 1] == byte.MaxValue && this.ioBuffer[num13 = num12 + 1] == (byte) 1)
        return 10;
      throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
    }

    private uint ReadUInt32Variant(bool trimNegative)
    {
      uint num1;
      int num2 = this.TryReadUInt32VariantWithoutMoving(trimNegative, out num1);
      if (num2 <= 0)
        throw ProtoReader.EoF(this);
      this.ioIndex += num2;
      this.available -= num2;
      this.position64 += (long) num2;
      return num1;
    }

    private bool TryReadUInt32Variant(out uint value)
    {
      int num = this.TryReadUInt32VariantWithoutMoving(false, out value);
      if (num <= 0)
        return false;
      this.ioIndex += num;
      this.available -= num;
      this.position64 += (long) num;
      return true;
    }

    /// <summary>
    /// Reads an unsigned 32-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public uint ReadUInt32()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return this.ReadUInt32Variant(false);
        case WireType.Fixed64:
          return checked ((uint) this.ReadUInt64());
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.position64 += 4L;
          this.available -= 4;
          return (uint) ((int) this.ioBuffer[this.ioIndex++] | (int) this.ioBuffer[this.ioIndex++] << 8 | (int) this.ioBuffer[this.ioIndex++] << 16 | (int) this.ioBuffer[this.ioIndex++] << 24);
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Returns the position of the current reader (note that this is not necessarily the same as the position
    /// in the underlying stream, if multiple readers are used on the same stream)
    /// </summary>
    public int Position => checked ((int) this.position64);

    /// <summary>
    /// Returns the position of the current reader (note that this is not necessarily the same as the position
    /// in the underlying stream, if multiple readers are used on the same stream)
    /// </summary>
    public long LongPosition => this.position64;

    internal void Ensure(int count, bool strict)
    {
      if (count > this.ioBuffer.Length)
      {
        BufferPool.ResizeAndFlushLeft(ref this.ioBuffer, count, this.ioIndex, this.available);
        this.ioIndex = 0;
      }
      else if (this.ioIndex + count >= this.ioBuffer.Length)
      {
        Buffer.BlockCopy((Array) this.ioBuffer, this.ioIndex, (Array) this.ioBuffer, 0, this.available);
        this.ioIndex = 0;
      }
      count -= this.available;
      int offset = this.ioIndex + this.available;
      int count1 = this.ioBuffer.Length - offset;
      if (this.isFixedLength && this.dataRemaining64 < (long) count1)
        count1 = (int) this.dataRemaining64;
      int num;
      while (count > 0 && count1 > 0 && (num = this.source.Read(this.ioBuffer, offset, count1)) > 0)
      {
        this.available += num;
        count -= num;
        count1 -= num;
        offset += num;
        if (this.isFixedLength)
          this.dataRemaining64 -= (long) num;
      }
      if (strict && count > 0)
        throw ProtoReader.EoF(this);
    }

    /// <summary>
    /// Reads a signed 16-bit integer from the stream: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public short ReadInt16() => checked ((short) this.ReadInt32());

    /// <summary>
    /// Reads an unsigned 16-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public ushort ReadUInt16() => checked ((ushort) this.ReadUInt32());

    /// <summary>
    /// Reads an unsigned 8-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public byte ReadByte() => checked ((byte) this.ReadUInt32());

    /// <summary>
    /// Reads a signed 8-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public sbyte ReadSByte() => checked ((sbyte) this.ReadInt32());

    /// <summary>
    /// Reads a signed 32-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public int ReadInt32()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return (int) this.ReadUInt32Variant(true);
        case WireType.Fixed64:
          return checked ((int) this.ReadInt64());
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.position64 += 4L;
          this.available -= 4;
          return (int) this.ioBuffer[this.ioIndex++] | (int) this.ioBuffer[this.ioIndex++] << 8 | (int) this.ioBuffer[this.ioIndex++] << 16 | (int) this.ioBuffer[this.ioIndex++] << 24;
        case WireType.SignedVariant:
          return ProtoReader.Zag(this.ReadUInt32Variant(true));
        default:
          throw this.CreateWireTypeException();
      }
    }

    private static int Zag(uint ziggedValue)
    {
      int num = (int) ziggedValue;
      return -(num & 1) ^ num >> 1 & int.MaxValue;
    }

    private static long Zag(ulong ziggedValue)
    {
      long num = (long) ziggedValue;
      return -(num & 1L) ^ num >> 1 & long.MaxValue;
    }

    /// <summary>
    /// Reads a signed 64-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public long ReadInt64()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return (long) this.ReadUInt64Variant();
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.position64 += 8L;
          this.available -= 8;
          return (long) this.ioBuffer[this.ioIndex++] | (long) this.ioBuffer[this.ioIndex++] << 8 | (long) this.ioBuffer[this.ioIndex++] << 16 | (long) this.ioBuffer[this.ioIndex++] << 24 | (long) this.ioBuffer[this.ioIndex++] << 32 | (long) this.ioBuffer[this.ioIndex++] << 40 | (long) this.ioBuffer[this.ioIndex++] << 48 | (long) this.ioBuffer[this.ioIndex++] << 56;
        case WireType.Fixed32:
          return (long) this.ReadInt32();
        case WireType.SignedVariant:
          return ProtoReader.Zag(this.ReadUInt64Variant());
        default:
          throw this.CreateWireTypeException();
      }
    }

    private int TryReadUInt64VariantWithoutMoving(out ulong value)
    {
      if (this.available < 10)
        this.Ensure(10, false);
      if (this.available == 0)
      {
        value = 0UL;
        return 0;
      }
      int ioIndex = this.ioIndex;
      ref ulong local = ref value;
      byte[] ioBuffer1 = this.ioBuffer;
      int index1 = ioIndex;
      int num1 = index1 + 1;
      long num2 = (long) ioBuffer1[index1];
      local = (ulong) num2;
      if (((long) value & 128L) == 0L)
        return 1;
      value &= (ulong) sbyte.MaxValue;
      if (this.available == 1)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer2 = this.ioBuffer;
      int index2 = num1;
      int num3 = index2 + 1;
      ulong num4 = (ulong) ioBuffer2[index2];
      value |= (ulong) (((long) num4 & (long) sbyte.MaxValue) << 7);
      if (((long) num4 & 128L) == 0L)
        return 2;
      if (this.available == 2)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer3 = this.ioBuffer;
      int index3 = num3;
      int num5 = index3 + 1;
      ulong num6 = (ulong) ioBuffer3[index3];
      value |= (ulong) (((long) num6 & (long) sbyte.MaxValue) << 14);
      if (((long) num6 & 128L) == 0L)
        return 3;
      if (this.available == 3)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer4 = this.ioBuffer;
      int index4 = num5;
      int num7 = index4 + 1;
      ulong num8 = (ulong) ioBuffer4[index4];
      value |= (ulong) (((long) num8 & (long) sbyte.MaxValue) << 21);
      if (((long) num8 & 128L) == 0L)
        return 4;
      if (this.available == 4)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer5 = this.ioBuffer;
      int index5 = num7;
      int num9 = index5 + 1;
      ulong num10 = (ulong) ioBuffer5[index5];
      value |= (ulong) (((long) num10 & (long) sbyte.MaxValue) << 28);
      if (((long) num10 & 128L) == 0L)
        return 5;
      if (this.available == 5)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer6 = this.ioBuffer;
      int index6 = num9;
      int num11 = index6 + 1;
      ulong num12 = (ulong) ioBuffer6[index6];
      value |= (ulong) (((long) num12 & (long) sbyte.MaxValue) << 35);
      if (((long) num12 & 128L) == 0L)
        return 6;
      if (this.available == 6)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer7 = this.ioBuffer;
      int index7 = num11;
      int num13 = index7 + 1;
      ulong num14 = (ulong) ioBuffer7[index7];
      value |= (ulong) (((long) num14 & (long) sbyte.MaxValue) << 42);
      if (((long) num14 & 128L) == 0L)
        return 7;
      if (this.available == 7)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer8 = this.ioBuffer;
      int index8 = num13;
      int num15 = index8 + 1;
      ulong num16 = (ulong) ioBuffer8[index8];
      value |= (ulong) (((long) num16 & (long) sbyte.MaxValue) << 49);
      if (((long) num16 & 128L) == 0L)
        return 8;
      if (this.available == 8)
        throw ProtoReader.EoF(this);
      byte[] ioBuffer9 = this.ioBuffer;
      int index9 = num15;
      int index10 = index9 + 1;
      ulong num17 = (ulong) ioBuffer9[index9];
      value |= (ulong) (((long) num17 & (long) sbyte.MaxValue) << 56);
      if (((long) num17 & 128L) == 0L)
        return 9;
      if (this.available == 9)
        throw ProtoReader.EoF(this);
      ulong num18 = (ulong) this.ioBuffer[index10];
      value |= num18 << 63;
      if (((long) num18 & -2L) != 0L)
        throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
      return 10;
    }

    private ulong ReadUInt64Variant()
    {
      ulong num1;
      int num2 = this.TryReadUInt64VariantWithoutMoving(out num1);
      if (num2 <= 0)
        throw ProtoReader.EoF(this);
      this.ioIndex += num2;
      this.available -= num2;
      this.position64 += (long) num2;
      return num1;
    }

    private string Intern(string value)
    {
      switch (value)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          if (this.stringInterner == null)
          {
            this.stringInterner = new Dictionary<string, string>()
            {
              {
                value,
                value
              }
            };
          }
          else
          {
            string str;
            if (this.stringInterner.TryGetValue(value, out str))
              value = str;
            else
              this.stringInterner.Add(value, value);
          }
          return value;
      }
    }

    /// <summary>
    /// Reads a string from the stream (using UTF8); supported wire-types: String
    /// </summary>
    public string ReadString()
    {
      if (this.wireType != WireType.String)
        throw this.CreateWireTypeException();
      int count = (int) this.ReadUInt32Variant(false);
      if (count == 0)
        return "";
      if (this.available < count)
        this.Ensure(count, true);
      string str = ProtoReader.encoding.GetString(this.ioBuffer, this.ioIndex, count);
      if (this.internStrings)
        str = this.Intern(str);
      this.available -= count;
      this.position64 += (long) count;
      this.ioIndex += count;
      return str;
    }

    /// <summary>
    /// Throws an exception indication that the given value cannot be mapped to an enum.
    /// </summary>
    public void ThrowEnumException(Type type, int value)
    {
      throw ProtoReader.AddErrorData((Exception) new ProtoException("No " + (type == (Type) null ? "<null>" : type.FullName) + " enum is mapped to the wire-value " + value.ToString()), this);
    }

    private Exception CreateWireTypeException()
    {
      return this.CreateException("Invalid wire-type; this usually means you have over-written a file without truncating or setting the length; see https://stackoverflow.com/q/2152978/23354");
    }

    private Exception CreateException(string message)
    {
      return ProtoReader.AddErrorData((Exception) new ProtoException(message), this);
    }

    /// <summary>
    /// Reads a double-precision number from the stream; supported wire-types: Fixed32, Fixed64
    /// </summary>
    public unsafe double ReadDouble()
    {
      switch (this.wireType)
      {
        case WireType.Fixed64:
          return *(double*) &this.ReadInt64();
        case WireType.Fixed32:
          return (double) this.ReadSingle();
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Reads (merges) a sub-message from the stream, internally calling StartSubItem and EndSubItem, and (in between)
    /// parsing the message in accordance with the model associated with the reader
    /// </summary>
    public static object ReadObject(object value, int key, ProtoReader reader)
    {
      return ProtoReader.ReadTypedObject(value, key, reader, (Type) null);
    }

    internal static object ReadTypedObject(object value, int key, ProtoReader reader, Type type)
    {
      SubItemToken token = reader.model != null ? ProtoReader.StartSubItem(reader) : throw ProtoReader.AddErrorData((Exception) new InvalidOperationException("Cannot deserialize sub-objects unless a model is provided"), reader);
      if (key >= 0)
        value = reader.model.Deserialize(key, value, reader);
      else if (!(type != (Type) null) || !reader.model.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, true, false, (object) null))
        TypeModel.ThrowUnexpectedType(type);
      ProtoReader.EndSubItem(token, reader);
      return value;
    }

    /// <summary>
    /// Makes the end of consuming a nested message in the stream; the stream must be either at the correct EndGroup
    /// marker, or all fields of the sub-message must have been consumed (in either case, this means ReadFieldHeader
    /// should return zero)
    /// </summary>
    public static void EndSubItem(SubItemToken token, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      long num = token.value64;
      if (reader.wireType == WireType.EndGroup)
      {
        if (num >= 0L)
          throw ProtoReader.AddErrorData((Exception) new ArgumentException(nameof (token)), reader);
        if (-(int) num != reader.fieldNumber)
          throw reader.CreateException("Wrong group was ended");
        reader.wireType = WireType.None;
        --reader.depth;
      }
      else
      {
        if (num < reader.position64)
          throw reader.CreateException(string.Format("Sub-message not read entirely; expected {0}, was {1}", (object) num, (object) reader.position64));
        if (reader.blockEnd64 != reader.position64 && reader.blockEnd64 != long.MaxValue)
          throw reader.CreateException("Sub-message not read correctly");
        reader.blockEnd64 = num;
        --reader.depth;
      }
    }

    /// <summary>
    /// Begins consuming a nested message in the stream; supported wire-types: StartGroup, String
    /// </summary>
    /// <remarks>The token returned must be help and used when callining EndSubItem</remarks>
    public static SubItemToken StartSubItem(ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      switch (reader.wireType)
      {
        case WireType.String:
          long num = (long) reader.ReadUInt64Variant();
          if (num < 0L)
            throw ProtoReader.AddErrorData((Exception) new InvalidOperationException(), reader);
          long blockEnd64 = reader.blockEnd64;
          reader.blockEnd64 = reader.position64 + num;
          ++reader.depth;
          return new SubItemToken(blockEnd64);
        case WireType.StartGroup:
          reader.wireType = WireType.None;
          ++reader.depth;
          return new SubItemToken((long) -reader.fieldNumber);
        default:
          throw reader.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Reads a field header from the stream, setting the wire-type and retuning the field number. If no
    /// more fields are available, then 0 is returned. This methods respects sub-messages.
    /// </summary>
    public int ReadFieldHeader()
    {
      if (this.blockEnd64 <= this.position64 || this.wireType == WireType.EndGroup)
        return 0;
      uint num;
      if (this.TryReadUInt32Variant(out num) && num != 0U)
      {
        this.wireType = (WireType) ((int) num & 7);
        this.fieldNumber = (int) (num >> 3);
        if (this.fieldNumber < 1)
          throw new ProtoException("Invalid field in source data: " + this.fieldNumber.ToString());
      }
      else
      {
        this.wireType = WireType.None;
        this.fieldNumber = 0;
      }
      if (this.wireType != WireType.EndGroup)
        return this.fieldNumber;
      if (this.depth > 0)
        return 0;
      throw new ProtoException("Unexpected end-group in source data; this usually means the source data is corrupt");
    }

    /// <summary>
    /// Looks ahead to see whether the next field in the stream is what we expect
    /// (typically; what we've just finished reading - for example ot read successive list items)
    /// </summary>
    public bool TryReadFieldHeader(int field)
    {
      if (this.blockEnd64 <= this.position64 || this.wireType == WireType.EndGroup)
        return false;
      uint num1;
      int num2 = this.TryReadUInt32VariantWithoutMoving(false, out num1);
      WireType wireType;
      if (num2 <= 0 || (int) num1 >> 3 != field || (wireType = (WireType) ((int) num1 & 7)) == WireType.EndGroup)
        return false;
      this.wireType = wireType;
      this.fieldNumber = field;
      this.position64 += (long) num2;
      this.ioIndex += num2;
      this.available -= num2;
      return true;
    }

    /// <summary>Get the TypeModel associated with this reader</summary>
    public TypeModel Model => this.model;

    /// <summary>
    /// Compares the streams current wire-type to the hinted wire-type, updating the reader if necessary; for example,
    /// a Variant may be updated to SignedVariant. If the hinted wire-type is unrelated then no change is made.
    /// </summary>
    public void Hint(WireType wireType)
    {
      if (this.wireType == wireType || (wireType & (WireType.StartGroup | WireType.EndGroup)) != this.wireType)
        return;
      this.wireType = wireType;
    }

    /// <summary>
    /// Verifies that the stream's current wire-type is as expected, or a specialized sub-type (for example,
    /// SignedVariant) - in which case the current wire-type is updated. Otherwise an exception is thrown.
    /// </summary>
    public void Assert(WireType wireType)
    {
      if (this.wireType == wireType)
        return;
      this.wireType = (wireType & (WireType.StartGroup | WireType.EndGroup)) == this.wireType ? wireType : throw this.CreateWireTypeException();
    }

    /// <summary>Discards the data for the current field.</summary>
    public void SkipField()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
        case WireType.SignedVariant:
          long num1 = (long) this.ReadUInt64Variant();
          break;
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.available -= 8;
          this.ioIndex += 8;
          this.position64 += 8L;
          break;
        case WireType.String:
          long num2 = (long) this.ReadUInt64Variant();
          if (num2 <= (long) this.available)
          {
            this.available -= (int) num2;
            this.ioIndex += (int) num2;
            this.position64 += num2;
            break;
          }
          this.position64 += num2;
          long count = num2 - (long) this.available;
          this.ioIndex = this.available = 0;
          if (this.isFixedLength)
          {
            if (count > this.dataRemaining64)
              throw ProtoReader.EoF(this);
            this.dataRemaining64 -= count;
          }
          ProtoReader.Seek(this.source, count, this.ioBuffer);
          break;
        case WireType.StartGroup:
          int fieldNumber = this.fieldNumber;
          ++this.depth;
          while (this.ReadFieldHeader() > 0)
            this.SkipField();
          --this.depth;
          if (this.wireType != WireType.EndGroup || this.fieldNumber != fieldNumber)
            throw this.CreateWireTypeException();
          this.wireType = WireType.None;
          break;
        case WireType.Fixed32:
          if (this.available < 4)
            this.Ensure(4, true);
          this.available -= 4;
          this.ioIndex += 4;
          this.position64 += 4L;
          break;
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Reads an unsigned 64-bit integer from the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public ulong ReadUInt64()
    {
      switch (this.wireType)
      {
        case WireType.Variant:
          return this.ReadUInt64Variant();
        case WireType.Fixed64:
          if (this.available < 8)
            this.Ensure(8, true);
          this.position64 += 8L;
          this.available -= 8;
          return (ulong) ((long) this.ioBuffer[this.ioIndex++] | (long) this.ioBuffer[this.ioIndex++] << 8 | (long) this.ioBuffer[this.ioIndex++] << 16 | (long) this.ioBuffer[this.ioIndex++] << 24 | (long) this.ioBuffer[this.ioIndex++] << 32 | (long) this.ioBuffer[this.ioIndex++] << 40 | (long) this.ioBuffer[this.ioIndex++] << 48 | (long) this.ioBuffer[this.ioIndex++] << 56);
        case WireType.Fixed32:
          return (ulong) this.ReadUInt32();
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Reads a single-precision number from the stream; supported wire-types: Fixed32, Fixed64
    /// </summary>
    public unsafe float ReadSingle()
    {
      switch (this.wireType)
      {
        case WireType.Fixed64:
          double d = this.ReadDouble();
          float f = (float) d;
          return !float.IsInfinity(f) || double.IsInfinity(d) ? f : throw ProtoReader.AddErrorData((Exception) new OverflowException(), this);
        case WireType.Fixed32:
          return *(float*) &this.ReadInt32();
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Reads a boolean value from the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    /// <returns></returns>
    public bool ReadBoolean()
    {
      switch (this.ReadUInt32())
      {
        case 0:
          return false;
        case 1:
          return true;
        default:
          throw this.CreateException("Unexpected boolean value");
      }
    }

    /// <summary>
    /// Reads a byte-sequence from the stream, appending them to an existing byte-sequence (which can be null); supported wire-types: String
    /// </summary>
    public static byte[] AppendBytes(byte[] value, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      switch (reader.wireType)
      {
        case WireType.Variant:
          return new byte[0];
        case WireType.String:
          int count1 = (int) reader.ReadUInt32Variant(false);
          reader.wireType = WireType.None;
          if (count1 == 0)
            return value ?? ProtoReader.EmptyBlob;
          int dstOffset;
          if (value == null || value.Length == 0)
          {
            dstOffset = 0;
            value = new byte[count1];
          }
          else
          {
            dstOffset = value.Length;
            byte[] dst = new byte[value.Length + count1];
            Buffer.BlockCopy((Array) value, 0, (Array) dst, 0, value.Length);
            value = dst;
          }
          reader.position64 += (long) count1;
          while (count1 > reader.available)
          {
            if (reader.available > 0)
            {
              Buffer.BlockCopy((Array) reader.ioBuffer, reader.ioIndex, (Array) value, dstOffset, reader.available);
              count1 -= reader.available;
              dstOffset += reader.available;
              reader.ioIndex = reader.available = 0;
            }
            int count2 = count1 > reader.ioBuffer.Length ? reader.ioBuffer.Length : count1;
            if (count2 > 0)
              reader.Ensure(count2, true);
          }
          if (count1 > 0)
          {
            Buffer.BlockCopy((Array) reader.ioBuffer, reader.ioIndex, (Array) value, dstOffset, count1);
            reader.ioIndex += count1;
            reader.available -= count1;
          }
          return value;
        default:
          throw reader.CreateWireTypeException();
      }
    }

    private static int ReadByteOrThrow(Stream source)
    {
      int num = source.ReadByte();
      return num >= 0 ? num : throw ProtoReader.EoF((ProtoReader) null);
    }

    /// <summary>
    /// Reads the length-prefix of a message from a stream without buffering additional data, allowing a fixed-length
    /// reader to be created.
    /// </summary>
    public static int ReadLengthPrefix(
      Stream source,
      bool expectHeader,
      PrefixStyle style,
      out int fieldNumber)
    {
      return ProtoReader.ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out int _);
    }

    /// <summary>
    /// Reads a little-endian encoded integer. An exception is thrown if the data is not all available.
    /// </summary>
    public static int DirectReadLittleEndianInt32(Stream source)
    {
      return ProtoReader.ReadByteOrThrow(source) | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24;
    }

    /// <summary>
    /// Reads a big-endian encoded integer. An exception is thrown if the data is not all available.
    /// </summary>
    public static int DirectReadBigEndianInt32(Stream source)
    {
      return ProtoReader.ReadByteOrThrow(source) << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source);
    }

    /// <summary>
    /// Reads a varint encoded integer. An exception is thrown if the data is not all available.
    /// </summary>
    public static int DirectReadVarintInt32(Stream source)
    {
      ulong num;
      if (ProtoReader.TryReadUInt64Variant(source, out num) <= 0)
        throw ProtoReader.EoF((ProtoReader) null);
      return checked ((int) num);
    }

    /// <summary>
    /// Reads a string (of a given lenth, in bytes) directly from the source into a pre-existing buffer. An exception is thrown if the data is not all available.
    /// </summary>
    public static void DirectReadBytes(Stream source, byte[] buffer, int offset, int count)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      int num;
      for (; count > 0 && (num = source.Read(buffer, offset, count)) > 0; offset += num)
        count -= num;
      if (count > 0)
        throw ProtoReader.EoF((ProtoReader) null);
    }

    /// <summary>
    /// Reads a given number of bytes directly from the source. An exception is thrown if the data is not all available.
    /// </summary>
    public static byte[] DirectReadBytes(Stream source, int count)
    {
      byte[] buffer = new byte[count];
      ProtoReader.DirectReadBytes(source, buffer, 0, count);
      return buffer;
    }

    /// <summary>
    /// Reads a string (of a given lenth, in bytes) directly from the source. An exception is thrown if the data is not all available.
    /// </summary>
    public static string DirectReadString(Stream source, int length)
    {
      byte[] numArray = new byte[length];
      ProtoReader.DirectReadBytes(source, numArray, 0, length);
      return Encoding.UTF8.GetString(numArray, 0, length);
    }

    /// <summary>
    /// Reads the length-prefix of a message from a stream without buffering additional data, allowing a fixed-length
    /// reader to be created.
    /// </summary>
    public static int ReadLengthPrefix(
      Stream source,
      bool expectHeader,
      PrefixStyle style,
      out int fieldNumber,
      out int bytesRead)
    {
      if (style != PrefixStyle.None)
        return checked ((int) ProtoReader.ReadLongLengthPrefix(source, expectHeader, style, out fieldNumber, out bytesRead));
      bytesRead = fieldNumber = 0;
      return int.MaxValue;
    }

    /// <summary>
    /// Reads the length-prefix of a message from a stream without buffering additional data, allowing a fixed-length
    /// reader to be created.
    /// </summary>
    public static long ReadLongLengthPrefix(
      Stream source,
      bool expectHeader,
      PrefixStyle style,
      out int fieldNumber,
      out int bytesRead)
    {
      fieldNumber = 0;
      switch (style)
      {
        case PrefixStyle.None:
          bytesRead = 0;
          return long.MaxValue;
        case PrefixStyle.Base128:
          bytesRead = 0;
          if (expectHeader)
          {
            ulong num1;
            int num2 = ProtoReader.TryReadUInt64Variant(source, out num1);
            bytesRead += num2;
            if (num2 > 0)
            {
              if (((long) num1 & 7L) != 2L)
                throw new InvalidOperationException();
              fieldNumber = (int) (num1 >> 3);
              int num3 = ProtoReader.TryReadUInt64Variant(source, out num1);
              bytesRead += num3;
              if (bytesRead == 0)
                throw ProtoReader.EoF((ProtoReader) null);
              return (long) num1;
            }
            bytesRead = 0;
            return -1;
          }
          ulong num4;
          int num5 = ProtoReader.TryReadUInt64Variant(source, out num4);
          bytesRead += num5;
          return bytesRead >= 0 ? (long) num4 : -1L;
        case PrefixStyle.Fixed32:
          int num6 = source.ReadByte();
          if (num6 < 0)
          {
            bytesRead = 0;
            return -1;
          }
          bytesRead = 4;
          return (long) (num6 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24);
        case PrefixStyle.Fixed32BigEndian:
          int num7 = source.ReadByte();
          if (num7 < 0)
          {
            bytesRead = 0;
            return -1;
          }
          bytesRead = 4;
          return (long) (num7 << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source));
        default:
          throw new ArgumentOutOfRangeException(nameof (style));
      }
    }

    /// <returns>The number of bytes consumed; 0 if no data available</returns>
    private static int TryReadUInt64Variant(Stream source, out ulong value)
    {
      value = 0UL;
      int num1 = source.ReadByte();
      if (num1 < 0)
        return 0;
      value = (ulong) (uint) num1;
      if (((long) value & 128L) == 0L)
        return 1;
      value &= (ulong) sbyte.MaxValue;
      int num2 = 1;
      int num3 = 7;
      while (num2 < 9)
      {
        int num4 = source.ReadByte();
        if (num4 < 0)
          throw ProtoReader.EoF((ProtoReader) null);
        value |= (ulong) (((long) num4 & (long) sbyte.MaxValue) << num3);
        num3 += 7;
        ++num2;
        if ((num4 & 128) == 0)
          return num2;
      }
      int num5 = source.ReadByte();
      if (num5 < 0)
        throw ProtoReader.EoF((ProtoReader) null);
      if ((num5 & 1) != 0)
        throw new OverflowException();
      value |= (ulong) (((long) num5 & (long) sbyte.MaxValue) << num3);
      int num6;
      return num6 = num2 + 1;
    }

    internal static void Seek(Stream source, long count, byte[] buffer)
    {
      if (source.CanSeek)
      {
        source.Seek(count, SeekOrigin.Current);
        count = 0L;
      }
      else if (buffer != null)
      {
        int num1;
        while (count > (long) buffer.Length && (num1 = source.Read(buffer, 0, buffer.Length)) > 0)
          count -= (long) num1;
        int num2;
        while (count > 0L && (num2 = source.Read(buffer, 0, (int) count)) > 0)
          count -= (long) num2;
      }
      else
      {
        buffer = BufferPool.GetBuffer();
        try
        {
          int num3;
          while (count > (long) buffer.Length && (num3 = source.Read(buffer, 0, buffer.Length)) > 0)
            count -= (long) num3;
          int num4;
          for (; count > 0L; count -= (long) num4)
          {
            if ((num4 = source.Read(buffer, 0, (int) count)) <= 0)
              break;
          }
        }
        finally
        {
          BufferPool.ReleaseBufferToPool(ref buffer);
        }
      }
      if (count > 0L)
        throw ProtoReader.EoF((ProtoReader) null);
    }

    internal static Exception AddErrorData(Exception exception, ProtoReader source)
    {
      if (exception != null && source != null && !exception.Data.Contains((object) "protoSource"))
        exception.Data.Add((object) "protoSource", (object) string.Format("tag={0}; wire-type={1}; offset={2}; depth={3}", (object) source.fieldNumber, (object) source.wireType, (object) source.position64, (object) source.depth));
      return exception;
    }

    private static Exception EoF(ProtoReader source)
    {
      return ProtoReader.AddErrorData((Exception) new EndOfStreamException(), source);
    }

    /// <summary>
    /// Copies the current field into the instance as extension data
    /// </summary>
    public void AppendExtensionData(IExtensible instance)
    {
      IExtension extension = instance != null ? instance.GetExtensionObject(true) : throw new ArgumentNullException(nameof (instance));
      bool commit = false;
      Stream stream = extension.BeginAppend();
      try
      {
        using (ProtoWriter writer = ProtoWriter.Create(stream, this.model))
        {
          this.AppendExtensionField(writer);
          writer.Close();
        }
        commit = true;
      }
      finally
      {
        extension.EndAppend(stream, commit);
      }
    }

    private void AppendExtensionField(ProtoWriter writer)
    {
      ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, writer);
      switch (this.wireType)
      {
        case WireType.Variant:
        case WireType.Fixed64:
        case WireType.SignedVariant:
          ProtoWriter.WriteInt64(this.ReadInt64(), writer);
          break;
        case WireType.String:
          ProtoWriter.WriteBytes(ProtoReader.AppendBytes((byte[]) null, this), writer);
          break;
        case WireType.StartGroup:
          SubItemToken token1 = ProtoReader.StartSubItem(this);
          SubItemToken token2 = ProtoWriter.StartSubItem((object) null, writer);
          while (this.ReadFieldHeader() > 0)
            this.AppendExtensionField(writer);
          ProtoReader.EndSubItem(token1, this);
          ProtoWriter.EndSubItem(token2, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteInt32(this.ReadInt32(), writer);
          break;
        default:
          throw this.CreateWireTypeException();
      }
    }

    /// <summary>
    /// Indicates whether the reader still has data remaining in the current sub-item,
    /// additionally setting the wire-type for the next field if there is more data.
    /// This is used when decoding packed data.
    /// </summary>
    public static bool HasSubValue(WireType wireType, ProtoReader source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (source.blockEnd64 <= source.position64 || wireType == WireType.EndGroup)
        return false;
      source.wireType = wireType;
      return true;
    }

    internal int GetTypeKey(ref Type type) => this.model.GetKey(ref type);

    internal NetObjectCache NetCache => this.netCache;

    internal Type DeserializeType(string value) => TypeModel.DeserializeType(this.model, value);

    internal void SetRootObject(object value)
    {
      this.netCache.SetKeyedObject(0, value);
      --this.trapCount;
    }

    /// <summary>
    /// Utility method, not intended for public use; this helps maintain the root object is complex scenarios
    /// </summary>
    public static void NoteObject(object value, ProtoReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if (reader.trapCount == 0U)
        return;
      reader.netCache.RegisterTrappedObject(value);
      --reader.trapCount;
    }

    /// <summary>
    /// Reads a Type from the stream, using the model's DynamicTypeFormatting if appropriate; supported wire-types: String
    /// </summary>
    public Type ReadType() => TypeModel.DeserializeType(this.model, this.ReadString());

    internal void TrapNextObject(int newObjectKey)
    {
      ++this.trapCount;
      this.netCache.SetKeyedObject(newObjectKey, (object) null);
    }

    internal void CheckFullyConsumed()
    {
      if (this.isFixedLength)
      {
        if (this.dataRemaining64 != 0L)
          throw new ProtoException("Incorrect number of bytes consumed");
      }
      else if (this.available != 0)
        throw new ProtoException("Unconsumed data left in the buffer; this suggests corrupt input");
    }

    /// <summary>
    /// Merge two objects using the details from the current reader; this is used to change the type
    /// of objects when an inheritance relationship is discovered later than usual during deserilazation.
    /// </summary>
    public static object Merge(ProtoReader parent, object from, object to)
    {
      TypeModel typeModel = parent != null ? parent.Model : throw new ArgumentNullException(nameof (parent));
      SerializationContext context = parent.Context;
      if (typeModel == null)
        throw new InvalidOperationException("Types cannot be merged unless a type-model has been specified");
      using (MemoryStream memoryStream = new MemoryStream())
      {
        typeModel.Serialize((Stream) memoryStream, from, context);
        memoryStream.Position = 0L;
        return typeModel.Deserialize((Stream) memoryStream, to, (Type) null);
      }
    }

    internal static ProtoReader Create(
      Stream source,
      TypeModel model,
      SerializationContext context,
      int len)
    {
      return ProtoReader.Create(source, model, context, (long) len);
    }

    /// <summary>Creates a new reader against a stream</summary>
    /// <param name="source">The source stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to deserialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    /// <param name="length">The number of bytes to read, or -1 to read until the end of the stream</param>
    public static ProtoReader Create(
      Stream source,
      TypeModel model,
      SerializationContext context = null,
      long length = -1)
    {
      ProtoReader recycled = ProtoReader.GetRecycled();
      if (recycled == null)
        return new ProtoReader(source, model, context, length);
      ProtoReader.Init(recycled, source, model, context, length);
      return recycled;
    }

    private static ProtoReader GetRecycled()
    {
      ProtoReader lastReader = ProtoReader.lastReader;
      ProtoReader.lastReader = (ProtoReader) null;
      return lastReader;
    }

    internal static void Recycle(ProtoReader reader)
    {
      if (reader == null)
        return;
      reader.Dispose();
      ProtoReader.lastReader = reader;
    }
  }
}
