// Decompiled with JetBrains decompiler
// Type: Jose.HmacUsingSha
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class HmacUsingSha : IJwsAlgorithm
  {
    private string hashMethod;

    public HmacUsingSha(string hashMethod) => this.hashMethod = hashMethod;

    public byte[] Sign(byte[] securedInput, object key)
    {
      using (KeyedHashAlgorithm keyedHashAlgorithm = this.KeyedHash(Ensure.Type<byte[]>(key, "HmacUsingSha alg expectes key to be byte[] array.")))
        return keyedHashAlgorithm.ComputeHash(securedInput);
    }

    public bool Verify(byte[] signature, byte[] securedInput, object key)
    {
      byte[] actual = this.Sign(securedInput, key);
      return Arrays.ConstantTimeEquals(signature, actual);
    }

    private KeyedHashAlgorithm KeyedHash(byte[] key)
    {
      if ("SHA256".Equals(this.hashMethod))
        return (KeyedHashAlgorithm) new HMACSHA256(key);
      if ("SHA384".Equals(this.hashMethod))
        return (KeyedHashAlgorithm) new HMACSHA384(key);
      if ("SHA512".Equals(this.hashMethod))
        return (KeyedHashAlgorithm) new HMACSHA512(key);
      throw new ArgumentException("Unsupported hashing algorithm: '{0}'", this.hashMethod);
    }
  }
}
