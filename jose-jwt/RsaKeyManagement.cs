// Decompiled with JetBrains decompiler
// Type: Jose.RsaKeyManagement
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Security.Cryptography;
using System.Collections.Generic;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class RsaKeyManagement : IKeyManagement
  {
    private bool useRsaOaepPadding;
    private bool useSha256;

    public RsaKeyManagement(bool useRsaOaepPadding, bool useSha256 = false)
    {
      this.useRsaOaepPadding = useRsaOaepPadding;
      this.useSha256 = useSha256;
    }

    public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
    {
      RSACryptoServiceProvider cryptoServiceProvider = Ensure.Type<RSACryptoServiceProvider>(key, "RsaKeyManagement alg expects key to be of RSACryptoServiceProvider type.");
      byte[] numArray1 = Arrays.Random(cekSizeBits);
      byte[][] numArray2;
      if (!this.useSha256)
        numArray2 = new byte[2][]
        {
          numArray1,
          cryptoServiceProvider.Encrypt(numArray1, this.useRsaOaepPadding)
        };
      else
        numArray2 = new byte[2][]
        {
          numArray1,
          RsaOaep.Encrypt(numArray1, RsaKey.New(cryptoServiceProvider.ExportParameters(false)), CngAlgorithm.Sha256)
        };
      return numArray2;
    }

    public byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      RSACryptoServiceProvider cryptoServiceProvider = Ensure.Type<RSACryptoServiceProvider>(key, "RsaKeyManagement alg expects key to be of RSACryptoServiceProvider type.");
      return this.useSha256 ? RsaOaep.Decrypt(encryptedCek, RsaKey.New(cryptoServiceProvider.ExportParameters(true)), CngAlgorithm.Sha256) : cryptoServiceProvider.Decrypt(encryptedCek, this.useRsaOaepPadding);
    }
  }
}
