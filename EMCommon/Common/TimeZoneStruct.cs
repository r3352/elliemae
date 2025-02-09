// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TimeZoneStruct
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct TimeZoneStruct
  {
    public int Bias;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string StandardName;
    public SystemTime StandardDate;
    public int StandardBias;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string DaylightName;
    public SystemTime DaylightDate;
    public int DaylightBias;

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      TimeZoneStruct timeZoneStruct = (TimeZoneStruct) obj;
      return timeZoneStruct.Bias == this.Bias && timeZoneStruct.DaylightBias == this.DaylightBias && timeZoneStruct.DaylightDate == this.DaylightDate && timeZoneStruct.StandardBias == this.StandardBias && timeZoneStruct.StandardDate == this.StandardDate;
    }

    public override int GetHashCode() => this.Bias;
  }
}
