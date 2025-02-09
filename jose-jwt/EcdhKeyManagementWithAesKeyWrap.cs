// Decompiled with JetBrains decompiler
// Type: Jose.EcdhKeyManagementWithAesKeyWrap
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Collections.Generic;

#nullable disable
namespace Jose
{
  public class EcdhKeyManagementWithAesKeyWrap : EcdhKeyManagement
  {
    private AesKeyWrapManagement aesKW;
    private int keyLengthBits;

    public EcdhKeyManagementWithAesKeyWrap(int keyLengthBits, AesKeyWrapManagement aesKw)
      : base(false)
    {
      this.aesKW = aesKw;
      this.keyLengthBits = keyLengthBits;
    }

    public override byte[][] WrapNewKey(
      int cekSizeBits,
      object key,
      IDictionary<string, object> header)
    {
      byte[] key1 = base.WrapNewKey(this.keyLengthBits, key, header)[0];
      return this.aesKW.WrapNewKey(cekSizeBits, (object) key1, header);
    }

    public override byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      byte[] key1 = base.Unwrap(Arrays.Empty, key, this.keyLengthBits, header);
      return this.aesKW.Unwrap(encryptedCek, (object) key1, cekSizeBits, header);
    }
  }
}
