// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.XT
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Encompass.Diagnostics;
using Encompass.Security.Cryptography;
using System;
using System.Diagnostics;
using System.Text;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class XT
  {
    private const uint delta = 2654435769;
    private static IKeyProvider x509KeyProv;
    private static bool x509ProvInitialized;

    public static string ESB64(string d, string k) => XT.Encrypt(d);

    private static IKeyProvider X509KeyProv
    {
      get
      {
        if (!XT.x509ProvInitialized)
        {
          try
          {
            XT.x509KeyProv = (IKeyProvider) new X509KeyProvider();
          }
          catch (Exception ex)
          {
            Tracing.Log(true, TraceLevel.Error.ToString(), nameof (XT), string.Format("Error while initializing X509KeyProvider with exceptionMessage:{0}, Stacktrace:{1}", (object) ex.Message, (object) ex.StackTrace));
          }
          finally
          {
            XT.x509ProvInitialized = true;
          }
        }
        return XT.x509KeyProv;
      }
    }

    public static string ESB64Legacy(string d, string k)
    {
      switch (d)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          uint[] k1 = XT.fk(k);
          if (d.Length % 2 != 0)
            d += "\0";
          byte[] bytes = Encoding.ASCII.GetBytes(d);
          string str = string.Empty;
          uint[] v = new uint[2];
          for (int index = 0; index < bytes.Length; index += 2)
          {
            v[0] = (uint) bytes[index];
            v[1] = (uint) bytes[index + 1];
            XT.c(v, k1);
            str = str + XT.toS(v[0]) + XT.toS(v[1]);
          }
          byte[] inArray = new byte[str.Length];
          for (int index = 0; index < str.Length; ++index)
            inArray[index] = (byte) str[index];
          return Convert.ToBase64String(inArray);
      }
    }

    public static string DSB64(string dd, string k)
    {
      if (string.IsNullOrWhiteSpace(dd))
        return (string) null;
      if (DataProtection.CanDecrypt(Convert.FromBase64String(dd)))
        return XT.Decrypt(dd);
      switch (dd)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          byte[] numArray1 = Convert.FromBase64String(dd);
          StringBuilder stringBuilder = new StringBuilder();
          for (int index = 0; index < numArray1.Length; ++index)
            stringBuilder.Append((char) numArray1[index]);
          dd = stringBuilder.ToString();
          uint[] k1 = XT.fk(k);
          int num1 = 0;
          uint[] v = new uint[2];
          byte[] bytes = new byte[dd.Length / 8 * 2];
          for (int startIndex = 0; startIndex < dd.Length; startIndex += 8)
          {
            v[0] = XT.toUI(dd.Substring(startIndex, 4));
            v[1] = XT.toUI(dd.Substring(startIndex + 4, 4));
            XT.d(v, k1);
            byte[] numArray2 = bytes;
            int index1 = num1;
            int num2 = index1 + 1;
            int num3 = (int) (byte) v[0];
            numArray2[index1] = (byte) num3;
            byte[] numArray3 = bytes;
            int index2 = num2;
            num1 = index2 + 1;
            int num4 = (int) (byte) v[1];
            numArray3[index2] = (byte) num4;
          }
          string str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
          if (str[str.Length - 1] == char.MinValue)
            str = str.Substring(0, str.Length - 1);
          return str;
      }
    }

    public static string ESB64x2(string d, string k) => XT.Encrypt(d);

    public static string DSB64x2(string dd, string k)
    {
      if (string.IsNullOrWhiteSpace(dd))
        return (string) null;
      if (DataProtection.CanDecrypt(Convert.FromBase64String(dd)))
        return XT.Decrypt(dd);
      switch (dd)
      {
        case null:
          return (string) null;
        case "":
          return "";
        default:
          byte[] inVal = Convert.FromBase64String(dd);
          if (inVal.Length % 8 != 0)
            throw new ArgumentException("Data must be in multiples of 8 characters in length.");
          uint[] k1 = XT.fk(k);
          uint[] numArray1 = new uint[2];
          byte[] numArray2 = new byte[inVal.Length];
          for (int index = 0; index < inVal.Length; index += 8)
          {
            XT.ByteArrayToUintArray(inVal, index, numArray1, 0);
            XT.d(numArray1, k1);
            XT.UintArrayToByteArray(numArray1, 0, numArray2, index);
          }
          string str = Encoding.ASCII.GetString(numArray2, 0, numArray2.Length);
          int length = str.IndexOf(char.MinValue);
          if (length > 0)
            str = str.Substring(0, length);
          return str;
      }
    }

    private static IKeyProvider getBaseKeyProvider()
    {
      return (IKeyProvider) new BaseKeyProvider(Encoding.UTF8.GetBytes(DiagUtility.LoggerScopeProvider.GetCurrent().Instance.ToUpperInvariant()));
    }

    private static string Encrypt(string plainText)
    {
      return string.IsNullOrEmpty(plainText) ? plainText : Convert.ToBase64String(new DataProtection(XT.X509KeyProv != null ? XT.X509KeyProv : XT.getBaseKeyProvider()).Encrypt(Encoding.UTF8.GetBytes(plainText)));
    }

    private static string Decrypt(string cypherText)
    {
      if (string.IsNullOrEmpty(cypherText))
        return cypherText;
      byte[] cipherText = Convert.FromBase64String(cypherText);
      byte providerIdentifier = DataProtection.GetProviderIdentifier(cipherText);
      return Encoding.UTF8.GetString(new DataProtection(XT.X509KeyProv == null || (int) providerIdentifier != (int) XT.X509KeyProv.Identifier ? XT.getBaseKeyProvider() : XT.X509KeyProv).Decrypt(cipherText));
    }

    public static string TryDSB64x2(string dd, string k)
    {
      try
      {
        return XT.DSB64x2(dd, k);
      }
      catch
      {
        return string.Empty;
      }
    }

    private static uint[] fk(string k)
    {
      k = k.Length != 0 ? k.PadRight(16, ' ').Substring(0, 16) : throw new ArgumentException("K must be between 1 and 16 characters in length");
      uint[] numArray = new uint[4];
      int num = 0;
      for (int startIndex = 0; startIndex < k.Length; startIndex += 4)
        numArray[num++] = XT.toUI(k.Substring(startIndex, 4));
      return numArray;
    }

    private static void c(uint[] v, uint[] k)
    {
      uint num1 = v[0];
      uint num2 = v[1];
      uint num3 = 0;
      uint num4 = 32;
      while (num4-- > 0U)
      {
        num1 += (uint) (((int) num2 << 4 ^ (int) (num2 >> 5)) + (int) num2 ^ (int) num3 + (int) k[(int) num3 & 3]);
        num3 += 2654435769U;
        num2 += (uint) (((int) num1 << 4 ^ (int) (num1 >> 5)) + (int) num1 ^ (int) num3 + (int) k[(int) (num3 >> 11) & 3]);
      }
      v[0] = num1;
      v[1] = num2;
    }

    private static void d(uint[] v, uint[] k)
    {
      uint num1 = v[0];
      uint num2 = v[1];
      uint num3 = 3337565984;
      uint num4 = 32;
      while (num4-- > 0U)
      {
        num2 -= (uint) (((int) num1 << 4 ^ (int) (num1 >> 5)) + (int) num1 ^ (int) num3 + (int) k[(int) (num3 >> 11) & 3]);
        num3 -= 2654435769U;
        num1 -= (uint) (((int) num2 << 4 ^ (int) (num2 >> 5)) + (int) num2 ^ (int) num3 + (int) k[(int) num3 & 3]);
      }
      v[0] = num1;
      v[1] = num2;
    }

    private static uint toUI(string Input)
    {
      return (uint) Input[0] + ((uint) Input[1] << 8) + ((uint) Input[2] << 16) + ((uint) Input[3] << 24);
    }

    private static string toS(uint Input)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append((char) (Input & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 8 & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 16 & (uint) byte.MaxValue));
      stringBuilder.Append((char) (Input >> 24 & (uint) byte.MaxValue));
      return stringBuilder.ToString();
    }

    private static void ByteArrayToUintArray(byte[] inVal, int inIdx, uint[] outVal, int outIdx)
    {
      outVal[outIdx] = (uint) ((int) inVal[inIdx] + ((int) inVal[inIdx + 1] << 8) + ((int) inVal[inIdx + 2] << 16) + ((int) inVal[inIdx + 3] << 24));
      outVal[outIdx + 1] = (uint) ((int) inVal[inIdx + 4] + ((int) inVal[inIdx + 5] << 8) + ((int) inVal[inIdx + 6] << 16) + ((int) inVal[inIdx + 7] << 24));
    }

    private static void UintArrayToByteArray(uint[] inVal, int inIdx, byte[] outVal, int outIdx)
    {
      int num1 = outIdx;
      byte[] numArray1 = outVal;
      int index1 = num1;
      int num2 = index1 + 1;
      int num3 = (int) (byte) (inVal[inIdx] & (uint) byte.MaxValue);
      numArray1[index1] = (byte) num3;
      byte[] numArray2 = outVal;
      int index2 = num2;
      int num4 = index2 + 1;
      int num5 = (int) (byte) (inVal[inIdx] >> 8 & (uint) byte.MaxValue);
      numArray2[index2] = (byte) num5;
      byte[] numArray3 = outVal;
      int index3 = num4;
      int num6 = index3 + 1;
      int num7 = (int) (byte) (inVal[inIdx] >> 16 & (uint) byte.MaxValue);
      numArray3[index3] = (byte) num7;
      byte[] numArray4 = outVal;
      int index4 = num6;
      int num8 = index4 + 1;
      int num9 = (int) (byte) (inVal[inIdx] >> 24 & (uint) byte.MaxValue);
      numArray4[index4] = (byte) num9;
      byte[] numArray5 = outVal;
      int index5 = num8;
      int num10 = index5 + 1;
      int num11 = (int) (byte) (inVal[inIdx + 1] & (uint) byte.MaxValue);
      numArray5[index5] = (byte) num11;
      byte[] numArray6 = outVal;
      int index6 = num10;
      int num12 = index6 + 1;
      int num13 = (int) (byte) (inVal[inIdx + 1] >> 8 & (uint) byte.MaxValue);
      numArray6[index6] = (byte) num13;
      byte[] numArray7 = outVal;
      int index7 = num12;
      int index8 = index7 + 1;
      int num14 = (int) (byte) (inVal[inIdx + 1] >> 16 & (uint) byte.MaxValue);
      numArray7[index7] = (byte) num14;
      outVal[index8] = (byte) (inVal[inIdx + 1] >> 24 & (uint) byte.MaxValue);
    }
  }
}
