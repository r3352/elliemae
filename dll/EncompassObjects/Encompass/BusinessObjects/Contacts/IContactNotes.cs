// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactNotes
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("C6FFFEF2-A6AE-469a-A317-84E5455B8A23")]
  public interface IContactNotes
  {
    ContactNote this[int index] { get; }

    int Count { get; }

    ContactNote Add(string subject, string details);

    void Remove(ContactNote note);

    IEnumerator GetEnumerator();

    void Refresh();
  }
}
