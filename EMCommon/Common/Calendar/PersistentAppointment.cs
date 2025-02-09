// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.PersistentAppointment
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  [Serializable]
  public abstract class PersistentAppointment : Appointment
  {
    private DateTime startTime;
    private TimeSpan duration;
    private string subject;

    public PersistentAppointment(DateTime startTime, TimeSpan duration, string subject)
      : base(Guid.NewGuid().ToString())
    {
      this.startTime = startTime;
      this.duration = duration;
      this.subject = subject;
    }

    public override DateTime StartTime => this.startTime;

    public override TimeSpan Duration => this.duration;

    public override string Subject
    {
      get => this.subject;
      set => this.subject = value;
    }

    public override void Reschedule(DateTime newStartTime, TimeSpan newDuration)
    {
      this.startTime = newStartTime;
      this.duration = newDuration;
    }
  }
}
