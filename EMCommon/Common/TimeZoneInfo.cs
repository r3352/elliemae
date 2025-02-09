// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TimeZoneInfo
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using Microsoft.Win32;
using System;
using System.Globalization;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public class TimeZoneInfo : IComparable
  {
    private static TimeZoneInfo[] timeZones;
    private string tziName;
    private TimeZoneStruct tziStruct;

    private TimeZoneInfo()
    {
      this.tziName = "";
      this.tziStruct = new TimeZoneStruct();
    }

    public string Name
    {
      get => this.tziName;
      set => this.tziName = value;
    }

    public TimeZoneStruct TimeZoneStruct
    {
      get => this.tziStruct;
      set => this.tziStruct = value;
    }

    public override string ToString() => this.tziName;

    public override bool Equals(object obj)
    {
      if (obj == null || this.GetType() != obj.GetType())
        return false;
      TimeZoneInfo timeZoneInfo = (TimeZoneInfo) obj;
      return timeZoneInfo.tziName == this.tziName && timeZoneInfo.tziStruct.Bias == this.tziStruct.Bias && timeZoneInfo.tziStruct.DaylightBias == this.tziStruct.DaylightBias && timeZoneInfo.tziStruct.DaylightDate == this.tziStruct.DaylightDate && timeZoneInfo.tziStruct.StandardBias == this.tziStruct.StandardBias && timeZoneInfo.tziStruct.StandardDate == this.tziStruct.StandardDate;
    }

    public override int GetHashCode() => this.tziStruct.Bias;

    public static bool operator ==(TimeZoneInfo tzi1, TimeZoneInfo tzi2)
    {
      return tzi1.Equals((object) tzi2);
    }

    public static bool operator !=(TimeZoneInfo tzi1, TimeZoneInfo tzi2)
    {
      return !tzi1.Equals((object) tzi2);
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      if ((object) (obj as TimeZoneInfo) == null)
        throw new ArgumentException();
      return ((TimeZoneInfo) obj).tziStruct.Bias - this.tziStruct.Bias;
    }

    [DllImport("kernel32.dll")]
    private static extern TimeZoneID GetTimeZoneInformation(out TimeZoneStruct tzi);

    [DllImport("kernel32.dll")]
    internal static extern bool SystemTimeToTzSpecificLocalTime(
      ref TimeZoneStruct tzOfInterest,
      ref SystemTime Utc,
      out SystemTime timeOfInterest);

    [DllImport("kernel32.dll")]
    internal static extern bool TzSpecificLocalTimeToSystemTime(
      ref TimeZoneStruct sourceTimeZone,
      ref SystemTime sourceTimeLocal,
      out SystemTime Utc);

    public static TimeZoneInfo CurrentTimeZone
    {
      get
      {
        TimeZoneInfo timeZoneInfo = new TimeZoneInfo();
        return TimeZoneInfo.GetTimeZoneInformation(out timeZoneInfo.tziStruct) != TimeZoneID.Invalid ? timeZoneInfo : throw new SystemException("Error occurred.");
      }
    }

    public static TimeZoneInfo[] AllTimeZones => TimeZoneInfo.timeZones;

    public static TimeZoneInfo GetTimeZone(string name)
    {
      foreach (TimeZoneInfo timeZone in TimeZoneInfo.timeZones)
      {
        if (string.Compare(timeZone.Name, name, true) == 0 || string.Compare(timeZone.TimeZoneStruct.StandardName, name, true) == 0 || string.Compare(timeZone.TimeZoneStruct.DaylightName, name, true) == 0)
          return timeZone;
      }
      return (TimeZoneInfo) null;
    }

    static TimeZoneInfo()
    {
      RegistryKey registryKey1;
      if (Environment.OSVersion.Platform == PlatformID.Win32NT)
      {
        registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Time Zones\\");
      }
      else
      {
        if (Environment.OSVersion.Platform != PlatformID.Win32Windows)
          throw new PlatformNotSupportedException();
        registryKey1 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Time Zones\\");
      }
      string[] subKeyNames = registryKey1.GetSubKeyNames();
      TimeZoneInfo[] timeZoneInfoArray = new TimeZoneInfo[subKeyNames.Length];
      Type type = typeof (TZI);
      int num = Marshal.SizeOf(type);
      for (int index = 0; index < subKeyNames.Length; ++index)
      {
        RegistryKey registryKey2 = registryKey1.OpenSubKey(subKeyNames[index]);
        string str1 = registryKey2.GetValue("Display").ToString();
        string str2 = registryKey2.GetValue("Std").ToString();
        string str3 = registryKey2.GetValue("Dlt").ToString();
        byte[] numArray = registryKey2.GetValue("TZI") as byte[];
        registryKey2.Close();
        if (numArray != null && numArray.Length >= num)
        {
          GCHandle gcHandle = GCHandle.Alloc((object) numArray, GCHandleType.Pinned);
          TZI structure = (TZI) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), type);
          gcHandle.Free();
          timeZoneInfoArray.SetValue((object) new TimeZoneInfo()
          {
            tziName = str1,
            tziStruct = {
              Bias = structure.Bias,
              DaylightBias = structure.DaylightBias,
              DaylightDate = structure.DaylightDate,
              StandardBias = structure.StandardBias,
              StandardDate = structure.StandardDate,
              DaylightName = str3,
              StandardName = str2
            }
          }, index);
        }
      }
      TimeZoneInfo.timeZones = timeZoneInfoArray;
    }

    public static DateTime Convert(
      TimeZoneInfo source,
      TimeZoneInfo destination,
      DateTime sourceLocalTime)
    {
      return sourceLocalTime == DateTime.MinValue || sourceLocalTime == DateTime.MaxValue ? sourceLocalTime : TimeZoneInfo.ToLocalTime(destination, TimeZoneInfo.ToUniversalTime(source, sourceLocalTime));
    }

    public static DateTime ToLocalTime(TimeZoneInfo destination, DateTime utcTime)
    {
      if (utcTime == DateTime.MinValue || utcTime == DateTime.MaxValue)
        return utcTime;
      if (Environment.OSVersion.Platform == PlatformID.Win32NT)
      {
        SystemTime Utc = SystemTime.FromDateTime(utcTime);
        SystemTime timeOfInterest;
        if (!TimeZoneInfo.SystemTimeToTzSpecificLocalTime(ref destination.tziStruct, ref Utc, out timeOfInterest))
          throw new SystemException("Error occurred.");
        return timeOfInterest.ToDateTime();
      }
      utcTime = utcTime.AddMinutes((double) -destination.tziStruct.Bias);
      return destination.IsDaylightSavingTime(utcTime) ? utcTime.AddMinutes((double) -destination.tziStruct.DaylightBias) : utcTime.AddMinutes((double) -destination.tziStruct.StandardBias);
    }

    public static DateTime ToUniversalTime(TimeZoneInfo source, DateTime sourceLocalTime)
    {
      if (Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5 && Environment.OSVersion.Version.Minor >= 1)
      {
        SystemTime sourceTimeLocal = SystemTime.FromDateTime(sourceLocalTime);
        SystemTime Utc;
        if (!TimeZoneInfo.TzSpecificLocalTimeToSystemTime(ref source.tziStruct, ref sourceTimeLocal, out Utc))
          throw new SystemException("Error occurred.");
        return Utc.ToDateTime();
      }
      return source.IsDaylightSavingTime(sourceLocalTime) ? sourceLocalTime.AddMinutes((double) (source.tziStruct.Bias + source.tziStruct.DaylightBias)) : sourceLocalTime.AddMinutes((double) (source.tziStruct.Bias + source.tziStruct.StandardBias));
    }

    private static DateTime getChangeDate(int year, SystemTime changeTime)
    {
      if (changeTime.Month == (short) 0)
        return DateTime.MinValue;
      DateTime changeDate;
      if (changeTime.Day < (short) 5)
      {
        changeDate = new DateTime(year, (int) changeTime.Month, 1, (int) changeTime.Hour, 0, 0);
        int num = (int) (short) changeDate.DayOfWeek > (int) changeTime.DayOfWeek ? (int) (7 - (changeDate.DayOfWeek - (int) changeTime.DayOfWeek)) : (int) ((int) changeTime.DayOfWeek - changeDate.DayOfWeek);
        changeDate = changeDate.AddDays((double) (num + 7 * ((int) changeTime.Day - 1)));
      }
      else
      {
        changeDate = new DateTime(year, (int) changeTime.Month + 1, 1, (int) changeTime.Hour, 0, 0);
        changeDate = (int) (short) changeDate.DayOfWeek <= (int) changeTime.DayOfWeek ? changeDate.AddDays((double) -(int) (7 - ((int) changeTime.DayOfWeek - changeDate.DayOfWeek))) : changeDate.AddDays((double) -(int) (changeDate.DayOfWeek - (int) changeTime.DayOfWeek));
      }
      return changeDate;
    }

    public DateTime Convert(TimeZoneInfo destination, DateTime localTime)
    {
      return TimeZoneInfo.Convert(this, destination, localTime);
    }

    public DateTime ToUniversalTime(DateTime sourceLocalTime)
    {
      return TimeZoneInfo.ToUniversalTime(this, sourceLocalTime);
    }

    public DateTime ToLocalTime(DateTime utcTime) => TimeZoneInfo.ToLocalTime(this, utcTime);

    public bool ObservesDaylightTime => this.tziStruct.DaylightDate.Month != (short) 0;

    public string GetTimeZoneName(DateTime time)
    {
      return this.IsDaylightSavingTime(time) ? this.tziStruct.DaylightName : this.tziStruct.StandardName;
    }

    public bool IsDaylightSavingTime(DateTime time)
    {
      DaylightTime daylightChanges = this.GetDaylightChanges(time.Year);
      return daylightChanges != null && TimeZoneInfo.IsDaylightSavingTime(time, daylightChanges);
    }

    public static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTime)
    {
      return daylightTime != null ? TimeZone.IsDaylightSavingTime(time, daylightTime) : throw new ArgumentNullException("daylightTime cannot be null");
    }

    public DateTime GetStandardDateTime(int year)
    {
      return TimeZoneInfo.getChangeDate(year, this.tziStruct.StandardDate);
    }

    public DateTime GetDaylightDateTime(int year)
    {
      return TimeZoneInfo.getChangeDate(year, this.tziStruct.DaylightDate);
    }

    public DaylightTime GetDaylightChanges(int year)
    {
      return !this.ObservesDaylightTime ? (DaylightTime) null : new DaylightTime(this.GetDaylightDateTime(year), this.GetStandardDateTime(year), TimeSpan.FromMinutes((double) -this.tziStruct.DaylightBias));
    }
  }
}
