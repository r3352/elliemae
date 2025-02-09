// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.BusinessCalendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  /// <summary>
  /// Represents a calendar with defined business and non-business days.
  /// </summary>
  /// <remarks>The dates in the Encompass calendars range from Jan 1, 2000 to Dec 31, 2029. Any
  /// date outside that range will cause an exception to be raised from the calendar.</remarks>
  public class BusinessCalendar : SessionBoundObject, IBusinessCalendar
  {
    private EllieMae.EMLite.ClientServer.BusinessCalendar calendar;

    internal BusinessCalendar(Session session, EllieMae.EMLite.ClientServer.BusinessCalendar calendar)
      : base(session)
    {
      this.calendar = calendar;
    }

    /// <summary>
    /// Gets the type of calendar represented by the current object.
    /// </summary>
    public BusinessCalendarType CalendarType => (BusinessCalendarType) this.calendar.CalendarType;

    /// <summary>
    /// Gets the workdays (i.e. non-weekend days) for the calendar
    /// </summary>
    public DaysOfWeek WorkDays => (DaysOfWeek) this.calendar.WorkDays;

    /// <summary>
    /// Indicates if a specific date falls on a weekend (as defined by the calendar).
    /// </summary>
    /// <param name="date">The date to test.</param>
    /// <returns>Return <c>true</c> if the date falls on a day marked as a weekend day,
    /// <c>false</c> otherwise.</returns>
    public bool IsWeekendDay(DateTime date) => this.calendar.IsWeekendDay(date);

    /// <summary>Indicates if a specific date is a business day.</summary>
    /// <param name="date">The date to test.</param>
    /// <returns>Return <c>true</c> if the date is a business day,
    /// <c>false</c> otherwise.</returns>
    public bool IsBusinessDay(DateTime date) => this.calendar.IsBusinessDay(date);

    /// <summary>Gets the type of day for a specific date.</summary>
    /// <param name="date">The date to test.</param>
    /// <returns>Returns the <see cref="T:EllieMae.Encompass.Configuration.BusinessCalendarDayType" /> of the specified date based
    /// on the definition of the calendar.</returns>
    public BusinessCalendarDayType GetDayType(DateTime date)
    {
      return (BusinessCalendarDayType) this.calendar.GetDayType(date);
    }

    /// <summary>
    /// Adds a specified number of business days to the specified date.
    /// </summary>
    /// <param name="date">The starting date of the operation.</param>
    /// <param name="count">The number of business days to add.</param>
    /// <param name="startFromClosestBusinessDay">A flag to indicate if the date should be advanced to the
    /// next closest business day prior to adding the number of days specified by the count.</param>
    /// <returns>Returns the date of the business day as computed from the calendar.</returns>
    /// <remarks>If the <c>date</c> parameter is a non-business day and
    /// <c>startFromClosestBusinessDay</c> is <c>true</c>, the computation will first advanced to
    /// the next closest business day and then add <c>count</c> additional business days. If
    /// the <c>startFromClosestBusinessDay</c> parameter is <c>false</c>, then the next closest
    /// business day will count as the first day in the computation. If the <c>date</c> parameter is
    /// a business day, then the <c>startFromClosestBusinessDay</c> value will have no effect on
    /// the computation.</remarks>
    public DateTime AddBusinessDays(DateTime date, int count, bool startFromClosestBusinessDay)
    {
      return this.calendar.AddBusinessDays(date, count, startFromClosestBusinessDay);
    }

    /// <summary>
    /// Returns the next closest business day to the specified date.
    /// </summary>
    /// <param name="date">The date from which to start.</param>
    /// <returns>The first business day on or after the specified <c>date</c>.</returns>
    /// <remarks>If the date specified by the <c>date</c> parameter is a business day,
    /// then that date will be returned. Otherwise, the calendar will advance from the specified
    /// date until a business day is found and return that date.</remarks>
    public DateTime GetNextClosestBusinessDay(DateTime date)
    {
      return this.calendar.GetNextClosestBusinessDay(date);
    }
  }
}
