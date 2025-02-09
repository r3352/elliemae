// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Key.Ellix509KeyFactory
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Elli.Identity.Key
{
  public class Ellix509KeyFactory : ITokenKeyFactory
  {
    public object GetSignatureKey()
    {
      RSACryptoServiceProvider privateKey = new X509Certificate2(ConfigurationManager.AppSettings["elli.Identity.Certificates:PfxFile"], ConfigurationManager.AppSettings["elli.Identity.Certificates:PfxPassword"], X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable).PrivateKey as RSACryptoServiceProvider;
      byte[] keyBlob;
      try
      {
        keyBlob = privateKey.ExportCspBlob(true);
      }
      catch
      {
        throw new ApplicationException("Private key fails to export");
      }
      RSACryptoServiceProvider signatureKey = new RSACryptoServiceProvider(new CspParameters(24));
      signatureKey.ImportCspBlob(keyBlob);
      return (object) signatureKey;
    }

    public object GetValidationKey()
    {
      return (object) (new X509Certificate2(ConfigurationManager.AppSettings["elli.Identity.Certificates:PublicFile"]).PublicKey.Key as RSACryptoServiceProvider);
    }

    public KeyAlgorithm Algorithm => KeyAlgorithm.RS256;
  }
}
