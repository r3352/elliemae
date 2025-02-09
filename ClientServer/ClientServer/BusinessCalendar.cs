// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.ClientServer.BusinessCalendar
// Assembly: ClientServer, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 301E11EA-0960-40C7-AC1B-26929E024B20
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\ClientServer.dll

using EllieMae.EMLite.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.ClientServer
{
  [Serializable]
  public class BusinessCalendar
  {
    public static readonly DateTime MinimumDate = new DateTime(2000, 1, 1);
    public static readonly DateTime MaximumDate = new DateTime(2029, 12, 31);
    private int calendarId = -1;
    private DaysOfWeek workDays = DaysOfWeek.Weekdays;
    private DateTime startDate;
    private DateTime endDate;
    private Dictionary<DateTime, CalendarDayType> dayTypes = new Dictionary<DateTime, CalendarDayType>();

    public BusinessCalendar(DateTime startDate, DateTime endDate)
    {
      if (startDate == DateTime.MinValue)
        startDate = BusinessCalendar.MinimumDate;
      else if (startDate < BusinessCalendar.MinimumDate)
        throw new ArgumentException("The minimum calendar date is " + (object) BusinessCalendar.MinimumDate);
      if (endDate == DateTime.MaxValue)
        endDate = BusinessCalendar.MaximumDate;
      else if (endDate < BusinessCalendar.MaximumDate)
        throw new ArgumentException("The maximum calendar date is " + (object) BusinessCalendar.MaximumDate);
      this.startDate = startDate.Date;
      this.endDate = endDate.Date;
    }

    public BusinessCalendar()
      : this(BusinessCalendar.MinimumDate, BusinessCalendar.MaximumDate)
    {
    }

    public BusinessCalendar(
      int calendarId,
      DaysOfWeek workDays,
      DateTime startDate,
      DateTime endDate)
    {
      this.calendarId = calendarId;
      this.workDays = workDays;
      DateTime dateTime;
      DateTime date1;
      if (!(startDate.Date > BusinessCalendar.MinimumDate.Date))
      {
        dateTime = BusinessCalendar.MinimumDate;
        date1 = dateTime.Date;
      }
      else
        date1 = startDate.Date;
      this.startDate = date1;
      DateTime date2 = endDate.Date;
      dateTime = BusinessCalendar.MaximumDate;
      DateTime date3 = dateTime.Date;
      DateTime date4;
      if (!(date2 < date3))
      {
        dateTime = BusinessCalendar.MaximumDate;
        date4 = dateTime.Date;
      }
      else
        date4 = endDate.Date;
      this.endDate = date4;
    }

    public int CalendarID => this.calendarId;

    public CalendarType CalendarType
    {
      get
      {
        switch (this.calendarId)
        {
          case 0:
            return CalendarType.Postal;
          case 1:
            return CalendarType.Business;
          case 2:
            return CalendarType.Custom;
          case 4:
            return CalendarType.RegZ;
          default:
            return CalendarType.Business;
        }
      }
    }

    public DateTime StartDate => this.startDate;

    public DateTime EndDate => this.endDate;

    public DaysOfWeek WorkDays
    {
      get => this.workDays;
      set => this.workDays = value;
    }

    public bool IsWeekendDay(DateTime date)
    {
      this.verifyDateInRange(date);
      return (DaysOfWeekConverter.ToDaysOfWeek(date.DayOfWeek) & this.workDays) == DaysOfWeek.None;
    }

    public bool IsBusinessDay(DateTime date)
    {
      this.verifyDateInRange(date);
      return this.GetDayType(date) == CalendarDayType.BusinessDay;
    }

    public CalendarDayType GetDayType(DateTime date)
    {
      this.verifyDateInRange(date);
      if (this.dayTypes.ContainsKey(date.Date))
        return this.dayTypes[date.Date];
      return this.IsWeekendDay(date) ? CalendarDayType.Weekend : CalendarDayType.BusinessDay;
    }

    public void SetDayType(DateTime date, CalendarDayType dayType)
    {
      this.verifyDateInRange(date);
      if (dayType == CalendarDayType.Weekend)
        throw new ArgumentException("Weekend days must be specified using the WorkDays property.");
      if (dayType == CalendarDayType.BusinessDay)
        this.dayTypes.Remove(date.Date);
      else
        this.dayTypes[date.Date] = dayType;
    }

    public DateTime[] GetNonRecurringNonWorkDays()
    {
      DateTime[] array = new DateTime[this.dayTypes.Count];
      this.dayTypes.Keys.CopyTo(array, 0);
      return array;
    }

    public DateTime AddBusinessDays(
      DateTime date,
      int count,
      bool startFromClosestBusinessDay,
      bool optInForSundayHoliday = false,
      bool optInForFederalHoliday = false,
      bool forECD = false)
    {
      this.verifyDateInRange(date);
      if (startFromClosestBusinessDay)
        date = this.GetNextClosestBusinessDay(date);
      int num1 = 0;
      int num2 = Math.Abs(count);
      while (num1 < num2)
      {
        date = date.AddDays(count > 0 ? 1.0 : -1.0);
        if (forECD & optInForSundayHoliday && num1 == num2 - 1 && date.DayOfWeek == DayOfWeek.Sunday || forECD & optInForFederalHoliday && num1 == num2 - 1 && this.GetDayType(date) == CalendarDayType.Holiday || this.GetDayType(date) == CalendarDayType.BusinessDay)
          ++num1;
      }
      return date;
    }

    public DateTime GetNextClosestBusinessDay(DateTime date)
    {
      while (this.GetDayType(date) != CalendarDayType.BusinessDay)
        date = date.AddDays(1.0);
      return date;
    }

    public DateTime GetNextClosestNotHolidayDay(DateTime date)
    {
      while (this.GetDayType(date) == CalendarDayType.Holiday)
        date = date.AddDays(1.0);
      return date;
    }

    public DateTime GetPreviousClosestBusinessDay(DateTime date)
    {
      while (this.GetDayType(date) != CalendarDayType.BusinessDay)
        date = date.AddDays(-1.0);
      return date;
    }

    public DateTime GetPreviousClosestNotHolidayDay(DateTime date)
    {
      while (this.GetDayType(date) == CalendarDayType.Holiday)
        date = date.AddDays(-1.0);
      return date;
    }

    public DateTime GetPreviousClosestBusinessDayNoWeekend(DateTime date)
    {
      while (this.GetDayType(date) != CalendarDayType.BusinessDay || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        date = date.AddDays(-1.0);
      return date;
    }

    public bool Contains(DateTime date) => date.Date >= this.startDate && date.Date <= this.endDate;

    private void verifyDateInRange(DateTime date)
    {
      if (!this.Contains(date))
        throw new ArgumentOutOfRangeException(nameof (date), "The specified date is outside the range of the current calendar.");
    }

    public void Merge(BusinessCalendar cal)
    {
      if (cal.StartDate > this.EndDate.AddDays(1.0) || cal.EndDate < this.StartDate.AddDays(-1.0))
        throw new ArgumentException("The specified calendars are not contiguous");
      if (cal.StartDate < this.startDate)
        this.startDate = cal.StartDate;
      if (cal.EndDate > this.endDate)
        this.endDate = cal.EndDate;
      foreach (DateTime key in cal.dayTypes.Keys)
      {
        if (!this.dayTypes.ContainsKey(key))
          this.dayTypes[key] = cal.dayTypes[key];
      }
    }
  }
}
