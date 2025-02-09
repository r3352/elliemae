// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Serialization.StringBuilderReader
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace EllieMae.EMLite.Common.Serialization
{
  [Serializable]
  public class StringBuilderReader : TextReader
  {
    private StringBuilder _sb;
    private int _pos;
    private int _length;

    public StringBuilderReader(StringBuilder sb)
    {
      this._sb = sb != null ? sb : throw new ArgumentNullException(nameof (sb));
      this._length = sb.Length;
    }

    public override void Close() => this.Dispose(true);

    protected override void Dispose(bool disposing)
    {
      this._sb = (StringBuilder) null;
      this._pos = 0;
      this._length = 0;
      base.Dispose(disposing);
    }

    public override int Peek()
    {
      if (this._sb == null)
        throw new ObjectDisposedException((string) null, "StringBuilderReader is already disposed");
      return this._pos == this._length ? -1 : (int) this._sb[this._pos];
    }

    public override int Read()
    {
      if (this._sb == null)
        throw new ObjectDisposedException((string) null, "StringBuilderReader is already disposed");
      return this._pos == this._length ? -1 : (int) this._sb[this._pos++];
    }

    public override int Read(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (buffer.Length - index < count)
        throw new ArgumentException("Insufficient buffer size.");
      if (this._sb == null)
        throw new ObjectDisposedException((string) null, "StringBuilderReader is already disposed");
      int count1 = this._length - this._pos;
      if (count1 > 0)
      {
        if (count1 > count)
          count1 = count;
        this._sb.CopyTo(this._pos, buffer, index, count1);
        this._pos += count1;
      }
      return count1;
    }

    public override string ReadToEnd()
    {
      if (this._sb == null)
        throw new ObjectDisposedException((string) null, "StringBuilderReader is already disposed");
      string end = this._pos != 0 ? this._sb.ToString(this._pos, this._length - this._pos) : this._sb.ToString();
      this._pos = this._length;
      return end;
    }

    public override string ReadLine()
    {
      if (this._sb == null)
        throw new ObjectDisposedException((string) null, "StringBuilderReader is already disposed");
      int pos;
      for (pos = this._pos; pos < this._length; ++pos)
      {
        char ch = this._sb[pos];
        switch (ch)
        {
          case '\n':
          case '\r':
            string str = this._sb.ToString(this._pos, pos - this._pos);
            this._pos = pos + 1;
            if (ch == '\r' && this._pos < this._length && this._sb[this._pos] == '\n')
              ++this._pos;
            return str;
          default:
            continue;
        }
      }
      if (pos <= this._pos)
        return (string) null;
      string str1 = this._sb.ToString(this._pos, pos - this._pos);
      this._pos = pos;
      return str1;
    }

    public override Task<string> ReadLineAsync() => Task.FromResult<string>(this.ReadLine());

    public override Task<string> ReadToEndAsync() => Task.FromResult<string>(this.ReadToEnd());

    public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (buffer.Length - index < count)
        throw new ArgumentException("Insufficient buffer size.");
      return Task.FromResult<int>(this.ReadBlock(buffer, index, count));
    }

    public override Task<int> ReadAsync(char[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException(nameof (buffer));
      if (index < 0)
        throw new ArgumentOutOfRangeException(nameof (index));
      if (count < 0)
        throw new ArgumentOutOfRangeException(nameof (count));
      if (buffer.Length - index < count)
        throw new ArgumentException("Insufficient buffer size.");
      return Task.FromResult<int>(this.Read(buffer, index, count));
    }
  }
}
