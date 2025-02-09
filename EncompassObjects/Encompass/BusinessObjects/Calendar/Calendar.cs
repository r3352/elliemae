// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.Calendar
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.Encompass.BusinessObjects.Users;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using System;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  /// <summary>Provides access to the user's calendar.</summary>
  /// <example>
  /// The following code display all of the appointments for the currently
  /// logged in user.
  /// <code>
  /// <![CDATA[
  /// using System;
  /// using System.IO;
  /// using EllieMae.Encompass.Client;
  /// using EllieMae.Encompass.BusinessObjects;
  /// using EllieMae.Encompass.BusinessObjects.Calendar;
  /// 
  /// class ContactManager
  /// {
  ///    public static void Main(string[] args)
  ///    {
  ///       // Open the session to the remote server
  ///       Session session = new Session();
  ///       session.Start("myserver", "mary", "maryspwd");
  /// 
  ///       // Get all of the current user's appointments and display them
  ///       // Mark items which are in a reminder state with a "*"
  ///       AppointmentList appts = session.Calendar.GetAllAppointments();
  /// 
  ///       foreach (Appointment appt in appts)
  ///       {
  ///          // Flag any appointment which is currently within its reminder period
  ///          if (appt.ReminderActive)
  ///             Console.Write("* ");
  /// 
  ///          Console.WriteLine(appt.StartTime + " - " + appt.EndTime + ": " + appt.Subject);
  ///       }
  /// 
  ///       // End the session to gracefully disconnect from the server
  ///       session.End();
  ///    }
  /// }
  /// ]]>
  /// </code>
  /// </example>
  public class Calendar : SessionBoundObject, ICalendar
  {
    internal Calendar(Session session)
      : base(session)
    {
    }

    /// <summary>
    /// Retrieves an appointment from the calendar with the specified ID.
    /// </summary>
    /// <param name="apptId">The <see cref="P:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.ID" /> of the Appointment.</param>
    /// <returns>The specified <see cref="T:EllieMae.Encompass.BusinessObjects.Calendar.Appointment" /> object, or <c>null</c>
    /// if no appointment exists with the specified ID.</returns>
    /// <remarks>Attempting to retrieve an appointment which does not belong to the
    /// currently logged in user will result in a <c>null</c> return value.</remarks>
    /// <example>
    /// The following code retrieves a specific appointment and moves the appointment
    /// time ahead by an hour.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the appointment specified on the command line
    ///       Appointment appt = session.Calendar.OpenAppointment(int.Parse(args[0]));
    /// 
    ///       // Make sure the appointment exists
    ///       if (appt == null)
    ///       {
    ///          Console.WriteLine("The specified appointment does not exist.");
    ///          return;
    ///       }
    /// 
    ///       // Move the time forward by an hour, starting with the End Time
    ///       appt.EndTime = appt.EndTime.AddHours(1);
    ///       appt.StartTime = appt.StartTime.AddHours(1);
    /// 
    ///       // Save the changes to the appointment
    ///       appt.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Appointment OpenAppointment(int apptId)
    {
      AppointmentInfo appointment = this.CalendarManager.GetAppointment(apptId.ToString());
      if (appointment == null)
        return (Appointment) null;
      return appointment.UserID != this.Session.UserID && !this.Session.GetUserInfo().IsAdministrator() ? (Appointment) null : new Appointment(this.Session, appointment);
    }

    /// <summary>Creates a new appointment in the calendar.</summary>
    /// <param name="startDate">The starting date and time for the appointment.</param>
    /// <param name="endDate">The ending date and time for the appointment.</param>
    /// <returns>The new <see cref="T:EllieMae.Encompass.BusinessObjects.Calendar.Appointment" /> object.</returns>
    /// <remarks>The returned Appointment object is unsaved and will not be
    /// persisted to the Encompass Server until the <see cref="M:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.Commit" />
    /// method is invoked.</remarks>
    /// <example>
    /// The following code demonstrates how to add a new Appointment to the
    /// current user's Calendar.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// using EllieMae.Encompass.BusinessObjects.Contacts;
    /// using EllieMae.Encompass.Query;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Create a new appointment in the calendar tha will start 3 hours from now
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3), DateTime.Now.AddHours(4));
    /// 
    ///       // Set the appointments properties
    ///       appt.Subject = "Meet with Margaret Taylor re: rate lock";
    ///       appt.Location = "220 W. Grand Ave.";
    /// 
    ///       // Set a reminder for an hour before the appointment
    ///       appt.ReminderEnabled = true;
    ///       appt.ReminderInterval = 60;
    /// 
    ///       // Fetch the contact to attach to this appointment
    ///       StringFieldCriterion fnCri = new StringFieldCriterion();
    ///       fnCri.FieldName = "Contact.FirstName";
    ///       fnCri.Value = "Margaret";
    /// 
    ///       StringFieldCriterion lnCri = new StringFieldCriterion();
    ///       lnCri.FieldName = "Contact.LastName";
    ///       lnCri.Value = "Taylor";
    /// 
    ///       ContactList contacts = session.Contacts.Query(fnCri.And(lnCri), ContactLoanMatchType.None,
    ///          ContactType.Borrower);
    /// 
    ///       // Add the contacts to the appointment
    ///       for (int i = 0; i < contacts.Count; i++)
    ///          appt.Contacts.Add(contacts[i]);
    /// 
    ///       // Save the changes to the appointment
    ///       appt.Commit();
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public Appointment CreateAppointment(DateTime startDate, DateTime endDate)
    {
      return new Appointment(this.Session, this.Session.UserID, startDate, endDate);
    }

    /// <summary>Creates a new appointment for the specified user.</summary>
    /// <param name="user">The user for whom the appointment is to be scheduled.</param>
    /// <param name="startDate">The starting date and time for the appointment.</param>
    /// <param name="endDate">The ending date and time for the appointment.</param>
    /// <remarks>The returned Appointment object is unsaved and will not be
    /// persisted to the Encompass Server until the <see cref="M:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.Commit" />
    /// method is invoked.
    /// <p>This method can only be invoked by users with the Administrator persona.</p>
    /// </remarks>
    public Appointment CreateAppointmentForUser(User user, DateTime startDate, DateTime endDate)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("This operation can only be invoked by users with Administrator persona");
      return new Appointment(this.Session, user.ID, startDate, endDate);
    }

    /// <summary>
    /// Retrieves all of the appointments for the current user.
    /// </summary>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> containing all appointments,
    /// past and present, for the current user.</returns>
    /// <remarks>The returned list will be sorted based on the appointments' start times.
    /// However, all-day events may be sequenced anywhere within the set of events which occur
    /// on the same day.
    /// <note type="implementnotes">If the user has a large number of appointments, this
    /// call may require significant time to complete.</note></remarks>
    /// <example>
    /// The following code display all of the appointments for the currently
    /// logged in user.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Get all of the current user's appointments and display them
    ///       // Mark items which are in a reminder state with a "*"
    ///       AppointmentList appts = session.Calendar.GetAllAppointments();
    /// 
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // Flag any appointment which is currently within its reminder period
    ///          if (appt.ReminderActive)
    ///             Console.Write("* ");
    /// 
    ///          Console.WriteLine(appt.StartTime + " - " + appt.EndTime + ": " + appt.Subject);
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public AppointmentList GetAllAppointments()
    {
      return Appointment.ToList(this.Session, this.CalendarManager.GetAllAppointmentsForUser(this.Session.UserID));
    }

    /// <summary>
    /// Retrieves a list of all the appointments for a specified user.
    /// </summary>
    /// <param name="user">The user for whom the appointments are to be retrieved.</param>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> containing all appointments,
    /// past and present, for the specified user.</returns>
    /// <remarks>The returned list will be sorted based on the appointments' start times.
    /// However, all-day events may be sequenced anywhere within the set of events which occur
    /// on the same day.
    /// <p>This method can only be invoked by users with the Administrator persona.</p>
    /// <note type="implementnotes">If the user has a large number of appointments, this
    /// call may require significant time to complete.</note></remarks>
    public AppointmentList GetAllAppointmentsForUser(User user)
    {
      if (user == null)
        throw new ArgumentNullException(nameof (user));
      if (!this.Session.GetUserInfo().IsAdministrator())
        throw new InvalidOperationException("This operation can only be invoked by users with Administrator persona");
      return Appointment.ToList(this.Session, this.CalendarManager.GetAllAppointmentsForUser(user.ID));
    }

    /// <summary>
    /// Retrieves all of the appointments that occur during a specified date.
    /// </summary>
    /// <param name="apptDate">The date for which appointments will be retrieved.</param>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> containing all appointments for
    /// the specified date.</returns>
    /// <example>
    /// The following code retrieves today's calendar for the current user and moves
    /// all meetings currently scheduled to occur in the "Meeting Room" to the
    /// "Board Room".
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Open the appointment specified on the command line
    ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
    /// 
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // If the meeting is in the "Meeting Room", change it's location and save it
    ///          if (appt.Location == "Meeting Room")
    ///          {
    ///             appt.Location = "Board Room";
    ///             appt.Commit();
    ///          }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public AppointmentList GetAppointmentsForDate(DateTime apptDate)
    {
      return this.GetAppointmentsInRange(apptDate.Date, apptDate.Date.AddDays(1.0));
    }

    /// <summary>
    /// Retrieves all of a user's appointments that occur during a specified date.
    /// </summary>
    /// <param name="user">The user for whom you are fetching the appointments.</param>
    /// <param name="apptDate">The date for which appointments will be retrieved.</param>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> containing all appointments for
    /// the specified date.</returns>
    /// <remarks>This method can only be invoked by users with the Administrator persona.</remarks>
    public AppointmentList GetAppointmentsForUserOnDate(User user, DateTime apptDate)
    {
      return this.GetAppointmentsForUserInRange(user, apptDate.Date, apptDate.Date.AddDays(1.0));
    }

    /// <summary>
    /// Retrieves all appointments that occur within a specified time range.
    /// </summary>
    /// <param name="rangeBegin">The start of the date/time range.</param>
    /// <param name="rangeEnd">The end of the date/time range.</param>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> that contains all appointments
    /// from the Calendar which begin and/or end in the specified time range.
    /// Appointments which begin exctly at the time specified by the <c>rangeEnd</c>
    /// parameter are not included in the result set. In other words, the date range used
    /// is denoted as [<c>rangeBegin</c>, <c>rangeEnd</c>).
    /// </returns>
    /// <example>
    /// The following code demonstrates how to determine what appointments the current
    /// user has in a specified time range.
    /// <code>
    /// <![CDATA[
    /// using System;
    /// using System.IO;
    /// using EllieMae.Encompass.Client;
    /// using EllieMae.Encompass.BusinessObjects;
    /// using EllieMae.Encompass.BusinessObjects.Calendar;
    /// 
    /// class ContactManager
    /// {
    ///    public static void Main(string[] args)
    ///    {
    ///       // Open the session to the remote server
    ///       Session session = new Session();
    ///       session.Start("myserver", "mary", "maryspwd");
    /// 
    ///       // Check what appointments are going on during the next hour
    ///       AppointmentList appts = session.Calendar.GetAppointmentsInRange(DateTime.Now,
    ///          DateTime.Now.AddHours(1));
    /// 
    ///       // Print them to the console
    ///       for (int i = 0; i < appts.Count; i++)
    ///          Console.WriteLine(appts[i].StartTime + " - " + appts[i].EndTime + ": " +
    ///             appts[i].Subject);
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public AppointmentList GetAppointmentsInRange(DateTime rangeBegin, DateTime rangeEnd)
    {
      return Appointment.ToList(this.Session, this.CalendarManager.GetAppointmentsForUser(this.Session.UserID, rangeBegin, rangeEnd));
    }

    /// <summary>
    /// Retrieves all appointments for the specified user that occur within a specified time range.
    /// </summary>
    /// <param name="user">The user for whom the appointments are scheduled.</param>
    /// <param name="rangeBegin">The start of the date/time range.</param>
    /// <param name="rangeEnd">The end of the date/time range.</param>
    /// <returns>An <see cref="T:EllieMae.Encompass.Collections.AppointmentList" /> that contains all appointments
    /// from the Calendar which begin and/or end in the specified time range.
    /// Appointments which begin exctly at the time specified by the <c>rangeEnd</c>
    /// parameter are not included in the result set. In other words, the date range used
    /// is denoted as [<c>rangeBegin</c>, <c>rangeEnd</c>).
    /// </returns>
    /// <remarks>This method can only be invoked by users with the Administrator persona.</remarks>
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
