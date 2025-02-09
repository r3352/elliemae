// Decompiled with JetBrains decompiler
// Type: Jose.EcdhKeyManagement
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Jose
{
  public class EcdhKeyManagement : IKeyManagement
  {
    private string algIdHeader;

    public EcdhKeyManagement(bool isDirectAgreement)
    {
      this.algIdHeader = isDirectAgreement ? "enc" : "alg";
    }

    public virtual byte[][] WrapNewKey(
      int cekSizeBits,
      object key,
      IDictionary<string, object> header)
    {
      byte[] cek = this.NewKey(cekSizeBits, key, header);
      byte[] numArray = this.Wrap(cek, key);
      return new byte[2][]{ cek, numArray };
    }

    private byte[] NewKey(int keyLength, object key, IDictionary<string, object> header)
    {
      CngKey cngKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.");
      EccKey eccKey = EccKey.Generate(cngKey);
      IDictionary<string, object> dictionary = (IDictionary<string, object>) new Dictionary<string, object>();
      dictionary["kty"] = (object) "EC";
      dictionary["x"] = (object) Base64Url.Encode(eccKey.X);
      dictionary["y"] = (object) Base64Url.Encode(eccKey.Y);
      dictionary["crv"] = (object) this.Curve(cngKey);
      header["epk"] = (object) dictionary;
      return this.DeriveKey(header, keyLength, cngKey, eccKey.Key);
    }

    public virtual byte[] Wrap(byte[] cek, object key) => Arrays.Empty;

    public virtual byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      CngKey privateKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.");
      Ensure.Contains(header, new string[1]{ "epk" }, "EcdhKeyManagement algorithm expects 'epk' key param in JWT header, but was not found");
      Ensure.Contains(header, new string[1]
      {
        this.algIdHeader
      }, "EcdhKeyManagement algorithm expects 'enc' header to be present in JWT header, but was not found");
      IDictionary<string, object> dict = (IDictionary<string, object>) header["epk"];
      Ensure.Contains(dict, new string[3]{ "x", "y", "crv" }, "EcdhKeyManagement algorithm expects 'epk' key to contain 'x','y' and 'crv' fields.");
      CngKey externalPublicKey = EccKey.New(Base64Url.Decode((string) dict["x"]), Base64Url.Decode((string) dict["y"]), usage: CngKeyUsages.KeyAgreement);
      return this.DeriveKey(header, cekSizeBits, externalPublicKey, privateKey);
    }

    private byte[] DeriveKey(
      IDictionary<string, object> header,
      int cekSizeBits,
      CngKey externalPublicKey,
      CngKey privateKey)
    {
      byte[] bytes1 = Encoding.UTF8.GetBytes((string) header[this.algIdHeader]);
      byte[] numArray1 = header.ContainsKey("apv") ? Base64Url.Decode((string) header["apv"]) : Arrays.Empty;
      byte[] numArray2 = header.ContainsKey("apu") ? Base64Url.Decode((string) header["apu"]) : Arrays.Empty;
      byte[] algorithmId = Arrays.Concat(Arrays.IntToBytes(bytes1.Length), bytes1);
      byte[] partyUInfo = Arrays.Concat(Arrays.IntToBytes(numArray2.Length), numArray2);
      byte[] partyVInfo = Arrays.Concat(Arrays.IntToBytes(numArray1.Length), numArray1);
      byte[] bytes2 = Arrays.IntToBytes(cekSizeBits);
      return ConcatKDF.DeriveKey(externalPublicKey, privateKey, cekSizeBits, algorithmId, partyVInfo, partyUInfo, bytes2);
    }

    private string Curve(CngKey key)
    {
      if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP256)
        return "P-256";
      if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP384)
        return "P-384";
      if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP521)
        return "P-521";
      throw new ArgumentException("Unknown curve type " + (object) key.Algorithm);
    }
  }
}
