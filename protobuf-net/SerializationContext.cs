// Decompiled with JetBrains decompiler
// Type: ProtoBuf.SerializationContext
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Additional information about a serialization operation
  /// </summary>
  public sealed class SerializationContext
  {
    private bool frozen;
    private object context;
    private static readonly SerializationContext @default = new SerializationContext();
    private StreamingContextStates state = StreamingContextStates.Persistence;

    internal void Freeze() => this.frozen = true;

    private void ThrowIfFrozen()
    {
      if (this.frozen)
        throw new InvalidOperationException("The serialization-context cannot be changed once it is in use");
    }

    /// <summary>
    /// Gets or sets a user-defined object containing additional information about this serialization/deserialization operation.
    /// </summary>
    public object Context
    {
      get => this.context;
      set
      {
        if (this.context == value)
          return;
        this.ThrowIfFrozen();
        this.context = value;
      }
    }

    static SerializationContext() => SerializationContext.@default.Freeze();

    /// <summary>
    /// A default SerializationContext, with minimal information.
    /// </summary>
    internal static SerializationContext Default => SerializationContext.@default;

    /// <summary>
    /// Gets or sets the source or destination of the transmitted data.
    /// </summary>
    public StreamingContextStates State
    {
      get => this.state;
      set
      {
        if (this.state == value)
          return;
        this.ThrowIfFrozen();
        this.state = value;
      }
    }

    /// <summary>Convert a SerializationContext to a StreamingContext</summary>
    public static implicit operator StreamingContext(SerializationContext ctx)
    {
      return ctx == null ? new StreamingContext(StreamingContextStates.Persistence) : new StreamingContext(ctx.state, ctx.context);
    }

    /// <summary>Convert a StreamingContext to a SerializationContext</summary>
    public static implicit operator SerializationContext(StreamingContext ctx)
    {
      return new SerializationContext()
      {
        Context = ctx.Context,
        State = ctx.State
      };
    }
  }
}
