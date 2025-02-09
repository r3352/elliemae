// Decompiled with JetBrains decompiler
// Type: Jose.Arrays
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Jose
{
  public class Arrays
  {
    public static readonly byte[] Empty = new byte[0];
    public static readonly byte[] Zero = new byte[1];
    private static RNGCryptoServiceProvider rng;

    public static byte[] SixtyFourBitLength(byte[] aad)
    {
      return Arrays.LongToBytes((long) (aad.Length * 8));
    }

    public static byte[] FirstHalf(byte[] arr)
    {
      Ensure.Divisible(arr.Length, 2, "Arrays.FirstHalf() expects even number of element in array.");
      int count = arr.Length / 2;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) arr, 0, (Array) dst, 0, count);
      return dst;
    }

    public static byte[] SecondHalf(byte[] arr)
    {
      Ensure.Divisible(arr.Length, 2, "Arrays.SecondHalf() expects even number of element in array.");
      int length = arr.Length / 2;
      byte[] dst = new byte[length];
      Buffer.BlockCopy((Array) arr, length, (Array) dst, 0, length);
      return dst;
    }

    public static byte[] Concat(params byte[][] arrays)
    {
      byte[] dst = new byte[((IEnumerable<byte[]>) arrays).Sum<byte[]>((Func<byte[], int>) (a => a == null ? 0 : a.Length))];
      int dstOffset = 0;
      foreach (byte[] array in arrays)
      {
        if (array != null)
        {
          Buffer.BlockCopy((Array) array, 0, (Array) dst, dstOffset, array.Length);
          dstOffset += array.Length;
        }
      }
      return dst;
    }

    public static byte[][] Slice(byte[] array, int count)
    {
      Ensure.MinValue(count, 1, "Arrays.Slice() expects count to be above zero, but was {0}", (object) count);
      Ensure.Divisible(array.Length, count, "Arrays.Slice() expects array length to be divisible by {0}", (object) count);
      int length = array.Length / count;
      byte[][] numArray = new byte[length][];
      for (int index = 0; index < length; ++index)
      {
        byte[] dst = new byte[count];
        Buffer.BlockCopy((Array) array, index * count, (Array) dst, 0, count);
        numArray[index] = dst;
      }
      return numArray;
    }

    public static byte[] Xor(byte[] left, long right)
    {
      Ensure.BitSize(left, 64, "Arrays.Xor(byte[], long) expects array size to be 8 bytes, but was {0}", (object) left.Length);
      return Arrays.LongToBytes(Arrays.BytesToLong(left) ^ right);
    }

    public static byte[] Xor(byte[] left, byte[] right)
    {
      Ensure.SameSize(left, right, "Arrays.Xor(byte[], byte[]) expects both arrays to be same legnth, but was given {0} and {1}", (object) left.Length, (object) right.Length);
      byte[] numArray = new byte[left.Length];
      for (int index = 0; index < left.Length; ++index)
        numArray[index] = (byte) ((uint) left[index] ^ (uint) right[index]);
      return numArray;
    }

    public static bool ConstantTimeEquals(byte[] expected, byte[] actual)
    {
      if (expected == actual)
        return true;
      if (expected == null || actual == null || expected.Length != actual.Length)
        return false;
      bool flag = true;
      for (int index = 0; index < expected.Length; ++index)
      {
        if ((int) expected[index] != (int) actual[index])
          flag = false;
      }
      return flag;
    }

    public static void Dump(byte[] arr, string label = "")
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(string.Format("{0}({1} bytes): [", (object) (label + " "), (object) arr.Length).Trim());
      foreach (byte num in arr)
      {
        stringBuilder.Append(num);
        stringBuilder.Append(",");
      }
      stringBuilder.Remove(stringBuilder.Length - 1, 1);
      stringBuilder.Append("] Hex:[").Append(BitConverter.ToString(arr).Replace("-", " "));
      stringBuilder.Append("] Base64Url:").Append(Base64Url.Encode(arr)).Append("\n");
      Console.Out.WriteLine(stringBuilder.ToString());
    }

    public static byte[] Random(int sizeBits = 128)
    {
      byte[] data = new byte[sizeBits / 8];
      Arrays.RNG.GetBytes(data);
      return data;
    }

    internal static RNGCryptoServiceProvider RNG
    {
      get => Arrays.rng ?? (Arrays.rng = new RNGCryptoServiceProvider());
    }

    public static byte[] IntToBytes(int value)
    {
      uint num = (uint) value;
      byte[] bytes;
      if (!BitConverter.IsLittleEndian)
        bytes = new byte[4]
        {
          (byte) (num & (uint) byte.MaxValue),
          (byte) (num >> 8 & (uint) byte.MaxValue),
          (byte) (num >> 16 & (uint) byte.MaxValue),
          (byte) (num >> 24 & (uint) byte.MaxValue)
        };
      else
        bytes = new byte[4]
        {
          (byte) (num >> 24 & (uint) byte.MaxValue),
          (byte) (num >> 16 & (uint) byte.MaxValue),
          (byte) (num >> 8 & (uint) byte.MaxValue),
          (byte) (num & (uint) byte.MaxValue)
        };
      return bytes;
    }

    public static byte[] LongToBytes(long value)
    {
      ulong num = (ulong) value;
      byte[] bytes;
      if (!BitConverter.IsLittleEndian)
        bytes = new byte[8]
        {
          (byte) (num & (ulong) byte.MaxValue),
          (byte) (num >> 8 & (ulong) byte.MaxValue),
          (byte) (num >> 16 & (ulong) byte.MaxValue),
          (byte) (num >> 24 & (ulong) byte.MaxValue),
          (byte) (num >> 32 & (ulong) byte.MaxValue),
          (byte) (num >> 40 & (ulong) byte.MaxValue),
          (byte) (num >> 48 & (ulong) byte.MaxValue),
          (byte) (num >> 56 & (ulong) byte.MaxValue)
        };
      else
        bytes = new byte[8]
        {
          (byte) (num >> 56 & (ulong) byte.MaxValue),
          (byte) (num >> 48 & (ulong) byte.MaxValue),
          (byte) (num >> 40 & (ulong) byte.MaxValue),
          (byte) (num >> 32 & (ulong) byte.MaxValue),
          (byte) (num >> 24 & (ulong) byte.MaxValue),
          (byte) (num >> 16 & (ulong) byte.MaxValue),
          (byte) (num >> 8 & (ulong) byte.MaxValue),
          (byte) (num & (ulong) byte.MaxValue)
        };
      return bytes;
    }

    public static long BytesToLong(byte[] array)
    {
      return (BitConverter.IsLittleEndian ? (long) ((int) array[0] << 24 | (int) array[1] << 16 | (int) array[2] << 8 | (int) array[3]) << 32 : (long) ((int) array[7] << 24 | (int) array[6] << 16 | (int) array[5] << 8 | (int) array[4]) << 32) | (BitConverter.IsLittleEndian ? (long) ((int) array[4] << 24 | (int) array[5] << 16 | (int) array[6] << 8 | (int) array[7]) & (long) uint.MaxValue : (long) ((int) array[3] << 24 | (int) array[2] << 16 | (int) array[1] << 8 | (int) array[0]) & (long) uint.MaxValue);
    }

    public static byte[] LeftmostBits(byte[] data, int lengthBits)
    {
      Ensure.Divisible(lengthBits, 8, "LeftmostBits() expects length in bits divisible by 8, but was given {0}", (object) lengthBits);
      int count = lengthBits / 8;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) data, 0, (Array) dst, 0, count);
      return dst;
    }

    public static byte[] RightmostBits(byte[] data, int lengthBits)
    {
      Ensure.Divisible(lengthBits, 8, "RightmostBits() expects length in bits divisible by 8, but was given {0}", (object) lengthBits);
      int count = lengthBits / 8;
      byte[] dst = new byte[count];
      Buffer.BlockCopy((Array) data, data.Length - count, (Array) dst, 0, count);
      return dst;
    }
  }
}
