// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.AppointmentOccurrence
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  public class AppointmentOccurrence : Appointment
  {
    private RecurringAppointment masterAppt;
    private DateTime startTime;

    public AppointmentOccurrence(RecurringAppointment masterAppt, DateTime startTime)
      : base(masterAppt.AppointmentID + "#" + startTime.ToString("mmddyyyy"))
    {
      this.masterAppt = masterAppt;
      this.startTime = startTime;
    }

    public override AppointmentType AppointmentType => AppointmentType.Occurrence;

    public override DateTime StartTime => this.startTime;

    public override TimeSpan Duration => this.masterAppt.Duration;

    public override string Subject
    {
      get => this.masterAppt.Subject;
      set => this.masterAppt.Subject = value;
    }

    public RecurringAppointment Recurrence => this.masterAppt;

    public override void Reschedule(DateTime newStartTime, TimeSpan newDuration)
    {
      throw new Exception("An occurrence cannot be rescheduled. You must create a new exception for this appointment.");
    }
  }
}
