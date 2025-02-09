// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.IPRange
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Diagnostics;
using System.Net;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class IPRange
  {
    private const string className = "IPRange�";
    private static readonly string sw = Tracing.SwOutsideLoan;
    public readonly int OID;
    public readonly string Userid;
    private readonly Version startIP;
    private readonly Version endIP;

    public string StartIP
    {
      get => this.startIP == (Version) null ? (string) null : this.startIP.ToString();
    }

    public string EndIP => this.endIP == (Version) null ? (string) null : this.endIP.ToString();

    public IPRange(int oid, string userid, IPAddress ipAddress)
      : this(oid, userid, ipAddress, (IPAddress) null)
    {
    }

    public IPRange(int oid, string userid, IPAddress startIP, IPAddress endIP)
      : this(oid, userid, startIP == null ? (string) null : startIP.ToString(), endIP == null ? (string) null : endIP.ToString())
    {
    }

    public IPRange(int oid, string userid, string ipAddress)
      : this(oid, userid, ipAddress, (string) null)
    {
    }

    public IPRange(int oid, string userid, string startIP, string endIP)
    {
      if (startIP == null)
        throw new Exception("Null start IP address");
      this.validIP(startIP);
      this.validIP(endIP);
      this.OID = oid;
      this.Userid = userid;
      this.startIP = new Version(startIP);
      this.endIP = endIP == null ? new Version(startIP) : new Version(endIP);
      if (!(this.startIP > this.endIP))
        return;
      Version startIp = this.startIP;
      this.startIP = this.endIP;
      this.endIP = startIp;
      Tracing.Log(IPRange.sw, nameof (IPRange), TraceLevel.Warning, "Start IP is larger than end IP; IPs switched.");
    }

    private void validIP(string ipString)
    {
      if (ipString == null)
        return;
      bool flag = false;
      string[] strArray = ipString.Split(new char[1]{ '.' }, StringSplitOptions.RemoveEmptyEntries);
      if (strArray.Length != 4)
      {
        flag = true;
      }
      else
      {
        foreach (string str in strArray)
        {
          try
          {
            int int32 = Convert.ToInt32(str);
            if (int32 >= 0)
            {
              if (int32 <= (int) byte.MaxValue)
                continue;
            }
            flag = true;
          }
          catch
          {
            flag = true;
          }
        }
      }
      if (flag)
        throw new Exception("Invalid IP address: " + ipString);
    }

    public bool ForEveryone() => (this.Userid ?? "").Trim() == "" || this.Userid == "*";

    private bool checkUser(string userid)
    {
      return this.ForEveryone() || !(this.Userid.ToLower() != userid.ToLower());
    }

    public static bool ExistsUserConfig(string userID, IPRange[] ranges)
    {
      foreach (IPRange range in ranges)
      {
        if (range.checkUser(userID))
          return true;
      }
      return false;
    }

    public bool IsInRange(string userid, IPAddress ipAddress)
    {
      return ipAddress != null && this.IsInRange(userid, ipAddress.ToString());
    }

    public bool IsInRange(string userid, string ipAddress)
    {
      if (ipAddress == null || !this.checkUser(userid))
        return false;
      Version version = new Version(ipAddress);
      return version >= this.startIP && version <= this.endIP;
    }

    public static bool IsInRanges(string userid, IPAddress ipAddress, IPRange[] ranges)
    {
      return IPRange.IsInRanges(userid, ipAddress == null ? (string) null : ipAddress.ToString(), ranges);
    }

    public static bool IsInRanges(string userid, string ipAddress, IPRange[] ranges)
    {
      if (ranges == null || ranges.Length == 0)
        return true;
      if (ipAddress == null)
        return false;
      foreach (IPRange range in ranges)
      {
        if (range.IsInRange(userid, ipAddress))
          return true;
      }
      return false;
    }

    public bool IsOverlapping(IPRange ipRange)
    {
      return this.checkUser(ipRange.Userid) && (ipRange.startIP >= this.startIP && ipRange.startIP <= this.endIP || ipRange.endIP >= this.startIP && ipRange.endIP <= this.endIP);
    }
  }
}
