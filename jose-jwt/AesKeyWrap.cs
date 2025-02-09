// Decompiled with JetBrains decompiler
// Type: Jose.AesKeyWrap
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public static class AesKeyWrap
  {
    private static readonly byte[] DefaultIV = new byte[8]
    {
      (byte) 166,
      (byte) 166,
      (byte) 166,
      (byte) 166,
      (byte) 166,
      (byte) 166,
      (byte) 166,
      (byte) 166
    };

    public static byte[] Wrap(byte[] cek, byte[] kek)
    {
      Ensure.MinBitSize(cek, 128, "AesKeyWrap.Wrap() expects content length not less than 128 bits, but was {0}", (object) (cek.Length * 8));
      Ensure.Divisible(cek.Length, 8, "AesKeyWrap.Wrap() expects content length to be divisable by 8, but was given a content of {0} bit size.", (object) (cek.Length * 8));
      byte[] numArray1 = AesKeyWrap.DefaultIV;
      byte[][] numArray2 = Arrays.Slice(cek, 8);
      long length = (long) numArray2.Length;
      for (long index1 = 0; index1 < 6L; ++index1)
      {
        for (long index2 = 0; index2 < length; ++index2)
        {
          long right = length * index1 + index2 + 1L;
          byte[] arr = AesKeyWrap.AesEnc(kek, Arrays.Concat(numArray1, numArray2[index2]));
          byte[] left = Arrays.FirstHalf(arr);
          numArray2[index2] = Arrays.SecondHalf(arr);
          numArray1 = Arrays.Xor(left, right);
        }
      }
      byte[][] numArray3 = new byte[length + 1L][];
      numArray3[0] = numArray1;
      for (long index = 1; index <= length; ++index)
        numArray3[index] = numArray2[index - 1L];
      return Arrays.Concat(numArray3);
    }

    public static byte[] Unwrap(byte[] encryptedCek, byte[] kek)
    {
      Ensure.MinBitSize(encryptedCek, 128, "AesKeyWrap.Unwrap() expects content length not less than 128 bits, but was {0}", (object) (encryptedCek.Length * 8));
      Ensure.Divisible(encryptedCek.Length, 8, "AesKeyWrap.Unwrap() expects content length to be divisable by 8, but was given a content of {0} bit size.", (object) (encryptedCek.Length * 8));
      byte[][] numArray1 = Arrays.Slice(encryptedCek, 8);
      byte[] numArray2 = numArray1[0];
      byte[][] numArray3 = new byte[numArray1.Length - 1][];
      for (int index = 1; index < numArray1.Length; ++index)
        numArray3[index - 1] = numArray1[index];
      long length = (long) numArray3.Length;
      for (long index1 = 5; index1 >= 0L; --index1)
      {
        for (long index2 = length - 1L; index2 >= 0L; --index2)
        {
          long right = length * index1 + index2 + 1L;
          byte[] numArray4 = Arrays.Xor(numArray2, right);
          byte[] arr = AesKeyWrap.AesDec(kek, Arrays.Concat(numArray4, numArray3[index2]));
          numArray2 = Arrays.FirstHalf(arr);
          numArray3[index2] = Arrays.SecondHalf(arr);
        }
      }
      if (!Arrays.ConstantTimeEquals(AesKeyWrap.DefaultIV, numArray2))
        throw new IntegrityException("AesKeyWrap integrity check failed.");
      return Arrays.Concat(numArray3);
    }

    private static byte[] AesDec(byte[] sharedKey, byte[] cipherText)
    {
      using (Aes aes = (Aes) new AesManaged())
      {
        aes.Key = sharedKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(cipherText, 0, cipherText.Length);
              cryptoStream.FlushFinalBlock();
              return memoryStream.ToArray();
            }
          }
        }
      }
    }

    private static byte[] AesEnc(byte[] sharedKey, byte[] plainText)
    {
      using (Aes aes = (Aes) new AesManaged())
      {
        aes.Key = sharedKey;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
            {
              cryptoStream.Write(plainText, 0, plainText.Length);
              cryptoStream.FlushFinalBlock();
              return memoryStream.ToArray();
            }
          }
        }
      }
    }
  }
}
