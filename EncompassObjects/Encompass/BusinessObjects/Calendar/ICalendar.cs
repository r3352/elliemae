// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.ICalendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Collections;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  /// <summary>Interface for Calendar class.</summary>
  /// <exclude />
  [Guid("3BD73B3C-5B1B-4053-B2C9-9F59A272534F")]
  public interface ICalendar
  {
    Appointment OpenAppointment(int apptId);

    Appointment CreateAppointment(DateTime startDate, DateTime endDate);

    AppointmentList GetAllAppointments();

    AppointmentList GetAppointmentsForDate(DateTime apptDate);

    AppointmentList GetAppointmentsInRange(DateTime rangeBegin, DateTime rangeEnd);

    Appointment CreateAppointmentForUser(User user, DateTime startDate, DateTime endDate);

    AppointmentList GetAllAppointmentsForUser(User user);

    AppointmentList GetAppointmentsForUserOnDate(User user, DateTime apptDate);

    AppointmentList GetAppointmentsForUserInRange(
      User user,
      DateTime rangeBegin,
      DateTime rangeEnd);
  }
}
