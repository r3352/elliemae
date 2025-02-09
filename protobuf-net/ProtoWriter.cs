﻿// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoWriter
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.IO;
using System.Text;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Represents an output stream for writing protobuf data.
  /// 
  /// Why is the API backwards (static methods with writer arguments)?
  /// See: http://marcgravell.blogspot.com/2010/03/last-will-be-first-and-first-will-be.html
  /// </summary>
  public sealed class ProtoWriter : IDisposable
  {
    private Stream dest;
    private TypeModel model;
    private readonly NetObjectCache netCache = new NetObjectCache();
    private int fieldNumber;
    private int flushLock;
    private WireType wireType;
    private int depth;
    private const int RecursionCheckDepth = 25;
    private MutableList recursionStack;
    private readonly SerializationContext context;
    private byte[] ioBuffer;
    private int ioIndex;
    private long position64;
    private static readonly UTF8Encoding encoding = new UTF8Encoding();
    private int packedFieldNumber;

    /// <summary>
    /// Write an encapsulated sub-object, using the supplied unique key (reprasenting a type).
    /// </summary>
    /// <param name="value">The object to write.</param>
    /// <param name="key">The key that uniquely identifies the type within the model.</param>
    /// <param name="writer">The destination.</param>
    public static void WriteObject(object value, int key, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      SubItemToken token = writer.model != null ? ProtoWriter.StartSubItem(value, writer) : throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
      if (key >= 0)
        writer.model.Serialize(key, value, writer);
      else if (writer.model == null || !writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, false, (object) null))
        TypeModel.ThrowUnexpectedType(value.GetType());
      ProtoWriter.EndSubItem(token, writer);
    }

    /// <summary>
    /// Write an encapsulated sub-object, using the supplied unique key (reprasenting a type) - but the
    /// caller is asserting that this relationship is non-recursive; no recursion check will be
    /// performed.
    /// </summary>
    /// <param name="value">The object to write.</param>
    /// <param name="key">The key that uniquely identifies the type within the model.</param>
    /// <param name="writer">The destination.</param>
    public static void WriteRecursionSafeObject(object value, int key, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      SubItemToken token = writer.model != null ? ProtoWriter.StartSubItem((object) null, writer) : throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
      writer.model.Serialize(key, value, writer);
      ProtoWriter.EndSubItem(token, writer);
    }

    internal static void WriteObject(
      object value,
      int key,
      ProtoWriter writer,
      PrefixStyle style,
      int fieldNumber)
    {
      if (writer.model == null)
        throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
      if (writer.wireType != WireType.None)
        throw ProtoWriter.CreateException(writer);
      switch (style)
      {
        case PrefixStyle.Base128:
          writer.wireType = WireType.String;
          writer.fieldNumber = fieldNumber;
          if (fieldNumber > 0)
          {
            ProtoWriter.WriteHeaderCore(fieldNumber, WireType.String, writer);
            break;
          }
          break;
        case PrefixStyle.Fixed32:
        case PrefixStyle.Fixed32BigEndian:
          writer.fieldNumber = 0;
          writer.wireType = WireType.Fixed32;
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (style));
      }
      SubItemToken token = ProtoWriter.StartSubItem(value, writer, true);
      if (key < 0)
      {
        if (!writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, false, (object) null))
          TypeModel.ThrowUnexpectedType(value.GetType());
      }
      else
        writer.model.Serialize(key, value, writer);
      ProtoWriter.EndSubItem(token, writer, style);
    }

    internal int GetTypeKey(ref Type type) => this.model.GetKey(ref type);

    internal NetObjectCache NetCache => this.netCache;

    internal WireType WireType => this.wireType;

    /// <summary>
    /// Writes a field-header, indicating the format of the next data we plan to write.
    /// </summary>
    public static void WriteFieldHeader(int fieldNumber, WireType wireType, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (writer.wireType != WireType.None)
        throw new InvalidOperationException("Cannot write a " + wireType.ToString() + " header until the " + writer.wireType.ToString() + " data has been written");
      if (fieldNumber < 0)
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      if (writer.packedFieldNumber == 0)
      {
        writer.fieldNumber = fieldNumber;
        writer.wireType = wireType;
        ProtoWriter.WriteHeaderCore(fieldNumber, wireType, writer);
      }
      else
      {
        if (writer.packedFieldNumber != fieldNumber)
          throw new InvalidOperationException("Field mismatch during packed encoding; expected " + writer.packedFieldNumber.ToString() + " but received " + fieldNumber.ToString());
        switch (wireType)
        {
          case WireType.Variant:
          case WireType.Fixed64:
          case WireType.Fixed32:
          case WireType.SignedVariant:
            writer.fieldNumber = fieldNumber;
            writer.wireType = wireType;
            break;
          default:
            throw new InvalidOperationException("Wire-type cannot be encoded as packed: " + wireType.ToString());
        }
      }
    }

    internal static void WriteHeaderCore(int fieldNumber, WireType wireType, ProtoWriter writer)
    {
      ProtoWriter.WriteUInt32Variant((uint) ((WireType) (fieldNumber << 3) | wireType & (WireType.StartGroup | WireType.EndGroup)), writer);
    }

    /// <summary>
    /// Writes a byte-array to the stream; supported wire-types: String
    /// </summary>
    public static void WriteBytes(byte[] data, ProtoWriter writer)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      ProtoWriter.WriteBytes(data, 0, data.Length, writer);
    }

    /// <summary>
    /// Writes a byte-array to the stream; supported wire-types: String
    /// </summary>
    public static void WriteBytes(byte[] data, int offset, int length, ProtoWriter writer)
    {
      if (data == null)
        throw new ArgumentNullException(nameof (data));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Fixed64:
          if (length != 8)
            throw new ArgumentException(nameof (length));
          break;
        case WireType.String:
          ProtoWriter.WriteUInt32Variant((uint) length, writer);
          writer.wireType = WireType.None;
          if (length == 0)
            return;
          if (writer.flushLock == 0 && length > writer.ioBuffer.Length)
          {
            ProtoWriter.Flush(writer);
            writer.dest.Write(data, offset, length);
            writer.position64 += (long) length;
            return;
          }
          break;
        case WireType.Fixed32:
          if (length != 4)
            throw new ArgumentException(nameof (length));
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
      ProtoWriter.DemandSpace(length, writer);
      Buffer.BlockCopy((Array) data, offset, (Array) writer.ioBuffer, writer.ioIndex, length);
      ProtoWriter.IncrementedAndReset(length, writer);
    }

    private static void CopyRawFromStream(Stream source, ProtoWriter writer)
    {
      byte[] ioBuffer = writer.ioBuffer;
      int count1 = ioBuffer.Length - writer.ioIndex;
      int num1 = 1;
      for (; count1 > 0 && (num1 = source.Read(ioBuffer, writer.ioIndex, count1)) > 0; count1 -= num1)
      {
        writer.ioIndex += num1;
        writer.position64 += (long) num1;
      }
      if (num1 <= 0)
        return;
      if (writer.flushLock == 0)
      {
        ProtoWriter.Flush(writer);
        int count2;
        while ((count2 = source.Read(ioBuffer, 0, ioBuffer.Length)) > 0)
        {
          writer.dest.Write(ioBuffer, 0, count2);
          writer.position64 += (long) count2;
        }
      }
      else
      {
        while (true)
        {
          ProtoWriter.DemandSpace(128, writer);
          int num2;
          if ((num2 = source.Read(writer.ioBuffer, writer.ioIndex, writer.ioBuffer.Length - writer.ioIndex)) > 0)
          {
            writer.position64 += (long) num2;
            writer.ioIndex += num2;
          }
          else
            break;
        }
      }
    }

    private static void IncrementedAndReset(int length, ProtoWriter writer)
    {
      writer.ioIndex += length;
      writer.position64 += (long) length;
      writer.wireType = WireType.None;
    }

    /// <summary>Indicates the start of a nested record.</summary>
    /// <param name="instance">The instance to write.</param>
    /// <param name="writer">The destination.</param>
    /// <returns>A token representing the state of the stream; this token is given to EndSubItem.</returns>
    public static SubItemToken StartSubItem(object instance, ProtoWriter writer)
    {
      return ProtoWriter.StartSubItem(instance, writer, false);
    }

    private void CheckRecursionStackAndPush(object instance)
    {
      if (this.recursionStack == null)
      {
        this.recursionStack = new MutableList();
      }
      else
      {
        int num;
        if (instance != null && (num = this.recursionStack.IndexOfReference(instance)) >= 0)
          throw new ProtoException("Possible recursion detected (offset: " + (this.recursionStack.Count - num).ToString() + " level(s)): " + instance.ToString());
      }
      this.recursionStack.Add(instance);
    }

    private void PopRecursionStack() => this.recursionStack.RemoveLast();

    private static SubItemToken StartSubItem(object instance, ProtoWriter writer, bool allowFixed)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (++writer.depth > 25)
        writer.CheckRecursionStackAndPush(instance);
      if (writer.packedFieldNumber != 0)
        throw new InvalidOperationException("Cannot begin a sub-item while performing packed encoding");
      switch (writer.wireType)
      {
        case WireType.String:
          writer.wireType = WireType.None;
          ProtoWriter.DemandSpace(32, writer);
          ++writer.flushLock;
          ++writer.position64;
          return new SubItemToken((long) writer.ioIndex++);
        case WireType.StartGroup:
          writer.wireType = WireType.None;
          return new SubItemToken((long) -writer.fieldNumber);
        case WireType.Fixed32:
          if (!allowFixed)
            throw ProtoWriter.CreateException(writer);
          ProtoWriter.DemandSpace(32, writer);
          ++writer.flushLock;
          SubItemToken subItemToken = new SubItemToken((long) writer.ioIndex);
          ProtoWriter.IncrementedAndReset(4, writer);
          return subItemToken;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>Indicates the end of a nested record.</summary>
    /// <param name="token">The token obtained from StartubItem.</param>
    /// <param name="writer">The destination.</param>
    public static void EndSubItem(SubItemToken token, ProtoWriter writer)
    {
      ProtoWriter.EndSubItem(token, writer, PrefixStyle.Base128);
    }

    private static void EndSubItem(SubItemToken token, ProtoWriter writer, PrefixStyle style)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (writer.wireType != WireType.None)
        throw ProtoWriter.CreateException(writer);
      int index = (int) token.value64;
      if (writer.depth <= 0)
        throw ProtoWriter.CreateException(writer);
      if (writer.depth-- > 25)
        writer.PopRecursionStack();
      writer.packedFieldNumber = 0;
      if (index < 0)
      {
        ProtoWriter.WriteHeaderCore(-index, WireType.EndGroup, writer);
        writer.wireType = WireType.None;
      }
      else
      {
        switch (style)
        {
          case PrefixStyle.Base128:
            int count = writer.ioIndex - index - 1;
            int required = 0;
            uint num1 = (uint) count;
            while ((num1 >>= 7) != 0U)
              ++required;
            if (required == 0)
            {
              writer.ioBuffer[index] = (byte) (count & (int) sbyte.MaxValue);
              break;
            }
            ProtoWriter.DemandSpace(required, writer);
            byte[] ioBuffer1 = writer.ioBuffer;
            Buffer.BlockCopy((Array) ioBuffer1, index + 1, (Array) ioBuffer1, index + 1 + required, count);
            uint num2 = (uint) count;
            do
            {
              ioBuffer1[index++] = (byte) ((int) num2 & (int) sbyte.MaxValue | 128);
            }
            while ((num2 >>= 7) != 0U);
            ioBuffer1[index - 1] = (byte) ((uint) ioBuffer1[index - 1] & 4294967167U);
            writer.position64 += (long) required;
            writer.ioIndex += required;
            break;
          case PrefixStyle.Fixed32:
            ProtoWriter.WriteInt32ToBuffer(writer.ioIndex - index - 4, writer.ioBuffer, index);
            break;
          case PrefixStyle.Fixed32BigEndian:
            int num3 = writer.ioIndex - index - 4;
            byte[] ioBuffer2 = writer.ioBuffer;
            ProtoWriter.WriteInt32ToBuffer(num3, ioBuffer2, index);
            byte num4 = ioBuffer2[index];
            ioBuffer2[index] = ioBuffer2[index + 3];
            ioBuffer2[index + 3] = num4;
            byte num5 = ioBuffer2[index + 1];
            ioBuffer2[index + 1] = ioBuffer2[index + 2];
            ioBuffer2[index + 2] = num5;
            break;
          default:
            throw new ArgumentOutOfRangeException(nameof (style));
        }
        if (--writer.flushLock != 0 || writer.ioIndex < 1024)
          return;
        ProtoWriter.Flush(writer);
      }
    }

    /// <summary>Creates a new writer against a stream</summary>
    /// <param name="dest">The destination stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to serialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    public static ProtoWriter Create(Stream dest, TypeModel model, SerializationContext context = null)
    {
      return new ProtoWriter(dest, model, context);
    }

    /// <summary>Creates a new writer against a stream</summary>
    /// <param name="dest">The destination stream</param>
    /// <param name="model">The model to use for serialization; this can be null, but this will impair the ability to serialize sub-objects</param>
    /// <param name="context">Additional context about this serialization operation</param>
    [Obsolete("Please use ProtoWriter.Create; this API may be removed in a future version", false)]
    public ProtoWriter(Stream dest, TypeModel model, SerializationContext context)
    {
      if (dest == null)
        throw new ArgumentNullException(nameof (dest));
      this.dest = dest.CanWrite ? dest : throw new ArgumentException("Cannot write to stream", nameof (dest));
      this.ioBuffer = BufferPool.GetBuffer();
      this.model = model;
      this.wireType = WireType.None;
      if (context == null)
        context = SerializationContext.Default;
      else
        context.Freeze();
      this.context = context;
    }

    /// <summary>
    /// Addition information about this serialization operation.
    /// </summary>
    public SerializationContext Context => this.context;

    void IDisposable.Dispose() => this.Dispose();

    private void Dispose()
    {
      if (this.dest != null)
      {
        ProtoWriter.Flush(this);
        this.dest = (Stream) null;
      }
      this.model = (TypeModel) null;
      BufferPool.ReleaseBufferToPool(ref this.ioBuffer);
    }

    internal static long GetLongPosition(ProtoWriter writer) => writer.position64;

    internal static int GetPosition(ProtoWriter writer) => checked ((int) writer.position64);

    private static void DemandSpace(int required, ProtoWriter writer)
    {
      if (writer.ioBuffer.Length - writer.ioIndex >= required)
        return;
      ProtoWriter.TryFlushOrResize(required, writer);
    }

    private static void TryFlushOrResize(int required, ProtoWriter writer)
    {
      if (writer.flushLock == 0)
      {
        ProtoWriter.Flush(writer);
        if (writer.ioBuffer.Length - writer.ioIndex >= required)
          return;
      }
      BufferPool.ResizeAndFlushLeft(ref writer.ioBuffer, required + writer.ioIndex, 0, writer.ioIndex);
    }

    /// <summary>
    /// Flushes data to the underlying stream, and releases any resources. The underlying stream is *not* disposed
    /// by this operation.
    /// </summary>
    public void Close()
    {
      if (this.depth != 0 || this.flushLock != 0)
        throw new InvalidOperationException("Unable to close stream in an incomplete state");
      this.Dispose();
    }

    internal void CheckDepthFlushlock()
    {
      if (this.depth != 0 || this.flushLock != 0)
        throw new InvalidOperationException("The writer is in an incomplete state");
    }

    /// <summary>Get the TypeModel associated with this writer</summary>
    public TypeModel Model => this.model;

    /// <summary>
    /// Writes any buffered data (if possible) to the underlying stream.
    /// </summary>
    /// <param name="writer">The writer to flush</param>
    /// <remarks>It is not always possible to fully flush, since some sequences
    /// may require values to be back-filled into the byte-stream.</remarks>
    internal static void Flush(ProtoWriter writer)
    {
      if (writer.flushLock != 0 || writer.ioIndex == 0)
        return;
      writer.dest.Write(writer.ioBuffer, 0, writer.ioIndex);
      writer.ioIndex = 0;
    }

    /// <summary>
    /// Writes an unsigned 32-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    private static void WriteUInt32Variant(uint value, ProtoWriter writer)
    {
      ProtoWriter.DemandSpace(5, writer);
      int num = 0;
      do
      {
        writer.ioBuffer[writer.ioIndex++] = (byte) ((int) value & (int) sbyte.MaxValue | 128);
        ++num;
      }
      while ((value >>= 7) != 0U);
      writer.ioBuffer[writer.ioIndex - 1] &= (byte) 127;
      writer.position64 += (long) num;
    }

    internal static uint Zig(int value) => (uint) (value << 1 ^ value >> 31);

    internal static ulong Zig(long value) => (ulong) (value << 1 ^ value >> 63);

    private static void WriteUInt64Variant(ulong value, ProtoWriter writer)
    {
      ProtoWriter.DemandSpace(10, writer);
      int num = 0;
      do
      {
        writer.ioBuffer[writer.ioIndex++] = (byte) (value & (ulong) sbyte.MaxValue | 128UL);
        ++num;
      }
      while ((value >>= 7) != 0UL);
      writer.ioBuffer[writer.ioIndex - 1] &= (byte) 127;
      writer.position64 += (long) num;
    }

    /// <summary>
    /// Writes a string to the stream; supported wire-types: String
    /// </summary>
    public static void WriteString(string value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (writer.wireType != WireType.String)
        throw ProtoWriter.CreateException(writer);
      switch (value)
      {
        case null:
          throw new ArgumentNullException(nameof (value));
        case "":
          ProtoWriter.WriteUInt32Variant(0U, writer);
          writer.wireType = WireType.None;
          break;
        default:
          int byteCount = ProtoWriter.encoding.GetByteCount(value);
          ProtoWriter.WriteUInt32Variant((uint) byteCount, writer);
          ProtoWriter.DemandSpace(byteCount, writer);
          ProtoWriter.IncrementedAndReset(ProtoWriter.encoding.GetBytes(value, 0, value.Length, writer.ioBuffer, writer.ioIndex), writer);
          break;
      }
    }

    /// <summary>
    /// Writes an unsigned 64-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public static void WriteUInt64(ulong value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Variant:
          ProtoWriter.WriteUInt64Variant(value, writer);
          writer.wireType = WireType.None;
          break;
        case WireType.Fixed64:
          ProtoWriter.WriteInt64((long) value, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteUInt32(checked ((uint) value), writer);
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Writes a signed 64-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public static void WriteInt64(long value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Variant:
          if (value >= 0L)
          {
            ProtoWriter.WriteUInt64Variant((ulong) value, writer);
            writer.wireType = WireType.None;
            break;
          }
          ProtoWriter.DemandSpace(10, writer);
          byte[] ioBuffer1 = writer.ioBuffer;
          int ioIndex1 = writer.ioIndex;
          ioBuffer1[ioIndex1] = (byte) ((ulong) value | 128UL);
          ioBuffer1[ioIndex1 + 1] = (byte) ((uint) (int) (value >> 7) | 128U);
          ioBuffer1[ioIndex1 + 2] = (byte) ((uint) (int) (value >> 14) | 128U);
          ioBuffer1[ioIndex1 + 3] = (byte) ((uint) (int) (value >> 21) | 128U);
          ioBuffer1[ioIndex1 + 4] = (byte) ((uint) (int) (value >> 28) | 128U);
          ioBuffer1[ioIndex1 + 5] = (byte) ((uint) (int) (value >> 35) | 128U);
          ioBuffer1[ioIndex1 + 6] = (byte) ((uint) (int) (value >> 42) | 128U);
          ioBuffer1[ioIndex1 + 7] = (byte) ((uint) (int) (value >> 49) | 128U);
          ioBuffer1[ioIndex1 + 8] = (byte) ((uint) (int) (value >> 56) | 128U);
          ioBuffer1[ioIndex1 + 9] = (byte) 1;
          ProtoWriter.IncrementedAndReset(10, writer);
          break;
        case WireType.Fixed64:
          ProtoWriter.DemandSpace(8, writer);
          byte[] ioBuffer2 = writer.ioBuffer;
          int ioIndex2 = writer.ioIndex;
          ioBuffer2[ioIndex2] = (byte) value;
          ioBuffer2[ioIndex2 + 1] = (byte) (value >> 8);
          ioBuffer2[ioIndex2 + 2] = (byte) (value >> 16);
          ioBuffer2[ioIndex2 + 3] = (byte) (value >> 24);
          ioBuffer2[ioIndex2 + 4] = (byte) (value >> 32);
          ioBuffer2[ioIndex2 + 5] = (byte) (value >> 40);
          ioBuffer2[ioIndex2 + 6] = (byte) (value >> 48);
          ioBuffer2[ioIndex2 + 7] = (byte) (value >> 56);
          ProtoWriter.IncrementedAndReset(8, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteInt32(checked ((int) value), writer);
          break;
        case WireType.SignedVariant:
          ProtoWriter.WriteUInt64Variant(ProtoWriter.Zig(value), writer);
          writer.wireType = WireType.None;
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Writes an unsigned 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public static void WriteUInt32(uint value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Variant:
          ProtoWriter.WriteUInt32Variant(value, writer);
          writer.wireType = WireType.None;
          break;
        case WireType.Fixed64:
          ProtoWriter.WriteInt64((long) (int) value, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteInt32((int) value, writer);
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Writes a signed 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public static void WriteInt16(short value, ProtoWriter writer)
    {
      ProtoWriter.WriteInt32((int) value, writer);
    }

    /// <summary>
    /// Writes an unsigned 16-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public static void WriteUInt16(ushort value, ProtoWriter writer)
    {
      ProtoWriter.WriteUInt32((uint) value, writer);
    }

    /// <summary>
    /// Writes an unsigned 8-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public static void WriteByte(byte value, ProtoWriter writer)
    {
      ProtoWriter.WriteUInt32((uint) value, writer);
    }

    /// <summary>
    /// Writes a signed 8-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public static void WriteSByte(sbyte value, ProtoWriter writer)
    {
      ProtoWriter.WriteInt32((int) value, writer);
    }

    private static void WriteInt32ToBuffer(int value, byte[] buffer, int index)
    {
      buffer[index] = (byte) value;
      buffer[index + 1] = (byte) (value >> 8);
      buffer[index + 2] = (byte) (value >> 16);
      buffer[index + 3] = (byte) (value >> 24);
    }

    /// <summary>
    /// Writes a signed 32-bit integer to the stream; supported wire-types: Variant, Fixed32, Fixed64, SignedVariant
    /// </summary>
    public static void WriteInt32(int value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Variant:
          if (value >= 0)
          {
            ProtoWriter.WriteUInt32Variant((uint) value, writer);
            writer.wireType = WireType.None;
            break;
          }
          ProtoWriter.DemandSpace(10, writer);
          byte[] ioBuffer1 = writer.ioBuffer;
          int ioIndex1 = writer.ioIndex;
          ioBuffer1[ioIndex1] = (byte) (value | 128);
          ioBuffer1[ioIndex1 + 1] = (byte) (value >> 7 | 128);
          ioBuffer1[ioIndex1 + 2] = (byte) (value >> 14 | 128);
          ioBuffer1[ioIndex1 + 3] = (byte) (value >> 21 | 128);
          ioBuffer1[ioIndex1 + 4] = (byte) (value >> 28 | 128);
          ioBuffer1[ioIndex1 + 5] = ioBuffer1[ioIndex1 + 6] = ioBuffer1[ioIndex1 + 7] = ioBuffer1[ioIndex1 + 8] = byte.MaxValue;
          ioBuffer1[ioIndex1 + 9] = (byte) 1;
          ProtoWriter.IncrementedAndReset(10, writer);
          break;
        case WireType.Fixed64:
          ProtoWriter.DemandSpace(8, writer);
          byte[] ioBuffer2 = writer.ioBuffer;
          int ioIndex2 = writer.ioIndex;
          ioBuffer2[ioIndex2] = (byte) value;
          ioBuffer2[ioIndex2 + 1] = (byte) (value >> 8);
          ioBuffer2[ioIndex2 + 2] = (byte) (value >> 16);
          ioBuffer2[ioIndex2 + 3] = (byte) (value >> 24);
          ioBuffer2[ioIndex2 + 4] = ioBuffer2[ioIndex2 + 5] = ioBuffer2[ioIndex2 + 6] = ioBuffer2[ioIndex2 + 7] = (byte) 0;
          ProtoWriter.IncrementedAndReset(8, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.DemandSpace(4, writer);
          ProtoWriter.WriteInt32ToBuffer(value, writer.ioBuffer, writer.ioIndex);
          ProtoWriter.IncrementedAndReset(4, writer);
          break;
        case WireType.SignedVariant:
          ProtoWriter.WriteUInt32Variant(ProtoWriter.Zig(value), writer);
          writer.wireType = WireType.None;
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Writes a double-precision number to the stream; supported wire-types: Fixed32, Fixed64
    /// </summary>
    public static unsafe void WriteDouble(double value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Fixed64:
          ProtoWriter.WriteInt64(*(long*) &value, writer);
          break;
        case WireType.Fixed32:
          float f = (float) value;
          if (float.IsInfinity(f) && !double.IsInfinity(value))
            throw new OverflowException();
          ProtoWriter.WriteSingle(f, writer);
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Writes a single-precision number to the stream; supported wire-types: Fixed32, Fixed64
    /// </summary>
    public static unsafe void WriteSingle(float value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      switch (writer.wireType)
      {
        case WireType.Fixed64:
          ProtoWriter.WriteDouble((double) value, writer);
          break;
        case WireType.Fixed32:
          ProtoWriter.WriteInt32(*(int*) &value, writer);
          break;
        default:
          throw ProtoWriter.CreateException(writer);
      }
    }

    /// <summary>
    /// Throws an exception indicating that the given enum cannot be mapped to a serialized value.
    /// </summary>
    public static void ThrowEnumException(ProtoWriter writer, object enumValue)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      throw new ProtoException("No wire-value is mapped to the enum " + (enumValue == null ? "<null>" : enumValue.GetType().FullName + "." + enumValue.ToString()) + " at position " + writer.position64.ToString());
    }

    internal static Exception CreateException(ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      return (Exception) new ProtoException("Invalid serialization operation with wire-type " + writer.wireType.ToString() + " at position " + writer.position64.ToString());
    }

    /// <summary>
    /// Writes a boolean to the stream; supported wire-types: Variant, Fixed32, Fixed64
    /// </summary>
    public static void WriteBoolean(bool value, ProtoWriter writer)
    {
      ProtoWriter.WriteUInt32(value ? 1U : 0U, writer);
    }

    /// <summary>
    /// Copies any extension data stored for the instance to the underlying stream
    /// </summary>
    public static void AppendExtensionData(IExtensible instance, ProtoWriter writer)
    {
      if (instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if (writer.wireType != WireType.None)
        throw ProtoWriter.CreateException(writer);
      IExtension extensionObject = instance.GetExtensionObject(false);
      if (extensionObject == null)
        return;
      Stream stream = extensionObject.BeginQuery();
      try
      {
        ProtoWriter.CopyRawFromStream(stream, writer);
      }
      finally
      {
        extensionObject.EndQuery(stream);
      }
    }

    /// <summary>
    /// Used for packed encoding; indicates that the next field should be skipped rather than
    /// a field header written. Note that the field number must match, else an exception is thrown
    /// when the attempt is made to write the (incorrect) field. The wire-type is taken from the
    /// subsequent call to WriteFieldHeader. Only primitive types can be packed.
    /// </summary>
    public static void SetPackedField(int fieldNumber, ProtoWriter writer)
    {
      if (fieldNumber <= 0)
        throw new ArgumentOutOfRangeException(nameof (fieldNumber));
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      writer.packedFieldNumber = fieldNumber;
    }

    /// <summary>
    /// Used for packed encoding; explicitly reset the packed field marker; this is not required
    /// if using StartSubItem/EndSubItem
    /// </summary>
    public static void ClearPackedField(int fieldNumber, ProtoWriter writer)
    {
      if (fieldNumber != writer.packedFieldNumber)
        throw new InvalidOperationException("Field mismatch during packed encoding; expected " + writer.packedFieldNumber.ToString() + " but received " + fieldNumber.ToString());
      writer.packedFieldNumber = 0;
    }

    /// <summary>
    /// Used for packed encoding; writes the length prefix using fixed sizes rather than using
    /// buffering. Only valid for fixed-32 and fixed-64 encoding.
    /// </summary>
    public static void WritePackedPrefix(int elementCount, WireType wireType, ProtoWriter writer)
    {
      if (writer.WireType != WireType.String)
        throw new InvalidOperationException("Invalid wire-type: " + (object) writer.WireType);
      if (elementCount < 0)
        throw new ArgumentOutOfRangeException(nameof (elementCount));
      ulong num;
      if (wireType != WireType.Fixed64)
      {
        if (wireType != WireType.Fixed32)
          throw new ArgumentOutOfRangeException(nameof (wireType), "Invalid wire-type: " + (object) wireType);
        num = (ulong) elementCount << 2;
      }
      else
        num = (ulong) elementCount << 3;
      ProtoWriter.WriteUInt64Variant(num, writer);
      writer.wireType = WireType.None;
    }

    internal string SerializeType(Type type) => TypeModel.SerializeType(this.model, type);

    /// <summary>
    /// Specifies a known root object to use during reference-tracked serialization
    /// </summary>
    public void SetRootObject(object value) => this.NetCache.SetKeyedObject(0, value);

    /// <summary>
    /// Writes a Type to the stream, using the model's DynamicTypeFormatting if appropriate; supported wire-types: String
    /// </summary>
    public static void WriteType(Type value, ProtoWriter writer)
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      ProtoWriter.WriteString(writer.SerializeType(value), writer);
    }
  }
}
