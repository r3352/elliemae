// Decompiled with JetBrains decompiler
// Type: Jose.AesGcmEncryption
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.jwe;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class AesGcmEncryption : IJweAlgorithm
  {
    private int keyLength;

    public AesGcmEncryption(int keyLength) => this.keyLength = keyLength;

    public byte[][] Encrypt(byte[] aad, byte[] plainText, byte[] cek)
    {
      Ensure.BitSize(cek, this.keyLength, string.Format("AES-GCM algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLength, (object) (cek.Length * 8)));
      byte[] iv = Arrays.Random(96);
      try
      {
        byte[][] numArray = AesGcm.Encrypt(cek, iv, aad, plainText);
        return new byte[3][]{ iv, numArray[0], numArray[1] };
      }
      catch (CryptographicException ex)
      {
        throw new EncryptionException("Unable to encrypt content.", (Exception) ex);
      }
    }

    public byte[] Decrypt(byte[] aad, byte[] cek, byte[] iv, byte[] cipherText, byte[] authTag)
    {
      Ensure.BitSize(cek, this.keyLength, string.Format("AES-GCM algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLength, (object) (cek.Length * 8)));
      try
      {
        return AesGcm.Decrypt(cek, iv, aad, cipherText, authTag);
      }
      catch (CryptographicException ex)
      {
        throw new EncryptionException("Unable to decrypt content or authentication tag do not match.", (Exception) ex);
      }
    }

    public int KeySize => this.keyLength;
  }
}
