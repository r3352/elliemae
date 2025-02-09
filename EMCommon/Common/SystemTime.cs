// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.SystemTime
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public struct SystemTime
  {
    public short Year;
    public short Month;
    public short DayOfWeek;
    public short Day;
    public short Hour;
    public short Minute;
    public short Second;
    public short Millisecond;

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      SystemTime systemTime = (SystemTime) obj;
      return (int) this.Year == (int) systemTime.Year && (int) this.Month == (int) systemTime.Month && (int) this.DayOfWeek == (int) systemTime.DayOfWeek && (int) this.Day == (int) systemTime.Day && (int) this.Hour == (int) systemTime.Hour && (int) this.Minute == (int) systemTime.Minute && (int) this.Second == (int) systemTime.Second && (int) this.Millisecond == (int) systemTime.Millisecond;
    }

    public override int GetHashCode() => this.ToDateTime().GetHashCode();

    public static bool operator ==(SystemTime st1, SystemTime st2) => st1.Equals((object) st2);

    public static bool operator !=(SystemTime st1, SystemTime st2) => !st1.Equals((object) st2);

    [DllImport("kernel32.dll")]
    internal static extern void GetLocalTime(out SystemTime localTime);

    [DllImport("kernel32.dll")]
    internal static extern void GetSystemTime(out SystemTime localTime);

    public DateTime ToDateTime() => SystemTime.ToDateTime(this);

    public static SystemTime UtcNow
    {
      get
      {
        SystemTime localTime;
        SystemTime.GetSystemTime(out localTime);
        return localTime;
      }
    }

    public static SystemTime Now
    {
      get
      {
        SystemTime localTime;
        SystemTime.GetLocalTime(out localTime);
        return localTime;
      }
    }

    public static DateTime ToDateTime(SystemTime time)
    {
      return new DateTime((int) time.Year, (int) time.Month, (int) time.Day, (int) time.Hour, (int) time.Minute, (int) time.Second, (int) time.Millisecond);
    }

    public static SystemTime FromDateTime(DateTime time)
    {
      SystemTime systemTime;
      systemTime.Day = (short) time.Day;
      systemTime.DayOfWeek = (short) time.DayOfWeek;
      systemTime.Hour = (short) time.Hour;
      systemTime.Millisecond = (short) time.Millisecond;
      systemTime.Minute = (short) time.Minute;
      systemTime.Month = (short) time.Month;
      systemTime.Second = (short) time.Second;
      systemTime.Year = (short) time.Year;
      return systemTime;
    }
  }
}
