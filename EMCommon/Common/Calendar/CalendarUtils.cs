// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.CalendarUtils
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  public static class CalendarUtils
  {
    public static int GetDayOfWeekIndex(DayOfWeek dayOfWeek)
    {
      switch (dayOfWeek)
      {
        case DayOfWeek.Sunday:
          return 0;
        case DayOfWeek.Monday:
          return 1;
        case DayOfWeek.Tuesday:
          return 2;
        case DayOfWeek.Wednesday:
          return 3;
        case DayOfWeek.Thursday:
          return 4;
        case DayOfWeek.Friday:
          return 5;
        case DayOfWeek.Saturday:
          return 6;
        default:
          throw new ArgumentException("Invalid day of week specified: " + (object) dayOfWeek);
      }
    }

    public static DateTime GetFirstDayInMonthOfRecurrence(
      DateTime monthYear,
      RecurrenceWeek recurrenceWeek,
      RecurrenceDay recurrenceDay,
      DaysOfWeek daysOfWeek)
    {
      DateTime dateTime = new DateTime(monthYear.Year, monthYear.Month, 1);
      int num1 = DateTime.DaysInMonth(monthYear.Year, monthYear.Month);
      DateTime monthOfRecurrence = DateTime.MinValue;
      int num2 = 0;
      for (int index = 0; index < num1; ++index)
      {
        if (CalendarUtils.IsDayOfRecurrence(dateTime.AddDays((double) index), recurrenceDay, daysOfWeek))
        {
          monthOfRecurrence = dateTime.AddDays((double) index);
          ++num2;
          if (recurrenceWeek == RecurrenceWeek.First && num2 == 1 || recurrenceWeek == RecurrenceWeek.Second && num2 == 2 || recurrenceWeek == RecurrenceWeek.Third && num2 == 3 || recurrenceWeek == RecurrenceWeek.Fourth && num2 == 4)
            return monthOfRecurrence;
        }
      }
      if (recurrenceWeek == RecurrenceWeek.Last)
        return monthOfRecurrence;
      throw new ArgumentException("One or more parameters specified an invalid recurrence");
    }

    public static bool IsDayOfRecurrence(
      DateTime date,
      RecurrenceDay recurrenceDay,
      DaysOfWeek daysOfWeek)
    {
      switch (recurrenceDay)
      {
        case RecurrenceDay.Any:
          return true;
        case RecurrenceDay.Weekday:
          return (DaysOfWeekConverter.ToDaysOfWeek(date.DayOfWeek) & DaysOfWeek.Weekdays) != 0;
        case RecurrenceDay.WeekendDay:
          return (DaysOfWeekConverter.ToDaysOfWeek(date.DayOfWeek) & DaysOfWeek.Weekends) != 0;
        case RecurrenceDay.DayOfWeek:
          return (DaysOfWeekConverter.ToDaysOfWeek(date.DayOfWeek) & daysOfWeek) != 0;
        default:
          return false;
      }
    }
  }
}
