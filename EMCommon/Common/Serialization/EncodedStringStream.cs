// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Serialization.EncodedStringStream
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Serialization
{
  public class EncodedStringStream : Stream
  {
    private string _s;
    private Encoding _encoding;
    private byte[] _preambleBytes;
    private char[] _charBuffer;
    private byte[] _byteBuffer;
    private int _streamPosition;
    private int _stringPosition;
    private int _byteInCurrentCharPosition;
    private int _length;
    private readonly EncodedStringStream.AdditionalBytes[] _additionalBytes;

    public EncodedStringStream(Encoding encoding, string s, bool includePreamble)
    {
      if (encoding == null)
        throw new ArgumentNullException(nameof (encoding));
      if (s == null)
        throw new ArgumentNullException(nameof (s));
      this._encoding = encoding;
      if (includePreamble)
      {
        this._preambleBytes = encoding.GetPreamble();
        if (this._preambleBytes != null && this._preambleBytes.Length == 0)
          this._preambleBytes = (byte[]) null;
      }
      else
        this._preambleBytes = (byte[]) null;
      this._s = s;
      this._charBuffer = new char[1];
      this._byteBuffer = new byte[encoding.GetMaxByteCount(1)];
      this._streamPosition = 0;
      this._stringPosition = -this.GetPreambleLength();
      this._byteInCurrentCharPosition = 0;
      this._additionalBytes = this.InitializeAditionalBytes();
      if (this._additionalBytes.Length == 0)
        this._length = this._s.Length + this.GetPreambleLength();
      else
        this._length = this._s.Length + this.GetPreambleLength() + this._additionalBytes[this._additionalBytes.Length - 1].CumulativeAdditionalBytes;
    }

    public override long Position
    {
      get => (long) this.CheckDisposed<int>(this._streamPosition);
      set
      {
        this.CheckDisposed();
        this._streamPosition = value >= 0L && value <= this.Length ? (int) value : throw new ArgumentOutOfRangeException("Postion should be greater than or equal to 0 and smaller than stream length");
        int num1 = this._streamPosition - this.GetPreambleLength();
        if (num1 < 0)
        {
          this._stringPosition = num1;
          this._byteInCurrentCharPosition = 0;
        }
        else
        {
          int num2 = 0;
          int index = -1;
          if (this._additionalBytes.Length != 0)
          {
            for (index = 0; index < this._additionalBytes.Length && this._additionalBytes[index].StringPosition + this._additionalBytes[index].CumulativeAdditionalBytes <= num1; ++index)
              num2 = this._additionalBytes[index].CumulativeAdditionalBytes;
          }
          this._stringPosition = num1 - num2;
          if (index > 0 && this._stringPosition == this._additionalBytes[index - 1].StringPosition)
            this._byteInCurrentCharPosition = num1 - this._additionalBytes[index - 1].StringPosition - this._additionalBytes[index - 1].CumulativeAdditionalBytes + 1;
          else
            this._byteInCurrentCharPosition = 0;
        }
      }
    }

    public override long Length => (long) this.CheckDisposed<int>(this._length);

    public override bool CanWrite => this.CheckDisposed<bool>(false);

    public override bool CanSeek => this.CheckDisposed<bool>(true);

    public override bool CanRead => this.CheckDisposed<bool>(true);

    private T CheckDisposed<T>(T t)
    {
      this.CheckDisposed();
      return t;
    }

    private void CheckDisposed()
    {
      if (this._s == null)
        throw new ObjectDisposedException((string) null, "Stream is already disposed");
    }

    private EncodedStringStream.AdditionalBytes[] InitializeAditionalBytes()
    {
      List<EncodedStringStream.AdditionalBytes> additionalBytesList = new List<EncodedStringStream.AdditionalBytes>();
      int num = 0;
      for (int index = 0; index < this._s.Length; ++index)
      {
        this._charBuffer[0] = this._s[index];
        int byteCount = this._encoding.GetByteCount(this._charBuffer);
        if (byteCount > 1)
        {
          num += byteCount - 1;
          additionalBytesList.Add(new EncodedStringStream.AdditionalBytes()
          {
            StringPosition = index,
            CumulativeAdditionalBytes = num
          });
        }
      }
      return additionalBytesList.ToArray();
    }

    private int GetPreambleLength() => this._preambleBytes != null ? this._preambleBytes.Length : 0;

    public override void Flush() => this.CheckDisposed();

    public override long Seek(long offset, SeekOrigin origin)
    {
      switch (origin)
      {
        case SeekOrigin.Begin:
          this.Position = offset;
          return this.Position;
        case SeekOrigin.Current:
          this.Position = offset + this.Position;
          return this.Position;
        default:
          throw new NotSupportedException("Seeking from End is not supported.");
      }
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException("Length can not be set");
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      this.CheckDisposed();
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (offset < 0)
        throw new ArgumentOutOfRangeException(nameof (offset));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (buffer.Length - offset < count)
        throw new ArgumentException("Insufficient buffer size.");
      int num1;
      for (num1 = 0; num1 < count && this._streamPosition < this.GetPreambleLength(); ++num1)
      {
        buffer[offset + num1] = this._preambleBytes[this._streamPosition];
        ++this._streamPosition;
        ++this._stringPosition;
        this._byteInCurrentCharPosition = 0;
      }
      int num2;
      for (; num1 < count && this._streamPosition < this._length; num1 += num2)
        num2 = this.ReadNextCharBytesInBuffer(buffer, offset + num1, count - num1);
      return num1;
    }

    private int ReadNextCharBytesInBuffer(byte[] buffer, int offset, int count)
    {
      this._charBuffer[0] = this._s[this._stringPosition];
      int bytes = this._encoding.GetBytes(this._charBuffer, 0, 1, this._byteBuffer, 0);
      int num;
      for (num = 0; num < count && this._byteInCurrentCharPosition < bytes; ++num)
      {
        buffer[offset] = this._byteBuffer[this._byteInCurrentCharPosition];
        ++offset;
        ++this._streamPosition;
        ++this._byteInCurrentCharPosition;
      }
      if (this._byteInCurrentCharPosition == bytes)
      {
        this._byteInCurrentCharPosition = 0;
        ++this._stringPosition;
      }
      return num;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException("Stream is not writable");
    }

    protected override void Dispose(bool disposing)
    {
      if (this._s != null)
      {
        this._s = (string) null;
        this._encoding = (Encoding) null;
        this._preambleBytes = (byte[]) null;
        this._charBuffer = (char[]) null;
        this._byteBuffer = (byte[]) null;
        this._streamPosition = 0;
        this._stringPosition = 0;
        this._length = 0;
      }
      base.Dispose(disposing);
    }

    private class AdditionalBytes
    {
      public int StringPosition { get; set; }

      public int CumulativeAdditionalBytes { get; set; }
    }
  }
}
