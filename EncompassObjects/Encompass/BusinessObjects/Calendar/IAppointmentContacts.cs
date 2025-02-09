// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Calendar.IAppointmentContacts
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll
// XML documentation location: C:\SmartClientCache\Apps\UAC\Ellie Mae\xIHR5EqGa7zPnRG0YpD5z4TPAB0=\Encompass360\EncompassObjects.xml

using EllieMae.Encompass.BusinessObjects.Contacts;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Calendar
{
  /// <summary>Interface for AppointmentContacts class.</summary>
  /// <exclude />
  [Guid("B9CD7DC1-D7A0-4990-AA8E-D5E1E2D22D31")]
  public interface IAppointmentContacts
  {
    Contact this[int index] { get; }

    int Count { get; }

    void Add(Contact contact);

    void Remove(Contact contact);

    void Clear();

    IEnumerator GetEnumerator();
  }
}
