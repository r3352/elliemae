// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.Collections.IAppointmentList
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Calendar;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.Collections
{
  /// <summary>Interface for AppointmentList class.</summary>
  /// <exclude />
  [Guid("0BAC5B37-0AED-475f-B91B-6A9CC4E30BD0")]
  public interface IAppointmentList
  {
    Appointment this[int index] { get; set; }

    int Count { get; }

    void Clear();

    void Add(Appointment value);

    bool Contains(Appointment value);

    int IndexOf(Appointment value);

    void Insert(int index, Appointment value);

    void Remove(Appointment value);

    Appointment[] ToArray();

    IEnumerator GetEnumerator();
  }
}
