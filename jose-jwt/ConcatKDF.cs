// Decompiled with JetBrains decompiler
// Type: Jose.ConcatKDF
// Assembly: jose-jwt, Version=1.8.0.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 80B48F01-9E85-4307-AD69-2D3CD0AE6B86
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\jose-jwt.dll

using Jose.native;
using Microsoft.Win32.SafeHandles;
using System;
using System.Security.Cryptography;

#nullable disable
namespace Jose
{
  public class ConcatKDF
  {
    public static byte[] DeriveKey(
      CngKey externalPubKey,
      CngKey privateKey,
      int keyBitLength,
      byte[] algorithmId,
      byte[] partyVInfo,
      byte[] partyUInfo,
      byte[] suppPubInfo)
    {
      using (ECDiffieHellmanCng diffieHellmanCng = new ECDiffieHellmanCng(privateKey))
      {
        using (SafeNCryptSecretHandle hSharedSecret = diffieHellmanCng.DeriveSecretAgreementHandle(externalPubKey))
        {
          using (NCrypt.NCryptBuffer ncryptBuffer1 = new NCrypt.NCryptBuffer(8U, algorithmId))
          {
            using (NCrypt.NCryptBuffer ncryptBuffer2 = new NCrypt.NCryptBuffer(10U, partyVInfo))
            {
              using (NCrypt.NCryptBuffer ncryptBuffer3 = new NCrypt.NCryptBuffer(9U, partyUInfo))
              {
                using (NCrypt.NCryptBuffer ncryptBuffer4 = new NCrypt.NCryptBuffer(11U, suppPubInfo))
                {
                  using (NCrypt.NCryptBufferDesc parameterList = new NCrypt.NCryptBufferDesc(new NCrypt.NCryptBuffer[4]
                  {
                    ncryptBuffer1,
                    ncryptBuffer2,
                    ncryptBuffer3,
                    ncryptBuffer4
                  }))
                  {
                    uint result;
                    uint num1 = NCrypt.NCryptDeriveKey(hSharedSecret, "SP800_56A_CONCAT", parameterList, (byte[]) null, 0U, out result, 0U);
                    if (num1 != 0U)
                      throw new CryptographicException(string.Format("NCrypt.NCryptDeriveKey() failed with status code:{0}", (object) num1));
                    byte[] numArray = new byte[(IntPtr) result];
                    uint num2 = NCrypt.NCryptDeriveKey(hSharedSecret, "SP800_56A_CONCAT", parameterList, numArray, result, out result, 0U);
                    if (num2 != 0U)
                      throw new CryptographicException(string.Format("NCrypt.NCryptDeriveKey() failed with status code:{0}", (object) num2));
                    return Arrays.LeftmostBits(numArray, keyBitLength);
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
