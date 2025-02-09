// Decompiled with JetBrains decompiler
// Type: Jose.EcdsaUsingSha
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class EcdsaUsingSha : IJwsAlgorithm
  {
    private int keySize;

    public EcdsaUsingSha(int keySize) => this.keySize = keySize;

    public byte[] Sign(byte[] securedInput, object key)
    {
      CngKey key1 = Ensure.Type<CngKey>(key, "EcdsaUsingSha alg expects key to be of CngKey type.");
      Ensure.BitSize(key1.KeySize, this.keySize, string.Format("ECDSA algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keySize, (object) key1.KeySize));
      try
      {
        using (ECDsaCng ecDsaCng = new ECDsaCng(key1))
        {
          ecDsaCng.HashAlgorithm = this.Hash;
          return ecDsaCng.SignData(securedInput);
        }
      }
      catch (CryptographicException ex)
      {
        throw new JoseException("Unable to sign content.", (Exception) ex);
      }
    }

    public bool Verify(byte[] signature, byte[] securedInput, object key)
    {
      CngKey key1 = Ensure.Type<CngKey>(key, "EcdsaUsingSha alg expects key to be of CngKey type.");
      Ensure.BitSize(key1.KeySize, this.keySize, string.Format("ECDSA algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keySize, (object) key1.KeySize));
      try
      {
        using (ECDsaCng ecDsaCng = new ECDsaCng(key1))
        {
          ecDsaCng.HashAlgorithm = this.Hash;
          return ecDsaCng.VerifyData(securedInput, signature);
        }
      }
      catch (CryptographicException ex)
      {
        return false;
      }
    }

    protected CngAlgorithm Hash
    {
      get
      {
        if (this.keySize == 256)
          return CngAlgorithm.Sha256;
        if (this.keySize == 384)
          return CngAlgorithm.Sha384;
        if (this.keySize == 521)
          return CngAlgorithm.Sha512;
        throw new ArgumentException(string.Format("Unsupported key size: '{0} bytes'", (object) this.keySize));
      }
    }
  }
}
