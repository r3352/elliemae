// Decompiled with JetBrains decompiler
// Type: Jose.RsaOaep
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.native;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public static class RsaOaep
  {
    public static byte[] Decrypt(byte[] cipherText, CngKey key, CngAlgorithm hash)
    {
      BCrypt.BCRYPT_OAEP_PADDING_INFO pvPadding = new BCrypt.BCRYPT_OAEP_PADDING_INFO(hash.Algorithm);
      uint pcbResult;
      uint num1 = NCrypt.NCryptDecrypt(key.Handle, cipherText, cipherText.Length, ref pvPadding, (byte[]) null, 0U, out pcbResult, 4U);
      if (num1 != 0U)
        throw new CryptographicException(string.Format("NCrypt.Decrypt() (plaintext buffer size) failed with status code:{0}", (object) num1));
      byte[] pbOutput = new byte[(IntPtr) pcbResult];
      uint num2 = NCrypt.NCryptDecrypt(key.Handle, cipherText, cipherText.Length, ref pvPadding, pbOutput, pcbResult, out pcbResult, 4U);
      if (num2 != 0U)
        throw new CryptographicException(string.Format("NCrypt.Decrypt() failed with status code:{0}", (object) num2));
      return pbOutput;
    }

    public static byte[] Encrypt(byte[] plainText, CngKey key, CngAlgorithm hash)
    {
      BCrypt.BCRYPT_OAEP_PADDING_INFO pvPadding = new BCrypt.BCRYPT_OAEP_PADDING_INFO(hash.Algorithm);
      uint pcbResult;
      uint num1 = NCrypt.NCryptEncrypt(key.Handle, plainText, plainText.Length, ref pvPadding, (byte[]) null, 0U, out pcbResult, 4U);
      if (num1 != 0U)
        throw new CryptographicException(string.Format("NCrypt.Encrypt() (ciphertext buffer size) failed with status code:{0}", (object) num1));
      byte[] pbOutput = new byte[(IntPtr) pcbResult];
      uint num2 = NCrypt.NCryptEncrypt(key.Handle, plainText, plainText.Length, ref pvPadding, pbOutput, pcbResult, out pcbResult, 4U);
      if (num2 != 0U)
        throw new CryptographicException(string.Format("NCrypt.Encrypt() failed with status code:{0}", (object) num2));
      return pbOutput;
    }
  }
}
