// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Serialization.HexEncoding
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common.Serialization
{
  public sealed class HexEncoding : Encoding
  {
    private const string hexDigits = "0123456789ABCDEF";
    private const string InvalidHex = "Invalid character '{0}' at position {1}";
    private static readonly char[] s_lookup = Enumerable.Range(0, 256).SelectMany<int, char>((Func<int, IEnumerable<char>>) (i => (IEnumerable<char>) new char[2]
    {
      "0123456789ABCDEF"[(i >> 4) % 16],
      "0123456789ABCDEF"[i % 16]
    })).ToArray<char>();
    public static HexEncoding Instance = new HexEncoding();

    public override string GetString(byte[] bytes)
    {
      char[] chars = new char[this.GetMaxCharCount(bytes.Length)];
      this.GetChars(bytes, 0, bytes.Length, chars, 0);
      return new string(chars);
    }

    public override int GetChars(
      byte[] bytes,
      int byteIndex,
      int byteCount,
      char[] chars,
      int charIndex)
    {
      int index = byteIndex;
      int num = index + byteCount;
      while (index < num)
        Array.Copy((Array) HexEncoding.s_lookup, (int) bytes[index] << 1, (Array) chars, index++ << 1, 2);
      return this.GetMaxCharCount(index - byteIndex);
    }

    public override byte[] GetBytes(string hexString)
    {
      int charIndex = hexString.StartsWith("0x") ? 2 : 0;
      char[] charArray = hexString.ToCharArray();
      byte[] bytes = new byte[this.GetMaxByteCount(hexString.Length - charIndex)];
      this.GetBytes(charArray, charIndex, charArray.Length - charIndex, bytes, 0);
      return bytes;
    }

    public override int GetBytes(
      char[] chars,
      int charIndex,
      int charCount,
      byte[] bytes,
      int byteIndex)
    {
      int num1 = byteIndex;
      int num2 = num1 + this.GetMaxByteCount(charCount);
      while (num1 < num2)
        bytes[num1++] = (byte) ((hexValue(charIndex++) << 4) + hexValue(charIndex++));
      return num1 - byteIndex;

      int hexValue(int pos)
      {
        int num = "0123456789ABCDEF".IndexOf(char.ToUpper(chars[pos]));
        return num > -1 ? num : throw new Exception(string.Format("Invalid character '{0}' at position {1}", (object) chars[pos], (object) pos));
      }
    }

    public override int GetByteCount(char[] chars, int index, int count)
    {
      return this.GetMaxByteCount(Math.Min(count, chars.Length - index));
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return this.GetMaxCharCount(Math.Min(count, bytes.Length - index));
    }

    public override int GetMaxByteCount(int charCount) => charCount >> 1;

    public override int GetMaxCharCount(int byteCount) => byteCount << 1;
  }
}
