// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactEvents
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using System;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("A9A1BE92-26DD-4180-89D6-C2750F415367")]
  public interface IContactEvents
  {
    ContactEvent this[int index] { get; }

    int Count { get; }

    ContactEvent Add(string eventType);

    void Remove(ContactEvent evnt);

    IEnumerator GetEnumerator();

    void Refresh();

    ContactEvent AddForDate(string eventType, DateTime eventDate);
  }
}
