// Decompiled with JetBrains decompiler
// Type: ProtoBuf.BufferExtension
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;
using System.IO;

#nullable disable
namespace ProtoBuf
{
  /// <summary>
  /// Provides a simple buffer-based implementation of an <see cref="T:ProtoBuf.IExtension">extension</see> object.
  /// </summary>
  public sealed class BufferExtension : IExtension, IExtensionResettable
  {
    private byte[] buffer;

    void IExtensionResettable.Reset() => this.buffer = (byte[]) null;

    int IExtension.GetLength() => this.buffer != null ? this.buffer.Length : 0;

    Stream IExtension.BeginAppend() => (Stream) new MemoryStream();

    void IExtension.EndAppend(Stream stream, bool commit)
    {
      using (stream)
      {
        int length1;
        if (!commit || (length1 = (int) stream.Length) <= 0)
          return;
        MemoryStream ms = (MemoryStream) stream;
        if (this.buffer == null)
        {
          this.buffer = ms.ToArray();
        }
        else
        {
          int length2 = this.buffer.Length;
          byte[] dst = new byte[length2 + length1];
          Buffer.BlockCopy((Array) this.buffer, 0, (Array) dst, 0, length2);
          Buffer.BlockCopy((Array) Helpers.GetBuffer(ms), 0, (Array) dst, length2, length1);
          this.buffer = dst;
        }
      }
    }

    Stream IExtension.BeginQuery()
    {
      return this.buffer != null ? (Stream) new MemoryStream(this.buffer) : Stream.Null;
    }

    void IExtension.EndQuery(Stream stream)
    {
      using (stream)
        ;
    }
  }
}
