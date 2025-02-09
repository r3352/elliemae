// Decompiled with JetBrains decompiler
// Type: Jose.AesKeyWrapManagement
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Collections.Generic;

#nullable disable
namespace Jose
{
  public class AesKeyWrapManagement : IKeyManagement
  {
    private readonly int kekLengthBits;

    public AesKeyWrapManagement(int kekLengthBits) => this.kekLengthBits = kekLengthBits;

    public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
    {
      byte[] numArray1 = Ensure.Type<byte[]>(key, "AesKeyWrap management algorithm expectes key to be byte[] array.");
      Ensure.BitSize(numArray1, this.kekLengthBits, string.Format("AesKeyWrap management algorithm expected key of size {0} bits, but was given {1} bits", (object) this.kekLengthBits, (object) (numArray1.Length * 8)));
      byte[] cek = Arrays.Random(cekSizeBits);
      byte[] numArray2 = AesKeyWrap.Wrap(cek, numArray1);
      return new byte[2][]{ cek, numArray2 };
    }

    public byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      byte[] numArray = Ensure.Type<byte[]>(key, "AesKeyWrap management algorithm expectes key to be byte[] array.");
      Ensure.BitSize(numArray, this.kekLengthBits, string.Format("AesKeyWrap management algorithm expected key of size {0} bits, but was given {1} bits", (object) this.kekLengthBits, (object) (numArray.Length * 8)));
      return AesKeyWrap.Unwrap(encryptedCek, numArray);
    }
  }
}
