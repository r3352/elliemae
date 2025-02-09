// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.RecurringAppointment
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  [Serializable]
  public class RecurringAppointment(DateTime startTime, TimeSpan duration, string subject) : 
    PersistentAppointment(startTime, duration, subject)
  {
    private DateTime recurrenceEndDate = DateTime.MaxValue;
    private RecurrenceType recurrenceType = RecurrenceType.Daily;
    private DaysOfWeek daysOfWeek = DaysOfWeek.All;
    private int recurrencePeriod = 1;
    private int numberOfOccurrences = -1;
    private RecurrenceWeek recurrenceWeek;
    private RecurrenceDay recurrenceDay = RecurrenceDay.Any;
    private DateTime recurrenceMonthDay = DateTime.MinValue;
    public List<DateTime> exceptionDates = new List<DateTime>();

    public override AppointmentType AppointmentType => AppointmentType.Recurring;

    public RecurrenceType RecurrenceType
    {
      get => this.recurrenceType;
      set
      {
        if (value == RecurrenceType.None)
          throw new ArgumentException("Invalid recurrence type specified");
        if (value == this.recurrenceType)
          return;
        this.recurrenceType = value;
        if (this.recurrenceType != RecurrenceType.Monthly && this.recurrenceType != RecurrenceType.Yearly)
        {
          this.recurrenceWeek = RecurrenceWeek.None;
          this.recurrenceDay = RecurrenceDay.None;
        }
        this.refreshRecurrenceSchedule();
      }
    }

    public DaysOfWeek DaysOfWeek
    {
      get => this.daysOfWeek;
      set
      {
        if (value == this.daysOfWeek)
          return;
        this.daysOfWeek = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public DateTime RecurrenceEndDate
    {
      get => this.recurrenceEndDate;
      set
      {
        if (!(value.Date != this.recurrenceEndDate.Date))
          return;
        this.recurrenceEndDate = value.Date;
        this.numberOfOccurrences = -1;
        this.refreshRecurrenceSchedule();
      }
    }

    public override DateTime EndTime => this.RecurrenceEndDate;

    public int RecurrencePeriod
    {
      get => this.recurrencePeriod;
      set
      {
        if (value == this.recurrencePeriod)
          return;
        this.recurrencePeriod = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public DateTime RecurrenceMonthDay
    {
      get => this.recurrenceMonthDay;
      set
      {
        if (!(value != this.recurrenceMonthDay))
          return;
        this.recurrenceMonthDay = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public int NumberOfOccurrences
    {
      get => this.numberOfOccurrences;
      set
      {
        if (value == this.numberOfOccurrences)
          return;
        this.numberOfOccurrences = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public RecurrenceWeek RecurrenceWeek
    {
      get => this.recurrenceWeek;
      set
      {
        if (this.recurrenceType != RecurrenceType.Monthly && this.recurrenceType != RecurrenceType.Yearly && value != RecurrenceWeek.None)
          throw new Exception("The specified value is invalid for the current Recurrence Type.");
        if (value == this.recurrenceWeek)
          return;
        this.recurrenceWeek = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public RecurrenceDay RecurrenceDay
    {
      get => this.recurrenceDay;
      set
      {
        if (this.recurrenceType != RecurrenceType.Monthly && this.recurrenceType != RecurrenceType.Yearly && value != RecurrenceDay.None)
          throw new Exception("The specified value is invalid for the current Recurrence Type.");
        if (value == this.recurrenceDay)
          return;
        this.recurrenceDay = value;
        this.refreshRecurrenceSchedule();
      }
    }

    public NonrecurringAppointment CreateException(DateTime dateOfException)
    {
      DateTime nextOccurrence = this.GetNextOccurrence(dateOfException.Date);
      if (nextOccurrence.Date != dateOfException.Date)
        throw new ArgumentException("There is no occurrence for the appointment on '" + dateOfException.ToShortDateString() + "'");
      this.exceptionDates.Add(dateOfException.Date);
      return new NonrecurringAppointment(this, nextOccurrence);
    }

    public DateTime GetNextOccurrence(DateTime fromDate)
    {
      if (fromDate < this.StartTime)
        fromDate = this.StartTime;
      DateTime minValue = DateTime.MinValue;
      switch (this.recurrenceType)
      {
        case RecurrenceType.Daily:
          return this.getNextDailyOccurrence(fromDate);
        case RecurrenceType.Weekly:
          return this.getNextWeeklyOccurrence(fromDate);
        case RecurrenceType.Monthly:
          return this.getNextMonthlyOccurrence(fromDate);
        case RecurrenceType.Yearly:
          return this.getNextYearlyOccurrence(fromDate);
        default:
          throw new Exception("The recurrence period for this appointment is invalid.");
      }
    }

    private DateTime getNextDailyOccurrence(DateTime fromDate)
    {
      int days = (fromDate.Date - this.StartTime.Date).Days;
      DateTime dateTime = this.StartTime;
      dateTime = dateTime.Date;
      dateTime = dateTime.AddDays((double) (days / this.recurrencePeriod * this.recurrencePeriod));
      for (DateTime nextDailyOccurrence = dateTime.Add(this.StartTime.TimeOfDay); nextDailyOccurrence.Date <= this.recurrenceEndDate; nextDailyOccurrence = nextDailyOccurrence.AddDays((double) this.recurrencePeriod))
      {
        if (nextDailyOccurrence >= fromDate && (DaysOfWeekConverter.ToDaysOfWeek(nextDailyOccurrence.DayOfWeek) & this.daysOfWeek) != DaysOfWeek.None && !this.exceptionDates.Contains(nextDailyOccurrence.Date))
          return nextDailyOccurrence;
      }
      return DateTime.MinValue;
    }

    private DateTime getNextWeeklyOccurrence(DateTime fromDate)
    {
      int num = this.recurrencePeriod * 7;
      DateTime dateTime1 = this.StartTime;
      dateTime1 = dateTime1.Date;
      DateTime dateTime2 = dateTime1.AddDays((double) (-1 * CalendarUtils.GetDayOfWeekIndex(this.StartTime.DayOfWeek)));
      int days = (fromDate.Date - dateTime2).Days;
      for (dateTime2 = dateTime2.AddDays((double) (days / num * num)); dateTime2 <= this.recurrenceEndDate; dateTime2 = dateTime2.AddDays((double) num))
      {
        for (int index = 0; index < 7; ++index)
        {
          DateTime weeklyOccurrence = dateTime2.AddDays((double) index).Add(this.StartTime.TimeOfDay);
          if (weeklyOccurrence.Date > this.recurrenceEndDate)
            return DateTime.MinValue;
          if (weeklyOccurrence >= fromDate && (DaysOfWeekConverter.ToDaysOfWeek(weeklyOccurrence.DayOfWeek) & this.daysOfWeek) != DaysOfWeek.None && !this.exceptionDates.Contains(weeklyOccurrence.Date))
            return weeklyOccurrence;
        }
      }
      return DateTime.MinValue;
    }

    private DateTime getNextMonthlyOccurrence(DateTime fromDate)
    {
      int months = ((fromDate.Year - this.StartTime.Year) * 12 + fromDate.Month - this.StartTime.Month) / this.recurrencePeriod * this.recurrencePeriod;
      DateTime dateTime1 = this.StartTime;
      int year = dateTime1.Year;
      dateTime1 = this.StartTime;
      int month = dateTime1.Month;
      dateTime1 = new DateTime(year, month, 1);
      for (DateTime monthYear = dateTime1.AddMonths(months); monthYear <= this.recurrenceEndDate; monthYear = monthYear.AddMonths(this.recurrencePeriod))
      {
        DateTime monthlyOccurrence = monthYear;
        DateTime dateTime2;
        if (this.recurrenceWeek == RecurrenceWeek.None)
        {
          ref DateTime local = ref monthlyOccurrence;
          dateTime2 = this.RecurrenceMonthDay;
          double num = (double) (dateTime2.Day - 1);
          monthlyOccurrence = local.AddDays(num);
        }
        else
          monthlyOccurrence = CalendarUtils.GetFirstDayInMonthOfRecurrence(monthYear, this.recurrenceWeek, this.recurrenceDay, this.daysOfWeek);
        if (monthlyOccurrence > this.recurrenceEndDate)
          return DateTime.MinValue;
        ref DateTime local1 = ref monthlyOccurrence;
        dateTime2 = this.StartTime;
        TimeSpan timeOfDay = dateTime2.TimeOfDay;
        monthlyOccurrence = local1.Add(timeOfDay);
        if (monthlyOccurrence >= fromDate && !this.exceptionDates.Contains(monthlyOccurrence.Date))
          return monthlyOccurrence;
      }
      return DateTime.MinValue;
    }

    private DateTime getNextYearlyOccurrence(DateTime fromDate)
    {
      for (DateTime dateTime1 = new DateTime(fromDate.Year, 1, 1); dateTime1 <= this.recurrenceEndDate; dateTime1 = dateTime1.AddYears(this.recurrencePeriod))
      {
        DateTime dateTime2 = dateTime1;
        DateTime dateTime3;
        if (this.recurrenceWeek == RecurrenceWeek.None)
        {
          ref DateTime local = ref dateTime2;
          int year = dateTime1.Year;
          dateTime3 = this.RecurrenceMonthDay;
          int month = dateTime3.Month;
          dateTime3 = this.RecurrenceMonthDay;
          int day = dateTime3.Day;
          local = new DateTime(year, month, day);
        }
        else
        {
          int year = dateTime1.Year;
          dateTime3 = this.RecurrenceMonthDay;
          int month = dateTime3.Month;
          dateTime2 = CalendarUtils.GetFirstDayInMonthOfRecurrence(new DateTime(year, month, 1), this.recurrenceWeek, this.recurrenceDay, this.daysOfWeek);
        }
        if (dateTime2 > this.recurrenceEndDate)
          return DateTime.MinValue;
        ref DateTime local1 = ref dateTime2;
        dateTime3 = this.StartTime;
        TimeSpan timeOfDay = dateTime3.TimeOfDay;
        DateTime yearlyOccurrence = local1.Add(timeOfDay);
        if (yearlyOccurrence >= fromDate && !this.exceptionDates.Contains(yearlyOccurrence.Date))
          return yearlyOccurrence;
      }
      return DateTime.MinValue;
    }

    public AppointmentOccurrence[] GetOccurrencesInRange(DateTime startDate, DateTime endDate)
    {
      List<AppointmentOccurrence> appointmentOccurrenceList = new List<AppointmentOccurrence>();
      for (DateTime dateTime = startDate; (dateTime = this.GetNextOccurrence(dateTime)) != DateTime.MinValue && !(dateTime >= endDate); dateTime = dateTime.AddDays(1.0))
        appointmentOccurrenceList.Add(new AppointmentOccurrence(this, dateTime));
      return appointmentOccurrenceList.ToArray();
    }

    private void refreshRecurrenceSchedule()
    {
      this.recalculateRecurrenceEndDate();
      this.validateExceptionDates();
    }

    private void recalculateRecurrenceEndDate()
    {
      if (this.numberOfOccurrences < 0)
        return;
      this.recurrenceEndDate = DateTime.MaxValue;
      List<DateTime> exceptionDates = this.exceptionDates;
      this.exceptionDates = new List<DateTime>();
      DateTime fromDate = this.StartTime;
      for (int index = 0; index < this.numberOfOccurrences; ++index)
      {
        fromDate = this.GetNextOccurrence(fromDate);
        if (index < this.numberOfOccurrences - 1)
          fromDate = fromDate.AddDays(1.0);
      }
      this.recurrenceEndDate = fromDate;
      this.exceptionDates = exceptionDates;
    }

    private void validateExceptionDates()
    {
      for (int index = this.exceptionDates.Count - 1; index >= 0; --index)
      {
        if (this.GetNextOccurrence(this.exceptionDates[index]).Date != this.exceptionDates[index].Date)
          this.exceptionDates.RemoveAt(index);
      }
    }

    private void clearExceptions() => this.exceptionDates.Clear();

    public static RecurringAppointment CreateDailyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Daily,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateDailyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Daily,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateDailyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Daily,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        RecurrenceEndDate = recurrenceEndDate
      };
    }

    public static RecurringAppointment CreateWeeklyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Weekly,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateWeeklyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Weekly,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateWeeklyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      DaysOfWeek daysOfWeek,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Weekly,
        RecurrencePeriod = recurrencePeriod,
        DaysOfWeek = daysOfWeek,
        RecurrenceEndDate = recurrenceEndDate
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      int dayOfMonth)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        recurrenceMonthDay = new DateTime(2000, 1, dayOfMonth),
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      int dayOfMonth,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        recurrenceMonthDay = new DateTime(2000, 1, dayOfMonth),
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      int dayOfMonth,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        recurrenceMonthDay = new DateTime(2000, 1, dayOfMonth),
        RecurrenceEndDate = recurrenceEndDate
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      RecurrenceWeek week,
      RecurrenceDay day)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        RecurrenceWeek = week,
        RecurrenceDay = day,
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      RecurrenceWeek week,
      RecurrenceDay day,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        RecurrenceWeek = week,
        RecurrenceDay = day,
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateMonthlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      int recurrencePeriod,
      RecurrenceWeek week,
      RecurrenceDay day,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Monthly,
        RecurrencePeriod = recurrencePeriod,
        RecurrenceWeek = week,
        RecurrenceDay = day,
        RecurrenceEndDate = recurrenceEndDate
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      DateTime monthDay)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        recurrenceMonthDay = monthDay,
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      DateTime monthDay,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        recurrenceMonthDay = monthDay,
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      DateTime monthDay,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        recurrenceMonthDay = monthDay,
        RecurrenceEndDate = recurrenceEndDate
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      RecurrenceWeek week,
      RecurrenceDay day,
      int monthOfYear)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        RecurrenceMonthDay = new DateTime(2000, monthOfYear, 1),
        RecurrenceWeek = week,
        RecurrenceDay = day,
        RecurrenceEndDate = DateTime.MaxValue
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      RecurrenceWeek week,
      RecurrenceDay day,
      int monthOfYear,
      int numberOfOccurrences)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        RecurrenceMonthDay = new DateTime(2000, monthOfYear, 1),
        RecurrenceWeek = week,
        RecurrenceDay = day,
        NumberOfOccurrences = numberOfOccurrences
      };
    }

    public static RecurringAppointment CreateYearlyRecurrence(
      DateTime startDate,
      TimeSpan duration,
      string subject,
      RecurrenceWeek week,
      RecurrenceDay day,
      int monthOfYear,
      DateTime recurrenceEndDate)
    {
      return new RecurringAppointment(startDate, duration, subject)
      {
        RecurrenceType = RecurrenceType.Yearly,
        RecurrencePeriod = 1,
        RecurrenceMonthDay = new DateTime(2000, monthOfYear, 1),
        RecurrenceWeek = week,
        RecurrenceDay = day,
        RecurrenceEndDate = recurrenceEndDate
      };
    }
  }
}
