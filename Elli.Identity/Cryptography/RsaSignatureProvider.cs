// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Cryptography.RsaSignatureProvider
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using Elli.Identity.Key;
using System;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Elli.Identity.Cryptography
{
  public class RsaSignatureProvider : ISignatureProvider
  {
    private readonly ITokenKeyFactory _keyFactory;

    public RsaSignatureProvider(ITokenKeyFactory keyFactory) => this._keyFactory = keyFactory;

    public string GenerateSignature(string algorithm, string data)
    {
      using (HashAlgorithm hashAlgorithm = this.GetHashAlgorithm(algorithm))
      {
        RSAPKCS1SignatureFormatter signatureFormatter = new RSAPKCS1SignatureFormatter((AsymmetricAlgorithm) this._keyFactory.GetSignatureKey());
        signatureFormatter.SetHashAlgorithm(algorithm);
        return Convert.ToBase64String(signatureFormatter.CreateSignature(hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(data))));
      }
    }

    public bool VerifySignature(string algorithm, string data, string signature)
    {
      using (HashAlgorithm hashAlgorithm = this.GetHashAlgorithm(algorithm))
      {
        RSAPKCS1SignatureDeformatter signatureDeformatter = new RSAPKCS1SignatureDeformatter((AsymmetricAlgorithm) this._keyFactory.GetValidationKey());
        signatureDeformatter.SetHashAlgorithm(algorithm);
        return signatureDeformatter.VerifySignature(hashAlgorithm.ComputeHash(Encoding.Unicode.GetBytes(data)), Convert.FromBase64String(signature));
      }
    }

    private HashAlgorithm GetHashAlgorithm(string algorithm)
    {
      switch (algorithm)
      {
        case "SHA256":
          return (HashAlgorithm) new SHA256Managed();
        case "SHA384":
          return (HashAlgorithm) new SHA384Managed();
        case "SHA512":
          return (HashAlgorithm) new SHA512Managed();
        default:
          throw new ArgumentException("Unsupported hashing algorithm: '{0}'", algorithm);
      }
    }
  }
}
