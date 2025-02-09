// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Key.ElliRsaKeyFactory
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System.Configuration;
using System.Security.Cryptography;

#nullable disable
namespace Elli.Identity.Key
{
  public class ElliRsaKeyFactory : ITokenKeyFactory
  {
    private readonly string _publicKey = "<RSAKeyValue><Modulus>m9ZyBGa3wDsnNA9 PV5vS8w1zDlUa4ecWjnSFnWVi9tbi9A1jc2rGfZNDpmg1eo4dJXH3JOD8hSCQC28TSpOdugssEtDTNXhT8qv2EWfo/NplLtrv5OvEz0zR3vzNz1f8wNCkMK5p4SGd/GwsVQ3jOzAipjqPTYn7czoNTfNriNJ+pRjU4RGcw3bfJPFmQFD4YzOC5HAnygJWZubZbCZpky6Zi75Np+1OvZB6s/pCYrIw2EYZzHxnxVQaY/zUjnMtpeU3qXt/US9uC2MgdZ1eHBSInLsn/2srFHrw4/+bzmyAoGQI1r8vYDFvCL7uTJ4EkIzu+tmrJyOery15ROx7cw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    private string _privateKey;

    public object GetSignatureKey()
    {
      if (this._privateKey == null)
      {
        this._privateKey = ConfigurationManager.AppSettings["elli.Identity.TokenProvider:PrivateKey"];
        if (string.IsNullOrEmpty(this._privateKey))
          throw new ConfigurationErrorsException("Missing 'elli.Identity.TokenProvider:PrivateKey' in configuration.");
      }
      RSA signatureKey = RSA.Create();
      signatureKey.FromXmlString(this._privateKey);
      return (object) signatureKey;
    }

    public object GetValidationKey()
    {
      RSA validationKey = RSA.Create();
      validationKey.FromXmlString(this._publicKey);
      return (object) validationKey;
    }

    public KeyAlgorithm Algorithm => KeyAlgorithm.RS256;
  }
}
