// Decompiled with JetBrains decompiler
// Type: Jose.AesGcmKeyWrapManagement
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Collections.Generic;

#nullable disable
namespace Jose
{
  public class AesGcmKeyWrapManagement : IKeyManagement
  {
    private int keyLengthBits;

    public AesGcmKeyWrapManagement(int keyLengthBits) => this.keyLengthBits = keyLengthBits;

    public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
    {
      byte[] numArray1 = Ensure.Type<byte[]>(key, "AesGcmKeyWrapManagement alg expectes key to be byte[] array.");
      Ensure.BitSize(numArray1, this.keyLengthBits, string.Format("AesGcmKeyWrapManagement management algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLengthBits, (object) (numArray1.Length * 8)));
      byte[] numArray2 = Arrays.Random(96);
      byte[] plainText = Arrays.Random(cekSizeBits);
      byte[][] numArray3 = AesGcm.Encrypt(numArray1, numArray2, (byte[]) null, plainText);
      header["iv"] = (object) Base64Url.Encode(numArray2);
      header["tag"] = (object) Base64Url.Encode(numArray3[1]);
      return new byte[2][]{ plainText, numArray3[0] };
    }

    public byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      byte[] numArray = Ensure.Type<byte[]>(key, "AesGcmKeyWrapManagement alg expectes key to be byte[] array.");
      Ensure.BitSize(numArray, this.keyLengthBits, string.Format("AesGcmKeyWrapManagement management algorithm expected key of size {0} bits, but was given {1} bits", (object) this.keyLengthBits, (object) (numArray.Length * 8)));
      Ensure.Contains(header, new string[1]{ "iv" }, "AesGcmKeyWrapManagement algorithm expects 'iv' param in JWT header, but was not found");
      Ensure.Contains(header, new string[1]{ "tag" }, "AesGcmKeyWrapManagement algorithm expects 'tag' param in JWT header, but was not found");
      byte[] iv = Base64Url.Decode((string) header["iv"]);
      byte[] authTag = Base64Url.Decode((string) header["tag"]);
      return AesGcm.Decrypt(numArray, iv, (byte[]) null, encryptedCek, authTag);
    }
  }
}
