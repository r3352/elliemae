// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.TimeZones.DateTimeWithZone
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.TimeZones
{
  [CLSCompliant(true)]
  public struct DateTimeWithZone
  {
    public DateTimeWithZone(DateTime srcDateTime, System.TimeZoneInfo destTimeZone)
    {
      if (srcDateTime == srcDateTime.Date)
      {
        this.TimeZone = destTimeZone;
        this.DateTime = DateTime.SpecifyKind(srcDateTime, DateTimeKind.Unspecified);
      }
      else
      {
        if (srcDateTime.Kind == DateTimeKind.Unspecified)
          throw new ArgumentException("srcDateTime.Kind cannot be unspecified.");
        if (srcDateTime.Kind == DateTimeKind.Local)
          srcDateTime = srcDateTime.ToUniversalTime();
        this.TimeZone = destTimeZone;
        this.DateTime = System.TimeZoneInfo.ConvertTimeFromUtc(srcDateTime, destTimeZone);
      }
    }

    public DateTimeWithZone(
      DateTime srcDateTime,
      System.TimeZoneInfo srcTimeZone,
      System.TimeZoneInfo destTimeZone)
    {
      if (srcDateTime == srcDateTime.Date)
      {
        this.TimeZone = destTimeZone;
        this.DateTime = DateTime.SpecifyKind(srcDateTime, DateTimeKind.Unspecified);
      }
      else
      {
        if (srcDateTime.Kind != DateTimeKind.Unspecified)
          throw new ArgumentException("srcDateTime.Kind should be unspecified.");
        this.TimeZone = destTimeZone;
        if (srcTimeZone.Equals(destTimeZone))
        {
          this.DateTime = srcDateTime;
        }
        else
        {
          srcDateTime = Utils.ConvertTimeToUtc(srcDateTime, srcTimeZone);
          this.DateTime = System.TimeZoneInfo.ConvertTimeFromUtc(srcDateTime, destTimeZone);
        }
      }
    }

    public DateTime DateTime { get; }

    public System.TimeZoneInfo TimeZone { get; }

    public static DateTimeWithZone ConvertToTimeZone(DateTime srcDate, System.TimeZoneInfo timeZoneInfo)
    {
      return new DateTimeWithZone(srcDate, timeZoneInfo);
    }

    public static DateTimeWithZone ConvertToTimeZone(
      DateTime srcDate,
      System.TimeZoneInfo srcTimeZoneInfo,
      System.TimeZoneInfo destTimeZoneInfo)
    {
      return new DateTimeWithZone(srcDate, srcTimeZoneInfo, destTimeZoneInfo);
    }

    public static DateTimeWithZone Create(DateTime srcDate, System.TimeZoneInfo timeZoneInfo)
    {
      return new DateTimeWithZone(srcDate, timeZoneInfo, timeZoneInfo);
    }
  }
}
