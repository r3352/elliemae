// Decompiled with JetBrains decompiler
// Type: Jose.RsaPss
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.native;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public static class RsaPss
  {
    public static byte[] Sign(byte[] input, CngKey key, CngAlgorithm hash, int saltSize)
    {
      using (System.Security.Cryptography.HashAlgorithm hashAlgorithm = RsaPss.HashAlgorithm(hash))
        return RsaPss.SignHash(hashAlgorithm.ComputeHash(input), key, hash.Algorithm, saltSize);
    }

    public static bool Verify(
      byte[] securedInput,
      byte[] signature,
      CngKey key,
      CngAlgorithm hash,
      int saltSize)
    {
      using (System.Security.Cryptography.HashAlgorithm hashAlgorithm = RsaPss.HashAlgorithm(hash))
        return RsaPss.VerifyHash(hashAlgorithm.ComputeHash(securedInput), signature, key, hash.Algorithm, saltSize);
    }

    private static bool VerifyHash(
      byte[] hash,
      byte[] signature,
      CngKey key,
      string algorithm,
      int saltSize)
    {
      BCrypt.BCRYPT_PSS_PADDING_INFO pPaddingInfo = new BCrypt.BCRYPT_PSS_PADDING_INFO(algorithm, saltSize);
      uint num = NCrypt.NCryptVerifySignature(key.Handle, ref pPaddingInfo, hash, hash.Length, signature, signature.Length, 8U);
      switch (num)
      {
        case 0:
          return true;
        case 2148073478:
          return false;
        default:
          throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() (signature size) failed with status code:{0}", (object) num));
      }
    }

    private static byte[] SignHash(byte[] hash, CngKey key, string algorithm, int saltSize)
    {
      BCrypt.BCRYPT_PSS_PADDING_INFO pPaddingInfo = new BCrypt.BCRYPT_PSS_PADDING_INFO(algorithm, saltSize);
      uint pcbResult;
      uint num1 = NCrypt.NCryptSignHash(key.Handle, ref pPaddingInfo, hash, hash.Length, (byte[]) null, 0, out pcbResult, 8U);
      if (num1 != 0U)
        throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() (signature size) failed with status code:{0}", (object) num1));
      byte[] pbSignature = new byte[(IntPtr) pcbResult];
      uint num2 = NCrypt.NCryptSignHash(key.Handle, ref pPaddingInfo, hash, hash.Length, pbSignature, pbSignature.Length, out pcbResult, 8U);
      if (num2 != 0U)
        throw new CryptographicException(string.Format("NCrypt.NCryptSignHash() failed with status code:{0}", (object) num2));
      return pbSignature;
    }

    private static System.Security.Cryptography.HashAlgorithm HashAlgorithm(CngAlgorithm hash)
    {
      if (hash == CngAlgorithm.Sha256)
        return (System.Security.Cryptography.HashAlgorithm) new SHA256Cng();
      if (hash == CngAlgorithm.Sha384)
        return (System.Security.Cryptography.HashAlgorithm) new SHA384Cng();
      if (hash == CngAlgorithm.Sha512)
        return (System.Security.Cryptography.HashAlgorithm) new SHA512Cng();
      throw new ArgumentException(string.Format("RsaPss expects hash function to be SHA256, SHA384 or SHA512, but was given:{0}", (object) hash));
    }
  }
}
