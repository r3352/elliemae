// Decompiled with JetBrains decompiler
// Type: Jose.Pbse2HmacShaKeyManagementWithAesKeyWrap
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Jose
{
  public class Pbse2HmacShaKeyManagementWithAesKeyWrap : IKeyManagement
  {
    private AesKeyWrapManagement aesKW;
    private int keyLengthBits;

    public Pbse2HmacShaKeyManagementWithAesKeyWrap(int keyLengthBits, AesKeyWrapManagement aesKw)
    {
      this.aesKW = aesKw;
      this.keyLengthBits = keyLengthBits;
    }

    public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
    {
      byte[] bytes1 = Encoding.UTF8.GetBytes(Ensure.Type<string>(key, "Pbse2HmacShaKeyManagementWithAesKeyWrap management algorithm expectes key to be string."));
      byte[] bytes2 = Encoding.UTF8.GetBytes((string) header["alg"]);
      int iterationCount = 8192;
      byte[] input = Arrays.Random(96);
      header["p2c"] = (object) iterationCount;
      header["p2s"] = (object) Base64Url.Encode(input);
      byte[] salt = Arrays.Concat(bytes2, Arrays.Zero, input);
      byte[] key1;
      using (HMAC prf = this.PRF)
        key1 = PBKDF2.DeriveKey(bytes1, salt, iterationCount, this.keyLengthBits, prf);
      return this.aesKW.WrapNewKey(cekSizeBits, (object) key1, header);
    }

    public byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      byte[] bytes1 = Encoding.UTF8.GetBytes(Ensure.Type<string>(key, "Pbse2HmacShaKeyManagementWithAesKeyWrap management algorithm expectes key to be string."));
      Ensure.Contains(header, new string[1]{ "p2c" }, "Pbse2HmacShaKeyManagementWithAesKeyWrap algorithm expects 'p2c' param in JWT header, but was not found");
      Ensure.Contains(header, new string[1]{ "p2s" }, "Pbse2HmacShaKeyManagementWithAesKeyWrap algorithm expects 'p2s' param in JWT header, but was not found");
      byte[] bytes2 = Encoding.UTF8.GetBytes((string) header["alg"]);
      int iterationCount = (int) header["p2c"];
      byte[] numArray = Base64Url.Decode((string) header["p2s"]);
      byte[] salt = Arrays.Concat(bytes2, Arrays.Zero, numArray);
      byte[] key1;
      using (HMAC prf = this.PRF)
        key1 = PBKDF2.DeriveKey(bytes1, salt, iterationCount, this.keyLengthBits, prf);
      return this.aesKW.Unwrap(encryptedCek, (object) key1, cekSizeBits, header);
    }

    private HMAC PRF
    {
      get
      {
        if (this.keyLengthBits == 128)
          return (HMAC) new HMACSHA256();
        if (this.keyLengthBits == 192)
          return (HMAC) new HMACSHA384();
        if (this.keyLengthBits == 256)
          return (HMAC) new HMACSHA512();
        throw new ArgumentException(string.Format("Unsupported key size: '{0}'", (object) this.keyLengthBits));
      }
    }
  }
}
