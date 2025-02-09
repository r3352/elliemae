// Decompiled with JetBrains decompiler
// Type: Security.Cryptography.RsaKey
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Security.Cryptography
{
  public class RsaKey
  {
    public static readonly byte[] BCRYPT_RSAPUBLIC_MAGIC = BitConverter.GetBytes(826364754);
    public static readonly byte[] BCRYPT_RSAPRIVATE_MAGIC = BitConverter.GetBytes(843141970);

    public static CngKey New(RSAParameters parameters)
    {
      return RsaKey.New(parameters.Exponent, parameters.Modulus, parameters.P, parameters.Q);
    }

    public static CngKey New(byte[] exponent, byte[] modulus, byte[] p = null, byte[] q = null)
    {
      bool flag = p == null || q == null;
      return CngKey.Import(Arrays.Concat(flag ? RsaKey.BCRYPT_RSAPUBLIC_MAGIC : RsaKey.BCRYPT_RSAPRIVATE_MAGIC, BitConverter.GetBytes(modulus.Length * 8), BitConverter.GetBytes(exponent.Length), BitConverter.GetBytes(modulus.Length), flag ? BitConverter.GetBytes(0) : BitConverter.GetBytes(p.Length), flag ? BitConverter.GetBytes(0) : BitConverter.GetBytes(q.Length), exponent, modulus, p, q), flag ? CngKeyBlobFormat.GenericPublicBlob : CngKeyBlobFormat.GenericPrivateBlob);
    }
  }
}
