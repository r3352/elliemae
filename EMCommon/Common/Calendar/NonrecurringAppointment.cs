// Decompiled with JetBrains decompiler
// Type: EllieMae.EMLite.Common.Calendar.NonrecurringAppointment
// Assembly: EMCommon, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: 6DB77CFB-E43D-49C6-9F8D-D9791147D23A
// Assembly location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EMCommon.dll

using System;

#nullable disable
namespace EllieMae.EMLite.Common.Calendar
{
  public class NonrecurringAppointment : PersistentAppointment
  {
    private string sourceRecurrenceId;
    private DateTime originalRecurrenceDate = DateTime.MinValue;

    public NonrecurringAppointment(DateTime startTime, TimeSpan duration, string subject)
      : base(startTime, duration, subject)
    {
    }

    internal NonrecurringAppointment(RecurringAppointment recurrence, DateTime occurrenceTime)
      : base(occurrenceTime, recurrence.Duration, recurrence.Subject)
    {
      this.sourceRecurrenceId = recurrence.AppointmentID;
      this.originalRecurrenceDate = occurrenceTime;
    }

    public override AppointmentType AppointmentType => AppointmentType.Nonrecurring;

    public bool IsRecurrenceException() => this.sourceRecurrenceId != null;

    public string SourceRecurrenceID => this.sourceRecurrenceId;

    public DateTime OriginalRecurrenceDate => this.originalRecurrenceDate;
  }
}
