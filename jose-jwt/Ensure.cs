// Decompiled with JetBrains decompiler
// Type: Jose.Ensure
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Jose
{
  public class Ensure
  {
    public static void IsEmpty(byte[] arr, string msg, params object[] args)
    {
      if (arr.Length != 0)
        throw new ArgumentException(msg);
    }

    public static T Type<T>(object obj, string msg, params object[] args)
    {
      return obj is T obj1 ? obj1 : throw new ArgumentException(msg);
    }

    public static void IsNull(object key, string msg, params object[] args)
    {
      if (key != null)
        throw new ArgumentException(msg);
    }

    public static void BitSize(byte[] array, int expectedSize, string msg, params object[] args)
    {
      if (expectedSize != array.Length * 8)
        throw new ArgumentException(string.Format(msg, args));
    }

    public static void BitSize(int actualSize, int expectedSize, string msg)
    {
      if (expectedSize != actualSize)
        throw new ArgumentException(msg);
    }

    public static void IsNotEmpty(string arg, string msg, params object[] args)
    {
      if (string.IsNullOrWhiteSpace(arg))
        throw new ArgumentException(msg);
    }

    public static void Divisible(int arg, int divisor, string msg, params object[] args)
    {
      if (arg % divisor != 0)
        throw new ArgumentException(string.Format(msg, args));
    }

    public static void MinValue(int arg, int min, string msg, params object[] args)
    {
      if (arg < min)
        throw new ArgumentException(string.Format(msg, args));
    }

    public static void MaxValue(int arg, long max, string msg, params object[] args)
    {
      if ((long) arg > max)
        throw new ArgumentException(string.Format(msg, args));
    }

    public static void MinBitSize(byte[] arr, int minBitSize, string msg, params object[] args)
    {
      Ensure.MinValue(arr.Length * 8, minBitSize, msg, args);
    }

    public static void Contains(
      IDictionary<string, object> dict,
      string[] keys,
      string msg,
      params object[] args)
    {
      if (((IEnumerable<string>) keys).Any<string>((Func<string, bool>) (key => !dict.ContainsKey(key))))
        throw new ArgumentException(string.Format(msg, args));
    }

    public static void SameSize(byte[] left, byte[] right, string msg, params object[] args)
    {
      if (left.Length != right.Length)
        throw new ArgumentException(string.Format(msg, args));
    }
  }
}
