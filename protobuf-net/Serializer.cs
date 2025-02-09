// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Provides protocol-buffer serialization capability for concrete, attributed types. This
  /// is a *default* model, but custom serializer models are also supported.
  /// </summary>
  /// <remarks>
  /// Protocol-buffer serialization is a compact binary format, designed to take
  /// advantage of sparse data and knowledge of specific data types; it is also
  /// extensible, allowing a type to be deserialized / merged even if some data is
  /// not recognised.
  /// </remarks>
  public static class Serializer
  {
    private const string ProtoBinaryField = "proto";
    /// <summary>
    /// The field number that is used as a default when serializing/deserializing a list of objects.
    /// The data is treated as repeated message with field number 1.
    /// </summary>
    public const int ListItemTag = 1;

    /// <summary>Suggest a .proto definition for the given type</summary>
    /// <typeparam name="T">The type to generate a .proto definition for</typeparam>
    /// <returns>The .proto definition as a string</returns>
    public static string GetProto<T>() => Serializer.GetProto<T>(ProtoSyntax.Proto2);

    /// <summary>Suggest a .proto definition for the given type</summary>
    /// <typeparam name="T">The type to generate a .proto definition for</typeparam>
    /// <returns>The .proto definition as a string</returns>
    public static string GetProto<T>(ProtoSyntax syntax)
    {
      return RuntimeTypeModel.Default.GetSchema(RuntimeTypeModel.Default.MapType(typeof (T)), syntax);
    }

    /// <summary>
    /// Create a deep clone of the supplied instance; any sub-items are also cloned.
    /// </summary>
    public static T DeepClone<T>(T instance)
    {
      return (object) instance != null ? (T) RuntimeTypeModel.Default.DeepClone((object) instance) : instance;
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance.
    /// </summary>
    /// <typeparam name="T">The type being merged.</typeparam>
    /// <param name="instance">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public static T Merge<T>(Stream source, T instance)
    {
      return (T) RuntimeTypeModel.Default.Deserialize(source, (object) instance, typeof (T));
    }

    /// <summary>Creates a new instance from a protocol-buffer stream</summary>
    /// <typeparam name="T">The type to be created.</typeparam>
    /// <param name="source">The binary stream to apply to the new instance (cannot be null).</param>
    /// <returns>A new, initialized instance.</returns>
    public static T Deserialize<T>(Stream source)
    {
      return (T) RuntimeTypeModel.Default.Deserialize(source, (object) null, typeof (T));
    }

    /// <summary>Creates a new instance from a protocol-buffer stream</summary>
    /// <param name="type">The type to be created.</param>
    /// <param name="source">The binary stream to apply to the new instance (cannot be null).</param>
    /// <returns>A new, initialized instance.</returns>
    public static object Deserialize(Type type, Stream source)
    {
      return RuntimeTypeModel.Default.Deserialize(source, (object) null, type);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream.
    /// </summary>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="destination">The destination stream to write to.</param>
    public static void Serialize<T>(Stream destination, T instance)
    {
      if ((object) instance == null)
        return;
      RuntimeTypeModel.Default.Serialize(destination, (object) instance);
    }

    /// <summary>
    /// Serializes a given instance and deserializes it as a different type;
    /// this can be used to translate between wire-compatible objects (where
    /// two .NET types represent the same data), or to promote/demote a type
    /// through an inheritance hierarchy.
    /// </summary>
    /// <remarks>No assumption of compatibility is made between the types.</remarks>
    /// <typeparam name="TFrom">The type of the object being copied.</typeparam>
    /// <typeparam name="TTo">The type of the new object to be created.</typeparam>
    /// <param name="instance">The existing instance to use as a template.</param>
    /// <returns>A new instane of type TNewType, with the data from TOldType.</returns>
    public static TTo ChangeType<TFrom, TTo>(TFrom instance)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Serializer.Serialize<TFrom>((Stream) memoryStream, instance);
        memoryStream.Position = 0L;
        return Serializer.Deserialize<TTo>((Stream) memoryStream);
      }
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied SerializationInfo.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="info">The destination SerializationInfo to write to.</param>
    public static void Serialize<T>(SerializationInfo info, T instance) where T : class, ISerializable
    {
      Serializer.Serialize<T>(info, new StreamingContext(StreamingContextStates.Persistence), instance);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied SerializationInfo.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="info">The destination SerializationInfo to write to.</param>
    /// <param name="context">Additional information about this serialization operation.</param>
    public static void Serialize<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (instance.GetType() != typeof (T))
        throw new ArgumentException("Incorrect type", nameof (instance));
      using (MemoryStream dest = new MemoryStream())
      {
        RuntimeTypeModel.Default.Serialize((Stream) dest, (object) instance, (SerializationContext) context);
        info.AddValue("proto", (object) dest.ToArray());
      }
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied XmlWriter.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="writer">The destination XmlWriter to write to.</param>
    public static void Serialize<T>(XmlWriter writer, T instance) where T : IXmlSerializable
    {
      if (writer == null)
        throw new ArgumentNullException(nameof (writer));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Serializer.Serialize<T>((Stream) memoryStream, instance);
        writer.WriteBase64(Helpers.GetBuffer(memoryStream), 0, (int) memoryStream.Length);
      }
    }

    /// <summary>
    /// Applies a protocol-buffer from an XmlReader to an existing instance.
    /// </summary>
    /// <typeparam name="T">The type being merged.</typeparam>
    /// <param name="instance">The existing instance to be modified (cannot be null).</param>
    /// <param name="reader">The XmlReader containing the data to apply to the instance (cannot be null).</param>
    public static void Merge<T>(XmlReader reader, T instance) where T : IXmlSerializable
    {
      if (reader == null)
        throw new ArgumentNullException(nameof (reader));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      byte[] buffer = new byte[4096];
      using (MemoryStream source = new MemoryStream())
      {
        int depth = reader.Depth;
        while (reader.Read() && reader.Depth > depth)
        {
          if (reader.NodeType == XmlNodeType.Text)
          {
            int count;
            while ((count = reader.ReadContentAsBase64(buffer, 0, 4096)) > 0)
              source.Write(buffer, 0, count);
            if (reader.Depth <= depth)
              break;
          }
        }
        source.Position = 0L;
        Serializer.Merge<T>((Stream) source, instance);
      }
    }

    /// <summary>
    /// Applies a protocol-buffer from a SerializationInfo to an existing instance.
    /// </summary>
    /// <typeparam name="T">The type being merged.</typeparam>
    /// <param name="instance">The existing instance to be modified (cannot be null).</param>
    /// <param name="info">The SerializationInfo containing the data to apply to the instance (cannot be null).</param>
    public static void Merge<T>(SerializationInfo info, T instance) where T : class, ISerializable
    {
      Serializer.Merge<T>(info, new StreamingContext(StreamingContextStates.Persistence), instance);
    }

    /// <summary>
    /// Applies a protocol-buffer from a SerializationInfo to an existing instance.
    /// </summary>
    /// <typeparam name="T">The type being merged.</typeparam>
    /// <param name="instance">The existing instance to be modified (cannot be null).</param>
    /// <param name="info">The SerializationInfo containing the data to apply to the instance (cannot be null).</param>
    /// <param name="context">Additional information about this serialization operation.</param>
    public static void Merge<T>(SerializationInfo info, StreamingContext context, T instance) where T : class, ISerializable
    {
      if (info == null)
        throw new ArgumentNullException(nameof (info));
      if ((object) instance == null)
        throw new ArgumentNullException(nameof (instance));
      if (instance.GetType() != typeof (T))
        throw new ArgumentException("Incorrect type", nameof (instance));
      using (MemoryStream source = new MemoryStream((byte[]) info.GetValue("proto", typeof (byte[]))))
      {
        if ((object) (T) RuntimeTypeModel.Default.Deserialize((Stream) source, (object) instance, typeof (T), (SerializationContext) context) != (object) instance)
          throw new ProtoException("Deserialization changed the instance; cannot succeed.");
      }
    }

    /// <summary>Precompiles the serializer for a given type.</summary>
    public static void PrepareSerializer<T>()
    {
      Serializer.NonGeneric.PrepareSerializer(typeof (T));
    }

    /// <summary>
    /// Creates a new IFormatter that uses protocol-buffer [de]serialization.
    /// </summary>
    /// <typeparam name="T">The type of object to be [de]deserialized by the formatter.</typeparam>
    /// <returns>A new IFormatter to be used during [de]serialization.</returns>
    public static IFormatter CreateFormatter<T>()
    {
      return RuntimeTypeModel.Default.CreateFormatter(typeof (T));
    }

    /// <summary>
    /// Reads a sequence of consecutive length-prefixed items from a stream, using
    /// either base-128 or fixed-length prefixes. Base-128 prefixes with a tag
    /// are directly comparable to serializing multiple items in succession
    /// (use the <see cref="F:ProtoBuf.Serializer.ListItemTag" /> tag to emulate the implicit behavior
    /// when serializing a list/array). When a tag is
    /// specified, any records with different tags are silently omitted. The
    /// tag is ignored. The tag is ignored for fixed-length prefixes.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="source">The binary stream containing the serialized records.</param>
    /// <param name="style">The prefix style used in the data.</param>
    /// <param name="fieldNumber">The tag of records to return (if non-positive, then no tag is
    /// expected and all records are returned).</param>
    /// <returns>The sequence of deserialized objects.</returns>
    public static IEnumerable<T> DeserializeItems<T>(
      Stream source,
      PrefixStyle style,
      int fieldNumber)
    {
      return RuntimeTypeModel.Default.DeserializeItems<T>(source, style, fieldNumber);
    }

    /// <summary>
    /// Creates a new instance from a protocol-buffer stream that has a length-prefix
    /// on data (to assist with network IO).
    /// </summary>
    /// <typeparam name="T">The type to be created.</typeparam>
    /// <param name="source">The binary stream to apply to the new instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <returns>A new, initialized instance.</returns>
    public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style)
    {
      return Serializer.DeserializeWithLengthPrefix<T>(source, style, 0);
    }

    /// <summary>
    /// Creates a new instance from a protocol-buffer stream that has a length-prefix
    /// on data (to assist with network IO).
    /// </summary>
    /// <typeparam name="T">The type to be created.</typeparam>
    /// <param name="source">The binary stream to apply to the new instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="fieldNumber">The expected tag of the item (only used with base-128 prefix style).</param>
    /// <returns>A new, initialized instance.</returns>
    public static T DeserializeWithLengthPrefix<T>(
      Stream source,
      PrefixStyle style,
      int fieldNumber)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      return (T) runtimeTypeModel.DeserializeWithLengthPrefix(source, (object) null, runtimeTypeModel.MapType(typeof (T)), style, fieldNumber);
    }

    /// <summary>
    /// Applies a protocol-buffer stream to an existing instance, using length-prefixed
    /// data - useful with network IO.
    /// </summary>
    /// <typeparam name="T">The type being merged.</typeparam>
    /// <param name="instance">The existing instance to be modified (can be null).</param>
    /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <returns>The updated instance; this may be different to the instance argument if
    /// either the original instance was null, or the stream defines a known sub-type of the
    /// original instance.</returns>
    public static T MergeWithLengthPrefix<T>(Stream source, T instance, PrefixStyle style)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      return (T) runtimeTypeModel.DeserializeWithLengthPrefix(source, (object) instance, runtimeTypeModel.MapType(typeof (T)), style, 0);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream,
    /// with a length-prefix. This is useful for socket programming,
    /// as DeserializeWithLengthPrefix/MergeWithLengthPrefix can be used to read the single object back
    /// from an ongoing stream.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="destination">The destination stream to write to.</param>
    public static void SerializeWithLengthPrefix<T>(
      Stream destination,
      T instance,
      PrefixStyle style)
    {
      Serializer.SerializeWithLengthPrefix<T>(destination, instance, style, 0);
    }

    /// <summary>
    /// Writes a protocol-buffer representation of the given instance to the supplied stream,
    /// with a length-prefix. This is useful for socket programming,
    /// as DeserializeWithLengthPrefix/MergeWithLengthPrefix can be used to read the single object back
    /// from an ongoing stream.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
    /// <param name="style">How to encode the length prefix.</param>
    /// <param name="destination">The destination stream to write to.</param>
    /// <param name="fieldNumber">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
    public static void SerializeWithLengthPrefix<T>(
      Stream destination,
      T instance,
      PrefixStyle style,
      int fieldNumber)
    {
      RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
      runtimeTypeModel.SerializeWithLengthPrefix(destination, (object) instance, runtimeTypeModel.MapType(typeof (T)), style, fieldNumber);
    }

    /// <summary>Indicates the number of bytes expected for the next message.</summary>
    /// <param name="source">The stream containing the data to investigate for a length.</param>
    /// <param name="style">The algorithm used to encode the length.</param>
    /// <param name="length">The length of the message, if it could be identified.</param>
    /// <returns>True if a length could be obtained, false otherwise.</returns>
    public static bool TryReadLengthPrefix(Stream source, PrefixStyle style, out int length)
    {
      int bytesRead;
      length = ProtoReader.ReadLengthPrefix(source, false, style, out int _, out bytesRead);
      return bytesRead > 0;
    }

    /// <summary>Indicates the number of bytes expected for the next message.</summary>
    /// <param name="buffer">The buffer containing the data to investigate for a length.</param>
    /// <param name="index">The offset of the first byte to read from the buffer.</param>
    /// <param name="count">The number of bytes to read from the buffer.</param>
    /// <param name="style">The algorithm used to encode the length.</param>
    /// <param name="length">The length of the message, if it could be identified.</param>
    /// <returns>True if a length could be obtained, false otherwise.</returns>
    public static bool TryReadLengthPrefix(
      byte[] buffer,
      int index,
      int count,
      PrefixStyle style,
      out int length)
    {
      using (Stream source = (Stream) new MemoryStream(buffer, index, count))
        return Serializer.TryReadLengthPrefix(source, style, out length);
    }

    /// <summary>
    /// Releases any internal buffers that have been reserved for efficiency; this does not affect any serialization
    /// operations; simply: it can be used (optionally) to release the buffers for garbage collection (at the expense
    /// of having to re-allocate a new buffer for the next operation, rather than re-use prior buffers).
    /// </summary>
    public static void FlushPool() => BufferPool.Flush();

    /// <summary>
    /// Provides non-generic access to the default serializer.
    /// </summary>
    public static class NonGeneric
    {
      /// <summary>
      /// Create a deep clone of the supplied instance; any sub-items are also cloned.
      /// </summary>
      public static object DeepClone(object instance)
      {
        return instance != null ? RuntimeTypeModel.Default.DeepClone(instance) : (object) null;
      }

      /// <summary>
      /// Writes a protocol-buffer representation of the given instance to the supplied stream.
      /// </summary>
      /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
      /// <param name="dest">The destination stream to write to.</param>
      public static void Serialize(Stream dest, object instance)
      {
        if (instance == null)
          return;
        RuntimeTypeModel.Default.Serialize(dest, instance);
      }

      /// <summary>Creates a new instance from a protocol-buffer stream</summary>
      /// <param name="type">The type to be created.</param>
      /// <param name="source">The binary stream to apply to the new instance (cannot be null).</param>
      /// <returns>A new, initialized instance.</returns>
      public static object Deserialize(Type type, Stream source)
      {
        return RuntimeTypeModel.Default.Deserialize(source, (object) null, type);
      }

      /// <summary>Applies a protocol-buffer stream to an existing instance.</summary>
      /// <param name="instance">The existing instance to be modified (cannot be null).</param>
      /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
      /// <returns>The updated instance</returns>
      public static object Merge(Stream source, object instance)
      {
        if (instance == null)
          throw new ArgumentNullException(nameof (instance));
        return RuntimeTypeModel.Default.Deserialize(source, instance, instance.GetType(), (SerializationContext) null);
      }

      /// <summary>
      /// Writes a protocol-buffer representation of the given instance to the supplied stream,
      /// with a length-prefix. This is useful for socket programming,
      /// as DeserializeWithLengthPrefix/MergeWithLengthPrefix can be used to read the single object back
      /// from an ongoing stream.
      /// </summary>
      /// <param name="instance">The existing instance to be serialized (cannot be null).</param>
      /// <param name="style">How to encode the length prefix.</param>
      /// <param name="destination">The destination stream to write to.</param>
      /// <param name="fieldNumber">The tag used as a prefix to each record (only used with base-128 style prefixes).</param>
      public static void SerializeWithLengthPrefix(
        Stream destination,
        object instance,
        PrefixStyle style,
        int fieldNumber)
      {
        if (instance == null)
          throw new ArgumentNullException(nameof (instance));
        RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
        runtimeTypeModel.SerializeWithLengthPrefix(destination, instance, runtimeTypeModel.MapType(instance.GetType()), style, fieldNumber);
      }

      /// <summary>
      /// Applies a protocol-buffer stream to an existing instance (or null), using length-prefixed
      /// data - useful with network IO.
      /// </summary>
      /// <param name="value">The existing instance to be modified (can be null).</param>
      /// <param name="source">The binary stream to apply to the instance (cannot be null).</param>
      /// <param name="style">How to encode the length prefix.</param>
      /// <param name="resolver">Used to resolve types on a per-field basis.</param>
      /// <returns>The updated instance; this may be different to the instance argument if
      /// either the original instance was null, or the stream defines a known sub-type of the
      /// original instance.</returns>
      public static bool TryDeserializeWithLengthPrefix(
        Stream source,
        PrefixStyle style,
        Serializer.TypeResolver resolver,
        out object value)
      {
        value = RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, (object) null, (Type) null, style, 0, resolver);
        return value != null;
      }

      /// <summary>
      /// Indicates whether the supplied type is explicitly modelled by the model
      /// </summary>
      public static bool CanSerialize(Type type) => RuntimeTypeModel.Default.IsDefined(type);

      /// <summary>Precompiles the serializer for a given type.</summary>
      public static void PrepareSerializer(Type t)
      {
        RuntimeTypeModel runtimeTypeModel = RuntimeTypeModel.Default;
        runtimeTypeModel[runtimeTypeModel.MapType(t)].CompileInPlace();
      }
    }

    /// <summary>
    /// Global switches that change the behavior of protobuf-net
    /// </summary>
    public static class GlobalOptions
    {
      /// <summary>
      /// <see cref="P:ProtoBuf.Meta.RuntimeTypeModel.InferTagFromNameDefault" />
      /// </summary>
      [Obsolete("Please use RuntimeTypeModel.Default.InferTagFromNameDefault instead (or on a per-model basis)", false)]
      public static bool InferTagFromName
      {
        get => RuntimeTypeModel.Default.InferTagFromNameDefault;
        set => RuntimeTypeModel.Default.InferTagFromNameDefault = value;
      }
    }

    /// <summary>Maps a field-number to a type</summary>
    public delegate Type TypeResolver(int fieldNumber);
  }
}
