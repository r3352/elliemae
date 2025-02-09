// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.Appointment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.EMLite.ClientServer;
using EllieMae.EMLite.ClientServer.Calendar;
using EllieMae.Encompass.Client;
using EllieMae.Encompass.Collections;
using Infragistics.Win.UltraWinSchedule;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  /// <summary>Represents a single appointment in a user's calendar.</summary>
  /// <example>
  /// The following code creates a new appointment and enables a reminder for
  /// one hour before the appointment begins.
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
  ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3),
  ///          DateTime.Now.AddHours(4));
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
  [ComSourceInterfaces(typeof (IPersistentObjectEvents))]
  public class Appointment : SessionBoundObject, IAppointment
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo apptInfo;
    private Infragistics.Win.UltraWinSchedule.Appointment appt;
    private AppointmentContacts contacts;

    /// <summary>Event indicating that the object has been committed to the server.</summary>
    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT(value.Invoke));
      }
    }

    internal Appointment(Session session, EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo apptInfo)
      : base(session)
    {
      this.initializeObject(apptInfo);
    }

    internal Appointment(Session session, string userId, DateTime startDate, DateTime endDate)
      : base(session)
    {
      if (endDate < startDate)
        throw new ArgumentException("End date must be on or after the start date");
      this.initializeObject(new EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo()
      {
        StartDateTime = startDate,
        EndDateTime = endDate,
        UserID = userId
      });
    }

    private void initializeObject(EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo apptInfo)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Appointment), "Committed");
      this.apptInfo = apptInfo;
      this.appt = this.createInfragisticsAppt(apptInfo);
      this.contacts = new AppointmentContacts(this);
    }

    /// <summary>Gets the unique identifier for this appointment.</summary>
    public int ID
    {
      get
      {
        this.ensureValid();
        return Convert.ToInt32(this.appt.DataKey);
      }
    }

    /// <summary>Gets or sets the subject for the Appointment.</summary>
    /// <example>
    /// The following code creates a new appointment and enables a reminder for
    /// one hour before the appointment begins.
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
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3),
    ///          DateTime.Now.AddHours(4));
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
    public string Subject
    {
      get
      {
        this.ensureValid();
        return this.appt.Subject;
      }
      set
      {
        this.ensureValid();
        this.appt.Subject = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the start date and time for the Appointment.
    /// </summary>
    /// <remarks>If you set the <c>StartTime</c> of an appointment to be after the <c>EndTime</c>,
    /// the <c>EndTime</c> will be automatically adjusted to be concurrent with the <c>StartTime</c>.
    /// </remarks>
    /// <example>
    /// The following code locates all appointments scheduled for today and reschedules
    /// them for tomorrow.
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
    ///       // Retrieve all appointments which are scheduled for today
    ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
    /// 
    ///       // For each appointment, move the appointment's start date to tomorrow
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // Get the length of the appointment so we can preserve it
    ///          TimeSpan span = appt.EndTime - appt.StartTime;
    /// 
    ///          // Advance the appointment to the same time tomorrow
    ///          appt.StartTime = DateTime.Today.AddDays(1) + appt.StartTime.TimeOfDay;
    ///          appt.EndTime = appt.StartTime + span;
    /// 
    ///          // Save the changes
    ///          appt.Commit();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public DateTime StartTime
    {
      get
      {
        this.ensureValid();
        return this.AllDayEvent ? this.appt.StartDateTime.Date : this.appt.StartDateTime;
      }
      set
      {
        this.ensureValid();
        if (this.AllDayEvent)
          value = value.Date;
        if (value > this.EndTime)
          this.appt.EndDateTime = value;
        this.appt.StartDateTime = value;
      }
    }

    /// <summary>
    /// Gets or sets the date and time at which the appointment ends.
    /// </summary>
    /// <remarks>If you set the <c>EndTime</c> of an appointment to be prior to the <c>StartTime</c>,
    /// the <c>StartTime</c> will be automatically adjusted to be concurrent with the <c>EndTime</c>.
    /// </remarks>
    /// <example>
    /// The following code locates all appointments scheduled for today and reschedules
    /// them for tomorrow.
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
    ///       // Retrieve all appointments which are scheduled for today
    ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
    /// 
    ///       // For each appointment, move the appointment's start date to tomorrow
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // Get the length of the appointment so we can preserve it
    ///          TimeSpan span = appt.EndTime - appt.StartTime;
    /// 
    ///          // Advance the appointment to the same time tomorrow
    ///          appt.StartTime = DateTime.Today.AddDays(1) + appt.StartTime.TimeOfDay;
    ///          appt.EndTime = appt.StartTime + span;
    /// 
    ///          // Save the changes
    ///          appt.Commit();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public DateTime EndTime
    {
      get
      {
        this.ensureValid();
        return this.AllDayEvent ? this.appt.EndDateTime.Date : this.appt.EndDateTime;
      }
      set
      {
        this.ensureValid();
        if (this.AllDayEvent)
          value = value.Date;
        if (value < this.StartTime)
          this.appt.StartDateTime = value;
        this.appt.EndDateTime = value;
      }
    }

    /// <summary>Gets or sets the description for the Appointment.</summary>
    /// <example>
    /// The following code creates a new appointment and enables a reminder for
    /// one hour before the appointment begins.
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
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3),
    ///          DateTime.Now.AddHours(4));
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
    public string Location
    {
      get
      {
        this.ensureValid();
        return this.appt.Location;
      }
      set
      {
        this.ensureValid();
        this.appt.Location = value ?? "";
      }
    }

    /// <summary>Gets or sets the comments for the Appointment.</summary>
    public string Comments
    {
      get
      {
        this.ensureValid();
        return this.appt.Description;
      }
      set
      {
        this.ensureValid();
        this.appt.Description = value ?? "";
      }
    }

    /// <summary>
    /// Gets or sets the color of the appointment's bar in the calendar view.
    /// </summary>
    public Color BarColor
    {
      get
      {
        this.ensureValid();
        return this.appt.BarColor;
      }
      set
      {
        this.ensureValid();
        this.appt.BarColor = value;
      }
    }

    /// <summary>
    /// Gets or sets a flag indicating if the event is an all-day event.
    /// </summary>
    /// <example>
    /// The following code removes all appointment from the calendar which have
    /// already ended.
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
    ///       // Create a temp list to hold the appointments to delete
    ///       AppointmentList appts = session.Calendar.GetAllAppointments();
    /// 
    ///       // Loop through the appoints to find any in the past
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          bool delete = false;
    /// 
    ///          // Handle all-day event specially since their end date is the final full
    ///          // day of the event.
    ///          if (appt.AllDayEvent)
    ///          {
    ///             if (appt.EndTime < DateTime.Today)
    ///                delete = true;
    ///          }
    ///          else
    ///          {
    ///             if (appt.EndTime < DateTime.Now)
    ///                delete = true;
    ///          }
    /// 
    ///          // Delete the appointment if required
    ///          if (delete)
    ///             appt.Delete();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public bool AllDayEvent
    {
      get
      {
        this.ensureValid();
        return this.appt.AllDayEvent;
      }
      set
      {
        this.ensureValid();
        this.appt.AllDayEvent = value;
      }
    }

    /// <summary>
    /// Gets or sets a flag indicating if a reminder should be shown for this event.
    /// </summary>
    /// <example>
    /// The following code creates a new appointment and enables a reminder for
    /// one hour before the appointment begins.
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
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3),
    ///          DateTime.Now.AddHours(4));
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
    public bool ReminderEnabled
    {
      get
      {
        this.ensureValid();
        return this.appt.Reminder.Enabled;
      }
      set
      {
        this.ensureValid();
        this.appt.Reminder.Enabled = value;
      }
    }

    /// <summary>
    /// Gets or sets the number of minutes prior to the appointment for the reminder to
    /// be displayed.
    /// </summary>
    /// <remarks>This property will return -1 if the <see cref="P:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.ReminderEnabled" />
    /// property is <c>false</c>.
    /// <p>This property can only take on values that represent a whole number minutes (if &lt; 60),
    /// hours (if &gt;= 60 and &lt; 1440), or days (if &gt;= 1440). Attempts to set this value to something
    /// which does not fit this criteria will result in the value being rounded down to the nearest hour
    /// or day.</p>
    /// <p>For example, setting this property to the value 330 (5 hours and 30 minutes) would
    /// result in the interval being rounded down to 300 (5 hours).</p>
    /// </remarks>
    /// <example>
    /// The following code displays all appointments which have a reminder set and
    /// are within the reminder period.
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
    ///       // Get a list of all current and future appointments
    ///       AppointmentList appts = session.Calendar.GetAppointmentsInRange(DateTime.Now,
    ///          DateTime.MaxValue);
    /// 
    ///       // Get all appointments which are within their reminder period
    ///       foreach (Appointment appt in session.Calendar.GetAllAppointments())
    ///       {
    ///          // If the appointment is in the future but the reminder time is
    ///          // in the past, print a notification
    ///          if (appt.ReminderEnabled &&
    ///             (appt.StartTime > DateTime.Now) &&
    ///             (appt.StartTime.AddMinutes(-appt.ReminderInterval) <= DateTime.Now))
    ///             {
    ///                Console.WriteLine("Reminder: " + appt.Subject + " starts at " + appt.StartTime);
    ///             }
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public int ReminderInterval
    {
      get
      {
        this.ensureValid();
        if (!this.ReminderEnabled)
          return -1;
        if (this.appt.Reminder.DisplayIntervalUnits == DisplayIntervalUnits.Minutes)
          return this.appt.Reminder.DisplayInterval;
        if (this.appt.Reminder.DisplayIntervalUnits == DisplayIntervalUnits.Hours)
          return this.appt.Reminder.DisplayInterval * 60;
        return this.appt.Reminder.DisplayIntervalUnits == DisplayIntervalUnits.Days ? this.appt.Reminder.DisplayInterval * 60 * 24 : 0;
      }
      set
      {
        this.ensureValid();
        if (!this.ReminderEnabled)
          throw new InvalidOperationException("Reminder is not enabled for this appointment");
        if (value < 0)
          throw new ArgumentException("Reminder interval must be positive if reminder is enabled");
        if (value < 60)
        {
          this.appt.Reminder.DisplayInterval = value;
          this.appt.Reminder.DisplayIntervalUnits = DisplayIntervalUnits.Minutes;
        }
        else if (value < 1440)
        {
          this.appt.Reminder.DisplayInterval = value / 60;
          this.appt.Reminder.DisplayIntervalUnits = DisplayIntervalUnits.Hours;
        }
        else
        {
          this.appt.Reminder.DisplayInterval = value / 1440;
          this.appt.Reminder.DisplayIntervalUnits = DisplayIntervalUnits.Days;
        }
      }
    }

    /// <summary>
    /// Indicates if the appointment's reminder is currently in effect.
    /// </summary>
    /// <remarks>This property returns <c>true</c> if the appointment's reminder is
    /// enabled and the current time is within the <see cref="P:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.ReminderInterval" />
    /// of the appointment's <see cref="P:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.StartTime" />.</remarks>
    public bool ReminderActive
    {
      get
      {
        this.ensureValid();
        return this.ReminderEnabled && DateTime.Now < this.StartTime && DateTime.Now >= this.StartTime.AddMinutes((double) -this.ReminderInterval);
      }
    }

    /// <summary>
    /// Gets the collection of contacts associated with this appointment.
    /// </summary>
    /// <example>
    /// The following code creates a new appointment and enables a reminder for
    /// one hour before the appointment begins.
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
    ///       Appointment appt = session.Calendar.CreateAppointment(DateTime.Now.AddHours(3),
    ///          DateTime.Now.AddHours(4));
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
    public AppointmentContacts Contacts
    {
      get
      {
        this.ensureValid();
        return this.contacts;
      }
    }

    /// <summary>
    /// Indicates if the Appointment has not yet been saved to the Encompass server.
    /// </summary>
    /// <returns></returns>
    public bool IsNew()
    {
      this.ensureValid();
      return this.ID < 0;
    }

    /// <summary>
    /// Commits the changes to the appointment to the Encompass server.
    /// </summary>
    /// <example>
    /// The following code locates all appointments scheduled for today and reschedules
    /// them for tomorrow.
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
    ///       // Retrieve all appointments which are scheduled for today
    ///       AppointmentList appts = session.Calendar.GetAppointmentsForDate(DateTime.Today);
    /// 
    ///       // For each appointment, move the appointment's start date to tomorrow
    ///       foreach (Appointment appt in appts)
    ///       {
    ///          // Get the length of the appointment so we can preserve it
    ///          TimeSpan span = appt.EndTime - appt.StartTime;
    /// 
    ///          // Advance the appointment to the same time tomorrow
    ///          appt.StartTime = DateTime.Today.AddDays(1) + appt.StartTime.TimeOfDay;
    ///          appt.EndTime = appt.StartTime + span;
    /// 
    ///          // Save the changes
    ///          appt.Commit();
    ///       }
    /// 
    ///       // End the session to gracefully disconnect from the server
    ///       session.End();
    ///    }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public void Commit()
    {
      this.ensureValid();
      ContactInfo[] contactInfoList = this.contacts.GetContactInfoList();
      if (this.IsNew())
        this.appt.DataKey = (object) this.Session.Calendar.CalendarManager.AddNewBlankRecordForAppointment(this.apptInfo.UserID).ToString();
      this.syncApptInfoWithAppt();
      this.Session.Calendar.CalendarManager.UpdateAppointment(this.apptInfo, contactInfoList);
      this.committed.Invoke((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }

    /// <summary>Deletes the current appointment from the server.</summary>
    public void Delete()
    {
      this.ensureValid();
      if (!this.IsNew())
        this.Session.Calendar.CalendarManager.DeleteAppointment(this.appt.DataKey.ToString());
      this.appt = (Infragistics.Win.UltraWinSchedule.Appointment) null;
      this.apptInfo = (EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo) null;
      this.contacts = (AppointmentContacts) null;
    }

    /// <summary>Refreshes the object from the Encompass Server.</summary>
    /// <remarks>Any changes made to this object since the last call to <see cref="M:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.Commit" />
    /// will be lost. Attempting to invoke this method on an Appointment for which <see cref="M:EllieMae.Encompass.BusinessObjects.Calendar.Appointment.IsNew" />
    /// returns <c>true</c> will result in an exception.</remarks>
    public void Refresh()
    {
      this.ensureValid();
      if (this.IsNew())
        throw new InvalidOperationException("Refresh cannot be called on a non-committed object");
      EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo appointment = this.Session.Calendar.CalendarManager.GetAppointment(this.apptInfo.DataKey);
      if (appointment == null)
        return;
      this.apptInfo = appointment;
      this.appt = this.createInfragisticsAppt(this.apptInfo);
      this.contacts.Refresh();
    }

    internal EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo Unwrap()
    {
      this.ensureValid();
      this.syncApptInfoWithAppt();
      return this.apptInfo;
    }

    private void ensureValid()
    {
      if (this.apptInfo == null)
        throw new ObjectDisposedException(nameof (Appointment));
    }

    private void syncApptInfoWithAppt()
    {
      this.apptInfo.AllDayEvent = this.appt.AllDayEvent;
      this.apptInfo.AllProperties = this.appt.Save();
      this.apptInfo.Description = this.appt.Description;
      this.apptInfo.EndDateTime = this.appt.EndDateTime;
      this.apptInfo.OriginalStartDateTime = this.appt.OriginalStartDateTime;
      this.apptInfo.OwnerKey = this.appt.OwnerKey;
      this.apptInfo.Recurrence = this.serializeObject((object) this.appt.Recurrence);
      this.apptInfo.RecurrenceId = this.appt.RecurrenceId == Guid.Empty ? "" : this.appt.RecurrenceId.ToString();
      this.apptInfo.ReminderEnabled = this.appt.Reminder.Enabled;
      this.apptInfo.ReminderInterval = this.appt.Reminder.DisplayInterval;
      this.apptInfo.ReminderUnits = (int) this.appt.Reminder.DisplayIntervalUnits;
      this.apptInfo.StartDateTime = this.appt.StartDateTime;
      this.apptInfo.Subject = this.appt.Subject;
      this.apptInfo.DataKey = this.appt.DataKey.ToString();
    }

    private byte[] serializeObject(object o)
    {
      if (o == null)
        return (byte[]) null;
      MemoryStream serializationStream = new MemoryStream();
      new BinaryFormatter().Serialize((Stream) serializationStream, o);
      return serializationStream.ToArray();
    }

    private Infragistics.Win.UltraWinSchedule.Appointment createInfragisticsAppt(
      EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo info)
    {
      if (info.AllProperties != null)
        return Infragistics.Win.UltraWinSchedule.Appointment.FromBytes(info.AllProperties);
      return new Infragistics.Win.UltraWinSchedule.Appointment(info.StartDateTime, info.EndDateTime)
      {
        BarColor = Color.FromArgb(244, 234, 212),
        DataKey = (object) "-1"
      };
    }

    internal static AppointmentList ToList(Session session, EllieMae.EMLite.ClientServer.Calendar.AppointmentInfo[] infoList)
    {
      AppointmentList list = new AppointmentList();
      for (int index = 0; index < infoList.Length; ++index)
        list.Add(new Appointment(session, infoList[index]));
      return list;
    }
  }
}
