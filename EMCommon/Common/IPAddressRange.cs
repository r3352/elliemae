// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.IPAddressRange
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class IPAddressRange
  {
    private IPAddress addr;
    private IPAddress subnet;

    public IPAddressRange(IPAddress addr, IPAddress subnet)
    {
      this.addr = addr;
      this.subnet = subnet;
    }

    public IPAddressRange(string addr, string subnet)
    {
      this.addr = IPAddress.Parse(addr);
      this.subnet = IPAddress.Parse(subnet);
    }

    public IPAddress Address => this.addr;

    public IPAddress SubnetMask => this.subnet;

    public bool IsInRange(IPAddress testAddr)
    {
      byte[] addressBytes1 = this.addr.GetAddressBytes();
      byte[] addressBytes2 = this.subnet.GetAddressBytes();
      byte[] addressBytes3 = testAddr.GetAddressBytes();
      for (int index = 0; index < 4; ++index)
      {
        if ((((int) addressBytes1[index] ^ (int) addressBytes3[index]) & (int) addressBytes2[index]) != 0)
          return false;
      }
      return true;
    }

    public bool IsInRange(string testAddr) => this.IsInRange(IPAddress.Parse(testAddr));

    public override string ToString() => this.addr.ToString() + "/" + this.subnet.ToString();

    public static IPAddressRange Parse(string value)
    {
      string[] strArray = value.Split('/');
      if (strArray.Length == 0 || strArray.Length > 2)
        throw new ArgumentException("Invalid IP Address Range format");
      return strArray.Length == 1 ? new IPAddressRange(strArray[0], "255.255.255.255") : new IPAddressRange(strArray[0], strArray[1]);
    }
  }
}
