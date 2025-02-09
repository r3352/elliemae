// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.DaysOfWeekConverter
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common
{
  public static class DaysOfWeekConverter
  {
    public static DayOfWeek ToDayOfWeek(DaysOfWeek day)
    {
      switch (day)
      {
        case DaysOfWeek.Sunday:
          return DayOfWeek.Sunday;
        case DaysOfWeek.Monday:
          return DayOfWeek.Monday;
        case DaysOfWeek.Tuesday:
          return DayOfWeek.Tuesday;
        case DaysOfWeek.Wednesday:
          return DayOfWeek.Wednesday;
        case DaysOfWeek.Thursday:
          return DayOfWeek.Thursday;
        case DaysOfWeek.Friday:
          return DayOfWeek.Friday;
        case DaysOfWeek.Saturday:
          return DayOfWeek.Saturday;
        default:
          throw new ArgumentException("Specified day '" + (object) day + "' cannot be converted to DayOfWeek");
      }
    }

    public static DaysOfWeek ToDaysOfWeek(DayOfWeek day)
    {
      switch (day)
      {
        case DayOfWeek.Sunday:
          return DaysOfWeek.Sunday;
        case DayOfWeek.Monday:
          return DaysOfWeek.Monday;
        case DayOfWeek.Tuesday:
          return DaysOfWeek.Tuesday;
        case DayOfWeek.Wednesday:
          return DaysOfWeek.Wednesday;
        case DayOfWeek.Thursday:
          return DaysOfWeek.Thursday;
        case DayOfWeek.Friday:
          return DaysOfWeek.Friday;
        case DayOfWeek.Saturday:
          return DaysOfWeek.Saturday;
        default:
          throw new ArgumentException("Specified day '" + (object) day + "' cannot be converted to DaysOfWeek");
      }
    }
  }
}
