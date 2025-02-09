// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.Calendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  public class Calendar : SessionBoundObject, ICalendar
  {
    internal Calendar(Session session)
      : base(session)
    {
    }

    public Appointment OpenAppointment(int apptId)
    {
      AppointmentInfo appointment = this.CalendarManager.GetAppointment(apptId.ToString());
      if (appointment == null)
        return (Appointment) null;
      return appointment.UserID != this.Session.UserID && !this.Session.GetUserInfo().IsAdministrator() ? (Appointment) null : new Appointment(this.Session, appointment);
    }

    public Appointment CreateAppointment(DateTime startDate, DateTime endDate)
    {
      return new Appointment(this.Session, this.Session.UserID, startDate, endDate);
    }

    public Appointment CreateAppointmentForUser(User user, DateTime startDate, DateTime endDate)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("This operation can only be invoked by users with Administrator persona");
      return new Appointment(this.Session, user.ID, startDate, endDate);
    }

    public AppointmentList GetAllAppointments()
    {
      return Appointment.ToList(this.Session, this.CalendarManager.GetAllAppointmentsForUser(this.Session.UserID));
    }

    public AppointmentList GetAllAppointmentsForUser(User user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("This operation can only be invoked by users with Administrator persona");
      return Appointment.ToList(this.Session, this.CalendarManager.GetAllAppointmentsForUser(user.ID));
    }

    public AppointmentList GetAppointmentsForDate(DateTime apptDate)
    {
      return this.GetAppointmentsInRange(apptDate.Date, apptDate.Date.AddDays(1.0));
    }

    public AppointmentList GetAppointmentsForUserOnDate(User user, DateTime apptDate)
    {
      return this.GetAppointmentsForUserInRange(user, apptDate.Date, apptDate.Date.AddDays(1.0));
    }

    public AppointmentList GetAppointmentsInRange(DateTime rangeBegin, DateTime rangeEnd)
    {
      return Appointment.ToList(this.Session, this.CalendarManager.GetAppointmentsForUser(this.Session.UserID, rangeBegin, rangeEnd));
    }

    public AppointmentList GetAppointmentsForUserInRange(
      User user,
      DateTime rangeBegin,
      DateTime rangeEnd)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("This operation can only be invoked by users with Administrator persona");
      return Appointment.ToList(this.Session, this.CalendarManager.GetAppointmentsForUser(user.ID, rangeBegin, rangeEnd));
    }

    internal ICalendarManager CalendarManager
    {
      get => (ICalendarManager) this.Session.GetObject(nameof (CalendarManager));
    }
  }
}
