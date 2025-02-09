// Decompiled with JetBrains decompiler
// Type: Security.Cryptography.EccKey
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Security.Cryptography
{
  public class EccKey
  {
    public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P256_MAGIC = BitConverter.GetBytes(827540293);
    public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P256_MAGIC = BitConverter.GetBytes(844317509);
    public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P384_MAGIC = BitConverter.GetBytes(861094725);
    public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P384_MAGIC = BitConverter.GetBytes(877871941);
    public static readonly byte[] BCRYPT_ECDSA_PUBLIC_P521_MAGIC = BitConverter.GetBytes(894649157);
    public static readonly byte[] BCRYPT_ECDSA_PRIVATE_P521_MAGIC = BitConverter.GetBytes(911426373);
    public static readonly byte[] BCRYPT_ECDH_PUBLIC_P256_MAGIC = BitConverter.GetBytes(827016005);
    public static readonly byte[] BCRYPT_ECDH_PRIVATE_P256_MAGIC = BitConverter.GetBytes(843793221);
    public static readonly byte[] BCRYPT_ECDH_PUBLIC_P384_MAGIC = BitConverter.GetBytes(860570437);
    public static readonly byte[] BCRYPT_ECDH_PRIVATE_P384_MAGIC = BitConverter.GetBytes(877347653);
    public static readonly byte[] BCRYPT_ECDH_PUBLIC_P521_MAGIC = BitConverter.GetBytes(894124869);
    public static readonly byte[] BCRYPT_ECDH_PRIVATE_P521_MAGIC = BitConverter.GetBytes(910902085);
    private CngKey key;
    private byte[] x;
    private byte[] y;
    private byte[] d;

    public byte[] X
    {
      get
      {
        if (this.x == null)
          this.ExportKey();
        return this.x;
      }
    }

    public byte[] Y
    {
      get
      {
        if (this.y == null)
          this.ExportKey();
        return this.y;
      }
    }

    public byte[] D
    {
      get
      {
        if (this.d == null)
          this.ExportKey();
        return this.d;
      }
    }

    public CngKey Key => this.key;

    public static CngKey New(byte[] x, byte[] y, byte[] d = null, CngKeyUsages usage = CngKeyUsages.Signing)
    {
      if (x.Length != y.Length)
        throw new ArgumentException("X,Y and D must be same size");
      if (d != null && x.Length != d.Length)
        throw new ArgumentException("X,Y and D must be same size");
      if (usage != CngKeyUsages.Signing && usage != CngKeyUsages.KeyAgreement)
        throw new ArgumentException("Usage parameter expected to be set either 'CngKeyUsages.Signing' or 'CngKeyUsages.KeyAgreement");
      bool flag = usage == CngKeyUsages.Signing;
      int length = x.Length;
      byte[] numArray;
      switch (length)
      {
        case 32:
          numArray = d == null ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P256_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P256_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P256_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P256_MAGIC);
          break;
        case 48:
          numArray = d == null ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P384_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P384_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P384_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P384_MAGIC);
          break;
        case 66:
          numArray = d == null ? (flag ? EccKey.BCRYPT_ECDSA_PUBLIC_P521_MAGIC : EccKey.BCRYPT_ECDH_PUBLIC_P521_MAGIC) : (flag ? EccKey.BCRYPT_ECDSA_PRIVATE_P521_MAGIC : EccKey.BCRYPT_ECDH_PRIVATE_P521_MAGIC);
          break;
        default:
          throw new ArgumentException("Size of X,Y or D must equal to 32, 48 or 66 bytes");
      }
      byte[] bytes = BitConverter.GetBytes(length);
      byte[] keyBlob;
      CngKeyBlobFormat format;
      if (d == null)
      {
        keyBlob = Arrays.Concat(numArray, bytes, x, y);
        format = CngKeyBlobFormat.EccPublicBlob;
      }
      else
      {
        keyBlob = Arrays.Concat(numArray, bytes, x, y, d);
        format = CngKeyBlobFormat.EccPrivateBlob;
      }
      return CngKey.Import(keyBlob, format);
    }

    public static EccKey Generate(CngKey recieverPubKey)
    {
      CngKey cngKey = CngKey.Create(recieverPubKey.Algorithm, (string) null, new CngKeyCreationParameters()
      {
        ExportPolicy = new CngExportPolicies?(CngExportPolicies.AllowPlaintextExport)
      });
      return new EccKey() { key = cngKey };
    }

    public static EccKey Export(CngKey _key)
    {
      return new EccKey() { key = _key };
    }

    private void ExportKey()
    {
      byte[] data = this.key.Export(CngKeyBlobFormat.EccPrivateBlob);
      int int32 = BitConverter.ToInt32(new byte[4]
      {
        data[4],
        data[5],
        data[6],
        data[7]
      }, 0);
      byte[][] numArray = Arrays.Slice(Arrays.RightmostBits(data, int32 * 24), int32);
      this.x = numArray[0];
      this.y = numArray[1];
      this.d = numArray[2];
    }
  }
}
