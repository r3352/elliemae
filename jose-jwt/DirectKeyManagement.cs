// Decompiled with JetBrains decompiler
// Type: Jose.DirectKeyManagement
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using System.Collections.Generic;

#nullable disable
namespace Jose
{
  public class DirectKeyManagement : IKeyManagement
  {
    public byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
    {
      return new byte[2][]
      {
        Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array."),
        Arrays.Empty
      };
    }

    public byte[] Unwrap(
      byte[] encryptedCek,
      object key,
      int cekSizeBits,
      IDictionary<string, object> header)
    {
      Ensure.IsEmpty(encryptedCek, "DirectKeyManagement expects empty content encryption key.");
      return Ensure.Type<byte[]>(key, "DirectKeyManagement alg expectes key to be byte[] array.");
    }
  }
}
