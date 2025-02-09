// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.Appointment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

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
  [ComSourceInterfaces(typeof (IPersistentObjectEvents))]
  public class Appointment : SessionBoundObject, IAppointment
  {
    private ScopedEventHandler<PersistentObjectEventArgs> committed;
    private AppointmentInfo apptInfo;
    private Appointment appt;
    private AppointmentContacts contacts;

    public event PersistentObjectEventHandler Committed
    {
      add
      {
        if (value == null)
          return;
        this.committed.Add(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
      remove
      {
        if (value == null)
          return;
        this.committed.Remove(new ScopedEventHandler<PersistentObjectEventArgs>.EventHandlerT((object) value, __methodptr(Invoke)));
      }
    }

    internal Appointment(Session session, AppointmentInfo apptInfo)
      : base(session)
    {
      this.initializeObject(apptInfo);
    }

    internal Appointment(Session session, string userId, DateTime startDate, DateTime endDate)
      : base(session)
    {
      if (endDate < startDate)
        throw new ArgumentException("End date must be on or after the start date");
      this.initializeObject(new AppointmentInfo()
      {
        StartDateTime = startDate,
        EndDateTime = endDate,
        UserID = userId
      });
    }

    private void initializeObject(AppointmentInfo apptInfo)
    {
      this.committed = new ScopedEventHandler<PersistentObjectEventArgs>(nameof (Appointment), "Committed");
      this.apptInfo = apptInfo;
      this.appt = this.createInfragisticsAppt(apptInfo);
      this.contacts = new AppointmentContacts(this);
    }

    public int ID
    {
      get
      {
        this.ensureValid();
        return Convert.ToInt32(this.appt.DataKey);
      }
    }

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

    public int ReminderInterval
    {
      get
      {
        this.ensureValid();
        if (!this.ReminderEnabled)
          return -1;
        if (this.appt.Reminder.DisplayIntervalUnits == null)
          return this.appt.Reminder.DisplayInterval;
        if (this.appt.Reminder.DisplayIntervalUnits == 1)
          return this.appt.Reminder.DisplayInterval * 60;
        return this.appt.Reminder.DisplayIntervalUnits == 2 ? this.appt.Reminder.DisplayInterval * 60 * 24 : 0;
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
          this.appt.Reminder.DisplayIntervalUnits = (DisplayIntervalUnits) 0;
        }
        else if (value < 1440)
        {
          this.appt.Reminder.DisplayInterval = value / 60;
          this.appt.Reminder.DisplayIntervalUnits = (DisplayIntervalUnits) 1;
        }
        else
        {
          this.appt.Reminder.DisplayInterval = value / 1440;
          this.appt.Reminder.DisplayIntervalUnits = (DisplayIntervalUnits) 2;
        }
      }
    }

    public bool ReminderActive
    {
      get
      {
        this.ensureValid();
        return this.ReminderEnabled && DateTime.Now < this.StartTime && DateTime.Now >= this.StartTime.AddMinutes((double) -this.ReminderInterval);
      }
    }

    public AppointmentContacts Contacts
    {
      get
      {
        this.ensureValid();
        return this.contacts;
      }
    }

    public bool IsNew()
    {
      this.ensureValid();
      return this.ID < 0;
    }

    public void Commit()
    {
      this.ensureValid();
      ContactInfo[] contactInfoList = this.contacts.GetContactInfoList();
      if (this.IsNew())
        this.appt.DataKey = (object) this.Session.Calendar.CalendarManager.AddNewBlankRecordForAppointment(this.apptInfo.UserID).ToString();
      this.syncApptInfoWithAppt();
      this.Session.Calendar.CalendarManager.UpdateAppointment(this.apptInfo, contactInfoList);
      this.committed((object) this, new PersistentObjectEventArgs(this.ID.ToString()));
    }

    public void Delete()
    {
      this.ensureValid();
      if (!this.IsNew())
        this.Session.Calendar.CalendarManager.DeleteAppointment(this.appt.DataKey.ToString());
      this.appt = (Appointment) null;
      this.apptInfo = (AppointmentInfo) null;
      this.contacts = (AppointmentContacts) null;
    }

    public void Refresh()
    {
      this.ensureValid();
      if (this.IsNew())
        throw new InvalidOperationException("Refresh cannot be called on a non-committed object");
      AppointmentInfo appointment = this.Session.Calendar.CalendarManager.GetAppointment(this.apptInfo.DataKey);
      if (appointment == null)
        return;
      this.apptInfo = appointment;
      this.appt = this.createInfragisticsAppt(this.apptInfo);
      this.contacts.Refresh();
    }

    internal AppointmentInfo Unwrap()
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

    private Appointment createInfragisticsAppt(AppointmentInfo info)
    {
      if (info.AllProperties != null)
        return Appointment.FromBytes(info.AllProperties);
      return new Appointment(info.StartDateTime, info.EndDateTime)
      {
        BarColor = Color.FromArgb(244, 234, 212),
        DataKey = (object) "-1"
      };
    }

    internal static AppointmentList ToList(Session session, AppointmentInfo[] infoList)
    {
      AppointmentList list = new AppointmentList();
      for (int index = 0; index < infoList.Length; ++index)
        list.Add(new Appointment(session, infoList[index]));
      return list;
    }
  }
}
