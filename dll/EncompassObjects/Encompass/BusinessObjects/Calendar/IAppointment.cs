// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.IAppointment
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Drawing;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  [Guid("C07D2952-C951-4da0-8322-793A36E5EC54")]
  public interface IAppointment
  {
    int ID { get; }

    string Subject { get; set; }

    DateTime StartTime { get; set; }

    DateTime EndTime { get; set; }

    string Location { get; set; }

    Color BarColor { get; set; }

    bool AllDayEvent { get; set; }

    bool ReminderEnabled { get; set; }

    int ReminderInterval { get; set; }

    bool ReminderActive { get; }

    AppointmentContacts Contacts { get; }

    bool IsNew();

    void Commit();

    void Delete();

    void Refresh();

    string Comments { get; set; }
  }
}
