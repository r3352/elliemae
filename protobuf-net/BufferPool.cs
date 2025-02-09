// Decompiled with JetBrains decompiler
// Type: ProtoBuf.BufferPool
// Assembly: protobuf-net, Version=2.4.0.0, Culture=neutral, PublicKeyToken=257b51d87d2e4d67
// MVID: 4CB0BA27-6305-4C8A-8F23-3BBBCE6B7DCC
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\protobuf-net.xml

using System;

#nullable disable
namespace ProtoBuf
{
  internal sealed class BufferPool
  {
    private const int POOL_SIZE = 20;
    internal const int BUFFER_LENGTH = 1024;
    private static readonly BufferPool.CachedBuffer[] Pool = new BufferPool.CachedBuffer[20];
    /// <remarks>
    /// https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/file-schema/runtime/gcallowverylargeobjects-element
    /// </remarks>
    private const int MaxByteArraySize = 2147483591;

    internal static void Flush()
    {
      lock (BufferPool.Pool)
      {
        for (int index = 0; index < BufferPool.Pool.Length; ++index)
          BufferPool.Pool[index] = (BufferPool.CachedBuffer) null;
      }
    }

    private BufferPool()
    {
    }

    internal static byte[] GetBuffer() => BufferPool.GetBuffer(1024);

    internal static byte[] GetBuffer(int minSize)
    {
      return BufferPool.GetCachedBuffer(minSize) ?? new byte[minSize];
    }

    internal static byte[] GetCachedBuffer(int minSize)
    {
      lock (BufferPool.Pool)
      {
        int index1 = -1;
        byte[] cachedBuffer1 = (byte[]) null;
        for (int index2 = 0; index2 < BufferPool.Pool.Length; ++index2)
        {
          BufferPool.CachedBuffer cachedBuffer2 = BufferPool.Pool[index2];
          if (cachedBuffer2 != null && cachedBuffer2.Size >= minSize && (cachedBuffer1 == null || cachedBuffer1.Length >= cachedBuffer2.Size))
          {
            byte[] buffer = cachedBuffer2.Buffer;
            if (buffer == null)
            {
              BufferPool.Pool[index2] = (BufferPool.CachedBuffer) null;
            }
            else
            {
              cachedBuffer1 = buffer;
              index1 = index2;
            }
          }
        }
        if (index1 >= 0)
          BufferPool.Pool[index1] = (BufferPool.CachedBuffer) null;
        return cachedBuffer1;
      }
    }

    internal static void ResizeAndFlushLeft(
      ref byte[] buffer,
      int toFitAtLeastBytes,
      int copyFromIndex,
      int copyBytes)
    {
      int length = buffer.Length * 2;
      if (length < 0)
        length = 2147483591;
      if (length < toFitAtLeastBytes)
        length = toFitAtLeastBytes;
      if (copyBytes == 0)
        BufferPool.ReleaseBufferToPool(ref buffer);
      byte[] dst = BufferPool.GetCachedBuffer(toFitAtLeastBytes) ?? new byte[length];
      if (copyBytes > 0)
      {
        Buffer.BlockCopy((Array) buffer, copyFromIndex, (Array) dst, 0, copyBytes);
        BufferPool.ReleaseBufferToPool(ref buffer);
      }
      buffer = dst;
    }

    internal static void ReleaseBufferToPool(ref byte[] buffer)
    {
      if (buffer == null)
        return;
      lock (BufferPool.Pool)
      {
        int index1 = 0;
        int num = int.MaxValue;
        for (int index2 = 0; index2 < BufferPool.Pool.Length; ++index2)
        {
          BufferPool.CachedBuffer cachedBuffer = BufferPool.Pool[index2];
          if (cachedBuffer == null || !cachedBuffer.IsAlive)
          {
            index1 = 0;
            break;
          }
          if (cachedBuffer.Size < num)
          {
            index1 = index2;
            num = cachedBuffer.Size;
          }
        }
        BufferPool.Pool[index1] = new BufferPool.CachedBuffer(buffer);
      }
      buffer = (byte[]) null;
    }

    private class CachedBuffer
    {
      private readonly WeakReference _reference;

      public int Size { get; }

      public bool IsAlive => this._reference.IsAlive;

      public byte[] Buffer => (byte[]) this._reference.Target;

      public CachedBuffer(byte[] buffer)
      {
        this.Size = buffer.Length;
        this._reference = new WeakReference((object) buffer);
      }
    }
  }
}
