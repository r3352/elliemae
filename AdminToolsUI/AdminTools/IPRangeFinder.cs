// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.AdminTools.IPRangeFinder
// Assembly: AdminToolsUI, Version=24.3.0.5, Culture=neutral, PublicKeyToken=null
// MVID: BCE9F231-878C-4206-826C-76CFCB8C9167
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\AdminToolsUI.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.AdminTools
{
  internal class IPRangeFinder
  {
    public IEnumerable<string> GetIPRange(IPAddress startIP, IPAddress endIP)
    {
      uint sIP = this.byteToUint(startIP.GetAddressBytes());
      for (uint eIP = this.byteToUint(endIP.GetAddressBytes()); sIP <= eIP; ++sIP)
        yield return new IPAddress((long) this.reverseBytesArray(sIP)).ToString();
    }

    protected uint reverseBytesArray(uint ip)
    {
      return (uint) BitConverter.ToInt32(((IEnumerable<byte>) BitConverter.GetBytes(ip)).Reverse<byte>().ToArray<byte>(), 0);
    }

    protected uint byteToUint(byte[] ipBytes)
    {
      ByteConverter byteConverter = new ByteConverter();
      uint num1 = 0;
      int num2 = 24;
      foreach (byte ipByte in ipBytes)
      {
        if (num1 == 0U)
        {
          num1 = (uint) byteConverter.ConvertTo((object) ipByte, typeof (uint)) << num2;
          num2 -= 8;
        }
        else
        {
          if (num2 >= 8)
            num1 += (uint) byteConverter.ConvertTo((object) ipByte, typeof (uint)) << num2;
          else
            num1 += (uint) byteConverter.ConvertTo((object) ipByte, typeof (uint));
          num2 -= 8;
        }
      }
      return num1;
    }
  }
}
