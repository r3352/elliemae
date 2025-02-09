// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Licensing.LicenseHelper
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using EllieMae.EMLite.JedLib;
using System;
using System.Globalization;
using System.Security.Permissions;

#nullable disable
namespace EllieMae.EMLite.Common.Licensing
{
  [StrongNameIdentityPermission(SecurityAction.LinkDemand, PublicKey = "0024000004800000940000000602000000240000525341310004000001000100ED611F91C2EEA49D628904356176F8405967CF8FD01EE7F2914038B5BA5F70C9EB8BE8489CD4F8E6DC00CE61D5127EE2B7D97AC08EA11B7D33829FFE313EDB7408F67A09F6226F942F86B01311DC7EE0088AA29491178EFEFF12B2826097CE5CAEE7B0DF8E69EE50D07034E8F79FA95CCD98BF9D9615E9EF53D5647217C330B9")]
  internal class LicenseHelper
  {
    private const int KeyLength = 8;

    public LicenseHelper() => a.a("abc0xyx8jfiwkcpo");

    public string BytesToString(byte[] bytes)
    {
      string str = "";
      Random random = new Random();
      byte[] buffer = new byte[8];
      random.NextBytes(buffer);
      for (int index = 0; index < buffer.Length; ++index)
        str += buffer[index].ToString("X2");
      int num1 = (int) buffer[(int) buffer[0] % 8];
      byte num2 = 35;
      for (int index = 0; index < bytes.Length; ++index)
      {
        num2 = (byte) ((uint) bytes[index] ^ (uint) buffer[(index + num1) % 8] ^ (uint) num2);
        str += num2.ToString("X2");
      }
      return str;
    }

    public byte[] StringToBytes(string v)
    {
      byte[] numArray = new byte[v.Length / 2];
      int num1 = 1;
      int index1 = 0;
      while (num1 < v.Length)
      {
        numArray[index1] = byte.Parse(v.Substring(num1 - 1, 2), NumberStyles.AllowHexSpecifier);
        num1 += 2;
        ++index1;
      }
      byte[] bytes = new byte[numArray.Length - 8];
      int num2 = (int) numArray[(int) numArray[0] % 8];
      for (int index2 = numArray.Length - 1; index2 >= 8; --index2)
      {
        byte num3 = index2 > 8 ? numArray[index2 - 1] : (byte) 35;
        bytes[index2 - 8] = (byte) ((uint) numArray[index2] ^ (uint) numArray[(index2 + num2) % 8] ^ (uint) num3);
      }
      return bytes;
    }
  }
}
