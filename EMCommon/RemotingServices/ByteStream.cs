// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.RemotingServices.ByteStream
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace EllieMae.EMLite.RemotingServices
{
  [Serializable]
  public class ByteStream : Stream
  {
    private static byte[] zeroBytes = new byte[BufferPools.LargePoolBufferSize];
    private BufferPool bufferPool;
    private List<byte[]> byteData = new List<byte[]>();
    private int length;
    private int position;
    private bool readOnly;

    public ByteStream(long estimatedSize)
    {
      if ((int) estimatedSize / BufferPools.LargePoolBufferSize < 2)
        this.bufferPool = BufferPools.SmallBufferPool;
      else
        this.bufferPool = BufferPools.LargeBufferPool;
    }

    public ByteStream(bool isLarge)
    {
      this.bufferPool = isLarge ? BufferPools.LargeBufferPool : BufferPools.SmallBufferPool;
    }

    public ByteStream(BufferPool bufferPool) => this.bufferPool = bufferPool;

    public ByteStream(byte[] data, bool readOnly)
      : this((long) data.Length)
    {
      this.Write(data, 0, data.Length);
      this.position = 0;
      this.readOnly = readOnly;
    }

    public ByteStream(byte[] data)
      : this(data, false)
    {
    }

    public override bool CanRead => true;

    public override bool CanWrite => !this.readOnly;

    public override bool CanSeek => true;

    public override bool CanTimeout => false;

    public override long Length => (long) this.length;

    public override long Position
    {
      get => (long) this.position;
      set
      {
        this.position = value >= 0L && value <= this.Length ? (int) value : throw new IndexOutOfRangeException("Position must be within bounds of the stream");
      }
    }

    public override void Flush()
    {
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.Position = offset;
          break;
        case SeekOrigin.Current:
          this.Position += offset;
          break;
        case SeekOrigin.End:
          this.Position = this.Length + offset;
          break;
        default:
          throw new ArgumentException("Invalid SeekOrigin value specified");
      }
      return this.Position;
    }

    public override void SetLength(long value) => this.length = (int) value;

    public override int Read(byte[] buffer, int offset, int count)
    {
      this.ensureNotDisposed();
      int bufferSize = this.bufferPool.BufferSize;
      int num;
      int count1;
      for (num = 0; num < count && this.position < this.length; this.position += count1)
      {
        int index = this.position / bufferSize;
        int srcOffset = this.position % bufferSize;
        count1 = Math.Min(Math.Min(this.length - this.position, count - num), bufferSize - srcOffset);
        Buffer.BlockCopy(index >= this.byteData.Count ? (Array) ByteStream.zeroBytes : (Array) this.byteData[index], srcOffset, (Array) buffer, offset + num, count1);
        num += count1;
      }
      return num;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.ensureNotDisposed();
      if (this.readOnly)
        throw new InvalidOperationException("Cannot write to a ByteStream marked as read-only");
      int num = 0;
      while (num < count)
      {
        int index = this.position / this.bufferPool.BufferSize;
        while (this.byteData.Count <= index)
          this.byteData.Add(this.bufferPool.TakeBuffer());
        byte[] dst = this.byteData[index];
        int dstOffset = this.position % this.bufferPool.BufferSize;
        int count1 = Math.Min(dst.Length - dstOffset, count - num);
        Buffer.BlockCopy((Array) buffer, offset + num, (Array) dst, dstOffset, count1);
        num += count1;
        this.position += count1;
        this.length = Math.Max(this.length, this.position);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.byteData != null)
      {
        for (int index = 0; index < this.byteData.Count; ++index)
          this.bufferPool.ReturnBuffer(this.byteData[index]);
        this.byteData = (List<byte[]>) null;
      }
      base.Dispose(disposing);
    }

    public byte[] ToArray()
    {
      this.ensureNotDisposed();
      int position = this.position;
      try
      {
        this.position = 0;
        byte[] buffer = new byte[this.length];
        this.Read(buffer, 0, this.length);
        return buffer;
      }
      finally
      {
        this.position = position;
      }
    }

    public void CopyFrom(Stream source)
    {
      this.ensureNotDisposed();
      if (this.readOnly)
        throw new InvalidOperationException("Cannot write to a ByteStream marked as read-only");
      int bufferSize = this.bufferPool.BufferSize;
      int num;
      do
      {
        int index = this.position / bufferSize;
        while (this.byteData.Count <= index)
          this.byteData.Add(this.bufferPool.TakeBuffer());
        byte[] buffer = this.byteData[index];
        int offset = this.position % bufferSize;
        int count = buffer.Length - offset;
        num = source.Read(buffer, offset, count);
        this.position += num;
        this.length = Math.Max(this.length, this.position);
      }
      while (num > 0);
    }

    private void ensureNotDisposed()
    {
      if (this.byteData == null)
        throw new ObjectDisposedException(nameof (ByteStream));
    }

    public static ByteStream FromFile(string filePath)
    {
      using (FileStream source = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
      {
        ByteStream byteStream = new ByteStream(source.Length);
        byteStream.CopyFrom((Stream) source);
        byteStream.Position = 0L;
        return byteStream;
      }
    }

    public static ByteStream FromStream(Stream source, bool resetSourceStream)
    {
      ByteStream byteStream = new ByteStream(source.Length);
      if (resetSourceStream)
        source.Position = 0L;
      byteStream.CopyFrom(source);
      byteStream.Position = 0L;
      return byteStream;
    }

    public static ByteStream FromStream(Stream source, bool resetSourceStream, long fileSize)
    {
      ByteStream byteStream = !(source is InflaterInputStream) ? new ByteStream(source.Length) : new ByteStream(fileSize);
      if (resetSourceStream)
        source.Position = 0L;
      byteStream.CopyFrom(source);
      byteStream.Position = 0L;
      return byteStream;
    }
  }
}
