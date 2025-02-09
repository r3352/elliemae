// Decompiled with JetBrains decompiler
// Type: Jose.AesCbcHmacEncryption
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.jwe;
using System;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class AesCbcHmacEncryption : IJweAlgorithm
  {
    private IJwsAlgorithm hashAlgorithm;
    private readonly int keyLength;

    public AesCbcHmacEncryption(IJwsAlgorithm hashAlgorithm, int keyLength)
    {
      this.hashAlgorithm = hashAlgorithm;
      this.keyLength = keyLength;
    }

    public byte[][] Encrypt(byte[] aad, byte[] plainText, byte[] cek)
    {
      Ensure.BitSize(cek, this.keyLength, string.Format("AES-CBC with HMAC algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLength, (object) (cek.Length * 8)));
      byte[] hmacKey = Arrays.FirstHalf(cek);
      byte[] numArray = Arrays.SecondHalf(cek);
      byte[] iv = Arrays.Random();
      byte[] array;
      try
      {
        using (Aes aes = (Aes) new AesManaged())
        {
          aes.Key = numArray;
          aes.IV = iv;
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
              using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
              {
                cryptoStream.Write(plainText, 0, plainText.Length);
                cryptoStream.FlushFinalBlock();
                array = memoryStream.ToArray();
              }
            }
          }
        }
      }
      catch (CryptographicException ex)
      {
        throw new EncryptionException("Unable to encrypt content.", (Exception) ex);
      }
      byte[] authTag = this.ComputeAuthTag(aad, iv, array, hmacKey);
      return new byte[3][]{ iv, array, authTag };
    }

    public byte[] Decrypt(byte[] aad, byte[] cek, byte[] iv, byte[] cipherText, byte[] authTag)
    {
      Ensure.BitSize(cek, this.keyLength, string.Format("AES-CBC with HMAC algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLength, (object) (cek.Length * 8)));
      byte[] hmacKey = Arrays.FirstHalf(cek);
      byte[] numArray = Arrays.SecondHalf(cek);
      if (!Arrays.ConstantTimeEquals(this.ComputeAuthTag(aad, iv, cipherText, hmacKey), authTag))
        throw new IntegrityException("Authentication tag do not match.");
      try
      {
        using (Aes aes = (Aes) new AesManaged())
        {
          aes.Key = numArray;
          aes.IV = iv;
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
              using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Write))
              {
                cryptoStream.Write(cipherText, 0, cipherText.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
              }
            }
          }
        }
      }
      catch (CryptographicException ex)
      {
        throw new EncryptionException("Unable to decrypt content", (Exception) ex);
      }
    }

    public int KeySize => this.keyLength;

    private byte[] ComputeAuthTag(byte[] aad, byte[] iv, byte[] cipherText, byte[] hmacKey)
    {
      byte[] bytes = Arrays.LongToBytes((long) (aad.Length * 8));
      return Arrays.FirstHalf(this.hashAlgorithm.Sign(Arrays.Concat(aad, iv, cipherText, bytes), (object) hmacKey));
    }
  }
}
