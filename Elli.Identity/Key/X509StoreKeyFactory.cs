// Decompiled with JetBrains decompiler
// Type: Elli.Identity.Key.X509StoreKeyFactory
// Assembly: Elli.Identity, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0ECC974-8E0E-42B3-88D3-2EAE5F37B212
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Identity.dll

using System;
using System.Configuration;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Elli.Identity.Key
{
  public class X509StoreKeyFactory : ITokenKeyFactory
  {
    public object GetSignatureKey()
    {
      StoreLocation storeLocation = StoreLocation.CurrentUser;
      string appSetting = ConfigurationManager.AppSettings["elli.Identity.Certificates:StoreLocation"];
      if (appSetting != null && appSetting.Equals("LocalMachine", StringComparison.OrdinalIgnoreCase))
        storeLocation = StoreLocation.LocalMachine;
      X509Store x509Store = new X509Store(StoreName.My, storeLocation);
      x509Store.Open(OpenFlags.OpenExistingOnly);
      RSACryptoServiceProvider cryptoServiceProvider;
      try
      {
        X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) ConfigurationManager.AppSettings["elli.Identity.Certificates:StoreHash"], true);
        cryptoServiceProvider = certificate2Collection.Count >= 1 ? certificate2Collection[0].PrivateKey as RSACryptoServiceProvider : throw new SecurityException("The requested certificate was not found in the store.");
      }
      finally
      {
        x509Store.Close();
      }
      byte[] keyBlob;
      try
      {
        keyBlob = cryptoServiceProvider.ExportCspBlob(true);
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
      StoreLocation storeLocation = StoreLocation.CurrentUser;
      string appSetting = ConfigurationManager.AppSettings["elli.Identity.Certificates:StoreLocation"];
      if (appSetting != null && appSetting.Equals("LocalMachine", StringComparison.OrdinalIgnoreCase))
        storeLocation = StoreLocation.LocalMachine;
      X509Store x509Store = new X509Store(StoreName.My, storeLocation);
      x509Store.Open(OpenFlags.OpenExistingOnly);
      X509Certificate2 x509Certificate2;
      try
      {
        X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) ConfigurationManager.AppSettings["elli.Identity.Certificates:StoreHash"], true);
        x509Certificate2 = certificate2Collection.Count >= 1 ? certificate2Collection[0] : throw new SecurityException("The requested certificate was not found in the store.");
      }
      finally
      {
        x509Store.Close();
      }
      return (object) (x509Certificate2.PublicKey.Key as RSACryptoServiceProvider);
    }

    public KeyAlgorithm Algorithm => KeyAlgorithm.RS256;
  }
}
