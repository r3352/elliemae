// Decompiled with JetBrains decompiler
// Type: Encompass.Security.Cryptography.X509KeyProvider
// Assembly: Encompass.Security, Version=24.3.0.5, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: D0C66F5F-92EC-4221-917C-9A4B032D1E4C
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Encompass.Security.dll

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

#nullable disable
namespace Encompass.Security.Cryptography
{
  public class X509KeyProvider : IKeyProvider
  {
    private readonly string _certificatePath;
    private const byte ProviderIdentifier = 2;
    private const string X509KeyIdentifier_Default = "";
    private const string X509CertificateStore_Default = "MY";
    private const string AppConfigCertPathKey = "Encompass.Security.X509CertificatePath";
    private const string AppConfigKeyBlobKey = "Encompass.Security.X509KeyProvider.KeyBlob";
    private readonly int _keySize = 128;
    private byte[] _cryptoKey;

    public X509KeyProvider(byte[] keyData = null)
    {
      this._certificatePath = ConfigurationManager.AppSettings["Encompass.Security.X509CertificatePath"];
      if (string.IsNullOrEmpty(this._certificatePath))
        throw new ArgumentException("Missing required 'Encompass.Security.X509CertificatePath' config setting from application configuration file.");
      byte[] data = keyData;
      if (keyData == null)
      {
        string appSetting = ConfigurationManager.AppSettings["Encompass.Security.X509KeyProvider.KeyBlob"];
        data = !string.IsNullOrEmpty(appSetting) ? Convert.FromBase64String(appSetting) : throw new ArgumentException("Missing required 'Encompass.Security.X509KeyProvider.KeyBlob' config setting from application configuration file.");
      }
      X509Certificate2 certificateFromStore = this.GetCertificateFromStore(this._certificatePath);
      try
      {
        this._cryptoKey = RSACertificateExtensions.GetRSAPrivateKey(certificateFromStore).Decrypt(data, RSAEncryptionPadding.OaepSHA256);
      }
      catch (Exception ex)
      {
        throw new CryptographicException("Failed to decrypt the KeyBlob.  Error: " + ex.Message, ex);
      }
    }

    ~X509KeyProvider()
    {
      if (this._cryptoKey == null)
        return;
      Array.Clear((Array) this._cryptoKey, 0, this._cryptoKey.Length);
    }

    public int KeySize => this._keySize;

    public byte Identifier => 2;

    public byte[] GenerateKey() => (byte[]) null;

    public byte[] GetDataProtectionKey() => this._cryptoKey;

    public object GetSignatureKey() => (object) null;

    public object GetValidationKey() => (object) null;

    public byte[] GenerateCryptoKeyBlob()
    {
      byte[] data = (byte[]) null;
      using (Aes aes = Aes.Create())
      {
        aes.KeySize = 128;
        aes.GenerateKey();
        data = aes.Key;
      }
      return RSACertificateExtensions.GetRSAPublicKey(this.GetCertificateFromStore(this._certificatePath)).Encrypt(data, RSAEncryptionPadding.OaepSHA256);
    }

    public string GenerateCryptoKeyBlobEncoded()
    {
      return Convert.ToBase64String(this.GenerateCryptoKeyBlob());
    }

    private X509Certificate2 GetCertificateFromStore(string certPath)
    {
      X509Certificate2 certificateFromStore = (X509Certificate2) null;
      string[] strArray = !string.IsNullOrEmpty(certPath) ? certPath.Trim().Split('\\') : throw new ArgumentNullException(nameof (certPath), "An empty or null certificate path was provided.");
      if (strArray.Length != 3)
        throw new ArgumentException(nameof (certPath), "An invalid certificate path was provided.");
      try
      {
        using (X509Store x509Store = new X509Store(strArray[1], strArray[0].ToLower() == "currentuser" ? StoreLocation.CurrentUser : StoreLocation.LocalMachine))
        {
          x509Store.Open(OpenFlags.OpenExistingOnly);
          X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByThumbprint, (object) strArray[2], false);
          certificateFromStore = certificate2Collection != null && certificate2Collection.Count >= 1 ? certificate2Collection[0] : throw new CryptographicException("Certificate with hash=" + strArray[2] + " was not found in the " + strArray[0] + "\\" + strArray[1] + " store.");
          if (!certificateFromStore.HasPrivateKey)
            throw new CryptographicException("No private key was found for this certificate.");
        }
      }
      catch (CryptographicException ex)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new CryptographicException("Failed to load the certificate from the provided certificate path. Error: " + ex.Message + " ", ex);
      }
      return certificateFromStore;
    }
  }
}
