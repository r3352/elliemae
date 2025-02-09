// Decompiled with JetBrains decompiler
// Type: EllieMae.Encompass.BusinessObjects.Contacts.IContactCursor
// Assembly: EncompassObjects, Version=1.5.1.0, Culture=neutral, PublicKeyToken=d11ef57bba4acf91
// MVID: BFD5C65C-A9EC-4558-A5A0-AEF78CA2996D
// Assembly location: C:\SmartClientCache\Apps\Ellie Mae\Encompass\SDK\EncompassObjects.dll

using EllieMae.Encompass.Collections;
using System.Collections;
using System.Runtime.InteropServices;

#nullable disable
namespace EllieMae.Encompass.BusinessObjects.Contacts
{
  [Guid("2C255B7B-0DAB-4c3c-91C6-288A7AD417B9")]
  public interface IContactCursor
  {
    int Count { get; }

    void Close();

    IEnumerator GetEnumerator();

    Contact GetItem(int index);

    ContactList GetItems(int startIndex, int count);
  }
}
