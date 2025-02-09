// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.AppointmentList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.BusinessObjects.Calendar;
using System;
using System.Collections;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  public class AppointmentList : ListBase, IAppointmentList
  {
    public AppointmentList()
      : base(typeof (Appointment))
    {
    }

    public AppointmentList(IList source)
      : base(typeof (Appointment), source)
    {
    }

    public Appointment this[int index]
    {
      get => (Appointment) this.List[index];
      set => this.List[index] = (object) value;
    }

    public void Add(Appointment value) => this.List.Add((object) value);

    public bool Contains(Appointment value) => this.List.Contains((object) value);

    public int IndexOf(Appointment value) => this.List.IndexOf((object) value);

    public void Insert(int index, Appointment value) => this.List.Insert(index, (object) value);

    public void Remove(Appointment value) => this.List.Remove((object) value);

    public Appointment[] ToArray()
    {
      Appointment[] array = new Appointment[this.List.Count];
      this.List.CopyTo((Array) array, 0);
      return array;
    }
  }
}
