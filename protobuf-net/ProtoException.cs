// Decompiled with JetBrains decompiler
// Type: ProtoBuf.ProtoException
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
  /// Indicates an error during serialization/deserialization of a proto stream.
  /// </summary>
  [Serializable]
  public class ProtoException : Exception
  {
    /// <summary>Creates a new ProtoException instance.</summary>
    public ProtoException()
    {
    }

    /// <summary>Creates a new ProtoException instance.</summary>
    public ProtoException(string message)
      : base(message)
    {
    }

    /// <summary>Creates a new ProtoException instance.</summary>
    public ProtoException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>Creates a new ProtoException instance.</summary>
    protected ProtoException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
    }
  }
}
