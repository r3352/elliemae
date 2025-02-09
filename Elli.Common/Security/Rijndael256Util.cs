// Decompiled with JetBrains decompiler
// Type: Elli.Common.Security.Rijndael256Util
// Assembly: Elli.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 5A516607-8D77-4351-85BB-54751A6A69B4
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\Elli.Common.dll

using EllieMae.EMLite.Common;
using Encompass.Diagnostics;
using Encompass.Security.Cryptography;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Elli.Common.Security
{
  public class Rijndael256Util
  {
    private static IKeyProvider x509KeyProv = (IKeyProvider) null;
    private static bool x509ProvInitialized = false;
    private static readonly byte[] Key = new byte[32]
    {
      (byte) 127,
      (byte) 241,
      (byte) 112,
      (byte) 42,
      (byte) 33,
      (byte) 188,
      (byte) 221,
      (byte) 107,
      (byte) 150,
      (byte) 110,
      (byte) 17,
      (byte) 72,
      (byte) 166,
      (byte) 163,
      (byte) 30,
      (byte) 94,
      (byte) 248,
      (byte) 83,
      (byte) 213,
      (byte) 134,
      (byte) 139,
      (byte) 227,
      (byte) 12,
      (byte) 221,
      (byte) 189,
      (byte) 120,
      (byte) 41,
      (byte) 21,
      (byte) 177,
      (byte) 216,
      (byte) 85,
      (byte) 101
    };
    private static readonly byte[] Iv = new byte[16]
    {
      (byte) 64,
      (byte) 190,
      (byte) 103,
      (byte) 118,
      (byte) 105,
      (byte) 124,
      (byte) 155,
      (byte) 96,
      (byte) 175,
      (byte) 63,
      (byte) 28,
      (byte) 66,
      (byte) 128,
      (byte) 193,
      (byte) 60,
      (byte) 120
    };

    private static IKeyProvider X509KeyProv
    {
      get
      {
        if (!Rijndael256Util.x509ProvInitialized)
        {
          try
          {
            Rijndael256Util.x509KeyProv = (IKeyProvider) new X509KeyProvider();
          }
          catch (Exception ex)
          {
            Tracing.Log(true, TraceLevel.Error.ToString(), nameof (Rijndael256Util), string.Format("Error while initializing X509KeyProvider with exceptionMessage:{0}, Stacktrace:{1}", (object) ex.Message, (object) ex.StackTrace));
          }
          finally
          {
            Rijndael256Util.x509ProvInitialized = true;
          }
        }
        return Rijndael256Util.x509KeyProv;
      }
    }

    public static string Encrypt(string plainText)
    {
      return Convert.ToBase64String(new DataProtection(Rijndael256Util.X509KeyProv != null ? Rijndael256Util.X509KeyProv : Rijndael256Util.getBaseKeyProvider()).Encrypt(Encoding.UTF8.GetBytes(plainText)));
    }

    private static IKeyProvider getBaseKeyProvider()
    {
      return (IKeyProvider) new BaseKeyProvider(Encoding.UTF8.GetBytes(DiagUtility.LoggerScopeProvider.GetCurrent().Instance.ToUpperInvariant()));
    }

    public static string Decrypt(string cypherText)
    {
      if (string.IsNullOrWhiteSpace(cypherText))
        return (string) null;
      byte[] numArray = Convert.FromBase64String(cypherText);
      if (DataProtection.CanDecrypt(numArray))
      {
        byte providerIdentifier = DataProtection.GetProviderIdentifier(numArray);
        return Encoding.UTF8.GetString(new DataProtection(Rijndael256Util.X509KeyProv == null || (int) providerIdentifier != (int) Rijndael256Util.X509KeyProv.Identifier ? Rijndael256Util.getBaseKeyProvider() : Rijndael256Util.X509KeyProv).Decrypt(numArray));
      }
      byte[] array;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
        {
          rijndaelManaged.KeySize = 256;
          rijndaelManaged.BlockSize = 128;
          rijndaelManaged.Key = Rijndael256Util.Key;
          rijndaelManaged.IV = Rijndael256Util.Iv;
          rijndaelManaged.Mode = CipherMode.CBC;
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
          {
            cryptoStream.Write(numArray, 0, numArray.Length);
            cryptoStream.Close();
          }
          array = memoryStream.ToArray();
        }
      }
      return Encoding.UTF8.GetString(array);
    }
  }
}
