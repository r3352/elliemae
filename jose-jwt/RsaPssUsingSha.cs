// Decompiled with JetBrains decompiler
// Type: Jose.RsaPssUsingSha
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Security.Cryptography;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class RsaPssUsingSha : IJwsAlgorithm
  {
    private int saltSize;

    public RsaPssUsingSha(int saltSize) => this.saltSize = saltSize;

    public byte[] Sign(byte[] securedInput, object key)
    {
      RSACryptoServiceProvider cryptoServiceProvider = Ensure.Type<RSACryptoServiceProvider>(key, "RsaUsingSha with PSS padding alg expects key to be of RSACryptoServiceProvider type.");
      try
      {
        return RsaPss.Sign(securedInput, RsaKey.New(cryptoServiceProvider.ExportParameters(true)), this.Hash, this.saltSize);
      }
      catch (CryptographicException ex)
      {
        throw new JoseException("Unable to sign content.", (Exception) ex);
      }
    }

    public bool Verify(byte[] signature, byte[] securedInput, object key)
    {
      RSACryptoServiceProvider cryptoServiceProvider = Ensure.Type<RSACryptoServiceProvider>(key, "RsaUsingSha with PSS padding alg expects key to be of RSACryptoServiceProvider type.");
      try
      {
        return RsaPss.Verify(securedInput, signature, RsaKey.New(cryptoServiceProvider.ExportParameters(false)), this.Hash, this.saltSize);
      }
      catch (CryptographicException ex)
      {
        return false;
      }
    }

    private CngAlgorithm Hash
    {
      get
      {
        if (this.saltSize == 32)
          return CngAlgorithm.Sha256;
        if (this.saltSize == 48)
          return CngAlgorithm.Sha384;
        if (this.saltSize == 64)
          return CngAlgorithm.Sha512;
        throw new ArgumentException(string.Format("Unsupported salt size: '{0} bytes'", (object) this.saltSize));
      }
    }
  }
}
