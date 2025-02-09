// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.Appointment
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  public abstract class Appointment
  {
    private string apptId;

    public Appointment(string apptId) => this.apptId = apptId;

    public string AppointmentID => this.apptId;

    public abstract DateTime StartTime { get; }

    public abstract TimeSpan Duration { get; }

    public abstract string Subject { get; set; }

    public abstract AppointmentType AppointmentType { get; }

    public abstract void Reschedule(DateTime newStartTime, TimeSpan newDuration);

    public virtual DateTime EndTime => this.StartTime.Add(this.Duration);

    public override bool Equals(object obj)
    {
      Appointment appointment = obj as Appointment;
      return obj != null && this.apptId == appointment.apptId;
    }

    public override int GetHashCode() => this.apptId.GetHashCode();
  }
}
