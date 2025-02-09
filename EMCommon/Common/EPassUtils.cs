// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.EPassUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class EPassUtils
  {
    private static readonly TimeZoneInfo tzInfo = TimeZoneInfo.GetTimeZone("Pacific Standard Time");

    static EPassUtils()
    {
      if (!(EPassUtils.tzInfo == (TimeZoneInfo) null))
        return;
      EPassUtils.tzInfo = TimeZoneInfo.CurrentTimeZone;
    }

    public static TimeZoneInfo TimeZoneInfo => EPassUtils.tzInfo;

    public static DateTime EPassTimeToLocalTime(DateTime date)
    {
      return TimeZoneInfo.Convert(EPassUtils.tzInfo, TimeZoneInfo.CurrentTimeZone, date);
    }
  }
}
