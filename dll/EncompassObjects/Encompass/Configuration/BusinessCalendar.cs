// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Configuration.BusinessCalendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.Encompass.Client;
using System;

#nullable disable
namespace EllieMae.Encompass.Configuration
{
  public class BusinessCalendar : SessionBoundObject, IBusinessCalendar
  {
    private BusinessCalendar calendar;

    internal BusinessCalendar(Session session, BusinessCalendar calendar)
      : base(session)
    {
      this.calendar = calendar;
    }

    public BusinessCalendarType CalendarType => (BusinessCalendarType) this.calendar.CalendarType;

    public DaysOfWeek WorkDays => (DaysOfWeek) this.calendar.WorkDays;

    public bool IsWeekendDay(DateTime date) => this.calendar.IsWeekendDay(date);

    public bool IsBusinessDay(DateTime date) => this.calendar.IsBusinessDay(date);

    public BusinessCalendarDayType GetDayType(DateTime date)
    {
      return (BusinessCalendarDayType) this.calendar.GetDayType(date);
    }

    public DateTime AddBusinessDays(DateTime date, int count, bool startFromClosestBusinessDay)
    {
      return this.calendar.AddBusinessDays(date, count, startFromClosestBusinessDay, false, false, false);
    }

    public DateTime GetNextClosestBusinessDay(DateTime date)
    {
      return this.calendar.GetNextClosestBusinessDay(date);
    }
  }
}
