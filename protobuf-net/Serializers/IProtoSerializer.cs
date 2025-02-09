// Decompiled with JetBrains decompiler
// Type: ProtoBuf.Serializers.IProtoSerializer
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using ProtoBuf.Compiler;
using System;

#nullable disable
namespace ProtoBuf.Serializers
{
  internal interface IProtoSerializer
  {
    /// <summary>
    /// The type that this serializer is intended to work for.
    /// </summary>
    Type ExpectedType { get; }

    /// <summary>Perform the steps necessary to serialize this data.</summary>
    /// <param name="value">The value to be serialized.</param>
    /// <param name="dest">The writer entity that is accumulating the output data.</param>
    void Write(object value, ProtoWriter dest);

    /// <summary>Perform the steps necessary to deserialize this data.</summary>
    /// <param name="value">The current value, if appropriate.</param>
    /// <param name="source">The reader providing the input data.</param>
    /// <returns>The updated / replacement value.</returns>
    object Read(object value, ProtoReader source);

    /// <summary>
    /// Indicates whether a Read operation <em>replaces</em> the existing value, or
    /// <em>extends</em> the value. If false, the "value" parameter to Read is
    /// discarded, and should be passed in as null.
    /// </summary>
    bool RequiresOldValue { get; }

    /// <summary>
    /// Now all Read operations return a value (although most do); if false no
    /// value should be expected.
    /// </summary>
    bool ReturnsValue { get; }

    /// <summary>Emit the IL necessary to perform the given actions
    /// to serialize this data.
    /// </summary>
    /// <param name="ctx">Details and utilities for the method being generated.</param>
    /// <param name="valueFrom">The source of the data to work against;
    /// If the value is only needed once, then LoadValue is sufficient. If
    /// the value is needed multiple times, then note that a "null"
    /// means "the top of the stack", in which case you should create your
    /// own copy - GetLocalWithValue.</param>
    void EmitWrite(CompilerContext ctx, Local valueFrom);

    /// <summary>
    /// Emit the IL necessary to perform the given actions to deserialize this data.
    /// </summary>
    /// <param name="ctx">Details and utilities for the method being generated.</param>
    /// <param name="entity">For nested values, the instance holding the values; note
    /// that this is not always provided - a null means not supplied. Since this is always
    /// a variable or argument, it is not necessary to consume this value.</param>
    void EmitRead(CompilerContext ctx, Local entity);
  }
}
