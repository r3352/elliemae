// Decompiled with JetBrains decompiler
// Type: Jose.RsaUsingSha
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class RsaUsingSha : IJwsAlgorithm
  {
    private string hashMethod;

    public RsaUsingSha(string hashMethod) => this.hashMethod = hashMethod;

    public byte[] Sign(byte[] securedInput, object key)
    {
      using (HashAlgorithm hashAlgorithm = this.HashAlgorithm)
      {
        RSAPKCS1SignatureFormatter signatureFormatter = new RSAPKCS1SignatureFormatter(Ensure.Type<AsymmetricAlgorithm>(key, "RsaUsingSha alg expects key to be of AsymmetricAlgorithm type."));
        signatureFormatter.SetHashAlgorithm(this.hashMethod);
        return signatureFormatter.CreateSignature(hashAlgorithm.ComputeHash(securedInput));
      }
    }

    public bool Verify(byte[] signature, byte[] securedInput, object key)
    {
      using (HashAlgorithm hashAlgorithm = this.HashAlgorithm)
      {
        AsymmetricAlgorithm key1 = Ensure.Type<AsymmetricAlgorithm>(key, "RsaUsingSha alg expects key to be of AsymmetricAlgorithm type.");
        byte[] hash = hashAlgorithm.ComputeHash(securedInput);
        RSAPKCS1SignatureDeformatter signatureDeformatter = new RSAPKCS1SignatureDeformatter(key1);
        signatureDeformatter.SetHashAlgorithm(this.hashMethod);
        return signatureDeformatter.VerifySignature(hash, signature);
      }
    }

    private HashAlgorithm HashAlgorithm
    {
      get
      {
        if (this.hashMethod.Equals("SHA256"))
          return (HashAlgorithm) new SHA256Managed();
        if (this.hashMethod.Equals("SHA384"))
          return (HashAlgorithm) new SHA384Managed();
        if (this.hashMethod.Equals("SHA512"))
          return (HashAlgorithm) new SHA512Managed();
        throw new ArgumentException("Unsupported hashing algorithm: '{0}'", this.hashMethod);
      }
    }
  }
}
