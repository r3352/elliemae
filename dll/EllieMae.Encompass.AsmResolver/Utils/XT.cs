// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.AsmResolver.Utils.XT
// Assembly: EllieMae.Encompass.AsmResolver, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: DF61C82F-0411-4587-80AB-293D90FB58E7
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\EllieMae.Encompass.AsmResolver.dll

using System;
using System.Text;

#nullable disable
namespace EllieMae.Encompass.AsmResolver.Utils
{
  internal class XT
  {
    private const uint delta = 2654435769;

    internal static string ESB64(string d, string k)
    {
      if (d.Length == 0)
        throw new ArgumentException("Data must be at least 1 character in length.");
      uint[] k1 = XT.fk(k);
      if (d.Length % 2 != 0)
        d += "\0";
      byte[] bytes = Encoding.ASCII.GetBytes(d);
      string str = string.Empty;
      uint[] v = new uint[2];
      for (int index = 0; index < bytes.Length; index += 2)
      {
        v[0] = (uint) bytes[index];
        v[1] = (uint) bytes[index + 1];
        XT.c(v, k1);
        str = str + XT.toS(v[0]) + XT.toS(v[1]);
      }
      byte[] inArray = new byte[str.Length];
      for (int index = 0; index < str.Length; ++index)
        inArray[index] = (byte) str[index];
      return Convert.ToBase64String(inArray);
    }

    internal static string DSB64(string dd, string k)
    {
      byte[] numArray1 = Convert.FromBase64String(dd);
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < numArray1.Length; ++index)
        stringBuilder.Append((char) numArray1[index]);
      dd = stringBuilder.ToString();
      uint[] k1 = XT.fk(k);
      int num1 = 0;
      uint[] v = new uint[2];
      byte[] bytes = new byte[dd.Length / 8 * 2];
      for (int startIndex = 0; startIndex < dd.Length; startIndex += 8)
      {
        v[0] = XT.toUI(dd.Substring(startIndex, 4));
        v[1] = XT.toUI(dd.Substring(startIndex + 4, 4));
        XT.d(v, k1);
        byte[] numArray2 = bytes;
        int index1 = num1;
        int num2 = index1 + 1;
        int num3 = (int) (byte) v[0];
        numArray2[index1] = (byte) num3;
        byte[] numArray3 = bytes;
        int index2 = num2;
        num1 = index2 + 1;
        int num4 = (int) (byte) v[1];
        numArray3[index2] = (byte) num4;
      }
      string str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
      if (str[str.Length - 1] == char.MinValue)
        str = str.Substring(0, str.Length - 1);
      return str;
    }

    private static uint[] fk(string k)
    {
      k = k.Length != 0 ? k.PadRight(16, ' ').Substring(0, 16) : throw new ArgumentException("K must be between 1 and 16 characters in length");
      uint[] numArray = new uint[4];
      int num = 0;
      for (int startIndex = 0; startIndex < k.Length; startIndex += 4)
        numArray[num++] = XT.toUI(k.Substring(startIndex, 4));
      return numArray;
    }

    private static void c(uint[] v, uint[] k)
    {
      uint num1 = v[0];
      uint num2 = v[1];
      uint num3 = 0;
      uint num4 = 32;
      while (num4-- > 0U)
      {
        num1 += (uint) (((int) num2 << 4 ^ (int) (num2 >> 5)) + (int) num2 ^ (int) num3 + (int) k[(int) num3 & 3]);
        num3 += 2654435769U;
        num2 += (uint) (((int) num1 << 4 ^ (int) (num1 >> 5)) + (int) num1 ^ (int) num3 + (int) k[(int) (num3 >> 11) & 3]);
      }
      v[0] = num1;
      v[1] = num2;
    }

    private static void d(uint[] v, uint[] k)
    {
      uint num1 = v[0];
      uint num2 = v[1];
      uint num3 = 3337565984;
      uint num4 = 32;
      while (num4-- > 0U)
      {
        num2 -= (uint) (((int) num1 << 4 ^ (int) (num1 >> 5)) + (int) num1 ^ (int) num3 + (int) k[(int) (num3 >> 11) & 3]);
        num3 -= 2654435769U;
        num1 -= (uint) (((int) num2 << 4 ^ (int) (num2 >> 5)) + (int) num2 ^ (int) num3 + (int) k[(int) num3 & 3]);
      }
      v[0] = num1;
      v[1] = num2;
    }

    private static uint toUI(string Input)
    {
      return (uint) ((int) Input[0] + ((int) Input[1] << 8) + ((int) Input[2] << 16) + ((int) Input[3] << 24));
    }

    private static string toS(uint Input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append((char) (Input & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 8 & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 16 & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 24 & (uint) byte.MaxValue));
      return stringBuilder.ToString();
    }
  }
}
