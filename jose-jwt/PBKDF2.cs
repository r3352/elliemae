// Decompiled with JetBrains decompiler
// Type: Jose.PBKDF2
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public static class PBKDF2
  {
    public static byte[] DeriveKey(
      byte[] password,
      byte[] salt,
      int iterationCount,
      int keyBitLength,
      HMAC prf)
    {
      prf.Key = password;
      Ensure.MaxValue(keyBitLength, (long) uint.MaxValue, "PBKDF2 expect derived key size to be not more that (2^32-1) bits, but was requested {0} bits.", (object) keyBitLength);
      int num1 = prf.HashSize / 8;
      int num2 = keyBitLength / 8;
      int length = (int) Math.Ceiling((double) num2 / (double) num1);
      int num3 = num2 - (length - 1) * num1;
      byte[][] numArray = new byte[length][];
      for (int index = 0; index < length; ++index)
        numArray[index] = PBKDF2.F(salt, iterationCount, index + 1, prf);
      numArray[length - 1] = Arrays.LeftmostBits(numArray[length - 1], num3 * 8);
      return Arrays.Concat(numArray);
    }

    private static byte[] F(byte[] salt, int iterationCount, int blockIndex, HMAC prf)
    {
      byte[] hash = prf.ComputeHash(Arrays.Concat(salt, Arrays.IntToBytes(blockIndex)));
      byte[] left = hash;
      for (int index = 2; index <= iterationCount; ++index)
      {
        hash = prf.ComputeHash(hash);
        left = Arrays.Xor(left, hash);
      }
      return left;
    }
  }
}
